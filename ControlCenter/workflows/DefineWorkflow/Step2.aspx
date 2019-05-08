<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Step2.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.DefineWorkflow.Step2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Define Workflow - Step 2 - Workflow Manifest
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Define Workflow - Step 2 - Workflow Manifest
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        In this next step of workflow definition, enter information about what executes when the workflow is instantiated and how that is loaded and triggered.
    </p>
    <p>
        Workflow Definition Guid: <br />
        <asp:TextBox runat="server" ID="DefinitionId" ReadOnly="true" Columns="80" Text="" />
    </p>
    <p>
        Workflow execution engine: <br />
        <asp:DropDownList runat="server" ID="ExecutionEngine">
            <asp:ListItem Text="Code-only Corkscrew Workflow" Value="CS1C" />
            <asp:ListItem Text=".NET Workflow Foundation v3 Xaml Workflow" Value="WF3X" />
            <asp:ListItem Text=".NET Workflow Foundation v3 Coded Workflow" Value="WF3C" />
            <asp:ListItem Text=".NET Workflow Foundation v4 Xaml Workflow" Value="WF4X" />
            <asp:ListItem Text=".NET Workflow Foundation v4 Coded Workflow" Value="WF4C" />
        </asp:DropDownList>
    </p>
    <p>
        Name of the assembly (DLL or EXE) hosting the workflow code or scripts: 
        <asp:RequiredFieldValidator runat="server" ID="rfvWorkflowAssemblyName" ControlToValidate="WorkflowAssemblyName" ForeColor="Yellow" ErrorMessage=" *" />
        <br />
        <asp:TextBox runat="server" ID="WorkflowAssemblyName" MaxLength="255" Columns="80" />
    </p>
    <p>
        Name of the class in the assembly that should be executed:
        <asp:RequiredFieldValidator runat="server" ID="rfvWorkflowClassName" ControlToValidate="WorkflowClassName" ForeColor="Yellow" ErrorMessage=" *" />
        <br />
        <asp:TextBox runat="server" ID="WorkflowClassName" MaxLength="1024" Columns="80" />
    </p>
    <p>
        Build information: <br />
        <em>required only if you want Corkscrew to build the workflow assembly from source code. This is the same as the data provided in &quot;AssemblyInfo&quot; files in .NET</em>
    </p>
    <blockquote>
        <p>
            Product Title:<br />
            <asp:TextBox runat="server" ID="BuildProductTitle" MaxLength="255" Columns="75" />
        </p>
        <p>
            Product Name:<br />
            <asp:TextBox runat="server" ID="BuildProductName" MaxLength="255" Columns="75" />
        </p>
        <p>
            Product Description:<br />
            <asp:TextBox runat="server" ID="BuildProductDescription" MaxLength="255" Columns="75" />
        </p>
        <p>
            Company:<br />
            <asp:TextBox runat="server" ID="BuildCompany" MaxLength="255" Columns="75" />
        </p>
        <p>
            Copyright notice:<br />
            <asp:TextBox runat="server" ID="BuildCopyrightNotice" MaxLength="255" Columns="75" />
        </p>
        <p>
            Trademark notice:<br />
            <asp:TextBox runat="server" ID="BuildTrademarkNotice" MaxLength="255" Columns="75" />
        </p>
        <p style="vertical-align: text-bottom;">
            Build assembly version: <br />
            <asp:TextBox runat="server" ID="BuildVersionMajor" MaxLength="3" Columns="3" Text="1" />.
            <asp:TextBox runat="server" ID="BuildVersionMinor" MaxLength="3" Columns="3" Text="0" />.
            <asp:TextBox runat="server" ID="BuildVersionBuild" MaxLength="3" Columns="3" Text="0" />.
            <asp:TextBox runat="server" ID="BuildVersionRevision" MaxLength="3" Columns="3" Text="0" />
        </p>
        <p style="vertical-align: text-bottom;">
            Build assembly file version: <br />
            <asp:TextBox runat="server" ID="BuildFileVersionMajor" MaxLength="3" Columns="3" Text="1" />.
            <asp:TextBox runat="server" ID="BuildFileVersionMinor" MaxLength="3" Columns="3" Text="0" />.
            <asp:TextBox runat="server" ID="BuildFileVersionBuild" MaxLength="3" Columns="3" Text="0" />.
            <asp:TextBox runat="server" ID="BuildFileVersionRevision" MaxLength="3" Columns="3" Text="0" />
        </p>
    </blockquote>
    <p>
        How do you want Corkscrew to handle the source code for the workflow?<br />
        <asp:RadioButtonList runat="server" ID="BuildCacheOptions">
            <asp:ListItem Text=" Always compile the workflow from source" Value="AlwaysCompile" />
            <asp:ListItem Text=" Build only if source code is modified and cache the assembly" Value="AlwaysCache" />
        </asp:RadioButtonList>
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
