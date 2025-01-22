Imports MaxMiddleware
Imports MaxGeneric

Namespace MaxPayroll

    Partial Class WebForms_pg_InfenionBNMCodes
        Inherits clsBasePage

#Region "Global Declarations "

        Private _Helper As New Helper
        Dim _clsGeneric As New MaxPayroll.Generic

#End Region

#Region "Get Rec Id "

        Private Function GetRecId() As Short

            Return clsGeneric.NullToShort(Request.QueryString("RecId"))

        End Function
#End Region

#Region "Get BankName "

        Private Function GetBankName() As String

            Return clsGeneric.NullToString(Request.QueryString("BankName"))

        End Function
#End Region

#Region "Get RentasBankCode "

        Private Function GetRentasBankCode() As String

            Return clsGeneric.NullToString(Request.QueryString("RentasBankCode"))

        End Function
#End Region

#Region "Get RentasBIC "

        Private Function GetRentasBIC() As String

            Return clsGeneric.NullToString(Request.QueryString("RentasBIC"))

        End Function
#End Region

#Region "Get GIRORoutingCode"

        Private Function GetGIRORoutingCode() As String

            Return clsGeneric.NullToString(Request.QueryString("GIRORoutingCode"))

        End Function
#End Region

#Region "Get BNMCode "

        Private Function GetBNMCode() As String

            Return clsGeneric.NullToString(Request.QueryString("BNMCode"))

        End Function
#End Region

#Region "Page Load"

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            Try

                If Page.IsPostBack = False Then

                    'Check if bank admin-start
                    If Not ss_strUserType = gc_UT_BankAdmin Then
                        Response.Clear()
                        Response.Redirect(gc_LogoutPath, False)
                    End If
                    'Check if bank admin-start

                    'Log details -start
                    Dim clsUsers As New clsUsers
                    Call clsUsers.prcDetailLog(CInt(Session(gc_Ses_UserID)), "View BNMCodes Page", "Y")
                    'Log details -stop

                    'Bind BNM COdes-start
                    BindBNMCodes()
                    'Bind BNM COdes data-stop

                    If GetRecId() > 0 Then

                        hfRecId.Value = GetRecId()
                        txtBankName.Text = GetBankName()
                        txtRentasBankCode.Text = GetRentasBankCode()
                        txtRentasBIC.Text = GetRentasBIC()
                        txtGIRORoutingCode.Text = GetGIRORoutingCode()
                        txtBNMCode.Text = GetBNMCode()

                    End If

                End If

            Catch ex As Exception

                _clsGeneric.ErrorLog("fnLogin - PG_InfenionBNMCodes.aspx.vb", Err.Number, Err.Description)

            End Try


        End Sub

#End Region

#Region "  Bind BNM Codes "

        Private Sub BindBNMCodes()

            'Variables declarations-start
            Dim dsSearch As New DataTable
            Dim CatchMessage As String = Nothing, _DataGridItem As DataGridItem
            Dim RecId As Short = 0, BankName As String = Nothing, RentasBankCode As String = Nothing
            Dim RentasBIC As String = Nothing, GIRORoutingCode As String = Nothing
            Dim BNMCode As String = Nothing, NavigateUrl As String = Nothing
            Dim _HyperLink As HyperLink = Nothing
            'Variables declarations-stop

            'dsSearch = clsDatabase.fncDisplayBankDef(txtBankCode.Text)
            dsSearch = PPS.GetData(_Helper.GetSQLBNMCodes, _Helper.GetSQLConnection, _Helper.GetSQLTransaction)
            If dsSearch.Rows.Count > 0 Then

                'Populate Data Grid
                Call FormHelp.PopulateDataGrid(dsSearch, dgBNMCodes)
                'lblErrorMessage.Text = ""

                'Loop thro the Data Grid Items - Start
                For Each _DataGridItem In dgBNMCodes.Items

                    'Get Values - Start
                    RecId = clsGeneric.NullToShort(_DataGridItem.Cells(0).Text)
                    BankName = clsGeneric.NullToString(_DataGridItem.Cells(1).Text)
                    RentasBankCode = clsGeneric.NullToString(_DataGridItem.Cells(2).Text)
                    RentasBIC = clsGeneric.NullToString(_DataGridItem.Cells(3).Text)
                    GIRORoutingCode = clsGeneric.NullToString(_DataGridItem.Cells(4).Text)
                    BNMCode = clsGeneric.NullToString(_DataGridItem.Cells(5).Text)
                    'Get Values - Stop

                    'Build Modify URL - Start
                    Call FormHelp.GetDataGridControl(_DataGridItem, "hlLink", _HyperLink)

                    'Build Navigation URL - Start
                    NavigateUrl = GetModuleUrl()
                    NavigateUrl &= "RecId" & clsGeneric.AddEqual & RecId
                    NavigateUrl &= clsGeneric.AddQueryString & "BankName"
                    NavigateUrl &= clsGeneric.AddEqual & BankName & clsGeneric.AddQueryString
                    NavigateUrl &= "RentasBankCode" & clsGeneric.AddEqual & RentasBankCode & clsGeneric.AddQueryString
                    NavigateUrl &= "RentasBIC" & clsGeneric.AddEqual & RentasBIC
                    NavigateUrl &= clsGeneric.AddQueryString & "GIRORoutingCode" & clsGeneric.AddEqual & GIRORoutingCode
                    NavigateUrl &= clsGeneric.AddQueryString & "BNMCode" & clsGeneric.AddEqual & BNMCode
                    'Build Navigation URL - Stop

                    'Set URL
                    _HyperLink.NavigateUrl = NavigateUrl

                Next
                'Loop thro the Data Grid Items - Stop
            Else
                Me.pnlGrid.Visible = False
                dgBNMCodes.Visible = False
                lblErrorMessage.Text = "No records available."

            End If
        End Sub
#End Region

#Region "GetModule Url "

        Private Function GetModuleUrl() As String

            Return "pg_InfenionBNMCodes.aspx?"

        End Function

#End Region

#Region " Submit Page "

        Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click

            'Variable declarations-start
            Dim RecId As Short = 0, BankName As String = Nothing
            Dim RentasBankCode As String = Nothing, RentasBIC As String = Nothing
            Dim GIRORoutingCode As String = Nothing, BNMCode As String = Nothing
            Dim CatchMessage As String = Nothing, Count As Short = 0
            'Variable declarations-start

            Try

           
                'Read UI Values -start
                RecId = clsGeneric.NullToShort(hfRecId.Value)
                BankName = clsGeneric.NullToString(txtBankName.Text)
                RentasBankCode = clsGeneric.NullToString(txtRentasBankCode.Text)
                RentasBIC = clsGeneric.NullToString(txtRentasBIC.Text)
                GIRORoutingCode = clsGeneric.NullToString(txtGIRORoutingCode.Text)
                BNMCode = clsGeneric.NullToString(txtBNMCode.Text) 'Read UI Values -start

                'If UI Values are Empty-start

                If BankName = "" Or RentasBankCode = "" Or RentasBIC = "" Or GIRORoutingCode = "" Or BNMCode = "" Then

                    lblErrorMessage.Text = " Please Provide all Values and submit "
                    Exit Try

                Else

                    Count = clsGeneric.NullToShort(PPS.SQLScalarValue(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMessage, _Helper.GetCommonSQL & _
                    "  Is_InfenionExist" & ",'" & BankName & "','" & RentasBankCode & "','" & RentasBIC & "','" & GIRORoutingCode & "','" & BNMCode & "'"))

                    If Count > 0 And RecId = 0 Then

                        lblErrorMessage.Text = " BNM Code Already Exist with the provided details "
                       


                        Exit Try

                    End If

                    If PPS.ExecuteSQL(_Helper.GetSQLConnection, _Helper.GetSQLTransaction, CatchMessage, _Helper.GetSQLInfenionInsertUPdate & RecId & ",'" & _
                    BankName & "','" & RentasBankCode & "','" & RentasBIC & "','" & GIRORoutingCode & "','" & BNMCode & "'") Then

                        lblErrorMessage.Text = " Records Inserted or Modified succesfully"

                    End If

                End If
                'If UI Values are Empty-stop

                Dim WebFrom As New WebForms_pg_InfenionBNMCodes

                FormHelp.ResetFormControls(WebFrom, txtBankName, txtRentasBankCode, txtRentasBIC, txtGIRORoutingCode, txtBNMCode)

                hfRecId.Value = ""

                'BindGrid
                BindBNMCodes()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region " Page Index "

        Protected Sub dgBNMCodes_PageIndexChanged(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridPageChangedEventArgs) Handles dgBNMCodes.PageIndexChanged

            dgBNMCodes.CurrentPageIndex = e.NewPageIndex
            BindBNMCodes()
        End Sub

#End Region

    End Class

End Namespace
