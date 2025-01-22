<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_BankMaster"
    CodeFile="pg_BankMaster.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <!--title and error msg table starts here-->
    <input type="hidden" id="hidBankID" runat="server" />
    <input type="hidden" id="hidBankState" runat="server" />
    <input type="hidden" id="hidStatus" runat="server" />
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">Bank Code Creation</asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" CssClass="MSG" ID="lblErrorMessage"></asp:Label></td>
        </tr>
    </table>
    <!--title and error msg table ends here-->
    <!--table create starts here-->
    <asp:Table ID="tblCreate" CellPadding="8" CellSpacing="0" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <!--tblerror starts here-->
                <asp:Table ID="tblError" CellSpacing="0" CellPadding="2" Width="100%" runat="server"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell Width="100%">
                            <asp:Label runat="server" CssClass="MSG" ID="Label20"></asp:Label>
                            <asp:Label runat="server" CssClass="PANELHEADING" ID="lblWarning"></asp:Label>
                            <!-- Validation Summmary Start Here -->
                            <asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
                            <!-- Validation Summmary End Here -->
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--tblerror ends here-->
                <!--BCC starts here-->
                <asp:Table Width="100%" ID="Table2" runat="server" CellPadding="2" CellSpacing="0"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell Width="20%">
                            <asp:Label ID="Label1" runat="server" Text="Bank Code" CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label22" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Width="80%">
                            <asp:TextBox ID="txtBankCode" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="7" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label2" runat="server" Text="Bank Name" CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label23" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankName" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label4" runat="server" Text="Address" CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label24" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankAdd1" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                TextMode="MultiLine" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label7" runat="server" Text="State" CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label25" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlBankState" runat="server" Width="144px" TabIndex="2">
                                <asp:ListItem Value="">Select State</asp:ListItem>
                                <asp:ListItem Value="1">Johor</asp:ListItem>
                                <asp:ListItem Value="2">Kedah</asp:ListItem>
                                <asp:ListItem Value="3">Kelantan</asp:ListItem>
                                <asp:ListItem Value="4">Melaka</asp:ListItem>
                                <asp:ListItem Value="5">Negeri Sembilan</asp:ListItem>
                                <asp:ListItem Value="6">Pahang</asp:ListItem>
                                <asp:ListItem Value="7">Penang</asp:ListItem>
                                <asp:ListItem Value="8">Perak</asp:ListItem>
                                <asp:ListItem Value="9">Perlis</asp:ListItem>
                                <asp:ListItem Value="10">Sarawak</asp:ListItem>
                                <asp:ListItem Value="11">Sabah</asp:ListItem>
                                <asp:ListItem Value="12">Selangor</asp:ListItem>
                                <asp:ListItem Value="13">Terengganu</asp:ListItem>
                                <asp:ListItem Value="14">W. P. Kuala Lumpur</asp:ListItem>
                                <asp:ListItem Value="15">W. P. Labuan</asp:ListItem>
                                <asp:ListItem Value="16">W. P. Putrajaya</asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label8" runat="server" Text="Post Code" CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label26" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankPostCode" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="7" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label9" runat="server" Text="Phone No." CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label27" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankPhone" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="15" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label10" runat="server" Text="Fax" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankFax" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="15" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label11" runat="server" Text="Bank Contact Person" CssClass="LABEL"></asp:Label>
                            <asp:Label ID="Label29" Text="*" CssClass="MSG" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankContPerson" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label12" runat="server" Text="Website Address" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtBankURL" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="60" Width="200" TabIndex="2"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="Label13" runat="server" Text="Status" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:RadioButtonList ID="radActive" runat="server" CssClass="LABEL" RepeatDirection="Horizontal">
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--BCC ends here-->
                <!--Button Table starts Here-->
                <asp:Table ID="tblButton" CellSpacing="0" CellPadding="3" Width="100%" runat="server"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnSubmit" Text="Save" Width="100px" runat="server"></asp:Button><asp:Button
                                ID="btnUpdate" Text="Update" Width="100px" runat="server"></asp:Button><asp:Button
                                    ID="btnConfirm" Text="Confirm" Width="100px" runat="server"></asp:Button><asp:Button
                                        ID="btnBackToView" Text="Back" Width="100px" runat="server" Visible="false"></asp:Button><input
                                            type="reset" value="Clear" runat="Server" class="BUTTON" id="btnReset" name="Reset1" />
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--Button Table ends Here-->
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <!--table create ends here-->
    <!--table submit starts here-->
    <asp:Table ID="tblSubmit" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <!--title and error message starts here-->
                <asp:Table runat="server" ID="Table5" CellPadding="1" CellSpacing="2" Width="100%">
                    <asp:TableRow>
                        <asp:TableCell Width="100%" HorizontalAlign="Center">
                            <asp:Label ID="Label21" Text="Bank Code Creation" CssClass="FORMHEAD" runat="server"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell Width="100%">&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell HorizontalAlign="Center">
                            <asp:Label ID="lblMessage" runat="server" CssClass="MSG"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--title and error message ends here-->
                <!--Main Table starts here-->
                <asp:Table ID="Table6" runat="server" CellPadding="1" CellSpacing="3" Width="100%"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell VerticalAlign="Top">
                            <!--table 1 start here-->
                            <asp:Table ID="Table7" runat="server" CellPadding="3" CellSpacing="1" Width="100%"
                                BorderWidth="0">
                                <asp:TableRow>
                                    <asp:TableCell HorizontalAlign="Center">
                                        <br>
                                        <asp:Button ID="btnBack" Text="Back" Width="100px" runat="server"></asp:Button>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <!--table 1 ends here-->
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--Main Table ends here-->
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    &nbsp;
    <!--Validation starts here-->
    <asp:CompareValidator ID="cvtxtBankCode" runat="server" ErrorMessage="Bank Code should be in numeric format only."
        Operator="DataTypeCheck" ControlToValidate="txtBankCode" Type="Integer" Display="None"></asp:CompareValidator>
    <asp:RequiredFieldValidator ID="rfvtxtBankCode" runat="server" Display="None" ErrorMessage="Bank Code Cannot Be Blank"
        ControlToValidate="txtBankCode"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgetxtBankCode" runat="server" Display="None"
        ErrorMessage="Bank Code is invalid" EnableClientScript="false" ControlToValidate="txtBankCode"
        ValidationExpression="^\d{1,7}$"></asp:RegularExpressionValidator>
    <asp:RequiredFieldValidator ID="rfvtxtBankName" runat="server" Display="None" ErrorMessage="Bank Name Cannot Be Blank"
        ControlToValidate="txtBankName"></asp:RequiredFieldValidator>
    <asp:CompareValidator ID="cvBankCode" runat="server" ErrorMessage="Bank Code should not be same to Bank Name"
        Operator="NotEqual" ControlToValidate="txtBankCode" ControlToCompare="txtBankName"
        Type="String" Display="none"></asp:CompareValidator>
    <asp:RequiredFieldValidator ID="rfvtxtBankAdd1" runat="server" Display="None" ErrorMessage="Bank Address Cannot Be Blank"
        ControlToValidate="txtBankAdd1" ></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvddlBankState" runat="Server" ControlToValidate="ddlBankState"
        ErrorMessage="Please select a state" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvtxtBankPostCode" runat="server" Display="None"
        ErrorMessage="Postcode Cannot Be Blank" ControlToValidate="txtBankPostCode"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgetxtBankPostCode" runat="server" Display="None"
        ErrorMessage="Postcode must be 5 to 7 Numeric Characters" ControlToValidate="txtBankPostCode"
        ValidationExpression="^\d{5,7}$"></asp:RegularExpressionValidator>
    <asp:RequiredFieldValidator ID="rfvtxtBankPhone" runat="server" Display="None" ErrorMessage="Phone Number Cannot Be Blank"
        ControlToValidate="txtBankPhone"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgetxtBankPhone" runat="server" Display="None"
        ErrorMessage="Phone number is invalid" ControlToValidate="txtBankPhone" ValidationExpression="^\d{1,15}$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgetxtFax" runat="server" Display="None" ErrorMessage="Fax number is invalid"
        ControlToValidate="txtBankFax" ValidationExpression="^\d{1,15}$"></asp:RegularExpressionValidator>
    <asp:RequiredFieldValidator ID="rfvtxtBankContPerson" runat="server" Display="None"
        ErrorMessage="Bank Contact Person Cannot Be Blank" ControlToValidate="txtBankContPerson"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgetxtBankContPerson" runat="server" ControlToValidate="txtBankContPerson"
        ErrorMessage="Bank Contact person name should contain characters only" Display="None"
        ValidationExpression="^[a-zA-Z \s]{1,20}$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgetxtBankAdd1" runat="server" ControlToValidate="txtBankAdd1"
        Display="None" ErrorMessage="Address has exceeded 100 characters.  [Enter] Keys represents 2 Characters."
        ValidationExpression="^[a-zA-Z0123456789,./:# \s]{1,100}$"></asp:RegularExpressionValidator><!--Validation ends here--><!--table submit ends here-->
    <asp:RegularExpressionValidator ID="rgetxtWebAddress" runat="server" ErrorMessage="Invalid Website Address"
        ControlToValidate="txtBankURL" Display="None" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?"></asp:RegularExpressionValidator>
</asp:Content>
