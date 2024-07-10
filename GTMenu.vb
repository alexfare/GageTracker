Imports System.Data.OleDb
Imports System.Threading.Tasks

Public Class GTMenu
    Dim connectionString As String
    Dim SearchCheck As Boolean

    Private Async Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"

        ' Disable search controls
        DisableSearchControls()

        ' Load data
        Await Task.WhenAll(Task.Run(Sub() LoadGageID()),
                           Task.Run(Sub() LoadStatus()),
                           Task.Run(Sub() LoadDepartment()),
                           Task.Run(Sub() LoadGageType()),
                           Task.Run(Sub() LoadUser()),
                           Task.Run(Sub() LoadCustomers()))

        ' Enable search controls
        EnableSearchControls()

        txtGageID.Focus()
        SearchCheck = False
        GlobalVars.UserActive = False
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Private Sub txtGageID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtGageID.KeyDown
        If e.KeyCode = Keys.Enter Then
            ' Prevent the ding sound on pressing Enter
            e.SuppressKeyPress = True

            ' Call the BtnSearch click event handler
            BtnSearch_Click(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(txtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()
                Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM [CalibrationTracker] WHERE GageID = ?", conn)
                checkCmd.Parameters.AddWithValue("@GageID", txtGageID.Text)
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count = 0 Then
                    Dim intervalMonths As Integer = 0
                    If Not Integer.TryParse(txtInterval.Text, intervalMonths) Then
                        intervalMonths = 0 ' Default to 0 if not a valid integer
                    End If

                    Dim inspectedDate As DateTime = dtInspectedDate.Value
                    Dim dueDate As DateTime = inspectedDate.AddMonths(intervalMonths)

                    Dim addCmd As New OleDbCommand("INSERT INTO [CalibrationTracker] (GageID, PartNumber, PartRev, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments, aN1, aN2, aN3, aN4, aN5, aA1, aA2, aA3, aA4, aA5, [Serial Number], Owner, [Nist Number]) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
                    addCmd.Parameters.AddWithValue("@GageID", txtGageID.Text)
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
                    addCmd.ExecuteNonQuery()
                    MessageBox.Show("Gage added successfully")
                    LoadGageID()
                    GageList.LoadData()
                Else
                    MessageBox.Show("This GageID already exists", "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End Using
        Catch ex As OleDbException
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        ' Check if the GageID text box is empty
        If String.IsNullOrWhiteSpace(txtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT PartNumber, PartRev, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments, aN1, aN2, aN3, aN4, aN5, aA1, aA2, aA3, aA4, aA5, [Serial Number], Owner, [Nist Number] FROM [CalibrationTracker] WHERE GageID = ?", conn)
                ' Use parameterized queries to prevent SQL Injection
                cmd.Parameters.AddWithValue("@GageID", txtGageID.Text)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtPartNumber.Text = reader.Item("PartNumber").ToString()
                        txtPartRev.Text = reader.Item("PartRev").ToString()
                        ' For the cmbStatus, find the correct status in the items and set it as selected
                        Dim statusIndex As Integer = cmbStatus.FindStringExact(reader.Item("Status").ToString())
                        If statusIndex >= 0 Then
                            cmbStatus.SelectedIndex = statusIndex
                        Else
                            cmbStatus.SelectedIndex = -1 ' No matching status found
                        End If
                        txtDescription.Text = reader.Item("Description").ToString()
                        txtDepartment.Text = reader.Item("Department").ToString()
                        txtGageType.Text = reader.Item("Gage Type").ToString()
                        txtCustomer.Text = reader.Item("Customer").ToString()
                        txtCalibratedBy.Text = reader.Item("Calibrated By").ToString()
                        txtInterval.Text = reader.Item("Interval (Months)").ToString()

                        If Not IsDBNull(reader.Item("Inspected Date")) Then
                            dtInspectedDate.Value = DateTime.Parse(reader.Item("Inspected Date").ToString())
                        End If
                        If Not IsDBNull(reader.Item("Due Date")) Then
                            dtDueDate.Value = DateTime.Parse(reader.Item("Due Date").ToString())
                        End If

                        txtComments.Text = reader.Item("Comments").ToString()
                        TxtSerialNumber.Text = reader.Item("Serial Number").ToString()
                        txtOwner.Text = reader.Item("Owner").ToString()
                        TxtNistNumber.Text = reader.Item("Nist Number").ToString()

                        ' Measurements
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

                        SearchCheck = True
                    Else
                        MessageBox.Show("GageID does not exist", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
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
            ' Prepare your UPDATE SQL command with parameters to prevent SQL injection
            Dim updateCmd As New OleDbCommand("UPDATE [CalibrationTracker] SET PartNumber = ?, PartRev = ?, Status = ?, Description = ?, Department = ?, [Gage Type] = ?, Customer = ?, [Calibrated By] = ?, [Interval (Months)] = ?, [Inspected Date] = ?, [Due Date] = ?, Comments = ?, aN1 = ?, aN2 = ?, aN3 = ?, aN4 = ?, aN5 = ?, aA1 = ?, aA2 = ?, aA3 = ?, aA4 = ?, aA5 = ?, [Serial Number] = ?, Owner = ?, [Nist Number] = ? WHERE GageID = ?", conn)

            ' Add parameters with the values from your form controls
            updateCmd.Parameters.Add(New OleDbParameter("@PartNumber", txtPartNumber.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@PartRev", txtPartRev.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Status", cmbStatus.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Description", txtDescription.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Department", txtDepartment.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@GageType", txtGageType.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Customer", txtCustomer.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@CalibratedBy", txtCalibratedBy.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@Interval", txtInterval.Text))
            updateCmd.Parameters.Add(New OleDbParameter("@InspectedDate", dtInspectedDate.Value))
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
            updateCmd.Parameters.Add(New OleDbParameter("@GageID", txtGageID.Text))

            ' Execute the UPDATE command
            Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()
            If rowsAffected > 0 Then
                MessageBox.Show("Record updated successfully")
                'SearchCheck = False
                GageList.LoadData()
                DueDateCategorizer.LoadData()
            Else
                MessageBox.Show("No record updated. Please check the GageID.")
            End If
        End Using
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles BtnClear.Click
        ClearForms()
    End Sub

    Private Sub txtInterval_TextChanged(sender As Object, e As EventArgs) Handles txtInterval.TextChanged
        UpdateDueDate()
    End Sub

    Private Sub dtInspectedDate_ValueChanged(sender As Object, e As EventArgs) Handles dtInspectedDate.ValueChanged
        UpdateDueDate()
    End Sub

    Private Sub UpdateDueDate()
        Dim intervalMonths As Integer
        If Integer.TryParse(txtInterval.Text, intervalMonths) Then
            ' If the interval is a valid integer, calculate and update the due date
            Dim inspectedDate As DateTime = dtInspectedDate.Value
            Dim dueDate As DateTime = inspectedDate.AddMonths(intervalMonths)
            dtDueDate.Value = dueDate
        End If
    End Sub

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

                ' Close the reader and connection
                reader.Close()

                ' Now invoke the UI thread to update the ComboBox
                Me.Invoke(Sub()
                              txtGageID.Items.Clear()
                              txtGageID.AutoCompleteMode = AutoCompleteMode.SuggestAppend
                              txtGageID.AutoCompleteSource = AutoCompleteSource.ListItems

                              For Each item As String In items
                                  txtGageID.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading GageID options: " & ex.Message)
            Finally
                ' Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub LoadStatus()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Status FROM [Status]", conn) ' Ensure the table name and column name are correct
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold status data

                While reader.Read()
                    items.Add(reader("Status").ToString())
                End While

                ' Now invoke the UI thread to update the ComboBox
                Me.Invoke(Sub()
                              cmbStatus.Items.Clear()
                              For Each item As String In items
                                  cmbStatus.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading status options: " & ex.Message)
            Finally
                ' Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub LoadDepartment()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Departments FROM [Departments]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Department data

                While reader.Read()
                    items.Add(reader("Departments").ToString())
                End While

                ' Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtDepartment.Items.Clear()
                              For Each item As String In items
                                  txtDepartment.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Departments options: " & ex.Message)
            Finally
                ' Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub LoadGageType()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageType FROM [GageType]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Gage Type data

                While reader.Read()
                    items.Add(reader("GageType").ToString())
                End While

                ' Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtGageType.Items.Clear()
                              For Each item As String In items
                                  txtGageType.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage Type options: " & ex.Message)
            Finally
                ' Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub LoadCustomers()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT CustomerName FROM Customers", conn) ' Make sure the table name is correct
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Customer data

                While reader.Read()
                    items.Add(reader("CustomerName").ToString())
                End While

                ' Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtCustomer.Items.Clear()
                              For Each item As String In items
                                  txtCustomer.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Customer options: " & ex.Message)
            Finally
                ' Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub LoadUser()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Username FROM [Credentials]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                Dim items As New List(Of String)() ' Temporary list to hold Gage Type data

                While reader.Read()
                    items.Add(reader("Username").ToString())
                End While

                ' Use Invoke to update the ComboBox on the UI thread
                Me.Invoke(Sub()
                              txtCalibratedBy.Items.Clear()
                              For Each item As String In items
                                  txtCalibratedBy.Items.Add(item)
                              Next
                          End Sub)

            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Gage Type options: " & ex.Message)
            Finally
                ' Ensure connection is closed if exception occurs
                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If
            End Try
        End Using
    End Sub

    Private Sub BtnAdmin_Click(sender As Object, e As EventArgs) Handles BtnAdmin.Click
        If GlobalVars.UserActive = True Then
            Dim adminMenu As New AdminMenu()
            adminMenu.Show()
            Me.Hide()

        Else
            Dim loginForm As New LoginForm1()
            loginForm.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub BtnGageList_Click(sender As Object, e As EventArgs) Handles BtnGageList.Click
        ' Check if GageList is already open
        Dim isOpen As Boolean = False
        Dim openForm As Form = Nothing ' To hold the already open GageList form
        For Each frm As Form In Application.OpenForms
            If TypeOf frm Is GageList Then
                isOpen = True
                openForm = frm ' Store the reference to the open GageList form
                Exit For
            End If
        Next

        If isOpen AndAlso openForm IsNot Nothing Then
            ' Bring the already open GageList form to the front
            openForm.Activate()
        Else
            ' Only open a new instance if it is not already open
            Dim gagelist As New GageList()
            gagelist.Show()
        End If

        ' Hide the current GTMenu form in both cases
        Me.Hide()
        GageList.LoadData()
    End Sub

    Private Sub btnReportIssue_Click(sender As Object, e As EventArgs) Handles btnReportIssue.Click
        ReportIssue.Show()
        Me.Hide()
    End Sub

    Private Sub BtnDelete_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        ' Check if the GageID text box is empty
        If String.IsNullOrWhiteSpace(txtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If GlobalVars.UserActive = False Then
            MessageBox.Show("Must be logged in to delete gage.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Confirm with the user before deleting the record
        Dim result As DialogResult = MessageBox.Show("Are you sure you want to delete this gage?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Try
                Using conn As New OleDbConnection(connectionString)
                    conn.Open()
                    ' Execute the DELETE query
                    Dim deleteCmd As New OleDbCommand("DELETE FROM [CalibrationTracker] WHERE GageID = ?", conn)
                    deleteCmd.Parameters.AddWithValue("@GageID", txtGageID.Text)
                    Dim rowsAffected As Integer = deleteCmd.ExecuteNonQuery()
                    If rowsAffected > 0 Then
                        MessageBox.Show("Gage deleted successfully.")
                        ' Clear the form fields after deletion
                        ClearForms()
                        LoadGageID()
                    Else
                        MessageBox.Show("No gage deleted. Please check the GageID.")
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub ClearForms()
        txtGageID.SelectedIndex = -1 ' Reset the ComboBox selection
        txtGageID.Text = ""
        txtPartNumber.Clear()
        txtPartRev.Clear()
        cmbStatus.SelectedIndex = -1 ' Reset the ComboBox selection
        cmbStatus.Text = ""
        txtDescription.Clear()
        txtDepartment.SelectedIndex = -1 ' Reset the ComboBox selection
        txtDepartment.Text = ""
        txtGageType.SelectedIndex = -1 ' Reset the ComboBox selection
        txtGageType.Text = ""
        txtCustomer.SelectedIndex = -1 ' Reset the ComboBox selection
        txtCustomer.Text = ""
        txtCalibratedBy.SelectedIndex = -1 ' Reset the ComboBox selection
        txtCalibratedBy.Text = ""
        txtInterval.Clear()
        txtComments.Clear()
        TxtSerialNumber.Clear()
        txtOwner.Clear()
        TxtNistNumber.Clear()
        dtInspectedDate.Value = DateTime.Now ' Reset to current date or some default
        dtDueDate.Value = DateTime.Now ' Reset to current date or some default

        ' Clear Measurements
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

        ' Audit Log
        LblDateAdded.Clear()
        LblLastSearched.Clear()
        LblLastEdited.Clear()
        LblEditBy.Clear()

        SearchCheck = False
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub GageListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GageListToolStripMenuItem.Click
        ' Check if GageList is already open
        Dim isOpen As Boolean = False
        Dim openForm As Form = Nothing ' To hold the already open GageList form
        For Each frm As Form In Application.OpenForms
            If TypeOf frm Is GageList Then
                isOpen = True
                openForm = frm ' Store the reference to the open GageList form
                Exit For
            End If
        Next

        If isOpen AndAlso openForm IsNot Nothing Then
            ' Bring the already open GageList form to the front
            openForm.Activate()
        Else
            ' Only open a new instance if it is not already open
            Dim gagelist As New GageList()
            gagelist.Show()
        End If

        ' Hide the current GTMenu form in both cases
        Me.Hide()
        GageList.LoadData()
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

    Private Sub AdminMenuToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdminMenuToolStripMenuItem.Click
        If GlobalVars.UserActive = True Then
            Dim adminMenu As New AdminMenu()
            adminMenu.Show()
            Me.Hide()

        Else
            Dim loginForm As New LoginForm1()
            loginForm.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub DisableSearchControls()
        txtGageID.Enabled = False
        BtnSearch.Enabled = False
    End Sub

    Private Sub EnableSearchControls()
        txtGageID.Enabled = True
        BtnSearch.Enabled = True
    End Sub

    Private Sub SearchAuditLog()
        Try
            Using conn As New OleDbConnection(connectionString)
                conn.Open()
                Dim searchCmd As New OleDbCommand("SELECT [Date Added], [Last Edited], [Last User] FROM [CalibrationTracker] WHERE GageID = ?", conn)
                searchCmd.Parameters.AddWithValue("@GageID", txtGageID.Text)

                Using reader As OleDbDataReader = searchCmd.ExecuteReader()
                    If reader.HasRows Then
                        reader.Read()
                        LblDateAdded.Text = If(IsDBNull(reader("Date Added")), String.Empty, reader("Date Added").ToString())
                        LblLastEdited.Text = If(IsDBNull(reader("Last Edited")), String.Empty, reader("Last Edited").ToString())
                        LblEditBy.Text = If(IsDBNull(reader("Last User")), String.Empty, reader("Last User").ToString())
                    Else
                        MessageBox.Show("No records found for the given GageID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class
