<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Configuration.aspx.cs" Inherits="Corkscrew.ControlCenter.farm.Configuration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Farm Configuration Settings
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Farm Configuration Settings
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Set up, modify or remove configuration settings for this Corkscrew Farm. Note that all changes are immediate and will be 
        visible across your entire Corkscrew Farm immediately. However, any code that has already retrieved the setting and is currently 
        processing will not be affected.
    </p>
    <p>
        NOTE: 
    </p>
    <ol>
        <li>Please be careful adding or modifying settings through this screen as there is no "undo" option. </li>
        <li>Any new configuration added from this page is added to the ConfigDB.</li>
    </ol>
    <asp:ListView runat="server" ID="lvConfigurationSettings" InsertItemPosition="FirstItem">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Name</th>
                    <th>Value</th>
                    <th>Actions</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td><%# DataBinder.Eval(Container.DataItem, "Key") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "Value") %></td>
                <td>
                    <asp:LinkButton runat="server" ID="RowEditButton" Text="EDIT" CssClass="buttonStyleLink" CommandName="Edit" CausesValidation="false" />
                    <asp:LinkButton runat="server" ID="RowDeleteButton" Text="DELETE" CssClass="buttonStyleLink" CommandName="Delete" CausesValidation="false" />
                </td>
                <asp:HiddenField runat="server" ID="hiddenRowKeyName" Value='<%# DataBinder.Eval(Container.DataItem, "Key") %>' />
            </tr>
        </ItemTemplate>
        <InsertItemTemplate>
            <tr>
                <td><asp:TextBox runat="server" ID="InsertItemKey" MaxLength="255" Columns="40" Text='<%# Bind("Key") %>' ValidationGroup="InsertItemValidationGroup" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvInsertItemKey" ControlToValidate="InsertItemKey" ValidationGroup="InsertItemValidationGroup" ErrorMessage=" *" ForeColor="Yellow" />
                </td>
                <td><asp:TextBox runat="server" ID="InsertItemValue" MaxLength="255" Columns="40" Text='<%# Bind("Value") %>' ValidationGroup="InsertItemValidationGroup" /></td>
                <td>
                    <asp:LinkButton runat="server" ID="RowInsertButton" Text="ADD" CssClass="buttonStyleLink" CommandName="Insert" ValidationGroup="InsertItemValidationGroup" />
                </td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr>
                <td><asp:TextBox runat="server" ID="EditItemKey" MaxLength="255" Columns="40" Text='<%# Bind("Key") %>' ValidationGroup="EditItemValidationGroup" />
                    <asp:RequiredFieldValidator runat="server" ID="rfvEditItemKey" ControlToValidate="EditItemKey" ValidationGroup="EditItemValidationGroup" ErrorMessage=" *" ForeColor="Yellow" />
                </td>
                <td><asp:TextBox runat="server" ID="EditItemValue" MaxLength="255" Columns="40" Text='<%# Bind("Value") %>' ValidationGroup="EditItemValidationGroup" /></td>
                <td>
                    <asp:LinkButton runat="server" ID="RowUpdateButton" Text="UPDATE" CssClass="buttonStyleLink" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Key") %>' ValidationGroup="InsertItemValidationGroup"  />
                    <asp:LinkButton runat="server" ID="RowUpdateCancelButton" Text="CANCEL" CssClass="buttonStyleLink" CommandName="Cancel" CausesValidation="false" />
                </td>
            </tr>
        </EditItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
