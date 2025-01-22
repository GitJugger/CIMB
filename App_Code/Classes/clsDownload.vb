'****************************************************************************************************
'Class Name     : clsDownload
'ProgId         : MaxPayroll.clsDownload
'Purpose        : Download Functions Used For Download Module
'Author         : Sujith Sharatchandran - 
'Created        : 15/01/2004
'*****************************************************************************************************
Imports System.IO
Imports System.Data
Imports MaxPayroll.Generic
'Imports MaxPayroll.Mailbox
Imports MaxPayroll.clsUpload
Imports System.Data.SqlClient


Namespace MaxPayroll


    Public Class clsDownload

#Region "Read File"

        '****************************************************************************************************
        'Function Name  : fnReadFile
        'Purpose        : To Get the Status of the Downloaded File
        'Arguments      : File Name
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 15/01/2004
        '*****************************************************************************************************
        Public Function fnReadFile(ByVal strFileName As String) As Boolean

            Dim flReader As StreamReader            'Create Instance of File Stream Reader Object
            Dim cmdPayStatus As New SqlCommand      'Create Instance of SQL Command Object
            Dim clsGeneric As New MaxPayroll.Generic   'Create Instance of Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon  'Create Instance of clsUpload Class Object

            'Variable Declarations
            Dim strTranDesc As String
            Dim IsUpdate As Boolean
            Dim intStartPos As Int16
            Dim lngAmount As Long
            Dim lngOrgId As Long
            Dim intTranStatus As Int16
            Dim IsRead As Boolean
            Dim strFileLine As String
            Dim intCounter As Int16
            Dim strAccNo As String = ""
            Dim strAmountCol As String = ""
            Dim strAccNoCol As String = ""
            Dim intEndPos As Int16
            Dim strSQL As String
            Dim lngFileId As Long
            Dim strFile As String

            Try

                'Set Default File Read to False
                IsRead = False

                'Set Counter
                intCounter = 1

                'Remove Path From File Name
                strFile = clsCommon.fncFileName(strFileName, False)

                'Get Organisation Id From File Name
                lngOrgId = IIf(IsNumeric(Left(strFile, 6)), Left(strFile, 6), 0)

                'Get File Id For the Given File Name
                lngFileId = fnFileId(strFile, lngOrgId)

                'If File Exists
                If File.Exists(strFileName) Then
                    'Open File
                    flReader = File.OpenText(strFileName)
                    'Loop Thro File
                    While flReader.Peek <> -1
                        'Get the Contents of the Current Line
                        strFileLine = flReader.ReadLine()
                        'Get the Transaction Status for The Downloaded File
                        intTranStatus = IIf(IsNumeric(Right(strFileLine, 2)), Right(strFileLine, 2), 0)
                        'If Transaction Code is greater Than zero
                        If intTranStatus > 0 And intCounter = 1 Then
                            'Get Trans Description
                            strTranDesc = fnTranDesc(intTranStatus)
                            'Update File Submission Status
                            IsUpdate = fnUpdateSubmission(strFileName, strTranDesc, 0)
                            'If Transaction Code is zero
                        ElseIf intTranStatus = 0 And intCounter = 1 Then
                            'Update File Submission Status
                            IsUpdate = fnUpdateSubmission(strFileName, "", 1)
                        ElseIf intTranStatus = 0 And intCounter > 1 Then
                            'Get Account Column Description,Start Position,End Position
                            Call fnFieldDesc("AN", strAccNoCol, intStartPos, intEndPos)
                            'Get Account No From File
                            strAccNo = Mid(strFileLine, intStartPos, (intEndPos - intStartPos) + 1)
                            'Get Amount Column Description,Start Position,End Position
                            Call fnFieldDesc("AM", strAmountCol, intStartPos, intEndPos)
                            'Get Amount From File
                            lngAmount = Mid(strFileLine, intStartPos, (intEndPos - intStartPos) + 1)
                            'Build Update Statement
                            strSQL = "Update tTemp_FileBody Set Status = 1 Where [File Id] = " & lngFileId & " And [" & strAccNoCol & "] = '" & strAccNo & "' And [" & strAmountCol & "] = '" & lngAmount & "'"
                            'Intialise SQL Connection
                            Call clsGeneric.SQLConnection_Initialize()
                            'Update Pay Status - Start
                            cmdPayStatus.CommandText = strSQL
                            cmdPayStatus.CommandType = CommandType.Text
                            cmdPayStatus.Connection = clsGeneric.SQLConnection
                            cmdPayStatus.ExecuteNonQuery()
                            'Update Pay Status - Start
                            'Terminate SQL Connection
                            Call clsGeneric.SQLConnection_Terminate()
                            strAccNo = ""
                            lngAmount = 0
                            intStartPos = 0
                            intEndPos = 0
                            strSQL = ""
                        End If
                        'Increment Counter
                        intCounter = intCounter + 1
                    End While
                    'Close File
                    flReader.Close()
                End If

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnReadFile", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy Instance of File Stream Reader
                flReader = Nothing

                'Destroy Instance of SQL Command Object
                cmdPayStatus = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Transaction Description"

        '****************************************************************************************************
        'Function Name  : fnTranDesc
        'Purpose        : To Get Description for the Transaction Code
        'Arguments      : Transaction Code
        'Return Value   : Transaction Description
        'Author         : Sujith Sharatchandran - 
        'Created        : 15/01/2004
        '*****************************************************************************************************
        Private Function fnTranDesc(ByVal intTranCode As Int16) As String

            Dim sdrTranDesc As SqlDataReader            'Create Instance SQL Data Reader
            Dim clsGeneric As New MaxPayroll.Generic       'Create Instance of Generic Class Object

            'Variable Declaration
            Dim strTranDesc As String = "", strSQL As String

            Try

                'Execute Stored Procedure
                strSQL = "Exec pg_GetTranDesc " & intTranCode

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Command Object to Execute SQL Data Reader
                Dim cmdTranDesc As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                sdrTranDesc = cmdTranDesc.ExecuteReader(CommandBehavior.CloseConnection)

                If sdrTranDesc.HasRows Then
                    sdrTranDesc.Read()
                    strTranDesc = sdrTranDesc("TDescription")
                    sdrTranDesc.Close()
                Else
                    sdrTranDesc.Close()
                End If

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()



            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnTranDesc", Err.Number, Err.Description)

            Finally

                'Destroy Instance of SQL Data Reader
                sdrTranDesc = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try
            'Return Transaction Description
            Return strTranDesc
        End Function

#End Region

#Region "Update Payroll File Status"

        '********************************************************************************************************
        'Function Name  : fnUpdateSubmission
        'Purpose        : To Update the Status of the Submitted File Which Has been Returned From Payment Server
        'Arguments      : File Name,Organisation Id,Remarks,Status
        'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
        'Created        : 15/01/2004
        '********************************************************************************************************
        Private Function fnUpdateSubmission(ByVal strFileName As String, ByVal strRemarks As String, ByVal intProcessed As Int16) As Boolean

            Dim clsGeneric As New MaxPayroll.Generic       'Create Instance of Generic Class Object
            Dim clsCommon As New MaxPayroll.clsCommon      'Create Instance of clsUpload Class Object

            'Variable Declaration
            Dim strSQL As String, lngOrgId As Long

            Try

                'Remove Path From File Name
                strFileName = clsCommon.fncFileName(strFileName, False)

                'Get Organisation Id From File Name
                lngOrgId = IIf(IsNumeric(Left(strFileName, 6)), Left(strFileName, 6), 0)

                'Update Statement
                strSQL = "Exec pg_UpdateSubmission 'Update'," & lngOrgId & ",'" & strFileName & "'," & intProcessed & ",'" & strRemarks & "'"

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                Dim cmdRemarks As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                cmdRemarks.ExecuteNonQuery()

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                cmdRemarks = Nothing

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, 0, "fnUpdateSubmission", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Field Description"

        '********************************************************************************************************
        'Function Name  : fnFieldDesc
        'Purpose        : To Get the Field Description and assign to Field Constants
        'Arguments      : N/A
        'Return Value   : Field Description
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/01/2004
        '********************************************************************************************************
        Public Sub fnFieldDesc(ByVal strOption As String, ByRef strDescription As String, ByRef intStartPos As Int16, ByRef intEndPos As Int16)

            'Create Instance of SQL Data Reader
            Dim sdrField As SqlDataReader

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String

            Try

                'SQL Statement
                strSQL = "Exec pg_FieldDesc '" & strOption & "'"

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Reader using SQL Command
                Dim cmdDesc As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                sdrField = cmdDesc.ExecuteReader(CommandBehavior.CloseConnection)

                'Get Field Description for Requested Predefined Option - Start
                If sdrField.HasRows() Then
                    sdrField.Read()
                    strDescription = sdrField("FDesc")
                    intStartPos = sdrField("SPos")
                    intEndPos = sdrField("EPos")
                    sdrField.Close()
                Else
                    sdrField.Close()
                End If
                'Get Field Description for Requested Predefined Option - Stop

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of SQL Command Object
                cmdDesc = Nothing

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fnFieldDesc", Err.Number, Err.Description)

            Finally

                'Destroy Instance of SQL Data Reader
                sdrField = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Get File Id"

        '********************************************************************************************************
        'Function Name  : fnFileId
        'Purpose        : To Get the File Id for the given File Name
        'Arguments      : File Name, Organisation Id
        'Return Value   : File Id
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/01/2004
        '********************************************************************************************************
        Private Function fnFileId(ByVal strFileName As String, ByVal lngOrgId As Long) As Long

            'Create Instance of SQL Data reader
            Dim sdrFileId As SqlDataReader

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim strSQL As String, lngFileId As Long

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec pg_UpdateSubmission 'File Id'," & lngOrgId & ",'" & strFileName & "',0,''"

                'Get File Id - Start
                Dim cmdFileId As New SqlCommand(strSQL, clsGeneric.SQLConnection)
                sdrFileId = cmdFileId.ExecuteReader(CommandBehavior.CloseConnection)

                If sdrFileId.HasRows Then
                    sdrFileId.Read()
                    lngFileId = sdrFileId("FID")
                    sdrFileId.Close()
                Else
                    lngFileId = 0
                    sdrFileId.Close()
                End If
                'Get File Id - Stop

                'Destroy Instance of SQL Command Object
                cmdFileId = Nothing

                Return lngFileId

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, 0, "fnFileId", Err.Number, Err.Description)

                Return 0

            Finally

                'Destroy Instance of SQL Data Reader
                sdrFileId = Nothing

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
