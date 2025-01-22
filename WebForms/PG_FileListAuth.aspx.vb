Imports MaxPayroll.Generic
Imports MaxPayroll.clsUpload
Imports MaxMiddleware


Namespace MaxPayroll

Partial Class PG_FileListAuth
        Inherits clsBasePage

#Region " Global declarations "

        Private _Helper As New Helper
        Private _CPSPhase3 As New clsCPSPhase3
        Private _ReadWriteGeneric As New MaxReadWrite.Generic

#End Region

        Private Sub prcMSCTrustGateTokenAuthentication()
            'If Len(hdnSignature.Value) > 0 Then
            '   Dim sMsg As String = ""
            '   Dim alMsg As New ArrayList
            '   Dim bCheckToken As Boolean
            '   txtalert.Visible = False
            '       bCheckToken = TokenReader.clsTokenVerify.VerifyToken(hdnSignature.Value, hdnData.Value, Session(gc_Ses_UserLoginName), CStr(Session("SYS_ORGID")), alMsg)

            '   prcConfirmAction(bCheckToken)
            '   hdnSignature.Value = ""
            'End If
        End Sub

        Enum enmDgItem
            chkBox
            FileTypeLink
            FileId
            FileType
            FileName
            PaymentDate
            WorkFlowID
            UploadDate
            NoOfTransaction
            TotalAmount
            OutBoundFileName
            FileType_Id = 12
            Service_Id = 13
        End Enum

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim dsFile As New System.Data.DataSet           'Create instance of System Data Set
            Dim clsGeneric As New MaxPayroll.Generic        'Create instance of Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon       'Create instance of Upload Class object

            'Variable Declarations
            Dim lngOrgId As Long
            Dim strAuthLock As String

            Try
                'check if only authoriser
                If ss_strUserType.Equals(gc_UT_Auth) = False Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                Me.txtAuthCode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnConfirm.ClientID + "').click();return false;}} else {return true}; ")

                'Disable Button Command on Click
                Call clsCommon.fncBtnDisable(btnConfirm, True)

                'Get Authorization Lock Status - Start


                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngUserID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    trSubmit.Enabled = False
                    trConfirm.Enabled = False
                    If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                        lblMessage.Text = "Your Token has been locked due to invalid attempts."
                    Else

                        lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                    End If

                End If

                'Get Authorization Lock Status - Stop

                If Not Page.IsPostBack Then
                    'Bind Data Grid
                    'BindBody(body)
                    Dim bContinue As Boolean = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    If CBool(Session(gc_Ses_Token)) Then
                        btnConfirm.Visible = False
                    Else
                        btnSign.Visible = False
                        Me.trChallengeCode.Visible = False
                        Me.trDynaPin.Visible = False
                    End If
                    Call prcBindGrid()
                    txtalert.Visible = False
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, ss_lngUserID, "Page Load - Pg_FileListAuth", Err.Number, Err.Description)

            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Upload Class Object
                clsCommon = Nothing
                Me.hdnData.Value = Guid.NewGuid.ToString
            End Try

        End Sub

#End Region

#Region "Check/UnCheck"

        '****************************************************************************************************
        'Procedure Name : prcCheck()
        'Purpose        : Check All Items in Datagrid
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 10/08/2006
        '*****************************************************************************************************
        Private Sub prcCheck(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnCheckAll.Click
            'System.Threading.Thread.Sleep(3000)
            'Create Instance of Datagrid Items
            Dim dgiFileRev As DataGridItem

            Try

                For Each dgiFileRev In dgFile.Items
                    Dim mychkSelect As CheckBox = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                    mychkSelect.Checked = True
                Next

            Catch ex As Exception

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : prcCheck()
        'Purpose        : Check All Items in Datagrid
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 10/08/2006
        '*****************************************************************************************************
        Private Sub prcUnCheck(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnUnCheck.Click

            'Create Instance of Datagrid Items
            Dim dgiFileRev As DataGridItem

            Try

                For Each dgiFileRev In dgFile.Items
                    Dim mychkSelect As CheckBox = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                    mychkSelect.Checked = False
                Next

            Catch ex As Exception

            End Try

        End Sub


#End Region

#Region "Reject Confirm"

        '****************************************************************************************************
        'Procedure Name : Page_Reject
        'Purpose        : To Reject the File
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 10/08/2006
        '*****************************************************************************************************
        Private Sub Page_Reject(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnReject.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Textbox
            Dim txtRemarks As TextBox
            Dim lblFileType As Label
            Dim lblaFileType As Label


            'Create Instance of Datagrid Items
            Dim dgiFileRev As DataGridItem

            'Create Instance of Checkbox
            Dim chkSelect As CheckBox

            'Variable Declarations
            Dim strFileid As String, strGivenName As String, strFileType As String, strMessage As String
            Dim lngOrgId As Long, lngUserId As Long, strFileStatus As String
            Dim intCounter As Int16, strRemarks As String, IsChecked As Boolean

            Try

                intCounter = 1
                strMessage = ""
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id

                For Each dgiFileRev In dgFile.Items

                    strFileid = dgiFileRev.Cells(2).Text                                                'Get File Id
                    strFileType = dgiFileRev.Cells(3).Text                                              'Get File Type
                    strGivenName = dgiFileRev.Cells(4).Text                                             'Get File Name
                    chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                    txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)
                    lblFileType = CType(dgiFileRev.FindControl("lblFileType"), Label)
                    lblaFileType = CType(dgiFileRev.FindControl("lblaFileType"), Label)
                    txtRemarks.Enabled = False
                    chkSelect.Enabled = False
                    lblaFileType.Visible = False
                    lblFileType.Visible = True
                    'Check if Remark Provided - START
                    If chkSelect.Checked Then
                        IsChecked = True


                        strRemarks = txtRemarks.Text                                                    'Get Remarks Textbox
                        txtRemarks.Enabled = True
                        If strRemarks = "" Then
                            txtRemarks.Enabled = True
                            IsChecked = True
                            strMessage = strMessage & "You are going to reject item on Row " & intCounter & ". Please enter Remarks.<BR>"
                        End If

                    End If
                    'Check if Remark Provided - STOP


                    'Check File Already Submitted - START
                    strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", strFileid, lngUserId)
                    If Not strFileStatus = "" Then
                        lblMessage.Text = lblMessage.Text & "<BR>" & "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus
                    End If
                    'Check File Already Submitted - STOP

                    intCounter = intCounter + 1

                Next

                If Not IsChecked Then
                    trSubmit.Visible = True
                    btnAccept.Enabled = True
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    trAuthCode.Visible = False
                    lblMessage.Text = "No Rows Selected."
                    For Each dgiFileRev In dgFile.Items
                        chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                        lblFileType = CType(dgiFileRev.FindControl("lblFileType"), Label)
                        lblaFileType = CType(dgiFileRev.FindControl("lblaFileType"), Label)
                        txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)
                        chkSelect.Enabled = True
                        lblFileType.Visible = False
                        lblaFileType.Visible = True
                        txtRemarks.Enabled = True
                    Next
                    Exit Try
                Else
                    Me.btnCheckAll.Enabled = False
                    Me.btnUnCheck.Enabled = False
                End If

                If strMessage = "" Then
                    hdncommand.Value = "R"
                    'Hide Authentication Code if the token setting is true and user type is authorizer. #15 Jan 2007 - Start
                    If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                        trAuthCode.Visible = False
                        lblMessage.Text = "Please click Sign button to validate Token."
                    Else
                        trAuthCode.Visible = True
                        lblMessage.Text = "Please Enter your Validation Code and Confirm File Rejection."
                    End If
                    'Hide Authentication Code if the token setting is true and user type is authorizer. #15 Jan 2007 - End
                    tblForm.Visible = True
                    trSubmit.Visible = False
                    trConfirm.Visible = True

                    If Session(gc_Ses_Token) Then
                        Me.trChallengeCode.Visible = True
                        Me.trDynaPin.Visible = True
                        prcTokenStatus()
                    End If
                    txtRemarks.ReadOnly = True
                    lblHeading.Text = "File Approve & Submission"
                    btnAccept.Enabled = True

                Else
                    trSubmit.Visible = True
                    btnAccept.Enabled = False
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    trAuthCode.Visible = False
                    lblMessage.Text = strMessage
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Confirm - Page_Reject", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Textbox
                txtRemarks = Nothing

                'Destroy Instance of Datagrid Item
                dgiFileRev = Nothing

                'Destroy Instance of Checkbox
                chkSelect = Nothing

            End Try

        End Sub

#End Region

#Region "Accept Confirm"


        '****************************************************************************************************
        'Procedure Name : Page_Accept
        'Purpose        : To Update the Status to Authorize
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 10/08/2006
        '*****************************************************************************************************
        Private Sub Page_Accept(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnAccept.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Textbox
            Dim txtRemarks As TextBox

            Dim lblFileType As Label
            Dim lblaFileType As Label

            'Create Instance of Datagrid Items
            Dim dgiFileRev As DataGridItem

            'Create Instance of Checkbox
            Dim chkSelect As CheckBox

            'Variable Declarations
            Dim strFileid As String, strGivenName As String, strFileType As String
            Dim strFileStatus As String
            Dim IsChecked As Boolean
            Dim intCheck As Int16
            Dim strDateType As String

            Try

                lblMessage.Text = ""
                txtalert.Text = ""
                lblHeading.Text = "File Approve & Submission"

                For Each dgiFileRev In dgFile.Items

                    strFileid = dgiFileRev.Cells(enmDgItem.FileId).Text
                    'Get File Id
                    strFileType = dgiFileRev.Cells(enmDgItem.FileType).Text                                              'Get File Type
                    strGivenName = dgiFileRev.Cells(enmDgItem.FileName).Text                                             'Get File Name
                    If IsDate(dgiFileRev.Cells(enmDgItem.PaymentDate).Text) Then
                        hdnpymtdt.Value = CDate(dgiFileRev.Cells(enmDgItem.PaymentDate).Text)
                    Else
                        hdnpymtdt.Value = DateTime.MinValue
                    End If
                    If IsNumeric(dgiFileRev.Cells(enmDgItem.TotalAmount).Text) Then
                        hdnTAmount.Value = dgiFileRev.Cells(enmDgItem.TotalAmount).Text
                    Else
                        hdnTAmount.Value = "0"
                    End If

                    chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)                    'Get Check Box
                    lblFileType = CType(dgiFileRev.FindControl("lblFileType"), Label)
                    lblaFileType = CType(dgiFileRev.FindControl("lblaFileType"), Label)
                    'Check if Checkbox Checked - START
                    If chkSelect.Checked Then
                        IsChecked = True
                    End If

                    If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                        strDateType = "Payment"
                    Else
                        strDateType = "Upload"
                    End If
                    'Check if Checkbox Checked - STOP

                    chkSelect.Enabled = False
                    lblFileType.Visible = True
                    lblaFileType.Visible = False
                    txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)
                    txtRemarks.Enabled = False
                    'Check File Already Submitted - START
                    strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", strFileid, ss_lngUserID)
                    If Not strFileStatus = "" Then
                        lblMessage.Text += "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus & gc_BR
                    End If
                    'Check File Already Submitted - STOP

                    'Check if Same Payment Date & Same Amount has already been Authorized - Start
                    If chkSelect.Checked Then
                        intCheck = clsCommon.fncUploadCheck("FINAL ALERT", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strFileType, hdnpymtdt.Value, hdnTAmount.Value)
                        If intCheck > 0 Then
                            txtalert.Visible = True
                            Select Case strFileType.Substring(0, 1).ToLower
                                Case "a", "e", "i", "o", "u"
                                    txtalert.Text = txtalert.Text & vbCrLf & "- Please Note: An " & strFileType
                                Case Else
                                    txtalert.Text = txtalert.Text & vbCrLf & "- Please Note: A " & strFileType
                            End Select

                            'txtalert.Text = txtalert.Text & " (" & strGivenName & ") with the same " & strDateType & " Date & Total Amount has already been reviewed by you. Please check File Status Report or contact " & gc_Const_CompanyName & " registration centre at " & gc_Const_CompanyContactNo & "."
                            txtalert.Text = txtalert.Text & " with the same " & strDateType & " Date & Total Amount has already been reviewed by you. Please check File Status Report or contact " & gc_Const_CompanyName & " registration centre at " & gc_Const_CompanyContactNo & "."
                        End If
                    End If
                    'Check if Same Payment Date & Same Amount has already been Authorized - Stop

                Next

                If Not IsChecked Then
                    trSubmit.Visible = True
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    trAuthCode.Visible = False
                    lblMessage.Text = "No Rows Selected."
                    For Each dgiFileRev In dgFile.Items
                        chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                        lblFileType = CType(dgiFileRev.FindControl("lblFileType"), Label)
                        lblaFileType = CType(dgiFileRev.FindControl("lblaFileType"), Label)
                        txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)
                        chkSelect.Enabled = True
                        lblFileType.Visible = False
                        lblaFileType.Visible = True
                        txtRemarks.Enabled = True
                    Next
                    Exit Try
                Else
                    Me.btnCheckAll.Enabled = False
                    Me.btnUnCheck.Enabled = False
                End If

                hdncommand.Value = "A"
                If CBool(Session(gc_Ses_Token)) AndAlso ss_strUserType.Equals(gc_UT_Auth) Then
                    trAuthCode.Visible = False
                    lblMessage.Text = "Please click Sign button to validate Token."
                Else
                    trAuthCode.Visible = True
                    lblMessage.Text = "Please Enter your Validation Code and Confirm File Approve."
                End If

                tblForm.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                'If Session(gc_Ses_Token) Then
                '    Me.trChallengeCode.Visible = True
                '    Me.trDynaPin.Visible = True
                '    prcTokenStatus()
                'End If
                'txtRemarks.ReadOnly = True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page_Accept - PG_FileListAuth", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Textbox
                txtRemarks = Nothing

                'Destroy Instance of Datagrid Item
                dgiFileRev = Nothing

                'Destroy Instance of Checkbox
                chkSelect = Nothing

            End Try

        End Sub

#End Region

#Region "Accept/Reject File"


        '****************************************************************************************************
        'Procedure Name : prcReviewFile
        'Purpose        : To Update the Status to authorize
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 10/08/2006
        '*****************************************************************************************************
        Private Sub prcReviewFile(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click
            prcConfirmAction()
        End Sub

        Private Sub prcConfirmAction(Optional ByVal bCheckToken As Boolean = False)
            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Textbox
            Dim txtRemarks As TextBox

            'Create Instance of Datagrid Items
            Dim dgiFileRev As DataGridItem

            'Create instance of System Data Set
            Dim dsFile As New System.Data.DataSet

            'Create instance of system CheckBox
            Dim chkSelect As CheckBox

            'Variable Declarations
            Dim strErrMsg As String
            Dim intRecordCount As Int32
            Dim strGivenName As String
            Dim strFileType As String
            Dim strFileStatus As String
            Dim strFileName As String = ""
            Dim lngUserId As Long
            Dim lngOrgId As Long
            Dim bIsAuthCode As Boolean
            Dim strReason As String
            Dim strUserIP As String
            Dim bIsCutoff As Boolean
            Dim strDbAuthCode As String
            Dim intAttempts As Int16
            Dim strTime As String = ""
            Dim strOption As String = ""
            Dim lngGroupId As Long
            Dim strUserRole As String
            Dim strUserName As String
            Dim strSubject As String
            Dim strBody As String
            Dim intFlowCount As Int16
            Dim strFileid As String
            Dim strWorkFlowId As String
            Dim bIsSubmit As Boolean
            Dim strFFileName As String
            Dim strDateType As String
            ''Declare For Member File Start
            Dim DivFileId As Long
            Dim FileGivenName As String
            Dim lngSubFlowId As Long
            Dim ErrorMsg As String
            Dim FileType_Id As Short = 0
            Dim Service_Id As Short = 0
            Dim ErrorDetails As String = Nothing
            Dim ServiceType As Short = 0
            Dim SqlStatement As String = Nothing
            ''Member File Stop
            'bIsAuthCode = fncChkAuth()

            If fncChkAuth(bCheckToken) Then

                strBody = ""
                strSubject = ""
                lblMessage.Text = ""                                               'Clear the label message
                strUserIP = Request.ServerVariables("REMOTE_ADDR")                 'User IP Address
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)       'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)      'Get User Id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)   'Get User Group

                For Each dgiFileRev In dgFile.Items
                    Try
                        strFileid = dgiFileRev.Cells(2).Text                               'Get File Id
                        strFileType = dgiFileRev.Cells(3).Text                             'Get File Type
                        strGivenName = dgiFileRev.Cells(4).Text                            'Get Given Name
                        If IsDate(dgiFileRev.Cells(5).Text) Then
                            hdnpymtdt.Value = CDate(dgiFileRev.Cells(5).Text)                         'Get Payment Date)
                        Else
                            hdnpymtdt.Value = DateTime.MinValue
                        End If

                        strWorkFlowId = dgiFileRev.Cells(6).Text                           'Get WorkFlow Id
                        hdnTAmount.Value = dgiFileRev.Cells(9).Text                         'Get Total Amount
                        strFFileName = dgiFileRev.Cells(10).Text                            'Get File Name
                        txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)   'Get Remarks Texbox
                        strReason = IIf(txtRemarks.Text = "", "NA", txtRemarks.Text)        'Reason/Remarks
                        chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                        FileType_Id = MaxGeneric.clsGeneric.NullToShort(dgiFileRev.Cells(12).Text)
                        Service_Id = MaxGeneric.clsGeneric.NullToShort(dgiFileRev.Cells(13).Text)

                        'Get service type based on serviceId -start
                        If Service_Id > 0 Then
                            SqlStatement = _Helper.GetSQLCommon & "'ServiceType'," & Service_Id

                            ServiceType = MaxGeneric.clsGeneric.NullToShort(MaxMiddleware.PPS.SQLScalarValue( _
                                             _Helper.GetSQLConnection, _Helper.GetSQLTransaction, String.Empty, SqlStatement))
                        End If
                        'Get service type based on serviceId -stop

                        If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                            strDateType = "Payment"
                        Else
                            strDateType = "Upload"
                        End If

                        'Get Check Box

                        ''Added For Member File Start
                        If strFileType = _Helper.CPSMember_Name Then
                            ErrorMsg = ""
                            DivFileId = 0
                            lngSubFlowId = 0
                            FileGivenName = ""

                            DivFileId = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                                        _CPSPhase3.SQL_MiniStatement & strFileid & "," & clsCPSPhase3.enmMiniStatement.GetDividendFileId)

                            lngSubFlowId = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                              _CPSPhase3.SQL_MiniStatement & DivFileId & "," & clsCPSPhase3.enmMiniStatement.GetWorkFlowId & "," & ss_lngUserID)

                            FileGivenName = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                                        _CPSPhase3.SQL_MiniStatement & DivFileId & "," & clsCPSPhase3.enmMiniStatement.GetFileGivenName)
                            ''Member FIle Stop
                        End If

                        'Update Status
                        If chkSelect.Checked Then



                            'Check File Already Submitted - START
                            strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", strFileid, lngUserId)
                            If Not strFileStatus = "" Then
                                lblMessage.Text += "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus & gc_BR
                                Me.btnCheckAll.Enabled = True
                                Me.btnUnCheck.Enabled = True
                                Exit Try
                            End If
                            'Check File Already Submitted - STOP

                            If FileType_Id > 0 And Service_Id > 0 And Not ServiceType = Helper.ServiceType.Mandates Then
                                'Check cutoff-start
                                If _ReadWriteGeneric.IsCutoffTime(FileType_Id, hdnpymtdt.Value, Service_Id, ErrorDetails, strTime) Then
                                    bIsCutoff = True
                                End If
                                'Check cutoff-stop
                            End If

                            'Check Cutoff Time - Start
                            If hdncommand.Value = "A" Then

                                'Get Table Name - START
                                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                                    'Check If Privilege Customer
                                    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)                                               'Get Privilege User
                                ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                                    strOption = "E"
                                ElseIf strFileType = "SOCSO File" Then
                                    strOption = "S"
                                ElseIf strFileType = "LHDN File" Then
                                    strOption = "L"
                                ElseIf strFileType = "Zakat" Then
                                    strOption = "Z"
                                ElseIf strFileType = _Helper.DirectDebit_Name Then
                                    strOption = "D"
                                ElseIf strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)
                                ElseIf strFileType = _Helper.CPS_Name Then
                                    strOption = "C"
                                ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                                    strOption = "I"
                                End If
                                'Get Table Name - STOP

                                'Check If File Reached Cutoff Time
                                'Dim strBankId As String = clsCommon.fncGetCutOffTimeBankId(strGivenName)

                                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.PayLinkPayRoll_Name Then
                                    bIsCutoff = clsCommon.fncCutoffTime(strFileType, lngOrgId, lngUserId, strTime, Day(hdnpymtdt.Value), _
                                        Month(hdnpymtdt.Value), Year(hdnpymtdt.Value), strOption)
                                End If

                                If bIsCutoff Then
                                    'Build Subject
                                    strSubject = strFileType & " Approve Failed - " & strGivenName & " Payment Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & "(" & strGivenName & ") for Payment Date: " & hdnpymtdt.Value & " cannot be Approved after the Cutoff Time (" & strTime & ")."
                                    lblMessage.Text += strBody & gc_BR
                                    'Block File
                                    Call clsCommon.prcBlockFile(strFileid, 4, lngOrgId, lngUserId)
                                    'Get File Name
                                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, lngUserId)
                                    'Delete File
                                    'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                    Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                                    'Send Mails
                                    Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                                    Me.btnCheckAll.Enabled = True
                                    Me.btnUnCheck.Enabled = True
                                    'Display Alert
                                    'Response.Write("<script language='JavaScript'>")
                                    'Response.Write("alert('" & strBody & "');")
                                    'Response.Write("</script>")
                                    'Server.Transfer("PG_FileListAuth.aspx", False)
                                    Exit Try
                                End If

                            Else

                                'Get Table Name - START
                                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                                    'Check If Privilege Customer
                                    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)                                               'Get Privilege User
                                ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                                    strOption = "E"
                                ElseIf strFileType = "SOCSO File" Then
                                    strOption = "S"
                                ElseIf strFileType = "LHDN File" Then
                                    strOption = "L"
                                ElseIf strFileType = "Zakat" Then
                                    strOption = "Z"
                                ElseIf strFileType = _Helper.DirectDebit_Name Then
                                    strOption = "D"
                                ElseIf strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)
                                ElseIf strFileType = _Helper.CPS_Name Then
                                    strOption = "C"
                                ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                                    strOption = "I"
                                End If
                                'Get Table Name - STOP

                                'Check If File Reached Cutoff Time

                                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.PayLinkPayRoll_Name Then

                                    bIsCutoff = clsCommon.fncCutoffTime(strFileType, lngOrgId, lngUserId, strTime, Day(hdnpymtdt.Value), _
                                        Month(hdnpymtdt.Value), Year(hdnpymtdt.Value), strOption)
                                End If

                                If bIsCutoff Then
                                    'Build Subject
                                    strSubject = strFileType & " Reject Failed - " & strGivenName & " Payment Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & "(" & strGivenName & ") for Payment Date: " & hdnpymtdt.Value & " cannot be Rejected after the Cutoff Time (" & strTime & ").<br>Note: The file has been blocked."
                                    lblMessage.Text += strBody & gc_BR
                                    'Block File
                                    Call clsCommon.prcBlockFile(strFileid, 4, lngOrgId, lngUserId)
                                    'Get File Name
                                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, lngUserId)
                                    'Delete File
                                    'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                    Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                                    'Send Mails
                                    Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                                    Me.btnCheckAll.Enabled = True
                                    Me.btnUnCheck.Enabled = True
                                    'Display Alert
                                    'Response.Write("<script language='JavaScript'>")
                                    'Response.Write("alert('" & strBody & "');")
                                    'Response.Write("</script>")
                                    'Server.Transfer("PG_FileListAuth.aspx", False)
                                    Exit Try
                                End If

                            End If
                            'Check Cutoff Time - Stop

                            'Check File Value Date Expired - Start
                            If (strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name) AndAlso hdnpymtdt.Value < Today Then
                                'Build Subject
                                strSubject = strFileType & " Approved Failed - " & strGivenName & " Payment Date: " & hdnpymtdt.Value
                                'Build Body
                                strBody = "The Payment Date " & "(" & hdnpymtdt.Value & ") for the " & strFileType & " has expired. Please rename and upload the file with a future Payment Date."
                                lblMessage.Text += "The Payment Date " & "(" & hdnpymtdt.Value & ") for the " & strFileType & "(" & strGivenName & ") has expired. Please rename and upload the file with a future Payment Date"
                                'Block File
                                Call clsCommon.prcBlockFile(strFileid, 4, lngOrgId, lngUserId)
                                'Get File Name
                                strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, lngUserId)
                                'Delete File
                                'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                                'Send Mails
                                Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                                Me.btnCheckAll.Enabled = True
                                Me.btnUnCheck.Enabled = True
                                'Display Alert
                                'Response.Write("<script language='JavaScript'>")
                                'Response.Write("alert('" & strBody & "');")
                                'Response.Write("</script>")
                                'Server.Transfer("PG_FileListAuth.aspx", False)
                                Exit Try
                            End If
                            'Check File Value Date Expired - Stop

                            'Check if Same Payment Date & Same Amount has already been Authorized - Start

                            'intCheck = clsCommon.fncUploadCheck("FINAL ALERT", lngOrgId, lngUserId, lngGroupId, strFileType, hdnpymtdt.Value, hdnTAmount.Value)
                            'If intCheck > 0 Then
                            '    'lblMessage.Text = lblMessage.Text & "<BR>" & "Please Note: A " & strFileType & " file with the same Payment Date & Total Amount has already been authorized by you. Please check file status Report or contact eHR<sup>2</sup> registration centre at 03-9280 6657."
                            '    Select Case strFileType.Substring(0, 1).ToLower
                            '        Case "a", "e", "i", "o", "u"
                            '            lblMessage.Text = "Please Note: An " & strFileType
                            '        Case Else
                            '            lblMessage.Text = "Please Note: A " & strFileType
                            '    End Select
                            '    lblMessage.Text += " file with the same Payment Date & Total Amount has already been reviewed by you. Please check file status Report or contact eHR<sup>2</sup> registration centre at 03-9280 6657."
                            'End If

                            'Check if Same Payment Date & Same Amount has already been Authorized - Stop

                            'Reject File - START
                            If hdncommand.Value = "R" Then
                                'Build Subject
                                strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", " & strDateType & " Date: " & hdnpymtdt.Value
                                'Build Body
                                strBody = "The " & strFileType & " with " & strDateType & " Date: " & hdnpymtdt.Value & " Rejected Successfully. Remarks: " & strReason

                                'Delete File From FTP Folder
                                'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                'Marcus: Move the rejected file to backup Folder
                                strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, lngUserId)
                                If Not strFileType = "Payroll File" And Not strFileType = _Helper.MandateFile_Name Then
                                    Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                                End If
                                'Update Workflow
                                Call clsCommon.prcWorkFlow(strFileid, strWorkFlowId, lngOrgId, lngUserId, "R", "Y", strReason, strUserIP, 0, 0, "Y")
                                'Delete Rejected Trans from the table -start

                                'Call clsCommon.prcDeleteRejTrans(strFileid, lngOrgId, lngUserId, strFileType)

                                'Delete Rejected Trans from the table -stop

                                'Display Message
                                lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Payment Date: " & hdnpymtdt.Value & " has been Rejected Successfully." & gc_BR
                            End If
                            'Reject File - STOP

                            'Show Message - START
                            If hdncommand.Value = "A" Then

                                'Update Workflow
                                Call clsCommon.prcWorkFlow(strFileid, strWorkFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "Y")

                                'Get Count of Balance Authorizers
                                intFlowCount = clsUsers.fnRoleCount("AUTHORIZER", "A", "PENDING", strFileid, lngGroupId)

                                If strFileType = _Helper.CPSMember_Name Then

                                    'Update WorkFlow Dividend
                                    Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "Y")

                                End If


                                'Get Last Authorizer Submit File
                                If intFlowCount = 0 Then

                                    'Submit File ''CPS Member/Dividend File does not generate any files
                                    If strFileType = "Payroll File" Or strFileType = "Mandate File" _
                                    Or strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name Or
                                        strFileType = _Helper.CPSDelimited_Dividen_Name Or strFileType = _Helper.CPSSingleFileFormat_Name Then
                                        bIsSubmit = True
                                    Else
                                        bIsSubmit = clsCommon.fncSubmitFile(lngOrgId, lngUserId, strFileType, strFFileName)
                                    End If


                                    If bIsSubmit Then



                                        'Build Subject
                                        strSubject = "The " & strFileType & " Approved & Submitted - " & strGivenName & ", " & strDateType & " Date: " & hdnpymtdt.Value & ", Submission Date: " & Now.ToShortDateString

                                        'Build Body
                                        strBody = "The " & strFileType & " with " & strDateType & " Date: " & hdnpymtdt.Value & " Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & "<br>Remarks: " & strReason

                                        'Display Message
                                        lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & hdnpymtdt.Value & " has been Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & gc_BR

                                        'Update File Status
                                        strFileid = clsCommon.fncFileDetails("FINAL", strFileid, strFileType, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)

                                        'Added  by NARESH to insert the record in approvalmatrix only one time i.e no of approvers is zero

                                        'Dim clsApprMatrix As New clsApprMatrix

                                        'If strFileType = "Mandate File" Then
                                        '    clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", CLng(strFileid), lngUserId, 0, CLng(strFileid), "Mandate File(" & strGivenName & ") Approval", "Mandate File Approval", "", 1)

                                        'End If


                                        If strFileType = _Helper.CPSMember_Name Then
                                            'Update File Status Dividend
                                            strFileid = clsCommon.fncFileDetails("FINAL", DivFileId, _Helper.CPSDividen_Name, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)

                                        End If



                                        'Marcus: Below Code for sending email is to be reviewd
                                        Dim clsEmail As New clsEmail
                                        'Hafeez: Add for CPS III
                                        If strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name Or _
                                        strFileType = _Helper.CPSDelimited_Dividen_Name Or strFileType = _Helper.CPSSingleFileFormat_Name Then

                                            Call clsCommon.prcSendMails("BANK USER", lngOrgId, lngUserId, 0, strSubject, strBody, 0)
                                        ElseIf strFileType = "Mandate File" Then
                                            Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserId, 0, strSubject, strBody, 0)
                                        Else
                                            clsEmail.NotifyBankDownloader(strFFileName, "", strFileType)
                                        End If
                                    Else

                                        'Build Subject
                                        strSubject = strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & hdnpymtdt.Value & " Validation & Submission Failed."

                                        'Build Body
                                        strBody = "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & hdnpymtdt.Value & " Validation & Submission Failed. Remarks: Please Re-Approve."

                                        'Message
                                        lblMessage.Text += strBody & gc_BR

                                        'Update Remarks
                                        Call clsCommon.fncRemarks(strFileid, strBody)

                                        'Update Workflow
                                        Call clsCommon.prcWorkFlow(strFileid, strWorkFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "N")

                                        If strFileType = _Helper.CPSMember_Name Then
                                            'Update Remarks
                                            Call clsCommon.fncRemarks(DivFileId, strBody)
                                            'Update WorkFlow Dividend
                                            Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "N")

                                        End If

                                    End If

                                Else

                                    'Build Subject
                                    strSubject = "The " & strFileType & " Approved - " & strGivenName & ", " & strDateType & " Date: " & hdnpymtdt.Value

                                    'Build Body
                                    strBody = "The " & strFileType & " with " & strDateType & " Date: " & hdnpymtdt.Value & " Approved Successfully. Remarks: " & strReason

                                    'Display Success Message and Balance No of Authorizers
                                    lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & hdnpymtdt.Value & " has been Approved Successfully.<BR>Please Note: " & intFlowCount & " more Approver(s) to Approve the file." & gc_BR

                                End If

                            End If
                            'Show Message - STOP

                            'Send Mails
                            Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)


                            'Commented by Naresh  reason: inserting the record more than one time if no of approvers are greater than 1 
                            'Needs to insert into the approval matrix only one time apfter all approvers approved succussfully

                            Dim clsApprMatrix As New clsApprMatrix
                            If strFileType = "Mandate File" Then
                                clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", CLng(strFileid), lngUserId, 0, CLng(strFileid), "Mandate File(" & strGivenName & ") Approval", "Mandate File Approval", "", 1)

                            End If


                        End If


                    Catch

                        'Error  Message
                        lblMessage.Text = "File Approve/Reject Failed."

                        'Log Error
                        clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcConfirmAction - PG_FileListAuth", Err.Number, Err.Description)

                    Finally



                    End Try
                Next

                ''Show/Hide Tablerow            
                'trAuthCode.Visible = False
                ''trBack.Visible = True
                'trSubmit.Visible = False
                'trConfirm.Visible = False

                'Populate Data Grid
                dsFile = clsCommon.fncFileGrid("A", lngOrgId, lngUserId, lngGroupId)
                intRecordCount = dsFile.Tables(0).Rows.Count    'get record count
                If intRecordCount > 0 Then

                    txtalert.Text = String.Empty
                    pnlGrid.Visible = True
                    dgFile.Visible = True
                    trSubmit.Visible = True
                    trAuthCode.Visible = False
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    btnCheckAll.Visible = True
                    btnUnCheck.Visible = True
                    dgFile.DataSource = dsFile
                    dgFile.DataBind()



                Else

                    txtalert.Text = String.Empty
                    dgFile.Visible = False
                    pnlGrid.Visible = False
                    trSubmit.Visible = False
                    trAuthCode.Visible = False
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    btnCheckAll.Visible = False
                    btnUnCheck.Visible = False
                End If
                'Populate Datagrid - STOP
                If txtalert.Text = "" Then
                    txtalert.Visible = False
                Else
                    txtalert.Visible = True
                End If
                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Upload Class Object
                clsCommon = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Instance of Textbox
                txtRemarks = Nothing

                'Destroy Instance of Datagrid Item
                dgiFileRev = Nothing

                'Destroy Instance of Data set
                dsFile = Nothing

                'Destroy Instance of Datagrid Item
                chkSelect = Nothing
            End If
            Me.btnCheckAll.Enabled = True
            Me.btnUnCheck.Enabled = True
        End Sub

        Private Function fncChkAuth(ByVal bCheckToken As Boolean) As Boolean
            Dim strDbAuthCode As String
            Dim bIsAuthCode As Boolean
            Dim clsCommon As New clsCommon
            Dim intAttempts As Integer

            'Check Session Value for Authorization Lock - Start
            If Not IsNumeric(Session("AUTH_LOCK")) Or Session("AUTH_LOCK") = 0 Then
                Session("AUTH_LOCK") = 0
            End If
            'Check Session Value for Authorization Lock - Stop

            'Check If AuthCode is Valid - Start
            strDbAuthCode = clsCommon.fncPassAuth(ss_lngUserID, "A", ss_lngOrgID)

            'If User Role is Authorizer, should not validate the Authorization Code.
            If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                bIsAuthCode = bCheckToken
            Else
                'Check If AuthCode is Valid - Stop
                bIsAuthCode = IIf(strDbAuthCode = txtAuthCode.Text, True, False)
            End If




            'Check for invalid Authorization Code Attempts - START
            If Not bIsAuthCode Then
                intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                If Not intAttempts = 2 Then
                    If Not bIsAuthCode Then
                        intAttempts = intAttempts + 1
                        Session("AUTH_LOCK") = intAttempts
                        If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                            lblMessage.Text = "Token is invalid, Please plugin a valid Token."
                        Else
                            lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                        End If

                        Return False
                    End If
                ElseIf intAttempts = 2 Then
                    If Not bIsAuthCode Then
                        Dim strUserRole, strUserName, strSubject, strBody, strErrMsg As String
                        Dim clsUsers As New clsUsers
                        'Disbale Table Row
                        trSubmit.Visible = False
                        trConfirm.Visible = False
                        Me.trChallengeCode.Visible = False
                        Me.trDynaPin.Visible = False
                        'Get User Role
                        strUserRole = clsCommon.fncBuildContent("User Role", "", ss_lngUserID, ss_lngUserID)
                        'Get User Name
                        strUserName = clsCommon.fncBuildContent("User Name", "", ss_lngUserID, ss_lngUserID)
                        'Build Subject
                        strSubject = strUserName & " (" & strUserRole & ") Locked/Inactive."
                        'Build Body
                        strBody = strUserName & " (" & strUserRole & ")" & " has been Locked/Inactive on " & Now() & "due to Invalid Validation Code attempts. Please change the User Validation Code."
                        'Lock Authorization Code
                        Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "A")
                        'Send Mails
                        Call clsCommon.prcSendMails("USER LOCK", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                        'Track Auth Lock
                        Call clsUsers.prcLockHistory(ss_lngUserID, "A")
                        'Display Alert
                        If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                            strErrMsg = "Sorry! Cannot Approve the file as Your Validation Rights has been locked due to 3 times attempt of invalid Token. Please contact your " & gc_UT_SysAdminDesc & "."
                        Else
                            strErrMsg = "Sorry! Cannot Approve the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                        End If

                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileListAuth.aspx", False)
                        Return False
                    End If
                End If
            End If
            'Check for invalid Authorization Code Attempts - STOP

            Return True
        End Function
#End Region

#Region "Bind Datagrid"

        '****************************************************************************************************
        'Procedure Name : Bind_Grid()
        'Purpose        : DataGrid Bind
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Eu Yean Lock- 
        'Created        : 12/08/2006
        '*****************************************************************************************************
        Private Sub prcBindGrid()

            Dim dsFile As New System.Data.DataSet           'Create instance of System Data Set
            Dim clsGeneric As New MaxPayroll.Generic        'Create instance of Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon       'Create instance of Upload Class object

            'Variable Declarations
            Dim intRecordCount As Int32, lngOrgId As Long, lngUserId As Long
            Dim strErrMsg As String, lngGroupId As Long

            Try

                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'get organisation id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'get user id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)  'get group id

                'Populate Data Grid
                dsFile = clsCommon.fncFileGrid("A", lngOrgId, lngUserId, lngGroupId)
                intRecordCount = dsFile.Tables(0).Rows.Count
                If intRecordCount > 0 Then
                    dgFile.Visible = True
                    trSubmit.Visible = True
                    btnAccept.Enabled = True
                    trAuthCode.Visible = False
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    btnCheckAll.Visible = True
                    btnUnCheck.Visible = True
                    dgFile.DataSource = dsFile
                    dgFile.DataBind()
                    pnlGrid.Visible = True
                Else
                    pnlGrid.Visible = False
                    dgFile.Visible = False
                    trSubmit.Visible = False
                    trAuthCode.Visible = False
                    trConfirm.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    btnCheckAll.Visible = False
                    btnUnCheck.Visible = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    'get error message
                    strErrMsg = Request.QueryString("Err")
                    'if error message from file authorize
                    If Not strErrMsg = "" Then
                        lblMessage.Text = strErrMsg & "<BR>" & "No Files Available For Approve."

                    Else    'if no error message from file authorize
                        lblMessage.Text = "No Files Available For Approve."
                    End If
                End If

                'Populate Datagrid - STOP

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "prcBindGrid - PG_FileList", Err.Number, Err.Description)

            Finally

                'destroy instance of system data set
                dsFile = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Upload Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region

#Region "Page Navigation"

        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try

                intStart = dgFile.CurrentPageIndex * dgFile.PageSize
                dgFile.CurrentPageIndex = E.NewPageIndex
                Call prcBindGrid()

            Catch ex As Exception

            End Try

        End Sub

#End Region

        '#Region "Back To View"

        '    Private Sub prView(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnView.Click
        '        Try
        '            Server.Transfer("PG_FileAuthorize.aspx", False)
        '        Catch ex As Exception

        '        End Try

        '    End Sub

        '#End Region




        Protected Sub btnSign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSign.Click
            If fncTokenAuthenticate(txtDynaPin.Text) Then
                prcConfirmAction(Session(gc_Ses_Token))
                'Me.lblMessage.Text = "authentication passed"
            Else

                prcTokenStatus()
                Me.lblMessage.Text = "Token Authentication Failed.  Please regenerate the new Dyna Pin by entering the new Challenge Code."
            End If
        End Sub

        Private Sub prcTokenStatus()
            If Me.fncRequestChallengeCode(Me.txtChallengeCode.Text) = False Then
                lblMessage.Text = txtChallengeCode.Text
                Me.trChallengeCode.Visible = False
                Me.trDynaPin.Visible = False
                btnSign.Enabled = False
            Else
                lblMessage.Text = ""
                Me.trChallengeCode.Visible = True
                Me.trDynaPin.Visible = True
                txtDynaPin.Text = ""
                btnSign.Enabled = True
            End If
        End Sub

        Protected Sub dgFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFile.ItemDataBound

            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim lblaFileType As New Label
                    lblaFileType = CType(e.Item.FindControl("lblaFileType"), Label)


                    Select Case e.Item.Cells(enmDgItem.FileType).Text.Trim
                        Case "Mandate File"
                            lblaFileType.Text = "<a id=""ancFileType"" href=""PG_MandateFileDetails.aspx?Type=" & enmMandateFileAction.Approve.ToString & _
                                "&Id=" & e.Item.Cells(enmDgItem.FileId).Text & "&WfId=" & e.Item.Cells(enmDgItem.WorkFlowID).Text & "&FN=" _
                                    & e.Item.Cells(enmDgItem.FileName).Text & "&VD=" & e.Item.Cells(enmDgItem.PaymentDate).Text & _
                                    "&FTID=" & e.Item.Cells(enmDgItem.FileType_Id).Text & "&SID=" & e.Item.Cells(enmDgItem.Service_Id).Text & _
                                        "&FT=" & e.Item.Cells(enmDgItem.FileType).Text & """>" & e.Item.Cells(enmDgItem.FileType).Text & "</a>"
                        Case Else
                            lblaFileType.Text = "<a id=""ancFileType"" href=""PG_FileAuth.aspx?Id=" & e.Item.Cells(enmDgItem.FileId).Text & _
                                  "&FTID=" & e.Item.Cells(enmDgItem.FileType_Id).Text & "&SID=" & e.Item.Cells(enmDgItem.Service_Id).Text & _
                                "&WfId=" & e.Item.Cells(enmDgItem.WorkFlowID).Text & "&FN=" & e.Item.Cells(enmDgItem.FileName).Text & "&VD=" _
                                        & e.Item.Cells(enmDgItem.PaymentDate).Text & "&FT=" & e.Item.Cells(enmDgItem.FileType).Text & """>" & _
                                            e.Item.Cells(enmDgItem.FileType).Text & "</a>"
                    End Select
                    If e.Item.Cells(enmDgItem.FileType).Text.Trim = _Helper.CPS_Name Then
                        e.Item.Cells(enmDgItem.FileName).Text = e.Item.Cells(enmDgItem.FileName).Text.Substring(e.Item.Cells(enmDgItem.FileName).Text.LastIndexOf(".") - 7)
                    End If

            End Select
        End Sub
    End Class

End Namespace
