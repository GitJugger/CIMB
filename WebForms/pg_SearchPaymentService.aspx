<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_SearchPaymentService" CodeFile="pg_SearchPaymentService.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   		<script type="text/javascript" src="../include/common.js"></script>
		
			<!--title and error msg table starts here-->
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"><%=MaxPayroll.pg_SearchPaymentService.sPageTitle%> Modification</asp:Label></td>
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
									<asp:Table Width="550" ID="Table2" Runat="server" CellPadding="2" CellSpacing="0" BorderWidth="0">
										<asp:TableRow>
											<asp:TableCell Width="30%">
												<asp:Label ID="lblPaymentSrvCode" Runat="server" Text="Payment Service Code" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="70%">
												<asp:TextBox ID="txtPaySrvCode" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="10"
													Width="200" TabIndex="1"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtPaySrvCode" ValidationExpression="^[\w\-\s]+$" ErrorMessage="PaySrvCode Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell>
												<asp:Label ID="lblPaySrvDesc" Runat="server" Text="Description" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtPaySrvDesc" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="60"
													Width="200" TabIndex="2"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtPaySrvDesc" ValidationExpression="^[\w\-\s]+$" ErrorMessage="PaySrvDesc Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan=2><br><br>
											<asp:Button ID="btnSearch" Runat="server" Text="Search" Width="100px"></asp:Button>&nbsp;&nbsp;<input type="button" onclick="window.location='pg_SearchPaymentService.aspx';" value="Clear" runat="server" class="BUTTON" id="Button1" />
										</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--table1 ends here-->
									<!--tblError starts here--><br>
									<br>
									<asp:Table Width="100%" ID="Table4" Runat="server" CellPadding="2" CellSpacing="0" BorderWidth="0">
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
			<table cellpadding="12" cellspacing="0" width="100%" border="0">
			    <tr>
			        <td>
			            <!--datagrid starts here-->
			            <asp:panel cssclass="GridDivNoScroll" id="pnlGrid" runat="server">
						<asp:DataGrid ID="dgPaySrvMaintenance" Runat="server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center"  CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"
							ItemStyle-CssClass="GridItemStyle"
						    HeaderStyle-HorizontalAlign="left" BorderWidth=0
							AutoGenerateColumns="False" Width="100%">
							<Columns>
							    <asp:BoundColumn DataField="PaySer_ID" Visible=false></asp:BoundColumn>
								<asp:BoundColumn DataField="PaySrvCode" HeaderText="Payment Service Code">
                                  
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="PaySer_Desc" HeaderText="Description">
                                    
                                </asp:BoundColumn>
								<asp:BoundColumn DataField="Status" HeaderText="Status">
                                   
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="hlLink" runat="server" Text="Edit"></asp:HyperLink>
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
    <asp:ValidationSummary DisplayMode="BulletList" ID="vsSearchPaymentService" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
</asp:Content>