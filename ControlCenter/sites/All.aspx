<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="All.aspx.cs" Inherits="Corkscrew.ControlCenter.sites.All" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    All Sites
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    All Sites
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>&nbsp;</p>
    <p>
        Sites in Corkscrew are what hosts content for your applications and users. Create and manage sites using this page.
    </p>
    <p>
        <asp:HyperLink runat="server" ID="hlCreateSiteHyperLink" Text="CREATE SITE" NavigateUrl="/sites/AddEdit.aspx" CssClass="buttonStyleLink" />
    </p>

    <asp:ListView runat="server" ID="lvDataView" InsertItemPosition="None">
        <LayoutTemplate>
            <table cellpadding="5" border="0" runat="server" id="htmlDataViewTable" style="width: 100%; height: 100%; padding: 5px;">
                <tr>
                    <th>Site Information</th>
                </tr>
                <tr runat="server" id="itemPlaceholder" />
                <tr>
                    <td style="background-color: #ffffff; height: 1px; padding: 0px;"></td>
                </tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    <table>
                        <tr>
                            <td style="padding: 10px; vertical-align: top;">
                                <p>
                                    <span style="font-weight: bold; font-variant: small-caps; font-size: 150%;"><%# DataBinder.Eval(Container.DataItem, "Name") %></span>
                                    <br />
                                    (Id: <%# Corkscrew.SDK.tools.Utility.SafeConvertToGuid(DataBinder.Eval(Container.DataItem, "Id")).ToString("d") %>)
                                </p>
                                <p>
                                    <%# DataBinder.Eval(Container.DataItem, "Description") %>
                                </p>
                            </td>
                            <td style="padding: 10px; vertical-align: top;">
                                <asp:Repeater runat="server" ID="rptDNSNames" DataSource='<%# DataBinder.Eval(Container.DataItem, "DNSNames") %>' Visible='<%# (((((IEnumerable<string>)DataBinder.Eval(Container.DataItem, "DNSNames")).Count() > 0) && (! ((Corkscrew.SDK.objects.CSSite)Container.DataItem).IsConfigSite)) ? true : false) %>'>
                                    <HeaderTemplate>
                                        <strong>DNS names mapped:</strong><br />
                                        <ul>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li><a href='http://<%# Container.DataItem.ToString() %>' target="_blank"><%# Container.DataItem.ToString() %></a></li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>
                                <p>
                                    Site database server: <%# DataBinder.Eval(Container.DataItem, "ContentDatabaseServerName") %>
                                    <br />
                                    Site database name: <%# DataBinder.Eval(Container.DataItem, "ContentDatabaseName") %>
                                    <br />
                                    Created by <%# ((Corkscrew.SDK.security.CSUser)DataBinder.Eval(Container.DataItem, "CreatedBy")).DisplayName %> on <%# ((DateTime)DataBinder.Eval(Container.DataItem, "Created")).ToString("MMM dd, yyyy HH:mm:ss") %>
                                    &nbsp;|&nbsp;
                                    Last Modified by <%# ((Corkscrew.SDK.security.CSUser)DataBinder.Eval(Container.DataItem, "ModifiedBy")).DisplayName %> on <%# ((DateTime)DataBinder.Eval(Container.DataItem, "Modified")).ToString("MMM dd, yyyy HH:mm:ss") %>
                                    <br />
                                    Site Quota: <%# DataBinder.Eval(Container.DataItem, "QuotaBytes") %>
                                    &nbsp;|&nbsp;
                                    Currently used: <%# DataBinder.Eval(Container.DataItem, "RootFolder.FolderSizeBytes") %> bytes.
                                </p>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding: 10px; vertical-align: top;">
                                <p>
                                    <a class="buttonStyleLink" href='<%# (IsFarmAdministrator ? "/filesystem/Explorer.aspx?SiteId=" + DataBinder.Eval( Container.DataItem, "Id" ) + "&Path=" + Server.UrlEncode("/") : "#") %>'>File System</a>&nbsp;
                                    <a class="buttonStyleLink" href='<%# (IsFarmAdministrator ? "/sites/SiteAdministrators.aspx?Id=" + DataBinder.Eval( Container.DataItem, "Id" ) : "#") %>'>Site Administrators</a>&nbsp;
                                    <a class="buttonStyleLink" style='<%# ((((Corkscrew.SDK.objects.CSSite)Container.DataItem).IsConfigSite) ? "display:none;" : "") %>' href='<%# (IsFarmAdministrator ? "/workflows/AssociateWorkflow.aspx?Scope=" + (Corkscrew.SDK.tools.Utility.SafeConvertToGuid(DataBinder.Eval( Container.DataItem, "Id")).Equals(Guid.Empty) ? "Farm" : "Site") + "&ObjectId=" + DataBinder.Eval( Container.DataItem, "Id" ) + "&ReturnUrl=" + Request.Url.ToString() : "#") %>'>Associate Workflow</a>&nbsp;
                                    <a class="buttonStyleLink" style='<%# ((((Corkscrew.SDK.objects.CSSite)Container.DataItem).IsConfigSite) ? "display:none;" : "") %>' href='<%# (IsFarmAdministrator ? "/sites/AddEdit.aspx?Id=" + DataBinder.Eval( Container.DataItem, "Id" ) : "#") %>'>Edit Site</a>&nbsp;
                                    <a class="buttonStyleLink" style='<%# ((((Corkscrew.SDK.objects.CSSite)Container.DataItem).IsConfigSite) ? "display:none;" : "") %>' href='<%# (IsFarmAdministrator ? "/sites/Delete.aspx?Id=" + DataBinder.Eval( Container.DataItem, "Id" ) : "#") %>'>Delete Site</a>
                                </p>
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField runat="server" ID="hiddenSiteId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                </td>
            </tr>
            <tr>
                <td style="background-color: #ffffff; height: 1px; padding: 0px;" colspan="2"></td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
