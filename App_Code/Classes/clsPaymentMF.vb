Imports Microsoft.VisualBasic
Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Namespace MaxPayroll
    Public Class clsPaymentMF

#Region "Duplicate Bank Code"

        '*******************************************************************************************************************
        'Procedure Name     : fncChkDuplicateBankCode 
        'Purpose            : To Check For Duplicate
        'Arguments          : Payment Window Code
        'Return Values      : Boolean
        'Author             : Victor Wong
        'Created            : 2007-02-08
        '********************************************************************************************************************
        Public Function fncChkDuplicatePayWinCode(ByVal strPayWinCode As String, ByVal intBankID As Integer) As Boolean

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
                    .CommandText = "pg_ChkDuplicatePaymentWindowCode"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@vcPayWinCode", strPayWinCode))
               '.Parameters.Add(New SqlParameter("@iBankID", intBankID))
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
                clsGeneric.ErrorLog("pg_ChkDuplicatePaymentWindowCode", Err.Number, Err.Description)

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
        'Function Name  : fncInsertPaymentWindowDefinitionTable
        'Purpose        : To insert Payment Type Information captured from the user registration page into database
        'Arguments      : N/A
        'Return Value   : Status of the operation
        'Author         : Victor Wong
        'Created        : 07/Feb/2007
        '***********************************************************************************************************
        Public Function fncInsertPaymentWindowDefinitionTable() As Boolean

            'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic
            Dim clsEncryption As New Encryption
            'Declaring new instance of System.Data.SqlClient.SqlCommand
            Dim cmdInsert As New SqlCommand

            'Declaring new instance of System.Web.HttpContext
            Dim AspNetContext As HttpContext = HttpContext.Current
            Dim strPayWinCode As String = ""
            Dim strSrvName As String = ""
            Dim strIPAddress As String = ""
            Dim strUploadDir As String = ""
            Dim strDownLdDir As String = ""
            Dim strUserID As String = ""
            Dim strPasswd As String = ""
            Dim strFtpType As String = ""
            Dim strPayWinStart As String = ""
            Dim strPayWinDesc As String = ""
            Dim strPayWinStatus As String = ""
            Dim intBankID As Integer
            Dim intMaker As Integer

            Dim bRetVal As Boolean = False

            Try

                'Initialize connection to the SQL Server 2000 
                clsGeneric.SQLConnection_Initialize()

                strPayWinCode = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPayWinCode") & "")
                strSrvName = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpServerName") & "")
                strIPAddress = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpIPAddress") & "")
                strUploadDir = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpUploadDir") & "")
                strDownLdDir = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpDownloadDir") & "")
                strUserID = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpUserID") & "")

                'strPasswd = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpPwd") & "")
                strPasswd = clsEncryption.Cryptography(Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpPwd") & ""))
                strFtpType = Trim(AspNetContext.Request.Form("ctl00$cphContent$ddlFtpFunction") & "")
                strPayWinStart = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPayWinStartTime") & "")
                strPayWinDesc = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPayWinDesc") & "")
                strPayWinStatus = Trim(AspNetContext.Request.Form("ctl00$cphContent$radStatus") & "")
                intBankID = CInt(Trim(AspNetContext.Request.Form("ctl00$cphContent$ddlBankID") & ""))


                'Getting the value from the user registration form

            intMaker = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)        'Logged in User Id

                With cmdInsert

                    'Setting the SqlCommad properties
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_InstPaymentWinDef"
                    .CommandType = CommandType.StoredProcedure

                    'Passing all the parameters required by the stored procedure

                    '@ErrorCD INTEGER = 0 OUTPUT  
                    .Parameters.Add(New SqlParameter("@PayWinCode", strPayWinCode))
                    .Parameters.Add(New SqlParameter("@BankID", intBankID))
                    .Parameters.Add(New SqlParameter("@SrvName", strSrvName))
                    .Parameters.Add(New SqlParameter("@IPAddress", strIPAddress))
                    .Parameters.Add(New SqlParameter("@UploadDir", strUploadDir))
                    .Parameters.Add(New SqlParameter("@DownLdDir", strDownLdDir))
                    .Parameters.Add(New SqlParameter("@UserID", strUserID))
                    .Parameters.Add(New SqlParameter("@Passwd", strPasswd))
                    .Parameters.Add(New SqlParameter("@FtpType", strFtpType))
                    .Parameters.Add(New SqlParameter("@PayWinStart", strPayWinStart))
                    .Parameters.Add(New SqlParameter("@PayWinDesc", strPayWinDesc))
                    .Parameters.Add(New SqlParameter("@PayWinStatus", strPayWinStatus))
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
                Call clsGeneric.ErrorLog("clsPaymentMF - fncInsertPaymentWindowDefinitionTable", Err.Number, Err.Description)
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

#Region "Update Bank Definition table"

        '***********************************************************************************************************
        'Function Name  : fncUpdatePaymentWindowDefinitionTable
        'Purpose        : To update all the data captured from the payment window definition registration form into database
        'Arguments      : 
        'Return Value   : Status of the operation
        'Author         : Victor Wong
        'Created        : 2007-02-08
        '***********************************************************************************************************
      Public Function fncUpdatePaymentWindowDefinitionTable(Optional ByVal strFTPPwd As String = "", Optional ByVal bNewPwd As Boolean = False) As Boolean

         'Declaring new instance of System.Data.SqlClient.SqlCommand
         Dim cmdUpdate As New SqlCommand

         'Declaring new instance of System.Web.HttpContext
         Dim AspNetContext As HttpContext = HttpContext.Current
         Dim clsEncryption As New Encryption
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring local variables
         Dim intLastModifyBy As Int16
         Dim dtLastModifyDate As Date
         Dim intBankID As Integer
         Dim intPayWinID As Integer
         Dim strSrvName As String
         Dim strIPAddress As String
         Dim strUploadDir As String
         Dim strDownLdDir As String
         Dim strUserID As String
         Dim strFtpType As String
         Dim strPayWinStart As String
         Dim strPayWinDesc As String
         Dim strPayWinStatus As String

         Dim bRetVal As Boolean = False
         Try

            'Initialize the connection to SQL Server 2000
            clsGeneric.SQLConnection_Initialize()

            'Getting the value from the merchant registration form
                intBankID = CInt(Trim(AspNetContext.Request.Form("ctl00$cphContent$hidBankID")))
                intPayWinID = CInt(Trim(AspNetContext.Request.Form("ctl00$cphContent$hidPayWinID")))
                strSrvName = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpServerName") & "")
                strIPAddress = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpIPAddress") & "")
                strUploadDir = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpUploadDir") & "")
                strDownLdDir = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpDownloadDir") & "")
                strUserID = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtFtpUserID") & "")
                strFtpType = Trim(AspNetContext.Request.Form("ctl00$cphContent$hidFtpFunction") & "")
                strPayWinStart = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPayWinStartTime") & "")
                strPayWinDesc = Trim(AspNetContext.Request.Form("ctl00$cphContent$txtPayWinDesc") & "")
                strPayWinStatus = Trim(AspNetContext.Request.Form("ctl00$cphContent$hidStatus") & "")



            intLastModifyBy = IIf(IsNumeric(AspNetContext.Session(gc_Ses_UserID)), AspNetContext.Session(gc_Ses_UserID), 0)
            dtLastModifyDate = Now

            With cmdUpdate

               'Setting the SqlCommad properties
               .Connection = clsGeneric.SQLConnection
               .CommandText = "pg_UpdPaymentWinDef"
               .CommandType = CommandType.StoredProcedure

               'Passing all the parameters required by the stored procedure
               .Parameters.Add(New SqlParameter("@PayWinId", intPayWinID))
               .Parameters.Add(New SqlParameter("@BankID", intBankID))
               .Parameters.Add(New SqlParameter("@SrvName", strSrvName))
               .Parameters.Add(New SqlParameter("@IPAddress", strIPAddress))
               .Parameters.Add(New SqlParameter("@UploadDir", strUploadDir))
               .Parameters.Add(New SqlParameter("@DownLdDir", strDownLdDir))
               .Parameters.Add(New SqlParameter("@FtpUserID", strUserID))
               If Len(strFTPPwd) > 0 Then
                  If bNewPwd Then
                     .Parameters.Add(New SqlParameter("@FtpPasswd", clsEncryption.Cryptography(strFTPPwd)))
                  Else
                     .Parameters.Add(New SqlParameter("@FtpPasswd", strFTPPwd))
                  End If

               End If
               .Parameters.Add(New SqlParameter("@FtpType", strFtpType))
               .Parameters.Add(New SqlParameter("@PayWinStart", strPayWinStart))
               .Parameters.Add(New SqlParameter("@PayWinDesc", strPayWinDesc))
               .Parameters.Add(New SqlParameter("@PayWinStatus", strPayWinStatus))
               .Parameters.Add(New SqlParameter("@LastModifyBy", intLastModifyBy))
               .Parameters.Add(New SqlParameter("@LastModifyDate", DateTime.Now))

               'Execute the store procedure
               .ExecuteNonQuery()
               bRetVal = True
            End With

         Catch Ex As SystemException

            'Log Error
            Call clsGeneric.ErrorLog("pg_UpdPaymentWinDef", Err.Number, Ex.Message)


         Finally
            clsGeneric.SQLConnection_Terminate()
            cmdUpdate = Nothing
            clsGeneric = Nothing
         End Try
         Return bRetVal
      End Function

#End Region

#Region "Search Payment Window Definition table"

        '****************************************************************************************************'
        'Function Name  : fncSearchPayWinDef
        'Purpose        : Retrieve full set of Payment Window Details
        'Arguments      : 
        'Return Value   : System.Data.DataSet
        'Author         : Victor Wong
        'Created        : 2007-02-08
        '*****************************************************************************************************
        Public Function fncSearchPayWinDef(Optional ByVal intPayWinID As Integer = 0, Optional ByVal strPayWinCode As String = "", Optional ByVal strPayWinDesc As String = "", Optional ByVal intBankID As Integer = 0) As System.Data.DataSet

            'Declaring new instance of System.Data.DataSet
            Dim dsQuery As New DataSet

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.SqlClient.SqlDataAdapter
            Dim sdaQuery As SqlDataAdapter
            Try

                'Initializing the connection to the SQL Server
                clsGeneric.SQLConnection_Initialize()

                'Executing the store procedure using SqlDataAdapter
                sdaQuery = New SqlDataAdapter("Exec pg_QryPaymentWinDef " & intPayWinID.ToString & ",'" & strPayWinCode & "','" & strPayWinDesc & "'," & intBankID, clsGeneric.SQLConnection)

                'Fill current dataset with results from SqlDataAdapter
                sdaQuery.Fill(dsQuery, "Query")

                'Return DataSet to caller function
                Return dsQuery

            Catch Ex As SystemException

                clsGeneric.ErrorLog("pg_QryPaymentWinDef", Err.Number, Err.Description)

            Finally
                clsGeneric.SQLConnection_Terminate()
                sdaQuery = Nothing
                clsGeneric = Nothing
            End Try
            Return Nothing
        End Function

#End Region


    End Class


End Namespace