<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ViewLogs" CodeFile="PG_ViewLogs.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			var strRequest;
			strRequest = ct100$cphContent$hUser.value;
			if(strRequest == "BU")
			{
				window.location.href = "PG_AccessLogs.aspx?User=BU&Log=Acc";	
			}
			else if(strRequest == "IU")
			{
				window.location.href = "PG_AccessLogs.aspx?User=IU&Log=Acc";	
			}
			else if(strRequest == "BA")
			{
				window.location.href = "PG_AccessLogs.aspx?User=BA&Log=Acc";	
			}
		}
    </script>
    
    
	<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
      <tr>
         <td><asp:Label ID="lblMessage" CssClass="MSG" Runat="Server"></asp:Label></td>
      </tr>
  </table>
	<!-- Main Table Starts Here -->
	<asp:Table ID="tblMain" CellPadding="2" CellSpacing="1" Runat="Server" Width="100%">
	<asp:TableRow>
		<asp:TableCell Width="100%">
			<asp:DataGrid ID="dgLogs" Runat="Server" AllowPaging="True" PageSize="10" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="1" OnPageIndexChanged="prPageChange" GridLines="Both" CellPadding="3" CellSpacing="0" AutoGenerateColumns="False" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" >
			<Columns>
				<asp:BoundColumn DataField="LLOGIN" HeaderText="User Id"></asp:BoundColumn>
				<asp:BoundColumn DataField="LNAME" HeaderText="User Name"></asp:BoundColumn>
				<asp:BoundColumn DataField="LDATE" HeaderText="Date"></asp:BoundColumn>
				<asp:BoundColumn DataField="LTIME" HeaderText="Time"></asp:BoundColumn>
				<asp:BoundColumn DataField="TRANS" HeaderText="Trans Type" ></asp:BoundColumn>
				<asp:BoundColumn DataField="STATUS" HeaderText="Successful"></asp:BoundColumn>
			</Columns>
			</asp:DataGrid>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow>
		<asp:TableCell Width="100%">
		    <asp:Button ID="btnBack" CssClass="BUTTON" Text="Back" runat="server" />
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<input type="hidden" id="hUser" name="hUser" runat="server" />
	<!-- Main Table Ends Here -->

</asp:Content>