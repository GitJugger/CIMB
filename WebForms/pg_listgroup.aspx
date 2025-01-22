<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ListGroup" CodeFile="PG_ListGroup.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>
    
		
	<!-- Main Table Starts Here -->
	    <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Group List" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td>
               <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
    <!-- Main Table Ends Here -->
	
	<!-- Datagrid Table Starts Here -->
	<asp:Table ID="tblMainForm" CellPadding="2" CellSpacing="1" Runat="Server" Width="90%">
	<asp:TableRow>
		<asp:TableCell Width="90%">
		  <asp:Panel ID="pnlGrid" runat="server" CssClass="GridDiv">
			<asp:DataGrid ID="dgGroup" Runat="Server" AllowPaging="True" AllowSorting="False" AutoGenerateColumns="False" PagerStyle-Mode="NumericPages"
			CellPadding="3" CellSpacing="0" PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" BorderColor="black" Font-Name="Verdana" Font-Size="8pt"
			 Width="100%" OnPageIndexChanged="prPageChange"  CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
			<Columns>
                <asp:TemplateColumn HeaderText="Group Name">
                    <ItemTemplate>
                       <asp:HyperLink ID="CRNo" runat="server" 
                 NavigateUrl='<%# Page.ResolveUrl(String.Format("PG_Group2.aspx?Mod=Modify&Id={0}", GetEncrypterString(Eval("GID").ToString()))) %>' 
                 Text='<%# Eval("GNAME") %>'>
                </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateColumn>
				
				<asp:BoundColumn datafield="REVNO" HeaderText="Reviewer's" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
				<asp:BoundColumn datafield="AUTHNO" HeaderText="Approver's" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
				<asp:BoundColumn datafield="CDATE" HeaderText="Created Date" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
				<asp:BoundColumn datafield="CNAME" HeaderText="Created By" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
				<asp:BoundColumn datafield="MDATE" HeaderText="Modified Date" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
				<asp:BoundColumn datafield="MNAME" HeaderText="Modified By" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
				<asp:BoundColumn datafield="GSTATUS" HeaderText="Status" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
			</Columns>
			</asp:DataGrid></asp:Panel>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Datagrid Table Ends Here -->

</asp:Content>