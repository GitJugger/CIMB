<%@ Page Language="vb" Inherits="MaxPayroll.PG_Organisation" AutoEventWireup="false" CodeFile="PG_Organisation.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
     <script type="text/javascript" src="../include/common.js"></script>
		
		<script type="text/javascript" language="javascript">
			function fncBack()
			{
				window.history.back();
			}
			function fncNew()
			{
				window.location.href = "PG_Organisation.aspx";
			}
			function fncView()
			{
				window.location.href = "PG_ViewOrganisation.aspx?Req=Modify";
			}
		</script>
 	
	<!-- Heading Table Starts Here -->
	<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label ID="lblHeading" Runat="Server" CssClass="FORMHEAD"></asp:Label></td>
      </tr>
      <tr>
         <td id="cErrMsg"><asp:Label ID="lblOrg" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
      <tr>
        <td height="5"></td>
      </tr>
  </table>
	<!-- Heading Table Ends Here -->
	<!-- Form Table Starts Here -->
	<asp:Table Width="100%" ID="tblForm" BorderWidth="0" CellPadding="8" CellSpacing="0" Runat="Server">
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Organization Name" CssClass="LABEL"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtOrgName" Runat="server" CssClass="LARGETEXT" MaxLength="40"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Region Name"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtRegion" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="35"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow Visible="True">
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Branch Code"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label13"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtBrCode" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="4" Text="0000"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False" VerticalAlign="Top">
				<asp:Label Runat="Server" Text="Address" CssClass="LABEL"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label1"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox ID="txtAddress" Runat="server" CssClass="LARGETEXT" Rows="3" TextMode="MultiLine" MaxLength="255"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="State"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label2"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Dropdownlist id="cmbState" Runat="server" CssClass="MEDIUMTEXT"></asp:Dropdownlist>&nbsp;
				<asp:Label Runat="Server" CssClass="LABEL" Text="If Others,"></asp:Label>&nbsp;
				<asp:TextBox ID="txtState" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:TextBox>			
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="PostCode"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label3"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtPincode" Runat="server" CssClass="SMALLTEXT" MaxLength="5"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Country"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCountry" Runat="server" CssClass="SMALLTEXT" Text="MALAYSIA"></asp:Textbox>				
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 1"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label4"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtPhone1" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 2"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtPhone2" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Fax"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtFax" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Email"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtEmail" Runat="server" CssClass="LARGETEXT" MaxLength="50"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Website"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtURL" Runat="Server" CssClass="LARGETEXT" MaxLength="50"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Organization Logo"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<input type='file' runat ='server' id="flImg" name="flImg" accept="image/*" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Employees"></asp:Label>&nbsp;
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label9"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:TextBox ID="txtEmployees" Runat="Server" CssClass="SMALLTEXT" MaxLength="5"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Groups"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label15"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtGroups" Runat="Server" CssClass="MINITEXT" MaxLength="2"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Corporate Administrator" CssClass="LABEL"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label5"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtContactPerson" Runat="server" CssClass="LARGETEXT" MaxLength="20"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corportate Administrator IC No"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label6"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtContactPerIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Authorizer"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label11"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%" Wrap="False">
				<asp:TextBox ID="txtCustomerAdmin" Runat="Server" CssClass="LARGETEXT" MaxLength="20"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Authorizer IC No"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label17"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCustomAdminIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Stop Payment Charge"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtStopCharges" MaxLength="8" Text="10.00" Runat="Server" CssClass="SMALLTEXT"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Subscription Fees"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox Runat="server" ID="txtAnnualFee" MaxLength="8" CssClass="SMALLTEXT" Text="0.00"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Business Registration No"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label12"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">			
				<asp:Textbox id="txtBusReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Tax Registration No"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtTaxReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Privilege Submission"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:CheckBox ID="chkPrivilege" Runat="Server"></asp:CheckBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Status"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:RadioButtonList ID="rdStatus" CssClass="LABEL" Runat="Server" RepeatDirection="Horizontal">
					<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
					<asp:ListItem Value="C">Inactive</asp:ListItem>
				</asp:RadioButtonList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Use&nbsp;Token"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:CheckBox ID="chkUseToken" Checked = 'True' Runat='server'></asp:CheckBox>
			</asp:TableCell>			
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Label Runat="Server" CssClass="BLABEL" Text="Payment Services"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2">
				<!-- Payment Data Grid Starts Here -->
				<asp:panel CssClass="GridDivNoScroll" runat= "server" ID="pnlGrid" Width="40%">
				<asp:DataGrid Runat="Server" ID="dgPayService" AllowPaging="False" PageSize="15" PagerStyle-Mode="NumericPages"
					PagerStyle-HorizontalAlign="Center" BorderWidth="0" 
					 HeaderStyle-CssClass="GridHeaderStyle"
					ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" width="100%" HeaderStyle-HorizontalAlign="left"
					AutoGenerateColumns="False" CssClass="Grid">
					<Columns>
					<asp:TemplateColumn HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="3%">
						<ItemTemplate>
							<asp:CheckBox ID="chkService" Runat="Server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="PDESC" HeaderText="Service" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="12%"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Charge Per Tran." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
						<ItemTemplate>
							<asp:TextBox ID="txtCharge" Runat="server" CssClass="MINITEXT" Text="0.00" MaxLength="8"></asp:TextBox>
							<asp:RegularExpressionValidator ID="revCharge" Runat="Server" ControlToValidate="txtCharge" Display="None" ErrorMessage="Charge should be in currency value" ValidationExpression="[0-9.,]{1,8}"></asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid></asp:panel>
				<!-- Payment Data Grid Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnSave"   Runat="Server" Text="Confirm" CssClass="BUTTON"></asp:Button>&nbsp;
				<input type="reset" runat="Server" value="Clear" class="BUTTON"/>&nbsp;
				<input type="button" runat="server" value="Back To View" onclick="fncView();"/>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Form Table Ends Here -->
	
	<!-- Confirmation Table Starts here -->
	<asp:Table ID="tblConfirm" Runat="Server" CellPadding="2" CellSpacing="1" BorderWidth="1" Width="90%" Visible="False">
	<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Organization Name" CssClass="LABEL" ID="Label10"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCOrgName" Runat="server" CssClass="LARGETEXT" MaxLength="40" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Region Name" ID="Label7"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCRegion" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="35" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="True">
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Branch Code" ID="Label14"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCBrCode" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="4" Text="0000" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False" VerticalAlign="Top">
				<asp:Label Runat="Server" Text="Address" CssClass="LABEL" ID="Label16"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox ID="txtCAddress" Runat="server" CssClass="BIGTEXT" Rows="3" TextMode="MultiLine" MaxLength="200" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="State" ID="Label18"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblState" Runat="Server" CssClass="LABEL"></asp:Label>&nbsp;
				<asp:Label Runat="Server" CssClass="LABEL" Text="If Others," ID="Label19"></asp:Label>&nbsp;
				<asp:TextBox ID="txtCState" Runat="Server" CssClass="MEDIUMTEXT" ReadOnly="True"></asp:TextBox>			
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="PostCode" ID="Label21"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCPinCode" Runat="Server" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Country" ID="Label23"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCCOuntry" Runat="server" CssClass="SMALLTEXT" ReadOnly="True"></asp:Textbox>				
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 1" ID="Label24"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCPhone1" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 2" ID="Label26"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCPhone2" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Fax" ID="Label27"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCFax" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Email" ID="Label28"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCEmail" Runat="server" CssClass="LARGETEXT" MaxLength="50" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Website" ID="Label29"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtCURL" Runat="Server" CssClass="LARGETEXT" MaxLength="50" ReadOnly="True"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Organization Logo" ID="Label30"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblLogo" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Employees" ID="Label8"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:TextBox ID="txtCEmployees" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Groups" ID="Label20"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCGroups" Runat="Server" CssClass="MINITEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator" ID="Label31"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCContactPerson" Runat="server" CssClass="LARGETEXT" MaxLength="60" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator IC No" ID="Label33"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCContactPerIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Authorizer" ID="Label35"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%" Wrap="False">
				<asp:TextBox ID="txtCCustomerAdmin" Runat="Server" CssClass="LARGETEXT" MaxLength="40" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Authorizer IC No" ID="Label37"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCCustomAdminIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Stop Payment Charge" ID="Label42"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCStopCharges" MaxLength="8" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Subscription Fees" ID="Label43"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox Runat="server" ID="txtCAnnualFee" MaxLength="8" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Business Registration No" ID="Label44"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">			
				<asp:Textbox id="txtCBusReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Tax Registration No" ID="Label49"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtCTaxReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Privilege Submission" ID="Label51"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:Label ID="lblPrvSub" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Status" ID="Label52"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblStatus" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Use&nbsp;Token" ID="Label22"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblUseToken" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Label Runat="Server" CssClass="BLABEL" Text="Payment Services" ID="Label53"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2">
				<!-- Payment Data Grid Starts Here -->
				<asp:DataGrid Runat="Server" ID="dgCPayService" AllowPaging="False"	PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" 
					GridLines="Both" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD"
					AlternatingItemStyle-CssClass="ALTERNATE" width="40%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center"
					AutoGenerateColumns="False">
					<Columns>
					<asp:BoundColumn DataField="PID" HeaderText="Service" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="12%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="PDESC" HeaderText="Service" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="12%"></asp:BoundColumn>
					<asp:BoundColumn DataField="PCHRG" HeaderText="Charge Per Tran." ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="12%"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Payment Data Grid Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trConfirm">
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnConfirm" Runat="Server" Text="Save" CssClass="BUTTON"></asp:Button>&nbsp;
				<input type="button" runat="Server" value="Back" onclick="fncBack();" class="BUTTON"/>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trNew" Visible="False">
			<asp:TableCell ColumnSpan="2">
				<input type="button" runat="Server" value=" Create New " onclick="fncNew();"/>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Confirmation Table Starts here --> 
	
	<!-- Hidden Box Starts Here -->
	<input id="hOrgId" type="hidden" name="hOrgId" runat="Server"/>
	<input id="hcmbState" type="hidden" name="hcmbState" runat="Server"/>
	<input id="hchkStatus" type="hidden" name="hchkStatus" runat="Server"/>
	<input id="hchkPrivilege" type="hidden" name="hchkPrivilege" runat="Server"/>
	<input id="hchkUseToken" type="hidden" name="hchkUseToken" runat="server"/>
	<!-- Hidden Box Ends Here -->
			
	<!-- Form Validations Starts Here -->
	<asp:ValidationSummary ID="vsOrganisation" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
	<asp:RequiredFieldValidator ID="rfvOrgName" Runat="Server" ControlToValidate="txtOrgName" Display="None" ErrorMessage="Organisation Name cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvAddress" Runat="Server" ControlToValidate="txtAddress" Display="None" ErrorMessage="Address cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvPinCode" Runat="Server" ControlToValidate="txtPincode" Display="None" ErrorMessage="Postcode cannot be blank."></asp:RequiredFieldValidator>
	<asp:RangeValidator ID="rgvState" Runat="Server" ControlToValidate="cmbState" MinimumValue="1" MaximumValue="14" Type="Integer" Display="None" ErrorMessage="Select State"></asp:RangeValidator>
	<asp:RequiredFieldValidator ID="rfvPhone1" Runat="Server" ControlToValidate="txtPhone1" Display="None" ErrorMessage="Phone 1 cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvEmployees" Runat="Server" ControlToValidate="txtEmployees" Display="None" ErrorMessage="No of Employees cannot be blank"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvContactPerson" Runat="Server" ControlToValidate="txtContactPerson" Display="None" ErrorMessage="Corporate Administrator cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvContactPersonIC" Runat="Server" ControlToValidate="txtContactPerIC" Display="None" ErrorMessage="Corporate Administrator IC cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCustomerAdmin" Runat="Server" ControlToValidate="txtCustomerAdmin" Display="None" ErrorMessage="Corporate Authorizer cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCustomerAdminIc" Runat="Server" ControlToValidate="txtCustomAdminIC" Display="None" ErrorMessage="Corporate Authorizer IC cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvBusiRegNo" Runat="Server" ControlToValidate="txtBusReg" Display="None" ErrorMessage="Business Resgistration No cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvStopCharge" Runat="Server" ControlToValidate="txtStopCharges" Display="None" ErrorMessage="Stop Payment cannot be blank"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvGroup" Runat="Server" ControlToValidate="txtGroups" Display="None" ErrorMessage="No of Groups cannot be blank"></asp:RequiredFieldValidator>
	<asp:RangeValidator ID="rngGroup" Runat="server" ControlToValidate="txtGroups" Display="None" MinimumValue="1" MaximumValue="99" Type="Integer" ErrorMessage="No Groups should be between 1-99"></asp:RangeValidator>
	<asp:RegularExpressionValidator ID="rgeBrCode" Runat="Server" ControlToValidate="txtBrCode" Display="None" ValidationExpression="\d{4}" ErrorMessage="Branch Code should be 4 digits"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="rgeStopCharge" Runat="Server" ControlToValidate="txtStopCharges" Display="None" ErrorMessage="Stop Payment Charge should be in currency value" ValidationExpression="[0-9.,]{1,14}"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="revEmployees" Runat="Server" ControlToValidate="txtEmployees" ValidationExpression="\d{1,5}" ErrorMessage="No of Employees must be numeric value" Display="None"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="rfvGroups" Runat="Server" ControlToValidate="txtGroups" ValidationExpression="\d{1,2}" Display="None" ErrorMessage="Groups Should be a numeric value"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="revPinCode" Runat="Server" ControlToValidate="txtPincode" ValidationExpression="\d{5}" ErrorMessage="PostCode must be 5 digits." Display="None"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="revEmail" Runat="Server" ControlToValidate="txtEmail" ValidationExpression=".+@.+\..+" ErrorMessage="Invalid Email Account" Display="None"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="revAnnual" Runat="Server" ControlToValidate="txtAnnualFee" ValidationExpression="[0-9.,]{1,14}" ErrorMessage="Subscription Fee should be in currency value." Display="None"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="rgetxtBankPhone" runat="server" Display="None"
        ErrorMessage="Phone number is invalid" ControlToValidate="txtPhone1" ValidationExpression="^\d{1,20}$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
        ErrorMessage="Phone number is invalid" ControlToValidate="txtPhone2" ValidationExpression="^\d{1,20}$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgetxtFax" runat="server" Display="None" ErrorMessage="Fax number is invalid"
        ControlToValidate="txtFax" ValidationExpression="^\d{1,15}$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgetxtWebAddress" runat="server" ErrorMessage="Invalid Website Address"
        ControlToValidate="txtURL" Display="None" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator>
    
    
    <!-- Form Validations Ends Here -->
	
</asp:Content>
