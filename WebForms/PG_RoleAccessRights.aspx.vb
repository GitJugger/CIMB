Imports MaxPayroll.Generic
Imports MaxPayroll.clsUsers
Imports System.Data.SqlClient
Imports MaxGeneric
Imports System.IdentityModel.Claims
Imports System.Web.Services
Imports System.Web.Script.Serialization
Imports MaxPayroll.Encryption



Namespace MaxPayroll


    Partial Class PG_RoleAccessRights

        Inherits clsBasePage
        Dim clsEncryption As New MaxPayroll.Encryption
#Region "Request.QueryString"
        Private ReadOnly Property rq_strID() As String
            Get
                Return clsEncryption.Cryptography(Request.QueryString("ID")) & ""
            End Get
        End Property
        Private ReadOnly Property rq_LoginId() As String
            Get
                If (Request.QueryString("LoginId")) IsNot Nothing Then
                    Return clsEncryption.Cryptography(Request.QueryString("LoginId"))
                Else
                    Return ""
                End If

            End Get
        End Property
        Private ReadOnly Property rq_iFieldLock() As Integer
            Get
                If IsNumeric(Request.QueryString("FieldLock")) Then
                    Return Request.QueryString("FieldLock")
                Else
                    Return -1
                End If

            End Get
        End Property
        Private ReadOnly Property rq_OrgId() As Long
            Get
                If IsNumeric(Request.QueryString("OrgID")) Then
                    Return CLng(Request.QueryString("OrgID"))
                Else
                    Return -1
                End If
            End Get
        End Property

        Shared strServer, strDatabase, strUserName, strPassword, strConnection As String

        Public Sub GetCon()
            strServer = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("SERVER"))
            strDatabase = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("DATABASE"))
            strUserName = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("USERNAME"))
            strPassword = clsEncryption.fnSQLCrypt(System.Configuration.ConfigurationManager.AppSettings("PASSWORD"))
            strConnection = "SERVER=" & strServer & ";DATABASE=" & strDatabase & ";UID=" & strUserName & ";PWD=" & strPassword
        End Sub

#End Region

#Region "Page Load"

        '****************************************************************************************************
        'Procedure Name : Page_Load()
        'Purpose        : Page Load 
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 16/02/2005
        'Modified By    : Victor Wong 
        'Modified Date  : 2007-03-08
        '*****************************************************************************************************

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            GetCon()
            ' Create Instance of Data Row
            Dim drGroups As DataRow

            ' Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            ' Create Instance of System Data Set
            Dim dsGroups As New System.Data.DataSet

            ' Create Instance of Common Class Object
            Dim clsCommon As New MaxPayroll.clsCommon

            ' Create Instance of Customer Class Object
            Dim clsCustomer As New MaxPayroll.clsCustomer

            ' Variable Declarations
            Dim strMinValue As String, strVerify As String, strMod As String
            Dim strAuthLock As String

            Try
                strMod = Request.QueryString("Mod")

                ' Handle Authentication
                If Not Len(ss_strUserType) = 2 Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                '' Get Authorization Lock Status
                'strAuthLock = clsCommon.fncBuildContent("Auth Status", "", ss_lngUserID, ss_lngUserID)
                'If strAuthLock = "Y" Then
                '    btnSubmit.Enabled = False
                '    lblMessage.Text = "Your Validation Code has been locked due to invalid attempts."
                'End If

                ' If Page is not a Postback
                If Not Page.IsPostBack Then
                    'ViewState("PageIndex") = 0
                    BindDataToGrid()
                    BindRole()
                    ' Hide BackToView if ID is empty
                    If Me.rq_strID = "" Then
                        'Me.inptBackToView.Visible = False
                    End If

                    ' Display the main form
                    tblMainForm.Visible = True


                    strMinValue = Format(Now(), "dd/MM/yyyy")

                    ' Get Verification Type
                    strVerify = Session(gc_Ses_VerificationType)

                    ' Populate Organisation Groups
                    'If ss_strUserType = gc_UT_SysAdmin OrElse ss_strUserType = gc_UT_SysAuth Then
                    '    dsGroups = clsCustomer.fncGrpCommon("LIST", ss_lngOrgID, ss_lngUserID, UCase(strVerify))

                    '    ' Populate cmbRoles dropdown
                    '    cmbRoles.Items.Add(New ListItem("", ""))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_UploaderDesc, gc_UT_Uploader))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_ReviewerDesc, gc_UT_Reviewer))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_AuthDesc, gc_UT_Auth))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_ReportDownloaderDesc, gc_UT_ReportDownloader))
                    'ElseIf ss_strUserType = gc_UT_BankAdmin OrElse ss_strUserType = gc_UT_BankAuth Then
                    '    cmbRoles.Items.Add(New ListItem("", ""))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_BankUserRole, gc_UT_BankUser))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_BankOperatorRole, gc_UT_BankOperator))
                    '    cmbRoles.Items.Add(New ListItem(gc_UT_BankAdminRole, gc_UT_BankAdmin))
                    'End If


                    'Dim userTypeNames As String() = [Enum].GetNames(GetType(enmUserType))

                    'cmbRoles.Items.Add(New ListItem("", ""))

                    'For Each userType As String In userTypeNames

                    '    Dim parts() As String = userType.ToString().Split("_"c)

                    '    Dim firstValue = parts(0)
                    '    Dim secondValue = parts(1)

                    '    cmbRoles.Items.Add(New ListItem(secondValue + " - " + firstValue, firstValue))

                    'Next

                    ' Set the mode (view or edit)
                    'If strMod = "View" Then
                    '    'trBack.Visible = True
                    '    trSubmit.Visible = False
                    '    lblMessage.Text = ""
                    'End If

                    '' Disable fields for Inquiry Users
                    'If Session("SYS_TYPE") = "IU" Then
                    '    btnSubmit.Enabled = False
                    '    btnReset.Disabled = True
                    'End If
                End If

            Catch ex As Exception
                ' Log the error
                LogError("Page_Load - PG_RoleAccessRights")

            Finally
                ' Cleanup
                drGroups = Nothing
                clsGeneric = Nothing
                dsGroups = Nothing
                clsCommon = Nothing
                clsCustomer = Nothing
            End Try
        End Sub


        Private Sub BindDataToGrid()
            tableContainer1.Visible = False
            tableContainer.Visible = True
            Dim clsGeneric As New MaxPayroll.Generic
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of DataSet
            Dim dsLogs As New Data.DataSet


            Try


                'Fetch Data from Database
                dsLogs = clsBank.GetMenuName()

                If dsLogs.Tables("LOGS").Rows.Count > 0 Then
                    Repeater1.DataSource = dsLogs.Tables("LOGS")
                    Repeater1.DataBind()
                Else
                    'lblMessage.Text = "No Records Found"
                    Repeater1.DataSource = Nothing
                    Repeater1.DataBind()
                End If

            Catch ex As Exception
                Dim msg = ex.Message
            End Try
        End Sub

#End Region


        Protected Sub roleName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cmbRoles.SelectedIndexChanged
            ' Get the selected value of the dropdown
            Dim selectedRole As String = cmbRoles.SelectedItem.Text
            Dim selectedRole1 As String = cmbRoles.SelectedItem.Value
            Dim clsGeneric As New MaxPayroll.Generic
            Dim clsBank As New MaxPayroll.clsBank

            roleName = cmbRoles.SelectedValue

            'Create Instance of DataSet
            Dim dsLogs As New Data.DataSet

            'Fetch Data from Database
            dsLogs = clsBank.GetRoleRights(selectedRole1)

            If dsLogs.Tables("LOGS").Rows.Count > 0 Then
                Repeater1.Visible = True
                Repeater2.Visible = False
                tableContainer.Visible = True
                tableContainer1.Visible = False

                Repeater1.DataSource = dsLogs.Tables("LOGS")
                Repeater1.DataBind()
            Else
                Repeater1.Visible = True
                Repeater2.Visible = False
                BindDataToGrid()
            End If

        End Sub

        Protected Function GetEncrypterString(Id As String) As String
            Dim clsEncryption As New MaxPayroll.Encryption
            Return clsEncryption.Cryptography(Id)
        End Function



        Shared roleName As String

        <WebMethod>
        Public Shared Function SaveData(ByVal tableData As List(Of TableRowData)) As String

            Dim dataTable As New DataTable("RoleRights")
            dataTable.Columns.Add("Role_Name", GetType(String))
            dataTable.Columns.Add("Menu_Name", GetType(String))
            dataTable.Columns.Add("Page_Name", GetType(String))
            dataTable.Columns.Add("Rights", GetType(Boolean))

            Dim result As Boolean = False


            ' Iterate through the submitted data and process it as needed
            For Each row In tableData
                'Dim id As String = row.RightsId
                'Dim itemName As String = row.MenuName
                'Dim PageName As String = row.PageName
                'Dim isChecked As Boolean = row.Rights

                Dim row1 As DataRow = dataTable.NewRow()
                row1("Role_Name") = roleName  ' Assign values from the ArrayList
                row1("Menu_Name") = row.MenuName
                row1("Page_Name") = row.PageName
                row1("Rights") = row.Rights
                dataTable.Rows.Add(row1)

                ' You can now store this data in your database, log it, etc.
                ' For example, save the checkbox states to a database.

                If String.IsNullOrEmpty(roleName) Then
                    Return "Please select role name."
                End If
            Next

            Dim allFalse As Boolean = dataTable.AsEnumerable().All(Function(row) row.Field(Of Boolean)("Rights") = False)

            If allFalse Then
                Return "Please select at least one checkbox."
            End If

            Dim selected As String = roleName
            Dim conn As String = strConnection

            Using connection As New SqlConnection(conn)
                connection.Open()

                Dim deleteQuery As String = "DELETE FROM RoleRights WHERE Role_Name = @ConditionValue"

                Using command As New SqlCommand(deleteQuery, connection)
                    ' Add parameters to prevent SQL injection
                    command.Parameters.AddWithValue("@ConditionValue", selected)

                    ' Execute the DELETE query
                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

                    ' Display the result
                    Console.WriteLine(rowsAffected.ToString() & " row(s) deleted.")
                End Using

                ' Initialize SqlBulkCopy
                Using bulkCopy As New SqlBulkCopy(connection)
                    bulkCopy.DestinationTableName = "RoleRights" ' Name of the table in the SQL database

                    Try
                        bulkCopy.ColumnMappings.Add("Role_Name", "Role_Name")       ' Map DataTable "SourceID" to SQL table "ID"
                        bulkCopy.ColumnMappings.Add("Menu_Name", "Menu_Name")     ' Map DataTable "FullName" to SQL table "Name"
                        bulkCopy.ColumnMappings.Add("Page_Name", "Page_Name")
                        bulkCopy.ColumnMappings.Add("Rights", "Rights")

                        ' Write from the DataTable to the database
                        bulkCopy.WriteToServer(dataTable)
                        Console.WriteLine("Bulk insert successful!")
                        result = True

                    Catch ex As Exception
                        Console.WriteLine("Error during bulk insert: " & ex.Message)
                        result = False
                    End Try
                End Using
            End Using

            'Dim instance As New PG_RoleAccessRights()  ' Create an instance
            'instance.TestMethod(result)

            ' Return "Success"

            If result Then
                Return "Role rights saved successfully."
            Else
                Return "Failed to save role rights."
            End If


        End Function

        Private Sub btnClears_Click(sender As Object, e As EventArgs) Handles btnClears.Click

            BindRole()
            BindDataToGrid()

        End Sub

        Private Sub BindRole()


            Dim userTypeNames As String() = [Enum].GetNames(GetType(enmUserType))


            cmbRoles.Items.Clear()

            cmbRoles.Items.Add(New ListItem("", ""))

            For Each userType As String In userTypeNames

                Dim parts() As String = userType.ToString().Split("_"c)

                If (parts.Length >= 2) Then

                    Dim firstValue = parts(0)
                    Dim secondValue = parts(1)

                    cmbRoles.Items.Add(New ListItem(secondValue + " - " + firstValue, firstValue))
                End If

            Next

            cmbRoles.SelectedValue = Nothing

            roleName = ""
        End Sub
    End Class



    Public Class TableRowData
        Public Property RightsId As String
        Public Property MenuName As String
        Public Property PageName As String
        Public Property Rights As Boolean
    End Class

End Namespace
