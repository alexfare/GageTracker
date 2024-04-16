Public Class AdminMenu
    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        Dim menuForm As New GTMenu()
        menuForm.Show()
        Me.Close()
    End Sub
End Class