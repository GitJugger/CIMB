Imports Microsoft.VisualBasic
Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll


    Public Class clsBankMF
        '****************************************************************************************************
        'Bank Definition Creation and Maintanence
        '****************************************************************************************************
        Public Shared Function fncGetBankCode(ByVal intBankID As Integer) As String
            Dim clsGeneric As New Generic

            Dim strRetVal As String = ""
            Dim strSQL As String

            Try

                strSQL = "pg_QryBankCodeBy_BankID " & intBankID.ToString
                strRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)

            Catch ex As Exception
                clsGeneric.SQLConnection_Initialize()
                Call clsGeneric.ErrorLog("fncGetBankCode", Err.Number, Err.Description)
                clsGeneric.SQLConnection_Terminate()
            Finally

                clsGeneric = Nothing

            End Try

            Return strRetVal
        End Function
        Public Shared Function fncGetBankIDByOrgFileType(ByVal intOrgID As Integer, ByVal strPaySrvType As String) As Integer
            Dim clsGeneric As New Generic
            'Dim cmdQuery As New SqlCommand
            Dim intRetVal As Integer = 0
            Dim strSQL As String
            Dim clsPaymentService As New clsPaymentService
            Dim intPaySrvID As Integer

            intPaySrvID = clsPaymentService.fncRetrievePaymentServiceID(strPaySrvType)

            Try
                'Initialize connection to the SQL Server 2000 
                'clsGeneric.SQLConnection_Initialize()
                strSQL = "pg_QryBankIDBy_OrgID_PaySrvID " & intOrgID.ToString & "," & intPaySrvID.ToString
                intRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
                'With cmdQuery

                '    'Setting the SqlCommad properties
                '    .Connection = clsGeneric.SQLConnection
                '    .CommandText = "pg_QryGroupBankID"
                '    .CommandType = CommandType.StoredProcedure

                '    'Passing all the parameters required by the stored procedure
                '    .Parameters.Add(New SqlParameter("@in_GroupID", lngGroupID))
                '    .Parameters.Add(New SqlParameter("@Out_BankID", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "", DataRowVersion.Default, intRetVal))

                '    'Execute the store procedure
                '    .ExecuteNonQuery()
                '    intRetVal = .Parameters("@Out_BankID").Value
                'End With
            Catch ex As Exception
                clsGeneric.SQLConnection_Initialize()
                Call clsGeneric.ErrorLog("fncGetBankIDByGroup", Err.Number, Err.Description)
                clsGeneric.SQLConnection_Terminate()
            Finally

                clsGeneric = Nothing
                'cmdQuery = Nothing
            End Try

            Return intRetVal
        End Function
        Public Shared Function fncGetBankIDByGroup(ByVal lngGroupID As Long) As Integer
            Dim clsGeneric As New Generic
            Dim cmdQuery As New SqlCommand
            Dim intRetVal As Integer = 0
            Try
                'Initialize connection to the SQL Server 2000 
                clsGeneric.SQLConnection_Initialize()

                With cmdQuery

                    'Setting the SqlCommad properties
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_QryGroupBankID"
                    .CommandType = CommandType.StoredProcedure

                    'Passing all the parameters required by the stored procedure
                    .Parameters.Add(New SqlParameter("@in_GroupID", lngGroupID))
                    .Parameters.Add(New SqlParameter("@Out_BankID", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "", DataRowVersion.Default, intRetVal))

                    'Execute the store procedure
                    .ExecuteNonQuery()
                    intRetVal = .Parameters("@Out_BankID").Value
                End With
            Catch ex As Exception
                Call clsGeneric.ErrorLog("fncGetBankIDByGroup", Err.Number, Err.Description)
            Finally
                clsGeneric.SQLConnection_Terminate()
                clsGeneric = Nothing
                cmdQuery = Nothing
            End Try

            Return intRetVal
        End Function
#Region "Insert Bank Definition table"

        '***********************************************************************************************************
        'Function Name  : fncInsertBankDefinitionTable
        'Purpose        : To insert user information captured from the user registration page into database
        'Arguments      : N/A
        'Return Value   : Status of the operation
        'Author         : Eric Wong Kok Tong - T-Melmax Sdn. Bhd.
        'Created        : 05/01/2006
        '***********************************************************************************************************
        Public Function fncInsertBankDefinitionTable() As Boolean

            'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.SqlClient.SqlCommand
            Dim cmdInsert As New SqlCommand

            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current

            'Declaring local variable
            Dim intMaker As Int16
            Dim dtMakerDate As Date
            Dim strBankCode As String = ""
            Dim strBankName As String = ""
            Dim strBankAdd As String = ""
            Dim strBankState As String = ""
            Dim strBankPostCode As String = ""
            Dim strBankPhone As String = ""
            Dim strBankFax As String = ""
            Dim strBankContPerson As String = ""
            Dim strBankURL As String = ""
            Dim strBankStatus As String = ""
            Dim lngOrgId As Long
            Dim lngUserCode As Long
            Dim bRetVal As Boolean = False

            Try

            lngOrgId = IIf(IsNumeric(AspNetContext.Session(MaxPayroll.mdConstant.gc_Ses_OrgId)), AspNetContext.Session(MaxPayroll.mdConstant.gc_Ses_OrgId), 0)
            lngUserCode = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)

                'Initialize connection to the SQL Server 2000 
                clsGeneric.SQLConnection_Initialize()

                'Getting the value from the user registration form
                strBankCode = AspNetContext.Request.Form("ctl00$cphContent$txtBankCode")
                strBankName = AspNetContext.Request.Form("ctl00$cphContent$txtBankName")
                strBankAdd = AspNetContext.Request.Form("ctl00$cphContent$txtBankAdd1") '& AspNetContext.Request.Form("txtBankAdd2") & AspNetContext.Request.Form("txtBankAdd3")
                strBankState = AspNetContext.Request.Form("ctl00$cphContent$ddlBankState")
                strBankPostCode = AspNetContext.Request.Form("ctl00$cphContent$txtBankPostCode")
                strBankPhone = AspNetContext.Request.Form("ctl00$cphContent$txtBankPhone")
                strBankFax = AspNetContext.Request.Form("ctl00$cphContent$txtBankFax")
                strBankContPerson = AspNetContext.Request.Form("ctl00$cphContent$txtBankContPerson")
                strBankURL = AspNetContext.Request.Form("ctl00$cphContent$txtBankURL")
            intMaker = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)        'Logged in User Id

                'Checking the value of the radiobutton list
                If Not AspNetContext.Request.Form("ctl00$cphContent$radActive") = "" Then
                    strBankStatus = AspNetContext.Request.Form("ctl00$cphContent$radActive")
                End If

                dtMakerDate = Now

                With cmdInsert

                    'Setting the SqlCommad properties
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_InstBankDefinition"
                    .CommandType = CommandType.StoredProcedure

                    'Passing all the parameters required by the stored procedure
                    .Parameters.Add(New SqlParameter("@BankCode", strBankCode))
                    .Parameters.Add(New SqlParameter("@BankName", strBankName))
                    .Parameters.Add(New SqlParameter("@BankAdd", strBankAdd))
                    .Parameters.Add(New SqlParameter("@BankState", strBankState))
                    .Parameters.Add(New SqlParameter("@BankPostCode", strBankPostCode))
                    .Parameters.Add(New SqlParameter("@BankPhone", strBankPhone))
                    .Parameters.Add(New SqlParameter("@BankFax", strBankFax))
                    .Parameters.Add(New SqlParameter("@BankContPerson", strBankContPerson))
                    .Parameters.Add(New SqlParameter("@BankURL", strBankURL))
                    .Parameters.Add(New SqlParameter("@BankStatus", strBankStatus))
                    .Parameters.Add(New SqlParameter("@Maker", intMaker))
                    .Parameters.Add(New SqlParameter("@MakerDate", dtMakerDate))

                    'Execute the store procedure
                    .ExecuteNonQuery()

                End With
                bRetVal = True

            Catch Ex As SystemException

                'In case of SystemException occur, return the error message to caller function.
                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "frmBankCodeCreation - Insert", Err.Number, Err.Description)
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

#Region "Diplay bank code"

        '****************************************************************************************************'
        'Function Name  : fncRetrieveBankCodeName
        'Purpose        : Retrieve all bank's code and name
        'Arguments      : Seller ID
        'Return Value   : System.Data.DataSet
        'Author         : Victor Wong
        'Created        : 2007-02-08
        '*****************************************************************************************************
      Public Function fncRetrieveBankCodeName(Optional ByVal sStatus As String = "") As System.Data.DataSet

         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring new instance of System.Data.DataSet
         Dim dsDisplay As New DataSet

         'Declaring new instance of System.Data.SqlDataAdapter
         Dim sdaDisplay As New SqlDataAdapter
         Try

            'Initialize the connection to SQL Server 2000
            clsGeneric.SQLConnection_Initialize()

            'Execute the stored procedure using SqlDataAdpater
            sdaDisplay = New SqlDataAdapter("Exec pg_QryBankCode " & IIf(Len(sStatus) = 0, "", "'" & sStatus & "'"), clsGeneric.SQLConnection)

            'Fill the DataSet with the result returned from the SqlDataAdapter
            sdaDisplay.Fill(dsDisplay, "Bank")

            'Return the DataSet to the caller function
            Return dsDisplay

         Catch Ex As SystemException

            'In case of SystemException occur, log error into database
            clsGeneric.ErrorLog("pg_QryBankCode", Err.Number, Err.Description)

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

#Region "Diplay bank code ID"

        '****************************************************************************************************'
        'Function Name  : fncDisplayBankID
        'Purpose        : To display all bank information returned from the search function
        'Arguments      : -
        'Return Value   : System.Data.DataSet
        'Author         : Deedee - T-Melmax Sdn. Bhd.
        'Created        : 23/02/2006
        '*****************************************************************************************************
        Public Function fncDisplayBankID(ByVal strBankCode As String) As System.Data.DataSet

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.DataSet
            Dim dsDisplay As New DataSet
            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current
            'Declaring new instance of System.Data.SqlDataAdapter
            Dim sdaDisplay As New SqlDataAdapter
            Dim lngOrgId As Long, lngUserCode As Long
            Try
            lngOrgId = IIf(IsNumeric(AspNetContext.Session(MaxPayroll.mdConstant.gc_Ses_OrgId)), AspNetContext.Session(MaxPayroll.mdConstant.gc_Ses_OrgId), 0)
            lngUserCode = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)
                'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

                'Execute the stored procedure using SqlDataAdpater
                sdaDisplay = New SqlDataAdapter("Exec fpx_QryBankCodeCreationId '" & strBankCode & "'", clsGeneric.SQLConnection)

                'Fill the DataSet with the result returned from the SqlDataAdapter
                sdaDisplay.Fill(dsDisplay, "BankID")

                'Return the DataSet to the caller function
                Return dsDisplay

            Catch Ex As SystemException

                'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog(lngOrgId, lngUserCode, "", Err.Number, Err.Description)
            Finally

                'Terminate the SQL Server 2000 connection
                Call clsGeneric.SQLConnection_Terminate()
                sdaDisplay = Nothing
                'Destory current instance of efpx
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function
#End Region

#Region "Duplicate Bank Code"

        '*******************************************************************************************************************
        'Procedure Name     : fncChkDuplicateBankCode 
        'Purpose            : To Check For Duplicate
        'Arguments          : User Login
        'Return Values      : Boolean
        'Author             : Sujith Sharatchandran - T-Melmax Sdn Bhd
        'Created            : 15/10/2003
        '********************************************************************************************************************
        Public Function fncChkDuplicateBankCode(ByVal strBankCode As String) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdDuplicate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim IsDuplicate As Boolean, intResult As Int32, strUserType As String, lngUserCode As Long, lngOrgId As Long
            Try
                'Get User Type
            strUserType = ASPNetContext.Session(gc_Ses_UserType)
                'Get Logged User Code
            lngUserCode = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_UserID)), ASPNetContext.Session(gc_Ses_UserID), 0)

                'Initialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDuplicate
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_ChkDuplicateBankCode"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@vcBankCode", strBankCode))
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
                clsGeneric.ErrorLog(lngOrgId, lngUserCode, "", Err.Number, Err.Description)

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

#Region "Search Bank Definition table"

        '****************************************************************************************************'
        'Function Name  : fncSearchBankDefinition
        'Purpose        : To search the database for matching seller id
        'Arguments      : Seller ID
        'Return Value   : System.Data.DataSet
        'Author         : Eric Wong Kok Tong - T-Melmax Sdn. Bhd.
        'Created        : 17/01/2006
        '*****************************************************************************************************
        Public Function fncSearchBankDefinition(Optional ByVal strBankCode As String = "", Optional ByVal strBankName As String = "", Optional ByVal intBankID As Integer = 0) As System.Data.DataSet

            'Declaring new instance of System.Data.DataSet
            Dim dsQuery As New DataSet

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.SqlClient.SqlDataAdapter
            Dim sdaQuery As SqlDataAdapter
            Try

                'Initializing the connection to the SQL Server
                clsGeneric.SQLConnection_Initialize()

                'Executing the store procedure using SqlDataAdapter
                sdaQuery = New SqlDataAdapter("Exec pg_QryBankDefinition '" & strBankCode & "','" & strBankName & "'," & intBankID, clsGeneric.SQLConnection)

                'Fill current dataset with results from SqlDataAdapter
                sdaQuery.Fill(dsQuery, "Query")

                'Return DataSet to caller function
                Return dsQuery

            Catch Ex As SystemException

                clsGeneric.ErrorLog("pg_QryBankDefinition", Err.Number, Err.Description)

            Finally
                clsGeneric.SQLConnection_Terminate()
                sdaQuery = Nothing
                clsGeneric = Nothing
            End Try
            Return Nothing
        End Function

#End Region

#Region "Update Bank Definition table"

        '***********************************************************************************************************
        'Function Name  : fncUpdateBankDefinitionTable
        'Purpose        : To update all the data captured from the bank definition registration form into database
        'Arguments      : Seller ID & Merchant ID
        'Return Value   : Status of the operation
        'Author         : Eric Wong Kok Tong - T-Melmax Sdn. Bhd.
        'Created        : 16/12/2005
        '***********************************************************************************************************
        Public Function fncUpdateBankDefinitionTable() As Boolean

            'Declaring new instance of System.Data.SqlClient.SqlCommand
            Dim cmdUpdate As New SqlCommand

            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring local variables
            Dim intLastModifyBy As Int16
            Dim dtLastModifyDate As Date
            Dim iBankID As Integer
            Dim strBankCode As String
            Dim strBankName As String
            Dim strBankAdd As String
            Dim strBankState As String
            Dim strBankPostCode As String
            Dim strBankPhone As String
            Dim strBankFax As String
            Dim strBankContPerson As String
            Dim strBankURL As String
            Dim strBankStatus As String
            Dim bRetVal As Boolean = False
            Try

                'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

                'Getting the value from the merchant registration form
                iBankID = CInt(Trim(AspNetContext.Request.Form("ctl00$cphContent$hidBankID")))

                strBankCode = AspNetContext.Request.Form("ctl00$cphContent$txtBankCode")
                strBankName = AspNetContext.Request.Form("ctl00$cphContent$txtBankName")
                strBankAdd = AspNetContext.Request.Form("ctl00$cphContent$txtBankAdd1")
                strBankState = AspNetContext.Request.Form("ctl00$cphContent$hidBankState")
                strBankPostCode = AspNetContext.Request.Form("ctl00$cphContent$txtBankPostCode")
                strBankPhone = AspNetContext.Request.Form("ctl00$cphContent$txtBankPhone")
                strBankFax = AspNetContext.Request.Form("ctl00$cphContent$txtBankFax")
                strBankContPerson = AspNetContext.Request.Form("ctl00$cphContent$txtBankContPerson")
                strBankURL = AspNetContext.Request.Form("ctl00$cphContent$txtBankURL")

                'Checking the value of the radiobutton list
                strBankStatus = AspNetContext.Request.Form("ctl00$cphContent$hidStatus")


            intLastModifyBy = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)
                dtLastModifyDate = Now

                With cmdUpdate

                    'Setting the SqlCommad properties
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_UpdBankDefinition"
                    .CommandType = CommandType.StoredProcedure

                    'Passing all the parameters required by the stored procedure
                    
                    .Parameters.Add(New SqlParameter("@BankID", iBankID))
                    .Parameters.Add(New SqlParameter("@BankCode", strBankCode))
                    .Parameters.Add(New SqlParameter("@BankName", strBankName))
                    .Parameters.Add(New SqlParameter("@BankAdd", strBankAdd))
                    .Parameters.Add(New SqlParameter("@BankState", strBankState))
                    .Parameters.Add(New SqlParameter("@BankPostCode", strBankPostCode))
                    .Parameters.Add(New SqlParameter("@BankPhone", strBankPhone))
                    .Parameters.Add(New SqlParameter("@BankFax", strBankFax))
                    .Parameters.Add(New SqlParameter("@BankContPerson", strBankContPerson))
                    .Parameters.Add(New SqlParameter("@BankURL", strBankURL))
                    .Parameters.Add(New SqlParameter("@BankStatus", strBankStatus))
                    .Parameters.Add(New SqlParameter("@LastModifyBy", intLastModifyBy))
                    .Parameters.Add(New SqlParameter("@LastModifyDate", dtLastModifyDate))


                    'Execute the store procedure
                    .ExecuteNonQuery()
                    bRetVal = True
                End With

            Catch Ex As SystemException

                'Log Error
                Call clsGeneric.ErrorLog("pg_UpdBankDefinition", Err.Number, Ex.Message)


            Finally
                clsGeneric.SQLConnection_Terminate()
                cmdUpdate = Nothing
                clsGeneric = Nothing
            End Try
            Return bRetVal
        End Function

#End Region

    End Class
End Namespace