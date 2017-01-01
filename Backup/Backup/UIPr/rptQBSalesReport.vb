Option Strict On
Imports BOPr
Imports C1.Win
Public Class rptQBSalesReport
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
            .Cols.Count = 7
            .Cols.Fixed = 0
            .Rows.Count = 2
            .Rows.Fixed = 1
            .ExtendLastCol = False
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .Styles.Normal.Font = New Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Styles.Normal.ForeColor = Color.Gray

            .Styles(C1FlexGrid.CellStyleEnum.Subtotal0).BackColor = Color.LightGray
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal0).ForeColor = Color.Black
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal1).BackColor = Color.White
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal1).ForeColor = Color.Black
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal2).BackColor = Color.White
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal2).ForeColor = Color.Black
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal0).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal1).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal2).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

            .BorderStyle = C1FlexGrid.Util.BaseControls.BorderStyleEnum.Fixed3D
            .Styles.EmptyArea.BackColor = Color.AntiqueWhite
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = False
            .AllowMerging = C1FlexGrid.AllowMergingEnum.Free
            .AllowDragging = C1FlexGrid.AllowDraggingEnum.Columns
            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .Cols(0).AllowMerging = True
            .Cols(1).AllowMerging = True


            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 0
        End With

        With fgReport.Cols(0)
            .Width = 220
            .WidthDisplay = 180
            fgReport(0, 0) = "Date"
            .Name = "SaleDate"
            .DataType = GetType(Date)
            .Format = "ddd MM/dd/yy"
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(1)
            .Width = 220
            .WidthDisplay = 120
            fgReport(0, 1) = "L/D"
            .Name = "Session"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(2)
            .Width = 140
            .WidthDisplay = 120
            fgReport(0, 2) = "Payment Method"
            .Name = "PaymentMethod"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(3)
            .Width = 140
            .WidthDisplay = 800
            fgReport(0, 3) = "No of Payments"
            .Name = "PaymentCount"
            .DataType = GetType(Integer)
            .AllowDragging = True
            .AllowSorting = True
            '  .AllowMerging = True
        End With
        With fgReport.Cols(4)
            .Width = 120
            .WidthDisplay = 100
            fgReport(0, 4) = "Check AmountPaid"
            .Name = "CheckAmountPaid"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowSorting = True
        End With
        With fgReport.Cols(5)
            .Width = 120
            .WidthDisplay = 90
            fgReport(0, 5) = "Tip Paid"
            .Name = "TipPaid"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowSorting = True
        End With

        With fgReport.Cols(6)
            .Width = 120
            .WidthDisplay = 100
            fgReport(0, 6) = "Total Paid"
            .Name = "TotalPaid"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowSorting = True
        End With


    End Sub

    Private Sub SetSubTotalTree()
        With fgReport
            ' show OutlineBar on column 0

            .SubtotalPosition = C1FlexGrid.SubtotalPositionEnum.BelowData
            .Tree.Column = 0
            .Tree.Style = C1FlexGrid.TreeStyleFlags.Simple
            ' clear existing subtotals
            .Subtotal(C1FlexGrid.AggregateEnum.Clear)
            'get a Grand total (use -1 instead of column index)
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("PaymentCount").Index, "Grand Total")
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("CheckAmountPaid").Index, "Grand Total")
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("TipPaid").Index, "Grand Total")
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("TotalPaid").Index, "Grand Total")


            '' total per Session (column 1)
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("PaymentCount").Index, "Total {0} ") '+ ReportDate.ToShortDateString)
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("CheckAmountPaid").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("TipPaid").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("TotalPaid").Index, "Total {0}")




        End With
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.QBSalesReport
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
        SortReport()
        SetSubTotalTree()
        ' size column widths based on content
        fgReport.AutoSizeCols(2, fgReport.Cols.Count - 1, 25)
        For Each fgrow In fgReport.Rows
            fgrow.HeightDisplay = 35
        Next

        fgReport.Tree.Show(1)
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
        rptDate = m_frmReportviewer.DateFrom
        While (rptDate <= m_frmReportviewer.DateTo)
            objSaleDay = SaleDays.CreateSaleDay.Item(rptDate, False)
            If objSaleDay Is Nothing Then
                'nop
            Else
                LoadSdSessions(objSaleDay)
                ReportDate = objSaleDay.SaleDate
            End If
            rptDate = rptDate.AddDays(1)
        End While
    End Sub

    Private Sub LoadSdSessions(ByVal objSaleDay As SaleDay)

        Dim objSDSession As SaleDaySession
        Dim paymentType() As String
        Dim intNextRow As Integer
        Dim i As Integer
        'intNextRow = fgReport.Rows.Count
        paymentType = [Enum].GetNames(GetType(Payment.enumPaymentMethod))
        'fgReport.Rows.Count = fgReport.Rows.Count + paymentType.Length
        For Each objSDSession In objSaleDay.AllSaleDaySessions
            objSaleDay.SetAllCurrentPayments(objSDSession.SaleDaySessionFrom, objSDSession.SaleDaySessionTo)
            objSaleDay.SetAllNonCurrentPayments(objSDSession.SaleDaySessionFrom, objSDSession.SaleDaySessionTo)
            intNextRow = fgReport.Rows.Count
            fgReport.Rows.Count = fgReport.Rows.Count + paymentType.Length
            With fgReport

                For i = 0 To paymentType.Length - 1
                    .Item(intNextRow, .Cols.Item("SaleDate").Index) = objSaleDay.SaleDate
                    .Item(intNextRow, .Cols.Item("Session").Index) = objSDSession.SessionName
                    Select Case paymentType(i)

                        Case "Amex"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "American Express"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.AmexCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.AmexAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.AmexTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.AmexTotalAmountForDay

                        Case "Cash"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Cash"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.CashCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.CashAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.CashTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.CashTotalAmountForDay

                        Case "DebitCard"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Debit Card"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.DebitCardCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.DebitCardAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.DebitCardTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.DebitCardTotalAmountForDay
                        Case "Discover"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Discover"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.DiscoverCardCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.DiscoverCardAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.DiscoverCardTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.DiscoverCardTotalAmountForDay
                        Case "HouseAccount"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "House Account"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.HouseAccountCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.HouseAccountAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.HouseAccountTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.HouseAccountTotalAmountForDay
                        Case "KnightExpress"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Knight Express"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.KnightExpressCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.KnightExpressAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.KnightExpressTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.KnightExpressTotalAmountForDay
                        Case "MasterCard"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Master Card"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.MasterCardCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.MasterCardAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.MasterCardTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.MasterCardTotalAmountForDay
                        Case "MGGiftCertificate"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "MG Gift Certificate"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.MGGiftCertificateCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.MGGiftCertificateAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.MGGiftCertificateTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.MGGiftCertificateTotalAmountForDay
                        Case "PersonalCheck"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Personal Check"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.PersonalCheckCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.PersonalCheckAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.PersonalCheckTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.PersonalCheckTotalAmountForDay
                        Case "TravellersChecks"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Travellers Check"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.TravellersChecksCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.TravellersChecksAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.TravellersChecksTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.TravellersChecksTotalAmountForDay

                        Case "Visa"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Visa"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.VisaCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.VisaAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.VisaTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.VisaTotalAmountForDay
                        Case "DinersClub"
                            .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Diners Club"
                            .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.DinersClubCountForDay
                            .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.DinersClubAmountForDay
                            .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.DinersClubTipAmountForDay
                            .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.DinersClubTotalAmountForDay

                    End Select
                    intNextRow += 1
                Next
            End With
        Next
        LoadDaysPaymentDetails(objSaleDay)
    End Sub

    Private Sub LoadDaysPaymentDetails(ByVal objSaleDay As SaleDay)

        Dim paymentType As Array
        Dim intNextRow As Integer
        Dim i As Integer
        objSaleDay.SetAllCurrentPayments(objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo)
        objSaleDay.SetAllNonCurrentPayments(objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom, objSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo)
        'paymentType = [Enum].GetNames(GetType(Payment.enumPaymentMethod))
        paymentType = [Enum].GetValues((GetType(Payment.enumPaymentMethod)))
        intNextRow = fgReport.Rows.Count
        fgReport.Rows.Count = fgReport.Rows.Count + paymentType.Length
        With fgReport

            For i = 0 To paymentType.Length - 1
                .Item(intNextRow, .Cols.Item("SaleDate").Index) = objSaleDay.SaleDate
                .Item(intNextRow, .Cols.Item("Session").Index) = "Day"
                Select Case CType(paymentType.GetValue(i), Payment.enumPaymentMethod)

                    Case Payment.enumPaymentMethod.Amex
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "American Express"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.AmexCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.AmexAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.AmexTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.AmexTotalAmountForDay

                    Case Payment.enumPaymentMethod.Cash
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Cash"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.CashCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.CashAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.CashTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.CashTotalAmountForDay
                    Case Payment.enumPaymentMethod.DebitCard
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Debit Card"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.DebitCardCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.DebitCardAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.DebitCardTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.DebitCardTotalAmountForDay
                    Case Payment.enumPaymentMethod.DinersClub
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Diners Club"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.DinersClubCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.DinersClubAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.DinersClubTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.DinersClubTotalAmountForDay
                    Case Payment.enumPaymentMethod.Discover
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Discover"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.DiscoverCardCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.DiscoverCardAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.DiscoverCardTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.DiscoverCardTotalAmountForDay
                    Case Payment.enumPaymentMethod.HouseAccount
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "House Account"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.HouseAccountCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.HouseAccountAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.HouseAccountTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.HouseAccountTotalAmountForDay
                    Case Payment.enumPaymentMethod.KnightExpress
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Knight Express"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.KnightExpressCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.KnightExpressAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.KnightExpressTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.KnightExpressTotalAmountForDay
                    Case Payment.enumPaymentMethod.MasterCard
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Master Card"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.MasterCardCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.MasterCardAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.MasterCardTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.MasterCardTotalAmountForDay
                    Case Payment.enumPaymentMethod.MGGiftCertificate
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "MG Gift Certificate"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.MGGiftCertificateCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.MGGiftCertificateAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.MGGiftCertificateTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.MGGiftCertificateTotalAmountForDay
                    Case Payment.enumPaymentMethod.MissingPayment
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Missing Payment"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.MissingPaymentCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.MissingPaymentAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.MissingPaymentTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.MissingPaymentTotalAmountForDay
                    Case Payment.enumPaymentMethod.PersonalCheck
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Personal Check"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.PersonalCheckCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.PersonalCheckAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.PersonalCheckTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.PersonalCheckTotalAmountForDay
                        'Case Payment.enumPaymentMethod.ShortPayment
                        '    .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Short Payment"
                        '    .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.ShortPaymentCountForDay
                        '    .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.ShortPaymentAmountForDay
                        '    .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.ShortPaymentTipAmountForDay
                        '    .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.ShortPaymentTotalAmountForDay
                    Case Payment.enumPaymentMethod.TravellersChecks
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Travellers Check"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.TravellersChecksCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.TravellersChecksAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.TravellersChecksTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.TravellersChecksTotalAmountForDay

                    Case Payment.enumPaymentMethod.Visa
                        .Item(intNextRow, .Cols.Item("PaymentMethod").Index) = "Visa"
                        .Item(intNextRow, .Cols.Item("PaymentCount").Index) = objSaleDay.VisaCountForDay
                        .Item(intNextRow, .Cols.Item("CheckAmountPaid").Index) = objSaleDay.VisaAmountForDay
                        .Item(intNextRow, .Cols.Item("TipPaid").Index) = objSaleDay.VisaTipAmountForDay
                        .Item(intNextRow, .Cols.Item("TotalPaid").Index) = objSaleDay.VisaTotalAmountForDay

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
