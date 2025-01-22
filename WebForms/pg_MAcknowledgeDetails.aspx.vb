Imports MaxGeneric
Imports MaxMiddleware
Namespace MaxPayroll
    Partial Class WebForms_pg_MAcknowledgeDetails
        Inherits clsBasePage

#Region "Global Declarations "

        Private _Helper As New Helper
        Private _AcknoledgeId As String = "AI"
        '' Private _AcknoledgeAction As String = "AA"
        Dim lngOrgId As Long, lngUserId As Long
#End Region

#Region "Properties "
        'Author     : Bhanu Teja
        'Purpose    : Get the Action like insert or update etc; with filetype action from hidden variable on page
        'Created    : 10/11/2008
        Private ReadOnly Property GetAcknoledgeId() As Short
            Get
                Return __Acknowledge_FileType_Id.Value
            End Get
        End Property
#End Region

#Region "Page Load "

        'Author     : Bhanu Teja
        'Purpose    : Page load Actions
        'Created    : 23/10/2008
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'Create Instance of Data Table
            Dim AcknowledgeDetails As DataTable = Nothing
            Dim _ContentPlaceHolder As ContentPlaceHolder = Nothing

            'Variable Declarations
            Dim SQLStatement As String = Nothing

            Try

                If Not Page.IsPostBack Then

                    'Populate to Send Status DDL
                    Call PPS.EnumToDropDown(GetType(PPS.Boolean), __Acknowledge_Send, True)

                    'Populate to Success DDL
                    Call PPS.EnumToDropDown(GetType(PPS.Boolean), __Acknowledge_Success, True)

                    'Populate Acnoweldge File Type
                    SQLStatement = _Helper.GetInputFileTypeSQL & " 0"
                    Call FormHelp.PopulateDropDownList(PPS.GetData(SQLStatement, _
                        _Helper.GetSQLConnection, _Helper.GetSQLTransaction), __Input_FileType_Id, _
                            "FileType_Name", "FileType_Id")

                    'Populate Parent File Type
                    Call FormHelp.PopulateDropDownList(PPS.GetData(_Helper.GetInputFileTypeSQL & " 0", _
                        _Helper.GetSQLConnection, _Helper.GetSQLTransaction), __Parent_FileType_Id, _
                            "FileType_Name", "FileType_Id")

                    'Populate Data Grid
                    Call PopulateDataGrid()

                    'Query String Valus
                    Call GetQueryString()

                    If GetAcknoledgeId > 0 Then

                        'Get Content Place Holder
                        _ContentPlaceHolder = Me.Form.FindControl("cphContent")

                        'Get FileType Details
                        SQLStatement = _Helper.GetAcnowledgeSQL & " 0," & GetAcknoledgeId
                        AcknowledgeDetails = PPS.GetData(SQLStatement, _Helper.GetSQLConnection, _
                               _Helper.GetSQLTransaction)

                        'Populate Data To Page
                        Call DataHelp.DataToPage(AcknowledgeDetails, _ContentPlaceHolder)

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

#Region "Populate Data Grid "
        'Author     : Bhanu Teja
        'Purpose    : Fill the Grid
        'Created    : 07/11/2008
        Private Sub PopulateDataGrid()

            Try

                'Populate Data Grid - Start
                FormHelp.PopulateDataGrid(PPS.GetData(_Helper.GetAcnowledgeSQL & " 0,0", _
                    _Helper.GetSQLConnection, _Helper.GetSQLTransaction), dgAcnowledge)
                'Populate Data Grid - Stop

            Finally

                'force garbage collection
                GC.Collect(0)

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
                    _Helper.InsAcnowledgeSQL, _Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
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
                    Call Clear()
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

#End Region

#Region "Get SQL Input Params/Value "

        Private Sub GetSQLParamValue(ByRef InputParams As String(), _
                ByRef InputParamsValues As String())
            Try
                Dim clsCommon As New MaxPayroll.clsCommon
                'Get Input Param Values - Start
                InputParams = PPS.GetStringArray("Acknowledge_Id", "Input_FileType_Id", "Parent_FileType_Id", "Acknowledge_Code",
                "Acknowledge_Desc", "Acknowledge_Send", "Acknowledge_Success")
                'Get Input Param Values - Stop
                'Get Input Param Values - Stop
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(__Acknowledge_Code.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim folder As Boolean = clsCommon.CheckScriptValidation(__Acknowledge_Desc.Text)
                If folder = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                'Get Input Param Values - Start
                InputParamsValues = PPS.GetStringArray(__Acknowledge_FileType_Id.Value, __Input_FileType_Id.SelectedValue,
                __Parent_FileType_Id.SelectedValue, __Acknowledge_Code.Text, __Acknowledge_Desc.Text,
                    __Acknowledge_Send.SelectedValue, __Acknowledge_Success.SelectedValue)
                'Get Input Param Values - Stop
            Catch ex As Exception



            End Try
        End Sub
#End Region

#Region "Get Query Strings "

        Private Sub GetQueryString()

            Try

                'Get the File Type Action
                ''__Acknowledge_Action.Value = clsGeneric.NullToShort( _
                ''             Request.QueryString(_AcknoledgeAction))

                __Acknowledge_FileType_Id.Value = clsGeneric.NullToShort( _
                        Request.QueryString(_AcknoledgeId))

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

#Region "Clear Text"
        Private Sub Clear()
            __Acknowledge_Code.Text = ""
            __Acknowledge_Desc.Text = ""
            __Acknowledge_FileType_Id.Value = "0"

        End Sub
#End Region

#Region "New/Cancel Button Click"
        'Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        '    Server.Transfer("pg_MAcknowledgeDetails.aspx?AI="" ")
        '    btnSubmit.Visible = True
        '    btnNew.Visible = False
        'End Sub
        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Call Clear()
            lblMessage.Text = ""
        End Sub
#End Region

    End Class
End Namespace
