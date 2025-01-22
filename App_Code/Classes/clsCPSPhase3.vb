Imports Microsoft.VisualBasic
Imports MaxMiddleware
Imports System.Configuration.ConfigurationManager
Imports System
Imports MaxGeneric
Imports MaxPayroll
Imports System.Web
Imports System.Data
Imports System.Configuration
Imports MaxReadWrite
Imports System.Data.SqlClient
'****************************************************************************************************
'PageName       : clsCPSPhase3
'Purpose        : All Functions Related to CIMBGW CPS Phase III
'Author         : Mohamad Hafeez Zakaria 
'Created        : 2009-04-24
'*****************************************************************************************************

Public Class clsCPSPhase3
    Private _Helper As New Helper
    Private _ReportHelper As New ReportHelper
    Dim clsCommon As New MaxPayroll.clsCommon
    Private _clsReadFile As New ReadFile
    Private _clsInsertData As New InsertData
    Private _ReadWriteHelper As New MaxReadWrite.CPSHelper
    Private _clsWrite As New MaxReadWrite.WriteFile
    Private _clsCommon As New MaxReadWrite.Common
    Private _Maxhelper As New MaxGateway.Helper



#Region "Enumarator"
    Enum enmTableName
        tpgt_Workflow
        tpgt_FileDetails
        tcor_PaylinkUploaded_Trailer
        tcor_PaylinkUploaded
        tcor_CPSDividendUploaded
        tcor_CPSMemberUploaded
        CIMBGW_MemberDividend_Relation
    End Enum
    Enum enmMiniStatement
        GetDividendFileId = 1
        GetWorkFlowId = 2
        GetFileGivenName = 3
        Update_tpgtFiledetails_Status = 4
        GetMemDiv_Relation = 5
        GetCountChequeNo = 6
        GetTotalTrans_tpgtFiledetails = 7
        Insert_MemDiv_Relation = 8
        GetDuplicateChequeNo = 9
        Update_MFiledetails_Status = 11
    End Enum



    Enum enmCPSFileStatus
        'FLOW WEB Files - After Customer Aprrove(5) -> 100 -> 101
        'FLOW H2H Files - After Customer Aprrove(7) -> 100 -> 101
        Cheque_No_Assign = 100
        Cheque_No_Confirm = 101

    End Enum
#End Region

#Region "Generate Charges Footer"
    Public Function GenerateCharges(ByVal CPScount As Integer, ByVal lngOrgId As Long)

        Dim dtCharges As New DataTable
        Dim i As Integer

        Dim From_No As String
        Dim To_No As String
        Dim Charges_Set As String
        'Dim Total_Charges As Decimal
        Dim CPS_Footer As String = ""

        dtCharges = PPS.GetData(_Helper.GetChargesSQL & " " & lngOrgId & ",0", _Helper.GetSQLConnection, _
                           _Helper.GetSQLTransaction)
        If dtCharges.Rows(0)("Charge_Type") = 2 Then

            If dtCharges.Rows.Count > 0 Then
                For i = 0 To dtCharges.Rows.Count - 1
                    From_No = dtCharges.Rows(i)("Tier_TransFrom")
                    To_No = dtCharges.Rows(i)("Tier_TransTo")
                    Charges_Set = dtCharges.Rows(i)("Tier_Charges")



                    'start fill- Remarks:Web.config

                    Charges_Set = Replace(Charges_Set, ".", "")
                    Charges_Set = Left(Charges_Set, Len(Charges_Set) - 2)

                    From_No = fnFiller(From_No, 1, 12, True)
                    To_No = fnFiller(To_No, 1, 12, True)
                    Charges_Set = fnFiller(Charges_Set, 1, 15, True)

                    'Total_Charges = ((Convert.ToDecimal(Charges_Set) * (Convert.ToDecimal(To_No) - Convert.ToDecimal(From_No -1)))
                    CPScount = CPScount - To_No

                    'end fill

                    CPS_Footer &= From_No & " " & To_No & " " & Charges_Set & Environment.NewLine

                Next


            End If


        ElseIf dtCharges.Rows(0)("Charge_Type") = 1 Then
            'fixed charges

            Charges_Set = dtCharges.Rows(0)("Fixed_Charges")
            Charges_Set = Replace(Charges_Set, ".", "")
            Charges_Set = Left(Charges_Set, Len(Charges_Set) - 2)

            CPS_Footer &= "Fixed" & Charges_Set & Environment.NewLine

        End If
        Return CPS_Footer
    End Function
#End Region

#Region "Store file Charges"
    Public Function InsertCharges_Record(ByVal lngOrgId As Long, ByVal lngFileId As Long) As Boolean
        Try


            Dim dtCharges As New DataTable
            Dim dtChargeRecord
            Dim ChargeType As Integer
            Dim i As Integer
            Dim Charge_ID As Integer
            dtCharges = PPS.GetData(_Helper.GetChargesSQL & " " & lngOrgId & ",0", _Helper.GetSQLConnection, _
                                               _Helper.GetSQLTransaction)
            If dtCharges.Rows.Count > 0 Then
                For i = 0 To dtCharges.Rows.Count - 1

                    ChargeType = dtCharges.Rows(i)("Charge_Type")
                    Charge_ID = dtCharges.Rows(i)(_Helper.ChargeIDCol)
                    dtChargeRecord = PPS.GetData(_Helper.InsFileChargesSQL & " " & lngFileId & "," & Charge_ID & "," & ChargeType, _Helper.GetSQLConnection, _
                                                      _Helper.GetSQLTransaction)

                Next
            End If

            Return True

        Catch ex As Exception

        End Try




    End Function



#End Region

#Region "Validation"
    Public Function CheckOrgCharges(ByVal lngOrgId As Long)
        Dim dtCharges As New DataTable
        Dim CPSChargesExist As Boolean = False
        dtCharges = PPS.GetData(_Helper.GetChargesSQL & " " & lngOrgId & ",0", _Helper.GetSQLConnection, _
                           _Helper.GetSQLTransaction)

        If dtCharges.Rows.Count > 0 Then
            CPSChargesExist = True
        End If

        Return CPSChargesExist
    End Function
#End Region

#Region "FieldName"

    Public Function TypeWEB_Name() As String

        Dim Name As String = Nothing
        Name = "WEB"
        Return Name
    End Function
    Public Function TypeH2H_Name() As String

        Dim Name As String = Nothing
        Name = "H2H"
        Return Name
    End Function
    Public Function ChequeNo_Name() As String

        Dim Name As String = Nothing
        Name = "Cheque_No"
        Return Name
    End Function
    Public Function DChequeNo() As String

        Dim Name As String = Nothing
        Name = "DIV_WRNTNO"
        Return Name
    End Function

    Public Function DGross1() As String

        Dim Name As String = Nothing
        Name = "DIV_GROSS1"
        Return Name
    End Function
    Public Function DGross2() As String
        Dim Name As String = Nothing
        Name = "DIV_GROSS2"
        Return Name
    End Function
    Public Function DGross3() As String
        Dim Name As String = Nothing
        Name = "DIV_GROSS3"
        Return Name
    End Function
    Public Function DGross4() As String
        Dim Name As String = Nothing
        Name = "DIV_GROSS4"
        Return Name
    End Function
    Public Function DTax1() As String
        Dim Name As String = Nothing
        Name = "DIV_TAX1"
        Return Name
    End Function
    Public Function DTax2() As String
        Dim Name As String = Nothing
        Name = "DIV_TAX2"
        Return Name
    End Function
    Public Function DTax3() As String
        Dim Name As String = Nothing
        Name = "DIV_TAX3"
        Return Name
    End Function
    Public Function DTax4() As String
        Dim Name As String = Nothing
        Name = "DIV_TAX4"
        Return Name
    End Function
    Public Function DNet1() As String
        Dim Name As String = Nothing
        Name = "DIV_NET1"
        Return Name
    End Function

    Public Function DNet2() As String
        Dim Name As String = Nothing
        Name = "DIV_NET2"
        Return Name
    End Function
    Public Function DNet3() As String
        Dim Name As String = Nothing
        Name = "DIV_NET3"
        Return Name
    End Function
    Public Function DNet4() As String
        Dim Name As String = Nothing
        Name = "DIV_NET4"
        Return Name
    End Function
    Public Function DTOTNET() As String
        Dim Name As String = Nothing
        Name = "DIV_TOTNET"
        Return Name
    End Function
#End Region

#Region "P3 Error Messages"
    Public Function Msg_CPSLineErrorMessage(ByVal gIntErrorMsgRowNo As Integer, ByVal strDescription As String) As String
        Dim ErrorMsg As String = Nothing


        ErrorMsg = "Line " & gIntErrorMsgRowNo + 1 & " does not have correct " & strDescription & " Value." & Environment.NewLine
        Return ErrorMsg
    End Function
    Public Function Msg_CPSChargesEmpty()
        Dim ErrorMsg As String = Nothing
        ErrorMsg = "There is no charges set for this Organization. Please contact Bank Administartor" & Environment.NewLine
        Return ErrorMsg
    End Function
    Public Function Msg_ValidateContent()
        Dim ErrorMsg As String = Nothing
        ErrorMsg = "Error Code: P3001 - Unable to Validate File" & Environment.NewLine
        Return ErrorMsg
    End Function
    Public Function Msg_HashTotal()
        Dim ErrorMsg As String = Nothing
        ErrorMsg = "Error Code: C3001 - Hash Total not correct" & Environment.NewLine
        Return ErrorMsg
    End Function
#End Region

#Region "Filler"
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

        Return strFiller & strContent

    End Function
#End Region

#Region "Store Procedure"

    Public ReadOnly Property SQL_InsSkipChequeNo() As String
        Get
            Return "EXEC " & AppSettings("SQL_InsSkipChequeNo") & " "
        End Get
    End Property

    Public ReadOnly Property SQL_MiniStatement() As String
        Get
            Return "EXEC " & AppSettings("SQL_MiniStatement") & " "
        End Get
    End Property
    Public ReadOnly Property SQL_UpdateChequeNumber() As String
        Get
            Return "EXEC " & AppSettings("SQL_UpdChequeNumber") & " "
        End Get
    End Property
    Public ReadOnly Property SQL_InsertChequeNumber() As String
        Get
            Return "EXEC " & AppSettings("SQL_InsChequeNumber") & " "
        End Get
    End Property
    Public ReadOnly Property SQL_CountNumberRecords() As String
        Get
            Return "EXEC " & AppSettings("SQL_CountNumberRecords") & " "
        End Get
    End Property
    Public ReadOnly Property SQL_GetNoRejectedCheque() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetNoRejectedRecord") & " "
        End Get
    End Property
    Public ReadOnly Property SQL_GetNextChequeNumber() As String
        Get
            Return "EXEC " & AppSettings("SQL_GetNextChequeNo") & " "
        End Get
    End Property
    Public ReadOnly Property CPSChequeDetailsSQL() As String
        Get
            Return "EXEC CIMBGW_CPS_Cheque_Details "
        End Get
    End Property
#End Region

#Region "Cheque Details Report "

    'Purpose		: Mandate Resgistration Details
    'Author			: Bhanu Teja/Hafeez - T-Melmax Sdn Bhd
    'Created Date	: 31/10/2008
    Public Function Report_GetChequeDetails(ByVal SearchType As Short, _
        ByVal SearchOption As Short, ByVal ParamArray SQLParams As String()) As DataTable

        'Create Instance of Data Table
        Dim MandateBillingDetails As DataTable = Nothing

        'Variable Declarations
        Dim SQLStatement As String = Nothing, Index As Short = 0

        Try

            'Build SQL Statement - Start
            SQLStatement = CPSChequeDetailsSQL
            SQLStatement &= SearchOption & "," & _ReportHelper.SessionOrgId

            'loop thro the param array - Start
            For Index = 0 To SQLParams.GetUpperBound(0)
                SQLStatement &= ",'" & clsGeneric.NullToString(SQLParams(Index)) & "'"
            Next
            'loop thro the param array - Stop
            'Build SQL Statement - Stop

            'Get Mandate Registration Details
            Return PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)

        Catch ex As Exception

            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            'Call clsGeneric.ErrorLog(OrgId, lngUserId, "GetMandateBillingDetails - ClsReportHelper", Err.Number, Err.Description)
            Return MandateBillingDetails


        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Function


#End Region

#Region "Monthly Cheque Status Report "

    'Purpose		: Mandate Resgistration Details
    'Author			: Bhanu Teja/Hafeez - T-Melmax Sdn Bhd
    'Created Date	: 31/10/2008
    Public Function Report_MonthlyChequeStatus(ByVal SearchType As Short, _
        ByVal SearchOption As Short, ByVal ParamArray SQLParams As String()) As DataTable

        'Create Instance of Data Table
        Dim MandateBillingDetails As DataTable = Nothing

        'Variable Declarations
        Dim SQLStatement As String = Nothing, Index As Short = 0

        Try

            'Build SQL Statement - Start
            SQLStatement = CPSChequeDetailsSQL
            SQLStatement &= SearchOption & "," & _ReportHelper.SessionOrgId

            'loop thro the param array - Start
            For Index = 0 To SQLParams.GetUpperBound(0)
                SQLStatement &= ",'" & clsGeneric.NullToString(SQLParams(Index)) & "'"
            Next
            'loop thro the param array - Stop
            'Build SQL Statement - Stop

            'Get Mandate Registration Details
            Return PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)

        Catch ex As Exception

            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            'Call clsGeneric.ErrorLog(OrgId, lngUserId, "GetMandateBillingDetails - ClsReportHelper", Err.Number, Err.Description)


        Finally

            'force garbage collection
            GC.Collect(0)
        End Try

    End Function


#End Region

#Region "Get Cheque Next Number"
    Public Function ChequeGetNextNo(ByVal OrgId As Integer)
        Dim dtCPSCheque As New DataTable
        Dim NextNumber As Long
        Dim CatchMsg As String = ""




        dtCPSCheque = PPS.GetData(SQL_GetNextChequeNumber & OrgId & "," & GetBatchNo(OrgId), _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)
        If dtCPSCheque.Rows.Count > 0 Then
            NextNumber = dtCPSCheque.Rows(0)(ChequeNo_Name) + 1
            NextNumber = fnFiller(NextNumber, 1, 6, True)
            'ElseIf (dtCPSCheque.Rows(0)(ChequeNo_Name) + 1) = 1000000 Then
            '    NextNumber = 1
        Else
            NextNumber = 1
        End If


        Return NextNumber
    End Function
#End Region

#Region "CPS Upload Portion"

    Public Function CPS_Upload_File(ByVal strFileName As String, ByVal strSubFileName As String, ByVal strFileType As String, _
    ByVal intValueDay As Int16, ByVal intValueMonth As Int16, ByVal intValueYear As Int16, ByVal lngOrgId As Long, _
    ByVal lngUserId As Long, ByVal intGroupId As Int32, ByVal lngAccId As Long, ByVal lngFormatId As Long, ByVal dcTranCharge As Decimal, _
    ByVal strIPAddr As String, ByVal AccNumber As String, Optional ByVal intSerAccId As Int16 = 0) As String


        ''Declare Instances Start
        Dim ValidateContents As DataSet = Nothing
        Dim SubValidateContents As DataSet = Nothing
        Dim DtCustId As New DataTable, DtSubCustId As New DataTable
        Dim UnmatchRows As New DataTable
        ''Declare Instances Stop

        ''Declare Variable Start
        Dim strRetVal As String, lngFileId As Long, lngSubFileId As Long, strIc As String = "N", strGivenName As String = Nothing, _
            strSubGivenName As String = Nothing, strFile As String = Nothing, strSubFile As String = Nothing, _
                strConMonth As String = "", gdblTotalAmount As Double = 0, ErrorMsg As String = Nothing, _
                    InsertStatus As String = "", InsertSubStatus As String = "", WriteStatus As String = Nothing, _
                        CompareStatus As String = "", dtValueDate As Date
        ''Declare Variable End     



        Try
            ''Process Insert Dividend File Start

            ''Get Date
            dtValueDate = intValueDay & "/" & intValueMonth & "/" & intValueYear
            ''Get Generated File Name
            strFile = GetExtension(strFileType, True, False) & lngOrgId.ToString & _
                                Format(intValueYear, "0000") & Format(intValueMonth, "00") & Format(intValueDay, "00") & _
                                GetExtension(strFileType, False, True)

            ''Get Original File Name
            strGivenName = clsCommon.fncFileName(strFileName, False)


            '*******PayLink PayRoll file reading -start
            '*******Added by Naresh
            '*******Date:26-11-10
            If strFileType = _Helper.PayLinkPayRoll_Name Then

                DtCustId = PPS.GetData(_Helper.GetCustIDSQL & "'" & _Helper.PayLinkPayRoll_Name & "'," & lngOrgId, _Helper.GetSQLConnection, _
                                                    _Helper.GetSQLTransaction)

                ''Insert Dividend File Into tpgt_filedetails start
                strRetVal = clsCommon.fncFileDetailsSeqNo("ADD", 0, _Helper.PayLinkPayRoll_Name, strGivenName, strFile, lngOrgId, _
                        lngUserId, dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, DtCustId.Rows(0)(_Helper.FileIdCol), _
                                strIc, intSerAccId)
                ''Get 1st File ID
                lngFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))

                ''Process Insert Dividend File Stop
                ValidateContents = _clsReadFile.FileContentsToDataset(lngOrgId, strFileName, DtCustId.Rows(0)(_Helper.FileIdCol), _
                            DtCustId.Rows(0)(_Helper.FileTypeIdCol), ErrorMsg, AccNumber, intValueDay, intValueMonth, intValueYear, strFileType)

                If ErrorMsg = Nothing And Not ValidateContents Is Nothing Then
                    If Not ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString()).Rows.Count > 0 Then
                        ErrorMsg = Msg_ValidateContent()
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        Exit Try
                    End If
                    ''Write File To Database
                    InsertStatus = _clsInsertData.WriteToDatabase(ValidateContents, DtCustId.Rows(0)(_Helper.FileTypeIdCol), _
                            lngOrgId, lngFileId)
                    ''Check If Errors
                    If InsertStatus <> Nothing Then
                        ''Delete Transaction If Error
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        ErrorMsg = InsertStatus
                        Exit Try
                    Else
                        ''Set Status To Ok
                        ErrorMsg = gc_Status_OK
                        ''Insert Charges File Id
                        InsertCharges_Record(lngOrgId, lngFileId)
                        ''Update tpgt_FileDetails
                        UpdTpgtDetails(ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString), lngFileId, dtValueDate, _Helper.PayLinkPayRoll_Name)

                        '** Added by Naresh on -10-02-11
                        'Create Bank format file -start
                        WriteStatus = _clsWrite.WriteToFile(lngOrgId, lngFileId, strFileType)
                        'Create Bank format file -start

                        If WriteStatus <> Nothing Then

                            ''Delete transaction From the tables
                            DeleteTransaction(lngFileId, enmTableName.tcor_PaylinkUploaded_Trailer.ToString)
                            DeleteTransaction(lngFileId, enmTableName.tcor_PaylinkUploaded.ToString)
                            DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                            ErrorMsg = WriteStatus

                            Exit Try
                        End If
                        ''Check If Errors and create the file -stop

                        ''Insert into Workflow
                        Call clsCommon.prcWorkFlow(lngFileId, 0, lngOrgId, lngUserId, "U", "N", "", strIPAddr, gdblTotalAmount, intGroupId, "Y")


                        ''Send Mail if Delimited Dividend File
                        _Helper.Uploader_SendMail(lngOrgId, lngUserId, intGroupId, strFileType, strGivenName, dtValueDate)



                    End If
                Else
                    ''Delete transaction From tpgt_FileDetails
                    DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                    Exit Try
                End If

                '*******PayLink PayRoll file reading -start

            ElseIf strFileType = _Helper.CPSSingleFileFormat_Name Then
                Dim HashTotal As String = ""

                DtCustId = PPS.GetData(_Helper.GetCustIDSQL & "'" & _Helper.CPSSingleFileFormat_Name & "'," & lngOrgId, _Helper.GetSQLConnection, _
                                                               _Helper.GetSQLTransaction)

                ''Insert SFF File Into tpgt_filedetails start
                strRetVal = clsCommon.fncFileDetailsSeqNo("ADD", 0, _Helper.CPSSingleFileFormat_Name, strGivenName, strFile, lngOrgId, _
                        lngUserId, dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, DtCustId.Rows(0)(_Helper.FileIdCol), _
                                strIc, intSerAccId)
                ''Get 1st File ID
                lngFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))

                ''Process Insert Dividend File Stop
                ValidateContents = _clsReadFile.FileContentsToDataset(lngOrgId, strFileName, DtCustId.Rows(0)(_Helper.FileIdCol), _
                            DtCustId.Rows(0)(_Helper.FileTypeIdCol), AccNumber, intValueDay, intValueMonth, intValueYear, ErrorMsg)

                If ErrorMsg = Nothing And Not ValidateContents Is Nothing Then
                    If Not ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString()).Rows.Count > 0 Then
                        ErrorMsg = Msg_ValidateContent()
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        Exit Try
                    End If

                    ''Validate HashTotal Start
                    'Convert Amount Column to Decimal from String and add to table - Start
                    ValidateContents.Tables(1).Columns.Add(_Helper.SFF_DivTotalNetCol & "_Hash", GetType(Decimal), _
                        "CONVERT([" & _Helper.SFF_DivTotalNetCol & "],'System.Decimal')")
                    'Convert Amount Column to Decimal from String and add to table - Stop


                    If clsGeneric.NullToInteger(ValidateContents.Tables(2).Rows(0)("Hash Total")) <> _
                        (clsGeneric.NullToInteger(ValidateContents.Tables(1).Compute(("SUM([" & _Helper.SFF_DivTotalNetCol & "_Hash])"), "")) + _
                            clsGeneric.NullToInteger(ValidateContents.Tables(2).Rows(0)("Total Records"))) Then

                        ErrorMsg = Msg_HashTotal()
                    Else
                        HashTotal = fnFiller(ValidateContents.Tables(2).Rows(0)("Hash Total"), 1, 15, True)
                        ValidateContents.Tables(1).Columns.Remove(_Helper.SFF_DivTotalNetCol & "_Hash")

                    End If
                    ''Validate Hash Total Stop


                    If ErrorMsg = Nothing Then
                        ''Write File To Database
                        InsertStatus = _clsInsertData.WriteToDatabase(ValidateContents, DtCustId.Rows(0)(_Helper.FileTypeIdCol), _
                                lngOrgId, lngFileId)
                        ''Check If Errors
                        If InsertStatus <> Nothing Then
                            ''Delete Transaction If Error
                            DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                            ErrorMsg = InsertStatus
                            Exit Try
                        Else
                            ''Set Status To Ok
                            ErrorMsg = gc_Status_OK
                            ''Insert Charges File Id
                            InsertCharges_Record(lngOrgId, lngFileId)
                            ''Update tpgt_FileDetails
                            UpdTpgtDetails(ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString), lngFileId, dtValueDate, _Helper.CPSSingleFileFormat_Name, HashTotal)
                            ''Insert into Workflow
                            Call clsCommon.prcWorkFlow(lngFileId, 0, lngOrgId, lngUserId, "U", "N", "", strIPAddr, gdblTotalAmount, intGroupId, "Y")


                            ''Send Mail if Delimited Dividend File
                            _Helper.Uploader_SendMail(lngOrgId, lngUserId, intGroupId, strFileType, strGivenName, dtValueDate)



                        End If
                    End If
                Else
                    ''Delete transaction From tpgt_FileDetails
                    DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                    Exit Try
                End If
                ''Process Insert Delimited Dividend File Stop

                '' Delimited Dividend Stop



                '' Delimited Dividend Start
            ElseIf strFileType = _Helper.CPSDelimited_Dividen_Name Then
                DtCustId = PPS.GetData(_Helper.GetCustIDSQL & "'" & _Helper.CPSDelimited_Dividen_Name & "'," & lngOrgId, _Helper.GetSQLConnection, _
                                                    _Helper.GetSQLTransaction)

                ''Insert Dividend File Into tpgt_filedetails start
                strRetVal = clsCommon.fncFileDetailsSeqNo("ADD", 0, _Helper.CPSDelimited_Dividen_Name, strGivenName, strFile, lngOrgId, _
                        lngUserId, dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, DtCustId.Rows(0)(_Helper.FileIdCol), _
                                strIc, intSerAccId)
                ''Get 1st File ID
                lngFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))

                ''Process Insert Dividend File Stop
                ValidateContents = _clsReadFile.FileContentsToDataset(lngOrgId, strFileName, DtCustId.Rows(0)(_Helper.FileIdCol), _
                            DtCustId.Rows(0)(_Helper.FileTypeIdCol), AccNumber, intValueDay, intValueMonth, intValueYear, ErrorMsg)

                If ErrorMsg = Nothing And Not ValidateContents Is Nothing Then
                    If Not ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString()).Rows.Count > 0 Then
                        ErrorMsg = Msg_ValidateContent()
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        Exit Try
                    End If
                    ''Write File To Database
                    InsertStatus = _clsInsertData.WriteToDatabase(ValidateContents, DtCustId.Rows(0)(_Helper.FileTypeIdCol), _
                            lngOrgId, lngFileId)
                    ''Check If Errors
                    If InsertStatus <> Nothing Then
                        ''Delete Transaction If Error
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        ErrorMsg = InsertStatus
                        Exit Try
                    Else
                        ''Set Status To Ok
                        ErrorMsg = gc_Status_OK
                        ''Insert Charges File Id
                        InsertCharges_Record(lngOrgId, lngFileId)
                        ''Update tpgt_FileDetails
                        UpdTpgtDetails(ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString), lngFileId, dtValueDate, _Helper.CPSDelimited_Dividen_Name)
                        ''Insert into Workflow
                        Call clsCommon.prcWorkFlow(lngFileId, 0, lngOrgId, lngUserId, "U", "N", "", strIPAddr, gdblTotalAmount, intGroupId, "Y")


                        ''Send Mail if Delimited Dividend File
                        _Helper.Uploader_SendMail(lngOrgId, lngUserId, intGroupId, strFileType, strGivenName, dtValueDate)



                    End If
                Else
                    ''Delete transaction From tpgt_FileDetails
                    DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                    Exit Try
                End If
                ''Process Insert Delimited Dividend File Stop

                '' Delimited Dividend Stop


            ElseIf strFileType = _Helper.CPSDividen_Name Or strFileType = _Helper.CPSMember_Name Then



                DtCustId = PPS.GetData(_Helper.GetCustIDSQL & "'" & _Helper.CPSDividen_Name & "'," & lngOrgId, _Helper.GetSQLConnection, _
                                    _Helper.GetSQLTransaction)

                ''Insert Dividend File Into tpgt_filedetails start
                strRetVal = clsCommon.fncFileDetailsSeqNo("ADD", 0, _Helper.CPSDividen_Name, strGivenName, strFile, lngOrgId, _
                        lngUserId, dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, DtCustId.Rows(0)(_Helper.FileIdCol), _
                                strIc, intSerAccId)
                ''Get 1st File ID
                lngFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))

                ''Process Insert Dividend File Stop

                ''Process Validate Dividend File Start            
                ''Read File To Dataset
                ValidateContents = _clsReadFile.FileContentsToDataset(lngOrgId, strFileName, DtCustId.Rows(0)(_Helper.FileIdCol), _
                            DtCustId.Rows(0)(_Helper.FileTypeIdCol), AccNumber, intValueDay, intValueMonth, intValueYear, ErrorMsg)

                If ErrorMsg = Nothing And Not ValidateContents Is Nothing Then
                    If Not ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString()).Rows.Count > 0 Then
                        ErrorMsg = Msg_ValidateContent()
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        Exit Try
                    End If
                    ''Write File To Database
                    InsertStatus = _clsInsertData.WriteToDatabase(ValidateContents, DtCustId.Rows(0)(_Helper.FileTypeIdCol), _
                            lngOrgId, lngFileId)
                    ''Check If Errors
                    If InsertStatus <> Nothing Then
                        ''Delete Transaction If Error
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        ErrorMsg = InsertStatus
                        Exit Try
                    Else
                        ''Set Status To Ok
                        ErrorMsg = gc_Status_OK
                        ''Insert Charges File Id
                        InsertCharges_Record(lngOrgId, lngFileId)
                        ''Update tpgt_FileDetails
                        UpdTpgtDetails(ValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString), lngFileId, dtValueDate, _Helper.CPSDividen_Name)
                        ''Insert into Workflow
                        Call clsCommon.prcWorkFlow(lngFileId, 0, lngOrgId, lngUserId, "U", "N", "", strIPAddr, gdblTotalAmount, intGroupId, "Y")

                        If strFileType = _Helper.CPSDividen_Name Then
                            ''Send Mail if Dividend File, If "Member File", mail will be send after Member file process sucess
                            _Helper.Uploader_SendMail(lngOrgId, lngUserId, intGroupId, strFileType, strGivenName, dtValueDate)

                        End If

                    End If
                Else
                    ''Delete transaction From tpgt_FileDetails
                    DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                    Exit Try
                End If
                ''Process Insert Dividend File Stop

                If strFileType = _Helper.CPSMember_Name Then

                    strSubFile = _Helper.CPSMemberStartName & lngOrgId.ToString & _
                                 Format(intValueYear, "0000") & Format(intValueMonth, "00") & Format(intValueDay, "00") & _
                                 _Helper.CPSMemberExt

                    strSubGivenName = clsCommon.fncFileName(strSubFileName, False)
                    ''insert File 2 into tpgt_filedetails
                    strRetVal = clsCommon.fncFileDetailsSeqNo("ADD", 0, strFileType, strSubGivenName, strSubFile, lngOrgId, lngUserId, _
                                dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, lngFormatId, strIc, _
                                        intSerAccId, dcTranCharge)

                    lngSubFileId = CLng(strRetVal.Substring(0, strRetVal.IndexOf(",")))
                    dcTranCharge = GetOrgCharges(lngFileId, lngOrgId)
                    clsCommon.fncFileDetailsSeqNo("UPDATE", lngSubFileId, strFileType, strSubGivenName, strFile, lngOrgId, lngUserId, _
                                                dtValueDate, "", 0, "", "0", "0", strConMonth, intGroupId, lngAccId, lngFormatId, strIc, _
                                                        intSerAccId)

                    DtSubCustId = PPS.GetData(_Helper.GetCustIDSQL & "'" & strFileType & "'," & lngOrgId, _Helper.GetSQLConnection, _
                                        _Helper.GetSQLTransaction)
                    SubValidateContents = _clsReadFile.FileContentsToDataset(lngOrgId, strSubFileName, DtSubCustId.Rows(0)(_Helper.FileIdCol), _
                            DtSubCustId.Rows(0)(_Helper.FileTypeIdCol), AccNumber, intValueDay, intValueMonth, intValueYear, ErrorMsg)

                    If ErrorMsg = Nothing And Not SubValidateContents Is Nothing Then
                        If Not SubValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString()).Rows.Count > 0 Then
                            ErrorMsg = Msg_ValidateContent()
                            ''Delete Dividend File From tpgt_FileDetails
                            DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                            ''Delete Member File From tpgt_FileDetails
                            DeleteTransaction(lngSubFileId, enmTableName.tpgt_FileDetails.ToString)
                            ''Delete Dividend Workflow 
                            DeleteTransaction(lngFileId, enmTableName.tpgt_Workflow.ToString)
                            ''Delete Dividend Details
                            DeleteTransaction(lngFileId, enmTableName.tcor_CPSDividendUploaded.ToString)
                            Exit Try
                        End If
                        InsertSubStatus = _clsInsertData.WriteToDatabase(SubValidateContents, DtSubCustId.Rows(0)(_Helper.FileTypeIdCol), _
                            lngOrgId, lngSubFileId)
                        CompareStatus = _clsReadFile.CompareTables(lngOrgId, ValidateContents.Tables(0), _
                            DtCustId.Rows(0)(_Helper.FileTypeIdCol), SubValidateContents.Tables(0), _
                                DtSubCustId.Rows(0)(_Helper.FileTypeIdCol), UnmatchRows, strGivenName)

                        If InsertSubStatus <> Nothing Or CompareStatus <> Nothing Then
                            ''Delete Dividend File From tpgt_FileDetails
                            DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                            ''Delete Member File From tpgt_FileDetails
                            DeleteTransaction(lngSubFileId, enmTableName.tpgt_FileDetails.ToString)
                            ''Delete Dividend Workflow 
                            DeleteTransaction(lngFileId, enmTableName.tpgt_Workflow.ToString)
                            ''Delete Dividend Details
                            DeleteTransaction(lngFileId, enmTableName.tcor_CPSDividendUploaded.ToString)
                            ''Delete Memeber Details
                            DeleteTransaction(lngSubFileId, enmTableName.tcor_CPSMemberUploaded.ToString)
                            ''Set Error Msg
                            ErrorMsg = InsertSubStatus & CompareStatus

                            Exit Try
                        Else
                            ''Set Status Ok
                            ErrorMsg = gc_Status_OK
                            ''Insert into Wokflow
                            Call clsCommon.prcWorkFlow(lngSubFileId, 0, lngOrgId, lngUserId, "U", "N", "", strIPAddr, gdblTotalAmount, intGroupId, "Y")
                            ''Update tpgt_FileDetails
                            UpdTpgtDetails(SubValidateContents.Tables(MaxReadWrite.CPSHelper.FileContentType.File_Body.ToString), lngSubFileId, dtValueDate, strFileType)
                            ''Insert Relation FileId
                            InsertMemDiv_Relation(lngFileId, lngSubFileId)
                            ''Send Mail Dividend
                            _Helper.Uploader_SendMail(lngOrgId, lngUserId, intGroupId, _Helper.CPSDividen_Name, strGivenName, dtValueDate)
                            ''Send Mail Member
                            _Helper.Uploader_SendMail(lngOrgId, lngUserId, intGroupId, strFileType, strSubGivenName, dtValueDate)


                        End If
                    Else
                        'ErrorMsg = gc_Status_Error & InsertSubStatus
                        DeleteTransaction(lngFileId, enmTableName.tpgt_FileDetails.ToString)
                        DeleteTransaction(lngSubFileId, enmTableName.tpgt_FileDetails.ToString)

                        ''Delete the Dividend File since 2nd File got error
                        DeleteTransaction(lngFileId, enmTableName.tpgt_Workflow.ToString)
                        DeleteTransaction(lngFileId, enmTableName.tcor_CPSDividendUploaded.ToString)
                        Exit Try
                    End If


                End If
            End If
        Catch ex As Exception
            Return ErrorMsg
        End Try
        Return ErrorMsg
    End Function


#End Region

#Region "Delete Transaction"
    Public Sub DeleteTransaction(ByVal FileID As Long, ByVal TableName As String)
        Dim ErrorMsg As String = ""
        Dim Col_FileId As String = "FileID"
        If TableName = enmTableName.tcor_CPSDividendUploaded.ToString Or TableName = enmTableName.tcor_CPSMemberUploaded.ToString _
             Or TableName = enmTableName.tcor_PaylinkUploaded_Trailer.ToString Or TableName = enmTableName.tcor_PaylinkUploaded.ToString Then
            Col_FileId = "[File Id]"
        End If
        ''Convert to SP
        PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, "DELETE FROM " & TableName & " where " & _
            Col_FileId & " =" & FileID)
    End Sub
#End Region

#Region "Insert Dividend Member Relation"
    Private Sub InsertMemDiv_Relation(ByVal FileID As Long, ByVal SubFileId As Long)
        Dim ErrorMsg As String = ""
        ''Convert to SP
        PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, SQL_MiniStatement & FileID & "," & _
            enmMiniStatement.Insert_MemDiv_Relation & "," & SubFileId)
    End Sub
#End Region

#Region "Get Charges"
    Public Function GetOrgCharges(ByVal FileId As Long, ByVal OrgId As Long)
        Dim Total_Charges As Decimal = 0
        Dim _DataRowCharges As DataRow = Nothing

        ''Get Fixed/Tier Charges Start
        Dim Trans_From As Integer, Trans_To As Integer
        Dim Trans_Charges As Double
        Dim dtTranCharges As New DataTable
        Dim icharge As Integer = 0
        Dim ChargeType As Integer
        Dim No_of_files As Integer
        Dim Errmsg As String = ""
        Dim DiffVal As Integer = 0

        Try



            No_of_files = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, Errmsg, _
                SQL_MiniStatement & FileId & "," & enmMiniStatement.GetTotalTrans_tpgtFiledetails)

            dtTranCharges = PPS.GetData(_Helper.GetChargesSQL & " " & OrgId, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

            ChargeType = dtTranCharges.Rows(0)("CHARGE_TYPE")
            If ChargeType <> 1 Then
                ''Tier Charges Start

                If dtTranCharges.Rows.Count > 0 Then
                    For Each _DataRowCharges In dtTranCharges.Rows
                        Trans_From = dtTranCharges.Rows(icharge)("TRANS_FROM")
                        Trans_To = dtTranCharges.Rows(icharge)("TRANS_TO")
                        Trans_Charges = dtTranCharges.Rows(icharge)("TRANS_CHARGE")

                        icharge += 1
                        DiffVal = (No_of_files - ((Trans_To - Trans_From) + 1))

                        If Trans_To = 0 Then
                            Total_Charges += Trans_Charges * No_of_files
                            Exit For
                        End If
                        If DiffVal < 0 Then
                            Total_Charges += Trans_Charges * No_of_files
                            Exit For
                        Else
                            Total_Charges += Trans_Charges * ((Trans_To - Trans_From) + 1)
                            No_of_files = DiffVal
                        End If

                    Next
                End If
            Else
                'Fixed Charges Start
                Trans_Charges = dtTranCharges.Rows(0)("FIXED_CHARGES")
                Total_Charges = No_of_files * Trans_Charges

            End If

        Catch ex As Exception

        End Try
        Return Total_Charges
    End Function
#End Region

#Region "Update Tpgt file details table "

    Private Sub UpdTpgtDetails(ByVal FileContents As DataTable, ByVal FileId As Integer, _
        ByVal dtValueDate As Date, ByVal strFileType As String, Optional ByVal HashTotal As String = "")

        ''Instances Declaration - Start
        Dim ToatalAmtResult As Object
        ''Instances Declaration - Stop

        ''Variable Declaration - Start
        Dim SuffixTag As String = "_Amt"
        Dim AmtColumnName As String = ""
        Dim TotalAmt As Decimal = 0, SQLStatement As String = Nothing, CatchMsg As String = Nothing
        ''Variable Declaration - Stop

        Try

            If strFileType = _Helper.CPSDelimited_Dividen_Name Then
                AmtColumnName = _Helper.Delimited_DivTotalNetCol
            ElseIf strFileType = _Helper.CPSDividen_Name Then
                AmtColumnName = _Helper.DivTotalNetCol
            ElseIf strFileType = _Helper.CPSSingleFileFormat_Name Then
                AmtColumnName = _Helper.SFF_DivTotalNetCol
            ElseIf strFileType = _Helper.PayLinkPayRoll_Name Then
                AmtColumnName = _Helper.Infenion_PaymentCol()
            End If

            ''Updating TpgtFileDetails - Start
            ''Convert the Total Amt field and add it to this table as new column-Start
            If Not strFileType = _Helper.CPSMember_Name Then
                FileContents.Columns.Add(AmtColumnName & SuffixTag, GetType(Decimal), _
                    "CONVERT([" & AmtColumnName & "],'System.Decimal')")

                ''Convert the Total Amt field and add it to this table as new column-Stop

                ''Get the Total Amt from The filecontent table-Start
                ToatalAmtResult = FileContents.Compute(("SUM([" & AmtColumnName & SuffixTag & "])"), "")
                If Not ToatalAmtResult.Equals(DBNull.Value) Then
                    If strFileType = _Helper.CPSSingleFileFormat_Name Then
                        TotalAmt = clsGeneric.NullToDecimal(ToatalAmtResult) / 100
                    Else
                        TotalAmt = clsGeneric.NullToDecimal(ToatalAmtResult)
                    End If

                End If
            End If

            ''Get the Total Amt from The filecontent table-Stop

            SQLStatement = _Helper.SQLFileDetails & PPS.PageAction.Update & "," & FileId & "," _
                    & FileContents.Rows.Count & "," & TotalAmt & ",'" & HashTotal & "','" & dtValueDate & "'"
            PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, SQLStatement)
            FileContents = Nothing

            ''Updating TpgtFileDetails - Stop
        Catch ex As Exception

        End Try
    End Sub

#End Region

#Region "GetBatchNo"
    Public Function GetBatchNo(ByVal OrgId As Integer) As Integer

        Dim No As Long
        Dim Batch_No As Integer
        Dim ErrorMsg As String = ""
        No = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
            SQL_MiniStatement & OrgId & "," & enmMiniStatement.GetCountChequeNo)
        Batch_No = (No / 999999)
        If Batch_No < 1 Then
            Batch_No = 1
        Else
            Batch_No = clsGeneric.NullToInteger(Batch_No) + 1
        End If

        Return Batch_No
    End Function



#End Region

#Region "CheckFileRelation"
    Public Function CheckFileRelation(ByVal FileID As Integer) As Boolean

        Dim dtfilerelation As New DataTable

        dtfilerelation = PPS.GetData(SQL_MiniStatement & FileID & "," & enmMiniStatement.GetMemDiv_Relation, _Helper.GetSQLConnection, _
                                   _Helper.GetSQLTransaction)
        If dtfilerelation.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

#Region "Update -tpgt_FileDetails Status"

    Public Sub UpdateStatus_tpgtFileDetails(ByVal Status As Integer, ByVal FileID As Integer, _
        ByVal Type As String, Optional ByVal Text As String = "")

        Dim ErrorMsg As String = ""
        Try

            ''Convert to SP
            If Type = TypeWEB_Name() Then
                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, SQL_MiniStatement & FileID & "," & _
                               enmMiniStatement.Update_tpgtFiledetails_Status & "," & Status & ",'" & Text & "'")
            Else
                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, SQL_MiniStatement & FileID & "," & _
                                               enmMiniStatement.Update_MFiledetails_Status & "," & Status & ",'" & Text & "'")
            End If




        Catch ex As Exception

        End Try
    End Sub
#End Region

#Region "Count No. Of Records - Remain"

    Public Function Count_NoOfRec(ByVal fileID As Integer, ByVal Type As String)
        Dim NoOfRecords As Integer
        Dim CatchMsg As String = ""
        Try


            ''Execute CIMBGW_CPSCountNumberRecords Start
            NoOfRecords = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                CatchMsg, SQL_CountNumberRecords & fileID & ",'C','" & Type & "'")
            Return NoOfRecords
        Catch ex As Exception

        End Try

    End Function


#End Region

#Region "Check Cheque Duplicate"
    Public Function CheckDuplicate(ByVal chequeNo As Long, ByVal OrgId As Integer, ByVal BatchNo As Integer) As Boolean
        Dim Duplicate As Long
        Dim ErrorMsg As String

        Duplicate = MaxGeneric.clsGeneric.NullToLong(PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                        SQL_MiniStatement & OrgId & "," & clsCPSPhase3.enmMiniStatement.GetDuplicateChequeNo & "," & chequeNo))
        If Duplicate <> 0 Then
            Return False
        Else
            Return True
        End If
    End Function
#End Region

#Region "Get StartName / Extension"
    Private Function GetExtension(ByVal strfiletype As String, Optional ByVal Start As Boolean = False, _
        Optional ByVal extension As Boolean = False) As String

        Dim Value As String = ""

        If strfiletype = _Helper.CPSDividen_Name Then
            If Start = True Then
                Value = _Helper.CPSDividendStartName()
            ElseIf extension = True Then
                Value = _Helper.CPSDividendExt()

            End If

        ElseIf strfiletype = _Helper.CPSMember_Name Then
            If Start = True Then
                Value = _Helper.CPSMemberStartName()
            ElseIf extension = True Then

                Value = _Helper.CPSMemberExt()
            End If

        ElseIf strfiletype = _Helper.CPSDelimited_Dividen_Name Then
            If Start = True Then
                Value = _Helper.CPSDelim_DividendStartName()
            ElseIf extension = True Then
                Value = _Helper.CPSDelim_DividendExt()
            End If

        ElseIf strfiletype = _Helper.CPSSingleFileFormat_Name Then
            If Start = True Then
                Value = _Helper.CPSSFFStartName()
            ElseIf extension = True Then
                Value = _Helper.CPSSFFExt()
            End If
        End If
        Return Value

    End Function


#End Region

#Region "Get Cheque Column Name"
    Public Function GetChequeCol_Name(ByVal FileID As Integer, Optional ByVal OrgId As Integer = 0) As String
        Dim dtFileTypeDetails As New DataTable
        Dim dtFileSettings As New DataTable
        Dim FileTypeId As Integer = 0
        Dim drFileSettings As DataRow = Nothing
        Dim ColName As String = ""

        Try
            dtFileTypeDetails = PPS.GetData(_ReadWriteHelper.SQLGetRelatedFileType & " " & FileID, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)
            If dtFileTypeDetails.Rows.Count > 0 Then
                ''Added temp for delimited dividend
                If dtFileTypeDetails.Rows(0)(_ReadWriteHelper.FileTypeCol) <> _Helper.CPSDelimited_Dividen_Name Then



                    FileTypeId = clsGeneric.NullToInteger(dtFileTypeDetails.Rows(0)(_ReadWriteHelper.FileTypeIdCol))
                    dtFileSettings = PPS.GetData("EXEC CIMBGW_Get_ColumnDetails " & FileTypeId, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)
                    '  drFileSettings = _clsCommon.GetFieldRow(dtFileSettings, 0, Helper.enmCPSOptions.CPS_Cheque_Col_Name_CN, OrgId)
                    drFileSettings = GetFieldRow(dtFileSettings, Helper.enmCPSOptions.CPS_Cheque_Col_Name_CN)

                    ColName = drFileSettings(_ReadWriteHelper.BankFieldDescCol)

                    Return ColName
                Else

                    ColName = "[Cheque_Number]"
                End If

            End If

        Catch ex As Exception

        End Try


    End Function
#End Region

#Region "Get Input Field Row "

    'Author         : Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Purpose        : Get Input Field Row for Input Field Id
    'Created        : 18/10/2008
    Public Function GetFieldRow(ByVal BankFileSettings As DataTable, ByVal PredefinedOption As [Enum]) As DataRow

        'Create Instance of Data Row
        Dim _DataRow As DataRow = Nothing

        Try

            'Loop thro the Bank File Settings - Start
            For Each _DataRow In BankFileSettings.Rows
                If Right([Enum].GetName(GetType(Helper.enmCPSOptions), _
                         PredefinedOption), 2) = _DataRow(_ReadWriteHelper.PredefinedOptionsCol) Then
                    Return _DataRow
                End If
            Next
            'Loop thro the Input File Settings - Stop

            Return Nothing

        Catch ex As Exception


            Return Nothing

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Function


#End Region

End Class
