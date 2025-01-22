Imports MaxMiddleware
Namespace MaxPayroll

    Partial Class WebForms_pg_CPSOrgList
        Inherits clsBasePage

        Private _Helper As New Helper

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not Page.IsPostBack Then
                    Call prBindGrid()
                End If

            Catch ex As Exception

            Finally

            End Try

        End Sub

#End Region

#Region "Bind Grid"

        Sub prBindGrid()

            'Create Instance of System Data Set
            Dim dsOrg As New System.Data.DataSet

            'Create Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            ''Dim clsGeneric As New 

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer


            'Variable Declarations
            Dim lngOrganisationId As Long, lngUserId As Long, intRecordCount As Int32

            Try

                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
                lngOrganisationId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)

                'Get the Organizations Which are applicable for charges
                dsOrg = PPS.GetDataSet(_Helper.SQLGetChargesOrganizations, _
                    _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

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
                clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "prBindGrid - PG_CPSOrgList", Err.Number, Err.Description)

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


    End Class

End Namespace
