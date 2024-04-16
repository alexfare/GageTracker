<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GageList
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GageList))
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GageIDDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StatusDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PartNumberDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DescriptionDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DepartmentDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GageTypeDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.InspectedDateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DueDateDataGridViewTextBoxColumn = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CalibrationTrackerBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.GTDatabaseDataSet = New GageCalibrationTracker.GTDatabaseDataSet()
        Me.CalibrationTrackerTableAdapter = New GageCalibrationTracker.GTDatabaseDataSetTableAdapters.CalibrationTrackerTableAdapter()
        Me.VScrollBar1 = New System.Windows.Forms.VScrollBar()
        Me.BtnMenu = New System.Windows.Forms.Button()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.CalibrationTrackerBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GTDatabaseDataSet, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'DataGridView1
        '
        Me.DataGridView1.AutoGenerateColumns = False
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.GageIDDataGridViewTextBoxColumn, Me.StatusDataGridViewTextBoxColumn, Me.PartNumberDataGridViewTextBoxColumn, Me.DescriptionDataGridViewTextBoxColumn, Me.DepartmentDataGridViewTextBoxColumn, Me.GageTypeDataGridViewTextBoxColumn, Me.InspectedDateDataGridViewTextBoxColumn, Me.DueDateDataGridViewTextBoxColumn})
        Me.DataGridView1.DataSource = Me.CalibrationTrackerBindingSource
        Me.DataGridView1.Location = New System.Drawing.Point(12, 12)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(850, 508)
        Me.DataGridView1.TabIndex = 0
        '
        'GageIDDataGridViewTextBoxColumn
        '
        Me.GageIDDataGridViewTextBoxColumn.DataPropertyName = "GageID"
        Me.GageIDDataGridViewTextBoxColumn.HeaderText = "GageID"
        Me.GageIDDataGridViewTextBoxColumn.Name = "GageIDDataGridViewTextBoxColumn"
        '
        'StatusDataGridViewTextBoxColumn
        '
        Me.StatusDataGridViewTextBoxColumn.DataPropertyName = "Status"
        Me.StatusDataGridViewTextBoxColumn.HeaderText = "Status"
        Me.StatusDataGridViewTextBoxColumn.Name = "StatusDataGridViewTextBoxColumn"
        '
        'PartNumberDataGridViewTextBoxColumn
        '
        Me.PartNumberDataGridViewTextBoxColumn.DataPropertyName = "PartNumber"
        Me.PartNumberDataGridViewTextBoxColumn.HeaderText = "PartNumber"
        Me.PartNumberDataGridViewTextBoxColumn.Name = "PartNumberDataGridViewTextBoxColumn"
        '
        'DescriptionDataGridViewTextBoxColumn
        '
        Me.DescriptionDataGridViewTextBoxColumn.DataPropertyName = "Description"
        Me.DescriptionDataGridViewTextBoxColumn.HeaderText = "Description"
        Me.DescriptionDataGridViewTextBoxColumn.Name = "DescriptionDataGridViewTextBoxColumn"
        '
        'DepartmentDataGridViewTextBoxColumn
        '
        Me.DepartmentDataGridViewTextBoxColumn.DataPropertyName = "Department"
        Me.DepartmentDataGridViewTextBoxColumn.HeaderText = "Department"
        Me.DepartmentDataGridViewTextBoxColumn.Name = "DepartmentDataGridViewTextBoxColumn"
        '
        'GageTypeDataGridViewTextBoxColumn
        '
        Me.GageTypeDataGridViewTextBoxColumn.DataPropertyName = "Gage Type"
        Me.GageTypeDataGridViewTextBoxColumn.HeaderText = "Gage Type"
        Me.GageTypeDataGridViewTextBoxColumn.Name = "GageTypeDataGridViewTextBoxColumn"
        '
        'InspectedDateDataGridViewTextBoxColumn
        '
        Me.InspectedDateDataGridViewTextBoxColumn.DataPropertyName = "Inspected Date"
        Me.InspectedDateDataGridViewTextBoxColumn.HeaderText = "Inspected Date"
        Me.InspectedDateDataGridViewTextBoxColumn.Name = "InspectedDateDataGridViewTextBoxColumn"
        '
        'DueDateDataGridViewTextBoxColumn
        '
        Me.DueDateDataGridViewTextBoxColumn.DataPropertyName = "Due Date"
        Me.DueDateDataGridViewTextBoxColumn.HeaderText = "Due Date"
        Me.DueDateDataGridViewTextBoxColumn.Name = "DueDateDataGridViewTextBoxColumn"
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
        'CalibrationTrackerTableAdapter
        '
        Me.CalibrationTrackerTableAdapter.ClearBeforeFill = True
        '
        'VScrollBar1
        '
        Me.VScrollBar1.Location = New System.Drawing.Point(865, 12)
        Me.VScrollBar1.Name = "VScrollBar1"
        Me.VScrollBar1.Size = New System.Drawing.Size(18, 511)
        Me.VScrollBar1.TabIndex = 1
        '
        'BtnMenu
        '
        Me.BtnMenu.Location = New System.Drawing.Point(12, 526)
        Me.BtnMenu.Name = "BtnMenu"
        Me.BtnMenu.Size = New System.Drawing.Size(75, 23)
        Me.BtnMenu.TabIndex = 2
        Me.BtnMenu.Text = "Menu"
        Me.BtnMenu.UseVisualStyleBackColor = True
        '
        'GageList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(892, 561)
        Me.Controls.Add(Me.BtnMenu)
        Me.Controls.Add(Me.VScrollBar1)
        Me.Controls.Add(Me.DataGridView1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "GageList"
        Me.Text = "GageList"
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.CalibrationTrackerBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GTDatabaseDataSet, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents GTDatabaseDataSet As GTDatabaseDataSet
    Friend WithEvents CalibrationTrackerBindingSource As BindingSource
    Friend WithEvents CalibrationTrackerTableAdapter As GTDatabaseDataSetTableAdapters.CalibrationTrackerTableAdapter
    Friend WithEvents GageIDDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents StatusDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents PartNumberDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DescriptionDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DepartmentDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents GageTypeDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents InspectedDateDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents DueDateDataGridViewTextBoxColumn As DataGridViewTextBoxColumn
    Friend WithEvents VScrollBar1 As VScrollBar
    Friend WithEvents BtnMenu As Button
End Class
