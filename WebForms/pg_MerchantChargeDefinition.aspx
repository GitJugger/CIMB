<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_MerchantChargeDefinition" CodeFile="pg_MerchantChargeDefinition.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
    
        <script type="text/javascript" src="../include/common.js"></script>
        
		
		<script>
		function fncBack()
		{
			window.history.back();
		}
		function fncBackMod()
		{
			window.history.go(-2);
		}	
		</script>
		
			<!-- Main Table Start Here -->
			<table id=tblMerchCharge runat=server cellpadding="5" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Organization Charge Definition</asp:Label></td>
                    </tr>
                    <tr>
                        <td id="Td1"><asp:label id="lblErrorMsg" CssClass="MSG" Runat="Server"></asp:label></td>
                    </tr>
                   
                </table>
						<!-- Title and message table end here -->
						<asp:table id="Table8" Width="100%" Runat="server" BorderWidth="0" CellPadding="8" CellSpacing="0">
							<asp:TableRow>
								<asp:TableCell>
									<!-- Main Table 1 Start Here -->
									<asp:table id="Table2" CellSpacing="1" CellPadding="3" Width="100%" Runat="server" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell VerticalAlign="top">
												<!--tblAlert starts here-->
												<asp:Table id="tblAlert" BorderWidth="0" Runat="server" CellSpacing="0" CellPadding="2" Width="100%">
													<asp:TableRow>
														<asp:TableCell Width="100%">
															<asp:Label Runat="server" CssClass="MSG" ID="lblAlert"></asp:Label>
															<asp:Label Runat="server" CssClass="PANELHEADING" ID="lblWarning"></asp:Label>
															<!-- Validation Summmary Start Here -->
															<asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" Runat="server" ShowSummary="true" ShowMessageBox="False"
																CssClass="MSG" HeaderText="There is one or more fields that requires your attention:" EnableViewState="True"
																EnableClientScript="True"></asp:ValidationSummary>
															<!-- Validation Summmary End Here -->
														</asp:TableCell>
													</asp:TableRow>
												</asp:Table>
												<!--tblAlert ends here-->
												<!-- Table 1 Start Here -->
												<asp:table id="Table1" Width="550" CellPadding="2" CellSpacing="0" Runat="server" BorderWidth="0">
													<asp:TableRow>
														<asp:TableCell Width="30%">
															<asp:Label Runat="server" Text="Payment Service:" CssClass="LABEL" ID="Label45"></asp:Label>
															<asp:label id="Label46" Text="*" CssClass="MSG" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
															<asp:DropDownList id="ddlModelType" runat="server" Width="144px" 
																AutoPostBack="True"></asp:DropDownList>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="50%">
															<asp:Label Runat="server" Text="Organization ID:" CssClass="LABEL" ID="Label1"></asp:Label>
															<asp:label id="Label43" Text="*" CssClass="MSG" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
															<asp:TextBox Runat="Server" ID="txtSellerID" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="10"
																TabIndex="2" Width="200" ReadOnly="True"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="30%">
															<asp:Label Runat="server" Text="Organizatio Name:" CssClass="LABEL" ID="Label7"></asp:Label>
															<asp:label id="Label21" Text="*" CssClass="MSG" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
															<asp:TextBox Runat="Server" ID="txtMerchName" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="60"
																TabIndex="2" Width="200" ReadOnly="True"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="30%">
															<asp:Label Runat="server" Text="Charge Settlement Account:" CssClass="LABEL"
																ID="Label8"></asp:Label>
															<asp:label id="Label23" Text="*" CssClass="MSG" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
														<asp:DropDownList id="ddlChargeSettleAcc" runat="server" Width="144px"></asp:DropDownList>&nbsp;&nbsp;
													</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="30%">
															<asp:Label Runat="server" Text="Charge Settlement Account (Alternate):" CssClass="LABEL" ID="Label44"></asp:Label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
															<asp:TextBox Runat="Server" ID="txtChargeSettleAcc" AutoPostBack="False" CssClass="SMALLTEXT"
																MaxLength="14" TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="30%">
															<asp:Label Runat="server" Text="Billing Frequency:" CssClass="LABEL" ID="Label4"></asp:Label>
															<asp:label id="Label22" Text="*" CssClass="MSG" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
															<asp:DropDownList id="ddlBillFrequency" runat="server" Width="144px">
																<asp:ListItem Value="D">Daily</asp:ListItem>
																<asp:ListItem Value="W">Weekly</asp:ListItem>
														        <asp:ListItem Value="M">Monthly</asp:ListItem>
															</asp:DropDownList>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell Width="30%">
															<asp:Label Runat="server" Text="Billing Type:" CssClass="LABEL" ID="Label30"></asp:Label>
															<asp:label id="Label24" Text="*" CssClass="MSG" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell Width="70%">
															<asp:RadioButtonList ID="radMerchBill" runat="server" AutoPostBack="True" CssClass="LABEL" RepeatDirection="Horizontal">
																<asp:ListItem Value="A" text="Auto Billing" Selected=True ></asp:ListItem>
																<asp:ListItem Value="M" text="Manual Billing"></asp:ListItem>
															</asp:RadioButtonList>
														</asp:TableCell>
													</asp:TableRow>
												</asp:table>
												<!-- Table 1 End Here -->
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan="2">
												<!-- Main Table 2 Starts Here -->
												<asp:table id="Table4" Runat="server" BorderWidth="0" CellSpacing="1" CellPadding="3" Width="550">
													<asp:TableRow>
														<asp:TableCell>
															<asp:label id="Label38" Text="Organization Charges" CssClass="BLABEL" Runat="Server"></asp:label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow ID="trFixed" Runat="server" >
														<asp:TableCell Width="200">
														<asp:radiobutton id="radFixedRate" runat="server" Text="Fixed Rate" CssClass="LABEL" GroupName="radMerchCharge"
																AutoPostBack="True" OnCheckedChanged="prcDisplayChargeTypeInfo"></asp:radiobutton>&nbsp;
													</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtFixedRate" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="8"
																TabIndex="2" Width="200"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow ID="trTier" Runat="server" >
														<asp:TableCell>
													<asp:radiobutton id="radTierRate" runat="server" Text="Tier Rate" CssClass="LABEL" GroupName="radMerchCharge"
																AutoPostBack="True" OnCheckedChanged="prcDisplayChargeTypeInfo"></asp:radiobutton>&nbsp;
													</asp:TableCell>
													</asp:TableRow>
												</asp:table>
												<asp:table id="tblTier" Runat="server" BorderWidth="0" CellSpacing="0" CellPadding="2" Width="600"
													>
													<asp:TableRow>
														<asp:TableCell>
														&nbsp;
													</asp:TableCell>
														<asp:TableCell>
															<asp:label id="Label9" Text="Minimum Transaction Value." CssClass="LABEL" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell>
															<asp:label id="Label10" Text="Maximum Transaction Value." CssClass="LABEL" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell>
															<asp:label id="Label11" Text="Rate in RM (Per Transaction)" CssClass="LABEL" Runat="Server"></asp:label>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell>
														&nbsp;
														&nbsp;
														<asp:label id="Label15" Text="Tier 1" CssClass="LABEL" Runat="Server"></asp:label>
													</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMinTier1" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMaxTier1" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtRateTier1" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell>
														&nbsp;
														&nbsp;
														<asp:label id="Label12" Text="Tier 2" CssClass="LABEL" Runat="Server"></asp:label>
													</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMinTier2" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMaxTier2" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtRateTier2" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow >
														<asp:TableCell>
														&nbsp;
														&nbsp;
														<asp:label id="Label13" Text="Tier 3" CssClass="LABEL" Runat="Server"></asp:label>
													</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMinTier3" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMaxTier3" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtRateTier3" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="150"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
												</asp:table>
												<asp:table id="tblPercent" Runat="server" BorderWidth="0" CellSpacing="0" CellPadding="2" Width="550">
													<asp:TableRow ID="trPercent" Runat="server" >
														<asp:TableCell ColumnSpan="3">
														<asp:radiobutton id="radPercentRate" runat="server" Text="Percentage Rate" CssClass="LABEL" GroupName="radMerchCharge"
																AutoPostBack="True" OnCheckedChanged="prcDisplayChargeTypeInfo"></asp:radiobutton>&nbsp;
													</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell></asp:TableCell>
														<asp:TableCell>
															<asp:label id="Label14" Text="Percentage Rate:" CssClass="LABEL" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtPercentRate" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="8"
																TabIndex="2" Width="200"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell>
														&nbsp;
													</asp:TableCell>
														<asp:TableCell>
															<asp:label id="Label16" Text="Minimum  Amount:" CssClass="LABEL" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMinAmt" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="200"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
													<asp:TableRow>
														<asp:TableCell>
														&nbsp;
													</asp:TableCell>
														<asp:TableCell>
															<asp:label id="Label17" Text="Maximum Amount:" CssClass="LABEL" Runat="Server"></asp:label>
														</asp:TableCell>
														<asp:TableCell>
															<asp:TextBox Runat="Server" ID="txtMaxAmt" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="13"
																TabIndex="2" Width="200"></asp:TextBox>
														</asp:TableCell>
													</asp:TableRow>
												</asp:table>
												<!-- Main Table 2 End Here -->
											</asp:TableCell>
										</asp:TableRow>
									</asp:table>
									<!-- Main Table 1 End Here -->
								</asp:TableCell>
							</asp:TableRow>
						</asp:table>
						<!-- Button table Start Here -->
						<asp:table id="Table5" CellSpacing="0" CellPadding="12" Width="550" Runat="server" BorderWidth="0">
							<asp:TableRow ID="trUpdate" Visible="true">
								<asp:TableCell>
									<asp:Button ID="btnSubmit" Text="Submit" Runat="server" CssClass="BUTTON"></asp:Button>
		                        							
								       &nbsp;
									<input type="reset" value="Clear" runat="Server" class="BUTTON" ID="btnClear">
		                    			&nbsp;				
								    <asp:Button ID="btnBack" Text="Back" Runat="server" CssClass="BUTTON"></asp:Button>	
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow ID="trBack" Visible="false">
								<asp:TableCell ColumnSpan="3" Width="100%">
									<input runat="server" ID="Button3" type="button" onclick=" fncBack();" value="Back" class="BUTTON"
										NAME="Button3">
									&nbsp;								
								</asp:TableCell>
							</asp:TableRow>
						</asp:table>
						<!-- Button table End Here -->
					</asp:TableCell>
				</asp:TableRow>
			</asp:table>
			<!-- Main Table End Here -->
			<!-- Required field validation starts here -->
			<!-- Acknowledgement Table Start Here --><asp:table cellspacing="0" CellPadding="2" id="tblAck" Runat="server" Width="100%" Visible="False">
				<asp:TableRow>
					<asp:TableCell Width="100%" HorizontalAlign="Center">
						<asp:label id="Label27" Text="Organization Charges Definition" CssClass="FORMHEAD" Runat="Server"></asp:label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="100%" HorizontalAlign="Center">
						<br>
						<br>
						<asp:label id="lblMessage" CssClass="MSG" Runat="Server"></asp:label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="100%" ColumnSpan="2" HorizontalAlign="Center">
						<br>
						<br>
						<br>
				<asp:Button ID="btnNew" Text="Create New Organization Charges" CssClass="BUTTON" Runat="Server" Width=300></asp:Button>&nbsp;									
				</asp:TableCell>
				</asp:TableRow>
			</asp:table>
			<!-- Acknowledgement Table End Here --><asp:requiredfieldvalidator id="rfvtxtFR" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Fixed  rate can't be empty"
				ControlToValidate="txtFixedRate"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvTxtPR" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization percent rate can't be empty"
				ControlToValidate="txtPercentRate"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtMinAmtMerch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Minimum Amount can't be empty"
				ControlToValidate="txtMinAmt"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtMaxAmtMerch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Maximum Amount can't be empty"
				ControlToValidate="txtMaxAmt"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtTierMin1Merch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Minimum Tier Rate 1 can't be empty"
				ControlToValidate="txtMinTier1"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtTierMin2Merch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Minimum Tier Rate 2 can't be empty"
				ControlToValidate="txtMinTier2"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtTierMax1Merch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Maximum Tier Rate 1 can't be empty"
				ControlToValidate="txtMaxTier1"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtTierMax2Merch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Maximum Tier Rate 2 can't be empty"
				ControlToValidate="txtMaxTier2"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtTierRate1Merch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Rate 1 can't be empty"
				ControlToValidate="txtRateTier1"></asp:requiredfieldvalidator><asp:requiredfieldvalidator id="rfvtxtTierRate2Merch" Runat="server" Enabled="False" Display="None" ErrorMessage="Organization Rate 2 can't be empty"
				ControlToValidate="txtRateTier2"></asp:requiredfieldvalidator>
				<asp:regularexpressionvalidator id="revtxtFR" Runat="server" Display="None" ErrorMessage="Fixed Rate allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" EnableViewState="True" EnableClientScript="True" Controltovalidate="txtFixedRate"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtPR" Runat="server" Display="None" ErrorMessage="Percent rate allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtPercentRate"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtPRMinAmt" Runat="server" Display="None" ErrorMessage="Minimum Rate allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtMinAmt"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtPRMaxAmt" Runat="server" Display="None" ErrorMessage="Maximum Rate allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtMaxAmt"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtTR1" Runat="server" Display="None" ErrorMessage="Rate of Tier Rate 1 allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtRateTier1"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtTR2" Runat="server" Display="None" ErrorMessage="Rate of Tier Rate 2 allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtRateTier2"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtTMin1" Runat="server" Display="None" ErrorMessage="Minimum Tier Rate 1 allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtMinTier1"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtTMin2" Runat="server" Display="None" ErrorMessage="Minimum Tier Rate 2 allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtMinTier2"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtTMax1" Runat="server" Display="None" ErrorMessage="Maximum Tier Rate 1 allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtMaxTier1"></asp:regularexpressionvalidator><asp:regularexpressionvalidator id="revtxtTMax2" Runat="server" Display="None" ErrorMessage="Maximum Tier Rate 2 allows decimal '99.99' only"
				ValidationExpression="\d*\w[.]\d{2}" Controltovalidate="txtMaxTier2"></asp:regularexpressionvalidator><input id="hChargeSettAcc" type="hidden" name="hChargeSettAcc" runat="server">
<INPUT id="hChargeType" type="hidden" name="hChargeType" runat="server">
				<asp:regularexpressionvalidator id="revSettleBankAccount" Runat="server" Display="None" ErrorMessage="Charge Settlement account number not valid"
				ControlToValidate="txtChargeSettleAcc" ValidationExpression="\d{14}"></asp:regularexpressionvalidator><asp:comparevalidator id="cvAlternateAcc" Runat="server" Display="None" ErrorMessage="Charge Settlement Account can not be same as Alternative Account or Its Invalid"
				ControlToValidate="txtChargeSettleAcc" Type="String" ControlToCompare="ddlChargeSettleAcc" Operator="NotEqual"></asp:comparevalidator>
			<asp:comparevalidator id="cvFixedRate" Runat="server" Display="None" ErrorMessage="Fixed rate can't be zero"
				ControlToValidate="txtFixedRate" Type="String" Operator="GreaterThan" ValueToCompare="0"></asp:comparevalidator><asp:comparevalidator id="cvPercentRate" Runat="server" Display="None" ErrorMessage="Percent rate can't be zero"
				ControlToValidate="txtPercentRate" Type="String" Operator="GreaterThan" ValueToCompare="0"></asp:comparevalidator><asp:comparevalidator id="cvPercentMin" Runat="server" Display="None" ErrorMessage="Minimum Amount of Percent rate can't be zero"
				ControlToValidate="txtMinAmt" Type="String" Operator="GreaterThan" ValueToCompare="0"></asp:comparevalidator>
			<asp:comparevalidator id="cvTierRate1" Runat="server" Display="None" ErrorMessage="Organization Charges Minimum Rate,Tier 1 can't be zero"
				ControlToValidate="txtMinTier1" Type="String" Operator="GreaterThan" ValueToCompare="0"></asp:comparevalidator>
			<asp:comparevalidator id="rvRate1" Runat="server" Display="None" ErrorMessage="Organization Charges Tier Rate 1,Rate per Transaction can't be zero"
				ControlToValidate="txtRateTier1" Type="String" Operator="GreaterThan" ValueToCompare="0"></asp:comparevalidator>
			<asp:comparevalidator id="cvRate2" Runat="server" Display="None" ErrorMessage="Organization Charges Rate Tier 2,Rate per Transaction can't be zero"
				ControlToValidate="txtRateTier2" Type="String" Operator="GreaterThan" ValueToCompare="0"></asp:comparevalidator>
			<asp:comparevalidator id="cvPMinimumAmt" Runat="server" Display="None" ErrorMessage="Minimum Amount of Organization Charges can not be greater or equal to Maximum Amount"
				ControlToValidate="txtMinAmt" Type="String" ControlToCompare="txtMaxAmt" Operator="LessThan"></asp:comparevalidator><asp:comparevalidator id="cvPMaxAmt" Runat="server" Display="None" ErrorMessage="Maximum Amount of Organization Charges can not be less than or equal to Minimum Amount"
				ControlToValidate="txtMaxAmt" Type="Double" ControlToCompare="txtMinAmt" Operator="GreaterThan"></asp:comparevalidator>
				<asp:comparevalidator id="cvMaxTier1" Runat="server" Display="None" ErrorMessage="Maximum Tier 1 value must be greater than or not equal to Minimum Tier 1 value"
				ControlToValidate="txtMaxTier1" Type="Double" ControlToCompare="txtMinTier1" Operator="GreaterThan"></asp:comparevalidator><asp:comparevalidator id="cvMinTier2" Runat="server" Display="None" ErrorMessage="Minimum Tier 2 value must be greater than or not equal to Maximum Tier 1 value"
				ControlToValidate="txtMinTier2" Type="Double" ControlToCompare="txtMaxTier1" Operator="GreaterThan"></asp:comparevalidator><asp:comparevalidator id="cvMaxTier2" Runat="server" Display="None" ErrorMessage="Maximum Tier 2 value must be greater than or not equal to Minimum Tier 2 value"
				ControlToValidate="txtMaxTier2" Type="Double" ControlToCompare="txtMinTier2" Operator="GreaterThan"></asp:comparevalidator><input type="hidden" id="hidCustChargeID" runat="server" /><input type="hidden" id="hidBillFrequency" runat=server /><input type="hidden" id="hidMerchBill" runat=server /><input type=hidden id="hidModelType" runat=server />

</asp:Content>