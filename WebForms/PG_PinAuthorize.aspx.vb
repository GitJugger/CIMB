Namespace MaxPayroll

    Partial Class PG_PinAuthorize
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
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strAuthLock As String

            Try

                If Not Page.IsPostBack Then
                    'BindBody(body)
                    Dim clsUsers As New clsUsers
                    Call clsUsers.prcDetailLog(ss_lngUserID, "Pin Authorization", "Y")
                End If
                'If User Type Not Bank User - Start
                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_BankAuth) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If User Type Not Bank User - Stop


                'Get Auth Lock Status
                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    btnShow.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                End If

            Catch ex As Exception

                LogError("PG_PinAuthorize - Page Load")

            Finally

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Show"

        '****************************************************************************************************
        'Procedure Name : prcShow()
        'Purpose        : Bind Data grid and Display
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcShow(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnShow.Click

            Try

                Call prcBindGrid()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Approve/UnApprove All"

        '****************************************************************************************************
        'Procedure Name : prcSelect()
        'Purpose        : Approve All Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcSelect(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSelect.Click

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            Try

                For Each dgiRequisition In dgRequisition.Items
                    Dim ddlStatus As DropDownList = CType(dgiRequisition.FindControl("ddlAction"), DropDownList)
                    ddlStatus.SelectedValue = "A"
                Next

            Catch ex As Exception

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : prcUnselect()
        'Purpose        : Unapprove all Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcUnselect(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnUnSelect.Click

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            Try

                For Each dgiRequisition In dgRequisition.Items
                    Dim ddlStatus As DropDownList = CType(dgiRequisition.FindControl("ddlAction"), DropDownList)
                    ddlStatus.SelectedValue = ""
                Next

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Confirm"

        '****************************************************************************************************
        'Procedure Name : prcConfirm()
        'Purpose        : Confirm Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcConfirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            'Variable Declarations
            Dim lngUserId As Long, IsSelected As Boolean

            Try

                IsSelected = False
                lblMessage.Text = ""
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Loop Thro Datagrid - Start
                For Each dgiRequisition In dgRequisition.Items
                    Dim txtRemarks As TextBox = CType(dgiRequisition.FindControl("txtARemarks"), TextBox)
                    Dim ddlAction As DropDownList = CType(dgiRequisition.FindControl("ddlAction"), DropDownList)
                    txtRemarks.Enabled = False
                    If ddlAction.SelectedValue <> "" Then
                        IsSelected = True
                    End If
                    If ddlAction.SelectedValue = "R" And txtRemarks.Text = "" Then
                        lblMessage.Text += "Please enter Reason for [" & dgiRequisition.Cells(2).Text & "]'s Pin Mailer Rejection" & gc_BR
                        txtRemarks.Enabled = True

                    End If
                    ddlAction.Enabled = False


                Next
                If Len(lblMessage.Text) > 0 Then
                    Exit Try
                End If
                'Loop Thro Datagrid - Stop

                If Not IsSelected Then
                    'Loop Thro Datagrid - Start
                    For Each dgiRequisition In dgRequisition.Items
                        Dim txtRemarks As TextBox = CType(dgiRequisition.FindControl("txtARemarks"), TextBox)
                        Dim ddlAction As DropDownList = CType(dgiRequisition.FindControl("ddlAction"), DropDownList)
                        ddlAction.Enabled = True
                        txtRemarks.Enabled = True
                    Next
                    'Loop Thro Datagrid - Stop
                    lblMessage.Text = "Please Approve/Reject at least one Requisition"
                    Exit Try
                End If
                lblMessage.Text = "Please Enter your Validation Code and Confirm your Approval/Rejection."
                trSelect.Visible = False    'Hide Select Buttons
                trSubmit.Visible = True     'Show Submit Button
                trAuthCode.Visible = True   'Show Auth Code

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcConfirm - PG_PinAuthorize", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Submit"

        '****************************************************************************************************
        'Procedure Name : prcSubmit()
        'Purpose        : Submit Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcSubmit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Variable Declarations
            Dim strBody As String, strApprRemark As String
            Dim lngToId As Long, IsInsert As Boolean, strSubject As String
            Dim lngUserId As Long, strAuthMsg As String, intReqId As Int16, strStatus As String

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
                    intReqId = dgiRequisition.Cells(0).Text
                    lngToId = dgiRequisition.Cells(1).Text
                    Dim txtRemarks As TextBox = CType(dgiRequisition.FindControl("txtARemarks"), TextBox)
                    Dim ddlAction As DropDownList = CType(dgiRequisition.FindControl("ddlAction"), DropDownList)
                    strStatus = ddlAction.SelectedValue
                    If Not strStatus = "" Then
                        strApprRemark = txtRemarks.Text
                        IsInsert = clsPinMailer.fncPinRequisition("U", intReqId, 0, lngUserId, "", strStatus, "", "", strApprRemark, "")
                        If Not IsInsert Then
                            lblMessage.Text = "Pin Mailer Requisition Approval/Rejection Failed."
                            Exit Try
                        End If
                    End If
                Next
                'Loop Thro Datagrid - Stop

                'Mail Subject
                strSubject = "Pin Mailer Requisition Approval/Rejection"
                'Mail Body
                strBody = "Pin Mailer Requisition Approval/Rejection Successful."
                'Send Mail
                Call clsCommon.prcSendMails("BANK AUTH", 100000, lngUserId, 0, strSubject, strBody, lngToId)

                tblGrid.Visible = False
                lblMessage.Text = "Pin Mailer Requisition Approval/Rejection Successful."

            Catch

                'Error Message
                lblMessage.Text = "Pin Mailer Requisition Approval/Rejection Failed."

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcSubmitRequest - PG_PinAuthorize", Err.Number, Err.Description)

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

#Region "Navigation"

        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try
                intStart = dgView.CurrentPageIndex * dgView.PageSize
                dgView.CurrentPageIndex = E.NewPageIndex
                Call prcBindGrid()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Bind Grid"

        '****************************************************************************************************
        'Procedure Name : prcBindGrid()
        'Purpose        : Bind Data grid and Display
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcBindGrid()

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Create Instance of System Data Set
            Dim dsPinStatus As New System.Data.DataSet

            'Variable Declarations
            Dim lngUserId As Long, strStatus As String, intDays As Int16

            Try

                intDays = txtDays.Text
                lblMessage.Text = ""
                strStatus = rdStatus.SelectedValue
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Populate Data Set
                dsPinStatus = clsPinMailer.fncPinStatus(lngUserId, strStatus, intDays)

                'Bind Data Grid - Start
                If dsPinStatus.Tables("PINSTATUS").Rows.Count > 0 Then
                    If Session("SYS_TYPE") = "BU" Then
                        fncGeneralGridTheme(dgView)
                        tblView.Visible = True
                        tblGrid.Visible = False
                        dgView.DataSource = dsPinStatus
                        dgView.DataBind()
                    Else
                        fncGeneralGridTheme(dgRequisition)
                        tblGrid.Visible = True
                        tblView.Visible = False
                        trSubmit.Visible = False
                        trAuthCode.Visible = False
                        dgRequisition.DataSource = dsPinStatus
                        dgRequisition.DataBind()
                        If strStatus = "A" Or strStatus = "R" Then
                            trSelect.Visible = False
                            dgRequisition.Columns(9).Visible = False
                        ElseIf strStatus = "P" Then
                            trSelect.Visible = True
                            dgRequisition.Columns(9).Visible = True
                        End If
                    End If
                Else
                    tblGrid.Visible = False
                    tblView.Visible = False
                    lblMessage.Text = "No Requisitions Found"
                End If
                'Bind Data Grid - Stop

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcBindGrid - PG_PinAuthorize", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsPinStatus = Nothing

            End Try

        End Sub

#End Region

    End Class

End Namespace
