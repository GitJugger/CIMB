Imports System
Imports System.IO
Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Reflection
Imports System.Data.SqlClient


Namespace MaxPayroll


    Partial Class PG_MandateFileDetails
        Inherits clsBasePage
        Private _Helper As New Helper


#Region "Declaration"
        Private ReadOnly Property rq_sType() As String
            Get
                Return Request.QueryString("Type") & ""
            End Get
        End Property
        Private ReadOnly Property strServerURL() As String
            Get
                Return Configuration.ConfigurationManager.AppSettings("ReportServerURL") & ""
            End Get
        End Property
        Private ReadOnly Property strReportDir() As String
            Get
                Return Configuration.ConfigurationManager.AppSettings("ReportDir") & ""
            End Get
        End Property

        Private ReadOnly Property rq_lngFileId() As Long
            Get
                If IsNumeric(Request.QueryString("ID")) Then
                    Return CLng(Request.QueryString("ID"))
                Else
                    Return -1
                End If
            End Get
        End Property

        Private ReadOnly Property lngOrgId() As Long
            Get
                If IsNumeric(Request.QueryString("OID")) Then
                    Return CLng(Request.QueryString("OID"))
                Else
                    Return ss_lngOrgID
                End If

            End Get
        End Property

        Private ReadOnly Property rq_sMode() As String
            Get
                Return Request.QueryString("Mode") & ""
            End Get
        End Property

        Enum enmDetailGridItem
            RefNo
            AccNo
            BankOrgCode
            CustomerName
            Cust_ICNumber
            LimitAmount
            Frequency
            FrequencyLimit
            ToUpdate
        End Enum
#End Region

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

            'Create Instance of Data Row
            Dim drFileReview As System.Data.DataRow

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of RAS Class Object
            'Dim clsRAS As New MaxPayroll.clsReportRAS

            'Create Instance of System Data Set
            Dim dsFileReview As New System.Data.DataSet

            'Create Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Custmer object 
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim strErrMsg As String = ""
            Dim IsCutoff As Boolean
            Dim lngFileId As Long
            Dim strTime As String = ""
            Dim dtValueDt As Date
            Dim strHeader5 As String = ""
            Dim strHeader6 As String = ""
            Dim intRecordCount As Int16
            Dim strAuthLock As String = ""
            Dim strSubject As String = ""
            Dim strBody As String
            Dim lngFormatId As Long
            Dim strFileStatus As String
            Dim strOption As String = ""
            Dim strHeader1 As String = ""
            Dim strHeader2 As String = ""
            Dim strHeader3 As String = ""
            Dim strFooter4 As String = ""
            Dim strFooter5 As String = ""
            Dim strFooter6 As String = ""
            Dim strFooter3 As String = ""
            Dim strFileType As String = ""
            Dim strFileName As String = ""
            Dim strGivenName As String = ""
            Dim intCheck As Int16
            Dim strHeader4 As String = ""
            Dim strTableName As String = ""
            Dim strFooter1 As String = ""
            Dim strFooter2 As String = ""
            Dim strFooter7 As String = ""
            Dim intDisplay As Int16
            lblMessage.Text = ""
            Try

                

                If Not Page.IsPostBack Then
                    'BindBody(body)
                    Select Case ss_strUserType
                        Case gc_UT_Reviewer, gc_UT_Auth, gc_UT_BankAuth
                        Case Else
                            Server.Transfer(gc_LogoutPath, False)
                    End Select

                    trBack.Visible = False                                                                  'Hide Table Row
                    If rq_sType = enmMandateFileAction.Review.ToString Then
                        lblHeading.Text = "File Review"
                        Me.iptBack.Value = "Review Another File"
                    Else
                        lblHeading.Text = "File Approval"
                        Me.iptBack.Value = "Approve Another File"
                    End If
                   
                   

                    'Set Heading
                    strFileType = Request.QueryString("FT")                                                 'Get File Type
                    strGivenName = Request.QueryString("FN")                                                'Get File Name
                    If IsDate(Request.QueryString("VD")) Then
                        dtValueDt = CDate(Request.QueryString("VD"))
                    End If
                    'Get Value Date


                    lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'Get File Id
                    hFileId.Value = lngFileId

                    'Disable Button Command on Click
                    Call clsCommon.fncBtnDisable(btnConfirm, True)

                    'Get Authorization Lock Status - Start
                    strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                    If strAuthLock = "Y" Then
                        Select Case rq_sType
                            Case enmMandateFileAction.Review.ToString
                                strErrMsg = "Sorry! cannot Review the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                            Case enmMandateFileAction.Approve.ToString
                                strErrMsg = "Sorry! cannot Approve the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                           

                        End Select

                        If Len(strErrMsg) > 0 Then
                            Response.Write("<script language='JavaScript'>")
                            Response.Write("alert('" & strErrMsg & "');")
                            Response.Write("</script>")
                            Server.Transfer("PG_FileList.aspx", False)
                        End If

                        Exit Try
                    End If
                    'Get Authorization Lock Status - Stop

                    'Check File Already Submitted - START
                    strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, ss_lngUserID)
                    If rq_sType <> enmMandateFileAction.BankAuthorize.ToString AndAlso Not strFileStatus = "" Then
                        strErrMsg = "The " & strFileType & " (" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") " & " has already been " & strFileStatus
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileList.aspx", False)
                        Exit Try
                    End If
                    'Check File Already Submitted - STOP

                    BindHeaderInfo()
                    BindDetailGrid()
                    trSubmit.Visible = True
                    trAuth.Visible = False
                    trConfirm.Visible = False

                    Select Case rq_sType
                        Case enmMandateFileAction.Review.ToString
                            lblHeading.Text = "File Review"
                            Me.iptBack.Value = "Review Another File"
                        Case enmMandateFileAction.Approve.ToString
                            lblHeading.Text = "File Approval"
                            Me.iptBack.Value = "Approve Another File"
                        Case enmMandateFileAction.BankAuthorize.ToString

                            If rq_sMode.ToLower = "edit" Then
                                lblHeading.Text = "Review for File Authorization"
                            Else
                                lblHeading.Text = "Mandate File Review"
                            End If

                            Me.trSubmit.Visible = False
                            trView.Visible = True
                            btnView.Text = "View Another File"
                    End Select


                    'Populate Remarks Data Grid - Start
                    dsFileReview = clsCommon.fncListRemarks(lngFileId, ss_lngOrgID, ss_lngUserID)
                    intRecordCount = dsFileReview.Tables("REMARKS").Rows.Count
                    If intRecordCount > 0 Then
                        dgRemarks.Visible = True
                        dgRemarks.DataSource = dsFileReview
                        dgRemarks.DataBind()
                    Else
                        dgRemarks.Visible = False
                        lblRemark.Text = "No Remarks History"
                    End If
                    'Populate Remarks Data Grid - Stop

                    If strFileType = "Mandate File" Then
                        strTableName = "tCor_MandatesFileDetails"
                    Else
                        strTableName = clsUpload.fncGetDBTableName(lngFormatId)
                    End If

                End If
            Catch ex As Exception
                If Err.Description <> "Thread was being aborted." Then

                    'Hide Tables



                    'Display Message
                    lblMessage.Text += "Sorry! Report Could Not be Loaded. Please try again."

                    'Log Error
                    clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page_Load - PG_FileReview", Err.Number, Err.Description)

                End If

            Finally

                'Destroy Instance of Data Row
                drFileReview = Nothing

                'Destroy Instance of Data Set
                dsFileReview = Nothing

                'Destroy report instance
                'clsRAS = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destory Generic Class Object
                clsGeneric = Nothing

                'Destory Customer Class Objec
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Load File"
        Private Sub BindHeaderInfo()
            Dim oItem As New clsMandates
            oItem = oItem.LoadFileHeader(rq_lngFileId)
            If oItem.paramFileName = "" Then
                lblMessage.Text += "Error in loading file header information."
            Else
                Me.lblFileName.Text = oItem.paramFileName
                Me.lblUploadBy.Text = oItem.paramCustomerName
                Me.lblUploadDate.Text = String.Format("{0:dd-MM-yyyy}", oItem.paramDoneDate)
                Me.lblTotalTransactions.Text = oItem.paramTotalTransaction
            End If
        End Sub
        Private Sub BindDetailGrid()
            Dim oItem As New clsMandates

            Me.dgMandateDetail.DataSource = oItem.LoadFileDetails(rq_lngFileId)
            dgMandateDetail.DataBind()

        End Sub

#End Region

#Region "Accept/Reject File"


        '****************************************************************************************************
        'Procedure Name : prcReviewFile
        'Purpose        : To Update the Status to Review
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Private Sub prcReviewFile(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strErrMsg As String
            Dim strFileType As String
            Dim strFileStatus As String
            Dim strFileName As String
            Dim strGivenName As String
            Dim lngUserId As Long
            Dim lngOrgId As Long
            Dim IsAuthCode As Boolean
            Dim lngFileId As Long
            Dim strDate As String
            Dim strReason As String
            Dim strUserIP As String
            Dim lngFlowId As Long
            Dim IsCutoff As Boolean
            Dim strDbAuthCode As String
            Dim intAttempts As Int16
            Dim strTime As String = ""
            Dim strOption As String = ""
            Dim lngGroupId As Long
            Dim strUserRole As String
            Dim strUserName As String
            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim intFlowCount As Int16

            Try

                strDate = lblUploadDate.Text                                                                                        'Get Value Date
                strFileType = Request.QueryString("FT")                                                                                     'Get File Type
                strGivenName = Request.QueryString("FN")                                                                                    'Get File Name
                strUserIP = Request.ServerVariables("REMOTE_ADDR")                                                                          'User IP Address
                strReason = IIf(txtReason.Text = "", "NA", txtReason.Text)                                                                  'Reason/Remarks
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                                                    'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)                                                 'Get User Id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)                                         'File Id  
                lngFlowId = IIf(IsNumeric(Request.QueryString("WfId")), Request.QueryString("WfId"), 0)                                     'WorkFlow Id

                'Check File Already Submitted - START
                strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, lngUserId)
                If Not strFileStatus = "" Then
                    strErrMsg = "The " & strFileType & " (" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") " & " has already been " & strFileStatus
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('" & strErrMsg & "');")
                    Response.Write("</script>")
                    Server.Transfer("PG_FileList.aspx", False)
                    Exit Try
                End If
                'Check File Already Submitted - STOP


                If MyBase.fncValidationCodeProcess(txtAuthCode.Text, lblMessage.Text) Then
                    'Accept/Reject File - START
                    Select Case rq_sType
                        Case enmMandateFileAction.Review.ToString
                            If hCommand.Value = "A" Then


                                'Build Subject
                                strSubject = "The " & strFileType & " Reviewed - " & strGivenName & ", Upload Date: " & strDate
                                'Build Body
                                strBody = "The " & strFileType & " with Upload Date " & strDate & " Reviewed Successfully. Remarks: " & strReason

                                'Update Remarks/Reason
                                Call clsCommon.fncRemarks(lngFileId, strReason)

                                'Update WorkFlow-- Added lngGroupId to arguments list as workflow is not getting updated if reviewed from details page
                                Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "R", "N", strReason, strUserIP, 0, lngGroupId, "Y")

                                'Get Reviewer Count
                                intFlowCount = clsUsers.fnRoleCount("REVIEWER", "A", "PENDING", lngFileId, lngGroupId)

                                If intFlowCount = 0 Then
                                    If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                        lblMessage.Text = "The " & strFileType & "(" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") with Payment Date: " & strDate & " has been Reviewed Successfully."
                                    Else
                                        lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Reviewed Successfully."
                                    End If
                                Else
                                    If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                        lblMessage.Text = "The " & strFileType & "(" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") with Payment Date: " & strDate & " has been Reviewed Successfully.<br> Please Note: There are " & intFlowCount & " more " & gc_UT_ReviewerDesc & "(s) to Review the file."
                                    Else
                                        lblMessage.Text = "The " & strFileType & "(" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") with Upload Date: " & strDate & " has been Reviewed Successfully.<br> Please Note: There are " & intFlowCount & " more " & gc_UT_ReviewerDesc & "(s) to Review the file."
                                    End If
                                End If

                            ElseIf hCommand.Value = "R" Then

                                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                    'Build Subject
                                    strSubject = "The " & strFileType & " Rejected - " & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ", Payment Date: " & strDate
                                    'Build Body
                                    strBody = "The " & strFileType & " with Payment Date " & strDate & " Rejected Successfully. Remarks: " & strReason
                                Else
                                    'Build Subject
                                    strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", Upload Date: " & strDate
                                    'Build Body
                                    strBody = "The " & strFileType & " with Upload Date " & strDate & " Rejected Successfully. Remarks: " & strReason
                                End If


                                'Update Remakrs/Reason
                                Call clsCommon.fncRemarks(lngFileId, strReason)

                                'Get File Name
                                strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, lngUserId)

                                'Delete File From FTP Folder
                                'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)

                                'Update Workflow
                                Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "R", "Y", strReason, strUserIP, 0, lngGroupId, "Y")

                                'Display Message
                                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                                    lblMessage.Text = "The " & strFileType & "(" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") with Payment Date: " & strDate & " has been Rejected Successfully."
                                Else
                                    lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Rejected Successfully."
                                End If


                            End If
                        Case enmMandateFileAction.Approve.ToString
                            'Update Remakrs/Reason
                            Call clsCommon.fncRemarks(lngFileId, strReason)

                            'Get File Name
                            strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, lngUserId)

                            'Reject File - START
                            If hCommand.Value = "R" Then
                                'Build Subject
                                strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", Upload Date: " & strDate
                                'Build Body
                                strBody = "The " & strFileType & " with Upload Date " & strDate & " Rejected Successfully. Remarks: " & strReason
                                'Delete File From FTP Folder
                                'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                'Marcus: Move the rejected file to backup Folder
                                ' Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                                'Update Workflow
                                Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "R", "Y", strReason, strUserIP, 0, 0, "Y")
                                'Display Message
                                lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Rejected Successfully."
                            End If
                            'Reject File - STOP

                            'Show Message - START
                            If hCommand.Value = "A" Then

                                'Update Workflow
                                Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "Y")

                                'Get Count of Balance Authorizers
                                intFlowCount = clsUsers.fnRoleCount("AUTHORIZER", "A", "PENDING", lngFileId, lngGroupId)

                                'Get Last Authorizer Submit File
                                If intFlowCount = 0 Then


                                    'Build Subject
                                    strSubject = "The " & strFileType & " Approved & Submitted - " & strGivenName & ", Upload Date: " & strDate & ", Submission Date: " & Now.ToShortDateString

                                    'Build Body
                                    strBody = "The " & strFileType & " with Upload Date: " & strDate & " Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & "<br>Remarks: " & strReason

                                    'Display Message
                                    lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString()

                                    'Update File Status
                                    lngFileId = clsCommon.fncFileDetails("FINAL", lngFileId, strFileType, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)
                                    Dim clsApprMatrix As New clsApprMatrix

                                    clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", lngFileId, lngUserId, 0, lngFileId, "Mandate File(" & strGivenName & ") Approval", "Mandate File Approval", "", 1)

                                Else

                                    'Build Subject
                                    strSubject = "The " & strFileType & " Approved - " & strGivenName & ", UploadDate: " & strDate

                                    'Build Body
                                    'strBody = "The " & strFileType & " with Payment Date " & strDate & " Approved Successfully. Remarks: " & strReason
                                    strBody = "The " & strFileType & " with Upload Date: " & strDate & " Approved Successfully. Remarks: " & strReason

                                    'Display Success Message and Balance No of Authorizers
                                    lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Approved Successfully.<BR>Please Note: " & intFlowCount & " more Approver(s) to Approve the file." & gc_BR
                                End If

                            End If
                            'Show Message - STOP

                            'Show/Hide Tablerow            
                            trAuth.Visible = False
                            trBack.Visible = True
                            trSubmit.Visible = False
                            trConfirm.Visible = False

                            'Send Mails 
                            Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                            Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserId, lngGroupId, strSubject, strBody, 0)



                        Case enmMandateFileAction.BankAuthorize.ToString
                            'Update Remakrs/Reason
                            Call clsCommon.fncRemarks(lngFileId, strReason)

                            'Get File Name
                            strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, lngUserId)

                            'Reject File - START
                            If hCommand.Value = "R" Then
                                'Build Subject
                                strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", Upload Date: " & strDate
                                'Build Body
                                strBody = "The " & strFileType & " with Upload Date " & strDate & " Rejected Successfully. Remarks: " & strReason
                                'Delete File From FTP Folder
                                'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                                'Marcus: Move the rejected file to backup Folder
                                ' Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                                'Update Workflow

                                'Display Message
                                lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Rejected Successfully."
                            End If
                            'Reject File - STOP

                            'Show Message - START
                            If hCommand.Value = "A" Then




                                'Build Subject
                                strSubject = "The " & strFileType & " Approved & Submitted - " & strGivenName & ", Upload Date: " & strDate & ", Submission Date: " & Now.ToShortDateString

                                'Build Body
                                strBody = "The " & strFileType & " with Upload Date: " & strDate & " Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & "<br>Remarks: " & strReason

                                'Display Message
                                lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with Upload Date: " & strDate & " has been Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString()

                                'Update File Status
                                lngFileId = clsCommon.fncFileDetails("FINAL", lngFileId, strFileType, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)
                                Dim clsApprMatrix As New clsApprMatrix

                                clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", lngFileId, lngUserId, 0, lngFileId, "Mandate File(" & strGivenName & ") Approval", "Mandate File Approval", "", 1)


                            End If
                            'Show Message - STOP

                            'Show/Hide Tablerow            
                            trAuth.Visible = False
                            trBack.Visible = True
                            trSubmit.Visible = False
                            trConfirm.Visible = False

                            'Send Mails 
                            Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                            Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserId, lngGroupId, strSubject, strBody, 0)
                    End Select

                    'Accept/Reject File - STOP

                    'Show/Hide Tablerow            
                    trAuth.Visible = False
                    trBack.Visible = True
                    trSubmit.Visible = False
                    trConfirm.Visible = False

                    'Send Mails
                    Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                End If

            Catch

                'Error  Message
                lblMessage.Text = "File Review/Reject Failed."

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcReviewFile - PG_FileReview", Err.Number, Err.Description)

            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Upload Class Object
                clsCommon = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

            End Try

        End Sub

#End Region

#Region "Reject Confirm"

        '****************************************************************************************************
        'Procedure Name : Page_Reject
        'Purpose        : To Reject the File
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Private Sub Page_Reject(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnReject.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim lngFileId As Long, strGivenName As String, strFileType As String
            Dim lngOrgId As Long, lngUserId As Long, strFileStatus As String, strErrMsg As String

            Try

                strFileType = Request.QueryString("FT")                                                 'Get File Type
                strGivenName = Request.QueryString("FN")                                                'Get File Name 
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'File Id  

                'Check File Already Submitted - START
                strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, lngUserId)
                If Not strFileStatus = "" Then
                    strErrMsg = "The " & strFileType & " (" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") " & " has already been " & strFileStatus
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('" & strErrMsg & "');")
                    Response.Write("</script>")
                    Server.Transfer("PG_FileList.aspx", False)
                    Exit Try
                End If
                'Check File Already Submitted - STOP

                'Check if Reason Provided - START
                If txtReason.Text = "" Then
                    lblMessage.Text = "Please enter Remarks for Rejection"
                    Exit Try
                End If
                'Check if Reason Provided - STOP

                hCommand.Value = "R"
                trAuth.Visible = True

                trSubmit.Visible = False
                trConfirm.Visible = True

                txtReason.ReadOnly = True

                lblHeading.Text = "File Review - Confirmation"
                lblMessage.Text = "Please Enter your Validation Code and Confirm File Rejection."

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Confirm - Page_Reject", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region

#Region "Accept Confirm"

        '****************************************************************************************************
        'Procedure Name : Page_Accept
        'Purpose        : To Update the Status to Review
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Private Sub Page_Accept(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnAccept.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim lngFileId As Long, strGivenName As String, strFileType As String
            Dim lngOrgId As Long, lngUserId As Long, strFileStatus As String, strErrMsg As String

            Try

                strFileType = Request.QueryString("FT")                                                 'Get File Type
                strGivenName = Request.QueryString("FN")                                                'Get File Name 
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'File Id  

                'Check File Already Submitted - START
                strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, lngUserId)
                If Not strFileStatus = "" Then
                    strErrMsg = "The " & strFileType & " (" & IIf(strFileType = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") " & " has already been " & strFileStatus
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('" & strErrMsg & "');")
                    Response.Write("</script>")
                    Server.Transfer("PG_FileList.aspx", False)
                    Exit Try
                End If
                'Check File Already Submitted - STOP

                hCommand.Value = "A"
                trAuth.Visible = True

                trSubmit.Visible = False
                trConfirm.Visible = True

                txtReason.ReadOnly = True

                If rq_sType = enmMandateFileAction.Review.ToString Then
                    lblHeading.Text = "File Review - Confirmation"
                    lblMessage.Text = "Please Enter your Validation Code and Confirm File Review."
                Else
                    lblHeading.Text = "File Approval - Confirmation"
                    lblMessage.Text = "Please Enter your Validation Code and Confirm File Approval."
                End If


            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Accept - PG_FileReview", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region

        Protected Sub dgMandateDetail_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgMandateDetail.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    e.Item.Cells(enmDetailGridItem.Frequency).Text = GetFrequencyDesc(e.Item.Cells(enmDetailGridItem.Frequency).Text)
                    If IsNumeric(e.Item.Cells(enmDetailGridItem.LimitAmount).Text.Trim) Then
                        e.Item.Cells(enmDetailGridItem.LimitAmount).Text = CDec(e.Item.Cells(enmDetailGridItem.LimitAmount).Text.Trim).ToString("N2")
                    Else
                        e.Item.Cells(enmDetailGridItem.LimitAmount).Text = "0"
                    End If

                    If e.Item.Cells(enmDetailGridItem.ToUpdate).Text = "True" Then
                        e.Item.Cells(enmDetailGridItem.ToUpdate).Text = "Update"
                    Else
                        e.Item.Cells(enmDetailGridItem.ToUpdate).Text = "New Record"
                    End If
            End Select
        End Sub

        Private Function GetFrequencyDesc(ByVal sValue As String) As String
            Dim sRetVal As String = "N/A"
            Try
                If sValue.Trim <> "" Then
                    Select Case sValue
                        Case clsCommon.fncGetPrefix(enmFrequency.DY_Daily)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.DY_Daily)
                        Case clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.HY_Half_Yearly)
                        Case clsCommon.fncGetPrefix(enmFrequency.MY_Monthly)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.MY_Monthly)
                        Case clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.QY_Quarterly)
                        Case clsCommon.fncGetPrefix(enmFrequency.UN_Unlimited)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.UN_Unlimited)
                        Case clsCommon.fncGetPrefix(enmFrequency.WY_Weekly)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.WY_Weekly)
                        Case clsCommon.fncGetPrefix(enmFrequency.YY_Yearly)
                            sRetVal = clsCommon.fncGetPostFix(enmFrequency.YY_Yearly)
                    End Select
                End If
            Catch

            End Try

            Return sRetVal
        End Function

        Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
            Server.Transfer("pg_ApprMatrix.aspx?Mode=" & rq_sMode, False)
        End Sub

   

        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            If ss_strUserType = mdConstant.gc_UT_Reviewer Then
                Server.Transfer("PG_FileList.aspx", False)
            ElseIf ss_strUserType = mdConstant.gc_UT_Auth Then
                Server.Transfer("PG_FileListAuth.aspx", False)
            End If

        End Sub

        Protected Sub dgMandateDetail_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgMandateDetail.PageIndexChanged
            dgMandateDetail.CurrentPageIndex = e.NewPageIndex
            BindDetailGrid()
        End Sub
    End Class

End Namespace
