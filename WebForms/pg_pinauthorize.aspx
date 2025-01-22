<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_PinAuthorize" CodeFile="PG_PinAuthorize.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
       <script type="text/javascript" src="../include/common.js"></script>
    
    <script type="text/javascript" language="JavaScript">
		function fncClear()
		{
			window.history.back();
		}
    </script>
    
    <!-- Main Table Starts Here -->
	 <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Pin Mailer - Authorization</asp:Label></td>
      </tr>
      <tr>
         <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
    </table>
	<asp:Table ID="tblMain" CellPadding="8" CellSpacing="0" Runat="Server" Width="100%">
	<asp:TableRow>
		<asp:TableCell Width="10%">
			<asp:Label Runat="server" CssClass="LABEL" Text="Select Status"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="80%">
			<asp:RadioButtonList ID="rdStatus" Runat="server" CssClass="LABEL" RepeatDirection="Horizontal">
				<asp:ListItem Value="P" Selected="True">Pending</asp:ListItem>
				<asp:ListItem Value="A">Approve</asp:ListItem>
				<asp:ListItem Value="R">Reject</asp:ListItem>
			</asp:RadioButtonList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="10%">
			<asp:Label Runat="server" CssClass="LABEL" Text="Show&nbsp;within"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="80%">&nbsp;
			<asp:TextBox ID="txtDays" CssClass="MINITEXT" MaxLength="3" Runat="server" Text="3"></asp:TextBox>&nbsp;
			<asp:Label Runat="server" CssClass="LABEL" Text="Days"></asp:Label>&nbsp;
			<asp:Button ID="btnShow"   Text="Show" Runat="Server" CssClass="BUTTON"></asp:Button>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Main Table Ends Here -->
	
	<!-- Datagrid Starts Here -->
	<asp:Table ID="tblGrid" Runat="server" Width="100%" CellPadding="12" CellSpacing="0" Visible="False">
	<asp:TableRow><asp:TableCell ColumnSpan="2" Width="90%" ID="cGrid" runat =server>
	<asp:panel ID="pnlGrid" CssClass="GridDivNoScroll" runat="server">
	<asp:DataGrid ID="dgRequisition" Runat="server" AllowPaging="False" cssclass="Grid"
		PagerStyle-HorizontalAlign="Center"
		HeaderStyle-CssClass="GridHeaderStyle"
		ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" width="100%" headerStyle-HorizontalAlign="left"
		AutoGenerateColumns="False" BorderWidth=0 GridLines=None>
		<Columns>
			<asp:BoundColumn Datafield="REQID" HeaderText="" HeaderStyle-Width="0%" ReadOnly="True" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn Datafield="REQUID" HeaderText="" HeaderStyle-Width="0%"  ReadOnly="True" Visible="False"></asp:BoundColumn>
			<asp:BoundColumn DataField="ORGID" HeaderText="Org. Id" ></asp:BoundColumn>
			<asp:BoundColumn DataField="NAME" HeaderText="Organization Name" ></asp:BoundColumn>
			<asp:BoundColumn DataField="VERIFY" HeaderText="Verification" ></asp:BoundColumn>
			<asp:BoundColumn DataField="TYPE" HeaderText="User Type" ></asp:BoundColumn>
			<asp:BoundColumn DataField="CODE" HeaderText="Code Type" ></asp:BoundColumn>
			<asp:TemplateColumn  ItemStyle-Width="10%" HeaderText="Request Remarks">
				<ItemTemplate>
					<asp:TextBox ID="txtRemarks" Runat="Server" CssClass="SMALLTEXT" TextMode="MultiLine" ReadOnly="True" Text=<%#DataBinder.Eval(Container.DataItem,"REQREMARKS")%>></asp:TextBox>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:BoundColumn DataField="REQDATE" HeaderText="Request Date" HeaderStyle-Width="10%"></asp:BoundColumn>
			<asp:TemplateColumn  ItemStyle-Width="10%" HeaderText="Action">
				<ItemTemplate>
					<asp:DropDownList Runat="server" ID="ddlAction" CssClass="SMALLTEXT">
						<asp:ListItem Value="">Select</asp:ListItem>
						<asp:ListItem Value="A">Approve</asp:ListItem>
						<asp:ListItem Value="R">Reject</asp:ListItem>
					</asp:DropDownList>
				</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn ItemStyle-Width="10%" HeaderText="Remarks">
				<ItemTemplate>
					<asp:TextBox ID="txtARemarks" Runat="Server" CssClass="SMALLTEXT" TextMode="MultiLine"></asp:TextBox>
                    <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtARemarks" Display="None"
                                            ErrorMessage="Remarks Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>

				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid>
	</asp:panel>
	</asp:TableCell></asp:TableRow>
	<asp:TableRow ID="trSelect">
		<asp:TableCell Width="90%" HorizontalAlign="Center" ColumnSpan="2">
			<asp:Button ID="btnSelect" Runat="server" Text="Approve All" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
			<asp:Button ID="btnUnSelect" Runat="server" Text="Unapprove All" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
			<asp:Button ID="btnConfirm" Runat="server" Text="Confirm" CssClass="BUTTON"></asp:Button>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trAuthCode">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Validation Code"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtAuthCode" CssClass="MEDIUMTEXT" Runat="Server" TextMode="Password" MaxLength="24"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trSubmit">
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnSubmit" CssClass="BUTTON" Runat="Server" Text="Submit"></asp:Button>&nbsp;
				<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncClear();">
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Datagrid Ends Here -->
	
	<!-- View Datagrid Starts Here -->
	<asp:Table ID="tblView" Runat="server" Width="100%" CellPadding="12" CellSpacing="0" Visible="False">
	<asp:TableRow>
		<asp:TableCell>
		<asp:panel ID="pnlGrid2" CssClass="GridDivNoScroll" runat="server">
			<asp:DataGrid cssclass="Grid" ID="dgView" Runat="server" AllowPaging="True" PageSize="10" PagerStyle-Mode="NumericPages"
				PagerStyle-HorizontalAlign="Center" OnPageIndexChanged="prPageChange"
				HeaderStyle-CssClass="GridHeaderStyle"
				AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle"  width="100%" HeaderStyle-HorizontalAlign="Left"
				AutoGenerateColumns="False" GridLines=None>
			<Columns>
					<asp:BoundColumn DataField="ORGID" HeaderText="Org. Id" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%"></asp:BoundColumn>
					<asp:BoundColumn DataField="NAME" HeaderText="Organization Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="18%"></asp:BoundColumn>
					<asp:BoundColumn DataField="VERIFY" HeaderText="Verification" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="5%"></asp:BoundColumn>
					<asp:BoundColumn DataField="TYPE" HeaderText="User Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"></asp:BoundColumn>
					<asp:BoundColumn DataField="CODE" HeaderText="Code Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Request Remarks">
						<ItemTemplate>
							<asp:TextBox Runat="Server" CssClass="SMALLTEXT" TextMode="MultiLine" ReadOnly="True" Text=<%# DataBinder.Eval(Container.DataItem,"REQREMARKS") %>></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="REQDATE" HeaderText="Request Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%" HeaderText="Remarks">
						<ItemTemplate>
							<asp:TextBox Runat="Server" CssClass="SMALLTEXT" TextMode="MultiLine" ReadOnly="True" Text=<%#DataBinder.Eval(Container.DataItem,"APPRREMARKS")%>></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn DataField="APPRDATE" HeaderText="Action Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"></asp:BoundColumn>
			</Columns>
			</asp:DataGrid>
			</asp:panel>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- View Datagrid Ends Here -->
	
	<!-- Validators Starts Here -->
	<asp:RequiredFieldValidator ID="rfvDays" Runat="Server" ControlToValidate="txtDays" Display="None" ErrorMessage="No Days cannot be blank."></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="revDays" Runat="Server" ControlToValidate="txtDays" ValidationExpression="\d{1,3}" ErrorMessage="No of Days must be numeric value" Display="None"></asp:RegularExpressionValidator>
	<asp:RequiredFieldValidator ID="rfvAuth" Runat="Server" ControlToValidate="txtAuthCode" ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
	<asp:ValidationSummary ID="vsPinRequest" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
	<!-- Validators Ends Here -->
    
</asp:Content>