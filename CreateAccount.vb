Imports System.Security.Cryptography
Imports System.Data.OleDb
Imports System.Text

Public Class CreateAccount
    Dim connectionString As String

    Private Sub CreateAccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    End Sub
    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        ' Validate inputs
        If txtUsername.Text = "" OrElse txtPassword.Text = "" OrElse txtConfirmPassword.Text = "" Then
            MessageBox.Show("Please fill in all fields.")
            Return
        End If

        If txtPassword.Text <> txtConfirmPassword.Text Then
            MessageBox.Show("Passwords do not match.")
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
                MessageBox.Show("Username already exists. Please choose a different username.")
                Return
            End If

            ' If username is unique, proceed to save the new credentials
            Dim cmd As New OleDbCommand("INSERT INTO Credentials ([Username], [Password]) VALUES (?, ?)", conn)
            cmd.Parameters.Add(New OleDbParameter("@Username", username))
            cmd.Parameters.Add(New OleDbParameter("@Password", hashedPassword))
            cmd.ExecuteNonQuery()
            MessageBox.Show("User account created successfully.")
            txtUsername.Clear()
            txtPassword.Clear()
            txtConfirmPassword.Clear()
        End Using
    End Sub


    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        AdminMenu.Show()
        Me.Close()
    End Sub
End Class