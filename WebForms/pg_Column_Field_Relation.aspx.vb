Imports MaxGeneric
Imports MaxMiddleware
Imports MaxPayroll

Partial Class WebForms_pg_Column_Field_Relation
    Inherits clsBasePage

    Private _Helper As New Helper

#Region "Page Load "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Call BindFiletype()
            __FileType.Enabled = False
            __Field_Desc.Enabled = False
            __FieldColumn.Enabled = False
        End If

    End Sub

#End Region

#Region "Bind FileType "

    Private Sub BindFiletype()

        Dim FileTypeTable As New DataTable

        Try
            FileTypeTable = PPS.GetData(_Helper.SQLGetFileType, _
                _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

            If FileTypeTable.Rows.Count > 0 Then

                __FileType.DataSource = FileTypeTable
                __FileType.DataTextField = _Helper.FileTypeCol
                __FileType.DataValueField = _Helper.FileTypeIdCol
                __FileType.DataBind()

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Bind Field Description "

    Private Sub BindFieldDesc()

        Dim FieldDescTable As New DataTable

        Try
            'Get the Column Desc for the Seleted FileType
            FieldDescTable = PPS.GetData(_Helper.SQLGetFieldDesc & " " & __FileType.SelectedValue & ",'" _
                & rblContent.SelectedValue & "'", _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

            If FieldDescTable.Rows.Count > 0 Then

                __Field_Desc.DataSource = FieldDescTable
                __Field_Desc.DataTextField = _Helper.BankFieldDescCol
                __Field_Desc.DataValueField = _Helper.BankFieldIdCol
                __Field_Desc.DataBind()

            Else
                __Field_Desc.DataSource = FieldDescTable
                __Field_Desc.DataTextField = _Helper.BankFieldDescCol
                __Field_Desc.DataValueField = _Helper.BankFieldIdCol
                __Field_Desc.DataBind()
                __Field_Desc.Text = "-- No Files to Display --"

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Bind Match Fields Columns "

    Private Sub BindFieldColumns()

        Dim FieldColumnTable As New DataTable

        Try
            'Get the Column Desc for the Seleted FileType
            FieldColumnTable = PPS.GetData(_Helper.SQLGetFieldsColumn & "'" _
                & rblContent.SelectedItem.Text & "'", _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

            If FieldColumnTable.Rows.Count > 0 Then

                __FieldColumn.DataSource = FieldColumnTable
                __FieldColumn.DataTextField = _Helper.COLUMNNAMECol
                __FieldColumn.DataBind()

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Seleted Radio Button Change "

    Protected Sub rblContent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblContent.SelectedIndexChanged

        Call BindFieldDesc()
        Call BindFieldColumns()
        Call BindGrid()

        __FileType.Enabled = True
        __Field_Desc.Enabled = True
        __FieldColumn.Enabled = True

    End Sub

#End Region

#Region "Selected FileType Change "

    Protected Sub __FileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles __FileType.SelectedIndexChanged
        Call BindFieldDesc()
        Call BindGrid()
    End Sub

#End Region

#Region "Save to DataBase "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim SQLStr As String = Nothing, CatchMsg As String = Nothing

        Try
            SQLStr = _Helper.SQLAssignFields & " " & __FileType.SelectedValue & "," _
                & __Field_Desc.SelectedValue & ",'" & __FieldColumn.SelectedItem.Text & "','" _
                & rblContent.SelectedValue & "'"

            PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, SQLStr)

            If Not FormHelp.IsBlank(CatchMsg) Then
                lblMsg.Text = CatchMsg

            Else
                lblMsg.Text = "Record has been Saved Successfully."
            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Bind Grid "

    Private Sub BindGrid()

        Dim ColumnFieldRelTable As New DataTable

        Try
            ColumnFieldRelTable = PPS.GetData(_Helper.SQLWebColumnFieldRel & " " & _
            __FileType.SelectedValue & ",'" & rblContent.SelectedValue & "'", _Helper.GetSQLConnection, _
            _Helper.GetSQLTransaction)

            If ColumnFieldRelTable.Rows.Count > 0 Then
                dgFileType.DataSource = ColumnFieldRelTable
                dgFileType.DataBind()
            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class
