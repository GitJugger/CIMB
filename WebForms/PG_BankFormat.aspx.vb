Option Strict Off
Option Explicit On 

Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient


Namespace MaxPayroll


Partial Class PG_BankFormat
      Inherits clsBasePage


#Region "Declaration"

        ReadOnly Property cmb_intBankID() As Integer
            Get
                If IsNumeric(cmbBank.SelectedValue) Then
                    Return CInt(cmbBank.SelectedValue)
                Else
                    Return 0
                End If
            End Get
        End Property
        ReadOnly Property cmb_strBankDesc() As String
            Get
                Return cmbBank.SelectedItem.Text & ""
            End Get
        End Property
        ReadOnly Property cmb_strFileType() As String
            Get
                Return Trim(cmbType.SelectedItem.Text & "")
            End Get
        End Property
        Private ReadOnly Property rq_iPageNo() As Integer
            Get
                If IsNumeric(Request.QueryString("PageNo")) Then
                    Return CInt(Request.QueryString("PageNo"))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_iBankID() As Integer
            Get
                If IsNumeric(Request.QueryString("BankID")) Then
                    Return CInt(Request.QueryString("BankID"))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_sType() As String
            Get
                Return Request.QueryString("Type") & ""
            End Get
        End Property
#End Region

#Region "Page Load"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here

            Dim clsBank As New MaxPayroll.clsBank                      'Create Instance Bank Class Object
            Dim clsUsers As New MaxPayroll.clsUsers                    'Create Instance of User Class Object
            Dim clsGeneric As New MaxPayroll.Generic                   'Create Instance of Generic Class

            Me.lblMessage.Text = ""
            Try

                If Not ss_strUserType = gc_UT_BankUser Then
                    Server.Transfer(gc_LogoutPath, False)
                End If

                'Populate the DataGrid - Start
                If Not Page.IsPostBack Then
                    'BindBody(body)
                    Call clsUsers.prcDetailLog(ss_lngUserID, "View Bank File Settings", "Y")
                    prcBindBank()

                    prcBindPaymentService()
                    If Me.cmbBank.Items.Count = 0 Then
                        lblMessage.Text = "Bank Code not created." & gc_BR
                    Else
                        '071018 Else part added by Marcus
                        'Purpose: To hide bank code info if there is a Default Bank Code set up in web.config

                        Dim sRetval As String
                        sRetval = clsCommon.fncDefaultBankChecking(Me.cmbBank, Me.lblBank)
                        If Len(sRetval) > 0 Then
                            lblMessage.Text = sRetval & gc_BR
                        End If

                    End If

                   
                    If Me.cmbType.Items.Count = 0 Then
                        lblMessage.Text += "File Type not created."
                    End If

                    If cmbBank.Items.Count = 0 OrElse cmbType.Items.Count = 0 Then
                        cmbBank.Items.Clear()
                        cmbType.Items.Clear()
                        cmbBank.Enabled = False
                        cmbType.Enabled = False
                        Me.btnCreate.Enabled = False
                        Me.btnNew.Enabled = False
                        Me.btnShow.Enabled = False
                    Else

                        If rq_sType.Length > 0 AndAlso rq_iBankID > 0 Then
                            cmbType.SelectedItem.Text = rq_sType
                            cmbBank.SelectedValue = rq_iBankID
                            prBindGrid(True)
                        End If

                        '071121 1 line added by Marcus
                        prcFillGeneralInfoTextboxes()
                        '071122 1 line added by Marcus
                        fncEnabeDisableGenerateDBTable()

                    End If

                End If
                


            Catch ex As Exception
                '071121 2 lines added by Marcus
                'Purpose: To log error message
                clsGeneric.ErrorLog(ss_lngOrgID(), ss_lngUserID(), "PG_BankFormat.aspx.vb", Err.Number, Err.Description)
                Me.lblMessage.Text += "Application Error: " + ex.Message
            Finally

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try



        End Sub

#End Region

#Region "Fill General info texboxed"
        '071121 Procedure created by Marcus
        'Purpose: To get and display bank format general info in the general info textboxes.
        Private Sub prcFillGeneralInfoTextboxes()

            Dim clsGeneric As New MaxPayroll.Generic
            Dim clsbank As New clsBank
            Dim dsBankFormatMaster As New DataSet

            dsBankFormatMaster = clsbank.fncGetBankFormatMasterInfo(cmb_intBankID(), cmb_strFileType())

            If dsBankFormatMaster.Tables(0).Rows.Count > 0 Then
                Me.txtDBTableName.Text = IIf(IsDBNull(dsBankFormatMaster.Tables(0).Rows(0).Item("TableName")), "", dsBankFormatMaster.Tables(0).Rows(0).Item("TableName"))
                Me.txtInboundFolder.Text = IIf(IsDBNull(dsBankFormatMaster.Tables(0).Rows(0).Item("InFolder")), "", dsBankFormatMaster.Tables(0).Rows(0).Item("InFolder"))
                Me.txtOutboundFolder.Text = IIf(IsDBNull(dsBankFormatMaster.Tables(0).Rows(0).Item("OutFolder")), "", dsBankFormatMaster.Tables(0).Rows(0).Item("OutFolder"))
                Me.txtResponseFolder.Text = IIf(IsDBNull(dsBankFormatMaster.Tables(0).Rows(0).Item("ResponseFolder")), "", dsBankFormatMaster.Tables(0).Rows(0).Item("ResponseFolder"))
            Else
                Me.txtDBTableName.Text = ""
                Me.txtInboundFolder.Text = ""
                Me.txtOutboundFolder.Text = ""
                Me.txtResponseFolder.Text = ""
            End If


        End Sub
#End Region

#Region "Data Grid Navigation"

        Protected Sub dgBank_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgBank.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    'PG_BankField.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"FId")%>&FType=<%#DataBinder.Eval(Container.DataItem,"FLType")%>"
                    Dim hlLink As New HyperLink
                    hlLink = CType(e.Item.FindControl("hlLink"), HyperLink)
                    hlLink.NavigateUrl = "PG_BankField.aspx?PageNo=" & dgBank.CurrentPageIndex.ToString & "&FileType=" & cmbType.SelectedItem.Text & "&BankID=" & cmbBank.SelectedValue.Trim & "&BankDesc=" & cmbBank.SelectedItem.Text & "&ID=" & e.Item.Cells(0).Text & "&FType=" & e.Item.Cells(1).Text
            End Select
        End Sub

        Sub Page_Change(ByVal Sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgBank.PageIndexChanged

            Dim intStart As Integer, strFileType As String

            Try

                intStart = dgBank.CurrentPageIndex * dgBank.PageSize
                dgBank.CurrentPageIndex = e.NewPageIndex
                strFileType = Request.QueryString("Type")
                prBindGrid()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Create Field"


        Private Sub prCreateNew(ByVal O As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

            'Variable Declarations

            Try
                Response.Clear()
                Response.Redirect("PG_BankField.aspx?BankID=" & cmb_intBankID.ToString & "&BankDesc=" & cmb_strBankDesc & "&Type=" & cmb_strFileType & _
                                 "&ServiceId=" & cmbType.SelectedValue, False)

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "View File Type"

        Private Sub prFileType(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnShow.Click

            Try

                'Response.Clear()
                'Response.Redirect("PG_BankFormat.aspx?Type=" & Request.Form("cmbType"), False)
                prBindGrid()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Bind Data Grid"

        '****************************************************************************************************
        'Function Name  : prBindGrid()
        'Purpose        : Bind The User Data Grid
        'Arguments      : N/A
        'Return Value   : Data Grid
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub prBindGrid(Optional ByVal bPredefinedLoadPageNo As Boolean = False)

            'Create Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Data Set
            Dim dsBankFormat As New System.Data.DataSet


            'Variable Declarations
            Dim intRecordCount As Int32

            Try

                dsBankFormat = clsBank.fnBankFormat(cmb_intBankID, cmb_strFileType)
                intRecordCount = dsBankFormat.Tables(0).Rows.Count
                If intRecordCount > 0 Then
                    dgBank.Visible = True
                    dgBank.DataSource = dsBankFormat
                    dgBank.DataBind()
                    lblMessage.Text = ""
                ElseIf intRecordCount = 0 Then
                    lblMessage.Text = "No Records Found"
                    dgBank.Visible = False
                End If
                If bPredefinedLoadPageNo AndAlso rq_iPageNo > 0 Then
                    dgBank.CurrentPageIndex = rq_iPageNo
                    dgBank.DataBind()
                End If

            Catch
                LogError("prBindGrid - PG_BankFormat")
            Finally
                clsBank = Nothing
            End Try

        End Sub

#End Region

#Region "Create Table"

        '071121 Procedure created by Marcus
        Private Sub prcCreateTable(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnCreate.Click

            Dim clsBank As New MaxPayroll.clsBank
            Dim strRetVal As String
            Dim strTableName As String = Me.txtDBTableName.Text

            strRetVal = clsBank.fncIsTableNameExist(strTableName, ss_lngOrgID(), ss_lngUserID())

            If strRetVal = gc_Status_OK Then

                Dim strCrtTableRetVal As String

                strCrtTableRetVal = clsBank.fncCreateDBTable(strTableName, cmb_strFileType, ss_lngOrgID, ss_lngUserID, cmb_intBankID)

                If strCrtTableRetVal.StartsWith("Table created") Then

                    'clsBank.fncBankFormateDBTblGenerated(cmb_intBankID(), cmb_strFileType, ss_lngOrgID, ss_lngUserID)
                    Me.txtDBTableName.ReadOnly = True
                    Me.btnCreate.Enabled = False
                End If

                Me.lblMessage.Text = strCrtTableRetVal

           
            ElseIf strRetVal = gc_Status_Error Then
                Me.lblMessage.Text = "Error, table name exists in the database."
                Exit Sub
            Else
                Me.lblMessage.Text = strRetVal
                Exit Sub
            End If



        End Sub

#End Region

#Region "General"
        Private Sub prcBindBank()
            Dim clsBankMF As New clsBankMF

            Me.cmbBank.DataSource = clsBankMF.fncRetrieveBankCodeName(clsCommon.fncGetPrefix(enmStatus.A_Active))
            Me.cmbBank.DataTextField = "BankName"
            Me.cmbBank.DataValueField = "BankID"
            cmbBank.DataBind()

        End Sub
        Private Sub prcBindPaymentService()
            Dim clsPaymentService As New clsPaymentService
            Me.cmbType.DataSource = clsPaymentService.fncRetrievePaymentService
            Me.cmbType.DataTextField = "PaySer_Desc"
            cmbType.DataValueField = "PaySer_Id"
            cmbType.DataBind()
        End Sub
#End Region

        Protected Sub btnUpdateGeneral_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateGeneral.Click

            Dim clsBank As New clsBank
            Dim strRetVal As String

            strRetVal = clsBank.fnDB_BankFormatMaster(cmb_intBankID(), cmb_strFileType(), Me.txtDBTableName.Text, _
                                        Me.txtInboundFolder.Text, Me.txtOutboundFolder.Text, _
                                        Me.txtResponseFolder.Text, ss_lngOrgID(), ss_lngUserID())

            If strRetVal = gc_Status_Error Then
                Me.lblMessage.Text = "Error, General Info not updated."
            Else
                Me.lblMessage.Text = "General Info updated successfully."
            End If

        End Sub
        '071122 Procedure created by Marcus
        Protected Sub cmbType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbType.SelectedIndexChanged

            prcFillGeneralInfoTextboxes()
            fncEnabeDisableGenerateDBTable()

            dgBank.CurrentPageIndex = 0
            dgBank.DataSource = Nothing
            dgBank.DataBind()

        End Sub
        '071122 Procedure created by Marcus
        Protected Sub cmbBank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBank.SelectedIndexChanged

            prcFillGeneralInfoTextboxes()
            fncEnabeDisableGenerateDBTable()

            dgBank.CurrentPageIndex = 0
            dgBank.DataSource = Nothing
            dgBank.DataBind()
        End Sub

#Region "Disable Generate Table button and DB Table text box if DB Table has already generated"
        '071122 Procedure created by Marcus
        Private Sub fncEnabeDisableGenerateDBTable()

            Dim bIsDBTableGenerated As Boolean
            Dim clsBank As New clsBank

            bIsDBTableGenerated = clsBank.fncCheckBankFormatDbTblGenerated(cmb_intBankID(), cmb_strFileType())

            If bIsDBTableGenerated Then
                Me.txtDBTableName.ReadOnly = True
                Me.btnCreate.Enabled = False
            Else
                Me.txtDBTableName.ReadOnly = False
                Me.btnCreate.Enabled = True
            End If

        End Sub
#End Region
    End Class

End Namespace
