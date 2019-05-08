<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Step3.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.DefineWorkflow.Step3" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Define Workflow - Step 3 - Items in Workflow Manifest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Define Workflow - Step 3 - Items in Workflow Manifest
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        In this step of workflow definition, add source code and other files to the workflow manifest. These files will be used to compile and execute the workflow.
    </p>
    <p>
        Workflow Definition Guid:
        <br />
        <asp:TextBox runat="server" ID="DefinitionId" ReadOnly="true" Columns="80" Text="" />
    </p>
    <p>
        Upload file:<br />
        <asp:FileUpload runat="server" ID="ManifestFileUpload" AllowMultiple="false" /><br />
        <asp:CheckBox runat="server" ID="UnzipUploadedFileIfZipFile" Text=" Unzip this file if it is a .ZIP file." Checked="true" />
    </p>
    <p>
        (Optional) Rename file as:
        <br />
        <asp:TextBox runat="server" ID="RenameUploadedFile" MaxLength="512" Columns="80" />
    </p>
    <p>
        <asp:Button runat="server" ID="BackButton" Text="BACK" OnClick="BackButton_Click" />&nbsp;
        <asp:Button runat="server" ID="SubmitButton" Text="NEXT STEP &gt;&gt;" OnClick="SubmitButton_Click" />
    </p>
    <p>
        <asp:Label runat="server" ID="ErrorMessage" Text="" ForeColor="Yellow" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
