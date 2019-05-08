<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="FinishUpZippedItemUpload.aspx.cs" Inherits="Corkscrew.ControlCenter.workflows.DefineWorkflow.FinishUpZippedItemUpload" %>

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
        You uploaded a .ZIP file in the previous step and required its contents to be used as manifest items. Now, provide the attributes for each file 
        in the zip. NOTE that at this time, these files have not yet been added to the manifest and exists in a temporary location.
    </p>
    <asp:Repeater runat="server" ID="UploadedFiles">
        <HeaderTemplate>
            <table cellpadding="5" border="0" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Directory</th>
                    <th>Filename</th>
                    <th>Filename extension</th>
                    <th>Item type</th>
                    <th>Build relative path</th>
                    <th>Required for execution</th>
                    <th>Runtime relative path</th>
                    <th colspan="2">Action</th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <%# DataBinder.Eval(Container.DataItem, "ParentDirectoryPath").ToString().Replace(workingDirectory.ParentDirectoryPath, "") %>
                </td>
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
                    <asp:TextBox runat="server" ID="rowBuildFolder" MaxLength="255" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "ParentDirectoryPath").ToString().Replace(workingDirectory.ParentDirectoryPath, "") %>' />
                </td>
                <td>
                    <asp:CheckBox runat="server" ID="rowRequiredForExec" Text=" Required" Checked="false" />
                </td>
                <td>
                    <asp:TextBox runat="server" ID="rowRuntimeFolder" MaxLength="255" Columns="30" Text='<%# DataBinder.Eval(Container.DataItem, "ParentDirectoryPath").ToString().Replace(workingDirectory.ParentDirectoryPath, "") %>' />
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="rowUpdateButton" CssClass="buttonStyleLink" Text="UPDATE" CommandName="Update" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                </td>
                <td>
                    <asp:LinkButton runat="server" ID="rowDeleteButton" CssClass="buttonStyleLink" Text="DELETE" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
