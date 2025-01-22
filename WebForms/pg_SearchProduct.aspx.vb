Namespace MaxPayroll

    Partial Class pg_SearchProduct
        Inherits clsBasePage

        Public Const sPageTitle As String = "Product"
        ReadOnly Property sPageMode() As String
            Get
                Return Request.QueryString("PageMode") & ""
            End Get
        End Property
        Private Enum enmGridItem
            ProductName = 0
            Status = 1
            hlAction = 2
            ProductID = 3
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
                BindGrid()
            End If
        End Sub
#End Region

#Region "General"

        Private Sub BindGrid()
            Dim oItem As New clsProduct
            Dim dsSearch As New DataSet

            dsSearch = oItem.fncRetrieveDataList(txtProductName.Text)

            If dsSearch.Tables(0).Rows.Count > 0 Then
                fncGeneralGridTheme(dgProduct)
                dgProduct.Visible = True
                dgProduct.DataSource = dsSearch
                dgProduct.DataBind()
                pnlGrid.Visible = True
                lblErrorMessage.Text = ""
            Else
                pnlGrid.Visible = False
                dgProduct.Visible = False
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

            BindGrid()

        End Sub

#End Region

#Region "Databind"
        Protected Sub dgProduct_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgProduct.ItemDataBound

            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    'Assigning URL - Start
                    Dim hlLink As New HyperLink
                    hlLink = CType(e.Item.FindControl("hlLink"), HyperLink)
                    hlLink.NavigateUrl = "pg_Product.aspx?ID=" & e.Item.Cells(enmGridItem.ProductID).Text & "&PageMode=" & enmPageMode.EditMode
                    'Assigning URL - End

                    'Assigning the whole word base on the status prefix - Start
                    Select Case e.Item.Cells(enmGridItem.Status).Text.Trim
                        Case clsCommon.fncGetPrefix(enmStatus.A_Active)
                            e.Item.Cells(enmGridItem.Status).Text = "Active"
                        Case clsCommon.fncGetPrefix(enmStatus.I_Inactive)
                            e.Item.Cells(enmGridItem.Status).Text = "Inactive"
                    End Select
                    'Assigning the whole word base on the status prefix - End
            End Select
        End Sub
#End Region

    End Class

End Namespace
