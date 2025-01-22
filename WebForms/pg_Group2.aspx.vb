Imports System.Data.SqlClient

Namespace MaxPayroll

    Partial Class PG_Group2
        Inherits clsBasePage
        Dim clsGeneric As New MaxPayroll.Generic
        Private _PPS As New MaxMiddleware.PPS
        Private _Helper As New Helper
        Dim clsEncryption As New MaxPayroll.Encryption

#Region "Get Org Id"
        Private Function GetOrgId()
            Return IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)
        End Function

#End Region

        Private ReadOnly Property rq_iPageMode() As Integer
            Get
                If IsNumeric(Request.QueryString("PageMode")) Then
                    Return Request.QueryString("PageMode")
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
        Private ReadOnly Property bIsOneServicePerGroup() As Boolean
            Get
                Try
                    Return False 'CBool(fncAppSettings("OneServicePerGroup"))
                Catch
                    Return False
                End Try
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
                If Request.QueryString("ID") IsNot Nothing Then
                    Return CLng(clsEncryption.Cryptography(Request.QueryString("ID")))
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
        Enum enmTable
            AllTables
            Organization
            GroupDetails
            PaymentService
        End Enum

        Public Function fncGetData(ByVal sConnection As String) As DataSet
            Dim dsRetVal As New DataSet
            Dim sqlConn As SqlConnection = New SqlConnection(sConnection)
            Dim daTemp As SqlDataAdapter
            Try
                sqlConn.Open()
                daTemp = New SqlDataAdapter("SELECT * FROM ORG_MASTER", sqlConn)
                daTemp.Fill(dsRetVal, "Organization")
                daTemp = New SqlDataAdapter("SELECT * FROM mCor_GroupDetails WHERE Group_Status = 'A' AND Group_Approved = 2", sqlConn)
                daTemp.Fill(dsRetVal, "GroupDetails")
                daTemp = New SqlDataAdapter("SELECT * FROM mCor_PaymentService WHERE PaySrvStatus = 'A'", sqlConn)
                daTemp.Fill(dsRetVal, "PaymentServices")
            Catch ex As Exception
                Throw ex
            Finally
                sqlConn.Close()
            End Try
            Return dsRetVal
        End Function

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
            Dim dsTemp As New System.Data.DataSet

            'Variable Declarations
            Dim intApproved As Int16, strVerify As String, lngApprId As Long
            Dim strAuthLock As String

            Try


             

                Dim a As DataSet = fncGetData(Me.sConnectionString)
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

                    'Populate No Reviewer
                    Call BindNoOfUserType(ddlNoReviewer, "R")

                    'Populate No Approver
                    Call BindNoOfUserType(ddlAuthorizer, "A")

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
                    dsTemp = clsCustomer.fncListGroup("GROUP ID", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                    If bIsOneServicePerGroup Then
                        Me.BindDDLPaymentService()
                    Else
                        trPaymentService.Visible = False
                    End If
                    Dim stat = 0
                    'Fill Form Values - Start
                    If dsTemp.Tables("LIST").Rows.Count > 0 Then

                        For Each drGroup In dsTemp.Tables("LIST").Rows
                            If ss_lngOrgID <> drGroup("OrgId") Then
                                stat = 1
                                Exit For
                            End If
                            txtGroupName.Text = drGroup("GNAME")
                            txtGroupDesc.Text = drGroup("GDESC")
                            intApproved = drGroup("GAPPR")
                            ddlAuthorizer.SelectedValue = drGroup("GAUTH")
                            ddlNoReviewer.SelectedValue = drGroup("NoReviewer")
                            rdStatus.SelectedValue = drGroup("GSTATUS")
                            ddlPaymentService.SelectedValue = drGroup("GPAYSRV")
                        Next
                        If stat = 1 Then
                            HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                            Exit Try
                        End If
                    End If
                    'Fill Form Values - Stop

                    'Populate Bank Accounts - START
                    dsTemp.Reset()
                        dsTemp = clsCustomer.fncGrpCommon("BANK DEFAULT", ss_lngOrgID, ss_lngUserID, "")
                        If Not fncPopulateCheckBox(dsTemp, chkBankAccts, "ACCNAME", "ACCID") Then
                            Server.Transfer("PG_ListGroup.aspx?Err=Bank")
                            Exit Try
                        End If

                        'Populate Bank Accounts - START
                        'BindAccountGrid()

                        BindFormatGrid()
                        'Populate Bank Accounts - STOP



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


                            'Get all the existing items and assign checked - Start
                            dsTemp.Reset()
                            dsTemp = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)

                            'Commented the code as all the bank accounts are getting selected bydefault-on 02-12-10
                            'Call prcCheckedItems(dsTemp, chkBankAccts, "FID")
                            clsCustomer.prcBindBankAccountCheckBox(chkBankAccts, ss_lngOrgID, Me.rq_lngGroupID)
                            'Get all the existing items and assign checked - End

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
                                    If rq_iPageMode <> enmPageMode.ViewMode Then
                                        lblMessage.Text = "Cannot Modify this Group. Pending for approval."
                                    End If

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
                            If rq_iPageMode = mdConstant.enmPageMode.ViewMode Then

                                prcDisableFields()
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
                dsTemp = Nothing

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

            Dim clsCommon As New MaxPayroll.clsCommon
            'Variable Declarations
            Dim IsChecked As Boolean
            Dim lngOrgId As Long
            Dim lngUserId As Long
            Dim IsDuplicate As Boolean
            Dim lngGroupId As Long
            Dim bSuccess As Boolean = True
            Dim sMsg As String = ""
            Dim sMsg2 As String = ""
            Dim chkFormat As CheckBoxList
            Dim chkAccount As CheckBoxList
            Dim chkFormatItem As ListItem
            Dim chkAccountItem As ListItem
            Dim bIsAccountChecked As Boolean
            Dim bIsFormatChecked As Boolean
            Dim bIsChecked As Boolean
            Dim bIsChecked2 As Boolean
            Try

                IsChecked = False
                hRequest.Value = "Submit"
     
                If bIsNewPage Then
                    lblHeading.Text = "Group Creation Confirmation"
                Else
                    lblHeading.Text = "Group Modification Confirmation"
                End If
                'Heading Text
                If Request.QueryString("ID") IsNot Nothing Then
                    lngGroupId = CLng(clsEncryption.Cryptography(Request.QueryString("ID")))
                Else
                    lngGroupId = 0
                End If
                '  lngGroupId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)           'Group Id
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)            'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)         'Get User Id
                lblMessage.Text = "Please Enter your Validation Code & Confirm Group Details"    'Message Label

                If Me.txtGroupName.Text.Trim = "" Then
                    sMsg += "Group Name cannot be blank." & gc_BR
                End If

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

                Dim lItem As ListItem
                Dim bIsSelected As Boolean = False
                For Each lItem In Me.chkBankAccts.Items
                    If lItem.Selected = True Then
                        bIsSelected = True
                        Exit For
                    End If
                Next
                If bIsSelected = False Then
                    sMsg += "Please select a bank account." & gc_BR
                End If


                'Check For Duplicate Group Name - START
                IsDuplicate = clsCustomer.fnOrgValidations("UPDATE", "GROUP NAME", txtGroupName.Text, lngOrgId, lngGroupId)
                If IsDuplicate Then
                    sMsg += "Group Name already exists. Please use a different name." & gc_BR
                End If
                'Check For Duplicate Group Name - STOP

                Dim sPleaseSelect As String = fncBindText("Please Select")
                Dim sAccount As String = fncBindText("Account(s).")


                'Populate Selected Payroll Files - START
                Dim dgi As DataGridItem

                Dim chkboxlist As New CheckBoxList
                Dim lsItem As New ListItem

                For Each dgi In dgFormat.Items
                    bIsChecked = False
                    bIsChecked2 = False
                    chkboxlist = CType(dgi.FindControl("chkFormat"), CheckBoxList)
                    For Each lsItem In chkboxlist.Items
                        If lsItem.Selected Then
                            bIsChecked = True
                        End If
                    Next
                    chkboxlist = CType(dgi.FindControl("chkStatutory"), CheckBoxList)
                    For Each lsItem In chkboxlist.Items
                        If lsItem.Selected Then
                            bIsChecked2 = True
                        End If
                    Next
                    If bIsChecked = False Then
                        'sMsg += sPleaseSelect & " " & dgi.Cells(0).Text & gc_BR
                        If dgi.Cells(enmDGFormat.PayStatutory).Text <> gc_Space AndAlso bIsChecked2 Then
                            sMsg2 += dgi.Cells(0).Text & "'s Statutory Number is checked but the File Format is not checked." & gc_BR
                        End If
                    Else
                        If dgi.Cells(enmDGFormat.PayStatutory).Text <> gc_Space AndAlso bIsChecked2 = False Then
                            sMsg2 += dgi.Cells(0).Text & "'s File Format is checked but the Statutory Number is not checked." & gc_BR
                        End If
                    End If

                Next

                If Me.ddlAuthorizer.SelectedValue = "0" Then
                    sMsg2 += "Please make selection on No. of Approver(s) for Approval." & gc_BR
                End If

                If bIsNewPage = False AndAlso txtReason.Text.Trim = "" Then
                    sMsg2 += "Modification Reason cannot be blank." & gc_BR
                End If

                If Len(sMsg) > 0 Or Len(sMsg2) > 0 Then
                    bSuccess = False
                    lblMessage.Text = sMsg & sMsg2
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

                'If Session(gc_Ses_VerificationType) = "DUAL" Then
                '    trReason.Visible = True
                txtCReason.Text = txtReason.Text
                'Else
                '    trCReason.Visible = False
                'End If

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
            For Each dgi In dgFormat.Items
                chkboxlist = New CheckBoxList
                chkboxlist = CType(dgi.FindControl("chkFormat"), CheckBoxList)
                chkboxlist.Enabled = False

                chkboxlist = New CheckBoxList
                chkboxlist = CType(dgi.FindControl("chkStatutory"), CheckBoxList)
                chkboxlist.Enabled = False
            Next
            Me.rdStatus.Enabled = False
            Me.txtGroupDesc.ReadOnly = True
            Me.txtGroupName.ReadOnly = True
            Me.txtReason.ReadOnly = True
            Me.ddlAuthorizer.Enabled = False
            Me.ddlNoReviewer.Enabled = False
            Me.chkBankAccts.Enabled = False
            Me.chkEpfAccts.Enabled = False
            Me.chkLHDNAccts.Enabled = False
            Me.chkSocAccts.Enabled = False
            Me.btnReset.Visible = False
            If bIsNewPage Then
                Me.trReason.Visible = False
            Else
                Me.trReason.Visible = True
            End If

            ddlPaymentService.Enabled = False
        End Sub

        Private Sub prcAssignConfirmedData()
            Me.hGroupDesc.Value = txtGroupDesc.Text
            Me.hGroupName.Value = txtGroupName.Text
            Me.hAuthNo.Value = ddlAuthorizer.SelectedValue
            Me.hReviewerNo.Value = ddlNoReviewer.SelectedValue
            Me.hPaymentService.Value = ddlPaymentService.SelectedValue
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
            Dim iApprID As Integer
            Dim strAuthCode As String, intApproved As Int16, IsAuthCode As Boolean, intAttempts As Int16, strBody As String
            Dim arrTemp As ArrayList
            Dim arrTemp2 As ArrayList
            Dim bSuccess As Boolean = False
            Dim sMsg As String = ""
            Dim sqlTrans As SqlClient.SqlTransaction
            Dim sqlConn As SqlClient.SqlConnection = New SqlClient.SqlConnection(sConnectionString)

            Try
                If Request.QueryString("ID") IsNot Nothing Then
                    lngGroupId = CLng(clsEncryption.Cryptography(Request.QueryString("ID")))
                Else
                    lngGroupId = 0
                End If
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)                'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
                ' lngGroupId = IIf(IsNumeric(Request.QueryString("ID")), Request.QueryString("ID"), 0)    'Get Group Id

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
                        sMsg += "Invalid Validation code. Please enter a valid Validation code."
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

                sqlConn.Open()
                sqlTrans = sqlConn.BeginTransaction

                'Create Group
                If bIsNewPage Then
                    lngGroupId = clsCustomer.fncGrpInsUpd(sqlTrans, "INSERT", 0, ss_lngOrgID, ss_lngUserID, intApproved)
                Else
                    lngGroupId = clsCustomer.fncGrpInsUpd(sqlTrans, "UPDATE", lngGroupId, lngOrgId, lngUserId, intApproved)
                End If

                If lngGroupId > 0 Then
                    'Delete all the Account Record - START
                    Call clsCustomer.prcGrpDelTrans(sqlTrans, "DELETE", lngGroupId, lngOrgId, lngUserId)
                    'Delete all the Account Record - END

                    arrTemp = New ArrayList
                    arrTemp2 = New ArrayList
                    Dim dgi As DataGridItem
                    Dim lsi As ListItem

                    For Each dgi In Me.dgFormat.Items
                        Dim chkTemp As New CheckBoxList

                        'Prepare data of Selected File Format for saving - Start
                        chkTemp = CType(dgi.FindControl("chkFormat"), CheckBoxList)
                        For Each lsi In chkTemp.Items
                            If lsi.Selected Then
                                arrTemp.Add(lsi.Value)
                            End If
                        Next
                        'Prepare data of Selected File Format for saving - End

                        'Prepare data of Selected Statutory for saving - Start
                        chkTemp = CType(dgi.FindControl("chkStatutory"), CheckBoxList)
                        For Each lsi In chkTemp.Items
                            If lsi.Selected Then
                                arrTemp2.Add(lsi.Value)
                            End If
                        Next
                        'Prepare data of Selected Statutory for saving - End

                    Next
                    bSuccess = clsCustomer.prcSaveFileFormat(sqlTrans, lngGroupId, arrTemp)
                    If bSuccess = False Then
                        sMsg += "Saving File Format Failed." & gc_BR
                    End If
                    bSuccess = clsCustomer.prcSaveStatutoryAccount(sqlTrans, lngGroupId, arrTemp2)
                    If bSuccess = False Then
                        sMsg += "Saving Statutory Account Failed." & gc_BR
                    End If
                    arrTemp.Clear()

                    Dim lsItem As ListItem
                    For Each lsItem In chkBankAccts.Items
                        If lsItem.Selected = True Then
                            arrTemp.Add(lsItem.Value)
                        End If
                    Next
                    bSuccess = clsCustomer.prcSaveGroupAccount(sqlTrans, lngGroupId, arrTemp)
                    If bSuccess = False Then
                        sMsg += "Saving Group Account Failed." & gc_BR
                    End If

                    
                    If bSuccess Then
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
                                iApprID = clsApprMatrix.prcApprovalMatrix(sqlTrans, lngOrgId, lngUserId, "INSERT", 0, lngUserId, lngToId, lngGroupId, strSubject, "Group Cancellation", "", 1, txtCReason.Text)
                            Else
                                'if group not cancelled
                                iApprID = clsApprMatrix.prcApprovalMatrix(sqlTrans, lngOrgId, lngUserId, "INSERT", 0, lngUserId, lngToId, lngGroupId, strSubject, IIf(bIsNewPage, "Group Creation", "Group Modification"), "", 1, txtCReason.Text)
                            End If

                            'Mail Body
                            strBody = strSubject & ", pending for approval."

                            'Send Mail
                            Call clsCommon.prcSendMails(sqlTrans, "SEND MAIL", lngOrgId, lngUserId, 0, strSubject, strBody, lngToId)

                            'Display Message
                            If bIsNewPage Then
                                lblMessage.Text = "Group Created Successfully. Request sent for approval."
                            Else
                                lblMessage.Text = "Group Modified Successfully. Request sent for approval."
                            End If

                            sqlTrans.Commit()
                            sqlTrans = Nothing
                        End If
                    Else
                        sqlTrans.Rollback()
                        sqlTrans = Nothing
                    End If
                Else
                    trConfirm.Visible = True
                    If bIsNewPage Then
                        sMsg += "Group Creation Failed."
                    Else
                        sMsg += "Group Modification Failed."
                    End If
                End If

            Catch
                If Not sqlTrans Is Nothing Then
                    sqlTrans.Rollback()
                End If

                trConfirm.Visible = True
                'If bIsNewPage Then
                '    sMsg += "Group Creation Failed."
                'Else
                '    lblMessage.Text = "Group Modification Failed."
                'End If

                'Log Errorr
                LogError("Page_Submit - PG_ModifyGroup")

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
                    For Each drCheckBoxList In dsCheckBoxList.Tables(0).Rows
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

        Private Sub prcCheckedItems(ByVal dsGroup As DataSet, ByVal chkGroup As CheckBoxList, ByVal strColumnName As String)

            'Variable Declarations
            Dim lngTransId As Long, lngTranId As Long, intCounter As Int16, drGroup As DataRow

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
            lblMessage.Text = ""
            If bIsOneServicePerGroup Then
                Me.dgFormat.DataSource = clsFileSetting.fncGetFileSetting(Me.ss_lngOrgID, Me.ddlPaymentService.SelectedValue)
            Else
                Me.dgFormat.DataSource = clsFileSetting.fncGetFileSetting(Me.ss_lngOrgID)
            End If

            dgFormat.DataBind()
        End Sub
        Private Sub BindCFormatGrid()

            Dim clsFileSetting As New clsFileSetting

            Me.dgCFormat.DataSource = clsFileSetting.fncGetFileSetting(Me.ss_lngOrgID)
            dgCFormat.DataBind()
        End Sub
#End Region

#Region "BindDDL"
        Private Sub BindDDLPaymentService()
            Dim clsFileSetting As New clsFileSetting
            Me.ddlPaymentService.DataSource = clsFileSetting.fncGetFileSetting(Me.ss_lngOrgID)
            ddlPaymentService.DataTextField = "File_Type"
            ddlPaymentService.DataValueField = "PaySer_Id"
            ddlPaymentService.DataBind()
            If bIsNewPage Then
                ddlPaymentService.Items.Insert(0, New ListItem("Select", 0))
            End If

        End Sub
#End Region

#Region "Datagrid_ItemDataBound"

        Enum enmDGFormat
            File_Type = 0
            chkboxFormat = 1
            chkStatutory = 2
            PayStatutory = 3
            PaySer_Id = 4
            PaySrvCode = 5
        End Enum

        Protected Sub dgFormat_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgFormat.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    Dim chkBox As New CheckBoxList
                    Dim sMsg As String = ""

                    Dim dsFormat As New DataSet

                    'Get all the File Format according to File Type -  Start
                    Dim clsCustomer As New clsCustomer

                    'dsFormat.Reset()
                    If e.Item.Cells(enmDGFormat.PayStatutory).Text <> "&nbsp;" Then
                        dsFormat = clsCustomer.fncGrpCommon("STATUTORY DEFAULT", ss_lngOrgID, ss_lngUserID, e.Item.Cells(enmDGFormat.PaySer_Id).Text, e.Item.Cells(enmDGFormat.PayStatutory).Text)

                        If dsFormat.Tables(0).Rows.Count = 0 Then
                            Dim lbl As New Label
                            lbl = CType(e.Item.FindControl("lblStatutory"), Label)
                            lbl.Text = "N/A"
                            lbl.ForeColor = Color.Red
                            sMsg = e.Item.Cells(0).Text + " statutory number(s)"
                        Else
                            chkBox = CType(e.Item.FindControl("chkStatutory"), CheckBoxList)
                            fncPopulateCheckBox(dsFormat, chkBox, "SRNAME", "SRID")
                            chkBox.CellPadding = 0
                            chkBox.CellSpacing = 0
                        End If

                        If bIsNewPage = False Then
                            If chkBox.Items.Count > 0 Then
                                dsFormat = clsCustomer.fncListGroup("STATUTORY TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                                Call prcCheckedItems(dsFormat, chkBox, "ACCID")
                            End If
                        End If
                    Else
                        Dim lbl As New Label
                        lbl = CType(e.Item.FindControl("lblStatutory"), Label)
                        lbl.Text = "N/A"
                    End If


                    chkBox = CType(e.Item.FindControl("chkFormat"), CheckBoxList)
                    dsFormat = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, e.Item.Cells(0).Text)
                    Call fncPopulateCheckBox(dsFormat, chkBox, "FFORMAT", "FID")
                    chkBox.CellPadding = 0
                    chkBox.CellSpacing = 0
                    'Get all the File Format according to File Type - End

                    If bIsNewPage = False Then
                        If chkBox.Items.Count > 0 Then
                            'Get all the existing items and assign checked - Start
                            'dsFormat.Reset()
                            dsFormat = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                            Call prcCheckedItems(dsFormat, chkBox, "FID")
                            'Get all the existing items and assign checked - End
                        Else
                            sMsg = "File Format" & IIf(Len(sMsg) > 0, "/" & sMsg, "")

                            Dim lblTemp As New Label
                            lblTemp = CType(e.Item.FindControl("lblFormatError"), Label)
                            lblTemp.Text = "N/A"
                        End If
                    End If
                    If Len(sMsg) > 0 Then
                        lblMessage.Text = sMsg & " not found. Please contact " & gc_UT_BankAdminDesc & " at " & gc_Const_CompanyContactNo & "."
                    End If
                    e.Item.Cells(enmDGFormat.File_Type).Text = fncConcateFormat(e.Item.Cells(enmDGFormat.File_Type).Text)
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

                    'Bind Drop Down List for Bank Account - Start
                    chkBox = CType(e.Item.FindControl("chkAccount"), CheckBoxList)
                    'clsCustomer.prcBindBankAccountCheckBox(chkBox, ss_lngOrgID, Me.rq_lngGroupID, CInt(e.Item.Cells(enmDGAccount.BankID).Text), CInt(e.Item.Cells(enmDGAccount.PaySer_ID).Text))
                    'Bind Drop Down List for Bank Account - End


                    chkBox = CType(e.Item.FindControl("chkFormat"), CheckBoxList)
                    'Get all the File Format according to File Type -  Start
                    dsFormat = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgID, ss_lngUserID, e.Item.Cells(enmDGAccount.PaySer_Desc).Text)
                    Call fncPopulateCheckBox(dsFormat, chkBox, "FFORMAT", "FID")
                    'Get all the File Format according to File Type - End

                    If bIsNewPage = False Then
                        'Get all the existing items and assign checked - Start
                        dsFormat.Reset()
                        dsFormat = clsCustomer.fncListGroup("FILE TRANS", ss_lngOrgID, ss_lngUserID, rq_lngGroupID)
                        'Get all the existing items and assign checked - End

                        Call prcCheckedItems(dsFormat, chkBox, "FID")
                    End If


                    e.Item.Cells(enmDGAccount.PaySer_Desc).Text = IIf(e.Item.Cells(enmDGAccount.IsMultipleBank).Text = "False", e.Item.Cells(enmDGAccount.BankName).Text & " : ", "") & e.Item.Cells(enmDGAccount.PaySer_Desc).Text

            End Select
        End Sub
#End Region

        Private Function fncConcateAccount(ByVal sValue As String) As String
            sValue += fncBindText(" Account(s)")
            Return sValue
        End Function
        Private Function fncConcateFormat(ByVal sValue As String) As String
            sValue += fncBindText(" Format(s)")
            Return sValue
        End Function

        Protected Sub ddlPaymentService_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPaymentService.SelectedIndexChanged
            BindFormatGrid()
        End Sub

#Region "Bind to Enum "

        Public Sub BindNoOfUserType(ByVal lc As ListControl, ByVal UserType As Char)
            Dim NoOfUser As Integer
            Dim CatchMessage As String = Nothing
            Dim ht As New Hashtable()
            Dim clsGeneric As New MaxPayroll.Generic

            NoOfUser = MaxGeneric.clsGeneric.NullToInteger(MaxMiddleware.PPS.SQLScalarValue(_Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction, CatchMessage, "CIMBGW_Get_No_Reviewer " & GetOrgId() & "," & UserType))

            For i As Integer = 1 To NoOfUser
                ht.Add(i, i)
            Next
            ' return the dictionary to be bound to
            lc.DataSource = ht
            lc.DataTextField = "Key"
            lc.DataValueField = "Value"
            lc.DataBind()
        End Sub
#End Region

    End Class
End Namespace
