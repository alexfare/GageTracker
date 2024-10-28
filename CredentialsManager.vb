Imports System.Text

Public Class CredentialsManager
    Private Shared ReadOnly part1 As String = "7s8x2V1r"
    Private Shared ReadOnly part2 As String = "T9k4q3w0"
    Private Shared ReadOnly ivPart1 As String = "Z3y6W8p7"
    Private Shared ReadOnly ivPart2 As String = "G4t2Q1f9"
    Private Shared ReadOnly ZW5jcnlwdGVkUGFzc3dvcmQ As String = "uP5NhLHKDoBmx/mVChJ87ltCLn0s1kF8LF8S+lJuCGw="

    Public Shared Function R2V0RGVjcnlwdGVkUGFzc3dvcmQ() As String
        Dim fullKey As String = part1 & part2
        Dim fullIV As String = ivPart1 & ivPart2
        Return SecureHandler.DecryptString(ZW5jcnlwdGVkUGFzc3dvcmQ, Encoding.UTF8.GetBytes(fullKey), Encoding.UTF8.GetBytes(fullIV))
    End Function
End Class