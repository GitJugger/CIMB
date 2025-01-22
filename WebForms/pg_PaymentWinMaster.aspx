<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_PaymentWinMaster"
    CodeFile="pg_PaymentWinMaster.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master"
    EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <!--title and error msg table starts here-->
    <input type="hidden" id="hidBankID" runat="server" />
    <input type="hidden" id="hidPayWinID" runat="server" />
    <input type="hidden" id="hidFtpFunction" runat="server" />
    <input type="hidden" id="hidStatus" runat="server" />
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">Payment Window Code Creation</asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="Label1"></asp:Label></td>
        </tr>
        <tr>
            <td id="cErrMsg">
                <asp:Label runat="server" CssClass="MSG" ID="lblErrorMessage"></asp:Label></td>
        </tr>
    </table>
    <!--	<asp:table id="tblMain" CellSpacing="2" CellPadding="1" Width="100%" Runat="server" BorderWidth="0">
				
				<asp:TableRow>
					<asp:TableCell Width="100%">
						<asp:Label ID="lblTitle" Text="Payment Window Code Creation" CssClass="FORMHEAD" Runat="server"></asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="100%"></asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell>
						
					</asp:TableCell>
				</asp:TableRow>
			</asp:table>
			title and error msg table ends here-->
    <!--table create starts here-->
    <asp:Table ID="tblCreate" CellSpacing="0" CellPadding="8" runat="server" Width="100%">
        <asp:TableRow>
            <asp:TableCell>
                <!--main table starts here-->
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
                <asp:Table Width="100%" ID="tblInput" runat="server" CellPadding="2" CellSpacing="0"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblPaymentWindowCode" runat="server" Text="Payment Window Code" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtPayWinCode" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="10" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblBank" runat="server" CssClass="LABEL">Bank<font class="MSG">*</font></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlBankID" Width="200" runat="server" TabIndex="1">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblFTPServerName" runat="server" Text="FTP Server Name" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFtpServerName" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="50" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblFTPServerIPAddress" runat="server" Text="FTP Server IP Address"
                                CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFtpIPAddress" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="20" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblFtpUploadDir" runat="server" Text="FTP Server Upload Directory"
                                CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFtpUploadDir" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="100" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblFtpDownloadDir" runat="server" Text="FTP Server Download Directory"
                                CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFtpDownloadDir" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="100" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblFtpUserID" runat="server" Text="FTP Server User ID" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFtpUserID" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="14" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trFtpPwdButton" Visible="false" runat="server">
                        <asp:TableCell>
                            <asp:Label ID="lblFtpPwdButton" runat="server" Text="FTP Server Password" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Button ID="btnChgPwd" Text="Change Password" runat="server" Width="200px" TabIndex="1" /><asp:Label
                                ID="lblPwdMsg" runat="server" Text="" CssClass="LABEL_ITALIC"></asp:Label>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="trFtpPwd" runat="server">
                        <asp:TableCell>
                            <asp:Label ID="lblFtpPwd" runat="server" Text="FTP Server Password" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtFtpPwd" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="40" TextMode="Password" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow runat="server" ID="trService" Visible="false">
                        <asp:TableCell>
                            <asp:Label ID="lblFtpFunction" runat="server" Text="FTP Function" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="ddlFtpFunction" runat="server" Width="144px" TabIndex="1">
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblWindowStartTime" runat="server" Text="Window Start Time" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtPayWinStartTime" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="4" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblWinDescription" runat="server" Text="Description" CssClass="LABEL"></asp:Label>
                            <font class="MSG">*</font>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtPayWinDesc" runat="server" AutoPostBack="False" CssClass="SMALLTEXT"
                                MaxLength="100" Width="200" TabIndex="1"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Label ID="lblStatus" runat="server" Text="Status" CssClass="LABEL"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:RadioButtonList ID="radStatus" runat="server" CssClass="LABEL" RepeatDirection="Horizontal"
                                TabIndex="1">
                            </asp:RadioButtonList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <!--BCC ends here-->
                <!--main table end here-->
                <br />
                <!--Button Table starts Here-->
                <asp:Table ID="tblButton" CellSpacing="0" CellPadding="3" Width="584px" runat="server"
                    BorderWidth="0">
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnSubmit" Text="Save" Width="100px" runat="server" TabIndex="2"></asp:Button>
                            <asp:Button ID="btnUpdate" Text="Update" Width="100px" runat="server" TabIndex="2"></asp:Button>
                            <asp:Button ID="btnConfirm" Text="Confirm" Width="100px" runat="server" TabIndex="2">
                            </asp:Button>
                            <asp:Button ID="btnBackToView" Text="Back" Width="100px" runat="server" Visible="false"
                                TabIndex="2" ValidationGroup="a"></asp:Button>
                            <input type="reset" value="Clear" runat="Server" class="BUTTON" id="btnReset" name="Reset1"
                                tabindex="2" />
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
                            <asp:Label ID="lblSubmitTitle" Text="Payment Window Code Creation" CssClass="FORMHEAD"
                                runat="server"></asp:Label>
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
                                        <asp:Button ID="btnBack" Text="Back" Width="100px" runat="server" TabIndex="2"></asp:Button>
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
    <!--Validation starts here-->
    <asp:RequiredFieldValidator ID="rfvtxtPayWinCode" runat="server" Display="None" ErrorMessage="Payment Window Code Cannot Be Blank"
        ControlToValidate="txtPayWinCode"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
            ID="rfvtxtFtpServerName" runat="server" Display="None" ErrorMessage="FTP Server Name Cannot Be Blank"
            ControlToValidate="txtFtpServerName"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                ID="rfvtxtFtpIPAddress" runat="server" Display="None" ErrorMessage="FTP Server IP Address Cannot Be Blank"
                ControlToValidate="txtFtpIPAddress"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                    ID="rfvtxtFtpUploadDir" runat="Server" ControlToValidate="txtFtpUploadDir" ErrorMessage="FTP Server Upload Directory Cannot Be Blank"
                    Display="None"></asp:RequiredFieldValidator><asp:RequiredFieldValidator ID="rfvtxtFtpDownloadDir"
                        runat="server" Display="None" ErrorMessage="FTP Server Download Directory Cannot Be Blank"
                        ControlToValidate="txtFtpDownloadDir"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                            ID="rfvtxtFtpUserID" runat="server" ControlToValidate="txtFtpUserID" Display="None"
                            ErrorMessage="FTP Server User ID Cannot Be Blank"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                ID="rfvtxtFtpPwd" runat="server" ControlToValidate="txtFtpPwd" Display="None"
                                ErrorMessage="FTP Server Password Cannot Be Blank"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                    ID="rfvtxtPayWinStartTime" runat="server" ControlToValidate="txtPayWinStartTime"
                                    Display="None" ErrorMessage="Window Start Time Cannot Be Blank"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                        ID="rfvtxtPayWinDesc" runat="server" ControlToValidate="txtPayWinDesc" Display="None"
                                        ErrorMessage="Description Cannot Be Blank"></asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                            ID="rgetxtPayWinCode" runat="server" ControlToValidate="txtPayWinCode" Display="None"
                                            ErrorMessage="Payment Window Code Accepts Alpha Numeric Only" ValidationExpression="^([a-zA-Z0-9]{1,10})$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgetxtFtpIpAddress" runat="server" ControlToValidate="txtFtpIpAddress"
        Display="None" ErrorMessage="FTP Server IP Address Accepts Alpha Numeric With [.], [:], [/] And [\] Only"
        ValidationExpression="^([a-zA-Z0-9\.\:\\\/]{1,20})$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgetxtFtpUploadDir" runat="server" ControlToValidate="txtFtpUploadDir"
        Display="None" ErrorMessage="FTP Server Upload Directory Accepts Alpha Numeric With [.], [:], [-], [/] And [\] Only"
        ValidationExpression="^([a-zA-Z0-9\.\:\\\/_]{1,100})$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator
            ID="rgetxtFtpDownloadDir" runat="server" ControlToValidate="txtFtpDownloadDir"
            Display="None" ErrorMessage="FTP Server Download Directory Accepts Alpha Numeric With [.], [:], [-], [/] And [\] Only"
            ValidationExpression="^([a-zA-Z0-9\.\:\\\/_]{1,100})$">

    </asp:RegularExpressionValidator><%--asp:RegularExpressionValidator
                        ID="rgetxtFtpUserID" runat="server" ControlToValidate="txtFtpUserID" Display="None"
                        ErrorMessage="FTP Server User ID Accepts Alphabets With [@] Only" ValidationExpression="^([a-zA-Z0-9\@]{1,14})$"></asp:RegularExpressionValidator>--%>
    <asp:RegularExpressionValidator ID="rgetxtPayWinStartTime" runat="server" ControlToValidate="txtPayWinStartTime"
        Display="None" ErrorMessage="Window Start Time Accepts Time Format In [HHMM] Only"
        ValidationExpression="^([0-1][0-9]|[2][0-3])([0-5][0-9])$"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPayWinDesc" Display="None"
                                            ErrorMessage="Payment Window Description Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
    
    <!--Validation ends here--><!--table submit ends here-->
</asp:Content>
