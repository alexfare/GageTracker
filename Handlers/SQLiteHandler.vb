' SQLiteHandler.vb
' Handles SQLite DB creation and connection for GageTracker

Imports System.Data.SQLite
Imports System.IO

Public Module SQLiteHandler
    Private dbFileName As String = "GTDatabase.sqlite"
    Public ReadOnly Property DatabasePath As String
        Get
            Return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, dbFileName)
        End Get
    End Property

    Public Sub EnsureDatabase()
        Dim dbExists As Boolean = File.Exists(DatabasePath)
        Using conn As New SQLiteConnection($"Data Source={DatabasePath};Version=3;")
            conn.Open()
            If Not dbExists Then
                CreateTables(conn)
            Else
                UpdateSchema(conn)
            End If
        End Using
    End Sub

    Private Sub CreateTables(conn As SQLiteConnection)
        Dim createCalibrationTracker As String = """
        CREATE TABLE IF NOT EXISTS CalibrationTracker (
            GageID TEXT PRIMARY KEY,
            Status TEXT,
            PartNumber TEXT,
            Description TEXT,
            Department TEXT,
            [Gage Type] TEXT,
            Customer TEXT,
            [Inspected Date] TEXT,
            [Due Date] TEXT
        );
        """
        Using cmd As New SQLiteCommand(createCalibrationTracker, conn)
            cmd.ExecuteNonQuery()
        End Using
        ' Add more CREATE TABLE statements here as needed
    End Sub

    Private Sub UpdateSchema(conn As SQLiteConnection)
        ' Example: Add new columns if missing (expand as needed)
        Dim columnsToAdd As New Dictionary(Of String, String) From {
            {"CalibrationTracker", "ALTER TABLE CalibrationTracker ADD COLUMN [NewColumn] TEXT"}
        }
        For Each kvp In columnsToAdd
            Try
                Using cmd As New SQLiteCommand(kvp.Value, conn)
                    cmd.ExecuteNonQuery()
                End Using
            Catch ex As SQLiteException
                ' Ignore if column already exists
            End Try
        Next
    End Sub

    Public Function GetConnection() As SQLiteConnection
        Return New SQLiteConnection($"Data Source={DatabasePath};Version=3;")
    End Function
End Module
