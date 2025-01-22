<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_DeleteFile" CodeFile="PG_DeleteFile.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

		<script type="text/javascript" src="../include/common.js"></script>
		<script language="JavaScript">
		function fncBack()
		{
			window.location.href = "PG_StopPayment.aspx";
		}
		</script>
		

			<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label CssClass="FORMHEAD" Runat="server" id="lblHeading">Payment Service - Stop Payment</asp:Label></td>
       </tr>
       <tr>
         <td><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
       </tr>
  </table>
						
			<!-- Form Table Starts Here -->
			<asp:Table Width="100%" CellPadding="3" CellSpacing="2" Runat="Server" ID="tblForm" BorderWidth="1">
			<asp:TableRow ID="trOrg">
				<asp:TableCell Width="20%">
					<asp:Label ID="lblOrgText" Runat="Server" Text="Organization Id/Name" CssClass="LABEL"></asp:Label>	
				</asp:TableCell>
				<asp:TableCell Width="80%">
					<asp:Label ID="lblOrgName" CssClass="LABEL" Runat="Server"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trGroup" Visible="False">
				<asp:TableCell Width="20%">
					<asp:Label CssClass="LABEL" Runat="Server" Text="Group Name" ID="lblGroup"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">
				<asp:Label Runat="Server" CssClass="LABEL" ID="lblGroupName"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>														
			<asp:TableRow>
				<asp:TableCell Width="20%">
					<asp:Label CssClass="LABEL" Runat="Server" Text="File Name"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">
					<asp:Label Runat="Server" CssClass="LABEL" ID="lblFileName"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="20%">
					<asp:Label CssClass="LABEL" Runat="Server" Text="Converted File Name"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">
					<asp:Label ID="lblCFName" Runat="Server" CssClass="LABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="20%">
					<asp:Label CssClass="LABEL" Runat="Server" Text="Upload Date"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">
					<asp:Label Runat="Server" CssClass="LABEL" ID="lblUploadDt"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="20%">
					<asp:Label CssClass="LABEL" Runat="Server" Text="Payment Date"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">
					<asp:Label Runat="Server" CssClass="LABEL" ID="lblValueDt"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Label ID="lblStopChrg" CssClass="MSG" Runat="Server"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trAuthCode" Visible="False">
				<asp:TableCell Width="20%">
					<asp:Label CssClass="LABEL" Runat="server" Text="Validation Code" ID="lblAuthCode"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">
					<asp:TextBox ID="txtAuthCode" TextMode="Password" Runat="Server"></asp:TextBox>
				</asp:TableCell>
			</asp:TableRow>		
			<asp:TableRow ID="trConfirm">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Button Runat="Server" Text="Confirm" CssClass="BUTTON" ID="btnStop"></asp:Button>&nbsp;
					<input type="button" runat="Server" value="Back" onclick="fncBack();" class="BUTTON">
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trSubmit">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Button Runat="Server" Text="Submit" CssClass="BUTTON" ID="btnSubmit"></asp:Button>&nbsp;
					<input type="button" runat="Server" value="Back" onclick="fncBack();" class="BUTTON">
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trBack" Visible=False>
				<asp:TableCell Width="100%" ColumnSpan="2">
					<input type="button" value="Back" runat="server" onclick="fncBack();" class="BUTTON">
				</asp:TableCell>
			</asp:TableRow>
			</asp:Table>
			<!-- Form Table Ends Here -->
			<asp:RequiredFieldValidator ID="rfvAuthCode" Runat="Server" ErrorMessage="Validation Code Cannot be blank" Display="None" ControlToValidate="txtAuthCode"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Validation Code must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:ValidationSummary ID="vsBlockFile" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>

</asp:Content>