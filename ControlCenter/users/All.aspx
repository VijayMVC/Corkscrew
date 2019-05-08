<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="All.aspx.cs" Inherits="Corkscrew.ControlCenter.users.All" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    All Users
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    All Users
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>&nbsp;</p>
    <p>
        Add users in Corkscrew to set fine-grained permissions on sites and filesystem objects.
    </p>

    <script runat="server">

        string strGuid = Guid.Empty.ToString("d");
        bool currentUserIsMe = false;
        Corkscrew.SDK.security.CSUser me = Corkscrew.ControlCenter.WebHelpers.GetSessionUser(HttpContext.Current);

        private string ParseUser(Corkscrew.SDK.security.CSUser binderUser)
        {
            currentUserIsMe = (me.Id.Equals(binderUser.Id));
            strGuid = binderUser.Id.ToString("d");
            return "";
        }

        private bool IsFarmAdministrator(object dataItem)
        {
            return Corkscrew.SDK.security.CSPermission.TestAccess(null, null, (Corkscrew.SDK.security.CSUser)dataItem).IsFarmAdministrator;
        }

    </script>

    <p>
        <asp:HyperLink runat="server" ID="hlCreateUserHyperLink" Text="CREATE USER" NavigateUrl="/users/AddEdit.aspx" CssClass="buttonStyleLink" />
    </p>

    <asp:ListView runat="server" ID="lvDataView" InsertItemPosition="None">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <%# ParseUser((Corkscrew.SDK.security.CSUser)Container.DataItem) %>
            <asp:HiddenField runat="server" ID="hiddenUserIdRow" Value='<%# strGuid %>' />
            <tr>
                <td style="padding: 10px; vertical-align: top;">
                    <p>
                        <strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LongformDisplayName").ToString()) %></strong>
                        <%# (IsFarmAdministrator(Container.DataItem) ? "<span class=\"badge\">Farm Administrator</span>"  : "") %>
                        <%# (currentUserIsMe ? "<span class=\"badge\">ME</span>" : "") %>
                        <br />
                        <br />
                        <blockquote>User Guid: <%# strGuid %></blockquote>
                    </p>

                    <p>
                        <asp:LinkButton runat="server" ID="RowSetPermissionsButton" Text="Site Access" CommandName="SiteACL" CommandArgument='<%# strGuid %>'
                            CssClass="buttonStyleLink" CausesValidation="false" />

                        <asp:LinkButton runat="server" ID="RowEditButton" Text="Edit User" CommandName="EditUser" CommandArgument='<%# strGuid %>'
                            CssClass="buttonStyleLink" CausesValidation="false" />

                        <asp:LinkButton runat="server" ID="RowDeleteButton" Text="Delete User" CssClass="buttonStyleLink" CommandName="DeleteUser" CommandArgument='<%# strGuid %>'
                            CausesValidation="false"
                            OnClientClick="return confirm('Are you sure you want to delete this user? This action cannot be undone!!!');"
                            Visible='<%# !currentUserIsMe %>' />
                    </p>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    <p>&nbsp;</p>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
