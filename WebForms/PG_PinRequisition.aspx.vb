Namespace MaxPayroll

    Partial Class PG_PinRequisition
        Inherits clsBasePage


#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Load Page Functions
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strAuthLock As String

            Try

                If Page.IsPostBack = False Then
                    'BindBody(body)
                    Dim clsUsers As New clsUsers
                    Call clsUsers.prcDetailLog(ss_lngUserID, "Pin Requisition", "Y")
                End If

                lblMessage.Text = ""
                'If User Type Not Bank User - Start
                If Not (Me.ss_strUserType = gc_UT_BankUser) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If User Type Not Bank User - Stop

                'Get Auth Lock Status
                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    btnSearch.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                End If

            Catch ex As Exception
                LogError("PG_PinRequisition - Page Load")
            Finally

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Search By"

        '****************************************************************************************************
        'Procedure Name : prcSearchBy()
        'Purpose        : Search By Display
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcSearchBy(ByVal O As System.Object, ByVal E As System.EventArgs) Handles rdSearchBy.SelectedIndexChanged

            Try

                If rdSearchBy.SelectedValue = "R" Then
                    tblGrid.Visible = False
                    tblButton.Visible = True
                    tblRange.Visible = True
                    tblSearch.Visible = False
                ElseIf rdSearchBy.SelectedValue = "S" Then
                    tblGrid.Visible = False
                    tblRange.Visible = False
                    tblSearch.Visible = True
                    tblButton.Visible = True
                End If

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Get Requested"

        '****************************************************************************************************
        'Procedure Name : prcGetRequisition()
        'Purpose        : Get Search Result
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcGetRequisition(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSearch.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Data Set
            Dim dsPinRequisition As System.Data.DataSet

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Variable Declarations
            Dim strSearchBy As String, strOrgType As String, lngOrgFrom As Long
            Dim lngOrgTo As Long, strOption As String, strCriteria As String, strKeyword As String


            Try

                strKeyword = txtKeyword.Text                                                                            'Get Search Keyword
                strOption = cmbOption.SelectedValue                                                                     'Get Searh Option
                strSearchBy = rdSearchBy.SelectedValue                                                                  'Get Search By Option
                strCriteria = cmbCriteria.SelectedValue                                                                 'Get Search Criteria
                strOrgType = rdOrganization.SelectedValue                                                               'Get Organization Type
                'Get User Id
                lngOrgTo = IIf(IsNumeric(txtOrgTo.Text), txtOrgTo.Text, 0)          'Get To Organization Id 
                lngOrgFrom = IIf(IsNumeric(txtOrgFrom.Text), txtOrgFrom.Text, 0)  'Get From Organization Id

                'Populate Data Set
                dsPinRequisition = clsPinMailer.fncSearchPinRequisition(ss_lngUserID, strSearchBy, strOrgType, lngOrgFrom, lngOrgTo, strOption, strCriteria, strKeyword)

                'Bind Data Grid - Start
                If dsPinRequisition.Tables("ORGPIN").Rows.Count > 0 Then
                    fncGeneralGridTheme(dgRequisition)
                    Me.pnlGrid.Visible = True
                    tblGrid.Visible = True
                    trSubmit.Visible = False
                    trAuthCode.Visible = False
                    dgRequisition.DataSource = dsPinRequisition
                    dgRequisition.DataBind()
                Else
                    Me.pnlGrid.Visible = False
                    tblGrid.Visible = False
                    lblMessage.Text = "No Records Found"
                End If
                'Bind Data Grid - Stop

                'For Exisiting Organisation
                If strOrgType = "E" And dsPinRequisition.Tables("ORGPIN").Rows.Count > 0 Then
                    dgRequisition.Dispose()
                    dgRequisition.DataSource = prcUserCode(ss_lngUserID, dsPinRequisition)
                    dgRequisition.DataBind()
                End If

            Catch

                'Log Errors
                Call clsGeneric.ErrorLog(100000, ss_lngUserID, "prcGetRequisition - PG_PinRequisition", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

                'Destroy Instance of Data Set
                dsPinRequisition = Nothing

            End Try

        End Sub

#End Region

#Region "Clear"

        Private Sub prcClear(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnClear.Click

            Try

                txtOrgTo.Text = ""
                txtKeyword.Text = ""
                txtOrgFrom.Text = ""
                cmbOption.SelectedValue = ""
                cmbCriteria.SelectedValue = ""

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Select/Unselect All"

        '****************************************************************************************************
        'Procedure Name : prcSelect()
        'Purpose        : To Select The List of Displayed Pin Request
        'Arguments      : Object,Event Arguments
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/07/2005
        '*****************************************************************************************************
        Private Sub prcSelect(ByVal Source As Object, ByVal E As EventArgs) Handles btnSelect.Click

            Dim GridItem As DataGridItem

            Try

                For Each GridItem In dgRequisition.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.Cells(5).Controls(0), CheckBox)
                    myCheckbox.Checked = True
                Next

            Catch ex As Exception

            End Try

        End Sub
        '****************************************************************************************************
        'Procedure Name : prcUnselect()
        'Purpose        : To Unselect The List of Displayed Pin Request
        'Arguments      : Object,Event Arguments
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/07/2005
        '*****************************************************************************************************
        Sub prUncheck(ByVal Source As Object, ByVal E As EventArgs) Handles btnUnSelect.Click

            Dim GridItem As DataGridItem

            Try

                For Each GridItem In dgRequisition.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.Cells(5).Controls(0), CheckBox)
                    myCheckbox.Checked = False
                Next

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Confirm Request"

        '****************************************************************************************************
        'Procedure Name : prcConfirmRequest()
        'Purpose        : Send Pin Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcConfirmRequest(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Create Instance of DataGrid Item
            Dim dgiPinRequisition As DataGridItem

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Dim clsPaymentService As New clsPaymentService

            'Variable Declarations
            Dim IsSelected As Boolean

            Dim sErrorMsg As String = ""
            Try

                IsSelected = False
                lblMessage.Text = "Please Enter your Validation Code and Confirm your Requisition"


                'Disable Datagrid - Start
                For Each dgiPinRequisition In dgRequisition.Items
                    Dim txtRemarks As TextBox = CType(dgiPinRequisition.FindControl("txtRemarks"), TextBox)
                    Dim chkSelect As CheckBox = CType(dgiPinRequisition.FindControl("chkSelect"), CheckBox)
                    If chkSelect.Checked Then
                        IsSelected = True
                        'sErrorMsg += clsPaymentService.fncCheckServiceAndAccount(CInt(dgiPinRequisition.Cells(0).Text.Replace(gc_Const_CCPrefix, "").Trim))
                    End If

                    chkSelect.Enabled = False
                    txtRemarks.ReadOnly = True
                Next
                'Disable Datagrid - Stop

                If Len(sErrorMsg) > 0 Then
                    'Enable Datagrid - Start
                    For Each dgiPinRequisition In dgRequisition.Items
                        Dim txtRemarks As TextBox = CType(dgiPinRequisition.FindControl("txtRemarks"), TextBox)
                        Dim chkSelect As CheckBox = CType(dgiPinRequisition.FindControl("chkSelect"), CheckBox)

                        chkSelect.Enabled = True
                        txtRemarks.ReadOnly = False
                    Next
                    'Enable Datagrid - Stop
                    lblMessage.Text = sErrorMsg
                    Exit Try
                End If


                'If Nothing Selected - Start
                If Not IsSelected Then
                    'Enable Datagrid - Start
                    For Each dgiPinRequisition In dgRequisition.Items
                        Dim txtRemarks As TextBox = CType(dgiPinRequisition.FindControl("txtRemarks"), TextBox)
                        Dim chkSelect As CheckBox = CType(dgiPinRequisition.FindControl("chkSelect"), CheckBox)
                        chkSelect.Enabled = True
                        txtRemarks.ReadOnly = False
                    Next
                    'Enaable Datagrid - Stop
                    lblMessage.Text = "Please Select atleast one Requisition"
                    Exit Try
                End If
                'If Nothing Selected - Stop

                trSelect.Visible = False    'Hide Select Buttons
                trSubmit.Visible = True     'Show Submit Button
                trAuthCode.Visible = True   'Show Auth Code

            Catch

                'Error Message
                lblMessage.Text = "Pin Mailer Requisition Failed"

                'Log Error
                Call clsGeneric.ErrorLog(100000, ss_lngUserID, "prcConfirmRequest - PG_PinRequisition", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Datagrid Item
                dgiPinRequisition = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Submit Request"

        '****************************************************************************************************
        'Procedure Name : prcConfirmRequest()
        'Purpose        : Confirm Pin Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcSubmitRequest(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Variable Declarations
            Dim IsInsert As Boolean, strSubject As String, strBody As String
            Dim lngUserId As Long, strAuthMsg As String, strVerifier As String, lngToId As Long
            Dim lngOrgId As Long, strReqRemark As String, strUserType As String, strCodeType As String

            Try

                'Get User Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Check Auth Code - Start
                strAuthMsg = clsGeneric.fncAuthCheck(100000, lngUserId, txtAuthCode.Text)
                If Not strAuthMsg = "" Then
                    lblMessage.Text = strAuthMsg
                    Exit Try
                End If
                'Check Auth Code - Stop

                'Loop Thro Datagrid - Start
                For Each dgiRequisition In dgRequisition.Items
                    Dim chkSelect As CheckBox = CType(dgiRequisition.FindControl("chkSelect"), CheckBox)
                    Dim txtRemarks As TextBox = CType(dgiRequisition.FindControl("txtRemarks"), TextBox)
                    If chkSelect.Checked Then
                        strReqRemark = txtRemarks.Text                              'Get request Remark
                        strUserType = dgiRequisition.Cells(3).Text                  'Get User Type
                        strCodeType = Mid(dgiRequisition.Cells(4).Text, 1, 1)       'Get Code Type
                        lngOrgId = dgiRequisition.Cells(0).Text                     'Get Organization Code
                        'If Sys Admin & Sys Auth
                        If strUserType = "Both" Then
                            strUserType = "B"
                            'If Only Sys Admin
                        ElseIf strUserType = "Sys Admin" Then
                            strUserType = "A"
                            'If Only Sys Auth
                        ElseIf strUserType = "Sys Auth" Then
                            strUserType = "S"
                        End If
                        'Insert Pin Request
                        IsInsert = clsPinMailer.fncPinRequisition("R", 0, lngOrgId, lngUserId, strReqRemark, "P", strUserType, strCodeType, "", "P")
                        'If Insert Failed
                        If Not IsInsert Then
                            'Display Failed Message
                            lblMessage.Text = "Pin Mailer Requisition Failed."
                            Exit Try
                        End If
                    End If
                Next
                'Loop Thro Datagrid - Stop

                'Get System Authorizer/Bank Super Admin
                strVerifier = clsCommon.fncBuildContent("Bank Super", "", 100000, lngUserId)
                If IsNumeric(Trim(strVerifier)) Then
                    lngToId = Trim(strVerifier)
                End If

                'Mail Subject
                strSubject = "Pin Mailer Requisition"
                'Mail Body
                strBody = "Pin Mailer Requisition sent for Approval."
                'Send Mail
                Call clsCommon.prcSendMails("BANK AUTH", 100000, lngUserId, 0, strSubject, strBody, 0)

                'Hide Grid Table
                tblGrid.Visible = False

                'Show Message
                lblMessage.Text = "Pin Mailer Requisition Successful. Request sent for Approval"

            Catch

                'Error Message
                lblMessage.Text = "Pin Mailer Requisition Failed."

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcSubmitRequest - PG_PinRequisition", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Datagrid Item
                dgiRequisition = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

            End Try

        End Sub

#End Region

#Region "Get User/Code Details"

        '****************************************************************************************************
        'Procedure Name : prcConfirmRequest()
        'Purpose        : Confirm Pin Request
        'Arguments      : User Id,Data Set
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Function prcUserCode(ByVal lngUserId As Long, _
                    ByVal dsPinRequisition As System.Data.DataSet) As System.Data.DataSet

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            'Create Instance of System Datarow
            Dim drUserCode As System.Data.DataRow

            'Create Instance of System Data Row
            Dim drNewRow As System.Data.DataRow

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsUserCode As New System.Data.DataSet

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Variable Declarations
            Dim strUserType As String = "", strUserType1 As String, strCodeType As String = ""
            Dim strPassLock As String = "", strAuthLock As String = "", strUserFlag As String = "", intRow As Int16
            Dim intCounter As Int16, strPassLock1 As String = "", strAuthLock1 As String = "", lngOrgId As Long

            Try

                'Intialise Row Count
                intRow = 0

                'Loop Thro the Data Grid - Start
                For Each dgiRequisition In dgRequisition.Items

                    'Get Organisation Id
                    lngOrgId = dgiRequisition.Cells(0).Text

                    'Populate Data Set
                    dsUserCode = clsPinMailer.fncUserCode(lngUserId, lngOrgId)

                    'Initialise Counter
                    intCounter = 0

                    'Loop Thro Data Set - Start
                    For Each drUserCode In dsUserCode.Tables("USERCODE").Rows
                        intCounter = intCounter + 1
                        If intCounter = 1 Then
                            strUserType = IIf(IsDBNull(drUserCode("UFLAG")), "N", drUserCode("UFLAG"))      'Get User Type
                            strPassLock = IIf(IsDBNull(drUserCode("PLOCK")), "N", drUserCode("PLOCK"))      'Get Password Lock Status
                            strAuthLock = IIf(IsDBNull(drUserCode("ALOCK")), "N", drUserCode("ALOCK"))      'Get Auth Code Lock Status
                        ElseIf intCounter = 2 Then
                            strUserType1 = IIf(IsDBNull(drUserCode("UFLAG")), "N", drUserCode("UFLAG"))     'Get User Type
                            strPassLock1 = IIf(IsDBNull(drUserCode("PLOCK")), "N", drUserCode("PLOCK"))     'Get Password Lock Status
                            strAuthLock1 = IIf(IsDBNull(drUserCode("ALOCK")), "N", drUserCode("ALOCK"))     'Get Auth Code Lock Status
                        End If
                    Next
                    'Loop Thro Data Set - Stop

                    'If One Record
                    If intCounter = 1 Then

                        Select Case strUserType
                            'If Sys Admin
                            Case "CA"
                                strUserFlag = "Sys Admin"
                                'If Sys Auth
                            Case "SA"
                                strUserFlag = "Sys Auth"
                        End Select

                        'If Password and Auth Code Locked
                        If strPassLock = "Y" And strAuthLock = "Y" Then
                            strCodeType = "Both"
                            'If Password Only
                        ElseIf strPassLock = "Y" And strAuthLock = "N" Then
                            strCodeType = "Password"
                            'If Auth Code Only
                        ElseIf strPassLock = "N" And strAuthLock = "Y" Then
                            strCodeType = "Auth Code"
                        End If

                        dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                        dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type

                        'If two Records
                    ElseIf intCounter = 2 Then

                        strUserFlag = "Both"
                        'If Password & Auth Code 
                        If strPassLock = "Y" And strPassLock1 = "Y" And strAuthLock = "Y" And strAuthLock1 = "Y" Then
                            strCodeType = "Both"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                            'If Password Only
                        ElseIf strPassLock = "Y" And strPassLock1 = "Y" And strAuthLock = "N" And strAuthLock1 = "N" Then
                            strCodeType = "Password"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                            'If Auth Code Only
                        ElseIf strAuthLock = "Y" And strAuthLock1 = "Y" And strPassLock = "N" And strPassLock1 = "N" Then
                            strCodeType = "Auth Code"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                            'If Password & Sys Admin Only 
                        ElseIf strPassLock = "Y" And strPassLock1 = "N" And strAuthLock = "N" And strAuthLock1 = "N" Then
                            strUserFlag = "Sys Admin"
                            strCodeType = "Password"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                            'If Password & Sys Auth Only
                        ElseIf strPassLock1 = "Y" And strPassLock = "N" And strAuthLock = "N" And strAuthLock1 = "N" Then
                            strUserFlag = "Sys Auth"
                            strCodeType = "Auth Code"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                            'If Auth Code & Sys Admin
                        ElseIf strAuthLock = "Y" And strAuthLock1 = "N" And strPassLock = "N" And strPassLock1 = "N" Then
                            strUserFlag = "Sys Admin"
                            strCodeType = "Auth Code"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                            'If Auth Code & Sys Auth
                        ElseIf strAuthLock1 = "Y" And strAuthLock = "N" And strPassLock = "N" And strPassLock1 = "N" Then
                            strUserFlag = "Sys Auth"
                            strCodeType = "Auth Code"
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(3) = strUserFlag    'Set User Type
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Item(4) = strCodeType    'Set Code Type
                        Else

                            'Build Row Details - Start
                            dsPinRequisition.Tables("ORGPIN").Rows(intRow).Delete()
                            drNewRow = dsPinRequisition.Tables("ORGPIN").NewRow()
                            drNewRow.Item(0) = dgiRequisition.Cells(0).Text
                            drNewRow.Item(1) = dgiRequisition.Cells(1).Text
                            drNewRow.Item(2) = dgiRequisition.Cells(2).Text
                            'Build Row Details - Stop

                            'If Sys Admin Password & Auth Code
                            If strPassLock = "Y" And strAuthLock = "Y" Then
                                drNewRow.Item(3) = "Sys Admin"
                                drNewRow.Item(4) = "Both"
                                dsPinRequisition.Tables("ORGPIN").Rows().Add(drNewRow)
                                'If Sys Admin and Password
                            ElseIf strPassLock = "Y" And strAuthLock = "N" Then
                                drNewRow.Item(3) = "Sys Admin"
                                drNewRow.Item(4) = "Password"
                                dsPinRequisition.Tables("ORGPIN").Rows().Add(drNewRow)
                                'If Sys Admin and Auth Code
                            ElseIf strPassLock = "N" And strAuthLock = "Y" Then
                                drNewRow.Item(3) = "Sys Admin"
                                drNewRow.Item(4) = "Auth Code"
                                dsPinRequisition.Tables("ORGPIN").Rows().Add(drNewRow)
                            End If

                            'Build Row Details - Start
                            drNewRow = dsPinRequisition.Tables("ORGPIN").NewRow()
                            drNewRow.Item(0) = dgiRequisition.Cells(0).Text
                            drNewRow.Item(1) = dgiRequisition.Cells(1).Text
                            drNewRow.Item(2) = dgiRequisition.Cells(2).Text
                            'Build Row Details - Stop

                            'If Sys Auth Password & Auth Code
                            If strPassLock1 = "Y" And strAuthLock1 = "Y" Then
                                drNewRow.Item(3) = "Sys Auth"
                                drNewRow.Item(4) = "Both"
                                dsPinRequisition.Tables("ORGPIN").Rows().Add(drNewRow)
                                'If Sys Auth and Password
                            ElseIf strPassLock1 = "Y" And strAuthLock1 = "N" Then
                                drNewRow.Item(3) = "Sys Auth"
                                drNewRow.Item(4) = "Password"
                                dsPinRequisition.Tables("ORGPIN").Rows().Add(drNewRow)
                                'If Sys Auth and Auth Code
                            ElseIf strPassLock1 = "N" And strAuthLock1 = "Y" Then
                                drNewRow.Item(3) = "Sys Auth"
                                drNewRow.Item(4) = "Auth Code"
                                dsPinRequisition.Tables("ORGPIN").Rows().Add(drNewRow)
                            End If

                        End If

                    End If

                    'Increment Row Count
                    intRow = intRow + 1

                Next
                'Loop Thro the Data Grid - Stop

                Return dsPinRequisition

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcUserCode - PG_Requisition", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Datagrid Item
                dgiRequisition = Nothing

                'Destroy Instance of system Data Row
                drUserCode = Nothing

                'Destroy Instance of System Data Row
                drNewRow = Nothing

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

                'Destroy Instance of System Data Set
                dsUserCode = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

    End Class

End Namespace
