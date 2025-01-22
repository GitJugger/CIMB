Namespace MaxPayroll

Partial Class PG_Message
        Inherits clsBasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub
    Protected WithEvents Label1 As System.Web.UI.WebControls.Label

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Variable Declarations
        Dim strMsgTyp As String, strErrMessage As String

        Try

            If Not Page.IsPostBack Then

                'Get Message Type To Be Displayed
                strMsgTyp = UCase(Request.QueryString("MsgTyp"))
                strErrMessage = Request.QueryString("ErrMsg")

                Select Case strMsgTyp
                    Case "PWD"
                        btnLog.Visible = True
                        btnCont.Visible = False
                            lblMsg.Text = "The Password changed successfully. Kindly SignOn again with the New Password."
                    Case "LOGIN"
                        btnLog.Visible = False
                        btnCont.Visible = True
                        lblMsg.Text = strErrMessage
                End Select

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Click Methods"

    Public Sub prContinue(ByVal O As System.Object, ByVal E As EventArgs) Handles btnCont.Click
        Try
                Server.Transfer("PG_Index.aspx?ErrFlag=Y", False)
            'Response.Write("<script language='JavaScript'>" & vbCrLf)
            'Response.Write("window.location.href = 'PG_Index.aspx?ErrFlag=Y';" & vbCrLf)
            'Response.Write("</script>" & vbCrLf)
            Exit Try
        Catch ex As Exception

        End Try
        
    End Sub

    Public Sub prLogOut(ByVal O As System.Object, ByVal E As EventArgs) Handles btnLog.Click

        Try
                Server.Transfer("PG_Logout.aspx?Timed=", False)
            'Response.Write("<script language='JavaScript'>")
            'Response.Write("window.location.href = 'PG_Logout.aspx?Timed=True';")
            'Response.Write("</script>")
            Exit Try
        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class

End Namespace
