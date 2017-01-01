Option Strict On
Imports C1.Win
Imports BOPr
Imports System.Drawing.Printing

Public Class frmCashRegisterCount
    Inherits System.Windows.Forms.Form
    Private WithEvents rbtnLunchOpen As RadioButton
    Private WithEvents rbtnLunchClose As RadioButton
    Private WithEvents rbtnDinnerOpen As RadioButton
    Private WithEvents rbtnDinnerClose As RadioButton
    Private WithEvents btnShowCount As Button
    ' Private WithEvents btnEdit As Button

    Private m_SelectedSaleDay As SaleDay
    Private WithEvents m_SelectedSDSession As SaleDaySession
    Private m_SelectedCashCountType As MoneyCount.enumMoneyCountType
    Private m_SelectedMoneyCount As MoneyCount

    'Point to 1 cash count type prior i.e. if selected= Dinner Close then Dinner Open etc.
    Private m_PriorSaleDay As SaleDay
    'Private m_PriorSDSession As SaleDaySession
    'Private m_PriorCashCountType As MoneyCount.enumMoneyCountType
    Private m_PriorMoneyCount As MoneyCount
    Private WithEvents fgBalance As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents fgDailyReport As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents fgCashCount As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents fgPaymentMethod As C1.Win.C1FlexGrid.C1FlexGrid

    Private lblCashCount As Label
    Private WithEvents dtePicker As DateTimePicker
    Private WithEvents sessionTab As TabControl

    Private pnlStep1 As Panel
    Private pnlStep2 As Panel
    Private pnlStep3 As Panel

    Private lblPanel2 As Label
    Private lblPanel1 As Label

    Private WithEvents btnOk As System.Windows.Forms.Button
    Private WithEvents btnPrint As System.Windows.Forms.Button
    Private WithEvents btnPreview As System.Windows.Forms.Button
    Private WithEvents btnReset As Button
    Private WithEvents chkCopyOpenBalances As CheckBox
    Private WithEvents chkCCSettlement As CheckBox

    Private m_CurrentMoneyCount As MoneyCount
    ' Private m_CRBalance As CRBalance

    Private m_timeFrom As DateTime
    Private m_timeTo As DateTime

    Private m_EditAllowed As Boolean

    Private m_IsCCSettlementdone As Boolean
    'Balance Summary Grid Row Enums
    Private Enum enumfgBalanceSummary
        HdrRow
        Cash
        CC
        Other
        Total
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
        Me.Text = "frmCashRegisterCount"
    End Sub

#End Region


    Private Sub frmCashRegisterCount_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DisableFormBtns()
    End Sub

    Private Sub SetUIComponents()
        With Me
            .AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            .ClientSize = New System.Drawing.Size(1160, 520)
            .ControlBox = False
            .FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            .MaximizeBox = False
            .MinimizeBox = False
            .Name = "frmCashRegisterCount"
            .ShowInTaskbar = False
            .Text = "Cash Register Count"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))

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
            .Dock = System.Windows.Forms.DockStyle.Left
            .Width = 132
            .TabIndex = 0
            .BorderStyle = BorderStyle.Fixed3D
        End With

        SetPanel1LabelProperties()
        SetDatePickerProperties()
        SetSDSessionRadioBtns()
        SetFormBtns()

        With pnlStep1
            .Controls.AddRange(New Control() {dtePicker, rbtnLunchOpen, rbtnLunchClose, rbtnDinnerOpen, rbtnDinnerClose, chkCCSettlement, btnShowCount, lblPanel1})
            .ResumeLayout()
        End With

    End Sub

    Private Sub SetPanel1LabelProperties()
        lblPanel1 = New Label()

        With lblPanel1
            .Dock = DockStyle.Top
            .Height = 20
            '.Size = New System.Drawing.Size(180, 20)
            '.Location = New System.Drawing.Point(CInt(pnlStep1.Width / 2) - 90, 5)
            .BackColor = Color.SandyBrown
            .Text = "Count Cash For Date"
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
    End Sub


    Private Sub SetDatePickerProperties()
        dtePicker = New DateTimePicker()

        With dtePicker
            .Size = New System.Drawing.Size(120, 20)
            .Location = New System.Drawing.Point(CInt((pnlStep1.Width - dtePicker.Width) / 2), lblPanel1.Height + 30)
            .CalendarMonthBackground = Color.FromKnownColor(KnownColor.Control)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.SandyBrown
            .Format = DateTimePickerFormat.Short
            .Visible = True
            .MaxDate = Today
            .TabIndex = 0
        End With
    End Sub

    Private Sub SetSDSessionRadioBtns()
        rbtnLunchOpen = New RadioButton()
        With rbtnLunchOpen
            .Size = New Size(80, 20)
            .Location = New Point(dtePicker.Location.X, dtePicker.Location.Y + 40)
            .Appearance = Appearance.Normal
            .AutoCheck = True
            .Text = "Lunch Open"
            .Name = "LunchOpen"
            .TabIndex = 1
        End With

        rbtnLunchClose = New RadioButton()
        With rbtnLunchClose
            .Size = New Size(80, 20)
            .Location = New Point(rbtnLunchOpen.Location.X, rbtnLunchOpen.Location.Y + 30)
            .Appearance = Appearance.Normal
            .AutoCheck = True
            .Text = "Lunch Close"
            .Name = "LunchClose"
            .TabIndex = 2
        End With

        rbtnDinnerOpen = New RadioButton()
        With rbtnDinnerOpen
            .Size = New Size(80, 20)
            .Location = New Point(rbtnLunchClose.Location.X, rbtnLunchClose.Location.Y + 30)
            .Appearance = Appearance.Normal
            .AutoCheck = True
            .Text = "Dinner Open"
            .Name = "DinnerOpen"
            .TabIndex = 3
        End With

        rbtnDinnerClose = New RadioButton()
        With rbtnDinnerClose
            .Size = New Size(80, 20)
            .Location = New Point(rbtnDinnerOpen.Location.X, rbtnDinnerOpen.Location.Y + 30)
            .Appearance = Appearance.Normal
            .AutoCheck = True
            .Text = "Dinner Close"
            .Name = "DinnerClose"
            .TabIndex = 4
        End With
    End Sub

    Private Sub SetFormBtns()
        chkCCSettlement = New CheckBox()
        With chkCCSettlement
            .Size = New System.Drawing.Size(100, 40)
            .Location = New Point(CInt(pnlStep1.Width / 2 - chkCCSettlement.Width / 2), CInt(Me.ClientSize.Height / 2 - chkCCSettlement.Height / 2))
            .FlatStyle = FlatStyle.Flat
            .Text = "Was CC Settlement done yesterday night?"
        End With
        btnShowCount = New Button()
        With btnShowCount
            .Size = New System.Drawing.Size(60, 40)
            .Location = New Point(CInt(pnlStep1.Width / 4 - btnShowCount.Width / 2), chkCCSettlement.Location.Y + chkCCSettlement.Height + 30)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .TabIndex = 5
            .Text = "Show Count"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
        'btnEdit = New Button()
        'With btnEdit
        '    .Size = New System.Drawing.Size(60, 40)
        '    .Location = New Point(btnShowCount.Location.X + btnShowCount.Width + 5, btnShowCount.Location.Y)
        '    .FlatStyle = FlatStyle.Flat
        '    .BackColor = Color.DarkBlue
        '    .ForeColor = Color.White
        '    .TabIndex = 6
        '    .Text = "Enter Count"
        '    .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        'End With
    End Sub

    Private Sub SetStep2PanelProperties()
        pnlStep2 = New Panel()
        With pnlStep2
            .SuspendLayout()
            .BackColor = Color.Ivory
            .Dock = System.Windows.Forms.DockStyle.Fill
            .Size = New System.Drawing.Size(Me.ClientSize.Width, Me.ClientSize.Height - (30))
            .BorderStyle = BorderStyle.Fixed3D
        End With

        SetPanel2LabelProperties()
        SetBalanceGridProperties()
        SetPaymentMethodGrid()
        SetCashCountGrid()
        SetPanel2ButtonProperties()

        With pnlStep2
            .Controls.AddRange(New Control() {fgBalance, fgCashCount, fgPaymentMethod, chkCopyOpenBalances, btnPreview, btnPrint, btnOk, btnReset, lblPanel2})
            .ResumeLayout()
            .Visible = True
        End With
    End Sub

    Private Sub SetPanel2LabelProperties()
        lblPanel2 = New Label()

        With lblPanel2
            '.Size = New System.Drawing.Size(sessionTab.Width, 35)
            '.Location = New System.Drawing.Point(1, 1)
            .Dock = DockStyle.Top
            .Height = 60
            .BackColor = Color.SandyBrown
            .Text = "  Cash Register Count"
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
    End Sub

    Private Sub SetPanel2LabelText()
        Dim sessionname As String
        Dim cashcounttype As String
        Select Case SelectedCashCountType
            Case MoneyCount.enumMoneyCountType.Closing
                cashcounttype = "Closing"
            Case MoneyCount.enumMoneyCountType.Opening
                cashcounttype = "Opening"
        End Select
        sessionname = SelectedSDSession.SessionName

        If lblPanel2 Is Nothing Then
            'nope
        Else
            lblPanel2.Text = "  Cash Register Count For " + dtePicker.Value.ToShortDateString + " At " + sessionname + " " + cashcounttype + " Time"
        End If
    End Sub

    'Private Sub LoadCRBalance()
    '    Dim sessionname As String
    '    Dim strtabText As String
    '    Dim objSessions As Sessions
    '    Dim objSession As Session
    '    Dim SessionId As Integer
    '    Dim objCrBalance As CRBalance
    '    Dim objCrBalances As CRBalances
    '    Dim objMoneyCountType As MoneyCount.enumMoneyCountType
    '    Dim objSaleDay As SaleDay
    '    Dim objSDSession As SaleDaySession
    '    Dim intsessionIndex As Integer
    '    Dim sessionnames() As String
    '    Dim enSession As Session.enumSessionName
    '    Dim i As Integer

    '    If sessionTab.SelectedIndex >= 0 Then
    '        'nope
    '    Else
    '        sessionTab.SelectedIndex = 0
    '    End If
    '    strtabText = sessionTab.TabPages(sessionTab.SelectedIndex).Text
    '    Select Case strtabText
    '        Case "Lunch Open"
    '            objMoneyCountType = MoneyCount.enumMoneyCountType.Opening
    '            sessionname = "Lunch"
    '            enSession = Session.enumSessionName.Lunch
    '        Case "Lunch Close"
    '            objMoneyCountType = MoneyCount.enumMoneyCountType.Closing
    '            sessionname = "Lunch"
    '            enSession = Session.enumSessionName.Lunch
    '        Case "Dinner Open"
    '            objMoneyCountType = MoneyCount.enumMoneyCountType.Opening
    '            sessionname = "Dinner"
    '            enSession = Session.enumSessionName.Dinner
    '        Case "Dinner Close"
    '            objMoneyCountType = MoneyCount.enumMoneyCountType.Closing
    '            sessionname = "Dinner"
    '            enSession = Session.enumSessionName.Dinner
    '    End Select

    '    objSaleDay = SaleDays.CreateSaleDay.Item(dtePicker.Value.Date, True)
    '    If objSaleDay Is Nothing Then
    '        'nope
    '    Else
    '        SelectedSDSession = objSaleDay.AllSaleDaySessions.Item(enSession)
    '        CurrentMoneyCount = SelectedSDSession.GetBalance(objMoneyCountType)
    '        'LoadMoneyCountDetails(objMoneyCountType)
    '        LoadfgBalanceSummary()
    '    End If
    'End Sub

    'Private Sub LoadfgBalanceSummary()
    '    fgBalance.SuspendLayout()
    '    With fgBalance
    '        .Item(1, 2) = CurrentMoneyCount.TotalCashAmount
    '        .Item(2, 2) = CurrentMoneyCount.TotalCreditCardAmount()
    '        .Item(3, 2) = CurrentMoneyCount.TotalCheckAmount()
    '        .Item(4, 2) = CurrentMoneyCount.TotalAmount()
    '    End With
    '    fgBalance.ResumeLayout()
    'End Sub

    Private Sub SetBalanceGridProperties()
        fgBalance = New C1.Win.C1FlexGrid.C1FlexGrid()
        SetfgBalanceStyles()
        With fgBalance
            '.Size = New System.Drawing.Size(952, 392)
            .Size = New System.Drawing.Size(294, 118)
            .Location = New System.Drawing.Point(pnlStep1.Width + 4, lblPanel2.Location.Y + lblPanel2.Height + 30)
            .ScrollBars = ScrollBars.None
            .Rows.Count = 5
            .Rows.Fixed = 1
            .Cols.Count = 3
            .Cols.Fixed = 1
            '.Cols.Fixed = 0

            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
            .KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.None
            .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Raised
            '.Styles.Focus.BackColor = Color.SteelBlue
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            '.Styles.Highlight.BackColor = Color.SteelBlue

            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = False
            .AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            .AllowMerging = C1FlexGrid.AllowMergingEnum.None
            .Styles.Alternate.BackColor = Color.PaleGoldenrod
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .Visible = True
            .Enabled = False
            .TabIndex = 0
        End With

        With fgBalance.Cols(0)
            .Width = 110
            .WidthDisplay = 110
            .Caption = "Summary"
            .Name = "BalanceType"
            .DataType = GetType(String)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .AllowResizing = False
        End With

        With fgBalance.Cols(1)
            .Width = 130
            .WidthDisplay = 130
            .AllowResizing = False
            .Caption = " As Per POS"
            .Name = "POSAmount"
            .DataType = GetType(Double)
            .Format = "###0.00"
        End With
        With fgBalance.Cols(2)
            .Width = 130
            .WidthDisplay = 130
            .Caption = "        Actual"
            .Name = "ActualAmount"
            .DataType = GetType(Double)
            .Format = "###0.00"
            .AllowResizing = False
        End With

        LoadfgBalanceSummaryFixedData()
    End Sub

    Private Sub SetfgBalanceStyles()
        Dim cs As C1.Win.C1FlexGrid.CellStyle

        cs = fgBalance.Styles.Add("DEError")
        With cs
            .Font = New Font(fgBalance.Font, FontStyle.Bold)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Red
        End With

    End Sub

    Private Sub LoadfgBalanceSummaryFixedData()
        fgBalance.Item(0, 0) = "Summary"
        fgBalance.Item(0, 1) = "As Per POS"
        fgBalance.Item(0, 2) = "      Actual"
        fgBalance.Item(1, 0) = "Cash"
        fgBalance.Item(2, 0) = "Credit Card"
        fgBalance.Item(3, 0) = "Other"
        fgBalance.Item(4, 0) = "   Total"
    End Sub

    Private Sub SetCashCountGrid()
        fgCashCount = New C1.Win.C1FlexGrid.C1FlexGrid()
        With fgCashCount
            '.Size = New System.Drawing.Size(952, 392)
            .Size = New System.Drawing.Size(456, 296)
            .AutoResize = False
            .Location = New System.Drawing.Point(fgBalance.Location.X + fgBalance.Width + 15, fgBalance.Location.Y)
            .ScrollBars = ScrollBars.None
            .Cols.Count = 6
            .Cols.Fixed = 0
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
            .Rows.Item(0).AllowMerging = True
            .Cols.Item(0).AllowMerging = True
            .Rows.Count = 13
            .Rows.Fixed = 2
            .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Raised
            '.Styles.Fixed.BackColor = Color.Moccasin
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .Styles.EmptyArea.BackColor = Color.Ivory

            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            .Styles.Alternate.BackColor = Color.PaleGoldenrod
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveDown
            .KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.None
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 4
            .Rows(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            LoadCashCountGridFixedData()
            '.Item(0, 1) = "Loose"
            '.Item(0, 2) = "Loose"
            '.Item(0, 3) = "Loose"
            '.Item(0, 4) = "Rolls"
            '.Item(0, 5) = "Rolls"
            '.Item(1, 2) = "Count"
            '.Item(1, 3) = "$"
            '.Item(1, 4) = "Count"
            '.Item(1, 5) = "$"

            '.Item(2, 0) = "Coins"
            '.Item(3, 0) = "Coins"
            '.Item(4, 0) = "Coins"
            '.Item(5, 0) = "Coins"
            '.Item(6, 0) = "Coins"

            '.Item(7, 0) = "Bills"
            '.Item(8, 0) = "Bills"
            '.Item(9, 0) = "Bills"
            '.Item(10, 0) = "Bills"
            '.Item(11, 0) = "Bills"
            '.Item(12, 0) = "Bills"

            '.Item(2, 1) = "1C"
            '.Item(3, 1) = "5C"
            '.Item(4, 1) = "10C"
            '.Item(5, 1) = "25C"
            '.Item(6, 1) = "1$ Coin"

            '.Item(7, 1) = "$1"
            '.Item(8, 1) = "$5"
            '.Item(9, 1) = "$10"
            '.Item(10, 1) = "$20"
            '.Item(11, 1) = "$50"
            '.Item(12, 1) = "$100"

        End With

        With fgCashCount.Cols(0)
            .Width = 90
            .WidthDisplay = 90
            .Name = ""
            .DataType = GetType(String)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .AllowEditing = False
        End With

        With fgCashCount.Cols(1)
            .Width = 90
            .WidthDisplay = 90
            .Name = ""
            .DataType = GetType(String)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .AllowEditing = False
        End With

        With fgCashCount.Cols(2)
            .Width = 140
            .WidthDisplay = 80
            .Name = "looseCount"
            .DataType = GetType(Integer)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgCashCount.Cols(3)
            .Width = 200
            .WidthDisplay = 120
            .Name = "loose$"
            .DataType = GetType(Double)
            .Format = "c"
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowEditing = False
        End With


        With fgCashCount.Cols(4)
            .Width = 140
            .WidthDisplay = 80
            .Name = "Count"
            .DataType = GetType(Integer)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgCashCount.Cols(5)
            .Width = 200
            .WidthDisplay = 120
            .Name = "$"
            .DataType = GetType(Double)
            .Format = "c"
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowDragging = False
        End With

        fgCashCount.Visible = False
        'SetCashCountLabelProperties()
        'SetDoneButtonProperties()
    End Sub

    Private Sub LoadCashCountGridFixedData()
        With fgCashCount
            .Item(0, 1) = "Loose"
            .Item(0, 2) = "Loose"
            .Item(0, 3) = "Loose"
            .Item(0, 4) = "Rolls"
            .Item(0, 5) = "Rolls"
            .Item(1, 2) = "Count"
            .Item(1, 3) = "$"
            .Item(1, 4) = "Count"
            .Item(1, 5) = "$"

            .Item(2, 0) = "Coins"
            .Item(3, 0) = "Coins"
            .Item(4, 0) = "Coins"
            .Item(5, 0) = "Coins"
            .Item(6, 0) = "Coins"

            .Item(7, 0) = "Bills"
            .Item(8, 0) = "Bills"
            .Item(9, 0) = "Bills"
            .Item(10, 0) = "Bills"
            .Item(11, 0) = "Bills"
            .Item(12, 0) = "Bills"

            .Item(2, 1) = "1C"
            .Item(3, 1) = "5C"
            .Item(4, 1) = "10C"
            .Item(5, 1) = "25C"
            .Item(6, 1) = "1$ Coin"

            .Item(7, 1) = "$1"
            .Item(8, 1) = "$5"
            .Item(9, 1) = "$10"
            .Item(10, 1) = "$20"
            .Item(11, 1) = "$50"
            .Item(12, 1) = "$100"
        End With
    End Sub

    Private Sub LoadCashDetails()
        fgCashCount.SuspendLayout()
        With fgCashCount
            .Item(2, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.OneCentLoose)
            .Item(2, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.OneCentLoose)
            .Item(3, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.FiveCentLoose)
            .Item(3, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.FiveCentLoose)
            .Item(4, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.TenCentLoose)
            .Item(4, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.TenCentLoose)
            .Item(5, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentLoose)
            .Item(5, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.TwentyFiveCentLoose)
            .Item(6, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarOneCoin)
            .Item(6, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarOneCoin)
            .Item(7, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarOneBill)
            .Item(7, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarOneBill)
            .Item(8, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarFiveBill)
            .Item(8, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarFiveBill)
            .Item(9, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarTenBill)
            .Item(9, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarTenBill)
            .Item(10, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarTwentyBill)
            .Item(10, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarTwentyBill)
            .Item(11, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarFiftyBill)
            .Item(11, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarFiftyBill)
            .Item(12, 2) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.DollarHundredBill)
            .Item(12, 3) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.DollarHundredBill)
            .Item(2, 4) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.OneCentRoll)
            .Item(2, 5) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.OneCentRoll)
            .Item(3, 4) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.FiveCentRoll)
            .Item(3, 5) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.FiveCentRoll)
            .Item(4, 4) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.TenCentRoll)
            .Item(4, 5) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.TenCentRoll)
            .Item(5, 4) = SelectedMoneycount.DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentRoll)
            .Item(5, 5) = SelectedMoneycount.DenominationAmount(MoneyCount.enumCashDenomination.TwentyFiveCentRoll)

        End With
        fgCashCount.ResumeLayout()
    End Sub

    Private Sub SetCashCountLabelProperties()
        lblCashCount = New Label()
        With lblCashCount
            .Size = New System.Drawing.Size(440, 15)
            .Location = New System.Drawing.Point(1, 1)
            .BackColor = Color.SandyBrown
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.MiddleCenter

        End With
        'Me.Controls.Add(lblCashCount)

    End Sub

    Private Sub SetBtnProperties(ByVal btn As Button)
        With btn
            .Size = New System.Drawing.Size(60, 40)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, _
                        System.Drawing.FontStyle.Bold, _
                         System.Drawing.GraphicsUnit.Point, _
                         CType(0, Byte))
            .Enabled = True
            .Visible = True
        End With
    End Sub

    Private Sub SetPanel2ButtonProperties()
        chkCopyOpenBalances = New CheckBox()
        With chkCopyOpenBalances
            .Size = New System.Drawing.Size(fgBalance.Cols(2).Width, 40)
            .Location = New Point(fgBalance.Location.X + fgBalance.Width - chkCopyOpenBalances.Width + 20, fgBalance.Location.Y + fgBalance.Height + 3)
            .FlatStyle = FlatStyle.Flat
            .Text = "Copy previous Closing Balance?"
        End With

        btnPreview = New Button()
        With btnPreview
            .Location = New System.Drawing.Point(fgBalance.Location.X, fgBalance.Location.Y + fgBalance.Height + chkCopyOpenBalances.Height + 40)
            SetBtnProperties(btnPreview)
            .Text = "Preview"
        End With

        btnPrint = New Button()
        With btnPrint
            .Location = New System.Drawing.Point(btnPreview.Location.X + btnPreview.Width + 20, btnPreview.Location.Y)
            SetBtnProperties(btnPrint)
            .Text = "Print"
        End With

        btnOk = New Button()
        With btnOk
            .Location = New System.Drawing.Point(btnPrint.Location.X + btnPrint.Width + 20, btnPreview.Location.Y)
            SetBtnProperties(btnOk)
            .Text = "Save + Close"
            .TabIndex = 7
        End With

        btnReset = New Button()
        With btnReset
            .Location = New System.Drawing.Point(btnOk.Location.X + btnOk.Width + 20, btnPreview.Location.Y)
            SetBtnProperties(btnReset)
            .Text = "Reset"
            .Enabled = True
            .Visible = True
            .TabIndex = 7
        End With
    End Sub

    Private Sub SetPaymentMethodGrid()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        fgPaymentMethod = New C1.Win.C1FlexGrid.C1FlexGrid()
        SetfgPaymentStyles()
        With fgPaymentMethod
            '.Size = New System.Drawing.Size(952, 392)
            .Size = New System.Drawing.Size(700, 100)
            .Location = New System.Drawing.Point(fgBalance.Location.X + fgBalance.Width + 15, fgBalance.Location.Y)
            .ScrollBars = ScrollBars.None
            .Cols.Count = 6
            .Cols.Fixed = 0
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
            .Rows.Item(0).AllowMerging = True
            .Rows.Item(1).AllowMerging = True
            '.Cols.Item(1).AllowMerging = True
            ' .Cols.Item(2).AllowMerging = True
            '.Cols.Item(3).AllowMerging = True
            '.Cols.Item(4).AllowMerging = True
            .Rows.Count = 6
            .Rows.Fixed = 2
            .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.Raised
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .Styles.Highlight.BackColor = Color.SteelBlue
            .Styles.EmptyArea.BackColor = Color.PaleGoldenrod
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.None
            .Styles.Alternate.BackColor = Color.PaleGoldenrod
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 5
            .Rows(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            .Visible = False
            .ScrollBars = ScrollBars.Horizontal
        End With

        With fgPaymentMethod.Cols(0)
            .Width = 170
            .WidthDisplay = 170
            .Name = "CreditCards"
            fgPaymentMethod.Item(0, 0) = "Credit Cards"
            fgPaymentMethod.Item(1, 0) = "Credit Cards"
            .DataType = GetType(String)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .AllowEditing = False
        End With

        With fgPaymentMethod.Cols(1)
            .Width = 70
            .WidthDisplay = 70
            .Name = "POSCount"
            fgPaymentMethod.Item(0, 1) = "As Per POS"
            fgPaymentMethod.Item(1, 1) = "#"
            .DataType = GetType(Integer)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowEditing = False
        End With

        With fgPaymentMethod.Cols(2)
            .Width = 105
            .WidthDisplay = 105
            .Name = "POSAmount"
            fgPaymentMethod.Item(0, 2) = "As Per POS"
            fgPaymentMethod.Item(1, 2) = "$"
            .DataType = GetType(Double)
            .Format = "c"
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
            .AllowEditing = False
        End With

        With fgPaymentMethod.Cols(3)
            .Width = 70
            .WidthDisplay = 70
            .Name = "ActualCount"
            fgPaymentMethod.Item(0, 3) = "Enter Actual"
            fgPaymentMethod.Item(1, 3) = "#"
            .DataType = GetType(Integer)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgPaymentMethod.Cols(4)
            .Width = 105
            .WidthDisplay = 105
            .Name = "ActualAmount"
            fgPaymentMethod.Item(0, 1) = "Enter Actual"
            fgPaymentMethod.Item(1, 1) = "$"
            .DataType = GetType(Double)
            .Format = "c"
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgPaymentMethod.Cols(5)
            .Width = 120
            .WidthDisplay = 120
            .Name = "DEError"
            .DataType = GetType(String)
            .TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            .Visible = True
            .AllowEditing = False

        End With


        cs = fgPaymentMethod.Styles.Add("DEError")
        With cs
            .Font = New Font(fgPaymentMethod.Font, FontStyle.Regular)
            .ForeColor = Color.Red
        End With
    End Sub

    Private Sub SetfgPaymentStyles()
        Dim cs As C1.Win.C1FlexGrid.CellStyle


        cs = fgPaymentMethod.Styles.Add("ValueError")
        With cs
            .BackColor = Color.Red
        End With

        cs = fgPaymentMethod.Styles.Add("DEError")
        With cs
            .Font = New Font(fgPaymentMethod.Font, FontStyle.Regular)
            .ForeColor = Color.Red
        End With
    End Sub



    Private Sub LoadPaymentData()

        With fgPaymentMethod
            .Rows.Count = 7
            .Size = New System.Drawing.Size(645, 188)
            '.Location = New System.Drawing.Point(501, 180)
            .Item(2, 0) = "Visa"
            .Item(3, 0) = "Master Card"
            .Item(4, 0) = "Discover"
            .Item(5, 0) = "Diners Club"
            .Item(6, 0) = "Amex"
            .Item(0, 1) = "As Per POS"
            .Item(0, 2) = "As Per POS"
            .Item(0, 3) = "      Actual"
            .Item(0, 4) = "      Actual"
            .Item(1, 1) = "       #"
            .Item(1, 2) = "  Amount"
            .Item(1, 3) = "       #"
            .Item(1, 4) = "  Amount"
        End With

    End Sub

    Private Sub LoadCreditCardDetails()
        fgPaymentMethod.SuspendLayout()
        With fgPaymentMethod
            Select Case m_IsCCSettlementdone
                Case True
                    .Item(2, 1) = 0
                    .Item(2, 2) = 0
                    .Item(3, 1) = 0
                    .Item(3, 2) = 0
                    .Item(4, 1) = 0
                    .Item(4, 2) = 0
                    .Item(5, 1) = 0
                    .Item(5, 2) = 0
                    .Item(6, 1) = 0
                    .Item(6, 2) = 0
                Case False
                    .Item(2, 1) = PriorMoneycount.VisaCount + SelectedSaleDay.VisaCountForDay _
                               + SelectedSaleDay.NonCurrentVisaCount
                    .Item(2, 2) = PriorMoneycount.VisaAmount + SelectedSaleDay.VisaTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentVisaTotalAmount

                    .Item(3, 1) = PriorMoneycount.MasterCardCount + SelectedSaleDay.MasterCardCountForDay _
                               + SelectedSaleDay.NonCurrentMasterCardCount
                    .Item(3, 2) = PriorMoneycount.MasterCardAmount + SelectedSaleDay.MasterCardTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentMasterCardTotalAmount

                    .Item(4, 1) = PriorMoneycount.DiscoverCount + SelectedSaleDay.DiscoverCardCountForDay _
                               + SelectedSaleDay.NonCurrentDiscoverCount
                    .Item(4, 2) = PriorMoneycount.DiscoverAmount + SelectedSaleDay.DiscoverCardTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentDiscoverTotalAmount

                    .Item(5, 1) = PriorMoneycount.DinersClubCount + SelectedSaleDay.DinersClubCountForDay _
                                                  + SelectedSaleDay.NonCurrentDinersClubCount
                    .Item(5, 2) = PriorMoneycount.DinersClubAmount + SelectedSaleDay.DinersClubTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentDinersClubTotalAmount

                    .Item(6, 1) = PriorMoneycount.AmexCount + SelectedSaleDay.AmexCountForDay _
                                                   + SelectedSaleDay.NonCurrentAmexCount
                    .Item(6, 2) = PriorMoneycount.AmexAmount + SelectedSaleDay.AmexTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentAmexTotalAmount
            End Select
            .Item(2, 3) = SelectedMoneycount.VisaCount
            .Item(2, 4) = SelectedMoneycount.VisaAmount

            .Item(3, 3) = SelectedMoneycount.MasterCardCount
            .Item(3, 4) = SelectedMoneycount.MasterCardAmount

            .Item(4, 3) = SelectedMoneycount.DiscoverCount
            .Item(4, 4) = SelectedMoneycount.DiscoverAmount

            .Item(5, 3) = SelectedMoneycount.DinersClubCount
            .Item(5, 4) = SelectedMoneycount.DinersClubAmount

            .Item(6, 3) = SelectedMoneycount.AmexCount
            .Item(6, 4) = SelectedMoneycount.AmexAmount
        End With
        fgPaymentMethod.ResumeLayout()
        SetPaymentStyle()
        ShowErrorInBalance()
        ChangebtnEnable()
    End Sub

    Private Sub SetPaymentStyle()
        Dim row As C1.Win.C1FlexGrid.Row
        Dim intRowNo As Integer
        For Each row In fgPaymentMethod.Rows
            If intRowNo > 1 Then
                ValidatePaymentCount(intRowNo, 3)
                ValidatePaymentAmount(intRowNo, 4)

            End If
            intRowNo += 1
        Next

    End Sub



    Private Sub LoadOtherPaymentData()

        With fgPaymentMethod
            .Rows.Count = 6
            .Item(2, 0) = "Personal Check"
            .Item(3, 0) = "Gift Certificate"
            .Item(4, 0) = "Travellers Check"
            .Item(5, 0) = "House Account"
            .Size = New System.Drawing.Size(645, 160)
            '.Location = New System.Drawing.Point(501, 180)
            .Item(0, 1) = "As Per POS"
            .Item(0, 2) = "As Per POS"
            .Item(0, 3) = "   Actual"
            .Item(0, 4) = "   Actual"
            .Item(1, 1) = "     #"
            .Item(1, 2) = "   Amount"
            .Item(1, 3) = "       #"
            .Item(1, 4) = "   Amount"

        End With

    End Sub

    Private Sub LoadOtherPaymentDetails()
        fgPaymentMethod.SuspendLayout()
        With fgPaymentMethod
            Select Case m_IsCCSettlementdone
                Case True
                    .Item(2, 1) = 0
                    .Item(2, 2) = 0
                    .Item(3, 1) = 0
                    .Item(3, 2) = 0
                    .Item(4, 1) = 0
                    .Item(4, 2) = 0
                    .Item(5, 1) = 0
                    .Item(5, 2) = 0
                Case False
                    .Item(2, 1) = PriorMoneycount.PersonalCheckCount + SelectedSaleDay.PersonalCheckCountForDay _
                                + SelectedSaleDay.NonCurrentPersonalCheckCount
                    .Item(2, 2) = PriorMoneycount.PersonalCheckAmount + SelectedSaleDay.PersonalCheckTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentPersonalCheckTotalAmount

                    .Item(3, 1) = PriorMoneycount.GiftCertificateCount + SelectedSaleDay.MGGiftCertificateCountForDay _
                               + SelectedSaleDay.NonCurrentMGGiftCertificateCount
                    .Item(3, 2) = PriorMoneycount.GiftCertificateAmount + SelectedSaleDay.MGGiftCertificateTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentMGGiftCertificateTotalAmount

                    .Item(4, 1) = PriorMoneycount.TravellersCheckCount + SelectedSaleDay.TravellersChecksCountForDay _
                                + SelectedSaleDay.NonCurrentTravellersChecksCount
                    .Item(4, 2) = PriorMoneycount.TravellersCheckAmount + SelectedSaleDay.TravellersChecksTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentTravellersChecksTotalAmount

                    .Item(5, 1) = PriorMoneycount.HouseAccountCount + SelectedSaleDay.HouseAccountCountForDay _
                                                   + SelectedSaleDay.NonCurrentHouseAccountCount
                    .Item(5, 2) = PriorMoneycount.HouseAccountAmount + SelectedSaleDay.HouseAccountTotalAmountForDay _
                               + SelectedSaleDay.NonCurrentHouseAccountTotalAmount
            End Select

            .Item(2, 3) = SelectedMoneycount.PersonalCheckCount
            .Item(2, 4) = SelectedMoneycount.PersonalCheckAmount


            .Item(3, 3) = SelectedMoneycount.GiftCertificateCount
            .Item(3, 4) = SelectedMoneycount.GiftCertificateAmount


            .Item(4, 3) = SelectedMoneycount.TravellersCheckCount
            .Item(4, 4) = SelectedMoneycount.TravellersCheckAmount


            .Item(5, 3) = SelectedMoneycount.HouseAccountCount
            .Item(5, 4) = SelectedMoneycount.HouseAccountAmount
        End With
        fgPaymentMethod.ResumeLayout()
        SetPaymentStyle()
        ShowErrorInBalance()
        ChangebtnEnable()
    End Sub



    Private Sub CalculateLooseAmount(ByVal intRow As Integer)

        With SelectedMoneycount

            Select Case intRow
                Case 2
                    .DenominationCount(MoneyCount.enumCashDenomination.OneCentLoose) = _
                     CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                     .DenominationAmount(MoneyCount.enumCashDenomination. _
                       OneCentLoose)

                Case 3
                    .DenominationCount(MoneyCount.enumCashDenomination.FiveCentLoose) = _
                     CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                      .DenominationAmount(MoneyCount.enumCashDenomination.FiveCentLoose)
                Case 4
                    .DenominationCount(MoneyCount.enumCashDenomination.TenCentLoose) = _
                    CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                   .DenominationAmount(MoneyCount.enumCashDenomination.TenCentLoose)
                Case 5
                    .DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentLoose) = _
                    CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                    .DenominationAmount(MoneyCount.enumCashDenomination.TwentyFiveCentLoose)
                Case 6
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarOneCoin) = _
                    CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                     .DenominationAmount(MoneyCount.enumCashDenomination.DollarOneCoin)
                Case 7
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarOneBill) = _
                    CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                    .DenominationAmount(MoneyCount.enumCashDenomination.DollarOneBill)

                Case 8
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarFiveBill) = _
                      CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                   .DenominationAmount(MoneyCount.enumCashDenomination.DollarFiveBill)
                Case 9
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarTenBill) = _
                     CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                    .DenominationAmount(MoneyCount.enumCashDenomination.DollarTenBill)
                Case 10
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarTwentyBill) = _
                    CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                     .DenominationAmount(MoneyCount.enumCashDenomination.DollarTwentyBill)
                Case 11
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarFiftyBill) = _
                    CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                   .DenominationAmount(MoneyCount.enumCashDenomination.DollarFiftyBill)
                Case 12
                    .DenominationCount(MoneyCount.enumCashDenomination.DollarHundredBill) = _
                      CType(fgCashCount.Item(intRow, 2), Integer)

                    fgCashCount.Item(intRow, 3) = _
                     .DenominationAmount(MoneyCount.enumCashDenomination.DollarHundredBill)

            End Select
        End With
    End Sub

    Private Sub CalculateRollAmount(ByVal intRow As Integer)

        With SelectedMoneycount
            Select Case intRow
                Case 2
                    .DenominationCount(MoneyCount.enumCashDenomination.OneCentRoll) = _
                     CType(fgCashCount.Item(intRow, 4), Integer)

                    fgCashCount.Item(intRow, 5) = _
                    .DenominationAmount(MoneyCount.enumCashDenomination.OneCentRoll)

                Case 3
                    .DenominationCount(MoneyCount.enumCashDenomination.FiveCentRoll) = _
                    CType(fgCashCount.Item(intRow, 4), Integer)

                    fgCashCount.Item(intRow, 5) = _
                     .DenominationAmount _
                    (MoneyCount.enumCashDenomination.FiveCentRoll)
                Case 4
                    .DenominationCount(MoneyCount.enumCashDenomination.TenCentRoll) = _
                    CType(fgCashCount.Item(intRow, 4), Integer)

                    fgCashCount.Item(intRow, 5) = _
                    .DenominationAmount(MoneyCount.enumCashDenomination.TenCentRoll)
                Case 5
                    .DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentRoll) = _
                     CType(fgCashCount.Item(intRow, 4), Integer)

                    fgCashCount.Item(intRow, 5) = _
                   .DenominationAmount(MoneyCount.enumCashDenomination.TwentyFiveCentRoll)

            End Select
        End With

    End Sub


    Private Sub fgCashCount_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) _
    Handles fgCashCount.AfterEdit
        Select Case e.Col
            Case 3
                e.Cancel = True
            Case 2
                CalculateLooseAmount(e.Row)
                ' updateCrBalance()
                LoadfgBalanceSummary()
            Case 4

                CalculateRollAmount(e.Row)
                ' updateCrBalance()
                LoadfgBalanceSummary()
            Case 5
                e.Cancel = True
        End Select
    End Sub

    Private Sub fgCashCount_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgCashCount.BeforeEdit



        Select Case e.Col

            Case 2
                'nope
            Case 3
                e.Cancel = True

            Case 4
                If e.Row > 5 Then
                    e.Cancel = True
                Else
                    e.Cancel = False
                End If
                'nope
            Case 5
                e.Cancel = True
        End Select

    End Sub



    Private Sub fgBalance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgBalance.Click
        Dim intRow As Integer
        Dim intCol As Integer
        Select Case CType(fgBalance.RowSel, enumfgBalanceSummary)
            Case enumfgBalanceSummary.Cash
                With fgCashCount
                    .Enabled = True
                    .Visible = True
                    .Focus()
                End With
                LoadCashDetails()
                fgPaymentMethod.Visible = False
            Case enumfgBalanceSummary.CC
                fgCashCount.Visible = False
                fgCashCount.Enabled = False
                With fgPaymentMethod
                    .Clear(C1.Win.C1FlexGrid.ClearFlags.Content)
                    .Visible = True
                    .Enabled = True
                    .Row = 1
                    .Col = 1
                    .Focus()

                End With
                LoadPaymentData()
                LoadCreditCardDetails()
                'ShowErrorInBalance()
            Case enumfgBalanceSummary.Other

                fgCashCount.Enabled = False
                fgCashCount.Visible = False
                With fgPaymentMethod
                    .Clear(C1.Win.C1FlexGrid.ClearFlags.Content)
                    .Enabled = True
                    .Visible = True
                    .Row = 1
                    .Col = 1
                    .Focus()
                End With
                LoadOtherPaymentData()
                LoadOtherPaymentDetails()
                'ShowErrorInBalance()


            Case enumfgBalanceSummary.HdrRow, enumfgBalanceSummary.Total
                fgPaymentMethod.Visible = False
                fgCashCount.Visible = False
        End Select
        SetData(False)
    End Sub


    Private Sub SetData(ByVal blnResetOverrides As Boolean)
        Dim intSQLReturnValue As Integer

        If SelectedSDSession Is Nothing Then
            'nope
        Else

            Select Case SelectedCashCountType
                Case MoneyCount.enumMoneyCountType.Opening
                    SelectedSDSession.SetSaleDaySessionFrom()
                Case MoneyCount.enumMoneyCountType.Closing
                    SelectedSDSession.SetSaleDaySessionTo()
            End Select
            intSQLReturnValue = SelectedSDSession.SetData(blnResetOverrides)
            If intSQLReturnValue < 0 Then
                MsgBox("SQL Server Error - while saving the cash count")
            Else
                'nope
            End If
        End If
    End Sub

    Private Sub UpdateCreditCardAmount(ByVal intRow As Integer, ByVal cardtype As String)
        If cardtype = "Visa" Then
            SelectedMoneycount.VisaAmount = CType(fgPaymentMethod.Item(intRow, 4), Double)

        ElseIf cardtype = "Master Card" Then
            SelectedMoneycount.MasterCardAmount = CType(fgPaymentMethod.Item(intRow, 4), Double)

        ElseIf cardtype = "Amex" Then
            SelectedMoneycount.AmexAmount = CType(fgPaymentMethod.Item(intRow, 4), Double)

        ElseIf cardtype = "Discover" Then
            SelectedMoneycount.DiscoverAmount = CType(fgPaymentMethod.Item(intRow, 4), Double)

        ElseIf cardtype = "Diners Club" Then
            SelectedMoneycount.DinersClubAmount = CType(fgPaymentMethod.Item(intRow, 4), Double)

        ElseIf cardtype = "Knight Express" Then
            'SelectedMoneycount.KnightExpress = fgPaymentMethod.Item(intRow, 1)
            'SelectedMoneycount.KnightExpress = fgPaymentMethod.Item(intRow, 2)
        ElseIf cardtype = "Personal Check" Then
            SelectedMoneycount.PersonalCheckAmount = CType(fgPaymentMethod.Item(intRow, 4), Decimal)

        ElseIf cardtype = "Gift Certificate" Then
            SelectedMoneycount.GiftCertificateAmount = CType(fgPaymentMethod.Item(intRow, 4), Decimal)

        ElseIf cardtype = "Travellers Check" Then
            SelectedMoneycount.TravellersCheckAmount = CType(fgPaymentMethod.Item(intRow, 4), Decimal)
        ElseIf cardtype = "House Account" Then
            SelectedMoneycount.HouseAccountAmount = CType(fgPaymentMethod.Item(intRow, 4), Decimal)
        End If
        LoadfgBalanceSummary()
    End Sub






    Private Sub UpdateCreditCardCount(ByVal intRow As Integer, ByVal cardtype As String)
        Try
            If cardtype = "Visa" Then
                SelectedMoneycount.VisaCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Master Card" Then
                SelectedMoneycount.MasterCardCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Amex" Then
                SelectedMoneycount.AmexCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Discover" Then
                SelectedMoneycount.DiscoverCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Diners Club" Then
                SelectedMoneycount.DinersClubCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Knight Express" Then
                'SelectedMoneycount.KnightExpress = fgPaymentMethod.Item(intRow, 1)
                'SelectedMoneycount.KnightExpress = fgPaymentMethod.Item(intRow, 2)
            ElseIf cardtype = "Personal Check" Then
                SelectedMoneycount.PersonalCheckCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Gift Certificate" Then
                SelectedMoneycount.GiftCertificateCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            ElseIf cardtype = "Travellers Check" Then
                SelectedMoneycount.TravellersCheckCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)
            ElseIf cardtype = "House Account" Then
                SelectedMoneycount.HouseAccountCount = CType(fgPaymentMethod.Item(intRow, 3), Integer)

            End If
            LoadfgBalanceSummary()
        Catch ex As Exception
            fgPaymentMethod.Item(intRow, 3) = 0
        End Try

    End Sub



    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles btnOk.Click

        SetData(True)

        SelectedSaleDay = Nothing
        SelectedSDSession = Nothing
        SelectedMoneycount = Nothing
        PriorMoneycount = Nothing
        Me.Close()
    End Sub

    Private Sub ShowErrorInBalance()
        Dim row As C1.Win.C1FlexGrid.Row
        Dim rowno As Integer
        Dim paymentstyle As C1.Win.C1FlexGrid.CellStyle
        Dim balancestyle As C1.Win.C1FlexGrid.CellStyle
        Dim errorflag As Boolean
        balancestyle = fgBalance.GetCellStyle(fgBalance.Row, 1)
        For Each row In fgPaymentMethod.Rows
            If rowno > 1 Then
                paymentstyle = fgPaymentMethod.GetCellStyle(rowno, 3)
                If paymentstyle Is Nothing Then
                    errorflag = False
                Else
                    Select Case paymentstyle.Name
                        Case "Frozen"
                            errorflag = True

                    End Select
                End If
            End If
            rowno += 1
        Next
        If errorflag = True Then
            fgBalance.SetCellStyle(fgBalance.Row, 2, fgBalance.Styles.Item("DEError"))
        Else

        End If

    End Sub

    Private Sub fgPaymentMethod_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgPaymentMethod.AfterEdit
        Dim strCardType As String
        Select Case e.Col
            Case 0
                e.Cancel = True
            Case 3
                strCardType = CType(fgPaymentMethod.Item(e.Row, 0), String)

                UpdateCreditCardCount(e.Row, strCardType)
                ValidatePaymentCount(e.Row, e.Col)
                ChangebtnEnable()
                ShowErrorInBalance()

            Case 4
                strCardType = CType(fgPaymentMethod.Item(e.Row, 0), String)


                UpdateCreditCardAmount(e.Row, strCardType)
                ValidatePaymentAmount(e.Row, e.Col)
                ChangebtnEnable()
                ShowErrorInBalance()
                'fgBalance.Item(2, 2) = m_CurrentMoneyCount.TotalCreditCardAmount()
        End Select

    End Sub


    'Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOpen.Click

    '    Dim objMoneyCounttype As MoneyCount.enumMoneyCountType
    '    objMoneyCounttype = MoneyCount.enumMoneyCountType.Opening

    '    lblStep3.Visible = False
    '    pnlStep3.Visible = False
    '    btnPrint.Visible = False
    '    lblStep2.Visible = True
    '    pnlStep2.Visible = True
    '    btnReconcile.Visible = False
    '    LoadCRBalance(objMoneyCounttype)
    'End Sub

    'Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
    '    Dim objMoneyCounttype As MoneyCount.enumMoneyCountType
    '    objMoneyCounttype = MoneyCount.enumMoneyCountType.Closing
    '    lblStep2.Visible = True
    '    pnlStep2.Visible = True
    '    lblStep3.Visible = False
    '    pnlStep3.Visible = False
    '    btnPrint.Visible = False
    '    btnReconcile.Visible = True

    '    LoadCRBalance(objMoneyCounttype)

    'End Sub

    'Private Property SelectedSDSession() As SaleDaySession
    '    Get
    '        Return m_SelectedSDSession
    '    End Get
    '    Set(ByVal Value As SaleDaySession)
    '        m_SelectedSDSession = Value
    '    End Set
    'End Property

    Private Property CurrentMoneyCounts() As MoneyCount
        Get
            Return m_CurrentMoneyCount
        End Get
        Set(ByVal Value As MoneyCount)

            m_CurrentMoneyCount = Value
        End Set
    End Property

    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim gp As C1.Win.C1FlexGrid.GridPrinter
        Dim dlg As PrintDialog
        Dim printdoc As PrintDocument
        Dim printset As PrinterSettings

        SetData(False)

        gp = fgBalance.PrintParameters
        With gp
            .Header = " Cash Summary  For Day " + dtePicker.Value.Date.ToLongDateString + ControlChars.NewLine + " Done By : " + _
                    ControlChars.NewLine + " Done At :"

            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With
        fgBalance.PrintGrid(gp.PrintDocument.ToString, C1.Win.C1FlexGrid.PrintGridFlags.FitToPage)

        gp = fgCashCount.PrintParameters
        LoadCashDetails()
        With gp
            .Header = " Cash Count  " + dtePicker.Value.Date.ToLongDateString + ControlChars.NewLine + " Done By : " + _
                    ControlChars.NewLine + " Done At :"

            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With

        fgCashCount.PrintGrid(gp.PrintDocument.ToString, C1.Win.C1FlexGrid.PrintGridFlags.FitToPage)
        LoadPaymentData()
        LoadCreditCardDetails()
        gp = fgPaymentMethod.PrintParameters

        With gp
            .Header = " Credit Card Details " + dtePicker.Value.Date.ToLongDateString
            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With
        fgPaymentMethod.PrintGrid(gp.PrintDocument.ToString, C1.Win.C1FlexGrid.PrintGridFlags.FitToPage)

        LoadOtherPaymentData()
        LoadOtherPaymentDetails()
        gp = fgPaymentMethod.PrintParameters

        With gp
            .Header = " Other Payment Details " + dtePicker.Value.Date.ToLongDateString
            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With
        fgPaymentMethod.PrintGrid(gp.PrintDocument.ToString, C1.Win.C1FlexGrid.PrintGridFlags.FitToPage)

    End Sub


    Private Sub ValidatePaymentCount(ByVal rowno As Integer, ByVal colno As Integer)
        Dim paymentcellstyle As C1.Win.C1FlexGrid.CellStyle
        Dim balancecellstyle As C1.Win.C1FlexGrid.CellStyle

        balancecellstyle = fgBalance.GetCellStyle(fgBalance.Row, 1)
        'fgPaymentMethod.Styles.Frozen.BackColor = Color.Red

        paymentcellstyle = fgPaymentMethod.GetCellStyle(rowno, colno)
        If paymentcellstyle Is Nothing Then
            'nope
        Else
            If paymentcellstyle.Name = "ValueError" Then
                paymentcellstyle = fgPaymentMethod.GetCellStyle(rowno, 0)
            End If
        End If

        With fgPaymentMethod
            .Styles.Frozen.BackColor = Color.Red
            .GetCellStyle(rowno, colno)
            If CType(.Item(rowno, 3), Integer) = 0 Then
                If CType(.Item(rowno, 4), Integer) = 0 Then
                    .SetCellStyle(rowno, 3, paymentcellstyle)
                    .SetCellStyle(rowno, 4, paymentcellstyle)
                    .Item(rowno, 5) = " "
                    .SetCellStyle(rowno, 5, paymentcellstyle)
                    fgBalance.SetCellStyle(fgBalance.Row, 2, balancecellstyle)
                ElseIf CType(fgPaymentMethod.Item(rowno, 4), Integer) > 0 Then
                    .SetCellStyle(rowno, 3, .Styles.Item("ValueError"))
                    .SetCellStyle(rowno, 4, .Styles.Item("ValueError"))
                    .Item(rowno, 5) = "Please Enter Count "
                    .SetCellStyle(rowno, 5, .Styles.Item("DEError"))
                    .SetCellStyle(fgBalance.Row, 2, fgBalance.Styles.Item("DEError"))
                End If

            ElseIf CType(.Item(rowno, 3), Integer) > 0 Then
                If CType(.Item(rowno, 4), Integer) = 0 Then

                    .SetCellStyle(rowno, 3, .Styles.Item("ValueError"))
                    .SetCellStyle(rowno, 4, .Styles.Item("ValueError"))
                    .Item(rowno, 5) = " Please Enter Amount "
                    .SetCellStyle(rowno, 5, .Styles.Item("DEError"))
                    fgBalance.SetCellStyle(fgBalance.Row, 2, fgBalance.Styles.Item("DEError"))

                ElseIf CType(fgPaymentMethod.Item(rowno, 4), Integer) > 0 Then
                    .SetCellStyle(rowno, 3, paymentcellstyle)
                    .SetCellStyle(rowno, 4, paymentcellstyle)
                    .Item(rowno, 5) = " "
                    .SetCellStyle(rowno, 5, paymentcellstyle)
                    fgBalance.SetCellStyle(fgBalance.Row, 2, balancecellstyle)
                End If

            End If
        End With
    End Sub

    Private Sub ChangebtnEnable()
        Dim row As C1.Win.C1FlexGrid.Row
        Dim paymentstyle As C1.Win.C1FlexGrid.CellStyle
        Dim rowno As Integer
        btnPrint.Enabled = True
        btnPrint.ForeColor = Color.White
        btnPreview.Enabled = True
        btnPreview.ForeColor = Color.White
        btnOk.Enabled = True
        btnOk.ForeColor = Color.White
        For Each row In fgPaymentMethod.Rows
            If rowno > 0 Then
                paymentstyle = fgPaymentMethod.GetCellStyle(rowno, 1)
                If paymentstyle Is Nothing Then
                    'nope
                Else
                    Select Case paymentstyle.Name
                        Case "ValueError"
                            btnPrint.Enabled = False
                            btnPrint.ForeColor = Color.Black
                            btnPreview.Enabled = False
                            btnPreview.ForeColor = Color.Black
                            'btnOk.Enabled = False
                            'btnOk.ForeColor = Color.Black
                            Exit For
                    End Select
                End If
            End If
            rowno += 1
        Next

    End Sub


    Private Sub ValidatePaymentAmount(ByVal rowno As Integer, ByVal colno As Integer)
        Dim paymentcellstyle As C1.Win.C1FlexGrid.CellStyle
        Dim balancecellstyle As C1.Win.C1FlexGrid.CellStyle

        balancecellstyle = fgBalance.GetCellStyle(fgBalance.Row, 1)

        'fgPaymentMethod.Styles.Frozen.BackColor = Color.Red

        paymentcellstyle = fgPaymentMethod.GetCellStyle(rowno, colno)
        If paymentcellstyle Is Nothing Then
            'nope
        Else
            If paymentcellstyle.Name = "ValueError" Then
                paymentcellstyle = fgPaymentMethod.GetCellStyle(rowno, 0)
            End If
        End If

        With fgPaymentMethod
            Try
                If CType(.Item(rowno, 4), Integer) = 0 Then
                    If CType(.Item(rowno, 3), Integer) = 0 Then
                        .SetCellStyle(rowno, 3, paymentcellstyle)
                        .SetCellStyle(rowno, 4, paymentcellstyle)
                        .Item(rowno, 5) = " "
                        .SetCellStyle(rowno, 5, paymentcellstyle)
                        fgBalance.SetCellStyle(fgBalance.Row, 2, balancecellstyle)
                    Else
                        .SetCellStyle(rowno, 3, .Styles.Item("ValueError"))
                        .SetCellStyle(rowno, 4, .Styles.Item("ValueError"))
                        .Item(rowno, 5) = " Please Enter Amount"
                        .SetCellStyle(rowno, 5, .Styles.Item("DEError"))
                        fgBalance.SetCellStyle(fgBalance.Row, 2, fgBalance.Styles.Item("DEError"))
                    End If

                ElseIf CType(.Item(rowno, 4), Integer) > 0 Then
                    If CType(.Item(rowno, 3), Integer) = 0 Then
                        .SetCellStyle(rowno, 3, .Styles.Item("ValueError"))
                        .SetCellStyle(rowno, 4, .Styles.Item("ValueError"))
                        .Item(rowno, 5) = " Please Enter Count"
                        .SetCellStyle(rowno, 5, .Styles.Item("DEError"))
                        fgBalance.SetCellStyle(fgBalance.Row, 2, fgBalance.Styles.Item("DEError"))
                    ElseIf CType(.Item(rowno, 3), Integer) > 0 Then
                        .SetCellStyle(rowno, 3, paymentcellstyle)
                        .SetCellStyle(rowno, 4, paymentcellstyle)
                        .Item(rowno, 5) = " "
                        .SetCellStyle(rowno, 5, paymentcellstyle)
                        fgBalance.SetCellStyle(fgBalance.Row, 2, balancecellstyle)
                    End If
                End If
            Catch ex As Exception
                .Item(rowno, .ColSel) = 0
            End Try
        End With


    End Sub



    'Private Sub fgPaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles fgPaymentMethod.KeyPress
    '    With fgPaymentMethod
    '        Select Case CStr(e.KeyChar)
    '            Case "+", "="
    '                Try
    '                    .Item(.Row, .Col) = .Item(.Row, .Col) + 1
    '                Catch ex As Exception
    '                    .Item(.Row, .Col) = 0
    '                Finally
    '                    e.Handled = True
    '                End Try
    '            Case "-"
    '                Try
    '                    If .Item(.Row, .Col) > 0 Then
    '                        .Item(.Row, .Col) = .Item(.Row, .Col) - 1
    '                    End If
    '                Catch ex As Exception
    '                    .Text = "0"
    '                Finally
    '                    e.Handled = True
    '                End Try
    '            Case Else
    '                Select Case Char.IsDigit(e.KeyChar)
    '                    Case True
    '                        e.Handled = False
    '                    Case Else
    '                        Select Case (Char.IsLetter(e.KeyChar) Or (Char.IsSeparator(e.KeyChar)))
    '                            Case True
    '                                e.Handled = True
    '                            Case Char.IsPunctuation(e.KeyChar)
    '                                If .Col = 1 Then
    '                                    e.Handled = True
    '                                End If
    '                            Case Else
    '                                Select Case Char.IsSymbol(e.KeyChar)
    '                                    Case True
    '                                        e.Handled = True
    '                                    Case Else
    '                                        e.Handled = True
    '                                End Select
    '                        End Select
    '                End Select
    '        End Select
    '    End With
    'End Sub

    Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        Dim gp As C1.Win.C1FlexGrid.GridPrinter
        Dim dlg As PrintPreviewDialog

        ' SetData()
        gp = fgBalance.PrintParameters
        With gp
            .Header = " Cash Summary  For Day " + dtePicker.Value.Date.ToLongDateString + ControlChars.NewLine + " Done By : " + _
                    ControlChars.NewLine + " Done At :"

            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With
        dlg = New PrintPreviewDialog()
        dlg.Document = gp.PrintDocument

        dlg.ShowDialog()


        gp = fgCashCount.PrintParameters
        LoadCashDetails()
        With gp
            .Header = " Cash Count  " + dtePicker.Value.Date.ToLongDateString + ControlChars.NewLine + " Done By : " + _
                    ControlChars.NewLine + " Done At :"

            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With

        dlg.Document = gp.PrintDocument
        dlg.ShowDialog()

        LoadPaymentData()
        LoadCreditCardDetails()
        gp = fgPaymentMethod.PrintParameters

        With gp
            .Header = " Credit Card Details " + dtePicker.Value.Date.ToLongDateString
            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With
        dlg.Document = gp.PrintDocument

        dlg.ShowDialog()


        LoadOtherPaymentData()
        LoadOtherPaymentDetails()
        gp = fgPaymentMethod.PrintParameters

        With gp
            .Header = " Other Payment Details " + dtePicker.Value.Date.ToLongDateString
            .HeaderFont = New Font("Ariel", 13, FontStyle.Bold)
            .Footer = gp.PageNumber.ToString + " of " + gp.PageCount.ToString
            .FooterFont = New Font("Ariel", 12, FontStyle.Bold)
            .PrintGridFlags = C1.Win.C1FlexGrid.PrintGridFlags.FitToPage
        End With
        dlg.Document = gp.PrintDocument

        dlg.ShowDialog()

    End Sub



    'Private Sub fgCashCount_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles fgCashCount.KeyPress

    '    Dim intValue As Integer

    '    'must obtain override at this stage 
    '    Select Case SelectedSDSession.MoneyCountEditAllowed
    '        Case False
    '            e.Handled = True    'Terminate any edit action by user
    '            Exit Sub
    '        Case True

    '            Select Case e.KeyChar.IsDigit(e.KeyChar)
    '                Case True
    '                    e.Handled = False
    '                Case False
    '                    Select Case e.KeyChar.IsSymbol(e.KeyChar) Or e.KeyChar.IsLetter(e.KeyChar) Or _
    '                                e.KeyChar.IsPunctuation(e.KeyChar) Or e.KeyChar.IsSeparator(e.KeyChar)
    '                        Case True
    '                            e.Handled = True

    '                        Case False

    '                            e.Handled = False

    '                    End Select
    '            End Select

    '    End Select
    'End Sub

    'Private Sub fgPaymentMethod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles fgPaymentMethod.KeyPress
    '    Dim dblValue As Double

    '    'must obtain override at this stage 
    '    Select Case SelectedSDSession.MoneyCountEditAllowed
    '        Case False
    '            e.Handled = True    'Terminate any edit action by user
    '            ' Exit Sub
    '        Case True
    '            Select Case e.KeyChar.IsLetter(e.KeyChar) Or e.KeyChar.IsSeparator(e.KeyChar)
    '                Case True
    '                    e.Handled = True
    '                Case False
    '                    Select Case e.KeyChar.IsDigit(e.KeyChar)
    '                        Case True
    '                            e.Handled = False
    '                        Case False

    '                            Try


    '                                dblValue = CType(CStr(e.KeyChar & "1"), Double)   'CType(CType(fgPaymentMethod.Item(fgPaymentMethod.Row, fgPaymentMethod.Col), Double) & e.KeyChar, Double)
    '                                If dblValue > 0 Then
    '                                    e.Handled = False
    '                                Else
    '                                    e.Handled = True    'Discard  key presses by user resulting in negative number  
    '                                End If
    '                            Catch ex As Exception
    '                                e.Handled = True    'Discard non numeric key presses by user  
    '                            Finally
    '                            End Try
    '                    End Select

    '            End Select


    '    End Select


    'End Sub

    Private Property CashCountEditAllowed() As Boolean
        Get
            Return m_EditAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_EditAllowed = Value
        End Set

    End Property




    Private Sub fgCashCount_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles fgCashCount.KeyPressEdit

        Dim intValue As Integer

        'must obtain override at this stage 
        Select Case SelectedSDSession.MoneyCountEditAllowed(SelectedMoneycount)
            Case False
                e.Handled = True    'Terminate any edit action by user
                Exit Sub
            Case True

                Select Case e.KeyChar.IsDigit(e.KeyChar)
                    Case True
                        e.Handled = False
                    Case False

                        Select Case e.KeyChar.IsSymbol(e.KeyChar) Or e.KeyChar.IsLetter(e.KeyChar) Or _
                                    e.KeyChar.IsPunctuation(e.KeyChar) Or e.KeyChar.IsSeparator(e.KeyChar)
                            Case True
                                e.Handled = True

                            Case False

                                e.Handled = False

                        End Select
                End Select

        End Select

    End Sub

    Private Sub fgPaymentMethod_KeyPressEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.KeyPressEditEventArgs) Handles fgPaymentMethod.KeyPressEdit
        Dim dblValue As Double
        Select Case SelectedSDSession.MoneyCountEditAllowed(SelectedMoneycount)
            Case False
                e.Handled = True    'Terminate any edit action by user
                ' Exit Sub
            Case True
                Select Case e.KeyChar.IsLetter(e.KeyChar) Or e.KeyChar.IsSeparator(e.KeyChar)
                    Case True
                        e.Handled = True
                    Case False
                        Try
                            If e.KeyChar = ControlChars.Back Or e.KeyChar.GetNumericValue(e.KeyChar) = CType(Shortcut.Del, Double) Then
                                e.Handled = False
                            Else

                                dblValue = CType(CStr(e.KeyChar & "1"), Double) 'CType(CType(fgPaymentMethod.Item(fgPaymentMethod.Row, fgPaymentMethod.Col), Double) & e.KeyChar, Double) 
                                If dblValue >= 0 Then
                                    e.Handled = False
                                Else
                                    e.Handled = True    'Discard  key presses by user resulting in negative number  
                                End If
                            End If
                        Catch ex As Exception
                            e.Handled = True    'Discard non numeric key presses by user  
                        Finally
                        End Try
                End Select
        End Select
    End Sub

    Private Sub OnOverrideNeeded(ByVal objOverride As BOPr.Override) Handles m_SelectedSDSession.OverrideNeeded
        Dim newfrmPOSOverride As frmPOSOverride
        newfrmPOSOverride = New frmPOSOverride()
        newfrmPOSOverride.newOverride = objOverride
        newfrmPOSOverride.ShowDialog()
    End Sub

    Private Sub fgCashCount_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgCashCount.EnterCell
        Dim intNextRow As Integer

        With fgCashCount
            Select Case .ColSel
                Case Is < 0
                    Exit Sub
                Case 0, 1
                    .ColSel = 2
                    .Select(.RowSel, .ColSel)
                    .ShowCell(.RowSel, .ColSel)

                Case 3
                    If .RowSel <= 5 Then
                        .ColSel = .ColSel + 1
                        .Select(.RowSel, .ColSel)
                        .ShowCell(.RowSel, .ColSel)
                    Else
                        If .RowSel = .Rows.Count - 1 Then
                            intNextRow = .Rows.Fixed()
                        Else
                            intNextRow = .RowSel + 1
                        End If

                        .Select(intNextRow, 2)
                        .ShowCell(.RowSel, 2)


                    End If

                Case 5
                    Select Case .ColSel = .Cols.Count - 1

                        Case True
                            If .RowSel = .Rows.Count - 1 Then
                                intNextRow = .Rows.Fixed()
                            Else
                                intNextRow = .RowSel + 1
                            End If
                            .Select(intNextRow, 2)
                            .ShowCell(intNextRow, 2)
                    End Select
            End Select
        End With

    End Sub

    Private Sub fgPaymentMethod_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgPaymentMethod.EnterCell

        Dim intNextRow As Integer

        With fgPaymentMethod
            Select Case .ColSel
                Case Is < 0
                    Exit Sub

                Case 0, 1, 2
                    .ColSel = 3
                    .Select(.RowSel, .ColSel)
                    .ShowCell(.RowSel, .ColSel)


                Case 3, 4, 5

                    Select Case .ColSel = .Cols.Count - 1

                        Case True
                            If .RowSel = .Rows.Count - 1 Then
                                intNextRow = .Rows.Fixed()
                            Else
                                intNextRow = .RowSel + 1
                            End If
                            .Select(intNextRow, 1)
                            .ShowCell(intNextRow, 1)
                    End Select
            End Select
        End With
    End Sub

    Private Sub fgBalance_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles fgBalance.KeyDown
        If e.KeyCode = Keys.Enter Then

            Select Case fgBalance.RowSel
                Case 1

                    LoadCashDetails()
                    With fgCashCount
                        .Visible = True
                        .Enabled = True
                        .Focus()
                    End With
                    fgPaymentMethod.Visible = False
                    fgPaymentMethod.Enabled = False



                Case 2
                    fgCashCount.Visible = False
                    fgCashCount.Enabled = False
                    With fgPaymentMethod
                        .Clear(C1.Win.C1FlexGrid.ClearFlags.Content)
                        .Enabled = True
                        .Visible = True
                        .Row = 1
                        .Col = 1
                        .Focus()

                    End With
                    LoadPaymentData()
                    LoadCreditCardDetails()

                Case 3

                    fgCashCount.Visible = False
                    fgCashCount.Enabled = False
                    With fgPaymentMethod
                        .Clear(C1.Win.C1FlexGrid.ClearFlags.Content)
                        .Enabled = True
                        .Visible = True
                        .Row = 1
                        .Col = 1
                        .Focus()
                    End With
                    LoadOtherPaymentData()
                    LoadOtherPaymentDetails()
                Case 4
                    fgPaymentMethod.Visible = False
                    fgCashCount.Visible = False
            End Select
        End If
    End Sub

    Private Property SelectedSaleDay() As SaleDay
        Get
            Return m_SelectedSaleDay
        End Get
        Set(ByVal Value As SaleDay)
            m_SelectedSaleDay = Value
        End Set
    End Property

    Private Sub SetSelectedSaleDay()
        CashCountEditAllowed = False
        SelectedSaleDay = SaleDays.CreateSaleDay.Item(dtePicker.Value.Date, True)
        'EnableFormBtns()
    End Sub

    Private Property SelectedSDSession() As SaleDaySession
        Get
            Return m_SelectedSDSession
        End Get
        Set(ByVal Value As SaleDaySession)
            m_SelectedSDSession = Value
        End Set
    End Property

    Private Property SelectedCashCountType() As MoneyCount.enumMoneyCountType
        Get
            Return m_SelectedCashCountType
        End Get
        Set(ByVal Value As MoneyCount.enumMoneyCountType)
            m_SelectedCashCountType = Value
        End Set
    End Property

    Private Property SelectedMoneycount() As MoneyCount
        Get
            Return m_SelectedMoneyCount
        End Get
        Set(ByVal Value As MoneyCount)
            m_SelectedMoneyCount = Value
        End Set
    End Property

    Private Property TimeFrom() As DateTime
        Get
            Return m_timeFrom
        End Get
        Set(ByVal Value As DateTime)
            m_timeFrom = Value
        End Set
    End Property

    Private Property TimeTo() As DateTime
        Get
            Return m_timeTo
        End Get
        Set(ByVal Value As DateTime)
            m_timeTo = Value
        End Set
    End Property

    Private Sub SetSelectedSDSessionAndCashCountType()

        Dim enumSessionId As Session.enumSessionName
        Dim enumCashCountType As MoneyCount.enumMoneyCountType

        CashCountEditAllowed = False
        If rbtnLunchOpen.Checked = True Then
            enumSessionId = Session.enumSessionName.Lunch
            SelectedCashCountType = MoneyCount.enumMoneyCountType.Opening
        ElseIf rbtnLunchClose.Checked = True Then
            enumSessionId = Session.enumSessionName.Lunch
            SelectedCashCountType = MoneyCount.enumMoneyCountType.Closing
        ElseIf rbtnDinnerOpen.Checked = True Then
            enumSessionId = Session.enumSessionName.Dinner
            SelectedCashCountType = MoneyCount.enumMoneyCountType.Opening
        ElseIf rbtnDinnerClose.Checked = True Then
            enumSessionId = Session.enumSessionName.Dinner
            SelectedCashCountType = MoneyCount.enumMoneyCountType.Closing
        Else
            'error
        End If
        SelectedSDSession = SelectedSaleDay.AllSaleDaySessions.Item(enumSessionId)
    End Sub



    'Private ReadOnly Property PriorSDSession() As SaleDaySession
    '    Get
    '        Return m_PriorSDSession
    '    End Get
    'End Property

    'Private ReadOnly Property PriorCashCountType() As MoneyCount.enumMoneyCountType
    '    Get
    '        Return m_PriorCashCountType
    '    End Get
    'End Property

    Private Property PriorMoneycount() As MoneyCount
        Get
            Return m_PriorMoneyCount
        End Get
        Set(ByVal Value As MoneyCount)
            m_PriorMoneyCount = Value
        End Set
    End Property

    Private Sub SetSelectedAndPriorMoneyCounts()
        Dim objPriorSaleDay As SaleDay
        Dim objPriorSDSession As SaleDaySession
        Dim enumPriorCashCountType As MoneyCount.enumMoneyCountType

        SelectedMoneycount = SelectedSDSession.AllMoneyCounts.Item(SelectedCashCountType)

        With SelectedSDSession
            If (.SessionId = Session.enumSessionName.Lunch) AndAlso (SelectedCashCountType = MoneyCount.enumMoneyCountType.Opening) Then
                objPriorSaleDay = SelectedSaleDay.PriorSaleDay
                If objPriorSaleDay Is Nothing Then
                    objPriorSaleDay = SelectedSaleDay
                    objPriorSDSession = SelectedSDSession
                    enumPriorCashCountType = MoneyCount.enumMoneyCountType.Opening
                    TimeFrom = SelectedSDSession.SaleDaySessionFrom
                Else
                    objPriorSDSession = objPriorSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Dinner)
                    enumPriorCashCountType = MoneyCount.enumMoneyCountType.Closing
                    TimeFrom = objPriorSDSession.SaleDaySessionTo
                End If

                If SelectedSDSession.SaleDaySessionFrom > DateTime.MinValue Then
                    TimeTo = SelectedSDSession.SaleDaySessionFrom
                Else
                    TimeTo = SaleDays.CreateSaleDay.Now
                End If
                m_IsCCSettlementdone = chkCCSettlement.Checked
            ElseIf (.SessionId = Session.enumSessionName.Lunch) AndAlso (SelectedCashCountType = MoneyCount.enumMoneyCountType.Closing) Then
                objPriorSaleDay = SelectedSaleDay
                objPriorSDSession = SelectedSDSession
                enumPriorCashCountType = MoneyCount.enumMoneyCountType.Opening
                TimeFrom = SelectedSDSession.SaleDaySessionFrom
                If SelectedSDSession.SaleDaySessionTo > DateTime.MinValue Then
                    TimeTo = SelectedSDSession.SaleDaySessionTo
                Else
                    TimeTo = SaleDays.CreateSaleDay.Now
                End If
                m_IsCCSettlementdone = False
            ElseIf (.SessionId = Session.enumSessionName.Dinner) AndAlso (SelectedCashCountType = MoneyCount.enumMoneyCountType.Opening) Then
                objPriorSaleDay = SelectedSaleDay
                objPriorSDSession = objPriorSaleDay.AllSaleDaySessions.Item(Session.enumSessionName.Lunch)
                enumPriorCashCountType = MoneyCount.enumMoneyCountType.Closing
                TimeFrom = objPriorSDSession.SaleDaySessionTo
                If SelectedSDSession.SaleDaySessionFrom > DateTime.MinValue Then
                    TimeTo = SelectedSDSession.SaleDaySessionFrom
                Else
                    TimeTo = SaleDays.CreateSaleDay.Now
                End If
                m_IsCCSettlementdone = False
            ElseIf (.SessionId = Session.enumSessionName.Dinner) AndAlso (SelectedCashCountType = MoneyCount.enumMoneyCountType.Closing) Then
                objPriorSaleDay = SelectedSaleDay
                objPriorSDSession = SelectedSDSession
                enumPriorCashCountType = MoneyCount.enumMoneyCountType.Opening
                TimeFrom = SelectedSDSession.SaleDaySessionFrom
                If SelectedSDSession.SaleDaySessionTo > DateTime.MinValue Then
                    TimeTo = SelectedSDSession.SaleDaySessionTo
                Else
                    TimeTo = SaleDays.CreateSaleDay.Now
                End If
                m_IsCCSettlementdone = False
            Else
                'error
            End If
        End With
        PriorMoneycount = objPriorSDSession.AllMoneyCounts.Item(enumPriorCashCountType)
    End Sub

    Private Sub dtePicker_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtePicker.Click
        'SetSelectedSaleDay()
        'InitialzeSessionData()
    End Sub

    Private Sub dtePicker_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtePicker.CloseUp
        SetSelectedSaleDay()
        InitialzeSessionData()
    End Sub
    Private Sub rbtnLunchOpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnLunchOpen.Click, rbtnLunchClose.Click, rbtnDinnerOpen.Click, rbtnDinnerClose.Click
        InitialzeSessionData()
        Select Case CType(sender, RadioButton).Name
            Case "LunchOpen"
                chkCCSettlement.Checked = True
            Case Else
                chkCCSettlement.Checked = False
        End Select
        'SetSelectedSaleDay()
        'SetSelectedSDSessionAndCashCountType()
        'SetSelectedAndPriorMoneyCounts()
        'HideGrids()
        btnShowCount.Enabled = True
        'EnableFormBtns()
    End Sub

    Private Sub btnShowCount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowCount.Click
        SetData(True)
        SetSelectedSaleDay()
        SetSelectedSDSessionAndCashCountType()
        SetSelectedAndPriorMoneyCounts()
        Me.Cursor = Cursors.WaitCursor
        SetPanel2LabelText()
        fgBalance.Enabled = True
        LoadfgBalanceSummary()
        Me.Cursor = Cursors.Default
        EnableFormBtns()
    End Sub

    Private Sub HideGrids()
        'fgCashCount.Visible = False
        'fgPaymentMethod.Visible = False
    End Sub

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Dim objMoneyCounts As MoneyCounts

        Select Case MsgBox("Press OK to go back to Cash, CC & Other counts saved previously, Or Cancel to keep Counts shown", MsgBoxStyle.OKCancel, "Confirm Reset?")

            Case MsgBoxResult.OK
                objMoneyCounts = SelectedSDSession.AllMoneyCounts
                objMoneyCounts.Remove(objMoneyCounts.Indexof(SelectedMoneycount))
                SetSelectedAndPriorMoneyCounts()
                LoadfgBalanceSummary()
                If (fgPaymentMethod.Visible = True) AndAlso (fgBalance.RowSel = CType(enumfgBalanceSummary.CC, Integer)) Then
                    LoadCreditCardDetails()
                ElseIf (fgPaymentMethod.Visible = True) AndAlso (fgBalance.RowSel = CType(enumfgBalanceSummary.Other, Integer)) Then
                    LoadOtherPaymentDetails()
                ElseIf fgCashCount.Visible Then
                    LoadCashDetails()
                Else
                    'nop
                End If
                SetData(False)
            Case Else
                'nop
        End Select
    End Sub

    Private Sub LoadfgBalanceSummary()
        SelectedSaleDay.SetAllCurrentPayments(TimeFrom, TimeTo)
        SelectedSaleDay.SetAllNonCurrentPayments(TimeFrom, TimeTo)
        fgBalance.SuspendLayout()

        'Load the Actuals
        With fgBalance
            .Item(enumfgBalanceSummary.Cash, fgBalance.Cols.Item("ActualAmount").Index) = SelectedMoneycount.TotalCashAmount
            .Item(enumfgBalanceSummary.CC, fgBalance.Cols.Item("ActualAmount").Index) = SelectedMoneycount.TotalCreditCardAmount
            .Item(enumfgBalanceSummary.Other, fgBalance.Cols.Item("ActualAmount").Index) = SelectedMoneycount.TotalCheckAmount
            .Item(enumfgBalanceSummary.Total, fgBalance.Cols.Item("ActualAmount").Index) = SelectedMoneycount.TotalAmount

            'Calculate POS Balances as Balances from Prior MoneyCount + POSAmountsPaid - Pettycash (CRTxns) 
            .Item(enumfgBalanceSummary.Cash, fgBalance.Cols.Item("POSAmount").Index) = _
                        PriorMoneycount.TotalCashAmount _
                        + SelectedSaleDay.CashTotalAmountForDay _
                        + SelectedSaleDay.NonCurrentCashTotalAmount _
                        - SelectedSaleDay.CRTXnAmount(TimeFrom, TimeTo)
            If m_IsCCSettlementdone = True Then
                .Item(enumfgBalanceSummary.CC, fgBalance.Cols.Item("POSAmount").Index) = 0
            Else
                .Item(enumfgBalanceSummary.CC, fgBalance.Cols.Item("POSAmount").Index) = _
                            PriorMoneycount.TotalCreditCardAmount _
                            + SelectedSaleDay.CreditCardTotalAmountForDay _
                            + SelectedSaleDay.NonCurrentCreditCardTotalAmount
            End If
            If m_IsCCSettlementdone = True Then
                .Item(enumfgBalanceSummary.Other, fgBalance.Cols.Item("POSAmount").Index) = 0
            Else
                .Item(enumfgBalanceSummary.Other, fgBalance.Cols.Item("POSAmount").Index) = _
                       PriorMoneycount.TotalCheckAmount _
                       + SelectedSaleDay.OtherTotalAmountForDay _
                       + SelectedSaleDay.NonCurrentOtherTotalAmount
            End If
            .Item(enumfgBalanceSummary.Total, fgBalance.Cols.Item("POSAmount").Index) = _
            CDbl(.Item(enumfgBalanceSummary.Cash, fgBalance.Cols.Item("POSAmount").Index)) _
            + CDbl(.Item(enumfgBalanceSummary.CC, fgBalance.Cols.Item("POSAmount").Index)) _
            + CDbl(.Item(enumfgBalanceSummary.Other, fgBalance.Cols.Item("POSAmount").Index))
            .ResumeLayout()
        End With
    End Sub

    Private Sub EnableFormBtns()
        btnPrint.Enabled = True
        btnPreview.Enabled = True
        btnOk.Enabled = True
        btnReset.Enabled = True
        btnShowCount.Enabled = True
        Select Case SelectedCashCountType
            Case MoneyCount.enumMoneyCountType.Opening
                chkCopyOpenBalances.Enabled = True
                Select Case SelectedSDSession.SessionId
                    Case Session.enumSessionName.Lunch
                        chkCCSettlement.Enabled = True
                    Case Else
                        chkCCSettlement.Enabled = False
                End Select
            Case Else
                chkCopyOpenBalances.Enabled = False
                chkCopyOpenBalances.Checked = False
        End Select
    End Sub

    Private Sub DisableFormBtns()

        If (SelectedSaleDay Is Nothing) OrElse (SelectedSDSession Is Nothing) Then
            btnShowCount.Enabled = False
            btnPrint.Enabled = False
            btnPreview.Enabled = False
            'btnOk.Enabled = False
            btnReset.Enabled = False
            chkCCSettlement.Enabled = False
            chkCCSettlement.Checked = False
            chkCopyOpenBalances.Enabled = False
            chkCopyOpenBalances.Checked = False
        End If
    End Sub

    Private Sub InitialzeSessionData()

        btnPrint.Enabled = False
        btnPreview.Enabled = False
        'btnOk.Enabled = False
        btnReset.Enabled = False
        chkCCSettlement.Enabled = False
        chkCCSettlement.Checked = False
        chkCopyOpenBalances.Enabled = False
        chkCopyOpenBalances.Checked = False
        fgPaymentMethod.Visible = False
        fgCashCount.Visible = False
        fgBalance.Enabled = False
        fgBalance.Clear(C1FlexGrid.ClearFlags.Content)
        LoadfgBalanceSummaryFixedData()

    End Sub

    Private Sub InitialzeFormData()
        dtePicker.Value = Today
        rbtnDinnerClose.Checked = False
        rbtnDinnerOpen.Checked = False
        rbtnLunchClose.Checked = False
        rbtnLunchOpen.Checked = False
        btnShowCount.Enabled = False
        btnPrint.Enabled = False
        btnPreview.Enabled = False
        'btnOk.Enabled = False
        btnReset.Enabled = False
        chkCCSettlement.Enabled = False
        chkCCSettlement.Checked = False
        chkCopyOpenBalances.Enabled = False
        chkCopyOpenBalances.Checked = False
        fgPaymentMethod.Visible = False
        fgCashCount.Visible = False
        fgBalance.Enabled = False
        DisableFormBtns()
        fgBalance.Clear(C1FlexGrid.ClearFlags.Content)
        LoadfgBalanceSummaryFixedData()

    End Sub

    Private Sub frmCashRegisterCount_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        'HideGrids()
    End Sub

    Private Sub chkCopyOpenBalances_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCopyOpenBalances.Click
        SelectedMoneycount.CopyStateFrom(PriorMoneycount)
        SetData(False)
        LoadfgBalanceSummary()
    End Sub

    Private Sub frmCashRegisterCount_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.VisibleChanged
        ' HideGrids()
    End Sub


End Class

