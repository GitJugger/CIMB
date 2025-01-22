<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_FileReview" CodeFile="PG_FileReview.aspx.vb"
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
				window.location.href = "PG_FileList.aspx";
			}
		</script>
		
		<table cellpadding="5" cellspacing="0" width="100%" border="0">
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
 
  <table cellpadding="8" cellspacing="0" width="100%" border="0">
      <tr>
         <td>
            <asp:panel ID="pnlReport" runat="server" BorderStyle="solid" BorderWidth="1" Height="357px" BorderColor="#000000" ScrollBars="None">
		           <rsweb:ReportViewer ID="rvReport" runat="server" Height="300px" Width="100%" ProcessingMode="Remote" ShowParameterPrompts="false"	>
               </rsweb:ReportViewer>		
            </asp:panel>
         </td>
      </tr>
      <tr>
         <td>
            <asp:panel ID="pnlReport2" runat="server" BorderStyle="solid" BorderWidth="1" Height="357px" BorderColor="#000000" ScrollBars="None" Visible="false" >
		           <rsweb:ReportViewer ID="rvReport_2" runat="server" Height="300px" Width="100%" ProcessingMode="Remote" ShowParameterPrompts="false">
               </rsweb:ReportViewer>		
            </asp:panel>
         </td>
      </tr>
  </table>
  
  
		<!-- RAS Table Starts Here -->
		<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblReview">
			
			<asp:TableRow ID="trNote" Visible="false">
				<asp:TableCell Width="100%">
					<asp:Label CssClass="LABEL" Runat="Server" Text="Please Note: Duplicate Account Numbers will be highlighted for your verification." ID="Label1" NAME="Label1"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow><asp:TableCell Width="100%"></asp:TableCell></asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%"><asp:Label Runat="Server" CssClass="BLABEL" Text="Remarks History" ID="Label2" NAME="Label2"></asp:Label></asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%">
					<asp:Label Runat="Server" CssClass="MSG" ID="lblRemark"></asp:Label><br />
				</asp:TableCell>	
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="100%">
					<!-- Data Grid Starts Here -->
					<asp:DataGrid Runat="Server" ID="dgRemarks" AllowPaging="False" PagerStyle-HorizontalAlign="Center" BorderWidth="1" GridLines="Both" CellPadding="3"
						CellSpacing="0" Font-Name="Verdana" Font-Size="8pt" HeaderStyle-CssClass="LABELHEAD" width="100%" HeaderStyle-Font-Bold="True" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False">
						<Columns>
							<asp:BoundColumn datafield="FUSER" HeaderText="User Name" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn datafield="FACTION" HeaderText="Role" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn datafield="FDATE" HeaderText="Date" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
							<asp:BoundColumn DataField="FREMARKS" HeaderText="Remarks" HeaderStyle-Width="55%" HeaderStyle-HorizontalAlign="center" ItemStyle-HorizontalAlign="Left"></asp:BoundColumn>
						</Columns>
					</asp:DataGrid>
					<!-- Data Grid Ends Here -->
				</asp:TableCell>
			</asp:TableRow>
			

		</asp:Table>
		<!-- RAS Table Ends Here -->
		
		<!-- Form Table Starts Here -->
		<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblForm">
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label ID="lblFType" Runat="Server" CssClass="LABEL" Text="File Type"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label ID="lblType" Runat="Server" CssClass="BLABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow>
			<asp:TableCell Width="20%">
				<asp:Label ID="lblName" Runat="Server" CssClass="LABEL" Text="File Name"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label ID="lblFName" Runat="Server" CssClass="BLABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trAmt">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" Text="Total Amount" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label ID="lblAmt" Runat="Server" CssClass="BLABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trTran">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" Text="Total Transactions" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label ID="lblTran" Runat="Server" CssClass="BLABEL"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trChrg">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" Text="Transaction Charge" CssClass="LABEL"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label Runat="Server" CssClass="BLABEL" ID="lblChrg"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		<asp:TableRow ID="trPHTotal">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" Text="Public Hash Total" CssClass="LABEL" ID="Label3" Visible="false"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label Runat="Server" CssClass="BLABEL" ID="lblPHTotal" Visible="false"></asp:Label>
			</asp:TableCell>
		</asp:TableRow>
		</asp:Table>
		
		<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblRemarks">
		<asp:TableRow ID="trRemarks">
			<asp:TableCell Width="20%" VerticalAlign="Top">
				<asp:Label Runat="Server" CssClass="LABEL" Text="Remarks"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:TextBox TextMode="MultiLine" ID="txtReason" Columns="30" Rows="4" Runat="Server" MaxLength="255"></asp:TextBox>
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
				
				<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncBack();"/>
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
				<input type="button" runat="Server" value="Review Another File" onclick="fncBack();"/>
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
		<asp:ValidationSummary Runat="Server" ID="vsFileReview" EnableClientScript="True" ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
		
</asp:Content>
