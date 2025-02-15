﻿Imports System.Data.OleDb

Public Class GageType
    Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    Dim insertQuery As String = "INSERT INTO GageType (GageType) VALUES (@GageType)"

    Private Sub GageType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        LoadGageType()
    End Sub

    Private Sub LoadGageType()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageType FROM GageType", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtGageType.Items.Clear()
                While reader.Read()
                    txtGageType.Items.Add(reader("GageType").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage type: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading Gage type: " & ex.Message
                Logger.LogErrors()
            End Try
        End Using
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(txtGageType.Text) Then
            ShowStatus("Gage type name cannot be blank.", False)
            Return
        End If

        Using checkConn As New OleDbConnection(connectionString)
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM GageType WHERE GageType = @Name", checkConn)
            checkCmd.Parameters.AddWithValue("@Name", txtGageType.Text)

            Try
                checkConn.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    ShowStatus("This Gage type already exists. Please enter a unique Gage type.", False)
                    Return
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while checking for duplicate names: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while checking for duplicate names: " & ex.Message
                Logger.LogErrors()
                Return
            End Try
        End Using

        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@GageType", txtGageType.Text)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    ShowStatus("Gage type added successfully.", False)
                    GlobalVars.SystemLog = txtGageType.Text + " Gage type added successfully."
                    Logger.LogSystem()
                    txtGageType.Items.Add(txtGageType.Text)
                    txtGageType.Text = String.Empty
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new Gage type: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while adding new Gage type: " & ex.Message
                    Logger.LogErrors()
                End Try
            End Using
        End Using
    End Sub
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim gageTypeDelete As String

        If txtGageType.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtGageType.Text) Then
            ShowStatus("Please select a Gage type to remove.", True)
            Return
        End If

        gageTypeDelete = txtGageType.Text

        If MessageBox.Show("Are you sure you want to delete " + gageTypeDelete + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Dim selectedGageType As String = txtGageType.SelectedItem.ToString()
        Dim query As String = "DELETE FROM GageType WHERE GageType = @Name"

        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Name", selectedGageType)

                Try
                    connection.Open()
                    Dim result As Integer = command.ExecuteNonQuery()
                    If result > 0 Then
                        ShowStatus("Gage type deleted successfully.", False)
                        GlobalVars.SystemLog = selectedGageType + " Gage type deleted successfully."
                        Logger.LogSystem()
                        txtGageType.Items.Remove(selectedGageType)
                        txtGageType.Text = String.Empty
                        txtGageType.SelectedIndex = -1
                    Else
                        MessageBox.Show("No records were deleted.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while deleting the Gage type: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while deleting the Gage type: " & ex.Message
                    Logger.LogErrors()
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    Private Sub ShowStatus(message As String, isError As Boolean)
        StatusLabel.ForeColor = If(isError, Color.Red, Color.Green)
        StatusLabel.Text = message
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""
        Timer1.Enabled = False
    End Sub
End Class