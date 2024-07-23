Module GlobalVars
    Public UserActive As Boolean
    Public GageIDString As String
    Public VersionString As String = "5.5.1.31"
    Public DatabaseLocation As String
    Public LoggedInUser As String = ""

    Public Sub LoadDatabaseLocation()
        DatabaseLocation = My.Settings.DatabaseLocation
    End Sub

    Public Sub SaveDatabaseLocation(location As String)
        ' Save the database location to a configuration file or registry
        My.Settings.DatabaseLocation = location
        My.Settings.Save()
    End Sub
End Module
