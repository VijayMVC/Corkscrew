using Corkscrew.SDK.security;
using Corkscrew.SDK.tools;
using System;

namespace Corkscrew.ControlCenter.usergroups
{
    public partial class AddEdit : System.Web.UI.Page
    {

        private Guid editGroupId = Guid.Empty;
        private CSUserGroup editGroup = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (! string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                editGroupId = Utility.SafeConvertToGuid(Request.QueryString["id"]);
                if (editGroupId != Guid.Empty)
                {
                    editGroup = CSUserGroup.GetById(editGroupId);
                    if (editGroup == null)
                    {
                        Response.Redirect("/usergroups/All.aspx");
                    }

                    if (! IsPostBack)
                    {
                        UsersUsername.Text = editGroup.Username;
                        UsersDisplayname.Text = editGroup.DisplayName;
                        UsersEmailAddress.Text = editGroup.EmailAddress;

                        UsersUsername.ReadOnly = true;
                        UsersDisplayname.Focus();
                    }
                }
            }

            if (string.IsNullOrEmpty(UsersUsername.Text))
            {
                UsersUsername.Focus();
            }
        }

        protected void AddEditButton_Click(object sender, EventArgs e)
        {
            if (editGroup == null)
            {
                if (! CSUserGroup.Exists(UsersUsername.Text))
                {
                    CSUserGroup.CreateUserGroup(
                        UsersUsername.Text, 
                        UsersDisplayname.Text,
                        UsersEmailAddress.Text
                    );
                }
            }
            else
            {
                editGroup.DisplayName = UsersDisplayname.Text;
                editGroup.EmailAddress = UsersEmailAddress.Text;
                editGroup.Save();
            }

            Response.Redirect("/usergroups/All.aspx");
        }
    }
}