<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Product" CodeFile="pg_Product.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
        
      <script type="text/javascript" src="../include/common.js"></script>
      <script type="text/javascript">
        function fncBackToView()
        {
            window.location = 'pg_SearchProduct.aspx'
            //window.location = 'pg_SearchProduct.aspx?Req=' + '<%=MaxPayroll.mdConstant.enmViewOrganizationReqType.BankCodeMapping.ToString%>'
        }
         function fncReset()
        {
            alert(document.forms[0].hPath.Value)
            window.location = document.forms[0].hPath.Value 
        }
      </script>
	<!-- Main Table Starts Here -->
	<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="Product Creation" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
  </table>
	<table width="100%" cellpadding="8" cellspacing="0">
	    <tr>
	        <td width="100%">
		        <!-- Form Table Starts Here -->
		        <table width="100%" cellpadding="2" cellspacing="0" border="0">
		            <tr>
		                <td colspan="2"><asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label></td>
		            </tr>
		            <tr>
			            <td width="20%"><asp:Label ID="lbltProductName" Runat="Server" Text="Product Name" CssClass="LABEL"></asp:Label></td>
			            <td>
				            <asp:TextBox ID="txtProductName" runat="server" CssClass="LABEL" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvProductName" runat="server" ControlToValidate="txtProductName" Display="None" ErrorMessage="Product Name Cannot Be Blank."  ValidationGroup="Page"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtProductName" ValidationExpression="^[\w\-\s]+$" ErrorMessage="ProductName Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>

			            </td>
		            </tr>
		            <tr>
			            <td><asp:Label ID="lbltProductLink" Runat="Server" Text="Product Link" CssClass="LABEL"></asp:Label></td>
			            <td>
				            <asp:TextBox ID="txtProductLink" runat="server" CssClass="LABEL" MaxLength="200"></asp:TextBox>
                             
			            </td>
		            </tr>
		            <tr>
			            <td><asp:Label ID="lbltStatus" Runat="Server" Text="Status" CssClass="LABEL"></asp:Label></td>
			            <td>
				            <asp:RadioButtonList ID="rblStatus" runat="server" RepeatColumns="2">
				                <asp:ListItem Text="Active" Selected="true" Value="A"></asp:ListItem>
				                <asp:ListItem Text="Inactive" Value="I"></asp:ListItem>
				            </asp:RadioButtonList>
			            </td>
		            </tr>
		            <tr>
		                <td colspan="2"><asp:Button ID="btnSave" runat="server" Width="100" Text="Save"  ValidationGroup="Page" />&nbsp;<asp:Button ID="btnReset" runat="server" Text="Clear" Width="100"/>&nbsp;<input type="button" id="btnBack" value="Back" onclick="fncBackToView()" runat="server" Width="100" /></td>
		            </tr>
		        </table>
		        <!-- Form Table Ends Here -->
	        </td>
	    </tr>
	</table>
	<!-- Main Table Ends Here -->
	<asp:ValidationSummary ID="vsProduct" ValidationGroup="Page" runat="server" ShowMessageBox="true" ShowSummary="false" />

</asp:Content>