<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Step1.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.DefineWorkflow.Step1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Define Workflow - Step 1 - Workflow Metadata
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Define Workflow - Step 1 - Workflow Metadata
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Start defining the workflow. Enter the basic metadata for the workflow on this page. 
    </p>
    <p>
        Guid: <br />
        <asp:TextBox runat="server" ID="DefinitionId" ReadOnly="true" Columns="80" Text="(auto-generated on save)" />
    </p>
    <p>
        Name: <asp:RequiredFieldValidator runat="server" ID="rfvName" ControlToValidate="Name" ErrorMessage=" *" ForeColor="Yellow" /><br />
        <asp:TextBox runat="server" ID="Name" Columns="80" MaxLength="255" />
    </p>
    <p>
        Description: <br />
        <asp:TextBox runat="server" ID="Description" Columns="80" Rows="4" MaxLength="1024" TextMode="MultiLine" />
    </p>
    <p>
        Default association data: <br />
        <asp:TextBox runat="server" ID="DefaultAssociationData" Columns="80" Rows="4" TextMode="MultiLine" />
    </p>
    <p>
        When should the workflow instances trigger?<br />
        <asp:CheckBox runat="server" ID="StartOnCreate" Checked="true" Text=" When a new item is created" /><br />
        <asp:CheckBox runat="server" ID="StartOnModify" Checked="false" Text=" When an existing item is modified" />
    </p>
    <p>
        Enable or disable this definition:<br />
        <asp:CheckBox runat="server" ID="IsEnabled" Checked="true" Text=" Set as enabled" /><br />
    </p>
    <p>
        What Corkscrew object-model events can this workflow be instantiated for?<br />
        <asp:CheckBoxList runat="server" ID="EventsList" RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Table">
            <asp:ListItem Text=" Farm Created" Value="farm_created" />
            <asp:ListItem Text=" Farm Modified" Value="farm_modified" />
            <asp:ListItem Text=" Farm Deleted" Value="farm_deleted" />
            <asp:ListItem Text=" Site Created" Value="site_created" />
            <asp:ListItem Text=" Site Modified" Value="site_modified" />
            <asp:ListItem Text=" Site Deleted" Value="site_deleted" />
            <asp:ListItem Text=" Directory Created" Value="directory_created" />
            <asp:ListItem Text=" Directory Modified" Value="directory_modified" />
            <asp:ListItem Text=" Directory Deleted" Value="directory_deleted" />
            <asp:ListItem Text=" File Created" Value="file_created" />
            <asp:ListItem Text=" File Modified" Value="file_modified" />
            <asp:ListItem Text=" File Deleted" Value="file_deleted" />
        </asp:CheckBoxList>
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
