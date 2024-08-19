Public NotInheritable Class LoadingScreen

    Private Sub LoadingScreen_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Application title
        If My.Application.Info.Title <> "" Then
            ApplicationTitle.Text = My.Application.Info.Title
        Else
            'If the application title is missing, use the application name, without the extension
            ApplicationTitle.Text = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Version.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)

        'Copyright info
        Copyright.Text = My.Application.Info.Copyright

        'Aduit
        GlobalVars.LastActivity = My.Settings.LastActivity
        My.Settings.isAdmin = False
        My.Settings.LoggedUser = ""
        My.Settings.Save()
    End Sub
End Class
