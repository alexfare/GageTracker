Imports System.Data.OleDb

Public Class GageList
    Private Sub GageList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            LoadData()
        Catch ex As OleDbException
            MessageBox.Show("Database error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("General error: " & ex.Message)
        End Try
    End Sub

    Private Sub LoadData()
        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=R:\Quality\GageCalibration\GTDatabase.accdb;"
        Dim query As String = "SELECT GageID, [Status], [PartNumber], [Description], Department, [Gage Type], [Inspected Date], [Due Date] FROM CalibrationTracker"

        Using connection As New OleDbConnection(connectionString)
            Try
                connection.Open()
                Dim command As New OleDbCommand(query, connection)
                Dim adapter As New OleDbDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)
                DataGridView1.DataSource = table
            Catch ex As OleDbException
                MessageBox.Show("OleDb error: " & ex.Message)
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub BtnMenu_Click(sender As Object, e As EventArgs) Handles BtnMenu.Click
        GTMenu.Show()
    End Sub
End Class
