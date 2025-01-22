<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_FileListAuth" CodeFile="PG_FileListAuth.aspx.vb"
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <script type="text/javascript" src="../include/common.js"></script>
<%-- <object
   classid="clsid:04E3E561-AC82-4B19-A646-020A4347C964"
   codebase="./tgmPKIXControl.cab#version=1,0,2,27"
   width="0"
   height="0"
   hspace="0"
   vspace="0"
   id="tgmPKIX"
 >
</object>--%>
		
    
    
      <script type="text/javascript" language="JavaScript">

       function SignMsg() 
   {
    var result;
    
    tgmPKIX.DBMainTitle = "Digital Certificate Authentication is required!";
    tgmPKIX.DBSelectTitle = " ";
    tgmPKIX.DBSelectText = "Select the Digital Certificate that was assigned to you by the bank.";
    tgmPKIX.DBIDTitle = "CIMB Gateway Digital ID";
    tgmPKIX.DBAction = "OK";
    
    result = tgmPKIX.SignMsg (document.forms[0].ctl00$cphContent$hdnData.value);

    if (result) {
     //alert("Error: " + result);
     return;
    }
    
    document.forms[0].hdnSignature.value = tgmPKIX.Signature;
    document.forms[0].submit();
    //return true;
   }
           	function fncBack()
		{
			window.location.href = "PG_FileListAuth.aspx";
		}
		function fncClear()
		{
			window.location.href = "PG_FileListAuth.aspx";
		}
	 </script>
    
       <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader" width="45%" height="18">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="File Approve & Submission" ID="lblHeading"></asp:Label>
            </td>
            <td align="left">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Include/Images/mozilla_blu.gif" />&nbsp;<font color="gray">Loading</font>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
         </tr>
         <tr>
            <td colspan="2">
               <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
	<!-- Main Table Starts Here -->
	<input type="hidden" id="hdnSignature" name="Signature" runat="server" />
	<input type="hidden" id="hdnData" name="Data" runat="server" />
	<asp:Table Width="100%" ID="tblMain" CellPadding="8" CellSpacing="0" Runat="Server">
		<asp:TableRow>
			<asp:TableCell Width="100%">
				<!--Form Table Starts Here -->
				<asp:Table Width="100%" ID="tblForm" CellPadding="2" CellSpacing="0" Runat="Server">
	                <asp:TableRow>
	                    <asp:TableCell Width="20%">
	                    </asp:TableCell>
	                    <asp:TableCell Width="80%">
	                    </asp:TableCell>
	                </asp:TableRow>			
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2">
							<!-- Data Grid Starts here -->
							<asp:panel id="pnlGrid" CssClass="GridDiv" runat="server">
							<asp:UpdatePanel ID="updpnl_Form" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
                                    <ContentTemplate>
							<asp:DataGrid ID="dgFile" Runat="Server" AllowSorting="False" AutoGenerateColumns="False" PagerStyle-Mode="NumericPages" 
								CellPadding="3" CellSpacing="0" GridLines="none" PagerStyle-HorizontalAlign="Center" BorderColor="black" BorderWidth="0" Font-Names="Verdana" Font-Size="8pt" Width="100%" HeaderStyle-Font-Bold="True" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-CssClass="GridItemStyle" AlternatingItemStyle-CssClass="GridAltItemStyle"  ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left">
								<Columns>
									<asp:TemplateColumn HeaderText="" HeaderStyle-Width="3%">
										<ItemTemplate>
                                                <%--<asp:CheckBox ID="chkSelect" Runat="Server"></asp:CheckBox>--%>		
                                                   <asp:CheckBox ID="chkSelect" runat="Server" AutoPostBack="true" ></asp:CheckBox>
								</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn HeaderText="File Type" HeaderStyle-Width="10%">
										<ItemTemplate>
										 <asp:Label Visible="false" ID="lblFileType" runat="server"><%#DataBinder.Eval(Container.DataItem,"FType")%></asp:Label>
										 <asp:Label ID="lblaFileType" runat="server"><%#DataBinder.Eval(Container.DataItem,"FType")%></asp:Label>
										
										</ItemTemplate>
									</asp:TemplateColumn>
										<asp:BoundColumn DataField="FId" HeaderText="File Id"  Visible ="False" ItemStyle-Width="15%" ></asp:BoundColumn>
										<asp:BoundColumn DataField="FType" HeaderText="File Type"  Visible ="False" ItemStyle-Width="15%"></asp:BoundColumn>
										<asp:BoundColumn DataField="FName" HeaderText="File Name"  ItemStyle-Width="15%"></asp:BoundColumn>
										<asp:BoundColumn DataField="VDate" HeaderText="Payment Date" ItemStyle-Width="15%"></asp:BoundColumn>
										<asp:BoundColumn DataField="WfId" HeaderText="WorkFlow Id"   Visible ="False" ItemStyle-Width="15%"></asp:BoundColumn>
										<asp:BoundColumn datafield="Date" HeaderText="Upload Date"  visible="false" ItemStyle-Width="15%"></asp:BoundColumn>
										<asp:BoundColumn datafield="FTrans" HeaderText="No. of Transaction" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
										<asp:BoundColumn datafield="FAmount" HeaderText="Total Amount" HeaderStyle-Width="10%"  HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"></asp:BoundColumn>
										<asp:BoundColumn datafield="FFName" HeaderText="File Name" HeaderStyle-Width="10%" visible = "false" ></asp:BoundColumn>
										<asp:TemplateColumn HeaderText="Remarks" HeaderStyle-Width="10%">
									<ItemTemplate>
										<asp:TextBox ID="txtRemarks" Runat="Server" CssClass="MEDIUMTEXT" TextMode="MultiLine" Rows="3" MaxLength="255"></asp:TextBox>
                                         <asp:RegularExpressionValidator ID="RegularExpressionValidator1" Runat="server" ControlToValidate="txtRemarks" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Remarks Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
									</ItemTemplate>
									</asp:TemplateColumn>
                                     <asp:BoundColumn DataField="FileType_Id" HeaderText="FileType_Id" HeaderStyle-Width="15%" Visible="false"></asp:BoundColumn> 
                                     <asp:BoundColumn DataField="PaySer_Id" HeaderText="PaySer_Id" HeaderStyle-Width="15%" Visible="false"></asp:BoundColumn>  
								</Columns>
							</asp:DataGrid>
							 </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnCheckAll" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnUnCheck" EventName="click" />
                                    </Triggers>
                                </asp:UpdatePanel>
							</asp:panel>
							<!-- Data Grid Ends Here -->
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
                        <asp:TableCell ColumnSpan="2"><asp:TextBox BorderWidth="0" Width="100%" Height="100" BorderStyle="None" ID="txtalert" Runat="server" CssClass="MSG" ReadOnly ="True" TextMode ="MultiLine" ></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2" HorizontalAlign="Center">
							<asp:Button ID="btnCheckAll" Runat="server" Text="Select All" CssClass="BUTTON"></asp:Button>&nbsp;  
							<asp:Button ID="btnUnCheck" Runat="server" Text="Unselect All" CssClass="BUTTON"></asp:Button>&nbsp;
					</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trAuthCode">
						<asp:TableCell>
							<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Validation Code" ID="Label1"></asp:Label>
						</asp:TableCell>
						<asp:TableCell>
						    <asp:TextBox ID="txtAuthCode" CssClass="BIGTEXT" Runat="Server" TextMode="Password" MaxLength="24"></asp:TextBox>
						</asp:TableCell>
					</asp:TableRow>
	                <asp:TableRow ID="trChallengeCode" Visible="false">
						<asp:TableCell>
							<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Challenge Code" ID="Label3"></asp:Label>
						</asp:TableCell>
						<asp:TableCell><asp:TextBox ID="txtChallengeCode" CssClass="BIGTEXT" Runat="Server" ReadOnly="true"></asp:TextBox></asp:TableCell>
					</asp:TableRow>				
					<asp:TableRow ID="trDynaPin" Visible="false">
						<asp:TableCell>
							<asp:Label Runat="Server" CssClass="LABEL" Text="Enter Dyna Pin" ID="Label2"></asp:Label>
						</asp:TableCell>
						<asp:TableCell><asp:TextBox ID="txtDynaPin" CssClass="BIGTEXT" Runat="Server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" Runat="server" ControlToValidate="txtDynaPin" ValidationExpression="^[\w\-\s]+$" ErrorMessage="Pin Accepts Alpha Numeric, Underscore, Dash Only" Display="None"></asp:RegularExpressionValidator>
                            ></asp:TableCell>
					</asp:TableRow>
					<asp:TableRow>
						<asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trSubmit">
						<asp:TableCell ColumnSpan="2">
							<asp:Button ID="btnAccept" CssClass="BUTTON" Runat="Server" Text="Accept"></asp:Button>&nbsp;
							<asp:Button ID="btnReject" CssClass="BUTTON" Runat="Server" Text="Reject"></asp:Button>&nbsp;
							<input type="button" id="btnReset" name="btnReset" runat="Server" value="Clear" class="BUTTON" onclick="fncClear();"/>
						</asp:TableCell>
					</asp:TableRow>
					<asp:TableRow ID="trConfirm">
						<asp:TableCell ColumnSpan="2">
							<asp:Button ID="btnConfirm" Runat="Server" CssClass="BUTTON" Text="Confirm"></asp:Button><input id="btnSignMSCTrustGate" runat="server" onclick="javascript:SignMsg();" class="BUTTON" value="Sign" type="button" visible="false"/><asp:Button ID="btnSign" runat="server" Text="Sign" CssClass="BUTTON" />&nbsp;
							<input type="button" id="btnBack" name="btnBack" runat="Server" value="Back" class="BUTTON" onclick="fncBack();"/>
						</asp:TableCell>
					</asp:TableRow>
				</asp:Table>
				<!-- Form Table Ends Here -->
			</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
	
			<input id="hdncommand" type="hidden" name="hdndcommand" runat="Server"/>
			<input id="hdnpymtdt" type="hidden" name="hdnpymtdt" runat="Server"/>
			<input id="hdnTAmount" type="hidden" name="hdnTAmount" runat="Server"/>
						
			<asp:RequiredFieldValidator ID="rfvAuth" Runat="Server" ControlToValidate="txtAuthCode" ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
			<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
			<asp:ValidationSummary Runat="Server" ID="vsFileReview" EnableClientScript="True" ShowMessageBox="True" ShowSummary="False" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
			
	<!-- Main Table Ends Here -->
 
</asp:Content> 