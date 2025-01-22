Option Strict Off
Option Explicit On 

Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient


Namespace MaxPayroll


    Partial Class PG_BankCodeMapping
        Inherits clsBasePage


#Region "Declaration"
      
        Private ReadOnly Property rq_iPageNo() As Integer
            Get
                If IsNumeric(Request.QueryString("PageNo")) Then
                    Return CInt(Request.QueryString("PageNo"))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_OrgID() As String
            Get
                Return Request.QueryString("ID") & ""
            End Get
        End Property
        Private ReadOnly Property rq_OrgName() As String
            Get
                Return Request.QueryString("Name") & ""
            End Get
        End Property
#End Region

#Region "Page Load"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            Dim clsUsers As New MaxPayroll.clsUsers
            Me.lblMessage.Text = ""
            Try

                If Not ss_strUserType = gc_UT_BankUser Then
                    Server.Transfer(gc_LogoutPath, False)
                End If
                'BindBody(body)
                If Not Page.IsPostBack Then
                    'hPath.Value = "PG_BankCodeMapping.aspx?Id=" & rq_OrgID & "&Name=" & rq_OrgName
                    If rq_OrgID <> ss_lngOrgID Then
                        HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                        Exit Try
                    End If
                    Me.lblOrganization.Text = rq_OrgName
                    Call clsUsers.prcDetailLog(ss_lngUserID, "View Bank File Settings", "Y")
                    BindGrid()
                End If
            Catch ex As Exception
                'Purpose: To log error message
                LogError("PG_BankCodeMapping - Page Load")
                Me.lblMessage.Text += "Application Error: Page Load " + ex.Message
            Finally
                'Destroy Instance of User Class Object
                clsUsers = Nothing
            End Try

        End Sub

#End Region

#Region "Data Grid Navigation"

        Protected Sub dgBankCodeMapping_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgBankCodeMapping.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim txtbox As New TextBox
                    txtbox = CType(e.Item.FindControl("txtCustBankCode"), TextBox)
                    txtbox.Text = e.Item.Cells(enmDGItem.hCustomerBankCode).Text.Replace(gc_Space, "")
            End Select
        End Sub

        Sub Page_Change(ByVal Sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgBankCodeMapping.PageIndexChanged
            Try
                BindGrid()
                dgBankCodeMapping.CurrentPageIndex = e.NewPageIndex
            Catch ex As Exception
                LogError("prBindGrid - Page_Change")
                lblMessage.Text = "Application Error: Page_Change " + ex.Message
            End Try
        End Sub
#End Region

#Region "Bind Data Grid"

        Enum enmDGItem
            BankCode
            BankName
            txtCustBankCode
            hBankID
            hCustomerBankCode
        End Enum

        '****************************************************************************************************
        'Function Name  : prBindGrid()
        'Purpose        : Bind The User Data Grid
        'Arguments      : N/A
        'Return Value   : Data Grid
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub BindGrid(Optional ByVal bPredefinedLoadPageNo As Boolean = False)

            'Create Bank Class Object
            Dim clsBankCodeMapping As New MaxPayroll.clsBankCodeMapping


            'Create Data Set
            Dim dsBankCodeMapping As New System.Data.DataSet

            Try
                Me.dgBankCodeMapping.DataSource = clsBankCodeMapping.fncGetBankCodeMapping(rq_OrgID)
                dgBankCodeMapping.DataBind()
            Catch ex As Exception
                LogError("BindGrid - PG_BankCodeMapping")
                lblMessage.Text = "Application Error: BindGrid " + ex.Message
            Finally
                clsBankCodeMapping = Nothing
            End Try

        End Sub

#End Region


        Protected Sub btnDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefault.Click
            Dim dgi As DataGridItem
            Dim txtbox As New TextBox
            For Each dgi In Me.dgBankCodeMapping.Items
                txtbox = CType(dgi.FindControl("txtCustBankCode"), TextBox)
                If Len(txtbox.Text.Trim) = 0 Then
                    txtbox.Text = dgi.Cells(enmDGItem.BankCode).Text
                End If
            Next
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim dgi As DataGridItem
            Dim txtbox As New TextBox
            Dim sMsg As String = ""
            Dim stat As Int16 = 0
            Try
                Dim clsCommon As New MaxPayroll.clsCommon
                Dim sCannotBeBlank As String = fncBindText("'s Customer Defined Bank Code cannot be blanked.")
                If btnSave.Text.ToLower = "save" Then

                    For Each dgi In Me.dgBankCodeMapping.Items
                        txtbox = CType(dgi.FindControl("txtCustBankCode"), TextBox)

                        txtbox.Enabled = False
                        If txtbox.Text.Trim = "" Then
                            sMsg += dgi.Cells(enmDGItem.BankName).Text & sCannotBeBlank & gc_BR
                        Else
                            Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtbox.Text.Trim)
                            If strEncUsername = False Then
                                stat = 1
                                Exit For
                            End If
                        End If
                    Next
                    If stat = 1 Then
                        Response.Write(clsCommon.ErrorCodeScript())
                    End If
                    If Len(sMsg) = 0 Then
                        lblMessage.Text = "Please confirm the changes."
                        btnSave.Text = "Confirm"
                    Else
                        For Each dgi In Me.dgBankCodeMapping.Items
                            txtbox = CType(dgi.FindControl("txtCustBankCode"), TextBox)
                            txtbox.Enabled = True
                        Next
                        lblMessage.Text = sMsg
                    End If
                Else
                    Dim oItem As New clsBankCodeMapping
                    Dim arrTemp As New ArrayList

                    'clsBankCodeMapping.fncSaveBankCodeMapping()
                    For Each dgi In Me.dgBankCodeMapping.Items
                        txtbox = CType(dgi.FindControl("txtCustBankCode"), TextBox)
                        txtbox.Enabled = True

                        Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtbox.Text)
                        If strEncUsername = False Then
                            stat = 1
                            Exit For
                        End If

                        oItem = New clsBankCodeMapping
                        oItem.iBankID = CInt(dgi.Cells(enmDGItem.hBankID).Text)
                        oItem.sCustomerBankCode = txtbox.Text
                        arrTemp.Add(oItem)
                        'oItem = Nothing
                    Next
                    If stat = 1 Then
                        Response.Write(clsCommon.ErrorCodeScript())
                    End If
                    If oItem.fncSaveBankCodeMapping(arrTemp, rq_OrgID) Then
                        lblMessage.Text = "Bank Code Mapping Saved Successfully."
                    Else
                        lblMessage.Text = "Bank Code Mapping Saving Failed."
                    End If
                    btnSave.Text = "Save"
                End If
            Catch ex As Exception
                LogError("prBindGrid - Page_Change")
                lblMessage.Text = "Application Error: Page_Change " + ex.Message
            End Try
        End Sub

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            Response.Redirect("PG_BankCodeMapping.aspx?Id=" & rq_OrgID & "&Name=" & rq_OrgName)
        End Sub
    End Class

End Namespace
