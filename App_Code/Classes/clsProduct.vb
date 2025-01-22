Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Namespace MaxPayroll
    Public Class clsProduct

        Private _iProductID As Integer = 0
        Private _sProductName As String = ""
        Private _sProductLink As String = ""
        Private _sStatus As String = ""

        Public Property iProductID() As Integer
            Get
                Return _iProductID
            End Get
            Set(ByVal value As Integer)
                _iProductID = value
            End Set
        End Property

        Public Property sProductName() As String
            Get
                Return _sProductName
            End Get
            Set(ByVal value As String)
                _sProductName = value
            End Set
        End Property

        Public Property sProductLink() As String
            Get
                Return _sProductLink
            End Get
            Set(ByVal value As String)
                _sproductlink = value
            End Set
        End Property

        Public Property sStatus() As String
            Get
                Return _sStatus
            End Get
            Set(ByVal value As String)
                _sstatus = value
            End Set
        End Property

        Public Function fncRetrieveDataList(ByVal sProductName As String) As DataSet
            Dim dsRetVal As New DataSet
            Dim strSQL As String = ""
            Dim clsGeneric As New Generic

            Try
                strSQL = "Select ProductID, ProductName, Status FROM mCor_Product WHERE ProductName " & clsDB.SQLStr(sProductName, clsDB.SQLDataTypes.Dt_LikeAnyWhere)
                dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                clsGeneric.ErrorLog("clsProduct-fncRetrieveDataList", Err.Number, Err.Description)
            End Try
            Return dsRetVal
        End Function

        Public Function fncRetrieveData(ByVal iProductID As Integer) As clsProduct
            Dim oItem As New clsProduct
            Dim clsGeneric As New Generic
            Dim strSQL As String
            Dim drInfo As SqlDataReader

            Try
                strSQL = "Select ProductName, ProductLink, Status FROM mCor_Product WHERE ProductID = " & iProductID.ToString
                drInfo = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
                While drInfo.Read
                    oItem.sProductName = CStr(drInfo("ProductName"))
                    oItem.sProductLink = CStr(drInfo("ProductLink"))
                    oItem.sStatus = CStr(drInfo("Status"))
                    Exit While
                End While
            Catch ex As Exception
                clsGeneric.ErrorLog("clsProduct-fncRetrieveData: " & iProductID.ToString, Err.Number, Err.Description)
            Finally
                clsGeneric = Nothing
            End Try

            Return oItem
        End Function

        Public Function fncSaveData(ByVal oItem As clsProduct) As Boolean
            Dim bRetVal As Boolean = False
            Dim clsGeneric As New Generic
            Dim Params(2) As SqlParameter

            Try
                Params(0) = New SqlParameter("@ProductName", oItem.sProductName)
                Params(1) = New SqlParameter("@ProductLink", oItem.sProductLink)
                Params(2) = New SqlParameter("@Status", oItem.sStatus)
                SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_InstProduct", Params)
                bRetVal = True
            Catch ex As Exception
                clsGeneric.ErrorLog("clsProduct-fncSaveData: " & sProductName & " " & sProductLink & " " & sStatus, Err.Number, Err.Description)
            Finally
                clsGeneric = Nothing
            End Try

            Return bRetVal
        End Function

        Public Function fncUpdateData(ByVal oItem As clsProduct) As Boolean
            Dim bRetVal As Boolean = False
            Dim clsGeneric As New Generic
            Dim Params(3) As SqlParameter

            Try
                Params(0) = New SqlParameter("@ProductID", oItem.iProductID)
                Params(1) = New SqlParameter("@ProductName", oItem.sProductName)
                Params(2) = New SqlParameter("@ProductLink", oItem.sProductLink)
                Params(3) = New SqlParameter("@Status", oItem.sStatus)
                SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_UpdProduct", Params)
                bRetVal = True
            Catch ex As Exception
                clsGeneric.ErrorLog("clsProduct-fncUpdateData: " & iProductID.ToString & " " & sProductName & " " & sProductLink & " " & sStatus, Err.Number, Err.Description)
            Finally
                clsGeneric = Nothing
            End Try

            Return bRetVal
        End Function
    End Class
End Namespace