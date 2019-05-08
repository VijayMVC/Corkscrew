<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AddMember.aspx.cs" Inherits="Corkscrew.ControlCenter.usergroups.AddMember" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Usergroup Membership
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Manage Usergroup Membership
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">

    <p>
        Manage membership of the user group using this page. You can add or remove users. If the same user is added to mulitple groups, their effective permissions 
        will be the sum of the least privileges among all the groups.
    </p>

    <p>
        User group being managed:
        <asp:Label runat="server" ID="Username" />
    </p>

    <table style="width: 100%; height: 100%; padding: 5px;" cellpadding="5" border="0">
        <tr>
            <td>Users who are not in this group:
            </td>
            <td>&nbsp;</td>
            <td>Users who are in this group:
            </td>
        </tr>
        <tr>
            <td>
                <asp:ListBox runat="server" ID="lbAvailableUsers" SelectionMode="Multiple"></asp:ListBox>
            </td>
            <td style="align-content: center; align-items: center; text-align: center;">
                <asp:Button runat="server" ID="AddToGroup" Text="&gt;&gt;" OnClick="AddToGroup_Click" />
                <br />
                <asp:Button runat="server" ID="RemoveFromGroup" Text="&lt;&lt;" OnClick="RemoveFromGroup_Click" />
            </td>
            <td>
                <asp:ListBox runat="server" ID="lbGroupMembers" SelectionMode="Multiple"></asp:ListBox>
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
