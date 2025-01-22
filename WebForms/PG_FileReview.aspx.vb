Imports System
Imports System.IO
Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Reflection
Imports System.Data.SqlClient
Imports MaxMiddleware


Namespace MaxPayroll


Partial Class PG_FileReview
        Inherits clsBasePage

#Region "Global Variables "

        Private _Helper As New Helper
        Private _CPSPhase3 As New clsCPSPhase3
        Private _ReadWriteGeneric As New MaxReadWrite.Generic()
        Dim strFileType As String = ""
        Dim lngFileId As Long
        Dim lngSubFlowId As Long = 0
        Dim lngUserId As Long
        Dim DivFileId As Long = 0
        Dim FileGivenName As String = ""

        Private _Common As New Common()
#End Region

#Region "Declaration"
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
#End Region

#Region "Page Load"
        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Loa
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

#Region "Report_Function"

        Private Sub Report_PageLoad(ByVal strfiletype As String, ByVal lngfileid As Long, Optional ByVal FileGivenName As String = Nothing)

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
            Dim strErrMsg As String
            Dim IsCutoff As Boolean
            'Dim lngFileId As Long
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
            'Dim strFileType As String = ""
            Dim strFileName As String = ""
            Dim strGivenName As String = ""
            Dim intCheck As Int16
            Dim strHeader4 As String = ""
            Dim strTableName As String = ""
            Dim strFooter1 As String = ""
            Dim strFooter2 As String = ""
            Dim strFooter7 As String = ""
            Dim intDisplay As Int16
            Dim Transaction_Charges As Double = 0

            Try

                If Not ss_strUserType = gc_UT_Reviewer Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If


                trBack.Visible = False                                                                  'Hide Table Row
                lblHeading.Text = "File Review"                                                         'Set Heading
                'strFileType = Request.QueryString("FT")                                                 'Get File Type
                If FileGivenName <> Nothing Then
                    strGivenName = FileGivenName
                Else
                    strGivenName = Request.QueryString("FN")
                End If 'Get File Name
                If IsDate(Request.QueryString("VD")) Then
                    dtValueDt = CDate(Request.QueryString("VD"))
                End If
                'Get Value Date
                'lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'Get File Id
                hFileId.Value = lngfileid

                'Disable Button Command on Click
                Call clsCommon.fncBtnDisable(btnConfirm, True)

                If Not Page.IsPostBack Then
                    Me.txtAuthCode.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + Me.btnConfirm.ClientID + "').click();return false;}} else {return true}; ")

                    'BindBody(body)

                    'Get Authorization Lock Status - Start
                    strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                    If strAuthLock = "Y" Then
                        strErrMsg = "Sorry! cannot Review the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileList.aspx", False)
                        Exit Try
                    End If
                    'Get Authorization Lock Status - Stop

                    'Check File Already Submitted - START
                    strFileStatus = clsCommon.fncBuildContent("File ReCheck", "", lngfileid, ss_lngUserID)
                    If Not strFileStatus = "" Then
                        strErrMsg = "The " & strfiletype & " (" & IIf(strfiletype = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") " & " has already been " & strFileStatus
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strErrMsg & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileList.aspx", False)
                        Exit Try
                    End If
                    'Check File Already Submitted - STOP

                    'Check File Value Date Expired - Start
                    If (strfiletype = "Payroll File" OrElse strfiletype = _Helper.DirectDebit_Name OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name) AndAlso dtValueDt < Today Then
                        'Build Subject
                        strSubject = strfiletype & " Review Failed - " & IIf(strfiletype = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & " Payment Date: " & dtValueDt
                        'Build Body
                        strBody = "The Payment Date " & "(" & dtValueDt & ") for the " & strfiletype & " has expired. Please rename and upload the file with a future Payment Date."
                        'Block File
                        Call clsCommon.prcBlockFile(lngfileid, 4, ss_lngOrgID, ss_lngUserID)
                        'Get File Name
                        strFileName = clsCommon.fncBuildContent("File Name", strfiletype, lngfileid, ss_lngUserID)
                        'Delete File
                        'Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)
                        'Move the file to rejected folder
                        Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, strfiletype, strFileName)
                        'Send Mails
                        Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                        'Display Alert
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strBody & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileList.aspx", False)
                        Exit Try
                    End If
                    'Check File Value Date Expired - Stop

                End If


                If strfiletype = "Payroll File" Then
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
                ElseIf strfiletype = _Helper.DirectDebit_Name Then
                    strFooter6 = ""
                    trNote.Visible = False
                    strOption = "D"
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


                If Not Page.IsPostBack Then

                    'Check Cutoff Time - Start
                    If strfiletype = "Payroll File" OrElse strfiletype = _Helper.DirectDebit_Name _
                      OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name _
                        OrElse strfiletype = _Helper.AutopaySNA_Name OrElse strfiletype = _Helper.PayLinkPayRoll_Name Then

                        IsCutoff = clsCommon.fncCutoffTime(Request.QueryString("FT"), ss_lngOrgID, ss_lngUserID, strTime, _
                                    Day(dtValueDt), Month(dtValueDt), Year(dtValueDt), strOption)
                    End If
                    If IsCutoff Then
                        'Build Subject
                        strSubject = strfiletype & " Review Failed - " & lblFName.Text & " Payment Date: " & dtValueDt
                        'Build Body
                        strBody = "The " & strfiletype & "(" & IIf(strfiletype = _Helper.CPS_Name, strGivenName.Substring(strGivenName.LastIndexOf(".") - 7), strGivenName) & ") for Payment Date " & dtValueDt & " cannot be Reviewed after the Cutoff Time (" & strTime & ")."
                        'Block File
                        Call clsCommon.prcBlockFile(lngfileid, 4, ss_lngOrgID, ss_lngUserID)
                        'Get File Name
                        strFileName = clsCommon.fncBuildContent("File Name", Request.QueryString("FT"), lngfileid, ss_lngUserID)
                        'Delete File
                        'Call clsCommon.prcDelFile(ss_lngOrgID, ss_lngUserID, Request.QueryString("FT"), strFileName)
                        Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, Request.QueryString("FT"), strFileName)
                        'Send Mails
                        Call clsCommon.prcSendMails("CUSTOMER", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strSubject, strBody)
                        'display alert message
                        Response.Write("<script language='JavaScript'>")
                        Response.Write("alert('" & strBody & "');")
                        Response.Write("</script>")
                        Server.Transfer("PG_FileList.aspx", False)
                        'Display Alert
                        Exit Try
                    End If
                    'Check Cutoff Time - Stop

                    trAuth.Visible = False
                    trSubmit.Visible = True
                    tblForm.Visible = False
                    trConfirm.Visible = False

                    'Populate Remarks Data Grid - Start
                    dsFileReview = clsCommon.fncListRemarks(lngfileid, ss_lngOrgID, ss_lngUserID)
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



                    'Populate Data Set
                    dsFileReview = clsCommon.fncGetRequested("File Review", ss_lngOrgID, ss_lngUserID, lngfileid, Request.QueryString("FT"))

                    'Loop Thro Data Set - START
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
                                strFooter4 = "Total Charge: RM" & Format(CDbl(_CPSPhase3.GetOrgCharges(lngfileid, ss_lngOrgID)), "##,##0.00")
                                strFooter2 = ""

                            Else
                                strFooter4 = "Total Charge: RM" & Format(CDbl(drFileReview("TCHRG")), "##,##0.00")
                            End If

                            hChrg.Value = "RM " & Format(CDbl(drFileReview("TCHRG")), "##,##0.00")

                            ''This Footer/Header not required in CPS
                            If strfiletype = _Helper.CPSDividen_Name Or strfiletype = _Helper.CPSMember_Name _
                            Or strfiletype = _Helper.CPSDelimited_Dividen_Name Or strfiletype = _Helper.CPSSingleFileFormat_Name Then
                                strFooter5 = ""
                                strHeader6 = ""
                                hChrg.Value = "RM " & Format(CDbl(_CPSPhase3.GetOrgCharges(lngfileid, ss_lngOrgID)), "##,##0.00")
                                If strfiletype = _Helper.CPSSingleFileFormat_Name Then
                                    strFooter5 = "Hash Total: " & drFileReview("HTOTAL")
                                End If
                            Else
                                strFooter5 = "Hash Total: " & drFileReview("HTOTAL")
                                strHeader6 = "Contribution Month: " & drFileReview("CMONTH")

                            End If

                        End If

                        lngFormatId = drFileReview("FRID")
                    Next
                    'Loop Thro Data Set - STOP

                    If strfiletype = "Mandate File" Then
                        strTableName = "tCor_MandatesFileDetails"
                    Else
                        strTableName = clsUpload.fncGetDBTableName(lngFormatId)
                    End If


                    strHeader1 = "Date: " & Today
                    strHeader2 = "File Type: " & strfiletype
                    If strfiletype = "Payroll File" OrElse strfiletype = _Helper.DirectDebit_Name OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                        strHeader4 = "Payment Date: " & txtDate.Value
                    Else
                        strHeader4 = "Upload Date: " & txtDate.Value
                    End If

                    If strfiletype = _Helper.CPS_Name Then
                        strHeader5 = "File Name: " & strGivenName.Substring(strGivenName.LastIndexOf(".") - 7) & " (" & txtDate.Value & ")"
                    Else
                        strHeader5 = "File Name: " & strGivenName
                    End If

                    If strfiletype = "Payroll File" Or strfiletype = _Helper.DirectDebit_Name OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                        strHeader6 = ""
                    End If


                    'Check If User Can See Detail - Start
                    intDisplay = clsCommon.fncBuildContent("Display", "", ss_lngOrgID, ss_lngUserID)
                    If intDisplay = 2 Then
                        lblType.Text = strfiletype
                        If strfiletype = _Helper.CPS_Name Then
                            lblFName.Text = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7) & " (" & txtDate.Value & ")"

                        Else
                            lblFName.Text = strGivenName
                        End If
                        lblAmt.Text = hAmount.Value
                        lblTran.Text = hTrans.Value
                        lblChrg.Text = hChrg.Value
                        lblPHTotal.Text = Session("CUST_PHTOTAL")
                        tblForm.Visible = True
                        tblReview.Visible = False
                    End If
                    'Check If User Can See Detail - Start
                End If
                'Check if Same Payment Date & Same Amount has already been Authorized - Start
                If Not Page.IsPostBack Then

                    intCheck = clsCommon.fncUploadCheck("FINAL ALERT", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, strfiletype, dtValueDt, hAmount.Value)
                    If intCheck > 0 Then
                        'lblMessage.Text = "Please Note: A " & strFileType & " file with the same Payment Date & Total Amount has already beeSn reviewed by you. Please check file status Report or contact eHR<sup>2</sup> registration centre at 03-9280 6657."
                        Select Case strfiletype.Substring(0, 1).ToLower
                            Case "a", "e", "i", "o", "u"
                                lblMessage.Text = "Please Note: An " & strfiletype
                            Case Else
                                lblMessage.Text = "Please Note: A " & strfiletype
                        End Select
                        If strfiletype = "Payroll File" OrElse strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.CPS_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                            lblMessage.Text += " with the same Payment Date & Total Amount has already been reviewed by you. Please check the File Status Report or contact Customer Relations at " & gc_Const_CompanyContactNo & "."
                        Else
                            lblMessage.Text += " with the same Upload Date & Total Amount has already been reviewed by you. Please check the File Status Report or contact Customer Relations at " & gc_Const_CompanyContactNo & "."
                        End If

                    End If


                    If intDisplay = 1 Then
                        rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                        rvReport.ServerReport.ReportPath = strReportDir & "DynamicReportDetails"

                        ''Hafeez - NTA - Check if DIV, request for Member file as well
                        Dim dsFieldName As New DataSet
                        dsFieldName = clsCommon.fncGetRequested("Field Position", ss_lngOrgID, ss_lngUserID, lngFormatId, strfiletype)

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

                        ElseIf strfiletype = _Helper.CPSDelimited_Dividen_Name Then

                            Dim drTempRow As DataRow

                            iCount = 0
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

                        ElseIf strfiletype = _Helper.Autopay_Name OrElse strfiletype = _Helper.AutopaySNA_Name Then
                            For iCount = 0 To 7
                                rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(dsFieldName.Tables(0).Rows(iCount).Item(0)))
                            Next
                        ElseIf strfiletype = _Helper.CPSDividen_Name Or strfiletype = _Helper.CPSMember_Name Then

                            Dim drTempRow As DataRow

                            'Transaction_Charges = _CPSPhase3.GetOrgCharges(lngFileId, ss_lngOrgID)
                            iCount = 0
                            If strfiletype = _Helper.CPSDividen_Name Then
                                rvReport.ServerReport.ReportPath = strReportDir & "DynamicReportDetails"
                                For Each drTempRow In dsFieldName.Tables(0).Rows
                                    If drTempRow.Item(0) = "DIV_MEMNO" _
                                       OrElse drTempRow.Item(0) = "DIV_SHARE" _
                                       OrElse drTempRow.Item(0) = _CPSPhase3.GetChequeCol_Name(lngfileid, ss_lngOrgID) _
                                       OrElse drTempRow.Item(0) = "DIV_WRNTNO" _
                                       OrElse drTempRow.Item(0) = "DIV_DATE" _
                                       OrElse drTempRow.Item(0) = "DIV_NAME" Then

                                        rptParam(iCount) = New Microsoft.Reporting.WebForms.ReportParameter("in_Field_" & (iCount + 1).ToString, CStr(drTempRow.Item(0)))

                                        iCount += 1
                                    End If
                                Next
                            ElseIf strfiletype = _Helper.CPSMember_Name Then
                                rvReport_2.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                                rvReport_2.ServerReport.ReportPath = strReportDir & "DynamicReportDetails"

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
                        ElseIf strfiletype = _Helper.CPSSingleFileFormat_Name Then
                            rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                            rvReport.ServerReport.ReportPath = strReportDir & "SFF_FileStatusDetails"
                            Dim rptParamSFF(0) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParamSFF(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileId", lngfileid)

                            rvReport.ServerReport.SetParameters(rptParamSFF)

                            rptParamSFF = Nothing
                            rvReport.ShowBackButton = True
                            Exit Try


                            '**Added by Naresh to generate the Payroll file report-11-02-11
                        ElseIf strfiletype = _Helper.PayLinkPayRoll_Name Then
                            rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                            rvReport.ServerReport.ReportPath = strReportDir & "PaylinkFile"
                            Dim rptParamInfenion(0) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParamInfenion(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileId", lngfileid)

                            rvReport.ServerReport.SetParameters(rptParamInfenion)

                            rptParamInfenion = Nothing
                            rvReport.ShowBackButton = True
                            Exit Try


                        ElseIf strfiletype = "CPS File" Then

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
                        rptParam(25) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileID", lngfileid)


                        If strfiletype = _Helper.CPSMember_Name Then
                            rvReport_2.ServerReport.SetParameters(rptParam)
                            rvReport_2.ShowBackButton = True
                        Else
                            rvReport.ServerReport.SetParameters(rptParam)
                        End If
                        rptParam = Nothing
                        rvReport.ShowBackButton = True
                        rvReport.ServerReport.Refresh()
                    Else
                        pnlReport.Visible = False
                    End If
                End If
                rvReport.ServerReport.Refresh()

            Catch ex As Exception
                If Err.Description <> "Thread was being aborted." Then

                    'Hide Tables
                    tblForm.Visible = False
                    tblReview.Visible = False

                    'Display Message
                    lblMessage.Text = "Sorry! Report Could Not be Loaded. Please try again."

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
            'Dim lngUserId As Long
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
            Dim strDateType As String
            Dim IsSubmit As Boolean
            Dim FileTypeId As Short = 0
            Dim ServiceId As Short = 0
            Dim SqlStatement As String = Nothing
            Dim ServiceType As Short = 0

            Try
                'reintialize the Message lable value
                lblMessage.Text = ""

                strDate = Request.Form("ctl00$cphContent$txtDate")                                                                                           'Get Value Date
                strFileType = Request.QueryString("FT")                                                                                     'Get File Type
                strGivenName = Request.QueryString("FN")                                                                                    'Get File Name
                strUserIP = Request.ServerVariables("REMOTE_ADDR")                                                                          'User IP Address
                strReason = IIf(txtReason.Text = "", "NA", txtReason.Text)                                                                  'Reason/Remarks
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                                                    'Get Organisation Id
                'lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)                                                 'Get User Id
                lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)
                lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)                                         'File Id  
                lngFlowId = IIf(IsNumeric(Request.QueryString("WfId")), Request.QueryString("WfId"), 0)                                     'WorkFlow Id
                FileTypeId = MaxGeneric.clsGeneric.NullToShort(Request.QueryString("FTID"))
                ServiceId = MaxGeneric.clsGeneric.NullToShort(Request.QueryString("SID"))

                'Get service type based on serviceId -start
                If ServiceId > 0 Then
                    SqlStatement = _Helper.GetSQLCommon & "'ServiceType'," & ServiceId

                    ServiceType = MaxGeneric.clsGeneric.NullToShort(MaxMiddleware.PPS.SQLScalarValue( _
                                     _Helper.GetSQLConnection, _Helper.GetSQLTransaction, String.Empty, SqlStatement))
                End If
                'Get service type based on serviceId -stop

                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _
                    _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                    strDateType = "Payment"
                Else
                    strDateType = "Upload"
                End If

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

                If FileTypeId > 0 And ServiceId > 0 And Not ServiceType = Helper.ServiceType.Mandates Then
                    'Check cutoff-start
                    If _ReadWriteGeneric.IsCutoffTime(FileTypeId, strDate, ServiceId, String.Empty, strTime) Then
                        IsCutoff = True
                    End If
                    'Check cutoff-stop
                End If

                '' ** Modified by Teja on 21/02/2011 for Adding reject code and fine tuning - Start
                If FileTypeId = 0 And ServiceId = 0 Then

                    IsCutoff = clsCommon.fncCutoffTime(strFileType, lngOrgId, lngUserId, strTime, _
                                    Day(txtDate.Value), Month(txtDate.Value), Year(txtDate.Value), _
                                        _Common.GetFileTypeIdentity(strFileType))
                End If

                If IsCutoff Then
                    'Build Subject
                    strSubject = strFileType & " Review Failed - " & lblFName.Text & " Payment Date: " & txtDate.Value
                    'Build Body
                    strBody = "The " & Request.QueryString("FT") & "(" & lblFName.Text & ") for Payment Date " & txtDate.Value & " cannot be Reviewed after the Cutoff Time (" & strTime & ")."
                    'Block File
                    Call clsCommon.prcBlockFile(lngFileId, 4, lngOrgId, lngUserId)
                    'Get File Name
                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, lngUserId)
                    'Delete File
                    'Call clsCommon.prcDelFile(lngOrgId, lngUserId, Request.QueryString("FT"), strFileName)
                    Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, strFileType, strFileName)
                    'Send Mails
                    Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                    'Display Alert
                    _Common.JavaScriptAlert(strBody)
                    Server.Transfer("PG_FileList.aspx", False)
                    Exit Try
                End If

                '' ** Modified by Teja on 21/02/2011 for Adding reject code and fine tuning - Stop

                'Check Cutoff Time - Start
                'If hCommand.Value = "A" Then

                ''Get Table Name - START
                'If strFileType = "Payroll File" Then
                '    'Check If Privilege Customer
                '    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)                                               'Get Privilege User
                'ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                '    strOption = "E"
                'ElseIf strFileType = "SOCSO File" Then
                '    strOption = "S"
                'ElseIf strFileType = "LHDN File" Then
                '    strOption = "L"
                'ElseIf strFileType = _Helper.DirectDebit_Name Then
                '    strOption = "D"
                'ElseIf strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                '    strOption = clsCommon.fncBuildContent("Privilege", "", lngOrgId, lngUserId)
                'ElseIf strFileType = _Helper.CPS_Name Then
                '    strOption = "C"
                'End If
                ''Get Table Name - STOP

                'Check If File Reached Cutoff Time
                'If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                '    IsCutoff = clsCommon.fncCutoffTime(Request.QueryString("FT"), lngOrgId, lngUserId, strTime, _
                '                                       Day(txtDate.Value), Month(txtDate.Value), Year(txtDate.Value), strOption)
                'End If

                'If IsCutoff Then
                '    'Build Subject
                '    strSubject = Request.QueryString("FT") & " Review Failed - " & lblFName.Text & " Payment Date: " & txtDate.Value
                '    'Build Body
                '    strBody = "The " & Request.QueryString("FT") & "(" & lblFName.Text & ") for Payment Date " & txtDate.Value & " cannot be Reviewed after the Cutoff Time (" & strTime & ")."
                '    'Block File
                '    Call clsCommon.prcBlockFile(lngFileId, 4, lngOrgId, lngUserId)
                '    'Get File Name
                '    strFileName = clsCommon.fncBuildContent("File Name", Request.QueryString("FT"), lngFileId, lngUserId)
                '    'Delete File
                '    'Call clsCommon.prcDelFile(lngOrgId, lngUserId, Request.QueryString("FT"), strFileName)
                '    Call clsCommon.prcMoveRejectedFile(lngOrgId, lngUserId, Request.QueryString("FT"), strFileName)
                '    'Send Mails
                '    Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, strSubject, strBody)
                '    'Display Alert
                '    Response.Write("<script language='JavaScript'>")
                '    Response.Write("alert('" & strBody & "');")
                '    Response.Write("</script>")
                '    Server.Transfer("PG_FileList.aspx", False)
                '    Exit Try
                'End If

                'End If
                'Check Cutoff Time - Stop

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
                            intAttempts = intAttempts + 1
                            Session("AUTH_LOCK") = intAttempts
                            lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
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
                            strErrMsg = "Sorry! cannot Review the file as Your Validation Code has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                            'Response.Write("<script language='JavaScript'>")
                            'Response.Write("alert('" & strErrMsg & "');")
                            'Response.Write("</script>")
                            _Common.JavaScriptAlert(strErrMsg)
                            Server.Transfer("PG_FileList.aspx", False)
                            Exit Try
                        End If
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP

                'Accept/Reject File - START
                If hCommand.Value = "A" Then

                    'Update Workflow
                    Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, lngOrgId, lngUserId, "R", "N", strReason, strUserIP, 0, lngGroupId, "Y")

                    'Get Count of Balance Authorizers
                    intFlowCount = clsUsers.fnRoleCount("REVIEWER", "A", "PENDING", lngFileId, lngGroupId)

                    If strFileType = _Helper.CPSMember_Name Then

                        'Update WorkFlow Dividend
                        Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, lngOrgId, lngUserId, "R", "N", strReason, strUserIP, 0, 0, "Y")

                    End If

                    'Get Last Authorizer Submit File - Start
                    If intFlowCount = 0 Then

                        If strFileType = _Helper.CPSMember_Name Then
                            'Build Subject
                            strSubject = "The " & strFileType & "and " & _Helper.CPSDividen_Name & " Reviewed & Submitted - " & _
                                strGivenName & "," & FileGivenName & " " & strDateType & " Date: " & strDate & ", Submission Date: " & _
                                    Now.ToShortDateString

                            'Build Body
                            strBody = "The " & strFileType & " and " & _Helper.CPSDividen_Name & " with " & strDateType & " Date: " & _
                                strDate & " Reviewed & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & _
                                    "<br>Remarks: " & strReason

                            'Display Message
                            lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") and " & _Helper.CPSDividen_Name & _
                            "(" & FileGivenName & ")with " & strDateType & " Date: " & strDate & " has been Reviewed & Submitted Successfully." & _
                                    "<br>Submisson Date: " & Now.ToShortDateString()

                        Else
                            'Build Subject
                            strSubject = "The " & strFileType & " Reviewed & Submitted - " & strGivenName & ", " & strDateType _
                                & " Date: " & strDate & ", Submission Date: " & Now.ToShortDateString

                            'Build Body
                            strBody = "The " & strFileType & " with " & strDateType & " Date: " & strDate _
                                & " Reviewed & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & "<br>Remarks: " _
                                    & strReason

                            'Display Message
                            lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " & strDate _
                                & " has been Reviewed & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString()

                        End If

                        ''EMAIL SEND START
                        Dim clsEmail As New clsEmail
                        If strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name Or _
                        strFileType = _Helper.CPSDelimited_Dividen_Name Or strFileType = _Helper.CPSSingleFileFormat_Name Then

                            Call clsCommon.prcSendMails("BANK USER", lngOrgId, lngUserId, 0, strSubject, strBody, 0)
                        ElseIf strFileType = "Mandate File" Then
                            Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserId, 0, strSubject, strBody, 0)
                        End If
                        ''EMAIL SEND STOP


                    Else
                        If strFileType = _Helper.CPSMember_Name Then
                            'Build Subject
                            strSubject = "The " & strFileType & "and " & _Helper.CPSDividen_Name & " Reviewed & Submitted - " & _
                                strGivenName & "," & FileGivenName & " " & strDateType & " Date: " & strDate & ", Submission Date: " & _
                                    Now.ToShortDateString

                            'Build Body
                            strBody = "The " & strFileType & " and " & _Helper.CPSDividen_Name & " with " & strDateType & " Date: " & _
                                strDate & " Reviewed & Submitted Successfully.<br>Submisson Date: " & Now.ToShortDateString() & _
                                    "<br>Remarks: " & strReason

                            'Display Message
                            lblMessage.Text = "The " & strFileType & "(" & strGivenName & ") and " & _Helper.CPSDividen_Name & _
                            "(" & FileGivenName & ")with " & strDateType & " Date: " & strDate & " has been Reviewed & Submitted Successfully." & _
                                    "<BR>Please Note: " & intFlowCount & " more Reviewer(s) to Approve the file." & gc_BR

                        Else
                            'Build Subject
                            strSubject = "The " & strFileType & " Reviewed - " & strGivenName & ", " & strDateType & " Date: " & strDate

                            'Build Body
                            strBody = "The " & strFileType & " with " & strDateType & " Date: " _
                                & strDate & " Reviewed Successfully. Remarks: " & strReason

                            'Display Success Message and Balance No of Authorizers
                            lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with " & strDateType & " Date: " _
                                & strDate & " has been Reviewed Successfully.<BR>Please Note: " & intFlowCount _
                                    & " more Reviewer(s) to Approve the file." & gc_BR

                        End If

                    End If
                    'Get Last Authorizer Submit File - Stop

                    '' ** Modified by Teja on 21/02/2011 for Adding reject code and fine tuning - Stop
                ElseIf hCommand.Value = "R" Then

                    'Build Subject
                    strSubject = "The " & strFileType & " Rejected - " & strGivenName & ", " & strDateType & " Date: " & strDate
                    'Build Body
                    strBody = "The " & strFileType & "(" & strGivenName & ") with Payment Date " & strDate & " Rejected Successfully. Remarks: " & strReason

                    'Update Remakrs/Reason
                    Call clsCommon.fncRemarks(lngFileId, strReason)

                    'Get File Name
                    strFileName = clsCommon.fncBuildContent("File Name", strFileType, lngFileId, ss_lngUserID)

                    'Move the rejected file to rejected folder
                    Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, strFileType, strFileName)

                    'Update Workflow - Start
                    Call clsCommon.prcWorkFlow(lngFileId, lngFlowId, ss_lngOrgID, ss_lngUserID, "R", "Y", _
                        strReason, strUserIP, 0, ss_lngGroupID, "Y")
                    'Update Workflow - Stop

                    'Call clsCommon.prcDeleteRejTrans(strFileid, ss_lngOrgID, ss_lngUserID, strFileType)

                    ''Added for Member File Start
                    If strFileType = _Helper.CPSMember_Name Then
                        Dim strDivFileName As String = ""
                        strDivFileName = clsCommon.fncBuildContent("File Name", _Helper.CPSDividen_Name, DivFileId, ss_lngUserID)
                        Call clsCommon.prcMoveRejectedFile(ss_lngOrgID, ss_lngUserID, _Helper.CPSDividen_Name, strDivFileName)
                        Call clsCommon.prcWorkFlow(DivFileId, lngSubFlowId, ss_lngOrgID, ss_lngUserID, _
                            "R", "Y", strReason, strUserIP, 0, ss_lngGroupID, "Y")
                    End If
                    ''Added for Member File Stop

                    'Display Message
                    lblMessage.Text += "The " & strFileType & "(" & strGivenName & ") with Payment Date: " & _
                        strDate & " has been Rejected Successfully." & gc_BR

                End If
                'Accept/Reject File - STOP
                '' ** Modified by Teja on 21/02/2011 for Adding reject code and fine tuning - Stop

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
                tblForm.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                tblReview.Visible = False
                txtReason.ReadOnly = True
                lblType.Text = strFileType
                lblChrg.Text = hChrg.Value
                lblAmt.Text = hAmount.Value
                lblTran.Text = hTrans.Value
                If strFileType = _Helper.CPS_Name Then
                    lblFName.Text = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                ElseIf strFileType = _Helper.CPSMember_Name Then
                    lblFName.Text = FileGivenName & " and " & strGivenName
                Else
                    lblFName.Text = strGivenName
                End If
                lblPHTotal.Text = Session("CUST_PHTOTAL")
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
                tblForm.Visible = True
                trSubmit.Visible = False
                trConfirm.Visible = True
                tblReview.Visible = False
                txtReason.ReadOnly = True
                lblType.Text = strFileType
                lblChrg.Text = hChrg.Value
                lblAmt.Text = hAmount.Value
                lblTran.Text = hTrans.Value
                If strFileType = _Helper.CPS_Name Then
                    lblFName.Text = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                ElseIf strFileType = _Helper.CPSMember_Name Then
                    lblFName.Text = FileGivenName & " and " & strGivenName
                Else
                    lblFName.Text = strGivenName
                End If
                lblPHTotal.Text = Session("CUST_PHTOTAL")
                lblHeading.Text = "File Review - Confirmation"
                lblMessage.Text = "Please Enter your Validation Code and Confirm File Review."

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

#Region "Get Query String - Remain"
        Private Sub GetQuery()
            strFileType = Request.QueryString("FT")                                                 'Get File Type
            lngFileId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)     'Get File Id
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)                                                 'Get User Id


        End Sub

#End Region

    End Class

End Namespace
