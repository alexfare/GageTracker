Imports System.Data.OleDb

Public Class StatusMenu
    Dim connectionString As String
    Dim insertQuery As String = "INSERT INTO Status (Status) VALUES (@Status)"

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        LoadStatus()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
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
            End Try
        End Using
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        ' Check for blank Status name
        If String.IsNullOrWhiteSpace(txtStatus.Text) Then
            MessageBox.Show("Status Name cannot be blank.")
            Return
        End If

        ' Check for duplicate status name
        Using checkConn As New OleDbConnection(connectionString)
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Status WHERE Status = @Name", checkConn)
            checkCmd.Parameters.AddWithValue("@Name", txtStatus.Text)

            Try
                checkConn.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    MessageBox.Show("This Status Name already exists. Please enter a unique name.")
                    Return
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while checking for duplicate names: " & ex.Message)
                Return
            End Try
        End Using

        ' Proceed with adding new status
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@Name", txtStatus.Text)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    MessageBox.Show("Data saved successfully.")
                    LoadStatus() ' Reload status list to include new data
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new status: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        ' Ensure that a status is selected before attempting deletion
        If txtStatus.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtStatus.Text) Then
            MessageBox.Show("Please select a Status to remove.")
            Return
        End If

        ' Confirm deletion
        If MessageBox.Show("Are you sure you want to delete this Status?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Dim selectedStatus As String = txtStatus.SelectedItem.ToString()

        ' Query to delete the selected status
        Dim query As String = "DELETE FROM Status WHERE Status = @Name"
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Name", selectedStatus)

                Try
                    connection.Open()
                    Dim result As Integer = command.ExecuteNonQuery()
                    If result > 0 Then
                        MessageBox.Show("Status deleted successfully.")
                        txtStatus.SelectedIndex = -1 ' Reset the ComboBox selection
                        LoadStatus() ' Reload status list to reflect the changes
                    Else
                        MessageBox.Show("No records were deleted.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while deleting the Status: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub
End Class