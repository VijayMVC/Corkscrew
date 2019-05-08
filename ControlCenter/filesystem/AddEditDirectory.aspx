<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AddEditDirectory.aspx.cs" Inherits="Corkscrew.ControlCenter.filesystem.AddEditDirectory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Add or Modify Directory
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Add or Modify Directory
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        To create a directory, enter its name and set its attributes.
    </p>
    <p>
        Parent directory: <br />
        <asp:TextBox runat="server" ID="ParentDirectoryPath" Columns="80" ReadOnly="true" />
    </p>
    <p>
        Corkscrew Uri to this directory: <br />
        <asp:TextBox runat="server" ID="CorkscrewUri" Columns="80" ReadOnly="true" />
    </p>
    <p>
        Directory name: <br />
        <asp:TextBox runat="server" ID="Filename" Columns="80" MaxLength="255" />
    </p>
    <p>
        Set attributes: <br />
        <asp:CheckBoxList runat="server" ID="Attributes">
            <asp:ListItem Text=" Readonly" Value="R" />
            <asp:ListItem Text=" Hidden" Value="H" />
        </asp:CheckBoxList>
    </p>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />&nbsp;
        <asp:Button runat="server" ID="CreateButton" UseSubmitBehavior="true" Text="CREATE" OnClick="CreateButton_Click" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
</asp:Content>
