<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ViewCustomer" CodeFile="PG_ViewCustomer.aspx.vb" %>


<html>
  <head>
    <title>PG_ViewCustomer</title>
    <link href="Styles.css" type="text/css" rel="Stylesheet">
    
  </head>
  <body onload="countDown()"  onmousemove="resetCounter()" onclick="resetCounter()" onchange="resetCounter()">
    <form id="timerform" name ="timerform" method="post" runat="server">
    <!--Main Table Starts here -->
    <table border="0" cellpadding="2" cellspacing="1" width="100%">
		<tr>
			<td width="100%"><asp:Label Runat="server" CssClass="FORMHEAD" Text="Customer File Mapping - View/Search Customers" id="Label1"></asp:Label></td>
		</tr>
		<tr><td width="100%">&nbsp;</td></tr>
		<tr>
			<td>
				<!-- Form Table Starts Here -->
				<table border="0" cellpadding="1" cellspacing="3" width="100%">
					
					<tr>
						<td width="20%"><asp:Label Runat="Server" ID="lblOption" Text="Search Option" CssClass="LABEL"></asp:Label></td>
						<td width="80%">
							<asp:DropDownList ID="cmbOption" CssClass="MEDIUMTEXT" Runat="Server">
								<asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="Organisation Id">Organization Id</asp:ListItem>
								<asp:ListItem Value="Organisation Name">Organization Name</asp:ListItem>
								<asp:ListItem Value="Registration No">Registration No</asp:ListItem>
								<asp:ListItem Value="EPF No">EPF No</asp:ListItem>
								<asp:ListItem Value="SOCSO No">SOCSO No</asp:ListItem>
								<asp:ListItem Value="STD No">STD No</asp:ListItem>
							</asp:DropDownList>
						</td>
						<asp:RequiredFieldValidator ID="rfvOption" Runat="server" ControlToValidate="cmbOption" ErrorMessage="Search Option"
							Display="None"></asp:RequiredFieldValidator>
					</tr>
					
					<tr>
						<td width="20%"><asp:Label Runat="Server" ID="lblCriteria" Text="Search Criteria" CssClass="LABEL"></asp:Label></td>
						<td width="80%">
							<asp:DropDownList ID="cmbCriteria" CssClass="MEDIUMTEXT" Runat="Server">
								<asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="Starts With">Starts With</asp:ListItem>
								<asp:ListItem Value="Contains">Contains</asp:ListItem>
								<asp:ListItem Value="Exact Match">Exact Match</asp:ListItem>
							</asp:DropDownList>
						</td>
						<asp:RequiredFieldValidator ID="rfvCriteria" Runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Search Criteria"
							Display="None"></asp:RequiredFieldValidator>
					</tr>
					<tr>
						<td width="20%"><asp:Label Runat="Server" ID="lblKeyword" Text="Search Keyword" CssClass="LABEL"></asp:Label></td>
						<td width="80%"><asp:TextBox ID="txtKeyword" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="20"></asp:TextBox></td>
						<asp:RequiredFieldValidator ID="rfvKeyword" Runat="server" ControlToValidate="txtKeyword" ErrorMessage="Search Keyword"
							Display="None"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator
                                            ID="revkeyword" runat="server" ControlToValidate="txtKeyword" Display="None"
                                            ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
					</tr>
					<tr>
						<td width="100%" colspan="2">
							<asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
							<asp:Button Runat="Server" ID="btnClear" Text="Clear" CssClass="BUTTON" CausesValidation="False"></asp:Button>
							
						</td>
					</tr>
				</table>
				<!-- Form Table Ends Here -->
			</td>
		</tr>				
		<tr>
			<td width="100%" colspan="2">
			<asp:Label Runat="server" ID="lblMessage" CssClass="MSG" Text=""></asp:Label>
				<!-- Data Grid Starts Here -->
				<asp:DataGrid Runat="Server" ID="dgOrganisation" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
					PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" GridLines="Both" CellPadding="3"
					CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" AlternatingItemStyle-CssClass="ALTERNATE"
					width="100%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False"
					OnPageIndexChanged="Page_Change">
					<Columns>
						<asp:TemplateColumn HeaderText="Organization Id" ItemStyle-HorizontalAlign="Center">
							<ItemTemplate>
								<a href="PG_CustomerFormat.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"OId")%>&Name=<%#DataBinder.Eval(Container.DataItem,"OName")%>"><%#DataBinder.Eval(Container.DataItem,"OId")%></a>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:BoundColumn datafield="OName" HeaderText="Organization Name" HeaderStyle-Width="150" HeaderStyle-HorizontalAlign="Center"></asp:BoundColumn>
						<asp:BoundColumn datafield="OAcNo" HeaderText="Ac Number" HeaderStyle-Width="150" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
						<asp:BoundColumn DataField="ODate" HeaderText="Create Date" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
						<asp:BoundColumn DataField="OPhone" HeaderText="Phone" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
						<asp:BoundColumn DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Data Grid Ends Here -->
			</td>
		</tr>
		<tr><td width="100%" colspan="2">
		<asp:Button Runat="Server" ID="btnShow" CausesValidation="False" Text="Show All" CssClass="BUTTON"></asp:Button>
		</td></tr>
	</table>
				<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields,"
					ID="vsViewCustomer" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
    <!--Main Table Ends here -->
	</form>
  </body>
</html>
