<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_PinRequisition" CodeFile="PG_PinRequisition.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

     <script type="text/javascript" src="../include/common.js"></script>
    
    <script language="JavaScript">
		function fncClear()
		{
			window.history.back();
		}
    </script>

	<!-- Main Table Starts Here -->
	<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Pin Mailer - Requisition</asp:Label></td>
      </tr>
      <tr>
         <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
  </table>
	<asp:Table ID="tblMain" CellPadding="8" CellSpacing="0" Runat="Server" Width="100%">
	<asp:TableRow>
		<asp:TableCell Width="10%">
			<asp:Label Runat="server" CssClass="LABEL" Text="Search By"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="70%">
			<asp:RadioButtonList ID="rdSearchBy" Runat="server" RepeatDirection="Horizontal" CssClass="LABEL" AutoPostBack="True">
				<asp:ListItem Value="R" Selected="True">By Range</asp:ListItem>
				<asp:ListItem Value="S">By Option</asp:ListItem>
			</asp:RadioButtonList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="10%" Wrap="False">
			<asp:Label Runat="server" CssClass="LABEL" Text="Organization Type"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="70%">
			<asp:RadioButtonList ID="rdOrganization" Runat="server" CssClass="LABEL" RepeatDirection="Horizontal">
				<asp:ListItem Value="N" Selected="True">New Organizations</asp:ListItem>
				<asp:ListItem Value="E">Existing Organizations</asp:ListItem>
			</asp:RadioButtonList>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Main Table Ends Here -->
	<br/>
	<!-- Search Table Starts Here -->
	<asp:Table ID="tblSearch" Runat="server" Width="100%" CellPadding="8" CellSpacing="0" Visible=False>
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Text="Search Option" Runat="server" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:DropDownList ID="cmbOption" Runat="server" CssClass="MEDIUMTEXT">
					<asp:ListItem Value=""></asp:ListItem>
					<asp:ListItem Value="ID">Organization Id</asp:ListItem>
					<asp:ListItem Value="NAME">Organization Name</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" Text="Search Criteria"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:DropDownList ID="cmbCriteria" Runat="server" CssClass="MEDIUMTEXT">
					<asp:ListItem Value=""></asp:ListItem>
					<asp:ListItem Value="STARTS WITH">Starts With</asp:ListItem>
					<asp:ListItem Value="CONTAINS">Contains</asp:ListItem>
					<asp:ListItem Value="EXACT MATCH">Exact Match</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" Text="Search Keyword" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtKeyword" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="50"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" Runat="server" ControlToValidate="txtKeyword" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Search Table Ends Here -->

	<!-- Range Table Starts Here -->
	<asp:Table ID="tblRange" Runat="server" Width="100%" CellPadding="8" CellSpacing="0">
		<asp:TableRow>
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="server" Text="Enter Organization Range From" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtOrgFrom" Runat="server" MaxLength="7" CssClass="SMALLTEXT"></asp:TextBox>&nbsp;
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtOrgFrom" ValidationExpression="^[\w\-\s]+$" ErrorMessage="OrgFrom Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>&nbsp;
				<asp:Label Runat="server" Text="To" CssClass="LABEL"></asp:Label>&nbsp;
				<asp:TextBox Runat="server" MaxLength="7" CssClass="SMALLTEXT" ID="txtOrgTo"></asp:TextBox>
                 <asp:RegularExpressionValidator ID="RegularExpressionValidator3" Runat="server" ControlToValidate="txtOrgTo" ValidationExpression="^[\w\-\s]+$" ErrorMessage="OrgTo Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
			</asp:TableCell> 
		</asp:TableRow>
		
	</asp:Table>
	<!-- Range Table Ends Here -->
	
	<asp:Table ID="tblButton" Runat="server" Width="100%" CellPadding="8" CellSpacing="0">
		<asp:TableRow>
			<asp:TableCell Width="90%">
				<asp:Button ID="btnSearch" Runat="server" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
				<asp:Button ID="btnClear" Runat="server" Text="Clear" CssClass="BUTTON" CausesValidation="False"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	
	<br/>
	
	<!-- Datagrid Starts Here -->
	
	<asp:Table ID="tblGrid" Runat="server" Width="90%" CellPadding="8" CellSpacing="0" Visible="False">
	<asp:TableRow><asp:TableCell ColumnSpan="2" Width="90%">
	<!--<div id="dvGridScroll" style="OVERFLOW: auto; WIDTH: 800px; HEIGHT: 250px; TEXT-ALIGN: left" align="center">-->
	<asp:Panel cssclass="GridDivNoScroll" ID="pnlGrid" runat="server">
	<asp:DataGrid ID="dgRequisition" Runat="server" AllowPaging="False"   GridLines="none"
		PagerStyle-HorizontalAlign="Center" CssClass="Grid"
		HeaderStyle-CssClass="GridHeaderStyle"
		ItemStyle-CssClass="GridItemStlye" AlternatingItemStyle-CssClass="GridAltItemStlye" width="100%"  HeaderStyle-HorizontalAlign="Left"
		AutoGenerateColumns="False">
		<Columns>
			<asp:BoundColumn DataField="ORGID" HeaderText="Org. Id"></asp:BoundColumn>
			<asp:BoundColumn DataField="NAME" HeaderText="Organization Name"></asp:BoundColumn>
			<asp:BoundColumn DataField="VERIFY" HeaderText="Verification"></asp:BoundColumn>
			<asp:BoundColumn DataField="TYPE" HeaderText="User Type"></asp:BoundColumn>
			<asp:BoundColumn DataField="CODE" HeaderText="Code Type"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Select">
				<ItemTemplate><asp:CheckBox Runat="server" ID="chkSelect"></asp:CheckBox></ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Remarks">
				<ItemTemplate>
					<asp:TextBox ID="txtRemarks" Runat="Server" CssClass="MEDIUMTEXT" TextMode="MultiLine"></asp:TextBox>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtRemarks" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Remarks Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
				</ItemTemplate>
			</asp:TemplateColumn>
		</Columns>
	</asp:DataGrid></asp:panel>
	</asp:TableCell></asp:TableRow>
	<asp:TableRow ID="trSelect">
		<asp:TableCell Width="90%" HorizontalAlign="Center" ColumnSpan="2">
			<asp:Button ID="btnSelect" Runat="server" Text="Select All" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
			<asp:Button ID="btnUnSelect" Runat="server" Text="Unselect All" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
			<asp:Button ID="btnSubmit" Runat="server" Text="Send Request" CssClass="BUTTON"></asp:Button>&nbsp;
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
				<asp:Button ID="btnConfirm" CssClass="BUTTON" Runat="Server" Text="Submit"></asp:Button>&nbsp;
				<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncClear();">
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Datagrid Ends Here -->
	
	<!-- Validators Starts Here -->
	<asp:RequiredFieldValidator ID="rfvFrom" Runat="Server" ControlToValidate="txtOrgFrom" Display="None" ErrorMessage="From Organization cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvTo" Runat="Server" ControlToValidate="txtOrgTo" Display="None" ErrorMessage="To Organization  cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvOption" Runat="Server" ControlToValidate="cmbOption" Display="None" ErrorMessage="Search Option cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCriteria" Runat="Server" ControlToValidate="cmbCriteria" Display="None" ErrorMessage="Search Criteria cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvKeyword" Runat="Server" ControlToValidate="txtKeyword" Display="None" ErrorMessage="Search Keyword cannot be blank."></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvAuth" Runat="Server" ControlToValidate="txtAuthCode" ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
	<asp:ValidationSummary ID="vsPinRequest" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
	<!-- Validators Ends Here -->
		
</asp:Content>