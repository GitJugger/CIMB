Imports MaxPayroll.Generic
Imports Microsoft.VisualBasic
Imports System.Data.SqlClient


Namespace MaxPayroll

   Partial Class pg_PinMailerSerial
      Inherits clsBasePage


#Region "Declaration"

      Dim strDbAuthCode As String
      Dim IsAuthCode As Boolean
      Dim strProcess As String
      Dim strMsgError As String



#End Region

#Region "PageLoad"


      Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
         Dim ClsGeneric As New MaxPayroll.Generic

         Try

            'Check if The user is a bank User
            If Not ss_strUserType = gc_UT_BankUser Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If


            If Not Page.IsPostBack Then
               'if page is not being posted back. then 
               'initial load
                    'BindBody(body)
               prBindGrid()
            End If

         Catch ex As Exception

         Finally

            'Destroy Generic
            ClsGeneric = Nothing

         End Try


      End Sub

#End Region

#Region "DataBound"

      Sub dgSerial_ItemDataBound(ByVal sender As Object, ByVal e As DataGridItemEventArgs)
         Dim ClsGeneric As New MaxPayroll.Generic
         Try


            If e.Item.ItemType <> ListItemType.Header And e.Item.ItemType <> ListItemType.Footer Then
               'Now, reference the LinkButton control that the Delete ButtonColumn has been rendered to
               'Dim deleteButton As String = e.Item.Cells(2).Controls(0).ID
               Dim SkipButton As Object = e.Item.Cells(5).Controls(0)
               Dim DeleteButton As Object = e.Item.Cells(6).Controls(0)
               'can now add the onclick event handler
               SkipButton.Attributes("onclick") = "javascript:return confirm('Are you sure you want to skip one serial no (" & _
                                DataBinder.Eval(e.Item.DataItem, "UseNext") & ") ?')"
               'can now add the onclick event handler
               DeleteButton.Attributes("onclick") = "javascript:return confirm('Are you sure you want to delete ?')"
            End If
         Catch ex As Exception
            LogError("pg_PinMailSerial - dgSerial_ItemDataBound")
         End Try


      End Sub

#End Region

#Region "Skip/Delete"

      Sub dgSerial_Skip(ByVal sender As System.Object, ByVal e As DataGridCommandEventArgs)




         Dim sdaOrg As New SqlDataAdapter                                       'Create SQL Data Adaptor
         Dim dsOrg As New System.Data.DataSet                                   'Create Data Set
         Dim clsGeneric As New MaxPayroll.Generic                                   'Create Generic Class Object

         If e.CommandName = "SKIP" Then
            Try
               If tblAuthoCode.Visible = False Then
                  lblMessage.Text = " Please enter your Validation Code."
                  strProcess = "S"
                  Session("StrProc") = strProcess
                  tblAuthoCode.Visible = True
                  btnInsert.Enabled = False
                  dgSerial.Enabled = False
                  txtInsrtEndNo.Visible = False
                  txtInsrtStart.Visible = False
               End If
            Catch
               ''Report to user
               lblMessage.Text = "Skip Unsuccessful"
               'Terminate SQL Connection
               Call clsGeneric.SQLConnection_Terminate()
               LogError("pg_PinMailerSerial - dgSerial_Skip")
            Finally
               'Destroy Generic Class Object
               clsGeneric = Nothing

               'Destroy SQL Data Adaptor
               sdaOrg = Nothing
            End Try
         End If
      End Sub
      Sub dgSerial_delete(ByVal sender As System.Object, ByVal e As DataGridCommandEventArgs)
         Dim clsGeneric As New MaxPayroll.Generic                                   'Create Generic Class Object
         Dim iDelAcctid As Object
         'Delete the specified range
         If e.CommandName = "DELETE" Then
            Try
               iDelAcctid = dgSerial.DataKeys(e.Item.ItemIndex)
               If tblAuthoCode.Visible = False Then
                  lblMessage.Text = " Please enter your Validation Code."
                  strProcess = "D"
                  Session("iDelAcctid") = iDelAcctid
                  Session("StrProc") = strProcess
                  tblAuthoCode.Visible = True
                  btnInsert.Enabled = False
                  dgSerial.Enabled = False
                  txtInsrtEndNo.Visible = False
                  txtInsrtStart.Visible = False
               End If
            Catch
               LogError("pg_PinMailerSerial - dgSerial_Delete")
            Finally
               clsGeneric = Nothing
            End Try
         End If
      End Sub

      Public Function fnAccDelete(ByVal iTheKey As Integer) As Boolean
         '/*=============================================
         'Function (fnAccDelete)
         'Written(by) : U.Umamahesh()
         'For        : EHR phase II
         'Date       : 15/02/2005 
         'Purpose    : For deleting choosed bank account.
         'Usage:
         'Modified   : 15/02/2005
         '=============================================== */
         Dim strSQLStatment As String 'holds Query string
         Dim clsGeneric As New Generic
         Dim sdaOrg As New SqlDataAdapter
         Dim dsOrg As New DataSet

         Try


            'Initialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()


            'Assign Search Parameters to Procedure
            strSQLStatment = "pg_DeleteSerial  " & iTheKey & " , " & ss_lngUserID

            'Execute SQL Command
            Dim cmd As New SqlCommand
            cmd.Connection = clsGeneric.SQLConnection
            cmd.CommandText = strSQLStatment
            cmd.ExecuteNonQuery()


            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Return true
            Return True

         Catch

            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Log Error
            LogError("pg_PinMailerSerial - fnAccDelete")


         Finally
            'Destroy Generic Class Object
            clsGeneric = Nothing
            ' destroy sql dataadapter
            sdaOrg = Nothing
            tblAuthoCode.Visible = False
            btnInsert.Enabled = True
            dgSerial.Enabled = True
         End Try

      End Function

#End Region

#Region "Bind Grid"

      Sub prBindGrid()

         Dim clsGeneric As New MaxPayroll.Generic    'Create Generic Class Object
         'Dim clsAdmin As New Gateway.Admin

         Dim dsOrg As New System.Data.DataSet

         'Variable Declarations

         Dim intRecordCount As Int32

         Try


            'Specific serials for particular user ???
            'dsOrg = fnSerialNumber(lngUserId)

            'For all users list
            dsOrg = fnSerialNumber(0)
            intRecordCount = dsOrg.Tables(0).Rows.Count
            If intRecordCount > 0 Then
               Me.pnlGrid.Visible = True
               fncGeneralGridTheme(dgSerial)
               lblMessage.Visible = False
               dgSerial.Visible = True
               dgSerial.DataSource = dsOrg
               dgSerial.DataBind()
            Else
               Me.pnlGrid.Visible = False
               lblMessage.Visible = True
               dgSerial.Visible = False
               lblMessage.Text = "No Data Found ."
            End If

         Catch
            LogError("pg_PinMailerSerial - prBindGrid")

         Finally

            'clsAdmin = Nothing
            clsGeneric = Nothing

         End Try

      End Sub

      Public Function fnSerialNumber(Optional ByVal User_Id As Long = 0) As DataSet
         '/*=============================================
         'Function (fnBankAccount)
         'Written(by) : U.Umamahesh()
         'For        : EHR phase II
         'Date       : 18/04/2005 
         'Purpose    : For retrieving and displaying existing Serial numbers
         ' details.
         'Usage:
         'Modified   : 18/04/2005
         '=============================================== */
         Dim strSQLStatment As String 'holds Query string
         Dim clsGeneric As New Generic
         Dim sdaOrg As New SqlDataAdapter
         Dim dsOrg As New DataSet

         Try

            'Initialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()

            'Assign Search Parameters to Procedure
            strSQLStatment = "Exec pg_PinSerialAvaliable  " & User_Id

            'Execute SQL Data Adaptor
            sdaOrg = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

            'Fill Data Set
            sdaOrg.Fill(dsOrg, "SerialNumberInfo")

            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Destroy SQL Data Adaptor
            sdaOrg = Nothing

            'Return Data Set
            Return dsOrg

         Catch

            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Log Error
            Call clsGeneric.ErrorLog(User_Id, 0, "fn", Err.Number, Err.Description)




         Finally
            'Destroy Generic Class Object
            clsGeneric = Nothing
            dsOrg.Dispose()
            'Destroy SQL Data Adaptor
            sdaOrg = Nothing



         End Try
         Return Nothing
      End Function

#End Region

#Region "Insert"


      Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsert.Click

         If fnInsrtValidate() Then 'if the entries are validated
            If tblAuthoCode.Visible = False Then
               lblMessage.Text = " Please enter your Validation Code."
               strProcess = "I"
               Session("StrProc") = strProcess
               tblAuthoCode.Visible = True
               btnInsert.Enabled = False
               dgSerial.Enabled = False
               txtInsrtEndNo.Enabled = False
               txtInsrtStart.Enabled = False
            ElseIf tblAuthoCode.Visible = True Then
               fninsertSerials() 'Perform insert
               prBindGrid() 'Bind the grid again 
               lblMessage.Visible = True 'show message
               lblMessage.Text = "Serial No Creation Successful"
               tblAuthoCode.Visible = False
               btnInsert.Enabled = True
               dgSerial.Enabled = True
            End If
         Else
            lblMessage.Visible = True 'if the entries are not validated show failure message.
            lblMessage.Text = "Insertion Failed " & "<BR>" & strMsgError

         End If

      End Sub

      Private Function fninsertSerials() As Boolean
         Dim strSQLStatment As String 'holds Query string
         Dim clsGeneric As New Generic
         Dim sdaOrg As New SqlDataAdapter
         Dim dsOrg As New DataSet
         Dim lngOrganisationId As Long, lngUserId As Long
         Try

            lngUserId = IIf(IsNumeric(Session("SYS_USERID")), Session("SYS_USERID"), 0)
            lngOrganisationId = IIf(IsNumeric(Session("SYS_ORGID")), Session("SYS_ORGID"), 0)

            'Initialize SQL Connection
            Call clsGeneric.SQLConnection_Initialize()

            'Read in the Primary Key Field

            Dim intStartNo As Integer = txtInsrtStart.Text
            Dim intEndNo As Integer = txtInsrtEndNo.Text

            'Assign Search Parameters to Procedure
            ' second parameter 0 to indicate insert operation
            strSQLStatment = "Exec pg_PinSerialNumIns  " & lngUserId & "," & intStartNo & "," & intEndNo & ""

            'Execute SQL Command
            Dim cmd As New SqlCommand
            cmd.Connection = clsGeneric.SQLConnection
            cmd.CommandText = strSQLStatment
            cmd.ExecuteNonQuery()


            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Return true
            Return True

         Catch

            'Terminate SQL Connection
            Call clsGeneric.SQLConnection_Terminate()

            'Log Error
            Call clsGeneric.ErrorLog(lngOrganisationId, lngUserId, "Pin Mailer Insert", Err.Number, Err.Description)
            lblMessage.Text = "Insertion failure"
            'Destroy Generic Class Object
            clsGeneric = Nothing

            Return False

         Finally

            clsGeneric = Nothing
            txtInsrtStart.Text = ""
            txtInsrtEndNo.Text = ""

         End Try

      End Function


      Private Function fnInsrtValidate() As Boolean
         Dim ErrEnt As Boolean

         ' Initialise Err Msg

         Try
            Dim intStartNo As Integer = CInt(txtInsrtStart.Text)
            Dim intEndNo As Integer = CInt(txtInsrtEndNo.Text)

            'Checking for account name
            If Not IsNumeric(intStartNo) Then
               strMsgError = strMsgError + " Error : Start Serial No" & "<BR>"
               'Set Error flags
               fnInsrtValidate = False
               ErrEnt = True
               Exit Function
            End If
            If Not IsNumeric(intEndNo) Then
               strMsgError = strMsgError + " Error : End Serial No" & "<BR>"
               'Set Error flags
               fnInsrtValidate = False
               ErrEnt = True
               Exit Function
            End If
            If (intStartNo) > (intEndNo) Then
               strMsgError = strMsgError + " Error : End No Less then start Serial No" & "<BR>"
               'Set Error flags
               fnInsrtValidate = False
               ErrEnt = True
            End If
            If ErrEnt = True Then
               Return False
            Else
               Return True
            End If

         Catch

         Finally

         End Try
      End Function


#End Region

#Region "AuthoClicks"

      Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
         Dim clsCommon As New MaxPayroll.clsCommon
         Dim clsGeneric As New MaxPayroll.Generic
         Dim clsUsers As New MaxPayroll.clsUsers
         Dim intAttempts As Long

         Dim sdaOrg As New SqlDataAdapter                                       'Create SQL Data Adaptor
         Dim dsOrg As New System.Data.DataSet                                   'Create Data Set

         Dim iDelAcctid As Object
         Dim send As Object = sender
         'Variable Declarations
         Dim strSQLStatment As String

         'Check If AuthCode is Valid
         strDbAuthCode = clsCommon.fncPassAuth(ss_lngUserID, "A", ss_lngOrgID)
         IsAuthCode = IIf(strDbAuthCode = TxtAuthorization.Text, True, False)
         'Check for invalid Authorization Code Attempts - START
         intAttempts = IIf(IsNumeric(Session("AUTH_LOCK")), Session("AUTH_LOCK"), 0)
         If Not intAttempts = 2 Then
            If Not IsAuthCode Then
               intAttempts = intAttempts + 1
               Session("AUTH_LOCK") = intAttempts
               lblMessage.Visible = True
               lblMessage.Text = "Validation code is invalid. Please enter a valid Validation Code."
               Exit Sub
            End If
         ElseIf intAttempts = 2 Then
            If Not IsAuthCode Then
               Call clsUsers.prcAuthLock(ss_lngOrgID, ss_lngUserID, "A")
               lblMessage.Visible = True
               lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
               Exit Sub
            End If
         End If
         strProcess = Session("StrProc")
         If IsAuthCode Then
            If strProcess = "I" Then
               btnInsert_Click(sender, e)
            ElseIf strProcess = "S" Then
               Try
                  'Initialize SQL Connection
                  Call clsGeneric.SQLConnection_Initialize()

                  strSQLStatment = "Exec pg_SkipSerial " & ss_lngUserID

                  'Execute SQL Data Adaptor
                  sdaOrg = New SqlDataAdapter(strSQLStatment, clsGeneric.SQLConnection)

                  'Fill Data Set
                  sdaOrg.Fill(dsOrg, "SerialNext")
                  'Bind grid again
                  prBindGrid()
                  'Report to user
                  lblMessage.Text = "Skip Successful"
                  'Terminate SQL Connection
                  Call clsGeneric.SQLConnection_Terminate()
                  'Destroy SQL Data Adaptor
                  sdaOrg = Nothing

               Catch
                  ''Report to user
                  lblMessage.Text = "Skip Unsuccessful"
                  'Terminate SQL Connection
                  Call clsGeneric.SQLConnection_Terminate()

                  'Log Error
                  LogError("pg_PinMailerSerial - btnConfirm")
               Finally
                  sdaOrg = Nothing
                  tblAuthoCode.Visible = False
                  dgSerial.Enabled = True
                  btnInsert.Enabled = True
                  txtInsrtStart.Visible = True
                  txtInsrtEndNo.Visible = True
               End Try
            ElseIf strProcess = "D" Then
               iDelAcctid = Session("iDelAcctid")
               If fnAccDelete(iDelAcctid) Then
                  lblMessage.Visible = True
                  lblMessage.Text = "Serial Range deleted "
                  prBindGrid()
               End If
               tblAuthoCode.Visible = False
               dgSerial.Enabled = True
               btnInsert.Enabled = True
               txtInsrtStart.Visible = True
               txtInsrtEndNo.Visible = True
            End If
         End If
         clsUsers = Nothing
         clsCommon = Nothing
      End Sub

      Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
         tblAuthoCode.Visible = False
         txtInsrtEndNo.Visible = True
         txtInsrtEndNo.Enabled = True
         txtInsrtStart.Visible = True
         txtInsrtStart.Visible = True
         btnInsert.Enabled = True
         dgSerial.Enabled = True
      End Sub

#End Region

   End Class

End Namespace
