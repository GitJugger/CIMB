Namespace MaxPayroll

Partial Class PG_ViewOrganisation
        Inherits clsBasePage

#Region "Global Variables"
        Enum enmGrid
            OrgID = 0
            Name = 1
            Phone = 2
            Registered = 3
            Status = 4
            ModifyOrg = 5
            ViewOrg = 6
            ModifyBankAccount = 7
            ModifyListFile = 8
            ViewListFile = 9
            'ModifyEPFService = 10
            'ModifySocsoService = 11
            'ModifyLHDNService = 12
            ViewBankAccount = 13
            'ViewEPFService = 14
            'ViewSocsoService = 15
            'ViewLHDNService = 16
            CreateOrgCharge = 17
            ModifyOrgCharge = 18
            'ModifyZAKATCharge = 19
            'ViewZAKATService = 20
            BankCodeMapping = 21
            OrganizationCreateUser = 22
            OrganizationModifyUser = 23
            MandatesCreation = 24
            MandatesModification = 25
            CPS_Maintainence_Charges = 26
            CPS_File_Submission = 27

        End Enum

        Private ReadOnly Property rq_strReq() As String
            Get
                Return Request.QueryString("Req") & ""
            End Get
        End Property
#End Region

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Not Page.IsPostBack Then
                    'BindBody(body)
                End If

                'If User Type Not Bank User or Inquiry User - Start
                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser Or ss_strUserType = gc_UT_BankAdmin) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If User Type Not Bank User or Inquiry User - Stop

                hReq.Value = rq_strReq

                If Not Page.IsPostBack Then
                    'Fill up the title
                    prcBindTitle()
                    'Bind Data grid
                    Call prBindGrid()
                End If

            Catch ex As Exception

            Finally

            End Try

        End Sub

        Private Sub prcBindTitle()
            Dim sTitle As String = " - Search Organization"
            btnNew.Visible = False
            Select Case rq_strReq
                Case enmViewOrganizationReqType.Modify.ToString
                    btnNew.Visible = True
                    lblHeading.Text = "Modify Organization" & sTitle
                Case enmViewOrganizationReqType.Bank.ToString
                    lblHeading.Text = "Bank Account" & sTitle
                Case enmViewOrganizationReqType.File.ToString
                    lblHeading.Text = "File Settings" & sTitle
                    'Case enmViewOrganizationReqType.Epf.ToString
                    '    lblHeading.Text = "EPF Accounts" & sTitle
                    'Case enmViewOrganizationReqType.Socso.ToString
                    '    lblHeading.Text = "SOCSO Accounts" & sTitle
                    'Case enmViewOrganizationReqType.LHDN.ToString
                    '    lblHeading.Text = "LHDN Accounts" & sTitle
                Case MaxPayroll.mdConstant.enmViewOrganizationReqType.CreateOrgCharge.ToString
                    lblHeading.Text = "Create Charges" & sTitle
                Case MaxPayroll.mdConstant.enmViewOrganizationReqType.ModifyOrgCharge.ToString
                    lblHeading.Text = "Charges" & sTitle
                    'Case enmViewOrganizationReqType.ZAKAT.ToString
                    '    lblHeading.Text = "ZAKAT Accounts" & sTitle
                Case (enmViewOrganizationReqType.BankCodeMapping.ToString)
                    lblHeading.Text = "Bank Code Mapping" & sTitle
                Case enmViewOrganizationReqType.Mandates.ToString
                    lblHeading.Text = "Organization Mandates" & sTitle
            End Select
        End Sub

#End Region

#Region "Bind Grid"
        Sub prBindGrid()

            'Create Instance of System Data Set
            Dim dsOrg As New System.Data.DataSet

            'Create  Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer


            'Variable Declarations
            Dim intRecordCount As Int32
            Try

                'Populate Data Set
                If rq_strReq = enmViewOrganizationReqType.H2HUser.ToString Then
                    dsOrg = clsCustomer.fnH2HOrgGrid(ss_lngOrgID, ss_lngUserID)
                Else
                    dsOrg = clsCustomer.fnOrgGrid(ss_lngOrgID, ss_lngUserID)
                End If


                'Get Total Records in Data Set
                intRecordCount = dsOrg.Tables("ORGANISATION").Rows.Count

                'If Records
                If intRecordCount > 0 Then

                    Me.pnlGrid.Visible = True
                    'Hide Messages Text
                    lblMessage.Visible = False

                    'Bind Data Grid - Start
                    dgOrganisation.Visible = True
                    dgOrganisation.DataSource = dsOrg
                    dgOrganisation.DataBind()
                    fncGeneralGridTheme(dgOrganisation)
                    'Bind Data Grid - Stop

                    'Column Display Control - Start
                    Select Case ss_strUserType
                        Case gc_UT_BankAdmin
                            Select Case rq_strReq
                                Case enmViewOrganizationReqType.H2HUser.ToString
                                    dgOrganisation.Columns(enmGrid.OrganizationCreateUser).Visible = True
                                    dgOrganisation.Columns(enmGrid.OrganizationModifyUser).Visible = True
                            End Select
                        Case gc_UT_BankUser
                            Select Case rq_strReq
                                Case enmViewOrganizationReqType.Modify.ToString
                                    dgOrganisation.Columns(enmGrid.ModifyOrg).Visible = True
                                Case enmViewOrganizationReqType.Bank.ToString
                                    dgOrganisation.Columns(enmGrid.ModifyBankAccount).Visible = True
                                Case enmViewOrganizationReqType.File.ToString
                                    dgOrganisation.Columns(enmGrid.ModifyListFile).Visible = True
                                    'Case enmViewOrganizationReqType.Epf.ToString
                                    '    dgOrganisation.Columns(enmGrid.ModifyEPFService).Visible = True
                                    'Case enmViewOrganizationReqType.Socso.ToString
                                    '    dgOrganisation.Columns(enmGrid.ModifySocsoService).Visible = True
                                    'Case enmViewOrganizationReqType.LHDN.ToString
                                    '    dgOrganisation.Columns(enmGrid.ModifyLHDNService).Visible = True
                                Case enmViewOrganizationReqType.CreateOrgCharge.ToString
                                    dgOrganisation.Columns(enmGrid.CreateOrgCharge).Visible = True
                                Case enmViewOrganizationReqType.ModifyOrgCharge.ToString
                                    dgOrganisation.Columns(enmGrid.ModifyOrgCharge).Visible = True
                                    'Case enmViewOrganizationReqType.ZAKAT.ToString
                                    '    dgOrganisation.Columns(enmGrid.ModifyZAKATCharge).Visible = True
                                Case enmViewOrganizationReqType.BankCodeMapping.ToString
                                    dgOrganisation.Columns(enmGrid.BankCodeMapping).Visible = True

                                Case enmViewOrganizationReqType.Mandates.ToString
                                    dgOrganisation.Columns(enmGrid.MandatesCreation).Visible = True
                                    dgOrganisation.Columns(enmGrid.MandatesModification).Visible = True
                                Case enmViewOrganizationReqType.CPS.ToString
                                    dgOrganisation.Columns(enmGrid.CPS_Maintainence_Charges).Visible = True
                                Case enmViewOrganizationReqType.CPS_ChequeModify.ToString
                                    dgOrganisation.Columns(enmGrid.CPS_File_Submission).Visible = True

                            End Select
                        Case gc_UT_InquiryUser
                            Select Case rq_strReq
                                Case enmViewOrganizationReqType.Modify.ToString
                                    dgOrganisation.Columns(enmGrid.ViewOrg).Visible = True
                                Case enmViewOrganizationReqType.Bank.ToString
                                    dgOrganisation.Columns(enmGrid.ViewBankAccount).Visible = True
                                Case enmViewOrganizationReqType.File.ToString
                                    dgOrganisation.Columns(enmGrid.ViewListFile).Visible = True
                                    'Case enmViewOrganizationReqType.Epf.ToString
                                    '    dgOrganisation.Columns(enmGrid.ViewEPFService).Visible = True
                                    'Case enmViewOrganizationReqType.Socso.ToString
                                    '    dgOrganisation.Columns(enmGrid.ViewSocsoService).Visible = True
                                    'Case enmViewOrganizationReqType.LHDN.ToString
                                    '    dgOrganisation.Columns(enmGrid.ViewLHDNService).Visible = True
                                    'Case enmViewOrganizationReqType.ZAKAT.ToString
                                    '    dgOrganisation.Columns(enmGrid.ViewZAKATService).Visible = True
                            End Select
                    End Select
                    'Column Display Control - End

                Else
                    'If No Records
                    Me.pnlGrid.Visible = False
                    lblMessage.Visible = True
                    dgOrganisation.Visible = False
                    lblMessage.Text = "No Organization Found/Matching."
                End If

            Catch
                LogError("pg_ViewOrganisation - PrBindGrid")
            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Data Set
                dsOrg = Nothing

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
                    Dim hlink As New HyperLink
                    hlink = CType(e.Item.FindControl("hlkModOrg"), HyperLink)
                    hlink.NavigateUrl = "PG_Organization.aspx?Id=" & e.Item.Cells(enmGrid.OrgID).Text & "&Name=" & e.Item.Cells(enmGrid.Name).Text & "&PageMode=" & enmPageMode.EditMode
                    hlink = New HyperLink
                    hlink = CType(e.Item.FindControl("hlkViewOrg"), HyperLink)
                    hlink.NavigateUrl = "PG_Organization.aspx?Id=" & e.Item.Cells(enmGrid.OrgID).Text & "&Name=" & e.Item.Cells(enmGrid.Name).Text & "&PageMode=" & enmPageMode.ViewMode

            End Select
        End Sub

        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function
    End Class

End Namespace
