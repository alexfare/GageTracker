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
        If DataGridView1.Columns.Contains("GageID") Then ' Check if any row is selected in the DataGridView
            If DataGridView1.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
                ' Use Try-Catch to handle potential errors when accessing a cell value
                Try
                    Dim gageID As String = selectedRow.Cells("GageID").Value.ToString()
                    ' Set the GageID in the global variable if found
                    GlobalVars.GageIDString = gageID
                Catch ex As Exception
                    ' Handle the case where GageID could not be retrieved
                    MessageBox.Show("Error retrieving GageID: " & ex.Message)
                End Try
            Else
                ' Optionally, inform the user when no row is selected
                MessageBox.Show("No row selected. Proceeding without GageID.")
            End If
        Else
            'MessageBox.Show("The column 'GageID' does not exist.")
        End If
        ' Show the GTMenu form regardless of whether GageID was set
        GTMenu.Show()
        Me.Close() 'Temp until I work out the refreshing
    End Sub
End Class
