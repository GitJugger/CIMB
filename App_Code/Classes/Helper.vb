#Region "NameSpaces "

Option Strict On
Option Explicit On
Imports MaxGeneric
Imports MaxGateway
Imports MaxMiddleware
Imports System.Data.SqlClient
Imports System.Configuration.ConfigurationManager
Imports Microsoft.ApplicationBlocks.Data


#End Region

Public Class Helper

#Region "CPS-III "
    '' Get Organizations Which can applicable for Charges Storedprocedure
    'Public ReadOnly Property SQLGetChargesOrganizations() As String
    '    Get
    '        Return "EXEC " & AppSettings("SQL_GetChargesOrganizations")
    '    End Get
    'End Property
    '' Get CPS Dividen Approved File Storedprocedure
    Public ReadOnly Property GetApproveFile() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetApproveFile")
        End Get
    End Property
    '' '' Inserting of TireCharges Storedprocedure
    ''Public ReadOnly Property InsUpdTireChargesSQL() As String
    ''    Get
    ''        Return "EXEC " & AppSettings("SQL_InsUpdTire")
    ''    End Get
    ''End Property
    '' '' Getting of TireCharges Storedprocedure
    ''Public ReadOnly Property GetTireChargesSQL() As String
    ''    Get
    ''        Return "EXEC " & AppSettings("SQL_GetTire")
    ''    End Get
    ''End Property
    '' '' Inserting of FixedCharges Storedprocedure
    ''Public ReadOnly Property InsUpdFixedChargesSQL() As String
    ''    Get
    ''        Return "EXEC " & AppSettings("SQL_InsUpdFixed")
    ''    End Get
    ''End Property
    '' '' Getting of TireCharges Storedprocedure
    ''Public ReadOnly Property GetFixedChargesSQL() As String
    ''    Get
    ''        Return "EXEC " & AppSettings("SQL_GetFixed")
    ''    End Get
    ''End Property
    '' '' Getting of AllCharges Storedprocedure
    ''Public ReadOnly Property GetChargesSQL() As String
    ''    Get
    ''        Return "EXEC " & AppSettings("SQL_GetCharges")
    ''    End Get
    ''End Property
    '' '' Inserting of FileID and Charges Storedprocedure
    ''Public ReadOnly Property InsFileChargesSQL() As String
    ''    Get
    ''        Return "EXEC " & AppSettings("SQL_InsChargeFile")
    ''    End Get
    ''End Property
#End Region

#Region "Global Declarations "

    Private clsEncryption As New MaxPayroll.Encryption

    Private _SessionFileType As String = "FileType"
    Private _SessionFileTypeAction As String = "FileTypeAction"
    Private _SessionFileTypeId As String = "FileTypeId"
    Private _SessionMatchFileTypeId As String = "MatchFileTypeId"
    Private _Helper As New MaxMiddleware.Helper
#End Region

#Region "Properties "

    Private ReadOnly Property GetServerName() As String
        Get
            Return clsEncryption.fnSQLCrypt(AppSettings("SERVER"))
        End Get
    End Property

    Private ReadOnly Property GetUserName() As String
        Get
            Return clsEncryption.fnSQLCrypt(AppSettings("USERNAME"))
        End Get
    End Property

    Private ReadOnly Property GetPassword() As String
        Get
            Return clsEncryption.fnSQLCrypt(AppSettings("PASSWORD"))
        End Get
    End Property

    Private ReadOnly Property GetDatabase() As String
        Get
            Return clsEncryption.fnSQLCrypt(AppSettings("DATABASE"))
        End Get
    End Property
    '' Getting of FileSettings Storedprocedure
    Private ReadOnly Property GetFileSettingsSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetFileSettings")
        End Get
    End Property
    '' Input of FileSettings Storedprocedures starts here
    Private ReadOnly Property InputFileSettingsSQL() As String
        Get
            Return AppSettings("SQL_InputFileSettings")
        End Get
    End Property
    Private ReadOnly Property OutputFileSettingsSQL() As String
        Get
            Return AppSettings("SQL_OutputFileSettings")
        End Get
    End Property
    Private ReadOnly Property ResponseFileSettingsSQL() As String
        Get
            Return AppSettings("SQL_ResponseFileSettings")
        End Get
    End Property
    Private ReadOnly Property ReturnFileSettingsSQL() As String
        Get
            Return AppSettings("SQL_ReturnFileSettings")
        End Get
    End Property
    '' Getting of Filetype Storedprocedures Ends here

    '' Getting of Filetype Storedprocedures starts here
    Public ReadOnly Property GetInputFileTypeSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetInputFileType")
        End Get
    End Property
    Private ReadOnly Property GetOutputFileTypeSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetOutputFileType")
        End Get
    End Property
    Private ReadOnly Property GetResponseFileTypeSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetResponseFileType")
        End Get
    End Property
    Private ReadOnly Property GetReturnFileTypeSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetReturnFileType")
        End Get
    End Property
    '' Getting of Filetype Storedprocedures ends here

    '' Input of Filetype Storedprocedures starts here
    Private ReadOnly Property InputFileTypeSQL() As String
        Get
            Return AppSettings("SQL_InputFileType")
        End Get
    End Property
    Private ReadOnly Property OutputFileTypeSQL() As String
        Get
            Return AppSettings("SQL_OutputFileType")
        End Get
    End Property
    Private ReadOnly Property ResponseFileTypeSQL() As String
        Get
            Return AppSettings("SQL_ResponseFileType")
        End Get
    End Property
    Private ReadOnly Property ReturnFileTypeSQL() As String
        Get
            Return AppSettings("SQL_ReturnFileType")
        End Get
    End Property
    '' Input of Filetype Storedprocedures ends here
    Private ReadOnly Property AcnowledgeProc() As String
        Get
            Return AppSettings("SQL_Acnowledge")
        End Get
    End Property
    Private ReadOnly Property GetAcnowledgeProc() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetAcnowledge")
        End Get
    End Property
    Public ReadOnly Property GetCommonSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetCommon")
        End Get
    End Property
    '' Input of FileSettings Delete Storedprocedure
    Public ReadOnly Property FileSettingsDeleteSQL(ByVal FileType As String, _
        ByVal FieldId As Short) As String
        Get
            Return "EXEC " & AppSettings("SQL_SettingsDelete") & " '" & FileType & "'" & "," & FieldId
        End Get
    End Property
    Public ReadOnly Property TableWizardSQL(ByVal FileTypeId As Short, _
            ByVal ContentType As Short) As String
        Get
            Return "EXEC " & AppSettings("SQL_TableWizard") & " " _
                & FileTypeId & "," & ContentType
        End Get
    End Property

    Public ReadOnly Property ValidateUserData() As String
        Get
            Return "Content_Validate_Check "
        End Get
    End Property
    Public ReadOnly Property UpdateReviewerWorkflow(ByVal FieldId As Short) As String
        Get
            Return "EXEC CIMBGW_Update_ReviewerStatus " & FieldId
        End Get
    End Property

    Public ReadOnly Property GetSQLBNMCodes() As String
        Get
            Return "EXEC MPG_SelectBNMCodes "
        End Get
    End Property

    Public ReadOnly Property GetSQLInfenionInsertUPdate() As String
        Get
            Return "EXEC MPG_Insert_UpdateBNMCodes "
        End Get
    End Property

    Public ReadOnly Property GetSQLFileTypeMenu() As String
        Get
            Return "EXEC MPG_Get_FileType_Menu "
        End Get
    End Property

    Public ReadOnly Property GetSQLServiceTypeFileType() As String
        Get
            Return "EXEC MPG_Get_ServiceTypeFileType "
        End Get
    End Property

    Public ReadOnly Property SQLCutoffTime() As String
        Get
            Return "EXEC MPG_CutoffTime "
        End Get
    End Property

    Public ReadOnly Property GetSQLServiceType() As String
        Get
            Return "EXEC MPG_Get_PayService "
        End Get
    End Property

    Public ReadOnly Property GetSQLFileTypeCutoff() As String
        Get
            Return "EXEC MPG_Get_CutOffTime "
        End Get
    End Property

    Public ReadOnly Property GetSQLFileTypes() As String
        Get
            Return "EXEC MPG_GetGrpPaymentService "
        End Get
    End Property
    Public ReadOnly Property GetSQLReportDownload_FileTypes() As String
        Get
            Return "EXEC MPG_GroupReport_PaymentService "
        End Get
    End Property

    Public ReadOnly Property GetSQLCommon() As String
        Get
            Return "EXEC CIMBGW_Get_Common "
        End Get
    End Property


#End Region

#Region "Column Names "

    Public ReadOnly Property GetServiceTypeCol() As String
        Get
            Return "ServiceType"
        End Get
    End Property

    Public ReadOnly Property GetFileIdCol() As String
        Get
            Return "FileId"
        End Get
    End Property


#End Region

#Region "Enumerators "

    Public Enum FileActionType
        Insert = 1
        Update = 2
        Delete = 3
    End Enum

    Public Enum ServiceType
        Payments = 1
        Collections = 2
        Cheques = 3
        Mandates = 4
    End Enum

    Public Enum EncryptionType
        None = 0
        PTPTNEncryption = 1
        CashMgmtKitsENC = 2
    End Enum

    Public Enum CommonRequest
        Mandate_BankOrgCode = 1
    End Enum

    Public Enum DataType
        Numeric = 1
        AlphaNumeric = 2
        Money = 4
    End Enum

#End Region

#Region "Get File Type SQL "

    Public Function GetFileTypeSQL(ByVal FileType As Short) As String

        If FileType = PPS.FileType.Inward Then
            Return GetInputFileTypeSQL()
        ElseIf FileType = PPS.FileType.Outward Then
            Return GetOutputFileTypeSQL()
        ElseIf FileType = PPS.FileType.Response Then
            Return GetResponseFileTypeSQL()
        ElseIf FileType = PPS.FileType.Returned Then
            Return GetReturnFileTypeSQL()
        End If

        Return String.Empty

    End Function

#End Region

#Region "Insert/Update File Type SQL "

    Public Function FileTypeSQL(ByVal FileType As Short) As String

        If FileType = PPS.FileType.Inward Then
            Return InputFileTypeSQL()
        ElseIf FileType = PPS.FileType.Outward Then
            Return OutputFileTypeSQL()
        ElseIf FileType = PPS.FileType.Response Then
            Return ResponseFileTypeSQL()
        ElseIf FileType = PPS.FileType.Returned Then
            Return ReturnFileTypeSQL()
        End If

        Return String.Empty

    End Function

#End Region

#Region "Acnowledge Procedures"
    Public Function InsAcnowledgeSQL() As String
        Return AcnowledgeProc()
    End Function
    Public Function GetAcnowledgeSQL() As String
        Return GetAcnowledgeProc()
    End Function
#End Region

#Region "Get FileSettings SQL "

    Public Function GetFileSettings(ByVal FileType As Short, _
        ByVal FileTypeId As Short, ByVal FieldId As Short) As DataTable

        'Variable Declarations
        Dim FileTypeStr As String = Nothing, SQLStatement As String = Nothing

        Try

            'Get File Type 
            FileTypeStr = [Enum].GetName(GetType(PPS.FileType), FileType)

            'Build SQL Statement
            SQLStatement = GetFileSettingsSQL() & " '" & FileTypeStr & "'," _
                & FileTypeId & "," & FieldId

            'return results as datatable
            Return PPS.GetData(SQLStatement, GetSQLConnection, GetSQLTransaction)

        Finally

            'force garbage collection

        End Try

    End Function

#End Region

#Region "Get FileSettings SQL "

    Public Function GetMatchField(ByVal FileTypeId As Short) As DataTable

        'Variable Declarations
        Dim SQLStatement As String = Nothing

        Try

            'Build SQL Statement
            SQLStatement = GetFileSettingsSQL() & " '" & PPS.FileType.Inward.ToString() & "'," _
                & FileTypeId & ",0"

            'return results as datatable
            Return PPS.GetData(SQLStatement, GetSQLConnection, GetSQLTransaction)

        Finally

            'force garbage collection

        End Try

    End Function

#End Region

#Region "Insert/Update File Settings SQL "

    Public Function FileSettingsSQL(ByVal FileType As Short) As String

        If FileType = PPS.FileType.Inward Then
            Return InputFileSettingsSQL()
        ElseIf FileType = PPS.FileType.Outward Then
            Return OutputFileSettingsSQL()
        ElseIf FileType = PPS.FileType.Response Then
            Return ResponseFileSettingsSQL()
        ElseIf FileType = PPS.FileType.Returned Then
            Return ReturnFileSettingsSQL()
        End If

        Return String.Empty

    End Function

#End Region

#Region "Get SQL Connection String "

    Public ReadOnly Property GetSQLConnectionString() As String
        Get
            Return "SERVER=" & GetServerName & ";DATABASE=" & GetDatabase & ";UID=" _
                & GetUserName & ";PWD=" & GetPassword
        End Get
    End Property

#End Region

#Region "SQL Connection/Transaction "

    Public Function GetSQLConnection() As SqlConnection

        Dim _SqlConnection As SqlConnection = New SqlConnection(GetSQLConnectionString)
        Return _SqlConnection

    End Function

    Public Function GetSQLTransaction() As SqlTransaction

        Dim _SqlTransaction As SqlTransaction = Nothing
        Return _SqlTransaction

    End Function

#End Region

#Region "Session Properties "

    Public Property SessionFileType() As Short
        Get
            Return clsGeneric.NullToShort( _
                HttpContext.Current.Session(_SessionFileType))
        End Get
        Set(ByVal value As Short)
            HttpContext.Current.Session(_SessionFileType) = value
        End Set
    End Property
    Public Property SessionFileTypeAction() As Short
        Get
            Return clsGeneric.NullToShort( _
                HttpContext.Current.Session(_SessionFileTypeAction))
        End Get
        Set(ByVal value As Short)
            HttpContext.Current.Session(_SessionFileTypeAction) = value
        End Set
    End Property


    Public Property SessionFileTypeId() As Short
        Get
            Return clsGeneric.NullToShort(HttpContext.Current.Session(_SessionFileTypeId))
        End Get
        Set(ByVal value As Short)
            HttpContext.Current.Session(_SessionFileTypeId) = value
        End Set
    End Property

    Public Property MatchSessionFileTypeId() As Short
        Get
            Return clsGeneric.NullToShort(HttpContext.Current.Session(_SessionMatchFileTypeId))
        End Get
        Set(ByVal value As Short)
            HttpContext.Current.Session(_SessionMatchFileTypeId) = value
        End Set
    End Property

#End Region

#Region "Set Match File Type "

    Public Sub SetMatchFileType(ByVal FileType As Short, ByVal FileTypeId As Short)

        'Variable Declarations
        Dim SQLStatement As String = Nothing

        Try

            'Build SQL Statement - Start
            SQLStatement = GetCommonSQL & " '" & MaxGateway.Helper._GetCommon. _
                FileTypeId_FileType_InputFileType.ToString() & "',"
            SQLStatement &= FileType & "," & FileTypeId
            'Build SQL Statement - Stop

            MatchSessionFileTypeId = clsGeneric.NullToShort(MaxMiddleware.PPS.SQLScalarValue( _
                GetSQLConnection, GetSQLTransaction, String.Empty, SQLStatement))

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Check File Settings "
    Public Function CheckFileSettings(ByVal StartField As Integer, ByVal EndField As Integer, _
                ByVal FieldId As Integer, ByVal Content As Short) As Short
        'Variable Declarations
        Dim SQLStatement As String = Nothing
        Dim Filetype As String = Nothing
        If SessionFileType = PPS.FileType.Inward Then
            Filetype = PPS.FileType.Inward.ToString()
        ElseIf SessionFileType = PPS.FileType.Outward Then
            Filetype = PPS.FileType.Outward.ToString()
        ElseIf SessionFileType = PPS.FileType.Response Then
            Filetype = PPS.FileType.Response.ToString()
        ElseIf SessionFileType = PPS.FileType.Returned Then
            Filetype = PPS.FileType.Returned.ToString()
        End If

        Try
            'Build SQL Statement - Start

            SQLStatement = GetCommonSQL & " 'Check_FileSettings'," & " '" & Filetype & "',"

            SQLStatement &= StartField & "," & EndField & "," & FieldId & "," & Content & "," & SessionFileTypeId
            'Build SQL Statement - Stop

            Return clsGeneric.NullToShort(MaxMiddleware.PPS.SQLScalarValue( _
                GetSQLConnection, GetSQLTransaction, String.Empty, SQLStatement))

        Catch ex As Exception

        End Try


    End Function
#End Region

#Region "Reading from Web.config "

#Region "Autopay SNA"

    Public Function AutopaySNAExt() As String
        Dim SNAExt As String = Nothing
        SNAExt = clsGeneric.NullToString(AppSettings("SNAExt"))
        Return SNAExt
    End Function

    Public Function AutopaySNAStartName() As String
        Dim SNAExt As String = Nothing
        SNAExt = clsGeneric.NullToString(AppSettings("SNAStartName"))
        Return SNAExt
    End Function
    Public Function AutopaySNADetailRecordtype() As String
        Dim RecordType As String = Nothing
        RecordType = clsGeneric.NullToString(AppSettings("BankFileSettings_DetailRecordType"))
        Return RecordType
    End Function
    Public ReadOnly Property SQLGetOrgcodeForOrgId() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetOrgcodeForOrgId")
        End Get
    End Property
#End Region

#Region "Autopay File"

    Public Function AutopayExt() As String
        Dim SNAExt As String = Nothing
        SNAExt = clsGeneric.NullToString(AppSettings("AutopayExt"))
        Return SNAExt
    End Function

    Public Function AutopayStartName() As String
        Dim SNAExt As String = Nothing
        SNAExt = clsGeneric.NullToString(AppSettings("AutopayStartName"))
        Return SNAExt
    End Function
#End Region

#Region "Direct Debit File"

    Public Function DDebitExt() As String
        Dim SNAExt As String = Nothing
        SNAExt = clsGeneric.NullToString(AppSettings("DDebitExt"))
        Return SNAExt
    End Function

    Public Function DDebitStartName() As String
        Dim SNAExt As String = Nothing
        SNAExt = clsGeneric.NullToString(AppSettings("DDebitStartName"))
        Return SNAExt
    End Function
#End Region

#Region "CPS Dividend File/SFF/Delimited Dividend"

    Public Function CPSDividendExt() As String
        Dim CPSDivExt As String = Nothing
        CPSDivExt = clsGeneric.NullToString(AppSettings("CPSDividendExt"))
        Return CPSDivExt
    End Function
    Public Function CPSDividendStartName() As String
        Dim CPSDivStart As String = Nothing
        CPSDivStart = clsGeneric.NullToString(AppSettings("CPSDividendStartName"))
        Return CPSDivStart
    End Function
    Public Function CPSDelim_DividendStartName() As String
        Dim CPSDivStart As String = Nothing
        CPSDivStart = clsGeneric.NullToString(AppSettings("CPSDelimStartName"))
        Return CPSDivStart
    End Function
    Public Function CPSDelim_DividendExt() As String
        Dim CPSDivExt As String = Nothing
        CPSDivExt = clsGeneric.NullToString(AppSettings("CPSDelimDividendExt"))
        Return CPSDivExt
    End Function

    Public Function CPSSFFStartName() As String
        Dim CPSDivStart As String = Nothing
        CPSDivStart = clsGeneric.NullToString(AppSettings("CPSSFFStartName"))
        Return CPSDivStart
    End Function
    Public Function CPSSFFExt() As String
        Dim CPSDivExt As String = Nothing
        CPSDivExt = clsGeneric.NullToString(AppSettings("CPSSFFExt"))
        Return CPSDivExt
    End Function
#End Region

#Region "CPS Member File"
    Public Function CPSMemberExt() As String
        Dim CPSDivExt As String = Nothing
        CPSDivExt = clsGeneric.NullToString(AppSettings("CPSMemberExt"))
        Return CPSDivExt
    End Function
    Public Function CPSMemberStartName() As String
        Dim CPSDivStart As String = Nothing
        CPSDivStart = clsGeneric.NullToString(AppSettings("CPSMemberStartName"))
        Return CPSDivStart
    End Function
#End Region

#Region "Common Properties "

    Public ReadOnly Property LineLimit() As Short
        Get
            Return clsGeneric.NullToShort(ConfigurationManager.AppSettings("LineLimit"))
        End Get
    End Property

    Public ReadOnly Property WebStartName() As String
        Get
            Return clsGeneric.NullToString(ConfigurationManager.AppSettings("WebFileStartName"))
        End Get
    End Property

#End Region

#End Region

#Region "Get File Original Name "
    Public Function fncGetFileName(ByVal FileGivenName As String, ByVal FileType As String) As String

        Dim strSQL As String = "", SqlStatement As String = Nothing
        Dim sqlParams(1) As SqlParameter, arrFile() As String = Nothing
        Dim clsGeneric As New MaxPayroll.Generic
        Dim dsRetVal As New DataSet
        Dim dtRetval As New DataTable
        Dim FileName As String = ""
        Try

            'If file type is PTPTN-START
            If FileType = HybridAutoPaySNA_Name() Or FileType = HybridDirectDebit_Name() Or _
                 FileType = HybridMandate_Name() Then

                arrFile = Split(FileGivenName, ".")

                'Check file Extension -start
                If arrFile(UBound(arrFile)) = PTPTNReportExt Then

                    If arrFile.Length > 0 Then
                        FileGivenName = arrFile(LBound(arrFile))
                    End If

                    'Build sql statement
                    SqlStatement = GetSQLCommon & "H2H_InbounFileName ," & FileGivenName
                    FileGivenName = MaxGeneric.clsGeneric.NullToString((PPS.SQLScalarValue _
                        (GetSQLConnection, GetSQLTransaction, String.Empty, SqlStatement)))
                    FileGivenName = FileGivenName & ".txt"
                End If
                'Check file Extension -stop

            End If

            'If file type is PTPTN-STOP

            sqlParams(0) = New SqlParameter("@File_Given_Name", SqlDbType.VarChar)
            sqlParams(0).Value = FileGivenName
            sqlParams(1) = New SqlParameter("@File_Type", SqlDbType.VarChar)
            sqlParams(1).Value = FileType

            ''dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_GetFileType", sqlParams)

            FileName = CStr(SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_GetFileName", sqlParams))

        Catch Ex As SystemException

            clsGeneric.ErrorLog("pg_GetFileName", Err.Number, Err.Description)
        Finally



            'Destory current instance of efpx
            clsGeneric = Nothing

        End Try
        '' dtRetval = dsRetVal.Tables(0)
        ''FileName = dtRetval.Rows(0)(0)
        Return FileName
    End Function
#End Region

#Region "Config Setting Report Dir "

    Public Shared Function ReportDownloaderPage() As String

        If FormHelp.IsBlank(clsGeneric.NullToString(
            ConfigurationManager.AppSettings("REPORT_DOWNLOADER_PAGE"))) Then

            Return clsGeneric.NullToString("pg_DownloadCIMBReport.aspx")

        Else

            Return clsGeneric.NullToString(ConfigurationManager.AppSettings("REPORT_DOWNLOADER_PAGE"))

        End If

    End Function

    Public ReadOnly Property ReportAutopayConfig() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("AutopayCIMBReturnReport")
        End Get
    End Property

    Public ReadOnly Property ReportAutopaySNAConfig() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("AutopaySNACIMBReturnReport")
        End Get
    End Property
    Public ReadOnly Property ReportDDebitConfig() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("DDebitCIMBReturnReport")
        End Get
    End Property
    Public ReadOnly Property ReportHDDebitConfig() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("HDDebitCIMBReturnReport")
        End Get
    End Property
    Public ReadOnly Property ReportHAPSConfig() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("HAPSCIMBReturnReport")
        End Get
    End Property
    Public ReadOnly Property ReportHMNDConfig() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("HMNDCIMBReturnReport")
        End Get
    End Property

    Public ReadOnly Property PTPTNReportExt() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("PTPTNReportExt")
        End Get
    End Property
#End Region

#Region "File Type Name Centralize"

    Public Function Autopay_Name() As String
        Dim Name As String = Nothing
        Name = "Autopay File"
        Return Name
    End Function
    Public Function CPSDividen_Name() As String
        Dim Name As String = Nothing
        Name = "CPS Dividend"
        Return Name
    End Function
    Public Function CPSDelimited_Dividen_Name() As String
        Dim Name As String = Nothing
        Name = "Delimited Dividend"
        Return Name
    End Function
    Public Function CPSMember_Name() As String
        Dim Name As String = Nothing
        Name = "CPS Member"
        Return Name
    End Function
    Public Function AutopaySNA_Name() As String
        Dim Name As String = Nothing
        Name = "AutopaySNA File"
        Return Name
    End Function
    Public Function CPSSingleFileFormat_Name() As String
        Dim Name As String = Nothing
        Name = "Single File Format"
        Return Name
    End Function

    Public Function DirectDebit_Name() As String
        Dim Name As String = Nothing
        Name = "Direct Debit"
        Return Name
    End Function
    Public Function MandateFile_Name() As String
        Dim Name As String = Nothing
        Name = "Mandate File"
        Return Name
    End Function

    Public Function CPS_Name() As String
        Dim Name As String = Nothing
        Name = "CPS File"
        Return Name
    End Function

    Public Function PayLinkPayRoll_Name() As String
        Dim Name As String = Nothing
        Name = "PayLinkPayroll"
        Return Name
    End Function

    Public Function HybridDirectDebit_Name() As String
        Dim Name As String = Nothing
        Name = "Hybrid Direct Debit"
        Return Name
    End Function
    Public Function HybridAutoPaySNA_Name() As String
        Dim Name As String = Nothing
        Name = "Hybrid AutopaySNA"
        Return Name
    End Function
    Public Function HybridMandate_Name() As String
        Dim Name As String = Nothing
        Name = "Hybrid Mandate"
        Return Name
    End Function


#End Region

#Region "File Setting Start Name (Header/Footer)"
    Public Function DirectDebit_Header() As String
        Dim Name As String = Nothing
        Name = "DDDRH"
        Return Name
    End Function
    Public Function DirectDebit_Footer() As String
        Dim Name As String = Nothing
        Name = "DDDRT"
        Return Name
    End Function
#End Region

#Region "Send Mail"

    Public Sub Uploader_SendMail(ByVal OrgId As Integer, ByVal UserId As Integer, ByVal GroupId As Integer, _
            ByVal strFileType As String, ByVal strGivenName As String, ByVal dtvaluedate As Date)
        Dim strSubject As String
        Dim strbody As String

        Dim _clsCom As New MaxPayroll.clsCommon
        ''Mail Distribution Start
        strSubject = strFileType & " Uploaded - " & strGivenName & ", Upload Date: " & dtvaluedate
        'Build Body
        strbody = "The " & strFileType & " has been successfully uploaded on " & Format(Today, "dd/MM/yyyy") & " at " & TimeOfDay
        'Send Mails To Group Reviewers/Authorizers
        Call _clsCom.prcSendMails("CUSTOMER", OrgId, UserId, GroupId, strSubject, strbody)
        ''Mail Distribution Stop
    End Sub
#End Region
    ''Teja CPS Upload Start
#Region "CPS-III "

#Region "Store Procedures "

    '' Get File Relation  Storedprocedure
    Public ReadOnly Property Get_FileRelationSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetFileRelation") & " "
        End Get
    End Property
    '' Get Mem/Div File Relation  Storedprocedure
    Public ReadOnly Property Get_MemDivRelationSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_MemDivRelation") & " "
        End Get
    End Property


    Public ReadOnly Property SQLFileDetails() As String
        Get
            Return "EXEC CIMBGW_Tpgt_FileDetails "
        End Get
    End Property


    ''------------------
    '' Inserting of TireCharges Storedprocedure
    Public ReadOnly Property InsUpdTireChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_InsUpdTire")
        End Get
    End Property
    '' Getting of TireCharges Storedprocedure
    Public ReadOnly Property GetTireChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetTire")
        End Get
    End Property
    '' Inserting of FixedCharges Storedprocedure
    Public ReadOnly Property InsUpdFixedChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_InsUpdFixed")
        End Get
    End Property
    '' Getting of TireCharges Storedprocedure
    Public ReadOnly Property GetFixedChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetFixed")
        End Get
    End Property
    '' Getting of AllCharges Storedprocedure
    Public ReadOnly Property GetChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetCharges")
        End Get
    End Property
    '' Inserting of FileID and Charges Storedprocedure
    Public ReadOnly Property InsFileChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_InsChargeFile")
        End Get
    End Property
    '' Getting of CustomerID,BankFormatID  Storedprocedure
    Public ReadOnly Property GetCustIDSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetCustID")
        End Get
    End Property

    '' Get the previous charges i.e last deleted charges Storedprocedure
    Public ReadOnly Property GetPreviousChargesSQL() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetPreviousCharges")
        End Get
    End Property

    '' Get Organizations Which can applicable for Charges Storedprocedure
    Public ReadOnly Property SQLGetChargesOrganizations() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetChargesOrganizations")
        End Get
    End Property

    Public ReadOnly Property SQLGetFieldDesc() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetFieldDesc")
        End Get
    End Property
    Public ReadOnly Property SQLGetFieldsColumn() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetFieldsColumn")
        End Get
    End Property
    Public ReadOnly Property SQLGetFileType() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetFileType")
        End Get
    End Property
    Public ReadOnly Property SQLAssignFields() As String
        Get
            Return "EXEC " & AppSettings("SQL_AssignFields")
        End Get
    End Property

    Public ReadOnly Property SQLFileTypeSettings() As String
        Get
            Return AppSettings("SQL_FileTypeSettings")
        End Get
    End Property
    Public ReadOnly Property SQLWebColumnFieldRel() As String
        Get
            Return AppSettings("SQL_WebColumnFieldRel")
        End Get
    End Property

#End Region

#Region "Enumerators "


    Enum enmCPSOptions
        Cheque_No
        Compare_Field
        Gross_Amount
        Tax
        Net_Amount
        Total_Net
        Body_Header
        Body_Body
        Body_Footer
        Header_Identifier
        Body_Identifier
        Footer_Identifier
        Trailer_Indicator
        Body_Header_Identifier
        CPS_Cheque_Col_Name_CN
        Null_Field_NF


    End Enum

    Public Enum ChargeType
        Fixed = 1
        Tier = 2
    End Enum


#End Region

#Region "Column Names And Fixed Names "

    Public ReadOnly Property ChargeTypeCol() As String
        Get
            Return "CHARGE_TYPE"    ''''1 for Fixed charges and 2 for Tier charges
        End Get
    End Property
    Public ReadOnly Property FixedChargesCol() As String
        Get
            Return "Fixed_Charges"
        End Get
    End Property
    Public ReadOnly Property FileIdCol() As String
        Get
            Return "FileId"
        End Get
    End Property
    Public ReadOnly Property FileTypeIdCol() As String
        Get
            Return "FileTypeId"
        End Get
    End Property
    Public ReadOnly Property TableToInsertCol() As String
        Get
            Return "TableToInsert"
        End Get
    End Property
    Public ReadOnly Property ChargeIDCol() As String
        Get
            Return "CHARGE_ID"
        End Get
    End Property
    Public ReadOnly Property TransactionToCol() As String
        Get
            Return "Trans_To"
        End Get
    End Property
    Public ReadOnly Property TransactionChargeCol() As String
        Get
            Return "TRANS_CHARGE"
        End Get
    End Property
    Public ReadOnly Property TransactionFromCol() As String
        Get
            Return "Trans_From"
        End Get
    End Property

    Public ReadOnly Property TierMaxDisplayCol() As String
        Get
            Return AppSettings("TierMaxDisplay")
        End Get
    End Property
    Public ReadOnly Property FileTypeCol() As String
        Get
            Return "FileType"
        End Get
    End Property
    Public ReadOnly Property BankFieldIdCol() As String
        Get
            Return "Bank_Field_Id"
        End Get
    End Property
    Public ReadOnly Property BankFieldDescCol() As String
        Get
            Return "Bank_Field_Desc"
        End Get
    End Property
    Public ReadOnly Property COLUMNNAMECol() As String
        Get
            Return "COLUMN_NAME"
        End Get
    End Property
#End Region

    Public ReadOnly Property FileUploadedPath() As String
        Get
            Return AppSettings("PATH")
        End Get
    End Property

    Public ReadOnly Property CPSDividendUploadedFolder() As String
        Get
            Return AppSettings("CPS_DIVIDEND_UPLOADED")
        End Get
    End Property
    Public ReadOnly Property CPSMemberUploadedFolder() As String
        Get
            Return AppSettings("CPS_MEMBER_UPLOADED")
        End Get
    End Property
    Public ReadOnly Property CPSDelimitedUploadedFolder() As String
        Get
            Return AppSettings("CPS_DELIMITED_UPLOADED")
        End Get
    End Property
    Public ReadOnly Property CPSSFFUploadedFolder() As String
        Get
            Return AppSettings("CPS_SFF_UPLOADED")
        End Get
    End Property



#Region "FileType Names "
    Public ReadOnly Property CPSMemberFileName() As String
        Get
            Return "CPS Member File"
        End Get
    End Property
    Public ReadOnly Property DivTotalNetCol() As String
        Get
            Return AppSettings("Div_TotalNet_Col")
        End Get
    End Property
    Public ReadOnly Property Delimited_DivTotalNetCol() As String
        Get
            Return "Total_Net_Div"
        End Get
    End Property
    Public ReadOnly Property SFF_DivTotalNetCol() As String
        Get
            Return "Transaction Amount"
        End Get
    End Property

    Public ReadOnly Property Infenion_PaymentCol() As String
        Get
            Return "Payment Amount"
        End Get
    End Property

#End Region


#End Region
    ''Teja CPS Upload End
#Region "Get Org Code For Org ID "

    Public Function GetOrgCodeForOrgId(ByVal OrgId As Long) As DataTable

        Dim OrgCodeTable As New DataTable

        Try

            OrgCodeTable = PPS.GetData(SQLGetOrgcodeForOrgId & " " & OrgId, _
                GetSQLConnection, GetSQLTransaction)

            Return OrgCodeTable

        Catch ex As Exception

        End Try

    End Function

#End Region

#Region "Get Array Count "

    'Author         : Bhanu teja V - T-Melmax Sdn Bhd
    'Purpose        : To Get the Number of Arrays For Specific Number of Lines
    'Created        : 19/03/2010
    Public Function GetArrayCount(ByVal LinesCount As Integer) As Short

        'Variable Declaration - Start
        Dim StartDigits As Short = 0, LastDigits As Short = 0
        Dim ArraysCount As String = Nothing, StringCounter As Short = 0
        'Variable Declaration - Stop

        'Get the String Counter to assign the array - start
        ArraysCount = clsGeneric.NullToString(clsGeneric.NullToDecimal(LinesCount) / clsGeneric.NullToInteger(LineLimit))

        'Check the division contain any decimal or Not
        If ArraysCount.IndexOf(".") > 0 Then
            'Get the Value up to decimal
            StartDigits = clsGeneric.NullToShort(Mid(ArraysCount, 1, ArraysCount.IndexOf(".")))
            'get the value after decimal
            LastDigits = clsGeneric.NullToShort(Mid(ArraysCount, ArraysCount.IndexOf(".") + 2, 4))

            'check if division does't have reminder and value >1
            If LastDigits = 0 And StartDigits > 1 Then
                StringCounter = clsGeneric.NullToShort(StartDigits - 1)
            Else
                StringCounter = StartDigits
            End If

        Else
            StringCounter = clsGeneric.NullToShort(clsGeneric.NullToInteger(ArraysCount) - 1)
        End If

        Return StringCounter

    End Function

#End Region

End Class
