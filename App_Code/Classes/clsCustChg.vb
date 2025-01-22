Imports Microsoft.VisualBasic
Imports System.Math
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient


Namespace MaxPayroll

    Public Class clsCustChg

        '****************************************************************************************************
        'Global Variables
        '****************************************************************************************************

#Region "Global variable declararion"

    'Declaring global variable
    Public SqlConnect As SqlConnection
    Public strFtpSrcId As String
    Public strFtpIp As String
    Public strFtpUsr As String
    Public strFtpPwd As String
    Public strNewMerchantId As String
    Private ConnectionString As String
    Public strOldAcc1 As String, strOldAcc2 As String, strOldAcc3 As String, strOldAcc4 As String
    Public strStatus As String, strType As String, strChargeType As String


#End Region

        '****************************************************************************************************
        'Customer Charges Management Module
        '****************************************************************************************************

#Region "Getting merchant id from charge table based on charge id"

    '****************************************************************************************************
    'Function Name  : fncGetMerchantId
    'Purpose        : To obtain latest merchant id from the database
    'Arguments      : N/A
    'Return Value   : Status
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created        : 06/01/2006
    '*****************************************************************************************************
    Public Function fncGetChargeMerchantId(ByVal strChargeId As String) As String

            'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.DataSet
        Dim dsDisplay As New DataSet

        'Declaring new instance of System.Data.DataSet
        Dim drDisplay As DataRow

        'Declaring new instance of System.Data.SqlDataAdapter
        Dim sdaDisplay As New SqlDataAdapter

        'Declaring local variable
        Dim strChargeMerchantId As String

        Try

                'Initialize the connection to SQL Server 2005
                clsGeneric.SQLConnection_Initialize()

            'Execute the stored procedure using SqlDataAdpater
            sdaDisplay = New SqlDataAdapter("Exec fpx_QryMerchantChargeTableMerchID ''," & "'" & strChargeId & "'", SqlConnect)

            'Fill the DataSet with the result returned from the SqlDataAdapter
            sdaDisplay.Fill(dsDisplay, "Display")

            If dsDisplay.Tables("Display").Rows.Count >= 1 Then

                'Process each rows return by the DataSet
                For Each drDisplay In dsDisplay.Tables("Display").Rows

                    'Getting the merchant id from the charge table
                    strChargeMerchantId = IIf(IsDBNull(drDisplay("ChargeMerchantID")), "", drDisplay("ChargeMerchantID"))

                Next

                    Return strChargeMerchantId

            End If

        Catch Ex As SystemException

            'In case of SystemException, return error message back to caller function
            Return Ex.Message

        End Try

    End Function

#End Region

#Region "Display merchant charge information"

    '****************************************************************************************************'
    'Function Name      : fncDisplayMerchantChargesInfo
    'Purpose            : To display all merchant charges information returned from the search function
    'Arguments          : Seller ID
    'Return Value       : System.Data.DataSet
        'Author             : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created            : 17/01/2006
    'Last Modified      : 04/07/2006 
    '*****************************************************************************************************
    Public Function fncDisplayMerchantChargesInfo(ByVal strSellerID As String, ByVal strModelType As String) As System.Data.DataSet


            'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.DataSet
        Dim dsDisplay As New DataSet

        'Declaring new instance of System.Data.SqlDataAdapter
        Dim sdaDisplay As New SqlDataAdapter

        'Declaring local variables

        Try

                'Initialize the connection to SQL Server 2005
                clsGeneric.SQLConnection_Initialize()

                'Execute the stored procedure using SqlDataAdpater
            sdaDisplay = New SqlDataAdapter("Exec fpx_QryMerchantChargeTable '" & strSellerID & "','" & strModelType & "'", SqlConnect)

            'Fill the DataSet with the result returned from the SqlDataAdapter
            sdaDisplay.Fill(dsDisplay, "Display")

            'Return the DataSet to the caller function
            Return dsDisplay

        Catch Ex As SystemException

            'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("efpx:clsDatabase - fncDisplayMerchantInfo", Err.Number, Ex.Message)

        Finally

                'Terminate the SQL Server 2005 connection
                clsGeneric.SQLConnection_Terminate()

            'Destory current instance of efpx
            clsGeneric = Nothing

        End Try

    End Function

#End Region

#Region "Display merchant charge information using Merchant Charge ID"

    '****************************************************************************************************'
    'Function Name  : fncDisplayMerchantChargesInfo
    'Purpose        : To display all merchant charges information returned from the search function
    'Arguments      : Seller ID
    'Return Value   : System.Data.DataSet
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created        : 17/01/2006
    '*****************************************************************************************************
    Public Function fncDisplayMerchantChargesInfoMID(ByVal strSellerID As String) As System.Data.DataSet

            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.DataSet
        Dim dsDisplay As New DataSet

        'Declaring new instance of System.Data.SqlDataAdapter
        Dim sdaDisplay As New SqlDataAdapter

        'Declaring local variables

        Try

            'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

            'Execute the stored procedure using SqlDataAdpater
            sdaDisplay = New SqlDataAdapter("Exec fpx_QryMerchantChargeTableMerchID '" & strSellerID & "'", SqlConnect)

            'Fill the DataSet with the result returned from the SqlDataAdapter
            sdaDisplay.Fill(dsDisplay, "Display")

            'Return the DataSet to the caller function
            Return dsDisplay

        Catch Ex As SystemException

            'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("efpx:clsDatabase - fncDisplayMerchantInfo", Err.Number, Ex.Message)

        Finally

            'Terminate the SQL Server 2000 connection
                clsGeneric.SQLConnection_Terminate()

            'Destory current instance of efpx
            clsGeneric = Nothing

        End Try

    End Function

#End Region

#Region "Getting modal type information based on charge id"

    Public Function fncGetChargeModalType(ByVal strChargeId As String) As String

            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.DataSet
        Dim dsDisplay As New DataSet

        'Declaring new instance of System.Data.DataSet
        Dim drDisplay As DataRow

        'Declaring new instance of System.Data.SqlDataAdapter
        Dim sdaDisplay As New SqlDataAdapter

        'Declaring local variables
        Dim strModalType As String

        Try

            'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

            'Execute the stored procedure using SqlDataAdpater
            sdaDisplay = New SqlDataAdapter("Exec fpx_QryMerchantChargeInformation '" & strChargeId & "'", SqlConnect)

            'Fill the DataSet with the result returned from the SqlDataAdapter
            sdaDisplay.Fill(dsDisplay, "Display")

            If Not dsDisplay.Tables("Display").Rows.Count = 0 Then

                For Each drDisplay In dsDisplay.Tables("Display").Rows

                    strModalType = drDisplay("ModelType")

                Next

            End If

            'Return the DataSet to the caller function
            Return strModalType

        Catch Ex As SystemException

            'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("efpx:clsDatabase - fncDisplayMerchantInfo", Err.Number, Ex.Message)

        Finally

            'Terminate the SQL Server 2000 connection
                clsGeneric.SQLConnection_Terminate()

            'Destory current instance of efpx
            clsGeneric = Nothing

        End Try

    End Function

#End Region

#Region "Display account information on the basis of Model Id for Merchant Charges"

    '****************************************************************************************************'
    'Function Name  : fncDisplayChargeAccountInfo
    'Purpose        : To display all merchant account information returned from the search function  
    '               : on the basis model type
    'Arguments      : Seller ID, model type
    'Return Value   : System.Data.DataSet
    'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created        : 07/09/2006
    '*****************************************************************************************************
        Public Function fncDisplayChargeAccountInfo(ByVal strOrgID As String, ByVal strPaymentType As String) As System.Data.DataSet

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.DataSet
            Dim dsDisplay As New DataSet

            'Declaring new instance of System.Data.SqlDataAdapter
            Dim sdaDisplay As New SqlDataAdapter

            'Declaring local variables

            Try

                'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

                'Execute the stored procedure using SqlDataAdpater
                sdaDisplay = New SqlDataAdapter("Exec pg_OrgBankAccount '" & strOrgID & "','" & strPaymentType & "'", clsGeneric.SQLConnection)

                'Fill the DataSet with the result returned from the SqlDataAdapter
                sdaDisplay.Fill(dsDisplay, "Display")

                'Return the DataSet to the caller function
                Return dsDisplay

            Catch Ex As SystemException

                'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("efpx:clsDatabase - fncDisplayMerchantInfo", Err.Number, Ex.Message)

            Finally

                'Terminate the SQL Server 2000 connection
                clsGeneric.SQLConnection_Terminate()

                'Destory current instance of efpx
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Display tier charge information"

    '****************************************************************************************************'
    'Function Name  : fncDisplayTierChargeInfo
    'Purpose        : To display all tier charge information returned from the search function
    'Arguments      : Payment code
    'Return Value   : System.Data.DataSet
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created        : 17/01/2006
    '*****************************************************************************************************
    Public Function fncDisplayTierChargeInfo(ByVal intTierChargeId As Int16) As System.Data.DataSet

        'Declaring new instance of Generic.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.DataSet
        Dim dsDisplay As New DataSet

        'Declaring new instance of System.Data.SqlDataAdapter
        Dim sdaDisplay As New SqlDataAdapter

        Try

            'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

            'Execute the stored procedure using SqlDataAdpater
            sdaDisplay = New SqlDataAdapter("Exec fpx_QryMerchantTierChargesTable '" & intTierChargeId & "'", SqlConnect)

            'Fill the DataSet with the result returned from the SqlDataAdapter
            sdaDisplay.Fill(dsDisplay, "Display")

            'Return the DataSet to the caller function
            Return dsDisplay

        Catch Ex As SystemException

            'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("efpx:clsDatabase - fncDisplayMerchantInfo", Err.Number, Ex.Message)

        Finally

            'Terminate the SQL Server 2000 connection
                clsGeneric.SQLConnection_Terminate()

            'Destory current instance of efpx
            clsGeneric = Nothing

            'Destory current instance of System.Data.DataSet
            dsDisplay = Nothing

            'Destory current instance of System.Data.SqlDataAdapter
            sdaDisplay = Nothing

        End Try

    End Function

#End Region

#Region "fncInstOrgCharge - Insert Organization Charges"

      Public Function fncGetInstOrgChargeID(ByVal lngOrgid As Long, ByVal intPaySer_ID As Integer) As Integer
         Dim clsGeneric As New Generic
         Dim intRetVal As Integer
         intRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, "pg_QryCustChgID " & lngOrgid.ToString & "," & intPaySer_ID.ToString)
         Return intRetVal
      End Function
      '****************************************************************************************************
      'Function Name  : fncInstOrgCharge
      'Purpose        : To insert Organization charges information captured by the charges registration form
      'Arguments      : Seller Id
      'Return Value   : Status
      'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created        : 20/01/2006
      'Last Modified  : 01/04/2006 
      'Last Modifiedby: 
      'Reason         : modified variavle intMerchFixedRate to dblMerchFixedRate as double datatype to pass decimal values  
      '*****************************************************************************************************
      Public Function fncInstOrgCharge(ByVal lngOrgId As Long, ByVal strChargeType As String) As String

         'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring new instance of System.Data.SqlClient.SqlCommand
         Dim cmdInsert As New SqlCommand

         'Declaring new instance of System.Web.HttpContext
         Dim AspNetObjects As HttpContext = HttpContext.Current


         'Declaring local variables
         Dim strBillFreq As String, strBillType As String, strChargeSettleAcc As String
         Dim dblFixedRate As Double, dblPercentRate As Double, dblPercentMinAmt As Double
         Dim dblPercentMaxAmt As Double
         Dim intMaker As Int16
         Dim dblFixedChargeAmt As Double
         Dim strMerchOtherAccount As String
         Dim strChargeSettleAltenateAcc As String, strChargeAccount As String
         Dim intPaymentType As Integer

         Try

            'Initialize the connection to SQL Server 2000
            clsGeneric.SQLConnection_Initialize()

                intPaymentType = AspNetObjects.Request.Form("ctl00$cphContent$ddlModelType")
                strBillFreq = AspNetObjects.Request.Form("ctl00$cphContent$ddlBillFrequency")

                strChargeSettleAcc = AspNetObjects.Request.Form("ctl00$cphContent$hChargeSettAcc")
                strMerchOtherAccount = IIf(IsDBNull(AspNetObjects.Request.Form("ctl00$cphContent$txtMerchOtherAccount")), 0, AspNetObjects.Request.Form("ctl00$cphContent$txtMerchOtherAccount"))

                strChargeSettleAltenateAcc = AspNetObjects.Request.Form("ctl00$cphContent$txtChargeSettleAcc")

                strBillType = AspNetObjects.Request.Form("ctl00$cphContent$radMerchBill")

            intMaker = IIf(IsNumeric(AspNetObjects.Session("SYS_USERID")), AspNetObjects.Session("SYS_USERID"), 0)

            If Not strChargeSettleAltenateAcc = "" Then
               strChargeAccount = Trim(strChargeSettleAltenateAcc)
            Else
               strChargeAccount = Trim(strChargeSettleAcc)
            End If

            'Verifying the content of fixed rate textbox
                If AspNetObjects.Request.Form("ctl00$cphContent$txtFixedRate") = "" Then

                    'In case fixed rate is empty, then assign default value to the parameter
                    dblFixedRate = 0

                Else : dblFixedRate = AspNetObjects.Request.Form("ctl00$cphContent$txtFixedRate")
                End If

            'Verifying the content of percent rate textbox
                If AspNetObjects.Request.Form("ctl00$cphContent$txtPercentRate") = "" Then

                    'In case percent rate is empty, then assign default value to the parameter
                    dblPercentRate = 0

                Else

                    'Assign each percent information to its appropriate parameters
                    dblPercentRate = AspNetObjects.Request.Form("ctl00$cphContent$txtPercentRate")
                    dblPercentMaxAmt = AspNetObjects.Request.Form("ctl00$cphContent$txtMaxAmt")
                    dblPercentMinAmt = AspNetObjects.Request.Form("ctl00$cphContent$txtMinAmt")

                End If


            'Added by Affarina T-Melmax Sdn Bhd 27092006 to get data from FPX  FPX charge

            'Verifying the content of fixed rate textbox
                If AspNetObjects.Request.Form("ctl00$cphContent$txtFPXChargeAmount") = "" Then

                    'In case fixed rate is empty, then assign default value to the parameter
                    dblFixedChargeAmt = 0

                Else : dblFixedChargeAmt = AspNetObjects.Request.Form("ctl00$cphContent$txtFPXChargeAmount")
                End If

            With cmdInsert

               'Setting the SqlCommad properties
               .Connection = clsGeneric.SQLConnection
               .CommandText = "pg_InstCustomerChg"
               .CommandType = CommandType.StoredProcedure

               'Passing all the parameters required by the stored procedure
               .Parameters.Add(New SqlParameter("@OrgID", Round(lngOrgId)))
               .Parameters.Add(New SqlParameter("@PaySerId", intPaymentType))
               .Parameters.Add(New SqlParameter("@CustSettleAcct", strChargeAccount))
               .Parameters.Add(New SqlParameter("@CustBillFreq", strBillFreq))
               .Parameters.Add(New SqlParameter("@CustChgType", strChargeType))
               .Parameters.Add(New SqlParameter("@CustBillType", strBillType))
               .Parameters.Add(New SqlParameter("@CustFixedRate", dblFixedRate))
               .Parameters.Add(New SqlParameter("@CustPercentRate", dblPercentRate))
               .Parameters.Add(New SqlParameter("@CustPercentMaxAmt", dblPercentMaxAmt))
               .Parameters.Add(New SqlParameter("@CustPercentMinAmt", dblPercentMinAmt))
               .Parameters.Add(New SqlParameter("@Maker", intMaker))

               'Execute the store procedure
               .ExecuteNonQuery()

            End With

         Catch Ex As SystemException

            'In case of SystemException occured, return the error message back to caller program
            Return Ex.Message

         Finally

         End Try
         Return Nothing
      End Function

#End Region

#Region "Insert Tier Charge table"

    '****************************************************************************************************'
    'Function Name  : fncInsertTierChargeTable
    'Purpose        : To get all the content from the database
    'Arguments      : Option, Date, Sequence No, Table, File Sequence No
    'Return Value   : DataSet
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created        : 16/12/2005
    '*****************************************************************************************************
      Public Function fncInsertTierChargeTable(ByVal intCustChargeID As Integer, ByVal decCustTierAmt As Decimal, ByVal decCustTierFrom As Decimal, ByVal decCustTierTo As Decimal, ByVal intTierNo As Integer) As String

         'Declaring new instance of System.Data.SqlClient.SqlCommand
         Dim cmdInsert As New SqlCommand
         Dim clsGeneric As New Generic
         Dim intRelVal As Integer = 0
         'Declaring new instance of System.Web.HttpContext
         Dim AspNetObjects As HttpContext = HttpContext.Current

         Dim Params(4) As SqlParameter

         Try
            Params(0) = New SqlParameter("@In_CustChargeID", SqlDbType.Int)
            Params(0).Value = intCustChargeID
            Params(1) = New SqlParameter("@In_CustTierAmt", SqlDbType.Decimal)
            Params(1).Value = decCustTierAmt
            Params(2) = New SqlParameter("@In_CustTierFrom", SqlDbType.Decimal)
            Params(2).Value = decCustTierFrom
            Params(3) = New SqlParameter("@In_CustTierTo", SqlDbType.Decimal)
            Params(3).Value = decCustTierTo
            Params(4) = New SqlParameter("@In_TierNo", SqlDbType.Int)
            Params(4).Value = intTierNo

            SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_InstCustTierChg", Params)

            'With cmdInsert

            '   'Setting the SqlCommand properties
            '   .Connection = SqlConnect
            '   .CommandText = "pg_InstCustTierChg"
            '   .CommandType = CommandType.StoredProcedure

            '   .Parameters.Add(New SqlParameter("@CustChargeID", intCustChargeID))
            '   .Parameters.Add(New SqlParameter("@CustTierAmt", decCustTierAmt))
            '   .Parameters.Add(New SqlParameter("@CustTierFrom", intCustTierFrom))
            '   .Parameters.Add(New SqlParameter("@CustTierTo", intCustTierTo))
            '   .Parameters.Add(New SqlParameter("@TierNo", intTierNo))

            '   'Execute the store procedure
            '   .ExecuteNonQuery()

            'End With

         Catch Ex As SystemException

            'In case of SystemException occur, return the message to the caller program
            Return Ex.Message

         Finally

            'Destory current instance of System.Data.SqlClient.SqlCommand
            cmdInsert = Nothing

            'Destory current instance of System.Web.HttpContext
            AspNetObjects = Nothing

         End Try
         Return ""
      End Function

#End Region

#Region "Update Merchant Charges"

      '****************************************************************************************************'
      'Function Name  : fncUpdateMerchantChargesTable
      'Purpose        : To update the merchant charges table
      'Arguments      : 
      'Return Value   : Status of the operation
      'Author         : Victor Wong
      'Created        : 2007-03-07

      '*****************************************************************************************************
      Public Function fncUpdMerchantChargesTable(ByVal intCustChgID As Integer) As String
         'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring new instance of System.Data.SqlClient.SqlCommand
         Dim cmdInsert As New SqlCommand

         'Declaring new instance of System.Web.HttpContext
         Dim AspNetObjects As HttpContext = HttpContext.Current


         'Declaring local variables
         Dim strBillFreq As String, strBillType As String, strChargeSettleAcc As String
         Dim dblFixedRate As Double, dblPercentRate As Double, dblPercentMinAmt As Double
         Dim dblPercentMaxAmt As Double
         Dim intLastModBy As Integer
         Dim strMerchOtherAccount As String
         Dim strChargeSettleAltenateAcc As String, strChargeAccount As String
         Dim intPaymentType As Integer
         Dim strChargeType As String

         Try

            'Initialize the connection to SQL Server 2000
            clsGeneric.SQLConnection_Initialize()

                intPaymentType = CInt(AspNetObjects.Request.Form("ctl00$cphContent$hidModelType"))
                strBillFreq = AspNetObjects.Request.Form("ctl00$cphContent$hidBillFrequency")

                strChargeSettleAcc = AspNetObjects.Request.Form("ctl00$cphContent$hChargeSettAcc")
                strMerchOtherAccount = IIf(IsDBNull(AspNetObjects.Request.Form("ctl00$cphContent$txtMerchOtherAccount")), 0, AspNetObjects.Request.Form("ctl00$cphContent$txtMerchOtherAccount"))

                strChargeSettleAltenateAcc = AspNetObjects.Request.Form("ctl00$cphContent$txtChargeSettleAcc")

                strBillType = AspNetObjects.Request.Form("ctl00$cphContent$hidMerchBill")

            intLastModBy = IIf(IsNumeric(AspNetObjects.Session("SYS_USERID")), AspNetObjects.Session("SYS_USERID"), 0)

            If Not strChargeSettleAltenateAcc = "" Then
               strChargeAccount = Trim(strChargeSettleAltenateAcc)
            Else
               strChargeAccount = Trim(strChargeSettleAcc)
            End If

            'Verifying the content of fixed rate textbox
                If AspNetObjects.Request.Form("ctl00$cphContent$txtFixedRate") = "" Then

                    'In case fixed rate is empty, then assign default value to the parameter
                    dblFixedRate = 0

                Else : dblFixedRate = AspNetObjects.Request.Form("ctl00$cphContent$txtFixedRate")
                End If
                strChargeType = AspNetObjects.Request.Form("ctl00$cphContent$hChargeType") & ""
            'Verifying the content of percent rate textbox
                If AspNetObjects.Request.Form("ctl00$cphContent$txtPercentRate") = "" Then

                    'In case percent rate is empty, then assign default value to the parameter
                    dblPercentRate = 0

                Else

                    'Assign each percent information to its appropriate parameters
                    dblPercentRate = AspNetObjects.Request.Form("ctl00$cphContent$txtPercentRate")
                    dblPercentMaxAmt = AspNetObjects.Request.Form("ctl00$cphContent$txtMaxAmt")
                    dblPercentMinAmt = AspNetObjects.Request.Form("ctl00$cphContent$txtMinAmt")

                End If


            With cmdInsert

               'Setting the SqlCommad properties
               .Connection = clsGeneric.SQLConnection
               .CommandText = "pg_UpdCustomerChg"
               .CommandType = CommandType.StoredProcedure

               'Passing all the parameters required by the stored procedure
               .Parameters.Add(New SqlParameter("@In_CustChargeID", Round(intCustChgID)))
               .Parameters.Add(New SqlParameter("@In_PaySerId", intPaymentType))
               .Parameters.Add(New SqlParameter("@In_CustSettleAcct", strChargeAccount))
               .Parameters.Add(New SqlParameter("@In_CustBillFreq", strBillFreq))
               .Parameters.Add(New SqlParameter("@In_CustBillType", strBillType))
               .Parameters.Add(New SqlParameter("@In_CustChgType", strChargeType))
               .Parameters.Add(New SqlParameter("@In_CustFixedRate", dblFixedRate))
               .Parameters.Add(New SqlParameter("@In_CustPercentRate", dblPercentRate))
               .Parameters.Add(New SqlParameter("@In_CustPercentMaxAmt", dblPercentMaxAmt))
               .Parameters.Add(New SqlParameter("@In_CustPercentMinAmt", dblPercentMinAmt))
               .Parameters.Add(New SqlParameter("@In_LastModBy", intLastModBy))
              
               'Execute the store procedure
               .ExecuteNonQuery()

            End With

         Catch Ex As SystemException

            'In case of SystemException occured, return the error message back to caller program
            Return Ex.Message

         Finally

         End Try

         Return ""
      End Function
      'Public Function fncUpdateMerchantChargesTable(ByVal strSellerId As String) As String

      '    'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
      '    Dim clsGeneric As New MaxPayroll.Generic

      '    'Declaring new instance of System.Web.HttpContext
      '    Dim AspNetObjects As HttpContext = HttpContext.Current

      '    'Declaring new instance of System.Data.SqlClient.SqlCommand
      '    Dim cmdUpdate As New SqlCommand

      '    'Declaring local variables
      '    Dim intLastModBy As Int16, intMerchBillingFreq As Int16, intLastAuthBy As Int16
      '    Dim dtMakerDate As DateTime, dtCheckerDate As Date, dtLastModDate As Date, dtLastAuthDate As Date
      '    Dim dblMerchPercentRate As Double, intMTC_MerchTierRateID As Int16, intMaker As Int16, intChecker As Int16
      '    Dim intMerchChargeID As Int16, intMerchSettleAccount As Int16, dblMerchFixedRate As Double, dblFpxChargeAmt As Double
      '    Dim strMerchOtherBank As String, dblMerchPercentMaxAmt As Double, dblMerchPercentMinAmt As Double, strEditChk As String
      '    Dim strMerchID As String, strMerch As String, strMerchBillingType As String, strAuthChk As String, strMerchOtherAccount As String
      '    Dim strMerchRegNo As String, strMerchCISNo As String, intBankCodeId As Integer, strMerchName As String
      '    ', intChargeSettleAcc As Integer
      '    Dim strChargeSettleAcc As String, strModelType As String
      '    Dim dblFPXPercentRate, dblFPXPercentMaxAmt, dblFPXPercentMinAmt As Double
      '    Dim strFPXChargeType As String
      '    Try

      '        'Initialize the connection to SQL Server 2000
      '        clsGeneric.SQLConnection_Initialize()

      '        'Getting the value from the merchant registration form
      '        strMerchID = AspNetObjects.Request.Form("txtcSellerID")
      '        strMerchRegNo = AspNetObjects.Request.Form("txtcMerchRegNo")
      '        strMerchCISNo = AspNetObjects.Request.Form("txtcCISNo")
      '        'intBankCodeId = AspNetObjects.Request.Form("txtcBankCode")
      '        strMerchName = AspNetObjects.Request.Form("txtcMerchName")

      '        'Modified by: Eric  Date: 23/03/2006
      '        'Replace textbox values with hidden field values
      '        intMerchBillingFreq = AspNetObjects.Request.Form("hBillFreq")

      '        'Modified by: Ali 31/03/2006
      '        'altered datatype of settlement charge account from integer to string
      '        'intChargeSettleAcc = AspNetObjects.Request.Form("hChargeAcc")
      '        strChargeSettleAcc = AspNetObjects.Request.Form("hChargeAcc")


      '        strModelType = AspNetObjects.Request.Form("hModelType")
      '        strMerchOtherAccount = AspNetObjects.Request.Form("txtcMerchOtherAccount")
      '        strMerchOtherBank = AspNetObjects.Request.Form("txtcMerchOtherBank")
      '        strMerchBillingType = AspNetObjects.Request.Form("hBilling")
      '        strChargeType = AspNetObjects.Request.Form("hChargeType")
      '        '28092006
      '        strFPXChargeType = AspNetObjects.Request.Form("hFPXChargeType")


      '        'strFR = AspNetObjects.Request.Form("htxtFR")
      '        'dblFixedRate = AspNetObjects.Request.Form("htxtFR")

      '        If AspNetObjects.Request.Form("htxtFR") = "" Then
      '            dblMerchFixedRate = 0
      '        Else : dblMerchFixedRate = AspNetObjects.Request.Form("htxtFR")
      '        End If

      '        If AspNetObjects.Request.Form("htxtPR") = "" Then
      '            dblMerchPercentRate = 0
      '        Else : dblMerchPercentRate = AspNetObjects.Request.Form("htxtPR")
      '        End If

      '        If AspNetObjects.Request.Form("htxtMxAmt") = "" Then
      '            dblMerchPercentMaxAmt = 0
      '        Else
      '            dblMerchPercentMaxAmt = AspNetObjects.Request.Form("htxtMxAmt")
      '        End If

      '        If AspNetObjects.Request.Form("htxtMAmt") = "" Then
      '            dblMerchPercentMinAmt = 0
      '        Else
      '            dblMerchPercentMinAmt = AspNetObjects.Request.Form("htxtMAmt")
      '        End If

      '        'can delete
      '        'dblFpxChargeAmt = AspNetObjects.Request.Form("htxtFPXChargeAmount")

      '        'Added Affarina Muhamad Appandi T-Melmax Sdn Bhd 28092006
      '        If AspNetObjects.Request.Form("htxtcFPXChgAmt") = "" Then
      '            dblFpxChargeAmt = 0
      '        Else : dblFpxChargeAmt = AspNetObjects.Request.Form("htxtcFPXChgAmt")
      '        End If

      '        If AspNetObjects.Request.Form("htxtcFPXPercentRate") = "" Then
      '            dblFPXPercentRate = 0
      '        Else : dblFPXPercentRate = AspNetObjects.Request.Form("htxtcFPXPercentRate")
      '        End If

      '        If AspNetObjects.Request.Form("htxtcFPXMaxAmt") = "" Then
      '            dblFPXPercentMaxAmt = 0
      '        Else
      '            dblFPXPercentMaxAmt = AspNetObjects.Request.Form("htxtcFPXMaxAmt")
      '        End If

      '        If AspNetObjects.Request.Form("htxtcFPXMinAmt") = "" Then
      '            dblFPXPercentMinAmt = 0
      '        Else
      '            dblFPXPercentMinAmt = AspNetObjects.Request.Form("htxtcFPXMinAmt")
      '        End If
      '        'end added by affarina 28092006

      '        intLastModBy = AspNetObjects.Session("SYS_USERID")
      '        'dtLastModDate = Today
      '        dtLastModDate = Now

      '        With cmdUpdate

      '            'Setting the SqlCommad properties
      '            .Connection = SqlConnect
      '            .CommandText = "fpx_UpdMerchantChargesTable"
      '            .CommandType = CommandType.StoredProcedure

      '            'Passing all the parameters required by the stored procedure
      '            .Parameters.Add(New SqlParameter("@MerchID", strSellerId))
      '            'Modified by: Ali 31/03/2006 intMerchSettleAccount is declared but value is not assigned
      '            '.Parameters.Add(New SqlParameter("@MerchSettleAccount", intMerchSettleAccount))
      '            .Parameters.Add(New SqlParameter("@MerchSettleAccount", strChargeSettleAcc))

      '            .Parameters.Add(New SqlParameter("@MerchBillingFreq", intMerchBillingFreq))
      '            .Parameters.Add(New SqlParameter("@MerchBillingType", strMerchBillingType))
      '            .Parameters.Add(New SqlParameter("@MerchOtherAccount", strMerchOtherAccount))
      '            .Parameters.Add(New SqlParameter("@MerchOtherBank", strMerchOtherBank))
      '            .Parameters.Add(New SqlParameter("@ChargeType", strChargeType))
      '            .Parameters.Add(New SqlParameter("@MerchFixedRate", dblMerchFixedRate))
      '            .Parameters.Add(New SqlParameter("@MerchPercentRate", dblMerchPercentRate))
      '            .Parameters.Add(New SqlParameter("@MerchPerecentMaxAmt", dblMerchPercentMaxAmt))
      '            .Parameters.Add(New SqlParameter("@MerchPerecentMinAmt", dblMerchPercentMinAmt))
      '            .Parameters.Add(New SqlParameter("@FPXChargeAmt", dblFpxChargeAmt))
      '            .Parameters.Add(New SqlParameter("@LastModBy", intLastModBy))
      '            .Parameters.Add(New SqlParameter("@LastModDate", dtLastModDate))
      '            .Parameters.Add(New SqlParameter("@ModelType", strModelType))
      '            'Added by Affarina T-Melmax Sdn Bhd 28092006
      '            .Parameters.Add(New SqlParameter("@FPXChargeType", strFPXChargeType))
      '            .Parameters.Add(New SqlParameter("@FPXPercentRate", dblFPXPercentRate))
      '            .Parameters.Add(New SqlParameter("@FPXPercentMaxAmt", dblFPXPercentMaxAmt))
      '            .Parameters.Add(New SqlParameter("@FPXPercentMinAmt", dblFPXPercentMinAmt))
      '            'Execute the store procedure
      '            .ExecuteNonQuery()

      '        End With

      '    Catch Ex As SystemException

      '        'In case of SystemException occur, return the error message to caller function.
      '        Return Ex.Message

      '    Finally

      '        'Terminate connection to SQL Server 2000
      '        clsGeneric.SQLConnection_Terminate()

      '        'Destory current instance of System.Data.SqlClient.SqlCommand
      '        cmdUpdate = Nothing

      '    End Try

      'End Function

#End Region

#Region "Update Tier Charge table"

    '****************************************************************************************************'
    'Function Name  : fncUpdateTierChargeTable
    'Purpose        : To update tier charge information changed into database
    'Arguments      : N/A
    'Return Value   : DataSet
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
    'Created        : 16/12/2005
    '*****************************************************************************************************
    Public Function fncUpdateTierChargeTable(ByVal intTierLevel As Int16, ByVal decAmt As Decimal, _
            ByVal decMinAmt As Decimal, ByVal decMaxAmt As Decimal, ByVal strSellerId As String, ByVal strModelType As String) As String

            'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.SqlClient.SqlCommand
        Dim cmdInsert As New SqlCommand

        'Declaring new instance of System.Web.HttpContext
        Dim AspNetObjects As HttpContext = HttpContext.Current

        Try

                clsGeneric.SQLConnection_Initialize()

            With cmdInsert
                'Setting the SqlCommand properties
                .Connection = SqlConnect
                .CommandText = "fpx_UpdMerchantTierChargesTable"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(New SqlParameter("@MerchantID", strSellerId))
                .Parameters.Add(New SqlParameter("@MerchTierAmt", decAmt))
                .Parameters.Add(New SqlParameter("@MerchTierFrom", decMinAmt))
                .Parameters.Add(New SqlParameter("@MerchTierTo", decMaxAmt))
                .Parameters.Add(New SqlParameter("@TierNo", intTierLevel))
                .Parameters.Add(New SqlParameter("@ModelType", strModelType))

                'Execute the store procedure
                .ExecuteNonQuery()

            End With

        Catch Ex As SystemException

            'In case of SystemException occur, return the message to the caller program
            Return Ex.Message

        Finally

            'Destory current instance of System.Data.SqlClient.SqlCommand
            cmdInsert = Nothing

            'Destory current instance of System.Web.HttpContext
            AspNetObjects = Nothing

        End Try

    End Function

#End Region

#Region "Update Merchant Charge Sharing table"

    '****************************************************************************************************
    'Function Name  : fncUpdateMerchChargeSharing
    'Purpose        : To update processing account information into database
    'Arguments      : N/A
    'Return Value   : Status
    'Author         : Deedee - T-Melmax Sdn. Bhd.
    'Created        : 15/02/2006
    '*****************************************************************************************************
    Public Function fncUpdateMerchChargeSharing(ByVal strMerchantID As String, ByVal strSellerID As String, ByVal strModalType As String, _
        ByVal dblBank As Double, ByVal dblSeller As Double, ByVal dblBuyer As Double, ByVal dblFpx As Double, ByVal strModelType As String) As String

        'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
            Dim clsGeneric As New MaxPayroll.Generic

        'Declaring new instance of System.Data.SqlClient.SqlCommand
        Dim cmdInsert As New SqlCommand

        'Declaring new instance of System.Web.HttpContext
        Dim AspNetObjects As HttpContext = HttpContext.Current


        Try

            'initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

            With cmdInsert

                'Setting the SqlCommand properties
                .Connection = SqlConnect
                .CommandText = "fpx_UpdMerchantChargeSharing"
                .CommandType = CommandType.StoredProcedure

                .Parameters.Add(New SqlParameter("@MerchantID", strMerchantID))
                .Parameters.Add(New SqlParameter("@SellerID", strSellerID))
                .Parameters.Add(New SqlParameter("@ModelType", strModalType))
                .Parameters.Add(New SqlParameter("@BankShare", dblBank))
                .Parameters.Add(New SqlParameter("@SellerShare", dblSeller))
                .Parameters.Add(New SqlParameter("@BuyerShare", dblBuyer))
                .Parameters.Add(New SqlParameter("@FpxShare", dblFpx))

                'Execute the store procedure
                .ExecuteNonQuery()

            End With

        Catch Ex As SystemException

            'In case of SystemException occur, return the message to the caller program
            'Return Ex.Message
            'Log Error
                Call clsGeneric.ErrorLog("efpx:frmModifyChargeSharing - prcUpdate", Err.Number, Ex.Message)

            Finally

                'Terminate connection to the SQL Server 2000
                clsGeneric.SQLConnection_Terminate()

                'Destory current instance of System.Data.SqlClient.SqlCommand
                cmdInsert = Nothing

                'Destory current instance of System.Web.HttpContext
                AspNetObjects = Nothing

            End Try

        End Function

#End Region

#Region "Delete Merchant Tier Charge table"

        '***********************************************************************************************************
        'Function Name  : fncDeleteMerchantTierCharge
        'Purpose        : To delete unwanted record from the merchant business modal table
        'Arguments      : Merchant Id, Seller Id, & Account number
        'Return Value   : Status of the operation
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
        'Created        : 22/02/2006
        '***********************************************************************************************************
      Public Function fncDeleteMerchantTierCharge(ByVal intCustChargeID As Integer) As String

         'Declaring new instance of efpx_MasterFileMgmt.clsGeneric
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring new instance of System.Data.SqlClient.SqlCommand
         Dim cmdUpdate As New SqlCommand

         Dim Params(0) As SqlParameter

         Try
            Params(0) = New SqlParameter("@In_CustChargeID", SqlDbType.Int)
            Params(0).Value = intCustChargeID
            SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_DelCustTierChg", Params)
            ''Initialize the connection to SQL Server 2000
            'clsGeneric.SQLConnection_Initialize()

            'With cmdUpdate

            '   ''Setting the SqlCommad properties
            '   '.Connection = SqlConnect
            '   '.CommandText = "pg_DelCustTierChg"
            '   '.CommandType = CommandType.StoredProcedure

            '   ''Passing all the parameters required by the stored procedure
            '   '.Parameters.Add(New SqlParameter("@In_CustChargeID", intCustChargeID))
            '   ''Execute the store procedure
            '   '.ExecuteNonQuery()

            'End With

         Catch Ex As SystemException

            'In case of SystemException occur, return the error message to caller function.
            Return Ex.Message

         Finally

            'Terminate the SQL Server connection
            clsGeneric.SQLConnection_Terminate()

            'Destory current instance of System.Data.SqlClient.SqlCommand
            cmdUpdate = Nothing

         End Try

      End Function

#End Region

#Region "Display Payment type*"

        '****************************************************************************************************'
        'Function Name  : fncDisplayPaymentServiceType
        'Purpose        : to display payment serivce against each Organization 
        'Arguments      : Organization Id
        'Return Value   : System.Data.DataSet
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
      'Created        : 27/02/2007
        '*****************************************************************************************************
      'Public Function fncDisplayPaymentServiceType(ByVal strOrgId As String, Optional ByVal ePageMode As enmPageMode = enmPageMode.NewMode) As DataSet

      '   Dim dsQuery As New DataSet

      '   Dim clsGeneric As New MaxPayroll.Generic

      '   'Declaring new instance of System.Data.SqlClient.SqlDataAdapter
      '   Dim sdaQuery As SqlDataAdapter
      '   Try

      '      'Initializing the connection to the SQL Server
      '      clsGeneric.SQLConnection_Initialize()

      '      'Executing the store procedure using SqlDataAdapter
      '      sdaQuery = New SqlDataAdapter("Exec pg_GetOrgChgCommon '" & "PAY SER" & "','" & strOrgId & "'," & "'NULL'", clsGeneric.SQLConnection)

      '      'Fill current dataset with results from SqlDataAdapter
      '      sdaQuery.Fill(dsQuery, "Query")

      '      'Return DataSet to caller function
      '      Return dsQuery

      '   Catch Ex As SystemException

      '      clsGeneric.ErrorLog("pg_QryBankDefinition", Err.Number, Err.Description)

      '   Finally
      '      clsGeneric.SQLConnection_Terminate()
      '      sdaQuery = Nothing
      '      clsGeneric = Nothing
      '   End Try
      '   Return Nothing

      'End Function
      Public Function fncDisplayPaymentServiceType(ByVal strOrgId As String, Optional ByVal ePageMode As enmPageMode = enmPageMode.NewMode) As SqlDataReader

         Dim dsQuery As New DataSet
         Dim drInfo As SqlDataReader
         Dim strSQL As String = "Exec pg_GetOrgChgCommon "
         Dim clsGeneric As New MaxPayroll.Generic

         'Declaring new instance of System.Data.SqlClient.SqlDataAdapter
         'Dim sdaQuery As SqlDataAdapter
         Try

            'Initializing the connection to the SQL Server
            'clsGeneric.SQLConnection_Initialize()

            ''Executing the store procedure using SqlDataAdapter
            'sdaQuery = New SqlDataAdapter("Exec pg_GetOrgChgCommon '" & "PAY SER" & "','" & strOrgId & "'," & "'NULL'", clsGeneric.SQLConnection)

            ''Fill current dataset with results from SqlDataAdapter
            'sdaQuery.Fill(dsQuery, "Query")

            'Return DataSet to caller function
            'Return dsQuery
            Select Case ePageMode
               Case enmPageMode.NewMode
                  strSQL += "'PAY SER'"
               Case enmPageMode.EditMode
                  strSQL += "'MOD PAY SER'"
               Case Else
                  strSQL += ""
            End Select
            strSQL += ",'" & strOrgId & "'," & "'NULL'"
            drInfo = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            Return drInfo
         Catch Ex As SystemException

            clsGeneric.ErrorLog("pg_QryBankDefinition", Err.Number, Err.Description)

         Finally
            'clsGeneric.SQLConnection_Terminate()
            'sdaQuery = Nothing
            clsGeneric = Nothing
         End Try
         Return Nothing

      End Function
#End Region

#Region "Display merchant information"

        '****************************************************************************************************'
        'Function Name  : fncDisplayMerchantInfo
        'Purpose        : To display all merchant information returned from the search function
        'Arguments      : Seller ID
        'Return Value   : System.Data.DataSet
        'Author         : Muhammad Ali - T-Melmax Sdn. Bhd.
        'Created        : 17/01/2006
        '*****************************************************************************************************
        Public Function fncDisplayMerchantInfo(ByVal strSellerID As String) As System.Data.DataSet

            Dim clsGeneric As New MaxPayroll.Generic

            'Declaring new instance of System.Data.DataSet
            Dim dsDisplay As New DataSet

            'Declaring new instance of System.Data.SqlDataAdapter
            Dim sdaDisplay As New SqlDataAdapter

            'Declaring local variables

            Try

                'Initialize the connection to SQL Server 2000
                clsGeneric.SQLConnection_Initialize()

                'Execute the stored procedure using SqlDataAdpater
                sdaDisplay = New SqlDataAdapter("Exec fpx_QryMerchantTable '" & strSellerID & "','" & strSellerID & "'", SqlConnect)

                'Fill the DataSet with the result returned from the SqlDataAdapter
                sdaDisplay.Fill(dsDisplay, "Display")

                'Return the DataSet to the caller function
                Return dsDisplay

            Catch Ex As SystemException

                'In case of SystemException occur, log error into database
                clsGeneric.ErrorLog("efpx:clsDatabase - fncDisplayMerchantInfo", Err.Number, Ex.Message)

            Finally

                'Terminate the SQL Server 2000 connection
                clsGeneric.SQLConnection_Terminate()

                'Destory current instance of maxPaygate
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Duplicate Alternate Account No -affarina"
        '***********************************************************************************************************
        'Function Name  : fncCheckDupAltenateAcc
        'Purpose        : To Check For Duplicate Account Number 
        'Arguments      : Account number
        'Return Value   : Status of the operation
        'Author         : Nor Affarina Muhamad Appandi - T-Melmax Sdn. Bhd.
        'Created        : 10/10/2006
        '***********************************************************************************************************
        Public Function fncCheckDupAltenateAcc(ByVal strSellerID As String, ByVal strAccNo As String) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdDuplicate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
         Dim IsDuplicate As Boolean, intResult As Int32

            Try
                'Initialize SQL Connection 
                Call clsGeneric.SQLConnection_Initialize()

                With cmdDuplicate
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "fpx_ChkAlternateAcct"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@MerchID", strSellerID))
                    .Parameters.Add(New SqlParameter("@MerchAcctNo", strAccNo))
                    .Parameters.Add(New SqlParameter("@out_Result", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "mIntResult", DataRowVersion.Default, intResult))
                    .ExecuteScalar()
                    intResult = .Parameters("@out_Result").Value
                End With

                If intResult > 0 Then
                    IsDuplicate = True
                Else
                    IsDuplicate = False
                End If

                Return IsDuplicate

            Catch

                'Log Error
                clsGeneric.ErrorLog("fncCheckDuplicateMerchant - clsDatabase", Err.Number, Err.Description)

            Finally

                'Destroy SQL Command Object
                cmdDuplicate = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Retrieve Data"
      Public Function fncQryCustomerChg(ByVal lngOrgID As Long, ByVal intPayID As Integer) As SqlDataReader
         Dim drInfo As SqlDataReader
         Dim clsGeneric As New Generic
         Dim params(1) As SqlParameter
         Try
            params(0) = New SqlParameter("@Org_Id", SqlDbType.Int)
            params(0).Value = lngOrgID
            params(1) = New SqlParameter("@PaySer_Id", SqlDbType.Int)
            params(1).Value = intPayID
            drInfo = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.StoredProcedure, "pg_QryCustomerChg", params)
            Return drInfo
         Catch ex As Exception
            clsGeneric.ErrorLog("clsCustChg - fncQryCustomerChg", Err.Number, Err.Description)
         Finally
            clsGeneric = Nothing
            'drInfo = Nothing
         End Try
         Return Nothing
      End Function
#End Region
   End Class

End Namespace