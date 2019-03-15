using Corkscrew.SDK.workflow;
using System;
using System.Runtime.Serialization;
using System.Security.Permissions;


namespace Corkscrew.SDK.exceptions
{

    /// <summary>
    /// This exception is thrown by the Corkscrew Workflow system.
    /// </summary>
    [Serializable]
    public sealed class CSWorkflowException : Exception
    {

        #region Properties

        /// <summary>
        /// The context we were running in when the exception fired
        /// </summary>
        public CSWorkflowRuntimeContext Context
        {
            get;
            private set;
        } = null;

        /// <summary>
        /// The workflow instance (if we have one)
        /// </summary>
        public CSWorkflowInstance WorkflowInstance
        {
            get
            {
                if ((Context != null) && (_instance == null))
                {
                    return Context.Instance;
                }

                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        private CSWorkflowInstance _instance = null;

        /// <summary>
        /// The workflow association (if we have one)
        /// </summary>
        public CSWorkflowAssociation WorkflowAssociation
        {
            get
            {
                if ((_association == null) && (WorkflowInstance != null))
                {
                    return WorkflowInstance.Association;
                }

                return _association;
            }
            private set
            {
                _association = value;
            }
        }
        private CSWorkflowAssociation _association = null;

        /// <summary>
        /// The workflow definition (if we have one)
        /// </summary>
        public CSWorkflowDefinition WorkflowDefinition
        {
            get
            {
                if ((_definition == null) && (WorkflowAssociation != null))
                {
                    return WorkflowAssociation.WorkflowDefinition;
                }

                return _definition;
            }
            private set
            {
                _definition = value;
            }
        }
        private CSWorkflowDefinition _definition = null;

        #endregion

        /// <summary>
        /// Default Constructor, everthing set to NULL.
        /// </summary>
        public CSWorkflowException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public CSWorkflowException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception to attach</param>
        public CSWorkflowException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="instance">Workflow instance that caused the exception</param>
        /// <param name="innerException">Inner exception to attach</param>
        public CSWorkflowException(string message, CSWorkflowInstance instance, Exception innerException = null) 
            : base(message, innerException)
        {
            WorkflowInstance = instance;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="association">Workflow association that caused the exception</param>
        /// <param name="innerException">Inner exception to attach</param>
        public CSWorkflowException(string message, CSWorkflowAssociation association, Exception innerException = null) 
            : base(message, innerException)
        {
            WorkflowAssociation = association;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="definition">Workflow definition that caused the exception</param>
        /// <param name="innerException">Inner exception to attach</param>
        public CSWorkflowException(string message, CSWorkflowDefinition definition, Exception innerException = null)
            : base(message, innerException)
        {
            WorkflowDefinition = definition;
        }

        /// <summary>
        /// Constructor (used in serialization context)
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public CSWorkflowException(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Get object data (serialization)
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
