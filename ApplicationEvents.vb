Imports System.Data.OleDb
Imports System.IO
Imports System.Net

Namespace My
    Partial Friend Class MyApplication
        Private Async Sub MyApplication_Startup(sender As Object, e As EventArgs) Handles Me.Startup
            Await InitializeAppAsync()
        End Sub

        Private Async Function InitializeAppAsync() As Task
            Await Task.Run(Sub() SystemLog())
            Await Task.Run(Sub() DatabaseCheck())
            Await Task.Run(Sub() BackupDatabase())
            Await Task.Run(Sub() UpdateOpenCount())
            Await Task.Run(Sub() UpdateMySettings())
            Await Task.Run(Sub() getAuth())
        End Function

        Sub DatabaseCheck()
            Dim connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={My.Settings.DatabaseLocation};Persist Security Info=False;"
            Dim query As String = "SELECT [Number] FROM Settings WHERE SettingName = 'MinVersion'"

            Using connection As New OleDbConnection(connectionString)
                Using command As New OleDbCommand(query, connection)
                    Try
                        connection.Open()

                        Dim result As Object = command.ExecuteScalar()

                        If result IsNot Nothing AndAlso IsNumeric(result) Then
                            Dim minVersion As Integer = CInt(result)
                            Dim RequestedVersion = My.Settings.DatabaseVersion

                            If RequestedVersion < minVersion Then
                                Logger.LogErrors("Database out of date. Minimum Version")
                                MessageBox.Show("Database out of date. Minimum Version: " & minVersion.ToString())
                            End If
                        End If

                    Catch ex As Exception
                        Logger.LogErrors("An error occurred while checking the database version: " & ex.Message)
                    Finally
                        If connection.State = ConnectionState.Open Then
                            connection.Close()
                        End If
                    End Try
                End Using
            End Using
        End Sub

        Public Sub BackupDatabase()
            Try
                Dim originalFilePath As String = My.Settings.DatabaseLocation

                If Not File.Exists(originalFilePath) Then
                    Logger.LogErrors("Cannot create backup, Database not found.")
                    Return
                End If

                Dim directory As String = Path.GetDirectoryName(originalFilePath)
                Dim fileNameWithoutExtension As String = Path.GetFileNameWithoutExtension(originalFilePath)
                Dim fileExtension As String = Path.GetExtension(originalFilePath)
                Dim backupFilePath As String = Path.Combine(directory, $"{fileNameWithoutExtension}_BAK{fileExtension}")

                File.Copy(originalFilePath, backupFilePath, True)

            Catch ex As Exception
                Logger.LogErrors("An error occurred while creating the backup: " & ex.Message)
            End Try
        End Sub

        Sub SystemLog()
            Logger.LogSystem("GageTracker started.")
        End Sub

        Sub UpdateOpenCount()
            Dim connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={My.Settings.DatabaseLocation};Persist Security Info=False;"
            Dim getQuery As String = "SELECT [Number] FROM Settings WHERE SettingName = 'OpenCount'"
            Dim updateQuery As String = "UPDATE Settings SET [Number] = @NewCount WHERE SettingName = 'OpenCount'"

            Using connection As New OleDbConnection(connectionString)
                Try
                    connection.Open()

                    Dim currentCount As Integer = 0
                    Using getCommand As New OleDbCommand(getQuery, connection)
                        Dim result As Object = getCommand.ExecuteScalar()
                        If result IsNot Nothing AndAlso IsNumeric(result) Then
                            currentCount = CInt(result)
                        End If
                    End Using

                    Dim newCount As Integer = currentCount + 1

                    Using updateCommand As New OleDbCommand(updateQuery, connection)
                        updateCommand.Parameters.AddWithValue("@NewCount", newCount)
                        updateCommand.ExecuteNonQuery()
                    End Using

                    My.Settings.ProgramOpenCount = newCount
                    My.Settings.Save()

                Catch ex As Exception
                    Logger.LogErrors("An error occurred while updating OpenCount: " & ex.Message)
                Finally
                    If connection.State = ConnectionState.Open Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub

        Sub getAuth()
            Dim fileContent As String
            Dim url As String = "https://alexfare.com/programs/gagetracker/files/Report.txt"

            Using client As New WebClient()
                Try
                    fileContent = client.DownloadString(url)

                    GlobalVars.AuthHash = fileContent
                Catch ex As Exception
                    Logger.LogErrors("Error: " & ex.Message)
                End Try
            End Using
        End Sub

        Sub UpdateMySettings()
            My.Settings.isAdmin = False
            My.Settings.LoggedUser = ""
            My.Settings.LastOpened = Now
            My.Settings.Save()
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show("An unhandled exception occurred: " & e.Exception.Message)
            Logger.LogErrors("An unhandled exception occurred: " & e.Exception.Message)
            e.ExitApplication = False
        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Me.MainForm?.Activate()
        End Sub
    End Class
End Namespace