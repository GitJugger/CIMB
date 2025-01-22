<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_Group2" CodeFile="pg_Group2.aspx.vb" 
    MasterPageFile="~/WebForms/mp_Master.master" EnableEventValidation="true" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
		<script type="text/javascript" src="../include/common.js"></script>
   		<script type="text/javascript" language="JavaScript">
		function fncBack()
		{
			var strRequest;	
			var iPageNo;
			var isNew;
			//alert(window.location.href);
			strRequest = document.all('ctl00$cphContent$hRequest').value;
			isNew = document.all('ctl00$cphContent$hIsNew').value;
			iPageNo = document.all('ctl00$cphContent$hPageNo').value;
			if(isNew == "New")
			{
				window.location.href = "pg_Group2.aspx?Mod=New";
			}
			else
			{
			    if(strRequest == "")
			    {
				    window.location.href = "PG_ListGroup.aspx?PageNo=" + iPageNo;
			    }
			    else if(strRequest == "Submit")
			    {
				    window.history.back();
			    }
			    else if(strRequest == "View")
			    {
				    window.location.href = "PG_ApprMatrix.aspx?Mode=View&PageNo=" + iPageNo;
			    }
			    else if(strRequest == "Reject")
			    {
				    window.location.href = "PG_ApprMatrix.aspx?Mode=Reject&PageNo=" + iPageNo;
			    }
			    else if(strRequest == "Done")
			    {
				    window.location.href = "PG_ApprMatrix.aspx?Mode=Done&PageNo=" + iPageNo;
			    }
			    else if(strRequest == "Edit")
			    {
				    window.location.href = "PG_ApprMatrix.aspx?Mode=Edit&PageNo=" + iPageNo;
			    }
	        }
		}
		function fncView()
		{
			window.location.href = "PG_ListGroup.aspx";
		}
		</script>
		
    
    <!-- Main Table Starts Here -->
      <table cellpadding="5" cellspacing="0" width="100%" border="0">
         <tr>
            <td id="cHeader">
               <asp:Label Runat="Server" CssClass="FORMHEAD" Text="Group Modification" ID="lblHeading"></asp:Label>
            </td>
         </tr>
         <tr>
            <td>
               <asp:Label Runat="Server" ID="lblMessage" CssClass="MSG"></asp:Label>
            </td>
         </tr>
      </table> 
    <!-- Main Table Ends Here -->
      <asp:Table ID="tblMainForm" CellPadding="2" CellSpacing="1" Runat="Server" BorderWidth="0" Width="100%">
										<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False">
			<asp:Label CssClass="LABEL" Runat="Server" Text="Group Name" ID="Label1" NAME="Label1"></asp:Label>&nbsp;
			<asp:Label CssClass="MAND" Runat="Server" Text="*" ID="Label2" NAME="Label2"></asp:Label>
		</asp:TableCell>
											<asp:TableCell Width="70%">
												<asp:TextBox ID="txtGroupName" Runat="Server" CssClass="LARGETEXT" MaxLength="50"></asp:TextBox>
                                                <asp:RegularExpressionValidator
                                            ID="revGroupName" runat="server" ControlToValidate="txtGroupName" Display="None"
                                            ErrorMessage="Group Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Group Description" ID="Label3" NAME="Label3"></asp:Label>&nbsp;
		</asp:TableCell>
											<asp:TableCell ColumnSpan="3">
												<asp:TextBox ID="txtGroupDesc" Runat="Server" CssClass="LARGETEXT" TextMode="MultiLine" Rows="3"
													Columns="10"></asp:TextBox>
                                                  <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtGroupDesc" Display="None"
                                            ErrorMessage="Group Desc Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Select Bank Account(s)" ID="Label4" NAME="Label4"></asp:Label>&nbsp;
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label5" NAME="Label5"></asp:Label>
		</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:CheckBoxList ID="chkBankAccts" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow> 
										<asp:TableRow ID="trEpfAccounts" Visible="false">
											<asp:TableCell>
			<asp:Label Runat="server" CssClass="LABEL" Text="Select EPF Number(s)" ID="Label28"></asp:Label>&nbsp;
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label29" NAME="Label10"></asp:Label>
		</asp:TableCell>
											<asp:TableCell>
												<asp:CheckBoxList ID="chkEpfAccts" Runat="server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trSocAccounts" Visible="false">
											<asp:TableCell>
												<asp:Label Runat="server" CssClass="LABEL" Text="Select SOCSO Number(s)" ID="Label30"></asp:Label>
												<asp:Label Runat="server" CssClass="MAND" Text="*"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:CheckBoxList ID="chkSocAccts" Runat="server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trLHDNAccounts" Visible="false">
											<asp:TableCell>
												<asp:Label Runat="server" CssClass="LABEL" Text="Select LHDN Number(s)" ID="Label33"></asp:Label>
												<asp:label Runat="server" CssClass="MAND" Text="*"></asp:label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:CheckBoxList ID="chkLHDNAccts" Runat="server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<%-- asp:TableRow ID="trPayroll">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Select Payroll File Format(s)" ID="Label6" NAME="Label6"></asp:Label>
												<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label22"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:CheckBoxList ID="chkPayroll" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trBillingFile">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Select Billing File Format(s)" ID="Label38" NAME="Label6"></asp:Label>
												<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label39"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:CheckBoxList ID="chkBilling" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trEPF">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Select EPF File Format(s)" ID="Label7"></asp:Label>
												<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label8"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:CheckBoxList ID="chkEpf" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trSoc">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Select SOCSO File Format(s)" ID="Label23"></asp:Label>
												<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label24"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:CheckBoxList ID="chkSocso" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trLHDN">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Select LHDN File Format(s)" ID="Label34"></asp:Label>
												<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label35"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:CheckBoxList ID="chkLHDN" Runat="Server" CssClass="LABEL" RepeatColumns="2"></asp:CheckBoxList>
											</asp:TableCell>
										</asp:TableRow--%>
										<asp:TableRow ID="trPaymentService">
										    <asp:TableCell>
										        <asp:Label ID="lblPaymentService" Text="Payment Service" runat="server"></asp:Label>
										    </asp:TableCell>
										    <asp:TableCell>
										        <asp:DropDownList ID="ddlPaymentService" runat="server" AutoPostBack="true"></asp:DropDownList>
										    </asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
										    <asp:TableCell ColumnSpan="2">
										        <hr style="width:100%" />
										    </asp:TableCell>
										</asp:TableRow>
										<asp:TableRow Visible="false">
										    <asp:TableCell ColumnSpan="2">
										        <asp:DataGrid Runat="Server" ID="dgAccount" AllowSorting="false" AllowPaging="false" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="1" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True"  AutoGenerateColumns="false" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ShowHeader="True" AlternatingItemStyle-CssClass="GridAltItemStyle" BorderWidth="1">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="BankName" HeaderText="Bank Name" Visible="False"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PaySer_Desc" HeaderText="Payment Service" ItemStyle-Width="245"></asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="Account" ItemStyle-VerticalAlign="Top">
                                                            <ItemTemplate><asp:CheckBoxList ID="chkAccount" Runat="Server" RepeatLayout="Table" CssClass="LABEL" RepeatColumns="1" ></asp:CheckBoxList>
                                                            </ItemTemplate>						
                                                        </asp:TemplateColumn> 
                                                        <asp:TemplateColumn HeaderText="File Format"  ItemStyle-VerticalAlign="Top">
                                                            <ItemTemplate><asp:CheckBoxList ID="chkFormat" Runat="Server" RepeatLayout="Table" CssClass="LABEL" RepeatColumns="1" ></asp:CheckBoxList>
                                                            </ItemTemplate>						
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="BankID" HeaderText="" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PaySer_Id" HeaderText="" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IsMultipleBank" HeaderText="" Visible="False"></asp:BoundColumn>
                                                        
                            
                                                    </Columns> 
                                                </asp:DataGrid> 
										    </asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
										    <asp:TableCell ColumnSpan="2">
										        
										        <asp:DataGrid Runat="Server" ID="dgFormat" AllowSorting="false" AllowPaging="false" PagerStyle-Mode="NumericPages" AlternatingItemStyle-CssClass="GridAltItemStyle" PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="3" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True"  AutoGenerateColumns="false" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ShowHeader="true" BorderWidth="1" BorderColor="black">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="File_Type" HeaderText="Payment Service" ItemStyle-Width="245" ItemStyle-VerticalAlign="Top" ></asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="File Format(s)" ItemStyle-Width="245">
                                                            <ItemTemplate><asp:CheckBoxList ID="chkFormat" Runat="Server" RepeatLayout="Table" CssClass="LABEL" RepeatColumns="1"></asp:CheckBoxList><asp:Label ID="lblFormatError" ForeColor="red" runat="server"></asp:Label>
                                                            </ItemTemplate>						
                                                        </asp:TemplateColumn> 
                                                       <asp:TemplateColumn HeaderText="Statutory Number(s)">
                                                        <ItemTemplate><asp:CheckBoxList ID="chkStatutory" Runat="Server" RepeatLayout="Table" CssClass="LABEL" RepeatColumns="1" ></asp:CheckBoxList><asp:Label ID="lblStatutory" runat="server"></asp:Label>
                                                        </ItemTemplate>	
                                                        </asp:TemplateColumn>
                                                        <asp:BoundColumn DataField="PayStatutory" HeaderText="" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PaySer_Id" HeaderText="" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PaySrvCode" HeaderText="" Visible="false"></asp:BoundColumn>
                                                    </Columns> 
                                                </asp:DataGrid> 
                                                <asp:DataGrid Runat="Server" ID="dgCFormat" AllowSorting="false" AllowPaging="false" PagerStyle-Mode="NumericPages" PagerStyle-HorizontalAlign="Center" GridLines="none" CellPadding="1" CellSpacing="0" Font-Names="Verdana" Font-Size="8pt" width="100%" HeaderStyle-Font-Bold="True"  AutoGenerateColumns="false" CssClass="Grid" HeaderStyle-CssClass="GridHeaderStyle" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ShowHeader="false" AlternatingItemStyle-CssClass="GridAltItemStyle" BorderWidth="1">
                                                    <Columns>
                                                        <asp:BoundColumn DataField="File_Type" HeaderText="File Type" ItemStyle-VerticalAlign="Top" ItemStyle-Width="245"></asp:BoundColumn>
                                                        <asp:TemplateColumn HeaderText="File Format">
                                                            <ItemTemplate><asp:ListBox ID="lsFormat" Runat="Server" Rows="6" CssClass="LABEL" ></asp:ListBox><asp:Label ID="lblFormat" runat="server" CssClass="LABEL"></asp:Label><asp:TextBox ID="txtFormat" runat="server" CssClass="LARGETEXT" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
                                                            </ItemTemplate>
                                                        </asp:TemplateColumn> 
                                                        <asp:BoundColumn DataField="BankName" HeaderText="" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="IsMultipleBank" HeaderText="" Visible="false"></asp:BoundColumn>
                                                        <asp:BoundColumn DataField="PaySer_Id" HeaderText="" Visible="false"></asp:BoundColumn>
                                                    </Columns> 
                                                </asp:DataGrid>
										    </asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
										    <asp:TableCell ColumnSpan="2" VerticalAlign="middle">
										        <hr style="width:100%" />
										    </asp:TableCell>
										</asp:TableRow>
										
                               <asp:TableRow ID="trReviwerNo">
                                <asp:TableCell Runat="Server" ID="Tablecell3" Width="40%">
                                <asp:Label ID="lblNoReviewer" Runat="Server" Text="No of Reviewer(s) for Review" CssClass="LABEL"></asp:Label>&nbsp;
                                <asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label6"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Runat="Server" ID="Tablecell4">
                                <asp:DropDownList CssClass="SMALLTEXT" Runat="Server" ID="ddlNoReviewer"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Runat="Server" ErrorMessage="Select No of Reviewer" ControlToValidate="ddlNoReviewer" Display="None"></asp:RequiredFieldValidator>
                                </asp:TableCell>
                                </asp:TableRow>
                                
                                <asp:TableRow ID="trApproverNo">
                                <asp:TableCell Runat="Server" ID="Tablecell1" Width="40%">
                                <asp:Label ID="lblNoApprover" Runat="Server" Text="No of Approver(s) for Approval" CssClass="LABEL"></asp:Label>&nbsp;
                                <asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Runat="Server" ID="Tablecell2">
                                <asp:DropDownList CssClass="SMALLTEXT" Runat="Server" ID="ddlAuthorizer"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Runat="Server" ErrorMessage="Select No of Approver" ControlToValidate="ddlAuthorizer" Display="None"></asp:RequiredFieldValidator>
                                </asp:TableCell>
                                </asp:TableRow>
										
								<%--		<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False">
			<asp:Label Runat="Server" CssClass="LABEL" Text="No of Approver(s) for Approval" ID="Label9"
													NAME="Label9"></asp:Label>&nbsp;
			<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label10"></asp:Label>
		</asp:TableCell>
											<asp:TableCell Width="60%" ColumnSpan="3">
												<asp:DropDownList ID="ddlAuthorizer" CssClass="MINITEXT" Runat="Server">
													<asp:ListItem Value="0"></asp:ListItem>
													<asp:ListItem Value="1">1</asp:ListItem>
													<asp:ListItem Value="2">2</asp:ListItem>
													<asp:ListItem Value="3">3</asp:ListItem>
													<asp:ListItem Value="4">4</asp:ListItem>
													<asp:ListItem Value="5">5</asp:ListItem>
													<asp:ListItem Value="6">6</asp:ListItem>
													<asp:ListItem Value="7">7</asp:ListItem>
													<asp:ListItem Value="8">8</asp:ListItem>
													<asp:ListItem Value="9">9</asp:ListItem>
												</asp:DropDownList>
											</asp:TableCell>
										</asp:TableRow>--%>
										<asp:TableRow>
											<asp:TableCell Width="30%">
												<asp:Label Runat="Server" Text="Status" CssClass="LABEL" ID="Label11" NAME="Label11"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%" ColumnSpan="3">
												<asp:RadioButtonList ID="rdStatus" Runat="Server" RepeatDirection="Horizontal" CssClass="LABEL">
													<asp:ListItem Value="A" Selected="True">Active</asp:ListItem>
													<asp:ListItem Value="C">Inactive</asp:ListItem>
													<asp:ListItem Value="D">Cancel</asp:ListItem>
												</asp:RadioButtonList>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trReason" runat="server">
											<asp:TableCell Width="30%">
			                                <asp:Label Runat="Server" CssClass="LABEL" Text="Modification Reason" ID="Label25"></asp:Label>
			                                 &nbsp;
                                                <asp:Label Text="*" Runat="server" CssClass="MAND" ID="Label26"></asp:Label>
		                                        </asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtReason" Runat="Server" CssClass="BIGTEXT" TextMode="MultiLine" Rows="3" MaxLength="255"></asp:TextBox>
											<asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtReason" Display="None"
                                            ErrorMessage="Reason Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
                                            </asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trSubmit">
											<asp:TableCell Width="90%" ColumnSpan="4">
			<asp:Button ID="btnSubmit" Text="Submit" CssClass="BUTTON" Runat="Server"></asp:Button>&nbsp;
			<input type="reset" id="btnReset" name="btnReset" value="      Clear     " runat="server"/>&nbsp;
			<input type="button" runat="Server" value="Back To View" onclick="fncBack();" class="BUTTON"/>
		</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trBack" Visible="False">
											<asp:TableCell Width="90%" ColumnSpan="4">
												<input type="button" runat="Server" onclick="fncBack();" value="Back" style="width=100"/>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trAuthCode">
											<asp:TableCell Width="30%">
												<asp:Label Runat="Server" Text="Enter Validation Code" CssClass="LABEL" ID="Label20" NAME="Label20"></asp:Label>
												<asp:Label Runat="Server" CssClass="MAND" Text="*" ID="Label21"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtAuthCode" CssClass="MEDIUMTEXT" MaxLength="14" TextMode="Password" Runat="Server"></asp:TextBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trConfirm">
											<asp:TableCell Width="90%" ColumnSpan="4">
			<asp:Button ID="btnConfirm" Text="Confirm" CssClass="BUTTON" Runat="Server"></asp:Button>&nbsp;
			<input type="button" value="Back" onclick="fncBack();" class="BUTTON" runat="server" id="ipBack" style="width:100"/>&nbsp;
		</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trNew" Visible="False">
											<asp:TableCell Width="90%" ColumnSpan="4">
												<input type="button" runat="Server" id="btnNew" name="btnNew" onclick="fncView();" value="Back to View" />
											</asp:TableCell>
										</asp:TableRow>
									</asp:Table>
									<input type="hidden" id="hBilling" name="hBilling" runat="Server"/>		
<input type="hidden" id="hEpf" name="hEpf" runat="Server"/>								
<input type="hidden" id="hSoc" name="hSoc" runat="Server"/>
<input type="hidden" id="hStatus" name="hStatus" runat="Server"/>
<input type="hidden" id="hPayroll" name="hPayroll" runat="Server"/>
<input type="hidden" id="hLHDN" name="hLHDN" runat="Server"/>
									<asp:ValidationSummary ID="vsCreateGroup" Runat="Server" EnableClientScript="True" HeaderText="Please incorporate the below validations,"
										ShowSummary="False" ShowMessageBox="True"></asp:ValidationSummary>
									<%-- asp:RequiredFieldValidator ID="rfvGroupName" Runat="Server" ControlToValidate="txtGroupName" ErrorMessage="Group Name cannot be blank"
										Display="None"></asp:RequiredFieldValidator>
									<asp:RangeValidator ID="rngGroupAuth" Runat="Server" ControlToValidate="ddlAuthorizer" Type="Integer"
										MinimumValue="1" MaximumValue="9" ErrorMessage="Select No of Approver(s) for Approval" Display="None"></asp:RangeValidator> --%>
									<asp:Table ID="tblConfirm" CellPadding="4" CellSpacing="1" Runat="Server" BorderWidth="0" Width="100%">
										<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False">
			<asp:Label CssClass="LABEL" Runat="Server" Text="Group Name" ID="Label12"></asp:Label>&nbsp;
		</asp:TableCell>
											<asp:TableCell Width="60%" ColumnSpan="3">
												<asp:TextBox ID="txtCGroupName" Runat="Server" CssClass="LARGETEXT" ReadOnly="True"></asp:TextBox>
                                                 <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtCGroupName" Display="None"
                                            ErrorMessage="Group Name Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Group Description" ID="Label13"></asp:Label>&nbsp;
		</asp:TableCell>
											<asp:TableCell Width="60%" ColumnSpan="3">
												<asp:TextBox ID="txtCGroupDesc" Runat="Server" CssClass="LARGETEXT" TextMode="MultiLine" Rows="3"
													Columns="10" ReadOnly="True"></asp:TextBox>
                                                <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtCGroupDesc" Display="None"
                                            ErrorMessage="Group Desc Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="Bank Accounts" ID="Label14"></asp:Label>&nbsp;
		</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCBankAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCEpfAccounts">
											<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
			<asp:Label Runat="Server" CssClass="LABEL" Text="EPF Number(s)" ID="Label31"></asp:Label>&nbsp;
		</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCEpfAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCSocAccounts">
											<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Socso Number(s)" ID="Label32"></asp:Label>&nbsp;
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCSocAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCLHDNAccounts">
											<asp:TableCell Width="30%" Wrap="False" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="LHDN Number(s)" ID="Label36"></asp:Label>&nbsp;
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCLHDNAccts" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCPayroll">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Payroll File Format" ID="Label15"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCPyrFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCBilling">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Billing File Format" ID="Label40"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCBillingFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCEpf">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="EPF File Format" ID="Label16"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCEpfFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCSoc">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="SOCSO File Format" ID="Label17"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCSocFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCLHDN">
											<asp:TableCell Width="20%" VerticalAlign="Top">
												<asp:Label Runat="Server" CssClass="LABEL" Text="LHDN File Format" ID="Label37"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%">
												<asp:ListBox ID="lbxCLHDNFormat" Runat="Server" Rows="6" CssClass="LABEL"></asp:ListBox>
											</asp:TableCell>
										</asp:TableRow>
										
										<asp:TableRow>
											<asp:TableCell Width="30%" Wrap="False">
			<asp:Label Runat="Server" CssClass="LABEL" Text="No of Approver(s) for Approval" ID="Label18"
													NAME="Label18"></asp:Label>&nbsp;
		</asp:TableCell>
											<asp:TableCell Width="60%" ColumnSpan="3">
												<asp:TextBox ID="txtCAuth" Runat="Server" CssClass="MINITEXT" ReadOnly="True"></asp:TextBox>
                                                <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtCAuth" Display="None"
                                            ErrorMessage="Auth Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow>
											<asp:TableCell Width="30%">
												<asp:Label Runat="Server" Text="Status" CssClass="LABEL" ID="Label19"></asp:Label>
											</asp:TableCell>
											<asp:TableCell Width="60%" ColumnSpan="3">
												<asp:TextBox ID="txtStatus" Runat="Server" ReadOnly="True" CssClass="SMALLTEXT"></asp:TextBox>

											</asp:TableCell>
										</asp:TableRow>
										<asp:TableRow ID="trCReason">
											<asp:TableCell Width="30%">
												<asp:Label Runat="Server" CssClass="LABEL" Text="Modification Reason" ID="Label27"></asp:Label>
											</asp:TableCell>
											<asp:TableCell>
												<asp:TextBox ID="txtCReason" Runat="Server" CssClass="BIGTEXT" TextMode="MultiLine" Rows="3"
													MaxLength="255"></asp:TextBox>
                                                <asp:RegularExpressionValidator
                                            ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtCReason" Display="None"
                                            ErrorMessage="Reason Accepts Alpha Numeric, Underscore, Dash Only" ValidationExpression="^[\w\-\s]+$"></asp:RegularExpressionValidator>
											</asp:TableCell>
										</asp:TableRow>
										
									</asp:Table>
        &nbsp;
									<asp:RequiredFieldValidator ID="rfvAuthCode" Runat="Server" ErrorMessage="Validation Code Cannot be blank"
										Display="None" ControlToValidate="txtAuthCode"></asp:RequiredFieldValidator>
									<%-- asp:RequiredFieldValidator ID="rfvReason" Runat="server" ControlToValidate="txtReason" ErrorMessage="Modification Reason cannot be blank"
										Display="None"></asp:RequiredFieldValidator> --%>
									<asp:RegularExpressionValidator ID="rgeAuthCode" Runat="server" ControlToValidate="txtAuthCode" ValidationExpression="[A-Za-z0-9]{8,24}"
										ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters" Display="None"></asp:RegularExpressionValidator>
							<input type="hidden" runat="Server" id="hRequest" name="hRequest"/>
	                        <input type="hidden" runat="Server" id="hIsNew" name="hIsNew"/>		
							<input type="hidden" runat="Server" id="hPageNo" name="hPageNo"/>
		                    <input type="hidden" runat="Server" id="hAuthNo" name="hAuthNo"/>
		                    <input type="hidden" runat="Server" id="hReviewerNo" name="hReviewerNo"/>					
		                    <input type="hidden" runat="Server" id="hGroupName" name="hGroupName"/>					
		                    <input type="hidden" runat="Server" id="hGroupDesc" name="hGroupDesc"/>				
		                    <input type="hidden" runat="Server" id="hPaymentService" name="hPaymentService"/>

</asp:Content>