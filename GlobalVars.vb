Module GlobalVars
    Public DatabaseLocation As String
    Public LastActivity As String
    Public DueDateMenuSelect As String
    Public SystemLog As String
    Public ErrorLog As String
    Public Username As String

    Public Sub LoadDatabaseLocation()
        DatabaseLocation = My.Settings.DatabaseLocation
    End Sub

    Public Sub SaveDatabaseLocation(location As String)
        My.Settings.DatabaseLocation = location
        My.Settings.Save()
    End Sub
End Module
