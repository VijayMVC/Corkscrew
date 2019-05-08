<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Explorer.aspx.cs" Inherits="Corkscrew.ControlCenter.filesystem.Explorer" %>
<%@ Import Namespace="Corkscrew.SDK.objects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Corkscrew Filesystem Explorer
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Corkscrew Filesystem Explorer
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Current Folder: <%= FolderNavigationBreadCrumb %>
    </p>
    <p>&nbsp;</p>
    <span style="font-weight: bold; color: yellow;"><%
        if ((!SiteId.Equals(Guid.Empty)) && (UserIsGlobalAdministrator))
        {
    %>As a Global Administrator, you may also manage the <a href="Explorer.aspx?SiteId=<%= Guid.Empty %>&Path=<%= Server.UrlEncode("/") %>">ConfigDB FileSystem</a><%
        }
        else if (SiteId.Equals(Guid.Empty))
        {
    %>WARNING! You are managing the Config DB File System! All changes made here will affect ALL websites on this Farm!<%      
        }
    %></span>
    <p>&nbsp;</p>
    <p>
        <a href='/filesystem/AddEditFile.aspx?SiteId=<%= SiteId %>&Path=<%= Server.UrlEncode(BrowserPath) %>' class="buttonStyleLink">CREATE FILE</a>&nbsp;
        <a href='/filesystem/AddEditDirectory.aspx?SiteId=<%= SiteId %>&Path=<%= Server.UrlEncode(BrowserPath) %>' class="buttonStyleLink">CREATE DIRECTORY</a>&nbsp;
        <a href='/filesystem/CreateFromZip.aspx?SiteId=<%= SiteId %>&Path=<%= Server.UrlEncode(BrowserPath) %>' class="buttonStyleLink">CREATE FROM ZIP</a>&nbsp;
        <a href='<%= "/workflows/AssociateWorkflow.aspx?Scope=Directory&ParentSiteId=" + SiteId.ToString("d") + "&ObjectId=" + currentFolder.Id.ToString("d") + "&ReturnUrl=" + Request.Url.ToString() %>' class="buttonStyleLink">Associate Workflow</a>&nbsp;
    </p>
    <asp:ListView runat="server" ID="lvFileSystemView" InsertItemPosition="None">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlFileSystemTable" style="width: auto; height: auto; padding: 5px;">
                <tr runat="server" id="itemPlaceholder" />
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td style="padding: 5px; vertical-align: top; width: 64px;">
                     <asp:Image runat="server" ID="imageItemIcon" AlternateText='<%# ((((bool)DataBinder.Eval( Container.DataItem, "IsFolder" )) == true) ? "Folder" : "File") %>'
                        ImageAlign="TextTop" Width="24" Height="24"
                        ImageUrl='<%# ((((bool)DataBinder.Eval( Container.DataItem, "IsFolder" )) == true) ? "/resources/images/icon_folder.png" : "GetFileExtensionIcon.ashx?o=GetByExtension&extension=" + DataBinder.Eval( Container.DataItem, "FilenameExtension") ) %>' />
                    &nbsp;
                    <% if (DownloadsAreAllowed){ %>
                    <a style="outline:none;border:0px;" target="_blank" href='Download.aspx?SiteId=<%= Request.QueryString["SiteId"] %>&Path=<%# DataBinder.Eval(Container.DataItem, "FullPath") %>'>
                        <img src="/resources/images/icon_download.png" alt="Download" width="24" height="24" style="vertical-align: middle;" />
                    </a>
                    <% } %>
                </td>
                <td style="padding: 5px; vertical-align: top; width: auto;">
                    <%# (
                            (((bool)DataBinder.Eval(Container.DataItem, "IsFolder")) == true) 
                            ? "<a href=\"Explorer.aspx?SiteId=" + Request.QueryString["SiteId"] + "&Path=" + Server.UrlEncode(DataBinder.Eval(Container.DataItem, "FullPath").ToString()) + "\">" + DataBinder.Eval(Container.DataItem, "FilenameWithExtension") + "</a>" 
                            : DataBinder.Eval(Container.DataItem, "FilenameWithExtension") 
                        ) 
                    %>
                </td>
                <td style="padding: 5px; vertical-align: top; width: 255px; text-align: right;">
                    <asp:Label runat="server" ID="labelItemSize" 
                        Text='<%# ((((bool)DataBinder.Eval( Container.DataItem, "IsFolder" )) == true) ? "" : DataBinder.Eval( (new CSFileSystemEntryFile(Container.DataItem as CSFileSystemEntry)), "SizeHuman")) %>' />
                </td>
                <td style="padding: 5px; vertical-align: top; width: 255px; text-align: center;">
                    <asp:Label runat="server" ID="labelLastModified" Text='<%# ((DateTime)DataBinder.Eval( Container.DataItem, "Modified")).ToString("MMM dd, yyyy hh:mm tt") %>' />
                </td>
                <td style="padding: 5px; vertical-align: top; width: 255px; text-align: center;">
                    <a class="buttonStyleLink" href='/filesystem/AddEdit<%# (Corkscrew.SDK.tools.Utility.SafeConvertToBool(DataBinder.Eval( Container.DataItem, "IsFolder" )) ? "Directory" : "File") %>.aspx?SiteId=<%# SiteId %>&Path=<%# DataBinder.Eval(Container.DataItem, "ParentDirectoryPath") %>&ItemId=<%# DataBinder.Eval( Container.DataItem, "Id" ) %>'>MODIFY</a>&nbsp;
                    <a class="buttonStyleLink" href='/filesystem/Delete.aspx?SiteId=<%# SiteId %>&ItemId=<%# DataBinder.Eval( Container.DataItem, "Id" ) %>'>DELETE</a>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
