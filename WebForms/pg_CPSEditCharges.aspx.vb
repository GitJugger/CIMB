#Region "Name Sapces "
Imports System.Math
Imports MaxGateway
Imports Maxpayroll
Imports MaxGeneric
Imports MaxMiddleware
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
#End Region

Namespace MaxPayroll
    Partial Class pg_CPSEditCharges
        Inherits clsBasePage

#Region "Global Declarations "

        Private _Generic As New clsBaseGeneric
        Private _Helper As New Helper
        Dim GridMsg As String = "Please Insert Here"

#End Region

#Region "Page Load "
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                If Not Page.IsPostBack Then
                    lblHeading.Text = "Fee Charges Maintenance"
                    Call GetQueryString()
                    GetCurrentCharges()
                    Call FillPreviousCharges()
                End If

            Catch
                'Log Error
                _Generic.LogError("Page Load - pg_CPSEditCharges")
            Finally

            End Try
        End Sub
#End Region

#Region "Read From Query "
        Private Sub GetQueryString()
            Try
                __OrgID.Value = Request.QueryString("OrgID")

            Catch ex As Exception

                'Log Error
                _Generic.LogError("GetQuaryString - pg_CPSEditCharges", ex.Message)


            End Try
        End Sub
#End Region

#Region "Get the Charges "

        'Created by : Bhanu Teja
        'Purpose    : To get the Charges from Db
        'Created    : 14/05/2009

        Private Sub GetCurrentCharges()

            Dim DtCharge As DataTable = CurrentCharges()

            Try
                If DtCharge.Rows.Count > 0 Then

                    ''Get the Fixed Charges to Page -Start
                    If DtCharge.Rows(0)(_Helper.ChargeTypeCol) = 1 Then
                        rbtlCharges.SelectedValue = Helper.ChargeType.Fixed
                        ShowOrHide()
                        __FixedChargeID.Value = DtCharge.Rows(0)(_Helper.ChargeIDCol)
                        __FixedCharge.Text = DtCharge.Rows(0)(_Helper.FixedChargesCol)

                        'Enable/Disable the Controls Based on Condition - Start
                        __FixedCharge.Enabled = False
                        rbtlCharges.Enabled = False
                        tr_FixedButtons.Visible = False
                        btnDelete.Visible = True
                        'Enable/Disable the Controls Based on Condition - Stop

                        ''Get the Fixed Charges to Page -Stop

                        ''Get the Tier Charges to Page -Start
                    ElseIf DtCharge.Rows(0)(_Helper.ChargeTypeCol) = 2 Then
                        rbtlCharges.SelectedValue = Helper.ChargeType.Tier
                        ShowOrHide()

                        'Enable/Disable the Controls Based on Condition - Start
                        rbtlCharges.Enabled = False
                        btnDelete.Visible = True
                        'Enable/Disable the Controls Based on Condition - Stop

                        ''Get the Tier Charges to Page -Stop
                    End If

                Else

                    rbtlCharges.SelectedValue = Helper.ChargeType.Fixed
                    Call ShowOrHide()
                    'Enable/Disable the Controls Based on Condition - Start
                    btnDelete.Visible = False
                    rbtlCharges.Enabled = True
                    tr_FixedButtons.Visible = True
                    'Enable/Disable the Controls Based on Condition - Stop

                End If
            Catch ex As Exception

                'Log Error
                _Generic.LogError("GetCurrentCharges - pg_CPSEditCharges", ex.Message)


            End Try
        End Sub

        Private Function CurrentCharges() As DataTable
            Dim DtCharge As New DataTable
            Dim SqlStr As String = Nothing

            Try
                SqlStr = _Helper.GetChargesSQL() & " " & __OrgID.Value & ",0"

                DtCharge = PPS.GetData(SqlStr, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

                Return DtCharge

            Catch ex As Exception

                'Log Error
                _Generic.LogError("CurrentCharges - pg_CPSEditCharges", ex.Message)

                Return Nothing

            Finally

                GC.Collect(0)

            End Try
        End Function
#End Region

#Region "Grid Binding "

        ''Created By : Bhanu Teja
        ''Purpose    : To fill the tier charges accordingly
        ''Created    : 05/05/2009
        ''Modified   : 26/05/2009

        Private Sub BindGrid()

            'Create Instances
            Dim _DtTireCharges As New DataTable
            Dim _DataRow As Data.DataRow = Nothing
            Dim _TransToRow() As Data.DataRow = Nothing

            'Declare Variables 
            Dim i As Integer = 0

            Try
                'Get the Tire charges from databse
                Dim DtTier As DataTable = CurrentCharges()

                'Check if tere is any tire charges or not
                If DtTier.Rows.Count > 0 Then
                    'Bind the tire charges to grid -start
                    grdTire.DataSource = DtTier
                    grdTire.DataBind()
                    'Bind the tire charges to grid -stop

                    'Clone the table structure to new table 
                    _DtTireCharges = DtTier.Clone()
                    'Change the datatype of column in new table 
                    _DtTireCharges.Columns(_Helper.TransactionToCol).DataType = System.Type.GetType("System.String")

                    'Import each row from Old datatable to New table with changed data type - Start
                    For Each _DataRow In DtTier.Rows
                        _DtTireCharges.ImportRow(_DataRow)
                    Next
                    'Import each row from Old datatable to New table with changed data type - Stop

                    'Get the Row which contains the Zero(0) Value i.e Infinity
                    _TransToRow = _DtTireCharges.Select(_Helper.TransactionToCol & " = 0")

                    'Check if the row contains value
                    If _TransToRow.Length > 0 Then

                        'Assign the text Infinity for each column which contains "0" - Start 
                        For i = 0 To UBound(_TransToRow)
                            _TransToRow(i)(_Helper.TransactionToCol) = _Helper.TierMaxDisplayCol
                        Next
                        'Assign the text Infinity for each column which contains "0" - Stop

                        'Bind the new datatable to Grid - Start
                        grdTire.DataSource = _DtTireCharges
                        grdTire.DataBind()
                        'Bind the new datatable to Grid - Stop

                        ' Make footer ro visible false as we already have charges upto infinity
                        Me.grdTire.FooterRow.Visible = False

                    End If

                    If grdTire.Rows.Count >= 2 Then
                        Dim FooterTransTo As TextBox = DirectCast(grdTire.FooterRow.FindControl("txtNewTo"), TextBox)
                        FooterTransTo.Text = 0
                        FooterTransTo.Enabled = False
                    End If

                    If grdTire.Rows.Count >= 3 Then
                        grdTire.FooterRow.Visible = False
                    End If

                    'Disable the charge type selection 
                    rbtlCharges.Enabled = False

                    'Make delete button visible 
                    btnDelete.Visible = True

                Else
                    DtTier.Rows.Add(DtTier.NewRow())
                    DtTier.Columns.Add(_Helper.ChargeIDCol)
                    DtTier.Columns.Add(_Helper.TransactionFromCol)
                    DtTier.Columns.Add(_Helper.TransactionToCol)
                    DtTier.Columns.Add(_Helper.TransactionChargeCol)
                    grdTire.DataSource = DtTier
                    grdTire.DataBind()

                    Dim TotalColumns As Integer = grdTire.Rows(0).Cells.Count
                    grdTire.Rows(0).Cells.Clear()
                    grdTire.Rows(0).Cells.Add(New TableCell())
                    grdTire.Rows(0).Cells(0).ColumnSpan = TotalColumns
                    grdTire.Rows(0).Cells(0).Text = GridMsg

                    rbtlCharges.Enabled = True

                End If
            Catch ex As Exception
                'Log Error
                _Generic.LogError("BindGrid - pg_CPSEditCharges", ex.ToString)

            End Try
        End Sub
#End Region

#Region "Hide/Show "
        'Created By : Bhanu teja
        'Purpose    : To Hide/Show the contraols based on the selected radio button list
        'Created on : 04/05/2009

        Private Sub ShowOrHide()
            'If Fixed charge type is selected - Start
            If rbtlCharges.SelectedValue = Helper.ChargeType.Fixed Then
                tr_Fixed.Visible = True
                tr_FixedButtons.Visible = True
                tr_Tier.Visible = False
                tr_note.Visible = False
                'tr_TireButtons.Visible = False
                'If Fixed charge type is selected - Stop

                'If Tier charge type is selected - Start
            ElseIf rbtlCharges.SelectedValue = Helper.ChargeType.Tier Then
                tr_Fixed.Visible = False
                tr_FixedButtons.Visible = False
                tr_Tier.Visible = True
                tr_note.Visible = True
                'tr_TireButtons.Visible = True
                Call BindGrid()

            End If
            'If Tier charge type is selected - Stop

        End Sub
#End Region

#Region "Save Tier Charges "

        Protected Sub grdTire_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdTire.RowCommand

            'Variable Declaration
            Dim ChargeErr As String = Nothing
            Dim Msg As String = Nothing, TransCharges As Decimal = 0, i As Integer = 0
            Dim TransFrom As Integer = 0, TransTo As Integer = 0, PrevCharges As Decimal = 0


            If e.CommandName = "Insert" Then
                Try
                    Dim FromNo As TextBox = DirectCast(grdTire.FooterRow.FindControl("txtNewFrom"), TextBox)
                    Dim ToNo As TextBox = DirectCast(grdTire.FooterRow.FindControl("txtNewTo"), TextBox)
                    Dim Charges As TextBox = DirectCast(grdTire.FooterRow.FindControl("txtNewCharge"), TextBox)

                    'Check If trans from Textbox is empty
                    If FromNo.Text = "" Then
                        Msg &= "\n Transactions From Field cannot be empty"
                    Else
                        TransFrom = clsGeneric.NullToInteger(FromNo.Text)
                    End If

                    'Check If trans to Textbox is empty
                    If ToNo.Text = "" Then
                        If Not ToNo.Visible = False Then
                            Msg &= "\n Transactions To Field cannot be empty"
                        Else
                            ToNo.Text = 0
                        End If

                    Else
                        TransTo = clsGeneric.NullToInteger(ToNo.Text)

                        If TransTo <= TransFrom And ToNo.Enabled = True And TransTo <> 0 Then
                            Msg &= "\n Transactions To Field cannot be Less than Transaction From"
                        End If
                    End If

                    'Check If chargesTextbox is empty
                    If Charges.Text = "" Then
                        Msg &= "\n Transactions Charges Field cannot be empty"

                    Else
                        'Get Present Charges for this record which has to store in DB
                        TransCharges = Round(clsGeneric.NullToDecimal(Charges.Text), 2, MidpointRounding.AwayFromZero)

                        ''Checking for present charges which are higher than previous - Start

                        'Loop thro the Gridview data - Start
                        'Check if the inserting is 1st row or not 
                        If grdTire.Rows.Count > 0 And Not grdTire.Rows(0).Cells(0).Text = GridMsg Then

                            For i = 0 To grdTire.Rows.Count - 1

                                'Get the Previous charges for this OrgId in this Grid
                                PrevCharges = clsGeneric.NullToDecimal _
                                    (DirectCast(grdTire.Rows(i).FindControl("lblCharge"), Label).Text)

                                'Check if Present Charges are Highe than previous charges - Start

                                'For 1st row as it Returns 0(Zero) for Previous Value, Check if Previous Value is Only Graterthan Zero
                                If i = 0 Then

                                    If TransCharges >= PrevCharges Then
                                        If PrevCharges >= 0 Then

                                            If ChargeErr = Nothing Then
                                                ChargeErr = "\n Present Charges " & TransCharges & " Cannot Greater Or Equal to earlier Charges " & PrevCharges & " "
                                            Else
                                                ChargeErr &= "," & PrevCharges

                                            End If
                                        End If
                                    End If

                                    'For 2nd row onwards as it Returns Charges lableValue for Previous Value, Check if Previous Value is  Graterthan and Equal to Zero
                                Else

                                    If TransCharges > PrevCharges And PrevCharges >= 0 Then
                                        If ChargeErr = Nothing Then
                                            ChargeErr = "\n Present Charges " & TransCharges & " Cannot Greater Or Equal to earlier Charges " & PrevCharges & " "
                                        Else
                                            ChargeErr &= "," & PrevCharges

                                        End If

                                    End If

                                End If
                                'Check if Present Charges are Highe than previous charges - Stop

                                If Not ChargeErr = Nothing Then

                                    ''Dim btnInsert As LinkButton = DirectCast(grdTire.FooterRow.FindControl("lnkAdd"), LinkButton)

                                    ''btnInsert.Attributes.Add(("OnCommand"), "javascript:return showConfirm();")
                                    'Response.Write("<script language='javaScript'>")
                                    'Response.Write("'getMessage()'")
                                    'Response.Write("</script>")
                                    ''Response.Write("<script language='javaScript'>")
                                    ''Response.Write("var name=prompt('" & ChargeErr & "',defaultvalue);")
                                    ''Response.Write("</script>")

                                    ''MsgBox("R u sure?", MsgBoxStyle.YesNo)



                                End If

                            Next
                            'Loop thro the Gridview data - Stop
                            ''Checking for present charges which are higher than previous - Stop

                            'Assign the ChargeErr to MSG
                            Msg &= ChargeErr
                        End If

                    End If

                    If Msg = Nothing Then

                        'Insert in to Data Table
                        Call InsUpdTier(0, TransFrom, TransTo, TransCharges)
                        Call BindGrid()

                    Else

                        Response.Write("<script language='javaScript'>")
                        Response.Write("alert('" & Msg & "',3);")
                        Response.Write("</script>")
                    End If

                Catch ex As Exception

                    'Log Error
                    _Generic.LogError("grdTire_RowCommand - pg_CPSEditCharges", ex.Message)

                Finally
                    GC.Collect(0)

                End Try
            End If
        End Sub
#End Region

#Region "Get Tier IDs for Delete "
        Private Sub DeleteTierIds()
            Try
                Dim RC As Integer = 0
                If grdTire.Rows.Count > 0 Then
                    For RC = 0 To grdTire.Rows.Count - 1
                        Dim TransId As Integer = grdTire.DataKeys(RC).Item(_Helper.ChargeIDCol)
                        InsUpdTier(TransId, 0, 0, 0)
                    Next
                End If
            Catch ex As Exception
                _Generic.LogError("GetIds - pg_CPSEditCharges", ex.ToString)
            End Try
        End Sub
#End Region

#Region "Fixed Charges Save "

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

            InsUpdFixed(0, Round(clsGeneric.NullToDecimal(__FixedCharge.Text), 2, MidpointRounding.AwayFromZero))
            Call GetCurrentCharges()

        End Sub
#End Region

#Region "Function Insert/Update "

        'Created By : Bhanu Teja
        'Purpose    : To Insert And update the tier charges
        'Created On : 12/05/2009

        Private Sub InsUpdTier(ByVal TransId As Integer, ByVal TransFrom As Integer, _
                ByVal TransTo As Integer, ByVal TransCharges As Decimal)

            'Variable Declaration
            Dim Sqlstatement As String = Nothing
            Dim Result As Boolean = False, CatchMessage As String = Nothing

            Try
                Sqlstatement = _Helper.InsUpdTireChargesSQL() & "  " & TransId & "," & __OrgID.Value & "," _
                        & TransFrom & "," & TransTo & "," & TransCharges & "," & _Generic.ss_lngUserID() & " "

                Result = PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMessage, Sqlstatement)


            Catch ex As Exception
                _Generic.LogError("Function InsUpd - pg_CPSEditCharges", CatchMessage)
            End Try

        End Sub

        'Created By : Bhanu Teja
        'Purpose    : To Insert And update the Fixed charges
        'Created On : 12/05/2009

        Private Sub InsUpdFixed(ByVal TransId As Integer, ByVal TransCharges As Decimal)

            'Variable Declaration
            Dim Sqlstatement As String = Nothing
            Dim Result As Boolean = False, CatchMessage As String = Nothing

            Try

                Sqlstatement = _Helper.InsUpdFixedChargesSQL() & "  " & TransId & "," & __OrgID.Value & "," _
                         & TransCharges & "," & _Generic.ss_lngUserID() & " "

                Result = PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                    CatchMessage, Sqlstatement)


            Catch ex As Exception

            End Try
        End Sub

#End Region

#Region " Delete Charges "

        Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Try
                If tr_Tier.Visible = True Then
                    DeleteTierIds()
                    Call BindGrid()
                    rbtlCharges.Enabled = True
                    Call ShowOrHide()
                    btnDelete.Visible = False

                    '' As Charges table is empty disply last/Previous charges for this organization
                    Call FillPreviousCharges()

                ElseIf tr_Fixed.Visible = True Then
                    InsUpdFixed(__FixedChargeID.Value, __FixedCharge.Text)
                    __FixedCharge.Enabled = True
                    rbtlCharges.Enabled = True
                    Call ShowOrHide()
                    btnDelete.Visible = False
                    Call Clear()

                    '' As Charges table is empty disply last/Previous charges for this organization
                    Call FillPreviousCharges()

                Else
                    rbtlCharges.Enabled = False
                    btnDelete.Visible = False
                End If
            Catch ex As Exception


            End Try

        End Sub
#End Region

#Region "Selected Radio Button Change "

        Protected Sub rbtlCharges_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtlCharges.SelectedIndexChanged

            Try
                Call ShowOrHide()

                '' As Charges table is empty disply last/Previous charges for this organization
                Call FillPreviousCharges()

            Catch ex As Exception

                'Log Error
                _Generic.LogError("rbtlCharges_SelectedIndexChanged - pg_CPSEditCharges", ex.Message)

            End Try

        End Sub
#End Region

#Region "Bind Previous Charges "

        'Created By : Bhanu Teja 
        'Purpose    : To Display the last deleted Charges to User
        'Created    : 26/05/2009

        Private Sub FillPreviousCharges()

            'Create Instances
            Dim _DtPreCharges As New DataTable

            Try

                _DtPreCharges = PPS.GetData(_Helper.GetPreviousChargesSQL & "  " & __OrgID.Value & "," _
                    & rbtlCharges.SelectedValue, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

                If _DtPreCharges.Rows.Count > 0 Then
                    grdPreviousCharge.DataSource = _DtPreCharges
                    grdPreviousCharge.DataBind()

                    If rbtlCharges.SelectedValue = Helper.ChargeType.Fixed _
                        And __FixedCharge.Enabled = True Then

                        tc_PreviousCharges.Visible = True

                    ElseIf rbtlCharges.SelectedValue = Helper.ChargeType.Tier _
                        And grdTire.Rows.Count <= 1 Then

                        tc_PreviousCharges.Visible = True

                    Else
                        tc_PreviousCharges.Visible = False
                    End If

                Else
                    tc_PreviousCharges.Visible = False
                End If


            Catch ex As Exception
                'Log Error
                _Generic.LogError("FillPreviousCharges - pg_CPSEditCharges", ex.Message)
            End Try

        End Sub
#End Region

#Region "Gets Staring Tans Number "
        Public Function GetStartVal(ByVal str As Object) As Integer
            Try
                Dim StartNo As Integer = 0
                Dim RowCont As Integer = grdTire.Rows.Count - 1
                StartNo = clsGeneric.NullToInteger(DirectCast(grdTire.Rows(RowCont).FindControl("lblTo"), Label).Text)
                'StartNo = Convert.ToInt32(DirectCast(grdTire.Rows(RowCont).FindControl("lblTo"), Label).Text)
                Return StartNo + 1
            Catch ex As Exception
                'Log Error
                _Generic.LogError("GetStartVal - pg_CPSEditCharges", ex.Message)
            End Try

        End Function
#End Region

#Region "Clear Text "
        Private Sub Clear()
            __FixedCharge.Text = ""
        End Sub
        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Call Clear()
        End Sub
#End Region



    End Class

End Namespace