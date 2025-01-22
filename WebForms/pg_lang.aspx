<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Lang" CodeFile="PG_Lang.aspx.vb" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html xmlns="http://www.w3.org/1999/xhtml">
	<head>
		<title>Display Language</title>
		<script type="text/javascript" src="../include/common.js"></script>
    <link href="../Include/Styles.css" rel="stylesheet" type="text/css" />
		<style type="text/css">
    LABEL { FONT-SIZE: 8pt; COLOR: #000000; font-family: Arial, Helvetica, sans-serif }
		</style>
		<meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1"/>
		<meta name="CODE_LANGUAGE" content="Visual Basic .NET 7.1"/>
		<meta name="vs_defaultClientScript" content="JavaScript"/>
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5"/>
	</head>
	<body id="body" runat="server">
	   <form id="Form1" method="post" runat="server">
	   <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Display Language" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td><asp:RadioButtonList ID="rbtnlLang" Runat="server">
				<asp:ListItem Value="en-GB">English</asp:ListItem>
				<asp:ListItem Value="zh-SG">华文</asp:ListItem>
				<asp:ListItem Value="ms-MY">Bahasa Melayu</asp:ListItem>
			</asp:RadioButtonList>
			<br />
			<asp:Button ID="btnChg" Runat="server" CssClass="BUTTON" Text="Change"></asp:Button></td>
         </tr>
      </table> 
		
		
			
		</form>
	</body>
</html>
