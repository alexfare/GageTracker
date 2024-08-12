Imports System.Security.Cryptography
Imports System.IO
Imports System.Text

Public Class SecureHandler
    Public Shared Function EncryptString(input As String, key As Byte(), iv As Byte()) As String ' Method to encrypt a string
        Using aes As Aes = Aes.Create()
            aes.Key = key
            aes.IV = iv
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

    Public Shared Function DecryptString(input As String, key As Byte(), iv As Byte()) As String ' Method to decrypt a string
        Dim buffer As Byte() = Convert.FromBase64String(input)
        Using aes As Aes = Aes.Create()
            aes.Key = key
            aes.IV = iv
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
