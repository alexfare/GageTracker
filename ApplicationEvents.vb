Imports System.Data.OleDb
Imports System.IO
Imports System.Net

Namespace My
    Partial Friend Class MyApplication
        Private Async Sub MyApplication_Startup(sender As Object, e As EventArgs) Handles Me.Startup
            GlobalVars.DatabaseLocation = My.Settings.DatabaseLocation
            Await InitializeAppAsync()
        End Sub

        Private Async Function InitializeAppAsync() As Task
            If Not System.IO.File.Exists(GlobalVars.DatabaseLocation) Then
                If DatabaseCheck() Then
                    DatabaseVersionCheck()
                    BackupDatabase()
                    UpdateOpenCount()

                    Await Task.Run(Sub() SystemLog())
                    Await Task.Run(Sub() UpdateMySettings())
                    Await Task.Run(Sub() getAuth())
                Else
                    MessageBox.Show("No valid database selected. The application will exit.",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                    Logger.LogErrors("No valid database selected.")
                    Environment.Exit(0)
                End If
            End If
        End Function

        Private Function DatabaseCheck() As Boolean
            Dim result As DialogResult = MessageBox.Show("No database found. Would you like to select a new database location?",
                                                 "Database Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If result = DialogResult.Yes Then
                Using openFileDialog As New OpenFileDialog()
                    openFileDialog.InitialDirectory = "C:\"
                    openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
                    openFileDialog.FilterIndex = 1
                    openFileDialog.RestoreDirectory = True

                    If openFileDialog.ShowDialog() = DialogResult.OK Then
                        GlobalVars.DatabaseLocation = openFileDialog.FileName
                        GlobalVars.SaveDatabaseLocation(GlobalVars.DatabaseLocation)
                        Return True
                    End If
                End Using
            Else
                Dim downloadResult As DialogResult = MessageBox.Show("Would you like to download the database instead?",
                                                             "Download Option", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                If downloadResult = DialogResult.Yes Then
                    Dim saveDialog As New SaveFileDialog()
                    saveDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
                    saveDialog.Title = "Save Database File"
                    saveDialog.FileName = "GTDatabase.accdb"

                    If saveDialog.ShowDialog() = DialogResult.OK Then
                        DownloadDatabase(saveDialog.FileName)
                        Return True
                    End If
                Else
                    Environment.Exit(0)
                End If
            End If
            Return False
        End Function

        Private Sub DownloadDatabase(savePath As String)
            Dim webClient As New WebClient()

            Try
                Dim downloadUrl As String = "https://alexfare.com/programs/gtdatabase/latest/GTDatabase.accdb"
                webClient.DownloadFile(downloadUrl, savePath)

                GlobalVars.DatabaseLocation = savePath
                GlobalVars.SaveDatabaseLocation(savePath)
            Catch ex As Exception
                MessageBox.Show("An error occurred while downloading the database: " & ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Logger.LogErrors("An error occurred while downloading the database: " & ex.Message)
            Finally
                webClient.Dispose()
            End Try
        End Sub

        Sub DatabaseVersionCheck()
            Dim query As String = "SELECT [Number] FROM Settings WHERE SettingName = 'MinVersion'"

            Using connection As OleDbConnection = DatabaseHelper.GetConnection()
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
            Dim getQuery As String = "SELECT [Number] FROM Settings WHERE SettingName = 'OpenCount'"
            Dim updateQuery As String = "UPDATE Settings SET [Number] = @NewCount WHERE SettingName = 'OpenCount'"

            Using connection As OleDbConnection = DatabaseHelper.GetConnection()
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