<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Message" CodeFile="PG_Message.aspx.vb"
 MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<% Response.CacheControl = "no-cache" %>
<% Response.AddHeader("Pragma", "no-cache") %>
<% Response.Expires = -1 %> 


	<br /><br />
	<asp:Table Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="1" Runat="Server" ID="Table1">
		<asp:TableRow>
			<asp:TableCell CssClass="LABELHEAD">
				<asp:Label Runat="Server" ID="lblHeading" Text="Message Center" CssClass="FORMHEAD"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" HorizontalAlign="Center">
				<asp:Label ID="lblMsg" Runat="Server" CssClass="MSG"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" HorizontalAlign="Center">
				<asp:Button CssClass="BUTTON" Text="Try Again" Runat="Server" ID="btnCont"></asp:Button>&nbsp;
				<asp:Button CssClass="BUTTON" Text="Sign Out" Runat="Server" ID="btnLog"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<br /><br /><br /><br /><br /><br /><br />

</asp:Content>