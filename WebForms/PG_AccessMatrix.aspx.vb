Option Strict Off
Option Explicit On


Namespace MaxPayroll


    Partial Class PG_AccessMatrix
        Inherits clsBasePage

#Region "Declaration"


        Private _names As String
        Public Property Names As String
            Get
                Return _names
            End Get
            Set(value As String)
                _names = value
            End Set
        End Property

        Public Sub New()
            ' Initialization code here
            ' Note: This constructor is not typically used for web form initialization

            'Dim currentDate As Date = Date.Now
            'Dim firstDateOfMonth As Date = New Date(currentDate.Year, currentDate.Month, 1)

            'textFromDt.Text = firstDateOfMonth.ToString("dd/MM/yyyy")
            'textToDt.Text = currentDate.ToString("dd/MM/yyyy")

        End Sub

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
            If Not IsPostBack Then
                ' Get the current date
                Dim currentDate As DateTime = DateTime.Now

                ' Get the first day of the current month
                Dim firstDayOfMonth As DateTime = New DateTime(currentDate.Year, currentDate.Month, 1)




            End If

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            _names = Session("SYS_USERNAME")

            lblHeading.Text = "Role Access Matrix Report"



            Try
                If Not Len(ss_strUserType) = 2 Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If

                If Not Page.IsPostBack Then
                    Call prcBindGrid("ALL")
                    BindRole()
                End If

            Catch ex As Exception

            Finally

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Sub



#End Region


#Region "Bind Grid"

        '****************************************************************************************************
        'Procedure Name : prcBindGrid()
        'Purpose        : Populate Data Grid
        'Arguments      : N/A
        'Return Value   : N/A
        'Author         : Jaganraj
        'Created        : 11/09/2024
        '*****************************************************************************************************

        Private Sub prcBindGrid(ByVal menuName As String)
            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of DataSet
            Dim dsLogs As New Data.DataSet

            Try

                'lblMessage.Text = ""

                'Fetch Data from Database
                dsLogs = clsBank.GetRoleRightsAll(menuName)

                If dsLogs.Tables("LOGS").Rows.Count > 0 Then
                    Repeater1.DataSource = dsLogs.Tables("LOGS")
                    Repeater1.DataBind()
                Else
                    'lblMessage.Text = "No Records Found"
                    Repeater1.DataSource = Nothing
                    Repeater1.DataBind()
                End If

            Catch ex As Exception
                'Handle Exception
            End Try
        End Sub


#End Region

#Region "Search Button"

        Private Sub prcSearch(ByVal O As System.Object, ByVal E As System.EventArgs) Handles btnSearchs.Click

            Try

                Dim menuName As String = cmbRoles.SelectedValue

                Call prcBindGrid(menuName)

            Catch ex As Exception
                ' Me.LogError("GetAccessMatrix - Search")
            End Try

        End Sub

#End Region

        Private Sub BindRole()

            cmbRoles.Items.Clear()
            cmbRoles.Items.Add(New ListItem("ALL", "ALL"))

            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of DataSet
            Dim dsLogs As New Data.DataSet

            Try

                'lblMessage.Text = ""

                'Fetch Data from Database
                dsLogs = clsBank.GetMenuDistinct()

                If dsLogs.Tables("LOGS").Rows.Count > 0 Then

                    For Each row As DataRow In dsLogs.Tables("LOGS").Rows

                        Dim test As String = row("MenuName")

                        cmbRoles.Items.Add(New ListItem(test, test))

                    Next


                End If

            Catch ex As Exception
                'Handle Exception
            End Try


        End Sub

        Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
            BindRole()
            prcBindGrid("ALL")

        End Sub
    End Class

End Namespace
