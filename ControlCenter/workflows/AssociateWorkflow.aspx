<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AssociateWorkflow.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.AssociateWorkflow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Associate workflow
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style>
        input[type="checkbox"][disabled="disabled"], input[type="checkbox"][disabled="disabled"] + label {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Associate workflow
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Enter information or make selections below to associate a workflow to a target object. If you do not see an option to create a new association it means that all available workflows definitions have been 
        associated to this object.
    </p>

    <asp:ListView runat="server" ID="lvAssociations" InsertItemPosition="FirstItem">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Association name</th>
                    <th>Workflow definition name</th>
                    <th>Workflow triggers</th>
                    <th>Is enabled</th>
                    <th>Instantiation events</th>
                    <th colspan="2">Actions</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <InsertItemTemplate>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="InsertAssociationName" Text="" MaxLength="255" /> 
                    <asp:RequiredFieldValidator runat="server" ID="rfvInsertAssociationName" ControlToValidate="InsertAssociationName" ErrorMessage=" *" ForeColor="Yellow" ValidationGroup="InsertValidationGroup" />
                </td>
                <td><asp:DropDownList runat="server" ID="InsertWorkflowNamesList" /></td>
                <td>
                    <asp:CheckBox runat="server" ID="InsertOnStartCreate" Text=" On Create" Checked="true" /><br />
                    <asp:CheckBox runat="server" ID="InsertOnStartModify" Text=" On Modify" Checked="true" />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="InsertIsEnabled" Text=" Yes" Checked="true" />
                </td>
                <td>
                    <asp:CheckBoxList runat="server" ID="InsertEventsList" RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Table">
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
                </td>
                <td colspan="2">
                    <asp:LinkButton runat="server" CssClass="buttonStyleLink" ID="InsertSubmitButton" Text="ASSOCIATE" CommandName="Insert" ValidationGroup="InsertValidationGroup" />
                </td>
            </tr>
        </InsertItemTemplate>
        <ItemTemplate>
            <tr>
                <asp:HiddenField runat="server" ID="hiddenAssociationId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                <td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "WorkflowDefinition.Name") %></td>
                <td>
                    <%# (Corkscrew.SDK.tools.Utility.SafeConvertToBool(DataBinder.Eval(Container.DataItem, "StartOnCreate")) ? "On Create" : "") %>
                    <%# ((Corkscrew.SDK.tools.Utility.SafeConvertToBool(DataBinder.Eval(Container.DataItem, "StartOnCreate")) && Corkscrew.SDK.tools.Utility.SafeConvertToBool(DataBinder.Eval(Container.DataItem, "StartOnModify"))) ? "," : "") %>
                    <%# (Corkscrew.SDK.tools.Utility.SafeConvertToBool(DataBinder.Eval(Container.DataItem, "StartOnModify")) ? "On Modify" : "") %>
                </td>
                <td>
                    <%# (Corkscrew.SDK.tools.Utility.SafeConvertToBool(DataBinder.Eval(Container.DataItem, "IsEnabled")) ? "Enabled" : "Disabled") %>
                </td>
                <td>
                    <asp:Literal runat="server" ID="rowItemEventsList" />
                </td>
                <td>
                    <asp:LinkButton runat="server" CssClass="buttonStyleLink" ID="rowEditButton" Text="EDIT" CommandName="Edit" CausesValidation="false" />
                </td>
                <td><asp:LinkButton runat="server" CssClass="buttonStyleLink" ID="rowDeleteButton" Text="DELETE" CommandName="Delete" CausesValidation="false" 
                    OnClientClick="return confirm('Are you sure you wish to delete this workflow association? This will immediately terminate all running workflow instances for this association, potentially leaving your data in an unknown state!');" /></td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <asp:HiddenField runat="server" ID="hiddenAssociationId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                <td>
                    <asp:TextBox runat="server" ID="EditAssociationName" Text='<%# DataBinder.Eval(Container.DataItem, "Name") %>' MaxLength="255" /> 
                    <asp:RequiredFieldValidator runat="server" ID="rfvEditAssociationName" ControlToValidate="EditAssociationName" ErrorMessage=" *" ForeColor="Yellow" ValidationGroup="EditValidationGroup" />
                </td>
                <td><%# DataBinder.Eval(Container.DataItem, "WorkflowDefinition.Name") %></td>
                <td>
                    <asp:CheckBox runat="server" ID="EditOnStartCreate" Text=" On Create" Checked='<%# DataBinder.Eval(Container.DataItem, "StartOnCreate") %>' /><br />
                    <asp:CheckBox runat="server" ID="EditOnStartModify" Text=" On Modify" Checked='<%# DataBinder.Eval(Container.DataItem, "StartOnModify") %>' />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="EditIsEnabled" Text=" Yes" Checked="true" />
                </td>
                <td>
                    <asp:CheckBoxList runat="server" ID="EditEventsList" RepeatDirection="Horizontal" RepeatColumns="3" RepeatLayout="Table">
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
                </td>
                <td>
                    <asp:LinkButton runat="server" CssClass="buttonStyleLink" ID="EditCancelButton" Text="CANCEL" CommandName="Cancel" CausesValidation="false" />
                </td>
                <td>
                    <asp:LinkButton runat="server" CssClass="buttonStyleLink" ID="EditUpdateButton" Text="UPDATE" CommandName="Update" ValidationGroup="EditValidationGroup" />
                </td>
            </tr>
        </EditItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
