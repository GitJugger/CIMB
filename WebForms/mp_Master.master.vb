Imports MaxPayroll

Partial Class WebForms_Master_mp_Master
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oMenu As New clsMenu
        'hidSubmenu.Value = Request.QueryString("subMenu") & ""
        hidOpenMenu.Value = Request.QueryString("subMenu") & ""
        hidSubmenu.Value = ""
        lblMenu1.Text = oMenu.fncGenerateMenu(hidSubmenu.Value)

        'lblMenu1.Text = oMenu.fncGenerateMenu("")

        If Not Session(gc_Ses_OrgId) Is Nothing Then
            body.Attributes.Add("onmousemove", "resetCounter();")
            body.Attributes.Add("onchange", "resetCounter()")
            body.Attributes.Add("onload", "javascript:countDown();onloadfunction();")
        Else
            body.Attributes.Add("onload", "javascript:onloadfunction();")
        End If
    End Sub
End Class

