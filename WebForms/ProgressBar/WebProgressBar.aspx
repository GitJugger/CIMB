<%@ Page Language="VB" AutoEventWireup="false" CodeFile="WebProgressBar.aspx.vb" Inherits="WebProgressBar" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WebProgressBar</title>
    <style type="text/css">
<!--
body {
	margin-left: 0px;
	margin-top: 0px;
	margin-right: 0px;
	margin-bottom: 0px;
}
a:link {
	color: #0000FF;
}
a:visited {
	color: #0000FF;
}
a:hover {
	color: #0000FF;
	text-decoration: none;
}
a:active {
	color: #0000FF;
	}
.basix {
	font-family: Arial, Helvetica, sans-serif, Arial, Helvetica, sans-serif;
	font-size: 11px;
}
.header1 {
	font-family: Arial, Helvetica, sans-serif, Arial, Helvetica, sans-serif;
	font-size: 11px;
	font-weight: bold;
	color: #006699;
}
.lgHeader1 {
	font-family: Arial, Helvetica, sans-serif;
	font-size: 18px;
	font-weight: bold;
	color: #0066CC;
	background-color: #CEE9FF;
}
-->
</style>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="752">
            <tr bgcolor="#5482fc">
                <td colspan="4">
                    <img height="1" src="/media/spacer.gif" width="1" /></td>
            </tr>
            <tr>
                <td bgcolor="#5482fc" width="1">
                    <img alt="Server Intellect" height="1" src="media/spacer.gif" width="1" /></td>
                <td width="250">
                    <a href="http://www.serverintellect.com">
                        <img alt="Server Intellect" border="0" height="75" src="media/logo.gif" width="250" /></a></td>
                <td bgcolor="#3399ff" width="500">
                    <a href="http://www.serverintellect.com">
                        <img alt="Server Intellect" border="0" height="75" src="media/headerR1.gif" width="500" /></a></td>
                <td bgcolor="#5482fc" width="1">
                    <img alt="Server Intellect" height="1" src="media/spacer.gif" width="1" /></td>
            </tr>
            <tr bgcolor="#5482fc">
                <td colspan="4">
                    <img height="1" src="media/spacer.gif" width="1" /></td>
            </tr>
        </table>
        <br />
        <table align="center" bgcolor="#5482fc" border="0" cellpadding="5" cellspacing="1"
            width="600">
            <tr>
                <td align="center" class="lgHeader1" height="50">
                    How to create web progress bar using ASP.NET 2.0 and VB.NET</td>
            </tr>
        </table>
        <br />
        <br />
        <br />
    <fieldset>
    <legend>WebProgressBar</legend>
    <div>
    <asp:Button id="Button1" runat="server" Text="Start Long Task!" OnClick="Button1_Click"></asp:Button>
    </div></fieldset>
        <br />
        <br />
        <br />
        <br />
        <table align="center" cellpadding="0" cellspacing="0" width="500">
            <tr>
                <td align="center" class="basix" height="50">
                    <strong>Power. Stability. Flexibility.</strong><br />
                    Hosting from <a href="http://www.serverintellect.com">Server Intellect</a><br />
                    <br />
                    For more ASP.NET Tutorials visit <a href="http://www.AspNetTutorials.com">www.AspNetTutorials.com</a></td>
            </tr>
        </table>
    </form>
</body>
</html>
