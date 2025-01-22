'****************************************************************************************************
'Class Name     : clsCommon
'ProgId         : MaxPayroll.clsCommon
'Purpose        : Common Functions
'Author         : Sujith Sharatchandran - 
'Created        : 04/10/2004
'*****************************************************************************************************

Option Strict Off
Option Explicit On 

Imports System.IO
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports MaxFTP.clsFTP
Imports Microsoft.ApplicationBlocks.Data
Imports System.Threading
Imports System.Diagnostics
Imports System.Configuration.ConfigurationManager
Imports MaxModule
Imports MaxGeneric

Namespace MaxPayroll

    Public Class clsCommon
        Private _Helper As New Helper

#Region "Global declaration "

        Private _MaxDataBase As New MaxModule.DataBase

#End Region
        Private _count As Object

        Private Property Count As Object
            Get
                Return _count
            End Get
            Set(value As Object)
                _count = value
            End Set
        End Property

        Public Sub fncBindDDLStatutory(ByRef DDL As DropDownList)
            DDL.Items.Add(New ListItem("N/A", ""))
            'DDL.Items.Add(New ListItem("EPF", fncGetPrefix(enmStatutory.E_EPF)))
            'DDL.Items.Add(New ListItem("Socso", fncGetPrefix(enmStatutory.S_Socso)))
            'DDL.Items.Add(New ListItem("LHDN", fncGetPrefix(enmStatutory.L_LHDN)))
            'DDL.Items.Add(New ListItem("ZAKAT", fncGetPrefix(enmStatutory.Z_Zakat)))
        End Sub

        Public Function fncGetUserTypeDesc(ByVal sUserType As String) As String
            Select Case sUserType
                Case gc_UT_Auth
                    Return gc_UT_AuthDesc
                Case gc_UT_BankAdmin
                    Return gc_UT_BankAdminDesc
                Case gc_UT_BankOperator
                    Return gc_UT_BankOperatorDesc
                Case gc_UT_BankAuth
                    Return gc_UT_BankAuthDesc
                Case gc_UT_BankUser
                    Return gc_UT_BankUserDesc
                Case gc_UT_SysAdmin
                    Return gc_UT_SysAdminDesc
                Case gc_UT_Interceptor
                    Return gc_UT_InterceptorDesc
                Case gc_UT_InquiryUser
                    Return gc_UT_InquiryUserDesc
                Case gc_UT_Reviewer
                    Return gc_UT_ReviewerDesc
                Case gc_UT_SysAuth
                    Return gc_UT_SysAuthDesc
                Case gc_UT_Uploader
                    Return gc_UT_UploaderDesc
                Case Else
                    Return ""
            End Select
        End Function

#Region "Get Enum Prefix (Specially Occupied By enmUserType)"

        Public Shared Function fncGetPrefix(ByVal sValue As String) As String
            Return sValue.Substring(0, sValue.IndexOf("_")) & ""
        End Function
        Public Shared Function fncGetPrefix(ByVal enumerator As Object) As String
            Return fncGetPrefix(enumerator.ToString)
        End Function
        Public Shared Function fncGetPostFix(ByVal enumerator As Object) As String
            Return fncGetPostFix(enumerator.ToString)
        End Function
        Public Shared Function fncGetPostFix(ByVal sValue As String) As String
            Return sValue.Substring(sValue.IndexOf("_") + 1).Replace("_", " ")
        End Function
        Public Function fncGetPrefixNS(ByVal enumerator As Object) As String
            Return fncGetPrefix(enumerator.ToString)
        End Function
#End Region

#Region "Check Password/Authorization Code"

        '****************************************************************************************************
        'Function Name  : fnValidateAuthCode
        'Purpose        : Validate Authorization Code
        'Arguments      : User ID
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Public Function fncPassAuth(ByVal lngUserId As Long, ByVal strRequest As String, ByVal lngOrgId As Long) As String

            'Create Instance of SQL Data reader Object
            Dim sdrUsers As SqlDataReader

            'Create Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of SQL Command Object
            Dim cmdUsers As New SqlCommand

            'Variable Declarations
            Dim strAuthCode As String = "", strPassword As String = ""

            Try
                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Get the Auth Code
                With cmdUsers
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "Exec pg_GetUserDetail " & lngUserId
                    sdrUsers = .ExecuteReader(CommandBehavior.CloseConnection)
                End With

                'If Record Found
                If sdrUsers.HasRows Then
                    sdrUsers.Read()
                    strPassword = sdrUsers("UPwd")
                    strAuthCode = sdrUsers("UAuthCode")
                    sdrUsers.Close()
                End If

                If Not strPassword = "" Then
                    strPassword = clsEncryption.Cryptography(strPassword)
                End If

                If Not strAuthCode = "" Then
                    strAuthCode = clsEncryption.Cryptography(strAuthCode)
                End If

                If strRequest = "P" Then
                    Return strPassword
                ElseIf strRequest = "A" Then
                    Return strAuthCode
                End If

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnValidateAuthCode - clsUpload", Err.Number, Err.Description)

                Return ""

            Finally

                'Destroy SQL Data Reader Object
                sdrUsers = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Encryption Class object
                clsEncryption = Nothing

                'Destroy Instance of SQL Command Object
                cmdUsers = Nothing

            End Try
            Return ""
        End Function

#End Region

#Region "Cutoff Time"

        '****************************************************************************************************
        'Procedure Name : fncCutoffTime()
        'Purpose        : To get the Cutoff Time for Selected Product
        'Arguments      : Product Type, Hour, Minute
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 11/08/2004
        '*****************************************************************************************************
        Public Function fncCutoffTime(ByVal strFileType As String, ByVal lngOrgId As Long, _
                    ByVal lngUserId As Long, ByRef strTime As String, ByVal intDay As Int16, _
                        ByVal intMonth As Int16, ByVal intYear As Int16, ByVal strOption As String, ByVal strBankId As String) As Boolean

            'Create Instance of SQL Data Reader
            Dim sdrCutOffTime As SqlDataReader

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdCutOffTime As New SqlCommand

            'Variable Declarations
            Dim intCurrDay As Int16 = 0, intCurrMonth As Int16 = 0, intCurrYear As Int16 = 0
            Dim IsCutoff As Boolean = False, strMode As String = "", intHour As Int16 = 0
            Dim intMinute As Int16 = 0, intCurrHour As Int16 = 0, intCurrMin As Int16 = 0
            Dim PaySerId As Short = 0

            Try

                'Get the PayserId for Filetype--Added on 12/06/2011--Naresh
                PaySerId = GetPayserId(strFileType, lngOrgId, lngUserId)

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                intCurrDay = Day(Today)             'Current Day
                intCurrMonth = Month(Today)         'Current Month
                intCurrYear = Year(Today)           'Current Year
                intCurrHour = Hour(TimeOfDay)       'Current Hour
                intCurrMin = Minute(TimeOfDay)      'Current Minute

                With cmdCutOffTime
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "Exec pg_GetTime '" & strOption & "','" & strBankId & "'" _
                        & MaxGeneric.clsGeneric.AddComma & PaySerId
                    sdrCutOffTime = .ExecuteReader(CommandBehavior.CloseConnection)
                End With

                If sdrCutOffTime.HasRows Then
                    sdrCutOffTime.Read()
                    strMode = sdrCutOffTime("TF")
                    intHour = sdrCutOffTime("HH")
                    intMinute = sdrCutOffTime("MM")
                    sdrCutOffTime.Close()
                End If

                strTime = Format(intHour, "00") & ":" & Format(intMinute, "00") & " " & strMode

                If strMode = "PM" And intHour < 12 Then
                    intHour = intHour + 12
                End If

                If intDay = intCurrDay And intMonth = intCurrMonth And intYear = intCurrYear Then

                    If intCurrHour > intHour Then
                        IsCutoff = True
                    ElseIf intCurrHour = intHour And intCurrMin > intMinute Then
                        IsCutoff = True
                    End If

                End If

                Return IsCutoff


            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcCutoffTime - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Data Reader
                sdrCutOffTime = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function


        Public Function fncCutoffTime(ByVal strFileType As String, ByVal lngOrgId As Long, _
                   ByVal lngUserId As Long, ByRef strTime As String, ByVal intDay As Int16, _
                       ByVal intMonth As Int16, ByVal intYear As Int16, ByVal strOption As String) As Boolean

            'Create Instance of SQL Data Reader
            Dim sdrCutOffTime As SqlDataReader

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdCutOffTime As New SqlCommand

            'Variable Declarations
            Dim intCurrDay As Int16 = 0, intCurrMonth As Int16 = 0
            Dim intCurrYear As Int16 = 0, IsCutoff As Boolean = 0
            Dim strMode As String = "", intHour As Int16 = 0
            Dim intMinute As Int16 = 0, intCurrHour As Int16 = 0
            Dim intCurrMin As Int16 = 0, PaySerId As Short = 0


            Try

                'Get the PayserId for Filetype--Added on 12/06/2011--Naresh
                PaySerId = GetPayserId(strFileType, lngOrgId, lngUserId)

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                intCurrDay = Day(Today)             'Current Day
                intCurrMonth = Month(Today)         'Current Month
                intCurrYear = Year(Today)           'Current Year
                intCurrHour = Hour(TimeOfDay)       'Current Hour
                intCurrMin = Minute(TimeOfDay)      'Current Minute

                With cmdCutOffTime
                    .Connection = clsGeneric.SQLConnection
                    '071024 Marcus: Temporary supply "2" for the new argument added in pg_GetTime
                    'This is to be called by file authorization screen, modification is needed for this screen.
                    .CommandText = "Exec pg_GetTime '" & strOption & "','" & ConfigurationManager.AppSettings("DefaultBankCode") & "'" & _
                        MaxGeneric.clsGeneric.AddComma & PaySerId
                    sdrCutOffTime = .ExecuteReader(CommandBehavior.CloseConnection)

                End With

                If sdrCutOffTime.HasRows Then
                    sdrCutOffTime.Read()
                    strMode = sdrCutOffTime("TF")
                    intHour = sdrCutOffTime("HH")
                    intMinute = sdrCutOffTime("MM")
                    sdrCutOffTime.Close()
                End If

                strTime = Format(intHour, "00") & ":" & Format(intMinute, "00") & " " & strMode

                If strMode = "PM" And intHour < 12 Then
                    intHour = intHour + 12
                End If

                If intDay = intCurrDay And intMonth = intCurrMonth And intYear = intCurrYear Then
                    If intCurrHour > intHour Then
                        IsCutoff = True
                    ElseIf intCurrHour = intHour And intCurrMin > intMinute Then
                        IsCutoff = True
                    End If
                End If

                Return IsCutoff

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcCutoffTime - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Data Reader
                sdrCutOffTime = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

        Public Function fncGetCutOffTimeBankId(ByVal strGivenName As String) As String

            Dim strRetVal As String
            Dim strSQL As String = "Exec pg_GetCutOffTimeBandId '" & strGivenName & "'"

            strRetVal = SqlHelper.ExecuteScalar(Generic.sSQLConnection, CommandType.Text, strSQL)

            Return strRetVal

        End Function


#End Region

#Region "Send Mails"
        '****************************************************************************************************
        'Procedure Name : prcSendMails
        'Purpose        : Send Mails to Uploader,Reviewer,Authorizer
        'Arguments      : Organisation Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Public Sub prcSendMails(ByVal strRequest As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                    ByVal lngGroupId As Long, ByVal strSubject As String, ByVal strBody As String, _
                                Optional ByVal lngToId As Long = 0)

            'Create Instance of DataRow
            Dim drMailList As System.Data.DataRow

            'Create Instance of SQL Command Object
            Dim cmdSendMail As New SqlCommand

            'Create Instance of SQL Data Adapter
            Dim sdaMailList As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsMailList As New System.Data.DataSet

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                If Not strRequest = "SEND MAIL" Then


                    If lngToId > 0 AndAlso strRequest <> "BANK AUTH" Then
                        With cmdSendMail
                            .Connection = clsGeneric.SQLConnection
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = "pg_SendMail"
                            .Parameters.Add(New SqlParameter("@in_Request", "SEND MAIL"))
                            .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                            .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                            .Parameters.Add(New SqlParameter("@in_FromId", lngUserId))
                            .Parameters.Add(New SqlParameter("@in_ToId", lngToId))
                            .Parameters.Add(New SqlParameter("@in_Subject", strSubject))
                            .Parameters.Add(New SqlParameter("@in_Body", strBody))
                            .ExecuteNonQuery()
                            .Parameters.Clear()
                        End With
                    Else
                        'Fetch Records And Assign to Adaptor
                        sdaMailList = New SqlDataAdapter("Exec pg_MailList " & lngOrgId & ",'" & strRequest & "'," & lngGroupId, clsGeneric.SQLConnection)

                        'Fill Data Set
                        sdaMailList.Fill(dsMailList, "MAILBOX")

                        'Loop Thro Entire List of Recipients - Start
                        For Each drMailList In dsMailList.Tables("MAILBOX").Rows

                            lngToId = drMailList("UID")

                            With cmdSendMail
                                .Connection = clsGeneric.SQLConnection
                                .CommandType = CommandType.StoredProcedure
                                .CommandText = "pg_SendMail"
                                .Parameters.Add(New SqlParameter("@in_Request", "SEND MAIL"))
                                .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                                .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                                .Parameters.Add(New SqlParameter("@in_FromId", lngUserId))
                                .Parameters.Add(New SqlParameter("@in_ToId", lngToId))
                                .Parameters.Add(New SqlParameter("@in_Subject", strSubject))
                                .Parameters.Add(New SqlParameter("@in_Body", strBody))
                                .ExecuteNonQuery()
                                .Parameters.Clear()
                            End With

                        Next
                        'Loop Thro Entire List of Recipients - Stop
                    End If


                Else

                    With cmdSendMail
                        .Connection = clsGeneric.SQLConnection
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = "pg_SendMail"
                        .Parameters.Add(New SqlParameter("@in_Request", strRequest))
                        .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                        .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                        .Parameters.Add(New SqlParameter("@in_FromId", lngUserId))
                        .Parameters.Add(New SqlParameter("@in_ToId", lngToId))
                        .Parameters.Add(New SqlParameter("@in_Subject", strSubject))
                        .Parameters.Add(New SqlParameter("@in_Body", strBody))
                        .ExecuteNonQuery()
                        .Parameters.Clear()
                    End With

                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcSendMails - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Create Instance of Data Row
                drMailList = Nothing

                'Destroy Instance of SQL Command Object
                cmdSendMail = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaMailList = Nothing

                'Destroy Instance of Data Ser
                dsMailList = Nothing

                'Destroy generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

        Public Sub prcSendMails(ByVal Trans As SqlTransaction, ByVal strRequest As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngGroupId As Long, ByVal strSubject As String, ByVal strBody As String, ByVal lngToId As Long)

            'Create Instance of DataRow
            Dim drMailList As System.Data.DataRow

            'Create Instance of SQL Command Object
            Dim cmdSendMail As New SqlCommand

            'Create Instance of SQL Data Adapter
            Dim sdaMailList As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsMailList As New System.Data.DataSet

            Try

                'Initialize SQL Connection
                'Call clsGeneric.SQLConnection_Initialize()

                If Not strRequest = "SEND MAIL" Then
                    If lngToId > 0 Then 'AndAlso strRequest <> "BANK AUTH" Then
                        prcSendMailTrans(Trans, "SEND MAIL", lngOrgId, lngUserId, lngGroupId, strSubject, strBody, lngToId)

                    Else
                        'Fetch Records And Assign to Adaptor
                        sdaMailList = New SqlDataAdapter("Exec pg_MailList " & lngOrgId & "," & clsDB.SQLStr(strRequest, clsDB.SQLDataTypes.Dt_String) & "," & lngGroupId, clsGeneric.SQLConnection)

                        'Fill Data Set
                        sdaMailList.Fill(dsMailList, "MAILBOX")

                        'Loop Thro Entire List of Recipients - Start
                        For Each drMailList In dsMailList.Tables("MAILBOX").Rows

                            lngToId = drMailList("UID")
                            prcSendMailTrans(Trans, "SEND MAIL", lngOrgId, lngUserId, lngGroupId, strSubject, strBody, lngToId)
                        Next
                        'Loop Thro Entire List of Recipients - Stop
                    End If
                Else
                    prcSendMailTrans(Trans, strRequest, lngOrgId, lngUserId, lngGroupId, strSubject, strBody, lngToId)
                End If
            Catch
                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcSendMails - clsCommon", Err.Number, Err.Description)
            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Create Instance of Data Row
                drMailList = Nothing

                'Destroy Instance of SQL Command Object
                cmdSendMail = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaMailList = Nothing

                'Destroy Instance of Data Ser
                dsMailList = Nothing

                'Destroy generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

        Private Function prcSendMailTrans(ByVal Trans As SqlTransaction, ByVal strRequest As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngGroupId As Long, ByVal strSubject As String, ByVal strBody As String, ByVal lngToId As Long) As Boolean
            Dim params(6) As SqlParameter
            Dim bRetVal As Boolean = False

            Try
                params(0) = New SqlParameter("@in_Request", strRequest)
                params(1) = New SqlParameter("@in_OrgId", lngOrgId)
                params(2) = New SqlParameter("@in_GroupId", lngGroupId)
                params(3) = New SqlParameter("@in_FromId", lngUserId)
                params(4) = New SqlParameter("@in_ToId", lngToId)
                params(5) = New SqlParameter("@in_Subject", strSubject)
                params(6) = New SqlParameter("@in_Body", strBody)
                SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "pg_SendMail", params)
                bRetVal = True
            Catch ex As Exception
                Throw ex
            End Try

            Return bRetVal

        End Function


#End Region
#Region " Check Script Encryption "
        'Author     : Deena T-Melmax Sdn Bhd
        'Created on : 29/05/2023
        'Purpose    : Check Script Validation
        Public Function CheckScriptValidation(ByVal FileTypeName As String) As Boolean

            Try
                Dim strBody As String = ""
                Dim isValid As Boolean = True
                Dim txtAuthCode1 As String = System.Web.HttpUtility.HtmlEncode(FileTypeName)
                If txtAuthCode1.Contains(">") OrElse txtAuthCode1.Contains("<") OrElse txtAuthCode1.Contains("i&gt") OrElse txtAuthCode1.Contains("alert") OrElse txtAuthCode1.Contains("&lt") OrElse txtAuthCode1.Contains("&gt") OrElse txtAuthCode1.Contains(")") OrElse txtAuthCode1.Contains("(") OrElse txtAuthCode1.Contains("&") OrElse txtAuthCode1.Contains("script") OrElse txtAuthCode1.Contains("onmouseover") OrElse txtAuthCode1.Contains("prompt") OrElse txtAuthCode1.Contains("confirm") Then
                    isValid = False
                End If
                Return isValid

            Catch ex As Exception
                Return False
            End Try

        End Function

        Public Function ErrorCodeScript() As String

            Try
                Dim sb As New StringBuilder()
                sb.Append("<script language='JavaScript'>")
                sb.Append("alert('Invalid Content');")
                sb.Append("</script>")
                Return sb.ToString
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function ErrorCodeScript2() As String

            Try
                Dim sb As New StringBuilder()
                sb.Append("<script language='JavaScript'>")
                sb.Append("alert('Cannot be more than 60 days');")
                sb.Append("</script>")
                Return sb.ToString
            Catch ex As Exception
                Return Nothing
            End Try

        End Function
#End Region
#Region "Build Requested Content"

        '****************************************************************************************************
        'Function Name  : fncBuildContent
        'Purpose        : Create Content
        'Arguments      : Matching Field,Organisatio Id, User Id
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Public Function fncBuildContent(ByVal strMatchField As String, ByVal strFileType As String, _
            ByVal lngOrgId As Long, ByVal lngUserId As Long, Optional ByVal strFormat As String = "", _
                Optional ByVal strAccNo As String = "", Optional ByVal intBankId As Integer = 0, _
                Optional ByVal intGroupId As Integer = 0, Optional ByVal intSerAccId As Int16 = 0) As String

            'Create Instance of SQL Command Object
            Dim cmdContent As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strContent As String

            Try

                'Initialize SQL connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdContent
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_GetMatchField"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_MatchField", strMatchField))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileFormat", strFormat))
                    .Parameters.Add(New SqlParameter("@in_TaxNo", strAccNo))
                    .Parameters.Add(New SqlParameter("@in_SerAccNo", intSerAccId))
                    '.Parameters.Add(New SqlParameter("@in_BankId", intBankId))
                    '.Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@out_MatchField", SqlDbType.VarChar, 100, ParameterDirection.Output, False, 0, 0, "out_MatchField", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                'Get Requested Content
                strContent = IIf(IsDBNull(cmdContent.Parameters("@out_MatchField").Value), "", cmdContent.Parameters("@out_MatchField").Value)

                Return strContent

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBuildContent - clsCommon", Err.Number, Err.Description)

                Return gc_Status_Error

            Finally

                'Terminate SQL connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdContent = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Organization Token Setting"

        '****************************************************************************************************
        'Function Name  : fncGetOrgTokenSetting
        'Purpose        : Returns Organization's Token Setting
        'Arguments      : Organization ID, User ID
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 15/1/2007
        '*****************************************************************************************************
        Public Function fncGetOrgTokenSetting(ByVal lngOrgId As Long, ByVal lngUserId As Long) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdContent As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Dim bRetVal As Boolean

            Try

                'Initialize SQL connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdContent
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_GetOrgTokenSetting"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    bRetVal = CBool(.ExecuteScalar())
                End With

                'Get Requested Content
                'strContent = IIf(IsDBNull(cmdContent.Parameters("@out_MatchField").Value), "", cmdContent.Parameters("@out_MatchField").Value)

                Return bRetVal

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "pg_GetOrgTokenSetting - clsCommon", Err.Number, Err.Description)

                Return gc_Status_Error

            Finally

                'Terminate SQL connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdContent = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

 Public Function fncBreakValue(ByVal strValue As String, ByVal strDelimiter As String, ByVal bFrontPortion As Boolean) As String
            Dim sRetVal As String = ""
            Try
                If bFrontPortion Then
                    sRetVal = strValue.Substring(0, strValue.IndexOf(strDelimiter))
                Else
                    sRetVal = strValue.Substring(strValue.IndexOf(strDelimiter) + Len(strDelimiter))
                End If
            Catch ex As Exception

            End Try

            Return sRetVal
        End Function

#Region "Get Requested Details"

        '****************************************************************************************************
        'Function Name  : fncGetRequested
        'Purpose        : Get Requested Details
        'Arguments      : File Extension
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Public Function fncGetRequested(ByVal strRequest As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngGroupId As Long, ByVal strFileType As String, Optional ByVal strFormat As String = "", Optional ByVal intBankID As Integer = 0) As System.Data.DataSet


            'Create Instance of SQL Data Adapter
            Dim sdaFileUpload As SqlDataAdapter

            'Create Instance of System Data Set
            Dim dsFileUpload As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Delcarations
            Dim strSQL As String

            Try

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'If Not (strRequest = gc_UCBankFormat AndAlso intBankID = 0) Then
                'SQL Statement
                strSQL = "Exec pg_UploadCommon " & intBankID.ToString & ",'" & strRequest & "'," & lngOrgId & "," & lngGroupId & ",'" & strFileType & "','" & strFormat & "'"

                'Excute SQL Data Adapter
                sdaFileUpload = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaFileUpload.Fill(dsFileUpload, "UPLOAD")

                ' End If

                Return dsFileUpload

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnGetRequested - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()


                'Destroy System Data Set
                dsFileUpload = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaFileUpload = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Get The File Name From The File Path"

        '****************************************************************************************************
        'Procedure Name : fnFileName
        'Purpose        : To get the File Name From the File Path.
        'Arguments      : File Path
        'Return Value   : File Name
        'Author         : Sujith Sharatchandran - 
        'Created        : 01/11/2003
        '*****************************************************************************************************
        Public Function fncFileName(ByVal strFilePath As String, ByVal IsExtension As Boolean) As String

            Dim strFileName As String = "", arrFile() As String

            Try

                'If Request For File Name
                If Not IsExtension Then
                    arrFile = Split(strFilePath, "\")
                    'If Request For File Extension
                Else
                    arrFile = Split(strFilePath, ".")
                End If
                If arrFile.Length > 0 Then
                    strFileName = arrFile(UBound(arrFile))
                End If
                'If UBound(arrFile) > 0 Then
                '    strFileName = arrFile(UBound(arrFile))
                'End If

                Return strFileName

            Catch

                Return gc_Status_Error

            End Try

        End Function

#End Region

#Region "Delete Records For Failed Trans"

        '****************************************************************************************************
        'Procedure Name : prcDeleteTrans
        'Purpose        : To Delete Records From The Temporary Table if Transactions Failed
        'Arguments      : File Id
        'Return Value   : Body Content
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/11/2003
        '*****************************************************************************************************
        Public Sub prcDeleteTrans(ByVal lngFileId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                    ByVal strTableName As String)

            'Create Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQLStatement As String
            Dim strFileType As String = ""

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Build SQL Statement
                'strSQLStatement = "Exec pg_DelFailedTrans " & lngFileId & ",'" & strFileType & "'"
                strSQLStatement = "If exists (SELECT * FROM dbo.sysobjects WHERE ID = object_id(N'[dbo].[" & strTableName & "]') AND " & _
                 " OBJECTPROPERTY(ID, N'IsUserTable') = 1) " & _
                " BEGIN "
                If strTableName.ToLower = "tcor_mandatesdetails" Then
                    strSQLStatement += " DELETE FROM " & strTableName & " WHERE [FileId] = " & lngFileId & _
                                                      " DELETE FROM tPgt_FileDetails WHERE FileId =" & lngFileId & _
                                                      " END"
                Else
                    strSQLStatement += " DELETE FROM " & strTableName & " WHERE [File Id] = " & lngFileId & _
                                  " DELETE FROM tPgt_FileDetails WHERE FileId =" & lngFileId & _
                                  " END"
                End If


                'Excute SQL Command Object
                Dim cmdFailedTrans As New SqlCommand(strSQLStatement, clsGeneric.SQLConnection)
                cmdFailedTrans.ExecuteNonQuery()

                'Destroy SQL Command Instance
                cmdFailedTrans = Nothing

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prDeleteTrans - ClsUpload", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Update WorkFlow Table"

        '****************************************************************************************************
        'Procedure Name : prDB_WorkFlow
        'Purpose        : Insert WorfFlow Details
        'Arguments      : File Id,Organisation Id,User Id,Action,Reason,IP Address
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Sub prcWorkFlow(ByVal lngFileId As Long, ByVal lngFlowId As Long, ByVal lngOrgId As Long, _
                                    ByVal lngUserId As Long, ByVal strAction As String, ByVal strReject As String, _
                                        ByVal strReason As String, ByVal strIPAddr As String, ByVal dcLimit As Decimal, _
                                            ByVal lngGroupId As Long, ByVal strActionDone As String)

            'Create Instance of SQL Command Objec
            Dim cmdWorkFlow As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic


            Try

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Insert Data - Start
                With cmdWorkFlow
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_WorkFlow"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_FlowId", lngFlowId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_Reject", strReject))
                    .Parameters.Add(New SqlParameter("@in_Reason", strReason))
                    .Parameters.Add(New SqlParameter("@in_UserIP", strIPAddr))
                    .Parameters.Add(New SqlParameter("@in_Limit", dcLimit))
                    .Parameters.Add(New SqlParameter("@in_ActionDone", strActionDone))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .ExecuteNonQuery()
                End With
                'Insert Data - Stop

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcWorkFlow - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdWorkFlow = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Duplicate Updation"

        '****************************************************************************************************
        'Procedure Name : prDuplicate
        'Purpose        : To Update Duplicate Account No With Same File Id 
        'Arguments      : File Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/01/2004
        '*****************************************************************************************************
        Public Sub prcDuplicate(ByVal lngFileId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                        ByVal strFileType As String)

            'Create Instance of System Data Row
            Dim drOriginal As System.Data.DataRow

            'Create Instance of System Data Row
            Dim drDuplicate As System.Data.DataRow

            'Create Instance of System Data Set
            Dim dsDuplicate As New System.Data.DataSet

            'Create Instance of Download Class Object
            Dim clsDownload As New MaxPayroll.clsDownload

            'Variable Declarations
            Dim intRecordCount As Int32, strField As String = "", strChkAccountNo As String
            Dim strAccountNo As String, strSQL As String, intStartPos As Int16, intEndPos As Int16

            Try

                'Populate Data Set
                dsDuplicate = fncGetRequested("TEMP TABLE", lngFileId, lngUserId, 0, "")

                'Get Account Number Column Name
                Call clsDownload.fnFieldDesc("AN", strField, intStartPos, intEndPos)

                'If Records Available
                If dsDuplicate.Tables("UPLOAD").Rows.Count > 0 Then
                    'Read Thro The Data Set Using Data Row - Start
                    For Each drOriginal In dsDuplicate.Tables("UPLOAD").Rows
                        intRecordCount = 0  'initialise counter
                        strChkAccountNo = drOriginal(strField)  'get account number
                        'loop thro to find out if account number duplicated - start
                        For Each drDuplicate In dsDuplicate.Tables("UPLOAD").Rows
                            strAccountNo = drDuplicate(strField)
                            If strChkAccountNo = strAccountNo Then
                                'Increment Record Count
                                intRecordCount = intRecordCount + 1
                            End If
                        Next
                        'loop thro to find out if account number duplicated - stop
                        'if account number duplicated.
                        If intRecordCount > 1 Then
                            'Build SQL Statement
                            strSQL = "Update tTemp_FileBody Set Duplicate = 'Y' Where [File Id] = " & lngFileId & " And [" & strField & "] = '" & strChkAccountNo & "'"
                            'Update Duplicate Account No
                            Call fncDuplicate(lngOrgId, lngUserId, strSQL)
                        End If
                    Next
                    'Read Thro The Data Set Using Data Row - Stop
                End If

            Catch ex As Exception

            Finally

                'Destroy Instance of System Data Row
                drDuplicate = Nothing

                'Destroy Instance of System Data Set
                dsDuplicate = Nothing

                'Destroy Instance of Download Class Object
                clsDownload = Nothing

            End Try

        End Sub

#End Region

#Region "Numeric Check"

        '****************************************************************************************************
        'Function Name  : fncIsNumber()
        'Purpose        : To Check Only Characters
        'Arguments      : Password
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 19/04/2004
        '*****************************************************************************************************
        Public Function fncIsCharacter(ByVal strValue As String) As Boolean

            'Variable Declarations
            Dim intCounter As Int16, IsCharacter As Boolean

            Try

                'Set To True
                IsCharacter = True

                'Loop Thro Entire Password To Check if Number exist - Start
                For intCounter = 1 To Len(strValue)
                    If IsNumeric(Mid(strValue, intCounter, 1)) Then
                        IsCharacter = False
                        Exit For
                    End If
                Next
                'Loop Thro Entire Password To Check if Number exist - Stop

                Return IsCharacter

            Catch ex As Exception

            End Try

        End Function

#End Region

#Region "Track File Details"

        '****************************************************************************************************
        'Function Name  : fncFileDetails
        'Purpose        : Insert File Details
        'Arguments      : File Type,File Name,Organisation ID, User Id
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Function fncFileDetails(ByVal strAction As String, ByVal lngFileId As Long, ByVal strFileType As String, _
            ByVal strGivenName As String, ByVal strFileName As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
            ByVal dtPaymentDate As Date, ByVal strHashTotal As String, ByVal intTotalTrans As Int32, _
            ByVal strAmount As String, ByVal strTotalEmpAmt As String, ByVal strTotalEmprAmt As String, _
            ByVal strConMonth As String, ByVal intGroupId As Int32, ByVal lngAccId As Long, _
            ByVal lngFormatId As Long, Optional ByVal strIC As String = "", _
            Optional ByVal intSerAccId As Int16 = 0, Optional ByVal dcTranCharge As Decimal = 0, _
            Optional ByVal intTotalFileLines As Integer = 0, Optional ByVal intTotalBatchHeader As Integer = 0, _
            Optional ByVal intTotalRecLines As Integer = 0, Optional ByVal strMD5Hash As String = "", Optional ByVal sBankOrgCode As String = "", Optional ByVal strSubFileName As String = "", _
            Optional ByVal strStateCode As String = "", Optional ByVal strContactPerson As String = "", _
            Optional ByVal strContactNumber As String = "", Optional ByVal strFileIndicator As String = "", _
            Optional ByVal strSeqNumber As String = "", Optional ByVal strRefNumber As String = "", _
            Optional ByVal strTestingMode As String = "", Optional ByVal strEmail As String = "", _
            Optional ByVal strEmployerName As String = "") As Long

            'Create Instance of SQL Command Object
            Dim cmdFile As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                If strHashTotal Is Nothing Then
                    strHashTotal = "0"
                End If

                'Insert File Details - Start
                With cmdFile
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_FileDetails"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileName", strFileName))
                    .Parameters.Add(New SqlParameter("@in_GivenName", strGivenName))
                    .Parameters.Add(New SqlParameter("@in_PaymentDate", dtPaymentDate))
                    .Parameters.Add(New SqlParameter("@in_HashTotal", strHashTotal))
                    .Parameters.Add(New SqlParameter("@in_TotalTrans", intTotalTrans))
                    .Parameters.Add(New SqlParameter("@in_TotalAmount", strAmount))
                    .Parameters.Add(New SqlParameter("@in_TotalEmpAmt", strTotalEmpAmt))
                    .Parameters.Add(New SqlParameter("@in_TotalEmrAmt", strTotalEmprAmt))
                    .Parameters.Add(New SqlParameter("@in_ConMonth", strConMonth))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_AccId", lngAccId))
                    .Parameters.Add(New SqlParameter("@in_FormatId", lngFormatId))
                    .Parameters.Add(New SqlParameter("@in_ICCheck", strIC))
                    .Parameters.Add(New SqlParameter("@in_SerAccId", intSerAccId))
                    .Parameters.Add(New SqlParameter("@in_TranCharge", dcTranCharge))
                    .Parameters.Add(New SqlParameter("@in_TotalFileLines ", intTotalFileLines))
                    .Parameters.Add(New SqlParameter("@in_TotalBatchHeader", intTotalBatchHeader))
                    .Parameters.Add(New SqlParameter("@in_TotalRecLines", intTotalRecLines))
                    .Parameters.Add(New SqlParameter("@in_BankOrgCode", sBankOrgCode))
                    .Parameters.Add(New SqlParameter("@in_MD5Hash", strMD5Hash))
                    .Parameters.Add(New SqlParameter("@in_SubFile", strSubFileName))
                    If Not strStateCode = "" Then
                        .Parameters.Add(New SqlParameter("@in_StateCode", strStateCode))
                    End If
                    If Not strContactPerson = "" Then
                        .Parameters.Add(New SqlParameter("@in_ContactPerson", strContactPerson))
                    End If
                    If Not strContactNumber = "" Then
                        .Parameters.Add(New SqlParameter("@in_ContactNumber", strContactNumber))
                    End If
                    If Not strFileIndicator = "" Then
                        .Parameters.Add(New SqlParameter("@in_FileIndicator", strFileIndicator))
                    End If
                    If Not strSeqNumber = "" Then
                        .Parameters.Add(New SqlParameter("@in_SeqNumber", strSeqNumber))
                    End If
                    If Not strRefNumber = "" Then
                        .Parameters.Add(New SqlParameter("@in_RefNumber", strRefNumber))
                    End If
                    If Not strTestingMode = "" Then
                        .Parameters.Add(New SqlParameter("@in_TestingMode", strTestingMode))
                    End If
                    If Not strEmail = "" Then
                        .Parameters.Add(New SqlParameter("@in_Email", strEmail))
                    End If
                    If Not strEmployerName = "" Then
                        .Parameters.Add(New SqlParameter("@in_EmployerName", strEmployerName))
                    End If

                    .Parameters.Add(New SqlParameter("@out_Identity", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Identity", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With
                'Insert File Details - Stop

                'Identity File Id
                lngFileId = IIf(IsNumeric(cmdFile.Parameters("@out_Identity").Value), cmdFile.Parameters("@out_Identity").Value, 0)

                Return lngFileId

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncFileDetails", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdFile = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Track File Details with Batch No. Generation"

        '****************************************************************************************************
        'Function Name  : fncFileDetails
        'Purpose        : Insert File Details
        'Arguments      : File Type,File Name,Organisation ID, User Id
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran, Modified by Marcus Yap
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Function fncFileDetailsBatchNo(ByVal strAction As String, ByVal lngFileId As Long, ByVal strFileType As String, _
            ByVal strGivenName As String, ByVal strFileName As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
            ByVal dtPaymentDate As Date, ByVal strHashTotal As String, ByVal intTotalTrans As Int32, _
            ByVal strAmount As String, ByVal strTotalEmpAmt As String, ByVal strTotalEmprAmt As String, _
            ByVal strConMonth As String, ByVal intGroupId As Int32, ByVal lngAccId As Long, _
            ByVal lngFormatId As Long, Optional ByVal strIC As String = "", _
            Optional ByVal intSerAccId As Int16 = 0, Optional ByVal dcTranCharge As Decimal = 0) As String

            'Create Instance of SQL Command Object
            Dim cmdFile As New SqlCommand
            Dim cmdBatchNumber As New SqlCommand
            Dim trnFile As SqlTransaction
            Dim strBatchNumber As String = ""

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            'Initialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()
            trnFile = clsGeneric.SQLConnection.BeginTransaction()

            Try


                If strHashTotal Is Nothing Then
                    strHashTotal = "0"
                End If

                'Insert File Details - Start
                With cmdFile
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trnFile
                    .CommandText = "pg_FileDetails"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileName", strFileName))
                    .Parameters.Add(New SqlParameter("@in_GivenName", strGivenName))
                    .Parameters.Add(New SqlParameter("@in_PaymentDate", dtPaymentDate))
                    .Parameters.Add(New SqlParameter("@in_HashTotal", strHashTotal))
                    .Parameters.Add(New SqlParameter("@in_TotalTrans", intTotalTrans))
                    .Parameters.Add(New SqlParameter("@in_TotalAmount", strAmount))
                    .Parameters.Add(New SqlParameter("@in_TotalEmpAmt", strTotalEmpAmt))
                    .Parameters.Add(New SqlParameter("@in_TotalEmrAmt", strTotalEmprAmt))
                    .Parameters.Add(New SqlParameter("@in_ConMonth", strConMonth))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_AccId", lngAccId))
                    .Parameters.Add(New SqlParameter("@in_FormatId", lngFormatId))
                    .Parameters.Add(New SqlParameter("@in_ICCheck", strIC))
                    .Parameters.Add(New SqlParameter("@in_SerAccId", intSerAccId))
                    .Parameters.Add(New SqlParameter("@in_TranCharge", dcTranCharge))
                    .Parameters.Add(New SqlParameter("@out_Identity", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Identity", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With
                'Insert File Details - Stop

                'Identity File Id
                lngFileId = IIf(IsNumeric(cmdFile.Parameters("@out_Identity").Value), cmdFile.Parameters("@out_Identity").Value, 0)

                If Not lngFileId = 0 Then
                    With cmdBatchNumber
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnFile
                        .CommandText = "pg_UpdateBatchNumber"
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                        .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                        .Parameters.Add(New SqlParameter("@out_BatchNumber", SqlDbType.VarChar, 50, ParameterDirection.Output, False, 0, 0, "out_BatchNumber", DataRowVersion.Default, ""))
                        .ExecuteNonQuery()
                    End With

                    strBatchNumber = cmdBatchNumber.Parameters("@out_BatchNumber").Value

                End If

                trnFile.Commit()

                If lngFileId = 0 OrElse strBatchNumber = "" Then
                    Return "0"
                Else
                    Return lngFileId.ToString + "," + strBatchNumber
                End If

            Catch

                trnFile.Rollback()
                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncFileDetails", Err.Number, Err.Description)

                Return "0"

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdFile = Nothing
                cmdBatchNumber = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Track File Details with File Reference No. Generation"

        '****************************************************************************************************
        'Function Name  : fncFileDetails
        'Purpose        : Insert File Details
        'Arguments      : File Type,File Name,Organisation ID, User Id
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran, Modified by Marcus Yap
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Function fncFileDetailsRefNo(ByVal strAction As String, ByVal lngFileId As Long, ByVal strFileType As String, _
            ByVal strGivenName As String, ByVal strFileName As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
            ByVal dtPaymentDate As Date, ByVal strHashTotal As String, ByVal intTotalTrans As Int32, _
            ByVal strAmount As String, ByVal strTotalEmpAmt As String, ByVal strTotalEmprAmt As String, _
            ByVal strConMonth As String, ByVal intGroupId As Int32, ByVal lngAccId As Long, _
            ByVal lngFormatId As Long, Optional ByVal strIC As String = "", _
            Optional ByVal intSerAccId As Int16 = 0, Optional ByVal dcTranCharge As Decimal = 0) As String

            'Create Instance of SQL Command Object
            Dim cmdFile As New SqlCommand
            Dim cmdBatchNumber As New SqlCommand
            Dim trnFile As SqlTransaction
            Dim strRefNumber As String = ""

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            'Initialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()
            trnFile = clsGeneric.SQLConnection.BeginTransaction()

            Try


                If strHashTotal Is Nothing Then
                    strHashTotal = "0"
                End If

                'Insert File Details - Start
                With cmdFile
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trnFile
                    .CommandText = "pg_FileDetails"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileName", strFileName))
                    .Parameters.Add(New SqlParameter("@in_GivenName", strGivenName))
                    .Parameters.Add(New SqlParameter("@in_PaymentDate", dtPaymentDate))
                    .Parameters.Add(New SqlParameter("@in_HashTotal", strHashTotal))
                    .Parameters.Add(New SqlParameter("@in_TotalTrans", intTotalTrans))
                    .Parameters.Add(New SqlParameter("@in_TotalAmount", strAmount))
                    .Parameters.Add(New SqlParameter("@in_TotalEmpAmt", strTotalEmpAmt))
                    .Parameters.Add(New SqlParameter("@in_TotalEmrAmt", strTotalEmprAmt))
                    .Parameters.Add(New SqlParameter("@in_ConMonth", strConMonth))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_AccId", lngAccId))
                    .Parameters.Add(New SqlParameter("@in_FormatId", lngFormatId))
                    .Parameters.Add(New SqlParameter("@in_ICCheck", strIC))
                    .Parameters.Add(New SqlParameter("@in_SerAccId", intSerAccId))
                    .Parameters.Add(New SqlParameter("@in_TranCharge", dcTranCharge))
                    .Parameters.Add(New SqlParameter("@out_Identity", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Identity", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With
                'Insert File Details - Stop

                'Identity File Id
                lngFileId = IIf(IsNumeric(cmdFile.Parameters("@out_Identity").Value), cmdFile.Parameters("@out_Identity").Value, 0)

                If Not lngFileId = 0 Then
                    With cmdBatchNumber
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnFile
                        .CommandText = "pg_UpdateFileRefNumber"
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                        .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                        .Parameters.Add(New SqlParameter("@out_RefNumber", SqlDbType.Int, 50, ParameterDirection.Output, False, 0, 0, "out_RefNumber", DataRowVersion.Default, ""))
                        .ExecuteNonQuery()
                    End With

                    strRefNumber = cmdBatchNumber.Parameters("@out_RefNumber").Value

                End If

                trnFile.Commit()

                If lngFileId = 0 OrElse strRefNumber = "" Then
                    Return "0"
                Else
                    Return lngFileId.ToString + "," + strRefNumber
                End If

            Catch

                trnFile.Rollback()
                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncFileDetails", Err.Number, Err.Description)

                Return "0"

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdFile = Nothing
                cmdBatchNumber = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Track File Details with EPF Sequence No. Generation"

        '****************************************************************************************************
        'Function Name  : fncFileDetails
        'Purpose        : Insert File Details
        'Arguments      : File Type,File Name,Organisation ID, User Id
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran, Modified by Marcus Yap
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Function fncFileDetailsSeqNo(ByVal strAction As String, ByVal lngFileId As Long, ByVal strFileType As String, _
            ByVal strGivenName As String, ByVal strFileName As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
            ByVal dtPaymentDate As Date, ByVal strHashTotal As String, ByVal intTotalTrans As Int32, _
            ByVal strAmount As String, ByVal strTotalEmpAmt As String, ByVal strTotalEmprAmt As String, _
            ByVal strConMonth As String, ByVal intGroupId As Int32, ByVal lngAccId As Long, _
            ByVal lngFormatId As Long, Optional ByVal strIC As String = "", _
            Optional ByVal intSerAccId As Int16 = 0, Optional ByVal dcTranCharge As Decimal = 0, _
            Optional ByVal strAdditionFileType As String = "") As String

            'Create Instance of SQL Command Object
            Dim cmdFile As New SqlCommand
            Dim cmdSeqNumber As New SqlCommand
            Dim cmdFileSeqNumber As New SqlCommand
            Dim trnFile As SqlTransaction
            Dim strSeqNumber As String = ""
            Dim strFileSeqNumber As String = ""

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            'Initialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()
            trnFile = clsGeneric.SQLConnection.BeginTransaction()

            Try


                If strHashTotal Is Nothing Then
                    strHashTotal = "0"
                End If

                'Insert File Details - Start
                With cmdFile
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trnFile
                    .CommandText = "pg_FileDetails"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileName", strFileName))
                    .Parameters.Add(New SqlParameter("@in_GivenName", strGivenName))
                    .Parameters.Add(New SqlParameter("@in_PaymentDate", dtPaymentDate))
                    .Parameters.Add(New SqlParameter("@in_HashTotal", strHashTotal))
                    .Parameters.Add(New SqlParameter("@in_TotalTrans", intTotalTrans))
                    .Parameters.Add(New SqlParameter("@in_TotalAmount", strAmount))
                    .Parameters.Add(New SqlParameter("@in_TotalEmpAmt", strTotalEmpAmt))
                    .Parameters.Add(New SqlParameter("@in_TotalEmrAmt", strTotalEmprAmt))
                    .Parameters.Add(New SqlParameter("@in_ConMonth", strConMonth))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_AccId", lngAccId))
                    .Parameters.Add(New SqlParameter("@in_FormatId", lngFormatId))
                    .Parameters.Add(New SqlParameter("@in_ICCheck", strIC))
                    .Parameters.Add(New SqlParameter("@in_SerAccId", intSerAccId))
                    .Parameters.Add(New SqlParameter("@in_TranCharge", dcTranCharge))
                    .Parameters.Add(New SqlParameter("@out_Identity", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Identity", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With
                'Insert File Details - Stop

                'Identity File Id
                lngFileId = IIf(IsNumeric(cmdFile.Parameters("@out_Identity").Value), cmdFile.Parameters("@out_Identity").Value, 0)

                If Not lngFileId = 0 Then
                    With cmdSeqNumber
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnFile
                        .CommandText = "pg_UpdateSequenceNumber"
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                        .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                        .Parameters.Add(New SqlParameter("@in_ConMonth", strConMonth))
                        .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                        .Parameters.Add(New SqlParameter("@out_SequenceNumber", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_SequenceNumber", DataRowVersion.Default, ""))
                        .ExecuteNonQuery()
                    End With

                    strSeqNumber = cmdSeqNumber.Parameters("@out_SequenceNumber").Value

                    With cmdFileSeqNumber
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnFile
                        .CommandText = "pg_GetBankFileSeqNo"
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                        .Parameters.Add(New SqlParameter("@in_AdditionFileType", strAdditionFileType))
                        .Parameters.Add(New SqlParameter("@out_FileSeqNumber", SqlDbType.VarChar, 0, ParameterDirection.Output, False, 0, 0, "out_FileSeqNumber", DataRowVersion.Default, ""))
                        .ExecuteNonQuery()
                    End With

                    strFileSeqNumber = cmdFileSeqNumber.Parameters("@out_FileSeqNumber").Value

                End If

                trnFile.Commit()

                If lngFileId = 0 OrElse strSeqNumber = "" OrElse strFileSeqNumber = "" Then
                    Return "0"
                Else
                    Return lngFileId.ToString + "," + strSeqNumber + "," + strFileSeqNumber
                End If

            Catch

                trnFile.Rollback()
                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncFileDetails", Err.Number, Err.Description)

                Return "0"

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdFile = Nothing
                cmdSeqNumber = Nothing
                cmdFileSeqNumber = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Put File"

        '****************************************************************************************************
        'Procedure Name : fnPutFile
        'Purpose        : To Put File to the Database Server
        'Arguments      : Server Name Or IP,UserId,Password,Source File  
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/01/2004
        '*****************************************************************************************************
        Public Function fncPutFile(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSourceFile As String, _
                ByVal strDestinationFile As String) As Boolean

            Dim clsFileTransfer As New MaxFTP.clsFTP                    'Create Instance of File Transfer Object
            Dim clsGeneric As New MaxPayroll.Generic                    'Create Instance of Generic Class Object
            Dim clsEncryption As New MaxPayroll.Encryption              'Create Instance of encryption Class Object

            'Variable Declarations
            Dim IsPutFile As Boolean, strServerName As String, strUserName As String, strPassword As String

            Try

                'strDestination = fncFileName(strSourceFile, False)                                     'Get Only File Name
                strServerName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPIP"))    'Get Server Name
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPUID"))     'Get User Name
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPPWD"))     'Get Password

                'Put File to the FTP Server
                IsPutFile = clsFileTransfer.fncPutFile(strServerName, strUserName, strPassword, strSourceFile, _
                                strDestinationFile)

                Return IsPutFile

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCommon - fncPutFile", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of File Transfer Object
                clsFileTransfer = Nothing

                'Destroy Instance of Common Class Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Get File"

        '****************************************************************************************************
        'Procedure Name : fnGetFile
        'Purpose        : To get File From the Database Server
        'Arguments      : Server Name Or IP,UserId,Password,Source File  
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/01/2004
        '*****************************************************************************************************
        Public Function fncGetFile(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSourceFile As String, _
                    ByVal strDestinationFile As String) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of File Transfer Object
            Dim clsFileTransfer As New MaxFTP.clsFTP

            'Create Instance of encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim IsGetFile As Boolean, strServerName As String, strUserName As String, strPassword As String

            Try

                strServerName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPIP"))    'Get Server Name
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPUID"))     'Get User Name
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPPWD"))     'Get Password

                'Put File to the Database Server
                IsGetFile = clsFileTransfer.fncGetFile(strServerName, strUserName, strPassword, _
                                strSourceFile, strDestinationFile)

                Return IsGetFile

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCommon - fncGetFile", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of File Transfer Object
                clsFileTransfer = Nothing

                'Destroy Instance of Common Class Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Update Duplicate Account No"

        '****************************************************************************************************
        'Procedure Name : fnDuplicate
        'Purpose        : To Return the Count of Account Numbers 
        'Arguments      : File Id
        'Return Value   : Count
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/01/2004
        '*****************************************************************************************************
        Private Sub fncDuplicate(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSQL As String)

            'Create Instance of SQL Command Object
            Dim cmdDuplicate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute The SQL Command Object - Start
                With cmdDuplicate
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    .ExecuteNonQuery()
                End With
                'Execute The SQL Command Object - Stop

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncDuplicate - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdDuplicate = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Insert Transaction Records"

        '****************************************************************************************************
        'Procedure Name : prcTranRecords
        'Purpose        : To Insert Transaction Records
        'Arguments      : File Id, Table Name, Fields, Values
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/08/2004
        'Modify By      : Victor Wong
        'Modify Date    : 2007-02-28
        '*****************************************************************************************************
        Public Function prcTranRecords(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngFileId As Long, ByVal strFields As String, ByVal strValues As String, ByVal strTableName As String, ByVal intSerial As Int16) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdTranRecords As New SqlCommand

            'Variable Declarations
            Dim strSQL As String

            Dim bRetVal As Boolean = False

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                strSQL = "INSERT INTO [" & strTableName & "] ([Rec Id],[File Id]," & strFields & ",[Duplicate],[Status]) Values (" & intSerial & "," & lngFileId & "," & strValues & ",'N',99)"

                With cmdTranRecords
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = strSQL
                    .ExecuteNonQuery()
                End With

                bRetVal = True

            Catch

                'Log error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsUpload - prcTranRecords", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Detsroy Instance of SQL Command Object
                cmdTranRecords = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

            Return bRetVal
        End Function

#End Region

#Region "Display File List"

        '****************************************************************************************************
        'Function Name  : fnFileGrid
        'Purpose        : Build The File List Grid
        'Arguments      : Action,Organisation Id,User ID
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Public Function fncFileGrid(ByVal strAction As String, ByVal lngOrgId As Long, _
                            ByVal lngUserId As Long, ByVal lngGroupId As Long) As System.Data.DataSet

            'Create Instance of SQL Data Adaptor
            Dim sdaFile As New SqlDataAdapter

            'Create Instance of System Data Set
            Dim dsFile As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQLStatement As String

            Try

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statment
                strSQLStatement = "Exec pg_FileList " & lngOrgId & "," & lngUserId & "," & lngGroupId & ",'" & strAction & "'"

                'Execute SQL Data Adaptor
                sdaFile = New SqlDataAdapter(strSQLStatement, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaFile.Fill(dsFile, "FILE")

                Return dsFile

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnFileGrid - clsUpload", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaFile = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Final Submission"

        '****************************************************************************************************
        'Procedure Name : fnFinal
        'Purpose        : To know if the Final Submission for the Payroll File
        'Arguments      : File Id 
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/12/2003
        '*****************************************************************************************************
        Public Function fncFinal(ByVal lngFileId As Long, ByVal lngUserId As Long, _
                            ByVal lngOrgId As Long, ByVal lngGroupId As Long) As Boolean

            'Create Instance of SQL Data Reader
            Dim cmdFinal As New SqlCommand

            'Create Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intCount As Int16

            Try

                'Intilise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdFinal
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_GetFlowStatus"
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .Parameters.Add(New SqlParameter("@out_Count", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Count", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                intCount = cmdFinal.Parameters("@out_Count").Value

                Return intCount

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCommon-fncFinal", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Update Remarks"

        '****************************************************************************************************
        'Function Name  : fnGetValueDate
        'Purpose        : To Get the Value Date For the Given Month,Year and Organisation Id
        'Arguments      : Organisation Id,Month,Year
        'Return Value   : Day
        'Author         : Sujith Sharatchandran - 
        'Created        : 09/11/2003
        '*****************************************************************************************************
        Public Sub fncRemarks(ByVal lngFileId As Long, ByVal strRemark As String)

            'Instance of SQL Command Object
            Dim cmdRemarks As New SqlCommand

            'Instance of Generic Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Instance of ASPNetContext Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngUserId As Long, lngOrgId As Long, strRemarks As String

            Try

                If strRemark = "" Then
                    strRemarks = ASPNetContext.Request.Form("ctl00$cphContent$txtReason")
                Else
                    strRemarks = strRemark
                End If

                lngOrgId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)
                lngUserId = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                Call clsGeneric.SQLConnection_Initialize()

                With cmdRemarks
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_Remarks"
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Remarks", strRemarks))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCommon-fncRemarks", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Data Command
                cmdRemarks = Nothing

                'Destroy Instance of ASPNetContext
                ASPNetContext = Nothing

            End Try

        End Sub

#End Region

#Region "Delete Rejected File"

        '****************************************************************************************************
        'Procedure Name : fnDeleteFile
        'Purpose        : To Delete The Customer File Which Has Been Deleted
        'Arguments      : File Name 
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/12/2003
        '*****************************************************************************************************
        Public Function fncDeleteFile(ByVal lngFileId As Long, ByVal strFileType As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal IsFileName As Boolean) As String

            'Create Instance of SQL Data Reader
            Dim sdrGetFile As SqlDataReader

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strFilePath As String, strSQL As String, strGivenFile As String = "", strCreateFile As String = ""

            Try
                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Stored Proc
                strSQL = "Exec pg_GetFile " & lngFileId

                'Execute Stored Proc via Command Object
                Dim cmdFile As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                sdrGetFile = cmdFile.ExecuteReader(CommandBehavior.CloseConnection)

                If sdrGetFile.HasRows Then
                    sdrGetFile.Read()
                    strGivenFile = sdrGetFile("GivenName")
                    strCreateFile = sdrGetFile("CreateName")
                    sdrGetFile.Close()
                Else
                    sdrGetFile.Close()
                End If

                'Destroy Command Object
                cmdFile = Nothing

                'Get File Path for Bank File
                strFilePath = fncFolder(strFileType, "Created", lngOrgId, 0)

                If Not IsFileName Then
                    'Check If File Exsists
                    If File.Exists(strFilePath & "\" & strCreateFile) Then
                        'Delete File Bank File
                        File.Delete(strFilePath & "\" & strCreateFile)
                    End If

                    'Get File Path for Customer File
                    strFilePath = fncFolder(strFileType, "Uploads", lngOrgId, 0)

                    'Check If File Exsists
                    If File.Exists(strFilePath & "\" & strGivenFile) Then
                        'Delete File Customer File
                        File.Delete(strFilePath & "\" & strGivenFile)
                    End If
                End If

                'Return File Path With File
                Return strFilePath & "\" & strCreateFile

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnDeleteFile", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Data Reader
                sdrGetFile = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Delete FTP File"

        '****************************************************************************************************
        'Procedure Name : fnDelFile
        'Purpose        : To Put File to the Database Server
        'Arguments      : Server Name Or IP,UserId,Password,Source File  
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/01/2004
        '*****************************************************************************************************
        Public Function fncDelFile(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSourceFile As String) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of File Transfer Object
            Dim clsFileTransfer As New MaxFTP.clsFTP

            'Create Instance of encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim IsDeleteFile As Boolean, strServerName As String, strUserName As String, strPassword As String

            Try

                strServerName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPIP"))    'Get Server Name
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPUID"))     'Get User Name
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("FTPPWD"))     'Get Password

                'Put File to the Database Server
                IsDeleteFile = clsFileTransfer.fncDeleteFile(strServerName, strUserName, strPassword, strSourceFile)

                Return IsDeleteFile

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCommon - fncDelFile", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of File Transfer Object
                clsFileTransfer = Nothing

                'Destroy Instance of Common Class Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Delete Records"

        '****************************************************************************************************
        'Procedure Name : fnDeleteSubmitted
        'Purpose        : To Delete records which have been submitted to the Bank Server
        'Arguments      : File Id 
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/12/2003
        '*****************************************************************************************************
        Public Function fncDeleteSubmitted(ByVal lngFileId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String

            Try

                'Initialize SQL connection
                Call clsGeneric.SQLConnection_Initialize()

                strSQL = "Exec pg_DeleteSubmitted " & lngFileId
                Dim cmdDelete As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                cmdDelete.ExecuteNonQuery()

                'Destory Command Object
                cmdDelete = Nothing

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnDeleteSubmitted", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Create/Get Folder"

        '****************************************************************************************************
        'Function Name  : fnFolder
        'Purpose        : Create/Get Folder
        'Arguments      : NA
        'Return Value   : Folder Path
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Public Function fncFolder(ByVal strFileType As String, ByVal strAction As String, ByVal lngOrgId As Long, ByVal lngUserId As Long) As String

            'Create Instance of Generic Class object    
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strRootFolder As String = "", strPath As String

            Try

                'Get Path
                strPath = AppSettings("PATH") ''change


                'Set Root Folder - START
                Select Case strFileType

                    Case "Billing File"
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("BILLUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("BILLCREATED")
                        ElseIf strAction = "CRYPT" Then
                            strRootFolder = strPath & AppSettings("CRYPTFOLDER")
                        End If
                    Case _Helper.DirectDebit_Name
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("DDEBITUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("DDEBITCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("DDEBITAPPROVED")
                            'Hafeez add'

                            'Hafeez end'
                        End If
                    Case "Payroll File"
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("PAYUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("PAYCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("PAYAPPROVED")
                        End If
                    Case _Helper.Autopay_Name
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("AUTOPAYUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("AUTOPAYCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("AUTOPAYAPPROVED")
                        End If
                    Case _Helper.CPS_Name
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("CPSUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("CPSCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("CPSAPPROVED")
                        End If
                    Case _Helper.AutopaySNA_Name
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("AUTOPAYSNAUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("AUTOPAYSNACREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("AUTOPAYSNAAPPROVED")
                        End If
                    Case _Helper.MandateFile_Name
                        strRootFolder = strPath & AppSettings("MANDATESUPLOADED")

                    Case _Helper.PayLinkPayRoll_Name
                        If strAction = "UPLOAD" Then
                            strRootFolder = strPath & AppSettings("PAYLINKUPLOADED")
                        ElseIf strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("PAYLINKCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("PAYLINKAPPROVED")
                        End If
                    Case _Helper.HybridDirectDebit_Name
                        If strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("HDDCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("HDDAPPROVED")
                        End If
                    Case _Helper.HybridAutoPaySNA_Name
                        If strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("HAPSCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("HAPSAPPROVED")
                        End If
                    Case _Helper.HybridMandate_Name
                        If strAction = "CREATE" Then
                            strRootFolder = strPath & AppSettings("HMNDCREATED")
                        ElseIf strAction = "Approved" Then
                            strRootFolder = strPath & AppSettings("HMNDAPPROVED")
                        End If
                End Select

                'Set Root Folder - STOP

                'Check If Year Folder Exists - START
                If Not System.IO.Directory.Exists(strRootFolder) Then
                    System.IO.Directory.CreateDirectory(strRootFolder)
                End If
                'Check If Year Folder Exists - STOP

                Return strRootFolder

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnFolder - clsUpload", Err.Number, Err.Description)

                Return gc_Status_Error

            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

        Public Function fncFolder(ByVal strFileType As String, ByVal strAction As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal bIsMultiBank As Boolean) As String

            'Create Instance of Generic Class object    
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strRootFolder As String = "", strPath As String

            Try

                'Get Path
                strPath = System.Configuration.ConfigurationManager.AppSettings("PATH")

                'Set Root Folder - START
                If strFileType = "Payroll File" OrElse bIsMultiBank Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("PAYUPLOADED")
                    ElseIf strAction = "CREATE" Then
                        strRootFolder = strPath & AppSettings("PAYCREATED")
                    ElseIf strAction = "Approved" Then
                        strRootFolder = strPath & AppSettings("PAYAPPROVED")
                    End If
                ElseIf strFileType = _Helper.MandateFile_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("MANDATESUPLOADED")
                    End If
                ElseIf strFileType = "Billing File" Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("BILLUPLOADED")
                    ElseIf strAction = "CREATE" Then
                        strRootFolder = strPath & AppSettings("BILLCREATED")
                    ElseIf strAction = "CRYPT" Then
                        strRootFolder = strPath & AppSettings("CRYPTFOLDER")
                    End If
                ElseIf strFileType = _Helper.DirectDebit_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("DDEBITUPLOADED")
                    ElseIf strAction = "CREATE" Then
                        strRootFolder = strPath & AppSettings("DDEBITCREATED")
                    ElseIf strAction = "Approved" Then
                        strRootFolder = strPath & AppSettings("DDEBITAPPROVED")
                    End If
                ElseIf strFileType = _Helper.Autopay_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("AUTOPAYUPLOADED")
                    ElseIf strAction = "CREATE" Then
                        strRootFolder = strPath & AppSettings("AUTOPAYCREATED")
                    ElseIf strAction = "Approved" Then
                        strRootFolder = strPath & AppSettings("AUTOPAYAPPROVED")
                    End If
                ElseIf strFileType = _Helper.CPS_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("CPSUPLOADED")
                    End If

                ElseIf strFileType = _Helper.AutopaySNA_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("AUTOPAYSNAUPLOADED")
                    ElseIf strAction = "CREATE" Then
                        strRootFolder = strPath & AppSettings("AUTOPAYSNACREATED")
                    ElseIf strAction = "Approved" Then
                        strRootFolder = strPath & AppSettings("AUTOPAYSNAAPPROVED")
                    End If

                ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("PAYLINKUPLOADED")
                    ElseIf strAction = "CREATE" Then
                        strRootFolder = strPath & AppSettings("PAYLINKCREATED")
                    ElseIf strAction = "Approved" Then
                        strRootFolder = strPath & AppSettings("PAYLINKAPPROVED")
                    End If
                ElseIf strFileType = _Helper.HybridDirectDebit_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("HDDUPLOADED")
                    End If
                ElseIf strFileType = _Helper.HybridAutoPaySNA_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("HAPSUPLOADED")
                    End If
                ElseIf strFileType = _Helper.HybridMandate_Name Then
                    If strAction = "UPLOAD" Then
                        strRootFolder = strPath & AppSettings("HMNDUPLOADED")
                    End If
                End If
                'Set Root Folder - STOP

                'Check If Year Folder Exists - START
                If Not System.IO.Directory.Exists(strRootFolder) Then
                    System.IO.Directory.CreateDirectory(strRootFolder)
                End If
                'Check If Year Folder Exists - STOP

                Return strRootFolder

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnFolder - clsUpload", Err.Number, Err.Description)

                Return gc_Status_Error

            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Save File"

        '****************************************************************************************************
        'Function Name  : fnSaveFile
        'Purpose        : Save File to Specified Folder
        'Arguments      : File Name
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Public Function fncSaveFile(ByVal strFolderType As String, ByVal strFileType As String, ByVal lngOrgId As Long) As String

            'Variable Declarations
            Dim strFolder As String

            'Get File Name
            strFolder = fncFolder(strFileType, strFolderType, lngOrgId, 0)

            'Return Folder Path
            Return strFolder

        End Function

#End Region

#Region "Block File"

        '****************************************************************************************************
        'Procedure Name : prBlockFile
        'Purpose        : To Block The File Form Being Submitted 
        'Arguments      : File Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/02/2004
        '*****************************************************************************************************
        Public Sub prcBlockFile(ByVal lngFileId As Long, ByVal intBlockCode As Int16, ByVal lngOrgId As Long, _
                        ByVal lngUserId As Long)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdBlockFile As New SqlCommand

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Stored Procedure
                strSQL = "Exec pg_BlockFile " & lngFileId & "," & intBlockCode & "," & lngUserId

                'Execute SQL Command - Start
                cmdBlockFile.Connection = clsGeneric.SQLConnection
                cmdBlockFile.CommandType = CommandType.Text
                cmdBlockFile.CommandText = strSQL
                cmdBlockFile.ExecuteNonQuery()
                'Execute SQL Command - Stop

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prBlockFile - clsUpload", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdBlockFile = Nothing

            End Try

        End Sub

#End Region

#Region "Load Remarks"

        '****************************************************************************************************
        'Procedure Name : fnListRemarks
        'Purpose        : To Load The Remarks
        'Arguments      : File Id 
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 07/12/2003
        '*****************************************************************************************************
        Public Function fncListRemarks(ByVal lngFileId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Instance of SQL Data Adaptor
            Dim sdaRemarks As New SqlDataAdapter

            'Instance of Data Set
            Dim dsRemarks As New System.Data.DataSet

            'Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQl Statement
                strSQL = "Exec pg_ListRemarks " & lngFileId & "," & lngOrgId

                'Execute SQL Data Adaptor
                sdaRemarks = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaRemarks.Fill(dsRemarks, "REMARKS")

                Return dsRemarks

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnListRemarks - clsUpload", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Data Set
                dsRemarks = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaRemarks = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "File Count"

        '****************************************************************************************************
        'Function Name  : fncFileCount
        'Purpose        : To get the EPF File Count for the Value Month and Value Year
        'Arguments      : Organisation Id,User Id,Value Month,Value Year,File Type,File Status
        'Return Value   : Integer
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/12/2004
        '*****************************************************************************************************
        Public Function fncFileCount(ByVal strAccNo As String, ByVal lngUserId As Long, _
                    ByVal intMonth As Int16, ByVal intYear As Int16, ByVal strFileType As String, _
                            ByVal intStatus As Int16) As Int16

            'Create Instance of SQL Command Object
            Dim cmdFileCount As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strConMonthYear As String, intCount As String

            Try

                'Initialize SQL connection
                Call clsGeneric.SQLConnection_Initialize()

                'build contribution month year
                strConMonthYear = MonthName(intMonth, True) & " " & intYear

                With cmdFileCount
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_FileCount"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_AccNo", strAccNo))
                    .Parameters.Add(New SqlParameter("@in_MonthYear", strConMonthYear))
                    .Parameters.Add(New SqlParameter("@in_Status", intStatus))
                    intCount = .ExecuteScalar
                End With

                Return intCount

            Catch

                'Log Error
                clsGeneric.ErrorLog(0, lngUserId, "fncFileCount - clsCommon", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdFileCount = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Put Decimal"

        '****************************************************************************************************
        'Function Name  : fncDecimal
        'Purpose        : Convert to Decimal Value
        'Arguments      : Number/Money
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 24/08/2004
        '*****************************************************************************************************
        Public Function fncDecimal(ByVal strNumber As String) As String

            Dim dblAmount As Double

            Try

                dblAmount = IIf(IsNumeric(strNumber), strNumber, 0)

                If dblAmount > 0 Then
                    Return Format(dblAmount, "##,##0.00")
                Else
                    Return strNumber
                End If

            Catch ex As Exception

            End Try
            Return ""
        End Function

#End Region

#Region "Upload Business Rules"

        '****************************************************************************************************
        'Function Name  : fncBusinessRules()
        'Purpose        : To Check Business Rules While Upload
        'Arguments      : Option, Organisation Id, User Id,Group Id,Limit 
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/02/2005
        '*****************************************************************************************************
        Public Function fncBusinessRules(ByVal strOption As String, ByVal lngOrgId As Long, _
             ByVal lngUserId As Long, ByVal intGroupId As Int32, ByVal dcLimit As Decimal) As Int32

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdBusiRules As New SqlCommand

            'Variable Declarations
            Dim intBusiResult As Int32

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdBusiRules
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_BusinessRule"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Limit", dcLimit))
                    .Parameters.Add(New SqlParameter("@out_Result", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Result", DataRowVersion.Default, 0))
                    .ExecuteNonQuery()
                End With

                'Get Result
                intBusiResult = IIf(IsNumeric(cmdBusiRules.Parameters("@out_Result").Value), cmdBusiRules.Parameters("@out_Result").Value, 0)

                Return intBusiResult

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBusinessRules - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Delete Rejected File by FTP"

        '****************************************************************************************************
        'Procedure Name : prcDelFile()
        'Purpose        : To Delete File to the Database Server
        'Arguments      : File Name 
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/01/2004
        '*****************************************************************************************************
        Public Sub prcDelFile(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String, _
                ByVal strFileName As String)

            Try

                If Not (strFileType = "Payroll File" Or strFileType = "Multiple Bank") Then
                    strFileName = Replace(strFileName, ".", "_")
                    strFileName = strFileName & ".cry"
                End If

                'Delete File
                Call fncDelFile(lngOrgId, lngUserId, "CREATED\" & strFileName)

                If (strFileType = "EPF File" Or strFileType = "EPF Test File") Then
                    'Delete File
                    Call fncDelFile(lngOrgId, lngUserId, "EPFIN\" & strFileName)
                ElseIf strFileType = "SOCSO File" Then
                    'Delete File
                    Call fncDelFile(lngOrgId, lngUserId, "SOCSOIN\" & strFileName)
                ElseIf strFileType = "LHDN File" Then
                    'Delete File
                    Call fncDelFile(lngOrgId, lngUserId, "LHDNIN\" & strFileName)
                Else
                    'Delete File
                    Call fncDelFile(lngOrgId, lngUserId, "IN\" & strFileName)
                End If

            Catch ex As Exception

            Finally


            End Try

        End Sub

#End Region

#Region "Delete Rejected File"

        '****************************************************************************************************
        'Procedure Name : prcMoveRejectedFile()
        'Purpose        : To Delete File to the Database Server
        'Arguments      : File Name 
        'Return Value   : N/A
        'Modified by    : Marcus Yap
        'Created        : 09/01/2008
        '*****************************************************************************************************
        Public Sub prcMoveRejectedFile(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String, _
                ByVal strFileName As String)

            'Variable Declarion
            Dim strSourceFile As String = Nothing, strBackupFolder As String = Nothing


            If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                strBackupFolder = ConfigurationManager.AppSettings("WebPAYRejectedPath")
            ElseIf (strFileType = "EPF File" Or strFileType = "EPF Test File") Then
                strBackupFolder = ConfigurationManager.AppSettings("WebEPFRejectedPath")
            ElseIf strFileType = "SOCSO File" Then
                strBackupFolder = ConfigurationManager.AppSettings("WebSOCSORejectedPath")
            ElseIf strFileType = "LHDN File" Then
                strBackupFolder = ConfigurationManager.AppSettings("WebLHDNRejectedPath")
            ElseIf strFileType = "Zakat" Then
                strBackupFolder = ConfigurationManager.AppSettings("WebZakatRejectedPath")
            ElseIf strFileType = _Helper.DirectDebit_Name Or strFileType = _Helper.MandateFile_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebDDebitRejectedPath")
            ElseIf strFileType = _Helper.Autopay_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebAutopayRejectedPath")
            ElseIf strFileType = _Helper.AutopaySNA_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebAutopaySNARejectedPath")
            ElseIf strFileType = _Helper.CPS_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebCPSRejectedPath")
            ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebPayLinkRejectedPath")
            ElseIf strFileType = _Helper.HybridDirectDebit_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebHDDRejectedPath")
            ElseIf strFileType = _Helper.HybridAutoPaySNA_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebHAPSRejectedPath")
            ElseIf strFileType = _Helper.HybridMandate_Name Then
                strBackupFolder = ConfigurationManager.AppSettings("WebHMNDRejectedPath")
            End If

            strSourceFile = fncFolder(strFileType, "CREATE", lngOrgId, lngUserId)
            Directory.Move(strSourceFile + "\" + strFileName, strBackupFolder + "\" + strFileName)

            If strFileType = _Helper.CPS_Name Then
                Dim strInvoiceFile As String
                strInvoiceFile = strFileName.Substring(0, strFileName.LastIndexOf(".") - 7) + "invoice.txt"
                If File.Exists(strSourceFile + "\" + strInvoiceFile) Then
                    Directory.Move(strSourceFile + "\" + strInvoiceFile, strBackupFolder + "\" + strInvoiceFile)
                End If
            End If

        End Sub

#End Region

#Region "Submit Authorized File"

        '****************************************************************************************************
        'Procedure Name : fncSubmitFile()
        'Purpose        : To Submit File to IN Folder
        'Arguments      : 
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/03/2005
        '*****************************************************************************************************
        Public Function fncSubmitFile(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String, _
                            ByVal strFileName As String) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strWebIncomingPath As String = ""
            Dim bIsFileExists As Boolean
            Dim IsMoveFile As Boolean
            Dim strSourceFile As String
            Dim clsCommon As New clsCommon


            Try

                Dim sApprovedPath As String = clsCommon.fncFolder(strFileType, "Approved", lngOrgId, lngUserId)
                'Get Download Source Folder
                strSourceFile = fncFolder(strFileType, "CREATE", lngOrgId, lngUserId)

                'If EPF File
                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                    'strFTPPath = "IN"
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebPayIncomingPath")
                ElseIf strFileType = "EPF Test File" Or strFileType = "EPF File" Then
                    'strFTPPath = "EPFIN"
                    'strFileName = Replace(strFileName, ".", "_") & ".cry"
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebEPFIncomingPath")
                ElseIf strFileType = "SOCSO File" Then
                    'strFTPPath = "SOCSOIN"
                    'strFileName = Replace(strFileName, ".", "_") & ".cry"
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebSOCSOIncomingPath")
                ElseIf strFileType = "LHDN File" Then
                    'strFTPPath = "LHDNIN"
                    'strFileName = Replace(strFileName, ".", "_") & ".cry"
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebLHDNIncomingPath")
                ElseIf strFileType = "Zakat" Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebZakatIncomingPath")
                ElseIf strFileType = _Helper.DirectDebit_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebDDEBITIncomingPath")
                ElseIf strFileType = _Helper.Autopay_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebAutoPayIncomingPath")
                ElseIf strFileType = _Helper.CPS_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebCPSIncomingPath")
                ElseIf strFileType = _Helper.AutopaySNA_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebAutopaySNAIncomingPath")
                ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebPayLinkIncomingPath")
                ElseIf strFileType = _Helper.HybridDirectDebit_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebHDDIncomingPath")
                ElseIf strFileType = _Helper.HybridAutoPaySNA_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebHAPScomingPath")
                ElseIf strFileType = _Helper.HybridMandate_Name Then
                    strWebIncomingPath = ConfigurationManager.AppSettings("WebHMNDcomingPath")
                End If

                'Source Folder Plus File Name
                strSourceFile = strSourceFile & "\" & strFileName

                'Download File From Created FTP Folder
                'IsGetFile = fncGetFile(lngOrgId, lngUserId, "CREATED\" & strFileName, strSourceFile)

                bIsFileExists = File.Exists(strSourceFile)

                'If Get File Successful
                If bIsFileExists Then

                    Select Case strFileType
                        Case "SOCSO File", "Zakat"

                            Dim strMasterFile As String() = Directory.GetFiles(strWebIncomingPath)

                            If strMasterFile.Length > 0 Then

                                Dim objFileReader As StreamReader
                                Dim strFileBody As String

                                objFileReader = New StreamReader(strSourceFile, System.Text.Encoding.Default)
                                strFileBody = objFileReader.ReadToEnd()

                                objFileReader.Close()

                                Dim objWriter As New System.IO.StreamWriter(strMasterFile(0), True)

                                'objWriter.Write(vbCrLf)
                                objWriter.Write(strFileBody)

                                objWriter.Close()

                                'Rename the file with the latest submited file name.
                                Directory.Move(strMasterFile(0), strWebIncomingPath + "\" + strFileName)
                                strMasterFile = Nothing
                                IsMoveFile = True

                                'Marcus: Copy the original file to approved folder for backup purpose.
                                clsCommon.prcBackupApprovedFile(strSourceFile, sApprovedPath + "\" + strFileName, lngOrgId, lngUserId)

                                'Directory.Move(strSourceFile, sApprovedPath + "\" + strFileName)

                            Else
                                File.Copy(strSourceFile, strWebIncomingPath + "\" + strFileName)
                                'Directory.Move(strSourceFile, strWebIncomingPath + "\" + strFileName)
                                IsMoveFile = True

                                'Marcus: Copy the original file to approved folder for backup purpose.
                                'Directory.Move(strSourceFile, sApprovedPath + "\" + strFileName)
                                clsCommon.prcBackupApprovedFile(strSourceFile, sApprovedPath + "\" + strFileName, lngOrgId, lngUserId)

                            End If

                        Case "LHDN File"

                            Dim strMasterFile As String() = Directory.GetFiles(strWebIncomingPath)

                            If strMasterFile.Length > 0 Then

                                'Marcus New Method begin here, the above coding is also usable.

                                Dim objFileReader As StreamReader
                                Dim strFileBody As String
                                Dim strMasterFileBody As String
                                Dim lngMasterHashTotal As Long
                                Dim objwriter As StreamWriter


                                Dim objFileStream As New FileStream(strMasterFile(0), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)

                                objFileReader = New StreamReader(strSourceFile, System.Text.Encoding.Default)
                                strFileBody = objFileReader.ReadToEnd()

                                objFileReader.Close()

                                objFileReader = New StreamReader(objFileStream, System.Text.Encoding.Default)
                                objwriter = New System.IO.StreamWriter(objFileStream, System.Text.Encoding.Default)

                                strMasterFileBody = objFileReader.ReadToEnd

                                Dim strtemp As String = Right(strFileBody, 16)
                                Dim strtemp2 As String = vbCrLf.Length

                                lngMasterHashTotal = CLng(Right(strFileBody, 17)) + CLng(Right(strMasterFileBody, 17))
                                'Need to modified to total file header length(get from database) + 2 (vbCrLf)
                                strFileBody = strFileBody.Substring(10)

                                '17 = lenght of hash total + vbCrLf(2)
                                strFileBody = strFileBody.Substring(0, strFileBody.LastIndexOf(Right(strFileBody, 17)))

                                strFileBody += Format(lngMasterHashTotal, "000000000000000") + vbCrLf

                                '18 = length of original trailer record(need to modified to get from database) + last vbCrLf(2)
                                strMasterFileBody = strMasterFileBody.Substring(0, strMasterFileBody.Length - 18)


                                objwriter.BaseStream.Position = 0
                                'objWriter.Write(vbCrLf)
                                objwriter.Write(strMasterFileBody + strFileBody)

                                objwriter.Close()
                                objFileReader.Close()

                                'Rename the file with the latest submited file name.
                                Directory.Move(strMasterFile(0), strWebIncomingPath + "\" + strFileName)
                                IsMoveFile = True

                                strMasterFile = Nothing
                                objFileStream = Nothing

                                'Marcus: Copy the original file to approved folder for backup purpose.
                                'Directory.Move(strSourceFile, sApprovedPath + "\" + strFileName)
                                clsCommon.prcBackupApprovedFile(strSourceFile, sApprovedPath + "\" + strFileName, lngOrgId, lngUserId)

                            Else
                                File.Copy(strSourceFile, strWebIncomingPath + "\" + strFileName)
                                'Directory.Move(strSourceFile, strWebIncomingPath + "\" + strFileName)
                                IsMoveFile = True
                                'Marcus: Copy the original file to approved folder for backup purpose.
                                'Directory.Move(strSourceFile, sApprovedPath + "\" + strFileName)
                                clsCommon.prcBackupApprovedFile(strSourceFile, sApprovedPath + "\" + strFileName, lngOrgId, lngUserId)
                            End If

                        Case "Payroll File"
                            IsMoveFile = True
                        Case _Helper.CPS_Name

                            File.Copy(strSourceFile, strWebIncomingPath + "\" + strFileName)

                            Dim strInvoiceFile As String
                            Dim strInvoiceFileName As String

                            strInvoiceFile = strSourceFile.Substring(0, strSourceFile.LastIndexOf(".") - 7) + "invoice.txt"
                            strInvoiceFileName = strFileName.Substring(0, strFileName.LastIndexOf(".") - 7) + "invoice.txt"
                            If File.Exists(strInvoiceFile) Then
                                File.Copy(strInvoiceFile, strWebIncomingPath + "\" + strInvoiceFileName)
                            End If

                            IsMoveFile = True

                        Case Else

                            'File.Copy(strSourceFile, strWebIncomingPath + "\" + strFileName)
                            File.Copy(strSourceFile, strWebIncomingPath + "\" + strFileName)
                            IsMoveFile = True

                            'Marcus: Copy the original file to approved folder for backup purpose.
                            clsCommon.prcBackupApprovedFile(strSourceFile, sApprovedPath + "\" + strFileName, lngOrgId, lngUserId)
                            'Directory.Move(strSourceFile, sApprovedPath + "\" + strFileName)
                    End Select

                    'Move File From Created Folder to In Folder
                    'IsMoveFile = fncPutFile(lngOrgId, lngUserId, strSourceFile, strFTPPath & "\" & strFileName)

                    'If Move File Successful
                    If IsMoveFile Then

                        'Delete File from Local Folder
                        'System.IO.File.Delete(strSourceFile)

                        'Delete from File Created FTP Folder
                        'Call fncDelFile(lngOrgId, lngUserId, "CREATED\" & strFileName)

                        Return True
                    Else
                        Return False
                    End If

                Else

                    Return False

                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncSubmitFile - clsCommon", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing
                clsCommon = Nothing

            End Try

        End Function

#End Region

#Region "Backup Approved File"
        Private Sub prcBackupApprovedFile(ByVal strSource As String, ByVal strDestination As String, ByVal lngOrgId As Long, ByVal lngUserId As Long)

            Dim clsGeneric As New MaxPayroll.Generic

            Try
                Directory.Move(strSource, strDestination)
            Catch ex As Exception
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcBackupApprovedFile - clsCommon", Err.Number, Err.Description)
            Finally
                clsGeneric = Nothing
            End Try

        End Sub

#End Region

#Region "Finalize"

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

#End Region

#Region "Disable Button"

        '*****************************************************************************************************
        'Procedure Name : fncBtnDisable()
        'Purpose        : To Disable Button
        'Arguments      : 
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/03/2005
        '*****************************************************************************************************
        Public Sub fncBtnDisable(ByVal btnDisable As System.Web.UI.WebControls.Button, ByVal PageHasValidators As Boolean)

            Dim strJavaScript As String, strAlert As String

            strAlert = "Please wait. We are processing your request. If you do not receive confirmation, please check your mail box or File Status Report for confirmation. Click OK to proceed."

            If PageHasValidators Then
                strJavaScript = "if (typeof(Page_ClientValidate) == 'function') {if (Page_ClientValidate())" & _
                "{this.value='Please wait...';this.disabled = true;alert('" & strAlert & "');" & _
                btnDisable.Page.GetPostBackEventReference(btnDisable) & ";}}"
            Else
                strJavaScript = "this.value='Please wait...';this.disabled = true;" & _
                btnDisable.Page.GetPostBackEventReference(btnDisable)
            End If

            btnDisable.Attributes.Add("onclick", strJavaScript)

        End Sub
        Public Sub fncUploadBtnDisable(ByVal btnDisable As System.Web.UI.WebControls.Button, ByVal PageHasValidators As Boolean)

            Dim strJavaScript As String, strAlert As String

            strAlert = "Please wait. We are processing your request. If you do not receive confirmation, please check your mail box or File Status Report for confirmation. Click OK to proceed."

            If PageHasValidators Then
                strJavaScript = "if (typeof(Page_ClientValidate) == 'function') {if (Page_ClientValidate())" &
                "{this.value='Please wait...';this.disabled = true;alert('" & strAlert & "');" &
                btnDisable.Page.GetPostBackEventReference(btnDisable) & ";}}"
            Else
                strJavaScript = "this.value='Please wait...';this.disabled = true;" & _
                btnDisable.Page.GetPostBackEventReference(btnDisable)
            End If

            btnDisable.Attributes.Add("onclick", strJavaScript)

        End Sub
#End Region

#Region "Upload Check"

        '*****************************************************************************************************
        'Procedure Name : fncBtnDisable()
        'Purpose        : To Disable Button
        'Arguments      : 
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/03/2005
        '*****************************************************************************************************
        Public Function fncUploadCheck(ByVal strRequest As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                ByVal lngGroupId As Long, ByVal strFileType As String, ByVal strPayDate As String, _
                        ByVal dcTotalAmount As Decimal) As Int16

            'Create Instance of SQL Command Object
            Dim cmdUploadCheck As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intCount As Int16

            Try
                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdUploadCheck
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_UploadCheck"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Option", strRequest))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_PayDate", strPayDate))
                    .Parameters.Add(New SqlParameter("@in_TotAmount", dcTotalAmount))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    intCount = .ExecuteScalar()
                End With

                Return intCount

            Catch

                'Log Error  
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncUploadCheck - clsCommon", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Mail List"

        '****************************************************************************************************
        'Procedure Name : fncMailList()
        'Purpose        : To Display List of Mails For the Logged In User
        'Arguments      : N/A
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 09/10/2003
        '*****************************************************************************************************
        Public Function fncMailList(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngGroupId As Long, _
                            Optional ByVal intDays As Int16 = 15) As System.Data.DataSet

            'Create Instance of SQL Data Adaptor
            Dim sdaMailBox As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsMailBox As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String

            Try

                'Create SQL Connection Object
                Call clsGeneric.SQLConnection_Initialize()

                'Build SQL Statement
                strSQL = "Exec pg_MailBox " & lngUserId & "," & lngGroupId & "," & intDays & ",'" & gc_Const_ApplicationName & " " & gc_Const_RegCenter & "'"

                'Fetch Records And Assign to Adaptor
                sdaMailBox = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaMailBox.Fill(dsMailBox, "MAILBOX")

                Return dsMailBox

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncMailList - clsCommon", Err.Number, Err.Description)

            Finally

                'Desroy SQL Connection Object
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Data Adaptor
                sdaMailBox = Nothing

                'Destroy Instance of Data Set
                dsMailBox = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Message Constructor"
        Public Shared Function fncMsgBankWithNoFormat(ByVal sBankName As String, ByVal sPaymentType As String) As String
            If Len(sBankName) > 0 AndAlso Len(sPaymentType) > 0 Then
                Return sBankName.Trim & " does not have file setting of [" & sPaymentType.Trim & "]."
            Else
                Return "Either Bank Name or Payment Type is not available for function to process"
            End If
        End Function


#End Region

#Region "Default Bank Checking "
        Public Shared Function fncDefaultBankChecking(ByRef ddBank As DropDownList, ByRef lblBank As Label) As String
            Dim sRetVal As String = ""

            Try
                Dim sDefaultBankCode = ConfigurationManager.AppSettings("DefaultBankCode")

                If Len(sDefaultBankCode) > 0 Then

                    Dim bIsBankCodeExist As Boolean
                    Dim oListItem As System.Web.UI.WebControls.ListItem

                    For Each oListItem In ddBank.Items
                        If oListItem.Value = sDefaultBankCode Then
                            bIsBankCodeExist = True
                            ddBank.SelectedValue = sDefaultBankCode
                            ddBank.Visible = False
                            lblBank.Visible = False
                            Exit For
                        End If
                    Next

                    If bIsBankCodeExist = False Then
                        sRetVal = "Default Bank Code not found in system database, please contact system administrator. <BR>"
                    End If

                End If

            Catch ex As Exception
                sRetVal = ex.Message
            End Try

            Return sRetVal
        End Function

        Public Shared Function fncDefaultBankChecking() As Boolean

            Dim sDefaultBankCode = ConfigurationManager.AppSettings("DefaultBankCode")


            If Len(sDefaultBankCode) > 0 Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "Display or hide bank label or textbox "

        Public Shared Function fncShowHideBankCodeControls(ByRef txtBank As TextBox, ByRef lblBank As Label) As String
            Dim sRetVal As String = ""

            Try
                Dim sDefaultBankCode = ConfigurationManager.AppSettings("DefaultBankCode")

                If Len(sDefaultBankCode) > 0 Then
                    txtBank.Visible = False
                    lblBank.Visible = False
                End If

            Catch ex As Exception
                sRetVal = ex.Message
            End Try

            Return sRetVal
        End Function

        Public Shared Function fncShowHideBankCodeControls(ByRef ddBank As DropDownList, ByRef lblBank As Label) As String
            Dim sRetVal As String = ""

            Try
                Dim sDefaultBankCode = ConfigurationManager.AppSettings("DefaultBankCode")

                If Len(sDefaultBankCode) > 0 Then
                    ddBank.Visible = False
                    lblBank.Visible = False
                End If

            Catch ex As Exception
                sRetVal = ex.Message
            End Try

            Return sRetVal
        End Function
#End Region

#Region "Execute CCrypt EXE  "

        Public Function ShellExecute(ByVal bUseShellExecute As Boolean, ByVal sProgName As String, ByVal sCommandArgs As String, ByVal strOutputFileName As String, ByVal lngUserId As Long, ByVal lngOrgId As Long) As Boolean
            Dim _Process As New Process
            Dim _ProcessStartInfo As ProcessStartInfo
            Dim bRetVal As Boolean = False
            Dim clsGeneric As New Generic
            Dim a As Integer
            Try
                'a = Shell(sProgName & " " & sCommandArgs, AppWinStyle.Hide)

                _ProcessStartInfo = New ProcessStartInfo

                With _ProcessStartInfo
                    .UseShellExecute = bUseShellExecute
                    '.WorkingDirectory = "D:\ccrypt\"
                    .FileName = sProgName '"ccrypt.exe"
                    .Arguments = sCommandArgs
                    '.Arguments = ""
                End With

                'With _ProcessStartInfo
                '    .UseShellExecute = False
                '    .FileName = "E:\CCRYPT\ccrypt1.bat"
                '    .Arguments = ""
                'End With

                '_Process.StartInfo.RedirectStandardOutput = False
                '_Process.StartInfo.RedirectStandardError = False
                '_Process.StartInfo.UseShellExecute = True
                '_Process.StartInfo.FileName = "E:\CCRYPT\ccrypt1.bat"
                '_Process.Start()
                ' _Process.WaitForExit()




                If File.Exists(strOutputFileName) Then
                    File.Delete(strOutputFileName)
                End If

                _Process = Process.Start(_ProcessStartInfo)
                'clsGeneric.ErrorLog(0, 0, "Trace", 0, _Process.MainWindowTitle.ToString)
                _Process.WaitForExit()
                If Not _Process.HasExited Then
                    _Process.Kill()
                End If
                bRetVal = True
            Catch ex As Exception
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "ShellExecute - clsCommon", Err.Number, Err.Description)
                GC.Collect(0)
                Throw ex
            End Try
            Return bRetVal
        End Function
#End Region

#Region "Get Organization CCrypt Encryption Key "
        Public Function fncGetOrgCcryptKey(ByVal lngOrgId As Long, ByVal lngUserId As Long) As String

            Dim clsGeneric As New Generic
            Dim strSql As String
            Dim strRetVal As String = ""
            Dim clsEncryption As New MaxPayroll.Encryption

            Try
                strSql = "select Org_EncryptionKey from org_master where Org_Id =" & lngOrgId
                strRetVal = SqlHelper.ExecuteScalar(clsGeneric.sSQLConnection, CommandType.Text, strSql)
                strRetVal = clsEncryption.Cryptography(strRetVal)

            Catch ex As Exception
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGetOrgCcryptKey - clsCommon", Err.Number, Err.Description)

            Finally
                clsGeneric = Nothing
                clsEncryption = Nothing
            End Try

            Return strRetVal

        End Function

#End Region

#Region "Validate approver's limit againts file total amount "
        Public Shared Function fncCheckApproverLimit(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngFileId As Long) As Boolean
            Dim bRetVal As Boolean
            Dim SqlParams(2) As SqlParameter

            SqlParams(0) = New SqlParameter("@in_FileId", SqlDbType.BigInt)
            SqlParams(0).Value = lngFileId
            SqlParams(1) = New SqlParameter("@in_UserId", SqlDbType.BigInt)
            SqlParams(1).Value = lngUserId
            SqlParams(2) = New SqlParameter("@out_Result", SqlDbType.Bit, 0, ParameterDirection.Output, False, 0, 0, "out_Result", DataRowVersion.Default, 0)

            SqlHelper.ExecuteNonQuery(Generic.sSQLConnection, CommandType.StoredProcedure, "pg_ValidateApproverLimit", SqlParams)

            If SqlParams(2).Value = 0 Then
                bRetVal = False
            Else
                bRetVal = True
            End If

            Return bRetVal

        End Function
#End Region

#Region "Generate Autopay File Type 2 (SNA) bottom value for body content "

        Public Function fncAutopaySNABody(ByVal sBeneficiaryId As String) As String

            Dim sIDIndicator As String = ""

            If IsNumeric(sBeneficiaryId) AndAlso sBeneficiaryId.Length = 12 Then
                sIDIndicator = "1"
            ElseIf (IsNumeric(sBeneficiaryId) AndAlso sBeneficiaryId.Length = 7) OrElse _
                    ((sBeneficiaryId.ToUpper.StartsWith("A") Or sBeneficiaryId.ToUpper.StartsWith("H")) And sBeneficiaryId.Length = 8) OrElse _
                    (sBeneficiaryId.ToUpper.StartsWith("K") And (sBeneficiaryId.Length = 8 Or sBeneficiaryId.Length = 7)) Then

                sIDIndicator = "2"

            ElseIf sBeneficiaryId.ToUpper.StartsWith("T") AndAlso sBeneficiaryId.Length = 7 Then
                sIDIndicator = "4"
            Else

                sIDIndicator = "4"
            End If

            Return "50" & Space(20) & sIDIndicator
        End Function
#End Region

#Region " Check If direct debit is registered  "

        Public Function fncCheckDirectDebit(ByVal lngOrgId As Long, ByVal lngGroupId As Long) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "Exec CIMB_ToGetMenu " & lngGroupId & "," & lngOrgId
            Dim iCount As Int16 = 0

            Try
                iCount = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSQL)
                If iCount > 0 Then
                    bRetVal = True
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return bRetVal
        End Function

#End Region

#Region " Check if FileType Is registered for that Organization and Group "

        Public Function CheckFileType(ByVal lngOrgId As Long, ByVal lngGroupId As Long, ByVal ParamArray FileTypeName() As String) As Boolean

            'Variable declaration-start
            Dim bRetVal As Boolean = False
            'Variable declaration-stop

            Dim strSQL As String = _Helper.GetSQLFileTypeMenu & lngGroupId & "," & lngOrgId

            For _count = 0 To FileTypeName.GetUpperBound(0)
                strSQL &= ",'" & FileTypeName(Count) & "'"
            Next

            Dim iCount As Int16 = 0

            Try
                iCount = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSQL)
                If iCount > 0 Then
                    bRetVal = True
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return bRetVal
        End Function

#End Region

#Region "Delete Records For Rejected  Trans"

        Public Sub prcDeleteRejTrans(ByVal lngFileId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                            ByVal strTableName As String)

            'Create Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQLStatement As String
            Try
                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Build SQL Statement

                strSQLStatement = " DELETE FROM tPgt_FileDetails WHERE FileId =" & lngFileId

                'Excute SQL Command Object
                Dim cmdFailedTrans As New SqlCommand(strSQLStatement, clsGeneric.SQLConnection)
                cmdFailedTrans.ExecuteNonQuery()

                'Destroy SQL Command Instance
                cmdFailedTrans = Nothing

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcDeleteRejTrans - ClsUpload", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region " Get the Payment Service Id based on FileType Name "
        'Author     : Naresh T-Melmax Sdn Bhd
        'Created on : 12/06/2011
        'Purpose    : To get the Payment Service Id based on FileType Name
        Public Function GetPayserId(ByVal FileTypeName As String, ByVal OrgId As Long, ByVal UserId As Long) As Short

            'Create Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Variables declaration
            Dim SqlStatement As String = Nothing

            Try

                'Build SqlStatement
                SqlStatement = _Helper.GetSQLCommon & "ServiceType_Id" & _
                    MaxGeneric.clsGeneric.AddComma & MaxGeneric.clsGeneric.AddQuotes(FileTypeName)

                'Execute SqlStatement
                Return _MaxDataBase.GetValue(SqlStatement)

            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(OrgId, UserId, "GetPayserId-clsCommon", Err.Number, Err.Description)

                'On error return nothing
                Return Nothing

            End Try

        End Function
#End Region



    End Class

End Namespace
