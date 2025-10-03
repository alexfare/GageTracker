Imports System.Data.OleDb

Public Class DueDateCategorizer

#Region "Loading"
    Private selectedGage As String
    Private Sub DueDateCategorizer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormSetup()

        Try
            LoadData()
        Catch ex As OleDbException
            MessageBox.Show("Database error: " & ex.Message)
            Logger.LogErrors("Database error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("General error: " & ex.Message)
            Logger.LogErrors("General error: " & ex.Message)
        End Try
    End Sub

    Private Sub FormSetup()
        ModernTheme.Apply(Me)
        ApplyModernLayout()
        DatagridHandler()
        TabSelect()
        MenuColor()
    End Sub

    Private Sub DatagridHandler()
        'Double Click
        AddHandler DataGridViewPastDue.CellDoubleClick, AddressOf DataGridViewPastDue_CellDoubleClick
        AddHandler DataGridViewWithin30Days.CellDoubleClick, AddressOf DataGridViewWithin30Days_CellDoubleClick
        AddHandler DataGridViewWithin60Days.CellDoubleClick, AddressOf DataGridViewWithin60Days_CellDoubleClick

        'Selection
        AddHandler DataGridViewPastDue.SelectionChanged, AddressOf DataGridViewPastDue_SelectionChanged
        AddHandler DataGridViewWithin30Days.SelectionChanged, AddressOf DataGridViewWithin30Days_SelectionChanged
        AddHandler DataGridViewWithin60Days.SelectionChanged, AddressOf DataGridViewWithin60Days_SelectionChanged

        'Select Line
        DataGridViewPastDue.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewPastDue.MultiSelect = False
        DataGridViewWithin30Days.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewWithin30Days.MultiSelect = False
        DataGridViewWithin60Days.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        DataGridViewWithin60Days.MultiSelect = False
    End Sub
#End Region

#Region "Settings"
    Public Sub LoadData()
        Dim query As String = "SELECT GageID, [Status], [PartNumber], [Description], Department, [Gage Type], [Customer], [Inspected Date], [Due Date] FROM CalibrationTracker"

        Using connection As OleDbConnection = DatabaseHelper.GetConnection()
            Try
                connection.Open()
                Dim command As New OleDbCommand(query, connection)
                Dim adapter As New OleDbDataAdapter(command)
                Dim table As New DataTable()
                adapter.Fill(table)

                'Filter and categorize the data
                Dim pastDue As DataTable = table.Clone()
                Dim within30Days As DataTable = table.Clone()
                Dim within60Days As DataTable = table.Clone()
                Dim today As Date = Date.Today

                For Each row As DataRow In table.Rows
                    Dim dueDate As Date = Convert.ToDateTime(row("Due Date"))
                    If dueDate < today Then
                        pastDue.ImportRow(row)
                    ElseIf dueDate >= today AndAlso dueDate <= today.AddDays(30) Then
                        within30Days.ImportRow(row)
                    ElseIf dueDate > today.AddDays(30) AndAlso dueDate <= today.AddDays(60) Then
                        within60Days.ImportRow(row)
                    End If
                Next

                DataGridViewPastDue.DataSource = pastDue
                DataGridViewWithin30Days.DataSource = within30Days
                DataGridViewWithin60Days.DataSource = within60Days

                'Column Aut size
                DataGridViewPastDue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                DataGridViewWithin30Days.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                DataGridViewWithin60Days.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

            Catch ex As OleDbException
                MessageBox.Show("OleDb error: " & ex.Message)
                Logger.LogErrors("OleDb error: " & ex.Message)
            Finally
                If connection.State = ConnectionState.Open Then
                    connection.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub MenuColor()
        If MenuStrip1 Is Nothing Then
            Return
        End If

        Dim baseColor = If(My.Settings.isAdmin, ModernTheme.DangerColor, ModernTheme.SecondaryColor)
        MenuStrip1.BackColor = Color.FromArgb(220, baseColor)
        MenuStrip1.ForeColor = Color.WhiteSmoke
        MenuStrip1.Renderer = New ModernMenuRenderer()
    End Sub
#End Region

#Region "ZebraStripingDataGrids"
    Private Sub DataGridView_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs)
        Dim grid As DataGridView = CType(sender, DataGridView)
        If e.RowIndex >= 0 Then
            If e.RowIndex Mod 2 = 0 Then
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = ModernTheme.SurfaceColor
            Else
                grid.Rows(e.RowIndex).DefaultCellStyle.BackColor = ModernTheme.AdjustColor(ModernTheme.SurfaceColor, -0.08)
            End If
        End If
    End Sub

    Private Sub DataGridViewPastDue_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridViewPastDue.RowPostPaint
        DataGridView_RowPostPaint(sender, e)
    End Sub

    Private Sub DataGridViewWithin30Days_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridViewWithin30Days.RowPostPaint
        DataGridView_RowPostPaint(sender, e)
    End Sub

    Private Sub DataGridViewWithin60Days_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles DataGridViewWithin60Days.RowPostPaint
        DataGridView_RowPostPaint(sender, e)
    End Sub
#End Region

#Region "Modern UI"
    Private Sub ApplyModernLayout()
        Dim grids = New DataGridView() {DataGridViewPastDue, DataGridViewWithin30Days, DataGridViewWithin60Days}
        For Each grid In grids
            grid.BorderStyle = BorderStyle.None
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None
            grid.RowHeadersVisible = False
        Next

        For Each tab As TabPage In TabControl1.TabPages
            tab.BackColor = ModernTheme.SecondaryColor
            tab.ForeColor = Color.WhiteSmoke
        Next

        BtnGageList.FlatStyle = FlatStyle.Flat
        BtnGageList.FlatAppearance.BorderSize = 0
        BtnGageList.BackColor = ModernTheme.AccentColor
        BtnGageList.ForeColor = Color.White
        BtnGageList.FlatAppearance.MouseOverBackColor = ModernTheme.AdjustColor(BtnGageList.BackColor, 0.2)
        BtnGageList.FlatAppearance.MouseDownBackColor = ModernTheme.AdjustColor(BtnGageList.BackColor, -0.15)
    End Sub
#End Region

#Region "MenuStrip"
    Private Sub MenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuToolStripMenuItem.Click
        GTMenu.Show()
        GTMenu.LoadGageID()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        Me.Close()
    End Sub
#End Region

#Region "Datagrid"
    Private Sub DataGridViewPastDue_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If DataGridViewPastDue.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridViewPastDue.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If

            GTMenu.Show()
            GTMenu.LoadGageID()
            Me.Close()
        End If
    End Sub

    Private Sub DataGridViewWithin30Days_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If DataGridViewWithin30Days.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridViewWithin30Days.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If

            GTMenu.Show()
            GTMenu.LoadGageID()
            Me.Close()
        End If
    End Sub

    Private Sub DataGridViewWithin60Days_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        If DataGridViewWithin60Days.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridViewWithin60Days.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If

            GTMenu.Show()
            GTMenu.LoadGageID()
            Me.Close()
        End If
    End Sub

    Private Sub DataGridViewPastDue_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridViewPastDue.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridViewPastDue.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If
        End If
    End Sub

    Private Sub DataGridViewWithin30Days_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridViewWithin30Days.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridViewWithin30Days.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If
        End If
    End Sub

    Private Sub DataGridViewWithin60Days_SelectionChanged(sender As Object, e As EventArgs)
        If DataGridViewWithin60Days.SelectedRows.Count > 0 Then
            Dim selectedRow As DataGridViewRow = DataGridViewWithin60Days.SelectedRows(0)

            If Not selectedRow.IsNewRow AndAlso selectedRow.Cells.Count > 0 AndAlso selectedRow.Cells(0) IsNot Nothing AndAlso Not IsDBNull(selectedRow.Cells(0).Value) Then
                selectedGage = selectedRow.Cells(0).Value.ToString()
                GlobalVars.selectedGage = selectedGage
            End If
        End If
    End Sub
#End Region

#Region "Misc"
    Private Sub BtnGageList_Click(sender As Object, e As EventArgs) Handles BtnGageList.Click
        Me.Close()
    End Sub

    Private Sub TabSelect()
        If GlobalVars.DueDateMenuSelect = "Past" Then
            TabControl1.SelectedIndex = 0
        ElseIf GlobalVars.DueDateMenuSelect = "30" Then
            TabControl1.SelectedIndex = 1
        ElseIf GlobalVars.DueDateMenuSelect = "60" Then
            TabControl1.SelectedIndex = 2
        Else
            TabControl1.SelectedIndex = 0
        End If
        GlobalVars.DueDateMenuSelect = ""
    End Sub
#End Region
End Class