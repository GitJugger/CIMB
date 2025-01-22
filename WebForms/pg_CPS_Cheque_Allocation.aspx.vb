Option Strict Off
Option Explicit On 

Imports MaxPayroll.Generic
Imports MaxPayroll.clsApprMatrix
Imports MaxMiddleware


Namespace MaxPayroll


    Partial Class pg_CPS_Cheque_Allocation
        Inherits clsBasePage
        'Private _clsCPSPhase3 As New clsCPSPhase3
        Private _Helper As New Helper
        Dim URL As String = Nothing

#Region "Declaration"
        Enum enmDGViewPending
            RequestDateTime
            LinkAction
            MDSC
        End Enum
        Enum enmDGView
            RequestDate
            LinkAction
            MDSC
            ActionDate
            Remark
            MDID
            APPR_SUB
            APPR_ID
        End Enum
        Enum enmDGApprove
            CheckBox
            APRID
            FRID
            MDID
            FName
            ReceiveDate
            Subject
            LinkAction
            MDSC
            StatusRadioBox
            Remark
        End Enum
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
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Variable Declarations
            Dim strRequest As String, strAuthLock As String

            Try
                'BindBody(body)

                'Get Authorization Lock Status - Start
                ''If UCase(Request.QueryString("Mode")) = "EDIT" Then

                ''    strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngOrgID, ss_lngUserID)
                ''    If strAuthLock = "Y" Then
                ''        btnSubmit.Enabled = False
                ''        btnConfirm.Enabled = False
                ''        lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                ''    End If
                ''End If
                'Get Authorization Lock Status - Stop

                If Not Page.IsPostBack Then

                    'trConfirm.Visible = False
                    'trAuthCode.Visible = False
                    strRequest = Request.QueryString("Mode")
                    hidMode.Value = strRequest

                    Call prcBindGrid(strRequest)

                    'Display Messages - START
                    Select Case LCase(strRequest)
                        Case "reject"
                            lblHeading.Text = "Rejected Requests"
                        Case "done"
                            lblHeading.Text = "Accepted Requests"
                        Case "view"
                            lblHeading.Text = "Pending Approval"
                        Case "edit"
                            lblHeading.Text = "Pending Requests"
                    End Select
                    'If UCase(strRequest) = "REJECT" Then
                    '   lblHeading.Text = "Rejected Requests"
                    'ElseIf UCase(strRequest) = "DONE" Then
                    '   lblHeading.Text = "Accepted Requests"
                    'ElseIf UCase(strRequest) = "DONE" Then
                    '   lblHeading.Text = "Pending Requests"
                    'ElseIf UCase(strRequest) = "VIEW" Then
                    '   lblHeading.Text = "Pending Authorization"
                    'End If
                    'Display Messages - STOP

                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "Page Load - PG_ApprMatrix", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of Common Class Object
                clsCommon = Nothing

            End Try

        End Sub

#End Region








#Region "Page Navigation"

        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try

                intStart = dgCPSMatrix.CurrentPageIndex * dgCPSMatrix.PageSize
                dgCPSMatrix.CurrentPageIndex = E.NewPageIndex
                'Call prcBindGrid()

            Catch ex As Exception

            End Try

        End Sub

        'Sub prVPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

        '    Dim intStart As Int16

        '    Try

        '        intStart = dgViewMatrix.CurrentPageIndex * dgViewMatrix.PageSize
        '        dgViewMatrix.CurrentPageIndex = E.NewPageIndex
        '        'Call prcBindGrid()

        '    Catch ex As Exception

        '    End Try

        'End Sub

        'Sub prPPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

        '    Dim intStart As Int16

        '    Try

        '        intStart = dgViewPend.CurrentPageIndex * dgViewPend.PageSize
        '        dgViewPend.CurrentPageIndex = E.NewPageIndex
        '        'Call prcBindGrid()

        '    Catch ex As Exception

        '    End Try

        'End Sub

#End Region

#Region "Bind Datagrid"
        '****************************************************************************************************
        'Procedure Name : Bind_Grid()
        'Purpose        : DataGrid Bind
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 11/03/2005
        '*****************************************************************************************************
        Private Sub prcBindGrid(ByVal strRequest As String)

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of System Data Set
            Dim dtCPSDividend As New System.Data.DataTable
            Dim dtCPSDelimDividend As New System.Data.DataTable
            Dim dtCPSSFF As New System.Data.DataTable

            Dim dtCPSMergeAll As New System.Data.DataTable

            'Create Instance of Approval Matrix Class Object
            Dim clsApprMatrix As New MaxPayroll.clsApprMatrix



            Try

                'Populate Datagrid - START

                'tblView.Visible = False
                'tblViewPend.Visible = False
                tblMainForm.Visible = True
                'Populate Data Set

                dtCPSDividend = PPS.GetData(_Helper.GetApproveFile & " '" & _Helper.CPSDividen_Name & "','" & strRequest & "'", _Helper.GetSQLConnection, _
                                                _Helper.GetSQLTransaction)

                dtCPSDelimDividend = PPS.GetData(_Helper.GetApproveFile & " '" & _Helper.CPSDelimited_Dividen_Name & "','" & strRequest & "'", _Helper.GetSQLConnection, _
                                _Helper.GetSQLTransaction)

                dtCPSSFF = PPS.GetData(_Helper.GetApproveFile & " '" & _Helper.CPSSingleFileFormat_Name & "','" & strRequest & "'", _Helper.GetSQLConnection, _
                                _Helper.GetSQLTransaction)

                'Merge all dataset to dtcpsmergeall'
                dtCPSMergeAll.Merge(dtCPSDividend)
                dtCPSMergeAll.Merge(dtCPSDelimDividend)
                dtCPSMergeAll.Merge(dtCPSSFF)




                If dtCPSMergeAll.Rows.Count > 0 Then
                    dgCPSMatrix.DataSource = dtCPSMergeAll
                    dgCPSMatrix.DataBind()
                    fncGeneralGridTheme(dgCPSMatrix)
                    pnlGrid.Visible = True
                Else
                    'btnCheckAll.Visible = False
                    'btnUnCheck.Visible = False
                    'btnAccept.Visible = False
                    'trSubmit.Visible = False
                    dgCPSMatrix.Visible = False
                    pnlGrid.Visible = False
                    lblMessage.Text = "No Records "


                End If
                'Populate Datagrid - STOP

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "prcBindGrid - pg_CPS_Cheque_Allocation", Err.Number, Err.Description)

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

                'Destroy Instance of System Data Set
                dtCPSMergeAll = Nothing
                dtCPSDividend = Nothing
                dtCPSDelimDividend = Nothing
                dtCPSSFF = Nothing

                'Destroy Instance of Approval Matrix
                clsApprMatrix = Nothing

            End Try

        End Sub

#End Region

        'Protected Sub dgViewMatrix_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgViewMatrix.ItemDataBound
        '    'href="PG_ViewApprMatrix.aspx?Id=<%#DataBinder.Eval(Container.DataItem,"MDID")%>&Module=<%#DataBinder.Eval(Container.DataItem,"APPR_SUB")%>&Mode=<%=Request.QueryString("Mode")%>&Appr=<%#DataBinder.Eval(Container.DataItem,"APPR_ID")%>"
        '    Select Case e.Item.ItemType
        '        Case ListItemType.AlternatingItem, ListItemType.Item
        '            Dim lnkAction As HtmlAnchor
        '            lnkAction = CType(e.Item.FindControl("lnkAction"), HtmlAnchor)
        '            lnkAction.HRef = "pg_CPS_Cheque_Allocation?PageNo=" & dgViewMatrix.CurrentPageIndex.ToString & "&Id=" & e.Item.Cells(enmDGView.MDID).Text & "&Module=" & e.Item.Cells(enmDGView.APPR_SUB).Text & "&Mode=" & Request.QueryString("Mode") & "&Appr=" & e.Item.Cells(enmDGView.APPR_ID).Text
        '    End Select
        'End Sub


        Public Function GetLink(ByVal Action As String) As String ''Action value return from SP [CIMBGW_CPSGETAPPROVEFILE]
            Dim URL As String = Nothing
            If Action = "CPS Cheque Allocation" Then
                URL = "PG_CPSAssignChequeNo.aspx"
            ElseIf Action = "CPS Cheque Confirmation" Then
                URL = "PG_CPSFileSubmission.aspx"
            End If
            Return URL
        End Function




    End Class

    
End Namespace
