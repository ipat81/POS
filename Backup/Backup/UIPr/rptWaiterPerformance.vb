Option Strict On
Imports BOPr
Imports C1.Win
Public Class rptWaiterPerformance
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
            .Cols.Count = 14
            .Cols.Fixed = 0
            .Rows.Count = 2
            .Rows.Fixed = 2
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
            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
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
            fgReport(1, 0) = "Date"
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
            fgReport(1, 1) = "L/D"
            .Name = "SessionName"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(2)
            fgReport(0, 2) = "Waiter"
            fgReport(1, 2) = "Waiter"
            .Name = "Waiter"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(3)
            fgReport(0, 3) = "Table"
            fgReport(1, 3) = "Table"
            .Name = "TableSeating"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(4)
            fgReport(1, 4) = "Base"
            .Name = "BaseSaleEatIn"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(5)
            fgReport(1, 5) = "Tips"
            .Name = "TipsEatIn"
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgReport.Cols(6)
            fgReport(1, 6) = "Tips %"
            .Name = "TipsPercentEatIn"
            .DataType = GetType(Double)
            .Format = "##.##"
        End With
        fgReport(0, 4) = "Eat In Amount"
        fgReport(0, 5) = "Eat In Amount"
        fgReport(0, 6) = "Eat In Amount"

        With fgReport.Cols(7)
            fgReport(1, 7) = "Orders"
            .Name = "EatInOrders"
            .DataType = GetType(Integer)
            .Format = "#,###"
        End With

        With fgReport.Cols(8)
            fgReport(1, 8) = "Adults"
            .Name = "EatInAdults"
            .DataType = GetType(Integer)
            .Format = "#,###"
        End With

        With fgReport.Cols(9)
            fgReport(1, 9) = "Kids"
            .Name = "EatInKids"
            .DataType = GetType(Integer)
            .Format = "#,###"
        End With
        fgReport(0, 7) = "Number Served"
        fgReport(0, 8) = "Number Served"
        fgReport(0, 9) = "Number Served"

        With fgReport.Cols(10)
            fgReport(1, 10) = "Apps"
            .Name = "AppsPerAdult"
            .DataType = GetType(Double)
            .Format = "##.##"
        End With

        With fgReport.Cols(11)
            fgReport(1, 11) = "Entrees"
            .Name = "EntreesPerAdult"
            .DataType = GetType(Double)
            .Format = "##.##"
        End With

        With fgReport.Cols(12)
            fgReport(1, 12) = "Desserts"
            .Name = "DessertsPerAdult"
            .DataType = GetType(Double)
            .Format = "##.##"
        End With

        With fgReport.Cols(13)
            fgReport(1, 13) = "Drinks"
            .Name = "DrinksPerAdult"
            .DataType = GetType(Double)
            .Format = "##.##"
        End With
        fgReport(0, 10) = "Quantity Sold Per Adult"
        fgReport(0, 11) = "Quantity Sold Per Adult"
        fgReport(0, 12) = "Quantity Sold Per Adult"
        fgReport(0, 13) = "Quantity Sold Per Adult"
    End Sub

    Private Sub SetSubTotalTree()
        ' show OutlineBar on column 0
        fgReport.SubtotalPosition = C1FlexGrid.SubtotalPositionEnum.BelowData
        fgReport.Tree.Column = 0
        fgReport.Tree.Style = C1FlexGrid.TreeStyleFlags.Simple
        ' clear existing subtotals
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Clear)
        ' get a Grand total (use -1 instead of column index)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, 4, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, 5, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, -1, -1, 6, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, 7, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, 8, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, 9, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, -1, -1, 10, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, -1, -1, 11, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, -1, -1, 12, "Grand Total")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, -1, -1, 13, "Grand Total")
        ' total per SaleDate (column 0)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, 4, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, 5, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 0, 0, 6, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, 7, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, 8, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, 9, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 0, 0, 10, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 0, 0, 11, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 0, 0, 12, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 0, 0, 13, "Total {0}")
        ' total per L/D (column 1)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, 4, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, 5, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 1, 1, 6, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, 7, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, 8, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, 9, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 1, 1, 10, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 1, 1, 11, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 1, 1, 12, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 1, 1, 13, "Total {0}")
        ' total per Waiter (column 2)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, 4, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, 5, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 2, 2, 6, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, 7, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, 8, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 2, 2, 9, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 2, 2, 10, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 2, 2, 11, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 2, 2, 12, "Total {0}")
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Average, 2, 2, 13, "Total {0}")
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.WaiterPerformance
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

        fgReport.Tree.Show(2)
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


        For Each objOrder In objSDSession.AllOrders
            Select Case objOrder.OrderType
                Case Order.enumOrderType.EatIn
                    With fgReport
                        .Rows.Count += 1
                        .Item(intNextRow, 0) = objOrder.Saledate
                        .Item(intNextRow, 1) = objOrder.orderSDSession.SessionName
                        .Item(intNextRow, 2) = objOrder.Waiter.EmployeeShortName
                        .Item(intNextRow, 3) = objOrder.Table.TableName & "-" & objOrder.TableSeating.ToString

                        If objOrder.OrderBaseAmount <> 0 Then
                            .Item(intNextRow, 4) = objOrder.OrderBaseAmount
                        End If
                        If objOrder.TipAmountPaid <> 0 Then
                            .Item(intNextRow, 5) = objOrder.TipAmountPaid
                            .Item(intNextRow, 6) = objOrder.TipAmountPaidPercent
                        End If

                        .Item(intNextRow, 7) = 1    'orders count
                        .Item(intNextRow, 8) = objOrder.AdultsCount
                        .Item(intNextRow, 9) = objOrder.KidsCount

                        .Item(intNextRow, 10) = objOrder.AppsCount / objOrder.AdultsCount
                        .Item(intNextRow, 11) = objOrder.EntreesCount / objOrder.AdultsCount
                        .Item(intNextRow, 12) = objOrder.DessertsCount / objOrder.AdultsCount
                        .Item(intNextRow, 13) = objOrder.DrinksCount / objOrder.AdultsCount
                        intNextRow += 1
                    End With
                Case Else
                    'skip take outs
            End Select
        Next
    End Sub

    Private Sub m_frmReportviewer_RunReport() Handles m_frmReportviewer.RunReport
        LoadData()
    End Sub

    Private Sub fgReport_BeforeDragColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.DragRowColEventArgs) Handles fgReport.BeforeDragColumn
        If e.Col <= 3 Then      'cols 4 and later are data columns
            SortReport()
        End If
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
End Class

