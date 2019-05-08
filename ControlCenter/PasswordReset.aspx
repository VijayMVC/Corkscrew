<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PasswordReset.aspx.cs" Inherits="Corkscrew.ControlCenter.PasswordReset" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password - Corkscrew Control Center</title>
    <link rel="stylesheet" type="text/css" href="/resources/styles/theme-main.css" />
    <link rel="icon" href="/resources/images/favicon.png" />

    <!-- Keep this here so that Jquery engine is initalized correctly -->
    <script type="text/javascript" src="/external/jquery/jquery-1.10.2.js"></script>

    <!-- This is the Adobe TypeKit font script -->
    <script type="text/javascript" src="/external/fvr3lgc.js"></script>
</head>
<body>
    <div class="contentArea" style="padding: 50px;">
        <h3>Change your Corkscrew Account Password</h3>
        <div class="contentAreaContent" style="width: auto !important; height: auto !important;">
            <form runat="server" id="LoginForm">
                <p>
                    <strong>Username:</strong> <asp:RequiredFieldValidator runat="server" ID="rfvLoginUsername" ControlToValidate="LoginUsername" ErrorMessage=" * Required" /><br />
                    <asp:TextBox runat="server" ID="LoginUsername" MaxLength="255" Columns="80" />&nbsp;
                </p>
                <p>
                    <strong>Current password:</strong> <asp:RequiredFieldValidator runat="server" ID="rfvCurrentPassword" ControlToValidate="CurrentPassword" ErrorMessage=" * Required" /><br />
                    <asp:TextBox runat="server" ID="CurrentPassword" MaxLength="255" Columns="80" TextMode="Password" />
                </p>
                <p>
                    <strong>New password:</strong> <asp:RequiredFieldValidator runat="server" ID="rfvNewPassword" ControlToValidate="NewPassword" ErrorMessage=" * Required" /><br />
                    <asp:TextBox runat="server" ID="NewPassword" MaxLength="255" Columns="80" TextMode="Password" />
                </p>
                <p>
                    <strong>Confirm new password:</strong> <asp:RequiredFieldValidator runat="server" ID="rfvConfirmNewPassword" ControlToValidate="ConfirmNewPassword" ErrorMessage=" * Required" /> 
                    <asp:CompareValidator runat="server" ID="cvConfirmNewPassword" ControlToValidate="ConfirmNewPassword" ControlToCompare="NewPassword" ErrorMessage=" Passwords do not match" ValueToCompare="Text" /><br />
                    <asp:TextBox runat="server" ID="ConfirmNewPassword" MaxLength="255" Columns="80" TextMode="Password" />
                </p>

                <p>
                    <asp:Literal runat="server" ID="LoginErrorMessage" />
                </p>
                <p>&nbsp;</p>
                <div class="alignleft">
                    <asp:Button runat="server" ID="ChangePasswordButton" Text="CHANGE PASSWORD" OnClick="ChangePasswordButton_Click" />
                </div>
                <p>&nbsp;</p>
            </form>
        </div>
    </div>
</body>
</html>
