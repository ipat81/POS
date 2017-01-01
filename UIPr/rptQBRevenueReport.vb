Option Strict On
Imports BOPr
Imports C1.Win
Public Class rptQBRevenueReport
    Friend WithEvents fgReport As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents m_frmReportviewer As frmFGReportViewer
    Private m_DateFrom As Date
    Private m_DateTo As Date
    Private m_ReportDate As Date
    Friend Sub New()
        SetfgReport()
    End Sub

    Private Sub SetfgReport()
        Dim fgrow As C1.Win.C1FlexGrid.Row

        fgReport = New C1.Win.C1FlexGrid.C1FlexGrid()
        With fgReport
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom Or AnchorStyles.Top
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.Vertical
            .Cols.Count = 4
            .Cols.Fixed = 0
            .Rows.Count = 1
            .Rows.Fixed = 1
            .ExtendLastCol = False
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White
            .Styles.Alternate.BackColor = Color.AntiqueWhite
            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .Styles.Normal.Font = New Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Styles.Normal.ForeColor = Color.Gray

            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal0).BackColor = Color.LightGray
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal0).ForeColor = Color.Black
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal1).BackColor = Color.White
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal1).ForeColor = Color.Black
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal2).BackColor = Color.White
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal2).ForeColor = Color.Black
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal0).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal1).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            '.Styles(C1FlexGrid.CellStyleEnum.Subtotal2).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

            .BorderStyle = C1FlexGrid.Util.BaseControls.BorderStyleEnum.Fixed3D
            .Styles.EmptyArea.BackColor = Color.AntiqueWhite
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = False
            .AllowMerging = C1FlexGrid.AllowMergingEnum.Free
            .AllowDragging = C1FlexGrid.AllowDraggingEnum.Columns
            '.Cols(0).AllowMerging = True
            '.Cols(1).AllowMerging = True
            .Rows(0).AllowMerging = True
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 0
        End With

        With fgReport.Cols(0)
            .Width = 220
            .WidthDisplay = 220
            fgReport(0, 0) = " "
            .Name = "RevenueItem"
            .DataType = GetType(String)
            .AllowDragging = False
            .AllowSorting = False
        End With

        With fgReport.Cols(1)
            .Width = 220
            .WidthDisplay = 120

            fgReport(0, 1) = "Taxable"
            .Name = "Taxable"
            .DataType = GetType(Double)
            .Format = "c"
        End With

        With fgReport.Cols(2)
            .Width = 220
            .WidthDisplay = 120
            fgReport(0, 2) = "Tax-Exempt"
            .Name = "TaxExempt"
            .DataType = GetType(Double)
            .Format = "c"
        End With

        With fgReport.Cols(3)
            .Width = 220
            .WidthDisplay = 120
            fgReport(0, 3) = "Total"
            .Name = "Total"
            .DataType = GetType(Double)
            .Format = "c"
        End With

        'With fgReport.Cols(4)
        '    .Width = 220
        '    .WidthDisplay = 120
        '    fgReport(0, 4) = "Dinner"
        '    fgReport(0, 5) = "Dinner"
        '    fgReport(0, 6) = "Dinner"
        '    fgReport(1, 4) = "Taxable"
        '    .Name = "DinnerTaxable"
        '    .DataType = GetType(Double)
        '    .Format = "c"
        'End With

        'With fgReport.Cols(5)
        '    .Width = 220
        '    .WidthDisplay = 120
        '    fgReport(0, 5) = "Dinner"
        '    fgReport(1, 5) = "Tax-Exempt"
        '    .Name = "DinnerTaxExempt"
        '    .DataType = GetType(Double)
        '    .Format = "c"
        'End With

        'With fgReport.Cols(6)
        '    .Width = 220
        '    .WidthDisplay = 120
        '    fgReport(0, 6) = "Dinner"
        '    fgReport(1, 6) = "Total"
        '    .Name = "DinnerTotal"
        '    .DataType = GetType(Double)
        '    .Format = "c"
        'End With

        'With fgReport.Cols(7)
        '    .Width = 220
        '    .WidthDisplay = 120
        '    fgReport(0, 7) = "Day"
        '    fgReport(0, 8) = "Day"
        '    fgReport(0, 9) = "Day"
        '    fgReport(1, 7) = "Taxable"
        '    .Name = "DayTaxable"
        '    .DataType = GetType(Double)
        '    .Format = "c"
        'End With

        'With fgReport.Cols(8)
        '    .Width = 220
        '    .WidthDisplay = 120
        '    fgReport(1, 8) = "Tax-Exempt"
        '    .Name = "DayTaxExempt"
        '    .DataType = GetType(Double)
        '    .Format = "c"
        'End With

        'With fgReport.Cols(9)
        '    .Width = 220
        '    .WidthDisplay = 120
        '    fgReport(1, 9) = "Total"
        '    .Name = "DayTotal"
        '    .DataType = GetType(Double)
        '    .Format = "c"
        'End With

    End Sub

    Private Sub SetSubTotalTree()
        'With fgReport
        '    ' show OutlineBar on column 0

        '    .SubtotalPosition = C1FlexGrid.SubtotalPositionEnum.BelowData
        '    .Tree.Column = 0
        '    .Tree.Style = C1FlexGrid.TreeStyleFlags.Simple
        '    ' clear existing subtotals
        '    .Subtotal(C1FlexGrid.AggregateEnum.Clear)
        '    'get a Grand total (use -1 instead of column index)
        '    '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("PaymentCount").Index, "Grand Total")
        '    '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("CheckAmountPaid").Index, "Grand Total")
        '    '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("TipPaid").Index, "Grand Total")
        '    '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("TotalPaid").Index, "Grand Total")


        '    '' total per Session (column 1)
        '    .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("PaymentCount").Index, "Total {0} ") '+ ReportDate.ToShortDateString)
        '    .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("CheckAmountPaid").Index, "Total {0}")
        '    .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("TipPaid").Index, "Total {0}")
        '    .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("TotalPaid").Index, "Total {0}")




        'End With
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.QBRevenueReport
            .AllowFutureDates = False
            .Show()
        End With
    End Sub

    Private Sub LoadData()
        Dim fgrow As C1.Win.C1FlexGrid.Row

        fgReport.Redraw = False
        'Clear the grid first
        fgReport.Rows.Count = fgReport.Rows.Fixed
        LoadSaleDays()
        'SortReport()
        'SetSubTotalTree()
        ' size column widths based on content
        fgReport.AutoSizeCols(1, fgReport.Cols.Count - 1, 10)
        For Each fgrow In fgReport.Rows
            fgrow.HeightDisplay = 30
        Next

        'fgReport.Tree.Show(1)
        fgReport.Redraw = True
        m_frmReportviewer.ReportGrid = fgReport

    End Sub

    Private Property ReportDate() As Date
        Get
            Return m_ReportDate
        End Get
        Set(ByVal Value As Date)
            m_ReportDate = Value
        End Set
    End Property

    Private Sub LoadSaleDays()
        Dim intTotalDays As Integer
        Dim rptDate As Date
        Dim endDate As Date
        Dim objSaleDay As SaleDay
        Dim paymentType As Array
        With fgReport
            .Rows.Count = fgReport.Rows.Fixed
            paymentType = [Enum].GetValues((GetType(Payment.enumPaymentMethod)))
            'Fixed rows + 8 rows fo revenue + 1 row for each payment method 
            .Rows.Count = fgReport.Rows.Count + paymentType.Length + 8
            '.Rows(2).AllowMerging = False
            '.Rows(3).AllowMerging = False
            '.Rows(4).AllowMerging = False
        End With
        rptDate = m_frmReportviewer.DateFrom

        While (rptDate <= m_frmReportviewer.DateTo)
            objSaleDay = SaleDays.CreateSaleDay.Item(rptDate, False)
            If objSaleDay Is Nothing Then
                'nop
            Else
                LoadRevenueData(objSaleDay)
                LoadPaymentsDataFromMoneyCount(objSaleDay)
                ReportDate = objSaleDay.SaleDate
            End If
            rptDate = rptDate.AddDays(1)
        End While
    End Sub

    Private Sub LoadRevenueData(ByVal objSaleDay As SaleDay)

        Dim objSDSession As SaleDaySession
        Dim intNextRow As Integer
        Dim i As Integer
        Dim colName As String
        Dim dblAmount As Double

        For Each objSDSession In objSaleDay.AllSaleDaySessions
            objSaleDay.SetAllCurrentPayments(objSDSession.SaleDaySessionFrom, objSDSession.SaleDaySessionTo)
            objSaleDay.SetAllNonCurrentPayments(objSDSession.SaleDaySessionFrom, objSDSession.SaleDaySessionTo)
            colName = objSDSession.SessionId.ToString

            With fgReport
                'Retail Base Revenue row for the session on hand
                Select Case objSDSession.SessionId
                    Case Session.enumSessionName.Lunch
                        intNextRow = 1
                    Case Session.enumSessionName.Dinner
                        intNextRow = 2

                End Select
                dblAmount = objSDSession.TaxableRetailBaseRevenue
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = colName & " Retail Base Sale"
                .Item(intNextRow, .Cols.Item("Taxable").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Taxable").Index)) _
                     + dblAmount

                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                    + dblAmount
                dblAmount = objSDSession.TaxExemptRetailBaseRevenue

                .Item(intNextRow, .Cols.Item("TaxExempt").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("TaxExempt").Index)) _
                    + dblAmount

                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                    + dblAmount

                'Retail Base Revenue row for the Total Day column
                '.Item(intNextRow, .Cols.Item("DayTaxable").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTaxable").Index)) _
                '     + objSDSession.TaxableRetailBaseRevenue

                '.Item(intNextRow, .Cols.Item("DayTaxExempt").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTaxExempt").Index)) _
                '    + objSDSession.TaxExemptRetailBaseRevenue

                '.Item(intNextRow, .Cols.Item("DayTotal").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTotal").Index)) _
                '    + objSDSession.TaxableRetailBaseRevenue + objSDSession.TaxExemptRetailBaseRevenue

                'Tips Revenue row for the session on hand
                Select Case objSDSession.SessionId
                    Case Session.enumSessionName.Lunch
                        intNextRow = 3
                    Case Session.enumSessionName.Dinner
                        intNextRow = 4

                End Select
                dblAmount = objSDSession.TipAmountPaidOnTaxableSale()
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = colName & " Retail Tips"
                .Item(intNextRow, .Cols.Item("Taxable").Index) = CDbl(.Item(intNextRow, .Cols.Item("Taxable").Index)) _
                      + dblAmount
                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                     + dblAmount

                dblAmount = objSDSession.TipAmountPaidOnTaxExemptSale()

                .Item(intNextRow, .Cols.Item("TaxExempt").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("TaxExempt").Index)) _
                    + dblAmount
                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                     + dblAmount

                'Revenue SubTotal row for the session on hand
                intNextRow = 5
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = "Total Retail Revenue"
                .Item(intNextRow, .Cols.Item("Taxable").Index) = _
                       CDbl(.Item(1, .Cols.Item("Taxable").Index)) _
                    + CDbl(.Item(2, .Cols.Item("Taxable").Index)) _
                    + CDbl(.Item(3, .Cols.Item("Taxable").Index)) _
                    + CDbl(.Item(4, .Cols.Item("Taxable").Index))

                .Item(intNextRow, .Cols.Item("TaxExempt").Index) = _
                        CDbl(.Item(1, .Cols.Item("TaxExempt").Index)) _
                    + CDbl(.Item(2, .Cols.Item("TaxExempt").Index)) _
                    + CDbl(.Item(3, .Cols.Item("TaxExempt").Index)) _
                    + CDbl(.Item(4, .Cols.Item("TaxExempt").Index))

                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(1, .Cols.Item("Total").Index)) _
                    + CDbl(.Item(2, .Cols.Item("Total").Index)) _
                    + CDbl(.Item(3, .Cols.Item("Total").Index)) _
                    + CDbl(.Item(4, .Cols.Item("Total").Index))

                ''Revenue SubTotal row for the Total Day column
                '.Item(intNextRow, .Cols.Item("DayTaxable").Index) = _
                '        CDbl(.Item(2, .Cols.Item("DayTaxable").Index)) _
                '        + CDbl(.Item(3, .Cols.Item("DayTaxable").Index)) _
                '        + CDbl(.Item(4, .Cols.Item("DayTaxable").Index))

                '.Item(intNextRow, .Cols.Item("DayTaxExempt").Index) = _
                '        CDbl(.Item(2, .Cols.Item("DayTaxExempt").Index)) _
                '        + CDbl(.Item(3, .Cols.Item("DayTaxExempt").Index)) _
                '        + CDbl(.Item(4, .Cols.Item("DayTaxExempt").Index))

                '.Item(intNextRow, .Cols.Item("DayTotal").Index) = _
                '        CDbl(.Item(2, .Cols.Item("DayTotal").Index)) _
                '       + CDbl(.Item(3, .Cols.Item("DayTotal").Index)) _
                '        + CDbl(.Item(4, .Cols.Item("DayTotal").Index))
                ''Tips Revenue row for the Total Day column
                '.Item(intNextRow, .Cols.Item("DayTaxable").Index) = 0

                '.Item(intNextRow, .Cols.Item("DayTaxExempt").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTaxExempt").Index)) _
                '    + dblAmount

                '.Item(intNextRow, .Cols.Item("DayTotal").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTotal").Index)) _
                '    + dblAmount

                'House A/c Base Revenue row for the session on hand
                intNextRow = 6
                dblAmount = +objSDSession.TaxableHouseBaseRevenue()
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = "House A/c Base Sale"
                .Item(intNextRow, .Cols.Item("Taxable").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Taxable").Index)) _
                     + dblAmount
                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                    + dblAmount
                dblAmount = +objSDSession.TaxExemptHouseBaseRevenue()
                .Item(intNextRow, .Cols.Item("TaxExempt").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("TaxExempt").Index)) _
                    + dblAmount
                .Item(intNextRow, .Cols.Item("Total").Index) = _
                        CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                    + dblAmount

                ''House A/c Base Revenue row for the Total Day column
                '.Item(intNextRow, .Cols.Item("DayTaxable").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTaxable").Index)) _
                '     + objSDSession.TaxableHouseBaseRevenue

                '.Item(intNextRow, .Cols.Item("DayTaxExempt").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTaxExempt").Index)) _
                '    + objSDSession.TaxExemptHouseBaseRevenue

                '.Item(intNextRow, .Cols.Item("DayTotal").Index) = _
                '        CDbl(.Item(intNextRow, .Cols.Item("DayTotal").Index)) _
                '    + objSDSession.TaxableHouseBaseRevenue + objSDSession.TaxExemptHouseBaseRevenue

                'Sale of Non-Revenue items like GCs for the Total Day column
                intNextRow = 7
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = "Gift Certificate Sale"
                .Item(intNextRow, .Cols.Item("Total").Index) = _
                       CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                   + objSDSession.NonRevenueSale
                'Sales Tax Collected row for the Total Day column
                intNextRow = 8
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = "Sales Tax Collected"
                .Item(intNextRow, .Cols.Item("Total").Index) = _
                       CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                   + objSDSession.SalesTaxCollected

            End With
        Next
    End Sub
    Private Sub LoadPaymentsDataFromPayments(ByVal objSaleDay As SaleDay)
        'Load Cash Payments from POS, actual Credit card & other payment Amounts from Moneycount
        Dim paymentType As Array
        Dim intNextRow As Integer
        Dim i As Integer
        Dim PaymentMethod As Payment.enumPaymentMethod
        Dim MoneycountAtDinnerClose As MoneyCount
        Dim objSDSession As SaleDaySession
        MoneycountAtDinnerClose = objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).AllMoneyCounts.Item(MoneyCount.enumMoneyCountType.Closing)

        objSaleDay.SetAllCurrentPayments(objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo)
        objSaleDay.SetAllNonCurrentPayments(objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo)
        'paymentType = [Enum].GetNames(GetType(Payment.enumPaymentMethod))
        paymentType = [Enum].GetValues((GetType(Payment.enumPaymentMethod)))
        For Each objSDSession In objSaleDay.AllSaleDaySessions
            intNextRow = fgReport.Rows.Count - paymentType.Length
            With fgReport
                For i = 0 To paymentType.Length - 1
                    PaymentMethod = CType(paymentType.GetValue(i), Payment.enumPaymentMethod)
                    .Item(intNextRow, .Cols.Item("RevenueItem").Index) = PaymentMethod.ToString & " Payments"

                    Select Case PaymentMethod
                        Case Payment.enumPaymentMethod.Amex
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.AmexAmount

                        Case Payment.enumPaymentMethod.Cash
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ objSaleDay.CashTotalAmountForDay + objSaleDay.NonCurrentCashTotalAmount
                        Case Payment.enumPaymentMethod.DebitCard
                            '.Item(intNextRow, .Cols.Item("Total").Index) = _
                            '    CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                            '   + MoneycountAtDinnerClose.DebitCardTotalAmountForDa
                        Case Payment.enumPaymentMethod.DinersClub
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.DinersClubAmount
                        Case Payment.enumPaymentMethod.Discover
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.DiscoverAmount
                        Case Payment.enumPaymentMethod.HouseAccount
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.HouseAccountAmount
                        Case Payment.enumPaymentMethod.KnightExpress
                            '.Item(intNextRow, .Cols.Item("Total").Index) = _
                            '    CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                            '   + MoneycountAtDinnerClose.KnightExpressAmount
                        Case Payment.enumPaymentMethod.MasterCard
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.MasterCardAmount
                        Case Payment.enumPaymentMethod.MGGiftCertificate
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.GiftCertificateAmount
                        Case Payment.enumPaymentMethod.MissingPayment
                            '.Item(intNextRow, .Cols.Item("Total").Index) = _
                            '    CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                            '   + MoneycountAtDinnerClose.MissingPaymentTotalAmountForDa
                        Case Payment.enumPaymentMethod.PersonalCheck
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.PersonalCheckAmount

                        Case Payment.enumPaymentMethod.TravellersChecks
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.TravellersCheckAmount

                        Case Payment.enumPaymentMethod.Visa
                            .Item(intNextRow, .Cols.Item("Total").Index) = _
                                CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                                  + objSDSession.PaymentAmountByMethod(CInt(PaymentMethod))
                            '+ MoneycountAtDinnerClose.VisaAmount

                    End Select
                    intNextRow += 1
                Next
            End With
        Next

    End Sub

    Private Sub LoadPaymentsDataFromMoneyCount(ByVal objSaleDay As SaleDay)
        'Load Cash Payments from POS, actual Credit card & other payment Amounts from Moneycount
        Dim paymentType As Array
        Dim intNextRow As Integer
        Dim i As Integer
        Dim PaymentMethod As Payment.enumPaymentMethod
        Dim MoneycountAtDinnerClose As MoneyCount
        Dim objSDSession As SaleDaySession
        MoneycountAtDinnerClose = objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).AllMoneyCounts.Item(MoneyCount.enumMoneyCountType.Closing)

        objSaleDay.SetAllCurrentPayments(objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo)
        objSaleDay.SetAllNonCurrentPayments(objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo)
        'paymentType = [Enum].GetNames(GetType(Payment.enumPaymentMethod))
        paymentType = [Enum].GetValues((GetType(Payment.enumPaymentMethod)))
        intNextRow = fgReport.Rows.Count - paymentType.Length
        With fgReport
            For i = 0 To paymentType.Length - 1
                PaymentMethod = CType(paymentType.GetValue(i), Payment.enumPaymentMethod)
                .Item(intNextRow, .Cols.Item("RevenueItem").Index) = PaymentMethod.ToString & " Payments"

                Select Case PaymentMethod
                    Case Payment.enumPaymentMethod.Amex
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.AmexAmount

                    Case Payment.enumPaymentMethod.Cash
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + objSaleDay.CashTotalAmountForDay + objSaleDay.NonCurrentCashTotalAmount
                    Case Payment.enumPaymentMethod.DebitCard
                        '.Item(intNextRow, .Cols.Item("Total").Index) = _
                        '    CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        '   + MoneycountAtDinnerClose.DebitCardTotalAmountForDa
                    Case Payment.enumPaymentMethod.DinersClub
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.DinersClubAmount
                    Case Payment.enumPaymentMethod.Discover
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.DiscoverAmount
                    Case Payment.enumPaymentMethod.HouseAccount
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.HouseAccountAmount
                    Case Payment.enumPaymentMethod.KnightExpress
                        '.Item(intNextRow, .Cols.Item("Total").Index) = _
                        '    CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        '   + MoneycountAtDinnerClose.KnightExpressAmount
                    Case Payment.enumPaymentMethod.MasterCard
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.MasterCardAmount
                    Case Payment.enumPaymentMethod.MGGiftCertificate
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.GiftCertificateAmount
                    Case Payment.enumPaymentMethod.MissingPayment
                        '.Item(intNextRow, .Cols.Item("Total").Index) = _
                        '    CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        '   + MoneycountAtDinnerClose.MissingPaymentTotalAmountForDa
                    Case Payment.enumPaymentMethod.PersonalCheck
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.PersonalCheckAmount

                    Case Payment.enumPaymentMethod.TravellersChecks
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.TravellersCheckAmount

                    Case Payment.enumPaymentMethod.Visa
                        .Item(intNextRow, .Cols.Item("Total").Index) = _
                            CDbl(.Item(intNextRow, .Cols.Item("Total").Index)) _
                        + MoneycountAtDinnerClose.VisaAmount

                End Select
                intNextRow += 1
            Next
        End With

    End Sub


    Private Sub m_frmReportviewer_RunReport() Handles m_frmReportviewer.RunReport
        LoadData()
    End Sub

    Private Sub SortReport()
        If fgReport.Cols(0).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols(0).Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols(0).Sort = C1FlexGrid.SortFlags.Ascending
        End If
        If fgReport.Cols(1).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols(1).Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols(1).Sort = C1FlexGrid.SortFlags.Ascending
        End If
        'If fgReport.Cols(3).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
        '    fgReport.Cols(3).Sort = C1FlexGrid.SortFlags.Descending
        'Else
        '    fgReport.Cols(3).Sort = C1FlexGrid.SortFlags.Ascending
        'End If
        fgReport.Sort(C1FlexGrid.SortFlags.UseColSort, 0, 3)
    End Sub

    Private Sub fgReport_AfterDragColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.DragRowColEventArgs) Handles fgReport.AfterDragColumn
        If e.Col < 3 Then      'cols 3 and later are data columns
            LoadData()
        End If
    End Sub

End Class

