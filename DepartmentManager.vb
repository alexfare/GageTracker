Imports System.Data.OleDb

Public Class DepartmentManager
    Dim insertQuery As String = "INSERT INTO Departments (Departments) VALUES (@Departments)"

    Private Sub Departments_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        LoadDepartments()
    End Sub

    Private Sub LoadDepartments()
        Using connection As OleDbConnection = DatabaseHandler.GetConnection()
            Try
                connection.Open()
                Dim cmd As New OleDbCommand("SELECT Departments FROM Departments", connection)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtDepartments.Items.Clear()
                While reader.Read()
                    txtDepartments.Items.Add(reader("Departments").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Departments: " & ex.Message)
                Logger.LogErrors("An error occurred while loading Departments: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(txtDepartments.Text) Then
            ShowStatus("Department name cannot be blank.", False)
            Return
        End If

        Using connection As OleDbConnection = DatabaseHandler.GetConnection()
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Departments WHERE Departments = @Name", connection)
            checkCmd.Parameters.AddWithValue("@Name", txtDepartments.Text)

            Try
                connection.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    ShowStatus("This Department already exists. Please enter a unique Department name.", False)
                    Return
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while checking for duplicate names: " & ex.Message)
                Logger.LogErrors("An error occurred while checking for duplicate names: " & ex.Message)
                Return
            End Try
        End Using

        Using connection As OleDbConnection = DatabaseHandler.GetConnection()
            Using command As New OleDbCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@Departments", txtDepartments.Text)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    ShowStatus("Department added successfully.", False)
                    Logger.LogSystem(txtDepartments.Text + " Department added successfully.")
                    txtDepartments.Items.Add(txtDepartments.Text)
                    txtDepartments.Text = String.Empty
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new Department: " & ex.Message)
                    Logger.LogErrors("An error occurred while adding new Department: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        Dim DepartmentDelete As String

        If txtDepartments.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtDepartments.Text) Then
            ShowStatus("Please select a Department to remove.", True)
            Return
        End If

        DepartmentDelete = txtDepartments.Text

        If MessageBox.Show("Are you sure you want to delete " + DepartmentDelete + "?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Dim selectedDepartment As String = txtDepartments.SelectedItem.ToString()
        Dim query As String = "DELETE FROM Departments WHERE Departments = @Name"

        Using connection As OleDbConnection = DatabaseHandler.GetConnection()
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Name", selectedDepartment)

                Try
                    connection.Open()
                    Dim result As Integer = command.ExecuteNonQuery()
                    If result > 0 Then
                        ShowStatus("Department deleted successfully.", False)
                        Logger.LogSystem(selectedDepartment + "Department deleted successfully.")
                        txtDepartments.Items.Remove(selectedDepartment)
                        txtDepartments.Text = String.Empty
                        txtDepartments.SelectedIndex = -1
                    Else
                        MessageBox.Show("No records were deleted.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while deleting the Department: " & ex.Message)
                    Logger.LogErrors("An error occurred while deleting the Department: " & ex.Message)
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