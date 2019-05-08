<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="All.aspx.cs" Inherits="Corkscrew.ControlCenter.usergroups.All" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    All User Groups
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    All User Groups
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>&nbsp;</p>
    <p>
        Add user groups in Corkscrew to set permissions for large groups of users and more efficiently manage permissions.
    </p>

    <p>
        <asp:HyperLink runat="server" ID="hlCreateUserHyperLink" Text="CREATE USER GROUP" NavigateUrl="/usergroups/AddEdit.aspx" CssClass="buttonStyleLink" />
    </p>

    <asp:ListView runat="server" ID="lvDataView" InsertItemPosition="None">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <asp:HiddenField runat="server" ID="hiddenUserIdRow" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
            <tr>
                <td style="padding: 10px; vertical-align: top;">
                    <p>
                        <strong><%# Server.HtmlEncode(DataBinder.Eval(Container.DataItem, "LongformDisplayName").ToString()) %></strong>
                        <br />
                        <br />
                        <blockquote>Usergroup Guid: <%# DataBinder.Eval(Container.DataItem, "Id") %></blockquote>
                    </p>

                    <p>
                        <asp:LinkButton runat="server" ID="RowSetPermissionsButton" Text="Site Access" CommandName="SiteACL" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            CssClass="buttonStyleLink" CausesValidation="false" />

                        <asp:LinkButton runat="server" ID="RowEditButton" Text="Edit Group" CommandName="EditGroup" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            CssClass="buttonStyleLink" CausesValidation="false" />

                        <asp:LinkButton runat="server" ID="RowDeleteButton" Text="Delete Group" CssClass="buttonStyleLink" CommandName="DeleteGroup" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>'
                            CausesValidation="false"
                            OnClientClick="return confirm('Are you sure you want to delete this user group? Doing so will clear the group's membership, but will NOT delete the actual user accounts. This action cannot be undone!!!');" />

                        <asp:LinkButton runat="server" ID="RowAddMembershipButton" Text="Manage Members" CssClass="buttonStyleLink" CommandName="ManageMembership" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' 
                            CausesValidation="false" />
                    </p>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
    <p>&nbsp;</p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
