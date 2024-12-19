Imports System.Data.OleDb

Public Class Dashboard
    Private connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        GetActiveEntriesCount()
        GetInactiveEntriesCount()
        GetLostEntriesCount()
        GetPastDueCount()
        GetDueWithin30DaysCount()
        GetDueWithin60DaysCount()
    End Sub

    Public Function GetActiveEntriesCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE Status = 'Active'"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtActive.Text = totalCount
                End Using
            Catch ex As Exception
                GlobalVars.ErrorLog = ("Error: " & ex.Message)
                Logger.LogErrors()
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Return totalCount
    End Function

    Public Function GetInactiveEntriesCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE Status = 'Inactive'"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtInactive.Text = totalCount
                End Using
            Catch ex As Exception
                GlobalVars.ErrorLog = ("Error: " & ex.Message)
                Logger.LogErrors()
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Return totalCount
    End Function

    Public Function GetLostEntriesCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE Status = 'Lost'"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtLost.Text = totalCount
                End Using
            Catch ex As Exception
                GlobalVars.ErrorLog = ("Error: " & ex.Message)
                Logger.LogErrors()
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Return totalCount
    End Function

    Public Function GetPastDueCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE [Due Date] < ?"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Today", DateTime.Now.Date)

                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtOverdue.Text = totalCount
                End Using
            Catch ex As Exception
                GlobalVars.ErrorLog = ("Error: " & ex.Message)
                Logger.LogErrors()
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Return totalCount
    End Function

    Public Function GetDueWithin30DaysCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE [Due Date] >= ? AND [Due Date] <= ?"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Today", DateTime.Now.Date)
                    command.Parameters.AddWithValue("@ThirtyDaysFromNow", DateTime.Now.Date.AddDays(30))

                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    Txt30.Text = totalCount
                End Using
            Catch ex As Exception
                GlobalVars.ErrorLog = ("Error: " & ex.Message)
                Logger.LogErrors()
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Return totalCount
    End Function

    Public Function GetDueWithin60DaysCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE [Due Date] >= ? AND [Due Date] <= ?"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Today", DateTime.Now.Date)
                    command.Parameters.AddWithValue("@ThirtyDaysFromNow", DateTime.Now.Date.AddDays(60))

                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    Txt60.Text = totalCount
                End Using
            Catch ex As Exception
                GlobalVars.ErrorLog = ("Error: " & ex.Message)
                Logger.LogErrors()
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Return totalCount
    End Function

    Private Sub BtnOverdue_Click(sender As Object, e As EventArgs) Handles BtnOverdue.Click
        GlobalVars.DueDateMenuSelect = "Past"
        DueDateCategorizer.Show()
        Me.Close()
    End Sub

    Private Sub Btn30_Click(sender As Object, e As EventArgs) Handles Btn30.Click
        GlobalVars.DueDateMenuSelect = "30"
        DueDateCategorizer.Show()
        Me.Close()
    End Sub

    Private Sub Btn60_Click(sender As Object, e As EventArgs) Handles Btn60.Click
        GlobalVars.DueDateMenuSelect = "60"
        DueDateCategorizer.Show()
        Me.Close()
    End Sub
End Class