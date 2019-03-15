using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// Provides information about the historical record of a Corkscrew Workflow instance's progress
    /// </summary>
    public class CSWorkflowHistory
    {

        #region Properties

        /// <summary>
        /// Guid of the record
        /// </summary>
        public Guid Id
        {
            get;
            internal set;
        } = Guid.Empty;

        /// <summary>
        /// The instance this historical record is for
        /// </summary>
        public CSWorkflowInstance Instance
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Reference to the workflow association
        /// </summary>
        public CSWorkflowAssociation Association
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Association data string
        /// </summary>
        public string AssociationData
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// Associated farm, will always return the current farm.
        /// </summary>
        public CSFarm Farm
        {
            get { return CSFarm.Open(CSUser.CreateSystemUser()); }
        }

        /// <summary>
        /// Associated Site 
        /// (will be NULL only for Farm level workflows)
        /// </summary>
        public CSSite Site
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// The instantiated entity.
        ///  Will never be NULL
        /// </summary>
        public IWorkflowInstantiable InstantiatedEntity
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// The state of this workflow
        /// </summary>
        public CSWorkflowEventTypesEnum State
        {
            get;
            internal set;
        } = CSWorkflowEventTypesEnum.Undefined;

        /// <summary>
        /// If CurrentState is Completed, then this would give the completion reason.
        /// </summary>
        public CSWorkflowEventCompletionTypesEnum CompletedReason
        {
            get;
            internal set;
        } = CSWorkflowEventCompletionTypesEnum.Undefined;

        /// <summary>
        /// The full error message (including stack trace, etc) if the CompletedReason is one of the error states
        /// </summary>
        public string ErrorMessage
        {
            get;
            internal set;
        } = null;

        /// <summary>
        /// The previous workflow event that occured. The latest one is found in the WorkflowInstance record.
        /// </summary>
        public CSWorkflowEventTypesEnum Event
        {
            get;
            internal set;
        } = CSWorkflowEventTypesEnum.Undefined;

        /// <summary>
        /// Date and time of creation. 
        /// Set by constructor.
        /// </summary>
        public DateTime Created { get; internal set; } = DateTime.Now;

        /// <summary>
        /// User who created.
        /// Set by constructor.
        /// </summary>
        public CSUser CreatedBy { get; internal set; } = null;

        #endregion

        #region Constructors

        internal CSWorkflowHistory() { }

        #endregion

    }

    /// <summary>
    /// The chain of historical record for a Corkscrew workflow. 
    /// </summary>
    /// <remarks>This LinkedList will be populated with the latest events at the end of the list. Therefore, CSWorkflowHistoryChain.First will be the earliest event.</remarks>
    [Serializable]
    public class CSWorkflowHistoryChain : LinkedList<CSWorkflowHistory>
    {
        /*
         *      Empty class. All the work is done by the base class (LinkedList<T>)
         */

        /// <summary>
        /// Public constructor, does nothing
        /// </summary>
        public CSWorkflowHistoryChain()
            : base()
        {

        }

        /// <summary>
        /// Constructor for serialization
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        protected CSWorkflowHistoryChain(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }

}
