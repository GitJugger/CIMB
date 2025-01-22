<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.pg_SearchReportServicesByOrganization" CodeFile="pg_SearchReportServicesByOrganization.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

		 <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
		<script type="text/javascript" language="JavaScript">
			function fncShow()
			{
				window.location.href = "pg_SearchReportServicesByOrganization.aspx";
			}
			function fncViewReport(OrgID)
			{
			    window.location.href="pg_ViewReportServicesByOrganization.aspx?Id=" + OrgID
			}
		</script>
		<script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
		
			<!--Main Table Starts Here -->
			<table border="0" cellpadding="5" cellspacing="0" width="100%">
				<tr>
					<td id="cHeader" width="100%"><asp:Label Runat="Server" CssClass="FORMHEAD" Text="View/Search Organization" ID="lblHeading"></asp:Label></td>
				</tr>
		</table>
			
						<!--Form Table Starts Here -->
		<table border="0" cellpadding="8" cellspacing="0" width="100%">
		
							<tr>
								<td width="30%" class="LABEL">&nbsp;Search Option</td>
								<td width="70%" align="left">&nbsp;<asp:DropDownList ID="cmbOption" Runat="server" CssClass="MEDIUMTEXT">
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
								</td>
							</tr>
				
				<tr>
					<td width="100%" colspan="2"><asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
					<input type="button" value="Clear" class="BUTTON" onclick="fncShow();" runat="server"></td>
				</tr>
			
				
				<tr>
					<td width="100%" colspan=2><asp:Label ID="lblMessage" Runat="server" CssClass="MSG" />
					<asp:panel CssClass="GridDivNoScroll" ID="pnlGrid" runat="server">
						
						<asp:DataGrid Runat="Server" ID="dgOrganisation" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center"  OnPageIndexChanged="prPageChange"
							GridLines="none"  HeaderStyle-CssClass="GridHeaderStyle"
							ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" width="100%"  HeaderStyle-HorizontalAlign="left"
							AutoGenerateColumns="False">
							<Columns>
		                        <asp:TemplateColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" HeaderText="Organization Id">
		                            <ItemTemplate>
		                                <a id="ancViewReport" runat=server><%# DataBinder.Eval(Container.DataItem, "ORGID")%></a>
		                            </ItemTemplate>
		                        </asp:TemplateColumn>						
								<asp:BoundColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" DataField="NAME" HeaderText="Organization Name"></asp:BoundColumn>
								<asp:BoundColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" DataField="PHONE" HeaderText="Phone"></asp:BoundColumn>
								<asp:BoundColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" DataField="REGISTERED" DataFormatString="{0:d}" HeaderText="Created Date"></asp:BoundColumn>
								<asp:BoundColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" DataField="STATUS" HeaderText="Status"></asp:BoundColumn>
							</Columns>
						</asp:DataGrid>
					</asp:panel>
					</td>
					</tr>
				
				<tr>
					<td colspan=2>
						<input type="button" runat="server" value="Show All" class="BUTTON" onclick="fncShow();">
					</td>
				</tr>
			</table>
			<!-- Main Table Ends Here-->
			<asp:RequiredFieldValidator ID="rfvOption" Runat="server" ControlToValidate="cmbOption" ErrorMessage="Search Option"
				Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvCriteria" Runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Search Criteria"
				Display="None"></asp:RequiredFieldValidator>
			<asp:RequiredFieldValidator ID="rfvKeyword" Runat="server" ControlToValidate="txtKeyword" ErrorMessage="Search Keyword"
				Display="None"></asp:RequiredFieldValidator>
     <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtKeyword" Display="None"
                                            ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
			<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields,"
				ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>

</asp:Content>
