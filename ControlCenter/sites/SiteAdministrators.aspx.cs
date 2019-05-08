using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System.Web;

namespace Corkscrew.ControlCenter.sites
{
    public partial class SiteAdministrators : System.Web.UI.Page
    {
        private CSSite _site = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid siteId = Utility.SafeConvertToGuid(Request.QueryString["Id"]);

            _site = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current)).AllSites.Find(siteId);
            if (_site == null)
            {
                Response.Redirect("sites/All.aspx");
            }

            if (!IsPostBack)
            {
                SiteName.Text = _site.Name;

                CSUserCollection allUsers = _site.Farm.AllUsers;
                Users.Items.Clear();

                if (allUsers.Count > 0)
                {
                    foreach (CSUser user in allUsers)
                    {
                        ListItem item = new ListItem(Server.HtmlEncode(user.LongformDisplayName), user.Id.ToString("d"));

                        CSPermission acl = CSPermission.TestAccess(_site, null, user);
                        if ((acl.CanFullControl) && (acl.IsSiteAdministrator || acl.IsFarmAdministrator))
                        {
                            item.Selected = true;
                            item.Enabled = (!acl.IsFarmAdministrator);
                        }

                        Users.Items.Add(item);
                    }
                }

                Users.Attributes["data-item-previous-states"] = SerializeSelectedItems(Users);
            }
        }

        protected void SaveAdministratorsButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, bool> previousStates = DeserializeSelectedItems(Users);

            foreach (ListItem item in Users.Items)
            {
                if (item.Enabled)
                {
                    Guid userId = Utility.SafeConvertToGuid(item.Value);
                    if (item.Selected != previousStates[item.Value])
                    {
                        CSPermission acl = CSPermission.TestAccess(_site, null, CSUser.GetById(userId));
                        if (!acl.IsFarmAdministrator)
                        {
                            acl.CanFullControl = !acl.IsSiteAdministrator;
                        }

                        acl.Save();
                    }
                }
            }

            Response.Redirect("/sites/All.aspx");
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("/sites/All.aspx");
        }

        private string SerializeSelectedItems(ListControl control)
        {
            string[] selections = new string[control.Items.Count];
            for (int i = 0; i < control.Items.Count; i++)
            {
                selections[i] = string.Format("{0};{1}", control.Items[i].Value, control.Items[i].Selected);
            }

            return string.Join("|", selections);
        }

        private Dictionary<string, bool> DeserializeSelectedItems(ListControl control)
        {
            Dictionary<string, bool> previousStates = new Dictionary<string, bool>();
            string[] state = Utility.SafeString(control.Attributes["data-item-previous-states"]).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string obj in state)
            {
                string[] kv = obj.Split(new char[] { ';' }, StringSplitOptions.None);
                previousStates.Add(Utility.SafeString(kv[0]), Utility.SafeConvertToBool(kv[1]));
            }

            return previousStates;
        }
    }
}