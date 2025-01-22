Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsApprMatrix
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports MaxGeneric


Namespace MaxPayroll
    Partial Class PG_MandatesDetails
        Inherits clsBasePage

#Region "Proprties"
        Private _SessionMainID As String = "MID"
        Public Property SessionMID() As Integer          ''Teja
            Get
                Return clsGeneric.NullToInteger( _
                    HttpContext.Current.Session(_SessionMainID))
            End Get
            Set(ByVal value As Integer)
                HttpContext.Current.Session(_SessionMainID) = value
            End Set
        End Property
        Private Property RecID() As Long
            Get
                If IsNumeric(ViewState("RecID")) Then
                    Return CLng(ViewState("RecID"))
                Else
                    Return 0
                End If
            End Get
            Set(ByVal value As Long)
                ViewState("RecID") = value
            End Set
        End Property
        Private Property OldValue() As clsMandates
            Get
                Try
                    Return DirectCast(Session("OldValue"), clsMandates)
                Catch
                    Return New clsMandates
                End Try
            End Get
            Set(ByVal value As clsMandates)
                Session("OldValue") = value
            End Set
        End Property
        Private ReadOnly Property rq_AccNo() As String
            Get
                Return Request.QueryString("AccNo") & ""
            End Get
        End Property
        Private ReadOnly Property rq_BankOrgCode() As String
            Get
                Return Request.QueryString("BankOrgCode") & ""
            End Get
        End Property
        Private ReadOnly Property rq_RefNo() As String
            Get
                Return Request.QueryString("RefNo") & ""
            End Get
        End Property
        Private ReadOnly Property rq_OrgID() As Integer
            Get
                Return Request.QueryString("ID")
            End Get
        End Property
        Private ReadOnly Property rq_PageMode() As enmPageMode
            Get
                Try
                    Return CType(Request.QueryString("PageMode"), enmPageMode)
                Catch
                    Return enmPageMode.NewMode
                End Try
            End Get
        End Property

#End Region

        Dim lngUserId As Long

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try
                If ss_strUserType <> gc_UT_BankUser AndAlso ss_strUserType <> gc_UT_BankAuth Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                If Page.IsPostBack = False Then
                    Dim oItem As New clsMandates
                    SessionMID = HttpContext.Current.Request.QueryString("MID")   ''Teja
                    oItem.BindDDLFrequency(ddlFrequency)
                    lblHeading.Text = "Mandate Details (" & rq_OrgID & ")"
                    Select Case rq_PageMode
                        Case enmPageMode.NewMode
                            btnSave.Text = enmButton.Save.ToString
                            rdStatus.Items.Add(New ListItem("Active", "A"))
                            rdStatus.Items.Add(New ListItem("Inactive", "I"))
                            rdStatus.SelectedValue = "A"
                            rdStatus.Enabled = False
                        Case enmPageMode.EditMode
                            oItem = oItem.Load(rq_OrgID, rq_RefNo, rq_AccNo, rq_BankOrgCode)
                            btnSave.Text = enmButton.Update.ToString
                            btnBack.Visible = True

                            RecID = oItem.paramRecID

                            txtBnkOrgCode.Text = oItem.paramBankOrgCode
                            txtLimitAmt.Text = oItem.paramLimitAmountDisplay
                            txtFrequencyLimit.Text = oItem.paramFrequencyLimit
                            ddlFrequency.SelectedValue = oItem.paramFrequency
                            txtRefNo.Text = oItem.paramRefNo
                            txtCustName.Text = oItem.paramCustomerName
                            txtAccNo.Text = oItem.paramAccNo
                            txtRefNo.Enabled = False
                            txtAccNo.Enabled = False
                            txtBnkOrgCode.Enabled = False
                            'hafeez
                            rdStatus.Items.Add(New ListItem("Active", "A"))
                            rdStatus.Items.Add(New ListItem("Inactive", "I"))
                            rdStatus.SelectedValue = oItem.paramTheStatus
                            'rdStatus.Enabled = False
                            'end hafeez
                            Select Case oItem.paramStatus
                                Case enmMandateStatus.Pending
                                    lblMessage.Text = "Current record is pending for authorization."
                                    Me.prcDisableUI()
                                    Me.btnSave.Enabled = False
                                    Me.btnReset.Disabled = True
                                Case enmMandateStatus.Reject
                                    lblMessage.Text = "Current record is rejected."
                                    Me.prcDisableUI()
                                    Me.btnSave.Enabled = False
                                    Me.btnReset.Disabled = True
                            End Select

                        Case Else
                            If Request.QueryString("RecType") = "New" Then
                                oItem = oItem.Load(rq_OrgID, rq_RefNo, rq_AccNo, rq_BankOrgCode)
                                RecID = oItem.paramRecID

                                txtBnkOrgCode.Text = oItem.paramBankOrgCode
                                txtLimitAmt.Text = oItem.paramLimitAmountDisplay
                                txtFrequencyLimit.Text = oItem.paramFrequencyLimit
                                ddlFrequency.SelectedValue = oItem.paramFrequency
                                txtRefNo.Text = oItem.paramRefNo
                                txtCustName.Text = oItem.paramCustomerName
                                txtAccNo.Text = oItem.paramAccNo
                                txtRefNo.Enabled = False
                                txtAccNo.Enabled = False
                                txtBnkOrgCode.Enabled = False

                                rdStatus.Items.Add(New ListItem("Active", "A"))
                                rdStatus.Items.Add(New ListItem("Inactive", "I"))

                                rdStatus.SelectedValue = oItem.paramTheStatus
                                rdStatus.Enabled = False
                            Else
                                Dim oNewItem As New clsMandates

                                oNewItem = oNewItem.LoadTemp(rq_OrgID, rq_RefNo, rq_AccNo, rq_BankOrgCode)
                                oItem = oItem.Load(rq_OrgID, rq_RefNo, rq_AccNo, rq_BankOrgCode)



                                RecID = oItem.paramRecID

                                txtBnkOrgCode.Text = oItem.paramBankOrgCode
                                txtLimitAmt.Text = oItem.paramLimitAmountDisplay
                                txtFrequencyLimit.Text = oItem.paramFrequencyLimit
                                ddlFrequency.SelectedValue = oItem.paramFrequency
                                txtRefNo.Text = oItem.paramRefNo
                                txtCustName.Text = oItem.paramCustomerName
                                txtAccNo.Text = oItem.paramAccNo
                                txtRefNo.Enabled = False
                                txtAccNo.Enabled = False
                                txtBnkOrgCode.Enabled = False
                                rdStatus.Items.Add(New ListItem("Active", "A"))
                                rdStatus.Items.Add(New ListItem("Inactive", "I"))

                                rdStatus.SelectedValue = oItem.paramTheStatus
                                rdStatus.Enabled = False
                                

                                If oItem.paramLimitAmountDisplay <> oNewItem.paramLimitAmountDisplay Then
                                    lblNewLimitAmt.Text = "New Value: " & oNewItem.paramLimitAmountDisplay
                                End If
                                If oItem.paramFrequencyLimit <> oNewItem.paramFrequencyLimit Then
                                    lblNewFrequencyLimit.Text = "New Value: " & oNewItem.paramFrequencyLimit
                                End If
                                If oItem.paramFrequency <> oNewItem.paramFrequency Then
                                    lblNewFrequency.Text = "New Value: " & oNewItem.fncGetFrequencyDesc(oNewItem.paramFrequency)
                                End If
                                If oItem.paramCustomerName <> oNewItem.paramCustomerName Then
                                    lblNewCustName.Text = "New Value: " & oNewItem.paramCustomerName
                                End If
                                If oItem.paramTheStatus <> oNewItem.paramTheStatus Then
                                    If oNewItem.paramTheStatus = "I" Then
                                        lblNewStatus.Text = "New Value: Inactive"
                                    Else
                                        lblNewStatus.Text = "New Value: Active"
                                    End If


                                End If
                            End If
                            prcDisableUI()
                            btnReset.Visible = False
                            btnSave.Visible = False
                            btnBack.Visible = True
                    End Select
                    ''Teja
                    ViewState("bankOrgCode") = txtBnkOrgCode.Text
                    ViewState("LimitAmt") = txtLimitAmt.Text
                    ViewState("FrequencyLimit") = txtFrequencyLimit.Text
                    ViewState("ddlFrequency") = ddlFrequency.SelectedValue
                    ViewState("RefNo") = txtRefNo.Text
                    ViewState("CustName") = txtCustName.Text
                    ViewState("AccNo") = txtAccNo.Text
                    ViewState("Status") = rdStatus.Text
                    ''
                End If

            Catch
                LogError("Page Load - PG_ApprMatrix")
            Finally

            End Try

        End Sub

#End Region

        Private Sub prcDisableFields()
            txtRefNo.ReadOnly = True
            txtAccNo.ReadOnly = True
            txtBnkOrgCode.ReadOnly = True
        End Sub

        Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
            Dim clsApprMatrix As New clsApprMatrix
            Dim lngTrnxId As Long = 0
            Dim countfieldchanges As Long = 0

            Dim clsCommon As New clsCommon
            Try
                Select Case btnSave.Text
                    Case enmButton.Save.ToString, enmButton.Update.ToString
                        Dim oItem As clsMandates

                        Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtRefNo.Text)
                        If strEncUsername = False Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                        Dim folder As Boolean = clsCommon.CheckScriptValidation(txtAccNo.Text)
                        If folder = False Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                        Dim errorfolder As Boolean = clsCommon.CheckScriptValidation(txtCustName.Text)
                        If errorfolder = False Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                        Dim txtLimitAmt1 As Boolean = clsCommon.CheckScriptValidation(txtLimitAmt.Text)
                        If txtLimitAmt1 = False Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                        Dim txtFrequencyLimit1 As Boolean = clsCommon.CheckScriptValidation(txtFrequencyLimit.Text)
                        If txtFrequencyLimit1 = False Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                        If fncValidation() Then
                            oItem = New clsMandates
                            oItem.paramRecID = RecID
                            oItem.paramCustomerName = txtCustName.Text
                            oItem.paramRefNo = txtRefNo.Text
                            oItem.paramOrgID = rq_OrgID
                            oItem.paramBankOrgCode = txtBnkOrgCode.Text
                            oItem.paramLimitAmount = txtLimitAmt.Text
                            oItem.paramAccNo = txtAccNo.Text
                            oItem.paramFrequency = ddlFrequency.SelectedValue
                            oItem.paramFrequencyLimit = txtFrequencyLimit.Text
                            oItem.paramDoneBy = ss_lngUserID
                            oItem.paramFileID = 0
                            oItem.paramTheStatus = rdStatus.SelectedValue

                            Select Case rq_PageMode
                                Case enmPageMode.NewMode
                                    lngTrnxId = oItem.Insert(oItem)
                                    If lngTrnxId > 0 Then
                                        clsApprMatrix.prcApprovalMatrix(rq_OrgID, ss_lngUserID, "INSERT", lngTrnxId, ss_lngUserID, 0, lngTrnxId, "Mandate Record(Ref No: " & oItem.paramRefNo & " with A/C No.: " & oItem.paramAccNo & ") Approval", "Mandate Record Creation", "", 1)
                                        Call clsCommon.prcSendMails("BANK AUTH", 100000, ss_lngUserID, 0, "Mandate Record Creation", "New mandate record has created for approval.", 0)
                                        prcDisableUI()
                                        lblMessage.Text = "New mandate record has created successfully."
                                        btnSave.Text = mdConstant.GetButtonName(enmButton.Create_New)
                                        btnReset.Disabled = True
                                        btnBack.Visible = True
                                    Else
                                        lblMessage.Text = "New mandate record's creation has failed."
                                    End If
                                Case enmPageMode.EditMode
                                    oItem.UpdateModifyStatusById(RecID, ss_lngUserID)

                                    '' lngTrnxId = oItem.InsertMandateHistory(oItem)

                                    lngTrnxId = oItem.InsertTemp(oItem)

                                    Call CheckChange()  ''Teja

                                    If lngTrnxId > 0 Then
                                        clsApprMatrix.prcApprovalMatrix(rq_OrgID, ss_lngUserID, "INSERT", lngTrnxId, ss_lngUserID, 0, lngTrnxId, "Mandate Record(Ref No: " & oItem.paramRefNo & " with A/C No.: " & oItem.paramAccNo & ") Approval", "Mandate Record Modification", "", 1)
                                        Call clsCommon.prcSendMails("BANK AUTH", 100000, ss_lngUserID, 0, "Mandate Record Modification", "Mandate record has been modified, pending for approval.", 0)
                                        prcDisableUI()
                                        btnSave.Enabled = False
                                        btnReset.Disabled = True
                                        lblMessage.Text = "Mandate record has modified successfully. Awaiting for authorization from " & gc_UT_BankAuthDesc & "."
                                    Else
                                        lblMessage.Text = "Mandate record's modification has failed."
                                    End If
                            End Select
                        End If
                    Case mdConstant.GetButtonName(enmButton.Create_New)
                        prcInitUI()
                        prcDisableUI(True)
                        btnSave.Text = enmButton.Save.ToString
                        btnReset.Disabled = False
                        btnBack.Visible = False
                        rdStatus.Enabled = False
                End Select
            Catch ex As Exception
                Dim a As String = ex.Message

            End Try

        End Sub

#Region "Check for Change in Fields"        ''Teja
        Private Sub CheckChange()

            If ViewState("LimitAmt") <> txtLimitAmt.Text Then
                Call InsertHistory("LimitAmount", ViewState("LimitAmt"), txtLimitAmt.Text)
            End If
            If ViewState("FrequencyLimit") <> txtFrequencyLimit.Text Then
                Call InsertHistory("FrequencyLimit", ViewState("FrequencyLimit"), txtFrequencyLimit.Text)
            End If
            If ViewState("ddlFrequency") <> ddlFrequency.SelectedValue Then
                Call InsertHistory("Frequency", ViewState("ddlFrequency"), ddlFrequency.SelectedValue)
            End If
            If ViewState("CustName") <> txtCustName.Text Then
                Call InsertHistory("CustomerName", ViewState("CustName"), txtCustName.Text)
            End If
            If ViewState("Status") <> rdStatus.Text Then
                Call InsertHistory("Status", ViewState("Status"), rdStatus.Text)
            End If


        End Sub
#End Region

#Region "Insert In to History"      ''Teja
        'tCor_TempMandatesDetails
        Public Function InsertHistory(ByVal FieldName As String, _
                ByVal Oldval As String, ByVal newval As String) As Long
            Dim lngRetVal As Long = 0
            Dim strSQL As String = "pg_InstMandatesHistory"
            Dim params(7) As SqlParameter
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id
            ' ''hafeez start
            Dim SQLGetFileID As String = "SELECT FileId FROM tcor_MandatesDetails where ID =" & RecID
            Dim FileID As Integer = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.Text, SQLGetFileID)
            ' ''hafeez end
            Try
                params(0) = New SqlParameter("@MandateID", SessionMID)
                params(1) = New SqlParameter("@OrgId", rq_OrgID)
                params(2) = New SqlParameter("@FileId", FileID)
                params(3) = New SqlParameter("@FieldName", FieldName)
                params(4) = New SqlParameter("@OldValue", Oldval)
                params(5) = New SqlParameter("@NewValue", newval)
                params(6) = New SqlParameter("@ModifyBy", lngUserId)
                params(7) = New SqlParameter("@Approve", "P") 'P - Pending, A - Approved, R - Rejected'

                lngRetVal = SqlHelper.ExecuteScalar(sSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Catch ex As Exception
                'ErrorLog("clsMandates.InsertTemp", Err.Number, ex.Message)
                lngRetVal = 0
            End Try
            Return lngRetVal
        End Function
#End Region


        Private Sub prcInitUI()
            lblMessage.Text = ""
            ddlFrequency.SelectedIndex = 0
            txtRefNo.Text = ""
            txtLimitAmt.Text = ""
            txtFrequencyLimit.Text = ""
            txtCustName.Text = ""
            txtBnkOrgCode.Text = ""
            txtAccNo.Text = ""
        End Sub


        Private Sub prcDisableUI(Optional ByVal ToEnable As Boolean = False)
            ddlFrequency.Enabled = ToEnable
            txtRefNo.Enabled = ToEnable
            txtLimitAmt.Enabled = ToEnable
            txtFrequencyLimit.Enabled = ToEnable
            txtCustName.Enabled = ToEnable
            txtBnkOrgCode.Enabled = ToEnable
            txtAccNo.Enabled = ToEnable
            rdStatus.Enabled = ToEnable
        End Sub

        Private Function fncValidation() As Boolean
            Dim oItem As New clsMandates
            Dim bRetVal As Boolean = True
            Dim sMsg As String = ""


            If rq_PageMode = enmPageMode.NewMode Then

                If oItem.IsDuplicate(rq_OrgID, txtRefNo.Text, txtAccNo.Text, txtBnkOrgCode.Text) Then
                    bRetVal = False
                    sMsg += "Record found duplicated." & gc_BR
                End If

            End If

            If IsNumeric(txtLimitAmt.Text) Then
                If CDec(txtLimitAmt.Text) > 999999999999.99 Then
                    bRetVal = False
                    sMsg += "Limit Amount cannot greater than 999,999,999,999.99." & gc_BR
                End If
            Else
                bRetVal = False
                sMsg += "Invalid Limit Amount." & gc_BR
            End If
            If Len(sMsg) > 0 Then
                lblMessage.Text = sMsg
            End If
            Return bRetVal
        End Function

        Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
            Select Case rq_PageMode
                Case enmPageMode.NewMode, enmPageMode.EditMode
                    Response.Redirect("pg_MandateList.aspx?Id=" & rq_OrgID)
                Case enmPageMode.ViewMode
                    Response.Redirect("PG_ApprMatrix.aspx?Mode=Edit")
            End Select

        End Sub
    End Class

End Namespace
