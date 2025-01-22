Imports Microsoft.VisualBasic
Imports System
Imports System.data
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll
    Public Class clsMandatesHistory
        Inherits Generic

#Region "Fields"
        Private _MandateID As Long = 0
        Private _OrgId As Long = 0
        Private _FileId As Integer = 0
        Private _FieldName As String = ""
        Private _OldValue As String = ""
        Private _NewValue As String = ""
        Private _ModifyBy As Integer = 0

        Property paramMandateID() As Long
            Get
                Return _MandateID
            End Get
            Set(ByVal value As Long)
                _MandateID = value
            End Set
        End Property
        Property paramOrgId() As Long
            Get
                Return _OrgId
            End Get
            Set(ByVal value As Long)
                _OrgId = value
            End Set
        End Property
        Property paramFileId() As Integer
            Get
                Return _FileId
            End Get
            Set(ByVal value As Integer)
                _FileId = value
            End Set
        End Property
        Property paramFieldName() As String
            Get
                Return _FieldName
            End Get
            Set(ByVal value As String)
                _FieldName = value
            End Set
        End Property
        Property paramOldValue() As String
            Get
                Return _OldValue
            End Get
            Set(ByVal value As String)
                _OldValue = value
            End Set
        End Property
        Property paramNewValue() As String
            Get
                Return _NewValue
            End Get
            Set(ByVal value As String)
                _NewValue = value
            End Set
        End Property
        Property paramModifyBy() As Integer
            Get
                Return _ModifyBy
            End Get
            Set(ByVal value As Integer)
                _ModifyBy = value
            End Set
        End Property

#End Region


#Region "Insert Data"
        Public Function Insert(ByVal oItem As clsMandatesHistory) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "pg_InstMandatesHistory"
            Dim params(6) As SqlParameter
            Try
                params(0) = New SqlParameter("@MandateID", oItem.paramMandateID)
                params(1) = New SqlParameter("@OrgId", oItem.paramOrgId)
                params(2) = New SqlParameter("@FileId", oItem.paramFileId)
                params(3) = New SqlParameter("@FieldName", oItem.paramFieldName)
                params(4) = New SqlParameter("@OldValue", oItem.paramOldValue)
                params(5) = New SqlParameter("@NewValue", oItem.paramNewValue)
                params(6) = New SqlParameter("@ModifyBy", oItem.paramModifyBy)
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Catch ex As Exception
                ErrorLog("clsMandatesHistory.Insert", Err.Number, ex.Message)
            End Try
            Return bRetVal
        End Function
#End Region

#Region "Update Data"
        'Public Function Update(ByVal oItem As clsMandatesHistory) As Boolean
        '    Dim bRetVal As Boolean = False
        '    Dim strSQL As String = "pg_UpdMandatesHistory"
        '    Dim params(7) As SqlParameter
        '    Try
        '        params(0) = New SqlParameter("@MandateID", oItem.MandateID)
        '        params(1) = New SqlParameter("@OrgId", oItem.OrgId)
        '        params(2) = New SqlParameter("@FileId", oItem.FileId)
        '        params(3) = New SqlParameter("@FieldName", oItem.FieldName)
        '        params(4) = New SqlParameter("@OldValue", oItem.OldValue)
        '        params(5) = New SqlParameter("@NewValue", oItem.NewValue)
        '        params(6) = New SqlParameter("@ModifyBy", oItem.ModifyBy)
        '        params(7) = New SqlParameter("@ModifyDatge", oItem.ModifyDatge)
        '        SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
        '    Catch ex As Exception
        '        ErrorLog("clsMandatesHistory.Update", Err.Number, ex.Message)
        '    End Try
        '    Return bRetVal
        'End Function
#End Region

    End Class
End Namespace

