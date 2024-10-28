Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text

Public Class AccountManagement
    Dim connectionString As String

    Private Sub CreateAccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        LoadUsers()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Validate inputs
        If txtUsername.Text = "" OrElse txtPassword.Text = "" OrElse txtConfirmPassword.Text = "" Then
            StatusLabel.Text = "Please fill in all fields."
            Timer1.Enabled = True
            Return
        End If

        If txtPassword.Text <> txtConfirmPassword.Text Then
            StatusLabel.Text = "Passwords do not match."
            Timer1.Enabled = True
            Return
        End If

        ' Hash password
        Dim hashedPassword As String = HashPassword(txtPassword.Text)

        ' Save to database
        SaveCredentials(txtUsername.Text, hashedPassword)
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha512 As SHA512 = SHA512Managed.Create()
            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(password)
            Dim hash As Byte() = sha512.ComputeHash(bytes)
            Dim sb As New StringBuilder()
            For Each b As Byte In hash
                sb.Append(b.ToString("x2"))  ' Converts byte to hexadecimal
            Next
            Return sb.ToString()
        End Using
    End Function

    Private Sub SaveCredentials(username As String, hashedPassword As String)
        Using conn As New OleDbConnection(connectionString)
            conn.Open()

            ' Check if username already exists
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Credentials WHERE [Username] = ?", conn)
            checkCmd.Parameters.Add(New OleDbParameter("@Username", username))
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count > 0 Then
                ' Ask user if they want to update the password
                Dim result As DialogResult = MessageBox.Show("Username already exists. Do you want to update the password?", "Update Confirmation", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then
                    ' Update password
                    Dim updateCmd As New OleDbCommand("UPDATE Credentials SET [Password] = ? WHERE [Username] = ?", conn)
                    updateCmd.Parameters.Add(New OleDbParameter("@Password", hashedPassword))
                    updateCmd.Parameters.Add(New OleDbParameter("@Username", username))
                    updateCmd.ExecuteNonQuery()
                    StatusLabel.Text = "Password updated successfully."
                    Timer1.Enabled = True
                    ClearInputs()
                End If
                Return
            End If

            ' If username is unique, proceed to save the new credentials
            Dim cmd As New OleDbCommand("INSERT INTO Credentials ([Username], [Password]) VALUES (?, ?)", conn)
            cmd.Parameters.Add(New OleDbParameter("@Username", username))
            cmd.Parameters.Add(New OleDbParameter("@Password", hashedPassword))
            cmd.ExecuteNonQuery()
            StatusLabel.Text = "User created successfully."
            Timer1.Enabled = True
            txtUsername.SelectedIndex = -1 ' Reset the ComboBox selection
            LoadUsers()
            ClearInputs()
        End Using
    End Sub

    Private Sub LoadUsers()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT [Username] FROM [Credentials]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtUsername.Items.Clear()
                While reader.Read()
                    txtUsername.Items.Add(reader("Username").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Usernames: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim username As String = txtUsername.Text
        If String.IsNullOrEmpty(username) Then
            StatusLabel.Text = "Please enter a username to delete."
            Timer1.Enabled = True
            Return
        End If

        Using conn As New OleDbConnection(connectionString)
            conn.Open()

            ' Check if username exists
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Credentials WHERE [Username] = ?", conn)
            checkCmd.Parameters.Add(New OleDbParameter("@Username", username))
            Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If count = 0 Then
                StatusLabel.Text = "Username does not exist."
                Timer1.Enabled = True
                Return
            End If

            ' Confirm deletion
            Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this user?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If result = DialogResult.Yes Then
                ' Execute delete operation
                Dim deleteCmd As New OleDbCommand("DELETE FROM Credentials WHERE [Username] = ?", conn)
                deleteCmd.Parameters.Add(New OleDbParameter("@Username", username))
                deleteCmd.ExecuteNonQuery()
                StatusLabel.Text = "User deleted successfully."
                Timer1.Enabled = True
                ClearInputs()
                LoadUsers()
            End If
        End Using
    End Sub

    Private Sub ClearInputs()
        txtUsername.SelectedIndex = -1 'Reset the ComboBox selection
        txtPassword.Clear()
        txtConfirmPassword.Clear()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""  'Clear the text
        Timer1.Enabled = False  'Stop the timer
        LoadUsers()
    End Sub
End Class