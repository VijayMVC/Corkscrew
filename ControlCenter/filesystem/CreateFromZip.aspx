<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="CreateFromZip.aspx.cs" Inherits="Corkscrew.ControlCenter.filesystem.CreateFromZip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Create from Zip file
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Create from Zip file
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Upload a .ZIP file. Corkscrew will extract the directories and files and match the structure and content. Items that are in the .ZIP file but not in Corkscrew FS will be 
        created, items that are present will be updated using content from the .ZIP file.
    </p>
    <p>
        Select file to upload:<br />
        <asp:FileUpload runat="server" ID="ZipFileUpload" AllowMultiple="false" />
    </p>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />&nbsp;
        <asp:Button runat="server" ID="UploadAndCreateButton" UseSubmitBehavior="true" Text="UPLOAD" OnClick="UploadAndCreateButton_Click" />
    </p>
    <p>
        <asp:Label runat="server" ID="ErrorMessage" Text="" />
        <asp:HyperLink runat="server" ID="BackToFSLink" CssClass="buttonStyleLink" Text="&lt;&lt; Back" NavigateUrl="~/filesystem/Explorer.aspx" Visible="false" />
    </p>
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
