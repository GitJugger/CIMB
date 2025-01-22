<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ApprMatrix" CodeFile="PG_ApprMatrix.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <script type="text/javascript" src="../include/common.js"></script>
    
    <script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			window.location.href = "PG_ApprMatrix.aspx?Mode=" + document.forms[0].ctl00$cphContent$hidMode.value ;
			//window.history.back();
		}
		function fncClear()
		{
			window.location.href = "PG_ApprMatrix.aspx?Mode=Edit" ;
		}
    </script>
    <script type="text/javascript" src="../include/common.js"></script>
 
    <input type="hidden" id="hidMode" runat="server" />
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
      <tr>
         <td><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
  </table>

		<!-- Datagrid Table Starts Here -->
		<asp:Table ID="tblMainForm" CellPadding="8" CellSpacing="0" Runat="Server" Width="100%">
		<asp:TableRow>
			<asp:TableCell Width="100%" ColumnSpan="2">
			 <asp:Panel ID="pnlGrid" runat="server" CssClass="GridDiv">
				<asp:DataGrid ID="dgApprMatrix" Runat="Server" AllowPaging="True" AllowSorting="False" 
				AutoGenerateColumns="False" PagerStyle-Mode="NumericPages" OnPageIndexChanged="prPageChange" 
				CellPadding="3" CellSpacing="0" PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" 
				Font-Names="Verdana" Font-Size="8pt" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" 
				ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  
				ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
				<Columns>
					<asp:TemplateColumn HeaderText="" HeaderStyle-Width="3%">
						<ItemTemplate>
							<asp:CheckBox ID="chkSelect" Runat="Server"></asp:CheckBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Datafield="APRID" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn Datafield="FRID" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn Datafield="MDID" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:BoundColumn Datafield="FNAME" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>
					<asp:BoundColumn Datafield="RDATE" HeaderText="Received" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>
					<asp:BoundColumn Datafield="ASUBJECT" HeaderText="From" HeaderStyle-Width="10%" ReadOnly="True" Visible="False"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Action" HeaderStyle-Width="15%">
						<ItemTemplate>
						  <a href="PG_ViewApprMatrix.aspx?Id=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "MDID"))%>&Module=<%#DataBinder.Eval(Container.DataItem,"ASUBJECT")%>&Mode=Edit&Appr=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "APRID"))%>"><%#DataBinder.Eval(Container.DataItem,"ASUBJECT")%></a>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Datafield="MDSC" HeaderText="Description" HeaderStyle-Width="10%" ReadOnly="True"></asp:BoundColumn>					
					<asp:TemplateColumn HeaderText="Action" HeaderStyle-Width="15%">
						<ItemTemplate>
							<asp:RadioButtonList ID="rdStatus" Runat="Server" CssClass="LABEL" RepeatDirection="Horizontal">
								<asp:ListItem Value="2">Accept</asp:ListItem>
								<asp:ListItem Value="3">Reject</asp:ListItem>
							</asp:RadioButtonList>
							<input type="hidden" id="hStatus" name="hStatus" runat="Server"/>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:TemplateColumn HeaderText="Remarks" HeaderStyle-Width="10%">
						<ItemTemplate>
							<asp:TextBox ID="txtRemarks" Runat="Server" CssClass="MEDIUMTEXT" TextMode="MultiLine" Rows="3" MaxLength="255"></asp:TextBox>
						<asp:RegularExpressionValidator
                                            ID="revkeyword" runat="server" ControlToValidate="txtRemarks" Display="None"
                                            ErrorMessage="remarks Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                        </ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
				</asp:DataGrid>
				</asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
				<asp:Button ID="btnCheckAll" Runat="server" Text="Select All" CssClass="BUTTON"></asp:Button>&nbsp;  
				<asp:Button ID="btnUnCheck" Runat="server" Text="Unselect All" CssClass="BUTTON"></asp:Button>&nbsp;
				<asp:Button ID="btnAccept" Runat="server" Text="Accept All" CssClass="BUTTON"></asp:Button>&nbsp;
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trAuthCode">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Validation Code"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="70%">
				<asp:TextBox ID="txtAuthCode" CssClass="BIGTEXT" Runat="Server" TextMode="Password" MaxLength="24"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trSubmit">
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnConfirm" CssClass="BUTTON" Runat="Server" Text="Submit"></asp:Button>&nbsp;
				<input type="button" id="btnReset" name="btnReset" runat="Server" value="Clear" class="BUTTON" onclick="fncClear();"/>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trConfirm">
			<asp:TableCell Width="90%" ColumnSpan="2">
				<asp:Button ID="btnSubmit" Runat="Server" CssClass="BUTTON" Text="Confirm"></asp:Button>&nbsp;
				<input type="button" id="btnBack" runat="Server" value="Back" onclick="fncBack();" class="BUTTON"/>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		<!-- Datagrid Table Ends Here -->
		
		<!-- View Datagrid Table Starts Here -->
		<asp:Table ID="tblView" Runat="Server" CellPadding="12" CellSpacing="0" Width="90%">
		<asp:TableRow>
			<asp:TableCell Width="90%">
			 <asp:Panel ID="pnlGridView" runat="server" CssClass="GridDivNoScroll">
				<asp:DataGrid Runat="Server" ID="dgViewMatrix" AllowSorting="False" AllowPaging="True" PageSize="15" 
				PagerStyle-Mode="NumericPages"	PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="3" 
				OnPageIndexChanged="prVPageChange" CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" width="100%"  
				AutoGenerateColumns="false" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" 
				ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  
				ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
						
				<Columns>
					<asp:BoundColumn  SortExpression="APPR_DATE" DataField="APPR_DATE" HeaderText="Request Date" HeaderStyle-Width="10%" ></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Action" HeaderStyle-Width="15%">
						<ItemTemplate>
							<a id="lnkAction" runat="server"><%#DataBinder.Eval(Container.DataItem,"APPR_SUB")%></a>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:BoundColumn Datafield="MDSC" HeaderText="Description" HeaderStyle-Width="20%" ReadOnly="True"></asp:BoundColumn>					
					<asp:BoundColumn  SortExpression="APPR_CDATE" DataField="APPR_CDATE" HeaderText="Action Date" HeaderStyle-Width="10%" ></asp:BoundColumn>
					<asp:TemplateColumn ItemStyle-Width="10%" HeaderText="Remarks">
						<ItemTemplate>
							<asp:TextBox ID="txtVRemarks" Runat="Server" CssClass="MEDIUMTEXT" TextMode="MultiLine" ReadOnly="True" Text='<%# DataBinder.Eval(Container.DataItem,"APPR_REMARKS")%>'></asp:TextBox>
						</ItemTemplate>
					</asp:TemplateColumn>
					<asp:boundcolumn DataField="MDID" Visible="false"></asp:boundcolumn>
					<asp:boundcolumn DataField="APPR_SUB" Visible="false"></asp:boundcolumn>
					<asp:boundcolumn DataField="APPR_ID" Visible="false"></asp:boundcolumn>
				</Columns> 
					</asp:DataGrid> </asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		<!-- View Datagrid Table Starts Here -->
		
		<!-- View Datagrid Table Starts Here -->
		<asp:Table ID="tblViewPend" Runat="Server" CellPadding="12" CellSpacing="0" Width="90%">
		<asp:TableRow>
			<asp:TableCell Width="90%">
			  <asp:Panel ID="pnlGridPending" runat="server" CssClass="GridDivNoScroll">
				<asp:DataGrid Runat="Server" ID="dgViewPend" AllowSorting="False" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages"
					PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="3" OnPageIndexChanged="prPPageChange"	CellSpacing="0" 
					Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True"  AutoGenerateColumns="false" 
					CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" 
					AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
						<Columns>
							<asp:BoundColumn  SortExpression="APPR_DATE" DataField="APPR_DATE" HeaderText="Request Date Time" 
							HeaderStyle-Width="15%" ></asp:BoundColumn>
							<asp:TemplateColumn HeaderText="Action" HeaderStyle-Width="15%">
								<ItemTemplate>
									<a href="PG_ViewApprMatrix.aspx?Id=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "MDID"))%>&Module=<%#DataBinder.Eval(Container.DataItem, "APPR_SUB")%>&Mode=View&Appr=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "APPR_ID"))%>"><%#DataBinder.Eval(Container.DataItem,"APPR_SUB")%></a>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:BoundColumn Datafield="MDSC" HeaderText="Description" HeaderStyle-Width="20%"  ItemStyle-HorizontalAlign="Left" ReadOnly="True"></asp:BoundColumn>					
						</Columns> 
					</asp:DataGrid> </asp:Panel>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		<!-- View Datagrid Table Starts Here -->
     <asp:ValidationSummary DisplayMode="BulletList" ID="lblValidator" runat="server"
                                ShowSummary="false" ShowMessageBox="true" CssClass="MSG" HeaderText="Please Incorporate the below Validations:">
                            </asp:ValidationSummary>
     <asp:RequiredFieldValidator ID="rfvAuthCode" Runat="Server" ErrorMessage="Validation Code Cannot be blank" Display="None" ControlToValidate="txtAuthCode"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Validation Code must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
</asp:Content>
