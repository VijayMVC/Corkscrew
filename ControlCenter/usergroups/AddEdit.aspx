<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AddEdit.aspx.cs" Inherits="Corkscrew.ControlCenter.usergroups.AddEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Add or Modify User Group
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Add or Modify User Group
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Create a user group or modify an existing user group. Enter the information below and click the button. All the fields are mandatory to fill in.
    </p>
    <p>
        Group alias name: 
        <asp:RequiredFieldValidator runat="server" ID="rfvUsersUserName" ControlToValidate="UsersUsername" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="ItemValidationGroup" />
        <br />
        <asp:TextBox runat="server" ID="UsersUsername" MaxLength="255" Columns="80" />
    </p>
    <p>
        Display name: 
        <asp:RequiredFieldValidator runat="server" ID="rfvUsersDisplayName" ControlToValidate="UsersDisplayname" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="ItemValidationGroup" />
        <br />
        <asp:TextBox runat="server" ID="UsersDisplayname" MaxLength="255" Columns="80" AutoCompleteType="DisplayName" />
    </p>
    <p>
        Email address: 
        <asp:RequiredFieldValidator runat="server" ID="rfvUsersEmailAddress" ControlToValidate="UsersEmailAddress" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="ItemValidationGroup" />
        <br />
        <asp:TextBox runat="server" ID="UsersEmailAddress" MaxLength="255" Columns="80" AutoCompleteType="Email" />
    </p>
    <p>
        <asp:Button runat="server" ID="AddEditButton" UseSubmitBehavior="true" Text="SUBMIT" OnClick="AddEditButton_Click" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
