Module GlobalVars
    Public DatabaseLocation As String

    Public Sub LoadDatabaseLocation()
        DatabaseLocation = My.Settings.DatabaseLocation
    End Sub

    Public Sub SaveDatabaseLocation(location As String)
        ' Save the database location to a configuration file or registry
        My.Settings.DatabaseLocation = location
        My.Settings.Save()
    End Sub
End Module
