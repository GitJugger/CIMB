<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ListFile" CodeFile="PG_ListFile.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
      <script type="text/javascript" src="../include/common.js"></script>
    <script language="JavaScript">
		function fncNew()
		{
			var lngOrgId;
			//lngOrgId = document.timerform.ctl00$cphContent$hOrgId.value;
			lngOrgId = document.all('ctl00$cphContent$hOrgId').value;
			window.location.href = "PG_CustomerFormat.aspx?OrgId="+lngOrgId;
		}
		function fncBack()
		{
			window.location.href = "PG_ViewOrganisation.aspx?Req=File";
		}
    </script>
    
  		<!-- Main Table Starts Here -->
		 <table cellpadding="5" cellspacing="0" width="100%" border="0">
            <tr>
                <td id="cHeader"><asp:Label ID="lblHeading" CssClass="FORMHEAD" Runat="Server">Organization File Settings</asp:Label></td>
            </tr>
            <tr>
                <td id="Td1"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
            </tr>
        </table>
		<!-- Main Table Ends Here -->
		<asp:Table Width="100%" CellPadding="8" CellSpacing="0" Runat="Server" ID="tblMainForm">
			<asp:TableRow ID="trOrgId" Visible="False">
				<asp:TableCell Width="20%">
					<asp:Label Runat="server" CssClass="LABEL" Text="Organization Id"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">	
					<asp:Label ID="lblOrgId" Runat="server" CssClass="BLABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trOrgName" Visible="False">
				<asp:TableCell Width="20%">
					<asp:Label Runat="server" CssClass="LABEL" Text="Organization Name"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="80%">	
					<asp:Label ID="lblOrgName" Runat="server" CssClass="BLABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%" ColumnSpan="2">
					<!-- Data Grid Starts Here -->
					<asp:panel ID=pnlGrid runat=server CssClass = "GridDivNoScroll" width="100%">
					<asp:DataGrid width="100%" cssClass="Grid" Runat="Server" ID="dgFile" AllowPaging="False"  BorderWidth="0"
					HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass= "GridAltItemStyle"
					  HeaderStyle-HorizontalAlign="left" AutoGenerateColumns="False">
					<Columns>
						<asp:BoundColumn DataField="FTYPE" HeaderText="File Type" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
						<asp:BoundColumn DataField="BankName" HeaderText="Bank" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
						<asp:BoundColumn datafield="FFORMAT" HeaderText="Format Type" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
						<asp:BoundColumn datafield="FNAME" HeaderText="Format Name" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
						<asp:BoundColumn DataField="FEXTN" HeaderText="File Extension" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Select" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
							<ItemTemplate>
								<a href="PG_CustomerFormat.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"FID")%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"ONAME")%>">Modify</a>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Select" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="10%">
							<ItemTemplate>
								<a href="PG_CustomerFormat.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"FID")%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"ONAME")%>">View</a>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
					</asp:DataGrid>
					</asp:panel>
					<!-- Data Grid Ends Here -->
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trNew"> 
				<asp:TableCell Width="100%" ColumnSpan="2">
					<input type="button" runat="server" value="Create New File" onclick="fncNew();" id="btnNew" name="btnNew">&nbsp;
					<input type="button" runat="server" value="Back" onclick="fncBack();" class="BUTTON">
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<input type="hidden" runat="server" id="hOrgId" name="hOrgId">

</asp:Content>