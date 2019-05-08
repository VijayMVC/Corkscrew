<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SiteACL.aspx.cs" Inherits="Corkscrew.ControlCenter.users.SetACL" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Set Access Control
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        
        input[type=radio]:checked+label {
            font-weight: bold;
            color: yellow;
        }

    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Set Access Control
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Add or remove permissions for this user. Using this page you can grant different levels of access to users to sites. Both Sites and Users must 
        already exist. You cannot edit permissions for sites where this user has Site Administrator rights. You cannot 
        change any permissions for users who are Global Administrators. To change restrict permissions for a Site or Global 
        Administrator, you must first down-grade them to a regular user.
    </p>
    <p>
        User being permissioned:
        <asp:Label runat="server" ID="UserName" ForeColor="Yellow" />
    </p><asp:ListView runat="server" ID="lvSitePermissions" InsertItemPosition="None">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlPermissionsTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Site name</th>
                    <th>Permissions</th>
                    <th>Action</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td style="padding: 5px; vertical-align: middle;">
                    <%# ((Corkscrew.SDK.tools.Utility.SafeString(DataBinder.Eval(Container.DataItem, "Name")) == "Corkscrew_ConfigDB") ? "Corkscrew Farm" : DataBinder.Eval(Container.DataItem, "Name"))  %>
                    <%# ((Corkscrew.SDK.tools.Utility.SafeConvertToGuid(DataBinder.Eval(Container.DataItem, "Id")) == Guid.Empty) ? "<span class='badge'>Farm</span>" : "" ) %>
                    <%# ((Corkscrew.SDK.tools.Utility.SafeConvertToGuid(DataBinder.Eval(Container.DataItem, "Id")) == Guid.Empty) ? "" : "<br />Id: " + DataBinder.Eval(Container.DataItem, "Id")) %> 
                </td>
                <td style="padding: 5px; vertical-align: middle;">
                    <asp:RadioButtonList runat="server" ID="SitePermissionsForUser" RepeatColumns="4" RepeatDirection="Horizontal">
                        <asp:ListItem Text="None" Value="N" />
                        <asp:ListItem Text="Read" Value="R" />
                        <asp:ListItem Text="Contribute" Value="C" />
                        <asp:ListItem Text="Full Control" Value="F" />
                    </asp:RadioButtonList>
                </td>
                <td style="padding: 5px; vertical-align: middle;">
                    <asp:LinkButton runat="server" ID="RowCommandLink" CommandName="Link" CommandArgument='<%# DataBinder.Eval( Container.DataItem, "Id" ) %>' Text="Update" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
