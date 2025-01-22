Namespace MaxPayroll

    Partial Class pg_PaymentWinMaster
      Inherits clsBasePage


#Region "Constants"
        Const sMsgDuplicate As String = "Payment Window Code Already Exists!"
        Const sTitle As String = "Payment Window Information"
        Const sMsgSaved As String = sTitle & " Saved Successfully."
        Const sMsgFail As String = sTitle & " Saved Unsuccessful."
        Const sMsgModOk As String = sTitle & " Modified Successfully."
        Const sMsgModFail As String = sTitle & " Modified Unsuccessful."
        Const sMsgConfirm As String = "Please Confirm The Modified Details."
        Const sMsgNotModify As String = "Modification Prohibited."
        Const sMsgModPage As String = "Modify " & sTitle & "."
        Const sMsgNewPage As String = "Create " & sTitle & "."
        Const sMsgPwdChg As String = "Ftp Password Has changed."
        Const sMsgPwdNoChg As String = "Ftp Password Remains As Previous."
#End Region

#Region "Declaration"
        Property sNewFTPPwd() As String
            Get
                Return ViewState("NewFTPPwd") & ""
            End Get
            Set(ByVal value As String)
                ViewState("NewFTPPwd") = value
            End Set
        End Property
        Property sOldFTPPwd() As String
            Get
                Return ViewState("OldFTPPwd") & ""
            End Get
            Set(ByVal value As String)
                ViewState("OldFTPPwd") = value
            End Set
        End Property
        ReadOnly Property sPaymentWindowID() As String
            Get
                Return Trim(Request.QueryString("PayWinID") & "")
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

            Try
                If Page.IsPostBack = False Then

                    'BindBody(body)

                    If ss_strUserType <> gc_UT_BankUser Then
                        Server.Transfer(gc_LogoutPath, False)
                    End If
                    Dim clsUsers As New MaxPayroll.clsUsers
                    prcBindBank()
                    If Me.ddlBankID.Items.Count = 0 Then
                        lblErrorMessage.Text = "Bank Code not created."
                        Exit Try
                    Else
                        '071018 Else part added by Marcus
                        'Purpose: To hide bank code info if there is a Default Bank Code set up in web.config

                        Dim sRetval As String
                        sRetval = clsCommon.fncDefaultBankChecking(Me.ddlBankID, Me.lblBank)
                        If Len(sRetval) > 0 Then
                            lblErrorMessage.Text = sRetval
                            Exit Try
                        End If

                    End If
                    prcBindFtpFunction()
                    Me.tblSubmit.Visible = False
                    If sPaymentWindowID.Length > 0 Then
                        lblTitle.Text = "Payment Window Code Creation"
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
                        lblTitle.Text = "Payment Window Code Modification"
                        prcBindStatus(enmPageMode.NewMode)
                        btnUpdate.Visible = False
                        btnConfirm.Visible = False
                        Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), sMsgNewPage, "Y")
                    End If

                End If
            Catch ex As Exception

            End Try

        End Sub
#End Region

#Region "General"
        Private Sub prcBindBank()
            Dim clsBankMF As New clsBankMF

         Me.ddlBankID.DataSource = clsBankMF.fncRetrieveBankCodeName(clsCommon.fncGetPrefix(enmStatus.A_Active))
            Me.ddlBankID.DataTextField = "BankName"
            Me.ddlBankID.DataValueField = "BankID"
            ddlBankID.DataBind()

        End Sub

        Private Sub prcBindFtpFunction()
            ddlFtpFunction.Items.Add(New ListItem("Download", "D"))
         ddlFtpFunction.Items.Add(New ListItem("Upload", "U"))
         ddlFtpFunction.SelectedValue = "U"
        End Sub

        Private Sub prcBindStatus(ByVal ePageMode As enmPageMode)
         radStatus.Items.Add(New ListItem("Active", "A"))
            radStatus.Items.Add(New ListItem("Inactive", "I"))
            If ePageMode = enmPageMode.EditMode Then
            radStatus.Items.Add(New ListItem("Cancelled", "C"))
            End If
            radStatus.Items.Item(0).Selected = True
        End Sub

        Private Sub prcValueOnSubmit()
            hidBankID.Value = ddlBankID.SelectedValue
            hidFtpFunction.Value = ddlFtpFunction.SelectedValue
            hidStatus.Value = radStatus.SelectedValue
        End Sub
        Private Sub fncDisableControls()

            Me.txtPayWinCode.ReadOnly = True
            Me.ddlBankID.Enabled = False
            Me.ddlFtpFunction.Enabled = False
            Me.txtFtpServerName.ReadOnly = True
            Me.txtFtpIPAddress.ReadOnly = True
            Me.txtFtpUploadDir.ReadOnly = True
            Me.txtFtpDownloadDir.ReadOnly = True
            Me.txtFtpUserID.ReadOnly = True
            Me.txtFtpPwd.ReadOnly = True
            Me.radStatus.Enabled = False
            Me.txtPayWinDesc.ReadOnly = True
         Me.txtPayWinStartTime.ReadOnly = True
         Me.btnChgPwd.Visible = False
         Me.trFtpPwdButton.Visible = False

        End Sub
#End Region

#Region "Retrieve Info"

      Private Sub prcDisplayInformation()

         'Declaring local variable
         Dim lngUserCode As Long

         'Declaring new instance of maxFPX21.clsDatabase
         Dim clsPaymentMF As New clsPaymentMF

         'Declaring new instance of maxFPX21.clsGeneric
         Dim clsGeneric As New Generic

         'Declaring new instance of System.Data.DataSet
         Dim dsPayWinInfo As New DataSet

         'Declaring new instance of System.Data.DataRow
         Dim drPayWinInfo As DataRow, strCustStatus As String = ""

         Try

            'Gettiing the DataSet result
            dsPayWinInfo = clsPaymentMF.fncSearchPayWinDef(sPaymentWindowID)


            'Process each rows returned by the DataSet
            For Each drPayWinInfo In dsPayWinInfo.Tables("Query").Rows
               'read only
               Me.txtPayWinCode.ReadOnly = True
               Dim LItem As ListItem

               Dim bExist As Boolean = False
               
               hidPayWinID.Value = CStr(drPayWinInfo("PayWinID") & "").Trim

               hidBankID.Value = CStr(drPayWinInfo("BankID") & "").Trim

               txtPayWinCode.Text = CStr(drPayWinInfo.Item("PayWinCode") & "").Trim
               Me.txtFtpServerName.Text = CStr(drPayWinInfo.Item("SrvName") & "").Trim
               Me.txtFtpIPAddress.Text = CStr(drPayWinInfo.Item("IPAddress") & "").Trim
               Me.txtFtpUploadDir.Text = CStr(drPayWinInfo.Item("UploadDir") & "").Trim
               Me.txtFtpDownloadDir.Text = CStr(drPayWinInfo.Item("DownldDir") & "").Trim
               Me.txtFtpUserID.Text = CStr(drPayWinInfo.Item("UserID") & "").Trim
               sOldFTPPwd = CStr(drPayWinInfo.Item("Passwd") & "").Trim
               'Me.lblFtpPwd.Visible = False
               'Me.txtFtpPwd.Visible = False
               'Me.rfvtxtFtpPwd.Enabled = False
               trFtpPwd.Visible = False
               trFtpPwdButton.Visible = True

               Me.ddlFtpFunction.SelectedValue = CStr(drPayWinInfo.Item("FtpType") & "").Trim
               Me.txtPayWinStartTime.Text = CStr(drPayWinInfo.Item("PayWinStart") & "").Trim
               Me.txtPayWinDesc.Text = CStr(drPayWinInfo.Item("PayWinDesc") & "").Trim
               Me.radStatus.SelectedValue = CStr(drPayWinInfo.Item("PayWinStatus") & "").Trim



               strCustStatus = Me.radStatus.SelectedValue

               'reject - inactive/active
               If strCustStatus = "I" Then
                  btnUpdate.Visible = True
                  btnBack.Visible = False
                  btnConfirm.Visible = False
                  btnSubmit.Visible = False
                  lblErrorMessage.Text = sMsgNotModify
                  'Call fncDisableControls()

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
               If strCustStatus <> "C" Then
                  If IsDBNull(drPayWinInfo.Item("BankID")) = False Then
                     'iBankID = CInt(drPayWinInfo.Item("BankID"))
                     For Each LItem In ddlBankID.Items
                        If LItem.Value = CInt(drPayWinInfo.Item("BankID")) Then
                           bExist = True
                           Exit For
                        End If
                     Next
                  End If
                  If bExist Then
                     Me.ddlBankID.SelectedValue = CStr(drPayWinInfo.Item("BankID") & "")
                  Else
                     lblErrorMessage.Text = "The Bank ID which originally tied with the Payment Window Code is either Inactive or Cancelled."
                     Me.fncDisableControls()
                     Me.ddlBankID.Items.Clear()
                     btnUpdate.Enabled = False
                     Exit Try
                  End If
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
            clsPaymentMF = Nothing

            'Destroy current instance of maxFPX21.clsGeneric
            clsGeneric = Nothing

            'Destroy current instance of System.Data.DataSet
            dsPayWinInfo = Nothing

            'Destroy current instance of System.Data.DataRow
            drPayWinInfo = Nothing

         End Try

      End Sub

#End Region

#Region "Submit Button"

        '****************************************************************************************************
        'Procedure Name : Submit_Page()
        'Purpose        : Execute At the time of page Submission
        'Return Value   : N/A
        'Author         : Victor Wong 
        'Created        : 2007-02-08
        'Modify By      : 
        'Modify Date    : 
        '*****************************************************************************************************
        Protected Sub prcSave(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

            'Declaring new instance of Database.clsDatabase
            Dim clsPaymentMF As New clsPaymentMF
            Dim clsUsers As New clsUsers
            Dim clsGeneric As New Generic
            Dim bRetVal As Boolean = False

            'Declaring new instance of System.Data.DataSet
            Dim dsBankID As New DataSet

            Dim IsDuplicate As Boolean

            Try

                'Duplicate checking
                IsDuplicate = clsPaymentMF.fncChkDuplicatePayWinCode(txtPayWinCode.Text.Trim, ddlBankID.SelectedValue.Trim)

                'If IsDuplicate > 0 Then
                If IsDuplicate Then

                    tblMain.Visible = True
                    lblErrorMessage.Text = sMsgDuplicate

                Else

                    bRetVal = clsPaymentMF.fncInsertPaymentWindowDefinitionTable


                    If bRetVal Then
                        tblMain.Visible = False
                        tblCreate.Visible = False
                        tblSubmit.Visible = True
                        lblMessage.Text = sMsgSaved
                    Else
                        lblErrorMessage.Text = sMsgFail
                    End If
                End If
            Catch

                'Log Error
                Call clsGeneric.ErrorLog("pg_PaymentWinMaster prcSave", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Users Class Object
                clsUsers = Nothing

                'Destroy Instance of database Class Object
                clsPaymentMF = Nothing

            End Try

        End Sub

#End Region

#Region "Back"
        Private Sub fncBack(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
            If Len(sPaymentWindowID) > 0 Then
                Response.Redirect("pg_PaymentWinMaster.aspx?PaymentWindowID=" & sPaymentWindowID)
            Else
                Response.Redirect("pg_PaymentWinMaster.aspx")
            End If
        End Sub

#End Region

#Region "Update Button"
        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

            Dim clsEncryption As New Encryption

            lblErrorMessage.Text = sMsgConfirm
            Me.hidBankID.Value = Me.ddlBankID.SelectedValue
            Me.hidFtpFunction.Value = Me.ddlFtpFunction.SelectedValue
            Me.hidStatus.Value = Me.radStatus.SelectedValue
            sNewFTPPwd = txtFtpPwd.Text
            trFtpPwd.Visible = False
            trFtpPwdButton.Visible = True
            Me.btnChgPwd.Visible = False
            If Len(sNewFTPPwd) > 0 AndAlso clsEncryption.Cryptography(sNewFTPPwd).Equals(sOldFTPPwd) = False Then
                Me.lblPwdMsg.Text = sMsgPwdChg
            Else
                lblPwdMsg.Text = sMsgPwdNoChg
            End If
            fncDisableControls()
            btnUpdate.Visible = False
            btnConfirm.Visible = True
        End Sub
#End Region

#Region "Confirm Button"

      Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

         Dim clsPaymentMF As New clsPaymentMF
         Dim clsUsers As New clsUsers

         'Declaring local variable
         Dim IntUserCode As Integer
         Dim bRetVal As Boolean
         Dim strFtpPwd As String = ""
         Dim bNewPwd As Boolean = False

         IntUserCode = IIf(IsNumeric(Session(gc_Ses_UserID)), Session(gc_Ses_UserID), 0)
         If Len(sNewFTPPwd) > 0 Then
            strFtpPwd = sNewFTPPwd
            bNewPwd = True
         Else
            strFtpPwd = sOldFTPPwd
         End If
         bRetVal = clsPaymentMF.fncUpdatePaymentWindowDefinitionTable(strFtpPwd, bNewPwd)
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
            Response.Redirect("pg_SearchPaymentWin.aspx?PayWinCode=&PayWinDesc=&BankID=")
        End Sub
#End Region

        Protected Sub btnChgPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChgPwd.Click
            trFtpPwd.Visible = True
            trFtpPwdButton.Visible = False
        End Sub
    End Class

End Namespace