Imports System.Text
Imports System.Windows.Forms ' Make sure this import is present for clipboard access

Public Class AdminMenu
    Dim connectionString As String

    Private Sub AdminMenu_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        CenterToScreen()
    End Sub

    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        If My.Settings.FromList = True Then
            My.Settings.FromList = False
            Me.Hide()
        Else
            GTMenu.Show()
            Me.Hide()
        End If
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs) Handles BtnLogout.Click
        My.Settings.isAdmin = False

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

    Private Sub btnRemoveGage_Click(sender As Object, e As EventArgs) Handles btnRemoveGage.Click
        RemoveGage.Show()
    End Sub

    Private Sub btnStatus_Click(sender As Object, e As EventArgs) Handles btnStatus.Click
        StatusMenu.Show()
    End Sub

    Private Sub btnAccount_Click(sender As Object, e As EventArgs) Handles btnAccount.Click
        AccountManagement.Show()
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
        Me.Hide()
    End Sub

    Private Sub GageListToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GageListToolStripMenuItem.Click
        My.Settings.FromList = False
        Me.Hide()
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
End Class