Option Strict Off
Option Explicit On 


Namespace MaxPayroll


Partial Class PG_AccessLogs
      Inherits clsBasePage

#Region "Declaration"
      Private ReadOnly Property rq_strRequest()
         Get
            Return Request.QueryString("Log") & ""
         End Get
      End Property
      Private ReadOnly Property rq_strUserType()
         Get
            Return Request.QueryString("User") & ""
         End Get
      End Property
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

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

            Try
                If Not Len(ss_strUserType) = 2 Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                If Not ss_strUserType = gc_UT_BankAdmin And Not ss_strUserType = gc_UT_BankAuth Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                If ss_strUserType = gc_UT_BankAdmin Then
                    If Not rq_strUserType = gc_UT_BankUser And Not rq_strUserType = gc_UT_InquiryUser Then
                        Server.Transfer(gc_LogoutPath, False)
                        Exit Try
                    End If
                End If
                If Not Page.IsPostBack Then
                    'BindBody(body)
                    prcBindTitle()
                    Call prcBindGrid(rq_strUserType, rq_strRequest)
                End If

            Catch ex As Exception

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

         End Try

      End Sub

      Private Sub prcBindTitle()
         Dim sTitle As String = ""
         Select Case rq_strRequest
            Case "Acc"
               sTitle = " Access Logs"
            Case "Mod"
               sTitle = " Modification Logs"
            Case "Del"
               sTitle = " Deletion Logs"
            Case "Fail"
               sTitle = " Unsuccessful Logs"
         End Select
         Select Case rq_strUserType
            Case gc_UT_BankAdmin
               lblHeading.Text = gc_UT_BankAdminDesc & sTitle
            Case gc_UT_BankUser
               lblHeading.Text = gc_UT_BankUserDesc & sTitle
            Case gc_UT_InquiryUser
               lblHeading.Text = gc_UT_InquiryUserDesc & sTitle
            Case gc_UT_BankAuth
               lblHeading.Text = gc_UT_BankAuthDesc & sTitle
         End Select
      End Sub

#End Region

#Region "Page Navigation"

      Public Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         'Variable Declarations
         Dim intStart As Int16

         Try
            intStart = dgAccessLog.CurrentPageIndex * dgAccessLog.PageSize
            dgAccessLog.CurrentPageIndex = E.NewPageIndex

            'Populate Data Grid
            Call prcBindGrid(rq_strUserType, rq_strRequest)

         Catch ex As Exception
            Me.LogError("pg_AccessLogs - prPageChange")
         End Try

      End Sub

      Sub prPageChange1(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         'Variable Declarations
         Dim intStart As Int16

         Try

            intStart = dgModifyLog.CurrentPageIndex * dgModifyLog.PageSize
            dgModifyLog.CurrentPageIndex = E.NewPageIndex

            'Populate Datagrid
            Call prcBindGrid(rq_strUserType, rq_strRequest)

         Catch ex As Exception
            Me.LogError("pg_AccessLogs - prPageChange1")
         End Try

      End Sub

      Sub prPageChange2(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         'Variable Declarations
         Dim intStart As Int16

         Try

            intStart = dgDeleteLog.CurrentPageIndex * dgDeleteLog.PageSize
            dgDeleteLog.CurrentPageIndex = E.NewPageIndex

            'Populate Datagrid
            Call prcBindGrid(rq_strUserType, rq_strRequest)

         Catch ex As Exception
            Me.LogError("pg_AccessLogs - prPageChange2")
         End Try

      End Sub

      Sub prPageChange3(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         'Variable Declarations
         Dim intStart As Int16

         Try

            intStart = dgFailLog.CurrentPageIndex * dgFailLog.PageSize
            dgFailLog.CurrentPageIndex = E.NewPageIndex

            'Populate Datagrid
            Call prcBindGrid(rq_strUserType, rq_strRequest)

         Catch ex As Exception
            Me.LogError("pg_AccessLogs - prPageChange3")
         End Try

      End Sub

#End Region

#Region "Bind Grid"

      '****************************************************************************************************
      'Procedure Name : prcBindGrid()
      'Purpose        : Populate Data Grid
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 14/02/2005
      '*****************************************************************************************************
        Private Sub prcBindGrid(ByVal strUser As String, ByVal strRequest As String)

            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of DataSet
            Dim dsLogs As New Data.DataSet

            'Variable Declarations 
            Dim strFromDt As String, strToDt As String, intRecordCount As Int16
            Dim strOption As String = "", strUserLogin As String, strUserName As String

            Try

                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtUserName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserId1 As Boolean = clsCommon.CheckScriptValidation(txtUserId.Text)
                If txtUserId1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                'Get Values - START
                strToDt = txtToDt.Text
                strFromDt = txtFromDt.Text
                strUserLogin = txtUserId.Text
                strUserName = txtUserName.Text
                lblMessage.Text = ""
                'Get Values - STOP

                'Select Option - START
                If rdUserId.Checked Then
                    strOption = "User Id"
                ElseIf rdUserName.Checked Then
                    strOption = "User Name"
                ElseIf rdFrom.Checked Then
                    strOption = "From To"
                ElseIf rdAll.Checked Then
                    strOption = ""
                End If
                'Select Option - STOP

                If rdFrom.Checked Then
                    If txtFromDt.Text = "" Or txtToDt.Text = "" Then
                        lblMessage.Text = "Please enter the From and To Date."
                        dgAccessLog.Visible = False
                        dgModifyLog.Visible = False
                        dgDeleteLog.Visible = False
                        dgFailLog.Visible = False
                        Exit Sub
                    End If
                End If

                'Populate Data Grid - START
                dsLogs = clsBank.fncLogs(strUser, strRequest, strOption, strUserLogin, strUserName, strFromDt, strToDt, 0)
                intRecordCount = dsLogs.Tables("LOGS").Rows.Count

                If intRecordCount > 0 Then
                    If strRequest = "Acc" Then
                        dgModifyLog.Visible = False
                        dgDeleteLog.Visible = False
                        dgAccessLog.Visible = True
                        dgAccessLog.DataSource = dsLogs
                        dgAccessLog.DataBind()
                        If strUser = gc_UT_BankAuth OrElse strUser = gc_UT_BankAdmin Then
                            dgAccessLog.Columns(4).Visible = False
                        End If
                    ElseIf strRequest = "Mod" Then
                        dgAccessLog.Visible = False
                        dgDeleteLog.Visible = False
                        dgModifyLog.Visible = True
                        dgModifyLog.DataSource = dsLogs
                        dgModifyLog.DataBind()
                    ElseIf strRequest = "Del" Then
                        dgAccessLog.Visible = False
                        dgModifyLog.Visible = False
                        dgDeleteLog.Visible = True
                        dgDeleteLog.DataSource = dsLogs
                        dgDeleteLog.DataBind()
                    ElseIf strRequest = "Fail" Then
                        dgModifyLog.Visible = False
                        dgDeleteLog.Visible = False
                        dgAccessLog.Visible = False
                        dgFailLog.Visible = True
                        dgFailLog.DataSource = dsLogs
                        dgFailLog.DataBind()
                    End If
                Else
                    dgAccessLog.Visible = False
                    dgModifyLog.Visible = False
                    dgDeleteLog.Visible = False
                    dgFailLog.Visible = False
                    lblMessage.Text = "No Records Found"
                End If
                'Populate Data Grid - STOP

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Search Button"

    Private Sub prcSearch(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSearch.Click

        Try

            'Populate Datagrid
            Call prcBindGrid(rq_strUserType, rq_strRequest)

         Catch ex As Exception
            Me.LogError("pg_AccessLogs - prcSearch")
         End Try

    End Sub

#End Region

      Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
         Me.txtFromDt.Text = ""
         Me.txtToDt.Text = ""
         Me.txtUserId.Text = ""
         Me.txtUserName.Text = ""
         Me.rdAll.Checked = True
         Me.rdFrom.Checked = False
         Me.rdUserId.Checked = False
         Me.rdUserName.Checked = False
      End Sub

     
   End Class

End Namespace
