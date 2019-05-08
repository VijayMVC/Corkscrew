
using Corkscrew.SDK.objects;
using Corkscrew.SDK.tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Corkscrew.ControlCenter.sites
{
    public partial class AddEdit : System.Web.UI.Page
    {

        private CSFarm farm = null;
        private Guid editSiteId = Guid.Empty;
        private CSSite editSite = null;


        protected void Page_Load(object sender, EventArgs e)
        {

            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));

            if (! string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                editSiteId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
                if (editSiteId != Guid.Empty)
                {
                    editSite = farm.AllSites.Find(editSiteId);
                    if (editSite == null)
                    {
                        Response.Redirect("/sites/All.aspx");
                    }

                    if (! IsPostBack)
                    {
                        SiteName.Text = editSite.Name;
                        Description.Text = editSite.Description;
                        DatabaseName.Text = editSite.ContentDatabaseName;
                        SiteQuotaBytes.Text = Utility.SafeString(editSite.QuotaBytes);

                        if (editSite.DNSNames.Count > 0)
                        {
                            SiteBindToDnsOption.Checked = true;
                            DNSHostNames.Enabled = true;
                            DNSHostNames.Text = string.Join(Environment.NewLine, editSite.DNSNames.ToList());
                        }

                        SiteName.Focus();
                    }
                }
                else
                {
                    Response.Redirect("/sites/All.aspx");
                }
            }
        }

        protected void AddEditButton_Click(object sender, EventArgs e)
        {

            long quota = Utility.SafeConvertToLong(SiteQuotaBytes.Text);
            if (quota < 1)
            {
                quota = 0;
            }

            if (editSite == null)
            {
                if (! CSSite.Exists(SiteName.Text))
                {
                    editSite = farm.CreateSite
                    (
                        SiteName.Text,
                        Description.Text,
                        DatabaseName.Text,
                        quota
                    );
                }
                else
                {
                    rfvSiteName.IsValid = false;
                    return;
                }
            }
            else
            {
                if (! editSite.Name.Equals(SiteName.Text))
                {
                    if (CSSite.Exists(SiteName.Text))
                    {
                        rfvSiteName.IsValid = false;
                        return;
                    }

                    editSite.Name = SiteName.Text;
                }

                editSite.Description = Description.Text;
                editSite.QuotaBytes = quota;

                editSite.Save();
            }

            if ((SiteBindToDnsOption.Checked) && (! string.IsNullOrEmpty(DNSHostNames.Text)))
            {
                List<string> requestedDnsNames = DNSHostNames.Text.Split(new string[] { Environment.NewLine, ",", ";" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                foreach(string dns in requestedDnsNames)
                {
                    if (editSite.DNSNames.IndexOf(dns) == -1)
                    {
                        editSite.DNSNames.Add(dns);
                    }
                }

                foreach(string dns in editSite.DNSNames)
                {
                    if (requestedDnsNames.IndexOf(dns) == -1)
                    {
                        editSite.DNSNames.Remove(dns);
                    }
                }
            }

            Response.Redirect("/sites/All.aspx");
        }

        protected void SiteBindToDnsOption_CheckedChanged(object sender, EventArgs e)
        {
            DNSHostNames.Enabled = SiteBindToDnsOption.Checked;
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/sites/All.aspx");
        }
    }
}