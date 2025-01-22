<%@ Page Language="vb" Inherits="MaxPayroll.PG_Inbox" AutoEventWireup="false" CodeFile="PG_Inbox.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
	<script type="text/javascript" src="../include/common.js"></script>

  <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
  </table>
	<!--Main Table Starts Here -->
	<table border="0" cellpadding="2" cellspacing="5" style="width: 100%">
	<tr>
		<td width="100%"></td></tr>
		<tr><td width="100%"><asp:Label ID="Label1" CssClass="MSG" Runat="Server">Your Last Successful Sign On:</asp:Label>&nbsp;<asp:Label ID="lblSignOn" CssClass="MSG" Runat="Server"></asp:Label></td></tr>
		<tr>
		<td width="100%">
		<!--Form Table Starts Here -->
		
		<table id="tblSearch" border="0" cellpadding="1" cellspacing="2" width="100%">
			<tr>
				<td>
					<asp:Table CellPadding="1" CellSpacing="2" Runat="Server" Width="55%">
						<asp:TableRow>
							<asp:TableCell Width="45%" HorizontalAlign="Left">
								<asp:Label Runat="Server" CssClass="LABEL" Text="Show within"></asp:Label>&nbsp;
								<asp:TextBox ID="txtDays" Runat="Server" Class="MINITEXT" MaxLength="2" Text="15"></asp:TextBox>
								&nbsp;<asp:Label Runat="Server" CssClass="LABEL" Text="Days"></asp:Label>
							</asp:TableCell>
							<asp:TableCell Width="10%" HorizontalAlign="Left">
								<asp:Button ID="btnShow" CssClass="BUTTON" Runat="Server" Text="Display"></asp:Button> 
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
				</td>
			</tr>
			</table >
			<asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
               <img alt="" src="../Include/images/ProgressCircle4.gif" /> NOW LOADING...
               <br />
               <br />
            </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="upnl_dgUser" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
               <ContentTemplate>
			<table border="0" cellpadding="1" cellspacing="2" width="100%">
		
			      <tr>
			         <td>
			         	<asp:panel id="pnlGrid" CssClass="GridDiv" runat="server"> 
                  <asp:DataGrid Runat="Server" ID="dgUser" AllowSorting="false" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True"  AutoGenerateColumns="false" OnPageIndexChanged="Page_Change" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
                     <Columns>
                        <asp:TemplateColumn ItemStyle-Width="3%" HeaderText="">
                          <ItemTemplate><asp:CheckBox Runat="server" ID="myCheckbox"></asp:CheckBox><input type="hidden" id="hId" name="hId" runat="Server" value='<%# DataBinder.Eval(Container.DataItem,"MSGID")%>' /></ItemTemplate>							
                        </asp:TemplateColumn> 
                        <asp:TemplateColumn ItemStyle-Width="5%" HeaderText="Read" SortExpression="MREAD">
                          <ItemTemplate>
                             <%# ChangeImage(DataBinder.Eval(Container.DataItem,"MREAD")) %>
                          </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn SortExpression="MFROM" DataField="MFROM"  HeaderText="From" HeaderStyle-Width="10%"></asp:BoundColumn>
                        <asp:TemplateColumn HeaderText="Subject" HeaderStyle-Width="50%">
                          <ItemTemplate>
                             <%--<a href="PG_Mail.aspx?From=<%# DataBinder.Eval(Container.DataItem, "MFROM")%>&Subject=<%# DataBinder.Eval(Container.DataItem, "MSUB")%>&Date=<%# DataBinder.Eval(Container.DataItem, "MDATE")%>&Id=<%#GetEncrypterString(DataBinder.Eval(Container.DataItem, "MSGID"))%>"><%# DataBinder.Eval(Container.DataItem, "MSUB")%></a>--%>
                          <a href="PG_Mail.aspx?Id=<%#GetEncrypterString(DataBinder.Eval(Container.DataItem, "MSGID"))%>"><%# DataBinder.Eval(Container.DataItem,"MSUB")%></a>
                              </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:BoundColumn  SortExpression="MDATE" DataField="MDATE" HeaderText="Date" HeaderStyle-Width="15%"></asp:BoundColumn>
                     </Columns> 
                  </asp:DataGrid> 
                 
                 
                  </asp:panel>
			         </td>
			      </tr>
			      <tr><td><asp:Label ID="lblMessage" CssClass="MSG" Runat="Server"></asp:Label></td></tr>
			      <tr>
				      <td align="center">
					      <asp:Button ID="btnCheckAll" Runat="server" Text="CheckAll" CssClass="BUTTON" CausesValidation="False"></asp:Button>  
					      <asp:Button ID="btnUnCheck" Runat="server" Text="UnCheckAll" CssClass="BUTTON" CausesValidation="False"></asp:Button>  
					      <asp:Button ID="btnDelete" Runat="server" Text="Delete" CssClass="BUTTON" CausesValidation="False"></asp:Button>    
				      </td>
			      </tr>
		      </table>
		       </ContentTemplate>
                   
					       </asp:UpdatePanel>
       &nbsp;
		      </td></tr>
		      </table>
		<!--Form Table Ends Here -->
		
		<!--Main Table Ends Here --> 	
		
		<!-- Validation Starts here -->
		<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
		<asp:RequiredFieldValidator ID="rfvDays" Runat="server" ControlToValidate="txtDays" ErrorMessage="No of Days cannot be blank" Display="None"></asp:RequiredFieldValidator>
		<asp:RangeValidator ID="rgvDays" Runat="Server" ControlToValidate="txtDays" MinimumValue="1" MaximumValue="60" Type="Integer" Display="None" ErrorMessage="No of Days should be between 1 - 60"></asp:RangeValidator>
		<!-- Validation Ends here -->
		
</asp:Content>
