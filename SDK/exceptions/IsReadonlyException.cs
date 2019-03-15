using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Corkscrew.SDK.exceptions
{

    /// <summary>
    /// This exception is thrown when an object or collection is set as readonly and a modification 
    /// operation is attempted on it.
    /// </summary>
    [Serializable]
    public class IsReadonlyException : Exception
    {

        /// <summary>
        /// Default Constructor, everthing set to NULL.
        /// </summary>
        public IsReadonlyException() : base() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public IsReadonlyException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception to attach</param>
        public IsReadonlyException(string message, Exception innerException) : base(message, innerException) { }

        /// <summary>
        /// Constructor (used in serialization context)
        /// </summary>
        /// <param name="info">Serialization info</param>
        /// <param name="context">Streaming context</param>
        public IsReadonlyException(SerializationInfo info, StreamingContext context) : base(info, context) { }

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
