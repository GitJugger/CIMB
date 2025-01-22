Namespace MaxPayroll

    Partial Class PG_ChangeAuthCode
        Inherits clsBasePage

#Region " Global Variable Declaration "
        Dim blSysUser As Boolean = True
        Dim clsEncryption As New MaxPayroll.Encryption
        Private ReadOnly Property rq_lngUserID() As Long
            Get
                If Request.QueryString("ID") IsNot Nothing Then
                    Return clsEncryption.Cryptography(Request.QueryString("ID"))
                Else
                    LogError("pg_ChangeAuthCode - rq_lngUserID")
                    Return -1
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
        Private ReadOnly Property rq_OrgID() As Long
            Get
                If Request.QueryString("OrgId") IsNot Nothing Then
                    Return CLng(clsEncryption.Cryptography(Request.QueryString("OrgId")))
                Else
                    Return -1
                End If
            End Get
        End Property
#End Region

#Region " Page Load "


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

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Users Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Data Set Object
            Dim dsUsers As New System.Data.DataSet

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strAuthLock As String = ""
            Dim strAuthChange As String = ""
            Dim strPassAuth As String = ""
            Dim bIsAuthCodeEmpty As Boolean = False



            Try

                trMsg.Visible = True
                hUserType.Value = ss_strUserType
                hOrgID.Value = rq_OrgID
                If Not Page.IsPostBack Then
                    'BindBody(body)
                    inbtnSignOut.Visible = rq_bShowSignOut
                    'If rq_bShowSignOut Then
                    '   lblHeading.Text = "Please change the Authorization Code for first time login."
                    'End If
                    'To display or not display the old Authorization Code field
                    prcOldAuthCodeDisplay()
                    'If Change Auth Code for Different User
                    If rq_lngUserID > 0 Then
                        btnBack.Visible = True
                        hUserId.Value = rq_lngUserID
                        dsUsers = clsUsers.fncUserDetails(rq_lngUserID, 0, 0)
                        'If Change Auth Code for logged in User
                    Else
                        btnBack.Visible = False
                        hUserId.Value = ss_lngUserID
                        dsUsers = clsUsers.fncUserDetails(ss_lngUserID, 0, 0)
                    End If

                    'Get User Details - Start
                    For Each drUsers In dsUsers.Tables("USER").Rows
                        hUserLogin.Value = drUsers("ULogin")                                             'User Id
                        strAuthLock = IIf(Not IsDBNull(drUsers("UALock")), drUsers("UALock"), "N")       'Authorization Code Lock Status
                        strAuthChange = IIf(Not IsDBNull(drUsers("UAChange")), drUsers("UAChange"), "N") 'Authorization Code Change Status
                    Next
                    'Get User Details - Stop

                    'If Same User
                    If ss_lngUserID > 0 And (rq_lngUserID = 0 Or rq_bShowSignOut) Then

                        'Show Old Authorization Code
                        'trOldAuth.Visible = True
                        Select Case ss_strUserType
                            Case gc_UT_SysAdmin, gc_UT_BankUser, gc_UT_BankAdmin, gc_UT_BankAuth
                                If strAuthChange = "Y" Then
                                    strPassAuth = clsCommon.fncPassAuth(ss_lngUserID, "A", ss_lngOrgID)
                                    If strPassAuth = "" Then
                                        hAuth.Value = "Y"
                                        trOldAuth.Visible = False
                                        lblMessage.Text = "You are requested to set your Validation Code."
                                        Exit Try
                                    End If
                                End If
                        End Select

                        If strAuthChange = "Y" Then
                            trMsg.Visible = True
                            lblMessage.Text = "You are requested to change your Validation Code since this is your first time SignOn."
                            Exit Try
                        End If

                        If strAuthLock = "Y" Then
                            trMsg.Visible = True
                            If ss_strUserType = gc_UT_SysAuth OrElse ss_strUserType = gc_UT_SysAdmin Then
                                lblMessage.Text = "Your Validation Code has been locked out. Please contact your " & gc_UT_BankAdminDesc & "."
                            Else
                                lblMessage.Text = "Your Validation Code has been locked out. Please contact your " & gc_UT_SysAdminDesc & "."
                            End If

                            btnSave.Enabled = False
                            btnClear.Disabled = True
                            Exit Try
                        End If

                        'If Different User
                    ElseIf rq_lngUserID > 0 Then

                        'Hide Old Authorization Code
                        trOldAuth.Visible = False

                        'If Authorization Code is Not Locked
                        If strAuthLock = "N" And strAuthChange = "N" Then
                            trMsg.Visible = True
                            btnSave.Enabled = False
                            lblMessage.Text = "Validation Code can be changed only if it has been Locked out."
                            Exit Try
                        End If

                    End If

                End If

            Catch

                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page Load", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destoy Instance of Data Row
                drUsers = Nothing

                'Destroy Instance of DataSet
                dsUsers = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region
#Region "Procedures"

        Private Sub prcOldAuthCodeDisplay()

            Dim clsUsers As New MaxPayroll.clsUsers
            Dim dsUserDetails As New System.Data.DataSet
            dsUserDetails = clsUsers.fncUserDetails(ss_lngUserID, ss_lngOrgID, IIf(IsNumeric(hUserId.Value), hUserId.Value, 0))

            If dsUserDetails.Tables(0).Rows(0).Item("UAuthCode") = "N" Then
                trOldAuth.Visible = False

            End If

        End Sub

#End Region

#Region " Page Submit "

        '****************************************************************************************************
        'Procedure Name : Page_Submit()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        '*****************************************************************************************************
        Public Sub Page_Submit(ByVal O As System.Object, ByVal E As EventArgs) Handles btnSave.Click

            'Create Instance User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim intAttempts As Int16
            Dim strAuthChange As String
            Dim lngUserCode As Long
            Dim strPassAuth As String
            Dim strAuthPolicy As String
            Dim IsRepeated As Boolean
            Dim IsChange As Boolean
            Dim IsUser As Boolean
            Dim dsUsers As New System.Data.DataSet
            Dim strAuthLock As String = ""
            Try

                strAuthChange = Session("AUTH_CHNG")                                          'Authorization Code Change Status  
                lngUserCode = IIf(IsNumeric(hUserId.Value), hUserId.Value, 0)                 'Get Change Auth Code User Id 
                dsUsers = clsUsers.fncUserDetails(lngUserCode, 0, 0)
                If rq_lngUserID < 0 Then
                    If lngUserCode <> ss_lngUserID Then
                        HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                        Exit Try
                    End If
                End If
                Dim stat = 0
                'Get User Details - Start
                For Each drUsers In dsUsers.Tables("USER").Rows
                    strAuthLock = IIf(Not IsDBNull(drUsers("UALock")), drUsers("UALock"), "N")
                    If ss_lngOrgID <> drUsers("UOrgId") Then
                        stat = 1
                        Exit For
                    End If
                Next
                If stat = 1 Then
                    HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                    Exit Try
                End If
                If strAuthLock = "N" And strAuthChange = "N" Then
                    If rq_lngUserID > 0 Then
                        txtOldAuthCode.Visible = False
                    End If
                    trMsg.Visible = True
                    btnSave.Enabled = False
                    lblMessage.Text = "Validation Code can be changed only if it has been Locked out."
                    Exit Try
                End If
                'Check If Same User
                IsUser = IIf(ss_lngUserID = lngUserCode, True, False)

                'If Old Auth Code Does Not Match
                If ss_lngUserID = lngUserCode And hAuth.Value = "" Then
                    'Get Count of Invalid Auth Code Attempts
                    intAttempts = IIf(IsNumeric(Session("AUTH_INVALID")), Session("AUTH_INVALID"), 0)
                    'Get Orginal Authorization Code
                    strPassAuth = clsCommon.fncPassAuth(lngUserCode, "A", ss_lngUserID)
                    If Not intAttempts = 2 Then

                        If trOldAuth.Visible AndAlso Not strPassAuth = txtOldAuthCode.Text Then
                            lblMessage.Text = "Your Old Validation Code does not match."
                            'Incerement Counter
                            intAttempts = intAttempts + 1
                            Session("AUTH_INVALID") = intAttempts
                            Exit Try
                        End If
                    ElseIf intAttempts = 2 Then
                        If Not strPassAuth = txtOldAuthCode.Text Then
                            lblMessage.Text = "Your Validation Code has been locked due to invalid attempts. Please contact " & gc_UT_SysAdminDesc & "."
                            btnSave.Enabled = False
                            'btnClear.Enabled = False
                            'Block User Auth Code
                            Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "A")
                            'update for lock out report
                            Call clsUsers.prcLockHistory(ss_lngUserID, "A")
                            Exit Try
                        End If
                    End If
                End If

                'Auth Code Policy
                strAuthPolicy = clsUsers.fnValidatePassword(txtNewAuthCode.Text, "New Validation Code")
                If Len(Trim(strAuthPolicy)) > 0 Then
                    lblMessage.Text = strAuthPolicy
                    Exit Try
                End If

                'If Old & New Auth Code Match
                If ss_lngUserID = lngUserCode And hAuth.Value = "" Then
                    If txtOldAuthCode.Text = txtNewAuthCode.Text Then
                        lblMessage.Text = "Old Validation Code and New Validation Code cannot be the same."
                        Exit Try
                    End If
                End If

                'If Auth Code and User Id same
                If txtNewAuthCode.Text = hUserLogin.Value Then
                    lblMessage.Text = "User Id and Validation Code cannot be the same."
                    Exit Try
                End If

                'If Available in Auth Code History
                IsRepeated = clsUsers.fnAuthCodeHistory()
                If IsRepeated Then
                    lblMessage.Text = "Validation Code has been used previously."
                    Exit Try
                End If

                'If Password and Auth Code is same
                strPassAuth = clsCommon.fncPassAuth(lngUserCode, "P", ss_lngOrgID)
                If strPassAuth = txtNewAuthCode.Text Then
                    lblMessage.Text = "Validation Code cannot be the same as password."
                    Exit Try
                End If

                'Change Auth Code
                IsChange = clsUsers.fncChangePassAuth(lngUserCode, ss_lngOrgID, IsUser, txtNewAuthCode.Text, "AUTH")
                If IsChange And strAuthChange = "Y" Then
                    Session("AUTH_CHNG") = "N"
                    'If Not (Session("SYS_TYPE") = "U" And Session("SYS_TYPE") = "R" And Session("SYS_TYPE") = "A") Then
                    If Not ss_strUserType = gc_UT_Uploader And Not ss_strUserType = gc_UT_Reviewer And Not ss_strUserType = gc_UT_Auth Then
                        Session("NoMenu") = "False"
                        Server.Transfer("PG_Inbox.aspx", False)
                    Else
                        Session("NoMenu") = "False"
                        Server.Transfer("PG_GroupChange.aspx", False)
                    End If
                ElseIf IsChange And strAuthChange = "N" Then
                    btnSave.Enabled = False
                    lblMessage.Text = "Validation Code changed successfully."
                ElseIf Not IsChange Then
                    lblMessage.Text = "Validation Code change failed."
                End If

            Catch

                'Log Error
                If Err.Description <> "Thread was being aborted." Then
                    Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page Submit - Change Validation Code", Err.Number, Err.Description)
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

    End Class

End Namespace
