Option Strict Off
Option Explicit On 

Imports System.IO
Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsCustomer


Namespace MaxPayroll


Partial Class PG_ModifyOrganisation
        Inherits clsBasePage

#Region "Declaration"
      Private ReadOnly Property rq_lngOrgID() As Long
         Get
            If IsNumeric(Request.QueryString("ID")) Then
               Return CLng(Request.QueryString("ID"))
            Else
               Return -1
            End If
         End Get
      End Property
#End Region

#Region "Page Load"

      '****************************************************************************************************
      'Procedure Name : Page_Load()
      'Purpose        : To Execute code during page load. 
      'Arguments      : System Object, System Event Args
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Modified       : 08/07/2004
      '*****************************************************************************************************
      Private Property bChkUseTokenPreviousValue() As Boolean
         Get
            Return ViewState("bChkUseTokenPreviousValue")
         End Get
         Set(ByVal value As Boolean)
            ViewState("bChkUseTokenPreviousValue") = value
         End Set
      End Property

      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Create Instance of DatagridItem
         Dim dgiPayService As DataGridItem

         'Create Instance of System Data Set
         Dim dsState As New System.Data.DataSet

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of User Class Object
         Dim clsUsers As New MaxPayroll.clsUsers

         'Create Instance of System Data Row
         Dim drOrganisation As System.Data.DataRow

         'Create Instance of System Data Set
         Dim dsOrganisation As New System.Data.DataSet

         'Create Instance of Customer Class Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'Variable Declarations
         Dim intState As Integer
         Dim strStatus As String = "", strPrivilege As String



         Try

            'check if only bank user, inquiry user or bank authoriser
            If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser Or ss_strUserType = gc_UT_BankAuth) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then

                    'BindBody(body)

               lblHeading.Text = "Modify Organization"
               'lblSysAdmin.Text = gc_SysAdminDesc
               'lblSysAdminIC.Text = gc_SysAdminDesc & "'s IC No."

               'Populate StateCode,State in Combo box - Start
               dsState = clsCustomer.GetState(0, 0)
               cmbState.DataSource = dsState
               cmbState.DataTextField = "State"
               cmbState.DataValueField = "StateCode"
               cmbState.DataBind()
               'Populate StateCode,State in Combo box - Stop

               'Get Organisation Details

                    txtOrgId.Text = rq_lngOrgID
               hOrgId.Value = rq_lngOrgID
               hMode.Value = Request.QueryString("Mode")

               'Populate Payment Service Data Grid - Start
               dsState = clsCustomer.fncPayService("M", rq_lngOrgID, ss_lngUserID)
               dgPayService.DataSource = dsState.Tables(0)
               fncGeneralGridTheme(dgPayService)
               dgPayService.DataBind()


               For Each drOrganisation In dsState.Tables(1).Rows
                  'Loop Thro Datagrid and Update Selected Payment Service - Start
                  For Each dgiPayService In dgPayService.Items
                     Dim intPaySerId As Int16 = CInt(dgiPayService.Cells(0).Text)                                'Get Payment Service Id
                     'If Payment Service Selected
                     If drOrganisation("PID") = intPaySerId Then
                        Dim txtCharge As TextBox = CType(dgiPayService.FindControl("txtCharge"), TextBox)       'Get Transaction Charge Text Box
                        Dim chkService As CheckBox = CType(dgiPayService.FindControl("chkService"), CheckBox)   'Get Check Box    
                        Dim hidCharge As HtmlInputHidden = CType(dgiPayService.FindControl("hidCharge"), HtmlInputHidden)   'Get Hidden Text Box
                        chkService.Checked = True
                        txtCharge.Text = drOrganisation("PCHRG")
                        hidCharge.Value = Format(CDbl(txtCharge.Text), "##,##0.00")
                        txtCharge.Text = Format(CDbl(txtCharge.Text), "##,##0.00")
                        Exit For
                     End If
                  Next
                  'Loop Thro Datagrid and Update Selected Payment Service - Stop
               Next
               'Populate Payment Service Data Grid - Stop

               'Populate Data Set
               dsOrganisation = clsCustomer.fnGetOrganisation(rq_lngOrgID)

               'Loop Thro Data Set - START
               For Each drOrganisation In dsOrganisation.Tables("ORGANISATION").Rows
                  'Fill Values - START
                  txtOrgName.Text = drOrganisation("ORGNAME")                                                             'Organisation Name
                  txtBrCode.Text = drOrganisation("ORGBRCODE")                                                            'Organisation Branch Code
                  txtAddress.Text = drOrganisation("ORGADDR")                                                             'Organisation Address
                  AtxtAddr.Text = drOrganisation("ORGADDR")
                  intState = drOrganisation("ORGSTATE")                                                                   'Organisation State
                  txtPincode.Text = drOrganisation("ORGPIN")                                                              'Organisation Pincode
                  txtPhone1.Text = drOrganisation("ORGPH1")                                                               'Organisation Phone 1
                  txtPhone2.Text = IIf(IsDBNull(drOrganisation("ORGPH2")), "", drOrganisation("ORGPH2"))                  'Organisation Phone 2
                  txtFax.Text = IIf(IsDBNull(drOrganisation("ORGFAX")), "", drOrganisation("ORGFAX"))                     'Organisation Fax
                  txtEmail.Text = IIf(IsDBNull(drOrganisation("ORGEMAIL")), "", drOrganisation("ORGEMAIL"))               'Organisation Email
                  txtURL.Text = IIf(IsDBNull(drOrganisation("ORGURL")), "", drOrganisation("ORGURL"))                     'Organisation URL
                  txtContactPerson.Text = IIf(IsDBNull(drOrganisation("ORGCONTACT")), "", drOrganisation("ORGCONTACT"))   'Contact Person
                  AhCA.Value = txtContactPerson.Text
                  txtContactPerIC.Text = IIf(IsDBNull(drOrganisation("ORGCNTIC")), "", drOrganisation("ORGCNTIC"))        'Contact Person IC
                  txtCustomerAdmin.Text = IIf(IsDBNull(drOrganisation("CUSTADMIN")), "", drOrganisation("CUSTADMIN"))     'Customer Administrator
                  AhSA.Value = txtCustomerAdmin.Text
                  txtCustomAdminIC.Text = IIf(IsDBNull(drOrganisation("CUSTADMINIC")), "", drOrganisation("CUSTADMINIC")) 'Customer Administrator
                        txtAnnualFee.Text = drOrganisation("ANNFEE")                                                            'Subscription Fee
                  txtAnnualFee.Text = Format(CDbl(txtAnnualFee.Text), "##,##0.00")                                       'Round Two Decimal Place
                  AhAnnFees.Value = txtAnnualFee.Text
                  txtBusReg.Text = drOrganisation("RegNumber")                                                            'Business Registration No
                  txtTaxReg.Text = drOrganisation("TaxNumber")                                                            'Tax Registration No
                  strStatus = drOrganisation("ORGSTATUS")
                  If strStatus = "F" Then
                     rdStatus.SelectedValue = "D"
                  Else
                     rdStatus.SelectedValue = strStatus
                  End If
                  hStatus.Value = strStatus
                  strPrivilege = drOrganisation("Privileged")
                  hAPriv.Value = drOrganisation("Privileged")
                  chkPrivilege.Checked = IIf(strPrivilege = "Y", True, False)
                  rdVerify.SelectedValue = drOrganisation("OVERIFY")
                  hIVerify.Value = rdVerify.SelectedValue
                  hAVerify.Value = drOrganisation("OVERIFY")
                  txtRegion.Text = IIf(Not IsDBNull(drOrganisation("ORGREG")), drOrganisation("ORGREG"), "")
                  txtEmployees.Text = IIf(IsNumeric(drOrganisation("ORGEMP")), drOrganisation("ORGEMP"), "")
                  txtStopCharges.Text = IIf(IsNumeric(drOrganisation("ORGSTOP")), drOrganisation("ORGSTOP"), 0)
                  txtStopCharges.Text = Format(CDbl(txtStopCharges.Text), "##,##0.00")
                  AhStopChrg.Value = txtStopCharges.Text
                  txtGroups.Text = drOrganisation("OGROUP")
                  AhGroups.Value = drOrganisation("OGROUP")
                  txtState.Text = drOrganisation("OSTATE")
                  chkUseToken.Checked = CBool(drOrganisation("OrgToken"))

                  'Marcus: Keep the current value of chkUseToken in view state
                  bChkUseTokenPreviousValue = chkUseToken.Checked

                  'Fill Values - STOP
               Next
               'Loop Thro Data Set - STOP

               If rdVerify.SelectedValue = "2" Then
                  trVerify.Visible = False
                  trVerify1.Visible = True
               End If

               'Check State Code - Start
               If intState > 15 Then
                  cmbState.SelectedValue = 0
               Else
                  cmbState.SelectedValue = intState
               End If
               'Check State Code - End

               'If Organisation Cancelled
               Select Case strStatus
                  Case "D"
                     trMain.Visible = False
                     lblOrg.Text = "Organization has been cancelled."
                  Case "F"
                     'if request for cancellation
                     trMain.Visible = False
                     lblOrg.Text = "Organization under cancellation request."
               End Select


               'if bank auth hide modify button
               Select Case ss_strUserType
                  Case gc_UT_BankAuth, gc_UT_InquiryUser
                     trMain.Visible = False
               End Select

               'if page called from approval matrix
               If Not hMode.Value = "" Then
                  trBack.Visible = True
               End If


               'Audit Trail
               Call clsUsers.prcDetailLog(ss_lngUserID, "View Organization", "Y")

            End If

         Catch

            'Log Error
            LogError("PG_MOdifyOrganization - Page Load")

         Finally

            'Destroy Instance of DatagridItem
            dgiPayService = Nothing

            'Destroy Instance of System Data Set
            dsState = Nothing

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of User Class Object
            clsUsers = Nothing

            'Destroy Instance of System Data Set
            dsOrganisation = Nothing

            'Destroy Instance of System Data Row
            drOrganisation = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

         End Try

      End Sub

#End Region

#Region "Confirm Page"

      '****************************************************************************************************
      'Procedure Name : Submit_Page()
      'Purpose        : To Execute code during page save. 
      'Arguments      : System Object, System Event Args
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Modified       : 21/07/2004
      '*****************************************************************************************************
      Public Sub Confirm_Page(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

         'Create Instance of DatagridItem
         Dim dgiPayService As DataGridItem

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of User Class Object
         Dim clsUsers As New MaxPayroll.clsUsers

         'Create Instance of System Data Set
         Dim dsPayService As New System.Data.DataSet

         'Create Instance of DataTable
         Dim dtPayService As New System.Data.DataTable

         'Create Instance of Customer Class Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'Variable Declarations
         Dim dcTranCharge As Decimal, strPayService As String
         Dim IsDuplicate As Boolean, lngUserId As Long, lngOrgId As Long, intRowIndex As Int16
         Dim IsUser As Boolean, strOrgName As String, strSysAdmin As String, strValue As String
            Dim clsCommon As New MaxPayroll.clsCommon
            Try

            lblOrg.Text = "Please confirm your changes."
            lngOrgId = IIf(IsNumeric(hOrgId.Value), hOrgId.Value, 0)                        'Get Organisation Id
            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id

            'Check If State Others - Start
            If cmbState.SelectedValue = 15 And txtState.Text = "" Then
               tblForm.Visible = True
               tblConfirm.Visible = False
               lblOrg.Text = "If State is Others, Please enter value for Other."
               Exit Try
            End If
            'Check If State Others - Stop

            'Check if Attempt to Convert From Dual back to Single - Start
            If hVerify.Value = "2" Then
               If rdVerify.SelectedValue = "1" Then
                  tblForm.Visible = True
                  tblConfirm.Visible = False
                  lblOrg.Text = "Cannot convert from Dual Verification to Single Verification."
                  Exit Try
               End If
            End If
            'Check if Attempt to Convert From Dual back to Single - Stop

            'Check If Dual Verification SA Details Available - START
            If rdVerify.SelectedValue = "2" Then
               If txtCustomerAdmin.Text = "" Or txtCustomAdminIC.Text = "" Then
                  tblForm.Visible = True
                  tblConfirm.Visible = False
                  lblOrg.Text = "If Dual verification " & gc_UT_SysAuthDesc & " details required."
                  Exit Try
               End If
            End If
            'Check If Dual Verification SA Details Available - STOP

            'Business Registration No
            strValue = txtBusReg.Text
            IsDuplicate = clsCustomer.fnOrgValidations("UPDATE", "REG NO", strValue, lngOrgId)
            If IsDuplicate Then
               tblForm.Visible = True
               tblConfirm.Visible = False
               lblOrg.Text = "Business Registration No Cannot be Duplicated."
               Exit Try
            End If
            'Check duplicate Entries - Stop

            tblForm.Visible = False
            tblConfirm.Visible = True
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtOrgName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtGroupDesc1 As Boolean = clsCommon.CheckScriptValidation(txtAddress.Text)
                If txtGroupDesc1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtState1 As Boolean = clsCommon.CheckScriptValidation(txtState.Text)
                If txtState1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If

                Dim txtContactPerson1 As Boolean = clsCommon.CheckScriptValidation(txtContactPerson.Text)
                If txtContactPerson1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtContactPerIC1 As Boolean = clsCommon.CheckScriptValidation(txtContactPerIC.Text)
                If txtContactPerIC1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtCustomerAdmin1 As Boolean = clsCommon.CheckScriptValidation(txtCustomerAdmin.Text)
                If txtCustomerAdmin1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtCustomAdminIC1 As Boolean = clsCommon.CheckScriptValidation(txtCustomAdminIC.Text)
                If txtCustomAdminIC1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtStopCharges1 As Boolean = clsCommon.CheckScriptValidation(txtStopCharges.Text)
                If txtStopCharges1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtBusReg1 As Boolean = clsCommon.CheckScriptValidation(txtBusReg.Text)
                If txtBusReg1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtTaxReg1 As Boolean = clsCommon.CheckScriptValidation(txtTaxReg.Text)
                If txtTaxReg1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtRegion1 As Boolean = clsCommon.CheckScriptValidation(txtRegion.Text)
                If txtRegion1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                If IsNothing(flImg.PostedFile) Then
                    'lblLogo.Text = ""            'Image
                Else
                    Dim ext As String = Path.GetExtension(flImg.PostedFile.FileName).ToString()
                    If ext <> "" Then
                        If Not (ext = ".png" Or ext = ".jpeg" Or ext = ".gif" Or ext = ".jpg") Then
                            Response.Write(clsCommon.ErrorCodeScript())
                            Exit Try
                        End If
                    End If
                End If


                lblHeading.Text = "Confirm Organization Modification"
            txtCOrgId.Text = txtOrgId.Text                      'Organisation Id
            txtCOrgName.Text = txtOrgName.Text                  'Organisation Name
            txtCBrCode.Text = txtBrCode.Text                    'Branch Code
            txtCAddress.Text = txtAddress.Text                  'Address
            lblState.Text = cmbState.SelectedItem.Text          'State
            txtCState.Text = txtState.Text                      'If Other State
            hcmbState.Value = cmbState.SelectedValue            'State to hidden box
            txtCPinCode.Text = txtPincode.Text                  'PinCode
            txtCCOuntry.Text = txtCountry.Text                  'Country
            txtCPhone1.Text = txtPhone1.Text                    'Phone 1
            txtCPhone2.Text = txtPhone2.Text                    'Phone 2
            txtCFax.Text = txtFax.Text                          'Fax
            txtCEmail.Text = txtEmail.Text                      'Email
            txtCURL.Text = txtURL.Text                          'URL
            lblLogo.Text = flImg.PostedFile.FileName            'Image
            txtCContactPerson.Text = txtContactPerson.Text      'Contact Person
            txtCContactPerIC.Text = txtContactPerIC.Text        'Contact Person IC
            txtCCustomerAdmin.Text = txtCustomerAdmin.Text      'Customer Admin
            txtCCustomAdminIC.Text = txtCustomAdminIC.Text      'Customer Admin IC
            txtCStopCharges.Text = txtStopCharges.Text          'Stop Payment Charges
                txtCAnnualFee.Text = txtAnnualFee.Text              'Subscription Fees
            txtCBusReg.Text = txtBusReg.Text                    'Business Registration Number
            txtCTaxReg.Text = txtTaxReg.Text                    'Tax Registration No
            txtCRegion.Text = txtRegion.Text                    'Region
            txtCEmployees.Text = txtEmployees.Text              'No of Employees
            txtCGroups.Text = txtGroups.Text                    'No of Groups
            If chkUseToken.Checked Then
               lblUseToken.Text = "Yes"
            Else
               lblUseToken.Text = "No"
            End If

            hchkUseToken.Value = chkUseToken.Checked.ToString

            If chkPrivilege.Checked Then
               lblPrvSub.Text = "Yes"
               hchkPrivilege.Value = "Y"
            Else
               lblPrvSub.Text = "No"
               hchkPrivilege.Value = "N"
            End If

            If rdStatus.SelectedValue = "A" Then
               lblStatus.Text = "Active"
               hchkStatus.Value = "A"
            ElseIf rdStatus.SelectedValue = "C" Then
               lblStatus.Text = "Inactive"
               hchkStatus.Value = "C"
            Else
               lblStatus.Text = "Cancelled"
               hchkStatus.Value = "D"
            End If

            If rdVerify.SelectedValue = "1" Then
               lblVerify.Text = "Single Verification"
               hVerify.Value = "1"
            Else
               lblVerify.Text = "Dual Verification"
               hVerify.Value = "2"
            End If

            'Populate Payment Service Data Grid - Start
            intRowIndex = 0
            dtPayService = (clsCustomer.fncPayService("N", 0, lngUserId)).Tables(0)

            For Each dgiPayService In dgPayService.Items

               strPayService = dgiPayService.Cells(2).Text
               Dim txtCharge As TextBox = CType(dgiPayService.FindControl("txtCharge"), TextBox)                   'Get Text Box Control
               Dim chkService As CheckBox = CType(dgiPayService.FindControl("chkService"), CheckBox)               'Get Check Box Control
               Dim hidCharge As HtmlInputHidden = CType(dgiPayService.FindControl("hidCharge"), HtmlInputHidden)   'Get Hidden Text Box

               If Not chkService.Checked Then
                  'Delete Payment Service Not Selected
                  dtPayService.Rows(intRowIndex).Delete()
               Else
                  'Check if charge per transaction is an even number - Start
                  dcTranCharge = IIf(IsNumeric(txtCharge.Text), txtCharge.Text, 0)
                  If ((dcTranCharge * 100) Mod 2) > 0 Then
                     tblForm.Visible = True
                     tblConfirm.Visible = False
                     lblOrg.Text = strPayService & " charge per transaction should be even number."
                     Exit Try
                  End If
                  'Check if charge per transaction is an even number - Stop
                  'Set Charges.
                  dtPayService.Rows(intRowIndex).Item(2) = txtCharge.Text
                  dtPayService.Rows(intRowIndex).Item(3) = hidCharge.Value
               End If
               intRowIndex = intRowIndex + 1

            Next

            dgCPayService.DataSource = dtPayService
            fncGeneralGridTheme(dgCPayService)

            dgCPayService.DataBind()
            'Populate Payment Service Data Grid - Stop

            

         Catch

            'Log Error
            Call clsGeneric.ErrorLog(0, 0, "Page Submit - PG_ModifyOrganisation", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Datagrid Item
            dgiPayService = Nothing

            'Destroy Instance of DataTable
            dtPayService = Nothing

            'Destroy Instance of DataSet
            dsPayService = Nothing

            'Destroy Instance of User Class Object
            clsUsers = Nothing

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

         End Try

      End Sub

#End Region

#Region "Submit Page"

      '****************************************************************************************************
      'Procedure Name : Submit_Page()
      'Purpose        : To Execute code during page save. 
      'Arguments      : System Object, System Event Args
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Modified       : 07/02/2005
      '*****************************************************************************************************
      Private Sub Submit_Page(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnModify.Click

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of User Class Object
         Dim clsUsers As New MaxPayroll.clsUsers

         'Create Instance of Customer Class Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'create instance of approval matrix class object
         Dim clsApprMatrix As New MaxPayroll.clsApprMatrix

         'Variable Declarations
         Dim lngOrgId As Long, lngUserId As Long, IsUser As Boolean
         Dim intApprId As Int32, strOrgName As String, strSystemAuth As String

         Try

            strOrgName = txtCOrgName.Text                                       'Organisation Name
            strSystemAuth = txtCCustomerAdmin.Text                              'System Authorizer Name
            lngOrgId = IIf(IsNumeric(hOrgId.Value), hOrgId.Value, 0)            'Get Organisation ID
            lngUserId = IIf(Session("SYS_USERID"), Session("SYS_USERID"), 0)    'Get User Id

            'Insert Organisation Details
            If Not rdStatus.SelectedValue = "D" Then    'if not organisation cancellation
               'modify organisation details
               lngOrgId = clsCustomer.fncOrganisation("U", lngOrgId, lngUserId, dgCPayService, IIf(bChkUseTokenPreviousValue = Me.chkUseToken.Checked, False, True))
            Else
               'modify organisation details
               lngOrgId = clsCustomer.fncOrganisation("D", lngOrgId, lngUserId, dgCPayService, IIf(bChkUseTokenPreviousValue = Me.chkUseToken.Checked, False, True))
            End If

            If lngOrgId = 0 Then

               trConfirm.Visible = True
               'display message
               lblOrg.Text = "Organization Modification Failed. Please try again."
               'update audit trail
               Call clsUsers.prcDetailLog(lngUserId, "Modify Organization - E" & lngOrgId, "N")

            Else

               'Check If Dual Verification - START
               If hIVerify.Value = "1" And rdVerify.SelectedValue = "2" Then
                  'Create System Authorizer
                  IsUser = clsCustomer.fnGenerateUser(8, strOrgName, strSystemAuth, "EO", lngOrgId, lngUserId)
                  If IsUser Then
                     trNew.Visible = True
                     trConfirm.Visible = False
                     lblOrg.Text = "Organization Modification Successful."
                     Call clsUsers.prcDetailLog(lngUserId, "Modify Organization - E" & lngOrgId, "Y")
                  Else
                     trConfirm.Visible = True
                     lblOrg.Text = "Organization Modification Failed. Please try again."
                     Call clsUsers.prcDetailLog(lngUserId, "Modify Organization - E" & lngOrgId, "N")
                  End If
               Else
                  If Not rdStatus.SelectedValue = "D" Then
                     trNew.Visible = True
                     trConfirm.Visible = False
                     lblOrg.Text = "Organisation Modification Successful."
                     Call clsUsers.prcDetailLog(lngUserId, "Modify Organization- E" & lngOrgId, "Y")
                  Else
                     trNew.Visible = True
                     trMain.Visible = False
                     trConfirm.Visible = False
                     'display message
                     lblOrg.Text = "Organisation Cancellation Successful. Request sent for approval."
                     'Send Requisition for Authorization 
                            intApprId = clsApprMatrix.prcApprovalMatrix(lngOrgId, lngUserId, "INSERT", 0, lngUserId, _
                                            0, lngOrgId, lngOrgId & " Organisation Cancelled", "Cancel Organization", "", 1)
                     'update audit trail
                     Call clsUsers.prcDetailLog(lngUserId, "Modify Organization- E" & lngOrgId, "Y")
                  End If

                  'Audit Trail
                  Call prcAuditTrail(lngOrgId, lngUserId, intApprId)

               End If
            End If
            'Check if Dual Verification - STOP
            Dim clsCommon As New clsCommon
            
            'Send Mail
                Call clsCommon.prcSendMails("BANK AUTH", 100000, ss_lngUserID, 0, "Organization has been modified", "Organization has been modified.", 0)
         Catch

            'Error Message
            lblOrg.Text = "Organisation Modification Failed"

            'Log Error
            Call clsGeneric.ErrorLog(100000, lngUserId, "PG_ModifyOrganisation - Submit_Page", Err.Number, Err.Description)

         Finally

            'Destroy Instance of Users Class Object
            clsUsers = Nothing

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

            'destroy instance of apporval matrix class object
            clsApprMatrix = Nothing

         End Try

      End Sub

#End Region

#Region "Audit Trail"

      '****************************************************************************************************
      'Procedure Name : prcAuditTrail()
      'Purpose        : To Update the Audit Trail Table 
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Modified       : 07/05/2005
      '*****************************************************************************************************
      Private Sub prcAuditTrail(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal intApprId As Int32)

         'Create Instance of User Class Object
         Dim clsUser As New MaxPayroll.clsUsers

         'create instance of datagrid item
         Dim dgiPayService As DataGridItem

         'Variable Declarations
         Dim strNewData As String, strOldData As String, strPayService As String

         Try

            'Audit Trail - START
            If Not txtCAddress.Text = AtxtAddr.Text Then
               'Track Address Change
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Address", _
                       txtCAddress.Text, AtxtAddr.Text, lngUserId, "O", 0)
            End If

            If Not txtCGroups.Text = AhGroups.Value Then
               'Track Group Limit Change
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Group", _
                       txtCGroups.Text, AhGroups.Value, lngUserId, "O", 0)
            End If

            If Not txtCContactPerson.Text = AhCA.Value Then
               'Track Customer Admin Change
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Customer Admin", _
                       txtCContactPerson.Text, AhCA.Value, lngUserId, "O", 0)
            End If

            If Not txtCCustomerAdmin.Text = AhSA.Value Then
               'Track System Auth Change
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "System Auth", _
                       txtCCustomerAdmin.Text, AhSA.Value, lngUserId, "O", 0)
            End If

            If Not txtCStopCharges.Text = AhStopChrg.Value Then
               'Track Stop Charge
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Stop Payment Charge", _
                       txtCStopCharges.Text, AhStopChrg.Value, lngUserId, "O", 0)
            End If

            If Not txtCAnnualFee.Text = AhAnnFees.Value Then
                    'Track Subscription Fees
                    Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Subscription Fees", _
                       txtCAnnualFee.Text, AhAnnFees.Value, lngUserId, "O", 0)
            End If

            'Track Privilege Change
            If chkPrivilege.Checked And hAPriv.Value = "N" Then
               strOldData = "No"
               strNewData = "Yes"
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Privilege", _
                       strNewData, strOldData, lngUserId, "O", 0)
            ElseIf Not chkPrivilege.Checked And hAPriv.Value = "Y" Then
               strOldData = "Yes"
               strNewData = "No"
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Privilege", _
                       strNewData, strOldData, lngUserId, "O", 0)
            End If

            'Track Verify Change
            If Not rdVerify.SelectedValue = hAVerify.Value Then
               strOldData = "Single"
               strNewData = "Dual"
               Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Verification", _
                       strNewData, strOldData, lngUserId, "O", 0)
            End If

            If Not rdStatus.SelectedValue = hStatus.Value Then
               If rdStatus.SelectedValue = "A" Then
                  strNewData = "Active"
               ElseIf rdStatus.SelectedValue = "C" Then
                  strNewData = "Inactive"
               ElseIf rdStatus.SelectedValue = "D" Then
                  strNewData = "Cancelled"
               End If
               If hStatus.Value = "A" Then
                  strOldData = "Active"
               ElseIf hStatus.Value = "C" Then
                  strOldData = "Inactive"
               ElseIf hStatus.Value = "D" Then
                  strOldData = "Cancelled"
               End If

               If Not rdStatus.SelectedValue = "D" Then
                  Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Status", _
                                          strNewData, strOldData, lngUserId, "O", 0)
               Else
                  Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", "Status", _
                                          strNewData, strOldData, lngUserId, "O", intApprId)
               End If
            End If

            'Track transaction fee changes - start
            For Each dgiPayService In dgCPayService.Items

               strOldData = dgiPayService.Cells(3).Text
               strNewData = dgiPayService.Cells(2).Text
               strPayService = dgiPayService.Cells(1).Text

               If Not strNewData = strOldData Then
                  Call clsUser.prcModifyLog(0, lngOrgId, "Modify Organisation", strPayService, _
                      strNewData, strOldData, lngUserId, "O", 0)
               End If

            Next

            'Audit Trail - STOP

         Catch Ex As Exception

         Finally

            'Destroy Instance of User Class Object
            clsUser = Nothing

         End Try

      End Sub

#End Region

   End Class

End Namespace
