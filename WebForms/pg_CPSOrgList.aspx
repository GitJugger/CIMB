<%@ Page Language="VB" MasterPageFile="~/WebForms/mp_Master.master" AutoEventWireup="false" CodeFile="pg_CPSOrgList.aspx.vb" Inherits="MaxPayroll.WebForms_pg_CPSOrgList" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" Runat="Server">

<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

		 <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
	
		<script type="text/javascript" language="JavaScript" src="PG_Calendar.js"></script>
		
			<!--Main Table Starts Here -->
			<table border="0" cellpadding="5" cellspacing="0" width="100%">
				<tr>
					<td id="cHeader" width="100%"><asp:Label Runat="Server" CssClass="FORMHEAD" Text="Charges Organizations List" ID="lblHeading"></asp:Label></td>
				</tr>
		</table>
			
						<!--Form Table Starts Here -->
		<table border="0" cellpadding="8" cellspacing="0" width="100%">
				<tr>
					<td width="100%" colspan="2"><asp:Label ID="lblMessage" Runat="server" CssClass="MSG" />
					<asp:panel CssClass="GridDivNoScroll" ID="pnlGrid" runat="server">
						
						<asp:DataGrid Runat="Server" ID="dgOrganisation" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
							PagerStyle-HorizontalAlign="Center"  OnPageIndexChanged="prPageChange"
							GridLines="none"  HeaderStyle-CssClass="GridHeaderStyle"
							ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" width="100%"  HeaderStyle-HorizontalAlign="left"
							AutoGenerateColumns="False">
						    <Columns>
							    <asp:TemplateColumn HeaderText="Organization Code" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left">
	                                <ItemTemplate>
	                                   <a href="pg_CPSEditCharges.aspx?OrgID=<%#DataBinder.Eval(Container.DataItem,("ORGID"))%>">
	                                            <%#DataBinder.Eval(Container.DataItem, "ORGID")%></a>
	                                </ItemTemplate>
			                    </asp:TemplateColumn>

							    <asp:BoundColumn ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="left" DataField="NAME" HeaderText="Organization Name"></asp:BoundColumn>
										
							</Columns>
						</asp:DataGrid>
					</asp:panel>
					</td>
					</tr>
				
				<tr>
					<td colspan="2">
						<input id="Button2" type="button" runat="server" value="Show All" class="BUTTON" onclick="fncShow();" />
					</td>
				</tr>
			</table>
			<!-- Main Table Ends Here-->

</asp:Content>

