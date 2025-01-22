Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient

Namespace MaxPayroll
    Public Class clsFileSetting
        Public Shared Function fncCheckExistBankFileSetting(ByVal iBankId As Integer, ByVal iFileType As Integer, ByVal sAccNo As String) As Boolean
            Dim iRetVal As Integer
            Dim clsGeneric As New MaxPayroll.Generic
            Dim strSQL As String = "Select Count(0) From mCor_BankAccounts Inner Join mCor_PaymentService On mCor_BankAccounts.PaySer_Id = mCor_PaymentService.PaySer_Id Where mCor_BankAccounts.BankId = " & iBankId.ToString & " And mCor_BankAccounts.PaySer_Id = " & iFileType.ToString & " And mCor_BankAccounts.Account_No = '" & sAccNo & "'"
            iRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            If iRetVal > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function fncCheckExistBankFileSetting(ByVal iBankId As Integer, ByVal iFileType As Integer) As Boolean
            Dim iRetVal As Integer
            Dim clsGeneric As New MaxPayroll.Generic
            Dim strSQL As String = "Select Count(0) From mCor_BankFormat Inner Join mCor_PaymentService On FileType = PaySer_Desc Where BankId = " & iBankId.ToString & " And PaySer_Id = " & iFileType.ToString
            iRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            If iRetVal > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
       
        Public Function fncGetFileSetting(ByVal iOrgID As Integer, Optional ByVal iPaySerID As Integer = 0) As DataSet
            Dim dsRetVal As New DataSet
            Dim clsGeneric As New Generic
            Dim strSQL As String = "pg_QryCustomerFileType"
            Dim params(1) As SqlParameter
            params(0) = New SqlParameter("@in_OrgID", SqlDbType.Int)
            params(0).Value = iOrgID
            params(1) = New SqlParameter("@in_PaySer_Id", SqlDbType.Int)
            params(1).Value = iPaySerID
            dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Return dsRetVal
        End Function
    End Class
End Namespace
