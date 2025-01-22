<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_TransGrid.aspx.vb" Inherits="MaxPayroll.WebForms_pg_TransGrid"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<asp:Label Text="FileType Tnansaction Details" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label><br /><br />
    <asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
    <!-- Form Table Starts Here -->
    
    	<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
    	<asp:TableRow ID="tr_InputFiletypeID">
		  <asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Input_FileType" Text="Input FileType"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Input_Filetype_Id" Runat="server" CssClass="LARGETEXT" AutoPostBack="true">
				</asp:DropDownList>
		  </asp:TableCell>
		</asp:TableRow>
    	<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<!-- Datagrid starts here -->
					<asp:DataGrid ID="dgTransGrid" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
					    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
						AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False" DataKeyField="FileType_Id" >
					<Columns>
						<asp:BoundColumn DataField="FileType_Name" HeaderText="FileType Name"></asp:BoundColumn>
						<asp:BoundColumn DataField="FileType_Folder" HeaderText="FileType Folder"></asp:BoundColumn>
						<asp:BoundColumn DataField="Input_FileType_Id" HeaderText="Input FileType Id" Visible="false"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
							<ItemTemplate>
								<a href="pg_TransCode.aspx?FTA=2&FTI=<%#DataBinder.Eval(Container.DataItem,("FileType_Id"))%>&OF=
								    <%#DataBinder.Eval(Container.DataItem,("FileType_Name"))%>&INFI=
								    <%#DataBinder.Eval(Container.DataItem,("Input_FileType_Id"))%>">Add/Modify</a>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Datagrid ends here -->
			</asp:TableCell>
		</asp:TableRow>
    	</asp:Table>
	<!-- Form Table Ends Here -->
</asp:Content>

