<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_AccessLogs" CodeFile="PG_AccessLogs.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager2" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
    
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
  </table>
	<!-- Main Table Starts Here -->
	<asp:Table Width="100%" Runat="Server" ID="tblform" CellPadding="8" CellSpacing="0">
	
	<asp:TableRow>
     
		<asp:TableCell Width="100%">
			<!-- Search Table Starts Here -->
			<asp:UpdatePanel ID="updtSearch" runat="server" ChildrenAsTriggers="false"  UpdateMode="conditional">
			<ContentTemplate>
			<asp:Table Width="90%" CellPadding="2" CellSpacing="0" ID="tblSearch" Runat="Server">
			<asp:TableRow>
				<asp:TableCell Width="90%" ColumnSpan="2">
					<asp:RadioButton ID="rdAll" Runat="Server" CssClass="LABEL" GroupName="SearchOptions" Text="All Records" Checked="True"></asp:RadioButton>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="20%" Wrap="False">
					<asp:RadioButton ID="rdUserId" Runat="Server" CssClass="LABEL" Text="User Id" GroupName="SearchOptions"></asp:RadioButton>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtUserId" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="14"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtUserId" ValidationExpression="^[\w\-\s]+$" ErrorMessage="UserId Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="20%" Wrap="False">
					<asp:RadioButton ID="rdUserName" Runat="Server" CssClass="LABEL" Text="Staff Name" GroupName="SearchOptions"></asp:RadioButton>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtUserName" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="20"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtUserName" ValidationExpression="^[\w\-\s]+$" ErrorMessage="UserName Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="20%" Wrap="False">
					<asp:RadioButton ID="rdFrom" Runat="Server" CssClass="LABEL" Text="From (DD/MM/YYYY)" GroupName="SearchOptions"></asp:RadioButton>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtFromDt" AutoPostBack="true" Runat="Server" CssClass="SMALLTEXT" MaxLength="10"></asp:TextBox>&nbsp;
					<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtFromDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0" /></a>
					<asp:Label Runat="Server" CssClass="LABEL" Text="To (DD/MM/YYYY)" ID="Label1"></asp:Label>&nbsp;
					<asp:TextBox ID="txtToDt" Runat="Server" CssClass="SMALLTEXT" MaxLength="10"></asp:TextBox>&nbsp;
					<a href="#" onclick='popUpCalendar(this, document.forms[0].ctl00$cphContent$txtToDt, "dd/mm/yyyy");'><img src="../Include/Images/date.gif" border="0" /></a>
					<asp:RegularExpressionValidator ID="revFromDate" Runat="server" ControlToValidate="txtFromDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
					<asp:RegularExpressionValidator ID="revToDate" Runat="server" ControlToValidate="txtToDt" ValidationExpression="^\d{2}/\d{2}/\d{4}" ErrorMessage="Date Format DD/MM/YYYY" Display="None"></asp:RegularExpressionValidator>
				</asp:TableCell> 
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
			</asp:TableRow>
			</asp:Table> 
			</ContentTemplate>
			<Triggers>
			   
			</Triggers>
			</asp:UpdatePanel> 
		<table cellpadding="2" border="0" width="100%" cellspacing=0>
		  <tr>
		     <td><asp:Button ID="btnSearch" Runat="Server" CssClass="BUTTON" Text="Search"></asp:Button>&nbsp;
					<asp:Button ID="btnClear" Runat="Server" CssClass="BUTTON" Text="Clear"></asp:Button></td>
		  </tr>
		</table>
			<!-- Search Table Ends Here -->
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">
			
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">
       <asp:UpdatePanel ID="updtGrid" runat="server">
       <ContentTemplate>
       <asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label>
			<asp:DataGrid ID="dgAccessLog" Runat="Server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"	PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" OnPageIndexChanged="prPageChange"	GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" AutoGenerateColumns="False" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
			<Columns>
				<asp:BoundColumn DataField="ULogin" HeaderText="User Id" ></asp:BoundColumn>
				<asp:BoundColumn DataField="UName" HeaderText="Staff Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="IDATE" HeaderText="Session Start Time"></asp:BoundColumn>
				<asp:BoundColumn DataField="ODATE" HeaderText="Session End Time"></asp:BoundColumn>
				<asp:TemplateColumn HeaderText="Transactions" ItemStyle-HorizontalAlign="Center">
					<ItemTemplate>
						<a href="PG_ViewLogs.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"LID")%>&User=<%=Request.QueryString("User")%>"><%#DataBinder.Eval(Container.DataItem,"TRANS")%></a>
					</ItemTemplate>							
				</asp:TemplateColumn>
			</Columns>
			</asp:DataGrid>
			<asp:DataGrid ID="dgModifyLog" Runat="Server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
			PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" OnPageIndexChanged="prPageChange1"
			GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt"
			AutoGenerateColumns="False" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
			<Columns>
				<asp:BoundColumn DataField="OID" HeaderText="Org Id"></asp:BoundColumn>
				<asp:BoundColumn DataField="UName" HeaderText="Staff/Org Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="MDate" HeaderText="Date Time"></asp:BoundColumn>
				<asp:BoundColumn DataField="MField" HeaderText="Field"></asp:BoundColumn>
				<asp:BoundColumn DataField="NData" HeaderText="New Data"></asp:BoundColumn>
				<asp:BoundColumn DataField="OData" HeaderText="Old Data"></asp:BoundColumn>
				<asp:BoundColumn DataField="MModify" HeaderText="Modified By"></asp:BoundColumn>
				<asp:BoundColumn DataField="UAPPRID" HeaderText="Approved By"></asp:BoundColumn>
			</Columns>
			</asp:DataGrid>
			<asp:DataGrid ID="dgDeleteLog" Runat="Server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
			PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" OnPageIndexChanged="prPageChange2"
			GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" AutoGenerateColumns="False"  CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
			<Columns>
				<asp:BoundColumn DataField="ULOGIN" HeaderText="User Id"></asp:BoundColumn>
				<asp:BoundColumn DataField="UNAME" HeaderText="Staff Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="UCDATE" HeaderText="Created Date"></asp:BoundColumn>
				<asp:BoundColumn DataField="UCREATE" HeaderText="Created By"></asp:BoundColumn>
				<asp:BoundColumn DataField="DDATE" HeaderText="Deleted Date"></asp:BoundColumn>
				<asp:BoundColumn DataField="UDELETE" HeaderText="Deleted By"></asp:BoundColumn>
				<asp:BoundColumn DataField="LSIGNON" HeaderText="Last Sign On"></asp:BoundColumn>
				<asp:BoundColumn DataField="LMODIFY" HeaderText="Last Modified"></asp:BoundColumn>
				<asp:BoundColumn DataField="UAPPRID" HeaderText="Approved By"></asp:BoundColumn>
			</Columns>
			</asp:DataGrid>
			<asp:DataGrid ID="dgFailLog" Runat="Server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
			PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" OnPageIndexChanged="prPageChange3"
			GridLines="none" CellPadding="3" CellSpacing="0"
			AutoGenerateColumns="False" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
			<Columns>
				<asp:BoundColumn DataField="ULogin" HeaderText="User Id" ></asp:BoundColumn>
				<asp:BoundColumn DataField="UName" HeaderText="Staff Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="UFADt" HeaderText="Unsuccessful Date/Time"></asp:BoundColumn>
			</Columns>
			</asp:DataGrid></ContentTemplate>
			   <Triggers>
			     
			   </Triggers>
			</asp:UpdatePanel>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Main Table Ends Here -->
     <asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
</asp:Content>