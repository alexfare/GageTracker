Imports System.Data.OleDb
Imports System.Net

Public Class AdminMenu
    Dim connectionString As String
    Dim SearchCheck As Boolean
    Dim ChangeDetected As Boolean
    Dim GageSearch As String
    Private isClosing As Boolean = False
    Dim originalTitle As String = "GageTracker - Admin Menu"
    Public WithEvents Timer1 As New Timer With {.Interval = 3000, .Enabled = False}

#Region "Admin Load"
    Private Async Sub AdminMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        StatusLabel.Text = ""
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        Me.Text = originalTitle
        MenuStrip1.BackColor = Color.IndianRed

        SearchCheck = False

        DisableSearchControls() 'Disable search controls
        Await Task.WhenAll(Task.Run(Sub() LoadGageID()),
                           Task.Run(Sub() LoadStatus()),
                           Task.Run(Sub() LoadDepartment()),
                           Task.Run(Sub() LoadGageType()),
                           Task.Run(Sub() LoadUser()),
                           Task.Run(Sub() LoadCustomers()))
        EnableSearchControls() 'Enable search controls

        TxtGageID.Focus()

        If Not String.IsNullOrEmpty(My.Settings.SelectedGage) Then
            TxtGageID.SelectedItem = My.Settings.SelectedGage
            BtnAdminSearch_Click(Me, EventArgs.Empty)
        End If

        'Audit Log
        TxtCurrentUser.Text = My.Settings.LoggedUser
        TxtLastActivity.Text = GlobalVars.LastActivity
        TxtLastOpened.Text = My.Settings.LastOpened
        TxtOpenCount.Text = My.Settings.ProgramOpenCount
    End Sub

    Private Sub ReloadData()
        LoadGageID()
        GageList.LoadData()
        DueDateCategorizer.LoadData()
        GageList.ApplyStatusFilter()
    End Sub

    Private Sub TxtAdminGageID_KeyDown(sender As Object, e As KeyEventArgs) Handles TxtGageID.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True 'Prevent the ding sound on pressing Enter

            If ChangeDetected = False Then
                BtnAdminSearch_Click(Me, EventArgs.Empty)
            End If
        End If
    End Sub
#End Region

#Region "Buttons"
    Private Sub BtnAdminSearch_Click(sender As Object, e As EventArgs) Handles BtnAdminSearch.Click
        If String.IsNullOrWhiteSpace(TxtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT PartNumber, PartRev, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments, aN1, aN2, aN3, aN4, aN5, aA1, aA2, aA3, aA4, aA5, [Serial Number], Owner, [Nist Number] FROM [CalibrationTracker] WHERE GageID = ?", conn)

                cmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtPartNumber.Text = reader.Item("PartNumber").ToString()
                        txtPartRev.Text = reader.Item("PartRev").ToString()

                        Dim statusIndex As Integer = cmbStatus.FindStringExact(reader.Item("Status").ToString())
                        If statusIndex >= 0 Then
                            cmbStatus.SelectedIndex = statusIndex
                        Else
                            cmbStatus.SelectedIndex = -1 'No matching status found
                        End If
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

                        SearchCheck = True
                        UpdateChangeStatus()
                        SearchAuditLog()
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

        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()
                Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM [CalibrationTracker] WHERE GageID = ?", conn)
                checkCmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count = 0 Then
                    Dim intervalMonths As Integer = 0
                    If Not Integer.TryParse(TxtInterval.Text, intervalMonths) Then
                        intervalMonths = 0 'Default to 0 if not a valid integer
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
                    StatusLabel.Text = "Gage added successfully"
                    Timer1.Enabled = True
                    ReloadData()
                    BtnClear.PerformClick()
                Else
                    MessageBox.Show("This GageID already exists", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End Using
        Catch ex As OleDbException
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "Database error: " & ex.Message
            Logger.LogErrors()
        Catch ex As Exception
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "An unexpected error occurred: {ex.Message}"
            Logger.LogErrors()
        End Try
    End Sub

    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles BtnUpdate.Click
        If SearchCheck = True Then
            BtnUpdateConfirmed()
        Else
            MessageBox.Show("Please search for Gage record.")
        End If
    End Sub

    Private Sub BtnUpdateConfirmed() 'Handles BtnUpdate.Click
        Using conn As New OleDbConnection(connectionString)
            conn.Open()

            Dim lastUser As String
            If String.IsNullOrEmpty(My.Settings.LoggedUser) Then
                lastUser = Environment.UserName 'Use the current logged-in computer user
            Else
                lastUser = My.Settings.LoggedUser
            End If

            Dim updateCmd As New OleDbCommand("UPDATE [CalibrationTracker] SET PartNumber = ?, PartRev = ?, Status = ?, Description = ?, Department = ?, [Gage Type] = ?, Customer = ?, [Calibrated By] = ?, [Interval (Months)] = ?, [Inspected Date] = ?, [Due Date] = ?, Comments = ?, aN1 = ?, aN2 = ?, aN3 = ?, aN4 = ?, aN5 = ?, aA1 = ?, aA2 = ?, aA3 = ?, aA4 = ?, aA5 = ?, [Serial Number] = ?, Owner = ?, [Nist Number] = ? WHERE GageID = ?", conn)
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
            updateCmd.Parameters.Add(New OleDbParameter("@GageID", TxtGageID.Text))

            'Execute the UPDATE command
            Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()
            If rowsAffected > 0 Then
                'Settings
                SearchCheck = False
                GageSearch = TxtGageID.Text

                'Status
                StatusLabel.Text = "Record updated successfully"
                Timer1.Enabled = True

                'Subs
                UpdateChangeStatus()
                ReloadData()
                ClearReset()

                'Display until restart
                LblLastEdited.Text = lastEdited
                LblEditBy.Text = lastUser
            Else
                MessageBox.Show("No record updated. Please check the GageID.")
            End If
        End Using
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        If SearchCheck = True Then
            DeleteConfirmed()
        Else
            MessageBox.Show("Please search for Gage record.")
        End If
    End Sub

    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        If My.Settings.FromList = True Then
            My.Settings.FromList = False
            Me.Close()
        Else
            GTMenu.Show()
            Me.Close()
        End If
    End Sub

    Private Sub btnCustomer_Click(sender As Object, e As EventArgs) Handles btnCustomer.Click
        CustomerEntry.Show()
    End Sub

    Private Sub btnStatus_Click(sender As Object, e As EventArgs) Handles btnStatus.Click
        StatusMenu.Show()
    End Sub

    Private Sub btnAccount_Click(sender As Object, e As EventArgs) Handles btnAccount.Click
        AccountManagement.Show()
    End Sub

    Private Sub BtnGageType_Click(sender As Object, e As EventArgs) Handles BtnGageType.Click
        GageType.Show()
    End Sub
#End Region

#Region "Controls"
    Private Sub DisableSearchControls()
        TxtGageID.Enabled = False
        BtnAdminSearch.Enabled = False
    End Sub

    Private Sub EnableSearchControls()
        TxtGageID.Enabled = True
        BtnAdminSearch.Enabled = True
    End Sub

    Private Sub DeleteConfirmed()
        If String.IsNullOrWhiteSpace(TxtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this gage?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Try
                Using conn As New OleDbConnection(connectionString)
                    conn.Open()

                    'Execute the DELETE query
                    Dim deleteCmd As New OleDbCommand("DELETE FROM [CalibrationTracker] WHERE GageID = ?", conn)
                    deleteCmd.Parameters.AddWithValue("@GageID", TxtGageID.Text)
                    Dim rowsAffected As Integer = deleteCmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        StatusLabel.Text = "Gage deleted successfully."
                        Timer1.Enabled = True
                        SearchCheck = False
                        ClearForms()
                        ReloadData()
                        GlobalVars.LastActivity = TxtGageID.Text + " deleted."
                    Else
                        MessageBox.Show("No gage deleted. Please check the GageID.")
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "Database error: " & ex.Message
                Logger.LogErrors()
            Catch ex As Exception
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "An unexpected error occurred: {ex.Message}"
                Logger.LogErrors()
            End Try
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StatusLabel.Text = ""  'Clear the text
        Timer1.Enabled = False  'Stop the timer
    End Sub

    Private Sub TextContains_TextChanged(sender As Object, e As EventArgs) Handles txtPartNumber.TextChanged, txtDescription.TextChanged, TxtInterval.TextChanged, txtComments.TextChanged, cmbStatus.SelectedIndexChanged, txtDepartment.SelectedIndexChanged, txtGageType.SelectedIndexChanged, txtCustomer.SelectedIndexChanged, txtCalibratedBy.SelectedIndexChanged, DtInspectedDate.ValueChanged, dtDueDate.ValueChanged
        If SearchCheck = True Then
            ChangeDetected = True
            Me.Text = "*" & originalTitle
        Else
            UpdateChangeStatus()
        End If
    End Sub

    Private Sub UpdateChangeStatus()
        ChangeDetected = False
        Me.Text = originalTitle
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
#End Region

#Region "Clear"
    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        ClearForms()
    End Sub

    Private Sub ClearReset()
        GageSearch = TxtGageID.Text
        ClearForms()
        TxtGageID.Text = GageSearch
        ReloadData()
        BtnAdminSearch.PerformClick()
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
        UpdateChangeStatus()
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

#Region "Menu Strip"
    Private Sub LogoutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles LogoutToolStripMenuItem.Click
        My.Settings.isAdmin = False
        GageList.MenuColor()

        If My.Settings.FromList = True Then
            My.Settings.FromList = False
            Me.Close()
        Else
            GTMenu.Show()
            Me.Close()
        End If
    End Sub

    Private Sub CloseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CloseToolStripMenuItem.Click
        If My.Settings.FromList = True Then
            My.Settings.FromList = False
            Me.Close()
        Else
            GTMenu.Show()
            Me.Close()
        End If
    End Sub

    Private Sub MenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MenuToolStripMenuItem.Click
        GTMenu.Show()
        Me.Close()
    End Sub

    Private Sub GageListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GageListToolStripMenuItem.Click
        My.Settings.FromList = False
        Me.Close()
    End Sub

    Private Sub DueDateCalenderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DueDateCalenderToolStripMenuItem.Click
        Me.Close()
        DueDateCategorizer.Show()
    End Sub
#End Region

#Region "Load"
    Public Sub LoadGageID()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageID FROM [CalibrationTracker]", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold GageID data

                While reader.Read()
                    items.Add(reader("GageID").ToString())
                End While

                'Close the reader and connection
                reader.Close()

                'Now invoke the UI thread to update the ComboBox
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
            Finally
                'Use connection is closed if exception occurs
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

                'Use invoke the UI thread to update the ComboBox
                Me.Invoke(Sub()
                              cmbStatus.Items.Clear()
                              For Each item As String In items
                                  cmbStatus.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading status options: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading status options: " & ex.Message
                Logger.LogErrors()
            Finally
                'Ensure connection is closed if exception occurs
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
                Dim cmd As New OleDbCommand("SELECT Departments FROM [Departments]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Department data

                While reader.Read()
                    items.Add(reader("Departments").ToString())
                End While

                'Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtDepartment.Items.Clear()
                              For Each item As String In items
                                  txtDepartment.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Departments options: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading Departments options: " & ex.Message
                Logger.LogErrors()
            Finally
                'Ensure connection is closed if exception occurs
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
                Dim cmd As New OleDbCommand("SELECT GageType FROM [GageType]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Gage Type data

                While reader.Read()
                    items.Add(reader("GageType").ToString())
                End While

                'Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtGageType.Items.Clear()
                              For Each item As String In items
                                  txtGageType.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage Type options: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading Gage Type options: " & ex.Message
                Logger.LogErrors()
            Finally
                'Ensure connection is closed if exception occurs
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
                Dim cmd As New OleDbCommand("SELECT CustomerName FROM Customers", conn) ' Make sure the table name is correct
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Customer data

                While reader.Read()
                    items.Add(reader("CustomerName").ToString())
                End While

                'Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtCustomer.Items.Clear()
                              For Each item As String In items
                                  txtCustomer.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Customer options: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading Customer options: " & ex.Message
                Logger.LogErrors()
            Finally
                'Ensure connection is closed if exception occurs
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
                Dim cmd As New OleDbCommand("SELECT Username FROM [Credentials]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Gage Type data

                While reader.Read()
                    items.Add(reader("Username").ToString())
                End While

                'Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtCalibratedBy.Items.Clear()
                              For Each item As String In items
                                  txtCalibratedBy.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage Type options: " & ex.Message)
                GlobalVars.ErrorLog = "An error occurred while loading Gage Type options: " & ex.Message
                Logger.LogErrors()
            Finally
                'Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub
#End Region

#Region "Database"
    Private Sub OpenDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenDatabaseToolStripMenuItem.Click
        Dim fullPath As String = My.Settings.DatabaseLocation

        If Not String.IsNullOrEmpty(fullPath) Then
            If IO.File.Exists(fullPath) Then
                Try
                    Process.Start("msaccess.exe", """" & fullPath & """")
                Catch ex As Exception
                    MessageBox.Show("Failed to open the database in Access: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    GlobalVars.ErrorLog = "Failed to open the database in Access: " & ex.Message
                    Logger.LogErrors()
                End Try
            Else
                MessageBox.Show("Database file does not exist: " & fullPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "Database file does not exist: " & fullPath
                Logger.LogErrors()
            End If
        Else
            MessageBox.Show("Database location is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "Database location is not set."
            Logger.LogErrors()
        End If
    End Sub

    Private Sub DatabasePathToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DatabasePathToolStripMenuItem.Click
        Dim fullPath As String = My.Settings.DatabaseLocation

        If Not String.IsNullOrEmpty(fullPath) Then
            Dim directoryPath As String = IO.Path.GetDirectoryName(fullPath)

            If IO.Directory.Exists(directoryPath) Then
                Process.Start("explorer.exe", directoryPath)
            Else
                MessageBox.Show("Directory does not exist: " & directoryPath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                GlobalVars.ErrorLog = "Directory does not exist: " & directoryPath
                Logger.LogErrors()
            End If
        Else
            MessageBox.Show("Database location is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "Database location is not set."
            Logger.LogErrors()
        End If
    End Sub

    Private Sub NewDatabaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewDatabaseToolStripMenuItem.Click
        Dim saveDialog As New SaveFileDialog()
        saveDialog.Filter = "Access Database Files (*.accdb)|*.accdb"
        saveDialog.Title = "Save Database File"
        saveDialog.FileName = "GTDatabase.accdb"

        If saveDialog.ShowDialog() = DialogResult.OK Then
            DownloadDatabase(saveDialog.FileName)
        End If
    End Sub

    Private Sub DownloadDatabase(savePath As String)
        Dim webClient As New WebClient()

        Try
            Dim downloadUrl As String = "https://alexfare.com/programs/gtdatabase/latest/GTDatabase.accdb"
            webClient.DownloadFile(downloadUrl, savePath)
            MessageBox.Show("Database downloaded complete.", "Download Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            GlobalVars.SystemLog = "New database downloaded."
            Logger.LogSystem()

            GlobalVars.DatabaseLocation = savePath
            GlobalVars.SaveDatabaseLocation(savePath)
        Catch ex As Exception
            MessageBox.Show("An error occurred while downloading the database: " & ex.Message, "Download Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            GlobalVars.ErrorLog = "An error occurred while downloading the database: " & ex.Message
            Logger.LogErrors()
        Finally
            webClient.Dispose()
        End Try
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