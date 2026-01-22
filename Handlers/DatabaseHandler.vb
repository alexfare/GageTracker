Imports System.Data.OleDb

Public Class DatabaseHandler
    Public Shared Function GetConnection() As OleDbConnection
        Try
            Dim connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={GlobalVars.DatabaseLocation};"
            Logger.LogSystem($"Creating DB connection. DataSource={GlobalVars.DatabaseLocation}; FileExists={System.IO.File.Exists(GlobalVars.DatabaseLocation)}")
            Return New OleDbConnection(connectionString)
        Catch ex As Exception
            Logger.LogSystem("Database connection error: " & ex.ToString())
            Throw
        End Try
    End Function
End Class