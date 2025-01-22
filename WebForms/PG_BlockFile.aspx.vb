Option Strict Off
Option Explicit On 

Imports MaxFTP.clsFTP
Imports MaxPayroll.clsBank
Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers


Namespace MaxPayroll



Partial Class PG_BlockFile
      Inherits clsBasePage

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'Create Instance of Bank Class Object
        Dim clsGeneric As New MaxPayroll.Generic

        'Create Instance of User Class Object
        Dim clsUsers As New MaxPayroll.clsUsers

       
        Try
            If Not Page.IsPostBack Then
               If Not ss_strUserType = gc_UT_SysAdmin Then
                  Server.Transfer(gc_LogoutPath, False)
                  Exit Try
               End If
               'Bind Data grid
               Call prBindGrid()
            End If

         Catch ex As Exception

         Finally

            'Destroy Instance of Bank Class Object
            clsGeneric = Nothing

            'Destroy Instance of User Class Object
            clsUsers = Nothing

         End Try

    End Sub

#End Region

#Region "Bind Data Grid"

    '****************************************************************************************************
    'Function Name  : prBindGrid()
    'Purpose        : Bind The User Data Grid
    'Arguments      : N/A
    'Return Value   : Data Grid
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub prBindGrid()

            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of System Data Set
            Dim dsBlock As New System.Data.DataSet

            'Variable Declarations
            Dim lngOrgId As Long, lngUserId As Long, intCount As Int16

            Try

                lngOrgId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)        'Get Organisation Id
                lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)     'Get User Id

                'Populate Data Set
                dsBlock = clsBank.fncStopPayment("Block File", "", lngOrgId, lngOrgId, lngUserId)
                intCount = dsBlock.Tables(0).Rows.Count

                'Bind Data Grid
                If intCount > 0 Then
                    dgBlock.Visible = True
                    dgBlock.DataSource = dsBlock
                    dgBlock.DataBind()
                    lblMessage.Text = ""
                Else
                    dgBlock.Visible = False
                    lblMessage.Text = "No Records Found."
                End If


            Catch ex As Exception

            Finally

                'Destroy Instance of Bank Class Object
                clsBank = Nothing

                'Destroy Instance of System Data Set
                dsBlock = Nothing

            End Try

        End Sub

#End Region

#Region "Data Grid Navigation"

        '****************************************************************************************************
        'Function Name  : prPageChange()
        'Purpose        : Navigation for Data Grid
        'Arguments      : System Objects,System EventArgs
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 25/10/2003
        '*****************************************************************************************************
        Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

            Dim intStart As Int16

            Try
                intStart = dgBlock.CurrentPageIndex * dgBlock.PageSize
                dgBlock.CurrentPageIndex = E.NewPageIndex
                Call prBindGrid()
            Catch ex As Exception

            End Try

        End Sub

#End Region

End Class

End Namespace
