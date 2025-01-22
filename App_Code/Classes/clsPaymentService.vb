Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll

    '****************************************************************************************************
    'Class Name     : clsBank
    'ProgId         : MaxPayroll.clsPaymentService
    'Purpose        : Payment Service Used For The Complete Project
    'Author         : Sujith Sharatchandran - 
    'Created        : 2007-02-04
    '*****************************************************************************************************

    Public Class clsPaymentService
#Region "Check Multiple Bank"
        Public Function fncIsMultipleBank(ByVal sFileType As String) As Boolean
            Dim bRetVal As Boolean = False
            Dim clsGeneric As New Generic
            Dim strSQL As String = "Select IsMultipleBank From mCor_PaymentService WHERE PaySer_Desc=" & clsDB.SQLStr(sFileType)
            Try
                bRetVal = CBool(SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL))
            Catch ex As Exception
                clsGeneric.ErrorLog("clsPaymentService-fncIsMultipleBank:" & sFileType.ToString, Err.Number, Err.Description)
            End Try
            Return bRetVal
        End Function

#End Region

#Region "Duplicate Bank Code"

        '*******************************************************************************************************************
        'Procedure Name     : fncChkDuplicatePaySrvCode 
        'Purpose            : To Check For Duplicate
        'Arguments          : Payment Service Code
        'Return Values      : Boolean
        'Author             : Victor Wong
        'Created            : 2007-02-12
        '********************************************************************************************************************
        Public Function fncChkDuplicatePaySrvCode(ByVal strPaySrvCode As String) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdDuplicate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim IsDuplicate As Boolean, intResult As Int32
            Try

                'Initialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDuplicate
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_ChkDuplicatePaymentServiceCode"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@vcPaySerCode", strPaySrvCode))
                    .Parameters.Add(New SqlParameter("@bIsDuplicate", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "mIntResult", DataRowVersion.Default, intResult))
                    '.ExecuteScalar()
                    .ExecuteNonQuery()
                    IsDuplicate = .Parameters("@bIsDuplicate").Value
                End With

                'If intResult > 0 Then
                '    IsDuplicate = True
                'Else
                '    IsDuplicate = False
                'End If

                Return IsDuplicate

            Catch

                'Log Error
                clsGeneric.ErrorLog("pg_ChkDuplicatePaymentServiceCode", Err.Number, Err.Description)
                Return False
            Finally

                'Destroy SQL Command Object
                cmdDuplicate = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Insert"
        '***********************************************************************************************************
        'Function Name  : fncInsertPaymentService
        'Purpose        : To insert Payment service data into database.
        'Arguments      : N/A
        'Return Value   : Status of the operation
        'Author         : Victor Wong
        'Created        : 2007-02-12
        '***********************************************************************************************************
        Public Function fncInsertPaymentService() As Boolean

            'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic
            Dim clsEncryption As New Encryption
            'Declaring new instance of System.Data.SqlClient.SqlCommand
            Dim cmdInsert As New SqlCommand

            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current
            Dim strPaySer_Desc As String = ""
            Dim strPaySrvCode As String = ""
            Dim strPaySrvDesc As String = ""
            Dim strPaySrvStatus As String = ""
            Dim strStatutory As String = ""
            Dim bIsMultipleBank As Boolean = False
            Dim intMaker As Integer

            Dim bRetVal As Boolean = False

            Try

                'Initialize connection to the SQL Server 2000 
                clsGeneric.SQLConnection_Initialize()
                strPaySer_Desc = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPaySrvDesc") & "")
                strPaySrvCode = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPaySrvCode") & "")
                strPaySrvDesc = ""
                strPaySrvStatus = Trim(AspNetContext.Request.Form("ctl00$cphContent$radStatus") & "")
                bIsMultipleBank = CBool(Trim(AspNetContext.Request.Form("ctl00$cphContent$rbIsMultipleBank")))
                strStatutory = Trim(AspNetContext.Request.Form("ctl00$cphContent$ddlStatutoryNumber") & "")
                'Getting the value from the user registration form

                intMaker = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)        'Logged in User Id

                With cmdInsert

                    'Setting the SqlCommad properties
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_InstPaymentService"
                    .CommandType = CommandType.StoredProcedure

                    'Passing all the parameters required by the stored procedure

                    '@ErrorCD INTEGER = 0 OUTPUT  
                    .Parameters.Add(New SqlParameter("@PaySer_Desc", strPaySer_Desc))
                    .Parameters.Add(New SqlParameter("@PaySrvCode", strPaySrvCode))
                    .Parameters.Add(New SqlParameter("@PaySrvDesc", strPaySrvDesc))
                    .Parameters.Add(New SqlParameter("@PaySrvStatus", strPaySrvStatus))
                    .Parameters.Add(New SqlParameter("@PayStatutory", strStatutory))
                    .Parameters.Add(New SqlParameter("@IsMultipleBank", bIsMultipleBank))
                    .Parameters.Add(New SqlParameter("@Maker", intMaker))
                    .Parameters.Add(New SqlParameter("@MakerDate", DateTime.Now))
                    '.Parameters.Add(New SqlParameter("@ErrorCD", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "mIntResult", DataRowVersion.Default, intResult))

                    'Execute the store procedure
                    .ExecuteNonQuery()

                End With
                bRetVal = True

            Catch Ex As SystemException

                'In case of SystemException occur, return the error message to caller function.
                'Log Error
                Call clsGeneric.ErrorLog("clsPaymentService - fncInsertPaymentService", Err.Number, Err.Description)
                'Return Ex.Message

            Finally

                'Terminate the SQL Server connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destory current instance of System.Data.SqlClient.SqlCommand
                cmdInsert = Nothing

                'Destory current instance of System.Web.HttpContext
                AspNetContext = Nothing

            End Try
            Return bRetVal
        End Function
#End Region

#Region "Search Payment Service Definition table"

        '****************************************************************************************************'
        'Function Name  : fncSearchPayWinDef
        'Purpose        : Retrieve full set of Payment Window Details
        'Arguments      : 
        'Return Value   : System.Data.DataSet
        'Author         : Victor Wong
        'Created        : 2007-02-08
        '*****************************************************************************************************
        Public Function fncSearchPaySrv(Optional ByVal intPaySrvID As Integer = 0, Optional ByVal strPaySrvCode As String = "", Optional ByVal strPaySrvDesc As String = "") As DataSet

            'Declaring new instance of System.Data.DataSet
            Dim dsQuery As New DataSet
            Dim cmdQuery As New SqlCommand

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.SqlClient.SqlDataAdapter
            Dim sdaQuery As SqlDataAdapter
            Try

                'Initializing the connection to the SQL Server
                clsGeneric.SQLConnection_Initialize()

                'Executing the store procedure using SqlDataAdapter
                sdaQuery = New SqlDataAdapter("Exec pg_QryPaymentService " & intPaySrvID.ToString & ",'" & strPaySrvCode & "','" & strPaySrvDesc & "'", clsGeneric.SQLConnection)

                'Fill current dataset with results from SqlDataAdapter
                sdaQuery.Fill(dsQuery, "Query")

                'Return DataSet to caller function
                Return dsQuery

            Catch Ex As SystemException

                clsGeneric.ErrorLog("pg_QryPaymentService", Err.Number, Err.Description)

            Finally
                clsGeneric.SQLConnection_Terminate()
                sdaQuery = Nothing
                clsGeneric = Nothing
            End Try
            Return Nothing
        End Function

#End Region



#Region "Update Payment Service Definition table"

        '***********************************************************************************************************
        'Function Name  : fncUpdatePaymentWindowDefinitionTable
        'Purpose        : To update all the data captured from the payment service definition registration form into database
        'Arguments      : 
        'Return Value   : Status of the operation
        'Author         : Victor Wong
        'Created        : 2007-02-12
        '***********************************************************************************************************
        Public Function fncUpdatePaymentServiceTable() As Boolean

            'Declaring new instance of System.Data.SqlClient.SqlCommand
            Dim cmdUpdate As New SqlCommand

            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current
            Dim clsEncryption As New Encryption
            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring local variables
            Dim strPaySrvDesc As String
            Dim strPaySrvCode As String
            Dim intPaySrvID As Integer
            Dim strPaySrvStatus As String
            Dim intLastModifyBy As Int16
            Dim dtLastModifyDate As Date
            Dim bIsMultipleBank As Boolean
            Dim strStatutory As String

            Dim bRetVal As Boolean = False
            Try

                'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

                'Getting the value from the merchant registration form
                intPaySrvID = Trim(AspNetContext.Request.Form("ctl00$cphContent$hidPaySerID") & "")
                strPaySrvDesc = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPaySrvDesc") & "")
                strPaySrvCode = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPaySrvCode") & "")
                strPaySrvStatus = Trim(AspNetContext.Request.Form("ctl00$cphContent$hidStatus") & "")
                bIsMultipleBank = CBool(Trim(AspNetContext.Request.Form("ctl00$cphContent$hidIsMultipleBank") & ""))
                strStatutory = Trim(AspNetContext.Request.Form("ctl00$cphContent$hidStatutory") & "")


                intLastModifyBy = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)
                dtLastModifyDate = Now

                With cmdUpdate

                    'Setting the SqlCommad properties
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_UpdPaymentService"
                    .CommandType = CommandType.StoredProcedure

                    'Passing all the parameters required by the stored procedure

                    .Parameters.Add(New SqlParameter("@PaySer_Id", intPaySrvID))
                    .Parameters.Add(New SqlParameter("@PaySrvCode", strPaySrvCode))
                    .Parameters.Add(New SqlParameter("@PaySer_Desc", strPaySrvDesc))
                    .Parameters.Add(New SqlParameter("@PaySrvStatus", strPaySrvStatus))
                    .Parameters.Add(New SqlParameter("@PayStatutory", strStatutory))
                    .Parameters.Add(New SqlParameter("@IsMultipleBank", bIsMultipleBank))
                    .Parameters.Add(New SqlParameter("@LastModifyBy", intLastModifyBy))
                    .Parameters.Add(New SqlParameter("@LastModifyDate", DateTime.Now))

                    'Execute the store procedure
                    .ExecuteNonQuery()
                    bRetVal = True
                End With

            Catch Ex As SystemException

                'Log Error
                Call clsGeneric.ErrorLog("pg_UpdPaymentService", Err.Number, Ex.Message)


            Finally
                clsGeneric.SQLConnection_Terminate()
                cmdUpdate = Nothing
                clsGeneric = Nothing
            End Try
            Return bRetVal
        End Function

#End Region

#Region "Retreive Payment Service ID"
        '****************************************************************************************************'
        'Function Name  : fncRetrievePaymentService
        'Purpose        : Retrieve all bank's code and name
        'Arguments      : 
        'Return Value   : System.Data.DataSet
        'Author         : Victor Wong
        'Created        : 2007-02-13
        '*****************************************************************************************************
        Public Function fncRetrievePaymentServiceID(ByVal strPaySrvType As String) As Integer
            Dim clsGeneric As New MaxPayroll.Generic

            Dim strSQL As String = ""
            Dim sqlParams(0) As SqlParameter
            Dim bRetVal As Integer = 0
            Try
                sqlParams(0) = New SqlParameter("@in_PaymentDesc", SqlDbType.VarChar)
                sqlParams(0).Value = strPaySrvType
                bRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_QryPaymentCodeByDesc", sqlParams)

            Catch Ex As SystemException

                'In case of SystemException occur, log error into database
                clsGeneric.SQLConnection_Initialize()
                clsGeneric.ErrorLog("pg_QryPaymentCode", Err.Number, Err.Description)
                'Terminate the SQL Server 2000 connection
                Call clsGeneric.SQLConnection_Terminate()
            Finally



                'Destory current instance of efpx
                clsGeneric = Nothing

            End Try
            Return bRetVal
        End Function

        Public Function fncDDLPayment(ByVal iOrgID As Integer, ByVal iBankID As Integer) As DataTable
            Dim strSQL As String = ""
            Dim sqlParams(1) As SqlParameter
            Dim clsGeneric As New MaxPayroll.Generic
            Dim dsRetVal As New DataSet
            Dim dtRetval As New DataTable
            Try
                sqlParams(0) = New SqlParameter("@in_OrgId", SqlDbType.Int)
                sqlParams(0).Value = iOrgID
                sqlParams(1) = New SqlParameter("@in_BankId", SqlDbType.Int)
                sqlParams(1).Value = iBankID
                dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_GetFileType", sqlParams)

            Catch Ex As SystemException

                clsGeneric.ErrorLog("pg_QryPaymentCode", Err.Number, Err.Description)
            Finally



                'Destory current instance of efpx
                clsGeneric = Nothing

            End Try
            dtRetval = dsRetVal.Tables(0)
            Return dtRetval
        End Function

#End Region

#Region "Diplay Payment Desc"

        '****************************************************************************************************'
        'Function Name  : fncRetrievePaymentService
        'Purpose        : Retrieve all bank's code and name
        'Arguments      : 
        'Return Value   : System.Data.DataSet
        'Author         : Victor Wong
        'Created        : 2007-02-13
        '*****************************************************************************************************
        Public Function fncRetrievePaymentService() As System.Data.DataSet

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.DataSet
            Dim dsDisplay As New DataSet

            'Declaring new instance of System.Data.SqlDataAdapter
            Dim sdaDisplay As New SqlDataAdapter
            Try

                'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

                'Execute the stored procedure using SqlDataAdpater
                sdaDisplay = New SqlDataAdapter("Exec pg_QryPaymentCode", clsGeneric.SQLConnection)

                'Fill the DataSet with the result returned from the SqlDataAdapter
                sdaDisplay.Fill(dsDisplay, "PaymentService")

                'Return the DataSet to the caller function
                Return dsDisplay

            Catch Ex As SystemException

                'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("pg_QryPaymentCode", Err.Number, Err.Description)

            Finally

                'Terminate the SQL Server 2000 connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destory current instance of efpx
                clsGeneric = Nothing
                sdaDisplay = Nothing
            End Try
            Return Nothing
        End Function
#End Region

#Region "Unknown"
        Public Shared Function fnPaymentService() As DataSet
            'Create Instance of SQL Data Adaptor
            Dim sdaPS As SqlDataAdapter
            sdaPS = New SqlDataAdapter

            'Create Instance of generic Class Object
            Dim clsGeneric As MaxPayroll.Generic
            clsGeneric = New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsPS As System.Data.DataSet
            dsPS = New System.Data.DataSet

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngOrgId As Long, lngUserCode As Long

            Try

                lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intialize SQL Conenction
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adaptor
                sdaPS = New SqlDataAdapter("Exec pg_mPaymentService", clsGeneric.SQLConnection)

                'Fill Data set
                sdaPS.Fill(dsPS, "PaymentService")

                Return dsPS

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "pg_mPaymentService", Err.Number, Err.Description)

                Return dsPS

            Finally

                'Destroy SQL Data Adaptor
                sdaPS = Nothing

                'Destroy Data Set
                dsPS = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try
        End Function
#End Region

        Public Function fncCheckServiceAndAccount(ByVal lngOrgId As Integer) As String
            Dim drInfo As SqlDataReader
            Dim clsGeneric As New Generic
            Dim Param(0) As SqlParameter
            Dim sRetVal As String = ""
            Param(0) = New SqlParameter("@in_Org_Id", SqlDbType.Int)
            Param(0).Value = lngOrgId
            drInfo = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_QryAvailablePaymentService", Param)
            While drInfo.Read
                If IsDBNull(drInfo.Item("SerAcc_No")) AndAlso CStr(drInfo.Item("PaySer_Desc")) <> "Payroll File" Then
                    sRetVal += "Payment Service [" & CStr(drInfo.Item("PaySer_Desc") & "") & "]'s Account No. is not yet created in organization [E" & lngOrgId.ToString & "]" & gc_BR
                End If
            End While
            Return sRetVal
        End Function

    End Class

End Namespace