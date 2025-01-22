Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsCustomer
Imports MaxPayroll.clscommon
Imports Microsoft.ApplicationBlocks.Data
Imports System.Data.SqlClient
Imports MaxGeneric

Namespace MaxPayroll


    Partial Class PG_CustomerFormat
        Inherits clsBasePage

#Region " Global declarations "

        Dim clsGeneric As New MaxPayroll.Generic
        Private _PPS As New MaxMiddleware.PPS
        Private _Helper As New Helper
#End Region

#Region " Query string values "

        ReadOnly Property intOrgID() As Integer
            Get
                Try
                    Return CInt(Request.QueryString("OrgId") & "")
                Catch ex As Exception
                    Return 0
                End Try
            End Get
        End Property
        Private ReadOnly Property rq_strOrgName() As String
            Get
                Return Request.QueryString("Name") & ""
            End Get
        End Property
        Private ReadOnly Property rq_lngFileID() As Long
            Get
                If IsNumeric(Request.QueryString("Id")) Then
                    Return CLng(Request.QueryString("Id"))
                Else
                    Return 0
                End If
            End Get
        End Property
#End Region

#Region "Page Load"

        '****************************************************************************************************
        'Function Name  : Page_Load
        'Purpose        : Page Load Functions
        'Arguments      : Page Items
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2003
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Data Set
            Dim dsFileType As New System.Data.DataSet

            'Create instance of system data row
            Dim drFileType As System.Data.DataRow

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer


            Try

                If Not ss_strUserType = gc_UT_BankUser And Not ss_strUserType = gc_UT_InquiryUser Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                hOrgId.Value = intOrgID
                hOrgN.Value = rq_strOrgName
                hFileId.Value = rq_lngFileID



                If Not Page.IsPostBack Then


                    'Heading
                    lblHead.Text = "File Settings"

                    '071019 line added by Marcus
                    'Purpose: To hide bank info when there is a default Bank Code set up in web.config
                    BindDDLBankCode()
                    fncDefaultBankChecking(Me.ddlBank, Me.lblBankSelect)
                    prcBindDDLFileFormat()

                    'Bind File Encryption types
                    MaxMiddleware.PPS.EnumToDropDown(GetType(Helper.EncryptionType), ddlEncryptionType, True)

                    If rq_lngFileID > 0 Then
                        Call prcFormat(intOrgID, ss_lngUserID, rq_lngFileID, ss_strUserType)
                    Else
                        Call prcClear()
                    End If

                    If ss_strUserType = gc_UT_InquiryUser Then
                        btnSave.Visible = False
                    End If

                    If cmbFile.SelectedItem.Text = _Helper.CPSDelimited_Dividen_Name Then
                        trduplicate.Visible = True
                        cbduplicate.Checked = True
                        cbduplicate.Enabled = False
                    Else
                        trduplicate.Visible = False
                        cbduplicate.Checked = False
                    End If

                End If

            Catch

                'Log Error
                LogError("Page Load - PG_CustomerFormat")

            Finally

                'Destroy instance of system data row
                drFileType = Nothing

                'Destroy Instance of system data set
                dsFileType = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomer = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

        Private Sub BindDDLBankCode()
            Dim clsBankMF As New clsBankMF
            ddlBank.Items.Clear()
            ddlBank.DataSource = clsBankMF.fncRetrieveBankCodeName("A")
            ddlBank.DataTextField = "BankName"
            ddlBank.DataValueField = "BankID"
            ddlBank.DataBind()
            ddlBank.Items.Insert(0, New ListItem("Select", ""))
        End Sub

#End Region

#Region "File Type"

        '****************************************************************************************************
        'Function Name  : prcFile()
        'Purpose        : File Type Change Function
        'Arguments      : Page Items
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2003
        '*****************************************************************************************************
        Private Sub prcFile(ByVal O As System.Object, ByVal E As System.EventArgs) Handles cmbFile.SelectedIndexChanged
            hFileType.Value = Me.cmbFile.SelectedItem.Text
            If cmbFile.SelectedItem.Text = _Helper.CPSDelimited_Dividen_Name Then
                trduplicate.Visible = True
                cbduplicate.Checked = True
                cbduplicate.Enabled = False
            Else
                trduplicate.Visible = False
                cbduplicate.Checked = False
            End If
            Call prcClear()

        End Sub

#End Region

#Region "File Format"

        Private Function fncValidate() As Boolean
            Dim sMsg As String = ""
            Dim clsFileSetting As New clsFileSetting
            'If ddlBank.Visible Then
            If ddlBank.SelectedValue = "" Then
                sMsg += "Please select Bank." & gc_BR
            End If
            If cmbFile.SelectedValue = "" Then
                sMsg += "Please select File Type." & gc_BR
            End If
            If Not (ddlBank.SelectedValue = "" And cmbFile.SelectedValue = "") AndAlso clsFileSetting.fncCheckExistBankFileSetting(ddlBank.SelectedValue, cmbFile.SelectedValue) = False Then
                sMsg += clsCommon.fncMsgBankWithNoFormat(ddlBank.SelectedItem.Text, cmbFile.SelectedItem.Text) & gc_BR
            End If
            'Else
            '    If cmbFile.SelectedValue = "" Then
            '        sMsg += "Please select File Type." & gc_BR
            '    End If
            '    If Not (cmbFile.SelectedValue = "") AndAlso clsFileSetting.fncCheckExistBankFileSetting(System.Configuration.ConfigurationManager.AppSettings("Default"), cmbFile.SelectedValue) = False Then
            '        sMsg += clsCommon.fncMsgBankWithNoFormat(ddlBank.SelectedItem.Text, cmbFile.SelectedItem.Text) & gc_BR
            '    End If
            'End If
            lblMessage.Text = sMsg
            If Len(sMsg) = 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub prFormat(ByVal O As System.Object, ByVal E As System.EventArgs) Handles cmbFormat.SelectedIndexChanged

            'Variable Declarations
            Dim strFormat As String = ""
            Dim iBankID As Integer = 0
            Try
                'lblMessage.Text = ""
                'If ddlBank.SelectedValue = "" Then
                '    lblMessage.Text += "Please select Bank"
                'End If
                'If cmbFile.SelectedValue = "" Then
                '    lblMessage.Text += "Please select File Type"
                'Else
                '    strFormat = cmbFormat.SelectedValue
                'End If


                'If Len(lblMessage.Text.Trim) = 0 AndAlso clsFileSetting.fncCheckExistBankFileSetting(ddlBank.SelectedValue, cmbFile.SelectedValue) = False Then
                '    lblMessage.Text = clsCommon.fncMsgBankWithNoFormat(ddlBank.SelectedItem.Text, cmbFile.SelectedItem.Text)
                '    prcClear()



                If fncValidate() Then
                    strFormat = cmbFormat.SelectedValue
                    trExtn.Visible = True
                    trSubmit.Visible = True


                    If strFormat = "COL" Then
                        trExtn.Visible = True
                        trName.Visible = True
                        trDelim.Visible = False
                        trHeader.Visible = False
                        trFooter.Visible = False
                        'Populate Extension Drop Down List - Start
                        cmbExtn.Items.Clear()
                        cmbExtn.Items.Insert(0, New ListItem("", ""))
                        cmbExtn.Items.Insert(1, New ListItem("xls", "xls"))
                        'Populate Extension Drop Down List - Stop
                    ElseIf strFormat = "DELIM" Then
                        trExtn.Visible = True
                        trName.Visible = True
                        trDelim.Visible = True
                        trHeader.Visible = False
                        trFooter.Visible = False
                        'Populate Extension Drop Down List - Start
                        cmbExtn.Items.Clear()
                        cmbExtn.Items.Insert(0, New ListItem(""))
                        cmbExtn.Items.Insert(1, New ListItem("csv"))
                        cmbExtn.Items.Insert(2, New ListItem("txt"))
                        'Populate Extension Drop Down List - Stop
                        'Populate Delimiter Drop Down List - Start
                        cmbDelim.Items.Clear()
                        cmbDelim.Items.Insert(0, New ListItem(""))
                        cmbDelim.Items.Insert(1, New ListItem("Comma"))
                        cmbDelim.Items.Insert(2, New ListItem("Pipe"))
                        cmbDelim.Items.Insert(3, New ListItem("Tab"))
                        cmbDelim.Items.Insert(4, New ListItem("Atsign"))
                        'Populate Delimiter Drop Down List - Stop
                    ElseIf strFormat = "POS" Then
                        trExtn.Visible = True
                        trName.Visible = True
                        trDelim.Visible = False
                        trHeader.Visible = True
                        trFooter.Visible = True
                        'Populate Extension Drop Down List - Start
                        cmbExtn.Items.Clear()
                        cmbExtn.Items.Insert(0, New ListItem(""))
                        cmbExtn.Items.Insert(1, New ListItem("txt"))
                        'Populate Extension Drop Down List - Stop
                    End If
                    'If cbduplicate.Checked = False Then
                    'Populate Col/Pos Grid
                    prcDispGrid(strFormat, cmbFile.SelectedItem.Text.Trim)
                    'End If
                Else
                    prcClear()
                End If


            Catch ex As Exception
                LogError("PG_CustomerFormat - cmbFormat SelectedIndexChanged")


            End Try

        End Sub

#End Region

#Region "Message"

        Public Function fnMessage(ByVal strFieldName As String, ByVal strMesage As String) As String
            Return strFieldName & " " & strMesage
        End Function

#End Region

#Region "Save"

        '****************************************************************************************************
        'Function Name  : pfSave
        'Purpose        : Save Customer Format Details
        'Arguments      : Page Items
        'Return Value   : Ok/Error Message
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2003
        '*****************************************************************************************************
        Private Sub pfSave(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click

            'Create Instance Data grid item Object
            Dim gridItem As DataGridItem

            'Create Instance of Banl Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of Customer Class Object
            Dim clsCustomers As New MaxPayroll.clsCustomer

            'Variable Declarations
            Dim ss_lngUserId As Long, lngFileId As Long, lngCustId As Long
            Dim intCounter As Int16, strDuplicate As String, IsDuplicate As Boolean

            Try

                intCounter = 0                                                                  'Initialise Counter
                hType.Value = cmbFile.SelectedItem.Text                                         'File Type
                hDelim.Value = cmbDelim.SelectedValue                                           'Delimiter
                hFormat.Value = cmbFormat.SelectedValue                                         'Format Type
                hHeader.Value = IIf(chkHeader.Checked, "Y", "N")                                'Header Status
                hFooter.Value = IIf(chkFooter.Checked, "Y", "N")                                'Footer Status
                lngFileId = IIf(IsNumeric(hFileId.Value), hFileId.Value, 0)                     'File Id
                lngCustId = IIf(IsNumeric(hOrgId.Value), hOrgId.Value, 0)                       'Get Organisation Id
                hfEncType.Value = MaxGeneric.clsGeneric.NullToShort(ddlEncryptionType.SelectedValue)
                Dim strEncUsername As Boolean = clsCommon.CheckScriptValidation(txtFormatName.Text)
                If strEncUsername = False Then
                    Response.Write(clsCommon.ErrorCodeScript())
                    Exit Try
                End If
                If lngFileId = 0 Then
                    IsDuplicate = clsCustomers.fnOrgValidations("ADD", "FORMAT NAME", txtFormatName.Text, lngCustId, lngFileId)
                Else
                    IsDuplicate = clsCustomers.fnOrgValidations("UPDATE", "FORMAT NAME", txtFormatName.Text, lngCustId, lngFileId)
                End If

                If IsDuplicate Then
                    tblParam.Visible = True
                    trSubmit.Visible = True
                    lblMessage.Text = "Format Name already exists. Please use a different Format name."
                    Exit Try
                End If
                'Check For Duplicate Group Name - STOP

                'Delimited grid is Selected or Column Separated
                If hFormat.Value = "COL" Or hFormat.Value = "DELIM" Then

                    For Each gridItem In dgColumn.Items

                        Dim lblFieldName As Label = CType(gridItem.Cells(0).Controls(1), Label)
                        Dim txtColPos As TextBox = CType(gridItem.Cells(2).Controls(1), TextBox)


                        'Clear Error Message Label
                        lblMessage.Text = ""

                        'Check if Any Column Has Zero Or Invalid Number
                        If txtColPos.Text = 0 Then
                            lblMessage.Text = lblFieldName.Text & " Column Position Cannot Be Zero."
                            Exit Try
                        End If

                        'Check If there is Duplication of Positions
                        strDuplicate = fnCheckColumnGrid(lblFieldName, txtColPos, dgColumn, intCounter)
                        If strDuplicate <> "" Then
                            lblMessage.Text = strDuplicate
                            Exit Try
                        End If

                        'Increment Counter
                        intCounter = intCounter + 1

                    Next

                    'Insert/Update Data - Start
                    lngFileId = clsCustomers.fncCustomerFormat(dgColumn, lngFileId, hFormat.Value, lngCustId, 0, _
                                                               cmbFile.SelectedValue, hfEncType.Value)
                    If lngFileId = 0 Then
                        trBack.Visible = False
                        trSubmit.Visible = True
                        lblMessage.Text = "Organization File Settings Failed"
                    ElseIf lngFileId > 0 Then
                        trBack.Visible = True
                        trSubmit.Visible = False
                        lblMessage.Text = "Organization File Settings Successful"
                    End If
                    'Insert/Update Data - Start

                    'Position Separated Grid Was Selected
                ElseIf hFormat.Value = "POS" Then

                    '    'Position Separated Grid for LHDN
                    '    If hType.Value = "LHDN File" Then

                    '        For Each gridItem In dgPosition.Items

                    '            Dim lblFieldName As Label = CType(gridItem.Cells(0).Controls(1), Label)
                    '            Dim txtStartPosLHDN As TextBox = CType(gridItem.Cells(2).Controls(3), TextBox)
                    '            Dim txtEndPosLHDN As TextBox = CType(gridItem.Cells(3).Controls(3), TextBox)

                    '            'Clear Error Message Label
                    '            lblMessage.Text = ""

                    '            'Check if Start Position is Zero
                    '            If txtStartPosLHDN.Text = 0 Then
                    '                lblMessage.Text = lblFieldName.Text & " Start Position Cannot Be Zero."
                    '                Exit Try
                    '            End If

                    '            'Check if End Position is Zero
                    '            If txtStartPosLHDN.Text = 0 Then
                    '                lblMessage.Text = lblFieldName.Text & " End Position Cannot Be Zero."
                    '                Exit Try
                    '            End If

                    '            'Check If Start Position Is Not Greater Than End Position
                    '            If CInt(txtStartPosLHDN.Text) > CInt(txtEndPosLHDN.Text) Then
                    '                lblMessage.Text = lblFieldName.Text & " Start Position Cannot Be Greater Than End Position"
                    '                Exit Try
                    '            End If

                    '            'Check If End Position Is Not Less Than Start Position
                    '            If CInt(txtEndPosLHDN.Text) < CInt(txtStartPosLHDN.Text) Then
                    '                lblMessage.Text = lblFieldName.Text & " End Position Cannot Be Less Than Start Position"
                    '                Exit Try
                    '            End If

                    '            'Check If there is Duplication of Positions
                    '            strDuplicate = fnCheckPositionGrid(lblFieldName, txtStartPosLHDN, txtEndPosLHDN, dgPosition, intCounter)
                    '            If strDuplicate <> "" Then
                    '                lblMessage.Text = strDuplicate
                    '                Exit Try
                    '            End If

                    '            'Increment Counter
                    '            intCounter = intCounter + 1

                    '        Next
                    'If Me.cbduplicate.Checked <> True Then


                    For Each gridItem In dgPosition.Items

                        Dim lblFieldName As Label = CType(gridItem.Cells(0).Controls(1), Label)
                        Dim txtStartPos As TextBox = CType(gridItem.Cells(2).Controls(1), TextBox)
                        Dim txtEndPos As TextBox = CType(gridItem.Cells(3).Controls(1), TextBox)
                        Dim lblContentType As Label = CType(gridItem.Cells(4).Controls(1), Label)

                        'Clear Error Message Label
                        lblMessage.Text = ""

                        'Check if Start Position is Zero
                        If txtStartPos.Text = 0 Then
                            lblMessage.Text = lblFieldName.Text & " Start Position Cannot Be Zero."
                            Exit Try
                        End If

                        'Check if End Position is Zero
                        If txtEndPos.Text = 0 Then
                            lblMessage.Text = lblFieldName.Text & " End Position Cannot Be Zero."
                            Exit Try
                        End If

                        'Check If Start Position Is Not Greater Than End Position
                        If CInt(txtStartPos.Text) > CInt(txtEndPos.Text) Then
                            lblMessage.Text = lblFieldName.Text & " Start Position Cannot Be Greater Than End Position"
                            Exit Try
                        End If

                        'Check If End Position Is Not Less Than Start Position
                        If CInt(txtEndPos.Text) < CInt(txtStartPos.Text) Then
                            lblMessage.Text = lblFieldName.Text & " End Position Cannot Be Less Than Start Position"
                            Exit Try
                        End If

                        'Check If there is Duplication of Positions
                        strDuplicate = fnCheckPositionGrid(lblFieldName, txtStartPos, txtEndPos, dgPosition, intCounter, lblContentType)
                        If strDuplicate <> "" Then
                            lblMessage.Text = strDuplicate
                            Exit Try
                        End If

                        'Increment Counter
                        intCounter = intCounter + 1

                    Next
                    'End If
                    'Insert/Update Data - Start
                    lngFileId = clsCustomers.fncCustomerFormat(dgPosition, lngFileId, hFormat.Value, lngCustId, 0, _
                                                                        cmbFile.SelectedValue, hfEncType.Value)
                    If lngFileId = 0 Then
                        trBack.Visible = False
                        trSubmit.Visible = True
                        lblMessage.Text = "Organization File Settings Failed"
                    ElseIf lngFileId > 0 Then
                        trBack.Visible = True
                        trSubmit.Visible = False
                        cmbFile.Enabled = False
                        cmbFormat.Enabled = False
                        lblMessage.Text = "Organization File Settings Successful"
                    End If
                    'Insert/Update Data - Start

                End If
            Catch

                'Log Error 
                Call clsGeneric.ErrorLog(lngCustId, ss_lngUserId, "pfSave - PG_CustomerFormat", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Data Grid Item
                gridItem = Nothing

                'Destroy Instance of Customer Class Object
                clsCustomers = Nothing

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

        '****************************************************************************************************
        'Function Name  : fnCheckPositionGrid
        'Purpose        : Check for Duplicate Entries for Position Grid
        'Arguments      : Label, Start position, End Position, Counter
        'Return Value   : Message
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2003
        '*****************************************************************************************************
        Private Function fnCheckPositionGrid(ByRef lblFieldName As Label, ByRef txtStartPosition As TextBox, _
            ByRef txtEndPosition As TextBox, ByRef dgPosition As DataGrid, ByRef intCounter As Int16, ByRef lblcontenttype As Label) As String

            'Create Grid Item Object
            Dim gridItem As DataGridItem
            'Variable Declarations
            Dim intGridCounter As Int16, intStartPosition As Int16, intEndPosition As Int16
            Dim ContentType As String

            Try

                'Assign Values
                intGridCounter = 0
                intEndPosition = IIf(IsNumeric(txtEndPosition.Text), txtEndPosition.Text, 0)
                intStartPosition = IIf(IsNumeric(txtStartPosition.Text), txtStartPosition.Text, 0)
                ContentType = lblcontenttype.Text

                For Each gridItem In dgPosition.Items
                    'If Not The Same Grid Row, then Check Else Skip
                    If intGridCounter <> intCounter Then

                        Dim lblFldName As Label = CType(gridItem.Cells(0).Controls(1), Label)
                        Dim txtStartPos As TextBox = CType(gridItem.Cells(2).Controls(1), TextBox)
                        Dim txtEndPos As TextBox = CType(gridItem.Cells(3).Controls(1), TextBox)
                        Dim lblCtype As String = CType(gridItem.Cells(4).Controls(1), Label).Text

                        'Check Start Position Is not Duplicated
                        If txtStartPos.Text = intStartPosition Or txtEndPos.Text = intStartPosition Then
                            If ContentType = lblCtype Then
                                Return lblFldName.Text & " Start Position Conflicts with " & lblFieldName.Text & " Positions."
                                'Check End Position Is not Duplicated
                            End If
                        ElseIf txtStartPos.Text = intEndPosition Or txtEndPos.Text = intEndPosition Then
                            If ContentType = lblCtype Then
                                Return lblFldName.Text & " End Position Conflicts with " & lblFieldName.Text & " Positions."
                                'Check Start Position does not fall between Other Positions
                            End If
                        ElseIf txtStartPos.Text >= intStartPosition And txtStartPos.Text <= intEndPosition Then
                            If ContentType = lblCtype Then
                                Return lblFldName.Text & " Start Position falls between " & lblFieldName.Text & " Positions."
                                'Check End Position does not fall between Other Positions
                            End If
                        ElseIf txtEndPos.Text >= intStartPosition And txtEndPos.Text <= intEndPosition Then
                            If ContentType = lblCtype Then
                                Return lblFldName.Text & " End Position falls between " & lblFieldName.Text & " Positions."
                            End If
                        End If
                    End If
                    'Increment Counter
                    intGridCounter = intGridCounter + 1
                Next

            Catch ex As Exception

            End Try
            Return ""
        End Function

        '****************************************************************************************************
        'Function Name  : fnCheckColumnGrid
        'Purpose        : Check for Duplicate Entries for Column Grid
        'Arguments      : Label, Start position, End Position, Counter
        'Return Value   : Message
        'Author         : Sujith Sharatchandran - 
        'Created        : 17/10/2003
        '*****************************************************************************************************
        Private Function fnCheckColumnGrid(ByRef lblFieldName As Label, ByRef txtColPosition As TextBox, ByRef dgColumn As DataGrid, ByRef intCounter As Int16) As String

            'Create Grid Item Object
            Dim gridItem As DataGridItem
            'Variable Declarations
            Dim intGridCounter As Int16, intColPosition As Int16

            Try

                'Assign Values
                intGridCounter = 0
                intColPosition = IIf(IsNumeric(txtColPosition.Text), txtColPosition.Text, 0)

                For Each gridItem In dgColumn.Items
                    'If Not The Same Grid Row, then Check Else Skip
                    If intGridCounter <> intCounter Then

                        Dim lblFldName As Label = CType(gridItem.Cells(0).Controls(1), Label)
                        Dim txtColPos As TextBox = CType(gridItem.Cells(2).Controls(1), TextBox)

                        'Check if Same fields with same Col position-start
                        If txtColPos.Text = intColPosition Then
                            '--Added on 25-02-11
                            If lblFldName.Text = lblFieldName.Text Then
                                Return lblFldName.Text & " Column Position is Same as " & lblFieldName.Text & " Column Position."
                                'Check End Position does not fall between Other Positions
                            End If

                        End If
                        'Check if Same fields with same Col position-start

                    End If
                    'Increment Counter
                    intGridCounter = intGridCounter + 1
                Next

            Catch ex As Exception

            End Try
            Return ""
        End Function

#End Region

#Region "Get Data"

        Public Function fnGetPosition(ByVal strPosition As String, ByVal intFieldId As Int16) As String

            'Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer
            Dim dsFormat As New System.Data.DataSet
            Dim clsBank As New MaxPayroll.clsBank

            'Variable Declarations
            Dim lngFileId As Long, strFormat As String
            Dim intPosition As Int16, lngOrgId As Long, strFileType As String

            Try

                strFileType = cmbFile.SelectedItem.Text
                strFormat = cmbFormat.SelectedValue
                lngOrgId = IIf(IsNumeric(hOrgId.Value), hOrgId.Value, 0)
                lngFileId = IIf(IsNumeric(hFileId.Value), hFileId.Value, 0)

                If cbduplicate.Checked = True Then
                    intPosition = clsCustomer.fnGetPosition(strPosition, strFormat, intFieldId, strFileType, lngOrgId, lngFileId, True)
                Else
                    intPosition = clsCustomer.fnGetPosition(strPosition, strFormat, intFieldId, strFileType, lngOrgId, lngFileId)
                End If

                If intPosition > 0 Then
                    Return intPosition
                Else
                    Return ""
                End If

            Catch ex As Exception

            Finally

                'Destroy Customer Class Object
                clsCustomer = Nothing

            End Try
            Return ""
        End Function

#End Region

#Region "Populate Format"

        '****************************************************************************************************
        'Function Name  : prcFormat()
        'Purpose        : Load File Parameters
        'Arguments      : File Type, Organisation Id,User Id
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 04/03/2005
        '*****************************************************************************************************
        Private Sub prcFormat(ByVal lngOrgId As Long, ByVal lngUserId As Long, _
                                    ByVal lngFileId As Long, ByVal strUserType As String)

            'Create Instance of System Data Set
            Dim dsFormat As New System.Data.DataSet

            'Create Instance of System Data Row
            Dim drFormat As System.Data.DataRow

            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strFormat As String = ""
            Dim strFileType As String = ""
            Dim intFooterLines As Int16
            Dim strExtension As String = ""
            Dim strDelimiter As String = ""
            Dim intHeaderLines As Int16

            Try

                'Populate File Settings Parameters - Start
                dsFormat = clsCommon.fncGetRequested("File Settings", lngOrgId, lngUserId, lngFileId, "", "")
                If dsFormat.Tables("UPLOAD").Rows.Count > 0 Then
                    For Each drFormat In dsFormat.Tables("UPLOAD").Rows
                        ddlBank.SelectedValue = drFormat("BankId")
                        hidBank.Value = drFormat("BankId")
                        ddlBank.Enabled = False
                        Me.prcBindDDLFileFormat()
                        cmbFile.SelectedValue = drFormat("PaySer_Id")
                        cmbFile.SelectedItem.Text = drFormat("FTYPE")
                        hType.Value = drFormat("FTYPE")
                        strFormat = drFormat("FFORMAT")
                        hFormat.Value = strFormat
                        txtFormatName.Text = drFormat("FRNAME")
                        strExtension = drFormat("FEXTN")
                        hExtn.Value = strExtension
                        hfEncType.Value = drFormat("ENC_TYPE")
                        If strFormat = "POS" Then
                            chkHeader.Checked = IIf(drFormat("FHEADER") = "Y", True, False)
                            chkFooter.Checked = IIf(drFormat("FFOOTER") = "Y", True, False)
                            intHeaderLines = IIf(IsNumeric(drFormat("FHLINES")), drFormat("FHLINES"), 0)
                            intFooterLines = IIf(IsNumeric(drFormat("FFLINES")), drFormat("FFLINES"), 0)
                            txtHLines.Text = intHeaderLines
                            txtFLines.Text = intFooterLines
                            hHeader.Value = drFormat("FHEADER")
                            hFooter.Value = drFormat("FFOOTER")

                            'hHLine.Value = intHeaderLines
                        ElseIf strFormat = "DELIM" Then
                            strDelimiter = drFormat("FDELIM")
                        End If
                        If drFormat("ISREPLICATE") = True Then
                            cbduplicate.Checked = True
                        End If
                    Next
                End If
                'Populate File Settings Parameters - Stop

                'If Column 
                If strFormat = "COL" Then
                    trExtn.Visible = True
                    trName.Visible = True
                    trDelim.Visible = False
                    trHeader.Visible = False
                    trFooter.Visible = False
                    'Populate Extension Drop Down List - Start
                    cmbExtn.Items.Clear()
                    cmbExtn.Items.Insert(0, New ListItem("", ""))
                    cmbExtn.Items.Insert(1, New ListItem("xls", "xls"))
                    'Populate Extension Drop Down List - Stop
                ElseIf strFormat = "DELIM" Then
                    trExtn.Visible = True
                    trName.Visible = True
                    trDelim.Visible = True
                    trHeader.Visible = False
                    trFooter.Visible = False
                    'Populate Extension Drop Down List - Start
                    cmbExtn.Items.Clear()
                    cmbExtn.Items.Insert(0, New ListItem(""))
                    cmbExtn.Items.Insert(1, New ListItem("csv"))
                    cmbExtn.Items.Insert(2, New ListItem("txt"))
                    'Populate Extension Drop Down List - Stop
                    'Populate Delimiter Drop Down List - Start
                    cmbDelim.Items.Clear()
                    cmbDelim.Items.Insert(0, New ListItem(""))
                    cmbDelim.Items.Insert(1, New ListItem("Comma"))
                    cmbDelim.Items.Insert(2, New ListItem("Pipe"))
                    cmbDelim.Items.Insert(3, New ListItem("Tab"))
                    cmbDelim.Items.Insert(4, New ListItem("Atsign"))
                    cmbDelim.SelectedValue = strDelimiter
                    'Populate Delimiter Drop Down List - Stop
                ElseIf strFormat = "POS" Then
                    trExtn.Visible = True
                    trName.Visible = True
                    trDelim.Visible = False
                    trHeader.Visible = True
                    trFooter.Visible = True
                    'Populate Extension Drop Down List - Start
                    cmbExtn.Items.Clear()
                    cmbExtn.Items.Insert(0, New ListItem(""))
                    cmbExtn.Items.Insert(1, New ListItem("txt"))
                    'Populate Extension Drop Down List - Stop
                End If

                trSubmit.Visible = True
                hFormat.Value = strFormat
                strFileType = cmbFile.SelectedItem.Text

                hType.Value = strFileType
                hDelim.Value = cmbDelim.SelectedValue
                hExtn.Value = strExtension
                cmbFormat.SelectedValue = strFormat
                cmbExtn.SelectedValue = strExtension

                ddlEncryptionType.SelectedValue = hfEncType.Value

                'Disable Controls - Start
                cmbFile.Enabled = False
                cmbExtn.Enabled = False
                cmbDelim.Enabled = False
                cmbFormat.Enabled = False
                'Disable Controls - Stop

                'If cbduplicate.Checked = False Then
                Call prcDispGrid(strFormat, strFileType)
                'End If
            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "prcFormat - PG_CustomerFormat", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of Data Set
                dsFormat = Nothing

                'Destroy Instance of System Data Row
                drFormat = Nothing

            End Try

        End Sub

#End Region

#Region "Clear Values"


        Private Sub prcClear()

            Try

                trExtn.Visible = False
                txtFormatName.Text = ""
                trDelim.Visible = False
                dgColumn.Visible = False
                trHeader.Visible = False
                trFooter.Visible = False
                trSubmit.Visible = False
                dgPosition.Visible = False
                cmbFormat.SelectedValue = ""

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Display Grid"

        Private Sub prcDispGrid(ByVal strFormat As String, ByVal strFileType As String)

            'Create Instance of System Data Set
            Dim dsFormat As New System.Data.DataSet

            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim lngUserId As Long, lngCustId As Long, iBankID As Integer

            'Create Instance of Datagrid Items

            Try

                'Populate Data Set
                iBankID = Me.ddlBank.SelectedValue

                dsFormat = clsBank.fnBodyFields(strFileType, intOrgID, iBankID)
                If dsFormat.Tables(0).Rows.Count = 0 Then
                    Me.btnSave.Visible = False
                    lblMessage.Text = "Organization has no account in bank [" & ddlBank.SelectedItem.Text & "] for the payment type [" & cmbFile.SelectedItem.Text & "]"
                    Exit Try
                End If

                If strFormat = "COL" Or strFormat = "DELIM" Then
                    dgColumn.DataSource = dsFormat
                    dgColumn.DataBind()
                    dgColumn.Enabled = True
                    dgColumn.Visible = True
                    dgPosition.Visible = False
                    dgPosition.Enabled = False
                ElseIf strFormat = "POS" Then
                    dgPosition.DataSource = dsFormat
                    dgPosition.DataBind()
                    dgColumn.Visible = False
                    dgPosition.Enabled = True
                    dgPosition.Visible = True


                End If


                ''Populate the Position Separated column for LHDN defaulted value
                'If strFileType = "LHDN File" Then

                '    For Each dgiPosition In dgPosition.Items
                '        CType(dgiPosition.FindControl("txtStartPos"), TextBox).Visible = False
                '        CType(dgiPosition.FindControl("txtEndPos"), TextBox).Visible = False
                '        CType(dgiPosition.FindControl("txtStartPosLHDN"), TextBox).Visible = True
                '        CType(dgiPosition.FindControl("txtEndPosLHDN"), TextBox).Visible = True
                '    Next

                'Else

                '    For Each dgiPosition In dgPosition.Items
                '        CType(dgiPosition.FindControl("txtStartPos"), TextBox).Visible = True
                '        CType(dgiPosition.FindControl("txtEndPos"), TextBox).Visible = True
                '        CType(dgiPosition.FindControl("txtStartPosLHDN"), TextBox).Visible = False
                '        CType(dgiPosition.FindControl("txtEndPosLHDN"), TextBox).Visible = False
                '    Next

                'End If

            Catch ex As Exception

                'Log Error 
                Call clsGeneric.ErrorLog(lngCustId, lngUserId, "prcDispGrid - PG_CustomerFormat", Err.Number, Err.Description)

            Finally

                'Destroy Instance of System Data Set
                dsFormat = Nothing

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

            End Try

        End Sub

#End Region

        Protected Sub ddlBank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlBank.SelectedIndexChanged
            Me.hidBank.Value = ddlBank.SelectedValue
            prcBindDDLFileFormat()
            cmbFile.SelectedIndex = 0
            prcClear()
        End Sub

        Private Sub prcBindDDLFileFormat()
            Dim oPaymentService As New clsPaymentService
            cmbFile.Items.Clear()

            If ddlBank.Visible Then
                If IsNumeric(ddlBank.SelectedValue) Then
                    cmbFile.DataSource = oPaymentService.fncDDLPayment(intOrgID, ddlBank.SelectedValue)
                End If
            Else
                cmbFile.DataSource = oPaymentService.fncDDLPayment(intOrgID, CInt(fncAppSettings("DefaultBankCode")))
            End If
            cmbFile.DataTextField = "FTYPE"
            cmbFile.DataValueField = "PAYID"
            cmbFile.DataBind()
            cmbFile.Items.Insert(0, New ListItem("Select", ""))
        End Sub

        'Public Sub DispGrid()

        '    prcDispGrid(strFormat, cmbFile.SelectedItem.Text.Trim)
        'End Sub      


    End Class
End Namespace
