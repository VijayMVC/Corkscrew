using Corkscrew.SDK.workflow;
using System;
using System.Collections.Generic;
using System.Text;

namespace Corkscrew.SDK.exceptions
{

    /// <summary>
    /// Class to help with Exception throwing and parsing
    /// </summary>
    public static class CSExceptionHelper
    {

        /// <summary>
        /// Throws an ArgumentNullException on the first parameter (array) that evaluates to a NULL.
        /// </summary>
        /// <param name="message">Optional array of messages. If no messages are found, a static message is thrown. If the number of messages is lesser than the number of array objects, 
        /// only the first message is used. Otherwise the message corresponding to the array index is used.</param>
        /// <param name="array">Array of objects to evaluate for NULL</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException by design.</exception>
        public static void ThrowIfNull(string[] message, params object[] array)
        {
            bool isStaticMessage = false;
            string staticMessage = null;

            if ((message == null) || (array == null))
            {
                isStaticMessage = true;
                staticMessage = "One or more input parameters is null";
            }
            else
            {
                isStaticMessage = (message.Length < array.Length);
                staticMessage = message[0];
            }
            

            for(int index = 0; index < array.Length; index++)
            {
                if (IsNullOrEmpty(array[index]))
                {
                    throw new ArgumentNullException((isStaticMessage ? staticMessage : message[index]));
                }
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException on the first parameter (array) that evaluates to a NULL.
        /// </summary>
        /// <param name="message">The message to throw</param>
        /// <param name="array">Array of objects to evaluate for NULL</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException by design.</exception>
        public static void ThrowIfNull(string message, params object[] array)
        {
            for (int index = 0; index < array.Length; index++)
            {
                if (IsNullOrEmpty(array[index]))
                {
                    throw new ArgumentNullException(message);
                }
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException on the first parameter (array) that evaluates to a NULL.
        /// </summary>
        /// <param name="array">Array of objects to evaluate for NULL</param>
        /// <exception cref="ArgumentNullException">Throws an ArgumentNullException by design.</exception>
        public static void ThrowIfNull(params object[] array)
        {
            for (int index = 0; index < array.Length; index++)
            {
                if (IsNullOrEmpty(array[index]))
                {
                    throw new ArgumentNullException("One or more arguments is NULL.");
                }
            }
        }

        /// <summary>
        /// Flattens the messages in an exception hierarchy. Does not provide a "stack trace" unlike Exception.ToString().
        /// </summary>
        /// <param name="exception">Top level exception to flatten</param>
        /// <returns>String with the compressed message.</returns>
        public static string GetExceptionRollup(Exception exception)
        {
            StringBuilder message = new StringBuilder();

            Exception copy = exception;
            int depth = 0;
            while (copy != null)
            {
                message.AppendFormat(
                    "{0} {1} exception: {2}",
                    (">").PadLeft(depth + 1, '-'),
                    copy.GetType().Name,
                    copy.Message
                );
                message.AppendLine();

                depth++;

                copy = copy.InnerException;
            }

            return message.ToString();
        }

        /// <summary>
        /// Checks if the given object is NULL. If the object is a string type, also checks for empty string.
        /// </summary>
        /// <param name="obj">Object to test</param>
        /// <returns>True if object is null or an empty string</returns>
        private static bool IsNullOrEmpty(object obj)
        {
            if (obj == null)
            {
                return true;
            }

            if ((obj is string) && (((string)obj) == string.Empty))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts an enumerable list of ValidationError objects into an Exception hierarchy.
        /// </summary>
        /// <param name="errors">ValidationError enumeration to convert</param>
        /// <returns>An Exception object</returns>
        /// <seealso cref="Exception.ToString"/>
        public static Exception GetExceptionFromValidationErrors(IEnumerable<CSCompilerValidationError> errors)
        {
            Exception exception = new Exception("One or more validation error occured.");
            foreach (CSCompilerValidationError error in errors)
            {
                exception.Data.Add(
                    error.OperationName,
                    string.Format(
                        "{0} number #{1}: {2}",
                        (error.IsWarning ? "Warning" : "Error"),
                        error.ErrorCode,
                        error.Message
                    )
                );
            }

            return exception;
        }
    }
}
