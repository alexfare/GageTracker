Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms

Public Module ModernTheme
    Public ReadOnly PrimaryColor As Color = Color.FromArgb(18, 24, 38)
    Public ReadOnly SecondaryColor As Color = Color.FromArgb(28, 37, 55)
    Public ReadOnly SurfaceColor As Color = Color.FromArgb(40, 52, 78)
    Public ReadOnly AccentColor As Color = Color.FromArgb(84, 160, 255)
    Public ReadOnly SuccessColor As Color = Color.FromArgb(76, 175, 80)
    Public ReadOnly WarningColor As Color = Color.FromArgb(255, 159, 67)
    Public ReadOnly DangerColor As Color = Color.FromArgb(235, 87, 87)

    Public Sub Apply(form As Form)
        If form Is Nothing Then
            Return
        End If

        form.SuspendLayout()
        form.BackColor = PrimaryColor
        form.ForeColor = Color.WhiteSmoke
        form.Font = New Font("Segoe UI", Math.Max(9.0F, form.Font.Size), FontStyle.Regular, GraphicsUnit.Point)
        SetDoubleBuffered(form)

        If form.MainMenuStrip IsNot Nothing Then
            form.MainMenuStrip.BackColor = Color.FromArgb(220, SecondaryColor)
            form.MainMenuStrip.ForeColor = Color.WhiteSmoke
            form.MainMenuStrip.Renderer = New ModernMenuRenderer()
            form.MainMenuStrip.Font = New Font("Segoe UI", 10.0F, FontStyle.Regular, GraphicsUnit.Point)
        End If

        ApplyToControls(form.Controls)

        form.ResumeLayout(False)
        form.PerformLayout()
    End Sub

    Private Sub ApplyToControls(controls As Control.ControlCollection)
        For Each ctrl As Control In controls
            StyleControl(ctrl)
            If ctrl.HasChildren Then
                ApplyToControls(ctrl.Controls)
            End If
        Next
    End Sub

    Private Sub StyleControl(ctrl As Control)
        Select Case True
            Case TypeOf ctrl Is Button
                StyleButton(DirectCast(ctrl, Button))
            Case TypeOf ctrl Is Label
                ctrl.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is GroupBox
                ctrl.ForeColor = Color.WhiteSmoke
                ctrl.BackColor = SecondaryColor
            Case TypeOf ctrl Is Panel
                ctrl.BackColor = SecondaryColor
            Case TypeOf ctrl Is TabControl
                Dim tab = DirectCast(ctrl, TabControl)
                tab.Appearance = TabAppearance.Normal
                tab.Font = New Font("Segoe UI", 9.5F, FontStyle.Regular)
                tab.DrawMode = TabDrawMode.Normal
                tab.BackColor = SecondaryColor
                tab.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is TabPage
                Dim tabPage = DirectCast(ctrl, TabPage)
                tabPage.UseVisualStyleBackColor = False
                tabPage.BackColor = SecondaryColor
                tabPage.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is TextBox
                Dim txt = DirectCast(ctrl, TextBox)
                txt.BorderStyle = BorderStyle.FixedSingle
                txt.BackColor = Color.FromArgb(33, 44, 66)
                txt.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is ComboBox
                Dim cmb = DirectCast(ctrl, ComboBox)
                cmb.FlatStyle = FlatStyle.Flat
                cmb.BackColor = Color.FromArgb(33, 44, 66)
                cmb.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is DateTimePicker
                Dim dtp = DirectCast(ctrl, DateTimePicker)
                dtp.CalendarMonthBackground = Color.FromArgb(33, 44, 66)
                dtp.CalendarForeColor = Color.WhiteSmoke
                dtp.BackColor = Color.FromArgb(33, 44, 66)
                dtp.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is NumericUpDown
                Dim num = DirectCast(ctrl, NumericUpDown)
                num.BorderStyle = BorderStyle.FixedSingle
                num.BackColor = Color.FromArgb(33, 44, 66)
                num.ForeColor = Color.WhiteSmoke
            Case TypeOf ctrl Is StatusStrip
                Dim strip = DirectCast(ctrl, StatusStrip)
                strip.BackColor = Color.FromArgb(220, SecondaryColor)
                strip.ForeColor = Color.WhiteSmoke
                strip.SizingGrip = False
                strip.Renderer = New ToolStripProfessionalRenderer(New ModernColorTable())
            Case TypeOf ctrl Is ToolStrip
                Dim strip = DirectCast(ctrl, ToolStrip)
                strip.BackColor = Color.FromArgb(220, SecondaryColor)
                strip.ForeColor = Color.WhiteSmoke
                strip.Renderer = New ToolStripProfessionalRenderer(New ModernColorTable())
            Case TypeOf ctrl Is DataGridView
                StyleDataGridView(DirectCast(ctrl, DataGridView))
            Case TypeOf ctrl Is ListView
                Dim list = DirectCast(ctrl, ListView)
                list.BackColor = SecondaryColor
                list.ForeColor = Color.WhiteSmoke
                list.BorderStyle = BorderStyle.None
            Case TypeOf ctrl Is RichTextBox
                Dim rtb = DirectCast(ctrl, RichTextBox)
                rtb.BorderStyle = BorderStyle.FixedSingle
                rtb.BackColor = Color.FromArgb(33, 44, 66)
                rtb.ForeColor = Color.WhiteSmoke
        End Select
    End Sub

    Private Sub StyleButton(btn As Button)
        Dim baseColor As Color = AccentColor
        Dim name = btn.Name.ToLowerInvariant()
        Dim text = btn.Text.ToLowerInvariant()

        If name.Contains("delete") OrElse name.Contains("remove") OrElse text.Contains("delete") OrElse text.Contains("remove") _
            OrElse name.Contains("cancel") OrElse text.Contains("cancel") OrElse name.Contains("close") OrElse text.Contains("close") Then
            baseColor = DangerColor
        ElseIf name.Contains("clear") OrElse text.Contains("clear") Then
            baseColor = WarningColor
        ElseIf name.Contains("save") OrElse text.Contains("save") Then
            baseColor = SuccessColor
        End If

        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.FlatAppearance.MouseOverBackColor = AdjustColor(baseColor, 0.15)
        btn.FlatAppearance.MouseDownBackColor = AdjustColor(baseColor, -0.1)
        btn.BackColor = baseColor
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI Semibold", Math.Max(9.0F, btn.Font.Size), FontStyle.Bold)
    End Sub

    Private Sub StyleDataGridView(grid As DataGridView)
        grid.BackgroundColor = PrimaryColor
        grid.GridColor = SecondaryColor
        grid.EnableHeadersVisualStyles = False
        grid.ColumnHeadersDefaultCellStyle.BackColor = SecondaryColor
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke
        grid.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        grid.DefaultCellStyle.BackColor = Color.FromArgb(34, 45, 66)
        grid.DefaultCellStyle.ForeColor = Color.WhiteSmoke
        grid.DefaultCellStyle.SelectionBackColor = AccentColor
        grid.DefaultCellStyle.SelectionForeColor = Color.White
        grid.RowHeadersDefaultCellStyle.BackColor = SecondaryColor
        grid.RowHeadersDefaultCellStyle.ForeColor = Color.WhiteSmoke
        SetDoubleBuffered(grid)
    End Sub

    Friend Function AdjustColor(color As Color, amount As Double) As Color
        amount = Math.Max(-1, Math.Min(1, amount))

        Dim r As Integer
        Dim g As Integer
        Dim b As Integer

        If amount >= 0 Then
            r = CInt(color.R + (255 - color.R) * amount)
            g = CInt(color.G + (255 - color.G) * amount)
            b = CInt(color.B + (255 - color.B) * amount)
        Else
            Dim factor As Double = 1 + amount
            r = CInt(color.R * factor)
            g = CInt(color.G * factor)
            b = CInt(color.B * factor)
        End If

        Return Color.FromArgb(color.A, Math.Max(0, Math.Min(255, r)), Math.Max(0, Math.Min(255, g)), Math.Max(0, Math.Min(255, b)))
    End Function

    Private Sub SetDoubleBuffered(ctrl As Control)
        Dim prop = ctrl.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance Or BindingFlags.NonPublic)
        If prop IsNot Nothing Then
            prop.SetValue(ctrl, True, Nothing)
        End If
    End Sub
End Module

Public Class ModernColorTable
    Inherits ProfessionalColorTable

    Public Overrides ReadOnly Property UseSystemColors As Boolean
        Get
            Return False
        End Get
    End Property

    Public Overrides ReadOnly Property MenuStripGradientBegin As Color
        Get
            Return ModernTheme.SecondaryColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuStripGradientEnd As Color
        Get
            Return ModernTheme.SecondaryColor
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripDropDownBackground As Color
        Get
            Return ModernTheme.SecondaryColor
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientBegin As Color
        Get
            Return ModernTheme.SecondaryColor
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientMiddle As Color
        Get
            Return ModernTheme.SecondaryColor
        End Get
    End Property

    Public Overrides ReadOnly Property ImageMarginGradientEnd As Color
        Get
            Return ModernTheme.SecondaryColor
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelected As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, 0.15)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientBegin As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, 0.15)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemSelectedGradientEnd As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, 0.05)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientBegin As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, -0.05)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientMiddle As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, -0.1)
        End Get
    End Property

    Public Overrides ReadOnly Property MenuItemPressedGradientEnd As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, -0.15)
        End Get
    End Property

    Public Overrides ReadOnly Property ToolStripBorder As Color
        Get
            Return Color.Transparent
        End Get
    End Property

    Public Overrides ReadOnly Property MenuBorder As Color
        Get
            Return Color.Transparent
        End Get
    End Property

    Public Overrides ReadOnly Property SeparatorDark As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, -0.2)
        End Get
    End Property

    Public Overrides ReadOnly Property SeparatorLight As Color
        Get
            Return ModernTheme.AdjustColor(ModernTheme.SecondaryColor, 0.2)
        End Get
    End Property
End Class

Public Class ModernMenuRenderer
    Inherits ToolStripProfessionalRenderer

    Public Sub New()
        MyBase.New(New ModernColorTable())
    End Sub

    Protected Overrides Sub OnRenderToolStripBorder(e As ToolStripRenderEventArgs)
        ' Remove default border for a cleaner look
    End Sub

    Protected Overrides Sub OnRenderMenuItemBackground(e As ToolStripItemRenderEventArgs)
        If e.Item.Selected OrElse (TypeOf e.Item Is ToolStripMenuItem AndAlso DirectCast(e.Item, ToolStripMenuItem).DropDown.Visible) Then
            Using brush As New SolidBrush(ModernTheme.AdjustColor(ModernTheme.SecondaryColor, 0.15))
                e.Graphics.FillRectangle(brush, e.Item.ContentRectangle)
            End Using
        Else
            Using brush As New SolidBrush(Color.FromArgb(220, ModernTheme.SecondaryColor))
                e.Graphics.FillRectangle(brush, e.Item.ContentRectangle)
            End Using
        End If
    End Sub
End Class
