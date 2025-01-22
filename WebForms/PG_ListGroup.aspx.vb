Namespace MaxPayroll

Partial Class PG_ListGroup
      Inherits clsBasePage

#Region "Declaration"
      Private ReadOnly Property rq_strErrorMsg() As String
         Get
            Return Request.QueryString("Err") & ""
         End Get
      End Property
#End Region

#Region "Page Load"

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            If Not ss_strUserType = gc_UT_SysAdmin Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If
            If Page.IsPostBack = False Then
                    'BindBody(body)
            End If
            'Bind Data Grid
            Call prcBindGrid(ss_lngOrgID, ss_lngUserID)
            prcBindTitle()
        Catch

            LogError("Page Load - PG_ListMonth")

        Finally


        End Try

      End Sub

      Private Sub prcBindTitle()
         Dim sTitle As String = " Please contact " & gc_Const_CompanyName & " registration center."

         Select Case rq_strErrorMsg.ToLower
            Case "limit"
               lblMessage.Text = "You have reached the group limit.  No more group can be created." & gc_BR & sTitle
            Case "auth"
               lblMessage.Text = "Validation code locked due to invalid attempts." & gc_BR & sTitle
            Case "bank"
               lblMessage.Text = "No bank accounts created." & gc_BR & sTitle
                    'Case "epfacc"
                    '   lblMessage.Text = "No EPF accounts created." & gc_BR & sTitle
                    'Case "socacc"
                    '   lblMessage.Text = "No SOCSO accounts created." & gc_BR & sTitle
                    'Case "lhdnacc"
                    '   lblMessage.Text = "No LHDN accounts created." & gc_BR & sTitle
                    'Case "pay"
                    '   lblMessage.Text = "No payroll file format." & gc_BR & sTitle
                    'Case "epf"
                    '   lblMessage.Text = "No EPF file format." & gc_BR & sTitle
                    'Case "soc"
                    '   lblMessage.Text = "No SOCSO file format." & gc_BR & sTitle
                    'Case "lhdn"
                    '   lblMessage.Text = "No LHDN file format." & gc_BR & sTitle

            End Select
      End Sub

#End Region

#Region "Page Change"

    Sub prPageChange(ByVal O As System.Object, ByVal E As DataGridPageChangedEventArgs)

         Dim intStart As Int16

        Try

            intStart = dgGroup.CurrentPageIndex * dgGroup.PageSize
            dgGroup.CurrentPageIndex = E.NewPageIndex

            'Bind Data Grid
            Call prcBindGrid(ss_lngOrgID, ss_lngUserID)

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Load Datagrid"

    Private Sub prcBindGrid(ByVal lngOrgId As Long, ByVal lngUserId As Long)

         'Create Instance of Customer Class Object
         Dim clsCustomer As New MaxPayroll.clsCustomer

         'Create Instance of DataSet
         Dim dsListGroup As New DataSet

         Try

            dsListGroup = clsCustomer.fncListGroup("LIST ALL", lngOrgId, lngUserId, 0)
            If dsListGroup.Tables(0).Rows.Count > 0 Then
               dgGroup.Visible = True
               dgGroup.DataSource = dsListGroup
               dgGroup.DataBind()
            Else
               If lblMessage.Text = "" Then
                  dgGroup.Visible = False
                  lblMessage.Text = "No Groups Available."
               End If
            End If
            fncGeneralGridTheme(dgGroup)
         Catch
            LogError("prcBindGrind - PG_CreateGroup")
         End Try

      End Sub

#End Region
        Public Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function
    End Class

End Namespace
