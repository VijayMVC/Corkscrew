using Corkscrew.SDK.exceptions;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.workflow;
using System;
using System.Activities;
using System.Activities.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Workflow.Runtime;

namespace CorkscrewWorkflowService
{

    /// <summary>
    /// Corkscrew Workflow management executive. The only thing required to be done to run workflows is to instantiate this class. 
    /// When the host for this class is shutting down, call the Dispose() method of this class.
    /// The executive always runs as [Corkscrew System] user.
    /// </summary>
    public class CSWorkflowManager : IDisposable
    {
        // This warning must be disabled to enable usage of WF3 objects below
#pragma warning disable 0618

        private Timer _workflowPumpTimer = null;                                                            // timer that polls and pushes along our workflows
        private readonly int TIMER_DELAY = (1 * 60 * 1000);        // (15 * 60 * 1000)                      // timer fire interval (production value: 15 min)

        // all running instances
        private Dictionary<Guid, CSWorkflowInstance> _runningInstances = null;
        private Dictionary<Guid, WorkflowEngineEnum> _runningInstanceEngineMap = null;                      // map of CSWorkflowInstance and the Engine

        // locators for v3, v4 and CS workflows
        private Dictionary<Guid, System.Workflow.Runtime.WorkflowInstance> _runningV3Workflows = null;
        private Dictionary<Guid, WorkflowApplication> _runningV4Workflows = null;
        private Dictionary<Guid, ICSWorkflow> _runningCSWorkflows = null;

        // WF 3.0 is the only thing that requires creation of an appdomain-like "Runtime"
        private WorkflowRuntime _v3WorkflowRuntime = null;

        private CSFarm _workflowFarm = null;

        /// <summary>
        /// Constructor. 
        /// Initializes the workflow runtime infrastructure and executes the workflows. Call Dispose() to shutdown.
        /// </summary>
        public CSWorkflowManager()
        {
            _runningInstances = new Dictionary<Guid, CSWorkflowInstance>();
            _runningInstanceEngineMap = new Dictionary<Guid, WorkflowEngineEnum>();
            _runningCSWorkflows = new Dictionary<Guid, ICSWorkflow>();
            _runningV3Workflows = new Dictionary<Guid, System.Workflow.Runtime.WorkflowInstance>();
            _runningV4Workflows = new Dictionary<Guid, WorkflowApplication>();

            // hard-coded open with sys user since workflow system runs under this account.
            _workflowFarm = CSFarm.Open(CSUser.CreateSystemUser());

            _workflowPumpTimer = new Timer(WorkflowPumpTimerCallback, null, TIMER_DELAY, TIMER_DELAY);

            // dont instantiate or subscribe V3 runtime here. That will be done the first time a V3 workflow is loaded (timer event)
        }

        // the main event pump. Polls the backend every TIMER_DELAY moments, picks up available runnables and tries to run them
        private void WorkflowPumpTimerCallback(object state)
        {
            // disable timer
            _workflowPumpTimer.Change(Timeout.Infinite, Timeout.Infinite);

            if (disposedValue)
            {
                // if we are already disposed, exit. No need to touch anything here!
                return;
            }

            CSFarm farm = CSFarm.Open(CSUser.CreateSystemUser());

            // get all instances running in the farm
            List<CSWorkflowInstance> instances = CSWorkflowInstanceCollection.GetAllRunnable(_workflowFarm).ToList();

            // for each site in the farm, get instances in that site
            foreach (CSSite site in farm.AllSites)
            {
                // skip for sites hosted in farm database
                if (site.ContentDatabaseName != CSFarm.FARM_DATABASENAME)
                {
                    // skip for sites we may have already seen
                    if (instances.Where(i => ((i.Site != null) && (i.Site.Id != site.Id))).Count() == 0)
                    {
                        instances.AddRange(CSWorkflowInstanceCollection.GetAllRunnable(site).ToList());
                    }
                }
            }

            // remove noted instances if they are no longer in the running list
            // though we do these removals from event subscriptions, the below loop takes care of exited workflows from 
            // backend-exit scenarios (like Def deletion, Site/FS modification, etc)

            IReadOnlyList<Guid> keys = _runningInstanceEngineMap.Keys.ToList().AsReadOnly();
            foreach (Guid id in keys)
            {
                if (instances.Select(i => i.Id.Equals(id)).Count() == 0)
                {
                    UnregisterWorkflow(id);
                }
            }

            foreach (CSWorkflowInstance csInstance in instances)
            {
                if (disposedValue)
                {
                    // may have been disposed while we were running an instance
                    return;
                }

                try
                {
                    if (_runningInstanceEngineMap.ContainsKey(csInstance.Id))
                    {
                        // (1) Executive is holding this instance
                        // so try to resume if necessary
                        TryResumeWorkflow(csInstance);
                    }
                    else
                    {
                        switch (csInstance.CurrentState)
                        {
                            case CSWorkflowEventTypesEnum.Undefined:
                                // (1) Executive has not seen this instance
                                // (2) State is "Not started"
                                // so instantiate the workflow
                                InstantiateWorkflow(csInstance);
                                break;

                            case CSWorkflowEventTypesEnum.Paused:
                                // (1) Executive has not seen this instance 
                                // (2) State is in a runnable, and beyond "Not started" (meaning it is in-progress)
                                // seems to be good, rehydrate and resume
                                RehydrateWorkflow(csInstance);
                                break;

                            default:
                                // (1) Executive has not seen this instance 
                                // (2) State is in a runnable, and beyond "Not started" (meaning it is in-progress)
                                // Executive possibly crashed. We have no idea if the workflow was coded well... 
                                csInstance.MarkErrored("Executive was not shutdown cleanly. Workflow instance aborted.");
                                break;
                        }
                    }
                }
                catch (CSWorkflowException wfEx)
                {
                    csInstance.WriteTrace(wfEx.Message);

                    if ((csInstance.CurrentState != CSWorkflowEventTypesEnum.Completed) || (csInstance.CurrentState != CSWorkflowEventTypesEnum.Errored))
                    {
                        csInstance.MarkErrored(wfEx);
                    }
                }
                finally
                {
                    if (!csInstance.CanChangeState)
                    {
                        UnregisterWorkflow(csInstance.Id);
                    }
                }
            }


            if (disposedValue)
            {
                // may have been disposed while we were running an instance
                return;
            }

            // enable timer
            _workflowPumpTimer.Change(TIMER_DELAY, TIMER_DELAY);
        }

        // attempt to get the Assembly to execute the CSWorkflowInstance
        // returns NULL if not found.
        private Assembly GetAssembly(CSWorkflowInstance csInstance)
        {
            // Do NOT load the assembly into a different AppDomain. Doing that is not useful to us.
            // we refer to the objects/events in the loaded assembly/workflow. The moment we do that, if 
            // the assembly is in a different Domain, it will get loaded into our AppDomain. That sucks!

            CSWorkflowManifest manifest = csInstance.Association.WorkflowDefinition.GetManifest();
            if (manifest != null)
            {
                // check if we are using an OOBE workflow
                string oobeAssemblyFilename = Path.GetFileName(typeof(CSWorkflowInstance).Assembly.Location);
                if ((manifest.OutputAssemblyName == oobeAssemblyFilename) || (manifest.OutputAssemblyName == Path.GetFileNameWithoutExtension(oobeAssemblyFilename)))
                {
                    Type IIworkflow = typeof(ICSWorkflow);
                    IEnumerable<Type> implementedWorkflows = AppDomain.CurrentDomain.GetAssemblies()
                                                            .SelectMany(a => a.GetTypes())
                                                                .Where(a => (
                                                                                IIworkflow.IsAssignableFrom(a)
                                                                                && (!a.IsInterface)
                                                                                && (a.Name.Equals(manifest.WorkflowClassName) || a.FullName.Equals(manifest.WorkflowClassName))
                                                                ));

                    Type wfType = implementedWorkflows.FirstOrDefault();
                    if ((wfType == null) || (wfType == default(Type)))
                    {
                        // configuration says find the workflow in this dll,
                        // its not there, so return NULL
                        return null;
                    }

                    return typeof(CSWorkflowInstance).Assembly;
                }

                CSWorkflowCompiler compiler = new CSWorkflowCompiler(manifest);
                try
                {
                    compiler.Compile(false);
                    if (compiler.CompileSuccessful)
                    {
                        return compiler.CompilerOutputAssembly;
                    }
                }
                catch
                {
                    // null will be returned below
                }
                finally
                {
                    compiler = null;
                }
            }

            return null;
        }

        // Run a WorkflowInstance for the first time (Started event)
        private void InstantiateWorkflow(CSWorkflowInstance csInstance)
        {
            // get cached or freshly compiled assembly
            Assembly workflowAssembly = GetAssembly(csInstance);
            if (workflowAssembly == null)
            {
                csInstance.MarkErrored("Could not retrieve or compile workflow assembly.");
                return;
            }

            CSWorkflowManifest manifest = csInstance.Association.WorkflowDefinition.GetManifest();
            WorkflowEngineEnum workflowEngine = manifest.WorkflowEngine;
            string workflowClassName = manifest.WorkflowClassName;
            manifest = null;

            try
            {
                switch (workflowEngine)
                {
                    case WorkflowEngineEnum.WF3C:
                    case WorkflowEngineEnum.WF3X:

                        EnsureV3Runtime();
                        Type wf3WorkflowClassType = workflowAssembly.GetType(workflowClassName, false, true);
                        if (wf3WorkflowClassType != null)
                        {
                            System.Workflow.Runtime.WorkflowInstance v3Instance = _v3WorkflowRuntime.CreateWorkflow(wf3WorkflowClassType, null, csInstance.Id);
                            if (v3Instance != null)
                            {
                                _runningInstanceEngineMap.Add(csInstance.Id, workflowEngine);
                                _runningV3Workflows.Add(csInstance.Id, v3Instance);

                                csInstance.RunInstanceCorrelationId = csInstance.Id;
                                v3Instance.Start();
                            }
                            else
                            {
                                throw new CSWorkflowException("Could not start workflow instance.");
                            }
                        }
                        else
                        {
                            throw new CSWorkflowException("Could not instantiate workflow instance class.");
                        }
                        break;

                    case WorkflowEngineEnum.WF4C:
                    case WorkflowEngineEnum.WF4X:
                        System.Activities.Activity v4Activity = (System.Activities.Activity)workflowAssembly.CreateInstance(workflowClassName, true);
                        if (v4Activity != null)
                        {
                            WorkflowApplication v4Instance = new WorkflowApplication(v4Activity);
                            if (v4Instance != null)
                            {
                                EnsureV4Events(v4Instance);

                                _runningInstanceEngineMap.Add(csInstance.Id, workflowEngine);
                                _runningV4Workflows.Add(csInstance.Id, v4Instance);

                                csInstance.RunInstanceCorrelationId = v4Instance.Id;
                                v4Instance.Run();
                            }
                            else
                            {
                                throw new CSWorkflowException("Could not start workflow instance.");
                            }
                        }
                        else
                        {
                            throw new CSWorkflowException("Could not instantiate workflow instance activity.");
                        }
                        break;

                    case WorkflowEngineEnum.CS1C:
                        ICSWorkflow csWorkflow = null;

                        Type csWorkflowClassType = workflowAssembly.GetType(workflowClassName);
                        if (csWorkflowClassType == null)
                        {
                            throw new CSWorkflowException("Could not find the workflow class type [" + workflowClassName + "] in assembly + [" + workflowAssembly.Location + "].");
                        }

                        // the Workflow class may have either a CTOR(CSWorkflowInstance) or a CTOR(). Find out which one we have to use.
                        ConstructorInfo constructor = csWorkflowClassType.GetConstructor(new Type[] { typeof(CSWorkflowInstance) });
                        bool isDefaultConstructor = false;

                        if (constructor == null)
                        {
                            constructor = workflowAssembly.GetType(workflowClassName).GetConstructor(Type.EmptyTypes);
                            isDefaultConstructor = true;

                            if (constructor == null)
                            {
                                throw new Exception("Workflow class has no constructor that accepts a CSWorkflowInstance, or a default public constructor.");
                            }
                        }

                        try
                        {
                            if (isDefaultConstructor)
                            {
                                csWorkflow = (ICSWorkflow)Activator.CreateInstance(workflowAssembly.GetType(workflowClassName));
                            }
                            else
                            {
                                csWorkflow = (ICSWorkflow)Activator.CreateInstance(workflowAssembly.GetType(workflowClassName), csInstance);
                            }
                        }
                        catch (Exception cs1ConstructorException)
                        {
                            throw new CSWorkflowException("Could not instantiate workflow instance.", cs1ConstructorException);
                        }

                        if (csWorkflow != null)
                        {
                            _runningInstanceEngineMap.Add(csInstance.Id, WorkflowEngineEnum.CS1C);
                            _runningCSWorkflows.Add(csInstance.Id, csWorkflow);

                            csInstance.RunInstanceCorrelationId = csInstance.Id;

                            // the started event is sent out of the switch below
                        }
                        break;
                }

                csInstance.IsLoadedInRuntime = true;
                _runningInstances.Add(csInstance.Id, csInstance);

                csInstance.Start();
            }
            catch (Exception error)
            {
                // so that this instance is not tried again
                csInstance.MarkErrored(error);
                csInstance.IsLoadedInRuntime = false;
                csInstance.RunInstanceCorrelationId = Guid.Empty;

                // unregister is done in the main timer loop
            }
            finally
            {
            }
        }

        // Run a WorkflowInstance for after it has been paused (Continued event)
        private void TryResumeWorkflow(CSWorkflowInstance csInstance)
        {
            WorkflowEngineEnum engine = _runningInstanceEngineMap[csInstance.Id];

            try
            {
                switch (engine)
                {
                    case WorkflowEngineEnum.WF3C:
                    case WorkflowEngineEnum.WF3X:
                        if ((csInstance.CurrentState == CSWorkflowEventTypesEnum.Paused) && (_runningV3Workflows[csInstance.Id].GetWorkflowNextTimerExpiration() <= DateTime.Now))
                        {
                            _runningV3Workflows[csInstance.Id].Resume();
                        }
                        break;

                    case WorkflowEngineEnum.WF4C:
                    case WorkflowEngineEnum.WF4X:
                        if (csInstance.CurrentState == CSWorkflowEventTypesEnum.Paused)
                        {
                            IReadOnlyCollection<BookmarkInfo> bookmarks = _runningV4Workflows[csInstance.Id].GetBookmarks();
                            if (bookmarks != null)
                            {
                                foreach (BookmarkInfo bookmark in bookmarks)
                                {
                                    // we dont actually care what happened to the resumption request
                                    _runningV4Workflows[csInstance.Id].ResumeBookmark(bookmark.BookmarkName, null);
                                }
                            }

                            bookmarks = _runningV4Workflows[csInstance.Id].GetBookmarks();
                            if ((bookmarks == null) || (bookmarks.Count == 0))
                            {
                                // no more bookmarks, mark completed
                                csInstance.Complete(CSWorkflowEventCompletionTypesEnum.Successful);
                            }
                        }
                        break;

                    case WorkflowEngineEnum.CS1C:
                        // try to continue instance if it is paused.
                        // idea is the Continued event handler will check if it is ready to continue and will then enter one of three states: 
                        // Paused, Completed or Errored.
                        if (csInstance.CurrentState == CSWorkflowEventTypesEnum.Paused)
                        {
                            csInstance.Continue();
                        }
                        break;
                }
            }
            catch (Exception error)
            {
                // so that this instance is not tried again
                csInstance.MarkErrored(error);
                csInstance.IsLoadedInRuntime = false;
                csInstance.RunInstanceCorrelationId = Guid.Empty;

                // unregister is done in the main timer loop
            }
            finally
            {

            }

        }

        // Load a paused workflow from backend and bring it up
        private void RehydrateWorkflow(CSWorkflowInstance csInstance)
        {
            // get cached or freshly compiled assembly
            Assembly workflowAssembly = GetAssembly(csInstance);
            if (workflowAssembly == null)
            {
                csInstance.MarkErrored("Could not retrieve or compile workflow assembly.");
                return;
            }

            CSWorkflowManifest manifest = csInstance.Association.WorkflowDefinition.GetManifest();
            WorkflowEngineEnum workflowEngine = manifest.WorkflowEngine;
            string workflowClassName = manifest.WorkflowClassName;
            manifest = null;

            try
            {
                switch (workflowEngine)
                {
                    case WorkflowEngineEnum.WF3C:
                    case WorkflowEngineEnum.WF3X:

                        EnsureV3Runtime();

                        Type workflowClassType = workflowAssembly.GetType(workflowClassName, false, true);
                        if (workflowClassType == null)
                        {
                            throw new CSWorkflowException("Could not instantiate workflow instance class.");
                        }

                        System.Workflow.Runtime.WorkflowInstance v3Instance = _v3WorkflowRuntime.GetWorkflow(csInstance.RunInstanceCorrelationId);
                        if (v3Instance == null)
                        {
                            throw new CSWorkflowException("Could not resume workflow instance.");
                        }

                        _runningInstanceEngineMap.Add(csInstance.Id, workflowEngine);
                        _runningV3Workflows.Add(csInstance.Id, v3Instance);

                        v3Instance.Resume();
                        break;

                    case WorkflowEngineEnum.WF4C:
                    case WorkflowEngineEnum.WF4X:
                        System.Activities.Activity v4Activity = (System.Activities.Activity)workflowAssembly.CreateInstance(workflowClassName, true);
                        if (v4Activity == null)
                        {
                            throw new CSWorkflowException("Could not instantiate workflow instance activity.");
                        }

                        WorkflowApplication v4Instance = new WorkflowApplication(v4Activity);
                        if (v4Instance == null)
                        {
                            throw new CSWorkflowException("Could not start workflow instance.");
                        }

                        EnsureV4Events(v4Instance);

                        _runningInstanceEngineMap.Add(csInstance.Id, workflowEngine);
                        _runningV4Workflows.Add(csInstance.Id, v4Instance);

                        v4Instance.Load(csInstance.RunInstanceCorrelationId);
                        v4Instance.Run();
                        break;

                    case WorkflowEngineEnum.CS1C:
                        ICSWorkflow csWorkflow = null;

                        Type csWorkflowClassType = workflowAssembly.GetType(workflowClassName);
                        if (csWorkflowClassType == null)
                        {
                            throw new CSWorkflowException("Could not find the workflow class type [" + workflowClassName + "] in assembly + [" + workflowAssembly.Location + "].");
                        }

                        // the Workflow class may have either a CTOR(CSWorkflowInstance) or a CTOR(). Find out which one we have to use.
                        ConstructorInfo constructor = csWorkflowClassType.GetConstructor(new Type[] { typeof(CSWorkflowInstance) });
                        bool isDefaultConstructor = false;

                        if (constructor == null)
                        {
                            constructor = workflowAssembly.GetType(workflowClassName).GetConstructor(Type.EmptyTypes);
                            isDefaultConstructor = true;

                            if (constructor == null)
                            {
                                throw new Exception("Workflow class has no constructor that accepts a CSWorkflowInstance, or a default public constructor.");
                            }
                        }

                        try
                        {
                            if (isDefaultConstructor)
                            {
                                csWorkflow = (ICSWorkflow)Activator.CreateInstance(workflowAssembly.GetType(workflowClassName));
                            }
                            else
                            {
                                csWorkflow = (ICSWorkflow)Activator.CreateInstance(workflowAssembly.GetType(workflowClassName), csInstance);
                            }
                        }
                        catch (Exception cs1ConstructorException)
                        {
                            throw new CSWorkflowException("Could not instantiate workflow instance.", cs1ConstructorException);
                        }

                        if (csWorkflow != null)
                        {
                            _runningInstanceEngineMap.Add(csInstance.Id, WorkflowEngineEnum.CS1C);
                            _runningCSWorkflows.Add(csInstance.Id, csWorkflow);

                            csInstance.RunInstanceCorrelationId = csInstance.Id;

                            // the continue event is sent out of the switch below
                        }
                        break;
                }

                csInstance.IsLoadedInRuntime = true;
                _runningInstances.Add(csInstance.Id, csInstance);

                csInstance.Continue();
            }
            catch (Exception error)
            {
                // so that this instance is not tried again
                csInstance.MarkErrored(error);
                csInstance.IsLoadedInRuntime = false;

                // unregister is done in the main timer loop
            }
            finally
            {
            }
        }

        // remove the workflow instance from our tracking dictionaries
        private void UnregisterWorkflow(Guid id)
        {

            WorkflowEngineEnum engine = WorkflowEngineEnum.Undefined;
            if (_runningInstanceEngineMap.ContainsKey(id))
            {
                engine = _runningInstanceEngineMap[id];
                _runningInstanceEngineMap.Remove(id);
            }

            if (_runningInstances.ContainsKey(id)) _runningInstances.Remove(id);

            switch (engine)
            {
                case WorkflowEngineEnum.WF3C:
                case WorkflowEngineEnum.WF3X:
                    if (_runningV3Workflows.ContainsKey(id)) _runningV3Workflows.Remove(id);
                    break;

                case WorkflowEngineEnum.WF4C:
                case WorkflowEngineEnum.WF4X:
                    if (_runningV4Workflows.ContainsKey(id)) _runningV4Workflows.Remove(id);
                    break;

                case WorkflowEngineEnum.CS1C:
                    if (_runningCSWorkflows.ContainsKey(id)) _runningCSWorkflows.Remove(id);
                    break;
            }
        }

        // WF4 does not let us set our own Guid. This method returns the CSWorkflowInstance.Id 
        // given the Guid of the WF4 workflow instance
        //
        // input Guid is: WF4 workflow instance.Id
        // output Guid is: CSWorkflowInstance.Id
        private Guid GetV4InstanceId(Guid id)
        {
            foreach (CSWorkflowInstance instance in _runningInstances.Values)
            {
                if (instance.RunInstanceCorrelationId.Equals(id))
                {
                    return instance.Id;
                }
            }

            return Guid.Empty;
        }

        // Ensures the WF3 WorkflowRuntime is created and events are subscribed to
        private void EnsureV3Runtime()
        {
            if (_v3WorkflowRuntime == null)
            {
                _v3WorkflowRuntime = new WorkflowRuntime();
                _v3WorkflowRuntime.ServicesExceptionNotHandled += WorkflowRuntime_ServiceExceptionNotHandled;
                _v3WorkflowRuntime.WorkflowCompleted += WorkflowRuntime_WorkflowCompleted;
                _v3WorkflowRuntime.WorkflowSuspended += WorkflowRuntime_WorkflowSuspended;
                _v3WorkflowRuntime.WorkflowTerminated += WorkflowRuntime_WorkflowTerminated;
                _v3WorkflowRuntime.WorkflowUnloaded += WorkflowRuntime_WorkflowUnloaded;
                _v3WorkflowRuntime.WorkflowAborted += WorkflowRuntime_WorkflowAborted;
                _v3WorkflowRuntime.WorkflowIdled += WorkflowRuntime_WorkflowIdled;
                _v3WorkflowRuntime.WorkflowResumed += WorkflowRuntime_WorkflowResumed;

                _v3WorkflowRuntime.StartRuntime();
            }
        }

        // Add event subscriptions to the WF4 WorkflowApplication
        private void EnsureV4Events(WorkflowApplication wfApp)
        {
            if (wfApp != null)
            {
                wfApp.Aborted =
                    (e) =>
                    {
                        Guid csId = GetV4InstanceId(e.InstanceId);
                        if (!csId.Equals(Guid.Empty))
                        {
                            _runningInstances[csId].Complete(CSWorkflowEventCompletionTypesEnum.Aborted);
                        }
                    };

                wfApp.Completed =
                    (e) =>
                    {
                        Guid csId = GetV4InstanceId(e.InstanceId);
                        if (!csId.Equals(Guid.Empty))
                        {
                            _runningInstances[csId].Complete(CSWorkflowEventCompletionTypesEnum.Successful);
                        }
                    };

                wfApp.PersistableIdle =
                    (e) =>
                    {
                        Guid csId = GetV4InstanceId(e.InstanceId);
                        if (!csId.Equals(Guid.Empty))
                        {
                            _runningInstances[csId].Pause("Persisted and unloaded");
                        }

                        return PersistableIdleAction.Persist | PersistableIdleAction.Unload;
                    };

                wfApp.Unloaded =
                    (e) =>
                    {
                        Guid csId = GetV4InstanceId(e.InstanceId);
                        if (!csId.Equals(Guid.Empty))
                        {
                            UnregisterWorkflow(csId);
                        }
                    };

                wfApp.OnUnhandledException =
                    (e) =>
                    {
                        Guid csId = GetV4InstanceId(e.InstanceId);
                        if (!csId.Equals(Guid.Empty))
                        {
                            _runningInstances[csId].MarkErrored(e.UnhandledException);
                        }

                        return UnhandledExceptionAction.Abort;
                    };
            }
        }

        #region V3 WorkflowRuntime Event handlers

        private void WorkflowRuntime_ServiceExceptionNotHandled(object sender, ServicesExceptionNotHandledEventArgs e)
        {
            Guid instanceId = e.WorkflowInstanceId;
            _runningInstances[instanceId].MarkErrored(e.Exception);
        }

        private void WorkflowRuntime_WorkflowCompleted(object sender, WorkflowCompletedEventArgs e)
        {
            Guid instanceId = e.WorkflowInstance.InstanceId;
            _runningInstances[instanceId].Complete(CSWorkflowEventCompletionTypesEnum.Successful);
        }

        private void WorkflowRuntime_WorkflowSuspended(object sender, WorkflowSuspendedEventArgs e)
        {
            Guid instanceId = e.WorkflowInstance.InstanceId;
            e.WorkflowInstance.Unload();

            _runningInstances[instanceId].Pause(e.Error);
        }

        private void WorkflowRuntime_WorkflowTerminated(object sender, WorkflowTerminatedEventArgs e)
        {
            Guid instanceId = e.WorkflowInstance.InstanceId;
            e.WorkflowInstance.Unload();

            _runningInstances[instanceId].Complete(CSWorkflowEventCompletionTypesEnum.TerminatedByUser);
        }

        private void WorkflowRuntime_WorkflowUnloaded(object sender, WorkflowEventArgs e)
        {
            if (_runningInstanceEngineMap.ContainsKey(e.WorkflowInstance.InstanceId))
            {
                // unloaded by the runtime
                Guid instanceId = e.WorkflowInstance.InstanceId;
                _runningInstances[instanceId].Pause("Instance unloaded by runtime");
            }
        }

        private void WorkflowRuntime_WorkflowAborted(object sender, WorkflowEventArgs e)
        {
            Guid instanceId = e.WorkflowInstance.InstanceId;
            e.WorkflowInstance.Unload();

            _runningInstances[instanceId].Complete(CSWorkflowEventCompletionTypesEnum.Aborted);
        }

        private void WorkflowRuntime_WorkflowIdled(object sender, WorkflowEventArgs e)
        {
            // unload event handler will take care of the rest
            e.WorkflowInstance.Unload();
        }

        private void WorkflowRuntime_WorkflowResumed(object sender, WorkflowEventArgs e)
        {
            Guid instanceId = e.WorkflowInstance.InstanceId;
            _runningInstances[instanceId].Continue();
        }

        #endregion

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes the workflow manager
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                disposedValue = true;

                // get rid of the timer first
                _workflowPumpTimer.Dispose();
                _workflowPumpTimer = null;

                if (disposing)
                {
                    if ((_v3WorkflowRuntime != null) && (_v3WorkflowRuntime.IsStarted))
                    {
                        foreach (System.Workflow.Runtime.WorkflowInstance instance in _v3WorkflowRuntime.GetLoadedWorkflows())
                        {
                            try
                            {
                                instance.Suspend("Workflow executive is being shutdown. Will be resumed later.");
                                // suspend event handler will unload
                            }
                            catch
                            {
                            }
                        }

                        _v3WorkflowRuntime.StopRuntime();
                        _v3WorkflowRuntime.Dispose();
                    }

                    foreach (WorkflowApplication instance in _runningV4Workflows.Values)
                    {
                        try
                        {
                            instance.Unload();  // persist and unload
                        }
                        catch
                        {
                        }
                    }

                    foreach (ICSWorkflow instance in _runningCSWorkflows.Values)
                    {
                        try
                        {
                            instance.Context.Instance.Pause(null);
                        }
                        catch
                        {
                        }
                    }

                    _runningCSWorkflows = null;
                    _runningV3Workflows = null;
                    _runningV4Workflows = null;
                    _runningInstanceEngineMap = null;
                    _runningInstances = null;
                }
            }
        }

        /// <summary>
        /// Shutdown the WorkflowManager executive.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


#pragma warning restore 0618
        // This warning must be disabled to enable usage of WF3 objects above
    }
}
