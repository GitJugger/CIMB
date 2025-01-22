<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ResetPassword" CodeFile="PG_ResetPassword.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
 
 		<script type="text/javascript" src="../include/common.js"></script>
		
			<!-- Main table Starts Here -->
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
            <tr>
                <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Reset Password</asp:Label></td>
            </tr>
            </table>
			<table border="0" cellpadding="8" cellspacing="0" width="100%">
				
				<tr>
					<td>
						<!-- Form Table Starts Here -->
						<table width="100%" cellpadding="2" cellspacing="0" border="0">
							<tr>
							</tr>
							<TR>
								<td width="100%" colspan="2"><asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label></td>
							</TR>
							<tr>
								<td width="20%"><asp:Label ID="lblOrgId" Runat="Server" Text="Organization ID" CssClass="LABEL"></asp:Label></td>
								<td width="80%"><asp:Label ID="lblTxtOrgId" Runat="Server" CssClass="LABEL" Font-Bold="True"></asp:Label></td>
							</tr>
							<tr>
								<td width="20%"><asp:Label ID="lblUserLogin" Runat="Server" Text="User Login" CssClass="LABEL"></asp:Label></td>
								<td width="80%"><asp:Label ID="lblTxtUserLogin" Runat="Server" CssClass="LABEL" Font-Bold="True"></asp:Label></td>
							</tr>
							<tr>
								<td width="20%"><asp:Label ID="lblOrgName" Runat="Server" Text="Organization Name" CssClass="LABEL"></asp:Label></td>
								<td width="80%"><asp:Label ID="lblTxtOrgName" Runat="Server" CssClass="LABEL" Font-Bold="True" Width="624px"></asp:Label></td>
							</tr>
							<tr>
								<td width="100%" colspan="2">
									<asp:Button CssClass="BUTTON" Runat="Server" ID="btnGenerate" Text="Reset"></asp:Button>&nbsp;
									<asp:Button Runat="Server" ID="btnBack" CssClass="BUTTON" Text="Back" CausesValidation="False"></asp:Button>&nbsp;
								</td>
							</tr>
						</table>
				<!-- Form Table Ends Here -->
			</table>
			</td></tr> 
			<input type =hidden name="hUserId" id="hUserId" runat =server >
			<input type =hidden name="hUserStatus" id="hUserStatus" runat =server >
			</Td> 
			<!-- Main Table Ends Here -->

</asp:Content>
