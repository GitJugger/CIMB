Imports MaxPayroll.Generic


Namespace MaxPayroll

   Partial Class pg_SearchReportServicesByGlobalCriteria2
      Inherits clsBasePage
#Region "Request.QueryString"
      Private ReadOnly Property rq_strOption() As String
         Get
            Return Trim(Request.QueryString("Option") & "")
         End Get
      End Property
      Private ReadOnly Property rq_strReportName() As String
         Get
            Return Trim(Request.QueryString("ReportName") & "")
         End Get
      End Property
#End Region

#Region "Page Load"

      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Variable Declarations
         Dim intYear As Int16, intIndex As Int16, intCounter As Int16

         Try
            If Page.IsPostBack = False Then
                    'BindBody(body)
               ddlFile.Items.Clear()
               prcBindPaymentService()
            End If
            If Not (ss_strUserType.Equals(gc_UT_BankUser) OrElse ss_strUserType.Equals(gc_UT_InquiryUser)) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            Select Case rq_strReportName
               Case gc_RptTransaction
                  intIndex = 0
                  intYear = Year(Today)
                  trTrans.Visible = True
                  trDorm.Visible = False
                  trExpiry.Visible = False
                  tblExpiry.Visible = False
                        lblHeading.Text = "Monthly Transaction / Subscription Fee Report"
                  If Not Page.IsPostBack Then
                     For intYear = intYear To intYear + 10
                        cmbYear.Items.Insert(intIndex, intYear)
                        intIndex = intIndex + 1
                     Next
                  End If
               Case gc_RptDormant
                  trTrans.Visible = False
                  trDorm.Visible = True
                  tblExpiry.Visible = False
                  trExpiry.Visible = False
                  lblHeading.Text = "Dormant Account Report"
               Case gc_RptFileSubmission
                  trDorm.Visible = False
                  trTrans.Visible = False
                  tblFileSub.Visible = True
                  tblExpiry.Visible = False
                  trExpiry.Visible = False
                  lblHeading.Text = "File Status Report - Query"
               Case gc_RptUserExpiry
                  trDorm.Visible = False
                  trTrans.Visible = False
                  tblExpiry.Visible = True
                  trExpiry.Visible = True
                  lblHeading.Text = "User Expiry Report"
                  If Not Page.IsPostBack Then
                     intYear = DatePart(DateInterval.Year, Now.Date)
                     ddlExpiryYear.Items.Insert(0, "")
                     For intCounter = 1 To 20
                        ddlExpiryYear.Items.Insert(intCounter, intYear)
                        intYear = intYear + 1
                     Next
                  End If
            End Select
         Catch ex As Exception

         Finally

            'Destroy Instance of Generic Class object
            clsGeneric = Nothing

         End Try

      End Sub

#End Region
      Private Sub prcBindPaymentService()
         Dim clsPaymentService As New clsPaymentService
         Me.ddlFile.DataSource = clsPaymentService.fncRetrievePaymentService
         Me.ddlFile.DataTextField = "PaySer_Desc"
         ddlFile.DataValueField = "PaySer_Desc"
         ddlFile.DataBind()
      End Sub
#Region "Annual/Trans"

      Private Sub AnnualTrans(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnTrans.Click

         'Variable Declarations
         Dim intMonth As Int16, intYear As Int16, strURL As String

         Try

            intYear = IIf(IsNumeric(cmbYear.SelectedValue), cmbYear.SelectedValue, 0)
            intMonth = IIf(IsNumeric(cmbMonth.SelectedValue), cmbMonth.SelectedValue, 0)
            strURL = "PG_ViewReports.aspx?Report=Monthly&Month=" & intMonth & "&Year=" & intYear
            Server.Transfer(strURL, False)
            'Response.Write("<script language='JavaScript'>")
            'Response.Write("window.location.href = 'PG_ViewReports.aspx?Report=Monthly&Month=" & intMonth & "&Year=" & intYear & "';")
            'Response.Write("</script>")
            Exit Try
         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "Dormant"

      Private Sub Dormant(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnDorm.Click

         'Variable Declarations
         Dim intPeriod As Int16, strURL As String

         Try

            lblHeading.Text = "Dormant Account Report"
            intPeriod = IIf(IsNumeric(cmbPeriod.SelectedValue), cmbPeriod.SelectedValue, 0)
            strURL = "PG_ViewReports.aspx?Report=Dorm&Period=" & intPeriod
            Server.Transfer(strURL, False)
            Exit Try

         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "File Submission"

      Private Sub prcFileSub(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

         'Variable Decalarations
         Dim strOption As String, strFromDt As String, strToDt As String
         Dim strURL As String = "", strSort As String, intStatus As Int16, strFileType As String

         Try

            strToDt = txtToDt.Text                      'Get To Date
            strFromDt = txtFromDt.Text                  'Get From Date
            strOption = ddlOption.SelectedValue         'Get Option
            strSort = ddlSort.SelectedValue             'Get Sort Field
            intStatus = ddlStatus.SelectedValue         'Get Status Code
            strFileType = ddlFile.SelectedValue         'Get File Type

            'Build URL
            strURL = "PG_ViewReportServices.aspx?Report=" & rq_strReportName & "&Option=" & strOption & "&FromDt=" & strFromDt & "&ToDt=" & strToDt & "&Status=" & intStatus & "&Sort=" & strSort & "&File=" & strFileType

         Catch ex As Exception

         End Try

         Server.Transfer(strURL, False)

      End Sub

#End Region

#Region "Expiry"

      Private Sub Expiry(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnExpiry.Click

         'Variable Declarations
         Dim intMonth, intYear As Int16, strURL As String

         Try

            lblHeading.Text = "User Expiry Report"
            intMonth = IIf(IsNumeric(ddlExpiryMonth.SelectedValue), ddlExpiryMonth.SelectedValue, 0)
            intYear = IIf(IsNumeric(ddlExpiryYear.SelectedValue), ddlExpiryYear.SelectedValue, 0)
            strURL = "PG_ViewReports.aspx?Report=Expiry&Month=" & intMonth & "&Year=" & intYear
            Server.Transfer(strURL, False)
            Exit Try

         Catch ex As Exception

         End Try

      End Sub

#End Region

        Protected Sub btnSearchReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchReport.Click
            Dim strURL As String = ("PG_ViewReportServices.aspx?ReportName=") & rq_strReportName
           
            Select Case rq_strReportName
                Case gc_RptTransaction

                    Server.Transfer(strURL & "&Month=" & IIf(IsNumeric(cmbMonth.SelectedValue), cmbMonth.SelectedValue, 0) & "&Year=" & IIf(IsNumeric(cmbYear.SelectedValue), cmbYear.SelectedValue, 0), False)

                Case gc_RptDormant

                    Server.Transfer(strURL & "&Period=" & IIf(IsNumeric(cmbPeriod.SelectedValue), cmbPeriod.SelectedValue, 0), False)

                Case gc_RptFileSubmission

                    Server.Transfer(strURL & "&Option=" & ddlOption.SelectedValue & "&FromDt=" & txtFromDt.Text & "&ToDt=" & txtToDt.Text & " &Status=" & ddlStatus.SelectedValue & "&Sort=" & ddlSort.SelectedValue & "&File=" & ddlFile.SelectedValue, False)

                Case gc_RptUserExpiry

                    Server.Transfer(strURL & "&Month=" & IIf(IsNumeric(ddlExpiryMonth.SelectedValue), ddlExpiryMonth.SelectedValue, 0) & "&Year=" & IIf(IsNumeric(ddlExpiryYear.SelectedValue), ddlExpiryYear.SelectedValue, 0), False)

            End Select
        End Sub
   End Class

End Namespace

