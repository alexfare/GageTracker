Imports System.IO

Module Logger
    Public Sub SaveLogEntry()
        Try
            Dim logFolderPath As String = "Logs"
            Dim logFilePath As String = Path.Combine(logFolderPath, "AuditLog.txt")

            If Not Directory.Exists(logFolderPath) Then
                Directory.CreateDirectory(logFolderPath)
            End If

            Dim logEntry As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " - " & GlobalVars.LastActivity

            File.AppendAllText(logFilePath, logEntry & Environment.NewLine)

        Catch ex As Exception
            MessageBox.Show("Error writing to the log file: " & ex.Message)
        End Try
    End Sub
End Module
