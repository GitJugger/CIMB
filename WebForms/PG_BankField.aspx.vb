Option Strict Off
Option Explicit On 

Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxMiddleware
Imports MaxGeneric


Namespace MaxPayroll


Partial Class PG_BankField
        Inherits clsBasePage
        Dim clsCommon As New MaxPayroll.clsCommon
#Region "Request.QueryString"
        Private ReadOnly Property rq_strFType() As String
            Get
                Return Request.QueryString("FType") & ""
            End Get
        End Property
        Private ReadOnly Property rq_strType() As String
            Get
                Return Request.QueryString("Type") & ""
            End Get
        End Property
        Private ReadOnly Property rq_intFieldID() As Int16
            Get
                If IsNumeric(Request.QueryString("ID")) Then
                    Return CInt(Request.QueryString("ID"))
                Else
                    Return 0
                End If
            End Get
        End Property

        Private ReadOnly Property rq_ServiceId() As Short
            Get
                If IsNumeric(Request.QueryString("ServiceId")) Then
                    Return CInt(Request.QueryString("ServiceId"))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_intBankID() As Integer
            Get
                If IsNothing(Request.QueryString("BankID")) OrElse Not IsNumeric(Request.QueryString("BankID")) Then
                    Return 0
                Else
                    Return CInt(Request.QueryString("BankID"))
                End If
            End Get
        End Property
        Private ReadOnly Property rq_strBankDesc() As String
            Get
                Return Trim(Request.QueryString("BankDesc") & "")
            End Get
        End Property
#End Region

#Region "BindDropdownlist"

      Private Sub BindMatchFld()

         cmbMatchFld.Items.Add(New ListItem("Select", enmMatchField.None.ToString))
         cmbMatchFld.Items.Add(New ListItem("Branch Code", enmMatchField.Org_BrCode.ToString))
         cmbMatchFld.Items.Add(New ListItem("State Code", enmMatchField.State.ToString))
         cmbMatchFld.Items.Add(New ListItem("Bank Code", enmMatchField.BNM_Code.ToString))
         cmbMatchFld.Items.Add(New ListItem("Bank Organization Code", enmMatchField.BankOrg_Code.ToString))
         cmbMatchFld.Items.Add(New ListItem("Organization Code", enmMatchField.Org_Code.ToString))
         cmbMatchFld.Items.Add(New ListItem("Organisation Name", enmMatchField.Org_Name.ToString))
         cmbMatchFld.Items.Add(New ListItem("Account Number", enmMatchField.Ac_Number.ToString))
            'cmbMatchFld.Items.Add(New ListItem("IC Number Checking", enmMatchField.Ic_Number.ToString))
         cmbMatchFld.Items.Add(New ListItem("Transaction Charge", enmMatchField.Chrg_PerTran.ToString))
         cmbMatchFld.Items.Add(New ListItem("EPF Transaction Charges", enmMatchField.Chrg_perTran_Epf.ToString))
         cmbMatchFld.Items.Add(New ListItem("Registration Number", enmMatchField.Reg_Number.ToString))
         cmbMatchFld.Items.Add(New ListItem("EPF Number", enmMatchField.Epf_Number.ToString))
         cmbMatchFld.Items.Add(New ListItem("STD Number", enmMatchField.Tax_Number.ToString))
            cmbMatchFld.Items.Add(New ListItem("SOCSO Number", enmMatchField.Soc_Number.ToString))
            '071008 1 line modified by Marcus.
            'Purpose: To change "EPF Officer" to "EPF/LHDN Officer" as the same matched field
            'is to be used in [Contact Person] of LHDN output file. 
            cmbMatchFld.Items.Add(New ListItem("EPF/LHDN Officer", enmMatchField.Contact_Person.ToString))
         cmbMatchFld.Items.Add(New ListItem("EPF Payment Type", enmMatchField.Status.ToString))
         cmbMatchFld.Items.Add(New ListItem("Contact Number", enmMatchField.Contact_Number.ToString))
         cmbMatchFld.Items.Add(New ListItem("CIMB Batch No.", enmMatchField.CIMB_Batch_No.ToString))
            cmbMatchFld.Items.Add(New ListItem("CIMB Reference No.", enmMatchField.CIMB_Reference_No.ToString))
            '071008 1 line Added by Marcus.
            'Purpose: To be included in LHDN Header.
            cmbMatchFld.Items.Add(New ListItem("Email Address", enmMatchField.Email_Address.ToString))
            cmbMatchFld.Items.Add(New ListItem("Zakat Employer Name", enmMatchField.Zakat_Employer_Name.ToString))
            '<asp:ListItem Value="None">Select</asp:ListItem>
         '<asp:ListItem Value="Org_BrCode">Branch Code</asp:ListItem>
         '<asp:ListItem Value="State">State Code</asp:ListItem>
         '<asp:ListItem Value="Org_Code">Organisation Code</asp:ListItem>
         '<asp:ListItem Value="Org_Name">Organisation Name</asp:ListItem>
         '<asp:ListItem Value="Ac_Number">Account Number</asp:ListItem>
         '<asp:ListItem Value="Ic_Number">IC Number Checking</asp:ListItem>
         '<asp:ListItem Value="Chrg_PerTran">Transaction Charge</asp:ListItem>
         '<asp:ListItem Value="Chrg_PerTran_Epf">EPF Transaction Charges</asp:ListItem>
         '<asp:ListItem Value="Reg_Number">Registration Number</asp:ListItem>
         '<asp:ListItem Value="Epf_Number">EPF Number</asp:ListItem>
         '<asp:ListItem Value="Tax_Number">STD Number</asp:ListItem>
         '<asp:ListItem Value="Soc_Number">SOCSO Number</asp:ListItem>
         '<asp:ListItem Value="Contact_Person">EPF Officer</asp:ListItem>
         '<asp:ListItem Value="EPF Status">EPF Payment Type</asp:ListItem>
         '<asp:ListItem Value="Contact_Number">Contact Number</asp:ListItem>
      End Sub

        Private Sub BindOptions()

            cmbOptions.Items.Add(New ListItem("Select", "NA"))
            cmbOptions.Items.Add(New ListItem("Autopay Email Address", "AE"))
            cmbOptions.Items.Add(New ListItem("Customer Credit Bank Code", "BC"))
            cmbOptions.Items.Add(New ListItem("Customer Debit Bank Code", "BD"))
            cmbOptions.Items.Add(New ListItem("Debit Account Number", "DA"))
            cmbOptions.Items.Add(New ListItem("Credit Account Number", "AN"))
            cmbOptions.Items.Add(New ListItem("BNM Code", "MC"))
            cmbOptions.Items.Add(New ListItem("Bank Organisation Code", "BO"))
            cmbOptions.Items.Add(New ListItem("Validate Bank Organisation Code", "VB"))

            cmbOptions.Items.Add(New ListItem("Batch Number", "BN"))
            cmbOptions.Items.Add(New ListItem("File Batch Number", "BT"))
            cmbOptions.Items.Add(New ListItem("Batch Number", "BN"))
            cmbOptions.Items.Add(New ListItem("Validate Batch Number", "VN"))
            cmbOptions.Items.Add(New ListItem("Batch Header Reference", "BH"))
            cmbOptions.Items.Add(New ListItem("Total Batch Header", "TB"))
            cmbOptions.Items.Add(New ListItem("Total Batch Records", "TR"))
            cmbOptions.Items.Add(New ListItem("Total File Lines", "TL"))
            cmbOptions.Items.Add(New ListItem("IC Number Checking", "IC"))
            cmbOptions.Items.Add(New ListItem("Passport Number", "PN"))
            cmbOptions.Items.Add(New ListItem("Reference Number", "RN"))
            cmbOptions.Items.Add(New ListItem("Debit Reference Number", "DR"))
            cmbOptions.Items.Add(New ListItem("Payee ID", "PI"))
            cmbOptions.Items.Add(New ListItem("Payee Name", "PE"))
            cmbOptions.Items.Add(New ListItem("Pay Amount", "AM"))
            cmbOptions.Items.Add(New ListItem("Total Amount", "TM"))
            cmbOptions.Items.Add(New ListItem("Transaction Charge", "TC"))
            cmbOptions.Items.Add(New ListItem("Payment Date", "VD"))
            cmbOptions.Items.Add(New ListItem("Payment Date(DDMMYYYY) - With assignment", "ND"))
            cmbOptions.Items.Add(New ListItem("Validate Payment Date(DDMMYYYY) - With assignment", "VP"))

            cmbOptions.Items.Add(New ListItem("Date Format(DDMMYYYY) - Checking only", "DD"))
            cmbOptions.Items.Add(New ListItem("Date (DDMMYYYY)", "DY"))
            cmbOptions.Items.Add(New ListItem("Time Format(HHMMSS)", "CT"))
            cmbOptions.Items.Add(New ListItem("Created Date", "CD"))
            cmbOptions.Items.Add(New ListItem("Address (Mandatory)", "AD"))
            cmbOptions.Items.Add(New ListItem("Address (Not mandatory)", "NM"))
            cmbOptions.Items.Add(New ListItem("Postcode", "PC"))
            cmbOptions.Items.Add(New ListItem("Record Count", "RC"))
            cmbOptions.Items.Add(New ListItem("File Name", "FN"))
            cmbOptions.Items.Add(New ListItem("Hash Total", "HT"))
            cmbOptions.Items.Add(New ListItem("Contribution Month(YYMM)", "CM"))
            cmbOptions.Items.Add(New ListItem("Contribution Month(MMYY)", "Y2"))
            cmbOptions.Items.Add(New ListItem("Contribution Month(MMYYYY)", "YM"))

            cmbOptions.Items.Add(New ListItem("Contribution Month(YYYYMM)", "MY"))
            cmbOptions.Items.Add(New ListItem("Deduction Year(YYYY)", "LY"))
            cmbOptions.Items.Add(New ListItem("Deduction Month(MM)", "LM"))
            'cmbOptions.Items.Add(New ListItem("EPF First Indicator", "FI"))
            cmbOptions.Items.Add(New ListItem("EPF Second Indicator", "SI"))
            cmbOptions.Items.Add(New ListItem("EPF New or Supp. Indicator", "NS"))
            cmbOptions.Items.Add(New ListItem("EPF Sequence Number", "ES"))
            cmbOptions.Items.Add(New ListItem("EPF File Sequence Number", "FS"))
            cmbOptions.Items.Add(New ListItem("EPF Transaction Date(YYYYMMDD)", "TD"))
            cmbOptions.Items.Add(New ListItem("EPF Transaction Time(HHMMSS)", "TT"))
            cmbOptions.Items.Add(New ListItem("EPF File Reference Number", "FR"))
            cmbOptions.Items.Add(New ListItem("EPF Testing Mode", "ET"))

            cmbOptions.Items.Add(New ListItem("Member Number", "MN"))
            cmbOptions.Items.Add(New ListItem("Employee Wages", "EW"))
            cmbOptions.Items.Add(New ListItem("EPF/SOCSO/LHDN/ZAKAT Employer No", "EN"))
            cmbOptions.Items.Add(New ListItem("Employee Name", "EM"))
            cmbOptions.Items.Add(New ListItem("EPF Payment Type", "PT"))
            cmbOptions.Items.Add(New ListItem("EPF Testing Mode Indicator", "ET"))

            cmbOptions.Items.Add(New ListItem("EPF Employer Amount", "RA"))
            cmbOptions.Items.Add(New ListItem("PCB Amount", "PA"))
            cmbOptions.Items.Add(New ListItem("CP38 Amount", "8A"))
            cmbOptions.Items.Add(New ListItem("PCB Total Amount", "TP"))
            cmbOptions.Items.Add(New ListItem("CP38 Total Amount", "8T"))
            cmbOptions.Items.Add(New ListItem("Total PCB Records", "PR"))
            cmbOptions.Items.Add(New ListItem("Total CP38 Records", "8R"))
            cmbOptions.Items.Add(New ListItem("Income Tax File No", "IT"))
            cmbOptions.Items.Add(New ListItem("LHDN Wife Code", "WC"))
            cmbOptions.Items.Add(New ListItem("LHDN Country Code", "CC"))

            cmbOptions.Items.Add(New ListItem("EPF Employee Amount", "EA"))
            cmbOptions.Items.Add(New ListItem("Total EPF Employer Amount", "ER"))
            cmbOptions.Items.Add(New ListItem("Total EPF Employee Amount", "EE"))
            cmbOptions.Items.Add(New ListItem("Employer No.(Validate)", "EU"))
            cmbOptions.Items.Add(New ListItem("Contribution Month-MMYY(Validate)", "CH"))
            cmbOptions.Items.Add(New ListItem("State Code", "SC"))
            cmbOptions.Items.Add(New ListItem("Fillers", "FL"))
            cmbOptions.Items.Add(New ListItem(enmMatchField.Unique_Reference.ToString(), "UR"))
            ''CPS Start
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Cheque_No.ToString(), "CQ"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Compare_Field.ToString(), "CF"))

            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Gross_Amount.ToString(), "GS"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Net_Amount.ToString(), "NT"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Total_Net.ToString(), "TN"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Tax.ToString(), "TX"))

            'cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Body_Header.ToString(), "HB"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Body_Header_Identifier.ToString(), "HB"))

            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Body_Body.ToString(), "BB"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Body_Footer.ToString(), "BF"))

            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Header_Identifier.ToString(), "HI"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Body_Identifier.ToString(), "BI"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Footer_Identifier.ToString(), "FI"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Trailer_Indicator.ToString(), "TI"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.CPS_Cheque_Col_Name_CN.ToString(), "CN"))
            cmbOptions.Items.Add(New ListItem(Helper.enmCPSOptions.Null_Field_NF.ToString(), "NF"))
            'cmbOptions.Items.Add(New ListItem("Employer Number(Validate)", "EB"))
            'cmbOptions.Items.Add(New ListItem("Contribution Month-MMYYYY(Validate)", "CH"))
            'cmbOptions.Items.Add(New ListItem("Contribution Month-MM(Validate)", "CO"))
            'cmbOptions.Items.Add(New ListItem("Contribution Year-YYYY(Validate)", "CA"))
            'cmbOptions.Items.Add(New ListItem("State Code(Validate & save)", "SE"))
            'cmbOptions.Items.Add(New ListItem("Contact Person(Save)", "CN"))
            'cmbOptions.Items.Add(New ListItem("Contact Number(Save)", "CR"))
            'cmbOptions.Items.Add(New ListItem("File Indicator(Validate & save)", "IR"))
            'cmbOptions.Items.Add(New ListItem("File Sequence Number(Save)", "SR"))
            'cmbOptions.Items.Add(New ListItem("File Reference Number(Save)", "RR"))
            'cmbOptions.Items.Add(New ListItem("EPF Testing Mode(Validate & save)", "PE"))

            'cmbOptions.Items.Add(New ListItem("Record Count(Validate)", "RT"))
            'cmbOptions.Items.Add(New ListItem("Total EPF Employer Amount(Validate)", "AT"))
            'cmbOptions.Items.Add(New ListItem("Total EPF Employee Amount(Validate)", "OT"))
            'cmbOptions.Items.Add(New ListItem("Hash Total(Validate)", "HL"))



        End Sub

#End Region

#Region "Page Load"


        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load Functions
        'Arguments      : Object, EventArgs
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/10/2003
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            If Not Page.IsPostBack Then
                Me.txtBankDesc.Text = rq_strBankDesc
                '071018 1 line added by Marcus
                'Purpose: To hide bank info when there is a default Bank Code set up in web.config
                clsCommon.fncShowHideBankCodeControls(Me.txtBankDesc, Me.lblBankCode)
                Call PPS.EnumToDropDown(GetType(EnumHelp.FileFormat), __ddlFileFormat, True)
                Call PPS.EnumToDropDown(GetType(PPS.Delimiter), __ddlFileDelimiter, True)
                BindOptions()
                BindMatchFld()
                'BindBody(body)
            End If

            'Create Data Row Object
            Dim drBank As System.Data.DataRow
            'Create Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank
            'Create Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic
            'Create System Data Set
            Dim dsBank As New System.Data.DataSet

            Try

                'Check If Session is Live
                If Not clsGeneric.fnSession() Then
                    Response.Clear()
                    Response.Redirect(gc_LogoutPath, False)
                    Exit Try
                End If

                If Not ss_strUserType.Equals(gc_UT_BankUser) Then
                    Response.Clear()
                    Response.Redirect(gc_LogoutPath, False)
                    Exit Try
                End If


                trMsg.Visible = False
                btnDelete.Visible = False

                'Get File Id 
                hFieldId.Value = rq_intFieldID
                If Page.IsPostBack = False Then
                    'If Update Fill Data - Start
                    If rq_intFieldID > 0 Then

                        btnDelete.Visible = True
                        txtFileType.Text = rq_strFType

                        'Get Bank Format Details
                        dsBank = clsBank.fnBankDetails(rq_intFieldID)
                        If dsBank.Tables("BANK").Rows.Count > 0 Then
                            For Each drBank In dsBank.Tables("BANK").Rows
                                txtFldDesc.Text = drBank("FDesc")
                                cmbFldType.SelectedValue = drBank("CType")
                                cmbMatchFld.SelectedValue = drBank("FMatch")
                                cmbDataType.SelectedValue = drBank("DType")
                                cmbOptions.SelectedValue = drBank("Options")
                                If drBank("ColPos") <> 0 Then
                                    __ddlFileFormat.SelectedValue = EnumHelp.FileFormat.Delimited
                                    trColPos.Visible = True
                                    txtColPos.Text = drBank("ColPos")
                                    trStartPos.Visible = False
                                    trEndPos.Visible = False
                                End If
                                txtStartPos.Text = drBank("SPos")
                                txtEndPos.Text = drBank("EPos")
                                txtDefault.Text = drBank("Dvalue")
                                If CBool(drBank("IsMandatory")) Then
                                    Me.rblIsMandatory.SelectedValue = 1
                                Else
                                    Me.rblIsMandatory.SelectedValue = 0
                                End If
                                If CBool(drBank("IsCustomerSetting")) Then
                                    Me.rblIsCustomerSetting.SelectedValue = 1
                                Else
                                    Me.rblIsCustomerSetting.SelectedValue = 0
                                End If

                            Next
                        End If

                        'Display Alert Message
                        btnDelete.Attributes("onclick") = "javascript:return confirm('Are you sure you want to delete Bank Account " & Replace(txtFldDesc.Text, "'", "\'") & "?')"
                    End If
                    'If Update Fill Data - Stop

                End If
                
                'Get File Type
                If txtFileType.Text = "" Then
                    txtFileType.Text = rq_strType
                End If

            Catch

                'Log Error
                clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page Load - PG_BankField", Err.Number, Err.Description)

            Finally

                drBank = Nothing
                clsBank = Nothing
                dsBank = Nothing
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Save"

        '****************************************************************************************************
        'Procedure Name : prSave()
        'Purpose        : Save Page Contents
        'Arguments      : Object, EventArgs
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/10/2003
        '*****************************************************************************************************
        Private Sub prSave(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click

            'Create Instance Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Declare Variables
            Dim strFileType As String
            Dim strResult As String
            Dim IsDuplicate As Boolean
            Dim strAction As String = ""
            Dim strOptions As String
            Dim strMatchField As String
            Dim strDefault As String
            Dim strDataType As String
            Dim strDescription As String
            Dim strFieldType As String
            Dim bMandatory As Boolean
            Dim bCustomerSetting As Boolean
            Dim intStartPos As Int16
            Dim intEndPos As Int16
            Dim intColPos As Int16
            Dim strMsg As String = ""
            Dim strBody As String = ""
            Try

                IsDuplicate = False

                If rq_intFieldID > 0 Then
                    strAction = "UPDATE"
                ElseIf rq_intFieldID = 0 Then
                    strAction = "ADD"
                End If
                If IsNumeric(txtStartPos.Text) Then
                    intStartPos = CInt(Request.Form("ctl00$cphContent$txtStartPos"))
                Else
                    strMsg += "Start Position accepts numeric only." & gc_BR
                End If
                If IsNumeric(txtEndPos.Text) Then
                    intEndPos = CInt(Request.Form("ctl00$cphContent$txtEndPos"))
                Else
                    strMsg += "End Position accepts numeric only." & gc_BR
                End If
                If IsNumeric(rblIsMandatory.SelectedValue) Then
                    bMandatory = CBool(rblIsMandatory.SelectedValue)
                End If
                If IsNumeric(rblIsCustomerSetting.SelectedValue) Then
                    bCustomerSetting = CBool(rblIsCustomerSetting.SelectedValue)
                End If

                strOptions = CStr(cmbOptions.SelectedValue)
                strDefault = CStr(txtDefault.Text)
                strFieldType = CStr(cmbFldType.SelectedValue)
                strDataType = CStr(cmbDataType.SelectedValue)
                strFileType = CStr(txtFileType.Text)
                strMatchField = CStr(cmbMatchFld.SelectedValue)
                strDescription = CStr(txtFldDesc.Text)
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtFldDesc.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                'Check If Content Type is Header, Then Either Matching Field Or Predefined Options Or Default Value Must Be Given - Start
                If strMatchField = "None" And strOptions = "None" And strDefault = "" Then
                    If strFieldType = "H" Then
                        strMsg += "If Content Type is Header, Please Select Matching Field Or Predefined Options or Enter Default Value." & gc_BR
                    ElseIf strFieldType = "F" Then
                        strMsg += "If Content Type is Footer, Please Select Matching Field Or Predefined Options or Enter Default Value." & gc_BR
                    End If
                End If
                'Check If Content Type is Header, Then Either Matching Field Or Predefined Options Or Default Value Must Be Given - Stop

                'Check If End Position is Not Less Than Start Position - Start
                If Int(intEndPos) < Int(intStartPos) Then
                    strMsg += "End Position Cannot Be Less Than Start Date." & gc_BR
                End If
                'Check If End Position is Not Less Than Start Position - Stop

                'Check For Duplicate Entries - Start
                If clsBank.fnDuplicate(rq_intBankID, "Description", strDescription, strFieldType, 0, 0, strAction, rq_intFieldID, strFileType) Then
                    strMsg += "Field Description Cannot be Duplicated." & gc_BR
                End If

                If __ddlFileFormat.SelectedValue = EnumHelp.FileFormat.Text Then


                    'Check If Start postion And Same Content Type Has Not Been Assigned
                    If clsBank.fnDuplicate(rq_intBankID, "Start Position", "None", strFieldType, intStartPos, 0, strAction, rq_intFieldID, strFileType) Then
                        strMsg += "Start Position Has Already Been Assigned. Please Enter Different Start Position" & gc_BR
                    End If

                    'Check If Start postion is Not Between Other Start & End Position With the Same Content Type
                    If clsBank.fnDuplicate(rq_intBankID, "Start Between", "None", strFieldType, intStartPos, 0, strAction, rq_intFieldID, strFileType) Then
                        strMsg += "Start Position Falls Between Other Start and End Position. Please Enter Different Start Position" & gc_BR
                    End If

                    'Check If Start postion And Same Content Type Has Not Been Assigned
                    If clsBank.fnDuplicate(rq_intBankID, "End Position", "None", strFieldType, 0, intEndPos, strAction, rq_intFieldID, strFileType) Then
                        strMsg += "End Position Has Already Been Assigned. Please Enter Different End Position" & gc_BR
                    End If

                    'Check If Start postion is Not Between Other Start & End Position With the Same Content Type
                    If clsBank.fnDuplicate(rq_intBankID, "End Between", "None", strFieldType, 0, intEndPos, strAction, rq_intFieldID, strFileType) Then
                        strMsg += "End Position Falls Between Other Start and End Position. Please Enter Different End Position" & gc_BR
                    End If
                End If

                If strMatchField <> "None" AndAlso rblIsCustomerSetting.SelectedValue = 1 Then
                    strMsg += "This field has been setted with Matching Field, therefore it shouldn't be displayed at Customer File Settings." & gc_BR
                End If

                'Check For Duplicate Entries - Stop

                ''Check If Content Type is Header, Then Either Matching Field Or Predefined Options Or Default Value Must Be Given - Start
                'If Not IsDuplicate Then
                '    If strFieldType = "H" And strMatchField = "None" And strOptions = "None" And strDefault = "" Then
                '        trMsg.Visible = True
                '        IsDuplicate = True
                '        lblMessage.Text = "If Content Type is Header, Please Select Matching Field Or Predefined Options or Enter Default Value."
                '    End If
                'End If
                ''Check If Content Type is Header, Then Either Matching Field Or Predefined Options Or Default Value Must Be Given - Stop

                ''Check If Content Type is Footer, Then Either Matching Field Or Predefined Options Or Default Value Must Be Given - Start
                'If Not IsDuplicate Then
                '    If strFieldType = "F" And strMatchField = "None" And strOptions = "None" And strDefault = "" Then
                '        trMsg.Visible = True
                '        IsDuplicate = True
                '        lblMessage.Text = "If Content Type is Footer, Please Select Matching Field Or Predefined Options or Enter Default Value."
                '    End If
                'End If
                ''Check If Content Type is Footer, Then Either Matching Field Or Predefined Options Or Default Value Must Be Given - Stop

                ''Check If End Position is Not Less Than Start Position - Start
                'If Not IsDuplicate Then
                '    If Int(intEndPos) < Int(intStartPos) Then
                '        trMsg.Visible = True
                '        IsDuplicate = True
                '        lblMessage.Text = "End Position Cannot Be Less Than Start Date."
                '    End If
                'End If
                ''Check If End Position is Not Less Than Start Position - Stop

                ''Check For Duplicate Entries - Start
                'If Not IsDuplicate Then
                '    IsDuplicate = clsBank.fnDuplicate(rq_intBankID, "Description", strDescription, strFieldType, 0, 0, strAction, rq_intFieldID, strFileType)
                '    If IsDuplicate Then
                '        trMsg.Visible = True
                '        IsDuplicate = True
                '        lblMessage.Text = "Field Description Cannot be Duplicated."
                '    End If
                'End If
                ''Check If Start postion And Same Content Type Has Not Been Assigned
                'If Not IsDuplicate Then
                '    IsDuplicate = clsBank.fnDuplicate(rq_intBankID, "Start Position", "None", strFieldType, intStartPos, 0, strAction, rq_intFieldID, strFileType)
                '    If IsDuplicate Then
                '        trMsg.Visible = True
                '        lblMessage.Text = "Start Position Has Already Been Assigned. Please Enter Different Start Position"
                '    End If
                'End If
                ''Check If Start postion is Not Between Other Start & End Position With the Same Content Type
                'If Not IsDuplicate Then
                '    IsDuplicate = clsBank.fnDuplicate(rq_intBankID, "Start Between", "None", strFieldType, intStartPos, 0, strAction, rq_intFieldID, strFileType)
                '    If IsDuplicate Then
                '        trMsg.Visible = True
                '        lblMessage.Text = "Start Position Falls Between Other Start and End Position. Please Enter Different Start Position"
                '    End If
                'End If
                ''Check If Start postion And Same Content Type Has Not Been Assigned
                'If Not IsDuplicate Then
                '    IsDuplicate = clsBank.fnDuplicate(rq_intBankID, "End Position", "None", strFieldType, 0, intEndPos, strAction, rq_intFieldID, strFileType)
                '    If IsDuplicate Then
                '        trMsg.Visible = True
                '        lblMessage.Text = "End Position Has Already Been Assigned. Please Enter Different End Position"
                '    End If
                'End If
                ''Check If Start postion is Not Between Other Start & End Position With the Same Content Type
                'If Not IsDuplicate Then
                '    IsDuplicate = clsBank.fnDuplicate(rq_intBankID, "End Between", "None", strFieldType, 0, intEndPos, strAction, rq_intFieldID, strFileType)
                '    If IsDuplicate Then
                '        trMsg.Visible = True
                '        lblMessage.Text = "End Position Falls Between Other Start and End Position. Please Enter Different End Position"
                '    End If
                'End If
                ''Check For Duplicate Entries - Stop

                If Len(strMsg) = 0 Then
                    'Execute Insert/Update Function

                    If rq_intFieldID = 0 Then

                        strResult = clsBank.fnDB_BankFormat("ADD", rq_intBankID, rq_ServiceId)

                        If strResult = gc_Status_OK Then
                            txtFldDesc.Text = ""
                            txtStartPos.Text = ""
                            txtEndPos.Text = ""
                            txtColPos.Text = ""
                            txtDefault.Text = ""
                            cmbFldType.SelectedIndex = 0
                            cmbDataType.SelectedIndex = 0
                            cmbMatchFld.SelectedIndex = 0
                            cmbOptions.SelectedIndex = 0
                            trMsg.Visible = True
                            lblMessage.Text = "Bank Field Creation Successful"
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Bank File Field Created", "Y")
                        Else
                            trMsg.Visible = True
                            lblMessage.Text = "Bank Field Creation Failed"
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Bank File Field Created", "N")
                        End If

                    ElseIf rq_intFieldID > 0 Then

                        strResult = clsBank.fnDB_BankFormat("Update", rq_intBankID, rq_ServiceId)

                        txtFldDesc.Text = strDescription
                        txtStartPos.Text = intStartPos
                        txtEndPos.Text = intEndPos
                        txtColPos.Text = intColPos ''Added CPS P3
                        txtDefault.Text = strDefault
                        cmbFldType.SelectedValue = strFieldType
                        cmbDataType.SelectedValue = strDataType
                        cmbMatchFld.SelectedValue = strMatchField
                        cmbOptions.SelectedValue = strOptions
                        trMsg.Visible = True

                        If strResult = gc_Status_OK Then
                            lblMessage.Text = "Bank Field Updation Successful"
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Bank File Field Updation", "Y")
                        Else
                            lblMessage.Text = "Bank Field Updation Failed"
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Bank File Field Updation", "N")
                        End If

                    End If
                Else
                    trMsg.Visible = True
                    lblMessage.Text = strMsg
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Save - PG_BankField", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

            End Try

        End Sub

#End Region

#Region "Back TO View"

      Private Sub prView(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnBack.Click

         Dim strFileType As String

         Try

            strFileType = txtFileType.Text
            Response.Clear()
            'Response.Redirect("PG_BankFormat.aspx?Type=" & strFileType, False)
            '
            Response.Redirect("PG_BankFormat.aspx?PageNo=" & Request.QueryString("PageNo") & "&BankID=" & Request.QueryString("BankID") & "&Type=" & strFileType, False)
         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "Delete Field"

        Private Sub prDelete(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnDelete.Click

            'Create Bank Class 
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Variable Declarations
            Dim strResult As String

            Try

                strResult = clsBank.fnDB_BankFormat("DELETE", rq_intBankID, rq_ServiceId)

                If strResult = gc_Status_OK Then
                    txtFldDesc.Text = ""
                    txtStartPos.Text = ""
                    txtEndPos.Text = ""
                    txtDefault.Text = ""
                    cmbFldType.SelectedIndex = 0
                    cmbDataType.SelectedIndex = 0
                    cmbMatchFld.SelectedIndex = 0
                    trMsg.Visible = True
                    lblMessage.Text = "Bank Field Deletion Successful"
                    Call clsUsers.prcDetailLog(ss_lngUserID, "Bank File Field Deletion", "Y")
                Else
                    trMsg.Visible = True
                    lblMessage.Text = "Bank Field Deletion Failed"
                    Call clsUsers.prcDetailLog(ss_lngUserID, "Bank File Field Deletion", "N")
                End If

            Catch

            Finally

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

            End Try

        End Sub

#End Region

        Protected Sub __ddlFileFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles __ddlFileFormat.SelectedIndexChanged
            If Me.__ddlFileFormat.SelectedValue = EnumHelp.FileFormat.Delimited Then
                trdelimiter.Visible = True
                trStartPos.Visible = False
                trEndPos.Visible = False
                trColPos.Visible = True

            Else
                trdelimiter.Visible = False
                trStartPos.Visible = True
                trEndPos.Visible = True
                trColPos.Visible = False
            End If
        End Sub
    End Class

End Namespace

