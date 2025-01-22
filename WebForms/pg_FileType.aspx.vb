Imports MaxGeneric
Imports MaxMiddleware
Imports System.IO
Imports System.Configuration.ConfigurationManager

Namespace MaxPayroll
    Partial Class WebForms_pg_FileType
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
        'Purpose    : Get the session and filetype from hidden variable
        'Created    : 23/10/2008
        Private Property FileType() As Short
            Get
                Return clsGeneric.NullToShort(_Helper.SessionFileType())
            End Get
            Set(ByVal value As Short)
                _Helper.SessionFileType = value
            End Set
        End Property
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
                Return __FileType_Id.Value
            End Get
        End Property
        'Author     : Bhanu Teja
        'Purpose    : To get the storedprocedures based on the filetype
        'Created    : 23/10/2008
        Private ReadOnly Property InsUpdStoredProc() As String
            Get
                Return _Helper.FileTypeSQL(FileType)
            End Get
        End Property

#End Region

#Region "Get Stored Procedure "

        Private Function GetStoredProc(ByVal iFileType As Short) As String

            Dim iFileType_1 As Short = 0

            Try
                'Get File Type Requested
                iFileType_1 = IIf(iFileType > 0, iFileType, FileType())

                Return _Helper.GetFileTypeSQL(iFileType_1)

            Finally

                'force garbagec collection
                GC.Collect(0)

            End Try

            Return String.Empty

        End Function

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

                'Hide Show Rows
                Call HideShowRows()

                ' Page Headings
                Call Headings()

                If Not Page.IsPostBack Then

                    'Populate to Status DDL
                    Call PPS.EnumToDropDown(GetType(PPS.FileStatus), __FileType_Status, True)

                    'Populate to Process DDL
                    Call PPS.EnumToDropDown(GetType(PPS.FileTypeProcess), __FileType_Process, True)

                    'Populate to FileType Table DDL
                    Call PPS.EnumToDropDown(GetType(PPS.Boolean), __FileType_Table, True)

                    'Populate to TrailerDisplay  DDL
                    Call PPS.EnumToDropDown(GetType(PPS.DisplayBodyTrailer), __FileType_Trailer_Display, True)

                    'Populate Data Grid
                    Call PopulateDataGrid()

                    If GetFileTypeId > 0 Then

                        'Get Content Place Holder
                        _ContentPlaceHolder = Me.Form.FindControl("cphContent")

                        'Get FileType Details
                        SQLStatement = GetStoredProc(0) & " " & GetFileTypeId
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
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_FileType", Err.Number, Err.Description)
            End Try

        End Sub

#End Region

#Region "Hide/Show Rows "

        'Author     : Bhanu Teja
        'Purpose    : To Hide or Show the controls based on conditions
        'Created    : 23/10/2008
        Private Sub HideShowRows()

            'Variable Declarations
            Dim SQLStatement As String = Nothing

            Try

                'if the file type is inward
                If FileType = PPS.FileType.Inward Then

                    'UI Management - Start
                    tr_outname.Visible = False
                    tr_inputid.Visible = False
                    tr_outputid.Visible = False
                    tr_Trailer_Display.Visible = False
                    tr_FileProcess.Visible = True
                    tr_FiletypeTable.Visible = True
                    tr_OrgEnd.Visible = True
                    tr_OrgStart.Visible = True
                    tr_ErrorFolder.Visible = True
                    tr_Archive.Visible = True
                    tr_TrailerStart.Visible = True
                    tr_TrailerEnd.Visible = True
                    tr_TrailerValue.Visible = True
                    'UI Management - Stop

                ElseIf FileType = Convert.ToInt16(PPS.FileType.Returned) Then

                    'UI Management - Start
                    tr_status.Visible = False
                    tr_folder.Visible = True
                    tr_inputid.Visible = True
                    tr_outname.Visible = False
                    tr_outputid.Visible = False
                    tr_FileProcess.Visible = False
                    tr_FiletypeTable.Visible = False
                    tr_OrgEnd.Visible = False
                    tr_OrgStart.Visible = False
                    tr_ErrorFolder.Visible = False
                    tr_Archive.Visible = False
                    tr_TrailerStart.Visible = False
                    tr_TrailerEnd.Visible = False
                    tr_TrailerValue.Visible = False
                    tr_Trailer_Display.Visible = False
                    'UI Management - Stop

                    If Not Page.IsPostBack Then
                        'Populate the Input File Type - Start
                        SQLStatement = GetStoredProc(PPS.FileType.Inward) & " 0"
                        Call FormHelp.PopulateDropDownList(PPS.GetData(SQLStatement, _
                            _Helper.GetSQLConnection, _Helper.GetSQLTransaction), __Input_Filetype_Id, _
                                "FileType_Name", "FileType_Id")
                        'Populate the Input File Type - Stop
                    End If


                ElseIf FileType = Convert.ToInt16(PPS.FileType.Outward) Then

                    'UI Management - Start
                    tr_inputid.Visible = True
                    tr_outname.Visible = True
                    tr_Trailer_Display.Visible = True
                    tr_outputid.Visible = False
                    tr_FileProcess.Visible = False
                    tr_FiletypeTable.Visible = False
                    tr_OrgEnd.Visible = False
                    tr_OrgStart.Visible = False
                    tr_ErrorFolder.Visible = False
                    tr_Archive.Visible = False
                    tr_TrailerStart.Visible = False
                    tr_TrailerEnd.Visible = False
                    tr_TrailerValue.Visible = False
                    'UI Management - Stop

                    If Not Page.IsPostBack Then
                        'Populate the Input File Type - Start
                        SQLStatement = GetStoredProc(PPS.FileType.Inward) & " 0"
                        Call FormHelp.PopulateDropDownList(PPS.GetData(SQLStatement, _
                            _Helper.GetSQLConnection, _Helper.GetSQLTransaction), __Input_Filetype_Id, _
                                "FileType_Name", "FileType_Id")
                        'Populate the Input File Type - Stop
                    End If


                ElseIf FileType = Convert.ToInt16(PPS.FileType.Response) Then

                    'UI Management - Start
                    tr_status.Visible = False
                    tr_inputid.Visible = False
                    tr_outname.Visible = False
                    tr_Trailer_Display.Visible = False
                    tr_outputid.Visible = True
                    tr_FileProcess.Visible = False
                    tr_FiletypeTable.Visible = False
                    tr_OrgEnd.Visible = False
                    tr_OrgStart.Visible = False
                    tr_ErrorFolder.Visible = False
                    tr_Archive.Visible = False
                    tr_TrailerStart.Visible = False
                    tr_TrailerEnd.Visible = False
                    tr_TrailerValue.Visible = False
                    'UI Management - Stop

                    If Not Page.IsPostBack Then
                        'Populate the Input File Type - Start
                        SQLStatement = GetStoredProc(PPS.FileType.Outward) & " 0,0"
                        Call FormHelp.PopulateDropDownList(PPS.GetData(SQLStatement, _
                            _Helper.GetSQLConnection, _Helper.GetSQLTransaction), __Output_Filetype_Id, _
                                "FileType_Name", "FileType_Id")
                        'Populate the Input File Type - Stop
                    End If


                End If

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

#Region "Get Query Strings "

        Private Sub GetQueryString()

            Try

                'Get the File Type Action
                FileTypeAction = clsGeneric.NullToShort( _
                        Request.QueryString(_FileTypeAction))

                'Get the File Type - Start
                If FileTypeAction = Helper.FileActionType.Insert Then
                    FileType = clsGeneric.NullToShort( _
                            Request.QueryString(_FileTypeQuery))
                End If
                'Get the File Type - Stop

                __FileType_Id.Value = clsGeneric.NullToShort( _
                        Request.QueryString(_FileTypeId))

                'Call _Helper.SetMatchFileType(FileType, __FileType_Id.Value)

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
                FormHelp.PopulateDataGrid(PPS.GetData(GetStoredProc(0) & " 0", _
                    _Helper.GetSQLConnection, _Helper.GetSQLTransaction), dgFileType)
                'Populate Data Grid - Stop

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
                If FileType = Convert.ToInt16(PPS.FileType.Inward) Then

                    'Get Input Param Values - Start
                    InputParams = PPS.GetStringArray("FileType_Id", "FileType_Process", "FileType_Name", "FileType_Folder",
                        "FileType_Header", "FileType_Footer", "FileType_Extn", "FileType_Status", "FileType_Table",
                         "Org_Start", "Org_End", "FileType_Error_Folder", "FileType_Archive_Folder",
                         "FileType_TrailerStart", "FileType_TrailerEnd", "FileType_TrailerValue")
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
                    Dim errorfolder As Boolean = clsCommon.CheckScriptValidation(__FileType_Error_Folder.Text)
                    If errorfolder = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim archievefolder As Boolean = clsCommon.CheckScriptValidation(__FileType_Archive_Folder.Text)
                    If archievefolder = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim orgstart As Boolean = clsCommon.CheckScriptValidation(__FileType_OrgStart.Text)
                    If orgstart = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim orgend As Boolean = clsCommon.CheckScriptValidation(__FileType_OrgEnd.Text)
                    If orgend = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim trailstart As Boolean = clsCommon.CheckScriptValidation(__FileType_TrailerStart.Text)
                    If trailstart = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim trailend As Boolean = clsCommon.CheckScriptValidation(__FileType_TrailerEnd.Text)
                    If trailend = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    Dim trailval As Boolean = clsCommon.CheckScriptValidation(__FileType_TrailerValue.Text)
                    If trailval = False Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                    'Get Input Param Values - Start
                    InputParamsValues = PPS.GetStringArray(__FileType_Id.Value, __FileType_Process.SelectedValue,
                        __FileType_Name.Text, __FileType_Folder.Text, __FileType_Header.Text,
                        __FileType_Footer.Text, __FileType_Extn.Text, __FileType_Status.Text,
                        __FileType_Table.SelectedValue, __FileType_OrgStart.Text, __FileType_OrgEnd.Text,
                        __FileType_Error_Folder.Text, __FileType_Archive_Folder.Text, __FileType_TrailerStart.Text,
                        __FileType_TrailerEnd.Text, __FileType_TrailerValue.Text)
                    'Get Input Param Values - Stop

                ElseIf FileType = Convert.ToInt16(PPS.FileType.Outward) Then

                    InputParams = PPS.GetStringArray("FileType_Id", "Input_FileType_Id", "FileType_Name",
                                         "FileType_Folder", "FileType_OutName", "FileType_Header", "FileType_Footer",
                                         "FileType_Extn", "FileType_Status", "FileType_Trailer_Display")
                    'Get Input Param Values - Stop

                    'Get Input Param Values - Start
                    InputParamsValues = PPS.GetStringArray(__FileType_Id.Value, __Input_Filetype_Id.SelectedValue,
                        __FileType_Name.Text, __FileType_Folder.Text, __Filetype_Outname.Text, __FileType_Header.Text,
                            __FileType_Footer.Text, __FileType_Extn.Text, __FileType_Status.Text,
                            __FileType_Trailer_Display.SelectedValue)
                    'Get Input Param Values - Stop

                ElseIf FileType = Convert.ToInt16(PPS.FileType.Response) Then

                    InputParams = PPS.GetStringArray("FileType_Id", "Output_FileType_Id", "FileType_Name", "FileType_Folder",
                                    "FileType_Header", "FileType_Footer", "FileType_Extn")
                    'Get Input Param Values - Stop

                    'Get Input Param Values - Start
                    InputParamsValues = PPS.GetStringArray(__FileType_Id.Value, __Output_Filetype_Id.Text,
                        __FileType_Name.Text, __FileType_Folder.Text, __FileType_Header.Text,
                            __FileType_Footer.Text, __FileType_Extn.Text)
                    'Get Input Param Values - Stop

                ElseIf FileType = Convert.ToInt16(PPS.FileType.Returned) Then

                    InputParams = PPS.GetStringArray("FileType_Id", "Input_FileType_Id", "FileType_Name", "FileType_Folder",
                                    "FileType_Header", "FileType_Footer", "FileType_Extn")
                    'Get Input Param Values - Stop

                    'Get Input Param Values - Start
                    InputParamsValues = PPS.GetStringArray(__FileType_Id.Value, __Input_Filetype_Id.SelectedValue,
                        __FileType_Name.Text, __FileType_Folder.Text, __FileType_Header.Text,
                            __FileType_Footer.Text, __FileType_Extn.Text)
                    'Get Input Param Values - Stop

                End If
            Catch ex As Exception



            End Try
            'Get Input Param Values - Start

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
                    InsUpdStoredProc, _Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                        CatchMessage, True, FileTypeId)
                'Save Data - Stop

                If Not Result Then
                    Dim clsGeneric As New MaxPayroll.Generic
                    Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_FileType", Err.Number, CatchMessage)
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
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_FileType", Err.Number, Err.Description)

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

#Region "New/Clear"
        Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click

            Server.Transfer("pg_FileType.aspx?FT= " & _Helper.SessionFileType & "&FTA=1&FTI=0")
        End Sub
        Private Sub cleartext()
            __FileType_Extn.Text = ""
            __FileType_Folder.Text = ""
            __FileType_Footer.Text = ""
            __FileType_Header.Text = ""
            __FileType_Name.Text = ""
            __Filetype_Outname.Text = ""

        End Sub
#End Region

#Region "Page Heading"
        Private Sub Headings()

            If FileType = PPS.FileType.Inward Then
                lblHeading.Text = "From Customer/Inward Filetype Page"
            ElseIf FileType = PPS.FileType.Outward Then
                lblHeading.Text = "To FSG/Outward Filetype Page"
            ElseIf FileType = PPS.FileType.Response Then
                lblHeading.Text = "From FSG/Response Filetype Page"
            ElseIf FileType = PPS.FileType.Returned Then
                lblHeading.Text = "To Customer/Return Filetype Page"
            Else
                lblHeading.Text = "File Type Page"
            End If
        End Sub

#End Region

    End Class

End Namespace
