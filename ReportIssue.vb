Imports System.Net.Mail

Public Class ReportIssue
    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Name is required.", "Required Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtName.Focus()
            Return
        End If
        If String.IsNullOrWhiteSpace(txtComment.Text) Then
            MessageBox.Show("Comment is required.", "Required Information Missing", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtComment.Focus()
            Return
        End If

        ManualReportHandler()
    End Sub

    Private Sub ManualReportHandler()
        Dim mail As New MailMessage()
        mail.From = New MailAddress(My.Settings.ReportSendFrom)
        mail.To.Add(My.Settings.ReportSendTo)
        mail.Subject = "GageTracker-v5 User Feedback"
        mail.Body = $"Name: {txtName.Text}" + Environment.NewLine +
            $"Email: {If(String.IsNullOrWhiteSpace(txtEmail.Text), "Not provided", txtEmail.Text)}" + Environment.NewLine +
            $"Comment: {txtComment.Text}" + Environment.NewLine +
            $"Version: {My.Settings.VersionString}"

        Dim ZGVjcnlwdGVkUGFzc3dvcmQ = SecureHandler.GetDecryptedPassword
        Dim ReportAuth As String = ZGVjcnlwdGVkUGFzc3dvcmQ
        Dim smtp As New SmtpClient("smtp.gmail.com")
        smtp.Port = My.Settings.ReportAuth
        smtp.EnableSsl = True
        smtp.UseDefaultCredentials = False
        smtp.Credentials = New System.Net.NetworkCredential(My.Settings.ReportSendFrom, ReportAuth)

        Try
            smtp.Send(mail)
            MessageBox.Show("Report sent successfully.")
            GlobalVars.SystemLog = "Issue report has been sent."
            Logger.LogSystem()
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Failed to send report. Error: " + ex.Message)
            GlobalVars.ErrorLog = "Failed to send report. Error: " + ex.Message
            Logger.LogErrors()
        End Try
    End Sub

    Public Sub AutoReportHandler()
        'WIP - Removed until testing is ready.
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Me.Close()
    End Sub
End Class