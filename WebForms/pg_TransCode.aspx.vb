Imports MaxGeneric
Imports MaxMiddleware
Imports System.Configuration.ConfigurationManager
Namespace MaxPayroll
    Partial Class WebForms_pg_TransCode
        Inherits clsBasePage

#Region "Global Declarations "

        Private _Helper As New Helper
        Private _TransId As String = "TI"
        Private _FileTypeId As String = "FTI"
        Private _InputFileId As String = "INFI"
        Private _OutputFile As String = "OF"
        Private _FileTypeAction As String = "FTA"
        Dim lngOrgId As Long, lngUserId As Long
#End Region

#Region "Properties "
        'Author     : Bhanu Teja
        'Purpose    : Get the Action like insert or update etc; with filetype action from hidden variable on page
        'Created    : 20/11/2008
        Private Property FileTypeAction() As Short
            Get
                Return clsGeneric.NullToShort(_Helper.SessionFileTypeAction())
            End Get
            Set(ByVal value As Short)
                _Helper.SessionFileTypeAction = value
            End Set
        End Property

        Private ReadOnly Property OutputFile() As String
            Get
                Return clsGeneric.NullToString(Request.QueryString(_OutputFile))
            End Get
        End Property
        Private ReadOnly Property InputFileID() As String
            Get
                Return clsGeneric.NullToString(Request.QueryString(_InputFileId))
            End Get
        End Property
        Private ReadOnly Property OutputFileTypeID() As Short
            Get
                Return clsGeneric.NullToShort(Request.QueryString(_FileTypeId))
            End Get
        End Property
        Private ReadOnly Property GetTransId() As Short
            Get
                Return __TransID.Value
            End Get
        End Property

#End Region

#Region "Stored Procedures"
        'Author     : Bhanu Teja
        'Purpose    : To get the storedprocedures 
        'Created    : 20/11/2008

        '' Input of TransCode Storedprocedures starts here
        Private ReadOnly Property InsUpdTransSQL() As String
            Get
                Return AppSettings("SQL_TransCode")
            End Get
        End Property
        Private ReadOnly Property GetTransSQL() As String
            Get
                Return "EXEC " & AppSettings("SQL_GetTransCode")
            End Get
        End Property
#End Region

#Region "Page Load "

        'Author     : Bhanu Teja
        'Purpose    : Page load Actions
        'Created    : 20/11/2008
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

                    __Output_FileType_Id.Text = OutputFile

                    'Populate to FileType Table DDL
                    Call PPS.EnumToDropDown(GetType(PPS.Boolean), __Tran_Successful, True)

                    Call PopulateDataGrid()

                    If GetTransId > 0 Then

                        'Get Content Place Holder
                        _ContentPlaceHolder = Me.Form.FindControl("cphContent")

                        'Get FileType Details
                        SQLStatement = GetTransSQL & " " & " 0,0," & GetTransId
                        FileTypeDetails = PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                                _Helper.GetSQLTransaction)

                        'Populate Data To Page
                        Call DataHelp.DataToPage(FileTypeDetails, _ContentPlaceHolder)

                    End If

                End If
            Catch

            End Try

        End Sub
#End Region

#Region "Page Submit"


        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            'Variable Declarations
            Dim Result As Boolean = False, FileTypeId As Short = 0
            Dim SQLStatement As String = Nothing, CatchMessage As String = Nothing
            Dim InputParams As String() = Nothing, InputParamsValues As String() = Nothing

            Try

                Call GetSQLParamValue(InputParams, InputParamsValues)

                'Save Data - Start
                Result = PPS.SQLInsertUpdate(InputParams, InputParamsValues, _
                    InsUpdTransSQL, _Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                        CatchMessage, True, FileTypeId)
                'Save Data - Stop

                If Not Result Then
                    Dim clsGeneric As New MaxPayroll.Generic
                    Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_MAcknoledge", Err.Number, CatchMessage)
                    If CatchMessage.Contains("duplicate") Then
                        lblMessage.Text = "Duplicate Content"
                    Else
                        lblMessage.Text = "Invalid Content"
                    End If
                    'lblMessage.Text = CatchMessage
                Else
                    '' Server.Transfer("pg_MAcknowledgeDetails.aspx?AI="" ")

                    lblMessage.Text = "Record has been inserted successfully"
                    Call PopulateDataGrid()
                    ' btnSubmit.Visible = False
                    'btnNew.Visible = True

                End If

            Catch ex As Exception
                'lblMessage.Text = CatchMessage
                ' Error Logs Starts Here
                Dim clsGeneric As New MaxPayroll.Generic
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Submit - PG_MAcknoledge", Err.Number, Err.Description)

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try
        End Sub
        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            __Tran_Code.Text = ""
            __Tran_Desc.Text = ""
        End Sub
#End Region

#Region "Get SQL Input Params/Value "

        Private Sub GetSQLParamValue(ByRef InputParams As String(), _
                ByRef InputParamsValues As String())

            'Get Input Param Values - Start
            InputParams = PPS.GetStringArray("Tran_Id", "Output_FileType_Id", "Tran_Code", "Tran_Desc", _
                "Tran_Successful", "Input_FileType_Id")
            'Get Input Param Values - Stop

            'Get Input Param Values - Start
            InputParamsValues = PPS.GetStringArray(__TransID.Value, OutputFileTypeID, _
                __Tran_Code.Text, __Tran_Desc.Text, __Tran_Successful.SelectedValue, InputFileID)
            'Get Input Param Values - Stop
        End Sub
#End Region

#Region "Get Query Strings "
        Private Sub GetQueryString()
            Try

                'Get the File Type Action
                FileTypeAction = clsGeneric.NullToShort( _
                        Request.QueryString(_FileTypeAction))
                __TransID.Value = clsGeneric.NullToShort( _
                        Request.QueryString(_TransId))
            Finally
                'force garbage collection
                GC.Collect(0)
            End Try
        End Sub
#End Region

#Region "Populate Data Grid "
        'Author     : Bhanu Teja
        'Purpose    : Fill the Grid
        'Created    : 20/11/2008
        Private Sub PopulateDataGrid()

            Try

                'Populate Data Grid - Start
                FormHelp.PopulateDataGrid(PPS.GetData(GetTransSQL & " " & InputFileID & " ", _
                    _Helper.GetSQLConnection, _Helper.GetSQLTransaction), dgTransCode)
                'Populate Data Grid - Stop

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

    End Class
End Namespace
