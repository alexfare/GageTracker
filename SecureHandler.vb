﻿Imports System.IO
Imports System.Security.Cryptography

Public Class SecureHandler
    Private Const KeySize As Integer = 16
    Private Const BlockSize As Integer = 16

    Public Shared Function EncryptString(input As String, key As Byte(), iv As Byte()) As String
        ' Validate parameters
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
        ' Validate parameters
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
End Class