<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ChangeAuthCode" CodeFile="PG_ChangeAuthCode.aspx.vb"     MasterPageFile="~/WebForms/mp_Master.master" EnableViewState="false" EnableEventValidation="false" EnableViewStateMac="true" ViewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
   		<script type="text/javascript" src="../include/common.js"></script>
		<script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			var strRequest;
			strRequest = document.forms[0].ctl00$cphContent$hUserType.value;
			if(strRequest == "CA")
			{
				window.location.href = "PG_ViewRoles.aspx";
			}
			else if(strRequest == "BU")
			{
				window.location.href = "PG_ViewPassword.aspx"
			}
			else if (strRequest == "BA")
			{
			    
			    if (document.forms[0].ctl00$cphContent$hOrgID.value > 0)
			    {
			        window.location.href = "PG_ViewOrganizationRoles.aspx?Id=" + document.forms[0].ctl00$cphContent$hOrgID.value
			        
			    }
			    else
			    {
			        window.location.href = "PG_ViewRoles.aspx";
			    }
			} 
		}
		function fncSign()
		{
			window.location.href = "PG_Logout.aspx?Timed=True";
		}
		</script>
		

	<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="Change Validation Code" Runat="server" CssClass="FORMHEAD" ID ="lblHeading"></asp:Label></td>
      </tr>
  </table>
			<!-- Main table Starts Here -->
			<asp:Table Width="100%" Runat="Server" CellPadding="8" CellSpacing="0" ID="tblAth">
				<asp:TableRow>
					<asp:TableCell>
						<!-- Form Table Starts Here -->
						<asp:Table Width="100%" CellPadding="2" CellSpacing="0" Runat="Server" BorderWidth="0">
							<asp:TableRow ID="trMsg">
								<asp:TableCell Width="100%" ColumnSpan="2"><asp:Label Runat="Server" CssClass="MSG" ID="lblMessage"></asp:Label></asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trOldAuth" runat="server">
								<asp:TableCell Width="20%" Wrap="False">
									<asp:Label ID="lblOldAuthCode" Runat="Server" Text="Old Validation Code" CssClass="LABEL"></asp:Label>&nbsp;
									<asp:Label Runat="Server" Text="*" id="lblOldAst" CssClass="MAND" ></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:TextBox ID="txtOldAuthCode" Runat="Server" TextMode="Password" MaxLength="14" CssClass="MEDIUMTEXT"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trNewAuth">
								<asp:TableCell Width="20%" Wrap="False">
									<asp:Label ID="lblNewAuthCode" Runat="Server" Text="New Validation Code" CssClass="LABEL"></asp:Label>&nbsp;
									<asp:Label Runat="Server" Text="*" CssClass="MAND" ID="Label3"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:TextBox ID="txtNewAuthCode" Runat="Server" TextMode="Password" MaxLength="24" CssClass="MEDIUMTEXT"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trConAuth">
								<asp:TableCell Width="20%" Wrap="False">
									<asp:Label ID="lblConAuthCode" Runat="Server" Text="Confirm Validation Code" CssClass="LABEL"></asp:Label>&nbsp;
									<asp:Label Runat="Server" Text="*" CssClass="MAND" ID="Label4"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:TextBox ID="txtConAuthCode" Runat="Server" TextMode="Password" MaxLength="24" CssClass="MEDIUMTEXT"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="100%" ColumnSpan="2">
									<asp:Button CssClass="BUTTON" Runat="Server" ID="btnSave" Text="Save"></asp:Button>&nbsp;<input id="btnClear" type="reset" runat="server" value="Clear" class="BUTTON"/>&nbsp;<input id="inbtnSignOut" type="button" runat="server" value="Sign Out" class="BUTTON" onclick="fncSign();"/><input type="button" runat="Server" onclick="fncBack();" value="Back" class="BUTTON" id="btnBack"/>&nbsp;
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<!-- Form Table Ends Here -->
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<!-- Main table Ends Here -->
			
			<!-- Hidden Controls Starts Here-->
			<input type="hidden" id="hAuth" name="hAuth" runat="Server"/>
			<input type="hidden" id="hUserId" name="hUserId" runat="Server"/>
			<input type="hidden" id="hUserType" name="hUserType" runat="server"/>
			<input type="hidden" id="hUserLogin" name="hUserLogin" runat="Server"/>
			<input type="hidden" id="hOrgID" name="hOrgID" runat="Server"/>
			<!-- Hidden Controls Ends Here-->
			
			<!-- Validators Starts Here -->
			<asp:RequiredFieldValidator ID="rfvOldAuthCode" Runat="server" ControlToValidate="txtOldAuthCode" ErrorMessage="Old Validation Code cannot be blank" Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvNewAuthCode" Runat="server" ControlToValidate="txtNewAuthCode" ErrorMessage="New Validation Code cannot be blank" Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvConAuthCode" Runat="server" ControlToValidate="txtConAuthCode" ErrorMessage="Confirm Validation Code cannot be blank" Display="None"></asp:RequiredFieldValidator>
			<asp:CompareValidator ID="cvAuthCode" Runat="Server" ControlToCompare="txtNewAuthCode" ControlToValidate="txtConAuthCode" ErrorMessage="New Validation Code and Confirm Validation Code does not match" Display="None"></asp:CompareValidator>
			<asp:RegularExpressionValidator ID="rgeOldAuthCode" Runat="server" ControlToValidate="txtOldAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Old Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:RegularExpressionValidator ID="rgeNewAuthCode" Runat="server" ControlToValidate="txtNewAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="New Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:RegularExpressionValidator ID="rgeConAuthCode" Runat="server" ControlToValidate="txtConAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Confirm Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>			
			<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations:" ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
			<!-- Validators Ends Here -->
			
</asp:Content>
