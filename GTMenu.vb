Imports System.Data.OleDb

Public Class GTMenu
    Private connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    Private SearchCheck As Boolean
    Private activeUser As String
    Private ChangeDetected As Boolean
    Private CanUpdate As Boolean
    Private GageSearch As String
    Private isClosing As Boolean = False
    Private saveString As String = ""
    Dim originalTitle As String = "GageTracker - Menu"
    Public WithEvents Timer1 As New Timer With {.Interval = 3000, .Enabled = False}
    Public WithEvents UpdateTimer As New Timer With {.Interval = 3100, .Enabled = False}
    Public WithEvents ReloadTimer As New Timer With {.Interval = 3100, .Enabled = False}

#Region "GTMenu Load"
    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        SetupUI()
        LoadData()

        If Not String.IsNullOrEmpty(GlobalVars.selectedGage) Then
            TxtGageID.Text = GlobalVars.selectedGage
            BtnSearch_Click(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub SetupUI()
        Me.Text = originalTitle
        MenuStrip1.BackColor = SystemColors.AppWorkspace
        SearchCheck = False
        CanUpdate = False
        StatusLabel.Text = ""
        TxtGageID.Focus()
    End Sub

    Private Async Sub LoadData()
        DisableSearchControls() 'Disable search controls
        Await Task.WhenAll(Task.Run(Sub() LoadGageID()),
                           Task.Run(Sub() LoadStatus()),
                           Task.Run(Sub() LoadDepartment()),
                           Task.Run(Sub() LoadGageType()),
                           Task.Run(Sub() LoadUser()),
                           Task.Run(Sub() LoadCustomers()))
        EnableSearchControls() 'Enable search controls
    End Sub

    Private Sub ReloadData()
        LoadGageID()
        GageList.LoadData()
        DueDateCategorizer.LoadData()
        GageList.ApplyStatusFilter()
    End Sub
#End Region

#Region "Load"
    Public Sub LoadGageID()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageID FROM [CalibrationTracker]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)()

                While reader.Read()
                    items.Add(reader("GageID").ToString())
                End While

                reader.Close()
                items.Sort()

                Me.Invoke(Sub()
                              TxtGageID.Items.Clear()
                              TxtGageID.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                              TxtGageID.AutoCompleteSource = AutoCompleteSource.ListItems

                              For Each item As String In items
                                  TxtGageID.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading GageID options: " & ex.Message)
                GlobalVars.ErrorLog = ("An error occurred while loading GageID options: " & ex.Message)
                Logger.LogErrors()
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadStatus()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Status FROM [Status]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)()

                While reader.Read()
                    items.Add(reader("Status").ToString())
                End While

                Me.Invoke(Sub()
                              cmbStatus.Items.Clear()
                              For Each item As String In items
                                  cmbStatus.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading status options: " & ex.Message)
                GlobalVars.ErrorLog = ("An error occurred while loading status options: " & ex.Message)
                Logger.LogErrors()
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadDepartment()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Departments FROM [Departments]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)()

                While reader.Read()
                    items.Add(reader("Departments").ToString())
                End While

                Me.Invoke(Sub()
                              txtDepartment.Items.Clear()
                              For Each item As String In items
                                  txtDepartment.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Departments options: " & ex.Message)
                GlobalVars.ErrorLog = ("An error occurred while loading Departments options: " & ex.Message)
                Logger.LogErrors()
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadGageType()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageType FROM [GageType]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)()

                While reader.Read()
                    items.Add(reader("GageType").ToString())
                End While

                Me.Invoke(Sub()
                              txtGageType.Items.Clear()
                              For Each item As String In items
                                  txtGageType.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage Type options: " & ex.Message)
                GlobalVars.ErrorLog = ("An error occurred while loading Gage Type options: " & ex.Message)
                Logger.LogErrors()
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadCustomers()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT CustomerName FROM Customers", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)()

                While reader.Read()
                    items.Add(reader("CustomerName").ToString())
                End While

                Me.Invoke(Sub()
                              txtCustomer.Items.Clear()
                              For Each item As String In items
                                  txtCustomer.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Customer options: " & ex.Message)
                GlobalVars.ErrorLog = ("An error occurred while loading Customer options: " & ex.Message)
                Logger.LogErrors()
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadUser()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Username FROM [Credentials]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)()

                While reader.Read()
                    items.Add(reader("Username").ToString())
                End While

                Me.Invoke(Sub()
                              txtCalibratedBy.Items.Clear()
                              For Each item As String In items
                                  txtCalibratedBy.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage Type options: " & ex.Message)
                GlobalVars.ErrorLog = ("An error occurred while loading Gage Type options: " & ex.Message)
                Logger.LogErrors()
            Finally
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub
#End Region

#Region "GTMenu Buttons"
    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        If String.IsNullOrWhiteSpace(TxtGageID.Text) Then
            'MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If ChangeDetected = True Then
            If MessageBox.Show("Changes detected! Any unsaved changes will be lost. Do you want to perform a new search?", "Exit", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                SearchHandler()
            Else
                TxtGageID.Text = saveString
            End If
        Else
            SearchHandler()
            PauseUserInput(2)
            saveString = TxtGageID.Text
            CanUpdate = True
        End If
    End Sub

    Private Sub SearchHandler()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT PartNumber, PartRev, [Status], Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments, aN1, aN2, aN3, aN4, aN5, aA1, aA2, aA3, aA4, aA5, [Serial Number], Owner, [Nist Number] FROM [CalibrationTracker] WHERE GageID = ?", conn)
                cmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtPartNumber.Text = reader.Item("PartNumber").ToString()
                        txtPartRev.Text = reader.Item("PartRev").ToString()
                        cmbStatus.Text = reader.Item("Status").ToString()
                        txtDescription.Text = reader.Item("Description").ToString()
                        txtDepartment.Text = reader.Item("Department").ToString()
                        txtGageType.Text = reader.Item("Gage Type").ToString()
                        txtCustomer.Text = reader.Item("Customer").ToString()
                        txtCalibratedBy.Text = reader.Item("Calibrated By").ToString()
                        TxtInterval.Text = reader.Item("Interval (Months)").ToString()

                        If Not IsDBNull(reader.Item("Inspected Date")) Then
                            DtInspectedDate.Value = DateTime.Parse(reader.Item("Inspected Date").ToString())
                        End If
                        If Not IsDBNull(reader.Item("Due Date")) Then
                            dtDueDate.Value = DateTime.Parse(reader.Item("Due Date").ToString())
                        End If

                        txtComments.Text = reader.Item("Comments").ToString()
                        TxtSerialNumber.Text = reader.Item("Serial Number").ToString()
                        txtOwner.Text = reader.Item("Owner").ToString()
                        TxtNistNumber.Text = reader.Item("Nist Number").ToString()

                        'Measurements
                        txtaN1.Text = reader.Item("aN1").ToString()
                        txtaN2.Text = reader.Item("aN2").ToString()
                        txtaN3.Text = reader.Item("aN3").ToString()
                        txtaN4.Text = reader.Item("aN4").ToString()
                        txtaN5.Text = reader.Item("aN5").ToString()
                        txtaA1.Text = reader.Item("aA1").ToString()
                        txtaA2.Text = reader.Item("aA2").ToString()
                        txtaA3.Text = reader.Item("aA3").ToString()
                        txtaA4.Text = reader.Item("aA4").ToString()
                        txtaA5.Text = reader.Item("aA5").ToString()

                        'Audit Log
                        SearchAuditLog()
                        UpdateChangeStatus()
                        GlobalVars.LastActivity = TxtGageID.Text + " Searched."
                        Logger.SaveLogEntry()

                        SearchCheck = True
                        saveString = TxtGageID.Text
                    Else
                        MessageBox.Show("GageID does not exist", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "Database error: " & ex.Message
                Logger.LogErrors()
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "An unexpected error occurred: " & ex.Message
                Logger.LogErrors()
            End Try
        End Using
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(TxtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        PauseUserInput(5)

        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()
                Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM [CalibrationTracker] WHERE GageID = ?", conn)
                checkCmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count = 0 Then
                    Dim intervalMonths As Integer = 0
                    If Not Integer.TryParse(TxtInterval.Text, intervalMonths) Then
                        intervalMonths = 0
                    End If

                    Dim inspectedDate As DateTime = DtInspectedDate.Value
                    Dim dueDate As DateTime = inspectedDate.AddMonths(intervalMonths)
                    Dim dateAdded As DateTime = Now

                    Dim addCmd As New OleDbCommand("INSERT INTO [CalibrationTracker] (GageID, PartNumber, PartRev, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments, aN1, aN2, aN3, aN4, aN5, aA1, aA2, aA3, aA4, aA5, [Serial Number], Owner, [Nist Number], [Date Added]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
                    addCmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)
                    addCmd.Parameters.AddWithValue("@PartNumber", txtPartNumber.Text)
                    addCmd.Parameters.AddWithValue("@PartRev", txtPartRev.Text)
                    addCmd.Parameters.AddWithValue("@Status", cmbStatus.Text)
                    addCmd.Parameters.AddWithValue("@Description", txtDescription.Text)
                    addCmd.Parameters.AddWithValue("@Department", txtDepartment.Text)
                    addCmd.Parameters.AddWithValue("@GageType", txtGageType.Text)
                    addCmd.Parameters.AddWithValue("@Customer", txtCustomer.Text)
                    addCmd.Parameters.AddWithValue("@CalibratedBy", txtCalibratedBy.Text)
                    addCmd.Parameters.Add(New OleDbParameter("@Interval", OleDbType.Integer)).Value = intervalMonths
                    addCmd.Parameters.Add(New OleDbParameter("@InspectedDate", OleDbType.Date)).Value = inspectedDate
                    addCmd.Parameters.Add(New OleDbParameter("@DueDate", OleDbType.Date)).Value = dueDate
                    addCmd.Parameters.AddWithValue("@Comments", txtComments.Text)
                    addCmd.Parameters.AddWithValue("@aN1", txtaN1.Text)
                    addCmd.Parameters.AddWithValue("@aN2", txtaN2.Text)
                    addCmd.Parameters.AddWithValue("@aN3", txtaN3.Text)
                    addCmd.Parameters.AddWithValue("@aN4", txtaN4.Text)
                    addCmd.Parameters.AddWithValue("@aN5", txtaN5.Text)
                    addCmd.Parameters.AddWithValue("@aA1", txtaA1.Text)
                    addCmd.Parameters.AddWithValue("@aA2", txtaA2.Text)
                    addCmd.Parameters.AddWithValue("@aA3", txtaA3.Text)
                    addCmd.Parameters.AddWithValue("@aA4", txtaA4.Text)
                    addCmd.Parameters.AddWithValue("@aA5", txtaA5.Text)
                    addCmd.Parameters.AddWithValue("@SerialNumber", TxtSerialNumber.Text)
                    addCmd.Parameters.AddWithValue("@Owner", txtOwner.Text)
                    addCmd.Parameters.AddWithValue("@NistNumber", TxtNistNumber.Text)
                    addCmd.Parameters.Add(New OleDbParameter("@DateAdded", OleDbType.Date)).Value = dateAdded
                    addCmd.ExecuteNonQuery()
                    ShowStatus("Gage added successfully", False)
                    GlobalVars.LastActivity = TxtGageID.Text + " added successfully"
                    Logger.SaveLogEntry()
                    ReloadTimer.Enabled = True
                Else
                    MessageBox.Show("This GageID already exists", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End Using
        Catch ex As OleDbException
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "Database error: {ex.Message}"
            Logger.LogErrors()
        Catch ex As Exception
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "An unexpected error occurred: {ex.Message}"
            Logger.LogErrors()
        End Try
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles BtnUpdate.Click
        If SearchCheck = True Then
            If ChangeDetected = True Then
                If CanUpdate = True Then
                    BtnUpdateConfirmed()
                    PauseUserInput(5)
                Else
                    ShowStatus("Warning GageID has been modified.", True)
                    FixGageID()
                End If
            Else
                    ShowStatus("No updates detected.", True)
            End If
        Else
            ShowStatus("Please search for Gage record.", True)
        End If
    End Sub

    Private Sub BtnUpdateConfirmed()
        Using conn As New OleDbConnection(connectionString)
            conn.Open()

            Dim lastUser As String
            If String.IsNullOrEmpty(My.Settings.LoggedUser) Then
                lastUser = Environment.UserName
            Else
                lastUser = My.Settings.LoggedUser
            End If

            Dim updateCmd As New OleDbCommand("UPDATE [CalibrationTracker] SET PartNumber = ?, PartRev = ?, Status = ?, Description = ?, Department = ?, [Gage Type] = ?, Customer = ?, [Calibrated By] = ?, [Interval (Months)] = ?, [Inspected Date] = ?, [Due Date] = ?, Comments = ?, aN1 = ?, aN2 = ?, aN3 = ?, aN4 = ?, aN5 = ?, aA1 = ?, aA2 = ?, aA3 = ?, aA4 = ?, aA5 = ?, [Serial Number] = ?, Owner = ?, [Nist Number] = ?, [Last User] = ?, [Last Edited] = ? WHERE GageID = ?", conn)
            Dim lastEdited As DateTime = Now

            updateCmd.Parameters.Add(New OleDbParameter("@PartNumber", txtPartNumber.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@PartRev", txtPartRev.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Status", cmbStatus.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Description", txtDescription.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Department", txtDepartment.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@GageType", txtGageType.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Customer", txtCustomer.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@CalibratedBy", txtCalibratedBy.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Interval", TxtInterval.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@InspectedDate", DtInspectedDate.Value))
            updateCmd.Parameters.Add(New OleDbParameter("@DueDate", dtDueDate.Value))
            updateCmd.Parameters.Add(New OleDbParameter("@Comments", txtComments.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aN1", txtaN1.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aN2", txtaN2.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aN3", txtaN3.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aN4", txtaN4.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aN5", txtaN5.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aA1", txtaA1.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aA2", txtaA2.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aA3", txtaA3.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aA4", txtaA4.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@aA5", txtaA5.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@SerialNumber", TxtSerialNumber.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Owner", txtOwner.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@NistNumber", TxtNistNumber.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@LastUser", lastUser))
            updateCmd.Parameters.Add(New OleDbParameter("@LastEdited", OleDbType.Date)).Value = lastEdited
            updateCmd.Parameters.Add(New OleDbParameter("@GageID", TxtGageID.Text))

            Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()
            If rowsAffected > 0 Then
                'Settings
                SearchCheck = False
                GageSearch = TxtGageID.Text

                'Logs
                GlobalVars.LastActivity = TxtGageID.Text + " updated successfully."
                Logger.SaveLogEntry()

                'Subs
                UpdateChangeStatus()
                ShowStatus("Gage updated successfully", False)
                UpdateTimer.Enabled = True

                'Display until restart
                LblLastEdited.Text = lastEdited
                LblEditBy.Text = lastUser
            Else
                MessageBox.Show("No record updated. Please check the GageID.")
            End If
        End Using
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        If ChangeDetected = True Then
            If MessageBox.Show("Changes have been detected. Unsaved changes will be lost. Are you sure you want to proceed with clearing?", "Exit", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                ClearForms()
            Else
                TxtGageID.Text = saveString
            End If
        Else
            ClearForms()
        End If
    End Sub

    Private Sub BtnAdmin_Click(sender As Object, e As EventArgs) Handles BtnAdmin.Click
        If My.Settings.isAdmin = True Then
            StartAdmin()
        Else
            StartLogin()
        End If
    End Sub

    Private Sub BtnGageList_Click(sender As Object, e As EventArgs) Handles BtnGageList.Click
        Dim isOpen As Boolean = False
        Dim openForm As Form = Nothing
        For Each frm As Form In Application.OpenForms
            If TypeOf frm Is GageList Then
                isOpen = True
                openForm = frm
                Exit For
            End If
        Next

        If isOpen AndAlso openForm IsNot Nothing Then
            openForm.Activate()
        Else
            Dim gagelist As New GageList()
            gagelist.Show()
        End If

        Me.Close()
        GageList.LoadData()
    End Sub

    Private Sub BtnReportIssue_Click(sender As Object, e As EventArgs)
        ReportIssue.Show()
        Me.Hide()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If SearchCheck = True Then
            DeleteConfirmed()
        Else
            ShowStatus("Please search for Gage record.", True)
        End If
    End Sub

    Private Sub BtnClearNom_Click(sender As Object, e As EventArgs) Handles BtnClearNom.Click
        txtaN1.Clear()
        txtaN2.Clear()
        txtaN3.Clear()
        txtaN4.Clear()
        txtaN5.Clear()
    End Sub

    Private Sub BtnClearActual_Click(sender As Object, e As EventArgs) Handles BtnClearActual.Click
        txtaA1.Clear()
        txtaA2.Clear()
        txtaA3.Clear()
        txtaA4.Clear()
        txtaA5.Clear()
    End Sub
#End Region

#Region "MenuStrip"
    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub AddGageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddGageToolStripMenuItem.Click
        BtnAdd.PerformClick()
    End Sub

    Private Sub UpdateGageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UpdateGageToolStripMenuItem.Click
        BtnUpdate.PerformClick()
    End Sub

    Private Sub DeleteGageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteGageToolStripMenuItem.Click
        BtnDelete.PerformClick()
    End Sub

    Private Sub AdminToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminToolStripMenuItem.Click
        If My.Settings.isAdmin = True Then
            StartAdmin()
        Else
            StartLogin()
        End If
    End Sub

    Private Sub OverdueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OverdueToolStripMenuItem.Click
        GlobalVars.DueDateMenuSelect = "Past"
        DueDateCategorizer.Show()
        Me.Close()
    End Sub

    Private Sub DueWithin30DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueWithin30DaysToolStripMenuItem.Click
        GlobalVars.DueDateMenuSelect = "30"
        DueDateCategorizer.Show()
        Me.Close()
    End Sub

    Private Sub DueWithin60DaysToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueWithin60DaysToolStripMenuItem.Click
        GlobalVars.DueDateMenuSelect = "60"
        DueDateCategorizer.Show()
        Me.Close()
    End Sub

    Private Sub GageListToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles GageListToolStripMenuItem1.Click
        Dim isOpen As Boolean = False
        Dim openForm As Form = Nothing
        For Each frm As Form In Application.OpenForms
            If TypeOf frm Is GageList Then
                isOpen = True
                openForm = frm
                Exit For
            End If
        Next

        If isOpen AndAlso openForm IsNot Nothing Then
            openForm.Activate()
        Else
            GageList.Show()
        End If

        Me.Close()
        GageList.LoadData()
    End Sub

    Private Sub ReportIssueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReportIssueToolStripMenuItem.Click
        ReportIssue.Show()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        About.Show()
    End Sub

    Private Sub WebsiteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WebsiteToolStripMenuItem.Click
        Dim url As String = "https://alexfare.com/programs/gagetracker/latest/"
        Try
            Process.Start(url)
        Catch ex As Exception
            MessageBox.Show("An error occurred while trying to open the URL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "An error occurred while trying to open the URL: " & ex.Message
            Logger.LogErrors()
        End Try
    End Sub

    Private Sub DashboardToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DashboardToolStripMenuItem.Click
        Dashboard.Show()
    End Sub
#End Region

#Region "Clear"
    Private Sub ClearReset()
        GageSearch = TxtGageID.Text
        ClearForms()
        TxtGageID.Text = GageSearch
        ReloadData()
        BtnSearch.PerformClick()
    End Sub

    Private Sub ClearForms()
        TxtGageID.SelectedIndex = -1
        TxtGageID.Text = ""
        txtPartNumber.Clear()
        txtPartRev.Clear()
        cmbStatus.SelectedIndex = -1
        cmbStatus.Text = ""
        txtDescription.Clear()
        txtDepartment.SelectedIndex = -1
        txtDepartment.Text = ""
        txtGageType.SelectedIndex = -1
        txtGageType.Text = ""
        txtCustomer.SelectedIndex = -1
        txtCustomer.Text = ""
        txtCalibratedBy.SelectedIndex = -1
        txtCalibratedBy.Text = ""
        TxtInterval.Clear()
        txtComments.Clear()
        TxtSerialNumber.Clear()
        txtOwner.Clear()
        TxtNistNumber.Clear()
        DtInspectedDate.Value = DateTime.Now
        dtDueDate.Value = DateTime.Now

        'Clear Measurements
        txtaN1.Clear()
        txtaN2.Clear()
        txtaN3.Clear()
        txtaN4.Clear()
        txtaN5.Clear()
        txtaA1.Clear()
        txtaA2.Clear()
        txtaA3.Clear()
        txtaA4.Clear()
        txtaA5.Clear()

        'Audit Log
        LblDateAdded.Clear()
        LblLastEdited.Clear()
        LblEditBy.Clear()

        SearchCheck = False
        CanUpdate = False
        UpdateChangeStatus()
    End Sub
#End Region

#Region "Misc"
    Private Sub TxtGageID_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtGageID.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True

            If ChangeDetected = False Then
                BtnSearch_Click(Me, EventArgs.Empty)
            End If
        End If
    End Sub

    Private Sub UpdateDueDate()
        Dim intervalMonths As Integer
        If Integer.TryParse(TxtInterval.Text, intervalMonths) Then
            Dim inspectedDate As DateTime = DtInspectedDate.Value
            Dim dueDate As DateTime = inspectedDate.AddMonths(intervalMonths)
            dtDueDate.Value = dueDate
        End If
    End Sub

    Private Sub DeleteConfirmed()
        If My.Settings.isAdmin = False Then
            MessageBox.Show("Must be logged in to delete gage.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this gage?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Try
                Using conn As New OleDbConnection(connectionString)
                    conn.Open()
                    Dim deleteCmd As New OleDbCommand("DELETE FROM [CalibrationTracker] WHERE GageID = ?", conn)
                    deleteCmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)
                    Dim rowsAffected As Integer = deleteCmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        ShowStatus("Gage deleted successfully.", False)
                        GlobalVars.LastActivity = TxtGageID.Text + " deleted."
                        Logger.SaveLogEntry()
                        SearchCheck = False
                        ClearForms()
                        ReloadData()
                    Else
                        ShowStatus("No gage deleted. Please check the GageID.", True)
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "Database error: {ex.Message}"
                Logger.LogErrors()
            Catch ex As Exception
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "An unexpected error occurred: {ex.Message}"
                Logger.LogErrors()
            End Try
        End If
    End Sub

    Private Sub DisableSearchControls()
        TxtGageID.Enabled = False
        BtnSearch.Enabled = False
    End Sub

    Private Sub EnableSearchControls()
        TxtGageID.Enabled = True
        BtnSearch.Enabled = True
    End Sub

    Private Sub SearchAuditLog()
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()
                Dim searchCmd As New OleDbCommand("SELECT [Date Added], [Last Edited], [Last User] FROM [CalibrationTracker] WHERE GageID = ?", conn)
                searchCmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)

                Using reader As OleDbDataReader = searchCmd.ExecuteReader()
                    If reader.HasRows Then
                        reader.Read()
                        LblDateAdded.Text = If(IsDBNull(reader("Date Added")), String.Empty, reader("Date Added").ToString())
                        LblLastEdited.Text = If(IsDBNull(reader("Last Edited")), String.Empty, reader("Last Edited").ToString())
                        LblEditBy.Text = If(IsDBNull(reader("Last User")), String.Empty, reader("Last User").ToString())
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "An error occurred: " & ex.Message
            Logger.LogErrors()
        End Try
    End Sub

    Private Sub StartLogin()
        UpdateChangeStatus()
        LoginForm1.Show()
        Me.Close()
        My.Settings.FromList = False
    End Sub

    Private Sub StartAdmin()
        UpdateChangeStatus()
        AdminMenu.Show()
        Me.Close()
        My.Settings.FromList = False
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""
        Timer1.Enabled = False
    End Sub

    Private Sub UpdateTimer_Tick(sender As Object, e As EventArgs) Handles UpdateTimer.Tick
        UpdateTimer.Enabled = False
        ClearReset()
    End Sub

    Private Sub ReloadTimer_Tick(sender As Object, e As EventArgs) Handles ReloadTimer.Tick
        ReloadTimer.Enabled = False
        ReloadData()
    End Sub

    Private Sub UpdateChangeStatus()
        ChangeDetected = False
        Me.Text = originalTitle
    End Sub

    Private Sub ShowStatus(message As String, isError As Boolean)
        StatusLabel.ForeColor = If(isError, Color.Red, Color.Green)
        StatusLabel.Text = message
        Timer1.Enabled = True
    End Sub

    Private Sub FixGageID()
        TxtGageID.Text = saveString
        CanUpdate = True
    End Sub

    Private Async Sub PauseUserInput(ByVal durationInSeconds As Integer)
        Me.Enabled = False
        Await Task.Delay(durationInSeconds * 1000)
        Me.Enabled = True
    End Sub
#End Region

#Region "TextChanged"
    Private Sub UpdateDueDate_TextChanged(sender As Object, e As EventArgs) Handles TxtInterval.TextChanged, DtInspectedDate.ValueChanged
        UpdateDueDate()
    End Sub

    Private Sub TextContains_TextChanged(sender As Object, e As EventArgs) Handles txtPartNumber.TextChanged, txtDescription.TextChanged, TxtInterval.TextChanged, txtComments.TextChanged, cmbStatus.SelectedIndexChanged, txtDepartment.SelectedIndexChanged, txtGageType.SelectedIndexChanged, txtCustomer.SelectedIndexChanged, txtCalibratedBy.SelectedIndexChanged, DtInspectedDate.ValueChanged, dtDueDate.ValueChanged, TxtSerialNumber.TextChanged, TxtNistNumber.TextChanged, txtOwner.TextChanged, txtaN1.TextChanged, txtaN2.TextChanged, txtaN3.TextChanged, txtaN4.TextChanged, txtaN5.TextChanged, txtaA1.TextChanged, txtaA2.TextChanged, txtaA3.TextChanged, txtaA4.TextChanged, txtaA5.TextChanged, txtPartRev.TextChanged
        If SearchCheck = True Then
            ChangeDetected = True
            Me.Text = "*" & originalTitle
        Else
            UpdateChangeStatus()
        End If
    End Sub
    Private Sub SearchChange_TextChanged(sender As Object, e As EventArgs) Handles TxtGageID.TextChanged
        CanUpdate = False
    End Sub
#End Region

#Region "Closing Form"
    Private Sub GTMenu_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If ChangeDetected = True Then
            If isClosing Then
                Return
            End If

            If MessageBox.Show("Changes have not been saved. Any unsaved changes will be lost. Do you want to exit without saving?", "Exit", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                isClosing = True
                Me.Close()
            Else
                e.Cancel = True
            End If
        End If
    End Sub
#End Region
End Class
