#Region "Name Spaces "

Option Strict On
Option Explicit On
Imports MaxGeneric
Imports System.Configuration.ConfigurationManager

#End Region

Public Class Message

#Region "Generic Messages "

    Public ReadOnly Property MsgThreadAborted() As String
        Get
            Return "Thread was being aborted."
        End Get
    End Property

    Public ReadOnly Property MsgEmpty() As String
        Get
            Return String.Empty
        End Get
    End Property

#End Region

#Region "Display Messages "

    Public ReadOnly Property MsgNoBankAccountOrgCodes() As String
        Get
            Return "No Org Codes ara available for Selected Bank Account."
        End Get
    End Property

    Public ReadOnly Property MsgAuthCodeLock(ByVal UserType As String) As String
        Get
            Return "Your Validation Code has been locked due to invalid 3 attempts. Please contact your " & UserType & "."
        End Get
    End Property

    Public ReadOnly Property MsgNoBusinessUsers(ByVal UserType As String, _
          ByVal ReviewerDesc As String, ByVal AuthorizerDesc As String) As String
        Get
            Return "Required Number(s) of " & ReviewerDesc & "s/" & AuthorizerDesc _
                        & "s are not Created or Active. Please contact your " & UserType & "."
        End Get
    End Property

    Public ReadOnly Property MsgNoFileFormats() As String
        Get
            Return "No File Formats Available."
        End Get
    End Property

    Public ReadOnly Property MsgNoBankAccounts() As String
        Get
            Return "No Bank Accounts Available."
        End Get
    End Property

    Public ReadOnly Property MsgConfirmUpload() As String
        Get
            Return "Please Enter your Validation Code & Confirm File Upload."
        End Get
    End Property

    Public ReadOnly Property MsgSamePayDate() As String
        Get
            Return "Please Note: There has already a file been Uploaded with the same Payment Date." & _
                "Please Enter your Validation Code and Confirm, if you wish to proceed."
        End Get
    End Property

    Public ReadOnly Property MsgFileUploaded() As String
        Get
            Return "File Uploaded Successfully."
        End Get
    End Property

#End Region

#Region "Alert Messages "

    Public ReadOnly Property MsgUploadAlert() As String
        Get
            Return "Please wait. We are processing your request. If you do not receive confirmation, please check your mail box or File Status Report for confirmation. Click OK to proceed."
        End Get
    End Property

    Public ReadOnly Property MsgAuthCodeLocked(ByVal SystemAdmin As String) As String
        Get
            Return "Your account has been locked due to invalid attempts. Please contact your " & SystemAdmin & "."
        End Get
    End Property

    Public ReadOnly Property MsgAuthCodeInvalid() As String
        Get
            Return "Validation code is invalid. Please enter a valid Validation Code."
        End Get
    End Property

#End Region

#Region "Validation Messages "

    Public Function MsgCutoffTime(ByVal ServiceType As String, _
           ByVal UserRole As Short, ByVal CutoffTime As String) As String

        Select Case UserRole

            Case CShort(EnumHelp.MpgUserRoles.Uploader)

                Return "The " & ServiceType & " with the Selected Payment Date " & _
                    "cannot be Uploaded after the Cutoff Time (" & CutoffTime & "). Please Select a later Payment Date."

            Case Else

                Return String.Empty

        End Select

    End Function

    Public ReadOnly Property MsgInvalidFile() As String
        Get
            Return "File not found or file content is emtpy. Please check your file."
        End Get
    End Property

    Public ReadOnly Property MsgInvalidExtension(ByVal ServiceType As String) As String
        Get
            Return "The " & ServiceType & " extension does not match with the Selected File Format."
        End Get
    End Property

    Public ReadOnly Property MsgFileUploaded(ByVal ServiceType As String) As String
        Get
            Return "The " & ServiceType & " has been previously uploaded. Please rename the file."
        End Get
    End Property

    Public ReadOnly Property MsgNoEncryptKey() As String
        Get
            Return "Unable to obtain Organization Encryption Key."
        End Get
    End Property

    Public ReadOnly Property MsgEncryptionFailed() As String
        Get
            Return "File decryption failed, please check your encryption key and try again."
        End Get
    End Property

#End Region

#Region "Mail Messages "

    Public ReadOnly Property MsgAuthCodeLockSubject(ByVal UserName As String, _
        ByVal UserRole As String) As String
        Get
            Return UserName & " (" & UserRole & ") Locked/Inactive."
        End Get
    End Property

    Public ReadOnly Property MsgAuthCodeLockBody(ByVal UserName As String, _
        ByVal UserRole As String) As String
        Get
            Return UserName & " (" & UserRole & ")" & " has been Locked/Inactive on " & Now() _
                & " due to Invalid Validation Code attempts. Please change the User Validation Code."
        End Get
    End Property

    Public ReadOnly Property MsgFileUploadSubject(ByVal ServiceType As String, _
        ByVal UploadFileName As String, ByVal TransactionDate As String) As String
        Get
            Return ServiceType & " Uploaded - " & UploadFileName & ", Upload Date: " & TransactionDate
        End Get
    End Property

    Public ReadOnly Property MsgFileUploadBody(ByVal ServiceType As String) As String
        Get
            Return "The " & ServiceType & " has been successfully uploaded on " & Format(Today, "dd/MM/yyyy") & " at " & TimeOfDay
        End Get
    End Property

#End Region

End Class
