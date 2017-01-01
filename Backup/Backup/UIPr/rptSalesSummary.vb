Option Strict On
Imports BOPr
Imports C1.Win
Public Class rptSalesSummary
    Friend WithEvents fgReport As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents m_frmReportviewer As frmFGReportViewer
    Private m_DateFrom As Date
    Private m_DateTo As Date
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
            .Cols.Count = 12
            .Cols.Fixed = 0
            .Rows.Count = 1
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

            .Styles(C1FlexGrid.CellStyleEnum.GrandTotal).BackColor = Color.LightGray
            .Styles(C1FlexGrid.CellStyleEnum.GrandTotal).ForeColor = Color.Black
            .Styles(C1FlexGrid.CellStyleEnum.GrandTotal).Font = New Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Styles(C1FlexGrid.CellStyleEnum.GrandTotal).Border.Style = C1FlexGrid.BorderStyleEnum.Raised

            .Styles(C1FlexGrid.CellStyleEnum.Subtotal0).BackColor = Color.LightGray
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal0).ForeColor = Color.Black
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal1).BackColor = Color.LightGray
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal1).ForeColor = Color.Black
            .Styles(C1FlexGrid.CellStyleEnum.Subtotal2).BackColor = Color.LightGray
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
            '.Rows(0).AllowMerging = True
            '.Rows(1).AllowMerging = True
            .Cols(0).AllowMerging = True
            .Cols(1).AllowMerging = True
            .Cols(2).AllowMerging = True
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 0
        End With

        With fgReport.Cols(0)
            .Width = 160
            .WidthDisplay = 140
            fgReport(0, 0) = "Date"
            'fgReport(1, 0) = "Date"
            .Name = "SaleDate"
            .DataType = GetType(Date)
            .Format = "ddd MM/dd/yy"
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(1)
            .Width = 80
            .WidthDisplay = 60
            fgReport(0, 1) = "L/D"
            'fgReport(1, 1) = "L/D"
            .Name = "SessionName"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(2)
            fgReport(0, 2) = "OrderType"
            .Width = 100
            .WidthDisplay = 60
            'fgReport(1, 2) = "OrderType"
            .Name = "OrderType"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(3)
            fgReport(0, 3) = "Table"
            .Width = 100
            .WidthDisplay = 40
            'fgReport(1, 3) = "Table"
            .Name = "TableSeating"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(4)
            fgReport(0, 4) = "Base Sale"
            .Width = 100
            .WidthDisplay = 80
            .Name = "BaseSale"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(5)
            fgReport(0, 5) = "Discount"
            .Width = 100
            .WidthDisplay = 60
            .Name = "Discount"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(6)
            fgReport(0, 6) = "GC Sale"
            .Width = 100
            .WidthDisplay = 60
            .Name = "GCSale"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(7)
            fgReport(0, 7) = "Net Base"
            .Width = 100
            .WidthDisplay = 90
            .Name = "NetBase"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(8)
            fgReport(0, 8) = "Tips"
            .Width = 100
            .WidthDisplay = 80
            .Name = "Tips"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(9)
            fgReport(0, 9) = "Total Net Sale"
            .Width = 100
            .WidthDisplay = 80
            .Name = "TotalNetSale"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(10)
            fgReport(0, 10) = "STax"
            .Width = 100
            .WidthDisplay = 65
            .Name = "STax"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(11)
            fgReport(0, 11) = "POS"
            .Width = 100
            .WidthDisplay = 80
            .Name = "POS"
            .DataType = GetType(Double)
            .Format = "C"
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
            ' get a Grand total (use -1 instead of column index)
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("BaseSale").Index, "Grand Total")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("Discount").Index, "Grand Total")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("NetBase").Index, "Grand Total")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("Tips").Index, "Grand Total")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("TotalNetSale").Index, "Grand Total")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("STax").Index, "Grand Total")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, .Cols.Item("POS").Index, "Grand Total")

            ' total per SaleDate (column 0)
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("BaseSale").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("Discount").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("NetBase").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("Tips").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("TotalNetSale").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("STax").Index, "Total {0}")
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("BaseSaleTotal").Index, "Total {0}")
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("TipsTotal").Index, "Total {0}")
            '.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("TotalTotal").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, .Cols.Item("POS").Index, "Total {0}")

            ' total per L/D (column 1)
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("BaseSale").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("Discount").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("NetBase").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("Tips").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("TotalNetSale").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("STax").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, .Cols.Item("POS").Index, "Total {0}")

            ' total per OrderType (column 2)
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("BaseSale").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("Discount").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("NetBase").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("Tips").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("TotalNetSale").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("STax").Index, "Total {0}")
            .Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, .Cols.Item("POS").Index, "Total {0}")
        End With
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.DailySalesSummary
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
        ' fgReport.AutoSizeCols(2, fgReport.Cols.Count - 1, 25)
        For Each fgrow In fgReport.Rows
            fgrow.HeightDisplay = 35
        Next

        fgReport.Tree.Show(1)
        fgReport.Redraw = True
        m_frmReportviewer.ReportGrid = fgReport

    End Sub

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
            End If
            rptDate = rptDate.AddDays(1)
        End While
    End Sub

    Private Sub LoadSdSessions(ByVal objSaleDay As SaleDay)

        Dim objSDSession As SaleDaySession

        For Each objSDSession In objSaleDay.AllSaleDaySessions
            LoadOrders(objSDSession)
        Next
    End Sub

    Private Sub LoadOrders(ByVal objSDSession As SaleDaySession)
        Dim objOrder As Order
        Dim intNextRow As Integer

        intNextRow = fgReport.Rows.Count

        fgReport.Rows.Count = fgReport.Rows.Count + objSDSession.AllOrders.Count

        For Each objOrder In objSDSession.AllOrders
            With fgReport
                .Item(intNextRow, 0) = objOrder.Saledate
                .Item(intNextRow, 1) = objOrder.orderSDSession.SessionName
                Select Case objOrder.OrderType
                    Case Order.enumOrderType.EatIn
                        .Item(intNextRow, 2) = "Eat In"
                    Case Order.enumOrderType.Delivery
                        .Item(intNextRow, 2) = "Take Out"
                    Case Order.enumOrderType.Pickup
                        .Item(intNextRow, 2) = "Take Out"
                End Select

                .Item(intNextRow, 3) = objOrder.Table.TableName & "-" & objOrder.TableSeating.ToString

                If objOrder.OrderBaseAmount <> 0 Then
                    .Item(intNextRow, .Cols.Item("BaseSale").Index) = objOrder.OrderBaseAmount
                    .Item(intNextRow, .Cols.Item("Discount").Index) = objOrder.DiscountAmount
                    .Item(intNextRow, .Cols.Item("GCSale").Index) = objOrder.NonRevenueSale
                    .Item(intNextRow, .Cols.Item("NetBase").Index) = objOrder.OrderBaseAmount + objOrder.DiscountAmount - objOrder.NonRevenueSale
                End If
                If objOrder.TipAmountPaid <> 0 Then
                    .Item(intNextRow, .Cols.Item("Tips").Index) = objOrder.TipAmountPaid
                End If

                .Item(intNextRow, .Cols.Item("TotalNetSale").Index) = objOrder.OrderBaseAmount + objOrder.DiscountAmount - objOrder.NonRevenueSale + objOrder.TipAmountPaid

                .Item(intNextRow, .Cols.Item("STax").Index) = objOrder.OrderSalesTaxAmount
                .Item(intNextRow, .Cols.Item("POS").Index) = objOrder.POSAmount
                intNextRow += 1
            End With
        Next
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
        If fgReport.Cols(2).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols(2).Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols(2).Sort = C1FlexGrid.SortFlags.Ascending
        End If

        If fgReport.Cols(3).Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols(3).Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols(3).Sort = C1FlexGrid.SortFlags.Ascending
        End If
        fgReport.Sort(C1FlexGrid.SortFlags.UseColSort, 0, 3)
    End Sub

    Private Sub fgReport_AfterDragColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.DragRowColEventArgs) Handles fgReport.AfterDragColumn
        If e.Col <= 3 Then      'cols 3 and later are data columns
            LoadData()
        End If
    End Sub
End Class
