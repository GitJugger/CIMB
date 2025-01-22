#Region "Name Spaces "

Imports System.IO
Imports MaxModule
Imports MaxPayroll
Imports MaxGeneric
Imports MaxMiddleware
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsUpload
Imports System.Data.SqlClient
Imports System.Reflection.MethodBase

#End Region

Namespace MaxPayroll

    Partial Class PG_UploadFile
        Inherits clsBasePage

#Region "Global Declaration "

        'Create Instances - Start
        Private _Message As New Message
        Private _DataBase As New DataBase
        Private _WebHelper As New WebHelper
        Private _WebCommon As New WebCommon
        Private _Encryption As New clsEncryption
        Private _Uploader As New MaxCams.Uploader
        Private _Common As New MaxPayroll.clsCommon
        Private _MaxCommon As New MaxReadWrite.Common
        'Create Instances - Stop

#End Region

#Region "Get Service Type "

        Private Function GetServiceType() As String
            Return MaxGeneric.clsGeneric.NullToString(Request.QueryString(WebHelper.ServiceType))
        End Function

#End Region

#Region "Get Service Type Id "

        Private Function GetServiceTypeId() As Short
            Return MaxGeneric.clsGeneric.NullToShort(hidServiceTypeId.Value)
        End Function

#End Region

#Region "Get Transaction Date "

        Private Function GetTransactionDate(ByVal IsConfirm As Boolean) As String
            If IsConfirm Then
                Return txtPayDate.Value
            End If
            Return txtCPayDate.Text
        End Function

#End Region

#Region "Get File Type "

        Private Function GetFileType() As String
            Return MaxGeneric.clsGeneric.NullToString(ddlFileType.SelectedItem.Text)
        End Function

#End Region

#Region "Get File Type Id "

        Private Function GetFileTypeId() As Short
            Return MaxGeneric.clsGeneric.NullToShort(hidFileTypeId.Value)
        End Function

#End Region

#Region "Get Service Id "

        Private Function GetServiceId() As Short
            Return MaxGeneric.clsGeneric.NullToShort(ddlFileType.SelectedValue)
        End Function

#End Region

#Region "Get Account No "

        Private Function GetAccountNo() As String
            Return MaxGeneric.clsGeneric.NullToString(hAccount.Value)
        End Function

#End Region

#Region "Get Format Id "

        Private Function GetFormatId(ByVal IsConfirm As Boolean) As Integer
            If IsConfirm Then
                Return MaxGeneric.clsGeneric.NullToInteger(ddlFormat.SelectedValue)
            End If
            Return MaxGeneric.clsGeneric.NullToInteger(hFormat.Value)
        End Function

#End Region

#Region "Is Encryption "

        Private Function IsEncryption() As Boolean
            Return chkEncrypted.Checked
        End Function

#End Region

#Region "Page Load "

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not Page.IsPostBack Then

                    'Set Upload Initialisations
                    Call PageInit()

                    'Page Validations
                    Call PageValidations()

                    'Set Encryption Key
                    Call SetEncryptionKey()

                    'Set File Type
                    Call SetFileType()

                End If

                'Disable button on Click
                'Call _WebHelper.UploadButtonDisable(btnSave)

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Page Initialization "

        Private Sub PageInit()

            'Create Instances
            Dim ServiceTypes As DataTable = Nothing

            Try

                'Payment Date Minimum Value
                rngPayDate.MinimumValue = Today

                'Payment Date Maximum Value
                rngPayDate.MaximumValue = DateAdd(DateInterval.Day, 60, Today)

                'Get the Service Types
                ServiceTypes = _WebCommon.GetGroupServiceTypes(ss_lngGroupID, GetServiceType())

                'Populate Service Type DropdownList - Start
                Call FormHelp.PopulateDropDownList(ServiceTypes, ddlFileType, _
                    _WebHelper.ColServiceType, _WebHelper.ColServiceId)
                'Populate Service Type DropdownList - Stop

                'Populate Bank Accounts
                Call PopulateBankAccounts()

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Page Validations "

        Private Sub PageValidations()

            'Variable Declarations
            Dim AuthLockStatus As String = Nothing, GroupReviewers As Short = 0, GroupApprovers As Short = 0

            Try

                'If The User is not the uploader - Start
                If Not ss_strUserType = gc_UT_Uploader Then
                    Call Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If The User is not the uploader - Stop

                'Get Authorization Lock Status - Start
                AuthLockStatus = _Common.fncBuildContent(WebHelper.AuthStatus, String.Empty, ss_lngOrgID, ss_lngUserID)
                If AuthLockStatus = WebHelper.Yes Then
                    lblMessage.Text = _Message.MsgAuthCodeLock(gc_UT_SysAdminDesc)
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, btnConfirm)
                    Exit Try
                End If
                'Get Authorization Lock Status - Stop

                'Get Business Users Roles - Start
                GroupReviewers = _Common.fncBusinessRules(WebHelper.Reviewer, ss_lngOrgID, ss_lngUserID, ss_lngGroupID, 0)
                GroupApprovers = _Common.fncBusinessRules(WebHelper.Authorizer, ss_lngOrgID, ss_lngUserID, ss_lngGroupID, 0)
                'Get Business Users Roles - Stop

                'Check for Active Reviewers/Authorisers - Start
                If GroupReviewers = 0 Or GroupApprovers = 0 Then
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, btnConfirm)
                    lblMessage.Text = _Message.MsgNoBusinessUsers(gc_UT_SysAdminDesc, gc_UT_ReviewerDesc, gc_UT_AuthDesc)
                    Exit Try
                End If
                'Check for Active Reviewers/Authorisers - Stop

            Catch ex As Exception

                'Log Error - Start
                If Not ex.Message = _Message.MsgThreadAborted Then
                    Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                End If
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Set Encryption "

        Private Sub SetEncryptionKey()

            'Get the Encryption key for this specifc Organization
            Dim EncryptionKey As String = _Common.fncGetOrgCcryptKey(ss_lngOrgID, ss_lngOrgID)

            'if Encyption key not available - Start
            If FormHelp.IsBlank(EncryptionKey) Then
                Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, chkEncrypted)
            End If
            'if Encyption key not available - Stop

        End Sub

#End Region

#Region "Set File Type "

        Private Sub SetFileType()

            'If File types exists - Start
            If ddlFileType.Items.Count >= 1 Then

                'Assign it to default as Service Type
                ddlFileType.SelectedItem.Text = GetServiceType()

                'Disable the Dropdown for not allowing to select other File types.
                Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, ddlFileType)

                'On Selected File type change 
                Call FileTypeChanged()

            Else
                ddlFileType.Items.Insert(0, New ListItem("Select", ""))
            End If
            'If File types exists - Start

        End Sub

#End Region

#Region "Progress Bar "

        Public Shared Sub OpenProgressBar(ByVal Page As System.Web.UI.Page)

            Dim sbScript As New StringBuilder()

            sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
            sbScript.Append("<!--" + ControlChars.Lf)
            sbScript.Append("window.showModalDialog('./ProgressBar/Progress.aspx','','dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: No; status: No;scroll:No;');" + ControlChars.Lf)
            sbScript.Append("// -->" + ControlChars.Lf)
            sbScript.Append("</script>" + ControlChars.Lf)
            Page.RegisterClientScriptBlock("OpenProgressBar", sbScript.ToString())

        End Sub

#End Region

#Region "File Type Selected Index Changed "

        Private Sub ddlFileType_SelectedIndexChanged(ByVal O As System.Object, ByVal E As System.EventArgs) _
            Handles ddlFileType.SelectedIndexChanged

            Call FileTypeChanged()

        End Sub

#End Region

#Region "File Type Changed "

        Private Sub FileTypeChanged()

            'Create Instance
            Dim ServiceDetails As DataTable = Nothing

            'Variable Declarations
            Dim ServiceTypeId As Short = 0, FileTypeId As Short = 0

            Try

                'Clear the file format dropdownlist
                ddlFormat.Items.Clear()

                'Get the Service Details
                ServiceDetails = _WebCommon.GetServiceDetails(GetServiceId())

                'If Service details Available - Start
                If ServiceDetails.Rows.Count > 0 Then

                    'Get values - Start
                    ServiceTypeId = MaxGeneric.clsGeneric.NullToShort( _
                        ServiceDetails.Rows(0)(_WebHelper.ColServiceType))
                    FileTypeId = MaxGeneric.clsGeneric.NullToInteger( _
                        ServiceDetails.Rows(0)(_WebHelper.ColFileTypeId))
                    'Get values - Stop

                    'Set Values - Start
                    hidFileTypeId.Value = FileTypeId
                    hidServiceTypeId.Value = ServiceTypeId
                    'Set Values - Stop

                End If
                'If Service details Available - Stop

                'Set the controls attribute based on File type - Start
                If ServiceTypeId = Helper.ServiceType.Collections Then

                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, tblPayroll, trPay)
                    Call FormHelp.ResetFormControls(Me, txtPayDate)

                End If
                'Set the controls attribute based on File type - Stop

                'Populate File Formats
                Call PopulateFileFormats()

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            End Try

        End Sub

#End Region

#Region "Populate File Formats "

        Private Sub PopulateFileFormats()

            'Get File formats
            Dim FileFormats As DataTable = _WebCommon.GetGroupFileFormats(GetFileType(), ss_lngOrgID, ss_lngGroupID)

            'if file formats available - Start
            If FileFormats.Rows.Count > 0 Then

                'Populate File Formats - Start
                Call FormHelp.PopulateDropDownList(FileFormats, ddlFormat, _
                    _WebHelper.ColFormatName, _WebHelper.ColFormatId)
                'Populate File Formats - Stop

            Else

                lblMessage.Text = _Message.MsgNoFileFormats
                Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)

            End If
            'if file formats available - Stop

        End Sub

#End Region

#Region "Populate Bank Accounts "

        Private Sub PopulateBankAccounts()

            'Get Bank Accounts
            Dim BankAccounts As DataTable = _WebCommon.GetGroupBankAccounts(ss_lngOrgID, ss_lngGroupID)

            'if Bank Accounts available - Start
            If BankAccounts.Rows.Count > 0 Then

                'Populate Bank Accounts - Start
                Call FormHelp.PopulateDropDownList(BankAccounts, ddlAccount, _
                    _WebHelper.ColAccountDisplay, _WebHelper.ColAccountNo)
                'Populate Bank Accounts - Stop

            Else

                lblMessage.Text = _Message.MsgNoBankAccounts
                Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)

            End If
            'if Bank Accounts available - Stop

        End Sub

#End Region

#Region "Page Confirm "

        Private Sub Page_Confirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Variable Declarations
            Dim UploadFileName As String = Nothing, UploadFolder As String = Nothing

            Try

                'Get Upload File Name
                UploadFileName = flUpload.PostedFile.FileName

                'if upload validations failed - Start
                If Not UploadValidations(UploadFileName) Then
                    Exit Try
                End If
                'if upload validations failed - Stop

                'Set Values - Start
                hFormat.Value = GetFormatId(True)
                txtFileType.Text = GetFileType()
                hAccount.Value = ddlAccount.SelectedValue
                txtCPayDate.Text = GetTransactionDate(True)
                txtFormat.Text = ddlFormat.SelectedItem.Text
                txtUploadFile.Text = flUpload.PostedFile.FileName
                txtBankAccount.Text = ddlAccount.SelectedItem.Text
                'Set Values - Stop

                'UI Management - Start
                lblMessage.Text = _Message.MsgConfirmUpload()
                Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trFile, trBank, _
                    trFormat, trUpload, trSubFile, trPay, trSubmit, trCAddl)
                Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trAuth, trCFile, _
                    trCBank, trCFormat, trCUpload, trCPay, trConfirm)
                Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, chkEncrypted)
                'UI Management - Start

                'Move File To Uploaded Folder - Start
                UploadFolder = _WebHelper.GetFileTypeFolder(GetServiceType(), WebHelper.UploadFolder)
                UploadFileName = UploadFolder & Path.GetFileName(UploadFileName)
                flUpload.PostedFile.SaveAs(UploadFileName)
                hFileName.Value = UploadFileName
                'Move File To Uploaded Folder - Stop

                'Decrypt File - Start
                If Not DecryptFile(UploadFileName) Then
                    Exit Try
                End If
                'Decrypt File - Stop

                'Get File Extension - Start
                If MaxGeneric.clsGeneric.NullToShort(_MaxCommon.GetMatchField( _
                    MaxReadWrite.Helper._CommonRequest.PayDate_Repeat.ToString(), _
                        ss_lngOrgID, ss_lngGroupID, GetTransactionDate(True), GetFileType())) > 0 Then
                    lblMessage.Text = _Message.MsgSamePayDate
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                End If
                'Get File Extension - Stop

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            Finally

                Call fncKillPopOut()

            End Try

        End Sub

#End Region

#Region "Upload Validations "

        Private Function UploadValidations(ByVal UploadFileName As String) As Boolean

            'Variable Declarations - Start
            Dim UploadFileExtension As String = Nothing
            Dim ErrorMessage As String = Nothing, FileExtension As String = Nothing
            'Variable Declarations - Stop

            Try

                'Check if file valid - Start
                If Not flUpload.PostedFile.ContentLength > 0 Then
                    lblMessage.Text = _Message.MsgInvalidFile
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Return False
                End If
                'Check if file valid - Stop

                'Check Cutoff Time - Start
                If _WebCommon.IsCutoffTime(GetServiceId(), GetServiceType(), _
                   GetTransactionDate(True), EnumHelp.MpgUserRoles.Uploader, ErrorMessage) Then
                    lblMessage.Text = ErrorMessage
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Return False
                End If
                'Check Cutoff Time - Stop

                'Get File Extension - Start
                FileExtension = MaxGeneric.clsGeneric.NullToString(_MaxCommon.GetMatchField( _
                    MaxReadWrite.Helper._CommonRequest.File_Extn.ToString(), GetFormatId(True)))
                'Get File Extension - Stop

                'Get uploaded File Extension
                UploadFileExtension = Path.GetExtension(UploadFileName).Replace(".", "")

                'Check File Extension - Start
                If IsEncryption() Then
                    If Not UCase(_WebHelper.EncryptFileExtension) = UCase(UploadFileExtension) Then
                        lblMessage.Text = _Message.MsgInvalidExtension(GetServiceType())
                        Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                        Return False
                    End If
                Else
                    If Not UCase(FileExtension) = UCase(UploadFileExtension) Then
                        lblMessage.Text = _Message.MsgInvalidExtension(GetServiceType())
                        Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                        Return False
                    End If
                End If
                'Check File Extension - Stop

                'Check File Uploaded- Start
                If Convert.ToBoolean(MaxGeneric.clsGeneric.NullToShort(_MaxCommon.GetMatchField( _
                   MaxReadWrite.Helper._CommonRequest.File_Check.ToString(), ss_lngOrgID, _
                    Path.GetFileName(UploadFileName)))) Then
                    lblMessage.Text = _Message.MsgFileUploaded(GetServiceType())
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Return False
                End If
                'Check File Uploaded- Stop

                Return True

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

                Return False

            End Try

        End Function

#End Region

#Region "Decrypt File "

        Private Function DecryptFile(ByVal UploadFileName As String) As Boolean

            'Variable Declarations
            Dim EncryptionKey As String = Nothing

            'if encryption Applicable  - Start
            If IsEncryption() Then

                'Get Encryption Key - Start
                EncryptionKey = MaxGeneric.clsGeneric.NullToString(_MaxCommon.GetMatchField( _
                    MaxReadWrite.Helper._CommonRequest.Encryption_Key.ToString(), ss_lngOrgID))
                'Get Encryption Key - Stop

                'if encryption key not available - Start
                If FormHelp.IsBlank(EncryptionKey) Then
                    lblMessage.Text = _Message.MsgNoEncryptKey
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Return False
                End If
                'if encryption key not available - Stop

                'Decrypt File - Start
                If Not _Encryption.DecryptMyFiles(UploadFileName, EncryptionKey) Then
                    lblMessage.Text = _Message.MsgEncryptionFailed
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                    Return False
                Else
                    Return True
                End If
                'Decrypt File - Stop

            End If
            'if encryption Applicable  - Stop

            Return True

        End Function

#End Region

#Region "Upload File "

        Private Sub Page_Submit(ByVal Sender As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click

            'Variable Declarations - Start
            Dim MailSubject As String = Nothing
            Dim CreatedPath As String = Nothing, InputFileId As Integer = 0, ErrorDetails As String = Nothing
            Dim IsAuthCodeLocked As Boolean = False, UploadFileName As String = Nothing, MailBody As String = Nothing
            'Variable Declarations - Stop

            Try

                'Check Auth Code - Start
                If Not _WebCommon.CheckAuthCode(ss_lngOrgID, ss_lngGroupID, _
                     ss_lngUserID, txtAuthCode.Text, IsAuthCodeLocked) Then

                    'if auth code lockec - Start
                    If IsAuthCodeLocked Then

                        'UI Management - Start
                        lblMessage.Text = _Message.MsgAuthCodeLocked(gc_UT_SysAdminDesc)
                        Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Enabled, btnSave)
                        Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                        Exit Sub
                        'UI Management - Stop

                    Else

                        'UI Management - Start
                        lblMessage.Text = _Message.MsgAuthCodeInvalid
                        Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, lblMessage)
                        Exit Sub
                        'UI Management - Stop

                    End If
                    'if auth code locked - Stop

                End If
                'Check Auth Code - Stop

                'Clear Messages - Start
                lblMessage.Text = _Message.MsgEmpty
                Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Enabled, btnSave)
                Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, lblMessage)
                'Clear Messages - Stop

                'Set File Name
                UploadFileName = hFileName.Value

                'if ecnrypted file - Start
                If IsEncryption() Then
                    'Remove encryption file extension
                    UploadFileName = UploadFileName.Replace(Path.GetExtension(UploadFileName), String.Empty)
                End If
                'if ecnrypted file - Stop

                'Get Created Path
                CreatedPath = _WebHelper.GetFileTypeFolder(GetServiceType(), WebHelper.CreateFolder)

                'Upload File - Start
                InputFileId = _Uploader.UploadFile(ss_lngUserID, ss_lngOrgID, GetFileType(), hidFileTypeId.Value, _
                   ss_lngGroupID, GetFormatId(False), GetServiceId(), hidServiceTypeId.Value, _
                    GetTransactionDate(False), UploadFileName, GetAccountNo(), False, CreatedPath, _
                        String.Empty, String.Empty, String.Empty, String.Empty, ErrorDetails)
                'Upload File - Stop

                'if file uploaded successfully - Start
                If InputFileId > 0 And FormHelp.IsBlank(ErrorDetails) Then

                    'UI Management - Start
                    lblMessage.Text = _Message.MsgFileUploaded
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trNew, lblMessage)
                    Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trAuth, trSubmit, trConfirm)
                    'UI Management - Stop

                    'Build Mail Subject - Start
                    MailSubject = _Message.MsgFileUploadSubject(GetServiceType(), _
                       Path.GetFileName(UploadFileName), GetTransactionDate(False))
                    'Build Mail Subject - Stop

                    'Build Mail Body - Start
                    MailBody = _Message.MsgFileUploadBody(GetServiceType())
                    'Build Mail Body - Stop

                    'Send Mail - Start
                    Call _WebCommon.SendMailToWorkFlowUsers(ss_lngOrgID, ss_lngGroupID, _
                        InputFileId, ss_lngUserID, MailSubject, MailBody)
                    'Send Mail - Stop

                End If
                'if file uploaded successfully - Stop

                'if file upload failed - Start
                If InputFileId = 0 And Not FormHelp.IsBlank(ErrorDetails) Then

                    'UI Management - Start
                    btnNew.Value = "Upload Again"
                    lblMessage.Text = ErrorDetails
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trNew, lblMessage)
                    Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trAuth, trSubmit, trConfirm)
                    'UI Management - Stop

                End If
                'if file upload failed - Stop

                'if no Application Error - Start
                If InputFileId = 0 And FormHelp.IsBlank(ErrorDetails) Then

                    'UI Management - Start
                    lblMessage.Text = WebHelper.AppErrorMessage
                    Call FormHelp.SetAttribute(True, EnumHelp.Attribute.Visible, trNew, lblMessage)
                    Call FormHelp.SetAttribute(False, EnumHelp.Attribute.Visible, trAuth, trSubmit, trConfirm)
                    'UI Management - Stop

                End If
                'if no Application Error - Stop

            Catch ex As Exception

                'Log Error - Start
                Call _WebCommon.ErrorLog(ss_lngOrgID, GetCurrentMethod().ToString(), ex.Message)
                'Log Error - Stop

            Finally

                Call fncKillPopOut()

            End Try

        End Sub

#End Region

    End Class

End Namespace
