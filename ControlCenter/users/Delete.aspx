<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Corkscrew.ControlCenter.users.Delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Delete User
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Delete User
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Are you sure you wish to delete this user account?
    </p>
    <p>
        User Guid:
        <br />
        <asp:TextBox runat="server" ID="UsersGuid" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Username: 
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
