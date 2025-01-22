Imports System.IO
Imports System.Data
Imports MaxPayroll.Generic
Imports MaxCrypt.clsCrypto
Imports System.Data.SqlClient
Imports MaxPayroll.Encryption
Imports MaxPayroll.clsCustomer
Imports Microsoft.ApplicationBlocks.data
Imports MaxReadWrite



Namespace MaxPayroll


'****************************************************************************************************
'Class Name     : clsUpload
'ProgId         : MaxPayroll.clsUpload
'Purpose        : Upload Functions Used For Upload Module
    'Author         : Sujith Sharatchandran -  
    'Created        : 27/10/2003
    '*****************************************************************************************************
    Public Class clsUpload
        'Private _clsCPSPhase3 As New clsCPSPhase3

#Region "Global Variable"

        Public strConnection As String
        Public SQLConnection As SqlConnection

        Dim gintCP38Recs As Short
        Dim gstrErrorMsg As String = "", gdblTotalEmpAmt As Double, gdblTotalEmrAmt As Double, gintPCBRecs As Short
        Dim glngHashTotal As Long, gdblTotalAmount As Double, IsFileReject As Boolean, gintRowNum As Int16
        Dim gstrFileBatchNo As String, gstrSeqNumber As String, gstrFileSeqNumber As String, gdblPCBHash As Double

        Dim gstrPreBatchRef As String = "", gintTotalBatchHeader As Integer, gstrPreBatchLine As String = ""
        Dim gIntErrorMsgRowNo As Integer
        'to be access by fncBody for Direct Debit to validate the mandate
        Dim gStrBankOrgCode As String = ""

        Dim TotalAccountNumber As Long
        Dim TotalAmount As Long

        Private _Helper As New Helper()
        Dim clsCommon As New MaxPayroll.clsCommon                      'Create Instance of Common Class Object
#End Region

#Region "SQL Connection"

        '****************************************************************************************************
        'Procedure Name : SQLConnection_Initialize()
        'Purpose        : Create SQL Connection
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 06/11/2003
        '*****************************************************************************************************
        Public Sub SQLConnection_Initialize()

            'Create Instance of Encryption Class Object 
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim strServerName As String, strUserName As String, strPassword As String, strDatabase As String

            Try

                'Get SQL Details - Start
                strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("USERNAME"))
                strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("PASSWORD"))
                strServerName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("SERVER"))
                strDatabase = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("DATABASE"))
                strConnection = "SERVER=" & strServerName & ";DATABASE=" & strDatabase & ";UID=" & strUserName & ";PWD=" & strPassword
                'Get SQL Details - Stop

                'Check If SQL Connection is Not Initialized
                If SQLConnection Is Nothing Then
                    'Create Connection Object
                    SQLConnection = New SqlConnection(strConnection)
                    'Open Connection
                    SQLConnection.Open()
                ElseIf SQLConnection.State = ConnectionState.Closed Then
                    'Open Connection
                    SQLConnection.Open()
                End If

            Catch ex As Exception

            Finally

                'Destroy Instance of Encryption Class Object
                clsEncryption = Nothing

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : SQLConnection_Terminate()
        'Purpose        : Destroy SQL Connection
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/08/2003
        '*****************************************************************************************************
        Public Sub SQLConnection_Terminate()
            'Check SQL connection Object is Created 
            If Not SQLConnection Is Nothing Then
                'If Connection Object Created and Open
                If SQLConnection.State = ConnectionState.Open Then
                    SQLConnection.Close()       'Close Connection
                    SQLConnection = Nothing     'Destroy Connection Object
                    'If Connection Object Created and Closed
                ElseIf SQLConnection.State = ConnectionState.Closed Then
                    SQLConnection = Nothing     'Destroy Connection
                End If
            End If
        End Sub

#End Region

#Region "On Init/Exit"

        Private Sub prInit()

            'Empty Global Variables
            gintRowNum = 0
            glngHashTotal = 0
            gdblTotalAmount = 0
            IsFileReject = False

        End Sub

        Private Sub prExit()

            'Empty Global Variables
            gintRowNum = 0
            glngHashTotal = 0
            gdblTotalAmount = 0
            IsFileReject = False

        End Sub

#End Region

#Region "Validate Uploaded File"

        '****************************************************************************************************
        'Function Name  : fnValidateFile
        'Purpose        : Validate the Contents of the Uploded File
        'Arguments      : File Name
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/10/2003
        '*****************************************************************************************************
        Public Function fncValidateFile(ByVal strFileName As String, ByVal strSubFileName As String, ByVal lngFormatId As Long, _
        ByVal intValueDay As Int16, ByVal intValueMonth As Int16, ByVal intValueYear As Int16, ByVal strAccNumber As String, _
        ByVal IsICCheck As Boolean, ByVal IsDuplicate As Boolean, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
        ByVal strFileType As String, ByVal strIPAddr As String, ByVal intConMonth As Int16, ByVal intConYear As Int16, _
        ByVal intGroupId As Int32, ByVal lngAccId As Long, ByRef IsAlert As Boolean, ByVal strCheck As String, _
        ByVal dcTranCharge As Decimal, ByVal bContributeMonth As Boolean, Optional ByVal intState As Int16 = 0, _
        Optional ByVal strTestStatus As String = "", Optional ByVal strSerAccNo As String = "", _
        Optional ByVal intSerAccId As Int16 = 0) As String

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strResult As String, strFilePrefix As String = "", CatchMessage As String = Nothing



            'Initialize Global Variables
            Call prInit()

            Try

                'Create Bank Format File - Start
                strResult = ""

                ''CPS Phase3 Start
                If strFileType = _Helper.CPSMember_Name Or strFileType = _Helper.CPSDividen_Name _
                    Or strFileType = _Helper.CPSDelimited_Dividen_Name Or _
                        strFileType = _Helper.CPSSingleFileFormat_Name Or _
                        strFileType = _Helper.PayLinkPayRoll_Name Then

                    'strResult = _clsCPSPhase3.CPS_Upload_File(strFileName, strSubFileName, strFileType, intValueDay, intValueMonth, _
                    'intValueYear, lngOrgId, lngUserId, intGroupId, lngAccId, lngFormatId, dcTranCharge, strIPAddr, strAccNumber)

                    ''CPS Phase3 End
                Else

                    strResult = fncCreateFile(strSubFileName, strFileName, strFileType, lngFormatId, lngOrgId, lngUserId, _
                        strIPAddr, intValueDay, intValueMonth, intValueYear, IsICCheck, IsDuplicate, intConMonth, strAccNumber, _
                        intConYear, intGroupId, lngAccId, IsAlert, strCheck, dcTranCharge, bContributeMonth, intState, _
                        strTestStatus, strSerAccNo, intSerAccId, CatchMessage)
                End If

                If strResult = gc_Status_Error Then
                    If strFileType = _Helper.CPSMember_Name Or strFileType = _Helper.CPSDividen_Name Then
                        strResult = gc_Status_Error

                    Else
                        strResult = CatchMessage & Environment.NewLine & "Upload Failed. Please try again. If the Problem Persists, Please contact " & gc_Const_CompanyName & " registration centre at " & gc_Const_CompanyContactNo & "."
                    End If
                End If

                'Create Bank Format File - Stop

                Return strResult

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncValidateFile - clsUpload", Err.Number, Err.Description)

                'Return Error Message
                Return "Upload Failed. Please try again. If the Problem Persists, Please contact " & gc_Const_CompanyName & " registration centre at " & gc_Const_CompanyContactNo & "."


            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Initialize Global Variables
                Call prExit()

            End Try

        End Function

#End Region

#Region "Get Field ID"

        '****************************************************************************************************
        'Function Name  : fnGetFieldId
        'Purpose        : Get the Requested Field Id
        'Arguments      : Request,Content Type
        'Return Value   : Field Id
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Private Function fnGetFieldId(ByVal strRequest As String, ByVal strContentType As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String) As Int16

            'Create Instance of SQL Command Object
            Dim cmdField As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intFieldId As Int16

            Try

                'Intialize SQL Connection
                Call SQLConnection_Initialize()

                With cmdField
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_GetFieldId"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Request", strRequest))
                    .Parameters.Add(New SqlParameter("@in_Content", strContentType))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@out_FieldId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_FieldId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                'Get Field Id
                intFieldId = IIf(IsNumeric(cmdField.Parameters("@out_FieldId").Value), cmdField.Parameters("@out_FieldId").Value, 0)

                'Destroy SQL Command Object
                cmdField = Nothing

                'Terminate SQL Connection
                Call SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return intFieldId

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnGetFieldId -  clsUpload", Err.Number, Err.Description)

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL command Object
                cmdField = Nothing

                Return 0

            End Try


        End Function

#End Region

#Region "Create Bank Format File"


        '****************************************************************************************************
        'Function Name  : fnCreateFile
        'Purpose        : Get Requested Details
        'Arguments      : File Extension
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/10/2003

        '*****************************************************************************************************

        Private Function fncCreateFile(ByRef strSubFileName As String, ByVal strFileName As String, ByVal strFileType As String, _
            ByVal lngFormatId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strIPAddr As String, _
                ByVal intValueDay As Int16, ByVal intValueMonth As Int16, ByVal intValueYear As Int16, ByVal IsICCheck As Boolean, _
                    ByVal IsDuplicate As Boolean, ByVal intConMonth As Int16, ByVal strAccNumber As String, ByVal intConYear As Int16, _
                        ByVal intGroupId As Int32, ByVal lngAccId As Long, ByRef IsAlert As Boolean, ByVal strCheck As String, _
                            ByVal dcTranCharge As Decimal, ByVal bContributeMonth As Boolean, Optional ByVal intState As Int16 = 0, _
                                Optional ByVal strTestStatus As String = "", Optional ByVal strSerAccNo As String = "", _
                                    Optional ByVal intSerAccId As Int16 = 0, Optional ByRef CatchMessage As String = Nothing) As String



            Dim drXLS As DataRow                                           'Create Instance Data Row Object
            Dim drFormat As DataRow                                        'Create Instance Data Row Object
            Dim dsColPos As New DataSet                                    'Create Instance of Data Set 
            Dim flReader As StreamReader                                   'Create Instance File Reader Object
            Dim flWriter As StreamWriter                                   'Create Instance File Writer Object
            Dim dsFileFormat As New DataSet                                'Create Instance Data Set Object
            Dim dsBodyFormat As New DataSet                                'Create Instance of Data Set Object
            Dim clsGeneric As New MaxPayroll.Generic                       'Create Instance Generic Class Object
            'Dim clsCommon As New MaxPayroll.clsCommon                      'Create Instance of Common Class Object
            Dim clsEncryption As New MaxPayroll.Encryption                 'Create Instance Encryption Class Object

            'Variable Declaration

            Dim IsError As Boolean
            Dim strIc As String
            Dim strFiller As String
            Dim IsLimit As Boolean
            Dim dtValueDate As Date
            Dim IsPutFile As Boolean
            Dim strFormat As String = ""
            Dim strDelimiter As String = ""
            Dim intFind As Int16
            Dim DateString As String = Nothing
            Dim strHeaderContent As String = ""
            Dim strBodyContent As String
            Dim strBodyDetailContent As String
            Dim strFooterContent As String = ""
            Dim intRecordCount As Int32
            Dim strSubject As String
            Dim strBody As String
            Dim strWholeBody As String = ""
            Dim strDecimal As String
            Dim strGivenName As String
            Dim lngFileId As Long
            Dim strHashTotal As String = ""
            Dim intCounter As Int16
            Dim strFile As String
            Dim IsCreate As Boolean
            Dim strFileLine As String
            Dim strWriteFolder As String
            Dim arrDecimal As String()
            Dim strCryptPassword As String
            Dim intCheck As Int16
            Dim intTotalCols As Int16
            Dim IsBlank As Boolean
            Dim IsHeader As Boolean
            Dim IsFooter As Boolean
            Dim intHeaderLines As Int16
            Dim intFooterLines As Int16
            Dim intLineCount As Integer
            Dim strTableName As String = ""
            Dim strCrypt As String
            Dim strCryptFilePath As String
            Dim strCryptFile As String
            Dim strConMonth As String
            Dim intBankID As Integer
            Dim intHour As Integer
            Dim intMinute As Integer
            Dim intSecond As Integer
            Dim intMilisecond As Integer
            Dim strBankHeaderContent As String = ""
            Dim strUploadedHeader As String = ""
            Dim strUploadedFooter As String = "", strFilePrefix As String = ""
            Dim strDebitAccNumber As String
            'CPS Dividend File Start
            Dim CPScount As Integer = 0
            Dim CPSChargesFooter As String = ""
            Dim ContentValidateCount As Integer = 0
            Dim FilePreFix As String = Nothing

            Try

                IsError = False
                IsCreate = True
                IsPutFile = False
                intRecordCount = 0

                If IsICCheck Then
                    strIc = "Y"
                Else
                    strIc = "N"
                End If

                'Get File Details - START
                dsFileFormat = clsCommon.fncGetRequested("File Settings", lngOrgId, lngUserId, lngFormatId, "")
                If dsFileFormat.Tables(0).Rows.Count > 0 Then
                    For Each drFormat In dsFileFormat.Tables(0).Rows
                        strFormat = drFormat("FFORMAT")                                                 'Get File Format
                        IsHeader = IIf(drFormat("FHEADER") = "Y", True, False)                          'Get Header Flag
                        IsFooter = IIf(drFormat("FFOOTER") = "Y", True, False)                          'Get Footer Flag
                        intHeaderLines = IIf(IsNumeric(drFormat("FHLINES")), drFormat("FHLINES"), 0)    'Get Header Lines
                        intFooterLines = IIf(IsNumeric(drFormat("FFLINES")), drFormat("FFLINES"), 0)    'Get Footer Lines
                        strDelimiter = IIf(IsDBNull(drFormat("FDELIM")), "", drFormat("FDELIM"))        'Get Delimiter
                    Next
                End If
                'Get File Details - STOP

                'Marcus: to initialize the file error line number for fncBody
                gIntErrorMsgRowNo = intHeaderLines

                strTableName = clsUpload.fncGetDBTableName(lngFormatId)

                'Get File Name Specified By Customer
                strGivenName = clsCommon.fncFileName(strFileName, False)
                'Replace Org Id With Empty Space
                'strGivenName = Replace(strGivenName, lngOrgId & "_", "")
                'strGivenName = strGivenName.Substring(7)

                'Get Contribution Month
                'If strFileType = "EPF File" Or strFileType = "SOCSO File" Or strFileType = "LHDN FILE" Then
                If bContributeMonth = False Then
                    strConMonth = ""
                Else

                    strConMonth = MonthName(intConMonth, True) & " " & intConYear
                End If

                'Get Value Date - Start
                dtValueDate = intValueDay & "/" & intValueMonth & "/" & intValueYear
                'Get Value Date - Stop


                intHour = Now.Hour
                intMinute = Now.Minute
                intSecond = Now.Second
                intMilisecond = Now.Millisecond

                'Generate File Name

                'If strFileType = "EPF File" AndAlso strTestStatus = "T" Then
                '    strFile = "TA03" & Format(intValueDay, "00") & Format(intValueMonth, "00") & intValueYear.ToString.Substring(2, 2) & Format(intHour, "00") & Format(intMinute, "00") & Format(intSecond, "00") & ".txt"
                'ElseIf strFileType = "EPF File" Then
                '    strFile = "FA03" & Format(intValueDay, "00") & Format(intValueMonth, "00") & intValueYear.ToString.Substring(2, 2) & Format(intHour, "00") & Format(intMinute, "00") & Format(intSecond, "00") & ".txt"
                If strFileType = _Helper.CPS_Name Then
                    strFilePrefix = lngOrgId & lngUserId & Format(intValueDay, "00") & Format(intValueMonth, "00") & Format(intValueYear, "00") & Format(intHour, "00") & Format(intMinute, "00") & Format(intSecond, "00") & Format(intMilisecond, "000")

                    strFile = strFilePrefix & "_payment.txt"
                Else
                    strFile = lngOrgId & Format(intValueDay, "00") & Format(intValueMonth, "00") & Format(intValueYear, "00") & Format(intHour, "00") & Format(intMinute, "00") & Format(intSecond, "00") & Format(intMilisecond, "000") & ".txt"
                End If


                'Populate Col Pos Data Set
                dsColPos = clsCommon.fncGetRequested("Field ColPos", lngOrgId, lngUserId, lngFormatId, "")

                'Populate Body Format
                'intBankID = clsBankMF.fncGetBankIDByGroup(intGroupId)
                intBankID = ConfigurationManager.AppSettings("DefaultBankCode")
                dsBodyFormat = clsCommon.fncGetRequested("BANK FORMAT", lngOrgId, lngUserId, 0, strFileType, "B", intBankID)


                'Remarks : CPS P3 added
                If strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.DirectDebit_Name _
                OrElse strFileType = _Helper.AutopaySNA_Name Then

                    Dim strRetVal As String
                    strRetVal = clsCommon.fncFileDetailsSeqNo("ADD", 0, strFileType, strGivenName, strFile, lngOrgId, _
                    lngUserId, dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, lngFormatId, strIc, _
                    intSerAccId, dcTranCharge)

                    If Not strRetVal = "0" Then
                        Dim int1stStartIndex As Integer = strRetVal.IndexOf(",") + 1
                        Dim int2ndStartIndex As Integer = strRetVal.LastIndexOf(",") + 1

                        gstrSeqNumber = strRetVal.Substring(int1stStartIndex, strRetVal.LastIndexOf(",") - int1stStartIndex)
                        gstrFileSeqNumber = strRetVal.Substring(int2ndStartIndex)
                        lngFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))
                    Else
                        Return "Error ocurred while inserting file information to database." & Environment.NewLine & "Please check your Inbox or File Status Report for confirmation or contact " & gc_Const_CompanyName & " registration center at " & gc_Const_CompanyContactNo & "."
                    End If


                ElseIf strFileType = "Payroll File" Then

                    Dim strRetVal As String
                    strRetVal = clsCommon.fncFileDetailsRefNo("ADD", 0, strFileType, strGivenName, strFile, lngOrgId, lngUserId, dtValueDate, "", 0, "", "0", "0", "", intGroupId, lngAccId, lngFormatId, strIc, intSerAccId, dcTranCharge)

                    If Not strRetVal = "0" Then
                        gstrFileBatchNo = strRetVal.Substring(strRetVal.IndexOf(",") + 1)
                        lngFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))
                    Else
                        Return "Error ocurred while inserting file information to database." & Environment.NewLine & "Please check your Inbox or File Status Report for confirmation or contact " & gc_Const_CompanyName & " registration center at " & gc_Const_CompanyContactNo & "."
                    End If


                Else
                    lngFileId = clsCommon.fncFileDetails("ADD", 0, strFileType, strGivenName, strFile, lngOrgId, lngUserId, dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, lngFormatId, strIc, intSerAccId, dcTranCharge)
                    If lngFileId = 0 Then
                        Return "Error ocurred while inserting file information to database." & Environment.NewLine & "Please check your Inbox or File Status Report for confirmation or contact " & gc_Const_CompanyName & " registration center at " & gc_Const_CompanyContactNo & "."
                    End If
                End If

                'Get File Body Contents - Start
                'If Positioned File
                If strFormat = "POS" Then

                    If System.IO.File.Exists(strFileName) Then

                        intCounter = 1
                        intLineCount = fncLineCount(strFileName)
                        intLineCount = (intLineCount - intFooterLines)


                        'flReader = File.OpenText(strFileName)
                        flReader = New StreamReader(strFileName, System.Text.Encoding.Default)

                        Dim strPreviousFileLine As String = ""
                        Dim bIsSameBatch As Boolean = False
                        Dim strDetailRefNo As String = ""


                        While flReader.Peek <> -1
                            If intCounter <= intHeaderLines Then
                                If intCounter = 1 Then
                                    strUploadedHeader = flReader.ReadLine
                                    If strFileType = _Helper.DirectDebit_Name Then
                                        gStrBankOrgCode = strUploadedHeader.Substring(ConfigurationManager.AppSettings("DDOrgCodeStartPOS") - 1, ConfigurationManager.AppSettings("DDOrgCodeLength")).Trim
                                        UpdateOrgCode(gStrBankOrgCode, lngFileId)
                                        ''Hafeez added to check header -START
                                        If Not strUploadedHeader.StartsWith("01") Then
                                            '_clsCPSPhase3.DeleteTransaction(lngFileId, clsCPSPhase3.enmTableName.tpgt_FileDetails.ToString)
                                            Return "There is an Error in your Body Record Type Value"
                                        End If
                                        ''Hafeez added to check header -END
                                    End If
                                Else
                                    strFileLine = flReader.ReadLine
                                End If
                                'marcus: below line to be review
                            ElseIf intCounter > intLineCount And Not strFileType = _Helper.Autopay_Name And Not strFileType = _Helper.AutopaySNA_Name Then
                                If Not (strFileType = _Helper.Autopay_Name Or strFileType = "Payroll File" Or strFileType = _Helper.AutopaySNA_Name) Then
                                    If intFooterLines <> 0 Then
                                        strUploadedFooter = flReader.ReadLine

                                        ''Hafeez added to check footer -START
                                        If strFileType = _Helper.DirectDebit_Name Then
                                            If Not strUploadedFooter.StartsWith("03") Then
                                                '_clsCPSPhase3.DeleteTransaction(lngFileId, clsCPSPhase3.enmTableName.tpgt_FileDetails.ToString)
                                                Return "There is an Error in your Footer Record Type Value"
                                            End If
                                        End If
                                        ''Hafeez added to check footer -END
                                    End If
                                    Exit While
                                End If

                            Else

                                If strFileType = "Payroll File" Then

                                    Dim strTempFileLine As String

                                    strTempFileLine = flReader.ReadLine

                                    If strTempFileLine.StartsWith("FLTRL") Then
                                        Exit While
                                    ElseIf strTempFileLine.StartsWith("BTHDR") Then

                                        bIsSameBatch = False
                                        gintTotalBatchHeader += 1
                                        strPreviousFileLine = strTempFileLine
                                        strTempFileLine = flReader.ReadLine
                                        strFileLine = strPreviousFileLine & strTempFileLine

                                    Else

                                        bIsSameBatch = True
                                        strFileLine = strPreviousFileLine & strTempFileLine

                                    End If

                                ElseIf (strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name) Then

                                    Dim strTempFileLine As String

                                    strTempFileLine = flReader.ReadLine
                                    'Footer line    
                                    If strTempFileLine.StartsWith("03") Then
                                        strUploadedFooter = strTempFileLine
                                        Exit While
                                        'Batch Header line(Body line)
                                    ElseIf strTempFileLine.StartsWith("02") Then
                                        'If strTempFileLine.Length < 230 Then
                                        'Return "Please chekout some fields are incorrect  in the body of  your file"
                                        ' End If
                                        bIsSameBatch = False
                                        strDetailRefNo = ""
                                        gintTotalBatchHeader += 1
                                        strPreviousFileLine = strTempFileLine

                                        'if Detail Reference Number not exist.
                                        If (strTempFileLine.Length <= 197) OrElse (strTempFileLine.Length > 197 AndAlso strTempFileLine.Substring(197, 5).Trim = "") Then
                                            strFileLine = strPreviousFileLine
                                        Else
                                            'if Detail Reference Number exist, read the following detail line.
                                            strDetailRefNo = strTempFileLine.Substring(197, 5)
                                            strTempFileLine = flReader.ReadLine

                                            'Check whether detail line exist, Details Reference Number exist and identical with Details Reference Number identical with batch header line.
                                            If Not strTempFileLine.StartsWith("00") Then
                                                Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                                                IsCreate = False
                                                Return "There is an Error in your file (Payment Detail Record not found for " & strDetailRefNo & "). Please check your File Format and try again"
                                            ElseIf Not strDetailRefNo = strTempFileLine.Substring(2, 5) Then
                                                Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                                                IsCreate = False
                                                Return "There is an Error in your file (Payment Reference Number not exist/not identical). Please check your File Format and try again"
                                            End If
                                            'Concatenate the batch header line and detail line for fncBody to perfrom validation and insert the data into database.
                                            strFileLine = strPreviousFileLine & strTempFileLine
                                        End If
                                    Else
                                        'if another detail line exist
                                        bIsSameBatch = True
                                        'Check whether Details Reference Number exist and identical with Details Reference Number identical with batch header line.
                                        If strDetailRefNo.Trim = "" OrElse strDetailRefNo <> strTempFileLine.Substring(2, 5) Then
                                            Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                                            IsCreate = False
                                            Return "There is an Error in your file (Payment Reference Number not exist/not identical). Please check your File Format and try again"
                                        End If
                                        'Concatenate the previous batch header line with detail line for fncBody to perfrom validation and insert the data into database.
                                        strFileLine = strPreviousFileLine & strTempFileLine

                                    End If

                                ElseIf strFileType = _Helper.DirectDebit_Name Then
                                    ''Added on 20/10/2009 to check bodystart 02 - START

                                    strFileLine = flReader.ReadLine

                                    Dim DDErrorMsg As String = ""


                                    ''body
                                    If intCounter < fncLineCount(strFileName) Then
                                        If strFileLine <> "" Then
                                            If Not strFileLine.StartsWith("02") Then
                                                DDErrorMsg = "Line " & intCounter & " - There is an Error in your Body Record Type Value"
                                            End If
                                        End If
                                    End If



                                    If DDErrorMsg <> "" Then
                                        '_clsCPSPhase3.DeleteTransaction(lngFileId, clsCPSPhase3.enmTableName.tpgt_FileDetails.ToString)
                                        Return DDErrorMsg
                                    End If


                                    ''Added on 20/10/2009 to check bodystart 02 - END
                                Else
                                    strFileLine = flReader.ReadLine
                                End If

                                strBodyContent = fncBody(strFileLine, strFormat, "", lngOrgId, lngUserId, strFileType, lngFileId, drXLS, IsICCheck, intCounter - 1, strGivenName, 0, strTableName, intValueMonth, intValueYear, lngFormatId, dsColPos, dsBodyFormat, strSerAccNo, intConMonth, intConYear, intGroupId, intBankID, intValueDay, intSerAccId, bIsSameBatch)

                                'If Application Error
                                If strBodyContent = gc_Status_Error Then
                                    'Delete failed Trans
                                    Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                                    IsCreate = False
                                    Return "There is an Error in your file. Please check your File Format and try again"
                                ElseIf Not IsFileReject Then
                                    If Len(Trim(strWholeBody)) = 0 Then
                                        strWholeBody = strWholeBody & strBodyContent
                                    Else
                                        If Not strBodyContent = "" Then
                                            strWholeBody = strWholeBody & vbCrLf & strBodyContent
                                        End If
                                    End If
                                End If

                                If Not bIsSameBatch Then
                                    intRecordCount = intRecordCount + 1
                                End If

                            End If

                            intCounter = intCounter + 1




                        End While


                        strWholeBody = strWholeBody & vbCrLf
                        flReader.Close()

                    End If

                    'If Delimited File
                ElseIf strFormat = "DELIM" Then

                    'Initialise Counter
                    intCounter = 0

                    If System.IO.File.Exists(strFileName) Then
                        flReader = New StreamReader(strFileName, System.Text.Encoding.Default)
                        'File.OpenText(strFileName)
                        While flReader.Peek <> -1
                            strFileLine = flReader.ReadLine
                            strBodyContent = fncBody(strFileLine, strFormat, strDelimiter, lngOrgId, _
                                                lngUserId, strFileType, lngFileId, drXLS, IsICCheck, intCounter, _
                                                    strGivenName, 0, strTableName, intValueMonth, intValueYear, _
                                                        lngFormatId, dsColPos, dsBodyFormat, strSerAccNo, intConMonth, intConYear, intGroupId)
                            'If Application Error
                            If strBodyContent = gc_Status_Error Then
                                'Delete failed Trans
                                Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                                IsCreate = False
                                Return "There is an Error in your file. Please check your File Format and try again"
                            ElseIf Not IsFileReject Then
                                If Len(Trim(strWholeBody)) = 0 Then
                                    strWholeBody = strWholeBody & strBodyContent
                                Else
                                    strWholeBody = strWholeBody & vbCrLf & strBodyContent
                                End If
                            End If
                            intCounter = intCounter + 1
                            intRecordCount = intRecordCount + 1
                        End While
                        strWholeBody = strWholeBody & vbCrLf
                        flReader.Close()
                    End If

                    'If Excel File
                ElseIf strFormat = "COL" Then

                    'Create Excel Connection
                    Dim strConnXls As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strFileName & ";Extended Properties='Excel 8.0;HDR=NO;IMEX=1';"

                    'Dim strConnXls As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & strFileName & ";Extended Properties='Excel 12.0;HDR=NO; IMEX=1';"
                    Dim conXls As New OleDb.OleDbConnection(strConnXls)
                    Dim sdaXls As New OleDb.OleDbDataAdapter("Select * From [Sheet1$]", conXls)
                    Dim dsXLS As DataSet = New DataSet
                    sdaXls.Fill(dsXLS, "SHEET")

                    If dsXLS.Tables("SHEET").Rows.Count > 0 Then
                        For intCounter = 0 To dsXLS.Tables("SHEET").Rows.Count - 1
                            intTotalCols = 0
                            drXLS = dsXLS.Tables("SHEET").Rows(intCounter)

                            IsBlank = fnBlank(drXLS, lngOrgId, lngUserId, intTotalCols)
                            If Not IsBlank Then
                                strBodyContent = fncBody("strFileLine", strFormat, "", lngOrgId, lngUserId, strFileType, _
                                                    lngFileId, drXLS, IsICCheck, intCounter, strGivenName, intTotalCols, _
                                                           strTableName, intValueMonth, intValueYear, _
                                                                lngFormatId, dsColPos, dsBodyFormat, strSerAccNo, intConMonth, intConYear, intGroupId, intBankID)
                                If strBodyContent = gc_Status_Error Then
                                    'Delete failed Trans
                                    Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                                    IsCreate = False
                                    conXls = Nothing
                                    strConnXls = Nothing
                                    Return "There is an Error in your file. Please check your File Format and try again"
                                ElseIf Not IsFileReject Then
                                    strWholeBody = strWholeBody & strBodyContent & vbCrLf

                                    If strFileType = "Payroll File" Then
                                        If Not strBodyContent.Substring(0, 1904) = gstrPreBatchLine Then
                                            gintTotalBatchHeader += 1
                                        End If
                                        gstrPreBatchLine = strBodyContent.Substring(0, 1904)
                                    End If

                                End If
                                intRecordCount = intRecordCount + 1
                            End If
                        Next
                    End If

                    conXls = Nothing
                    strConnXls = Nothing

                End If
                'Get File Body Contents - Stop

                'added by victor
                If strWholeBody = "" Then
                    'Delete failed Trans
                    Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                    Return gc_Status_Error
                End If


                'if payroll file
                If strFileType = "Multiple Bank" Then
                    'gdblTotalAmount = System.Math.Round(gdblTotalAmount, 2)
                    'glngHashTotal = (glngHashTotal / 100) + gdblTotalAmount
                    strHashTotal = Right(glngHashTotal, 15)
                ElseIf strFileType = _Helper.DirectDebit_Name Then
                    strHashTotal = glngHashTotal
                    'fncDDebitHashTotal(Format(glngHashTotal, "0000000000000"))
                ElseIf strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                    strHashTotal = (gdblTotalAmount * 100) + intRecordCount

                    'if epf file or LHDN File
                ElseIf strFileType = "Payroll File" Then

                    strHashTotal = gdblTotalAmount + gintTotalBatchHeader + intRecordCount
                End If
                If Not IsFileReject Then
                    'Check Auth With Required Limit - Start
                    IsLimit = clsCommon.fncBusinessRules("LIMIT", lngOrgId, lngUserId, intGroupId, gdblTotalAmount)
                    If Not IsLimit Then
                        Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                        Return "The Total amount of the " & strFileType & " exceeds the Validation Limit of the available " & gc_UT_AuthDesc & "s"
                    End If
                    'Check Auth With Required Limit - Stop
                End If


                'Prefix With Zeros To Meet Length - start
                If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Or strFileType = _Helper.DirectDebit_Name Then
                    strFiller = fnFiller(strHashTotal, 1, 15, True)
                    strHashTotal = strFiller & strHashTotal
                    '    'ElseIf (strFileType = "EPF File" Or strFileType = "EPF Test File") Then
                    '    '    strFiller = fnFiller(strHashTotal, 1, 13, True)
                    '    '    strHashTotal = strFiller & strHashTotal
                    '    'ElseIf strFileType = "SOCSO File" Then
                    '    '    strFiller = fnFiller(strHashTotal, 1, 13, True)
                    '    '    strHashTotal = strFiller & strHashTotal
                    '    'ElseIf strFileType = "LHDN File" Then
                    '    '    strFiller = fnFiller(strHashTotal, 1, 13, True)
                    '    '    strHashTotal = strFiller & strHashTotal
                End If
                'Prefix With Zeros To Meet Length - stop



                'Marcus: If EPF or LHDN File Type then get the content of Bank Header
                'If strFileType = "EPF File" OrElse strFileType = "LHDN File" Then

                '    strBankHeaderContent = fncHeaderFooter("N", lngOrgId, lngUserId, strFileType, gdblTotalAmount, _
                '                    gdblTotalEmpAmt, gdblTotalEmrAmt, intRecordCount, strFile, strHashTotal, _
                '                        intValueDay, intValueMonth, intValueYear, intConMonth, _
                '                                strAccNumber, intConYear, strIc, IsError, dcTranCharge, intBankID, intGroupId, strFormat, _
                '                                        intState, strTestStatus, strSerAccNo, gintPCBRecs, gintCP38Recs, intSerAccId, _
                '                                            intHour, intMinute, intSecond, intMilisecond)

                'End If

                Dim sBankOrgCode As String = ""
                Dim strStateCode As String = ""
                Dim strContactPerson As String = ""
                Dim strContactNumber As String = ""
                Dim strFileIndicator As String = ""
                Dim strSeqNumber As String = ""
                Dim strRefNumber As String = ""
                Dim strTestingMode As String = ""
                Dim strEmail As String = ""
                Dim strEmployerName As String = ""

                'Get File Header Contents - Start
                strHeaderContent = fncHeaderFooter("H", lngOrgId, lngUserId, strFileType, gdblTotalAmount, _
                                    gdblTotalEmpAmt, gdblTotalEmrAmt, intRecordCount, strFile, strHashTotal, _
                                    intValueDay, intValueMonth, intValueYear, intConMonth, _
                                    strAccNumber, intConYear, strIc, IsError, dcTranCharge, intBankID, intGroupId, strFormat, _
                                    intState, strTestStatus, strSerAccNo, gintPCBRecs, gintCP38Recs, intSerAccId, _
                                    intHour, intMinute, intSecond, intMilisecond, sBankOrgCode, strUploadedHeader, , _
                                    strStateCode, strContactPerson, strContactNumber, strFileIndicator, strSeqNumber, _
                                    strRefNumber, strTestingMode, strEmail, strEmployerName)


                'If IsError Then
                '    'Delete failed Trans
                '    Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                '    Return strHeaderContent
                'Else
                '    strHeaderContent = strUploadedHeader
                'End If
                ''Get File Header Contents - Stop

                'Get File Footer Contents - Start

                strFooterContent = fncHeaderFooter("F", lngOrgId, lngUserId, strFileType, gdblTotalAmount, _
                                   gdblTotalEmpAmt, gdblTotalEmrAmt, intRecordCount, strFile, strHashTotal, _
                                   intValueDay, intValueMonth, intValueYear, intConMonth, strAccNumber, _
                                   intConYear, strIc, IsError, dcTranCharge, intBankID, intGroupId, _
                                   strFormat, intState, strTestStatus, strSerAccNo, gintPCBRecs, _
                                   gintCP38Recs, intSerAccId, , , , , sBankOrgCode, , strUploadedFooter, _
                                   strStateCode, strContactPerson, strContactNumber, strFileIndicator, _
                                   strSeqNumber, strRefNumber, strTestingMode, strEmail, strEmployerName)



                'Retrun Error Msg from Header, Body and Footer if any.
                If IsFileReject Or IsError Or ((strFileType = _Helper.Autopay_Name Or strFileType = _Helper.AutopaySNA_Name) And (strUploadedHeader.Length <> 73 Or strUploadedFooter.Length <> 21)) Then
                    Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                    Dim sRetVal As String = ""
                    sRetVal = "Upload Failed: " & Environment.NewLine


                    If strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name Then
                        If Not (strHeaderContent.StartsWith("01") Or strHeaderContent = "") Then
                            sRetVal += strHeaderContent
                        End If
                        If IsFileReject Then
                            sRetVal += gstrErrorMsg
                        End If
                        If Not (strFooterContent.StartsWith("03") Or strFooterContent = "") Then
                            sRetVal += strFooterContent
                        End If
                        'If strUploadedHeader.Length <> 73 Then
                        '    sRetVal += "File Header Record's length is invalid" & gc_BR
                        'End If

                        'If strUploadedFooter.Length <> 21 Then
                        '    sRetVal += "File Footer Record's length is invalid" & gc_BR
                        'End If
                        'ElseIf strFileType = "EPF File" Then
                        '    If Not (strHeaderContent.StartsWith("01") Or strHeaderContent = "") Then
                        '        sRetVal += strHeaderContent
                        '    End If
                        '    If IsFileReject Then
                        '        sRetVal += gstrErrorMsg
                        '    End If
                        '    If Not (strFooterContent.StartsWith("99") Or strFooterContent = "") Then
                        '        sRetVal += strFooterContent
                        '    End If
                        'ElseIf strFileType = "LHDN" OrElse strFileType = "ZAKAT" Then
                        '    If Not (strHeaderContent.StartsWith("H") Or strHeaderContent = "") Then
                        '        sRetVal += strHeaderContent
                        '    End If
                        '    If IsFileReject Then
                        '        sRetVal += gstrErrorMsg
                        '    End If

                    ElseIf strFileType = _Helper.DirectDebit_Name Then
                        If Not (strHeaderContent.StartsWith(_Helper.DirectDebit_Header) Or strHeaderContent = "") Then
                            sRetVal += strHeaderContent
                        End If
                        If IsFileReject Then
                            sRetVal += gstrErrorMsg
                        End If
                        If Not (strFooterContent.StartsWith(_Helper.DirectDebit_Footer) Or strFooterContent = "") Then
                            sRetVal += strFooterContent
                        End If
                    Else
                        If IsFileReject Then
                            sRetVal += gstrErrorMsg
                        End If
                    End If

                    Return sRetVal
                    'If Not strHeaderContent.StartsWith("01") Then
                    '    Return "Upload Failed:" & Environment.NewLine & gstrErrorMsg & strHeaderContent
                    'ElseIf Not strFooterContent.StartsWith("03") Then
                    '    Return "Upload Failed:" & Environment.NewLine & gstrErrorMsg & strFooterContent
                    'End If

                End If
                If strFileType = _Helper.Autopay_Name Then
                    strHeaderContent = strUploadedHeader
                    strFooterContent = strUploadedFooter
                ElseIf strFileType = _Helper.AutopaySNA_Name Then
                    strHeaderContent = strUploadedHeader & Space(77)
                    strFooterContent = strUploadedFooter & Space(129)
                End If


                'To get MD5 Hash Code for body content
                Dim objMD5 As New clsMD5
                Dim strMD5Hash As String
                strMD5Hash = objMD5.getMd5Hash(strHeaderContent + strWholeBody + strFooterContent)

                'File Dublication Check using MD5 Hashed Value - start

                'File Dublication Check using MD5 Hashed Value- stop

                'To check if any file with the same amount uploaded - start
                If strCheck = "N" Then
                    Dim sTemp As String = ""

                    intCheck = clsCommon.fncUploadCheck("TOTAL AMOUNT", lngOrgId, lngUserId, intGroupId, strFileType, dtValueDate, gdblTotalAmount)
                    If intCheck > 0 Then
                        sTemp = "- There has already a file been Uploaded with the same Total Amount." & Environment.NewLine
                    End If

                    intCheck = objMD5.fncMD5Validation(lngOrgId, lngUserId, strFileType, strMD5Hash)
                    If intCheck > 0 Then
                        sTemp += "- There has already a file been Uploaded with the same content." & Environment.NewLine
                    End If

                    If Len(sTemp) > 0 Then
                        IsAlert = True
                        Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)
                        Return "Please Note:" & Environment.NewLine & sTemp & "If you wish to proceed, please Enter your Validation Code and Confirm."
                    End If
                End If
                'To check if any file with the same amount uploaded - stop

                FilePrefix = Format(intValueYear, "0000") & Format(intValueMonth, "00") & Format(intValueDay, "00") & _
                                     Format(intHour, "00") & Format(intMinute, "00") _
                                    & Format(intSecond, "00") & Format(intMilisecond, "000") & Format(CInt(gstrSeqNumber), "000")

                If strFileType = _Helper.Autopay_Name Then

                    strFile = _Helper.AutopayStartName & lngOrgId.ToString & sBankOrgCode & FilePrefix & _Helper.AutopayExt

                ElseIf strFileType = _Helper.DirectDebit_Name Then

                    strFile = _Helper.DDebitStartName & lngOrgId.ToString & sBankOrgCode & FilePrefix & _Helper.DDebitExt

                    strHashTotal = strFooterContent.Substring(26, 15)

                    '17/12/2008 - AutopaySNA Output Filename format
                ElseIf strFileType = _Helper.AutopaySNA_Name Then

                    strFile = _Helper.AutopaySNAStartName & lngOrgId.ToString & sBankOrgCode & FilePrefix & _Helper.AutopaySNAExt
                    'added for CPS Phase III-start
                    'ElseIf strFileType = _Helper.CPSDividen_Name Then
                    '    strFile = _Helper.CPSDividendStartName & lngOrgId.ToString & sBankOrgCode & _
                    '                Format(intValueYear, "0000") & Format(intValueMonth, "00") & _
                    '                Format(intValueDay, "00") & Format(CInt(gstrSeqNumber), "000") & _Helper.CPSDividendExt

                End If


                ' Call Validation Content Check - Start by Naresh on 13-04-10
                If strFileType = _Helper.Autopay_Name Or strFileType = _Helper.AutopaySNA_Name Or strFileType = _Helper.DirectDebit_Name Then

                    ContentValidateCount = ContentDuplicateCheck(sBankOrgCode, dtValueDate, gdblTotalAmount, intRecordCount)
                    If ContentValidateCount > 0 Then
                        'Delete failed Trans
                        Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)

                        Return "Duplicate Content file Organization code, Total Amount, Total transactions With Same Payment Date"
                    End If
                    ' Call Validation Content Check - Stop by Naresh on 13-04-10

                End If

                'Update File Details
                If Len(strSubFileName & "") = 0 Then
                    lngFileId = clsCommon.fncFileDetails("UPDATE", lngFileId, strFileType, strGivenName, strFile, lngOrgId, _
                        lngUserId, dtValueDate, strHashTotal, intRecordCount, Math.Round(gdblTotalAmount, 2), gdblTotalEmpAmt, _
                        gdblTotalEmrAmt, strConMonth, intGroupId, lngAccId, lngFormatId, , , , intRecordCount + gintTotalBatchHeader + 2, _
                        gintTotalBatchHeader, intRecordCount + gintTotalBatchHeader, strMD5Hash, sBankOrgCode.Trim, , strStateCode.Trim, _
                        strContactPerson.Trim, strContactNumber.Trim, strFileIndicator.Trim, strSeqNumber.Trim, strRefNumber.Trim, _
                        strTestingMode.Trim, strEmail.Trim, strEmployerName.Trim)
                Else
                    lngFileId = clsCommon.fncFileDetails("UPDATE", lngFileId, strFileType, strGivenName, strFile, lngOrgId, lngUserId, _
                    dtValueDate, strHashTotal, intRecordCount, Math.Round(gdblTotalAmount, 2), gdblTotalEmpAmt, gdblTotalEmrAmt, _
                    strConMonth, intGroupId, lngAccId, lngFormatId, , , , intRecordCount + gintTotalBatchHeader + 2, _
                    gintTotalBatchHeader, intRecordCount + gintTotalBatchHeader, strMD5Hash, sBankOrgCode.Trim, _
                    strFilePrefix & "_invoice.txt")
                End If


                'Create The File
                'If Not strFileType = "Payroll File" AndAlso strFileType <> _Helper.CPS_Name Then

                If strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.DirectDebit_Name OrElse _
                    strFileType = _Helper.AutopaySNA_Name Then '17/12/2008 - hafeez create file after approved AutopaySNA


                    strWriteFolder = clsCommon.fncFolder(strFileType, "CREATE", lngOrgId, lngUserId)
                    strWriteFolder = strWriteFolder & "\" & strFile
                    flWriter = File.CreateText(strWriteFolder)

                    If Len(strBankHeaderContent) > 0 Then
                        flWriter.WriteLine(strBankHeaderContent)
                    End If

                    '071008- IF Then Else Condition added by Marcus.
                    'Purpose: Some of the file format do not require Header content. 
                    If Len(strHeaderContent) > 0 Then
                        flWriter.WriteLine(strHeaderContent)
                    End If

                    ''Write Body
                    flWriter.Write(strWholeBody)

                    '071008- IF Then Else Condition added by Marcus.
                    'Purpose: Some of the file format do not require Footer content. 
                    If Len(strFooterContent) > 0 Then
                        flWriter.WriteLine(strFooterContent)
                    End If

                    'flWriter.Write(strWholeBody)

                    flWriter.Flush()
                    flWriter.Close()
                ElseIf strFileType = _Helper.CPS_Name Then '_Helper.Autopay_Name Then

                    strWholeBody = Nothing
                    If File.Exists(strFileName) Then
                        strWriteFolder = clsCommon.fncFolder(strFileType, "CREATE", lngOrgId, lngUserId)
                        strWriteFolder = strWriteFolder & "\" & strFile
                        File.Copy(strFileName, strWriteFolder)
                        If strSubFileName <> "" Then
                            strWriteFolder = clsCommon.fncFolder(strFileType, "CREATE", lngOrgId, lngUserId)
                            strWriteFolder += "\" & strFilePrefix & "_invoice.txt"
                            File.Copy(strSubFileName, strWriteFolder)
                        End If
                    Else
                        Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strFileType)
                        'Update Error Log
                        Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnCreateFile - clsUpload", 0, lngFileId & " - Error copying file to Created Folder or file in Uploaded Folder does not exist")
                        'Return Error
                        Return "Uploaded file not found. Please contact " & gc_Const_CompanyName & " Registration Center."
                    End If

                End If


                'Insert Work FLow Details
                Call clsCommon.prcWorkFlow(lngFileId, 0, lngOrgId, lngUserId, "U", "N", "", strIPAddr, gdblTotalAmount, intGroupId, "Y")

                'Build Subject
                If strFileType = "Payroll File" OrElse strFileType = _Helper.DirectDebit_Name OrElse strFileType = _Helper.Autopay_Name _
                    OrElse strFileType = _Helper.CPS_Name OrElse strFileType = _Helper.AutopaySNA_Name Then


                    If strFileType = _Helper.CPS_Name Then
                        strGivenName = strGivenName.Substring(strGivenName.LastIndexOf(".") - 7)
                    End If

                    strSubject = strFileType & " Uploaded - " & strGivenName & ", Payment Date: " & dtValueDate
                    'Build Body
                    strBody = "The " & strFileType & " with Payment date " & dtValueDate & " has been successfully uploaded on " & Format(Today, "dd/MM/yyyy") & " at " & TimeOfDay
                Else
                    strSubject = strFileType & " Uploaded - " & strGivenName & ", Upload Date: " & dtValueDate
                    'Build Body
                    strBody = "The " & strFileType & " has been successfully uploaded on " & Format(Today, "dd/MM/yyyy") & " at " & TimeOfDay
                End If

                'Send Mails To Group Reviewers/Authorizers

                Call clsCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, intGroupId, strSubject, strBody)

                'Check for Duplicate Account Numbers
                If strFileType = "Payroll File" And IsDuplicate Then
                    Call clsCommon.prcDuplicate(lngFileId, lngOrgId, lngUserId, strFileType)
                End If

                Return gc_Status_OK

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnCreateFile - clsUpload", Err.Number, Err.Description)

                'Delete failed Trans
                Call clsCommon.prcDeleteTrans(lngFileId, lngOrgId, lngUserId, strTableName)

                'Assign Catch Exception
                CatchMessage = Err.Description()

                Return gc_Status_Error

            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Data Row
                drXLS = Nothing

                'Close The text File 
                If Not flReader Is Nothing Then
                    flReader.Close()
                End If

                'Destroy File reader Object
                flReader = Nothing

                'Destroy File Writer Object
                flWriter = Nothing

                'Destroy instance of data set
                dsBodyFormat = Nothing

                'Destroy Instance of Data Set
                dsFileFormat = Nothing

                'Destroy Instance of Encryption Class Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Update WorkFlow Table"

        '****************************************************************************************************
        'Procedure Name : prDB_WorkFlow
        'Purpose        : Insert WorfFlow Details
        'Arguments      : File Id,Organisation Id,User Id,Action,Reason,IP Address
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/10/2003
        '*****************************************************************************************************
        Public Sub prDB_WorkFlow(ByVal lngFileId As Long, ByVal lngFlowId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strAction As String, ByVal strReject As String, ByVal strReason As String, ByVal strIPAddr As String)

            'Create Instance of SQL Command Object
            Dim cmdWorkFlow As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations

            Try

                'Intialize SQL Connection
                Call SQLConnection_Initialize()

                'Insert Data - Start
                With cmdWorkFlow
                    .Connection = SQLConnection
                    .CommandText = "pg_WorkFlow"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_FlowId", lngFlowId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_Reject", strReject))
                    .Parameters.Add(New SqlParameter("@in_Reason", strReason))
                    .Parameters.Add(New SqlParameter("@in_UserIP", strIPAddr))
                    .ExecuteNonQuery()
                End With
                'Insert Data - Stop

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prDB_WorkFlow - clsUpload", Err.Number, Err.Description)

            Finally

                'Terminate SQL connection
                Call SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdWorkFlow = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

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
        Private Function prTempTable(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String) As Boolean

            'Create Instance of SQL Data Adaptor
            Dim daTempTable As New SqlDataAdapter

            'Create Instance of Data Row
            Dim drTempTable As System.Data.DataRow

            'Create Instance of Data Set
            Dim dsTempTable As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQLStatement As String, strCreateTable As String = "", strFieldDesc As String, intCounter As Int16

            Try
                'Initialize SQL Connection
                Call SQLConnection_Initialize()

                'Execute View
                strSQLStatement = "Select * From pg_GetBankBody"

                'Execute SQL Data Adaptor
                daTempTable = New SqlDataAdapter(strSQLStatement, SQLConnection)

                'Fill Data Set
                daTempTable.Fill(dsTempTable, "TEMP")

                'Destroy Instance SQL Data Adaptor
                daTempTable = Nothing

                'Create Table String
                intCounter = 0

                'Read the Data Set Using Data Row - Start
                If dsTempTable.Tables("TEMP").Rows.Count > 0 Then

                    'Start to build SQL Statement To be Executed
                    strCreateTable = strCreateTable & "If Not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tTemp_FileBody]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)"
                    strCreateTable = strCreateTable & " Create Table [tTemp_FileBody] ([File Id][Numeric](9,0),"

                    For Each drTempTable In dsTempTable.Tables("TEMP").Rows
                        strFieldDesc = drTempTable("FID")
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
                    Dim cmdTempTabl As New SqlCommand(strCreateTable, SQLConnection)
                    cmdTempTabl.ExecuteNonQuery()
                End If

                Return True

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "prTempTable - clsUpload", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call SQLConnection_Terminate()

                'Destroy Instance of Data Set
                dsTempTable = Nothing

                'Destroy Instace of Data row
                drTempTable = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get The Delimited Value From The Demlimited File"

        '****************************************************************************************************
        'Procedure Name : fnDelimitedValue
        'Purpose        : To get the Delimited Value.
        'Arguments      : String With Delimiter,Delimiter,Position
        'Return Value   : Delimited Value
        'Author         : Sujith Sharatchandran - 
        'Created        : 01/11/2003
        '*****************************************************************************************************
        Private Function fnDelimitedValue(ByVal strToDelimit As String, ByVal strDelimiter As String, ByVal intPosition As Int16) As String

            Dim arrToDelimit() As String, strDelimitedValue As String = ""

            Try

                intPosition = intPosition - 1

                If strDelimiter = "Comma" Then
                    strDelimiter = ","
                ElseIf strDelimiter = "Pipe" Then
                    strDelimiter = "|"
                ElseIf strDelimiter = "Tab" Then
                    strDelimiter = Chr(9)
                End If

                arrToDelimit = Split(strToDelimit, strDelimiter)

                If UBound(arrToDelimit) > 0 And intPosition >= 0 Then
                    strDelimitedValue = arrToDelimit(intPosition)
                End If

                Return strDelimitedValue

            Catch

                Return ""

            End Try

        End Function

#End Region

#Region "Get The Positioned Value From the Position Separated File"

        '****************************************************************************************************
        'Procedure Name : fnPositionValue
        'Purpose        : To get the Position Value.
        'Arguments      : String,Start Position,End Position
        'Return Value   : Position Value
        'Author         : Sujith Sharatchandran - 
        'Created        : 01/11/2003
        '*****************************************************************************************************
        Private Function fnPositionValue(ByVal strToPosition As String, ByVal intStartPos As Int16, ByVal intEndPos As Int16) As String

            Dim strPositionValue As String
            Dim intStrLength As Integer

            Try

                intStrLength = (intEndPos - intStartPos) + 1
                strPositionValue = Mid(strToPosition, intStartPos, intStrLength)

                Return strPositionValue

            Catch

                Return ""

            End Try

        End Function

#End Region

#Region "Create Header/Footer Content"



        '****************************************************************************************************
        'Function Name  : fnHeaderFooter
        'Purpose        : To Create The Header & Footer Contents
        'Arguments      : Organisation Id,User Id,File Type
        'Return Value   : Header Content
        'Author         : Sujith Sharatchandran - 
        'Created        : 01/11/2003
        '*****************************************************************************************************

        Private Function fncHeaderFooter(ByVal strContentType As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
            ByVal strFileType As String, ByVal dblTotalAmount As Double, ByVal dblTotalEmpAmt As Double, _
            ByVal dblTotalEmprAmt As Double, ByVal intRecordCount As Int32, ByVal strFileName As String, _
            ByVal lngHashTotal As String, ByVal intValueDay As Int16, ByVal intValueMonth As Int16, _
            ByVal intValueYear As Int16, ByVal intConMonth As Int16, ByVal strAccNumber As String, _
            ByVal intConYear As Int16, ByVal strIC As String, ByRef IsError As Boolean, _
            ByVal dcTranCharge As Decimal, ByVal intBankID As Integer, ByVal intGroupID As Integer, _
            ByVal strFormat As String, Optional ByVal intState As Int16 = 0, _
            Optional ByVal strTestStatus As String = "", Optional ByVal strSerAccNo As String = "", _
            Optional ByVal intTotalPCBRecs As Short = 0, Optional ByVal intTotalCP38Recs As Short = 0, Optional ByVal intSerAccId As Int16 = 0, _
            Optional ByVal intHour As Integer = 0, Optional ByVal intMinute As Integer = 0, Optional ByVal intSecond As Integer = 0, _
            Optional ByVal intMilisecond As Integer = 0, Optional ByRef sBankOrgCode As String = "", _
            Optional ByVal strUploadedHeader As String = "", Optional ByVal strUploadedFooter As String = "", _
            Optional ByRef strStateCode As String = "", Optional ByRef strContactPerson As String = "", _
            Optional ByRef strContactNumber As String = "", Optional ByRef strFileIndicator As String = "", _
            Optional ByRef strSeqNumber As String = "", Optional ByRef strRefNumber As String = "", Optional ByRef strTestingMode As String = "", _
            Optional ByRef strEmail As String = "", Optional ByRef strEmployerName As String = "") As String

            'Dim daHeader As New SqlDataAdapter         'Create Instance of SQL Data Adaptor
            Dim drHeader As System.Data.DataRow         'Create Instance of Data Row
            Dim clsGeneric As New MaxPayroll.Generic    'Create Generic Class Object
            Dim dsHeader As New System.Data.DataSet     'Create Instance of DataSet
            Dim clsCommon As New MaxPayroll.clsCommon   'Create Instance of Common Class Object

            'Variable Declarations
            Dim intFixLen As Int16
            Dim intStringLen As Int16
            Dim strContent As String = ""
            Dim strOption As String
            Dim strDefaultValue As String
            Dim intStartPos As Int16
            Dim intEndPos As Int16
            Dim strColumnContent As String = ""
            Dim intDate As Int32
            Dim strDataType As String
            Dim strFiller As String
            Dim intValueDate As Int32
            Dim strTotalAmount As String
            Dim strDescription As String
            Dim intFieldId As Int16
            Dim strMatchField As String
            Dim strTotalEmpAmt As String = ""
            Dim strTotalEmprAmt As String = ""
            Dim intFileCount As Int16
            Dim strUploadedFileLine As String
            Dim strHeaderErrMsg As String = ""
            Dim strContentTypeDesc As String = ""

            Try

                If strContentType = "H" Then
                    strContentTypeDesc = "header"
                ElseIf strContentType = "F" Then
                    strContentTypeDesc = "footer"
                End If
                'Get Value Date - Start
                intValueDate = intValueYear & Format(intValueMonth, "00") & Format(intValueDay, "00")
                'Get Value Date - Stop

                'Remove Decimal Point For TotalAmount - Start
                If (strFileType = "Payroll File" Or strFileType = _Helper.Autopay_Name Or strFileType = _Helper.AutopaySNA_Name Or strFileType = _Helper.DirectDebit_Name) Then
                    strTotalAmount = Format(dblTotalAmount, "##,##0.00")

                    'Marcus: For Satutory Body files except EPF, the previous programmers has been assign 
                    'total CP38 and PCB amount to strTotalEmpAmt and strTotalEmprAmt.

                    strTotalEmpAmt = Format(dblTotalEmpAmt, "##,##0.00")
                    strTotalEmprAmt = Format(dblTotalEmprAmt, "##,##0.00")

                    strTotalAmount = Replace(strTotalAmount, ",", "")
                    strTotalEmpAmt = Replace(strTotalEmpAmt, ",", "")
                    strTotalEmprAmt = Replace(strTotalEmprAmt, ",", "")

                    strTotalAmount = Replace(strTotalAmount, ".", "")
                    strTotalEmpAmt = Replace(strTotalEmpAmt, ".", "")
                    strTotalEmprAmt = Replace(strTotalEmprAmt, ".", "")

                    'ElseIf strFileType = "Zakat" Then
                    '    strTotalAmount = Format(dblTotalAmount, "##,##0.00")
                    '    strTotalEmpAmt = Format(dblTotalEmpAmt, "##,##0.00")
                    '    strTotalEmprAmt = Format(dblTotalEmprAmt, "##,##0.00")
                    '    strTotalAmount = Replace(strTotalAmount, ",", "")

                Else
                    strTotalAmount = dblTotalAmount
                    strTotalAmount = Replace(strTotalAmount, ",", "")
                    strTotalAmount = Replace(strTotalAmount, ".", "")
                End If



                'If (strContentType = "F" Or strContentType = "H") And strFileType = "EPF File" Then

                '    strTotalEmpAmt = Replace(dblTotalEmpAmt, ",", "")
                '    strTotalEmpAmt = Replace(dblTotalEmpAmt, ".", "")
                '    strTotalEmprAmt = Replace(dblTotalEmprAmt, ",", "")
                '    strTotalEmprAmt = Replace(dblTotalEmprAmt, ".", "")

                'End If
                'Remove Decimal Point For TotalAmount - Stop

                'Build Date
                intDate = Year(Today) & Format(Month(Today), "00") & Format(Day(Today), "00")

                'Get Header Footer Bank Format
                dsHeader = clsCommon.fncGetRequested("BANK FORMAT", lngOrgId, lngUserId, 0, strFileType, strContentType, intBankID)

                'Check if Main File or Supplementery File (EPF)
                If Not strFileType = "Payroll File" And Not strFileType = "Multiple Bank" And Not strFileType = _Helper.Autopay_Name And Not strFileType = _Helper.AutopaySNA_Name Then
                    'get file count for given contribution month & year
                    intFileCount = clsCommon.fncFileCount(strSerAccNo, lngUserId, intConMonth, intConYear, strFileType, 5)
                End If

                'Read Thro The Data Set Using Data Row - Start
                If dsHeader.Tables("UPLOAD").Rows.Count > 0 Then
                    For Each drHeader In dsHeader.Tables("UPLOAD").Rows

                        strDataType = drHeader("DataType")             'Data Type
                        strDescription = drHeader("Description")       'Description
                        intFieldId = drHeader("FieldId")               'Field Id
                        strMatchField = drHeader("MatchField")         'Matching field From Org_Master
                        strOption = drHeader("PredefinedOptions")      'Predefined Options
                        strDefaultValue = drHeader("DefaultValue")     'Default Value
                        intStartPos = drHeader("StartPos")             'Start Position
                        intEndPos = drHeader("EndPos")                 'End Position


                        'File Name
                        If strOption = "FN" Then
                            strFileName = Mid(strFileName, 1, (intEndPos - intStartPos) + 1)
                        End If

                        'Build Column Contents - Start
                        If strMatchField <> "None" Then
                            'If Match Field Is Available
                            'to be reviewed: strContentType <> "N"
                            If strFormat = "POS" AndAlso strMatchField = "Contact_Person" AndAlso strContentType <> "N" Then
                                If strContentType = "H" Then
                                    strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                Else
                                    strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                End If
                                strContactPerson = strColumnContent
                            ElseIf strFormat = "POS" AndAlso strMatchField = "Contact_Number" AndAlso strContentType <> "N" Then
                                If strContentType = "H" Then
                                    strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                Else
                                    strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                End If
                                strContactNumber = strColumnContent

                            ElseIf strFormat = "POS" AndAlso strMatchField = "Email_Address" AndAlso strContentType <> "N" Then
                                If strContentType = "H" Then
                                    strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                Else
                                    strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                End If
                                strEmail = strColumnContent
                            ElseIf strFormat = "POS" AndAlso strMatchField = "Zakat_Employer_Name" AndAlso strContentType <> "N" Then
                                If strContentType = "H" Then
                                    strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                Else
                                    strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                End If
                                strEmployerName = strColumnContent
                            Else
                                strColumnContent = clsCommon.fncBuildContent(strMatchField, strFileType, lngOrgId, lngUserId, , , , , intSerAccId)
                            End If

                            'If Match Field is Not Available
                        ElseIf strMatchField = "None" Then
                            'If Deafult Value is Available
                            If strDefaultValue <> "" And strOption = "NA" Then
                                strColumnContent = strDefaultValue
                                'If Predefined Options is Available
                            ElseIf strDefaultValue = "" And strOption <> "NA" Then


                                ''Newly added "Validate and Save" Option for Footer -- end

                                'If Payment Date
                                If strOption = "VD" Then
                                    strColumnContent = intValueDate
                                ElseIf strOption = "BO" Then
                                    Select Case strFileType
                                        Case _Helper.Autopay_Name
                                            'If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                            'End If
                                        Case _Helper.AutopaySNA_Name
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)

                                        Case _Helper.DirectDebit_Name
                                            strColumnContent = strUploadedHeader.Substring(ConfigurationManager.AppSettings("DDOrgCodeStartPOS") - 1, ConfigurationManager.AppSettings("DDOrgCodeLength"))
                                    End Select
                                    If Not fncValidateBankOrgCode(lngOrgId, strAccNumber.Trim, strColumnContent.Trim) Then
                                        IsError = True
                                        'Return "Invalid Organisation Code in the file header record" & Environment.NewLine
                                        strHeaderErrMsg += "Invalid Organisation Code in the file header record" & Environment.NewLine
                                    Else
                                        sBankOrgCode = strColumnContent
                                    End If
                                    'IF Payment Date in DDMMYYYY
                                ElseIf strOption = "ND" Then
                                    strColumnContent = Format(intValueDay, "00") & Format(intValueMonth, "00") & intValueYear

                                    If (strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name) AndAlso strContentType = "H" Then
                                        If Not strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1).Trim Then
                                            IsError = True
                                            strHeaderErrMsg += "Crediting Date in the file header record is not identical with the selected Payment Date" & Environment.NewLine
                                        End If
                                    ElseIf strFileType = _Helper.DirectDebit_Name AndAlso strContentType = "H" Then
                                        If Not strColumnContent = strUploadedHeader.Substring(ConfigurationManager.AppSettings("DDPayDateStartPOS") - 1, ConfigurationManager.AppSettings("DDPayDateLenght")) Then
                                            IsError = True
                                            strHeaderErrMsg += "Billing Date in the file header record is not identical with the selected Payment Date" & Environment.NewLine
                                        End If
                                    End If

                                    'YYYYMMDD
                                ElseIf strOption = "TD" Then
                                    strColumnContent = intValueYear & Format(intValueMonth, "00") & Format(intValueDay, "00")
                                ElseIf strOption = "TT" Then
                                    strColumnContent = Format(intHour, "00") & Format(intMinute, "00") & Format(intSecond, "00")
                                    'If EPF/Socso/LHDN state Code
                                ElseIf strOption = "SC" Then

                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If

                                        If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = intState Then
                                            IsError = True

                                            strHeaderErrMsg += "Invalid State Code in the file " & strContentTypeDesc & " record" & Environment.NewLine

                                        Else
                                            strStateCode = strColumnContent
                                        End If
                                    Else
                                        strColumnContent = intState
                                    End If

                                    'If EPF or Socso Number
                                ElseIf strOption = "EN" Then

                                    If strFormat = "POS" AndAlso strContentType <> "N" Then

                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If strDataType = "N" Then
                                            If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = CInt(strSerAccNo) Then
                                                IsError = True
                                                strHeaderErrMsg += "Invalid Employer Number in the file " & strContentTypeDesc & " record" & Environment.NewLine
                                            End If
                                        Else
                                            If Not strColumnContent.TrimEnd = strSerAccNo.Trim Then
                                                IsError = True
                                                strHeaderErrMsg += "Invalid Employer Number in the file " & strContentTypeDesc & " record" & Environment.NewLine
                                            End If
                                        End If

                                    Else
                                        strColumnContent = strSerAccNo
                                    End If

                                    'If Account Number
                                ElseIf strOption = "AN" Then
                                    strColumnContent = strAccNumber
                                    'If Creation Date
                                ElseIf strOption = "CD" Then
                                    strColumnContent = intDate
                                ElseIf strOption = "DY" Then
                                    strColumnContent = Format(Day(Today), "00") & Format(Month(Today), "00") & Year(Today)
                                    'If Fillers
                                ElseIf strOption = "FL" Then
                                    strColumnContent = ""
                                    'Total Amount
                                ElseIf strOption = "TM" Then
                                    strColumnContent = strTotalAmount

                                    'For Autopay
                                    If (strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name) AndAlso strContentType = "F" AndAlso strFormat = "POS" Then
                                        Dim strUploadedTotalAmount As String
                                        If IsNumeric(strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1).Trim) Then
                                            strUploadedTotalAmount = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1).Trim


                                            If Not CInt(strUploadedTotalAmount) = CInt(strTotalAmount) Then
                                                IsError = True
                                                strHeaderErrMsg += "Incorrect Total Amount in the file trailer record" & Environment.NewLine
                                            Else
                                                strColumnContent = strUploadedTotalAmount
                                            End If

                                        Else
                                            IsError = True
                                            strHeaderErrMsg += "Invalid Total Amount in the file trailer record, please check the data value or data position" & Environment.NewLine
                                        End If
                                    Else
                                        'For file format other than Autopay
                                        If strFormat = "POS" AndAlso strContentType <> "N" Then

                                            Select Case strFileType
                                                Case _Helper.DirectDebit_Name
                                                    strColumnContent = strUploadedFooter.Substring(ConfigurationManager.AppSettings("DDTotalAmtStartPOS") - 1, ConfigurationManager.AppSettings("DDTotalAmtLenght"))
                                                Case Else
                                                    If strContentType = "H" Then
                                                        strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                                    Else
                                                        strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                                    End If
                                            End Select

                                            If Not IIf(IsNumeric(strColumnContent), CDec(strColumnContent), -1) = CDec(strTotalAmount) Then
                                                IsError = True
                                                strHeaderErrMsg += "Invalid Total Amount in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                            End If
                                        Else
                                            strColumnContent = strTotalAmount
                                        End If
                                    End If

                                    'Total Records
                                ElseIf strOption = "RC" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        Select Case strFileType
                                            Case _Helper.DirectDebit_Name
                                                strColumnContent = strUploadedFooter.Substring(ConfigurationManager.AppSettings("DDTotalRecStartPOS") - 1, ConfigurationManager.AppSettings("DDTotalRecLenght"))
                                            Case Else
                                                If strContentType = "H" Then
                                                    strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                                Else
                                                    strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                                End If
                                        End Select

                                        If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = intRecordCount Then
                                            IsError = True
                                            'Return "Invalid Organisation Code in the file header record" & gc_BR
                                            strHeaderErrMsg += "Invalid Total Number of Record in the file " & strContentTypeDesc & " record" & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = intRecordCount
                                    End If
                                    'total PCB records
                                ElseIf strOption = "PR" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = intTotalPCBRecs Then
                                            IsError = True
                                            strHeaderErrMsg += "Invalid PCB Total Record in the file " & strContentTypeDesc & " record" & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = intTotalPCBRecs
                                    End If
                                    'total CP38 records
                                ElseIf strOption = "8R" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = intTotalCP38Recs Then
                                            IsError = True
                                            strHeaderErrMsg += "Invalid CP38 Total Record in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = intTotalCP38Recs
                                    End If

                                    'File Name
                                ElseIf strOption = "FN" Then
                                    strColumnContent = strFileName
                                    'If Hash Total
                                ElseIf strOption = "HT" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then

                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        'hafeez
                                        If strFileType = _Helper.DirectDebit_Name Then
                                            strColumnContent = strUploadedFooter.Substring(73, 15)
                                        End If
                                        lngHashTotal = Right((TotalAccountNumber + TotalAmount), 15)
                                        'end hafeez
                                        If Not IIf(IsNumeric(strColumnContent), CLng(strColumnContent), -1) = lngHashTotal Then
                                            IsError = True
                                            strHeaderErrMsg += "Invalid Hash Total in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        End If
                                        'Clear Total after validations - Start
                                        TotalAccountNumber = Nothing
                                        TotalAmount = Nothing
                                        'Clear Total after validations - Stop
                                    Else
                                        strColumnContent = lngHashTotal
                                    End If

                                    'If IC Checking
                                ElseIf (strOption = "IC" Or strOption = "PN") Then
                                    strColumnContent = strIC.Trim()
                                ElseIf strOption = "PT" Then
                                    strColumnContent = strTestStatus
                                ElseIf strOption = "ET" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        'EPF Testing Mode Value: Y/N
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If strColumnContent <> "Y" AndAlso strColumnContent <> "N" Then
                                            IsError = True
                                            'Return "Invalid Organisation Code in the file header record" & Environment.NewLine
                                            strHeaderErrMsg += "Invalid Testing Mode in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        Else
                                            strTestingMode = strColumnContent
                                        End If
                                    Else
                                        If strTestStatus.ToUpper = "T" Then
                                            strColumnContent = "Y"
                                        Else
                                            strColumnContent = "N"
                                        End If
                                    End If

                                ElseIf strOption = "NS" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        'EPF New/Supplementary payment Indicator Value: N/S
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If strColumnContent <> "N" AndAlso strColumnContent <> "S" Then
                                            IsError = True
                                            strHeaderErrMsg += "Invalid New/Supplementary payment Indicator in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        Else
                                            strFileIndicator = strColumnContent
                                        End If
                                    Else
                                        strColumnContent = fncEPFNewSupp(lngOrgId, intConMonth, intConYear)
                                    End If

                                    'If Employer Amount
                                ElseIf strOption = "ER" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        'Total EPF Employer Amount
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not IIf(IsNumeric(strColumnContent), CDec(strColumnContent), -1) = CDec(strTotalEmprAmt) Then
                                            IsError = True
                                            'Return "Invalid Organisation Code in the file header record" & gc_BR
                                            strHeaderErrMsg += "Invalid Total Employer Contribution in the " & strContentTypeDesc & " footer record " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = strTotalEmprAmt
                                    End If

                                    'If Employee Amount
                                ElseIf strOption = "EE" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        'Total EPF Employee Amount
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not IIf(IsNumeric(strColumnContent), CDec(strColumnContent), -1) = CDec(strTotalEmpAmt) Then
                                            IsError = True
                                            'Return "Invalid Organisation Code in the file header record" & Environment.NewLine
                                            strHeaderErrMsg += "Invalid Total Employee Contribution in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = strTotalEmpAmt
                                    End If

                                    'if pcb/std total amount
                                ElseIf strOption = "TP" Then

                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        'Total EPF Employee Amount
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = CInt(strTotalEmprAmt) Then
                                            IsError = True
                                            'Return "Invalid Organisation Code in the file header record" & Environment.NewLine
                                            strHeaderErrMsg += "Invalid PCB Total Amount in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = strTotalEmprAmt
                                    End If

                                    'If cp38 amount
                                ElseIf strOption = "8T" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        'Total EPF Employee Amount
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = CInt(strTotalEmpAmt) Then
                                            IsError = True
                                            'Return "Invalid Organisation Code in the file header record" & Environment.NewLine
                                            strHeaderErrMsg += "Invalid CP38 Total Amount in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = strTotalEmpAmt
                                    End If
                                    'If Contribution Month
                                ElseIf strOption = "CM" Then
                                    strColumnContent = Right(intConYear, 2) & Format(intConMonth, "00")
                                ElseIf strOption = "YM" Then

                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not strColumnContent.TrimEnd = (Format(intConMonth, "00") & intConYear) Then
                                            IsError = True
                                            strHeaderErrMsg += "Contribution Month and Year in the file " & strContentTypeDesc & " record is not identical with the selected Contribution Month and Year" & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = Format(intConMonth, "00") & intConYear
                                    End If

                                ElseIf strOption = "MY" Then
                                    strColumnContent = intConYear & Format(intConMonth, "00")
                                    'if LHDN/Zakat Contribution Month
                                ElseIf strOption = "LM" Then
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not strColumnContent.TrimEnd = Format(intConMonth, "00") Then
                                            IsError = True
                                            strHeaderErrMsg += "Contribution Month in the file " & strContentTypeDesc & " record is not identical with the selected Contribution Month and Year " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = Format(intConMonth, "00")
                                    End If

                                    'if LHDN/Zakat Contribution Year
                                ElseIf strOption = "LY" Then

                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        If Not strColumnContent.TrimEnd = Format(intConYear, "0000") Then
                                            IsError = True
                                            strHeaderErrMsg += "Contribution Year in the file " & strContentTypeDesc & " record is not identical with the selected Contribution Month and Year " & Environment.NewLine
                                        End If
                                    Else
                                        strColumnContent = Format(intConYear, "0000")
                                    End If

                                ElseIf strOption = "FI" Then
                                    'If Not Test File
                                    If strTestStatus = "P" Then
                                        strColumnContent = IIf(intFileCount = 0, "N", "S")
                                        'If Test File
                                    ElseIf strTestStatus = "T" Then
                                        strColumnContent = "N"
                                    End If
                                    'If Supplementery Indicator
                                ElseIf strOption = "SI" Then
                                    strColumnContent = IIf(strTestStatus = "P", intFileCount, "00")
                                    'If Transaction Charge
                                ElseIf strOption = "TC" Then
                                    strColumnContent = Format(dcTranCharge, "##,##0.00")
                                    strColumnContent = Replace(strColumnContent, ".", "")
                                    strColumnContent = fnFiller(strColumnContent, 1, 4, True) & strColumnContent
                                ElseIf strOption = "BT" Then
                                    strColumnContent = gstrFileBatchNo
                                ElseIf strOption = "ES" Then
                                    'EPF Sequence Number
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If

                                        If Not IsNumeric(strColumnContent) Then
                                            IsError = True
                                            strHeaderErrMsg += "Invalid Sequence Number in the file " & strContentTypeDesc & " record" & Environment.NewLine
                                        Else
                                            strSeqNumber = strColumnContent
                                        End If
                                    Else
                                        strColumnContent = gstrSeqNumber
                                    End If
                                ElseIf strOption = "FR" Then
                                    'File Ref Number
                                    If strFormat = "POS" AndAlso strContentType <> "N" Then
                                        If strContentType = "H" Then
                                            strColumnContent = strUploadedHeader.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        Else
                                            strColumnContent = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1)
                                        End If
                                        'must be numeric value or numeric value prefixed with zero (according to epf file format)
                                        If Not IsNumeric(strColumnContent) Then
                                            IsError = True
                                            strHeaderErrMsg += "Invalid Reference Number in the file " & strContentTypeDesc & " record " & Environment.NewLine
                                        Else
                                            strRefNumber = strColumnContent
                                        End If
                                    Else
                                        strColumnContent = intValueYear & Format(intValueMonth, "00") & Format(intValueDay, "00") & _
                                                                                                Format(intHour, "00") & Format(intMinute, "00") & Format(intSecond, "00") & _
                                                                                                intMilisecond.ToString.Substring(0, 2)
                                    End If

                                ElseIf strOption = "FS" Then
                                    strColumnContent = gstrFileSeqNumber
                                ElseIf strOption = "TB" Then
                                    strColumnContent = gintTotalBatchHeader
                                    If (strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name) AndAlso strContentType = "F" Then
                                        Dim strTotalBatchHeader As String


                                        If IsNumeric(strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1).Trim) Then
                                            strTotalBatchHeader = strUploadedFooter.Substring(intStartPos - 1, intEndPos - intStartPos + 1).Trim
                                            If Not CInt(strTotalBatchHeader) = gintTotalBatchHeader Then
                                                IsError = True
                                                strHeaderErrMsg += "Incorrect Total Number of Records in the file trailer record" & Environment.NewLine
                                            Else
                                                strColumnContent = strTotalBatchHeader
                                            End If
                                        Else
                                            IsError = True
                                            strHeaderErrMsg += "Invalid Total Number of Records in the file trailer record, please check the data value or data position" & Environment.NewLine
                                        End If

                                    End If
                                ElseIf strOption = "TR" Then
                                    strColumnContent = intRecordCount + gintTotalBatchHeader
                                ElseIf strOption = "TL" Then
                                    strColumnContent = intRecordCount + gintTotalBatchHeader + 2

                                End If
                            End If
                        End If
                        'Build Column Contents - Stop

                        'Assign Bank Organization Code - Start
                        If strMatchField.Equals("BankOrg_Code") Then
                            'strColumnContent = Me.fncMatchField(strMatchField, lngOrgId, intGroupID)
                            strColumnContent = Me.fncMatchField(strMatchField, lngOrgId, intGroupID, strAccNumber, intValueYear.ToString & "-" & intValueMonth.ToString & "-" & intValueDay.ToString)
                            If strColumnContent = gc_Status_Error Then
                                IsError = True
                                Return "Your Bank Organization Code has been used for the selected payment date.  Please choose a different payment date than the one selected for required submission(s)." & Environment.NewLine
                            ElseIf Len(strColumnContent & "") = 0 Then
                                IsError = True
                                Return "Your Bank Organization Code is not setup properly.  Please contact " & gc_Const_CompanyName & " " & gc_Const_RegCenter & " at " & gc_Const_CompanyContactNo & "." & Environment.NewLine
                            Else
                                sBankOrgCode = strColumnContent
                            End If
                        End If
                        If strMatchField.Equals("Org_Name") Then
                            strColumnContent = Me.fncMatchField(strMatchField, lngOrgId)
                        End If
                        'Assign Bank Organization Code - End



                        'Trim Spaces
                        If Not strColumnContent = Nothing Then
                            strColumnContent = strColumnContent.Trim()
                        End If

                        'Check for Length Exceeding - Start
                        intFixLen = (intEndPos - intStartPos) + 1
                        intStringLen = Len(strColumnContent)
                        If intStringLen > intFixLen Then
                            IsError = True
                            Return strDescription & " has more than " & intFixLen & " characters."
                        End If
                        'Check for Length Exceeding - Stop

                        'If Data Type is Numeric, Prefix With Zero To Meet Start Position And End Position
                        If strDataType = "N" Then
                            strFiller = fnFiller(strColumnContent, intStartPos, intEndPos, True)
                            strColumnContent = strFiller & strColumnContent
                            'If Data Type is Character, Prefix With EmptySpaces To Meet Start Position And End Position
                        ElseIf strDataType = "C" Then
                            strFiller = fnFiller(strColumnContent, intStartPos, intEndPos, False)
                            strColumnContent = strColumnContent & strFiller
                        End If

                        'Concat Column Content To Create Single Content
                        strContent = strContent & strColumnContent

                        'Clear Content
                        strColumnContent = ""
                        strDataType = ""
                        strDescription = ""
                        intFieldId = 0
                        strMatchField = ""
                        strOption = ""
                        strDefaultValue = ""
                        intStartPos = 0
                        intEndPos = 0

                    Next
                End If
                'Read Thro The Data Set Using Data Row - Stop

                If IsError Then
                    Return strHeaderErrMsg
                Else
                    Return strContent
                End If


            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnHeader - clsUpload", Err.Number, Err.Description)

                IsError = True

                Return "There was a Error in your file. Please check your File Format and try again " & Environment.NewLine

            Finally

                'Terminate SQL Connection
                Call SQLConnection_Terminate()

                'Destroy Instance Data Set
                dsHeader = Nothing

                'Destroy Instance of Data row
                drHeader = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing



            End Try

        End Function

#End Region

#Region "Fill Up Blank Content"

        '****************************************************************************************************
        'Function Name  : fnFiller
        'Purpose        : To Fill Space or Zero till the End Position
        'Arguments      : String,Start Position, End Position, If Numeric Or Not
        'Return Value   : Filled Up String
        'Author         : Sujith Sharatchandran - 
        'Created        : 01/11/2003
        '*****************************************************************************************************
        Public Function fnFiller(ByVal strContent As String, ByVal intStartPos As Int16, ByVal intEndPos As Int16, ByVal IsNumber As Boolean) As String

            Dim intLength As Int16, intCounter As Int16, strFiller As String = ""

            'Get Number of Blank Spaces
            intLength = ((intEndPos - intStartPos) + 1) - Len(strContent)

            'Fill Blank Spaces With Either Zero or Empty Spaces
            If intLength > 0 Then
                For intCounter = 1 To intLength
                    If Not IsNumber Then
                        strFiller = strFiller & " "
                    Else
                        strFiller = strFiller & "0"
                    End If
                Next
            End If

            Return strFiller

        End Function

#End Region

#Region "Create Body Contents"


        '****************************************************************************************************
        'Function Name  : fnBody
        'Purpose        : To Create The Body Contents
        'Arguments      : 
        'Return Value   : Body Content
        'Author         : Sujith Sharatchandran - 
        'Created        : 01/11/2003
        '*****************************************************************************************************
        Private Function fncBody(ByVal strBodyString As String, ByVal strFormat As String, ByVal strDelimiter As String, _
                ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String, ByVal lngFileId As Long, _
                ByVal drXLS As DataRow, ByVal IsICCheck As Boolean, ByVal intRowNum As Int16, ByVal strFileName As String, _
                ByVal intTotalCols As Int16, ByVal strTableName As String, ByVal intValueMonth As Int16, _
                ByVal intValueYear As Int16, ByVal lngFormatId As Long, ByVal dsColPos As DataSet, _
                ByVal dsBody As DataSet, ByVal strSerAccNo As String, ByVal intConMonth As Int16, _
                ByVal intConYear As Int16, ByVal intGroupId As Integer, Optional ByVal intBankID As Integer = 0, _
                Optional ByVal intValueDay As Int16 = 0, Optional ByVal intSerAccId As Int16 = 0, _
                Optional ByVal bIsRepeated As Boolean = False) As String

            Dim drBody As System.Data.DataRow                   'Create Instance of Data Row    
            Dim clsGeneric As New MaxPayroll.Generic            'Create Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon           'Creat Instance of Common Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer       'Create instance of Customer Class Object
            Dim clsCheckDigit As New MaxPayroll.clsCheckDigit   'Create instance of check digit class object

            'Variable Declarations
            Dim strAmount As String
            Dim intStrLen As Int16
            Dim intColCounter As Int16
            Dim intRecLen As Int16
            Dim dblEmrAmt As Double
            Dim dblEmpAmt As Double
            Dim lngMemberNo As Double
            Dim intDecimal As Int16
            Dim strOption As String
            Dim strDefaultValue As String
            Dim intStartPos As Int16
            Dim intEndPos As Int16
            Dim intLength As Int16
            Dim strColumnContent As String = ""
            Dim strDataType As String
            Dim strFiller As String
            Dim strDescription As String
            Dim intFieldId As Int16
            Dim strMatchField As String
            Dim intColPos As Int16
            Dim intStPos As Int16
            Dim intEnPos As Int16
            Dim IsAccounNo As Boolean
            Dim lngAccountNo As Long
            Dim strFields As String = ""
            Dim strValues As String = ""
            Dim dblHashTotal As Int64
            Dim ICCheck As Boolean
            Dim dblAmount As Double
            Dim strRetContent As String = ""
            Dim intSerial As Int16
            Dim intFixLen As Int16
            Dim lngICNumber As Long
            Dim CCCheck As Boolean
            Dim PNCheck As Boolean
            Dim PCBCheck As Boolean
            Dim CP38Check As Boolean
            Dim booIsMandatory As Boolean

            'Variables for CPS Usage - Start
            Dim CPS_PaymentMode As Integer = 0
            Dim CPS_BNMRentasCode As String = ""
            Dim CPS_BeneficiaryAccNo As String = ""
            'Variables for CPS Usage - End

            'Variables for Direct Debit Body Content - Start
            Dim strDebitRefNumber As String = ""
            Dim strDebitAccNumber As String = ""
            Dim strPayeeName As String = ""
            'Variables for Direct Debit Body Content - End

            Try

                'Set IC Check to False
                ICCheck = False

                'Set Passport/Country Code check to False
                CCCheck = False
                PNCheck = False

                'Intialise Column Counter
                intColCounter = 0

                'Read Thro The Data Set Using Data Row - Start
                If dsBody.Tables("UPLOAD").Rows.Count > 0 Then
                    For Each drBody In dsBody.Tables("UPLOAD").Rows

                        strDataType = drBody("DataType")             'Data Type
                        strDescription = drBody("Description")       'Description
                        intFieldId = drBody("FieldId")               'Field Id
                        strMatchField = drBody("MatchField")         'Matching field From Org_Master
                        strOption = drBody("PredefinedOptions")      'Predefined Options
                        strDefaultValue = drBody("DefaultValue")     'Default Value
                        intStartPos = drBody("StartPos")             'Start Position
                        intEndPos = drBody("EndPos")                 'End Position
                        booIsMandatory = drBody("IsMandatory")
                        intLength = (intEndPos - intStartPos) + 1
                        'If Match Field Is Available - START

                        If (strFileType = _Helper.Autopay_Name OrElse strFileType = _Helper.AutopaySNA_Name) AndAlso strDescription = "Detail Record Type" Then
                            If bIsRepeated = False AndAlso strBodyString.Length > 197 AndAlso strBodyString.Substring(197, 5).Trim <> "" Then
                                gIntErrorMsgRowNo += 1
                            End If
                        End If

                        If strMatchField <> "None" Then

                            strColumnContent = clsCommon.fncBuildContent(strMatchField, "", lngOrgId, lngUserId, , , , intSerAccId)

                            'If Default Value Is Available
                        ElseIf strMatchField = "None" And strDefaultValue <> "" Then
                            If (Not strFileType = _Helper.Autopay_Name And Not strFileType = _Helper.AutopaySNA_Name) OrElse intEndPos <= strBodyString.Length Then
                                strColumnContent = strDefaultValue
                            Else
                                strColumnContent = IIf(strDataType = "N", 0, "")
                            End If
                            'If Match Field And Default Not Available
                        ElseIf strMatchField = "None" And strDefaultValue = "" And Not (strOption = "EN" Or strOption = "CM" Or strOption = "Y2") Then
                            'If Delimited File
                            If strFormat = "DELIM" Then
                                'Get the Column Position
                                intColPos = fncGetColPos(intFieldId, dsColPos, "COLUMN")
                                'Get the Content From the String With Delimiter
                                strColumnContent = fnDelimitedValue(strBodyString, strDelimiter, intColPos)
                                'if position separated file 
                            ElseIf strFormat = "POS" Then
                                'Get Start Position
                                intStPos = fncGetColPos(intFieldId, dsColPos, "START")
                                'Get End Position
                                intEnPos = fncGetColPos(intFieldId, dsColPos, "END")
                                'Get Column Content

                                If (Not strFileType = _Helper.Autopay_Name And Not strFileType = _Helper.AutopaySNA_Name) OrElse intEndPos <= strBodyString.Length Then
                                    strColumnContent = fnPositionValue(strBodyString, intStPos, intEnPos)
                                ElseIf (Not strFileType = _Helper.Autopay_Name And Not strFileType = _Helper.AutopaySNA_Name) OrElse (intEndPos > strBodyString.Length AndAlso intStPos <= strBodyString.Length) Then
                                    strColumnContent = fnPositionValue(strBodyString, intStPos, strBodyString.Length)
                                Else
                                    strColumnContent = IIf(strDataType = "N", 0, "")
                                End If
                                'if excel file
                            ElseIf strFormat = "COL" Then
                                'Check Column Less Than Available Column
                                If intColCounter <= intTotalCols Then
                                    'Get the Column Position
                                    intColPos = fncGetColPos(intFieldId, dsColPos, "COLUMN")
                                    'if column position available - start
                                    If intColPos > 0 Then
                                        'check if excel file contains the colummn - start
                                        If (drXLS.Table.Columns.Contains("F" & intColPos)) Then
                                            If IsDBNull(drXLS(intColPos - 1)) Then
                                                If strDataType = "C" Or strDataType = "A" Then
                                                    strColumnContent = String.Empty
                                                ElseIf strDataType = "N" Then
                                                    strColumnContent = 0
                                                End If
                                            Else
                                                strColumnContent = drXLS(intColPos - 1)
                                            End If
                                        Else
                                            If strDataType = "C" Or strDataType = "A" Then
                                                strColumnContent = String.Empty
                                            ElseIf strDataType = "N" Then
                                                strColumnContent = 0
                                            End If
                                        End If

                                    End If
                                    'if column position available - stop
                                    intColCounter = intColCounter + 1
                                End If
                            End If
                        End If
                        'If Match Field Is Available - STOP

                        'Validate Employer Number
                        If strOption = "EU" Then
                            If strDataType = "N" Then
                                If Not IIf(IsNumeric(strColumnContent), CInt(strColumnContent), -1) = CInt(strSerAccNo) Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                End If
                            Else
                                If Not strColumnContent.TrimEnd = strSerAccNo.Trim Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                End If
                            End If
                        End If

                        If strOption = "CH" Then
                            If Not strColumnContent = Format(intConMonth, "00") & intConYear.ToString.Substring(2, 2) Then
                                gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                IsFileReject = True
                            End If
                        End If

                        'Reference Number - START
                        If strOption = "RN" Then
                            strColumnContent += CStr(intRowNum + 1)
                        End If
                        'Reference Number - END

                        'Check/generate Unique Reference Number - Start
                        If strOption = "UR" Then
                            strColumnContent = GetUniqueRefNo(lngOrgId, lngUserId, _
                                strColumnContent, intStartPos, intEndPos, intRowNum)
                        End If
                        'Check/Generate Unique Reference Number - Stop

                        'Reference Number - START
                        If strOption = "BT" Then
                            strColumnContent = gstrFileBatchNo
                        End If
                        'Reference Number - END

                        'EPF Sequence Number - START
                        If strOption = "ES" Then
                            strColumnContent = gstrSeqNumber
                        End If
                        'EPF Sequence Number - END

                        'Marcus: Currently used by epf Header Format Type 1
                        If strOption = "FS" Then
                            strColumnContent = gstrFileSeqNumber
                        End If

                        If strOption = "BH" Then
                            If Not gstrPreBatchRef = strColumnContent.Trim Then
                                gintTotalBatchHeader += 1
                                gstrPreBatchRef = strColumnContent.Trim
                            End If
                        End If

                        'Predefined options for direct debit mandate validation - start
                        If strOption = "DR" Then
                            strDebitRefNumber = strColumnContent.Trim

                        ElseIf strOption = "PE" Then
                            strPayeeName = strColumnContent.Trim
                        End If
                        'Predefined options for direct debit mandate validation - end


                        '071201 Option Created by Marcus
                        'Debit Account No. Checking - Start
                        If strOption = "DA" Then

                            Dim bIsACNoValid As Boolean
                            bIsACNoValid = clsUpload.fncValidateGroupAccountNo(intGroupId, strColumnContent.Trim)

                            If bIsACNoValid = False Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            End If

                        End If
                        'Debit Account No. Checking - End

                        Select Case strOption
                            Case "ND" 'Check against Selected Payment Date with body's payment date 
                                If strColumnContent.Trim <> "" AndAlso strColumnContent <> Format(intValueDay, "00") & Format(intValueMonth, "00") & intValueYear.ToString.Substring(2, 2) Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has a " & strDescription & " which is not identical to the selected payment date" & Environment.NewLine
                                    IsFileReject = True
                                End If

                            Case "DD" 'Check against date format and check whether is it a valid date
                                If strColumnContent.Trim <> "" AndAlso IsDate("20" & strColumnContent.Substring(4, 2) & "-" & strColumnContent.Substring(2, 2) & "-" & strColumnContent.Substring(0, 2)) = False Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & "  has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                End If

                            Case "CT" 'Check against time format and check whether is it a valid time
                                If strColumnContent.Trim <> "" AndAlso IsDate(String.Format("{0:yyyy-MM-dd}", DateTime.Now) & " " & strColumnContent.Substring(0, 2) & ":" & strColumnContent.Substring(2, 2) & ":" & strColumnContent.Substring(4, 2)) = False Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                End If

                            Case "PM" 'Check CPS Payment Mode
                                If strColumnContent.Trim <> "" AndAlso strColumnContent <> "1" AndAlso strColumnContent <> "2" AndAlso strColumnContent <> "3" Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                Else
                                    CPS_PaymentMode = CInt(strColumnContent)
                                End If

                            Case "DM" 'Check CPS Delivery Mode
                                If strColumnContent.Trim <> "" AndAlso strColumnContent <> "1" Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & "  has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                End If

                            Case "PC" 'Check CPS Post Code
                                If IsNumeric(strColumnContent) = False Then
                                    gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & "  has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                Else
                                    If strColumnContent.Length < 5 Then
                                        gstrErrorMsg += "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                        IsFileReject = True
                                    End If
                                End If

                            Case "BB" 'Recording CPS BNM Rentas Code
                                CPS_BNMRentasCode = strColumnContent

                            Case "BC" 'Recording CPS Beneficiary Account Number
                                CPS_BeneficiaryAccNo = strColumnContent

                        End Select

                        If strOption = "MC" Then
                            'BNM Code Validation - Start
                            If Not fncBNMValid(strColumnContent.Trim) Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            End If
                            'BNM Code Validation - End
                        End If
                        'IC Validations - START
                        If IsICCheck Then
                            If (strOption = "IC" Or strOption = "PN") And Not ICCheck Then
                                ICCheck = IIf(Trim(strColumnContent) = "", False, True)
                                If InStr(strColumnContent, "-") > 0 Then
                                    gstrErrorMsg = gstrErrorMsg & strDescription & " cannot have dash(-). Please check line " & gIntErrorMsgRowNo + 1 & "." & Environment.NewLine
                                    IsFileReject = True
                                End If
                                'check if all zeros
                                If IsNumeric(strColumnContent) Then
                                    If Not strColumnContent > 0 Then
                                        gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                        IsFileReject = True
                                    End If
                                End If
                            End If
                        End If
                        'IC Validations - END

                        'Passport and Country Code Validations
                        If (strOption = "PN" Or strOption = "CC") And strFileType = "LHDN File" Then
                            If strOption = "PN" Then
                                PNCheck = IIf(Trim(strColumnContent) = "", False, True)
                            Else
                                CCCheck = IIf(Trim(strColumnContent) = "", False, True)
                            End If
                        End If

                        'pcb amount and cp38 amount - either one must be filled. ---start
                        If strOption = "PA" Or strOption = "8A" Then
                            If strOption = "PA" Then
                                PCBCheck = IIf(Trim(strColumnContent) = 0, False, True)
                            Else
                                CP38Check = IIf(Trim(strColumnContent) = 0, False, True)
                            End If
                        End If
                        'pcb amount and cp38 amount - either one must be filled. ---stop


                        'if employer account no
                        If strOption = "EN" Then
                            strColumnContent = strSerAccNo
                            'if contribution month
                        ElseIf strOption = "Y2" Then
                            strColumnContent = Format(intConMonth, "00") & Right(intConYear, 2)
                        ElseIf strOption = "CM" Then
                            strColumnContent = Right(intConYear, 2) & Format(intConMonth, "00")
                            'if bank account number
                        ElseIf strOption = "BN" Then
                            'for CIMB billing file's Batch Number field
                            'strColumnContent = CStr(intValueDay) + CStr(intValueMonth) + CStr(intValueYear) + CStr(intRowNum + 1)
                            strColumnContent = fncGetBillingFileBatchNo()
                        ElseIf strOption = "AN" Then

                            IsAccounNo = True
                            If Not IsAccounNo Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            Else
                                If IsNumeric(strColumnContent) Then
                                    lngAccountNo = strColumnContent
                                    If strFileType = _Helper.DirectDebit_Name Then
                                        strDebitAccNumber = strColumnContent.Trim
                                    End If
                                Else
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If

                            End If


                            'End If
                            'if amount or employer amount or employee amount
                        ElseIf (strOption = "AM" Or strOption = "RA" Or strOption = "EA" Or strOption = "EW" Or strOption = "PA") Then

                            'marcus:???''Get Decimal Place Position
                            intDecimal = InStr(strColumnContent, ".")

                            'check if text file has dot in amount - start
                            If strFormat = "POS" And intDecimal > 0 And strFileType <> "Zakat" Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " " & strDescription & " has dot in the amount. " & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            End If
                            'check if text file has dot in amount - stop

                            'if amount or employee wages or cp38 amount
                            If (strOption = "AM" Or strOption = "EW" Or strOption = "8A") Then
                                'If Decimal Place Available and Decimal Value more than 2
                                If intDecimal > 0 And Len(Mid(strColumnContent, intDecimal + 1)) > 2 Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has more than two decimal places for " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If
                                'if employer amount or employee amount or pcb amount
                            ElseIf (strOption = "RA" Or strOption = "EA" Or strOption = "PA") Then
                                'If Decimal Place Available
                                If intDecimal > 0 Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " should not contain decimal places for " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If
                            End If
                            'if socso or epf number
                        ElseIf strOption = "MN" Then
                            'Get Employee EPF Number
                            lngMemberNo = IIf(IsNumeric(strColumnContent), strColumnContent, 0)
                            If lngMemberNo > 0 Then
                                If Not intStrLen = intFixLen Then
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " " & " has invalid " & strDescription & Environment.NewLine
                                    IsFileReject = True
                                End If
                            End If
                            'if IC Number and numeric value
                        ElseIf strOption = "IC" And strFileType = "SOCSO File" Then
                            If Trim(strColumnContent) = "" Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has no " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            Else
                                'get the last five digits of the ic number
                                lngICNumber = Right(Replace(strColumnContent, " ", ""), 5)
                            End If
                            'if LHDN Income tax no, pick the last 5 digits - start
                        ElseIf strFileType = "LHDN File" And strOption = "IT" Then

                            If Not clsCheckDigit.fncCheckDigitLHDN(strColumnContent.Trim) Then
                                IsFileReject = True
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                            Else
                                lngICNumber = CLng(strColumnContent.Trim)
                            End If

                        End If

                        'if Employee Wages/EPF Employee/EPF Employer/Pay Amount is numeric and not greater then 0 - start
                        If strOption = "EW" Or strOption = "EA" Or strOption = "RA" Then
                            If IsNumeric(strColumnContent) Then
                                If Not Trim(strColumnContent) > 0 Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If
                            Else
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                                strColumnContent = 0
                            End If

                            'hafeez 6-11-2208 - Direct Debit Bypass if Debit amount = 0'
                        ElseIf strOption = "AM" Then
                            If IsNumeric(strColumnContent) Then
                                If Not Trim(strColumnContent) >= 0 Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If
                            Else
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                                strColumnContent = 0
                            End If
                        End If
                        'if Employee Wages/EPF Employee/EPF Employer/Pay Amount is numeric and not greater then 0 - stop

                        'if LHDN Wife Code is blank---start
                        If strOption = "WC" Then
                            'if excel column for WC is blank then reject the file.
                            If strFormat = "COL" AndAlso IsDBNull(drXLS(intColPos - 1)) Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            Else
                                If Not IsNumeric(strColumnContent) Then
                                    'If Trim(strColumnContent) = "" Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                    strColumnContent = 0
                                    'End If
                                End If
                            End If
                        End If
                        'if LHDN Wife Code is blank---stop

                        'if Employee Name is blank ----start
                        If strOption = "EM" Then
                            If Not IsNumeric(strColumnContent) Then
                                If Trim(strColumnContent) = "" Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If
                            Else
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                                strColumnContent = ""
                            End If
                        End If
                        'if Employee Name is blank ----stop

                        'check if data type is numeric and given value is also numeric or blank - start
                        If strDataType = "N" AndAlso Not strOption = "AN" Then
                            If Not IsNumeric(strColumnContent) Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has invalid " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            End If
                        End If
                        'check if data type is numeric and given value is also numeric - stop
                        'Mandatory Field Checking -START
                        If booIsMandatory AndAlso (strMatchField = "None" And strDefaultValue = "") AndAlso Not (strOption = "EW" Or strOption = "EA" Or strOption = "RA" Or strOption = "AM" Or strOption = "FL") Then
                            If strDataType = "N" Then
                                If (strColumnContent = "" Or strColumnContent = "0") OrElse (IsNumeric(strColumnContent) AndAlso CDec(strColumnContent) = 0D) Then
                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has 0 value for mandatory field: " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True
                                End If
                            Else
                                If strColumnContent.Trim = "" Then

                                    'Build Error Message
                                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has empty value for mandatory field: " & strDescription & Environment.NewLine
                                    'File Reject Status
                                    IsFileReject = True


                                End If
                            End If
                        End If

                        If strFileType = _Helper.AutopaySNA_Name AndAlso strOption = "IC" Then

                            If strColumnContent.Trim = "" Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " has empty value for mandatory field: " & strDescription & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True
                            End If

                        End If
                        'Mandatory Field Checking - STOP


                        'Assign BNM Code - Start
                        If strMatchField.Equals("BNM_Code") Then
                            strColumnContent = Me.fncMatchField(strMatchField, intBankID)
                        End If
                        'Assign BNM Code - End

                        'Remove Spaces
                        strColumnContent = Trim(strColumnContent)

                        'If EPF File and Position Separated divide amount divided by 100

                        If strFormat = "POS" And strFileType = "EPF File" And (strOption = "RA" Or strOption = "EA") Then
                            dblAmount = strColumnContent / 100
                            'if Position Separated and EPF File and Employee Wages divided by 100 amount
                        ElseIf strFormat = "POS" And strFileType = "EPF File" And strOption = "EW" Then
                            dblAmount = strColumnContent / 100
                            'If Payroll File or socso file and Position Separated divide amount 100
                        ElseIf strFormat = "POS" And Not strFileType = "EPF File" And (strOption = "AM" Or strOption = "PA" Or strOption = "8A") Then
                            If strFileType = "Zakat" Then
                                dblAmount = strColumnContent
                            Else
                                dblAmount = strColumnContent / 100
                            End If
                            'If Not Position Separated, do not divide amount by 100
                        ElseIf (strFormat = "COL" Or strFormat = "DELIM") And (strOption = "RA" Or strOption = "EA" Or strOption = "AM" Or strOption = "EW" Or strOption = "PA" Or strOption = "8A") Then
                            dblAmount = strColumnContent
                        End If

                        'if employer amount or PCB/STD Amount
                        If (strOption = "RA" Or strOption = "PA") Then
                            dblEmrAmt = dblAmount
                            strColumnContent = Format(dblAmount, "##,##0.00")
                            'if PCB amount greater then zero
                            If dblEmrAmt > 0 And strFileType = "LHDN File" Then
                                gintPCBRecs = gintPCBRecs + 1
                            End If
                            'if employee amount or CP38 amount
                        ElseIf strOption = "EA" Or strOption = "8A" Then
                            dblEmpAmt = dblAmount
                            strColumnContent = Format(dblAmount, "##,##0.00")
                            'if CP38 amount greater then zero
                            If dblEmpAmt > 0 And strFileType = "LHDN File" Then
                                gintCP38Recs = gintCP38Recs + 1
                            End If
                            'if amount or employee wages
                        ElseIf strOption = "AM" Or strOption = "EW" Then
                            'convert to currency to display in report
                            strAmount = Format(dblAmount, "##,##0.00")
                            strColumnContent = strAmount
                        End If

                        'Build Insert Statement For File Body - START
                        If strMatchField = "None" And strDefaultValue = "" And Not (strOption = "FL" Or strOption = "EN" Or strOption = "_") Then
                            If intSerial = 0 Then
                                strFields = "[" & strDescription & "]"
                                strValues = "'" & strColumnContent & "'"
                            Else
                                strFields = strFields & "," & "[" & strDescription & "]"
                                strValues = strValues & ",'" & Replace(strColumnContent, "'", "''") & "'"
                            End If
                            intSerial = intSerial + 1
                        End If
                        'Build Insert Statement For File Body - END

                        'Marcus: Remove Email Address for Autopay file body
                        If strOption = "AE" Then
                            strColumnContent = ""
                        End If



                        'If Data Type is Numeric, Prefix With Zero To Meet Start Position And End Position
                        If strDataType = "N" Then
                            'if employer amount or employee amount no decimal value
                            If (strOption = "RA" Or strOption = "EA") Then
                                strColumnContent = Left(strColumnContent, InStr(strColumnContent, "."))
                            End If
                            'remove comma
                            strColumnContent = Replace(strColumnContent, ",", "")

                            'remove dot execept PCB and CP38
                            'If Not (strFileType = "Zakat" Or strOption = "PA" Or strOption = "8A") Then
                            If Not strFileType = "Zakat" Then
                                strColumnContent = Replace(strColumnContent, ".", "")
                            End If

                            'prefix zero to meet required length
                            strFiller = fnFiller(strColumnContent, intStartPos, intEndPos, True)
                            strColumnContent = strFiller & strColumnContent
                            'If Data Type is Character, Prefix With Empty Spaces To Meet Start Position And End Position
                        ElseIf strDataType = "C" Or strDataType = "A" Then
                            'suffix space to meet required length
                            strFiller = fnFiller(strColumnContent, intStartPos, intEndPos, False)
                            strColumnContent = strColumnContent & strFiller
                        End If

                        'Check if Column Length Extends Required Length - START
                        If strFormat = "COL" Or strFormat = "DELIM" Then
                            'reguired length
                            intFixLen = (intEndPos - intStartPos) + 1
                            'Get the Length of the Value
                            intStrLen = Len(Trim(strColumnContent))
                            'Check if Value length is greater than Required length 
                            If intStrLen > intFixLen Then
                                'Build Error Message
                                gstrErrorMsg = gstrErrorMsg & strDescription & " for Line " & gIntErrorMsgRowNo + 1 & " has more than " & intFixLen & " characters." & Environment.NewLine
                                'File Reject Status
                                IsFileReject = True

                            End If
                        End If

                        strRetContent = strRetContent & strColumnContent
                        'End If
                        'Set Record Length
                        intRecLen = intEndPos

                        'CPS Dividen Validation - Start (Hafeez April162009) Remarks:Make as function
                        'If strFileType = _Helper.CPSDividen_Name Then
                        '    Try


                        '        Dim Value As String = strRetContent.Substring(intStartPos - 1, intLength)
                        '        Dim Gross1 As Double
                        '        Dim Gross2 As Double
                        '        Dim Gross3 As Double
                        '        Dim Gross4 As Double

                        '        Dim Tax1 As Double
                        '        Dim Tax2 As Double
                        '        Dim Tax3 As Double
                        '        Dim Tax4 As Double

                        '        Dim Net1 As Long
                        '        Dim Net2 As Long
                        '        Dim Net3 As Long
                        '        Dim Net4 As Long

                        '        Dim TotalNet As Long

                        '        If strDescription = _clsCPSPhase3.DGross1 Then
                        '            Gross1 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DGross2 Then
                        '            Gross2 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DGross3 Then
                        '            Gross3 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DGross4 Then
                        '            Gross4 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DTax1 Then
                        '            Tax1 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DTax2 Then
                        '            Tax2 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DTax3 Then
                        '            Tax3 = Convert.ToDouble(Value)
                        '        ElseIf strDescription = _clsCPSPhase3.DTax4 Then
                        '            Tax4 = Convert.ToDouble(Value)

                        '        ElseIf strDescription = _clsCPSPhase3.DNet1 Then
                        '            Net1 = Convert.ToDouble(Value)
                        '            If (Gross1 - Tax1) <> Net1 Then
                        '                gstrErrorMsg = gstrErrorMsg & _clsCPSPhase3.Msg_CPSLineErrorMessage(gIntErrorMsgRowNo, strDescription)
                        '                IsFileReject = True
                        '            End If
                        '        ElseIf strDescription = _clsCPSPhase3.DNet2 Then
                        '            Net2 = Convert.ToDouble(Value)
                        '            If (Gross2 - Tax2) <> Net2 Then
                        '                gstrErrorMsg = gstrErrorMsg & _clsCPSPhase3.Msg_CPSLineErrorMessage(gIntErrorMsgRowNo, strDescription)
                        '                IsFileReject = True
                        '            End If
                        '        ElseIf strDescription = _clsCPSPhase3.DNet3 Then
                        '            Net3 = Convert.ToDouble(Value)
                        '            If (Gross3 - Tax3) <> Net3 Then
                        '                gstrErrorMsg = gstrErrorMsg & _clsCPSPhase3.Msg_CPSLineErrorMessage(gIntErrorMsgRowNo, strDescription)
                        '                IsFileReject = True
                        '            End If
                        '        ElseIf strDescription = _clsCPSPhase3.DNet4 Then
                        '            Net4 = Convert.ToDouble(Value)
                        '            If (Gross4 - Tax4) <> Net4 Then
                        '                gstrErrorMsg = gstrErrorMsg & _clsCPSPhase3.Msg_CPSLineErrorMessage(gIntErrorMsgRowNo, strDescription)
                        '                IsFileReject = True
                        '            End If
                        '        ElseIf strDescription = _clsCPSPhase3.DTOTNET Then
                        '            TotalNet = Convert.ToDouble(Value)
                        '            If (Net1 + Net2 + Net3 + Net4) <> TotalNet Then
                        '                gstrErrorMsg = gstrErrorMsg & _clsCPSPhase3.Msg_CPSLineErrorMessage(gIntErrorMsgRowNo, strDescription)
                        '                IsFileReject = True
                        '            End If

                        '        End If
                        '    Catch ex As Exception

                        '    End Try
                        'End If
                        'CPS Dividen Validation - Stop

                        'Clear Variables 
                        strDataType = ""
                        strDescription = ""
                        intFieldId = 0
                        strMatchField = ""
                        strOption = ""
                        strDefaultValue = ""
                        intStartPos = 0
                        intEndPos = 0
                        strColumnContent = ""
                    Next
                End If

                'Direct Debit Mandate Validation - Start
                If strFileType = _Helper.DirectDebit_Name Then
                    Dim oItem As New clsMandates
                    Dim strRetMsg As String = ""

                    oItem.paramOrgID = lngOrgId
                    oItem.paramBankOrgCode = gStrBankOrgCode
                    oItem.paramRefNo = strDebitRefNumber
                    oItem.paramAccNo = strDebitAccNumber
                    oItem.paramCustomerName = strPayeeName
                    oItem.paramLimitAmount = CDec(dblAmount)

                    strRetMsg = oItem.ValidateMandate(gIntErrorMsgRowNo + 1)

                    If Not strRetMsg = gc_Status_OK Then
                        gstrErrorMsg += strRetMsg
                        IsFileReject = True
                    End If
                End If
                'Direct Debit Mandate Validation - End
                'Incase IC Checking Failed Return and Reject Upload - Start
                If IsICCheck And Not ICCheck And (strFileType = "Payroll File" Or strFileType = "LHDN File" Or strFileType = "Multiple Bank" Or strFileType = _Helper.DirectDebit_Name) Then
                    gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " does not have IC related details. Please Note: IC checking has been opted at the time of registration." & Environment.NewLine
                    IsFileReject = True
                End If
                'Incase IC Checking Failed Return and Reject Upload - Stop

                'Incase Passport and Country Code Checking Failed Return and Reject Upload - Start
                If strFileType = "LHDN File" Then
                    If PNCheck Then
                        If Not CCCheck Then
                            gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " does not have Passport No./Country Code related details. Please Note: Both column must be filled in." & Environment.NewLine
                            IsFileReject = True
                        End If

                    End If
                End If
                'Incase Passport and Country Code Checking Failed Return and Reject Upload - Stop

                'Incase PCB amount and cp38 amount Checking Failed Return and Reject Upload - Start
                If strFileType = "LHDN File" Then
                    If Not PCBCheck Then
                        If Not CP38Check Then
                            gstrErrorMsg = gstrErrorMsg & "Line " & gIntErrorMsgRowNo + 1 & " does not have PCB Amount/CP38 Amount related details. Please Note: Either one of the column must be filled in." & Environment.NewLine
                            IsFileReject = True
                        End If
                    End If
                End If
                'Incase PCB amount and cp38 amount Checking Failed Return and Reject Upload - Stop

                'Build Total Amount
                If dblAmount > 0 Then
                    If Not bIsRepeated Then
                        gdblTotalAmount = gdblTotalAmount + dblAmount
                    End If
                End If
                'Build Direct Debit Hash Total-hafeez
                If strFileType = _Helper.DirectDebit_Name Then
                    TotalAccountNumber += lngAccountNo
                    TotalAmount += CLng(dblAmount * 100)
                    'glngHashTotal += lngAccountNo + (CLng(dblAmount * 100))
                End If
                'hafeez

                'Build if EPF File, Total Employee Amount - If LHDN File Total PCB/STD Amount
                If dblEmpAmt > 0 Then
                    If strFileType = "LHDN File" Then
                        gdblPCBHash += dblEmpAmt

                    End If

                    gdblTotalEmpAmt = gdblTotalEmpAmt + dblEmpAmt
                End If

                'Build if EPF File, Total Employer Amount - If LHDN File Total CP38 Amount
                If dblEmrAmt > 0 Then
                    gdblTotalEmrAmt = gdblTotalEmrAmt + dblEmrAmt
                End If

                'Build Hash Total
                If (strFileType = "Payroll File" Or strFileType = "Multiple Bank" Or strFileType = _Helper.Autopay_Name Or strFileType = _Helper.AutopaySNA_Name) And lngAccountNo > 0 Then
                    glngHashTotal += (lngAccountNo + (CLng(dblAmount * 100)))

                ElseIf strFileType = "EPF File" Then
                    'add the epf numbers
                    glngHashTotal = glngHashTotal + lngMemberNo
                ElseIf (strFileType = "SOCSO File") Then
                    'add the ic number
                    glngHashTotal = glngHashTotal + lngICNumber
                ElseIf (strFileType = "LHDN File") Then
                    glngHashTotal = glngHashTotal + lngICNumber
                End If

                'Insert Record For Report Purpose - Start
                '2007-02-28: Victor : This part definitely need rewrite, because it initiates a connection to database when complete process 1 line of record. imagine 10000 lines, will require 10000 times connection.  the sql script should only be formed at this point, then pass to outside fncCreateFile, then perform saving at fncCreateFile.
                If strFields <> "" And strValues <> "" Then
                    gintRowNum = gintRowNum + 1
                    If Not clsCommon.prcTranRecords(lngOrgId, lngUserId, lngFileId, strFields, strValues, strTableName, gintRowNum) Then
                        'Return Error
                        Return gc_Status_Error
                    End If
                End If
                'Insert Record For Report Purpose - Stop

                gIntErrorMsgRowNo += 1

                'If strFileType = _Helper.Autopay_Name Then
                '    If bIsRepeated Then
                '        Return "" 'strRetContent.Substring(202)
                '        'ElseIf strRetContent.Substring(204, 5).Trim = "" Then
                '        'Return strRetContent.Substring(0, 127) 'strRetContent.Substring(0, 202)
                '    Else
                '        Return strRetContent.Substring(0, 127) 'strRetContent.Substring(0, 202) '& vbCrLf & strRetContent.Substring(202)
                '    End If
                If strFileType = _Helper.AutopaySNA_Name Or strFileType = _Helper.Autopay_Name Then
                    If bIsRepeated Then
                        Return strRetContent.Substring(202)
                    Else
                        If strRetContent.Substring(202, 2) = "00" Then 'hafeez - "00" is payment details
                            Return strRetContent.Substring(0, 202) & vbCrLf & strRetContent.Substring(202)
                        Else
                            Return strRetContent.Substring(0, 202) ''+ clsCommon.fncAutopaySNABody(strRetContent.Substring(106, 20).Trim)
                        End If



                    End If
                Else
                    Return strRetContent
                End If

                'End If


            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnBody - clsUpload", Err.Number, Err.Description)

                'Return Error
                Return gc_Status_Error

            Finally

                'Terminate SQL Connection
                Call SQLConnection_Terminate()

                'Destroy Instance of Data Set
                dsBody = Nothing

                'Destroy Instance of Data Row
                drBody = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'destroy instance of customer class object
                clsCustomer = Nothing

                'destroy instance of check digit class object
                clsCheckDigit = Nothing

            End Try

        End Function

#End Region

#Region "Generic match field and predefined options"
        Private Function fncMatchField(ByVal strMacthString As String, Optional ByVal Value1 As Object = Nothing, Optional ByVal Value2 As Object = Nothing, Optional ByVal Value3 As Object = Nothing, Optional ByVal Value4 As Object = Nothing) As String
            Dim strRetVal As String
            Try
                Select Case strMacthString.Trim.ToLower
                    Case "bnm_code"
                        strRetVal = clsBankMF.fncGetBankCode(CInt(Value1))
                    Case "bankorg_code"
                        If IsDate(Value4) Then
                            strRetVal = clsCustomer.fncGetBankOrgID(CLng(Value1), CInt(Value2), CStr(Value3), CStr(Value4))
                        Else
                            Return gc_Status_Error
                        End If

                    Case "org_name"
                        strRetVal = clsCustomer.fnGetOrgnizationName(CInt(Value1))
                    Case Else
                        strRetVal = ""
                End Select
            Catch ex As Exception
                strRetVal = ""
            End Try

            Return strRetVal
        End Function
#End Region

#Region "Check for Null Row in Excel"

        '****************************************************************************************************
        'Procedure Name : fnBlank
        'Purpose        : To Check If the row is blank in excel
        'Arguments      : Data Row
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 06/12/2003
        '*****************************************************************************************************
        Private Function fnBlank(ByVal drXls As DataRow, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByRef intColCount As Int16) As Boolean

            'Instance of Generic Class
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim IsBlank As Boolean, strData As String, intCounter As Int16, intTotalCols As Int16

            Try

                'Add Data Row to Data Table
                intTotalCols = drXls.ItemArray.Length
                'UBound(drXls.ItemArray)

                'Set Total Column Count
                intColCount = intTotalCols

                'Set to False
                IsBlank = False

                'Run Thro The Data Row to Check if the complete Row is Blank - Start
                For intCounter = 0 To intTotalCols
                    strData = IIf(IsDBNull(drXls(intCounter)), "", drXls(intCounter))
                    If strData = "" Then
                        IsBlank = True
                    ElseIf strData <> "" Then
                        IsBlank = False
                        Exit For
                    End If
                Next
                'Run Thro The Data Row to Check if the complete Row is Blank - Stop

                Return IsBlank

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnBlank - clsUpload", Err.Number, Err.Description)

                Return True

            Finally

                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Line Count"

        '****************************************************************************************************
        'Procedure Name : fnLineCount
        'Purpose        : To get the Number of Lines in a 
        'Arguments      : Server Name Or IP,UserId,Password,Source File  
        'Return Value   : Integer
        'Author         : Sujith Sharatchandran - 
        'Created        : 28/04/2004
        '*****************************************************************************************************
        Public Function fncLineCount(ByVal strFileName As String) As Integer

            'Variable Declarations
            Dim intLineCount As Integer

            'Create File Reader Object
            Dim flReader As StreamReader, strFileContent As String

            Try

                flReader = File.OpenText(strFileName)

                While flReader.Peek <> -1
                    strFileContent = flReader.ReadLine
                    intLineCount = intLineCount + 1
                End While

                Return intLineCount

            Catch ex As Exception

            Finally

                'Close The text File 
                If Not flReader Is Nothing Then
                    flReader.Close()
                Else
                    flReader = Nothing
                End If

            End Try

        End Function

#End Region

#Region "Password Rules Methods "

        '****************************************************************************************************
        'Function Name  : fncIsCharacter()
        'Purpose        : To Check Only Characters
        'Arguments      : Password
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 19/04/2004
        '*****************************************************************************************************
        Public Function fncIsCharacter(ByVal strContent As String) As Boolean

            'Variable Declarations
            Dim intCounter As Int16, IsCharacter As Boolean

            Try

                'Set Status to False
                IsCharacter = True

                'Loop Thro Entire Password To Check if Character exist - Start
                For intCounter = 1 To Len(strContent)
                    If IsNumeric(Mid(strContent, intCounter, 1)) Then
                        IsCharacter = False
                        Exit For
                    End If
                Next
                'Loop Thro Entire Password To Check if Character exist - Stop

                Return IsCharacter

            Catch ex As Exception

            End Try

        End Function

        '****************************************************************************************************
        'Function Name  : fncIsNumber()
        'Purpose        : To Check Only Characters
        'Arguments      : Password
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 19/04/2004
        '*****************************************************************************************************
        Public Function fncIsNumber(ByVal strContent As String) As Boolean

            'Variable Declarations
            Dim intCounter As Int16, IsNumber As Boolean

            Try

                'Set Status to False
                IsNumber = True

                'Loop Thro Entire Password To Check if Number exist - Start
                For intCounter = 1 To Len(strContent)
                    If Not IsNumeric(Mid(strContent, intCounter, 1)) Then
                        IsNumber = False
                        Exit For
                    End If
                Next
                'Loop Thro Entire Password To Check if Number exist - Stop

                Return IsNumber

            Catch ex As Exception

            End Try

        End Function

#End Region

#Region "Get Column/Position"

        '****************************************************************************************************
        'Procedure Name : fncGetColPos
        'Purpose        : To get the Column, Start or End Position
        'Arguments      : Field Id, Data Set,Request Type
        'Return Value   : Integer
        'Author         : Sujith Sharatchandran - 
        'Created        : 06/05/2005
        '***************************************************************************************************
        Private Function fncGetColPos(ByVal intFieldId As Int16, ByVal dsColPos As DataSet, ByVal strRequest As String) As Int16

            'Create Instance of Data Row
            Dim drColPos As System.Data.DataRow

            'Declare Variables
            Dim intColPos As Int16

            Try

                'Loop Thro The Data Set to Get the Column or Start/End Position - START
                For Each drColPos In dsColPos.Tables(0).Rows
                    'If Field Id Matches
                    If drColPos("FID") = intFieldId Then
                        'If Request For Column Position
                        If strRequest = "COLUMN" Then
                            intColPos = drColPos("FCPOS")
                            'If Request for Start Position
                        ElseIf strRequest = "START" Then
                            intColPos = drColPos("FSPOS")
                            'If Request for End Position
                        ElseIf strRequest = "END" Then
                            intColPos = drColPos("FEPOS")
                        End If
                        'Exit For When Condition Satisfied
                        Exit For
                    End If
                Next
                'Loop Thro The Data Set to Get the Column or Start/End Position - STOP

                Return intColPos

            Catch ex As Exception

                Return 0

            Finally

                'Destroy Instance of System Data Row
                drColPos = Nothing

            End Try

        End Function
#End Region

#Region "Get CIMB Billing File Batch No."

        '****************************************************************************************************
        'Function Name  : fncGetBillingFileBatchNo
        'Purpose        : Calculate the Batch Number for CIMB Billing File
        'Arguments      : 
        'Return Value   : Batch No
        'Author         : Marcus Yap 
        'Created        : 30/07/2007
        '*****************************************************************************************************
        Private Function fncGetBillingFileBatchNo() As String

            'Create Instance of SQL Command Object
            Dim cmdField As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim sDate As String
            Dim sRetVal As String

            sDate = DateTime.Now.ToString("yyyy-MM-dd")

            Try

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdField
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_GetBillingFileBatchNo"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@dtDate", sDate))
                    .Parameters.Add(New SqlParameter("@out_BatchNo", SqlDbType.VarChar, 2, ParameterDirection.Output, False, 0, 0, "out_BatchId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                sRetVal = cmdField.Parameters("@out_BatchNo").Value

                'Destroy SQL Command Object
                cmdField = Nothing

                'Terminate SQL Connection
                Call SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                Return sRetVal

            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_UserID), "fncGetBillingFileBatchNo -  clsUpload", Err.Number, Err.Description)

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL command Object
                cmdField = Nothing

                Return "Error"

            End Try


        End Function

#End Region

#Region "Finalize"

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

#End Region

#Region "Get DB Table Name"
        '071127 Function created by Marcus
        Public Shared Function fncGetDBTableName(ByVal strCustFileId As String) As String

            Dim strSQL As String

            Dim clsGeneric As New MaxPayroll.Generic
            Dim objRetVal As New Object

            'strSQL = "select mCor_BankFormat.TableName from mCor_BankFormat" & _
            '        " inner join mCor_CustomerFormat on mCor_BankFormat.BankId = mCor_CustomerFormat.BankId" & _
            '        " and mCor_BankFormat.FileType = mCor_CustomerFormat.File_Type" & _
            '        " Where mCor_CustomerFormat.File_Id = " & clsDB.SQLStr(strCustFileId)
            strSQL = "SELECT mCor_BankFormat.TableName FROM mCor_BankFormat " & _
                    "INNER JOIN mCor_CustomerFormat ON mCor_BankFormat.FileType = mCor_CustomerFormat.File_Type " & _
                    "WHERE mCor_CustomerFormat.File_Id = " & clsDB.SQLStr(strCustFileId)

            objRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)

            If IsDBNull(objRetVal) OrElse objRetVal = Nothing Then
                Return gc_Status_Error
            Else
                Return objRetVal.ToString
            End If

        End Function
#End Region

#Region "Get Mapped Bank Code"
        '071201 Function created by Marcus
        Private Shared Function fncGetMappedBankCode(ByVal strCustomerBankCode As String, ByVal lngOrgId As Long) As String
            Dim strSql As String
            Dim strRetVal As String
            Dim clsGeneric As New Generic

            strSql = "select mCor_BankDefinition.BankCode from tCor_BankMapping" & _
                        " inner join mCor_BankDefinition on tCor_BankMapping.BankId = mCor_BankDefinition.BankId" & _
                        " where tCor_BankMapping.CustomerBankCode =" & clsDB.SQLStr(strCustomerBankCode) & _
                        " and tCor_BankMapping.Org_Id =" & lngOrgId

            Try
                strRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection(), CommandType.Text, strSql)

            Catch ex As Exception
                strRetVal = gc_Status_Error
            End Try

            Return strRetVal
            clsGeneric = Nothing

        End Function
#End Region

#Region "Validate Account Number"
        '071201 Function created by Marcus
        Private Shared Function fncValidateGroupAccountNo(ByVal intGroupId As Integer, ByVal strAccountNo As String) As Boolean
            Dim strSql As String
            Dim intRetVal As Integer
            Dim clsGeneric As New Generic

            strSql = "select count(0) from mCor_BankAccounts" & _
                        " inner join tCor_GroupAccounts on mCor_BankAccounts.Account_Id = tCor_GroupAccounts.Account_Id" & _
                        " where mCor_BankAccounts.Account_No =" & clsDB.SQLStr(strAccountNo) & " and tcor_groupaccounts.Group_Id=" & intGroupId

            Try
                intRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection(), CommandType.Text, strSql)

            Catch ex As Exception
                clsGeneric.ErrorLog(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_UserID), "fncValidateGroupAccountNo -  clsUpload", Err.Number, Err.Description)
            End Try

            If intRetVal > 0 Then
                Return True
            Else
                Return False
            End If

            clsGeneric = Nothing

        End Function
#End Region

#Region "Validate Bank Org Code"

        '071201 Function created by Marcus
        Private Shared Function fncValidateBankOrgCode(ByVal lngOrgId As Long, ByVal strAccountNo As String, ByVal strOrgCode As String) As Boolean
            Dim strSql As String
            Dim intRetVal As Integer
            Dim clsGeneric As New Generic

            strSql = "select count(0) from tCor_BankOrgAccounts" & _
                    " where Org_ID = " & lngOrgId & " and Account_No = " & clsDB.SQLStr(strAccountNo) & _
                    " and BankOrgCode = " & clsDB.SQLStr(strOrgCode)

            Try
                intRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection(), CommandType.Text, strSql)

            Catch ex As Exception
                clsGeneric.ErrorLog(HttpContext.Current.Session(gc_Ses_OrgId), HttpContext.Current.Session(gc_Ses_UserID), "fncValidateGroupAccountNo -  clsUpload", Err.Number, Err.Description)
            End Try

            If intRetVal > 0 Then
                Return True
            Else
                Return False
            End If

            clsGeneric = Nothing

        End Function

#End Region

#Region "Get File Batch Number"
        Private Shared Function fncGetBatchNumner(ByVal strFileType As String) As String

            Dim intRetVal As Integer
            Dim strDay As String = IIf(Now.Day.ToString.Length > 2, Now.Day, "0" & Now.Day.ToString)
            Dim strMonth As String = IIf(Now.Month.ToString.Length > 2, Now.Month, "0" & Now.Month.ToString)
            Dim strYear As String = Left(Now.Year.ToString, 2)

            Dim strSql As String = "select  count(0) from tPgt_FileDetails" & _
                                    " where FileDtTm >= " & clsDB.SQLStr(Now.ToShortDateString & " 00:00", clsDB.SQLDataTypes.Dt_DateTime) & _
                                    " and FileDtTm < " & clsDB.SQLStr(Now.AddDays(1).ToShortDateString & " 00:00", clsDB.SQLDataTypes.Dt_DateTime) & _
                                    " and FileType= " & clsDB.SQLStr(strFileType)

            intRetVal = SqlHelper.ExecuteScalar(Generic.sSQLConnection(), CommandType.Text, strSql)

            Return strDay & strMonth & strYear & intRetVal.ToString

        End Function
#End Region

#Region "EFP New or Supp. Contribution Checking"
        Private Shared Function fncEPFNewSupp(ByVal lngOrgId As Long, ByVal intConMonth As Integer, ByVal intconyear As Integer) As Char

            Dim strSql As String
            Dim intRetVal As Integer
            Dim strConMonth As String

            strConMonth = MonthName(intConMonth, True) & " " & intconyear

            strSql = "select count(0) from tPgt_FileDetails where OrgId = " & clsDB.SQLStr(lngOrgId, clsDB.SQLDataTypes.Dt_Integer) & _
                        " and conMonth = " & clsDB.SQLStr(strConMonth) & " and FileType= " & clsDB.SQLStr("EPF File")

            intRetVal = SqlHelper.ExecuteScalar(Generic.sSQLConnection, CommandType.Text, strSql)

            If intRetVal = 0 Then
                Return "N"
            Else
                Return "S"
            End If

        End Function
#End Region

#Region "BNM Code validation"
        Private Shared Function fncBNMValid(ByVal sBNMCode As String) As Boolean

            Dim intRetval As Integer = 0

            If sBNMCode.Length < 7 OrElse sBNMCode.Substring(2) <> "00000" OrElse IsNumeric(sBNMCode) = False Then
                Return False
            Else
                Dim strSql As String
                Dim clsGeneric As New Generic

                strSql = "select count(0) from mCor_BNM where BNMCode = '" & sBNMCode.Substring(0, 2) & "'"
                intRetval = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection(), CommandType.Text, strSql)
                clsGeneric = Nothing
            End If

            If intRetval > 0 Then
                Return True
            Else
                Return False
            End If


        End Function
#End Region

#Region "Calculate Direct Debit Hash Total"
        Private Shared Function fncDDebitHashTotal(ByVal strTotalAmount As String) As String
            'Dim i As Integer
            'Dim j As Integer = 5
            'Dim strRetVal As String = ""

            'strTotalAmount = strTotalAmount.Replace(".", "")
            'strTotalAmount = Strings.Format(CInt(strTotalAmount), "000000000000000")
            'For i = 0 To 14
            '    strRetVal = strRetVal & (CInt(strTotalAmount.Chars(i).ToString) * j)
            '    j = IIf(j = 5, 1, 5)
            'Next

            strTotalAmount = Strings.Format(CLng(strTotalAmount), "000000000000000")

            Return strTotalAmount

        End Function
#End Region

#Region "Updata Bank Org Code for Direct Debit"
        Private Shared Sub UpdateOrgCode(ByVal strBankOrgCode As String, ByVal lngFileId As Long)
            Dim strSql As String = "Update tPgt_FileDetails set BankOrgCode = '" & strBankOrgCode & "'" & _
                                    " Where FileId = " & lngFileId

            SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, strSql)

        End Sub
#End Region

#Region "Unique Reference No "

        'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
        'Created        : 02/12/2008
        'Purpose        : To Generate Unique Reference No
        Private Function GetUniqueRefNo(ByVal lngOrgId As Long, ByVal lngUserId As Long, _
            ByVal FieldValue As String, ByVal StartPosition As Short, _
            ByVal EndPosition As Short, ByVal RowIndex As Integer) As String

            'Create Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                If FieldValue.Trim() = "" Then
                    FieldValue = RowIndex + 1
                    FieldValue = fnFiller(FieldValue, StartPosition, EndPosition, True) & FieldValue
                End If

                Return FieldValue

            Catch ex As Exception

                'Log Error
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "GetUniqueRefNo - clsUpload", Err.Number, Err.Description)

                Return String.Empty

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Function

#End Region

#Region " Content Validate Check"

        'Author         : Nareshwar- T-Melmax Sdn Bhd
        'Purpose        : to check whether the file is uploaded with same paydate,tmat,orgcode,totaltrans or not  
        'Created        : 13/04/2010

        'Check if any file is existing with same payment date,totalamt,total trans and OrgC

        Public Function ContentDuplicateCheck(ByVal OrgCode As String, ByVal PaymentDate As String, _
                ByVal TotalAmount As Double, ByVal TotalTrans As Integer) As Integer

            Dim SqlStatement As String = Nothing
            Dim count As Integer = 0
            'variable declaration -stop

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Dim MaxGeneric As New MaxGeneric.clsGeneric
            'initialize sql connection
            clsGeneric.SQLConnection_Initialize()

            Try

                'Build SqlStatement 
                SqlStatement = _Helper.GetCommonSQL & " " & "'" & _Helper.ValidateUserData & "'" & "," & OrgCode & "," & "'" & PaymentDate & "'" & "," & TotalAmount & "," & TotalTrans

                count = MaxMiddleware.PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, String.Empty, SqlStatement)

                'Return count of  files alredy uploaded with same content as per validatoin rules

                Return count

            Catch ex As Exception

                Return gc_Status_Error

            Finally

                'Terminate SqlConnection
                clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing


            End Try


        End Function
#End Region

    End Class

End Namespace
