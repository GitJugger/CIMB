Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsCustomer


Namespace MaxPayroll


Partial Class PG_CreateGroup
        Inherits clsBasePage

#Region "Request.QueryString"
        Public ReadOnly Property rq_strID() As String
            Get
                Return Request.QueryString("ID") & ""
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
    'Created        : 11/02/2005
    '*****************************************************************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of Data Set
        Dim dsGrpCommon As New System.Data.DataSet

        'Create Instance of Customer Class Object
        Dim clsCustomer As New MaxPayroll.clsCustomer

        'Create Instance of Common Class Object
        Dim clsCommon As New MaxPayroll.clsCommon

         'Variable Declarations
         Dim IsPopulated As Boolean, IsGrpLimit As Boolean
         Dim strAuthLock As String
         Try



            'Check if only SA/CA - Start
            If Not (ss_strUserType = gc_UT_SysAuth Or ss_strUserType = gc_UT_SysAdmin) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If
            'Check if only SA/CA - Stop


            IsGrpLimit = clsCommon.fncBuildContent("Group Limit", "", ss_lngOrgID, ss_lngUserID)      'Get Group Limit
            strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngUserID, ss_lngUserID)    'Get Authorization

            If IsGrpLimit Then
               Server.Transfer("PG_ListGroup.aspx?Err=Limit")
               Exit Try
            End If

            'Get Authorization Lock Status - Start
            If strAuthLock = "Y" Then
               Server.Transfer("PG_ListGroup.aspx?Err=Auth")
               Exit Try
            End If
            'Get Authorization Lock Status - Stop

            If Not Page.IsPostBack Then
                    'BindBody(body)

               If rq_strID = "" Then
                  btnView.Visible = False
               End If

               tblMainForm.Visible = True                                                                  'Main Form Table
               tblConfirm.Visible = False                                                                  'Confirm Form Table
               lblHeading.Text = "Group Creation"                                                          'Heading Text
               hEpf.Value = clsCommon.fncBuildContent("EPF", "EPF File", ss_lngOrgId, ss_lngUserId)              'EPF Service Status
               hSoc.Value = clsCommon.fncBuildContent("SOCSO", "SOCSO File", ss_lngOrgId, ss_lngUserId)          'EPF Service Status
               hPayroll.Value = clsCommon.fncBuildContent("PAYROLL", "Payroll File", ss_lngOrgId, ss_lngUserId)  'Payroll Service Status
               hLHDN.Value = clsCommon.fncBuildContent("LHDN", "LHDN File", ss_lngOrgId, ss_lngUserId)           'LHDN Service Status

               'Populate Bank Accounts - START
               dsGrpCommon = clsCustomer.fncGrpCommon("BANK DEFAULT", ss_lngOrgId, ss_lngUserId, "")
               IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkBankAccts, "ACCNAME", "ACCID")
               If Not IsPopulated Then
                  Server.Transfer("PG_ListGroup.aspx?Err=Bank")
                  Exit Try
               End If
               'Populate Bank Accounts - STOP

               'Populate EPF Accounts - START
               If hEpf.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("EPF DEFAULT", ss_lngOrgId, ss_lngUserId, "")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkEpfAccts, "SRNAME", "SRID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=EpfAcc")
                     
                     Exit Try
                  End If
               Else
                  trEPF.Visible = False
                  trEpfAccounts.Visible = False
               End If
               'Populate EPF Accounts - STOP

               'Populate Socso Accounts - START
               If hSoc.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("SOCSO DEFAULT", ss_lngOrgId, ss_lngUserId, "")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkSocAccts, "SRNAME", "SRID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=SocAcc")
                     Exit Try
                  End If
               Else
                  trSoc.Visible = False
                  trSocAccounts.Visible = False
               End If
               'Populate Socso Accounts - STOP

               'Populate LHDN Accounts - START
               If hLHDN.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("LHDN DEFAULT", ss_lngOrgId, ss_lngUserId, "")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkLHDNAccts, "SRNAME", "SRID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=LHDNAcc")
                     Exit Try
                  End If
               Else
                  trLHDN.Visible = False
                  trLHDNAccounts.Visible = False
               End If
               'Populate EPF Accounts - STOP


               'Populate Payroll Files - START
               If hPayroll.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgId, ss_lngUserId, "Payroll File")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkPayroll, "FFORMAT", "FID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=Pay")
                     Exit Try
                  End If
               Else
                  trPayroll.Visible = False
               End If
               'Populate Payroll Files - STOP

               'Populate EPF Files - START
               If hEpf.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgId, ss_lngUserId, "EPF File")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkEpf, "FFORMAT", "FID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=Epf")
                     Exit Try
                  End If
               Else
                  trEPF.Visible = False
               End If
               'Populate EPF Files - STOP

               'Populate Socso Files - START
               If hSoc.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgId, ss_lngUserId, "SOCSO File")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkSocso, "FFORMAT", "FID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=Soc")
                     Exit Try
                  End If
               Else
                  trSoc.Visible = False
               End If
               'Populate Socso Files - STOP

               'Populate LHDN Files - START
               If hLHDN.Value = "Y" Then
                  dsGrpCommon.Reset()
                  dsGrpCommon = clsCustomer.fncGrpCommon("FORMAT DEFAULT", ss_lngOrgId, ss_lngUserID, "LHDN File")
                  IsPopulated = fncPopulateCheckBox(dsGrpCommon, chkLHDN, "FFORMAT", "FID")
                  If Not IsPopulated Then
                     Server.Transfer("PG_ListGroup.aspx?Err=Soc")
                     Exit Try
                  End If
               Else
                  trLHDN.Visible = False
               End If
               'Populate Socso Files - STOP


            End If

         Catch

            'Log Error
            If Err.Description <> "Thread was being aborted." Then
               LogError("PG_CreateGroup - Page Load")
            End If

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of DataSet
            dsGrpCommon = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

            'Destroy Instance of Common Class Object
            clsCommon = Nothing

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
        'Created        : 11/02/2005
        '*****************************************************************************************************
        Public Sub Page_Confirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim IsDuplicate As Boolean
         Dim IsChecked As Boolean
         Dim sMsg As String = ""

         Try

            IsChecked = False
            tblConfirm.Visible = True                                                           'Confirm Form Table
            tblMainForm.Visible = False                                                         'Main Form Table
            txtCGroupName.Text = txtGroupName.Text                                              'Group Name
            txtCGroupDesc.Text = txtGroupDesc.Text                                              'Group Description
            txtCAuth.Text = ddlAuthorizer.SelectedValue                                         'No of Authorizer's
            lblHeading.Text = "Group Creation Confirmation"                                     'Heading Text
           
            lblMessage.Text = "Please Enter your Validation Code & Confirm Group Details"    'Message Label

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

                'Check For Duplicate Group Name - START
                IsDuplicate = clsCustomer.fnOrgValidations("ADD", "GROUP NAME", txtGroupName.Text, ss_lngOrgId, 0)
            If IsDuplicate Then
               tblMainForm.Visible = True
               tblConfirm.Visible = False
               sMsg += "Group Name already exists." & gc_BR
               'Exit Try
            End If
            'Check For Duplicate Group Name - STOP

            'Populate Selected Bank Accounts - START
            IsChecked = fncIsChecked(chkBankAccts, lbxCBankAccts)
            If Not IsChecked Then
               tblMainForm.Visible = True
               tblConfirm.Visible = False
               sMsg += "Please Select Bank Account(s)." & gc_BR
               'Exit Try
            End If
            'Populate Selected Bank Accounts - STOP

            'Populate Selected EPF Accounts - START
            If hEpf.Value = "Y" Then
               IsChecked = fncIsChecked(chkEpfAccts, lbxCEpfAccts)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select EPF Account(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCEpfAccounts.Visible = False
            End If
            'Populate Selected EPF Accounts - STOP

            'Populate Selected Socso Accounts - START
            If hSoc.Value = "Y" Then
               IsChecked = fncIsChecked(chkSocAccts, lbxCSocAccts)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select SOCSO Account(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCSocAccounts.Visible = False
            End If
            'Populate Selected Socso Accounts - STOP

            'Populate Selected LHDN Accounts - START
            If hLHDN.Value = "Y" Then
               IsChecked = fncIsChecked(chkLHDNAccts, lbxCLHDNAccts)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select LHDN Account(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCLHDNAccounts.Visible = False
            End If
            'Populate Selected Socso Accounts - STOP

            'Populate Selected Payroll Files - START
            If hPayroll.Value = "Y" Then
               IsChecked = fncIsChecked(chkPayroll, lbxCPyrFormat)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select Payroll File Format(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCPayroll.Visible = False
            End If
            'Populate Selected Payroll Files - STOP

            'Populate Selected EPF Files - START
            If hEpf.Value = "Y" Then
               IsChecked = fncIsChecked(chkEpf, lbxCEpfFormat)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select EPF File Format(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCEpf.Visible = False
            End If
            'Populate Selected EPF Files - STOP

            'Populate Selected EPF Files - START
            If hSoc.Value = "Y" Then
               IsChecked = fncIsChecked(chkSocso, lbxCSocFormat)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select SOCSO File Format(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCSoc.Visible = False
            End If
            'Populate Selected EPF Files - STOP

            'Populate Selected LHDN Files - START
            If hLHDN.Value = "Y" Then
               IsChecked = fncIsChecked(chkLHDN, lbxCLHDNFormat)
               If Not IsChecked Then
                  tblMainForm.Visible = True
                  tblConfirm.Visible = False
                  sMsg += "Please Select LHDN File Format(s)." & gc_BR
                  'Exit Try
               End If
            Else
               trCLHDN.Visible = False
            End If
            'Populate Selected EPF Files - STOP

            If Len(sMsg) > 0 Then
               lblMessage.Text = sMsg
               Exit Try
            End If

            If rdStatus.SelectedValue = "A" Then
               txtStatus.Text = "Active"
               hStatus.Value = "A"
            Else
               txtStatus.Text = "Inactive"
               hStatus.Value = "C"
            End If

         Catch

            'Log Error
            LogError("PG_CreateGroup - Page_Confirm")

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

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
        'Created        : 12/02/2005
        '*****************************************************************************************************
        Private Sub Page_Submit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Approval Matrix Class Object
            Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

            'Variable Declarations
         Dim lngGroupId As Long, intCounter As Int16, strVerifier As String
         Dim lngTransId As Long, intTotalItems As Int16, strVerify As String, lngToId As Long, strAuthCode As String
         Dim IsAuthCode As Boolean, intAttempts As Int16, intApproved As Int16, strSubject As String, strBody As String

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
                'Check If AuthCode is Valid - Start
                strAuthCode = clsCommon.fncPassAuth(ss_lngUserID, "A", ss_lngOrgID)
            IsAuthCode = IIf(strAuthCode = txtAuthCode.Text, True, False)
            'Check If AuthCode is Valid - Stop

            'Check for invalid Authorization Code Attempts - START
            If Not IsAuthCode Then
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
                     Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "A")
                     'Track Auth Lock
                     Call clsUsers.prcLockHistory(ss_lngUserID, "A")
                     Server.Transfer("PG_ListGroup.aspx?Err=Auth")
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

            'Get Group Id
            lngGroupId = clsCustomer.fncGrpInsUpd("INSERT", 0, ss_lngOrgID, ss_lngUserID, intApproved)

            If lngGroupId > 0 Then

               'Delete Bank Accounts - START
               Call clsCustomer.prcGrpDelTrans("BANK ACCTS", lngGroupId, ss_lngOrgID, ss_lngUserID)
               'Delete Bank Accounts - STOP

               'Insert Bank Accounts - START
               intTotalItems = lbxCBankAccts.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCBankAccts.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("BANK ACCTS", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'Insert Bank Accounts - STOP

               'Insert EPF Accounts - START
               intTotalItems = lbxCEpfAccts.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCEpfAccts.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("EPF ACCTS", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'Insert EPF Accounts - STOP

               'Insert Socso Accounts - START
               intTotalItems = lbxCSocAccts.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCSocAccts.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("SOCSO ACCTS", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'Insert Socso Accounts - STOP

               'Insert LHDN Accounts - START
               intTotalItems = lbxCLHDNAccts.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCLHDNAccts.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("LHDN ACCTS", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'Insert LHDN Accounts - STOP

               'Delete File Format - START
               Call clsCustomer.prcGrpDelTrans("FILE FORMAT", lngGroupId, ss_lngOrgID, ss_lngUserID)
               'Delete File Format - STOP

               'Payroll File Format - START
               intTotalItems = lbxCPyrFormat.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCPyrFormat.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'Payroll File Format - STOP

               'EPF File Format - START
               intTotalItems = lbxCEpfFormat.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCEpfFormat.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'EPF File Format - STOP

               'SOCSO File Format - START
               intTotalItems = lbxCSocFormat.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCSocFormat.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'SOCSO File Format - STOP

               'LHDN File Format - START
               intTotalItems = lbxCLHDNFormat.Items.Count
               For intCounter = 0 To intTotalItems - 1
                  lngTransId = lbxCLHDNFormat.Items(intCounter).Value
                  Call clsCustomer.prcGrpTrans("FILE FORMAT", lngGroupId, ss_lngOrgID, ss_lngUserID, lngTransId)
               Next
               'LHDN File Format - STOP

               trNew.Visible = True
               trConfirm.Visible = False
               trAuthCode.Visible = False

               'If Single Verification
               If UCase(strVerify) = "SINGLE" Then

                  lblMessage.Text = "Group Created Successfully"
                  'If Dual Verification
               ElseIf UCase(strVerify) = "DUAL" Then

                  'Get System Authorizer
                  strVerifier = clsCommon.fncBuildContent("System Auth", "", ss_lngOrgID, ss_lngUserID)
                  If IsNumeric(Trim(strVerifier)) Then
                     lngToId = Trim(strVerifier)
                  End If

                  'Mail Subject
                  strSubject = txtCGroupName.Text & " Group Created"

                  'Send Requisition for Authorization 
                  Call clsApprMatrix.prcApprovalMatrix(ss_lngOrgID, ss_lngUserID, "INSERT", 0, ss_lngUserID, lngToId, lngGroupId, strSubject, "Group Creation", "", 1)

                  'Mail Body
                  strBody = strSubject & " , pending for approval."

                  'Send Mail
                  Call clsCommon.prcSendMails("SEND MAIL", ss_lngOrgID, ss_lngUserID, 0, strSubject, strBody, lngToId)

                  'Display Message
                  lblMessage.Text = "Group Created Successfully. Request sent for Approval."

               End If

            Else

               trConfirm.Visible = True
               lblMessage.Text = "Group Creation Failed"

            End If

         Catch

            'Error Message
            lblMessage.Text = "Group Creation Failed."

            'Log Errorr
            If Err.Description <> "Thread was being aborted." Then
               LogError("PG_CreateGroup - btnConfirm Page_Submit")
            End If

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

            'Destroy Instance of Common Class Object
            clsCommon = Nothing

            'Destroy Instance of Users Class Object
            clsUsers = Nothing

            'Destroy Instance of Approval Matrix Class Object
            clsApprMatrix = Nothing

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

#Region "Button Controls"

    Private Sub prcView(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnView.Click

        Try

            Server.Transfer("PG_ListGroup.aspx", False)

        Catch ex As Exception

        End Try

    End Sub

    Private Sub prcNew(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnNew.Click

        Try

            Server.Transfer("PG_CreateGroup.aspx", False)

        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class   

End Namespace
