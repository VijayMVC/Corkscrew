using Corkscrew.SDK.objects;
using System;
using System.Web;

namespace Corkscrew.ControlCenter
{
    public partial class Default : System.Web.UI.Page
    {

        public CSFarm CurrentFarm
        {
            get
            {
                if (_farm == null)
                {
                    _farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));
                }

                return _farm;
            }
        }
        private CSFarm _farm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}