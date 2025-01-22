<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_CPS_Cheque_Allocation" CodeFile="pg_CPS_Cheque_Allocation.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <script type="text/javascript" src="../include/common.js"></script>
    
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			window.location.href = "pg_CPS_Cheque_Allocation?Mode=" + document.forms[0].ctl00$cphContent$hidMode.value ;
			//window.history.back();
		}
		function fncClear()
		{
			window.location.href = "pg_CPS_Cheque_Allocation?Mode=Edit" ;
		}
    </script>
    <script type="text/javascript" src="../include/common.js"></script>
 
    <input type="hidden" id="hidMode" runat="server" />
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
      <tr>
         <td><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
  </table>

		<!-- Datagrid Table Starts Here -->
		<asp:Table ID="tblMainForm" CellPadding="8" CellSpacing="0" Runat="Server" Width="100%">
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
			 <asp:Panel ID="pnlGrid" runat="server" CssClass="GridDiv">
				<asp:DataGrid ID="dgCPSMatrix" Runat="Server" AllowPaging="True" AllowSorting="False" AutoGenerateColumns="False" PagerStyle-Mode="NumericPages" OnPageIndexChanged="prPageChange" CellPadding="3" CellSpacing="0" PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" Font-Names="Verdana" Font-Size="8pt" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
				<Columns>
					
					<asp:BoundColumn Datafield="APRID" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn Datafield="FNAME" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>
					<asp:BoundColumn Datafield="RDATE" HeaderText="Received" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>
					<asp:BoundColumn Datafield="ASUBJECT" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Action" HeaderStyle-Width="15%">
					
						<ItemTemplate>
						  <a href="<%#GetLink(Eval("ASUBJECT")) %>?OrgId=<%#DataBinder.Eval(Container.DataItem,"FNAME")%>&FileId=<%#DataBinder.Eval(Container.DataItem,"APRID")%>&FileName=<%#DataBinder.Eval(Container.DataItem,"FileName")%>&Type=<%#DataBinder.Eval(Container.DataItem,"TYPE")%>"><%#DataBinder.Eval(Container.DataItem,"ASUBJECT")%></a>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Datafield="TYPE" HeaderText="Type" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>
					<asp:BoundColumn Datafield="MDSC" HeaderText="Description" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>					
					
				</Columns>
				</asp:DataGrid>
				</asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>

    
</asp:Content>
