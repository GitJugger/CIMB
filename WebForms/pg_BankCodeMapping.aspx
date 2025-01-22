<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_BankCodeMapping" CodeFile="pg_BankCodeMapping.aspx.vb"     MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
  
        
      <link href="../Include/Styles.css" rel="stylesheet" type="text/css" />
      <script type="text/javascript" src="../include/common.js"></script>
      <script type="text/javascript">
        function fncBackToView()
        {
    
            window.location = 'PG_ViewOrganisation.aspx?Req=' + '<%=MaxPayroll.mdConstant.enmViewOrganizationReqType.BankCodeMapping.ToString%>'
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
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Bank Code Mapping</asp:Label></td>
      </tr>
  </table>
	<table width="100%" cellpadding="8" cellspacing="0">
	    <tr >
	        <td height ="5px" width="100%" ></td>
	    </tr>
	    <tr>
	        <td width="100%">
		        <!-- Form Table Starts Here -->
		        <table width="100%" cellpadding="2" cellspacing="0" border="0">
		            
		            <tr>
		                <td colspan="3"><asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label></td>
		            </tr>
		            <tr>
		              <td width="20%"></td>
		              <td></td>
		              <td></td>
		            </tr>
		            <tr>
			            <td><asp:Label ID="lbltOrganization" Runat="Server" Text="Organization" CssClass="LABEL"></asp:Label></td>
			            <td colspan="2">
				            <asp:Label ID="lblOrganization" Runat="Server" CssClass="LABEL"></asp:Label>
			            </td>
		            </tr>
		            <tr>
		                <td colspan="3"><hr style="width:100%" /></td>
		            </tr>
		            <tr>
		                <td colspan="3">
		                <asp:panel id="pnlGrid" CssClass="GridDivNoScroll" runat="server"> 
		                    <asp:DataGrid Runat="Server" ID="dgBankCodeMapping" AllowPaging="false" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" BorderWidth="0" GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="left" AutoGenerateColumns="False" HeaderStyle-CssClass="GridHeaderStyle"  ShowFooter="false" AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" OnPageIndexChanged="Page_Change">     
            				    <Columns>
            		                <asp:BoundColumn DataField="BankCode" headertext="Bank Code"></asp:BoundColumn>
            		                <asp:BoundColumn DataField="BankName" HeaderText="Bank Name"></asp:BoundColumn>		        
            		                <asp:TemplateColumn HeaderText="Customer Defined Bank Code">
            		                    <ItemTemplate>
            		                        <asp:TextBox ID="txtCustBankCode" runat="server" CssClass="LARGETEXT" MaxLength="5"></asp:TextBox>
            		                    </ItemTemplate>
            		                </asp:TemplateColumn>  
            		                <asp:BoundColumn DataField="BankID" Visible="false"></asp:BoundColumn>
            		                <asp:BoundColumn DataField="CustomerBankCode" visible="false"></asp:BoundColumn>
            				    </Columns>
				            </asp:DataGrid>
				            </asp:panel> 
		                </td>
		            </tr>
		            <tr>
		                <td colspan="3"><hr style="width:100%" /></td>
		            </tr>
		            <tr>
		                <td colspan="3"><asp:Button ID="btnSave" runat="server" Width="100" Text="Save" />&nbsp;<asp:Button ID="btnReset" runat="server" Text="Reset" Width="100"/>&nbsp;<input type="button" id="btnBack" value="Back To View" onclick="fncBackToView()" />&nbsp;<asp:Button ID="btnDefault" runat="server" Text="Default" Width="100"/> </td>
		            </tr>
		        </table>
		        <!-- Form Table Ends Here -->
	        </td>
	    </tr>
	</table>
	<!-- Main Table Ends Here -->

</asp:Content>