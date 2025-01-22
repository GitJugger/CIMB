Imports Microsoft.VisualBasic
Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Namespace MaxPayroll
    Public Class clsBankCodeMapping
        Private _iBankID As Integer
        'Private _iOrgId As Integer
        Private _sCustomerBankCode As String
        Property iBankID() As Integer
            Get
                Return _iBankID
            End Get
            Set(ByVal value As Integer)
                _iBankID = value
            End Set
        End Property
        'Property iOrgId() As Integer
        '    Get
        '        Return _iOrgId
        '    End Get
        '    Set(ByVal value As Integer)
        '        _iOrgId = value
        '    End Set
        'End Property
        Property sCustomerBankCode() As String
            Get
                Return _sCustomerBankCode
            End Get
            Set(ByVal value As String)
                _sCustomerBankCode = value
            End Set
        End Property
        '071203 Victor
        Public Function fncGetBankCodeMapping(ByVal iOrgID As Integer) As DataSet
            Dim dsRetVal As New DataSet
            Dim param(0) As SqlParameter
            Dim clsGeneric As New Generic
            Try
                param(0) = New SqlParameter("@in_OrgID", iOrgID)
                dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_QryBankMapping", param)
            Catch ex As Exception
                Throw ex
            End Try
            Return dsRetVal
        End Function

        Public Function fncSaveBankCodeMapping(ByVal oItems As ArrayList, ByVal iOrgId As Integer) As Boolean
            Dim bRetVal As Boolean = False
            Dim oItem As clsBankCodeMapping
            Dim strSQL As String = ""
            Dim clsGeneric As New Generic
            fncDeleteBankCodeMapping(iOrgId)
            Try
                For Each oItem In oItems
                    strSQL += "INSERT INTO [tCor_BankMapping] (BankID, Org_Id, CustomerBankCode) values(" & oItem.iBankID & "," & iOrgId & "," & clsDB.SQLStr(oItem.sCustomerBankCode) & ");"
                Next
                SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
                bRetVal = True
            Catch

            Finally
                clsGeneric = Nothing
            End Try

            Return bRetVal
        End Function

        Public Function fncDeleteBankCodeMapping(ByVal iOrgId As Integer) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "pg_DelBankMapping"
            Dim clsGeneric As New Generic

            Try
                SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("@in_Org_Id", iOrgId))
                bRetVal = True
            Catch

            Finally
                clsGeneric = Nothing
            End Try

            Return bRetVal
        End Function
    End Class
End Namespace
