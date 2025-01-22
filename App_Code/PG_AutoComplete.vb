#Region "Imports "

Imports System.Web
Imports MaxGeneric
Imports System.Web.Services
Imports System.Collections.Generic
Imports System.Reflection.MethodBase
Imports System.Web.Services.Protocols

#End Region

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class PG_AutoComplete
    Inherits System.Web.Services.WebService

#Region "Global Variables "

    'Create Instances - Start
    Private _WebHelper As New WebHelper
    Private _WebCommon As New WebCommon
    Private _DataBase As New MaxModule.DataBase
    'Create Instances - Stop

#End Region

#Region "Get Account Number "

    'Author: Sujith Sharatchandran
    'Purpose: To Get the Account Number for autocomplete option
    <WebMethod(EnableSession:=True)> _
    Public Function GetAccountNo(ByVal PrefixText As String, ByVal Count As Short, _
        ByVal OrganizationId As Integer, ByVal GroupId As Integer) As String()

        'Create Instances - Start
        Dim _DataRow As DataRow = Nothing
        Dim AccountNumbers As DataTable = Nothing
        'Create Instances - Stop

        'Variable Declarations
        Dim SqlStatement As String = Nothing

        Try

            'Get the AccountNumbers
            AccountNumbers = _DataBase.GetData(SqlStatement)

            'Declare Array List
            Dim AccountNumberList As New List(Of String)(AccountNumbers.Rows.Count)

            'Loop thro the Account Numbers - Start
            For Each _DataRow In AccountNumbers.Rows

                'Add Account Number to Array List
                AccountNumberList.Add(_DataRow(_WebHelper.ColAccountNo))

            Next
            'Loop thro the Account Numbers - Stop

            Return AccountNumberList.ToArray()

        Catch ex As Exception

            'Log Error - Start
            Call _WebCommon.ErrorLog(OrganizationId, GetCurrentMethod().ToString(), ex.Message)

            Return Nothing

        End Try

    End Function

#End Region

End Class