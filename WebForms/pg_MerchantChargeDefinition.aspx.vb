Imports System.Math
Imports System.Data.SqlClient

Namespace MaxPayroll


    Partial Class pg_MerchantChargeDefinition
      Inherits clsBasePage

#Region "Request.QueryString"
      Private ReadOnly Property rq_strOrgID() As String
         Get
            Return Request.QueryString("ID") & ""
         End Get
      End Property

      Private ReadOnly Property rq_strRole() As String
         Get
            Return Request.QueryString("Role") & ""
         End Get
      End Property
      ReadOnly Property rq_ePageMode() As enmPageMode
         Get
            Try
               Return CType(Trim(Request.QueryString("PageMode") & ""), enmPageMode)
            Catch ex As Exception
               Return enmPageMode.NewMode
            End Try
         End Get
      End Property
#End Region

#Region "Page Load"

      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring new instance of testMaxPG.clsGeneric
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring new instance of testMaxPG.clsUsers
         Dim clsUsers As New MaxPayroll.clsUsers

         Try
            'Check if valid user type
            If ss_strUserType.Equals(gc_UT_BankUser) = False Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then
                    'BindBody(body)
               'Bind Payment type to dropdownlist based on organization id
               prcBindPaymentService()

               ' Displaying merchant information
               prcDisplayMerchantInfo()

               ' Display appropriate account number to dropdownlist based on model type selected.
               prcBindChargeSettleAcc(ddlModelType.SelectedValue)
               If rq_ePageMode.Equals(enmPageMode.EditMode) Then
                  If Me.ddlModelType.Items.Count > 0 Then
                     prcRetrieveData()
                     btnSubmit.Text = "Update"
                     btnClear.Visible = False
                     btnNew.Text = "Modify Organization Charges"
                  Else
                     lblErrorMsg.Text = "Payment Services not created for this Organization [E" & Me.rq_strOrgID & "]."
                     fncDisableFormControls()
                  End If

               End If


            End If

         Catch ex As Exception

            'Log Error
            Call clsGeneric.ErrorLog("Page_Load - frmMerchChargeDefinition", Err.Number, Err.Description)
         End Try

      End Sub
#End Region

#Region "Retrieve Data"
      Private Sub prcRetrieveData()
         Dim clsCustChg As New clsCustChg
         Dim drInfo As SqlDataReader
         Dim lItem As ListItem
         Dim bBindDDLSettleAcc As Boolean = False
         Dim intlItemIndex As Int16 = 0

         Try
            drInfo = clsCustChg.fncQryCustomerChg(CLng(rq_strOrgID), Me.ddlModelType.SelectedValue)
            While drInfo.Read
               hidCustChargeID.Value = CStr(drInfo("CustChargeID")).Trim

               'Loop through and compare all the items within the dropdownlist
               For Each lItem In Me.ddlChargeSettleAcc.Items
                  If lItem.Text.Equals(CStr(drInfo("CustSettleAcct") & "").Trim) Then
                     bBindDDLSettleAcc = True
                     Exit For
                  End If
                  intlItemIndex += 1
               Next

               If bBindDDLSettleAcc Then
                  txtChargeSettleAcc.Text = ""
                  ddlChargeSettleAcc.SelectedIndex = intlItemIndex '.Text = CStr(drInfo("CustSettleAcct") & "").Trim
               Else
                  txtChargeSettleAcc.Text = CStr(drInfo("CustSettleAcct") & "").Trim
               End If

               ddlBillFrequency.SelectedValue = CStr(drInfo("CustBillFreq") & "").Trim
               radMerchBill.SelectedValue = CStr(drInfo("CustBillType") & "").Trim
               hChargeType.Value = CStr(drInfo("CustChgType") & "").Trim
               Select Case hChargeType.Value
                  Case "F"
                     radFixedRate.Checked = True
                     radPercentRate.Checked = False
                     radTierRate.Checked = False
                     Me.txtFixedRate.Text = CStr(drInfo("CustFixedRate")).Trim
                     Me.txtPercentRate.Text = ""
                     Me.txtMinAmt.Text = ""
                     Me.txtMaxAmt.Text = ""
                  Case "P"
                     radFixedRate.Checked = False
                     radPercentRate.Checked = True
                     radTierRate.Checked = False
                     Me.txtFixedRate.Text = ""
                     Me.txtPercentRate.Text = CStr(drInfo("CustPercentRate")).Trim
                     Me.txtMinAmt.Text = CStr(drInfo("CustPercentMinAmt")).Trim
                     Me.txtMaxAmt.Text = CStr(drInfo("CustPercentMaxAmt")).Trim
                  Case "T"
                     radFixedRate.Checked = False
                     radPercentRate.Checked = False
                     radTierRate.Checked = True
                     Me.txtFixedRate.Text = ""
                     Me.txtPercentRate.Text = ""
                     Me.txtMinAmt.Text = ""
                     Me.txtMaxAmt.Text = ""
               End Select

            End While
            drInfo.Close()
            drInfo = Nothing

         Catch ex As Exception
            LogError("pg_MerchantChargeDefinition - prcRetrieveData")
            lblErrorMsg.Text = "Encounter some problem in data loading."
         End Try
      End Sub
#End Region

#Region "Set controls in the forms as readonly"

      Public Function fncReadOnlyControl()

         Try

            txtSellerID.ReadOnly = True
            txtMerchName.ReadOnly = True

         Catch Ex As SystemException

         End Try
         Return Nothing
      End Function

#End Region

#Region "Clear all the merchant charges contents"

      Public Sub prcClearChargeContent()

         Try

            'Clear the charge type information
            txtFixedRate.Text = ""
            txtMinTier1.Text = ""
            txtMaxTier1.Text = ""
            txtRateTier1.Text = ""
            txtMinTier2.Text = ""
            txtMaxTier2.Text = ""
            txtRateTier2.Text = ""
            txtMinTier3.Text = ""
            txtMaxTier3.Text = ""
            txtRateTier3.Text = ""
            txtPercentRate.Text = ""
            txtMinAmt.Text = ""
            txtMaxAmt.Text = ""

            'Clear charge type radio button
            radFixedRate.Checked = False
            radPercentRate.Checked = False
            radTierRate.Checked = False

            radMerchBill.SelectedValue = "A"

            ''Added By Nor Affarina 26/09/2006
            txtChargeSettleAcc.Text = ""

         Catch Ex As SystemException
            Throw Ex
         End Try
      End Sub

#End Region

#Region "Enable charge controls for insertion"

      Public Sub fncEnableChargeControls()

         Try

            'Disabling all the textbox control in the form
            txtSellerID.Enabled = True
            txtMerchName.Enabled = True
            txtFixedRate.Enabled = True
            txtMinTier1.Enabled = True
            txtMaxTier1.Enabled = True
            txtRateTier1.Enabled = True
            txtMinTier2.Enabled = True
            txtMaxTier2.Enabled = True
            txtRateTier2.Enabled = True
            txtMinTier3.Enabled = True
            txtMaxTier3.Enabled = True
            txtRateTier3.Enabled = True
            txtPercentRate.Enabled = True
            txtMinAmt.Enabled = True
            txtMaxAmt.Enabled = True
            txtChargeSettleAcc.Enabled = True

            'Disabling all the dropdownlist control in the form
            ddlBillFrequency.Enabled = True
            ddlChargeSettleAcc.Enabled = True
            ddlModelType.Enabled = True

            'Disabling all the radiobutton control in the form
            radFixedRate.Enabled = True
            radTierRate.Enabled = True
            radPercentRate.Enabled = True
            radMerchBill.Enabled = True


         Catch Ex As SystemException

         End Try

      End Sub

#End Region

#Region "Disable all the controls in the form"

      '********************************************************************************************************************************
      'Function Name      : fncDisableFormControls
      'Purpose            : Provide the functionality to disable all the controls when no longer required by the form
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      '********************************************************************************************************************************
      Private Sub fncDisableFormControls()


         Try

            'Disabling all the textbox control in the form
            txtSellerID.ReadOnly = True
            txtMerchName.ReadOnly = True
            txtFixedRate.ReadOnly = True
            txtMinTier1.ReadOnly = True
            txtMaxTier1.ReadOnly = True
            txtRateTier1.ReadOnly = True
            txtMinTier2.ReadOnly = True
            txtMaxTier2.ReadOnly = True
            txtRateTier2.ReadOnly = True
            txtMinTier3.ReadOnly = True
            txtMaxTier3.ReadOnly = True
            txtRateTier3.ReadOnly = True
            txtPercentRate.ReadOnly = True
            txtMinAmt.ReadOnly = True
            txtMaxAmt.ReadOnly = True

            txtChargeSettleAcc.ReadOnly = True
            txtChargeSettleAcc.ReadOnly = True


            'Disabling all the dropdownlist control in the form
            ddlBillFrequency.Enabled = False
            ddlChargeSettleAcc.Enabled = False
            ddlModelType.Enabled = False

            'Disabling all the radiobutton control in the form
            radFixedRate.Enabled = False
            radTierRate.Enabled = False
            radPercentRate.Enabled = False
            radMerchBill.Enabled = False


         Catch Ex As SystemException

         Finally

         End Try

      End Sub

#End Region

#Region "Disable and hide unwanted charge sharing controls from the view"

      '********************************************************************************************************************************
      'Function Name      : fncDisableChargeSharingControls
      'Purpose            : Provide functionality to disable unwanted charge sharing control in the forms
      'Arguments          : Model type
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      '********************************************************************************************************************************
      Private Function fncDisableChargeSharingControls()

         Try

            Return Nothing

         Catch Ex As SystemException

         End Try
         Return Nothing
      End Function


#End Region

#Region "Disable charge type controls from the view"

      '********************************************************************************************************************************
      'Function Name      : fncDisableChargeTypeControls
      'Purpose            : Provide functionality to disable unwanted charge sharing control in the forms
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Last Modified By   : Nor Affarina Muhamad Appandi T-Melmax Sdn Bhd
      'Last Modified Date : 25 /09/2006
      'Reason             : Incorporate with radio button 
      '********************************************************************************************************************************
      Private Function fncDisableChargeTypeControls()

         Try


            If radFixedRate.Checked = True Then

               txtMinTier1.Enabled = False
               txtMaxTier1.Enabled = False
               txtMinTier2.Enabled = False
               txtMaxTier2.Enabled = False
               txtRateTier1.Enabled = False
               txtRateTier2.Enabled = False
               txtPercentRate.Enabled = False
               txtMaxAmt.Enabled = False
               txtMinAmt.Enabled = False
               txtFixedRate.Enabled = True

            ElseIf radPercentRate.Checked = True Then

               txtMinTier1.Enabled = False
               txtMaxTier1.Enabled = False
               txtMinTier2.Enabled = False
               txtMaxTier2.Enabled = False
               txtRateTier1.Enabled = False
               txtRateTier2.Enabled = False
               txtPercentRate.Enabled = True
               txtMaxAmt.Enabled = True
               txtMinAmt.Enabled = True
               txtFixedRate.Enabled = False

            ElseIf radTierRate.Checked = True Then

               txtMinTier1.Enabled = True
               txtMaxTier1.Enabled = True
               txtMinTier2.Enabled = True
               txtMaxTier2.Enabled = True
               txtRateTier1.Enabled = True
               txtRateTier2.Enabled = True
               txtPercentRate.Enabled = False
               txtMaxAmt.Enabled = False
               txtMinAmt.Enabled = False
               txtFixedRate.Enabled = False

            Else
               'Disable all charge type controls in the form
               txtMinTier1.Enabled = False
               txtMaxTier1.Enabled = False
               txtMinTier2.Enabled = False
               txtMaxTier2.Enabled = False
               txtRateTier1.Enabled = False
               txtRateTier2.Enabled = False
               txtPercentRate.Enabled = False
               txtMaxAmt.Enabled = False
               txtMinAmt.Enabled = False
               txtFixedRate.Enabled = False
            End If



         Catch Ex As SystemException

         End Try

      End Function

#End Region

#Region "Disable charge type controls from the view"

      '********************************************************************************************************************************
      'Function Name      : fncDisableFPXChargeTypeControls
      'Purpose            : Provide functionality to disable unwanted charge sharing control in the forms
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Nor Affarina Muhamad Appandi T-Melmax Sdn Bhd
      'Date               : 25 /09/2006
      'Reason             : Disable FPX Charge Area
      '********************************************************************************************************************************
      Private Function fncDisableFPXChargeTypeControls()

         Try

            'Disable all charge type controls in the form
            txtMinTier1.Enabled = False
            txtMaxTier1.Enabled = False
            txtMinTier2.Enabled = False
            txtMaxTier2.Enabled = False
            txtRateTier1.Enabled = False
            txtRateTier2.Enabled = False
            txtPercentRate.Enabled = False
            txtMaxAmt.Enabled = False
            txtMinAmt.Enabled = False
            txtFixedRate.Enabled = False


         Catch Ex As SystemException

         End Try
         Return Nothing
      End Function

#End Region

      Private Function fncEnableAllControls() As String

         'Disabling all the textbox control in the form
         txtSellerID.Enabled = True
         txtMerchName.Enabled = True
         txtFixedRate.Enabled = True
         txtMinTier1.Enabled = True
         txtMaxTier1.Enabled = True
         txtRateTier1.Enabled = True
         txtMinTier2.Enabled = True
         txtMaxTier2.Enabled = True
         txtRateTier2.Enabled = True
         txtMinTier3.Enabled = True
         txtMaxTier3.Enabled = True
         txtRateTier3.Enabled = True
         txtPercentRate.Enabled = True
         txtMinAmt.Enabled = True
         txtMaxAmt.Enabled = True

         txtChargeSettleAcc.Enabled = True

         'Disabling all the dropdownlist control in the form
         ddlBillFrequency.Enabled = True
         'ddlcBillFrequency.Enabled = True
         ddlChargeSettleAcc.Enabled = True
         'added by Affarina 17/08/2006 
         ddlModelType.Enabled = True

         'Disabling all the radiobutton control in the form
         radFixedRate.Enabled = True
         radTierRate.Enabled = True
         radPercentRate.Enabled = True
         radMerchBill.Enabled = True



         fncReadOnlyControl()
         Return Nothing
      End Function


#Region "Display merchant charge type information"

      '********************************************************************************************************************************
      'Function Name      : fncDisplayChargeTypeInfo
      'Purpose            : Provide functionality to display merchant charge sharing information based on model type selected
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      '********************************************************************************************************************************
      Private Sub fncDisplayChargeTypeInfo()

         Try

            'Disable all charge type control
            'fncDisableChargeTypeControls()

            If radFixedRate.Checked = True Then

               rfvtxtFR.Enabled = True
               rfvTxtPR.Enabled = False
               rfvtxtMinAmtMerch.Enabled = False
               rfvtxtMaxAmtMerch.Enabled = False
               rfvtxtTierMin1Merch.Enabled = False
               rfvtxtTierMin2Merch.Enabled = False
               rfvtxtTierMax1Merch.Enabled = False
               rfvtxtTierMax2Merch.Enabled = False
               rfvtxtTierRate1Merch.Enabled = False
               rfvtxtTierRate2Merch.Enabled = False

               'In case of fixed rate been selected, enable its controls for data entry
               txtFixedRate.Enabled = True
               txtPercentRate.Enabled = False
               txtMaxAmt.Enabled = False
               txtMinAmt.Enabled = False
               'Added by Affarina 18/09/2006 to clear unrelated informations
               txtPercentRate.Text = ""
               txtMaxAmt.Text = ""
               txtMinAmt.Text = ""
               txtMinTier1.Text = ""
               txtMaxTier1.Text = ""
               txtMinTier2.Text = ""
               txtMaxTier2.Text = ""
               txtRateTier1.Text = ""
               txtRateTier2.Text = ""

            ElseIf radPercentRate.Checked = True Then

               rfvtxtFR.Enabled = False
               rfvTxtPR.Enabled = True
               rfvtxtMinAmtMerch.Enabled = True
               rfvtxtMaxAmtMerch.Enabled = True
               rfvtxtTierMin1Merch.Enabled = False
               rfvtxtTierMin2Merch.Enabled = False
               rfvtxtTierMax1Merch.Enabled = False
               rfvtxtTierMax2Merch.Enabled = False
               rfvtxtTierRate1Merch.Enabled = False
               rfvtxtTierRate2Merch.Enabled = False

               'In case of percentage rate been selected, enable its controls for data entry
               txtPercentRate.Enabled = True
               txtMaxAmt.Enabled = True
               txtMinAmt.Enabled = True
               txtFixedRate.Enabled = False
               'Added by Affarina 18/09/2006 to clear unrelated informations
               txtFixedRate.Text = ""
               txtMinTier1.Text = ""
               txtMaxTier1.Text = ""
               txtMinTier2.Text = ""
               txtMaxTier2.Text = ""
               txtRateTier1.Text = ""
               txtRateTier2.Text = ""

            ElseIf radTierRate.Checked = True Then
               rfvtxtFR.Enabled = False
               rfvTxtPR.Enabled = False
               rfvtxtMinAmtMerch.Enabled = False
               rfvtxtMaxAmtMerch.Enabled = False
               rfvtxtTierMin1Merch.Enabled = True
               rfvtxtTierMin2Merch.Enabled = True
               rfvtxtTierMax1Merch.Enabled = True
               rfvtxtTierMax2Merch.Enabled = True
               rfvtxtTierRate1Merch.Enabled = True
               rfvtxtTierRate2Merch.Enabled = True

               'In case of tier rate been selected, enable its controls for data entry
               txtMinTier1.Enabled = True
               txtMaxTier1.Enabled = True
               txtMinTier2.Enabled = True
               txtMaxTier2.Enabled = True
               txtRateTier1.Enabled = True
               txtRateTier2.Enabled = True
               'Added by Affarina 18/09/2006 to clear unrelated informations
               txtPercentRate.Text = ""
               txtMaxAmt.Text = ""
               txtMinAmt.Text = ""
               txtFixedRate.Text = ""

            End If

         Catch Ex As SystemException

         End Try

      End Sub

#End Region

#Region "Display tier rate charge information"
      '********************************************************************************************************************************
      'Function Name      : fncDisplayMerchantTierCharge
      'Purpose            : Provide functionality to display merchant information
      'Arguments          : Merchant Charge Id
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Modified By        : Nor Affarina Muhamad Appandi T-Melmax Sdn Bhd
      'Modified Date      : 09/11/2006
      'Reason             : Assign proper tier value to proper txtbox
      '********************************************************************************************************************************
      Private Function fncDisplayTierCharge(ByVal intChargeId As Int16) As String

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring new instance of System.DataSet
         Dim dsTierRate As New DataSet

         'Declaring new instance of System.DataRow
         Dim drTierRate As DataRow

         Try

            'Getting tier rate information from the database
            dsTierRate = clsCustChg.fncDisplayTierChargeInfo(intChargeId)

            If Not dsTierRate.Tables("Display").Rows.Count = 0 Then

               For Each drTierRate In dsTierRate.Tables("Display").Rows

                  If Trim(drTierRate("TierNo")) = 1 Then

                     'Display tier rate 1 information
                     txtMinTier1.Text = IIf(IsDBNull(drTierRate("TierFrom")), "", drTierRate("TierFrom"))
                     txtMaxTier1.Text = IIf(IsDBNull(drTierRate("TierTo")), "", drTierRate("TierTo"))
                     txtRateTier1.Text = IIf(IsDBNull(drTierRate("TierAmt")), "", drTierRate("TierAmt"))

                  ElseIf Trim(drTierRate("TierNo")) = 2 Then

                     'Display tier rate 2 information
                     txtMaxTier2.Text = IIf(IsDBNull(drTierRate("TierFrom")), "", drTierRate("TierFrom"))
                     txtMinTier2.Text = IIf(IsDBNull(drTierRate("TierTo")), "", drTierRate("TierTo"))
                     txtRateTier2.Text = IIf(IsDBNull(drTierRate("TierAmt")), "", drTierRate("TierAmt"))


                  ElseIf Trim(drTierRate("TierNo")) = 3 Then

                     'Display tier rate 3 information
                     txtMinTier3.Text = IIf(IsDBNull(drTierRate("TierFrom")), "", drTierRate("TierFrom"))
                     txtMaxTier3.Text = IIf(IsDBNull(drTierRate("TierTo")), "", drTierRate("TierTo"))
                     txtRateTier3.Text = IIf(IsDBNull(drTierRate("TierAmt")), "", drTierRate("TierAmt"))

                  End If

               Next

            End If

         Catch Ex As SystemException

         End Try

      End Function

#End Region

#Region "Display charge information"

      '********************************************************************************************************************************
      'Function Name      : fncDisplayCharge
      'Purpose            : Provide functionality to display charge information from the database
      '                   : screen
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      '********************************************************************************************************************************
      Private Function fncDisplayCharge(ByVal strModel As String) As String

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring new instance of System.DataSet
         Dim dsMerchantInfo As New DataSet

         'Declaring new instance of System.DataRow
         Dim drMerchantInfo As DataRow

         'Declaring local variables
         Dim intChargeId As Int16, strStatus As String, strAlternateAcc As String

         Try

            'Getting charge information from the database
            dsMerchantInfo = clsCustChg.fncDisplayMerchantChargesInfo(rq_strOrgID, ddlModelType.SelectedValue)

            If Not dsMerchantInfo.Tables("Display").Rows.Count = 0 Then

               'Process each rows returned by the DataSet
               For Each drMerchantInfo In dsMerchantInfo.Tables("Display").Rows

                  'Assigning charge id to global variable
                  intChargeId = Trim(drMerchantInfo("ChargeID"))

                  'Displaying billing frequency information
                  ddlBillFrequency.SelectedValue = Trim(drMerchantInfo("Frequency"))

                  'Added by Affarina Muhamad Appandi T-melmax 06102006
                  'Getting alternate charge
                  strAlternateAcc = IIf(Trim(drMerchantInfo("SettleAccount")) = "0", "", Trim(drMerchantInfo("SettleAccount")))

                  If Trim(strAlternateAcc) = Trim(ddlChargeSettleAcc.SelectedItem.Text) Then

                     txtChargeSettleAcc.Text = ""

                  Else : txtChargeSettleAcc.Text = strAlternateAcc

                  End If
                  'End Added by Affarina Muhamad Appandi T-melmax 06102006

                  'Displaying billing method information
                  If Trim(drMerchantInfo("BillingType")) = "M" Then
                     radMerchBill.Items(0).Selected = True
                  ElseIf Trim(drMerchantInfo("BillingType")) = "A" Then
                     radMerchBill.Items(1).Selected = True
                  End If

                  If Trim(drMerchantInfo("ChargeType")) = "T" Then
                     radTierRate.Checked = True

                     'Getting the charge id from the DataSet
                     intChargeId = IIf(IsDBNull(drMerchantInfo("ChargeId")), "", drMerchantInfo("ChargeId"))

                     'Displaying the tier rate information
                     fncDisplayTierCharge(intChargeId)

                  ElseIf Trim(drMerchantInfo("ChargeType")) = "P" Then
                     radPercentRate.Checked = True

                     'Displaying percent rate information
                     txtPercentRate.Text = Trim(drMerchantInfo("PercentRate"))
                     txtMaxAmt.Text = IIf(IsDBNull(drMerchantInfo("PercentMax")), "", drMerchantInfo("PercentMax"))
                     txtMinAmt.Text = IIf(IsDBNull(drMerchantInfo("PercentMin")), "", drMerchantInfo("PercentMin"))

                  ElseIf Trim(drMerchantInfo("ChargeType")) = "F" Then
                     radFixedRate.Checked = True

                     'Displaying fixed rate information
                     txtFixedRate.Text = IIf(IsDBNull(drMerchantInfo("FixedRate")), "", drMerchantInfo("FixedRate"))

                  End If

                  'Displaying charge sharing information
                  'fncDisplayChargeSharingInfo(strModel, strMerchId)

                  'Hide acknowledgement screen from the user
                  'tblAck.Visible = False
                  'commented by eric
                  'tblAck.Visible = True

                  'In case charges has already been defined for current model, display alert to the user
                  lblAlert.Text = "Merchant charges for current model has already been defined"
                  lblAlert.Visible = True
                  strStatus = "Message"
                  'added by Nor Affarina 18092006
                  trBack.Visible = True
                  trUpdate.Visible = False
                  'added by Nor Affarina 05102006
                  fncDisableFormControls()

               Next

            Else

               'Hide the alert message
               lblAlert.Visible = False
               trBack.Visible = False
               trUpdate.Visible = True
               strStatus = ""

            End If

            'Enable back the model dropdown list to allow user to select other model type
            ddlModelType.Enabled = True

            Return strStatus

         Catch Ex As SystemException

            'In case of SystemException, return error to caller function
            Return Ex.Message

         Finally

            'Destroy current instance of MaxPayroll.clsCustChg 
            clsCustChg = Nothing

            'Destroy current instance of System.DataSet
            dsMerchantInfo = Nothing

         End Try

      End Function

#End Region

#Region "fncDisplayPaymentType() - Populate Payment Service Type to dropdown list"

      '********************************************************************************************************************************
      'Function Name      : fncDisplayPaymentType
      'Purpose            : Populate dropdown control to display payment service registered for each organzation
      '                   : screen
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 27/02/2007
      '********************************************************************************************************************************
      Private Sub prcBindPaymentService()

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring new instance of System.Data.DataSet
         Dim dsPaymentType As New DataSet

         Try
            ddlModelType.Items.Clear()
            'dsPaymentType = clsCustChg.fncDisplayPaymentServiceType(rq_strOrgID)
            ddlModelType.DataSource = clsCustChg.fncDisplayPaymentServiceType(rq_strOrgID, rq_ePageMode)
            ddlModelType.DataTextField = "PayService"
            ddlModelType.DataValueField = "PaySerId"
            ddlModelType.DataBind()
            If ddlModelType.Items.Count = 0 Then
               Me.lblErrorMsg.Text = "Payment Service for Organization [E" & rq_strOrgID & "] is not set properly."
               prcDisableProcess()
            End If

         Catch Ex As SystemException

            'In case of SystemExceptioin, return error message to caller function
            Throw Ex

         Finally

            'Destory current instance of MaxPayroll.clsCustChg 
            clsCustChg = Nothing

         End Try

      End Sub

#End Region

#Region "Populate account number dropdown list"

      '********************************************************************************************************************************
      'Function Name      : fncDisplayAccountNumber
      'Purpose            : Display Account number of each organization
      '                   : screen
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      'Last Modified      : 
      'Last Modified By   : 
      'Action             : 
      'Reason             : 
      '********************************************************************************************************************************
      Private Sub prcBindChargeSettleAcc(ByVal strModelType As String)


         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring new instance of System.DataSet
         Dim dsChargeSetAcc As New DataSet

         Try

            'Modified by Muhammad Ali 07/09/2006 T-Melmax to display account info on the basis of model type
            dsChargeSetAcc = clsCustChg.fncDisplayChargeAccountInfo(rq_strOrgID, ddlModelType.SelectedValue)

            ddlChargeSettleAcc.DataSource = dsChargeSetAcc
            ddlChargeSettleAcc.DataTextField = "AcctNo"
            ddlChargeSettleAcc.DataValueField = "AcctID"
            ddlChargeSettleAcc.DataBind()


            If ddlChargeSettleAcc.Items.Count = 0 Then
               Me.lblErrorMsg.Text = "Bank Account for Organization [E" & rq_strOrgID & "] is not set properly."
               prcDisableProcess()
            Else
               hChargeSettAcc.Value = Trim(ddlChargeSettleAcc.SelectedItem.Text)
            End If
         Catch Ex As SystemException

            'In case of SystemExceptioin, return error message to caller function
            Throw Ex

         Finally

            'Destory current instance of MaxPayroll.clsCustChg 
            clsCustChg = Nothing

            'Destroy current instance of System.DataSet
            dsChargeSetAcc = Nothing

         End Try

      End Sub

#End Region
      Private Sub prcDisableProcess()
         Me.btnNew.Enabled = False
         Me.btnSubmit.Enabled = False
      End Sub
#Region "Verify the entry screen before inserting into database"

      '********************************************************************************************************************************
      'Function Name      : fncVerifyInformation
      'Purpose            : Provide functionality to verify all the information entered in the merchant charge definition screen
      'Arguments          : N/A
      'Return Value       : N/A
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      '********************************************************************************************************************************
      Private Function fncVerifyInformation() As Boolean

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg
         Dim blnErrMsg As Boolean
         Dim strStatus As String
         Dim DblTotShare As Double, intCheck As Int16
         Dim IsDuplicateAlternateAcc As Boolean

         'Added by Affarina 30/10/2006 validation 0 value
         If Not txtChargeSettleAcc.Text = "" Then
            'Check if percentage rate is Zero
            If IsNumeric(txtChargeSettleAcc.Text) Then
               If CDbl(txtChargeSettleAcc.Text) = CDbl("00000000000000") Then
                  revSettleBankAccount.IsValid = False
                  revSettleBankAccount.Display = ValidatorDisplay.None
                  revSettleBankAccount.ErrorMessage = "Charge Settlement account number can't be zero"
                  blnErrMsg = True
               End If
            End If
         End If

         'Added by Affarina T-Melmax Sdn Bhd 10102006
         If Not txtChargeSettleAcc.Text = "" Then
            IsDuplicateAlternateAcc = clsCustChg.fncCheckDupAltenateAcc(rq_strOrgID, txtChargeSettleAcc.Text)
         Else
            IsDuplicateAlternateAcc = False
         End If

         'If IsDuplicate > 0 Then
         If IsDuplicateAlternateAcc Then

            If IsDuplicateAlternateAcc = True Then
               revSettleBankAccount.IsValid = False
               revSettleBankAccount.Display = ValidatorDisplay.None
               revSettleBankAccount.ErrorMessage = "Alternate Account No is Existing, please enter new account no"
               blnErrMsg = True
            End If
         End If


         If radFixedRate.Checked = False And radPercentRate.Checked = False And radTierRate.Checked = False Then

            'In case none of the merchant charge type been selected, display alert to the user
            'lblAlert.Text = "At lease one charge type information must be entered"
            revtxtFR.IsValid = False
            revtxtFR.Display = ValidatorDisplay.None
            revtxtFR.ErrorMessage = "At lease one Merchant charge type information must be entered"
            blnErrMsg = True
            'Exit Function  1

         Else

            If radFixedRate.Checked = True Then
               hChargeType.Value = "F"
            ElseIf radPercentRate.Checked = True Then
               hChargeType.Value = "P"
            ElseIf radTierRate.Checked Then
               hChargeType.Value = "T"
            End If
         End If

         'Added By Ali   Date: 02/04/2006 
         'Check if alternative account and RHB accounts is same
         If Trim(txtChargeSettleAcc.Text) = Trim(ddlChargeSettleAcc.SelectedItem.Text) Then
            revSettleBankAccount.IsValid = False
            revSettleBankAccount.Display = ValidatorDisplay.None
            revSettleBankAccount.ErrorMessage = "Charge Settlement Account can not be same as Alternative Account or Its Invalid"
            blnErrMsg = True
            ' Exit Function

         End If





         Return blnErrMsg

      End Function

#End Region

#Region "Verify merchant status and authorization"

      Private Function fncVerifyMerchantStatus(ByVal strSellerId As String) As String

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring new instance of System.DataSet
         Dim dsMerchantInfo As New DataSet

         'Declaring new instance of System.DataRow
         Dim drMerchantInfo As DataRow

         Try

            'Getting the merchant information
            dsMerchantInfo = clsCustChg.fncDisplayMerchantInfo(strSellerId)

            'Process each rows returned by the DataSet
            For Each drMerchantInfo In dsMerchantInfo.Tables("Display").Rows

               If Trim(drMerchantInfo("AuthChk")) = "P" Then

                  'In case merchant is pending for authorization, display alert to the user
                  tblAck.Visible = True
                  tblMerchCharge.Visible = False
                  lblMessage.Text = "Merchant creation request is pending for approval"
                  Exit Try

               ElseIf Trim(drMerchantInfo("AuthChk")) = "R" Then

                  'In case merchant is been rejected, display alert to the user
                  tblAck.Visible = True
                  tblMerchCharge.Visible = False
                  lblMessage.Text = "Merchant creation request is rejected"
                  Exit Try

               ElseIf Trim(drMerchantInfo("AuthChk")) = "A" Then

                  'In case of merchant has been authorized, then validate current merchant status
                  'Modified by Affarina 20/09/2006 authchk was assign to cancel.
                  If Trim(drMerchantInfo("Status")) = "Cancel" Then

                     'In case of merchant has been cancelled, display alert to the user
                     tblAck.Visible = True
                     tblMerchCharge.Visible = False
                     lblMessage.Text = "Merchant " & drMerchantInfo("MerchantName") & " is already cancelled so the charges cannot be defined for this merchant"
                     Exit Try

                  End If

               End If

            Next

         Catch Ex As SystemException

            'In case of SystemException occur, return error message back to caller function
            Return Ex.Message

         Finally

            'Destroy current instance of MaxPayroll.clsCustChg 
            clsCustChg = Nothing

            'Destroy current instance of System.DataSet
            dsMerchantInfo = Nothing

         End Try


      End Function

#End Region

#Region "Insert merchant charge definition into the database"

      '********************************************************************************************************************************
      'Function Name      : fncUpdateChargeInformation
      'Purpose            : Provide functionality to insert all the information entered in the merchant charge definition screen
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Victor Wong
      'Created            : 
      '********************************************************************************************************************************
      Private Function fncUpdateChargeInformation(ByVal intCustChargeID As Integer) As String

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring local variables
         Dim strStatus As String

         Try

            'Insert merchant charge definition into database
            strStatus = clsCustChg.fncUpdMerchantChargesTable(CInt(Me.hidCustChargeID.Value))

            If Not strStatus = "" Then
               Return strStatus
               Exit Try
            Else
               If Me.radTierRate.Checked Then
                  clsCustChg.fncDeleteMerchantTierCharge(CInt(Me.hidCustChargeID.Value))

                  If Not txtMinTier1.Text = "" And Not txtMaxTier1.Text = "" Then
                     'If Tier charge type is selected, insert tier charge information into database
                     strStatus = clsCustChg.fncInsertTierChargeTable(CInt(Me.hidCustChargeID.Value), txtRateTier1.Text, txtMinTier1.Text, txtMaxTier1.Text, 1)

                  End If

                  If (IsNothing(strStatus) OrElse strStatus = "") AndAlso (txtMinTier2.Text <> "" And txtMaxTier2.Text <> "") Then
                     'If Tier charge type is selected, insert tier charge information into database
                     strStatus = clsCustChg.fncInsertTierChargeTable(CInt(Me.hidCustChargeID.Value), txtRateTier2.Text, txtMinTier2.Text, txtMaxTier2.Text, 2)

                  End If

                  If (IsNothing(strStatus) OrElse strStatus = "") AndAlso (txtMinTier3.Text <> "" And txtMaxTier3.Text <> "") Then
                     'If Tier charge type is selected, insert tier charge information into database
                     strStatus = clsCustChg.fncInsertTierChargeTable(CInt(Me.hidCustChargeID.Value), txtRateTier3.Text, txtMinTier3.Text, txtMaxTier3.Text, 3)

                  End If
               Else
                  strStatus = clsCustChg.fncDeleteMerchantTierCharge(CInt(rq_strOrgID))
               End If
               If IsNothing(strStatus) = False AndAlso strStatus <> "" Then
                  LogError(strStatus)
               End If
            End If
         Catch Ex As SystemException


         Finally

         End Try

      End Function

      '********************************************************************************************************************************
      'Function Name      : fncInsertChargeInformation
      'Purpose            : Provide functionality to insert all the information entered in the merchant charge definition screen
      'Arguments          : N/A
      'Return Value       : Status of the operation
      'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created            : 
      '********************************************************************************************************************************
      Private Function fncInsertChargeInformation(ByVal strCharge As String) As String

         'Declaring new instance of MaxPayroll.clsCustChg 
         Dim clsCustChg As New MaxPayroll.clsCustChg

         'Declaring local variables
         Dim strStatus As String

         Try

            'Insert merchant charge definition into database
            strStatus = clsCustChg.fncInstOrgCharge(rq_strOrgID, strCharge)

            If Not strStatus = "" Then
               Exit Try
            Else
               If Me.radTierRate.Checked Then
                  hidCustChargeID.Value = clsCustChg.fncGetInstOrgChargeID(CLng(rq_strOrgID), ddlModelType.SelectedValue)

                  If Not txtMinTier1.Text = "" And Not txtMaxTier1.Text = "" Then
                     'If Tier charge type is selected, insert tier charge information into database
                     strStatus = clsCustChg.fncInsertTierChargeTable(CInt(Me.hidCustChargeID.Value), txtRateTier1.Text, txtMinTier1.Text, txtMaxTier1.Text, 1)

                  End If

                  If (IsNothing(strStatus) OrElse strStatus = "") AndAlso (txtMinTier2.Text <> "" And txtMaxTier2.Text <> "") Then
                     'If Tier charge type is selected, insert tier charge information into database
                     strStatus = clsCustChg.fncInsertTierChargeTable(CInt(Me.hidCustChargeID.Value), txtRateTier2.Text, txtMinTier2.Text, txtMaxTier2.Text, 2)

                  End If

                  If (IsNothing(strStatus) OrElse strStatus = "") AndAlso (txtMinTier3.Text <> "" And txtMaxTier3.Text <> "") Then
                     'If Tier charge type is selected, insert tier charge information into database
                     strStatus = clsCustChg.fncInsertTierChargeTable(CInt(Me.hidCustChargeID.Value), txtRateTier3.Text, txtMinTier3.Text, txtMaxTier3.Text, 3)

                  End If
               
               End If
               If IsNothing(strStatus) = False AndAlso strStatus <> "" Then
                  LogError(strStatus)
               End If
            End If
         Catch Ex As SystemException


         Finally

         End Try

      End Function

#End Region

#Region "Sending information to the approval matrix"

      Private Function fncInsertApprovalMatrix(ByVal strUser As String, ByVal strUserName As String, _
                  ByVal strRoles As String, ByVal intUserCode As Integer) As String


         Dim clsCustChg As New MaxPayroll.clsCustChg

         Dim dsMerchantInfo As New DataSet

         Dim drMerchantInfo As DataRow

         'Declaring local variables
         Dim strSubject As String

         Try

            'Getting the merchant charges information
            'dsMerchantInfo = clsCustChg.fncDisplayMerchantChargesInfo(rq_strOrgId, ddlModelType.SelectedValue) 'strModelType)

            'For Each drMerchantInfo In dsMerchantInfo.Tables("Display").Rows
            '   strUserId = Trim(drMerchantInfo("ChargeID"))
            'Next
            'Me.prcBindPaymentService()
            'Building the subject title
            strSubject = strUserName & "(Merchant) Merchant Charge Definition"


            If rq_ePageMode = enmPageMode.NewMode Then
               tblMerchCharge.Visible = False
               tblAck.Visible = True
               lblMessage.Text = "Merchant charges registered successfully."
            Else
               tblMerchCharge.Visible = False
               tblAck.Visible = True
               lblMessage.Text = "Merchant charges modified successfully."
            End If


         Catch ex As Exception

         End Try

      End Function

#End Region

      Public Sub prcDisplayChargeTypeInfo(ByVal sender As System.Object, ByVal e As System.EventArgs)

         fncDisplayChargeTypeInfo()

      End Sub


#Region "Submit"
      Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
         'Declaring new instance of testMaxPG.clsGeneric
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring local variables
         Dim intUserCode As Int16
         Dim strUserName As String, strStatus As String

         Try

            'Modified by Ali    Date: 12/03/2006 
            'Getting Merchant ID as primarty key from previous page
            'strMerchId = Request.QueryString("ID")

            'strUser = Session("SYS_TYPE")          'User type
            intUserCode = Session("SYS_USERID")    'User id
            strUserName = txtMerchName.Text        'Merchant name
            'strRoles = Request.QueryString("Role") 'Merchant type

            'Modified by: Eric  Date: 10/07/2006
            'Create a separate function to verify the information in the merchant creation screen
            strStatus = fncVerifyInformation()

            'strStatus = False
            'If Not strStatus = "" Then
            If strStatus Then
               lblAlert.Enabled = True
               'exit from 
               Exit Try

            Else
               ''Modified by: Eric  Date: 10/07/2006
               'Modified by Affarina T-Melmax Sdn Bhd 21/09/2006
               'move to insert procedure
               ''Insert information in the merchant creation screen to the database
               If rq_ePageMode = enmPageMode.NewMode Then
                  strStatus = fncInsertChargeInformation(hChargeType.Value)
               ElseIf rq_ePageMode = enmPageMode.EditMode Then
                  If btnSubmit.Text.Equals("Update") Then
                     hidModelType.Value = ddlModelType.SelectedValue
                     hidMerchBill.Value = radMerchBill.SelectedValue
                     hidBillFrequency.Value = ddlBillFrequency.SelectedValue
                     Me.fncDisableFormControls()
                     btnSubmit.Text = "Submit"
                     lblErrorMsg.Text = "Please confirm the changes before submit."
                  Else
                     strStatus = fncUpdateChargeInformation(CInt(hidCustChargeID.Value))
                  End If

               End If



               If Not strStatus = "" Then
                  Exit Try
               Else
                  'Modified by: Eric  Date: 10/07/2006
                  'Sending request for approval
                  fncInsertApprovalMatrix(Me.ss_strUserType, strUserName, rq_strRole, Me.ss_lngUserID)
               End If

            End If


         Catch Ex As SystemException

            'Log Error
            Call clsGeneric.ErrorLog("Page_Submmit - frmMerchChargeDefinition", Err.Number, Err.Description)

         End Try

      End Sub
#End Region

#Region "Create New Merchant Charges"
      Private Sub prNew(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click

         Try
            If rq_ePageMode = enmPageMode.NewMode Then
               Response.Redirect(Page.AppRelativeVirtualPath & "?" & Page.ClientQueryString)
            Else
               Response.Redirect("PG_ViewOrganisation.aspx?Req=ModifyOrgCharge")
            End If
         Catch ex As Exception

         End Try

      End Sub

#End Region


#Region "Back"

      Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
         Try
            If rq_ePageMode = enmPageMode.NewMode Then
               Response.Redirect("PG_ViewOrganisation.aspx?Req=CreateOrgCharge")
            Else
               Response.Redirect("PG_ViewOrganisation.aspx?Req=ModifyOrgCharge")
            End If

         Catch ex As Exception

         End Try

      End Sub

#End Region

      'Private Sub btnTry_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTry.Click
      '    Response.Redirect("frmSearchModifyMerchant.aspx?Log=DefMerchCharge")
      'End Sub

#Region "fncDisplayMerchantInfo"
      Private Sub prcDisplayMerchantInfo()
            txtSellerID.Text = Me.rq_strOrgID
         txtMerchName.Text = clsCustomer.fnGetOrgnizationName(rq_strOrgID)
      End Sub
#End Region

      Protected Sub ddlModelType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlModelType.SelectedIndexChanged
         If rq_ePageMode = enmPageMode.EditMode Then
            Me.prcRetrieveData()
         End If
      End Sub
   End Class

End Namespace

