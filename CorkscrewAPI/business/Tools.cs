using Corkscrew.SDK.security;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Corkscrew.API.business
{
    internal static class Tools
    {

        public static CSUser GetCurrentlyAuthenticatedUser(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new FaultException("Token is not valid.");
            }

            RemoteEndpointMessageProperty remp = OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            return CSUser.VerifyAPILogin(token, remp.Address);
        }


    }
}