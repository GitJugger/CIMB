Imports MaxGeneric
Imports MaxMiddleware
Imports MaxPayroll


Partial Class WebForms_pg_ShowReport
    Inherits clsBasePage
    Private _clsgeneric As clsGeneric

#Region "Global Declarations "

    Private _ReportHelper As New ReportHelper
    Dim lngOrgId As Long, lngUserId As Long
#End Region

#Region "Properties "

    Private ReadOnly Property GetReportName() As String
        Get
            If _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Manual_Regis.ToString() Or _
                _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Auto_Regis.ToString() Then
                Return ReportHelper.ReportNames.Mandate_Auto_Manual_Regis.ToString()
            ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Billing_Upload.ToString() Or _
                _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Billing_Reject.ToString() Then
                Return ReportHelper.ReportNames.Direct_Debit_Billing.ToString()
            ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Summary.ToString() Then
                Return ReportHelper.ReportNames.Direct_Debit_Summary.ToString()
            ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Movement_Auto.ToString() _
            Or _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.Mandate_Movement_Manual.ToString() Then
                Return ReportHelper.ReportNames.Daily_Mandate_Movement.ToString()
                ''CPS Start
            ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Cheque_Details.ToString() _
            Or _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Stale_Cheque.ToString() _
            Or _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque.ToString() _
            Or _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Damage_Cheque.ToString() Then
                Return ReportHelper.ReportNames.CPSDynamicDetails.ToString()
            ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Monthly_Cheque.ToString() Then
                Return ReportHelper.ReportNames.CPS_MonthlyChequeStatus.ToString()
            End If
            Return String.Empty
        End Get
    End Property

    Public ReadOnly Property StartDate() As String
        Get
            Return clsGeneric.NullToString( _
            Request.QueryString("StartDt"))
        End Get

    End Property
    Public ReadOnly Property ReplaceCheque() As String
        Get
            Return clsGeneric.NullToString( _
            Request.QueryString("ReplaceCheque"))
        End Get

    End Property
    Public ReadOnly Property EndDate() As String
        Get
            Return clsGeneric.NullToString( _
            Request.QueryString("EndDt"))
        End Get

    End Property
    Public ReadOnly Property ChequeNo() As String
        Get


            If clsGeneric.NullToString(Request.QueryString("ChequeNo")) = Nothing Then
                ChequeNo = 0
            Else
                ChequeNo = clsGeneric.NullToString(Request.QueryString("ChequeNo"))
            End If
            Return ChequeNo
        End Get

    End Property
    Public ReadOnly Property SearchMonth() As Short
        Get
            Return clsGeneric.NullToShort( _
            Request.QueryString("Month"))
        End Get

    End Property
    Public ReadOnly Property SearchYear() As Short
        Get
            Return clsGeneric.NullToShort( _
            Request.QueryString("Year"))
        End Get

    End Property

    Public ReadOnly Property CPS_DirectGetReportName() As String
        Get
            Return Request.QueryString("ReportName")
        End Get

    End Property
#End Region

#Region "Page Load "

    'Author     : Bhanu Teja/Hafeez
    'Purpose    : Page load Actions
    'Created    : 29/10/2008
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Variable Declarations
        Dim Index As Short = 0
        Dim ParamName As String() = Nothing, ParamValue As String() = Nothing

        Try
            If Not Page.IsPostBack Then

                If GetReportName = ReportHelper.ReportNames.Mandate_Auto_Manual_Regis.ToString() Then

                    'Get Mandate Report Parameters - Start
                    Call _ReportHelper.LoadMandateRegReport(_ReportHelper.FileId, clsGeneric.NullToShort( _
                        _ReportHelper.GetReportType), ParamName, ParamValue)
                    'Get Mandate Report Parameters - Stop

                ElseIf GetReportName = ReportHelper.ReportNames.Direct_Debit_Billing.ToString() Then

                    'Get Mandate Billing Report Parameters - Start
                    Call _ReportHelper.LoadMandateBillingReport(_ReportHelper.FileId, clsGeneric.NullToShort( _
                        _ReportHelper.GetReportType), ParamName, ParamValue)
                    'Get Mandate Billing Report Parameters - Stop

                ElseIf GetReportName = ReportHelper.ReportNames.Daily_Mandate_Movement.ToString() Then

                    Call _ReportHelper.LoadMandateMovementReport(_ReportHelper.FieldName, clsGeneric.NullToShort( _
                        _ReportHelper.GetReportType), ParamName, ParamValue)

                    'Get Mandate Movement Report Parameters - Stop

                ElseIf GetReportName = ReportHelper.ReportNames.Direct_Debit_Summary.ToString() Then

                    ParamValue = PPS.GetStringArray(StartDate, EndDate, SearchMonth, SearchYear, _ReportHelper.SessionOrgId)
                    ParamName = PPS.GetStringArray("in_StartDate", "in_EndDate", "in_Month", "in_Year", "in_OrgId")

                ElseIf GetReportName = ReportHelper.ReportNames.CPSDynamicDetails.ToString() Then

                    ''CPS_Cheque_Details = 8
                    ''CPS_Monthly_Cheque = 9
                    ''CPS_Stale_Cheque = 10
                    ''CPS_Unclaimed_Cheque = 11
                    ''CPS_Damage_Cheque = 12
                    ''CPS_Charges_Report = 13
                    ''CPS_Complience_Report = 14
               

                    ''CPS CPS_Cheque_Details Start
                    If _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Cheque_Details.ToString() Then

                        If _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3ChequeDetailsReport.Cheque_Number Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.Criteria(), ReportHelper.MandateSearchType.CPS_Cheque_Details)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3ChequeDetailsReport.All_In_This_Organization Then

                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, ReportHelper.MandateSearchType.CPS_Cheque_Details)
                        Else
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, 0, ReportHelper.MandateSearchType.CPS_Cheque_Details)
                        End If


                        ''CPS CPS_Cheque_Details Stop

                        ''CPS CPS_Stale_Cheque Start
                    ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Stale_Cheque.ToString() Then
                        If _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3StaleExpiredChequeReport.Time_Duration Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, _
                                                                ReportHelper.MandateSearchType.CPS_Stale_Cheque)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3StaleExpiredChequeReport.All_In_This_Organization Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, _
                                    ReportHelper.MandateSearchType.CPS_Stale_Cheque)

                        End If
                        ''CPS CPS_Unclaimed_Cheque Start
                    ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque.ToString() Then

                        If _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Unclaim_ChequeReport.Account_Number Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.Criteria(), _
                            ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Unclaim_ChequeReport.Cheque_Number Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.Criteria(), _
                                 ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Unclaim_ChequeReport.All_In_This_Organization Then

                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, _
                                ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Unclaim_ChequeReport.Payment_Value_Date_Range Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, _
                                ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque)
                        End If

                    ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Damage_Cheque.ToString() Then
                        If _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Damage_ChequeReport.Payment_Value_Date_Range Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, _
                                                            ReportHelper.MandateSearchType.CPS_Damage_Cheque)

                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Damage_ChequeReport.All_In_This_Organization Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.SessionOrgId, _
                                ReportHelper.MandateSearchType.CPS_Damage_Cheque)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Damage_ChequeReport.Replace_Cheque_Number Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, ReplaceCheque, _
                                                             ReportHelper.MandateSearchType.CPS_Damage_Cheque)
                        ElseIf _ReportHelper.SessionSearchOption = ReportHelper.CPSPhase3Damage_ChequeReport.Original_Cheque_Number Then
                            ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.FileId, _ReportHelper.Criteria(), _
                                                                                         ReportHelper.MandateSearchType.CPS_Damage_Cheque)
                        End If

                    End If

                    ParamName = PPS.GetStringArray("in_StartDate", "in_EndDate", "in_FileID", "in_Criteria", "in_Option")
                    ElseIf CPS_DirectGetReportName = ReportHelper.MandateSearchType.CPS_Charges_Report.ToString() Then
                        rvReport.ServerReport.ReportServerUrl = New System.Uri(_ReportHelper.ReportServerURL)
                        rvReport.ServerReport.ReportPath = _ReportHelper.ReportServerPath & CPS_DirectGetReportName
                        _ReportHelper.SessionReportName = Nothing
                        Exit Try

                    ElseIf _ReportHelper.SessionReportName = ReportHelper.MandateSearchType.CPS_Monthly_Cheque.ToString() Then
                        ParamValue = PPS.GetStringArray(StartDate, EndDate, _ReportHelper.SessionSearchOption, _ReportHelper.SessionOrgId)
                        ParamName = PPS.GetStringArray("in_StartDate", "in_EndDate", "in_Option", "in_OrgId")


                    End If

                    'Build Report URL/Path - Start
                    rvReport.ServerReport.ReportServerUrl = New System.Uri(_ReportHelper.ReportServerURL)
                    rvReport.ServerReport.ReportPath = _ReportHelper.ReportServerPath & GetReportName
                    'Build Report URL/Path - Stop

                    'Create Instance of Report Parameter
                    Dim _ReportParameter(ParamName.GetUpperBound(0)) As  _
                         Microsoft.Reporting.WebForms.ReportParameter

                    'loop thro the Report Parameters - Start
                    For Index = 0 To ParamName.GetUpperBound(0)

                        _ReportParameter(Index) = New Microsoft.Reporting.WebForms. _
                            ReportParameter(ParamName(Index), ParamValue(Index))

                    Next
                    'loop thro the Report Parameters - Stop

                    'Refresh Report
                    rvReport.ServerReport.SetParameters(_ReportParameter)
                    rvReport.ServerReport.Refresh()
                    _ReportHelper.SessionReportName = Nothing

            End If

            ''Edited on 7/12/12015
            'If Page.IsPostBack Then
            '    If GetReportName = ReportHelper.ReportNames.Mandate_Auto_Manual_Regis.ToString() Then

            '        'Get Mandate Report Parameters - Start
            '        Call _ReportHelper.LoadMandateRegReport(_ReportHelper.FileId, clsGeneric.NullToShort( _
            '            _ReportHelper.GetReportType), ParamName, ParamValue)
            '        'Get Mandate Report Parameters - Stop
            '    End If
            '    'Build Report URL/Path - Start
            '    rvReport.ServerReport.ReportServerUrl = New System.Uri(_ReportHelper.ReportServerURL)
            '    rvReport.ServerReport.ReportPath = _ReportHelper.ReportServerPath & GetReportName
            '    'Build Report URL/Path - Stop

            '    'Create Instance of Report Parameter
            '    Dim _ReportParameter(ParamName.GetUpperBound(0)) As  _
            '         Microsoft.Reporting.WebForms.ReportParameter

            '    'loop thro the Report Parameters - Start
            '    For Index = 0 To ParamName.GetUpperBound(0)

            '        _ReportParameter(Index) = New Microsoft.Reporting.WebForms. _
            '            ReportParameter(ParamName(Index), ParamValue(Index))

            '    Next
            '    'loop thro the Report Parameters - Stop

            '    'Refresh Report
            '    rvReport.ServerReport.SetParameters(_ReportParameter)
            '    rvReport.ServerReport.Refresh()
            '    _ReportHelper.SessionReportName = Nothing
            'End If

        Catch ex As Exception

            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_ShowReport", Err.Number, Err.Description)



        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

End Class
