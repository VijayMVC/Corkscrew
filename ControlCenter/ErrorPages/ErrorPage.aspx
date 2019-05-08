<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Corkscrew.ControlCenter.ErrorPages.ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        Oops! We hit a snag... | Corkscrew Manager</title>

    <link rel="stylesheet" type="text/css" href="/resources/styles/theme-main.css" />
    <link rel="icon" href="/resources/images/favicon.png" />

    <script type="text/javascript" src="/external/jquery/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="/external/fvr3lgc.js"></script>
    <script type="text/javascript">

        $(document).ready(function ($) {
            try {
                Typekit.load();
            } catch (e) { }
        });
    </script>
</head>
<body>
    <div class="topBanner dropShadowBox">
        <img id="siteActionsMenuGear" class="siteActionsMenu" src="/resources/images/corkscrew_icon.png" onclick="document.location.href='/'; return true;" />
        <span class="topBannerText dropShadowText">Aquarius Corkscrew - Central Control Center</span>
    </div>
    <div class="contentArea">
        <div class="contentAreaContent">
            <h1>
                Oops! We hit a snag...
            </h1>
            <p>
                We had a problem. 
                <%
                    try
                    {
                        string error = Corkscrew.SDK.tools.Utility.Base64Decode(Request["Error"]);
                        
                        if (! string.IsNullOrEmpty(error))
                        {
                            Response.Write(error);
                        }
                    }
                    catch { }
                %>
            </p>
            <p>
                Return to <a href="/">homepage</a>.
            </p>
        </div>
    </div>
</body>
</html>
