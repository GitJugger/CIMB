Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsApprMatrix


Namespace MaxPayroll


Partial Class PG_ApprMatrix
        Inherits clsBasePage

#Region "Declaration"
      Enum enmDGViewPending
         RequestDateTime
         LinkAction
         MDSC
      End Enum
      Enum enmDGView
         RequestDate
         LinkAction
         MDSC
         ActionDate
         Remark
         MDID
         APPR_SUB
         APPR_ID
      End Enum
      Enum enmDGApprove
         CheckBox
         APRID
         FRID
         MDID
         FName
         ReceiveDate
         Subject
         LinkAction
         MDSC
         StatusRadioBox
         Remark
      End Enum
      Private ReadOnly Property rq_iPageNo() As Integer
         Get
            If IsNumeric(Request.QueryString("PageNo")) Then
               Return CInt(Request.QueryString("PageNo"))
            Else
               Return 0
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
      'Created        : 14/02/2005
      '*****************************************************************************************************
      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of Common Class Object
         Dim clsCommon As New MaxPayroll.clsCommon

         'Variable Declarations
         Dim strRequest As String, strAuthLock As String

         Try
                'BindBody(body)

            'Get Authorization Lock Status - Start
            If UCase(Request.QueryString("Mode")) = "EDIT" Then

               strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
               If strAuthLock = "Y" Then
                  btnSubmit.Enabled = False
                  btnConfirm.Enabled = False
                  lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
               End If
            End If
            'Get Authorization Lock Status - Stop

            If Not Page.IsPostBack Then

               trConfirm.Visible = False
               trAuthCode.Visible = False
               strRequest = Request.QueryString("Mode")
               hidMode.Value = strRequest
               'Bind Data Grid
               Call prcBindGrid()

               'Display Messages - START
               Select Case LCase(strRequest)
                  Case "reject"
                     lblHeading.Text = "Rejected Requests"
                  Case "done"
                     lblHeading.Text = "Accepted Requests"
                  Case "view"
                     lblHeading.Text = "Pending Approval"
                  Case "edit"
                     lblHeading.Text = "Pending Requests"
               End Select
               'If UCase(strRequest) = "REJECT" Then
               '   lblHeading.Text = "Rejected Requests"
               'ElseIf UCase(strRequest) = "DONE" Then
               '   lblHeading.Text = "Accepted Requests"
               'ElseIf UCase(strRequest) = "DONE" Then
               '   lblHeading.Text = "Pending Requests"
               'ElseIf UCase(strRequest) = "VIEW" Then
               '   lblHeading.Text = "Pending Authorization"
               'End If
               'Display Messages - STOP

            End If

         Catch

            'Log Error
            Call clsGeneric.ErrorLog(0, 0, "Page Load - PG_ApprMatrix", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

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
      'Created        : 15/02/2005
      '*****************************************************************************************************
      Private Sub Page_Confirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

         'Create Instance of Hidden Box
         Dim hStatus As HtmlInputHidden

         'Create Instance of RadiobuttonList
         Dim rdStatus As RadioButtonList

         'Create Instance of Textbox
         Dim txtRemarks As TextBox

         'Create Instance of Datagrid Item
         Dim dgiApprMatrix As DataGridItem

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of Checkbox
         Dim chkSelect As CheckBox

         'Variable Declarations
         Dim lngOrgId As Long, lngUserId As Long, strStatus As String, strMessage As String
         Dim intCounter As Int16, strRemarks As String, IsChecked As Boolean

         Try

            intCounter = 1
            strMessage = ""
            lblMessage.Text = "Please Enter your Validation Code and Confirm your changes"
            lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id

            For Each dgiApprMatrix In dgApprMatrix.Items

               chkSelect = CType(dgiApprMatrix.FindControl("chkSelect"), CheckBox)         'Get Checkbox Status
               rdStatus = CType(dgiApprMatrix.FindControl("rdStatus"), RadioButtonList)    'Get Status RadiobuttonList
               hStatus = CType(dgiApprMatrix.FindControl("hStatus"), HtmlInputHidden)      'Get HTML Hidden box
               txtRemarks = CType(dgiApprMatrix.FindControl("txtRemarks"), TextBox)        'Get Remarks Textbox

               strRemarks = txtRemarks.Text                                                'Get Remarks Textbox
               strStatus = rdStatus.SelectedValue                                          'Get Status RadiobuttonList

               If chkSelect.Checked Then
                  If strStatus = "" Then
                     IsChecked = True
                     strMessage = strMessage & "Row " & intCounter & " is checked. Please select the required action.<BR>"
                  ElseIf strStatus = 3 And txtRemarks.Text = "" Then
                     IsChecked = True
                     strMessage = strMessage & "Row " & intCounter & " is Rejected. Please enter Remarks.<BR>"
                  Else
                     IsChecked = True
                     rdStatus.Enabled = False
                     chkSelect.Enabled = False
                     btnCheckAll.Enabled = False
                     btnUnCheck.Enabled = False
                     btnAccept.Enabled = False
                     txtRemarks.Enabled = False
                     hStatus.Value = strStatus
                  End If
               Else
                  rdStatus.Enabled = False
                  chkSelect.Enabled = False
                  txtRemarks.Enabled = False
               End If

               intCounter = intCounter + 1

            Next

            If Not IsChecked Then
               trSubmit.Visible = True
               trConfirm.Visible = False
               trAuthCode.Visible = False
               lblMessage.Text = "No Rows Selected."
               For Each dgiApprMatrix In dgApprMatrix.Items
                  chkSelect = CType(dgiApprMatrix.FindControl("chkSelect"), CheckBox)
                  rdStatus = CType(dgiApprMatrix.FindControl("rdStatus"), RadioButtonList)
                  hStatus = CType(dgiApprMatrix.FindControl("hStatus"), HtmlInputHidden)
                  txtRemarks = CType(dgiApprMatrix.FindControl("txtRemarks"), TextBox)
                  rdStatus.Enabled = True
                  chkSelect.Enabled = True
                  btnCheckAll.Enabled = True
                  btnUnCheck.Enabled = True
                  btnAccept.Enabled = True
                  txtRemarks.Enabled = True
               Next
               Exit Try
            End If

            If strMessage = "" Then
               trSubmit.Visible = False
               trConfirm.Visible = True
               trAuthCode.Visible = True
            Else
               trSubmit.Visible = True
               trConfirm.Visible = False
               trAuthCode.Visible = False
               lblMessage.Text = strMessage
            End If

         Catch

            'Log Error
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Confirm - PG_ApprMatrix", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of RadiobuttonList
            rdStatus = Nothing

            'Destroy Instance of Textbox
            txtRemarks = Nothing

            'Destroy Instance of Datagrid Item
            dgiApprMatrix = Nothing

            'Destroy Instance of Hiddenbox
            hStatus = Nothing

            'Destroy Instance of Checkbox
            chkSelect = Nothing

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
      'Created        : 14/02/2005
      '*****************************************************************************************************
        Private Sub Page_Submit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click


            'Create Instance of Checkbox
            Dim chkSelect As CheckBox

            'Create Instance of Datagrid Item
            Dim dgiApprMatrix As DataGridItem

            'Create Instance of Data Set
            Dim dsApprMatrix As New DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of USer Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Approval Matrix Class Object
            Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

            'Variable Declarations
            Dim IsAuthCode As Boolean
            Dim intAttempts As Int16
            Dim strCAuthCode As String
            Dim lngAprId As Long
            Dim lngTransId As Long
            Dim lngToId As Long
            Dim strBody As String
            Dim lngOrgId As Long
            Dim lngUserId As Long
            Dim intStatus As Int16
            Dim strRemarks As String = ""
            Dim strSubject As String
            Dim strStatus As String
            Dim strUserName As String

            Try

                strBody = ""
                strSubject = ""
                trConfirm.Visible = True
                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id

                'Check If AuthCode is Valid - Start
                strCAuthCode = clsCommon.fncPassAuth(lngUserId, "A", lngOrgId)
                IsAuthCode = IIf(strCAuthCode = txtAuthCode.Text, True, False)
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
                            trAuthCode.Visible = False
                            Call clsUsers.prcAuthLock(lngOrgId, lngUserId, "A")
                            'Track Auth Lock
                            Call clsUsers.prcLockHistory(lngUserId, "A")
                            lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                            Exit Try
                        End If
                    End If
                End If
                'Check for invalid Authorization Code Attempts - STOP

                'Loop Thro Datagrid - START
                For Each dgiApprMatrix In dgApprMatrix.Items

                    lngAprId = dgiApprMatrix.Cells(1).Text                                              'Get Apporval Id
                    lngToId = dgiApprMatrix.Cells(2).Text                                               'Get To Id
                    lngTransId = dgiApprMatrix.Cells(3).Text                                            'Get Module Id
                    strSubject = dgiApprMatrix.Cells(6).Text                                            'Get Subject
                    chkSelect = CType(dgiApprMatrix.FindControl("chkSelect"), CheckBox)                 'Get Check Box

                    'Update Status
                    If chkSelect.Checked Then

                        intStatus = CType(dgiApprMatrix.FindControl("hStatus"), HtmlInputHidden).Value  'Get HTML Hidden box
                        strRemarks = CType(dgiApprMatrix.FindControl("txtRemarks"), TextBox).Text       'Get Remarks Textbox

                        'User Creation or Modification
                        If strSubject = "User Creation" Or strSubject = "User Modification" Then
                            Call clsApprMatrix.prcApprTrans(lngOrgId, lngUserId, "USER", intStatus, lngTransId, lngAprId)
                            'If User Deletion
                        ElseIf strSubject = "User Deletion" Then
                            Call clsApprMatrix.prcApprTrans(lngOrgId, lngUserId, "USER DEL", intStatus, lngTransId, lngToId)
                            'If Group Creation or Group Modification or Group Cancellation
                        ElseIf strSubject = "Group Creation" Or strSubject = "Group Modification" Or strSubject = "Group Cancellation" Then
                            Call clsApprMatrix.prcApprTrans(lngOrgId, lngUserId, "GROUP", intStatus, lngTransId)
                            'if cancel organisation
                        ElseIf strSubject = "Cancel Organization" Then
                            Call clsApprMatrix.prcApprTrans(lngOrgId, lngUserId, "ORG", intStatus, lngTransId, lngAprId)
                        ElseIf strSubject = "Mandate File Approval" Then
                            Call clsApprMatrix.prcApprFileMandateTrans(lngOrgId, lngUserId, dgiApprMatrix.Cells(8).Text, intStatus, lngTransId, lngAprId)

                        ElseIf strSubject = "Mandate Record Creation" Then
                            Call clsApprMatrix.prcApprCreateMandateTrans(lngOrgId, lngUserId, dgiApprMatrix.Cells(8).Text, intStatus, lngTransId, lngAprId)
                        ElseIf strSubject = "Mandate Record Modification" Then
                            Call clsApprMatrix.prcApprModifyMandateTrans(lngOrgId, lngUserId, dgiApprMatrix.Cells(8).Text, intStatus, lngTransId, lngAprId)
                        End If

                        'Send Mail - START
                        strStatus = IIf(intStatus = 2, "Accepted", "Rejected")
                        'If User Creation
                        If strSubject = "User Creation" Then
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("User Name", "", lngTransId, lngUserId)
                            'Get Subject
                            strSubject = strUserName & " Creation: " & strStatus
                            'Get Body
                            strBody = strUserName & " Creation has been " & strStatus & ". Remarks: " & strRemarks
                            'User Modification
                        ElseIf strSubject = "User Modification" Then
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("User Name", "", lngTransId, lngUserId)
                            'Get Subject
                            strSubject = strUserName & " Modification: " & strStatus
                            'Get Body
                            strBody = strUserName & " Modification has been " & strStatus & ". Remarks: " & strRemarks
                        ElseIf strSubject = "User Deletion" Then
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("User Name", "", lngTransId, lngUserId)
                            'Get Subject
                            strSubject = strUserName & " Deletion: " & strStatus
                            'Get Body
                            strBody = strUserName & " Deletion has been " & strStatus & ". Remarks: " & strRemarks
                            'If Group Creation
                        ElseIf strSubject = "Group Creation" Then
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("Group Name", "", lngTransId, lngUserId)
                            'Get Subject
                            strSubject = strUserName & " Creation: " & strStatus
                            'Get Body
                            strBody = strUserName & " Creation has been " & strStatus & ". Remarks: " & strRemarks
                            'If Group Modification
                        ElseIf strSubject = "Group Modification" Then
                            'Get User Name
                            strUserName = clsCommon.fncBuildContent("Group Name", "", lngTransId, lngUserId)
                            'Get Subject
                            strSubject = strUserName & " Modification: " & strStatus
                            'Get Body
                            strBody = strUserName & " Creation has been " & strStatus & ". Remarks: " & strRemarks
                        ElseIf strSubject = "Cancel Organization" Then
                            'Get Subject
                            strSubject = lngTransId & " Organization Cancellation: " & strStatus
                            'Get Body
                            strBody = lngTransId & " Organization Cancellation has been " & strStatus & ". Remarks: " & strRemarks

                        ElseIf strSubject = "Mandate Record Creation" OrElse strSubject = "Mandate Record Modification" Then
                            'Get Subject
                            strSubject = dgiApprMatrix.Cells(8).Text & "'s status : " & strStatus
                            'Get Body
                            strBody = dgiApprMatrix.Cells(8).Text & " has been " & strStatus & ". Remarks: " & strRemarks
                        End If

                        'Update Approval Matrix
                        Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "UPDATE", lngAprId, 0, 0, 0, "", "", strRemarks, intStatus)

                        'Send Mail
                        Call clsCommon.prcSendMails("SEND MAIL", lngOrgId, lngUserId, 0, strSubject, strBody, lngToId)

                    End If

                Next
                'Loop Thro Datagrid - STOP

                trConfirm.Visible = False
                trSubmit.Visible = True
                btnCheckAll.Enabled = True
                btnUnCheck.Enabled = True
                btnAccept.Enabled = True
                trAuthCode.Visible = False

                Me.prcBindGrid()
                lblMessage.Text = "Approval Status updated"
                'Populate Datagrid - START
                '    dsApprMatrix = clsApprMatrix.fncListMatrix("PENDING", lngOrgId, lngUserId, Session("SYS_TYPE"))
                '    If dsApprMatrix.Tables(0).Rows.Count > 0 Then
                '        dgApprMatrix.DataSource = dsApprMatrix
                '        dgApprMatrix.DataBind()
                'Else


                '   btnCheckAll.Visible = False
                '   btnUnCheck.Visible = False
                '   btnAccept.Visible = False
                '   trSubmit.Visible = False
                '   trConfirm.Visible = False
                '   dgApprMatrix.Visible = False
                '    End If
                'Populate Datagrid - STOP

            Catch

                'Error Message
                lblMessage.Text = "Approval Matrix updation failed."

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_ApprMatrix", Err.Number, Err.Description)

                'Update Approval Matrix
                Call clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "UPDATE", lngAprId, 0, 0, 0, "", "", strRemarks, 1)

            Finally

                'Destroy Instance of Datagrid item
                dgiApprMatrix = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Approval Matrix Class Object
                clsApprMatrix = Nothing

                'Destroy Instance of Checkbox
                chkSelect = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

            End Try

        End Sub

#End Region

#Region "Check/UnCheck"

      '****************************************************************************************************
      'Procedure Name : prcCheck()
      'Purpose        : Check All Items in Datagrid
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 24/02/2005
      '*****************************************************************************************************
      Private Sub prcCheck(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnCheckAll.Click

         'Create Instance of Datagrid Items
         Dim dgiApprMatrix As DataGridItem

         Try

            For Each dgiApprMatrix In dgApprMatrix.Items
               Dim mychkSelect As CheckBox = CType(dgiApprMatrix.FindControl("chkSelect"), CheckBox)
               mychkSelect.Checked = True
            Next

         Catch ex As Exception

         End Try

      End Sub

      '****************************************************************************************************
      'Procedure Name : prcCheck()
      'Purpose        : Check All Items in Datagrid
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 24/02/2005
      '*****************************************************************************************************
      Private Sub prcUnCheck(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnUnCheck.Click

         'Create Instance of Datagrid Items
         Dim dgiApprMatrix As DataGridItem

         Try

            For Each dgiApprMatrix In dgApprMatrix.Items
               Dim mychkSelect As CheckBox = CType(dgiApprMatrix.FindControl("chkSelect"), CheckBox)
               mychkSelect.Checked = False
            Next

         Catch ex As Exception

         End Try

      End Sub

      '****************************************************************************************************
      'Procedure Name : prcAccept()
      'Purpose        : Accept All Items in Datagrid
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 24/02/2005
      '*****************************************************************************************************
      Private Sub prcAccept(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnAccept.Click

         'Create Instance of Datagrid Items
         Dim dgiApprMatrix As DataGridItem

         Try

            For Each dgiApprMatrix In dgApprMatrix.Items
               Dim myrdSelect As RadioButtonList = CType(dgiApprMatrix.FindControl("rdstatus"), RadioButtonList)
               myrdSelect.SelectedValue = "2"
            Next

         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "Page Navigation"

      Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         Dim intStart As Int16

         Try

            intStart = dgApprMatrix.CurrentPageIndex * dgApprMatrix.PageSize
            dgApprMatrix.CurrentPageIndex = E.NewPageIndex
            Call prcBindGrid()

         Catch ex As Exception

         End Try

      End Sub

      Sub prVPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         Dim intStart As Int16

         Try

            intStart = dgViewMatrix.CurrentPageIndex * dgViewMatrix.PageSize
            dgViewMatrix.CurrentPageIndex = E.NewPageIndex
            Call prcBindGrid()

         Catch ex As Exception

         End Try

      End Sub

      Sub prPPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         Dim intStart As Int16

         Try

            intStart = dgViewPend.CurrentPageIndex * dgViewPend.PageSize
            dgViewPend.CurrentPageIndex = E.NewPageIndex
            Call prcBindGrid()

         Catch ex As Exception

         End Try

      End Sub

#End Region

#Region "Bind Datagrid"
      '****************************************************************************************************
      'Procedure Name : Bind_Grid()
      'Purpose        : DataGrid Bind
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 11/03/2005
      '*****************************************************************************************************
      Private Sub prcBindGrid()

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of System Data Set
         Dim dsApprMatrix As New System.Data.DataSet

         'Create Instance of Approval Matrix Class Object
         Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

         'Variable Declarations
         Dim strRequest As String

         Try

            strRequest = Request.QueryString("Mode") & ""

            'Populate Datagrid - START
            If UCase(strRequest) = "EDIT" Then
               tblView.Visible = False
               tblViewPend.Visible = False
               tblMainForm.Visible = True
               'Populate Data Set
               dsApprMatrix = clsApprMatrix.fncListMatrix("PENDING", ss_lngOrgID, ss_lngUserID, ss_strUserType)
               If dsApprMatrix.Tables("MATRIX").Rows.Count > 0 Then
                  dgApprMatrix.DataSource = dsApprMatrix
                  dgApprMatrix.DataBind()
                  fncGeneralGridTheme(dgApprMatrix)
                  pnlGrid.Visible = True
               Else
                  btnCheckAll.Visible = False
                  btnUnCheck.Visible = False
                  btnAccept.Visible = False
                  trSubmit.Visible = False
                  dgApprMatrix.Visible = False
                  pnlGrid.Visible = False
                  lblMessage.Text = "No Records for Approval."
               End If
            ElseIf UCase(strRequest) = "VIEW" Then
               tblView.Visible = False
               tblViewPend.Visible = True
               tblMainForm.Visible = False
               dsApprMatrix = clsApprMatrix.fncListMatrix(UCase(strRequest), ss_lngOrgID, ss_lngUserID, ss_strUserType)
               If dsApprMatrix.Tables("MATRIX").Rows.Count > 0 Then
                  dgViewPend.DataSource = dsApprMatrix
                  dgViewPend.DataBind()
                  pnlGridPending.Visible = True
                  fncGeneralGridTheme(dgViewPend)
               Else
                  pnlGridPending.Visible = False
                  dgViewPend.Visible = False
                  lblMessage.Text = "No Records Available."
               End If
            ElseIf UCase(strRequest) = "REJECT" Or UCase(strRequest) = "DONE" Then
               tblView.Visible = True
               tblViewPend.Visible = False
               tblMainForm.Visible = False
               dsApprMatrix = clsApprMatrix.fncListMatrix(UCase(strRequest), ss_lngOrgID, ss_lngUserID, ss_strUserType)
               If dsApprMatrix.Tables("MATRIX").Rows.Count > 0 Then
                  dgViewMatrix.DataSource = dsApprMatrix
                  If rq_iPageNo > 0 Then
                     dgViewMatrix.CurrentPageIndex = rq_iPageNo
                  End If
                  dgViewMatrix.DataBind()
                  pnlGridView.Visible = True

                  fncGeneralGridTheme(dgViewMatrix)
               Else
                  pnlGridView.Visible = False
                  dgViewMatrix.Visible = False
                  lblMessage.Text = "No Records Available."
               End If
            End If
            'Populate Datagrid - STOP

         Catch

            'Log Error
            Call clsGeneric.ErrorLog(0, 0, "prcBindGrid - PG_ApprMatrix", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of System Data Set
            dsApprMatrix = Nothing

            'Destroy Instance of Approval Matrix
            clsApprMatrix = Nothing

         End Try

      End Sub

#End Region

        Protected Sub dgViewMatrix_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgViewMatrix.ItemDataBound
            'href="PG_ViewApprMatrix.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"MDID")%>&Module=<%#DataBinder.Eval(Container.DataItem,"APPR_SUB")%>&Mode=<%=Request.QueryString("Mode")%>&Appr=<%#DataBinder.Eval(Container.DataItem,"APPR_ID")%>"
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim lnkAction As HtmlAnchor
                    lnkAction = CType(e.Item.FindControl("lnkAction"), HtmlAnchor)
                    Dim EncryptId = GetEncrypterString(e.Item.Cells(enmDGView.MDID).Text)
                    Dim EncryptAppr = GetEncrypterString(e.Item.Cells(enmDGView.APPR_ID).Text)
                    lnkAction.HRef = "PG_ViewApprMatrix.aspx?PageNo=" & dgViewMatrix.CurrentPageIndex.ToString & "&Id=" & EncryptId & "&Module=" & e.Item.Cells(enmDGView.APPR_SUB).Text & "&Mode=" & Request.QueryString("Mode") & "&Appr=" & EncryptAppr
            End Select
        End Sub
        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function
    End Class

End Namespace
