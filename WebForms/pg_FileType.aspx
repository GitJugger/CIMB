<%@ Page Language="VB" AutoEventWireup="false" Inherits="MaxPayroll.WebForms_pg_FileType" CodeFile="pg_FileType.aspx.vb" 
MasterPageFile="~/WebForms/mp_Master.master" title="File Type Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">
<asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label><br /><br />
    <asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
    <!-- Form Table Starts Here -->
    
    	<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
    	<asp:TableRow ID="tr_FileProcess">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Process" Text="FileType Process"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__FileType_Process" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
    	<asp:TableRow ID="tr_inputid">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Input_Filetype_Id" Text="Inward File Id"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Input_Filetype_Id" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
    	<asp:TableRow ID="tr_outputid">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Output_Filetype_Id" Text="OutWard Filetype"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__Output_Filetype_Id" Runat="server" CssClass="LARGETEXT">
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
		<asp:TableRow ID="tr_ErrorFolder">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Error_Folder" Text="Error Folder"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Error_Folder" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
			&nbsp;<asp:RequiredFieldValidator ID="reqfErrorFolder" runat="server" ErrorMessage="Please Enter Error Folder Location"
                ControlToValidate="__FileType_Error_Folder"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_Archive">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Archive_Folder" Text="Archive Folder"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Archive_Folder" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
			&nbsp;<asp:RequiredFieldValidator ID="reqfArchive" runat="server" ErrorMessage="Please Enter Archive Folder Location"
                ControlToValidate="__FileType_Archive_Folder"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_OrgStart">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_OrgStart" Text="Organization Start Field"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_OrgStart" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
			&nbsp;<asp:RequiredFieldValidator ID="reqfOrgStart" runat="server" ErrorMessage="Please Enter Org Start Field"
                ControlToValidate="__FileType_OrgStart"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_OrgEnd">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_OrgEnd" Text="Organization End Field"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_OrgEnd" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
			&nbsp;<asp:RequiredFieldValidator ID="reqfOrgEnd" runat="server" ErrorMessage="Please Enter Org End Field"
                ControlToValidate="__FileType_OrgEnd"></asp:RequiredFieldValidator>
			</asp:TableCell>
		</asp:TableRow>
		
				<asp:TableRow ID="tr_TrailerStart">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="TrailerStart" Text="Trailer Start"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_TrailerStart" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
			<%--&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please Enter Trailer Start"
                ControlToValidate="__FileType_TrailerStart"></asp:RequiredFieldValidator>--%>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_TrailerEnd">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="TrailerEnd" Text="Trailer End"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_TrailerEnd" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
		<%--	&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please Enter Trailer End"
                ControlToValidate="__FileType_TrailerEnd"></asp:RequiredFieldValidator>--%>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_TrailerValue">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="TrailerValue" Text="Trailer Value"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_TrailerValue" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
				
		<%--	&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please Enter Trailer Value"
                ControlToValidate="__FileType_TrailerValue"></asp:RequiredFieldValidator>--%>
			</asp:TableCell>
		</asp:TableRow>
		
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Header" Text="Header Lines"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Header" Runat="server" CssClass="LARGETEXT" Text="0" MaxLength="2"></asp:TextBox>
			
			&nbsp;<asp:RegularExpressionValidator ID="rgexHeader" runat="server" ErrorMessage="Header lines Must be Numeric" 
                    ControlToValidate="__FileType_Header" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow >
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Footer" Text="Footer Lines"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__FileType_Footer" Runat="server" CssClass="LARGETEXT" Text="0" MaxLength="2"></asp:TextBox>
			
			&nbsp;<asp:RegularExpressionValidator ID="regxFooter" runat="server" ErrorMessage="Footer Lines Must be Numeric" 
                ControlToValidate="__FileType_Footer" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
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
				
		<asp:TableRow ID="tr_outname">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="Filetype_Outname" Text="OutName"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="__Filetype_Outname" Runat="server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
			
			&nbsp;<asp:RequiredFieldValidator ID="reqfOutname" runat="server" ErrorMessage="Please Enter OutName"
                ControlToValidate="__Filetype_Outname"></asp:RequiredFieldValidator>
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
		<asp:TableRow ID="tr_Trailer_Display">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="TrailerDisplay" Text="Trailer Display"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__FileType_Trailer_Display" Runat="server" CssClass="LARGETEXT">
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="tr_FiletypeTable">
			<asp:TableCell Width="20%">
				<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Table" Text="FileType Table"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:DropDownList ID="__FileType_Table" Runat="server" CssClass="LARGETEXT">
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
								<a href="pg_FileType.aspx?FTA=2&FTI=<%#DataBinder.Eval(Container.DataItem,("FileType_Id"))%>">Modify</a>
							</ItemTemplate>
						</asp:TemplateColumn>
						<asp:TemplateColumn HeaderText="Action" ItemStyle-HorizontalAlign="center">
							<ItemTemplate>
								<a href="pg_FileListing.aspx?FTI=<%#DataBinder.Eval(Container.DataItem,("FileType_Id"))%>
								&Name=<%#DataBinder.Eval(Container.DataItem,("FileType_Name"))%>
								&HLine=<%#DataBinder.Eval(Container.DataItem,("FileType_Header"))%>
								&FLine=<%#DataBinder.Eval(Container.DataItem,("FileType_Footer"))%>
								&Req=1">File Settings</a>
							</ItemTemplate>
						</asp:TemplateColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Datagrid ends here -->
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	<!-- Form Table Ends Here -->
	<input type="hidden" runat="server" id="__FileType" name="__FileType" value="0"/>
	<input type="hidden" runat="server" id="__FileType_Id" name="__FileType_Id" value="0"/>
	<input type="hidden" runat="server" id="__FileTypeAction" name="__FileTypeAction" value="1"/>
	<asp:ValidationSummary Runat="Server" ID="vsFileType" EnableClientScript="True" ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
</asp:Content>