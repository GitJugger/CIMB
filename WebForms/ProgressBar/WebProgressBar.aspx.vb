'This tutorial is provided in part by Server Intellect Web Hosting Solutions http://www.serverintellect.com

'Visit http://www.AspNetTutorials.com for more ASP.NET Tutorials

Imports System.Web.SessionState
Imports System.Text
Imports System.Threading
Partial Class WebProgressBar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    'simulate tase
    'each simulation tase  progress to different state
    Private Sub LongTask()

        Dim i As Integer
        For i = 0 To 10
            Thread.Sleep(1000)
            Session("State") = i + 1
        Next
        Session("State") = 100
    End Sub
    Public Shared Sub OpenProgressBar(ByVal Page As System.Web.UI.Page)
        Dim sbScript As New StringBuilder()

        sbScript.Append("<script language='JavaScript' type='text/javascript'>" + ControlChars.Lf)
        sbScript.Append("<!--" + ControlChars.Lf)
        sbScript.Append("window.showModalDialog('Progress.aspx','','dialogHeight: 100px; dialogWidth: 350px; edge: Raised; center: Yes; help: No; resizable: No; status: No;scroll:No;');" + ControlChars.Lf)
        sbScript.Append("// -->" + ControlChars.Lf)
        sbScript.Append("</script>" + ControlChars.Lf)
        Page.RegisterClientScriptBlock("OpenProgressBar", sbScript.ToString())
    End Sub
    Public Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim thread As New Thread(New ThreadStart(AddressOf LongTask))

        thread.Start()
        Session("State") = 1
        OpenProgressBar(Me.Page)
    End Sub
End Class
