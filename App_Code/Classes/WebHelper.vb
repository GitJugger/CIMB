#Region "Name Spaces "

Option Strict On
Option Explicit On
Imports MaxGeneric
Imports System.Configuration.ConfigurationManager

#End Region

Public Class WebHelper

#Region "Global Declarations "

    'Create Instances
    Private _Message As New Message

#End Region

#Region "Constants "

    Public Const No As String = "N"
    Public Const Yes As String = "Y"
    Public Const PM As String = "PM"
    Public Const Reviewer As String = "REVIEW"
    Public Const UploadFolder As String = "UPLOAD"
    Public Const CreateFolder As String = "CREATE"
    Public Const Authorizer As String = "AUTHORIZE"
    Public Const AuthStatus As String = "Auth Status"
    Public Const ApprovedFolder As String = "APPROVED"
    Public Const ThreadAborted As String = "Thread was being aborted."

#End Region

#Region "Session Constants "

    Public Const AuthLock As String = "AUTH_LOCK"

#End Region

#Region "Enumerators "

    Public Enum UserLockType
        Password = 1
        AuthorizationCode = 2
    End Enum

#End Region

#Region "Message Properties "

    Public Shared ReadOnly Property AppErrorMessage() As String
        Get
            Return clsGeneric.NullToString(AppSettings("APP_ERR_MSG"))
        End Get
    End Property

#End Region

#Region "Config Properties "

    Public ReadOnly Property EncryptFileExtension() As String
        Get
            Return AppSettings("CIMB_Crypt_Extension")
        End Get
    End Property

    Public ReadOnly Property RootPath() As String
        Get
            Return AppSettings("PATH")
        End Get
    End Property

#End Region

#Region "Query String Constants "

    Public Const ServiceType As String = "SERVTYPE"

#End Region

#Region "Session Properties "

    Public Property AuthLockCount() As Integer
        Get
            Return clsGeneric.NullToShort(HttpContext.Current.Session(AuthLock))
        End Get
        Set(value As Integer)
            HttpContext.Current.Session(AuthLock) = value
        End Set
    End Property

#End Region

#Region "Service Type Properties "

    Public Function DirectDebit_Name() As String
        Dim Name As String = Nothing
        Name = "Direct Debit"
        Return Name
    End Function

    Public Function HybridDirectDebit_Name() As String
        Dim Name As String = Nothing
        Name = "Hybrid Direct Debit"
        Return Name
    End Function

#End Region

#Region "Column Properties "

    Public ReadOnly Property ColOrgCode() As String
        Get
            Return "Org_Code"
        End Get
    End Property

    Public ReadOnly Property ColServiceType() As String
        Get
            Return "Service_Type"
        End Get
    End Property

    Public ReadOnly Property ColServiceId() As String
        Get
            Return "Service_Id"
        End Get
    End Property

    Public ReadOnly Property ColFileTypeId() As String
        Get
            Return "FileType_Id"
        End Get
    End Property

    Public ReadOnly Property ColFormatId() As String
        Get
            Return "Format_Id"
        End Get
    End Property

    Public ReadOnly Property ColFormatName() As String
        Get
            Return "Format_Name"
        End Get
    End Property

    Public ReadOnly Property ColAccountId() As String
        Get
            Return "Account_Id"
        End Get
    End Property

    Public ReadOnly Property ColAccountNo() As String
        Get
            Return "Account_No"
        End Get
    End Property

    Public ReadOnly Property ColAccountName() As String
        Get
            Return "Account_Name"
        End Get
    End Property

    Public ReadOnly Property ColAccountDisplay() As String
        Get
            Return "Account_Display"
        End Get
    End Property

    Public ReadOnly Property ColCutoffHour() As String
        Get
            Return "Cutoff_Hour"
        End Get
    End Property

    Public ReadOnly Property ColCutoffMin() As String
        Get
            Return "Cutoff_Min"
        End Get
    End Property

    Public ReadOnly Property ColCutoffType() As String
        Get
            Return "Cutoff_Type"
        End Get
    End Property

    Public ReadOnly Property ColUserRole() As String
        Get
            Return "User_Role"
        End Get
    End Property

    Public ReadOnly Property ColUserName() As String
        Get
            Return "User_Name"
        End Get
    End Property

    Public ReadOnly Property ColUserAuthCode() As String
        Get
            Return "User_AuthCode"
        End Get
    End Property

    Public ReadOnly Property ColUserFlag() As String
        Get
            Return "User_Flag"
        End Get
    End Property

    Public ReadOnly Property ColUserId() As String
        Get
            Return "User_Id"
        End Get
    End Property

    Public ReadOnly Property ColFileType() As String
        Get
            Return "File_Type"
        End Get
    End Property

    Public ReadOnly Property ColFileName() As String
        Get
            Return "File_Name"
        End Get
    End Property

    Public ReadOnly Property ColFileDateTime() As String
        Get
            Return "File_DateTime"
        End Get
    End Property

    Public ReadOnly Property ColCreatedFileName() As String
        Get
            Return "Created_FileName"
        End Get
    End Property

#End Region

#Region "SQL Properties "

    Public ReadOnly Property SqlGetAccountNoOrgCodes() As String
        Get
            Return "MPG_Get_AccountNo_OrgCodes "
        End Get
    End Property

    Public ReadOnly Property SqlGetGroupPaymentServices() As String
        Get
            Return "MPG_Get_Group_Payment_Services "
        End Get
    End Property

    Public ReadOnly Property SqlGetServiceDetails() As String
        Get
            Return "MPG_Get_Service_Details "
        End Get
    End Property

    Public ReadOnly Property SqlGetGroupFileFormats() As String
        Get
            Return "MPG_Get_Org_Service_Formats "
        End Get
    End Property

    Public ReadOnly Property SqlGetGroupBankAccounts() As String
        Get
            Return "MPG_Get_Org_Bank_Accounts "
        End Get
    End Property

    Public ReadOnly Property SqlGetCutoffTimes() As String
        Get
            Return "MPG_Get_Cutoff_Time "
        End Get
    End Property

    Public ReadOnly Property SqlGetUserDetails() As String
        Get
            Return "MPG_Get_User_Details "
        End Get
    End Property

    Public ReadOnly Property SqlSendMails() As String
        Get
            Return "MPG_Send_Mails "
        End Get
    End Property

    Public ReadOnly Property SqlGetOrgUserDetails() As String
        Get
            Return "MPG_Get_Org_User_Details "
        End Get
    End Property

    Public ReadOnly Property SqlGetWorkflowUsers() As String
        Get
            Return "MPG_Get_Workflow_Users "
        End Get
    End Property

    Public ReadOnly Property SQLErrorLog() As String
        Get
            Return "MPG_Error_Log "
        End Get
    End Property

    Public ReadOnly Property SQLUserLockType() As String
        Get
            Return "MPG_User_Lock "
        End Get
    End Property

#End Region

#Region "Upload Button Disable "

    Public Sub UploadButtonDisable(ByVal DisableButton As System.Web.UI.WebControls.Button)

        Dim strJavaScript As String = "if (typeof(Page_ClientValidate) == 'function') {if (Page_ClientValidate())" & _
                "{this.value='Please wait...';this.disabled = true;alert('" & _Message.MsgUploadAlert & "');" & _
                    "window.showModelessDialog('progress.aspx','','dialogHeight: 100px; dialogWidth: 350px;" & _
                        "edge: Raised; center: Yes; help: No; resizable: yes; status: No;scroll:yes;');" & _
                DisableButton.Page.GetPostBackEventReference(DisableButton) & ";}}"

        Call DisableButton.Attributes.Add("onclick", strJavaScript)

    End Sub

#End Region

#Region "Get Folders "

    Public Function GetFileTypeFolder(ByVal ServiceType As String, _
        ByVal FolderRequest As String) As String

        Select Case ServiceType

            Case DirectDebit_Name()

                Select Case FolderRequest

                    Case UploadFolder
                        Return RootPath & AppSettings("DDEBITUPLOADED") & "\"
                    Case CreateFolder
                        Return RootPath & AppSettings("DDEBITCREATED") & "\"
                    Case ApprovedFolder
                        Return RootPath & AppSettings("DDEBITAPPROVED") & "\"
                    Case Else
                        Return String.Empty

                End Select

            Case Else
                Return String.Empty

        End Select

    End Function

#End Region

End Class
