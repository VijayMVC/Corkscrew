using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Corkscrew.ControlCenter.filesystem
{
    /// <summary>
    /// Summary description for GetFileExtensionIcon
    /// </summary>
    public class GetFileExtensionIcon : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string operation = context.Request.Params["o"];

            switch (operation.ToLower())
            {
                case "getbyextension":
                    GetByExtension(context);
                    break;
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        private void GetByExtension(HttpContext context)
        {
            string extension = Utility.SafeString(context.Request.Params["extension"], removeAtStart: ".");
            FileInfo file = null;

            if (string.IsNullOrEmpty(extension))
            {
                file = new FileInfo(HttpContext.Current.Server.MapPath("/resources/images/icon_file.png"));

                HttpContext.Current.Response.AddHeader("Content-length", file.Length.ToString());
                HttpContext.Current.Response.WriteFile("/resources/images/icon_file.png");
            }
            else
            {
                // check if we have a file extension icon
                string extensionIconFilePath = HttpContext.Current.Server.MapPath("/resources/mimetype-icons/" + extension + ".png");

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "image/png";

                if (File.Exists(extensionIconFilePath))
                {
                    file = new FileInfo(extensionIconFilePath);

                    HttpContext.Current.Response.AddHeader("Content-length", file.Length.ToString());
                    HttpContext.Current.Response.WriteFile("/resources/mimetype-icons/" + extension + ".png");
                }
                else
                {
                    file = new FileInfo(HttpContext.Current.Server.MapPath("/resources/images/icon_file.png"));

                    HttpContext.Current.Response.AddHeader("Content-length", file.Length.ToString());
                    HttpContext.Current.Response.WriteFile("/resources/images/icon_file.png");
                }
            }

            HttpContext.Current.Response.End();
        }
    }
}