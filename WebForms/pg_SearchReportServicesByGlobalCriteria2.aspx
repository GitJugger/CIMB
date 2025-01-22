<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_SearchReportServicesByGlobalCriteria2" CodeFile="pg_SearchReportServicesByGlobalCriteria2.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
    <link href="Styles.css" type="text/css" rel="stylesheet"/>
    
	<!-- Heading Table Starts Here -->
 <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Create Group" ID="lblHeading"></asp:Label>
            </td>
         </tr>
      </table> 
	<!-- Heading Table Ends Here -->

	<!-- Form Table Starts Here -->
	<!-- Expiry table start here -->
	<table cellpadding="8" cellspacing="0" width="100%"><tr><td>
	<asp:Table Width="100%" ID="tblExpiry" BorderWidth="0" CellPadding="2" CellSpacing="1" Runat="Server">
		<asp:TableRow ID="trExpiry" Visible="False">
			<asp:TableCell Width="30%" Wrap="False">
				<asp:Label Runat="Server" Text="Select Month / Year" CssClass="LABEL" ID="Label3"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:DropDownList ID="ddlExpiryMonth" CssClass="MEDIUMTEXT" Runat="Server">
					<asp:ListItem Selected=""></asp:ListItem>
					<asp:ListItem Value="1">January</asp:ListItem>
					<asp:ListItem Value="2">February</asp:ListItem>
					<asp:ListItem Value="3">March</asp:ListItem>
					<asp:ListItem Value="4">April</asp:ListItem>
					<asp:ListItem Value="5">May</asp:ListItem>
					<asp:ListItem Value="6">June</asp:ListItem>
					<asp:ListItem Value="7">July</asp:ListItem>
					<asp:ListItem Value="8">August</asp:ListItem>
					<asp:ListItem Value="9">September</asp:ListItem>
					<asp:ListItem Value="10">October</asp:ListItem>
					<asp:ListItem Value="11">November</asp:ListItem>
					<asp:ListItem Value="12">December</asp:ListItem>
				</asp:DropDownList>
				&nbsp;
				<asp:DropDownList ID="ddlExpiryYear" CssClass="SMALLTEXT" Runat="Server">
				</asp:DropDownList>
				&nbsp;
				<asp:Button ID="btnExpiry" Visible=false CssClass="BUTTON" Text="Display" Runat="Server"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Expiry table stop here --> 
	<asp:Table Width="100%" ID="tblForm" BorderWidth="0" CellPadding="2" CellSpacing="0" Runat="Server">
		<asp:TableRow ID="trTrans" Visible="False">
			<asp:TableCell Width="30%" Wrap="False">
				<asp:Label Runat="Server" Text="Select Month / Year" CssClass="LABEL" ID="Label1"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:DropDownList ID="cmbMonth" CssClass="MEDIUMTEXT" Runat="Server">
					<asp:ListItem Value="1">January</asp:ListItem>
					<asp:ListItem Value="2">February</asp:ListItem>
					<asp:ListItem Value="3">March</asp:ListItem>
					<asp:ListItem Value="4">April</asp:ListItem>
					<asp:ListItem Value="5">May</asp:ListItem>
					<asp:ListItem Value="6">June</asp:ListItem>
					<asp:ListItem Value="7">July</asp:ListItem>
					<asp:ListItem Value="8">August</asp:ListItem>
					<asp:ListItem Value="9">September</asp:ListItem>
					<asp:ListItem Value="10">October</asp:ListItem>
					<asp:ListItem Value="11">November</asp:ListItem>
					<asp:ListItem Value="12">December</asp:ListItem>
				</asp:DropDownList>
				&nbsp;
				<asp:DropDownList ID="cmbYear" CssClass="SMALLTEXT" Runat="Server">
					
				</asp:DropDownList>
				&nbsp;
				<asp:Button ID="btnTrans" Visible=false CssClass="BUTTON" Text="Display" Runat="Server"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trDorm" Visible="False">
			<asp:TableCell Wrap="False">
				<asp:Label Runat="Server" Text="Select Period" CssClass="LABEL" ID="Label2"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Wrap="False">
				<asp:DropDownList Runat="Server" ID="cmbPeriod" CssClass="MEDIUMTEXT">
					<asp:ListItem Value="3">Last 3 Months</asp:ListItem>
					<asp:ListItem Value="6">Last 6 Months</asp:ListItem>
					<asp:ListItem Value="9">Last 9 Months</asp:ListItem>
					<asp:ListItem Value="12">Last 12 Months</asp:ListItem>
				</asp:DropDownList>
				&nbsp;
				<asp:Button ID="btnDorm" Visible=false Runat="Server" CssClass="BUTTON" Text="Display"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<asp:Table Width="100%" ID="tblFileSub" BorderWidth="0" CellPadding="2" CellSpacing="0" Runat="Server" Visible="False">
		<asp:TableRow>
			<asp:TableCell Width="30%">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Select Option"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:DropDownList CssClass="MEDIUMTEXT" ID="ddlOption" Runat="Server">
					<asp:ListItem Value="UPLOAD DATE">Upload Date</asp:ListItem>
					<asp:ListItem Value="PAY DATE">Payment Date</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>	
		<asp:TableRow>
			<asp:TableCell Wrap="False">
				<asp:Label CssClass="LABEL" Text="From Date (DD/MM/YYYY)" Runat="Server"></asp:Label>
	
			</asp:TableCell>
			<asp:TableCell>
				<asp:TextBox ID="txtFromDt" CssClass="SMALLTEXT" MaxLength="10" Runat="Server"></asp:TextBox>&nbsp;
				<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtFromDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
				<asp:Label Runat="Server" CssClass="LABEL" Text="To Date (DD/MM/YYYY)"></asp:Label>&nbsp;
				<asp:TextBox Runat="Server" CssClass="SMALLTEXT" MaxLength="10" ID="txtToDt"></asp:TextBox>
				<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtToDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"/></a>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" CssClass="LABEL" Text="Select Status"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList CssClass="MEDIUMTEXT" Runat="Server" ID="ddlStatus">
					<asp:ListItem Value="8">All</asp:ListItem>
					<asp:ListItem Value="0">Pending</asp:ListItem>
					<asp:ListItem Value="4">Blocked</asp:ListItem>
					<asp:ListItem Value="5">Submitted</asp:ListItem>
					<asp:ListItem Value="1">Processed</asp:ListItem>
					<asp:ListItem Value="6">Customer Rejection</asp:ListItem>
					<asp:ListItem Value="7">Host Rejection</asp:ListItem>
					<asp:ListItem Value="3">Stop Payment</asp:ListItem>
					<asp:ListItem Value="9">Reconciliation</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="server" CssClass="LABEL" Text="File Type"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList ID="ddlFile" Runat="server" CssClass="MEDIUMTEXT">
				
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" CssClass="LABEL" Text="Sort By"></asp:Label>
			</asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList ID="ddlSort" CssClass="MEDIUMTEXT" Runat="Server">
					<asp:ListItem Value="Org Id">Organisation Id</asp:ListItem>
					<asp:ListItem Value="File Name">File Name</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Button ID="btnSubmit" Visible=false Runat="server" CssClass="BUTTON" Text="Submit"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>	</td></tr></table>
	<table width="90%" cellpadding="8" cellspacing="0">
	    <tr>
	        <td><asp:Button ID="btnSearchReport" runat=server Text="Search Report" CssClass="BUTTON" /></td>	       
	    </tr>
	</table>
	<asp:RequiredFieldValidator ID="rfvFromDt" ControlToValidate="txtFromDt" Display="None" ErrorMessage="Enter From Date" Runat="server"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvToDt" ControlToValidate="txtToDt" Display="None" ErrorMessage="Enter To Date" Runat="server"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvExpiredFromDt" Runat="server" ControlToValidate="ddlExpiryMonth" ErrorMessage="Select Month" Display="None"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvExpiredToDt" Runat="server" ControlToValidate="ddlExpiryYear" ErrorMessage="Select Year" Display="None"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="revFromDt" Runat="server" ControlToValidate="txtFromDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="From Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="revToDt" Runat="server" ControlToValidate="txtToDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="To Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
	<asp:ValidationSummary ID="vsReportCriteria2" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
	<!-- Form Table Ends Here -->
   
   </asp:Content>