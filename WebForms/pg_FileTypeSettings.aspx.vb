Imports System.IO
Imports MaxGeneric
Imports MaxMiddleware
Imports System.Configuration.ConfigurationManager
Imports MaxPayroll

Partial Class WebForms_pg_FileTypeSettings
    Inherits clsBasePage

#Region "Global Declarations "

    Private _Helper As New Helper
    Private _FileTypeId As String = "FTI"
    Private _FileTypeQuery As String = "FT"
    Private _FileTypeAction As String = "FTA"
    Dim lngOrgId As Long, lngUserId As Long
#End Region

#Region "Properties "

    'Author     : Bhanu Teja
    'Purpose    : Get the Action like insert or update etc; with filetype action from hidden variable on page
    'Created    : 23/10/2008
    Private Property FileTypeAction() As Short
        Get
            Return clsGeneric.NullToShort(_Helper.SessionFileTypeAction())
        End Get
        Set(ByVal value As Short)
            _Helper.SessionFileTypeAction = value
        End Set
    End Property
    Private ReadOnly Property GetFileTypeId() As Short
        Get
            Return __FileTypeId.Value
        End Get
    End Property

#End Region

#Region "Get Query Strings "

    Private Sub GetQueryString()

        Try

            'Get the File Type Action
            FileTypeAction = clsGeneric.NullToShort( _
                    Request.QueryString(_FileTypeAction))
            'Get the File Type - Stop

            __FileTypeId.Value = clsGeneric.NullToShort( _
                    Request.QueryString(_FileTypeId))

            'Call _Helper.SetMatchFileType(FileType, __FileType_Id.Value)

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Page Load "

    'Author     : Bhanu Teja
    'Purpose    : Page load Actions
    'Created    : 23/10/2008
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Create Instance of Data Table
        Dim FileTypeDetails As DataTable = Nothing
        Dim _ContentPlaceHolder As ContentPlaceHolder = Nothing

        'Variable Declarations
        Dim SQLStatement As String = Nothing

        Try
            'Query String Valus
            Call GetQueryString()

            If Not Page.IsPostBack Then

                'Populate to Status DDL
                Call PPS.EnumToDropDown(GetType(PPS.FileStatus), __FileType_Status, True)

                'Populate to Is Multiple Header DDL
                Call PPS.EnumToDropDown(GetType(PPS.Boolean), __Is_MultipleHeader, True)

                Call BindFiletype()

                'Populate Data Grid
                Call PopulateDataGrid()

                If GetFileTypeId > 0 Then

                    'Get Content Place Holder
                    _ContentPlaceHolder = Me.Form.FindControl("cphContent")

                    'Get FileType Details
                    SQLStatement = _Helper.SQLFileTypeSettings & "  0," & GetFileTypeId
                    FileTypeDetails = PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                            _Helper.GetSQLTransaction)

                    'Populate Data To Page
                    Call DataHelp.DataToPage(FileTypeDetails, _ContentPlaceHolder)

                End If

            End If

        Catch ex As Exception
            lblMessage.Text = ex.Message
            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_FileTypeSettings", Err.Number, Err.Description)
        End Try

    End Sub

#End Region

#Region "Bind FileType "

    Private Sub BindFiletype()

        Dim FileTypeTable As New DataTable

        Try
            FileTypeTable = PPS.GetData(_Helper.SQLGetFileType, _
                _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

            If FileTypeTable.Rows.Count > 0 Then

                __Filetype_Id.DataSource = FileTypeTable
                __Filetype_Id.DataTextField = _Helper.FileTypeCol
                __Filetype_Id.DataValueField = _Helper.FileTypeIdCol
                __Filetype_Id.DataBind()

            End If

        Catch ex As Exception

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
            FormHelp.PopulateDataGrid(PPS.GetData(_Helper.SQLFileTypeSettings & " 0,0", _
                _Helper.GetSQLConnection, _Helper.GetSQLTransaction), dgFileType)
            'Populate Data Grid - Stop

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Page Submit "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        'Variable Declarations
        Dim Result As Boolean = False, FileTypeId As Short = 0
        Dim SQLStatement As String = Nothing, CatchMessage As String = Nothing
        Dim InputParams As String() = Nothing, InputParamsValues As String() = Nothing

        Try

            Call GetSQLParamValue(InputParams, InputParamsValues)

            'Save Data - Start
            Result = PPS.SQLInsertUpdate(InputParams, InputParamsValues, _
                _Helper.SQLFileTypeSettings, _Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                    CatchMessage, True, FileTypeId)
            'Save Data - Stop

            If Not Result Then
                Dim clsGeneric As New MaxPayroll.Generic
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_FileTypeSettings", Err.Number, CatchMessage)
                If CatchMessage.Contains("duplicate") Then
                    lblMessage.Text = "Duplicate Content"
                Else
                    lblMessage.Text = "Invalid Content"
                End If
                'lblMessage.Text = CatchMessage
            Else
                lblMessage.Text = "Record has been inserted successfully"
                Call cleartext()
                Call PopulateDataGrid()
            End If

        Catch ex As Exception
            lblMessage.Text = CatchMessage
            ' Error Logs Starts Here
            Dim clsGeneric As New MaxPayroll.Generic
            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_FileTypeSettings", Err.Number, Err.Description)

        Finally

            'force garbage collection
            GC.Collect(0)

        End Try

    End Sub

#End Region

#Region "Get SQL Input Params/Value "

    Private Sub GetSQLParamValue(ByRef InputParams As String(),
            ByRef InputParamsValues As String())
        Try
            Dim clsCommon As New MaxPayroll.clsCommon
            'Get Input Param Values - Start
            InputParams = PPS.GetStringArray("Action", "FileTypeID", "NameFormat", "FileExt",
                "MultipleHeader", "BodyLine", "FileFolder", "Status")
            'Get Input Param Values - Stop
            Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(__FileType_Name.Text)
            If strEncUsername = False Then
                Response.Write(clsCommon.ErrorCodeScript())
                Exit Try
            End If
            Dim folder As Boolean = clsCommon.CheckScriptValidation(__FileType_Folder.Text)
            If folder = False Then
                Response.Write(clsCommon.ErrorCodeScript())
                Exit Try
            End If
            Dim errorfolder As Boolean = clsCommon.CheckScriptValidation(__FileType_Extn.Text)
            If errorfolder = False Then
                Response.Write(clsCommon.ErrorCodeScript())
                Exit Try
            End If
            'Get Input Param Values - Start
            InputParamsValues = PPS.GetStringArray(__FileTypeAction.Value, __Filetype_Id.SelectedValue,
                __FileType_Name.Text, __FileType_Extn.Text, __Is_MultipleHeader.SelectedValue,
                __BodyLines_PerHeader.Text, __FileType_Folder.Text, __FileType_Status.Text)
            'Get Input Param Values - Stop
        Catch ex As Exception



        End Try

    End Sub

#End Region

#Region " New/Clear "

    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Server.Transfer("pg_FileTypeSettings.aspx?FTA=1")
    End Sub

    Private Sub cleartext()
        __FileType_Extn.Text = ""
        __FileType_Folder.Text = ""
        __BodyLines_PerHeader.Text = ""
        __FileType_Name.Text = ""

    End Sub
#End Region

End Class
