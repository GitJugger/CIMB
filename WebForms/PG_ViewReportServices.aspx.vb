Option Strict Off
Option Explicit On 

Imports System
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Reflection
'Imports MaxPayroll.clsReporter



Namespace MaxPayroll


   Partial Class PG_ViewReportServices
        Inherits clsBasePage

#Region "Request.QueryString"
      Private ReadOnly Property rq_strReportName() As String
         Get
            Return Request.QueryString("ReportName") & ""
         End Get
      End Property
      Private ReadOnly Property rq_intPeriod() As Integer
         Get
            If IsNumeric(Request.QueryString("Period")) Then
               Return Request.QueryString("Period")
            Else
               Return 0
            End If
         End Get
      End Property
      Private ReadOnly Property rq_intMonth() As Integer
         Get
            If IsNumeric(Request.QueryString("Month")) Then
               Return Request.QueryString("Month")
            Else
               Return 0
            End If
         End Get
      End Property
      Private ReadOnly Property rq_intYear() As Integer
         Get
            If IsNumeric(Request.QueryString("Year")) Then
               Return Request.QueryString("Year")
            Else
               Return 0
            End If
         End Get
      End Property

      Private ReadOnly Property rq_intOrgId() As Integer
         Get
            If IsNumeric(Request.QueryString("ID")) Then
               Return Request.QueryString("ID")
            Else
               If Not Request.QueryString("LoadOrgIDSession") Is Nothing Then
                  Return ss_lngOrgID
               Else
                  Return 0
               End If
            End If
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
#End Region

#Region "Page Load Method"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Page.IsPostBack = False Then
                Try
                    'BindBody(body, True, False)
                    rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
                    rvReport.ServerReport.ReportPath = strReportDir & rq_strReportName

                    Select Case rq_strReportName

                        Case gc_RptDormant
                            Dim rptParam(0) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_Period", rq_intPeriod)
                            rvReport.ServerReport.SetParameters(rptParam)
                            rptParam = Nothing

                        Case gc_RptTransaction, gc_RptUserExpiry
                            Dim rptParam(1) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_Month", rq_intMonth)
                            rptParam(1) = New Microsoft.Reporting.WebForms.ReportParameter("in_Year", rq_intYear)
                            rvReport.ServerReport.SetParameters(rptParam)
                            rptParam = Nothing
                        Case gc_RptFileStatus
                            rvReport.ShowBackButton = True
                            Dim clsCommon As New clsCommon

                            Dim intDisplay As Integer
                            Dim rptParam(3) As Microsoft.Reporting.WebForms.ReportParameter
                            'Dim rptParam(2) As Microsoft.Reporting.WebForms.ReportParameter 
                            If ss_strUserType.Equals(gc_UT_BankUser) AndAlso rq_intOrgId > 0 Then
                                rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_OrgId", rq_intOrgId)
                                rptParam(2) = New Microsoft.Reporting.WebForms.ReportParameter("in_GroupId", 0)
                            Else
                                rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_OrgId", ss_lngOrgID)
                                rptParam(2) = New Microsoft.Reporting.WebForms.ReportParameter("in_GroupId", ss_lngGroupID)
                            End If
                            'rptParam(1) = New Microsoft.Reporting.WebForms.ReportParameter("in_UserType", ss_strUserType)
                            rptParam(1) = New Microsoft.Reporting.WebForms.ReportParameter("in_User", ss_strUserType)
                            intDisplay = clsCommon.fncBuildContent("Display", "", ss_lngOrgID, ss_lngUserID)


                            If intDisplay = 1 Then
                                rptParam(3) = New Microsoft.Reporting.WebForms.ReportParameter("in_DisplayDetail", True)
                            Else
                                rptParam(3) = New Microsoft.Reporting.WebForms.ReportParameter("in_DisplayDetail", False)
                            End If

                            rvReport.ServerReport.SetParameters(rptParam)
                            rptParam = Nothing
                        Case gc_RptFileSubmission
                            Dim rptParam(6) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_Request", Request.QueryString("Option"))
                            rptParam(1) = New Microsoft.Reporting.WebForms.ReportParameter("in_FromDt", Request.QueryString("FromDt"))
                            rptParam(2) = New Microsoft.Reporting.WebForms.ReportParameter("in_ToDt", Request.QueryString("ToDt"))
                            rptParam(3) = New Microsoft.Reporting.WebForms.ReportParameter("in_Status", Request.QueryString("Status"))
                            rptParam(4) = New Microsoft.Reporting.WebForms.ReportParameter("in_SortBy", Request.QueryString("Sort"))
                            rptParam(5) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileType", Request.QueryString("File"))
                            rptParam(6) = New Microsoft.Reporting.WebForms.ReportParameter("in_User", ss_strUserType)
                            rvReport.ServerReport.SetParameters(rptParam)
                            rptParam = Nothing
                        Case gc_RptStopPayment
                            Dim rptParam(8) As Microsoft.Reporting.WebForms.ReportParameter

                            rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_Option", "Interceptor")
                            rptParam(1) = New Microsoft.Reporting.WebForms.ReportParameter("in_OrgId", ss_lngOrgID)
                            rptParam(2) = New Microsoft.Reporting.WebForms.ReportParameter("in_OrgName", "")
                            rptParam(3) = New Microsoft.Reporting.WebForms.ReportParameter("in_FileName", "")
                            rptParam(4) = New Microsoft.Reporting.WebForms.ReportParameter("in_StopDt", Today)
                            rptParam(5) = New Microsoft.Reporting.WebForms.ReportParameter("in_Month", 0)
                            rptParam(6) = New Microsoft.Reporting.WebForms.ReportParameter("in_Year", 0)
                            rptParam(7) = New Microsoft.Reporting.WebForms.ReportParameter("in_FromDt", Today)
                            rptParam(8) = New Microsoft.Reporting.WebForms.ReportParameter("in_ToDt", Today)
                            'rptParam(9) = New Microsoft.Reporting.WebForms.ReportParameter("in_GroupId", Me.ss_lngGroupID)

                            rvReport.ServerReport.SetParameters(rptParam)
                            'Call clsReporter.prReportParameter("@in_Option", "Interceptor")
                            'Call clsReporter.prReportParameter("@in_OrgId", lngOrgId)
                            'Call clsReporter.prReportParameter("@in_OrgName", "")
                            'Call clsReporter.prReportParameter("@in_FileName", "")
                            'Call clsReporter.prReportParameter("@in_StopDt", Format(Today, "dd/MM/yyyy)"))
                            'Call clsReporter.prReportParameter("@in_Month", 0)
                            'Call clsReporter.prReportParameter("@in_Year", 0)
                            'Call clsReporter.prReportParameter("@in_FromDt", Format(Today, "dd/MM/yyyy)"))
                            'Call clsReporter.prReportParameter("@in_ToDt", Format(Today, "dd/MM/yyyy)"))
                            'Call clsReporter.prReportParameter("@in_GroupId", lngGroupId)
                        Case Else
                            Dim rptParam(0) As Microsoft.Reporting.WebForms.ReportParameter
                            rptParam(0) = New Microsoft.Reporting.WebForms.ReportParameter("in_OrgId", rq_intOrgId)
                            rvReport.ServerReport.SetParameters(rptParam)
                            rptParam = Nothing

                    End Select
                    rvReport.ServerReport.Refresh()

                    Dim clsUsers As New clsUsers
                    Select Case rq_strReportName
                        Case gc_RptFileStatus
                            Call clsUsers.prcDetailLog(ss_lngUserID, "File Status Report", "Y")
                        Case gc_RptOrganizationList
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Organization List Report", "Y")
                        Case gc_RptDormant
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Dormant Report", "Y")
                        Case gc_RptUserRole
                            Call clsUsers.prcDetailLog(ss_lngUserID, "User Role List Report", "Y")
                        Case gc_RptUploadList
                            Call clsUsers.prcDetailLog(ss_lngUserID, "File Upload List Report", "Y")
                    End Select
                    clsUsers = Nothing
                Catch ex As Exception
                    LogError("Pg_ViewReportServices")
                End Try
            End If



            'lblHeader.Visible = False
            'lngFileId = IIf(IsNumeric(Request.QueryString("FileId")), Request.QueryString("FileId"), 0)
            'lngCustId = IIf(IsNumeric(Trim(Request.QueryString("Id"))), Trim(Request.QueryString("Id")), 0)
            'If Request.QueryString("Report") = "" Then
            '    strReportName = Session("REPORT")
            'Else
            '    strReportName = Request.QueryString("Report")
            'End If
            'intPeriod = IIf(IsNumeric(Request.QueryString("Period")), Request.QueryString("Period"), 0)
            'intMonth = IIf(IsNumeric(Request.QueryString("Month")), Request.QueryString("Month"), 0)
            'intYear = IIf(IsNumeric(Request.QueryString("Year")), Request.QueryString("Year"), 0)
            'lngOrgId = IIf(IsNumeric(Session(gc_Ses_OrgId)), Session(gc_Ses_OrgId), 0)        'Get Organisation Id
            'lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id
            'lngGroupId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)  'Group Id

            'Dim BlockOrgId As Long = System.Configuration.ConfigurationManager.AppSettings("BLOCKID")

            'Select Case strReportName

            '    Case "Organisation"

            '        Dim rptOrganisations As New Organisation                                'Create Instance of Organisation Report
            '        clsReporter.prReportName = rptOrganisations                             'Report Name
            '        Call clsReporter.prReportConnection()                                   'Call Report Connection
            '        rptOrganisations = Nothing                                              'Destroy Instance of Organisation Report
            '        lblHeader.Text = "List of Registered Organisations"                     'Header Label
            '        Call clsUsers.prcDetailLog(lngUserId, "Organization List Report", "Y")

            '    Case "Upload"

            '        Dim rptUploads As New UploadList                                        'Create Instance of Upload Report
            '        clsReporter.prReportName = rptUploads                                   'Report Name
            '        Call clsReporter.prReportConnection()                                   'Call Report Connection
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        rptUploads = Nothing                                                    'Destroy Instance of Organisation Report
            '        lblHeader.Text = "List of Successful Uploads Organisation wise"         'Header Label
            '        Call clsUsers.prcDetailLog(lngUserId, "File Upload List Report", "Y")

            '    Case "Charges"

            '        Dim rptCharges As New TransactionList                                   'Create Instance of Upload Report
            '        clsReporter.prReportName = rptCharges                                   'Report Name
            '        Call clsReporter.prReportConnection()                                   'Call Report Connection
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        rptCharges = Nothing                                                    'Destroy Instance of Organisation Report
            '        lblHeader.Text = "List of Transaction Charges Organisation wise"        'Header Label
            '        Call clsUsers.prcDetailLog(lngUserId, "Transaction List Report", "Y")

            '    Case "Monthly"

            '        Dim rptMonthly As New AnnualTrans
            '        clsReporter.prReportName = rptMonthly
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_Month", intMonth)
            '        Call clsReporter.prReportParameter("@in_Year", intYear)
            '        rptMonthly = Nothing
            '        Call clsUsers.prcDetailLog(lngUserId, "Monthly Trans Report", "Y")

            '    Case "Roles"

            '        Dim rptUsers As New UsersList
            '        clsReporter.prReportName = rptUsers
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        rptUsers = Nothing
            '        Call clsUsers.prcDetailLog(lngUserId, "User Role List Report", "Y")

            '    Case "Lock"

            '        Dim rptLock As New PasswordLock
            '        clsReporter.prReportName = rptLock
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        rptLock = Nothing

            '    Case "Expiry"

            '        Dim rptExpiry As New UserExpiry
            '        clsReporter.prReportName = rptExpiry
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_Month", intMonth)
            '        Call clsReporter.prReportParameter("@in_Year", intYear)
            '        rptExpiry = Nothing

            '    Case "Users"

            '        Dim rptUsers As New UsersList
            '        clsReporter.prReportName = rptUsers
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngOrgId)
            '        rptUsers = Nothing

            '    Case "Logs"

            '        Dim rptLogs As New UsersLog
            '        clsReporter.prReportName = rptLogs
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngOrgId)
            '        rptLogs = Nothing

            '    Case "Status"

            '        Dim rptStatus As New CrystalDecisions.CrystalReports.Engine.ReportDocument

            '        rptStatus.Load(Server.MapPath("../App_Code/reports/FileStatus.rpt"))
            '        clsReporter.prReportName = rptStatus
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngOrgId)
            '        Call clsReporter.prReportParameter("@in_User", Session("SYS_TYPE"))
            '        Call clsReporter.prReportParameter("@in_GroupId", lngGroupId)

            '        rptStatus = Nothing

            '    Case "Status1"

            '        'Bank User Report
            '        Dim rptStatus As New FileStatus
            '        clsReporter.prReportName = rptStatus
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        Call clsReporter.prReportParameter("@in_User", Session("SYS_TYPE"))
            '        Call clsReporter.prReportParameter("@in_GroupId", 0)
            '        rptStatus = Nothing
            '        Call clsUsers.prcDetailLog(lngUserId, "File Status Report", "Y")

            '    Case "Dorm"

            '        Dim rptDorm As New DormantAccount
            '        clsReporter.prReportName = rptDorm
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_Period", intPeriod)
            '        rptDorm = Nothing
            '        Call clsUsers.prcDetailLog(lngUserId, "Dormant Report", "Y")

            '    Case "FileStatus"

            '        Dim rptStatus As New Status
            '        clsReporter.prReportName = rptStatus
            '        clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_FileId", lngFileId)
            '        Call clsReporter.prReportParameter("@in_User", Session("SYS_TYPE"))
            '        rptStatus = Nothing

            '    Case "Group"

            '        Dim rptGroup As New GroupInfo
            '        clsReporter.prReportName = rptGroup
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngOrgId)
            '        rptGroup = Nothing

            '    Case "Groups"

            '        Dim rptGroup As New GroupInfo
            '        clsReporter.prReportName = rptGroup
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        rptGroup = Nothing

            '    Case "Pin"

            '        Dim rptPinGen As New PinGen
            '        clsReporter.prReportName = rptPinGen
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
            '        rptPinGen = Nothing


            '    Case "FileSub"

            '        strToDt = Request.QueryString("ToDt")
            '        strSort = Request.QueryString("Sort")
            '        strOption = Request.QueryString("Option")
            '        strFromDt = Request.QueryString("FromDt")
            '        intStatus = Request.QueryString("Status")
            '        strFileType = Request.QueryString("File")

            '        Dim rptFileSub As New FileSub
            '        clsReporter.prReportName = rptFileSub
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_Request", strOption)
            '        Call clsReporter.prReportParameter("@in_FromDt", strFromDt)
            '        Call clsReporter.prReportParameter("@in_ToDt", strToDt)
            '        Call clsReporter.prReportParameter("@in_Status", intStatus)
            '        Call clsReporter.prReportParameter("@in_FileType", strFileType)
            '        Call clsReporter.prReportParameter("@in_SortBy", strSort)
            '        Call clsReporter.prReportParameter("@in_User", Session("SYS_TYPE"))
            '        rptFileSub = Nothing

            '    Case "StopPay"

            '        Dim rptStopPayment As New StopPayment
            '        clsReporter.prReportName = rptStopPayment
            '        Call clsReporter.prReportConnection()
            '        Call clsReporter.prReportParameter("@in_Option", "Interceptor")
            '        Call clsReporter.prReportParameter("@in_OrgId", lngOrgId)
            '        Call clsReporter.prReportParameter("@in_OrgName", "")
            '        Call clsReporter.prReportParameter("@in_FileName", "")
            '        Call clsReporter.prReportParameter("@in_StopDt", Format(Today, "dd/MM/yyyy)"))
            '        Call clsReporter.prReportParameter("@in_Month", 0)
            '        Call clsReporter.prReportParameter("@in_Year", 0)
            '        Call clsReporter.prReportParameter("@in_FromDt", Format(Today, "dd/MM/yyyy)"))
            '        Call clsReporter.prReportParameter("@in_ToDt", Format(Today, "dd/MM/yyyy)"))
            '        Call clsReporter.prReportParameter("@in_GroupId", lngGroupId)
            '        rptStopPayment = Nothing

            'End Select

            'crViewer.BestFitPage = False                                                    'Auto Page Fit Set to False
            'crViewer.DisplayGroupTree = False                                               'Group Display Set to False
            'crViewer.DisplayToolbar = True                                                  'Toolbar Display Set to False    
            'crViewer.HasExportButton = True                                                 'Export Button Set To False
            'crViewer.HasPrintButton = True                                                  'Print Button Set to False
            'crViewer.PageZoomFactor = 100                                                   'View Size 150 Percent
            'crViewer.Height = System.Web.UI.WebControls.Unit.Pixel(450)                     'Set Height
            'crViewer.Width = System.Web.UI.WebControls.Unit.Pixel(800)                      'Set Width
            'crViewer.ParameterFieldInfo.Clear()
            'crViewer.ReportSource = clsReporter.prReportName                                'Crystal Report Source

            'Catch

            '    'Log Error
            '    Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page Load - PG_ViewReports", Err.Number, Err.Description)

            'Finally

            '    'Destroy Instance of Generic Class Object
            '    clsUsers = Nothing
            '    clsGeneric = Nothing
            '    clsReporter = Nothing

            'End Try

        End Sub

#End Region

   End Class

End Namespace
