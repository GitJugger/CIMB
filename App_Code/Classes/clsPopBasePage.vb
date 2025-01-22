Imports Microsoft.VisualBasic
Namespace MaxPayroll


   Public Class clsPopBasePage
      Inherits clsBaseGeneric

#Region "   Logout"
      Public Sub prcLogout()
         Server.Transfer(gc_LogoutPath, False)
      End Sub
#End Region

#Region "   Protected Overridable Function"

      Protected Overridable Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
         Session.LCID = 2057
         prcCacheControl()
         If Not fncIsSessionExpired() Then
            Response.Write("<script language='javascript'>window.returnValue=true;self.close();</script>")
         End If

         'onload="countDown();fillColor('cHeader');"  onmousemove="resetCounter()" onclick="resetCounter()" style="margin: 0px 0px 0px 0px"
      End Sub

#End Region
   End Class

End Namespace