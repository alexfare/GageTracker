Imports System.Data.OleDb

Public Class GageList
    Private isClosing As Boolean = False
    Private selectedGage As String ' Variable to hold the selected GageID

    Private Sub GageList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            GlobalVars.LoadDatabaseLocation()
            LoadData()
        Catch ex As OleDbException
            MessageBox.Show("Database error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("General error: " & ex.Message)
        End Try

        Me.StartPosition = FormStartPosition.CenterScreen

        ' Populate the ComboBox for Filter Type
        CmbFilterType.Items.Add("Contains")
        CmbFilterType.Items.Add("Exact")
        CmbFilterType.SelectedIndex = 0 ' Default to Exact

        ' Populate the ComboBox for Contains with relevant column names
        CmbContains.Items.AddRange(New String() {"GageID", "Status", "PartNumber", "Description", "Department", "Gage Type", "Customer", "Inspected Date", "Due Date", "Comments"})
        CmbContains.SelectedIndex = 0 ' Default to Status

        ' Set default text for TextContains
        TextContains.Text = ""

        ' Add handler for DataGridView selection changed event
        AddHandler DataGridView1.SelectionChanged, AddressOf DataGridView1_SelectionChanged

        ' Add handler for DataGridView cell double click event
        AddHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Public Sub LoadData(Optional filterQuery As String = "")
        If Not System.IO.File.Exists(GlobalVars.DatabaseLocation) Then
            If PromptForDatabaseLocation() Then
                ' Try loading the data again with the new location
                LoadData()
                Return
            Else
                MessageBox.Show("No valid database selected. Application will exit.")
                Application.Exit()
            End If
        End If

        Dim connectionString As String
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        Dim query As String = "SELECT GageID, [Status], [PartNumber], [Description], Department, [Gage Type], [Customer], [Inspected Date], [Due Date] FROM CalibrationTracker"

        If Not String.IsNullOrEmpty(filterQuery) Then
            query &= " WHERE " & filterQuery
        End If

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

    Private Function PromptForDatabaseLocation() As Boolean
        Dim result As DialogResult = MessageBox.Show("No database found. Would you like to select a new database location?", "Database Not Found", MessageBoxButtons.YesNo)
        If result = DialogResult.Yes Then
            Using openFileDialog As New OpenFileDialog()
                openFileDialog.InitialDirectory = "C:\"
                openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
                openFileDialog.FilterIndex = 1
                openFileDialog.RestoreDirectory = True

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    GlobalVars.DatabaseLocation = openFileDialog.FileName
                    GlobalVars.SaveDatabaseLocation(GlobalVars.DatabaseLocation)
                    Return True
                End If
            End Using
        End If
        Return False
    End Function

    ' Set Zebra striping
    Private Sub DataGridView1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint
        Dim grid As DataGridView = CType(sender, DataGridView)
        If e.RowIndex >= 0 Then
            If e.RowIndex Mod 2 = 0 Then
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGray
            Else
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
            End If
        End If
    End Sub

    Private Sub BtnMenu_Click(sender As Object, e As EventArgs) Handles BtnMenu.Click
        ' Show the GTMenu form regardless of whether GageID was set
        GTMenu.Show()
        GTMenu.LoadGageID()
    End Sub

    '/----- Toolbar Strip -----/
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub ChangeDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangeDatabaseToolStripMenuItem.Click
        Using openFileDialog As New OpenFileDialog()
            openFileDialog.InitialDirectory = "C:\"
            openFileDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
            openFileDialog.FilterIndex = 1
            openFileDialog.RestoreDirectory = True

            If openFileDialog.ShowDialog() = DialogResult.OK Then
                GlobalVars.DatabaseLocation = openFileDialog.FileName
                GlobalVars.SaveDatabaseLocation(GlobalVars.DatabaseLocation)
            End If
        End Using
    End Sub

    Private Sub MenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuToolStripMenuItem.Click
        ' Show the GTMenu form regardless of whether GageID was set
        GTMenu.Show()
        GTMenu.LoadGageID()
    End Sub

    Private Sub ReportIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportIssueToolStripMenuItem.Click
        ReportIssue.Show()
    End Sub

    Private Sub AdminMenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminMenuToolStripMenuItem.Click
        If My.Settings.isAdmin = True Then
            StartAdmin()
        Else
            StartLogin()
        End If
    End Sub

    Private Sub StartLogin()
        Dim loginForm As New LoginForm1()
        loginForm.Show()
        My.Settings.FromList = True
    End Sub

    Private Sub StartAdmin()
        Dim adminMenu As New AdminMenu()
        adminMenu.Show()
        My.Settings.FromList = True
    End Sub

    Private Sub DueDateCalenderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueDateCalenderToolStripMenuItem.Click
        DueDateCategorizer.Show()
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        ' Check if the closing process has already been initiated
        If isClosing Then
            Return
        End If

        ' Check if the user really wants to close the application
        If MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            ' Set the flag to true to indicate the application is closing
            isClosing = True

            ' Ensure all forms are closed
            Dim openForms As New List(Of Form)(Application.OpenForms.Cast(Of Form)())
            For Each frm As Form In openForms
                frm.Close()
            Next

            ' Ensure all threads and resources are terminated
            Application.Exit()
        Else
            ' Cancel the close event if the user decides not to close
            e.Cancel = True
        End If
    End Sub

    Private Sub TextContains_TextChanged(sender As Object, e As EventArgs) Handles TextContains.TextChanged
        Dim selectedColumn As String = CmbContains.SelectedItem.ToString()
        Dim filterText As String = TextContains.Text.Trim()
        Dim filterType As String = CmbFilterType.SelectedItem.ToString()
        Dim filterQuery As String = ""

        If Not String.IsNullOrEmpty(filterText) Then
            If filterType = "Contains" Then
                filterQuery = "[" & selectedColumn & "] LIKE '%" & filterText & "%'"
            ElseIf filterType = "Exact" Then
                filterQuery = "[" & selectedColumn & "] = '" & filterText & "'"
            End If
        End If

        LoadData(filterQuery)
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Ensure the selected row is not a new row and has the required columns
            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                My.Settings.SelectedGage = selectedGage
                ' MessageBox.Show("Selected GageID: " & selectedGage) 'For debugging purposes
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            ' Ensure the selected row is not a new row and has the required columns
            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                My.Settings.SelectedGage = selectedGage
                ' MessageBox.Show("Selected GageID: " & selectedGage) 'For debugging purposes
            End If
            ' Open the GTMenu form
            GTMenu.Show()
            GTMenu.LoadGageID()
        End If
    End Sub
End Class
