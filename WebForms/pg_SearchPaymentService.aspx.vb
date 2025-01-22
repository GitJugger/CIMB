Namespace MaxPayroll

    Partial Class pg_SearchPaymentService
        Inherits clsBasePage

        Public Const sPageTitle As String = "Payment Service"
        ReadOnly Property sPageMode() As String
            Get
                Return Request.QueryString("PageMode") & ""
            End Get
        End Property
        Private Enum enmGridItem
            ePaySrvID = 0
            ePaySrvCode = 1
            ePaySrvDesc = 2
            ePaySrvStatus = 3
            cLink = 4
        End Enum

#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
            If Page.IsPostBack = False Then
                'BindBody(body)
                If Not ss_strUserType = gc_UT_BankAdmin Then
                    Response.Clear()
                    Response.Redirect(gc_LogoutPath, False)

                End If
                Dim clsUsers As New clsUsers

                Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), sPageTitle & " Page", "Y")
                RetrieveData()
            End If
        End Sub
#End Region

#Region "General"

        Private Sub RetrieveData()
            'Declaring new instance of Database.clsDatabase
            Dim clsPaymentService As New clsPaymentService
            Dim dsSearch As New DataSet
            dsSearch = clsPaymentService.fncSearchPaySrv(0, Me.txtPaySrvCode.Text.Trim, txtPaySrvDesc.Text.Trim)

            If dsSearch.Tables("Query").Rows.Count > 0 Then
                fncGeneralGridTheme(dgPaySrvMaintenance)
                dgPaySrvMaintenance.Visible = True
                dgPaySrvMaintenance.DataSource = dsSearch
                dgPaySrvMaintenance.DataBind()
                lblErrorMessage.Text = ""
                pnlGrid.Visible = True
            Else
                pnlGrid.Visible = False
                dgPaySrvMaintenance.Visible = False
                lblErrorMessage.Text = "No records available."

            End If
        End Sub
#End Region

#Region "Submit Page"

        '****************************************************************************************************
        'Procedure Name : Submit_Page()
        'Purpose        : Execute At the time of page Submission
        'Return Value   : N/A
        'Author         : Victor Wong
        'Created        : 2007-02-08
        '*****************************************************************************************************
        Protected Sub prcSearch(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSearch.Click

            RetrieveData()

        End Sub

#End Region

#Region "Payment Window Master Databind"
        Protected Sub dgPaySrvMaintenance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPaySrvMaintenance.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    'Assigning URL - Start
                    Dim hlLink As New HyperLink
                    hlLink = CType(e.Item.FindControl("hlLink"), HyperLink)
                    hlLink.NavigateUrl = "pg_PaymentService.aspx?PaySrvID=" & e.Item.Cells(enmGridItem.ePaySrvID).Text & "&Status=" & e.Item.Cells(enmGridItem.ePaySrvStatus).Text.Substring(0, 1) & "&PageMode=" & sPageMode
                    'Assigning URL - End

                    'If View Mode, change Edit to View - Start
                    If sPageMode = CStr(enmPageMode.ViewMode) Or e.Item.Cells(enmGridItem.ePaySrvStatus).Text.Trim = "C" Or e.Item.Cells(enmGridItem.ePaySrvStatus).Text.Trim = "Cancel" Then
                        hlLink.Text = "View"
                    End If
                    'If View Mode, change Edit to View - End                 

                    'Assigning the whole word base on the status prefix - Start
                    Select Case e.Item.Cells(enmGridItem.ePaySrvStatus).Text.Trim
                        Case "C"
                            e.Item.Cells(enmGridItem.ePaySrvStatus).Text = "Cancelled"
                        Case "A"
                            e.Item.Cells(enmGridItem.ePaySrvStatus).Text = "Active"
                        Case "I"
                            e.Item.Cells(enmGridItem.ePaySrvStatus).Text = "Inactive"
                    End Select
                    'Assigning the whole word base on the status prefix - End
            End Select
        End Sub
#End Region

    End Class

End Namespace
