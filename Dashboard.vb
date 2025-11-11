Imports System.Data.OleDb
Imports System.Drawing.Drawing2D

Public Class Dashboard

    Private Sub Dashboard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UI_Setup()

        ' Load data
        GetActiveEntriesCount()
        GetInactiveEntriesCount()
        GetLostEntriesCount()
        GetPastDueCount()
        GetDueWithin30DaysCount()
        GetDueWithin60DaysCount()
    End Sub

#Region "Database Commands"
    Public Function GetActiveEntriesCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE Status = 'Active'"

        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtActive.Text = totalCount
                End Using
            Catch ex As Exception
                Logger.LogErrors("Error: " & ex.Message)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return totalCount
    End Function

    Public Function GetInactiveEntriesCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "
                                SELECT COUNT(*)
                                FROM CalibrationTracker
                                WHERE Status NOT IN ('Active', 'Lost')
                             "


        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtInactive.Text = totalCount
                End Using
            Catch ex As Exception
                Logger.LogErrors("Error: " & ex.Message)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return totalCount
    End Function

    Public Function GetLostEntriesCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE Status = 'Lost'"

        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtLost.Text = totalCount
                End Using
            Catch ex As Exception
                Logger.LogErrors("Error: " & ex.Message)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return totalCount
    End Function

    Public Function GetPastDueCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE [Due Date] < ?"

        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Today", DateTime.Now.Date)
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    TxtOverdue.Text = totalCount

                    TxtOverdue.BackColor = If(totalCount = 0, SystemColors.Control, Color.Red)
                End Using
            Catch ex As Exception
                Logger.LogErrors("Error: " & ex.Message)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return totalCount
    End Function

    Public Function GetDueWithin30DaysCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE [Due Date] >= ? AND [Due Date] <= ?"

        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Today", DateTime.Now.Date)
                    command.Parameters.AddWithValue("@ThirtyDays", DateTime.Now.Date.AddDays(30))
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    Txt30.Text = totalCount

                    Txt30.BackColor = If(totalCount = 0, SystemColors.Control, Color.Yellow)
                End Using
            Catch ex As Exception
                Logger.LogErrors("Error: " & ex.Message)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return totalCount
    End Function

    Public Function GetDueWithin60DaysCount() As Integer
        Dim totalCount As Integer = 0
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE [Due Date] >= ? AND [Due Date] <= ?"

        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Using command As New OleDbCommand(query, connection)
                    command.Parameters.AddWithValue("@Today", DateTime.Now.Date)
                    command.Parameters.AddWithValue("@SixtyDays", DateTime.Now.Date.AddDays(60))
                    totalCount = Convert.ToInt32(command.ExecuteScalar())
                    Txt60.Text = totalCount

                    Txt60.BackColor = If(totalCount = 0, SystemColors.Control, Color.LightBlue)
                End Using
            Catch ex As Exception
                Logger.LogErrors("Error: " & ex.Message)
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Return totalCount
    End Function
#End Region

#Region "Buttons"
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

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
#End Region

#Region "UI Enhancements"
    Private Sub UI_Setup()
        Me.BackColor = Color.FromArgb(245, 245, 250)
        Me.Font = New Font("Segoe UI", 10)
        Me.FormBorderStyle = FormBorderStyle.None

        For Each pnl As Panel In Panel1.Controls.OfType(Of Panel)()
            RoundControl(pnl, 15)
            pnl.BackColor = Color.White
            pnl.BorderStyle = BorderStyle.None
        Next

        Dim buttons = {BtnOverdue, Btn30, Btn60, BtnClose}
        For Each btn In buttons
            RoundControl(btn, 10)
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0
            btn.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            btn.ForeColor = Color.White

            If btn Is BtnClose Then
                btn.BackColor = Color.FromArgb(244, 67, 54)
            Else
                btn.BackColor = Color.FromArgb(76, 175, 80)
            End If

            AddHandler btn.MouseEnter, AddressOf Button_HoverEnter
            AddHandler btn.MouseLeave, AddressOf Button_HoverLeave
        Next
    End Sub

    Private Sub Button_HoverEnter(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, Button)
        If btn Is BtnClose Then
            btn.BackColor = Color.FromArgb(211, 47, 47)
        Else
            btn.BackColor = Color.FromArgb(56, 142, 60)
        End If
    End Sub

    Private Sub Button_HoverLeave(sender As Object, e As EventArgs)
        Dim btn = DirectCast(sender, Button)
        If btn Is BtnClose Then
            btn.BackColor = Color.FromArgb(244, 67, 54)
        Else
            btn.BackColor = Color.FromArgb(76, 175, 80)
        End If
    End Sub

    Private Sub RoundControl(ctrl As Control, radius As Integer)
        Dim path As New GraphicsPath()
        path.StartFigure()
        path.AddArc(New Rectangle(0, 0, radius, radius), 180, 90)
        path.AddArc(New Rectangle(ctrl.Width - radius, 0, radius, radius), 270, 90)
        path.AddArc(New Rectangle(ctrl.Width - radius, ctrl.Height - radius, radius, radius), 0, 90)
        path.AddArc(New Rectangle(0, ctrl.Height - radius, radius, radius), 90, 90)
        path.CloseFigure()
        ctrl.Region = New Region(path)
    End Sub
#End Region

End Class
