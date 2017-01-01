Option Strict On
Imports C1.Win
Imports BOPr
Imports System.Drawing.Printing

Public Class frmReconcileCashRegister
    Inherits System.Windows.Forms.Form

    Private Enum RowName
        Hdr
        POSSale
        Tips
        TotalInFromSale
        NonCurrentPayments
        OpeningBalance
        SubTotalClosingBalance
        PettyCashAndDeposit
        CalculatedClosingBalance
        ActualCash
        ActualCreditCard
        ActualOther
        ActualClosingBalance
        ShortExcess
        MissingPayments
        OpenOrders
    End Enum


#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetUIComponents()
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
        Me.Text = "frmReconcileCashRegister"
    End Sub

#End Region

    Private WithEvents dtePicker As DateTimePicker
    Private pnlStep1 As Panel
    Private pnlStep2 As Panel
    Private lblPanel1 As Label
    Private WithEvents fgDailyReport As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents btnOk As Button
    Private WithEvents btnPrint As Button
    Private WithEvents btnPreview As Button
    Private WithEvents btnApprove As Button
    Private WithEvents btnCancelApprove As Button

    Private WithEvents m_SaleDaySession As SaleDaySession
    Private m_SaleDay As SaleDay

    Private Sub frmReconcileCashRegister_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SaleDaySelected = SaleDays.CreateSaleDay.Item(dtePicker.Value.Date, False)
    End Sub

    Private Sub dtePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtePicker.ValueChanged
        'SaleDaySelected = SaleDays.CreateSaleDay.Item(dtePicker.Value.Date, False)
    End Sub

    Private Sub dtePicker_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtePicker.CloseUp
        SaleDaySelected = SaleDays.CreateSaleDay.Item(dtePicker.Value.Date, False)
    End Sub

    Private Property SaleDaySelected() As SaleDay
        Get
            Return m_SaleDay
        End Get
        Set(ByVal Value As SaleDay)
            m_SaleDay = Value
            If m_SaleDay Is Nothing Then
                'show empty grid
                LoadDailyReportFixedData()
            Else
                fgDailyReport.Clear(C1.Win.C1FlexGrid.ClearFlags.Content)
                LoadDailyReportFixedData()
                LoadReconciliationData()
            End If
            EnableDisableFormBtns()
        End Set
    End Property

    Private Property SDSessionSelectedForApproval() As SaleDaySession
        Get
            Return m_SaleDaySession
        End Get
        Set(ByVal Value As SaleDaySession)
            m_SaleDaySession = Value
        End Set
    End Property

    Private Sub OnOverrideNeeded(ByVal objOverride As Override) Handles m_SaleDaySession.OverrideNeeded
        'create override form, pass override object and get override from user
        Dim overrideResult As DialogResult
        Dim newfrmPOSOverride As New frmPOSOverride()
        newfrmPOSOverride.newOverride = objOverride
        overrideResult = newfrmPOSOverride.ShowDialog()
    End Sub

    Private Sub SetUIComponents()
        With Me
            .AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            .ClientSize = New System.Drawing.Size(932, 680)
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .MaximizeBox = False
            .MinimizeBox = False
            .Name = "frmReconcileCashRegister"
            .ShowInTaskbar = False
            .Text = "Reconcile Cash Register"
            SetStep1PanelProperties()
            SetStep2PanelProperties()
            .Controls.AddRange(New Control() {pnlStep1, pnlStep2})
        End With
    End Sub

    Private Sub SetStep1PanelProperties()
        pnlStep1 = New Panel()
        With pnlStep1
            .SuspendLayout()
            .BackColor = Color.SandyBrown
            .Location = New System.Drawing.Point(0, 0)
            .Dock = System.Windows.Forms.DockStyle.Top
            .Width = Me.ClientSize.Width
            .Height = 30
            .TabIndex = 0
            .BorderStyle = BorderStyle.Fixed3D
        End With

        SetPanel1LabelProperties()
        SetDatePickerProperties()

        With pnlStep1
            .Controls.AddRange(New Control() {lblPanel1, dtePicker})
            .ResumeLayout()
        End With
    End Sub

    Private Sub SetPanel1LabelProperties()
        lblPanel1 = New Label()

        With lblPanel1
            .Size = New System.Drawing.Size(240, 20)
            .Location = New System.Drawing.Point(CInt(pnlStep1.Width / 3), 5)
            .BackColor = Color.SandyBrown
            .Text = " Cash Register Reconcilation "
            .TextAlign = ContentAlignment.MiddleRight
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
    End Sub

    Private Sub SetDatePickerProperties()
        dtePicker = New DateTimePicker()

        With dtePicker
            .Size = New System.Drawing.Size(120, 20)
            .Location = New System.Drawing.Point((CInt(pnlStep1.Width / 3) + lblPanel1.Width + 10), 5)
            .CalendarMonthBackground = Color.FromKnownColor(KnownColor.Control)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Format = DateTimePickerFormat.Short
            .Visible = True
            .TabIndex = 0
        End With
    End Sub

    Private Sub SetStep2PanelProperties()
        pnlStep2 = New Panel()
        With pnlStep2
            .SuspendLayout()
            .BackColor = Color.Ivory
            .Dock = System.Windows.Forms.DockStyle.Fill
            .Size = New System.Drawing.Size(Me.ClientSize.Width, Me.ClientSize.Height - (20))
            .Location = New System.Drawing.Point(0, 30)
            .BorderStyle = BorderStyle.Fixed3D
        End With

        SetDailyReportGrid()
        SetOKButtonProperties()
        SetPrintButtonProperties()
        SetPreviewButtonProperties()
        SetApproveButtonProperties()
        SetCancelApproveButtonProperties()

        With pnlStep2
            .Controls.AddRange(New Control() {fgDailyReport, btnCancelApprove, btnApprove, btnPreview, btnPrint, btnOk})
            .ResumeLayout()
            .Visible = True
        End With

    End Sub

    Private Sub SetDailyReportGrid()
        Dim ShortExcesstCellStyle As C1.Win.C1FlexGrid.CellStyle
        Dim ClosingBalanceStyle As C1.Win.C1FlexGrid.CellStyle
        fgDailyReport = New C1.Win.C1FlexGrid.C1FlexGrid()

        ClosingBalanceStyle = fgDailyReport.Styles.Add("closingbalance")
        With ClosingBalanceStyle
            .BackColor = Color.BurlyWood
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
        ShortExcesstCellStyle = fgDailyReport.Styles.Add("ShortExcesstCellStyle")
        With ShortExcesstCellStyle

            .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Moccasin
            .ForeColor = Color.Black

        End With
        With fgDailyReport
            .Size = New System.Drawing.Size(725, 423)
            .Location = New System.Drawing.Point(100, 100)
            .ScrollBars = ScrollBars.None
            .Cols.Count = 5
            .Cols.Fixed = 1
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.FixedOnly
            .Cols.Item(0).AllowMerging = True
            .Rows.Count = 16
            .Rows.Fixed = 1
            .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Raised
            .Styles.Fixed.BackColor = Color.BlanchedAlmond
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = False
            .AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
            .Styles.Alternate.BackColor = Color.AntiqueWhite
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .Rows.Item(RowName.ShortExcess).AllowMerging = True
            .Rows.Item(RowName.ShortExcess).Style = ShortExcesstCellStyle
            .Rows.Item(RowName.CalculatedClosingBalance).Height = 48
            .Rows.Item(RowName.ActualClosingBalance).Height = 48
            .Rows.Item(RowName.ShortExcess).Height = 48
            fgDailyReport.Rows.Item(RowName.CalculatedClosingBalance).Style = ClosingBalanceStyle
            fgDailyReport.Rows.Item(RowName.ActualClosingBalance).Style = ClosingBalanceStyle

        End With

        With fgDailyReport.Cols(0)
            .Width = 120
            .WidthDisplay = 120
            .Caption = ""
            .Name = "RowGroupHdr"
            .DataType = GetType(String)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .AllowEditing = False
            .AllowMerging = True
        End With

        With fgDailyReport.Cols(1)
            .Width = 270
            .WidthDisplay = 270
            .Caption = ""
            .Name = "RowHdr"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowMerging = True
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
        End With

        With fgDailyReport.Cols(2)
            .Width = 110
            .WidthDisplay = 110
            .Caption = "   Lunch"
            .Name = "Lunch"
            .DataType = GetType(Double)
            .Format = "$#,##0.00"
            .AllowEditing = False

        End With

        With fgDailyReport.Cols(3)
            .Width = 110
            .WidthDisplay = 110
            .Caption = "   Dinner"
            .Name = "Dinner"
            .DataType = GetType(Double)
            .Format = "$#,##0.00"
            .AllowEditing = False

        End With

        With fgDailyReport.Cols(4)
            .Width = 110
            .WidthDisplay = 110
            .Caption = "   Day"
            .Name = "Day"
            .DataType = GetType(Double)
            .Format = "$#,##0.00"
            .AllowEditing = False
        End With
    End Sub

    Private Sub LoadDailyReportFixedData()
        With fgDailyReport
            .Item(RowName.POSSale, 0) = "As Per POS"
            .Item(RowName.POSSale, 1) = " POS Sale"
            .Item(RowName.Tips, 0) = "As Per POS"
            .Item(RowName.Tips, 1) = " + Tips "
            .Item(RowName.TotalInFromSale, 0) = "As Per POS"
            .Item(RowName.TotalInFromSale, 1) = " Total in from Sale"
            .Item(RowName.NonCurrentPayments, 0) = "As Per POS"
            .Item(RowName.NonCurrentPayments, 1) = " + Past Or Future Sale Payments"
            .Item(RowName.OpeningBalance, 0) = "As Per POS"
            .Item(RowName.OpeningBalance, 1) = "+ Opening Balance"
            .Item(RowName.SubTotalClosingBalance, 0) = "As Per POS"
            .Item(RowName.SubTotalClosingBalance, 1) = " Sub Total Closing Balance"
            .Item(RowName.PettyCashAndDeposit, 0) = "As Per POS"
            .Item(RowName.PettyCashAndDeposit, 1) = "Less: Petty Cash & Deposit"
            .Item(RowName.CalculatedClosingBalance, 0) = "As Per POS"
            .Item(RowName.CalculatedClosingBalance, 1) = " Calculated Closing Balance"
            .Item(RowName.ActualCash, 0) = "Actual"
            .Item(RowName.ActualCash, 1) = "Cash"
            .Item(RowName.ActualCreditCard, 0) = "Actual"
            .Item(RowName.ActualCreditCard, 1) = " Credit Card"
            .Item(RowName.ActualOther, 0) = "Actual"
            .Item(RowName.ActualOther, 1) = " Other"
            .Item(RowName.ActualClosingBalance, 0) = "Actual"
            .Item(RowName.ActualClosingBalance, 1) = " Actual Closing Balance"
            .Item(RowName.ShortExcess, 0) = " Short/Excess "
            .Item(RowName.ShortExcess, 1) = " Short/Excess "
            .Item(RowName.OpenOrders, 0) = " Short/Excess "
            .Item(RowName.OpenOrders, 1) = " Open Orders "
            .Item(RowName.MissingPayments, 0) = " Short/Excess "
            .Item(RowName.MissingPayments, 1) = " Missing Payments "
        End With
        fgDailyReport.Cols(2).Caption = "    Lunch"
        fgDailyReport.Cols(3).Caption = "   Dinner"
        fgDailyReport.Cols(4).Caption = "      Day"
    End Sub

    Private Sub LoadReconciliationData()
        Dim i As Integer
        Dim enumSession As Session.enumSessionName

        For i = 2 To 4      'loop for cols 2,3 & 4
            Select Case i
                Case 2
                    enumSession = Session.enumSessionName.Lunch
                Case 3
                    enumSession = Session.enumSessionName.Dinner
                Case 4
                    enumSession = Session.enumSessionName.Day
            End Select
            LoadPOSData(enumSession)
            LoadActualBalances(enumSession)
        Next
    End Sub

    Private Sub LoadPOSData(ByVal enumSession As Session.enumSessionName)
        Dim fgcol As Integer
        Dim objSaleDaySession As SaleDaySession
        Dim fromTime As DateTime
        Dim ToTime As DateTime

        Select Case enumSession
            Case Session.enumSessionName.Lunch, Session.enumSessionName.Dinner
                objSaleDaySession = SaleDaySelected.AllSaleDaySessions.Item(enumSession)
                fromTime = objSaleDaySession.SaleDaySessionFrom
                ToTime = objSaleDaySession.SaleDaySessionTo
                If ToTime = DateTime.MinValue Then ToTime = SaleDays.CreateSaleDay.Now
                'calculate all payments received today from sale made today (current)
                SaleDaySelected.SetAllCurrentPayments(fromTime, ToTime)
                'calculate all payments received today from sale made on all days except today (noncurrent)
                SaleDaySelected.SetAllNonCurrentPayments(fromTime, ToTime)

                Select Case enumSession
                    Case Session.enumSessionName.Lunch
                        fgcol = 2
                    Case Session.enumSessionName.Dinner
                        fgcol = 3
                End Select
                fgDailyReport.Item(Me.RowName.POSSale, fgcol) = objSaleDaySession.POSSaleAmount(fromTime, ToTime)
                fgDailyReport.Item(Me.RowName.Tips, fgcol) = objSaleDaySession.TipAmountPaid(fromTime, ToTime)
                fgDailyReport.Item(Me.RowName.PettyCashAndDeposit, fgcol) = SaleDaySelected.CRTXnAmount(fromTime, ToTime)
                fgDailyReport.Item(Me.RowName.NonCurrentPayments, fgcol) = SaleDaySelected.NonCurrentCashTotalAmount _
                                                                           + SaleDaySelected.NonCurrentCreditCardTotalAmount _
                                                                           + SaleDaySelected.NonCurrentOtherTotalAmount
                fgDailyReport.Item(Me.RowName.MissingPayments, fgcol) = SaleDaySelected.MissingPaymentTotalAmountForDay
                fgDailyReport.Item(Me.RowName.OpenOrders, fgcol) = SaleDaySelected.OpenOrdersPOSAmount(ToTime)
            Case Session.enumSessionName.Day
                'fromTime = SaleDaySelected.SaleDate
                'ToTime = SaleDaySelected.SaleDate.AddDays(1)
                fromTime = SaleDaySelected.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom
                ToTime = SaleDaySelected.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo
                If ToTime = DateTime.MinValue Then ToTime = SaleDays.CreateSaleDay.Now
                'calculate all payments received today from sale made today (current)
                SaleDaySelected.SetAllCurrentPayments(fromTime, ToTime)
                'calculate all payments received today from sale made on all days except today (noncurrent)
                SaleDaySelected.SetAllNonCurrentPayments(fromTime, ToTime)

                fgcol = 4
                fgDailyReport.Item(Me.RowName.POSSale, fgcol) = SaleDaySelected.POSSaleAmountForADay
                fgDailyReport.Item(Me.RowName.Tips, fgcol) = SaleDaySelected.TotalTipForADay()
                fgDailyReport.Item(Me.RowName.PettyCashAndDeposit, fgcol) = SaleDaySelected.CRTXnAmount(fromTime, ToTime)
                fgDailyReport.Item(Me.RowName.NonCurrentPayments, fgcol) = SaleDaySelected.NonCurrentCashTotalAmount _
                                                                            + SaleDaySelected.NonCurrentCreditCardTotalAmount _
                                                                            + SaleDaySelected.NonCurrentOtherTotalAmount
                fgDailyReport.Item(Me.RowName.MissingPayments, fgcol) = SaleDaySelected.MissingPaymentTotalAmountForDay
                fgDailyReport.Item(Me.RowName.OpenOrders, fgcol) = SaleDaySelected.OpenOrdersPOSAmount(ToTime)
        End Select
        fgDailyReport.Item(Me.RowName.TotalInFromSale, fgcol) = CDbl(fgDailyReport.Item(Me.RowName.POSSale, fgcol)) _
                                                                + CDbl(fgDailyReport.Item(Me.RowName.Tips, fgcol))
    End Sub

    Private Sub LoadActualBalances(ByVal enumSession As Session.enumSessionName)
        Dim objSaleDaySession As SaleDaySession
        Dim objSDSessionCrBalance As CRBalance
        Dim objMoneyCounts As MoneyCounts
        Dim objMoneyCount As MoneyCount
        Dim fgCol As Integer

        'first load the POS Balance data
        Select Case enumSession
            Case Session.enumSessionName.Lunch, Session.enumSessionName.Dinner
                objSaleDaySession = SaleDaySelected.AllSaleDaySessions.Item(enumSession)
                objMoneyCounts = objSaleDaySession.AllMoneyCounts
                Select Case enumSession
                    Case Session.enumSessionName.Lunch
                        fgCol = 2
                    Case Session.enumSessionName.Dinner
                        fgCol = 3
                End Select
            Case Session.enumSessionName.Day
                fgCol = 4
                'For Day, op balance is that for Lunch
                objSaleDaySession = SaleDaySelected.AllSaleDaySessions.Item(Session.enumSessionName.Lunch)
                objMoneyCounts = objSaleDaySession.AllMoneyCounts
        End Select

        objMoneyCount = objMoneyCounts.Item(MoneyCount.enumMoneyCountType.Opening)

        With objMoneyCount
            fgDailyReport.Item(Me.RowName.OpeningBalance, fgCol) = .TotalAmount
            fgDailyReport.Item(Me.RowName.SubTotalClosingBalance, fgCol) = CDbl(fgDailyReport.Item(Me.RowName.TotalInFromSale, fgCol)) _
                                                                            + CDbl(fgDailyReport.Item(Me.RowName.NonCurrentPayments, fgCol)) _
                                                                            + CDbl(fgDailyReport.Item(Me.RowName.OpeningBalance, fgCol))
            fgDailyReport.Item(Me.RowName.CalculatedClosingBalance, fgCol) = CDbl(fgDailyReport.Item(Me.RowName.SubTotalClosingBalance, fgCol)) - CDbl(fgDailyReport.Item(Me.RowName.PettyCashAndDeposit, fgCol))
        End With

        'now load the Actual Balances part of the grid
        Select Case enumSession
            Case Session.enumSessionName.Lunch, Session.enumSessionName.Dinner
                'nop: all objects required to reach closing moneycount are same as that for opening 
            Case Session.enumSessionName.Day
                fgCol = 4
                'For Day, closing balance is that for Dinner
                objSaleDaySession = SaleDaySelected.AllSaleDaySessions.Item(Session.enumSessionName.Dinner)
                objMoneyCounts = objSaleDaySession.AllMoneyCounts
        End Select
        objMoneyCount = objMoneyCounts.Item(MoneyCount.enumMoneyCountType.Closing)
        With objMoneyCount
            fgDailyReport.Item(Me.RowName.ActualCash, fgCol) = objMoneyCount.TotalCashAmount
            fgDailyReport.Item(Me.RowName.ActualCreditCard, fgCol) = objMoneyCount.TotalCreditCardAmount()
            fgDailyReport.Item(Me.RowName.ActualOther, fgCol) = objMoneyCount.TotalCheckAmount
            fgDailyReport.Item(Me.RowName.ActualClosingBalance, fgCol) = objMoneyCount.TotalAmount
            fgDailyReport.Item(Me.RowName.ShortExcess, fgCol) = CDbl(fgDailyReport.Item(Me.RowName.ActualClosingBalance, fgCol)) - CDbl(fgDailyReport.Item(Me.RowName.CalculatedClosingBalance, fgCol))
        End With

    End Sub

    Private Sub SetOKButtonProperties()
        btnOk = New Button()
        With btnOk
            .Location = New System.Drawing.Point(fgDailyReport.Location.X + 298, fgDailyReport.Location.Y + fgDailyReport.Height + 20)
            .Size = New System.Drawing.Size(65, 35)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, _
                        System.Drawing.FontStyle.Bold, _
                         System.Drawing.GraphicsUnit.Point, _
                         CType(0, Byte))
            .Text = "Close"
            .Enabled = True
            .Visible = True
            '.TabIndex = 7
        End With
    End Sub

    Private Sub SetPrintButtonProperties()
        btnPrint = New Button()
        With btnPrint
            .Location = New System.Drawing.Point(btnOk.Location.X + btnOk.Width + 10, btnOk.Location.Y)
            .Size = New System.Drawing.Size(75, 35)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Print"
            .Visible = True
        End With
    End Sub

    Private Sub SetPreviewButtonProperties()
        btnPreview = New Button()
        With btnPreview
            .Location = New System.Drawing.Point(btnPrint.Location.X + btnPrint.Width + 10, btnOk.Location.Y)
            .Size = New System.Drawing.Size(75, 35)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Preview"
            .Visible = True
        End With
    End Sub

    Private Sub SetApproveButtonProperties()
        btnApprove = New Button()
        With btnApprove
            .Location = New System.Drawing.Point(btnPreview.Location.X + btnPreview.Width + 10, btnOk.Location.Y)
            .Size = New System.Drawing.Size(75, 35)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Approve"
            .Visible = True
        End With

    End Sub

    Private Sub SetCancelApproveButtonProperties()
        btnCancelApprove = New Button()
        With btnCancelApprove
            .Location = New System.Drawing.Point(btnApprove.Location.X + btnApprove.Width + 10, btnOk.Location.Y)
            .Size = New System.Drawing.Size(75, 35)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Cancel Approve"
            .Visible = True
        End With

    End Sub

    'Private Function GetCrBalanceForSession(ByVal objSaleDaySession As SaleDaySession) As CRBalance
    '    Dim objSaleDay As SaleDay
    '    Dim objSaleDaysessions As SaleDaySessions
    '    Dim objSession As Session
    '    Dim objCrbalances As CRBalances
    '    Dim objCrbalance As CRBalance

    '    If objSaleDaySession Is Nothing Then
    '        'nope
    '    Else
    '        For Each objCrbalance In objSaleDaySession.CrBalances
    '            If objCrbalance.CRBalanceSaleDate = dtePicker.Value.Date And _
    '                 objCrbalance.SessionID = objSaleDaySession.SessionId Then
    '                Exit For
    '            Else
    '                objCrbalance = Nothing
    '            End If
    '        Next
    '    End If
    '    Return objCrbalance
    'End Function

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        SaleDaySelected = Nothing
        SDSessionSelectedForApproval = Nothing
        Me.Close()
    End Sub

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim gp As C1.Win.C1FlexGrid.GridPrinter
        Dim dlg As PrintDialog
        Dim printdoc As PrintDocument
        Dim printset As PrinterSettings
        gp = fgDailyReport.PrintParameters
        With gp
            .Header = " Cash Register Reconciliation Report For " + dtePicker.Value.ToLongDateString + ControlChars.NewLine + " Done By : " + _
                         ControlChars.NewLine + " Done At :"

            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)

            .Footer = gp.PageNumber.ToString + "of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)

        End With

        fgDailyReport.PrintGrid(gp.PrintDocument.ToString, C1.Win.C1FlexGrid.PrintGridFlags.FitToPage)
    End Sub

    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim gp As C1.Win.C1FlexGrid.GridPrinter
        Dim dlg As PrintPreviewDialog
        Dim printdoc As PrintDocument

        gp = fgDailyReport.PrintParameters

        gp.Header = " Cash Register Reconciliation Report For " + dtePicker.Value.ToLongDateString + ControlChars.NewLine + " Done By : " + _
                    ControlChars.NewLine + " Done At :"

        gp.HeaderFont = New Font("Ariel", 13, FontStyle.Bold)

        gp.Footer = gp.PageNumber.ToString
        gp.FooterFont = New Font("Ariel", 12, FontStyle.Bold)
        gp.PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPageWidth


        dlg = New PrintPreviewDialog()

        dlg.Document = gp.PrintDocument

        dlg.ShowDialog()
    End Sub

    Private Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        'Approving any session which is not approved for the selected day
        Dim objSaleDaySession As SaleDaySession

        For Each objSaleDaySession In SaleDaySelected.AllSaleDaySessions
            If objSaleDaySession.ReconciliationApprovedAt <= Date.MinValue Then
                SDSessionSelectedForApproval = objSaleDaySession
                Select Case m_SaleDaySession.AllOrdersClosed
                    Case True
                        SDSessionSelectedForApproval.getApproval()

                    Case False
                        MsgBox("There are some orders which are either unpaid or payment not entered. You need to " & _
                                " close those orders first before reconciling the cash register", MsgBoxStyle.Information)
                End Select

            End If
            SDSessionSelectedForApproval = Nothing
        Next

    End Sub

    Private Sub btnCancelApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelApprove.Click
        'Cancel both Lunch and Dinner Approvals

        Dim objSaleDaySession As SaleDaySession

        For Each objSaleDaySession In SaleDaySelected.AllSaleDaySessions
            If objSaleDaySession.ReconciliationApprovedAt > Date.MinValue Then
                SDSessionSelectedForApproval = objSaleDaySession
                SDSessionSelectedForApproval.CancelApproval()
            End If
        Next
    End Sub

    Private Sub EnableDisableFormBtns()
        If SaleDaySelected Is Nothing Then
            btnApprove.Enabled = False
            btnCancelApprove.Enabled = False
            btnOk.Enabled = False
            btnPrint.Enabled = False
            btnPreview.Enabled = False
        Else
            btnApprove.Enabled = True
            btnCancelApprove.Enabled = True
            btnOk.Enabled = True
            btnPrint.Enabled = True
            btnPreview.Enabled = True
        End If
    End Sub


End Class

