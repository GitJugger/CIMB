Imports System.IO
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsUpload
Imports System.Data.SqlClient
Imports MaxMiddleware



Namespace MaxPayroll

    Partial Class PG_FileUpload
        Inherits clsBasePage
        Private _Helper As New Helper
        Private _Encryption As New clsEncryption


        Private ReadOnly Property bIsMultiBank() As Boolean
            Get
                Dim oPaySer As New clsPaymentService
                Return oPaySer.fncIsMultipleBank(ddlFileType.SelectedValue)
            End Get
        End Property

#Region "Global Declaration"
        Dim filePath As String = Nothing
        Dim filename As String = Nothing
        Dim ServerPath As String = Nothing
        Dim FolderDir As String = Nothing

        Dim fileSubPath As String = Nothing
        Dim Subfilename As String = Nothing
        Dim SubServerPath As String = Nothing


#End Region

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Valuee  : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of System Data Row
            Dim drUpload As System.Data.DataRow

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Upload Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of System Data Set
            Dim dsUpload As New System.Data.DataSet

            'Create Instance of Customer Class Object
            Dim clsCustomer As New clsCustomer

            'Variable Declarations
            Dim strAuthLock As String
            Dim strSocso As String
            Dim intReviewers As Int16, intAuthorisers As Int16
            Dim strEPF As String, strPayroll As String, IsICCheck As Boolean
            Dim strLHDN As String

            'If Len(lblMessage.Text) > 0 Then
            '    Page.RegisterStartupScript("Alert", "<script type='javascript'>alert('" & Me.lblMessage.Text & "');</script>")
            '    Me.lblMessage.Text = ""
            'End If

            Try


                'contribution year minimum value
                rngYear.MinimumValue = (Today.Year - 1)
                'contribution year maximum value
                rngYear.MaximumValue = Today.Year
                'Payment Date Minimum Value
                rngPayDate.MinimumValue = Today
                'Payment Date Maximum Value
                rngPayDate.MaximumValue = DateAdd(DateInterval.Day, 60, Today)

                If Not ss_strUserType = gc_UT_Uploader Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                trEpfAcct.Visible = False
                trCEpfAcct.Visible = False

                '4 lines commented by Marcus.
                'Reason: It seems these variable unusable.
                'strEPF = clsCommon.fncBuildContent("EPF", "EPF File", ss_lngOrgID, ss_lngUserID)                  'EPF Service Status
                'strSocso = clsCommon.fncBuildContent("SOCSO", "SOCSO File", ss_lngOrgID, ss_lngUserID)            'SOCSO Service Status
                'strPayroll = clsCommon.fncBuildContent("PAYROLL", "Payroll File", ss_lngOrgID, ss_lngUserID)      'Payroll Service Status
                'strLHDN = clsCommon.fncBuildContent("LHDN", "LHDN File", ss_lngOrgID, ss_lngUserID)               'LHDN Service Status

                'Get Authorization Lock Status - Start
                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    btnConfirm.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid 3 attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Get Authorization Lock Status - Stop

                'Check for Active Reviewers/Authorisers - START
                intReviewers = clsCommon.fncBusinessRules("REVIEW", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, 0)
                intAuthorisers = clsCommon.fncBusinessRules("AUTHORIZE", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, 0)

                If intReviewers = 0 Or intAuthorisers = 0 Then
                    btnConfirm.Enabled = False
                    lblMessage.Text = "Required Number(s) of " & gc_UT_ReviewerDesc & "s/" & gc_UT_AuthDesc & "s are not Created or Active. Please contact your " & gc_UT_SysAdminDesc & "."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Check for Active Reviewers/Authorisers - STOP

                If Not Page.IsPostBack Then
                    'btnConfirm.Attributes.Add("onclick", "javascript:fncProgressBar();")
                    'BindBody(body)
                    hAlert.Value = "N"
                    trAddl.Visible = False
                    trConMnth.Visible = False

                    Dim strEncKey As String = clsCommon.fncGetOrgCcryptKey(ss_lngOrgID, ss_lngOrgID)
                    If strEncKey = "" Or strEncKey = Nothing Then
                        Me.chkEncrypted.Enabled = False
                    End If

                    'lblGuidance.Text = fncBindText("Note: For further guidance, please contact ") & gc_Const_CompanyName & fncBindText(" Registration Center at ") & gc_Const_CompanyContactNo & fncBindText(" during operation hours.")

                    'Check if IC Number checking is Needed or Not - START
                    IsICCheck = IIf(clsCommon.fncBuildContent("IC_Number", "", ss_lngOrgID, ss_lngUserID) = "Y", True, False)
                    chkIC.Checked = IsICCheck
                    'Check if IC Number checking is Needed or Not - STOP

                    'Get File Types - START

                    ddlFileType.DataSource = clsCustomer.fncGetGroupPaymentService(ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                    ddlFileType.DataTextField = "FTYPE"
                    ddlFileType.DataValueField = "FTYPE"
                    ddlFileType.DataBind()

                    'clsCustomer.fncBindDDLGroupPaymentService(ddlFileType, ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                    If ddlFileType.Items.Count = 1 Then
                        ddlFileType.SelectedIndex = 0

                        prcFileTypeChanged()
                        BindDDlFormat()
                        'Me.trFile.Visible = False

                    Else
                        ddlFileType.Items.Insert(0, New ListItem("Select", ""))
                    End If

                    'Get File Types - STOP

                    If Request.QueryString("PTYPE") <> "" Then
                        ddlFileType.SelectedValue = Request.QueryString("PTYPE")
                        prcFileTypeChanged()
                        BindDDlFormat()
                    End If


                End If
                If ddlFileType.SelectedValue = "LHDN File" Then
                    'contribution year minimum value
                    rngYear.MinimumValue = 1995
                    'contribution year maximum value
                    rngYear.MaximumValue = (Today.Year + 3)
                    'Payment Date Minimum Value
                    rngPayDate.MinimumValue = Today
                    'Payment Date Maximum Value
                    rngPayDate.MaximumValue = DateAdd(DateInterval.Year, 3, Today)
                End If

                prcDDLEnabling()

                Call clsCommon.fncUploadBtnDisable(btnSave, True)

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "PG_FileUpload - Page_Load", Err.Number, Err.Description)

            Finally
                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Upload Class Object
                clsCommon = Nothing

                'Destroy Instance of System Data Set
                dsUpload = Nothing

                'Destroy Instance of System Data Row
                drUpload = Nothing

            End Try

        End Sub

#End Region

        Private Sub prcDDLEnabling()
            If Me.ddlFormat.Items.Count = 0 Then
                ddlFormat.Enabled = False
            Else
                ddlFormat.Enabled = True
            End If
            If Me.ddlAccount.Items.Count = 0 Then
                ddlAccount.Enabled = False
            Else
                ddlAccount.Enabled = True
            End If
        End Sub

#Region "Save"


        Public Shared Sub OpenProgressBar(ByVal Page As System.Web.UI.Page)
            Dim sbScript As New StringBuilder()

            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("<!--" + ControlChars.Lf)
            sbScript.Append("window.showModalDialog('./ProgressBar/Progress.aspx','','dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: No; status: No;scroll:No;');" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            Page.RegisterClientScriptBlock("OpenProgressBar", sbScript.ToString())
        End Sub

        '****************************************************************************************************
        'Function Name  : prUpload
        'Purpose        : Upload File
        'Arguments      : 
        'Return Value   : Message
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Private Sub prcUpload(ByVal Sender As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click
            'Create Instance of Data Row
            Dim drServiceAccts As DataRow

            'Create Instance of Data Set
            Dim dsServiceAccts As New DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance Upload Class Object
            Dim clsUpload As New MaxPayroll.clsUpload

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim intSerAccId As Int16, intState As Int16
            Dim strFileType As String, lngOrgId As Long, lngUserId As Long, IsAuthCode As Boolean
            Dim intValueMonth As Int16, IsIC As Boolean, IsDuplicate As Boolean, intValueYear As Int16, strAccNo As String
            Dim strUserName As String, strUserRole As String, strSubject As String, strBody As String, intConYear As Int16
            Dim strDbAuthCode As String, intConMonth As Int16, intAttempts As Int16, intGroupId As Int32
            Dim lngFormatId As Long, intValueDay As Int16, strValidation As String, strFileName As String, strSubFileName As String
            Dim IsAlert As Boolean, lngAccId As Long, dcTranCharge As Decimal, strTestStatus As String = "", strSerAccNo As String = "", bContributeMonth As Boolean



            Try

                IsAlert = False
                lblMessage.Text = ""
                strAccNo = hAccount.Value                                                           'Get Account Number

                ''Change for CPS - Hafeez
                strFileType = txtFileType.Text
                If strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name _
                    Or strFileType = _Helper.CPSDelimited_Dividen_Name Or _
                        strFileType = _Helper.CPSSingleFileFormat_Name Or strFileType = _Helper.AutopaySNA_Name Then
                    strFileName = txtUploadFile.Text
                    'Get File Name
                    If strFileType = _Helper.CPSMember_Name Then
                        strSubFileName = txtUploadSubFile.Text
                    End If

                Else
                    strFileName = hFileName.Value                                                       'Get File Name
                    strSubFileName = hSubFile.Value
                End If

                ''End


                'Get File Type
                IsIC = IIf(hICMatch.Value = "Y", True, False)                                       'IC Checking Option
                IsDuplicate = IIf(hDuplicate.Value = "Y", True, False)                              'Duplicate Account Checking
                intConYear = IIf(IsNumeric(txtYear.Text), txtYear.Text, 0)                          'EPF Contribution Year
                intConMonth = IIf(IsNumeric(hMonth.Value), hMonth.Value, 0)                         'EPF Contribution Month
                lngFormatId = IIf(IsNumeric(hFormat.Value), hFormat.Value, 0)                   'File Format Id

                'If (strFileType = "EPF File" Or strFileType = "SOCSO File" Or strFileType = "LHDN File" Or strFileType = "Zakat") Then
                '    bContributeMonth = True
                'Else
                bContributeMonth = False
                'End If

                If bIsMultiBank = False Then
                    lngAccId = IIf(IsNumeric(clsCommon.fncBreakValue(ddlAccount.SelectedValue, " - ", False)), CLng(clsCommon.fncBreakValue(ddlAccount.SelectedValue, " - ", False)), 0)    'Account Id
                End If

                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)            'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)         'Get User Id
                intGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)      'Get Group Id

                'Ic checking on oldic, newic and passpost is blank
                'If strFileType = "LHDN File" Then
                '    IsIC = True
                'End If

                'Check Session Value for Authorization Lock - Start
                If Not IsNumeric(Session("AUTH_LOCK")) Or Session("AUTH_LOCK") = 0 Then
                    Session("AUTH_LOCK") = 0
                End If
                'Check Session Value for Authorization Lock - Stop

                'Check If AuthCode is Valid - Start
                strDbAuthCode = clsCommon.fncPassAuth(lngUserId, "A", lngOrgId)
                IsAuthCode = IIf(strDbAuthCode = txtAuthCode.Text, True, False)
                'Check If AuthCode is Valid - Stop

                'Check for invalid Authorization Code Attempts - START
                If Not IsAuthCode Then
                    intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                    If Not intAttempts = 2 Then
                        If Not IsAuthCode Then
                            'Increment Attempts
                            intAttempts = intAttempts + 1
                            'Assign Current Attempt to Session
                            Session("AUTH_LOCK") = intAttempts
                            'Display Message
                            lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                            lblMessage.Visible = True
                            Exit Try
                        End If
                    ElseIf intAttempts = 2 Then
                        If Not IsAuthCode Then
                            'Disbable Button
                            btnSave.Enabled = False
                            'Get User Role
                            strUserRole = clsCommon.fncBuildContent("User Role", "", lngUserId, lngUserId)
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("User Name", "", lngUserId, lngUserId)
                            'Mail Subject
                            strSubject = strUserName & " (" & strUserRole & ") Locked/Inactive."
                            'Mail Body
                            strBody = strUserName & " (" & strUserRole & ")" & " has been Locked/Inactive on " & Now() & " due to Invalid Validation Code attempts. Please change the User Validation Code."
                            'Lock Authorization Code
                            Call clsUsers.prcAuthLock(lngOrgId, lngUserId, "A")
                            'Send Mail
                            Call clsCommon.prcSendMails("USER LOCK", lngOrgId, lngUserId, intGroupId, strSubject, strBody)
                            'Track Auth Lock
                            Call clsUsers.prcLockHistory(lngUserId, "A")
                            'Display Message
                            lblMessage.Text = "Your account has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                            lblMessage.Visible = True
                            Exit Try
                        End If
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP

                'Set Pay Date - START
                intValueDay = Day(txtCPayDate.Text)     'Get Pay Day
                intValueYear = Year(txtCPayDate.Text)   'Get Pay Year
                intValueMonth = Month(txtCPayDate.Text) 'Get Pay Month
                'Set Pay Date - STOP

                'Get Service Accounts Details - Start
                'If (strFileType = "EPF File" Or strFileType = "SOCSO File" Or strFileType = "LHDN File" Or strFileType = "Zakat") Then
                '    intSerAccId = ddlEpfAcct.SelectedValue
                '    'Populate Data Set
                '    dsServiceAccts = clsCommon.fncGetRequested("SERVICE ACCTS", ddlEpfAcct.SelectedValue, lngUserId, 0, ddlFileType.SelectedValue, "")
                '    For Each drServiceAccts In dsServiceAccts.Tables("UPLOAD").Rows
                '        intState = drServiceAccts("SCODE")              'Get State Code
                '        strSerAccNo = drServiceAccts("SRVNO")           'Get Service Account No 
                '        dcTranCharge = drServiceAccts("TCHARGE")        'Get Transaction Charge
                '        strTestStatus = drServiceAccts("SRVTEST")       'Get Service Account Test Status
                '    Next
                'Else
                'Get Payroll Transaction Charge
                dcTranCharge = MaxGeneric.clsGeneric.NullToDecimal(clsCommon.fncBuildContent("TRAN CHARGE", ddlFileType.SelectedValue, lngOrgId, lngUserId))
                'End If
                'Get Service Accounts Details - Stop

                If strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name Or _
                    strFileType = _Helper.CPSDelimited_Dividen_Name Or strFileType = _Helper.CPSSingleFileFormat_Name Or strFileType = _Helper.AutopaySNA_Name Then
                    strFileName = MainFilePath.Value
                    If strFileType = _Helper.CPSMember_Name Then
                        strSubFileName = SubFilePath.Value
                    End If

                End If

                If Me.chkEncrypted.Checked Then
                    strFileName = strFileName.Remove(strFileName.LastIndexOf("."), Len(_Encryption.EncryptFileExtension))
                    If strSubFileName <> "" Then
                        strSubFileName = strSubFileName.Remove(strFileName.LastIndexOf("."), Len(_Encryption.EncryptFileExtension))

                    End If
                End If

                'Validate File - Start
                strValidation = clsUpload.fncValidateFile(strFileName, strSubFileName, lngFormatId, intValueDay, intValueMonth, _
                    intValueYear, strAccNo, IsIC, IsDuplicate, lngOrgId, lngUserId, strFileType, _
                        Request.ServerVariables("REMOTE_ADDR"), intConMonth, intConYear, intGroupId, _
                            lngAccId, IsAlert, hAlert.Value, dcTranCharge, bContributeMonth, intState, _
                                strTestStatus, strSerAccNo, intSerAccId)

                If IsAlert And Not strValidation = "" Then
                    hAlert.Value = "Y"
                    lblMessage.Text = strValidation
                    lblMessage.Visible = True
                    'txtMessage.Visible = True
                    'txtMessage.Text = strValidation
                ElseIf Not IsAlert And strValidation = gc_Status_OK Then
                    trNew.Visible = True
                    trAuth.Visible = False
                    trSubmit.Visible = False
                    trConfirm.Visible = False
                    lblMessage.Text = "File Uploaded Successfully"
                    lblMessage.Visible = True
                    'If System.IO.File.Exists(strFileName) Then
                    'System.IO.File.Delete(strFileName)
                    'End If
                ElseIf Not IsAlert And Not strValidation = "" Then
                    trNew.Visible = True
                    trAuth.Visible = False
                    trSubmit.Visible = False
                    trConfirm.Visible = False
                    btnNew.Value = "Upload Again"
                    lblMessage.Text = strValidation
                    lblMessage.Visible = True
                    'txtMessage.Visible = True
                    'txtMessage.Text = strValidation

                End If

            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prUpload - PG_FileUpload", Err.Number, Err.Description)
                lblMessage.Text = "strvalidation" & Err.Description
                lblMessage.Visible = True
            Finally

                'Destroy Instance of Data Row
                drServiceAccts = Nothing

                'Destroy Instance of System Data Set
                dsServiceAccts = Nothing

                'Destroy Generic class Object
                clsGeneric = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Instance of Upload Class Object
                clsUpload = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try
            fncKillPopOut()
        End Sub

#End Region

#Region "File Type"


        '****************************************************************************************************
        'Function Name  : prcFileType
        'Purpose        : Function to be excuted when File type Changed
        'Arguments      : 
        'Return Value   : Message
        'Author         : Sujith Sharatchandran - 
        'Created        : 10/03/2005
        '*****************************************************************************************************
        Private Sub ddlFileType_SelectedIndexChanged(ByVal O As System.Object, ByVal E As System.EventArgs) Handles ddlFileType.SelectedIndexChanged
            'System.Threading.Thread.Sleep(3000)



            prcFileTypeChanged()
            BindDDlFormat()

        End Sub

        Private Sub prcFileTypeChanged()
            'Create Instance of Data Row
            Dim drUpload As System.Data.DataRow

            'Create Instance of System Data Set
            Dim dsUpload As New System.Data.DataSet

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strFileType As String
            Try

                strFileType = ddlFileType.SelectedValue

                ddlFormat.Items.Clear()
                ''Populate File Format based on File Type - Start
                'ddlFormat.Items.Clear()
                'ddlFormat.Items.Add(New ListItem("Select", ""))
                'dsUpload = clsCommon.fncGetRequested("File Format", lngOrgId, lngUserId, lngGroupId, strFileType)
                'If dsUpload.Tables("UPLOAD").Rows.Count > 0 Then
                '   For Each drUpload In dsUpload.Tables("UPLOAD").Rows
                '      lngFormatId = drUpload("FID")
                '      strFormatName = drUpload("FNAME")
                '      ddlFormat.Items.Add(New ListItem(strFormatName, lngFormatId))
                '   Next
                'End If
                'Populate File Format based on File Type - Stop


                ''Populate EPF Accounts if EPF File - Start
                'If (strFileType = "EPF File" Or strFileType = "SOCSO File" Or strFileType = "LHDN File" Or strFileType = "Zakat") Then
                '    'Populate Data Set
                '    dsUpload = clsCommon.fncGetRequested("EPF ACCTS", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, Left(strFileType, 1))
                '    ddlEpfAcct.Items.Clear()
                '    ddlEpfAcct.Items.Add(New ListItem("Select", ""))
                '    If dsUpload.Tables("UPLOAD").Rows.Count > 0 Then
                '        For Each drUpload In dsUpload.Tables("UPLOAD").Rows
                '            ddlEpfAcct.Items.Add(New ListItem(drUpload("SRVNAME"), drUpload("SRVID")))
                '        Next
                '    End If
                '    If strFileType = "EPF File" Then
                '        lblService.Text = "Select EPF Number"
                '        lblCService.Text = "EPF Number"
                '        tblEPF.Visible = True
                '        tblSocso.Visible = False
                '        tblPayroll.Visible = False
                '        Me.trEpfAcct.Visible = True
                '        tblLHDN.Visible = False
                '        tblZAKAT.Visible = False
                '    ElseIf strFileType = "SOCSO File" Then
                '        lblService.Text = "Select SOCSO Number"
                '        lblCService.Text = "SOCSO Number"
                '        tblEPF.Visible = False
                '        tblSocso.Visible = True
                '        tblPayroll.Visible = False
                '        Me.trEpfAcct.Visible = False
                '        tblLHDN.Visible = False
                '        tblZAKAT.Visible = False
                '    ElseIf strFileType = "LHDN File" Then
                '        lblService.Text = "Select LHDN Number"
                '        lblCService.Text = "LHDN Number"
                '        tblEPF.Visible = False
                '        tblSocso.Visible = False
                '        tblPayroll.Visible = False
                '        tblLHDN.Visible = True
                '        Me.trEpfAcct.Visible = False
                '        tblZAKAT.Visible = False
                '    ElseIf strFileType = "Zakat" Then
                '        lblService.Text = "Select ZAKAT Reference"
                '        lblCService.Text = "ZAKAT Reference"
                '        tblEPF.Visible = False
                '        tblSocso.Visible = False
                '        tblPayroll.Visible = False
                '        tblLHDN.Visible = False
                '        Me.trEpfAcct.Visible = False
                '        tblZAKAT.Visible = True
                '    End If

                '    Me.txtPayDate.Value = Now.ToShortDateString
                '    Me.trPay.Visible = False

                '    '071128 Condition added by Marcus
                '    'Purpose: To hide Bank Account Dropdown and bind File Format dropdown under Multiple Bank Payment Service

                'Else
                If ddlFileType.SelectedValue = _Helper.CPSMember_Name Then
                    trSubFile.Visible = True


                Else
                    If ddlFileType.SelectedValue = _Helper.CPS_Name Then
                        'Response.Redirect("pg_CPSFileUpload.aspx", True)
                        Me.trSubFile.Visible = True
                        lblUpload.Text = "Payment File to be submitted"
                    Else
                        trSubFile.Visible = False
                        lblUpload.Text = "File to be submitted"
                    End If


                End If

                tblEPF.Visible = False
                tblSocso.Visible = False
                tblPayroll.Visible = True
                tblLHDN.Visible = False
                tblZAKAT.Visible = False

                Me.txtPayDate.Value = ""
                Me.trPay.Visible = True
                Me.trEpfAcct.Visible = False

                'End If
                'Populate EPF Accounts if EPF File - Stop


                'Get BankAccounts - START
                If bIsMultiBank Then
                    Me.ddlAccount.Items.Clear()
                    trBank.Visible = False

                    'Populate File Format based on File Type - Start
                    ddlFormat.Items.Clear()
                    Try
                        ddlFormat.DataSource = clsCommon.fncGetRequested("File Format", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, ddlFileType.SelectedValue, "", 0)
                        ddlFormat.DataTextField = "FNAME"
                        ddlFormat.DataValueField = "FID"
                        ddlFormat.DataBind()
                        ddlFormat.Items.Insert(0, New ListItem("Select", ""))

                        trAddl.Visible = False
                        trConMnth.Visible = False
                        trEpfAcct.Visible = False
                        tblEPF.Visible = False
                        tblSocso.Visible = False
                        tblLHDN.Visible = False
                        tblPayroll.Visible = True

                        prcDDLEnabling()

                    Catch ex As Exception
                        Me.lblMessage.Text = "Error in loading the File Format"
                        lblMessage.Visible = True
                        Me.LogError("ddlFileType.SelectedIndexChanged")
                    End Try
                Else
                    trBank.Visible = True
                    ddlAccount.Items.Clear()
                    dsUpload = clsCommon.fncGetRequested("Bank Accts", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, Me.ddlFileType.SelectedValue)
                    If dsUpload.Tables("UPLOAD").Rows.Count > 0 Then
                        For Each drUpload In dsUpload.Tables("UPLOAD").Rows
                            'lngAccId = drUpload("ACCID")
                            ddlAccount.Items.Add(New ListItem(CStr(drUpload("BACCName") & "") & " (" & CStr(drUpload("Account_No") & "") & ")", CStr(drUpload("BACCID") & "")))
                        Next
                    End If
                    ddlAccount.DataSource = clsCommon.fncGetRequested("Bank Accts", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, "")
                    ddlAccount.Items.Insert(0, New ListItem("Select", ""))

                End If
                'Get BankAccounts - STOP
                prcDDLEnabling()
            Catch ex As Exception

            Finally

                'Destroy Instance of System Data Row
                drUpload = Nothing

                'Destroy Instance of Data Set
                dsUpload = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try
        End Sub

#End Region

#Region "Page Confirm"

        '****************************************************************************************************
        'Procedure Name : Page_Confirm()
        'Purpose        : Page Confirm
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 10/03/2005
        '*****************************************************************************************************
        Private Sub Page_Confirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim lngGroupId As Long, strExtn As String, strFileType As String
            Dim intYear As Int16, strTime As String = "", strOption As String = "", lngFormatId As Long
            Dim strFileCheck As String, strFileName As String, strSubFileName As String = "", strFilePrefix As String, strPath As String, intCheck As Int16
            Dim lngOrgId As Long, lngUserId As Long, IsCutoff As Boolean, intDay As Int16, intMonth As Int16
            Dim strEPF As String, strEPF1() As String
            Dim bIsMultipleBank As Boolean = bIsMultiBank

            Try

                
                intDay = Day(txtPayDate.Value)                                                  'Get Value Day
                intYear = Year(txtPayDate.Value)                                                'Get Value Year
                intMonth = Month(txtPayDate.Value)                                              'Get Value Month
                lngFormatId = ddlFormat.SelectedValue                                           'Get File Format
                strFileName = flUpload.PostedFile.FileName                                      'Get File Name
                strFileType = ddlFileType.SelectedValue                                     'Get File Type
                If strFileType = _Helper.CPSMember_Name Or strFileType = _Helper.CPS_Name Then

                    strSubFileName = flSubFile.PostedFile.FileName

                End If
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)  'Get Group Id

                'Get File Type Option - START
                'Marcus : then naming of Payroll File product to be changed to Single File Format

                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Or _
                    strFileType = _Helper.Autopay_Name Or strFileType = _Helper.AutopaySNA_Name Then

                    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)                                               'Get Privilege User

                ElseIf strFileType = _Helper.DirectDebit_Name Then
                    strOption = "D"
                ElseIf strFileType = _Helper.CPS_Name Then
                    strOption = "C"
                Else
                    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)
                End If

                'Get File Type Option - STOP

                '071128 2 lines added by Marcus
                'Purpose: To check whether the database table has already created for the selected format.
                Dim strDBTableName As String


                strDBTableName = clsUpload.fncGetDBTableName(lngFormatId)

                If strDBTableName = gc_Status_Error OrElse strDBTableName = "" Then
                    lblMessage.Text = " Error, database table for this file format is not created."
                    lblMessage.Visible = True
                    Exit Try
                End If

                If strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name _
                    Or strFileType = _Helper.CPSDelimited_Dividen_Name Or _
                        strFileType = _Helper.CPSSingleFileFormat_Name Or strFileType = _Helper.AutopaySNA_Name Then
                    ''Store the uploaded file to FTP folder at Server - Start

                    Dim filePath As String = flUpload.PostedFile.FileName
                    Dim filename As String = Path.GetFileName(filePath)



                    ''Set Temp Folder for each File
                    If strFileType = _Helper.CPSDelimited_Dividen_Name Then
                        ServerPath = _Helper.FileUploadedPath & _Helper.CPSDelimitedUploadedFolder & "\" & filename
                        FolderDir = _Helper.FileUploadedPath & _Helper.CPSDelimitedUploadedFolder

                    ElseIf strFileType = _Helper.CPSSingleFileFormat_Name Then
                        ServerPath = _Helper.FileUploadedPath & _Helper.CPSSFFUploadedFolder & "\" & filename
                        FolderDir = _Helper.FileUploadedPath & _Helper.CPSSFFUploadedFolder
                    Else
                        ServerPath = _Helper.FileUploadedPath & _Helper.CPSDividendUploadedFolder & "\" & filename
                        FolderDir = _Helper.FileUploadedPath & _Helper.CPSDividendUploadedFolder
                    End If


                    'Create the path where we have to store in server
                    'ServerPath = _Helper.FileUploadedPath & _Helper.CPSDividendUploadedFolder & "\" & filename
                    MainFilePath.Value = ServerPath

                    If Not System.IO.Directory.Exists(FolderDir) Then
                        System.IO.Directory.CreateDirectory(FolderDir)
                    End If
                    'Save the file in Specified location
                    flUpload.PostedFile.SaveAs(ServerPath)

                    If trSubFile.Visible = True Then
                        Dim fileSubPath As String = flSubFile.PostedFile.FileName
                        Dim Subfilename As String = Path.GetFileName(fileSubPath)


                        'Create the path where we have to store in server
                        SubServerPath = _Helper.FileUploadedPath & _Helper.CPSMemberUploadedFolder & "\" & Subfilename
                        FolderDir = _Helper.FileUploadedPath & _Helper.CPSMemberUploadedFolder
                        If Not System.IO.Directory.Exists(FolderDir) Then
                            System.IO.Directory.CreateDirectory(FolderDir)
                        End If
                        flSubFile.PostedFile.SaveAs(SubServerPath)
                        SubFilePath.Value = SubServerPath

                    End If


                    FolderDir = Nothing
                    ''Store the uploaded file to FTP folder at Server - Stop
                End If

                'Check Cutoff Time - Start
                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse _
                        strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then

                    IsCutoff = clsCommon.fncCutoffTime(strFileType, lngOrgId, lngUserId, strTime, _
                             intDay, intMonth, intYear, strOption, ConfigurationManager.AppSettings("DefaultBankCode"))

                    If IsCutoff Then

                        lblMessage.Text = "The " & strFileType & " with the Selected Payment Date " & _
                            "cannot be Uploaded after the Cutoff Time (" & strTime & "). Please Select a later Payment Date."

                        lblMessage.Visible = True
                        Exit Try

                    End If
                End If
                'Check Cutoff Time - Stop

                'Check File Name - Start
                If strFileType = _Helper.CPS_Name Then
                    Dim sMsg As String = ""

                    If Me.chkEncrypted.Checked Then
                        If strFileName.Substring(strFileName.LastIndexOf("\") + 1).ToUpper <> "PAYMENT.TXT" & _Encryption.EncryptFileExtension Then
                            sMsg += "Incorrect payment file name." & gc_BR
                        End If

                        If strSubFileName <> "" AndAlso strSubFileName.Substring(strSubFileName.LastIndexOf("\") + 1).ToUpper <> "INVOICE.TXT" & _Encryption.EncryptFileExtension Then
                            sMsg += "Incorrect invoice file name." & gc_BR
                        End If
                    Else
                        If strFileName.Substring(strFileName.LastIndexOf("\") + 1).ToUpper <> "PAYMENT.TXT" Then
                            sMsg += "Incorrect payment file name." & gc_BR
                        End If

                        If strSubFileName <> "" AndAlso strSubFileName.Substring(strSubFileName.LastIndexOf("\") + 1).ToUpper <> "INVOICE.TXT" Then
                            sMsg += "Incorrect invoice file name." & gc_BR
                        End If
                    End If

                    If sMsg <> "" Then
                        lblMessage.Text = sMsg
                        lblMessage.Visible = True
                        Exit Try
                    End If
                End If
                'Check File Name - End

                'Check File Extension - START
                strExtn = clsCommon.fncBuildContent("Upload Extn", "", lngFormatId, lngUserId)
                If Me.chkEncrypted.Checked Then
                    If Not ((UCase(strFileName.Substring(strFileName.LastIndexOf(".") - 4, 4)) = "." & UCase(strExtn)) AndAlso _
                            (Len(strFileName.Substring(strFileName.LastIndexOf("."))) = 4 AndAlso UCase(strFileName.Substring(strFileName.LastIndexOf("."), 4)) = _Encryption.EncryptFileExtension)) Then

                        lblMessage.Text = "The " & strFileType & " extension does not match with the Selected File Format."
                        lblMessage.Visible = True
                        Exit Try
                    End If
                Else
                    If Not UCase(Right(strFileName, 3)) = UCase(strExtn) Then
                        lblMessage.Text = "The " & strFileType & " extension does not match with the Selected File Format."
                        lblMessage.Visible = True
                        Exit Try
                    End If
                End If

                If Not flUpload.PostedFile.ContentLength > 0 Then
                    lblMessage.Text = "File not found or file content is emtpy. Please check your file."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Check if Valid File - STOP

                'Check If File Previously Uploaded - START
                strFileName = clsCommon.fncFileName(strFileName, False)

                strFileCheck = clsCommon.fncBuildContent("File Check", strFileType, lngOrgId, lngUserId, _
                      IIf(Me.chkEncrypted.Checked, strFileName.Substring(0, strFileName.LastIndexOf(".")), strFileName))

                If strFileCheck = "Y" Then
                    lblMessage.Text = "The " & strFileType & " has been previously uploaded. Please rename the file."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Check If File Previously Uploaded - STOP

                trAuth.Visible = True
                trFile.Visible = False
                trCFile.Visible = True
                trBank.Visible = False
                trCBank.Visible = True
                trFormat.Visible = False
                trCFormat.Visible = True
                trUpload.Visible = False
                trCUpload.Visible = True
                trSubFile.Visible = False

                If strFileType = _Helper.CPS_Name Then
                    trSubFile.Visible = False
                    Me.trCtrSubFile.Visible = True
                End If

                trPay.Visible = False
                trCPay.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                trConMnth.Visible = False
                txtFileType.Text = strFileType
                txtCPayDate.Text = txtPayDate.Value
                hMonth.Value = cmbMonth.SelectedValue
                hFormat.Value = ddlFormat.SelectedValue
                txtFormat.Text = ddlFormat.SelectedItem.Text
                txtUploadFile.Text = flUpload.PostedFile.FileName 'strFileName 'flUpload.PostedFile.FileName
                txtBankAccount.Text = ddlAccount.SelectedItem.Text
                If strFileType = _Helper.CPSMember_Name Then
                    trCSubfile.Visible = True
                    txtUploadSubFile.Text = flSubFile.PostedFile.FileName

                ElseIf strFileType = _Helper.CPS_Name Then
                    Me.txtSubFile.Text = flSubFile.PostedFile.FileName
                End If

                Me.chkEncrypted.Enabled = False

                If bIsMultipleBank = False Then
                    txtBankAccount.Text = ddlAccount.SelectedItem.Text
                    hAccount.Value = clsCommon.fncBuildContent("Ac_Number", "", clsCommon.fncBreakValue(ddlAccount.SelectedValue, " - ", False), lngUserId)
                Else
                    txtBankAccount.Visible = False
                    lblBankAccount.Visible = False
                End If
                hICMatch.Value = IIf(chkIC.Checked, "Y", "N")
                hDuplicate.Value = IIf(chkDuplicate.Checked, "Y", "N")
                lblMessage.Text = "Please Enter your Validation Code & Confirm File Upload."
                lblMessage.Visible = True

                If Not (strFileType = "Payroll File" OrElse strFileType = "Billing File" OrElse strFileType = _
                    _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _
                        _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _
                            _Helper.CPSDividen_Name OrElse strFileType = _Helper.CPSMember_Name OrElse _
                                strFileType = _Helper.CPSDelimited_Dividen_Name OrElse _
                                    strFileType = _Helper.CPSSingleFileFormat_Name OrElse _
                                        strFileType = _Helper.PayLinkPayRoll_Name) Then

                    trCEpfAcct.Visible = True
                    txtEpfAcct.Text = ddlEpfAcct.SelectedItem.Text
                    trCConMnth.Visible = True
                    txtCYear.Text = txtYear.Text
                    hMonth.Value = cmbMonth.SelectedValue
                    txtCMonth.Text = cmbMonth.SelectedItem.Text
                Else
                    trAddl.Visible = False
                    trCAddl.Visible = False
                    trCConMnth.Visible = False
                    If chkIC.Checked And chkDuplicate.Checked Then
                        lblAddCheck.Text = "Duplicate Account & IC Matching"
                    ElseIf Not chkIC.Checked And chkDuplicate.Checked Then
                        lblAddCheck.Text = "Duplicate Account"
                    ElseIf chkIC.Checked And Not chkDuplicate.Checked Then
                        lblAddCheck.Text = "IC Matching"
                    ElseIf Not chkIC.Checked And Not chkDuplicate.Checked Then
                        lblAddCheck.Text = "None"
                    End If
                End If

                'Save File To Specified Folder - Start
                strFileName = txtUploadFile.Text
                strFileName = clsCommon.fncFileName(strFileName, False)
                If strFileType <> _Helper.CPSMember_Name And strFileType <> _Helper.CPSDividen_Name _
                    And strFileType <> _Helper.CPSDelimited_Dividen_Name And strFileType <> _Helper.CPSSingleFileFormat_Name Then
                    strPath = clsCommon.fncFolder(txtFileType.Text, "UPLOAD", lngOrgId, lngUserId, bIsMultipleBank)


                    If strPath = gc_Status_Error Then
                        lblMessage.Text = "Application Error: Web Config's [PATH] Parameter not set correctly."
                        lblMessage.Visible = True
                        Exit Try
                    End If
                    If strFileType = _Helper.CPS_Name Then
                        strFilePrefix = strPath & "\" & lngOrgId & "_" & ss_lngUserID.ToString & "_" & String.Format("{0:yyyyMMddhhmmss}", DateTime.Now) & "_"
                    Else
                        strFilePrefix = strPath & "\"
                    End If
                    strFileName = strFilePrefix & strFileName
                    If Len(strSubFileName & "") > 0 Then
                        strSubFileName = clsCommon.fncFileName(strSubFileName, False)
                        hSubFile.Value = strFilePrefix & strSubFileName
                        flSubFile.PostedFile.SaveAs(hSubFile.Value)
                    End If
                Else
                    strFileName = MainFilePath.Value
                    If strFileType = _Helper.CPSMember_Name Then
                        strSubFileName = SubFilePath.Value
                    End If

                End If
                flUpload.PostedFile.SaveAs(strFileName)

                'system.IO.File.Copy(hidFile.value,strfilename)
                If Me.chkEncrypted.Checked Then
                    Dim strCCryptKey As String
                    Dim DecryptResult As Boolean = False

                    strCCryptKey = clsCommon.fncGetOrgCcryptKey(lngOrgId, lngUserId)
                    If strCCryptKey = "" OrElse strCCryptKey = Nothing Then
                        lblMessage.Text = "Unable to obtain Organization Encryption Key."
                        lblMessage.Visible = True
                        Me.btnSave.Enabled = False
                        Exit Try
                    End If
                   
                    ''Decrypt Start
                    'If strFileType = _Helper.CPSDelimited_Dividen_Name Or _Helper.CPSSingleFileFormat_Name Or _Helper.CPSDividen_Name Then
                    DecryptResult = _Encryption.DecryptMyFiles(MainFilePath.Value, strCCryptKey)
                    'Else
                    'DecryptResult = _Encryption.DecryptMyFiles(strFileName, strCCryptKey)
                    'End If
                    ''Decrypt Stop

                    'strFileName = strFileName.Substring(0, strFileName.LastIndexOf("."))
                    If Len(strSubFileName & "") > 0 Then
                        DecryptResult = _Encryption.DecryptMyFiles(hSubFile.Value, strCCryptKey)
                    End If

                    If strFileType = _Helper.CPSMember_Name Then
                        DecryptResult = _Encryption.DecryptMyFiles(SubFilePath.Value, strCCryptKey)
                    End If

                    If DecryptResult = False Then
                        lblMessage.Text = "File decryption failed, please check your encryption key and try again."
                        lblMessage.Visible = True
                        Me.btnSave.Enabled = False
                        Exit Try

                    End If

                End If

                hFileName.Value = strFileName
                'Save File To Specified Folder - Stop

                If strFileType = "Payroll File" OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _
                _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.DirectDebit_Name Then
                    'Check If File With Same Payment Date - Start
                    intCheck = clsCommon.fncUploadCheck("PAY DATE", lngOrgId, lngUserId, lngGroupId, strFileType, txtPayDate.Value, 0)
                    If intCheck > 0 Then
                        lblMessage.Text = "Please Note: There has already a file been Uploaded with the same Payment Date. Please Enter your Validation Code and Confirm, if you wish to proceed."
                        lblMessage.Visible = True
                    Else
                        If chkIC.Checked Then
                            lblMessage.Text = lblMessage.Text & "<br><br>Note: You have selected IC Checking, please ensure that your employee's IC Numbers are correct with the host record at your branch."
                            lblMessage.Visible = True
                        End If
                    End If
                    'Check If File With Same Payment Date - Stop
                End If

            Catch ex As Exception

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Confirm - PG_FileUpload", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            fncKillPopOut()
        End Sub

#End Region

#Region "Populate Month"

        Private Sub prcMonth()

            'Variable declarations
            Dim intCounter As Int16

            Try

                cmbMonth.Items.Clear()
                cmbMonth.Items.Add(New ListItem("Select", "00"))
                For intCounter = 1 To 12
                    cmbMonth.Items.Add(New ListItem(MonthName(intCounter), Format(intCounter, "00")))
                Next

            Catch ex As Exception

            End Try

        End Sub

#End Region

        'Protected Sub ddlAccount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlAccount.SelectedIndexChanged
        Private Sub BindDDlFormat()
            Dim clsCommon As New clsCommon
            'Populate File Format based on File Type - Start
            ddlFormat.Items.Clear()
            Try
                ddlFormat.DataSource = clsCommon.fncGetRequested("File Format", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, ddlFileType.SelectedValue, "")
                ddlFormat.DataTextField = "FNAME"
                ddlFormat.DataValueField = "FID"
                ddlFormat.DataBind()
                ddlFormat.Items.Insert(0, New ListItem("Select", ""))
                Select Case Me.ddlFileType.SelectedValue
                    Case "Payroll File", _Helper.DirectDebit_Name
                        trAddl.Visible = False
                        trConMnth.Visible = False
                        tblEPF.Visible = False
                        tblSocso.Visible = False
                        tblLHDN.Visible = False
                        tblPayroll.Visible = True
                    Case Else
                        trAddl.Visible = False
                        trConMnth.Visible = False
                        trEpfAcct.Visible = False
                        tblEPF.Visible = False
                        tblSocso.Visible = False
                        tblLHDN.Visible = False
                        tblPayroll.Visible = True
                End Select
                prcDDLEnabling()
            Catch ex As Exception
                Me.lblMessage.Text = "Error in loading the File Format"
                lblMessage.Visible = True
                Me.LogError("ddlAccount_SelectedIndexChanged")
            End Try
        End Sub

#Region "Search Option Selected Change "

        Protected Sub Option_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFileType.SelectedIndexChanged

            Try
                Call HideShowOptions()



            Catch ex As Exception
                ' Error Logs Starts Here
                Dim clsGeneric As New MaxPayroll.Generic
                Call clsGeneric.ErrorLog("SearchOption Select- PG_FileUpload", Err.Number, Err.Description)


            End Try

        End Sub

#End Region

#Region "Hide Show Rate Option"
        Private Sub HideShowOptions()
            If ddlFileType.SelectedItem.Value = _Helper.CPSMember_Name Then

                lblUpload.Text = "Dividend File to be submitted"
                Me.trSubFile.Visible = True
            End If

        End Sub
#End Region

    End Class

End Namespace
