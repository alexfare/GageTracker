Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

Public Class SecureHandler
    Private Const KeySize As Integer = 16
    Private Const BlockSize As Integer = 16
    Private Const part1 As String = "7s8x2V1r"
    Private Const part2 As String = "T9k4q3w0"
    Private Const ivPart1 As String = "Z3y6W8p7"
    Private Const ivPart2 As String = "G4t2Q1f9"
    Private Shared encryptedPassword As String = GlobalVars.AuthHash

    Public Shared Function EncryptString(input As String, key As Byte(), iv As Byte()) As String
        If String.IsNullOrEmpty(input) Then Throw New ArgumentException("Input string cannot be null or empty.")
        If key Is Nothing OrElse key.Length <> KeySize Then Throw New ArgumentException($"Key must be {KeySize} bytes.")
        If iv Is Nothing OrElse iv.Length <> BlockSize Then Throw New ArgumentException($"IV must be {BlockSize} bytes.")

        Using aes As Aes = Aes.Create()
            aes.Key = key
            aes.IV = iv
            aes.Mode = CipherMode.CBC
            aes.Padding = PaddingMode.PKCS7

            Dim encryptor As ICryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV)

            Using ms As New MemoryStream()
                Using cs As New CryptoStream(ms, encryptor, CryptoStreamMode.Write)
                    Using sw As New StreamWriter(cs)
                        sw.Write(input)
                    End Using
                End Using
                Return Convert.ToBase64String(ms.ToArray())
            End Using
        End Using
    End Function

    Public Shared Function DecryptString(input As String, key As Byte(), iv As Byte()) As String
        If String.IsNullOrEmpty(input) Then Throw New ArgumentException("Input string cannot be null or empty.")
        If key Is Nothing OrElse key.Length <> KeySize Then Throw New ArgumentException($"Key must be {KeySize} bytes.")
        If iv Is Nothing OrElse iv.Length <> BlockSize Then Throw New ArgumentException($"IV must be {BlockSize} bytes.")

        Dim buffer As Byte() = Convert.FromBase64String(input)

        Using aes As Aes = Aes.Create()
            aes.Key = key
            aes.IV = iv
            aes.Mode = CipherMode.CBC
            aes.Padding = PaddingMode.PKCS7

            Dim decryptor As ICryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV)

            Using ms As New MemoryStream(buffer)
                Using cs As New CryptoStream(ms, decryptor, CryptoStreamMode.Read)
                    Using sr As New StreamReader(cs)
                        Return sr.ReadToEnd()
                    End Using
                End Using
            End Using
        End Using
    End Function

    Public Shared Function GetDecryptedPassword() As String
        Try
            Dim fullKey As String = part1 & part2
            Dim fullIV As String = ivPart1 & ivPart2
            Return SecureHandler.DecryptString(encryptedPassword, Encoding.UTF8.GetBytes(fullKey), Encoding.UTF8.GetBytes(fullIV))
        Catch ex As Exception
            GlobalVars.ErrorLog = "An error occurred while decrypting the password."
            Logger.LogErrors()
            Throw New ApplicationException("An error occurred while decrypting the password.", ex)
        End Try
    End Function
End Class