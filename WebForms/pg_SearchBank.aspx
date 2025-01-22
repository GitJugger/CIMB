<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_SearchBank" CodeFile="pg_SearchBank.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
        <script type="text/javascript" src="../include/common.js"></script>
		
			<!--title and error msg table starts here-->
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Bank Code Modification</asp:Label></td>
      </tr>
  </table>

						<!--main table starts here-->
						<asp:Table id="Table1" CellSpacing="0" CellPadding="8" Width="100%" Runat="server" BorderWidth="0">
							<asp:TableRow>
								<asp:TableCell VerticalAlign="Top">
									<!--table1 starts here-->
									<asp:Table Width="100%" ID="Table2" Runat="server" CellPadding="2" CellSpacing="0" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell Width="10%">
												<asp:Label ID="Label1" Runat="server" Text="Bank Code" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell >
												<asp:TextBox ID="txtBankCode" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="10"
													Width="200" TabIndex="1"></asp:TextBox>
                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtBankCode" ValidationExpression="^[\w\-\s]+$" ErrorMessage="BankCode Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="Label2" Runat="server" Text="Bank Name" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtBankName" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="60"
													Width="200" TabIndex="2"></asp:TextBox>
                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtBankName" ValidationExpression="^[\w\-\s]+$" ErrorMessage="BankName Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan="2"><br/><br/>
											<asp:Button ID="btnSearch" Runat="server" Text="Search" Width="100px"></asp:Button>&nbsp;&nbsp;<input type="button" onclick="window.location='pg_SearchBank.aspx';" value="Clear" runat="server" class="BUTTON" id="Reset1" />
										</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--table1 ends here-->
									<!--tblError starts here--><br/>
									<br/>
									<asp:Table Width="100%" ID="Table4" Runat="server" CellPadding="2" CellSpacing="0" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label Runat="server" CssClass="MSG" ID="lblErrorMessage"></asp:Label>
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--tblError ends here-->
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
							 <asp:TableCell ColumnSpan="2">
							    <table cellpadding="2" cellspacing="0" width="100%" border="0">
			    <tr>
			        <td>
			            <!--datagrid starts here-->
			            <asp:panel id="pnlGrid" CssClass="GridDivNoScroll" runat="server"> 
						<asp:DataGrid ID="dgBankCodeMaintenance" Runat="server" CssClass="Grid" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center" BorderWidth="0px" BorderColor="Black" CellPadding="3" HeaderStyle-CssClass="GridHeaderStyle"  ItemStyle-CssClass= "GridItemStyle" 
							AlternatingItemStyle-CssClass="GridAltItemStyle"
							Font-Names="Verdana" Font-Size="8pt" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="left"
							AutoGenerateColumns="False" Width="100%">
							<Columns>
								<asp:BoundColumn DataField="BankCode" HeaderText="Bank Code">
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="BankName" HeaderText="Bank Name">
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="Status" HeaderText="Status">
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Action">
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
							 </asp:TableCell>
							</asp:TableRow>
						</asp:Table>
    <asp:ValidationSummary DisplayMode="BulletList" ID="vsSearchBank" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>


</asp:Content>			
