<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_ReportSearch.aspx.vb" Inherits="WebForms_pg_ReportSearch" title="Report Search" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<script type="text/javascript" src="PG_Calendar.js"></script>
<!-- Form Table Starts Here -->
<br />
<p><b><asp:Label ID="lblReportName" Runat="server" CssClass="LABEL" ></asp:Label></b></p>
    <asp:ValidationSummary ID="ValidationSummary" runat="server" HeaderText="You must correct the value in the following fields:"
                CssClass="LABEL" DisplayMode="BulletList"
/>
<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
<asp:TableRow >
	<asp:TableCell Width="20%">
		<asp:Label ID="Label1" Runat="server" CssClass="LABEL" Text="Search Option"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="Search_Option" Runat="server" CssClass="LARGETEXT" 
		OnSelectedIndexChanged="Option_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_1" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" Text="Search Criteria"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="__Param_1" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="__Param_1" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Data Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_2" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" Text="Start Date (DD/MM/YYYY)"></asp:Label>
			</asp:TableCell>
	        <asp:TableCell Width="80%">
              <input type="text" id="txtStartDate" name="txtStartDate" class="SMALLTEXT" runat="server"
                    onfocus="popUpCalendar(this, document.all('ctl00$cphContent$txtStartDate'), 'dd/mm/yyyy');" />&nbsp;
                <a href="#" onclick="popUpCalendar(this, document.all('ctl00$cphContent$txtStartDate'), 'dd/mm/yyyy');">
                    <img src="../Include/Images/date.gif" border="0"></a>
                    &nbsp;<asp:RequiredFieldValidator Display="None" ID="reqfStartDate" runat="server" ErrorMessage="Please Enter Start Date"
                ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
            </asp:TableCell>
	
</asp:TableRow>
<asp:TableRow  ID="trParam_3" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label2" Runat="server" CssClass="LABEL" Text="End Date (DD/MM/YYYY)"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
              <input type="text" id="txtEndDate" name="txtEndDate" class="SMALLTEXT" runat="server"
                    onfocus="popUpCalendar(this, document.all('ctl00$cphContent$txtEndDate'), 'dd/mm/yyyy');" />&nbsp;
                <a href="#" onclick="popUpCalendar(this, document.all('ctl00$cphContent$txtEndDate'), 'dd/mm/yyyy');">
                    <img src="../Include/Images/date.gif" border="0"></a>
                    &nbsp;<asp:RequiredFieldValidator Display="None" ID="reqfEndDate" runat="server" ErrorMessage="Please Enter End Date"
                ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
            </asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_4" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label3" Runat="server" CssClass="LABEL" Text="Select Month"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="ddlMonth" Runat="server" CssClass="LARGETEXT" 
				>
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
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_5" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label4" Runat="server" CssClass="LABEL" Text="Year"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="txtYear" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
		&nbsp;<asp:RequiredFieldValidator Display="None" ID="reqfYear" runat="server" ErrorMessage="Please Enter Year"
                ControlToValidate="txtYear"></asp:RequiredFieldValidator><br />
                 <asp:RegularExpressionValidator Display="None" ID="reqfYear1" runat="server" ControlToValidate="txtYear"
        ErrorMessage="Year Must Be In YYYY format" ValidationExpression="(\d{4})$"></asp:RegularExpressionValidator>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_6" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label5" Runat="server" CssClass="LABEL" Text="Select Field"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="ddlFieldName" Runat="server" CssClass="LARGETEXT" 
				>
				<asp:ListItem Value="CustomerName">Customer Name</asp:ListItem>
				<asp:ListItem Value="LimitAmount">Limit Amount</asp:ListItem>
				<asp:ListItem Value="Frequency">Frequency</asp:ListItem>
				<asp:ListItem Value="FrequencyLimit">Frequency Limit</asp:ListItem>
				<asp:ListItem Value="Status">Status</asp:ListItem>
				</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_7" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label6" Runat="server" CssClass="LABEL" Text="Status"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="ddlStatus" Runat="server" CssClass="LARGETEXT" 
				>
				<asp:ListItem Value="A">Active</asp:ListItem>
				<asp:ListItem Value="I">Inactive</asp:ListItem>
				<asp:ListItem Value="B">Both</asp:ListItem>
				</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow  ID="trParam_8" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label7" Runat="server" CssClass="LABEL" Text="Cheque Number Range:"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%" >
		From : <asp:TextBox ID="txtChequeNo" runat="server"  MaxLength="6"></asp:TextBox>
		To :   <asp:TextBox ID="txtChequeNo_To" runat="server"  MaxLength="6"></asp:TextBox>
		<br />
		<asp:RequiredFieldValidator Display="None" ID="rfvChequeno" runat="server" ControlToValidate ="txtChequeNo" ErrorMessage="From Cheque No Cannot Be Empty"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator Display="None" ID="revChequeNo" runat="server" ControlToValidate="txtChequeNo"
        ErrorMessage="From Cheque Number Must Be range of 1 - 999999" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}?$"></asp:RegularExpressionValidator>
        
		<asp:RequiredFieldValidator Display="None" ID="rfvChequeNoTo" runat="server" ControlToValidate ="txtChequeNo_To" ErrorMessage="To Cheque No Cannot Be Empty" ></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator Display="None" ID="revChequeNoTo" runat="server" ControlToValidate="txtChequeNo_To"
        ErrorMessage="To Cheque Number Must Be range of 1 - 999999" ValidationExpression="(?!^0*$)(?!^0*\.0*$)^\d{1,6}?$" ValidationGroup="Group1"></asp:RegularExpressionValidator>
	</asp:TableCell>
</asp:TableRow>

<asp:TableRow  ID="trParam_9" Visible="false">
	<asp:TableCell Width="20%">
		<asp:Label ID="Label8" Runat="server" CssClass="LABEL" Text="Range"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="ddlRange" Runat="server" CssClass="LARGETEXT" 
				>
				<asp:ListItem Value="3">3 Months</asp:ListItem>
				<asp:ListItem Value="6">6 Months</asp:ListItem>
				<asp:ListItem Value="12">More then 12</asp:ListItem>
				
				</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>

<asp:TableRow>
	<asp:TableCell Width="100%" ColumnSpan="2">
		<asp:Button ID="btnSubmit" Runat="server" CssClass="BUTTON" Text="Submit"></asp:Button><br />&nbsp;
        <asp:CompareValidator Display="None" ID="CompareValidator1" ControlToCompare="txtStartDate" ControlToValidate="txtEndDate" runat="server"
         Operator="GreaterThanEqual" Type="Date" ErrorMessage="Start Date Cannot be Greater than End Date"></asp:CompareValidator>
         <br />
         <asp:CompareValidator Display="None" ID="CompareValidator2" ControlToCompare="txtChequeNo" ControlToValidate="txtChequeNo_To" runat="server"
         Operator="GreaterThanEqual" Type="Integer" ErrorMessage="(From) Cheque No cannot be greater then (To) Cheque No"></asp:CompareValidator>
         <br />
		<asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow><asp:TableCell Width="100%" ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
<asp:TableRow>
	<asp:TableCell Width="100%" ColumnSpan="2">
	    <!-- Datagrid starts here -->
		<asp:DataGrid ID="dgMandate" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
		    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
			AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False" DataKeyField="Search_Criteria"
			 AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" BorderWidth="1">
		<Columns>
		        <asp:BoundColumn DataField="Org_Id" HeaderText="Organization Id"></asp:BoundColumn>
		        <asp:BoundColumn DataField="Org_Code" HeaderText="Org Code" Visible="false" ></asp:BoundColumn>
		        <asp:BoundColumn DataField="Search_Criteria" HeaderText="Search Criteria"></asp:BoundColumn>  
				<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
					<ItemTemplate>
						<a href=
						"pg_ReportSearch.aspx?ShowRep=True&FID=<%#DataBinder.Eval(Container.DataItem,("File_Id"))%>
						&Criteria=<%#DataBinder.Eval(Container.DataItem,("Search_Criteria"))%>&OrgId=
						<%#DataBinder.Eval(Container.DataItem,("Org_Id"))%>">View Report</a>
					</ItemTemplate>
				</asp:TemplateColumn>
		</Columns>
		</asp:DataGrid>
		<!-- Datagrid Ends here -->
	</asp:TableCell>
</asp:TableRow>
</asp:Table>
<!-- Form Table Ends Here -->

</asp:Content>

