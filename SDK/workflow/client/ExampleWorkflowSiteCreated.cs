using Corkscrew.SDK.objects;

namespace Corkscrew.SDK.workflow
{
    /// <summary>
    /// This is an example workflow that should be set to fire when a new site is created
    /// </summary>
    public class ExampleWorkflowSiteCreated : CSWorkflow
    {

        private CSSite _site = null;    // the site that was created

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="instance">Workflow instance that was started</param>
        public ExampleWorkflowSiteCreated(CSWorkflowInstance instance)
            : base(instance)
        {
            _site = instance.Site;
        }

        /// <summary>
        /// The Workflow started event
        /// </summary>
        /// <param name="sender">Instance that started</param>
        /// <param name="e">Workflow event arguments</param>
        protected override void OnStarted(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            base.OnStarted(sender, e);
            base.Context.Instance.WriteTrace("ExampleWorkflowSiteCreated: Started");

            if (_site != null)
            {
                base.Context.Instance.WriteTrace("ExampleWorkflowSiteCreated: New site with Id {0} created.", _site.Id);

                using (CSFileSystemEntryFile readMe = _site.RootFolder.CreateFile("ReadMe", "txt"))
                {
                    if (readMe != null)
                    {
                        if (readMe.Open(System.IO.FileAccess.Write))
                        {
                            byte[] text = System.Text.Encoding.UTF8.GetBytes("Welcome to Corkscrew. This is your new site.");
                            readMe.Write(text, 0, text.Length);
                            readMe.Close();
                        }
                    }
                }
            }
            else
            {
                base.Context.Instance.WriteTrace("ExampleWorkflowSiteCreated: Instance has Site set to NULL.");
            }

            // nothing more for this workflow to do :-)
            Complete(CSWorkflowEventCompletionTypesEnum.Successful);
        }

        /// <summary>
        /// The Workflow errored event handler
        /// </summary>
        /// <param name="sender">Instance that errored</param>
        /// <param name="e">Workflow event arguments</param>
        protected override void OnError(CSWorkflowInstance sender, CSWorkflowEventArgs e)
        {
            base.OnError(sender, e);
            base.Context.Instance.WriteTrace("ExampleWorkflowSiteCreated: Errored with message: ", e.Context.Instance.ErrorMessage);
        }

    }
}
