Imports Microsoft.VisualBasic
Imports MaxPayroll.mdConstant
Imports Microsoft.ApplicationBlocks.Data

Namespace MaxPayroll

    Public Class clsBaseGeneric
        Inherits System.Web.UI.Page
        Dim objResxMgr As New MaxPayroll.SatelliteResx.ResourceManagerEx

#Region "Grid"
        Public Sub fncGeneralGridTheme(ByRef dg As DataGrid)
            If dg.PageCount < 2 Then
                dg.PagerStyle.Visible = False
            Else
                dg.PagerStyle.Visible = True
            End If
        End Sub
#End Region

        Private Sub Page_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.PreRender

            objResxMgr.TextBind(Me)

        End Sub

        Public Function fncBindText(ByVal sTargetText As String) As String
            'Return objResxMgr.TextBind(sTargetText)
            Return sTargetText
        End Function

#Region "   Check Session Timeout"

        Public Function fncIsSessionExpired() As Boolean

            Dim bRetVal As Boolean = False
            Try
                If ss_lngOrgID > 0 AndAlso ss_lngUserID > 0 Then
                    bRetVal = True
                End If
            Catch ex As Exception

            End Try
            Return bRetVal
            'Return True

        End Function

        Public Sub fncKillPopOut()
            If IsNothing(Session(gc_Ses_PopOut)) = False AndAlso Session(gc_Ses_PopOut) = enmPIType.Waiting Then
                Session(gc_Ses_PopOut) = enmPIType.Close
            End If
        End Sub

#End Region

#Region "Get Company Name"
        Public Function fncGetCompanyName() As String
            Dim strSQL As String
            strSQL = "Select top 1 Org_Name From Org_Master Where Org_Id = " & ss_lngOrgID.ToString
            Try
                Return SqlHelper.ExecuteScalar(Generic.sSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                Return ""
            End Try
        End Function

#End Region

        Public ReadOnly Property sConnectionString() As String
            Get
                Dim clsGeneric As New Generic
                Return clsGeneric.strSQLConnection
            End Get
        End Property

#Region "   Log Error"
        Public Sub LogError(ByVal strProblemSource As String)
            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Log Error
            Call clsGeneric.ErrorLog(ss_lngOrgID, ss_lngUserID, strProblemSource, Err.Number, Err.Description)

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing
        End Sub
        Public Sub LogError(ByVal lngOrgID As Long, ByVal strProblemSource As String)
            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Log Error
            Call clsGeneric.ErrorLog(lngOrgID, ss_lngUserID, strProblemSource, Err.Number, Err.Description)

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing
        End Sub
#End Region

        Public Function fncAppSettings(ByVal sConfigName As String) As String
            Return System.Configuration.ConfigurationManager.AppSettings(sConfigName) & ""
        End Function

#Region "Session "

        Public ReadOnly Property ss_lngOrgID() As Long
            Get
                If IsNothing(Session(gc_Ses_OrgId)) = False AndAlso IsNumeric(Session(gc_Ses_OrgId)) Then
                    Return Session(gc_Ses_OrgId)
                Else
                    Return 0
                End If
            End Get
        End Property
        Public ReadOnly Property ss_lngUserID() As Long
            Get
                If IsNumeric(Session(gc_Ses_UserID)) Then
                    Return Session(gc_Ses_UserID)
                Else
                    Return 0
                End If
            End Get
        End Property
        Public ReadOnly Property ss_lngGroupID() As Long
            Get
                If IsNothing(Session(gc_Ses_GroupID)) = False AndAlso IsNumeric(Session(gc_Ses_GroupID)) Then
                    Return Session(gc_Ses_GroupID)
                Else
                    Return 0
                End If
            End Get
        End Property
        Public ReadOnly Property ss_strGroupName() As String
            Get
                Return Session(gc_Ses_GroupName) & ""
            End Get
        End Property
        Public ReadOnly Property ss_strUserType() As String
            Get
                Return Session(gc_Ses_UserType) & ""
            End Get
        End Property
        Public ReadOnly Property ss_PwdChgStatus() As String
            Get
                Return Session(gc_Ses_PwdChgStatus) & ""
            End Get
        End Property
        Public ReadOnly Property ss_AuthChgStatus() As String
            Get
                Return Session(gc_Ses_AuthChgStatus) & ""
            End Get
        End Property
#End Region

#Region "   Protected Overridable Function"

        Public Sub BindBody(ByRef body As System.Web.UI.HtmlControls.HtmlGenericControl, Optional ByVal bBindResetScript As Boolean = True, Optional ByVal bFillHeaderColour As Boolean = True)
            If bFillHeaderColour = True AndAlso bBindResetScript = True Then
                body.Attributes.Add("onload", "countDown();fillColor('cHeader');")
            ElseIf bFillHeaderColour = False AndAlso bBindResetScript = True Then
                body.Attributes.Add("onload", "countDown();")
            ElseIf bFillHeaderColour = True AndAlso bBindResetScript = False Then
                body.Attributes.Add("onload", "fillColor('cHeader');")
            End If
            If bBindResetScript Then
                body.Attributes.Add("onmousemove", "resetCounter();")
                body.Attributes.Add("onchange", "resetCounter()")

            End If
            body.Attributes.Add("style", "margin: 0px 0px 0px 0px")
        End Sub

        Public Sub prcCacheControl()
            'Response.CacheControl = "no-cache"
            'Response.AddHeader("Pragma", "no-cache")
            'Response.Expires = -1
        End Sub
#End Region

    End Class

End Namespace