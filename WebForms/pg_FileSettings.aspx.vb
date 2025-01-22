Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports MaxPayroll.clsApprMatrix
Imports MaxGeneric
Imports MaxMiddleware

Namespace MaxPayroll
    Partial Class WebForms_pg_FileSettings
        Inherits clsBasePage

#Region "Global Declarations "

        Private _Helper As New Helper
        Private _FieldId As String = "FI"
        Private _FileTypeAction As String = "FTA"
        Private _MatchFileTypeId As String = "MFTI"
        Dim lngOrgId As Long, lngUserId As Long

#End Region

#Region "Properties "

        'Author     : Bhanu Teja
        'Purpose    : Get the Action like insert or update etc; with filetype action from hidden variable on page
        'Created    : 23/10/2008
        Private ReadOnly Property GetFileTypeAction() As Short
            Get
                Return __FileTypeAction.Value
            End Get
        End Property
        'Author     : Bhanu Teja
        'Purpose    : To get the storedprocedures based on the filetype
        'Created    : 23/10/2008
        Private ReadOnly Property InsUpdStoredProc() As String
            Get
                Return _Helper.FileSettingsSQL(_Helper.SessionFileType)
            End Get
        End Property
        Private ReadOnly Property GetMatchFileTypeId() As Short
            Get
                Return 0
            End Get
        End Property

        Private ReadOnly Property GetFieldId() As Short
            Get
                Return __Field_Id.Value
            End Get
        End Property

#End Region

#Region "Page Load "

        'Author     : Bhanu Teja
        'Purpose    : Page load Actions
        'Created    : 23/10/2008
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Create Instance of Data Table/Row
            Dim FileSettingsDetails As DataTable = Nothing
            Dim _ContentPlaceHolder As ContentPlaceHolder = Nothing

            'Variable Declarations
            Dim SQLStatement As String = Nothing

            Try

                'Query String Values
                Call GetQueryString()

                'Hide/Show Rows while page is loading
                Call HideShowRows()

                'Puttings Headings
                Call Headings()

                If Not Page.IsPostBack Then

                    'Populate Drop Down Lists - Start
                    Call PopulateMatchField()
                    Call PPS.EnumToDropDown(GetType(PPS.Boolean), __Field_Repeat, True)
                    Call PPS.EnumToDropDown(GetType(PPS.DataType), __Field_DataType, True)
                    Call PPS.EnumToDropDown(GetType(PPS.Boolean), __Field_Mandatory, True)
                    Call PPS.EnumToDropDown(GetType(EnumHelp.FillerType), __Filler_Type, True)
                    Call PPS.EnumToDropDown(GetType(PPS.ContentType), __Field_ContentType, True)
                    Call PPS.EnumToDropDown(GetType(PPS.PredefinedOptions), __Field_Options, True)
                    Call PPS.EnumToDropDown(GetType(EnumHelp.FieldOperator), __Field_Operator, True)
                    'Populate Drop Down Lists - Stop

                    If GetFieldId > 0 Then

                        'Get Content Place Holder
                        _ContentPlaceHolder = Me.Form.FindControl("cphContent")

                        'Get FileType Details
                        FileSettingsDetails = _Helper.GetFileSettings( _
                           _Helper.SessionFileType, _Helper.SessionFileTypeId, GetFieldId)

                        'Populate Data To Page
                        Call DataHelp.DataToPage(FileSettingsDetails, _ContentPlaceHolder)

                    End If

                End If

            Catch ex As Exception

                lblMessage.Text = ex.Message
                ' Error Logs Starts Here
                Dim clsGeneric As New MaxPayroll.Generic
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_Settings", Err.Number, Err.Description)

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
                __FileTypeAction.Value = clsGeneric.NullToShort( _
                        Request.QueryString(_FileTypeAction))

                'Get Field Id
                __Field_Id.Value = clsGeneric.NullToShort( _
                        Request.QueryString(_FieldId))



            Finally

                'force garbage collection
                GC.Collect(0)

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
                If _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Inward) Then

                    'UI Management - Start
                    tr_Match.Visible = True
                    tr_FieldRepeat.Visible = True
                    tr_FillerChar.Visible = False
                    tr_FillerType.Visible = False

                    'UI Management - Stop

                ElseIf _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Returned) Then

                    'UI Management - Start
                    tr_FieldMandatory.Visible = True
                    tr_Match.Visible = True
                    tr_FillerType.Visible = True
                    'UI Management - Stop

                ElseIf _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Outward) Then

                    'UI Management - Start
                    tr_FieldRepeat.Visible = False
                    tr_FillerChar.Visible = True
                    tr_FillerType.Visible = True
                    'UI Management - Stop

                ElseIf _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Response) Then

                    'UI Management - Start

                    tr_Match.Visible = True
                    tr_FillerType.Visible = False
                    tr_FillerChar.Visible = False
                    tr_FieldRepeat.Visible = False
                    tr_FieldDefault.Visible = False
                    tr_FieldMandatory.Visible = False
                    'UI Management - Stop
                End If

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

#Region "Populate Match Field "

        Private Sub PopulateMatchField()

            'Create Instance of Data Table/Row
            Dim _DataRow As DataRow = Nothing
            Dim MatchFileSettings As DataTable = Nothing

            'Variable Declarations
            Dim SQLStatement As String = Nothing, MatchFieldName As String = Nothing

            Try

                If Not _Helper.SessionFileType = PPS.FileType.Inward Then

                    'Populate Match Field Drop Down List - Start
                    __Field_Match.Items.Add(New ListItem("Select", "0"))
                    MatchFileSettings = _Helper.GetMatchField(_Helper.MatchSessionFileTypeId)

                    For Each _DataRow In MatchFileSettings.Rows
                        MatchFieldName = _DataRow("Field_Description")
                        MatchFieldName &= " - " & [Enum].GetName(GetType(PPS.ContentType), _
                            clsGeneric.NullToInteger(_DataRow("Field_ContentType")))
                        __Field_Match.Items.Add(New ListItem(MatchFieldName, _DataRow("Field_Id")))
                    Next
                    'Populate Match Field Drop Down List - Stop

                End If

            Finally
                'force garbage collection
                GC.Collect(0)
            End Try

        End Sub

#End Region

#Region "Get SQL Input Params/Value "

        Private Sub GetSQLParamValue(ByRef InputParams As String(), _
                ByRef InputParamsValues As String())

            If _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Inward) Then

                'Get Input Param Values - Start
                InputParams = PPS.GetStringArray("Field_Id", "FileType_Id", "Field_Description", _
                    "Field_DataType", "Field_ContentType", "Field_Mandatory", "Field_DefaultValue", _
                    "Field_Options", "Field_Match", "Field_Operator", "Field_Repeat", "Field_Start", "Field_End")
                'Get Input Param Values - Stop

                'Get Input Param Values - Start
                InputParamsValues = PPS.GetStringArray(__Field_Id.Value, _
                        _Helper.SessionFileTypeId, __Field_Description.Text, __Field_DataType.SelectedValue, _
                        __Field_ContentType.SelectedValue, __Field_Mandatory.SelectedValue, _
                        __Field_DefaultValue.Text, __Field_Options.SelectedValue, __Field_Match.SelectedValue, _
                        __Field_Operator.SelectedValue, __Field_Repeat.SelectedValue, __Field_Start.Text, _
                        __Field_End.Text)
                'Get Input Param Values - Stop

            ElseIf _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Outward) Then

                InputParams = PPS.GetStringArray("Field_Id", "FileType_Id", "Field_Description", _
                    "Field_DataType", "Field_ContentType", "Field_Mandatory", "Field_DefaultValue", _
                    "Field_Options", "Field_Match", "Field_Operator", "Filler_Type", "Filler_Char", _
                    "Field_Start", "Field_End")
                'Get Input Param Values - Stop

                'Get Input Param Values - Start
                InputParamsValues = PPS.GetStringArray(__Field_Id.Value, _
                        _Helper.SessionFileTypeId, __Field_Description.Text, __Field_DataType.SelectedValue, _
                        __Field_ContentType.SelectedValue, __Field_Mandatory.SelectedValue, _
                        __Field_DefaultValue.Text, __Field_Options.SelectedValue, _
                        __Field_Match.SelectedValue, __Field_Operator.SelectedValue, _
                        __Filler_Type.SelectedValue, __Filler_Char.Text, __Field_Start.Text, __Field_End.Text)
                'Get Input Param Values - Stop

            ElseIf _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Response) Then

                InputParams = PPS.GetStringArray("Field_Id", "FileType_Id", "Field_Description", _
                    "Field_DataType", "Field_ContentType", "Field_Options", "Field_Match", _
                    "Field_Operator", "Field_Start", "Field_End")
                'Get Input Param Values - Stop

                'Get Input Param Values - Start
                InputParamsValues = PPS.GetStringArray(__Field_Id.Value, _
                        _Helper.SessionFileTypeId, __Field_Description.Text, __Field_DataType.SelectedValue, _
                        __Field_ContentType.SelectedValue, __Field_Options.SelectedValue, _
                        __Field_Match.Text, __Field_Operator.SelectedValue, __Field_Start.Text, __Field_End.Text)
                'Get Input Param Values - Stop

            ElseIf _Helper.SessionFileType = Convert.ToInt16(PPS.FileType.Returned) Then

                InputParams = PPS.GetStringArray("Field_Id", "FileType_Id", "Field_Description", _
                    "Field_DataType", "Field_ContentType", "Field_Mandatory", "Field_DefaultValue", _
                    "Field_Options", "Field_Operator", "Field_Match", "Filler_Type", "Filler_Char", _
                    "Field_Start", "Field_End")
                'Get Input Param Values - Stop

                'Get Input Param Values - Start
                InputParamsValues = PPS.GetStringArray(__Field_Id.Value, _
                        _Helper.SessionFileTypeId, __Field_Description.Text, __Field_DataType.SelectedValue, _
                        __Field_ContentType.SelectedValue, __Field_Mandatory.SelectedValue, _
                        __Field_DefaultValue.Text, __Field_Options.SelectedValue, _
                        __Field_Operator.SelectedValue, __Field_Match.SelectedValue, _
                        __Filler_Type.SelectedValue, __Filler_Char.Text, __Field_Start.Text, __Field_End.Text)
                'Get Input Param Values - Stop

            End If
            'Get Input Param Values - Start

        End Sub

#End Region

#Region "Page Submit "

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            'Variable Declarations
            Dim filecheck As Short = 0
            Dim Result As Boolean = False, FieldId As Short = 0
            Dim SQLStatement As String = Nothing, CatchMessage As String = Nothing
            Dim InputParams As String() = Nothing, InputParamsValues As String() = Nothing


            Try

                If __FileTypeAction.Value = 3 Then

                    SQLStatement = _Helper.FileSettingsDeleteSQL([Enum].GetName(GetType(PPS.FileType), _
                                            _Helper.SessionFileType), __Field_Id.Value)
                    Result = PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                        CatchMessage, SQLStatement)

                    If Result Then
                        'success message
                        lblMessage.Text = "Record has been deleted Successfully"
                        Call cleartext()
                    Else
                        'error message
                        Dim clsGeneric As New MaxPayroll.Generic
                        Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnSave_Click - PG_Settings", Err.Number, CatchMessage)
                        If CatchMessage.Contains("duplicate") Then
                            lblMessage.Text = "Duplicate Content"
                        Else
                            lblMessage.Text = "Invalid Content"
                        End If
                        'lblMessage.Text = CatchMessage
                    End If

                Else
                    ''Calling Chec File settings tables to Check the Start and End Fields
                    filecheck = _Helper.CheckFileSettings(__Field_Start.Text, __Field_End.Text, GetFieldId, _
                                    __Field_ContentType.SelectedValue)

                    If filecheck = 0 Then

                        Call GetSQLParamValue(InputParams, InputParamsValues)

                        'Save Data - Start
                        Result = PPS.SQLInsertUpdate(InputParams, InputParamsValues, _
                            InsUpdStoredProc, _Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                                CatchMessage, True, FieldId)
                        'Save Data - Stop

                        If Not Result Then
                            Dim clsGeneric As New MaxPayroll.Generic
                            Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnSave_Click - PG_Settings", Err.Number, CatchMessage)
                            If CatchMessage.Contains("duplicate") Then
                                lblMessage.Text = "Duplicate Content"
                            Else
                                lblMessage.Text = "Invalid Content"
                            End If
                            'lblMessage.Text = CatchMessage
                        Else
                            lblMessage.Text = "Record has been inserted/modified Successfully"
                            Call cleartext()

                        End If

                    ElseIf filecheck > 0 Then
                        lblMessage.Text = "Record With these Start And End Fields is alredy Exist. Please enter another Numbers"
                    End If

                End If

            Catch ex As Exception
                lblMessage.Text = ex.Message
                ' Error Logs Starts Here
                Dim clsGeneric As New MaxPayroll.Generic
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "BtnSave_Click - PG_Settings", Err.Number, Err.Description)

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

#Region "Checking File Settings For start and end Fields length"
        Private Sub CheckFileSettings()
            ''Dim filecheck As New Short
            '' filecheck = _Helper.CheckFileSettings(__Field_Start.Text, __Field_End.Text, GetFieldId, __Field_ContentType.SelectedValue)

        End Sub

#End Region

#Region "Clear text"
        Private Sub cleartext()
            __Field_End.Text = ""
            __Field_Start.Text = ""
            __Filler_Char.Text = ""
            __Field_Description.Text = ""
            __Field_DefaultValue.Text = ""

        End Sub
#End Region

#Region "Page Headings"
        Private Sub Headings()
            If _Helper.SessionFileType = PPS.FileType.Inward Then
                lblHeading.Text = "Inward FileSettings Page"
            ElseIf _Helper.SessionFileType = PPS.FileType.Outward Then
                lblHeading.Text = "Outward FileSettings Page"
            ElseIf _Helper.SessionFileType = PPS.FileType.Response Then
                lblHeading.Text = "Response FileSettings Page"
            ElseIf _Helper.SessionFileType = PPS.FileType.Returned Then
                lblHeading.Text = "Return FileSettings Page"
            Else
                lblHeading.Text = "File Settings Page"
            End If
        End Sub
#End Region

#Region "New "
        Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
            Response.Redirect("pg_FileSettings.aspx?FTA=1", True)
        End Sub
#End Region


    End Class
End Namespace