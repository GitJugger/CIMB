Imports maxpayroll.clsCommon
Namespace MaxPayroll

    Partial Class PG_ListFile
        Inherits clsBasePage

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 03/04/2005
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            'Create Instance of System Data Set
            Dim dsFileList As New System.Data.DataSet

            'Variable Declarations
            Dim lngOrgId As Long, lngUserId As Long
            Try

                If Not Page.IsPostBack Then
                    'BindBody(body)
                End If

                If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_InquiryUser) Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                If Left(Trim(Request.QueryString("Status")), 1) = "C" Then
                    Response.Write("<script language='JavaScript'>")
                    Response.Write("alert('Organization has been cancelled');")
                    Response.Write("window.location.href = 'PG_ViewOrganisation.aspx?Req=File'")
                    Response.Write("</script>")
                    Exit Try
                End If

                trOrgId.Visible = True
                trOrgName.Visible = True
                hOrgId.Value = Request.QueryString("Id")                                            'Organisation Id
                lblOrgName.Text = Request.QueryString("Name")                                       'Organisation Name
                lblOrgId.Text = Request.QueryString("Id")                                     'Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)         'Get User Id
                lngOrgId = IIf(IsNumeric(Request.QueryString("Id")), Request.QueryString("Id"), 0)  'Get Organisation Id

                'Populate Data Set
                dsFileList = clsCommon.fncGetRequested("File Master", lngOrgId, lngUserId, 0, "")

                'Populate Data Grid - START
                If dsFileList.Tables("UPLOAD").Rows.Count > 0 Then
                    fncGeneralGridTheme(dgFile)
                    dgFile.DataSource = dsFileList
                    dgFile.DataBind()
                    If Session("SYS_TYPE") = "BU" Then
                        dgFile.Columns(5).Visible = True
                        dgFile.Columns(6).Visible = False
                    Else
                        dgFile.Columns(5).Visible = False
                        dgFile.Columns(6).Visible = True
                    End If

                    '071022 3 lines added by Marcus
                    'Purpose: To hide bank info in datagrid if there is a default bank set up in web.config.
                    If fncDefaultBankChecking() Then
                        dgFile.Columns(1).Visible = False
                    End If


                Else
                    Me.pnlGrid.Visible = False
                    lblMessage.Text = "No Records Available"
                End If
                'Populate Data Grid - STOP

                If Session("SYS_TYPE") = "IU" Then
                    btnNew.Visible = False
                End If

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "Page_Load - PG_ListFile", Err.Number, Err.Description)

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub

#End Region

    End Class

End Namespace
