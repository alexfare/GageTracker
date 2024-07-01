<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DueDateCategorizer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DueDateCategorizer))
        Me.BtnGageList = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.DataGridViewPastDue = New System.Windows.Forms.DataGridView()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.DataGridViewWithin30Days = New System.Windows.Forms.DataGridView()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.DataGridViewWithin60Days = New System.Windows.Forms.DataGridView()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.DataGridViewPastDue, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        CType(Me.DataGridViewWithin30Days, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage3.SuspendLayout()
        CType(Me.DataGridViewWithin60Days, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnGageList
        '
        Me.BtnGageList.Location = New System.Drawing.Point(12, 603)
        Me.BtnGageList.Name = "BtnGageList"
        Me.BtnGageList.Size = New System.Drawing.Size(75, 23)
        Me.BtnGageList.TabIndex = 3
        Me.BtnGageList.Text = "GageList"
        Me.BtnGageList.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Location = New System.Drawing.Point(12, 12)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(974, 585)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.DataGridViewPastDue)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(966, 559)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Past Due"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'DataGridViewPastDue
        '
        Me.DataGridViewPastDue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewPastDue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewPastDue.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewPastDue.Name = "DataGridViewPastDue"
        Me.DataGridViewPastDue.Size = New System.Drawing.Size(960, 553)
        Me.DataGridViewPastDue.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.DataGridViewWithin30Days)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(972, 559)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "30 days"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'DataGridViewWithin30Days
        '
        Me.DataGridViewWithin30Days.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewWithin30Days.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewWithin30Days.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewWithin30Days.Name = "DataGridViewWithin30Days"
        Me.DataGridViewWithin30Days.Size = New System.Drawing.Size(966, 553)
        Me.DataGridViewWithin30Days.TabIndex = 1
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.DataGridViewWithin60Days)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(972, 559)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "60 Days"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'DataGridViewWithin60Days
        '
        Me.DataGridViewWithin60Days.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridViewWithin60Days.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridViewWithin60Days.Location = New System.Drawing.Point(3, 3)
        Me.DataGridViewWithin60Days.Name = "DataGridViewWithin60Days"
        Me.DataGridViewWithin60Days.Size = New System.Drawing.Size(966, 553)
        Me.DataGridViewWithin60Days.TabIndex = 2
        '
        'DueDateCategorizer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(998, 635)
        Me.Controls.Add(Me.BtnGageList)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "DueDateCategorizer"
        Me.Text = "DueDateCategorizer"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.DataGridViewPastDue, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        CType(Me.DataGridViewWithin30Days, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage3.ResumeLayout(False)
        CType(Me.DataGridViewWithin60Days, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BtnGageList As Button
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents DataGridViewPastDue As DataGridView
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents DataGridViewWithin30Days As DataGridView
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents DataGridViewWithin60Days As DataGridView
End Class
