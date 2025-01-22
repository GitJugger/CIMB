Option Strict Off
Option Explicit On

Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports MaxPayroll.Encryption
Imports System.Web.UI


Namespace MaxPayroll


    Partial Class PG_Index
        Inherits System.Web.UI.Page
        Dim objResxMgr As New MaxPayroll.SatelliteResx.ResourceManagerEx

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            'objResxMgr.TextBind(Me)
            'objResxMgr.TextBind(lblHeader, "longa5")
        End Sub

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Session.LCID = 2057
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strRequest As String

            Try

                'Flag to know Try Again
                strRequest = Request.QueryString("ErrFlag")

                If Not Page.IsPostBack Then

                    'Dim body As HtmlGenericControl
                    'body = CType(Master.FindControl("body"), HtmlGenericControl)
                    'body.Attributes.Add("onload", "javascript:Page_Init();")
                    If strRequest = "" Then
                        Session.Clear()
                        Session.Abandon()
                        Session.RemoveAll()
                    End If

                    'If Len(lblHeader.Text) = 0 Then
                    '   lblHeader.Text = gc_Const_ApplicationName & " Business Banking SignOn"
                    'End If

                End If

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Submit Page"

        '****************************************************************************************************
        'Procedure Name : Submit_Page()
        'Purpose        : Execute At the time of page Submission
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/10/2003
        '*****************************************************************************************************
        Protected Sub prSave(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click

            'Create Instance of Generic Class 
            Dim clsGeneric As New MaxPayroll.Generic

            'Delcare Variables
            Dim strResult As String, lngLogId As Long, intGrpCount As Int16
            Dim strUserType As String = ""

            Try

                intGrpCount = 0
                strResult = fnLogin(intGrpCount, strUserType)

                If UCase(strResult) = "OK" Then
                    'Check If Any other User Than Uploader, Reviewer, Authorizer & Interceptor
                    Session("NoMenu") = "False"
                    If strUserType = "U" Or strUserType = "R" Or strUserType = "A" Or strUserType = "RD" Then 'Or strUserType = "BD"
                        lngLogId = clsGeneric.fnWriteLog(0)
                        Session("LOG_ID") = lngLogId
                        Server.Transfer("PG_GroupChange.aspx", False)
                        Exit Try
                    Else
                        lngLogId = clsGeneric.fnWriteLog(0)
                        Session("LOG_ID") = lngLogId
                        Server.Transfer("PG_Inbox.aspx")
                        Exit Try
                    End If
                Else
                    'to log the userlogin unsuccesful attempts for BANK
                    'If strUserType = "BA" Or strUserType = "BS" Or strUserType = "BU" Or strUserType = "IU" Then
                    Call clsGeneric.fnWriteFailedLog(0, 0, strResult)
                    'End If
                    Session("NoMenu") = "True"
                    'Session.Clear()
                    'Session.Abandon()
                    'Session.RemoveAll()
                    Server.Transfer("PG_Message.aspx?MsgTyp=Login&ErrMsg=" & strResult, False)
                    Exit Try
                End If

            Catch ex As Exception

                Dim msg As String = ex.Message

            Finally

                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Login Check"

        '****************************************************************************************************
        'Procedure Name : fnLogin()
        'Purpose        : Login Validation
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 08/10/2003
        '*****************************************************************************************************
        Private Function fnLogin(ByRef intGrpCount As Int16, ByRef strUseType As String) As String

            'Create Instance of SQL Data Reader
            Dim sdrLogin As SqlDataReader

            'Create Instance of SQL Command
            Dim cmdLogin As New SqlCommand

            'Create Instance of User Class Object
            Dim clsUser As New MaxPayroll.clsUsers

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declaration
            Dim dtUserExpiryDt As Date, dtPassExpiryDt As Date, strUserType As String, strAuthChange As String
            Dim strUserStatus As String, strOrgStatus As String, IsBlock As Boolean, strChangePassword As String
            Dim strBody As String, lngOrgId As Long, lngUserId As Long, strUserName As String
            Dim strUserRole As String, strSubject As String, intApproved As Int16, strPassLock As String, strVerify As String
            Dim strOrgId As String, strUserLogin As String, strPassword As String, strtxtPassword As String, strModule As String, strUserLoginName As String
            Dim bUseToken As Boolean, IsOnline As Int16

            Try

                'Read Values From Login Form
                'strOrgId = Request.Form("txtOrgId")           'Organisation ID
                'strUserLogin = Request.Form("txtUserId")      'User Login
                'strtxtPassword = Request.Form("txtPassword")  'User Password
                strOrgId = txtOrgId.Text
                strUserLogin = txtUserId.Text
                strtxtPassword = txtPassword.Text

                'Organisation Id Validations - START
                If IsNumeric(strOrgId) = False Then
                    Return "Invalid SignOn Details. Please try again."
                    'Check If Organisation ID Length is equal to 7
                ElseIf Len(Trim(strOrgId)) <> 6 Then
                    Return "Invalid SignOn Details. Please try again."
                    'Check If E is Prefixed to the Organisation ID
                    'ElseIf Not Left(strOrgId, 1) = gc_Const_CCPrefix Then
                    'Return "Invalid SignOn Details. Please try again."
                    'Else
                    'strOrgId = strOrgId.Replace("M", "")
                End If
                'Organisation Id Validations - STOP

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute Stored Procedure
                With cmdLogin
                    .Connection = clsGeneric.SQLConnection                              'SQL Connection
                    .CommandText = "pg_Login"                                           'Stored Procedure
                    .CommandType = CommandType.StoredProcedure                          'Command Type is Stored Procedure
                    .Parameters.Add(New SqlParameter("@in_OrgId", strOrgId))            'Organisation Id
                    .Parameters.Add(New SqlParameter("@in_UserLogin", strUserLogin))    'User Id
                End With

                'Execute SQl Data Reader
                sdrLogin = cmdLogin.ExecuteReader()

                'Read Values
                If sdrLogin.HasRows Then
                    sdrLogin.Read()
                    lngOrgId = sdrLogin("OrgId")                                           'Organisation Id
                    lngUserId = sdrLogin("UserId")                                        'User Id
                    strUserType = CStr(sdrLogin("Type"))                                          'User Type
                    strUseType = strUserType                                                'User Type
                    strUserRole = CStr(sdrLogin("UROLE") & "")                                      'User Role
                    strUserName = CStr(sdrLogin("UNAME"))                                         'User Name
                    strUserLoginName = CStr(sdrLogin("User_Login")) 'User Login Name
                    intApproved = CShort(sdrLogin("UAPPR"))                                         'User Approved
                    intGrpCount = CShort(sdrLogin("GCount"))                                        'Group Count
                    strPassword = CStr(sdrLogin("Password"))                                      'Password
                    strPassLock = CStr(sdrLogin("UPLOCK"))                                        'Password Lock Status
                    strChangePassword = CStr(sdrLogin("Change"))                                  'Password Change Required
                    strUserStatus = CStr(sdrLogin("UserStatus"))                                  'Account Status
                    strOrgStatus = CStr(sdrLogin("OrgStatus"))                                    'Organisation Status
                    dtUserExpiryDt = CDate(sdrLogin("UserExp"))
                    'Format(sdrLogin("UserExp"), "dd/MM/yyyy")              'User expiry
                    strAuthChange = CStr(sdrLogin("UAChange"))
                    If Not IsDBNull(sdrLogin("PassExp")) Then
                        dtPassExpiryDt = CDate(sdrLogin("PassExp"))
                    Else
                        dtPassExpiryDt = Date.Now
                    End If
                    IsOnline = CShort(sdrLogin("IsOnline"))
                    'dtPassExpiryDt = IIf(Not IsDBNull(sdrLogin("PassExp")), CDate(sdrLogin("PassExp")), Date.Now) 'Format(sdrLogin("PassExp"), "dd/MM/yyyy"), Format(Now, "dd/MM/yyyy"))
                    'bUseToken = CBool(sdrLogin("User_UseToken"))
                    'busetoken = 
                    'bUseToken = clsCommon.fncGetOrgTokenSetting(lngOrgId, lngUserId)

                    Session("SYS_USERID") = lngUserId
                    Session("SYS_ORGID") = lngOrgId
                    Session("SYS_TYPE") = strUserType

                    sdrLogin.Close()
                Else
                    sdrLogin.Close()
                    Return "Invalid SignOn Details. Please try again."
                End If



                'Admin & Customer Log In Checking - START
                strModule = System.Configuration.ConfigurationManager.AppSettings("USER")
                If strModule = "BANK" Then
                    If Not strOrgId = "100000" Then
                        Return "Invalid Sign On Details. Please try again."
                    End If
                ElseIf strModule = "CUSTOMER" Then
                    If strOrgId = "100000" And Not strUserType = "BO" Then
                        Return "Invalid Sign On Details. Please try again."
                    End If
                Else
                    Return "Invalid Sign On Details. Please try again."
                End If
                'Admin & Customer Log In Checking - STOP

                'Check If User Approved
                If Not intApproved = 2 Then
                    Return "Your Account is not approved by your " & gc_UT_SysAuthDesc & "."
                End If

                'Validate Organisation Status
                If strOrgStatus = "C" Then
                    Return "Your Organization is Inactive/Disable."
                ElseIf strOrgStatus = "D" Then
                    Return "Your Organisation Subscription has been Cancelled."
                End If

                'Validate User Status
                If strUserStatus = "C" Or strPassLock = "Y" Then
                    Return "Your Account is Inactive/Locked Out."
                ElseIf strUserStatus = "D" Then
                    Return "Your Account has been deleted."
                End If

                'Decrypt Password
                strPassword = clsEncryption.Cryptography(strPassword)

                'Get verification Type
                strVerify = clsCommon.fncBuildContent("Verification", "", lngOrgId, lngUserId)

                'Validate Entered Password and DB Password
                If strPassword = strtxtPassword Then
                    'Check If Already Online
                    'If IsOnline = 1 Then
                    '    Return "User is already online,concurrent user are not allowed. Please Contact Administrator !"
                    'End If
                    'Check If Account Expired 
                    If dtUserExpiryDt < Format(Now, "dd/MM/yyyy") Then
                        Return "Account expired, Please Contact " & gc_Const_CompanyName & " Registration Center/" & gc_UT_SysAdminDesc & "."
                    Else
                        'Assign Session Values - START
                        Session("EXP_MSG") = "Y"                                                        'Expiry Message
                        Session("SYS_TYPE") = strUserType                                               'User Type
                        Session("SYS_ORGID") = lngOrgId                                                 'Organisation Id
                        Session("SYS_USERID") = lngUserId                                               'User Id
                        Session("EXP_DT") = dtUserExpiryDt                                              'Expiry Date
                        Session("AUTH_CHNG") = strAuthChange                                            'Authorization Change Status
                        Session("PWD_CHNG") = strChangePassword                                         'Password Change Status
                        Session(gc_Ses_VerificationType) = strVerify                                               'Verification Type
                        Session("PWD_EXP") = IIf(dtPassExpiryDt < Format(Now, "dd/MM/yyyy"), "Y", "N")  'Password Expiry Date
                        Session("SYS_WARN") = "N"                                                       'Warning Message Flag
                        Session(gc_Ses_Token) = bUseToken 'Token Setting
                        Session(gc_Ses_UserLoginName) = strUserLoginName

                        Session("SYS_USERNAME") = strUserName
                        'Check if user already logged in - start
                        Dim intInterval As Int16 = System.Configuration.ConfigurationManager.AppSettings("INTERVAL")
                        Dim strTerminal As String = clsGeneric.Login(lngOrgId, lngUserId, Now, intInterval)
                        If strTerminal = "Y" Then
                            Dim strErrMessage As String = "You have not Signed-out properly from the last session or "
                            strErrMessage = strErrMessage & "already Signed-on from a different terminal\browser."
                            strErrMessage = strErrMessage & " Please try again later."
                            'Session.Clear()
                            Return strErrMessage
                        End If
                        'Check if user already logged in - stop

                        Return "OK"
                        'Assign Session Values - STOP
                    End If
                Else
                    'Assign Session Values - START

                    Session("SYS_ORGID") = lngOrgId                                                 'Organisation Id
                    Session("SYS_USERID") = lngUserId                                               'User Id
                    Session("SYS_TYPE") = strUserType

                    'User Type

                    Session("SYS_USERNAME") = strUserName
                    'Assign Session Values - STOP

                    'Check for Invalid Attempts
                    IsBlock = clsUser.fnLogAttemp(lngOrgId, lngUserId)
                    If IsBlock Then
                        strSubject = strUserName & " (" & strUserRole & ") Locked/Inactive."
                        strBody = strUserName & " (" & strUserRole & ")" & " has been Locked/Inactive on " & Now() & "due to Invalid Password attempts. Please change the Password and activate the user."
                        Call clsUser.prcLockHistory(lngUserId, "P")
                        'Session.Clear()
                        'Session.Abandon()
                        'Session.RemoveAll()
                        Return "Your account has been locked due to invalid attempts."
                    Else
                        'Session.Clear()
                        'Session.Abandon()
                        'Session.RemoveAll()
                        Return "Invalid SignOn Details. Please try again."
                    End If


                End If

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnLogin - PG_Index.aspx.vb", Err.Number, Err.Description)
                Return "Application Error. Please try again."

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Data Reader
                sdrLogin = Nothing

                'Destroy Instance of SQL Command Object
                cmdLogin = Nothing

                'Destroy Instance of User Class Object
                clsUser = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class
                clsGeneric = Nothing

                'Destroy Instance of Encryption Class
                clsEncryption = Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace

