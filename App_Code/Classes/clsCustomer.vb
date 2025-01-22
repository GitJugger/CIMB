
'****************************************************************************************************
'Class Name     : clsCustomer
'ProgId         : MaxPayroll.clsCustomer
'Purpose        : Customer Functions Used For The Complete Project
'Author         : Sujith Sharatchandran - 
'Created        : 17/10/2003
'*****************************************************************************************************

Option Strict Off
Option Explicit On

Imports System.Data
Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.Encryption
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
Imports MaxPayroll



Namespace MaxPayroll


    Public Class clsCustomer
        Dim _generic As New clsBaseGeneric


#Region "View/Search Customer"

        '****************************************************************************************************
        'Function Name  : fnCustomer
        'Purpose        : View/Search Customer
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2003
        '*****************************************************************************************************
        Public Function fnCustomer() As System.Data.DataSet

            Dim sdaCustomer As New SqlDataAdapter                   'Create SQL Data Adaptor
            Dim clsGeneric As New MaxPayroll.Generic                   'Create Generic Class Object
            Dim dsCustomer As New System.Data.DataSet               'Create Data Set
            Dim ASPNetContext As HttpContext = HttpContext.Current  'Create ASPNet Context Object

            'Variable Declarations
            Dim lngOrgId As Long, lngUserCode As Long, strSQLStatement As String
            Dim strOption As String, strCriteria As String, strKeyword As String

            Try

                'Assign Values
                strOption = ASPNetContext.Request.Form("ctl00$cphContent$cmbOption")
                strCriteria = ASPNetContext.Request.Form("ctl00$cphContent$cmbCriteria")
                strKeyword = ASPNetContext.Request.Form("ctl00$cphContent$txtKeyword")
                lngOrgId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)
                lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                Call clsGeneric.SQLConnection_Initialize()

                'SQL String
                strSQLStatement = "Exec pg_SearchCustomer '" & strOption & "','" & strCriteria & "','" & strKeyword & "'"

                'Execute SQL Data Adaptor
                sdaCustomer = New SqlDataAdapter(strSQLStatement, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaCustomer.Fill(dsCustomer, "CUST")

                Return dsCustomer

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQl Data Adaptor
                sdaCustomer = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy Data Set
                dsCustomer = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Customer File Format"

        '****************************************************************************************************
        'Function Name  : fnDB_CustomerFormat
        'Purpose        : Insert/Update Customer File Format Details
        'Arguments      : Data grid, Grid Name
        'Return Value   : OK/Error
        'Author         : Sujith Sharatchandran - 
        'Created        : 20/10/2003
        '*****************************************************************************************************
        Public Function fncCustomerFormat(ByRef dgCustomerGrid As DataGrid, ByVal lngFileId As Long, _
                            ByVal strGridName As String, ByVal lngCustId As Long, ByVal intApproved As Int16, _
                                Service_Id As Short, EncryptionType As Short) As Long

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdCustomerFormat As New SqlCommand

            'Create Instance of SQL Transaction
            Dim trnCustomerFormat As SqlTransaction

            'Create Instance of ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim gridItem As DataGridItem, strFileType As String, lngOrgId As Long
            Dim strHeader As String, strFooter As String, intHeaderLines As Int16, intFooterLines As Int16
            Dim lngUserCode As Long, strFileFormat As String, strFileDelimiter As String, strFileExtension As String
            Dim intStartPos As Int16, intEndPos As Int16, intColPos As Int16, intFieldId As Int16, strFormatName As String, _
            intBankId As Integer, intFileReplicate As String, FileReplicate As Boolean, No_Reviewer As Integer = 0
            'Assign Values
            If lngFileId > 0 Then
                strHeader = ASPNetContext.Request.Form("ctl00$cphContent$hHeader")
                strFooter = ASPNetContext.Request.Form("ctl00$cphContent$hFooter")
                strFileType = ASPNetContext.Request.Form("ctl00$cphContent$hType")
                strFileExtension = ASPNetContext.Request.Form("ctl00$cphContent$hExtn")
                strFileFormat = UCase(ASPNetContext.Request.Form("ctl00$cphContent$hFormat"))
                strFileDelimiter = IIf(ASPNetContext.Request.Form("ctl00$cphContent$hDelim") <> "", ASPNetContext.Request.Form("ctl00$cphContent$hDelim"), "")

                intBankId = ASPNetContext.Request.Form("ctl00$cphContent$hidBank")




            Else
                strFileType = ASPNetContext.Request.Form("ctl00$cphContent$hFileType")
                strFileExtension = ASPNetContext.Request.Form("ctl00$cphContent$cmbExtn")
                strFileFormat = UCase(ASPNetContext.Request.Form("ctl00$cphContent$cmbFormat"))
                strHeader = IIf(ASPNetContext.Request.Form("ctl00$cphContent$chkHeader") = "on", "Y", "N")
                strFooter = IIf(ASPNetContext.Request.Form("ctl00$cphContent$chkFooter") = "on", "Y", "N")
                strFileDelimiter = IIf(ASPNetContext.Request.Form("ctl00$cphContent$cmbDelim") <> "", ASPNetContext.Request.Form("ctl00$cphContent$cmbDelim"), "")
                intBankId = ASPNetContext.Request.Form("ctl00$cphContent$ddlBank")
                intFileReplicate = ASPNetContext.Request.Form("ctl00$cphContent$cbduplicate")
                If intFileReplicate = "on" Then
                    FileReplicate = True
                End If
            End If

            strFormatName = ASPNetContext.Request.Form("ctl00$cphContent$txtFormatName")
            lngUserCode = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)
            intHeaderLines = IIf(IsNumeric(ASPNetContext.Request.Form("ctl00$cphContent$txtHLines")), ASPNetContext.Request.Form("ctl00$cphContent$txtHLines"), 0)
            intFooterLines = IIf(IsNumeric(ASPNetContext.Request.Form("ctl00$cphContent$txtFLines")), ASPNetContext.Request.Form("ctl00$cphContent$txtFLines"), 0)
            'Intialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()

            'Begin Transaction
            trnCustomerFormat = clsGeneric.SQLConnection.BeginTransaction()

            Try


                With cmdCustomerFormat
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trnCustomerFormat
                    .CommandText = "pg_MainCustFormat"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Clear()
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngCustId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileFormat", strFileFormat))
                    .Parameters.Add(New SqlParameter("@in_FormatName", strFormatName))
                    .Parameters.Add(New SqlParameter("@in_Delimiter", strFileDelimiter))
                    .Parameters.Add(New SqlParameter("@in_FileExtension", strFileExtension))
                    .Parameters.Add(New SqlParameter("@in_Header", strHeader))
                    .Parameters.Add(New SqlParameter("@in_Footer", strFooter))
                    .Parameters.Add(New SqlParameter("@in_HeaderLines", intHeaderLines))
                    .Parameters.Add(New SqlParameter("@in_FooterLines", intFooterLines))
                    .Parameters.Add(New SqlParameter("@in_BankId", intBankId))
                    .Parameters.Add(New SqlParameter("@in_FileReplication", FileReplicate))
                    .Parameters.Add(New SqlParameter("@in_Service_Id", Service_Id)) 'Added by Naresh -08-04-11
                    .Parameters.Add(New SqlParameter("@in_Encryption", EncryptionType)) 'Added by Naresh -11-05-11
                    '.Parameters.Add(New SqlParameter("@in_ReviewerNo", FileReplicate))
                    lngFileId = cmdCustomerFormat.ExecuteScalar()
                End With

                'Loop Thro grid - Start
                For Each gridItem In dgCustomerGrid.Items

                    If strGridName = "POS" Then
                        'If strFileType = "LHDN File" Then
                        Dim txtFieldId As TextBox = CType(gridItem.Cells(1).Controls(1), TextBox)
                        '    Dim txtStartPosLHDN As TextBox = CType(gridItem.Cells(2).Controls(3), TextBox)
                        '    Dim txtEndPosLHDN As TextBox = CType(gridItem.Cells(3).Controls(3), TextBox)
                        '    intColPos = 0
                        '    intEndPos = IIf(IsNumeric(txtEndPosLHDN.Text), txtEndPosLHDN.Text, 0)
                        '    intFieldId = IIf(IsNumeric(txtFieldId.Text), txtFieldId.Text, 0)
                        '    intStartPos = IIf(IsNumeric(txtStartPosLHDN.Text), txtStartPosLHDN.Text, 0)
                        'Else
                        '    Dim txtFieldId As TextBox = CType(gridItem.Cells(1).Controls(1), TextBox)
                        Dim txtStartPos As TextBox = CType(gridItem.Cells(2).Controls(1), TextBox)
                        Dim txtEndPos As TextBox = CType(gridItem.Cells(3).Controls(1), TextBox)
                        intColPos = 0
                        intEndPos = IIf(IsNumeric(txtEndPos.Text), txtEndPos.Text, 0)
                        intFieldId = IIf(IsNumeric(txtFieldId.Text), txtFieldId.Text, 0)
                        intStartPos = IIf(IsNumeric(txtStartPos.Text), txtStartPos.Text, 0)
                        'End If
                    ElseIf strGridName = "COL" Or strGridName = "DELIM" Then
                        Dim txtFieldId As TextBox = CType(gridItem.Cells(1).Controls(1), TextBox)
                        Dim txtColPos As TextBox = CType(gridItem.Cells(2).Controls(1), TextBox)
                        intEndPos = 0
                        intStartPos = 0
                        intColPos = IIf(IsNumeric(txtColPos.Text), txtColPos.Text, 0)
                        intFieldId = IIf(IsNumeric(txtFieldId.Text), txtFieldId.Text, 0)
                    End If

                    With cmdCustomerFormat
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnCustomerFormat
                        .CommandText = "pg_CustomerFormat"
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Clear()
                        .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                        .Parameters.Add(New SqlParameter("@in_FieldId", intFieldId))
                        .Parameters.Add(New SqlParameter("@in_StartPos", intStartPos))
                        .Parameters.Add(New SqlParameter("@in_EndPos", intEndPos))
                        .Parameters.Add(New SqlParameter("@in_ColPos", intColPos))
                        .ExecuteNonQuery()
                    End With

                Next
                'Loop Thro grid - Stop

                'Commit Transaction
                trnCustomerFormat.Commit()

                Return lngFileId

            Catch

                'Rollback Transaction
                trnCustomerFormat.Rollback()

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserCode, "fnDb_CustomerFormat - clsCustomer", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdCustomerFormat = Nothing

                'Destroy Instance of SQL Transaction
                trnCustomerFormat = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Position"

        '****************************************************************************************************
        'Function Name  : fnGetPosition
        'Purpose        : get the Start Position,End Position & Column Position
        'Arguments      : Position Requested,Field Id, File Type,Customer Id
        'Return Value   : Position
        'Author         : Sujith Sharatchandran - 
        'Created        : 20/10/2003
        '*****************************************************************************************************
        Public Function fnGetPosition(ByVal strPosition As String, ByVal strFormat As String, ByVal intFieldId As Int16, _
            ByVal strFileType As String, ByVal lngCustId As Long, ByVal lngFileId As Long, Optional ByVal Duplicate As Boolean = False) As Int16

            'Create Instance of SQL Command Object
            Dim cmdCustomer As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            Dim ASPNetContext As HttpContext = HttpContext.Current      'Create ASP Net Context Object

            'Variable Declaration
            Dim intPosition As Int16, lngOrgId As Long, lngUserId As Long

            Try

                lngOrgId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)
                lngUserId = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdCustomer
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_GetPosition"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Position", strPosition))
                    .Parameters.Add(New SqlParameter("@in_Format", strFormat))
                    .Parameters.Add(New SqlParameter("@in_FieldId", intFieldId))
                    .Parameters.Add(New SqlParameter("@in_FileType", strFileType))
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngCustId))
                    .Parameters.Add(New SqlParameter("@out_Position", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Position", DataRowVersion.Default, ""))

                    .Parameters.Add(New SqlParameter("@in_duplicate", Duplicate))

                    .ExecuteNonQuery()
                End With

                'Get Position
                intPosition = IIf(IsNumeric(cmdCustomer.Parameters("@out_Position").Value), cmdCustomer.Parameters("@out_Position").Value, 0)

                Return intPosition

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnGetPosition - clsCustomer", Err.Number, Err.Description)

                Return 0

            Finally

                'Destroy ASPNetContext Object
                ASPNetContext = Nothing

                'Destroy SQL Command Object
                cmdCustomer = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Customer Format"

        '****************************************************************************************************
        'Function Name  : fnGetCustFormat
        'Purpose        : To Get the Customer Format Details
        'Arguments      : Organisation Id
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 11/03/2004
        '*****************************************************************************************************
        Public Function prGetCustFormat(ByRef strFormat As String, ByRef strFormatName As String, ByRef strDelimiter As String, ByRef strExtension As String, _
            ByRef strFileHeader As String, ByRef intHeaderLines As Int16, ByRef strFileFooter As String, ByRef intFooterLines As Int16, _
                ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Reader
            Dim sdrCustomerFormat As SqlDataReader

            'Variable Declarations
            Dim strSQL As String

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec pg_GetCustomerFormat " & lngOrgId & ",'" & strFormat & "','" & strFileType & "'"

                'Execute SQL Data Reader
                Dim cmdCustomer As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                sdrCustomerFormat = cmdCustomer.ExecuteReader(CommandBehavior.CloseConnection)

                'If Record Found - Start
                If sdrCustomerFormat.HasRows Then
                    sdrCustomerFormat.Read()
                    strFormat = sdrCustomerFormat("FFormat")
                    strDelimiter = sdrCustomerFormat("FDelimiter")
                    strExtension = sdrCustomerFormat("FExtn")
                    strFormatName = IIf(Not IsDBNull(sdrCustomerFormat("FFormat")), sdrCustomerFormat("FFormat"), "")
                    strFileHeader = IIf(Not IsDBNull(sdrCustomerFormat("FHeader")), sdrCustomerFormat("FHeader"), "N")
                    intHeaderLines = IIf(IsNumeric(sdrCustomerFormat("FHLines")), sdrCustomerFormat("FHLines"), 0)
                    strFileFooter = IIf(Not IsDBNull(sdrCustomerFormat("FFooter")), sdrCustomerFormat("FFooter"), "N")
                    intFooterLines = IIf(IsNumeric(sdrCustomerFormat("FFLines")), sdrCustomerFormat("FFLines"), 0)
                    sdrCustomerFormat.Close()
                Else
                    sdrCustomerFormat.Close()
                End If
                'If Record Found - Stop

                cmdCustomer = Nothing

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prGetCustFormat - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                clsGeneric = Nothing        'Destroy Instance of Generic class object
                sdrCustomerFormat = Nothing 'Destroy Instance 

            End Try
            Return Nothing
        End Function

#End Region

#Region "Populate State"

        '****************************************************************************************************
        'Function Name  : GetState
        'Purpose        : To populate State 
        'Arguments      : Organisation ID, User Id
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 07/07/2004
        '*****************************************************************************************************
        Public Function GetState(ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance SQL Data Adapter
            Dim sdaState As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsState As New System.Data.DataSet

            'Variable Declarations

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                sdaState = New SqlDataAdapter("Exec pg_GetState ", clsGeneric.SQLConnection)
                sdaState.Fill(dsState, "ACCESS")

                Return dsState

            Catch

                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCustomer - GetState", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL DataAdapter
                sdaState = Nothing

                'Destroy Instance of generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsState = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Populate State LHDN"

        '****************************************************************************************************
        'Function Name  : GetStateLHDN
        'Purpose        : To populate State LHDN
        'Arguments      : File type
        'Return Value   : Data Set
        'Author         : Eu Yean Lock - 
        'Created        : 31/10/2006
        '*****************************************************************************************************
        Public Function GetStateLHDN(ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance SQL Data Adapter
            Dim sdaState As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsState As New System.Data.DataSet

            'Variable Declarations

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                sdaState = New SqlDataAdapter("Exec pg_GetStateLHDN ", clsGeneric.SQLConnection)
                sdaState.Fill(dsState, "ACCESS")

                Return dsState

            Catch

                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCustomer - GetStateLHDN", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL DataAdapter
                sdaState = Nothing

                'Destroy Instance of generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsState = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Populate State ZAKAT"


        Public Function GetStateZAKAT(ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance SQL Data Adapter
            Dim sdaState As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsState As New System.Data.DataSet

            'Variable Declarations

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                sdaState = New SqlDataAdapter("Exec pg_GetStateZAKAT", clsGeneric.SQLConnection)
                sdaState.Fill(dsState, "ACCESS")

                Return dsState

            Catch

                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCustomer - GetStateZAKAT", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL DataAdapter
                sdaState = Nothing

                'Destroy Instance of generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsState = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Check Duplicate Values For Organisation"

        '****************************************************************************************************
        'Procedure Name : fnOrgValidations()
        'Purpose        : Check Duplicate For Organisation Registration
        'Arguments      : Action,Request,Check Value,Organisation ID
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/10/2003
        '*****************************************************************************************************
        Public Function fnOrgValidations(ByVal strAction As String, ByVal strRequest As String, _
            ByVal strCheckValue As String, ByVal lngCustId As Long, Optional ByVal lngModuleId As Long = 0, Optional ByVal intPaySer_Id As Integer = 0) As Boolean

            'Create Instance Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance SQL Command Object
            Dim cmdOrgValidations As New SqlCommand

            'Create Instance ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim IsDuplicate As Boolean, intResult As Int16, lngUserId As Long, lngOrgId As Long

            Try

                'Assign values
                IsDuplicate = False
                lngOrgId = IIf(IsNumeric(ASPNetContext.Session("SYS_ORGID")), ASPNetContext.Session("SYS_ORGID"), 0)
                lngUserId = IIf(IsNumeric(ASPNetContext.Session("SYS_USERID")), ASPNetContext.Session("SYS_USERID"), 0)

                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Command Object To Get Desired Result - Start
                With cmdOrgValidations
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_OrgDuplicate"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Option", strAction))
                    .Parameters.Add(New SqlParameter("@in_Request", strRequest))
                    .Parameters.Add(New SqlParameter("@in_CheckValue", strCheckValue))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngCustId))
                    .Parameters.Add(New SqlParameter("@in_ModuleId", lngModuleId))
                    '.Parameters.Add(New SqlParameter("@in_PaySer_Id", intPaySer_Id))
                    .Parameters.Add(New SqlParameter("@out_Result", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Result", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With
                'Execute SQL Command Object To Get Desired Result - Stop

                'Get Result
                intResult = IIf(Not IsDBNull(cmdOrgValidations.Parameters("@out_Result").Value), cmdOrgValidations.Parameters("@out_Result").Value, 0)

                'Check If result is Duplicate
                IsDuplicate = IIf(intResult > 0, True, False)

                Return IsDuplicate

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnOrgValidations - clsCustomer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy ASP Net Context Object
                ASPNetContext = Nothing

                'Destroy SQL Command Object
                cmdOrgValidations = Nothing

            End Try

        End Function

#End Region

#Region "Insert/Update Organisation"

        '****************************************************************************************************
        'Procedure Name : fnOrganisation()
        'Purpose        : Insert/Update Organisation Details
        'Arguments      : Action - Insert/Update
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/10/2003
        '*****************************************************************************************************
        Public Function fncOrganisation(ByVal strAction As String, ByVal lngOrgId As Long, ByVal lngCreatedBy As Long, _
                    ByVal dgPayService As DataGrid, Optional ByVal bIsChkUseTokenModified As Boolean = False) As Long

            'Create Instance of Datagrid Item
            Dim dgiPayService As DataGridItem

            'Create Instance SQL Transaction
            Dim trnOrganisation As SqlTransaction

            'Create Instance of SQL Command Object
            Dim cmdOrganisation As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of ASPNetContext Object
            Dim ASPNetObjects As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strTaxRegNo As String
            Dim dcStop As Decimal
            Dim strPriviliged As String
            Dim dcCharge As Decimal
            Dim dcAnnualFee As Decimal
            Dim strBusinessRegNo As String
            Dim intVerify As Int16
            Dim intPaySerId As Int16
            Dim strCustAdmin As String
            Dim strCustAdminIC As String
            Dim intEmployees As Int32
            Dim strOrgPhone1 As String
            Dim strOrgPhone2 As String
            Dim strOrgFax As String
            Dim strOrgEmail As String
            Dim strOrgName As String
            Dim strOrgAddress As String
            Dim intOrgState As Int16
            Dim intOrgPostCode As Int32
            Dim strWebSite As String
            Dim bytLogo As Byte()
            Dim strContactPerson As String
            Dim strContactPersonIC As String
            Dim intGroups As Int16
            Dim strCountry As String
            Dim strLogoPath As String
            Dim strOrgStatus As String
            Dim strBrCode As String
            Dim strRegion As String
            Dim strState As String
            Dim bOrg_Token As Boolean
            Dim sEncryptionKey As String = ""
            Dim sSubscriptionFeePaymentMode As String = ""
            Dim bH2H As Boolean = False

            'Intialise SQL Connection
            Call clsGeneric.SQLConnection_Initialize()

            'Begin Transaction 
            trnOrganisation = clsGeneric.SQLConnection.BeginTransaction()

            Try

                'Get Organisation Details - Start
                strOrgName = ASPNetObjects.Request.Form("ctl00$cphContent$txtCOrgName")
                intOrgState = ASPNetObjects.Request.Form("ctl00$cphContent$hcmbState")
                strOrgAddress = ASPNetObjects.Request.Form("ctl00$cphContent$txtCAddress")
                intOrgPostCode = ASPNetObjects.Request.Form("ctl00$cphContent$txtCPincode")
                strCountry = ASPNetObjects.Request.Form("ctl00$cphContent$txtCCountry")
                strOrgPhone1 = ASPNetObjects.Request.Form("ctl00$cphContent$txtCPhone1")
                strOrgPhone2 = ASPNetObjects.Request.Form("ctl00$cphContent$txtCPhone2")
                strOrgFax = ASPNetObjects.Request.Form("ctl00$cphContent$txtCFax")
                strOrgEmail = ASPNetObjects.Request.Form("ctl00$cphContent$txtCEmail")
                strWebSite = ASPNetObjects.Request.Form("ctl00$cphContent$txtCURL")
                strLogoPath = ASPNetObjects.Server.MapPath("../Include/Images/no-photo.jpg")
                'intEmployees = ASPNetObjects.Request.Form("ctl00$cphContent$txtCEmployees") 'No of Employees


                'Stop Payment Charges
                strBusinessRegNo = ASPNetObjects.Request.Form("ctl00$cphContent$txtCBusReg")
                strTaxRegNo = ASPNetObjects.Request.Form("ctl00$cphContent$txtCTaxReg")
                strOrgStatus = ASPNetObjects.Request.Form("ctl00$cphContent$hchkStatus")
                strPriviliged = ASPNetObjects.Request.Form("ctl00$cphContent$hchkPrivilege")
                strBrCode = ASPNetObjects.Request.Form("ctl00$cphContent$txtCBrCode") & ""
                intVerify = ASPNetObjects.Request.Form("ctl00$cphContent$hVerify")
                strRegion = ASPNetObjects.Request.Form("ctl00$cphContent$txtCRegion") & ""

                strState = ASPNetObjects.Request.Form("ctl00$cphContent$txtCState") & ""
                bOrg_Token = Boolean.Parse(ASPNetObjects.Request.Form("ctl00$cphContent$hchkUseToken"))
                bytLogo = clsGeneric.fnGetPhoto(strLogoPath) 'Organisation Logo

                If IsNothing(ASPNetObjects.Request.Form("ctl00$cphContent$hH2H")) = False Then
                    bH2H = CBool(ASPNetObjects.Request.Form("ctl00$cphContent$hH2H"))
                End If

                If bH2H = False Then
                    strContactPerson = ASPNetObjects.Request.Form("ctl00$cphContent$txtCContactPerson") 'Corporate Administrator Name
                    strContactPersonIC = ASPNetObjects.Request.Form("ctl00$cphContent$txtCContactPerIC") 'Corporate Administrator IC
                    strCustAdmin = ASPNetObjects.Request.Form("ctl00$cphContent$txtCCustomerAdmin") 'Corporate Authorizer Name
                    strCustAdminIC = ASPNetObjects.Request.Form("ctl00$cphContent$txtCCustomAdminIC") 'Corporate Authorizer IC
                    intGroups = ASPNetObjects.Request.Form("ctl00$cphContent$txtCGroups") 'No of Groups
                    sEncryptionKey = ASPNetObjects.Request.Form("ctl00$cphContent$hEncryptionKey") & ""
                Else
                    strContactPerson = ""
                    strContactPersonIC = ""
                    strCustAdmin = ""
                    strCustAdminIC = ""
                    intGroups = 0
                    sEncryptionKey = ""
                End If


                dcAnnualFee = ASPNetObjects.Request.Form("ctl00$cphContent$txtCAnnualFee")
                If IsNothing(ASPNetObjects.Request.Form("ctl00$cphContent$txtCStopCharges")) Then
                    dcStop = 0
                Else
                    dcStop = ASPNetObjects.Request.Form("ctl00$cphContent$txtCStopCharges")
                End If
                'hafeez start 24/10/2008'

                If ASPNetObjects.Request.Form("ctl00$cphContent$txtCEmployees") = "" Or ASPNetObjects.Request.Form("ctl00$cphContent$txtCEmployees") < "0" Then
                    intEmployees = 0
                Else
                    intEmployees = ASPNetObjects.Request.Form("ctl00$cphContent$txtCEmployees")
                End If

                'hafeez end'
                'intEmployees = ASPNetObjects.Request.Form("ctl00$cphContent$txtCEmployees") 'No of Employees

                If sEncryptionKey.Length > 0 Then
                    Dim clsEncryption As New Encryption
                    sEncryptionKey = clsEncryption.Cryptography(sEncryptionKey)
                End If
                'Get Organisation Details - Stop

                sSubscriptionFeePaymentMode = ASPNetObjects.Request.Form("ctl00$cphContent$hSubscriptionFeePaymentMode") & ""

                With cmdOrganisation
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trnOrganisation
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_Organisation"
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_BrCode", strBrCode))
                    .Parameters.Add(New SqlParameter("@in_OrgName", strOrgName))
                    .Parameters.Add(New SqlParameter("@in_Address", strOrgAddress))
                    .Parameters.Add(New SqlParameter("@in_State", intOrgState))
                    .Parameters.Add(New SqlParameter("@in_OthState", strState))
                    .Parameters.Add(New SqlParameter("@in_PostCode", intOrgPostCode))
                    .Parameters.Add(New SqlParameter("@in_Country", strCountry))
                    .Parameters.Add(New SqlParameter("@in_ContactPerson", strContactPerson))
                    .Parameters.Add(New SqlParameter("@in_ContactPerIC", strContactPersonIC))
                    .Parameters.Add(New SqlParameter("@in_CustomerAdmin", strCustAdmin))
                    .Parameters.Add(New SqlParameter("@in_CustAdminIC", strCustAdminIC))
                    .Parameters.Add(New SqlParameter("@in_Phone1", strOrgPhone1))
                    .Parameters.Add(New SqlParameter("@in_Phone2", strOrgPhone2))
                    .Parameters.Add(New SqlParameter("@in_Fax", strOrgFax))
                    .Parameters.Add(New SqlParameter("@in_Email", strOrgEmail))
                    .Parameters.Add(New SqlParameter("@in_URL", strWebSite))
                    .Parameters.Add(New SqlParameter("@in_Logo", bytLogo))
                    .Parameters.Add(New SqlParameter("@in_Status", strOrgStatus))
                    .Parameters.Add(New SqlParameter("@in_BusiRegNo", strBusinessRegNo))
                    .Parameters.Add(New SqlParameter("@in_TaxRegNo", strTaxRegNo))
                    .Parameters.Add(New SqlParameter("@in_AnnualFee", dcAnnualFee))
                    .Parameters.Add(New SqlParameter("@in_StopChrg", dcStop))
                    .Parameters.Add(New SqlParameter("@in_Privileged", strPriviliged))
                    .Parameters.Add(New SqlParameter("@in_Verify", intVerify))
                    .Parameters.Add(New SqlParameter("@in_CreatedBy", lngCreatedBy))
                    .Parameters.Add(New SqlParameter("@in_Region", strRegion))
                    .Parameters.Add(New SqlParameter("@in_Employees", intEmployees))
                    .Parameters.Add(New SqlParameter("@in_Groups", intGroups))
                    .Parameters.Add(New SqlParameter("@in_OrgToken", bOrg_Token))
                    .Parameters.Add(New SqlParameter("@in_Org_EncryptionKey", sEncryptionKey))
                    .Parameters.Add(New SqlParameter("@in_Org_FeePaymentType", sSubscriptionFeePaymentMode))
                    .Parameters.Add(New SqlParameter("@in_Org_H2H", bH2H))

                    .Parameters.Add(New SqlParameter("@out_OrgId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_OrgId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                'Get Organisation Id
                lngOrgId = IIf(IsNumeric(cmdOrganisation.Parameters("@out_OrgId").Value), cmdOrganisation.Parameters("@out_OrgId").Value, 0)

                'Loop Thro Payment Service Datagrid - Start
                For Each dgiPayService In dgPayService.Items
                    'Get Payment Service Id
                    intPaySerId = CInt(dgiPayService.Cells(0).Text)
                    'Get Payment Service Charges
                    dcCharge = dgiPayService.Cells(2).Text
                    With cmdOrganisation
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnOrganisation
                        .CommandType = CommandType.StoredProcedure
                        .CommandText = "pg_PaymentService"
                        .Parameters.Clear()
                        .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                        .Parameters.Add(New SqlParameter("@in_PaySerId", intPaySerId))
                        .Parameters.Add(New SqlParameter("@in_TranCharge", dcCharge))
                        .ExecuteNonQuery()
                    End With
                Next
                'Loop Thro Payment Service Datagrid - Stop

                If bIsChkUseTokenModified Then
                    With cmdOrganisation
                        .Connection = clsGeneric.SQLConnection
                        .Transaction = trnOrganisation
                        .CommandType = CommandType.StoredProcedure

                        If bOrg_Token Then
                            .CommandText = "pg_ResetChangeAuthCodeFlag"
                        Else
                            .CommandText = "pg_UpdateChangeAuthCodeFlag"
                        End If

                        .Parameters.Clear()
                        .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                        .ExecuteNonQuery()
                    End With
                End If

                'Commit Transaction
                trnOrganisation.Commit()

                Return lngOrgId

            Catch ex As Exception

                'Rollback Transaction
                trnOrganisation.Rollback()

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnOrganisation - clsCustomer", Err.Number, Err.Description)

                Return 0
5:
            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Datagrid Item
                dgiPayService = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdOrganisation = Nothing

                'Destroy Instance of SQL Transaction
                trnOrganisation = Nothing

                'Destroy Instance of ASPNet Objects
                ASPNetObjects = Nothing

            End Try

        End Function

#End Region

#Region "Generate User Logins"

        '****************************************************************************************************
        'Procedure Name : fnGenerateUser()
        'Purpose        : Generate User Logins
        'Arguments      : Length, Organisation Name, User Name, Password 
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/10/2003
        '*****************************************************************************************************
        Public Function fnGenerateUser(ByVal intLength As Int16, ByVal strOrgName As String, _
            ByVal strUserName As String, ByVal strUserType As String, _
                ByVal lngOrgId As Long, ByVal lngUserId As Long) As Boolean

            'Create Instance of SQL Command Object
            Dim cmdUser As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim strPassword As String = ""
            Dim strUserId As String = ""
            Dim intApproved As Int16
            Dim intLimit As Int16
            Dim strAction As String
            Dim strStatus As String
            Dim strCreateDt As String
            Dim strAuthCode As String = ""
            Dim strExpiryDate As String
            Dim strChangeUnit As String
            Dim intChangePeriod As Int16
            Dim intResult As Int16

            Try

                'Generate Password
                Call clsEncryption.fncGenerate(strUserName, intLength, strUserId, strPassword, strAuthCode)
                'Remove Empty Space
                strUserId = Replace(strUserId, " ", "")
                'Encrypt Authorization Code
                strAuthCode = clsEncryption.Cryptography(strAuthCode)
                'Encrypt Password
                strPassword = clsEncryption.Cryptography(strPassword)

                'Get Customer Administrator Details - Start
                intApproved = 2
                strAction = "ADD"
                strExpiryDate = Format(DateAdd(DateInterval.Year, 1, Now), "dd/MM/yyyy")
                strCreateDt = Format(Now, "dd/MM/yyyy")
                strChangeUnit = "Y"
                intChangePeriod = 1
                strStatus = "A"
                'Get Customer Administrator Details - Stop

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdUser
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_CheckDuplicate"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_UserType", strUserType))
                    .Parameters.Add(New SqlParameter("@in_UserId", strUserId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@out_Result", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_Result", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                'Get result If Duplicate User Login Available
                intResult = cmdUser.Parameters("@out_Result").Value

                If intResult > 0 Then
                    'replace the fourth character of user id with 
                    'next following character of the alphabitical order
                    If Not UCase(Mid(strUserId, 1, 4)) = "Z" Then
                        strUserId = Replace(strUserId, Mid(strUserId, 1, 4), Chr(Asc(Mid(strUserId, 1, 4)) + 1))
                    Else
                        strUserId = Replace(strUserId, Mid(strUserId, 1, 4), Chr(Asc(Mid(strUserId, 1, 4)) - 1))
                    End If
                End If



                With cmdUser
                    .Parameters.Clear()
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_UserDetails"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_UserLogin", strUserId))
                    .Parameters.Add(New SqlParameter("@in_UserName", strUserName))
                    .Parameters.Add(New SqlParameter("@in_Password", strPassword))
                    .Parameters.Add(New SqlParameter("@in_ExpiryDt", strExpiryDate))
                    .Parameters.Add(New SqlParameter("@in_ChangeUnit", strChangeUnit))
                    .Parameters.Add(New SqlParameter("@in_ChangePeriod", intChangePeriod))
                    .Parameters.Add(New SqlParameter("@in_AuthCode", strAuthCode))
                    .Parameters.Add(New SqlParameter("@in_UserType", strUserType))
                    .Parameters.Add(New SqlParameter("@in_CreatedBy", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_UserStatus", strStatus))
                    .Parameters.Add(New SqlParameter("@in_Approved", intApproved))
                    .Parameters.Add(New SqlParameter("@in_AuthLimit", intLimit))
                    .Parameters.Add(New SqlParameter("@in_Display", intApproved))
                    .Parameters.Add(New SqlParameter("@out_UserId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_UserId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnGenerateUser - clsCustomer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdUser = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Encryption Class Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region " View/Search Org "

        '****************************************************************************************************
        'Procedure Name : fnOrgGrid()
        'Purpose        : Organisation Grid 
        'Arguments      : Organisation Id 
        'Return Value   : DataSet
        'Author         : Sujith Sharatchandran - 
        'Created        : 22/10/2003
        '*****************************************************************************************************
        Public Function fnOrgGrid(ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create SQL Data Adaptor
            Dim sdaOrg As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsOrg As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strOption As String, strCriteria As String, strKeyword As String, strSQLStatment As String

            Try

                'Assign Values to Variables
                strOption = ASPNetContext.Request.Form("ctl00$cphContent$cmbOption")
                strKeyword = Trim(ASPNetContext.Request.Form("ctl00$cphContent$txtKeyword"))
                strCriteria = ASPNetContext.Request.Form("ctl00$cphContent$cmbCriteria")

                'If search for E char - Start
                'If strOption = "ID" Then
                '    Select Case strCriteria
                '        Case "EXACT MATCH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '                If Len(strKeyword) = 7 Then
                '           If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '              strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '              If Not IsNumeric(strKeyword) Then
                '                 strKeyword = 0
                '              End If
                '           Else
                '              strKeyword = 0
                '           End If
                '                Else
                '                    strKeyword = 0
                '                End If

                '            End If
                '        Case "CONTAINS"
                '            If Not IsNumeric(strKeyword) = True Then
                '        If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '           strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '        End If
                '            End If
                '        Case "STARTS WITH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '        strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '            End If
                '    End Select
                'End If
                'If search for E char - Stop

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Assign Search Parameters to Procedure
                strSQLStatment = "Exec up_SearchOrganisation '" & strOption & "','" & strCriteria & "','" & strKeyword & "'"

                'Execute SQL Data Adaptor
                sdaOrg = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaOrg.Fill(dsOrg, "ORGANISATION")

                'Return Data Set
                Return dsOrg

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnOrgGrid - clsCustomer", Err.Number, Err.Description)

            Finally

                'Destroy Instance of SQL DataAdapter
                sdaOrg = Nothing

                'Destroy Instance of Data Set
                dsOrg = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of ASP Net Object
                ASPNetContext = Nothing

            End Try
            Return Nothing
        End Function
        Public Function fnH2HOrgGrid(ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create SQL Data Adaptor
            Dim sdaOrg As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsOrg As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create ASP Net Context Object
            Dim ASPNetContext As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strOption As String, strCriteria As String, strKeyword As String, strSQLStatment As String

            Try

                'Assign Values to Variables
                strOption = ASPNetContext.Request.Form("ctl00$cphContent$cmbOption")
                strKeyword = Trim(ASPNetContext.Request.Form("ctl00$cphContent$txtKeyword"))
                strCriteria = ASPNetContext.Request.Form("ctl00$cphContent$cmbCriteria")

                'If search for E char - Start
                'If strOption = "ID" Then
                '    Select Case strCriteria
                '        Case "EXACT MATCH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '                If Len(strKeyword) = 7 Then
                '                    If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '                        strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '                        If Not IsNumeric(strKeyword) Then
                '                            strKeyword = 0
                '                        End If
                '                    Else
                '                        strKeyword = 0
                '                    End If
                '                Else
                '                    strKeyword = 0
                '                End If

                '            End If
                '        Case "CONTAINS"
                '            If Not IsNumeric(strKeyword) = True Then
                '                If Left(strKeyword, 1) = gc_Const_CCPrefix Then
                '                    strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '                End If
                '            End If
                '        Case "STARTS WITH"
                '            If IsNumeric(strKeyword) = True Then
                '                strKeyword = 0
                '            Else
                '                strKeyword = Replace(strKeyword, gc_Const_CCPrefix, "")
                '            End If
                '    End Select
                'End If
                'If search for E char - Stop

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Assign Search Parameters to Procedure
                strSQLStatment = "Exec pg_SearchH2HOrganisation '" & strOption & "','" & strCriteria & "','" & strKeyword & "'"

                'Execute SQL Data Adaptor
                sdaOrg = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaOrg.Fill(dsOrg, "ORGANISATION")

                'Return Data Set
                Return dsOrg

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fnOrgGrid - clsCustomer", Err.Number, Err.Description)

            Finally

                'Destroy Instance of SQL DataAdapter
                sdaOrg = Nothing

                'Destroy Instance of Data Set
                dsOrg = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of ASP Net Object
                ASPNetContext = Nothing

            End Try
            Return Nothing
        End Function
#End Region

#Region "Organisation Details"

        '****************************************************************************************************
        'Procedure Name : fnGetOrganisation()
        'Purpose        : To Get Organisation Details For the Given Organisation Id
        'Arguments      : Organisation ID
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/08/2003
        '*****************************************************************************************************
        Public Function fnGetOrganisation(ByVal lngOrgId As Long) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adapter
            Dim sdaOrganisation As New SqlDataAdapter

            'Create Instance of System Data Set
            Dim dsOrganisation As New System.Data.DataSet

            Try

                'Intialize Connection String
                Call clsGeneric.SQLConnection_Initialize()

                'Fetch Records and Assign to DataAdaptor
                sdaOrganisation = New SqlDataAdapter("pg_GetOrganisation " & lngOrgId, clsGeneric.SQLConnection)

                'Fill Dataset
                sdaOrganisation.Fill(dsOrganisation, "ORGANISATION")

                Return dsOrganisation

            Catch

                'Log Error
                clsGeneric.ErrorLog(0, 0, "OrganisationDetails-Admin", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaOrganisation = Nothing

                'Desstroy Instance of System Data Set
                dsOrganisation = Nothing

            End Try
            Return Nothing
        End Function

        Public Shared Function fnGetOrgnizationName(ByVal intOrgID As Integer) As String
            Dim strRetval As String = ""
            Dim clsGeneric As New Generic
            Dim strSQL As String
            strSQL = "pg_GetOrgName " & intOrgID
            Try
                strRetval = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                Throw ex
            End Try
            Return strRetval
        End Function

#End Region

#Region "Group Common"
        '****************************************************************************************************
        'Procedure Name : fncGrpCommon()
        'Purpose        : To Get Group Common Details
        'Arguments      : Option,Organisation Id,User Id,File Type
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 10/02/2005
        '*****************************************************************************************************
        Public Function fncGrpCommon(ByVal strOption As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal iPaySerID As Integer, ByVal sStatutory As String) As DataSet

            'Create Instance of Data Set
            Dim dsGroupCommon As New DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String

            Dim params(3) As SqlParameter


            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Build SQL String
                strSQL = "Exec pg_GetGrpCommon " & clsDB.SQLStr(strOption) & "," & lngOrgId & ","
                params(0) = New SqlParameter("@in_Option", SqlDbType.VarChar)
                params(0).Value = strOption
                params(1) = New SqlParameter("@in_OrgId", SqlDbType.Int)
                params(1).Value = lngOrgId
                params(2) = New SqlParameter("@in_PaySerID", SqlDbType.Int)
                params(2).Value = iPaySerID
                params(3) = New SqlParameter("@in_Statutory", SqlDbType.VarChar)
                params(3).Value = sStatutory
                dsGroupCommon = SqlHelper.ExecuteDataset(clsGeneric.SQLConnection, CommandType.StoredProcedure, "pg_GetGrpcommon", params)

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBankAccts - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()



                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return dsGroupCommon
        End Function

        '****************************************************************************************************
        'Procedure Name : fncGrpCommon()
        'Purpose        : To Get Group Common Details
        'Arguments      : Option,Organisation Id,User Id,File Type
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 10/02/2005
        '*****************************************************************************************************
        Public Function fncGrpCommon(ByVal strOption As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String) As DataSet

            'Create Instance of Data Set
            Dim dsGroupCommon As New DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Adapter
            Dim sdaGroupCommon As New SqlDataAdapter

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Build SQL String
                strSQL = "Exec pg_GetGrpCommon '" & strOption & "'," & lngOrgId & ",'" & strFileType & "'"

                'Execute SQL Data Adapter
                sdaGroupCommon = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill DataSet
                sdaGroupCommon.Fill(dsGroupCommon, "GROUP")

                Return dsGroupCommon

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBankAccts - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of DataSet
                dsGroupCommon = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaGroupCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

        Public Function fncGetGroupPaymentService(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal iGroupID As Integer) As DataSet
            Dim dsRetVal As New DataSet
            Dim strSQL As String = "pg_GetGrpPaymentService"
            Dim Param As SqlParameter
            Dim clsGeneric As New Generic
            Try
                Param = New SqlParameter("@in_GroupID", iGroupID)
                dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, Param)
            Catch ex As Exception
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGetGroupPaymentService - clsCustomer", Err.Number, Err.Description)
            Finally
                clsGeneric = Nothing
            End Try
            Return dsRetVal
        End Function

        Public Sub fncBindDDLGroupPaymentService(ByRef DDL As DropDownList, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal iGroupID As Integer)
            Dim dsRetVal As New DataSet
            Dim drInfo As SqlDataReader
            Dim strSQL As String = "pg_GetGrpPaymentService"
            Dim Param As SqlParameter
            Dim clsGeneric As New Generic
            Try
                Param = New SqlParameter("@in_GroupID", iGroupID)
                drInfo = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, Param)
                While drInfo.Read
                    DDL.Items.Add(New ListItem(drInfo("FTYPE"), drInfo("FTYPE")))
                End While
                drInfo.Close()
                drInfo = Nothing
            Catch ex As Exception
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBindDDLGroupPaymentService - clsCustomer", Err.Number, Err.Description)
            Finally
                clsGeneric = Nothing
            End Try
        End Sub

#End Region

#Region "Group Insert/Update"

        '****************************************************************************************************
        'Function Name  : fncGrpInsUpd()
        'Purpose        : To Insert/Update Group Details
        'Arguments      : Action, Organisation Id,User Id
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 12/02/2005
        '*****************************************************************************************************
        Public Function fncGrpInsUpd(ByVal strAction As String, ByVal lngGroupId As Long, ByVal lngOrgId As Long, _
            ByVal lngUserId As Long, ByVal intApproved As Int16) As Long

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdGroupDetails As New SqlCommand

            'Create Instance of ASPNetContext Object
            Dim ASPNetObjects As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strGroupName As String, strGroupDesc As String, intAuthorizers As Int16, strGroupStatus As String

            Try

                strGroupStatus = ASPNetObjects.Request.Form("ctl00$cphContent$hStatus")                  'Status
                intAuthorizers = ASPNetObjects.Request.Form("ctl00$cphContent$hAuthNo")                 'No of Authorizer
                strGroupName = ASPNetObjects.Request.Form("ctl00$cphContent$hGroupName")              'Group Name
                strGroupDesc = ASPNetObjects.Request.Form("ctl00$cphContent$hGroupDesc")              'Group Description

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdGroupDetails
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_GroupDetails"
                    .Parameters.Add(New SqlParameter("@in_Action", strAction))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_GroupName", strGroupName))
                    .Parameters.Add(New SqlParameter("@in_GroupDesc", strGroupDesc))
                    .Parameters.Add(New SqlParameter("@in_AuthNo", intAuthorizers))
                    .Parameters.Add(New SqlParameter("@in_Status", strGroupStatus))
                    .Parameters.Add(New SqlParameter("@in_Approved", intApproved))
                    .Parameters.Add(New SqlParameter("@in_CreatedBy", lngUserId))
                    .Parameters.Add(New SqlParameter("@out_GroupId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_GroupId", DataRowVersion.Default, ""))
                    .ExecuteNonQuery()
                End With

                lngGroupId = cmdGroupDetails.Parameters("@out_GroupId").Value

                Return lngGroupId

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGrpInsUpd - clsCustomer", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

        Public Function fncGrpInsUpd(ByVal Trans As SqlTransaction, ByVal strAction As String, ByVal lngGroupId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal intApproved As Int16) As Long

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdGroupDetails As New SqlCommand

            'Create Instance of ASPNetContext Object
            Dim ASPNetObjects As HttpContext = HttpContext.Current

            'Variable Declarations
            Dim strGroupName As String, strGroupDesc As String, intAuthorizers As Int16, intReviewers As Int16, strGroupStatus As String, _
            iGroupPaymentService As Integer
            Dim params(11) As SqlParameter

            Try

                strGroupStatus = ASPNetObjects.Request.Form("ctl00$cphContent$hStatus")
                intAuthorizers = ASPNetObjects.Request.Form("ctl00$cphContent$hAuthNo")

                intReviewers = ASPNetObjects.Request.Form("ctl00$cphContent$hReviewerNo")

                strGroupName = ASPNetObjects.Request.Form("ctl00$cphContent$hGroupName")
                strGroupDesc = ASPNetObjects.Request.Form("ctl00$cphContent$hGroupDesc")

                If IsNumeric(ASPNetObjects.Request.Form("ctl00$cphContent$hPaymentService")) Then
                    iGroupPaymentService = CInt(ASPNetObjects.Request.Form("ctl00$cphContent$hPaymentService"))
                End If


                params(0) = New SqlParameter("@in_Action", strAction)
                params(1) = New SqlParameter("@in_GroupId", lngGroupId)
                params(2) = New SqlParameter("@in_OrgId", lngOrgId)
                params(3) = New SqlParameter("@in_Group_PaymentService", iGroupPaymentService)
                params(4) = New SqlParameter("@in_GroupName", strGroupName)
                params(5) = New SqlParameter("@in_GroupDesc", strGroupDesc)
                params(6) = New SqlParameter("@in_AuthNo", intAuthorizers)
                params(7) = New SqlParameter("@in_Status", strGroupStatus)
                params(8) = New SqlParameter("@in_Approved", intApproved)
                params(9) = New SqlParameter("@in_CreatedBy", lngUserId)
                params(10) = New SqlParameter("@in_NoReviewer", intReviewers)
                params(11) = New SqlParameter("@out_GroupId", SqlDbType.Int, 0, ParameterDirection.Output, False, 0, 0, "out_GroupId", DataRowVersion.Default, lngGroupId)
                SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "pg_GroupDetails", params)

                lngGroupId = params(11).Value

                Return lngGroupId

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGrpInsUpd - clsCustomer", Err.Number, Err.Description)

                Return 0

            Finally
                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Group Transaction Details"

        '****************************************************************************************************
        'Procedure Name  : fncGrpTrans()
        'Purpose        : To Insert/Update Group Details
        'Arguments      : Action, Organisation Id,User Id
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 12/02/2005
        '*****************************************************************************************************
        Public Sub prcGrpTrans(ByVal strOption As String, ByVal lngGroupId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngTransId As Long)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdGroupTrans As New SqlCommand

            Try
                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdGroupTrans
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_GroupTrans"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .Parameters.Add(New SqlParameter("@in_TransId", lngTransId))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcGrpTrans - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

        Public Function prcSaveGroupAccount(ByVal iGroup As Integer, ByVal arrAccount As ArrayList) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = ""
            Dim iArrItem As Integer
            Dim clsGeneric As New Generic
            Try
                If arrAccount.Count > 0 Then
                    For Each iArrItem In arrAccount
                        strSQL += "INSERT INTO tCor_GroupAccounts (Group_Id,Account_Id) VALUES(" & clsDB.SQLStr(iGroup, clsDB.SQLDataTypes.Dt_Integer) & "," & clsDB.SQLStr(iArrItem, clsDB.SQLDataTypes.Dt_Integer) & ");"
                    Next
                    SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
                    bRetVal = True
                End If
            Catch ex As Exception
                'log error
                Dim cls As New clsBaseGeneric
                cls.LogError("fncGrpDelTrans - clsCustomer")
            End Try

            Return bRetVal
        End Function

        'Create By Victor 2007-12-02
        'Overloading function of prcSaveGroupAccount that accepts Trans as SqlTransaction
        Public Function prcSaveGroupAccount(ByVal Trans As SqlTransaction, ByVal iGroup As Integer, ByVal arrAccount As ArrayList) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = ""
            Dim iArrItem As Integer
            Try
                If arrAccount.Count > 0 Then
                    For Each iArrItem In arrAccount
                        strSQL += "INSERT INTO tCor_GroupAccounts (Group_Id,Account_Id) VALUES(" & clsDB.SQLStr(iGroup, clsDB.SQLDataTypes.Dt_Integer) & "," & clsDB.SQLStr(iArrItem, clsDB.SQLDataTypes.Dt_Integer) & ");"
                    Next
                    SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, strSQL)
                    bRetVal = True
                End If
            Catch ex As Exception
                'log error
                Dim cls As New clsBaseGeneric
                cls.LogError("fncGrpDelTrans - clsCustomer")
            End Try

            Return bRetVal
        End Function


        Public Function prcSaveFileFormat(ByVal iGroup As Integer, ByVal arrFileFormat As ArrayList) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = ""
            Dim iArrItem As Integer
            Dim clsGeneric As New Generic
            Try
                If arrFileFormat.Count > 0 Then
                    For Each iArrItem In arrFileFormat
                        strSQL += "INSERT INTO tCor_GroupFile (Group_Id,File_Id) VALUES(" & clsDB.SQLStr(iGroup, clsDB.SQLDataTypes.Dt_Integer) & "," & clsDB.SQLStr(iArrItem, clsDB.SQLDataTypes.Dt_Integer) & ");"
                    Next
                    SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
                    bRetVal = True
                End If
            Catch ex As Exception
                'log error
                Dim cls As New clsBaseGeneric
                cls.LogError("fncGrpDelTrans - clsCustomer")
            End Try

            Return bRetVal
        End Function
        'Create By Victor 2007-12-02
        'Overloading function of prcSaveFileFormat that accepts Trans as SqlTransaction
        Public Function prcSaveFileFormat(ByVal Trans As SqlTransaction, ByVal iGroup As Integer, ByVal arrFileFormat As ArrayList) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = ""
            Dim iArrItem As Integer
            Try
                If arrFileFormat.Count > 0 Then
                    For Each iArrItem In arrFileFormat
                        strSQL += "INSERT INTO tCor_GroupFile (Group_Id,File_Id) VALUES(" & clsDB.SQLStr(iGroup, clsDB.SQLDataTypes.Dt_Integer) & "," & clsDB.SQLStr(iArrItem, clsDB.SQLDataTypes.Dt_Integer) & ");"
                    Next
                    SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, strSQL)
                    bRetVal = True
                End If
            Catch ex As Exception
                'log error
                Dim cls As New clsBaseGeneric
                cls.LogError("fncGrpDelTrans - clsCustomer")
            End Try

            Return bRetVal
        End Function

#End Region

        Public Function prcSaveStatutoryAccount(ByVal Trans As SqlTransaction, ByVal iGroup As Integer, ByVal arrFileFormat As ArrayList) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = ""
            Dim iArrItem As Integer
            Try
                If arrFileFormat.Count > 0 Then
                    For Each iArrItem In arrFileFormat
                        strSQL += "INSERT INTO tCor_ServiceAccount (Group_Id,SerAcc_Id) VALUES(" & clsDB.SQLStr(iGroup, clsDB.SQLDataTypes.Dt_Integer) & "," & clsDB.SQLStr(iArrItem, clsDB.SQLDataTypes.Dt_Integer) & ");"
                    Next
                    SqlHelper.ExecuteNonQuery(Trans, CommandType.Text, strSQL)
                    bRetVal = True
                End If
            Catch ex As Exception
                'log error
                Dim cls As New clsBaseGeneric
                cls.LogError("fncGrpDelTrans - clsCustomer")
            End Try

            Return bRetVal
        End Function


#Region "Delete Group Transaction Details"

        '****************************************************************************************************
        'Procedure Name : fncGrpDelTrans()
        'Purpose        : To Insert/Update Group Details
        'Arguments      : Action, Organisation Id,User Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 12/02/2005
        '*****************************************************************************************************
        Public Sub prcGrpDelTrans(ByVal strOption As String, ByVal lngGroupId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdGroupTrans As New SqlCommand

            Try
                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdGroupTrans
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_GroupDelTrans"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_GroupId", lngGroupId))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGrpDelTrans - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

        ' Created By Victor 2007-12-02
        ' Overloading function of prcGrpDelTrans with accepts Trans as SqlTransaction.
        Public Sub prcGrpDelTrans(ByVal Trans As SqlTransaction, ByVal strOption As String, ByVal lngGroupId As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Dim Params(1) As SqlParameter

            Try
                'Intialise SQL Connection
                Params(0) = New SqlParameter("@in_Option", strOption)
                Params(1) = New SqlParameter("@in_GroupId", lngGroupId)
                SqlHelper.ExecuteNonQuery(Trans, CommandType.StoredProcedure, "pg_GroupDelTrans", Params)
            Catch
                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGrpDelTrans - clsCustomer", Err.Number, Err.Description)
            Finally
                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing
            End Try

        End Sub

#End Region

#Region "List Group"

        '****************************************************************************************************
        'Function Name  : fncListGroup()
        'Purpose        : To Populate Group to Data Set
        'Arguments      : Organisation Id,User Id
        'Return Value   : DataSet
        'Author         : Sujith Sharatchandran - 
        'Created        : 13/02/2005
        '*****************************************************************************************************
        Public Function fncListGroup(ByVal strOption As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngGroupId As Long) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adapter
            Dim sdaListGroup As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsListGroup As New DataSet

            'Variable Declarations
            Dim strSQL As String

            Try
                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec pg_ListGroup '" & strOption & "'," & lngOrgId & "," & lngGroupId

                'Execute SQL Data Adapter
                sdaListGroup = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaListGroup.Fill(dsListGroup, "LIST")

                Return dsListGroup

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncListGroup - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Data Adapter
                sdaListGroup = Nothing

                'Destroy Instance of Data Set
                dsListGroup = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "BankAccs"
        '****************************************************************************************************
        'Function Name  : fncUpdateBankAccs()
        'Purpose        : To Insert/UpDate Bank Accts
        'Arguments      : Organisation Id,Datagrid
        'Return Value   : DataSet
        'Author         : Uma Mahesh - 
        'Created        : 13/02/2005
        'Modified       : Sujith Sharatchandran - .
        'Date           : 23/02/2005
        '*****************************************************************************************************
        Public Function fncUpdateBankAccs(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal dgBankAccts As DataGrid) As Boolean

            'Create Instance of Data Set
            Dim dsBankAccts As New DataSet

            'Create Instance of SQL Command Object
            Dim cmdBankAccts As New SqlCommand

            'Create Instance of Datagrid Item
            Dim dgiBankAccts As DataGridItem

            'Create Instance of SQL Data Adapter
            Dim sdaBankAccts As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intAcctId As Int16

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                For Each dgiBankAccts In dgBankAccts.Items

                    If IsNumeric(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex)) Then
                        intAcctId = Convert.ToInt32(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex))
                    Else
                        intAcctId = 0
                    End If

                    Dim strAcctName As String = CType(dgiBankAccts.FindControl("txtAccName"), TextBox).Text
                    Dim strAcctNumber As String = CType(dgiBankAccts.FindControl("txtAccNo"), TextBox).Text
                    Dim intBankID As Integer = CInt(DirectCast(dgiBankAccts.FindControl("ddlBankName"), DropDownList).SelectedValue)
                    'Dim intPaymentID As Integer = CInt(DirectCast(dgiBankAccts.FindControl("ddlPaymentType"), DropDownList).SelectedValue)
                    'Dim bIsDrAccType As Boolean = CBool(DirectCast(dgiBankAccts.FindControl("ddlAccType"), DropDownList).SelectedValue)
                    'Dim strOrgCode As String = DirectCast(dgiBankAccts.FindControl("txtOrgCode"), TextBox).Text
                    If intAcctId = 0 Then

                        'Execute SQL Command
                        With cmdBankAccts
                            .Connection = clsGeneric.SQLConnection
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = "pg_BankAcctsUpdate"
                            .Parameters.Clear()
                            .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                            .Parameters.Add(New SqlParameter("@in_AcctName", strAcctName))
                            .Parameters.Add(New SqlParameter("@in_AcctNo", strAcctNumber))
                            .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                            '.Parameters.Add(New SqlParameter("@in_BnkOrgCode", strOrgCode))
                            '.Parameters.Add(New SqlParameter("@in_PaySerID", intPaymentID))
                            '.Parameters.Add(New SqlParameter("@in_IsDrAccType", bIsDrAccType))

                            .ExecuteNonQuery()
                        End With

                    End If

                Next

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog("fncUpdateBankAccs - clsCustomer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdBankAccts = Nothing

                'Destroy Instance of DataSet
                dsBankAccts = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Datagrid Item
                dgiBankAccts = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaBankAccts = Nothing

            End Try

        End Function
        Public Function fncUpdateBankAccs(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strAcctName As String, ByVal strAcctNumber As String, ByVal intBankID As Integer, ByVal intPaymentID As Integer, ByVal intAcctType As Integer) As Boolean
            'Create Instance of Data Set
            Dim dsBankAccts As New DataSet

            'Create Instance of SQL Command Object
            Dim cmdBankAccts As New SqlCommand

            'Create Instance of Datagrid Item
            Dim dgiBankAccts As DataGridItem

            'Create Instance of SQL Data Adapter
            Dim sdaBankAccts As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            'Dim intAcctId As Int16

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'For Each dgiBankAccts In dgBankAccts.Items

                'If IsNumeric(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex)) Then
                '   intAcctId = Convert.ToInt32(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex))
                'Else
                '   intAcctId = 0
                'End If


                'Dim strOrgCode As String = DirectCast(dgiBankAccts.FindControl("txtOrgCode"), TextBox).Text
                'Dim intAcctId As Integer = 0

                'Execute SQL Command
                With cmdBankAccts
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_BankAcctsUpdate"
                    .Parameters.Clear()
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_AcctName", strAcctName))
                    .Parameters.Add(New SqlParameter("@in_AcctNo", strAcctNumber))
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                    '.Parameters.Add(New SqlParameter("@in_BnkOrgCode", strOrgCode))
                    .Parameters.Add(New SqlParameter("@in_PaySerID", intPaymentID))
                    .Parameters.Add(New SqlParameter("@in_AcctType", intAcctType))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog("fncUpdateBankAccs - clsCustomer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdBankAccts = Nothing

                'Destroy Instance of DataSet
                dsBankAccts = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Datagrid Item
                dgiBankAccts = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaBankAccts = Nothing

            End Try

        End Function
        Public Function fncUpdateBankAccs(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strAcctName As String, ByVal strAcctNumber As String, ByVal intBankID As Integer) As Boolean

            'Create Instance of Data Set
            Dim dsBankAccts As New DataSet

            'Create Instance of SQL Command Object
            Dim cmdBankAccts As New SqlCommand

            'Create Instance of Datagrid Item
            Dim dgiBankAccts As DataGridItem

            'Create Instance of SQL Data Adapter
            Dim sdaBankAccts As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            'Dim intAcctId As Int16

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'For Each dgiBankAccts In dgBankAccts.Items

                'If IsNumeric(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex)) Then
                '   intAcctId = Convert.ToInt32(dgBankAccts.DataKeys(dgiBankAccts.ItemIndex))
                'Else
                '   intAcctId = 0
                'End If


                'Dim strOrgCode As String = DirectCast(dgiBankAccts.FindControl("txtOrgCode"), TextBox).Text
                'Dim intAcctId As Integer = 0

                'Execute SQL Command
                With cmdBankAccts
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_BankAcctsUpdate"
                    .Parameters.Clear()
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_AcctName", strAcctName))
                    .Parameters.Add(New SqlParameter("@in_AcctNo", strAcctNumber))
                    .Parameters.Add(New SqlParameter("@in_BankID", intBankID))
                    '.Parameters.Add(New SqlParameter("@in_BnkOrgCode", strOrgCode))
                    '.Parameters.Add(New SqlParameter("@in_PaySerID", intPaymentID))
                    '.Parameters.Add(New SqlParameter("@in_AcctType", intAcctType))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog("fncUpdateBankAccs - clsCustomer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdBankAccts = Nothing

                'Destroy Instance of DataSet
                dsBankAccts = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Datagrid Item
                dgiBankAccts = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaBankAccts = Nothing

            End Try

        End Function


#End Region

#Region "Get Bank Accts"

        Public Shared Function fncGetBankOrgID(ByVal intOrgId As Long, ByVal intGroupId As Integer, ByVal sTempDebitBankAccNo As String, ByVal sPaymentDate As String) As String
            Dim strRetVal As String = ""
            Dim clsGeneric As New Generic
            Dim strSQL As String = "pg_QryBankOrgCode " & intOrgId.ToString & "," & intGroupId.ToString & "," & clsDB.SQLStr(sTempDebitBankAccNo) & "," & clsDB.SQLStr(sPaymentDate)
            Try
                strRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                Throw ex
            End Try
            Return strRetVal
        End Function

        'Public Shared Function fncGetBankOrgID(ByVal intOrgId As Integer, ByVal intGroupId As Integer) As String
        '    Dim strRetVal As String = ""
        '    Dim clsGeneric As New Generic
        '    Dim strSQL As String = "pg_QryBankOrgCode " & intOrgId.ToString & "," & intGroupId.ToString
        '    Try
        '        strRetVal = SqlHelper.ExecuteScalar(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        '    Return strRetVal
        'End Function

        '****************************************************************************************************
        'Function Name  : fncGetBankAccts()
        'Purpose        : To Populate the Bank Accounts
        'Arguments      : Organisation Id,Datagrid
        'Return Value   : DataSet
        'Author         : Sujith Sharatchandran - 
        'Created        : 13/02/2005
        '*****************************************************************************************************
        Public Function fncGetBankAccts(ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance of Data Set
            Dim dsBankAccts As New DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adapter
            Dim sdaBankAccts As New SqlDataAdapter

            'Variable Declarations
            Dim strSQL As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Build SQL Statement
                strSQL = "Exec pg_BankAccts " & lngOrgId

                'Execute SQL Data Adapter
                sdaBankAccts = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaBankAccts.Fill(dsBankAccts, "ACCTS")

                'Return Data Set
                Return dsBankAccts

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncGetBankAccts - clsCustomer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Data Adapter
                sdaBankAccts = Nothing

                'Destroy Instance of Data Set
                dsBankAccts = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

        Public Function fncQryGroupBankAccountList(ByVal iOrgID As Integer, ByVal iGroupID As Integer, ByVal iBankID As Integer) As DataSet
            Dim strSQL As String = "pg_QryBankAccounts"
            Dim clsGeneric As New Generic
            Dim params(2) As SqlParameter
            Dim dsRetVal As New DataSet
            params(0) = New SqlParameter("@in_Org_ID", SqlDbType.Int)
            params(0).Value = iOrgID
            params(1) = New SqlParameter("@in_Group_ID", SqlDbType.Int)
            params(1).Value = iGroupID
            params(2) = New SqlParameter("@in_BankID", SqlDbType.Int)
            params(2).Value = iBankID
            dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Return dsRetVal
        End Function

        'Public Function fncQrySelectedGroupBankAccount(ByVal iOrgID As Integer, ByVal iGroupID As Integer, ByVal iBankID As Integer) As DataSet
        '    Dim strSQL As String = "pg_QrySelectedBankAccounts"
        '    Dim clsGeneric As New Generic
        '    Dim params(2) As SqlParameter
        '    Dim dsRetVal As New DataSet
        '    params(0) = New SqlParameter("@in_Org_ID", SqlDbType.Int)
        '    params(0).Value = iOrgID
        '    params(1) = New SqlParameter("@in_Group_ID", SqlDbType.Int)
        '    params(1).Value = iGroupID
        '    params(2) = New SqlParameter("@in_BankID", SqlDbType.Int)
        '    params(2).Value = iBankID
        '    dsRetVal = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, params)
        '    Return dsRetVal
        'End Function

        Public Function fncQrySelectedGroupBankAccount(ByVal iOrgID As Integer, ByVal iGroupID As Integer) As SqlDataReader
            Dim strSQL As String = "pg_QrySelectedBankAccounts"
            Dim clsGeneric As New Generic
            Dim params(1) As SqlParameter
            Dim drRetVal As SqlDataReader
            params(0) = New SqlParameter("@in_Org_ID", SqlDbType.Int)
            params(0).Value = iOrgID
            params(1) = New SqlParameter("@in_Group_ID", SqlDbType.Int)
            params(1).Value = iGroupID
            'params(3) = New SqlParameter("@in_BankID", SqlDbType.Int)
            'params(3).Value = iBankID
            'params(2) = New SqlParameter("@in_PaySer_ID", SqlDbType.Int)
            'params(2).Value = iPaySerID
            drRetVal = SqlHelper.ExecuteReader(clsGeneric.strSQLConnection, CommandType.StoredProcedure, strSQL, params)
            Return drRetVal
        End Function

        Public Sub prcBindBankAccountCheckBox(ByRef ChkBoxList As CheckBoxList, ByVal iOrgID As Integer, ByVal iGroupID As Integer)
            Dim drInfo As SqlDataReader
            Dim lsItem As ListItem

            drInfo = fncQrySelectedGroupBankAccount(iOrgID, iGroupID)

            While drInfo.Read
                'ChkBoxList.Items.Add(New ListItem(drInfo.Item("Account_Name"), drInfo.Item("Account_Id")))
                If drInfo.Item("Checked") = "1" Then
                    For Each lsItem In ChkBoxList.Items
                        If drInfo.Item("Account_Id") = lsItem.Value Then
                            lsItem.Selected = True
                        End If
                    Next
                    'ChkBoxList.Items(ChkBoxList.Items.Count - 1).Selected = True
                End If
            End While
        End Sub
#End Region

#Region "Delete Bank Accts"

        '****************************************************************************************************
        'Function Name  : fncDelBankAccts()
        'Purpose        : To Delete Bank Accounts
        'Arguments      : Organisation Id,User Id, Account Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 06/03/2005
        '*****************************************************************************************************
        Public Function fncDelBankAccts(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal lngAccId As Long, _
                Optional ByVal strRequest As String = "D", Optional ByVal IsPrimary As Boolean = False) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdBankAccts As New SqlCommand

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdBankAccts
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_DelBankAccts"
                    .Parameters.Add(New SqlParameter("@in_Request", strRequest))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_AccId", lngAccId))
                    .Parameters.Add(New SqlParameter("@in_Primary", IsPrimary))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch ex As Exception

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncDelBankAccts - clsCustomer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region
#Region "Payment parentchild"
        Public Function fncGetChild(ByVal strOption As String, ByVal lngOrgId As Long, ByVal parentid As Integer) As System.Data.DataSet

            'Create Instance SQL Data Adapter
            Dim sdaPayService As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsPayService As New System.Data.DataSet

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'If New or Modify Organisation
                If strOption = "N" Then

                    ''Get Payment Services

                    sdaPayService = New SqlDataAdapter("Exec CIMBGW_SubPaymentService 'N'," & lngOrgId & "," & parentid, clsGeneric.SQLConnection)
                    sdaPayService.Fill(dsPayService, "PAYSERVICE")

                    'Get Sub Payment Service
                ElseIf strOption = "Y" Then
                    sdaPayService = New SqlDataAdapter("Exec CIMBGW_SubPaymentService 'Y'," & lngOrgId & "," & parentid, clsGeneric.SQLConnection)
                    sdaPayService.Fill(dsPayService, "PAYSERVICE")



                End If

                Return dsPayService

            Catch

                Call clsGeneric.ErrorLog(lngOrgId, _generic.ss_lngUserID, "clsCustomer - fncPayService", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL DataAdapter
                sdaPayService = Nothing

                'Destroy Instance of generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsPayService = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Payment Service/Service Account"

        '****************************************************************************************************
        'Function Name  : fncPayService
        'Purpose        : To populate State 
        'Arguments      : Organisation ID, User Id
        'Return Value   : Data Set
        'Author         : Sujith Sharatchandran - 
        'Created        : 07/07/2004
        '*****************************************************************************************************
        Public Function fncPayService(ByVal strOption As String, ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance SQL Data Adapter
            Dim sdaPayService As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsPayService As New System.Data.DataSet

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'If New or Modify Organisation
                If strOption = "N" Or strOption = "M" Then

                    'Get Payment Services
                    sdaPayService = New SqlDataAdapter("Exec pg_GetPayService 'N'," & lngOrgId, clsGeneric.SQLConnection)
                    sdaPayService.Fill(dsPayService, "PAYSERVICE")

                    'Get Selected Payment Service
                    If strOption = "M" Then
                        sdaPayService = New SqlDataAdapter("Exec pg_GetPayService 'M'," & lngOrgId, clsGeneric.SQLConnection)
                        sdaPayService.Fill(dsPayService, "SELPAYSERVICE")
                    End If

                    'If Service Account No
                ElseIf (strOption = "E" Or strOption = "S" Or strOption = "L" Or strOption = "Z") Then

                    'Get Service Accounts
                    sdaPayService = New SqlDataAdapter("Exec pg_GetPayService '" & strOption & "'," & lngOrgId, clsGeneric.SQLConnection)
                    sdaPayService.Fill(dsPayService, "SERVICEACC")

                End If

                Return dsPayService

            Catch

                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCustomer - fncPayService", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL DataAdapter
                sdaPayService = Nothing

                'Destroy Instance of generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsPayService = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Service Accounts"

        '****************************************************************************************************
        'Function Name  : fncUpdateServiceAccs()
        'Purpose        : To Insert/UpDate Service Accts
        'Arguments      : Organisation Id,Datagrid
        'Return Value   : DataSet
        'Author         : Sujith Sharatchandran - .
        'Date           : 13/10/2005
        '*****************************************************************************************************
        Public Function fncUpdateServiceAccs(ByVal strAction As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                        ByVal dgServiceAccts As DataGrid, ByVal strService As String) As Boolean

            'Create Instance of Data Set
            Dim dsServiceAccts As New DataSet

            'Create Instance of SQL Command Object
            Dim cmdServiceAccts As New SqlCommand

            'Create Instance of Datagrid Item
            Dim dgiServiceAccts As DataGridItem

            'Create Instance of SQL Data Adapter
            Dim sdaServiceAccts As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intAcctId As Int16

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                For Each dgiServiceAccts In dgServiceAccts.Items

                    If IsNumeric(dgServiceAccts.DataKeys(dgiServiceAccts.ItemIndex)) Then
                        intAcctId = Convert.ToInt32(dgServiceAccts.DataKeys(dgiServiceAccts.ItemIndex))
                    Else
                        intAcctId = 0
                    End If

                    Dim strAcctName As String = CType(dgiServiceAccts.FindControl("txtServName"), TextBox).Text
                    Dim strAcctNumber As String = CType(dgiServiceAccts.FindControl("txtServAcc"), TextBox).Text
                    Dim intState As Int16 = CType(dgiServiceAccts.FindControl("cmbState"), DropDownList).SelectedValue

                    Dim strEmpName As String = ""

                    If Not IsNothing(dgiServiceAccts.FindControl("txtEmpName")) Then
                        strEmpName = CType(dgiServiceAccts.FindControl("txtEmpName"), TextBox).Text
                    End If

                    If intAcctId = 0 And strAction = "ADD" Then

                        'Execute SQL Command
                        With cmdServiceAccts
                            .Connection = clsGeneric.SQLConnection
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = "pg_ServiceAccount"
                            .Parameters.Clear()
                            .Parameters.Add(New SqlParameter("@in_Action", strAction))
                            .Parameters.Add(New SqlParameter("@in_AcctId", intAcctId))
                            .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                            .Parameters.Add(New SqlParameter("@in_SerName", strAcctName))
                            .Parameters.Add(New SqlParameter("@in_SerNo", strAcctNumber))
                            .Parameters.Add(New SqlParameter("@in_State", intState))
                            .Parameters.Add(New SqlParameter("@in_SerCode", strService))
                            .Parameters.Add(New SqlParameter("@in_EmpName", strEmpName))
                            .ExecuteNonQuery()
                        End With

                    ElseIf intAcctId > 0 And strAction = "UPDATE" Then
                        'Execute SQL Command
                        With cmdServiceAccts
                            .Connection = clsGeneric.SQLConnection
                            .CommandType = CommandType.StoredProcedure
                            .CommandText = "pg_ServiceAccount"
                            .Parameters.Clear()
                            .Parameters.Add(New SqlParameter("@in_Action", strAction))
                            .Parameters.Add(New SqlParameter("@in_AcctId", intAcctId))
                            .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                            .Parameters.Add(New SqlParameter("@in_SerName", strAcctName))
                            .Parameters.Add(New SqlParameter("@in_SerNo", strAcctNumber))
                            .Parameters.Add(New SqlParameter("@in_State", intState))
                            .Parameters.Add(New SqlParameter("@in_SerCode", strService))
                            .Parameters.Add(New SqlParameter("@in_EmpName", strEmpName))
                            .ExecuteNonQuery()
                        End With
                    End If

                Next

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "clsCustomer - fncUpdateServiceAccs", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdServiceAccts = Nothing

                'Destroy Instance of DataSet
                dsServiceAccts = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Datagrid Item
                dgiServiceAccts = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaServiceAccts = Nothing

            End Try

        End Function


#End Region

#Region "Get Public Hash Total"

        '****************************************************************************************************
        'Function Name  : fnPHTotal
        'Purpose        : To Get Public Hash Total for the Salary
        'Arguments      : File Id
        'Return Value   : Public Hash Total
        'Author         : Eu Yean Lock - 
        'Created        : 02/10/2006
        '*****************************************************************************************************
        Public Function fnPHTotal(ByVal lngFileId As Long, ByVal strFileType As String) As String

            Dim sdrPHTotal As SqlDataReader            'Create Instance SQL Data Reader
            Dim clsGeneric As New MaxPayroll.Generic       'Create Instance of Generic Class Object

            'Variable Declaration
            Dim strPHTotal As String = "", strSQL As String

            Try

                'Execute Stored Procedure
                strSQL = "Exec pg_PubHashTotal " & lngFileId & ", '" & strFileType & "'"

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Command Object to Execute SQL Data Reader
                Dim cmdTranDesc As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                sdrPHTotal = cmdTranDesc.ExecuteReader(CommandBehavior.CloseConnection)

                If sdrPHTotal.HasRows Then
                    sdrPHTotal.Read()
                    strPHTotal = sdrPHTotal("PHTotal")
                    sdrPHTotal.Close()
                Else
                    sdrPHTotal.Close()

                End If

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Return Public Hash Total
                Return strPHTotal

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnPHTotal", Err.Number, Err.Description)

            Finally

                'Destroy Instance of SQL Data Reader
                sdrPHTotal = Nothing

                'Destroy Instance of Generic Class Object

                clsGeneric = Nothing


            End Try
            Return ""
        End Function

#End Region

    End Class

End Namespace
