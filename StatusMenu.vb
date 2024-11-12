Imports System.Data.OleDb
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class StatusMenu
    Dim connectionString As String
    Dim insertQuery As String = "INSERT INTO Status (Status) VALUES (@Status)"

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
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
        'Check for blank Status name
        If String.IsNullOrWhiteSpace(txtStatus.Text) Then
            StatusLabel.Text = "Status Name cannot be blank."
            Timer1.Enabled = True
            Return
        End If

        'Check for duplicate status name
        Using checkConn As New OleDbConnection(connectionString)
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Status WHERE Status = @Name", checkConn)
            checkCmd.Parameters.AddWithValue("@Name", txtStatus.Text)

            Try
                checkConn.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    StatusLabel.Text = "This Status already exists. Please enter a unique Status."
                    Timer1.Enabled = True
                    Return
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while checking for duplicate names: " & ex.Message)
                Return
            End Try
        End Using

        'Proceed with adding new status
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@Name", txtStatus.Text)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    StatusLabel.Text = "Status added successfully."
                    GlobalVars.SystemLog = txtStatus.Text + " status added successfully."
                    Logger.LogSystem()
                    Timer1.Enabled = True
                    LoadStatus() 'Reload status list to include new data
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new status: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while adding new status: " & ex.Message
                    Logger.LogErrors()
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If txtStatus.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtStatus.Text) Then
            StatusLabel.Text = "Please select a Status to remove."
            Timer1.Enabled = True
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete this Status?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
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
                        StatusLabel.Text = "Status deleted successfully."
                        GlobalVars.SystemLog = txtStatus.Text + " status deleted successfully."
                        Logger.LogSystem()
                        Timer1.Enabled = True
                        txtStatus.SelectedIndex = -1 'Reset the ComboBox selection
                        txtStatus.Text = ""
                        LoadStatus() 'Reload status list to reflect the changes
                    Else
                        StatusLabel.Text = "No records were deleted."
                        Timer1.Enabled = True
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
End Class