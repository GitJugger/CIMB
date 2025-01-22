'This tutorial is provided in part by Server Intellect Web Hosting Solutions http://www.serverintellect.com

'Visit http://www.AspNetTutorials.com for more ASP.NET Tutorials
Namespace MaxPayroll

   Partial Class progress
      Inherits System.Web.UI.Page
      Private state As Integer = 0
      Private Property PopOutState() As enmPIType
         Get
            Try
               Return Session(gc_Ses_PopOut)
            Catch ex As Exception
               Return Nothing
            End Try
         End Get
         Set(ByVal value As enmPIType)
            Session(gc_Ses_PopOut) = value
         End Set
      End Property

      Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         If IsNothing(PopOutState) OrElse PopOutState = enmPIType.Open Then
            PopOutState = enmPIType.Waiting
         End If
         If PopOutState <> enmPIType.Close Then

            Page.RegisterStartupScript("", "<script language='javascript'>window.setTimeout('window.Form1.submit()',3000);</script>")
         Else
            Page.RegisterStartupScript("", "<script language='javascript'>window.close();</script>")
            PopOutState = enmPIType.Open
         End If
      End Sub
   End Class
End Namespace
