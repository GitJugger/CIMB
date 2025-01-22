<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_SessionCheck" CodeFile="PG_SessionCheck.aspx.vb" %>

<html xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>PG_SessionCheck</title>
    <link href="Styles.css" type="text/css" rel="stylesheet" />
	
  </head>
  <body>

    <form id="Form1" method="post" runat="server">
		<!-- Main Table Starts Here -->
		<asp:Table Width="100%" CellPadding="1" CellSpacing="2" BorderWidth="0" Runat="server">
			<asp:TableRow>
				<asp:TableCell Width="100%">
					<asp:Label CssClass="FORMHEAD" Runat="Server" Text="User Session Maintenance"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow><asp:TableCell Width="100%"><br></asp:TableCell></asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%"><asp:Label ID="lblMsg" Runat="Server" CssClass="MSG"></asp:Label></asp:TableCell>
			</asp:TableRow>
			<asp:TableRow><asp:TableCell Width="100%"><br></asp:TableCell></asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%">
					<asp:DataGrid Runat="Server" ID="dgSessionList" AllowCustomPaging="True" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
					PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" GridLines="Both" CellPadding="3"
					CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" AlternatingItemStyle-CssClass="ALTERNATE"
					width="75%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False"
					OnPageIndexChanged="Page_Change">
						<Columns>
							<asp:TemplateColumn ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%" HeaderText="Select">
								<ItemTemplate>
									<asp:CheckBox Runat="server" ID="chkSelect"></asp:CheckBox>
									<input type="hidden" id="hId" name="hId" runat="Server" value=<%# DataBinder.Eval(Container.DataItem,"UID")%>>									
								</ItemTemplate>	
							</asp:TemplateColumn>
							<asp:BoundColumn DataField="ORGID"  HeaderText="Organisation Id" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn DataField="USRID"  HeaderText="User Id" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn DataField="UNAME"  HeaderText="User Name" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn DataField="UROLE"  HeaderText="User Role" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn DataField="SDATE"  HeaderText="User Id" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow><asp:TableCell Width="100%"><br></asp:TableCell></asp:TableRow>
			<asp:TableRow ID="trBtn">
				<asp:TableCell Width="100%">
					<asp:Button ID="btnSave" Runat="Server" CssClass="BUTTON" Text="Delete Session"></asp:Button>&nbsp;
					<input type="reset" runat="server" value="     Clear     ">
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<!-- Main Table Ends Here -->
    </form>

  </body>
</html>
