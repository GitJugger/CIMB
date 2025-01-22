Imports Microsoft.VisualBasic
Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports Microsoft.ApplicationBlocks.Data
'Imports System.Net.Mail
Imports System.Web.mail

Namespace MaxPayroll


    Public Class clsEmail
        Private _Helper As New Helper
#Region "Email upon Approve file"
        Public Function NotifyBankDownloader(ByVal sFileName As String, ByVal sSubFileName As String, ByVal sFileType As String) As Boolean
            Dim bRetVal As Boolean = False
            Dim strSQL As String = "Select User_Email, User_Name from tcor_userdetails WHERE User_Flag = 'BD' AND User_Status = 'A' AND User_Approved = 2 AND User_Email <> ''"
            Dim oGeneric As New MaxPayroll.Generic
            Dim drInfo As SqlDataReader
            Dim sMailBody As String = ""
            Dim sMailSubject As String = ""
            Dim sEmailTo As String = ""
            Try
                drInfo = SqlHelper.ExecuteReader(sSQLConnection, CommandType.Text, strSQL)
                While drInfo.Read

                    sEmailTo += CStr(drInfo("User_Email") & "") & " ,"

                End While
                sEmailTo = sEmailTo.Substring(0, sEmailTo.Length - 2)
                sMailSubject = "Attention : New " & sFileType & " approved file submitted"
                sMailBody = "<p style=""font-family:Arial;"">"
                sMailBody += "Dear " & gc_UT_BankDownloaderDesc & "(s)," & gc_BR & gc_BR
                sMailBody += "Please be informed that, the file [" & sFileName & "]"
                'The below block of code is to display the sub file (if any and for CPS only).  Currently not functionable.
                If sFileType = _Helper.CPS_Name AndAlso sSubFileName <> "" Then
                    sMailBody += ", [" & sSubFileName & "]"
                End If
                'The above block of code is to display the sub file (if any and for CPS only).  Currently not functionable.

                sMailBody += " has been submitted and awaiting for your further action." & gc_BR & gc_BR
                sMailBody += "Auto-generated message from CIMB Gateway."
                sMailBody += "</p>"

                If SendEmail(sEmailTo, sMailSubject, sMailBody) = False Then
                    oGeneric.ErrorLog("clsEmail - NotifyBankDownloader", 0, String.Format("Send Mail to {0} has failed for submitted file: " & sFileName, CStr(drInfo("Email") & "")))
                End If
            Catch ex As Exception

            End Try


            Return bRetVal

        End Function
#End Region


#Region "EMAIL Component"



        Public Function SendEmail(ByVal sEmailTo As String, ByVal sEmailSubject As String, ByVal sEmailBody As String) As Boolean
            Dim bRetVal As Boolean
            Dim clsEncrypt As New Encryption
            Dim sEmailFrom As String
            Dim sEmailPassword As String
            Dim message As New MailMessage


            'Dim smtp As New SmtpClient
            Try

                sEmailFrom = clsEncrypt.fnSQLCrypt(ConfigurationManager.AppSettings("SystemAdminEmailAddress"))
                message.BodyFormat = MailFormat.Html
                message.To = Trim(sEmailTo)
                message.From = sEmailFrom
                message.Priority = MailPriority.High
                message.Subject = sEmailSubject

                message.Body = sEmailBody

                'On Error Resume Next
                SmtpMail.SmtpServer = clsEncrypt.fnSQLCrypt(ConfigurationManager.AppSettings("SystemAdminEmailSMTPHost"))
                SmtpMail.Send(message)

                'sEmailFrom = clsEncrypt.fnSQLCrypt(ConfigurationManager.AppSettings("SystemAdminEmailAddress"))
                'sEmailPassword = clsEncrypt.fnSQLCrypt(ConfigurationManager.AppSettings("SystemAdminEmailPassword"))
                ' Dim mm As New MailMessage(sEmailFrom, sEmailTo)
                'mm.Bcc.Add(New MailAddress(sEmailFrom))
                'mm.Subject = sEmailSubject
                'mm.Body = sEmailBody
                'mm.IsBodyHtml = True
                'mm.Priority = MailPriority.High

                'mm.Headers.Add("Disposition-Notification-To", "<" & sEmailFrom & ">")
                'smtp.Host = clsEncrypt.fnSQLCrypt(ConfigurationManager.AppSettings("SystemAdminEmailSMTPHost"))
                'smtp.Credentials = New System.Net.NetworkCredential(sEmailFrom, sEmailPassword)
                'End If
                'Execute Email

                'smtp.Send(mm)
                bRetVal = True
            Catch ex As Exception
                Dim clsGeneric As New Generic
                clsGeneric.ErrorLog("clsEmail - SendEmail", Err.Number, ex.Message)
                clsGeneric = Nothing
                bRetVal = False
            End Try
            Return bRetVal
        End Function
#End Region

    End Class
End Namespace