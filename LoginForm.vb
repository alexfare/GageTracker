Imports System.Data.OleDb
Imports System.Security.Cryptography
Imports System.Text
Public Class LoginForm1
    Dim ConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"

    Private Sub LoginForm1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim username As String = txtUsername.Text
        Dim password As String = txtPassword.Text
        StatusLabel.Text = ""

        If String.IsNullOrWhiteSpace(txtUsername.Text) Then
            StatusLabel.Text = "Username cannot be blank."
            txtUsername.Focus()
            Timer1.Enabled = True
            Return
        End If

        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            StatusLabel.Text = "Password cannot be blank."
            txtPassword.Focus()
            Timer1.Enabled = True
            Return
        End If

        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Password FROM [Credentials] WHERE Username = ?", conn)
                cmd.Parameters.AddWithValue("@Username", username)

                Dim storedPasswordHash As Object = cmd.ExecuteScalar()

                If storedPasswordHash IsNot Nothing Then
                    Dim enteredPasswordHash As String = HashPassword(password)

                    If enteredPasswordHash.Equals(storedPasswordHash.ToString(), StringComparison.OrdinalIgnoreCase) Then
                        My.Settings.LoggedUser = username
                        My.Settings.isAdmin = True
                        My.Settings.Save()
                        GageList.MenuColor()
                        AdminMenu.Show()
                        Me.Close()

                        Logger.LogSystem(username + " has successfully logged in.")
                    Else
                        InvalidLogin()
                    End If
                Else
                    InvalidLogin()
                End If
            Catch ex As OleDbException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Logger.LogErrors("Database error: " & ex.Message)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Logger.LogErrors("An unexpected error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub InvalidLogin()
        txtPassword.Text = ""
        txtPassword.Focus()
        StatusLabel.Text = "Invalid username or password."
        Timer1.Enabled = True
        Logger.LogSystem("Invalid login attempt on user: " + txtUsername.Text)
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        If My.Settings.FromList = True Then
            Me.Close()
            My.Settings.FromList = False
        Else
            GTMenu.Show()
            Me.Close()
        End If
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha512Hash As SHA512 = SHA512.Create()
            Dim bytes As Byte() = sha512Hash.ComputeHash(Encoding.UTF8.GetBytes(password))

            Dim builder As New StringBuilder()
            For i As Integer = 0 To bytes.Length - 1
                builder.Append(bytes(i).ToString("x2"))
            Next
            Return builder.ToString()
        End Using
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""
        Timer1.Enabled = False
    End Sub
End Class