Imports MaxPayroll.Generic
Imports MaxPayroll.clsUpload
Imports MaxMiddleware
Imports MaxReadWrite


Namespace MaxPayroll

    Partial Class PG_FileList
        Inherits clsBasePage

#Region " Global declarations "

        Private _Helper As New Helper
        Private _CPSPhase3 As New clsCPSPhase3
        Private _ReadWriteGeneric As New MaxReadWrite.Generic
#End Region

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
            FileType_Id = 11
            Service_Id = 12
        End Enum


#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 12/08/2006
        '*****************************************************************************************************

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Dim dsFile As New System.Data.DataSet           'Create instance of System Data Set
            Dim clsGeneric As New MaxPayroll.Generic        'Create instance of Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon       'Create instance of Upload Class object

            Dim strAuthLock As String
            Try
                'BindBody(body)
                'check if only reviewer
                If Not ss_strUserType.Equals(gc_UT_Reviewer) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                'Disable Button Command on Click
                Call clsCommon.fncBtnDisable(btnConfirm, True)

                'Get Authorization Lock Status - Start

                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    trSubmit.Enabled = False
                    trConfirm.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                End If

                'Get Authorization Lock Status - Stop

                If Not Page.IsPostBack Then
                    'Bind Data Grid
                    Call prcBindGrid()
                    txtalert.Visible = False
                End If

                Me.txtAuthCode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + Me.btnConfirm.ClientID + "').click();return false;}} else {return true}; ")

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page Load - Pg_FileList", Err.Number, Err.Description)

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
            Dim intCounter As Int16, strRemarks As String, bIsChecked As Boolean, bIsRemarkFilled As Boolean = True

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
                        bIsChecked = True
                        txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)               'Get Remarks Texbox
                        strRemarks = txtRemarks.Text                                                    'Get Remarks Textbox

                        If strRemarks = "" Then
                            bIsChecked = True
                            txtRemarks.Enabled = True
                            strMessage = strMessage & "Row " & intCounter & " has been selected for Rejected. Please enter Remarks.<BR>"
                            bIsRemarkFilled = False
                        End If

                    End If
                    'Check if Remark Provided - STOP


                    'Check File Already Submitted - START
                    strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", strFileid, lngUserId)
                    If Not strFileStatus = "" Then
                        lblMessage.Text = "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus
                    End If
                    'Check File Already Submitted - STOP

                    intCounter = intCounter + 1

                Next

                If Not bIsChecked Then
                    trSubmit.Visible = True
                    btnAccept.Enabled = True
                    trConfirm.Visible = False
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
                    trAuthCode.Visible = True
                    tblForm.Visible = True
                    trSubmit.Visible = False
                    trConfirm.Visible = True
                    txtRemarks.ReadOnly = True
                    lblHeading.Text = "File Review - Confirmation"
                    lblMessage.Text = "Please Enter your Validation Code and Confirm File Rejection."
                Else
                    trSubmit.Visible = True
                    btnAccept.Enabled = False
                    trConfirm.Visible = False
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
        'Purpose        : To Update the Status to Review
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

            Try
                lblMessage.Text = ""
                txtalert.Text = ""

                'Get User Group

                For Each dgiFileRev In dgFile.Items

                    strFileid = dgiFileRev.Cells(2).Text
                    'Get File Id
                    strFileType = dgiFileRev.Cells(3).Text
                    'Get File Type
                    strGivenName = dgiFileRev.Cells(4).Text
                    'Get File Name
                    If IsDate(dgiFileRev.Cells(5).Text) Then
                        hdnpymtdt.Value = CDate(dgiFileRev.Cells(5).Text)
                    Else
                        hdnpymtdt.Value = CDate(dgiFileRev.Cells(7).Text)
                    End If
                    If IsNumeric(dgiFileRev.Cells(9).Text) Then
                        hdnTAmount.Value = dgiFileRev.Cells(9).Text
                    Else
                        hdnTAmount.Value = 0
                    End If

                    chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)
                    'Get Check Box
                    lblFileType = CType(dgiFileRev.FindControl("lblFileType"), Label)
                    lblaFileType = CType(dgiFileRev.FindControl("lblaFileType"), Label)

                    'Check if Checkbox Checked - START
                    If chkSelect.Checked Then
                        IsChecked = True
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
                        lblMessage.Text = "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus
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

                            If strFileType = _Helper.Autopay_Name OrElse strFileType = "Payroll File" OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                'txtalert.Text = txtalert.Text & " (" & strGivenName & ") with the same Payment Date & Total Amount has already been reviewed by you. Please check the File Status Report or contact Customer Relations at " & gc_Const_CompanyContactNo & "."
                                txtalert.Text = txtalert.Text & " with the same Payment Date & Total Amount has already been reviewed by you. Please check the File Status Report or contact Customer Relations at " & gc_Const_CompanyContactNo & "."
                            Else
                                'txtalert.Text = txtalert.Text & " (" & strGivenName & ") with the same Upload Date & Total Amount has already been reviewed by you. Please check the File Status Report or contact Customer Relations at " & gc_Const_CompanyContactNo & "."
                                txtalert.Text = txtalert.Text & " with the same Upload Date & Total Amount has already been reviewed by you. Please check the File Status Report or contact Customer Relations at " & gc_Const_CompanyContactNo & "."
                            End If

                        End If
                    End If
                    'Check if Same Payment Date & Same Amount has already been Authorized - Stop
                Next

                If Not IsChecked Then
                    trSubmit.Visible = True
                    trConfirm.Visible = False
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
                trAuthCode.Visible = True
                tblForm.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                lblHeading.Text = "File Review - Confirmation"
                lblMessage.Text = "Please Enter your Validation Code and Confirm File Review."

            Catch

                'Log Error
                LogError("Page_Accept - PG_FileList")

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
        'Purpose        : To Update the Status to Review
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 10/08/2006
        '*****************************************************************************************************

        Private Sub prcReviewFile(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

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
            Dim strErrMsg As String = Nothing, sMsg As String = "", sAlert As String = ""
            Dim intRecordCount As Int32 = 0, strGivenName As String = Nothing, strFileType As String = Nothing
            Dim strFileStatus As String = Nothing, strFileName As String = Nothing, bIsAuthCode As Boolean = True
            Dim strReason As String = Nothing, strUserIP As String = Nothing, IsCutoff As Boolean = False
            Dim strDbAuthCode As String = Nothing, intAttempts As Int16 = 0, strTime As String = ""
            Dim strOption As String = "", strUserRole As String = Nothing, strUserName As String = Nothing
            Dim strSubject As String = Nothing, strBody As String = Nothing, intFlowCount As Int16 = 0
            Dim strFileid As String = Nothing, strWorkFlowId As String = Nothing, FileType_Id As Short = 0
            Dim Service_Id As Short = 0, ErrorDetails As String = Nothing, SqlStatement As String = Nothing
            Dim ServiceType As Short = 0
            ''Declare For Member File Start
            Dim DivFileId As Long
            Dim FileGivenName As String
            Dim lngSubFlowId As Long
            Dim ErrorMsg As String
            ''Member File Stop
            bIsAuthCode = fncChkAuth()

            strBody = ""
            strSubject = ""
            lblMessage.Text = ""                                               'Clear the label message
            strUserIP = Request.ServerVariables("REMOTE_ADDR")                 'User IP Address
            If bIsAuthCode Then


                For Each dgiFileRev In dgFile.Items
                    Try

                        strFileid = dgiFileRev.Cells(2).Text                               'Get File Id
                        strFileType = dgiFileRev.Cells(3).Text                             'Get File Type
                        strGivenName = dgiFileRev.Cells(4).Text                            'Get File Name
                        If IsDate(dgiFileRev.Cells(5).Text) Then
                            hdnpymtdt.Value = CDate(dgiFileRev.Cells(5).Text)                         'Get Payment Date
                        Else
                            hdnpymtdt.Value = CDate(dgiFileRev.Cells(7).Text)
                        End If

                        strWorkFlowId = dgiFileRev.Cells(6).Text                                'Get WorkFlow Id
                        If IsNumeric(dgiFileRev.Cells(9).Text) Then
                            hdnTAmount.Value = dgiFileRev.Cells(9).Text                         'Get Total Amount
                        Else
                            hdnTAmount.Value = 0
                        End If

                        txtRemarks = CType(dgiFileRev.FindControl("txtRemarks"), TextBox)   'Get Remarks Texbox
                        strReason = IIf(txtRemarks.Text = "", "NA", txtRemarks.Text)        'Reason/Remarks
                        chkSelect = CType(dgiFileRev.FindControl("chkSelect"), CheckBox)    'Get Check Box
                        FileType_Id = MaxGeneric.clsGeneric.NullToShort(dgiFileRev.Cells(11).Text)
                        Service_Id = MaxGeneric.clsGeneric.NullToShort(dgiFileRev.Cells(12).Text)

                        'Get service type based on serviceId -start
                        If Service_Id > 0 Then
                            SqlStatement = _Helper.GetSQLCommon & "'ServiceType'," & Service_Id

                            ServiceType = MaxGeneric.clsGeneric.NullToShort(MaxMiddleware.PPS.SQLScalarValue( _
                                             _Helper.GetSQLConnection, _Helper.GetSQLTransaction, String.Empty, SqlStatement))
                        End If
                        'Get service type based on serviceId -stop

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
                            strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", strFileid, ss_lngUserID)
                            If Not strFileStatus = "" Then

                                lblMessage.Text += "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus & gc_BR


                                Exit Try
                            End If
                            'Check File Already Submitted - STOP



                            'CheckSession cutoff time-start
                            If FileType_Id > 0 And Service_Id > 0 And Not ServiceType = Helper.ServiceType.Mandates Then

                                If _ReadWriteGeneric.IsCutoffTime(FileType_Id, hdnpymtdt.Value, Service_Id, ErrorDetails, strTime) Then

                                    IsCutoff = True

                                End If
                            End If
                            'CheckSession cutoff time-stop

                            'Check Cutoff Time - Start
                            If hdncommand.Value = "A" Then

                                'Get Table Name - START
                                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                                    'Check If Privilege Customer
                                    strOption = clsCommon.fncBuildContent("Privilege", "", ss_lngOrgID, ss_lngUserID)                                               'Get Privilege User
                                ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                                    strOption = "E"
                                ElseIf strFileType = "SOCSO File" Then
                                    strOption = "S"
                                ElseIf strFileType = "LHDN File" Then
                                    strOption = "L"
                                ElseIf strFileType = _Helper.DirectDebit_Name Then
                                    strOption = "D"
                                ElseIf strFileType = _Helper.Autopay_Name Or strFileType = _Helper.AutopaySNA_Name Then
                                    strOption = clsCommon.fncBuildContent("Privilege", "", ss_lngOrgID, ss_lngUserID)
                                ElseIf strFileType = _Helper.CPS_Name Then
                                    strOption = "C"
                                ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                                    strOption = "I"
                                End If
                                'Get Table Name - STOP

                                'Check If File Reached Cutoff Time
                                If strFileType = "Payroll File" Or strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.PayLinkPayRoll_Name Then

                                    IsCutoff = clsCommon.fncCutoffTime(strFileType, ss_lngOrgID, ss_lngUserID, strTime, _
                                           Day(hdnpymtdt.Value), Month(hdnpymtdt.Value), Year(hdnpymtdt.Value), strOption)

                                End If
                                If IsCutoff Then
                                    'Build Subject
                                    strSubject = strFileType & " Review Failed - " & strGivenName & " Payment Date: " & hdnpymtdt.Value

                                    'Build Body
                                    strBody = "The " & strFileType & "(" & strGivenName & ") for Payment Date " & hdnpymtdt.Value & _
                                        " cannot be Reviewed after the Cutoff Time (" & strTime & ")."

                                    lblMessage.Text += strBody & gc_BR
                                    'Block File
                                    Call clsCommon.prcBlockFile(strFileid, 4, ss_lngOrgID, ss_lngUserID)
                                    'Get File Name
                                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, ss_lngUserID)
                                    'Delete File
                                    'Move the file to Rejected Folder
                                    'Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                                    clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                                    'Send Mails
                                    Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                                    'Display Alert

                                    Exit Try
                                End If

                            Else

                                'Get Table Name - START
                                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                                    'Check If Privilege Customer
                                    strOption = clsCommon.fncBuildContent("Privilege", "", ss_lngOrgID, ss_lngUserID)                                               'Get Privilege User
                                ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                                    strOption = "E"
                                ElseIf strFileType = "SOCSO File" Then
                                    strOption = "S"
                                ElseIf strFileType = "LHDN File" Then
                                    strOption = "L"
                                ElseIf strFileType = _Helper.DirectDebit_Name Then
                                    strOption = "D"
                                ElseIf strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                    strOption = clsCommon.fncBuildContent("Privilege", "", ss_lngOrgID, ss_lngUserID)
                                ElseIf strFileType = _Helper.CPS_Name Then
                                    strOption = "C"
                                ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                                    strOption = "I"
                                End If
                                'Get Table Name - STOP

                                'Check If File Reached Cutoff Time
                                If strFileType = "Payroll File" OrElse strFileType = _Helper.Autopay_Name _
                                    OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name _
                                        OrElse strFileType = _Helper.PayLinkPayRoll_Name Then

                                    IsCutoff = clsCommon.fncCutoffTime(strFileType, ss_lngOrgID, ss_lngUserID, strTime, _
                                                        Day(hdnpymtdt.Value), Month(hdnpymtdt.Value), Year(hdnpymtdt.Value), strOption)
                                End If

                                If IsCutoff Then
                                    'Build Subject
                                    strSubject = strFileType & " Reject Failed - " & strGivenName & " Payment Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & "(" & strGivenName & ") for Payment Date " & hdnpymtdt.Value & " cannot be Rejected after the Cutoff Time (" & strTime & ").<br>Note: The file has been blocked."
                                    lblMessage.Text += strBody & gc_BR
                                    'Block File
                                    Call clsCommon.prcBlockFile(strFileid, 4, ss_lngOrgID, ss_lngUserID)
                                    'Get File Name
                                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, ss_lngUserID)
                                    'Delete File
                                    'Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                                    Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                                    'Send Mails
                                    Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                                    'Display Alert

                                    Exit Try
                                End If
                            End If
                            'Check Cutoff Time - Stop

                            'Check File Value Date Expired - Start
                            If strFileType = "Payroll File" Or strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                If hdnpymtdt.Value < Today Then
                                    'Build Subject
                                    strSubject = strFileType & " Review Failed - " & strGivenName & " Payment Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The Payment Date " & "(" & hdnpymtdt.Value & ") for the " & strFileType & "(" & strGivenName & ") has expired. Please rename and upload the file again with a future Payment Date."
                                    sAlert += strBody & "\n"
                                    lblMessage.Text += strBody & gc_BR
                                    'Block File
                                    Call clsCommon.prcBlockFile(strFileid, 4, ss_lngOrgID, ss_lngUserID)
                                    'Get File Name
                                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, ss_lngUserID)
                                    'Delete File
                                    Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                                    'Send Mails
                                    Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                                    'Display Alert

                                    'Exit Try
                                End If
                            End If
                            'Check File Value Date Expired - Stop

                            'Accept/Reject File - START
                            If hdncommand.Value = "A" Then

                                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                    'Build Subject
                                    strSubject = "The " & strFileType & " Reviewed - " & strGivenName & ", Payment Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & " with Payment Date " & hdnpymtdt.Value & " Reviewed Successfully. Remarks: " & strReason
                                Else
                                    strSubject = "The " & strFileType & " Reviewed - " & strGivenName & ", Upload Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & " with Upload Date " & hdnpymtdt.Value & " Reviewed Successfully. Remarks: " & strReason
                                End If
                                'Update Remarks/Reason
                                Call clsCommon.fncRemarks(strFileid, strReason)

                                'Update WorkFlow
                                Call clsCommon.prcWorkFlow(strFileid, strWorkFlowId, ss_lngOrgID, ss_lngUserID, "R", "N", strReason, strUserIP, 0, ss_lngGroupID, "Y")

                                ''Update Dividend File Start.
                                If strFileType = _Helper.CPSMember_Name Then
                                    Call clsCommon.fncRemarks(DivFileId, strReason)
                                    Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, ss_lngOrgID, ss_lngUserID, "R", "N", strReason, strUserIP, 0, ss_lngGroupID, "Y")

                                End If
                                ''Update Dividend File Stop.

                                If ((strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.CPSMember_Name _
                                            OrElse strFileType = _Helper.CPSDividen_Name) AndAlso hdnpymtdt.Value >= Today) Then
                                    'Get Reviewer Count
                                    intFlowCount = clsUsers.fnRoleCount("REVIEWER", "A", "PENDING", strFileid, ss_lngGroupID)

                                    If intFlowCount = 0 Then
                                        lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Payment Date: " & hdnpymtdt.Value & " has been Reviewed Successfully." & gc_BR
                                    Else
                                        lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Payment Date: " & hdnpymtdt.Value & " has been Reviewed Successfully." & gc_BR & "Please Note: There are " & intFlowCount & " more " & gc_UT_ReviewerDesc & "(s) to Review the file." & gc_BR
                                    End If
                                ElseIf Not (strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.CPSMember_Name _
                                            OrElse strFileType = _Helper.CPSDividen_Name) Then

                                    intFlowCount = clsUsers.fnRoleCount("REVIEWER", "A", "PENDING", strFileid, ss_lngGroupID)

                                    If intFlowCount = 0 Then
                                        lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & hdnpymtdt.Value & " has been Reviewed Successfully." & gc_BR
                                    Else
                                        lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & hdnpymtdt.Value & " has been Reviewed Successfully." & gc_BR & "Please Note: There are " & intFlowCount & " more " & gc_UT_ReviewerDesc & "(s) to Review the file." & gc_BR
                                    End If
                                End If


                            ElseIf hdncommand.Value = "R" Then

                                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.CPSMember_Name _
                                            OrElse strFileType = _Helper.CPSDividen_Name Then
                                    'Build Subject
                                    strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", Payment Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & "(" & strGivenName & ") with Payment Date " & hdnpymtdt.Value & " Rejected Successfully. Remarks: " & strReason
                                Else
                                    'Build Subject
                                    strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", Upload Date: " & hdnpymtdt.Value
                                    'Build Body
                                    strBody = "The " & strFileType & "(" & strGivenName & ") with Upload Date " & hdnpymtdt.Value & " Rejected Successfully. Remarks: " & strReason
                                End If
                                'Update Remakrs/Reason
                                Call clsCommon.fncRemarks(strFileid, strReason)

                                'Get File Name
                                strFileName = clsCommon.fncBuildContent("File Name", strFileType, strFileid, ss_lngUserID)

                                'Move the rejected file to rejected folder
                                Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)

                                'Update Workflow
                                Call clsCommon.prcWorkFlow(strFileid, strWorkFlowId, ss_lngOrgID, ss_lngUserID, "R", "Y", strReason, strUserIP, 0, ss_lngGroupID, "Y")

                                'Call clsCommon.prcDeleteRejTrans(strFileid, ss_lngOrgID, ss_lngUserID, strFileType)

                                ''Added for Member File Start
                                If strFileType = _Helper.CPSMember_Name Then
                                    Dim strDivFileName As String = ""
                                    strDivFileName = clsCommon.fncBuildContent("File Name", _Helper.CPSDividen_Name, DivFileId, ss_lngUserID)
                                    Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, _Helper.CPSDividen_Name, strDivFileName)
                                    Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, ss_lngOrgID, ss_lngUserID, "R", "Y", strReason, strUserIP, 0, ss_lngGroupID, "Y")
                                End If
                                ''Added for Member File Stop


                                'Display Message
                                If (strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                                    OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                                        OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.CPSMember_Name _
                                            OrElse strFileType = _Helper.CPSDividen_Name) AndAlso hdnpymtdt.Value > Today Then
                                    lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Payment Date: " & hdnpymtdt.Value & " has been Rejected Successfully." & gc_BR
                                ElseIf Not (strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name) Then
                                    lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & hdnpymtdt.Value & " has been Rejected Successfully." & gc_BR
                                End If


                                'Accept/Reject File - STOP
                            End If

                            'Send Mails
                            If (strFileType = "Payroll File" AndAlso hdnpymtdt.Value >= Today) OrElse _
                            (strFileType = _Helper.DirectDebit_Name AndAlso hdnpymtdt.Value >= Today) OrElse _
                            (strFileType = _Helper.Autopay_Name AndAlso hdnpymtdt.Value >= Today) OrElse _
                            (strFileType = _Helper.CPS_Name AndAlso hdnpymtdt.Value >= Today) OrElse _
                            (strFileType = _Helper.AutopaySNA_Name AndAlso hdnpymtdt.Value >= Today) OrElse _
                                (strFileType = _Helper.CPSMember_Name AndAlso hdnpymtdt.Value >= Today) OrElse _
                                (strFileType = _Helper.CPSDividen_Name AndAlso hdnpymtdt.Value >= Today) OrElse _
                            (strFileType <> "Payroll File" AndAlso strFileType <> _Helper.DirectDebit_Name AndAlso strFileType <> _Helper.Autopay_Name AndAlso strFileType <> _Helper.CPS_Name AndAlso strFileType <> _Helper.AutopaySNA_Name) Then

                                Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)

                            End If

                        End If



                    Catch

                        'Error  Message
                        lblMessage.Text = "File Review/Reject Failed."

                        'Log Error
                        clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "prcReviewFile - PG_FileList", Err.Number, Err.Description)

                    Finally


                    End Try
                Next


                If bIsAuthCode Then
                    'Populate Data Grid
                    dsFile = clsCommon.fncFileGrid("R", ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                    intRecordCount = dsFile.Tables(0).Rows.Count    'get record count
                    btnCheckAll.Enabled = True
                    btnUnCheck.Enabled = True

                    If intRecordCount > 0 Then
                        txtalert.Visible = False
                        txtalert.Text = String.Empty
                        dgFile.Visible = True
                        trSubmit.Visible = True
                        trAuthCode.Visible = False
                        trConfirm.Visible = False
                        btnCheckAll.Visible = True
                        btnUnCheck.Visible = True
                        dgFile.DataSource = dsFile
                        dgFile.DataBind()
                    Else
                        txtalert.Visible = False
                        txtalert.Text = String.Empty
                        dgFile.Visible = False
                        trSubmit.Visible = False
                        trAuthCode.Visible = False
                        trConfirm.Visible = False
                        btnCheckAll.Visible = False
                        btnUnCheck.Visible = False
                    End If
                    'Populate Datagrid - STOP
                    If Len(lblMessage.Text.Trim) > 0 Then
                        prcBindGrid(True)
                    Else
                        prcBindGrid()
                    End If

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

            Else
                lblMessage.Text = "You have entered an invalid validation code."
            End If
        End Sub


        Private Function fncChkAuth() As Boolean
            Dim strDbAuthCode As String
            Dim IsAuthCode As Boolean
            Dim clsCommon As New clsCommon
            Dim intAttempts As Integer


            'Check Session Value for Authorization Lock - Start
            If Not IsNumeric(Session("AUTH_LOCK")) Or Session("AUTH_LOCK") = 0 Then
                Session("AUTH_LOCK") = 0
            End If
            'Check Session Value for Authorization Lock - Stop

            'Check If AuthCode is Valid - Start
            strDbAuthCode = clsCommon.fncPassAuth(ss_lngUserID, "A", ss_lngOrgID)
            IsAuthCode = IIf(strDbAuthCode = txtAuthCode.Text, True, False)
            'Check If AuthCode is Valid - Stop

            'Check for invalid Authorization Code Attempts - START
            If Not IsAuthCode Then
                intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                If Not intAttempts = 2 Then
                    If Not IsAuthCode Then
                        intAttempts = intAttempts + 1
                        Session("AUTH_LOCK") = intAttempts
                        lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                        Return False
                    End If
                ElseIf intAttempts = 2 Then
                    If Not IsAuthCode Then
                        'Disbale Table Row
                        trSubmit.Visible = False
                        trConfirm.Visible = False
                        'Get User Role
                        Dim strUserRole, strUserName, strSubject, strBody, strErrMsg As String
                        Dim clsUsers As New clsUsers

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
                        strErrMsg = "Sorry! cannot Review the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
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
        Private Sub prcBindGrid(Optional ByVal bMsgExisted As Boolean = False)

            Dim dsFile As New System.Data.DataSet           'Create instance of System Data Set
            Dim clsGeneric As New MaxPayroll.Generic        'Create instance of Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon       'Create instance of Upload Class object

            'Variable Declarations
            Dim intRecordCount As Int32
            Dim strErrMsg As String

            Try

                'Populate Data Grid
                dsFile = clsCommon.fncFileGrid("R", ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                intRecordCount = dsFile.Tables(0).Rows.Count    'get record count
                If intRecordCount > 0 Then
                    Me.dvGridScroll.Visible = True
                    dgFile.Visible = True
                    trSubmit.Visible = True
                    btnAccept.Enabled = True
                    trAuthCode.Visible = False
                    trConfirm.Visible = False
                    btnCheckAll.Visible = True
                    btnUnCheck.Visible = True
                    dgFile.DataSource = dsFile
                    dgFile.DataBind()
                Else
                    Me.dvGridScroll.Visible = False
                    dgFile.Visible = False
                    trSubmit.Visible = False
                    trAuthCode.Visible = False
                    trConfirm.Visible = False
                    btnCheckAll.Visible = False
                    btnUnCheck.Visible = False
                    'get error message
                    strErrMsg = Request.QueryString("Err") & ""
                    'if error message from file review
                    If bMsgExisted = False Then
                        If Not strErrMsg = "" Then
                            lblMessage.Text = strErrMsg & gc_BR & "No Files Available For Review."
                        Else    'if no error message from file review
                            lblMessage.Text = "No Files Available For Review."
                        End If
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

        Protected Sub dgFile_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFile.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim lblaFileType As New Label
                    lblaFileType = CType(e.Item.FindControl("lblaFileType"), Label)

                    If e.Item.Cells(enmDgItem.FileType).Text.Trim = _Helper.CPS_Name Then
                        e.Item.Cells(enmDgItem.FileName).Text = e.Item.Cells(enmDgItem.FileName).Text.Substring(e.Item.Cells(enmDgItem.FileName).Text.LastIndexOf(".") - 7)
                    End If

                    If e.Item.Cells(enmDgItem.FileType).Text.Trim = "Mandate File" Then
                        lblaFileType.Text = "<a id=""ancFileType"" href=""PG_MandateFileDetails.aspx?Type=" & enmMandateFileAction.Review.ToString & _
                        "&FTID=" & e.Item.Cells(enmDgItem.FileType_Id).Text & "&SID=" & e.Item.Cells(enmDgItem.Service_Id).Text & _
                            "&Id=" & e.Item.Cells(enmDgItem.FileId).Text & "&WfId=" & e.Item.Cells(enmDgItem.WorkFlowID).Text & "&FN=" & _
                                e.Item.Cells(enmDgItem.FileName).Text & "&VD=" & e.Item.Cells(enmDgItem.PaymentDate).Text & "&FT=" & _
                                    e.Item.Cells(enmDgItem.FileType).Text & """>" & e.Item.Cells(enmDgItem.FileType).Text & "</a>"
                    Else
                        lblaFileType.Text = "<a id=""ancFileType"" href=""PG_FileReview.aspx?Id=" & e.Item.Cells(enmDgItem.FileId).Text & _
                              "&FTID=" & e.Item.Cells(enmDgItem.FileType_Id).Text & "&SID=" & e.Item.Cells(enmDgItem.Service_Id).Text & _
                            "&WfId=" & e.Item.Cells(enmDgItem.WorkFlowID).Text & "&FN=" & e.Item.Cells(enmDgItem.FileName).Text & "&VD=" & _
                                e.Item.Cells(enmDgItem.PaymentDate).Text & "&FT=" & e.Item.Cells(enmDgItem.FileType).Text & """>" & _
                                    e.Item.Cells(enmDgItem.FileType).Text & "</a>"
                    End If

            End Select
        End Sub

    End Class


End Namespace
