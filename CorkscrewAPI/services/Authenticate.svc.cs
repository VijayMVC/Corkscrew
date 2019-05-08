using Corkscrew.SDK.security;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Corkscrew.API.services
{
    /// <summary>
    /// Any new client-session must call into this service to obtain the session token. Without this session-token, 
    /// all subsequent calls will fail.
    /// </summary>
    public class Authenticate : IAuthenticate
    {
        /// <summary>
        /// Login to the Corkscrew system
        /// </summary>
        /// <param name="username">Username to login with</param>
        /// <param name="password">SHA-256 hash of the corresponding password to login with</param>
        /// <returns>The session authentication token. All API calls require this token to be passed.</returns>
        public string Login(string username, string password)
        {
            RemoteEndpointMessageProperty remp = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string tokenstring = CSUser.APILogin(username, password, remp.Address);
            if (tokenstring == null)
            {
                throw new FaultException("Username or password is invalid.");
            }

            return tokenstring;
        }
    }
}
