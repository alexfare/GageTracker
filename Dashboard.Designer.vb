<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Dashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dashboard))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TxtOverdue = New System.Windows.Forms.TextBox()
        Me.Txt30 = New System.Windows.Forms.TextBox()
        Me.Txt60 = New System.Windows.Forms.TextBox()
        Me.TxtLost = New System.Windows.Forms.TextBox()
        Me.TxtInactive = New System.Windows.Forms.TextBox()
        Me.TxtActive = New System.Windows.Forms.TextBox()
        Me.BtnOverdue = New System.Windows.Forms.Button()
        Me.Btn30 = New System.Windows.Forms.Button()
        Me.Btn60 = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(35, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Active Gages"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(170, 121)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 13)
        Me.Label5.TabIndex = 8
        Me.Label5.Text = "Due within 30 days"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(304, 121)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(97, 13)
        Me.Label4.TabIndex = 10
        Me.Label4.Text = "Due within 60 days"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(35, 121)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Overdue Gages"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(170, 36)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Inactive Gages"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(304, 36)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Lost Gages"
        '
        'TxtOverdue
        '
        Me.TxtOverdue.Location = New System.Drawing.Point(38, 146)
        Me.TxtOverdue.Name = "TxtOverdue"
        Me.TxtOverdue.ReadOnly = True
        Me.TxtOverdue.Size = New System.Drawing.Size(100, 20)
        Me.TxtOverdue.TabIndex = 7
        Me.TxtOverdue.TabStop = False
        '
        'Txt30
        '
        Me.Txt30.Location = New System.Drawing.Point(173, 146)
        Me.Txt30.Name = "Txt30"
        Me.Txt30.ReadOnly = True
        Me.Txt30.Size = New System.Drawing.Size(100, 20)
        Me.Txt30.TabIndex = 9
        Me.Txt30.TabStop = False
        '
        'Txt60
        '
        Me.Txt60.Location = New System.Drawing.Point(307, 146)
        Me.Txt60.Name = "Txt60"
        Me.Txt60.ReadOnly = True
        Me.Txt60.Size = New System.Drawing.Size(100, 20)
        Me.Txt60.TabIndex = 11
        Me.Txt60.TabStop = False
        '
        'TxtLost
        '
        Me.TxtLost.Location = New System.Drawing.Point(307, 61)
        Me.TxtLost.Name = "TxtLost"
        Me.TxtLost.ReadOnly = True
        Me.TxtLost.Size = New System.Drawing.Size(100, 20)
        Me.TxtLost.TabIndex = 5
        Me.TxtLost.TabStop = False
        '
        'TxtInactive
        '
        Me.TxtInactive.Location = New System.Drawing.Point(173, 61)
        Me.TxtInactive.Name = "TxtInactive"
        Me.TxtInactive.ReadOnly = True
        Me.TxtInactive.Size = New System.Drawing.Size(100, 20)
        Me.TxtInactive.TabIndex = 3
        Me.TxtInactive.TabStop = False
        '
        'TxtActive
        '
        Me.TxtActive.Location = New System.Drawing.Point(38, 61)
        Me.TxtActive.Name = "TxtActive"
        Me.TxtActive.ReadOnly = True
        Me.TxtActive.Size = New System.Drawing.Size(100, 20)
        Me.TxtActive.TabIndex = 1
        Me.TxtActive.TabStop = False
        '
        'BtnOverdue
        '
        Me.BtnOverdue.Location = New System.Drawing.Point(38, 172)
        Me.BtnOverdue.Name = "BtnOverdue"
        Me.BtnOverdue.Size = New System.Drawing.Size(100, 23)
        Me.BtnOverdue.TabIndex = 0
        Me.BtnOverdue.Text = "Display Overdue"
        Me.BtnOverdue.UseVisualStyleBackColor = True
        '
        'Btn30
        '
        Me.Btn30.Location = New System.Drawing.Point(173, 172)
        Me.Btn30.Name = "Btn30"
        Me.Btn30.Size = New System.Drawing.Size(100, 23)
        Me.Btn30.TabIndex = 1
        Me.Btn30.Text = "Display 30"
        Me.Btn30.UseVisualStyleBackColor = True
        '
        'Btn60
        '
        Me.Btn60.Location = New System.Drawing.Point(301, 172)
        Me.Btn60.Name = "Btn60"
        Me.Btn60.Size = New System.Drawing.Size(100, 23)
        Me.Btn60.TabIndex = 2
        Me.Btn60.Text = "Display 60"
        Me.Btn60.UseVisualStyleBackColor = True
        '
        'Dashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(460, 247)
        Me.Controls.Add(Me.Btn60)
        Me.Controls.Add(Me.Btn30)
        Me.Controls.Add(Me.BtnOverdue)
        Me.Controls.Add(Me.Txt60)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Txt30)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.TxtOverdue)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.TxtLost)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TxtInactive)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.TxtActive)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Dashboard"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dashboard"
        Me.TopMost = True
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents TxtOverdue As TextBox
    Friend WithEvents Txt30 As TextBox
    Friend WithEvents Txt60 As TextBox
    Friend WithEvents TxtLost As TextBox
    Friend WithEvents TxtInactive As TextBox
    Friend WithEvents TxtActive As TextBox
    Friend WithEvents BtnOverdue As Button
    Friend WithEvents Btn30 As Button
    Friend WithEvents Btn60 As Button
End Class
