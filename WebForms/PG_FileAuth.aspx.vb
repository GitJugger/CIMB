Imports System
Imports System.IO
Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Reflection
Imports System.Data.SqlClient
Imports MaxMiddleware


Namespace MaxPayroll

   Partial Class PG_FileAuth
        Inherits clsBasePage
        Private _Helper As New Helper
        Private _CPSPhase3 As New clsCPSPhase3
        Private _ReadWriteGeneric As New MaxReadWrite.Generic()
        Dim strFileType As String = ""
        Dim lngFileId As Long
        Dim lngSubFlowId As Long = 0
        Dim lngUserId As Long
        Dim DivFileId As Long = 0
        Dim FileGivenName As String = ""



#Region "Declaration "
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
        Private ReadOnly Property rq_strFileType() As String
            Get
                Return Request.QueryString("FT") & ""
            End Get
        End Property
        Private ReadOnly Property rq_strFileName() As String
            Get
                Return Request.QueryString("FN") & ""
            End Get
        End Property
        Private ReadOnly Property rq_dtValueDate() As Date
            Get
                If IsDate(Request.QueryString("VD")) Then
                    Return CDate(Request.QueryString("VD"))
                Else
                    Return Nothing
                End If
            End Get
        End Property
        'Private ReadOnly Property rq_lngFileId() As Long
        '   Get
        '      If IsNumeric(Request.QueryString("ID")) Then
        '         Return CLng(Request.QueryString("ID"))
        '      Else
        '         Return 0
        '      End If
        '   End Get
        'End Property

#End Region

#Region "Page Load  "
        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            GetQuery()

            Report_PageLoad(strFileType, lngFileId)
            If strFileType = _Helper.CPSMember_Name Then

                Dim ErrorMsg As String = ""


                DivFileId = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
            _CPSPhase3.SQL_MiniStatement & lngFileId & "," & clsCPSPhase3.enmMiniStatement.GetDividendFileId)

                lngSubFlowId = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
            _CPSPhase3.SQL_MiniStatement & DivFileId & "," & clsCPSPhase3.enmMiniStatement.GetWorkFlowId & "," & lngUserId)

                FileGivenName = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                            _CPSPhase3.SQL_MiniStatement & DivFileId & "," & clsCPSPhase3.enmMiniStatement.GetFileGivenName)

                Report_PageLoad(_Helper.CPSDividen_Name, DivFileId, FileGivenName)
                pnlReport2.Visible = True
            End If


        End Sub

#End Region

#Region "Report_Function "

        Private Sub Report_PageLoad(ByVal strfiletype As String, ByVal rq_lngFileId As Long, Optional ByVal FileGivenName As String = Nothing)
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

            Dim strFileName As String = ""
            Dim strTime As String = ""
            Dim dtValueDt As Date

            Dim intRecordCount As Int16
            Dim strAuthLock As String = ""
            'Dim strFileType As String = ""
            Dim strGivenName As String = ""
            Dim intDisplay As Int16
            Dim intCheck As Int16
            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim lngFormatId As Long
            Dim strFileStatus As String = ""
            Dim strOption As String = ""

            Dim strHeader1 As String = ""
            Dim strHeader2 As String = ""
            Dim strHeader3 As String = ""
            Dim strHeader4 As String = ""
            Dim strHeader5 As String = ""
            Dim strHeader6 As String = ""
            Dim strFooter1 As String = ""
            Dim strFooter2 As String = ""
            Dim strFooter3 As String = ""
            Dim strFooter4 As String = ""
            Dim strFooter5 As String = ""
            Dim strFooter6 As String = ""
            Dim strFooter7 As String = ""

            Dim strTableName As String = ""

            Dim IsCutoff As Boolean
            Dim strDateType As String

            Try

                If Not ss_strUserType = gc_UT_Auth Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                Me.txtAuthCode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + Me.btnConfirm.ClientID + "').click();return false;}} else {return true}; ")

                trBack.Visible = False
                lblHeading.Text = "File Approve"
                'strfiletype = Request.QueryString("FT")
                'Get File Type
                If FileGivenName <> Nothing Then
                    strGivenName = FileGivenName
                Else
                    strGivenName = Request.QueryString("FN")
                End If 'Get File Name

                If strfiletype = _Helper.CPS_Name Then
                    strGivenName = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                End If 'Get File Name
                dtValueDt = CDate(Request.QueryString("VD"))

                If strfiletype = "Payroll File" OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                    strDateType = "Payment"
                Else
                    strDateType = "Upload"
                End If 'Get Value Date

                'Get User Id
                hFileId.Value = rq_lngFileId

                'Disable Button Command on Click
                Call clsCommon.fncBtnDisable(btnConfirm, True)

                If Not Page.IsPostBack Then



                    'Get Authorization Lock Status - Start
                    strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                    If strAuthLock = "Y" Then

                        If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                            strErrMsg = "Sorry! Cannot Approve the file as Your Validation Rights has been locked due to 3 times attempt of invalid Token. Please contact your " & gc_UT_SysAdminDesc & "."
                        Else
                            strErrMsg = "Sorry! Cannot Approve the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                        End If
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileListAuth.aspx", False)
                        Exit Try
                    End If
                    'Get Authorization Lock Status - Stop

                    'Check File Already Submitted - START
                    strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", rq_lngFileId, ss_lngUserID)
                    If Not strFileStatus = "" Then
                        strErrMsg = "The " & strfiletype & " (" & strGivenName & ") " & " has already been " & strFileStatus
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileListAuth.aspx", False)
                        Exit Try
                    End If
                    'Check File Already Submitted - STOP

                    'Check File Value Date Expired - Start
                    If (strfiletype = "Payroll File" OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name) AndAlso dtValueDt < Today Then
                        'Build Subject
                        strSubject = strfiletype & " Approve Failed - " & strGivenName & " Payment Date: " & dtValueDt
                        'Build Body
                        strBody = "The Payment Date " & "(" & dtValueDt & ") for the " & strfiletype & " has expired. Please rename and upload the file again with a future Payment Date."
                        'Block File
                        Call clsCommon.prcBlockFile(rq_lngFileId, 4, ss_lngOrgID, ss_lngUserID)
                        'Get File Name
                        strFileName = clsCommon.fncBuildContent("File Name", strfiletype, rq_lngFileId, ss_lngUserID)
                        'Delete File
                        'Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                        Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, strfiletype, strFileName)

                        'Send Mails
                        Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                        'Display Alert
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strBody & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileListAuth.aspx", False)
                        Exit Try
                    End If
                    'Check File Value Date Expired - Stop

                End If

                'Get Table Name - START
                If strfiletype = "Payroll File" Or strfiletype = "Multiple Bank" Then
                    strFooter6 = ""
                    'trNote.Visible = True
                    'strTableName = "tTemp_FileBody"
                    strOption = clsCommon.fncBuildContent("Privilege", "", ss_lngOrgID, ss_lngUserID)                                               'Get Privilege User
                ElseIf strfiletype = "EPF File" Or strfiletype = "EPF Test File" Then
                    strOption = "E"
                    trNote.Visible = False
                    'strTableName = "tEpf_FileBody"
                ElseIf strfiletype = "SOCSO File" Then
                    strOption = "S"
                    trNote.Visible = False
                    'strTableName = "tSocso_FileBody"
                ElseIf strfiletype = "LHDN File" Then
                    strOption = "L"
                    trNote.Visible = False
                    'strTableName = "tLHDN_FileBody"
                ElseIf strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                    strOption = clsCommon.fncBuildContent("Privilege", "", ss_lngOrgID, ss_lngUserID)
                    trNote.Visible = False
                ElseIf strfiletype = _Helper.CPS_Name Then
                    strOption = "C"
                    trNote.Visible = False
                ElseIf strfiletype = _Helper.PayLinkPayRoll_Name Then
                    strOption = "I"
                    trNote.Visible = False

                End If

                'Get Table Name - STOP

                If Not Page.IsPostBack Then

                    'BindBody(body)
                    'Check Cutoff Time - Start
                    If strfiletype = "Payroll File" OrElse strfiletype = _Helper.Autopay_Name _
                        OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name _
                        OrElse strfiletype = _Helper.PayLinkPayRoll_Name Then

                        IsCutoff = clsCommon.fncCutoffTime(Request.QueryString("FT"), ss_lngOrgID, ss_lngUserID, strTime, _
                                    Day(dtValueDt), Month(dtValueDt), Year(dtValueDt), strOption)
                    End If

                    If IsCutoff Then
                        'Build Subject
                        strSubject = strfiletype & " Approve Failed - " & lblFName.Text & " Payment Date: " & dtValueDt
                        'Build Body
                        strBody = "The " & strfiletype & "(" & strGivenName & ") for Payment Date " & dtValueDt & " cannot be Approved after the Cutoff Time (" & strTime & ")."
                        'Block File
                        Call clsCommon.prcBlockFile(rq_lngFileId, 4, ss_lngOrgID, ss_lngUserID)
                        'Get File Name
                        strFileName = clsCommon.fncBuildContent("File Name", Request.QueryString("FT"), rq_lngFileId, ss_lngUserID)
                        'Delete File
                        'Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, Request.QueryString("FT"), strFileName)
                        Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, Request.QueryString("FT"), strFileName)
                        'Send Mails
                        Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                        'Display Alert
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strBody & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileListAuth.aspx", False)

                        Exit Try

                    End If
                    'Check Cutoff Time - Stop

                    trAuth.Visible = False
                    trSubmit.Visible = True
                    tblForm.Visible = False
                    trConfirm.Visible = False

                    'Populate Remarks Data Grid - Start
                    dsFileReview = clsCommon.fncListRemarks(rq_lngFileId, ss_lngOrgID, ss_lngUserID)
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


                    dsFileReview = clsCommon.fncGetRequested("File Review", ss_lngOrgID, ss_lngUserID, rq_lngFileId, Request.QueryString("FT"))
                    For Each drFileReview In dsFileReview.Tables(0).Rows

                        If strfiletype = "Mandate File" Then
                            hAmount.Value = 0
                            txtDate.Value = Format(drFileReview("UDATE"), "dd/MM/yyyy")
                        Else
                            txtDate.Value = Format(drFileReview("PDATE"), "dd/MM/yyyy")

                            If Not strfiletype = _Helper.CPSMember_Name Then
                                hAmount.Value = Format(CDbl(drFileReview("TAMOUNT")), "##,##0.00")
                                strFooter1 = "Total Amount: RM " & Format(CDbl(drFileReview("TAMOUNT")), "##,##0.00")
                            Else
                                hAmount.Value = 0
                                strFooter1 = ""
                            End If

                            strFooter2 = "Charge Per Transaction: RM" & Format(CDbl(drFileReview("CHRG")), "##,##0.00")
                            strFooter3 = "Total Transactions: " & drFileReview("TTRANS")
                            hTrans.Value = drFileReview("TTRANS")

                            If strfiletype = _Helper.CPSDividen_Name Or strfiletype = _Helper.CPSMember_Name _
                            Or strfiletype = _Helper.CPSDelimited_Dividen_Name Or strfiletype = _Helper.CPSSingleFileFormat_Name Then
                                strFooter4 = "Total Charge: RM" & Format(CDbl(_CPSPhase3.GetOrgCharges(rq_lngFileId, ss_lngOrgID)), "##,##0.00")
                                strFooter2 = ""

                            Else
                                strFooter4 = "Total Charge: RM" & Format(CDbl(drFileReview("TCHRG")), "##,##0.00")
                            End If

                            hChrg.Value = Format(CDbl(drFileReview("TCHRG")), "##,##0.00")

                            ''This Footer/Header not required in CPS
                            If strfiletype = _Helper.CPSDividen_Name Or strfiletype = _Helper.CPSMember_Name _
                            Or strfiletype = _Helper.CPSDelimited_Dividen_Name Or strfiletype = _Helper.CPSSingleFileFormat_Name Then
                                strFooter5 = ""
                                strHeader6 = ""
                                hChrg.Value = "RM " & Format(CDbl(_CPSPhase3.GetOrgCharges(rq_lngFileId, ss_lngOrgID)), "##,##0.00")
                                If strfiletype = _Helper.CPSSingleFileFormat_Name Then
                                    strFooter5 = "Hash Total: " & drFileReview("HTOTAL")
                                End If

                            Else
                                strFooter5 = "Hash Total: " & drFileReview("HTOTAL")
                                strHeader6 = "Contribution Month: " & drFileReview("CMONTH")

                            End If

                        End If
                        strHeader6 = "Contribution Month: " & drFileReview("CMONTH")
                        lngFormatId = drFileReview("FRID")
                        hFileName.Value = drFileReview("FNAME")
                    Next

                    'Get File body table name.
                    strTableName = clsUpload.fncGetDBTableName(lngFormatId)

                    strHeader1 = "Date: " & Today
                    strHeader2 = "File Type: " & strfiletype
                    strHeader4 = strDateType & ": " & txtDate.Value
                    'strHeader5 = "File Name: " & strGivenName

                    If strfiletype = _Helper.CPS_Name Then
                        strHeader5 = "File Name: " & strGivenName & " (" & txtDate.Value & ")"
                    Else
                        strHeader5 = "File Name: " & strGivenName
                    End If

                    If strfiletype = "Payroll File" Or strfiletype = "Multiple Bank" OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                        strHeader6 = ""
                    End If

                    'Retrieve Public Hash Total for Payroll, EPF and Socso
                    'ViewState("CUSTA_PHTOTAL") = clsCustomer.fnPHTotal(rq_lngFileId, strFileType)

                    'strFooter7 = "Public Hash Total: " & ViewState("CUSTA_PHTOTAL")

                    'Check If User Can See Detail - Start
                    intDisplay = clsCommon.fncBuildContent("Display", "", ss_lngOrgID, ss_lngUserID)
                    If intDisplay = 2 Then

                        lblType.Text = strfiletype
                        If strfiletype = _Helper.CPS_Name Then
                            lblFName.Text = strGivenName & " (" & txtDate.Value & ")"
                        Else
                            lblFName.Text = strGivenName
                        End If

                        lblAmt.Text = hAmount.Value
                        lblTran.Text = hTrans.Value
                        lblChrg.Text = hChrg.Value
                        lblPHTotal.Text = ViewState("CUSTA_PHTOTAL")
                        tblForm.Visible = True
                        tblReview.Visible = False
                    End If
                    'Check If User Can See Detail - Start

                End If
                'Check if Same Payment Date & Same Amount has already been Authorized - Start
                If Not Page.IsPostBack Then
                    intCheck = clsCommon.fncUploadCheck("FINAL ALERT", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strfiletype, dtValueDt, hAmount.Value)
                    If intCheck > 0 Then
                        lblMessage.Text = "Please Note: A " & strfiletype & " with the same " & strDateType & " Date & Total Amount has already been approved by you. Please check File Status report or contact " & gc_Const_CompanyName & " registration centre at " & gc_Const_CompanyContactNo & "."
                    End If
                    Dim bContinue As Boolean = False
                    Me.trChallengeCode.Visible = False
                    Me.trDynaPin.Visible = False
                    If CBool(Session(gc_Ses_Token)) Then
                        btnConfirm.Visible = False
                        If Me.fncRequestChallengeCode(Me.txtChallengeCode.Text) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then

                            bContinue = True

                        End If
                    Else
                        bContinue = True
                        btnSign.Visible = False
                        Me.trChallengeCode.Visible = False
                        Me.trDynaPin.Visible = False
                    End If
                    If bContinue = False Then
                        'disable everything
                        Me.btnAccept.Enabled = False
                        Me.btnReject.Enabled = False

                        Me.lblMessage.Text = "Token Error"
                    End If
                    If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                        btnConfirm.Visible = False
                        trAuth.Visible = False
                    Else
                        btnSign.Visible = False
                    End If


                    Me.hdnData.Value = Guid.NewGuid.ToString
                    If intDisplay = 1 Then
                        rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                        rvReport.ServerReport.ReportPath = strReportDir & "DynamicReportDetails"

                        Dim dsFieldName As DataSet = clsCommon.fncGetRequested("Field Position", ss_lngOrgID, ss_lngUserID, lngFormatId, strfiletype)

                        Dim iCount As Integer = 0, iCount2 As Integer = 0
                        Dim rptParam(25) As Microsoft.Reporting.WebForms.ReportParameter


                        If strfiletype = "Payroll File" Then

                            Dim drTempRow As DataRow

                            iCount = 0
                            For Each drTempRow In dsFieldName.Tables(0).Rows
                                If drTempRow.Item(0) = "Debiting Account Number" _
                                   OrElse drTempRow.Item(0) = "Payment Value Date" _
                                   OrElse drTempRow.Item(0) = "Beneficiary Account Number" _
                                   OrElse drTempRow.Item(0) = "Beneficiary Name" _
                                   OrElse drTempRow.Item(0) = "Beneficiary IC Passport Reg No" _
                                   OrElse drTempRow.Item(0) = "Payment Reference Number" _
                                   OrElse drTempRow.Item(0) = "Payment Details" _
                                   OrElse drTempRow.Item(0) = "Payment Description" _
                                   OrElse drTempRow.Item(0) = "Transaction Amount Detail" Then

                                    rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(drTempRow.Item(0)))

                                    iCount += 1
                                End If
                            Next

                        ElseIf strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then

                            For iCount = 0 To 7
                                rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(dsFieldName.Tables(0).Rows(iCount).Item(0)))
                            Next
                        ElseIf strfiletype = _Helper.CPSDividen_Name Or strfiletype = _Helper.CPSMember_Name Then
                            Dim drTempRow As DataRow

                            iCount = 0
                            If strfiletype = _Helper.CPSDividen_Name Then
                                rvReport.ServerReport.ReportPath = strReportDir & "DynamicReportDetails" ''Added For CPS Phase 3
                                For Each drTempRow In dsFieldName.Tables(0).Rows
                                    If drTempRow.Item(0) = "DIV_MEMNO" _
                                       OrElse drTempRow.Item(0) = "DIV_SHARE" _
                                       OrElse drTempRow.Item(0) = _CPSPhase3.GetChequeCol_Name(rq_lngFileId, ss_lngOrgID) _
                                       OrElse drTempRow.Item(0) = "DIV_WRNTNO" _
                                       OrElse drTempRow.Item(0) = "DIV_DATE" _
                                       OrElse drTempRow.Item(0) = "DIV_NAME" Then

                                        rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(drTempRow.Item(0)))

                                        iCount += 1
                                    End If
                                Next
                            ElseIf strfiletype = _Helper.CPSMember_Name Then
                                rvReport_2.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                                rvReport_2.ServerReport.ReportPath = strReportDir & "DynamicReportDetails" ''Added For CPS Phase 3

                                For Each drTempRow In dsFieldName.Tables(0).Rows
                                    If drTempRow.Item(0) = "MEM_ACCNO" _
                                       OrElse drTempRow.Item(0) = "MEM_SHRHOLDING" _
                                       OrElse drTempRow.Item(0) = "MEM_NADD1" _
                                       OrElse drTempRow.Item(0) = "MEM_IC" Then

                                        rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(drTempRow.Item(0)))

                                        iCount += 1
                                    End If
                                Next

                            End If

                        ElseIf strfiletype = _Helper.CPSDelimited_Dividen_Name Then
                            Dim drTempRow As DataRow

                            iCount = 0

                            rvReport.ServerReport.ReportPath = strReportDir & "DynamicReportDetails" ''Added For CPS Phase 3
                            For Each drTempRow In dsFieldName.Tables(0).Rows

                                If drTempRow.Item(0) = "Shares_No" _
                               OrElse drTempRow.Item(0) = "Cheque_Amount" _
                               OrElse drTempRow.Item(0) = "Address_1" _
                               OrElse drTempRow.Item(0) = "Address_2" _
                               OrElse drTempRow.Item(0) = "Address_3" _
                               OrElse drTempRow.Item(0) = "Total_Net_Div" _
                                Then

                                    rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(drTempRow.Item(0)))

                                    iCount += 1
                                End If
                            Next

                        ElseIf strfiletype = _Helper.CPSSingleFileFormat_Name Then

                            rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                            rvReport.ServerReport.ReportPath = strReportDir & "SFF_FileStatusDetails"
                            Dim rptParamSFF(0) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParamSFF(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileId", lngFileId)

                            rvReport.ServerReport.SetParameters(rptParamSFF)

                            rptParamSFF = Nothing
                            rvReport.ShowBackButton = True
                            Exit Try


                            '**Added by Naresh to generate the Payroll file report-11-02-11
                        ElseIf strfiletype = _Helper.PayLinkPayRoll_Name Then
                            rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                            rvReport.ServerReport.ReportPath = strReportDir & "PaylinkFile"
                            Dim rptParamInfenion(0) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParamInfenion(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileId", lngFileId)

                            rvReport.ServerReport.SetParameters(rptParamInfenion)

                            rptParamInfenion = Nothing
                            rvReport.ShowBackButton = True
                            Exit Try

                        ElseIf strfiletype = _Helper.CPS_Name Then

                            Dim drTempRow As DataRow

                            iCount = 0
                            For Each drTempRow In dsFieldName.Tables(0).Rows
                                If drTempRow.Item(0) = "Payment Reference" _
                                   OrElse drTempRow.Item(0) = "Amount" _
                                   OrElse drTempRow.Item(0) = "Value Date" _
                                   OrElse drTempRow.Item(0) = "Beneficiary Name" Then

                                    rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(drTempRow.Item(0)))

                                    iCount += 1
                                End If
                            Next


                        Else
                            For iCount = 0 To dsFieldName.Tables(0).Rows.Count - 1
                                rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(dsFieldName.Tables(0).Rows(iCount).Item(0)))
                            Next
                        End If

                        If iCount < 10 Then
                            For iCount2 = iCount To 9
                                rptParam(iCount2) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount2 + 1).ToString, "")
                            Next
                        End If
                        rptParam(10) = New Microsoft.Reporting.WebForms.ReportParameter("in_TableName", strTableName)
                        rptParam(11) = New Microsoft.Reporting.WebForms.ReportParameter("in_CompanyName", Me.fncGetCompanyName)

                        rptParam(12) = New Microsoft.Reporting.WebForms.ReportParameter("in_Header1", strHeader1)
                        rptParam(13) = New Microsoft.Reporting.WebForms.ReportParameter("in_Header2", strHeader2)
                        rptParam(14) = New Microsoft.Reporting.WebForms.ReportParameter("in_Header3", strHeader3)
                        rptParam(15) = New Microsoft.Reporting.WebForms.ReportParameter("in_Header4", strHeader4)
                        rptParam(16) = New Microsoft.Reporting.WebForms.ReportParameter("in_Header5", strHeader5)
                        rptParam(17) = New Microsoft.Reporting.WebForms.ReportParameter("in_Header6", strHeader6)
                        rptParam(18) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer1", strFooter1)
                        rptParam(19) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer2", strFooter2)
                        rptParam(20) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer3", strFooter3)
                        rptParam(21) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer4", strFooter4)
                        rptParam(22) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer5", strFooter5)
                        rptParam(23) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer6", strFooter6)
                        rptParam(24) = New Microsoft.Reporting.WebForms.ReportParameter("in_Footer7", strFooter7)
                        rptParam(25) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileID", rq_lngFileId)
                        'rvReport.ServerReport.SetParameters(rptParam)
                        ''Added For CPS Phase 3 Start
                        If strfiletype = _Helper.CPSMember_Name Then
                            rvReport_2.ServerReport.SetParameters(rptParam)
                            rvReport_2.ShowBackButton = True
                        Else
                            rvReport.ServerReport.SetParameters(rptParam)
                        End If
                        ''Added For CPS Phase 3 Stop
                        rptParam = Nothing
                        rvReport.ShowBackButton = True
                        rvReport.ServerReport.Refresh()
                    Else
                        pnlReport.Visible = False
                    End If
                End If

            Catch

                If Err.Description <> "Thread was being aborted." Then

                    'Hide Tables
                    tblForm.Visible = False
                    tblReview.Visible = False

                    'Display Message
                    lblMessage.Text = "Sorry! Report Could Not be Loaded. Please try again."

                    'Log Error
                    clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page_Load - PG_FileAuth", Err.Number, Err.Description)

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

#Region "Accept/Reject File "

        '****************************************************************************************************
        'Procedure Name : prcReviewFile
        'Purpose        : To Update the Status to Review
        'Arguments      : User ID
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Private Sub prcAuthorizeFile(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click
            prcConfirmAction()
        End Sub

        Private Sub prcConfirmAction(Optional ByVal bCheckToken As Boolean = False)
            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strErrMsg As String = ""
            Dim strReason As String = ""
            Dim strUserIP As String = ""
            Dim lngFlowId As Long
            Dim IsCutoff As Boolean
            Dim strDbAuthCode As String = ""
            Dim intAttempts As Int16
            Dim strTime As String = ""
            Dim strOption As String = ""
            Dim lngGroupId As Long
            Dim strUserRole As String = ""
            Dim strUserName As String = ""
            Dim strSubject As String = ""
            Dim strBody As String = ""
            Dim intFlowCount As Int16
            Dim strFileType As String = ""
            Dim strGivenName As String = ""
            Dim IsSubmit As Boolean
            Dim strFileName As String = ""
            Dim strFileStatus As String = ""
            Dim lngUserId As Long
            Dim lngOrgId As Long
            Dim IsAuthCode As Boolean
            Dim lngFileId As Long
            Dim strDate As String = ""
            Dim strDateType As String
            Dim FileTypeId As Short = 0
            Dim ServiceId As Short = 0
            Dim ServiceType As Short = 0
            Dim SqlStatement As String = Nothing
            Try

                strDate = Request.Form("ctl00$cphContent$txtDate")                                                                                           'Get Value Date
                strFileType = Request.QueryString("FT")                                                                                     'Get File Type
                strGivenName = Request.QueryString("FN")                                                                                    'Get File Name
                If strFileType = _Helper.CPS_Name Then
                    strGivenName = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                End If
                strUserIP = Request.ServerVariables("REMOTE_ADDR")                                                                          'User IP Address
                strReason = IIf(txtReason.Text = "", "NA", txtReason.Text)                                                                  'Reason/Remarks
                lngOrgId = IIf(IsNumeric(Session(gc_Ses_OrgId)), Session(gc_Ses_OrgId), 0)                                                    'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)                                                 'Get User Id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)                                              'Get Group Id
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)                                         'File Id  
                lngFlowId = IIf(IsNumeric(Request.QueryString("WfId")), Request.QueryString("WfId"), 0)
                FileTypeId = MaxGeneric.clsGeneric.NullToShort(Request.QueryString("FTID"))
                ServiceId = MaxGeneric.clsGeneric.NullToShort(Request.QueryString("SID"))

                'Get service type based on serviceId -start
                If ServiceId > 0 Then
                    SqlStatement = _Helper.GetSQLCommon & "'ServiceType'," & ServiceId

                    ServiceType = MaxGeneric.clsGeneric.NullToShort(MaxMiddleware.PPS.SQLScalarValue( _
                                     _Helper.GetSQLConnection, _Helper.GetSQLTransaction, String.Empty, SqlStatement))
                End If
                'Get service type based on serviceId -stop

                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                    strDateType = "Payment"
                Else
                    strDateType = "Upload"
                End If

                'WorkFlow Id

                'Check File Already Submitted - START
                strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, lngUserId)
                If Not strFileStatus = "" Then
                    strErrMsg = "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('" & strErrMsg & "');")
                    Response.Write("</script>")
                    Server.Transfer("PG_FileListAuth.aspx", False)
                    Exit Try
                End If
                'Check File Already Submitted - STOP

                If FileTypeId > 0 And ServiceId > 0 And Not ServiceType = Helper.ServiceType.Mandates Then
                    'Check cutoff-start
                    If _ReadWriteGeneric.IsCutoffTime(FileTypeId, strDate, ServiceId, String.Empty, strTime) Then
                        IsCutoff = True
                    End If
                    'Check cutoff-stop
                End If

                'Check Cutoff Time - Start
                'Commented on 25/11/2015
                If hCommand.Value = "A" Then

                    'Get Table Name - START
                    If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                        strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)                                               'Get Privilege User
                    ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                        strOption = "E"
                    ElseIf strFileType = "SOCSO File" Then
                        strOption = "S"
                    ElseIf strFileType = "LHDN File" Then
                        strOption = "L"
                    ElseIf strFileType = "ZAKAT" Then
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
                    'IsCutoff = clsCommon.fncCutoffTime(strFileType, lngOrgId, lngUserId, strTime, _
                    'Day(txtDate.Value), Month(txtDate.Value), Year(txtDate.Value), strOption, ConfigurationManager.AppSettings("DefaultBankCode"))

                    If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name _
                        OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name _
                            OrElse strFileType = _Helper.AutopaySNA_Name OrElse strFileType = _Helper.PayLinkPayRoll_Name Then

                        IsCutoff = clsCommon.fncCutoffTime(strFileType, lngOrgId, lngUserId, strTime, _
                        Day(txtDate.Value), Month(txtDate.Value), Year(txtDate.Value), strOption, ConfigurationManager.AppSettings("DefaultBankCode"))
                    End If

                    If IsCutoff Then
                        'Build Subject
                        strSubject = strFileType & " Approve Failed - " & strGivenName & " Payment Date: " & txtDate.Value
                        'Build Body
                        strBody = "The " & strFileType & "(" & strGivenName & ") for Payment Date " & txtDate.Value & " cannot be Reviewed after the Cutoff Time (" & strTime & ")."
                        'Block File
                        Call clsCommon.prcBlockFile(lngFileId, 4, lngOrgId, lngUserId)
                        'Get File Name
                        strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, lngUserId)
                        'Delete File
                        'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                        Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                        'Send Mails
                        Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                        'Display Alert
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strBody & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileListAuth.aspx", False)
                        Exit Try
                    End If

                End If
                'until here 25/11/2015
                'Check Cutoff Time - Stop

                'Check Session Value for Authorization Lock - Start
                If Not IsNumeric(Session("AUTH_LOCK")) Or Session("AUTH_LOCK") = 0 Then
                    Session("AUTH_LOCK") = 0
                End If
                'Check Session Value for Authorization Lock - Stop

                If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                    IsAuthCode = bCheckToken
                Else
                    'Check If AuthCode is Valid - Start
                    strDbAuthCode = clsCommon.fncPassAuth(lngUserId, "A", lngOrgId)
                    IsAuthCode = IIf(strDbAuthCode = txtAuthCode.Text, True, False)
                    'Check If AuthCode is Valid - Stop
                End If

                'Check for invalid Authorization Code Attempts - START
                If Not IsAuthCode Then
                    intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                    If Not intAttempts = 2 Then
                        If Not IsAuthCode Then
                            intAttempts = intAttempts + 1
                            Session("AUTH_LOCK") = intAttempts
                            If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                                lblMessage.Text = "Token is invalid, Please plugin a valid Token."
                            Else
                                lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                            End If

                            Exit Try
                        End If
                    ElseIf intAttempts = 2 Then
                        If Not IsAuthCode Then
                            'Disbale Table Row
                            trSubmit.Visible = False
                            trConfirm.Visible = False
                            'Get User Role
                            strUserRole = clsCommon.fncBuildContent("User Role", "", lngUserId, lngUserId)
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("User Name", "", lngUserId, lngUserId)
                            'Build Subject
                            strSubject = strUserName & " (" & strUserRole & ") Locked/Inactive."
                            'Build Body
                            strBody = strUserName & " (" & strUserRole & ")" & " has been Locked/Inactive on " & Now() & "due to Invalid Validation Code attempts. Please change the User Validation Code."
                            'Lock Authorization Code
                            Call clsUsers.prcAuthLock(lngOrgId, lngUserId, "A")
                            'Send Mails
                            Call clsCommon.prcSendMails("USER LOCK", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                            'Track Auth Lock
                            Call clsUsers.prcLockHistory(lngUserId, "A")
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
                            Exit Try
                        End If
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP

                'Update Remakrs/Reason
                Call clsCommon.fncRemarks(lngFileId, strReason)

                'Get File Name
                strFileName = hFileName.Value

                'Reject File - START
                If hCommand.Value = "R" Then
                    'Build Subject
                    strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", " & strDateType & " Date: " & strDate
                    'Build Body
                    strBody = "The " & strFileType & " with " & strDateType & " Date " & strDate & " Rejected Successfully. Remarks: " & strReason
                    'Delete File From FTP Folder
                    'Call clsCommon.prcDelFile(lngOrgId, lngUserId, strFileType, strFileName)
                    'Marcus: Move the rejected file to backup Folder
                    Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                    'Update Workflow
                    Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "R", "Y", strReason, strUserIP, 0, 0, "Y")
                    'Display Message
                    lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & strDate & " has been Rejected Successfully."
                End If
                'Reject File - STOP

                'Show Message - START
                If hCommand.Value = "A" Then

                    'Update Workflow
                    Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "Y")

                    'Get Count of Balance Authorizers
                    intFlowCount = clsUsers.fnRoleCount("AUTHORIZER", "A", "PENDING", lngFileId, lngGroupId)

                    If strFileType = _Helper.CPSMember_Name Then

                        'Update WorkFlow Dividend
                        Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "Y")

                    End If

                    'Get Last Authorizer Submit File
                    If intFlowCount = 0 Then

                        'Submit File
                        'If strFileType = "Payroll File" Then
                        '    IsSubmit = True
                        'Else
                        '    IsSubmit = clsCommon.fncSubmitFile(lngOrgId, lngUserId, Request.QueryString("FT"), strFileName)
                        'End If

                        If strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name OrElse _
                            strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.DirectDebit_Name _
                            OrElse strFileType = _Helper.PayLinkPayRoll_Name OrElse strFileType = _Helper.HybridAutoPaySNA_Name _
                                 OrElse strFileType = _Helper.HybridDirectDebit_Name OrElse strFileType = _Helper.HybridMandate_Name Then

                            IsSubmit = clsCommon.fncSubmitFile(lngOrgId, lngUserId, Request.QueryString("FT"), strFileName)
                        Else
                            'No file generation or movement for statutory body files
                            IsSubmit = True
                        End If

                        If IsSubmit Then
                            'Marcus: Below Code for sending email is to be reviewd
                            'Dim clsEmail As New clsEmail
                            'If clsEmail.NotifyBankDownloader(strFileName, "", strFileType) Then

                            'End If

                            If strFileType = _Helper.CPSMember_Name Then
                                'Build Subject
                                strSubject = "The " & strFileType & "and " & _Helper.CPSDividen_Name & " Approved & Submitted - " & _
                                    strGivenName & "," & FileGivenName & " " & strDateType & " Date: " & strDate & ", Submission Date: " & _
                                        Now.ToShortDateString

                                'Build Body
                                strBody = "The " & strFileType & " and " & _Helper.CPSDividen_Name & " with " & strDateType & " Date: " & _
                                    strDate & " Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & _
                                        "<br>Remarks: " & strReason

                                'Display Message
                                lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") and " & _Helper.CPSDividen_Name & _
                                "(" & FileGivenName & ")with " & strDateType & " Date: " & strDate & " has been Approved & Submitted Successfully." & _
                                        "<br>Submisson Date: " & Now.ToShortDateString()

                                'Update File Status Member
                                lngFileId = clsCommon.fncFileDetails("FINAL", lngFileId, strFileType, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)
                                'Update File Status Dividend
                                DivFileId = clsCommon.fncFileDetails("FINAL", DivFileId, _Helper.CPSDividen_Name, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)


                            Else
                                'Build Subject
                                strSubject = "The " & strFileType & " Approved & Submitted - " & strGivenName & ", " & strDateType & " Date: " & strDate & ", Submission Date: " & Now.ToShortDateString

                                'Build Body
                                strBody = "The " & strFileType & " with " & strDateType & " Date: " & strDate & " Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & "<br>Remarks: " & strReason

                                'Display Message
                                lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & strDate & " has been Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString()

                                'Update File Status
                                lngFileId = clsCommon.fncFileDetails("FINAL", lngFileId, strFileType, "", "", lngOrgId, lngUserId, Today, "", 0, "", "", "", "", 0, 0, 0)
                            End If
                            ''EMAIL SEND START
                            Dim clsEmail As New clsEmail
                            If strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name Or _
                            strFileType = _Helper.CPSDelimited_Dividen_Name Or strFileType = _Helper.CPSSingleFileFormat_Name Then

                                Call clsCommon.prcSendMails("BANK USER", lngOrgId, lngUserId, 0, strSubject, strBody, 0)
                            ElseIf strFileType = "Mandate File" Then
                                Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserId, 0, strSubject, strBody, 0)
                            Else
                                clsEmail.NotifyBankDownloader(strFileName, "", strFileType)
                            End If
                            ''EMAIL SEND STOP
                        Else

                            'Get File Name
                            strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, lngUserId)


                            'Build Subject
                            'strSubject = strFileType & "(" & strGivenName & ") with Payment Date: " & strDate & " Approval & Submission Failed."
                            strSubject = strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & strDateType & " Validation & Submission Failed."
                            'Build Body
                            'strBody = "The " & strFileType & "(" & strGivenName & ") with Payment Date " & strDate & " Approval & Submission Failed. Remarks: Please Re-Approve."
                            strBody = "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & strDateType & " Validation & Submission Failed. Remarks: Please Re-Approve."
                            'Message
                            lblMessage.Text = strBody

                            'Update Remarks
                            Call clsCommon.fncRemarks(lngFileId, strBody)

                            'Update Workflow
                            Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "N")

                            If strFileType = _Helper.CPSMember_Name Then
                                'Update Remarks
                                Call clsCommon.fncRemarks(DivFileId, strBody)
                                'Update WorkFlow Dividend
                                Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, lngOrgId, lngUserId, "A", "N", strReason, strUserIP, 0, 0, "N")

                            End If
                        End If

                    Else
                        If strFileType = _Helper.CPSMember_Name Then
                            'Build Subject
                            strSubject = "The " & strFileType & "and " & _Helper.CPSDividen_Name & " Approved & Submitted - " & _
                                strGivenName & "," & FileGivenName & " " & strDateType & " Date: " & strDate & ", Submission Date: " & _
                                    Now.ToShortDateString

                            'Build Body
                            strBody = "The " & strFileType & " and " & _Helper.CPSDividen_Name & " with " & strDateType & " Date: " & _
                                strDate & " Approved & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & _
                                    "<br>Remarks: " & strReason

                            'Display Message
                            lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") and " & _Helper.CPSDividen_Name & _
                            "(" & FileGivenName & ")with " & strDateType & " Date: " & strDate & " has been Approved & Submitted Successfully." & _
                                    "<BR>Please Note: " & intFlowCount & " more Approver(s) to Approve the file." & gc_BR

                        Else
                            'Build Subject
                            strSubject = "The " & strFileType & " Approved - " & strGivenName & ", " & strDateType & " Date: " & strDate

                            'Build Body
                            strBody = "The " & strFileType & " with " & strDateType & " Date: " & strDate & " Approved Successfully. Remarks: " & strReason

                            'Display Success Message and Balance No of Authorizers
                            lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & strDate & " has been Approved Successfully.<BR>Please Note: " & intFlowCount & " more Approver(s) to Approve the file." & gc_BR

                        End If

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

            Catch

                'Log Error
                lblMessage.Text = "File Approve/Reject Failed."
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcAuthorizeFile - PG_FileAuth", Err.Number, Err.Description)

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

#Region "Reject Confirm  "

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
                strGivenName = Request.QueryString("FN")
                If strFileType = _Helper.CPS_Name Then
                    strGivenName = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                End If 'Get File Name 
                lngOrgId = IIf(IsNumeric(Session(gc_Ses_OrgId)), Session(gc_Ses_OrgId), 0)                'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'File Id  

                'Check File Already Submitted - START
                strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, lngUserId)
                If Not strFileStatus = "" Then
                    strErrMsg = "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('" & strErrMsg & "');")
                    Response.Write("</script>")
                    Server.Transfer("PG_FileListAuth.aspx", False)
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
                tblForm.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                tblReview.Visible = False
                txtReason.ReadOnly = True
                lblType.Text = strFileType
                lblChrg.Text = hChrg.Value
                lblAmt.Text = hAmount.Value
                lblTran.Text = hTrans.Value
                If strFileType = _Helper.CPSMember_Name Then
                    lblFName.Text = FileGivenName & " and " & strGivenName
                Else
                    lblFName.Text = strGivenName
                End If
                lblPHTotal.Text = ViewState("CUSTA_PHTOTAL")
                lblHeading.Text = "File Approve - Rejection Confirmation"

                'Hide Authentication Code if the token setting is true and user type is authorizer. #15 Jan 2007 - Start
                If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                    trAuth.Visible = False
                    Me.trChallengeCode.Visible = True
                    Me.trDynaPin.Visible = True

                    lblMessage.Text = "Please click Sign button to validate Token."
                Else
                    trAuth.Visible = True
                    lblMessage.Text = "Please Enter your Validation Code and Confirm File Rejection."
                End If
                'Hide Authentication Code if the token setting is true and user type is authorizer. #15 Jan 2007 - End

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "PG_FileAuth - Page_Reject", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region

#Region "Accept Confirm  "

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
            Dim bExceedLimit As Boolean

            Try

                strFileType = Request.QueryString("FT")                                                 'Get File Type
                strGivenName = Request.QueryString("FN")
                If strFileType = _Helper.CPS_Name Then
                    strGivenName = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                End If 'Get File Name 
                lngOrgId = IIf(IsNumeric(Session(gc_Ses_OrgId)), Session(gc_Ses_OrgId), 0)              'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'File Id  

                'Check File Already Submitted - START
                strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngFileId, lngUserId)
                If Not strFileStatus = "" Then
                    strErrMsg = "The " & strFileType & " (" & strGivenName & ") " & " has already been " & strFileStatus
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('" & strErrMsg & "');")
                    Response.Write("</script>")
                    Server.Transfer("PG_FileListAuth.aspx", False)
                    Exit Try
                End If
                'Check File Already Submitted - STOP

                'bExceedLimit = clsCommon.fncCheckApproverLimit(lngOrgId, lngUserId, lngFileId)
                'If Not strFileStatus = "" Then
                '    strErrMsg = "The total amount of this file has already exceed your validation limit."
                '    Response.Write("<script language='JavaScript'>")
                '    Response.Write("alert('" & strErrMsg & "');")
                '    Response.Write("</script>")
                '    Server.Transfer("PG_FileListAuth.aspx", False)
                '    Exit Try
                'End If

                hCommand.Value = "A"
                If CBool(Session(gc_Ses_Token)) AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A") Then
                    trAuth.Visible = False
                    Me.trChallengeCode.Visible = True
                    Me.trDynaPin.Visible = True
                    lblMessage.Text = "Please click Sign button to validate Token."
                Else
                    trAuth.Visible = True
                    lblMessage.Text = "Please Enter your Validation Code and Confirm File Approve."

                End If
                tblForm.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                tblReview.Visible = False
                txtReason.ReadOnly = True
                lblType.Text = strFileType
                lblChrg.Text = hChrg.Value
                lblAmt.Text = hAmount.Value
                lblTran.Text = hTrans.Value
                If strFileType = _Helper.CPSMember_Name Then
                    lblFName.Text = FileGivenName & " and " & strGivenName
                Else
                    lblFName.Text = strGivenName
                End If
                lblPHTotal.Text = ViewState("CUSTA_PHTOTAL")
                lblHeading.Text = "File Approve - Acceptence Confirmation"


            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Accept - PG_FileAuth", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region


        Protected Sub btnSign_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSign.Click
            If fncTokenAuthenticate(txtDynaPin.Text) Then
                prcConfirmAction(Session(gc_Ses_Token))
                'Me.lblMessage.Text = "authentication passed"
            Else
                Me.lblMessage.Text = "Token Authentication Failed.  Please Re-enter Dyna Pin"
                If Me.fncRequestChallengeCode(Me.txtChallengeCode.Text) = False Then
                    lblMessage.Text = txtChallengeCode.Text
                    txtChallengeCode.Text = ""
                End If
            End If
        End Sub

#Region "Get Query String - Remain"
        Private Sub GetQuery()
            strFileType = Request.QueryString("FT")                                                 'Get File Type
            lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'Get File Id
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)                                                 'Get User Id


        End Sub

#End Region

    End Class

End Namespace
