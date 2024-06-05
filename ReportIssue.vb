Imports System.Net.Mail

Public Class ReportIssue
    Dim ConnectionString As String

    Private Sub ReportIssue_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & GlobalVars.DatabaseLocation & ";"
    End Sub

    Private Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        ' Validate the Name field
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Setup the MailMessage
        Dim mail As New MailMessage()
        mail.From = New MailAddress("ninsosoft@gmail.com")
        mail.To.Add("alexfare94@gmail.com")
        mail.Subject = "User Feedback"
        mail.Body = $"Name: {txtName.Text}" + Environment.NewLine +
                $"Email: {If(String.IsNullOrWhiteSpace(txtEmail.Text), "Not provided", txtEmail.Text)}" + Environment.NewLine +
                $"Comment: {txtComment.Text}"

        ' Configure the SMTP client
        Dim smtp As New SmtpClient("smtp.gmail.com")
        smtp.Port = 587
        smtp.EnableSsl = True
        smtp.Credentials = New System.Net.NetworkCredential("ninsosoft@gmail.com", "ygtvawlzlgmvydru")  ' App password
        smtp.UseDefaultCredentials = False

        ' Send the email
        Try
            smtp.Send(mail)
            MessageBox.Show("Email sent successfully.")
        Catch ex As Exception
            MessageBox.Show("Failed to send email. Error: " + ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        GTMenu.Show()
        Me.Close()
    End Sub
End Class