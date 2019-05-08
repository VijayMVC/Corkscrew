using Corkscrew.SDK.objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corkscrew.ControlCenter.sites
{
    public partial class All : System.Web.UI.Page
    {
        private CSFarm farm = null;
        public bool IsFarmAdministrator = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));
            IsFarmAdministrator = farm.IsAuthenticatedUserFarmAdministrator;

            if (lvDataView != null)
            {
                if (!IsPostBack)
                {
                    lvDataView.DataSource = farm.AllSites.ToList().OrderBy(u => u.Name);
                    lvDataView.DataBind();
                }
            }
        }
    }
}