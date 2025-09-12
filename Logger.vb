Imports System.IO

Module Logger
    Dim logFolderPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GageTracker", "Logs")

    Private Sub WriteLog(fileName As String, message As String)
        Try
            If Not Directory.Exists(logFolderPath) Then
                Directory.CreateDirectory(logFolderPath)
            End If

            Dim logFilePath As String = Path.Combine(logFolderPath, fileName)
            Dim logEntry As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " - " & message

            File.AppendAllText(logFilePath, logEntry & Environment.NewLine)

        Catch ex As Exception
            Debug.WriteLine("Error writing to the log file: " & ex.Message)
        End Try
    End Sub

    Public Sub SaveLogEntry()
        WriteLog("AuditLog.txt", GlobalVars.LastActivity)
    End Sub

    Public Sub LogSystem(message As String)
        WriteLog("System.txt", message)
    End Sub

    Public Sub LogErrors(message As String)
        WriteLog("Errors.txt", message)
    End Sub
End Module
