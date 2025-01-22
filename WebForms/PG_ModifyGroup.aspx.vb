Namespace MaxPayroll

Partial Class PG_ModifyGroup
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

                If Not rq_strMod = "View" Then

                    hRequest.Value = ""
                Else
                    trBack.Visible = True
                    trSubmit.Visible = False
                    hRequest.Value = rq_strMode
                End If


                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)        'Get Auth Status
                lngApprId = IIf(IsNumeric(Request.QueryString("APPR")), Request.QueryString("APPR"), 0) 'Get Approval Id

                'Get Authorization Lock Status - Start
                If strAuthLock = "Y" Then
                    Server.Transfer("PG_ListGroup.aspx?Err=Auth")
                    Exit Try
                End If
                'Get Authorization Lock Status - Stop

                If Not Page.IsPostBack Then
                    'BindBody(body)


                    BindFormatGrid()

                    'Main Form Table
                    tblMainForm.Visible = True

                    'Confirm Form Table
                    tblConfirm.Visible = False

                    'Heading Text
                    lblHeading.Text = "Group Modification"

                    'EPF Service Status
                    'hEpf.Value = clsCommon.fncBuildContent("EPF", "EPF File", ss_lngOrgID, ss_lngUserID)

                    'hBilling.Value = clsCommon.fncBuildContent("BILLING", "Billing File", ss_lngOrgID, ss_lngUserID)
                    'EPF Service Status
                    'hSoc.Value = clsCommon.fncBuildContent("SOCSO", "SOCSO File", ss_lngOrgID, ss_lngUserID)

                    'Payroll Service Status
                    'hPayroll.Value = clsCommon.fncBuildContent("PAYROLL", "Payroll File", ss_lngOrgID, ss_lngUserID)

                    'LHDN Service Status
                    'hLHDN.Value = clsCommon.fncBuildContent("LHDN", "LHDN File", ss_lngOrgID, ss_lngUserID)

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
                    'dsGroup.Reset()
                    'dsGroup = clsCustomer.fncGrpCommon("BANK DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                    'Call fncPopulateCheckBox(dsGroup, chkBankAccts, "ACCNAME", "ACCID")
                    'Populate Bank Accounts - STOP
                    BindAccountGrid()



                    ''Populate EPF Accounts - START
                    'If hEpf.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("EPF DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                    '    Call fncPopulateCheckBox(dsGroup, chkEpfAccts, "SRNAME", "SRID")
                    'Else
                    '    'trEPF.Visible = False
                    '    trEpfAccounts.Visible = False
                    'End If
                    ''Populate EPF Accounts - STOP

                    ''Populate Socso Accounts - START
                    'If hSoc.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("SOCSO DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                    '    Call fncPopulateCheckBox(dsGroup, chkSocAccts, "SRNAME", "SRID")
                    'Else
                    '    'trSoc.Visible = False
                    '    trSocAccounts.Visible = False
                    'End If
                    ''Populate Socso Accounts - STOP

                    ''Populate LHDN Accounts - START
                    'If hLHDN.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("LHDN DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                    '    Call fncPopulateCheckBox(dsGroup, chkLHDNAccts, "SRNAME", "SRID")
                    'Else
                    '    'trLHDN.Visible = False
                    '    trLHDNAccounts.Visible = False
                    'End If
                    ''Populate LHDN Accounts - STOP

                    'Populate Payroll Files - START
                    'If hPayroll.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, "Payroll File")
                    '    Call fncPopulateCheckBox(dsGroup, chkPayroll, "FFORMAT", "FID")
                    'Else
                    '    trPayroll.Visible = False
                    'End If
                    'Populate Payroll Files - STOP

                    'Populate Billing Files - START
                    'If hBilling.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, "Billing File")
                    '    Call fncPopulateCheckBox(dsGroup, chkBilling, "FFORMAT", "FID")

                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncListGroup("BILLING TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                    '    Call prcCheckedItems(dsGroup, drGroup, chkBilling, "ACCID")
                    'End If
                    ''Populate Billing Files - STOP

                    ''Populate EPF Files - START
                    'If hEpf.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, "EPF File")
                    '    Call fncPopulateCheckBox(dsGroup, chkEpf, "FFORMAT", "FID")
                    'Else
                    '    trEPF.Visible = False
                    'End If
                    ''Populate EPF Files - STOP

                    ''Populate Socso Files - START
                    'If hSoc.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, "SOCSO File")
                    '    Call fncPopulateCheckBox(dsGroup, chkSocso, "FFORMAT", "FID")
                    'Else
                    '    trSoc.Visible = False
                    'End If
                    ''Populate Socso Files - STOP

                    ''Populate LHDN Files - START
                    'If hLHDN.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, "LHDN File")
                    '    Call fncPopulateCheckBox(dsGroup, chkLHDN, "FFORMAT", "FID")
                    'Else
                    '    trLHDN.Visible = False
                    'End If
                    'Populate LHDN Files - STOP

                    'Get Checked Bank Accounts - Start
                    dsGroup.Reset()
                    dsGroup = clsCustomer.fncListGroup("ACCTS TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                    Call prcCheckedItems(dsGroup, drGroup, chkBankAccts, "ACCID")
                    'Get Checked Bank Accounts - Stop

                    ''Get Checked Epf Accounts - Start
                    'If hEpf.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncListGroup("EPF TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                    '    Call prcCheckedItems(dsGroup, drGroup, chkEpfAccts, "ACCID")
                    'End If
                    ''Get Checked Epf Accounts - Stop

                    ''Get Checked Socso Accounts - Start
                    'If hSoc.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncListGroup("SOCSO TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                    '    Call prcCheckedItems(dsGroup, drGroup, chkSocAccts, "ACCID")
                    'End If
                    ''Get Checked Socso Accounts - Stop

                    ''Get Checked LHDN Accounts - Start
                    'If hLHDN.Value = "Y" Then
                    '    dsGroup.Reset()
                    '    dsGroup = clsCustomer.fncListGroup("LHDN TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                    '    Call prcCheckedItems(dsGroup, drGroup, chkLHDNAccts, "ACCID")
                    'End If
                    ''Get Checked LHDN Accounts - Stop

                    'dsGroup.Reset()
                    'dsGroup = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)

                    ''Get Selected Payroll File - Start
                    'If hPayroll.Value = "Y" Then
                    '    Call prcCheckedItems(dsGroup, drGroup, chkPayroll, "FID")
                    'End If
                    ''Get Selected Payroll File - Stop

                    ''Get Selected Epf File - Start
                    'If hEpf.Value = "Y" Then
                    '    Call prcCheckedItems(dsGroup, drGroup, chkEpf, "FID")
                    'End If
                    ''Get Selected Epf File - Stop

                    ''Get Selected Socso File - Start
                    'If hSoc.Value = "Y" Then
                    '    Call prcCheckedItems(dsGroup, drGroup, chkSocso, "FID")
                    'End If
                    ''Get Selected Socso File - Stop

                    ''Get Selected LHDN File - Start
                    'If hLHDN.Value = "Y" Then
                    '    Call prcCheckedItems(dsGroup, drGroup, chkLHDN, "FID")
                    'End If
                    'Get Selected LHDN File - Stop

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
                        chkBankAccts.Enabled = False
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
            Dim clsCommon As New MaxPayroll.clsCommon
            Try
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtGroupName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtGroupDesc1 As Boolean = clsCommon.CheckScriptValidation(txtGroupDesc.Text)
                If txtGroupDesc1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                IsChecked = False
                hRequest.Value = "Submit"
                trAuthCode.Visible = True
                tblConfirm.Visible = True                                                           'Confirm Form Table
                'tblMainForm.Visible = False                                                         'Main Form Table
                txtCGroupName.Text = txtGroupName.Text                                              'Group Name
                txtCGroupDesc.Text = txtGroupDesc.Text                                              'Group Description
                txtCAuth.Text = ddlAuthorizer.SelectedValue                                         'No of Authorizer's
                lblHeading.Text = "Group Modification Confirmation"                                 'Heading Text
                lngGroupId = IIf(Request.QueryString("ID"), Request.QueryString("ID"), 0)           'Group Id
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)            'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)         'Get User Id
                lblMessage.Text = "Please Enter your Validation Code & Confirm Group Details"    'Message Label

                Dim dgFormatItem As DataGridItem
                Dim dgAccountItem As DataGridItem
                For Each dgFormatItem In dgFormat.Items
                    chkFormat = New CheckBoxList
                    chkFormat = CType(dgFormatItem.Cells(0).FindControl("chkFormat"), CheckBoxList)
                    For Each dgAccountItem In dgAccount.Items
                        chkAccount = New CheckBoxList
                        chkAccount = CType(dgAccountItem.Cells(0).FindControl("chkAccount"), CheckBoxList)
                        If dgFormatItem.Cells(enmDGFormat.PaySer_Id).Text.Trim = dgAccountItem.Cells(enmDGAccount.PaySer_ID).Text.Trim Then
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
                                sMsg += dgAccountItem.Cells(enmDGAccount.PaySer_Desc).Text.Trim & " is not checked" & gc_BR
                            ElseIf bIsAccountChecked AndAlso bIsFormatChecked = False Then
                                sMsg += dgFormatItem.Cells(enmDGFormat.File_Type).Text.Trim & " is not checked" & gc_BR
                            End If
                        End If

                    Next
                Next


                prcAssignFormatData()
                prcDisableFields()
                'Check For Duplicate Group Name - START
                IsDuplicate = clsCustomer.fnOrgValidations("UPDATE", "GROUP NAME", txtGroupName.Text, lngOrgId, lngGroupId)
                If IsDuplicate Then
                    sMsg += "Group Name already exists. Please use a different name." & gc_BR
                End If
                'Check For Duplicate Group Name - STOP


                'IsChecked = fncIsChecked(chkBankAccts, lbxCBankAccts)
                'If Not IsChecked Then
                '    sMsg += "Please Select Bank Account(s)." & gc_BR
                'End If
                'Populate Selected Bank Accounts - STOP

                'Populate Selected EPF Accounts - START
                If hEpf.Value = "Y" Then
                    IsChecked = fncIsChecked(chkEpfAccts, lbxCEpfAccts)
                    If Not IsChecked Then
                        sMsg += "Please Select EPF Account(s)." & gc_BR
                    End If
                Else
                    trCEpfAccounts.Visible = False
                End If
                'Populate Selected EPF Accounts - STOP



                'Populate Selected Socso Accounts - START
                If hSoc.Value = "Y" Then
                    IsChecked = fncIsChecked(chkSocAccts, lbxCSocAccts)
                    If Not IsChecked Then
                        sMsg += "Please Select SOCSO Account(s)." & gc_BR
                    End If
                Else
                    trCSocAccounts.Visible = False
                End If
                'Populate Selected Socso Accounts - STOP

                'Populate Selected LHDN Accounts - START
                If hLHDN.Value = "Y" Then
                    IsChecked = fncIsChecked(chkLHDNAccts, lbxCLHDNAccts)
                    If Not IsChecked Then
                        sMsg += "Please Select LHDN Account(s)." & gc_BR
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
                        sMsg += "Please select " & dgi.Cells(0).Text & gc_BR
                    End If
                Next
                If Len(sMsg) > 0 Then
                    bSuccess = False
                    lblMessage.Text = sMsg
                    Exit Try
                End If
                'If hPayroll.Value = "Y" Then
                '    IsChecked = fncIsChecked(chkPayroll, lbxCPyrFormat)
                '    If Not IsChecked Then
                '        tblMainForm.Visible = True
                '        tblConfirm.Visible = False
                '        lblMessage.Text = "Please Select Payroll File Format(s)."
                '        Exit Try
                '    End If
                'Else
                '    trCPayroll.Visible = False
                'End If
                ''Populate Selected Payroll Files - STOP

                'If hBilling.Value = "Y" Then
                '    IsChecked = fncIsChecked(chkBilling, lbxCBillingFormat)
                '    If IsChecked = False Then
                '        tblMainForm.Visible = True
                '        tblConfirm.Visible = False
                '        lblMessage.Text = "Please Select Billing File Format(s)."
                '        Exit Try
                '    End If
                'End If

                ''Populate Selected EPF Files - START
                'If hEpf.Value = "Y" Then
                '    IsChecked = fncIsChecked(chkEpf, lbxCEpfFormat)
                '    If Not IsChecked Then
                '        tblMainForm.Visible = True
                '        tblConfirm.Visible = False
                '        lblMessage.Text = "Please Select EPF File Format(s)."
                '        Exit Try
                '    End If
                'Else
                '    trCEpf.Visible = False
                'End If
                ''Populate Selected EPF Files - STOP

                ''Populate Selected EPF Files - START
                'If hSoc.Value = "Y" Then
                '    IsChecked = fncIsChecked(chkSocso, lbxCSocFormat)
                '    If Not IsChecked Then
                '        tblMainForm.Visible = True
                '        tblConfirm.Visible = False
                '        lblMessage.Text = "Please Select SOCSO File Format(s)."
                '        Exit Try
                '    End If
                'Else
                '    trCSoc.Visible = False
                'End If
                ''Populate Selected EPF Files - STOP

                ''Populate Selected LHDN Files - START
                'If hLHDN.Value = "Y" Then
                '    IsChecked = fncIsChecked(chkLHDN, lbxCLHDNFormat)
                '    If Not IsChecked Then
                '        tblMainForm.Visible = True
                '        tblConfirm.Visible = False
                '        lblMessage.Text = "Please Select LHDN File Format(s)."
                '        Exit Try
                '    End If
                'Else
                '    trCLHDN.Visible = False
                'End If
                'Populate Selected LHDN Files - STOP

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
            'dgFormat.Visible = False

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
            Dim lngTransId As Long, intTotalItems As Int16, strVerify As String, strVerifier As String, strSubject As String
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
                lngGroupId = clsCustomer.fncGrpInsUpd("UPDATE", lngGroupId, lngOrgId, lngUserId, intApproved)

                If lngGroupId > 0 Then

                    'Delete all the Account Record - START
                    Call clsCustomer.prcGrpDelTrans("DELETE", lngGroupId, lngOrgId, lngUserId)
                    'Delete all the Account Record - END

                    arrTemp = New ArrayList
                    Dim dgi As DataGridItem
                    Dim lsi As ListItem

                    For Each dgi In Me.dgFormat.Items
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
                        sMsg = "Saving File Format Failed"
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
                        sMsg = "Saving Group Account Failed"
                        Exit Try
                    End If
                    'intTotalItems = lbxCPyrFormat.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCPyrFormat.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next

                    'arrTemp.Add()
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCBankAccts.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("BANK ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next

                    ''Delete Bank Accounts - START
                    'Call clsCustomer.prcGrpDelTrans("BANK ACCTS", lngGroupId, lngOrgId, lngUserId)
                    ''Delete Bank Accounts - STOP

                    'Insert Bank Accounts - START
                    'intTotalItems = lbxCBankAccts.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCBankAccts.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("BANK ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next
                    'Insert Bank Accounts - STOP

                    'Delete Epf Accounts - START
                    'Call clsCustomer.prcGrpDelTrans("EPF ACCTS", lngGroupId, lngOrgId, lngUserId)
                    'Delete Epf Accounts - STOP

                    'Insert EPF Accounts - START
                    intTotalItems = lbxCEpfAccts.Items.Count
                    For intCounter = 0 To intTotalItems - 1
                        lngTransId = lbxCEpfAccts.Items(intCounter).Value
                        Call clsCustomer.prcGrpTrans("EPF ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    Next
                    'Insert EPF Accounts - STOP

                    'Insert Socso Accounts - START
                    intTotalItems = lbxCSocAccts.Items.Count
                    For intCounter = 0 To intTotalItems - 1
                        lngTransId = lbxCSocAccts.Items(intCounter).Value
                        Call clsCustomer.prcGrpTrans("SOCSO ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    Next
                    'Insert Socso Accounts - STOP

                    'Insert LHDN Accounts - START
                    intTotalItems = lbxCLHDNAccts.Items.Count
                    For intCounter = 0 To intTotalItems - 1
                        lngTransId = lbxCLHDNAccts.Items(intCounter).Value
                        Call clsCustomer.prcGrpTrans("LHDN ACCTS", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    Next
                    ''Insert LHDN Accounts - STOP

                    ''Delete File Format - START
                    'Call clsCustomer.prcGrpDelTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId)
                    ''Delete File Format - STOP

                    ''Payroll File Format - START
                    'intTotalItems = lbxCPyrFormat.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCPyrFormat.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next
                    ''Payroll File Format - STOP

                    ''Billing File Format - START
                    'intTotalItems = lbxCBillingFormat.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCBillingFormat.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next
                    ''Billing File Format - STOP


                    ''EPF File Format - START
                    'intTotalItems = lbxCEpfFormat.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCEpfFormat.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next
                    ''EPF File Format - STOP

                    ''SOCSO File Format - START
                    'intTotalItems = lbxCSocFormat.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCSocFormat.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next
                    ''SOCSO File Format - STOP

                    ''LHDN File Format - START
                    'intTotalItems = lbxCLHDNFormat.Items.Count
                    'For intCounter = 0 To intTotalItems - 1
                    '    lngTransId = lbxCLHDNFormat.Items(intCounter).Value
                    '    Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, lngOrgId, lngUserId, lngTransId)
                    'Next
                    'LHDN File Format - STOP

                    trNew.Visible = True
                    trConfirm.Visible = False
                    trAuthCode.Visible = False

                    'If Single Verification
                    If UCase(strVerify) = "SINGLE" Then
                        lblMessage.Text = "Group Modified Successfully"
                        'If Dual Verification
                    ElseIf UCase(strVerify) = "DUAL" Then

                        'Get System Authorizer
                        strVerifier = clsCommon.fncBuildContent("System Auth", "", lngOrgId, lngUserId)
                        If IsNumeric(Trim(strVerifier)) Then
                            lngToId = Trim(strVerifier)
                        End If

                        'Mail Subject
                        strSubject = txtCGroupName.Text & " Group Modified"

                        'Update Approval Matrix
                        If hStatus.Value = "D" Then
                            'if group cancelled
                            Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserId, lngToId, lngGroupId, strSubject, "Group Cancellation", "", 1, txtCReason.Text)
                        Else
                            'if group not cancelled
                            Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserId, lngToId, lngGroupId, strSubject, "Group Modification", "", 1, txtCReason.Text)
                        End If

                        'Mail Body
                        strBody = strSubject & " , pending for approval."

                        'Send Mail
                        Call clsCommon.prcSendMails("SEND MAIL", lngOrgId, lngUserId, 0, strSubject, strBody, lngToId)

                        'Display Message
                        lblMessage.Text = "Group Modified Successfully. Request sent for approval."

                    End If

                Else

                    trConfirm.Visible = True
                    lblMessage.Text = "Group Modification Failed"

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
                    lblMessage.Text = "Group Modification Failed"
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
            Dim bIsDefaultBank As Boolean = clsCommon.fncDefaultBankChecking()
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

                    'Get all the existing items and assign checked - Start
                    dsFormat.Reset()
                    dsFormat = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)

                    Call prcCheckedItems(dsFormat, drFormat, chkBox, "FID")

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
            chkbox = 2
            BankName = 0
            BankID = 3
            PaySer_ID = 4
        End Enum

        Protected Sub dgAccount_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgAccount.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim chkBox As New CheckBoxList
                    Dim clsCustomer As New clsCustomer
                    chkBox = CType(e.Item.FindControl("chkAccount"), CheckBoxList)
                    'chkBox.DataSource = clsCustomer.fncQrySelectedGroupBankAccount(ss_lngOrgID, Me.rq_lngGroupID, iDefaultBankID)
                    'clsCustomer.prcBindBankAccountCheckBox(chkBox, ss_lngOrgID, Me.rq_lngGroupID, CInt(e.Item.Cells(enmDGAccount.BankID).Text), CInt(e.Item.Cells(enmDGAccount.PaySer_ID).Text))
                    e.Item.Cells(enmDGAccount.PaySer_Desc).Text = fncConcateAccount(e.Item.Cells(enmDGAccount.BankName).Text & " : " & e.Item.Cells(enmDGAccount.PaySer_Desc).Text)

            End Select
        End Sub
#End Region

        Private Function fncConcateAccount(ByVal sValue As String) As String
            sValue += " Account(s)"
            Return sValue
        End Function
        Private Function fncConcateFormat(ByVal sValue As String) As String
            sValue += " Format(s)"
            Return sValue
        End Function

    End Class

End Namespace
