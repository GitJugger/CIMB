Imports MaxGeneric
Imports MaxMiddleware
Imports MaxPayroll

Partial Class WebForms_pg_ReportSearch
    Inherits clsBasePage

#Region "Global Declarations "

    Private _ReportHelper As New ReportHelper
    Private _clsCPSPhase3 As New clsCPSPhase3
    Dim lngOrgId As Long, lngUserId As Long, CriteriaOf As String = Nothing


#End Region

#Region "Properties "

    Private ReadOnly Property IsShowReport() As Boolean
        Get
            If MaxMiddleware.Helper.IsBlank(clsGeneric.NullToString(Request.QueryString("ShowRep"))) Then
                Return False
            Else
                Return True
            End If
        End Get
    End Property

    Private ReadOnly Property GetReportType() As Short
        Get
            Return _ReportHelper.GetReportType
        End Get
    End Property

    Private ReadOnly Property rq_strReportName() As String
        Get
            Return Request.QueryString("ReportName") & ""
        End Get
    End Property

#End Region

#Region "Page Load "

    'Author     : Hafeez
    'Purpose    : Page load Actions
    'Created    : 29/10/2008
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try

            If Not Page.IsPostBack And Not IsShowReport Then

                'if user type not bank user - Start
                If Not _ReportHelper.SessionUserType = clsCommon.fncGetPrefix(enmUserType.BU_BankUser) Then
                    _ReportHelper.SessionReportName = rq_strReportName
                    _ReportHelper.SessionOrgId = Session(gc_Ses_OrgId)
                Else
                    _ReportHelper.SessionOrgId = clsGeneric.NullToString(Request.QueryString("Id"))
                End If
                'if user type not bank user - Stop

                'if Mandate Auto/Manual Report - Start
                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Manual_Regis Then

                    'if User Type Checking - Start
                    If _ReportHelper.SessionUserType = clsCommon.fncGetPrefix(enmUserType.BU_BankUser) Then

                        Call PPS.EnumToDropDown(GetType(ReportHelper.MandateSearchOptionManual), _
                            Search_Option, True)

                    ElseIf _ReportHelper.SessionUserType = clsCommon.fncGetPrefix(enmUserType.U_Uploader) _
                        Or _ReportHelper.SessionUserType = clsCommon.fncGetPrefix(enmUserType.R_Reviewer) _
                            Or _ReportHelper.SessionUserType = clsCommon.fncGetPrefix(enmUserType.A_Authorizer) Then

                        Call PPS.EnumToDropDown(GetType(ReportHelper.MandateSearchOptionCustomerManual), _
                            Search_Option, True)

                    End If
                    'if User Type Checking - Stop
                ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Auto_Regis Then

                    Call PPS.EnumToDropDown(GetType(ReportHelper.MandateSearchOptionAuto), Search_Option, True)

                End If
                'if Mandate Auto/Manual Report - Stop

                'if Mandate Billing Report - Start
                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Billing_Upload _
                    Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Billing_Reject Then

                    Call PPS.EnumToDropDown(GetType(ReportHelper.MandateBilling), Search_Option, True)

                End If
                'if Mandate Billing Report - Stop

                'if Direct Debit Summary Report - Start
                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Summary Then

                    Call PPS.EnumToDropDown(GetType(ReportHelper.MandateSearchOptionBillingSummary), _
                        Search_Option, True)

                End If
                'if Direct Debit Summary Report - Stop

                'if Mandate Movement Report - Start
                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Movement_Auto _
                Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Movement_Manual Then

                    Call PPS.EnumToDropDown(GetType(ReportHelper.MandateMovement), _
                        Search_Option, True)
                    Call Clear()

                End If
                'if Mandate Movement Report - Stop
                ''if CPS Report - Start
                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Cheque_Details Then
                    lblReportName.Text = "CPS Cheque Details Report"
                    Call PPS.EnumToDropDown(GetType(ReportHelper.CPSPhase3ChequeDetailsReport), _
                                            Search_Option, True)
                    Call Clear()
                End If

                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Monthly_Cheque Then
                    lblReportName.Text = "CPS Monthly Cheque Report"
                    Call PPS.EnumToDropDown(GetType(ReportHelper.CPSPhase3_MonthlyChequeStatus_Report), _
                                                               Search_Option, True)
                End If

                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Stale_Cheque Then
                    lblReportName.Text = "CPS Stale Cheque Report"
                    Call PPS.EnumToDropDown(GetType(ReportHelper.CPSPhase3StaleExpiredChequeReport), _
                                                                Search_Option, True)
                End If

                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque Then
                    lblReportName.Text = "CPS Unclaimed Cheque Report"
                    Call PPS.EnumToDropDown(GetType(ReportHelper.CPSPhase3Unclaim_ChequeReport), _
                                                                Search_Option, True)
                End If

                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Damage_Cheque Then
                    lblReportName.Text = "CPS Damage Cheque Report"
                    Call PPS.EnumToDropDown(GetType(ReportHelper.CPSPhase3Damage_ChequeReport), _
                                                                Search_Option, True)
                End If
                ''if CPS Report - Stop


                'Hide or Show Rows
                Call HideShowRows()

            ElseIf Not Page.IsPostBack And IsShowReport Then
                'If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Cheque_Details Then
                '    If trParam_2.Visible = True And trParam_3.Visible = True Then

                '    End If
                'End If

                If __Param_1.Text = "" Then
                    _ReportHelper.SessionSearchCriteria = clsGeneric.NullToString( _
                        HttpContext.Current.Request.QueryString("Criteria"))
                End If

                'Go to report view page
                Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId & _
                    "&ReportName=" & _ReportHelper.SessionReportName & "&Criteria=" _
                        & _ReportHelper.SessionSearchCriteria & "&Status=" & ddlStatus.SelectedValue, True)

                End If

        Catch ex As Exception

            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_ReportSearch", Err.Number, Err.Description)


        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Page Submit "

    Protected Sub Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Try

            Call BindGrid()




        Catch ex As Exception

            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "PageSubmit - PG_ReportSearch", Err.Number, Err.Description)



        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Bind Grid"
    Private Sub BindGrid()
        'Create Instance of Data Table
        Dim GetSearchDetails As DataTable = Nothing

        'Variable Declarations
        Dim SearchOption As Short = 0

        'Get Search Option
        SearchOption = clsGeneric.NullToShort(Search_Option.SelectedValue)
        _ReportHelper.SessionSearchOption = Search_Option.SelectedValue

        If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Manual_Regis _
            Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Auto_Regis Then

                If trParam_1.Visible = True Then

                    GetSearchDetails = _ReportHelper.GetMandateRegDetails(GetReportType,
                        SearchOption, __Param_1.Text.ToString(), ddlStatus.SelectedValue)
                    _ReportHelper.TheStatus = ddlStatus.SelectedValue

                    If __Param_1.Text = "" Then
                        _ReportHelper.SessionSearchCriteria = clsGeneric.NullToString(
                        HttpContext.Current.Request.QueryString("Criteria"))
                    Else
                        _ReportHelper.SessionSearchCriteria = __Param_1.Text

                    End If


                ElseIf trParam_2.Visible = True And trParam_3.Visible = True Then



                    GetSearchDetails = _ReportHelper.GetMandateRegDetails(GetReportType,
                      SearchOption, txtStartDate.Value.ToString(), txtEndDate.Value.ToString())

                ElseIf trParam_1.Visible = False And trParam_2.Visible = False And trParam_3.Visible = False Then
                    GetSearchDetails = _ReportHelper.GetMandateRegDetails(GetReportType,
                        SearchOption, ddlStatus.SelectedValue)
                    _ReportHelper.TheStatus = ddlStatus.SelectedValue
                    If Not GetSearchDetails.Rows.Count = 0 Then
                        Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                            "&ReportName=" & _ReportHelper.SessionReportName, True)
                    End If

                End If

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Billing_Upload _
            Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Billing_Reject Then

                If trParam_1.Visible = True Then

                    GetSearchDetails = _ReportHelper.GetMandateBillingDetails(GetReportType,
                            SearchOption, __Param_1.Text.ToString())

                ElseIf trParam_2.Visible = True And trParam_3.Visible = True Then


                    GetSearchDetails = _ReportHelper.GetMandateBillingDetails(GetReportType,
                          SearchOption, txtStartDate.Value.ToString(), txtEndDate.Value.ToString())


                End If

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Summary Then

                If trParam_2.Visible = True And trParam_3.Visible = True Then

                    'Go to report view page
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                    "&ReportName=" & _ReportHelper.SessionReportName & "&StartDt=" _
                        & txtStartDate.Value & "&EndDt=" & txtEndDate.Value, True)



                ElseIf trParam_4.Visible = True And trParam_5.Visible = True Then

                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                            "&ReportName=" & _ReportHelper.SessionReportName & "&Month=" _
                             & ddlMonth.SelectedValue & "&Year=" & txtYear.Text, True)
                End If

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Movement_Auto _
        Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Movement_Manual Then

                If trParam_2.Visible = True And trParam_3.Visible = True Then

                    GetSearchDetails = _ReportHelper.GetMandateMovement(GetReportType,
                          SearchOption, txtStartDate.Value.ToString(), txtEndDate.Value.ToString())
                    'Go to report view page
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                    "&ReportName=" & _ReportHelper.SessionReportName & "&StartDt=" _
                        & txtStartDate.Value & "&EndDt=" & txtEndDate.Value, True)



                ElseIf trParam_6.Visible = True Then

                    GetSearchDetails = _ReportHelper.GetMandateMovement(GetReportType,
                                              SearchOption, ddlFieldName.SelectedValue.ToString())

                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                            "&ReportName=" & _ReportHelper.SessionReportName & "&FieldName=" _
                             & ddlFieldName.SelectedItem.Value.ToString(), True)



                End If
                ''CPS Start

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Cheque_Details Then
                ''CPS Cheque Details Reports Start
                If trParam_2.Visible = True And trParam_3.Visible = True Then ''Search By Date
                    GetSearchDetails = _clsCPSPhase3.Report_GetChequeDetails(GetReportType,
                         SearchOption, txtStartDate.Value.ToString(), txtEndDate.Value.ToString())

                ElseIf trParam_8.Visible = True Then ''Search By Cheque No
                    GetSearchDetails = _clsCPSPhase3.Report_GetChequeDetails(GetReportType,
                                         SearchOption, txtChequeNo.Text.ToString(), txtChequeNo_To.Text.ToString())
                ElseIf Search_Option.Text = ReportHelper.CPSPhase3ChequeDetailsReport.All_In_This_Organization Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                    "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Cheque_Details)
                End If
                ''CPS Cheque Details Reports Stop
            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Monthly_Cheque Then
                Server.Transfer("pg_ShowReport.aspx?" &
                                            "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Monthly_Cheque _
                                                & "&StartDt=" & txtStartDate.Value & "&EndDt=" & txtEndDate.Value & "&SearchOption=" & SearchOption & "&OrgId=" & _ReportHelper.SessionOrgId, True)

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Stale_Cheque Then
                If trParam_2.Visible = True And trParam_3.Visible = True Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                                "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Stale_Cheque & "&StartDt=" _
                        & txtStartDate.Value & "&EndDt=" & txtEndDate.Value, True)

                ElseIf Search_Option.Text = ReportHelper.CPSPhase3StaleExpiredChequeReport.All_In_This_Organization Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                    "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Stale_Cheque)
                End If

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque Then
                If trParam_1.Visible = True Then
                    GetSearchDetails = _clsCPSPhase3.Report_GetChequeDetails(GetReportType,
                                         SearchOption, __Param_1.Text.ToString())

                ElseIf trParam_2.Visible = True And trParam_3.Visible = True Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                                                "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque & "&StartDt=" _
                                        & txtStartDate.Value & "&EndDt=" & txtEndDate.Value, True)
                ElseIf trParam_8.Visible = True Then
                    GetSearchDetails = _clsCPSPhase3.Report_GetChequeDetails(GetReportType,
                                         SearchOption, txtChequeNo.Text.ToString(), txtChequeNo_To.Text.ToString())

                ElseIf Search_Option.Text = ReportHelper.CPSPhase3Unclaim_ChequeReport.All_In_This_Organization Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                    "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque)

                End If

            ElseIf _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Damage_Cheque Then
                If trParam_2.Visible = True And trParam_3.Visible = True Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                    "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Damage_Cheque _
                        & "&StartDt=" & txtStartDate.Value & "&EndDt=" & txtEndDate.Value, True)
                ElseIf Search_Option.Text = ReportHelper.CPSPhase3Damage_ChequeReport.All_In_This_Organization Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                                    "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Damage_Cheque)
                ElseIf Search_Option.Text = ReportHelper.CPSPhase3Damage_ChequeReport.Replace_Cheque_Number Then
                    Server.Transfer("pg_ShowReport.aspx?FID=" & _ReportHelper.FileId &
                    "&ReportName=" & _ReportHelper.SessionReportName & "&ReportType=" & ReportHelper.MandateSearchType.CPS_Damage_Cheque _
                        & "&ReplaceCheque=" & True, True)
                ElseIf Search_Option.Text = ReportHelper.CPSPhase3Damage_ChequeReport.Original_Cheque_Number Then
                    GetSearchDetails = _clsCPSPhase3.Report_GetChequeDetails(GetReportType,
                                                         SearchOption, txtChequeNo.Text.ToString(), txtChequeNo_To.Text.ToString())
                End If

                ''CPS END
            End If

            If Not _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Movement_Auto _
            Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Movement_Manual Then
                'populate data grid
                'dgMandate.CurrentPageIndex = 0
                Try
                    If GetSearchDetails.Rows.Count = 0 Then
                        Dim GetSearchDetails2 As DataTable = Nothing
                        Call FormHelp.PopulateDataGrid(GetSearchDetails2, dgMandate)
                        lblMessage.Text = "No Record Found"
                    Else
                        lblMessage.Text = ""
                        Call FormHelp.PopulateDataGrid(GetSearchDetails, dgMandate)
                    End If
                Catch
                    Try
                        dgMandate.CurrentPageIndex = 0
                        Call FormHelp.PopulateDataGrid(GetSearchDetails, dgMandate)
                    Catch ex As Exception

                    End Try
                End Try
                'Call Clear()
            End If


    End Sub
#End Region

#Region "Hide Show Rows "

    Private Sub HideShowRows()

        Try

            If Search_Option.SelectedItem.Text = ReportHelper.MandateBilling.Submission_Date.ToString() _
              Or Search_Option.SelectedItem.Text = ReportHelper.MandateSearchOptionCustomer.Upload_Date.ToString() _
                Or Search_Option.SelectedItem.Text = ReportHelper.MandateSearchOptionBillingSummary.Daily_Report.ToString() _
                    Or Search_Option.SelectedItem.Text = ReportHelper.MandateMovement.Modify_Date.ToString() _
                    Or Search_Option.SelectedItem.Text = ReportHelper.CPSPhase3ChequeDetailsReport.Payment_Value_Date.ToString() _
                    Or Search_Option.SelectedItem.Text = ReportHelper.CPSPhase3Unclaim_ChequeReport.Payment_Value_Date_Range.ToString() _
                    Or Search_Option.SelectedItem.Text = ReportHelper.CPSPhase3StaleExpiredChequeReport.Time_Duration.ToString() _
                    Or Search_Option.SelectedItem.Text = ReportHelper.CPSPhase3Damage_ChequeReport.Payment_Value_Date_Range.ToString() _
                    Or _ReportHelper.GetReportType = ReportHelper.MandateSearchType.CPS_Monthly_Cheque Then

                trParam_1.Visible = False
                trParam_2.Visible = True
                trParam_3.Visible = True
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = False
                trParam_8.Visible = False

            ElseIf Search_Option.SelectedItem.Text = ReportHelper. _
                MandateSearchOptionBillingSummary.Monthly_Report.ToString() Then

                trParam_1.Visible = False
                trParam_2.Visible = False
                trParam_3.Visible = False
                trParam_4.Visible = True
                trParam_5.Visible = True
                trParam_6.Visible = False
                trParam_8.Visible = False
            ElseIf Search_Option.SelectedItem.Text = ReportHelper. _
            MandateMovement.Field_Name.ToString() Then

                trParam_1.Visible = False
                trParam_2.Visible = False
                trParam_3.Visible = False
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = True
                trParam_8.Visible = False

            ElseIf Search_Option.SelectedItem.Text = ReportHelper. _
            MandateSearchOptionManual.All_In_This_Organization.ToString() Then

                trParam_1.Visible = False
                trParam_2.Visible = False
                trParam_3.Visible = False
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = False
                trParam_8.Visible = False
                trParam_9.Visible = False
                ''CPS Start
            ElseIf Search_Option.SelectedItem.Text = ReportHelper. _
                       CPSPhase3ChequeDetailsReport.Cheque_Number.ToString() Then

                trParam_1.Visible = False
                trParam_2.Visible = False
                trParam_3.Visible = False
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = False
                trParam_8.Visible = True
            ElseIf _ReportHelper.GetReportType.ToString() = ReportHelper. _
                   MandateSearchType.CPS_Monthly_Cheque Then

                If Search_Option.SelectedItem.Text = ReportHelper. _
                                       CPSPhase3_MonthlyChequeStatus_Report.Stale_Expired_Cheque.ToString() Then
                    trParam_1.Visible = False
                    trParam_2.Visible = False
                    trParam_3.Visible = False
                    trParam_4.Visible = False
                    trParam_5.Visible = False
                    trParam_6.Visible = False
                    trParam_8.Visible = False
                    trParam_9.Visible = True
                Else
                    trParam_1.Visible = False
                    trParam_2.Visible = False
                    trParam_3.Visible = False
                    trParam_4.Visible = True
                    trParam_5.Visible = True
                    trParam_6.Visible = False
                    trParam_8.Visible = False
                    trParam_9.Visible = False
                End If
            ElseIf _ReportHelper.GetReportType.ToString() = ReportHelper. _
               MandateSearchType.CPS_Stale_Cheque Then

                trParam_1.Visible = False
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = False
                trParam_8.Visible = False

                trParam_9.Visible = False

            ElseIf _ReportHelper.GetReportType.ToString() = ReportHelper. _
          MandateSearchType.CPS_Damage_Cheque Then
                trParam_1.Visible = False
                trParam_2.Visible = False
                trParam_3.Visible = False
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = False
                If Search_Option.SelectedItem.Text = ReportHelper.CPSPhase3Damage_ChequeReport.Original_Cheque_Number.ToString() Then
                    trParam_8.Visible = True
                Else
                    trParam_8.Visible = False

                End If




            Else

                trParam_1.Visible = True
                trParam_2.Visible = False
                trParam_3.Visible = False
                trParam_4.Visible = False
                trParam_5.Visible = False
                trParam_6.Visible = False
                If _ReportHelper.GetReportType = ReportHelper.MandateSearchType.Mandate_Manual_Regis Then
                    trParam_7.Visible = True
                End If
                trParam_8.Visible = False

            End If

        Catch ex As Exception
            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Hide/Show - PG_ReportSearch", Err.Number, Err.Description)


        End Try

    End Sub

#End Region

#Region "Search Option Selectec Change "

    Protected Sub Option_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search_Option.SelectedIndexChanged
        'Create Instance of Data Table
        Dim GetSearchDetails As DataTable = Nothing

        Try
            Call HideShowRows()
            Call Clear()

            dgMandate.DataBind()
        Catch ex As Exception
            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "SearchOption Select- PG_ReportSearch", Err.Number, Err.Description)


        End Try

    End Sub

#End Region

#Region "Clear Text"
    Private Sub Clear()
        txtStartDate.Value = ""
        txtEndDate.Value = ""
        txtYear.Text = ""
        lblMessage.Text = ""




    End Sub
#End Region

#Region "Paging"
    Protected Sub dgMandate_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgMandate.PageIndexChanged
        dgMandate.CurrentPageIndex = e.NewPageIndex
        Call BindGrid()
    End Sub
#End Region

End Class
