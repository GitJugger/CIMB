Imports System.IO
Imports MaxPayroll
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsUpload
Imports System.Data.SqlClient
Imports MaxMiddleware
Imports MaxModule
Imports MaxGeneric
Imports MaxCams

Namespace MaxPayroll
    Partial Class pg_PTPTNUpload
        Inherits clsBasePage


#Region " Global declarations "

        Private _Helper As New Helper
        Private _WebCommon As New WebCommon
        Private _WebHelper As New WebHelper
        Private clsCustomer As New clsCustomer
        Private _Encryption As New clsEncryption
        Private clsUsers As New MaxPayroll.clsUsers
        Private clsGeneric As New MaxPayroll.Generic
        Private clsCommon As New MaxPayroll.clsCommon
        Private _MaxDataBase As New MaxModule.DataBase
        Private clsUpload As New MaxPayroll.clsUpload
        Private _MaxCamsUploader As New MaxCams.Uploader

#End Region

#Region "Global Variables Declaration "

        Dim filePath As String = Nothing, filename As String = Nothing
        Dim ServerPath As String = Nothing, FolderDir As String = Nothing
        'Shared _ServiceType As Short = 0, _FileTypeId As Integer = 0

#End Region

#Region "Properties "

        Private ReadOnly Property bIsMultiBank() As Boolean
            Get
                Dim oPaySer As New clsPaymentService
                Return oPaySer.fncIsMultipleBank(ddlFileType.SelectedValue)
            End Get
        End Property

        Private Property ServiceType() As Short
            Get
                Return MaxGeneric.clsGeneric.NullToShort(ViewState("ServiceType").ToString())
            End Get

            Set(ByVal value As Short)
                ViewState("ServiceType") = value
            End Set

        End Property

        Private Property FileTypeId() As Integer
            Get
                Return MaxGeneric.clsGeneric.NullToInteger(ViewState("FileTypeId").ToString())
            End Get

            Set(ByVal value As Integer)
                ViewState("FileTypeId") = value
            End Set

        End Property

#End Region

#Region "Page Load "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Create Instance -start
            Dim drUpload As DataRow = Nothing, dsUpload As DataSet = Nothing
            Dim FileTypeTable As DataSet = Nothing
            'Create Instance -stop

            'Variable Declarations-start
            Dim strAuthLock As String = Nothing, IsICCheck As Boolean = False
            Dim intReviewers As Int16 = 0, intAuthorisers As Int16 = 0
            Dim strPayroll As String = Nothing
            'Variable Declarations-stop

            Try

                'Payment Date Minimum Value
                rngPayDate.MinimumValue = Today
                'Payment Date Maximum Value
                rngPayDate.MaximumValue = DateAdd(DateInterval.Day, 60, Today)

                If Not ss_strUserType = gc_UT_Uploader Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

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

                    hAlert.Value = "N"

                    ' FileTypeTable = clsCustomer.fncGetGroupPaymentService(ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                    FileTypeTable = PPS.GetDataSet(_Helper.GetSQLFileTypes & ss_lngGroupID, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

                    'Populate FileType DropdownList-start
                    FormHelp.PopulateDropDownList(_WebCommon.GetGroupServiceTypes(ss_lngGroupID, "Hybrid"),
                        ddlFileType, _WebHelper.ColServiceType, _WebHelper.ColServiceId)
                    'Populate FileType DropdownList- Stop

                    ddlFileType.Items.Insert(0, New ListItem("Select", ""))
                    'Populate FileType DropdownList-stop

                    If Request.QueryString("PTYPE") <> "" Then
                        ddlFileType.SelectedValue = Request.QueryString("PTYPE")
                        prcFileTypeChanged()
                        BindDDlFormat()
                    End If

                    prcDDLEnabling()
                    Call clsCommon.fncUploadBtnDisable(btnSave, True)

                    'Intially make BankAcc and OrgCode TextBoxes visible false-start
                    trBankAcc.Visible = False
                    trOrgCode.Visible = False
                    'Intially make BankAcc and OrgCode TextBoxes visible false-stop

                End If

            Catch ex As Exception

            End Try
        End Sub

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
#End Region

#Region "FileType Changed "

        Private Sub prcFileTypeChanged()

            'Create Instance -start
            Dim drUpload As DataRow = Nothing, dsUpload As New DataSet
            Dim FileFormat As DataSet = Nothing, FileTypeDetails As DataTable = Nothing
            'Create Instance -stop

            'Variable Declarations-start
            Dim strFileType As String = Nothing
            'Variable Declarations-start


            Try

                'Get filetype name
                strFileType = ddlFileType.SelectedItem.Text

                'Clear file format
                ddlFormat.Items.Clear()

                'Get the ServiceType and FileTypeId based on the selected filtype - Start
                FileTypeDetails = PPS.GetData(_Helper.GetSQLServiceTypeFileType &
                        MaxGeneric.clsGeneric.NullToInteger(ddlFileType.SelectedValue),
                             _Helper.GetSQLConnection, _Helper.GetSQLTransaction)
                'Get the ServiceType and FileTypeId based on the selected filtype - Stop

                'If filetype details exist - Start
                If FileTypeDetails.Rows.Count > 0 Then

                    'Get values - Start
                    ServiceType = MaxGeneric.clsGeneric.NullToShort(FileTypeDetails.Rows(0)(_Helper.GetServiceTypeCol))
                    FileTypeId = MaxGeneric.clsGeneric.NullToInteger(FileTypeDetails.Rows(0)(_Helper.GetFileIdCol))
                    'Get values - Stop

                End If
                'If filetype details exist - Stop

                'FlieType visible True/False -start
                If ServiceType = Helper.ServiceType.Collections Then

                    FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trBankAcc, trOrgCode)
                    FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, tblPayroll, trPay)
                    FormHelp.ResetFormControls(Me, txtPayDate)
                ElseIf ServiceType = Helper.ServiceType.Payments Then

                    FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trBank)
                    FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, tblPayroll, trPay, trBankAcc, trOrgCode)
                    FormHelp.ResetFormControls(Me, txtPayDate)
                ElseIf ServiceType = Helper.ServiceType.Mandates Then
                    FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trBankAcc, trOrgCode,
                             trBank, trPay, trMainLabel, tblPayroll)
                    lblUpload.Text = "Mandate File"
                End If
                'FlieType visible True/False -stop


                'If ServiceType Is Collections-start
                If ServiceType = Helper.ServiceType.Collections Then

                    'Get BankAccounts - START
                    If bIsMultiBank Then

                        Try

                            Me.ddlAccount.Items.Clear()
                            trBank.Visible = False

                            'Populate File Format based on File Type - Start
                            ddlFormat.Items.Clear()

                            FileFormat = clsCommon.fncGetRequested("File Format", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, ddlFileType.SelectedItem.Text, "", 0)
                            FormHelp.PopulateDropDownList(FileFormat.Tables(0), ddlFormat, "FNAME", "FID")
                            ddlFormat.Items.Insert(0, New ListItem("Select", ""))
                            tblPayroll.Visible = True

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
                                ddlAccount.Items.Add(New ListItem(CStr(drUpload("BACCName") & "") _
                                     & " (" & CStr(drUpload("Account_No") & "") & ")", CStr(drUpload("BACCID") & "")))
                            Next
                        End If
                        ddlAccount.DataSource = clsCommon.fncGetRequested("Bank Accts", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, "")
                        ddlAccount.Items.Insert(0, New ListItem("Select", ""))
                    End If

                    'Get BankAccounts - STOP
                    prcDDLEnabling()

                End If
                'If ServiceType Is Collections _ Stop

            Catch ex As Exception

            Finally
                'Force Garbage Collecter
                GC.Collect(0)
            End Try

        End Sub

#End Region

#Region "Bind File Format "

        Private Sub BindDDlFormat()
            'Instances declaration -start
            Dim FileFormat As DataSet = Nothing
            'Instances declaration -stop

            Try
                'Populate File Format based on File Type - Start
                ddlFormat.Items.Clear()
                FileFormat = clsCommon.fncGetRequested("File Format", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, ddlFileType.SelectedItem.Text, "")
                FormHelp.PopulateDropDownList(FileFormat.Tables(0), ddlFormat, "FNAME", "FID")
                ddlFormat.Items.Insert(0, New ListItem("Select", ""))
                'tblPayroll.Visible = True
                prcDDLEnabling()

            Catch ex As Exception
                Me.lblMessage.Text = "Error in loading the File Format"
                lblMessage.Visible = True
                Me.LogError("ddlAccount_SelectedIndexChanged")
            End Try
        End Sub

#End Region

#Region "FileType Changed "

        Protected Sub ddlFileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFileType.SelectedIndexChanged

            'UI Management 
            Call prcFileTypeChanged()

            'If Not ServiceType = Helper.ServiceType.Mandates Then
            Call BindDDlFormat()
            'End If

        End Sub

#End Region

#Region "Page Confirm "

        Protected Sub btnConfirm_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) Handles btnConfirm.Command

            'Variable Declarations-start
            Dim lngGroupId As Long = 0, strExtn As String = Nothing
            Dim strFileType As String = Nothing, intYear As Int16 = 0
            Dim strTime As String = Nothing, strOption As String = Nothing
            Dim lngFormatId As Long = 0, strFileCheck As String = Nothing
            Dim strFileName As String = Nothing, strSubFileName As String = Nothing
            Dim strFilePrefix As String = Nothing, strPath As String = Nothing
            Dim intCheck As Int16 = 0, lngOrgId As Long = 0, lngUserId As Long = 0
            Dim IsCutoff As Boolean = False, intDay As Int16 = 0, intMonth As Int16 = 0
            Dim bIsMultipleBank As Boolean = bIsMultiBank, strDBTableName As String = Nothing
            'Variable Declarations-stop
            Try

                'If Not mandates file Read the  date values -start
                If Not ServiceType = Helper.ServiceType.Mandates Then
                    intDay = Day(txtPayDate.Value)                                                 'Get Value Day
                    intYear = Year(txtPayDate.Value)                                                 'Get Value Year
                    intMonth = Month(txtPayDate.Value)                                                 'Get Value Month
                End If
                'If Not mandates file Read the  date values -stop

                If ddlFormat.Items.Count > 0 Then
                    lngFormatId = ddlFormat.SelectedValue                                        'Get File Format
                End If

                strFileName = MaxGeneric.clsGeneric.NullToString(flUpload.PostedFile.FileName)  'Get File Name

                If ddlFileType.Items.Count > 0 Then
                    strFileType = ddlFileType.SelectedItem.Text                                 'Get File Type
                End If

                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)  'Get Group Id

                'Check the table created -start
                strDBTableName = clsUpload.fncGetDBTableName(lngFormatId)

                If strDBTableName = gc_Status_Error OrElse strDBTableName = "" Then
                    lblMessage.Text = " Error, database table for this file format is not created."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Check the table created -stop

                'Check File Extension - START
                strExtn = clsCommon.fncBuildContent("Upload Extn", "", lngFormatId, lngUserId)

                If Not UCase(Right(strFileName, 3)) = UCase(strExtn) Then
                    lblMessage.Text = "The " & strFileType & " extension does not match with the Selected File Format."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Check File Extension - STOP

                'If file has data-start
                If Not flUpload.PostedFile.ContentLength > 0 Then
                    lblMessage.Text = "File not found or file content is emtpy. Please check your file."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'If file has data-stop

                'Check If File Previously Uploaded - START
                strFileName = clsCommon.fncFileName(strFileName, False)
                strFileCheck = clsCommon.fncBuildContent("File Check", strFileType, lngOrgId, lngUserId, strFileName)

                If strFileCheck = "Y" Then
                    lblMessage.Text = "The " & strFileType & " File has been previously uploaded. Please rename the file."
                    lblMessage.Visible = True
                    Exit Try
                End If
                'Check If File Previously Uploaded - STOP

                FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trFile, trBank,
                            trFormat, trUpload, trPay, trSubmit)

                'Visible True/False-start
                If ServiceType = Helper.ServiceType.Collections Then
                    FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trAuth, trCFile,
                      trCBank, trCFormat, trCUpload, trCPay, trConfirm)
                End If

                If ServiceType = Helper.ServiceType.Payments Then
                    FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trAuth,
                       trCFormat, trCUpload, trCPay, trConfirm)
                End If

                If ServiceType = Helper.ServiceType.Mandates Then

                    FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trCFile, trCFormat, trCUpload, trConfirm, trAuth)
                End If

                'Visible True/False-stop

                txtFileType.Text = MaxGeneric.clsGeneric.NullToString(strFileType)
                txtCPayDate.Text = MaxGeneric.clsGeneric.NullToString(txtPayDate.Value)
                hFormat.Value = ddlFormat.SelectedValue
                txtFormat.Text = MaxGeneric.clsGeneric.NullToString(ddlFormat.SelectedItem.Text)
                txtUploadFile.Text = MaxGeneric.clsGeneric.NullToString(flUpload.PostedFile.FileName) 'strFileName 'flUpload.PostedFile.FileName

                If ddlAccount.Items.Count > 0 Then
                    txtBankAccount.Text = MaxGeneric.clsGeneric.NullToString(ddlAccount.SelectedItem.Text)
                End If


                If bIsMultipleBank = False Then
                    If ddlAccount.Items.Count > 0 Then
                        txtBankAccount.Text = ddlAccount.SelectedItem.Text
                        hAccount.Value = clsCommon.fncBuildContent("Ac_Number", "", clsCommon.fncBreakValue(ddlAccount.SelectedValue, " - ", False), lngUserId)
                    Else
                        txtBankAccount.Text = MaxGeneric.clsGeneric.NullToString(txtBankAcc.Text)
                        hAccount.Value = MaxGeneric.clsGeneric.NullToString(txtBankAcc.Text)
                    End If
                Else
                    FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, txtBankAccount, lblBankAccount)
                End If

                lblMessage.Text = "Please Enter your Validation Code & Confirm File Upload."
                lblMessage.Visible = True

                'Save File To Specified Folder - Start
                strFileName = txtUploadFile.Text
                strFileName = clsCommon.fncFileName(strFileName, False)

                'Get Uploaded path based on the fileType
                strPath = clsCommon.fncFolder(txtFileType.Text, "UPLOAD", lngOrgId, lngUserId, bIsMultipleBank)

                If strPath = gc_Status_Error Then
                    lblMessage.Text = "Application Error: Web Config's [PATH] Parameter not set correctly."
                    lblMessage.Visible = True
                    trAuth.Visible = False
                    Exit Try
                End If

                strFilePrefix = strPath & "\"
                strFileName = strFilePrefix & strFileName
                flUpload.PostedFile.SaveAs(strFileName)

                hFileName.Value = strFileName
                'Save File To Specified Folder - Stop

                'Check If File With Same Payment Date - Start
                intCheck = clsCommon.fncUploadCheck("PAY DATE", lngOrgId, lngUserId, lngGroupId, strFileType, txtPayDate.Value, 0)

                If intCheck > 0 And Not ServiceType = Helper.ServiceType.Mandates Then
                    lblMessage.Text = "Please Note: There has already a file been Uploaded with the same Payment Date. Please Enter your Validation Code and Confirm, if you wish to proceed."
                    lblMessage.Visible = True
                End If
                'Check If File With Same Payment Date - Stop

            Catch ex As Exception

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Confirm - PG_FileUpload", Err.Number, Err.Description)
            Finally

                GC.Collect(0)
            End Try

            fncKillPopOut()

        End Sub

#End Region

#Region " Save "

        Public Shared Sub OpenProgressBar(ByVal Page As System.Web.UI.Page)
            Dim sbScript As New StringBuilder()

            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("<!--" + ControlChars.Lf)
            sbScript.Append("window.showModalDialog('./ProgressBar/Progress.aspx','','dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: No; status: No;scroll:No;');" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            Page.RegisterClientScriptBlock("OpenProgressBar", sbScript.ToString())
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

            'Create Instance-start
            Dim drServiceAccts As DataRow = Nothing, dsServiceAccts As New DataSet
            'Create Instance-stop

            'Variable Declarations
            Dim intSerAccId As Int16 = 0, intState As Int16 = 0
            Dim strFileType As String = Nothing, lngOrgId As Long = 0, lngUserId As Long = 0
            Dim IsAuthCode As Boolean = False, intValueMonth As Int16 = 0, IsIC As Boolean = False
            Dim IsDuplicate As Boolean = False, intValueYear As Int16 = 0, strAccNo As String = Nothing
            Dim strUserName As String = Nothing, strUserRole As String = Nothing, strSubject As String = Nothing, strBody As String = Nothing, intConYear As Int16
            Dim strDbAuthCode As String = Nothing, intConMonth As Int16 = 0, intAttempts As Int16 = 0
            Dim intGroupId As Int32 = 0, lngFormatId As Long = 0, intValueDay As Int16 = 0
            Dim strValidation As String = Nothing, strFileName As String = Nothing
            Dim strSubFileName As String = Nothing, IsAlert As Boolean = False
            Dim lngAccId As Long = 0, dcTranCharge As Decimal = 0, strTestStatus As String = Nothing
            Dim strSerAccNo As String = Nothing, bContributeMonth As Boolean = False
            Dim CreatedPath As String = Nothing, ErrorDetails As String = Nothing
            Dim FileId As Integer = 0, BankOrgCode As Integer = 0
            Try

                IsAlert = False
                lblMessage.Text = ""
                strAccNo = hAccount.Value
                strFileType = txtFileType.Text
                strFileName = hFileName.Value
                lngFormatId = IIf(IsNumeric(hFormat.Value), hFormat.Value, 0)

                If bIsMultiBank = False And Not ServiceType = Helper.ServiceType.Mandates Then
                    If ddlAccount.Items.Count > 0 Then
                        lngAccId = IIf(IsNumeric(clsCommon.fncBreakValue(ddlAccount.SelectedValue, " - ", False)), CLng(clsCommon.fncBreakValue(ddlAccount.SelectedValue, " - ", False)), 0)    'Account Id
                    Else
                        lngAccId = 0
                    End If
                End If

                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)            'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)         'Get User Id
                intGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)      'Get Group Id

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
                            strBody = strUserName & " (" & strUserRole & ")" & " has been Locked/Inactive on " & Now() &
                                                " due to Invalid Validation Code attempts. Please change the User Validation Code."
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
                If Not ServiceType = Helper.ServiceType.Mandates Then
                    intValueDay = Day(txtCPayDate.Text)     'Get Pay Day
                    intValueYear = Year(txtCPayDate.Text)   'Get Pay Year
                    intValueMonth = Month(txtCPayDate.Text) 'Get Pay Month
                End If
                'Set Pay Date - STOP

                'Get Bank OrgCode Fro 
                BankOrgCode = MaxGeneric.clsGeneric.NullToInteger(txtOrgCode.Text)

                'Get Payroll Transaction Charge
                dcTranCharge = MaxGeneric.clsGeneric.NullToDecimal(clsCommon.fncBuildContent("TRAN CHARGE", ddlFileType.SelectedItem.Text, lngOrgId, lngUserId))
                'End If

                'Get Created path based on the fileType
                CreatedPath = clsCommon.fncFolder(txtFileType.Text, "CREATE", lngOrgId, lngUserId)

                CreatedPath = CreatedPath & "\"

                FileId = _MaxCamsUploader.UploadFile(lngUserId, lngOrgId, strFileType, FileTypeId(), intGroupId, lngFormatId, ddlFileType.SelectedValue, ServiceType(),
                                                txtCPayDate.Text, strFileName, strAccNo, IsDuplicate, CreatedPath, "", "", "", "", ErrorDetails, BankOrgCode)

                If FileId > 0 And FormHelp.IsBlank(ErrorDetails) Then
                    trNew.Visible = True
                    trAuth.Visible = False
                    trSubmit.Visible = False
                    trConfirm.Visible = False
                    lblMessage.Text = "File Uploaded Successfully"
                    lblMessage.Visible = True

                    'Build Mail subject
                    strSubject = strFileType & " Uploaded - " & clsCommon.fncFileName(strFileName, False) & ", Upload Date: " & txtCPayDate.Text
                    'Build Body
                    strBody = "The " & strFileType & " has been successfully uploaded on " & Format(Today, "dd/MM/yyyy") & " at " & TimeOfDay

                    'Send Mails To Group Reviewers/Authorizers
                    Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, intGroupId, strSubject, strBody)

                Else : FileId = 0 And Not FormHelp.IsBlank(ErrorDetails)
                    trNew.Visible = True
                    trAuth.Visible = False
                    trSubmit.Visible = False
                    trConfirm.Visible = False
                    btnNew.Value = "Upload Again"
                    lblMessage.Text = ErrorDetails
                    lblMessage.Visible = True
                End If

            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prUpload - PG_FileUpload", Err.Number, Err.Description)
                lblMessage.Text = "strvalidation" & Err.Description
                lblMessage.Visible = True

            Finally
                'Force Garbage Collecter
                GC.Collect(0)
            End Try
            fncKillPopOut()
        End Sub
#End Region

    End Class

End Namespace