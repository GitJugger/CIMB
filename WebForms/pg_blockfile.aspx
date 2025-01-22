<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_BlockFile" CodeFile="PG_BlockFile.aspx.vb" %>
<html>
  <head>
    <title>PG_BlockFile</title>
    <link href="Styles.css" type="text/css" rel="stylesheet">
    
  </head>
  <body onload="countDown()"  onmousemove="resetCounter()" onclick="resetCounter()" onchange="resetCounter()">
    <form id="Form1" method="post" runat="server">
    <!-- Main Table Starts Here -->
    <asp:Table CellPadding="1" CellSpacing="2" Runat="Server" Width="100%">
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" CssClass="FORMHEAD" Text="Block File" ID="Label1"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell>
				<asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%">
				<asp:DataGrid ID="dgBlock" Runat="Server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="3" CellSpacing="0" 
				PageSize="15" GridLines="Both" PagerStyle-HorizontalAlign="Center" BorderWidth="1" Font-Name="Verdana" Font-Size="8pt" PagerStyle-Mode="NumericPages"
				HeaderStyle-CssClass="LABELHEAD" Width="100%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" OnPageIndexChanged="prPageChange">
				<Columns>
					<asp:BoundColumn DataField="FName" HeaderText="File Name" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					<asp:BoundColumn datafield="GName" HeaderText="Group Name" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					<asp:BoundColumn datafield="FCName" HeaderText="Converted File Name" HeaderStyle-Width="24%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					<asp:BoundColumn DataField="UDate" HeaderText="Upload Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					<asp:BoundColumn DataField="VDate" HeaderText="Payment Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					<asp:BoundColumn datafield="Status" HeaderText="Status" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Select" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
						<ItemTemplate>
							<a href="PG_DeleteFile.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"FID")%>&OId=<%#DataBinder.Eval(Container.DataItem,"OID")%>&OName=<%#DataBinder.Eval(Container.DataItem,"OName")%>&FName=<%#DataBinder.Eval(Container.DataItem,"FName")%>&GName=<%#DataBinder.Eval(Container.DataItem,"GName")%>&FCName=<%#DataBinder.Eval(Container.DataItem,"FCName")%>&UDate=<%#DataBinder.Eval(Container.DataItem,"UDate")%>&VDate=<%#DataBinder.Eval(Container.DataItem,"VDate")%>&Ftype=<%#DataBinder.Eval(Container.DataItem,"Ftype")%>&Status=<%#DataBinder.Eval(Container.DataItem,"Status")%>&Mode=Block File">Block File</a>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				</asp:DataGrid>
			</asp:TableCell>
		</asp:TableRow>
    </asp:Table>
    <!-- Main Table Ends Here -->
    </form>
  </body>
</html>
