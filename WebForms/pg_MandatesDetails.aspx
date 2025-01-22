<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_MandatesDetails" CodeFile="pg_MandatesDetails.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    
    <script type="text/javascript" src="../include/common.js"></script>
    
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			window.location.href = "PG_ApprMatrix.aspx?Mode=" + document.forms[0].ctl00$cphContent$hidMode.value ;
			//window.history.back();
		}
		function fncClear()
		{
			window.location.href = "PG_ApprMatrix.aspx?Mode=Edit" ;
		}
    </script>
    <script type="text/javascript" src="../include/common.js"></script>
 
    <input type="hidden" id="hidMode" runat="server" />
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Mandates Details</asp:Label></td>
      </tr>
      <tr>
         <td><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
  </table>

		<!-- Datagrid Table Starts Here -->
		<table cellpadding="8" cellspacing="0" width="100%">
		    <tr>
		        <td style="width:30%"><asp:Label ID="lblTRefNo" runat="server" Text="Reference No."></asp:Label></td>
		        <td style="width:70%"><asp:TextBox ID="txtRefNo" runat="server" CssClass="LABEL" MaxLength="30" Width="250px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvRefNo" runat="server" ErrorMessage="Reference No. cannot be blank" ControlToValidate="txtRefNo"  Display="None"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNewRefNo" runat="server"></asp:Label></td>      
		    </tr>
		    <tr>
		        <td><asp:Label ID="lblTBnkOrgCode" runat="server" Text="Bank Organization Code"></asp:Label></td>
		        <td><asp:TextBox ID="txtBnkOrgCode" runat="server" CssClass="LABEL" MaxLength="4" Width="250px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvBnkOrgCode" runat="server" ErrorMessage="Bank Organization Code cannot be blank" ControlToValidate="txtBnkOrgCode" Display="None"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="revBnkOrgCode" runat="server" ErrorMessage="Incorrect Organization Code" ControlToValidate="txtBnkOrgCode" ValidationExpression="^[a-zA-Z0-9]{4}$" Display="None"></asp:RegularExpressionValidator><asp:Label ID="lblNewBnkOrgCode" runat="server"></asp:Label></td>
		    </tr>
		    <tr>
		        <td><asp:Label ID="lblTAccNo" runat="server" Text="Account No."></asp:Label></td>
		        <td><asp:TextBox ID="txtAccNo" runat="server" MaxLength="16" CssClass="LABEL" Width="250px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvAccNo" runat="server" ErrorMessage="Account No. cannot be blank" ControlToValidate="txtAccNo" Display="None"></asp:RequiredFieldValidator><asp:CompareValidator ID="cvAccNoDataType" runat="server" ErrorMessage="Account No. must be numeric" ControlToValidate="txtAccNo" Display="None" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator><asp:Label ID="lblNewAccNo" runat="server"></asp:Label></td>
		    </tr>
            <tr>
		        <td><asp:Label ID="lblTCustName" runat="server" Text="Customer Name"></asp:Label></td>
		        <td><asp:TextBox ID="txtCustName" runat="server" CssClass="LABEL" MaxLength="20" Width="250px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvCustName" runat="server" ErrorMessage="Customer Name cannot be blank" ControlToValidate="txtCustName" Display="None"></asp:RequiredFieldValidator><asp:Label ID="lblNewCustName" runat="server"></asp:Label></td>
            </tr>
            <tr>
                <td><asp:Label ID="lblTLimitAmt" runat="server" Text="Limit Amount"></asp:Label></td>
		        <td><asp:TextBox ID="txtLimitAmt" MaxLength="15" runat="server" CssClass="LABEL" Width="250px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvLimitAmt" runat="server" ErrorMessage="Limit Amount cannot be blank" ControlToValidate="txtLimitAmt" Display="None"></asp:RequiredFieldValidator><asp:CompareValidator ID="cvLimitAmt" runat="server" ErrorMessage="Limit Amount must be currency" ControlToValidate="txtLimitAmt" Display="None" Type="Currency" Operator="DataTypeCheck"></asp:CompareValidator><asp:Label ID="lblNewLimitAmt" runat="server"></asp:Label></td>
		    </tr>
		    <tr>
		        <td><asp:Label ID="lblFrequency" runat="server" Text="Frequency"></asp:Label></td>
		        <td><asp:DropDownList ID="ddlFrequency" runat="server" Width="250px"></asp:DropDownList><asp:RequiredFieldValidator ID="rfvFrequency" runat="server" ErrorMessage="Please select the Frequency" ControlToValidate="ddlFrequency" Display="None"></asp:RequiredFieldValidator><asp:Label ID="lblNewFrequency" runat="server"></asp:Label></td>
		    </tr>
		    <tr>
		        <td><asp:Label ID="lblFrequencyLimit" runat="server" Text="Frequency Limit"></asp:Label></td>
		        <td><asp:TextBox ID="txtFrequencyLimit" runat="server" CssClass="LABEL" MaxLength="4" Width="250px"></asp:TextBox><asp:RequiredFieldValidator ID="rfvFrequencyLimit" runat="server" ErrorMessage="Frequency Limit cannot be blank" ControlToValidate="txtFrequencyLimit" Display="None"></asp:RequiredFieldValidator><asp:CompareValidator ID="cvFrequencyLimit" runat="server" ErrorMessage="Frequency Limit must be numeric" ControlToValidate="txtFrequencyLimit" Display="None" Type="Integer" Operator="DataTypeCheck"></asp:CompareValidator><asp:Label ID="lblNewFrequencyLimit" runat="server"></asp:Label></td>
		    </tr>
		    <tr>
		        <td><asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label></td>
		        <td><asp:RadioButtonList ID="rdStatus" Runat="Server" CssClass="LABEL" RepeatDirection="Horizontal"></asp:RadioButtonList><asp:Label ID="lblNewStatus" runat="server"></asp:Label></td>
                           
		        
		    </tr>
		    <tr>
		        <td colspan="2"><asp:Button ID="btnSave" runat="server" Text="Save" CssClass="BUTTON" />&nbsp;<input type="reset" id="btnReset" value="Clear" runat="server" class="BUTTON" />&nbsp;<asp:Button id="btnBack" runat="server" CausesValidation="false" Text="Back" CssClass="BUTTON" Visible="false" /></td>
		    </tr>
		</table>
    <asp:ValidationSummary ID="vsSummary" runat="server" ShowSummary="false" ShowMessageBox="true" />
		

</asp:Content>
