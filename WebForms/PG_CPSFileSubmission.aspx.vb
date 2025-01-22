Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports MaxPayroll.clsApprMatrix
Imports MaxMiddleware
Imports MaxGeneric
Namespace MaxPayroll

    Partial Class WebForms_PG_CPSFileSubmission
        Inherits clsBasePage
        Private _clsCPSPhase3 As New clsCPSPhase3
        Private _Helper As New Helper
        Private _clsBasePage As New clsBasePage
        Dim OrgID As Integer = 0
        Dim FileId As Integer = 0
        Dim FileName As String = ""
        Dim CatchMsg As String = ""
        Dim Type As String = ""
        Private _clsWrite As New MaxReadWrite.WriteFile
        Private _clsHelper As New MaxReadWrite.CPSHelper
        Private _clsCommon As New MaxReadWrite.Common
#Region "Page Load"




        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            GetQuery()
            Me.btnReset.Visible = False
            'Me.lblnextno.Text = _clsCPSPhase3.ChequeGetNextNo(OrgID)
        End Sub
       

#End Region

#Region "Get Query String"
        Private Sub GetQuery()
            OrgID = Request.QueryString("OrgId")
            FileId = Request.QueryString("FileId")
            FileName = Request.QueryString("FileName")
            Type = Request.QueryString("Type")
        End Sub

#End Region

#Region "Button Event"
        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            
            Try


                If trAuth.Visible = True Then
                    If _clsBasePage.fncValidationCodeProcess(txtAuthCode.Text, lblError.Text) Then
                        CPSInsertChequeNo(FileId)
                        'Call Function replacement here

                        trAuth.Visible = False

                        Exit Try
                    End If
                    lblError.Visible = True
                    
                Else

                    'lbDetailsRecords.Visible = False
                    Me.rblOption.Enabled = False
                    'Me.txtNewBatchNo.Enabled = False
                    'Me.cbChequeNewBatch.Enabled = False

                    Dim dgitem As DataGridItem

                    If trGridDetailRecord.Visible = True Then

                        For Each dgitem In dgRecordwithNo.Items
                            Dim txtNewNo As TextBox = DirectCast(dgitem.FindControl("txtNewChequeNo"), TextBox)
                            txtNewNo.Enabled = False
                        Next


                    End If



                    trAuth.Visible = True
                    lblError.Text = "Please Enter your Validation Code to proceed."
                    lblError.Visible = True
                    btnSubmit.Text = "Confirm and Generate File for Processing"
                End If
            Catch ex As Exception

            End Try
        End Sub

        Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
            Refresh()
        End Sub

        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Response.Redirect("pg_CPS_Cheque_Allocation.aspx?Mode=Submission&subMenu=sub4")
        End Sub

#End Region

#Region "Checkbox Event"




        'Protected Sub cbChequeNewBatch_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbChequeNewBatch.CheckedChanged
        '    If Me.cbChequeNewBatch.Checked = True Then
        '        Me.txtNewBatchNo.Enabled = True
        '        Me.txtNewBatchNo.Text = ""
        '    Else
        '        Me.txtNewBatchNo.Enabled = False
        '        Me.txtNewBatchNo.Text = _clsCPSPhase3.ChequeGetNextNo(OrgID)
        '    End If

        'End Sub
#End Region

#Region "Textbox Event"



        'Protected Sub txtNewBatchNo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewBatchNo.TextChanged
        '    Dim EnterNo As Integer = Me.txtNewBatchNo.Text
        '    Dim dgitem As DataGridItem = Nothing
        '    Dim i As Integer = 0

        '    ''check if cheque no is already in used
        '    If EnterNo < _clsCPSPhase3.ChequeGetNextNo(OrgID) Then
        '        lblError.Text = "Cheque No cannot be less then Next Available Cheque No"
        '        lblError.Visible = True
        '        Me.btnSubmit.Enabled = False
        '    Else
        '        ''populate cheque no in grid
        '        Me.btnSubmit.Enabled = True
        '        For Each dgitem In dgRecordwithNo.Items
        '            Dim txtNewNo As TextBox = DirectCast(dgitem.FindControl("txtNewChequeNo"), TextBox)
        '            txtNewNo.Text = EnterNo + i

        '            i = i + 1
        '        Next

        '        ''on submit, current cheque no set to available
        '    End If
        'End Sub

#End Region

#Region "RadioButton Event"



        Protected Sub rblOption_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblOption.SelectedIndexChanged
            If Me.rblOption.SelectedIndex.ToString = 1 Then
                'Me.trRecordWithNo.Visible = True
                Dim dtRecWithCheque As DataTable



                dtRecWithCheque = PPS.GetData(_clsCPSPhase3.SQL_GetNoRejectedCheque & FileId & "," & OrgID & ",'R','" & Type & "'", _
                    _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

                dgRecordwithNo.DataSource = dtRecWithCheque
                dgRecordwithNo.DataBind()
                trGridDetailRecord.Visible = True
                Me.btnReset.Visible = True
            Else
                Refresh()
            End If
        End Sub
#End Region

#Region "Insert Cheque No."


        Private Function CPSInsertChequeNo(ByVal FileId)
            Try
                ''Replace Damage Start Here
                Dim dtGetRecords As New DataTable

                Dim DgDamageItem As DataGridItem = Nothing
                Dim dgValidation As DataGridItem = Nothing
                Dim ChequeNo As Long = 0
                Dim RecordId As Integer = 1
                Dim PrevChequeNo As Long
                Dim ErrMsg As String = ""
                Dim BatchNo As Integer = 0
                Dim IsExist As Boolean = False
                Dim i As Integer = 1


                Dim RemainingRec As Integer = _clsCPSPhase3.Count_NoOfRec(FileId, Type)

                If dgRecordwithNo.Items.Count <> 0 Then
                    ''Validation Start
                    For Each DgDamageItem In dgRecordwithNo.Items
                        ChequeNo = clsGeneric.NullToLong(DirectCast(DgDamageItem.FindControl("txtNewChequeNo"), TextBox).Text)
                        ''1st Validation, Check if cheque No is in Cheque Available Cheque Table
                        IsExist = PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, "Select Cheque_No from CIMBGW_AvailableCheque" & _
                        "Where Cheque_No = " & ChequeNo & "AND OrgID =" & OrgID & "And Status = 0")

                        If Not IsExist Then
                            ''2nd Validation
                            If ChequeNo <> Nothing And ChequeNo > 0 Then
                                If ChequeNo < _clsCPSPhase3.ChequeGetNextNo(OrgID) Then
                                    lblError.Text = "ERROR : Cheque Number " & ChequeNo & " is in use and pending processing. " & _
                            "Please enter Cheque number greater than " & _clsCPSPhase3.ChequeGetNextNo(OrgID)
                                    lblError.Visible = True
                                    btnSubmit.Enabled = False
                                    Return False
                                    Exit Try
                                End If
                            End If
                        End If
                        ''3rd Validation. Check if No duplicate cheque No entered. Loop in loop
                        Dim j As Integer = 1
                        For Each dgValidation In dgRecordwithNo.Items
                            Dim CompareChequeNo As Long = 0
                            CompareChequeNo = clsGeneric.NullToLong(DirectCast(dgValidation.FindControl("txtNewChequeNo"), TextBox).Text)
                            If ChequeNo = CompareChequeNo And i <> j Then
                                If ChequeNo <> 0 And CompareChequeNo <> 0 Then
                                    lblError.Text = "ERROR : Cheque No row " & i & " is same as Cheque No row " & j
                                    btnSubmit.Enabled = False
                                    Return False
                                    Exit Try
                                End If
                            End If
                            j += 1
                        Next
                        i += 1
                    Next
                    ''Validation Stop

                    For Each DgDamageItem In dgRecordwithNo.Items
                        If RemainingRec > 0 Then
                            ChequeNo = clsGeneric.NullToLong(DirectCast(DgDamageItem.FindControl("txtNewChequeNo"), TextBox).Text)
                            If ChequeNo <> Nothing And ChequeNo > 0 Then

                                ''Update  CIMBGW_ChequeNo AND DIVIDEND TABLE with replace Cheque No - Start
                                ''Get Previous Cheque No/Batch Start

                                dtGetRecords = PPS.GetData("Select Cheque_No,Cheque_BatchNo from CIMBGW_ChequeNo WHERE RecId =" & RecordId & "AND FileId =" & FileId & _
                                "AND OrgId = " & OrgID, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)



                                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, _clsCPSPhase3.SQL_UpdateChequeNumber & _
                                ChequeNo & "," & RecordId & "," & FileId & ",'" & Type & "'")
                                ''Update  CIMBGW_ChequeNo with replace Cheque No - Stop

                                If dtGetRecords.Rows.Count > 0 Then
                                    ''Get Previous Cheque No/Batch Start
                                    PrevChequeNo = dtGetRecords.Rows(0)("Cheque_No")
                                    BatchNo = dtGetRecords.Rows(0)("Cheque_BatchNo")
                                    ''Get Previous Cheque No/Batch Stop

                                    ''Set the Damage to available for future use Start
                                    PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMsg, "INSERT INTO CIMBGW_AvailableCheque (OrgId,Cheque_No,Batch_No)" & _
                                        "VALUES (" & OrgID & "," & PrevChequeNo & "," & BatchNo & ")")
                                    ''set the damage to available for future use Stop
                                End If
                                ''Insert Into Dividend Table 


                                RemainingRec -= -1
                                RecordId += 1

                            End If
                        End If

                    Next
                End If

                ''Replace Damage Ends Here
            Catch ex As Exception
                Return False
            End Try
            ''Update tpgt_filedetail process flag to 101



            ''Write TO Files Start
            If Type = _clsCPSPhase3.TypeWEB_Name Then
                lblError.Text = ""
                lblError.Text = _clsWrite.WriteToFile(OrgID, FileId)
            End If
            If lblError.Text Is Nothing Then
                lblError.Text = "STATUS: Cheque Number has been confirmed and out file successfully generated."
                _clsCPSPhase3.UpdateStatus_tpgtFileDetails(clsCPSPhase3.enmCPSFileStatus.Cheque_No_Confirm, FileId, Type)
            End If

            '' Write to files end


            lblError.Visible = True
            btnSubmit.Enabled = False
            btnReset.Enabled = False
            Return True
        End Function
#End Region

#Region "Refresh page"
        Private Sub Refresh()
            Response.Redirect("PG_CPSFileSubmission.aspx?OrgId=" & OrgID & "&FileId=" & FileId)
        End Sub
#End Region



    End Class
End Namespace