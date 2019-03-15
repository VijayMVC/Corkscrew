using Corkscrew.SDK.security;
using System;

namespace Corkscrew.SDK.workflow
{

    /// <summary>
    /// This class contains properties passed into a workflow when it is "run". This includes 
    /// both initialization and continuation runs. 
    /// This is a runtime class, and not persisted anywhere.
    /// </summary>
    public class CSWorkflowRuntimeContext
    {

        #region Properties

        /// <summary>
        /// The specific association of this trigger
        /// </summary>
        public CSWorkflowAssociation Association
        {
            get
            {
                if (Instance != null)
                {
                    return Instance.Association;
                }

                return null;
            }
        }

        /// <summary>
        /// The workflow definition
        /// </summary>
        public CSWorkflowDefinition Definition
        {
            get
            {
                if (Instance != null)
                {
                    return Instance.Association.WorkflowDefinition;
                }

                return null;
            }
        }

        /// <summary>
        /// This particular instance
        /// </summary>
        public CSWorkflowInstance Instance
        {
            get;
            internal set;
        }

        /// <summary>
        /// The current state of this workflow instance
        /// </summary>
        public CSWorkflowEventTypesEnum State
        {
            get
            {
                if (Instance != null)
                {
                    return Instance.CurrentState;
                }

                return CSWorkflowEventTypesEnum.Undefined;
            }
        }

        /// <summary>
        /// Credential to be used for the association
        /// </summary>
        public CSUser Credential
        {
            get
            {
                return _credentialSystemUser;
            }
        }
        private CSUser _credentialSystemUser = CSUser.CreateSystemUser();

        /// <summary>
        /// Guid of the workflow. Returns the Id of the underlying definition.
        /// </summary>
        public Guid WorkflowId
        {
            get
            {
                if (Association != null)
                {
                    return Association.WorkflowDefinition.Id;
                }

                return Guid.Empty;
            }
        }

        /// <summary>
        /// Guid of the workflow association.
        /// </summary>
        public Guid AssociationId
        {
            get
            {
                if (Association != null)
                {
                    return Association.Id;
                }

                return Guid.Empty;
            }
        }

        /// <summary>
        /// Guid for correlating between all runs of this workflow
        /// </summary>
        public Guid CorrelationId
        {
            get
            {
                if (Instance != null)
                {
                    return Instance.Id;
                }

                return Guid.Empty;
            }
        }

        #endregion

        #region Constructors

        // internal constructor
        internal CSWorkflowRuntimeContext() { }

        /// <summary>
        /// Create an instance of the context
        /// </summary>
        /// <param name="instance">Reference to the Corkscrew workflow instance the properties are being created for</param>
        /// <returns>The created context</returns>
        /// <exception cref="ArgumentNullException">If instance is null</exception>
        public static CSWorkflowRuntimeContext CreateContext(CSWorkflowInstance instance) 
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }

            return new CSWorkflowRuntimeContext()
            {
                Instance = instance
            };
        }
        

        #endregion

    }
}
