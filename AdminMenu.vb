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
        If GlobalVars.AdminLoad = "1" Then
            GageList.Show()
            GlobalVars.AdminLoad = ""
        Else
            GTMenu.Show()
        End If
        Me.Hide()
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs) Handles BtnLogout.Click
        GlobalVars.UserActive = False
        If GlobalVars.AdminLoad = "1" Then
            GageList.Show()
            GlobalVars.AdminLoad = ""
        Else
            GTMenu.Show()
        End If
        Me.Hide()
    End Sub

    Private Sub btnCustomer_Click(sender As Object, e As EventArgs) Handles btnCustomer.Click
        CustomerEntry.Show()
        Me.Close()
    End Sub

    Private Sub btnRemoveGage_Click(sender As Object, e As EventArgs) Handles btnRemoveGage.Click
        RemoveGage.Show()
        Me.Hide()
    End Sub

    Private Sub btnStatus_Click(sender As Object, e As EventArgs) Handles btnStatus.Click
        StatusMenu.Show()
    End Sub

    Private Sub btnAccount_Click(sender As Object, e As EventArgs) Handles btnAccount.Click
        AccountManagement.Show()
        Me.Hide()
    End Sub
End Class