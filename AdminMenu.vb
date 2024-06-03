Public Class AdminMenu
    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        GTMenu.Show()
        Me.Close()
    End Sub

    Private Sub BtnLogout_Click(sender As Object, e As EventArgs) Handles BtnLogout.Click
        GlobalVars.UserActive = False
        GTMenu.Show()
        Me.Close()
    End Sub

    Private Sub btnCustomer_Click(sender As Object, e As EventArgs) Handles btnCustomer.Click
        CustomerEntry.Show()
        Me.Close()
    End Sub
End Class