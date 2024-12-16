Imports System.Data.OleDb

Public Class StatusMenu
    Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    Dim insertQuery As String = "INSERT INTO Status (Status) VALUES (@Status)"

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        LoadStatus()
    End Sub

    Private Sub LoadStatus()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Status FROM Status", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtStatus.Items.Clear()
                While reader.Read()
                    txtStatus.Items.Add(reader("Status").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Status: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading Status: " & ex.Message
                Logger.LogErrors()
            End Try
        End Using
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(txtStatus.Text) Then
            ShowStatus("Status name cannot be blank.", False)
            Timer1.Enabled = True
            Return
        End If

        Using checkConn As New OleDbConnection(connectionString)
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Status WHERE Status = @Name", checkConn)
            checkCmd.Parameters.AddWithValue("@Name", txtStatus.Text)

            Try
                checkConn.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    ShowStatus("This Status already exists. Please enter a unique Status.", True)
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
                command.Parameters.AddWithValue("@Name", txtStatus.Text)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    ShowStatus("Status added successfully.", False)
                    GlobalVars.SystemLog = txtStatus.Text + " status added successfully."
                    Logger.LogSystem()
                    LoadStatus()
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new status: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while adding new status: " & ex.Message
                    Logger.LogErrors()
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim statusDelete As String

        If txtStatus.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtStatus.Text) Then
            ShowStatus("Please select a Status to remove.", True)
            Return
        End If

        statusDelete = txtStatus.Text

        If MessageBox.Show("Are you sure you want to delete " + statusDelete + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Dim selectedStatus As String = txtStatus.SelectedItem.ToString()

        Dim query As String = "DELETE FROM Status WHERE Status = @Name"
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Name", selectedStatus)

                Try
                    connection.Open()
                    Dim result As Integer = command.ExecuteNonQuery()
                    If result > 0 Then
                        ShowStatus("Status deleted successfully.", False)
                        GlobalVars.SystemLog = txtStatus.Text + " status deleted successfully."
                        Logger.LogSystem()
                        txtStatus.SelectedIndex = -1
                        txtStatus.Text = ""
                        LoadStatus()
                    Else
                        ShowStatus("No records were deleted.", False)
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while deleting the Status: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while deleting the Status: " & ex.Message
                    Logger.LogErrors()
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""
        Timer1.Enabled = False
        LoadStatus()
    End Sub

    Private Sub ShowStatus(message As String, isError As Boolean)
        StatusLabel.ForeColor = If(isError, Color.Red, Color.Green)
        StatusLabel.Text = message
        Timer1.Enabled = True
    End Sub
End Class