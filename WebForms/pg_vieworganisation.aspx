<%@ Page Language="vb" Inherits="MaxPayroll.PG_ViewOrganisation" AutoEventWireup="false" CodeFile="PG_ViewOrganisation.aspx.vb"     MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    

		<script type="text/javascript" language="JavaScript">
		function fncNew()
		{
			window.location.href = "PG_Organisation.aspx";
		}
		function fncShow()
		{
			var strRequest;
			strRequest = document.forms[0].ctl00$cphContent$hReq.value;
			window.location.href = "PG_ViewOrganisation.aspx?Req=" + strRequest;
		}
		</script>

      <script type="text/javascript" src="../include/common.js"></script>
		
			<!--Main Table Starts Here -->
			<input type="hidden" id="hReq" name="hReq" runat="server" />
			
				
				<table cellpadding="5" cellspacing="0" width="100%" border="0">
                    <tr>
                        <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
                    </tr>
                </table>
				<table border="0" cellpadding="8" cellspacing="0" width="100%">
				<tr>
					<td>
						<!--Form Table Starts Here -->
						<table border="0" cellpadding="2" cellspacing="0" width="100%">
							<tr>
								<td style="width:20%" class="LABEL">&nbsp;Search Option</td>
								<td style="width:80%">&nbsp;<asp:DropDownList ID="cmbOption" Runat="server" CssClass="MEDIUMTEXT">
								<asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="ID">Organization Id</asp:ListItem>
								<asp:ListItem Value="NAME">Organization Name</asp:ListItem>
								</asp:DropDownList></td>
							</tr>
							<tr>
								<td class="LABEL">&nbsp;Search Criteria</td>
								<td>&nbsp;<asp:DropDownList ID="cmbCriteria" Runat="server" CssClass="MEDIUMTEXT">
								<asp:ListItem Value=""></asp:ListItem>
								<asp:ListItem Value="STARTS WITH">Starts With</asp:ListItem>
								<asp:ListItem Value="CONTAINS">Contains</asp:ListItem>
								<asp:ListItem Value="EXACT MATCH">Exact Match</asp:ListItem>
								</asp:DropDownList></td>
							</tr>
							<tr>
								<td class="LABEL">&nbsp;Search Keyword</td>
								<td>&nbsp;<asp:TextBox ID="txtKeyword" Runat="Server" CssClass="MEDIUMTEXT" MaxLength="50"></asp:TextBox>
                                    <asp:RegularExpressionValidator
                                            ID="revkeyword" runat="server" ControlToValidate="txtKeyword" Display="None"
                                            ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
								</td>
							</tr>
						</table>
				<!--Form Table Ends Here -->
				        </td>
				    </tr>
				<tr>
					<td><asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
						<input type="button" value="Clear" runat="Server" class="BUTTON" onclick="fncShow();" /></td>
				</tr>
				<tr>
					<td>
						<asp:Label ID="lblMessage" Runat="server" CssClass="MSG" />
						<asp:panel ID="pnlGrid" CssClass ="GridDiv" runat = "server">
						<asp:DataGrid Runat="Server" ID="dgOrganisation" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center" BorderWidth="0" OnPageIndexChanged="prPageChange"
							GridLines="none"  HeaderStyle-CssClass="GridHeaderStyle"
							AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" HeaderStyle-HorizontalAlign="Left"
							AutoGenerateColumns="False" CssClass ="Grid" >
							<Columns>
							    
								<asp:BoundColumn DataField="ORGID" HeaderText="Organization&nbsp;Id"  HeaderStyle-Width="12%"></asp:BoundColumn>
								<asp:BoundColumn DataField="NAME" HeaderText="Organization&nbsp;Name"></asp:BoundColumn>
								<asp:BoundColumn DataField="PHONE" HeaderText="Phone"></asp:BoundColumn>
								<asp:BoundColumn DataField="REGISTERED" HeaderText="Created Date" ></asp:BoundColumn>
								<asp:BoundColumn DataField="STATUS" HeaderText="Status" ></asp:BoundColumn>
		                        <asp:TemplateColumn HeaderText="Select" Visible="false">
		                            <ItemTemplate>
		                                <asp:HyperLink ID="hlkModOrg" Text="Modify" runat="server"></asp:HyperLink>
		                            </ItemTemplate>
		                        </asp:TemplateColumn>												                    <asp:TemplateColumn HeaderText="Select" Visible="false">
		                            <ItemTemplate>
		                                <asp:HyperLink ID="hlkViewOrg" Text="View" runat="server"></asp:HyperLink>
		                            </ItemTemplate>
		                        </asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_BankAccount3.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">Add/Delete</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_ListFile.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">Add/Modify</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_ListFile.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">View</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=E">Add/Delete</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=S">Add/Delete</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=L">Add/Delete</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_BankAccount3.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">View</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=E">View</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=S">View</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=L">View</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="pg_MerchantChargeDefinition.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&PageMode=<%#MaxPayroll.mdConstant.enmPageMode.NewMode%>">Add payment</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="pg_MerchantChargeDefinition.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&PageMode=<%#MaxPayroll.mdConstant.enmPageMode.EditMode%>">Modify</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select" >
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=Z">Add/Delete</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_ServiceAccount.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&Ser=Z">View</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_BankCodeMapping.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">Add/Modify</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="pg_CreateRole.aspx?OrgId=<%# DataBinder.Eval(Container.DataItem, "ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">Create User</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_ViewOrganizationRoles.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">User Maintenance</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="pg_MandatesDetails.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>">Create Mandates</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="pg_MandateList.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&PageMode=<%=MaxPayroll.mdConstant.enmPageMode.NewMode%>">Mandates Maintenance</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="pg_CPSEditCharges.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&PageMode=<%=MaxPayroll.mdConstant.enmPageMode.NewMode%>">Edit Charges</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								<asp:TemplateColumn Visible="false" HeaderText="Select">
									<ItemTemplate>
										<a href="PG_CPSFileSubmission.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"ID")%>&Name=<%#DataBinder.Eval(Container.DataItem,"NAME")%>&Status=<%#DataBinder.Eval(Container.DataItem,"STATUS")%>&PageMode=<%=MaxPayroll.mdConstant.enmPageMode.NewMode%>">View Files</a>
									</ItemTemplate>
								</asp:TemplateColumn>
								</Columns>
								
						</asp:DataGrid></asp:panel>
					</td>
				</tr>
				<tr>
					<td>
						<!--<input type="button" runat="Server" value="Create New" class="BUTTON" id="btnNew" visible="false" onclick="fncNew();">  -->
						<input type="button" runat="server" value="Show All" class="BUTTON" onclick="fncShow();" />
					</td>
				</tr>
			</table>
			<!-- Main Table Ends Here-->
			<asp:RequiredFieldValidator ID="rfvOption" Runat="server" ControlToValidate="cmbOption" ErrorMessage="Search Option" Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvCriteria" Runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Search Criteria" Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvKeyword" Runat="server" ControlToValidate="txtKeyword" ErrorMessage="Search Keyword" Display="None"></asp:RequiredFieldValidator>
			<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields," ID="vsViewOrganization" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>

</asp:Content>