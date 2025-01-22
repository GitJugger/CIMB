<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_FileListing.aspx.vb" Inherits="WebForms_pg_FileListing" title="File Listing Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br /><br />
<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Button ID="btnCreate" runat="server" CssClass="BUTTON" Text="Create New" CausesValidation="False"/> &nbsp;&nbsp;
				<asp:Button ID="btnHeader" runat="server" CssClass="BUTTON" Text="Header" CausesValidation="False" Visible="false"/>&nbsp;
				<asp:Button ID="btnBody" runat="server" CssClass="BUTTON" Text="Body" CausesValidation="False" Visible="false"/>&nbsp;
				<asp:Button ID="btnBodyTrailer" runat="server" CssClass="BUTTON" Text="BodyTrailer" CausesValidation="False" Visible="false"/>&nbsp;
				<asp:Button ID="btnFooter" runat="server" CssClass="BUTTON" Text="Footer" CausesValidation="False" Visible="false"/>&nbsp;
			</asp:TableCell>
		</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
    <asp:DataGrid ID="dgFileListing" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
	AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False">
	<Columns>
			<asp:BoundColumn DataField="Field_Description" HeaderText="Field Description"></asp:BoundColumn>
			<asp:BoundColumn DataField="Field_ContentType" HeaderText="Content Type"></asp:BoundColumn>
			<asp:BoundColumn DataField="Field_Start" HeaderText="Start Pos"></asp:BoundColumn>
			<asp:BoundColumn DataField="Field_End" HeaderText="End Pos"></asp:BoundColumn>
			<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
			<ItemTemplate>
			    <a href="pg_FileSettings.aspx?FTA=2&FI=
			    <%#DataBinder.Eval(Container.DataItem,("Field_Id"))%>">Modify</a>
			</ItemTemplate>
			</asp:TemplateColumn>
			<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
			<ItemTemplate>
			    <a href="pg_FileSettings.aspx?FTA=3&FI=
			    <%#DataBinder.Eval(Container.DataItem,("Field_Id"))%>">Delete</a>
			</ItemTemplate>
			</asp:TemplateColumn>
    </Columns>						
	</asp:DataGrid>
</asp:TableCell>
</asp:TableRow>
</asp:Table>
</asp:Content>

