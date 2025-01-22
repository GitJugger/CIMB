<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_Column_Field_Relation.aspx.vb" Inherits="WebForms_pg_Column_Field_Relation" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<asp:Label ID="lblMsg" runat="server"  CssClass="MSG"></asp:Label>

<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">

<asp:TableRow >
<asp:TableCell>
<asp:Label ID="lblHeading" runat="server" Text="Select Field" CssClass="FORMHEAD"></asp:Label>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow >
<asp:TableCell>
    <asp:RadioButtonList ID="rblContent" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
        <asp:ListItem Value="H">Header_Fields</asp:ListItem>
        <asp:ListItem Value="F">Footer_Fields</asp:ListItem>
    </asp:RadioButtonList>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow ></asp:TableRow>
<asp:TableRow ID="tr_FileType" >
<asp:TableCell Width="20%">
	<asp:Label Runat="server" CssClass="LABEL" ID="FileType_Id" Text="Select FileType"></asp:Label>
</asp:TableCell>
<asp:TableCell Width="80%">
	<asp:DropDownList ID="__FileType" Runat="server" CssClass="LARGETEXT" AutoPostBack="True">
	</asp:DropDownList>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_FieldDesc" >
<asp:TableCell Width="20%">
	<asp:Label Runat="server" CssClass="LABEL" ID="lblField_Desc" Text="Select Field"></asp:Label>
</asp:TableCell>
<asp:TableCell Width="80%">
	<asp:DropDownList ID="__Field_Desc" Runat="server" CssClass="LARGETEXT">
	</asp:DropDownList>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
    ErrorMessage="Please Select Field" ControlToValidate="__Field_Desc"></asp:RequiredFieldValidator>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_MatchField" >
<asp:TableCell Width="20%">
	<asp:Label Runat="server" CssClass="LABEL" ID="lblMatch" Text="Select Match Field"></asp:Label>
</asp:TableCell>
<asp:TableCell Width="80%">
	<asp:DropDownList ID="__FieldColumn" Runat="server" CssClass="LARGETEXT">
	</asp:DropDownList>
</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
<asp:TableCell>
    <asp:Button ID="btnSave" runat="server" Text="Save" />
</asp:TableCell>
</asp:TableRow>
</asp:Table>
    <asp:Label ID="lblNote" runat="server" Text="* Note : Please Select Field1,Field2,...... For Match Field" ForeColor="red">
    </asp:Label>
    <asp:Table ID="tblGrid" runat="server">
    <asp:TableRow>
    <asp:TableCell>
    				<!-- Datagrid starts here -->
					<asp:DataGrid ID="dgFileType" Runat="Server" GridLines="Both" CellPadding="3" CellSpacing="0" 
					    HeaderStyle-CssClass="GRIDHEAD" ItemStyle-CssClass="GRIDITEM" width="100%" CssClass="DATAGRID"
						AlternatingItemStyle-CssClass="GRIDALTERITEM" AutoGenerateColumns="False" >
					<Columns>
						<asp:BoundColumn DataField="Bank_Field_Desc" HeaderText="FileType_Name"></asp:BoundColumn>
						<asp:BoundColumn DataField="FieldName" HeaderText="Match Field Name"></asp:BoundColumn>
					</Columns>
				</asp:DataGrid>
				<!-- Datagrid ends here -->
    </asp:TableCell>
    </asp:TableRow>
    </asp:Table>
</asp:Content>

