Imports System.Data.SqlClient


Namespace MaxPayroll


Partial Class PG_SessionCheck
      Inherits clsBasePage

#Region " Web Form Designer Generated Code "

    'This call is required by the Web Form Designer.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        'CODEGEN: This method call is required by the Web Form Designer
        'Do not modify it using the code editor.
        InitializeComponent()
    End Sub

#End Region

#Region "Page Load"

    '****************************************************************************************************
    'Procedure Name : Page_Load()
    'Purpose        : Page Load
    'Arguments      : N/A
    'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/01/2006
        '*****************************************************************************************************
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

            'create instance of generic class object
            Dim clsGeneric As New MaxPayroll.Generic


         Try

            'Check if only Bank User or Bank Authorizer
            If Not (ss_strUserType = gc_UT_BankUser Or ss_strUserType = gc_UT_BankAuth) Then
               Server.Transfer(gc_LogoutPath, False)
               Exit Try
            End If

            If Not Page.IsPostBack Then
               'bind data grid
               Call prcBindDataGrid()
            End If

         Catch ex As Exception

            'log error
            LogError("PG_SessionCheck - Page_Load")

         Finally

            'destroy instance of generic class object
            clsGeneric = Nothing

         End Try

        End Sub

#End Region

#Region "Data Bind"

        '****************************************************************************************************
        'Procedure Name : prcBindDataGrid()
        'Purpose        : Bind Data Grid Function
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/01/2006
        '*****************************************************************************************************
        Private Sub prcBindDataGrid()

            'create instance of sql data reader
            Dim sdrSessionList As SqlDataReader

            'create instance of Sql command object
            Dim cmdSessionList As New SqlCommand

            'create instance of generic class object
            Dim clsGeneric As New MaxPayroll.Generic

            'variable declarations
            Dim strSQL As String

            Try

                If Session("SYS_TYPE") = "BU" Then
                    strSQL = "Select * From pg_ListSessCust"
                ElseIf Session("SYS_TYPE") = "BS" Then
                    strSQL = "Select * From pg_ListSessBank"
                End If

                'intialise sql connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdSessionList
                    .Connection = clsGeneric.SQLConnection
                    .CommandType = CommandType.Text
                    .CommandText = strSQL
                    sdrSessionList = .ExecuteReader()
                End With

                If sdrSessionList.HasRows Then
                    dgSessionList.DataSource = sdrSessionList
                    dgSessionList.DataBind()
                    sdrSessionList.Close()
                Else
                    trBtn.Visible = False
                    lblMsg.Text = "No Records available"
                End If


            Catch ex As Exception

                'log error
                Call clsGeneric.ErrorLog(0, 0, "PG_SessionCheck - prcBindDataGrid", Err.Number, ex.Message)

            Finally

                'terminate sql connection
                Call clsGeneric.SQLConnection_Terminate()

                'destroy instance of generic class object
                clsGeneric = Nothing

                'destroy instance of sql reader object
                sdrSessionList = Nothing

                'destroy instance of sql command object
                cmdSessionList = Nothing

            End Try

        End Sub

#End Region

#Region "Page_Change"

        '****************************************************************************************************
        'Procedure Name : Page_Change()
        'Purpose        : Page Navigation
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 14/02/2005
        '*****************************************************************************************************
        Sub Page_Change(ByVal Sender As Object, ByVal e As DataGridPageChangedEventArgs) Handles dgSessionList.PageIndexChanged

            'Variable Declarations
            Dim intStart As Integer

            Try
                intStart = dgSessionList.CurrentPageIndex * dgSessionList.PageSize
                dgSessionList.CurrentPageIndex = e.NewPageIndex
                Call prcBindDataGrid()

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Page Submit"

        '****************************************************************************************************
        'Procedure Name : Page_Submit()
        'Purpose        : Delete User Session
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 05/01/2006
        '*****************************************************************************************************
        Private Sub Page_Submit(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click

            'create instance of datagrid item
            Dim dgiSessionList As DataGridItem

            'create instance of user class object
            Dim clsUser As New MaxPayroll.clsUsers

            Try

                For Each dgiSessionList In dgSessionList.Items
                    Dim chkSelect As CheckBox = CType(dgiSessionList.FindControl("chkSelect"), CheckBox)
                    If chkSelect.Checked Then
                        Dim hUserId As HtmlInputHidden = CType(dgiSessionList.FindControl("hId"), HtmlInputHidden)
                        Call clsUser.fncSessionCheck("D", hUserId.Value)
                    End If
                Next

                Server.Transfer("PG_SessionCheck.aspx")

            Catch ex As Exception

            Finally

                'destroy instance of datagrid item
                dgiSessionList = Nothing

                'destroy instance of user class object
                clsUser = Nothing

            End Try

        End Sub


#End Region

End Class

End Namespace
