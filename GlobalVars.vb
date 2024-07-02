Module GlobalVars
    Public UserActive As Boolean
    Public GageIDString As String
    Public VersionString As String = "5.2.1.24"
    Public DatabaseLocation As String
    Public LoggedInUser As String = ""
    Public AdminLoad As String = ""

    Public Sub LoadDatabaseLocation()
        ' Try to read the database location from a configuration file or registry
        ' For example, using a configuration file:
        DatabaseLocation = My.Settings.DatabaseLocation
        If String.IsNullOrEmpty(DatabaseLocation) Then
            DatabaseLocation = "R:\Quality\GageCalibration\GTDatabase.accdb"
        End If
    End Sub

    Public Sub SaveDatabaseLocation(location As String)
        ' Save the database location to a configuration file or registry
        My.Settings.DatabaseLocation = location
        My.Settings.Save()
    End Sub
End Module
