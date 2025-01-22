Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsCustomer


Namespace MaxPayroll


Partial Class PG_ViewCustomer
      Inherits clsBasePage

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of User Class Object
        Dim clsUsers As New MaxPayroll.clsUsers

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        Try


            If Not ss_strUserType = gc_UT_BankUser And Not ss_strUserType = gc_UT_InquiryUser Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then
               Call prBindGrid()
               Call clsUsers.prcDetailLog(ss_lngUserID, "View Customer File Format", "Y")
            End If

        Catch ex As Exception
            LogError("PG_ViewCustomer - Page Load")
        Finally

            'Destroy Instance of User Class Object
            clsUsers = Nothing

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

        End Try

    End Sub

#End Region

#Region "Page_Change"

    Sub Page_Change(ByVal Sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgOrganisation.PageIndexChanged

        Dim intStart As Integer

        Try
            intStart = dgOrganisation.CurrentPageIndex * dgOrganisation.PageSize
            dgOrganisation.CurrentPageIndex = e.NewPageIndex
            Call prBindGrid()
        Catch ex As Exception

        End Try

        

    End Sub

#End Region

#Region "Show All"

    Private Sub prShow(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnShow.Click

        Try
            Server.Transfer("PG_ViewCustomer.aspx", False)
        Catch ex As Exception

        End Try


    End Sub

#End Region

#Region "Clear Contents"

    Private Sub prClear(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnClear.Click

        txtKeyword.Text = ""
        cmbOption.SelectedValue = ""
        cmbCriteria.SelectedValue = ""

    End Sub

#End Region

#Region "Bind Data grid"

    '****************************************************************************************************
    'Function Name  : prBindGrid()
    'Purpose        : Bind The User Data Grid
    'Arguments      : N/A
    'Return Value   : Data Grid
        'Author         : Sujith Sharatchandran - 
    'Created        : 25/10/2003
    '*****************************************************************************************************
    Sub prBindGrid()

        Dim clsGeneric As New MaxPayroll.Generic               'Create Generic Class Object
        Dim dsCustomer As New System.Data.DataSet           'Create Data Set
        Dim clsCustomer As New MaxPayroll.clsCustomer          'Create Customer Class Object

        'Variable Declarations
        Dim lngOrganisationId As Long, lngUserId As Long, intRecordCount As Int32

        Try

            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
            lngOrganisationId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)

            'Populate Data Grid - Start
            dsCustomer = clsCustomer.fnCustomer()
            intRecordCount = dsCustomer.Tables(0).Rows.Count
            If intRecordCount > 0 Then
                dgOrganisation.Visible = True
                lblMessage.Text = ""
                dgOrganisation.DataSource = dsCustomer
                dgOrganisation.DataBind()
            Else
                dgOrganisation.Visible = False
                lblMessage.Text = "No Customers Found/Available"
            End If
            'Populate Data Grid - Stop

        Catch

            clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "prBindGrid - PG_ViewCustomer", Err.Number, Err.Description)

        Finally

            clsCustomer = Nothing
            clsGeneric = Nothing

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

End Class

End Namespace
