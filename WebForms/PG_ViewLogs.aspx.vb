Option Strict Off
Option Explicit On 

Imports MaxPayroll.clsBank

Namespace MaxPayroll
   Partial Class PG_ViewLogs
      Inherits clsBasePage

#Region "Declaration"
      Private ReadOnly Property rq_lngLogID() As Long
         Get
            If IsNumeric(Request.QueryString("ID")) Then
               Return Request.QueryString("ID")
            Else
               Return -1
            End If
         End Get
      End Property
      Private ReadOnly Property rq_strUserType() As String
         Get
            Return Request.QueryString("User") & ""
         End Get
      End Property
#End Region

#Region "Page Load"

      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         Try
                If Page.IsPostBack = False Then
                    'BindBody(body)
                End If

                Call prcBindGrid()

         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "Bind Grid"

      Private Sub prcBindGrid()

         'Create Instance of Data Set
         Dim dsLogs As New Data.DataSet

         'Create Instance of Bank Class Object
         Dim clsBank As New MaxPayroll.clsBank

         'Variable Declarations
         Dim intRecordCount As Int16

         Try

                ViewState("UserType") = rq_strUserType

            dsLogs = clsBank.fncLogs("", "Trans", "", "", "", "", "", rq_lngLogID)
            intRecordCount = dsLogs.Tables("LOGS").Rows.Count

            If intRecordCount > 0 Then
               dgLogs.Visible = True
               dgLogs.DataSource = dsLogs
               dgLogs.DataBind()
               fncGeneralGridTheme(dgLogs)
            Else
               dgLogs.Visible = False
               lblMessage.Text = "No Transactions available."
            End If

            If Page.IsPostBack = False Then
               prcBindTitle()
            End If

         Catch ex As Exception

         Finally

            'Destroy Instance of Data Set
            dsLogs = Nothing

            'Destroy Instance of Bank Class Object
            clsBank = Nothing

         End Try

      End Sub

      Private Sub prcBindTitle()
         Dim sTitle As String = " Transaction Logs"
         Select Case rq_strUserType
            Case gc_UT_BankAdmin
               lblHeading.Text = gc_UT_BankAdminDesc & sTitle
            Case gc_UT_BankUser
               lblHeading.Text = gc_UT_BankUserDesc & sTitle
            Case gc_UT_InquiryUser
               lblHeading.Text = gc_UT_InquiryUserDesc & sTitle
         End Select
      End Sub

#End Region

#Region "Page Navigation"

      Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         'Variable Declarations
         Dim intStart As Int16

         Try

            intStart = dgLogs.CurrentPageIndex * dgLogs.PageSize
            dgLogs.CurrentPageIndex = E.NewPageIndex
            Call prcBindGrid()

         Catch ex As Exception

         End Try

      End Sub

#End Region

        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

            Select Case ViewState("UserType")
                Case gc_UT_BankUser, gc_UT_InquiryUser, gc_UT_BankAdmin
                    Server.Transfer("PG_AccessLogs.aspx?User=" & ViewState("UserType") & "&Log=Acc", False)
            End Select
        End Sub
    End Class

End Namespace
