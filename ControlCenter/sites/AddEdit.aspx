<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AddEdit.aspx.cs" Inherits="Corkscrew.ControlCenter.sites.AddEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Add or Modify Site
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Add or Modify Site
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        Create a site or modify an existing site. Enter the information below and click the button. All the fields are mandatory to fill in.
    </p>
    <p>
        <strong>Enter a unique name for the website</strong>
        &nbsp;<asp:RequiredFieldValidator runat="server" ID="rfvSiteName" ControlToValidate="SiteName" ForeColor="Yellow" ErrorMessage=" *" ValidationGroup="ItemValidationGroup" />
        <br />
        <asp:TextBox runat="server" ID="SiteName" MaxLength="255" Columns="80" />
    </p>
    <p>
        <strong>Enter a description for this website (optional)</strong><br />
        <em>This will be displayed in site directories and search results.</em><br />
        <asp:TextBox runat="server" ID="Description" MaxLength="512" Columns="80" Rows="4" TextMode="MultiLine" />
    </p>
    <p>
        <strong>Name of the content database for site</strong><br />
        <em>Cannot be changed in this version of Corkscrew.</em><br />
        <asp:TextBox runat="server" ID="DatabaseName" MaxLength="255" Columns="80" ReadOnly="true" Text="Corkscrew_ConfigDB" />
    </p>
    <p>
        <strong>Enter the quota for the new site (optional, set to 0 to disable quota)</strong><br />
        <em>This value is in bytes. Ensure you set it carefully!</em><br />
        <asp:TextBox runat="server" ID="SiteQuotaBytes" MaxLength="24" Columns="80" />
    </p>
    <p>
        <asp:CheckBox runat="server" ID="SiteBindToDnsOption" Text=" Bind site to DNS" Checked="false" OnCheckedChanged="SiteBindToDnsOption_CheckedChanged" AutoPostBack="true" /><br />
        <blockquote>
            <p>
                <strong>Enter the DNS hostnames to bind to</strong>
                <br />
                <em>Enter one hostname per line (or seperate them using a comma, semi-colon or a space. If you are using IIS, Corkscrew also will automatically add a binding for this hostname</em><br />
                <asp:TextBox runat="server" ID="DNSHostNames" MaxLength="4000" Columns="80" Rows="5" TextMode="MultiLine" Enabled="false" />
            </p>
        </blockquote>
    </p>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />
        <asp:Button runat="server" ID="AddEditButton" UseSubmitBehavior="true" Text="SUBMIT" 
            OnClientClick="return confirm('If we are unable to deploy the Site database to the specified server with the specified name, the Site will use the ConfigDB as a fallback. Confirm if this is OK ?');"
            OnClick="AddEditButton_Click" />
    </p>
    <p>
        NOTE: If we are unable to deploy the Site database to the specified server with the specified name, the Site will use the ConfigDB as a fallback.
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
