<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ViewPassword" CodeFile="PG_ViewPassword.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

      <script type="text/javascript" src="../include/common.js"></script>
		<script language="JavaScript">
		function fncShow()
		{
			window.location.href = "PG_ViewPassword.aspx";
		}
		</script>
		
			<table cellpadding="5" cellspacing="0" width="100%" border="0">
          <tr>
              <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">User Information - View/Search User</asp:Label></td>
          </tr>
      </table>
							<table border="0" cellpadding="8" cellspacing="0" width="100%">
								<tr valign ="top" >
									<td>
										<!-- Form Table Starts Here -->
										<table border="0" cellpadding="2" cellspacing="0" width="100%">
											<tr>
												<td width="20%"><asp:Label Runat="Server" ID="lblOption" Text="Search Option" CssClass="LABEL"></asp:Label></td>
												<td width="80%"><asp:DropDownList ID="cmbOption" CssClass="MEDIUMTEXT" Runat="Server">
												<asp:ListItem Value=""></asp:ListItem>
												<asp:ListItem Value="USER LOGIN">User Id</asp:ListItem>
												<asp:ListItem Value="ORG ID">Organization Id</asp:ListItem>
												<asp:ListItem Value="ORG NAME">Organization Name</asp:ListItem>														
												</asp:DropDownList></td>
												<asp:RequiredFieldValidator ID="rfvOption" Runat="server" ControlToValidate="cmbOption" ErrorMessage="Search Option"
													Display="None"></asp:RequiredFieldValidator>
											</tr>
											<tr>
												<td><asp:Label Runat="Server" ID="lblCriteria" Text="Search Criteria" CssClass="LABEL"></asp:Label></td>
												<td><asp:DropDownList ID="cmbCriteria" CssClass="MEDIUMTEXT" Runat="Server">
												<asp:ListItem Value=""></asp:ListItem>
												<asp:ListItem Value="STARTS WITH">Starts With</asp:ListItem>
												<asp:ListItem Value="CONTAINS">Contains</asp:ListItem>
												<asp:ListItem Value="EXACT MATCH">Exact Match</asp:ListItem>
												</asp:DropDownList></td>
												<asp:RequiredFieldValidator ID="rfvCriteria" Runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Search Criteria"
													Display="None"></asp:RequiredFieldValidator>
											</tr>
											<tr>
												<td><asp:Label Runat="Server" ID="lblKeyword" Text="Search Keyword" CssClass="LABEL"></asp:Label></td>
												<td><asp:TextBox ID="txtKeyword" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="20"></asp:TextBox></td>
												<asp:RequiredFieldValidator ID="rfvKeyword" Runat="server" ControlToValidate="txtKeyword" ErrorMessage="Search Keyword"
													Display="None"></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator
                                            ID="revkeyword" runat="server" ControlToValidate="txtKeyword" Display="None"
                                            ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</tr>
											<tr>
												<td colspan="2">
													<asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
													<asp:Button Runat="Server" ID="btnClear" Text="Clear" CssClass="BUTTON" CausesValidation="false" ></asp:Button>
												</td>
											</tr>
										</table>													
										
										
									</td>
								</tr>
								
								<tr>
									<td width="100%" colspan="2">	
																			
									<asp:Label ID="lblMsg" Runat="server" CssClass="MSG"></asp:Label>
									<asp:panel runat="server" ID="pnlGrid">	
										 <asp:DataGrid CssClass="Grid" ID="dgUser" Runat="Server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
											PageSize="15"  BorderWidth="0" PagerStyle-Mode="NumericPages"
											HeaderStyle-CssClass="GridHeaderStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" ItemStyle-CssClass="GridItemStyle" Width="100%" HeaderStyle-HorizontalAlign="left" OnPageIndexChanged="prPageChange">
											<Columns>														
												<asp:BoundColumn datafield="OrgId" HeaderText="Organization Id" HeaderStyle-Width="75" ></asp:BoundColumn>														
												<asp:BoundColumn DataField="UserLogin" HeaderText="User Id" ></asp:BoundColumn>														
												<asp:BoundColumn DataField="UserName" HeaderText="User Name" ></asp:BoundColumn>
												<asp:BoundColumn DataField="User_Flag" HeaderText="User Type" ></asp:BoundColumn>
												<asp:BoundColumn DataField="UserStatus" HeaderText="User Status" ></asp:BoundColumn>
												<asp:TemplateColumn HeaderText="Modify">
													<ItemTemplate>
														<a href="PG_CreateRole.aspx?Id=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OrgId")%>&Mode=BU">Modify</a>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Modify">
													<ItemTemplate>
														<a href="PG_CreateRole.aspx?Id=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OrgId")%>&Mode=BU">View</a>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Reset" >
													<ItemTemplate>
														<a href="PG_ResetPassword.aspx?Id=<%#GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>&OName=<%#DataBinder.Eval(Container.DataItem,"OrgName")%>&Mode=PASS">Password</a>
														<%--<a href="PG_ResetPassword.aspx?Id=<%#DataBinder.Eval(Container.DataItem, "UserId")%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OrgId")%>&ULogin=<%#DataBinder.Eval(Container.DataItem,"UserLogin")%>&OName=<%#DataBinder.Eval(Container.DataItem,"OrgName")%>&UStatus=<%#DataBinder.Eval(Container.DataItem,"UserStatus")%>&UFlag=<%#DataBinder.Eval(Container.DataItem,"UFlag")%>&PLOCK=<%#DataBinder.Eval(Container.DataItem,"PLOCK")%>&Mode=PASS">Password</a>--%>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Reset">
													<ItemTemplate>
														<a href="PG_ResetPassword.aspx?Id=<%#GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>&OName=<%#DataBinder.Eval(Container.DataItem,"OrgName")%>&Mode=AUTH">Validation Code</a>
														<%--<a href="PG_ResetPassword.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"UserId")%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OrgId")%>&ULogin=<%#DataBinder.Eval(Container.DataItem,"UserLogin")%>&OName=<%#DataBinder.Eval(Container.DataItem,"OrgName")%>&UStatus=<%#DataBinder.Eval(Container.DataItem,"UserStatus")%>&UFlag=<%#DataBinder.Eval(Container.DataItem,"UFlag")%>&ALOCK=<%#DataBinder.Eval(Container.DataItem, "ALOCK")%>&Mode=AUTH">Validation Code</a>--%>
													</ItemTemplate>
												</asp:TemplateColumn>
												<asp:TemplateColumn HeaderText="Modify">
													<ItemTemplate>
														<a href="PG_CreateRole.aspx?UFlag=<%#DataBinder.Eval(Container.DataItem,"UFlag")%>&OrgId=<%#DataBinder.Eval(Container.DataItem,"OrgId")%>&UStatus=<%#DataBinder.Eval(Container.DataItem,"UserStatus")%>&Mode=New">Create</a>
													</ItemTemplate>
												</asp:TemplateColumn>
											</Columns>
										</asp:DataGrid>
									</asp:panel>
									</td>
								</tr>
								<tr>
									<td width="100%" colspan="2">
										<input type="button" runat="server" value="Show All" class="BUTTON" onclick="fncShow();"/>
									</td>
								</tr>
							</table>
							<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
											
</asp:Content>