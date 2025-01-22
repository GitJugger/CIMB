Imports MaxPayroll.Generic


Namespace MaxPayroll

Partial Class PG_QueryReport
      Inherits clsBasePage

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Variable Declarations
         Dim intYear As Int16, intIndex As Int16, intCounter As Int16, strOption As String

        Try

            If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser) Then
               Server.Transfer(gc_logoutpath, False)
               Exit Try
            End If

            strOption = Request.QueryString("Option")

            If strOption = "Trans" Then
               intIndex = 0
               intYear = Year(Today)
               trTrans.Visible = True
               trDorm.Visible = False
               trExpiry.Visible = False
               tblExpiry.Visible = False
                    lblHead.Text = "Monthly Transaction / Subscription Fee Report"
               If Not Page.IsPostBack Then
                  For intYear = intYear To intYear + 10
                     cmbYear.Items.Insert(intIndex, intYear)
                     intIndex = intIndex + 1
                  Next
               End If
            ElseIf strOption = "Dorm" Then
               trTrans.Visible = False
               trDorm.Visible = True
               tblExpiry.Visible = False
               trExpiry.Visible = False
               lblHead.Text = "Dormant Account Report"
            ElseIf strOption = "FileSub" Then
               trDorm.Visible = False
               trTrans.Visible = False
               tblFileSub.Visible = True
               tblExpiry.Visible = False
               trExpiry.Visible = False
               lblHead.Text = "File Status Report - Query"
            ElseIf strOption = "Expiry" Then
               trDorm.Visible = False
               trTrans.Visible = False
               tblExpiry.Visible = True
               trExpiry.Visible = True
               lblHead.Text = "User Expiry Report"
               If Not Page.IsPostBack Then
                  intYear = DatePart(DateInterval.Year, Now.Date)
                  ddlExpiryYear.Items.Insert(0, "")
                  For intCounter = 1 To 20
                     ddlExpiryYear.Items.Insert(intCounter, intYear)
                     intYear = intYear + 1
                  Next
               End If
            End If

         Catch ex As Exception

         Finally

            'Destroy Instance of Generic Class object
            clsGeneric = Nothing

         End Try

    End Sub

#End Region

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

            lblHead.Text = "Dormant Account Report"
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
                strURL = "PG_ViewReports.aspx?Report=FileSub&Option=" & strOption & "&FromDt=" & strFromDt & "&ToDt=" & strToDt & "&Status=" & intStatus & "&Sort=" & strSort & "&File=" & strFileType

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

            lblHead.Text = "User Expiry Report"
            intMonth = IIf(IsNumeric(ddlExpiryMonth.SelectedValue), ddlExpiryMonth.SelectedValue, 0)
            intYear = IIf(IsNumeric(ddlExpiryYear.SelectedValue), ddlExpiryYear.SelectedValue, 0)
            strURL = "PG_ViewReports.aspx?Report=Expiry&Month=" & intMonth & "&Year=" & intYear
            Server.Transfer(strURL, False)
            Exit Try

        Catch ex As Exception

        End Try

    End Sub

#End Region
End Class

End Namespace

