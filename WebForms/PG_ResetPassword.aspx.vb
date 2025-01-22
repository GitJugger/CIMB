Namespace MaxPayroll

   Partial Class PG_ResetPassword
      Inherits clsBasePage
#Region "Declaration"
        Dim clsEncryption As New MaxPayroll.Encryption
        Private ReadOnly Property rq_strID() As String
            Get
                Return clsEncryption.Cryptography(Request.QueryString("ID")) & ""
            End Get
        End Property
#End Region
#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic
            Dim dsDetails As New System.Data.DataSet
            'Variable Declarations
            Dim strPassLock As String = "N", strAuthLock As String = "N"

            Try

            If Not ss_strUserType = gc_UT_BankUser Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

                'Get user id 
                'hUserId.Value = Request.QueryString("ID")
                hUserId.Value = rq_strID

                If Request.QueryString("Mode") = "AUTH" Then
               lblHeading.Text = "Reset Validation Code"
            ElseIf Request.QueryString("Mode") = "PASS" Then
               lblHeading.Text = "Reset Password"
            End If

            If Not Page.IsPostBack Then
                    'BindBody(body)
                    'Populate Data Set
                    dsDetails = clsGeneric.fnLoadUserDetails(rq_strID)

                    If dsDetails.Tables("USERDETAILS").Rows.Count > 0 Then
                        For Each drMail In dsDetails.Tables("USERDETAILS").Rows
                            lblTxtOrgId.Text = drMail("UOrgId")
                            strPassLock = drMail("ULock")
                            strAuthLock = drMail("UALock")
                            lblTxtOrgName.Text = Request.QueryString("OName")
                            lblTxtUserLogin.Text = drMail("ULogin")
                            hUserStatus.Value = drMail("UStatus")
                        Next
                    End If
                    '     lblTxtOrgId.Text = Request.QueryString("OrgId")
                    'strPassLock = Trim(Request.QueryString("PLOCK"))
                    'strAuthLock = Trim(Request.QueryString("ALOCK"))
                    'lblTxtOrgName.Text = Request.QueryString("OName")
                    'lblTxtUserLogin.Text = Request.QueryString("ULogin")
                    'hUserStatus.Value = Left(Request.QueryString("UStatus"), 1)

                    If Trim(Request.QueryString("Mode")) = "PASS" Then
                  If strPassLock = "N" Then
                     btnGenerate.Enabled = False
                     lblMessage.Text = "You can Reset Password only if User Password is Locked out."
                  End If
               ElseIf Trim(Request.QueryString("Mode")) = "AUTH" Then
                  If strAuthLock = "N" Then
                     btnGenerate.Enabled = False
                     lblMessage.Text = "You can Reset Validation Code only if User Validation Code is Locked out."
                  End If
               End If

            End If

         Catch ex As Exception
            LogError("PG_ResetPassword - Page Load")
         Finally

            'Destroy Generic
            clsGeneric = Nothing

         End Try

      End Sub

#End Region

#Region "Generate New Password"

      Public Sub btnGenerate_Click(ByVal O As System.Object, ByVal E As EventArgs) Handles btnGenerate.Click

         'Create Instance of User Class Object
         Dim clsUser As New MaxPayroll.clsUsers

         'Create Instance of Encryption Class Object
         Dim clsEncryption As New MaxPayroll.Encryption

            'Variable declaration
            Dim strPassword As String, lngUserId As Long, IsGenerate As Boolean

            Dim clsGeneric As New MaxPayroll.Generic
            Dim dsDetails As New System.Data.DataSet

            Dim strPassLock As String = "N", strAuthLock As String = "N"

            Try

                'Populate Data Set
                dsDetails = clsGeneric.fnLoadUserDetails(rq_strID)

                If dsDetails.Tables("USERDETAILS").Rows.Count > 0 Then
                    For Each drMail In dsDetails.Tables("USERDETAILS").Rows
                        strPassLock = drMail("ULock")
                        strAuthLock = drMail("UALock")
                    Next
                End If

                If Trim(Request.QueryString("Mode")) = "PASS" Then
                    If strPassLock = "N" Then
                        btnGenerate.Enabled = False
                        lblMessage.Text = "You can Reset Password only if User Password is Locked out."
                        Exit Try
                    End If
                ElseIf Trim(Request.QueryString("Mode")) = "AUTH" Then
                    If strAuthLock = "N" Then
                        btnGenerate.Enabled = False
                        lblMessage.Text = "You can Reset Validation Code only if User Validation Code is Locked out."
                        Exit Try
                    End If
                End If
                'Generate New Password
                strPassword = clsEncryption.fncPassword(8)

            'Get User Id
            lngUserId = IIf(IsNumeric(hUserId.Value), hUserId.Value, 0)

            'Update CA password in database
            IsGenerate = clsUser.fnResetPassword(strPassword, Trim(Request.QueryString("MODE")), lngUserId)

            'If Password Generation Successful
            If IsGenerate Then
               btnGenerate.Enabled = False
               If Trim(Request.QueryString("MODE")) = "PASS" Then
                  lblMessage.Text = "Password Generation Successful."
               ElseIf Trim(Request.QueryString("MODE")) = "AUTH" Then
                  lblMessage.Text = "Validation Code Generation Successful."
               End If
               'If Password Generation Failed
            Else
               btnGenerate.Enabled = True
               lblMessage.Text = "Password Generation Failed. Please Try Again"
            End If

         Catch ex As Exception
            LogError("PG_ResetPassword - btnGenerate")
         Finally

            'Destroy Instance of User Class Object
            clsUser = Nothing

            'Destroy Instance of Encryption  Class Object
            clsEncryption = Nothing

         End Try

      End Sub

#End Region

#Region "Page Back"

      Public Sub btnBack_Click(ByVal O As System.Object, ByVal E As EventArgs) Handles btnBack.Click

         Try
            Server.Transfer("PG_ViewPassword.aspx", False)
         Catch ex As Exception
            LogError("PG_ResetPassword - btnBack")
         End Try

      End Sub

#End Region

   End Class

End Namespace
