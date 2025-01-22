<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_BankAccount" CodeFile="PG_BankAccount.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
     <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			window.location.href = "PG_ViewOrganisation.aspx?Req=Bank";
		}
		//function fncShowAccOrgCode(sId, sAccId, sAccNo)
		//{
		//  sPath = 'pg_BankOrgCode.aspx?OrgId=' + sId + '&AccId=' + sAccId + '&AccNo=' + sAccNo
		//  window.showModalDialog(sPath);
  //      }
        function fncShowAccOrgCode(sId, sAccId, sAccNo) {
            sPath = 'pg_BankOrgCode.aspx?OrgId=' + sId + '&AccId=' + sAccId + '&AccNo=' + sAccNo

            var w = 800;
            var h = 500;
            var left = (screen.width / 2) - (w / 2);
            var top = (screen.height / 2) - (h / 2);
            var targetWin = window.open(sPath, '_blank', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + top + ', left=' + left);

        }
    </script>
    
 	

	<!-- Main Table Starts Here -->
	 <table id="tblMain" runat="server" cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server" Text=""></asp:Label></td>
      </tr>
      <tr>
         <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
    </table>
	<!-- Main Table Ends Here -->
	<!-- Form Table Starts Here -->
	<asp:Table Width="100%" ID="tblForm" CellPadding="8" CellSpacing="0" Runat="Server">
			<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="3">
			<asp:panel ID="pnlGrid" runat="server" CssClass="GridDivNoScroll" Width="100%">
				<asp:DataGrid Width="100%" cssclass="Grid" ID="dgBankAccts" Runat="Server" AllowPaging="true" AllowSorting="False" AutoGenerateColumns="False" DataKeyField="ACCID" CellPadding="3" OnDeleteCommand="dgBankAccts_Delete"
				PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" BorderWidth="0"
				HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle"  HeaderStyle-HorizontalAlign="left" PagerStyle-Mode="NumericPages">
				<Columns>
					<asp:BoundColumn Datafield="ACCID" HeaderText="From"  ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn Datafield="ACCNO"   ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="No" HeaderStyle-Width="5%" >
						<ItemTemplate>
							<asp:Label ID="lblNo" Runat="Server" CssClass="LABEL"></asp:Label>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Account Name" HeaderStyle-Width="35%">
						<ItemTemplate>
							<asp:TextBox ID="txtAccName" Runat="Server" CssClass="LARGETEXT" MaxLength="50" Text= '<%# Eval("ACCNAME") %>'></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvAccName" Runat="Server" Display="None" ErrorMessage="Account Name Cannot be blank" ControlToValidate="txtAccName"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator
                                            ID="revAccName" runat="server" ControlToValidate="txtAccName" Display="None"
                                            ErrorMessage="Acc Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Account Number" HeaderStyle-Width="20%" >
						<ItemTemplate>
							<asp:TextBox ID="txtAccNo" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="14" Text='<%# Eval("ACCNO") %>'></asp:TextBox>
							<asp:RequiredFieldValidator ID="rfvAccNo" Runat="Server" Display="None" ErrorMessage="Account No Cannot be blank" ControlToValidate="txtAccNo"></asp:RequiredFieldValidator>
                             <asp:RegularExpressionValidator
                                            ID="revAccNo" runat="server" ControlToValidate="txtAccNo" Display="None"
                                            ErrorMessage="Acc No Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Payment Type" HeaderStyle-Width="20%">
						<ItemTemplate>
						    <asp:TextBox ID="txtPaymentType" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="14" Text='<%# Eval("PaySerDesc") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="revPaytype" runat="server" ControlToValidate="txtPaymentType" Display="None"
                                            ErrorMessage="Payment Type Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
							<asp:DropDownList ID="ddlPaymentType"   Runat="Server" CssClass="MEDIUMTEXT" DataValueField="PaySer_Id"  DataTextField="PaySer_Desc" DataSource='<%# procPmtTypeList() %>'></asp:DropDownList>
							
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Bank Name" HeaderStyle-Width="20%">
						<ItemTemplate>
						<asp:TextBox ID="txtBankName"  Runat="Server" CssClass="MEDIUMTEXT" MaxLength="14" Text='<%# Eval("BankDesc") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator
                                            ID="revBankName" runat="server" ControlToValidate="txtBankName" Display="None"
                                            ErrorMessage="Bank Name Accepts Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
							<asp:DropDownList ID="ddlBankName" Runat="Server" CssClass="MEDIUMTEXT" DataValueField="BankId"  DataTextField="BankName" DataSource='<%# procBankNameList() %>' ></asp:DropDownList>
		
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Bank Org. Code" HeaderStyle-Width="20%">
						<ItemTemplate>
						 <a href="javascript:fncShowAccOrgCode('<%# Request.QueryString("Id") %>','<%# Eval("ACCID") %>','<%# Eval("ACCNO") %>')"><asp:label ID="lblAddOrgCode" runat="server">Add/Modify</asp:label></a>
							<asp:TextBox ID="txtOrgCode" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="14" Text='<%# Eval("BnkOrgCode") %>' Visible="false"></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Account Type" HeaderStyle-Width="20%">
						<ItemTemplate>
						   <asp:DropDownList ID="ddlAccType" runat="server" >
						      <asp:ListItem Text="Credit" Value="0"></asp:ListItem>
						      <asp:ListItem Text="Debit" Value="1"></asp:ListItem>
						   </asp:DropDownList>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete" HeaderText="Action" HeaderStyle-Width="10%" ></asp:ButtonColumn>
			    <asp:BoundColumn DataField="IsDrAccType" Visible="false"></asp:BoundColumn>
				</Columns>
				</asp:DataGrid>
				</asp:panel>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow><asp:TableCell ColumnSpan="3">&nbsp;</asp:TableCell> </asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="10%" Wrap="False">	
				<asp:Label CssClass="LABEL" Runat="Server" Text="Select Subscription Fees Debit A/C"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="25%" HorizontalAlign="Left">
				<asp:DropDownList ID="ddlPrimary" Runat="server"  Width="350px"></asp:DropDownList>
			</asp:TableCell>
			<asp:TableCell Width="15%" HorizontalAlign="Left">
				<asp:Button ID="btnPrimary" Runat="server" CssClass="BUTTON" Text="Update" CausesValidation="False"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow><asp:TableCell ColumnSpan="3">&nbsp;</asp:TableCell> </asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="3">
				<input type="button" runat="server" value="Back To View" class="BUTTON" onclick="fncBack();"/>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
	<br/>
	<input type="hidden" runat="server" id="hAction" name="hAction"/>
	<input type="hidden" runat="server" id="hStatus" name="hStatus"/>
	<!-- Insert Table Starts Here -->
	<!-- Form Table Ends Here -->
	<asp:ValidationSummary Runat="Server" ShowMessageBox="True" ShowSummary="False" ID="valSummary" EnableClientScript="True" HeaderText="Please Incorporate The Below Validations,"></asp:ValidationSummary>

</asp:Content>