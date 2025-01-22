<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ChangePassword" CodeFile="PG_ChangePassword.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" EnableViewStateMac="true" ViewStateEncryptionMode="Always"
    
    %>

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
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Change Password</asp:Label></td>
      </tr>
  </table>
			<!-- Main table Starts Here -->
			<asp:Table Width="100%" Runat="Server" CellPadding="8" CellSpacing="0" ID="tblPwd">
				
				<asp:TableRow>
					<asp:TableCell>
						<!-- Form Table Starts Here -->
						<asp:Table Width="100%" CellPadding="2" CellSpacing="0" Runat="Server" BorderWidth="0">
							<asp:TableRow>
								<asp:TableCell Width="100%" ColumnSpan="2"><asp:Label Runat="Server" CssClass="MSG" ID="lblMessage"></asp:Label></asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trOldPwd">
								<asp:TableCell Width="20%" Wrap="False">
									<asp:Label ID="lblOldPwd" Runat="Server" Text="Old Password" CssClass="LABEL"></asp:Label>&nbsp;
									<asp:Label ID="lblOldAst" Runat="Server" Text="*" CssClass="MAND"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:TextBox ID="txtOldPwd" Runat="Server" TextMode="Password" MaxLength="24" CssClass="MEDIUMTEXT"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trNewPwd">
								<asp:TableCell Width="20%" Wrap="False">
									<asp:Label ID="lblNewPwd" Runat="Server" Text="New Password" CssClass="LABEL"></asp:Label>&nbsp;
									<asp:Label Runat="Server" Text="*" CssClass="MAND" ID="Label1"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:TextBox ID="txtNewPwd" Runat="Server" TextMode="Password" MaxLength="24" CssClass="MEDIUMTEXT"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trConPwd">
								<asp:TableCell Width="20%" Wrap="False">
									<asp:Label ID="lblConPwd" Runat="Server" Text="Confirm Password" CssClass="LABEL"></asp:Label>&nbsp;
									<asp:Label Runat="Server" Text="*" CssClass="MAND" ID="Label2"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:TextBox ID="txtConPwd" Runat="Server" TextMode="Password" MaxLength="24" CssClass="MEDIUMTEXT"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="100%" ColumnSpan="2">
									<asp:Button CssClass="BUTTON" Runat="Server" ID="btnSave" Text="Save"></asp:Button>&nbsp;
									<input type="reset" runat="server" value="Clear" class="BUTTON"/>&nbsp;<input type="button" runat="Server" onclick="fncBack();" value="Back" class="BUTTON" id="btnBack" />&nbsp;<input id="inbtnSignOut" type="button" runat="server" value="Sign Out" class="BUTTON" onclick="fncSign();"/>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trSign" Visible="false">
								<asp:TableCell Width="100%" ColumnSpan="2">
									
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<!-- Form Table Ends Here -->
					</asp:TableCell>					
				</asp:TableRow>
			</asp:Table>
			<!-- Main table Ends Here -->
			
			<!-- Hidden Values Starts Here -->
			<input type="hidden" id="hUserType" name="hUserType" runat="server"/>
			<input type="hidden" id="hUserId" name="hUserId" runat="Server"/>
			<input type="hidden" id="hUserLogin" name="hUserLogin" runat="Server"/>
			<input type="hidden" id="hUserPswdExp" name="hUserPswdExp" runat="Server"/>
			<input type="hidden" id="hUserPwdReset" name="hUserPwdReset" runat="Server"/>
			<input type="hidden" id="hUserPwdResetDt" name="hUserPwdResetDt" runat="Server"/>
	        <input type="hidden" id="hOrgID" name="hOrgID" runat="server"/>		
			<!-- Hidden Values Ends Here -->
			
			<!-- Form Validators Starts Here -->
			<asp:RequiredFieldValidator ID="rfvOldPwd" Runat="server" ControlToValidate="txtOldPwd" ErrorMessage="Old Password cannot be blank" Display="None"></asp:RequiredFieldValidator>			
			<asp:RequiredFieldValidator ID="rfvNewPwd" Runat="server" ControlToValidate="txtNewPwd" ErrorMessage="New Password cannot be blank" Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvConPwd" Runat="server" ControlToValidate="txtConPwd" ErrorMessage="Confirm Password cannot be blank" Display="None"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="rgeOldPwd" Runat="server" ControlToValidate="txtOldPwd" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Old Password must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:RegularExpressionValidator ID="rgeNewPwd" Runat="server" ControlToValidate="txtNewPwd" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="New Password must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:RegularExpressionValidator ID="rgeConPwd" Runat="server" ControlToValidate="txtConPwd" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Confirm Password must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:CompareValidator ID="cvPass" Runat="Server" ControlToCompare="txtNewPwd" ControlToValidate="txtConPwd" ErrorMessage="New Password and Confirm Password does not Match" Display="None"></asp:CompareValidator>
			<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations;," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
			<!-- Form Validators Ends Here -->
			
</asp:Content>