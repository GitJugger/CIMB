Namespace MaxPayroll

    Partial Class pg_SearchPaymentWin
      Inherits clsBasePage
      Public Const sPageTitle As String = "Payment Window"
        ReadOnly Property sPageMode() As String
            Get
                Return Request.QueryString("PageMode") & ""
            End Get
        End Property
        Private Enum enmGridItem
            ePayWinID = 0
            ePayWinCode = 1
            ePayWinDesc = 2
            eBankName = 3
            ePayWinStatus = 4
            cLink = 5
        End Enum

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here

            If Page.IsPostBack = False Then
                'BindBody(body)
                If Not ss_strUserType = gc_UT_BankUser Then
                    Server.Transfer(gc_LogoutPath, False)
                End If
                Dim clsUsers As New clsUsers
                prcBindBank()

                Dim sRetval As String
                sRetval = clsCommon.fncDefaultBankChecking(Me.ddlBankID, Me.lblBankID)
                If Len(sRetval) > 0 Then
                    Me.lblErrorMessage.Text = sRetval
                    Exit Sub
                End If

                Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), "View Bank Code Page", "Y")
                RetrieveData()
            End If
        End Sub
#End Region

#Region "General"
        Private Sub prcBindBank()
            Dim clsBankMF As New clsBankMF

         ddlBankID.DataSource = clsBankMF.fncRetrieveBankCodeName("A")
            ddlBankID.DataTextField = "BankName"
            ddlBankID.DataValueField = "BankID"
            ddlBankID.DataBind()
            ddlBankID.Items.Insert(0, New ListItem("Select", 0))
        End Sub

        Private Sub RetrieveData()
            'Declaring new instance of Database.clsDatabase
            Dim clsPaymentMF As New clsPaymentMF
            Dim dsSearch As New DataSet

            'dsSearch = clsDatabase.fncDisplayBankDef(txtBankCode.Text)
            dsSearch = clsPaymentMF.fncSearchPayWinDef(0, txtPayWinCode.Text.Trim, txtPayWinDesc.Text.Trim, ddlBankID.SelectedValue.Trim)
         If dsSearch.Tables("Query").Rows.Count > 0 Then
            fncGeneralGridTheme(dgPayWinMaintenance)
            dgPayWinMaintenance.Visible = True
            dgPayWinMaintenance.DataSource = dsSearch
                dgPayWinMaintenance.DataBind()

                '071022 3 lines added by Marcus
                'Purpose: To hide bank info in datagrid if there is a default bank set up in web.config.
                If clsCommon.fncDefaultBankChecking() Then
                    dgPayWinMaintenance.Columns(enmGridItem.eBankName).Visible = False
                End If

            lblErrorMessage.Text = ""
         Else
            Me.pnlGrid.Visible = False
            dgPayWinMaintenance.Visible = False
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
        Protected Sub dgPayWinMaintenance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPayWinMaintenance.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    'Assigning URL - Start
                    Dim hlLink As New HyperLink
                    hlLink = CType(e.Item.FindControl("hlLink"), HyperLink)
                    hlLink.NavigateUrl = "pg_PaymentWinMaster.aspx?PayWinId=" & e.Item.Cells(enmGridItem.ePayWinID).Text & "&Status=" & e.Item.Cells(enmGridItem.ePayWinStatus).Text.Substring(0, 1) & "&PageMode=" & sPageMode
                    'Assigning URL - End

                    'If View Mode, change Edit to View - Start
                    If sPageMode = CStr(enmPageMode.ViewMode) Or e.Item.Cells(enmGridItem.ePayWinStatus).Text.Trim = "C" Or e.Item.Cells(enmGridItem.ePayWinStatus).Text.Trim = "Cancel" Then
                        hlLink.Text = "View"
                    End If
                    'If View Mode, change Edit to View - End                 

                    'Assigning the whole word base on the status prefix - Start
                    Select Case e.Item.Cells(enmGridItem.ePayWinStatus).Text.Trim
                  Case "Cancel"
                     e.Item.Cells(enmGridItem.ePayWinStatus).Text = "Cancelled"
                  Case "Active"
                     e.Item.Cells(enmGridItem.ePayWinStatus).Text = "Active"
                  Case "Inactive"
                     e.Item.Cells(enmGridItem.ePayWinStatus).Text = "Inactive"
               End Select
                    'Assigning the whole word base on the status prefix - End
            End Select
        End Sub
#End Region

    End Class

End Namespace
