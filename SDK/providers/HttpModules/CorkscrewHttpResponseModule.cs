using System;
using System.Web;

namespace Corkscrew.SDK.providers.httpmodules
{

    /// <summary>
    /// Handles HTTP Response modification
    /// We change the headers in the response data during PreSendRequestHeaders event in the pipeline. 
    /// We change the ASP.NET "Server" header to read our name, and modify the Content-Type header in some cases.
    /// </summary>
    public class CorkscrewHttpResponseModule : IHttpModule
    {

        /// <summary>
        /// Dispose the module
        /// </summary>
        public void Dispose()
        {
            //
        }

        /// <summary>
        /// Initialize the module
        /// </summary>
        /// <param name="context">The HttpApplication context</param>
        public void Init(HttpApplication context)
        {
            context.PreSendRequestHeaders += context_PreSendRequestHeaders;
        }

        /// <summary>
        /// Event handler for the PreSendRequestHeaders event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
            {
                // avoid throwing an error here!
                return;
            }

            HttpResponse response = HttpContext.Current.Response;

            if (response.Headers != null)
            {
                // remove headers we do not want
                response.Headers.Remove("Server");
                response.Headers.Add("Server", "Corkscrew CMS Web Server");

                // modify some headers
                if ((response.StatusCode != 200) && (response.Headers["Content-Type"] != null))
                {
                    string[] contentTypeHeader = response.ContentType.ToLowerInvariant().Split(new char[] { ';' });

                    switch (contentTypeHeader[0])
                    {
                        case "text/html":
                            response.Headers.Add("X-UA-Compatible", "IE=edge");
                            break;

                        case "application/javascript":
                            // changing this content type helps with performance
                            response.ContentType = "text/javascript";
                            break;
                    }
                }

                
            }
        }

    }
}
