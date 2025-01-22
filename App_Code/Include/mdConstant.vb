Namespace MaxPayroll

    Public Module mdConstant

#Region "Constant"
        'Constant for Session - Begin
        Public Const gc_Ses_Token As String = "UseToken"
        Public Const gc_Ses_LogId As String = "LOG_ID"
        Public Const gc_Ses_OrgId As String = "SYS_ORGID"
        Public Const gc_Ses_ExpiryMsg As String = "EXP_MSG"
        Public Const gc_Ses_UserType As String = "SYS_TYPE"
        Public Const gc_Ses_UserID As String = "SYS_USERID"
        Public Const gc_Ses_GroupName As String = "SYS_GNAME"
        Public Const gc_Ses_GroupID As String = "SYS_GROUPID"
        Public Const gc_Ses_UserLoginName As String = "LoginName"
        Public Const gc_Ses_UserExpiryDate As String = "EXP_DT"
        Public Const gc_Ses_AuthChgStatus As String = "AUTH_CHNG"
        Public Const gc_Ses_AuthLock As String = "AUTH_LOCK"
        Public Const gc_Ses_PwdChgStatus As String = "PWD_CHNG"
        Public Const gc_Ses_PwdInvalid As String = "PASS_INVALID"
        Public Const gc_Ses_VerificationType As String = "SYS_VERIFY"
        Public Const gc_Ses_PwdExpiryDate As String = "PWD_EXP"
        Public Const gc_Ses_WarningFlag As String = "SYS_WARN"

        Public Const gc_Ses_PopOut As String = "PopOutState"
        'Constant for Session - End

        'Constant for Return Type - Start
        Public Const gc_Status_OK As String = "OK"
        Public Const gc_Status_Error As String = "Error"
        'Constant for Return Type - End

        'Constant for Path - Start
        Public Const gc_LogoutPath As String = "PG_Logout.aspx?Timed=True"
        'Constant for Path - End

        'Constant for Title - Start
        Public Const gc_Const_ApplicationName As String = "CIMB Gateway"
        Public Const gc_Const_CompanyName As String = "CIMB Gateway"

        Public Const gc_Const_RegCenter As String = "Registration Center"

        Public Const gc_Const_CompanyContactNo As String = "1-300-888-828"
        Public Const gc_Const_CompanyCustCareNo As String = "[From Victor, Pls give me the number]"
        Public Const gc_Const_CCPrefix As String = "M"
        'Constant for Title - End

        'COnstanst For pg_UploadCommon
        Public Const gc_UCBankFormat As String = "BANK FORMAT"

        'General Message for Errors
        Public Const gc_MsgError As String = "The accessed function has encountered technical problem :"
        Public Const gc_MsgInvalidValidationCode As String = "Validation code is invalid. Please enter a valid Validation Code."
        Public Const gc_MsgValidationLocked As String = "Your Validation Code has been locked due to invalid attempts."

        'Html Constant
        Public Const gc_BR As String = "<BR>"
        Public Const gc_Space As String = "&nbsp;"

        Public Const gc_UT_SysAdmin As String = "CA"
        Public Const gc_UT_SysAdminDesc As String = "Corporate Administrator"
        Public Const gc_UT_SysAuth As String = "SA"
        Public Const gc_UT_SysAuthDesc As String = "Corporate Authorizer"
        Public Const gc_UT_BankAdmin As String = "BA"
        Public Const gc_UT_BankAdminDesc As String = "CIMB Gateway Administrator"
        Public Const gc_UT_BankAuth As String = "BS"
        Public Const gc_UT_BankAuthDesc As String = "CIMB Gateway Authorizer"
        Public Const gc_UT_BankUser As String = "BU"
        Public Const gc_UT_BankUserDesc As String = "Operation User"
        Public Const gc_UT_BankOperator As String = "BO"
        Public Const gc_UT_BankOperatorDesc As String = "Operator"
        Public Const gc_UT_InquiryUser As String = "IU"
        Public Const gc_UT_InquiryUserDesc As String = "CSR User"
        Public Const gc_UT_Auth As String = "A"
        Public Const gc_UT_AuthDesc As String = "Approver"
        Public Const gc_UT_Uploader As String = "U"
        Public Const gc_UT_UploaderDesc As String = "Uploader"
        Public Const gc_UT_Reviewer As String = "R"
        Public Const gc_UT_ReviewerDesc As String = "Reviewer"
        Public Const gc_UT_Interceptor As String = "I"
        Public Const gc_UT_InterceptorDesc As String = "Interceptor"
        Public Const gc_UT_BankDownloader As String = "BD"
        Public Const gc_UT_BankDownloaderDesc As String = "Back Office User"
        Public Const gc_UT_ReportDownloader As String = "RD"
        Public Const gc_UT_ReportDownloaderDesc As String = "Report Downloader"

        Public Const gc_RptUploadList As String = "UploadList"
        Public Const gc_RptFileStatus As String = "FileStatus"
        Public Const gc_RptFileStatusDetails As String = "FileStatusDetails"
        Public Const gc_RptUserRole As String = "UserList"
        Public Const gc_RptUserLog As String = "UsersLog"
        Public Const gc_RptUserGroup As String = "GroupInfo"
        Public Const gc_RptUserLock As String = "PasswordLock"
        Public Const gc_RptStopPayment As String = "StopPayment"
        Public Const gc_RptOrganizationList As String = "OrganizationList"
        Public Const gc_RptUserExpiry As String = "UserExpiry"
        Public Const gc_RptPinGeneration As String = "PinGeneration"
        Public Const gc_RptDormant As String = "DormantAccount"
        Public Const gc_RptCancel As String = "Cancellation"
        Public Const gc_RptFileSubmission As String = "FileSubmission"
        Public Const gc_RptRegistration As String = "OrganizationRegistration"
        Public Const gc_RptInfenionDetails As String = "InfenionReport"

        Public Const gc_RptTransaction As String = ""

        Public Const gc_WC_DefaultBank As String = "DefaultBankCode"


        Public Const pre_Date_yyyyMMdd_CheckingOnly As String = "DD"
        Public Const pre_Time_HHmmss_CheckingOnly As String = "CT"
        Public Const pre_CPSPaymentMode As String = "PM"
        Public Const pre_DeliveryMode As String = "DM"
        Public Const pre_PaymentDate_CheckingOnly As String = "ND"

        Public Const gc_UT_BankUserRole As String = "BankUser - BU"
        Public Const gc_UT_BankOperatorRole As String = "BankOperator - BO"
        Public Const gc_UT_BankAdminRole As String = "BankAdmin - BA"


#End Region

#Region "Enumeration"
        Enum enmMandateStatus
            Pending
            Approve
            Reject
        End Enum
        Enum enmMandateFileAction
            Review
            Approve
            BankAuthorize
        End Enum
        Enum enmFrequency
            UN_Unlimited
            DY_Daily
            WY_Weekly
            MY_Monthly
            QY_Quarterly
            HY_Half_Yearly
            YY_Yearly
        End Enum

        Public Function GetButtonName(ByVal eType As enmButton) As String
            Return eType.ToString.Replace("_", " ")
        End Function
        Enum enmButton
            Save
            Update
            Modify
            Clear
            Reset
            Confirm
            Create_New
            Submit
        End Enum
        Enum enmDisable
            Save_Update
            Confirm
            AfterConfirm
        End Enum

        Enum enmCPSDoc
            CPSPayment
            CPSInvoice
        End Enum

        'Progress Indicator pop out state control
        Enum enmPIType
            Open = 0
            Waiting = 100
            Close = 200
        End Enum

        'User Type - Start
        Enum enmUserType
            CA_SysAdmin
            SA_SysAuthorizer
            BA_BankAdmin
            BS_BankAuthorizer
            BU_BankUser
            BO_BankOperator
            IU_InquiryUser
            A_Authorizer
            U_Uploader
            R_Reviewer
            I_Interceptor
            BD_BankDownloader
            RD_ReportDownloader
            Login
        End Enum
        'User Type - End

        'Status - Start
        Enum enmStatus
            A_Active
            I_Inactive
            C_Cancel
            D_Delete
        End Enum
        'Status - End

        Enum enmStatutory
            E_EPF
            S_Socso
            L_LHDN
            Z_Zakat
        End Enum

        'Page Mode - Start
        Enum enmPageMode
            NewMode = 0
            EditMode = 1
            ViewMode = 2
            NonEditableMode = 3
        End Enum
        'Page Mode - End

        'Match Field in Bank File Format - Start
        Enum enmMatchField
            None
            Org_BrCode
            State
            BNM_Code
            BankOrg_Code
            Org_Code
            Org_Name
            Ac_Number
            Ic_Number
            Chrg_PerTran
            Chrg_perTran_Epf
            Reg_Number
            Epf_Number
            Tax_Number
            Soc_Number
            Contact_Person
            Status
            Contact_Number
            CIMB_Batch_No
            CIMB_Reference_No
            Email_Address
            Zakat_Employer_Name
            Unique_Reference '02/12/2008 , to be used to generate unique data to identify records for updation
        End Enum
        'Match Field in Bank File Format - End

        Enum enmViewOrganizationReqType
            Modify
            CreateOrgCharge
            ModifyOrgCharge
            File
            Bank
            Epf
            Socso
            LHDN
            ZAKAT
            BankCodeMapping
            H2HUser
            Mandates
            CPS
            CPS_ChequeModify
        End Enum
#End Region

    End Module

End Namespace
