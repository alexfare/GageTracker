Imports System.Net
Imports System.Net.Mail

Public Class ReportIssue

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        If String.IsNullOrWhiteSpace(txtComment.Text) Then
            MessageBox.Show("Comment is required.", "Required Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtComment.Focus()
            Return
        End If

        ReportHandler()
    End Sub

    Private Sub ReportHandler()
        Dim mail As New MailMessage()
        mail.From = New MailAddress(My.Settings.ReportSendFrom)
        mail.To.Add(My.Settings.ReportSendTo)
        mail.Subject = "GageTracker-v5 User Feedback"
        mail.Body = $"Name: {If(String.IsNullOrWhiteSpace(txtName.Text), "Not provided", txtName.Text)}" + Environment.NewLine +
            $"Email: {If(String.IsNullOrWhiteSpace(txtEmail.Text), "Not provided", txtEmail.Text)}" + Environment.NewLine +
            $"Comment: {txtComment.Text}" + Environment.NewLine +
            $"Version: {My.Settings.VersionString}"

        Dim ReportAuth As String = SecureHandler.GetDecryptedPassword
        Dim smtp As New SmtpClient("smtp.gmail.com")
        smtp.Port = My.Settings.ReportAuth
        smtp.EnableSsl = True
        smtp.UseDefaultCredentials = False
        smtp.Credentials = New System.Net.NetworkCredential(My.Settings.ReportSendFrom, ReportAuth)

        Try
            smtp.Send(mail)
            MessageBox.Show("Report sent successfully.")
            GlobalVars.SystemLog = "Report has been sent."
            Logger.LogSystem()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Failed to send report. Error: " + ex.Message)
            GlobalVars.ErrorLog = "Failed to send report. Error: " + ex.Message
            Logger.LogErrors()
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub
End Class