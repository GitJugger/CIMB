Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data


Namespace MaxPayroll
    Partial Class pg_BankOrgCode
        Inherits clsBasePage

#Region "Declaration"
        Private ReadOnly Property rq_lngOrgId() As Integer
            Get
                If IsNumeric(Request.QueryString("OrgId")) Then
                    Return CLng(Request.QueryString("OrgId"))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_iAccId() As Integer
            Get
                If IsNumeric(Request.QueryString("AccId")) Then
                    Return CInt(Request.QueryString("AccId"))
                Else
                    Return 0
                End If
            End Get
        End Property
        Private ReadOnly Property rq_sAccNo() As String
            Get
                Return Request.QueryString("AccNo") & ""
            End Get
        End Property
#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                If Not ss_strUserType.Equals(gc_UT_BankUser) Then
                    Response.Write("<script language='javascript'>window.returnValue=true;self.close();</script>")
                    Exit Sub
                End If
                BindBody(body, False, False)
                Me.lblAccNo.Text = rq_sAccNo
                BindGrid()
            End If
        End Sub

        Private Sub BindGrid()
            If rq_iAccId = 0 OrElse rq_lngOrgId = 0 Then
                lblMessage.Text = "Error on parameters, pls contact " & gc_Const_CompanyName & " " & gc_Const_RegCenter & "."
                Me.pnlGrid.Visible = False
            Else
                Me.pnlGrid.Visible = True
                Dim oBank As New clsBank

                Me.dgBankOrgCode.DataSource = oBank.fncGetBankOrgCode(rq_lngOrgId, rq_sAccNo)
                dgBankOrgCode.DataBind()
                If dgBankOrgCode.Items.Count = 0 Then
                    pnlGrid.Visible = False
                Else
                    pnlGrid.Visible = True
                End If

            End If
        End Sub

        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click

            If Validation(lblMessage.Text) Then
                If Save() Then
                    BindGrid()
                    txtBnkOrgCode.Text = ""
                Else
                    lblMessage.Text = "Bank Organization Code saving fail."
                End If
            End If


        End Sub

        Private Function Save() As Boolean
            'Create Instance of SQL Command Object
            Dim cmdInseart As New SqlCommand

            Dim bRetVal As Boolean = False

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Call clsGeneric.SQLConnection_Initialize()

            Try
                With cmdInseart
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_InsertBankAccountOrgCode"
                    .CommandType = CommandType.StoredProcedure

                    .Parameters.Add(New SqlParameter("@in_OrgId", rq_lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_AccNo", rq_sAccNo))
                    .Parameters.Add(New SqlParameter("@in_OrgCode", Me.txtBnkOrgCode.Text.Trim))
                    .ExecuteNonQuery()
                    bRetVal = True
                End With
            Catch

                'Log Error
                Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, "pg_BankOrgCode - Insert", Err.Number, Err.Description)
                Me.lblMessage.Text = Err.Description
            Finally

                'Destroy SQL Command Object
                cmdInseart = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

            Return bRetVal
        End Function

        Private Function Validation(ByRef sMsg As String) As Boolean
            sMsg = ""
            If IsNumeric(txtBnkOrgCode.Text) = False Then
                sMsg += "Bank Organization Code only accepts numeric value" & gc_BR
            End If
            'If Len(txtBnkOrgCode.Text) <> 5 Then
            '    sMsg += "Bank Organization Code must be not less than 5 digits" & gc_BR
            'End If
            If fncCheckDuplicate() Then
                sMsg += "[" & txtBnkOrgCode.Text & "] is exists in Database." & gc_BR
            End If

            If Len(sMsg) = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Function fncCheckDuplicate() As Boolean
            'txtBnkOrgCode.text
            Dim strSQL As String = "SELECT COUNT(0) as TotalCount FROM tcor_BankOrgAccounts WHERE BankOrgCode = " & clsDB.SQLStr(txtBnkOrgCode.Text)
            Dim iCount As Integer = 0
            Dim bRetVal As Boolean = False
            Dim oGeneric As New Generic
            Try
                iCount = SqlHelper.ExecuteScalar(oGeneric.strSQLConnection, CommandType.Text, strSQL)
                If iCount > 0 Then
                    bRetVal = True
                End If
            Catch ex As Exception
                LogError("pg_BankOrgCode - fncCheckDuplicate")
                oGeneric = Nothing
            End Try
            Return bRetVal
        End Function

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Dim dgItem As DataGridItem

            'Create Instance SQL Transaction
            Dim trnDelete As SqlTransaction

            'Create Instance of SQL Command Object
            Dim cmdDelete As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Intialise SQL Connection
            Call clsGeneric.SQLConnection_Initialize()

            'Begin Transaction 
            trnDelete = clsGeneric.SQLConnection.BeginTransaction()

            Try

                For Each dgItem In Me.dgBankOrgCode.Items
                    If dgItem.ItemType = ListItemType.AlternatingItem OrElse dgItem.ItemType = ListItemType.Item Then
                        If CType(dgItem.Cells(0).FindControl("chkSelect"), CheckBox).Checked Then
                            With cmdDelete
                                .Connection = clsGeneric.SQLConnection
                                .Transaction = trnDelete
                                .CommandType = CommandType.StoredProcedure
                                .CommandText = "pg_DeleteBankAccountOrgCode"
                                .Parameters.Clear()
                                .Parameters.Add(New SqlParameter("@in_OrgId", dgItem.Cells(1).Text.Trim))
                                .Parameters.Add(New SqlParameter("@in_AccNo", dgItem.Cells(2).Text.Trim))
                                .Parameters.Add(New SqlParameter("@in_OrgCode", dgItem.Cells(3).Text.Trim))
                                .ExecuteNonQuery()
                            End With
                        End If
                    End If
                Next


                'Commit Transaction
                trnDelete.Commit()

            Catch

                'Rollback Transaction
                trnDelete.Rollback()

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnOrganisation - clsCustomer", Err.Number, Err.Description)
                Me.lblMessage.Text = Err.Description

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()


                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdDelete = Nothing

                'Destroy Instance of SQL Transaction
                trnDelete = Nothing


            End Try

            BindGrid()

        End Sub

        Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click

            Response.Write("<script language=javascript>window.close()</script>")
        End Sub
    End Class
End Namespace