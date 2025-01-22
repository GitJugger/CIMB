Namespace MaxPayroll

    Partial Class pg_SearchBank
      Inherits clsBasePage

#Region "Declaration"
        ReadOnly Property sPageMode() As String
            Get
                Return Request.QueryString("PageMode") & ""
            End Get
        End Property
        Private Enum enmGridItem
            eBankCode = 0
            eBankName = 1
            eStatus = 2
            cLink = 3
        End Enum
#End Region

#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            'Put user code to initialize the page here
         If Page.IsPostBack = False Then

                'BindBody(body)

            If Not ss_strUserType = gc_UT_BankUser Then
               Response.Clear()
               Response.Redirect(gc_LogoutPath, False)

            End If
            Dim clsUsers As New clsUsers
            Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), "View Bank Code Page", "Y")
            RetrieveData()
         End If
        End Sub
#End Region

#Region "General"
        Private Sub RetrieveData()
            'Declaring new instance of Database.clsDatabase
            Dim clsBankMF As New clsBankMF

            Dim dsSearch As New DataSet

            'dsSearch = clsDatabase.fncDisplayBankDef(txtBankCode.Text)
            dsSearch = clsBankMF.fncSearchBankDefinition(txtBankCode.Text, txtBankName.Text, 0)
         If dsSearch.Tables("Query").Rows.Count > 0 Then
            fncGeneralGridTheme(dgBankCodeMaintenance)
            dgBankCodeMaintenance.Visible = True
            dgBankCodeMaintenance.DataSource = dsSearch
            dgBankCodeMaintenance.DataBind()
            lblErrorMessage.Text = ""
         Else
            Me.pnlGrid.Visible = False
            dgBankCodeMaintenance.Visible = False
            lblErrorMessage.Text = "No records available."

         End If
        End Sub
#End Region

#Region "Submit Page"

        '****************************************************************************************************
        'Procedure Name : Submit_Page()
        'Purpose        : Execute At the time of page Submission
        'Return Value   : N/A
        'Author         : Deedee Ibrahim - T-Melmax Sdn Bhd
        'Created        : 19/01/2006
        '*****************************************************************************************************
        Protected Sub prcSave(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSearch.Click

            RetrieveData()

        End Sub

#End Region

#Region "Bank Master Databind"
        Protected Sub dgBankCodeMaintenance_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgBankCodeMaintenance.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.Item, ListItemType.AlternatingItem
                    'Assigning URL - Start
                    Dim hlLink As New HyperLink
                    hlLink = CType(e.Item.FindControl("hlLink"), HyperLink)
                    hlLink.NavigateUrl = "pg_BankMaster.aspx?BankId=" & e.Item.Cells(enmGridItem.eBankCode).Text & "&Status=" & e.Item.Cells(enmGridItem.eStatus).Text.Substring(0, 1) & "&PageMode=" & sPageMode
                    'Assigning URL - End

                    'If View Mode, change Edit to View - Start
                    If sPageMode = CStr(enmPageMode.ViewMode) Or e.Item.Cells(enmGridItem.eStatus).Text.Trim = "C" Then
                        hlLink.Text = "View"
                    End If
                    'If View Mode, change Edit to View - End                 

                    'Assigning the whole word base on the status prefix - Start
                    Select Case e.Item.Cells(enmGridItem.eStatus).Text.Trim
                        Case "C"
                     e.Item.Cells(enmGridItem.eStatus).Text = "Cancelled"
                        Case "A"
                            e.Item.Cells(enmGridItem.eStatus).Text = "Active"
                        Case "I"
                            e.Item.Cells(enmGridItem.eStatus).Text = "Inactive"
                    End Select
                    'Assigning the whole word base on the status prefix - End
            End Select
        End Sub
#End Region

    End Class

End Namespace
