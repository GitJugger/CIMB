<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_FileTypeSettings.aspx.vb" Inherits="WebForms_pg_FileTypeSettings" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label><br /><br />
    <asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
    <!-- Form Table Starts Here -->
    
    	<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">

    	<asp:TableRow ID="tr_FileTypeid">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Filetype_Id" Text="File Type"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Filetype_Id" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" CssClass="LABEL" ID="FileType_Name" Text="File Type Name"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Name" Runat="server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
			
			 &nbsp;<asp:RequiredFieldValidator ID="reqfFiletypeName" runat="server" ErrorMessage="Please Enter FiletypeName"
                ControlToValidate="__FileType_Name"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_folder">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Folder" Text="Folder Name"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Folder" Runat="server" CssClass="LARGETEXT" ></asp:TextBox>
				
			&nbsp;<asp:RequiredFieldValidator ID="reqfFolder" runat="server" ErrorMessage="Please Enter Foldername"
                ControlToValidate="__FileType_Folder"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		
		   <asp:TableRow ID="tr_MultiPleHeader">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="__Is_Multiple_Header" ID="Label1" Text="Is Multiple Header"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Is_MultipleHeader" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		
				<asp:TableRow ID="tr_BodyLines">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="__BodyLines" Text="No Of Body Lines"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__BodyLines_PerHeader" Runat="server" CssClass="LARGETEXT" ></asp:TextBox>
		
			&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter No Of Lines"
                ControlToValidate="__BodyLines_PerHeader"></asp:RequiredFieldValidator>
                &nbsp; <asp:RegularExpressionValidator ID="rgetxtheader" runat="server" Display="None"
        ErrorMessage="header is invalid" ControlToValidate="__BodyLines_PerHeader"
        ValidationExpression="^\d{1,1000}$"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Extn" Text="File Extension"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Extn" Runat="server" CssClass="LARGETEXT" MaxLength="5"></asp:TextBox>
			
			&nbsp;<asp:RequiredFieldValidator ID="reqfExtn" runat="server" ErrorMessage="Please Enter FileExtension"
                ControlToValidate="__FileType_Extn"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
				
		<asp:TableRow ID="tr_status">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Status" Text="File Status"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__FileType_Status" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>

		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Button ID="btnSubmit" Runat="server" CssClass="BUTTON" Text="Submit"></asp:Button>&nbsp;
				<input type="reset" id="btnReset" runat="server" name="btnReset" class="BUTTON" value="Reset" />&nbsp;
				<asp:Button ID="btnCreate" runat="server" CssClass="BUTTON" Text="New" CausesValidation="False"/>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow><asp:TableCell Width="100%" ColumnSpan="2">&nbsp;</asp:TableCell></asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
				<!-- Datagrid starts here -->
					<asp:DataGrid ID="dgFileType" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
					    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
						AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False" >
					<Columns>
						<asp:BoundColumn DataField="FileType_Name" HeaderText="FileType_Name"></asp:BoundColumn>
						<asp:BoundColumn DataField="FileType_Folder" HeaderText="FileType_Folder"></asp:BoundColumn>
						<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
							<ItemTemplate>
								<a href="pg_FileTypeSettings.aspx?FTA=2&FTI=<%#DataBinder.Eval(Container.DataItem,("FileType_Id"))%>">Modify</a>
							</ItemTemplate>
						</asp:TemplateColumn>

					</Columns>
				</asp:DataGrid>
				<!-- Datagrid ends here -->
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Form Table Ends Here -->
		<input type="hidden" runat="server" id="__FileTypeId" name="__FileTypeId" value="0"/>
			<input type="hidden" runat="server" id="__FileTypeAction" name="__FileTypeAction" value="1"/>
    <asp:ValidationSummary Runat="Server" ID="vsFileTypeSetting" EnableClientScript="True" ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
</asp:Content>

