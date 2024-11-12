
Imports System.Text


Public Class CredentialsManager
    Private Const part1 As String = "7s8x2V1r"
    Private Const part2 As String = "T9k4q3w0"
    Private Const ivPart1 As String = "Z3y6W8p7"
    Private Const ivPart2 As String = "G4t2Q1f9"
    Private Const encryptedPassword As String = "uP5NhLHKDoBmx/mVChJ87ltCLn0s1kF8LF8S+lJuCGw="

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