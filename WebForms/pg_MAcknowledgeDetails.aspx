<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_MAcknowledgeDetails.aspx.vb" Inherits="MaxPayroll.WebForms_pg_MAcknowledgeDetails" title="Acknowledge Details Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label><br /><br />
    <asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
    <!-- Form Table Starts Here -->
    
    	<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
    	<asp:TableRow ID="tr_FileType">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Input_FileType" Text="Input FileType"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Input_FileType_Id" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
    	<asp:TableRow ID="tr_ParentFiletype">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Parent_FileType" Text="Parent FileType"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Parent_FileType_Id" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" CssClass="LABEL" ID="FileType_Name" Text="Acknowledge Code"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__Acknowledge_Code" Runat="server" CssClass="LARGETEXT" MaxLength="10"></asp:TextBox>
			
			 &nbsp;<asp:RequiredFieldValidator ID="reqfAcnowCode" runat="server" ErrorMessage="Please Enter Acnowledge Code"
                ControlToValidate="__Acknowledge_Code"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_folder">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Acknowledge_Desc" Text="Acknowledge Description"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__Acknowledge_Desc" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_status">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Acknowledge_Send" Text="Acknowledge Send"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Acknowledge_Send" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow >
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Acknowledge_Success" Text="Acknowledge Success"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Acknowledge_Success" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Button ID="btnSubmit" Runat="server" CssClass="BUTTON" Text="Submit"></asp:Button>&nbsp;
				<%--<asp:Button ID="btnNew" Runat="server" CssClass="BUTTON" Text="New" Visible="false" CausesValidation="false"></asp:Button>&nbsp;--%>
				<asp:Button ID="btnCancel" runat="server" CssClass="BUTTON" Text="Cancel" CausesValidation="False"/>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow><asp:TableCell Width="100%" ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<!-- Datagrid starts here -->
					<asp:DataGrid ID="dgAcnowledge" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
					    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
						AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False" DataKeyField="Acknowledge_Id">
					<Columns>
						<asp:BoundColumn DataField="Input_FileType_Name" HeaderText="FileType Name"></asp:BoundColumn>
						<asp:BoundColumn DataField="Acknowledge_Code" HeaderText="Code"></asp:BoundColumn>
						<asp:BoundColumn DataField="Acknowledge_Desc" HeaderText="Description"></asp:BoundColumn>
												
						<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
							<ItemTemplate>
								<a href="pg_MAcknowledgeDetails.aspx?AI=<%#DataBinder.Eval(Container.DataItem,("Acknowledge_Id"))%>">Modify</a>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Datagrid ends here -->
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Form Table Ends Here -->
	<input type="hidden" runat="server" id="__Acknowledge_FileType_Id" name="__Acknowledge_FileType_Id" value="0"/>
	<%--<input type="hidden" runat="server" id="__Acknowledge_Action" name="__Acknowledge_Action" value="1"/>--%>
</asp:Content>

