<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Dashboard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Dashboard))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TxtOverdue = New System.Windows.Forms.TextBox()
        Me.Txt30 = New System.Windows.Forms.TextBox()
        Me.Txt60 = New System.Windows.Forms.TextBox()
        Me.TxtLost = New System.Windows.Forms.TextBox()
        Me.TxtInactive = New System.Windows.Forms.TextBox()
        Me.BtnOverdue = New System.Windows.Forms.Button()
        Me.Btn30 = New System.Windows.Forms.Button()
        Me.Btn60 = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TxtActive = New System.Windows.Forms.TextBox()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.Panel1.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(15, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 20)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Active Gages"
        '
        'TxtOverdue
        '
        Me.TxtOverdue.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TxtOverdue.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtOverdue.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtOverdue.Location = New System.Drawing.Point(19, 44)
        Me.TxtOverdue.Name = "TxtOverdue"
        Me.TxtOverdue.ReadOnly = True
        Me.TxtOverdue.Size = New System.Drawing.Size(100, 19)
        Me.TxtOverdue.TabIndex = 7
        Me.TxtOverdue.TabStop = False
        Me.TxtOverdue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Txt30
        '
        Me.Txt30.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Txt30.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Txt30.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Txt30.Location = New System.Drawing.Point(17, 44)
        Me.Txt30.Name = "Txt30"
        Me.Txt30.ReadOnly = True
        Me.Txt30.Size = New System.Drawing.Size(100, 19)
        Me.Txt30.TabIndex = 9
        Me.Txt30.TabStop = False
        Me.Txt30.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Txt60
        '
        Me.Txt60.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Txt60.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Txt60.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Txt60.Location = New System.Drawing.Point(15, 44)
        Me.Txt60.Name = "Txt60"
        Me.Txt60.ReadOnly = True
        Me.Txt60.Size = New System.Drawing.Size(100, 19)
        Me.Txt60.TabIndex = 11
        Me.Txt60.TabStop = False
        Me.Txt60.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxtLost
        '
        Me.TxtLost.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TxtLost.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtLost.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtLost.Location = New System.Drawing.Point(15, 44)
        Me.TxtLost.Name = "TxtLost"
        Me.TxtLost.ReadOnly = True
        Me.TxtLost.Size = New System.Drawing.Size(100, 19)
        Me.TxtLost.TabIndex = 5
        Me.TxtLost.TabStop = False
        Me.TxtLost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'TxtInactive
        '
        Me.TxtInactive.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TxtInactive.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtInactive.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtInactive.Location = New System.Drawing.Point(16, 44)
        Me.TxtInactive.Name = "TxtInactive"
        Me.TxtInactive.ReadOnly = True
        Me.TxtInactive.Size = New System.Drawing.Size(100, 19)
        Me.TxtInactive.TabIndex = 3
        Me.TxtInactive.TabStop = False
        Me.TxtInactive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'BtnOverdue
        '
        Me.BtnOverdue.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnOverdue.Location = New System.Drawing.Point(11, 236)
        Me.BtnOverdue.Name = "BtnOverdue"
        Me.BtnOverdue.Size = New System.Drawing.Size(107, 39)
        Me.BtnOverdue.TabIndex = 0
        Me.BtnOverdue.Text = "Overdue"
        Me.BtnOverdue.UseVisualStyleBackColor = True
        '
        'Btn30
        '
        Me.Btn30.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn30.Location = New System.Drawing.Point(152, 236)
        Me.Btn30.Name = "Btn30"
        Me.Btn30.Size = New System.Drawing.Size(106, 39)
        Me.Btn30.TabIndex = 1
        Me.Btn30.Text = "Due in 30"
        Me.Btn30.UseVisualStyleBackColor = True
        '
        'Btn60
        '
        Me.Btn60.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btn60.Location = New System.Drawing.Point(295, 236)
        Me.Btn60.Name = "Btn60"
        Me.Btn60.Size = New System.Drawing.Size(106, 39)
        Me.Btn60.TabIndex = 2
        Me.Btn60.Text = "Due in 60"
        Me.Btn60.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Panel7)
        Me.Panel1.Controls.Add(Me.Panel4)
        Me.Panel1.Controls.Add(Me.Btn30)
        Me.Panel1.Controls.Add(Me.BtnOverdue)
        Me.Panel1.Controls.Add(Me.Btn60)
        Me.Panel1.Controls.Add(Me.Panel6)
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Controls.Add(Me.Panel5)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Location = New System.Drawing.Point(12, 12)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(433, 278)
        Me.Panel1.TabIndex = 12
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel7.Controls.Add(Me.Label9)
        Me.Panel7.Controls.Add(Me.Txt60)
        Me.Panel7.Location = New System.Drawing.Point(285, 132)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(135, 98)
        Me.Panel7.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(3, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(114, 20)
        Me.Label9.TabIndex = 0
        Me.Label9.Text = "Due in 60 days"
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel4.Controls.Add(Me.Label2)
        Me.Panel4.Controls.Add(Me.TxtLost)
        Me.Panel4.Location = New System.Drawing.Point(285, 3)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(135, 98)
        Me.Panel4.TabIndex = 16
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(92, 20)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Lost Gages"
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel6.Controls.Add(Me.Label8)
        Me.Panel6.Controls.Add(Me.Txt30)
        Me.Panel6.Location = New System.Drawing.Point(144, 132)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(135, 98)
        Me.Panel6.TabIndex = 18
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(3, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(114, 20)
        Me.Label8.TabIndex = 0
        Me.Label8.Text = "Due in 30 days"
        '
        'Panel3
        '
        Me.Panel3.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel3.Controls.Add(Me.Label7)
        Me.Panel3.Controls.Add(Me.TxtInactive)
        Me.Panel3.Location = New System.Drawing.Point(144, 3)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(135, 98)
        Me.Panel3.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(6, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(116, 20)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "Inactive Gages"
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel5.Controls.Add(Me.Label3)
        Me.Panel5.Controls.Add(Me.TxtOverdue)
        Me.Panel5.Location = New System.Drawing.Point(3, 132)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(135, 98)
        Me.Panel5.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 20)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Overdue Gages"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.TxtActive)
        Me.Panel2.Location = New System.Drawing.Point(3, 3)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(135, 98)
        Me.Panel2.TabIndex = 14
        '
        'TxtActive
        '
        Me.TxtActive.BackColor = System.Drawing.SystemColors.ActiveCaption
        Me.TxtActive.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TxtActive.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TxtActive.Location = New System.Drawing.Point(19, 44)
        Me.TxtActive.Name = "TxtActive"
        Me.TxtActive.ReadOnly = True
        Me.TxtActive.Size = New System.Drawing.Size(100, 19)
        Me.TxtActive.TabIndex = 1
        Me.TxtActive.TabStop = False
        Me.TxtActive.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(23, 296)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(75, 23)
        Me.BtnClose.TabIndex = 13
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'Dashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(456, 323)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Dashboard"
        Me.Opacity = 0.99R
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Dashboard - GageTracker"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.Panel3.PerformLayout()
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents TxtOverdue As TextBox
    Friend WithEvents Txt30 As TextBox
    Friend WithEvents Txt60 As TextBox
    Friend WithEvents TxtLost As TextBox
    Friend WithEvents TxtInactive As TextBox
    Friend WithEvents BtnOverdue As Button
    Friend WithEvents Btn30 As Button
    Friend WithEvents Btn60 As Button
    Friend WithEvents Panel1 As Panel
    Friend WithEvents BtnClose As Button
    Friend WithEvents TxtActive As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents Label7 As Label
    Friend WithEvents Panel4 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label3 As Label
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label8 As Label
    Friend WithEvents Panel7 As Panel
    Friend WithEvents Label9 As Label
End Class
