<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_SearchPaymentWin" CodeFile="pg_SearchPaymentWin.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <script type="text/javascript" src="../include/common.js"></script>
		
			<!--title and error msg table starts here-->
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"><%=MaxPayroll.pg_SearchPaymentWin.sPageTitle%> Modification</asp:Label></td>
      </tr>
  </table>
			
			<!--title and error msg table ends here-->
			<asp:Table ID="table6" Runat="server" Width="100%" cellpadding="8" CellSpacing="0">
				<asp:TableRow>
					<asp:TableCell VerticalAlign="Top">
						<!--main table starts here-->
						<asp:Table id="Table1" CellSpacing="0" CellPadding="2" Width="100%" Runat="server" BorderWidth="0">
							<asp:TableRow>
								<asp:TableCell VerticalAlign="Top">
									<!--table1 starts here-->
									<asp:Table Width="550" ID="Table2" Runat="server" CellPadding="1" CellSpacing="3" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell Width="30%">
												<asp:Label ID="lblPaymentWinCode" Runat="server" Text="Payment Window Code" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="70%">
												<asp:TextBox ID="txtPayWinCode" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="10"
													Width="200" TabIndex="1"></asp:TextBox><asp:RegularExpressionValidator
                                            ID="rgetxtPayWinCode" runat="server" ControlToValidate="txtPayWinCode" Display="None"
                                            ErrorMessage="Payment Window Code Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblPayWinDesc" Runat="server" Text="Description" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtPayWinDesc" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="60"
													Width="200" TabIndex="2"></asp:TextBox>
                                                <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtPayWinDesc" Display="None"
                                            ErrorMessage="Payment Window Description Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblBankID" Runat="server" Text="Bank" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:DropDownList ID="ddlBankID"	Width="200" runat="server"></asp:DropDownList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan="2"><br/><br/>
											<asp:Button ID="btnSearch" Runat="server" Text="Search" Width="100px"></asp:Button>&nbsp;&nbsp;<input type="button" onclick="window.location='pg_SearchBank.aspx';" value="Clear" runat="server" class="BUTTON" id="Button1" />
										</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--table1 ends here-->
									<!--tblError starts here--><br>
									<br>
									<asp:Table Width="100%" ID="Table4" Runat="server" CellPadding="1" CellSpacing="2" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label Runat="server" CssClass="MSG" ID="lblErrorMessage"></asp:Label>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--tblError ends here-->
								</asp:TableCell>
								<asp:TableCell VerticalAlign="Top" HorizontalAlign="Right" RowSpan="2">
									
								</asp:TableCell>
							</asp:TableRow>
						</asp:Table>
						<!--main table ends here-->
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<table cellpadding=12 cellspacing=0 width=100% border=0>
			    <tr>
			        <td>
			            <!--datagrid starts here-->
			            <asp:panel ID="pnlGrid" CssClass = "GridDivNoScroll" runat = "server">
						<asp:DataGrid ID="dgPayWinMaintenance" Runat="server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center" BorderWidth="0px" BorderColor="Black" CssClass = "grid" HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"
							ItemStyle-CssClass="GridItemStyle"
							 HeaderStyle-HorizontalAlign="left"
							AutoGenerateColumns="False" Width="100%">
							<Columns>
							    <asp:BoundColumn DataField="PayWinID" Visible=false></asp:BoundColumn>
								<asp:BoundColumn DataField="PayWinCode" HeaderText="Payment Window Code">
                                   
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="PayWinDesc" HeaderText="Description">
                                   
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="BankName" HeaderText="Bank">
                                    
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="Status" HeaderText="Status">
                                   
                                </asp:BoundColumn>
                                <asp:TemplateColumn  HeaderText="Action" >
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlLink" runat=server Text="Edit"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
                           
						</asp:DataGrid>
						</asp:panel>
						<!--datagrid ends here-->
			        </td>
			    </tr>
			</table>
			<asp:ValidationSummary DisplayMode="BulletList" ID="vsSearchPaymentWin" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
</asp:Content>