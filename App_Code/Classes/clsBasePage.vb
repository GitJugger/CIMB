Imports Microsoft.VisualBasic
Imports MaxPayroll.mdConstant
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll

   Public Class clsBasePage
      Inherits clsBaseGeneric

#Region "Sorting"
      'Public Function fncSort() As String
      '   If ViewState("sort") = Nothing Then
      '      ViewState("sort") = e.SortExpression
      '      ViewState("sortstate") = " asc"
      '   ElseIf ViewState("sort") = e.SortExpression Then
      '      If ViewState("sortstate") = Nothing Then
      '         ViewState("sortstate") = " asc"
      '      Else
      '         If ViewState("sortstate") = " asc" Then
      '            ViewState("sortstate") = " desc"
      '         Else
      '            ViewState("sortstate") = " asc"
      '         End If
      '      End If
      '   ElseIf ViewState("sort") <> e.SortExpression Then
      '      ViewState("sort") = e.SortExpression
      '      ViewState("sortstate") = " asc"
      '   End If
      'End Function

#End Region

#Region "   Token"
      Public Function fncRequestChallengeCode(ByRef sRetVal As String) As Boolean
         Dim oToken As New dyna2.client
         Dim bRetVal As Boolean
         Try
            With oToken
               .Host = fncAppSettings("TokenHost")
               .Port = fncAppSettings("TokenPort")
               .Type = "Request"
               .CustomerID = ss_lngUserID
               sRetVal = oToken.Process
               If IsNothing(sRetVal) Then
                  Dim sTemp() As String = oToken.ChallengeCode.Split
                  bRetVal = False
                  If sTemp(0).Length > 1 Then
                     Select Case sTemp(0)
                        Case "ID_Blocked"
                           sRetVal = "Token for [" & Session(gc_Ses_UserLoginName) & "] has been Blocked."
                        Case "ID_Cancel"
                           sRetVal = "Token for [" & Session(gc_Ses_UserLoginName) & "] has been Cancelled."
                        Case "ID_Inactive"
                           sRetVal = "Token for [" & Session(gc_Ses_UserLoginName) & "] is Inactive."
                        Case "Invalid_ID"
                           sRetVal = "Token for [" & Session(gc_Ses_UserLoginName) & "] has either been Cancelled or Invalid."
                        Case "Not_Activate"
                           sRetVal = "Token for [" & Session(gc_Ses_UserLoginName) & "] is not yet activated."
                        Case "001"
                           sRetVal = "Token Authentication Error 001 - Update SQL Error.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "002"
                           sRetVal = "Token Authentication Error 002 -  Insert SQL Error.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "003"
                           sRetVal = "Token Authentication Error 003 - Invalid Status.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "004"
                           sRetVal = "Token Authentication Error 004 -  No Record Found.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "005"
                           sRetVal = "Token Authentication Error 005 - No Token found During Request Challenge Code.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "006"
                           sRetVal = "Token Authentication Error 006 - Error connect to Operation Table.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "007"
                           sRetVal = "Token Authentication Error 007 - Verification error, no valid Request found.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "008"
                           sRetVal = "Token Authentication Error 008 - Invalid Request ID.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "009"
                           sRetVal = "Token Authentication Error 009 - No token found during verification.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "015"
                           sRetVal = "Token Authentication Error 015 - Verification error, verfication interval exceeded timeout duration.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case "016"
                           sRetVal = "Token Authentication Error 016 - SQL Transaction Timeout.  Please contact " & gc_Const_CompanyName & "'s Administrator."
                        Case Else
                           sRetVal = oToken.ChallengeCode
                           bRetVal = True
                     End Select
                  End If
               Else
                  bRetVal = False
               End If
            End With
         Catch ex As Exception

            LogError("clsBasePage - fncLoadChallengeCode")
         Finally
            oToken = Nothing
         End Try
         Return bRetVal
      End Function
      Public Function fncTokenAuthenticate(ByVal sOTP As String) As Boolean
         Dim oToken As New dyna2.client
         Dim bRetVal As Boolean = False
         Try
            With oToken
               .Host = fncAppSettings("TokenHost")
               .Port = fncAppSettings("TokenPort")
               .Type = "Submit"
               .OTP = sOTP

               .CustomerID = ss_lngUserID

               'If Customer ID is supplied, LogonID and OrgID not need to be supplied.
               '.LogonID = Session(gc_UserLoginName)
               '.OrgID = ss_lngOrgID
               oToken.Process()
               bRetVal = oToken.Authenticated
            End With
         Catch ex As Exception
            LogError("clsBasePage - fncTokenAuthenticate")
         Finally
            oToken = Nothing
         End Try
         Return bRetVal
      End Function
#End Region

#Region "   Logout"
      Public Sub prcLogout()
         Server.Transfer(gc_LogoutPath, False)
      End Sub
#End Region

#Region "   Protected Overridable Function"

      Protected Overridable Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
         Session.LCID = 2057
         prcCacheControl()
         If Not fncIsSessionExpired() Then
            Server.Transfer(gc_LogoutPath, False)
         End If

         'onload="countDown();fillColor('cHeader');"  onmousemove="resetCounter()" onclick="resetCounter()" style="margin: 0px 0px 0px 0px"
      End Sub

#End Region

#Region "Validation Code Process"

        Public Function fncValidationCodeProcess(ByVal sValidationCode As String, ByRef sMsg As String) As Boolean
            Dim sDbAuthCode As String = ""
            Dim bIsAuthCode As Boolean = False
            Dim iAttempts As Int16
            Dim sUserName As String = ""
            Dim sSubject As String = ""
            Dim sBody As String = ""
            Dim clsCommon As New clsCommon
            Dim clsUsers As New clsUsers
            Dim bRetVal As Boolean = True

            Try
                'Check Session Value for Authorization Lock - Start
                If Not IsNumeric(Session("AUTH_LOCK")) Or Session("AUTH_LOCK") = 0 Then
                    Session("AUTH_LOCK") = 0
                End If
                'Check Session Value for Authorization Lock - Stop

                'Check If AuthCode is Valid - Start
                sDbAuthCode = clsCommon.fncPassAuth(ss_lngUserID, "A", ss_lngOrgID)
                bIsAuthCode = IIf(sDbAuthCode = sValidationCode, True, False)
                'Check If AuthCode is Valid - Stop

                'Check for invalid Authorization Code Attempts - START
                If Not bIsAuthCode Then
                    iAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                    If Not iAttempts = 2 Then
                        If Not bIsAuthCode Then
                            'Increment Attempts
                            iAttempts = iAttempts + 1
                            'Assign Current Attempt to Session
                            Session("AUTH_LOCK") = iAttempts
                            'Display Message
                            sMsg = "Validation code is invalid. Please enter a valid Validation Code."
                            bRetVal = False
                            Exit Try
                        End If
                    ElseIf iAttempts = 2 Then
                        If Not bIsAuthCode Then
                            'Disbable Button


                            'Get User Name
                            sUserName = clsCommon.fncBuildContent("User Name", "", ss_lngUserID, ss_lngUserID)
                            'Mail Subject
                            sSubject = sUserName & " (" & ss_strUserType & ") Locked/Inactive."
                            'Mail Body
                            sBody = sUserName & " (" & ss_strUserType & ")" & " has been Locked/Inactive on " & Now() & " due to Invalid Validation Code attempts. Please change the User Validation Code."
                            'Lock Authorization Code
                            Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "A")
                            'Send Mail
                            Call clsCommon.prcSendMails("USER LOCK", ss_lngOrgID, ss_lngUserID, ss_lngGroupID, sSubject, sBody)
                            'Track Auth Lock
                            Call clsUsers.prcLockHistory(ss_lngUserID, "A")
                            'Display Message
                            sMsg = "Your account has been locked due to invalid attempts. Please contact your " & gc_UT_SysAdminDesc & "."
                            bRetVal = False
                            Exit Try
                        End If
                    End If
                End If
            Catch ex As Exception
                LogError("clsBasePage - fncValidationCodeProcess")
            End Try
            Return bRetVal
        End Function

#End Region

   End Class

End Namespace