#Region "Name Spaces "

Imports MaxModule
Imports MaxGeneric
Imports System.Reflection.MethodBase

#End Region

Public Class WebCommon

#Region "Global Declarations "

    'Create Instances - Start
    Private _Message As New Message
    Private _DataBase As New DataBase
    Private _WebHelper As New WebHelper
    Private _MaxCommon As New MaxReadWrite.Common
    'Create Instances - Stop

#End Region

#Region "Get Group Service Types "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To get the Group Service Types
    'Created: 21/06/2012
    Public Function GetGroupServiceTypes(ByVal GroupId As Integer, _
       ByVal ServiceType As String) As DataTable

        Return _DataBase.GetData(_WebHelper.SqlGetGroupPaymentServices & GroupId & _
               clsGeneric.AddComma() & clsGeneric.AddQuotes(ServiceType))

    End Function

#End Region

#Region "Get Group Service Details "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To get the Service Details
    'Created: 21/06/2012
    Public Function GetServiceDetails(ByVal ServiceId As Short) As DataTable

        Return _DataBase.GetData(_WebHelper.SqlGetServiceDetails & ServiceId)

    End Function

#End Region

#Region "Get Group File Formats "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To get the Group File Formats
    'Created: 21/06/2012
    Public Function GetGroupFileFormats(ByVal ServiceType As String, _
      ByVal OrganizationId As Integer, ByVal GroupId As Integer) As DataTable

        Return _DataBase.GetData(_WebHelper.SqlGetGroupFileFormats & OrganizationId & _
           clsGeneric.AddComma() & GroupId & clsGeneric.AddComma() & clsGeneric.AddQuotes(ServiceType))

    End Function

#End Region

#Region "Get Group Bank Accounts "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To get the Group Bank Accounts
    'Created: 21/06/2012
    Public Function GetGroupBankAccounts(ByVal OrganizationId As Integer, _
         ByVal GroupId As Integer) As DataTable

        Return _DataBase.GetData(_WebHelper.SqlGetGroupBankAccounts & OrganizationId & _
           clsGeneric.AddComma() & GroupId)

    End Function

#End Region

#Region "Cutoff Time Check "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : Cutoff Time Check
    'Created        : 11/02/2010
    Public Function IsCutoffTime(ByVal ServiceId As Short, ByVal ServiceType As String, _
        ByVal TransactionDate As String, ByVal UserRole As Short, ByRef ErrorDetails As String) As Boolean

        'Create Instances
        Dim CutoffTimes As DataTable = Nothing

        'Variable Declarations - Start
        Dim CurHour As Short = 0, CurMinute As Short = 0, CutHour As Short = 0
        Dim CutMinute As Short = 0, CutMode As String = Nothing, CutoffTime As String = Nothing
        'Variable Declarations - Stop

        Try

            'If payment date is current date - Start
            If Convert.ToDateTime(TransactionDate) = DateTime.Today Then

                'Get Cutoff Times - Start
                CutoffTimes = _DataBase.GetData(_WebHelper.SqlGetCutoffTimes & _
                   clsGeneric.AddQuotes(ServiceType) & clsGeneric.AddComma() & ServiceId)
                'Get Cutoff Time - Stop

                'if cutoff time available - Start
                If CutoffTimes.Rows.Count > 0 Then

                    'Get Values - Start
                    CutHour = clsGeneric.NullToShort(CutoffTimes.Rows(0)(_WebHelper.ColCutoffHour))
                    CutMinute = clsGeneric.NullToShort(CutoffTimes.Rows(0)(_WebHelper.ColCutoffMin))
                    CutMode = clsGeneric.NullToString(CutoffTimes.Rows(0)(_WebHelper.ColCutoffType))
                    'Get Values - Stop

                End If
                'if cutoff time available - Stop

                'get values - Start
                CurHour = clsGeneric.NullToShort(DateTime.Now.Hour)
                CurMinute = clsGeneric.NullToShort(DateTime.Now.Minute)
                'get values - Stop

                'Convert to 24 hour format - Start
                If CutMode = WebHelper.PM And CutHour < 12 Then
                    CutHour = clsGeneric.NullToShort(CutHour + 12)
                End If
                'Convert to 24 hour format - Stop

                'Build Cutoff Time
                CutoffTime = Format(CutHour, "00") & ":" & Format(CutMinute, "00") & " " & CutMode

                'if Current hour greater than cutoff hour
                If CurHour > CutHour Then
                    ErrorDetails = _Message.MsgCutoffTime(ServiceType, UserRole, CutoffTime)
                    Return True
                    'if current hour and cutoff hour same and current minute greater than cutoff minute
                ElseIf CurHour = CutHour And CurMinute > CutMinute Then
                    ErrorDetails = _Message.MsgCutoffTime(ServiceType, UserRole, CutoffTime)
                    Return True
                End If

            End If
            'If payment date is current date - Stop

            'if Transaction Date past date - Start
            If Convert.ToDateTime(TransactionDate) < DateTime.Today Then
                ErrorDetails = _Message.MsgCutoffTime(ServiceType, UserRole, CutoffTime)
                Return True
            End If
            'if Transaction Date past date - Stop

            Return False

        Finally

        End Try

    End Function

#End Region

#Region "Auth Code Check "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To Check user Auth Code
    'Created: 21/06/2012
    Public Function CheckAuthCode(ByVal OrgId As Integer, ByVal GroupId As Integer, _
        ByVal UserId As Integer, ByVal AuthCode As String, _
        ByRef IsAuthCodeLocked As Boolean) As Boolean

        'Create instances - Start
        Dim _DataRow As DataRow = Nothing
        Dim UserDetails As DataTable = Nothing
        Dim OrgUserDetails As DataTable = Nothing
        Dim clsEncryption As New MaxPayroll.Encryption
        'Create instances - Stop

        'Variable Declarations - Start
        Dim OrgUserId As Integer = 0
        Dim MailSubject As String = Nothing, MailBody As String = Nothing, UserFlag As Short = 0
        Dim UserAuthCode As String = Nothing, UserName As String = Nothing, UserRole As String = Nothing
        'Variable Declarations - Stop

        Try

            'Get User Details
            UserDetails = _DataBase.GetData(_WebHelper.SqlGetUserDetails & UserId)

            'if user details available - Start
            If UserDetails.Rows.Count > 0 Then

                'Get Values - Start
                UserRole = clsGeneric.NullToString(UserDetails.Rows(0)(_WebHelper.ColUserRole))
                UserName = clsGeneric.NullToString(UserDetails.Rows(0)(_WebHelper.ColUserName))
                UserAuthCode = clsEncryption.Cryptography(UserDetails.Rows(0)(_WebHelper.ColUserAuthCode))
                'Get Values - Stop

            End If
            'if user details available - Stop

            'If Auth Code does not match - Start
            If Not AuthCode = UserAuthCode Then

                'if Invalid Count Limit Reached - Start
                If Not _WebHelper.AuthLockCount = 2 Then

                    'increment counter
                    _WebHelper.AuthLockCount = _WebHelper.AuthLockCount + 1

                    Return False

                Else

                    'Set Auth Code Lock Flag
                    IsAuthCodeLocked = True

                    'Build Mail Subject
                    MailSubject = _Message.MsgAuthCodeLockSubject(UserName, UserRole)

                    'Build Mail Body
                    MailBody = _Message.MsgAuthCodeLockBody(UserName, UserRole)

                    'Lock User Auth Code
                    Call PassAuthLock(UserId, WebHelper.UserLockType.AuthorizationCode)

                    'get Org User Details
                    OrgUserDetails = _DataBase.GetData(_WebHelper.SqlGetOrgUserDetails & OrgId)

                    'if org user details available - Start
                    If OrgUserDetails.Rows.Count > 0 Then

                        'Loop thro the Org Users - Start
                        For Each _DataRow In OrgUserDetails.Rows

                            'Get Values - Start
                            OrgUserId = clsGeneric.NullToInteger(_DataRow(_WebHelper.ColUserId))
                            UserRole = clsGeneric.NullToShort(_DataRow(_WebHelper.ColUserRole))
                            'Get Values - Stop

                            'if customr admin or customer auth - Start
                            If UserRole = EnumHelp.MpgUserRoles.Customer_Admin Or _
                                UserRole = EnumHelp.MpgUserRoles.Customer_Auth Then

                                'Send Mails
                                Call SendMail(OrgId, GroupId, UserId, OrgUserId, MailSubject, MailBody)

                            End If
                            'if customr admin or customer auth - Stop

                        Next
                        'Loop thro the Org Users - Stop

                    End If
                    'if org user details available - Stop

                    Return False

                End If
                'if Invalid Count Limit Reached - Stop

            End If
            'If Auth Code does not match - Stop

            Return True

        Finally

        End Try

    End Function

#End Region

#Region "Send Mail To Workflow Users "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To Send Mail to work flow users
    'Created: 21/06/2012
    Public Sub SendMailToWorkFlowUsers(ByVal OrgId As Integer, ByVal GroupId As Integer, _
       ByVal InputFileId As Integer, ByVal MailFrom As Integer, ByVal MailSubject As String, _
       ByVal MailBody As String)

        'Create Instances - Start
        Dim _DataRow As DataRow = Nothing
        Dim WorkflowUsers As DataTable = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim UserId As Integer = 0, UserFlag As Short = 0

        Try

            'Get Workflow Users
            WorkflowUsers = _DataBase.GetData(_WebHelper.SqlGetWorkflowUsers & InputFileId)

            'If Workflow users available - Start
            If WorkflowUsers.Rows.Count > 0 Then

                'Loop thro Work flow users - Start
                For Each _DataRow In WorkflowUsers.Rows

                    'Get Values - Start
                    UserId = clsGeneric.NullToInteger(_DataRow(_WebHelper.ColUserId))
                    UserFlag = clsGeneric.NullToShort(_DataRow(_WebHelper.ColUserFlag))
                    'Get Values - Stop

                    'Send Mails
                    Call SendMail(OrgId, GroupId, MailFrom, UserId, MailSubject, MailBody)

                Next
                'Loop thro Work flow users - Stop

            End If
            'If Workflow users available - Stop

        Catch ex As Exception

            'Log Error - Start
            Call ErrorLog(OrgId, GetCurrentMethod().ToString(), ex.Message)

        End Try

    End Sub

#End Region

#Region "Send Mails "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To Send Mails to Inbox
    'Created: 21/06/2012
    Public Sub SendMail(ByVal OrgId As Integer, ByVal GroupId As Integer, _
       ByVal MailFrom As Integer, ByVal MailTo As Integer, _
       ByVal MailSubject As String, ByVal MailBody As String)

        Dim SqlStatement As String = _WebHelper.SqlSendMails & OrgId
        SqlStatement &= clsGeneric.AddComma() & GroupId
        SqlStatement &= clsGeneric.AddComma() & MailFrom
        SqlStatement &= clsGeneric.AddComma() & MailTo
        SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(MailSubject)
        SqlStatement &= clsGeneric.AddComma() & clsGeneric.AddQuotes(MailBody)
        Call _DataBase.ExecuteSQL(SqlStatement)

    End Sub

#End Region

#Region "Error Log "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To Track Error Logs
    'Created: 21/06/2012
    Public Sub ErrorLog(ByVal OrganizationId As Integer, ByVal ErrorSource As String, _
                ByVal CatchMessage As String)

        Call _DataBase.ExecuteSQL(_WebHelper.SQLErrorLog & OrganizationId & _
               clsGeneric.AddComma & clsGeneric.AddQuotes(ErrorSource) _
                   & clsGeneric.AddComma & clsGeneric.AddQuotes( _
                       clsGeneric.ReplaceInvalidSQLChars(CatchMessage)))

    End Sub

#End Region

#Region "Password\Auth Code Lock "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To Password\Authcode Lock
    'Created: 21/06/2012
    Public Sub PassAuthLock(ByVal UserId As Integer, _
        ByVal UserLockType As Short)

        Call _DataBase.ExecuteSQL(_WebHelper.SQLUserLockType & UserId & _
            clsGeneric.AddComma() & UserLockType)

    End Sub

#End Region

#Region "Get Org Codes for Bank Accounts "

    'Author: Sujith Sharatchandran, T-Melmax Sdn Bhd
    'Purpose: To get the OrgCodes for given Bank Account
    'Created: 15/07/2012
    Public Function GetBankAccountOrgCodes(ByVal OrganizationId As Integer, _
         ByVal AccountNumber As String) As DataTable

        Return _DataBase.GetData(_WebHelper.SqlGetAccountNoOrgCodes & OrganizationId & _
           clsGeneric.AddComma() & AccountNumber)

    End Function

#End Region

End Class
