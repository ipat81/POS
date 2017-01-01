Option Strict On
Imports BOPr
Imports C1.Win.C1FlexGrid
Imports System.Drawing.Printing

Public Class frmFGReportViewer
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        Me.Text = "frmFGReportViewer"
    End Sub

#End Region
    Friend Enum enumReport As Integer
        DailySalesSummary
        WaiterPerformance
        ProductPerformance
        FutureOrdersList
        OverridesList
        ProductsList
        CurrentMenuList
        QBPayrollReport
        QBSalesReport
        LateTablesList
        QBRevenueReport
        FixPayroll
    End Enum

    Private pnlSelector As Panel
    Private pnlfg As Panel
    Private lblReport As Label

    Private splitter1 As Splitter
    ' Friend WithEvents fgReport As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents dtePickerDateFrom As DateTimePicker
    Friend WithEvents dtePickerDateTo As DateTimePicker
    Private WithEvents btnRunReport As Button
    Private WithEvents btnPrint As Button
    Private WithEvents btnClose As Button
    Private lblDateFrom As New Label()

    Private m_Report As enumReport
    Private WithEvents m_ReportGrid As C1FlexGrid

    Private m_DateFrom As Date
    Private m_DateTo As Date
    Private m_AllowFutureDates As Boolean

    Friend Event RunReport()
    ' Friend Event GridClick(ByVal sender As Object, ByVal e As System.EventArgs)

    Private Sub SetUI()
        Me.components = New System.ComponentModel.Container()
        Me.splitter1 = New System.Windows.Forms.Splitter()
        Me.pnlSelector = New System.Windows.Forms.Panel()
        Me.pnlfg = New System.Windows.Forms.Panel()
        Me.Size = New System.Drawing.Size(1200, 900)
        With Me.pnlSelector

            .SuspendLayout()
            .Dock = System.Windows.Forms.DockStyle.Left
            .Width = Me.ClientSize.Width \ 5
            .Height = Me.ClientSize.Height
            .TabIndex = 0
            .BorderStyle = BorderStyle.Fixed3D
            SetpnlSelectorUI()

        End With

        With Me.pnlfg
            .Dock = System.Windows.Forms.DockStyle.Fill
            .TabIndex = 1
            .BorderStyle = BorderStyle.Fixed3D
            SetpnlfgUI()
        End With

        With Me.splitter1
            .Location = New System.Drawing.Point(pnlSelector.Width, 0)
            .Width = 3
            .BackColor = Color.LightSteelBlue
            .BorderStyle = BorderStyle.Fixed3D
            ' Set TabStop to false for ease of use when negotiating
            ' the user interface.
            .TabStop = False
        End With

        ' The order of the controls in this call is critical.
        With Me
            .Size = New System.Drawing.Size(1200, 900)
            .Controls.AddRange(New System.Windows.Forms.Control() _
                   {Me.pnlfg, Me.splitter1, Me.pnlSelector})
            .AutoScroll = False
            .ControlBox = False
            .FormBorderStyle = FormBorderStyle.None
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
    End Sub

    Private Sub SetpnlSelector()
        Dim lblHdrSelector As New Label()
        With lblHdrSelector
            .Text = " "
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.MiddleCenter
            .BackColor = Color.SandyBrown
            .Dock = DockStyle.Top
            .Height = 20
        End With
    End Sub

    'Private Sub Setpnlfg()
    '    Dim lblHdrpnlfg As New Label()
    '    With lblHdrpnlfg
    '        .Text = " "
    '        .TextAlign = ContentAlignment.MiddleCenter
    '        .BackColor = Color.SandyBrown
    '        .Dock = DockStyle.Top
    '        .Height = 20
    '    End With
    'End Sub

    Private Sub SetpnlSelectorUI()
        Dim lblSelector As New Label()
        ' Dim lblDateFrom As New Label()
        Dim lblDateTo As New Label()

        btnRunReport = New Button()
        btnPrint = New Button()
        btnClose = New Button()
        dtePickerDateFrom = New DateTimePicker()
        dtePickerDateTo = New DateTimePicker()

        With lblSelector
            .Size = New System.Drawing.Size(pnlSelector.Width, 35)
            .BackColor = Color.SandyBrown
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = " Select Report Parameters"
        End With

        With lblDateFrom
            .Size = New System.Drawing.Size(pnlSelector.Width - 80, 35)
            .Location = New Point(lblSelector.Location.X + 10, lblSelector.Location.Y + lblSelector.Height + 40)
            '.BackColor = Color.SandyBrown
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Select Date Report starts From"
        End With
        With dtePickerDateFrom
            .Size = New System.Drawing.Size(lblDateFrom.Width, 25)
            .Location = New Point(lblDateFrom.Location.X, lblDateFrom.Location.Y + lblDateFrom.Height + 5)
            .CalendarMonthBackground = Color.FromKnownColor(KnownColor.Control)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Format = DateTimePickerFormat.Short
            .Name = "dtePickerDateFrom"
            .Visible = True
            If AllowFutureDates = False Then .MaxDate = Today
        End With

        With lblDateTo
            .Size = New System.Drawing.Size(lblDateFrom.Width, 25)
            .Location = New Point(dtePickerDateFrom.Location.X, dtePickerDateFrom.Location.Y + 40)
            '.BackColor = Color.SandyBrown
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Select Last Date Report Includes"
        End With
        With dtePickerDateTo
            .Size = New System.Drawing.Size(lblDateFrom.Width, 25)
            .Location = New Point(lblDateTo.Location.X, lblDateTo.Location.Y + lblDateTo.Height + 5)
            .CalendarMonthBackground = Color.FromKnownColor(KnownColor.Control)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Format = DateTimePickerFormat.Short
            If AllowFutureDates = False Then .MaxDate = Today
            .Name = "dtePickerDateTo"
        End With

        With btnRunReport
            getButtonProperties(btnRunReport)
            .Text = "Run Report"
            .Location = New Point(lblDateTo.Location.X, dtePickerDateTo.Location.Y + dtePickerDateTo.Height + 50)
        End With

        With btnPrint
            getButtonProperties(btnPrint)
            .Text = "Print"
            .Location = New Point(btnRunReport.Location.X, btnRunReport.Location.Y + btnRunReport.Height + 25)
        End With

        With btnClose
            getButtonProperties(btnClose)
            .Text = "Close"
            .Location = New Point(btnPrint.Location.X + btnPrint.Size.Width + 15, btnPrint.Location.Y)
        End With


        pnlSelector.Controls.AddRange(New Control() _
          {lblSelector, lblDateFrom, dtePickerDateFrom, lblDateTo, dtePickerDateTo, _
          btnRunReport, btnPrint, btnClose})
        pnlSelector.ResumeLayout()

    End Sub

    Private Sub SetpnlfgUI()
        lblReport = New Label()
        With lblReport
            GetLabelProperties(lblReport)
            .Height = 31
            .Dock = DockStyle.Top
            .Text = ReportName

            pnlfg.Controls.AddRange(New Control() {lblReport})
        End With
    End Sub

    Private Sub getButtonProperties(ByVal btn As Button)
        With btn
            .Size = New System.Drawing.Size(75, 50)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, _
                        System.Drawing.FontStyle.Bold, _
                         System.Drawing.GraphicsUnit.Point, _
                         CType(0, Byte))
            .Enabled = True
            .Visible = True
        End With
    End Sub

    Private Sub GetLabelProperties(ByVal lbl As Label)
        With lbl
            .BackColor = Color.SandyBrown
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
    End Sub

    'Private Sub getdtePickerProperties(ByVal dtePicker As DateTimePicker)
    '    With dtePicker
    '        .Size = New System.Drawing.Size(lbldatefrom.width, 25)
    '        .CalendarMonthBackground = Color.FromKnownColor(KnownColor.Control)
    '        .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    '        .Format = DateTimePickerFormat.Short
    '        If AllowFutureDates = False Then .MaxDate = Today
    '    End With
    'End Sub

    Private Sub frmFGReportViewer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetUI()
        EnableDisableFromDatePicker()
    End Sub

    Private Sub EnableDisableFromDatePicker()
        'Select Case Report
        '    Case enumReport.FutureOrdersList
        '        dtePickerDateFrom.Visible = False
        '        lblDateFrom.Visible = False
        '    Case Else
        '        dtePickerDateFrom.Visible = True
        '        lblDateFrom.Visible = True
        'End Select
    End Sub

    Friend Property Report() As enumReport
        Get
            Return m_Report
        End Get
        Set(ByVal Value As enumReport)
            m_Report = Value
            If lblReport Is Nothing Then
                'nop
            Else
                lblReport.Text = ReportName
            End If

        End Set
    End Property

    Public ReadOnly Property ReportName() As String
        Get
            Select Case Report
                Case enumReport.CurrentMenuList
                    Return "Menu Items List - Menu # " '& menuname
                Case enumReport.DailySalesSummary
                    Return "Detailed Sale By Order - From " & _
                    DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
                Case enumReport.FutureOrdersList
                    Return "Future Orders - From " & _
                 " Now To " & DateTo.ToShortDateString
                Case enumReport.OverridesList
                    Return "Daily Overrides Summary - From " & _
                    DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
                Case enumReport.ProductPerformance
                    Return "Daily Sales Summary - From " & _
                    DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
                Case enumReport.QBPayrollReport
                    Return "Payroll Clock Report - From " & _
                    DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
                Case enumReport.QBSalesReport
                    Return "QB Sales Report - From " & _
                    DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
                Case enumReport.WaiterPerformance
                    Return "Waiter Performance Summary - From " & _
                    DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString

                Case enumReport.LateTablesList
                    Return "List of Tables Closed After CashCount - From " & _
                  DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
                Case enumReport.QBRevenueReport
                    Return "Retai Sales Summary For QB - From " & _
                  DateFrom.ToShortDateString & " To " & DateTo.ToShortDateString
            End Select
        End Get
    End Property

    Friend Property ReportGrid() As C1FlexGrid
        Get
            Return m_ReportGrid
        End Get
        Set(ByVal Value As C1FlexGrid)
            m_ReportGrid = Value
            pnlfg.Controls.AddRange(New Control() {m_ReportGrid, lblReport})
        End Set
    End Property

    Friend Property DateFrom() As Date
        Get
            Return m_DateFrom
        End Get
        Set(ByVal Value As Date)
            'If DateTo = Date.MinValue Then
            '    m_DateFrom = Value.Date
            'End If

            'If Value <= DateTo Then
            m_DateFrom = Value.Date
            dtePickerDateTo.MinDate = Value.Date
            If lblReport Is Nothing Then
                'nop
            Else
                lblReport.Text = ReportName
            End If
            'End If
        End Set
    End Property

    Friend Property DateTo() As Date
        Get
            Return m_DateTo
        End Get
        Set(ByVal Value As Date)
            'If Value >= DateFrom Then
            m_DateTo = Value.Date
            dtePickerDateFrom.MaxDate = Value.Date
            If lblReport Is Nothing Then
                'nop
            Else
                lblReport.Text = ReportName
            End If
            'End If
        End Set
    End Property

    Friend Property AllowFutureDates() As Boolean
        Get
            Return m_AllowFutureDates
        End Get
        Set(ByVal Value As Boolean)
            m_AllowFutureDates = Value
        End Set
    End Property

    Private Sub btnRunReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRunReport.Click
        Me.Cursor = Cursors.WaitCursor
        If DateFrom <= Date.MinValue Then DateFrom = dtePickerDateFrom.Value.Date
        If DateTo <= Date.MinValue Then DateTo = dtePickerDateTo.Value.Date
        RaiseEvent RunReport()
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub dtePickerDateFrom_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtePickerDateFrom.ValueChanged
        DateFrom = dtePickerDateFrom.Value.Date
    End Sub

    Private Sub dtePickerDateTo_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtePickerDateTo.ValueChanged
        DateTo = dtePickerDateTo.Value.Date
    End Sub
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim gp As GridPrinter()
        Dim dlg As PrintDialog
        Dim printdoc As PrintDocument
        Dim printset As PrinterSettings


        With ReportGrid.PrintParameters
            .Header = ReportName & _
                         ControlChars.NewLine & "Printed on:" & DateTo.AddDays(3).ToLongDateString
            '                        ControlChars.NewLine & "Printed at:" & DateTime.Now.ToLongDateString

            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = .PageNumber.ToString + "of " + .PageCount.ToString
            .PrintDocument.PrinterSettings.DefaultPageSettings.Margins.Left = 1
            .PrintDocument.PrinterSettings.DefaultPageSettings.Margins.Right = 1

            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            Select Case Report
                Case enumReport.QBRevenueReport
                    .PrintDocument.DefaultPageSettings.Landscape = False
                Case Else
                    .PrintDocument.DefaultPageSettings.Landscape = True
            End Select

        End With

        ReportGrid.PrintGrid(Me.Report.ToString, C1.Win.C1FlexGrid.PrintGridFlags.ActualSize)

    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub


End Class

