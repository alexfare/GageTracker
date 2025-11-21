Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text

Public Class AccountManagement

    Private Sub CreateAccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        LoadUsers()
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If txtUsername.Text = "" OrElse txtPassword.Text = "" OrElse txtConfirmPassword.Text = "" Then
            ShowStatus("Please fill in all fields.", True)
            Return
        End If

        If txtPassword.Text <> txtConfirmPassword.Text Then
            ShowStatus("Passwords do not match.", True)
            Return
        End If

        Dim hashedPassword As String = HashPassword(txtPassword.Text)

        SaveCredentials(txtUsername.Text, hashedPassword)
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha512 As SHA512 = SHA512Managed.Create()
            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha512.ComputeHash(bytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))
            Next
            Return sb.ToString()
        End Using
    End Function

    Private Sub SaveCredentials(username As String, hashedPassword As String)
        Using conn As OleDbConnection = DatabaseHandler.GetConnection()
            conn.Open()

            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Credentials WHERE [Username] = ?", conn)
            checkCmd.Parameters.Add(New OleDbParameter("@Username", username))
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count > 0 Then
                Dim result As DialogResult = MessageBox.Show("Username already exists. Do you want to update the password?", "Update Confirmation", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    Dim updateCmd As New OleDbCommand("UPDATE Credentials SET [Password] = ? WHERE [Username] = ?", conn)
                    updateCmd.Parameters.Add(New OleDbParameter("@Password", hashedPassword))
                    updateCmd.Parameters.Add(New OleDbParameter("@Username", username))
                    updateCmd.ExecuteNonQuery()
                    ShowStatus("Password updated successfully.", False)
                    Logger.LogSystem(username + " password updated.")
                    ClearInputs()
                End If
                Return
            End If

            Dim cmd As New OleDbCommand("INSERT INTO Credentials ([Username], [Password]) VALUES (?, ?)", conn)
            cmd.Parameters.Add(New OleDbParameter("@Username", username))
            cmd.Parameters.Add(New OleDbParameter("@Password", hashedPassword))
            cmd.ExecuteNonQuery()
            ShowStatus("User created successfully.", False)
            Logger.LogSystem("User " + username + " created.")
            txtUsername.SelectedIndex = -1
            LoadUsers()
            ClearInputs()
        End Using
    End Sub

    Private Sub LoadUsers()
        Using conn As OleDbConnection = DatabaseHandler.GetConnection()
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT [Username] FROM [Credentials]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtUsername.Items.Clear()
                While reader.Read()
                    txtUsername.Items.Add(reader("Username").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Usernames: " & ex.Message)
                Logger.LogErrors("An error occurred while loading Usernames: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim username As String = txtUsername.Text
        If String.IsNullOrEmpty(username) Then
            ShowStatus("Please enter a username to delete.", True)
            Return
        End If

        Using conn As OleDbConnection = DatabaseHandler.GetConnection()
            conn.Open()

            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Credentials WHERE [Username] = ?", conn)
            checkCmd.Parameters.Add(New OleDbParameter("@Username", username))
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count = 0 Then
                ShowStatus("Username does not exist.", True)
                Return
            End If

            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this user?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                Dim deleteCmd As New OleDbCommand("DELETE FROM Credentials WHERE [Username] = ?", conn)
                deleteCmd.Parameters.Add(New OleDbParameter("@Username", username))
                deleteCmd.ExecuteNonQuery()
                ShowStatus("User deleted successfully.", False)
                Logger.LogSystem("User " + username + " deleted.")
                ClearInputs()
                LoadUsers()
            End If
        End Using
    End Sub

    Private Sub ClearInputs()
        txtUsername.SelectedIndex = -1
        txtPassword.Clear()
        txtConfirmPassword.Clear()
    End Sub

    Private Sub ShowStatus(message As String, isError As Boolean)
        StatusLabel.ForeColor = If(isError, Color.Red, Color.Green)
        StatusLabel.Text = message
        Timer1.Enabled = True
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""
        Timer1.Enabled = False
        LoadUsers()
    End Sub
End Class