Imports System.Data.SQLite
Imports System.IO

Public Class DatabaseHandler
    Public Shared Function GetConnection() As SQLiteConnection
        Try
            SQLiteHandler.EnsureDatabase()
            Return SQLiteHandler.GetConnection()
        Catch ex As Exception
            Logger.LogSystem("Database connection error: " & ex.Message)
            Throw
        End Try
    End Function
End Class