using Corkscrew.SDK.objects;
using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.users
{
    public partial class SetACL : System.Web.UI.Page
    {
        private CSUser user = null;
        private CSFarm farm = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Guid UserId = Utility.SafeConvertToGuid(Request.QueryString["Id"]);
            if (UserId.Equals(Guid.Empty))
            {
                Response.Redirect("/users/All.aspx");
            }

            farm = CSFarm.Open(WebHelpers.GetSessionUser(HttpContext.Current));
            user = CSUser.GetById(UserId);
            if ((user == null) || (user.IsSystemUser()))
            {
                Response.Redirect("/users/All.aspx");
            }

            UserName.Text = Server.HtmlEncode(user.LongformDisplayName);
            lvSitePermissions.ItemCreated += lvSitePermissions_ItemCreated;
            lvSitePermissions.ItemCommand += lvSitePermissions_ItemCommand;

            if (!IsPostBack)
            {
                lvSitePermissions.DataSource = farm.AllSites.ToList();
                lvSitePermissions.DataBind();
            }
        }

        protected void lvSitePermissions_ItemCreated(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                CSSite site = (CSSite)e.Item.DataItem;
                CSPermission acl = CSPermission.TestAccess(site, null, user);

                // dont bind hierarchical access acls
                if (acl.IsHierarchicalAccess)
                {
                    return;
                }

                RadioButtonList rbl = (RadioButtonList)e.Item.FindControl("SitePermissionsForUser");
                if (rbl != null)
                {
                    rbl.ClearSelection();
                    string value = (acl.CanFullControl ? "F" : (acl.CanContribute ? "C" : (acl.CanRead ? "R" : "")));

                    if (!string.IsNullOrEmpty(value))
                    {
                        if (acl.IsSiteAdministrator || acl.IsFarmAdministrator)
                        {
                            rbl.Items.FindByValue("F").Selected = true;
                            rbl.Enabled = false;

                            LinkButton lb = (LinkButton)e.Item.FindControl("RowCommandLink");
                            if (lb != null)
                            {
                                lb.Visible = false;
                            }

                        }
                        else
                        {
                            rbl.Items.FindByValue(value).Selected = true;
                        }
                    }
                }
            }
        }

        protected void lvSitePermissions_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            Guid siteId = Utility.SafeConvertToGuid(e.CommandArgument);

            CSPermission acl = CSPermission.TestAccess( ((siteId == Guid.Empty) ? null : farm.AllSites.Find(siteId)), null, user);

            if ((!acl.IsSiteAdministrator) && (!acl.IsFarmAdministrator))
            {
                RadioButtonList rbl = (RadioButtonList)e.Item.FindControl("SitePermissionsForUser");
                if (rbl != null)
                {
                    if (!string.IsNullOrEmpty(rbl.SelectedValue))
                    {
                        string oldACL = (acl.CanFullControl ? "F" : (acl.CanContribute ? "C" : (acl.CanRead ? "R" : "H")));

                        if (!oldACL.Equals(rbl.SelectedValue))
                        {
                            switch (rbl.SelectedValue)
                            {
                                case "N":
                                    acl.CanRead = false;
                                    acl.CanContribute = false;
                                    acl.CanFullControl = false;
                                    break;

                                case "R":
                                    acl.CanRead = true;
                                    acl.CanContribute = false;
                                    acl.CanFullControl = false;
                                    break;

                                case "C":
                                    acl.CanRead = false;
                                    acl.CanContribute = true;
                                    acl.CanFullControl = false;
                                    break;

                                case "F":
                                    acl.CanRead = false;
                                    acl.CanContribute = false;
                                    acl.CanFullControl = true;
                                    break;
                            }

                            acl.Save();
                        }
                    }
                }
            }
        }
    }
}