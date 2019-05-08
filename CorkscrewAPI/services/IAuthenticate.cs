using System.ServiceModel;

namespace Corkscrew.API.services
{

    /// <summary>
    /// Any new client-session must call into this service to obtain the session token. Without this session-token, 
    /// all subsequent calls will fail.
    /// </summary>
    [ServiceContract]
    public interface IAuthenticate
    {

        /// <summary>
        /// Login to the Corkscrew system
        /// </summary>
        /// <param name="username">Username to login with</param>
        /// <param name="password">Corresponding password to login with</param>
        /// <returns>The session authentication token. All API calls require this token to be passed.</returns>
        [OperationContract]
        string Login(string username, string password);

    }
}
