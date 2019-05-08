<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="EditExistingManifestFiles.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.DefineWorkflow.EditExistingManifestFiles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Define Workflow - Edit existing manifest items
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Define Workflow - Edit existing manifest items
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Listed below are the existing manifest items. You may choose to edit items one by one or act on them as a group. To add or replace all or a set of these items at a time, 
        upload a .ZIP file with the new items. 
    </p>
    <p>
        <asp:LinkButton runat="server" ID="ClearManifestItemsButton" Text="CLEAR ALL" CssClass="buttonStyleLink" OnClick="ClearManifestItemsButton_Click" />&nbsp;
        <asp:LinkButton runat="server" ID="UploadZipButton" Text="UPLOAD ZIP" CssClass="buttonStyleLink" OnClick="UploadZipButton_Click" />
    </p>
    <asp:ListView runat="server" ID="lvExistingManifestItems" InsertItemPosition="None">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Filename</th>
                    <th>Filename extension</th>
                    <th>Item type</th>
                    <th>Build relative path</th>
                    <th>Required for execution</th>
                    <th>Runtime relative path</th>
                    <th colspan="2">Action</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <asp:HiddenField runat="server" ID="hiddenRowManifestItemId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                <td><%# DataBinder.Eval(Container.DataItem, "Filename") %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "FilenameExtension") %></td>
                <td><%# Enum.GetName(typeof(Corkscrew.SDK.workflow.WorkflowManifestItemTypeEnum), DataBinder.Eval(Container.DataItem, "ItemType")) %></td>
                <td><%# DataBinder.Eval(Container.DataItem, "BuildtimeRelativeFolder") %></td>
                <td>
                    <asp:CheckBox runat="server" ID="rowRequiredForExec" Enabled="false" Checked='<%# DataBinder.Eval(Container.DataItem, "RequiredForExecution") %>' Text=" Yes" /></td>
                <td><%# DataBinder.Eval(Container.DataItem, "RuntimeRelativeFolder") %></td>
                <td>
                    <asp:LinkButton runat="server" ID="rowUpdateButton" CssClass="buttonStyleLink" Text="EDIT" CommandName="Edit" /></td>
                <td>
                    <asp:LinkButton runat="server" ID="rowDeleteButton" CssClass="buttonStyleLink" Text="DELETE" CommandName="Delete" />
                </td>
            </tr>
        </ItemTemplate>
        <EditItemTemplate>
            <tr>
                <asp:HiddenField runat="server" ID="hiddenRowManifestItemId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                <td>
                    <asp:TextBox runat="server" ID="rowFilename" MaxLength="255" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "Filename") %>' />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="rowFilenameExtension" MaxLength="255" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "FilenameExtension") %>' />
                </td>
                <td>
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
                </td>
                <td>
                    <asp:TextBox runat="server" ID="rowBuildFolder" MaxLength="255" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "BuildtimeRelativeFolder") %>' />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="rowRequiredForExec" Text=" Yes" Checked='<%# DataBinder.Eval(Container.DataItem, "RequiredForExecution") %>' />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="rowRuntimeFolder" MaxLength="255" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "RuntimeRelativeFolder") %>' />
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="rowUpdateButton" CssClass="buttonStyleLink" Text="UPDATE" CommandName="Update" /></td>
                <td>
                    <asp:LinkButton runat="server" ID="rowDeleteButton" CssClass="buttonStyleLink" Text="CANCEL" CommandName="Cancel" />
                </td>
            </tr>
        </EditItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
