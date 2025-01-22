Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports System.Reflection
Imports System
'Imports MaxPayroll.clsReporter
Imports Microsoft.Reporting.WebForms

Namespace MaxPayroll


   Partial Class PG_SearchReportServicesByGlobalCriteria
      Inherits clsBasePage

#Region "Request.QueryString"
      Private ReadOnly Property rq_strReportName() As String
         Get
            Return Request.QueryString("ReportName") & ""
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

#Region "Page Load"

      '****************************************************************************************************
      'Procedure Name : Page_Load()
      'Purpose        : Page Load Functions
      'Arguments      : System Object,System Events Arguments
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 24/11/2004
      '*****************************************************************************************************
      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Variable Declarations
         Dim intCounter As Int16, intYear As Int16

         Try

            intYear = 2004
            '

            ddlYear.Items.Insert(0, "Select")
            For intCounter = 1 To 20
               ddlYear.Items.Insert(intCounter, intYear)
               intYear = intYear + 1
            Next
            Select Case rq_strReportName
               Case gc_RptStopPayment  'N/A
                  trAccNo.Visible = False
                  trCancel.Visible = False
                  trFileName.Visible = True
                  trStopDate.Visible = True
                  trCreateBy.Visible = False
                  trCreateDt.Visible = False
                  lblHeading.Text = "Stop Payment Report"
               Case gc_RptCancel 'N/A
                  trAccNo.Visible = False
                  trCancel.Visible = True
                  trFileName.Visible = False
                  trStopDate.Visible = False
                  trCreateBy.Visible = False
                  trCreateDt.Visible = False
                  lblHeading.Text = "Customer Cancellation Report"
               Case gc_RptOrganizationList
                  trAccNo.Visible = False
                  trCancel.Visible = False
                  trFileName.Visible = False
                  trStopDate.Visible = False
                  trCreateBy.Visible = True
                  trCreateDt.Visible = True
                        lblHeading.Text = "Organization List Report"
               Case gc_RptRegistration 'N/A
                  trAccNo.Visible = True
                  trCancel.Visible = False
                  trFileName.Visible = False
                  trStopDate.Visible = False
                  trCreateBy.Visible = False
                  trCreateDt.Visible = True
                  lblHeading.Text = "Customer Registration Report"
               Case gc_RptPinGeneration
                  trAccNo.Visible = False
                  trCancel.Visible = False
                  trFileName.Visible = False
                  trStopDate.Visible = False
                  trCreateBy.Visible = False
                  trCreateDt.Visible = True
                  trRequest.Visible = True
                  trApprove.Visible = True
                  lblHeading.Text = "Pin Generation Report"
            End Select


            If Page.IsPostBack = False Then
               'Call prcShowReport()
                    'BindBody(body)
            End If


         Catch

            'Log Error
            Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "PG_ReportSearch - Page_Load", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

         End Try

      End Sub

#End Region

#Region "Submit"

      Private Sub prcSubmit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSearch.Click

         If Me.rdOrgId.Checked AndAlso txtOrgId.Text.Trim = "" Then
            lblMessage.Text = "Please enter Organization ID."
            Exit Sub
         End If
         If Me.rdOrgName.Checked AndAlso txtOrgName.Text.Trim = "" Then
            lblMessage.Text = "Please enter Organization Name."
            Exit Sub
         End If
         If Me.rdCreateBy.Checked AndAlso Me.txtCreateBy.Text.Trim = "" Then
            lblMessage.Text = "Please enter User ID."
            Exit Sub
         End If
         If Me.rdCreateDt.Checked AndAlso Me.txtCreateDt.Text.Trim = "" Then
            lblMessage.Text = "Please enter Organization Creation Date."
            Exit Sub
         End If
         If rdFrom.Checked AndAlso (txtFromDt.Text.Trim = "" OrElse txtToDt.Text.Trim = "") Then
            lblMessage.Text = "Please enter Date Range."
            Exit Sub
         End If
         If Me.rdMonth.Checked AndAlso (Me.ddlMonth.SelectedValue = "" OrElse ddlYear.SelectedValue = 0) Then
            lblMessage.Text = "Please select value for Month and Year."
            Exit Sub
         End If

         Call prcShowReport()

      End Sub

#End Region

#Region "Display Report"

      Private Sub prcShowReport()

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of Reporter Class Object
         'Dim clsReporter As New MaxPayroll.clsReporter

         'Variable Declarations
         Dim strOption As String = "", strAccNo As String, strRequestBy As String, strApproveBy As String
         Dim strCancelBy As String, strStopDate As String, strCreateDate As String, strCreateBy As String
         Dim lngOrgId As Long, lngUserId As Long, lngCustId As Long, strOrgName As String, strFileName As String
         Dim intYear As Int16, strFromDate As String, strToDate As String, intMonth As Int16, strRequest As String

         Try

            tblForm.Visible = False
            tblMain.Visible = False
            rvReport.Visible = True
            strRequest = Request.QueryString("Report")
            lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                strToDate = Request.Form("ctl00$cphContent$txtToDt")
                strAccNo = Request.Form("ctl00$cphContent$txtAccNo")
                strOrgName = Request.Form("ctl00$cphContent$txtOrgName")
                strFromDate = Request.Form("ctl00$cphContent$txtFromDt")
                strFileName = Request.Form("ctl00$cphContent$txtFileName")
                strCancelBy = Request.Form("ctl00$cphContent$txtCancelBy")
                strStopDate = Request.Form("ctl00$cphContent$txtStopDate")
                strCreateBy = Request.Form("ctl00$cphContent$txtCreateBy")
                strCreateDate = Request.Form("ctl00$cphContent$txtCreateDt")
                strRequestBy = Request.Form("ctl00$cphContent$txtRequestBy")
                strApproveBy = Request.Form("ctl00$cphContent$txtApproveBy")
                intYear = IIf(IsNumeric(Request.Form("ctl00$cphContent$ddlYear")), Request.Form("ctl00$cphContent$ddlYear"), 0)
                intMonth = IIf(IsNumeric(Request.Form("ctl00$cphContent$ddlMonth")), Request.Form("ctl00$cphContent$ddlMonth"), 0)
                lngCustId = IIf(IsNumeric(Request.Form("ctl00$cphContent$txtOrgId")), Request.Form("ctl00$cphContent$txtOrgId"), 0)

            If rdOrgId.Checked Then
               strOption = "Org Id"
            ElseIf rdAll.Checked Then
               strOption = "All"
            ElseIf rdOrgName.Checked Then
               strOption = "Org Name"
            ElseIf rdCreateDt.Checked Then
               strOption = "Create Date"
            ElseIf rdCreateBy.Checked Then
               strOption = "Create By"
            ElseIf rdMonth.Checked Then
               strOption = "Month Year"
            ElseIf rdFrom.Checked Then
               strOption = "From To"
            ElseIf rdAccNo.Checked Then
               strOption = "Acc No"
            ElseIf rdFileName.Checked Then
               strOption = "File Name"
            ElseIf rdDate.Checked Then
               strOption = "Stop Date"
            ElseIf rdCancel.Checked Then
               strOption = "Cancel By"
            ElseIf rdRequestBy.Checked Then
               strOption = "Request By"
            ElseIf rdApproveBy.Checked Then
               strOption = "Approve By"
            End If

           

                rvReport.Visible = True
                pnlReport.Visible = True
            rvReport.ServerReport.ReportServerUrl = New System.Uri(strServerURL)
            rvReport.ServerReport.ReportPath = strReportDir & rq_strReportName
            Select Case rq_strReportName

               Case gc_RptStopPayment
                  Dim rptParam(8) As ReportParameter
                  If strOption.ToUpper = "ALL" Then
                     strOption = ""
                  End If
                  rptParam(0) = New ReportParameter("in_Option", strOption)
                  rptParam(1) = New ReportParameter("in_OrgId", lngCustId)
                  rptParam(2) = New ReportParameter("in_OrgName", strOrgName)
                  rptParam(3) = New ReportParameter("in_FileName", strFileName)
                  rptParam(4) = New ReportParameter("in_StopDt", strStopDate)
                  rptParam(5) = New ReportParameter("in_Month", intMonth)
                  rptParam(6) = New ReportParameter("in_Year", intYear)
                  rptParam(7) = New ReportParameter("in_FromDt", strFromDate)
                  rptParam(8) = New ReportParameter("in_ToDt", strToDate)

                  rvReport.ServerReport.SetParameters(rptParam)

               Case gc_RptCancel
                  Dim rptParam(7) As ReportParameter

                  rptParam(0) = New ReportParameter("in_Option", strOption)
                  rptParam(1) = New ReportParameter("in_OrgId", lngCustId)
                  rptParam(2) = New ReportParameter("in_OrgName", strOrgName)
                  rptParam(3) = New ReportParameter("in_CancelBy", strCancelBy)
                  rptParam(4) = New ReportParameter("in_Month", intMonth)
                  rptParam(5) = New ReportParameter("in_Year", intYear)
                  rptParam(6) = New ReportParameter("in_FromDt", strFromDate)
                  rptParam(7) = New ReportParameter("in_ToDt", strToDate)

                  rvReport.ServerReport.SetParameters(rptParam)
                  'Dim rptCancel As New OrgCancell
                  'clsReporter.prReportName = rptCancel
                  'Call clsReporter.prReportConnection()
                  'Call clsReporter.prReportParameter("@in_Option", strOption)
                  'Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
                  'Call clsReporter.prReportParameter("@in_OrgName", strOrgName)
                  'Call clsReporter.prReportParameter("@in_CancelBy", strCancelBy)
                  'Call clsReporter.prReportParameter("@in_Month", intMonth)
                  'Call clsReporter.prReportParameter("@in_Year", intYear)
                  'Call clsReporter.prReportParameter("@in_FromDt", strFromDate)
                  'Call clsReporter.prReportParameter("@in_ToDt", strToDate)
                  'rptCancel = Nothing

               Case gc_RptOrganizationList


                  Dim rptParam(8) As ReportParameter

                  rptParam(0) = New ReportParameter("in_Option", strOption)
                  rptParam(1) = New ReportParameter("in_OrgId", lngCustId)
                  rptParam(2) = New ReportParameter("in_OrgName", strOrgName)
                  rptParam(3) = New ReportParameter("in_CreateDt", strCreateDate)
                  rptParam(4) = New ReportParameter("in_CreateBy", strCreateBy)
                  rptParam(5) = New ReportParameter("in_Month", intMonth)
                  rptParam(6) = New ReportParameter("in_Year", intYear)
                  rptParam(7) = New ReportParameter("in_FromDt", strFromDate)
                  rptParam(8) = New ReportParameter("in_ToDt", strToDate)

                  rvReport.ServerReport.SetParameters(rptParam)


               Case gc_RptRegistration
                  Dim rptParam(7) As ReportParameter

                  rptParam(0) = New ReportParameter("in_Option", strOption)
                  rptParam(1) = New ReportParameter("in_OrgId", lngCustId)
                  rptParam(2) = New ReportParameter("in_OrgName", strOrgName)
                  rptParam(3) = New ReportParameter("in_CreateDt", strCreateDate)
                  rptParam(4) = New ReportParameter("in_Month", intMonth)
                  rptParam(5) = New ReportParameter("in_Year", intYear)
                  rptParam(6) = New ReportParameter("in_FromDt", strFromDate)
                  rptParam(7) = New ReportParameter("in_ToDt", strToDate)

                  rvReport.ServerReport.SetParameters(rptParam)
                  'Dim rptRegistration As New OrgRegis
                  'clsReporter.prReportName = rptRegistration
                  'Call clsReporter.prReportConnection()
                  'Call clsReporter.prReportParameter("@in_Option", strOption)
                  'Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
                  'Call clsReporter.prReportParameter("@in_OrgName", strOrgName)
                  'Call clsReporter.prReportParameter("@in_CreateDt", strCreateDate)
                  'Call clsReporter.prReportParameter("@in_Month", intMonth)
                  'Call clsReporter.prReportParameter("@in_Year", intYear)
                  'Call clsReporter.prReportParameter("@in_FromDt", strFromDate)
                  'Call clsReporter.prReportParameter("@in_ToDt", strToDate)
                  'rptRegistration = Nothing

               Case gc_RptPinGeneration
                  Dim rptParam(9) As ReportParameter

                  rptParam(0) = New ReportParameter("in_Option", strOption)
                  rptParam(1) = New ReportParameter("in_OrgId", lngCustId)
                  rptParam(2) = New ReportParameter("in_OrgName", strOrgName)
                  rptParam(3) = New ReportParameter("in_CreateDt", strCreateDate)
                  rptParam(4) = New ReportParameter("in_Month", intMonth)
                  rptParam(5) = New ReportParameter("in_Year", intYear)
                  rptParam(6) = New ReportParameter("in_FromDt", strFromDate)
                  rptParam(7) = New ReportParameter("in_ToDt", strToDate)
                  rptParam(8) = New ReportParameter("in_RequestBy", strRequestBy)
                  rptParam(9) = New ReportParameter("in_ApproveBy", strApproveBy)
                  rvReport.ServerReport.SetParameters(rptParam)
                  'Dim rptPinGen As New PinGen
                  'clsReporter.prReportName = rptPinGen
                  'Call clsReporter.prReportConnection()
                  'Call clsReporter.prReportParameter("@in_Option", strOption)
                  'Call clsReporter.prReportParameter("@in_OrgId", lngCustId)
                  'Call clsReporter.prReportParameter("@in_OrgName", strOrgName)
                  'Call clsReporter.prReportParameter("@in_CreateDt", strCreateDate)
                  'Call clsReporter.prReportParameter("@in_Month", intMonth)
                  'Call clsReporter.prReportParameter("@in_Year", intYear)
                  'Call clsReporter.prReportParameter("@in_FromDt", strFromDate)
                  'Call clsReporter.prReportParameter("@in_ToDt", strToDate)
                  'Call clsReporter.prReportParameter("@in_RequestBy", strRequestBy)
                  'Call clsReporter.prReportParameter("@in_ApproveBy", strApproveBy)
                  'rptPinGen = Nothing

            End Select
            rvReport.ServerReport.Refresh()
            'crvBankUser.BestFitPage = False                                                    'Auto Page Fit Set to False
            'crvBankUser.DisplayGroupTree = False                                               'Group Display Set to False
            'crvBankUser.DisplayToolbar = True                                                  'Toolbar Display Set to False    
            'crvBankUser.HasExportButton = True                                                 'Export Button Set To False
            'crvBankUser.HasPrintButton = True                                                  'Print Button Set to False
            'crvBankUser.PageZoomFactor = 100                                                   'View Size 150 Percent
            'crvBankUser.Height = System.Web.UI.WebControls.Unit.Pixel(400)                     'Set Height
            'crvBankUser.Width = System.Web.UI.WebControls.Unit.Pixel(700)                      'Set Width
            'crvBankUser.ReportSource = clsReporter.prReportName                                'Crystal Report Source

         Catch ex As Exception

            'Log Error
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "PG_ReportSearch - prcShowReport", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Reported Class Object
            'clsReporter = Nothing

         End Try

      End Sub

#End Region

   End Class

End Namespace

