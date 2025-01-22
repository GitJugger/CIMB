Imports Microsoft.VisualBasic
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Namespace MaxPayroll


    Public Class clsBankDownloader
        Private _Helper As New Helper
        Public Function fncGetApprovedFileList(ByVal bIsH2H As Boolean, ByVal bNewFile As Boolean, ByVal sFileType As String) As DataSet
            Dim strSQL As String
            strSQL = "SELECT FileId, FileType, FileName, SubmissionDate, PaymentDate, 'Main' FileAttribute from " & IIf(bIsH2H, "tH2H_FileDetails", "tPGT_FileDetails") & " WHERE(IsDownload = " & IIf(bNewFile, "0", "1") & " And Processed = 5) and filetype=" & clsDB.SQLStr(sFileType)
            If sFileType = _Helper.CPS_Name AndAlso bIsH2H = False Then
                strSQL += "UNION SELECT FileId, FileType, SubFileName, SubmissionDate, PaymentDate, 'Sub' FileAttribute from " & IIf(bIsH2H, "tH2H_FileDetails", "tPGT_FileDetails") & " WHERE SubFileName <> '' And(IsSubFileDownload = " & IIf(bNewFile, "0", "1") & " And Processed = 5) and filetype=" & clsDB.SQLStr(sFileType)
            End If
            strSQL += " Order by PaymentDate,FileName Desc"
            Dim ds As New DataSet
            Dim clsGeneric As New Generic

            Try
                ds = SqlHelper.ExecuteDataset(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                clsGeneric.ErrorLog("clsBankDownloader-fncGetApprovedFileList", Err.Number, ex.Message)
            Finally
                clsGeneric = Nothing
            End Try
            Return ds
        End Function
        Public Function fncUpdateDownloadedFile(ByVal bIsH2H As Boolean, ByVal sFileID As Long, ByVal sFileAttribute As String) As Boolean
            Dim strSQL As String
            If sFileAttribute = "Main" Then
                strSQL = "Update " & IIf(bIsH2H, "tH2H_FileDetails", "tPGT_FileDetails") & " set IsDownload = 1 WHERE FileID = " & sFileID.ToString
            Else
                strSQL = "Update " & IIf(bIsH2H, "tH2H_FileDetails", "tPGT_FileDetails") & " set IsSubFileDownload = 1 WHERE FileID = " & sFileID.ToString
            End If

            Dim clsGeneric As New Generic
            Try
                SqlHelper.ExecuteNonQuery(clsGeneric.strSQLConnection, CommandType.Text, strSQL)
            Catch ex As Exception
                clsGeneric.ErrorLog("clsBankDownloader-fncUpdateDownloadedFile", Err.Number, ex.Message)
            Finally
                clsGeneric = Nothing
            End Try
        End Function
    End Class
End Namespace
