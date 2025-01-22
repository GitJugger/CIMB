Namespace MaxPayroll

   Partial Class PG_Lang
        Inherits clsBasePage

#Region " Web Form Designer Generated Code "

      'This call is required by the Web Form Designer.
      <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

      End Sub
      Protected WithEvents Panel1 As System.Web.UI.WebControls.Panel
      Protected WithEvents Label1 As System.Web.UI.WebControls.Label


      Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
         'CODEGEN: This method call is required by the Web Form Designer
         'Do not modify it using the code editor.
         InitializeComponent()
      End Sub

#End Region

      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         Dim objUIC As System.Globalization.CultureInfo
         Dim objLItm As System.Web.UI.WebControls.ListItem

         If (Not Me.IsPostBack) Then
            Dim clsbase As New clsBasePage
            clsbase.BindBody(body, False, True)
            clsbase = Nothing
                objUIC = System.Threading.Thread.CurrentThread.CurrentUICulture
                rbtnlLang.Items(0).Selected = True
                For Each objLItm In Me.rbtnlLang.Items
                    If (objLItm.Value = objUIC.Name) Then
                        objLItm.Selected = True
                        Exit For
                    End If
                Next

                objLItm = Nothing
                objUIC = Nothing
            End If

      End Sub

      Private Sub btnChg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChg.Click

         Dim strLang As String
         Dim strS As String

         strLang = Me.rbtnlLang.SelectedValue

         Response.Cookies([Global].Key_DisplayLanguage).Value = strLang

         System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.CreateSpecificCulture(strLang)

         strS = "<script lang=""javascript"">" & _
             "var objP = window.parent;" & _
             "objP.navigate(objP.document.URL);" & _
             "</script>"

         Me.RegisterStartupScript("Startup_ReloadParent", strS)

      End Sub

      Dim objResxMgr As New MaxPayroll.SatelliteResx.ResourceManagerEx(Me)

      Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            'objResxMgr.TextBind()

      End Sub

   End Class

End Namespace
