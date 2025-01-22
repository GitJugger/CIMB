Option Strict Off
Option Explicit On 

Imports System.IO
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsCustomer
'Imports MaxPayroll.clsCheckDigit


Namespace MaxPayroll


Partial Class PG_Organisation
        Inherits clsBasePage

#Region "Page Load"

    '****************************************************************************************************
    'Procedure Name : Page_Load()
    'Purpose        : To Execute code during page load. 
    'Arguments      : System Object, System Event Args
    'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
    'Modified       : 08/07/2004
    '*****************************************************************************************************
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of Customer Class Object
        Dim clsCustomer As New MaxPayroll.clsCustomer

        'Create Instance of System Data Set
        Dim dsState As New System.Data.DataSet


            Try

            If Not ss_strUserType = gc_UT_BankUser Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then
                    'BindBody(body)
               lblHeading.Text = "Create Organization"

               'Populate StateCode,State in Combo box
               dsState = clsCustomer.GetState(ss_lngOrgId, ss_lngUserID)
               cmbState.DataSource = dsState
               cmbState.DataTextField = "State"
               cmbState.DataValueField = "StateCode"
               cmbState.DataBind()

               'Populate Payment Service Data Grid
               fncGeneralGridTheme(dgPayService)
               dsState = clsCustomer.fncPayService("N", ss_lngOrgId, ss_lngUserID)
               dgPayService.DataSource = dsState
               dgPayService.DataBind()

            End If

         Catch ex As Exception

            LogError("PG_Organization - Page Load")

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of Customer Class Object
            clsCustomer = Nothing

            'Destroy Instance of System Data Set
            dsState = Nothing

         End Try


    End Sub

#End Region

#Region "Page Confirm"

    '****************************************************************************************************
    'Procedure Name : Submit_Page()
    'Purpose        : To Execute code during page save. 
    'Arguments      : System Object, System Event Args
    'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Modified       : 08/07/2004
        '*****************************************************************************************************
        Public Sub Confirm_Page(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of DatagridItem
            Dim dgiPayService As DataGridItem

            'Create Instance of System Data Set
            Dim dsPayService As New System.Data.DataSet

            'Create Instance of DataTable
            Dim dtPayService As New System.Data.DataTable

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim IsDuplicate As Boolean, strValue As String, lngUserId As Long, intRowIndex As Int16
            Dim dcTranCharge As Decimal, strPayService As String, IsChecked As Boolean
            Dim strBody As String = ""
            Try

                'Get Logged User ID
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Check If State Others - Start
                If cmbState.SelectedValue = 15 And txtState.Text = "" Then
                    tblForm.Visible = True
                    tblConfirm.Visible = False
                    lblOrg.Text = "If State is Others, Please enter value for Other."
                    Exit Try
                End If
                'Check If State Others - Stop

                'Business Registration No
                strValue = txtBusReg.Text
                If strValue <> "" Then
                    IsDuplicate = clsCustomer.fnOrgValidations("ADD", "REG NO", strValue, 0)
                    If IsDuplicate Then
                        tblForm.Visible = True
                        tblConfirm.Visible = False
                        lblOrg.Text = "Business Registration No Cannot be Duplicated."
                        Exit Try
                    End If
                End If
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtOrgName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim strphone1 As Boolean = clsCommon.CheckScriptValidation(txtPhone1.Text)
                If strphone1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim strphone2 As Boolean = clsCommon.CheckScriptValidation(txtPhone2.Text)
                If strphone2 = False Then
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
                    'lblLogo.Text = flImg.PostedFile.FileName            'Image
                    Dim ext As String = Path.GetExtension(flImg.PostedFile.FileName).ToString()
                    If ext <> ".png" Or ext <> ".jpeg" Or ext <> ".gif" Then
                        Response.Write(clsCommon.ErrorCodeScript())
                        Exit Try
                    End If
                End If
                tblForm.Visible = False
                tblConfirm.Visible = True

                lblOrg.Text = "Please Confirm Organization Details"
                lblHeading.Text = "Confirm Organisation Creation"
                txtCOrgName.Text = txtOrgName.Text                  'Organisation Name
                txtCBrCode.Text = txtBrCode.Text                    'Branch Code
                txtCAddress.Text = txtAddress.Text                  'Address
                lblState.Text = cmbState.SelectedItem.Text          'State
                txtCState.Text = txtState.Text                      'If Other State
                hcmbState.Value = cmbState.SelectedValue            'State to hidden box
                txtCPincode.Text = txtPincode.Text                  'PinCode
                txtCCountry.Text = txtCountry.Text                  'Country
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
                Else
                    lblStatus.Text = "Inactive"
                    hchkStatus.Value = "C"
                End If

                'Populate Payment Service Data Grid - Start
                intRowIndex = 0
                dtPayService = (clsCustomer.fncPayService("N", 0, lngUserId)).Tables(0)

                For Each dgiPayService In dgPayService.Items

                    strPayService = dgiPayService.Cells(1).Text
                    Dim txtCharge As TextBox = CType(dgiPayService.FindControl("txtCharge"), TextBox)                   'Get Text Box Control
                    Dim chkService As CheckBox = CType(dgiPayService.FindControl("chkService"), CheckBox)               'Get Check Box Control

                    If Not chkService.Checked Then
                        'Delete Payment Service Not Selected
                        dtPayService.Rows(intRowIndex).Delete()
                    Else
                        IsChecked = True
                        'Check if charge per transaction is an even number - Start
                        dcTranCharge = IIf(IsNumeric(txtCharge.Text), txtCharge.Text, 0)
                        dcTranCharge = System.Math.Round(dcTranCharge, 2)
                        If ((dcTranCharge * 100) Mod 2) > 0 Then
                            tblForm.Visible = True
                            tblConfirm.Visible = False
                            lblOrg.Text = strPayService & " charge per transaction should be even number."
                            Exit Try
                        End If
                        'Check if charge per transaction is an even number - Stop
                        'Set Charges.
                        dtPayService.Rows(intRowIndex).Item(2) = txtCharge.Text
                    End If
                    intRowIndex = intRowIndex + 1

                Next

                If Not IsChecked Then
                    tblForm.Visible = True
                    tblConfirm.Visible = False
                    lblOrg.Text = "Please select a payment service"
                    Exit Try
                End If

                dgCPayService.DataSource = dtPayService
                dgCPayService.DataBind()
                'Populate Payment Service Data Grid - Stop

           
            Catch

                'Error Message
                lblOrg.Text = "Organisation Creation Failed."

                'Log Error
                Call clsGeneric.ErrorLog(0, lngUserId, "PG_Organisation - Confirm_Page", Err.Number, Err.Description)

            Finally

                'Destroy Instance of DatagridItem
                dgiPayService = Nothing

                'Destroy Instance of DataTable
                dtPayService = Nothing

                'Destroy Instance of DataSet
                dsPayService = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

#Region "Page Save"

        '****************************************************************************************************
        'Procedure Name : Submit_Page()
        'Purpose        : To Execute code during page save. 
        'Arguments      : System Object, System Event Args
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Modified       : 06/02/2005
        '*****************************************************************************************************
        Private Sub Submit_Page(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim lngOrgId As Long, lngUserId As Long, IsUser As Boolean, strOrgName As String
            Dim strSystemAdmin As String, strSystemAuth As String, strOrg As String
            Dim strBody As String = ""
            Try

                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtCOrgName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtCCustomerAdmin1 As Boolean = clsCommon.CheckScriptValidation(txtCCustomerAdmin.Text)
                If txtCCustomerAdmin1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                Dim txtCContactPerson1 As Boolean = clsCommon.CheckScriptValidation(txtCContactPerson.Text)
                If txtCContactPerson1 = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                'Insert Organisation Details
                strOrgName = txtCOrgName.Text
                strSystemAuth = txtCCustomerAdmin.Text
                strSystemAdmin = txtCContactPerson.Text

                'Insert New Organisation Details
                lngOrgId = clsCustomer.fncOrganisation("I", 0, lngUserId, dgCPayService)
                If lngOrgId = 0 Then
                    lblOrg.Text = "Organisation Creation Failed. Please try again."
                    Exit Try
                End If

                If lngOrgId > 0 Then

                    'Create System Administrator
               IsUser = clsCustomer.fnGenerateUser(8, strOrgName, strSystemAdmin, gc_UT_SysAdmin, _
                                    lngOrgId, lngUserId)

                    'If Dual Verification
                    If IsUser Then
                        'create system authorizer
                  IsUser = clsCustomer.fnGenerateUser(8, strOrgName, strSystemAuth, gc_UT_SysAuth, _
                                        lngOrgId, lngUserId)
                        If Not IsUser Then
                            strOrg = clsCommon.fncBuildContent("Org Del", "", lngOrgId, lngUserId)
                        End If
                    ElseIf Not IsUser Then
                        strOrg = clsCommon.fncBuildContent("Org Del", "", lngOrgId, lngUserId)
                    End If

                    If IsUser Then
                        trNew.Visible = True
                        trConfirm.Visible = False
                        lblOrg.Text = "Organization E" & lngOrgId & " Created Successfully."
                        Call clsUsers.prcDetailLog(lngUserId, "Create Organization - E" & lngOrgId, "Y")
                    Else
                        lblOrg.Text = "Organisation Creation Failed. Please try again."
                        Call clsUsers.prcDetailLog(lngUserId, "Create Organization", "N")
                    End If

                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, lngUserId, "PG_Organisation - Submit_Page", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Uses Class Object
                clsUsers = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

            End Try

        End Sub

#End Region

End Class

End Namespace
