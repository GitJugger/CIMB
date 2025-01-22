Namespace MaxPayroll

    Partial Class PG_ServiceAccount
        Inherits clsBasePage

#Region "Global Variables"
        Enum enmDGItem
            SRVID
            No
            AccName
            AccNo
            EmpName
            State
            SRTest
            Delete
        End Enum
#End Region

#Region "Page Load"
        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 125/10/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim lngOrgId As Long
            Try


                'Check if only Bank User
                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                hOption.Value = Request.QueryString("Ser")                                              'Get Service Type
                hStatus.Value = Left(Request.QueryString("Status"), 1)                                  'Get Organisation Status
                hRequest.Value = Request.QueryString("Req")

                lngOrgId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)      'Get Organisation Id

                If hStatus.Value = "C" Then
                    lblMessage.Text = "Organization has been cancelled."
                End If

                'If hOption.Value = "E" Then
                '    lblHeading.Text = "Organization Payment Service Accounts - EPF"
                'ElseIf hOption.Value = "S" Then
                '    lblHeading.Text = "Organization Payment Service Accounts - SOCSO"
                'ElseIf hOption.Value = "L" Then
                '    lblHeading.Text = "Organization Payment Service Accounts - LHDN"
                'ElseIf hOption.Value = "Z" Then
                '    lblHeading.Text = "Organization Payment Service Accounts - ZAKAT"
                'End If

                If Not Page.IsPostBack Then
                    'BindBody(body)
                    'Populate Data Grind
                    Call prcBindGrid(hOption.Value, lngOrgId, ss_lngUserID)
                Else
                    'Insert New Accounts
                    If hAction.Value = "A" Then
                        'Insert New Service Account
                        Call fncServiceAccts()
                        'Clear Hidden Box
                        hAction.Value = ""
                    ElseIf hAction.Value = "U" Then
                        'Update Service Account
                        Call fncServiceAccts()
                        'Clear Hidden Box
                        hAction.Value = ""
                    End If
                End If

            Catch

                'Error Log
                LogError("PG_BankAccount - Page_Load")

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Bind Datagrid"

        '****************************************************************************************************
        'Procedure Name : prcBindGrind()
        'Purpose        : Load Account Grid
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/03/2005
        '*****************************************************************************************************
        Private Sub prcBindGrid(ByVal strOption As String, ByVal lngOrgId As Long, ByVal lngUserId As Long)

            'Create Instance of DataRow
            Dim drServiceAcc As DataRow

            'Create Instance of System Data Set
            Dim dsServiceAcc As New DataSet
            Dim dsBank As New DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim intRecordCount As Int16

            Try

                'Populate Data Set
                dsServiceAcc = clsCustomer.fncPayService(strOption, lngOrgId, lngUserId)

                'Get Total Records
                intRecordCount = dsServiceAcc.Tables("SERVICEACC").Rows.Count

                If Not hStatus.Value = "C" Then
                    'Add New Row
                    drServiceAcc = dsServiceAcc.Tables("SERVICEACC").NewRow
                    dsServiceAcc.Tables("SERVICEACC").Rows.Add(drServiceAcc)
                End If

                'Bind Data Grid
                If dsServiceAcc.Tables(0).Rows.Count > 0 Then
                    fncGeneralGridTheme(dgServiceAccount)
                    dgServiceAccount.DataSource = dsServiceAcc
                    dgServiceAccount.DataBind()
                    If hOption.Value = "S" Or hOption.Value = "L" Or hOption.Value = "Z" Then
                        dgServiceAccount.Columns(enmDGItem.SRTest).Visible = False
                    End If
                Else
                    pnlGrid.Visible = False
                End If

                If hOption.Value = "L" Then

                    'Get State LHDN
                    dsServiceAcc = clsCustomer.GetStateLHDN(lngOrgId, lngUserId)
                    'Insert New Row
                    Call prcAddRow(dgServiceAccount, intRecordCount, dsServiceAcc)

                ElseIf hOption.Value = "Z" Then
                    'Get State LHDN
                    dsServiceAcc = clsCustomer.GetStateZAKAT(lngOrgId, lngUserId)
                    'Insert New Row
                    Call prcAddRow(dgServiceAccount, intRecordCount, dsServiceAcc)

                Else
                    'Get State
                    dsServiceAcc = clsCustomer.GetState(lngOrgId, lngUserId)

                    'dsBank = clsba
                    'Insert New Row
                    Call prcAddRow(dgServiceAccount, intRecordCount, dsServiceAcc)
                End If


                'if inquiry user hide buttons
                If ss_strUserType = gc_UT_InquiryUser Then
                    Dim intColCount As Int16 = dgServiceAccount.Columns.Count - 1
                    dgServiceAccount.Columns(intColCount).Visible = False
                End If

            Catch ex As Exception

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "PG_ServiceAccount - prcBindGrid", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of System Data Row
                drServiceAcc = Nothing

                'Destroy Instance of Data Set
                dsServiceAcc = Nothing

            End Try

        End Sub

#End Region

#Region "Insert Add Button and Edit Button"

        '****************************************************************************************************
        'Procedure Name : prcAddRow()
        'Purpose        : To Add New Row
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran/ Eu Yean Lock - 
        'Created        : 05/03/2005
        '*****************************************************************************************************
        Private Sub prcAddRow(ByVal dgServiceAccount As DataGrid, ByVal intRecordCount As Int16, ByVal dsState As DataSet)

            'Create Instance of Button
            Dim btnAdd As Button

            'Create Instance of Button
            Dim btnEdit As Button

            'Create Instance of Textbox
            Dim txtService As TextBox

            'Create Instance of Textbox
            Dim txtServiceAcc As TextBox

            'Create Instance of Label
            Dim lblNo As Label

            'Create Instance of Datagrid Item
            Dim dgiServAccts As DataGridItem

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Dim clsBankMF As New clsBankMF


            'Variable Declarations
            Dim intCounter As Int16, strStatus As String, intState As Int16

            Try

                strStatus = Left(hStatus.Value, 1)

                'Loop Thor Data Grid
                For Each dgiServAccts In dgServiceAccount.Items

                    'Populate State
                    Dim ddlState As DropDownList = CType(dgiServAccts.FindControl("cmbState"), DropDownList)
                    ddlState.DataSource = dsState.Tables("ACCESS")
                    ddlState.DataTextField = "State"
                    ddlState.DataValueField = "StateCode"
                    ddlState.DataBind()



                    'Get Selected State - Start
                    If IsNumeric(CType(dgiServAccts.FindControl("hState"), HtmlInputHidden).Value) Then
                        intState = CType(dgiServAccts.FindControl("hState"), HtmlInputHidden).Value
                    Else
                        intState = 0
                    End If



                    If intState = 17 Then
                        ddlState.SelectedIndex = 1
                    ElseIf intState = 18 Then
                        ddlState.SelectedIndex = 2
                    ElseIf intState = 19 Then
                        ddlState.SelectedIndex = 3
                    Else
                        'Select State
                        ddlState.SelectedValue = intState
                        'Get Selected State - Stop
                    End If



                    If Not strStatus = "C" Then
                        'Remove Delete Button
                        If intCounter = intRecordCount Then
                            btnAdd = New Button
                            btnAdd.Text = "  Add  "
                            btnAdd.CssClass = "SHORTBUTTON"
                            btnAdd.Attributes("onclick") = "document.all('ctl00$cphContent$hAction').value = 'A';"
                            lblNo = CType(dgiServAccts.FindControl("lblNo"), Label)
                            lblNo.Text = intCounter + 1
                            dgiServAccts.Cells(enmDGItem.Delete).Controls.RemoveAt(0)
                            dgiServAccts.Cells(enmDGItem.Delete).Controls.Add(btnAdd)
                            'Make Account No and Account Name as Readonly
                        ElseIf intCounter = 0 Then
                            btnEdit = New Button
                            btnEdit.Text = " Update "
                            btnEdit.CssClass = "SHORTBUTTON"
                            btnEdit.CausesValidation = False
                            lblNo = CType(dgiServAccts.FindControl("lblNo"), Label)
                            btnEdit.Attributes("onclick") = "document.all('ctl00$cphContent$hAction').value = 'U';"
                            lblNo.Text = intCounter + 1
                            txtService = CType(dgiServAccts.FindControl("txtServName"), TextBox)
                            'txtService.ReadOnly = True
                            txtServiceAcc = CType(dgiServAccts.FindControl("txtServAcc"), TextBox)
                            'txtServiceAcc.ReadOnly = True
                            dgiServAccts.Cells(enmDGItem.Delete).Controls.RemoveAt(0)
                            dgiServAccts.Cells(enmDGItem.Delete).Controls.Add(btnEdit)
                            'Replace Add Button with Delete Button
                        Else
                            btnEdit = New Button
                            btnEdit.Text = " Update "
                            btnEdit.CssClass = "SHORTBUTTON"
                            btnEdit.CausesValidation = False
                            btnEdit.Attributes("onclick") = "document.all('ctl00$cphContent$hAction').value = 'U';"
                            lblNo = CType(dgiServAccts.FindControl("lblNo"), Label)
                            lblNo.Text = intCounter + 1
                            txtService = CType(dgiServAccts.FindControl("txtServName"), TextBox)
                            'txtService.ReadOnly = True
                            txtServiceAcc = CType(dgiServAccts.FindControl("txtServAcc"), TextBox)
                            'txtServiceAcc.ReadOnly = True
                            Dim lblBlank As Label = New Label
                            lblBlank.Text = "&nbsp;"
                            dgiServAccts.Cells(enmDGItem.Delete).Controls.Add(lblBlank)
                            dgiServAccts.Cells(enmDGItem.Delete).Controls.Add(btnEdit)

                        End If
                    Else
                        lblNo = CType(dgiServAccts.FindControl("lblNo"), Label)
                        lblNo.Text = intCounter + 1
                        txtService = CType(dgiServAccts.FindControl("txtServName"), TextBox)
                        txtService.ReadOnly = True
                        txtServiceAcc = CType(dgiServAccts.FindControl("txtServAcc"), TextBox)
                        txtServiceAcc.ReadOnly = True
                        dgiServAccts.Cells(enmDGItem.Delete).Controls.RemoveAt(0)
                        'Replace Add Button with Delete Button
                    End If
                    intCounter = intCounter + 1
                Next

            Catch ex As Exception
                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "prcAddRow - PG_ServiceAccount", Err.Number, Err.Description)
            Finally

                'Destroy Instance of Button
                btnAdd = Nothing

                'Destroy Instance of Button
                btnEdit = Nothing

                'Destroy Instance of Datagrid Item
                dgiServAccts = Nothing

                'Destroy Instance of Text box
                txtService = Nothing

                'Destroy Instance of Label
                lblNo = Nothing

                'Destroy Insance of textbox
                txtServiceAcc = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of dsstate
                dsState = Nothing


            End Try

        End Sub

#End Region

#Region "Alert Delete Accts"

        '****************************************************************************************************
        'Procedure Name : dgBankAccts_ItemDataBound()
        'Purpose        : To Show Alert for Delete Accounts
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/03/2005
        '*****************************************************************************************************
        Public Sub dgServiceAccts_ItemDataBound(ByVal O As System.Object, ByVal E As DataGridItemEventArgs)

            Try

                If E.Item.ItemType <> ListItemType.Header And E.Item.ItemType <> ListItemType.Footer Then

                    'Create Instance of Button
                    Dim btnDelete As Object = CType(E.Item.Cells(enmDGItem.SRTest).Controls(0), Object)
                    Dim strAccountName As String = CType(E.Item.FindControl("txtServName"), TextBox).Text

                    'Display Alert Message
                    CType(btnDelete, Button).CssClass = "SHORTBUTTON"
                    btnDelete.Attributes("onclick") = "javascript:return confirm('Are you sure you want to delete this Account " & _
                                                     strAccountName & "?')"
                End If

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Delete Accts"

        '****************************************************************************************************
        'Procedure Name : dgServiceAccts_Delete()
        'Purpose        : To Delete Accounts
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/03/2005
        '*****************************************************************************************************
        Public Sub dgServiceAccts_Delete(ByVal O As System.Object, ByVal E As DataGridCommandEventArgs)

            'Create Instance of Customer Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim lngAccId As Long, lngOrgId As Long, lngUserId As Long, IsDelete As Boolean, strService As String = ""

            Try
                'strService = IIf(hOption.Value = "E", "EPF", "SOCSO")
                Select Case hOption.Value
                    Case "E"
                        strService = "EPF"
                    Case "S"
                        strService = "SOCSO"
                    Case "L"
                        strService = "LHDN"
                    Case "Z"
                        strService = "ZAKAT"
                End Select

                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)                                                 'Get User Id
                lngOrgId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)                                          'Get Organisation Id
                lngAccId = IIf(IsNumeric(dgServiceAccount.DataKeys(E.Item.ItemIndex)), dgServiceAccount.DataKeys(E.Item.ItemIndex), 0)      'Get Account Id

                'Delete Account - START
                IsDelete = clsCustomer.fncDelBankAccts(lngOrgId, lngUserId, lngAccId, hOption.Value, False)
                If IsDelete Then
                    lblMessage.Text = strService & " Account Deleted Successfully"
                    'Bind Datagrid
                    Call prcBindGrid(hOption.Value, lngOrgId, lngUserId)
                Else
                    lblMessage.Text = strService & " Account Deletion Failed"
                End If
                'Delete Account - STOP

                'Bind Datagrid
                Call prcBindGrid(hOption.Value, lngOrgId, lngUserId)

            Catch ex As Exception

            Finally

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Insert/Update Service Accounts"


        '****************************************************************************************************
        'Procedure Name : fncServiceAccts()
        'Purpose        : To Add New Service Accounts
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 13/10/2005
        '*****************************************************************************************************
        Private Sub fncServiceAccts()

            'Create Instance of Datagrid Item
            Dim dgiServiceAccts As DataGridItem

            'Create Instance of System Data Set
            Dim dsState As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Create Instance of Check Digit Class Object
            Dim clsCheckDigit As New MaxPayroll.clsCheckDigit

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strAccNo As String, intServiceId As Int32, IsDuplicate As Boolean, intState As Int16
            Dim strErrMessage As String, intCounter As Int16, IsError As Boolean, strService As String = ""
            Dim lngOrgId As Long, lngUserId As Long, IsUpdIns As Boolean, strAccName As String, IsValidNo As Boolean
            Dim strTaxNoCheck As String

            Try

                intCounter = 1                                                                       'Initialise Counter
                strErrMessage = "Errors:<br>"                                                        'Error Message
                'strService = IIf(hOption.Value = "E", "EPF", "SOCSO")                                'Get Service Type 
                Select Case hOption.Value
                    Case "E"
                        strService = "EPF"
                    Case "S"
                        strService = "SOCSO"
                    Case "L"
                        strService = "LHDN"
                    Case "Z"
                        strService = "ZAKAT"
                End Select

                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)          'Get User Id
                lngOrgId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)   'Get Organisation Id

                'Check For Errors - START
                For Each dgiServiceAccts In dgServiceAccount.Items

                    If IsNumeric(dgServiceAccount.DataKeys(dgiServiceAccts.ItemIndex)) Then
                        intServiceId = Convert.ToInt32(dgServiceAccount.DataKeys(dgiServiceAccts.ItemIndex))
                    Else
                        intServiceId = 0
                    End If

                    strAccNo = CType(dgiServiceAccts.FindControl("txtServAcc"), TextBox).Text               'Get Account No
                    strAccName = CType(dgiServiceAccts.FindControl("txtServName"), TextBox).Text            'Get Account Name
                    intState = CType(dgiServiceAccts.FindControl("cmbState"), DropDownList).SelectedValue   'Get State Code

                    If Not strAccNo = "" And Not strAccName = "" Then

                        'Check Digit for Account No
                        If hOption.Value = "E" Then
                            'Check Digit for EPF Number - True
                            'IsValidNo = clsCheckDigit.fncCheckDigitEPF(strAccNo)
                            If Not IsValidNo Then
                                IsError = True
                                strErrMessage = strErrMessage & "No. " & intCounter & ": Invalid " & strService & " Account Number.<br>"
                            End If
                        ElseIf hOption.Value = "S" Then
                            'Check Digit for SOCSO Number - True
                            ' IsValidNo = clsCheckDigit.fncCheckDigitSOCSO(strAccNo)
                            If Not IsValidNo Then
                                IsError = True
                                strErrMessage = strErrMessage & "No. " & intCounter & ": Invalid " & strService & " Account Number.<br>"
                            End If
                        ElseIf hOption.Value = "L" Then
                            'Validate LHDN Employer Tax No - True
                            'strTaxNoCheck = clsCommon.fncBuildContent("LHDN Employer Tax No", "", lngOrgId, lngUserId, "", strAccNo)
                            If Not Len(strAccNo) = 10 Then
                                IsValidNo = False
                            Else
                                'IsValidNo = clsCheckDigit.fncCheckDigitLHDN(strAccNo)
                                IsValidNo = True
                            End If

                            If Not IsValidNo Then
                                IsError = True
                                strErrMessage = strErrMessage & "No. " & intCounter & ": Invalid " & strService & " Account Number.<br>"
                            End If

                        End If

                        'Check For Duplicate Account No
                        If hOption.Value = "E" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "EPF NO", strAccNo, lngOrgId, intServiceId)
                        ElseIf hOption.Value = "S" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "SOCSO NO", strAccNo, lngOrgId, intServiceId)
                        ElseIf hOption.Value = "L" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "LHDN NO", strAccNo, lngOrgId, intServiceId)
                        ElseIf hOption.Value = "Z" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "ZAKAT NO", strAccNo, lngOrgId, intServiceId)
                        End If

                        If IsDuplicate Then
                            IsError = True
                            strErrMessage = strErrMessage & "No. " & intCounter & ": " & strService & " Account No Duplicated.<br>"
                        End If

                        'Check For Duplicate Account Name
                        If hOption.Value = "E" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "EPF NAME", strAccName, lngOrgId, intServiceId)
                        ElseIf hOption.Value = "S" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "SOCSO NAME", strAccName, lngOrgId, intServiceId)
                        ElseIf hOption.Value = "L" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "LHDN NAME", strAccName, lngOrgId, intServiceId)
                        ElseIf hOption.Value = "Z" Then
                            IsDuplicate = clsCustomer.fnOrgValidations("ADD", "ZAKAT NAME", strAccName, lngOrgId, intServiceId)
                        End If

                        If IsDuplicate Then
                            IsError = True
                            strErrMessage = strErrMessage & "No. " & intCounter & ": " & strService & " Account Name Duplicated.<br>"
                        End If
                        'Account No Validation - STOP

                    End If

                    'Increment Counter
                    intCounter = intCounter + 1

                Next
                'Check For Errors - STOP

                'Return Error Message if any Errors
                If IsError And Not hOption.Value = "L" Then
                    lblMessage.Text = strErrMessage
                    dsState = clsCustomer.GetState(lngOrgId, lngUserId)
                    Call prcAddRow(dgServiceAccount, (intCounter - 2), dsState)
                    Exit Try
                End If

                ' Retrun Error Message if any errors
                If IsError And hOption.Value = "L" Then
                    lblMessage.Text = strErrMessage
                    dsState = clsCustomer.GetStateLHDN(lngOrgId, lngUserId)
                    Call prcAddRow(dgServiceAccount, (intCounter - 2), dsState)
                    Exit Try
                End If

                'Insert/Update Bank Accts
                If hAction.Value = "A" Then
                    IsUpdIns = clsCustomer.fncUpdateServiceAccs("ADD", lngOrgId, lngUserId, dgServiceAccount, hOption.Value)
                Else
                    IsUpdIns = clsCustomer.fncUpdateServiceAccs("UPDATE", lngOrgId, lngUserId, dgServiceAccount, hOption.Value)
                End If

                If hAction.Value = "A" Then
                    If IsUpdIns Then
                        'Display Success Message
                        lblMessage.Text = strService & " Account Details Created Successfully."
                    ElseIf IsUpdIns Then
                        'Display Failed Message
                        lblMessage.Text = strService & "Account Details Creation Failed."
                    End If
                Else
                    If IsUpdIns Then
                        'Display Success Message
                        lblMessage.Text = strService & " Account Details Updated Successfully."
                    ElseIf IsUpdIns Then
                        'Display Failed Message
                        lblMessage.Text = strService & "Account Details Updated Failed."
                    End If
                End If

                'Bind Grid
                Call prcBindGrid(hOption.Value, lngOrgId, lngUserId)

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncServiceAccts - PG_ServiceAccount", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Intstance Datagrid Item
                dgiServiceAccts = Nothing

                'Destroy Instance of System Data Set
                dsState = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Check Digit Class Object
                'clsCheckDigit = Nothing

            End Try

        End Sub

#End Region

#Region "Back To View"
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click

            Select Case hOption.Value
                Case "E"
                    Server.Transfer("PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Epf.ToString)
                Case "S"
                    Server.Transfer("PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.Socso.ToString)
                Case "L"
                    Server.Transfer("PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.LHDN.ToString)
                Case "Z"
                    Server.Transfer("PG_ViewOrganisation.aspx?Req=" & MaxPayroll.mdConstant.enmViewOrganizationReqType.ZAKAT.ToString)
            End Select


        End Sub
#End Region
    End Class

End Namespace
