Namespace MaxPayroll

    Partial Class PG_ViewApprMatrix
        Inherits clsBasePage

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

#Region "Declaration"
        Private ReadOnly Property rq_iPageNo() As Integer
            Get
                If IsNumeric(Request.QueryString("PageNo")) Then
                    Return CInt(Request.QueryString("PageNo"))
                Else
                    Return 0
                End If
            End Get
        End Property
#End Region

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load 
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/02/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Dim clsEncryption As New MaxPayroll.Encryption
            'Variable Declarations
            Dim strModule As String, lngTransId As Long, strMode As String, lngApprId As Long, strURL As String

            Try
                strMode = Request.QueryString("Mode")                                                       'Get Request Mode
                strModule = Request.QueryString("Module")                                                   'Get Request Module
                If (Request.QueryString("Id")) IsNot Nothing Then
                    lngTransId = clsEncryption.Cryptography(Request.QueryString("Id"))
                Else
                    lngTransId = 0
                End If
                If (Request.QueryString("Appr")) IsNot Nothing Then
                    lngApprId = clsEncryption.Cryptography(Request.QueryString("Appr"))
                Else
                    lngApprId = 0
                End If


                'lngTransId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)        'Get Transaction Id
                ' lngApprId = IIf(IsNumeric(Request.QueryString("Appr")), Request.QueryString("Appr"), 0)     'Get Approval Request Id

                'If User Role Creation/Modification
                If (strModule = "User Creation" Or strModule = "User Modification" Or strModule = "User Deletion") Then
                    Dim EncryptedId = GetEncrypterString(lngTransId)
                    strURL = "PG_CreateRole.aspx?PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & EncryptedId & "&Mod=View&Mode=" & strMode & "&APPR=" & lngApprId
                    Server.Transfer(strURL, False)
                    Exit Try
                    'If Group Creation/Modification
                ElseIf (strModule = "Group Creation" Or strModule = "Group Modification" Or strModule = "Group Cancellation") Then
                    Dim EncryptedId = GetEncrypterString(lngTransId)

                    strURL = "PG_Group2.aspx?PageNo=" & rq_iPageNo.ToString & "&PageMode=" & enmPageMode.ViewMode & "&Id=" & EncryptedId & "&Mod=View&Mode=" & strMode & "&APPR=" & lngApprId
                    Server.Transfer(strURL, False)
                    Exit Try
                    'if organisation cancel
                ElseIf strModule = "Cancel Organization" Then
                    strURL = "PG_ModifyOrganisation.aspx?PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & lngTransId & "&Mode=" & strMode
                    Server.Transfer(strURL, False)
                    Exit Try
                ElseIf strModule = "Mandate File Approval" Then
                    'LoadFileHeader
                    Dim oItem As New clsMandates
                    oItem = oItem.LoadFileHeader(lngTransId)

                    strURL = "pg_MandateFileDetails.aspx?Type=" & enmMandateFileAction.BankAuthorize.ToString & "&PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & lngTransId & "&Mode=" & strMode & "&FN=" & oItem.paramFileName & "&VD=" & oItem.paramDoneDate & "&FT=Mandate File"

                    Server.Transfer(strURL, False)
                    Exit Try
                ElseIf strModule = "Mandate Record Creation" Then
                    Dim oItem As New clsMandates
                    oItem = oItem.LoadById(lngTransId)
                    'pg_MandatesDetails.aspx?Id=100001&RefNo=135794&BankOrgCode=2028&AccNo=14060545723527&Name=MOHD%20FIRDAUS%20BIN%20MOH&PageMode=1
                    'strURL = "pg_MandatesDetails.aspx?Type=" & enmMandateFileAction.BankAuthorize.ToString & "&PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & lngTransId & "&Mode=" & strMode & "&FN=" & oItem.paramFileName & "&VD=" & oItem.paramDoneDate & "&FT=Mandate File"
                    strURL = "pg_MandatesDetails.aspx?Id=" & oItem.paramOrgID & "&RefNo=" & oItem.paramRefNo & "&BankOrgCode=" & oItem.paramBankOrgCode & "&AccNo=" & oItem.paramAccNo & "&Name=" & oItem.paramCustomerName & "&RecType=New&PageMode=" & enmPageMode.ViewMode
                    Server.Transfer(strURL, False)
                    Exit Try
                ElseIf strModule = "Mandate Record Modification" Then
                    Dim oItem As New clsMandates
                    oItem = oItem.LoadTempById(lngTransId)

                    'pg_MandatesDetails.aspx?Id=100001&RefNo=135794&BankOrgCode=2028&AccNo=14060545723527&Name=MOHD%20FIRDAUS%20BIN%20MOH&PageMode=1
                    'strURL = "pg_MandatesDetails.aspx?Type=" & enmMandateFileAction.BankAuthorize.ToString & "&PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & lngTransId & "&Mode=" & strMode & "&FN=" & oItem.paramFileName & "&VD=" & oItem.paramDoneDate & "&FT=Mandate File"
                    'strURL = "pg_MandatesDetails.aspx?Type=" & enmMandateFileAction.BankAuthorize.ToString & "&PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & oItem.paramOrgID & "&Mode=" & strMode & "&RefNo=" & oItem.paramRefNo & "&BankOrgCode=" & oItem.paramBankOrgCode & "&AccNo=" & oItem.paramAccNo & "&Name=" & oItem.paramCustomerName & "&RecType=Mod&PageMode=" & enmPageMode.ViewMode'
                    strURL = "pg_MandatesDetails.aspx?Type=" & enmMandateFileAction.BankAuthorize.ToString & "&PageNo=" & rq_iPageNo.ToString & "&FieldLock=" & enmPageMode.NonEditableMode & "&Id=" & oItem.paramOrgID & "&Mode=" & strMode & "&RefNo=" & oItem.paramRefNo & "&BankOrgCode=" & oItem.paramBankOrgCode & "&AccNo=" & oItem.paramAccNo & "&Name=" & oItem.paramCustomerName & "&RecType=Mod&PageMode=" & enmPageMode.ViewMode

                    Server.Transfer(strURL, False)
                    Exit Try
                End If

            Catch ex As Exception
                Dim a As String = ex.Message
            End Try

        End Sub

#End Region
        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function
    End Class


End Namespace
