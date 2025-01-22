Imports System.Web
Imports System.Web.Security
Imports System.Web.SessionState
Imports System.Security.Principal


Namespace MaxPayroll


Public Class [Global]
    Inherits System.Web.HttpApplication

#Region " Component Designer Generated Code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Required by the Component Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Component Designer
    'It can be modified using the Component Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
    End Sub

#End Region

    Public Const Key_DisplayLanguage As String = "DisplayLanguage"

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
            ' Fires when the session is started
            If (System.Web.Security.FormsAuthentication.RequireSSL And Request.IsSecureConnection) Then
                Response.Cookies("ASP.NET_SessionId").Secure = True
                Response.Cookies("AuthToken").Secure = True
                Response.Cookies("__AntiXsrfToken").Secure = True
                Response.Cookies("subMenu").Secure = True
            End If
            'Session.Timeout = 10

        End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        If (Request.Path.IndexOf(Chr(92)) >= 0 Or _
            System.IO.Path.GetFullPath(Request.PhysicalPath) <> Request.PhysicalPath) Then
            Throw New HttpException(404, "Not Found")
        End If

        If (Request.Cookies(Key_DisplayLanguage) Is Nothing) Then
            Response.Cookies(Key_DisplayLanguage).Value = System.Globalization.CultureInfo.CurrentUICulture.Name
        Else

            Dim strDLang As String
            strDLang = Request.Cookies(Key_DisplayLanguage).Value
            If (strDLang <> System.Globalization.CultureInfo.CurrentUICulture.Name) Then
                System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CurrentUICulture.CreateSpecificCulture(strDLang)
            End If

        End If

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use

        If Not (HttpContext.Current.User Is Nothing) Then
            If HttpContext.Current.User.Identity.IsAuthenticated Then
                If TypeOf HttpContext.Current.User.Identity Is FormsIdentity Then
                    Dim fi As FormsIdentity = CType(HttpContext.Current.User.Identity, FormsIdentity)
                    Dim fat As FormsAuthenticationTicket = fi.Ticket

                    ' Get the stored user-data, in this case, our roles
                    Dim userData As String = fat.UserData
                    Dim roles() As String = userData.Split(",")
                    HttpContext.Current.User = New GenericPrincipal(fi, roles)
                End If
            End If
        End If
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        Dim intUserId As Int32 = Session("SYS_USERID")      'Get User Id
        If intUserId > 0 Then
            Dim clsUsers As New MaxPayroll.clsUsers         'create instance of user class object
            Call clsUsers.fncSessionCheck("D", intUserId)   'Delete Session Check
            clsUsers = Nothing
        End If
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class

End Namespace
