Imports System.Data.OleDb
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class GageType
    Dim connectionString As String
    Dim insertQuery As String = "INSERT INTO GageType (GageType) VALUES (@GageType)"

    Private Sub GageType_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
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
                MessageBox.Show("An error occurred while loading GageType: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading GageType: " & ex.Message
                Logger.LogErrors()
            End Try
        End Using
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(txtGageType.Text) Then
            StatusLabel.Text = "GageType Name cannot be blank."
            Timer1.Enabled = True
            Return
        End If

        Using checkConn As New OleDbConnection(connectionString)
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM GageType WHERE GageType = @Name", checkConn)
            checkCmd.Parameters.AddWithValue("@Name", txtGageType.Text)

            Try
                checkConn.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    StatusLabel.Text = "This GageType already exists. Please enter a unique GageType."
                    Timer1.Enabled = True
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
                command.Parameters.AddWithValue("@GageType", txtGageType.Text)  ' Updated parameter name to match the query

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    StatusLabel.Text = "GageType added successfully."
                    GlobalVars.SystemLog = txtGageType.Text + " gage type added successfully."
                    Logger.LogSystem()
                    Timer1.Enabled = True
                    txtGageType.Items.Add(txtGageType.Text)

                    txtGageType.Text = String.Empty
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new GageType: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while adding new GageType: " & ex.Message
                    Logger.LogErrors()
                End Try
            End Using
        End Using
    End Sub
    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If txtGageType.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtGageType.Text) Then
            MessageBox.Show("Please select a GageType to remove.")
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete this GageType?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
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
                        StatusLabel.Text = "GageType deleted successfully."
                        GlobalVars.SystemLog = selectedGageType + " gage type deleted successfully."
                        Logger.LogSystem()
                        Timer1.Enabled = True
                        txtGageType.Items.Remove(selectedGageType)
                        txtGageType.Text = String.Empty
                        txtGageType.SelectedIndex = -1
                    Else
                        MessageBox.Show("No records were deleted.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while deleting the GageType: " & ex.Message)
                    GlobalVars.ErrorLog = "An error occurred while deleting the GageType: " & ex.Message
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
    End Sub
End Class