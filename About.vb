Public NotInheritable Class About

    Private Sub About_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ModernTheme.Apply(Me)
        Dim ApplicationTitle As String
        If My.Application.Info.Title <> "" Then
            ApplicationTitle = My.Application.Info.Title
        Else
            ApplicationTitle = System.IO.Path.GetFileNameWithoutExtension(My.Application.Info.AssemblyName)
        End If
        Me.Text = String.Format("About {0}", ApplicationTitle)

        Me.LabelProductName.Text = My.Application.Info.ProductName
        Me.LabelVersion.Text = String.Format("Version {0}", My.Application.Info.Version.ToString)
        Me.LabelCopyright.Text = My.Application.Info.Copyright
        Me.LabelCompanyName.Text = My.Application.Info.CompanyName
        Me.TextBoxDescription.Text = My.Application.Info.Description

        TextBoxDescription.BackColor = ModernTheme.SurfaceColor
        TextBoxDescription.ForeColor = Color.WhiteSmoke
        OKButton.FlatStyle = FlatStyle.Flat
        OKButton.FlatAppearance.BorderSize = 0
        OKButton.BackColor = ModernTheme.AccentColor
        OKButton.ForeColor = Color.White
        OKButton.FlatAppearance.MouseOverBackColor = ModernTheme.AdjustColor(OKButton.BackColor, 0.2)
        OKButton.FlatAppearance.MouseDownBackColor = ModernTheme.AdjustColor(OKButton.BackColor, -0.15)
    End Sub

    Private Sub OKButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OKButton.Click
        Me.Close()
    End Sub

End Class
