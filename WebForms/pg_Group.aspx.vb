Namespace MaxPayroll

    Partial Class PG_Group
        Inherits clsBasePage

        Private ReadOnly Property rq_iFieldLock() As Integer
            Get
                If IsNumeric(Request.QueryString("FieldLock")) Then
                    Return Request.QueryString("FieldLock")
                Else
                    Return -1
                End If

            End Get
        End Property
        Private ReadOnly Property rq_strMod() As String
            Get
                Return Request.QueryString("Mod") & ""
            End Get
        End Property
        Private ReadOnly Property bIsNewPage() As Boolean
            Get
                If rq_strMod = "New" Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property
        Private ReadOnly Property rq_strMode() As String
            Get
                Return Request.QueryString("Mode") & ""
            End Get
        End Property
        Private ReadOnly Property rq_lngGroupID() As Long
            Get
                If IsNumeric(Request.QueryString("ID")) Then
                    Return CLng(Request.QueryString("ID"))
                Else
                    Return -1
                End If
            End Get
        End Property
        Private ReadOnly Property rq_iPageNo() As Integer
            Get
                If IsNumeric(Request.QueryString("PageNo")) Then
                    Return Request.QueryString("PageNo")
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property iDefaultBankID() As Integer
            Get
                If clsCommon.fncDefaultBankChecking Then
                    Return fncAppSettings(gc_WC_DefaultBank)
                Else
                    Return 0
                End If
            End Get
        End Property
#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 11/02/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Data Row
            Dim drGroup As DataRow

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Data Set
            Dim dsGroup As New System.Data.DataSet

            'Variable Declarations
            Dim intApproved As Int16, strVerify As String, lngApprId As Long
            Dim strAuthLock As String

            Try

                If Not (ss_strUserType = gc_UT_SysAdmin Or ss_strUserType = gc_UT_SysAuth) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)        'Get Auth Status
                lngApprId = IIf(IsNumeric(Request.QueryString("APPR")), Request.QueryString("APPR"), 0) 'Get Approval Id

                If Not rq_strMod = "View" Then
                    hRequest.Value = ""
                Else
                    trBack.Visible = True
                    trSubmit.Visible = False
                    hRequest.Value = rq_strMode
                End If

                'Get Authorization Lock Status - Start
                If strAuthLock = "Y" Then
                    Server.Transfer("PG_ListGroup.aspx?Err=Auth")
                    Exit Try
                End If
                'Get Authorization Lock Status - Stop

                If Not Page.IsPostBack Then
                    'BindBody(body)

                    'BindFormatGrid()
                    trConfirm.Visible = False
                    trAuthCode.Visible = False
                    'Main Form Table
                    tblMainForm.Visible = True

                    'Confirm Form Table
                    tblConfirm.Visible = False


                    'EPF Service Status
                    hEpf.Value = clsCommon.fncBuildContent("EPF", "EPF File", ss_lngOrgID, ss_lngUserID)

                    hBilling.Value = clsCommon.fncBuildContent("BILLING", "Billing File", ss_lngOrgID, ss_lngUserID)
                    'EPF Service Status
                    hSoc.Value = clsCommon.fncBuildContent("SOCSO", "SOCSO File", ss_lngOrgID, ss_lngUserID)

                    'Payroll Service Status
                    hPayroll.Value = clsCommon.fncBuildContent("PAYROLL", "Payroll File", ss_lngOrgID, ss_lngUserID)

                    'LHDN Service Status
                    hLHDN.Value = clsCommon.fncBuildContent("LHDN", "LHDN File", ss_lngOrgID, ss_lngUserID)

                    hPageNo.Value = rq_iPageNo.ToString

                    'Populate Data Set
                    dsGroup = clsCustomer.fncListGroup("GROUP ID", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)

                    'Fill Form Values - Start
                    If dsGroup.Tables("LIST").Rows.Count > 0 Then
                        For Each drGroup In dsGroup.Tables("LIST").Rows
                            txtGroupName.Text = drGroup("GNAME")
                            txtGroupDesc.Text = drGroup("GDESC")
                            intApproved = drGroup("GAPPR")
                            ddlAuthorizer.SelectedValue = drGroup("GAUTH")
                            rdStatus.SelectedValue = drGroup("GSTATUS")
                        Next
                    End If
                    'Fill Form Values - Stop

                    'Populate Bank Accounts - START
                    BindAccountGrid()
                    'Populate Bank Accounts - STOP

                    'Populate EPF Accounts - START
                    If hEpf.Value = "Y" Then
                        dsGroup.Reset()
                        dsGroup = clsCustomer.fncGrpCommon("EPF DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                        Call fncPopulateCheckBox(dsGroup, chkEpfAccts, "SRNAME", "SRID")
                    Else
                        'trEPF.Visible = False
                        trEpfAccounts.Visible = False
                    End If
                    'Populate EPF Accounts - STOP

                    'Populate Socso Accounts - START
                    If hSoc.Value = "Y" Then
                        dsGroup.Reset()
                        dsGroup = clsCustomer.fncGrpCommon("SOCSO DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                        Call fncPopulateCheckBox(dsGroup, chkSocAccts, "SRNAME", "SRID")
                    Else
                        'trSoc.Visible = False
                        trSocAccounts.Visible = False
                    End If
                    'Populate Socso Accounts - STOP

                    'Populate LHDN Accounts - START
                    If hLHDN.Value = "Y" Then
                        dsGroup.Reset()
                        dsGroup = clsCustomer.fncGrpCommon("LHDN DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                        Call fncPopulateCheckBox(dsGroup, chkLHDNAccts, "SRNAME", "SRID")
                    Else
                        'trLHDN.Visible = False
                        trLHDNAccounts.Visible = False
                    End If
                    'Populate LHDN Accounts - STOP



                    If bIsNewPage Then
                        lblHeading.Text = "Group Creation"
                        Dim sIsGrpLimit As String
                        sIsGrpLimit = clsCommon.fncBuildContent("Group Limit", "", ss_lngOrgID, ss_lngUserID)      'Get Group Limit
                        strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngUserID, ss_lngUserID)    'Get Authorization

                        If CBool(sIsGrpLimit) Then
                            Server.Transfer("PG_ListGroup.aspx?Err=Limit")
                            Exit Try
                        End If
                        'Me.trNew.Visible = True
                        'rfvReason.Enabled = False
                        trReason.Visible = False
                        hIsNew.Value = "New"
                    Else
                        lblHeading.Text = "Group Modification"
                        'Get Checked Epf Accounts - Start
                        If hEpf.Value = "Y" Then
                            dsGroup.Reset()
                            dsGroup = clsCustomer.fncListGroup("EPF TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                            Call prcCheckedItems(dsGroup, drGroup, chkEpfAccts, "ACCID")
                        End If
                        'Get Checked Epf Accounts - Stop

                        'Get Checked Socso Accounts - Start
                        If hSoc.Value = "Y" Then
                            dsGroup.Reset()
                            dsGroup = clsCustomer.fncListGroup("SOCSO TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                            Call prcCheckedItems(dsGroup, drGroup, chkSocAccts, "ACCID")
                        End If
                        'Get Checked Socso Accounts - Stop

                        'Get Checked LHDN Accounts - Start
                        If hLHDN.Value = "Y" Then
                            dsGroup.Reset()
                            dsGroup = clsCustomer.fncListGroup("LHDN TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                            Call prcCheckedItems(dsGroup, drGroup, chkLHDNAccts, "ACCID")
                        End If
                        'Get Checked LHDN Accounts - Stop

                        'Get Verification Type
                        strVerify = Session(gc_Ses_VerificationType)

                        If strVerify = "DUAL" Then
                            trReason.Visible = True
                            If lngApprId > 0 Then
                                txtReason.Text = clsCommon.fncBuildContent("Appr Reason", "", lngApprId, ss_lngUserID)
                                txtReason.ReadOnly = True
                            End If
                        ElseIf strVerify = "SINGLE" Then
                            trReason.Visible = False
                        End If
                        If Not rq_strMode = "View" Then
                            If strVerify = "DUAL" And intApproved = 1 Then
                                btnSubmit.Enabled = False
                                btnConfirm.Enabled = False
                                lblMessage.Text = "Cannot Modify this Group. Pending for approval"
                                prcDisableFields()
                                Exit Try
                            End If
                        End If

                        'if group approved and cancelled
                        If intApproved = 2 And rdStatus.SelectedValue = "D" Then
                            'disable buttons
                            btnSubmit.Enabled = False
                            btnConfirm.Enabled = False
                            lblMessage.Text = "Group has been cancelled"
                        End If
                        If rq_iFieldLock = mdConstant.enmPageMode.NonEditableMode Then
                            txtGroupName.ReadOnly = True
                            txtGroupDesc.ReadOnly = True
                            'chkBankAccts.Enabled = False
                            chkEpfAccts.Enabled = False
                            chkSocAccts.Enabled = False
                            chkLHDNAccts.Enabled = False
                            'chkPayroll.Enabled = False
                            'chkEpf.Enabled = False
                            'chkSocso.Enabled = False
                            'chkLHDN.Enabled = False
                            ddlAuthorizer.Enabled = False
                            rdStatus.Enabled = False
                            lblHeading.Text = "View Group"
                        End If
                    End If


                End If

            Catch

                'Log Error
                LogError("Page_Load - PG_ModifyGroup")

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Data Set
                dsGroup = Nothing

                'Destroy Instance of Data Row
                drGroup = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region

#Region "Confirm and validate the data before submit page to store data"

        '****************************************************************************************************
        'Procedure Name : Page_Confirm()
        'Purpose        : Page Confirm
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 11/02/2005
        '*****************************************************************************************************
        Public Sub Page_Submit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim IsChecked As Boolean
            Dim lngOrgId As Long
            Dim lngUserId As Long
            Dim IsDuplicate As Boolean
            Dim lngGroupId As Long
            Dim bSuccess As Boolean = True
            Dim sMsg As String = ""
            Dim chkFormat As CheckBoxList
            Dim chkAccount As CheckBoxList
            Dim chkFormatItem As ListItem
            Dim chkAccountItem As ListItem
            Dim bIsAccountChecked As Boolean
            Dim bIsFormatChecked As Boolean
            Dim bIsChecked As Boolean
            Try

                IsChecked = False
                hRequest.Value = "Submit"
                'trAuthCode.Visible = True
                'tblConfirm.Visible = True                                                           'Confirm Form Table
                'tblMainForm.Visible = False                                                         'Main Form Table
                'txtCGroupName.Text = txtGroupName.Text                                              'Group Name
                'txtCGroupDesc.Text = txtGroupDesc.Text                                              'Group Description
                'txtCAuth.Text = ddlAuthorizer.SelectedValue                                         'No of Authorizer's
                lblHeading.Text = "Group Modification Confirmation"                                 'Heading Text
                lngGroupId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)           'Group Id
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)            'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)         'Get User Id
                lblMessage.Text = "Please Enter your Validation Code & Confirm Group Details"    'Message Label

                If Me.txtGroupName.Text.Trim = "" Then
                    sMsg += "Group Name cannot be blank." & gc_BR
                End If

                Dim dgAccountItem As DataGridItem

                For Each dgAccountItem In dgAccount.Items
                    chkAccount = New CheckBoxList
                    chkAccount = CType(dgAccountItem.Cells(0).FindControl("chkAccount"), CheckBoxList)
                    chkFormat = New CheckBoxList
                    chkFormat = CType(dgAccountItem.Cells(0).FindControl("chkFormat"), CheckBoxList)

                    bIsChecked = False
                    bIsFormatChecked = False
                    bIsAccountChecked = False
                    For Each chkFormatItem In chkFormat.Items
                        If chkFormatItem.Selected Then
                            bIsFormatChecked = True
                            Exit For
                        End If
                    Next
                    For Each chkAccountItem In chkAccount.Items
                        If chkAccountItem.Selected Then
                            bIsAccountChecked = True
                            Exit For
                        End If
                    Next
                    If bIsAccountChecked = False AndAlso bIsFormatChecked Then
                        sMsg += dgAccountItem.Cells(enmDGAccount.PaySer_Desc).Text.Trim & "'s Account is not checked while Format is checked." & gc_BR
                    ElseIf bIsAccountChecked AndAlso bIsFormatChecked = False Then
                        sMsg += dgAccountItem.Cells(enmDGAccount.PaySer_Desc).Text.Trim & "'s Format is not checked while Account is checked." & gc_BR
                    End If

                Next

                'Check For Duplicate Group Name - START
                IsDuplicate = clsCustomer.fnOrgValidations("UPDATE", "GROUP NAME", txtGroupName.Text, lngOrgId, lngGroupId)
                If IsDuplicate Then
                    sMsg += "Group Name already exists. Please use a different name." & gc_BR
                End If
                'Check For Duplicate Group Name - STOP

                Dim sPleaseSelect As String = fncBindText("Please Select")
                Dim sAccount As String = fncBindText("Account(s).")

                'Populate Selected EPF Accounts - START
                If hEpf.Value = "Y" Then
                    IsChecked = fncIsChecked(chkEpfAccts, lbxCEpfAccts)
                    If Not IsChecked Then
                        sMsg += sPleaseSelect & " EPF " & sAccount & gc_BR
                    End If
                Else
                    trCEpfAccounts.Visible = False
                End If
                'Populate Selected EPF Accounts - STOP

                'Populate Selected Socso Accounts - START
                If hSoc.Value = "Y" Then
                    IsChecked = fncIsChecked(chkSocAccts, lbxCSocAccts)
                    If Not IsChecked Then
                        sMsg += sPleaseSelect & " SOCSO " & sAccount & gc_BR
                    End If
                Else
                    trCSocAccounts.Visible = False
                End If
                'Populate Selected Socso Accounts - STOP

                'Populate Selected LHDN Accounts - START
                If hLHDN.Value = "Y" Then
                    IsChecked = fncIsChecked(chkLHDNAccts, lbxCLHDNAccts)
                    If Not IsChecked Then
                        sMsg += sPleaseSelect & " LHDN " & sAccount & gc_BR
                        'a = Nothing
                    End If
                Else
                    trCLHDNAccounts.Visible = False
                End If
                'Populate Selected LHDN Accounts - STOP

                'Populate Selected Payroll Files - START
                Dim dgi As DataGridItem

                Dim chkboxlist As New CheckBoxList
                Dim lsItem As New ListItem
                bIsChecked = False
                For Each dgi In dgFormat.Items
                    bIsChecked = False
                    chkboxlist = CType(dgi.FindControl("chkFormat"), CheckBoxList)
                    For Each lsItem In chkboxlist.Items
                        If lsItem.Selected Then
                            bIsChecked = True
                        End If
                    Next
                    If bIsChecked = False Then
                        sMsg += sPleaseSelect & dgi.Cells(0).Text & gc_BR
                    End If
                Next

                If Me.ddlAuthorizer.SelectedValue = "0" Then
                    sMsg += "Please make selection on No. of Approver(s) for Approval." & gc_BR
                End If

                If bIsNewPage = False AndAlso txtReason.Text.Trim = "" Then
                    sMsg += "Modification Reason cannot be blank." & gc_BR
                End If

                If Len(sMsg) > 0 Then
                    bSuccess = False
                    lblMessage.Text = sMsg
                    Exit Try
                End If

                If rdStatus.SelectedValue = "A" Then
                    txtStatus.Text = "Active"
                    hStatus.Value = "A"
                ElseIf rdStatus.SelectedValue = "C" Then
                    txtStatus.Text = "Inactive"
                    hStatus.Value = "C"
                ElseIf rdStatus.SelectedValue = "D" Then
                    txtStatus.Text = "Cancelled"
                    hStatus.Value = "D"
                End If

                If Session(gc_Ses_VerificationType) = "DUAL" Then
                    trCReason.Visible = True
                    txtCReason.Text = txtReason.Text
                Else
                    trCReason.Visible = False
                End If

                prcDisableFields()
                prcAssignConfirmedData()
                prcConfirmScreen()
            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Confirm - PG_ModifyGroup", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try
            If bSuccess = False Then
                tblMainForm.Visible = True
                tblConfirm.Visible = False
            End If
        End Sub

        Private Sub prcDisableFields()
            Dim dgi As DataGridItem
            Dim chkboxlist As CheckBoxList
            For Each dgi In dgAccount.Items
                chkboxlist = New CheckBoxList
                chkboxlist = CType(dgi.FindControl("chkFormat"), CheckBoxList)
                chkboxlist.Enabled = False

                chkboxlist = New CheckBoxList
                chkboxlist = CType(dgi.FindControl("chkAccount"), CheckBoxList)
                chkboxlist.Enabled = False
            Next
            Me.rdStatus.Enabled = False
            Me.txtGroupDesc.ReadOnly = True
            Me.txtGroupName.ReadOnly = True
            Me.txtReason.ReadOnly = True
            Me.ddlAuthorizer.Enabled = False
            'Me.chkBankAccts.Enabled = False
            Me.chkEpfAccts.Enabled = False
            Me.chkLHDNAccts.Enabled = False
            Me.chkSocAccts.Enabled = False
        End Sub

        Private Sub prcAssignConfirmedData()
            Me.hGroupDesc.Value = txtGroupDesc.Text
            Me.hGroupName.Value = txtGroupName.Text
            Me.hAuthNo.Value = ddlAuthorizer.SelectedValue

        End Sub

        Private Sub prcConfirmScreen()
            Me.trAuthCode.Visible = True
            Me.trConfirm.Visible = True
            Me.trSubmit.Visible = False
        End Sub

        Private Sub prcAssignFormatData()
            Dim dgItem As DataGridItem
            Dim dgCItem As DataGridItem
            Dim chkBox As New CheckBoxList
            Dim lsBox As New ListBox
            Dim iChkBox As Integer = 0
            Dim bIsMatched As Boolean
            Dim bIsCheck As Boolean
            Dim lblFormat As New Label
            Dim txtFormat As New TextBox
            'BindCFormatGrid()
            For Each dgItem In dgFormat.Items
                chkBox = CType(dgItem.FindControl("chkFormat"), CheckBoxList)
                bIsCheck = False
                For Each dgCItem In dgCFormat.Items
                    bIsMatched = False
                    If dgItem.Cells(0).Text = dgCItem.Cells(0).Text Then
                        bIsMatched = True
                        lsBox = CType(dgCItem.FindControl("lsFormat"), ListBox)
                        lsBox.Visible = False
                        lblFormat = CType(dgCItem.FindControl("lblFormat"), Label)
                        txtFormat = CType(dgCItem.FindControl("txtFormat"), TextBox)
                        For iChkBox = 0 To chkBox.Items.Count - 1
                            If chkBox.Items(iChkBox).Selected Then
                                'lsBox.Items.Add(New ListItem(chkBox.Items(iChkBox).Text, chkBox.Items(iChkBox).Value))
                                'lblFormat.Text += chkBox.Items(iChkBox).Text & gc_BR
                                txtFormat.Text += chkBox.Items(iChkBox).Text & vbCrLf
                                bIsCheck = True
                            End If
                        Next
                    End If
                    If bIsMatched Then
                        If bIsCheck Then
                            dgCItem.Visible = True
                            'Validation to be input
                        Else
                            dgCItem.Visible = False
                        End If
                        Exit For
                    End If
                Next
            Next
        End Sub
#End Region

#Region "Submit the page to Store data"

        '****************************************************************************************************
        'Procedure Name : Page_Submit()
        'Purpose        : Page Submit
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 12/02/2005
        '*****************************************************************************************************
        Private Sub Page_Confirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Apporval Matrix Class Object
            Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

            'Variable Declarations
            Dim lngOrgId As Long, lngUserId As Long, lngGroupId As Long, intCounter As Int16, lngToId As Long
            Dim lngTransId As Long, strVerify As String, strVerifier As String, strSubject As String
            Dim strAuthCode As String, intApproved As Int16, IsAuthCode As Boolean, intAttempts As Int16, strBody As String
            Dim arrTemp As ArrayList
            Dim bSuccess As Boolean = False
            Dim sMsg As String = ""

            Try

                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
                lngGroupId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)    'Get Group Id

                'Check If AuthCode is Valid - Start
                strAuthCode = clsCommon.fncPassAuth(lngUserId, "A", lngOrgId)
                IsAuthCode = IIf(strAuthCode = txtAuthCode.Text, True, False)
                'Check If AuthCode is Valid - Stop

                'Check for invalid Authorization Code Attempts - START
                intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
                If Not intAttempts = 2 Then
                    If Not IsAuthCode Then
                        intAttempts = intAttempts + 1
                        Session("AUTH_LOCK") = intAttempts
                        lblMessage.Text = "Invalid Validation code. Please enter a valid Validation code."
                        Exit Try
                    End If
                ElseIf intAttempts = 2 Then
                    If Not IsAuthCode Then
                        trConfirm.Visible = False
                        Call clsUsers.prcAuthLock(lngOrgId, lngUserId, "A")
                        'Track Auth Lock
                        Call clsUsers.prcLockHistory(lngUserId, "A")
                        Server.Transfer("PG_ListGroup.aspx?Err=Auth")
                        Exit Try
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

                'Create Group
                If bIsNewPage Then
                    lngGroupId = clsCustomer.fncGrpInsUpd("INSERT", 0, ss_lngOrgID, ss_lngUserID, intApproved)
                Else
                    lngGroupId = clsCustomer.fncGrpInsUpd("UPDATE", lngGroupId, lngOrgId, lngUserId, intApproved)
                End If



                If lngGroupId > 0 Then
                    Dim sqlTrans As SqlClient.SqlTransaction

                    'Dim sqlConn As SqlClient.SqlCommand = new SqlClient.SqlCommand(

                    'Delete all the Account Record - START
                    Call clsCustomer.prcGrpDelTrans("DELETE", lngGroupId, lngOrgId, lngUserId)
                    'Delete all the Account Record - END

                    arrTemp = New ArrayList
                    Dim dgi As DataGridItem
                    Dim lsi As ListItem

                    For Each dgi In Me.dgAccount.Items
                        Dim chkFormat As New CheckBoxList
                        chkFormat = CType(dgi.Cells(0).FindControl("chkFormat"), CheckBoxList)

                        For Each lsi In chkFormat.Items
                            If lsi.Selected Then
                                arrTemp.Add(lsi.Value)
                            End If
                        Next
                    Next
                    bSuccess = clsCustomer.prcSaveFileFormat(lngGroupId, arrTemp)
                    If bSuccess = False Then
                        sMsg = "Saving File Format Failed."
                        Exit Try
                    End If
                    arrTemp.Clear()
                    For Each dgi In Me.dgAccount.Items
                        Dim chkAccount As New CheckBoxList
                        chkAccount = CType(dgi.Cells(0).FindControl("chkAccount"), CheckBoxList)

                        For Each lsi In chkAccount.Items
                            If lsi.Selected Then
                                arrTemp.Add(lsi.Value)
                            End If
                        Next
                    Next
                    bSuccess = clsCustomer.prcSaveGroupAccount(lngGroupId, arrTemp)
                    If bSuccess = False Then
                        sMsg = "Saving Group Account Failed."
                        Exit Try
                    End If

                    'Insert LHDN, EPF, Socso Number - Start
                    For Each lsi In Me.chkLHDNAccts.Items
                        If lsi.Selected Then
                            lngTransId = lsi.Value
                            Call clsCustomer.prcGrpTrans("LHDN ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                        End If
                    Next

                    For Each lsi In Me.chkEpfAccts.Items
                        If lsi.Selected Then
                            lngTransId = lsi.Value
                            Call clsCustomer.prcGrpTrans("EPF ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                        End If
                    Next

                    For Each lsi In Me.chkSocAccts.Items
                        If lsi.Selected Then
                            lngTransId = lsi.Value
                            Call clsCustomer.prcGrpTrans("SOCSO ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                        End If
                    Next

                    trNew.Visible = True
                    trConfirm.Visible = False
                    trAuthCode.Visible = False

                    'If Single Verification
                    If UCase(strVerify) = "SINGLE" Then
                        If bIsNewPage Then
                            lblMessage.Text = "Group Created Successfully."
                        Else
                            lblMessage.Text = "Group Modified Successfully."
                        End If

                        'If Dual Verification
                    ElseIf UCase(strVerify) = "DUAL" Then

                        'Get System Authorizer
                        strVerifier = clsCommon.fncBuildContent("System Auth", "", lngOrgId, lngUserId)
                        If IsNumeric(Trim(strVerifier)) Then
                            lngToId = Trim(strVerifier)
                        End If

                        'Mail Subject
                        strSubject = txtCGroupName.Text & IIf(bIsNewPage, " Group Created", " Group Modified")

                        'Update Approval Matrix
                        If hStatus.Value = "D" Then
                            'if group cancelled
                            Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserId, lngToId, lngGroupId, strSubject, "Group Cancellation", "", 1, txtCReason.Text)
                        Else
                            'if group not cancelled
                            Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserId, lngToId, lngGroupId, strSubject, IIf(bIsNewPage, "Group Creation", "Group Modification"), "", 1, txtCReason.Text)
                        End If

                        'Mail Body
                        strBody = strSubject & ", pending for approval."

                        'Send Mail
                        Call clsCommon.prcSendMails("SEND MAIL", lngOrgId, lngUserId, 0, strSubject, strBody, lngToId)

                        'Display Message
                        If bIsNewPage Then
                            lblMessage.Text = "Group Created Successfully. Request sent for Approval."
                        Else
                            lblMessage.Text = "Group Modified Successfully. Request sent for approval."
                        End If

                    End If

                Else

                    trConfirm.Visible = True
                    If bIsNewPage Then
                        lblMessage.Text = "Group Creation Failed."
                    Else
                        lblMessage.Text = "Group Modification Failed."
                    End If


                End If

            Catch

                'Log Errorr
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_ModifyGroup", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Approval Matrix Class Object
                clsApprMatrix = Nothing

            End Try
            If bSuccess = False Then
                trConfirm.Visible = True
                If sMsg <> "" Then
                    lblMessage.Text = sMsg
                Else
                    lblMessage.Text = "Group Modification Failed."
                End If

            End If
        End Sub


#End Region

#Region "Populate Check Box List"

        '****************************************************************************************************
        'Procedure Name : fncPopulateCheckBox()
        'Purpose        : To Check if the given Check List has an item checked
        'Arguments      : Check List
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2005
        '*****************************************************************************************************
        Private Function fncPopulateCheckBox(ByVal dsCheckBoxList As DataSet, ByRef chkGroup As CheckBoxList, _
            ByVal strDataText As String, ByVal strDataValue As String) As Boolean

            'Create Instance of DataRow
            Dim drCheckBoxList As DataRow

            Try

                'If Records Available
                If dsCheckBoxList.Tables(0).Rows.Count > 0 Then
                    'Populate Check Box List - Start
                    For Each drCheckBoxList In dsCheckBoxList.Tables("GROUP").Rows
                        chkGroup.Items.Add(New ListItem(drCheckBoxList(strDataText), drCheckBoxList(strDataValue)))
                    Next
                    'Populate Check Box List - Stop
                    Return True
                Else
                    Return False
                End If

            Catch

            Finally

                'Destroy Instance of Data Row
                drCheckBoxList = Nothing

            End Try

        End Function

#End Region

#Region "Get Checked Items"

        Private Sub prcCheckedItems(ByVal dsGroup As DataSet, ByVal drGroup As DataRow, _
                ByVal chkGroup As CheckBoxList, ByVal strColumnName As String)

            'Variable Declarations
            Dim lngTransId As Long, lngTranId As Long, intCounter As Int16

            Try

                'Checked Bank Accounts - START
                If dsGroup.Tables("LIST").Rows.Count > 0 Then
                    For Each drGroup In dsGroup.Tables("LIST").Rows
                        lngTransId = drGroup(strColumnName)
                        For intCounter = 0 To ((chkGroup.Items.Count) - 1)
                            lngTranId = chkGroup.Items(intCounter).Value
                            If lngTranId = lngTransId Then
                                chkGroup.Items(intCounter).Selected = True
                            End If
                        Next
                    Next
                End If
                'Checked Bank Accounts - STOP

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Check if Item Selected in Check box List"

        '****************************************************************************************************
        'Procedure Name : fncIsChecked()
        'Purpose        : To Check if the given Check List has an item checked
        'Arguments      : Check List
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2005
        '*****************************************************************************************************
        Private Function fncIsChecked(ByVal chkGroup As CheckBoxList, ByRef lbxGroup As ListBox) As Boolean

            'Variable Declarations
            Dim intTotalItems As Int16, IsChecked As Boolean, strDataValue As String, strDataText As String

            Try

                IsChecked = False                                           'Initiliase Status
                lbxGroup.Items.Clear()                                      'Clear Listbox Items

                'Loop Thro the Check Box - Start
                For intTotalItems = 0 To (chkGroup.Items.Count - 1)
                    'If Selected
                    If chkGroup.Items(intTotalItems).Selected Then
                        IsChecked = True                                                'Set Status 
                        strDataText = chkGroup.Items(intTotalItems).Text                'Get Data Text
                        strDataValue = chkGroup.Items(intTotalItems).Value              'Get Data Value
                        lbxGroup.Items.Add(New ListItem(strDataText, strDataValue))     'Add to List Box
                    End If
                Next
                'Loop Thro the Check Box - Stop

                Return IsChecked

            Catch ex As Exception

            End Try

        End Function


#End Region

#Region "BindGrid"
        Private Sub BindAccountGrid()
            Dim clsCustomer As New clsCustomer
            'Dim bIsDefaultBank As Boolean = clsCommon.fncDefaultBankChecking()
            Me.dgAccount.DataSource = clsCustomer.fncQryGroupBankAccountList(ss_lngOrgID, rq_lngGroupID, iDefaultBankID)
            dgAccount.DataBind()
        End Sub
        Private Sub BindFormatGrid()
            Dim clsFileSetting As New clsFileSetting
            Me.dgFormat.DataSource = clsFileSetting.fncGetFileSetting(Me.ss_lngOrgID)
            dgFormat.DataBind()
        End Sub
        Private Sub BindCFormatGrid()
            Dim clsFileSetting As New clsFileSetting
            Me.dgCFormat.DataSource = clsFileSetting.fncGetFileSetting(Me.ss_lngOrgID)
            dgCFormat.DataBind()
        End Sub
#End Region

#Region "Datagrid_ItemDataBound"

        Enum enmDGFormat
            File_Type = 0
            chkbox = 1
            BankName = 2
            IsMultiple = 3
            PaySer_Id = 4
        End Enum
        Protected Sub dgFormat_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFormat.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim chkBox As New CheckBoxList

                    chkBox = CType(e.Item.FindControl("chkFormat"), CheckBoxList)
                    Dim dsFormat As New DataSet
                    Dim drFormat As DataRow

                    'Get all the File Format according to File Type -  Start
                    Dim clsCustomer As New clsCustomer
                    dsFormat = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, e.Item.Cells(0).Text)
                    Call fncPopulateCheckBox(dsFormat, chkBox, "FFORMAT", "FID")

                    'Get all the File Format according to File Type - End

                    If bIsNewPage = False Then
                        'Get all the existing items and assign checked - Start
                        dsFormat.Reset()
                        dsFormat = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)

                        Call prcCheckedItems(dsFormat, drFormat, chkBox, "FID")
                    End If


                    e.Item.Cells(enmDGFormat.File_Type).Text = fncConcateFormat(e.Item.Cells(enmDGFormat.BankName).Text & ": " & e.Item.Cells(enmDGFormat.File_Type).Text)
                    'Get all the existing items and assign checked - End
            End Select
        End Sub

        Protected Sub dgCFormat_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgCFormat.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    e.Item.Cells(0).Text = fncConcateFormat(e.Item.Cells(0).Text)
            End Select
        End Sub

        Enum enmDGAccount
            PaySer_Desc = 1
            cbFormat = 2
            cbAccount = 3
            BankName = 0
            BankID = 4
            PaySer_ID = 5
            IsMultipleBank = 6
        End Enum

        Protected Sub dgAccount_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAccount.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim chkBox As New CheckBoxList
                    Dim clsCustomer As New clsCustomer
                    Dim dsFormat As New DataSet
                    Dim drFormat As DataRow

                    'Bind Drop Down List for Bank Account - Start
                    chkBox = CType(e.Item.FindControl("chkAccount"), CheckBoxList)
                    'clsCustomer.prcBindBankAccountCheckBox(chkBox, ss_lngOrgID, Me.rq_lngGroupID, CInt(e.Item.Cells(enmDGAccount.BankID).Text), CInt(e.Item.Cells(enmDGAccount.PaySer_ID).Text))
                    'Bind Drop Down List for Bank Account - End


                    chkBox = CType(e.Item.FindControl("chkFormat"), CheckBoxList)
                    'Get all the File Format according to File Type -  Start
                    dsFormat = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, e.Item.Cells(enmDGAccount.PaySer_Desc).Text)
                    Call fncPopulateCheckBox(dsFormat, chkBox, "FFORMAT", "FID")
                    'Get all the File Format according to File Type - End

                    If bIsNewPage = false Then
                        'Get all the existing items and assign checked - Start
                        dsFormat.Reset()
                        dsFormat = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                        'Get all the existing items and assign checked - End

                        Call prcCheckedItems(dsFormat, drFormat, chkBox, "FID")
                    End If


                    e.Item.Cells(enmDGAccount.PaySer_Desc).Text = IIf(e.Item.Cells(enmDGAccount.IsMultipleBank).Text = "False", e.Item.Cells(enmDGAccount.BankName).Text & " : ", "") & e.Item.Cells(enmDGAccount.PaySer_Desc).Text

            End Select
        End Sub
#End Region

        Private Function fncConcateAccount(ByVal sValue As String) As String
            sValue += fncBindText(" Account(s).")
            Return sValue
        End Function
        Private Function fncConcateFormat(ByVal sValue As String) As String
            sValue += fncBindText(" Format(s).")
            Return sValue
        End Function

    End Class

End Namespace
