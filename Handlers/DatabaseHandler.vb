Imports System.Data.OleDb

Public Class DatabaseHandler
    Public Shared Function GetConnection() As OleDbConnection
        Try
            Dim connectionString As String = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={GlobalVars.DatabaseLocation};"
            Return New OleDbConnection(connectionString)
        Catch ex As Exception
            Logger.LogSystem("Database connection error: " & ex.Message)
            Throw
        End Try
    End Function
End Class