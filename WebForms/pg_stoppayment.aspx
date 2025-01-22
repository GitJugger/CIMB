<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_StopPayment" CodeFile="PG_StopPayment.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

		<script type="text/javascript" src="../include/common.js"></script>
    
    <script type="text/javascript" language="javascript">
    function fncShow()
    {
		document.location.href = "PG_StopPayment.aspx";
	}
    </script>
	 <table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label CssClass="FORMHEAD" Runat="server" id="lblHeading">Stop Payment - View Files</asp:Label></td>
       </tr>
      
  </table>
	<!-- Main Table Starts Here -->
	<table Width="100%" CellPadding="8" CellSpacing="0">
	<tr>
		<td Width="100%">
			<!-- Form Table Starts Here -->
			<asp:Table Width="100%" CellPadding="2" CellSpacing="0" Runat="Server" ID="tblSearch">
				<asp:TableRow ID="trOption">
					<asp:TableCell Width="20%">
						<asp:Label Runat="Server" ID="lblOption" Text="Search Option" CssClass="LABEL"></asp:Label></asp:TableCell>
					<asp:TableCell Width="80%">
						<asp:DropDownList ID="cmbOption" CssClass="MEDIUMTEXT" Runat="Server">
							<asp:ListItem Value=""></asp:ListItem>
							<asp:ListItem Value="OrgId">Organization Id</asp:ListItem>
							<asp:ListItem Value="OrgName">Organization Name</asp:ListItem>
							<asp:ListItem Value="FName">File Name</asp:ListItem>
						</asp:DropDownList>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow ID="trCriteria">
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
				<asp:TableRow ID="trKeyword">
					<asp:TableCell Width="20%">
						<asp:Label Runat="Server" ID="lblKeyword" Text="Search Keyword" CssClass="LABEL"></asp:Label></asp:TableCell>
					<asp:TableCell Width="80%">
						<asp:TextBox ID="txtKeyword" CssClass="MEDIUMTEXT" Runat="Server" MaxLength="50"></asp:TextBox></asp:TableCell>
				</asp:TableRow>
				<asp:TableRow ID="trButton">
					<asp:TableCell Width="100%" ColumnSpan="2">
						<asp:Button Runat="Server" ID="btnSearch" Text="Search" CssClass="BUTTON"></asp:Button>&nbsp;
						<asp:Button Runat="Server" ID="btnClear" Text="Clear" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
						<input type="button" class="BUTTON" value="Show All" onclick="fncShow();" />
					</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
			<!-- Form Table Ends Here -->
		</td>
	</tr>
	</table>
	<asp:RequiredFieldValidator ID="rfvOption" Runat="server" ControlToValidate="cmbOption" ErrorMessage="Search Option" Display="Dynamic"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvCriteria" Runat="server" ControlToValidate="cmbCriteria" ErrorMessage="Search Criteria" Display="Dynamic"></asp:RequiredFieldValidator>
	<asp:RequiredFieldValidator ID="rfvKeyword" Runat="server" ControlToValidate="txtKeyword" ErrorMessage="Search Keyword" Display="Dynamic"></asp:RequiredFieldValidator>
	<asp:RegularExpressionValidator
                                            ID="revKeyWord" runat="server" ControlToValidate="txtKeyword" Display="None"
                                            ErrorMessage="KeyWord Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
    <asp:ValidationSummary ShowMessageBox="True" ShowSummary="False" HeaderText="Please Fill in the Following Fields," ID="vsStopPayment" Runat="Server" EnableClientScript="True"></asp:ValidationSummary>
	<!-- Data Grid Starts Here -->
	<asp:Table Runat="server" Width="100%" CellPadding="12" CellSpacing="0" id="Table3">
	<asp:TableRow>
	<asp:TableCell Width="100%" ColumnSpan="3">&nbsp;</asp:TableCell></asp:TableRow>
		<asp:TableRow ID="trMsg">
			<asp:TableCell>
				<asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label></asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trGrid">
			<asp:TableCell Width="100%">
				<asp:DataGrid ID="dgStopPay" Runat="Server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="3" CellSpacing="0" 
				PageSize="15" GridLines="none" PagerStyle-HorizontalAlign="Center" BorderWidth="1" BorderColor="black" PagerStyle-Mode="NumericPages" OnPageIndexChanged="prPageChange" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
				<Columns>
					<asp:BoundColumn datafield="OID" HeaderText="Organisation Id" HeaderStyle-Width="14%"></asp:BoundColumn>
					<asp:BoundColumn datafield="OName" HeaderText="Organisation Name" HeaderStyle-Width="30%"></asp:BoundColumn>
					<asp:BoundColumn DataField="GName" HeaderText="Group Name" HeaderStyle-Width="20%"></asp:BoundColumn>
					<asp:BoundColumn DataField="UDate" HeaderText="Upload Date"  HeaderStyle-Width="12%"></asp:BoundColumn>
					<asp:BoundColumn DataField="VDate" HeaderText="Payment Date"  HeaderStyle-Width="12%" ></asp:BoundColumn>
					<asp:BoundColumn DataField="FName" HeaderText="File Name"></asp:BoundColumn>
					<asp:TemplateColumn HeaderText="Select" >
						<ItemTemplate>
							<a href="PG_DeleteFile.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"FID")%>&OId=<%#DataBinder.Eval(Container.DataItem,"OID")%>&OName=<%#DataBinder.Eval(Container.DataItem,"OName")%>&FName=<%#DataBinder.Eval(Container.DataItem,"FName")%>&FCName=<%#DataBinder.Eval(Container.DataItem,"FCName")%>&UDate=<%#DataBinder.Eval(Container.DataItem,"UDate")%>&VDate=<%#DataBinder.Eval(Container.DataItem,"VDate")%>&Ftype=<%#DataBinder.Eval(Container.DataItem,"Ftype")%>&Status=<%#DataBinder.Eval(Container.DataItem,"Status")%>&Group=<%#DataBinder.Eval(Container.DataItem,"GID")%>&GName=<%#DataBinder.Eval(Container.DataItem,"GName")%>">Stop Payment</a>
						</ItemTemplate>
					</asp:TemplateColumn>
				</Columns>
			</asp:DataGrid>
		</asp:TableCell>
	</asp:TableRow>
	</asp:Table>
	<!-- Data Grid Ends Here -->
	<!--Main Table Ends Here -->

</asp:Content>