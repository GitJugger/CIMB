Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports MaxPayroll.clsApprMatrix
Imports MaxMiddleware
Imports MaxGeneric
Namespace MaxPayroll
    '****************************************************************************************************
    'PageName       : PG_CPSAssignChequeNo
    'Purpose        : Cheque Number Allocation for CIMBGW CPS Phase III
    'Author         : Mohamad Hafeez Zakaria 
    'Created        : 2009-06-01
    '*****************************************************************************************************

    Partial Class PG_CPSAssignChequeNo
        Inherits clsBasePage
        Private _Helper As New Helper
        Private _clsCPSPhase3 As New clsCPSPhase3
        Private _clsBasePage As New clsBasePage
        Private _clscommon As New clsCommon
        Dim OrgID As Integer = 0
        Dim FileId As Integer = 0
        Dim Type As String = ""
        Dim CatchMsg As String = ""

#Region "Page Load"
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                Dim Count_SkipCheque As New DataTable


                GetQuery()  ''Get OrgId and FileID From URL

                Clear() ''Clear all Labels
                'Payment Date Minimum Value


                lblReusable.Text = Count_GetReusableCheque(FileId, OrgID, "C", "") ''Display No of Reusable Cheque

                'Show Details Link - Start
                If lblReusable.Text <> 0 Then
                    Me.lbDetails.Visible = True
                End If
                'Show Details Link - Stop

                lblNoofRecords.Text = _clsCPSPhase3.Count_NoOfRec(FileId, Type)

                ''Display Next Cheque Number-Start
                If cbUseCustomCheque.Checked = False Then
                    txtChequeNo.Text = _clsCPSPhase3.fnFiller(_clsCPSPhase3.ChequeGetNextNo(OrgID), 1, 6, True)
                End If
                ''Display Next Cheque Number-Stop


            Catch ex As Exception

            End Try


        End Sub

#End Region

#Region "Get Query String - Remain"
        Private Sub GetQuery()
            OrgID = Request.QueryString("OrgId")
            FileId = Request.QueryString("FileId")
            Type = Request.QueryString("Type")
        End Sub

#End Region

#Region "Checkbox Event - Remain"
        ''Custom Cheque No Check Box Control - Start
        Public Sub checked_clicked(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim cb As CheckBox
            cb = CType(sender, CheckBox)

            If cb.Checked = True Then
                txtChequeNo.Enabled = True
                txtChequeNo.Text = ""
            Else
                txtChequeNo.Enabled = False
                txtChequeNo.Text = _clsCPSPhase3.fnFiller(_clsCPSPhase3.ChequeGetNextNo(OrgID), 1, 6, True)
            End If
            Clear()
        End Sub
        ''Custom Cheque No Check Box Control - Stop
#End Region

        '#Region "Count No. Of Records - Remain"

        '        Private Function Count_NoOfRec(ByVal fileID As Integer)
        '            Dim NoOfRecords As Integer

        '            ''Execute CIMBGW_CPSCountNumberRecords Start
        '            NoOfRecords = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
        '                CatchMsg, _clsCPSPhase3.SQL_CountNumberRecords & fileID & ",'C'," & _clsCPSPhase3.DChequeNo)
        '            Return NoOfRecords
        '        End Function


        '#End Region

#Region "Get Reusable Cheque"
        Private Function Count_GetReusableCheque(ByVal FileID As Integer, ByVal OrgId As Integer, ByVal COption As Char, _
                                            ByVal ColumnName As String)
            Dim NoRecords As String
            ColumnName = "''"
            ''Move to SP
            NoRecords = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                            CatchMsg, "SELECT Count(*) from CIMBGW_AvailableCheque Where OrgID =" & OrgId & "And Status = 1")
            Return NoRecords


        End Function

        



#End Region

#Region "Insert Cheque Number"

        Public Function CPSInsertChequeNo(ByVal FileID As Integer)

            '-- Cheque AVAILABLE Status 
            '--W - Waiting For Cheque Allocation
            '--P - Pending For Submission
            '--A - Approve for processing
            '--R - Rejected by Customer
            '--E - Stale/ Expired Cheque 

            ''Declare Instances - Start
            Dim dtGridCheque As New DataTable
            Dim DgItem As DataGridItem = Nothing
            Dim DgDamageItem As DataGridItem = Nothing
            ''Declare Instances Stop

            ''Declare Variable - Start
            Dim ChequeNo As Long
            Dim BatchNo As Integer
            Dim i As Integer = 0
            Dim cb As CheckBox
            Dim RemainingRec As Integer = _clsCPSPhase3.Count_NoOfRec(FileID, Type)
            Dim RecordId As Integer = 1
            Dim FileName As String = "", ErrorMsg As String = ""
            Dim IsCutoff As Boolean = False
            ''Declare Variable - Stop

            Try

                IsCutoff = _clscommon.fncCutoffTime(_Helper.CPS_Name, OrgID, 5, Now.TimeOfDay.ToString, _
                                    Day(txtPayDate.Value), Month(txtPayDate.Value), Year(txtPayDate.Value), "C")

                If IsCutoff Then
                    lblError.Text = "ERROR: Cut off time for CPS Submission exceeded. Please select future date to proceed."
                    Disable_Button()
                    Return False
                    Exit Try
                End If

                ''Check any Error Input by Customer - Start


                If txtPayDate.Value < Format(Today.Date, "dd/MM/yyyy") Then
                    lblError.Text = "ERROR : Payment Date cannot be less than today's date"
                    Disable_Button()
                    Return False
                    Exit Try
                End If

                If Count_CBSelected() > _clsCPSPhase3.Count_NoOfRec(FileID, Type) Then
                    lblError.Text = "ERROR : No. of selected cheque number cannot be more than no of records"
                    Disable_Button()
                    Return False
                    Exit Try
                End If

                If cbUseCustomCheque.Checked = True Then
                    ChequeNo = MaxGeneric.clsGeneric.NullToLong(txtChequeNo.Text)
                    If ChequeNo < _clsCPSPhase3.ChequeGetNextNo(OrgID) Or _clsCPSPhase3.CheckDuplicate(ChequeNo, OrgID, _clsCPSPhase3.GetBatchNo(OrgID)) = False Then
                        lblError.Text = "ERROR : Cheque Number " & ChequeNo & " is in use and pending processing. " & _
                            "Please enter Cheque number greater than " & _clsCPSPhase3.ChequeGetNextNo(OrgID) & _
                                ". Press 'Cancel' to continue."
                        Disable_Button()
                        Return False
                        Exit Try
                    End If
                End If
                ''Check any Error Input by Customer -  Stop

                ''Check Payment Date < Current Date Start
                If txtPayDate.Value < Today Then
                    lblError.Text = "ERROR : Payment Date Cannot be Past Date"
                    Disable_Button()
                    Return False
                    Exit Try
                End If
                ''Check Payment Date < Current Date Stop

                ''If User Select Reusable Cheque Number - Start.
                If dgRejectedCheque.Items.Count <> 0 Then

                    For Each DgItem In dgRejectedCheque.Items
                        If RemainingRec > 0 Then


                            cb = DirectCast(DgItem.FindControl("cbChequeNo"), CheckBox)
                            If cb.Checked Then
                                ChequeNo = DgItem.Cells(i).Text
                                BatchNo = DgItem.Cells(i + 1).Text
                                ''Insert here
                                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, _clsCPSPhase3.SQL_InsertChequeNumber _
                                                            & FileID & "," & RecordId & "," & ChequeNo & "," & OrgID & "," & _
                                                               BatchNo & ",'P','" & Type & "'")
                                ''Update reusable status to false
                                If CatchMsg = Nothing Then
                                    PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, _
                                        "UPDATE CIMBGW_AvailableCheque SET Status = 0 where OrgId =" & OrgID & " and Cheque_No =" & ChequeNo & " and Batch_No =" & BatchNo)
                                End If

                                RemainingRec = RemainingRec - 1
                                RecordId = RecordId + 1
                            End If

                        End If

                        If RemainingRec = 0 Then Exit For
                    Next
                End If
                ''If User Select Reusable Cheque Number - Stop.



                ''Auto-Populate Cheque Number - Start
                If RemainingRec <> 0 Then

                    ''Check If User Use Auto Number - Start
                    If cbUseCustomCheque.Checked = False Then

                        ''Autopopulate from last cheque number
                        ChequeNo = _clsCPSPhase3.ChequeGetNextNo(OrgID)

                        ''Get Number of Transaction from tpgt_FileDetails
                        i = 0
                        Do Until i = RemainingRec


                            'Dim Duplicate As Long = 0
                            'Duplicate = MaxGeneric.clsGeneric.NullToLong(PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                            '                                            _clsCPSPhase3.SQL_MiniStatement & FileID & "," & clsCPSPhase3.enmMiniStatement.GetDuplicateChequeNo & "," & ChequeNo))
                            If _clsCPSPhase3.CheckDuplicate(ChequeNo, OrgID, _clsCPSPhase3.GetBatchNo(OrgID)) Then

                                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, _clsCPSPhase3.SQL_InsertChequeNumber _
                                & FileID & "," & RecordId & "," & ChequeNo & "," & OrgID & "," & _
                                    _clsCPSPhase3.GetBatchNo(OrgID) & ",'P','" & Type & "'")

                                'RemainingRec = RemainingRec - 1
                                RecordId = RecordId + 1
                                i += 1
                            Else
                                If i = 0 Then
                                    i = 0
                                Else
                                    i -= 1
                                End If

                            End If
                            ChequeNo = ChequeNo + 1

                        Loop
                        ''Check If User Use Auto Number - Stop

                    Else
                        ''Check If User Use Custom Number - Start
                        ChequeNo = MaxGeneric.clsGeneric.NullToLong(txtChequeNo.Text) ''Get Customer Input Number
                        Dim No_of_Skip_Cheque As Long
                        No_of_Skip_Cheque = ChequeNo - _clsCPSPhase3.ChequeGetNextNo(OrgID)
                        


                        ' ''PerformanceStart
                        For i = 0 To RemainingRec - 1
                            Dim LastChequeNumberUsed As Long = _clsCPSPhase3.ChequeGetNextNo(OrgID)
                            'Dim q As Long = 0
                            'Dim SkipUnion As Boolean = False
                            'Dim SQLSkipStatement As String = ""




                            ''Insert All skip no into Available Table-Start
                            If ChequeNo > LastChequeNumberUsed Then

                                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, _clsCPSPhase3.SQL_InsSkipChequeNo _
                                 & _clsCPSPhase3.ChequeGetNextNo(OrgID) & "," & No_of_Skip_Cheque & "," & OrgID & "," & _clsCPSPhase3.GetBatchNo(OrgID))

                                'SQLSkipStatement = "INSERT INTO CIMBGW_AvailableCheque (OrgId,Cheque_No,Batch_No)" & Environment.NewLine
                                'For q = LastChequeNumberUsed To ChequeNo - 1
                                '    If SkipUnion = True Then
                                '        SQLSkipStatement += "UNION ALL" & Environment.NewLine
                                '    End If
                                '    SQLSkipStatement += "SELECT " & OrgID & "," & q & "," & _clsCPSPhase3.GetBatchNo(OrgID) & Environment.NewLine

                                '    SkipUnion = True
                                'Next
                                'PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, SQLSkipStatement)

                            End If
                            ''Insert All skip no into Available Table-Stop
                            If ChequeNo = 1000000 Then
                                ChequeNo = 1
                            End If

                            PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, _clsCPSPhase3.SQL_InsertChequeNumber _
                            & FileID & "," & RecordId & "," & ChequeNo & "," & OrgID & "," & _clsCPSPhase3.GetBatchNo(OrgID) & ",'P','" & Type & "'")

                            RemainingRec = RemainingRec - 1
                            ChequeNo = ChequeNo + 1
                            RecordId = RecordId + 1
                        Next
                        ' ''Performance End


                        ''Check If User Use Custom Number - Stop
                    End If


                End If
                FileName = PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, ErrorMsg, _
                                            _clsCPSPhase3.SQL_MiniStatement & FileID & "," & clsCPSPhase3.enmMiniStatement.GetFileGivenName)
                lblError.Text = "STATUS : Cheque Number for " & FileName & " is successfully allocated."
                ''Update tpgpt_FileDetails & Update Respected Table with Payment Date

                _clsCPSPhase3.UpdateStatus_tpgtFileDetails(clsCPSPhase3.enmCPSFileStatus.Cheque_No_Assign, FileID, Type, txtPayDate.Value)




                lblError.Visible = True
                btnSubmit.Enabled = False
                btnReset.Enabled = False
                lbDetails.Visible = False
                txtPayDate.Disabled = True
                txtChequeNo.Enabled = False

                Return True

            Catch ex As Exception
                Return False
            End Try

        End Function

#End Region

#Region "Events"

#Region "Checkbox Reuse All No."



        ''Checkbox Reuse All Reject/Skip Cheque Number - Start
        Protected Sub cbRejectNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbRejectNo.CheckedChanged
            Dim cb As CheckBox
            cb = CType(sender, CheckBox)
            Dim dgitem As DataGridItem
            If cb.Checked = True Then
                For Each dgitem In dgRejectedCheque.Items
                    Dim chMain As CheckBox = DirectCast(dgitem.FindControl("cbChequeNo"), CheckBox)
                    chMain.Checked = True

                Next
            Else

                For Each dgitem In dgRejectedCheque.Items
                    Dim chMain As CheckBox = DirectCast(dgitem.FindControl("cbChequeNo"), CheckBox)
                    chMain.Checked = False

                Next
            End If
        End Sub
        ''Checkbox Reuse All Reject/Skip Cheque Number - Stop
#End Region

#Region "Button Submit - Events"


        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            ''Declare Instances - Start
            Dim dgitem As DataGridItem = Nothing
            ''Declare Instances - Stop
            Dim clsCommon As New MaxPayroll.clsCommon
            Try

                If txtPayDate.Value = "" Then
                    lblError.Text = "Please Enter a valid Payment date (dd/mm/yyyy)"
                    lblError.Visible = True
                    Exit Try
                End If
                If trAuth.Visible = True Then
                    If txtAuthCode.Text IsNot Nothing Then
                        Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtAuthCode.Text)
                        If strEncUsername = False Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                    End If

                    If _clsBasePage.fncValidationCodeProcess(txtAuthCode.Text, lblError.Text) Then ''Check Validation Code
                        CPSInsertChequeNo(FileId)
                        trAuth.Visible = False

                        Exit Try
                    End If
                    lblError.Visible = True
                    lbDetails.Visible = False
                Else

                    If trGrid.Visible = True Then
                        For Each dgitem In dgRejectedCheque.Items
                            Dim chMain As CheckBox = DirectCast(dgitem.FindControl("cbChequeNo"), CheckBox)
                            chMain.Enabled = False
                        Next
                        cbRejectNo.Enabled = False
                    End If


                    cbUseCustomCheque.Enabled = False
                    lbDetails.Visible = False
                    trAuth.Visible = True
                    lblError.Text = "Please Enter your Validation Code to confirm Cheque Number Allocation Cheque Number. "
                    lblError.Visible = True
                    btnSubmit.Text = "Confirm"
                End If
            Catch ex As Exception

            End Try
        End Sub
#End Region

#Region "Button Reset - Events"

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            Response.Redirect("PG_CPSAssignChequeNo.aspx?OrgId=" & OrgID & "&FileId=" & FileId)
        End Sub
#End Region

#Region "Label Hyperlinks Details"
        Protected Sub lbDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbDetails.Click
            Dim dtReuseCheque As DataTable
            Dim dtfiller As New DataTable
            Dim dtSkipChequeNo As New DataTable

            Dim i As Integer = 0
            Dim j As Integer = 0
            dtfiller.Columns.Add("CHEQUE_NO")
            dtfiller.Columns.Add("BATCH_NO")


            dtReuseCheque = PPS.GetData(_clsCPSPhase3.SQL_GetNoRejectedCheque & FileId & "," & OrgID & ",'','" & Type & "'", _
                _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

            For i = 0 To dtReuseCheque.Rows.Count - 1
                dtfiller.Rows.Add()
                dtfiller.Rows(i)("CHEQUE_NO") = _clsCPSPhase3.fnFiller(dtReuseCheque.Rows(i)("CHEQUE_NO"), 1, 6, True)
                dtfiller.Rows(i)("BATCH_NO") = dtReuseCheque.Rows(i)("BATCH_NO")

            Next
            Me.trRejectCheque.Visible = True

            dgRejectedCheque.DataSource = dtfiller
            dgRejectedCheque.DataBind()
            trGrid.Visible = True
        End Sub
#End Region

#End Region

#Region "Calculate number of Checkbox Selected"
        Private Function Count_CBSelected()
            Dim Count As Integer
            Dim dgCount As DataGridItem = Nothing
            Dim cb As CheckBox

            For Each dgCount In dgRejectedCheque.Items
                cb = DirectCast(dgCount.FindControl("cbChequeNo"), CheckBox)
                If cb.Checked = True Then
                    Count = Count + 1
                End If

            Next

            Return Count
        End Function
#End Region

#Region "Clear"
        Private Sub Clear()
            If Me.lblError.Text <> "" Then
                Me.lblError.Text = ""
                Me.lblError.Visible = False
            End If
        End Sub
#End Region

#Region "Disable Button"
        Private Sub Disable_Button()
            lblError.Visible = True
            lbDetails.Visible = False
            btnSubmit.Enabled = False
            txtChequeNo.Enabled = False
            cbUseCustomCheque.Enabled = False
        End Sub
#End Region




        
        
        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Response.Redirect("pg_CPS_Cheque_Allocation.aspx?Mode=Allocation&subMenu=sub4")

        End Sub
    End Class
End Namespace