
Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsCommon
Imports MaxPayroll.clsCustomer


Namespace MaxPayroll


Partial Class PG_GroupChange
      Inherits clsBasePage

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Generic Class
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of User Class Object
        Dim clsUser As New MaxPayroll.clsUsers

        'Create Instance of Common Class Object
        Dim clsCommon As New MaxPayroll.clsCommon

        'Create Instance of Customer Class Object
        Dim clsCustomer As New MaxPayroll.clsCustomer

        'Create Instance of Data Set
        Dim dsCustomer As New System.Data.DataSet

        'Variable Declarations
         Dim strRequest As String
         Dim strUserName As String, strSysType As String, intCount As Int16, lngLogID As Long, lngGroupID As Long

        Try
            'Get User Type
            strSysType = ss_strUserType
            strRequest = Request.QueryString("Mode")

            'Check If Any other User Than Uploader, Reviewer, Authorizer & Interceptor
                If Not ss_strUserType.Equals(gc_UT_Uploader) And Not ss_strUserType.Equals(gc_UT_Reviewer) And Not ss_strUserType.Equals(gc_UT_Auth) And Not ss_strUserType.Equals(gc_UT_ReportDownloader) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

            If Not IsPostBack Then
                    'BindBody(body)
               'strUserName = clsCommon.fncBuildContent("User Name", "", ss_lngUserID, ss_lngUserID)   'Get User Name

               lblUserName.Text = "WELCOME " & CStr(Session("SYS_USERNAME")).ToUpper & ","

                'Get User Groups
               dsCustomer = clsCustomer.fncListGroup("USER GROUP", ss_lngUserID, ss_lngUserID, "0")

                'Get Record Count
                intCount = dsCustomer.Tables(0).Rows.Count

                'Populate Drop Down List - START
                If intCount > 0 Then
                    With ddlGroupList
                        .DataSource = dsCustomer
                        .DataTextField = "GNAME"
                        .DataValueField = "GID"
                        .DataBind()
                        .Visible = True
                    End With
                Else
                    tblForm1.Visible = False
                    tblForm2.Visible = False
                  lblMessage.Text = "No groups available. Please contact your " & gc_UT_SysAdminDesc & "."
                  btnSignOut.Visible = True
                  Call clsUser.fncSessionCheck("D", ss_lngUserID)
                End If
                'Populate Drop Down List - STOP

            End If

            If intCount = 1 Then

                'Get Group Id
               ' strGroupName = ddlGroupList.SelectedItem.Text
               ' Session("SYS_GNAME") = strGroupName
               lngGroupId = IIf(IsNumeric(ddlGroupList.SelectedValue), ddlGroupList.SelectedValue, 0)
               Session(gc_Ses_GroupName) = ddlGroupList.SelectedItem.Text



                'Check if Logged In Group and Selected Group is Same
               If Not ss_lngGroupID = lngGroupID Then
                  lngLogID = clsGeneric.fnWriteLog(0, lngGroupID)
                  Session("LOG_ID") = lngLogID
                  Session(gc_Ses_GroupID) = lngGroupID
               End If

                'Check if Force Change Password - Start
                If Session("PWD_CHNG") = "Y" Or Session("AUTH_CHNG") = "Y" Then
                  Session(gc_Ses_GroupName) = ddlGroupList.SelectedItem.Text
                    Server.Transfer("PG_Inbox.aspx", False)
                    Exit Try
                End If
                'Check if Force Change Password - Stop

                If Not strRequest = "" Then
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('Sorry! You belong to only one group.');")
                    Response.Write("document.location.href = 'PG_Inbox.aspx'")
                    Response.Write("</script>")
                Else
                    Server.Transfer("PG_Inbox.aspx", False)
                    Exit Try
                End If

            ElseIf intCount > 1 Then

                'Check if Force Change Password - Start
                If Session("PWD_CHNG") = "Y" Or Session("AUTH_CHNG") = "Y" Then
                  Session(gc_Ses_GroupName) = ddlGroupList.SelectedItem.Text
                    Server.Transfer("PG_Inbox.aspx", False)
                    Exit Try
                End If
                'Check if Force Change Password - Stop

            End If

         Catch ex As Exception


            'Log Error
                If Err.Description <> "Thread was being aborted." Then
                    LogError("PG_GroupChange.aspx.vb - Page Load")
                End If

        Finally

            'Destroy Instance of Generic class Object
            clsGeneric = Nothing

            'Destroy Instance of Common Class Object
            clsCommon = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

            'Destroy Instance of System Data Set
            dsCustomer = Nothing

            'destroy instance of User Class Object
            clsUser = Nothing

        End Try

    End Sub

#End Region

#Region "Proceed"

      Private Sub prcProceed(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnProceed.Click

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Variable Declarations
         Dim lngOrgId As Long, lngUserId As Long
         Dim lngGroupId As Long, lngLogId As Long, lngSesGrpId As Long, strGroupName As String

         'Check to verify that something has been selected
         If ddlGroupList.SelectedIndex <> -1 Then

            Try
               'Get Organisation Id
               lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)
               'Get User Id
               lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
               'Get Session Group Id
               lngSesGrpId = IIf(IsNumeric(Session("SYS_GROUPID")), Session("SYS_GROUPID"), 0)
               'Get Selected Group Id
               lngGroupId = IIf(IsNumeric(ddlGroupList.SelectedValue), ddlGroupList.SelectedValue, 0)
               'Get Group Name
               strGroupName = ddlGroupList.SelectedItem.Text

               'Check If Session group Id greater than Zero
               If lngSesGrpId > 0 Then
                  If Not lngGroupId = lngSesGrpId Then
                     lngLogId = IIf(IsNumeric(Session("LOG_ID")), Session("LOG_ID"), 0)
                     lngLogId = clsGeneric.fnWriteLog(lngLogId)
                     lngLogId = clsGeneric.fnWriteLog(0, lngGroupId)
                     Session("LOG_ID") = lngLogId
                     Session(gc_Ses_GroupID) = lngGroupId
                     strGroupName = ddlGroupList.SelectedItem.Text
                     Session(gc_Ses_GroupName) = strGroupName
                  End If
               Else
                  Session(gc_Ses_GroupName) = strGroupName
                  Session(gc_Ses_GroupID) = lngGroupId
               End If

               Server.Transfer("PG_Inbox.aspx", False)
               Exit Try

                Catch ex As Exception

                    'Log Error
                    If Not ex.Message = "Thread was being aborted." Then
                        Call LogError("PG_GroupChange.aspx.vb - prcProceed")
                    End If

            Finally

               'Destroy Instance of Generic Class Object
                    clsGeneric = Nothing

            End Try

         End If

      End Sub

#End Region

      Protected Sub btnSignOut_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSignOut.Click
         Server.Transfer(gc_LogoutPath, False)
      End Sub
   End Class

End Namespace
