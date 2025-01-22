Option Strict Off
Option Explicit On

Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.Encryption
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data



Namespace MaxPayroll

    Public Class clsUsers

#Region "User Maintenance"

        '*******************************************************************************************************************
        'Procedure Name     :   fnDB_User
        'Purpose            :   To Handle User Insert/Update
        'Arguments          :   Add/Update
        'Return Values      :   Ok/Error Msg
        'Author             :   Sujith Sharatchandran - 
        'Created            :   10/10/2003
        '********************************************************************************************************************
        Public Function fnDB_User(ByVal strAction As String, ByVal intApproved As Int16, Optional ByVal lngOrgId As Long = 0) As Long

            'Create Instance of SQL Command Object
            Dim cmdUserDetails As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of ASP .Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarartions
            Dim lngOrganisationId As Long
            Dim lngUserCode As Long
            Dim lngUserId As Long
            Dim dcLimit As Decimal
            Dim strReset As String
            Dim strUserId As String
            Dim strUserName As String
            Dim strUserType As String
            Dim strUserStatus As String
            Dim strPassword As String
            Dim strChangeUnit As String
            Dim intChangePeriod As Int16
            Dim dtExpiryDt As String
            Dim strAuthCode As String
            Dim intDisplay As Int16
            Dim strUser As String
            Dim bReceiveEmail As Boolean
            Dim sEmail As String = ""
            Dim strStaffNumber As String = ""

            Try

                'Assign Values to Declared Variables

                strUser = ASPNetContext.Session("SYS_TYPE")
                intDisplay = ASPNetContext.Request.Form("ctl00$cphContent$hDisplay")                                                              'Report Display Type
                strUserType = ASPNetContext.Request.Form("ctl00$cphContent$txtRole")                                                              'User Type
                strUserStatus = ASPNetContext.Request.Form("ctl00$cphContent$hStatus")                                                            'User Status
                strPassword = ASPNetContext.Request.Form("ctl00$cphContent$hPassword")                                                            'Password
                strAuthCode = ASPNetContext.Request.Form("ctl00$cphContent$hAuthCode")                                                            'Auth Code
                dtExpiryDt = ASPNetContext.Request.Form("ctl00$cphContent$txtCExpDate")                                                           'Expiry Date 
                strUserId = ASPNetContext.Request.Form("ctl00$cphContent$txtCUserLogin")                                                          'Login User Id
                strUserName = ASPNetContext.Request.Form("ctl00$cphContent$txtCUserName")                                                         'User Name
                strReset = ASPNetContext.Request.Form("ctl00$cphContent$hReset")                                                               'Reset
                strChangeUnit = ASPNetContext.Request.Form("ctl00$cphContent$txtCChangeUnit")                                                     'Password Change Period Period
                intChangePeriod = ASPNetContext.Request.Form("ctl00$cphContent$txtCPassChangePeriod")                                             'Password Change Period Unit
                dcLimit = IIf(IsNumeric(ASPNetContext.Request.Form("ctl00$cphContent$hLimit")), ASPNetContext.Request.Form("ctl00$cphContent$hLimit"), 0)          'User Auth Limit
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)        'Logged in User Id
                lngUserId = IIf(IsNumeric(ASPNetContext.Request.Form("ctl00$cphContent$hUserId")), ASPNetContext.Request.Form("ctl00$cphContent$hUserId"), 0)      'User Id
                If lngOrgId > 0 Then
                    lngOrganisationId = lngOrgId
                Else
                    lngOrganisationId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)    'Organisation Id
                End If


                sEmail = ASPNetContext.Request.Form("ctl00$cphContent$hEmail") & ""

                strStaffNumber = ASPNetContext.Request.Form("ctl00$cphContent$txtCStaffNumber")

                'If Sys Admin
                If strUser.Equals(gc_UT_SysAdmin) Then
                    If strUserType = gc_UT_ReportDownloaderDesc Then
                        strUserType = gc_UT_ReportDownloader
                    Else
                        strUserType = Left(strUserType, 1)
                    End If

                    'If Bank Admin
                ElseIf strUser = gc_UT_BankAdmin Then
                    'If Bank User
                    If strUserType = gc_UT_BankUserDesc Then
                        strUserType = gc_UT_BankUser

                        If ASPNetContext.Request.Form("ctl00$cphContent$hIsReceiveEmail") = "1" Then
                            bReceiveEmail = True
                        Else
                            bReceiveEmail = False
                        End If
                        'If Bank Operator
                    ElseIf strUserType = gc_UT_BankOperatorDesc Then
                        strUserType = gc_UT_BankOperator
                    ElseIf strUserType = gc_UT_InquiryUserDesc Then
                        strUserType = gc_UT_InquiryUser
                    ElseIf strUserType = gc_UT_BankDownloaderDesc Then
                        strUserType = gc_UT_BankDownloader
                    ElseIf strUserType = gc_UT_ReportDownloaderDesc Then
                        strUserType = gc_UT_ReportDownloader
                    Else
                        'Special Error Handling #1
                        clsGeneric.ErrorLog(lngOrganisationId, lngUserCode, "fnDB_User - clsUsers: Invalid User Type - Pls find the word with [Special Error Handling #1] in clsUsers", Err.Number, Err.Description)

                        Return 0
                    End If
                ElseIf strUser = gc_UT_BankUser Then
                    strUserType = ASPNetContext.Request.Form("ctl00$cphContent$AhURole")
                End If

                'Check IF User Password Exist
                If strPassword <> "" Then
                    strPassword = clsEncryption.Cryptography(strPassword)
                ElseIf strPassword = "" Then
                    strPassword = "N"
                End If

                'Check If User Authorization Code Exist
                If strAuthCode <> "" Then
                    strAuthCode = clsEncryption.Cryptography(strAuthCode)
                ElseIf strAuthCode = "" Then
                    strAuthCode = "N"
                End If

                'Initialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdUserDetails
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_UserDetails"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrganisationId))
                    .Parameters.Add(New SqlParameter("@in_UserLogin", strUserId))
                    .Parameters.Add(New SqlParameter("@in_UserName", strUserName))
                    .Parameters.Add(New SqlParameter("@in_Password", strPassword))
                    .Parameters.Add(New SqlParameter("@in_ExpiryDt", dtExpiryDt))
                    .Parameters.Add(New SqlParameter("@in_ChangeUnit", strChangeUnit))
                    .Parameters.Add(New SqlParameter("@in_ChangePeriod", intChangePeriod))
                    .Parameters.Add(New SqlParameter("@in_AuthCode", strAuthCode))
                    .Parameters.Add(New SqlParameter("@in_UserType", strUserType))
                    .Parameters.Add(New SqlParameter("@in_CreatedBy", lngUserCode))
                    .Parameters.Add(New SqlParameter("@in_UserStatus", strUserStatus))
                    .Parameters.Add(New SqlParameter("@in_Approved", intApproved))
                    .Parameters.Add(New SqlParameter("@in_AuthLimit", dcLimit))
                    .Parameters.Add(New SqlParameter("@in_Display", intDisplay))
                    .Parameters.Add(New SqlParameter("@in_Reset", strReset))
                    .Parameters.Add(New SqlParameter("@in_ReceiveEmail", bReceiveEmail))
                    .Parameters.Add(New SqlParameter("@in_Email", sEmail))
                    .Parameters.Add(New SqlParameter("@in_StaffNumber", strStaffNumber))
                    .Parameters.Add(New SqlParameter("@out_UserId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_UserId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                lngUserId = cmdUserDetails.Parameters("@out_UserId").Value

                Return lngUserId

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrganisationId, lngUserCode, "fnDB_User - clsUsers", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL Connection 
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdUserDetails = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Encryption
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Get User Details"

        '*******************************************************************************************************************
        'Procedure Name     :   fncUserDetails
        'Purpose            :   To Get User Details
        'Arguments          :   User Id
        'Return Values      :   DataSet
        'Author             :   Sujith Sharatchandran - 
        'Created            :   10/10/2003
        '********************************************************************************************************************
        Public Function fncUserDetails(ByVal lngUserId As Long, ByVal lngOrgId As Long, ByVal lngUserCode As Long) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adaptor
            Dim sdaUserDetails As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsUserDetails As New System.Data.DataSet

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adaptor
                sdaUserDetails = New SqlDataAdapter("Exec pg_GetUserDetail " & lngUserId, clsGeneric.SQLConnection)

                'Fetch Record And Fill Data Set
                sdaUserDetails.Fill(dsUserDetails, "USER")

                Return dsUserDetails

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fncUserDetails - clsUsers", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Data Adaptor
                sdaUserDetails = Nothing

                'Destroy Data Set
                dsUserDetails = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "View/Search Users"

        '*******************************************************************************************************************
        'Procedure Name     : UserGrid
        'Purpose            : To Handle User View/Search
        'Arguments          : N/A
        'Return Values      : Data Set
        'Author             : Sujith Sharatchandran - 
        'Created            : 10/10/2003
        'Modified           : 25/10/2003
        '********************************************************************************************************************
        Public Function UserGrid() As System.Data.DataSet

            'Create Instance of SQL Data Adaptor
            Dim sdaUser As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsUser As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngOrganisationId As Long, lngUserId As Long, strSQLStatment As String
            Dim strOption As String, strCriteria As String, strKeyword As String, strUserType As String

            Try

                'Assign Values to Variables
                strOption = ASPNetContext.Request.Form("ctl00$cphContent$cmbOption")
                strKeyword = ASPNetContext.Request.Form("ctl00$cphContent$txtKeyword")
                strUserType = UCase(ASPNetContext.Session("SYS_TYPE"))
                strCriteria = ASPNetContext.Request.Form("ctl00$cphContent$cmbCriteria")
                lngUserId = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
                lngOrganisationId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)

                'If search for E char - Start
                'If strOption = "ORG ID" Then
                '    Select Case strCriteria
                '        Case "EXACT MATCH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '                If Len(strKeyword) = 7 Then
                '           If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '              strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '              If Not IsNumeric(strKeyword) Then
                '                 strKeyword = 0
                '              End If
                '           Else
                '              strKeyword = 0
                '           End If
                '                Else
                '                    strKeyword = 0
                '                End If

                '            End If
                '        Case "CONTAINS"
                '            If Not IsNumeric(strKeyword) = True Then
                '        If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '           strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '        End If
                '            End If
                '        Case "STARTS WITH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '        strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '            End If
                '    End Select
                'End If
                'If search for E char - Stop

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Assign Search Parameters to Procedure
                strSQLStatment = "Exec pg_SearchUser3 '" & strOption & "','" & strCriteria & "','" & strKeyword & "'," & lngOrganisationId & ",'" & strUserType & "'"

                'Execute SQL Data Adaptor
                sdaUser = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaUser.Fill(dsUser, "USER")

                'Return Data Set
                Return dsUser

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "fnUserGrid", Err.Number, Err.Description)

                Return dsUser

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaUser = Nothing

            End Try

        End Function

        Public Function H2HUserGrid(ByVal strOption As String, ByVal strCriteria As String, ByVal strKeyword As String, ByVal lngOrgId As Long) As System.Data.DataSet

            'Create Instance of SQL Data Adaptor
            Dim sdaUser As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsUser As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngOrganisationId As Long, lngUserId As Long, strSQLStatment As String
            Dim strUserType As String

            Try

                'Assign Values to Variables
                'strOption = ASPNetContext.Request.Form("ctl00$cphContent$cmbOption")
                'strKeyword = ASPNetContext.Request.Form("ctl00$cphContent$txtKeyword")
                strUserType = UCase(ASPNetContext.Session("SYS_TYPE"))
                'strCriteria = ASPNetContext.Request.Form("ctl00$cphContent$cmbCriteria")
                lngUserId = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
                'lngOrganisationId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)

                'If search for E char - Start
                'If strOption = "ORG ID" Then
                '    Select Case strCriteria
                '        Case "EXACT MATCH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '                If Len(strKeyword) = 7 Then
                '                    If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '                        strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '                        If Not IsNumeric(strKeyword) Then
                '                            strKeyword = 0
                '                        End If
                '                    Else
                '                        strKeyword = 0
                '                    End If
                '                Else
                '                    strKeyword = 0
                '                End If

                '            End If
                '        Case "CONTAINS"
                '            If Not IsNumeric(strKeyword) = True Then
                '                If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '                    strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '                End If
                '            End If
                '        Case "STARTS WITH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '                strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '            End If
                '    End Select
                'End If
                'If search for E char - Stop

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Assign Search Parameters to Procedure
                strSQLStatment = "Exec pg_SearchH2HUser '" & strOption & "','" & strCriteria & "','" & strKeyword & "'," & lngOrgId

                'Execute SQL Data Adaptor
                sdaUser = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaUser.Fill(dsUser, "USER")

                'Return Data Set
                Return dsUser

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "fnUserGrid", Err.Number, Err.Description)

                Return dsUser

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaUser = Nothing

            End Try

        End Function

#End Region

#Region "Duplicate User"

        '*******************************************************************************************************************
        'Procedure Name     : fnCheckDuplicate
        'Purpose            : To Check For Duplicate
        'Arguments          : User Login
        'Return Values      : Boolean
        'Author             : Sujith Sharatchandran - 
        'Created            : 15/10/2003
        '********************************************************************************************************************
        Public Function fnCheckDuplicate(ByVal strUserLogin As String) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdDuplicate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim IsDuplicate As Boolean, intResult As Int32, strUserType As String, lngOrganisationId As Long, lngUserCode As Long

            Try
                'Get User Type
                strUserType = ASPNetContext.Session("SYS_TYPE")
                'Get Logged User Code
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
                'Get Organisation Code
                lngOrganisationId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)

                'Initialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDuplicate
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_CheckDuplicate"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_UserType", strUserType))
                    .Parameters.Add(New SqlParameter("@in_UserId", strUserLogin))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrganisationId))
                    .Parameters.Add(New SqlParameter("@out_Result", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Result", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                'Get result If Duplicate User Login Available
                intResult = cmdDuplicate.Parameters("@out_Result").Value

                If intResult > 0 Then
                    IsDuplicate = True
                Else
                    IsDuplicate = False
                End If

                Return IsDuplicate

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrganisationId, lngUserCode, "fnCheckDuplicate - Users", Err.Number, Err.Description)

            Finally

                'Destroy SQL Command Object
                cmdDuplicate = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Change Password/Authorization Code"

        Public Function fncChangePassAuth(ByVal lngUserId As Long, ByVal lngOrgId As Long, ByVal IsUser As Boolean, ByVal strPassAuth As String, ByVal strOption As String) As Boolean

            'Create Instance of SQL Command Objec
            Dim cmdPassword As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim strMode As String

            Try

                'Assign Values to variable
                strMode = IIf(IsUser, "Y", "N")
                strPassAuth = Trim(clsEncryption.Cryptography(strPassAuth))

                'Intialize SQl Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdPassword
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_ChangePassword"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Function", strOption))
                    .Parameters.Add(New SqlParameter("@in_Password", strPassAuth))
                    .Parameters.Add(New SqlParameter("@in_Mode", strMode))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnChangePassword - Users", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy SQL Command Object
                cmdPassword = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Encryption Class Object
                clsEncryption = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Password History"

        Public Function fnPasswordHistory() As Boolean

            'Create Instance of SQl Data Reader
            Dim sdrHistory As SqlDataReader

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of ASPNet Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Declare Variables
            Dim strSQLStatment As String, strPassword As String, strTxtPassword As String
            Dim lngUserId As Long, lngUserCode As Long, lngOrganisationID As Long, IsHistory As Boolean

            Try

                strTxtPassword = ASPNetContext.Request.Form("ctl00$cphContent$txtNewPwd")
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
                lngUserId = IIf(IsNumeric(ASPNetContext.Request.Form("ctl00$cphContent$hUserId")), ASPNetContext.Request.Form("ctl00$cphContent$hUserId"), 0)
                lngOrganisationID = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement To Fetch The List Of Password History
                strSQLStatment = "Exec pg_PassHistory " & lngUserId & ",PASS"

                Dim cmdHistory As New SqlCommand(strSQLStatment, clsGeneric.SQLConnection)
                sdrHistory = cmdHistory.ExecuteReader(CommandBehavior.CloseConnection)

                IsHistory = False

                'If Record Found, Check If Password Repeated - Start
                If sdrHistory.HasRows Then
                    While sdrHistory.Read
                        If Not IsDBNull(clsEncryption.Cryptography(sdrHistory("UPass"))) Then
                            strPassword = clsEncryption.Cryptography(sdrHistory("UPass"))
                            If strPassword = strTxtPassword Then
                                IsHistory = True
                                Exit While
                            End If
                        End If
                    End While
                End If
                'If Record Found, Check If Password Repated - Stop

                'Destroy SQL Command Object
                cmdHistory = Nothing

                Return IsHistory

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrganisationID, lngUserCode, "fnPasswordHistory", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQl Data Reader
                sdrHistory = Nothing

                'Destroy generic Class Object
                clsGeneric = Nothing

                'Destroy Encryption Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Get Role Count"

        '****************************************************************************************************
        'Function Name  : fnRoleCount()
        'Purpose        : Get the Role Count for the given role
        'Arguments      : Requested Role,Customer Id
        'Return Value   : Role Count
        'Author         : Sujith Sharatchandran - 
        'Created        : 23/10/2003
        '*****************************************************************************************************
        Public Function fnRoleCount(ByVal strRole As String, ByVal strStatus As String, ByVal strAction As String,
            ByVal lngUserCode As Long, Optional ByVal lngGroupId As Long = 0) As Int16

            'Create Instance of SQL Command Object  
            Dim cmdRoleCount As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASPNet Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim intRoleCount As Int16, lngUserId As Long, lngOrgId As Long

            Try

                'Assign Values
                intRoleCount = 0
                lngOrgId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)
                lngUserId = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Command Object - Start
                With cmdRoleCount
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_GetRoles"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_Option", strRole))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserCode))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .Parameters.Add(New SqlParameter("@in_Status", strStatus))
                    .Parameters.Add(New SqlParameter("@out_Count", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Count", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With
                'Execute SQL Command Object - Stop

                'Get Role Count
                intRoleCount = cmdRoleCount.Parameters("@out_Count").Value

                Return intRoleCount

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnRoleCount - clsUsers", Err.Number, Err.Description)

                Return 0

            Finally

                'Destroy SQL Command Object
                cmdRoleCount = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

        '****************************************************************************************************
        'Function Name  : fnRoleTotalCount()
        'Purpose        : Get the Total Role Count 
        'Arguments      : Organization Id
        'Return Value   : Role Count and Role Type
        'Author         : Victor Wong
        'Created        : 2007-05-22
        '*****************************************************************************************************
        Public Function fnRoleTotalCount(ByVal lngOrgId As Long, ByVal lngUserId As Long) As SqlDataReader


            Dim Param(0) As SqlParameter
            Dim drInfo As SqlDataReader
            Dim clsGeneric As New Generic
            Try
                Param(0) = New SqlParameter("@in_Org_Id", SqlDbType.Int)
                Param(0).Value = lngOrgId
                drInfo = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_QryGetRoleTotal", Param)

                Return drInfo

            Catch
                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnRoleCount - clsUsers", Err.Number, Err.Description)
            Finally
                'Destroy generic Class Object
                clsGeneric = Nothing
            End Try
            Return Nothing
        End Function

#End Region

#Region "Change Authorization Code"

        '*******************************************************************************************************************
        'Procedure Name     :   fnChangeAuthCode
        'Purpose            :   To Handle User Change Authorization Code
        'Arguments          :   User Id
        'Return Values      :   True /False
        'Author             :   Zulkefle Idris - 
        'Created            :   23/10/2003
        '********************************************************************************************************************
        Public Function fnChangeAuthCode(ByVal lngUserId As Long) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdAuthCode As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of AspNetContext Object
            Dim AspNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strAuthCode As String, lngOrganisationId As Long, lngUserCode As Long, strMode As String

            Try

                'Assign Values to variable
                strAuthCode = AspNetContext.Request.Form("ctl00$cphContent$txtNewAuthCode")
                strAuthCode = clsEncryption.Cryptography(strAuthCode)
                lngUserCode = IIf(IsNumeric(AspNetContext.Session("SYS_USERID")), AspNetContext.Session("SYS_USERID"), 0)
                lngOrganisationId = IIf(IsNumeric(AspNetContext.Session("SYS_ORGID")), AspNetContext.Session("SYS_ORGID"), 0)
                strMode = "N"

                'Intialize SQl Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdAuthCode
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_ChangePassword"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Function", "AUTH"))
                    .Parameters.Add(New SqlParameter("@in_Password", strAuthCode))
                    .Parameters.Add(New SqlParameter("@in_Mode", strMode))
                    .ExecuteNonQuery()
                End With

                'Destroy SQL Command Object
                cmdAuthCode = Nothing

                'Destroy AspNetContext Object
                AspNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Encryption Class Object
                clsEncryption = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return True

            Catch

                'Destroy SQL Command Object
                cmdAuthCode = Nothing

                'Destroy AspNetContext Object
                AspNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Log Error
                Call clsGeneric.ErrorLog(lngOrganisationId, lngUserCode, "fnChangeAuthCode - Users", Err.Number, Err.Description)

                'Destroy Encryption Class Object
                clsEncryption = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return False

            End Try

        End Function

#End Region

#Region "Validation Code History"

        Public Function fnAuthCodeHistory() As Boolean

            Dim sdrHistory As SqlDataReader                             'SQl Data Reader
            Dim clsGeneric As New MaxPayroll.Generic                       'Create Generic Class Object
            Dim clsEncryption As New MaxPayroll.Encryption                 'Create Encryption Class Object
            Dim ASPNetContext As HttpContext = HttpContext.Current      'Create ASPNet Context Object

            'Declare Variables
            Dim strSQLStatment As String, strAuthCode As String, strTxtAuthCode As String
            Dim lngUserId As Long, lngUserCode As Long, lngOrganisationID As Long, IsHistory As Boolean

            Try

                strTxtAuthCode = ASPNetContext.Request.Form("ctl00$cphContent$txtNewAuthCode")
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
                lngUserId = IIf(IsNumeric(ASPNetContext.Request.Form("ctl00$cphContent$hUserId")), ASPNetContext.Request.Form("ctl00$cphContent$hUserId"), 0)
                lngOrganisationID = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement To Fetch The List Of Password History
                strSQLStatment = "Exec pg_PassHistory " & lngUserId & ",AUTH"

                Dim cmdHistory As New SqlCommand(strSQLStatment, clsGeneric.SQLConnection)
                sdrHistory = cmdHistory.ExecuteReader(CommandBehavior.CloseConnection)

                IsHistory = False

                'If Record Found, Check If Password Repeated - Start
                If sdrHistory.HasRows Then
                    While sdrHistory.Read
                        strAuthCode = clsEncryption.Cryptography(sdrHistory("UPass"))
                        If strAuthCode = strTxtAuthCode Then
                            IsHistory = True
                            Exit While
                        End If
                    End While
                End If
                'If Record Found, Check If Password Repated - Stop

                'Destroy SQl Data Reader
                sdrHistory = Nothing

                'Destroy SQL Command Object
                cmdHistory = Nothing

                'Destroy Encryption Object
                clsEncryption = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy generic Class Object
                clsGeneric = Nothing

                Return IsHistory

            Catch

                'Destroy SQl Data Reader
                sdrHistory = Nothing

                'Destroy Encryption Object
                clsEncryption = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Log Error
                Call clsGeneric.ErrorLog(lngOrganisationID, lngUserCode, "fnAuthCodeHistory", Err.Number, Err.Description)

                'Destroy generic Class Object
                clsGeneric = Nothing

                Return IsHistory

            End Try

        End Function

#End Region

#Region "Block User"

        '****************************************************************************************************
        'Procedure Name : prLogAttemp(ByVal strOrgID As String, ByVal strUser As String)
        'Purpose        : Block User After 3 Times Fail Login
        'Return Value   : N/A
        'Author         : Nazir Erwan - 
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Function fnLogAttemp(ByVal lngOrgID As Long, ByVal lngUserID As Long) As Boolean

            'Create Instance of Generic Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of AspNetContext Object
            Dim AspNetContext As HttpContext = HttpContext.Current

            Dim blBlock As Boolean, strSQLStatment As String
            Dim intAttemp As Integer, strSesAtt As String, strCurAtt As String

            blBlock = False
            intAttemp = 0
            strCurAtt = 0
            strSesAtt = 0

            strCurAtt = lngOrgID & lngUserID
            strSesAtt = AspNetContext.Session("ATTEMP_USR")
            intAttemp = AspNetContext.Session("ATTEMP")

            If intAttemp = 0 Or IsDBNull(intAttemp) Then
                AspNetContext.Session("ATTEMP") = 1
            End If

            Select Case intAttemp

                Case 0

                    AspNetContext.Session("ATTEMP") = AspNetContext.Session("ATTEMP") + 1
                    AspNetContext.Session("ATTEMP_USR") = lngOrgID & lngUserID

                Case 3

                    If strSesAtt = strCurAtt Then
                        Try
                            'Intialize SQL Connection
                            Call clsGeneric.SQLConnection_Initialize()

                            'SQL Statement To Fetch The List Of Password History
                            strSQLStatment = "Exec pg_BlockUser " & lngOrgID & "," & lngUserID & ",'P'"
                            Dim cmdblock As New SqlCommand(strSQLStatment, clsGeneric.SQLConnection)

                            cmdblock.ExecuteScalar()

                            blBlock = True

                            'Terminate SQL Connection
                            Call clsGeneric.SQLConnection_Terminate()

                            'Clear Session Object
                            AspNetContext.Session.Clear()
                            AspNetContext.Session.Abandon()
                            AspNetContext.Session.RemoveAll()

                        Catch ex As Exception

                            'Terminate SQL Connection
                            Call clsGeneric.SQLConnection_Terminate()

                            'Log Error
                            Call clsGeneric.ErrorLog(lngOrgID, lngUserID, "fnLogAttemp", Err.Number, Err.Description)

                            'Destroy Generic Class
                            clsGeneric = Nothing
                        End Try
                    Else
                        AspNetContext.Session("ATTEMP") = 2
                        AspNetContext.Session("ATTEMP_USR") = lngOrgID & lngUserID
                    End If

                Case Else

                    If strSesAtt = strCurAtt Then
                        AspNetContext.Session("ATTEMP") = AspNetContext.Session("ATTEMP") + 1

                    Else
                        'Clear Session Object
                        AspNetContext.Session.Clear()
                        AspNetContext.Session.Abandon()
                        AspNetContext.Session.RemoveAll()
                    End If

            End Select

            'Destroy Generic Class
            clsGeneric = Nothing

            Return blBlock
        End Function
#End Region

#Region "Validate Password"
        Public Function fnValidatePassword(ByVal strPassword As String, ByVal mode As String) As String
            Dim strMsg As String

            fnValidatePassword = ""
            strMsg = ""

            strMsg = fnCheckLength(strPassword)
            If Not Len(Trim(strMsg)) = 0 Then
                fnValidatePassword = mode + strMsg
                Exit Function
            End If

            strMsg = fnCheckOnlyChar(strPassword)
            If Not Len(Trim(strMsg)) = 0 Then
                fnValidatePassword = mode + strMsg
                Exit Function
            End If

            strMsg = fnCheckOnlyNumeric(strPassword)
            If Not Len(Trim(strMsg)) = 0 Then
                fnValidatePassword = mode + strMsg
                Exit Function
            End If

            strMsg = fnCheckRepeatChar(strPassword)
            If Not Len(Trim(strMsg)) = 0 Then
                fnValidatePassword = mode + strMsg
                Exit Function
            End If

            strMsg = fnCheckSeq(strPassword)
            If Not Len(Trim(strMsg)) = 0 Then
                fnValidatePassword = mode + strMsg
                Exit Function
            End If

        End Function
        Private Function fnCheckLength(ByVal strPassword As String) As String
            Dim intLen As Integer
            Dim strMsg As String = ""
            fnCheckLength = ""
            intLen = 0
            intLen = Len(strPassword)
            If intLen < 8 Then
                strMsg = " Cannot Be Less Than 8 Characters"
            End If
            If intLen > 24 Then
                strMsg = " Cannot Be More Than 24 Characters"
            End If
            fnCheckLength = strMsg
        End Function
        Private Function fnCheckOnlyChar(ByVal strPassword As String) As String
            Dim strMsg As String
            Dim intCnt As Integer
            Dim chrTmp As String

            fnCheckOnlyChar = ""
            strMsg = ""
            chrTmp = Mid(strPassword, 1, 1)
            For intCnt = 1 To Len(strPassword)
                If IsNumeric(Mid(strPassword, intCnt, 1)) Then
                    Exit Function
                End If
            Next
            fnCheckOnlyChar = " Must Consist Combination Of Character(s) And Integer(s)"

        End Function
        Private Function fnCheckOnlyNumeric(ByVal strPassword As String) As String
            Dim strMsg As String
            Dim intCnt As Integer

            fnCheckOnlyNumeric = ""
            strMsg = ""

            For intCnt = 1 To Len(strPassword)
                If Not IsNumeric(Mid(strPassword, intCnt, 1)) Then
                    Exit Function
                End If
            Next
            fnCheckOnlyNumeric = " Must Consist Combination Of Character(s) And Integer(s)"
        End Function
        Private Function fnCheckRepeatChar(ByVal strPassword As String) As String
            Dim strMsg As String = ""
            Dim intCnt As Integer

            fnCheckRepeatChar = ""
            For intCnt = 1 To Len(Trim(strPassword)) - 1
                If Mid(strPassword, intCnt, 1) = Mid(strPassword, intCnt + 1, 1) Then
                    strMsg = " Cannot Use Repeated Characters"
                    Exit For
                End If
            Next
            fnCheckRepeatChar = strMsg
        End Function
        Private Function fnCheckSeq(ByVal strPassword As String) As String
            Dim strMsg As String = ""
            Dim intCnt As Integer

            fnCheckSeq = ""
            For intCnt = 1 To Len(strPassword) - 1
                If Asc(Mid(strPassword, intCnt, 1)) + 1 = Asc(Mid(strPassword, intCnt + 1, 1)) Then
                    strMsg = " Cannot Use Sequences Characters"
                    Exit For
                End If
            Next
            fnCheckSeq = strMsg

        End Function
#End Region

#Region "Reset Password"

        '*******************************************************************************************************************
        'Procedure Name     :   fnResetPassword
        'Purpose            :   To reset CA password
        'Arguments          :   strNewPassword,lngUserId
        'Return Values      :   True/False
        'Author             :   Zulkefle Idris - T-Melmax
        'Created            :   08/03/2004
        '********************************************************************************************************************
        Public Function fnResetPassword(ByVal strNewPassword As String, ByVal strMode As String, ByVal lngUserId As Long)

            'Create Instace of SQL Command Object
            Dim cmdPassword As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of AspNetContext Object
            Dim AspNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngOrganisationId As Long, lngUserCode As Long

            Try

                'Assign Values to variable
                strNewPassword = clsEncryption.Cryptography(strNewPassword)
                lngUserCode = IIf(IsNumeric(AspNetContext.Session("SYS_USERID")), AspNetContext.Session("SYS_USERID"), 0)
                lngOrganisationId = IIf(IsNumeric(AspNetContext.Session("SYS_ORGID")), AspNetContext.Session("SYS_ORGID"), 0)

                'Intialize SQl Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdPassword
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_ResetPassword"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Mode", strMode))
                    .Parameters.Add(New SqlParameter("@in_NewPassword", strNewPassword))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrganisationId, lngUserCode, "fnResetPassword - clsUsers", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy SQL Command Object
                cmdPassword = Nothing

                'Destroy AspNetContext Object
                AspNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Encryption Class Object
                clsEncryption = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Authorization Lock"

        '****************************************************************************************************
        'Function Name  : prUpload
        'Purpose        : Upload File
        'Arguments      : Organisation Id, User Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/06/2004
        '*****************************************************************************************************
        Public Sub prcAuthLock(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strModule As String)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdAuthLock As New SqlCommand

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Execute pg_BlockUser " & lngOrgId & "," & lngUserId & ",'" & strModule & "'"

                'SQL Command
                cmdAuthLock.Connection = clsGeneric.SQLConnection
                cmdAuthLock.CommandText = strSQL
                cmdAuthLock.ExecuteScalar()

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "", Err.Number, "prcAuthLock - clsUsers")

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdAuthLock = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Detail Log"

        '****************************************************************************************************
        'Function Name  : prcDetailLogs()
        'Purpose        : Detail Log
        'Arguments      : User Id, Transaction Type, Status
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/11/2004
        '*****************************************************************************************************
        Public Sub prcDetailLog(ByVal lngUserId As Long, ByVal strTransactionType As String, ByVal strStatus As String)

            'Create Instance of SQL Command Object
            Dim cmdDetailLog As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDetailLog
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_DetailLog"
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_TransType", strTransactionType))
                    .Parameters.Add(New SqlParameter("@in_Status", strStatus))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, lngUserId, "clsUsers - prcDetailLog", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdDetailLog = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Modify Log"

        '****************************************************************************************************
        'Function Name  : prcDetailLogs()
        'Purpose        : Detail Log
        'Arguments      : User Id, Transaction Type, Status
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/11/2004
        '*****************************************************************************************************
        Public Sub prcModifyLog(ByVal lngUserId As Long, ByVal lngOrgId As Long, ByVal strTransModule As String,
                ByVal strTransField As String, ByVal strNewData As String, ByVal strOldData As String,
                    ByVal lngModifyBy As Long, ByVal strType As String, ByVal intApprId As Int32)

            'Create Instance of SQL Command Object
            Dim cmdModifyLog As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdModifyLog
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_ModifyLog"
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_TransModule", strTransModule))
                    .Parameters.Add(New SqlParameter("@in_TransField", strTransField))
                    .Parameters.Add(New SqlParameter("@in_NewData", strNewData))
                    .Parameters.Add(New SqlParameter("@in_OldData", strOldData))
                    .Parameters.Add(New SqlParameter("@in_ModifyBy", lngModifyBy))
                    .Parameters.Add(New SqlParameter("@in_Type", strType))
                    .Parameters.Add(New SqlParameter("@in_ApprId", intApprId))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, lngUserId, "clsUsers - prcModifyLog", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdModifyLog = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Delete Log"

        '****************************************************************************************************
        'Function Name  : prcDetailLogs()
        'Purpose        : Detail Log
        'Arguments      : User Id, Transaction Type, Status
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/11/2004
        '*****************************************************************************************************
        Public Sub prcDeleteLog(ByVal lngUserId As Long, ByVal lngDeleteBy As Long, ByVal lngApproverId As Long)

            'Create Instance of SQL Command Object
            Dim cmdDeleteLog As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDeleteLog
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_DeleteLog"
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_DeleteBy", lngDeleteBy))
                    .Parameters.Add(New SqlParameter("@in_ApproverId", lngApproverId))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, lngUserId, "clsUsers - prcDeleteLog", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdDeleteLog = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Bank Admin Search"

        '****************************************************************************************************
        'Function Name  : fncBankAdmin()
        'Purpose        : Search Records and return DataSet
        'Arguments      : Search Option, Search Criteria,Search Keyword,User Type
        'Return Value   : DataSet
        'Author         : Sujith Sharatchandran - 
        'Created        : 23/11/2004
        '*****************************************************************************************************
        Public Function fncBankAdmin(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strOption As String, ByVal strCriteria As String, ByVal strKeyword As String, ByVal strUserType As String) As Data.DataSet

            'Create Instance of Data Set
            Dim dsBankAdmin As New Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adapter
            Dim sdaBankAdmin As New SqlDataAdapter

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec pg_BankAdmin '" & strOption & "','" & strCriteria & "','" & strKeyword & "','" & strUserType & "'"

                'Execute SQL Data Adapter
                sdaBankAdmin = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaBankAdmin.Fill(dsBankAdmin, "BANKADMIN")

                'return Data Set
                Return dsBankAdmin

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsUsers-fncBankAdmin", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaBankAdmin = Nothing

                'Destroy Instance of Data Set
                dsBankAdmin = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Password/Authorize Lock Track"

        '****************************************************************************************************
        'Procedure Name : prcLockHistory()
        'Purpose        : Track Lock History
        'Arguments      : User Id, Module
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/10/2003
        '*****************************************************************************************************
        Public Sub prcLockHistory(ByVal lngUserId As Long, ByVal strModule As String)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdLockHistory As New SqlCommand

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdLockHistory
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "PG_PassLock"
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Module", strModule))
                    .ExecuteNonQuery()
                End With

            Catch ex As Exception

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdLockHistory = Nothing

            End Try

        End Sub

#End Region

#Region "Session Check"

        '****************************************************************************************************
        'Procedure Name : fncSessionCheck()
        'Purpose        : Insert\Check\Delete Session
        'Arguments      : User Id, Option
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/01/2006
        '*****************************************************************************************************
        Public Function fncSessionCheck(ByVal strOption As String, ByVal intUserId As Int32) As Boolean

            'create instance of generic class object
            Dim clsGeneric As New MaxPayroll.Generic

            'create instance of sql command object
            Dim cmdSessionCheck As New SqlCommand

            'variable declarations
            Dim intResult As Int32

            Try

                'intialise sql connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdSessionCheck
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_SessionCheck"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_UserId", intUserId))
                    intResult = .ExecuteScalar()
                End With

                If intResult > 0 Then
                    Return True
                Else
                    Return False
                End If

            Catch Ex As Exception

                'log error
                Call clsGeneric.ErrorLog(0, intUserId, "fncSessionCheck - clsUsers", Err.Number, Ex.Message)

                Return False

            Finally

                'terminate sql connection
                Call clsGeneric.SQLConnection_Terminate()

                'destroy instance of generic class object
                clsGeneric = Nothing

                'destroy instance of sql command object
                cmdSessionCheck = Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
