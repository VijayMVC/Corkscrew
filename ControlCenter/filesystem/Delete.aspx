<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Delete.aspx.cs" Inherits="Corkscrew.ControlCenter.filesystem.Delete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Delete <%= ItemType %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Delete <%= ItemType %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Are you sure you wish to delete this <%= ItemType %>?
    </p>
    <p>
        Guid: <br />
        <asp:TextBox runat="server" ID="ItemGuid" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Filename: <br />
        <asp:TextBox runat="server" ID="ItemNameWithExtension" ReadOnly="true" Columns="80" />
    </p>
    <p>
        Corkscrew Uri: <br />
        <asp:TextBox runat="server" ID="CorkscrewUri" ReadOnly="true" Columns="80" />
    </p>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />&nbsp;
        <asp:Button runat="server" ID="DeleteButton" UseSubmitBehavior="true" Text="DELETE" OnClick="DeleteButton_Click" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
