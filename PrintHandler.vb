Imports System.Drawing.Printing
Imports System.Security.Cryptography.X509Certificates

Public Class PrintHandler
    Private selectedPaperSize As PaperSize
    Private selectedPrinter As String

    Public GageID As String = GlobalVars.GageID
    Public CalDate As Date = GlobalVars.CalDate
    Public CalDue As Date = GlobalVars.DueDate
    Public PartNumber As String = GlobalVars.PartNumber
    Public CalBy As String = GlobalVars.CalBy

    Private Sub PrintHandler_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ShowPrinters()
        LabelSize()
    End Sub

    Private Sub ShowPrinters()
        cmbPrinters.Items.Clear()
        For Each printer As String In PrinterSettings.InstalledPrinters
            cmbPrinters.Items.Add(printer)
        Next

        If Not String.IsNullOrEmpty(My.Settings.Printer) AndAlso cmbPrinters.Items.Contains(My.Settings.Printer) Then
            cmbPrinters.SelectedItem = My.Settings.Printer
        ElseIf cmbPrinters.Items.Count > 0 Then
            cmbPrinters.SelectedIndex = 0
        End If
    End Sub

    Private Sub LabelSize()
        cmbPrintSize.Items.Clear()
        cmbPrintSize.Items.AddRange(New String() {"Large", "Medium", "Small"})

        If Not String.IsNullOrEmpty(My.Settings.PrintSize) AndAlso cmbPrintSize.Items.Contains(My.Settings.PrintSize) Then
            cmbPrintSize.SelectedItem = My.Settings.PrintSize
        Else
            cmbPrintSize.SelectedIndex = 0
        End If
    End Sub

    Private Sub cmbPrinters_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrinters.SelectedIndexChanged
        My.Settings.Printer = cmbPrinters.SelectedItem.ToString()
        My.Settings.Save()
    End Sub

    Private Sub cmbPrintSize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPrintSize.SelectedIndexChanged
        My.Settings.PrintSize = cmbPrintSize.SelectedItem.ToString()
        My.Settings.Save()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        selectedPrinter = cmbPrinters.SelectedItem?.ToString()
        If String.IsNullOrEmpty(selectedPrinter) Then
            MessageBox.Show("Please select a printer.")
            Return
        End If

        Dim gageFont As New Font("Arial", 20, FontStyle.Bold)
        Dim labelFont As New Font("Arial", 10, FontStyle.Bold)
        Dim valueFont As New Font("Arial", 10, FontStyle.Regular)

        ' Measure total required width
        Dim bmp As New Bitmap(1, 1)
        Dim g As Graphics = Graphics.FromImage(bmp)

        Dim contentWidth As Integer = 0
        contentWidth = Math.Max(contentWidth, g.MeasureString("ID: " & GageID, gageFont).Width)
        contentWidth += g.MeasureString("Part Number: " & PartNumber, valueFont).Width
        contentWidth += g.MeasureString("Cal Date: " & CalDate.ToShortDateString(), valueFont).Width
        contentWidth += g.MeasureString("Due Date: " & CalDue.ToShortDateString(), valueFont).Width
        contentWidth += g.MeasureString("Calibrated By: " & CalBy, valueFont).Width
        contentWidth += 60 ' extra padding

        Dim dpi As Single = g.DpiX
        Dim paperWidthInHundredths = CInt(contentWidth * 100 / dpi)
        Dim defaultLabelHeightInHundredths = 370 ' 94mm ≈ 3.7 inches

        Dim paperSize As New PaperSize("CustomLength", paperWidthInHundredths, defaultLabelHeightInHundredths)

        Dim printDoc As New PrintDocument()
        printDoc.PrinterSettings.PrinterName = selectedPrinter
        printDoc.DefaultPageSettings.PaperSize = paperSize
        printDoc.DefaultPageSettings.Landscape = True

        AddHandler printDoc.PrintPage, AddressOf Me.PrintPageHandler
        printDoc.Print()
    End Sub

    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        g.TranslateTransform(e.PageBounds.Width, 0)
        g.RotateTransform(90)

        Dim x As Integer = 10
        Dim y As Integer = 10

        Dim gageFont As New Font("Arial", 20, FontStyle.Bold)
        Dim labelFont As New Font("Arial", 10, FontStyle.Bold)
        Dim valueFont As New Font("Arial", 10, FontStyle.Regular)

        g.DrawString("ID: " & GageID, gageFont, Brushes.Black, x, y)
        y += gageFont.Height + 10
        g.DrawString("Part Number:", labelFont, Brushes.Black, x, y)
        y += labelFont.Height
        g.DrawString(PartNumber, valueFont, Brushes.Black, x, y)
        y += valueFont.Height + 10
        g.DrawString("Cal Date:", labelFont, Brushes.Black, x, y)
        y += labelFont.Height
        g.DrawString(CalDate.ToShortDateString(), valueFont, Brushes.Black, x, y)
        y += valueFont.Height + 10
        g.DrawString("Due Date:", labelFont, Brushes.Black, x, y)
        y += labelFont.Height
        g.DrawString(CalDue.ToShortDateString(), valueFont, Brushes.Black, x, y)
        y += valueFont.Height + 10
        g.DrawString("Calibrated By:", labelFont, Brushes.Black, x, y)
        y += labelFont.Height
        g.DrawString(CalBy, valueFont, Brushes.Black, x, y)
    End Sub




    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub
End Class
