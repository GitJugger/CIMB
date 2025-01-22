Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data


Namespace MaxPayroll


    '****************************************************************************************************
    'Class Name     : clsBank
    'ProgId         : MaxPayroll.clsBank
    'Purpose        : Bank Functions Used For The Complete Project
    'Author     : Sujith Sharatchandran - 
    'Created        : 14/10/2003
    '*****************************************************************************************************
    Public Class clsBank

#Region "View Bank Formats"
        Public Function fnBankFormat(ByVal intBankID As Integer, ByVal strFileType As String) As System.Data.DataSet

            Dim sdaFormat As New SqlDataAdapter                         'SQL Data Adaptor
            Dim clsGeneric As New MaxPayroll.Generic                       'Create Generic Class Object
            Dim dsFormat As New System.Data.DataSet                     'SQL Data Set
            Dim ASPNetContext As HttpContext = HttpContext.Current      'Create ASP Net Context Object

            'Variable Declaration
            Dim lngOrgId As Long, lngUserCode As Long, strSQLStatement As String

            Try

                'Assign Values To Declared variables
                lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQLStatement = "Exec pg_QryBankFormat '" & strFileType & "'," & intBankID.ToString


                'Execute SQL Data Adaptor
                sdaFormat = New SqlDataAdapter(strSQLStatement, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaFormat.Fill(dsFormat, "BANK")

                Return dsFormat

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnBankFormat - clsBank", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Data Adaptor
                sdaFormat = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Destroy Data Set
                dsFormat = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function
        'Public Function fnBankFormat(ByVal strFileType As String) As System.Data.DataSet

        '    Dim sdaFormat As New SqlDataAdapter                         'SQL Data Adaptor
        '    Dim clsGeneric As New MaxPayroll.Generic                       'Create Generic Class Object
        '    Dim dsFormat As New System.Data.DataSet                     'SQL Data Set
        '    Dim ASPNetContext As HttpContext = HttpContext.Current      'Create ASP Net Context Object

        '    'Variable Declaration
        '    Dim lngOrgId As Long, lngUserCode As Long, strSQLStatement As String

        '    Try

        '        'Assign Values To Declared variables
        '            lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
        '        lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

        '        'Intialize SQL Connection
        '        Call clsGeneric.SQLConnection_Initialize()

        '        'SQL Statement
        '        strSQLStatement = "Exec pg_ViewBankFormat '" & strFileType & "'"

        '        'Execute SQL Data Adaptor
        '        sdaFormat = New SqlDataAdapter(strSQLStatement, clsGeneric.SQLConnection)

        '        'Fill Data Set
        '        sdaFormat.Fill(dsFormat, "BANK")

        '        Return dsFormat

        '    Catch

        '        'Log Error
        '        Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnBankFormat - clsBank", Err.Number, Err.Description)

        '    Finally

        '        'Terminate SQL Connection
        '        Call clsGeneric.SQLConnection_Terminate()

        '        'Destroy SQL Data Adaptor
        '        sdaFormat = Nothing

        '        'Destroy ASP Net Context Object
        '        ASPNetContext = Nothing

        '        'Destroy Data Set
        '        dsFormat = Nothing

        '        'Destroy Generic Class Object
        '        clsGeneric = Nothing

        '    End Try
        '        Return Nothing
        'End Function

#End Region

#Region "Bank Format Maintenance"


        '****************************************************************************************************
        'Procedure Name : fnDB_BankFormat()
        'Purpose        : Insert/Update Bank Format Details
        'Arguments      : Add/Update
        'Return Value   : Ok/Error
        'Author         : Sujith Sharatchandran - 
        'Created        : 15/10/2003
        '*****************************************************************************************************

        Public Function fnDB_BankFormat(ByVal strAction As String, ByVal intBankID As Integer, ByVal Service_Id As Short) As String

            'Create Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create SQL Command Object
            Dim cmdBankFormat As New SqlCommand

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strFileType As String, strDescription As String, strDataType As String, strContentType As String, strMatchField As String,
                strPredfinedOptions As String, strDefaultValue As String, intStartPos As Int16, intEndPos As Int16, lngOrgId As Long,
                    lngUserId As Long, dtCurrDate As Date, intFieldId As Int16, bIsMandatory As Boolean, bIsCustomerSetting As Boolean,
                        intColPos As Int16, intFileFormat As Int16

            Try
                'Assign Values To Declared Variables
                dtCurrDate = Now
                With ASPNetContext.Request
                    'File Type
                    strFileType = .Form("ctl00$cphContent$txtFileType")
                    'Field Description
                    strDescription = .Form("ctl00$cphContent$txtFldDesc")
                    'Field Type
                    strContentType = .Form("ctl00$cphContent$cmbFldType")
                    'Data Type
                    strDataType = .Form("ctl00$cphContent$cmbDataType")
                    'Matching Field
                    strMatchField = .Form("ctl00$cphContent$cmbMatchFld")
                    'Default Data
                    strDefaultValue = .Form("ctl00$cphContent$txtDefault")
                    'Predefined Options
                    strPredfinedOptions = .Form("ctl00$cphContent$cmbOptions")
                    'Field Id
                    intFieldId = IIf(IsNumeric(.Form("ctl00$cphContent$hFieldId")), .Form("ctl00$cphContent$hFieldId"), 0)
                    'End Position
                    intEndPos = IIf(IsNumeric(.Form("ctl00$cphContent$txtEndPos")), .Form("ctl00$cphContent$txtEndPos"), 0)
                    'Start Position
                    intStartPos = IIf(IsNumeric(.Form("ctl00$cphContent$txtStartPos")), .Form("ctl00$cphContent$txtStartPos"), 0)
                    'Column Position
                    intColPos = IIf(IsNumeric(.Form("ctl00$cphContent$txtColPos")), .Form("ctl00$cphContent$txtColPos"), 0)
                    'File Format
                    intFileFormat = IIf(IsNumeric(.Form("ctl00$cphContent$__ddlFileDelimiter")), .Form("ctl00$cphContent$__ddlFileDelimiter"), 0)

                    If IsNothing(.Form("ctl00$cphContent$rblIsMandatory")) = False Then
                        Try
                            bIsMandatory = CBool(.Form("ctl00$cphContent$rblIsMandatory"))
                        Catch
                            bIsMandatory = False
                        End Try
                    End If
                    If IsNothing(.Form("ctl00$cphContent$rblIsCustomerSetting")) = False Then
                        Try
                            bIsCustomerSetting = CBool(.Form("ctl00$cphContent$rblIsCustomerSetting"))
                        Catch
                            bIsCustomerSetting = False
                        End Try
                    End If
                End With



                'Intialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdBankFormat
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_BankFormat"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                    .Parameters.Add(New SqlParameter("@in_Option", strAction))
                    .Parameters.Add(New SqlParameter("@in_FieldId", intFieldId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_Description", strDescription))
                    .Parameters.Add(New SqlParameter("@in_DataType", strDataType))
                    .Parameters.Add(New SqlParameter("@in_ContentType", strContentType))
                    .Parameters.Add(New SqlParameter("@in_MatchField", strMatchField))
                    .Parameters.Add(New SqlParameter("@in_PredfinedOptions", strPredfinedOptions))
                    .Parameters.Add(New SqlParameter("@in_DefaultValue", strDefaultValue))
                    .Parameters.Add(New SqlParameter("@in_StartPos", intStartPos))
                    .Parameters.Add(New SqlParameter("@in_EndPos", intEndPos))
                    .Parameters.Add(New SqlParameter("@in_IsMandatory", bIsMandatory))
                    .Parameters.Add(New SqlParameter("@in_IsCustomerSetting", bIsCustomerSetting))
                    .Parameters.Add(New SqlParameter("@in_ColPos", intColPos))
                    .Parameters.Add(New SqlParameter("@in_CreateBy", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_CreateDt", dtCurrDate))
                    .Parameters.Add(New SqlParameter("@in_FileFormat", intFileFormat))
                    .Parameters.Add(New SqlParameter("@in_Service_Id", Service_Id))
                    .ExecuteNonQuery()
                End With

                'Destroy SQL Command Object
                cmdBankFormat = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return gc_Status_OK

            Catch

                'Destroy SQL Command Object
                cmdBankFormat = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnDB_BankFormat - clsBank", Err.Number, Err.Description)

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return gc_Status_Error

            End Try

        End Function

#End Region

#Region "BNM Retrieve"
        'This function is not able to use in this version
        'Public Shared Function fnBNM() As System.Data.DataSet
        'Create Instance of SQL Data Adaptor
        'Dim sdaBNM As SqlDataAdapter
        '    sdaBNM = New SqlDataAdapter

        ''Create Instance of generic Class Object
        'Dim clsGeneric As MaxPayroll.Generic
        '    clsGeneric = New MaxPayroll.Generic

        ''Create Instance of System Data Set
        'Dim dsBNM As System.Data.DataSet
        '    dsBNM = New System.Data.DataSet

        ''Create Instance of ASP Net Context Object
        'Dim ASPNetContext As HttpContext = HttpContext.Current

        ''Variable Declarations
        'Dim lngOrgId As Long, lngUserCode As Long

        '    Try
        '        lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
        '        lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

        ''Intialize SQL Conenction
        '        Call clsGeneric.SQLConnection_Initialize()

        ''Execute SQL Data Adaptor
        '        sdaBNM = New SqlDataAdapter("Exec pg_BNM", clsGeneric.SQLConnection)

        ''Fill Data set
        '        sdaBNM.Fill(dsBNM, "BANK")

        '        Return dsBNM

        '    Catch

        ''Log Error
        '        Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "pg_BNM", Err.Number, Err.Description)

        '        Return dsBNM

        '    Finally

        ''Destroy SQL Data Adaptor
        '        sdaBNM = Nothing

        ''Destroy Data Set
        '        dsBNM = Nothing

        ''Terminate SQL Connection
        '        Call clsGeneric.SQLConnection_Terminate()

        ''Destroy Generic Class Object
        '        clsGeneric = Nothing

        '    End Try

        'End Function
#End Region

#Region "Bank Master File"
        Public Enum enmProcedureStatus
            Add
            Modify
            Delete
            Retrieve
        End Enum
        Public Function fnBankMFDetails(ByVal eStatus As enmProcedureStatus, ByVal sBankCode As String, ByVal sBNMCode As String, ByVal sBankDesc As String) As Boolean

            'Create Instance of SQL Command Object
            Dim cmd As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            Dim lngOrgId As Long, lngUserCode As Long

            Try

                lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intitialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmd
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_Bank"
                    .CommandType = CommandType.StoredProcedure

                    .Parameters.Add(New SqlParameter("@in_Status", eStatus.ToString.Substring(0, 1)))
                    .Parameters.Add(New SqlParameter("@BankCode", sBankCode))
                    .Parameters.Add(New SqlParameter("@BNMCode", sBNMCode))
                    .Parameters.Add(New SqlParameter("@BankDesc", sBankDesc))
                    .Parameters.Add(New SqlParameter("@UserID", lngUserCode))

                    .ExecuteNonQuery()
                End With

                'Get Result
                'intRecordCount = IIf(IsNumeric(cmdDuplicate.Parameters("@out_Result").Value), cmdDuplicate.Parameters("@out_Result").Value, 0)

                'IsDuplicate = IIf(intRecordCount > 0, True, False)

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnBankMFDetails - pg_Bank '" & eStatus.ToString & "', '" & sBankCode & "', '" & sBNMCode & "', '" & lngUserCode.ToString & "'", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy SQL Command Object
                cmd = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

            Return False
        End Function
#End Region

#Region "Bank Format Details"

        Public Function fnBankDetails(ByVal lngFieldId As Long) As System.Data.DataSet

            'Create Instance of SQL Data Adaptor
            Dim sdaBank As New SqlDataAdapter

            'Create Instance of generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsBank As New System.Data.DataSet

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngOrgId As Long, lngUserCode As Long

            Try

                'Intialize SQL Conenction
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adaptor
                sdaBank = New SqlDataAdapter("Exec pg_GetBankDetails " & lngFieldId, clsGeneric.SQLConnection)

                'Fill Data set
                sdaBank.Fill(dsBank, "BANK")

                Return dsBank

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnBankDetails", Err.Number, Err.Description)

                Return dsBank

            Finally

                'Destroy SQL Data Adaptor
                sdaBank = Nothing

                'Destroy Data Set
                dsBank = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Check Duplicate For Bank File Settings"

        '****************************************************************************************************
        'Procedure Name : fnDuplicate()
        'Purpose        : Check Duplicate For Bank File Settings
        'Arguments      : Option,Check Value,Type,Start Position, End Position
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 15/10/2003
        '*****************************************************************************************************
        Public Function fnDuplicate(ByVal intBankID As Integer, ByVal strOption As String, ByVal strValue As String, ByVal strType As String, ByVal intStartPos As Int16, ByVal intEndPos As Int16, ByVal strAction As String, ByVal intFieldId As Int16, ByVal strFileType As String) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdDuplicate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            Dim IsDuplicate As Boolean, intRecordCount As Int16, lngOrgId As Long, lngUserCode As Long

            Try

                lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intitialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDuplicate
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_BankDuplicate"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_FieldId", intFieldId))
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_Value", strValue))
                    .Parameters.Add(New SqlParameter("@in_FieldType", strType))
                    .Parameters.Add(New SqlParameter("@in_StartPos", intStartPos))
                    .Parameters.Add(New SqlParameter("@in_EndPos", intEndPos))
                    .Parameters.Add(New SqlParameter("@out_Result", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Result", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                'Get Result
                intRecordCount = IIf(IsNumeric(cmdDuplicate.Parameters("@out_Result").Value), cmdDuplicate.Parameters("@out_Result").Value, 0)

                IsDuplicate = IIf(intRecordCount > 0, True, False)

                Return IsDuplicate

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnDuplicate - clsBank", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy SQL Command Object
                cmdDuplicate = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Display Body Fields"

        Public Function fnBodyFields(ByVal strFileType As String, ByVal intCustOrgID As Integer, ByVal intBankID As Integer) As System.Data.DataSet

            'Create Instance of System Data Set
            Dim dsBankBody As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adaptor Object
            Dim sdaBankBody As New SqlDataAdapter

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim lngOrgId As Long, lngUserCode As Long, strSQLStatement As String

            Try

                lngOrgId = IIf(IsNumeric(ASPNetContext.Session(gc_Ses_OrgId)), ASPNetContext.Session(gc_Ses_OrgId), 0)
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
                'intGroupId = IIf(IsNumeric(ASPNetContext.Session("SYS_GROUPID")), ASPNetContext.Session("SYS_GROUPID"), 0)
                'intBankID = clsBankMF.fncGetBankIDByGroup(intGroupId)
                'intBankID = clsBankMF.fncGetBankIDByOrgFileType(intCustOrgID, strFileType)

                'Intitialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute View
                strSQLStatement = "Exec pg_GetCustBankField '" & strFileType & "'," & intBankID.ToString

                'Execute SQL Data Adaptor
                sdaBankBody = New SqlDataAdapter(strSQLStatement, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaBankBody.Fill(dsBankBody, "BODY")

                Return dsBankBody

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnBodyFields-clsBank", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Data Adaptor
                sdaBankBody = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsBankBody = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Stop Payment Grid"

        '****************************************************************************************************
        'Procedure Name : fncStopPayment()
        'Purpose        : Populate Data Set For Stop Payment
        'Arguments      : Option,Criteria,Keyword
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 24/05/2004
        '*****************************************************************************************************
        Public Function fncStopPayment(ByVal strOption As String, ByVal strCriteria As String,
                ByVal strKeyword As String, ByVal lngOrgId As Long,
                    ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsStopPayment As New System.Data.DataSet

            'Create Instance of SQL DataAdapter
            Dim sdaStopPayment As New SqlDataAdapter

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement String
                strSQL = "Exec pg_StopPayment '" & strOption & "','" & strCriteria & "','" & strKeyword & "'"

                'Execute SQL Data Adapter
                sdaStopPayment = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaStopPayment.Fill(dsStopPayment, "STOPPAY")

                'Return Data Set
                Return dsStopPayment

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncStopPayment - clsBank", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsStopPayment = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaStopPayment = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Access Logs"

        '****************************************************************************************************
        'Procedure Name : fncAccessLog()
        'Purpose        : Populate Data Set For Access Logs
        'Arguments      : User,Option,User Login,User Name,From Date,To Date
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/11/2004
        '*****************************************************************************************************
        Public Function fncLogs(ByVal strUser As String, ByVal strModule As String, ByVal strOption As String,
                    ByVal strUserLogin As String, ByVal strUserName As String, ByVal strFromDt As String,
                            ByVal strToDt As String, ByVal lngLogId As Long) As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                If strModule = "Acc" Then
                    strSQL = "Exec pg_AccessSearch '" & strUser & "','" & strOption & "','" & strUserLogin & "','" & strUserName & "','" & strFromDt & "','" & strToDt & "'"
                ElseIf strModule = "Mod" Then
                    strSQL = "Exec pg_SearchModLogs '" & strUser & "','" & strOption & "','" & strUserLogin & "','" & strUserName & "','" & strFromDt & "','" & strToDt & "'"
                ElseIf strModule = "Trans" Then
                    strSQL = "Exec pg_ViewLogs " & lngLogId
                ElseIf strModule = "Del" Then
                    strSQL = "Exec pg_DeleteSearch '" & strOption & "','" & strUserLogin & "','" & strUserName & "','" & strFromDt & "','" & strToDt & "'"
                ElseIf strModule = "Fail" Then
                    strSQL = "Exec pg_FailAttempt_Search '" & strUser & "','" & strOption & "','" & strUserLogin & "','" & strUserName & "','" & strFromDt & "','" & strToDt & "'"
                End If

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "clsBank - fncLogs", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function



#End Region

#Region "Create Temporary Table"

        '****************************************************************************************************
        'Procedure Name : prTempTable
        'Purpose        : Create Temporary Table For File Body, Based On The Bank Format
        'Arguments      : Organisation Id,User Id File Type
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 31/10/2003
        '*****************************************************************************************************
        Public Sub prcCreateTable(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String, ByVal strTableName As String)

            'Create Instance of SQL Data Adaptor
            Dim daTempTable As New SqlDataAdapter

            'Create Instance of Data Row
            Dim drTempTable As System.Data.DataRow

            'Create Instance of Data Set
            Dim dsTempTable As New System.Data.DataSet

            'Create Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQLStatement As String
            Dim strCreateTable As String
            Dim strFieldDesc As String
            Dim intCounter As Int16
            strCreateTable = ""

            Try
                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute View
                strSQLStatement = "Exec pg_BankField '" & strFileType & "'"

                'Execute SQL Data Adaptor
                daTempTable = New SqlDataAdapter(strSQLStatement, clsGeneric.SQLConnection)

                'Fill Data Set
                daTempTable.Fill(dsTempTable, "TEMP")

                'Destroy Instance SQL Data Adaptor
                daTempTable = Nothing

                'Create Table String
                intCounter = 0

                'Read the Data Set Using Data Row - Start
                If dsTempTable.Tables("TEMP").Rows.Count > 0 Then

                    'Start to build SQL Statement To be Executed
                    strCreateTable = strCreateTable & "If Not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" & strTableName & "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                    strCreateTable = strCreateTable & " Create Table [" & strTableName & "] ([Rec Id][numeric](18, 0),[File Id][Numeric](9,0),"

                    For Each drTempTable In dsTempTable.Tables("TEMP").Rows
                        strFieldDesc = drTempTable("FDESC")
                        If intCounter = 0 Then
                            'Continue to build SQL Statement To be Executed
                            strCreateTable = strCreateTable & "[" & strFieldDesc & "][varchar](255)"
                        Else
                            'Continue to build SQL Statement To be Executed
                            strCreateTable = strCreateTable & ",[" & strFieldDesc & "][varchar](255)"
                        End If
                        intCounter = intCounter + 1
                    Next

                    'Final SQL Statement Statement
                    strCreateTable = strCreateTable & ",[Duplicate][varchar](1),[Status] [int] NULL)"

                End If
                'Read the Data Set Using Data Row - Stop

                If strCreateTable <> "" Then
                    'Execute the Build SQL Statment
                    Dim cmdTempTabl As New SqlCommand(strCreateTable, clsGeneric.SQLConnection)
                    cmdTempTabl.ExecuteNonQuery()
                End If

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcCreateTable - clsBank", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Data Set
                dsTempTable = Nothing

                'Destroy Instace of Data row
                drTempTable = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Bank Org Code"
#Region "Retrieve"
        Public Function fncGetBankOrgCode(ByVal lngOrgID As Long, ByVal strAccNo As String) As System.Data.DataSet
            Dim ds As System.Data.DataSet
            Dim sProcName As String = "pg_QryBankOrgAccounts"
            Dim Params(1) As SqlParameter

            Params(0) = New SqlParameter("In_OrgID", SqlDbType.Int)
            Params(0).Value = lngOrgID
            Params(1) = New SqlParameter("In_AccNo", SqlDbType.VarChar)
            Params(1).Value = strAccNo

            ds = SqlHelper.ExecuteDataset(Generic.sSQLConnection, CommandType.StoredProcedure, sProcName, Params)

            Return ds
        End Function
#End Region
#End Region

#Region "Bank Format Master Maintenance"

        '071121 Function created by Marcus   
        Public Function fnDB_BankFormatMaster(ByVal intBankID As Integer,
                                                ByVal strFileType As String,
                                                ByVal strTableName As String,
                                                ByVal strInboundFolder As String,
                                                ByVal strOutboundFolder As String,
                                                ByVal strResponseFolder As String,
                                                ByVal lngOrgId As Long,
                                                ByVal lngUserId As Long) As String

            Dim clsGeneric As New MaxPayroll.Generic
            Dim cmdBankFormat As New SqlCommand
            Dim dtCurrDate As Date

            Try

                'Assign Values To Declared Variables
                dtCurrDate = Now                                                              'File Type

                'Intialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdBankFormat
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_InsertBankFormatMaster"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_TableName", strTableName))
                    .Parameters.Add(New SqlParameter("@in_InFolder", strInboundFolder))
                    .Parameters.Add(New SqlParameter("@in_OutFolder", strOutboundFolder))
                    .Parameters.Add(New SqlParameter("@in_ResponseFolder", strResponseFolder))
                    .Parameters.Add(New SqlParameter("@in_CreateBy", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_CreateDt", dtCurrDate))
                    .ExecuteNonQuery()
                End With


                'Destroy SQL Command Object
                cmdBankFormat = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return gc_Status_OK

            Catch

                'Destroy SQL Command Object
                cmdBankFormat = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnDB_BankFormatMaster - clsBank", Err.Number, Err.Description)

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return gc_Status_Error

            End Try

        End Function

#End Region

#Region "Get bank format master info"
        '071121 Function created by Marcus
        Public Function fncGetBankFormatMasterInfo(ByVal intBankID As Integer, ByVal strFileType As String) As DataSet

            Dim clsGeneric As New MaxPayroll.Generic
            Dim adtBankFormat As SqlDataAdapter
            Dim dsBankFormatMaster As New DataSet
            Dim cmdBankFormat As New SqlCommand

            Call clsGeneric.SQLConnection_Initialize()

            With cmdBankFormat
                .Connection = clsGeneric.SQLConnection
                .CommandText = "pg_GetBankFormatMaster"
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
            End With

            adtBankFormat = New SqlDataAdapter(cmdBankFormat)
            adtBankFormat.Fill(dsBankFormatMaster)

            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Destroy Generic Class Object
            clsGeneric = Nothing

            Return dsBankFormatMaster

        End Function

#End Region

#Region "Check for duplicated table name"
        '071121 Functian created by Marcus
        Public Function fncIsTableNameExist(ByVal strTableName As String, ByVal lngOrgId As Long, ByVal lngUserId As Long) As String

            Dim clsGeneric As New MaxPayroll.Generic
            Dim strTableExistSql As String
            Dim intTableExist As Integer


            Try

                strTableExistSql = "select count(0) from dbo.sysobjects where id = object_id(N'[dbo].[" & strTableName & "]') and OBJECTPROPERTY(id, N'IsUserTable')=1"

                intTableExist = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection(), CommandType.Text, strTableExistSql)

                If intTableExist = 0 Then
                    Return gc_Status_OK
                Else
                    Return gc_Status_Error
                End If

            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncIsTableNameExist - clsBank", Err.Number, Err.Description)

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return "Application Error: " + ex.Message

            End Try

        End Function


#End Region

#Region "Create Table"

        '071121 Function added by Marcus
        Public Function fncCreateDBTable(ByVal strTableName As String, ByVal strFileType As String,
                                        ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal intBankId As Integer) As String

            'create instance of sql command object
            Dim cmdCreateTable As New SqlCommand

            'create instance of generic class object
            Dim clsGeneric As New MaxPayroll.Generic

            'create instance of sql data reader
            Dim sdrCreateTable As SqlDataReader

            'variable declarations
            Dim strCreateTable As String = ""
            Dim dtCurrDate As Date = Now

            Try

                'intialise sql connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdCreateTable
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "CIMBGW_CreateUploaded_Table "
                    .Parameters.Add(New SqlParameter("@in_TableName", strTableName))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankId))
                    .Parameters.Add(New SqlParameter("@in_UserID", lngUserId))
                    .ExecuteNonQuery()
                End With

                Return "Table created. Please confirm with Database Administrator."

            Catch ex As Exception

                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncDBCreateTable - clsBank", Err.Number, Err.Description)

                Return "Application Error: " + ex.Message

            Finally

                'terminate sql connection
                Call clsGeneric.SQLConnection_Terminate()

                'destroy instance of sql command object
                cmdCreateTable = Nothing

                'destroy instance of generic class object
                clsGeneric = Nothing

                'destroy instance of sql data reader
                sdrCreateTable = Nothing

            End Try

        End Function

#End Region

        '#Region "Create Table"

        '        '071121 Function added by Marcus
        '        Public Function fncCreateDBTable(ByVal strTableName As String, ByVal strFileType As String, _
        '                                        ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal intBankId As Integer) As String

        '            'create instance of sql command object
        '            Dim cmdCreateTable As New SqlCommand

        '            'create instance of generic class object
        '            Dim clsGeneric As New MaxPayroll.Generic

        '            'create instance of sql data reader
        '            Dim sdrCreateTable As SqlDataReader

        '            'variable declarations
        '            Dim intCounter As Int16
        '            Dim intFieldLength As Int16
        '            Dim strCreateTable As String = ""
        '            Dim dtCurrDate As Date = Now

        '            Try

        '                'intialise sql connection
        '                Call clsGeneric.SQLConnection_Initialize()

        '                With cmdCreateTable
        '                    .Connection = clsGeneric.SQLConnection
        '                    .CommandType = CommandType.StoredProcedure
        '                    .CommandText = "pg_GetCustDBTableField"
        '                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
        '                    .Parameters.Add(New SqlParameter("@in_BankID", intBankId))
        '                    sdrCreateTable = .ExecuteReader
        '                End With

        '                If sdrCreateTable.HasRows Then

        '                    'build create table string
        '                    'strCreateTable = strCreateTable & "If Not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" & strTableName & "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
        '                    strCreateTable = strCreateTable & " Create Table [" & strTableName & "] ([Rec Id][INT],[File Id][Numeric](9,0),"

        '                    While sdrCreateTable.Read()
        '                        'get the field size and increment by 15
        '                        intFieldLength = ((sdrCreateTable("EPOS") - sdrCreateTable("SPOS")) + 15)
        '                        'if first column
        '                        If intCounter = 0 Then
        '                            strCreateTable = strCreateTable & "[" & sdrCreateTable("FDESC") & "][varchar](" & intFieldLength & ")"
        '                        Else
        '                            strCreateTable = strCreateTable & ",[" & sdrCreateTable("FDESC") & "][varchar](" & intFieldLength & ")"
        '                        End If
        '                        'increment counter
        '                        intCounter = intCounter + 1
        '                    End While

        '                    'add more fields
        '                    strCreateTable += ",[Duplicate][varchar](1),[Status] [int] NULL)"
        '                    'Marcus: Update the  TableName and IsDBGenerated field in mCor_BankFormat
        '                    strCreateTable += " update mCor_BankFormat set TableName = @strTableName,IsDBTableGenerated =1" & _
        '                                        " ,ModifyBy = @lngUserId, ModifyDt = @dtCurrDate" & _
        '                                        " where BankID = @intBankId and FileType = @strFileType"


        '                Else
        '                    Return "Error: Bank format field(s) not found for " + strFileType
        '                End If

        '                'close reader
        '                sdrCreateTable.Close()

        '                'Build new table - start
        '                With cmdCreateTable
        '                    .Connection = clsGeneric.SQLConnection
        '                    .CommandType = CommandType.Text
        '                    .CommandText = strCreateTable
        '                    .Parameters.AddWithValue("@strTableName", strTableName)
        '                    .Parameters.AddWithValue("@intBankId", intBankId)
        '                    .Parameters.AddWithValue("@strFileType", strFileType)
        '                    .Parameters.AddWithValue("@lngUserId", lngUserId)
        '                    .Parameters.AddWithValue("@dtCurrDate", dtCurrDate)
        '                    .ExecuteNonQuery()
        '                End With
        '                'Build new table - stop

        '                Return "Table created. Please confirm with Database Administrator."

        '            Catch ex As Exception

        '                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncDBCreateTable - clsBank", Err.Number, Err.Description)

        '                Return "Application Error: " + ex.Message

        '            Finally

        '                'terminate sql connection
        '                Call clsGeneric.SQLConnection_Terminate()

        '                'destroy instance of sql command object
        '                cmdCreateTable = Nothing

        '                'destroy instance of generic class object
        '                clsGeneric = Nothing

        '                'destroy instance of sql data reader
        '                sdrCreateTable = Nothing

        '            End Try

        '        End Function

        '#End Region

#Region "Update Bank Format IsTableGenerated Field"

        Public Function fncBankFormateDBTblGenerated(ByVal intBankId As Integer, ByVal strFileType As String,
                                       ByVal lngOrgId As Long, ByVal lngUserId As Long) As String

            Dim clsGeneric As New MaxPayroll.Generic
            Dim cmdBankFormat As New SqlCommand
            Dim dtCurrDate As Date

            Try

                'Assign Values To Declared Variables
                dtCurrDate = Now                                                              'File Type

                'Intialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdBankFormat
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_BankFormatMasterDBTableGenerated"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_CreateBy", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_CreateDt", dtCurrDate))
                    .ExecuteNonQuery()
                End With


                'Destroy SQL Command Object
                cmdBankFormat = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return gc_Status_OK

            Catch

                'Destroy SQL Command Object
                cmdBankFormat = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBankFormateDBTblGenerated - clsBank", Err.Number, Err.Description)

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return gc_Status_Error

            End Try

        End Function

#End Region

#Region "Check Bank Format DB Table Generated"
        '071122 Function created by Marcus
        Public Function fncCheckBankFormatDbTblGenerated(ByVal intBankID As Integer, ByVal strFileType As String) As Boolean

            Dim clsGeneric As New MaxPayroll.Generic
            Dim strSql As String
            Dim sqlCommand As New SqlCommand
            Dim bRetVal As Boolean

            strSql = "Select isDBTableGenerated from mCor_BankFormat" &
                        " where BankID = @intBankID And FileType = @strFileType"

            Call clsGeneric.SQLConnection_Initialize()

            With sqlCommand
                .Connection = clsGeneric.SQLConnection
                .CommandType = CommandType.Text
                .CommandText = strSql
                .Parameters.AddWithValue("@intBankID", intBankID)
                .Parameters.AddWithValue("@strFileType", strFileType)
                bRetVal = .ExecuteScalar
            End With

            Call clsGeneric.SQLConnection_Terminate()

            sqlCommand = Nothing

            clsGeneric = Nothing

            Return bRetVal

        End Function
#End Region

#Region "Reports Requirement On Sep 2024"

        Public Function GetMenuName() As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec fin_GetMenu"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetAuditTrail", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

        Public Function GetVoilationReport(FromDate As String, ToDate As String, UserId As String) As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec fin_ViolationReport '" & FromDate & "','" & ToDate & "','" & UserId & "'"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetVoilationReport", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

        Public Function GetRoleRights(roleName As String) As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "EXEC fin_GetRoleRights '" & roleName & "'"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetRoleRights", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

        Public Function GetAuditTrail(FromDate As String, ToDate As String, UserId As String, UserName As String, Optional UserType As String = "") As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec fin_AuditTrail '" & FromDate & "','" & ToDate & "','" & UserId & "','" & UserName & "','" & UserType & "'"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetAuditTrail", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

        Public Function GetUserListing(FromDate As String, ToDate As String, UserId As String, UserName As String, StaffNumber As String) As Data.DataSet


            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec fin_UserListing '" & FromDate & "','" & ToDate & "','" & UserId & "','" & UserName & "','" & StaffNumber & "'"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetAuditTrail", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

        Public Function GetRoleRightsAll(ByVal menuName As String) As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec fin_GetRoleRightsAll '" + menuName + "'"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetAuditTrail", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

        Public Function GetMenuDistinct() As Data.DataSet

            'Create Instance of Data Set
            Dim dsAccessLog As New Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaAccessLog As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String
            strSQL = ""

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Select MenuName from MenuSetup Order by SeqNo"

                sdaAccessLog = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)
                sdaAccessLog.Fill(dsAccessLog, "LOGS")

                Return dsAccessLog

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "GetAuditTrail", Err.Number, Err.Description)

                Return dsAccessLog

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Data Set
                dsAccessLog = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaAccessLog = Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
