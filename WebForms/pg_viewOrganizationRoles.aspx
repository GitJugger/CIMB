<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_ViewOrganizationRoles" CodeFile="pg_viewOrganizationRoles.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>
    
    <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="Roles Information - View/Search Roles" CssClass="FORMHEAD" Runat="server" id="lblHeading"></asp:Label></td>
      </tr>
  </table>
	<!-- Main Table Starts Here -->
	<asp:Table ID="tblRoles" Runat="Server" Width="100%" CellPadding="8" CellSpacing="0">
	<asp:TableRow>
		<asp:TableCell Width="100%">
			<!-- Form Table Starts Here -->
			<asp:Table Width="100%" CellPadding="2" CellSpacing="0" Runat="Server">
				<asp:TableRow>
					<asp:TableCell Width="20%">
					<asp:Label Runat="Server" ID="lblOption" Text="Search Option" CssClass="LABEL"></asp:Label></asp:TableCell>
					<asp:TableCell Width="80%">
						<asp:DropDownList ID="cmbOption" CssClass="MEDIUMTEXT" Runat="Server">
							<asp:ListItem Value=""></asp:ListItem>
							<asp:ListItem Value="User Login">User Id</asp:ListItem>
							<asp:ListItem Value="User Name">User Name</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="20%">
							<asp:Label Runat="Server" ID="lblCriteria" Text="Search Criteria" CssClass="LABEL"></asp:Label></asp:TableCell>
					<asp:TableCell Width="80%">
						<asp:DropDownList ID="cmbCriteria" CssClass="MEDIUMTEXT" Runat="Server">
							<asp:ListItem Value=""></asp:ListItem>
							<asp:ListItem Value="Starts With">Starts With</asp:ListItem>							
							<asp:ListItem Value="Contains">Contains</asp:ListItem>
							<asp:ListItem Value="Exact Match">Exact Match</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="20%">
							<asp:Label Runat="Server" ID="lblKeyword" Text="Search Keyword" CssClass="LABEL"></asp:Label></asp:TableCell>
					<asp:TableCell Width="80%">
							<asp:TextBox ID="txtKeyword" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="20"></asp:TextBox>
                        <asp:RegularExpressionValidator
                                            ID="revkeyword" runat="server" ControlToValidate="txtKeyword" Display="None"
                                            ErrorMessage="Keyword Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="100%" ColumnSpan="2">
						<asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
						<asp:Button Runat="Server" ID="btnClear" Text="Clear" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
						
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<!-- Form Table Ends Here -->
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<asp:RequiredFieldValidator ID="rfvOption" Runat="server" ControlToValidate="cmbOption" ErrorMessage="Search Option" Display="None"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCriteria" Runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Search Criteria" Display="None"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvKeyword" Runat="server" ControlToValidate="txtKeyword" ErrorMessage="Search Keyword" Display="None"></asp:RequiredFieldValidator>
	<asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields," ID="frmValidator" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
	<!-- List Roles Starts Here -->
	<table cellpadding="8" cellspacing=0 width="100%"><tr><td>
	<asp:Table Width="100%" CellPadding="2" BorderWidth="0" CellSpacing="0" Runat="server" ID="tblRole">
		<asp:TableRow CssClass="GridHeaderStyle" BorderWidth="1">
			<asp:TableCell Width="20%" HorizontalAlign="left">
			<asp:Label Runat="Server" Text="User Type" CssClass="BLABEL"></asp:Label></asp:TableCell>
			<asp:TableCell Width="20%" HorizontalAlign="Center">
			<asp:Label Runat="Server" Text="Active" CssClass="BLABEL"></asp:Label></asp:TableCell>
			<asp:TableCell Width="20%" HorizontalAlign="Center">
			<asp:Label Runat="Server" Text="Inactive" CssClass="BLABEL"></asp:Label></asp:TableCell>
		  <asp:TableCell Width="20%" HorizontalAlign="Center">
			<asp:Label ID="lblDeleted" Runat="Server" Text="Deleted" CssClass="BLABEL"></asp:Label></asp:TableCell>	
		</asp:TableRow>
		<asp:TableRow BorderWidth="1">
			<asp:TableCell><asp:Label Runat="Server" Text="Uploader" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" ID="lblAUploader" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblCUploader"></asp:Label></asp:TableCell>
		<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblDUploader"></asp:Label></asp:TableCell>	
		</asp:TableRow>
		<asp:TableRow BorderWidth="1">
			<asp:TableCell ><asp:Label Runat="Server" Text="Reviewer" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" ID="lblAReviewer" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblCReviewer"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblDReviewer"></asp:Label></asp:TableCell>	
		</asp:TableRow>
		<asp:TableRow BorderWidth="1">
			<asp:TableCell ><asp:Label Runat="Server" Text="Approver" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" ID="lblAAuthor" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblCAuthor"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblDAuthor"></asp:Label></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow BorderWidth="1">
			<asp:TableCell ><asp:Label Runat="Server" Text="Interceptor" CssClass="LABEL" ID="Label1" NAME="Label1"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" ID="lblAInterceptor" CssClass="LABEL"></asp:Label></asp:TableCell>
			<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblCInterceptor"></asp:Label></asp:TableCell>
		<asp:TableCell  HorizontalAlign="Center">&nbsp;
			<asp:Label Runat="Server" CssClass="LABEL" ID="lblDInterceptor"></asp:Label></asp:TableCell>	
		</asp:TableRow>
	</asp:Table></td></tr></table>
	<!-- List Roles Ends Here -->
	<!-- Data Grid Starts Here -->
	<asp:Table Runat="server" Width="100%" CellPadding="12" CellSpacing="0" id="Table2">
	<asp:TableRow Visible ="false">
		<asp:TableCell Width="100%">			
			<asp:Button Runat="Server" ID="btnNew" CausesValidation="False" Text="Create User Role" CssClass="BUTTON"></asp:Button>&nbsp;
			<asp:Button Runat="Server" ID="btnShow" CausesValidation="False" Text="Show All" CssClass="BUTTON"></asp:Button>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="trGrid">
		<asp:TableCell Width="100%">
		  <asp:Panel ID="pnlGrid" runat="server" CssClass="GridDivNoScroll">
			<asp:DataGrid ID="dgUsers" Runat="Server" AllowPaging="True" AllowSorting="false" AutoGenerateColumns="False" CellPadding="3" CellSpacing="0" 
			PageSize="15" GridLines="none" BorderWidth="1" Font-Names="Verdana" Font-Size="8pt" PagerStyle-Mode="NumericPages" OnPageIndexChanged="prPageChange"  CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
				<Columns>
            <asp:BoundColumn datafield="UserLogin" HeaderText="User Id" HeaderStyle-Width="5%"></asp:BoundColumn>
            <asp:BoundColumn datafield="UserName" HeaderText="User Name" HeaderStyle-Width="15%"></asp:BoundColumn>

            <asp:TemplateColumn HeaderText="User Type" HeaderStyle-Width="10%">
                <ItemTemplate>
                    <asp:Label ID="lblUserType" Text='<%#DataBinder.Eval(Container.DataItem,"UserType")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>				
            <asp:TemplateColumn HeaderText="User Status" HeaderStyle-Width="10%" >
                <ItemTemplate>
                    <asp:Label ID="lblUserStatus" Text='<%#DataBinder.Eval(Container.DataItem,"UserCurrentStatus")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>				

            <asp:BoundColumn DataField="CreateDate" HeaderText="Created Date" HeaderStyle-Width="10%"></asp:BoundColumn>
            <asp:TemplateColumn HeaderText="Select" HeaderStyle-Width="5%">
                <ItemTemplate>
                    <a id="aModify" href="PG_CreateRole.aspx?OrgId=<%# DataBinder.Eval(Container.DataItem, "OrgId") %>&Id=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>"><asp:Label ID="lblModify" runat="server">Modify</asp:Label></a>
                </ItemTemplate>
            </asp:TemplateColumn>				
            <asp:TemplateColumn HeaderText="Select" HeaderStyle-Width="7%">
                <ItemTemplate>
                    <a id="aChgPwd" href="PG_ChangePassword.aspx?OrgId=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "OrgId")) %>&Id=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>"><asp:Label ID="lblChgPwd" runat="server">Change Password</asp:Label></a>
                </ItemTemplate>
            </asp:TemplateColumn>	
            <asp:TemplateColumn HeaderText="Select" HeaderStyle-Width="12%">
                <ItemTemplate>
                    <a id="aChgPwd" href="PG_ChangeAuthCode.aspx?OrgId=<%# GetEncrypterString(DataBinder.Eval(Container.DataItem, "OrgId")) %>&Id=<%#GetEncrypterString(DataBinder.Eval(Container.DataItem, "UserId"))%>"><asp:Label ID="lblChgAuth" runat="server">Change Validation Code</asp:Label></a>
                </ItemTemplate>
            </asp:TemplateColumn>	
            <asp:TemplateColumn HeaderText="Last Approval Status" HeaderStyle-Width="10%">
                <ItemTemplate>
                    <asp:Label ID="lblLastApprovalStatus" Text='<%#DataBinder.Eval(Container.DataItem,"LastApprovalStatus")%>' runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="User_Flag" Visible="false" />
				</Columns>
			</asp:DataGrid></asp:Panel>
		</asp:TableCell>
	</asp:TableRow>
	<asp:TableRow ID="trMsg">
		<asp:TableCell HorizontalAlign="Center">
		<asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label></asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Data Grid Ends Here -->

</asp:Content>