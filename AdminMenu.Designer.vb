<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AdminMenu
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AdminMenu))
        Me.BtnBack = New System.Windows.Forms.Button()
        Me.BtnLogout = New System.Windows.Forms.Button()
        Me.btnCustomer = New System.Windows.Forms.Button()
        Me.btnRemoveGage = New System.Windows.Forms.Button()
        Me.btnStatus = New System.Windows.Forms.Button()
        Me.btnAccount = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BtnBack
        '
        Me.BtnBack.Location = New System.Drawing.Point(12, 182)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(75, 23)
        Me.BtnBack.TabIndex = 4
        Me.BtnBack.Text = "Back"
        Me.BtnBack.UseVisualStyleBackColor = True
        '
        'BtnLogout
        '
        Me.BtnLogout.Location = New System.Drawing.Point(283, 182)
        Me.BtnLogout.Name = "BtnLogout"
        Me.BtnLogout.Size = New System.Drawing.Size(75, 23)
        Me.BtnLogout.TabIndex = 5
        Me.BtnLogout.Text = "Logout"
        Me.BtnLogout.UseVisualStyleBackColor = True
        '
        'btnCustomer
        '
        Me.btnCustomer.Location = New System.Drawing.Point(12, 12)
        Me.btnCustomer.Name = "btnCustomer"
        Me.btnCustomer.Size = New System.Drawing.Size(82, 46)
        Me.btnCustomer.TabIndex = 0
        Me.btnCustomer.Text = "Customer Menu"
        Me.btnCustomer.UseVisualStyleBackColor = True
        '
        'btnRemoveGage
        '
        Me.btnRemoveGage.Location = New System.Drawing.Point(100, 12)
        Me.btnRemoveGage.Name = "btnRemoveGage"
        Me.btnRemoveGage.Size = New System.Drawing.Size(82, 46)
        Me.btnRemoveGage.TabIndex = 1
        Me.btnRemoveGage.Text = "Remove Gage"
        Me.btnRemoveGage.UseVisualStyleBackColor = True
        '
        'btnStatus
        '
        Me.btnStatus.Location = New System.Drawing.Point(188, 12)
        Me.btnStatus.Name = "btnStatus"
        Me.btnStatus.Size = New System.Drawing.Size(82, 46)
        Me.btnStatus.TabIndex = 2
        Me.btnStatus.Text = "Status Menu"
        Me.btnStatus.UseVisualStyleBackColor = True
        '
        'btnAccount
        '
        Me.btnAccount.Location = New System.Drawing.Point(276, 12)
        Me.btnAccount.Name = "btnAccount"
        Me.btnAccount.Size = New System.Drawing.Size(82, 46)
        Me.btnAccount.TabIndex = 3
        Me.btnAccount.Text = "Account Management"
        Me.btnAccount.UseVisualStyleBackColor = True
        '
        'AdminMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 214)
        Me.Controls.Add(Me.btnAccount)
        Me.Controls.Add(Me.btnStatus)
        Me.Controls.Add(Me.btnRemoveGage)
        Me.Controls.Add(Me.btnCustomer)
        Me.Controls.Add(Me.BtnLogout)
        Me.Controls.Add(Me.BtnBack)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AdminMenu"
        Me.Text = "AdminMenu"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents BtnBack As Button
    Friend WithEvents BtnLogout As Button
    Friend WithEvents btnCustomer As Button
    Friend WithEvents btnRemoveGage As Button
    Friend WithEvents btnStatus As Button
    Friend WithEvents btnAccount As Button
End Class
