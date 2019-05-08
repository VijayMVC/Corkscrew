using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace Corkscrew.ControlCenter.usergroups
{
    public partial class AddMember : System.Web.UI.Page
    {
        private Guid editGroupId = Guid.Empty;
        private CSUserGroup editGroup = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            editGroupId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
            if (editGroupId == Guid.Empty)
            {
                Response.Redirect("/usergroups/All.aspx");
            }
            editGroup = CSUserGroup.GetById(editGroupId);
            if (editGroup == null)
            {
                Response.Redirect("/usergroups/All.aspx");
            }

            if (!IsPostBack)
            {
                Username.Text = editGroup.LongformDisplayName;

                lbAvailableUsers.DataTextField = "Username";
                lbAvailableUsers.DataValueField = "Id";
                lbAvailableUsers.DataSource = editGroup.GetNonMembers().OrderBy(u => u.LongformDisplayName);
                lbAvailableUsers.DataBind();

                lbGroupMembers.DataTextField = "Username";
                lbGroupMembers.DataValueField = "Id";
                lbGroupMembers.DataSource = editGroup.Members.OrderBy(u => u.LongformDisplayName);
                lbGroupMembers.DataBind();
            }

            if (lbAvailableUsers.Items.Count > 0)
            {
                lbAvailableUsers.Focus();
            }
            else if (lbGroupMembers.Items.Count > 0)
            {
                lbGroupMembers.Focus();
            }
        }

        protected void AddToGroup_Click(object sender, EventArgs e)
        {
            foreach(ListItem li in lbAvailableUsers.Items)
            {
                if (li.Selected)
                {
                    Guid id = Utility.SafeConvertToGuid(li.Value);
                    if (id != Guid.Empty)
                    {
                        CSUser user = CSUser.GetById(id);
                        if (user != null)
                        {
                            editGroup.Add(user);
                        }
                    }
                }
            }

            Response.Redirect(Request.Url.ToString());
        }

        protected void RemoveFromGroup_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in lbGroupMembers.Items)
            {
                if (li.Selected)
                {
                    Guid id = Utility.SafeConvertToGuid(li.Value);
                    if (id != Guid.Empty)
                    {
                        CSUser user = CSUser.GetById(id);
                        if (user != null)
                        {
                            editGroup.Remove(user);
                        }
                    }
                }
            }

            Response.Redirect(Request.Url.ToString());
        }

    }
}