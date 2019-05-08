<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SiteAdministrators.aspx.cs" Inherits="Corkscrew.ControlCenter.sites.SiteAdministrators" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Site Administrators
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Site Administrators
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        This page allows you configure users as Site Administrators for Corkscrew websites. Currently we are setting up 
        site administrators for the website: <u><asp:Label runat="server" ID="SiteName" Text="" /></u>. From the list of users 
        below, check ON users you wish to add as administrators and check OFF users you wish to remove from this role. Existing users 
        are shown as already checked ON.
    </p>
    <p>
        <strong>NOTE:</strong> Global Administrators cannot be checked OFF. To remove Global Administrators you need to delete or disable their account.
    </p>
    <p>
        <asp:CheckBoxList runat="server" ID="Users" RenderWhenDataEmpty="false" RepeatDirection="Vertical" RepeatColumns="1" />
    </p>
    <p>
        <asp:Button runat="server" ID="SaveAdministratorsButton" Text="Save Administrators" OnClick="SaveAdministratorsButton_Click" /> 
        &nbsp;
        <asp:Button runat="server" ID="CancelButton" Text="Cancel" OnClick="CancelButton_Click" />
    </p>
    <p>&nbsp;</p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
