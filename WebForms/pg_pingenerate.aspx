<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_PinGenerate" CodeFile="PG_PinGenerate.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
  

      <script type="text/javascript" src="../include/common.js"></script>
    
    <script language="JavaScript" type="text/javascript">
		function fncClear()
		{
			window.history.back();
		}
		var reloadTimer = null;
        var sURL = unescape(window.location.pathname);

		function setReloadTime(secs) 
        { //this function is used to refresh the broswer
            if (arguments.length == 1) 
            { //if some seconds are passed in then create and set the timer and 
              //have it call this function again with no seconds passed in
                if (reloadTimer) clearTimeout(reloadTimer);
                reloadTimer = setTimeout("setReloadTime()", 
                                 Math.ceil(parseFloat(secs) * 1000));
            }
            else 
            { //No seconds were passed in the timer must be up clear the timer 
             //and refresh the browser
                reloadTimer = null;
                //passing true causes the request to go back to the web server
                // false refreshs the page from history
                //This is javascript 1.2
                //location.reload(true);
                //This is javascript 1.1
                window.location.replace( sURL );
            }
        }
    </script>
		<!-- Main Table Starts Here -->
		<table cellpadding="5" cellspacing="0" width="100%" border="0">
      <tr>
         <td id="cHeader"><asp:Label Text="" CssClass="FORMHEAD" Runat="server" id="lblHeading">Pin Mailer - Generation</asp:Label></td>
      </tr>
       <tr>
         <td id="cErrMsg"><asp:Label ID="lblMessage" Runat="Server" CssClass="MSG"></asp:Label></td>
      </tr>
  </table>
		<asp:Table ID="tblMain" CellPadding="8" CellSpacing="0" Runat="Server" Width="100%">
		<asp:TableRow>
			<asp:TableCell Width="10%" Wrap="False">
				<asp:Label Runat="server" CssClass="LABEL" Text="Approved within"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">&nbsp;
				<asp:TextBox ID="txtDays" CssClass="MINITEXT" MaxLength="3" Runat="server" Text="3"></asp:TextBox>&nbsp;
				<asp:Label Runat="server" CssClass="LABEL" Text="Days" ID="Label3"></asp:Label>&nbsp;
				<asp:Button ID="btnShow" Text="Show" CausesValidation="false" Runat="Server" CssClass="BUTTON"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		<!-- Main Table Ends Here -->
		
		<!-- Datagrid Starts Here -->
		<asp:Table ID="tblGrid" Runat="server" Width="100%" CellPadding="12" CellSpacing="0" Visible="False">
		<asp:TableRow><asp:TableCell ColumnSpan="2" Width="100%">
		<asp:panel CssClass = "GridDivNoScroll" runat =server ID="pnlGrid">
		<asp:DataGrid CssClass = "Grid" Borderwidth=0 ID="dgPinGenerate" Runat="server" AllowPaging="False" 
			PagerStyle-HorizontalAlign="Center" 
			HeaderStyle-CssClass="GridHeaderStyle"
			AlternatingItemStyle-CssClass="GridAltItemStyle"
		    ItemStyle-CssClass="GridItemStyle" width="100%" HeaderStyle-HorizontalAlign="Left"
			AutoGenerateColumns="False">
			<Columns>
				<asp:BoundColumn Datafield="REQID" HeaderText="" HeaderStyle-Width="0%"  ReadOnly="True" Visible="False"></asp:BoundColumn>
				<asp:BoundColumn DataField="ORGID" HeaderText="Org. Id" HeaderStyle-Width="5%"></asp:BoundColumn>
				<asp:BoundColumn DataField="NAME" HeaderText="Organization Name"  HeaderStyle-Width="18%"></asp:BoundColumn>
				<asp:BoundColumn DataField="VERIFY" HeaderText="Verification"  HeaderStyle-Width="5%"></asp:BoundColumn>
				<asp:BoundColumn DataField="TYPE" HeaderText="User Type" HeaderStyle-Width="10%"></asp:BoundColumn>
				<asp:BoundColumn DataField="CODE" HeaderText="Code Type" HeaderStyle-Width="10%"></asp:BoundColumn>
				<asp:TemplateColumn  ItemStyle-Width="3%" HeaderText="Select">
					<ItemTemplate><asp:CheckBox Runat="server" ID="chkSelect"></asp:CheckBox></ItemTemplate>
				</asp:TemplateColumn>
			</Columns>
		</asp:DataGrid>
		</asp:panel>
		</asp:TableCell></asp:TableRow>
		<asp:TableRow ID="trSelect">
			<asp:TableCell Width="90%" HorizontalAlign="Center" ColumnSpan="2">
				<asp:Button ID="btnSelect" Runat="server" Text="Select All" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
				<asp:Button ID="btnUnSelect" Runat="server" Text="Unselect All" CssClass="BUTTON" CausesValidation="False"></asp:Button>&nbsp;
				<asp:Button ID="btnConfirm" Runat="server" Text="Confirm" CssClass="BUTTON"></asp:Button>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
				<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trAuthCode">
				<asp:TableCell Width="20%">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Validation Code" ID="Label1"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtAuthCode" CssClass="MEDIUMTEXT" Runat="Server" TextMode="Password" MaxLength="24"></asp:TextBox>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="90%" ColumnSpan="2">&nbsp;</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trSubmit">
				<asp:TableCell Width="90%" ColumnSpan="2">
					<asp:Button ID="btnGenerate" CssClass="BUTTON" Runat="Server" Text="Generate"></asp:Button>&nbsp;
					<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncClear();">
				</asp:TableCell>
			</asp:TableRow>
		</asp:Table>
		<!-- Datagrid Ends Here -->
		
		<!-- Validators Starts Here -->
		<asp:RequiredFieldValidator ID="rfvDays" Runat="Server" ControlToValidate="txtDays" Display="None" ErrorMessage="No Days cannot be blank."></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="revDays" Runat="Server" ControlToValidate="txtDays" ValidationExpression="\d{1,3}" ErrorMessage="No of Days must be numeric value" Display="None"></asp:RegularExpressionValidator>
		<asp:RequiredFieldValidator ID="rfvAuth" Runat="Server" ControlToValidate="txtAuthCode" ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
		<asp:ValidationSummary ID="vsPinRequest" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations," ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
		<!-- Validators Ends Here -->
		
</asp:Content>