Option Explicit On 
Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient


Namespace MaxPayroll


Partial Class PG_ViewRoles
        Inherits clsBasePage

#Region "Declaration"
        Public Enum dg
            UserLogin
            UserName
            UserType
            UserStatus
            CreateDate
            lnkModify
            lnkChgPwd
            lnkAuthCode
            LastApprovalStatus
        End Enum
#End Region

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Put user code to initialize the page here

        'Create Instance of Generic Class
        Dim clsGeneric As New MaxPayroll.Generic


        Try
            If ss_strUserType <> gc_UT_SysAdmin And ss_strUserType <> gc_UT_BankAdmin Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            ElseIf ss_strUserType = gc_UT_BankAdmin Then
               tblRole.Visible = False
            End If
          

            If Not ss_strUserType.Equals(gc_UT_BankAdmin) Then
               'get Details
               Call prGetRoleCount()
            End If

            If Not Page.IsPostBack Then
               'Bind Data grid
                    'BindBody(body)
               Call prBindGrid()
            End If

         Catch ex As Exception

         Finally

            clsGeneric = Nothing

         End Try

    End Sub

#End Region

#Region "Get Role Count"


      '****************************************************************************************************
      'Function Name  : fnGetRoleCount()
      'Purpose        : Call the function to get the role count
      'Arguments      : Requested Role
      'Return Value   : Role Count
      'Author         : Sujith Sharatchandran - 
      'Created        : 23/10/2003
      '*****************************************************************************************************
      Sub prGetRoleCount()

         Dim clsUsers As New MaxPayroll.clsUsers      'Create Customer Class Object

         Dim drInfo As SqlDataReader

         Try

            lblAUploader.Text = "0"
            lblCUploader.Text = "0"
            lblDUploader.Text = "0"

            lblAReviewer.Text = "0"
            lblCReviewer.Text = "0"
            lblDReviewer.Text = "0"

            lblAAuthor.Text = "0"
            lblCAuthor.Text = "0"
            lblDAuthor.Text = "0"

            lblAInterceptor.Text = "0"
            lblCInterceptor.Text = "0"
                lblDInterceptor.Text = "0"

            drInfo = clsUsers.fnRoleTotalCount(ss_lngOrgID, ss_lngUserID)
            While drInfo.Read
               Select Case CStr(drInfo("User_Status"))
                  Case clsCommon.fncGetPrefix(enmStatus.A_Active)
                     Select Case CStr(drInfo("User_Flag"))
                        Case gc_UT_Auth
                           lblAAuthor.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_Uploader
                           lblAUploader.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_Reviewer
                           lblAReviewer.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_ReportDownloader
                                    lblAInterceptor.Text = CStr(drInfo("RoleCount"))
                            End Select
                  Case clsCommon.fncGetPrefix(enmStatus.C_Cancel)
                     Select Case CStr(drInfo("User_Flag"))
                        Case gc_UT_Auth
                           lblCAuthor.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_Uploader
                           lblCUploader.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_Reviewer
                           lblCReviewer.Text = CStr(drInfo("RoleCount"))
                                Case gc_UT_ReportDownloader
                                    lblCInterceptor.Text = CStr(drInfo("RoleCount"))
                            End Select
                  Case clsCommon.fncGetPrefix(enmStatus.D_Delete)
                     Select Case CStr(drInfo("User_Flag"))
                        Case gc_UT_Auth
                           lblDAuthor.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_Uploader
                           lblDUploader.Text = CStr(drInfo("RoleCount"))
                        Case gc_UT_Reviewer
                           lblDReviewer.Text = CStr(drInfo("RoleCount"))
                                Case gc_UT_ReportDownloader
                                    lblDInterceptor.Text = CStr(drInfo("RoleCount"))
                            End Select
               End Select
            End While
            drInfo.Close()
            drInfo = Nothing
         Catch ex As Exception

         Finally

            'Destroy Customer Class Object
            clsUsers = Nothing

         End Try



      End Sub

#End Region

#Region "Bind Data Grid"

      '****************************************************************************************************
      'Function Name  : prBindGrid()
      'Purpose        : Bind The User Data Grid
      'Arguments      : N/A
      'Return Value   : Data Grid
      'Author         : Sujith Sharatchandran - 
      'Created        : 25/10/2003
      '*****************************************************************************************************
      Sub prBindGrid()

         'Create Instance of User Class Object
         Dim clsUsers As New MaxPayroll.clsUsers

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance System Data Set Object
         Dim dsUser As New System.Data.DataSet

         'Variable Declarations
         Dim intRecordCount As Int32


         Try



            dsUser = clsUsers.UserGrid()
            intRecordCount = dsUser.Tables("USER").Rows.Count
            If intRecordCount > 0 Then
               trMsg.Visible = False
               trGrid.Visible = True

               dgUsers.DataSource = dsUser
               dgUsers.DataBind()
               Me.fncGeneralGridTheme(dgUsers)
            Else
               trMsg.Visible = True
               trGrid.Visible = False
               lblMessage.Text = "No Roles Found/Matching."
            End If

         Catch
            LogError("PG_ViewRoles - prBindGrid")


         Finally

            'Destroy Instance of Data Set
            dsUser = Nothing

            'Destroy Instance of User Class Object
            clsUsers = Nothing

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

         End Try

      End Sub

      '****************************************************************************************************
      'Function Name  : dgUsers_ItemDataBound()
      'Purpose        : Hide the Modify Authentication Code Link when the Role type is Authorizer with Token Setting is True.
      'Handles        : dgUsers.ItemDataBound
      'Arguments      : Sender, DataGridItemEventArgs
      'Return Value   : N/A
      'Author         : Victor Wong - 
      'Created        : 11/Jan/2007
      '*****************************************************************************************************
      Private Sub dgUsers_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgUsers.ItemDataBound
         Select Case e.Item.ItemType
            Case ListItemType.Item, ListItemType.AlternatingItem
               If (CBool(Session(gc_Ses_Token)) = True AndAlso e.Item.Cells(9).Text = gc_UT_Auth) Or e.Item.Cells(9).Text = gc_UT_BankOperator Or e.Item.Cells(9).Text = gc_UT_InquiryUser Then
                  Dim lblChgAuth As Label
                  lblChgAuth = CType(e.Item.FindControl("lblChgAuth"), Label)
                  lblChgAuth.Text = ""
               End If


         End Select
      End Sub

#End Region

#Region "Data Grid Navigation"

        '****************************************************************************************************
        'Function Name  : prPageChange()
        'Purpose        : Navigation for Data Grid
        'Arguments      : System Objects,System EventArgs
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try
                intStart = dgUsers.CurrentPageIndex * dgUsers.PageSize
                dgUsers.CurrentPageIndex = E.NewPageIndex
                prBindGrid()
            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Show All"

    Private Sub prShow(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btnShow.Click
        Try
            Server.Transfer("PG_ViewRoles.aspx", False)
        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Create Role"

    Private Sub prNew(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        Try
            Server.Transfer("PG_CreateRole.aspx", False)
        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Clear Screen"

    Private Sub prClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click

        cmbOption.SelectedValue = ""
        cmbCriteria.SelectedValue = ""
        txtKeyword.Text = ""

    End Sub

#End Region

#Region "Search"

    Private Sub prSearch(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            dgUsers.CurrentPageIndex = 0
            Call prBindGrid()
        Catch ex As Exception

        End Try
        

    End Sub

#End Region


        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function
    End Class

End Namespace
