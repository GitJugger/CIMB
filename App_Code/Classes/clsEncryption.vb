Imports Microsoft.VisualBasic
Imports System.IO
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsUpload
Imports System.Data.SqlClient
Imports MaxMiddleware

Public Class clsEncryption
    Private _Helper As New Helper


#Region "Config File Settings"
    Public ReadOnly Property EncryptFileExtension() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("CIMB_Crypt_Extension")
        End Get
    End Property

    Public ReadOnly Property AppEncryptFile() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("CCryptExePath")
        End Get
    End Property

    Public ReadOnly Property CIMB_Crypt_Delim() As String
        Get
            Return System.Configuration.ConfigurationManager.AppSettings("CIMB_Crypt_Delim")
        End Get
    End Property
#End Region

#Region "Decrypt Start"

    Public Function DecryptMyFiles(ByVal strFileName As String, ByVal strCCryptKey As String) As Boolean

        Try



            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon
            Dim CommandArgs As String = ""
            Dim ShellResult As Boolean = False
            Dim ShellOutput As String = ""
            Dim ErrorDetails As String = ""


            CommandArgs = " -f" & CIMB_Crypt_Delim & "-d" & CIMB_Crypt_Delim & "-K" & CIMB_Crypt_Delim & strCCryptKey & CIMB_Crypt_Delim & strFileName

            ShellResult = MaxGeneric.clsGeneric.ShellExecute(False, AppEncryptFile, CommandArgs, "", ShellOutput, ErrorDetails)

            If ErrorDetails = "" Then
                Return True
            End If


        Catch ex As Exception

        End Try

    End Function
#End Region

End Class
