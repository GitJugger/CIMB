Imports System.Data
Imports MaxPayroll.Generic
Imports System.data.SqlClient


Namespace MaxPayroll


Public Class clsCheckDigit

#Region "Bank Account Check Digit"

    '****************************************************************************************************
    'Procedure Name : fnCheckDigitBank()
    'Purpose        : To Assign Check Digit to the Given Account No
    'Arguments      : Account No
    'Return Value   : Account No Along With the Check Digit
        'Author         : Sujith Sharatchandran - 
    'Created        : 08/10/2003
    '*****************************************************************************************************
        'Public Function fncCheckDigitBank_RHB(ByVal strAccountNo As String) As Boolean

        '    'Variable Declarations
        '    Dim strCheckDigit As String
        '    Dim intReminder As Int16
        '    Dim intCounter As Int16
        '    Dim intAccNo As Int16
        '    Dim intWeight As Int16
        '    Dim intMultiple As Int16
        '    Dim intBalance As Int16
        '    Dim intTotal As Int16
        '    Dim intCheckDigit As Int16
        '    Dim strWeight As String

        '    If IsNumeric(strAccountNo) Then

        '        'Get the Assigned Weight
        '        strWeight = "6543298765432"

        '        'Loop thro the Entire String
        '        For intCounter = 1 To 13

        '            'Get the First Digit of the Account No
        '            intAccNo = Mid(strAccountNo, intCounter, 1)
        '            'Get the First Digit of the Weight Assigned
        '            intWeight = Mid(strWeight, intCounter, 1)
        '            'Get the value By Multiplying Account No and Weight Assigned
        '            intMultiple = intAccNo * intWeight
        '            'Sum the Result from Above
        '            intTotal = intTotal + intMultiple

        '        Next

        '        'Get the Mod of the Total With 11 
        '        intReminder = intTotal Mod 11

        '        'Deduct The Remaining Balance From 10
        '        intBalance = 10 - intReminder

        '        'Get the Last Digit From Above Result, Which Shall be the Check Digit
        '        intCheckDigit = Right(intBalance, 1)

        '        'Return Account Number With Computed Check Digit
        '        strCheckDigit = Left(strAccountNo, 13) & intCheckDigit

        '        If strCheckDigit = strAccountNo Then
        '            Return True
        '        Else
        '            Return False
        '        End If

        '    End If

        'End Function

        Public Function fncCheckDigitBank_CIMB(ByVal strAccountNo As String) As Boolean
            Dim intResult, intCheckDigit As Integer
            Dim strCheckDigit As String

            If Not IsNumeric(strAccountNo) Then
                Return False
            End If

            intResult = cal(strAccountNo)

            If intResult = 10 Then
                Dim temp As Int16
                temp = Mid(strAccountNo, 13, 1)

                If temp = 9 Then
                    temp = 0
                Else
                    temp += 1
                End If

                Dim sTempAcNo As String
                sTempAcNo = strAccountNo.Substring(0, 11) + temp.ToString + strAccountNo.Substring(13, 1)
                intCheckDigit = cal(sTempAcNo)

            ElseIf intResult = 11 Then
                intCheckDigit = 0
            Else
                intCheckDigit = intResult

            End If

            strCheckDigit = Left(strAccountNo, 13) & intCheckDigit

            If strCheckDigit.ToString = strAccountNo.ToString Then
                Return True
            Else
                Return False
            End If

        End Function

        Function cal(ByVal strAccountNo As String) As Integer
            Dim strWeight As String() = {8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2}
            Dim intCounter, intAccNo, intWeight, intMultiple, intTotal, intRemainder, intResult As Integer

            Dim lngAccountNo As Long = CLng(strAccountNo)

            For intCounter = 1 To 13

                intAccNo = Mid(lngAccountNo, intCounter, 1)
                intWeight = strWeight(intCounter - 1)
                intMultiple = intAccNo * intWeight
                intTotal = intTotal + intMultiple
            Next

            intRemainder = intTotal Mod 11
            intResult = 11 - intRemainder

            Return intResult
        End Function

#End Region

#Region "EPF Account Check Digit"

    '****************************************************************************************************
    'Procedure Name : fnCheckDigitEPF()
    'Purpose        : To Assign Check Digit to the Given EPF Employer No
    'Arguments      : EPF No
    'Return Value   : Account No Along With the Check Digit
        'Author         : Sujith Sharatchandran - 
    'Created        : 08/10/2003
    '*****************************************************************************************************
    Public Function fncCheckDigitEPF(ByVal strEPFNo As String) As Boolean

        'Variable Declarations
        Dim strCheckDigit As String, intReminder As Int16
        Dim intCounter As Int16, intAccNo As Int16, intWeight As Int16, intMultiple As Int16
        Dim intBalance As Int16, intTotal As Int16, intCheckDigit As Int16, strWeight As String

        Try

            'Get the Assigned Weight
            strWeight = "32765432"

            'Loop thro the Entire String
            For intCounter = 1 To 8
                'Get the First Digit of the Account No
                intAccNo = Mid(strEPFNo, intCounter, 1)
                'Get the First Digit of the Weight Assigned
                intWeight = Mid(strWeight, intCounter, 1)
                'Get the value By Multiplying Account No and Weight Assigned
                intMultiple = intAccNo * intWeight
                'Sum the Result from Above
                intTotal = intTotal + intMultiple
            Next

            'Get the Mod of the Total With 11 
            intReminder = intTotal Mod 11

            'Deduct The Remaining Balance From 11
            intBalance = 11 - intReminder

            'Get Check Digit - Start
            If intBalance > 9 Then
                intCheckDigit = 0
            Else
                intCheckDigit = intBalance
            End If
            'Get Check Digit - Stop

            'Return EPF Number With Computed Check Digit
            strCheckDigit = Left(strEPFNo, 8) & intCheckDigit

            'If Valid EPF number return true
            If strEPFNo = strCheckDigit Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception

            Return False

        End Try

        
    End Function

#End Region

#Region "SOCSO Check Digit"

    '****************************************************************************************************
    'Procedure Name : fncCheckDigitSOCSO()
    'Purpose        : To Assign Check Digit to the Given Socso Employer Number
    'Arguments      : Socso No
    'Return Value   : Boolean
        'Author         : Sujith Sharatchandran - 
    'Created        : 19/10/2005
    '*****************************************************************************************************
    Public Function fncCheckDigitSOCSO(ByVal strEmpSocNo As String) As Boolean

        'Variable Declarations
        Dim intRemainder As Int16, strCheckDigitTable As String, intCheckDigit As Int16
        Dim intFindZone As Int16, intCheckThird As Int16, intComputeCheckTotal As Int16
        Dim strZone As String, strZoneValue As String, intZoneNo As Int16, strCheckDigit As String

        Try

            strZone = Mid(strEmpSocNo, 1, 1)                                        'Get Zone Alphabet
            intZoneNo = Mid(strEmpSocNo, 2, 2)                                      'Get Zone Number
            intCheckThird = Mid(strEmpSocNo, 4, 1)                                  'Get Third Object
                strZoneValue = System.Configuration.ConfigurationManager.AppSettings(strZone)               'Get Zone Value
                strCheckDigitTable = System.Configuration.ConfigurationManager.AppSettings("SOCTABLE")      'Get Check Digit Table
            intFindZone = InStr(strZoneValue, intZoneNo)                            'Check if Zone Aplabet & Zone Number Valid

            'If Valid Zone and Third Digit Zero
            If intFindZone > 0 And intCheckThird = 0 Then
                intComputeCheckTotal = ((Mid(strEmpSocNo, 2, 1)) * 64) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 3, 1)) * 32) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 5, 1)) * 16) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 6, 1)) * 8) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 7, 1)) * 4) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 8, 1)) * 2) + intComputeCheckTotal
                'If Valid Zone and Third digit not zero
            ElseIf intFindZone > 0 And Not intCheckThird = 0 Then
                intComputeCheckTotal = ((Mid(strEmpSocNo, 2, 1)) * 128) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 3, 1)) * 64) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 4, 1)) * 32) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 5, 1)) * 16) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 6, 1)) * 8) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 7, 1)) * 4) + intComputeCheckTotal
                intComputeCheckTotal = ((Mid(strEmpSocNo, 8, 1)) * 2) + intComputeCheckTotal
            End If

            'Get Reminder Using Modulus 11
            intRemainder = intComputeCheckTotal Mod 11

            'Subtract 11 with the remmainder
            intCheckDigit = 11 - intRemainder

            'Get Check Digit
            strCheckDigit = Mid(strCheckDigitTable, intCheckDigit, 1)

            'Check if check digit is valid for the given Socso No
            If strCheckDigit = Right(strEmpSocNo, 1) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception

            Return False

        End Try

    End Function

#End Region

#Region "LHDN Check Digit"

        Public Function fncCheckDigitLHDN(ByVal strLHDNNo As String) As Boolean

            Dim k(8) As Integer
            Dim j As Integer
            Dim tempM As String
            Dim m As Integer
            Dim m1 As Integer
            Dim u As Integer
            Dim i As Int16
            Dim h As Int16 = 9
            Dim g As Int16 = 0
            Dim intResult As Int16

            Try
                For i = 1 To 8
                    k(i) = Val(strLHDNNo.Chars(g)) * h
                    h -= 1
                    g += 1
                Next

                For i = 1 To 8
                    j += k(i)
                Next

                'tempM = j / 11
                'tempM = String.Format(tempM, "{0:D}")
                m = Math.DivRem(j, 11, CLng(tempM))


                'm = CInt(tempM.Substring(0, 2))
                m1 = j - (m * 11)
                u = 11 - m1

                If u = 11 Then
                    intResult = 0
                Else
                    intResult = u
                End If

                If CInt(strLHDNNo.Substring(8)) = intResult Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Return False
            End Try

        End Function

#End Region
End Class

End Namespace
