<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_FileSettings.aspx.vb" Inherits="MaxPayroll.WebForms_pg_FileSettings" title="File Settings Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<!-- Form Table Starts Here -->
<asp:Label Text="File Settings" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label><br /><br />
<asp:Label runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label><br />
<asp:Table ID="tblForm" Runat="Server" CellPadding="2" CellSpacing="1" Width="100%">
<asp:TableRow  >
	<asp:TableCell Width="20%" >
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Description" Text="Description"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="__Field_Description" Runat="server" CssClass="LARGETEXT"></asp:TextBox>
	
		&nbsp;<asp:RequiredFieldValidator ID="reqfDescription" runat="server" ErrorMessage="Please Enter Description"
    ControlToValidate="__Field_Description"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="__Field_Description" ValidationExpression="[A-Za-z0-9]" ErrorMessage="Description Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
    </asp:TableCell>
</asp:TableRow>
<asp:TableRow>
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_DataType" Text="DataType"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_DataType" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>

<asp:TableRow>
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_ContentType" Text="Content Type "></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_ContentType" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_FieldMandatory">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Mandatory" Text="Mandatory Field"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_Mandatory" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow id="tr_FieldDefault">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_DefaultValue" Text="Default Value"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="__Field_DefaultValue" Runat="server" CssClass="LARGETEXT"></asp:TextBox>   
         <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="__Field_DefaultValue" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Field Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Options" Text="Options"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_Options" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_Match">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Match" Text="Match Field"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_Match" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Operator" Text="Operator"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_Operator" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_FillerType">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Filler_Type" Text="Filler Type"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Filler_Type" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_FillerChar" HorizontalAlign="Left">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Filler_Char" Text="Filler Char Code"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="__Filler_Char" Runat="server" CssClass="LARGETEXT" MaxLength="3"></asp:TextBox>
	&nbsp;<asp:RequiredFieldValidator ID="reqfFillerCode" runat="server" ErrorMessage="Please Enter Filler Code"
    ControlToValidate="__Filler_Char"></asp:RequiredFieldValidator>	
	<asp:RegularExpressionValidator ID="regxFiller" runat="server" ErrorMessage="Numeric Only" 
    ControlToValidate="__Filler_Char" ValidationExpression="^\d+$" ></asp:RegularExpressionValidator>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow ID="tr_FieldRepeat">
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Repeat" Text="Repeat Field"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:DropDownList ID="__Field_Repeat" Runat="server" CssClass="LARGETEXT">
		</asp:DropDownList>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow >
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_Start" Text="Start Field"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="__Field_Start" Runat="server" CssClass="LARGETEXT" MaxLength="3" ></asp:TextBox>
		&nbsp;<asp:RequiredFieldValidator ID="reqfStartField" runat="server" ErrorMessage="Please Enter Start Field"
    ControlToValidate="__Field_Start"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="rgexStart" runat="server" ErrorMessage="Numeric Only" 
    ControlToValidate="__Field_Start" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
		</asp:TableCell>
</asp:TableRow>
<asp:TableRow >
	<asp:TableCell Width="20%">
		<asp:Label Runat="server" CssClass="LABEL" ID="Field_End" Text="End Field"></asp:Label>
	</asp:TableCell>
	<asp:TableCell Width="80%">
		<asp:TextBox ID="__Field_End" Runat="server" CssClass="LARGETEXT" MaxLength="3"></asp:TextBox>
		
		&nbsp;<asp:RequiredFieldValidator ID="reqfEndField" runat="server" ErrorMessage="Please Enter End Field"
    ControlToValidate="__Field_End"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="regxEnd" runat="server" ErrorMessage="Numeric Only" 
    ControlToValidate="__Field_End" ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
	</asp:TableCell>
</asp:TableRow>
<asp:TableRow>
	<asp:TableCell Width="100%" ColumnSpan="2">
		<asp:Button ID="btnSubmit" Runat="server" CssClass="BUTTON" Text="Submit"></asp:Button>&nbsp;
		<input type="reset" id="btnReset" runat="server" name="btnReset" class="BUTTON" value="Reset" />&nbsp;
		<asp:Button ID="btnCreate" runat="server" CssClass="BUTTON" Text="New" CausesValidation="False"/>
	</asp:TableCell>
</asp:TableRow>
</asp:Table>
<input type="hidden" runat="server" id="__Field_Id" name="__Field_Id" value="0"/>
<input type="hidden" runat="server" id="__FileTypeAction" name="__FileTypeAction" value="1"/>
<!-- Form Table Ends Here -->

</asp:Content>

