'****************************************************************************************************
'Class Name     : Generic
'ProgId         : MaxPayroll.Generic
'Purpose        : Generic Functions Used For The Complete Project
'Author         : Sujith Sharatchandran - 
'Created        : 22/08/2003
'*****************************************************************************************************
Option Explicit On
Imports System.IO
Imports System.Data
Imports MaxPayroll.Encryption
Imports System.Data.SqlClient


Namespace MaxPayroll


    Public Class Generic

#Region "Global Variables"

        Public strConnection As String                                  'Connection String
        Public SQLConnection As SqlConnection                       'Global SQL Connection
        Dim lngOrganisationId As Long, lngUserCode As Long              'Organisation ID
        Dim ASPNetContext As HttpContext = HttpContext.Current          'ASP Net Context Object
        Public SQLCmd As SqlCommand
        Public SQLTxn As SqlTransaction

#End Region

#Region "SQL Connection"
        Public Shared ReadOnly Property sSQLConnection() As String
            Get
                Dim strServer As String, strDatabase As String, strUserName As String, strPassword As String
                'Create Instance of Encryption Class Object
                Dim clsEncryption As New MaxPayroll.Encryption


                'Get SQL Details - Start
                strServer = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("SERVER"))
                strDatabase = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("DATABASE"))
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("USERNAME"))
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("PASSWORD"))
                Return "SERVER=" & strServer & ";DATABASE=" & strDatabase & ";UID=" & strUserName & ";PWD=" & strPassword
            End Get
        End Property

        Public ReadOnly Property strSQLConnection() As String
            Get
                Dim strServer As String, strDatabase As String, strUserName As String, strPassword As String
                'Create Instance of Encryption Class Object
                Dim clsEncryption As New MaxPayroll.Encryption


                'Get SQL Details - Start
                strServer = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("SERVER"))
                strDatabase = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("DATABASE"))
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("USERNAME"))
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("PASSWORD"))
                Return "SERVER=" & strServer & ";DATABASE=" & strDatabase & ";UID=" & strUserName & ";PWD=" & strPassword
            End Get
        End Property

        '****************************************************************************************************
        'Procedure Name : SQLConnection_Initialize()
        'Purpose        : Create SQL Connection
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/08/2003
        '*****************************************************************************************************
        Public Sub SQLConnection_Initialize()

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim strServer As String, strDatabase As String, strUserName As String, strPassword As String

            Try

                'Get SQL Details - Start
                strServer = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("SERVER"))
                strDatabase = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("DATABASE"))
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("USERNAME"))
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("PASSWORD"))
                strConnection = "SERVER=" & strServer & ";DATABASE=" & strDatabase & ";UID=" & strUserName & ";PWD=" & strPassword
                'Get SQL Details - Stop

                'Check If SQL Connection is Not Initialized
                If SQLConnection Is Nothing Then
                    'Assign Connection String
                    strConnection = strConnection
                    'Create Connection Object
                    SQLConnection = New SqlConnection(strConnection)
                    'Open Connection
                    SQLConnection.Open()
                ElseIf SQLConnection.State = ConnectionState.Closed Then
                    'Open Connection
                    SQLConnection.Open()
                End If

            Catch

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : SQLConnection_Terminate()
        'Purpose        : Destroy SQL Connection
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/08/2003
        '*****************************************************************************************************
        Public Sub SQLConnection_Terminate()
            'Check SQL connection Object is Created 
            If Not SQLConnection Is Nothing Then
                'If Connection Object Created and Open
                If SQLConnection.State = ConnectionState.Open Then
                    SQLConnection.Close()       'Close Connection
                    SQLConnection = Nothing     'Destroy Connection Object
                    'If Connection Object Created and Closed
                ElseIf SQLConnection.State = ConnectionState.Closed Then
                    SQLConnection = Nothing     'Destroy Connection
                End If
            End If
        End Sub
        '****************************************************************************************************
        'Procedure Name : CheckSession
        'Purpose        : Check the session
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith - 
        'Created        : 22/09/2003
        '*****************************************************************************************************
        Public Function CheckSession() As Boolean

            'Declare Variables
            Dim strUrl As String
            'Read the Session Value
            lngUserCode = IIf(IsNumeric(ASPNetContext.Session("USER_ID")), ASPNetContext.Session("USER_ID"), 0)
            lngOrganisationId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
            'Reset the Session
            lngOrganisationId = 1
            If lngOrganisationId = 0 Then

                strUrl = "PG_Login.aspx"
                ASPNetContext.Response.Write("<script language = 'Javascript'>" & vbCrLf)
                ASPNetContext.Response.Write("top.location.href='" & strUrl & "';")
                ASPNetContext.Response.Write(vbCrLf & "</script>")

                Return False
            Else
                Return True
            End If

        End Function

#End Region

#Region "Error Log"

        '****************************************************************************************************
        'Procedure Name : ErrorLog()
        'Purpose        : Log All Errors Encountered in the Applcation
        'Arguments      : UserID
        'Return Value   : N/A
        'Author         : Victor Wong 
        'Created        : 06/Feb/2007
        '*****************************************************************************************************
        Public Sub ErrorLog(ByVal strSource As String,
                    ByVal lngErrorNo As Long, ByVal strErrorDesc As String)

            'Create Command Object 
            Dim cmdErrorLog As New SqlCommand
            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current
            Dim lngOrgId As Long, lngUserCode As Long
            Try
                lngOrgId = IIf(IsNumeric(AspNetContext.Session(MaxPayroll.mdConstant.gc_Ses_OrgId)), AspNetContext.Session(MaxPayroll.mdConstant.gc_Ses_OrgId), 0)
                lngUserCode = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)

                'Intialize Connection String
                Call SQLConnection_Initialize()

                'Insert Command Parameters - Start
                With cmdErrorLog
                    .Connection = SQLConnection
                    .CommandText = "pg_ErrorLog"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserCode))
                    .Parameters.Add(New SqlParameter("@in_Operation", strSource))
                    .Parameters.Add(New SqlParameter("@in_ErrNumber", lngErrorNo))
                    .Parameters.Add(New SqlParameter("@in_ErrDesc", strErrorDesc))
                    .ExecuteNonQuery()
                End With
                'Insert Command Parameters - Stop

            Catch ex As Exception

            Finally

                'Terminate Connection String
                Call SQLConnection_Terminate()

                'Destroy Command Object
                cmdErrorLog = Nothing


            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : ErrorLog()
        'Purpose        : Log All Errors Encountered in the Applcation
        'Arguments      : UserID
        'Return Value   : N/A
        'Author         : Sujith - 
        'Created        : 14/09/2003
        '*****************************************************************************************************
        Public Sub ErrorLog(ByVal lngOrgId As Long, ByVal lngUsrCode As Long, ByVal strSource As String,
                    ByVal lngErrorNo As Long, ByVal strErrorDesc As String)

            'Create Command Object 
            Dim cmdErrorLog As New SqlCommand

            Try

                'Intialize Connection String
                Call SQLConnection_Initialize()

                'Insert Command Parameters - Start
                With cmdErrorLog
                    .Connection = SQLConnection
                    .CommandText = "pg_ErrorLog"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUsrCode))
                    .Parameters.Add(New SqlParameter("@in_Operation", strSource))
                    .Parameters.Add(New SqlParameter("@in_ErrNumber", lngErrorNo))
                    .Parameters.Add(New SqlParameter("@in_ErrDesc", strErrorDesc))
                    .ExecuteNonQuery()
                End With
                'Insert Command Parameters - Stop

            Catch ex As Exception

            Finally

                'Terminate Connection String
                Call SQLConnection_Terminate()

                'Destroy Command Object
                cmdErrorLog = Nothing

            End Try

        End Sub

#End Region

#Region "Get Organisation Logo"

        Public Function fnGetPhoto(ByVal strPath As String) As Byte()

            Dim fs As FileStream = New FileStream(strPath, FileMode.Open, FileAccess.Read)
            Dim br As BinaryReader = New BinaryReader(fs)

            Dim photo() As Byte = br.ReadBytes(0)
            Try
                photo = br.ReadBytes(fs.Length)

                br.Close()
                fs.Close()
            Catch ex As Exception
                br.Close()
                fs.Close()
            End Try

            Return photo

        End Function

#End Region

#Region "Session Timeout"

        '****************************************************************************************************
        'Function Name  : fnSession()
        'Purpose        : To Check Session is Live
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith - 
        'Created        : 06/11/2003
        '*****************************************************************************************************
        Public Function fnSession() As Boolean

            'Create Instance of ASPNet Context 
            Dim ASPNetContext As HttpContext = HttpContext.Current

            Try
                'IF Organisation ID Session And User ID Session is Not Null Then Return True
                If IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)) And IsNumeric(ASPNetContext.Session("SYS_USERID")) Then
                    'And ASPNetContext.Session("SYS_USERID") <> "" Then
                    Return True
                    'IF Organisation ID Session And User ID Session is Null Then Return False
                Else
                    Return False
                End If

            Catch ex As Exception

            Finally

                ASPNetContext = Nothing

            End Try

        End Function

#End Region

#Region "Write Access Log"

        Public Function fnWriteLog(ByVal lngLogId As Long, Optional ByVal intGroupId As Int32 = 0) As Long

            'Create Instance of SQL Command Object
            Dim cmdLogin As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP Net Objects
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strUserIP As String, lngOrgId As Long, lngUserId As Long, lngAccId As Long


            Try
                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                lngOrgId = ASPNetContext.Session(gc_Ses_OrgId)                               'Organisation Id
                lngUserId = ASPNetContext.Session("SYS_USERID")                             'User Id
                strUserIP = ASPNetContext.Request.ServerVariables("REMOTE_ADDR")            'User IP Address

                'Execute Stored Procedure
                With cmdLogin
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_AccessLog"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_AccId", lngLogId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_UserIP", strUserIP))
                    .Parameters.Add(New SqlParameter("@out_AccId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_AccId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                lngAccId = IIf(IsNumeric(cmdLogin.Parameters("@out_AccId").Value), cmdLogin.Parameters("@out_AccId").Value, 0)

                Return lngAccId

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnWriteLog - clsGeneric", Err.Number, Err.Description)

            Finally

                'Destroy SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Command Object
                cmdLogin = Nothing

                'Destroy Generic Class
                clsGeneric = Nothing

                'Destroy Asp Net Context Object
                ASPNetContext = Nothing

            End Try

        End Function

#End Region

#Region "Write User Login Unsuccessful attempts"

        '****************************************************************************************************
        'Procedure Name : fnWriteFailedLog()
        'Purpose        : To Log Users Unsuccessful attempts 
        'Arguments      : Log Id, Group Id
        'Return Value   : N/A
        'Author         : Eu Yean Lock - 
        'Created        : 19/10/2006
        '*****************************************************************************************************

        Public Function fnWriteFailedLog(ByVal lngLogId As Long, Optional ByVal intGroupId As Int32 = 0, Optional ByVal errMsg As String = "") As Long

            'Create Instance of SQL Command Object
            Dim cmdLogin As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP Net Objects
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strUserIP As String, lngOrgId As Long, lngUserId As Long

            Try
                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                lngOrgId = ASPNetContext.Session(gc_Ses_OrgId)                               'Organisation Id
                lngUserId = ASPNetContext.Session("SYS_USERID")                             'User Id
                strUserIP = ASPNetContext.Request.ServerVariables("REMOTE_ADDR")            'User IP Address

                'Execute Stored Procedure
                With cmdLogin
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_Userlogin_Failed"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_GroupId", intGroupId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_UserIP", strUserIP))
                    .Parameters.Add(New SqlParameter("@in_ErrorMsg", errMsg))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnWriteFailedLog - clsGeneric", Err.Number, Err.Description)

            Finally

                'Destroy SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Command Object
                cmdLogin = Nothing

                'Destroy Generic Class
                clsGeneric = Nothing

                'Destroy Asp Net Context Object
                ASPNetContext = Nothing

            End Try

            Return 1

        End Function

#End Region

#Region "Authorization Code Check"

        '****************************************************************************************************
        'Function Name  : fncAuthCheck()
        'Purpose        : To Check Authorization Code is Valid
        'Arguments      : USer Id
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 06/07/2005
        '*****************************************************************************************************
        Public Function fncAuthCheck(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strAuthCode As String) As String

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of ASPContext Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strDbAuthCode As String, IsAuthCode As Boolean, intAttempts As Int16

            Try

                'Check Session Value for Authorization Lock - Start
                If Not IsNumeric(ASPNetContext.Session("AUTH_LOCK")) Or ASPNetContext.Session("AUTH_LOCK") = 0 Then
                    ASPNetContext.Session("AUTH_LOCK") = 0
                End If
                'Check Session Value for Authorization Lock - Stop

                'Check If AuthCode is Valid - Start
                strDbAuthCode = clsCommon.fncPassAuth(lngUserId, "A", 100000)
                IsAuthCode = IIf(strDbAuthCode = strAuthCode, True, False)
                'Check If AuthCode is Valid - Stop

                'Check for invalid Authorization Code Attempts - START
                If Not IsAuthCode Then
                    intAttempts = IIf(IsNumeric(ASPNetContext.Session("AUTH_LOCK")), ASPNetContext.Session("AUTH_LOCK"), 0)
                    If Not intAttempts = 2 Then
                        'Increment Attempts
                        intAttempts = intAttempts + 1
                        'Assign Current Attempt to Session
                        ASPNetContext.Session("AUTH_LOCK") = intAttempts
                        'Display Message
                        Return "Validation code is invalid. Please enter a valid Validation Code."
                    ElseIf intAttempts = 2 Then
                        'Lock Authorization Code
                        Call clsUsers.prcAuthLock(100000, lngUserId, "A")
                        'Track Auth Lock
                        Call clsUsers.prcLockHistory(lngUserId, "A")
                        'Display Message
                        Return "Your Validation Code has been locked due to invalid attempts. Please contact your Bank Administrator."
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP
            Catch ex As Exception

            Finally

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of ASPContext Object
                ASPNetContext = Nothing

            End Try

            Return ""

        End Function

#End Region

#Region "Delete Mail"

        Public Sub DeleteMail(ByVal strOption As String, ByVal lngMailId As Long, ByVal lngOrgId As Long)

            'Create Command Object
            Dim cmdMail As New SqlCommand

            Try
                'Create SQL Connection Object
                Call SQLConnection_Initialize()

                With cmdMail
                    .Connection = SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_DeleteMail"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_MailId", lngMailId))
                    .Parameters.Add(New SqlParameter("@orgId", lngOrgId))
                    .ExecuteNonQuery()
                End With

            Catch


            Finally

                'Destory Command object
                cmdMail = Nothing

                'Desroy SQL Connection Object
                Call SQLConnection_Terminate()

            End Try

        End Sub


#End Region

#Region "Load Mail"

        '****************************************************************************************************
        'Procedure Name : fnLoadMail()
        'Purpose        : Load Mail Contents
        'Arguments      : Mail Id
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 02/11/2003
        '*****************************************************************************************************
        Public Function fnLoadMail(ByVal lngMail As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngGroupId As Long) As System.Data.DataSet

            Dim dsMail As New System.Data.DataSet   'Create System Date Set Object
            Dim sdaMail As New SqlDataAdapter       'Create SQL Data Adaptor Object
            Dim clsGeneric As New MaxPayroll.Generic   'Create Generic Class Object

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adaptor
                sdaMail = New SqlDataAdapter("Exec pg_DisplayMail " & lngMail, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaMail.Fill(dsMail, "MAIL")

                Return dsMail

            Catch

                'Log Error 
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnLoadMail - Mailbox", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaMail = Nothing

                'Destroy Data Set
                dsMail = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "User Login"

        Public Function Login(ByVal lngOrgId As Long, ByVal intUserId As Int32,
            ByVal strLoginDate As String, ByVal intInterval As Int16) As String

            'create instance of sql command object
            Dim cmdLogin As New SqlCommand

            'variable decalarations
            Dim strResult As String

            Try

                'initialise sql connection
                Call SQLConnection_Initialize()

                With cmdLogin
                    .Connection = SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_UserLogin_Login"
                    With .Parameters
                        .Add(New SqlParameter("@in_UserId", intUserId))
                        .Add(New SqlParameter("@in_LoginDate", strLoginDate))
                        .Add(New SqlParameter("@in_Interval", intInterval))
                    End With
                    strResult = .ExecuteScalar()
                End With

                Return strResult

            Catch ex As Exception

                'log error
                ErrorLog(lngOrgId, intUserId, "Generic - Login", 0, ex.Message)

            Finally

                'terminate sql connection
                Call SQLConnection_Terminate()

                'destroy instance of sql command object
                cmdLogin = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "User Logoff"

        Public Sub Logoff(ByVal lngOrgId As Long, ByVal intUserId As Int32)


            'create instance of sql command object
            Dim cmdLogin As New SqlCommand

            Try

                'initialise sql connection
                Call SQLConnection_Initialize()

                With cmdLogin
                    .Connection = SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_UserLogin_Logoff"
                    .Parameters.Add(New SqlParameter("@in_UserId", intUserId))
                    .ExecuteNonQuery()
                End With

            Catch ex As Exception

                'log error
                ErrorLog(lngOrgId, intUserId, "Generic - Logoff", 0, ex.Message)

            Finally

                'terminate sql connection
                Call SQLConnection_Terminate()

                'destroy instance of sql command object
                cmdLogin = Nothing

            End Try

        End Sub

#End Region

#Region "User Details"

        Public Function fnLoadUserDetails(ByVal lngUserId As Long) As System.Data.DataSet

            Dim dsMail As New System.Data.DataSet   'Create System Date Set Object
            Dim sdaMail As New SqlDataAdapter       'Create SQL Data Adaptor Object
            Dim clsGeneric As New MaxPayroll.Generic   'Create Generic Class Object

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adaptor
                sdaMail = New SqlDataAdapter("Exec pg_GetUserDetail " & lngUserId, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaMail.Fill(dsMail, "USERDETAILS")

                Return dsMail

            Catch

                'Log Error 
                clsGeneric.ErrorLog(0, lngUserId, "fnLoadUserDetails", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaMail = Nothing

                'Destroy Data Set
                dsMail = Nothing

            End Try
            Return Nothing
        End Function

#End Region

    End Class

End Namespace
