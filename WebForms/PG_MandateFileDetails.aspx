<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_MandateFileDetails" CodeFile="PG_MandateFileDetails.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    

    <script type="text/javascript" src="../include/common.js"></script>
    <script type="text/javascript" language="JavaScript">
        function fncBack()
        {
	        window.location.href = "PG_FileListAuth.aspx";
        }
    </script>
		
    <table cellpadding="5" cellspacing="0" border="0">
        <tr>
            <td id="cHeader">
                <asp:Label Runat="Server" CssClass="FORMHEAD" Text="" ID="lblHeading"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
        </tr>
    </table> 
     
    <table cellpadding="5" cellspacing="0" border="0">
         <tr>
            <td>File Name</td>
            <td><asp:Label ID="lblFileName" runat="SERVER"></asp:Label></td>
        </tr>
        <tr>
            <td>Total Transactions</td>
            <td><asp:Label ID="lblTotalTransactions" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Upload Date</td>
            <td><asp:Label ID="lblUploadDate" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td>Upload By</td>
            <td><asp:Label ID="lblUploadBy" runat="server"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="2">
               
	        </td>
        </tr>
    </table>  
    <asp:panel id="pnlGrid" CssClass="GridDiv" runat="server" Width="650px"> 
        <asp:DataGrid Runat="Server" ID="dgMandateDetail" AllowPaging="True" PageSize="15" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" BorderWidth="1" GridLines="Both" CellPadding="3" CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" width="300px" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundColumn datafield="BankOrgCode" HeaderText="Organization Code"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="RefNo" HeaderText="Reference No."  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="CustomerName" HeaderText="Customer Name"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="Cust_ICNumber" HeaderText="Customer IC No."  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="AccNo" HeaderText="A/C No."  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="LimitAmount" HeaderText="Limit Amount"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="Frequency" HeaderText="Frequency"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="FrequencyLimit" HeaderText="Frequency Limit"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                <asp:BoundColumn datafield="ToUpdate" HeaderText="Record Type"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>            

            </Columns>
        </asp:DataGrid>
    </asp:panel>
    <table cellpadding="8" cellspacing="0" width="80%">
       
        <tr>
            <td><asp:Label Runat="Server" CssClass="BLABEL" Text="Remarks History" ID="Label2"></asp:Label></td>
        </tr>
        <tr>
            <td><asp:Label Runat="Server" CssClass="MSG" ID="lblRemark"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:DataGrid Runat="Server" ID="dgRemarks" AllowPaging="False" PagerStyle-HorizontalAlign="Center" BorderWidth="1" GridLines="Both" CellPadding="3" CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" width="100%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
				    <Columns>
                        <asp:BoundColumn datafield="FUSER" HeaderText="User Name" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                        <asp:BoundColumn datafield="FACTION" HeaderText="Role" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                        <asp:BoundColumn datafield="FDATE" HeaderText="Date" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
                        <asp:BoundColumn DataField="FREMARKS" HeaderText="Remarks" HeaderStyle-Width="55%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
                    </Columns>
			    </asp:DataGrid>
		    </td>
        </tr>
    </table>
 
		
		
		<!-- Form Table Starts Here -->
   
		
		<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblRemarks">
		<asp:TableRow ID="trRemarks">
			<asp:TableCell Width="20%" VerticalAlign="Top">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Remarks"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox TextMode="MultiLine" ID="txtReason" Columns="30" Rows="4" Runat="Server" MaxLength="255">
				</asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtReason" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Reason Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trAuth" Visible="False">
			<asp:TableCell Width="20%">
				<asp:Label ID="lblUpload" Runat="Server" CssClass="LABEL" Text="Enter Valiation Code"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox ID="txtAuthCode" TextMode="Password" Runat="Server"></asp:TextBox>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trSubmit" Visible="False">
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Button Runat="Server" ID="btnAccept" CssClass="BUTTON" Text="Accept"></asp:Button>&nbsp;
				<asp:Button Runat="Server" ID="btnReject" CssClass="BUTTON" Text="Reject"></asp:Button>&nbsp;
				<asp:Button runat="server" ID="btnBack" CssClass="BUTTON" Text="Back" CausesValidation="false" />
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trView" Visible="false">
		    <asp:TableCell ColumnSpan="2">
		        <asp:Button ID="btnView" CssClass="BUTTON" Text="Back" runat="server" />
		    </asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trConfirm">
			<asp:TableCell Width="100%" ColumnSpan="2">
				<asp:Button ID="btnConfirm" Runat="Server" CssClass="BUTTON" Text="Confirm"></asp:Button>&nbsp;
				<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncBack();"/> 
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trBack">
			<asp:TableCell Width="100%" ColumnSpan="2">
				<input id="iptBack" type="button" runat="Server" value="Review Another File" onclick="fncBack();"/>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		<!-- Form Table Ends Here -->
		
		<input type="hidden" id="txtDate" name="txtDate" runat="Server"/>
		<input type="hidden" id="hFileId" name="hFileId" runat="Server"/>
		<input type="hidden" id="hAmount" name="hAmount" runat="Server"/>
		<input type="hidden" id="hTrans" name="hTrans" runat="Server"/>
		<input type="hidden" id="hChrg" name="hChrg" runat="Server"/>
		<input type="hidden" id="hCommand" name="hCommand" runat="Server"/>
		<asp:RequiredFieldValidator ID="rfvAuth" Runat="Server" ControlToValidate="txtAuthCode" ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
		<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{6,24}" ErrorMessage="Validation Code must be 6 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
		<asp:ValidationSummary Runat="Server" ID="vsMandateDetails" EnableClientScript="True" ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
		
</asp:Content>
