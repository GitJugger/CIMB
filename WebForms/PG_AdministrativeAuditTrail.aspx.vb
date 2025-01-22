Option Strict Off
Option Explicit On


Namespace MaxPayroll


    Partial Class PG_AdministrativeAuditTrail
        Inherits clsBasePage

#Region "Declaration"
        'Private ReadOnly Property rq_strRequest()
        '    Get
        '        Return Request.QueryString("Log") & ""
        '    End Get
        'End Property
        'Private ReadOnly Property rq_strUserType()
        '    Get
        '        Return Request.QueryString("User") & ""
        '    End Get
        'End Property

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
        Public ReadOnly Property UserType As String
            Get
                ' Replace this with the actual logic to retrieve the user type
                Return HttpUtility.JavaScriptStringEncode("YourDynamicValue")
            End Get
        End Property
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not IsPostBack Then
                ' Get the current date
                Dim currentDate As DateTime = DateTime.Now

                ' Get the first day of the current month
                Dim firstDayOfMonth As DateTime = New DateTime(currentDate.Year, currentDate.Month, 1)

                ' Set the "From Date" as the first day of the current month
                textFromDt.Text = firstDayOfMonth.ToString("dd/MM/yyyy")

                ' Set the "To Date" as the current date
                textToDt.Text = currentDate.ToString("dd/MM/yyyy")
            End If

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic
            _names = Session("SYS_USERNAME")

            lblHeading.Text = "Audit Trail Report"



            Try
                If Not Len(ss_strUserType) = 2 Then
                    Server.Transfer(gc_LogoutPath, False)
                    Exit Try
                End If
                'If Not ss_strUserType = gc_UT_BankAdmin And Not ss_strUserType = gc_UT_BankAuth Then
                '    Server.Transfer(gc_LogoutPath, False)
                '    Exit Try
                'End If
                'If ss_strUserType = gc_UT_BankAdmin Then
                '    If Not rq_strUserType = gc_UT_BankUser And Not rq_strUserType = gc_UT_InquiryUser Then
                '        Server.Transfer(gc_LogoutPath, False)
                '        Exit Try
                '    End If
                'End If
                If Not Page.IsPostBack Then
                    'BindBody(body)

                    Call prcBindGrid()
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

        Private Sub prcBindGrid()
            'Create Instance of Bank Class Object
            Dim clsBank As New MaxPayroll.clsBank

            'Create Instance of DataSet
            Dim dsLogs As New Data.DataSet

            'Variable Declarations 
            Dim strFromDt As String, strToDt As String, strOption As String = "", strUserLogin As String, strUserName As String

            Try
                'Get Values
                strToDt = textToDt.Text
                strFromDt = textFromDt.Text
                strUserLogin = textUserId.Text
                strUserName = textUserName.Text
                'lblMessage.Text = ""

                If Not String.IsNullOrEmpty(strFromDt) Or Not String.IsNullOrEmpty(strToDt) Then
                    Dim format As String = "dd/MM/yyyy" ' Specify the exact format of the input string
                    Dim fromDate As Date = DateTime.ParseExact(textFromDt.Text, format, System.Globalization.CultureInfo.InvariantCulture)
                    Dim toDate As Date = DateTime.ParseExact(textToDt.Text, format, System.Globalization.CultureInfo.InvariantCulture)

                    strFromDt = fromDate.ToString("yyyy-MM-dd")
                    strToDt = toDate.ToString("yyyy-MM-dd")

                End If

                ''Select Option
                'If radUserId.Checked Then
                '    strOption = "User Id"
                'ElseIf radUserName.Checked Then
                '    strOption = "User Name"
                'ElseIf radFrom.Checked Then
                '    strOption = "From To"
                'ElseIf radAll.Checked Then
                '    strOption = ""
                'End If

                ''Validate Dates
                'If radFrom.Checked AndAlso (textFromDt.Text = "" Or textToDt.Text = "") Then
                '    'lblMessage.Text = "Please enter the From and To Date."
                '    Repeater1.Visible = False
                '    Exit Sub
                'End If

                'Fetch Data from Database
                dsLogs = clsBank.GetAuditTrail(strFromDt, strToDt, strUserLogin, strUserName)

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

                'Populate Datagrid
                Call prcBindGrid()

            Catch ex As Exception
                Me.LogError("GetAuditTrail - Search")
            End Try

        End Sub

#End Region

        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClears.Click
            Me.textFromDt.Text = ""
            Me.textToDt.Text = ""
            Me.textUserId.Text = ""
            Me.textUserName.Text = ""
            Me.radAll.Checked = True
            Repeater1.DataSource = Nothing
            Repeater1.DataBind()
        End Sub


    End Class

End Namespace
