Imports System.Data
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient
Imports MaxModule
Imports MaxGeneric
Imports MaxMiddleware


Namespace MaxPayroll

    Partial Class WebForms_pg_SetCutOffTime
        Inherits clsBasePage

#Region " Global declarations"

        Dim _clsGeneric As New MaxPayroll.Generic
        Dim clsUsers As New MaxPayroll.clsUsers
        Dim _MaxdataBase As New MaxModule.DataBase
        Private _Helper As New Helper
        Private Shared __cutoff_Identity As String = Nothing

#End Region

#Region "Get Service Types "

        Private Function GetServiceTypes() As DataTable

            Return PPS.GetData(_Helper.GetSQLServiceType & 0 & "," & 0, _
                  _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

        End Function

#End Region

#Region "Get Service Type Id "

        Private Function GetServiceTypeId() As Short
            Return clsGeneric.NullToShort(Request.QueryString("PayserId"))
        End Function

#End Region

#Region "Get Cutoff Id "

        Private Function GetCutoffId() As Short
            Return clsGeneric.NullToShort(Request.QueryString("CutoffId"))
        End Function

#End Region

#Region "Get Cutoff_Identity "

        Private Sub GetCutoffIdentity()

            __cutoff_Identity = clsGeneric.NullToString(Request.QueryString("CutoffIdentity"))

        End Sub

#End Region

#Region "Get Cut Off Times "

        Private Function GetCutOffTimes(ByVal PayserId As Short, _
                   ByVal CutoffId As Short) As DataTable

            Return PPS.GetData(_Helper.GetSQLFileTypeCutoff & PayserId _
                  & MaxGeneric.clsGeneric.AddComma & CutoffId, _
                    _Helper.GetSQLConnection, _Helper.GetSQLTransaction)

        End Function

#End Region

#Region "Page Load"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'Variable declaration-start
            Dim intCounter As Integer = 0, strItem As String = Nothing
            'Variable declaration-stop
            Try

                If Not Session(gc_Ses_UserType) = gc_UT_BankUser Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                'If not page postback -start
                If Not Page.IsPostBack Then
                    prcBindBank()

                    clsCommon.fncDefaultBankChecking(__BankId, lblBank)

                    'Populate Cut Off Times - Start
                    Call PopulateCutOffMins(__Cutoff_Min)
                    Call PopulateCutOffHours(__Cutoff_Hour)
                    'Populate Cut Off Times - Stop           

                    FormHelp.PopulateDropDownList(GetServiceTypes, __PaySer_Id, "Service_Desc", "Service_Id")

                    'Bind Cutofftime Grid
                    Call DataGridItemBind()

                    If GetServiceTypeId() > 0 Then
                        'Get Cutoff Identity from the query string 
                        GetCutoffIdentity()
                        Call Modify()

                    End If

                    'log details
                    Call clsUsers.prcDetailLog(ss_lngUserID, "View Cutoff Time", "Y")
                End If
                'If not page postback -stop               

            Catch ex As Exception
                Me.lblMessage.Text = ex.Message
            End Try

        End Sub

#End Region

#Region "Bind bank drop down list"

        Private Sub prcBindBank()

            'create instances -start
            Dim clsBankMF As New clsBankMF, BankCodes As DataSet = Nothing
            'create instances -start

            BankCodes = clsBankMF.fncRetrieveBankCodeName(clsCommon.fncGetPrefix(enmStatus.A_Active))

            FormHelp.PopulateDropDownList(BankCodes.Tables(0), __BankId, "BankName", "BankID")

            __BankId.Items.Insert(0, "--Select--")

        End Sub
#End Region

#Region "Data Grid Item Bind "

        Private Sub DataGridItemBind()

            'Populate Data Grid
            Call FormHelp.PopulateDataGrid(GetCutOffTimes(0, 0), dgCutoffTime)

            'Datagrid Item Management
            Call DataGridItemManagement()

        End Sub

#End Region

#Region "Data Grid Item Management "

        Private Sub DataGridItemManagement()

            'Create Instances - Start
            Dim ModifyLink As HyperLink = Nothing
            Dim _DataGridItem As DataGridItem = Nothing
            'Create Instances - Stop

            'Variable Declarations
            Dim ServiceTypeId As Short = 0, CutoffIdentity As String = Nothing
            Dim CutoffId As Short = 0

            Try

                'loop thro the data grid items - Start
                For Each _DataGridItem In dgCutoffTime.Items

                    'Get Values - Start
                    ServiceTypeId = clsGeneric.NullToShort(_DataGridItem.Cells(0).Text)
                    CutoffId = clsGeneric.NullToShort(_DataGridItem.Cells(1).Text)
                    CutoffIdentity = clsGeneric.NullToString(_DataGridItem.Cells(6).Text)
                    'Get Values - Stop

                    'Get Controls - Start
                    Call FormHelp.GetDataGridControl(_DataGridItem, "Modify_Url", ModifyLink)
                    'Get Controls - Stop

                    'Set Hyper Link Attributes - Start
                    ModifyLink.NavigateUrl = "pg_SetCutOffTime.aspx?" & "PayserId=" & ServiceTypeId _
                        & "&CutoffId=" & CutoffId & "&CutoffIdentity=" & CutoffIdentity
                    'Set Hyper Link Attributes - Stop

                Next
                'loop thro the data grid items - Stop

            Catch ex As Exception

                'Log Error - Start              

            End Try

        End Sub

#End Region

#Region "Modify "

        Private Sub Modify()

            Try

                Dim _ContentPlaceHolder As ContentPlaceHolder = Nothing

                _ContentPlaceHolder = Me.Form.FindControl("cphContent")

                'Load Details
                Call DataHelp.DataToPage(GetCutOffTimes(GetServiceTypeId(), _
                       GetCutoffId()), _ContentPlaceHolder)


            Catch ex As Exception

            End Try

        End Sub
#End Region

#Region "Populate CutOff Mins"


        Public Sub PopulateCutOffMins(ByRef _DropDownList As DropDownList)

            'Variable Declaration
            Dim MinsCounterString As String = Nothing

            'Loop thro the Mins Counter to 60(Mins) and Add those values to Drop Down - Start
            For MinsCounter As Integer = 0 To 60

                MinsCounterString = clsGeneric.NullToString(clsGeneric.Filler(MinsCounter.ToString(), "0", 2, True))
                _DropDownList.Items.Add(New ListItem(MinsCounterString, MinsCounterString))
            Next
            'Loop thro the Mins Counter to 60(Mins) and Add those values to Drop Down - Stop

        End Sub

#End Region

#Region " Populate CutOff Hours "


        Public Sub PopulateCutOffHours(ByRef _DropDownList As DropDownList)

            'Variable Declaration
            Dim HoursCounterString As String = Nothing

            'Loop thro the Mins Counter to 60(Mins) and Add those values to Drop Down - Start
            For HoursCounter As Integer = 1 To 12
                HoursCounterString = clsGeneric.NullToString(clsGeneric.Filler(HoursCounter.ToString(), "0", 2, True))
                _DropDownList.Items.Add(New ListItem(HoursCounterString, HoursCounterString))
            Next
            'Loop thro the Mins Counter to 60(Mins) and Add those values to Drop Down - Stop

        End Sub

#End Region

#Region " Page submit "

        Protected Sub btnSubmit_Click(sender As Object, e As System.EventArgs) Handles btnSubmit.Click

            'Variables declarations-start
            Dim PageData As DataTable = Nothing, CatchMessage As String = Nothing
            Dim SqlStatement As String = Nothing, CutoffIdentity As String = Nothing
            'Variables declarations-stop
            Try

                'If New record get the cutoffIdentity basedon FileType-start
                If GetCutoffId() = 0 Then

                    CutoffIdentity = GetFileTypeCutoffIdentity _
                        (clsGeneric.NullToString(__PaySer_Id.SelectedItem.Text))
                Else
                    CutoffIdentity = clsGeneric.NullToString(__cutoff_Identity)
                End If
                'If New record get the cutoffIdentity basedon FileType-stop

                'Build SqlStatement -start
                SqlStatement = _Helper.SQLCutoffTime & GetCutoffId() & clsGeneric.AddComma & __PaySer_Id.SelectedValue & _
                    clsGeneric.AddComma & clsGeneric.AddQuotes(__Cutoff_Hour.SelectedItem.Text) & clsGeneric.AddComma & _
                         clsGeneric.AddQuotes(__Cutoff_Min.SelectedItem.Text) & clsGeneric.AddComma & clsGeneric.AddQuotes _
                            (__Cutoff_Type.SelectedItem.Text) & clsGeneric.AddComma & __BankId.SelectedValue & clsGeneric.AddComma _
                                & clsGeneric.AddQuotes(CutoffIdentity)
                'Build SqlStatement -stop

                'Execute Sql Statement
                PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMessage, SqlStatement)

                'Check if any error -start
                If FormHelp.IsBlank(CatchMessage) Then
                    lblMessage.Text = " Record saved successfully "
                Else
                    lblMessage.Text = " Record Insertion/modification failed "
                End If
                'Check if any error -stop

                'Rebind the datagrid
                Call DataGridItemBind()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Get cutoff Identity based on FileTypeName "

        Public Function GetFileTypeCutoffIdentity(ByVal FileTypeName As String) As String

            'Variables declaration
            Dim CutoffIdentity As String = Nothing

            Try

                Select Case FileTypeName

                    Case _Helper.Autopay_Name
                        CutoffIdentity = "P"
                    Case _Helper.AutopaySNA_Name
                        CutoffIdentity = "P"
                    Case _Helper.HybridDirectDebit_Name
                        CutoffIdentity = "D"
                    Case _Helper.PayLinkPayRoll_Name
                        CutoffIdentity = "I"
                    Case _Helper.CPS_Name
                        CutoffIdentity = "C"

                End Select

                Return CutoffIdentity

            Catch ex As Exception

                'On exception return nothing
                Return Nothing

            End Try

        End Function

#End Region

    End Class

End Namespace