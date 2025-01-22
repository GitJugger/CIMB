Option Strict Off
Option Explicit On 

Imports MaxFTP.clsFTP
Imports MaxPayroll.clsUsers
Imports MaxPayroll.clsCommon
Imports MaxPayroll.Encryption
Imports MaxPayroll.clsApprMatrix


Namespace MaxPayroll



   Partial Class PG_DeleteFile
      Inherits clsBasePage

#Region "Declaration"
      Private ReadOnly Property rq_lngFileId() As Long
         Get
            If IsNumeric(Request.QueryString("Id")) Then
               Return CLng(Request.QueryString("Id"))
            Else
               Return 0
            End If
         End Get
      End Property
      Private ReadOnly Property rq_lngOrgId() As Long
         Get
                If IsNumeric(Request.QueryString("OID")) Then
                    Return CLng(Request.QueryString("OID"))
                Else
                    Return 0
                End If
         End Get
      End Property
      Private ReadOnly Property rq_strOrgId() As String
         Get
            Return Request.QueryString("OID") & ""
         End Get
      End Property
      Private ReadOnly Property rq_strOrgName() As String
         Get
            Return Request.QueryString("OName") & ""
         End Get
      End Property
      Private ReadOnly Property rq_strGroupName() As String
         Get
            Return Request.QueryString("GName") & ""
         End Get
      End Property
      Private ReadOnly Property rq_intStatus() As Integer
         Get
            If IsNumeric(Request.QueryString("Status")) Then
               Return CInt(Request.QueryString("Status"))
            Else
               Return 0
            End If
         End Get
      End Property
      Private ReadOnly Property rq_strFType() As String
         Get
            Return Request.QueryString("FType") & ""
         End Get
      End Property
      Private ReadOnly Property rq_strFCName() As String
         Get
            Return Request.QueryString("FCName") & ""
         End Get
      End Property
      Private ReadOnly Property rq_strFName() As String
         Get
            Return Request.QueryString("FName") & ""
         End Get
      End Property
      Private ReadOnly Property rq_dtVDate() As Date
         Get
            If IsDate(Request.QueryString("VDate")) Then
               Return CDate(Request.QueryString("VDate"))
            Else
               Return Nothing
            End If
         End Get
      End Property
      Private ReadOnly Property rq_dtUDate() As Date
         Get
            If IsDate(Request.QueryString("UDate")) Then
               Return CDate(Request.QueryString("UDate"))
            Else
               Return Nothing
            End If
         End Get
      End Property
      Private ReadOnly Property rq_lngGroupId() As Long
         Get
            If IsNumeric(Request.QueryString("Group")) Then
               Return Request.QueryString("Group")
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

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Create Instance of System Data Set
         Dim dsDeleteFile As New System.Data.DataSet

         'Create Instance of System Data Row
         Dim drDeleteFile As System.Data.DataRow

         'Create Instance of Common Class Object
         Dim clsCommon As New MaxPayroll.clsCommon

         'Variable Declarations
         Dim strFileType As String, dtValueDate As Date, strTime As String = "", IsCutoff As Boolean
         Dim strMessage As String, strOption As String = "", intStatus As Int16
         Dim strAuthLock As String

         Try

            If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_Interceptor) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then
                    'BindBody(body)
               trConfirm.Visible = False
               trAuthCode.Visible = False

               'Get Authorization Lock Status - Start
               strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngUserID, ss_lngUserID)
               If strAuthLock = "Y" Then
                  strMessage = "Sorry! cannot Stop Payment the file as Your Validation Code has been locked due to invalid attempts. Please contact your Bank Administrator."
                  Server.Transfer("PG_StopPayment.aspx?Err=" & strMessage, False)
                  Exit Try
               End If
               'Get Authorization Lock Status - Stop

               'Get File Details - Start
               intStatus = rq_intStatus
               strFileType = rq_strFType
               lblCFName.Text = rq_strFCName
               If ss_strUserType = gc_UT_BankUser Then
                  trOrg.Visible = True
                  trGroup.Visible = False
                  lblOrgName.Text = rq_strOrgId & " " & rq_strOrgName
               ElseIf ss_strUserType = gc_UT_Interceptor Then
                  trOrg.Visible = False
                  trGroup.Visible = True
                  lblGroupName.Text = rq_strOrgId & " " & rq_strGroupName
               End If
               lblFileName.Text = rq_strFName
               dtValueDate = Format(rq_dtVDate, "dd/MM/yyyy")
               lblValueDt.Text = Format(rq_dtVDate, "dd/MM/yyyy")
               lblUploadDt.Text = Format(rq_dtUDate, "dd/MM/yyyy")
               'Get File Details - Stop

               'Check Cutoff Time - START
               If intStatus = 5 Then

                        If strFileType = "Payroll File" Or strFileType = "Multiple Bank" Then
                            strOption = clsCommon.fncBuildContent("Privilege", "", rq_lngOrgId, ss_lngUserID)
                            'ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                            '    strOption = "E"
                            'ElseIf strFileType = "SOCSO File" Then
                            '    strOption = "S"
                            'ElseIf strFileType = "LHDN File" Then
                            '    strOption = "L"
                        End If

                  'Check Cutoff Time
                  IsCutoff = clsCommon.fncCutoffTime(strFileType, rq_lngOrgId, ss_lngUserID, strTime, Day(dtValueDate), _
                                  Month(dtValueDate), Year(dtValueDate), strOption)

                  If IsCutoff Then
                     btnSubmit.Enabled = False
                     lblMessage.Text = "Stop Payment cannot be done after Cutoff Time (" & strTime & ")"
                     Exit Try
                  End If

               End If
               'Check Cutoff Time - STOP

            End If
            'Check Cutoff Time - STOP

         Catch

            'Log Error
            If Err.Description <> "Thread was being aborted." Then
               Call clsGeneric.ErrorLog(rq_lngOrgId, ss_lngUserID, "PG_DeleteFile - Page_Load", Err.Number, Err.Description)
            End If

         Finally

            'Destroy Instance of Data Set
            dsDeleteFile = Nothing

            'Destroy Instance of Data Row
            drDeleteFile = Nothing

            'Destroy Instance Generic class Object
            clsGeneric = Nothing

            'Destroy Instance of Common Class Object
            clsCommon = Nothing

         End Try

      End Sub

#End Region

#Region "Block/Stop File"

      '****************************************************************************************************
      'Procedure Name : Page_Load()
      'Purpose        : Page Load 
      'Arguments      : N/A
      'Return Value   : N/A
      'Author         : Sujith Sharatchandran - 
      'Created        : 16/02/2005
      '*****************************************************************************************************
      Private Sub prcStop(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnStop.Click

         'Create Instance of Upload Class Object
         Dim clsCommon As New MaxPayroll.clsCommon

         'Create Instance of User Class Object
         Dim clsUsers As New MaxPayroll.clsUsers

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Variable Declarations
         Dim strSubject As String = ""

         Dim IsAuthCode As Boolean
         Dim intAttempts As Int16

         Dim IsDelete As Boolean


         Dim strBody As String = ""
         Dim strDbAuthCode As String


         Try
            'Get Organisation Id
            'rq_intStatus = Request.QueryString("Status")                                              'Get Request From
            'rq_strFName = Request.QueryString("FName")                                              'Get File Name
            'rq_strFType = Request.QueryString("FType")                                              'Get File Type
            'rq_dtVDate = Request.QueryString("VDate")                                             'Get Value Date
            'rq_strFCName = Request.QueryString("FCName")                                            'Get Converted File Name
            'ss_lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)             'Get User Id
            'ss_lngOrgID = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)
            'rq_lngFileId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)     'Get File Id
            'rq_lngOrgId = IIf(IsNumeric(Replace(strOrgId, "E", "")), Replace(strOrgId, "E", ""), 0)    'Get Organisation Id
            'rq_lngGroupId = IIf(IsNumeric(Request.QueryString("Group")), Request.QueryString("Group"), 0)  'Get Group Id

            'Check Session Value for Authorization Lock - Start
            If Not IsNumeric(Session(gc_Ses_AuthLock)) Or Session(gc_Ses_AuthLock) = 0 Then
               Session(gc_Ses_AuthLock) = 0
            End If
            'Check Session Value for Authorization Lock - Stop

            'Check If AuthCode is Valid - Start
            strDbAuthCode = clsCommon.fncPassAuth(ss_lngUserID, "A", rq_lngOrgId)
            IsAuthCode = IIf(strDbAuthCode = txtAuthCode.Text, True, False)
            'Check If AuthCode is Valid - Stop

            'Check for invalid Authorization Code Attempts - START
            If Not IsAuthCode Then
               intAttempts = IIf(IsNumeric(Session(gc_Ses_AuthLock)), Session(gc_Ses_AuthLock), 0)
               If Not intAttempts = 2 Then
                  If Not IsAuthCode Then
                     intAttempts = intAttempts + 1
                     Session(gc_Ses_AuthLock) = intAttempts
                     lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
                     Exit Try
                  End If
               ElseIf intAttempts = 2 Then
                  If Not IsAuthCode Then
                     lblMessage.Text = ""
                     trConfirm.Visible = False
                     trAuthCode.Visible = False
                     'lock out user auth code
                     Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "A")
                     'update for lock out report
                     Call clsUsers.prcLockHistory(ss_lngUserID, "A")
                     lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                     Exit Try
                  End If
               End If
            End If
            'Check for invalid Authorization Code Attempts - STOP

            'Delete File Function
            IsDelete = fncBlock(rq_lngOrgId, ss_lngUserID, rq_strFCName, rq_strFType, rq_intStatus)

            'If Successful/Failed - START
            If IsDelete Then

               'Display Message
               lblMessage.Text = "Stop Payment Successful"

               'Build Subject
               strSubject = rq_strFType & " (" & rq_strFName & ") " & "Stop Payment Successful."

               'Build Body
               strBody = "The " & rq_strFType & " (" & rq_strFName & ") " & "with Payment Date " & rq_dtVDate.ToString & " has been succesfully Stopped."

               'Block File Status
               Call clsCommon.prcBlockFile(rq_lngFileId, 3, rq_lngOrgId, ss_lngUserID)

               'Delete File From FTP Folder
               Call clsCommon.prcDelFile(rq_lngOrgId, ss_lngUserID, rq_strFType, rq_strFName)

               'If Delete And Stop Payment
            ElseIf Not IsDelete Then

               'Display Message
               lblMessage.Text = "Stop Payment Failed"

               'Build Subject
               strSubject = rq_strFType & " (" & rq_strFName & ") " & "Stop Payment Failed."     'Subject

               'Build Body
               strBody = "The " & rq_strFType & " (" & rq_strFName & ") " & "with Payment Date " & rq_dtVDate & ". Stop Payment failed."

            End If
            'If Successful/Failed - STOP

            'Send Mail 
            If Not ss_strUserType = gc_UT_Interceptor Then   'if Bank user does stop payment
               Call clsCommon.prcSendMails("CUSTOMER", rq_lngOrgId, 0, rq_lngGroupId, strSubject, strBody)
            Else    'if interceptor does stop payment
               Call clsCommon.prcSendMails("CUSTOMER", rq_lngOrgId, ss_lngUserID, rq_lngGroupId, strSubject, strBody)
            End If

            trSubmit.Visible = True
            btnSubmit.Visible = False
            trConfirm.Visible = False
            trAuthCode.Visible = False

         Catch

            'Log Error
            LogError("PG_DeleteFile - btnStop prcStop")

         Finally

            'Destroy Instance of User Class Object
            clsUsers = Nothing

            'Destroy Instance of Upload Class Object
            clsCommon = Nothing

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

         End Try

      End Sub

#End Region

#Region "Delete File"

      '****************************************************************************************************
      'Procedure Name : fncBlock()
      'Purpose        : Block / Stop File
      'Arguments      : 
      'Return Value   : Boolean
      'Author         : Sujith Sharatchandran - 
      'Created        : 16/02/2005
      '*****************************************************************************************************
      Private Function fncBlock(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileName As String, _
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
                        'ElseIf strFileType = "EPF File" Or strFileType = "EPF Test File" Then
                        '    strFolder = "EPFIN\"
                        '    strFileName = Replace(strFileName, ".", "_") & ".cry"
                        'ElseIf strFileType = "SOCSO File" Then
                        '    strFolder = "SOCSOIN\"
                        '    strFileName = Replace(strFileName, ".", "_") & ".cry"
                        'ElseIf strFileType = "LHDN File" Then
                        '    strFolder = "LHDNIN\"
                        '    strFileName = Replace(strFileName, ".", "_") & ".cry"
                    End If
            ElseIf intStatus = 0 Then
               strFolder = "CREATED\"
                    'If (strFileType = "EPF File" Or strFileType = "EPF Test File" Or strFileType = "SOCSO File" Or strFileType = "LHDN File") Then
                    '   strFileName = Replace(strFileName, ".", "_") & ".cry"
                    'End If
            End If
            'Get File Type and FTP Folder - Stop

            'Put File to the Database Server
            IsDelete = clsCommon.fncDelFile(lngOrgId, lngUserId, strFolder & strFileName)

            Return IsDelete

         Catch

            'Log Error
            LogError("PG_BlockFile-fncBlock")

            Return False

         Finally

            'Destroy Instance of Generic Class Object
            clsGeneric = Nothing

            'Destroy Instance of common Class Object
            clsCommon = Nothing

         End Try

      End Function

#End Region

#Region "Confirm"

      Private Sub prcSubmit(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSubmit.Click

         Try

            trConfirm.Visible = True
            trSubmit.Visible = False
            trAuthCode.Visible = True
            lblMessage.Text = "Please Enter your Validation Code and Confirm Stop Payment"

         Catch

         End Try

      End Sub

#End Region

   End Class

End Namespace
