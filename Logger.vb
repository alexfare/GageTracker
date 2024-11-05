Imports System.IO

Module Logger
    Dim logFolderPath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GageTracker", "Logs")

    Public Sub SaveLogEntry()
        Try
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

    Public Sub LogSystem()
        Try
            Dim logFilePath As String = Path.Combine(logFolderPath, "System.txt")

            If Not Directory.Exists(logFolderPath) Then
                Directory.CreateDirectory(logFolderPath)
            End If

            Dim logEntry As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " - " & GlobalVars.SystemLog

            File.AppendAllText(logFilePath, logEntry & Environment.NewLine)

        Catch ex As Exception
            MessageBox.Show("Error writing to the log file: " & ex.Message)
        End Try
    End Sub

    Public Sub LogErrors()
        Try
            Dim logFilePath As String = Path.Combine(logFolderPath, "Errors.txt")

            If Not Directory.Exists(logFolderPath) Then
                Directory.CreateDirectory(logFolderPath)
            End If

            Dim logEntry As String = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") & " - " & GlobalVars.ErrorLog

            File.AppendAllText(logFilePath, logEntry & Environment.NewLine)

        Catch ex As Exception
            MessageBox.Show("Error writing to the log file: " & ex.Message)
        End Try
    End Sub
End Module
