<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Corkscrew.ControlCenter.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Corkscrew Control Center</title>
    <link rel="stylesheet" type="text/css" href="/resources/styles/theme-main.css" />
    <link rel="icon" href="/resources/images/favicon.png" />

    <!-- Keep this here so that Jquery engine is initalized correctly -->
    <script type="text/javascript" src="/external/jquery/jquery-1.10.2.js"></script>

    <!-- This is the Adobe TypeKit font script -->
    <script type="text/javascript" src="/external/fvr3lgc.js"></script>
</head>
<body>
    <div class="contentArea" style="padding: 50px;">
        <h3>Login to Corkscrew Central Control Center</h3>
        <div class="contentAreaContent" style="width: auto !important; height: auto !important;">
            <form runat="server" id="LoginForm">
                <p>
                    <strong>Username:</strong><br />
                    <asp:TextBox runat="server" ID="LoginUsername" MaxLength="255" Columns="80" />&nbsp;
                    <asp:RequiredFieldValidator runat="server" ID="rfvLoginUsername" ControlToValidate="LoginUsername" ErrorMessage=" * " />
                </p>
                <p>
                    <strong>Password:</strong><br />
                    <asp:TextBox runat="server" ID="LoginPassword" MaxLength="255" Columns="80" TextMode="Password" />
                </p>
                <p>
                    <asp:CheckBox runat="server" ID="LoginRememberMe" Text=" Remember me" Checked="false" />
                </p>
                <p>
                    <asp:Literal runat="server" ID="LoginErrorMessage" />
                </p>
                <p>&nbsp;</p>
                <div class="alignleft">
                    <asp:Button runat="server" ID="LoginButton" Text="LOGIN" OnClick="LoginButton_Click" />
                </div>
                <p><strong>Forgot your userid or password?</strong> You know how to change it!</p>
                <p>&nbsp;</p>
            </form>
        </div>
    </div>
</body>
</html>
