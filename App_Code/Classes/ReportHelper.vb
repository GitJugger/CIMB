#Region "NameSpaces "

Imports System
Imports MaxGeneric
Imports MaxPayroll
Imports System.Web
Imports System.Data
Imports MaxMiddleware
Imports System.Configuration
Imports Microsoft.VisualBasic

#End Region

Public Class ReportHelper

#Region "Global Declarations "

    Private _Helper As New Helper
    Private _SessionOrgId As String = "_ORGID"
    Private _SessionReportName As String = "SRN"
    Private _SessionSearchOption As String = "Option"
    Private _SessionSearchCriteria As String = "Criteria"
    Private _SessionMandateOption As String = "MO"
    Private _RequestStartDate As String = "StartDt"
    Private _RequestEndDate As String = "EndDt"
    Private _RequestFieldName As String = "FieldName"
    Private _SessionUserType As String = gc_Ses_UserType
    Private _TheStatus As String = "ST"
    Dim lngOrgId As Long, lngUserId As Long
    Private _CPSSesion_DDL As String = "DDLSession"

#End Region

#Region "Enumerators "
    ''CPS Report Start
    Public Enum CPSPhase3ChargeSearchOption
        Fixed = 1
        Tier = 2
    End Enum
    Public Enum CPSPhase3ChequeDetailsReport
        Payment_Value_Date = 2
        Cheque_Number = 9 ''Continue from mandate
        All_In_This_Organization = 10
    End Enum

    Public Enum CPSPhase3_MonthlyChequeStatus_Report

        Cleared_Cheque = 10
        Pending_Cheque = 11
        Stale_Expired_Cheque = 12

    End Enum
    Public Enum CPSPhase3StaleExpiredChequeReport
        Time_Duration = 11
        All_In_This_Organization = 10
    End Enum

    Public Enum CPSPhase3Unclaim_ChequeReport
        Payment_Value_Date_Range = 11
        Account_Number = 13
        Cheque_Number = 9
        All_In_This_Organization = 10
    End Enum
    Public Enum CPSPhase3Damage_ChequeReport
        Payment_Value_Date_Range = 11
        Original_Cheque_Number = 9
        Replace_Cheque_Number = 12
        All_In_This_Organization = 10
    End Enum


    Public Enum MandateSearchType
        Mandate_Auto_Regis = 1
        Mandate_Manual_Regis = 2
        Mandate_Billing_Upload = 3
        Mandate_Billing_Reject = 4
        Mandate_Summary = 5
        Mandate_Movement_Auto = 6
        Mandate_Movement_Manual = 7
        CPS_Cheque_Details = 8
        CPS_Monthly_Cheque = 9
        CPS_Stale_Cheque = 10
        CPS_Unclaimed_Cheque = 11
        CPS_Damage_Cheque = 12
        CPS_Charges_Report = 13
        CPS_Complience_Report = 14
    End Enum
    Public Enum MandateSearchOption
        File_Id = 6

    End Enum
    Public Enum MandateSearchOptionAuto
        File_Name = 1
        Upload_Date = 2
    End Enum
    Public Enum MandateSearchOptionManual
        Customer_Name = 3
        Reference_No = 4
        Bank_Org_Code = 5
        All_In_This_Organization = 8
    End Enum
    Public Enum MandateSearchOptionBillingSummary
        Daily_Report = 1
        Monthly_Report = 4 'Contiues from mandate billing
    End Enum
    Public Enum MandateSearchOptionCustomerManual
        Customer_Name = 3
        Reference_No = 4
        All_In_This_Organization = 8
    End Enum

    Public Enum MandateSearchOptionCustomer
        File_Name = 1
        Upload_Date = 2
    End Enum
    Public Enum MandateBilling
        Submission_Date = 1
        File_Name = 2
        Batch_Number = 3
    End Enum
    Public Enum MandateMovement
        Modify_Date = 1
        Field_Name = 2
    End Enum

    Public Enum ReportNames
        Mandate_Auto_Manual_Regis = 1
        Direct_Debit_Billing = 2
        Direct_Debit_Summary = 3
        Daily_Mandate_Movement = 4
        CPSDynamicDetails = 5
        CPS_MonthlyChequeStatus = 6
        'MandateMovementManual = 5
    End Enum
    Public Enum CPS_FeeOption
        Tier = 1
        Fixed = 2
    End Enum
    Public Enum CPS_FileType
        Type_1 = 1
        Type_2 = 2
        Single_File_Format = 3
    End Enum

#End Region

#Region "Properties "

    Public ReadOnly Property ReportServerURL() As String
        Get
            Return ConfigurationManager.AppSettings("ReportServerURL")
        End Get
    End Property

    Public ReadOnly Property ReportServerPath() As String
        Get
            Return ConfigurationManager.AppSettings("ReportDir")
        End Get
    End Property

    Public ReadOnly Property MandateAutoManualSearchSQL() As String
        Get
            Return "EXEC CIMBGW_Mandate_Auto_Manual_Search "
        End Get
    End Property

    Public ReadOnly Property MandateBillingSearchSQL() As String
        Get
            Return "EXEC CIMBGW_Mandate_Billing_Search "
        End Get
    End Property

    Public ReadOnly Property MandateMovementSQL() As String
        Get
            Return "EXEC CIMBGW_Mandate_Movement_Search "
        End Get
    End Property
    ''CPS Report Start
    'Public Property Sesion_CPSDDLOption() As String
    '    Get

    '    End Get
    '    Set(ByVal value As String)

    '    End Set
    'End Property

    ''CPS Report End

    Public Property TheStatus() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Session(_TheStatus))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_TheStatus) = value
        End Set
    End Property
    Public ReadOnly Property FileId() As Integer
        Get
            Return clsGeneric.NullToInteger( _
                HttpContext.Current.Request.QueryString("FID"))
        End Get
    End Property
    Public ReadOnly Property FieldName() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Request.QueryString("FieldName"))
        End Get
    End Property
    Public ReadOnly Property Criteria() As String
        Get
            Return clsGeneric.NullToString( _
            HttpContext.Current.Request.QueryString("Criteria"))
        End Get
    End Property

    Public Property SessionReportName() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Session(_SessionReportName))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_SessionReportName) = value
        End Set
    End Property
    Public Property SessionSearchOption() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Session(_SessionSearchOption))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_SessionSearchOption) = value
        End Set
    End Property
    Public Property SessionSearchCriteria() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Session(_SessionSearchCriteria))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_SessionSearchCriteria) = value
        End Set
    End Property


    Public Property SessionOrgId() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Session(_SessionOrgId))
        End Get
        Set(ByVal value As String)
            HttpContext.Current.Session(_SessionOrgId) = value
        End Set
    End Property

    Public ReadOnly Property SessionUserType() As String
        Get
            Return clsGeneric.NullToString( _
                HttpContext.Current.Session(gc_Ses_UserType))
        End Get
    End Property
    Public ReadOnly Property GetReportType() As Short
        Get
            Return [Enum].Parse(GetType(ReportHelper.MandateSearchType), SessionReportName)
        End Get
    End Property
    Public Property GetMandateOption() As Short
        Get
            Return clsGeneric.NullToString( _
                           HttpContext.Current.Session(_SessionMandateOption))
        End Get
        Set(ByVal value As Short)
            HttpContext.Current.Session(_SessionMandateOption) = value
        End Set
    End Property
    Public ReadOnly Property GetStartDate() As String
        Get
            Return clsGeneric.NullToString( _
                           HttpContext.Current.Request.QueryString(_RequestStartDate))
        End Get

    End Property
    Public ReadOnly Property GetEndDate() As String
        Get
            Return clsGeneric.NullToString( _
                           HttpContext.Current.Request.QueryString(_RequestEndDate))
        End Get

    End Property
    Private ReadOnly Property GetFieldName() As String
        Get
            Return clsGeneric.NullToString( _
                           HttpContext.Current.Request.QueryString(_RequestFieldName))
        End Get

    End Property


#End Region

#Region "Get Mandate Registration Details "

    'Purpose		: Mandate Resgistration Details
    'Author			: Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date	: 29/10/2008
    Public Function GetMandateRegDetails(ByVal SearchType As Short, _
        ByVal SearchOption As Short, ByVal ParamArray SQLParams As String()) As DataTable

        'Create Instance of Data Table
        Dim MandateRegDetails As DataTable = Nothing

        'Variable Declarations
        Dim SQLStatement As String = Nothing, Index As Short = 0

        Try

            'Build SQL Statement - Start
            SQLStatement = MandateAutoManualSearchSQL
            SQLStatement &= SearchType & "," & SearchOption & "," & SessionOrgId

            'loop thro the param array - Start
            For Index = 0 To SQLParams.GetUpperBound(0)
                SQLStatement &= ",'" & clsGeneric.NullToString(SQLParams(Index)) & "'"
            Next
            'SQLStatement &= ddlStatus.SelectedValue & "'"
            'loop thro the param array - Stop
            'Build SQL Statement - Stop

            'Get Mandate Registration Details
            Return PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Function

#End Region

#Region "Load Mandate Registration Report "

    'Purpose		: Load Mandate Resgistration Reports
    'Author			: Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date	: 29/10/2008
    Public Sub LoadMandateRegReport(ByVal FileId As Integer, _
        ByVal MandateType As Short, ByRef ParamName As String(), _
            ByRef ParamValue As String())

        'Create Instance of Data Table/Row
        Dim MandateDetails As DataTable = Nothing, Index As Short = 0

        'Variable Declarations
        Dim SQLStatement As String = Nothing, OrgId As Integer = 0
        Dim UploadDate As String = Nothing, RegisType As Short = 0
        Dim OrgName As String = Nothing, OrgCode As String = Nothing
        Dim FileName As String = Nothing, BatchId As String = Nothing
        Dim SearchCriteria As String = Nothing, Status As String = Nothing

        Try

            'Build SQL Param Names - Start
            ParamName = PPS.GetStringArray("in_File_Id", "in_Org_Id", "in_Org_Name", _
                "in_Org_Code", "in_File_Name", "in_Batch_ID", "in_Upload_Date", _
                    "in_File_Status", "in_SearchOption", "in_SearchCriteria", "in_Status")
            'Build SQL Param Names - Stop

            'Build SQL Statement - Start
            If GetReportType = MandateSearchType.Mandate_Auto_Regis Then

                SQLStatement = MandateAutoManualSearchSQL & MandateType & ","
                SQLStatement &= MandateSearchOption.File_Id & "," & SessionOrgId & "," & FileId

            ElseIf GetReportType = MandateSearchType.Mandate_Manual_Regis Then

                SQLStatement = MandateAutoManualSearchSQL & MandateType & ","
                SQLStatement &= MandateSearchOption.File_Id & "," & SessionOrgId & "," & TheStatus

            End If
            'Build SQL Statement - Stop

            'Get Mandate Details
            MandateDetails = PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)

            'Get Values - Start
            If MandateDetails.Rows.Count > 0 Then
                RegisType = GetReportType
                FileId = clsGeneric.NullToInteger(MandateDetails.Rows(0)("File_Id"))
                OrgId = clsGeneric.NullToInteger(MandateDetails.Rows(0)("Org_Id"))
                OrgName = clsGeneric.NullToString(MandateDetails.Rows(0)("Org_Name"))
                OrgCode = clsGeneric.NullToString(MandateDetails.Rows(0)("Org_Code"))
                BatchId = clsGeneric.NullToString(MandateDetails.Rows(0)("Batch_Id"))
                FileName = clsGeneric.NullToString(MandateDetails.Rows(0)("File_Name"))
                UploadDate = clsGeneric.NullToString(MandateDetails.Rows(0)("Upload_Date"))
                Status = clsGeneric.NullToString(MandateDetails.Rows(0)("Status"))
            End If
            'Get Values - Stop

            ''Build SQL Param Values - Start
            If SessionSearchCriteria = "" Then
                SearchCriteria = Criteria
            Else
                SearchCriteria = SessionSearchCriteria
            End If
            ParamValue = PPS.GetStringArray(FileId, SessionOrgId, OrgName, OrgCode, _
                FileName, BatchId, UploadDate, RegisType, SessionSearchOption, _
                    SearchCriteria, Status)
            'Build SQL Param Values - Stop

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Load Mandate Movement Report "

    'Purpose		: Load Mandate Resgistration Reports
    'Author			: Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date	: 29/10/2008
    Public Sub LoadMandateMovementReport(ByVal FieldName As String, _
        ByVal MandateType As Short, ByRef ParamName As String(), _
            ByRef ParamValue As String())

        'Create Instance of Data Table/Row
        Dim MandateMovementDetails As DataTable = Nothing, Index As Short = 0

        'Variable Declarations
        Dim SQLStatement As String = Nothing, OrgId As Integer = 0
        Dim OrgName As String = Nothing, OrgCode As String = Nothing

        Try

            'Build SQL Param Names - Start
            ParamName = PPS.GetStringArray("in_Org_Name", "in_Org_Code", "in_Org_Id", "in_OptionType", _
                     "in_Start_Date", "in_End_Date", "in_Field_Name")
            'Build SQL Param Names - Stop

            'Build SQL Statement - Start
            If GetReportType = MandateSearchType.Mandate_Movement_Auto _
                Or GetReportType = MandateSearchType.Mandate_Movement_Manual Then

                SQLStatement = MandateMovementSQL & MandateType & ","
                If Not FieldName = "" Then
                    SQLStatement &= MandateSearchOption.File_Id & "," & SessionOrgId & "," & FieldName
                Else
                    SQLStatement &= SessionSearchOption & "," & SessionOrgId & ",'" & GetStartDate & "','" & GetEndDate & "'"
                End If

            End If
            'Build SQL Statement - Stop

            'Get Mandate Details
            MandateMovementDetails = PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)

            'Get Values - Start
            If MandateMovementDetails.Rows.Count > 0 Then
                OrgName = clsGeneric.NullToString(MandateMovementDetails.Rows(0)("Org_Name"))
                OrgCode = clsGeneric.NullToString(MandateMovementDetails.Rows(0)("Org_Code"))
                OrgId = clsGeneric.NullToInteger(MandateMovementDetails.Rows(0)("Org_Id"))
            Else
                OrgName = ""
                OrgCode = 0
                OrgId = 0

            End If
            'Get Values - Stop

            'Build SQL Param Values - Start
            ParamValue = PPS.GetStringArray(OrgName, OrgCode, OrgId, MandateType, GetStartDate, _
                            GetEndDate, GetFieldName)
            'Build SQL Param Values - Stop

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Get Mandate Billing Details "

    'Purpose		: Mandate Resgistration Details
    'Author			: Bhanu Teja/Hafeez - T-Melmax Sdn Bhd
    'Created Date	: 31/10/2008
    Public Function GetMandateBillingDetails(ByVal SearchType As Short, _
        ByVal SearchOption As Short, ByVal ParamArray SQLParams As String()) As DataTable

        'Create Instance of Data Table
        Dim MandateBillingDetails As DataTable = Nothing

        'Variable Declarations
        Dim SQLStatement As String = Nothing, Index As Short = 0

        Try

            'Build SQL Statement - Start
            SQLStatement = MandateBillingSearchSQL
            SQLStatement &= SearchType & "," & SearchOption & "," & SessionOrgId

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
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "GetMandateBillingDetails - ClsReportHelper", Err.Number, Err.Description)



        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Function


#End Region

#Region "Load Mandate Registration Report "

    'Purpose		: Load Mandate Billing Reports
    'Author			: Sujith Sharatchandran - T-Melmax Sdn Bhd
    'Created Date	: 29/10/2008
    Public Sub LoadMandateBillingReport(ByVal FileId As Integer, _
        ByVal MandateType As Short, ByRef ParamName As String(), _
            ByRef ParamValue As String())

        'Create Instance of Data Table/Row
        Dim MandateDetails As DataTable = Nothing, Index As Short = 0

        'Variable Declarations
        Dim SQLStatement As String = Nothing, OrgId As Integer = 0
        Dim UploadDate As String = Nothing, ReportType As String = Nothing
        Dim OrgName As String = Nothing, OrgCode As String = Nothing
        Dim BillingDate As String = Nothing, BatchId As String = Nothing

        Try

            'Build SQL Param Names - Start
            ParamName = PPS.GetStringArray("in_File_Id", "in_Org_Name", _
                "in_Org_Code", "in_Batch_ID", "in_Upload_Date", _
                    "in_Billing_Date", "in_Report_Type")
            'Build SQL Param Names - Stop

            'Build SQL Statement - Start
            If GetReportType = MandateSearchType.Mandate_Billing_Upload _
                Or GetReportType = MandateSearchType.Mandate_Billing_Reject Then

                SQLStatement = MandateBillingSearchSQL & MandateType & ","
                SQLStatement &= MandateSearchOption.File_Id & "," & SessionOrgId & "," & FileId

            End If
            'Build SQL Statement - Stop

            'Get Mandate Details
            MandateDetails = PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                _Helper.GetSQLTransaction)

            'Get Values - Start
            If MandateDetails.Rows.Count > 0 Then
                ReportType = GetReportType
                FileId = clsGeneric.NullToInteger(MandateDetails.Rows(0)("File_Id"))
                OrgName = clsGeneric.NullToString(MandateDetails.Rows(0)("Org_Name"))
                OrgCode = clsGeneric.NullToString(MandateDetails.Rows(0)("Org_Code"))
                BatchId = clsGeneric.NullToString(MandateDetails.Rows(0)("Batch_Id"))
                BillingDate = clsGeneric.NullToString(MandateDetails.Rows(0)("Billing_Date"))
                UploadDate = clsGeneric.NullToString(MandateDetails.Rows(0)("Upload_Date"))
            End If
            'Get Values - Stop

            If ReportType = MandateSearchType.Mandate_Billing_Upload Then
                ReportType = "U"
            ElseIf ReportType = MandateSearchType.Mandate_Billing_Reject Then
                ReportType = "R"
            End If

            'Build SQL Param Values - Start
            ParamValue = PPS.GetStringArray(FileId, OrgName, OrgCode, _
                BatchId, UploadDate, BillingDate, ReportType)
            'Build SQL Param Values - Stop

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Get Mandate Movement Details "

    'Purpose		: Mandate Resgistration Details
    'Author			: Bhanu Teja/Hafeez - T-Melmax Sdn Bhd
    'Created Date	: 31/10/2008
    Public Function GetMandateMovement(ByVal SearchType As Short, _
        ByVal SearchOption As Short, ByVal ParamArray SQLParams As String()) As DataTable

        'Create Instance of Data Table
        Dim MandateMovement As DataTable = Nothing

        'Variable Declarations
        Dim SQLStatement As String = Nothing, Index As Short = 0

        Try

            'Build SQL Statement - Start
            SQLStatement = MandateMovementSQL
            SQLStatement &= SearchType & "," & SearchOption & "," & SessionOrgId

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
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "GetMandateMovementDetails - ClsReportHelper", Err.Number, Err.Description)


        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Function


#End Region


End Class
