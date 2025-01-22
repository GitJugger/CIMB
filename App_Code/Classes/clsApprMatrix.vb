'****************************************************************************************************
'Class Name     : clsApprMatrix
'ProgId         : MaxPayroll.clsApprMatrix
'Purpose        : Approval Matrix Functions
'Author         : Siti Aishah\Sujith Sharatchandran - 
'Created        : 14/02/2005
'*****************************************************************************************************

Option Strict Off
Option Explicit On

Imports MaxPayroll.Generic
Imports MaxPayroll.Encryption
Imports System.Data.SqlClient
Imports MaxFTP.clsFTP
Imports Microsoft.ApplicationBlocks.Data
Imports System.Collections.Generic

Namespace MaxPayroll



    Public Class clsApprMatrix

#Region "Approval Matrix"

        '****************************************************************************************************
        'Procedure Name : prcApporvalMatrix()
        'Purpose        : Approval Matrix Insert/Update
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Public Function prcApprovalMatrix(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strOption As String, _
            ByVal lngApprId As Long, ByVal lngFromId As Long, ByVal lngToId As Long, ByVal lngModuleId As Long, _
                ByVal strModuleDesc As String, ByVal strSubject As String, ByVal strRemarks As String, _
                    ByVal intStatus As Int16, Optional ByVal strReason As String = "") As Int32

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdApprMatrix As New SqlCommand

            'variable declarations
            Dim intApprId As Int32

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Insert/Update in Database - START
                With cmdApprMatrix
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = Data.CommandType.StoredProcedure
                    .CommandText = "pg_ApprovalMatrix"
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_ApprId", lngApprId))
                    .Parameters.Add(New SqlParameter("@in_FromId", lngFromId))
                    .Parameters.Add(New SqlParameter("@in_ToId", lngToId))
                    .Parameters.Add(New SqlParameter("@in_ModuleId", lngModuleId))
                    .Parameters.Add(New SqlParameter("@in_ModuleDesc", strModuleDesc))
                    .Parameters.Add(New SqlParameter("@in_Subject", strSubject))
                    .Parameters.Add(New SqlParameter("@in_Remarks", strRemarks))
                    .Parameters.Add(New SqlParameter("@in_Status", intStatus))
                    .Parameters.Add(New SqlParameter("@in_Reason", strReason))
                    intApprId = .ExecuteScalar()
                End With
                'Insert/Update in Database - STOP

                Return intApprId

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcApporvalMatrix - clsApprMatrix", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdApprMatrix = Nothing

            End Try

        End Function

        Public Function prcApprovalMatrix(ByVal Trans As SqlTransaction, ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strOption As String, ByVal lngApprId As Long, ByVal lngFromId As Long, ByVal lngToId As Long, ByVal lngModuleId As Long, ByVal strModuleDesc As String, ByVal strSubject As String, ByVal strRemarks As String, ByVal intStatus As Int16, Optional ByVal strReason As String = "") As Int32

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'variable declarations
            Dim intApprId As Int32

            Dim params(9) As SqlParameter

            Try
                params(0) = New SqlParameter("@in_Option", strOption)
                params(1) = New SqlParameter("@in_ApprId", lngApprId)
                params(2) = New SqlParameter("@in_FromId", lngFromId)
                params(3) = New SqlParameter("@in_ToId", lngToId)
                params(4) = New SqlParameter("@in_ModuleId", lngModuleId)
                params(5) = New SqlParameter("@in_ModuleDesc", strModuleDesc)
                params(6) = New SqlParameter("@in_Subject", strSubject)
                params(7) = New SqlParameter("@in_Remarks", strRemarks)
                params(8) = New SqlParameter("@in_Status", intStatus)
                params(9) = New SqlParameter("@in_Reason", strReason)

                intApprId = SqlHelper.ExecuteScalar(Trans, CommandType.StoredProcedure, "pg_ApprovalMatrix", params)

                Return intApprId

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcApporvalMatrix - clsApprMatrix", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                'Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "List Apporval"

        '****************************************************************************************************
        'Function Name  : fncListMatrix()
        'Purpose        : List of Records for Approval
        'Arguments      : User Id, Organisation Id
        'Return Value   : Dataset
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Public Function fncListMatrix(ByVal strOption As String, ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                    ByVal strUserType As String) As System.Data.DataSet

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dsListMatrix As New System.Data.DataSet

            'Create Instance of SQL Data Adapter
            Dim sdaListMatrix As New SqlDataAdapter

            'Variable Declarations
            Dim strSQL As String

            Try
                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'SQL Statement
                strSQL = "Exec pg_GetApprMatrix '" & strOption & "'," & lngUserId & ",'" & strUserType & "'"

                'Execute SQL Data Adapter
                sdaListMatrix = New SqlDataAdapter(strSQL, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaListMatrix.Fill(dsListMatrix, "MATRIX")

                'Return Data Set
                Return dsListMatrix

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncListMatrix - clsApprMatrix", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Data Adapter
                sdaListMatrix = Nothing

                'Destroy Instance of Data Set
                dsListMatrix = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Update Status"

        Public Sub prcApprFileMandateTrans(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSubject As String, ByVal lngStatus As Long, ByVal lngTransId As Long, Optional ByVal intReqId As Int32 = 0)
            Dim oNewItems As New List(Of clsMandates)
            Dim oOldItem As New clsMandates
            Dim oNewItem As New clsMandates
            Dim clsCommon As New clsCommon
            Dim lngGroupId As Long = 0
            Try
                oNewItems = oNewItem.LoadFileDetailsCollection(lngTransId)
                For Each oNewItem In oNewItems
                    If oNewItem.paramToUpdate = False Then
                        If lngStatus = 2 Then
                            oNewItem.Insert(oNewItem)
                        End If

                    Else
                        oOldItem = oOldItem.Load(oNewItem.paramOrgID, oNewItem.paramRefNo, oNewItem.paramAccNo, oNewItem.paramBankOrgCode)
                        oNewItem.paramDoneBy = lngUserId
                        oNewItem.paramRecID = oOldItem.paramRecID
                        If lngStatus = 2 Then
                            oNewItem.Update(oNewItem, oOldItem)
                        End If
                        oNewItem.Approve(oNewItem)
                    End If
                Next
                If lngStatus = 2 Then
                    oNewItem.UpdateStatusByFileId(lngTransId, lngUserId, enmMandateStatus.Approve, True)
                    oNewItem.UpdateStatusByFileId(lngTransId, lngUserId, enmMandateStatus.Approve, False)
                Else
                    oNewItem.UpdateStatusByFileId(lngTransId, lngUserId, enmMandateStatus.Reject, True)
                    oNewItem.UpdateStatusByFileId(lngTransId, lngUserId, enmMandateStatus.Reject, False)
                End If
                lngGroupId = fncGetGroupID(lngTransId)
                Call clsCommon.prcSendMails("CUSTOMER", oNewItem.paramOrgID, lngUserId, lngGroupId, strSubject, strSubject & IIf(lngStatus = 2, " has been authorized by " & gc_UT_BankAuthDesc & ".", " has been rejected by " & gc_UT_BankAuthDesc & "."))

            Catch ex As Exception

            End Try

        End Sub

        Public Sub prcApprCreateMandateTrans(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSubject As String, ByVal lngStatus As Long, ByVal lngTransId As Long, Optional ByVal intReqId As Int32 = 0)
            Dim oNewItems As New List(Of clsMandates)
            Dim oOldItem As New clsMandates
            Dim oNewItem As New clsMandates
            Dim clsCommon As New clsCommon
            Dim lngGroupId As Long = 0
            Try
                If lngStatus = 2 Then
                    oNewItem.UpdateStatusById(lngTransId, lngUserId, enmMandateStatus.Approve)
                Else
                    oNewItem.UpdateStatusById(lngTransId, lngUserId, enmMandateStatus.Reject)
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Sub prcApprModifyMandateTrans(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strSubject As String, ByVal lngStatus As Long, ByVal lngTransId As Long, Optional ByVal intReqId As Int32 = 0)
            Dim oNewItems As New List(Of clsMandates)
            Dim oOldItem As New clsMandates
            Dim oNewItem As New clsMandates
            Dim clsCommon As New clsCommon
            Dim lngGroupId As Long = 0
            Try
                oNewItem = oNewItem.LoadTempById(lngTransId)
                oNewItem.paramDoneBy = lngUserId
                oOldItem = oOldItem.Load(oNewItem.paramOrgID, oNewItem.paramRefNo, oNewItem.paramAccNo, oNewItem.paramBankOrgCode)
                If lngStatus = 2 Then
                    oNewItem.Update(oNewItem, oOldItem)
                    oOldItem.UpdateStatusById(oOldItem.paramRecID, lngUserId, enmMandateStatus.Approve)
                    'hafeez - del if mandate duplicate in mandatefiledetails.
                    
                    'oOldItem.DeleteMandateFileDetails(oNewItem.paramOrgID, oNewItem.paramRefNo, oNewItem.paramAccNo, oNewItem.paramBankOrgCode)


                Else
                    oOldItem.UpdateStatusById(oOldItem.paramRecID, lngUserId, enmMandateStatus.Reject)
                End If
                'edit hafeez - 15/10/2008'
                'oNewItem.DeleteTemp(lngTransId)'
                'end hafeez'
            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Private Function fncGetGroupID(ByVal FileId As Long) As Long
            Dim strSQL As String = "SELECT top 1 Group_Id FROM tPgt_FileDetails WHERE FileId = " & FileId.ToString
            Dim lngRetVal As Long = 0
            Try
                lngRetVal = SqlHelper.ExecuteScalar(Generic.sSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                Throw ex
            End Try
            Return lngRetVal
        End Function



        '****************************************************************************************************
        'Procedure      : prcApprTrans()
        'Purpose        : To Update the Status for all pending Requisitions
        'Arguments      : Module,Status,Transaction Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 24/02/2005
        '*****************************************************************************************************
        Public Sub prcApprTrans(ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                ByVal strModule As String, ByVal lngStatus As Long, ByVal lngTransId As Long, _
                    Optional ByVal intReqId As Int32 = 0)

            'Create Instance of Generi Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of SQL Command Object
            Dim cmdApprTrans As New SqlCommand

            Try
                'Initialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdApprTrans
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = Data.CommandType.StoredProcedure
                    .CommandText = "pg_ApprTrans"
                    .Parameters.Add(New SqlParameter("@in_Module", strModule))
                    .Parameters.Add(New SqlParameter("@in_TransId", lngTransId))
                    .Parameters.Add(New SqlParameter("@in_Status", lngStatus))
                    .Parameters.Add(New SqlParameter("@in_ReqId", intReqId))
                    .Parameters.Add(New SqlParameter("@in_ApprId", lngUserId))
                    .ExecuteNonQuery()
                End With

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcApprTrans - clsApprMatrix", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of SQL Command Object
                cmdApprTrans = Nothing

            End Try

        End Sub

#End Region

#Region "Load Approval Matrix Message"

        '****************************************************************************************************
        'Procedure Name : fncLoadMessage()
        'Purpose        : Load Approval Matrix Message Contents
        'Arguments      : Approval Id
        'Return Value   : Data Set
        'Author         : Siti Aishah Sepawi - 
        'Created        : 03/02/2005
        '*****************************************************************************************************
        Public Function fncLoadMessage(ByVal lngMatrix As Long, ByVal lngOrgId As Long, ByVal lngUserId As Long) As System.Data.DataSet

            'Create Instance System Date Set Object
            Dim dsMatrix As New System.Data.DataSet

            'Create Instance of SQL Data Adaptor Object
            Dim sdaMatrix As New SqlDataAdapter

            'Create Instance Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Initialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Execute SQL Data Adaptor
                sdaMatrix = New SqlDataAdapter("Exec pg_DisplayMessage " & lngMatrix, clsGeneric.SQLConnection)

                'Fill Data Set
                sdaMatrix.Fill(dsMatrix, "MATRIX")

                Return dsMatrix

            Catch

                'Log Error 
                clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncLoadMessage - clsApprMatrix", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Generic Class Object
                clsGeneric = Nothing

                'Destroy SQL Data Adaptor
                sdaMatrix = Nothing

                'Destroy Data Set
                dsMatrix = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Block/Stop File"

        '****************************************************************************************************
        'Procedure Name : fncBlock()
        'Purpose        : To Block/Stop File
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 27/02/2005
        '*****************************************************************************************************
        Public Function fncBlock(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileName As String, _
            ByVal strFileType As String, ByVal intStatus As Int16) As Boolean

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strFolder As String = "", IsDelete As Boolean

            Try

                'Get File Type and FTP Folder - Start
                If intStatus = 5 Then
                    If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                        strFolder = "IN\"
                    ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                        strFolder = "EPFIN\"
                        strFileName = Replace(strFileName, ".", "_") & ".cry"
                    End If
                ElseIf intStatus = 0 Then
                    strFolder = "CREATED\"
                    If strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                        strFileName = Replace(strFileName, ".", "_") & ".cry"
                    End If
                End If
                'Get File Type and FTP Folder - Stop

                'Delete file
                IsDelete = clsCommon.fncDelFile(lngOrgId, lngUserId, strFolder & strFileName)

                Return IsDelete

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncBlock - clsApprMatrix", Err.Number, Err.Description)

                Return False

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of common Class Object
                clsCommon = Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace
