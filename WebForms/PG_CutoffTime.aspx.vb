Option Strict Off
Option Explicit On 

Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient


Namespace MaxPayroll


Partial Class PG_CutoffTime

      Inherits clsBasePage

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of User Class Object
            Dim clsUsers As New MaxPayroll.clsUsers

            'Put user code to initialize the page here
            Dim intCounter As Int16, strItem As String

            Try
                If Not Session(gc_Ses_UserType) = gc_UT_BankUser Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                If Not Page.IsPostBack Then
                    prcBindBank()

                    clsCommon.fncDefaultBankChecking(cmbBank, lblBank)

                    'BindBody(body)
                    'Populate the Hour Combo Box - Start
                    cmbHour.Items.Insert(0, New ListItem("00"))
                    cmbPHour.Items.Insert(0, New ListItem("00"))
                    cmbEHour.Items.Insert(0, New ListItem("00"))
                    cmbSHour.Items.Insert(0, New ListItem("00"))
                    cmbLHour.Items.Insert(0, New ListItem("00"))
                    cmbDHour.Items.Insert(0, New ListItem("00"))
                    cmbZHour.Items.Insert(0, New ListItem("00"))
                    cmbCHour.Items.Insert(0, New ListItem("00"))
                    cmbIhour.Items.Insert(0, New ListItem("00"))

                    For intCounter = 1 To 12
                        If intCounter >= 10 Then
                            strItem = intCounter
                        Else
                            strItem = "0" & intCounter
                        End If
                        cmbHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbPHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbEHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbSHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbLHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbDHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbZHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbCHour.Items.Insert(intCounter, New ListItem(strItem))
                        cmbIhour.Items.Insert(intCounter, New ListItem(strItem))
                    Next
                    'Populate the Hour Combo Box - Stop

                    'Populate the Min Combo Box - Start
                    cmbMin.Items.Insert(0, New ListItem("00"))
                    cmbPMin.Items.Insert(0, New ListItem("00"))
                    cmbEMin.Items.Insert(0, New ListItem("00"))
                    cmbSMin.Items.Insert(0, New ListItem("00"))
                    cmbLMin.Items.Insert(0, New ListItem("00"))
                    cmbDMin.Items.Insert(0, New ListItem("00"))
                    cmbZMin.Items.Insert(0, New ListItem("00"))
                    cmbCMin.Items.Insert(0, New ListItem("00"))
                    cmbImin.Items.Insert(0, New ListItem("00"))
                    For intCounter = 1 To 59
                        If intCounter >= 10 Then
                            strItem = intCounter
                        Else
                            strItem = "0" & intCounter
                        End If
                        cmbMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbPMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbEMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbSMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbLMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbDMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbZMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbCMin.Items.Insert(intCounter, New ListItem(strItem))
                        cmbImin.Items.Insert(intCounter, New ListItem(strItem))
                    Next
                    'Populate the Min Combo Box - Stop

                    'Populate the Type Combo Box - Start
                    cmbType.Items.Insert(0, New ListItem("AM"))
                    cmbType.Items.Insert(1, New ListItem("PM"))
                    cmbPType.Items.Insert(0, New ListItem("AM"))
                    cmbPType.Items.Insert(1, New ListItem("PM"))
                    cmbEType.Items.Insert(0, New ListItem("AM"))
                    cmbEType.Items.Insert(1, New ListItem("PM"))
                    cmbSType.Items.Insert(0, New ListItem("AM"))
                    cmbSType.Items.Insert(1, New ListItem("PM"))
                    cmbLType.Items.Insert(0, New ListItem("AM"))
                    cmbLType.Items.Insert(1, New ListItem("PM"))
                    cmbDType.Items.Insert(0, New ListItem("AM"))
                    cmbDType.Items.Insert(1, New ListItem("PM"))
                    cmbZType.Items.Insert(0, New ListItem("AM"))
                    cmbZType.Items.Insert(1, New ListItem("PM"))
                    cmbCType.Items.Insert(0, New ListItem("AM"))
                    cmbCType.Items.Insert(1, New ListItem("PM"))
                    cmbIType.Items.Insert(0, New ListItem("AM"))
                    cmbIType.Items.Insert(1, New ListItem("PM"))
                    'Populate the Type Combo Box - Stop

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)
                    Call prGetCutOffTime("I", cmbBank.SelectedValue)


                    Call clsUsers.prcDetailLog(ss_lngUserID, "View Cutoff Time", "Y")

                End If



            Catch ex As Exception

                Me.lblMessage.Text = ex.Message

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of User Class Object
                clsUsers = Nothing

            End Try

        End Sub

#End Region

#Region "Page Submit"

        Public Sub prSave(ByVal S As System.Object, ByVal E As System.EventArgs) Handles btnSave.Click

            If Not cmbBank.SelectedIndex = 0 Then

                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Declare Variable
                Dim strResult As String, strHour As String, strMin As String, strType As String
                Dim strPHour As String, strPMin As String, strPType As String, lngUserId As Long

                Try

                    strMin = cmbMin.SelectedValue
                    strHour = cmbHour.SelectedValue
                    strType = cmbType.SelectedValue

                    strPMin = cmbPMin.SelectedValue
                    strPHour = cmbPHour.SelectedValue
                    strPType = cmbPType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("N", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "Payroll Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(lngUserId, "Payroll Submission Cutoff Time", "N")
                        Else
                            lblMessage.Text = "Payroll Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(lngUserId, "Payroll Submission Cutoff Time", "Y")
                        End If
                    End If

                    If Not strPHour = "00" Then
                        strResult = fnCutoffTime("P", strPHour, strPMin, strPType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "Payroll Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(lngUserId, "Payroll Submission Privilege Cutoff Time", "N")
                        Else
                            lblMessage.Text = "Payroll Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(lngUserId, "Payroll Submission Privilege Cutoff Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please Select a Bank."
            End If

        End Sub

#End Region

#Region "Database Interaction (Save Cut Off Time)"

        Private Function fnCutoffTime(ByVal strOption As String, ByVal strHour As String, ByVal strMin As String, ByVal strType As String, ByVal strBankId As String) As String

            'Create Instance of SQL Command Object
            Dim cmdTime As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Declare Variables
            Dim strMessage As String

            Try

                'Create SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Insert/Update Cutoff Time - Start
                With cmdTime
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = "pg_CutoffTime"
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@in_Option", strOption))
                    .Parameters.Add(New SqlParameter("@in_Hour", strHour))
                    .Parameters.Add(New SqlParameter("@in_Min", strMin))
                    .Parameters.Add(New SqlParameter("@in_Type", strType))
                    '071024 1 line added by Marcus
                    'Purpose: Save 1 more parameter to Cut Off Time table.
                    .Parameters.Add(New SqlParameter("@in_BankId", strBankId))
                    .ExecuteNonQuery()
                End With
                strMessage = gc_Status_OK
                'Insert/Update Cutoff Time - Stop

                Return strMessage

            Catch

                'Log Error
                LogError("pg_CutoffTime - fnCutoffTime")

                Return gc_Status_Error

            Finally

                'Destroy SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy SQL Command Object
                cmdTime = Nothing

                'Destroy Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Get Cutoff Time"


        Sub prGetCutOffTime(ByVal strOption As String, ByVal strBankID As String)

            'Create Instance of SQL Data Reader    
            Dim sdrTime As SqlDataReader

            'Create Instance of generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim SQLStatement As String = ""
            Dim strHour As String = ""
            Dim strMinutes As String = ""
            Dim strType As String = ""

            Try
                'Intialize SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                SQLStatement = "Exec pg_GetTime '" & strOption & "','" & strBankID & "'"

                Dim cmdTime As New SqlCommand(SQLStatement, clsGeneric.SQLConnection)
                sdrTime = cmdTime.ExecuteReader(CommandBehavior.CloseConnection)

                Dim bHasRow As Boolean
                'If Record Found get Assigned Weight
                If sdrTime.HasRows Then
                    sdrTime.Read()
                    strHour = sdrTime("HH")
                    strMinutes = sdrTime("MM")
                    strType = sdrTime("TF")
                    sdrTime.Close()
                    bHasRow = True
                End If

                cmdTime = Nothing

                '071024 If Else statement modified by Marcus
                'Purpose: to display "N/A" when cut off time not set.

                If strOption = "N" Then

                    If bHasRow Then
                        lblTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblTime.Text = "N/A"
                    End If

                ElseIf strOption = "P" Then

                    If bHasRow Then
                        lblPTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblPTime.Text = "N/A"
                    End If

                ElseIf strOption = "E" Then

                    If bHasRow Then
                        lblETime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblETime.Text = "N/A"
                    End If

                ElseIf strOption = "S" Then
                    If bHasRow Then
                        lblSTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblSTime.Text = "N/A"
                    End If
                ElseIf strOption = "L" Then

                    If bHasRow Then
                        lblLTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblLTime.Text = "N/A"
                    End If

                ElseIf strOption = "D" Then

                    If bHasRow Then
                        lbldTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lbldTime.Text = "N/A"
                    End If

                ElseIf strOption = "Z" Then

                    If bHasRow Then
                        lblZTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblZTime.Text = "N/A"
                    End If
                ElseIf strOption = "C" Then

                    If bHasRow Then
                        lblCTime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblCTime.Text = "N/A"
                    End If
                ElseIf strOption = "I" Then

                    If bHasRow Then
                        lblITime.Text = strHour & ":" & strMinutes & " " & strType
                    Else
                        lblITime.Text = "N/A"
                    End If

                End If

            Catch e As Exception

                Me.lblMessage.Text = e.Message
            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                sdrTime = Nothing
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

#Region "Clear"

    Private Sub prClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click

        Server.Transfer("PG_Cutofftime.aspx", False)

    End Sub

    Private Sub prEClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEClear.Click

        Server.Transfer("PG_Cutofftime.aspx", False)

    End Sub

    Private Sub prSClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSClear.Click

        Server.Transfer("PG_Cutofftime.aspx", False)

    End Sub

    Private Sub prLClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLClear.Click

        Server.Transfer("PG_Cutofftime.aspx", False)

        End Sub

        Private Sub prZClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnZClear.Click

            Server.Transfer("PG_Cutofftime.aspx", False)

        End Sub

        Private Sub prCClear(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCClear.Click

            Server.Transfer("PG_Cutofftime.aspx", False)

        End Sub

        Protected Sub btnDClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDClear.Click
            Server.Transfer("PG_Cutofftime.aspx", False)
        End Sub

#End Region

#Region "EPF Submit"


        Private Sub prcEPF(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnEPF.Click

            If Not cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbEMin.SelectedValue
                    strHour = cmbEHour.SelectedValue
                    strType = cmbEType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("E", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "EPF Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "EPF Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "EPF Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "EPF Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else

                lblMessage.Text = "Please Select a Bank."
            End If
        End Sub

#End Region

#Region "SOCSO Submit"

    Private Sub prcSocso(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSocso.Click
            If Not cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbSMin.SelectedValue
                    strHour = cmbSHour.SelectedValue
                    strType = cmbSType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("S", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "SOCSO Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "EPF Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "SOCSO Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "EPF Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please select a Bank."
            End If
        End Sub

#End Region

#Region "LHDN Submit"


        Private Sub prcLHDN(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnLHDN.Click

            If Not Me.cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbLMin.SelectedValue
                    strHour = cmbLHour.SelectedValue
                    strType = cmbLType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("L", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "LHDN Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "LHDN Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "LHDN Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "LHDN Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please select a Bank"
            End If

        End Sub

#End Region

#Region "Zakat Submit"


        Private Sub prcZAKAT(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnZAKAT.Click

            If Not Me.cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbZMin.SelectedValue
                    strHour = cmbZHour.SelectedValue
                    strType = cmbZType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("Z", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "ZAKAT Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "ZAKAT Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "ZAKAT Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "ZAKAT Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please select a Bank"
            End If

        End Sub

#End Region

#Region "CPS Submit"


        Private Sub prcCPS(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnCPS.Click

            If Not Me.cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbCMin.SelectedValue
                    strHour = cmbCHour.SelectedValue
                    strType = cmbCType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("C", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "CPS Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "ZAKAT Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "CPS Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "CPS Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please select a Bank"
            End If

        End Sub

#End Region

#Region "Bank drop down event handler"
        Protected Sub cmbBank_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbBank.SelectedIndexChanged

            Call prGetCutOffTime("N", cmbBank.SelectedValue)
            Call prGetCutOffTime("P", cmbBank.SelectedValue)
            Call prGetCutOffTime("E", cmbBank.SelectedValue)
            Call prGetCutOffTime("S", cmbBank.SelectedValue)
            Call prGetCutOffTime("L", cmbBank.SelectedValue)
            Call prGetCutOffTime("D", cmbBank.SelectedValue)

        End Sub
#End Region

#Region "Bind bank drop down list"
        Private Sub prcBindBank()
            Dim clsBankMF As New clsBankMF

            Me.cmbBank.DataSource = clsBankMF.fncRetrieveBankCodeName(clsCommon.fncGetPrefix(enmStatus.A_Active))
            Me.cmbBank.DataTextField = "BankName"
            Me.cmbBank.DataValueField = "BankID"

            cmbBank.DataBind()
            Me.cmbBank.Items.Insert(0, "--Select--")

        End Sub
#End Region

#Region "Direct Debit Submit"

        Protected Sub prcDirecDebit(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDirectDebit.Click

            If Not Me.cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbDMin.SelectedValue
                    strHour = cmbDHour.SelectedValue
                    strType = cmbDType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("D", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "Direct Debit Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Direct Debit Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "Direct Debit Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Direct Debit Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please select a Bank"
            End If
        End Sub
#End Region

#Region " PayLink Submit"

        Protected Sub prcPayLink(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInfenion.Click

            If Not Me.cmbBank.SelectedIndex = 0 Then
                'Create Instance of User Class Object
                Dim clsUsers As New MaxPayroll.clsUsers

                'Variable Declaration
                Dim strHour As String, strMin As String, strType As String, strResult As String

                Try

                    strMin = cmbImin.SelectedValue
                    strHour = cmbIhour.SelectedValue
                    strType = cmbIType.SelectedValue

                    If Not strHour = "00" Then
                        strResult = fnCutoffTime("I", strHour, strMin, strType, cmbBank.SelectedValue)
                        If Not strResult = gc_Status_OK Then
                            lblMessage.Text = "Paylink Submission Cut off Time Failed."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Paylink Submission Cut off Time", "N")
                        Else
                            lblMessage.Text = "Paylink Submission Cut off Time Successfully Set."
                            Call clsUsers.prcDetailLog(ss_lngUserID, "Paylink Submission Cut off Time", "Y")
                        End If
                    End If

                    Call prGetCutOffTime("N", cmbBank.SelectedValue)
                    Call prGetCutOffTime("P", cmbBank.SelectedValue)
                    Call prGetCutOffTime("E", cmbBank.SelectedValue)
                    Call prGetCutOffTime("S", cmbBank.SelectedValue)
                    Call prGetCutOffTime("L", cmbBank.SelectedValue)
                    Call prGetCutOffTime("D", cmbBank.SelectedValue)
                    Call prGetCutOffTime("Z", cmbBank.SelectedValue)
                    Call prGetCutOffTime("C", cmbBank.SelectedValue)
                    Call prGetCutOffTime("I", cmbBank.SelectedValue)

                Catch ex As Exception

                Finally

                    'Destroy Instance of Users Class Object
                    clsUsers = Nothing

                End Try
            Else
                lblMessage.Text = "Please select a Bank"
            End If
        End Sub
#End Region
      
    End Class
End Namespace