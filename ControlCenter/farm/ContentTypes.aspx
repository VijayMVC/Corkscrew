<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ContentTypes.aspx.cs" Inherits="Corkscrew.ControlCenter.farm.ContentTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Manage Content Types
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Manage Content Types
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Content types allow the web browsers and software used by the end user's systems to find out the right information about the content you are sending them 
        through your pages. Configuring or overriding them here, allows you to control that behavior. HOWEVER, please note that these content types are FARM-WIDE settings 
        and will affect ALL the Sites on this Farm!
    </p>

    <asp:ListView runat="server" ID="lvDataView" InsertItemPosition="FirstItem">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Filename Extension</th>
                    <th>Content or MIME Type</th>
                    <th colspan="2">Actions</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
                <tr>
                    <td style="background-color: #ffffff; height: 1px; padding: 0px;" colspan="3"></td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <asp:HiddenField runat="server" ID="hiddenItemFileExtension" Value='<%# DataBinder.Eval(Container.DataItem, "FileExtension") %>' />
            <tr>
                <td style="padding: 10px; vertical-align: top;">
                    <p>
                        <strong><%# DataBinder.Eval(Container.DataItem, "FileExtension") %></strong>
                    </p>
                </td>
                <td style="padding: 10px; vertical-align: top;">
                    <p>
                        <%# DataBinder.Eval(Container.DataItem, "KnownMimeType") %>
                    </p>
                </td>
                <td style="padding: 10px; vertical-align: top;">
                    <asp:LinkButton runat="server" ID="RowEditButton" Text="Edit" CommandName="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileExtension") %>'
                        CssClass="buttonStyleLink" CausesValidation="false" />
                </td>
                <td style="padding: 10px; vertical-align: top;">
                    <asp:LinkButton runat="server" ID="RowDeleteButton" Text="Delete" CssClass="buttonStyleLink" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileExtension") %>'
                        CausesValidation="false"
                        OnClientClick="return confirm('Are you sure you want to delete this content type? This action cannot be undone!!!');" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #ffffff; height: 1px; padding: 0px;" colspan="4"></td>
            </tr>
        </ItemTemplate>
        <InsertItemTemplate>
            <tr>
                <td>
                    <asp:TextBox runat="server" ID="InsertFilenameExtension" MaxLength="24" Columns="30" Text='<%# Bind("FilenameExtension") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="rfvInsertFilenameExtension" ControlToValidate="InsertFilenameExtension" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="InsertItemValidationGroup" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="InsertMIMETypeName" MaxLength="255" Columns="80" AutoCompleteType="Enabled" Text='<%# Bind("DisplayName") %>' />
                    <asp:RequiredFieldValidator runat="server" ID="rfvInsertMIMETypeName" ControlToValidate="InsertMIMETypeName" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="InsertItemValidationGroup" />
                </td>
                <td colspan="2">
                    <asp:LinkButton runat="server" ID="InsertItemCommitButton" Text="Create Content Type" CommandName="Insert" CssClass="buttonStyleLink" CausesValidation="true" ValidationGroup="InsertItemValidationGroup" />
                </td>
            </tr>
            <tr>
                <td style="background-color: #ffffff; height: 1px; padding: 0px;" colspan="4"></td>
            </tr>
        </InsertItemTemplate>
        <EditItemTemplate>
            <tr>
                <td style="background-color: #313131; padding: 20px; border: solid 1px #ffffff; margin: 5px;">
                    <p>
                        <asp:RequiredFieldValidator runat="server" ID="rfvEditFilenameExtension" ControlToValidate="EditFilenameExtension" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="EditItemValidationGroup" />
                        <br />
                        <asp:TextBox runat="server" ID="EditFilenameExtension" MaxLength="24" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "FileExtension") %>' ReadOnly="true" />
                    </p>
                </td>
                <td style="background-color: #313131; padding: 20px; border: solid 1px #ffffff; margin: 5px;">
                    <p>
                        <asp:RequiredFieldValidator runat="server" ID="rfvEditMIMETypeName" ControlToValidate="EditMIMETypeName" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="EditItemValidationGroup" />
                        <br />
                        <asp:TextBox runat="server" ID="EditMIMETypeName" MaxLength="255" Columns="80" AutoCompleteType="Enabled" Text='<%# DataBinder.Eval(Container.DataItem, "KnownMimeType") %>' />
                    </p>
                </td>
                <td style="vertical-align: middle; text-align: center; background-color: #313131; padding: 20px; border: solid 1px #ffffff; margin: 5px;">
                    <asp:LinkButton runat="server" ID="EditItemRowCancelButton" Text="Cancel Update" CssClass="buttonStyleLink" CommandName="Cancel" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileExtension") %>' CausesValidation="false" />
                </td>
                <td style="vertical-align: middle; text-align: center; background-color: #313131; padding: 20px; border: solid 1px #ffffff; margin: 5px;">
                    <asp:LinkButton runat="server" ID="EditItemRowUpdateButton" Text="Update Content Type" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileExtension") %>' CssClass="buttonStyleLink" CausesValidation="true" ValidationGroup="EditItemValidationGroup" />
                </td>
            </tr>
        </EditItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
