

Namespace MaxPayroll

    Partial Class pg_SearchReportServicesByOrganization
        Inherits clsBasePage

#Region "Request.QueryString"
        Private ReadOnly Property rq_strReportName() As String
            Get
                Return Request.QueryString("ReportName") & ""
            End Get
        End Property

        Private _ReportHelper As New ReportHelper

#End Region

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon
            Try
                If rq_strReportName IsNot Nothing Then
                    Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(rq_strReportName)
                    If strEncUsername = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                End If
                If ss_PwdChgStatus = "Y" Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                If ss_AuthChgStatus = "Y" Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                If Not Page.IsPostBack Then
                    'BindBody(body)
                    'Store Report Name in Session
                    Session("REPORT") = rq_strReportName
                    'Bind Data grid
                    Call prBindGrid()
                End If

            Catch ex As Exception

            Finally

                'Destroy Generic
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Bind Grid"

        Sub prBindGrid()

            'Create Instance of System Data Set
            Dim dsOrg As New System.Data.DataSet

            'Create Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer


            'Variable Declarations
            Dim lngOrganisationId As Long, lngUserId As Long, intRecordCount As Int32

            Try

                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
                lngOrganisationId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)

                dsOrg = clsCustomer.fnOrgGrid(ss_lngOrgID, ss_lngUserID) 'lngOrganisationId, lngUserId)
                intRecordCount = dsOrg.Tables(0).Rows.Count
                If intRecordCount > 0 Then
                    pnlGrid.Visible = True
                    lblMessage.Visible = False
                    dgOrganisation.Visible = True
                    dgOrganisation.DataSource = dsOrg
                    dgOrganisation.DataBind()
                Else
                    pnlGrid.Visible = False
                    lblMessage.Visible = True
                    dgOrganisation.Visible = False
                    lblMessage.Text = "No Organization Found/Matching."
                End If

            Catch

                'Log Error
                clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "prBindGrid - PG_ViewOrganisation", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Button Events"

        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try
                intStart = dgOrganisation.CurrentPageIndex * dgOrganisation.PageSize
                dgOrganisation.CurrentPageIndex = E.NewPageIndex
                Call prBindGrid()
            Catch ex As Exception

            End Try

        End Sub

        Sub Create_New(ByVal Sender As Object, ByVal e As EventArgs)
            Try
                Server.Transfer("PG_Organisation.aspx", False)
            Catch ex As Exception

            End Try

        End Sub

        Sub Show_All(ByVal Sender As Object, ByVal e As EventArgs)

            Try
                Server.Transfer("PG_SearchReports.aspx", False)
            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Search"

        Private Sub prSearch(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

            Try
                dgOrganisation.CurrentPageIndex = 0
                Call prBindGrid()
            Catch ex As Exception

            End Try

        End Sub
#End Region

        Protected Sub dgOrganisation_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgOrganisation.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem

                    Dim ancViewReport As New System.Web.UI.HtmlControls.HtmlAnchor
                    ancViewReport = CType(e.Item.FindControl("ancViewReport"), HtmlAnchor)

                    _ReportHelper.SessionReportName = rq_strReportName

                    If rq_strReportName = ReportHelper.MandateSearchType.Mandate_Manual_Regis.ToString() _
                        Or rq_strReportName = ReportHelper.MandateSearchType.Mandate_Auto_Regis.ToString() _
                            Or rq_strReportName = ReportHelper.MandateSearchType.Mandate_Billing_Upload.ToString() _
                            Or rq_strReportName = ReportHelper.MandateSearchType.Mandate_Billing_Reject.ToString() _
                              Or rq_strReportName = ReportHelper.MandateSearchType.Mandate_Summary.ToString() _
                                Or rq_strReportName = ReportHelper.MandateSearchType.Mandate_Movement_Auto.ToString() _
                                  Or rq_strReportName = ReportHelper.MandateSearchType.Mandate_Movement_Manual.ToString() _
                                    Or rq_strReportName = ReportHelper.MandateSearchType.CPS_Cheque_Details.ToString() _
                                     Or rq_strReportName = ReportHelper.MandateSearchType.CPS_Monthly_Cheque.ToString() _
                                      Or rq_strReportName = ReportHelper.MandateSearchType.CPS_Stale_Cheque.ToString() _
                                       Or rq_strReportName = ReportHelper.MandateSearchType.CPS_Unclaimed_Cheque.ToString() _
                                        Or rq_strReportName = ReportHelper.MandateSearchType.CPS_Damage_Cheque.ToString() _
                                         Or rq_strReportName = ReportHelper.MandateSearchType.CPS_Charges_Report.ToString() Then
                        ancViewReport.HRef = "javascript:window.location.href='PG_ReportSearch.aspx?ReportName=" & rq_strReportName & "&Id=" & ancViewReport.InnerText & "';"
                    Else
                        ancViewReport.HRef = "javascript:window.location.href='PG_ViewReportServices.aspx?ReportName=" & rq_strReportName & "&Id=" & ancViewReport.InnerText & "';"
                    End If


            End Select
        End Sub
    End Class

End Namespace
