Option Explicit On
Option Strict On

Imports Microsoft.VisualBasic.CompilerServices

Public Class clsDB
    ' Nested Types
    Public Enum SQLDataTypes
        ' Fields
        Dt_Boolean = 0
        Dt_Date = 3
        Dt_DateTime = 6
        Dt_Double = 5
        Dt_Integer = 2
        Dt_LikeAnyWhere = 8
        Dt_LikeBegining = 9
        Dt_LikeEnd = 10
        Dt_NullValue = 12
        Dt_String = 1
        Dt_Sysdate = 4
        Dt_Time = 7
        Dt_UnicodeString = 11
    End Enum

    Public Shared Function SQLStr(ByVal strVal As Object, Optional ByVal sType As SQLDataTypes = SQLDataTypes.Dt_String) As String
        Dim text1 As String = ""
        Dim time1 As DateTime
        Select Case sType
            Case SQLDataTypes.Dt_Boolean
                Return StringType.FromObject(Interaction.IIf(BooleanType.FromObject(strVal), 1, 0))
            Case SQLDataTypes.Dt_String
                If (Strings.InStr(StringType.FromObject(strVal), "'", CompareMethod.Binary) > 0) Then
                    Return ("'" & Strings.Replace(StringType.FromObject(strVal), "'", "''", 1, -1, CompareMethod.Binary) & "'")
                End If
                Return ("'" & StringType.FromObject(strVal) & "'")
            Case SQLDataTypes.Dt_Integer
                Return IntegerType.FromObject(strVal).ToString
            Case SQLDataTypes.Dt_Date
                time1 = DateType.FromObject(strVal)
                Return ("'" & time1.ToString("yyyy-MM-dd") & "'")
            Case SQLDataTypes.Dt_Sysdate
                Return StringType.FromObject(strVal)
            Case SQLDataTypes.Dt_Double
                Return DoubleType.FromObject(strVal).ToString
            Case SQLDataTypes.Dt_DateTime
                time1 = DateType.FromObject(strVal)
                Return ("'" & time1.ToString("yyyy-MM-dd h:mm:ss tt") & "'")
            Case SQLDataTypes.Dt_Time
                time1 = DateType.FromObject(strVal)
                Return ("'" & time1.ToString("h:mm:ss tt") & "'")
            Case SQLDataTypes.Dt_LikeAnyWhere
                If (Strings.InStr(StringType.FromObject(strVal), "'", CompareMethod.Text) > 0) Then
                    Return (" LIKE '%" & Strings.Replace(StringType.FromObject(strVal), "'", "''", 1, -1, CompareMethod.Binary) & "%'")
                End If
                Return (" LIKE '%" & StringType.FromObject(strVal) & "%'")
            Case SQLDataTypes.Dt_LikeBegining
                If (Strings.InStr(StringType.FromObject(strVal), "'", CompareMethod.Text) > 0) Then
                    Return (" LIKE '%" & Strings.Replace(StringType.FromObject(strVal), "'", "''", 1, -1, CompareMethod.Binary) & "'")
                End If
                Return (" LIKE '%" & StringType.FromObject(strVal) & "'")
            Case SQLDataTypes.Dt_LikeEnd
                If (Strings.InStr(StringType.FromObject(strVal), "'", CompareMethod.Text) > 0) Then
                    Return (" LIKE '" & Strings.Replace(StringType.FromObject(strVal), "'", "''", 1, -1, CompareMethod.Binary) & "'")
                End If
                Return (" LIKE '" & StringType.FromObject(strVal) & "%'")
            Case SQLDataTypes.Dt_UnicodeString
                If (Strings.InStr(StringType.FromObject(strVal), "'", CompareMethod.Binary) > 0) Then
                    Return ("N'" & Strings.Replace(StringType.FromObject(strVal), "'", "''", 1, -1, CompareMethod.Binary) & "'")
                End If
                Return ("N'" & StringType.FromObject(strVal) & "'")
            Case SQLDataTypes.Dt_NullValue
                Return "NULL"
        End Select
        Return text1
    End Function

    Public Shared Function ConcateConnectionString(ByVal sServerID As String, ByVal sUserID As String, ByVal sPassword As String, Optional ByVal sDBname As String = "") As String
        If Len(sDBname) = 0 Then
            Return "SERVER=" & sServerID & ";DATABASE=master;UID=" & sUserID & ";PWD=" & sPassword
        Else
            Return "SERVER=" & sServerID & ";DATABASE=" & sDBname & ";UID=" & sUserID & ";PWD=" & sPassword
            '    Return "Network Library=DBMSSOCN;Data Source=" & sServerID & ";Initial Catalog=master;Persist Security Info=True;User Id=" & sUserID & ";Password=" & sPassword
            'Else
            '    Return "Network Library=DBMSSOCN;Data Source=" & sServerID & ";Initial Catalog=" & sDBname & ";Persist Security Info=True;User Id=" & sUserID & ";Password=" & sPassword
        End If
    End Function

    Public Shared Function SQLAddAND(ByVal sParam As String, ByVal sValue As String) As String
        If Len(sParam.Trim) > 0 Then
            Return sParam & " And " & sValue
        Else
            Return sValue
        End If
    End Function

    Public Shared Function SQLAddOR(ByVal sParam As String, ByVal sValue As String) As String
        If Len(sParam.Trim) > 0 Then
            Return sParam & " Or " & sValue
        Else
            Return sValue
        End If
    End Function

    Public Shared Function SQLAddAND(ByVal sValue As String) As String
        If Len(sValue) > 0 Then
            Return " And " & sValue
        Else
            Return sValue
        End If
    End Function

    Public Shared Function SQLAddComma(ByVal sValue1 As String, ByVal sValue2 As String) As String
        If Len(sValue1.Trim) > 0 Then
            Return ", " & sValue2
        Else
            Return sValue2
        End If
    End Function

    Public Shared Function SQLAddOR(ByVal sValue As String) As String
        If Len(sValue) > 0 Then
            Return " Or " & sValue
        Else
            Return sValue
        End If
    End Function

    Public Shared Function SQLAddWhere(ByVal sValue As String) As String
        If sValue.ToUpper.StartsWith(" AND ") Then
            sValue = sValue.Substring(5)
        ElseIf sValue.ToUpper.StartsWith("AND ") Then
            sValue = sValue.Substring(4)
        ElseIf sValue.ToUpper.StartsWith(" OR ") Then
            sValue = sValue.Substring(4)
        ElseIf sValue.ToUpper.StartsWith("OR ") Then
            sValue = sValue.Substring(3)
        End If
        If Len(sValue) > 0 Then
            Return " Where " & sValue
        Else
            Return sValue
        End If
    End Function

    Public Shared Function SQLDateRange(ByVal sFieldName As String, ByVal dtDateFrom As Date, ByVal dtDateTo As Date) As String
        Dim sRetVal As String

        If IsNothing(dtDateFrom) Then
            dtDateFrom = Date.MinValue
        End If

        If IsNothing(dtDateTo) Then
            dtDateTo = Date.MinValue
        End If

        If dtDateFrom > Date.MinValue AndAlso dtDateTo > Date.MinValue Then
            sRetVal = sFieldName & " Between " & SQLStr(dtDateFrom, SQLDataTypes.Dt_Date) & " And " & SQLStr(dtDateTo, SQLDataTypes.Dt_Date)
        ElseIf dtDateFrom > Date.MinValue AndAlso dtDateTo = Date.MinValue Then
            sRetVal = sFieldName & " > " & SQLStr(dtDateFrom, SQLDataTypes.Dt_Date)
        ElseIf dtDateTo > Date.MinValue AndAlso dtDateTo = Date.MinValue Then
            sRetVal = sFieldName & " < " & SQLStr(dtDateTo, SQLDataTypes.Dt_Date)
        Else
            sRetVal = ""
        End If
        Return sRetVal
    End Function

    Public Shared Function ConvertSQLStr(ByVal sValue As String, ByVal sDataType As String) As String
        Try
            Select Case sDataType.ToLower
                Case "bigint", "int", "numeric", "smallint", "tinyint"
                    If Len(sValue.Trim) = 0 Then
                        sValue = "0"
                    End If
                    Return SQLStr(sValue, SQLDataTypes.Dt_Integer)
                Case "money", "real", "smallmoney", "decimal", "float"
                    If Len(sValue.Trim) = 0 Then
                        sValue = "0"
                    End If
                    Return SQLStr(sValue, SQLDataTypes.Dt_Double)
                Case "bit"
                    If Len(sValue.Trim) = 0 Then
                        sValue = "0"
                    End If
                    Return SQLStr(sValue, SQLDataTypes.Dt_Boolean)
                Case "datetime", "smalldatetime"
                    If Len(sValue.Trim) = 0 Then
                        sValue = DateTime.MinValue.ToString
                    End If
                    Return SQLStr(sValue, SQLDataTypes.Dt_DateTime)
                Case Else
                    Return SQLStr(sValue)
            End Select
        Catch ex As Exception
            Return SQLStr(sValue)
        End Try


    End Function

End Class

