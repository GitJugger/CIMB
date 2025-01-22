<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_BankField" CodeFile="PG_BankField.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

		<base target="_self"/>
		<script type="text/javascript" src="../include/common.js"></script>
		
		<script type="text/javascript">
		    function CheckIsMandatory()
		    {
		        //rblIsMandatory rblIsCustomerSetting
		        //document.forms[0].rblIsMandatory.
		        debugger;
		        var a;
		        a = 'dfd'
		    }
		</script>
			<!-- Main Table Starts Here -->
			<table id="tblMain" runat="server" cellpadding="5" cellspacing="0" width="100%" border="0">
			<tr>
					<td  id="cHeader"><asp:Label Runat="Server" ID="lblHead" Text="Create / Modify Bank File Settings" CssClass="FORMHEAD"></asp:Label></td>
				</tr>
		  </table>
	    <table cellpadding="0" cellspacing="8" border="0" style="width:100%">
				<tr>
					<td width="100%">
						<!-- Form Table Starts Here -->
						<asp:Table CellPadding="2" CellSpacing="0" Width="100%" Runat="Server" BorderWidth="0" ID="tbl">
                            				
							<asp:TableRow ID="trMsg">
								<asp:TableCell Width="100%" Runat="Server" ColumnSpan="2">
									<asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
                                <asp:TableCell ID="TableCell1" Width="20%" Runat="Server">
                                   <asp:Label ID="lblBankCode" CssClass="LABEL" runat="server">Bank</asp:Label>
                                </asp:TableCell>
                                <asp:TableCell ID="TableCell2" Width="80%" Runat="Server">
                                   <asp:TextBox Runat="Server" ID="txtBankDesc" ReadOnly="True" CssClass="MEDIUMTEXT" AutoPostBack="False"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="20%" Runat="Server">
									<asp:Label Runat="Server" ID="lblFile" Text="File Type" CssClass="LABEL"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:TextBox Runat="Server" ID="txtFileType" ReadOnly="True" CssClass="MEDIUMTEXT" AutoPostBack="False"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							
							<%--ADDED FOR TRICOR-START--%>
							<asp:TableRow ID="trFileFormat">
			<asp:TableCell>
				<asp:Label ID="Label6" Runat="Server" Text="Select File Format" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList Runat="Server" ID="__ddlFileFormat" CssClass="BIGTEXT" AutoPostBack="True">
					
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trdelimiter" Visible="false">
			<asp:TableCell>
				<asp:Label ID="Label7" Runat="Server" Text="Select File Format" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell>
				<asp:DropDownList Runat="Server" ID="__ddlFileDelimiter" CssClass="BIGTEXT" AutoPostBack="True">
					
				</asp:DropDownList>
			</asp:TableCell>
		</asp:TableRow>
							<%--TRICOR END--%>
							<asp:TableRow>
								<asp:TableCell Width="20%" Runat="Server"><asp:Label ID="lblDesc" Runat="Server" CssClass="LABEL" Text="Field Description"></asp:Label>&nbsp;<asp:Label CssClass="MAND" Runat="Server" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:TextBox ID="txtFldDesc" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="50" AutoPostBack="False"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="20%" Runat="Server"><asp:Label Runat="Server" CssClass="LABEL" Text="Field Data Type"></asp:Label>&nbsp;<asp:Label Runat="Server" CssClass="MAND" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:DropDownList Runat="Server" CssClass="SMALLTEXT" ID="cmbDataType" Width="120">
										<asp:ListItem Value="">Select</asp:ListItem>
										<asp:ListItem Value="N">Numeric</asp:ListItem>
										<asp:ListItem Value="C">Character</asp:ListItem>
									</asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="20%" Runat="Server"><asp:Label ID="lblFldType" Runat="Server" CssClass="LABEL" Text="Content Type"></asp:Label>&nbsp;<asp:Label CssClass="MAND" Runat="Server" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
					<asp:DropDownList ID="cmbFldType" Runat="Server" CssClass="SMALLTEXT" Width ="120">
										<asp:ListItem Value="">Select</asp:ListItem>
										<asp:ListItem Value="N">Bank Header</asp:ListItem>
										<asp:ListItem Value="H">Header</asp:ListItem>
										<asp:ListItem Value="B">Body</asp:ListItem>
										<asp:ListItem Value="F">Footer</asp:ListItem>
										<asp:ListItem Value="T">Trailers</asp:ListItem>
									</asp:DropDownList>&nbsp;
				</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="20%">
									<asp:Label Runat="Server" Text="Matching Field" CssClass="LABEL"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%">
									<asp:DropDownList ID="cmbMatchFld" Runat="Server" CssClass="BIGTEXT">
										
									</asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="20%" Runat="Server">
									<asp:Label Text="Predefined Options" CssClass="LABEL" Runat="Server" ID="Label1"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:DropDownList ID="cmbOptions" Runat="Server" CssClass="BIGTEXT">
										
									</asp:DropDownList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="20%" Runat="Server">
									<asp:Label CssClass="LABEL" Text="Default Value" Runat="Server"></asp:Label>
								</asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:TextBox Runat="Server" CssClass="MEDIUMTEXT" MaxLength="25" ID="txtDefault"></asp:TextBox>
                                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtDefault" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Default Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trStartPos" Visible="true" >
								<asp:TableCell Width="20%" Runat="Server"><asp:Label ID="lblStartPos" Runat="Server" CssClass="LABEL" Text="Start Position"></asp:Label>&nbsp;<asp:Label Text="*" Runat="Server" CssClass="MAND"></asp:Label></asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:TextBox ID="txtStartPos" Runat="Server" CssClass="MINITEXT" MaxLength="4" Text="0"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trEndPos" Visible="true" >
								<asp:TableCell Width="20%" Runat="Server"><asp:Label ID="lblEndPos" Runat="Server" CssClass="LABEL" Text="End Position"></asp:Label>&nbsp;<asp:Label CssClass="MAND" Runat="Server" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell Width="80%" Runat="Server">
									<asp:TextBox ID="txtEndPos" Runat="Server" CssClass="MINITEXT" MaxLength="4" Text="0"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trColPos" Visible="false" >
								<asp:TableCell ID="TableCell7" Width="20%" Runat="Server"><asp:Label ID="lblColPos" Runat="Server" CssClass="LABEL" Text="Column Position"></asp:Label>&nbsp;<asp:Label ID="Label9" CssClass="MAND" Runat="Server" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell ID="TableCell8" Width="80%" Runat="Server">
									<asp:TextBox ID="txtColPos" Runat="Server" CssClass="MINITEXT" MaxLength="4"></asp:TextBox>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ID="TableCell3" Width="20%" Runat="Server"><asp:Label ID="Label2" Runat="Server" CssClass="LABEL" Text="Mandatory"></asp:Label>&nbsp;<asp:Label ID="Label3" CssClass="MAND" Runat="Server" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell ID="TableCell4" Width="80%" Runat="Server">
									<asp:RadioButtonList ID="rblIsMandatory" runat="server" CssClass="LABEL" RepeatDirection="Horizontal"><asp:ListItem Text="Yes" Value="1"></asp:ListItem><asp:ListItem Text="No" Value="0"></asp:ListItem></asp:RadioButtonList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell ID="TableCell5" Width="20%" Runat="Server"><asp:Label ID="Label4" Runat="Server" CssClass="LABEL" Text="Display At Customer File Settings"></asp:Label>&nbsp;<asp:Label ID="Label5" CssClass="MAND" Runat="Server" Text="*"></asp:Label></asp:TableCell>
								<asp:TableCell ID="TableCell6" Width="80%" Runat="Server">
									<asp:RadioButtonList ID="rblIsCustomerSetting" runat="server" CssClass="LABEL" RepeatDirection="Horizontal"><asp:ListItem Text="Yes" Value="1"></asp:ListItem><asp:ListItem Text="No" Value="0"></asp:ListItem></asp:RadioButtonList>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="100%" ColumnSpan="2">&nbsp;</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="100%" Runat="Server" ColumnSpan="2">
					<asp:Button Runat="Server" ID="btnSave" Text="Save" CssClass="BUTTON"></asp:Button>&nbsp;
					<input type="reset" runat="Server" value="      Clear      " class="BUTTON"/>&nbsp;
					<asp:Button Runat="Server" ID="btnDelete" Text="Delete" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
					<asp:Button Runat="Server" ID="btnBack" Text="Back" CssClass="BUTTON" CausesValidation="False"></asp:Button>
				</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<!-- Form Table Ends Here -->
						<!-- Form Validations Starts here -->
						<asp:RequiredFieldValidator ID="rfvFldDesc" ControlToValidate="txtFldDesc" Runat="Server" ErrorMessage="Field Description Cannot Be Blank"
							Display="None"></asp:RequiredFieldValidator>
						<asp:RequiredFieldValidator ID="rfvDataType" ControlToValidate="cmbDataType" Runat="Server" ErrorMessage="Field Data Type Cannot Be Blank"
							Display="None"></asp:RequiredFieldValidator>
						<asp:RequiredFieldValidator ID="rfvFldType" ControlToValidate="cmbFldType" Runat="Server" ErrorMessage="Content Type Cannot Be Blank"
							Display="None"></asp:RequiredFieldValidator>
						<asp:RequiredFieldValidator ID="rfvStartPos" ControlToValidate="txtStartPos" Runat="Server" ErrorMessage="Start Position Cannot Be Blank"
							Display="None"></asp:RequiredFieldValidator>
						<asp:RequiredFieldValidator ID="rfvEndPos" ControlToValidate="txtEndPos" Runat="Server" ErrorMessage="End Position Cannot Be Blank"
							Display="None"></asp:RequiredFieldValidator>
							<asp:RequiredFieldValidator ID="rfvColPos" ControlToValidate="txtColPos" Runat="Server" ErrorMessage="Column Position Cannot Be Blank"
							Display="None"></asp:RequiredFieldValidator>
						<asp:RegularExpressionValidator ID="revStartPos" Runat="Server" ControlToValidate="txtStartPos" ErrorMessage="Start Position Should be Numeric"
							Display="None" ValidationExpression="[0-9]{1,4}"></asp:RegularExpressionValidator>
						<asp:RegularExpressionValidator ID="revEndPos" Runat="Server" ControlToValidate="txtEndPos" ErrorMessage="End Position Shoubld be Numeric"
							Display="None" ValidationExpression="[0-9]{1,4}"></asp:RegularExpressionValidator>
							<asp:RegularExpressionValidator ID="revColPos" Runat="Server" ControlToValidate="txtColPos" ErrorMessage="Column Position Shoubld be Numeric"
							Display="None" ValidationExpression="[0-9]{1,4}"></asp:RegularExpressionValidator>
						<asp:ValidationSummary Runat="Server" ID="vsBankFile" ShowMessageBox="True" EnableClientScript="True" ShowSummary="False"
							HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
						<!-- Form Validations Ends here -->
					</td>
				</tr>
			</table>
			<!-- Main Table Ends Here -->
			<input type="hidden" name="hFieldId" id="hFieldId" runat="server"/> 
			<input type="hidden" name="hFieldDesc" id="hFieldDesc" runat="Server"/>
			<input type="hidden" name="hFieldType" id="hFieldType" runat="Server"/> 
			<input type="hidden" name="hFieldCont" id="hFieldCont" runat="Server"/>
			<input type="hidden" name="hFieldMatch" id="hFieldMatch" runat="Server"/> 
			<input type="hidden" name="hFieldOption" id="hFieldOptions" runat="Server"/>
			<input type="hidden" name="hFieldDefault" id="hFieldDefault" runat="Server"/> 
			<input type="hidden" name="hFieldStart" id="hFieldStart" runat="Server"/>
			<input type="hidden" name="hFieldEnd" id="hFieldEnd" runat="Server"/>
			
			<input type="hidden" name="hFieldMandatory" id="hidIsMandatory" runat="Server"/>
			<input type="hidden" name="hFieldCustomerSetting" id="hidIsCustomerSetting" runat="Server"/>

</asp:Content>