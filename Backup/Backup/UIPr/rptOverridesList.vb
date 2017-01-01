Option Strict On
Imports BOPr
Imports C1.Win
Public Class rptOverridesList

    Friend WithEvents fgReport As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents m_frmReportviewer As frmFGReportViewer
    Private m_Overrides As OverridesCol
    Private m_Managers As Employees
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
            .Cols.Count = 5
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
            .Width = 300
            .WidthDisplay = 240
            fgReport(0, 0) = "Override Type"
            .Name = "OType"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True


        End With
        With fgReport.Cols(1)
            .Width = 140
            .WidthDisplay = 120
            fgReport(0, 1) = "Done By"
            .Name = "ODoneBy"
            .DataType = GetType(String)
            .AllowDragging = True
            .AllowSorting = True
        End With
        With fgReport.Cols(2)
            .Width = 140
            .WidthDisplay = 120
            fgReport(0, 2) = "Done At"
            .Name = "ODoneAt"
            .DataType = GetType(DateTime)
            .Format = "ddd MM/dd/yy hh:mm tt"
            .AllowDragging = True
            .AllowSorting = True
        End With

        With fgReport.Cols(3)
            .Width = 200
            .WidthDisplay = 160
            fgReport(0, 3) = "What Happened"
            .Name = "OContext"
            .DataType = GetType(String)
            .AllowSorting = True
        End With

        With fgReport.Cols(4)
            .Width = 200
            .WidthDisplay = 160
            fgReport(0, 4) = "OReason"
            .Name = "OReason"
            .DataType = GetType(String)
            .AllowSorting = True
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
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Count, -1, -1, fgReport.Cols.Item("ODoneBy").Index, "Grand Total")

        ' total per Override Type(column 0)
        fgReport.Subtotal(C1FlexGrid.AggregateEnum.Count, 0, 0, fgReport.Cols.Item("ODoneBy").Index, "Total Overrides : ")

        ' total per Employee(column 1)
        'fgReport.Subtotal(C1FlexGrid.AggregateEnum.Count, 1, 1, 0, "Week # {0}")
    End Sub

    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.OverridesList
            .AllowFutureDates = False
            .Show()
        End With
    End Sub

    Private Sub LoadData()
        Dim fgrow As C1.Win.C1FlexGrid.Row

        fgReport.Redraw = False
        'Clear the grid first
        fgReport.Rows.Count = fgReport.Rows.Fixed
        LoadOverrides()
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

    Private Sub LoadOverrides()

        Dim objOverride As Override
        Dim intNextRow As Integer
        Dim rg As C1.Win.C1FlexGrid.CellRange

        With fgReport
            intNextRow = fgReport.Rows.Count
            .Rows.Count += OverridesForAPeriod.Count

            For Each objOverride In OverridesForAPeriod
                Select Case objOverride.OverrideType


                    Case Override.enumOverrideType.AddOrderItem
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "Added an Item to Order After First KOT Printed"

                    Case Override.enumOverrideType.ClockEdit
                        .Item(intNextRow, 0) = "Changed Clock In/Out Time"

                    Case Override.enumOverrideType.EditCRTXN
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = " Entered or Changed a petty cash Transaction"

                    Case Override.enumOverrideType.EditMoneyCount
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "Changed CashRegister Count"

                    Case Override.enumOverrideType.EditOrderItem
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "Changed an Item In the Order"

                    Case Override.enumOverrideType.EditPayment
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "Changed a Payment "

                    Case Override.enumOverrideType.MarkPaymentReceivedButNotEntered
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = " Payment is Marked As Received without entering it "

                    Case Override.enumOverrideType.PrintGuestCheck
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "Additional Guest Check Printed"

                    Case Override.enumOverrideType.PrintKOT
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "KOT Printed Again"

                    Case Override.enumOverrideType.ReconciliationCancelApproval
                        .Item(intNextRow, fgReport.Cols.Item("OType").Index) = "Reconciliation Report Approve/Cancel"
                End Select

                .Item(intNextRow, fgReport.Cols.Item("ODoneBy").Index) = Managers.Item(Managers.IndexOf(objOverride.OverrideBy)).EmployeeFullName
                .Item(intNextRow, fgReport.Cols.Item("ODoneAt").Index) = objOverride.OverrideAt
                .Item(intNextRow, fgReport.Cols.Item("OContext").Index) = objOverride.OverrideContext
                .Item(intNextRow, fgReport.Cols.Item("OReason").Index) = objOverride.OverrideReason

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

    Private Property OverridesForAPeriod() As OverridesCol
        Get
            Return m_Overrides
        End Get
        Set(ByVal Value As OverridesCol)
            m_Overrides = Value
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
        OverridesForAPeriod = New OverridesCol(m_frmReportviewer.DateFrom, m_frmReportviewer.DateTo)
        Managers = SaleDays.CreateSaleDay.ActiveEmployees(Employees.EnumView.CompleteView)
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

