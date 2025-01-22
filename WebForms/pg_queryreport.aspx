<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_QueryReport" CodeFile="PG_QueryReport.aspx.vb" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>PG_QueryReport</title>
    <script language="JavaScript" src="PG_Calendar.js"></script>
    <LINK href="Styles.css" type="text/css" rel="stylesheet"/>
    
  </head>
  <body onload="countDown()"  onmousemove="resetCounter()" onclick="resetCounter()">
    <form id="Form1" method="post" runat="server">
	<!-- Heading Table Starts Here -->
	<asp:Table Width="100%" CellPadding="2" CellSpacing="1" ID="tblMain" Runat="Server">
		<asp:TableRow>
			<asp:TableCell Width="100%">
				<asp:Label ID="lblHead" Runat="Server" CssClass="FORMHEAD"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Heading Table Ends Here -->
	<br>
	<!-- Form Table Starts Here -->
	<!-- Expiry table start here -->
	<asp:Table Width="90%" ID="tblExpiry" BorderWidth="1" CellPadding="2" CellSpacing="1" Runat="Server">
		<asp:TableRow ID="trExpiry" Visible="False">
			<asp:TableCell Width="20%" Wrap="False">
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
				<asp:Button ID="btnExpiry" CssClass="BUTTON" Text="Display" Runat="Server"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:table>
	<!-- Expiry table stop here --> 
	<asp:Table Width="90%" ID="tblForm" BorderWidth="1" CellPadding="2" CellSpacing="1" Runat="Server">
		<asp:TableRow ID="trTrans" Visible="False">
			<asp:TableCell Width="20%" Wrap="False">
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
				<asp:Button ID="btnTrans" CssClass="BUTTON" Text="Display" Runat="Server"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trDorm" Visible="False">
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label Runat="Server" Text="Select Period" CssClass="LABEL" ID="Label2"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%" Wrap="False">
				<asp:DropDownList Runat="Server" ID="cmbPeriod" CssClass="MEDIUMTEXT">
					<asp:ListItem Value="3">Last 3 Months</asp:ListItem>
					<asp:ListItem Value="6">Last 6 Months</asp:ListItem>
					<asp:ListItem Value="9">Last 9 Months</asp:ListItem>
					<asp:ListItem Value="12">Last 12 Months</asp:ListItem>
				</asp:DropDownList>
				&nbsp;
				<asp:Button ID="btnDorm" Runat="Server" CssClass="BUTTON" Text="Display"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<asp:Table Width="90%" ID="tblFileSub" BorderWidth="1" CellPadding="2" CellSpacing="1" Runat="Server" Visible="False">
		<asp:TableRow>
			<asp:TableCell Width="20%">
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
			<asp:TableCell Width="20%" Wrap="False">
				<asp:Label CssClass="LABEL" Text="From Date (DD/MM/YYYY)" Runat="Server"></asp:Label>
	
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtFromDt" CssClass="SMALLTEXT" MaxLength="10" Runat="Server"></asp:TextBox>&nbsp;
				<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtFromDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
				<asp:Label Runat="Server" CssClass="LABEL" Text="To Date (DD/MM/YYYY)"></asp:Label>&nbsp;
				<asp:TextBox Runat="Server" CssClass="SMALLTEXT" MaxLength="10" ID="txtToDt"></asp:TextBox>
				<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtToDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0"></a>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Select Status"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
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
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" Text="File Type"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="ddlFile" Runat="server" CssClass="MEDIUMTEXT">
					<asp:ListItem Value="Payroll File">Payroll File</asp:ListItem>
					<asp:ListItem Value="EPF File">EPF File</asp:ListItem>
					<asp:ListItem Value="SOCSO File">SOCSO File</asp:ListItem>
					<asp:ListItem Value="LHDN File">LHDN File</asp:ListItem>
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%">
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
			<asp:TableCell Width="20%">
				<asp:Button ID="btnSubmit" Runat="server" CssClass="BUTTON" Text="Submit"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>	
	<asp:RequiredFieldValidator ID="rfvFromDt" ControlToValidate="txtFromDt" Display="None" ErrorMessage="Enter From Date" Runat="server"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvToDt" ControlToValidate="txtToDt" Display="None" ErrorMessage="Enter To Date" Runat="server"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvExpiredFromDt" Runat="server" ControlToValidate="ddlExpiryMonth" ErrorMessage="Select Month" Display="None"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvExpiredToDt" Runat="server" ControlToValidate="ddlExpiryYear" ErrorMessage="Select Year" Display="None"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="revFromDt" Runat="server" ControlToValidate="txtFromDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="From Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
	<asp:RegularExpressionValidator ID="revToDt" Runat="server" ControlToValidate="txtToDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="To Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
	<asp:ValidationSummary ID="vsReport" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
	<!-- Form Table Ends Here -->
    </form>
  </body>
</html>
