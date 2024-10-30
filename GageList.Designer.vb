<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GageList
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GageList))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GageIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PartNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DescriptionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DepartmentDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GageTypeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Customer = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.InspectedDateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DueDateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CalibrationTrackerBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GTDatabaseDataSet = New GageCalibrationTracker.GTDatabaseDataSet()
        Me.BtnMenu = New System.Windows.Forms.Button()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeDatabaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PastDueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DueWithin30DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DueWithin60DaysToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdminToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SettingsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AboutToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportIssueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GithubToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WebsiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextContains = New System.Windows.Forms.TextBox()
        Me.CmbFilterType = New System.Windows.Forms.ComboBox()
        Me.CmbContains = New System.Windows.Forms.ComboBox()
        Me.CheckBoxShowAll = New System.Windows.Forms.CheckBox()
        Me.CalibrationTrackerTableAdapter = New GageCalibrationTracker.GTDatabaseDataSetTableAdapters.CalibrationTrackerTableAdapter()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CalibrationTrackerBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GTDatabaseDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.GageIDDataGridViewTextBoxColumn, Me.StatusDataGridViewTextBoxColumn, Me.PartNumberDataGridViewTextBoxColumn, Me.DescriptionDataGridViewTextBoxColumn, Me.DepartmentDataGridViewTextBoxColumn, Me.GageTypeDataGridViewTextBoxColumn, Me.Customer, Me.InspectedDateDataGridViewTextBoxColumn, Me.DueDateDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.CalibrationTrackerBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(12, 72)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        Me.DataGridView1.Size = New System.Drawing.Size(1366, 639)
        Me.DataGridView1.TabIndex = 5
        '
        'GageIDDataGridViewTextBoxColumn
        '
        Me.GageIDDataGridViewTextBoxColumn.DataPropertyName = "GageID"
        Me.GageIDDataGridViewTextBoxColumn.HeaderText = "GageID"
        Me.GageIDDataGridViewTextBoxColumn.Name = "GageIDDataGridViewTextBoxColumn"
        Me.GageIDDataGridViewTextBoxColumn.ReadOnly = True
        '
        'StatusDataGridViewTextBoxColumn
        '
        Me.StatusDataGridViewTextBoxColumn.DataPropertyName = "Status"
        Me.StatusDataGridViewTextBoxColumn.HeaderText = "Status"
        Me.StatusDataGridViewTextBoxColumn.Name = "StatusDataGridViewTextBoxColumn"
        Me.StatusDataGridViewTextBoxColumn.ReadOnly = True
        '
        'PartNumberDataGridViewTextBoxColumn
        '
        Me.PartNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber"
        Me.PartNumberDataGridViewTextBoxColumn.HeaderText = "PartNumber"
        Me.PartNumberDataGridViewTextBoxColumn.Name = "PartNumberDataGridViewTextBoxColumn"
        Me.PartNumberDataGridViewTextBoxColumn.ReadOnly = True
        '
        'DescriptionDataGridViewTextBoxColumn
        '
        Me.DescriptionDataGridViewTextBoxColumn.DataPropertyName = "Description"
        Me.DescriptionDataGridViewTextBoxColumn.HeaderText = "Description"
        Me.DescriptionDataGridViewTextBoxColumn.Name = "DescriptionDataGridViewTextBoxColumn"
        Me.DescriptionDataGridViewTextBoxColumn.ReadOnly = True
        '
        'DepartmentDataGridViewTextBoxColumn
        '
        Me.DepartmentDataGridViewTextBoxColumn.DataPropertyName = "Department"
        Me.DepartmentDataGridViewTextBoxColumn.HeaderText = "Department"
        Me.DepartmentDataGridViewTextBoxColumn.Name = "DepartmentDataGridViewTextBoxColumn"
        Me.DepartmentDataGridViewTextBoxColumn.ReadOnly = True
        '
        'GageTypeDataGridViewTextBoxColumn
        '
        Me.GageTypeDataGridViewTextBoxColumn.DataPropertyName = "Gage Type"
        Me.GageTypeDataGridViewTextBoxColumn.HeaderText = "Gage Type"
        Me.GageTypeDataGridViewTextBoxColumn.Name = "GageTypeDataGridViewTextBoxColumn"
        Me.GageTypeDataGridViewTextBoxColumn.ReadOnly = True
        '
        'Customer
        '
        Me.Customer.DataPropertyName = "Customer"
        Me.Customer.HeaderText = "Customer"
        Me.Customer.Name = "Customer"
        Me.Customer.ReadOnly = True
        '
        'InspectedDateDataGridViewTextBoxColumn
        '
        Me.InspectedDateDataGridViewTextBoxColumn.DataPropertyName = "Inspected Date"
        Me.InspectedDateDataGridViewTextBoxColumn.HeaderText = "Inspected Date"
        Me.InspectedDateDataGridViewTextBoxColumn.Name = "InspectedDateDataGridViewTextBoxColumn"
        Me.InspectedDateDataGridViewTextBoxColumn.ReadOnly = True
        '
        'DueDateDataGridViewTextBoxColumn
        '
        Me.DueDateDataGridViewTextBoxColumn.DataPropertyName = "Due Date"
        Me.DueDateDataGridViewTextBoxColumn.HeaderText = "Due Date"
        Me.DueDateDataGridViewTextBoxColumn.Name = "DueDateDataGridViewTextBoxColumn"
        Me.DueDateDataGridViewTextBoxColumn.ReadOnly = True
        '
        'CalibrationTrackerBindingSource
        '
        Me.CalibrationTrackerBindingSource.DataMember = "CalibrationTracker"
        Me.CalibrationTrackerBindingSource.DataSource = Me.GTDatabaseDataSet
        '
        'GTDatabaseDataSet
        '
        Me.GTDatabaseDataSet.DataSetName = "GTDatabaseDataSet"
        Me.GTDatabaseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'BtnMenu
        '
        Me.BtnMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BtnMenu.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BtnMenu.Location = New System.Drawing.Point(12, 717)
        Me.BtnMenu.Name = "BtnMenu"
        Me.BtnMenu.Size = New System.Drawing.Size(293, 23)
        Me.BtnMenu.TabIndex = 4
        Me.BtnMenu.Text = "Menu"
        Me.BtnMenu.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(248, 42)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "GageTracker"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.ViewToolStripMenuItem, Me.AdminToolStripMenuItem, Me.SettingsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1390, 24)
        Me.MenuStrip1.TabIndex = 7
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MenuToolStripMenuItem, Me.ChangeDatabaseToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'MenuToolStripMenuItem
        '
        Me.MenuToolStripMenuItem.Name = "MenuToolStripMenuItem"
        Me.MenuToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.MenuToolStripMenuItem.Text = "Menu"
        '
        'ChangeDatabaseToolStripMenuItem
        '
        Me.ChangeDatabaseToolStripMenuItem.Name = "ChangeDatabaseToolStripMenuItem"
        Me.ChangeDatabaseToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.ChangeDatabaseToolStripMenuItem.Text = "Change Database"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PastDueToolStripMenuItem, Me.DueWithin30DaysToolStripMenuItem, Me.DueWithin60DaysToolStripMenuItem, Me.RefreshToolStripMenuItem})
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'PastDueToolStripMenuItem
        '
        Me.PastDueToolStripMenuItem.Name = "PastDueToolStripMenuItem"
        Me.PastDueToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.PastDueToolStripMenuItem.Text = "Past Due"
        '
        'DueWithin30DaysToolStripMenuItem
        '
        Me.DueWithin30DaysToolStripMenuItem.Name = "DueWithin30DaysToolStripMenuItem"
        Me.DueWithin30DaysToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.DueWithin30DaysToolStripMenuItem.Text = "Due within 30 days"
        '
        'DueWithin60DaysToolStripMenuItem
        '
        Me.DueWithin60DaysToolStripMenuItem.Name = "DueWithin60DaysToolStripMenuItem"
        Me.DueWithin60DaysToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.DueWithin60DaysToolStripMenuItem.Text = "Due within 60 days"
        '
        'RefreshToolStripMenuItem
        '
        Me.RefreshToolStripMenuItem.Name = "RefreshToolStripMenuItem"
        Me.RefreshToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.RefreshToolStripMenuItem.Text = "Refresh"
        '
        'AdminToolStripMenuItem
        '
        Me.AdminToolStripMenuItem.Name = "AdminToolStripMenuItem"
        Me.AdminToolStripMenuItem.Size = New System.Drawing.Size(55, 20)
        Me.AdminToolStripMenuItem.Text = "Admin"
        '
        'SettingsToolStripMenuItem
        '
        Me.SettingsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AboutToolStripMenuItem, Me.ReportIssueToolStripMenuItem, Me.GithubToolStripMenuItem, Me.WebsiteToolStripMenuItem})
        Me.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem"
        Me.SettingsToolStripMenuItem.Size = New System.Drawing.Size(61, 20)
        Me.SettingsToolStripMenuItem.Text = "Settings"
        '
        'AboutToolStripMenuItem
        '
        Me.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem"
        Me.AboutToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.AboutToolStripMenuItem.Text = "About"
        '
        'ReportIssueToolStripMenuItem
        '
        Me.ReportIssueToolStripMenuItem.Name = "ReportIssueToolStripMenuItem"
        Me.ReportIssueToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.ReportIssueToolStripMenuItem.Text = "Report Issue"
        '
        'GithubToolStripMenuItem
        '
        Me.GithubToolStripMenuItem.Name = "GithubToolStripMenuItem"
        Me.GithubToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.GithubToolStripMenuItem.Text = "Github"
        '
        'WebsiteToolStripMenuItem
        '
        Me.WebsiteToolStripMenuItem.Name = "WebsiteToolStripMenuItem"
        Me.WebsiteToolStripMenuItem.Size = New System.Drawing.Size(138, 22)
        Me.WebsiteToolStripMenuItem.Text = "Website"
        '
        'TextContains
        '
        Me.TextContains.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextContains.Location = New System.Drawing.Point(508, 45)
        Me.TextContains.Name = "TextContains"
        Me.TextContains.Size = New System.Drawing.Size(753, 20)
        Me.TextContains.TabIndex = 0
        '
        'CmbFilterType
        '
        Me.CmbFilterType.FormattingEnabled = True
        Me.CmbFilterType.Location = New System.Drawing.Point(393, 43)
        Me.CmbFilterType.Name = "CmbFilterType"
        Me.CmbFilterType.Size = New System.Drawing.Size(109, 21)
        Me.CmbFilterType.TabIndex = 2
        '
        'CmbContains
        '
        Me.CmbContains.FormattingEnabled = True
        Me.CmbContains.Location = New System.Drawing.Point(278, 43)
        Me.CmbContains.Name = "CmbContains"
        Me.CmbContains.Size = New System.Drawing.Size(109, 21)
        Me.CmbContains.TabIndex = 1
        '
        'CheckBoxShowAll
        '
        Me.CheckBoxShowAll.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CheckBoxShowAll.AutoSize = True
        Me.CheckBoxShowAll.Location = New System.Drawing.Point(1267, 47)
        Me.CheckBoxShowAll.Name = "CheckBoxShowAll"
        Me.CheckBoxShowAll.Size = New System.Drawing.Size(111, 17)
        Me.CheckBoxShowAll.TabIndex = 3
        Me.CheckBoxShowAll.Text = "Show All Statuses"
        Me.CheckBoxShowAll.UseVisualStyleBackColor = True
        '
        'CalibrationTrackerTableAdapter
        '
        Me.CalibrationTrackerTableAdapter.ClearBeforeFill = True
        '
        'GageList
        '
        Me.AcceptButton = Me.BtnMenu
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1390, 752)
        Me.Controls.Add(Me.CheckBoxShowAll)
        Me.Controls.Add(Me.CmbContains)
        Me.Controls.Add(Me.CmbFilterType)
        Me.Controls.Add(Me.TextContains)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.BtnMenu)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "GageList"
        Me.Text = "GageTracker - GageList"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CalibrationTrackerBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GTDatabaseDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents GTDatabaseDataSet As GTDatabaseDataSet
    Friend WithEvents CalibrationTrackerBindingSource As BindingSource
    Friend WithEvents CalibrationTrackerTableAdapter As GTDatabaseDataSetTableAdapters.CalibrationTrackerTableAdapter
    Friend WithEvents BtnMenu As Button
    Friend WithEvents ColorDialog1 As ColorDialog
    Friend WithEvents GageIDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents StatusDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PartNumberDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DescriptionDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DepartmentDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents GageTypeDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Customer As DataGridViewTextBoxColumn
    Friend WithEvents InspectedDateDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DueDateDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents Label1 As Label
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ChangeDatabaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdminToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SettingsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AboutToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MenuToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReportIssueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TextContains As TextBox
    Friend WithEvents CmbFilterType As ComboBox
    Friend WithEvents CmbContains As ComboBox
    Friend WithEvents CheckBoxShowAll As CheckBox
    Friend WithEvents WebsiteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GithubToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PastDueToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DueWithin30DaysToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DueWithin60DaysToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RefreshToolStripMenuItem As ToolStripMenuItem
End Class
