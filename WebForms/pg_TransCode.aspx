<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_TransCode.aspx.vb" Inherits="MaxPayroll.WebForms_pg_TransCode" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<asp:Label Text="Transactions Code Details" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label><br /><br />
    <asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
    <!-- Form Table Starts Here -->
    
    	<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
   		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" CssClass="LABEL" ID="FileType_Id" Text="Output File"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__Output_FileType_Id" Runat="server" CssClass="LARGETEXT" Enabled="false"></asp:TextBox>
			&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Output File"
                ControlToValidate="__Output_FileType_Id"></asp:RequiredFieldValidator>
               
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_TransCode">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Tran_Code" Text="Trans Code"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__Tran_Code" Runat="server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
				<asp:RegularExpressionValidator
                                            ID="revTranCode1" runat="server" ControlToValidate="__Tran_Code" Display="None"
                                            ErrorMessage="Tran Code Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
			<%--&nbsp;<asp:RequiredFieldValidator ID="reqfTransCode" runat="server" ErrorMessage="Please Enter Trans Code"
                ControlToValidate="__Tran_Code"></asp:RequiredFieldValidator>--%>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_TransDesc">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Tran_Desc" Text="Trans Desc"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__Tran_Desc" Runat="server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
				
			&nbsp;<asp:RequiredFieldValidator ID="reqfTransDesc" runat="server" ErrorMessage="Please Enter Trans Desc"
                ControlToValidate="__Tran_Desc"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator
                                            ID="revTranDesc" runat="server" ControlToValidate="__Tran_Desc" Display="None"
                                            ErrorMessage="Tran Desc Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>						
		
		<asp:TableRow ID="tr_FiletypeTable">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Tran_Successful" Text="Trans Successful"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Tran_Successful" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Button ID="btnSubmit" Runat="server" CssClass="BUTTON" Text="Save"></asp:Button>&nbsp;
				<asp:Button ID="btnReset" Runat="server" CssClass="BUTTON" Text="Reset"></asp:Button>&nbsp;
				</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow><asp:TableCell Width="100%" ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<!-- Datagrid starts here -->
					<asp:DataGrid ID="dgTransCode" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
					    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
						AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False" DataKeyField="Tran_Id" >
					<Columns>
						<asp:BoundColumn DataField="Tran_Code" HeaderText="Trans Code"></asp:BoundColumn>
						<asp:BoundColumn DataField="Tran_Desc" HeaderText="Trans Desc"></asp:BoundColumn>
						<asp:BoundColumn DataField="Output_FileType_Id" HeaderText="FileType Id" Visible="false"></asp:BoundColumn>
						<asp:BoundColumn DataField="Input_FileType_Id" HeaderText="In_FileType Id" Visible="false"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
							<ItemTemplate>
								<a href="pg_TransCode.aspx?FTA=2&TI=<%#DataBinder.Eval(Container.DataItem,("Tran_Id"))%>&FTI=
								<%#DataBinder.Eval(Container.DataItem,("Output_FileType_Id"))%>&INFI=
								<%#DataBinder.Eval(Container.DataItem,("Input_FileType_Id"))%>">Modify</a>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Datagrid ends here -->
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Form Table Ends Here -->
	<input type="hidden" runat="server" id="__FileTypeAction" name="__FileTypeAction" value="1"/>
	<input type="hidden" runat="server" id="__TransID" name="__TransID" value="0"/>
	<input type="hidden" runat="server" id="__FiletypeIDQuery" name="__FiletypeIDQuery" />
    <asp:Literal ID="ltlFileID" runat="server" Visible="false"></asp:Literal>
</asp:Content>

