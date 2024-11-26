Public Class ExitConfirmationForm

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        If rbDoNotAskAgain.Checked Then
            My.Settings.ExitConfirmation = True
            My.Settings.Save()
        End If
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        If rbDoNotAskAgain.Checked Then
            My.Settings.ExitConfirmation = True
            My.Settings.Save()
        End If
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub
End Class
