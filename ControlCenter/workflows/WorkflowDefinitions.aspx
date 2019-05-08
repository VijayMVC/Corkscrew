<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="WorkflowDefinitions.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.WorkflowDefinitions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Workflow Definitions
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Manage Workflow Definitions
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Use this page to add and manage workflow definitions. 
    </p>
    <p>
        <asp:LinkButton runat="server" ID="CreateWorkflowDefinitionButton" Text="DEFINE NEW WORKFLOW" CssClass="buttonStyleLink" OnClick="CreateWorkflowDefinitionButton_Click" />
    </p>
    <asp:ListView runat="server" ID="lvWorkflowDefinitions" InsertItemPosition="None" DataKeyNames="Id">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Name &amp; Description</th>
                    <th>Associations</th>
                    <th>Enabled</th>
                    <th>Actions</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <asp:HiddenField runat="server" ID="hiddenRowItemId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
            <tr>
                <td><span style="font-size: larger"><%# DataBinder.Eval(Container.DataItem, "Name") %></span><br /><br />
                    <%# DataBinder.Eval(Container.DataItem, "Description") %><br /><br />
                    <a class="buttonStyleLink" href='/workflows/DefineWorkflow/Step1.aspx?DefinitionId=<%# DataBinder.Eval(Container.DataItem, "Id") %>'>METADATA</a> 
                    <a class="buttonStyleLink" href='/workflows/DefineWorkflow/Step2.aspx?DefinitionId=<%# DataBinder.Eval(Container.DataItem, "Id") %>'>MANIFEST &amp; BUILD</a> 
                    <a class="buttonStyleLink" href='/workflows/DefineWorkflow/Step3.aspx?DefinitionId=<%# DataBinder.Eval(Container.DataItem, "Id") %>'>UPLOAD FILE(S)</a> 
                    <a class="buttonStyleLink" href='/workflows/DefineWorkflow/EditExistingManifestFiles.aspx?DefinitionId=<%# DataBinder.Eval(Container.DataItem, "Id") %>'>MANIFEST FILES</a> 
                </td>
                <td style="text-align: center;">
                    <%# DataBinder.Eval(Container.DataItem, "Associations.Count") %>
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="cbRowItemEnabled" Text=" Enabled" Checked='<%# DataBinder.Eval(Container.DataItem, "IsEnabled") %>' AutoPostBack="true" OnCheckedChanged="cbRowItemEnabled_CheckedChanged" /></td>
                <td>
                    <asp:LinkButton runat="server" CssClass="buttonStyleLink"  ID="RowEditButton" Text="EDIT" CommandName="Edit" />
                    <asp:LinkButton runat="server" CssClass="buttonStyleLink"  ID="RowDeleteButton" Text="DELETE" CommandName="Delete" />
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
