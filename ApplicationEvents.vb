Namespace My
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As EventArgs) Handles Me.Startup
            Dim CurrentOpenCount As Integer
            Dim NewOpenCount As Integer

            CurrentOpenCount = My.Settings.ProgramOpenCount
            NewOpenCount = CurrentOpenCount + 1
            My.Settings.ProgramOpenCount = NewOpenCount

            My.Settings.isAdmin = False
            My.Settings.LoggedUser = ""
            My.Settings.LastOpened = Now
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
