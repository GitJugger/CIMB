<%@ Page Language="vb" Inherits="MaxPayroll.PG_Organization" AutoEventWireup="false" CodeFile="pg_Organization.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" MaintainScrollPositionOnPostback="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    

		<script type="text/javascript" src="../include/common.js"></script>
 		
		<script type="text/javascript" language="javascript">
			function fncBack(strRequest)
			{
				if(strRequest == "")
				{
					//window.history.back();
					window.location.href = "pg_vieworganisation.aspx";
				}
				else if(strRequest == "List")
				{
					window.location.href = "PG_ViewOrganisation.aspx?Req=Modify";
				}
				else if(strRequest == "Appr")
				{
					var strMode;
					strMode = document.forms[0].ctl00$cphContent$hMode.value;
					window.location.href = "PG_ApprMatrix.aspx?Mode=" + strMode;
				}
			}
			
			function NumericOnly()
{
var key = window.event.keyCode; 

if (key <48 || key >57)
{
window.event.returnValue = false; 
}
}
        </script>
	
	<!-- Heading Table Starts Here -->
	<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label ID="lblHeading" Runat="Server" CssClass="FORMHEAD"></asp:Label></td>
      </tr>
      <tr>
         <td id="Td1"><asp:Label ID="lblOrg" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
      <tr>
      <td style="height:5"></td>
      </tr>
  </table>
	<!-- Heading Table Ends Here -->
	<!-- Form Table Starts Here -->
	<asp:Table Width="100%" ID="tblForm" BorderWidth="0" CellPadding="8" CellSpacing="0" Runat="Server">
		<asp:TableRow ID="trOrgId">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Organization Id" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtOrgId" CssClass="SMALLTEXT" ReadOnly="True" Runat="Server"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Organization Name" CssClass="LABEL" ID="Label1"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label2"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtOrgName" Runat="server" CssClass="LARGETEXT" MaxLength="40"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="H2H" ID="Label61"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
			    <asp:CheckBox ID="chkH2H" runat="server" AutoPostBack="true" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Region Name" ID="Label43"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtRegion" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="35"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Branch Code" ID="Label3"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label13"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtBrCode" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="4" Text="0000"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False" VerticalAlign="Top">
				<asp:Label Runat="Server" Text="Address" CssClass="LABEL" ID="Label4"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label5"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox ID="txtAddress" Runat="server" CssClass="BIGTEXT" Rows="3" TextMode="MultiLine" MaxLength="255"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="State" ID="Label6"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label7"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Dropdownlist id="cmbState" Runat="server" ></asp:Dropdownlist>
				<asp:Label Runat="Server" CssClass="LABEL" Text="If Others," ID="Label83" Visible="false"></asp:Label>&nbsp;
				<asp:TextBox ID="txtState" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25" Visible="false"></asp:TextBox>		
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Post Code" ID="Label8"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label9"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtPincode" Runat="server" CssClass="SMALLTEXT" MaxLength="5"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Country" ID="Label10"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCountry" Runat="server" CssClass="SMALLTEXT" Text="MALAYSIA" ReadOnly="True"></asp:Textbox>				
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 1" ID="Label11"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label12"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtPhone1" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" OnKeyPress="NumericOnly()"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 2" ID="Label14"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtPhone2" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" OnKeyPress="NumericOnly()"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Fax" ID="Label15"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtFax" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Email" ID="Label16"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtEmail" Runat="server" CssClass="LARGETEXT" MaxLength="150"></asp:Textbox><br /><font size="1">Note: Please separate multiple Email addresses by comma(,)</font>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Website" ID="Label17"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtURL" Runat="Server" CssClass="LARGETEXT" MaxLength="50"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trNonH2H1">
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trLogo">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Organization Logo" ID="Label18"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<input type="file" runat="server" id="flImg" name="flImg" accept="image/*" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow  ID="trNoEmployee">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Employees" ID="Label44"></asp:Label>&nbsp;
				
			</asp:TableCell>
			<asp:TableCell>
				<asp:TextBox ID="txtEmployees" Runat="Server" CssClass="SMALLTEXT"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trNoGroup">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No of Groups" ID="Label84"></asp:Label>&nbsp;
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label34"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtGroups" Runat="Server" CssClass="MINITEXT" MaxLength="2"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow  ID="trNonH2H2">
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCorpAdmin">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator" ID="Label19"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label20"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtContactPerson" Runat="server" CssClass="LARGETEXT" MaxLength="40"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCorpAdminIC">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator IC No" ID="Label21"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label22"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtContactPerIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCorpAuth">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Authorizer" ID="Label23"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label36"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%" Wrap="False">
				<asp:TextBox ID="txtCustomerAdmin" Runat="Server" CssClass="LARGETEXT" MaxLength="40"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCorpAuthIC">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Authorizer IC No" ID="Label24"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label40"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCustomAdminIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow  ID="trNonH2H3">
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Stop Payment Charge" ID="Label28"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtStopCharges" MaxLength="8" Text="0.00" Runat="Server" CssClass="SMALLTEXT"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Subscription Fees" ID="Label29"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox Runat="server" ID="txtAnnualFee" MaxLength="8" CssClass="SMALLTEXT" Text="0.00"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Subscription Fee's Payment Mode" ID="Label59"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:RadioButtonList ID="rblSubscriptionFeePaymentMode" runat="server" RepeatDirection="Horizontal">
				    <asp:ListItem Selected="true" Text="Annually" Value="A"></asp:ListItem>
		            <asp:ListItem Text="Monthly" Value="M"></asp:ListItem>		    
				</asp:RadioButtonList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Business Registration No" ID="Label30"></asp:Label>
				<asp:Label Runat="Server" Text=" * " CssClass="MAND" ID="Label31"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">			
				<asp:Textbox id="txtBusReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Tax Registration No" ID="Label35"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtTaxReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Privilege Submission" ID="Label37"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:CheckBox ID="chkPrivilege" Runat="Server"></asp:CheckBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trNonH2H4">
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trEncryptionKey">
		    <asp:TableCell><asp:Label Runat="Server" CssClass="LABEL" Text="Encryption Key" ID="Label41"></asp:Label></asp:TableCell>
		    <asp:TableCell><asp:textbox id="txtEncryptionKey" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="20"></asp:textbox></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Status" ID="Label38"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:RadioButtonList ID="rdStatus" CssClass="LABEL" Runat="Server" RepeatDirection="Horizontal">
					<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
					<asp:ListItem Value="C">Inactive</asp:ListItem>
					<asp:ListItem Value="D">Cancel</asp:ListItem>
				</asp:RadioButtonList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Use&nbsp;Token" ID="Label32"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:CheckBox ID="chkUseToken" Runat="server"></asp:CheckBox>
			</asp:TableCell>			
		</asp:TableRow>
		<asp:TableRow ID="trVerify" Visible="false">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Verification" ID="Label25"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:RadioButtonList ID="rdVerify" Runat="Server" CssClass="LABEL" RepeatDirection="Horizontal">
					<asp:ListItem Value="1">Single Verification</asp:ListItem>
					<asp:ListItem Value="2" Selected="True">Dual Verification</asp:ListItem>
				</asp:RadioButtonList>
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow ID="trVerify1" Visible="False">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Verification" ID="Label26"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Dual Veirification" ID="Label27"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
		    <asp:TableCell ColumnSpan="2"><hr /></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Label Runat="Server" CssClass="BLABEL" Text="Payment Services" ID="Label39"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2">
				<!-- Payment Data Grid Starts Here -->
				<asp:panel runat="server" ID="pnlGrid" CssClass="GridDivNoScroll" width="40%">
				<asp:DataGrid CssClass="Grid" Runat="Server" ID="dgPayService" AllowPaging="False" PageSize="15" PagerStyle-Mode="NumericPages" 
				PagerStyle-HorizontalAlign="Center"  HeaderStyle-CssClass="GridHeaderStyle" 
				AlternatingItemStyle-CssClass="GridAltItemStyle" Width="100%" HeaderStyle-HorizontalAlign="left" 
				AutoGenerateColumns="False" BorderWidth="0">
					<Columns>
					<asp:BoundColumn DataField="PID" HeaderText="Service"  HeaderStyle-Width="12%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Select" HeaderStyle-Width="3%">
						<ItemTemplate>
							<asp:CheckBox ID="chkService" Runat="Server" OnCheckedChanged="checked_clicked" AutoPostBack="true" ></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="PDESC" HeaderText="Service"  HeaderStyle-Width="12%"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Charge Per Tran." ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
						<ItemTemplate>
							<asp:TextBox ID="txtCharge" Runat="server" CssClass="MINITEXT" Text="0.00" MaxLength="8"></asp:TextBox>
							<input type="hidden" id="hidCharge" name="hidCharge" value="0.00" runat="server"/>
							<asp:RegularExpressionValidator ID="revCharge" Runat="Server" ControlToValidate="txtCharge" Display="None" ErrorMessage="Charge should be in currency value" ValidationExpression="[0-9.,]{1,8}"></asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
				</asp:panel>
				<!-- Payment Data Grid Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trMain">
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnSave" Runat="Server" Text="Update" CssClass="BUTTON"></asp:Button>&nbsp;
				<input type="button" id="btnReset" runat="Server" value="Clear"  class="BUTTON"/>&nbsp;
				<input type="button" runat="server" value="Back" id="btnBack" onclick="fncBack('List')" class="BUTTON"/>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trBack" Visible="False">
			<asp:TableCell ColumnSpan="2">
				<input type="button" runat="Server" value="Back" onclick="fncBack('Appr');" class="BUTTON" id="btnBackToView" name="Button1" />
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
        &nbsp; &nbsp;&nbsp; &nbsp;
	<!-- Form Table Ends Here -->
	
	<!-- Confirmation Table Starts here -->
	<asp:Table ID="tblConfirm" Runat="Server" CellPadding="8" CellSpacing="0" BorderWidth="0" Width="100%" Visible="False">
		<asp:TableRow ID="trCOrgId">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Organization Id" CssClass="LABEL" ID="Label79"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCOrgId" CssClass="SMALLTEXT" ReadOnly="True" Runat="Server"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Organization Name" CssClass="LABEL" ID="Label45"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCOrgName" Runat="server" CssClass="LARGETEXT" MaxLength="40" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Wrap="False">
				<asp:Label Runat="Server" Text="Organization Type" CssClass="LABEL" ID="Label62"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Wrap="False">
				<asp:Textbox id="txtCH2H" Runat="server" CssClass="LARGETEXT" MaxLength="40" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Region Name" ID="Label81"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCRegion" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="35" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="True">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Branch Code" ID="Label46"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCBrCode" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="4" Text="0000" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False" VerticalAlign="Top">
				<asp:Label Runat="Server" Text="Address" CssClass="LABEL" ID="Label47"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox ID="txtCAddress" Runat="server" CssClass="BIGTEXT" Rows="3" TextMode="MultiLine" MaxLength="200" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="State" ID="Label48"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblState" Runat="Server" CssClass="LABEL"></asp:Label>&nbsp;
				<asp:Label Visible="false" Runat="Server" CssClass="LABEL" Text="If Others," ID="Label85"></asp:Label>&nbsp;
				<asp:TextBox Visible="false" ID="txtCState" Runat="Server" CssClass="MEDIUMTEXT" ReadOnly="True"></asp:TextBox>			
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Post Code" ID="Label49"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCPinCode" Runat="Server" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Country" ID="Label50"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCCOuntry" Runat="server" CssClass="SMALLTEXT" ReadOnly="True"></asp:Textbox>				
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 1" ID="Label51"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCPhone1" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Phone 2" ID="Label52"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCPhone2" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Fax" ID="Label53"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCFax" Runat="server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Email" ID="Label54"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCEmail" Runat="server" CssClass="LARGETEXT" MaxLength="150" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Website" ID="Label55"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtCURL" Runat="Server" CssClass="LARGETEXT" MaxLength="50" ReadOnly="True"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCLogo">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Organization Logo" ID="Label56"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblLogo" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCNoEmployee">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No. of Employees" ID="Label82"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:TextBox ID="txtCEmployees" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCNoGroup">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="No. of Groups" ID="Label86"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCGroups" Runat="Server" CssClass="MINITEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCCorpAdmin">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator" ID="Label57"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Textbox id="txtCContactPerson" Runat="server" CssClass="LARGETEXT" MaxLength="60" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCCorpAdminIC">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator IC No" ID="Label58"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCContactPerIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCCorpAuth">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL"  ID="lblCCustomerAdmin"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%" Wrap="False">
				<asp:TextBox ID="txtCCustomerAdmin" Runat="Server" CssClass="LARGETEXT" MaxLength="40" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCCorpAuthIC">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Corporate Administrator IC No" ID="Label60"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox ID="txtCCustomAdminIC" Runat="Server" CssClass="SMALLTEXT" MaxLength="12" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Stop Payment Charge" ID="Label64"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtCStopCharges" MaxLength="8" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Subscription Fees" ID="Label65"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:TextBox Runat="server" ID="txtCAnnualFee" MaxLength="8" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
		    <asp:TableCell>
		        <asp:Label runat="server" CssClass="LABEL" Text = "Subscription Fee's Payment Mode" ID="lblCTSubscriptionMode"></asp:Label>
		    </asp:TableCell>
		    <asp:TableCell>
		        <asp:Label runat="server" CssClass="LABEL" ID="lblCSubscriptionFeePaymentMode"></asp:Label>
		    </asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Business Registration No" ID="Label66"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">			
				<asp:Textbox id="txtCBusReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:Textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Tax Registration No" ID="Label70"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:textbox id="txtCTaxReg" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25" ReadOnly="True"></asp:textbox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Privilege Submission" ID="Label72"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:Label ID="lblPrvSub" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trCEncryptionKey">
		    <asp:TableCell><asp:Label Runat="Server" CssClass="LABEL" Text="Encryption Key" ID="Label42"></asp:Label></asp:TableCell>
		    <asp:TableCell><asp:Label Runat="Server" CssClass="LABEL" Text="***" ID="lblCEncryptionKey"></asp:Label></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Status" ID="Label73"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblStatus" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false" >
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Use&nbsp;Token" ID="Label33"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:Label ID="lblUseToken" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow Visible="false">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Verification" CssClass="LABEL" ID="Label78"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:Label ID="lblVerify" Runat="Server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Label Runat="Server" CssClass="BLABEL" Text="Payment Services" ID="Label74"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2">
				<!-- Payment Data Grid Starts Here -->
				<asp:panel ID="pnlGrid2" CssClass="GridDivNoScroll" runat="server" width="40%">
				<asp:DataGrid cssclass="Grid" Runat="Server" ID="dgCPayService" AllowPaging="False" PagerStyle-HorizontalAlign="Center"  BorderWidth="0" GridLines="Both"  HeaderStyle-CssClass="GridHeaderStyle" 
				AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" width="100%" HeaderStyle-HorizontalAlign="Left" 
				AutoGenerateColumns="False">
					<Columns>
					<asp:BoundColumn DataField="PID" HeaderText="Service"  HeaderStyle-Width="12%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn DataField="PDESC" HeaderText="Service" HeaderStyle-Width="12%"></asp:BoundColumn>
					<asp:BoundColumn DataField="PCHRG" HeaderText="Service" HeaderStyle-Width="12%"></asp:BoundColumn>
					<asp:BoundColumn DataField="HCHRG" ReadOnly="True" Visible="False"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
				</asp:panel>
				<!-- Payment Data Grid Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trConfirm">
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnModify" Runat="Server" Text="Confirm" CssClass="BUTTON"></asp:Button>&nbsp;
				<input type="button" runat="Server" value="Back" onclick="window.history.back();" class="BUTTON" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trNew" Visible="False">
			<asp:TableCell ColumnSpan="2">
				<input type="button" runat="Server" value="Back" onclick="fncBack('List');" class="BUTTON" />
			</asp:TableCell>
		</asp:TableRow>
		
	</asp:Table>
	<!-- Confirmation Table Starts here --> 
	
	<!-- Hidden Box Starts Here -->
	<input id="hVerify" type="hidden" name="hVerify" runat="Server" />
	<input id="hOrgId" type="hidden" name="hOrgId" runat="Server" />
	<input id="hcmbState" type="hidden" name="hcmbState" runat="Server" />
	<input id="hchkStatus" type="hidden" name="hchkStatus" runat="Server" />
	<input id="hchkPrivilege" type="hidden" name="hchkPrivilege" runat="Server" />
	<input id="hIVerify" type="hidden" name="hIVerify" runat="Server" />
	<input id="hStatus" type="hidden" name="hStatus" runat="Server" />
	<input id="hMode" type="hidden" name="hMode" runat="server" />
	<input id="hchkUseToken" type="hidden" name="hchkUseToken" runat="server" />
	<input id="hEncryptionKey" type="hidden" name="hEncryptionKey" runat="server" />
	<input id="hSubscriptionFeePaymentMode" type="hidden" name="hSubscriptionFeePaymentMode" runat="server" />
    <input id="hH2H" type="hidden" runat="server" />	
	<!-- Hidden Box Ends Here -->
	
	<!-- Audit Trail Box Starts Here -->
	<asp:TextBox ID="AtxtAddr" TextMode="MultiLine" Runat="server" Visible="False"></asp:TextBox>
	<input type="hidden" runat="server" name="AhGroups" id="AhGroups" />
	<input type="hidden" runat="server" name="AhCA" id="AhCA" />
	<input type="hidden" runat="Server" name="AhSA" id="AhSA" />
	<input type="hidden" runat="server" name="AhStopChrg" id="AhStopChrg" />
	<input type="hidden" runat="server" name="AhAnnFees" id="AhAnnFees" />
	<input type="hidden" runat="server" name="hAPriv" id="hAPriv" />
	<input type="hidden" runat="server" name="hAVerify" id="hAVerify" />
	<!-- Audit Trail Box Ends Here -->
			
	<!-- Form Validations Starts Here -->
	<asp:ValidationSummary ID="vsOrganisation" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
	<asp:RequiredFieldValidator ID="rfvOrgName" Runat="Server" ControlToValidate="txtOrgName" Display="None" ErrorMessage="Organisation Name cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvAddress" Runat="Server" ControlToValidate="txtAddress" Display="None" ErrorMessage="Address cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvPinCode" Runat="Server" ControlToValidate="txtPincode" Display="None" ErrorMessage="Postcode cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvPhone1" Runat="Server" ControlToValidate="txtPhone1" Display="None" ErrorMessage="Phone 1 cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvContactPerson" Runat="Server" ControlToValidate="txtContactPerson" Display="None" ErrorMessage="Corporate Administrator cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvContactPersonIC" Runat="Server" ControlToValidate="txtContactPerIC" Display="None" ErrorMessage="Corporate Administrator IC cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCorporateAuth" Runat="Server" ControlToValidate="txtCustomerAdmin" Display="None" ErrorMessage="Corporate Authorizer cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCorporateAuthIC" Runat="Server" ControlToValidate="txtCustomAdminIC" Display="None" ErrorMessage="Corporate Authorizer IC cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvBusiRegNo" Runat="Server" ControlToValidate="txtBusReg" Display="None" ErrorMessage="Business Registration No cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvStopCharge" Runat="Server" ControlToValidate="txtStopCharges" Display="None" ErrorMessage="Stop Payment cannot be blank"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvGroup" Runat="Server" ControlToValidate="txtGroups" Display="None" ErrorMessage="No. of Groups cannot be blank"></asp:RequiredFieldValidator>
	<asp:RangeValidator ID="rngGroup" Runat="server" ControlToValidate="txtGroups" Display="None" MinimumValue="1" MaximumValue="99" Type="Integer" ErrorMessage="No Groups should be between 1-99"></asp:RangeValidator>
	<asp:RangeValidator ID="rgvState" Runat="Server" ControlToValidate="cmbState" MaximumValue="16" MinimumValue="1" Type="Integer" Display="None" ErrorMessage="Select State"></asp:RangeValidator>
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
	
