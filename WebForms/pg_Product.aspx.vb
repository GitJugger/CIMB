Option Strict Off
Option Explicit On 

Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient


Namespace MaxPayroll


    Partial Class PG_Product
        Inherits clsBasePage


#Region "Declaration"
        Private ReadOnly Property rq_PageMode() As Integer
            Get
                If IsNumeric(Request.QueryString("PageMode")) Then
                    Return Request.QueryString("PageMode")
                Else
                    Return enmPageMode.NewMode
                End If
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
        Private ReadOnly Property rq_iProductID() As Integer
            Get
                If IsNumeric(Request.QueryString("ID")) Then
                    Return CInt(Request.QueryString("ID"))
                Else
                    Return 0
                End If
            End Get
        End Property
#End Region

#Region "Page Load"


        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            Dim clsUsers As New MaxPayroll.clsUsers
            Me.lblMessage.Text = ""
            Try
                If Not ss_strUserType = gc_UT_BankAdmin Then
                    Server.Transfer(gc_LogoutPath, False)
                End If
                'BindBody(body)
                If Not Page.IsPostBack Then
                    If rq_PageMode = enmPageMode.EditMode Then
                        Me.lblHeading.Text = "Modify Product"
                        Me.btnReset.Text = enmButton.Reset.ToString
                        Me.btnSave.Text = enmButton.Update.ToString
                        prcDisplayInfo()
                    ElseIf rq_PageMode = enmPageMode.NewMode Then
                        Me.lblHeading.Text = "Create Product"
                        Me.btnReset.Text = enmButton.Clear.ToString
                        Me.btnSave.Text = enmButton.Save.ToString
                    Else
                        prcDisableFields()
                        lblHeading.Text = "Application Error: Invalid Page Parameter. Please contact Administrator at " & gc_Const_CompanyContactNo
                    End If
                    Call clsUsers.prcDetailLog(ss_lngUserID, lblHeading.Text, "Y")
                End If
            Catch ex As Exception
                'Purpose: To log error message
                LogError("PG_Product - Page Load")
                Me.lblMessage.Text += "Application Error: Page Load " & ex.Message
            Finally
                'Destroy Instance of User Class Object
                clsUsers = Nothing
            End Try

        End Sub

#End Region
        
        Private Sub prcDisableFields(Optional ByVal bIsConfirm As Boolean = False)
            txtProductLink.Enabled = False
            txtProductName.Enabled = False
            rblStatus.Enabled = False
            If bIsConfirm = False Then
                btnSave.Enabled = False
                btnReset.Enabled = False
            End If
        End Sub

        Private Sub prcDisplayInfo()
            Dim oItem As New clsProduct
            oItem = oItem.fncRetrieveData(rq_iProductID)
            txtProductName.Text = oItem.sProductName
            txtProductLink.Text = oItem.sProductLink
            rblStatus.SelectedValue = oItem.sStatus
            txtProductName.Enabled = False
        End Sub

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            If Me.rq_PageMode = enmPageMode.NewMode Then
                Response.Redirect("pg_Product.aspx?PageMode=" & enmPageMode.NewMode)
            ElseIf Me.rq_PageMode = enmPageMode.EditMode Then
                Response.Redirect("pg_Product.aspx?ID=" & Me.rq_iProductID & "&PageMode=" & rq_PageMode)
            End If
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim oItem As New clsProduct
            Dim bRetVal As Boolean
            Dim sMsg As String = ""
            Dim clsCommon As New MaxPayroll.clsCommon
            Try

                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtProductLink.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                'Assign Data - Start
                oItem.iProductID = Me.rq_iProductID
                oItem.sProductName = txtProductName.Text
                oItem.sProductLink = txtProductLink.Text
                oItem.sStatus = rblStatus.SelectedValue
                'Assign Data - End

                'Confirm/Update/Save Process - Start
                Select Case btnSave.Text
                    Case fncBindText(enmButton.Save.ToString), fncBindText(enmButton.Update.ToString)
                        btnSave.Text = enmButton.Confirm.ToString
                        sMsg = "Please confirm your changes."
                        prcDisableFields(True)
                    Case fncBindText(enmButton.Confirm.ToString)
                        If Me.rq_PageMode = enmPageMode.NewMode Then
                            bRetVal = oItem.fncSaveData(oItem)
                            If bRetVal Then
                                sMsg = "Product Creation Successfully."
                                prcDisableFields()
                            Else
                                sMsg = "Product Creation Failed."
                            End If
                        ElseIf Me.rq_PageMode = enmPageMode.EditMode Then
                            bRetVal = oItem.fncUpdateData(oItem)
                            If bRetVal Then
                                sMsg = "Product Modification Successfully."
                                prcDisableFields()
                            Else
                                sMsg = "Product Modification Failed."
                            End If
                        End If
                End Select
                'Confirm/Update/Save Process - End

            Catch ex As Exception
                LogError("pg_Product - btnSave")
                If Me.rq_PageMode = enmPageMode.NewMode Then
                    sMsg = "Product Creation Failed."
                ElseIf Me.rq_PageMode = enmPageMode.EditMode Then
                    sMsg = "Product Modification Failed."
                End If
            End Try
           
            lblMessage.Text = sMsg

        End Sub
    End Class

End Namespace
