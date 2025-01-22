Imports MaxPayroll.Generic
Imports MaxPayroll.clsCommon

Namespace MaxPayroll

    Partial Class PG_Inbox
        Inherits clsBasePage


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

         'Create Instance of Generic Class
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of Common Class Object
         Dim clsCommon As New MaxPayroll.clsCommon

         'Variable Declarations
         Dim lngOrgId As Long, strSignOn As String
            Dim lngUserId As Long, dtUserExpiryDt As Date, dtReminderDt As Date, dtCurrDt As Date

            btnDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete the message?');")


            Try

                If Page.IsPostBack = False Then
                    'BindBody(body)
                End If

                'Warning message confirm - Start
                'If Session("SYS_WARN") = "N" Then
                '   Dim strScript As String, strAlert As String

                '   strAlert = "Only authorized users are allowed to access to " & gc_Const_ApplicationName & " System. If you are not an authorized user, please sign"
                '   strAlert += " out immediately. Unauthorized user is prohibited and illegal access will be"
                '   strAlert += " prosecuted to the full extent of the law."

                '   strScript = "<script type='text/javascript'>"
                '   strScript += ""
                '   strScript += "if(!confirm('" & strAlert & "'))"
                '   strScript += "{ location.href = 'PG_Logout.aspx'; }"
                '   strScript += "</script>"

                '   'register script
                '   RegisterStartupScript("Confirm", strScript)

                '   Session("SYS_WARN") = "Y"
                'End If
                'Warning message confirm - Stop

                If Len(Session("SYS_TYPE")) = 1 Then
                    lblHeading.Text = "MAIL BOX (" & Session("SYS_GNAME") & ")"
                Else
                    lblHeading.Text = "MAIL BOX"
                End If

                'Get User Login Id
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Get Last SignOn
                strSignOn = clsCommon.fncBuildContent("Sign On", "", lngUserId, lngUserId)
                If Not strSignOn = "" Then
                    lblSignOn.Text = strSignOn
                End If

                'Check If needs to Force User to Change Password
                If Session("PWD_CHNG") = "Y" Then
                    Session("NoMenu") = "True"
                    Server.Transfer("PG_ChangePassword.aspx?SignOut=1", False)
                    Exit Try
                    'Check If Needs to force User to Change Auth Code
                ElseIf Session("AUTH_CHNG") = "Y" Then
                    If Not (Session("SYS_TYPE") = "IU" Or Session("SYS_TYPE") = "BO" Or (Session(gc_Ses_Token) = True AndAlso CStr(Session("SYS_TYPE")).ToUpper.Equals("A"))) Then
                        Session("NoMenu") = "True"
                        Server.Transfer("PG_ChangeAuthCode.aspx?CHNG=Y&SignOut=1", False)
                        Exit Try
                    End If
                End If

                'Check If Password Needs to be Changed, If Yes
                If Session("PWD_EXP") = "Y" Then
                    Session("NoMenu") = "True"
                    Server.Transfer("PG_ChangePassword.aspx", False)
                    Exit Try

                Else

                    'Load the Menu
                    Dim strScript As String
                    'strScript = "<script language='javascript'>" & System.Environment.NewLine & "parent.frames['Menu'].location.href = 'PG_Menu2.aspx';parent.frames['banner'].location.href = 'pg_Banner.aspx';" & System.Environment.NewLine & "</script>"
                    '     RegisterStartupScript("Confirm1", strScript)
                    If Not Page.IsPostBack Then

                        Call prBindGrid()

                        'Reminder for User Expiry - Start
                        If Session("EXP_MSG") = "Y" Then
                            dtUserExpiryDt = CDate(Session("EXP_DT"))
                            dtCurrDt = Today
                            dtReminderDt = dtUserExpiryDt.AddDays(-7)

                            'Display Reminder
                            If dtReminderDt <= dtCurrDt Then
                                strScript = "<script type='text/javascript'>" & "alert('Your Account will expire after " & Format(dtUserExpiryDt, "dd/MM/yyyy") & "');" & "</script>"
                                RegisterStartupScript("Confirm2", strScript)
                            End If

                            'Reset Session Value
                            Session("EXP_MSG") = "N"

                        End If
                        'Reminder for User Expiry - Stop
                    End If

                End If

            Catch ex As Exception

                'Log Error
                If Err.Description <> "Thread was being aborted." Then
               Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "PG_Inbox.aspx.vb - Page Load", Err.Number, Err.Description)
            End If

         Finally

            'Destroy Instance of Generic class Object
            clsGeneric = Nothing

            'Destroy Instance of Common Class Object
            clsCommon = Nothing

         End Try
        End Sub

#End Region

#Region "Page_Change"

        '****************************************************************************************************
        'Procedure Name : Page_Change()
        'Purpose        : Page Navigation
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
      Sub Page_Change(ByVal Sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgUser.PageIndexChanged

         'Variable Declarations
         Dim intStart As Integer

         Try
            intStart = dgUser.CurrentPageIndex * dgUser.PageSize

            dgUser.CurrentPageIndex = e.NewPageIndex
            Call prBindGrid(fncSort())

         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "Display Related Methods"

        Function ChangeImage(ByVal strRead As String) As String

            If UCase(strRead) = "READ" Then
                Return "<img src='../Include/Images/Mail_Open.gif'>"
            Else
                Return "<img src='../Include/Images/Mail_Close.gif'>"
            End If

        End Function

        '****************************************************************************************************
        'Procedure Name : prCheckAll()
        'Purpose        : To Select The List of Displayed Mails
        'Arguments      : Object,Event Arguments
        'Return Value   : 
        'Author         : Sujith Sharatchandran - 
        'Created        : 09/10/2003
        '*****************************************************************************************************
        Sub prCheck(ByVal Source As Object, ByVal E As EventArgs) Handles btnCheckAll.Click

            Dim GridItem As DataGridItem

            Try

                For Each GridItem In dgUser.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.Cells(0).Controls(0), CheckBox)
                    myCheckbox.Checked = True
                Next

            Catch ex As Exception

            End Try

        End Sub
        '****************************************************************************************************
        'Procedure Name : prUncheck()
        'Purpose        : To Unselect The List of Displayed Mails
        'Arguments      : Object,Event Arguments
        'Return Value   : 
        'Author         : Sujith Sharatchandran - 
        'Created        : 09/10/2003
        '*****************************************************************************************************
        Sub prUncheck(ByVal Source As Object, ByVal E As EventArgs) Handles btnUnCheck.Click

            Dim GridItem As DataGridItem

            Try

                For Each GridItem In dgUser.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.Cells(0).Controls(0), CheckBox)
                    myCheckbox.Checked = False
                Next

            Catch ex As Exception

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : prDelete()
        'Purpose        : To Delete The List of Displayed Mails
        'Arguments      : Object,Event Arguments
        'Return Value   : 
        'Author         : Sujith Sharatchandran - 
        'Created        : 09/10/2003
        '*****************************************************************************************************
        Sub prDelete(ByVal o As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click


            Dim GridItem As DataGridItem
            Dim clsMail As New MaxPayroll.Generic
            'Create Instance of System Data Row
            Dim drMail As System.Data.DataRow

            'Create Instance of System Data Set
            Dim dsMail As New System.Data.DataSet
            Dim lngMailId As Long

            Try
                Dim stat = 0
                For Each GridItem In dgUser.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.Cells(0).Controls(0), CheckBox)
                    If myCheckbox.Checked Then
                        Dim hTextBox As HtmlInputHidden = CType(GridItem.Cells(0).FindControl("hID"), HtmlInputHidden)
                        lngMailId = IIf(IsNumeric(hTextBox.Value), hTextBox.Value, 0)
                        dsMail = clsMail.fnLoadMail(lngMailId, ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                        If dsMail.Tables("MAIL").Rows.Count > 0 Then

                            For Each drMail In dsMail.Tables("MAIL").Rows
                                'If ss_lngOrgID <> drMail("UOrgId") Then
                                '    stat = 1
                                '    Exit For
                                'End If
                                'If ss_lngGroupID <> drMail("UGroupId") Then
                                '    stat = 1
                                '    Exit For
                                'End If
                                'If ss_lngUserID <> drMail("UMaiLTo") Then
                                '    stat = 1
                                '    Exit For
                                'End If
                            Next
                        Else
                            stat = 1
                        End If
                        If stat = 1 Then
                            Throw New System.Exception("InvalidId")
                        End If
                        clsMail.DeleteMail("MAIL", lngMailId, ss_lngOrgID)
                    End If
                Next
                prBindGrid()
                'Server.Transfer("PG_Inbox.aspx", False)

            Catch ex As Exception
                If ex.Message = "InvalidId" Then
                    HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                Else
                    LogError("PG_Inbox - Page Load")
                End If
            Finally

                clsMail = Nothing

            End Try

        End Sub

#End Region

#Region "Data Grid"
        '****************************************************************************************************
        'Procedure Name : prBindGrid()
        'Purpose        : Populate Data Set
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Private Sub prBindGrid(Optional ByVal sSort As String = "")

         'Create Instance of Common Class Object
         Dim clsCommon As New MaxPayroll.clsCommon

         'Create Instance of System Data Set
         Dim dsMailBox As New System.Data.DataSet

         'Variable Declarations
         Dim lngOrgId As Long, lngGroupId As Long, lngUserId As Long, intDays As Int16

         Try

            intDays = txtDays.Text                                                          'No of Days

                If intDays > 60 Then
                    Response.Write(clsCommon.ErrorCodeScript2())
                    Exit Try
                End If
                'Populate Date Set
                If intDays < 61 Then
                    dsMailBox = clsCommon.fncMailList(ss_lngOrgID, ss_lngUserID, ss_lngGroupID, intDays)
                End If

                Dim dv As New DataView
            If Len(sSort.Trim) > 0 Then
               dv = dsMailBox.Tables(0).DefaultView
               dv.Sort = sSort
            End If

            If dsMailBox.Tables(0).Rows.Count > 0 Then
               lblMessage.Text = ""
               dgUser.Visible = True
               btnDelete.Visible = True
               btnUnCheck.Visible = True
               btnCheckAll.Visible = True
               If Len(sSort.Trim) > 0 Then
                  dgUser.DataSource = dv
               Else
                  dgUser.DataSource = dsMailBox
               End If
               dgUser.DataBind()
               fncGeneralGridTheme(dgUser)
               pnlGrid.Visible = True

            Else
               Me.pnlGrid.Visible = False
               lblMessage.Text = "No Messages Available"
               dgUser.Visible = False
               btnCheckAll.Visible = False
               btnUnCheck.Visible = False
               btnDelete.Visible = False
            End If

         Catch ex As Exception

         Finally

            'Destroy Instance of Common Class Object
            clsCommon = Nothing

            'Destroy Instance of System Data Set
            dsMailBox = Nothing

         End Try


      End Sub

#End Region

#Region "Day Wise"

      Private Sub prcDays(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnShow.Click
         'System.Threading.Thread.Sleep(3000)
         Try
            Call prBindGrid()

         Catch ex As Exception

         End Try

      End Sub

#End Region

      Protected Sub dgUser_SortCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridSortCommandEventArgs) Handles dgUser.SortCommand
         'If ViewState("sort") = Nothing Then
         '   ViewState("sort") = e.SortExpression
         '   ViewState("sortstate") = " asc"
         '   prBindGrid(e.SortExpression)
         'ElseIf ViewState("sort") = e.SortExpression Then
         '   If ViewState("sortstate") = Nothing Then
         '      ViewState("sortstate") = " asc"
         '   Else
         '      If ViewState("sortstate") = " asc" Then
         '         ViewState("sortstate") = " desc"
         '      Else
         '         ViewState("sortstate") = " asc"
         '      End If
         '   End If
         '   prBindGrid(e.SortExpression & ViewState("sortstate"))
         'ElseIf ViewState("sort") <> e.SortExpression Then
         '   ViewState("sort") = e.SortExpression
         '   ViewState("sortstate") = " asc"
         '   prBindGrid(e.SortExpression)
         'End If
         prBindGrid(fncSort(e.SortExpression))
      End Sub
        Private Function fncSort(Optional ByVal sSort As String = "") As String
            Dim sRetVal As String = ""
            If sSort = "" Then
                If ViewState("sort") <> "" Then
                    sRetVal = ViewState("sort") & ViewState("sortstate")
                End If

            ElseIf ViewState("sort") = sSort OrElse ViewState("sort") = "" Then
                If ViewState("sortstate") = "" Or ViewState("sortstate") = Nothing Then
                    ViewState("sortstate") = " asc"
                Else
                    If ViewState("sortstate") = " asc" Then
                        ViewState("sortstate") = " desc"
                    Else
                        ViewState("sortstate") = " asc"
                    End If
                End If
                ViewState("sort") = sSort
                sRetVal = ViewState("sort") & ViewState("sortstate")
            Else
                ViewState("sort") = sSort
                ViewState("sortstate") = " asc"
                sRetVal = ViewState("sort") & ViewState("sortstate")
            End If
            Return sRetVal
        End Function

        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Dim s As String = clsEncryption.fnSQLCrypt(Id)
            Return s

        End Function

    End Class

End Namespace
