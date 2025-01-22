Imports Microsoft.VisualBasic

Public Class clsCIMBReports

    Inherits ArrayList
    Public Enum ReportFields
        FileName
        FileDateTime
    End Enum 'ASNListFields

    Public Overloads Sub Sort(ByVal sortField As ReportFields, ByVal isAscending As Boolean)
        Select Case sortField
            Case ReportFields.FileDateTime
                MyBase.Sort(New FileDateTimeComparer)
        End Select

        If Not isAscending Then
            MyBase.Reverse()
        End If
    End Sub 'Sort

    Private NotInheritable Class FileDateTimeComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim first As clsCIMBReport = CType(x, clsCIMBReport)
            Dim second As clsCIMBReport = CType(y, clsCIMBReport)
            Return first.FileDateTime.CompareTo(second.FileDateTime)
        End Function 'Compare
    End Class 'ASNNoComparer
End Class
