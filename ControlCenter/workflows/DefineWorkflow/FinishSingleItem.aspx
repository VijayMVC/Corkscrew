<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="FinishSingleItem.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.DefineWorkflow.FinishSingleItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Define Workflow - Finish - Set attributes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Define Workflow - Finish - Set attributes
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        You either uploaded a file in the previous step or entered a filename. Now, provide the attributes for that file. 
    </p>
    <p>
        Workflow Definition Guid:
        <br />
        <asp:TextBox runat="server" ID="DefinitionId" ReadOnly="true" Columns="80" Text="" />
    </p>
    <p>
        Filename:<br />
        <asp:TextBox runat="server" ID="rowFilename" MaxLength="255" Columns="30" Text="" />
    </p>
    <p>
        Filename extension:<br />
        <asp:TextBox runat="server" ID="rowFilenameExtension" MaxLength="255" Columns="30" Text="" />
    </p>
    <p>
        Item type:<br />
        <asp:DropDownList runat="server" ID="rowItemType">
            <asp:ListItem Text="Primary assembly (contains workflow)" Value="PrimaryAssembly" />
            <asp:ListItem Text="Dependency assembly (primary assembly depends on this)" Value="DependencyAssembly" />
            <asp:ListItem Text="Source code file (*.cs, *.vb, etc)" Value="SourceCodeFile" />
            <asp:ListItem Text="Xaml file (*.xaml, *.xamlx)" Value="XamlFile" />
            <asp:ListItem Text="Configuration file (*.config)" Value="ConfigurationFile" />
            <asp:ListItem Text="Media file (images, audio, video)" Value="MediaResourceFile" />
            <asp:ListItem Text="Stylesheet file (*.css)" Value="Stylesheet" />
            <asp:ListItem Text="Custom data file (*.xml, *.txt, etc)" Value="CustomDataFile" />
            <asp:ListItem Text="Resource file (*.res, *.resx, *.resource)" Value="ResourceFile" />
        </asp:DropDownList>
    </p>
    <p>
        Build-time folder: <br />
        <asp:TextBox runat="server" ID="rowBuildFolder" MaxLength="255" Columns="30" Text="" />
    </p>
    <p>
        Run-time folder:<br />
        <asp:CheckBox runat="server" ID="rowRequiredForExec" Text=" Is required for execution" Checked="false" /><br />
        <asp:TextBox runat="server" ID="rowRuntimeFolder" MaxLength="255" Columns="30" Text="" />
    </p>
    <p>
        <asp:Button runat="server" ID="BackButton" Text="BACK" OnClick="BackButton_Click" />&nbsp;
        <asp:Button runat="server" ID="SubmitButton" Text="FINISH" OnClick="SubmitButton_Click" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
