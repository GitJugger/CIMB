<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Mail" CodeFile="PG_Mail.aspx.vb" 
   MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <script type="text/javascript" language="JavaScript">
    function fncBack()
    {
		window.location.href = "PG_Inbox.aspx";
    }
    </script>



	<asp:Table Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0" Runat="Server">
	<asp:TableRow>
		<asp:TableCell Width="100%" HorizontalAlign="Right"><asp:Label ID="lblCurrDate" Runat="Server" CssClass="LABEL"></asp:Label></asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%"><asp:Label Runat="Server" CssClass="FORMHEAD" Text="Mail Message" ID="Label1" NAME="Label1"></asp:Label></asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%"><asp:Label Id="lblDate" Runat="Server" CssClass="LABEL" Text="Date:"></asp:Label>&nbsp;
			<asp:Label ID="lblDtValue" Runat="Server" CssClass="LABEL"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%"><asp:Label ID="lblTime" Runat="Server" CssClass="LABEL" Text="Time:"></asp:Label>&nbsp;
			<asp:Label ID="lblTmValue" Runat="Server" CssClass="LABEL"></asp:Label>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow><asp:TableCell Width="100%">&nbsp;</asp:TableCell></asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%"><asp:Label ID="lblSubject" Runat="Server" CssClass="LABEL" Text="Subject:"></asp:Label>&nbsp;
			<asp:Label ID="lblSbValue" Runat="Server" CssClass="BLABEL"></asp:Label>
	</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%"><asp:Label ID="lblTo" Runat="Server" CssClass="LABEL"></asp:Label></asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%"><asp:Label ID="lblBody" CssClass="LABEL" Runat="Server"></asp:Label></asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell width="100%"><asp:Label ID="lblFrom" CssClass="LABEL" Runat="Server"></asp:Label></asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">
			<asp:Button CssClass="BUTTON" Runat="Server" ID="btnDelete" Text="Delete"></asp:Button>&nbsp;
			<input type="button" runat="server" class="BUTTON" onclick="fncBack();" value="Back"/>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Main Table Ends Here -->
</asp:Content>
