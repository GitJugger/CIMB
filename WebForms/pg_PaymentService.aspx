<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.Pg_PaymentService" CodeFile="pg_PaymentService.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
        <script type="text/javascript" src="../include/common.js"></script>
			<!--title and error msg table starts here-->
			<input type="hidden" id="hidBankID" runat="server" />
			<input type="hidden" id="hidPaySerID" runat="server" />
			<input type="hidden" id="hidFtpFunction" runat="server" />
			<input type="hidden" id="hidStatus" runat="server" />
			<input type="hidden" id="hidIsMultipleBank" runat="server" />
			<input type="hidden" id="hidStatutory" runat="server" />
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
                <tr>
                    <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
                </tr>
                <tr>
                    <td><asp:Label Runat="server" CssClass="MSG" ID="lblErrorMessage"></asp:Label></td>
                </tr>
            </table>
			<!--title and error msg table ends here-->
			<!--table create starts here-->
			<asp:Table ID="tblCreate" Runat="server" Width="100%" cellpadding="8" CellSpacing="0">
				<asp:TableRow>
					<asp:TableCell>
						<!--main table starts here-->
						<asp:table id="Table1" CellSpacing="0" CellPadding="2" Width="100%" Runat="server" BorderWidth="0">
							<asp:TableRow>
								<asp:TableCell VerticalAlign="Top">
									<!--tblerror starts here-->
									<asp:Table ID="tblError" CellSpacing="2" CellPadding="1" Width="100%" Runat="server" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell Width="100%">
												<asp:Label Runat="server" CssClass="MSG" ID="Label20"></asp:Label>
												<asp:Label Runat="server" CssClass="PANELHEADING" ID="lblWarning"></asp:Label>
												<!-- Validation Summmary Start Here -->
												<asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" Runat="server" ShowSummary="false" ShowMessageBox="true"
													CssClass="MSG" HeaderText="Please Incorporate the below Validations:"></asp:ValidationSummary>
												<!-- Validation Summmary End Here -->
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--tblerror ends here-->
									<!--BCC starts here-->
									<asp:Table Width="550" ID="tblInput" Runat="server" CellPadding="2" CellSpacing="0" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell Width="50%">
												<asp:Label ID="lblPaySrvCode" Runat="server" Text="Payment Service Code" CssClass="LABEL"></asp:Label>
												<font class="MSG">*</font>
											</asp:TableCell>
											<asp:TableCell Width="70%">
												<asp:TextBox ID="txtPaySrvCode" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="2"
													Width="200" TabIndex="1"></asp:TextBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblPaySrvDesc" Runat="server" Text="Payment Service Description" CssClass="LABEL"></asp:Label>
												<font class="MSG">*</font>
											</asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtPaySrvDesc" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="100"
													Width="200" TabIndex="1"></asp:TextBox>
                                                <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPaySrvDesc" Display="None"
                                            ErrorMessage="Payment serv Description Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblPayTypeDesc" Runat="server" Text="Statutory Number(s)" CssClass="LABEL" ></asp:Label>
												<font class="MSG">*</font>
											</asp:TableCell>
											<asp:TableCell>
												<asp:DropDownList ID="ddlStatutoryNumber" runat="server" Width="200"></asp:DropDownList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trMultipleBank" runat="server">
											<asp:TableCell>
												<asp:Label ID="lblMultipleBank" Runat="server" Text="Is Multiple Bank" CssClass="LABEL"></asp:Label>
												<font class="MSG">*</font>
											</asp:TableCell>
											<asp:TableCell>
												<asp:RadioButtonList ID="rbIsMultipleBank" runat="server" RepeatDirection="Horizontal" >
			                                        <asp:ListItem Selected="true" Text="No" Value="False"></asp:ListItem>									
			                                        <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
												</asp:RadioButtonList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblStatus" Runat="server" Text="Status" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:RadioButtonList ID="radStatus" Runat="server" CssClass="LABEL" RepeatDirection="Horizontal" TabIndex="1">
												</asp:RadioButtonList>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--BCC ends here-->
								</asp:TableCell>
							</asp:TableRow>
						</asp:table>
						<!--main table end here--><br/>
						<!--Button Table starts Here-->
						<asp:table id="tblButton" CellSpacing="1" CellPadding="3" Width="584px" Runat="server" BorderWidth="0">
							<asp:TableRow>
								<asp:TableCell>
									<asp:Button ID="btnSubmit" Text="Save" Width="100px" Runat="server" TabIndex=2></asp:Button><asp:Button ID="btnUpdate" Text="Update" Width="100px" Runat="server" TabIndex=2></asp:Button><asp:Button ID="btnConfirm" Text="Confirm" Width="100px" Runat="server" TabIndex="2"></asp:Button><asp:Button ID="btnBackToView" Text="Back" Width="100px" Runat="server" Visible="false" TabIndex="2"></asp:Button><input type="reset" value="Clear" runat="Server" class="BUTTON" id="btnReset" name="Reset1" tabindex="2" />
								</asp:TableCell>
							</asp:TableRow>
						</asp:table>
						<!--Button Table ends Here-->
						
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<!--table create ends here-->
			<!--table submit starts here-->
			<asp:Table ID="tblSubmit" Runat="server" Width="100%" cellpadding="8" CellSpacing="0">
				<asp:TableRow>
					<asp:TableCell>
						<!--title and error message starts here-->
						<asp:Table Runat="server" ID="Table5" CellPadding="1" CellSpacing="2" Width="100%">
							<asp:TableRow>
								<asp:TableCell Width="100%" HorizontalAlign="Center">
									<asp:Label ID="lblSubmitTitle" Text="Payment Window Code Creation" CssClass="FORMHEAD" Runat="server"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="100%">&nbsp;</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell HorizontalAlign="Center">
									<asp:Label ID="lblMessage" Runat="server" CssClass="MSG"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<!--title and error message ends here-->
						<!--Main Table starts here-->
						<asp:Table ID="Table6" Runat="server" CellPadding="0" CellSpacing="2" Width="100%" BorderWidth="0">
							<asp:TableRow>
								<asp:TableCell VerticalAlign="Top">
									<!--table 1 start here-->
									<asp:Table ID="Table7" Runat="server" CellPadding="3" CellSpacing="1" Width="100%" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell HorizontalAlign="Center">
												<br>
												<asp:Button ID="btnBack" Text="Back" Width="100px" Runat="server"  TabIndex=2 ></asp:Button>
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
			<asp:requiredfieldvalidator id="rfvtxtPaySrvCode" Runat="server" Display="None" ErrorMessage="Payment Service Code Cannot Be Blank" ControlToValidate="txtPaySrvCode"></asp:requiredfieldvalidator>
			<asp:requiredfieldvalidator id="rfvtxtPaySrvDesc" Runat="server" Display="None" ErrorMessage="Payment Service Description Cannot Be Blank" ControlToValidate="txtPaySrvDesc"></asp:requiredfieldvalidator>
			<asp:RegularExpressionValidator ID="rgetxtPaySrvCode" runat="server" ControlToValidate="txtPaySrvCode" Display="None" ErrorMessage="Payment Service Code Accepts Alpha Numeric Only" ValidationExpression="^([a-zA-Z0-9]{1,2})$"></asp:RegularExpressionValidator><!--Validation ends here--><!--table submit ends here-->
	
</asp:Content>