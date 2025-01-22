<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_FileAuth" CodeFile="PG_FileAuth.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
   
   		<script type="text/javascript" src="../include/common.js"></script>
   		<%--<object
	  classid="clsid:04E3E561-AC82-4B19-A646-020A4347C964"
	  codebase="./tgmPKIXControl.cab#version=1,0,2,27"
	  width="0"
	  height="0"
	  hspace="0"
	  vspace="0"
	  id="tgmPKIX">
	  
</object>--%>
		<script type="text/javascript" language="JavaScript">
			function SignMsg() 
			{
				var result;
				
				tgmPKIX.DBMainTitle	= "Digital Certificate Authentication is required!";
				tgmPKIX.DBSelectTitle	= " ";
				tgmPKIX.DBSelectText	= "Select the Digital Certificate that was assigned to you by the bank.";
				tgmPKIX.DBIDTitle	= "CIMB Gateway Digital ID";
				tgmPKIX.DBAction	= "OK";
				
				result = tgmPKIX.SignMsg (document.forms[0].ctl00$cphContent$hdnData.value);

				if (result) {
					//alert("Error: " + result);
					return;
				}
				
				document.forms[0].ctl00$cphContent$hdnSignature.value = tgmPKIX.Signature;
				document.forms[0].submit();
				//return true;
			}
			//{
				/*
				Must add the object before tgPKI can be called. refer to line 10.
				Hidden field ( Data and Signature ) must be declare.
				Data will be the message to be displayed.
				The values of these 2 fields are required to retrieve the certificate.  
				*/
			//	tgPKI.Reset;
				//document.forms[0].Data.value = "RHB Banking e-HR";
				
			//	var result = tgPKI.SignMsg (document.forms[0].hdnData.value, true);
				
			//	if (result > 0) 
			//		return;
		
			//	document.forms[0].hdnSignature.value = tgPKI.Signature;
				//alert( tgPKI.VisibleDockClientCount )
			//	document.forms[0].submit();
			//}
			function fncBack()
			{
				window.location.href = "PG_FileListAuth.aspx";
			}
		</script>
		
		  <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Approve Submitted File" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td>
               <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
			<input type="hidden" id="hdnSignature" runat="server" name="hdnSignature"/>
			<input type="hidden" id="hdnData" runat="server" name="hdnData"/>
			<table cellpadding="8" cellspacing="0" border="0">
			   <tr>
			      <td><asp:panel ID="pnlReport" runat="server" CssClass="GridDivNoScroll" Height="357px">
			   <rsweb:ReportViewer ID="rvReport" runat="server" Height="300px" Width="100%" ProcessingMode="Remote" ShowParameterPrompts="false">
                </rsweb:ReportViewer>	
			  </asp:panel></td>
			   </tr>
			   <tr>
         <td>
            <asp:panel ID="pnlReport2" runat="server" BorderStyle="solid" BorderWidth="1" Height="357px" BorderColor="#000000" ScrollBars="None" Visible="false" >
		           <rsweb:ReportViewer ID="rvReport_2" runat="server" Height="300px" Width="100%" ProcessingMode="Remote" ShowParameterPrompts="false"	>
               </rsweb:ReportViewer>		
            </asp:panel>
         </td>
      </tr>
			</table>
			
			<!-- RAS Table Starts Here -->
			<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblReview">
				<asp:TableRow>
					<asp:TableCell Width="100%">
						
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow ID="trNote" Visible="false">
					<asp:TableCell Width="100%">
						<asp:Label CssClass="LABEL" Runat="Server" Text="Please Note: Duplicate Account Numbers will be highlighted for your verification." ID="Label1" NAME="Label1"></asp:Label>
					</asp:TableCell>
				</asp:TableRow>
				<asp:TableRow><asp:TableCell Width="100%"></asp:TableCell></asp:TableRow>
				
			</asp:Table>
			<!-- RAS Table Ends Here -->
			
			<!-- Remarks Data grid Starts Here -->
			<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblRemark" Width="75%">
				<asp:TableRow>
					<asp:TableCell Width="100%"><asp:Label Runat="Server" CssClass="BLABEL" Text="Remarks History" ID="Label2" NAME="Label2"></asp:Label></asp:TableCell>
				</asp:TableRow>
				<asp:TableRow>
					<asp:TableCell Width="100%">
						<asp:Label Runat="Server" CssClass="MSG" ID="lblRemark"></asp:Label><br />
					<asp:Label Runat="Server" CssClass="MSG" ID="lblPartnerFile" Visible="false" ></asp:Label>
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
				<asp:TableRow>
					<asp:TableCell>&nbsp;</asp:TableCell>
				</asp:TableRow>
			</asp:Table>
		
			<!-- Form Table Starts Here -->
			<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblForm">
			<asp:TableRow>
				<asp:TableCell Width="45%">
					<asp:Label ID="lblFType" Runat="Server" CssClass="LABEL" Text="File Type"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="55%">
					<asp:Label ID="lblType" Runat="Server" CssClass="BLABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow>
				<asp:TableCell Width="45%">
					<asp:Label ID="lblName" Runat="Server" CssClass="LABEL" Text="File Name"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="55%">
					<asp:Label ID="lblFName" Runat="Server" CssClass="BLABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trAmt">
				<asp:TableCell Width="45%">
					<asp:Label Runat="Server" Text="Total Amount" CssClass="LABEL" ID="Label3"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="55%">
					<asp:Label ID="lblAmt" Runat="Server" CssClass="BLABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trTran">
				<asp:TableCell Width="45%">
					<asp:Label Runat="Server" Text="Total Transactions" CssClass="LABEL" ID="Label4"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="55%">
					<asp:Label ID="lblTran" Runat="Server" CssClass="BLABEL"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trChrg">
				<asp:TableCell Width="45%">
					<asp:Label Runat="Server" Text="Transaction Charge" CssClass="LABEL" ID="Label5"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="55%">
					<asp:Label Runat="Server" CssClass="BLABEL" ID="lblChrg"></asp:Label>
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trPHTotal">
			<asp:TableCell Width="20%">
				<asp:Label Runat="Server" Text="Public Hash Total" CssClass="LABEL" ID="Label7" Visible="false"></asp:Label>
			</asp:TableCell>
			<asp:TableCell Width="80%">
				<asp:Label Runat="Server" CssClass="BLABEL" ID="lblPHTotal" Visible="false"></asp:Label>
			</asp:TableCell>
			</asp:TableRow>
			</asp:Table>
			
			<asp:Table Runat="server" CellPadding="8" CellSpacing="0" id="tblRemarks">
			<asp:TableRow ID="trRemarks">
				<asp:TableCell Width="30%" VerticalAlign="Top">
					<asp:Label Runat="Server" CssClass="LABEL" Text="Remarks" ID="Label6"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox TextMode="MultiLine" ID="txtReason" Columns="30" Rows="4" Runat="Server" MaxLength="255"></asp:TextBox>
			        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtReason" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Reason Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
				</asp:TableCell>
			</asp:TableRow>
			
			<asp:TableRow ID="trAuth" Visible="False">
				<asp:TableCell Width="30%">
					<asp:Label ID="lblUpload" Runat="Server" CssClass="LABEL" Text="Enter Validation Code"></asp:Label>
				</asp:TableCell>
				<asp:TableCell Width="70%">
					<asp:TextBox ID="txtAuthCode" TextMode="Password" Runat="Server"></asp:TextBox>

				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trChallengeCode" Visible=false>
				<asp:TableCell>
					<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Challenge Code" ID="Label8"></asp:Label>
				</asp:TableCell>
				<asp:TableCell><asp:TextBox ID="txtChallengeCode" CssClass="BIGTEXT" Runat="Server" ReadOnly=true></asp:TextBox></asp:TableCell>
			</asp:TableRow>				
			<asp:TableRow ID="trDynaPin" Visible=false>
				<asp:TableCell>
					<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Dyna Pin" ID="Label9"></asp:Label>
				</asp:TableCell>
				<asp:TableCell><asp:TextBox ID="txtDynaPin" CssClass="BIGTEXT" Runat="Server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtDynaPin" ValidationExpression="[A-Za-z0-9]" ErrorMessage="Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>

				</asp:TableCell>
	        </asp:TableRow>			
			<asp:TableRow ID="trSubmit" Visible="False">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Button Runat="Server" ID="btnAccept" CssClass="BUTTON" Text="Accept"></asp:Button>&nbsp;
					<asp:Button Runat="Server" ID="btnReject" CssClass="BUTTON" Text="Reject"></asp:Button>&nbsp;
					<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncBack();" ID="Button1" NAME="Button1">
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trConfirm">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<asp:Button ID="btnConfirm" Runat="Server" CssClass="BUTTON" Text="Confirm"></asp:Button><asp:Button ID="btnSign" Text="Sign" CssClass="BUTTON" runat=server/>&nbsp;
					<input type="button" runat="Server" value="Back" class="BUTTON" onclick="fncBack();" ID="Button2" NAME="Button2"> 
				</asp:TableCell>
			</asp:TableRow>
			<asp:TableRow ID="trBack">
				<asp:TableCell Width="100%" ColumnSpan="2">
					<input type="button" runat="Server" value="Approve Another File" onclick="fncBack();" ID="Button3" NAME="Button3">
				</asp:TableCell>
			</asp:TableRow>
			</asp:Table>
			<!-- Form Table Ends Here -->
		
			<input type="hidden" id="txtDate" name="txtDate" runat="Server">
			<input type="hidden" id="hFileId" name="hFileId" runat="Server">
			<input type="hidden" id="hAmount" name="hAmount" runat="Server">
			<input type="hidden" id="hTrans" name="hTrans" runat="Server">
			<input type="hidden" id="hChrg" name="hChrg" runat="Server">
			<input type="hidden" id="hCommand" name="hCommand" runat="Server">
			<input type="hidden" id="hFileName" name="hFileName" runat="server">
			
			<asp:RequiredFieldValidator ID="rfvAuth" Runat="Server" ControlToValidate="txtAuthCode" ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:ValidationSummary Runat="Server" ID="vsFileReview" EnableClientScript="True" ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
			
</asp:Content>