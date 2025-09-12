Imports System.Data.OleDb
Imports System.Net

Public Class GageList
    Private isClosing As Boolean = False
    Private selectedGage As String = ""
    Private WithEvents filterTimer As New Timer With {.Interval = 300}

#Region "GageList Load"
    Private Sub GageList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AddHandler DataGridView1.SelectionChanged, AddressOf DataGridView1_SelectionChanged
        AddHandler DataGridView1.CellDoubleClick, AddressOf DataGridView1_CellDoubleClick
        Dim getVersion As String = My.Settings.VersionString
        Me.Text = "GageTracker - GageList - " + getVersion

        DataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridView1.MultiSelect = False

        TextContains.Text = ""

        Try
            GlobalVars.LoadDatabaseLocation()
            LoadData()
        Catch ex As OleDbException
            MessageBox.Show("Database error: " & ex.Message)
            Logger.LogErrors("Database error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("General error: " & ex.Message)
            Logger.LogErrors("General error: " & ex.Message)
        End Try

        FilterSetup()
        ApplyStatusFilter()
        MenuColor()
        StartDashboard()
    End Sub
#End Region

#Region "Buttons"
    Private Sub BtnMenu_Click(sender As Object, e As EventArgs) Handles BtnMenu.Click
        GTMenu.Show()
    End Sub
#End Region

#Region "Filter"
    Private Sub CmbContains_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbContains.SelectedIndexChanged, CmbFilterType.SelectedIndexChanged
        ApplyFilter()
    End Sub

    Private Sub TextContains_TextChanged(sender As Object, e As EventArgs) Handles TextContains.TextChanged
        filterTimer.Stop()
        filterTimer.Start()
    End Sub

    Private Sub filterTimer_Tick(sender As Object, e As EventArgs) Handles filterTimer.Tick
        filterTimer.Stop()
        ApplyFilter()
    End Sub

    Private Sub ApplyFilter()
        If CmbContains.SelectedItem Is Nothing OrElse CmbFilterType.SelectedItem Is Nothing Then
            Exit Sub
        End If

        Dim selectedColumn As String = CmbContains.SelectedItem.ToString()
        Dim filterText As String = TextContains.Text.Trim()
        Dim filterType As String = CmbFilterType.SelectedItem.ToString()

        Dim baseFilter As String = ""
        If Not My.Settings.ShowAll Then
            baseFilter = "[Status] = 'Active'"
        End If

        Dim textFilter As String = ""
        If Not String.IsNullOrEmpty(filterText) Then
            If filterType = "Contains" Then
                textFilter = $"[{selectedColumn}] LIKE '%{filterText}%'"
            ElseIf filterType = "Exact" Then
                textFilter = $"[{selectedColumn}] = '{filterText}'"
            End If
        End If

        Dim combinedFilter As String = ""
        If Not String.IsNullOrEmpty(baseFilter) AndAlso Not String.IsNullOrEmpty(textFilter) Then
            combinedFilter = baseFilter & " AND " & textFilter
        ElseIf Not String.IsNullOrEmpty(baseFilter) Then
            combinedFilter = baseFilter
        ElseIf Not String.IsNullOrEmpty(textFilter) Then
            combinedFilter = textFilter
        End If

        LoadData(combinedFilter)
    End Sub

    Private Sub FilterSetup()
        CmbFilterType.Items.Add("Contains")
        CmbFilterType.Items.Add("Exact")
        CmbFilterType.SelectedIndex = 0
        CmbContains.Items.AddRange(New String() {"GageID", "Status", "PartNumber", "Description", "Department", "Gage Type", "Customer", "Inspected Date", "Due Date", "Comments"})
        CmbContains.SelectedIndex = 0
        CheckBoxShowAll.Checked = My.Settings.ShowAll
    End Sub

    Private Sub CheckBoxShowAll_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxShowAll.CheckedChanged
        My.Settings.ShowAll = CheckBoxShowAll.Checked
        My.Settings.Save()
        If TextContains.Text <> "" Then
            Dim previousText As String = TextContains.Text
            ApplyFilter()
            TextContains.Text = previousText
        Else
            ApplyFilter()
        End If
    End Sub

    Public Sub ApplyStatusFilter()
        Dim filterQuery As String = ""
        If Not My.Settings.ShowAll Then
            filterQuery = "[Status] = 'Active'"
        End If
        LoadData(filterQuery)
    End Sub
#End Region

#Region "Database"
    Public Sub LoadData(Optional filterQuery As String = "")
        Cursor.Current = Cursors.WaitCursor
        DataGridView1.DataSource = Nothing
        Application.DoEvents()

        If Not System.IO.File.Exists(GlobalVars.DatabaseLocation) Then
            If PromptForDatabaseLocation() Then
                LoadData()
                Return
            Else
                MessageBox.Show("No valid database selected. The application will exit.",
                            "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                Logger.LogErrors("No valid database selected.")
                Environment.Exit(0)
            End If
        End If

        Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
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
                Cursor.Current = Cursors.Default

            Catch ex As OleDbException
                Logger.LogErrors("OleDb error: " & ex.Message)
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
                Logger.LogErrors("An error occurred: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Function PromptForDatabaseLocation() As Boolean
        Dim result As DialogResult = MessageBox.Show("No database found. Would you like to select a new database location?",
                                                 "Database Not Found", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
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
        Else
            Dim downloadResult As DialogResult = MessageBox.Show("Would you like to download the database instead?",
                                                             "Download Option", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly)
            If downloadResult = DialogResult.Yes Then
                Dim saveDialog As New SaveFileDialog()
                saveDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
                saveDialog.Title = "Save Database File"
                saveDialog.FileName = "GTDatabase.accdb"

                If saveDialog.ShowDialog() = DialogResult.OK Then
                    DownloadDatabase(saveDialog.FileName)
                    Return True
                End If
            Else
                LoadingScreen.Close()
                MessageBox.Show("No valid database selected. The application will exit.", "Database Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Environment.Exit(0)
            End If
        End If
        Return False
    End Function

    Private Sub DownloadDatabase(savePath As String)
        Dim webClient As New WebClient()

        Try
            Dim downloadUrl As String = "https://alexfare.com/programs/gtdatabase/latest/GTDatabase.accdb"
            webClient.DownloadFile(downloadUrl, savePath)
            MessageBox.Show("Database downloaded complete.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

            GlobalVars.DatabaseLocation = savePath
            GlobalVars.SaveDatabaseLocation(savePath)
        Catch ex As Exception
            MessageBox.Show("An error occurred while downloading the database: " & ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Logger.LogErrors("An error occurred while downloading the database: " & ex.Message)
        Finally
            webClient.Dispose()
        End Try
    End Sub
#End Region

#Region "DataGrid"
    Private Sub DataGridView1_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridView1.RowPostPaint ' Set Zebra striping
        Dim grid As DataGridView = CType(sender, DataGridView)
        If e.RowIndex >= 0 Then
            If e.RowIndex Mod 2 = 0 Then
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGray
            Else
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
            End If
        End If
    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If
        End If
    End Sub

    Private Sub DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If DataGridView1.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If
            GTMenu.Show()
            GTMenu.LoadGageID()
        End If
    End Sub
#End Region

#Region "Settings & Misc"
    Private Sub StartLogin()
        LoginForm1.Show()
        My.Settings.FromList = True
    End Sub

    Private Sub StartAdmin()
        AdminMenu.Show()
        My.Settings.FromList = True
    End Sub

    Public Sub MenuColor()
        If My.Settings.isAdmin = True Then
            MenuStrip1.BackColor = Color.IndianRed
        Else
            MenuStrip1.BackColor = SystemColors.AppWorkspace
        End If
    End Sub

    Private Sub StartDashboard()
        Dashboard.Show()
    End Sub
#End Region

#Region "Menu Toolbar Strip"
    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub PastDueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PastDueToolStripMenuItem.Click
        GlobalVars.DueDateMenuSelect = "Past"
        DueDateCategorizer.Show()
    End Sub

    Private Sub DueWithin30DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueWithin30DaysToolStripMenuItem.Click
        GlobalVars.DueDateMenuSelect = "30"
        DueDateCategorizer.Show()
    End Sub

    Private Sub DueWithin60DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueWithin60DaysToolStripMenuItem.Click
        GlobalVars.DueDateMenuSelect = "60"
        DueDateCategorizer.Show()
    End Sub

    Private Sub GithubToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GithubToolStripMenuItem.Click
        Dim url As String = "https://github.com/alexfare/GageTracker"
        Try
            Process.Start(url)
        Catch ex As Exception
            MessageBox.Show("An error occurred while trying to open the URL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Logger.LogErrors("An error occurred while trying to open the URL: " & ex.Message)
        End Try
    End Sub

    Private Sub WebsiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WebsiteToolStripMenuItem.Click
        Dim url As String = "https://alexfare.com/programs/gagetracker/latest/"
        Try
            Process.Start(url)
        Catch ex As Exception
            MessageBox.Show("An error occurred while trying to open the URL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Logger.LogErrors("An error occurred while trying to open the URL: " & ex.Message)
        End Try
    End Sub

    Private Sub MenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuToolStripMenuItem.Click
        GlobalVars.selectedGage = ""
        GTMenu.Show()
    End Sub

    Private Sub ReportIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportIssueToolStripMenuItem.Click
        ReportIssue.Show()
    End Sub

    Private Sub AdminToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminToolStripMenuItem.Click
        If My.Settings.isAdmin = True Then
            StartAdmin()
        Else
            StartLogin()
        End If
    End Sub
    Private Sub RefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RefreshToolStripMenuItem.Click
        LoadData()
        FilterSetup()
        ApplyStatusFilter()
    End Sub

    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        Dashboard.Show()
    End Sub
#End Region

#Region "Closing Actions"
    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If isClosing Then
            Return
        End If

        If My.Settings.ExitConfirmation = True Then
            PerformExitTasks()
            Return
        End If

        Dim confirmForm As New ExitConfirmationForm
        Dim result = confirmForm.ShowDialog()
        If result = DialogResult.Yes Then
            PerformExitTasks()
        Else
            e.Cancel = True
        End If
    End Sub

    Private Sub PerformExitTasks()
        isClosing = True

        Dim openForms As New List(Of Form)(Application.OpenForms.Cast(Of Form)())
        For Each frm As Form In openForms
            frm.Close()
        Next

        My.Settings.LastActivity = GlobalVars.LastActivity
        My.Settings.isAdmin = False
        My.Settings.LoggedUser = ""
        My.Settings.Save()

        Application.Exit()
    End Sub
#End Region

End Class