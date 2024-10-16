﻿
Imports System.Data.OleDb

Public Class CustomerEntry
    Dim connectionString As String
    Dim insertQuery As String = "INSERT INTO Customers (CustomerName, CustomerAddress, CustomerPhone, CustomerWebsite) VALUES (@Name, @Address, @Phone, @Website)"

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
        LoadCustomers()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        If String.IsNullOrWhiteSpace(txtCustomerName.Text) Then
            MessageBox.Show("Customer Name cannot be blank.")
            Return
        End If

        Using checkConn As New OleDbConnection(connectionString)
            Dim checkCmd As New OleDbCommand("SELECT COUNT(*) FROM Customers WHERE CustomerName = @Name", checkConn)
            checkCmd.Parameters.AddWithValue("@Name", txtCustomerName.Text)

            Try
                checkConn.Open()
                Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                If count > 0 Then
                    MessageBox.Show("This Customer Name already exists. Please enter a unique name.")
                    Return
                End If
            Catch ex As Exception
                MessageBox.Show("An error occurred while checking for duplicate names: " & ex.Message)
                Return
            End Try
        End Using

        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@Name", txtCustomerName.Text)
                command.Parameters.AddWithValue("@Address", txtCustomerAddress.Text)
                command.Parameters.AddWithValue("@Phone", txtCustomerPhone.Text)
                command.Parameters.AddWithValue("@Website", txtCustomerWebsite.Text)

                Try
                    connection.Open()
                    command.ExecuteNonQuery()
                    MessageBox.Show("Customer added successfully.")
                    LoadCustomers()
                Catch ex As Exception
                    MessageBox.Show("An error occurred while adding new customer: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub


    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub

    Private Sub LoadCustomers()
        Using conn As New OleDbConnection(connectionString)
            Try
                conn.Open()
                Dim cmd As New OleDbCommand("SELECT CustomerName FROM Customers", conn)
                Dim reader As OleDbDataReader = cmd.ExecuteReader()
                txtCustomerName.Items.Clear()
                While reader.Read()
                    txtCustomerName.Items.Add(reader("CustomerName").ToString())
                End While
            Catch ex As Exception
                MessageBox.Show("An error occurred while loading Customers: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub txtCustomerName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtCustomerName.SelectedIndexChanged
        If txtCustomerName.SelectedItem Is Nothing Then
            txtCustomerAddress.Clear()
            txtCustomerPhone.Clear()
            txtCustomerWebsite.Clear()
            Return
        End If

        Dim selectedCustomer As String = txtCustomerName.SelectedItem.ToString()

        Dim query As String = "SELECT CustomerAddress, CustomerPhone, CustomerWebsite FROM Customers WHERE CustomerName = @Name"
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Name", selectedCustomer)

                Try
                    connection.Open()
                    Dim reader As OleDbDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        txtCustomerAddress.Text = reader("CustomerAddress").ToString()
                        txtCustomerPhone.Text = reader("CustomerPhone").ToString()
                        txtCustomerWebsite.Text = reader("CustomerWebsite").ToString()
                    Else
                        txtCustomerAddress.Clear()
                        txtCustomerPhone.Clear()
                        txtCustomerWebsite.Clear()
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while fetching customer details: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If txtCustomerName.SelectedIndex = -1 Then
            MessageBox.Show("Please select a customer to update.")
            Return
        End If

        Dim selectedCustomer As String = txtCustomerName.SelectedItem.ToString()

        Dim query As String = "UPDATE Customers SET CustomerAddress = @Address, CustomerPhone = @Phone, CustomerWebsite = @Website WHERE CustomerName = @Name"
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Address", txtCustomerAddress.Text)
                command.Parameters.AddWithValue("@Phone", txtCustomerPhone.Text)
                command.Parameters.AddWithValue("@Website", txtCustomerWebsite.Text)
                command.Parameters.AddWithValue("@Name", selectedCustomer)

                Try
                    connection.Open()
                    Dim rowsAffected As Integer = command.ExecuteNonQuery()

                    If rowsAffected > 0 Then
                        MessageBox.Show("Customer details updated successfully.")
                    Else
                        MessageBox.Show("No records updated.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while updating customer details: " & ex.Message)
                End Try
            End Using
        End Using
        LoadCustomers()
    End Sub

    Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearText()
    End Sub

    Private Sub ClearText()
        txtCustomerName.SelectedIndex = -1
        txtCustomerAddress.Clear()
        txtCustomerPhone.Clear()
        txtCustomerWebsite.Clear()
    End Sub

    Private Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click
        If txtCustomerName.SelectedIndex = -1 OrElse String.IsNullOrEmpty(txtCustomerName.Text) Then
            MessageBox.Show("Please select a customer to remove.")
            Return
        End If

        If MessageBox.Show("Are you sure you want to delete this customer?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
            Return
        End If

        Dim selectedCustomer As String = txtCustomerName.SelectedItem.ToString()

        Dim query As String = "DELETE FROM Customers WHERE CustomerName = @Name"
        Using connection As New OleDbConnection(connectionString)
            Using command As New OleDbCommand(query, connection)
                command.Parameters.AddWithValue("@Name", selectedCustomer)

                Try
                    connection.Open()
                    Dim result As Integer = command.ExecuteNonQuery()
                    If result > 0 Then
                        MessageBox.Show("Customer deleted successfully.")
                        ClearText()
                        LoadCustomers()
                    Else
                        MessageBox.Show("No records were deleted.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred while deleting the customer: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

End Class