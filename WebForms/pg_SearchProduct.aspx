<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_SearchProduct" CodeFile="pg_SearchProduct.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
   		<script type="text/javascript" src="../include/common.js"></script>
		
			<!--title and error msg table starts here-->
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"><%=MaxPayroll.pg_SearchProduct.sPageTitle%> Modification</asp:Label></td>
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
												<asp:Label ID="lblProductName" Runat="server" Text="Product Name" CssClass="LABEL"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="70%">
												<asp:TextBox ID="txtProductName" Runat="server" AutoPostBack="False" CssClass="SMALLTEXT" MaxLength="10" Width="200" TabIndex="1"></asp:TextBox>
                                                 <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtProductName" Display="None"
                                            ErrorMessage="Product Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell ColumnSpan="2">
											    <asp:Button ID="btnSearch" Runat="server" Text="Search" Width="100px"></asp:Button>&nbsp;&nbsp;<input type="button" onclick="window.location='pg_SearchProduct.aspx';" value="Clear" runat="server" class="BUTTON" id="Button1" />
										    </asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<!--table1 ends here-->
									<!--tblError starts here-->
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
						<asp:DataGrid ID="dgProduct" Runat="server" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center"  CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" HeaderStyle-HorizontalAlign="left" BorderWidth="0" AutoGenerateColumns="False" Width="100%">
							<Columns>
							    <asp:BoundColumn DataField="ProductName" HeaderText="Product Name"></asp:BoundColumn>				    
			                    <asp:BoundColumn DataField="Status" HeaderText="Status"></asp:BoundColumn>				    
			                    <asp:TemplateColumn HeaderText="Action">
			                        <ItemTemplate>
			                            <asp:HyperLink ID="hlLink" runat="server" Text="Modify"></asp:HyperLink>
			                        </ItemTemplate>
			                    </asp:TemplateColumn>
			                    <asp:BoundColumn DataField="ProductID" HeaderText="Product Name" Visible="false"></asp:BoundColumn>
							</Columns>
                            <PagerStyle HorizontalAlign="Center" Mode="NumericPages" />
						</asp:DataGrid>
						</asp:panel>
						<!--datagrid ends here-->
			        </td>
			    </tr>
			</table>
    	<asp:ValidationSummary DisplayMode="BulletList" ID="vsSearchProduct" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
</asp:Content>