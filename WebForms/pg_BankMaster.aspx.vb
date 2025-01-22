Namespace MaxPayroll

   Partial Class pg_BankMaster
      Inherits clsBasePage

#Region "Constants"
      Const sMsgDuplicate As String = "Bank Code Already Exists!"
      Const sMsgSaved As String = "Bank Information Saved Successfully."
      Const sMsgFail As String = "Bank Information Saved Unsuccessful."
      Const sMsgModOk As String = "Bank Information Modified Successfully."
      Const sMsgModFail As String = "Bank Information Modified Unsuccessful."
      Const sMsgConfirm As String = "Please Confirm The Modified Details."
      Const sMsgNotModify As String = "Modification Prohibited."
      Const sMsgModPage As String = "Modify Bank Information"
      Const sMsgNewPage As String = "Create Bank Information"
#End Region

#Region "Declaration"
      ReadOnly Property rq_strBankId() As String
         Get
            Return Trim(Request.QueryString("BankId") & "")
         End Get
      End Property
      ReadOnly Property rq_strPageMode() As String
         Get
            Return Trim(Request.QueryString("PageMode") & "")
         End Get
      End Property
      ReadOnly Property rq_strStatus() As String
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
            If Not ss_strUserType = gc_UT_BankUser Then
               Server.Transfer(gc_LogoutPath, False)
            End If
            Me.tblSubmit.Visible = False
            If rq_strBankId.Length > 0 Then
               'pnlReset.Visible = False
               btnReset.Visible = False
               btnBackToView.Visible = True
               prcBindStatus(enmPageMode.EditMode)
               prcDisplayInformation()
               Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), sMsgModPage, "Y")
               btnBack.Text = "Back"
               If rq_strPageMode = CStr(enmPageMode.ViewMode) Or rq_strStatus = "C" Then
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
            End If
         End If
      End Sub
#End Region

#Region "General"
      Private Sub prcBindStatus(ByVal ePageMode As enmPageMode)
         radActive.Items.Add(New ListItem("Active", "A"))
         radActive.Items.Add(New ListItem("Inactive", "I"))
         If ePageMode = enmPageMode.EditMode Then
            radActive.Items.Add(New ListItem("Cancelled", "C"))
         End If
         radActive.Items.Item(0).Selected = True
      End Sub

      Private Sub fncDisableControls()
         txtBankCode.ReadOnly = True
         txtBankName.ReadOnly = True
         txtBankAdd1.ReadOnly = True
         txtBankPostCode.ReadOnly = True
         txtBankPhone.ReadOnly = True
         txtBankFax.ReadOnly = True
         txtBankContPerson.ReadOnly = True
         txtBankURL.ReadOnly = True

         ddlBankState.Enabled = False
         radActive.Enabled = False
      End Sub
#End Region

#Region "Retrieve Info"
      Private Sub prcDisplayInformation()

         'Declaring local variable
         Dim lngUserCode As Long

         'Declaring new instance of maxFPX21.clsDatabase
         Dim clsBankMF As New clsBankMF

         'Declaring new instance of maxFPX21.clsGeneric
         Dim clsGeneric As New Generic

         'Declaring new instance of System.Data.DataSet
         Dim dsBankInfo As New DataSet

         'Declaring new instance of System.Data.DataRow
         Dim drBankInfo As DataRow, strCustStatus As String = ""

         Try

            'Gettiing the DataSet result
            dsBankInfo = clsBankMF.fncSearchBankDefinition(rq_strBankId)

            'Process each rows returned by the DataSet
            For Each drBankInfo In dsBankInfo.Tables("Query").Rows
               'read only
               txtBankCode.ReadOnly = True

               hidBankID.Value = drBankInfo("BankID")

               txtBankCode.Text = Trim(drBankInfo("BankCode"))
               txtBankName.Text = Trim(drBankInfo("BankName"))

               Dim Data As String, varArr As Array

               Data = Convert.ToString(drBankInfo("Address"))
               varArr = Data.Split(",")


               txtBankAdd1.Text = Trim(drBankInfo("Address"))
               ddlBankState.SelectedValue() = Trim(drBankInfo("State"))
               txtBankPostCode.Text = Trim(drBankInfo("Postcode"))
               txtBankPhone.Text = Trim(drBankInfo("Phone"))
               txtBankFax.Text = Trim(drBankInfo("Fax"))
               txtBankContPerson.Text = Trim(drBankInfo("ContactPerson"))
               txtBankURL.Text = Trim(drBankInfo("Url"))

               If Trim(drBankInfo("Status")) = "A" Then
                  radActive.Items(0).Selected = True
                  radActive.Items(1).Selected = False
                  radActive.Items(2).Selected = False
                  strCustStatus = "A"
               ElseIf Trim(drBankInfo("Status")) = "I" Then
                  radActive.Items(1).Selected = True
                  radActive.Items(0).Selected = False
                  radActive.Items(2).Selected = False
                  strCustStatus = "I"
               ElseIf Trim(drBankInfo("Status")) = "C" Then
                  radActive.Items(2).Selected = True
                  radActive.Items(0).Selected = False
                  radActive.Items(1).Selected = False
                  strCustStatus = "C"
               End If

               'reject - inactive/active
               If strCustStatus = "I" Then
                  btnUpdate.Visible = True
                  btnBack.Visible = False
                  btnConfirm.Visible = False
                  btnSubmit.Visible = False

                  'Call fncDisableControls()

                  'approved - cancel
               ElseIf strCustStatus = "C" Then
                  btnUpdate.Visible = False
                  btnBack.Visible = True
                  btnConfirm.Visible = False
                  btnSubmit.Visible = False
                  lblErrorMessage.Text = sMsgNotModify
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
            clsGeneric.ErrorLog("pg_BankMaster - prcDisplayInformation", Err.Number, Ex.Message)

         Finally

            'Destroy current instance of maxFPX21.clsDatabase
            clsBankMF = Nothing

            'Destroy current instance of maxFPX21.clsGeneric
            clsGeneric = Nothing

            'Destroy current instance of System.Data.DataSet
            dsBankInfo = Nothing

            'Destroy current instance of System.Data.DataRow
            drBankInfo = Nothing

         End Try

      End Sub

#End Region

#Region "Submit Button"

      '****************************************************************************************************
      'Procedure Name : Submit_Page()
      'Purpose        : Execute At the time of page Submission
      'Return Value   : N/A
      'Author         : Deedee Ibrahim - T-Melmax Sdn Bhd
      'Created        : 19/01/2006
      'Modify By      : Victor Wong
      'Modify Date    : 2007-02-06
      '*****************************************************************************************************
      Protected Sub prcSave(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

         'Declaring new instance of Database.clsDatabase
         Dim clsBankMF As New clsBankMF
         Dim clsUsers As New clsUsers
         Dim clsGeneric As New Generic
         Dim bRetVal As Boolean = False
         Dim IntUserCode As Integer
         Dim strBankCode As String = ""

         'Declaring new instance of System.Data.DataSet
         Dim dsBankID As New DataSet

         Dim IsDuplicate As Boolean

         Try
            IntUserCode = IIf(IsNumeric(Session(gc_Ses_UserID)), Session(gc_Ses_UserID), 0)

            'Check If User Id Exist - Start
            strBankCode = txtBankCode.Text

            'Duplicate checking
            IsDuplicate = clsBankMF.fncChkDuplicateBankCode(strBankCode)

            'If IsDuplicate > 0 Then
            If IsDuplicate Then

               'tblMain.Visible = True
               lblErrorMessage.Text = sMsgDuplicate

            Else

               bRetVal = clsBankMF.fncInsertBankDefinitionTable()

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
            Call clsGeneric.ErrorLog("Pg_BankMaster prcSave", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Users Class Object
            clsUsers = Nothing

            'Destroy Instance of database Class Object
            clsBankMF = Nothing

         End Try

      End Sub

#End Region

#Region "Back Button"
      Private Sub fncBack(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
         If Len(rq_strBankId) > 0 Then
            Response.Redirect("pg_BankMaster.aspx?BankID=" & rq_strBankId)
         Else
            Response.Redirect("pg_BankMaster.aspx")
         End If
      End Sub

#End Region

#Region "Update Button"
      Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
         lblErrorMessage.Text = sMsgConfirm
         hidBankState.Value = ddlBankState.SelectedValue
         hidStatus.Value = Me.radActive.SelectedValue
         fncDisableControls()
         btnUpdate.Visible = False
         btnConfirm.Visible = True
      End Sub
#End Region

#Region "Confirm Button"
      Protected Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click

         Dim clsBankMF As New clsBankMF
         Dim clsUsers As New clsUsers

         'Declaring local variable
         Dim IntUserCode As Integer
         Dim bRetVal As Boolean

         IntUserCode = IIf(IsNumeric(Session(gc_Ses_UserID)), Session(gc_Ses_UserID), 0)

         bRetVal = clsBankMF.fncUpdateBankDefinitionTable()
         If bRetVal Then
            lblErrorMessage.Text = sMsgModOk
         Else
            lblErrorMessage.Text = sMsgModFail
         End If
         btnConfirm.Visible = False

      End Sub
#End Region

#Region "Back To View Page"
      Protected Sub btnBackToView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackToView.Click
         Response.Redirect("pg_SearchBank.aspx?BankCode=&BankName=")
      End Sub
#End Region

   End Class

End Namespace