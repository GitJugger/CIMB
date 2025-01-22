Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports System.Data.SqlClient


Namespace MaxPayroll


Partial Class PG_BankAccount
      Inherits clsBasePage

#Region "Declaration"
      Private ReadOnly Property rq_strStatus() As String
         Get
            Return Request.QueryString("Status") & ""
         End Get
      End Property
      Private ReadOnly Property rq_lngOrgId() As Long
         Get
            If IsNumeric(Request.QueryString("Id")) Then
               Return Request.QueryString("Id")
            Else
               Return -1
            End If
         End Get
      End Property

      Enum enmGridItem
         eAccName = 3
         eAccNo = 4
         ePaymentType = 5
         eBankName = 6
         eAccType = 8
         eAddModify = 7
         AddDelete = 9
         IsDrAccType = 10
      End Enum

#End Region

#Region "Page Load"

    '****************************************************************************************************
    'Procedure Name : Page_Load()
    'Purpose        : Page Load
    'Arguments      : N/A
    'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
    'Created        : 05/03/2005
    '*****************************************************************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal E As System.EventArgs) Handles MyBase.Load

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of Customer Class Object
        Dim clsCustomer As New MaxPayroll.clsCustomer

        'Variable Declarations
         Dim strAction As String

        Try

            strAction = hAction.Value
            hStatus.Value = rq_strStatus
            lblHeading.Text = "Organization Bank Accounts"
           
            'if not Bank user or Inquiry User
            If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Left(hStatus.Value, 1) = "C" Then
                btnPrimary.Enabled = False
                ddlPrimary.Enabled = False
                lblMessage.Text = "Organization has been cancelled."
            End If

            'if inquiry user disable buttons
            If ss_strUserType = gc_UT_InquiryUser Then
               btnPrimary.Enabled = False
               ddlPrimary.Enabled = False
            End If

            If Not Page.IsPostBack Then
                    'Populate Data grid
                    'BindBody(body)
                    If rq_lngOrgId <> ss_lngOrgID Then
                        HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                        Exit Try
                    End If

                    Call prcBindGrid(rq_lngOrgId, ss_lngUserID)
                    End If

                    If strAction = "A" Then
               'Insert New Bank Account
               Call fncBankAccts()

               'Clear Hidden Box
               hAction.Value = ""
            End If


        Catch

            'Error Log
            Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "Page_Load - PG_BankAccount", Err.Number, Err.Description)

        Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Instance of Customer Class Object
            clsCustomer = Nothing

        End Try

    End Sub

#End Region

#Region "Bind Grid"
      '****************************************************************************************************
      'Procedure Name : prcBindGrind()
      'Purpose        : Load Account Grid
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 05/03/2005
      '*****************************************************************************************************
      Private Sub prcBindGrid(ByVal lngOrgId As Long, ByVal lngUserId As Long)

         'Create Instance of DataRow
         Dim drBankAccts As DataRow

         'Create Instance of System Data Set
         Dim dsBankAccts As New DataSet

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of Customer Class Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'Variable Declarations
         Dim intRecordCount As Int16, strStatus As String

         Try

            'Populate Bank Accts
            dsBankAccts = clsCustomer.fncGetBankAccts(lngOrgId, lngUserId)

            'Get Total Records
            intRecordCount = dsBankAccts.Tables("ACCTS").Rows.Count

            'If Organisation Not Cancelled
            strStatus = Left(hStatus.Value, 1)
            If Not strStatus = "C" Then
               drBankAccts = dsBankAccts.Tables("ACCTS").NewRow
               dsBankAccts.Tables("ACCTS").Rows.Add(drBankAccts)
            End If

            'Bind Data Grid
            fncGeneralGridTheme(dgBankAccts)
            dgBankAccts.DataSource = dsBankAccts

            dgBankAccts.DataBind()

            'Insert New Row
            Call prcAddRow(dgBankAccts, intRecordCount)

            'Populate Bank Accts
            dsBankAccts = clsCustomer.fncGetBankAccts(lngOrgId, lngUserId)

            With ddlPrimary
               .DataSource = dsBankAccts
               .DataTextField = "ACCPRIM"
               .DataValueField = "ACCID"
               .DataBind()
            End With

            'if inquiry user disable buttons
            If ss_strUserType = gc_UT_InquiryUser Then
               Dim intColCount As Int16 = dgBankAccts.Columns.Count
               dgBankAccts.Columns(intColCount - 1).Visible = False
            End If

            If dgBankAccts.Items.Count = 0 Then
               pnlGrid.Visible = False
            End If

            Me.dgBankAccts.Items(dgBankAccts.Items.Count - 1).Cells(enmGridItem.eAddModify).FindControl("lblAddOrgCode").Visible = False

         Catch

            'Log Error
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcBindGrid - PG_BankAccount", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

            'Destroy Instance of System Data Row
            drBankAccts = Nothing

            'Destroy Instance of Data Set
            dsBankAccts = Nothing

         End Try

      End Sub

#End Region

#Region "Page Navigation"

      Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         'Variable Declarations
         Dim intStart As Int16

         Try

            intStart = dgBankAccts.CurrentPageIndex * dgBankAccts.PageSize
            dgBankAccts.CurrentPageIndex = E.NewPageIndex
            Call prcBindGrid(rq_lngOrgId, ss_lngUserID)

         Catch ex As Exception

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
      Public Sub dgBankAccts_ItemDataBound(ByVal O As System.Object, ByVal E As DataGridItemEventArgs) Handles dgBankAccts.ItemDataBound

         Try

            If E.Item.ItemType <> ListItemType.Header And E.Item.ItemType <> ListItemType.Footer Then

               'Create Instance of Button
               Dim btnDelete As Object = CType(E.Item.Cells(enmGridItem.AddDelete).Controls(0), Object)
               Dim strAccountName As String = CType(E.Item.FindControl("txtAccName"), TextBox).Text
               Dim ddlAccType As New DropDownList

               ddlAccType = CType(E.Item.FindControl("ddlAccType"), DropDownList)
               If E.Item.Cells(enmGridItem.IsDrAccType).Text = "True" Then
                  ddlAccType.SelectedValue = "1"
               Else
                  ddlAccType.SelectedValue = "0"
               End If

               'CType(E.Item.Cells(7).FindControl("ancAddOrgCode"), HtmlAnchor).HRef = "javascript:fncShowAccOrgCode('" & rq_lngOrgId() & "','" & E.Item.Cells(0).Text & "','" & E.Item.Cells(1).Text & ")"

               'Display Alert Message
               btnDelete.Attributes("onclick") = "javascript:return confirm('Are you sure you want to delete Bank Account " & strAccountName & "?')"
            End If

         Catch ex As Exception
            'MsgBox(ex.Message & ex.StackTrace)
            LogError("pg_BankAccount - dgBankAccts_ItemDataBound")
         End Try

      End Sub

#End Region

#Region "Delete Bank Accts"

      '****************************************************************************************************
      'Procedure Name : dgBankAccts_Delete()
      'Purpose        : To Delete Accounts
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 05/03/2005
      '*****************************************************************************************************
      Public Sub dgBankAccts_Delete(ByVal O As System.Object, ByVal E As DataGridCommandEventArgs)

         'Create Instance of Customer Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'Variable Declarations
         Dim lngAccId As Long, IsDelete As Boolean

         Try
            lngAccId = IIf(IsNumeric(dgBankAccts.DataKeys(E.Item.ItemIndex)), dgBankAccts.DataKeys(E.Item.ItemIndex), 0)    'Get Account Id

            'Delete Bank Account - START
            IsDelete = clsCustomer.fncDelBankAccts(rq_lngOrgId, ss_lngUserID, lngAccId)
            If IsDelete Then
               lblMessage.Text = "Bank Account Deleted Successfully"
               'Call prcBindGrid(rq_lngOrgId, ss_lngUserID)
            Else
               lblMessage.Text = "Bank Account Deletion Failed"
            End If
            'Delete Bank Account - STOP

            'Bind Datagrid
            Call prcBindGrid(rq_lngOrgId, ss_lngUserID)

         Catch ex As Exception
            LogError("PG_BankAccount - dgBankAccts_Delete")
         Finally

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

         End Try

      End Sub

#End Region

#Region "Insert Add Button"

      '****************************************************************************************************
      'Procedure Name : prcAddRow()
      'Purpose        : To Add New Row
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 05/03/2005
      '*****************************************************************************************************
      Private Sub prcAddRow(ByVal dgBankAccts As DataGrid, ByVal intRecordCount As Int16)

         'Create Instance of Button
         Dim btnAdd As Button

         'Create Instance of Textbox
         Dim txtBank As TextBox

         Dim ddlPmtType As DropDownList

         'Create Instance of Label
         Dim lblNo As Label

         'Create Instance of Datagrid Item
         Dim dgiBankAccts As DataGridItem

         'Variable Declarations
         Dim intCounter As Int16, strStatus As String


         Try
            strStatus = Left(hStatus.Value, 1)

            'Loop Thor Data Grid
            For Each dgiBankAccts In dgBankAccts.Items
               If Not strStatus = "C" Then
                        'Remove Delete Button From Subscription Fees Debit Account
                  If intCounter = intRecordCount Then
                     btnAdd = New Button
                     btnAdd.Text = "  Add  "
                            btnAdd.Attributes("onclick") = "document.Form1.ctl00$cphContent$hAction.value = 'A';"
                     lblNo = CType(dgiBankAccts.FindControl("lblNo"), Label)
                     lblNo.Text = intCounter + 1
                     dgiBankAccts.Cells(enmGridItem.AddDelete).Controls.RemoveAt(0)
                     dgiBankAccts.Cells(enmGridItem.AddDelete).Controls.Add(btnAdd)
                     txtBank = CType(dgiBankAccts.FindControl("txtPaymentType"), TextBox)
                     txtBank.ReadOnly = True
                     txtBank.Visible = False
                     txtBank = CType(dgiBankAccts.FindControl("txtBankName"), TextBox)
                     txtBank.ReadOnly = True
                     txtBank.Visible = False
                     'Make Account No and Account Name as Readonly
                  ElseIf intCounter = 0 Then
                     lblNo = CType(dgiBankAccts.FindControl("lblNo"), Label)
                     lblNo.Text = intCounter + 1
                     txtBank = CType(dgiBankAccts.FindControl("txtAccName"), TextBox)
                     txtBank.ReadOnly = True
                     txtBank = CType(dgiBankAccts.FindControl("txtAccNo"), TextBox)
                     txtBank.ReadOnly = True
                     txtBank = CType(dgiBankAccts.FindControl("txtPaymentType"), TextBox)
                     txtBank.ReadOnly = True
                     ddlPmtType = CType(dgiBankAccts.FindControl("ddlPaymentType"), DropDownList)
                     ddlPmtType.Enabled = True
                     ddlPmtType.Visible = False
                     txtBank = CType(dgiBankAccts.FindControl("txtBankName"), TextBox)
                     txtBank.ReadOnly = True
                     ddlPmtType = CType(dgiBankAccts.FindControl("ddlBankName"), DropDownList)
                     ddlPmtType.Enabled = True
                     ddlPmtType.Visible = False
                     txtBank = CType(dgiBankAccts.FindControl("txtOrgCode"), TextBox)
                     txtBank.ReadOnly = True
                     ddlPmtType = CType(dgiBankAccts.FindControl("ddlAccType"), DropDownList)
                     ddlPmtType.Enabled = False
                     dgiBankAccts.Cells(enmGridItem.AddDelete).Controls.RemoveAt(0)
                     'Replace Add Button with Delete Button
                  Else

                     lblNo = CType(dgiBankAccts.FindControl("lblNo"), Label)
                     lblNo.Text = intCounter + 1
                     txtBank = CType(dgiBankAccts.FindControl("txtAccName"), TextBox)
                     txtBank.ReadOnly = True
                     txtBank = CType(dgiBankAccts.FindControl("txtAccNo"), TextBox)
                     txtBank.ReadOnly = True
                     txtBank = CType(dgiBankAccts.FindControl("txtPaymentType"), TextBox)
                     txtBank.ReadOnly = True
                     ddlPmtType = CType(dgiBankAccts.FindControl("ddlPaymentType"), DropDownList)
                     ddlPmtType.Enabled = True
                     ddlPmtType.Visible = False
                     txtBank = CType(dgiBankAccts.FindControl("txtBankName"), TextBox)
                     txtBank.ReadOnly = True
                     ddlPmtType = CType(dgiBankAccts.FindControl("ddlBankName"), DropDownList)
                     ddlPmtType.Enabled = True
                     ddlPmtType.Visible = False
                     ddlPmtType = CType(dgiBankAccts.FindControl("ddlAccType"), DropDownList)
                     ddlPmtType.Enabled = False
                     txtBank = CType(dgiBankAccts.FindControl("txtOrgCode"), TextBox)
                     txtBank.ReadOnly = True
                  End If
               Else
                  lblNo = CType(dgiBankAccts.FindControl("lblNo"), Label)
                  lblNo.Text = intCounter + 1
                  txtBank = CType(dgiBankAccts.FindControl("txtAccName"), TextBox)
                  txtBank.ReadOnly = True
                  txtBank = CType(dgiBankAccts.FindControl("txtAccNo"), TextBox)
                  txtBank.ReadOnly = True
                  txtBank = CType(dgiBankAccts.FindControl("txtPaymentType"), TextBox)
                  txtBank.ReadOnly = True
                  ddlPmtType = CType(dgiBankAccts.FindControl("ddlPaymentType"), DropDownList)
                  ddlPmtType.Enabled = True
                  ddlPmtType.Visible = False
                  ddlPmtType = CType(dgiBankAccts.FindControl("ddlBankName"), DropDownList)
                  ddlPmtType.Enabled = False
                  ddlPmtType = CType(dgiBankAccts.FindControl("ddlBankName"), DropDownList)
                  ddlPmtType.Enabled = True
                  ddlPmtType.Visible = False
                  txtBank = CType(dgiBankAccts.FindControl("txtOrgCode"), TextBox)
                  txtBank.ReadOnly = True
                  dgiBankAccts.Cells(enmGridItem.AddDelete).Controls.RemoveAt(0)
                  'Replace Add Button with Delete Button
               End If
               intCounter = intCounter + 1
            Next

         Catch ex As Exception
            LogError("PG_BankAccount - prcAddRow")
         Finally

            'Destroy Instance of Button
            btnAdd = Nothing

            'Destroy Instance of Datagrid Item
            dgiBankAccts = Nothing

            'Destroy Instance of Text box
            txtBank = Nothing

            'Destroy Instance of Label
            lblNo = Nothing

         End Try
      End Sub

#End Region

#Region "Insert/Update Bank Accounts"

      '****************************************************************************************************
      'Procedure Name : fncBankAccts()
      'Purpose        : To Add New Row
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 05/03/2005
      '*****************************************************************************************************
        Private Sub fncBankAccts()
            Dim clsFileSetting As New clsFileSetting
            If clsFileSetting.fncCheckExistBankFileSetting(CType(dgBankAccts.Items(dgBankAccts.Items.Count - 1).FindControl("ddlBankName"), DropDownList).SelectedValue, CType(dgBankAccts.Items(dgBankAccts.Items.Count - 1).FindControl("ddlPaymentType"), DropDownList).SelectedValue) Then
                'Create Instance of Datagrid Item
                Dim dgiBankAccts As DataGridItem
                'Create Instance of Generic Class Object
                Dim clsGeneric As New MaxPayroll.Generic

                'Create Instance of Customer Class Object
                Dim clsCustomer As New MaxPayroll.clsCustomer

                'Create Instance of Check Digit Class Object
                Dim clsCheckDigit As New MaxPayroll.clsCheckDigit

                'Variable Declarations
                Dim IsAccNo As Boolean


                Dim lngOrgId As Long, lngUserId As Long, IsUpdIns As Boolean, strAccName As String
                Dim strAccNo As String, intAcctId As Int32, IsDuplicate As Boolean, IsPrimary As Boolean
                Dim strCheckDigit As String, strErrMessage As String, intCounter As Int16, IsError As Boolean = False
                Dim intBankID, intPaySer_ID As Int16
                Dim strBnkOrgCode As String
                Try
                    'Initialise Counter
                    intCounter = 1
                    'Error Message
                    strErrMessage = "Errors:<br>"
                    'Get User Id
                    lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
                    'Get Organisation Id
                    lngOrgId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)

                    ''Check For Errors - START
                    'dgiBankAccts = dgBankAccts.Items(dgBankAccts.Items.Count)

                    'strCheckDigit = ""

                    'If IsNumeric(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex)) Then
                    '    intAcctId = Convert.ToInt32(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex))
                    'Else
                    '    intAcctId = 0
                    'End If

                    'strAccNo = CType(dgiBankAccts.FindControl("txtAccNo"), TextBox).Text
                    'strAccName = CType(dgiBankAccts.FindControl("txtAccName"), TextBox).Text

                    'intPaySer_ID = CType(dgiBankAccts.FindControl("ddlPaymentType"), DropDownList).SelectedValue
                    'intBankID = CType(dgiBankAccts.FindControl("ddlBankName"), DropDownList).SelectedValue
                    'strBnkOrgCode = CType(dgiBankAccts.FindControl("txtOrgCode"), TextBox).Text

                    'If Not strAccNo = "" And Not strAccName = "" Then

                    '    'Account No Validation - START
                    '    If Not (Mid(strAccNo, 1, 1) = 1 Or Mid(strAccNo, 1, 1) = 2) Then
                    '        IsError = True
                    '        strErrMessage = strErrMessage & "No. " & intCounter & ": Account Number should start with 1 or 2." & gc_BR
                    '    End If

                    '    'Check Digit for Account No
                    '    IsAccNo = clsCheckDigit.fncCheckDigitBank_RHB(strAccNo)
                    '    If Not IsAccNo Then
                    '        IsError = True
                    '        strErrMessage = strErrMessage & "No. " & intCounter & ": Invalid Account Number." & gc_BR
                    '    End If

                    '    'Check For Duplicate Account Name
                    '    IsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NAME", strAccName, lngOrgId, intAcctId)
                    '    If IsDuplicate Then
                    '        IsError = True
                    '        strErrMessage = strErrMessage & "No. " & intCounter & ": Account Name Duplicated." & gc_BR
                    '    End If

                    '    'Check For Duplicate Account No Inter Org.
                    '    IsDuplicate = clsCustomer.fnOrgValidations("ADD", "UNIQUE ACC", strAccNo, lngOrgId)

                    '    If IsDuplicate Then
                    '        IsError = True
                    '        strErrMessage = strErrMessage & "No. " & intCounter & ": Account No Duplicated in other Organization." & gc_BR
                    '    End If

                    '    'Check For Duplicate Account No Within Org.
                    '    IsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NO", strAccNo, lngOrgId, intAcctId, intPaySer_ID)


                    '    If IsDuplicate Then
                    '        IsError = True
                    '        strErrMessage = strErrMessage & "No. " & intCounter & ": Account No Duplicated." & gc_BR
                    '    End If
                    '    'Account No Validation - STOP

                    'End If

                    ''Check For Errors - STOP

                    'Check For Errors - START
                    For Each dgiBankAccts In dgBankAccts.Items
                        'Only perform checking on last row - Start
                        If intCounter = Me.dgBankAccts.Items.Count Then
                            strCheckDigit = ""

                            'If IsNumeric(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex)) Then
                            '   intAcctId = Convert.ToInt32(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex))
                            'Else
                            '   intAcctId = 0
                            'End If


                            strAccNo = CType(dgiBankAccts.Cells(enmGridItem.eAccNo).FindControl("txtAccNo"), TextBox).Text
                            strAccName = CType(dgiBankAccts.Cells(enmGridItem.eAccName).FindControl("txtAccName"), TextBox).Text

                            intPaySer_ID = CType(dgiBankAccts.Cells(enmGridItem.ePaymentType).FindControl("ddlPaymentType"), DropDownList).SelectedValue
                            intBankID = CType(dgiBankAccts.Cells(enmGridItem.eBankName).FindControl("ddlBankName"), DropDownList).SelectedValue
                            'strBnkOrgCode = CType(dgiBankAccts.FindControl("txtOrgCode"), TextBox).Text

                            If Not strAccNo = "" And Not strAccName = "" Then

                                'Account No Validation - START
                                If Not (Mid(strAccNo, 1, 1) = 1 Or Mid(strAccNo, 1, 1) = 2) Then
                                    IsError = True
                                    strErrMessage = strErrMessage & "No. " & intCounter & ": Account Number should start with 1 or 2." & gc_BR
                                End If

                                'Check Digit for Account No
                                IsAccNo = clsCheckDigit.fncCheckDigitBank_CIMB(strAccNo)
                                If Not IsAccNo Then
                                    IsError = True
                                    strErrMessage = strErrMessage & "No. " & intCounter & ": Invalid Account Number." & gc_BR
                                End If

                                'Check For Duplicate Account Name
                                IsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NAME", strAccName, rq_lngOrgId, intAcctId)
                                If IsDuplicate Then
                                    IsError = True
                                    strErrMessage = strErrMessage & "No. " & intCounter & ": Account Name Duplicated." & gc_BR
                                End If

                                'Check For Duplicate Account No Inter Org.
                                IsDuplicate = clsCustomer.fnOrgValidations("ADD", "UNIQUE ACC", strAccNo, rq_lngOrgId)

                                If IsDuplicate Then
                                    IsError = True
                                    strErrMessage = strErrMessage & "No. " & intCounter & ": Account Number Duplicated in other Organization." & gc_BR
                                End If

                                'Check For Duplicate Account No Within Org.
                                IsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NO", strAccNo, rq_lngOrgId, intAcctId, intPaySer_ID)


                                If IsDuplicate Then
                                    IsError = True
                                    strErrMessage = strErrMessage & "No. " & intCounter & ": Account Number Duplicated." & gc_BR
                                End If
                                'Account No Validation - STOP

                            End If
                        End If
                        'Only perform checking on last row - End

                        'Counter Increment
                        intCounter = intCounter + 1

                    Next
                    'Check For Errors - STOP

                    'Return Error Message if any Errors
                    If IsError Then
                        lblMessage.Text = strErrMessage
                        Call prcAddRow(dgBankAccts, (intCounter - 2))
                        Exit Try
                    End If

                    'Insert/Update Bank Accts
                    IsUpdIns = clsCustomer.fncUpdateBankAccs(rq_lngOrgId, ss_lngUserID, dgBankAccts)

                    If IsUpdIns Then
                        'Set Subscription Fees Account
                        IsPrimary = clsCustomer.fncDelBankAccts(rq_lngOrgId, ss_lngUserID, 0, "F", True)
                        'Display Success Message
                        lblMessage.Text = "Bank Account Details Created Successfully."
                    ElseIf IsUpdIns Then
                        'Display Failed Message
                        lblMessage.Text = "Bank Account Details Creation Failed."
                    End If

                    'Bind Grid
                    Call prcBindGrid(rq_lngOrgId, ss_lngUserID)

                Catch

                    'Log Error
                    Call clsGeneric.ErrorLog(rq_lngOrgId, ss_lngUserID, "fncBankAccts - PG_BankAccount", Err.Number, Err.Description)

                Finally

                    'Destroy Instance of Generic Class Object
                    clsGeneric = Nothing

                    'Destroy Intstance Datagrid Item
                    dgiBankAccts = Nothing

                    'Destroy Instance of Customer Class Object
                    clsCustomer = Nothing

                    'Destroy Instance of Check Digit Class Object
                    clsCheckDigit = Nothing

                End Try
            Else
                Me.lblMessage.Text = clsCommon.fncMsgBankWithNoFormat(CType(dgBankAccts.Items(dgBankAccts.Items.Count - 1).FindControl("ddlBankName"), DropDownList).SelectedItem.Text, CType(dgBankAccts.Items(dgBankAccts.Items.Count - 1).FindControl("ddlPaymentType"), DropDownList).SelectedItem.Text)
                Call prcAddRow(dgBankAccts, (dgBankAccts.Items.Count - 1))
            End If


        End Sub

#End Region

#Region "Set Primary Account"

      '****************************************************************************************************
      'Procedure Name : prcPrimaryAccts()
      'Purpose        : To Set the primary Account
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 05/03/2005
      '*****************************************************************************************************
      Private Sub prcPrimaryAccts(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnPrimary.Click

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of Customer Class Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'Variable Declarations
         Dim lngAccountId As Long, IsPrimary As Boolean

         Try

            lngAccountId = ddlPrimary.SelectedValue                                                 'Get Account Id


                'Set Subscription Fees Account
            IsPrimary = clsCustomer.fncDelBankAccts(rq_lngOrgId, ss_lngUserID, lngAccountId, "P", True)

            'Bind Grid
            Call prcBindGrid(rq_lngOrgId, ss_lngUserID)

            If IsPrimary Then
                    lblMessage.Text = "Subscription Fees Debit A/C Successfully Set"
            Else
                    lblMessage.Text = "Subscription Fees Debit A/C could not be Set"
            End If

         Catch

            'Log Error
            Call clsGeneric.ErrorLog(rq_lngOrgId, ss_lngUserID, "PG_BankAccounts  prcPrimaryAccts", Err.Number, Err.Description)

            'Error Message
                lblMessage.Text = "Subscription Fees Debit A/C could not be Set"

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

         End Try

      End Sub

#End Region

#Region "Populate Payment Type DropDownList"

      Public Function procPmtTypeList() As DataSet

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic
         'Create Instance of System Data Set
         Dim dsPmtType As New DataSet
         'SQL Data Adaptor
         Dim sdaPmtType As SqlDataAdapter
         'Initialize SQL Connection
         Call clsGeneric.SQLConnection_Initialize()

         'Set the datagrid's datasource to the datareader and databind
         sdaPmtType = New SqlDataAdapter("Exec pg_QryPaymentService", clsGeneric.SQLConnection)
         sdaPmtType.Fill(dsPmtType)

         Return (dsPmtType)

      End Function

#End Region

#Region "Populate Bank Name DropDownList"

      Public Function procBankNameList() As DataSet

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic
         'Create Instance of System Data Set
         Dim dsBankName As New DataSet
         'SQL Data Adaptor
         Dim sdaBankName As SqlDataAdapter

         'Initialize SQL Connection
         Call clsGeneric.SQLConnection_Initialize()

         'Set the datagrid's datasource to the datareader and databind  select BankId,BankName from mCor_BankDefinition
         sdaBankName = New SqlDataAdapter("select distinct mCor_BankDefinition.BankId,mCor_BankDefinition.BankName from mCor_BankDefinition Inner Join mCor_PaymentWinDef On mCor_BankDefinition.BankID = mCor_PaymentWinDef.BankID", clsGeneric.SQLConnection)
         sdaBankName.Fill(dsBankName)
         Return dsBankName


         'Terminate SQL Connection
         Call clsGeneric.SQLConnection_Terminate()

         'Destroy Generic Class Object
         clsGeneric = Nothing

         'Destroy SQL Data Adaptor
         sdaBankName = Nothing

         'Destroy Data Set
         dsBankName = Nothing


      End Function

#End Region

   End Class

End Namespace
