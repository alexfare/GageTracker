﻿Imports System.Data.OleDb

Public Class Menu
    Dim ConnectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=R:\Quality\GageCalibration\GTDatabase.accdb;"

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadStatusOptions()
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
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

                    Dim addCmd As New OleDbCommand("INSERT INTO [CalibrationTracker] (GageID, PartNumber, Status, Description, Department, [Gage Type], Customer, [Calibrated By], [Interval (Months)], [Inspected Date], [Due Date], Comments) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)", conn)
                    ' Prepare and add parameters
                    addCmd.Parameters.AddWithValue("@GageID", txtGageID.Text)
                    addCmd.Parameters.AddWithValue("@PartNumber", txtPartNumber.Text)
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
                    MessageBox.Show("This GageID already exists")
                End If
            End Using
        Catch ex As OleDbException
            MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
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
                    Else
                        MessageBox.Show("GageID does not exist")
                    End If
                End Using
            Catch ex As OleDbException
                MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Catch ex As Exception
                MessageBox.Show("An unexpected error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub


    Private Sub BtnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
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
            Else
                MessageBox.Show("No record updated. Please check the GageID.")
            End If
        End Using
    End Sub

    Private Sub BtnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ' Clear all input fields
        txtGageID.Clear()
        txtPartNumber.Clear()
        cmbStatus.SelectedIndex = -1 ' Reset the ComboBox selection
        txtDescription.Clear()
        txtDepartment.Clear()
        txtGageType.Clear()
        txtCustomer.Clear()
        txtCalibratedBy.Clear()
        txtInterval.Clear()
        txtComments.Clear()
        dtInspectedDate.Value = DateTime.Now ' Reset to current date or some default
        dtDueDate.Value = DateTime.Now ' Reset to current date or some default
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
End Class
