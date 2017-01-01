
Option Strict On
Imports BOPr
Imports C1.Win
Public Class rptFutureOrderList

    Friend WithEvents fgReport As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents m_frmReportviewer As frmFGReportViewer
    Private m_Orders As Orders
    Private m_Managers As Employees
    Private m_DateFrom As Date
    Private m_DateTo As Date
    Private m_SortColumnFutureOrderGrid As C1FlexGrid.Column
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
            .Width = 170
            .WidthDisplay = 160
            fgReport(0, 0) = "Promised Date"
            .Name = "PromisedDate"
            .DataType = GetType(Date)
            .Format = "ddd MM/dd/yy"
            .AllowDragging = True
            .AllowSorting = True


        End With
        With fgReport.Cols(1)
            .Width = 140
            .WidthDisplay = 120
            fgReport(0, 1) = "Promised Time"
            .Name = "PromisedTime"
            .DataType = GetType(Date)
            .Format = "HH:mm"
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(2)
            .Width = 200
            .WidthDisplay = 160
            fgReport(0, 2) = "Customer"
            .Name = "Customer"
            .DataType = GetType(String)
            .AllowSorting = True
        End With

        With fgReport.Cols(3)
            .Width = 200
            .WidthDisplay = 160
            fgReport(0, 3) = "Order Type"
            .Name = "OrderType"
            .DataType = GetType(String)
            .AllowSorting = True
        End With

        With fgReport.Cols(4)
            .Width = 140
            .WidthDisplay = 120
            fgReport(0, 4) = "Date Entered"
            .Name = "DateEntered"
            .DataType = GetType(DateTime)
            .Format = "ddd MM/dd/yy "
            .AllowSorting = True
        End With




        With fgReport.Cols(5)
            .Width = 200
            .WidthDisplay = 160
            fgReport(0, 5) = "Total $"
            .Name = "BaseAmount"
            .DataType = GetType(Decimal)
            .Format = "c"
            .AllowSorting = True
        End With

        With fgReport.Cols(6)
            .Width = 60
            .WidthDisplay = 40
            fgReport(0, 6) = "Table Name - Seating"
            .Name = "Seating"
            .DataType = GetType(String)
        End With

    End Sub

    Private Sub SetSubTotalTree()
        ' show OutlineBar on column 0
        fgReport.SubtotalPosition = C1FlexGrid.SubtotalPositionEnum.BelowData
        fgReport.Tree.Column = 0
        fgReport.Tree.Style = C1FlexGrid.TreeStyleFlags.Simple
        ' clear existing subtotals
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Clear)
        ' get a Grand total (use -1 instead of column index)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Count, -1, -1, fgReport.Cols.Item("Seating").Index, "Grand Total")

        ' total per Override Type(column 0)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Count, 0, 0, fgReport.Cols.Item("Seating").Index, "Total On :{0} ")

        ' total per Date(column 1)
        'fgReport.Subtotal(C1FlexGrid.AggregateEnum.Count, 0, 1, 4, "On {0}")
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.FutureOrdersList
            .AllowFutureDates = True
            '.DateFrom = Date.Now
            .Show()
        End With
    End Sub

    Private Sub LoadData()
        Dim fgrow As C1.Win.C1FlexGrid.Row

        fgReport.Redraw = False
        'Clear the grid first
        fgReport.Rows.Count = fgReport.Rows.Fixed
        LoadFutureOrderList()
        SortReport()
        SetSubTotalTree()
        ' size column widths based on content
        fgReport.AutoSizeCols(2, fgReport.Cols.Count - 1, 25)
        For Each fgrow In fgReport.Rows
            fgrow.HeightDisplay = 35
        Next

        fgReport.Tree.Show(0)
        fgReport.Redraw = True
        m_frmReportviewer.ReportGrid = fgReport

    End Sub

    Private Sub LoadFutureOrderList()

        Dim objOrder As Order
        Dim intNextRow As Integer
        Dim rg As C1.Win.C1FlexGrid.CellRange

        With fgReport
            intNextRow = fgReport.Rows.Count
            .Rows.Count += FutureOrdersForAPeriod.Count

            For Each objOrder In FutureOrdersForAPeriod


                .Item(intNextRow, fgReport.Cols.Item("PromisedDate").Index) = objOrder.PromisedAt.Date
                .Item(intNextRow, fgReport.Cols.Item("PromisedTime").Index) = objOrder.PromisedAt

                .Item(intNextRow, fgReport.Cols.Item("DateEntered").Index) = objOrder.EnteredAt

                Select Case objOrder.OrderType
                    Case Order.enumOrderType.Delivery
                        .Item(intNextRow, 0) = "Delivery"
                        .Item(intNextRow, fgReport.Cols.Item("OrderType").Index) = "Delivery"
                        .Item(intNextRow, fgReport.Cols.Item("Customer").Index) = objOrder.TakeOutCustomerName

                    Case Order.enumOrderType.EatIn
                        .Item(intNextRow, 0) = "Eat In"
                        .Item(intNextRow, fgReport.Cols.Item("OrderType").Index) = "Eat In"
                        If objOrder.Customer Is Nothing Then
                            'nop
                        Else
                            .Item(intNextRow, fgReport.Cols.Item("Customer").Index) = objOrder.Customer.CustomerName
                        End If
                    Case Order.enumOrderType.Pickup
                        .Item(intNextRow, fgReport.Cols.Item("OrderType").Index) = "Pick Up"
                        .Item(intNextRow, fgReport.Cols.Item("Customer").Index) = objOrder.TakeOutCustomerName
                End Select

                .Item(intNextRow, fgReport.Cols.Item("BaseAmount").Index) = objOrder.OrderBaseAmount
                .Item(intNextRow, fgReport.Cols.Item("Seating").Index) = objOrder.Table.TableName + "-" + objOrder.TableSeating.ToString

                intNextRow += 1

            Next
            rg = fgReport.GetCellRange(0, 0, intNextRow - 1, fgReport.Cols.Count - 1)
            rg.StyleNew.WordWrap = True
        End With

    End Sub
    Private Sub SetfgReportStyles()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        Dim rg As C1.Win.C1FlexGrid.CellRange
        cs = fgReport.Styles.Add("Header")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.LightBlue
            .ForeColor = Color.DarkBlue
        End With

        cs = fgReport.Styles.Add("Void")
        With cs
            .Font = New Font(fgReport.Font, FontStyle.Strikeout)
            .ForeColor = Color.Red
        End With

        cs = fgReport.Styles.Add("OverridenOld")
        With cs
            .Font = New Font(fgReport.Font, FontStyle.Strikeout)
            .ForeColor = Color.Red
        End With

        cs = fgReport.Styles.Add("OverridenNew")
        With cs
            .Font = New Font(fgReport.Font, FontStyle.Bold)
            .ForeColor = Color.Green
        End With

        cs = fgReport.Styles.Add("DEError")
        With cs
            .BackColor = Color.Red
            .ForeColor = Color.Black
        End With
    End Sub

    Private Property FutureOrdersForAPeriod() As Orders
        Get
            Return m_Orders
        End Get
        Set(ByVal Value As Orders)
            m_Orders = Value
        End Set
    End Property

    Private Property Managers() As Employees
        Get
            Return m_Managers
        End Get
        Set(ByVal Value As Employees)
            m_Managers = Value
        End Set
    End Property



    Private Sub m_frmReportviewer_RunReport() Handles m_frmReportviewer.RunReport
        FutureOrdersForAPeriod = New Orders(m_frmReportviewer.DateFrom, m_frmReportviewer.DateTo)
        LoadData()
    End Sub

    Private Sub fgReport_AfterDragColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.DragRowColEventArgs) Handles fgReport.AfterDragColumn
        If e.Col <= 2 Then      'cols 4 and later are data columns
            LoadData()
        End If
    End Sub

    Private Sub SortFutureOrderGrid(ByVal fgGrid As C1.Win.C1FlexGrid.C1FlexGrid)
        Dim sortCol As C1.Win.C1FlexGrid.Column

        With fgGrid
            If fgGrid Is fgReport Then
                sortCol = SortColumnFutureOrderGrid

            Else
                Exit Sub
            End If

            If sortCol.Sort = C1FlexGrid.SortFlags.Ascending Then
                sortCol.Sort = C1FlexGrid.SortFlags.Descending
            Else
                sortCol.Sort = C1FlexGrid.SortFlags.Ascending
            End If

            '.Sort(C1FlexGrid.SortFlags.UseColSort, sortCol.Index)
            .Tree.Sort(0, C1FlexGrid.SortFlags.UseColSort, sortCol.Index, sortCol.Index)
        End With
    End Sub


    Private Property SortColumnFutureOrderGrid() As C1.Win.C1FlexGrid.Column
        Get
            If m_SortColumnFutureOrderGrid Is Nothing Then
                Return fgReport.Cols.Item(0)
            Else
                Return m_SortColumnFutureOrderGrid
            End If
        End Get
        Set(ByVal Value As C1.Win.C1FlexGrid.Column)
            m_SortColumnFutureOrderGrid = Value
        End Set
    End Property

    Private Sub SortReport()
        If fgReport.Cols("OrderType").Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols("OrderType").Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols("OrderType").Sort = C1FlexGrid.SortFlags.Ascending
        End If

        If fgReport.Cols("PromisedDate").Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols("PromisedDate").Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols("PromisedDate").Sort = C1FlexGrid.SortFlags.Ascending
        End If

        If fgReport.Cols("PromisedTime").Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols("PromisedTime").Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols("PromisedTime").Sort = C1FlexGrid.SortFlags.Ascending
        End If

        If fgReport.Cols("DateEntered").Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols("DateEntered").Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols("DateEntered").Sort = C1FlexGrid.SortFlags.Ascending
        End If
        If fgReport.Cols("BaseAmount").Sort = C1.Win.C1FlexGrid.SortFlags.Ascending Then
            fgReport.Cols("BaseAmount").Sort = C1FlexGrid.SortFlags.Descending
        Else
            fgReport.Cols("BaseAmount").Sort = C1FlexGrid.SortFlags.Ascending
        End If

        fgReport.Sort(C1FlexGrid.SortFlags.UseColSort, 0, 2)
    End Sub


    Private Sub fgReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgReport.Click
        Dim fg As C1.Win.C1FlexGrid.C1FlexGrid
        Dim fgCol As C1.Win.C1FlexGrid.Column

        fg = CType(sender, C1FlexGrid.C1FlexGrid)
        Select Case fg.MouseRow < fg.Rows.Fixed
            Case True
                fgCol = fg.Cols.Item(fg.MouseCol)
                SortColumnFutureOrderGrid = fgCol
                SortFutureOrderGrid(fg)

        End Select

    End Sub
End Class

