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
        Dim query As String = "SELECT COUNT(*) FROM CalibrationTracker WHERE Status = 'Inactive'"

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
        ModernTheme.Apply(Me)
        Me.FormBorderStyle = FormBorderStyle.None

        Panel1.BackColor = Color.Transparent

        Dim metricPanels = New Panel() {Panel2, Panel3, Panel4, Panel5, Panel6, Panel7}
        For Each pnl In metricPanels
            RoundControl(pnl, 15)
            pnl.BackColor = ModernTheme.SurfaceColor
            pnl.BorderStyle = BorderStyle.None
            pnl.ForeColor = Color.WhiteSmoke
        Next

        Dim metricText = New TextBox() {TxtActive, TxtInactive, TxtLost, TxtOverdue, Txt30, Txt60}
        For Each txt In metricText
            txt.BackColor = ModernTheme.SurfaceColor
            txt.ForeColor = ModernTheme.AccentColor
        Next

        Dim buttons = {BtnOverdue, Btn30, Btn60}
        For Each btn In buttons
            RoundControl(btn, 10)
            btn.FlatStyle = FlatStyle.Flat
            btn.FlatAppearance.BorderSize = 0
            btn.BackColor = ModernTheme.AccentColor
            btn.ForeColor = Color.White
            btn.FlatAppearance.MouseOverBackColor = ModernTheme.AdjustColor(btn.BackColor, 0.2)
            btn.FlatAppearance.MouseDownBackColor = ModernTheme.AdjustColor(btn.BackColor, -0.15)
        Next

        RoundControl(BtnClose, 10)
        BtnClose.FlatStyle = FlatStyle.Flat
        BtnClose.FlatAppearance.BorderSize = 0
        BtnClose.BackColor = ModernTheme.DangerColor
        BtnClose.ForeColor = Color.White
        BtnClose.FlatAppearance.MouseOverBackColor = ModernTheme.AdjustColor(BtnClose.BackColor, 0.2)
        BtnClose.FlatAppearance.MouseDownBackColor = ModernTheme.AdjustColor(BtnClose.BackColor, -0.15)
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
