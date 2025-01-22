<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_CreateGroup" CodeFile="PG_CreateGroup.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			//window.history.back();
	    window.location='pg_creategroup.aspx'
		}
		</script>
    
 
    <!-- Main Table Starts Here -->
      <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Create Group" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td>
               <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
    <!-- Main Table Ends Here -->

    <!-- Main Form Table Starts Here -->
    <asp:Table ID="tblMainForm" CellPadding="5" CellSpacing="1" Runat="Server" BorderWidth="0" Width="90%">
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False">
			<asp:Label CssClass="LABEL" Runat="Server" Text="Group Name"></asp:Label>&nbsp;
			<asp:Label CssClass="MAND" Runat="Server" Text="*"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:TextBox ID="txtGroupName" Runat="Server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
             <asp:RegularExpressionValidator
                                            ID="revtxtGroupName" runat="server" ControlToValidate="txtGroupName" Display="None"
                                            ErrorMessage="Group Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Group Description"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:TextBox ID="txtGroupDesc" Runat="Server" CssClass="LARGETEXT" TextMode="MultiLine" Rows="3" Columns="10"></asp:TextBox>
             <asp:RegularExpressionValidator
                                            ID="revGroupDesc" runat="server" ControlToValidate="txtGroupDesc" Display="None"
                                            ErrorMessage="Group Desc Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Account(s)"></asp:Label>&nbsp;
			<asp:Label Runat="Server" CssClass="MAND" Text="*"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:CheckBoxList ID="chkBankAccts" Runat="Server" CssClass="LABEL" RepeatColumns="2">
			</asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="trEpfAccounts">
		<asp:TableCell Width="20%">
			<asp:Label Runat="server" CssClass="LABEL" Text="EPF Number(s)"></asp:Label>&nbsp;
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label10" NAME="Label10"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:CheckBoxList ID="chkEpfAccts" Runat="server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="trSocAccounts">
		<asp:TableCell Width="20%">
			<asp:Label Runat="server" CssClass="LABEL" Text="SOCSO Number(s)"></asp:Label>
			<asp:Label Runat="server" CssClass="MAND" Text="*"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:CheckBoxList ID="chkSocAccts" Runat="server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="trLHDNAccounts">
		<asp:TableCell Width="20%">
			<asp:Label Runat="server" CssClass="LABEL" Text="LHDN Number(s)"></asp:Label>
			<asp:Label Runat="server" CssClass="MAND" Text="*"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:CheckBoxList ID="chkLHDNAccts" Runat="server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="trPayroll">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Payroll File Format(s)"></asp:Label>
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label4"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:CheckBoxList ID="chkPayroll" Runat="Server" CssClass="LABEL" RepeatColumns="2">
			</asp:CheckBoxList>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trEPF">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="EPF File Format(s)"></asp:Label>
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label7"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:CheckBoxList ID="chkEpf" Runat="Server" CssClass="LABEL" RepeatColumns="2">
			</asp:CheckBoxList>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trSoc">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="SOCSO File Format(s)"></asp:Label>
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label15"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:CheckBoxList ID="chkSocso" Runat="Server" CssClass="LABEL" RepeatColumns="2">
			</asp:CheckBoxList>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trLHDN">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="LHDN File Format(s)" ID="Label16" NAME="Label16"></asp:Label>
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label17"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:CheckBoxList ID="chkLHDN" Runat="Server" CssClass="LABEL" RepeatColumns="2">
			</asp:CheckBoxList>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False">
			<asp:Label Runat="Server" CssClass="LABEL" Text="No of Approver's for Approval"></asp:Label>&nbsp;
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label3"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:DropDownList ID="ddlAuthorizer" CssClass="MINITEXT" Runat="Server">
				<asp:ListItem Value="0"></asp:ListItem>
				<asp:ListItem Value="1">1</asp:ListItem>
				<asp:ListItem Value="2">2</asp:ListItem>
				<asp:ListItem Value="3">3</asp:ListItem>
				<asp:ListItem Value="4">4</asp:ListItem>
				<asp:ListItem Value="5">5</asp:ListItem>
				<asp:ListItem Value="6">6</asp:ListItem>
				<asp:ListItem Value="7">7</asp:ListItem>
				<asp:ListItem Value="8">8</asp:ListItem>
				<asp:ListItem Value="9">9</asp:ListItem>
			</asp:DropDownList>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%">
			<asp:Label Runat="Server" Text="Status" CssClass="LABEL"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:RadioButtonList ID="rdStatus" Runat="Server" RepeatDirection="Horizontal" CssClass="LABEL">
				<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
				<asp:ListItem Value="C">Inactive</asp:ListItem>
			</asp:RadioButtonList>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="90%" ColumnSpan="4">
			<asp:Button ID="btnSubmit" Text="Submit" CssClass="BUTTON" Runat="Server"></asp:Button>&nbsp;
			<input type="reset" id="btnReset" onclick="" name="btnReset" value="Clear" class="BUTTON" />&nbsp;
			<asp:Button CssClass="BUTTON" Runat="server" ID="btnView" Text="Back to view"></asp:Button>
		</asp:TableCell>
    </asp:TableRow>
    </asp:Table>
    <input type="hidden" id="hEpf" name="hEpf" runat="Server"/>
    <input type="hidden" id="hSoc" name="hSoc" runat="Server"/>
    <input type="hidden" id="hStatus" name="hStatus" runat="Server"/>
    <input type="hidden" id="hPayroll" name="hPayroll" runat="Server"/>
    <input type="hidden" id="hLHDN" name="hLHDN" runat="Server"/>
    <asp:RequiredFieldValidator ID="rfvGroupName" Runat="Server" ControlToValidate="txtGroupName" ErrorMessage="Group Name cannot be blank" Display="None"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rngGroupAuth" Runat="Server" ControlToValidate="ddlAuthorizer" Type="Integer" MinimumValue="1" MaximumValue="9" ErrorMessage="Please select the number of Approver's for Approval" Display="None"></asp:RangeValidator>
    <!-- Main Form Table Ends Here -->
    
    <!-- Confirm Table Starts Here -->
    <asp:Table ID="tblConfirm" CellPadding="2" CellSpacing="1" Runat="Server" BorderWidth="1" Width="90%">
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False">
			<asp:Label CssClass="LABEL" Runat="Server" Text="Group Name" ID="Label1"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:TextBox ID="txtCGroupName" Runat="Server" CssClass="LARGETEXT" ReadOnly="True"></asp:TextBox>
            <asp:RegularExpressionValidator
                                            ID="revGroupName" runat="server" ControlToValidate="txtCGroupName" Display="None"
                                            ErrorMessage="Group Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Group Description" ID="Label5"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:TextBox ID="txtCGroupDesc" Runat="Server" CssClass="LARGETEXT" TextMode="MultiLine" Rows="3" Columns="10" ReadOnly="True"></asp:TextBox>
            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCGroupDesc" Display="None"
                                            ErrorMessage="Group Desc Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Accounts" ID="Label6"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCBankAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trCEpfAccounts">
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="EPF Number(s)" ID="Label11"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCEpfAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
     <asp:TableRow ID="trCSocAccounts">
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Socso Number(s)" ID="Label12"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCSocAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trCLHDNAccounts">
		<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="LHDN Number(s)" ID="Label14"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCLHDNAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trCPayroll">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Payroll File Format" ID="Label8"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCPyrFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trCEpf">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="EPF File Format" ID="Label9"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCEpfFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trCSoc">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="SOCSO File Format"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCSocFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trCLHDN">
		<asp:TableCell Width="20%" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" text="LHDN File Format"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%">
			<asp:ListBox ID="lbxCLHDNFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%" Wrap="False">
			<asp:Label Runat="Server" CssClass="LABEL" Text="No of Approver's for Approval"></asp:Label>&nbsp;
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:TextBox ID="txtCAuth" Runat="Server" CssClass="MINITEXT" ReadOnly="True"></asp:TextBox>
            <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCAuth" Display="None"
                                            ErrorMessage="Auth Accepts Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
		<asp:TableCell Width="30%">
			<asp:Label Runat="Server" Text="Status" CssClass="LABEL" ID="Label13"></asp:Label>
		</asp:TableCell>
		<asp:TableCell Width="60%" ColumnSpan="3">
			<asp:TextBox ID="txtStatus" Runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trAuthCode">
		<asp:TableCell Width="30%">
			<asp:Label Runat="Server" Text="Enter Validation Code" CssClass="LABEL"></asp:Label>
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label2"></asp:Label>
		</asp:TableCell>
		<asp:TableCell>
			<asp:TextBox ID="txtAuthCode" CssClass="MEDIUMTEXT" MaxLength="14" TextMode="Password" Runat="Server"></asp:TextBox>
           
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trConfirm">
		<asp:TableCell Width="90%" ColumnSpan="4">
			<asp:Button ID="btnConfirm" Text="Confirm" CssClass="BUTTON" Runat="Server"></asp:Button>&nbsp;
			<input type="button" id="btnBack" name="btnBack" value="Back" onclick="fncBack();" class="BUTTON">&nbsp;
		</asp:TableCell>
    </asp:TableRow>
    <asp:TableRow ID="trNew" Visible=False>
		<asp:TableCell Width="90%" ColumnSpan="4">
			<asp:Button ID="btnNew" Runat="server" Text="Create another group"></asp:Button>
		</asp:TableCell>
    </asp:TableRow>
    </asp:Table>
    <!-- Confirm Table Ends Here -->
    <asp:RequiredFieldValidator ID="rfvAuthCode" Runat="Server" ErrorMessage="Validation Code Cannot be blank" Display="None" ControlToValidate="txtAuthCode"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Validation Code must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
	<asp:ValidationSummary ID="vsCreateGroup" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>

</asp:Content>
