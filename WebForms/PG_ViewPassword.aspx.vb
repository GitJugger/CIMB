Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers


Namespace MaxPayroll


Partial Class PG_ViewPassword
      Inherits clsBasePage

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load, Me.Load

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic


            Try
                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                If Not Page.IsPostBack Then
                    'Bind Grid
                    'BindBody(body)
                    Call prBindGrid()
                    Call clsUsers.prcDetailLog(ss_lngUserID, "View/Search Users", "Y")
                End If

            Catch ex As Exception
                LogError("PG_ViewPassword - Page Load")
            Finally

                'Destroy Instance of User Class Object
                clsUsers = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Bind Grid "

    Sub prBindGrid()

        'Create Instance of User Class Object
        Dim clsUsers As New MaxPayroll.clsUsers

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of System Data Set Object
        Dim dsUser As New System.Data.DataSet

        'Variable Declarations

        Dim lngOrganisationId As Long, lngUserId As Long, intRecordCount As Int32

        Try

            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
            lngOrganisationId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)

            dsUser = clsUsers.UserGrid()
            intRecordCount = dsUser.Tables("USER").Rows.Count
            If intRecordCount > 0 Then
                lblMsg.Visible = False
                dgUser.Visible = True
                dgUser.DataSource = dsUser
                dgUser.DataBind()
                If Session("SYS_TYPE") = "IU" Then
                    dgUser.Columns(5).Visible = False
                    dgUser.Columns(7).Visible = False
                    dgUser.Columns(8).Visible = False
                    dgUser.Columns(9).Visible = False
                Else
                    dgUser.Columns(6).Visible = False
                    dgUser.Columns(9).Visible = False
                    If Request.QueryString("Req") = "C" Then
                        dgUser.Columns(5).Visible = False
                        dgUser.Columns(6).Visible = False
                        dgUser.Columns(7).Visible = False
                        dgUser.Columns(8).Visible = False
                        dgUser.Columns(9).Visible = True
                    End If
                End If
            Else
                lblMsg.Visible = True
                dgUser.Visible = False
                lblMsg.Text = "No User Found/Matching."
            End If

        Catch

            'Log Error
            Call clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "prBindGrid - PG_ViewPassword", Err.Number, Err.Description)

        Finally

            'Destroy Instance of Data Set
            dsUser = Nothing

            'Destroy Instance of User Class Object
            clsUsers = Nothing

            'Destroy Instance of Generic Class object
            clsGeneric = Nothing

        End Try

    End Sub
#End Region

#Region "Page_Change"

    Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

        Dim intStart As Int16

        Try
            intStart = dgUser.CurrentPageIndex * dgUser.PageSize
            dgUser.CurrentPageIndex = E.NewPageIndex
            Call prBindGrid()
        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Search"

    Private Sub prSearch(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            dgUser.CurrentPageIndex = 0
            Call prBindGrid()
        Catch ex As Exception

        End Try

    End Sub

#End Region

      Protected Sub dgUser_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUser.ItemDataBound
         Select Case e.Item.ItemType
            Case ListItemType.AlternatingItem, ListItemType.Item
               Select Case e.Item.Cells(3).Text.ToUpper
                  Case gc_UT_Auth
                     e.Item.Cells(3).Text = gc_UT_AuthDesc
                  Case gc_UT_BankAdmin
                     e.Item.Cells(3).Text = gc_UT_BankAdminDesc
                  Case gc_UT_BankAuth
                     e.Item.Cells(3).Text = gc_UT_BankAuthDesc
                  Case gc_UT_BankOperator
                     e.Item.Cells(3).Text = gc_UT_BankOperatorDesc
                  Case gc_UT_BankUser
                     e.Item.Cells(3).Text = gc_UT_BankUserDesc
                  Case gc_UT_InquiryUser
                     e.Item.Cells(3).Text = gc_UT_InquiryUserDesc
                  Case gc_UT_Interceptor
                     e.Item.Cells(3).Text = gc_UT_InterceptorDesc
                  Case gc_UT_Reviewer
                     e.Item.Cells(3).Text = gc_UT_ReviewerDesc
                  Case gc_UT_SysAdmin
                     e.Item.Cells(3).Text = gc_UT_SysAdminDesc
                  Case gc_UT_SysAuth
                     e.Item.Cells(3).Text = gc_UT_SysAuthDesc
                  Case gc_UT_Uploader
                     e.Item.Cells(3).Text = gc_UT_UploaderDesc

               End Select
         End Select
      End Sub

        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Me.cmbCriteria.SelectedIndex = 0
            Me.cmbOption.SelectedIndex = 0
            Me.txtKeyword.Text = ""

            'dgUser.CurrentPageIndex = 0
            'Call prBindGrid()

        End Sub
        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function
    End Class

End Namespace

