Namespace MaxPayroll

Partial Class PG_Logout
    Inherits System.Web.UI.Page

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub


    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'create instance of user class object
        Dim clsUsers As New MaxPayroll.clsUsers

            'Variable Declarations
            Dim lngOrgId As Long, intUserid As Int32, lngLogId As Long
            'Dim SessionID As String


            Try
                If Page.IsPostBack = False Then
               'Me.lblLogout.Text = "Thank you for using " & MaxPayroll.mdConstant.gc_Const_CompanyName & " Business Banking" & gc_BR & gc_BR & "To SignOn again, please click <a href='PG_Index.aspx'>here</a>." & gc_BR & gc_BR & "Please remember to clear your browser's cache before you close your Internet browser."
                    'Me.lblLogout02.Text = lblLogout.Text
                End If
                'Write Access Log
                lngOrgId = Session("SYS_ORGID")
                intUserid = Session("SYS_USERID")
                lngLogId = Session("LOG_ID")
                'SessionID = Session.SessionID
                'Update Access log
                Call clsGeneric.fnWriteLog(lngLogId)

                'Delete Session Check
                Call clsGeneric.Logoff(lngOrgId, intUserid)

                'Put user code to initialize the page here
                Session.Clear()
                Session.Abandon()
                Session.RemoveAll()

                'Load the Menu
                'Response.Write("<script language='JavaScript'>")
                'Response.Write("parent.frames['Menu'].location.href = 'PG_Menu.aspx';parent.frames['banner'].location.href = 'pg_Banner.aspx';")
                'Response.Write("</script>")

            Catch ex As Exception

            Finally

                'destroy instance of user class object
                clsUsers = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing
            End Try

        End Sub

#End Region


    Dim objResxMgr As New MaxPayroll.SatelliteResx.ResourceManagerEx(Me)

    Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            '    Me.objResxMgr.TextBind(Me.lblLogout, "longh01")
            'Me.objResxMgr.TextBind(Me.lblLogout02, "longh01")

            'objResxMgr.TextBind()

    End Sub

End Class

End Namespace
