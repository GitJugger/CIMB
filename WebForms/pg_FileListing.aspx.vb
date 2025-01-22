Imports MaxGeneric
Imports MaxMiddleware
Imports MaxPayroll

Partial Class WebForms_pg_FileListing
    Inherits clsBasePage

#Region "Global Declarations "

    Private _Helper As New Helper
    Private FileType As String = "FT"
    Private FileTypeId As String = "FTI"
    Private MatchFileTypeId As String = "MFTI"
    Dim lngOrgId As Long, lngUserId As Long

#End Region

#Region "Page Load "

    'Author     : Bhanu Teja
    'Purpose    : Page load Actions
    'Created    : 23/10/2008
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id
            'Query String Valus
            Call GetQueryString()

            'Populate Data Grid
            Call PopulateDataGrid()

            Call _Helper.SetMatchFileType(_Helper.SessionFileType, _
                _Helper.SessionFileTypeId)
            If _Helper.SessionFileType = PPS.FileType.Inward Then
                btnBody.Visible = True
                btnHeader.Visible = True
                btnFooter.Visible = True
                btnBodyTrailer.Visible = True
            Else
                btnBody.Visible = False
                btnHeader.Visible = False
                btnFooter.Visible = False
                btnBodyTrailer.Visible = False
            End If

        Catch ex As Exception

            'log error
            lblMessage.Text = ex.Message
            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_FileListing", Err.Number, Err.Description)

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Get Query Strings "

    Private Sub GetQueryString()

        Try

            'Get File Type Id
            _Helper.SessionFileTypeId = clsGeneric.NullToShort( _
                    Request.QueryString(FileTypeId))

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Populate Data Grid "

    'Author     : Bhanu Teja
    'Purpose    : Fill the Grid
    'Created    : 23/10/2008
    Private Sub PopulateDataGrid()

        Try

            'Populate Data Grid - Start
            Call FormHelp.PopulateDataGrid(_Helper.GetFileSettings( _
                _Helper.SessionFileType, _Helper.SessionFileTypeId, 0), dgFileListing)
            'Populate Data Grid - Stop

        Catch ex As Exception
            lblMessage.Text = ex.Message
            ' Error Logs Starts Here
            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "PopulateDataGrid - PG_FileListing", Err.Number, Err.Description)
        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Create New "

    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click

        Try

            Server.Transfer("pg_FileSettings.aspx?FTA=1", True)

        Catch ex As Exception
            'ErrorLog
            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnCreate_Click - PG_FileListing", Err.Number, Err.Description)
        End Try

    End Sub
#End Region

#Region "Create Header "

    Protected Sub btnHeader_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHeader.Click

        Try

            Call TableWizard(_Helper.SessionFileTypeId, PPS.ContentType.Header, PPS.ContentType.Header.ToString())

        Catch ex As Exception

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnHeader_Click - PG_FileListing", Err.Number, Err.Description)

        End Try

    End Sub
#End Region

#Region "Create Body "

    Protected Sub btnBody_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBody.Click

        Try

            Call TableWizard(_Helper.SessionFileTypeId, PPS.ContentType.Body, PPS.ContentType.Body.ToString())

        Catch ex As Exception

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnBody_Click - PG_FileListing", Err.Number, Err.Description)

        End Try

    End Sub
#End Region

#Region "Create BodyTrailer "

    Protected Sub btnBodyTrailer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBodyTrailer.Click

        Try

            Call TableWizard(_Helper.SessionFileTypeId, PPS.ContentType.Body_Trailer, PPS.ContentType.Body_Trailer.ToString())

        Catch ex As Exception

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnBody_Click - PG_FileListing", Err.Number, Err.Description)

        End Try

    End Sub
#End Region

#Region "Create Footer "

    Protected Sub btnFooter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFooter.Click

        Try

            Call TableWizard(_Helper.SessionFileTypeId, PPS.ContentType.Footer, PPS.ContentType.Footer.ToString())

        Catch ex As Exception

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnFooter_Click - PG_FileListing", Err.Number, Err.Description)

        End Try

    End Sub
#End Region

#Region "Header/Body/Footer Table "

    Private Sub TableWizard(ByVal FileTypeId As Short, _
        ByVal ContentType As Short, ByVal TableName As String)

        'Variable Declarations
        Dim SQLStatement As String = Nothing, CatchMessage As String = Nothing

        Try

            'build sqls statement
            SQLStatement = _Helper.TableWizardSQL(FileTypeId, ContentType)

            'execute sql statement
            Call PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                CatchMessage, SQLStatement)

            If Not MaxMiddleware.Helper.IsBlank(CatchMessage) Then
                Dim clsGeneric As New MaxPayroll.Generic
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "TableWizard- PG_FileListing", Err.Number, CatchMessage)
                If CatchMessage.Contains("duplicate") Then
                    lblMessage.Text = "Duplicate Content"
                Else
                    lblMessage.Text = "Invalid Content"
                End If
                'lblMessage.Text = CatchMessage
            Else
                lblMessage.Text = TableName & " Table Created/Modified Successfully."
            End If

        Catch ex As Exception

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "TableWizard- PG_FileListing", Err.Number, Err.Description)

        End Try

    End Sub

#End Region

End Class
