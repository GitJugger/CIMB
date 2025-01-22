Namespace MaxPayroll

    Partial Class pg_PaymentService
      Inherits clsBasePage

#Region "Constants"
      Const sMsgDuplicate As String = "Payment Service code already exists."
        Const sTitle As String = "Payment Service Information"
        Const sMsgSaved As String = sTitle & " Saved Successfully."
        Const sMsgFail As String = sTitle & " Saved Unsuccessful."
        Const sMsgModOk As String = sTitle & " Modified Successfully."
        Const sMsgModFail As String = sTitle & " Modified Unsuccessful."
        Const sMsgConfirm As String = "Please Confirm The Modified Details."
        Const sMsgNotModify As String = "Modification Prohibited."
        Const sMsgModPage As String = "Modify " & sTitle & "."
        Const sMsgNewPage As String = "Create " & sTitle & "."
#End Region

#Region "Declaration"
       
        ReadOnly Property sPaymentServiceID() As String
            Get
                Return Trim(Request.QueryString("PaySrvID") & "")
            End Get
        End Property
        ReadOnly Property sPageMode() As String
            Get
                Return Trim(Request.QueryString("PageMode") & "")
            End Get
        End Property
        ReadOnly Property sStatus() As String
            Get
                Return Trim(Request.QueryString("Status") & "")
            End Get
        End Property
#End Region

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Page.IsPostBack = False Then
                'BindBody(body)
                Dim clsUsers As New MaxPayroll.clsUsers
                Dim clsCommon As New clsCommon
                If Not ss_strUserType = gc_UT_BankAdmin Then
                    Server.Transfer(gc_LogoutPath, False)
                End If
                Me.tblSubmit.Visible = False
                clsCommon.fncBindDDLStatutory(Me.ddlStatutoryNumber)
                If sPaymentServiceID.Length > 0 Then
                    lblHeading.Text = "Payment Service Modification"
                    btnReset.Visible = False
                    btnSubmit.Visible = False
                    btnConfirm.Visible = False
                    btnBackToView.Visible = True
                    prcBindStatus(enmPageMode.EditMode)
                    prcDisplayInformation()
                    Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), sMsgModPage, "Y")
                    btnBack.Text = "Back"
                    If sPageMode = CStr(enmPageMode.ViewMode) Or sStatus = "C" Then
                        fncDisableControls()
                        btnUpdate.Visible = False
                        btnConfirm.Visible = False
                        btnSubmit.Visible = False


                    End If
                Else
                    prcBindStatus(enmPageMode.NewMode)
                    btnUpdate.Visible = False
                    btnConfirm.Visible = False
                    Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), sMsgNewPage, "Y")
                    lblHeading.Text = "Payment Service Creation"
                End If
            End If
        End Sub
#End Region

#Region "General"

        Private Sub prcBindStatus(ByVal ePageMode As enmPageMode)
         radStatus.Items.Add(New ListItem("Active", "A"))
            radStatus.Items.Add(New ListItem("Inactive", "I"))
            If ePageMode = enmPageMode.EditMode Then
            radStatus.Items.Add(New ListItem("Cancelled", "C"))
            End If
            radStatus.Items.Item(0).Selected = True
        End Sub

        Private Sub prcValueOnSubmit()

            hidStatus.Value = radStatus.SelectedValue
            hidIsMultipleBank.Value = rbIsMultipleBank.SelectedValue
            hidStatutory.Value = ddlStatutoryNumber.SelectedValue
        End Sub
        Private Sub fncDisableControls()
            Me.txtPaySrvCode.ReadOnly = True
            Me.txtPaySrvDesc.ReadOnly = True
            Me.radStatus.Enabled = False
            rbIsMultipleBank.Enabled = False
            ddlStatutoryNumber.Enabled = False
        End Sub
#End Region

#Region "Retrieve Info"
        Private Sub prcDisplayInformation()

            'Declaring local variable
            Dim lngUserCode As Long

            'Declaring new instance of maxFPX21.clsDatabase
            Dim clsPaymentService As New clsPaymentService

            'Declaring new instance of maxFPX21.clsGeneric
            Dim clsGeneric As New Generic

            'Declaring new instance of System.Data.DataSet
            Dim dsPaySrvInfo As New DataSet

            'Declaring new instance of System.Data.DataRow
            Dim drPaySrvInfo As DataRow, strCustStatus As String = ""

            Try

                'Gettiing the DataSet result
                dsPaySrvInfo = clsPaymentService.fncSearchPaySrv(sPaymentServiceID)

                'Process each rows returned by the DataSet
                For Each drPaySrvInfo In dsPaySrvInfo.Tables("Query").Rows
                    'read only
                    Me.txtPaySrvCode.ReadOnly = True
                    Me.hidPaySerID.Value = CStr(drPaySrvInfo("PaySer_ID") & "").Trim

                    Me.txtPaySrvCode.Text = CStr(drPaySrvInfo.Item("PaySrvCode") & "").Trim
                    Me.txtPaySrvDesc.Text = CStr(drPaySrvInfo.Item("PaySer_Desc") & "").Trim

                    Me.radStatus.SelectedValue = CStr(drPaySrvInfo.Item("PaySrvStatus") & "").Trim
                    ddlStatutoryNumber.SelectedValue = CStr(drPaySrvInfo.Item("PayStatutory") & "")
                    strCustStatus = Me.radStatus.SelectedValue

                    If IsNumeric(fncAppSettings(mdConstant.gc_WC_DefaultBank)) = False Then
                        trMultipleBank.Visible = True
                    Else
                        Me.rbIsMultipleBank.SelectedValue = CBool(drPaySrvInfo.Item("IsMultipleBank"))
                    End If
                    'reject - inactive/active
                    If strCustStatus = "I" Then
                        btnUpdate.Visible = True
                        btnBack.Visible = False
                        btnConfirm.Visible = False
                        btnSubmit.Visible = False
                        lblErrorMessage.Text = sMsgNotModify

                        'approved - cancel
                    ElseIf strCustStatus = "C" Then
                        btnUpdate.Visible = False
                        btnBack.Visible = True
                        btnConfirm.Visible = False
                        btnSubmit.Visible = False
                        'lblErrorMessage.Text = "Bank Code cannot be modified, Cancellation request is Authorized."
                        Call fncDisableControls()

                    Else
                        'approved
                        btnUpdate.Visible = True
                        btnBack.Visible = False
                        btnConfirm.Visible = False
                        btnSubmit.Visible = False
                    End If

                    'authorised check end

                Next

                If Not Page.IsPostBack Then

                    lngUserCode = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                End If

            Catch Ex As SystemException

                'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("pg_PaymentWinMaster - prcDisplayInformation", Err.Number, Ex.Message)

            Finally

                'Destroy current instance of maxFPX21.clsDatabase
                clsPaymentService = Nothing

                'Destroy current instance of maxFPX21.clsGeneric
                clsGeneric = Nothing

                'Destroy current instance of System.Data.DataSet
                dsPaySrvInfo = Nothing

                'Destroy current instance of System.Data.DataRow
                drPaySrvInfo = Nothing

            End Try

        End Sub

#End Region

#Region "Submit Button"

        '****************************************************************************************************
        'Procedure Name : Submit_Page()
        'Purpose        : Execute At the time of page Submission
        'Return Value   : N/A
        'Author         : Victor Wong 
        'Created        : 2007-02-12
        'Modify By      : 
        'Modify Date    : 
        '*****************************************************************************************************
        Protected Sub prcSave(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Declaring new instance of Database.clsDatabase
            Dim clsPaymentService As New clsPaymentService
            Dim clsUsers As New clsUsers
            Dim clsGeneric As New Generic
            Dim bRetVal As Boolean = False

            'Declaring new instance of System.Data.DataSet
            Dim dsBankID As New DataSet

            Dim IsDuplicate As Boolean

            Try

                'Duplicate checking
                IsDuplicate = clsPaymentService.fncChkDuplicatePaySrvCode(txtPaySrvCode.Text.Trim)

                'If IsDuplicate > 0 Then
                If IsDuplicate Then

               'tblMain.Visible = True
                    lblErrorMessage.Text = sMsgDuplicate

                Else

                    bRetVal = clsPaymentService.fncInsertPaymentService


                    If bRetVal Then
                  'tblMain.Visible = False
                        tblCreate.Visible = False
                        tblSubmit.Visible = True
                        lblMessage.Text = sMsgSaved
                    Else
                        lblErrorMessage.Text = sMsgFail
                    End If
                End If
            Catch

                'Log Error
                Call clsGeneric.ErrorLog("pg_PaymentService prcSave", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Users Class Object
                clsUsers = Nothing

                'Destroy Instance of database Class Object
                clsPaymentService = Nothing

            End Try

        End Sub

#End Region

#Region "Back"
        Private Sub fncBack(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
            If Len(sPaymentServiceID) > 0 Then
                Response.Redirect("pg_PaymentService.aspx?PaymentSerID=" & sPaymentServiceID)
            Else
                Response.Redirect("pg_PaymentService.aspx")
            End If
        End Sub

#End Region

#Region "Update Button"
        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

            'Dim clsEncryption As New Encryption
            prcValueOnSubmit()
            lblErrorMessage.Text = sMsgConfirm
            'Me.hidBankID.Value = Me.ddlBankID.SelectedValue
            'Me.hidFtpFunction.Value = Me.ddlFtpFunction.SelectedValue
            'Me.hidStatus.Value = Me.radStatus.SelectedValue
            'sNewFTPPwd = txtFtpPwd.Text
            'trFtpPwd.Visible = False
            'trFtpPwdButton.Visible = True
            'Me.btnChgPwd.Visible = False
            'If Len(sNewFTPPwd) > 0 AndAlso clsEncryption.Cryptography(sNewFTPPwd).Equals(sOldFTPPwd) = False Then
            '    Me.lblPwdMsg.Text = sMsgPwdChg
            'Else
            '    lblPwdMsg.Text = sMsgPwdNoChg
            'End If
            fncDisableControls()
            btnUpdate.Visible = False
            btnConfirm.Visible = True
        End Sub
#End Region

#Region "Confirm Button"
        Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

            Dim clsPaymentService As New clsPaymentService
            Dim clsUsers As New clsUsers

            'Declaring local variable
            Dim IntUserCode As Integer
            Dim bRetVal As Boolean
            IntUserCode = IIf(IsNumeric(Session(gc_Ses_UserID)), Session(gc_Ses_UserID), 0)
            bRetVal = clsPaymentService.fncUpdatePaymentServiceTable()
            If bRetVal Then
                lblErrorMessage.Text = sMsgModOk
            Else
                lblErrorMessage.Text = sMsgModFail
            End If
            btnConfirm.Visible = False

        End Sub
#End Region

#Region "Back To Search Page"
        Protected Sub btnBackToView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackToView.Click
            Response.Redirect("pg_SearchPaymentService.aspx?PaymentServiceID=&PaymentServiceName=")
        End Sub
#End Region

    End Class

End Namespace