Imports System.Data.OleDb

Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As EventArgs) Handles Me.Startup
            DatabaseCheck()
            SystemLog()
            UpdateOpenCount()
            UpdateMySettings()
        End Sub

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
                                GlobalVars.ErrorLog = "Database out of date. Minimum Version"
                                Logger.LogErrors()
                                MessageBox.Show("Database out of date. Minimum Version: " & minVersion.ToString())
                            End If
                        End If

                    Catch ex As Exception
                        MessageBox.Show("An error occurred while checking the database version: " & ex.Message)
                        GlobalVars.ErrorLog = "An error occurred while checking the database version: " & ex.Message
                        Logger.LogErrors()
                    Finally
                        If connection.State = ConnectionState.Open Then
                            connection.Close()
                        End If
                    End Try
                End Using
            End Using
        End Sub

        Sub SystemLog()
            GlobalVars.SystemLog = "GageTracker started."
            Logger.LogSystem()
        End Sub

        Sub UpdateOpenCount()
            Dim connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={My.Settings.DatabaseLocation};Persist Security Info=False;"
            Dim getQuery As String = "SELECT [Number] FROM Settings WHERE SettingName = 'OpenCount'"
            Dim updateQuery As String = "UPDATE Settings SET [Number] = @NewCount WHERE SettingName = 'OpenCount'"

            Using connection As New OleDbConnection(connectionString)
                Try
                    connection.Open()

                    'Step 1: Retrieve the current OpenCount value
                    Dim currentCount As Integer = 0
                    Using getCommand As New OleDbCommand(getQuery, connection)
                        Dim result As Object = getCommand.ExecuteScalar()
                        If result IsNot Nothing AndAlso IsNumeric(result) Then
                            currentCount = CInt(result)
                        End If
                    End Using

                    'Step 2: Increment the OpenCount value
                    Dim newCount As Integer = currentCount + 1

                    'Step 3: Update the OpenCount value in the database
                    Using updateCommand As New OleDbCommand(updateQuery, connection)
                        updateCommand.Parameters.AddWithValue("@NewCount", newCount)
                        updateCommand.ExecuteNonQuery()
                    End Using

                    'Step 4: Update the application setting if needed
                    My.Settings.ProgramOpenCount = newCount
                    My.Settings.Save()

                Catch ex As Exception
                    MessageBox.Show("An error occurred while updating OpenCount: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while updating OpenCount: " & ex.Message
                    Logger.LogErrors()
                Finally
                    If connection.State = ConnectionState.Open Then
                        connection.Close()
                    End If
                End Try
            End Using
        End Sub

        Sub UpdateMySettings()
            My.Settings.isAdmin = False
            My.Settings.LoggedUser = ""
            My.Settings.LastOpened = Now
            My.Settings.SelectedGage = ""
            My.Settings.Save()
        End Sub

        Private Sub MyApplication_UnhandledException(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            MessageBox.Show("An unhandled exception occurred: " & e.Exception.Message)
            GlobalVars.ErrorLog = "An unhandled exception occurred: " & e.Exception.Message
            Logger.LogErrors()
            e.ExitApplication = False
        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            If Me.MainForm IsNot Nothing Then
                Me.MainForm.Activate()
            End If
        End Sub
    End Class
End Namespace
