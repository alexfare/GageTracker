﻿Imports System.Data.OleDb

Public Class GageList
    Private Sub GageList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            GlobalVars.LoadDatabaseLocation()
            LoadData()
        Catch ex As OleDbException
            MessageBox.Show("Database error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("General error: " & ex.Message)
        End Try

        txtVersion.Text = GlobalVars.VersionString
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Public Sub LoadData()
        If Not System.IO.File.Exists(GlobalVars.DatabaseLocation) Then
            If PromptForDatabaseLocation() Then
                ' Try loading the data again with the new location
                LoadData()
                Return
            Else
                MessageBox.Show("No valid database selected. Application will exit.")
                Application.Exit()
            End If
        End If

        Dim connectionString As String
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        Dim query As String = "SELECT GageID, [Status], [PartNumber], [Description], Department, [Gage Type], [Customer], [Inspected Date], [Due Date] FROM CalibrationTracker"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Dim command As New OleDbCommand(query, connection)
                Dim adapter As New OleDbDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)
                DataGridView1.DataSource = table
            Catch ex As OleDbException
                MessageBox.Show("OleDb error: " & ex.Message)
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try
        End Using
    End Sub

    Private Function PromptForDatabaseLocation() As Boolean
        Dim result As DialogResult = MessageBox.Show("No database found. Would you like to select a new database location?", "Database Not Found", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Using openFileDialog As New OpenFileDialog()
                openFileDialog.InitialDirectory = "C:\"
                openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
                openFileDialog.FilterIndex = 1
                openFileDialog.RestoreDirectory = True

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    GlobalVars.DatabaseLocation = openFileDialog.FileName
                    GlobalVars.SaveDatabaseLocation(GlobalVars.DatabaseLocation)
                    Return True
                End If
            End Using
        End If
        Return False
    End Function

    ' Set Zebra striping
    Private Sub DataGridView1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint
        Dim grid As DataGridView = CType(sender, DataGridView)
        If e.RowIndex >= 0 Then
            If e.RowIndex Mod 2 = 0 Then
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGray
            Else
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
            End If
        End If
    End Sub

    Private Sub BtnMenu_Click(sender As Object, e As EventArgs) Handles BtnMenu.Click
        ' Show the GTMenu form regardless of whether GageID was set
        GTMenu.Show()
        GTMenu.LoadGageID()
    End Sub

    Private Sub BtnDueList_Click(sender As Object, e As EventArgs) Handles BtnDueList.Click
        DueDateCategorizer.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub AdminToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminToolStripMenuItem.Click
        If GlobalVars.UserActive = True Then
            Dim adminMenu As New AdminMenu()
            adminMenu.Show()
            Me.Hide()
            GlobalVars.AdminLoad = "1"

        Else
            Dim loginForm As New LoginForm1()
            loginForm.Show()
            Me.Hide()
            GlobalVars.AdminLoad = "1"
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub ChangeDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeDatabaseToolStripMenuItem.Click
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.InitialDirectory = "C:\"
            openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
            openFileDialog.FilterIndex = 1
            openFileDialog.RestoreDirectory = True

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                GlobalVars.DatabaseLocation = openFileDialog.FileName
                GlobalVars.SaveDatabaseLocation(GlobalVars.DatabaseLocation)
            End If
        End Using
    End Sub

    Private Sub MenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuToolStripMenuItem.Click
        ' Show the GTMenu form regardless of whether GageID was set
        GTMenu.Show()
        GTMenu.LoadGageID()
    End Sub
End Class
