Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text
Public Class LoginForm1
    Dim ConnectionString As String

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text

        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Password FROM [Credentials] WHERE Username = ?", conn)
                cmd.Parameters.AddWithValue("@Username", username)

                ' Retrieve the stored password hash from the database
                Dim storedPasswordHash As Object = cmd.ExecuteScalar()

                If storedPasswordHash IsNot Nothing Then
                    ' Hash the entered password
                    Dim enteredPasswordHash As String = HashPassword(password)

                    ' Compare the entered password hash with the stored hash
                    If enteredPasswordHash.Equals(storedPasswordHash.ToString(), StringComparison.OrdinalIgnoreCase) Then
                        ' Passwords match
                        Dim adminMenu As New AdminMenu()
                        adminMenu.Show()
                        My.Settings.LoggedUser = txtUsername.Text

                        'New Settings
                        My.Settings.isAdmin = True
                        Me.Close()  ' Optionally hide or close the LoginForm
                    Else
                        MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                Else
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As OleDbException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        If My.Settings.FromList = True Then
            GageList.Show()
            My.Settings.FromList = False
        Else
            GTMenu.Show()
        End If
        Me.Hide()
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha512Hash As SHA512 = SHA512.Create()
            ' ComputeHash - returns byte array
            Dim bytes As Byte() = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(password))

            ' Convert byte array to a string
            Dim builder As New StringBuilder()
            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function
End Class
