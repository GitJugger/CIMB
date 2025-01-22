Imports MaxGeneric
Imports MaxMiddleware
Namespace MaxPayroll
    Partial Class WebForms_pg_TransGrid
        Inherits clsBasePage

#Region "Global Declarations "
        Private _Helper As New Helper
#End Region

#Region "Page Load "

        'Author     : Bhanu Teja
        'Purpose    : Page load Actions
        'Created    : 23/10/2008
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not Page.IsPostBack Then
                Call BindDropdown()
            End If


        End Sub
#End Region

#Region "Populate Data Grid "
        'Author     : Bhanu Teja
        'Purpose    : Fill the Grid
        'Created    : 23/10/2008
        Private Sub PopulateDataGrid()

            Try

                'Populate Data Grid - Start
                FormHelp.PopulateDataGrid(PPS.GetData(_Helper.GetFileTypeSQL(PPS.FileType.Outward) & " 0," _
                            & __Input_Filetype_Id.SelectedValue, _Helper.GetSQLConnection, _
                            _Helper.GetSQLTransaction), dgTransGrid)
                'Populate Data Grid - Stop

            Finally

                'force garbage collection
                GC.Collect(0)

            End Try

        End Sub

#End Region

#Region "Bind Dropdown"
        Private Sub BindDropdown()
            Dim SQLStatement As String = Nothing
            Dim InputFiletype As DataTable = Nothing
            Dim _DataRow As DataRow = Nothing, FileType_Name As String = Nothing

            'Populate the Input File Type - Start
            __Input_Filetype_Id.Items.Add(New ListItem("Select", "-1"))
            SQLStatement = _Helper.GetInputFileTypeSQL & " 0"
            InputFiletype = PPS.GetData(SQLStatement, _
               _Helper.GetSQLConnection, _Helper.GetSQLTransaction)
            For Each _DataRow In InputFiletype.Rows
                FileType_Name = _DataRow("FileType_Name")
                __Input_Filetype_Id.Items.Add(New ListItem(FileType_Name, _DataRow("FileType_Id")))
            Next
            'Populate the Input File Type - Stop
        End Sub

#End Region

        Protected Sub __Input_Filetype_Id_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles __Input_Filetype_Id.SelectedIndexChanged
            Call PopulateDataGrid()

        End Sub
    End Class
End Namespace
