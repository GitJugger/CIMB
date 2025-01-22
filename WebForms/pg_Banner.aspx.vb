Namespace MaxPayroll

   Partial Class pg_Banner
      Inherits Web.UI.Page


      Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         'If IsNothing(Session(gc_Ses_UserType)) = False Then
         '   Dim clsCommon As New clsCommon
         '   lblMsg.Text = "You have logged in as " & clsCommon.fncGetUserTypeDesc(Session(gc_Ses_UserType))

         '   lblSignOut.Text = "<a href='PG_Logout.aspx' target='Main'>Sign Out</a>"
         'Else
         '   lblMsg.Text = String.Empty
         '   lblSignOut.Text = String.Empty
         'End If


      End Sub
   End Class
End Namespace