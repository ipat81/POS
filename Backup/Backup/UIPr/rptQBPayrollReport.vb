Option Strict On
Imports BOPr
Imports C1.Win
Imports System.Globalization
Public Class rptQBPayrollReport
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
            SetfgReportStyles()
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
            .Width = 300
            .WidthDisplay = 240
            fgReport(0, 0) = "Employee"
            .Name = "Employee"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(1)
            .Width = 60
            .WidthDisplay = 60
            fgReport(0, 1) = "Week"
            .Name = "PayrollWeek"
            .DataType = GetType(Integer)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(2)
            .Width = 160
            .WidthDisplay = 140
            fgReport(0, 2) = "Date"
            .Name = "SaleDate"
            .DataType = GetType(Date)
            .Format = "ddd MM/dd/yy"
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(3)
            fgReport(0, 3) = "Clock In"
            .Name = "ClockIn"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(4)
            fgReport(0, 4) = "Clock Out"
            .Name = "ClockOut"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(5)
            fgReport(0, 5) = "Hours:Mins"
            .Name = "HoursWorked1"
            .DataType = GetType(String)
        End With
        With fgReport.Cols(6)
            fgReport(0, 6) = "Hours.Mins"
            .Name = "HoursWorked2"
            .DataType = GetType(Double)
            .Format = "##.##"
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
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, -1, -1, 6, "Grand Total")

        ' total per Employee (column 0)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 0, 0, 6, "Total " & fgReport.Cols.Item(0).Name & "{0}")

        ' total per Week per Employee(column 1)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Sum, 1, 1, 6, "Total " & fgReport.Cols.Item(1).Name & "{0}")
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer, ByVal mode As String)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.QBPayrollReport
            .AllowFutureDates = False
            If mode = "AUTO" Then       'run from fixpayroll
                LoadData()
            Else
                .Show()                 'run from menu
            End If
        End With

    End Sub

    Private Sub LoadData()
        Dim fgrow As C1.Win.C1FlexGrid.Row

        fgReport.Redraw = False
        'Clear the grid first
        fgReport.Rows.Count = fgReport.Rows.Fixed
        LoadClocks()
        SortReport()
        SetSubTotalTree()
        ' size column widths based on content
        fgReport.AutoSizeCols(2, fgReport.Cols.Count - 1, 25)
        For Each fgrow In fgReport.Rows
            fgrow.HeightDisplay = 35
        Next

        fgReport.Tree.Show(3)
        fgReport.Redraw = True
        m_frmReportviewer.ReportGrid = fgReport

    End Sub

    Private Sub LoadClocks()
        Dim objClocks As Clocks
        Dim objClock As Clock
        Dim objEmployees As Employees
        Dim intNextRow As Integer
        objClocks = New Clocks(m_frmReportviewer.DateFrom, m_frmReportviewer.DateTo)
        objEmployees = SaleDays.CreateSaleDay.ActiveEmployees(Employees.EnumView.CompleteView)

        With fgReport
            intNextRow = fgReport.Rows.Count
            .Rows.Count += objClocks.Count

            For Each objClock In objClocks
                .Item(intNextRow, "Employee") = objEmployees.Item(objEmployees.IndexOf(objClock.EmployeeId)).EmployeeFullName
                .Item(intNextRow, "PayrollWeek") = objClock.ClockWeek
                .Item(intNextRow, "SaleDate") = objClock.ClockTimeIn.Date
                .Item(intNextRow, "ClockIn") = objClock.ClockTimeIn.ToShortTimeString
                If objClock.ClockTimeOut = DateTime.MinValue Then
                    '.Item(introw, 4) = " "
                Else
                    .Item(intNextRow, "ClockOut") = objClock.ClockTimeOut.ToShortTimeString
                End If
                .Item(intNextRow, "HoursWorked1") = objClock.TimeWorkedDisplay
                .Item(intNextRow, "HoursWorked2") = Math.Floor(objClock.TimeWorked.TotalMinutes) / 60
                Select Case objClock.ClockStatus
                    Case Clock.enumClockStatus.OverriddenOld
                        fgReport.SetCellStyle(intNextRow, fgReport.Cols.Item("ClockIn").Index, fgReport.Styles.Item("OverridenOld"))
                        fgReport.SetCellStyle(intNextRow, fgReport.Cols.Item("ClockOut").Index, fgReport.Styles.Item("OverridenOld"))
                    Case Clock.enumClockStatus.OverriddenNew
                        fgReport.SetCellStyle(intNextRow, fgReport.Cols.Item("ClockIn").Index, fgReport.Styles.Item("OverridenNew"))
                        fgReport.SetCellStyle(intNextRow, fgReport.Cols.Item("ClockOut").Index, fgReport.Styles.Item("OverridenNew"))
                    Case Clock.enumClockStatus.Active
                        'nop
                    Case Clock.enumClockStatus.Voided
                        fgReport.Rows.Item(intNextRow).Style = fgReport.Styles.Item("VoidItem")
                End Select

                intNextRow += 1
            Next
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

        fgReport.Sort(C1FlexGrid.SortFlags.UseColSort, 0, 2)
    End Sub

    Private Sub fgReport_AfterDragColumn(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.DragRowColEventArgs) Handles fgReport.AfterDragColumn
        If e.Col <= 2 Then      'cols 4 and later are data columns
            LoadData()
        End If
    End Sub
End Class

