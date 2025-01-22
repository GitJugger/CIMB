'****************************************************************************************************
'Class Name     : clsPinMailer.vb
'ProgId         : MaxPayroll.clsPinMailer
'Purpose        : Pin Mailer Functions Used For The Complete Project
'Author         : Sujith Sharatchandran - 
'Created        : 28/01/2004
'*****************************************************************************************************
Imports System.IO
Imports System.Net
Imports System.Text
Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient


Namespace MaxPayroll


Public Class clsPinMailer

#Region "Get Generate Id"

    '****************************************************************************************************
    'Procedure Name : fncGenerateId
    'Purpose        : Get Unique Id to be used as Generate Id
    'Arguments      : N/A
    'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 06/07/2005
        '*****************************************************************************************************
        Public Function fncGenerateId(ByVal lngUserId As Long) As Int16

            'Create Instance of SQL Command Object
            Dim cmdGenerate As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations 
            Dim intGenId As Int16

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdGenerate
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_GenerateId"
                    intGenId = .ExecuteScalar()
                End With

                Return intGenId

            Catch

                'Log Error
                clsGeneric.ErrorLog(100000, lngUserId, "fncGenerateId - clsPinMailer", Err.Number, Err.Description)

                Return 0

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Search Pin Requisition"

        '****************************************************************************************************
        'Procedure Name : fncPinRequisition()
        'Purpose        : Get Pin Mailer Details from Organisation table
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Public Function fncSearchPinRequisition(ByVal lngUserId As Long, ByVal strSearchBy As String, _
                        ByVal strOrgType As String, ByVal lngOrgIdFrom As Long, ByVal lngOrgIdTo As Long, _
                                ByVal strOption As String, ByVal strCriteria As String, ByVal strKeyword As String)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsPinRequisition As New DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaPinRequisition As New SqlDataAdapter

            'Variable Declarations
            Dim strSQL As String

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()


                'Build SQL Statment
                strSQL = "Exec pg_SearchPinRequisition '" & strSearchBy & "','" & strOrgType & "'," & lngOrgIdFrom & "," & lngOrgIdTo & ",'"
                strSQL = strSQL & strOption & "','" & strCriteria & "','" & strKeyword & "'"

                'Execute SQL Data Adaptor
                sdaPinRequisition = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fetch Record And Fill Data Set
                sdaPinRequisition.Fill(dsPinRequisition, "ORGPIN")

                Return dsPinRequisition

            Catch

                'Log Error
                clsGeneric.ErrorLog(100000, lngUserId, "fncSearchPinRequisition - clsPinMailer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of System Data Set
                dsPinRequisition = Nothing

                'Destroy Instance SQL Data Adaptor
                sdaPinRequisition = Nothing

                'Destroy Instance Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Build String For Pin Mailer"

    '****************************************************************************************************
    'Procedure Name : fncFileContents
    'Purpose        : To Build the String to be written to the File
    'Arguments      : Dataset
    'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 29/04/2004
        '*****************************************************************************************************
        Public Function fncFileContents(ByVal intGenId As Int16) As String

            'Create Instance of Data Row
            Dim drPinMailer As DataRow

            'Create Instance of System Data Set
            Dim dsPinMailer As New System.Data.DataSet

            'Create Instance of SQL Data Adaptor
            Dim sdaPinMailer As New SqlDataAdapter

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Encryption Class Object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Variable Declarations
            Dim IsGenerate As Boolean
            Dim strReqUser As String
            Dim strOrgId As String
            Dim strOrgName As String
            Dim strPassword As String
            Dim strRow As String
            Dim intReqId As Integer
            Dim lngUserId As Long
            Dim strFileContent As String = ""
            Dim intSerial As Int16
            Dim strCodeReq As String
            Dim strPhone As String
            Dim strAuthCode As String
            Dim strOrgAdd As String
            Dim strOrgState As String
            Dim strOrgPin As String
            Dim strOrgCountry As String
            Dim strUserLogin As String
            Dim strUserType As String
            Dim strUserName As String

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adapter
                sdaPinMailer = New SqlDataAdapter("Exec pg_PinGenerateDetails " & intGenId, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaPinMailer.Fill(dsPinMailer, "PINMAILER")

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Loop Thro the Data Set - START
                For Each drPinMailer In dsPinMailer.Tables(0).Rows

                    IsGenerate = False                      'Set Generate Flag 
                    lngUserId = drPinMailer("UID")          'Get User Id
                    strOrgId = drPinMailer("ORGID")         'Get Organization Id
                    intReqId = drPinMailer("REQID")         'Get Request Id
                    strPhone = drPinMailer("ORGPHN")        'Get Organization Phone
                    strOrgName = drPinMailer("ONAME")       'Get Organization Name
                    strPassword = drPinMailer("UPWD")       'Get User Password
                    strAuthCode = drPinMailer("UAUTH")      'Get Authorzation Code
                    strOrgAdd = drPinMailer("ORGADD")       'Get Organization Address
                    strOrgPin = drPinMailer("ORGPIN")       'Get Organization Pincode
                    strUserType = drPinMailer("UTYPE")      'Get User Type
                    strUserName = drPinMailer("UNAME")      'Get User Name
                    strUserLogin = drPinMailer("ULOGIN")    'Get User Login
                    strOrgState = drPinMailer("ORGSTATE")   'Get Organization State
                    strOrgCountry = drPinMailer("ORGCON")   'Get Organization Country
                    strCodeReq = drPinMailer("REQCODE")     'Get Requested Code
                    strReqUser = drPinMailer("REQUSER")     'Get Requested User

                    'Replace Carriage Return with empty space
                    strOrgAdd = Replace(strOrgAdd, vbCrLf, " ")

                    'If both Custom Admin/System Auth
                    If strReqUser = "B" Then
                        IsGenerate = True
                        'If Only Custom Admin
                    ElseIf strReqUser = "A" Then
                        If strUserType = "CA" Then
                            IsGenerate = True
                        ElseIf strUserType = "SA" Then
                            IsGenerate = False
                        End If
                        'If Only System Auth
                    ElseIf strReqUser = "S" Then
                        If strUserType = "CA" Then
                            IsGenerate = False
                        ElseIf strUserType = "SA" Then
                            IsGenerate = True
                        End If
                    End If

                    If IsGenerate Then

                        Select Case strCodeReq

                            Case "B"

                                'Get Running Serial No
                                intSerial = fnGetSerial()

                                'Update Serial No
                                Call prcUserSerial(intReqId, strUserType, intSerial, "P")

                                'Decrypt Password
                                strPassword = clsEncryption.Cryptography(strPassword)

                                'Build String
                                strRow = 0 & ";" & strOrgId & ";" & strOrgName & ";" & strOrgAdd & ";" & strOrgPin & ";" & strOrgState & ";" & strOrgCountry & ";"
                                strRow = strRow & strPhone & ";" & strUserName & ";" & strUserLogin & ";" & strPassword & ";" & strUserType & ";P" & vbCrLf

                                'Append Each Row
                                strFileContent = strFileContent & strRow

                                'Get Running Serial No
                                intSerial = fnGetSerial()

                                'Update Serial No
                                Call prcUserSerial(intReqId, strUserType, intSerial, "A")

                                'Decrypt Authorization
                                strAuthCode = clsEncryption.Cryptography(strAuthCode)

                                'Build String
                                strRow = 0 & ";" & strOrgId & ";" & strOrgName & ";" & strOrgAdd & ";" & strOrgPin & ";" & strOrgState & ";" & strOrgCountry & ";"
                                strRow = strRow & strPhone & ";" & strUserName & ";" & strUserLogin & ";" & strAuthCode & ";" & strUserType & ";A" & vbCrLf

                                'Append Each Row
                                strFileContent = strFileContent & strRow

                            Case "P"

                                'Get Running Serial No
                                intSerial = fnGetSerial()

                                'Update Serial No
                                Call prcUserSerial(intReqId, strUserType, intSerial, "P")

                                'Decrypt Password
                                strPassword = clsEncryption.Cryptography(strPassword)

                                'Build String
                                strRow = 0 & ";" & strOrgId & ";" & strOrgName & ";" & strOrgAdd & ";" & strOrgPin & ";" & strOrgState & ";" & strOrgCountry & ";"
                                strRow = strRow & strPhone & ";" & strUserName & ";" & strUserLogin & ";" & strPassword & ";" & strUserType & ";P" & vbCrLf

                                'Append Each Row
                                strFileContent = strFileContent & strRow

                            Case "A"

                                'Get Running Serial No
                                intSerial = fnGetSerial()

                                'Update Serial No
                                Call prcUserSerial(intReqId, strUserType, intSerial, "A")

                                'Decrypt Authorization
                                strAuthCode = clsEncryption.Cryptography(strAuthCode)

                                'Build String
                                strRow = vbCrLf & 0 & ";" & strOrgId & ";" & strOrgName & ";" & strOrgAdd & ";" & strOrgPin & ";" & strOrgState & ";" & strOrgCountry & ";"
                                strRow = strRow & strPhone & ";" & strUserName & ";" & strUserLogin & ";" & strAuthCode & ";" & strUserType & ";A" & vbCrLf

                                'Append Each Row
                                strFileContent = strFileContent & strRow

                        End Select

                    End If

                Next
                'Loop Thro the Data Set - STOP

                Return strFileContent

            Catch

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fncFileContents - clsPinMailer", Err.Number, Err.Description)

                Return ""

            Finally

                'Destroy Instance of Datarow
                drPinMailer = Nothing

                'Destroy Instance of System Data Set
                dsPinMailer = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaPinMailer = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Encryption Class Object
                clsEncryption = Nothing

            End Try

        End Function

#End Region

#Region "Update User Serial No"

        '****************************************************************************************************
        'Procedure Name : prcUserSerial
        'Purpose        : To Update the User Id Serial No
        'Arguments      : Dataset
        'Return Value   : String
        'Author         : Sujith Sharatchandran - 
        'Created        : 29/04/2004
        '*****************************************************************************************************
        Private Sub prcUserSerial(ByVal intReqId As Integer, ByVal strUserType As String, ByVal intSerial As Integer, ByVal strCodeType As String)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdPinMailer As New SqlCommand

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdPinMailer
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_UserSerialNo"
                    .Parameters.Add(New SqlParameter("@in_ReqId", intReqId))
                    .Parameters.Add(New SqlParameter("@in_UserType", strUserType))
                    .Parameters.Add(New SqlParameter("@in_SerialNo", intSerial))
                    .Parameters.Add(New SqlParameter("@in_CodeType", strCodeType))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "prcUserSerial - clsPinMailer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : fnGetSerial()
        'Purpose        : This function is for returning the next serial number and update the database by one
        'Arguments      : Dataset
        'Return Value   : String
        'Author         : Uma Mahesh - 
        'Created        : 18/04/2005
        '*****************************************************************************************************
        Private Function fnGetSerial() As Integer

            'Create Instance of DataTable
            Dim datTable As DataTable

            'Create Instance of SQL Data Adapter
            Dim sdaOrg As New SqlDataAdapter

            'Create Instance of Data Set
            Dim dsOrg As New System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQLStatment As String, intRecordCount As Integer

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                strSQLStatment = "Exec pg_GetNextSerial"

                'Execute SQL Data Adaptor
                sdaOrg = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaOrg.Fill(dsOrg, "SerialNext")

                intRecordCount = dsOrg.Tables("SerialNext").Rows.Count
                If intRecordCount > 0 Then
                    datTable = dsOrg.Tables("SerialNext")
                    fnGetSerial = datTable.Rows(0)("NextSerial")
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnOrgGrid", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaOrg = Nothing

            End Try

        End Function
#End Region

#Region "Insert Requisition"

        '****************************************************************************************************
        'Procedure Name : fncPinRequisition()
        'Purpose        : Insert Pin Requisition Details
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************

        Public Function fncPinRequisition(ByVal strOption As String, ByVal intReqId As Int16, _
                ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strReqRemark As String, _
                    ByVal strReqStatus As String, ByVal strUserType As String, ByVal strCodeType As String, _
                            ByVal strApprRemark As String, ByVal strGenFlag As String, Optional ByVal intGenId As Int16 = 0) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdPinRequisition As New SqlCommand

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdPinRequisition
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_PinRequisition"
                    .Parameters.Clear()
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_ReqId", intReqId))
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .Parameters.Add(New SqlParameter("@in_ReqUserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_ReqRemark", strReqRemark))
                    .Parameters.Add(New SqlParameter("@in_ReqStatus", strReqStatus))
                    .Parameters.Add(New SqlParameter("@in_UserType", strUserType))
                    .Parameters.Add(New SqlParameter("@in_CodeType", strCodeType))
                    .Parameters.Add(New SqlParameter("@in_ApprRemark", strApprRemark))
                    .Parameters.Add(New SqlParameter("@in_GenFlag", strGenFlag))
                    .Parameters.Add(New SqlParameter("@in_GenId", intGenId))
                    .ExecuteNonQuery()
                End With

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog("100000", lngUserId, "fncPinRequisition - clsPinMailer", Err.Number, Err.Description)

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdPinRequisition = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Pin Mailer Status"

        '****************************************************************************************************
        'Procedure Name : fncPinStatus()
        'Purpose        : Insert Pin Requisition Details
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Public Function fncPinStatus(ByVal lngUserId As Long, ByVal strStatus As String, ByVal intDays As Int16) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adapter
            Dim sdaRequisition As New SqlDataAdapter

            'Create Instance of System Data Set
            Dim dsRequisition As New System.Data.DataSet

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                sdaRequisition = New SqlDataAdapter("Exec pg_PinStatus '" & strStatus & "'," & intDays, clsGeneric.SQLConnection)
                sdaRequisition.Fill(dsRequisition, "PINSTATUS")

                Return dsRequisition

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "fncPinStatus - clsPinMailer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Annual Fee Charge"

        '****************************************************************************************************
        'Procedure Name : fncFeeCharge()
        'Purpose        : Annual Fees Charge
        'Arguments      : Organisation Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Public Sub fncFeeCharge(ByVal lngOrgId As Long)

            'Create Instance of SQL Command Object
            Dim cmdFeeCharge As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdFeeCharge
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_FeeCharge"
                    .Parameters.Add(New SqlParameter("@in_OrgId", lngOrgId))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, 0, "clsPinMailer - fncFeeCharge", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdFeeCharge = Nothing

            End Try

        End Sub

#End Region

#Region "Get User/Code Details"

        '****************************************************************************************************
        'Procedure Name : fncUserCode()
        'Purpose        : To get the User Type and Code Type
        'Arguments      : Organisation Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Public Function fncUserCode(ByVal lngUserId As Long, ByVal lngOrgId As Long) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Data Adapter
            Dim sdaUserCode As New SqlDataAdapter

            'Create Instance of System Data Set
            Dim dsUserCode As New System.Data.DataSet

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL DataAdapter
                sdaUserCode = New SqlDataAdapter("Exec pg_PinUserCode " & lngOrgId, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaUserCode.Fill(dsUserCode, "USERCODE")

                Return dsUserCode

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "fncUserCode - clsPinMailer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of System Data Set
                dsUserCode = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaUserCode = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Pin Serial Check"

        '****************************************************************************************************
        'Procedure Name : fncPinSerialCheck()
        'Purpose        : To Check if Pin Serial No Available
        'Arguments      : Option
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Public Function fncPinSerialCheck(ByVal strOption As String, ByVal lngUserId As Long) As Int16

            'Create Instance of SQL Command Object
            Dim cmdSerialNo As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intCount As Int16

            Try

                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdSerialNo
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_PinSerialCheck"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    intCount = .ExecuteScalar()
                End With

                Return intCount

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "fncPinSerialCheck - clsPinMailer", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection 
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdSerialNo = Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
