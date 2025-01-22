Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Collections.Generic
Imports microsoft.ApplicationBlocks.Data
Imports System.Data
Imports System.Data.SqlClient

Namespace MaxPayroll
    Public Class clsMandateUpload
        Inherits clsUpload

#Region "Global Variables "

        Private Const gc_Frequency_Unlimited As String = "00"
        Private Const gc_Frequency_Monthly As String = "01"
        Private Const gc_Frequency_Quarterly As String = "02"
        Private Const gc_Frequency_HalfYearly As String = "03"
        Private Const gc_Frequency_Yearly As String = "04"

        'Instaces declaration - Start
        Private _Helper As New Helper
        Private _Maxdatabase As New MaxModule.DataBase()
        Private MiddlewareHelper As New MaxMiddleware.Helper()
        'Instaces declaration - Stop

#End Region

#Region "Check Duplicate "

        Private Function CheckDuplicate(ByVal oItem As clsMandates, ByVal oItems As List(Of clsMandates)) As Boolean
            Dim bRetVal As Boolean = False
            Dim Temp As clsMandates
            Try
                If oItems.Count = 0 Then
                    bRetVal = False
                Else
                    For Each Temp In oItems
                        If oItem.paramRefNo = Temp.paramRefNo AndAlso oItem.paramBankOrgCode = Temp.paramBankOrgCode AndAlso oItem.paramAccNo = Temp.paramAccNo Then
                            bRetVal = True
                            Exit For
                        End If
                    Next
                End If
            Catch ex As Exception
                bRetVal = True
            End Try

            Return bRetVal
        End Function

#End Region

#Region "Upload "

        Public Function Upload(ByVal lngOrgId As Long, ByVal lngGroupId As Long, ByVal lngUserId As Long, _
            ByVal sOriginalFileName As String, ByVal sFileName As String, ByRef sMsg As String) As Boolean

            'Variable Declaration - Start
            Dim oItems As New List(Of clsMandates)
            Dim flReader As StreamReader, sReadLine As String = ""
            Dim oItem As New clsMandates, bRetVal As Boolean = True
            Dim iLineCount As Integer = 0, iTotalLineCount As Integer = 0
            Dim strSQL As String = "", sTemp As String = "", sBody As String = ""
            Dim oCommon As New clsCommon, lngFileID As Long = 0, sSubject As String = ""
            Dim SQLStatementArr() As String = Nothing, RowLineLimit As Short = 0
            Dim CatchMessage As String = Nothing, LineIndex As Integer = 1, AssignCount As Short = 0
            'Variable Declaration - Stop

            Try

                ' If File Exists - Start
                If System.IO.File.Exists(sFileName) Then

                    iTotalLineCount = fncLineCount(sFileName)

                    flReader = New StreamReader(sFileName, System.Text.Encoding.Default)
                    sFileName = sFileName.Substring(sFileName.LastIndexOf("\") + 1)

                    sOriginalFileName = sOriginalFileName.Substring(sOriginalFileName.LastIndexOf("\") + 1)

                    lngFileID = oCommon.fncFileDetails("ADD", 0, "Mandate File", sOriginalFileName, sFileName, lngOrgId, _
                         lngUserId, DateTime.Now, "", 0, "", "0", "0", "", lngGroupId, 0, 0, "", 0, 0, iTotalLineCount, 0, iTotalLineCount)

                    'Assign the Size of the string array
                    ReDim Preserve SQLStatementArr(_Helper.GetArrayCount(iTotalLineCount))

                    'Loop through the file to perform validations and Insert/Update Script compilation - Start
                    While flReader.Peek <> -1
                        iLineCount += 1
                        sReadLine = flReader.ReadLine()
                        oItem = New clsMandates
                        If Len(sReadLine) >= 115 Then


                            oItem.paramFileID = lngFileID
                            oItem.paramAccNo = sReadLine.Substring(2, 14).Trim
                            oItem.paramBankOrgCode = sReadLine.Substring(16, 4).Trim
                            oItem.paramRefNo = sReadLine.Substring(68, 30).Trim
                            oItem.paramIC = sReadLine.Substring(98, 12).Trim

                            If CheckDuplicate(oItem, oItems) Then
                                sMsg += "Line " & iLineCount.ToString & " has found duplicated." & gc_BR
                                bRetVal = False
                            Else
                                oItems.Add(oItem)
                            End If
                            oItem.paramCustomerName = sReadLine.Substring(20, 20).Trim
                            sTemp = (sReadLine.Substring(40, 14).Trim)
                            If IsNumeric(sTemp) Then
                                oItem.paramLimitAmount = CDec(sTemp / 100)
                            ElseIf oItem.paramLimitAmount <= 0 Then 'add by hafeez(12/11/2008) to remove duplicate error message
                                sMsg += "Line " & iLineCount.ToString & " contains an invalid Limit Amount." & gc_BR
                            Else
                                sMsg += "Line " & iLineCount.ToString & " contains an invalid Limit Amount." & gc_BR
                            End If

                            oItem.paramFrequency = sReadLine.Substring(110, 2).Trim

                            If oItem.paramFrequency.Trim = "" Then
                                oItem.paramFrequency = "00"
                            End If

                            sTemp = sReadLine.Substring(112, 3).Trim
                            If IsNumeric(sTemp) Then
                                oItem.paramFrequencyLimit = sReadLine.Substring(112, 3).Trim
                            Else
                                If sTemp = "" Then
                                    oItem.paramFrequencyLimit = 0
                                Else
                                    sMsg += "Line " & iLineCount.ToString & " contains an invalid Frequency Limit." & gc_BR
                                End If
                            End If

                            oItem.paramOrgID = lngOrgId
                            oItem.paramDoneBy = lngUserId

                            If Len(oItem.paramBankOrgCode) <> 4 Then
                                sMsg += "Line " & iLineCount.ToString & " contains a Bank Organization Code which less than 4 digits." & gc_BR
                                bRetVal = False
                            End If


                            '**New Validation added to validate Mandate Bank OrgCode- by Naresh - 26-09-2011
                            'Validate Mandate Bankorgcode - Start
                            If Not (MaxGeneric.clsGeneric.NullToShort(_Maxdatabase.GetValue(_Helper.GetSQLCommon & MaxGeneric.clsGeneric.AddQuotes _
                                       (Helper.CommonRequest.Mandate_BankOrgCode.ToString()) & MaxGeneric.clsGeneric.AddComma & MaxGeneric.clsGeneric. _
                                            AddQuotes(oItem.paramBankOrgCode) & MaxGeneric.clsGeneric.AddComma & oItem.paramOrgID))) > 0 Then

                                sMsg += "Line " & iLineCount.ToString & " Contains Invalid Bank Organization Code." & gc_BR
                                bRetVal = False
                            End If
                            'Validate Mandate Bankorgcode - Stop

                            If Len(oItem.paramRefNo) = 0 Then
                                sMsg += "Line " & iLineCount.ToString & " contains a blank Reference Number." & gc_BR
                                bRetVal = False
                            End If

                            If oItem.paramFrequency <> gc_Frequency_Unlimited AndAlso oItem.paramFrequency <> gc_Frequency_Monthly _
                                AndAlso oItem.paramFrequency <> gc_Frequency_Quarterly AndAlso oItem.paramFrequency <> gc_Frequency_HalfYearly _
                                AndAlso oItem.paramFrequency <> gc_Frequency_Yearly Then
                                sMsg += "Line " & iLineCount.ToString & " contains an invalid Frequency Code." & gc_BR
                                bRetVal = False
                            Else
                                Select Case oItem.paramFrequency
                                    Case gc_Frequency_Unlimited
                                        oItem.paramFrequency = clsCommon.fncGetPrefix(enmFrequency.UN_Unlimited)
                                    Case gc_Frequency_Monthly
                                        oItem.paramFrequency = clsCommon.fncGetPrefix(enmFrequency.MY_Monthly)
                                    Case gc_Frequency_Quarterly
                                        oItem.paramFrequency = clsCommon.fncGetPrefix(enmFrequency.QY_Quarterly)
                                    Case gc_Frequency_HalfYearly
                                        oItem.paramFrequency = clsCommon.fncGetPrefix(enmFrequency.HY_Half_Yearly)
                                    Case gc_Frequency_Yearly
                                        oItem.paramFrequency = clsCommon.fncGetPrefix(enmFrequency.YY_Yearly)
                                End Select
                            End If
                            If oItem.paramFrequency = clsCommon.fncGetPrefix(enmFrequency.UN_Unlimited) AndAlso oItem.paramFrequencyLimit <> 0 Then
                                sMsg += "Line " & iLineCount.ToString & " contains an invalid Frequency Limit." & gc_BR
                                bRetVal = False
                            End If
                        Else
                            sMsg += "Line " & iLineCount.ToString & " has a record with insufficient length." & gc_BR
                            bRetVal = False
                        End If

                        If oItem.IsDuplicateTemp(oItem.paramOrgID, oItem.paramRefNo, oItem.paramAccNo, oItem.paramBankOrgCode, oItem.paramRecID) Then
                            sMsg += "Line " & iLineCount.ToString & " has a unapproved record." & gc_BR
                            bRetVal = False
                        End If

                        'hafeez start
                        If oItem.IsActive(oItem.paramOrgID, oItem.paramRefNo, oItem.paramAccNo, oItem.paramBankOrgCode, oItem.paramRecID) Then
                            sMsg += "Line " & iLineCount.ToString & " has a inactive record." & gc_BR
                            bRetVal = False
                        End If
                        'hafeez end

                        ' Build SQL Statemement for Insert - Start
                        If bRetVal = True Then
                            If oItem.IsDuplicate(oItem.paramOrgID, oItem.paramRefNo, oItem.paramAccNo, oItem.paramBankOrgCode, oItem.paramRecID) Then
                                strSQL += oItem.GetInsertTempString(oItem, True)
                            Else
                                strSQL += oItem.GetInsertTempString(oItem, False)
                            End If

                            'Incriment Row Counter -- Added on 10/02/2011 by Teja
                            RowLineLimit = RowLineLimit + 1

                        Else
                            strSQL = ""
                        End If
                        ' Build SQL Statemement for Insert - Stop

                        '-- Added on 10/02/2011 by Teja - Start
                        'If the Rows are Grater or Equal to Number Of max Per Batch - Start
                        If RowLineLimit >= _Helper.LineLimit Then

                            If Not MaxMiddleware.Helper.IsBlank(strSQL) Then

                                'Assign the string to String Array - Start
                                SQLStatementArr(AssignCount) = strSQL
                                AssignCount += 1
                                'Assign the string to String Array - Stop

                                'Clear Values - Start
                                strSQL = String.Empty
                                RowLineLimit = 0
                                'Clear Values - Stop

                            End If

                        End If
                        'If the Rows are Grater or Equal to Number Of max Per Batch - Stop
                        '-- Added on 10/02/2011 by Teja - Stop

                    End While
                    'Loop through the file to perform validations and Insert/Update Script compilation - End

                    '-- Added on 10/02/2011 by Teja - Start
                    'If SQL statement Is not Blank For Last Round Of loops - Start
                    If Not MaxMiddleware.Helper.IsBlank(strSQL) Then

                        'Assign the string to String Array - Start
                        SQLStatementArr(AssignCount) = strSQL
                        'Assign the string to String Array - Stop

                        'Clear Values
                        strSQL = String.Empty
                        RowLineLimit = 0

                    End If
                    'If SQL statement Is not Blank For Last Round Of loops - Stop

                    'Execute the SQL statements - Start
                    If bRetVal AndAlso lngFileID > 0 AndAlso Not MaxMiddleware.Helper.IsBlank(SQLStatementArr) Then

                        Call MaxMiddleware.PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, _
                            CatchMessage, SQLStatementArr)

                        'If has error in SQL Execution - Start
                        If Not MaxMiddleware.Helper.IsBlank(CatchMessage) Then
                            sMsg += "Database processing encountered error."
                            'Delete the Mandate transactions
                            oCommon.prcDeleteTrans(lngFileID, lngOrgId, lngUserId, "tCor_MandatesDetails")
                            bRetVal = False
                        Else
                            'Update tPgt_FileDetails Table - Start
                            If oItem.UploadComplate(lngFileID, iLineCount) Then

                                'Update Work flow Table - Start
                                Call oCommon.prcWorkFlow(lngFileID, 0, lngOrgId, lngUserId, _
                                    "U", "N", "", "", 0, lngGroupId, "Y")
                                'Update Work flow Table - Stop

                                sSubject = "Mandate File Uploaded - " & sOriginalFileName

                                'Build Body - Start
                                sBody = "The Mandate File has been successfully uploaded on " & _
                                    Format(Today, "dd/MM/yyyy") & " at " & TimeOfDay
                                'Build Body - Stop

                                'Send Mails To Group Reviewers/Authorizers - Start
                                Call oCommon.prcSendMails("CUSTOMER", lngOrgId, lngUserId, lngGroupId, _
                                    sSubject, sBody)
                                'Send Mails To Group Reviewers/Authorizers - Stop
                            Else
                                'Delete the Mandate transactions
                                oCommon.prcDeleteTrans(lngFileID, lngOrgId, lngUserId, "tCor_MandatesDetails")
                            End If
                            'Update tPgt_FileDetails Table - Stop

                        End If
                        'If has error in SQL Execution - Stop

                    End If
                    'Execute the SQL statements - Stop

                    '-- Added on 10/02/2011 by Teja - Stop
                Else
                    sMsg += "Uploading process on file [" & sFileName & "] has encountered error."
                    bRetVal = False
                End If
                ' If File Exists - Stop

            Catch ex As Exception
                Dim oGeneric As New Generic
                oGeneric.ErrorLog("clsMandateUpload.Upload", Err.Number, ex.Message)
                oGeneric = Nothing
                bRetVal = False
                If lngFileID > 0 Then
                    oCommon.prcDeleteTrans(lngFileID, lngOrgId, lngUserId, "tCor_MandatesDetails")
                End If

            Finally
                If Not flReader Is Nothing Then
                    flReader.Close()
                    flReader.Dispose()
                End If
            End Try

            If bRetVal = False Then
                oCommon.prcDeleteTrans(lngFileID, lngOrgId, lngUserId, "tCor_MandatesDetails")
            End If

            Return bRetVal

        End Function

#End Region

    End Class

End Namespace

