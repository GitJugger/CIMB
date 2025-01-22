Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers


Namespace MaxPayroll


    Partial Class PG_CreateRole

        Inherits clsBasePage
        Dim clsEncryption As New MaxPayroll.Encryption
#Region "Request.QueryString"
        Private ReadOnly Property rq_strID() As String
            Get
                Return clsEncryption.Cryptography(Request.QueryString("ID")) & ""
            End Get
        End Property
        Private ReadOnly Property rq_LoginId() As String
            Get
                If (Request.QueryString("LoginId")) IsNot Nothing Then
                    Return clsEncryption.Cryptography(Request.QueryString("LoginId"))
                Else
                    Return ""
                End If

            End Get
        End Property
        Private ReadOnly Property rq_iFieldLock() As Integer
            Get
                If IsNumeric(Request.QueryString("FieldLock")) Then
                    Return Request.QueryString("FieldLock")
                Else
                    Return -1
                End If

            End Get
        End Property
        Private ReadOnly Property rq_OrgId() As Long
            Get
                If IsNumeric(Request.QueryString("OrgID")) Then
                    Return CLng(Request.QueryString("OrgID"))
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
        'Created        : 16/02/2005
        'Modified By    : Victor Wong 
        'Modified Date  : 2007-03-08
        '*****************************************************************************************************



        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Create Instance of Data Row
            Dim drGroups As DataRow

            'Create Instane of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsGroups As New System.Data.DataSet

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim strMinValue As String, strVerify As String, strMod As String
            Dim strAuthLock As String

            Try

                strMod = Request.QueryString("Mod")
                hRequest.Value = Request.QueryString("Mode")
                hOrgId.Value = rq_OrgId
                If Not Len(ss_strUserType) = 2 Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                'Get Authorization Lock Status - Start
                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngUserID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    btnSubmit.Enabled = False
                    btnConfirm.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                End If
                'Get Authorization Lock Status - Stop

                If Not Page.IsPostBack Then
                    'BindBody(body)
                    If Me.rq_strID = "" Then
                        Me.inptBackToView.Visible = False
                    End If
                    'If Me.rq_LoginId <> "" Then
                    '    txtUserLogin.Text = rq_LoginId
                    'End If

                    trCreate.Visible = False                                                                'Create Table Row
                        tblConfirm.Visible = False                                                              'Main Form
                        tblMainForm.Visible = True                                                              'Confirm Form 
                        hApproved.Value = "0"                                                                    'Get User Type
                        strMinValue = Format(Now(), "dd/MM/yyyy")                                                'Set Current Date
                        'rngExpiryDate.MinimumValue = strMinValue                                                'Set Minimum Date



                        'Get Verification Type
                        strVerify = Session(gc_Ses_VerificationType)
                        hVerify.Value = strVerify

                    'Populate Organisation Groups - START
                    'If Not rq_OrgId = -1 Then
                    If ss_strUserType = gc_UT_SysAdmin OrElse ss_strUserType = gc_UT_SysAuth Then
                        dsGroups = clsCustomer.fncGrpCommon("LIST", ss_lngOrgID, ss_lngUserID, UCase(strVerify))
                        If dsGroups.Tables("GROUP").Rows.Count > 0 Then
                            For Each drGroups In dsGroups.Tables("GROUP").Rows
                                chkGroups.Items.Add(New ListItem(drGroups("GNAME"), drGroups("GID")))
                            Next
                        Else
                            Response.Write("<script language='JavaScript'>")
                            Response.Write("alert('No Groups Created or Approved. Please Create/Approve Group(s) before Creating users.');")
                            Response.Write("window.location.href = 'PG_ListGroup.aspx';")
                            Response.Write("</script>")
                            Exit Try
                        End If
                        cmbRoles.AutoPostBack = True
                        cmbRoles.Items.Add(New ListItem("", ""))
                        cmbRoles.Items.Add(New ListItem(gc_UT_UploaderDesc, gc_UT_Uploader))
                        cmbRoles.Items.Add(New ListItem(gc_UT_ReviewerDesc, gc_UT_Reviewer))
                        cmbRoles.Items.Add(New ListItem(gc_UT_AuthDesc, gc_UT_Auth))
                        cmbRoles.Items.Add(New ListItem(gc_UT_ReportDownloaderDesc, gc_UT_ReportDownloader))
                        'cmbRoles.Items.Add(New ListItem(gc_UT_InterceptorDesc, gc_UT_Interceptor))
                    ElseIf ss_strUserType = gc_UT_BankAdmin Or ss_strUserType = gc_UT_BankAuth Then
                        trGroups.Visible = False
                        cmbRoles.AutoPostBack = True
                        If rq_OrgId = -1 Then
                            cmbRoles.Items.Add(New ListItem("", ""))
                            cmbRoles.Items.Add(New ListItem(gc_UT_BankUserDesc, gc_UT_BankUser))
                            cmbRoles.Items.Add(New ListItem(gc_UT_BankOperatorDesc, gc_UT_BankOperator))
                            cmbRoles.Items.Add(New ListItem(gc_UT_InquiryUserDesc, gc_UT_InquiryUser))
                            cmbRoles.Items.Add(New ListItem(gc_UT_BankDownloaderDesc, gc_UT_BankDownloader))
                        Else
                            cmbRoles.Items.Add(New ListItem(gc_UT_ReportDownloaderDesc, gc_UT_ReportDownloader))
                        End If


                    ElseIf (ss_strUserType = gc_UT_BankUser) And hRequest.Value <> "BU" Then
                        Server.Transfer(gc_LogoutPath, False)
                        Exit Try
                        'trGroups.Visible = False
                        '    txtUserLogin.ReadOnly = False
                        '    cmbRoles.Items.Add(New ListItem("", ""))
                        '    cmbRoles.Items.Add(New ListItem(gc_UT_SysAdminDesc, gc_UT_SysAdmin))
                        '    cmbRoles.Items.Add(New ListItem(gc_UT_SysAuthDesc, gc_UT_SysAuth))
                    ElseIf (ss_strUserType = gc_UT_BankUser) And hRequest.Value = "BU" Then
                        'Server.Transfer(gc_LogoutPath, False)
                        'Exit Try
                        trGroups.Visible = False
                        txtUserLogin.ReadOnly = False
                        cmbRoles.Items.Add(New ListItem("", ""))
                        cmbRoles.Items.Add(New ListItem(gc_UT_SysAdminDesc, gc_UT_SysAdmin))
                        cmbRoles.Items.Add(New ListItem(gc_UT_SysAuthDesc, gc_UT_SysAuth))
                    ElseIf (ss_strUserType = gc_UT_InquiryUser) Then
                        'Server.Transfer(gc_LogoutPath, False)
                        'Exit Try
                        trGroups.Visible = False
                        txtUserLogin.ReadOnly = False
                        cmbRoles.Items.Add(New ListItem("", ""))
                        cmbRoles.Items.Add(New ListItem(gc_UT_SysAdminDesc, gc_UT_SysAdmin))
                        cmbRoles.Items.Add(New ListItem(gc_UT_SysAuthDesc, gc_UT_SysAuth))
                    End If
                        'Else
                        '    trGroups.Visible = True
                        '    cmbRoles.AutoPostBack = False
                        '    cmbRoles.Items.Add(New ListItem("", ""))
                        '    cmbRoles.Items.Add(New ListItem(gc_UT_ReportDownloaderDesc, gc_UT_ReportDownloader))
                        '    End If

                        'Populate Organisation Groups - STOP

                        If IsNumeric(rq_strID) AndAlso CInt(rq_strID) > 0 Then
                            btnUpdate.Visible = True
                            hUserId.Value = rq_strID

                            rdStatus.Items.Clear()
                            rdStatus.Items.Add(New ListItem("Active", "A"))
                            rdStatus.Items.Add(New ListItem("Inactive", "C"))
                            rdStatus.Items.Add(New ListItem("Delete", "D"))

                            If Not Page.IsPostBack Then
                                Call prcBindData(rq_strID)
                            End If
                            lblHeading.Text = "Modify User"
                        Else
                            rdStatus.Items.Add(New ListItem("Active", "A"))
                            rdStatus.Items.Add(New ListItem("Inactive", "C"))
                            rdStatus.SelectedValue = "A"
                            lblHeading.Text = "Create User"
                        End If

                        If strMod = "View" Then
                            trBack.Visible = True
                            trSubmit.Visible = False
                            lblMessage.Text = ""
                        End If

                        If Session("SYS_TYPE") = "IU" Then
                            btnSubmit.Enabled = False
                            btnReset.Disabled = True
                        End If

                        If rq_iFieldLock = enmPageMode.NonEditableMode Then
                            txtUserLogin.ReadOnly = True
                            txtUserName.ReadOnly = True
                            cmbRoles.Enabled = False
                            Me.imgCalExpDate.Visible = False
                            txtExpDate.Attributes.Remove("onfocus")
                            txtExpDate.Attributes.Add("READONLY", "true")
                            txtPassChangePeriod.ReadOnly = True
                            cbPassChangePeriod.Enabled = False
                            rdDisplay.Enabled = False
                            rdLimit.Enabled = False
                            Me.rdNLimit.Enabled = False
                            Me.txtLimit.ReadOnly = True
                            chkGroups.Enabled = False
                            rdStatus.Enabled = False
                            lblHeading.Text = "View User"
                            btnReset.Disabled = True
                            Me.chkEmailAlert.Enabled = False
                            'start - add by hafeez - 14-10-2008'
                            txtEmailAddress.ReadOnly = True
                            'end'
                        End If
                    End If

            Catch

                'Log Error
                LogError("Page_Load - PG_CreateRole")

            Finally

                'Destroy Instance of Datarow
                drGroups = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsGroups = Nothing

                'Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Page Confirm"

        '****************************************************************************************************
        'Procedure Name : Page_Confirm()
        'Purpose        : Page Confirm 
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/02/2005
        '*****************************************************************************************************
        Private Sub Page_Confirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'create instance of encryption class object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strPassword As String = ""
            Dim strAuthCode As String = ""
            Dim strUserId As String = ""
            Dim strResult As String = ""
            Dim strAction As String = ""
            Dim IsChecked As Boolean
            Dim strValue As String = ""
            Dim strText As String = ""
            Dim lngUserId As Long
            Dim strUserType As String = ""
            Dim lngOrgId As Long
            Dim lngUserCode As Long
            Dim intCounter As Int16
            Dim intTotalItems As Int16
            Dim IsDuplicate As Boolean
            Dim sMsg As String = ""
            Dim strBody As String = ""
            Try

                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtUserName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim strEncUsername1 As Boolean = clsCommon.CheckScriptValidation(Request.Form("ctl00$cphContent$txtCUserName"))
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserLogin1 As Boolean = clsCommon.CheckScriptValidation(txtUserLogin.Text)
                If txtUserLogin1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserpassword1 As Boolean = clsCommon.CheckScriptValidation(txtPassword.Text)
                If txtUserpassword1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtPassChangePeriod1 As Boolean = clsCommon.CheckScriptValidation(txtPassChangePeriod.Text)
                If txtPassChangePeriod1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtAuthCode1 As Boolean = clsCommon.CheckScriptValidation(txtAuthCode.Text)
                If txtAuthCode1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If

                If hUserId.Value = "" Then
                    strAction = "ADD"
                Else
                    strAction = "UPDATE"
                End If

                'Check If Limit - Start
                txtLimit.Text = IIf(IsNumeric(txtLimit.Text), txtLimit.Text, 0)
                If rdLimit.Checked And Not Trim(txtLimit.Text) > 0 Then
                    tblMainForm.Visible = True
                    tblConfirm.Visible = False
                    sMsg = "Validation Limit must be greater than zero."
                    Exit Try
                End If
                'Check If Limit - Stop\


                If strAction = "ADD" Then


                    'Check If User Id Exist - Start
                    IsDuplicate = clsUsers.fnCheckDuplicate(txtUserLogin.Text)
                    If IsDuplicate Then
                        tblMainForm.Visible = True
                        tblConfirm.Visible = False
                        sMsg += "User Id Already Exist. Please Use a Different User Id." & gc_BR

                    End If
                    'Check If User Id Exist - Stop

                    'Do Password Policy Validation - Start
                    strResult = clsUsers.fnValidatePassword(txtPassword.Text, "Password")
                    If Len(Trim(strResult)) > 0 Then
                        tblMainForm.Visible = True
                        tblConfirm.Visible = False
                        sMsg += strResult & gc_BR
                    End If
                    'Do Password Policy Validation - Stop

                    If IsDate(Me.txtExpDate.Value) AndAlso CDate(txtExpDate.Value) < Date.Now Then
                        sMsg += "Date must be greater than Today's Date (" & Date.Now.Date & ")" & gc_BR
                    End If

                    'Do Auth Code Policy Validation - Start
                    'This part will only be executed when the roles is NOT 
                    '1. Inquiry User
                    '2. Bank Operator 
                    '3. Authorizer with Token Setting's value is True - Added #15 Jan 07
                    If Not (cmbRoles.SelectedValue = gc_UT_InquiryUser Or cmbRoles.SelectedValue = gc_UT_BankDownloader Or cmbRoles.SelectedValue = gc_UT_ReportDownloader Or cmbRoles.SelectedValue = gc_UT_BankOperator Or (Session(gc_Ses_Token) AndAlso cmbRoles.SelectedValue = gc_UT_Auth)) Then
                        strResult = clsUsers.fnValidatePassword(txtAuthCode.Text, "Validation Code")
                        If Len(Trim(strResult)) > 0 Then
                            tblMainForm.Visible = True
                            tblConfirm.Visible = False
                            sMsg += strResult & gc_BR
                            Exit Try
                        End If
                    End If
                    'Do Auth Code Policy Validation - Stop

                    'If User Id and Password is same Then prompt User to Change - Start
                    If txtUserLogin.Text = txtPassword.Text Then
                        tblMainForm.Visible = True
                        tblConfirm.Visible = False
                        sMsg += "User Id and Password Cannot be Same." & gc_BR

                    End If
                    'If User Id and Password is same Then prompt User to Change - Stop

                    'If User Id and Auth Code is same Then prompt User to Change - Start
                    If txtUserLogin.Text = txtAuthCode.Text Then
                        tblMainForm.Visible = True
                        tblConfirm.Visible = False
                        sMsg += "User Id and Validation Code Cannot be Same." & gc_BR

                    End If
                    'If User Id and AuthCode is same Then prompt User to Change - Stop

                    'If User Id and Password is same Then prompt User to Change - Start
                    If txtAuthCode.Text = txtPassword.Text Then
                        tblMainForm.Visible = True
                        tblConfirm.Visible = False
                        sMsg += "Validation Code and Password Cannot be Same." & gc_BR

                    End If
                    'If User Id and Password is same Then prompt User to Change - Stop

                    lblMessage.Text = sMsg
                    If Len(lblMessage.Text) > 0 Then
                        Exit Try
                    End If
                End If

                tblConfirm.Visible = True
                'Display Confirm Form

                'Hide the Authentication Code Row in Confirm Form when the role is Authorizer with Token Setting is True. #15 Jan 07 - Start
                If CBool(Session(gc_Ses_Token)) AndAlso cmbRoles.SelectedValue = "A" Then
                    trCAuthCode.Visible = False
                    'trAuth.Visible = False
                End If
                'Hide the Authentication Code Row in Confirm Form when the role is Authorizer with Token Setting is True. #15 Jan 07 - End
                If Request.QueryString("ID") IsNot Nothing Then
                    lngUserId = clsEncryption.Cryptography(Request.QueryString("ID"))
                Else
                    lngUserId = 0
                End If
                tblMainForm.Visible = False                                                         'Hide Main Form
                strUserType = Session("SYS_TYPE")                                                   'Get User Type
                lblMessage.Text = "Please Enter your Validation Code and Confirm User Details"   'Set Heading
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)            'Get Organisation Id
                lngUserCode = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)       'Get Logged in User Id
                '  lngUserId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0) 'Get Modify User Id
                txtCUserLogin.Text = txtUserLogin.Text                                              'User Login
                txtCUserName.Text = txtUserName.Text                                                'User Name
                hPassword.Value = txtPassword.Text                                                  'Password
                txtCExpDate.Text = txtExpDate.Value                                                 'Expiry Data
                txtCPassChangePeriod.Text = txtPassChangePeriod.Text                                'Password Change Period
                txtCChangeUnit.Text = cbPassChangePeriod.SelectedItem.Text
                hAuthCode.Value = txtAuthCode.Text
                txtRole.Text = cmbRoles.SelectedItem.Text
                txtCDisplay.Text = rdDisplay.SelectedItem.Text
                hDisplay.Value = rdDisplay.SelectedValue
                txtStatus.Text = rdStatus.SelectedItem.Text
                hStatus.Value = rdStatus.SelectedValue
                hReset.Value = IIf(chkReset.Checked, "Y", "N")
                Me.hEmail.Value = Me.txtEmailAddress.Text
                If Len(txtEmailAddress.Text) > 0 Then
                    Me.trCEmailAddress.Visible = True
                    txtCEmailAddress.Text = txtEmailAddress.Text
                End If

                Me.txtCStaffNumber.Text = Me.txtStaffNumber.Text

                'Check if reset - start
                If hReset.Value = "Y" Then
                    'Generate Password,Auth Code and User Id
                    Call clsEncryption.fncGenerate(txtCUserName.Text, 8, strUserId, strPassword, strAuthCode)
                    'if user name changed, user id changed
                    If Not AhUName.Value = txtUserName.Text Then
                        'Remove Empty Space
                        txtCUserLogin.Text = Replace(strUserId, " ", "")
                    End If
                    'Encrypt Authorization Code
                    hAuthCode.Value = strAuthCode
                    'Encrypt Password
                    hPassword.Value = strPassword
                End If
                'Check if reset - start

                If lngUserId > 0 Then
                    btnUpdate.Visible = True
                    btnConfirm.Visible = False
                End If

                If Session(gc_Ses_VerificationType) = "DUAL" Then
                    trCReason.Visible = True
                    txtCReason.Text = txtReason.Text
                ElseIf Session(gc_Ses_VerificationType) = "SINGLE" Then
                    trReason.Visible = False
                    trCReason.Visible = False
                End If

                If cmbRoles.SelectedValue = gc_UT_Auth Then
                    trCLimit.Visible = True
                    trCDisplay.Visible = True
                    If rdNLimit.Checked Then
                        lblLimit.Text = "No Limit"
                        hLimit.Value = "0"
                    ElseIf rdLimit.Checked Then
                        lblLimit.Text = "RM " & Format(CDbl(txtLimit.Text), "##,##0.00")
                        hLimit.Value = txtLimit.Text
                    End If
                ElseIf cmbRoles.SelectedValue = gc_UT_Interceptor Then
                    trCLimit.Visible = False
                    trCDisplay.Visible = False
                ElseIf cmbRoles.SelectedValue = gc_UT_Uploader Then
                    trCLimit.Visible = False
                    trCDisplay.Visible = False
                ElseIf strUserType = gc_UT_BankAdmin Then
                    trCGroups.Visible = False
                    trCLimit.Visible = False
                    trCDisplay.Visible = False
                ElseIf strUserType = gc_UT_BankUser Then
                    trCLimit.Visible = False
                    trCDisplay.Visible = False
                    trCGroups.Visible = False
                    trCAuthCode.Visible = False
                    trCPassword.Visible = False
                ElseIf strUserType = gc_UT_BankDownloader Then
                    trCLimit.Visible = False
                    trCDisplay.Visible = False
                    trCGroups.Visible = False
                    trCAuthCode.Visible = False
                    trCPassword.Visible = False
                Else
                    trCLimit.Visible = False
                    trCDisplay.Visible = True
                End If

                'check if group(s) selected and move to list box - start
                If strUserType = "CA" Then
                    lbxGroups.Items.Clear()
                    intCounter = chkGroups.Items.Count - 1
                    For intTotalItems = 0 To intCounter
                        If chkGroups.Items(intTotalItems).Selected Then
                            IsChecked = True
                            strValue = chkGroups.Items(intTotalItems).Value
                            strText = chkGroups.Items(intTotalItems).Text
                            lbxGroups.Items.Add(New ListItem(strText, strValue))
                        End If
                    Next
                    'if no groups selected, display error message
                    If Not IsChecked Then
                        tblMainForm.Visible = True
                        tblConfirm.Visible = False
                        sMsg = "Please Select User Group(s)" & gc_BR
                        Exit Try
                    End If
                End If
                'check if group(s) selected and move to list box - stop

                'if bank user hide confirm reason
                If strUserType = gc_UT_BankUser Then
                    trCReason.Visible = False
                End If

                If cmbRoles.SelectedValue = gc_UT_InquiryUser Or cmbRoles.SelectedValue = gc_UT_BankOperator Then
                    trCAuthCode.Visible = False
                ElseIf cmbRoles.SelectedValue = gc_UT_BankUser Then
                    Me.trCEmailAlert.Visible = True
                    If Me.chkEmailAlert.Checked Then
                        Me.lblReceiveEmail.Text = "Yes"
                        Me.hIsReceiveEmail.Value = "1"
                    Else
                        Me.lblReceiveEmail.Text = "No"
                        Me.hIsReceiveEmail.Value = "0"
                    End If
                End If

                If hUserId.Value = "" Then
                    trCReason.Visible = False
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "Page_Confirm - PG_CreateRole", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'destroy instance of encryption class object
                clsEncryption = Nothing

                'Destroy Instance of Users Class Object
                clsUsers = Nothing

            End Try
            If Len(sMsg) > 0 Then
                lblMessage.Text = sMsg
            End If
        End Sub

#End Region

#Region "Save"

        Private Function fncValidate() As Boolean

            Dim sMsg As String = ""


            If IsDate(Me.txtExpDate.Value) AndAlso CDate(txtExpDate.Value) < Date.Now Then
                sMsg += "Date must be greater than Today's Date (" & Date.Now & ")"
            End If


            lblMessage.Text = sMsg

            If (sMsg) > 0 Then
                Return False
            Else
                Return True
            End If

        End Function


        '****************************************************************************************************
        'Procedure Name : prSave()
        'Purpose        : Submit Contents to Database
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Private Sub prSave(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click


            'Create Instance of Users Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Approval Matrix Class Object
            Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

            'Variable Declarations
            Dim lngOrgId As Long
            Dim lngUserId As Long
            Dim strVerify As String = ""
            Dim strAuthCode As String = ""
            Dim strRoles As String = ""
            Dim strBody As String = ""
            Dim strAuthTo As String = ""
            Dim strCAuthCode As String = ""
            Dim lngUserCode As Long
            Dim strUser As String = ""
            Dim strVerifier As String = ""
            Dim lngToId As Long
            Dim intTotalItems As Int16
            Dim intCounter As Int16
            Dim IsAuthCode As Boolean
            Dim intAttempts As Int16
            Dim strUserName As String = ""
            Dim strSubject As String = ""
            Dim intApproved As Int16
            Dim strUserLogin As String = ""
            Dim strUserPassword As String = ""
            Dim lngTransId As Long

            Try
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtUserName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim strEncUsername1 As Boolean = clsCommon.CheckScriptValidation(Request.Form("ctl00$cphContent$txtCUserName"))
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserLogin1 As Boolean = clsCommon.CheckScriptValidation(txtUserLogin.Text)
                If txtUserLogin1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserpassword1 As Boolean = clsCommon.CheckScriptValidation(txtPassword.Text)
                If txtUserpassword1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtPassChangePeriod1 As Boolean = clsCommon.CheckScriptValidation(txtPassChangePeriod.Text)
                If txtPassChangePeriod1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtAuthCode1 As Boolean = clsCommon.CheckScriptValidation(txtAuthCode.Text)
                If txtAuthCode1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                strUser = Session("SYS_TYPE")                                                   'User Type
                strUserName = txtCUserName.Text                                                  'User Name
                strRoles = Request.Form("ctl00$cphContent$txtRole")                                              'User Roles
                strAuthCode = Request.Form("ctl00$cphContent$hAuthCode")                                         'User Authorization Code
                strUserPassword = Request.Form("ctl00$cphContent$hPassword")                                     'User Password
                strUserLogin = Request.Form("ctl00$cphContent$txtCUserLogin")                                    'User Id
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Organisation Id
                lngUserCode = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)   'User Id

                If txtCAuthCode.Text = "" Then
                    lblMessage.Text = "Please enter your Validation Code"
                    Exit Try
                End If

                'Check If AuthCode is Valid - Start
                strCAuthCode = clsCommon.fncPassAuth(lngUserCode, "A", lngOrgId)
                IsAuthCode = IIf(strCAuthCode = txtCAuthCode.Text, True, False)
                'Check If AuthCode is Valid - Stop

                'Check for invalid Authorization Code Attempts - START
                If Not IsAuthCode Then
                    intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                    If Not intAttempts = 2 Then
                        If Not IsAuthCode Then
                            intAttempts = intAttempts + 1
                            Session("AUTH_LOCK") = intAttempts
                            'display message
                            lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                            Exit Try
                        End If
                    ElseIf intAttempts = 2 Then
                        If Not IsAuthCode Then
                            trConfirm.Visible = False
                            'lock user auth code
                            Call clsUsers.prcAuthLock(lngOrgId, lngUserCode, "A")
                            'update for lock out report
                            Call clsUsers.prcLockHistory(lngUserCode, "A")
                            'display message
                            lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                            Exit Try
                        End If
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP

                'Get Verification Type
                strVerify = Session(gc_Ses_VerificationType)
                If strVerify = "SINGLE" Then
                    intApproved = 2
                ElseIf strVerify = "DUAL" Then
                    intApproved = 1
                End If

                'Add User Details
                lngUserId = clsUsers.fnDB_User("ADD", intApproved, rq_OrgId)

                If lngUserId > 0 Then

                    'Hide Save Button
                    trAuth.Visible = False
                    trCreate.Visible = True
                    trConfirm.Visible = False

                    'User Group Insert - START
                    If strUser = "CA" Then
                        intTotalItems = lbxGroups.Items.Count
                        For intCounter = 0 To intTotalItems - 1
                            lngTransId = lbxGroups.Items(intCounter).Value
                            Call clsCustomer.prcGrpTrans("USER DETAILS", lngTransId, lngOrgId, lngUserCode, lngUserId)
                        Next
                    End If
                    'User Group Insert - START

                    If strUser = "CA" Then
                        strAuthTo = "System Auth"
                    ElseIf strUser = "BA" Then
                        strAuthTo = "Bank Super"
                    End If

                    'If Single Verification
                    If UCase(strVerify) = "SINGLE" Then
                        lblMessage.Text = "User Role Created Successfully"
                        'If Dual Verification
                    ElseIf UCase(strVerify) = "DUAL" Then

                        'Get System Authorizer/Bank Super Admin
                        strVerifier = clsCommon.fncBuildContent(strAuthTo, "", lngOrgId, lngUserId)
                        If IsNumeric(Trim(strVerifier)) Then
                            lngToId = Trim(strVerifier)
                        End If

                        'Mail Subject
                        strSubject = strUserName & "(" & strRoles & ")" & " User Created"
                        'Mail Body
                        strBody = strSubject & " , pending for approval."




                        'Update Approval Matrix
                        If strUser = "BA" Then  'send to bank authorizers
                            Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserCode, "INSERT", 0, lngUserCode, 0, lngUserId, strSubject, "User Creation", "", 1, txtCReason.Text)
                            'Send Mail
                            Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserCode, 0, strSubject, strBody, lngToId)
                        Else    'send to system authorizer
                            Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserCode, "INSERT", 0, lngUserCode, lngToId, lngUserId, strSubject, "User Creation", "", 1, txtCReason.Text)
                            'Send Mail
                            Call clsCommon.prcSendMails("SYS AUTH", lngOrgId, lngUserCode, 0, strSubject, strBody, lngToId)
                        End If
                        'Display Message
                        lblMessage.Text = "User Role Created Successfully. Request sent for Approval."


                    End If
                Else
                    lblMessage.Text = "Role Creation Failed"
                End If

            Catch

                lblMessage.Text = "Role Creation Failed."
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prSave - Create Role", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Users Class Object
                clsUsers = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Approval Matrix Class Object
                clsApprMatrix = Nothing

            End Try

        End Sub

#End Region

#Region "Back To View"

        '****************************************************************************************************
        'Procedure Name : prBack()
        'Purpose        : Take Back to View/Search Screen
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        'Private Sub prBack(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnBack.Click

        '    Try

        '        'Redirect to View/Search Screen
        '        Server.Transfer("PG_ViewRoles.aspx", False)

        '    Catch ex As Exception

        '    End Try

        'End Sub

#End Region

#Region "Modify Roles"

        '****************************************************************************************************
        'Procedure Name : prBindData()
        'Purpose        : Populate Values for the Requested User Id
        'Arguments      : User Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 26/10/2003
        '*****************************************************************************************************
        Private Sub prcBindData(ByVal lngUserId As Long)

            'Create Instance of DataRow
            Dim drGroup As DataRow

            'Create Instance of DataSet
            Dim dsGroup As New DataSet

            'Create Instance of Data Row Object
            Dim drUsers As System.Data.DataRow

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Data Set Object
            Dim dsUsers As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim lngApprId As Long
            Dim intTotalItems As Int16, lngTransId As Long, lngTranId As Long, strUser As String, strVerify As String
            Dim lngOrgId As Long, lngUserCode As Long, intCounter As Int16, intApporved As Int16
            Dim bIsReceiveEmail As Boolean = False
            Dim strBody As String = ""
            Try
                If Request.QueryString("ID") IsNot Nothing Then
                    lngUserId = clsEncryption.Cryptography(Request.QueryString("ID"))
                Else
                    lngUserId = 0
                End If
                tblConfirm.Visible = False
                tblMainForm.Visible = True
                strUser = Session("SYS_TYPE")
                strVerify = Session(gc_Ses_VerificationType)
                lblHeading.Text = "Roles Information - Modify User Role"
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)
                lngUserCode = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
                'lngUserId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)
                lngApprId = IIf(IsNumeric(Request.QueryString("APPR")), Request.QueryString("APPR"), 0)

                btnUpdate.Visible = True                     'Show Update Button
                hUserId.Value = lngUserId                    'User Id
                trPassword.Visible = False                   'Password Table Row Hidden
                trAuthCode.Visible = False                   'Authorization Table Row Hidden
                rfvAuthCode.Enabled = False                  'Disable Auth Code Req fld Valid
                rgeAuthCode.Enabled = False                  'Disable Auth Code Reqular fld Valid   
                rfvPassword.Enabled = False                  'Disable Password Req fld Valid   
                rgePassword.Enabled = False                  'Disable Auth Code Reqular fld Valid
                txtUserLogin.ReadOnly = True                 'Set User Id Text Box to Readonly

                'Get User Details
                dsUsers = clsUsers.fncUserDetails(lngUserId, lngOrgId, lngUserCode)
                Dim stat = 0
                'Assign Values - Start
                For Each drUsers In dsUsers.Tables("USER").Rows
                    'If ss_lngOrgID <> drUsers("UOrgId") Then
                    'stat = 1
                    'Exit For
                    'End If
                    If drUsers("UType") = "A" Then
                        trLimit.Visible = True
                        trDisplay.Visible = True
                        If drUsers("ULimit") = 0 Then
                            rdNLimit.Checked = True
                            txtLimit.Text = "0.00"
                        Else
                            rdLimit.Checked = True
                            txtLimit.Text = System.Math.Round(CDbl(txtLimit.Text), 2)
                        End If
                    ElseIf drUsers("UType") = "R" Then
                        trDisplay.Visible = True
                        'ElseIf drUsers("UType") = "I" Then
                        '    trLimit.Visible = False
                        '    trDisplay.Visible = False
                    ElseIf drUsers("UType") = "RD" Then

                        trDisplay.Visible = True
                    Else
                        trLimit.Visible = False
                    End If

                    intApporved = drUsers("UAppr")                                  'User Apporval
                    txtLimit.Text = drUsers("ULimit")                               'User Limit
                    txtUserName.Text = drUsers("UName")                             'User Name
                    txtExpDate.Value = drUsers("UExpiry")                           'Expiry Date
                    txtUserLogin.Text = drUsers("ULogin")                           'User Id
                    cmbRoles.SelectedValue = drUsers("UType")                       'User Type
                    rdStatus.SelectedValue = drUsers("UStatus")                     'User Status
                    rdDisplay.SelectedValue = drUsers("UDisplay")                   'User Display
                    txtPassChangePeriod.Text = drUsers("UPeriod")                   'User Password Period
                    cbPassChangePeriod.SelectedValue = drUsers("UUnit")             'User Password Change Period


                    If drUsers("UType") = gc_UT_BankUser Then
                        Me.trEmailAlert.Visible = True
                        Me.chkEmailAlert.Checked = CBool(drUsers("IsReceiveEmail"))
                    ElseIf drUsers("UType") = gc_UT_BankDownloader Then
                        Me.trEmailAddress.Visible = True
                        txtEmailAddress.Text = CStr(drUsers("User_Email") & "")
                        Me.hEmail.Value = txtEmailAddress.Text

                    End If

                    txtStaffNumber.Text = drUsers("StaffNumber")

                Next
                'Assign Values - Stop

                'Audit Trail
                '  If stat = 1 Then
                'HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                'Exit Try
                '  End If

                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtUserName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserLogin1 As Boolean = clsCommon.CheckScriptValidation(txtUserLogin.Text)
                If txtUserLogin1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserpassword1 As Boolean = clsCommon.CheckScriptValidation(txtPassword.Text)
                If txtUserpassword1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtPassChangePeriod1 As Boolean = clsCommon.CheckScriptValidation(txtPassChangePeriod.Text)
                If txtPassChangePeriod1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtAuthCode1 As Boolean = clsCommon.CheckScriptValidation(txtAuthCode.Text)
                If txtAuthCode1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                AhUName.Value = txtUserName.Text
                AhExpry.Value = txtExpDate.Value
                AhStats.Value = rdStatus.SelectedValue
                AhURole.Value = cmbRoles.SelectedValue
                AhPerid.Value = txtPassChangePeriod.Text
                AhPUnit.Value = cbPassChangePeriod.SelectedValue

                'Checked Group - START
                If (strUser = "CA" Or strUser = "SA") Then
                    intCounter = 0
                    intTotalItems = (chkGroups.Items.Count) - 1
                    dsGroup = clsCustomer.fncListGroup("USER GROUP", lngUserId, lngUserId, 0)
                    If dsGroup.Tables("LIST").Rows.Count > 0 Then
                        For Each drGroup In dsGroup.Tables("LIST").Rows
                            lngTransId = drGroup("GID")
                            For intCounter = 0 To intTotalItems
                                lngTranId = chkGroups.Items(intCounter).Value
                                If lngTranId = lngTransId Then
                                    chkGroups.Items(intCounter).Selected = True
                                End If
                            Next
                        Next
                    End If
                End If
                'Checked Group - STOP

                'Check If User Not Yet Approved - START
                If strVerify = "DUAL" Then
                    trReason.Visible = True
                    If lngApprId > 0 Then
                        txtReason.Text = clsCommon.fncBuildContent("Appr Reason", "", lngApprId, lngUserId)
                        txtReason.ReadOnly = True
                    End If
                    If intApporved = 1 Then
                        btnSubmit.Enabled = False
                        lblMessage.Text = "User Role cannot be modified, since pending for approval."
                        Exit Try
                    End If
                End If
                'Check If User Not Yet Approved - STOP

                'Check is user is deleted - start
                If rdStatus.SelectedValue = "D" Then
                    trReason.Visible = False
                    btnSubmit.Enabled = False
                    lblMessage.Text = "User Role has been deleted."
                    Exit Try
                End If
                'Check is user is deleted - stop

                'If BU Modifying CA/SA - Start
                If strUser = "BU" Then
                    trReset.Visible = True
                    trReason.Visible = False

                End If
                'If BU Modifying CA/SA - Stop

            Catch

                clsGeneric.ErrorLog(lngOrgId, lngUserCode, "prBindData - PG_CreateRole", Err.Number, Err.Description)

            Finally

                'Destroy Instance of DataRow
                drGroup = Nothing

                'Destroy Instance of DataSet
                dsGroup = Nothing

                'Destroy Instance of Data Row
                drUsers = Nothing

                'Destroy Instance of Data Set
                dsUsers = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Update"

        '****************************************************************************************************
        'Procedure Name : prUpdate()
        'Purpose        : Update Contents to Database
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Private Sub prUpdate(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

            'Create Instance of Users Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance ofs Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Objectg
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Approval Matrix Class Object
            Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

            'Variable Declarations

            Dim strOldData As String
            Dim strNewData As String
            Dim intApproved As Int16
            Dim strSubject As String = ""
            Dim strAuthTo As String = ""
            Dim lngUserCode As Long
            Dim strVerify As String
            Dim strCAuthCode As String
            Dim IsAuthCode As Boolean
            Dim intAttempts As Int16
            Dim strBody As String
            Dim strUserName As String
            Dim intCounter As Int16
            Dim lngTransId As Long
            Dim strVerifier As String
            Dim lngToId As Long
            Dim strUser As String
            Dim lngCustId As Long
            Dim strRoles As String
            Dim lngOrgId As Long
            Dim lngUserId As Long
            Dim intTotalItems As Int16
            Dim intApprId As Int32

            Try
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtUserName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim strEncUsername1 As Boolean = clsCommon.CheckScriptValidation(Request.Form("ctl00$cphContent$txtCUserName"))
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserLogin1 As Boolean = clsCommon.CheckScriptValidation(txtUserLogin.Text)
                If txtUserLogin1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtUserpassword1 As Boolean = clsCommon.CheckScriptValidation(txtPassword.Text)
                If txtUserpassword1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtPassChangePeriod1 As Boolean = clsCommon.CheckScriptValidation(txtPassChangePeriod.Text)
                If txtPassChangePeriod1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtAuthCode1 As Boolean = clsCommon.CheckScriptValidation(txtAuthCode.Text)
                If txtAuthCode1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                lngUserCode = hUserId.Value
                strUser = Session("SYS_TYPE")
                strUserName = txtCUserName.Text
                strRoles = Request.Form("ctl00$cphContent$txtRole")                                                                                  'Get Role    
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                                            'Get Organization Id
                lngUserCode = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Get User Id
                'Create Instance of Data Set Object
                Dim dsUsers As New System.Data.DataSet
                Dim lngUserId1 As Long
                If Request.QueryString("ID") IsNot Nothing Then
                    lngUserId1 = clsEncryption.Cryptography(Request.QueryString("ID"))
                Else
                    lngUserId1 = 0
                End If
                dsUsers = clsUsers.fncUserDetails(lngUserId1, lngOrgId, lngUserCode)
                Dim stat = 0
                ' For Each drUsers In dsUsers.Tables("USER").Rows
                'If ss_lngOrgID <> drUsers("UOrgId") Then
                'stat = 1
                'Exit For
                'End If
                'Next
                '   If stat = 1 Then
                '  HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                '  Exit Try
                '   End If
                If Not Request.QueryString("OrgId") = "" Then
                    lngCustId = Request.QueryString("OrgId") 'IIf(Replace(Request.QueryString("OrgId"), gc_Const_CCPrefix, ""), Replace(Request.QueryString("OrgId"), gc_Const_CCPrefix, ""), 0)  'Get Organization Id
                Else
                    lngCustId = lngOrgId
                End If

                If txtCAuthCode.Text = "" Then
                    lblMessage.Text = "Please enter your Validation Code"
                    Exit Try
                End If

                'Check If AuthCode is Valid - Start
                strCAuthCode = clsCommon.fncPassAuth(lngUserCode, "A", lngOrgId)
                IsAuthCode = IIf(strCAuthCode = txtCAuthCode.Text, True, False)
                'Check If AuthCode is Valid - Stop

                'Check for invalid Authorization Code Attempts - START
                If Not IsAuthCode Then
                    intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                    If Not intAttempts = 2 Then
                        If Not IsAuthCode Then
                            intAttempts = intAttempts + 1
                            Session("AUTH_LOCK") = intAttempts
                            lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                            Exit Try
                        End If
                    ElseIf intAttempts = 2 Then
                        If Not IsAuthCode Then
                            trConfirm.Visible = False
                            'lock out user
                            Call clsUsers.prcAuthLock(lngOrgId, lngUserCode, "A")
                            'update for lock out report
                            Call clsUsers.prcLockHistory(lngUserCode, "A")
                            'display message
                            lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                            Exit Try
                        End If
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP

                'Get Verification Type
                strVerify = Session(gc_Ses_VerificationType)
                If strVerify = "SINGLE" Then
                    intApproved = 2
                ElseIf strVerify = "DUAL" Then
                    If strUser = "BU" Then
                        intApproved = 2
                    Else
                        intApproved = 1
                    End If
                End If

                'Update User Details
                lngUserId = clsUsers.fnDB_User("UPDATE", intApproved)

                If lngUserId > 0 Then

                    'Hide Save Button
                    trCBack.Visible = True
                    trAuth.Visible = False
                    trConfirm.Visible = False

                    If strUser = "CA" Then

                        'Delete User from Group - START
                        Call clsCustomer.prcGrpDelTrans("GROUP USER", lngUserId, lngOrgId, lngUserId)
                        'Delete User from Group - STOP

                        'User Group Insert - START
                        intTotalItems = lbxGroups.Items.Count
                        For intCounter = 0 To intTotalItems - 1
                            lngTransId = lbxGroups.Items(intCounter).Value
                            Call clsCustomer.prcGrpTrans("USER DETAILS", lngTransId, lngOrgId, lngUserCode, lngUserId)
                        Next
                        'User Group Insert - STOP

                    End If

                    If Not strUser = "BU" Then

                        If strUser = "CA" Then
                            strAuthTo = "System Auth"
                        ElseIf strUser = "BA" Then
                            strAuthTo = "Bank Super"
                        End If

                        'If Single Verification
                        If UCase(strVerify) = "SINGLE" Then

                            lblMessage.Text = "User Role Modified Successfully"

                            'If Dual Verification
                        ElseIf UCase(strVerify) = "DUAL" Then

                            'Get System Authorizer
                            strVerifier = clsCommon.fncBuildContent(strAuthTo, "", lngOrgId, lngUserId)
                            If IsNumeric(Trim(strVerifier)) Then
                                lngToId = Trim(strVerifier)
                            End If

                            'Update Approval Matrix
                            If hStatus.Value = "A" Or hStatus.Value = "C" Then
                                'Mail Subject
                                strSubject = strUserName & "(" & strRoles & ")" & " User Modified"
                                If strUser = "BA" Then
                                    intApprId = clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserCode, 0, lngUserId, strSubject, "User Modification", "", 1, txtCReason.Text)
                                Else
                                    intApprId = clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserCode, lngToId, lngUserId, strSubject, "User Modification", "", 1, txtCReason.Text)
                                End If
                            ElseIf hStatus.Value = "D" Then
                                'Mail Subject
                                strSubject = strUserName & "(" & strRoles & ")" & " User Deleted"
                                If strUser = "BA" Then
                                    intApprId = clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserCode, 0, lngUserId, strSubject, "User Deletion", "", 1, txtCReason.Text)
                                Else
                                    intApprId = clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserCode, lngToId, lngUserId, strSubject, "User Deletion", "", 1, txtCReason.Text)
                                End If
                            End If

                            'Mail Body
                            strBody = strSubject & " , pending for approval."
                            If ConfigurationManager.AppSettings("USER") = "BANK" Then
                                Call clsCommon.prcSendMails("BANK AUTH", lngOrgId, lngUserCode, 0, strSubject, strBody, lngToId)
                            Else
                                Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserCode, 0, strSubject, strBody, lngToId)

                            End If 'Send Mail

                            'Audit Trail - START
                            If Not txtCUserName.Text = AhUName.Value Then
                                'Track User Name Change
                                Call clsUsers.prcModifyLog(lngUserId, lngCustId, "Modify User", "User Name", _
                                        txtCUserName.Text, AhUName.Value, lngUserCode, "U", intApprId)
                            End If
                            If Not cmbRoles.SelectedValue = AhURole.Value Then
                                'Track User Role Change
                                Call clsUsers.prcModifyLog(lngUserId, lngCustId, "Modify User", "User Role", _
                                        cmbRoles.SelectedValue, AhURole.Value, lngUserCode, "U", intApprId)
                            End If
                            If Not txtExpDate.Value = AhExpry.Value Then
                                'Track Expiry Date
                                Call clsUsers.prcModifyLog(lngUserId, lngCustId, "Modify User", "Expiry Date", _
                                        txtExpDate.Value, AhExpry.Value, lngUserCode, "U", intApprId)
                            End If
                            If Not txtPassChangePeriod.Text = AhPerid.Value Then
                                'Track Pass Change Period
                                Call clsUsers.prcModifyLog(lngUserId, lngCustId, "Modify User", "Pass Change Period", _
                                        txtPassChangePeriod.Text, AhPerid.Value, lngUserCode, "U", intApprId)
                            End If
                            If Not cbPassChangePeriod.SelectedValue = AhPUnit.Value Then
                                'Track Pass Change Unit
                                Call clsUsers.prcModifyLog(lngUserId, lngCustId, "Modify User", "Pass Change Unit", _
                                        cbPassChangePeriod.SelectedValue, AhPUnit.Value, lngUserCode, "U", intApprId)
                            End If
                            If Not rdStatus.SelectedValue = AhStats.Value Then
                                strOldData = AhStats.Value
                                If strOldData = "A" Then
                                    strOldData = "Active"
                                ElseIf strOldData = "C" Then
                                    strOldData = "Inactive"
                                ElseIf strOldData = "D" Then
                                    strOldData = "Delete"
                                End If

                                strNewData = rdStatus.SelectedValue
                                If strNewData = "A" Then
                                    strNewData = "Active"
                                ElseIf strNewData = "C" Then
                                    strNewData = "Inactive"
                                ElseIf strNewData = "D" Then
                                    strNewData = "Delete"
                                    'Insert delete logs for single verification
                                    'If UCase(strVerify) = "SINGLE" Then

                                    'End If

                                End If

                                'Track Status
                                Call clsUsers.prcModifyLog(lngUserId, lngCustId, "Modify User", "Status",
                                        strNewData, strOldData, lngUserCode, "U", intApprId)

                            End If
                            'Audit Trail - STOP

                            'Display Message
                            lblMessage.Text = "User Role Modified Successfully. Request sent for Approval."

                            End If

                    Else

                        lblMessage.Text = "User Role Modified Successfully"

                    End If

                Else
                    lblMessage.Text = "User Role Modification Failed"
                End If

            Catch

                lblMessage.Text = "User Role Updation Failed."
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prUpdate - Modify Role", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Users Class Object
                clsUsers = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Approval Matrix Class Object
                clsApprMatrix = Nothing

            End Try

        End Sub

#End Region

#Region "Show/Hide"

        Private Sub prcRole(ByVal O As System.Object, ByVal E As System.EventArgs) Handles cmbRoles.SelectedIndexChanged

            'Base on the length of user id to determine current state of program is Modify State or Create State. #11 Jan 07 - Start
            'Dim lngUserId As Long = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)
            'Base on the length of user id to determine current state of program is Modify State or Create State. #11 Jan 07 - End

            'IDim lngUserId As Longn below codes, only Interceptor, Reviewer and other roles (except Authorizer, Bank Operator, Inquiry User, Bank User) will be having Authentication Code Field when Create Role. Else, hide the Authentication Code.
            Dim lngUserId As Long
            If Request.QueryString("ID") IsNot Nothing Then
                lngUserId = clsEncryption.Cryptography(Request.QueryString("ID"))
            Else
                lngUserId = 0
            End If
            Select Case cmbRoles.SelectedValue
                Case gc_UT_Auth
                    'Hide Authentication Code if the token setting is true; Else, display the Authentication Code Field when Create Role (not Edit Role). #15 Jan 07 - Start
                    If CBool(Session(gc_Ses_Token)) Then
                        trAuthCode.Visible = False
                    Else
                        If lngUserId = 0 Then
                            trAuthCode.Visible = True
                        Else
                            trAuthCode.Visible = False
                        End If
                    End If
                    'Hide Authentication Code if the token setting is true; Else, display the Authentication Code Field when Create Role (not Edit Role). #15 Jan 07 - End
                    trLimit.Visible = True
                    trDisplay.Visible = True
                    Me.trEmailAlert.Visible = False
                Case gc_UT_Auth, gc_UT_BankOperator
                    'If Inquiry User or Bank Operator
                    trAuthCode.Visible = False
                    trDisplay.Visible = False
                    Me.trEmailAlert.Visible = False
                Case gc_UT_BankUser
                    trAuthCode.Visible = True
                    trDisplay.Visible = False
                    Me.trEmailAlert.Visible = True
                Case gc_UT_Interceptor
                    If lngUserId = 0 Then
                        trAuthCode.Visible = True
                    Else
                        trAuthCode.Visible = False
                    End If
                    trLimit.Visible = False
                    trDisplay.Visible = False
                    Me.trEmailAlert.Visible = False
                Case gc_UT_BankDownloader
                    trAuthCode.Visible = False
                    trLimit.Visible = False
                    trDisplay.Visible = False
                    trEmailAddress.Visible = True
                Case gc_UT_Reviewer
                    If lngUserId = 0 Then
                        trAuthCode.Visible = True
                    Else
                        trAuthCode.Visible = False
                    End If

                    trDisplay.Visible = True
                    Me.trEmailAlert.Visible = False
                Case gc_UT_InquiryUser, gc_UT_ReportDownloader
                    trAuthCode.Visible = False
                    trLimit.Visible = False
                    trDisplay.Visible = False
                    Me.trEmailAlert.Visible = False
                Case Else
                    If lngUserId = 0 Then
                        trAuthCode.Visible = True
                    Else
                        trAuthCode.Visible = False
                    End If
                    trLimit.Visible = False
                    trDisplay.Visible = False
                    Me.trEmailAlert.Visible = False
            End Select


        End Sub

#End Region

#Region "Create New"

        '****************************************************************************************************
        'Procedure Name : pcrNew()
        'Purpose        : Create New User Role
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 18/02/2005
        '*****************************************************************************************************
        'Private Sub prcNew(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnNew.Click

        '    Try
        '        Server.Transfer("PG_CreateRole.aspx", False)
        '    Catch ex As Exception

        '    End Try

        'End Sub

#End Region

#Region "Limit Change"

        Private Sub prcLimit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles rdNLimit.CheckedChanged

            Try
                If rdNLimit.Checked Then
                    txtLimit.Text = "0.00"
                End If
            Catch ex As Exception

            End Try

        End Sub

#End Region

        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function

        Private Sub Page_BacView(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnBackToView.Click
            Dim userId = Request.Form("ctl00$cphContent$hUserId")
            If userId IsNot Nothing AndAlso userId <> "0" Then
                Dim strURL = "PG_CreateRole.aspx?Id=" & userId
                Server.Transfer(strURL, False)
            Else
                Dim strURL = "PG_CreateRole.aspx"
                Server.Transfer(strURL, False)
            End If
        End Sub
    End Class

End Namespace
