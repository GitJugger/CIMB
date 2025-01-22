Option Strict Off
Option Explicit On 

Imports MaxFTP.clsFTP
Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers


Namespace MaxPayroll



Partial Class PG_StopPayment
      Inherits clsBasePage

#Region "Page Load"

    '****************************************************************************************************
    'Procedure Name : Page_Load()
    'Purpose        : Page Load Functions
    'Arguments      : 
    'Return Value   : 
        'Author         : Sujith Sharatchandran - 
    'Created        : 24/05/2004
    '*****************************************************************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Bank Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of User Class Object
        Dim clsUsers As New MaxPayroll.clsUsers

        Try

            'check only if bank user & interceptor
            If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_Interceptor) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then
                    'BindBody(body)
               'hide search table if interceptor
               If ss_strUserType = gc_UT_Interceptor Then
                  tblSearch.Visible = False
               End If
               'Bind Data grid
               Call prBindGrid()
               'Audit Trail
               Call clsUsers.prcDetailLog(ss_lngUserID, "View/Search Stop Payment", "Y")
            End If

        Catch ex As Exception
            LogError("PG_StopPayment - Page Load")
        Finally

            'Destroy Instance of Bank Class Object
            clsGeneric = Nothing

            'Destroy Instance of User Class Object
            clsUsers = Nothing

        End Try

    End Sub

#End Region

#Region "Data Grid Navigation"

    '****************************************************************************************************
    'Function Name  : prPageChange()
    'Purpose        : Navigation for Data Grid
    'Arguments      : System Objects,System EventArgs
    'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try
                intStart = dgStopPay.CurrentPageIndex * dgStopPay.PageSize
                dgStopPay.CurrentPageIndex = E.NewPageIndex
                Call prBindGrid()
            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Bind Data Grid"

        '****************************************************************************************************
        'Function Name  : prBindGrid()
        'Purpose        : Bind The User Data Grid
        'Arguments      : N/A
        'Return Value   : Data Grid
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub prBindGrid()

            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of System Data Set
            Dim dsStopPayment As New System.Data.DataSet

            'Variable Declarations
            Dim strErrMsg As String, strKeyword As String, intCount As Int16
            Dim lngOrgId As Long, lngUserId As Long, strOption As String, strCriteria As String

            Try

                strOption = Request.Form("ctl00$cphContent$cmbOption")                                           'Search Option
                strCriteria = Request.Form("ctl00$cphContent$cmbCriteria")                                       'Search Criteria
                strKeyword = Request.Form("ctl00$cphContent$ctl00$cphContent$txtKeyword")                                         'Search Keyword
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id

                'If search for E char - Start
                If Session("SYS_TYPE") = "BU" Then
                    'If strOption = "OrgId" Then
                    '    If strCriteria = "Exact Match" Then
                    ' If UCase(strKeyword) = gc_Const_CCPrefix Then
                    '    strKeyword = 0
                    ' Else
                    '    If InStr(UCase(strKeyword), gc_Const_CCPrefix) = 0 Then
                    '       strKeyword = 0
                    '    Else
                    '       strKeyword = Replace(UCase(strKeyword), gc_Const_CCPrefix, "")
                    '    End If
                    ' End If
                    '    ElseIf strCriteria = "Contains" Then
                    ' If UCase(strKeyword) = gc_Const_CCPrefix Then
                    '    strKeyword = Nothing
                    ' Else
                    '    strKeyword = Replace(UCase(strKeyword), gc_Const_CCPrefix, "")
                    ' End If
                    '    ElseIf strCriteria = "Starts With" Then
                    '        If IsNumeric(strKeyword) = True Then
                    '            strKeyword = 0
                    ' ElseIf UCase(strKeyword) = gc_Const_CCPrefix Then
                    '    strKeyword = Nothing
                    '        End If
                    '    End If
                    'End If
                    'if interceptor
                ElseIf Session("SYS_TYPE") = "I" Then
                    strOption = "Interceptor"
                    strKeyword = lngOrgId
                End If
                'If search for E char - Stop

                'Populate Data Set
                dsStopPayment = clsBank.fncStopPayment(strOption, strCriteria, strKeyword, lngOrgId, lngUserId)
                intCount = dsStopPayment.Tables(0).Rows.Count

                'Bind Data Grid
                If intCount > 0 Then
                    dgStopPay.Visible = True
                    dgStopPay.DataSource = dsStopPayment
                    dgStopPay.DataBind()
                    lblMessage.Text = ""
                    'if bank user hide group name
                    If Session("SYS_TYPE") = "BU" Then
                        dgStopPay.Columns(2).Visible = False
                        'if interceptor hide organisation name
                    ElseIf Session("SYS_TYPE") = "I" Then
                        dgStopPay.Columns(1).Visible = False
               End If
               fncGeneralGridTheme(dgStopPay)
                Else
                    'hide datagrid
                    dgStopPay.Visible = False
                    'if error message
                    If Not strErrMsg = "" Then
                        lblMessage.Text = strErrMsg & "<BR>" & "No Files found for stop payment."
                    Else    'if no error message
                        lblMessage.Text = "No Files found for stop payment."
                    End If
                End If


            Catch ex As Exception

            Finally

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of System Data Set
                dsStopPayment = Nothing

            End Try

        End Sub

#End Region

#Region "Show All"

      'Private Sub prcShowAll(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnShow.Click

      '    Try

      '        Server.Transfer("PG_StopPayment.aspx", False)

      '    Catch ex As Exception

      '    End Try

      'End Sub

#End Region

#Region "Search"

    Private Sub prcSearch(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSearch.Click

        Try

            'Bind Data grid
            Call prBindGrid()

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Clear"

    Private Sub prcClear(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnClear.Click

        Try

            cmbOption.SelectedValue = ""
            cmbCriteria.SelectedValue = ""
            txtKeyword.Text = ""

            prBindGrid()

        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class

End Namespace
