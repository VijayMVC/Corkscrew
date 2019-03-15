using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Corkscrew.SDK.providers.httpmodules
{

    /// <summary>
    /// This module takes care of mapping requests for static files to the IIS StaticFileHandler module.
    /// Without this module, this mapping will need to be taken care of in the web.config [handlers] node.
    /// </summary>
    public class CorkscrewMapRequestHandlerModule : IHttpModule
    {

        private IHttpHandler _staticFileHandler = null;
        private List<string> _ignoreExtensions = new List<string>();

        /// <summary>
        /// Dispose the module
        /// </summary>
        public void Dispose()
        {
            _staticFileHandler = null;
            _ignoreExtensions = null;
        }

        /// <summary>
        /// Initialize the module
        /// </summary>
        /// <param name="context">The HttpApplication context</param>
        public void Init(HttpApplication context)
        {
            Type t_sfh = typeof(HttpApplication).Assembly.GetType("System.Web.StaticFileHandler", true);    // if this throws we will end up with a HTTP 500
            _staticFileHandler = (IHttpHandler)Activator.CreateInstance(t_sfh, true);
            t_sfh = null;

            string nonStaticExtensions = ".aspx,.master";
            CSConfigurationCollection cfg = new CSConfigurationCollection(CSFarm.Open(CSUser.CreateAnonymousUser()));
            if (cfg.Contains("Corkscrew/Farm/Defaults/RequestHandler/IgnoreExtensions"))
            {
                nonStaticExtensions = cfg["Corkscrew/Farm/Defaults/RequestHandler/IgnoreExtensions"];
            }

            if (!string.IsNullOrEmpty(nonStaticExtensions))
            {
                _ignoreExtensions = nonStaticExtensions.Replace("*.", ".").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            if ((_ignoreExtensions != null) && (_ignoreExtensions.Count > 0))
            {
                // we attempt to map the handler in both the PostAuthorizeRequest and PostMapRequestHandler events
                // based on the request, it will succeed in one of these two events
                context.PostAuthorizeRequest += RedirectToStaticHandler;
                context.PostMapRequestHandler += RedirectToStaticHandler;
            }
        }

        /// <summary>
        /// Event handler that handles redirection to the StaticFileHandler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RedirectToStaticHandler(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;

            // check if handler is already mapped
            if (context.Handler == null)
            {
                string requestResourceExtension = Path.GetExtension(context.Request.Url.AbsolutePath);

                if ((_ignoreExtensions != null) && (_ignoreExtensions.Count > 0) && (!_ignoreExtensions.ContainsNoCase(requestResourceExtension, true)))
                {
                    if (context.CurrentNotification == RequestNotification.AuthorizeRequest)
                    {
                        ((HttpApplication)sender).Context.RemapHandler(_staticFileHandler);
                    }
                    else if (context.CurrentNotification == RequestNotification.MapRequestHandler)
                    {
                        ((HttpApplication)sender).Context.Handler = _staticFileHandler;
                    }
                }

            }
        }
    }
}
