Option Strict On
Imports C1.Win
Imports BOPr
Imports System.Collections.Specialized

Public Class frmEditCRTxn
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
        Me.Text = "frmEditCRTxn"
    End Sub

#End Region

    Friend WithEvents fgCrTXNs As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents fromdtePicker As DateTimePicker
    Friend WithEvents TodtePicker As DateTimePicker
    Friend panel1 As Panel
    Friend lblFrom As Label
    Friend lblTo As Label
    Friend lblTitle As Label
    Friend WithEvents btnOK As Button
    Friend WithEvents btnVoid As Button
    Private WithEvents m_CrTxns As CRTXNs
    Private WithEvents m_CRTXN As CRTXN

    Friend Event StatusChanged(ByVal StatusMsg As String)
    ' Private m_Employees As Employees

    Private Sub SetUI()
        Me.AutoScaleBaseSize = New System.Drawing.Size(7, 16)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1007, 460)
        Me.ControlBox = False
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        'Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEditCRTxn"
        Me.ShowInTaskbar = False
        Me.Text = "Cash Register Transactions"

        setPanelProperties()
        SetfgCrTXNsProperties()
        SetbtnOkProperties()
        SetbtnVoidProperties()

        Me.Controls.AddRange(New Control() {panel1, fgCrTXNs, btnOK, btnVoid})
    End Sub

    Private Property AllCrTransactions() As CRTXNs
        Get
            Return m_CrTxns
        End Get
        Set(ByVal Value As CRTXNs)
            m_CrTxns = Value
        End Set
    End Property

    Private Property CurrentlyEditedCrTXn() As CRTXN
        Get
            Return m_CRTXN()
        End Get
        Set(ByVal Value As CRTXN)
            m_CRTXN = Value
        End Set
    End Property

    Private Sub SetfgCrTXNsProperties()
        Dim objEmployee As Employee

        fgCrTXNs = New C1.Win.C1FlexGrid.C1FlexGrid()
        SetfgCrTXNsStyles()

        With fgCrTXNs
            '.Anchor = AnchorStyles.Bottom Or AnchorStyles.Top
            '.Dock = DockStyle.Fill
            .Size = New System.Drawing.Size(966, 246)
            .Location = New System.Drawing.Point(25, 140)
            .ScrollBars = ScrollBars.Vertical
            .BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.FixedSingle
            .Cols.Count = 7
            .Cols.Fixed = 0
            .Rows.Fixed = 1
            .Rows.Count = .Rows.Fixed
            .ExtendLastCol = True
            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .Styles.EmptyArea.BackColor = Color.White
            .Styles.Alternate.BackColor = Color.BlanchedAlmond
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black

            .AllowEditing = True

            '.AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.None
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.None
            .Font = New Font(Me.Font.FontFamily, 10.25, FontStyle.Regular)
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
        End With

        With fgCrTXNs.Cols(0)
            .Width = 40
            .WidthDisplay = 40
            fgCrTXNs(0, 0) = "#"
            .Name = "CRTXnsSeqNum"
            .AllowEditing = False
            .DataType = GetType(Integer)
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgCrTXNs.Cols(1)

            .Width = 100
            .WidthDisplay = 100
            fgCrTXNs(0, 1) = "Date"
            .Name = "CrTXNDate"
            .DataType = GetType(DateTime)
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .AllowEditing = False

        End With

        With fgCrTXNs.Cols(2)
            .Width = 200
            .WidthDisplay = 200
            fgCrTXNs(0, 2) = "TXn Type"
            .Name = "TXNType"
            .DataType = GetType(CRTXN.enumCRTXNType)
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .AllowEditing = True

        End With

        With fgCrTXNs.Cols(3)
            .Width = 90
            .WidthDisplay = 90
            fgCrTXNs(0, 3) = "  Amount"
            .Name = "Amount"
            .DataType = GetType(Double)
            .Format = "C"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowEditing = True
        End With

        With fgCrTXNs.Cols(4)
            .Width = 140
            .WidthDisplay = 140
            fgCrTXNs(0, 4) = " Employee Name"
            .Name = "EmployeeName"

            ' .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowEditing = True


            .DataType = GetType(Integer)

            .DataMap = SaleDays.CreateSaleDay.ActiveCashierList
        End With
        With fgCrTXNs.Cols(5)
            .Width = 140
            .WidthDisplay = 140
            fgCrTXNs(0, 5) = " Receipt Given "
            .Name = "ReceiptGiven"
            .DataType = GetType(Boolean)
            .Caption = "Receipt Given"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowEditing = True
        End With

        With fgCrTXNs.Cols(6)
            .Width = 250
            .WidthDisplay = 250
            .AllowEditing = True
            fgCrTXNs(0, 6) = " Description   "
            .Name = "Description"
            .DataType = GetType(String)
            .AllowEditing = True

        End With

    End Sub

    Private Sub SetfgCrTXNsStyles()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        Dim rg As C1.Win.C1FlexGrid.CellRange
        cs = fgCrTXNs.Styles.Add("Header")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.LightBlue
            .ForeColor = Color.DarkBlue
        End With

        cs = fgCrTXNs.Styles.Add("Void")
        With cs
            .Font = New Font(fgCrTXNs.Font, FontStyle.Strikeout)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ForeColor = Color.Red
        End With

    End Sub


    Private Sub SetfromdtePickerproperties()
        fromdtePicker = New DateTimePicker()
        getDatePickerProperties(fromdtePicker)
        With fromdtePicker

            .Location = New System.Drawing.Point(140, 5)
            .TabIndex = 1
        End With
    End Sub

    Private Sub SettodtePickerproperties()
        TodtePicker = New DateTimePicker()
        getDatePickerProperties(TodtePicker)
        With TodtePicker

            .Location = New System.Drawing.Point(330, 5)
            .TabIndex = 1
        End With
    End Sub

    Private Sub getDatePickerProperties(ByVal dtePicker As DateTimePicker)



        With dtePicker
            .Size = New System.Drawing.Size(110, 25)
            .CalendarMonthBackground = Color.FromKnownColor(KnownColor.Control)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Format = DateTimePickerFormat.Short
            .Visible = True
            .MaxDate = Today

        End With

    End Sub


    Private Sub setPanelProperties()
        panel1 = New Panel()
        With panel1
            .SuspendLayout()
            .Size = New System.Drawing.Size(966, 50)
            .Location = New System.Drawing.Point(25, 25)
            .BackColor = Color.SandyBrown
            .Visible = True
        End With
        SetfromLabelProperties()
        SetfromdtePickerproperties()
        SetToLabelproperties()
        SettodtePickerproperties()
        SetlblTitleProperties()
        panel1.Controls.AddRange(New Control() {lblFrom, fromdtePicker, lblTo, TodtePicker, lblTitle})
        panel1.ResumeLayout()
    End Sub

    Private Sub SetfromLabelProperties()
        lblFrom = New Label()
        GetLabelProperties(lblFrom)
        With lblFrom
            .Location = New System.Drawing.Point(50, 5)
            .Text = " From "
        End With
    End Sub

    Private Sub SetToLabelproperties()
        lblTo = New Label()
        GetLabelProperties(lblTo)
        With lblTo
            .Location = New System.Drawing.Point(240, 5)
            .Text = " To "
        End With

    End Sub

    Private Sub SetlblTitleProperties()
        lblTitle = New Label()
        GetLabelProperties(lblTitle)
        With lblTitle
            .Size = New System.Drawing.Size(180, 25)
            .Location = New System.Drawing.Point(460, 5)
            .Text = "Petty Cash Transactions"
        End With
    End Sub

    Private Sub GetLabelProperties(ByVal lbl As Label)


        With lbl
            .Size = New System.Drawing.Size(80, 25)
            .BackColor = Color.SandyBrown
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

    End Sub

    Private Sub getButtonProperties(ByVal btn As Button)
        With btn
            .Size = New System.Drawing.Size(75, 50)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, _
                        System.Drawing.FontStyle.Regular, _
                         System.Drawing.GraphicsUnit.Point, _
                         CType(0, Byte))
            .Enabled = True
            .Visible = True

        End With

    End Sub


    Private Sub SetbtnOkProperties()
        btnOK = New Button()
        getButtonProperties(btnOK)

        With btnOK
            .Location = New System.Drawing.Point(840, 400)
            .Text = "Save + Close"
        End With

    End Sub

    Private Sub SetbtnVoidProperties()
        btnVoid = New Button()
        getButtonProperties(btnVoid)

        With btnVoid
            .Location = New System.Drawing.Point(925, 400)
            .Text = "Void"
        End With

    End Sub

    Private Sub frmEditCRTxn_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadCRTXnCollection()
        LoadfgCRTXN()
        With fgCrTXNs
            .RowSel = fgCrTXNs.Rows.Fixed
            .ColSel = 2
            .Select(.RowSel, .ColSel)
            .ShowCell(.RowSel, .ColSel)
            .Focus()
        End With

    End Sub

    Private Sub fromdtePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles fromdtePicker.ValueChanged

        If TodtePicker Is Nothing Then Exit Sub
        LoadCRTXnCollection()
        If fgCrTXNs Is Nothing Then Exit Sub
        LoadfgCRTXN()

    End Sub


    Private Sub TodtePicker_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TodtePicker.ValueChanged
        If fromdtePicker Is Nothing Then Exit Sub
        LoadCRTXnCollection()
        If fgCrTXNs Is Nothing Then Exit Sub
        LoadfgCRTXN()

    End Sub


    Private Sub LoadCRTXnCollection()
        If fromdtePicker.Value = Nothing Or TodtePicker.Value = Nothing Then
            'nope
        Else

            AllCrTransactions = New CRTXNs(CRTXNs.EnumFilter.Active, CRTXNs.EnumView.CompleteView, fromdtePicker.Value, TodtePicker.Value)
        End If
        AllCrTransactions.AddCRTXN()
    End Sub



    Private Sub LoadfgCRTXN()
        Dim objCRTXn As CRTXN
        Dim introw As Integer
        Dim intselRow As Integer
        Dim intselCol As Integer
        'ParentOrder.AllPayments.AddNewPaymentObject()
        'save the selected cell before load to restore after load 
        With fgCrTXNs
            intselCol = .ColSel
            intselRow = .RowSel
        End With

        introw = fgCrTXNs.Rows.Fixed
        With fgCrTXNs
            .SuspendLayout()
            .Rows.Count = .Rows.Fixed   'to clear data rows without losing column header fixed row
            .Rows.Count = AllCrTransactions.Count + .Rows.Fixed
        End With
        For Each objCRTXn In AllCrTransactions
            RefreshfgCRTXnRow(introw, objCRTXn)
            introw += 1
        Next

        With fgCrTXNs
            .Select(intselRow, intselCol)
            .ShowCell(intselRow, intselCol)
            .ResumeLayout()
        End With

    End Sub


    Private Sub RefreshfgCRTXnRow(ByVal introw As Integer, ByVal objCrTxn As CRTXN)

        Dim row As C1.Win.C1FlexGrid.Row

        With fgCrTXNs
            .Rows.Item(introw).UserData = AllCrTransactions.IndexOf(objCrTxn)
            Select Case objCrTxn.CRTXNStatus

                Case CRTXN.enumCRTXNStatus.Active

                    .Item(introw, 0) = introw - .Rows.Fixed + 1
                    .Item(introw, 1) = objCrTxn.CRTXNAt.ToString
                    .Item(introw, 2) = objCrTxn.CRTXNType.ToString
                    .Item(introw, 3) = objCrTxn.CRTXNAmount
                    .Item(introw, 4) = objCrTxn.CRTXNBy()
                    .Item(introw, 5) = objCrTxn.CRTXNReceiptGiven
                    .Item(introw, 6) = objCrTxn.CRTXNComments

                Case CRTXN.enumCRTXNStatus.Voided

                    .Item(introw, 0) = introw - .Rows.Fixed + 1
                    .Item(introw, 1) = objCrTxn.CRTXNAt.ToString
                    .Item(introw, 2) = objCrTxn.CRTXNType.ToString
                    .Item(introw, 3) = objCrTxn.CRTXNAmount
                    .Item(introw, 4) = objCrTxn.CRTXNBy()
                    .Item(introw, 5) = True
                    .Item(introw, 6) = objCrTxn.CRTXNComments
                    fgCrTXNs.Rows.Item(introw).Style = fgCrTXNs.Styles.Item("Void")
            End Select
        End With

    End Sub


    Private ReadOnly Property SelectedCRTXn() As CRTXN
        Get
            Dim intCrTXnIndex As Integer

            If (fgCrTXNs.RowSel < fgCrTXNs.Rows.Fixed) Then
                Return Nothing
            Else
                intCrTXnIndex = CInt(fgCrTXNs.Rows(fgCrTXNs.RowSel).UserData)
                If (intCrTXnIndex < AllCrTransactions.Count) And _
                    (intCrTXnIndex >= 0) Then
                    Return AllCrTransactions.Item(intCrTXnIndex)

                Else
                    Return Nothing
                End If
            End If
        End Get
    End Property



    Private Function ProcessNewCrTxn() As CRTXN
        Dim objCrTxn As CRTXN
        Dim objUserData As Object

        objUserData = fgCrTXNs.Rows.Item(fgCrTXNs.RowSel).UserData
        If objUserData Is Nothing Then
            objCrTxn = AllCrTransactions.AddCRTXN
            objCrTxn.CRTXNStatus = CRTXN.enumCRTXNStatus.NewCrTXn
            objCrTxn.CRTXNAt = SaleDays.CreateSaleDay.Now
        Else
            objCrTxn = AllCrTransactions.Item(CInt(objUserData))
        End If

        Return objCrTxn
        'LoadfgCrTxn()
    End Function


    Private Sub fgCrTXNs_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgCrTXNs.AfterEdit
        Dim rowno As Integer
        Dim objCrTxn As CRTXN
        Dim paymentType As String
        Dim objEmployee As Employee

        objCrTxn = SelectedCRTXn
        Select Case e.Col
            Case 0, 1
                'nop : not editable
            Case 2
                If objCrTxn Is Nothing Then Exit Sub
                objCrTxn.CRTXNType = CType(fgCrTXNs(e.Row, e.Col), CRTXN.enumCRTXNType)
                LoadfgCRTXN()
            Case 3
                If objCrTxn Is Nothing Then Exit Sub
                objCrTxn.CRTXNAmount = CType(fgCrTXNs(e.Row, e.Col), Decimal)
                LoadfgCRTXN()
            Case 4
                If objCrTxn Is Nothing Then Exit Sub
                objCrTxn.CRTXNBy = CType(fgCrTXNs(e.Row, e.Col), Integer)

            Case 5
                If objCrTxn Is Nothing Then Exit Sub
                objCrTxn.CRTXNReceiptGiven = CBool(fgCrTXNs(e.Row, e.Col))
                LoadfgCRTXN()
            Case 6

                If objCrTxn Is Nothing Then Exit Sub
                objCrTxn.CRTXNComments = CStr(fgCrTXNs(e.Row, e.Col))
                LoadfgCRTXN()

        End Select
    End Sub

    Private Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Dim intSQLReturnValue As Integer
        intSQLReturnValue = AllCrTransactions.SetData()
        If intSQLReturnValue < 0 Then
            MsgBox("Error in Saving the Changes . Please Try again")
        Else
            Me.Hide()
        End If
    End Sub


    Private Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid.Click
        Dim objCrTxn As CRTXN
        Dim Col As C1.Win.C1FlexGrid.Column


        With fgCrTXNs

            If (.RowSel < .Rows.Fixed) OrElse (.RowSel >= .Rows.Count) Then
                MsgBox("First select an item to void and then click Void.", MsgBoxStyle.Critical)
                Exit Sub
            End If
            objCrTxn = SelectedCRTXn
            If objCrTxn.CRTXNAt < Today Then
                RaiseEvent StatusChanged(" You Can't Void Prior days Transactions")
                Exit Sub
            End If
            objCrTxn.CRTXNStatus = CRTXN.enumCRTXNStatus.Voided
            LoadfgCRTXN()
        End With

    End Sub


    Private Function AllowfgCrTXNEdit(ByVal objCrTXn As CRTXN) As Boolean
        'must obtain override at this stage 
        Return m_CRTXN.CRTXnEditAllowed(objCrTXn)
    End Function

    Private Sub fgCrTXNs_StartEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgCrTXNs.StartEdit
        Dim objUserData As Object
        Dim objCRTXn As CRTXN

        objUserData = fgCrTXNs.Rows.Item(fgCrTXNs.RowSel).UserData

        Select Case e.Col

            Case 2
                If objUserData Is Nothing Then
                    'm_CRTXN = ProcessNewCrTxn()
                    'nope
                Else
                    CurrentlyEditedCrTXn = SelectedCRTXn
                    'e.Cancel = True
                End If
                Select Case AllowfgCrTXNEdit(CurrentlyEditedCrTXn)
                    Case False
                        e.Cancel = True  'Terminate any edit action by user
                        Exit Sub
                    Case True
                        If CurrentlyEditedCrTXn.CRTXNStatus = CRTXN.enumCRTXNStatus.NewCrTXn Then
                            CurrentlyEditedCrTXn.CRTXNStatus = CRTXN.enumCRTXNStatus.Active
                            CurrentlyEditedCrTXn.CRTXNAt = SaleDays.CreateSaleDay.Now
                        End If
                        If fgCrTXNs.RowSel = fgCrTXNs.Rows.Count - 1 Then
                            AllCrTransactions.AddCRTXN()
                        End If
                        LoadfgCRTXN()

                End Select

            Case Else
                If objUserData Is Nothing Then
                    e.Cancel = True
                    Exit Sub
                Else
                    CurrentlyEditedCrTXn = SelectedCRTXn
                    Select Case AllowfgCrTXNEdit(CurrentlyEditedCrTXn)
                        Case False
                            e.Cancel = True
                        Case True
                            'nope
                    End Select
                End If

        End Select
    End Sub

    Private Sub m_CRTXN_OverrideNeeded(ByVal newOverride As BOPr.Override) Handles m_CRTXN.OverrideNeeded
        Dim newfrmPOSOverride As New frmPOSOverride()
        newfrmPOSOverride.newOverride = newOverride
        newfrmPOSOverride.ShowDialog()
    End Sub


    Private Sub fgCrTXNs_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgCrTXNs.EnterCell

        Dim intNextRow As Integer

        With fgCrTXNs
            Select Case .ColSel
                Case Is < 0
                    Exit Sub

                Case 0, 1

                    .ColSel = 2
                    .Select(.RowSel, .ColSel)
                    .ShowCell(.RowSel, .ColSel)

                Case 2, 3, 4, 5, 6
                    If SelectedCRTXn Is Nothing Then
                        Exit Sub

                    ElseIf SelectedCRTXn.CRTXNStatus = CRTXN.enumCRTXNStatus.NewCrTXn Then
                        .ColSel = 2
                        .Select(.RowSel, .ColSel)
                        .ShowCell(.RowSel, .ColSel)

                    End If




            End Select
        End With
    End Sub


    Private Sub fgCrTXNs_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles fgCrTXNs.KeyPressEdit
        Dim dblValue As Double
        Select Case e.Col
            Case 3
                Try
                    dblValue = CType(CStr(e.KeyChar & "1"), Double)
                    If dblValue >= 0 Then
                        e.Handled = False
                    Else
                        e.Handled = True    'Discard  key presses by user resulting in negative number  
                    End If
                Catch ex As Exception
                    e.Handled = True    'Discard non numeric key presses by user  
                Finally
                End Try
        End Select
    End Sub

    Private Sub fgCrTXNs_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles fgCrTXNs.KeyDown
        Dim intColSel As Integer
        Dim intRowSel As Integer
        Select Case e.KeyCode
            Case Keys.Right
                If fgCrTXNs.ColSel < fgCrTXNs.Cols.Count - 1 Then
                    intColSel = intColSel - 1
                Else
                    intColSel = fgCrTXNs.Cols.Count - 1
                End If
                Select Case intColSel
                    Case 2, 3, 4, 5
                        'nope
                    Case fgCrTXNs.Cols.Count - 1
                        e.Handled = True
                        With fgCrTXNs
                            .ColSel = 2
                            .RowSel = fgCrTXNs.RowSel + 1
                            .Select(.RowSel, .ColSel, True)
                        End With
                End Select
            Case Keys.Left
                If fgCrTXNs.ColSel > 0 Then
                    intColSel = fgCrTXNs.ColSel
                Else
                    intColSel = 0
                End If
                Select Case intColSel
                    Case 2
                        e.Handled = True
                        With fgCrTXNs
                            .ColSel = 6
                            If .RowSel > fgCrTXNs.Rows.Fixed Then
                                .RowSel = fgCrTXNs.RowSel - 1
                                .ColSel = 6
                            Else
                                .ColSel = 2
                            End If
                            .Select(.RowSel, .ColSel, True)
                        End With
                End Select
        End Select
    End Sub
End Class

