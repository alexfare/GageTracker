Imports System.Data.OleDb

Public Class RemoveGage
    Dim ConnectionString As String
    Dim SearchCheck As Boolean

    Private Sub RemoveGage_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        LoadGageIDOptions()
        txtGageID.Focus()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ' Check if the GageID text box is empty
        If String.IsNullOrWhiteSpace(txtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Confirm with the user before deleting the record
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Try
                Using conn As New OleDbConnection(ConnectionString)
                    conn.Open()
                    ' Execute the DELETE query
                    Dim deleteCmd As New OleDbCommand("DELETE FROM [CalibrationTracker] WHERE GageID = ?", conn)
                    deleteCmd.Parameters.AddWithValue("@GageID", txtGageID.Text)
                    Dim rowsAffected As Integer = deleteCmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("Record deleted successfully")
                        ' Clear the form fields after deletion
                        ClearFormFields()
                    Else
                        MessageBox.Show("No record deleted. Please check the GageID.")
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub ClearFormFields()
        ' Clear all input fields
        txtGageID.SelectedIndex = -1 ' Reset the ComboBox selection
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    Private Sub LoadGageIDOptions()
        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageID FROM [CalibrationTracker]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtGageID.Items.Clear()
                While reader.Read()
                    txtGageID.Items.Add(reader("GageID").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading GageID options: " & ex.Message)
            End Try
        End Using
    End Sub
End Class