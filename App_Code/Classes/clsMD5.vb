Imports Microsoft.VisualBasic

Imports System
Imports System.Security.Cryptography
Imports System.Data
Imports MaxPayroll.Generic
Imports System.Data.SqlClient
Imports System.Text
Namespace MaxPayroll

    Public Class clsMD5

#Region "Get MD5Hash"

        '**********************************************************************************
        'Procedure Name     :   fncGetMD5Hash()
        'Purpose            :   To hash the file
        'Arugments          :   input
        'Return Value       :   hexadecimal string - hash code
        'Author             :   Eu Yean Lock - T-Melmax Sdn Bhd
        'Created            :   17/05/2007
        '*********************************************************************************

        ' Hash an input string and return the hash as
        ' a 32 character hexadecimal string.
        Public Function getMd5Hash(ByVal input As String) As String

            ' Create a new instance of the MD5CryptoServiceProvider object.
            Dim md5Hasher As New MD5CryptoServiceProvider

            ' Create a new Stringbuilder to collect the bytes
            ' and create a string.
            Dim sBuilder As New StringBuilder

            Try

                ' Convert the input string to a byte array and compute the hash.
                Dim data As Byte() = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input))

                ' Loop through each byte of the hashed data 
                ' and format each one as a hexadecimal string.
                Dim i As Integer
                For i = 0 To data.Length - 1
                    sBuilder.Append(data(i).ToString("x2"))
                Next i

                ' Return the hexadecimal string.
                Return sBuilder.ToString()

            Catch ex As Exception
                Throw ex
            Finally

                'Destroy MD5CryptoServiceProvider object.
                md5Hasher = Nothing

                'Destroy Stringbuilder Object
                sBuilder = Nothing

            End Try

        End Function

#End Region

#Region "Get Verify MD5Hash"

        ''**********************************************************************************
        ''Procedure Name     :   fncVerifyMD5Hash()
        ''Purpose            :   To verify hash on file
        ''Arugments          :   strfile, hash
        ''Return Value       :   Byte - hash code
        ''Author             :   Eu Yean Lock - T-Melmax Sdn Bhd
        ''Created            :   17/05/2007
        ''*********************************************************************************
        '' Verify a hash against a string.
        'Function verifyMd5Hash(ByVal input As String, ByVal hash As String) As Boolean
        '    ' Hash the input.
        '    Dim hashOfInput As String = getMd5Hash(input)

        '    ' Create a StringComparer an comare the hashes.
        '    Dim comparer As StringComparer = StringComparer.OrdinalIgnoreCase

        '    If 0 = Comparer.Compare(hashOfInput, hash) Then
        '        Return True
        '    Else
        '        Return False
        '    End If

        'End Function

#End Region

#Region "Insert Hash Code"

        '****************************************************************************************************
        'Procedure Name : fncInsertHashCode()
        'Purpose        : Insert Hash Code
        'Arguments      : Action - Insert
        'Return Value   : Boolean
        'Author         : Eu Yean Lock - T-Melmax Sdn Bhd
        'Created        : 21/05/2007
        '*****************************************************************************************************
        Public Function fncInsertHashCode(ByVal lngFileId As Long, ByVal HashCode As String) As Boolean

            'Create Instance SQL Transaction
            Dim trnMD5Hash As SqlTransaction

            'Create Instance of SQL Command Object
            Dim cmdMD5Hash As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Begin Transaction 
                trnMD5Hash = clsGeneric.SQLConnection.BeginTransaction()

                With cmdMD5Hash
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trnMD5Hash
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_InsertHashCode"
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_HashCode", HashCode))
                    .ExecuteNonQuery()
                End With

                trnMD5Hash.Commit()

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fncInsertHashCode - clsMd5Hash", Err.Number, Err.Description)
                trnMD5Hash.Rollback()

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Insert Digital Signature"

        '****************************************************************************************************
        'Procedure Name : fncInsertDigSignature()
        'Purpose        : Insert Digital Signature
        'Arguments      : Action - Insert
        'Return Value   : Boolean
        'Author         : Eu Yean Lock - T-Melmax Sdn Bhd
        'Created        : 04/06/2007
        '*****************************************************************************************************
        Public Function fncInsertDigSignature(ByVal lngFileId As Long, ByVal lngUserId As Long, ByVal strDSignature As String) As Boolean

            'Create Instance SQL Transaction
            Dim trxDigSignature As SqlTransaction

            'Create Instance of SQL Command Object
            Dim cmdDigSignature As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            Try

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                'Begin Transaction 
                trxDigSignature = clsGeneric.SQLConnection.BeginTransaction()

                With cmdDigSignature
                    .Connection = clsGeneric.SQLConnection
                    .Transaction = trxDigSignature
                    .CommandType = CommandType.StoredProcedure
                    .CommandText = "pg_InsertDigSignature"
                    .Parameters.Add(New SqlParameter("@in_FileId", lngFileId))
                    .Parameters.Add(New SqlParameter("@in_UserId", lngUserId))
                    .Parameters.Add(New SqlParameter("@in_DigSignature", strDSignature))
                    .ExecuteNonQuery()
                End With

                trxDigSignature.Commit()

                Return True

            Catch

                'Log Error
                Call clsGeneric.ErrorLog(0, 0, "fncInsertDigSignature - clsMd5Hash", Err.Number, Err.Description)
                trxDigSignature.Rollback()

                Return False

            Finally

                'Terminate SQL Connection
                Call clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

#Region "Upload Check"

        '*****************************************************************************************************
        'Author         : Marcus Yap Poh Ching 
        'Created        : 06/03/2008
        '*****************************************************************************************************
        Public Function fncMD5Validation(ByVal lngOrgId As Long, ByVal lngUserId As Long, ByVal strFileType As String, ByVal strMD5Hash As String) As Int16

            'Create Instance of SQL Command Object
            Dim cmdUploadCheck As New SqlCommand

            'Create Instance of Generic Class Object
            Dim clsGeneric As New MaxPayroll.Generic

            'Variable Declarations
            Dim intCount As Int16
            Dim strSql As String

            Try

                strSql = "select count(0) from tPgt_FileDetails where OrgId = " & lngOrgId & _
                            " and FileType =" & clsDB.SQLStr(strFileType) & _
                            " and MD5Hash =" & clsDB.SQLStr(strMD5Hash)

                'Intialise SQL Connection
                Call clsGeneric.SQLConnection_Initialize()

                With cmdUploadCheck
                    .Connection = clsGeneric.SQLConnection
                    .CommandText = strSql
                    .CommandType = CommandType.Text
                    intCount = .ExecuteScalar()
                End With

                Return intCount

            Catch ex As Exception

                'Log Error  
                Call clsGeneric.ErrorLog(lngOrgId, lngUserId, "fncMD5Validation - clsMD5", Err.Number, Err.Description)
                Throw ex
            Finally

                'Terminate SQL Connection
                clsGeneric.SQLConnection_Terminate()

                'Destroy Instance of Generic Class Object
                clsGeneric = Nothing

            End Try

        End Function

#End Region

    End Class
End Namespace


