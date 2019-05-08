<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Corkscrew.ControlCenter.sites.Delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Delete Site
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Delete Site
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Are you sure you wish to delete this site?
    </p>
    <p>
        Site Guid:
        <br />
        <asp:TextBox runat="server" ID="SiteGuid" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Site name: 
        <br />
        <asp:TextBox runat="server" ID="SiteName" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Description: 
        <br />
        <asp:TextBox runat="server" ID="SiteDescription" ReadOnly="true"  Columns="80" />
    </p>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />&nbsp;
        <asp:Button runat="server" ID="DeleteButton" UseSubmitBehavior="true" Text="DELETE" OnClick="DeleteButton_Click" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
