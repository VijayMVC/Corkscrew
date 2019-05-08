<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Corkscrew.ControlCenter.usergroups.Delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Delete User Group
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Delete User Group
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Are you sure you wish to delete this user group? Membership of the group will be cleared (all users will be forced to leave the group), but user accounts will remain untouched.
    </p>
    <p>
        Group Guid:
        <br />
        <asp:TextBox runat="server" ID="UsersGuid" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Group alias: 
        <br />
        <asp:TextBox runat="server" ID="UsersUsername" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Display name: 
        <br />
        <asp:TextBox runat="server" ID="UsersDisplayname" ReadOnly="true"  Columns="80" />
    </p>
    <p>
        Email address: 
        <br />
        <asp:TextBox runat="server" ID="UsersEmailAddress" ReadOnly="true"  Columns="80" />
    </p>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />&nbsp;
        <asp:Button runat="server" ID="DeleteButton" UseSubmitBehavior="true" Text="DELETE" OnClick="DeleteButton_Click" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
