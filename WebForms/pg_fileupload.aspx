<%@ Page Language="vb" AutoEventWireup="false" Inherits="MaxPayroll.PG_FileUpload"
    CodeFile="PG_FileUpload.aspx.vb" MasterPageFile="~/WebForms/mp_Master.master"
    EnableEventValidation="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script type="text/javascript" src="../include/common.js"></script>

    <script type="text/javaScript">
      function fncBack()
      {
      
        window.history.back();
      }
      function fncNew()
      {
          
        window.location.href = "PG_FileUpload.aspx";
      }
      function fncProgressBar()
      {
         
         Page_InvalidControlToBeFocused = null;
     
         if (typeof(Page_Validators) == "undefined") 
         {
            return true;
         }
         
         var i;
         for (i = 0; i < Page_Validators.length; i++) 
         {
            ValidatorValidate(Page_Validators[i], '', null);
         }
         ValidatorUpdateIsValid();
        // if (Page_IsValid == true)
          //  window.showModelessDialog('progress.aspx','','dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: yes; status: No;scroll:yes;');
       }
       
       
    </script>

    <script type="text/javascript" src="PG_Calendar.js"></script>

    <table cellpadding="5" cellspacing="0" width="100%" border="0">
        <tr>
            <td id="cHeader" width="45%" height="18">
                <asp:Label Text="" CssClass="FORMHEAD" runat="server" ID="lblHeading">&nbsp;File Upload</asp:Label></td>
            <td align="left">
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <img src="../Include/Images/mozilla_blu.gif" />&nbsp;<font color="gray">Loading</font>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </td>
        </tr>
    </table>
    <%-- Main Table Starts Here --%>
    <table width="100%" cellpadding="8" cellspacing="0">
        <tr>
            <td>
                <%--<asp:Label ID="lblMessage" runat="Server" CssClass="MSG"></asp:Label>--%>
               <asp:TextBox TextMode="multiline" runat="server" ID="lblMessage" CssClass="MSG" Rows="3" Visible="false" Width="80%"></asp:TextBox>

                <asp:TextBox TextMode="multiline" runat="server" ID="txtMessage" CssClass="MSG" Rows="3" Visible="false" Width="80%"></asp:TextBox>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="updpnl_Form" runat="server" ChildrenAsTriggers="true" UpdateMode="Always">
        <ContentTemplate>
            <asp:Table Width="100%" runat="server" CellPadding="8" CellSpacing="0" ID="Table1"
                BorderWidth="0" BorderColor="#cccccc">
                <asp:TableRow>
                    <asp:TableCell Width="100%">
                        <%-- Form Table Starts Here --%>
                        <asp:Table Width="100%" runat="Server" CellPadding="3" CellSpacing="2" ID="tblForm">
                            <asp:TableRow>
                                <asp:TableCell ColumnSpan="2" Wrap="False">
                                    <asp:Label ID="Label43" runat="Server" CssClass="LABEL" Text="Please enter the file details:"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trFile">
                                <asp:TableCell Width="20%">
                                    <asp:Label ID="lblFType" runat="Server" CssClass="LABEL" Text="File Type"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:DropDownList ID="ddlFileType" runat="Server" CssClass="MEDIUMTEXT" AutoPostBack="True">
                                    </asp:DropDownList>
                                </asp:TableCell>
                                </asp:TableRow>
                                <%--<asp:TableRow ID="trCPSSubFile">
                                <asp:TableCell Width="20%" >
                                    <asp:Label ID="lblCPSSUb" runat="Server" CssClass="LABEL" Text="CPS Sub File Type"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="50%">
                                    <asp:DropDownList ID="ddlCPSType" runat="Server" CssClass="MEDIUMTEXT" AutoPostBack="True" OnSelectedIndexChanged="Option_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>--%>
                            <asp:TableRow ID="trCFile" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="Label5" runat="Server" CssClass="LABEL" Text="File Type"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtFileType" runat="Server" ReadOnly="True"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trBank" runat="server">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="Server" CssClass="LABEL" Text="Bank Account" ID="Label2" NAME="Label2"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:DropDownList ID="ddlAccount" runat="Server" CssClass="LARGETEXT">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvAccount" runat="Server" ControlToValidate="ddlAccount"
                                        ErrorMessage="Select Bank Account" EnableClientScript="true" Display="None"></asp:RequiredFieldValidator>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCBank" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="Server" CssClass="LABEL" Text="Bank Account" ID="lblBankAccount"
                                        Width="245"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtBankAccount" runat="Server" ReadOnly="True" CssClass="LARGETEXT"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trFormat">
                                <asp:TableCell Width="20%" Wrap="False">
									<asp:Label Runat="Server" CssClass="LABEL" Text="File Format"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:DropDownList ID="ddlFormat" runat="Server" CssClass="LARGETEXT">
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCFormat" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="Server" CssClass="LABEL" Text="File Format" ID="Label7"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtFormat" CssClass="LARGETEXT" runat="Server" ReadOnly="True"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trPay">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label CssClass="LABEL" runat="Server" Text="Payment Date (DD/MM/YYYY)" ID="Label3"
                                        NAME="Label3"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <input type="text" id="txtPayDate" name="txtPayDate" class="SMALLTEXT" runat="server"
                                        onfocus="popUpCalendar(this, document.all('ctl00$cphContent$txtPayDate'), 'dd/mm/yyyy');" />&nbsp;
                                    <a href="#" onclick="popUpCalendar(this, document.all('ctl00$cphContent$txtPayDate'), 'dd/mm/yyyy');">
                                        <img src="../Include/Images/date.gif" border="0"></a>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCPay" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
									<asp:Label CssClass="LABEL" Runat="Server" Text="Payment Date (DD/MM/YYYY)"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtCPayDate" runat="Server" CssClass="SMALLTEXT" MaxLength="10"
                                        ReadOnly="True"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                          <asp:TableRow ID="trEpfAcct" Visible="false">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="server" CssClass="LABEL" Text="" ID="lblService"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:DropDownList CssClass="LARGETEXT" runat="server" ID="ddlEpfAcct">
                                    </asp:DropDownList>
                                </asp:TableCell>
                            </asp:TableRow>
                           <asp:TableRow ID="trCEpfAcct" Visible="false">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="server" CssClass="LABEL" Text="" ID="lblCService"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtEpfAcct" runat="server" CssClass="LARGETEXT" ReadOnly="True"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trUpload">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="lblUpload" runat="Server" CssClass="LABEL" Text="File to be submitted"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <input type="file" id="flUpload" name="flUpload" runat="Server" size="50" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCUpload" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="Label9" runat="Server" CssClass="LABEL" Text="Upload File"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtUploadFile" runat="Server" CssClass="LARGETEXT" ReadOnly="true"></asp:TextBox>
                                   
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trSubFile" Visible="false">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="lblCPS" runat="Server" CssClass="LABEL" Text="Member File to be submitted"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <input type="file" id="flSubFile" name="flUpload" runat="Server" size="50" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCSubfile" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="Label6" runat="Server" CssClass="LABEL" Text="Upload File"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtUploadSubFile" runat="Server" CssClass="LARGETEXT" ReadOnly="true"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            
                            <asp:TableRow ID="trCtrSubFile" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="lblCCPS" runat="Server" CssClass="LABEL" Text="Invoice File"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtSubFile" runat="Server" CssClass="LARGETEXT" ReadOnly="true"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trConMnth" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
									<asp:Label Runat="Server" CssClass="LABEL" Text="Contribution Month/Year (YYYY)"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:DropDownList ID="cmbMonth" CssClass="SMALLTEXT" runat="Server">
                                    </asp:DropDownList>&nbsp;
                                    <asp:TextBox ID="txtYear" CssClass="MINITEXT" runat="Server" MaxLength="4"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCConMnth" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="Server" CssClass="LABEL" Text="Contribution Month/Year (YYYY)"
                                        ID="Label4"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox ID="txtCMonth" runat="Server" CssClass="SMALLTEXT" ReadOnly="True"></asp:TextBox>
                                    &nbsp;
                                    <asp:TextBox ID="txtCYear" CssClass="MINITEXT" runat="Server" ReadOnly="True"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trAddl">
                                <asp:TableCell Width="20%" Wrap="False">
									<asp:Label Runat="Server" CssClass="LABEL" Text="Additional Checks"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%" Wrap="False">
                                    <asp:CheckBox ID="chkDuplicate" CssClass="LABEL" Text="Duplicate Account Numbers"
                                        runat="Server"></asp:CheckBox>&nbsp;
                                    <asp:CheckBox ID="chkIC" CssClass="LABEL" Text="IC Checking" runat="Server"></asp:CheckBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trCAddl" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label runat="Server" CssClass="LABEL" Text="Additional Checks" ID="Label8" NAME="Label8"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%" Wrap="False">
                                    <asp:Label ID="lblAddCheck" runat="Server" CssClass="LABEL"></asp:Label>
                                    &nbsp;
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trEncrypted">
                                <asp:TableCell Width="20%" Wrap="False">
									<asp:Label Runat="Server" CssClass="LABEL" Text="Encrypted File"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%" Wrap="False">
                                    <asp:CheckBox ID="chkEncrypted" CssClass="LABEL" runat="Server"></asp:CheckBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trAuth" Visible="False">
                                <asp:TableCell Width="20%" Wrap="False">
                                    <asp:Label ID="lblAuth" runat="Server" CssClass="LABEL" Text="Enter Validation Code"></asp:Label>
                                </asp:TableCell>
                                <asp:TableCell Width="80%">
                                    <asp:TextBox runat="Server" CssClass="MEDIUMTEXT" TextMode="Password" ID="txtAuthCode"
                                        MaxLength="24"></asp:TextBox>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trSubmit">
                                <asp:TableCell Width="100%" ColumnSpan="2">
                                    <asp:Button runat="Server" ID="btnConfirm" CssClass="BUTTON" Text="Submit" OnClientClick="javascript:fncProgressBar();">
                                    </asp:Button>&nbsp;
                                    <input type="button" class="BUTTON" runat="server" onclick="fncNew();" value="Clear" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trConfirm" Visible="False">
                                <asp:TableCell Width="100%" ColumnSpan="2">
                                    <asp:Button runat="Server" ID="btnSave" CssClass="BUTTON" Text="Upload"></asp:Button>&nbsp;
                                    <input type="button" class="BUTTON" runat="server" onclick="fncNew();" value="Cancel" />
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow ID="trNew" Visible="False">
                                <asp:TableCell Width="100%" ColumnSpan="2">
                                    <input type="button" runat="Server" value="Upload Another File" onclick="fncNew();"
                                        id="btnNew" />
                                </asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell Width="100%">
                        <%-- Payroll File Comments Starts Here --%>
                        <asp:Table Width="100%" CellPadding="2" CellSpacing="1" runat="Server" ID="tblPayroll"
                            Visible="False">
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="MSG" Text="Please read the below instructions before uploading, "
                                        ID="Label1"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="1. File can be uploaded 60 days prior to the Payment Date."
                                        ID="Label13"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="2. If file is in excel format, the sheet name should be 'Sheet1'."
										ID="Label14"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>
							<asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="3. All File Extenstions should be in small letters (.txt,.xls etc)."
										ID="Label15"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="2. An alert message will be displayed, if a file with same payment date, same total amount or same content has been uploaded previously."
                                        ID="Label16"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="3. If the file is uploaded on the Payment Date, it should be before the cutoff time."
                                        ID="Label17"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="4. IC (New IC No, Old IC No, Passport) should not contain 'Hyphen'."
                                        ID="Label10"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="7. 'For Excel Format' the first column is mandatory." ID="Label34"></asp:Label>
								</asp:TableCell>
							</asp:TableRow> --%>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP"
										ID="lblGuidance"></asp:Label><br /> <br />
									<asp:Label Runat="Server" CssClass="HELP" Text="Operation Hours: <BR>Monday - Friday : 8:45AM to 5:45PM"
										ID="Label33"></asp:Label>
								</asp:TableCell>
							</asp:TableRow> --%>
                        </asp:Table>
                        <%-- Payroll File Comments Ends Here --%>
                        <%-- EPF File Comments Starts Here --%>
                        <asp:Table Width="100%" CellPadding="2" CellSpacing="1" runat="Server" ID="tblEPF"
                            Visible="False">
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="MSG" Text="Please read the below instructions before uploading, "
                                        ID="Label28"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="1. File can be uploaded 60 days prior to the Payment Date."
										ID="Label18"></asp:Label>
								</asp:TableCell>
							</asp:TableRow> --%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="1. If file is in excel format, the sheet name should be 'Sheet1'."
                                        ID="Label19"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="2. All File Extenstions should be in small letters (.txt,.xls etc)."
                                        ID="Label20"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="3.  An alert message will be displayed, if a file with same payment date, same total amount or same content has been uploaded previously."
                                        ID="Label21"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%--<asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. If the file is uploaded on the Payment Date, it should be before the cutoff time."
										ID="Label22"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="4. EPF cut-off time is 6.00 pm daily. File(s) submitted after 6.00 pm will be processed on next working day."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. Please ensure that your EPF File has no decimal value in the Contribution Amount."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="6. For your foreign workers who contribute EPF, please key in their passport number as  000000000000 in the IC column."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="7. Contribution for Current or Future month and year is allowed. Previous contribution month / year is not allowed."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="8. 'For Excel Format' the first column is mandatory."
                                        ID="Label41"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%--<asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP"
										ID="Label30">Note: For further guidance, please contact <%=MaxPayroll.mdConstant.gc_Const_CompanyName%> Registration Center at <%=MaxPayroll.mdConstant.gc_Const_CompanyContactNo%> during operation hours.<BR> <BR></asp:Label>
									<asp:Label Runat="Server" CssClass="HELP" Text="Operation Hours: <BR>Monday - Friday : 8:45AM to 5:45PM"
										ID="Label31"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                        </asp:Table>
                        <%-- EPF File Comments Ends Here --%>
                        <%--SOCSO File Comments Starts Here --%>
                        <asp:Table Width="100%" CellPadding="2" CellSpacing="1" runat="Server" ID="tblSocso"
                            Visible="False">
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="MSG" Text="Please read the below instructions before uploading, "
                                        ID="Label29"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="1. File can be uploaded 60 days prior to the Payment Date."
										ID="Label23"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="1. If file is in excel format, the sheet name should be 'Sheet1'."
                                        ID="Label24"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="2. All File Extenstions should be in small letters (.txt,.xls etc)."
                                        ID="Label25"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="3. An alert message will be displayed, if a file with same payment date, same total amount or same content has been uploaded previously."
                                        ID="Label26"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. If the file is uploaded on the Payment Date, it should be before the cutoff time."
										ID="Label27"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="4. SOCSO cut-off time is 6.00 pm daily. File(s) submitted after 6.00 pm will be processed on next working day."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. Contribution for Previous and Current Month / Year is allowed. Future month contribution is not allowed."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="6. IC no for all employees is mandatory."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="7. 'For Excel Format' the first column is mandatory."
                                        ID="Label42"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%--<asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP"
										ID="Label11">Note: For further guidance, please contact <%=MaxPayroll.mdConstant.gc_Const_CompanyName%> Registration Center at <%=MaxPayroll.mdConstant.gc_Const_CompanyContactNo%> during operation hours.<BR> <BR></asp:Label>
									<asp:Label Runat="Server" CssClass="HELP" Text="Operation Hours: <BR>Monday - Friday : 8:45AM to 5:45PM"
										ID="Label12"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                        </asp:Table>
                        <%-- SOCSO File Comments Ends Here --%>
                        <%-- LHDN File Comments Starts Here --%>
                        <asp:Table Width="100%" CellPadding="2" CellSpacing="1" runat="Server" ID="tblLHDN"
                            Visible="False">
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="MSG" Text="Please read the below instructions before uploading, "
                                        ID="Label35"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="1. Please use file format as instructed by LHDN. For more info on the file conversion please visit www.hasilnet.org.my"
                                        ID="Label36"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="2. Please choose the account according to the region: Semenanjung, Sabah or Sarawak."
                                        ID="Label37"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="3. The cutoff time is at 6.00 p.m. File(s) submitted after 6.00 pm will be processed on next working day."
                                        ID="Label38"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%--<asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="4. If the file is uploaded on the Payment Date, it should be before the cutoff time."
										ID="Label39"></asp:Label>
								</asp:TableCell>
							</asp:TableRow> --%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
                                    <asp:Label runat="Server" CssClass="HELP" Text="4. Future payment is allowed for current year + 3 years."
                                        ID="Label40"></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. For backdated payment before 1995, please liaise with LHDN."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="6. An alert message will be displayed, if a file with same payment date, same total amount or same content has been uploaded previously."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="7. 'For Excel Format' the first column is mandatory."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%--<asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" 
										ID="Label44">Note: For further guidance, please contact <%=MaxPayroll.mdConstant.gc_Const_CompanyName%> Registration Center at <%=MaxPayroll.mdConstant.gc_Const_CompanyContactNo%> during operation hours.<br/> <br/></asp:Label>
									<asp:Label Runat="Server" CssClass="HELP" Text="Operation Hours: <br/>Monday - Friday : 8:45AM to 5:45PM"
										ID="Label45"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                        </asp:Table>
                        <%--LHDN File Comments Ends Here --%>
                        <%--ZAKAT File Comments Starts Here --%>
                        <asp:Table Width="100%" CellPadding="2" CellSpacing="1" runat="Server" ID="tblZAKAT"
                            Visible="False">
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="MSG" Text="Please read the below instructions before uploading, "
									></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="1. File can be uploaded 60 days prior to the Payment Date."
										ID="Label23"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="1. If file is in excel format, the sheet name should be 'Sheet1'."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="2. All File Extenstions should be in small letters (.txt,.xls etc)."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="3. An alert message will be displayed, if a file with same payment date, same total amount or same content has been uploaded previously."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. If the file is uploaded on the Payment Date, it should be before the cutoff time."
										ID="Label27"></asp:Label>
								</asp:TableCell>
							</asp:TableRow>--%>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="4. ZAKAT cut-off time is 6.00 pm daily. File(s) submitted after 6.00 pm will be processed on next working day."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="5. IC no for all employees is mandatory."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <asp:TableRow>
                                <asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP" Text="6. 'For Excel Format' the first column is mandatory."></asp:Label>
                                </asp:TableCell>
                            </asp:TableRow>
                            <%-- <asp:TableRow>
								<asp:TableCell Width="100%">
									<asp:Label Runat="Server" CssClass="HELP"
										>Note: For further guidance, please contact <%=MaxPayroll.mdConstant.gc_Const_CompanyName%> Registration Center at <%=MaxPayroll.mdConstant.gc_Const_CompanyContactNo%> during operation hours.<BR> <BR></asp:Label>
									<asp:Label Runat="Server" CssClass="HELP" Text="Operation Hours: <BR>Monday - Friday : 8:45AM to 5:45PM"
										></asp:Label>
								</asp:TableCell>
							</asp:TableRow> --%>
                        </asp:Table>
                        <%-- ZAKAT File Comments Ends Here --%>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnConfirm" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
    <%-- Main Table Ends Here --%>
    <%-- Validation Controls Starts Here --%>
    <asp:RequiredFieldValidator ID="rfvUpload" runat="Server" ControlToValidate="ddlFileType"
        ErrorMessage="Select File Type" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvEpfAcct" runat="Server" ControlToValidate="ddlEpfAcct"
        ErrorMessage="Select EPF Account" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvFormat" runat="Server" ControlToValidate="ddlFormat"
        ErrorMessage="Select File Format" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvPayDate" runat="Server" ControlToValidate="txtPayDate"
        ErrorMessage="Select Payment Date" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvFile" runat="Server" ControlToValidate="flUpload"
        ErrorMessage="Select Upload File" Display="None"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rngMonth" runat="server" ControlToValidate="cmbMonth" ErrorMessage="Select Contribution Month"
        Display="None" Type="Integer" MinimumValue="1" MaximumValue="12"></asp:RangeValidator>
    <asp:RequiredFieldValidator ID="rfvYear" runat="Server" ControlToValidate="txtYear"
        ErrorMessage="Contribution Year cannot be blank" Display="None"></asp:RequiredFieldValidator>
    <asp:RequiredFieldValidator ID="rfvAuth" runat="Server" ControlToValidate="txtAuthCode"
        ErrorMessage="Enter Validation Code" Display="None"></asp:RequiredFieldValidator>
    <asp:RangeValidator ID="rngPayDate" runat="server" ControlToValidate="txtPayDate"
        ErrorMessage="Invalid Payment Date" Type="Date" Display="None"></asp:RangeValidator>
    <asp:RangeValidator ID="rngYear" runat="server" ControlToValidate="txtYear" ErrorMessage="Invalid contribution Year"
        Display="None" Type="Integer"></asp:RangeValidator>
    <asp:RegularExpressionValidator ID="rgeAuthCode" runat="server" ControlToValidate="txtAuthCode"
        ValidationExpression="[A-Za-z0-9]{8,24}" ErrorMessage="Validation Code must be 8 to 24 Alpha Numeric Characters"
        Display="None"></asp:RegularExpressionValidator>
    <asp:RegularExpressionValidator ID="rgeYear" runat="Server" ControlToValidate="txtYear"
        ValidationExpression="^\d{4}" ErrorMessage="Year Format Should be YYYY" Display="None"></asp:RegularExpressionValidator>
    <asp:ValidationSummary runat="Server" ID="vsUpload" EnableClientScript="true" ShowMessageBox="true"
        ShowSummary="false" HeaderText="Please Incorporate the below Validations,"></asp:ValidationSummary>
    <%-- Validation Controls Starts Here --%>
    <%--Hidden Boxes Starts here --%>
    <input type="hidden" id="hMonth" name="hMonth" runat="Server" />
    <input type="hidden" id="hICMatch" name="hICMatch" runat="Server" />
    <input type="hidden" id="hDuplicate" name="hDuplicate" runat="Server" />
    <input type="hidden" id="hFormat" name="hFormat" runat="Server" />
    <input type="hidden" id="hAccount" name="hAccount" runat="Server" />
    <input type="hidden" id="hFileName" name="hFileName" runat="Server" />
    <input type="hidden" id="hAlert" name="hAlert" runat="Server" />
    <input type="hidden" id="hEncrypted" name="hEncrypted" runat="Server" />
    <input type="hidden" id="hSubFile" runat="Server" />
    
        <input type="hidden" id="MainFilePath" runat="Server" />
            <input type="hidden" id="SubFilePath" runat="Server" />


    <%--Hidden Boxes Ends Here --%>
</asp:Content>
