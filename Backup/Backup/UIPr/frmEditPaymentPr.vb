Option Strict On
Imports C1.Win
Imports System.Windows.Forms.Control
Imports BOPr
Imports System.Collections.Specialized

Public Class frmEditPaymentPr
    Inherits System.Windows.Forms.Form
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        SetUI()
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
        Me.Text = "frmEditPaymentPr"
    End Sub

#End Region

    Friend Event ParentOrderChanged(ByVal newOrder As Order)
    Friend Event FetchNextOrder()

    Private pnlPayment As Panel
    Private WithEvents fgPayment As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents cboPaymentMethods As ComboBox
    Private WithEvents fgPaymentTotals As C1.Win.C1FlexGrid.C1FlexGrid

    Private panelFormBtns As Panel
    Private WithEvents btnClose As Button
    Private WithEvents btnSaveClose As Button
    Private WithEvents btnSaveNext As Button
    Private WithEvents btnReset As Button
    Private WithEvents btnVoid As Button

    Private m_ParentOrder As Order
    Private m_SelectedOrders As Orders
    Private m_objPayments As Payments

    Friend Property ParentOrder() As Order
        Get
            Return m_ParentOrder
        End Get
        Set(ByVal Value As Order)
            m_ParentOrder = Value
            If m_ParentOrder Is Nothing Then
                'nop
            Else
                LoadfgPayment()
            End If
        End Set
    End Property

    Friend Property SelectedOrders() As Orders
        Get
            Return m_SelectedOrders
        End Get
        Set(ByVal Value As Orders)
            m_SelectedOrders = Value
        End Set
    End Property
    Private Sub frmEditPaymentPr_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        EnableDisableFormBtns()
    End Sub

    Private Sub LoadfgPayment()
        Dim objPayment As Payment
        Dim introw As Integer
        Dim intselRow As Integer
        Dim intselCol As Integer
        'save the selected cell before load to restore after load 
        With fgPayment
            intselCol = .ColSel
            intselRow = .RowSel
        End With

        introw = fgPayment.Rows.Fixed
        With fgPayment
            .SuspendLayout()
            .Rows.Count = .Rows.Fixed   'to clear data rows without losing column header fixed row
            .Rows.Count = 20
        End With
        LoadPaymentGridHeader()
        With ParentOrder
            For Each objPayment In .AllPayments
                RefreshfgPaymentRow(introw, objPayment)
                introw += 1
            Next
        End With
        loadSubTotals()
        'restore the selected cell before load 
        With fgPayment
            .Select(intselRow, intselCol)
            .ShowCell(intselRow, intselCol)
            .ResumeLayout()
        End With
        EnableDisableFormBtns()
    End Sub

    Private Sub loadSubTotals()
        Dim introw As Integer
        fgPayment.Subtotal(C1FlexGrid.AggregateEnum.None)
        fgPayment.SubtotalPosition = C1FlexGrid.SubtotalPositionEnum.BelowData
        fgPayment.Rows.Item(fgPayment.Rows.Count - 1).Style = fgPayment.Styles.Item("Footer")
        fgPayment.Rows.Item(fgPayment.Rows.Count - 1).Height = fgPayment.Rows.DefaultSize + 5
        introw = fgPayment.Rows.Count - 1
        With ParentOrder
            'Order level Totals in each  col
            fgPayment(introw, 1) = "Total Paid"
            fgPayment(introw, 2) = .POSAmountPaid
            fgPayment(introw, 3) = .TipAmountPaid
            fgPayment(introw, 4) = .AmountTendered
            fgPayment(introw, 5) = .ChangeAmountReturned
        End With
    End Sub

    Private Sub RefreshfgPaymentRow(ByVal introw As Integer, ByVal objPayment As Payment)
        Dim row As C1.Win.C1FlexGrid.Row

        With fgPayment
            .Rows.Item(introw).UserData = ParentOrder.AllPayments.IndexOf(objPayment)
            Select Case objPayment.Status
                Case Payment.enumStatus.NewPayment
                    'nop : keep it blank
                Case Payment.enumStatus.Active, Payment.enumStatus.Voided

                    .Item(introw, 0) = introw - .Rows.Fixed + 1
                    .Item(introw, 1) = objPayment.PaymentMethod.ToString
                    .Item(introw, 2) = objPayment.POSAmountPaid
                    .Item(introw, 3) = objPayment.TipAmountPaid
                    .Item(introw, 4) = objPayment.AmountTendered()
                    .Item(introw, 5) = objPayment.ChangeAmountReturned
                    If objPayment.Status = Payment.enumStatus.Voided Then
                        fgPayment.Rows.Item(introw).Style = fgPayment.Styles.Item("VoidItem")
                    End If
                    Select Case objPayment.DEError
                        Case Payment.enumPaymentDEError.NoError
                            .Item(introw, 6) = String.Empty
                        Case Else
                            fgPayment.SetCellStyle(introw, 6, fgPayment.Styles.Item("DEError"))
                            Select Case objPayment.DEError
                                Case Payment.enumPaymentDEError.AmountTenderedDoesNotMatchOtherAmounts
                                    .Item(introw, 6) = "Individual amounts do not add up to Amount Tendered."
                                Case Payment.enumPaymentDEError.ChkAmountExceedsPOSAmount
                                    .Item(introw, 6) = "Check Amount Paid is more than Total Order Amount."
                                Case Payment.enumPaymentDEError.TipsPaidAreLessThanOrderTips
                                    .Item(introw, 6) = "Total Tip Paid must not be less than Tip added to Order."
                                Case Payment.enumPaymentDEError.CustomerDoesNotHaveHouseAccount
                                    .Item(introw, 6) = "This Customer Does Not Have a House Account."
                                Case Payment.enumPaymentDEError.GuestCheckNotPrinted
                                    .Item(introw, 6) = "Check Not printed!"
                            End Select
                    End Select
            End Select
        End With
    End Sub

    Private Sub EnableDisableFormBtns()
        Select Case ParentOrder.AllPayments.IsDirty
            Case True
                btnSaveClose.Enabled = True
                btnReset.Enabled = True
                btnReset.Enabled = True
            Case False
                btnSaveClose.Enabled = False
                btnReset.Enabled = False
        End Select

        If (SelectedOrders Is Nothing) OrElse (SelectedOrders.Count <= 1) Then
            btnSaveNext.Enabled = False
        Else
            btnSaveNext.Enabled = True
        End If
        If ParentOrder.AllPayments.Count > 0 Then
            btnVoid.Enabled = True
        Else
            btnVoid.Enabled = False
        End If
    End Sub

    Private Sub fgPayment_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) _
                Handles fgPayment.AfterEdit
        Dim rowno As Integer
        Dim objPayment As Payment
        Dim paymentType As String

        objPayment = SelectedPayment
        Select Case e.Col
            Case 0
                'nop : not editable
            Case 1
                ProcessNewPayment(CType(fgPayment(e.Row, e.Col), Payment.enumPaymentMethod))
            Case 2
                If objPayment Is Nothing Then Exit Sub
                objPayment.POSAmountPaid = CDbl(fgPayment(e.Row, e.Col))
                LoadfgPayment()
            Case 3
                If objPayment Is Nothing Then Exit Sub
                objPayment.TipAmountPaid = CDbl(fgPayment(e.Row, e.Col))
                LoadfgPayment()
            Case 4
                If objPayment Is Nothing Then Exit Sub
                objPayment.AmountTendered = CDbl(fgPayment(e.Row, e.Col))
                LoadfgPayment()
            Case 5
                If objPayment Is Nothing Then Exit Sub
                objPayment.ChangeAmountReturned = CDbl(fgPayment(e.Row, e.Col))
                LoadfgPayment()
        End Select
    End Sub

    Private Sub fgPayment_StartEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgPayment.StartEdit
        Select Case AllowFgPaymentEdit()
            Case False
                e.Cancel = True  'Terminate any edit action by user
                Exit Sub
            Case True
                'nop
        End Select
    End Sub

    Private ReadOnly Property SelectedPayment() As Payment
        Get
            Dim intPaymentIndex As Integer
            Dim objSelectedPayment As Payment
            If (ParentOrder Is Nothing) OrElse (fgPayment.RowSel < fgPayment.Rows.Fixed) Then
                Return Nothing
            Else
                intPaymentIndex = CInt(fgPayment.Rows(fgPayment.RowSel).UserData)
                If (intPaymentIndex < ParentOrder.AllPayments.Count) And _
                    (intPaymentIndex >= 0) Then
                    Return ParentOrder.AllPayments.Item(intPaymentIndex)
                Else
                    Return Nothing
                End If
            End If
        End Get
    End Property

    Private Sub fgPayment_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) _
                        Handles fgPayment.KeyPressEdit
        Dim dblValue As Double

        Select Case e.Col
            Case 2, 3, 4, 5
                Try
                    dblValue = CType(CStr(e.KeyChar & "1"), Double)
                    If dblValue > 0 Then
                        e.Handled = False
                    Else
                        e.Handled = True    'Discard  key presses by user resulting in negative number  
                    End If
                Catch ex As Exception
                    Select Case Val(e.KeyChar)
                        Case Keys.Back, Keys.Right, Keys.Left
                            e.Handled = False
                        Case Else
                            e.Handled = True    'Discard non numeric key presses by user  
                    End Select
                Finally
                End Try
        End Select
    End Sub

    Private Function AllowFgPaymentEdit() As Boolean
        'must obtain override at this stage 
        Return ParentOrder.AllowPaymentEdit
    End Function

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnReset.Click
        Dim objPayments As Payments
        ParentOrder.ResetAllPayments()
        LoadfgPayment()
    End Sub

    Private Sub SetUI()
        With Me
            .BackColor = System.Drawing.Color.White
            .ClientSize = New System.Drawing.Size(560, 462)
            .ControlBox = False
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .MaximizeBox = False
            .MinimizeBox = False
            .Name = "frmEditPaymentPr"
            .StartPosition = System.Windows.Forms.FormStartPosition.Manual

            SetpnlPayment()
            SetpanelFormBtns()
            .Controls.AddRange(New System.Windows.Forms.Control() _
            {.pnlPayment, .panelFormBtns})
        End With
    End Sub

    Private Sub SetpnlPayment()
        pnlPayment = New Panel()

        With Me.pnlPayment
            .Dock = System.Windows.Forms.DockStyle.Fill
            .BorderStyle = BorderStyle.Fixed3D
            .TabIndex = 1
            SetfgPayment()
            .Controls.AddRange(New System.Windows.Forms.Control() {Me.fgPayment})
        End With
    End Sub

    Private Sub SetpanelFormBtns()
        panelFormBtns = New Panel()
        With Me.panelFormBtns
            .Anchor = AnchorStyles.Right
            .Width = 60
            .Dock = System.Windows.Forms.DockStyle.Right
            .BorderStyle = BorderStyle.Fixed3D
            .AutoScroll = False
            SetFormBtns()
            .Controls.AddRange(New System.Windows.Forms.Control() _
                    {Me.btnSaveClose, Me.btnSaveNext, Me.btnClose, Me.btnReset, Me.btnVoid})
            .TabIndex = 5
        End With
    End Sub

    Private Sub SetFormBtns()
        btnClose = GetAButton(4)
        btnClose.Dock = DockStyle.Bottom
        btnClose.Text = "Close"

        btnSaveClose = GetAButton(0)
        btnSaveClose.Location = New Point(btnClose.Location.X, 40)
        btnSaveClose.Text = "Save & Close"

        btnSaveNext = GetAButton(1)
        btnSaveNext.Location = New Point(btnClose.Location.X, btnSaveClose.Location.Y + btnSaveClose.Height + 10)
        btnSaveNext.Text = "Save & Next"

        btnReset = GetAButton(2)
        btnReset.Location = New Point(btnClose.Location.X, btnSaveNext.Location.Y + btnSaveNext.Height + 10)
        btnReset.Text = "Reset"

        btnVoid = GetAButton(3)
        btnVoid.Location = New Point(btnClose.Location.X, btnReset.Location.Y + btnReset.Height + 10)
        btnVoid.Text = "Void"

    End Sub

    Private Function GetAButton(ByVal intTabSeq As Integer) As Button
        Dim btn As Button
        btn = New Button()
        With btn
            .Size = New System.Drawing.Size(Me.panelFormBtns.Width, 40)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .TabIndex = intTabSeq
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

        End With
        Return btn
    End Function

    Private Sub SetfgPayment()
        Dim fgCellstyle As C1.Win.C1FlexGrid.CellStyle

        fgPayment = New C1.Win.C1FlexGrid.C1FlexGrid()
        SetfgPaymentStyles()

        With fgPayment
            .Anchor = AnchorStyles.Bottom Or AnchorStyles.Top
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.None
            .Cols.Count = 7
            .Cols.Fixed = 0
            .Rows.Count = 29
            .Rows.Fixed = 3
            .ExtendLastCol = True

            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black

            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .Styles.EmptyArea.BackColor = Color.White
            .Styles.Alternate.BackColor = Color.AliceBlue
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowMerging = C1FlexGrid.AllowMergingEnum.FixedOnly
            .Rows(0).AllowMerging = True
            .Rows(1).AllowMerging = True
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.None
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .Font = New Font(Me.Font.FontFamily, 10.25, FontStyle.Regular)
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
        End With

        With fgPayment.Cols(0)
            .Width = 40
            .WidthDisplay = 40
            fgPayment(2, 0) = "#"
            .Name = "PaymentItemSeqNum"
            .AllowEditing = False
            .DataType = GetType(Integer)
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgPayment.Cols(1)
            .Width = 400
            .WidthDisplay = 200
            fgPayment(2, 1) = "Payment Method"
            .Name = "PaymentMethodName"
            .DataType = GetType(Payment.enumPaymentMethod)
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        End With

        With fgPayment.Cols(2)
            .Width = 140
            .WidthDisplay = 140
            fgPayment(2, 2) = "Check Amount"
            .Name = "ChkAmount"
            .DataType = GetType(Double)
            .Format = "C"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgPayment.Cols(3)
            .Width = 90
            .WidthDisplay = 90
            fgPayment(2, 3) = "  Tip"
            .Name = "Tip"
            .DataType = GetType(Double)
            .Format = "C"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgPayment.Cols(4)
            .Width = 140
            .WidthDisplay = 140
            fgPayment(2, 4) = " Total Paid"
            .Name = "TotalPaid"
            .DataType = GetType(Double)
            .Format = "C"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With
        With fgPayment.Cols(5)
            .Width = 90
            .WidthDisplay = 90
            fgPayment(2, 5) = "    Change"
            .Name = "Change"
            .DataType = GetType(Double)
            .Format = "C"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgPayment.Cols(6)
            .Width = 70
            .WidthDisplay = 70
            .AllowEditing = False
            fgPayment(2, 6) = "    "
            .Name = "DEError"
            .DataType = GetType(String)
        End With
    End Sub

    Private Sub SetfgPaymentStyles()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        Dim rg As C1.Win.C1FlexGrid.CellRange

        cs = fgPayment.Styles.Add("Header")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.SandyBrown
            .ForeColor = Color.Black
            .TextAlign = C1FlexGrid.TextAlignEnum.GeneralCenter
        End With

        cs = fgPayment.Styles.Add("Footer")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Moccasin
            .ForeColor = Color.Black
        End With

        cs = fgPayment.Styles.Add("VoidItem")
        With cs
            .Font = New Font(fgPayment.Font, FontStyle.Strikeout)
            .ForeColor = Color.Red
        End With

        cs = fgPayment.Styles.Add("DEError")
        With cs
            .Font = New Font(fgPayment.Font, FontStyle.Bold)
            .ForeColor = Color.Red
        End With
    End Sub

    Private Sub LoadPaymentGridHeader()
        Dim sb1 As New System.Text.StringBuilder()
        Dim sb2 As New System.Text.StringBuilder()
        Dim Row0AllCells As New C1.Win.C1FlexGrid.CellRange()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        Dim r As Integer
        Dim c As Integer
        cs = fgPayment.Styles("Header")
        For r = 0 To fgPayment.Rows.Fixed - 2           'col hdrs i.e. last fixed row is normal size
            fgPayment.Rows.Item(r).Height = fgPayment.Rows.DefaultSize + 5
            For c = 0 To fgPayment.Cols.Count - 1
                fgPayment.SetCellStyle(r, c, cs)
            Next
        Next

        Row0AllCells = fgPayment.GetCellRange(0, 0, 0, 6)

        Row0AllCells.Data = ("Enter Payments For").PadLeft(90)

        With ParentOrder
            Select Case .OrderType
                Case Order.enumOrderType.EatIn
                    If .Customer Is Nothing Then
                        sb1.Append(("Table: " & .Table.TableName & "-" & .TableSeating.ToString).PadLeft(20) & _
                                    (" Waiter: " & .Waiter.EmployeeShortName).PadLeft(24) & _
                                    (" Order At: " & Format(.EnteredAt, "hh:mm tt") & " " & _
                                                       Format(.EnteredAt, "MM/dd/yy")).PadLeft(40))
                    Else
                        sb1.Append(("Table: " & .Table.TableName & "-" & .TableSeating.ToString).PadLeft(20) & _
                                                           (" Waiter: " & .Waiter.EmployeeShortName).PadLeft(24) & _
                                                           (" Order At: " & Format(.EnteredAt, "hh:mm tt") & " " & _
                                                                              Format(.EnteredAt, "MM/dd/yy")).PadLeft(40) & _
                                                           (" Customer: " & .Customer.CustomerName).PadLeft(24))
                    End If
                Case Order.enumOrderType.Pickup
                    sb1.Append((" Pickup For : " & .TakeOutCustomerName & "  " & .TakeOutCustomerPhone).PadLeft(40) & _
                                    (" Waiter: " & .Waiter.EmployeeShortName).PadLeft(24) & _
                                    (" Order At: " & Format(.EnteredAt, "hh:mm tt") & " " & _
                                                       Format(.EnteredAt, "MM/dd/yy")).PadLeft(50))

                Case Order.enumOrderType.Delivery
                    sb1.Append((" Deliver To : " & .TakeOutCustomerName & "  " & .TakeOutCustomerPhone).PadLeft(40) & _
                                    (" Waiter: " & .Waiter.EmployeeShortName).PadLeft(24) & _
                                    (" Order At: " & Format(.EnteredAt, "hh:mm tt") & " " & _
                                                       Format(.EnteredAt, "MM/dd/yy")).PadLeft(50))

            End Select
        End With
        Row0AllCells = fgPayment.GetCellRange(1, 0, 1, 6)
        Row0AllCells.Data = sb1.ToString
    End Sub

    Private Sub ProcessNewPayment(ByVal enumnewPaymentMethod As Payment.enumPaymentMethod)
        Dim objPayment As Payment
        Dim objUserData As Object

        objUserData = fgPayment.Rows.Item(fgPayment.RowSel).UserData
        If objUserData Is Nothing Then
            objPayment = ParentOrder.AddPayment
            objPayment.Status = Payment.enumStatus.NewPayment
        Else
            objPayment = ParentOrder.AllPayments.Item(CInt(objUserData))
        End If
        With objPayment
            Select Case .Status
                Case Payment.enumStatus.NewPayment
                    .PaymentMethod = enumnewPaymentMethod
                    .Status = Payment.enumStatus.Active

                    .POSAmountPaid = ParentOrder.BalancePOSAmount
                    .TipAmountPaid = ParentOrder.BalanceTipAmount
                Case Payment.enumStatus.Active
                    .PaymentMethod = enumnewPaymentMethod
                Case Else
                    'nop
            End Select
        End With

        LoadfgPayment()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ParentOrder.SetPaymentEndTime()
        Me.Close()
    End Sub

    Private Sub btnSaveClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveClose.Click
        ParentOrder.SetPaymentEndTime()
        RaiseEvent FetchNextOrder()
        Me.Close()
    End Sub

    Private Sub btnSaveNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNext.Click
        Dim intIndex As Integer

        ParentOrder.SetPaymentEndTime()
        RaiseEvent FetchNextOrder()
        'With SelectedOrders
        '    intIndex = .IndexOf(ParentOrder)
        '    If intIndex >= .Count - 1 Then
        '        intIndex = 0
        '    Else
        '        intIndex += 1
        '    End If
        '    ParentOrder = .Item(intIndex)
        '    RaiseEvent ParentOrderChanged(ParentOrder)
        'End With
        If ParentOrder Is Nothing Then
            Me.Close()
        Else
            fgPayment.Focus()
            fgPayment.Select(fgPayment.Rows.Fixed, 1)
        End If
    End Sub

    Private Sub fgPayment_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgPayment.EnterCell
        Dim intNextRow As Integer
        Dim blnActiveRow As Boolean
        Dim objUserData As Object
        Dim objPayment As Payment

        With fgPayment
            If .RowSel < .Rows.Fixed Then .RowSel = .Rows.Fixed - 1
            Do Until blnActiveRow
                objUserData = .Rows.Item(.RowSel).UserData
                If objUserData Is Nothing Then
                    objPayment = Nothing
                    Exit Do
                Else
                    objPayment = ParentOrder.AllPayments.Item(CInt(objUserData))
                End If
                If (objPayment.Status = Payment.enumStatus.Voided) Then
                    .Select((Math.Min(.RowSel + 1, .Rows.Count - 1)), .ColSel)
                    If .RowSel = .Rows.Count - 1 Then blnActiveRow = True
                Else
                    blnActiveRow = True
                End If
            Loop

            Select Case .ColSel
                Case Is < 0
                    Exit Sub
                Case 0
                    .AutoSearch = C1FlexGrid.AutoSearchEnum.FromTop
                Case 1, 2, 3, 4, 5
                    .AutoSearch = C1FlexGrid.AutoSearchEnum.None
                Case Else
                    Select Case .ColSel = .Cols.Count - 1
                        Case True
                            If .RowSel = .Rows.Count - 1 Then
                                intNextRow = .Rows.Fixed()
                            Else
                                intNextRow = .RowSel + 1
                            End If
                            .Select(intNextRow, 0)
                            .ShowCell(intNextRow, 0)
                        Case False
                            .Select(.RowSel, .ColSel + 1)
                            .ShowCell(.RowSel, .ColSel + 1)
                    End Select
            End Select
        End With
    End Sub

    Private Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click
        With fgPayment
            If (.RowSel < .Rows.Fixed) OrElse (.RowSel >= .Rows.Count) Then
                MsgBox("First select an item to void and then click Void.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            Select Case ParentOrder.AllowPaymentEdit
                Case True
                    ParentOrder.VoidAPayment(CInt(.Rows.Item(.RowSel).UserData))
                    LoadfgPayment()
                Case False
                    'nop
            End Select
        End With
    End Sub

    Private Sub fgPayment_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles fgPayment.KeyDown
        Dim intColSel As Integer
        Dim intRowSel As Integer
        Select Case e.KeyCode
            Case Keys.Right
                If fgPayment.ColSel < fgPayment.Cols.Count - 1 Then
                    intColSel = intColSel - 1
                Else
                    intColSel = fgPayment.Cols.Count - 1
                End If
                Select Case intColSel
                    Case 2, 3, 4, 5
                        'nope
                    Case fgPayment.Cols.Count - 1
                        e.Handled = True
                        With fgPayment
                            .ColSel = 1
                            If .RowSel = .Rows.Count - 1 Then
                                .RowSel = .Rows.Fixed
                            Else
                                .RowSel = fgPayment.RowSel + 1
                            End If
                            .Select(.RowSel, .ColSel, True)
                        End With
                End Select
        End Select
    End Sub
End Class

