Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll

    Partial Class PG_BankAccount3
        Inherits clsBasePage


        Dim clsCommon As New MaxPayroll.clsCommon
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
            eAccName = 4
            eAccNo = 5
            ePaymentType = 6
            eBankName = 3
            eAccType = 8
            eBankOrgCodeLink = 7
            AddDelete = 9
            eAccountID = 10
            IsDrAccType = 11
        End Enum

#End Region

#Region "Page  Load "

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim strAction As String

            Try
                'if not Bank user or Inquiry User
                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser) Then

                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                Else
                    If ss_strUserType = gc_UT_InquiryUser Then
                        tblSearch.Visible = False
                        btnPrimary.Visible = False
                    End If
                End If

                strAction = hAction.Value
                hStatus.Value = rq_strStatus
                lblHeading.Text = "Organization Bank Accounts"

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
                    Me.BindDDLBank()
                    'Me.BindDDLPaymentType()

                    '071018 8 lines modified by Marcus.
                    'Purpose: To hide bank code info if there is a Default Bank Code set up in web.config.
                    'Populate grid data if no error message return from fncDefaultBankChecking function.
                    'Dim sRetval As String
                    'sRetval = clsCommon.fncDefaultBankChecking(Me.ddlBankName, Me.lblBank)
                    'If Len(sRetval) > 0 Then
                    '    lblMessage.Text = sRetval
                    'Else
                    'body.Attributes.Add("onload", "ddPaymentTypeChanged();")
                    ' If rq_lngOrgId <> ss_lngOrgID Then
                    ' HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                    ' Exit Try
                    'End If
                    Call prcBindGrid(rq_lngOrgId, ss_lngUserID)
                    'End If


                End If

                If strAction = "A" Then
                    'Insert New Bank Account
                    'Call fncBankAccts()

                    'Clear Hidden Box
                    hAction.Value = ""
                End If

                'If Not Me.ddlPaymentType.SelectedValue = 6 Then
                '   Me.lblAccType.Visible = False
                '   Me.ddlAccType.Visible = False
                'Else
                '   Me.lblAccType.Visible = True
                '   Me.ddlAccType.Visible = True
                'End If

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

            Try

                'Populate Bank Accts
                dsBankAccts = clsCustomer.fncGetBankAccts(lngOrgId, lngUserId)

                'bind Primary Account drop down list
                With ddlPrimary
                    .DataSource = dsBankAccts
                    .DataTextField = "ACCPRIM"
                    .DataValueField = "ACCID"
                    .DataBind()
                End With

                'Bind Data Grid
                'fncGeneralGridTheme(dgBankAccts)-- Function call changed to After Bind Grid by Naresh on 26-07-11
                dgBankAccts.DataSource = dsBankAccts
                dgBankAccts.DataBind()

                fncGeneralGridTheme(dgBankAccts)

                '071022 3 lines added by Marcus
                'Purpose: To hide bank info in datagrid if there is a default bank set up in web.config.
                'If clsCommon.fncDefaultBankChecking() Then
                ' dgBankAccts.Columns(enmGridItem.eBankName).Visible = False
                ' End If


            Catch ex As Exception

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

#Region "Bind DropDownList"
        'Private Sub BindDDLPaymentType()
        '    Dim dr As SqlDataReader
        '    dr = SqlHelper.ExecuteReader(Generic.sSQLConnection, CommandType.StoredProcedure, "pg_QryPaymentService")
        '    Me.ddlPaymentType.DataSource = dr
        '    ddlPaymentType.DataBind()
        'End Sub
        Private Sub BindDDLBank()
            'Dim dr As SqlDataReader
            'dr = SqlHelper.ExecuteReader(Generic.sSQLConnection, CommandType.Text, "Select distinct mCor_BankDefinition.BankId,mCor_BankDefinition.BankName from mCor_BankDefinition Inner Join mCor_PaymentWinDef On mCor_BankDefinition.BankID = mCor_PaymentWinDef.BankID Where mCor_BankDefinition.IsMultipleBank=0")
            'Me.ddlBankName.DataSource = dr
            'ddlBankName.DataBind()
            ddlBankName.Items.Add(New ListItem("", 1))
            ddlBankName.Visible = False
            Me.lblBank.Visible = False

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

#Region "Grid Item Databound "

        Protected Sub dgBankAccts_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgBankAccts.ItemDataBound

            If ss_strUserType = gc_UT_InquiryUser Then
                e.Item.Cells(enmGridItem.AddDelete).Visible = False
            End If

            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    'If e.Item.Cells(enmGridItem.ePaymentType).Text = "Payroll File" AndAlso CBool(e.Item.Cells(enmGridItem.IsDrAccType).Text) Then
                    '    e.Item.Cells(enmGridItem.eAccType).Text = "Debit A/C"
                    'ElseIf e.Item.Cells(enmGridItem.ePaymentType).Text = "Payroll File" Then
                    '    e.Item.Cells(enmGridItem.eAccType).Text = "Credit A/C"
                    'Else
                    '    e.Item.Cells(enmGridItem.eAccType).Text = ""
                    'End If


                    'Create Instance of Button
                    'Dim btnDelete As Object = CType(e.Item.Cells(enmGridItem.AddDelete).Controls(0), Object)
                    Dim btnDelete As New Button
                    btnDelete = CType(e.Item.FindControl("btnDelete"), Button)
                    Dim strAccountName As String = e.Item.Cells(enmGridItem.eAccName).Text 'CType(e.Item.FindControl("txtAccName"), TextBox).Text
                    ' Dim ddlAccType As New DropDownList

                    'ddlAccType = CType(e.Item.FindControl("ddlAccType"), DropDownList)
                    'If e.Item.Cells(enmGridItem.IsDrAccType).Text = "True" Then
                    '    ddlAccType.SelectedValue = "1"
                    'Else
                    '    ddlAccType.SelectedValue = "0"
                    'End If

                    'CType(E.Item.Cells(7).FindControl("ancAddOrgCode"), HtmlAnchor).HRef = "javascript:fncShowAccOrgCode('" & rq_lngOrgId() & "','" & E.Item.Cells(0).Text & "','" & E.Item.Cells(1).Text & ")"


                    'Display Alert Message
                    If ddlPrimary.SelectedValue = e.Item.Cells(enmGridItem.eAccountID).Text Then
                        btnDelete.Visible = False
                    Else
                        btnDelete.Attributes("onclick") = "javascript:return confirm('Are you sure you want to delete Bank Account " & strAccountName & "?')"
                    End If

            End Select

        End Sub

#End Region

#Region "Addd "

        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            Dim strBody As String = ""
            Dim strAccNo As String = Me.txtAccNo.Text
            Dim strAccName As String = Me.txtAccName.Text

            'Dim intPaySer_ID As Integer = Me.ddlPaymentType.SelectedValue
            Dim intBankID As Integer = Me.ddlBankName.SelectedValue
            'Dim intAccType As Integer = Me.ddlAccType.SelectedValue

            Dim bIsError As Boolean
            Dim strErrmessage As String = ""
            Dim bIsDuplicate As Boolean

            Dim clsCustomer As New MaxPayroll.clsCustomer
            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Dim bIsUpdIns As Boolean
            Dim bIsPrimary As Boolean

            Try
                If Not strAccNo = "" And Not strAccName = "" Then
                    Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(strAccNo)
                    If strEncUsername = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim strEncUsername1 As Boolean = clsCommon.CheckScriptValidation(strAccName)
                    If strEncUsername = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If

                    'Account No Validation - START
                    'If Not (Mid(strAccNo, 1, 1) = 1 Or Mid(strAccNo, 1, 1) = 2) Then
                    '    bIsError = True
                    '    strErrmessage = "Account Number should start with 1 or 2." & gc_BR
                    'End If

                    'Check Digit for Account No
                    'IsAccNo = clsCheckDigit.fncCheckDigitBank_RHB(strAccNo)
                    'If Not IsAccNo Then
                    '   IsError = True
                    '   strErrmessage = strErrmessage & "No. " & intCounter & ": Invalid Account Number." & gc_BR
                    'End If

                    'Check For Duplicate Account Name

                    bIsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NAME", strAccName, rq_lngOrgId)
                    If bIsDuplicate Then
                        bIsError = True
                        strErrmessage += "Account Name Duplicated." & gc_BR
                    End If

                    'Check For Duplicate Account No Inter Org.
                    bIsDuplicate = clsCustomer.fnOrgValidations("ADD", "UNIQUE ACC", strAccNo, rq_lngOrgId)
                    If bIsDuplicate Then
                        bIsError = True
                        strErrmessage += "Account Number Duplicated." & gc_BR
                    End If

                    'Check For Duplicate Account No Within Org.
                    'bIsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NO", strAccNo, rq_lngOrgId, 0, intPaySer_ID)
                    bIsDuplicate = clsCustomer.fnOrgValidations("ADD", "ACC NO", strAccNo, rq_lngOrgId, 0)
                    If bIsDuplicate Then
                        bIsError = True
                        strErrmessage += "Account Number Duplicated." & gc_BR
                    End If
                    'Account No Validation - STOP
                End If

                'Return Error Message if any Errors
                If bIsError Then
                    lblMessage.Text = strErrmessage
                    Exit Try
                End If

                'Insert/Update Bank Accts
                bIsUpdIns = clsCustomer.fncUpdateBankAccs(rq_lngOrgId, ss_lngUserID, strAccName, strAccNo, intBankID) ', intPaySer_ID, intAccType)

                If bIsUpdIns Then
                    'Set Subscription Fees Account
                    bIsPrimary = clsCustomer.fncDelBankAccts(rq_lngOrgId, ss_lngUserID, 0, "F", True)
                    'Display Success Message
                    lblMessage.Text = "Bank Account Details Created Successfully."
                    Call prcBindGrid(rq_lngOrgId, ss_lngUserID)
                Else
                    'Display Failed Message
                    lblMessage.Text = "Bank Account Details Creation Failed."
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(rq_lngOrgId, ss_lngUserID, "fncBankAccts - PG_BankAccount", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

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
                If Me.ddlPrimary.SelectedValue = lngAccId Then
                    lblMessage.Text = "This is the Primary Account. Deletion Failed."
                Else
                    IsDelete = clsCustomer.fncDelBankAccts(rq_lngOrgId, ss_lngUserID, lngAccId)
                    If IsDelete Then
                        lblMessage.Text = "Bank Account Deleted Successfully"
                        'Call prcBindGrid(rq_lngOrgId, ss_lngUserID)
                    Else
                        lblMessage.Text = "Bank Account Deletion Failed"
                    End If
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

#Region "Grid Page changed "

        Sub dgPageChanged(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Try
                dgBankAccts.CurrentPageIndex = E.NewPageIndex
                Call prcBindGrid(rq_lngOrgId, ss_lngUserID)

            Catch ex As Exception

            End Try

        End Sub

#End Region

    End Class

End Namespace