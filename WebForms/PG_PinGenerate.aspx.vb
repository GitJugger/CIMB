Option Strict Off
Option Explicit On 

Imports System.IO
Imports System.Text
Imports MaxPayroll.Generic


Namespace MaxPayroll


    Partial Class PG_PinGenerate
        Inherits clsBasePage

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Load Page Functions
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Variable Declarations
            Dim strAuthLock As String, intSerialNo As Int16

            Try

                If IsNothing(Session("tempCtrlFlag")) = False AndAlso IsNumeric(Session("tempCtrlFlag")) Then
                    lblMessage.Text = "Pin Mailer Generates Successfully"
                    Session("tempCtrlFlag") = Nothing
                End If
                If Not Page.IsPostBack Then
                    'BindBody(body)
                    Dim clsUsers As New clsUsers
                    Call clsUsers.prcDetailLog(ss_lngUserID, "Pin Generation", "Y")
                    Me.btnGenerate.Attributes.Add("onclick", "setReloadTime(3)")
                End If

                'If User Type Not Bank User - Start
                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = "BS") Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If User Type Not Bank User - Stop

                'Get Auth Lock Status
                strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                If strAuthLock = "Y" Then
                    btnShow.Enabled = False
                    lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                End If

                'Get if Serial No Available
                intSerialNo = clsPinMailer.fncPinSerialCheck("N", ss_lngUserID)

                If intSerialNo = 0 Then
                    btnShow.Enabled = False
                    lblMessage.Text = "Please enter Serial No range before Pin Generation."
                End If

                'Call clsCommon.fncBtnDisable(btnGenerate, True)

            Catch ex As Exception

                LogError("PG_PinGenerate - Page Load")

            Finally

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

            End Try

        End Sub

#End Region

#Region "Show"

        '****************************************************************************************************
        'Procedure Name : prcShow()
        'Purpose        : Bind Data grid and Display
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcShow(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnShow.Click

            Try

                Call prcBindGrid()
                trSelect.Visible = True
            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Bind Grid"

        '****************************************************************************************************
        'Procedure Name : prcBindGrid()
        'Purpose        : Bind Data grid and Display
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcBindGrid()

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Create Instance of System Data Set
            Dim dsPinStatus As New System.Data.DataSet

            'Variable Declarations
            Dim lngUserId As Long, intDays As Int16

            Try

                intDays = txtDays.Text
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Populate Data Set
                dsPinStatus = clsPinMailer.fncPinStatus(lngUserId, "G", intDays)

                'Bind Data Grid - Start
                If dsPinStatus.Tables("PINSTATUS").Rows.Count > 0 Then
                    fncGeneralGridTheme(dgPinGenerate)
                    lblMessage.Text = ""
                    tblGrid.Visible = True
                    trSubmit.Visible = False
                    trAuthCode.Visible = False
                    dgPinGenerate.DataSource = dsPinStatus
                    dgPinGenerate.DataBind()
                Else

                    tblGrid.Visible = False
                    lblMessage.Text = "No Pin Mailer Requisition for Generation"
                End If
                'Bind Data Grid - Stop

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcBindGrid - PG_PinGenerate", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dsPinStatus = Nothing

            End Try

        End Sub

#End Region

#Region "Select/Unselect All"

        '****************************************************************************************************
        'Procedure Name : prcSelect()
        'Purpose        : To Select The List of Displayed Pin Request
        'Arguments      : Object,Event Arguments
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/07/2005
        '*****************************************************************************************************
        Private Sub prcSelect(ByVal Source As Object, ByVal E As EventArgs) Handles btnSelect.Click

            Dim GridItem As DataGridItem

            Try

                For Each GridItem In dgPinGenerate.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.FindControl("chkSelect"), CheckBox)
                    myCheckbox.Checked = True
                Next

            Catch ex As Exception

            End Try

        End Sub

        '****************************************************************************************************
        'Procedure Name : prcUnselect()
        'Purpose        : To Unselect The List of Displayed Pin Request
        'Arguments      : Object,Event Arguments
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/07/2005
        '*****************************************************************************************************
        Sub prUncheck(ByVal Source As Object, ByVal E As EventArgs) Handles btnUnSelect.Click

            Dim GridItem As DataGridItem

            Try

                For Each GridItem In dgPinGenerate.Items
                    Dim myCheckbox As CheckBox = CType(GridItem.FindControl("chkSelect"), CheckBox)
                    myCheckbox.Checked = False
                Next

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Confirm"

        '****************************************************************************************************
        'Procedure Name : prcConfirm()
        'Purpose        : Confirm Request
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcConfirm(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnConfirm.Click

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            'Variable Declarations
            Dim lngUserId As Long, IsSelected As Boolean

            Try

                IsSelected = False
                lblMessage.Text = "Please Enter your Validation Code and Confirm your Generation."
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Loop Thro Datagrid - Start
                For Each dgiRequisition In dgPinGenerate.Items
                    Dim chkSelect As CheckBox = CType(dgiRequisition.FindControl("chkSelect"), CheckBox)
                    If chkSelect.Checked Then
                        IsSelected = True
                    End If
                    chkSelect.Enabled = False
                Next
                'Loop Thro Datagrid - Stop

                'If Nothing Selected - Start
                If Not IsSelected Then
                    'Loop Thro Datagrid - Start
                    For Each dgiRequisition In dgPinGenerate.Items
                        Dim chkSelect As CheckBox = CType(dgiRequisition.FindControl("chkSelect"), CheckBox)
                        chkSelect.Enabled = True
                    Next
                    'Loop Thro Datagrid - Stop
                    lblMessage.Text = "Please Select atleast one Requisition"
                    Exit Try
                End If
                'If Nothing Selected - Stop

                trSelect.Visible = False    'Hide Select Buttons
                trSubmit.Visible = True     'Show Submit Button
                trAuthCode.Visible = True   'Show Auth Code

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(100000, lngUserId, "prcConfirm - PG_PinGenerate", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Submit Request"

        '****************************************************************************************************
        'Procedure Name : prcGenerate()
        'Purpose        : Generate Pin Requisition
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcGenerate(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnGenerate.Click

            'Create Instance of Stream Writer
            Dim stmWriter As StreamWriter

            'Create Instance of Datagrid Item
            Dim dgiRequisition As DataGridItem

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'create instance of encryption class object
            Dim clsEncryption As New MaxPayroll.Encryption

            'Create Instance of Pin Mailer Class Object
            Dim clsPinMailer As New MaxPayroll.clsPinMailer

            'Variable Declarations
            Dim lngUserId As Long
            Dim intReqId As Int16
            Dim intGenId As Int16
            Dim strTempFileName As String
            Dim lngOrgId As Long
            Dim strCryptPassword As String
            Dim strCrypt As String
            Dim IsInsert As Boolean
            Dim strFileContent As String
            Dim strOutFile As String
            Dim strAuthMsg As String

            Try

                'Get User Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)

                'Check Auth Code - Start
                strAuthMsg = clsGeneric.fncAuthCheck(100000, lngUserId, txtAuthCode.Text)
                If Not strAuthMsg = "" Then
                    lblMessage.Text = strAuthMsg
                    Exit Try
                End If
                'Check Auth Code - Stop

                'Get Generate Id
                intGenId = clsPinMailer.fncGenerateId(lngUserId)

                'Loop Thro Datagrid - Start
                For Each dgiRequisition In dgPinGenerate.Items
                    intReqId = dgiRequisition.Cells(0).Text
                    Dim chkSelect As CheckBox = CType(dgiRequisition.FindControl("chkSelect"), CheckBox)
                    If chkSelect.Checked Then
                        'Update Genarate Flag and Generate Id
                        IsInsert = clsPinMailer.fncPinRequisition("C", intReqId, 0, lngUserId, "", "", "", "", "", "", intGenId)
                        If Not IsInsert Then
                            lblMessage.Text = "Pin Mailer Generation Failed."
                            Exit Try
                        End If
                    End If
                Next
                'Loop Thro Datagrid - Stop

                'Get File Content
                strFileContent = clsPinMailer.fncFileContents(intGenId)

                'Check If Empty File Contents - Start
                If strFileContent = "" Then
                    lblMessage.Text = "Pin Mailer File Generation Failed."
                    Exit Try
                End If
                'Check If Empty File Contents - Start

                'Create File - Start
                strTempFileName = System.Configuration.ConfigurationManager.AppSettings("PATH") & "PG" & Format(Day(Today), "00") & Format(Month(Today), "00") & Format(Year(Today), "00") & Format(Now.Hour, "00") & Format(Now.Minute, "00") & Format(Now.Second, "00") & ".pg"
                stmWriter = New StreamWriter(strTempFileName, True, Encoding.Unicode)
                stmWriter.WriteLine(strFileContent)
                stmWriter.Flush()
                stmWriter.Close()
                'Create File - Stop

                'Build Encryption File Name
                strOutFile = Replace(strTempFileName, ".", "_")
                strOutFile = strOutFile & ".cry"

                'Get encryption password
                strCryptPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("ENCPWD"))

                'encrypt file
                strCrypt = MaxCrypt.clsCrypto.fncEncryptFile(strTempFileName, strOutFile, strCryptPassword)

                'Delete Temp File - Start
                If File.Exists(strTempFileName) Then
                    File.Delete(strTempFileName)
                End If
                'Delete Temp File - Stop

                'Check Encryption Successful - Start
                If Not strCrypt = gc_Status_OK Then
                    lblMessage.Text = "Pin Mailer Encryption Failed"
                    Exit Try
                End If
                'Check Encryption Successful - Stop

                'Loop Thro Datagrid - Start
                For Each dgiRequisition In dgPinGenerate.Items
                    intReqId = dgiRequisition.Cells(0).Text
                    lngOrgId = dgiRequisition.Cells(1).Text
                    Dim chkSelect As CheckBox = CType(dgiRequisition.FindControl("chkSelect"), CheckBox)
                    If chkSelect.Checked Then
                        'Update Generate Flag
                        IsInsert = clsPinMailer.fncPinRequisition("G", intReqId, 0, lngUserId, "", "", "", "", "", "", 0)
                        'Charge Annual Fee
                        Call clsPinMailer.fncFeeCharge(lngOrgId)
                        If Not IsInsert Then
                            lblMessage.Text = "Pin Mailer Generation Failed."
                            Exit Try
                        End If
                    End If
                Next
                'Loop Thro Datagrid - Stop

                'Hide Grid
                tblGrid.Visible = False

                'Display Successful Message
                lblMessage.Text = "Pin Mailer Generation Successful."
                Session("tempCtrlFlag") = lngOrgId
                'Download File
                Call prcDownload(strOutFile)

            Catch

                'Error Message
                lblMessage.Text = "Pin Mailer Requisition Failed."

                'Log Error
                If Err.Description <> "Thread was being aborted." Then
                    Call clsGeneric.ErrorLog(100000, lngUserId, "prcGenerate - PG_PinGenerate", Err.Number, Err.Description)
                End If

            Finally

                'Destroy Instance of Stream Writer
                stmWriter = Nothing

                'Destroy Instance of Datagrid Item
                dgiRequisition = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Pin Mailer Class Object
                clsPinMailer = Nothing

                'destroy instance of encryption class object
                clsEncryption = Nothing

            End Try

        End Sub

#End Region

#Region "DownLoad File"

        '****************************************************************************************************
        'Procedure Name : prcDownLoad()
        'Purpose        : DownLoad Generated File
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/07/2005
        '*****************************************************************************************************
        Private Sub prcDownload(ByVal strFileName As String)

            'Create Instanced of File Info
            Dim File As FileInfo

            Try

                If Not strFileName = "" Then
                    File = New FileInfo(strFileName)
                End If

                'Download File - Start
                If File.Exists Then
                    'Response.Clear()
                    'Response.AddHeader("Content-Disposition", "attachment; filename=" & File.Name)
                    'Response.AddHeader("Content-Length", File.Length.ToString())
                    'Response.ContentType = "application/octet-stream"
                    'Response.WriteFile(File.FullName)
                    'Response.End()

                    Response.ContentType = "text"
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" & File.FullName)
                    Response.TransmitFile(File.FullName)
                    Response.End()
                End If
                'Download File - Stop

            Catch ex As Exception
                LogError("pg_PinGenerate - prcDownload")
            End Try

        End Sub

#End Region

    End Class

End Namespace
