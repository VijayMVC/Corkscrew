<%@ Page Title="" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="AddEditFile.aspx.cs" Inherits="Corkscrew.ControlCenter.filesystem.AddEditFile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PageTitle" runat="server">
    Add or Modify File
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentAreaPageHeading" runat="server">
    Add or Modify File
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentAreaContent" runat="server">
    <p>
        To create a file, enter its name and set its attributes.
    </p>
    <p>
        Parent directory:
        <br />
        <asp:TextBox runat="server" ID="ParentDirectoryPath" Columns="80" ReadOnly="true" />
    </p>
    <p>
        Corkscrew Uri to this directory:
        <br />
        <asp:TextBox runat="server" ID="CorkscrewUri" Columns="80" ReadOnly="true" />
    </p>
    <p>
        <asp:RadioButton runat="server" ID="OptionUseNamedFileMethod" Text=" Create file manually" GroupName="OptionCreateFileMethod" AutoPostBack="true" OnCheckedChanged="OptionUseNamedFileMethod_CheckedChanged" />
    </p>
    <asp:Panel runat="server" ID="panelNamedMethod" Visible="false">
        <blockquote>
            <p>
                Filename:
        <br />
                <asp:TextBox runat="server" ID="Filename" Columns="80" MaxLength="255" />
            </p>
            <p>
                Filename extension:
        <br />
                <asp:TextBox runat="server" ID="FilenameExtension" Columns="80" MaxLength="255" />
            </p>
        </blockquote>
    </asp:Panel>
    <p>
        <asp:RadioButton runat="server" ID="OptionUseUploadFileMethod" Text=" Create from uploaded file" GroupName="OptionCreateFileMethod" AutoPostBack="true" OnCheckedChanged="OptionUseUploadFileMethod_CheckedChanged" />
    </p>
    <asp:Panel runat="server" ID="panelUploadMethod" Visible="true">
        <blockquote>
            <p>
                Upload file:
                <br />
                <asp:FileUpload runat="server" ID="UploadedFile" AllowMultiple="false" />
            </p>
        </blockquote>
    </asp:Panel>
    <p>
        Set attributes:
        <br />
        <asp:CheckBoxList runat="server" ID="Attributes">
            <asp:ListItem Text=" Readonly" Value="R" />
            <asp:ListItem Text=" Hidden" Value="H" />
        </asp:CheckBoxList>
    </p>
    <asp:Panel runat="server" ID="FileContentEditor">
        <asp:TextBox runat="server" ID="FileContent" TextMode="MultiLine" Rows="25" Width="95%" />
    </asp:Panel>
    <p>
        <asp:Button runat="server" ID="CancelButton" Text="CANCEL" OnClick="CancelButton_Click" />&nbsp;
        <asp:Button runat="server" ID="CreateButton" UseSubmitBehavior="true" Text="CREATE" OnClick="CreateButton_Click" />
    </p>
    <p>
        <asp:Label runat="server" ID="ErrorMessage" ForeColor="Red" BackColor="Yellow" Text="" />
    </p>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="scripts" runat="server">
    <% if (editorEnabled) { %>
    <script src="//cdn.tinymce.com/4/tinymce.min.js"></script>
    <script type="text/javascript">
        tinymce.init({
            selector: 'textarea',
            height: 500,
            theme: 'modern',
            plugins: [
                'advlist autolink lists link image charmap print preview hr anchor pagebreak',
                'searchreplace wordcount visualblocks visualchars code fullscreen',
                'insertdatetime media nonbreaking save table contextmenu directionality',
                'emoticons template paste textcolor colorpicker textpattern imagetools codesample toc'
            ],
            toolbar1: 'undo redo | insert | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
            toolbar2: 'print preview media | forecolor backcolor emoticons | codesample | code',
            image_advtab: true,
            content_css: [
                '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
                '//www.tinymce.com/css/codepen.min.css'
            ]
        });
    </script>
    <% } %>
</asp:Content>
