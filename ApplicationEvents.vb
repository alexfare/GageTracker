Imports System.Data.OleDb

Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As EventArgs) Handles Me.Startup
            GetOpenCount()
            PlusOne()
            SystemLog()
            UpdateMySettings()
        End Sub

        Sub GetOpenCount()
            Dim connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={My.Settings.DatabaseLocation};Persist Security Info=False;"
            Dim query As String = "SELECT Number FROM Settings WHERE SettingName = 'OpenCount'"

            Using connection As New OleDbConnection(connectionString)
                Using command As New OleDbCommand(query, connection)
                    Try
                        connection.Open()

                        Dim result As Object = command.ExecuteScalar()

                        If result IsNot Nothing AndAlso IsNumeric(result) Then
                            My.Settings.ProgramOpenCount = CInt(result)
                            My.Settings.Save()
                        End If

                    Catch ex As Exception
                        MessageBox.Show("An error occurred: " & ex.Message)
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

        Sub PlusOne()
            Dim AddOne As Integer

            AddOne = My.Settings.ProgramOpenCount
            My.Settings.ProgramOpenCount = AddOne + 1
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
            e.ExitApplication = False
        End Sub

        Private Sub MyApplication_StartupNextInstance(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            If Me.MainForm IsNot Nothing Then
                Me.MainForm.Activate()
            End If
        End Sub
    End Class
End Namespace
