<%@ Page Title="Create the first Farm Administrator" Language="C#" AutoEventWireup="true" CodeBehind="CreateFirstUser.aspx.cs" Inherits="Corkscrew.ControlCenter.InitialSetupWizard.CreateFirstUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Corkscrew Manager</title>
    <link rel="stylesheet" type="text/css" href="/resources/styles/theme-main.css" />
    <link rel="icon" href="/resources/images/favicon.png" />

    <script type="text/javascript" src="/external/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="//use.typekit.net/fvr3lgc.js"></script>
</head>
<body>
    <div class="topBanner dropShadowBox">
        <img id="siteActionsMenuGear" class="siteActionsMenu" src="/resources/images/gear_dark.png" onclick="toggleSiteActions('siteActionsMenuDropDownMenu', 'siteActionsMenuGear'); return false;" />
        <span class="topBannerText dropShadowText">Aquarius Corkscrew - Central Control Center</span>
    </div>
    <div class="contentArea" style="padding: 50px;">
        <h3>Create a Login for yourself</h3>
        <div class="contentAreaContent" style="width: auto !important; height: auto !important;">
            <p>
                Hmmm... Looks like your farm has been freshly deployed without using one of the Corkscrew deployment tools. There is no Farm Administrator user account created yet. 
            Without this account, you will not be able to add users or create sites or perform any actions on this Corkscrew deployment.
            </p>
            <form runat="server" id="CreateFirstUserForm">
                <p>
                    <strong>Choose a username for yourself:</strong>
                    <asp:RequiredFieldValidator runat="server" ID="rfvUsersUserName" ControlToValidate="UsersUsername" ForeColor="Yellow" ErrorMessage=" *" />
                    <br />
                    <asp:TextBox runat="server" ID="UsersUsername" MaxLength="255" Columns="80" />
                </p>
                <p>
                    <strong>Enter (a part of) your real name:</strong>
                    <asp:RequiredFieldValidator runat="server" ID="rfvUsersDisplayName" ControlToValidate="UsersDisplayname" ForeColor="Yellow" ErrorMessage=" *" />
                    <br />
                    <em>This is how we will call you throughout the Corkscrew system and in any generated emails</em><br />
                    <asp:TextBox runat="server" ID="UsersDisplayname" MaxLength="255" Columns="80" AutoCompleteType="DisplayName" />
                </p>
                <p>
                    <strong>Enter your email address:</strong>
                    <asp:RequiredFieldValidator runat="server" ID="rfvUsersEmailAddress" ControlToValidate="UsersEmailAddress" ForeColor="Yellow" ErrorMessage=" *" />
                    <br />
                    <em>This is where you will receive notifications, workflow email and other notifications from Corkscrew</em><br />
                    <asp:TextBox runat="server" ID="UsersEmailAddress" MaxLength="255" Columns="80" AutoCompleteType="Email" TextMode="Email" />
                </p>
                <p>
                    <strong>Choose a good password:</strong>
                    <asp:RequiredFieldValidator runat="server" ID="rfvUsersPassword" ControlToValidate="UsersPassword" ForeColor="Yellow" ErrorMessage=" *" />
                    <br />
                    <em>Choosing a strong password is important because anyone who can guess your username or password will be able to control everything in this installation!</em>
                    <br />
                    <asp:TextBox runat="server" ID="UsersPassword" MaxLength="255" Columns="80" TextMode="Password" />
                </p>
                <p>&nbsp;</p>
                <p>After your user account has been created, you will be redirect to the Login page to login to Corkscrew Central Control Center.</p>
                <p>&nbsp;</p>
                <div class="alignleft">
                    <asp:Button runat="server" ID="UsersAddNewUserButton" Text="CREATE MY USER ACCOUNT" OnClick="UsersAddNewUserButton_Click" />
                </div>
                <p>&nbsp;</p>
            </form>
        </div>
    </div>
</body>
</html>
