'******************************************************************
'Class Name     :   Encryption.vb
'Prog Id        :   MaxPayroll.Encryption
'Purpose        :   Generate User Id and Password Automatically
'Author         :   Sujith - 
'Created        :   27/08/2003
'Modified       :   02/11/2003
'******************************************************************
Option Strict Off
Option Explicit On

Imports System
Imports System.IO
Imports System.Text
Imports System.Data
Imports System.Security
Imports MaxPayroll.Generic
Imports System.Security.Cryptography


Namespace MaxPayroll


    Public Class Encryption

#Region "Global Variable"

        Dim arrKey(255), arrBox(255), intPwdLen As Int32

#End Region

#Region "Generate User Login Details"

        '*********************************************************************
        'Function Name      :   fncGenerate()
        'Purpose            :   To Generate User Id & Password
        'Arguments          :   User Name,Length
        'Return Value       :   String        
        'Author             :   Sujith - 
        'Created            :   21/02/2005
        '********************************************************************
        Public Sub fncGenerate(ByVal strUserName As String, ByVal intLength As Int16, _
                    ByRef strUserId As String, ByRef strPassword As String, ByRef strAuthCode As String)

            Try

                'Generate User Id
                strUserId = fncUserId(intLength, strUserName)

                'Generate Password
                strPassword = fncPassword(intLength)

                'Generate AuthCode
                strAuthCode = fncPassword(intLength)

            Catch ex As Exception

            End Try

        End Sub

#End Region

#Region "Cryptographic Methods"

        '****************************************************************************************************
        'Procedure Name : Cryptography()
        'Purpose        : Encrypt/Decrypt String
        'Arguments      : Calls The Encryption Function
        'Return Value   : Encrypted/Decrypted String
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/08/2003
        '*****************************************************************************************************
        Public Function Cryptography(ByVal strEncryptText As String) As String
            Return Cryption(strEncryptText)
        End Function

        '****************************************************************************************************
        'Procedure Name : Cryption()
        'Purpose        : Encrypt/Decrypt String
        'Arguments      : Encrypt/Decrypt Text Encrypt/Decrypt Password
        'Return Value   : Encrypted/Decrypted String
        'Author         : Sujith Sharatchandran - 
        'Created        : 26/08/2003
        '*****************************************************************************************************
        Private Function Cryption(ByVal strEncryptText As String) As String

            Dim intModResult As Int32
            Dim intModResult1 As Int32
            Dim strTemp As String
            Dim strConstant As String
            Dim intCounter As Int32
            Dim strCryptographBy As String
            Dim strCryptograph As String = ""

            If Len(strEncryptText) > 0 Then
                intModResult = 0
                intModResult1 = 0

                PwdInitialize("Gateway")
                For intCounter = 1 To Len(strEncryptText)

                    intModResult = (intModResult + 1) Mod 256
                    intModResult1 = (intModResult1 + arrBox(intModResult)) Mod 256
                    strTemp = arrBox(intModResult)
                    arrBox(intModResult) = arrBox(intModResult1)
                    arrBox(intModResult1) = strTemp

                    strConstant = arrBox((arrBox(intModResult) + arrBox(intModResult1)) Mod 256)

                    strCryptographBy = Asc(Mid(strEncryptText, intCounter, 1)) Xor strConstant

                    strCryptograph = strCryptograph & Chr(strCryptographBy)

                Next
            End If


            Return strCryptograph

        End Function

        '****************************************************************************************************
        'Procedure Name : PwdInitialize()
        'Purpose        : Password Initializer
        'Arguments      : Encrypt Password
        'Return Value   : N/A
        'Author         : Sujith Sharatchandran - 
        'Created        : 30/08/2003
        '*****************************************************************************************************
        Private Sub PwdInitialize(ByVal strEncryptPwd As String)

            Dim strSwap As String, intCounter As Int32, intModResult As Int32

            intPwdLen = Len(strEncryptPwd)
            For intCounter = 0 To 255
                arrKey(intCounter) = Asc(Mid(strEncryptPwd, (intCounter Mod intPwdLen) + 1, 1))
                arrBox(intCounter) = intCounter
            Next

            intModResult = 0
            For intCounter = 0 To 255
                intModResult = (intModResult + arrBox(intCounter) + arrKey(intCounter)) Mod 256
                strSwap = arrBox(intCounter)
                arrBox(intCounter) = arrBox(intModResult)
                arrBox(intModResult) = strSwap
            Next

        End Sub
#End Region

#Region "SQL Encrypt"


        Public Function fnSQLCrypt(ByVal strCryptText As String) As String

            Dim strTempChar As String = "", intCounter As Integer

            Try

                For intCounter = 1 To Len(strCryptText)

                    If Asc(Mid$(strCryptText, intCounter, 1)) < 128 Then
                        strTempChar = CType(Asc(Mid$(strCryptText, intCounter, 1)) + 128, String)
                    ElseIf Asc(Mid$(strCryptText, intCounter, 1)) > 128 Then
                        strTempChar = CType(Asc(Mid$(strCryptText, intCounter, 1)) - 128, String)
                    End If
                    Mid$(strCryptText, intCounter, 1) = Chr(CType(strTempChar, Integer))

                Next intCounter

                Return strCryptText

            Catch ex As Exception

            End Try
            Return Nothing
        End Function

#End Region

#Region "Generate User Id"

        '*********************************************************************
        'Function Name      :   fncUserId()
        'Purpose            :   To Generate User Id
        'Arguments          :   User Name
        'Return Value       :   String        
        'Author             :   Sujith - 
        'Created            :   21/02/2005
        '********************************************************************
        Private Function fncUserId(ByVal intLength As Int16, ByVal strUserName As String) As String

            'Create Instance of Random class object
            Dim clsRandom As New Random

            'Variable Declarations
            Dim strRandomNo As String, strUserId As String

            Try

                'get the first 4 characters from the user name
                strUserName = Left(strUserName, 4)

                'generate random number
                strRandomNo = clsRandom.Next().ToString()

                'get the first four numbers 
                strRandomNo = Left(strRandomNo, 4)

                'build user id
                strUserId = strUserName & strRandomNo

                Return strUserId

            Catch ex As Exception

            Finally

                'destroy instance of random class object
                clsRandom = Nothing

            End Try
            Return Nothing
        End Function

#End Region

#Region "Check Invalid Characters"

        '**********************************************************************************
        'Procedure Name     :   fncInvalidCheck()
        'Purpose            :   Replace Single Quote and Hypen with Vowels
        'Arugments          :   strCheckText
        'Return Value       :   String
        'Author             :   Sujith - 
        'Created            :   21/02/2005
        '*********************************************************************************
        Private Function fncInvalidCheck(ByVal strCheckText As String) As String

            'Variable Declarations
            Dim strReplace As String, strData As String
            Const strVocal = "aeiouabcefghijklmnoprsut"

            Try
                'Assign text to be Checked
                strData = strCheckText

                'Find Hyphen - START
                If InStr(strData, "-") > 0 Then
                    strReplace = Mid(strVocal, Len(strVocal) * (Rnd()) + 1, 1)
                    strData = Replace(strData, "-", strReplace)
                End If
                'Find Hyphen - STOP

                'Find Single Quotes - START
                If InStr(strData, "'") > 0 Then
                    strReplace = Mid(strVocal, Len(strVocal) * (Rnd()) + 1, 1)
                    strData = Replace(strData, "'", strReplace)
                End If
                'Find Single Quotes - STOP

                Return strData

            Catch ex As Exception

            End Try
            Return Nothing
        End Function

#End Region

#Region "Generate Password"

        '***********************************************************************
        'Procedure Name     :   fncPassword()
        'Purpose            :   Generate the Password
        'Arguments          :   N/A
        'Return Value       :   N/A
        'Author             :   Sujith - 
        'Created            :   21/02/2005
        '***********************************************************************
        Public Function fncPassword(ByVal intLength As Int16) As String

            'Variable Declarations
            Dim strGeneratedPassword As String

            Try

                'generate password
                strGeneratedPassword = System.Guid.NewGuid().ToString

                'remove hyphen
                strGeneratedPassword = Replace(strGeneratedPassword, "-", "")

                'get only required length of characters
                strGeneratedPassword = Left(strGeneratedPassword, intLength)

                Return strGeneratedPassword

            Catch ex As Exception

            End Try
            Return Nothing
        End Function

#End Region

    End Class

End Namespace
