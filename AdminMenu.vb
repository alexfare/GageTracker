Public Class AdminMenu
    Private Sub BtnBack_Click(sender As Object, e As EventArgs) Handles BtnBack.Click
        Dim menuForm As New Menu()
        menuForm.Show()
        Me.Close()
    End Sub
End Class