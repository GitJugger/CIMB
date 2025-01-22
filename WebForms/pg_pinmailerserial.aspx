<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_PinMailerSerial" CodeFile="pg_PinMailerSerial.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
        <script type="text/javascript" src="../include/common.js"></script>

		
	 <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Pin Mailer - Serial Number Maintenence</asp:Label></td>
      </tr>
      <tr>
			<td id="cErrMsg"><asp:label id="lblMessage" CssClass="MSG" Runat="server"></asp:label></td>
			</tr>
     </table>
		<table id="TblMain" borderColor="black" width="100%" border="0">
			<!-- Start of Table for Label Head and Message Label -->
			<tr>
				<td>
					<TABLE id="tblGrid"  width="100%" border="0">
						<tr>
							<td>
								<asp:label id="Label14" CssClass="BLABEL" Runat="server">Existing 
								Serial Range Nos. Information</asp:label></td>
						</tr>
						<tr>
							<td>
							    <asp:panel ID="pnlGrid" runat="server" CssClass = "GridDivNoScroll">
								<asp:datagrid CssClass="Grid" id="dgSerial" runat="server"  PagerStyle-HorizontalAlign="Center"
									HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" width="100%"
									HeaderStyle-HorizontalAlign="left" AutoGenerateColumns="False"  OnItemDataBound="dgSerial_ItemDataBound"
									OnitemCommand="dgSerial_Skip" OnDeleteCommand="dgSerial_Delete" GridLines="None" DataKeyField="TheKey">
									
									
									<Columns>
										<asp:BoundColumn Visible="False" DataField="TheKey" HeaderText="TheKey">
                                            <HeaderStyle  Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" />
											
										</asp:BoundColumn>
										<asp:BoundColumn DataField="StartNo" HeaderText="Start">
											
										</asp:BoundColumn>
										<asp:BoundColumn DataField="EndNo" HeaderText="End">
											
										</asp:BoundColumn>
										<asp:BoundColumn DataField="UseNext" HeaderText="Next Serial No.">
											
										</asp:BoundColumn>
										<asp:BoundColumn DataField="State" HeaderText="State">
											
										</asp:BoundColumn>
										<asp:ButtonColumn Text="SKIP ONE" HeaderText="SKIP ONE" CommandName="SKIP">
										
										</asp:ButtonColumn>
										<asp:ButtonColumn Text="DELETE" HeaderText="DELETE" CommandName="DELETE">
											
										</asp:ButtonColumn>
									</Columns>
									<PagerStyle HorizontalAlign="Center"></PagerStyle>
                                    <AlternatingItemStyle CssClass="GridAltItemStyle" />
                                    <ItemStyle CssClass="GridItemStyle" />
                                    <HeaderStyle  CssClass="GridHeaderStyle" HorizontalAlign="Left" />
								</asp:datagrid>
								</asp:panel>
							</td>
						</tr>
						<!--End of Data Grid Table --></TABLE>
				</td>
			</tr>
			<tr>
				<td height="10">
				</td>
			</tr>
			<tr>
				<td>
					<TABLE id="ADDNEW">
						<tr>
							<td>
								<asp:label id="Label12" CssClass="BLABEL" Runat="server">Enter 
								Serial No Range </asp:label></td>
						</tr>
						<tr>
							<td style="WIDTH: 20%"><asp:label id="Label1" CssClass="LABEL" Runat="Server" Width="95px">Start Serial No.</asp:label>&nbsp; 
								&nbsp; &nbsp;
								<asp:label id="Label2" CssClass="LABEL" Runat="Server" Width="193px">End Serial No.</asp:label></td>
						</tr>
						<tr>
							<td width="20%"><asp:textbox id="txtInsrtStart" CssClass="SMALLTEXT" Runat="Server" Width="100px"></asp:textbox>&nbsp;&nbsp;&nbsp;
								<asp:textbox id="txtInsrtEndNo" CssClass="SMALLTEXT" Runat="Server" Width="100px"></asp:textbox>&nbsp;&nbsp;&nbsp;
								<asp:button id="btnInsert" CssClass="BUTTON" Runat="Server" Text="Insert"></asp:button></td>
						</tr>
					</TABLE>
				</td>
			</tr>
			<tr>
				<td>
					<asp:table id="tblAuthoCode" runat="server" Width="100%" Visible="False">
						<asp:TableRow>
							<asp:TableCell Width="100%" ColumnSpan="3">
								<asp:label id="lblAMessg" runat="server" Width="100%" CssClass="MSG"></asp:label>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell width="20%">
								<asp:label id="Label3" runat="server" CssClass="MSG"> Enter Validation Code </asp:label>
							</asp:TableCell>
							<asp:TableCell width="70%" HorizontalAlign="left">
								<asp:TextBox id="TxtAuthorization" runat="server" textmode="Password" CssClass="MEDIUMTEXT"></asp:TextBox>
							</asp:TableCell>
						</asp:TableRow>
						<asp:TableRow>
							<asp:TableCell width="10%"></asp:TableCell>
							<asp:TableCell HorizontalAlign="Left">
								<asp:button id="btnConfirm" runat="server" CssClass="BUTTON" Text="Confirm" TabIndex="1"></asp:button>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
								<asp:button id="btnCancel" CssClass="BUTTON" Runat="server" Text="Back" TabIndex="2"></asp:button>
							</asp:TableCell>
						</asp:TableRow>
					</asp:table>
				</td>
			</tr>
			<tr>
				<td>
					<asp:ValidationSummary ID="vsPinSerial" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations,"
						ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
					<asp:RequiredFieldValidator ID="rfvFrom" Runat="Server" ControlToValidate="txtInsrtStart" Display="None" ErrorMessage="Start From No cannot be blank."></asp:RequiredFieldValidator>
					<asp:RequiredFieldValidator ID="rfvTo" Runat="Server" ControlToValidate="txtInsrtEndNo" Display="None" ErrorMessage="End To No cannot be blank."></asp:RequiredFieldValidator>
					<asp:RegularExpressionValidator ID="revInsertStart" Runat="Server" ControlToValidate="txtInsrtStart" ValidationExpression="\d{1,9}"
						ErrorMessage="Start Serial must be numeric value" Display="None"></asp:RegularExpressionValidator>
					<asp:RegularExpressionValidator ID="revInsrtEndNo" Runat="Server" ControlToValidate="txtInsrtEndNo" ValidationExpression="\d{1,9}"
						ErrorMessage="End Serial must be numeric value" Display="None"></asp:RegularExpressionValidator>
                    <%--<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="TxtAuthorization" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>--%>
                    
				</td>
			</tr>
		</table>

</asp:Content>