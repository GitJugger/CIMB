Imports System.Web
Imports Microsoft.VisualBasic

Public Class Common

#Region "Global Variables "

    Private _Helper As New Helper()
    Private _clsCommon As New MaxPayroll.clsCommon()

#End Region

#Region "Get File Type Identity "

    Public Function GetFileTypeIdentity(ByVal FileType As String) As String


        Dim Identity As String = Nothing, OrganizationId As Integer = 0

        OrganizationId = IIf(IsNumeric(HttpContext.Current.Session("SYS_ORGID")), HttpContext.Current.Session("SYS_ORGID"), 0)

        'Get File Type Identification - START
        If FileType = "Payroll File" Then
            'Check If Privilege Customer
            Identity = _clsCommon.fncBuildContent("Privilege", "", OrganizationId, 0)                                               'Get Privilege User
        ElseIf FileType = "EPF File" Or FileType = "EPF Test File" Then
            Identity = "E"
        ElseIf FileType = "SOCSO File" Then
            Identity = "S"
        ElseIf FileType = "LHDN File" Then
            Identity = "L"
        ElseIf FileType = _Helper.DirectDebit_Name Then
            Identity = "D"
        ElseIf FileType = _Helper.Autopay_Name OrElse FileType = _Helper.AutopaySNA_Name Then
            Identity = _clsCommon.fncBuildContent("Privilege", "", OrganizationId, 0)
        ElseIf FileType = _Helper.CPS_Name Then
            Identity = "C"
        ElseIf FileType = _Helper.PayLinkPayRoll_Name Then
            Identity = "I"
        End If
        'Get File Type Identification - STOP

        Return Identity

    End Function

#End Region

#Region "Java Script Alert "

    Public Sub JavaScriptAlert(ByVal Message As String)

        'Display Alert - Start
        HttpContext.Current.Response.Write("<script language='JavaScript'>")
        HttpContext.Current.Response.Write("alert('" & Message & "');")
        HttpContext.Current.Response.Write("</script>")
        'Display Alert - Stop

    End Sub

#End Region

End Class
