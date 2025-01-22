Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports system.Data
Imports Microsoft.ApplicationBlocks.Data
Imports System.Collections.Generic
Imports System.ComponentModel

Namespace MaxPayroll
    '<Serializable()> _
    Public Class clsMandates
        Inherits Generic

#Region "Fields "


        Enum enmInitialDataEntry
            Upload = 1
            KeyIn = 0
            None = Nothing
        End Enum

        'Use by header only - start
        Private _FileName As String = ""
        Private _TotalTransaction As Long = 0
        Property paramFileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal value As String)
                _FileName = value
            End Set
        End Property
        Property paramTotalTransaction() As Long
            Get
                Return _TotalTransaction
            End Get
            Set(ByVal value As Long)
                _TotalTransaction = value
            End Set
        End Property
        'Use by header only - End

        Private _ID As Integer = 0
        Private _FileID As Long = 0
        Private _RecID As Long = 0
        Private _OrgID As Long = 0
        Private _RefNo As String = ""
        Private _AccNo As String = ""
        Private _BankOrgCode As String = ""
        Private _CustomerName As String = ""
        Private _LimitAmount As Decimal = 0.0
        Private _Frequency As String = ""
        Private _ICNumber As String = ""
        Private _FrequencyLimit As Int16 = 0
        Private _DoneBy As Integer = 0
        Private _DoneDate As DateTime = DateTime.MinValue
        Private _InitDataEntry As enmInitialDataEntry = enmInitialDataEntry.None
        Private _ToUpdate As Boolean = False
        Private _MStatus As enmMandateStatus
        Private _Status As String = ""

        Property paramID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property
        Property paramFileID() As Long
            Get
                Return _FileID
            End Get
            Set(ByVal value As Long)
                _FileID = value
            End Set
        End Property
        Property paramRecID() As Long
            Get
                Return _RecID
            End Get
            Set(ByVal value As Long)
                _RecID = value
            End Set
        End Property
        Property paramOrgID() As Long
            Get
                Return _OrgID
            End Get
            Set(ByVal value As Long)
                _OrgID = value
            End Set
        End Property
        Property paramRefNo() As String
            Get
                Return _RefNo
            End Get
            Set(ByVal value As String)
                _RefNo = value
            End Set
        End Property
        Property paramAccNo() As String
            Get
                Return _AccNo
            End Get
            Set(ByVal value As String)
                _AccNo = value
            End Set
        End Property
        Property paramBankOrgCode() As String
            Get
                Return _BankOrgCode
            End Get
            Set(ByVal value As String)
                _BankOrgCode = value
            End Set
        End Property
        Property paramCustomerName() As String
            Get
                Return _CustomerName
            End Get
            Set(ByVal value As String)
                _CustomerName = value
            End Set
        End Property
        Property paramLimitAmount() As Decimal
            Get
                Return _LimitAmount
            End Get
            Set(ByVal value As Decimal)
                _LimitAmount = value
            End Set
        End Property
        ReadOnly Property paramLimitAmountDisplay() As String
            Get
                Return _LimitAmount.ToString("N2")
            End Get
        End Property
        Property paramFrequency() As String
            Get
                Return _Frequency
            End Get
            Set(ByVal value As String)
                _Frequency = value
            End Set
        End Property
        Property paramIC() As String
            Get
                Return _ICNumber
            End Get
            Set(ByVal value As String)
                _ICNumber = value
            End Set
        End Property
        'hafeez
        Property paramTheStatus() As String 'Status either Active = "A" or Inactive = "I"
            Get
                Return _Status
            End Get
            Set(ByVal value As String)
                _Status = value
            End Set
        End Property
        'end hafeez
        ReadOnly Property paramFrequencyDesc() As String
            Get
                Dim sRetVal As String = ""
                Select Case _Frequency
                    Case clsCommon.fncGetPrefix(enmFrequency.DY_Daily)
                        sRetVal = clsCommon.fncGetPostFix(enmFrequency.DY_Daily)
                    Case clsCommon.fncGetPrefix(enmFrequency.WY_Weekly)
                        sRetVal = clsCommon.fncGetPostFix(enmFrequency.WY_Weekly)
                    Case clsCommon.fncGetPrefix(enmFrequency.MY_Monthly)
                        sRetVal = clsCommon.fncGetPostFix(enmFrequency.MY_Monthly)
                    Case clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly)
                        sRetVal = clsCommon.fncGetPostFix(enmFrequency.QY_Quarterly)
                    Case clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly)
                        sRetVal = clsCommon.fncGetPostFix(enmFrequency.HY_Half_Yearly)
                    Case clsCommon.fncGetPrefix(enmFrequency.YY_Yearly)
                        sRetVal = clsCommon.fncGetPostFix(enmFrequency.YY_Yearly)
                End Select
                Return sRetVal
            End Get
        End Property
        Property paramFrequencyLimit() As Int16
            Get
                Return _FrequencyLimit
            End Get
            Set(ByVal value As Int16)
                _FrequencyLimit = value
            End Set
        End Property
        Property paramDoneBy() As Integer
            Get
                Return _DoneBy
            End Get
            Set(ByVal value As Integer)
                _DoneBy = value
            End Set
        End Property
        Property paramDoneDate() As DateTime
            Get
                Return _DoneDate
            End Get
            Set(ByVal value As DateTime)
                _DoneDate = value
            End Set
        End Property
        Property paramInitDataEntry() As enmInitialDataEntry
            Get
                Return _InitDataEntry
            End Get
            Set(ByVal value As enmInitialDataEntry)
                _InitDataEntry = value
            End Set
        End Property
        Property paramToUpdate() As Boolean
            Get
                Return _ToUpdate
            End Get
            Set(ByVal value As Boolean)
                _ToUpdate = value
            End Set
        End Property
        Property paramStatus() As enmMandateStatus
            Get
                Return _MStatus
            End Get
            Set(ByVal value As enmMandateStatus)
                _MStatus = value
            End Set
        End Property
#End Region

#Region "Retrieve Data "
        Public Sub BindDDLFrequency(ByRef ddlTemp As DropDownList)
            ddlTemp.DataSource = LoadFrequency()
            ddlTemp.DataTextField = "Key"
            ddlTemp.DataValueField = "Value"
            ddlTemp.DataBind()
            ddlTemp.Items.Insert(0, New ListItem("Select", ""))
        End Sub

        Private Function LoadFrequency() As Hashtable
            Dim htRetVal As New Hashtable
            With htRetVal
                .Add(clsCommon.fncGetPostFix(enmFrequency.UN_Unlimited), clsCommon.fncGetPrefix(enmFrequency.UN_Unlimited))
                .Add(clsCommon.fncGetPostFix(enmFrequency.MY_Monthly), clsCommon.fncGetPrefix(enmFrequency.MY_Monthly))
                .Add(clsCommon.fncGetPostFix(enmFrequency.QY_Quarterly), clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly))
                .Add(clsCommon.fncGetPostFix(enmFrequency.HY_Half_Yearly), clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly))
                .Add(clsCommon.fncGetPostFix(enmFrequency.YY_Yearly), clsCommon.fncGetPrefix(enmFrequency.YY_Yearly))
            End With
            Return htRetVal
        End Function

        Public Function LoadList() As DataSet
            Dim strSQL As String = "pg_QryMandateList"
            Dim oItems As New List(Of clsMandates)
            Dim ds As New DataSet
            Dim Params(4) As SqlParameter
            Try

                Params(0) = New SqlParameter("@OrgID", paramOrgID)
                Params(1) = New SqlParameter("@RefNo", paramRefNo)
                Params(2) = New SqlParameter("@AccNo", paramAccNo)
                Params(3) = New SqlParameter("@BankOrgCode", paramBankOrgCode)
                Params(4) = New SqlParameter("@CustomerName", paramCustomerName)

                ds = SqlHelper.ExecuteDataset(sSQLConnection, CommandType.StoredProcedure, strSQL, Params)
            Catch ex As Exception
                Throw ex
            End Try
            Return ds
        End Function

        Public Function LoadFileHeader(ByVal lngFileId As Long) As clsMandates
            Dim oRetVal As New clsMandates
            Dim strSQL As String = "pg_QryMandateHeaderByFileID"
            Dim drInfo As SqlDataReader
            Try

                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("@FileId", lngFileId))
                If drInfo.HasRows Then
                    While drInfo.Read
                        oRetVal.paramCustomerName = CStr(drInfo.Item("User_Login"))
                        oRetVal.paramFileName = CStr(drInfo.Item("FileGivenName"))
                        oRetVal.paramDoneDate = CDate(drInfo.Item("FileDtTm"))
                        oRetVal.paramTotalTransaction = CLng(drInfo.Item("TotalRecLines"))
                    End While
                    drInfo.Close()
                    drInfo = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return oRetVal
        End Function

        Public Function LoadFileHeader(ByVal lngOrgId As Long, ByVal lngFileId As Long) As clsMandates
            Dim oRetVal As New clsMandates
            Dim params(1) As SqlParameter
            Dim strSQL As String = "pg_QryMandateHeader"
            Dim drInfo As SqlDataReader
            Try
                params(0) = New SqlParameter("@OrgId", lngOrgId)
                params(1) = New SqlParameter("@FileId", lngFileId)
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
                If drInfo.HasRows Then
                    While drInfo.Read
                        oRetVal.paramCustomerName = CStr(drInfo.Item("User_Login"))
                        oRetVal.paramFileName = CStr(drInfo.Item("FileGivenName"))
                        oRetVal.paramDoneDate = CDate(drInfo.Item("FileDtTm"))
                        oRetVal.paramTotalTransaction = CLng(drInfo.Item("TotalRecLines"))
                    End While
                    drInfo.Close()
                    drInfo = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return oRetVal
        End Function

        Public Function LoadFileDetailsCollection(ByVal lngFileId As Long) As List(Of clsMandates)
            Dim oRetVal As New List(Of clsMandates)
            Dim oItem As New clsMandates
            Dim strSQL As String = "pg_QryMandateDetailsByFileID"
            Dim drInfo As SqlDataReader

            Try
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("@FileId", lngFileId))
                If drInfo.HasRows Then
                    While drInfo.Read
                        oItem = New clsMandates
                        With oItem
                            .paramOrgID = drInfo.Item("OrgID")
                            .paramRefNo = drInfo.Item("RefNo")
                            .paramAccNo = drInfo.Item("AccNo")
                            .paramBankOrgCode = drInfo.Item("BankOrgCode")
                            .paramCustomerName = drInfo.Item("CustomerName")
                            .paramLimitAmount = drInfo.Item("LimitAmount")
                            .paramFrequency = drInfo.Item("Frequency")
                            .paramFrequencyLimit = drInfo.Item("FrequencyLimit")
                            .paramToUpdate = drInfo.Item("ToUpdate")
                            .paramFileID = drInfo.Item("FileId")
                            .paramTheStatus = drInfo.Item("Active") 'Added by Naresh to update the status 17-03-11
                        End With
                        oRetVal.Add(oItem)

                    End While
                    drInfo.Close()
                    drInfo = Nothing
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return oRetVal
        End Function

        Public Function LoadFileDetails(ByVal lngFileId As Long) As DataSet
            Dim dsRetVal As New DataSet
            Dim strSQL As String = "pg_QryMandateDetailsByFileID"

            Try
                dsRetVal = SqlHelper.ExecuteDataset(sSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("@FileId", lngFileId))
            Catch ex As Exception
                Throw ex
            End Try

            Return dsRetVal
        End Function

        Public Function LoadFileDetails(ByVal lngOrgId As Long, ByVal lngFileId As Long) As DataSet
            Dim dsRetVal As New DataSet
            Dim params(1) As SqlParameter
            Dim strSQL As String = "pg_QryMandateDetails"

            Try
                params(0) = New SqlParameter("@OrgId", lngOrgId)
                params(1) = New SqlParameter("@FileId", lngFileId)

                dsRetVal = SqlHelper.ExecuteDataset(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Catch ex As Exception
                Throw ex
            End Try

            Return dsRetVal
        End Function

        Public Function Load(ByVal lngOrgId As Long) As List(Of clsMandates)
            Dim strSQL As String = "pg_QryMandatesDetails"
            Dim oItems As New List(Of clsMandates)
            Dim oItem As clsMandates
            Dim drInfo As SqlDataReader
            Try
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("@OrgId", lngOrgId))
                If drInfo.HasRows Then
                    While drInfo.Read
                        oItem = New clsMandates
                        oItem.paramOrgID = CLng(drInfo.Item("OrgId"))
                        oItem.paramRefNo = CStr(drInfo.Item("RefNo"))
                        oItem.paramAccNo = CStr(drInfo.Item("AccNo"))
                        oItem.paramBankOrgCode = CStr(drInfo.Item("BankOrgCode"))
                        oItems.Add(oItem)
                        oItem = Nothing
                    End While
                    drInfo.Close()
                End If
                drInfo = Nothing
            Catch ex As Exception
                ErrorLog("clsMandates.Load", Err.Number, ex.Message)
                oItems = New List(Of clsMandates)
            End Try
            Return oItems
        End Function

        Public Function Load(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String) As clsMandates
            Dim oItem As New clsMandates
            Dim strSQL As String = "pg_QryMandatesDetails"
            Dim params(3) As SqlParameter
            Dim drInfo As SqlDataReader

            Try
                params(0) = New SqlParameter("@OrgId", lngOrgId)
                params(1) = New SqlParameter("@RefNo", sRefNo)
                params(2) = New SqlParameter("@AccNo", sAccNo)
                params(3) = New SqlParameter("@BankOrgCode", sBankOrgCode)
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
                If drInfo.HasRows Then
                    While drInfo.Read
                        oItem = New clsMandates
                        oItem.paramRecID = CLng(drInfo.Item("ID"))
                        oItem.paramOrgID = CLng(drInfo.Item("OrgId"))
                        oItem.paramRefNo = CStr(drInfo.Item("RefNo"))
                        oItem.paramAccNo = CStr(drInfo.Item("AccNo"))
                        oItem.paramBankOrgCode = CStr(drInfo.Item("BankOrgCode"))
                        oItem.paramCustomerName = CStr(drInfo.Item("CustomerName"))
                        oItem.paramLimitAmount = CDec(drInfo.Item("LimitAmount"))
                        oItem.paramFrequency = CStr(drInfo.Item("Frequency"))
                        oItem.paramFrequencyLimit = CInt(drInfo.Item("FrequencyLimit"))
                        oItem.paramStatus = CInt(drInfo.Item("ApprovalStatus"))
                        oItem.paramTheStatus = CStr(drInfo.Item("Status"))
                        Exit While
                    End While
                    drInfo.Close()
                End If
                drInfo = Nothing

            Catch ex As Exception
                ErrorLog("clsMandates.Load", Err.Number, ex.Message)
                oItem = New clsMandates
            End Try
            Return oItem
        End Function
        'hafeez start
        Public Function DeleteMandateFileDetails(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String) As clsMandates
            Dim oItem As New clsMandates
            Dim strSQL As String = "pg_DelMandatesFileDetails"
            Dim params(3) As SqlParameter
            Dim drInfo As SqlDataReader

            Try
                params(0) = New SqlParameter("@OrgId", lngOrgId)
                params(1) = New SqlParameter("@RefNo", sRefNo)
                params(2) = New SqlParameter("@AccNo", sAccNo)
                params(3) = New SqlParameter("@BankOrgCode", sBankOrgCode)
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, params)

                drInfo = Nothing

            Catch ex As Exception
                ErrorLog("clsMandates.Delete", Err.Number, ex.Message)
                oItem = New clsMandates
            End Try
            Return oItem
        End Function
        'hafeez end

        Public Function LoadTemp(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String) As clsMandates
            Dim oItem As New clsMandates
            Dim strSQL As String = "pg_QryTempMandatesDetails"
            Dim params(3) As SqlParameter
            Dim drInfo As SqlDataReader

            Try
                params(0) = New SqlParameter("@OrgId", lngOrgId)
                params(1) = New SqlParameter("@RefNo", sRefNo)
                params(2) = New SqlParameter("@AccNo", sAccNo)
                params(3) = New SqlParameter("@BankOrgCode", sBankOrgCode)
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
                If drInfo.HasRows Then
                    While drInfo.Read
                        oItem = New clsMandates
                        oItem.paramRecID = CLng(drInfo.Item("ID"))
                        oItem.paramOrgID = CLng(drInfo.Item("OrgId"))
                        oItem.paramRefNo = CStr(drInfo.Item("RefNo"))
                        oItem.paramAccNo = CStr(drInfo.Item("AccNo"))
                        oItem.paramBankOrgCode = CStr(drInfo.Item("BankOrgCode"))
                        oItem.paramCustomerName = CStr(drInfo.Item("CustomerName"))
                        oItem.paramLimitAmount = CDec(drInfo.Item("LimitAmount"))
                        oItem.paramFrequency = CStr(drInfo.Item("Frequency"))
                        oItem.paramFrequencyLimit = CInt(drInfo.Item("FrequencyLimit"))
                        oItem.paramTheStatus = CStr(drInfo.Item("Status"))
                        Exit While
                    End While
                    drInfo.Close()
                End If
                drInfo = Nothing

            Catch ex As Exception
                ErrorLog("clsMandates.Load", Err.Number, ex.Message)
                oItem = New clsMandates
            End Try
            Return oItem
        End Function

        Public Function LoadById(ByVal lngRecID As Long) As clsMandates
            Dim oItem As New clsMandates
            Dim strSQL As String = "pg_QryMandatesDetailsById"

            Dim drInfo As SqlDataReader

            Try
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("ID", lngRecID))
                If drInfo.HasRows Then
                    While drInfo.Read
                        oItem = New clsMandates
                        oItem.paramRecID = CLng(drInfo.Item("ID"))
                        oItem.paramOrgID = CLng(drInfo.Item("OrgId"))
                        oItem.paramRefNo = CStr(drInfo.Item("RefNo"))
                        oItem.paramAccNo = CStr(drInfo.Item("AccNo"))
                        oItem.paramBankOrgCode = CStr(drInfo.Item("BankOrgCode"))
                        oItem.paramCustomerName = CStr(drInfo.Item("CustomerName"))
                        oItem.paramLimitAmount = CDec(drInfo.Item("LimitAmount"))
                        oItem.paramFrequency = CStr(drInfo.Item("Frequency"))
                        oItem.paramFrequencyLimit = CInt(drInfo.Item("FrequencyLimit"))
                        Exit While
                    End While
                    drInfo.Close()
                End If
                drInfo = Nothing

            Catch ex As Exception
                ErrorLog("clsMandates.Load", Err.Number, ex.Message)
                oItem = New clsMandates
            End Try
            Return oItem
        End Function

        Public Function LoadTempById(ByVal lngRecID As Long) As clsMandates
            Dim oItem As New clsMandates
            'SELECT * FROM tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & " AND RefNo = " & clsDB.SQLStr(strRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(strAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(strBankOrgCode, clsDB.SQLDataTypes.Dt_String)'
            Dim strSQL As String = "pg_QryTempMandatesDetailsById"

            Dim drInfo As SqlDataReader

            Try

                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.StoredProcedure, strSQL, New SqlParameter("ID", lngRecID))
                If drInfo.HasRows Then
                    While drInfo.Read
                        oItem = New clsMandates
                        oItem.paramRecID = CLng(drInfo.Item("ID"))
                        oItem.paramOrgID = CLng(drInfo.Item("OrgId"))
                        oItem.paramRefNo = CStr(drInfo.Item("RefNo"))
                        oItem.paramAccNo = CStr(drInfo.Item("AccNo"))
                        oItem.paramBankOrgCode = CStr(drInfo.Item("BankOrgCode"))
                        oItem.paramCustomerName = CStr(drInfo.Item("CustomerName"))
                        oItem.paramLimitAmount = CDec(drInfo.Item("LimitAmount"))
                        oItem.paramFrequency = CStr(drInfo.Item("Frequency"))
                        oItem.paramFrequencyLimit = CInt(drInfo.Item("FrequencyLimit"))
                        oItem.paramTheStatus = CStr(drInfo.Item("Status"))
                        Exit While
                    End While
                    drInfo.Close()
                End If
                drInfo = Nothing

            Catch ex As Exception
                ErrorLog("clsMandates.Load", Err.Number, ex.Message)
                oItem = New clsMandates
            End Try
            Return oItem
        End Function
#End Region

#Region "Insert Data"
        Public Function GetInsertTempString(ByVal oItem As clsMandates, ByVal bUpdate As Boolean) As String
            Dim sRetVal As String = ""
            Try
                sRetVal = "INSERT INTO tCor_MandatesFileDetails (OrgId,RefNo,AccNo, BankOrgCode,FileId, " & _
                    " CustomerName,LimitAmount,Frequency,FrequencyLimit,CreateBy, ToUpdate, Cust_ICNumber) Values (" & _
                        clsDB.SQLStr(oItem.paramOrgID, clsDB.SQLDataTypes.Dt_Integer) & _
                        "," & clsDB.SQLStr(oItem.paramRefNo) & _
                        "," & clsDB.SQLStr(oItem.paramAccNo) & _
                        "," & clsDB.SQLStr(oItem.paramBankOrgCode) & _
                        "," & clsDB.SQLStr(oItem.paramFileID, clsDB.SQLDataTypes.Dt_Integer) & _
                        "," & clsDB.SQLStr(oItem.paramCustomerName) & _
                        "," & clsDB.SQLStr(oItem.paramLimitAmount, clsDB.SQLDataTypes.Dt_Double) & _
                        "," & clsDB.SQLStr(oItem.paramFrequency) & _
                        "," & clsDB.SQLStr(oItem.paramFrequencyLimit, clsDB.SQLDataTypes.Dt_Integer) & _
                        "," & clsDB.SQLStr(oItem.paramDoneBy, clsDB.SQLDataTypes.Dt_Integer) & _
                        "," & IIf(bUpdate, "1", "0") & _
                        "," & clsDB.SQLStr(oItem.paramIC) & ");" & vbCrLf
                If bUpdate Then
                    sRetVal += "UPDATE tCor_MandatesFileDetails SET ApprovalStatus = " & enmMandateStatus.Pending & " WHERE OrgId=" & clsDB.SQLStr(oItem.paramOrgID, clsDB.SQLDataTypes.Dt_Integer) & " AND RefNo=" & clsDB.SQLStr(oItem.paramRefNo) & " AND AccNo=" & clsDB.SQLStr(oItem.paramAccNo) & " AND BankOrgCode=" & clsDB.SQLStr(oItem.paramBankOrgCode) & ";" & vbCrLf
                End If
            Catch ex As Exception
                ErrorLog("clsMandates.GetInsertTempString", Err.Number, ex.Message)
                sRetVal = ""
            End Try
            Return sRetVal
        End Function
        
        Public Function GetInsertString(ByVal oItem As clsMandates) As String
            Dim sRetVal As String = ""
            Try
                sRetVal = "INSERT INTO [tCor_MandatesDetails] ([OrgId], [RefNo],[AccNo]," & _
                    " [BankOrgCode], [FileId], [CustomerName], [LimitAmount], [Frequency], " & _
                        "[FrequencyLimit],[CreateBy]) Values (" & _
                                          clsDB.SQLStr(oItem.paramOrgID, clsDB.SQLDataTypes.Dt_Integer) & _
                                    "," & clsDB.SQLStr(oItem.paramRefNo) & _
                                    "," & clsDB.SQLStr(oItem.paramAccNo) & _
                                    "," & clsDB.SQLStr(oItem.paramBankOrgCode) & _
                                    "," & clsDB.SQLStr(oItem.paramFileID, clsDB.SQLDataTypes.Dt_Integer) & _
                                    "," & clsDB.SQLStr(oItem.paramCustomerName) & _
                                    "," & clsDB.SQLStr(oItem.paramLimitAmount, clsDB.SQLDataTypes.Dt_Double) & _
                                    "," & clsDB.SQLStr(oItem.paramFrequency) & _
                                    "," & clsDB.SQLStr(oItem.paramFrequencyLimit, clsDB.SQLDataTypes.Dt_Integer) & _
                                    "," & clsDB.SQLStr(oItem.paramDoneBy, clsDB.SQLDataTypes.Dt_Integer) & ");" & vbCrLf
            Catch ex As Exception
                ErrorLog("clsMandates.Insert", Err.Number, ex.Message)
                sRetVal = ""
            End Try
            Return sRetVal
        End Function
        Public Function Insert(ByVal oItem As clsMandates) As Long
            Dim lngRetVal As Long = 0
            Dim strSQL As String = "pg_InstMandatesDetails"
            Dim params(9) As SqlParameter

            Try
                params(0) = New SqlParameter("@OrgId", oItem.paramOrgID)
                params(1) = New SqlParameter("@RefNo", oItem.paramRefNo)
                params(2) = New SqlParameter("@AccNo", oItem.paramAccNo)
                params(3) = New SqlParameter("@BankOrgCode", oItem.paramBankOrgCode)
                params(4) = New SqlParameter("@CustomerName", oItem.paramCustomerName)
                params(5) = New SqlParameter("@LimitAmount", oItem.paramLimitAmount)
                params(6) = New SqlParameter("@Frequency", oItem.paramFrequency)
                params(7) = New SqlParameter("@FrequencyLimit", oItem.paramFrequencyLimit)
                params(8) = New SqlParameter("@DoneBy", oItem.paramDoneBy)
                params(9) = New SqlParameter("@FileId", oItem.paramFileID)

                lngRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Catch ex As Exception
                ErrorLog("clsMandates.Insert", Err.Number, ex.Message)
                lngRetVal = 0
            End Try
            Return lngRetVal
        End Function
        'tCor_TempMandatesDetails

        Public Function InsertTemp(ByVal oItem As clsMandates) As Long
            Dim lngRetVal As Long = 0
            Dim strSQL As String = "pg_InstTempMandatesDetails"
            Dim params(9) As SqlParameter
            Try
                params(0) = New SqlParameter("@OrgId", oItem.paramOrgID)
                params(1) = New SqlParameter("@RefNo", oItem.paramRefNo)
                params(2) = New SqlParameter("@AccNo", oItem.paramAccNo)
                params(3) = New SqlParameter("@BankOrgCode", oItem.paramBankOrgCode)
                params(4) = New SqlParameter("@CustomerName", oItem.paramCustomerName)
                params(5) = New SqlParameter("@LimitAmount", oItem.paramLimitAmount)
                params(6) = New SqlParameter("@Frequency", oItem.paramFrequency)
                params(7) = New SqlParameter("@FrequencyLimit", oItem.paramFrequencyLimit)
                params(8) = New SqlParameter("@DoneBy", oItem.paramDoneBy)
                params(9) = New SqlParameter("@Status", oItem.paramTheStatus) ' hafeez


                lngRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Catch ex As Exception
                ErrorLog("clsMandates.InsertTemp", Err.Number, ex.Message)
                lngRetVal = 0
            End Try
            Return lngRetVal
        End Function

        

        
#End Region

#Region "Delete"
        Public Function DeleteTemp(ByVal lngRecID As Long) As Boolean
            Dim strSQL As String = "DELETE tCor_TempMandatesDetails WHERE Id = " & lngRecID.ToString
            Dim bRetVal As Boolean = False
            Try
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQL)
                bRetVal = True
            Catch ex As Exception
                Throw ex
            End Try
            Return bRetVal '
        End Function
#End Region

#Region "Update Data"
       
        Public Function GetUpdateString(ByVal oItem As clsMandates) As String
            Dim sRetVal As String = ""
            Try
                sRetVal = "UPDATE [tCor_MandatesDetails]" & _
                " SET " & _
                " [AccNo] = " & clsDB.SQLStr(oItem.paramAccNo) & _
                ", [BankOrgCode] = " & clsDB.SQLStr(oItem.paramBankOrgCode) & _
                ", [CustomerName] = " & clsDB.SQLStr(oItem.paramCustomerName) & _
                ", [LimitAmount] = " & clsDB.SQLStr(oItem.paramLimitAmount, clsDB.SQLDataTypes.Dt_Double) & _
                ", [Frequency] = " & clsDB.SQLStr(oItem.paramFrequency) & _
                ", [FrequencyLimit] = " & clsDB.SQLStr(oItem.paramFrequencyLimit, clsDB.SQLDataTypes.Dt_Integer) & _
                ", [ModifyBy] = " & clsDB.SQLStr(oItem.paramDoneBy, clsDB.SQLDataTypes.Dt_Integer) & ", [ModifyDate] = GetDate()" & _
                " WHERE ID = " & clsDB.SQLStr(oItem.paramRecID, clsDB.SQLDataTypes.Dt_Integer) & " AND [OrgId] = " & clsDB.SQLStr(oItem.paramOrgID, clsDB.SQLDataTypes.Dt_Integer) & " AND [RefNo] = " & clsDB.SQLStr(oItem.paramRefNo) & ";" & vbCrLf
            Catch ex As Exception
                ErrorLog("clsMandates.Update", Err.Number, ex.Message)
                sRetVal = ""
            End Try
            Return sRetVal
        End Function

        Public Function UpdateStatusByFileId(ByVal FileId As Integer, ByVal DoneBy As Integer, ByVal eStatus As enmMandateStatus, ByVal bIsTemp As Boolean) As Boolean

            Dim bRetVal As Boolean = False, strSQL As String = Nothing
            'Dim strSQL As String = "UPDATE " & IIf(bIsTemp = False, "tCor_MandatesDetails", "tCor_MandatesFileDetails") & " SET ApprovedBy = " & DoneBy.ToString & ", ApprovalStatus = " & eStatus & " WHERE FileID = " & FileId.ToString

            'Modified by Naresh on 15-03-11 for deleting the related records from temp table
            If bIsTemp Then
                strSQL = "DELETE FROM tCor_MandatesFileDetails WHERE FileID = " & FileId.ToString
            Else
                strSQL = "UPDATE tCor_MandatesDetails SET ApprovedBy = " & DoneBy.ToString & ", ApprovalStatus = " & eStatus & " WHERE FileID = " & FileId.ToString
            End If
            Try
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQL)
                bRetVal = True
            Catch ex As Exception
                ErrorLog("clsMandates.UpdateStatus", Err.Number, ex.Message)
                bRetVal = False
            End Try
            Return bRetVal
        End Function



        Public Function UpdateModifyStatusById(ByVal Id As Integer, ByVal DoneBy As Integer) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "UPDATE tCor_MandatesDetails SET ModifyBy = " & DoneBy.ToString & ", ApprovalStatus = " & enmMandateStatus.Pending & ", ModifyDate = GETDATE() WHERE ID = " & Id.ToString

            Try
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQL)
                bRetVal = True
            Catch ex As Exception
                ErrorLog("clsMandates.UpdateModifyStatusById", Err.Number, ex.Message)
                bRetVal = False
            End Try
            Return bRetVal
        End Function

        Public Function UpdateStatusById(ByVal Id As Integer, ByVal DoneBy As Integer, ByVal eStatus As enmMandateStatus) As Boolean
            Dim bRetVal As Boolean = False
            Dim bRetValHistory As Boolean = False
            Dim strSQLHistory As String = Nothing
            Dim strSQL As String = "UPDATE tCor_MandatesDetails SET ApprovedBy = " & DoneBy.ToString & ", ApprovalStatus = " & eStatus & ", ApprovedDate = GETDATE() WHERE ID = " & Id.ToString
            If eStatus = enmMandateStatus.Approve Then
                strSQLHistory = "UPDATE tCor_MandatesHistory SET Approved = 'A' WHERE MandateID = " & Id.ToString
            ElseIf eStatus = enmMandateStatus.Reject Then
                strSQLHistory = "UPDATE tCor_MandatesHistory SET Approved = 'R' WHERE MandateID = " & Id.ToString
            End If
            Try
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQL)
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQLHistory)
                bRetVal = True
            Catch ex As Exception
                ErrorLog("clsMandates.UpdateStatusById", Err.Number, ex.Message)
                bRetVal = False
            End Try
            Return bRetVal
        End Function

        Public Function Update(ByVal oItem As clsMandates, ByVal OldItem As clsMandates) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "pg_UpdMandatesDetails"
            Dim params(10) As SqlParameter
            If fncCompareAndSaveHistory(oItem, OldItem) Then
                Try
                    params(0) = New SqlParameter("@RecId", OldItem.paramRecID)
                    params(1) = New SqlParameter("@OrgId", OldItem.paramOrgID)
                    params(2) = New SqlParameter("@RefNo", OldItem.paramRefNo)
                    params(3) = New SqlParameter("@AccNo", OldItem.paramAccNo)
                    params(4) = New SqlParameter("@BankOrgCode", OldItem.paramBankOrgCode)
                    params(5) = New SqlParameter("@CustomerName", oItem.paramCustomerName)
                    params(6) = New SqlParameter("@LimitAmount", oItem.paramLimitAmount)
                    params(7) = New SqlParameter("@Frequency", oItem.paramFrequency)
                    params(8) = New SqlParameter("@FrequencyLimit", oItem.paramFrequencyLimit)
                    params(9) = New SqlParameter("@DoneBy", oItem.paramDoneBy)
                    params(10) = New SqlParameter("@Status", oItem.paramTheStatus)

                    SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
                    bRetVal = True
                Catch ex As Exception
                    ErrorLog("clsMandates.Update", Err.Number, ex.Message)
                    bRetVal = False
                End Try
            End If
            Return bRetVal
        End Function

        Public Function UploadComplate(ByVal lngFileId As Long, ByVal TotRecord As Integer) As Boolean
            Dim strSQL As String = "UPDATE tPgt_FileDetails SET PaymentDate = GETDATE(), TotalTrans = " & TotRecord & ", TotalRecLines = " & TotRecord.ToString & ", TotalFileLines = " & TotRecord.ToString & " WHERE FileId = " & lngFileId.ToString
            Dim bRetVal As Boolean = False

            Try
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQL)
                bRetVal = True
            Catch ex As Exception
                ErrorLog("clsMandates.UploadComplate", Err.Number, ex.Message)
                bRetVal = False
            End Try

            Return bRetVal

        End Function

        Public Function Approve(ByVal oItem As clsMandates) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "UPDATE tCor_MandatesDetails SET ApprovalStatus = 1, ApprovedBy = " & oItem.paramDoneBy.ToString & ", ApprovedDate = GetDate() WHERE ID = " & oItem.paramRecID.ToString

            Try
                SqlHelper.ExecuteNonQuery(sSQLConnection, CommandType.Text, strSQL)
                bRetVal = True
            Catch ex As Exception
                ErrorLog("clsMandates.Approve", Err.Number, ex.Message)
                bRetVal = False
            End Try

            Return bRetVal
        End Function
#End Region

#Region "Compare Data"
        Private Function fncCompareAndSaveHistory(ByVal NewItem As clsMandates, ByVal OldItem As clsMandates) As Boolean
            Dim oHistoryItem As New clsMandatesHistory
            Dim bRetVal As Boolean = False
            Try
                oHistoryItem.paramMandateID = OldItem.paramRecID
                oHistoryItem.paramOrgId = OldItem.paramOrgID
                oHistoryItem.paramModifyBy = NewItem.paramDoneBy
                oHistoryItem.paramFileId = NewItem.paramFileID

                If NewItem.paramRefNo <> OldItem.paramRefNo Then
                    oHistoryItem.paramFieldName = "Reference No."
                    oHistoryItem.paramOldValue = OldItem.paramRefNo.ToString
                    oHistoryItem.paramNewValue = NewItem.paramRefNo.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                If NewItem.paramBankOrgCode <> OldItem.paramBankOrgCode Then
                    oHistoryItem.paramFieldName = "Bank Org. Code"
                    oHistoryItem.paramOldValue = OldItem.paramBankOrgCode.ToString
                    oHistoryItem.paramNewValue = NewItem.paramBankOrgCode.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                If NewItem.paramAccNo <> OldItem.paramAccNo Then
                    oHistoryItem.paramFieldName = "Account No."
                    oHistoryItem.paramOldValue = OldItem.paramAccNo.ToString
                    oHistoryItem.paramNewValue = NewItem.paramAccNo.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                If NewItem.paramCustomerName <> OldItem.paramCustomerName Then
                    oHistoryItem.paramFieldName = "Customer Name"
                    oHistoryItem.paramOldValue = OldItem.paramCustomerName.ToString
                    oHistoryItem.paramNewValue = NewItem.paramCustomerName.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                If NewItem.paramLimitAmount <> OldItem.paramLimitAmount Then
                    oHistoryItem.paramFieldName = "Limit Amount"
                    oHistoryItem.paramOldValue = OldItem.paramLimitAmount.ToString
                    oHistoryItem.paramNewValue = NewItem.paramLimitAmount.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                If NewItem.paramFrequency <> OldItem.paramFrequency Then
                    oHistoryItem.paramFieldName = "Frequency"
                    oHistoryItem.paramOldValue = OldItem.paramFrequency.ToString
                    oHistoryItem.paramNewValue = NewItem.paramFrequency.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                If NewItem.paramFrequencyLimit <> OldItem.paramFrequencyLimit Then
                    oHistoryItem.paramFieldName = "Frequency Limit"
                    oHistoryItem.paramOldValue = OldItem.paramFrequencyLimit.ToString
                    oHistoryItem.paramNewValue = NewItem.paramFrequencyLimit.ToString
                    oHistoryItem.Insert(oHistoryItem)
                End If
                bRetVal = True
            Catch ex As Exception
                ErrorLog("clsMandates.fncCompare", Err.Number, ex.Message)
                bRetVal = False
            End Try
            Return bRetVal
        End Function
#End Region

#Region "Parse Data"
        Public Function fncGetFrequencyDesc(ByVal sValue As String) As String
            Dim sRetVal As String = ""
            Select Case sValue
                Case clsCommon.fncGetPrefix(enmFrequency.DY_Daily)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.DY_Daily)
                Case clsCommon.fncGetPrefix(enmFrequency.WY_Weekly)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.WY_Weekly)
                Case clsCommon.fncGetPrefix(enmFrequency.MY_Monthly)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.MY_Monthly)
                Case clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.QY_Quarterly)
                Case clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.HY_Half_Yearly)
                Case clsCommon.fncGetPrefix(enmFrequency.YY_Yearly)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.YY_Yearly)
                Case clsCommon.fncGetPrefix(enmFrequency.UN_Unlimited)
                    sRetVal = clsCommon.fncGetPostFix(enmFrequency.UN_Unlimited)
            End Select
            Return sRetVal
        End Function
#End Region

#Region "Validation"
        '### Validation Rules - Start
        '1) Primary key will be combination of Org ID, Ref. No., Acc No. and Bank Org Code.
        '2) Bank Org Code should be unique but can be reuse in 1 organization.
        '   i.e. if 1234 is belongs to Org A, means it cannot be used in other Organization.
        '### Validation Rules - End
        Public Function IsDuplicateBankOrgCode(ByVal lngOrgId As Long, ByVal sBankOrgCode As String) As Boolean
            Dim bRetVal As Boolean = True
            Dim iTemp As Integer = 0
            Dim strSQL As String = "SELECT COUNT(0) FROM tCor_MandatesDetails WHERE OrgId <> " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & " AND BankOrgCode  = " & clsDB.SQLStr(sBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Try
                iTemp = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSQL)
                If iTemp = 0 Then
                    bRetVal = False
                End If
            Catch ex As Exception
                ErrorLog("clsMandates.IsDuplicateBankOrgCode Param:OID" & lngOrgId.ToString & ";BOrgCd:" & sBankOrgCode, Err.Number, ex.Message)
            End Try
            Return bRetVal
        End Function

        Public Function IsDuplicate(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String, Optional ByRef lngRecId As Long = 0) As Boolean
            Dim bRetVal As Boolean = True
            Dim iTemp As Integer = 0
            Dim strSQL As String = "SELECT TOP 1 ID FROM tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & " AND RefNo = " & clsDB.SQLStr(sRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(sAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(sBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Try
                lngRecId = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSQL)
                If lngRecId = 0 Then
                    bRetVal = False
                End If
            Catch ex As Exception
                ErrorLog("clsMandates.IsDuplicate Param:OID" & lngOrgId.ToString & ";RefNo" & sRefNo & ";AccNo:" & sAccNo & ";BOrgCd:" & sBankOrgCode, Err.Number, ex.Message)
            End Try
            Return bRetVal
        End Function

        Public Function IsDuplicateTemp(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String, Optional ByRef lngRecId As Long = 0) As Boolean
            Dim bRetVal As Boolean = True
            Dim iTemp As Integer = 0
            Dim strSQL As String = "SELECT TOP 1 ID FROM tCor_MandatesFileDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & " AND RefNo = " & clsDB.SQLStr(sRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(sAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(sBankOrgCode, clsDB.SQLDataTypes.Dt_String) & " AND ApprovalStatus = 0"
            Try
                lngRecId = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSQL)
                If lngRecId = 0 Then
                    bRetVal = False
                End If
            Catch ex As Exception
                ErrorLog("clsMandates.IsDuplicate Param:OID" & lngOrgId.ToString & ";RefNo" & sRefNo & ";AccNo:" & sAccNo & ";BOrgCd:" & sBankOrgCode, Err.Number, ex.Message)
            End Try
            Return bRetVal
        End Function
        'hafeez start
        Public Function IsActive(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String, Optional ByRef lngRecId As Long = 0) As Boolean
            Dim bRetVal As Boolean = True
            Dim iTemp As Integer = 0
            Dim strSQL As String = "SELECT TOP 1 ID FROM tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & " AND RefNo = " & clsDB.SQLStr(sRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(sAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(sBankOrgCode, clsDB.SQLDataTypes.Dt_String) & " AND Status = 'I'"
            Try
                lngRecId = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSQL)
                If lngRecId = 0 Then
                    bRetVal = False
                End If
            Catch ex As Exception
                ErrorLog("clsMandates.IsActive Param:OID" & lngOrgId.ToString & ";RefNo" & sRefNo & ";AccNo:" & sAccNo & ";BOrgCd:" & sBankOrgCode, Err.Number, ex.Message)
            End Try
            Return bRetVal
        End Function
        'hafeez end

#End Region

        '18-09-2008 Public function created by Marcus Yap

        Public Function ValidateMandate(ByVal intLineNumber As Integer) As String
            Dim strReturnMsg As String = ""

            If clsMandates.fncIsMandateExists(_OrgID, _RefNo, _AccNo, _BankOrgCode) Then
                If Not fncMandateActive(_OrgID, _RefNo, _AccNo, _BankOrgCode) Then
                    strReturnMsg = "Line " & intLineNumber & " has inactive customer. Reference Number: " & _RefNo & Environment.NewLine
                End If
                'If Not fncValidCustomerName(_OrgID, _RefNo, _AccNo, _BankOrgCode, _CustomerName) Then
                '    strReturnMsg += "Line " & intLineNumber & " has invalid customer name. Reference Number: " & _RefNo & Environment.NewLine
                'End If
                If Not fncValidLimitAmount(_OrgID, _RefNo, _AccNo, _BankOrgCode, _LimitAmount) Then
                    strReturnMsg += "Line " & intLineNumber & "'s debit amount has exceed customer limit. Reference Number: " & _RefNo & Environment.NewLine
                End If
                If Not fncValidateSubmissionFeq(_OrgID, _RefNo, _AccNo, _BankOrgCode) Then
                    strReturnMsg += "Line " & intLineNumber & "' has exceed submission limit. Reference Number: " & _RefNo & Environment.NewLine
                End If
                'hafeez start
                If IsActive(_OrgID, _RefNo, _AccNo, _BankOrgCode) Then
                    strReturnMsg += "Line " & intLineNumber & " has a inactive record." & gc_BR
                End If
                'hafeez end

                If strReturnMsg = "" Then
                    strReturnMsg = gc_Status_OK
                End If
            Else
                strReturnMsg = "Line " & intLineNumber & "'s customer does not exist. Reference Number: " & _RefNo & Environment.NewLine
            End If

            Return strReturnMsg
        End Function

        '18-09-2008 private function created by Marcus Yap
        Private Shared Function fncIsMandateExists(ByVal lngOrgId As Long, ByVal strRefNo As String, ByVal strAccNo As String, ByVal strBankOrgCode As String) As Boolean

            Dim intRetVal As String = 0
            Dim booIsValid As Boolean = False
            Dim strSql As String = "SELECT COUNT(0) FROM tCor_MandatesDetails WHERE OrgId = " & _
                clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & " AND RefNo = " & clsDB.SQLStr _
                (strRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(strAccNo, clsDB.SQLDataTypes.Dt_String) _
                    & " AND BankOrgCode = " & clsDB.SQLStr(strBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Dim clsGeneric As New Generic

            Try
                intRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSql)

                If intRetVal > 0 Then
                    booIsValid = True
                Else
                    booIsValid = False
                End If
            Catch ex As Exception
                clsGeneric.ErrorLog("clsMandates.fncIsMandateExists Param:OID" & lngOrgId.ToString & ";RefNo" & strRefNo & ";AccNo:" & strAccNo & ";BOrgCd:" & strBankOrgCode, Err.Number, ex.Message)
                Throw ex
            Finally
                clsGeneric = Nothing
            End Try

            Return booIsValid
        End Function

        '18-09-2008 private function created by Marcus Yap
        Private Shared Function fncValidCustomerName(ByVal lngOrgId As Long, ByVal strRefNo As String, ByVal strAccNo As String, ByVal strBankOrgCode As String, ByVal strCustomerName As String) As Boolean
            Dim strRetVal As String = ""
            Dim booIsValid As Boolean = False
            Dim strSql As String = "SELECT CustomerName FROM tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & "AND RefNo = " & clsDB.SQLStr(strRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(strAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(strBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Dim clsGeneric As New Generic

            Try
                strRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSql)

                If strRetVal.Trim = strCustomerName Then
                    booIsValid = True
                End If

            Catch ex As Exception
                clsGeneric.ErrorLog("clsMandates.fncValidateCustomerName Param:OID" & lngOrgId.ToString & ";RefNo" & strRefNo & ";AccNo:" & strAccNo & ";BOrgCd:" & strBankOrgCode, Err.Number, ex.Message)
                Throw ex
            Finally
                clsGeneric = Nothing
            End Try

            Return booIsValid

        End Function

        '18-09-2008 private function created by Marcus Yap
        Private Shared Function fncMandateActive(ByVal lngOrgId As Long, ByVal strRefNo As String, ByVal strAccNo As String, ByVal strBankOrgCode As String) As Boolean

            Dim intRetVal As Integer = 0
            Dim booIsValid As Boolean = False
            Dim strSql As String = "SELECT ApprovalStatus FROM tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & "AND RefNo = " & clsDB.SQLStr(strRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(strAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(strBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Dim clsGeneric As New Generic

            Try
                intRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSql)

                If intRetVal = 1 Then
                    booIsValid = True
                End If

            Catch ex As Exception
                clsGeneric.ErrorLog("clsMandates.fncValidateCustomerName Param:OID" & lngOrgId.ToString & ";RefNo" & strRefNo & ";AccNo:" & strAccNo & ";BOrgCd:" & strBankOrgCode, Err.Number, ex.Message)
                Throw ex
            Finally
                clsGeneric = Nothing
            End Try

            Return booIsValid

        End Function

        '18-09-2008 private function created by Marcus Yap
        Private Shared Function fncValidLimitAmount(ByVal lngOrgId As Long, ByVal strRefNo As String, ByVal strAccNo As String, ByVal strBankOrgCode As String, ByVal decLimitAmount As Decimal) As Boolean
            Dim decRetVal As String = ""
            Dim booIsValid As Boolean = False
            Dim strSql As String = "SELECT LimitAmount FROM tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & "AND RefNo = " & clsDB.SQLStr(strRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(strAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(strBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Dim clsGeneric As New Generic

            Try

                decRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSql)

                If Not decLimitAmount > decRetVal Then
                    booIsValid = True
                End If

            Catch ex As Exception
                clsGeneric.ErrorLog("clsMandates.fncValidateLimitAmount Param:OID" & lngOrgId.ToString & ";RefNo" & strRefNo & ";AccNo:" & strAccNo & ";BOrgCd:" & strBankOrgCode, Err.Number, ex.Message)
                Throw ex
            Finally
                clsGeneric = Nothing
            End Try

            Return booIsValid

        End Function

        Private Shared Function fncValidateSubmissionFeq(ByVal lngOrgId As Long, ByVal strRefNo As String, ByVal strAccNo As String, ByVal strBankOrgCode As String) As Boolean
            Dim ds As New DataSet
            Dim strFrequency As String = ""
            Dim intLimit As Integer = 0
            Dim intTransaction As Integer = 0
            Dim strFeqSql As String = "select Frequency,FrequencyLimit from tCor_MandatesDetails WHERE OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & "AND RefNo = " & clsDB.SQLStr(strRefNo, clsDB.SQLDataTypes.Dt_String) & " AND AccNo = " & clsDB.SQLStr(strAccNo, clsDB.SQLDataTypes.Dt_String) & " AND BankOrgCode = " & clsDB.SQLStr(strBankOrgCode, clsDB.SQLDataTypes.Dt_String)
            Dim strLimitSql As String = ""
            Dim strTempSql As String = ""
            Dim intMonth As Integer = 0
            Dim booIsValid As Boolean = False
            Dim clsGeneric As New Generic

            Try
                ds = SqlHelper.ExecuteDataset(sSQLConnection, CommandType.Text, strFeqSql)
                strFrequency = ds.Tables(0).Rows(0).Item("Frequency")
                intLimit = CInt(ds.Tables(0).Rows(0).Item("FrequencyLimit"))

                If Not strFrequency = clsCommon.fncGetPrefix(enmFrequency.UN_Unlimited) Then
                    Select Case strFrequency
                        Case clsCommon.fncGetPrefix(enmFrequency.MY_Monthly)
                            strTempSql = "month(tPgt_FileDetails.FileDtTm) = " & Month(Now)
                        Case clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly)
                            intMonth = Month(Now)
                            If intMonth <= 3 Then
                                strTempSql = "month(tPgt_FileDetails.FileDtTm) in (1,2,3)"
                            ElseIf intMonth >= 4 AndAlso intMonth <= 6 Then
                                strTempSql = "month(tPgt_FileDetails.FileDtTm) in (4,5,6)"
                            ElseIf intMonth >= 7 AndAlso intMonth <= 9 Then
                                strTempSql = "month(tPgt_FileDetails.FileDtTm) in (7,8,9)"
                            ElseIf intMonth >= 10 Then
                                strTempSql = "month(tPgt_FileDetails.FileDtTm) in (10,11,12)"
                            End If
                        Case clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly)
                            intMonth = Month(Now)
                            If intMonth <= 6 Then
                                strTempSql = "month(tPgt_FileDetails.FileDtTm) in (1,2,3,4,5,6)"
                            Else
                                strTempSql = "month(tPgt_FileDetails.FileDtTm) in (7,8,9,10,11,12)"
                            End If
                        Case clsCommon.fncGetPrefix(enmFrequency.YY_Yearly)
                            strTempSql = "year(tPgt_FileDetails.FileDtTm) = " & Year(Now)
                    End Select

                    strLimitSql = "select count(0) from tCor_DirectDebitUploaded inner join tPgt_FileDetails on tCor_DirectDebitUploaded.[File Id] = tPgt_FileDetails.FileId" & _
                                            " and tPgt_FileDetails.FileType = 'Direct Debit' and  " & strTempSql & _
                                            " and tPgt_FileDetails.OrgId = " & lngOrgId & " and tPgt_FileDetails.BankOrgCode = '" & strBankOrgCode & "' and tCor_DirectDebitUploaded.[Debit Reference Number] = '" & strRefNo & _
                                            "' and  tCor_DirectDebitUploaded.[CIMB Debit Account Number] = '" & strAccNo & "'" & _
                                            " and tPgt_FileDetails.Blocked = 0 and tPgt_FileDetails.Processed <> 6 and (tcor_directdebituploaded.[Bank Process Code] is null or tcor_directdebituploaded.[Bank Process Code] = '00')"

                    If (strFrequency = clsCommon.fncGetPrefix(enmFrequency.MY_Monthly) Or _
                      strFrequency = clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly) _
                     Or strFrequency = clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly)) Then
                        'Added Year Checking on 04-03-2012 
                        strLimitSql = strLimitSql & " and year(tPgt_FileDetails.FileDtTm)= " & Year(Now)

                    End If

                    intTransaction = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strLimitSql)

                    If Not intTransaction + 1 > intLimit Then
                        booIsValid = True
                    End If
                Else
                    booIsValid = True
                End If
            Catch ex As Exception
                clsGeneric.ErrorLog("clsMandates.IsDuplicate Param:OID" & lngOrgId.ToString & ";RefNo" & strRefNo & ";AccNo:" & strAccNo & ";BOrgCd:" & strBankOrgCode, Err.Number, ex.Message)
                Throw ex
            Finally
                clsGeneric = Nothing
                ds = Nothing
            End Try

            Return booIsValid

        End Function

        Public Sub New(ByVal lngOrgId As Long, ByVal sRefNo As String, ByVal sAccNo As String, ByVal sBankOrgCode As String, ByVal sCustomerName As String, ByVal dLimitAmount As Decimal)
            _OrgID = lngOrgId
            _RefNo = sRefNo
            _AccNo = sAccNo
            _BankOrgCode = sBankOrgCode
            _CustomerName = sCustomerName
            _LimitAmount = dLimitAmount
        End Sub
        Public Sub New()

        End Sub
    End Class
End Namespace
