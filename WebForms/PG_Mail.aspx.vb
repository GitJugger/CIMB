Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient


Namespace MaxPayroll


Partial Class PG_Mail
      Inherits clsBasePage
#Region "Declaration"
        Dim clsEncryption As New MaxPayroll.Encryption
        Private ReadOnly Property rq_lngMail() As String
            Get
                If Request.QueryString("ID") IsNot Nothing Then
                    Return clsEncryption.fnSQLCrypt(Request.QueryString("ID"))
                Else
                    Return -1
                End If
            End Get
        End Property
        Private ReadOnly Property rq_dtDate() As DateTime
         Get
            If IsDate(Request.QueryString("Date")) Then
               Return CDate(Request.QueryString("Date"))
            Else
               Return Nothing
            End If
         End Get
      End Property
      Private ReadOnly Property rq_strSubject() As String
         Get
            Return Request.QueryString("Subject") & ""
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
      '*****************************************************************************************************
      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

         'Create Instance of System Data Row
         Dim drMail As System.Data.DataRow

         'Create Instance of System Data Set
         Dim dsMail As New System.Data.DataSet

         'Create Instance of Generic Class Object
         Dim clsGeneric As New MaxPayroll.Generic

         'Variable Declarations
         Dim strMailTo As String

            Try

                If Not Page.IsPostBack Then
                    Dim stat = 0
                    'Populate Data Set
                    dsMail = clsGeneric.fnLoadMail(rq_lngMail, ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                    Dim vOut As Long = (rq_lngMail)
                    Dim IDSentOld As Double = Convert.ToDouble(rq_lngMail)
                    Dim IDSent As Long = CLng(IDSentOld)
                    If dsMail.Tables("MAIL").Rows.Count > 0 Then
                        For Each drMail In dsMail.Tables("MAIL").Rows
                            'If ss_lngOrgID <> drMail("UOrgId") Then
                            '    stat = 1
                            '    Exit For
                            'End If
                            'If ss_lngGroupID <> drMail("UGroupId") Then
                            '    stat = 1
                            '    Exit For
                            'End If
                            If ss_lngUserID <> drMail("MAILTOID") Then
                                stat = 1
                                Exit For
                            End If
                            'lblDtValue.Text = Format(drMail("MDATE"), "dd/MM/yyyy")
                            'lblTmValue.Text = Format(drMail("MDATE"), "hh:mm tt")
                            'lblSbValue.Text = rq_strSubject
                            lblDtValue.Text = drMail("MDATE")
                            lblTmValue.Text = drMail("MDATE")
                            lblSbValue.Text = drMail("SUBJECT")
                            lblBody.Text = drMail("BODY")
                            strMailTo = drMail("MailTo")
                            lblTo.Text = "Dear " & strMailTo & ","
                            'lblFrom.Text = "From " & gc_BR & Request.QueryString("From")
                            lblFrom.Text = "From " & gc_BR & drMail("MailFrom")
                        Next
                    End If
                    If stat = 1 Then
                        Call clsGeneric.Logoff(ss_lngOrgID, ss_lngUserID)
                        Throw New System.Exception("InvalidId")
                        Exit Try
                    End If
                    btnDelete.Attributes.Add("onClick", "return confirm('Are you sure you want to delete the message?');")
                End If

            Catch ex As Exception
                If ex.Message = "InvalidId" Then
                    HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                Else
                    LogError("PG_Mail - Page Load")
                End If
            Finally

            'Destroy Data Set
            dsMail = Nothing

            'Destroy Data Row
            drMail = Nothing

            'Destroy Generic Class Object
            clsGeneric = Nothing

            'Destroy MailBox Class Object
            'clsMailbox = Nothing

         End Try

      End Sub

#End Region

#Region "Delete Mail"

    Private Sub prDelete(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnDelete.Click

        'Create Instance of SQL Command Object
        Dim cmdDelMail As New SqlCommand

        'Create Instance of Generic Class Object
        Dim clsGeneric As New MaxPayroll.Generic
            Dim drMail As System.Data.DataRow

            'Create Instance of System Data Set
            Dim dsMail As New System.Data.DataSet
            Dim stat = 0
            Try
                dsMail = clsGeneric.fnLoadMail(rq_lngMail, ss_lngOrgID, ss_lngUserID, ss_lngGroupID)
                If dsMail.Tables("MAIL").Rows.Count > 0 Then
                    For Each drMail In dsMail.Tables("MAIL").Rows
                        'If ss_lngOrgID <> drMail("UOrgId") Then
                        '    stat = 1
                        '    Exit For
                        'End If
                        'If ss_lngGroupID <> drMail("UGroupId") Then
                        '    stat = 1
                        '    Exit For
                        'End If
                        'If ss_lngUserID <> drMail("UMaiLTo") Then
                        '    stat = 1
                        '    Exit For
                        'End If
                        If ss_lngUserID <> drMail("MAILTOID") Then
                            stat = 1
                            Exit For
                        End If
                    Next
                End If
                If stat = 1 Then
                    Call clsGeneric.Logoff(ss_lngOrgID, ss_lngUserID)
                    Throw New System.Exception("InvalidId")
                    Exit Try
                End If

                clsGeneric.DeleteMail("MAIL", rq_lngMail, ss_lngOrgID)


                Server.Transfer("PG_Inbox.aspx", False)

            Catch ex As Exception
                If ex.Message = "InvalidId" Then
                    HttpContext.Current.Response.Redirect("~/WebForms/PG_Index.aspx")
                Else
                    LogError("PG_Mail - btnDelete")
                End If
            Finally

                'Destroy Generic Class Object
                clsGeneric = Nothing

            'Destroy Instance of SQL Command Object
            cmdDelMail = Nothing

         End Try

    End Sub

#End Region




    End Class

End Namespace
