Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsCommon
Imports MaxPayroll.Encryption


Namespace MaxPayroll


    Partial Class PG_ChangePassword
        Inherits clsBasePage

#Region " Global Variable Declaration "
        Dim blRedirect As Boolean, blSysUser As Boolean
        Dim clsEncryption As New MaxPayroll.Encryption
        Private ReadOnly Property rq_lngUserID() As Long
            Get
                If Request.QueryString("ID") IsNot Nothing Then
                    Return CLng(clsEncryption.Cryptography(Request.QueryString("ID")))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_bShowSignOut() As Boolean
            Get
                If IsNothing(Request.QueryString("SignOut")) Then
                    Return False
                Else
                    If CStr(Request.QueryString("SignOut")) = "1" Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End Get
        End Property
        Private ReadOnly Property rq_FromH2H() As Boolean
            Get
                If IsNothing(Request.QueryString("FromH2H")) Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property
        Private ReadOnly Property rq_OrgId() As Long
            Get
                If Request.QueryString("OrgId") IsNot Nothing Then
                    Return CLng(clsEncryption.Cryptography(Request.QueryString("OrgId")))
                Else
                    Return -1
                End If
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
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Data Row Object
            Dim drUsers As System.Data.DataRow

            'Create Instance of Users Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of COmmon Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Data Set Object
            Dim dsUsers As New System.Data.DataSet

            'Variable Declarations
            Dim strUserLogin As String = ""
            Dim dtResetDate As String = ""

            Dim strPassReset As String = ""
            Dim strPwdExp As String = ""
            Dim strPassword As String = ""
            Dim strPassChange As String = ""
            Dim strPassLock As String = ""
            Dim strUserStatus As String = ""
            Dim dtCurrDate As Date

            Try

                dtCurrDate = Today
                hUserType.Value = ss_strUserType
                hOrgID.Value = rq_OrgId
                If Not Page.IsPostBack Then
                    'BindBody(body)
                    'inbtnSignOut.Visible = rq_bShowSignOut
                    If rq_bShowSignOut Then
                        lblHeading.Text = "Please change the Password for first time login."
                    End If

                    'If Password Change for Different User

                    If rq_lngUserID > 0 Then
                        btnBack.Visible = True
                        hUserId.Value = rq_lngUserID
                        dsUsers = clsUsers.fncUserDetails(rq_lngUserID, 0, 0)
                        'If Change Password for logged in User
                    Else
                        btnBack.Visible = False
                        hUserId.Value = ss_lngUserID
                        dsUsers = clsUsers.fncUserDetails(ss_lngUserID, 0, 0)
                    End If

                    'Get User Details - START
                    strPwdExp = Session(gc_Ses_PwdExpiryDate)                                                                                              'User Password Expiry Status
                    strPassChange = Session(gc_Ses_PwdChgStatus)                                                                                         'User Password Change Period Status
                    For Each drUsers In dsUsers.Tables("USER").Rows
                        strUserLogin = drUsers("ULogin")
                        strUserStatus = IIf(drUsers("UStatus") = "A", "Y", "N")                                                                 'User Status
                        strPassword = IIf(Not IsDBNull(drUsers("UPwd")), drUsers("UPwd"), "")                                                   'User Password
                        strPassLock = IIf(Not IsDBNull(drUsers("ULock")), drUsers("ULock"), "N")                                                'User Password Lock Status
                        strPassReset = IIf(Not IsDBNull(drUsers("UReset")), drUsers("UReset"), "N")                                             'User Password Reset Status
                        If IsDBNull(drUsers("UResetDt")) Then
                            dtResetDate = Format(drUsers("UCreateDt"), "dd/MM/yyyy")
                        Else
                            dtResetDate = Format(drUsers("UResetDt"), "dd/MM/yyyy")
                        End If
                    Next
                    hUserLogin.Value = strUserLogin
                    hUserPwdReset.Value = strPassReset
                    hUserPwdResetDt.Value = dtResetDate
                    'Get User Details - END

                    'Force Change Password for Logged User
                    If ss_lngUserID > 0 And rq_lngUserID = 0 Then

                        'Show Buttons
                        trOldPwd.Visible = True

                        'If Password Change Status 
                        If strPassChange = "Y" Then
                            'btnBack.Visible = False
                            lblMessage.Text = "You are requested to change your Password since this is your first SignOn."
                            Exit Try
                            'If Password Change Period
                        ElseIf strPassChange = "N" Then
                            If strPwdExp = "Y" Then
                                'btnBack.Visible = False
                                lblMessage.Text = "You are requested to change your Password since your Password change period has reached."
                                Exit Try
                            End If
                        End If

                        'Check Password Change More than once in a day
                        If CDate(hUserPwdResetDt.Value) = dtCurrDate Then
                            If strPassReset = "N" Then
                                btnSave.Enabled = False
                                lblMessage.Text = "You are not allowed to Change your Password more than once Per-Day."
                                Exit Try
                            End If
                        End If

                        'If Different User
                    ElseIf rq_lngUserID > 0 Then

                        'Hide Buttons
                        trOldPwd.Visible = False

                        'If Password Not Locked out
                        If strPassLock = "N" Then
                            btnBack.Visible = True
                            btnSave.Enabled = False
                            lblMessage.Text = "Password can be changed only if the User Password has been Locked out"
                        End If

                    End If

                End If

            Catch

                'Log Error
                LogError("Page Load - PG_ChangePassword")
            Finally

                'Destroy Instance Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance User Class Object
                clsUsers = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destoy Instance Data Row
                drUsers = Nothing

                'Destroy Instance DataSet
                dsUsers = Nothing

            End Try

        End Sub

#End Region

#Region "Page Submit"

        '****************************************************************************************************
        'Procedure Name : Page_Submit()
        'Purpose        : Page Submit
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        '*****************************************************************************************************
        Public Sub Page_Submit(ByVal O As System.Object, ByVal E As EventArgs) Handles btnSave.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Comman Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim intAttempts As Int16, IsUser As Boolean, strPassChange As String
            Dim strPassPolicy As String, IsRepeated As Boolean, IsChange As Boolean
            Dim dtCurrDate As Date, strPassAuth As String

            Try

                dtCurrDate = Today()                                                            'Get Current Date
                strPassChange = Session(gc_Ses_PwdChgStatus)                                             'Get Password Change Status

                'Check If Same User
                IsUser = IIf(ss_lngUserID = rq_lngUserID, True, False)

                If rq_lngUserID = 0 Then
                    IsUser = True
                End If

                'If Password Changed More than Once a Day
                If ss_lngUserID = rq_lngUserID And hUserPwdReset.Value = "N" Then
                    If IsDate(hUserPwdResetDt.Value) Then
                        If hUserPwdResetDt.Value = dtCurrDate Then
                            lblMessage.Text = "You are not allowed to Change Password more than once Per-Day."
                            Exit Try
                        End If
                    End If
                End If

                'If Old Password Match
                If ss_lngUserID = rq_lngUserID Then
                    'Get Count of Invalid Password Attempts
                    intAttempts = IIf(IsNumeric(Session(gc_Ses_PwdInvalid)), Session(gc_Ses_PwdInvalid), 0)
                    'Get Orginal Password
                    strPassAuth = clsCommon.fncPassAuth(rq_lngUserID, "P", ss_lngOrgID)
                    'If Password does not match and not reached 3 invalid attempts
                    If Not intAttempts = 2 Then
                        If Not strPassAuth = txtOldPwd.Text Then
                            lblMessage.Text = "Your Old Password Does Not Match."
                            'Incerement Counter
                            intAttempts = intAttempts + 1
                            Session(gc_Ses_PwdInvalid) = intAttempts
                            Exit Try
                        End If
                    ElseIf intAttempts = 2 Then
                        If Not strPassAuth = txtOldPwd.Text Then
                            lblMessage.Text = "Your account has been locked due to invalid attempts. Please contact " & gc_UT_SysAdminDesc & "."
                            btnSave.Enabled = False
                            'Block User Account
                            Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "P")
                            'update for lock out report
                            Call clsUsers.prcLockHistory(ss_lngUserID, "P")
                            'Clear Session
                            Session.Clear()
                            Session.Abandon()
                            Session.RemoveAll()
                            ''Clear the Menu
                            'Response.Write("<script language='JavaScript'>")
                            'Response.Write("parent.frames['Menu'].location.reload();")
                            'Response.Write("</script>")
                            Exit Try
                        End If
                    End If
                End If

                'Password Policy
                strPassPolicy = clsUsers.fnValidatePassword(txtNewPwd.Text, "New Password")
                If Len(Trim(strPassPolicy)) > 0 Then
                    lblMessage.Text = strPassPolicy
                    Exit Try
                End If

                strPassAuth = clsCommon.fncPassAuth(ss_lngUserID, "P", ss_lngOrgID)
                'If Old Password Does Not Match
                If Not strPassAuth = txtOldPwd.Text Then
                    lblMessage.Text = "Your Old Password Does Not Match."
                    'Incerement Counter
                    'intAttempts = intAttempts + 1
                    'Session(gc_Ses_PwdInvalid) = intAttempts
                    Exit Try
                End If

                'If Old Password and New password same
                If ss_lngUserID = rq_lngUserID Then
                    If txtOldPwd.Text = txtNewPwd.Text Then
                        lblMessage.Text = "Old Password and New Password cannot be the same."
                        Exit Try
                    End If
                End If

                'If Password and User Login Same
                If txtNewPwd.Text = hUserLogin.Value Then
                    lblMessage.Text = "User Id and Password cannot be the same"
                    Exit Try
                End If

                'If Password Previously Used
                IsRepeated = clsUsers.fnPasswordHistory()
                If IsRepeated Then
                    lblMessage.Text = "Password has been used previously."
                    Exit Try
                End If

                'If Password and Auth Code is same
                strPassAuth = clsCommon.fncPassAuth(rq_lngUserID, "A", ss_lngOrgID)
                If txtNewPwd.Text = strPassAuth Then
                    lblMessage.Text = "Password cannot be same as Validation code."
                    Exit Try
                End If

                'If New & Confirm Password does not Match
                If Not txtNewPwd.Text = txtConPwd.Text Then
                    lblMessage.Text = "New Password and Confirm Password does not match."
                    Exit Try
                End If

                'Change Password
                If rq_lngUserID = 0 Then
                    IsChange = clsUsers.fncChangePassAuth(ss_lngUserID, ss_lngOrgID, IsUser, txtNewPwd.Text, "PASS")
                Else
                    IsChange = clsUsers.fncChangePassAuth(rq_lngUserID, ss_lngOrgID, IsUser, txtNewPwd.Text, "PASS")
                End If

                If IsChange Then
                    If strPassChange = "Y" Then

                        Session(gc_Ses_PwdChgStatus) = "N"
                        Server.Transfer("PG_Message.aspx?MsgTyp=PWD", False)
                        Exit Try
                    ElseIf Session(gc_Ses_PwdExpiryDate) = "Y" Then

                        Session(gc_Ses_PwdExpiryDate) = "N"
                        Server.Transfer("PG_Message.aspx?MsgTyp=PWD", False)
                        Exit Try
                    Else
                        Session(gc_Ses_PwdChgStatus) = "N"
                        btnSave.Enabled = False
                        lblMessage.Text = "Password changed Successfully."
                    End If
                Else
                    lblMessage.Text = "Password change failed."
                End If

            Catch

                'Log Error
                If Err.Description <> "Thread was being aborted." Then
                    LogError("Page Submit-Change Password")
                End If

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try


        End Sub

#End Region
        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function

    End Class

End Namespace
