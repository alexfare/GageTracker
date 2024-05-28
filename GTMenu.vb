Imports System.Data.OleDb

Public Class GTMenu
    Dim ConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=R:\Quality\GageCalibration\GTDatabase.accdb;"
    Dim SearchCheck As Boolean

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadGageIDOptions()
        LoadStatusOptions()
        LoadDepartmentOptions()
        LoadGageTypeOptions()
        txtGageID.Focus()
        SearchCheck = False
        GlobalVars.UserActive = False
    End Sub
    Private Sub txtGageID_KeyDown(sender As Object, e As KeyEventArgs) 
        If e.KeyCode = Keys.Enter Then
            ' Prevent the ding sound on pressing Enter
            e.SuppressKeyPress = True

            ' Call the BtnSearch click event handler
            BtnSearch_Click(Me, EventArgs.Empty)
        End If
    End Sub
    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        ' Check if the GageID text box is empty
        If String.IsNullOrWhiteSpace(txtGageID.Text) Then
            MessageBox.Show("GageID cannot be blank. Please enter a valid GageID.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            Using conn As New OleDbConnection(ConnectionString)
                conn.Open()
                ' Check if the GageID already exists
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

                    Dim addCmd As New OleDbCommand("INSERT INTO [CalibrationTracker] (GageID, PartNumber, PartRev, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
                    ' Prepare and add parameters
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
                    addCmd.ExecuteNonQuery()
                    MessageBox.Show("Record added successfully")
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

        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT PartNumber, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments FROM [CalibrationTracker] WHERE GageID = ?", conn)
                ' Use parameterized queries to prevent SQL Injection
                cmd.Parameters.AddWithValue("@GageID", txtGageID.Text)

                Using reader As OleDbDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtPartNumber.Text = reader.Item("PartNumber").ToString()
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
        Using conn As New OleDbConnection(ConnectionString)
            conn.Open()
            ' Prepare your UPDATE SQL command with parameters to prevent SQL injection
            Dim updateCmd As New OleDbCommand("UPDATE [CalibrationTracker] SET PartNumber = ?, Status = ?, Description = ?, Department = ?, [Gage Type] = ?, Customer = ?, [Calibrated By] = ?, [Interval (Months)] = ?, [Inspected Date] = ?, [Due Date] = ?, Comments = ? WHERE GageID = ?", conn)

            ' Add parameters with the values from your form controls
            updateCmd.Parameters.Add(New OleDbParameter("@PartNumber", txtPartNumber.Text))
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
            updateCmd.Parameters.Add(New OleDbParameter("@GageID", txtGageID.Text))

            ' Execute the UPDATE command
            Dim rowsAffected As Integer = updateCmd.ExecuteNonQuery()
            If rowsAffected > 0 Then
                MessageBox.Show("Record updated successfully")
                SearchCheck = False
            Else
                MessageBox.Show("No record updated. Please check the GageID.")
            End If
        End Using
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' Clear all input fields
        txtGageID.SelectedIndex = -1 ' Reset the ComboBox selection
        txtPartNumber.Clear()
        cmbStatus.SelectedIndex = -1 ' Reset the ComboBox selection
        txtDescription.Clear()
        txtDepartment.SelectedIndex = -1 ' Reset the ComboBox selection
        txtGageType.SelectedIndex = -1 ' Reset the ComboBox selection
        txtCustomer.Clear()
        txtCalibratedBy.Clear()
        txtInterval.Clear()
        txtComments.Clear()
        dtInspectedDate.Value = DateTime.Now ' Reset to current date or some default
        dtDueDate.Value = DateTime.Now ' Reset to current date or some default

        SearchCheck = False
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
        Else
            ' Optionally handle the case where the interval is not a valid number
            ' For example, you could clear the due date or set it to the inspected date
            ' dtDueDate.Value = inspectedDate
        End If
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
    Private Sub LoadStatusOptions()
        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Status FROM [Status]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                cmbStatus.Items.Clear()
                While reader.Read()
                    cmbStatus.Items.Add(reader("Status").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading status options: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub LoadDepartmentOptions()
        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT Departments FROM [Departments]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtDepartment.Items.Clear()
                While reader.Read()
                    txtDepartment.Items.Add(reader("Departments").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Departments options: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub LoadGageTypeOptions()
        Using conn As New OleDbConnection(ConnectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT GageType FROM [GageType]", conn) ' Adjust table and column names as necessary
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtGageType.Items.Clear()
                While reader.Read()
                    txtGageType.Items.Add(reader("GageType").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading GageType options: " & ex.Message)
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
    End Sub
End Class
