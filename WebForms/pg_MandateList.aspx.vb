Namespace MaxPayroll

    Partial Class pg_MandateList
        Inherits clsBasePage

#Region "Global Variables"
        Enum enmGrid
            OrgID
            RefNo
            BankOrgCode
            CustomerName
            AccNo
            LimitAmount
            Frequency
            FrequencyLimit
        End Enum

        Private ReadOnly Property rq_strReq() As String
            Get
                Return Request.QueryString("Req") & ""
            End Get
        End Property

        Private ReadOnly Property rq_ID() As Long
            Get
                Dim sTemp As String = Request.QueryString("ID")
                If IsNumeric(sTemp) Then
                    Return CLng(sTemp)
                Else
                    Return 0
                End If
            End Get
        End Property
#End Region

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                'If User Type Not Bank User or Inquiry User - Start
                If ss_strUserType <> gc_UT_BankUser AndAlso ss_strUserType <> gc_UT_BankAuth Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If User Type Not Bank User or Inquiry User - Stop

                hReq.Value = rq_strReq


                If Not Page.IsPostBack Then
                    'Fill up the title
                    lblHeading.Text = "Mandate Listing (" & rq_ID & ")"
                    'Bind Data grid
                    Call prBindGrid()
                End If

            Catch ex As Exception

            Finally

            End Try

        End Sub

#End Region

#Region "Bind Grid"
        Sub prBindGrid()


            Dim oItem As New clsMandates

            Try
                'Create Instance of Common Class Object
                Dim clsCommon As New MaxPayroll.clsCommon
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtRefNo.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim folder As Boolean = clsCommon.CheckScriptValidation(txtAccountNo.Text)
                If folder = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim errorfolder As Boolean = clsCommon.CheckScriptValidation(txtCustName.Text)
                If errorfolder = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If

                oItem.paramOrgID = Me.rq_ID
                oItem.paramRefNo = Me.txtRefNo.Text
                oItem.paramAccNo = Me.txtAccountNo.Text
                oItem.paramBankOrgCode = Me.txtBankOrgCode.Text
                oItem.paramCustomerName = Me.txtCustName.Text


                Me.dgMandateList.DataSource = oItem.LoadList()
                dgMandateList.DataBind()
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

#End Region

#Region "Button Events"

        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try
                'intStart = dgOrganisation.CurrentPageIndex * dgOrganisation.PageSize
                'dgOrganisation.CurrentPageIndex = E.NewPageIndex
                Call prBindGrid()
            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Search"

        Private Sub prSearch(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

            Try
                Me.dgMandateList.CurrentPageIndex = 0
                Call prBindGrid()
            Catch ex As Exception

            End Try

        End Sub
#End Region



        Protected Sub dgMandateList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgMandateList.ItemDataBound
            Select Case e.Item.ItemType
                Case ListItemType.AlternatingItem, ListItemType.Item
                    Dim oItem As New clsMandates
                    e.Item.Cells(enmGrid.Frequency).Text = oItem.fncGetFrequencyDesc(e.Item.Cells(enmGrid.Frequency).Text)
            End Select
        End Sub

        Protected Sub dgMandateList_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgMandateList.PageIndexChanged
            Me.dgMandateList.CurrentPageIndex = e.NewPageIndex
            prBindGrid()
        End Sub
    End Class

End Namespace
