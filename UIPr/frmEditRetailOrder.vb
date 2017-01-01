Option Strict On
Imports C1.Win
Imports System.Windows.Forms.Control
Imports BOPr
Imports PosPrintServer

Public Class frmEditRetailOrder
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call
        'SetUI()
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
        '
        'frmEditRetailOrder
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(984, 590)
        Me.Name = "frmEditRetailOrder"
        Me.Text = "frmEditRetailOrder"

    End Sub

#End Region
    Private WithEvents splitter1 As System.Windows.Forms.Splitter
    Private WithEvents panelHTop As System.Windows.Forms.Panel
    Private WithEvents panelHBot As System.Windows.Forms.Panel

    Private WithEvents splitterBot As System.Windows.Forms.Splitter
    Private WithEvents panelMenu As System.Windows.Forms.Panel
    Private WithEvents panelBotRight As System.Windows.Forms.Panel
    Private WithEvents panelOrder As System.Windows.Forms.Panel
    Private WithEvents panelOrderTotals As System.Windows.Forms.Panel
    Private WithEvents panelFormBtns As System.Windows.Forms.Panel

    Private WithEvents splitterTop As System.Windows.Forms.Splitter
    Private WithEvents panelInfo As System.Windows.Forms.Panel
    Private WithEvents panelSelectOrder As System.Windows.Forms.Panel

    Private WithEvents pnlOrderFilters As Panel
    Private WithEvents btnEditOrderFiters As Button
    Private WithEvents cboOrderStatusFilter As ComboBox
    Private WithEvents cboSessionFilter As ComboBox
    Private WithEvents dteSaleDateFilter As DateTimePicker

    Private WithEvents tabCurrentOrders As TabControl

    'Eat-in New Order Tab Page controls
    Private WithEvents cboTables As ComboBox
    Private WithEvents cboEatInWaiters As ComboBox
    Private WithEvents cboEatInCustomers As ComboBox
    Private WithEvents chkDesiEatin As CheckBox
    Private WithEvents txtAdults As TextBox
    Private WithEvents txtKids As TextBox
    Private WithEvents dteEatInOrderPromisedAt As DateTimePicker
    Private WithEvents btnOKEatInNewOrder As Button
    Private WithEvents btnCancelEatInNewOrder As Button

    'Take Out New Order Tab Page Controls
    Private WithEvents cboTakeOutWaiters As ComboBox
    Private WithEvents cboTakeOutCustomers As ComboBox
    Private lblCustomers As Label
    'Private WithEvents txtCustomerName As TextBox
    Private WithEvents txtTakeOutCustomer As TextBox
    Private WithEvents cboPhoneNo As ComboBox
    Private WithEvents cboFoodProvidedBy As ComboBox
    Private WithEvents cboOrderReceivedFrom As ComboBox
    Private WithEvents txtAddress As TextBox
    'Private WithEvents txtDirections As TextBox
    Private WithEvents dteTakeOutOrderPromisedAt As DateTimePicker
    Private WithEvents btnOKTakeOutNewOrder As Button
    Private WithEvents btnCancelTakeOutNewOrder As Button
    Private WithEvents chkDelivery As CheckBox
    Private WithEvents chkDesi As CheckBox
    Private WithEvents chkHouseACOrderEatIn As CheckBox
    Private WithEvents chkHouseACOrderTakeOut As CheckBox

    'Product Category Tab Controls (to display Menu
    Private WithEvents tabMenu As TabControl
    Private WithEvents fgMenuItems As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents cboProductFamily As ComboBox
    'Order Grid & Panel
    Private WithEvents fgorder As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents chkLbo As CheckedListBox
    Private WithEvents lblOrderSubTotal As Label
    'This grid, aligned to the Order grid, shows the totals, discount , TipType & taxes for the order
    Private WithEvents fgOrderTotals As C1.Win.C1FlexGrid.C1FlexGrid

    Private WithEvents cboTips As ComboBox
    Private WithEvents cboDiscount As ComboBox
    Private WithEvents cboTax As ComboBox
    'Private WithEvents cboprint As ComboBox

    'Form Buttons
    Private WithEvents btnKOT As Button
    Private WithEvents btnCheck As Button
    Private WithEvents btnHouseCustomer As Button
    Private WithEvents btnPay As Button
    Private WithEvents btnReset As Button
    Private WithEvents btnClose As Button
    Private WithEvents btnSave As Button
    Private WithEvents btnVoid As Button
    Private WithEvents btnVoid1 As Button

    Private WithEvents fgSelectOrderEatin As C1.Win.C1FlexGrid.C1FlexGrid
    Private WithEvents fgSelectOrderTakeOut As C1.Win.C1FlexGrid.C1FlexGrid
    Private m_CurrentRowOfExistingOrderGrid As C1.Win.C1FlexGrid.Row
    Private m_SortColumnSelectedOrdersTakeOutGrid As C1.Win.C1FlexGrid.Column
    Private m_SortColumnSelectedOrdersEatInGrid As C1.Win.C1FlexGrid.Column

    Private m_ProductCategory As ProductCategory

    Private WithEvents m_CurrentOrder As Order

    Private m_SelectedOrderTypeOrSection As Orders.EnumFilterOrderTypeOrSection
    Private m_SelectedOrderStatus As Orders.EnumFilter
    Private m_selectedSDSession As SaleDaySession
    Private m_SelectedSaleDay As SaleDay
    Private WithEvents m_SelectedOrders As Orders

    Private WithEvents m_TableForNewOrder As DTable
    'Private m_CustomerForNewOrder As Customer
    Private m_WaiterForNewOrder As Employee

    Private WithEvents m_frmEditPaymentPr As frmEditPaymentPr
    Private m_POSPrint As PosPrintServer.Printer
    Private m_POSText As PosPrintServer.PosText

    Private Sub frmEditRetailOrder_FormClosed(sender As Object, e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        CloseMe()
    End Sub

    Private Sub frmEditRetailOrder_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        CloseMe()
        e.Cancel = False
    End Sub

    Private Sub frmEditRetailOrder_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown

    End Sub

    Private Sub frmEditRetailOrder_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Me.KeyPress

    End Sub

    Private Sub frmEditRetailOrder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        SetUI()
        EnableDisableFormBtns()
        CurrentOrderPageSelected()  'initially load the current orders grid
    End Sub

    Public Sub SetUI()
        m_POSPrint = Printer.Instance
        m_POSText = New PosText()
        Me.SuspendLayout()
        With SaleDays.CreateSaleDay
            SetSelectedSD(.GetTargetSaleDate)
            SetSelectedSDSession(.Now)
        End With

        Me.components = New System.ComponentModel.Container()
        Me.splitter1 = New System.Windows.Forms.Splitter()
        Me.panelHTop = New System.Windows.Forms.Panel()
        Me.panelHBot = New System.Windows.Forms.Panel()

        With Me.panelHTop
            .Dock = System.Windows.Forms.DockStyle.Top
            .Height = Me.ClientSize.Height \ 4
            .TabIndex = 0
            .BorderStyle = BorderStyle.Fixed3D
        End With

        With Me.panelHBot
            .Dock = System.Windows.Forms.DockStyle.Fill
            .TabIndex = 1
            .BorderStyle = BorderStyle.Fixed3D
        End With

        SetPanelHBot()

        With Me.splitter1
            .Dock = System.Windows.Forms.DockStyle.Top
            .BorderStyle = BorderStyle.Fixed3D
            .Height = 3
            .TabIndex = 1
            .BackColor = Color.LightSteelBlue
            .TabIndex = 1
            ' Set TabStop to false for ease of use when negotiating
            ' the user interface.
            .TabStop = False
        End With
        SetPanelHTop()
        Dim lblHdr As New Label()
        With lblHdr
            .Text = "Eat In & Take Out Orders"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.MiddleCenter
            .BackColor = Color.SandyBrown
            .Dock = DockStyle.Top
            .Height = 20
        End With
        ' The order of the controls in this call is critical.
        Me.Controls.AddRange(New System.Windows.Forms.Control() _
           {Me.panelHBot, Me.splitter1, Me.panelHTop, lblHdr})
        Me.Text = "Eat in & Take Out Order "
        Me.AutoScroll = False
        Me.ControlBox = False
        Me.FormBorderStyle = FormBorderStyle.None
        Me.ResumeLayout()
        'go to take out page on load 6/27/16
        ShowAppropriateCurrentOrderTabPage()
    End Sub

    Private Sub SetPanelHBot()
        Dim lblHdr2 As New Label()

        With lblHdr2
            .Text = "Menu"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.MiddleCenter
            .BackColor = Color.SandyBrown
            .Dock = DockStyle.Top
            .Height = 20
        End With

        panelMenu = New Panel()

        cboProductFamily = New ComboBox()
        With cboProductFamily
            .Width = 180
            .Height = 100
            .DropDownStyle = ComboBoxStyle.DropDownList
        End With
        With Me.panelMenu
            .BackColor = Color.White
            .Dock = System.Windows.Forms.DockStyle.Left
            .BorderStyle = BorderStyle.Fixed3D
            .Width = CInt(Me.ClientSize.Width / 4.25)
            .TabIndex = 0
            SetMenuCategoryTab()
            .Controls.AddRange(New System.Windows.Forms.Control() {tabMenu, lblHdr2})
        End With

        panelBotRight = New Panel()

        With Me.panelBotRight
            .BackColor = Color.White
            .Dock = System.Windows.Forms.DockStyle.Fill
            .BorderStyle = BorderStyle.Fixed3D
            .TabIndex = 1
            SetPanelOrder()
            SetpanelOrderTotals()
            .Controls.AddRange(New System.Windows.Forms.Control() _
            {Me.panelOrder, Me.panelOrderTotals})
        End With

        ' Set properties of the Splitter control for Bottom panel -divides menu & orderdetails.
        splitterBot = New Splitter()
        With Me.splitterBot
            .Location = New System.Drawing.Point(121, 0)
            .Width = 3
            .BackColor = Color.LightSteelBlue
            .TabStop = False
        End With
        With panelHBot
            .Controls.AddRange(New System.Windows.Forms.Control() _
               {Me.panelBotRight, Me.splitterBot, Me.panelMenu})
        End With
    End Sub

    Private Sub SetPanelOrder()
        panelOrder = New Panel()

        With Me.panelOrder
            .BackColor = Color.AntiqueWhite
            .Dock = System.Windows.Forms.DockStyle.Fill
            .BorderStyle = BorderStyle.Fixed3D
            .TabIndex = 0
            SetOrderGrid()
            SetpanelFormBtns()
            SetHouseACCustomerControls()
            .Controls.AddRange(New System.Windows.Forms.Control() _
            {Me.fgorder, Me.chkLbo, Me.lblCustomers, Me.cboEatInCustomers, Me.panelFormBtns})
        End With
    End Sub

    Private Sub SetpanelFormBtns()
        panelFormBtns = New Panel()
        With Me.panelFormBtns
            .Anchor = AnchorStyles.Right
            .Width = CInt(panelBotRight.Width / 3)
            .Dock = System.Windows.Forms.DockStyle.Right
            .BorderStyle = BorderStyle.Fixed3D
            .AutoScroll = False
            .BackColor = Color.AntiqueWhite
            SetFormBtns()
            .Controls.AddRange(New System.Windows.Forms.Control() _
                    {Me.btnKOT, Me.btnHouseCustomer, Me.btnCheck, Me.btnPay, Me.btnReset, Me.btnVoid1, Me.btnVoid, Me.btnClose})
            .TabIndex = 1
        End With
    End Sub

    Private Sub SetFormBtns()
        btnClose = GetAButton(6)
        btnClose.Dock = DockStyle.Bottom
        btnClose.Text = "Close"

        btnKOT = GetAButton(0)
        btnKOT.Location = New Point(btnClose.Location.X, 40)
        btnKOT.Text = "KOT"

        btnHouseCustomer = GetAButton(1)
        btnHouseCustomer.Location = New Point(btnClose.Location.X, btnKOT.Location.Y + btnKOT.Height + 10)
        btnHouseCustomer.Text = "House A/C"

        btnCheck = GetAButton(2)
        btnCheck.Location = New Point(btnClose.Location.X, btnHouseCustomer.Location.Y + btnHouseCustomer.Height + 10)
        btnCheck.Text = "Check"

        btnPay = GetAButton(3)
        btnPay.Location = New Point(btnClose.Location.X, btnCheck.Location.Y + btnCheck.Height + 10)
        btnPay.Text = "Pay"

        btnReset = GetAButton(4)
        btnReset.Location = New Point(btnClose.Location.X, btnPay.Location.Y + btnPay.Height + 10)
        btnReset.Text = "Reset"

        btnVoid = GetAButton(5)
        btnVoid.Location = New Point(btnClose.Location.X, btnReset.Location.Y + btnReset.Height + 10)
        btnVoid.Text = "Void"

        btnVoid1 = GetAButton(6)
        btnVoid1.Location = New Point(btnClose.Location.X, btnReset.Location.Y + btnReset.Height + 10)
        btnVoid1.Text = "Void New"
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

    Private Sub SetPanelHTop()
        panelInfo = New Panel()
        With Me.panelInfo
            .Dock = System.Windows.Forms.DockStyle.Left
            .BorderStyle = BorderStyle.Fixed3D
            .Width = CInt(Me.ClientSize.Width / 4.25)
            .TabStop = False
        End With
        panelSelectOrder = New Panel()
        With Me.panelSelectOrder
            .Dock = System.Windows.Forms.DockStyle.Fill
            .BorderStyle = BorderStyle.Fixed3D
            SetCurrentOrdersTabControl()
            SetOrderFilterControls()
            SetbtnEditOrderFiters()
            .Controls.AddRange(New System.Windows.Forms.Control() _
               {Me.tabCurrentOrders, Me.btnEditOrderFiters})
            .TabIndex = 0
        End With
        ' Set properties of Form's Splitter control for Top panel -divides info & selectorder panels.
        splitterTop = New Splitter()
        With Me.splitterTop
            .Location = New System.Drawing.Point(121, 0)
            .Width = 3
            .BackColor = Color.LightSteelBlue
            .TabStop = False
        End With
        With panelHTop
            .Controls.AddRange(New System.Windows.Forms.Control() _
               {Me.panelSelectOrder, Me.splitterTop, Me.panelInfo})
        End With
    End Sub

    Private Sub SetbtnEditOrderFiters()
        btnEditOrderFiters = New Button()
        With btnEditOrderFiters
            LoadTextOfCurrentOrderFiltersSelection()
            .Anchor = AnchorStyles.Top
            .Height = 22
            .Dock = System.Windows.Forms.DockStyle.Top
            .FlatStyle = FlatStyle.Flat
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .TabStop = False
        End With
    End Sub

    Private Sub SetOrderFilterControls()
        Dim lblOrderStatusFilter As Label
        Dim lblSessionFilter As Label
        Dim lblSaleDatePicker As Label

        'Dim i As Integer
        Dim StatusNames() As String
        Dim StatusName As String

        pnlOrderFilters = New Panel()

        With Me.pnlOrderFilters
            .Dock = System.Windows.Forms.DockStyle.Fill
            .Visible = False
            .Enabled = False
            .BorderStyle = BorderStyle.None
            .BackColor = Color.BlanchedAlmond
        End With

        lblOrderStatusFilter = New Label()
        With lblOrderStatusFilter
            .Size = New System.Drawing.Size(100, 25)
            .Location = New System.Drawing.Point(80, 20)
            .Text = "Payment Status"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = pnlOrderFilters.BackColor
            .TabIndex = 0
        End With

        cboOrderStatusFilter = New ComboBox()
        With cboOrderStatusFilter
            .Size = New System.Drawing.Size(100, 50)
            .TabIndex = 1
            .Location = New System.Drawing.Point(80, 45)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.White
            .DropDownStyle = ComboBoxStyle.DropDownList
            StatusNames = [Enum].GetNames(GetType(Orders.EnumFilter))
            .BeginUpdate()
            For Each StatusName In StatusNames
                .Items.Add(StatusName)
            Next
            .EndUpdate()
            .Visible = True
            .SelectedIndex = 0
        End With

        lblSessionFilter = New Label()
        With lblSessionFilter
            .Size = New System.Drawing.Size(100, 25)
            .Location = New System.Drawing.Point(cboOrderStatusFilter.Width + cboOrderStatusFilter.Location.X + 20, 20)
            .Text = "Lunch/Dinner"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        cboSessionFilter = New ComboBox()
        With cboSessionFilter
            .Size = New System.Drawing.Size(100, 50)
            .Location = New System.Drawing.Point(cboOrderStatusFilter.Width + cboOrderStatusFilter.Location.X + 20, 45)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.White
            .DropDownStyle = ComboBoxStyle.DropDownList
            .TabIndex = 2
            .Visible = True
        End With
        lblSaleDatePicker = New Label()
        With lblSaleDatePicker
            .Size = New System.Drawing.Size(110, 25)
            .Location = New System.Drawing.Point(cboSessionFilter.Width + cboSessionFilter.Location.X + 20, 20)
            .Text = "Date"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
        dteSaleDateFilter = New DateTimePicker()
        With dteSaleDateFilter
            .Size = New System.Drawing.Size(110, 50)
            .Location = New System.Drawing.Point(cboSessionFilter.Width + cboSessionFilter.Location.X + 20, 45)
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.White
            .Format = DateTimePickerFormat.Short
            .TabIndex = 3
        End With
        LoadcboSessionFilter()
        cboSessionFilter.SelectedIndex = SaleDays.CreateSaleDay.ActiveSessions.Indexof(SelectedSDSession.SessionId)
        pnlOrderFilters.Controls.AddRange(New Control() _
                {lblOrderStatusFilter, cboOrderStatusFilter, lblSessionFilter, cboSessionFilter, lblSaleDatePicker, dteSaleDateFilter})
    End Sub

    Private Sub LoadcboSessionFilter()
        Dim objSession As Session

        With cboSessionFilter
            .BeginUpdate()
            For Each objSession In SaleDays.CreateSaleDay.ActiveSessions
                .Items.Add(objSession.SessionName)
            Next
            .EndUpdate()
        End With
    End Sub

    Private Sub SetCurrentOrdersTabControl()
        Dim i As Integer

        tabCurrentOrders = New TabControl()
        With tabCurrentOrders
            .Dock = DockStyle.Fill
            .BackColor = Color.Green
            .ForeColor = Color.DarkBlue
            .HotTrack = True
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Appearance = TabAppearance.FlatButtons
            .TabStop = False
            .ShowToolTips = True
            .Visible = True
            .Enabled = True
            .TabPages.Add(GetPageToCurrentOrdersTab("W"))
            .TabPages.Add(GetPageToCurrentOrdersTab("M"))
            .TabPages.Add(GetPageToCurrentOrdersTab("B"))
            .TabPages.Add(GetPageToCurrentOrdersTab("T"))
            .TabPages.Add(GetPageToCurrentOrdersTab("A"))
            .TabPages.Add(GetPageToCurrentOrdersTab("All Sections"))
            .TabPages.Add(GetPageToCurrentOrdersTab("EatIn New Order"))
            .TabPages.Add(GetPageToCurrentOrdersTab("TakeOut"))
            .TabPages.Add(GetPageToCurrentOrdersTab("TakeOut New Order"))
        End With

        SetSelectOrderEatInGrid()
        tabCurrentOrders.TabPages.Item(0).Controls.Add(fgSelectOrderEatin)
        SetSelectOrderTakeOutGrid()
        SetNewEatinOrderTabPage()
        SetNewTakeOutOrderTabPage()

    End Sub

    Private Function GetPageToCurrentOrdersTab(ByVal strPageName As String) As TabPage
        Dim tabpg As TabPage
        tabpg = New TabPage()
        With tabpg
            .Name = "pg" & strPageName
            .Text = strPageName
        End With
        Return tabpg
    End Function

    Private Sub SetSelectOrderEatInGrid()
        Dim fgCellstyle As C1.Win.C1FlexGrid.CellStyle

        fgSelectOrderEatin = New C1.Win.C1FlexGrid.C1FlexGrid()
        fgCellstyle = fgSelectOrderEatin.Styles.Fixed

        fgCellstyle.Font = New Font(Font, FontStyle.Bold)
        fgCellstyle.ForeColor = Color.DarkBlue
        With fgSelectOrderEatin
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.Both
            .Cols.Count = 12
            .Cols.Fixed = 0
            .Rows.Count = 1
            .Rows.Fixed = 1
            .AllowResizing = C1FlexGrid.AllowResizingEnum.Columns
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowSorting = C1FlexGrid.AllowSortingEnum.None
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols
            .ExtendLastCol = True
            .HighLight = C1FlexGrid.HighLightEnum.Always
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .Styles.Alternate.BackColor = Color.AntiqueWhite
            .Styles.EmptyArea.BackColor = Color.AntiqueWhite
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.None
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 0
        End With

        With fgSelectOrderEatin.Cols(0)
            .Width = 40
            .WidthDisplay = 40
            .Name = "TableName"
            .DataType = GetType(String)
            .AllowMerging = True
            .AllowSorting = False
            .AllowEditing = True
        End With

        With fgSelectOrderEatin.Cols(1)
            .Width = 40
            .WidthDisplay = 40
            .Name = "TableSeating"
            .DataType = GetType(Integer)
            .AllowMerging = True
            .AllowSorting = False
            .AllowEditing = True
        End With

        With fgSelectOrderEatin.Cols(2)
            .Width = 140
            .WidthDisplay = 100
            .Name = "WaiterName"
            .DataType = GetType(String)
            .AllowMerging = True
            .AllowSorting = False
            .AllowEditing = False
        End With

        With fgSelectOrderEatin.Cols(3)
            .Width = 100
            .WidthDisplay = 80
            .Name = "AdultKidNo"
            .DataType = GetType(String)
            .AllowEditing = True
        End With
        With fgSelectOrderEatin.Cols(4)
            .Width = 140
            .WidthDisplay = 60
            .Name = "OrderStarted"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With

        With fgSelectOrderEatin.Cols(5)
            .Width = 140
            .WidthDisplay = 60
            .Name = "KOTAt"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With

        With fgSelectOrderEatin.Cols(6)
            .Width = 140
            .WidthDisplay = 60
            .Name = "LastKOTAt"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With

        With fgSelectOrderEatin.Cols(7)
            .Width = 140
            .WidthDisplay = 60
            .Name = "CheckPrinted"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With

        With fgSelectOrderEatin.Cols(8)
            .Width = 80
            .WidthDisplay = 60
            .Name = "OrderAmount"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowEditing = False
        End With

        With fgSelectOrderEatin.Cols(9)
            .Width = 100
            .WidthDisplay = 80
            .Name = "PaidStatus"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With

        With fgSelectOrderEatin.Cols(10)
            .Width = 80
            .WidthDisplay = 60
            .Name = "TipPaid"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowEditing = False
        End With

        With fgSelectOrderEatin.Cols(11)
            .Width = 140
            .WidthDisplay = 110
            .Name = "PaymentMethod"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With
        SetSelectOrderEatInGridColumnHdr()
    End Sub

    Private Sub SetSelectOrderEatInGridColumnHdr()
        With fgSelectOrderEatin
            .Cols("TableName").Caption = "Table"
            .Cols("TableSeating").Caption = "Seating"
            .Cols("WaiterName").Caption = "Waiter"
            .Cols("AdultKidNo").Caption = "Adult + Kid"
            .Cols("OrderStarted").Caption = "Started"
            .Cols("KOTAt").Caption = "1st KOT"
            .Cols("LastKOTAt").Caption = "Last KOT"
            .Cols("CheckPrinted").Caption = "Chk Printed"
            .Cols("OrderAmount").Caption = "POS $"
            .Cols("PaidStatus").Caption = "Paid"
            .Cols("TipPaid").Caption = "Tip $"
            .Cols("PaymentMethod").Caption = "Paid By"
        End With
    End Sub

    Private Sub SetSelectOrderTakeOutGrid()
        Dim fgCellstyle As C1.Win.C1FlexGrid.CellStyle

        fgSelectOrderTakeOut = New C1.Win.C1FlexGrid.C1FlexGrid()
        fgCellstyle = fgSelectOrderTakeOut.Styles.Fixed

        fgCellstyle.Font = New Font(Font, FontStyle.Bold)
        fgCellstyle.ForeColor = Color.DarkBlue
        With fgSelectOrderTakeOut
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.Vertical
            .Cols.Count = 14
            .Cols.Fixed = 0
            .Rows.Count = 1
            .Rows.Fixed = 1
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .ExtendLastCol = True

            .HighLight = C1FlexGrid.HighLightEnum.Always
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black

            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .Styles.Alternate.BackColor = Color.LightSteelBlue
            .Styles.EmptyArea.BackColor = Color.White

            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.FromTop
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
            .TabIndex = 0
        End With

        With fgSelectOrderTakeOut.Cols(0)
            .Width = 100
            .WidthDisplay = 100
            .Name = "CustomerName"
            .DataType = GetType(String)
            .AllowSorting = False
            .AllowEditing = True
        End With

        With fgSelectOrderTakeOut.Cols(1)
            .Width = 100
            .WidthDisplay = 100
            .Name = "Phone"
            .DataType = GetType(String)
            .AllowSorting = False
        End With

        With fgSelectOrderTakeOut.Cols(2)
            .Width = 140
            .WidthDisplay = 100
            .Name = "WaiterName"
            .DataType = GetType(String)
            .AllowSorting = False
        End With
        With fgSelectOrderTakeOut.Cols(3)
            .Width = 50
            .WidthDisplay = 50
            .Name = "Delivery"
            .DataType = GetType(Boolean)
            .AllowSorting = False
            .AllowEditing = False
        End With
        With fgSelectOrderTakeOut.Cols(4)
            .Width = 140
            .WidthDisplay = 60
            .Name = "OrderStarted"
            .DataType = GetType(String)
            .AllowSorting = False
        End With
        With fgSelectOrderTakeOut.Cols(5)
            .Width = 140
            .WidthDisplay = 60
            .Name = "PromisedAt"
            .DataType = GetType(String)
            .AllowSorting = False
        End With

        With fgSelectOrderTakeOut.Cols(6)
            .Width = 140
            .WidthDisplay = 60
            .Name = "KOTAt"
            .DataType = GetType(String)
            .AllowSorting = False
        End With

        With fgSelectOrderTakeOut.Cols(7)
            .Width = 140
            .WidthDisplay = 60
            .Name = "CheckPrinted"
            .DataType = GetType(String)
            .AllowSorting = False
        End With

        With fgSelectOrderTakeOut.Cols(8)
            .Width = 80
            .WidthDisplay = 60
            .Name = "OrderAmount"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowSorting = False
        End With

        With fgSelectOrderTakeOut.Cols(9)
            .Width = 100
            .WidthDisplay = 80
            .Name = "PaidStatus"
            .DataType = GetType(String)
            .AllowSorting = False
        End With

        With fgSelectOrderTakeOut.Cols(10)
            .Width = 80
            .WidthDisplay = 60
            .Name = "TipPaid"
            .DataType = GetType(Double)
            .Format = "C"
            .AllowSorting = False
        End With
        With fgSelectOrderTakeOut.Cols(11)
            .Width = 140
            .WidthDisplay = 110
            .Name = "PaymentMethod"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With
        With fgSelectOrderTakeOut.Cols(12)
            .Width = 140
            .WidthDisplay = 110
            .Name = "FoodBy"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With
        With fgSelectOrderTakeOut.Cols(13)
            .Width = 140
            .WidthDisplay = 110
            .Name = "OrderReceivedFrom"
            .DataType = GetType(String)
            .AllowEditing = False
            .AllowSorting = False
        End With

        SetSelectOrderTakeOutGridColumnHdr()
    End Sub

    Private Sub SetSelectOrderTakeOutGridColumnHdr()
        With fgSelectOrderTakeOut
            .Cols("CustomerName").Caption = "Customer"
            .Cols("Phone").Caption = "Phone #"
            .Cols("WaiterName").Caption = "Waiter"
            .Cols("Delivery").Caption = "Delivery?"
            .Cols("OrderStarted").Caption = "Order At"
            .Cols("PromisedAt").Caption = "Promised At"
            .Cols("KOTAt").Caption = "KOT At"
            .Cols("CheckPrinted").Caption = "Chk Printed"
            .Cols("OrderAmount").Caption = "POS $"
            .Cols("PaidStatus").Caption = "Paid"
            .Cols("TipPaid").Caption = "Tip $"
            .Cols("PaymentMethod").Caption = "Paid By"
            .Cols("FoodBy").Caption = "Food From"
            .Cols("OrderReceivedFrom").Caption = "Order From"
        End With
    End Sub

    Private Sub SetNewEatinOrderTabPage()
        Dim objTable As DTable
        'Dim objCustomer As Customer
        Dim objWaiter As Employee
        Dim lblTables As System.Windows.Forms.Label
        'Dim lblCustomers As System.Windows.Forms.Label
        Dim lblWaiters As System.Windows.Forms.Label
        Dim lblAdults As System.Windows.Forms.Label
        Dim lblKids As System.Windows.Forms.Label

        Dim lblDate As System.Windows.Forms.Label


        lblWaiters = New Label()
        With lblWaiters
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(45, 15)
            .Text = "Select a Waiter"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        'cbo for waiters combo
        cboEatInWaiters = New ComboBox()
        With cboEatInWaiters
            .Size = New System.Drawing.Size(100, 100)
            .Location = New System.Drawing.Point(lblWaiters.Location.X, lblWaiters.Location.Y + lblWaiters.Height + 5)
            .DropDownStyle = ComboBoxStyle.DropDownList
            .BeginUpdate()
            For Each objWaiter In SaleDays.CreateSaleDay.ActiveCashiers
                .Items.Add(objWaiter.EmployeeFullName)
            Next
            .EndUpdate()
            .Visible = True
            .TabIndex = 0
        End With

        lblAdults = New Label()
        With lblAdults
            .Size = New System.Drawing.Size(45, 15)
            .Location = New System.Drawing.Point(lblWaiters.Location.X, lblWaiters.Location.Y + lblWaiters.Height + 60)
            .Text = " Adults"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        txtAdults = New TextBox()
        With txtAdults
            .Size = New System.Drawing.Size(45, 15)
            .Location = New System.Drawing.Point(lblAdults.Location.X, lblAdults.Location.Y + lblAdults.Height + 15)
            .TextAlign = HorizontalAlignment.Center
            .Visible = True
            .MaxLength = 2
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 1
        End With


        lblKids = New Label()
        With lblKids
            .Size = New System.Drawing.Size(45, 15)
            .Location = New System.Drawing.Point(lblAdults.Location.X + lblAdults.Width + 5, lblAdults.Location.Y)
            .Text = "Kids"
            .Visible = True
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        txtKids = New TextBox()
        With txtKids
            .Size = New System.Drawing.Size(45, 15)
            .Location = New System.Drawing.Point(lblKids.Location.X, lblKids.Location.Y + lblKids.Height + 15)
            .TextAlign = HorizontalAlignment.Center
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 2
        End With

        chkDesiEatin = New CheckBox()
        With chkDesiEatin
            .Size = New System.Drawing.Size(80, 30)
            .Location = New System.Drawing.Point(txtKids.Location.X + txtKids.Width + 10, txtKids.Location.Y)
            .Checked = False
            .Text = "Is Desi"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 3
        End With

        chkHouseACOrderEatIn = New CheckBox()
        With chkHouseACOrderEatIn
            .Size = New System.Drawing.Size(90, 30)
            .Location = New System.Drawing.Point(chkDesiEatin.Location.X + chkDesiEatin.Width + 10, chkDesiEatin.Location.Y)
            .Checked = False
            .Text = "House A/C Order?"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 4
        End With

        lblDate = New Label()
        With lblDate
            .Size = New System.Drawing.Size(136, 15)
            .Location = New System.Drawing.Point(lblWaiters.Location.X + lblWaiters.Width + 60, lblWaiters.Location.Y)
            .Text = " Sale Date"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With
        dteEatInOrderPromisedAt = New DateTimePicker()
        With dteEatInOrderPromisedAt
            .Size = New System.Drawing.Size(136, 15)
            .Location = New System.Drawing.Point(lblDate.Location.X, lblDate.Location.Y + lblDate.Height + 5)
            .CustomFormat = "MMM dd hh:mm tt"
            .Format = DateTimePickerFormat.Custom
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 8
            .MinDate = CDate("01/02/2004")
        End With
        'Lable for Tables
        lblTables = New Label()
        With lblTables
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(lblDate.Location.X + lblDate.Width + 60, lblWaiters.Location.Y)
            .Text = "Select a Table"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        'cbo for tables combo
        cboTables = New ComboBox()
        With cboTables
            .Size = New System.Drawing.Size(100, 100)
            .Location = New System.Drawing.Point(lblTables.Location.X, lblTables.Location.Y + lblTables.Height + 5)
            .DropDownStyle = ComboBoxStyle.DropDownList
            .BeginUpdate()
            For Each objTable In SaleDays.CreateSaleDay.ActiveTables
                .Items.Add(objTable.TableDescription)
            Next
            .EndUpdate()
            .Visible = True
            .TabIndex = 5
        End With
        btnOKEatInNewOrder = New Button()
        With btnOKEatInNewOrder
            .Size = New System.Drawing.Size(75, 60)
            .Location = New System.Drawing.Point(cboTables.Location.X, txtKids.Location.Y + txtKids.Height - btnOKEatInNewOrder.Height)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Text = "Start New Order"
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 6
        End With
        btnCancelEatInNewOrder = New Button()
        With btnCancelEatInNewOrder
            .Size = New System.Drawing.Size(75, 60)
            .Location = New System.Drawing.Point(btnOKEatInNewOrder.Location.X + btnOKEatInNewOrder.Width + 10, btnOKEatInNewOrder.Location.Y)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Text = "Cancel"
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 7
        End With
        With tabCurrentOrders.TabPages.Item(6)

            .Controls.AddRange(New Control() _
                {lblTables, cboTables, lblWaiters, cboEatInWaiters, chkHouseACOrderEatIn, chkDesiEatin, _
                lblAdults, txtAdults, lblKids, txtKids, lblDate, dteEatInOrderPromisedAt, btnOKEatInNewOrder, _
                btnCancelEatInNewOrder})
        End With
        LoadEatInNewOrderDefaults()
    End Sub

    Private Sub SetNewTakeOutOrderTabPage()
        'Dim objCustomer As Customer
        Dim objWaiter As Employee
        Dim lblCustomers As System.Windows.Forms.Label
        Dim lblWaiters As System.Windows.Forms.Label
        Dim lblPhoneNo As System.Windows.Forms.Label
        Dim lblFoodProvidedBy As System.Windows.Forms.Label
        Dim lblOrderReceivedFrom As System.Windows.Forms.Label
        Dim lblPromisedTime As System.Windows.Forms.Label
        Dim lblAddress As System.Windows.Forms.Label
        Dim lblDirections As System.Windows.Forms.Label

        lblWaiters = New Label()
        With lblWaiters
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(45, 15)
            .Text = "Select a Waiter"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        'cbo for waiters combo
        cboTakeOutWaiters = New ComboBox()
        With cboTakeOutWaiters
            .Size = New System.Drawing.Size(100, 100)
            .Location = New System.Drawing.Point(lblWaiters.Location.X, lblWaiters.Location.Y + lblWaiters.Height + 5)
            .DropDownStyle = ComboBoxStyle.DropDownList
            .BeginUpdate()
            For Each objWaiter In SaleDays.CreateSaleDay.ActiveCashiers
                .Items.Add(objWaiter.EmployeeFullName)
            Next
            .EndUpdate()
            .Visible = True
            .TabIndex = 0
        End With

        lblCustomers = New Label()
        With lblCustomers
            .Size = New System.Drawing.Size(116, 15)
            .Location = New System.Drawing.Point(cboTakeOutWaiters.Location.X, cboTakeOutWaiters.Location.Y + 35)
            .Text = "Customer Name"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        txtTakeOutCustomer = New TextBox()
        With txtTakeOutCustomer
            .Size = New System.Drawing.Size(160, 100)
            .Location = New System.Drawing.Point(lblCustomers.Location.X, lblCustomers.Location.Y + lblCustomers.Height + 5)
            '.DropDownStyle = ComboBoxStyle.DropDown
            '.BeginUpdate()
            'For Each objCustomer In SaleDays.CreateSaleDay.ActiveCustomers
            '    .Items.Add(objCustomer.CustomerName)
            'Next
            '.EndUpdate()
            .Visible = True
            .TabIndex = 1
        End With

        chkDesi = New CheckBox()
        With chkDesi
            .Size = New System.Drawing.Size(80, 30)
            .Location = New System.Drawing.Point(txtTakeOutCustomer.Location.X + txtTakeOutCustomer.Width + 10, txtTakeOutCustomer.Location.Y)
            .Checked = False
            .Text = "Is Desi"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 2
        End With

        chkHouseACOrderTakeOut = New CheckBox()
        With chkHouseACOrderTakeOut
            .Size = New System.Drawing.Size(90, 30)
            .Location = New System.Drawing.Point(chkDesi.Location.X + chkDesi.Width + 10, chkDesi.Location.Y)
            .Checked = False
            .Text = "House A/C Order?"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 3
        End With

        lblPhoneNo = New Label()
        With lblPhoneNo
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(txtTakeOutCustomer.Location.X, txtTakeOutCustomer.Location.Y + 60)
            .Text = "Phone No"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        cboPhoneNo = New ComboBox()
        With cboPhoneNo
            .Size = New System.Drawing.Size(120, 30)
            .MaxLength = 12
            .Location = New System.Drawing.Point(lblPhoneNo.Location.X, lblPhoneNo.Location.Y + lblPhoneNo.Height + 5)
            .DropDownStyle = ComboBoxStyle.DropDown
            .BeginUpdate()
            .Items.Add("609-921-")
            .Items.Add("609-924-")
            .Items.Add("609-258-")
            .Items.Add("609-252-")
            .Items.Add("609-986-")
            .Items.Add("609-734-")
            .EndUpdate()
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 4
        End With

        lblFoodProvidedBy = New Label()
        With lblFoodProvidedBy
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(lblPhoneNo.Location.X + lblPhoneNo.Width + 100, lblPhoneNo.Location.Y)
            .Text = "Food From"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        cboFoodProvidedBy = New ComboBox()
        With cboFoodProvidedBy
            .Size = New System.Drawing.Size(120, 30)
            .MaxLength = 12
            .Location = New System.Drawing.Point(lblFoodProvidedBy.Location.X, lblFoodProvidedBy.Location.Y + lblFoodProvidedBy.Height + 5)
            .DropDownStyle = ComboBoxStyle.DropDown
            .BeginUpdate()
            .Items.Add("Crown")
            .Items.Add("Mehek")
            .EndUpdate()
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 4
        End With
        lblOrderReceivedFrom = New Label()
        With lblOrderReceivedFrom
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(lblFoodProvidedBy.Location.X + lblFoodProvidedBy.Width + 100, lblFoodProvidedBy.Location.Y)
            .Text = "Order From"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        cboOrderReceivedFrom = New ComboBox()
        With cboOrderReceivedFrom
            .Size = New System.Drawing.Size(120, 30)
            .MaxLength = 12
            .Location = New System.Drawing.Point(lblOrderReceivedFrom.Location.X, lblOrderReceivedFrom.Location.Y + lblOrderReceivedFrom.Height + 5)
            .DropDownStyle = ComboBoxStyle.DropDown
            .BeginUpdate()
            .Items.Add("BeyondMenu")
            .Items.Add("Eat24")
            .Items.Add("EatStreet")
            .Items.Add("GrubHub")
            .Items.Add("Phone-MG")
            .Items.Add("Email-MG")
            .EndUpdate()
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 4
        End With

        lblPromisedTime = New Label()
        With lblPromisedTime
            .Size = New System.Drawing.Size(100, 15)
            .Location = New System.Drawing.Point(lblWaiters.Location.X + lblWaiters.Width + 100, lblWaiters.Location.Y)
            .Text = "Time Promised"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        dteTakeOutOrderPromisedAt = New DateTimePicker()
        With dteTakeOutOrderPromisedAt
            .Size = New System.Drawing.Size(136, 15)
            .Location = New System.Drawing.Point(lblPromisedTime.Location.X, lblPromisedTime.Location.Y + lblPromisedTime.Height + 5)
            .CustomFormat = "MMM dd hh:mm tt"
            .Format = DateTimePickerFormat.Custom
            .Enabled = True
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 8
            .MinDate = CDate("01/02/2004")
        End With

        chkDelivery = New CheckBox()
        With chkDelivery
            .Size = New System.Drawing.Size(90, 20)
            .Location = New System.Drawing.Point(dteTakeOutOrderPromisedAt.Location.X + dteTakeOutOrderPromisedAt.Width + 60, lblPromisedTime.Location.Y)
            .Checked = False
            .Text = "Delivery"
            .TextAlign = ContentAlignment.MiddleLeft
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 5
        End With

        lblAddress = New Label()
        With lblAddress
            .Size = New System.Drawing.Size(140, 15)
            .Location = New System.Drawing.Point(chkDelivery.Location.X, chkDelivery.Location.Y + chkDelivery.Height + 5)
            .Text = "Address"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        End With

        txtAddress = New TextBox()
        With txtAddress
            .Size = New System.Drawing.Size(140, 80)
            .Location = New System.Drawing.Point(lblAddress.Location.X, lblAddress.Location.Y + lblAddress.Height + 5)
            .Multiline = True
            .TextAlign = HorizontalAlignment.Center
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 9
        End With


        'lblDirections = New Label()
        'With lblDirections
        '    .Size = New System.Drawing.Size(100, 15)
        '    .Location = New System.Drawing.Point(lblAddress.Location.X + lblAddress.Width + 20, lblAddress.Location.Y)
        '    .Text = "Directions"
        '    .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        'End With

        'txtDirections = New TextBox()
        'With txtDirections
        '    .Size = New System.Drawing.Size(140, 80)
        '    .Location = New System.Drawing.Point(lblDirections.Location.X, lblDirections.Location.Y + lblDirections.Height + 5)
        '    .Multiline = True
        '    .TextAlign = HorizontalAlignment.Center
        '    .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        '    .TabIndex = 10
        'End With

        btnOKTakeOutNewOrder = New Button()
        With btnOKTakeOutNewOrder
            .Size = New System.Drawing.Size(75, 60)
            .Location = New System.Drawing.Point(txtAddress.Location.X + txtAddress.Width + 15, cboPhoneNo.Location.Y - 40)
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .FlatStyle = FlatStyle.Flat
            .Text = " New Take Out"
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 6
        End With

        btnCancelTakeOutNewOrder = New Button()
        With btnCancelTakeOutNewOrder
            .Size = New System.Drawing.Size(75, 60)
            .Location = New System.Drawing.Point(btnOKTakeOutNewOrder.Location.X + btnOKTakeOutNewOrder.Width + 10, btnOKTakeOutNewOrder.Location.Y)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Text = "Cancel"
            .TextAlign = ContentAlignment.MiddleCenter
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TabIndex = 7
        End With

        tabCurrentOrders.TabPages.Item(8).Controls.AddRange(New Control() _
       {lblWaiters, cboTakeOutWaiters, lblCustomers, txtTakeOutCustomer, _
       lblPhoneNo, cboPhoneNo, lblFoodProvidedBy, cboFoodProvidedBy, lblOrderReceivedFrom, cboOrderReceivedFrom, lblPromisedTime, dteTakeOutOrderPromisedAt, chkDelivery, _
       chkDesi, chkHouseACOrderTakeOut, lblAddress, txtAddress, lblDirections, btnOKTakeOutNewOrder, _
       btnCancelTakeOutNewOrder})

    End Sub

    Private Sub SetMenuCategoryTab()
        'Dim fgCol As C1.Win.C1FlexGrid.Column
        Dim objProductCategory As ProductCategory
        Dim tabpg As TabPage
        'Dim intfgCol As Integer

        tabMenu = New TabControl()
        With tabMenu
            .Appearance = TabAppearance.Buttons
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Blue
            .HotTrack = True
            .Alignment = TabAlignment.Top
            .Multiline = True
            .Dock = DockStyle.Fill
            .Location = New Point(0, 0)
            .TabIndex = 0
        End With

        Dim m_ActiveProductCategories As ProductCategories

        m_ActiveProductCategories = SaleDays.CreateSaleDay.ActiveProductCategories
        For Each objProductCategory In SaleDays.CreateSaleDay.ActiveProductCategories
            tabpg = New TabPage()
            With tabpg
                .Name = "tabpg" & objProductCategory.ProductCategoryID
                .Text = objProductCategory.ProductCategoryName
                .ForeColor = Color.DarkBlue
            End With
            tabMenu.TabPages.Add(tabpg)
        Next

        SetMenuItemGrid()
        tabMenu.TabPages(0).Controls.Add(fgMenuItems)

        CurrentProductCategory = SaleDays.CreateSaleDay.ActiveProductCategories.Item(0)

        LoadMenuItemsGrid()
    End Sub

    Private Sub SetMenuItemGrid()
        fgMenuItems = New C1.Win.C1FlexGrid.C1FlexGrid()

        With fgMenuItems
            .SuspendLayout()
            .Size = New Size(290, 300)
            .Location = New Point(0, 0)
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.Vertical
            .Cols.Count = 3
            .Cols.Fixed = 0
            .Rows.Count = 25
            .Rows.Fixed = 0
            .HighLight = C1FlexGrid.HighLightEnum.WithFocus
            .Styles.Highlight.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Highlight.BackColor = Color.YellowGreen
            .Styles.Highlight.ForeColor = Color.White

            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = False
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.None
            .AutoSearchDelay = 20
            .ShowButtons = C1FlexGrid.ShowButtonsEnum.WithFocus
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
            .KeyActionTab = C1FlexGrid.KeyActionEnum.None
            .Styles.Alternate.BackColor = Color.LightYellow
            .Styles.EmptyArea.BackColor = Color.AntiqueWhite
            .TabIndex = 0
            With .Cols(0)
                .Width = 40
                .WidthDisplay = 40
                .AllowEditing = False
            End With
            With .Cols(1)
                .Width = 180
                .WidthDisplay = 180
                .AllowEditing = False
            End With

            With .Cols(2)
                .Width = 60
                .WidthDisplay = 60
                .AllowEditing = True
                .ComboList = "..."
            End With
            .Font = New Font(fgMenuItems.Font.FontFamily, 10.25, FontStyle.Regular)
            .Enabled = False
            .ResumeLayout()
        End With
    End Sub

    Private Sub SetpanelOrderTotals()
        panelOrderTotals = New Panel()
        With Me.panelOrderTotals
            .Anchor = AnchorStyles.Bottom
            .Height = 125
            .Dock = System.Windows.Forms.DockStyle.Bottom
            .BorderStyle = BorderStyle.Fixed3D
            .AutoScroll = False
            SetOrderTotalsGrid()
            SetOrderTotalCbos()
            .Controls.AddRange(New System.Windows.Forms.Control() _
            {cboTips, cboDiscount, cboTax, fgOrderTotals})
            .TabStop = False
        End With
    End Sub

    Private Sub SetOrderGrid()
        fgorder = New C1.Win.C1FlexGrid.C1FlexGrid()
        With fgorder
            .Anchor = AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom Or AnchorStyles.Top
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.Vertical
            .Cols.Count = 13
            .Cols.Fixed = 0
            .Rows.Count = 2
            .Rows.Fixed = 2
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
            .Styles.EmptyArea.BackColor = Color.AntiqueWhite
            .Styles.Alternate.BackColor = Color.AntiqueWhite
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowMerging = C1FlexGrid.AllowMergingEnum.FixedOnly
            .Rows(0).AllowMerging = True
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.None
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = Nothing
            .Font = New Font(fgMenuItems.Font.FontFamily, 10.25, FontStyle.Regular)
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.Always
            .TabIndex = 0
        End With
        SetOrderGridStyles()
        With fgorder.Cols(0)
            .Width = 40
            .WidthDisplay = 40
            fgorder(1, 0) = "#"
            .Name = "OrderItemSeqNum"
            .AllowEditing = False
            .DataType = GetType(Integer)

        End With
        With fgorder.Cols(1)
            .Width = 200
            .WidthDisplay = 200
            fgorder(1, 1) = "Item"
            .Name = "OrderItemMenuItemName"
            '.AllowEditing = True

            .DataType = GetType(String)
        End With

        With fgorder.Cols(2)
            .Width = 50
            .WidthDisplay = 50
            fgorder(1, 2) = "Course"
            .Name = "Course"
            .DataType = GetType(String)
            .ComboList = "1st. Course|2nd. Course|3rd. Course|4th. Course|Straight Fire|Take Out|Up Front"
        End With

        With fgorder.Cols(3)
            .Width = 35
            .WidthDisplay = 35
            fgorder(1, 3) = "  Qty"
            .Name = "Quantity"
            .DataType = GetType(Integer)
        End With

        With fgorder.Cols(4)
            .Width = 80
            .WidthDisplay = 60
            fgorder(1, 4) = "Spicy"
            .Name = "Mod"
            .DataType = GetType(String)
            .ComboList = "MILD|REG|MED|AMERICAN|DESI"
        End With

        With fgorder.Cols(5)
            .Width = 300
            .WidthDisplay = 180
            fgorder(1, 5) = "Note to Chef"
            .Name = "ModNote"
            .DataType = GetType(String)
            .ComboList = "..."
            .AllowEditing = True
        End With
        SetModNoteCheckedListBox()

        With fgorder.Cols(6)
            .Width = 60
            .WidthDisplay = 60
            fgorder(1, 6) = "       Price"
            .Name = "Price"
            '.AllowEditing = true
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgorder.Cols(7)
            .Width = 70
            .WidthDisplay = 70
            fgorder(1, 7) = "  $ Amount"
            .Name = "OrderItemAmount"
            .AllowEditing = False
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgorder.Cols(8)
            .Width = 40
            .WidthDisplay = 40
            fgorder(1, 8) = "KOT?"
            .Name = "KOTPrinted"
            .AllowEditing = False
            .DataType = GetType(Boolean)
        End With

        With fgorder.Cols(9)
            .Width = 40
            .WidthDisplay = 40
            fgorder(1, 9) = "Chk?"
            .Name = "CheckPrinted"
            .AllowEditing = False
            .DataType = GetType(Boolean)
        End With

        'With fgorder.Cols(10)
        '    .Width = 40
        '    .WidthDisplay = 40
        '    fgorder(1, 10) = "Voided?"
        '    .Name = "Voided"
        '    .AllowEditing = False
        '    .DataType = GetType(Boolean)
        'End With

        With fgorder.Cols(10)
            .Width = 40
            .WidthDisplay = 40
            fgorder(1, 10) = "Check #"
            .Name = "Check"
            '.AllowEditing = true
            .DataType = GetType(Integer)
        End With
        With fgorder.Cols(11)
            .Width = 200
            .WidthDisplay = 300
            fgorder(1, 11) = "Void Reason"
            .Name = "VoidReason"
            .AllowEditing = True
            .DataType = GetType(String)
            .ComboList = "WrongEntry|CustomerComplaintTaste|CustomerCompaintSpiciness|LateDelivery|MissingFromDelivery"
        End With
        With fgorder.Cols(12)
            .Width = 80
            .WidthDisplay = 80
            fgorder(1, 12) = "Food From"
            .Name = "FoodFrom"
            .AllowEditing = True
            .DataType = GetType(String)
            .ComboList = "Crown|MG"
        End With
        LoadOrderGridHeader()
    End Sub
    'Private Sub RowVoidBtn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RowVoidBtn.Click
    '    Dim test As Object = sender
    'End Sub

    Private Sub SetOrderTotalsGrid()

        fgOrderTotals = New C1.Win.C1FlexGrid.C1FlexGrid()

        With fgOrderTotals
            .Dock = DockStyle.Fill
            .ScrollBars = ScrollBars.None
            .Cols.Count = 4
            .Cols.Fixed = 1
            .Rows.Count = 5
            .Rows.Fixed = 0
            .ExtendLastCol = True
            .Styles.Fixed.BackColor = fgorder.Styles.Item("Footer").BackColor
            .Styles.Fixed.ForeColor = Color.DarkBlue
            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.BackColor = fgorder.Styles.Item("Footer").BackColor
            .Styles.Normal.ForeColor = Color.DarkBlue
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .Styles.Focus.Border.Style = C1FlexGrid.BorderStyleEnum.Double
            .Styles.Focus.BackColor = Color.YellowGreen
            .Styles.Focus.ForeColor = Color.Black
            .HighLight = C1FlexGrid.HighLightEnum.Never
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowSorting = C1FlexGrid.AllowSortingEnum.None
            .AllowMerging = C1FlexGrid.AllowMergingEnum.None
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
            .KeyActionTab = C1FlexGrid.KeyActionEnum.MoveAcrossOut
            .Font = New Font(fgMenuItems.Font.FontFamily, 10.25, FontStyle.Regular)
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
        End With

        With fgOrderTotals.Cols(0)
            .Width = 40
            .WidthDisplay = 40
            .Name = "SeqNum"
            .AllowEditing = False
        End With
        With fgOrderTotals.Cols(1)
            .Width = 585
            .WidthDisplay = 585
            .Name = "SummaryItem"
            .AllowEditing = True
            .TextAlign = C1FlexGrid.TextAlignEnum.RightCenter
            .DataType = GetType(String)
            .ComboList = "..."
        End With

        With fgOrderTotals.Cols(2)
            .Width = 90
            .WidthDisplay = 90
            .Name = "SummaryAmount"
            .AllowEditing = False
            .DataType = GetType(Double)
            .Format = "C"
        End With

        With fgOrderTotals.Cols(3)
            .Width = 40
            .WidthDisplay = 40
            .Name = "Dummy"
            .AllowEditing = False
        End With
        fgOrderTotals(0, 1) = "Sub Total Base Sale"
        fgOrderTotals(4, 1) = "Total Pay"

        LoadOrderTotalsGrid()
    End Sub

    Private Sub LoadOrderTotalsGrid()
        fgOrderTotals.SuspendLayout()

        If CurrentOrder Is Nothing Then
            'Summary Item Descriptions in col1 when Current Order does not exist
            If cboDiscount Is Nothing Then
                fgOrderTotals(1, 1) = "Less:Discount @ 0 %"
            Else
                With cboDiscount
                    fgOrderTotals(1, 1) = .GetItemText(.Items(.SelectedIndex))
                End With
            End If
            If cboTax Is Nothing Then
                fgOrderTotals(2, 1) = "Add: NJ Sales Tax @ 7%"
            Else
                With cboTax
                    fgOrderTotals(2, 1) = .GetItemText(.Items(.SelectedIndex))
                End With
            End If
            'fgOrderTotals(2, 1) = "Add: NJ Sales Tax @ 7%"

            If cboTips Is Nothing Then
                fgOrderTotals(3, 1) = "Less:Discount @ 0 %"
            Else
                With cboTips
                    fgOrderTotals(3, 1) = .GetItemText(.Items(.SelectedIndex))
                End With
            End If

            'Summary Item Data in col2 when Current Order does not exist
            fgOrderTotals(0, 2) = 0
            fgOrderTotals(1, 2) = 0
            fgOrderTotals(2, 2) = 0
            fgOrderTotals(3, 2) = 0
            fgOrderTotals(4, 2) = 0
        Else
            With CurrentOrder
                'Summary Item Descriptions in col1 when Current Order exists
                Select Case .DiscountType
                    Case Order.enumMGCoupon.NoDiscount
                        fgOrderTotals(1, 1) = "Less:NO Discount"
                    Case Order.enumMGCoupon.HappyCurryHourDiscount
                        fgOrderTotals(1, 1) = "Less:Happy Curry Hour Discount @ 10 %"
                    Case Order.enumMGCoupon.SpecialDiscount1
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 15 %"
                    Case Order.enumMGCoupon.SpecialDiscount2
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 20 %"
                    Case Order.enumMGCoupon.SpecialDiscount3
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 25 %"
                    Case Order.enumMGCoupon.SpecialDiscount4
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 30 %"
                    Case Order.enumMGCoupon.SpecialDiscount5
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 35 %"
                    Case Order.enumMGCoupon.SpecialDiscount6
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 40 %"
                    Case Order.enumMGCoupon.SpecialDiscount7
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 45 %"
                    Case Order.enumMGCoupon.SpecialDiscount8
                        fgOrderTotals(1, 1) = "Less:Special Discount @ 50 %"
                    Case Else
                        fgOrderTotals(1, 1) = "Less:NO Discount"
                End Select
                Select Case .SalesTaxStatus
                    Case Order.enumOrderSalesTaxStatus.Taxable
                        fgOrderTotals(2, 1) = "Add:7% NJ Sales Tax"
                    Case Else
                        fgOrderTotals(2, 1) = "Sales Tax Exempt Customer"
                End Select
                Select Case .TipType
                    Case Order.enumTips.TipAt18PercentAddedFor6OrMore
                        fgOrderTotals(3, 1) = "Add:18 % Standard Tip For Table of 6 or More"
                    Case Order.enumTips.TipAt15PercentAddedFor6OrMore
                        fgOrderTotals(3, 1) = "Add:15 % Standard Tip For Table of 6 or More"
                    Case Else
                        fgOrderTotals(3, 1) = "Add:No Tip Added"
                End Select
                'Summary Item Data in col2 when Current Order exists
                fgOrderTotals(0, 2) = .OrderBaseAmount
                fgOrderTotals(1, 2) = .DiscountAmount
                fgOrderTotals(2, 2) = .OrderSalesTaxAmount
                fgOrderTotals(3, 2) = .TipAmount
                fgOrderTotals(4, 2) = .PayAmount
            End With
            'LoadSelectedOrdersGrid()
        End If
        fgOrderTotals.ResumeLayout()

    End Sub

    Private Sub fgOrderTotals_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) _
                Handles fgOrderTotals.CellButtonClick
        If (CurrentOrder Is Nothing) OrElse (CurrentOrder.AllowedToAddTipsOrDiscount = False) Then
            'nop
        Else
            Select Case e.Row
                Case 1
                    ShowcboDiscount()
                Case 2
                    ShowcboTax()
                Case 3
                    ShowcboTips()
                Case Else
                    'nop
            End Select
        End If
    End Sub

    Private Sub SetHouseACCustomerControls()
        Dim objCustomer As Customer

        lblCustomers = New Label()
        With lblCustomers
            .Size = New System.Drawing.Size(180, 15)
            .Location = New System.Drawing.Point(panelFormBtns.Location.X - lblCustomers.Width, 100)
            .Text = "Select a House A/C Customer"
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.LightYellow
            .Visible = False
        End With

        cboEatInCustomers = New ComboBox()
        With cboEatInCustomers
            .Size = New System.Drawing.Size(180, 100)
            .Location = New System.Drawing.Point(lblCustomers.Location.X, lblCustomers.Location.Y + lblCustomers.Height + 5)
            .BackColor = Color.LightYellow
            .DropDownStyle = ComboBoxStyle.Simple
            .BeginUpdate()
            For Each objCustomer In SaleDays.CreateSaleDay.ActiveCustomers
                .Items.Add(objCustomer.CustomerName)
            Next
            .EndUpdate()
            .Visible = False
            .TabIndex = 1
        End With
    End Sub

    Private Sub SetModNoteCheckedListBox()
        chkLbo = New CheckedListBox()
        With chkLbo
            .Size = New System.Drawing.Size(fgorder.Cols.Item("ModNote").Width, 100)
            .BackColor = fgorder.BackColor
            .ForeColor = Color.Blue
            .CheckOnClick = True
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BeginUpdate()
            .Items.Add("Salt:NO")
            .Items.Add("Oily:NO")
            .Items.Add("Oily:LESS")
            .Items.Add("Vegan")
            .Items.Add("Use Coconut Milk, No cream")
            .Items.Add("RUSH")
            .Items.Add("Tofu:NO")
            .Items.Add("Tofu:ADD")
            .Items.Add("Onion:NO")
            .Items.Add("Onion-Garlic:NO")
            .Items.Add("Ginger:NO")
            .Items.Add("Cilantro:NO")
            .Items.Add("Nuts:NO")
            .Items.Add("Peppers:NO")
            .Items.Add("CK:White Only")
            .EndUpdate()
            .TabIndex = 0
            .SelectedIndex = -1
            .Visible = False
        End With

    End Sub

    Private Sub SetOrderTotalCbos()
        cboDiscount = New ComboBox()
        With cboDiscount
            .Size = New System.Drawing.Size(300, 100)
            .DropDownStyle = ComboBoxStyle.Simple
            .BackColor = Color.White
            .ForeColor = Color.Blue
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BeginUpdate()
            .Items.Add("Less:Discount @ 0 %")
            .Items.Add("Less:10 % Happy Curry Hour Discount")
            .Items.Add("Less:15 % Discount")
            .Items.Add("Less:20 % Discount")
            .Items.Add("Less:25 % Discount")
            .Items.Add("Less:30 % Discount")
            .Items.Add("Less:35 % Discount")
            .Items.Add("Less:40 % Discount")
            .Items.Add("Less:45 % Discount")
            .Items.Add("Less:50 % Discount")
            .EndUpdate()
            .TabIndex = 0
            .SelectedIndex = 0
            .Visible = False
        End With

        cboTax = New ComboBox()
        With cboTax
            .Size = New System.Drawing.Size(300, 100)
            .DropDownStyle = ComboBoxStyle.DropDownList
            .BackColor = Color.White
            .ForeColor = Color.Blue
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BeginUpdate()
            .Items.Add("Add: NJ Sales Tax @ 7%")
            .Items.Add("Add:No NJ Sales Tax Tax Exempt Customer")
            .Items.Add("Add:No NJ Sales Tax Out Of NJ Customer")
            .EndUpdate()
            .Visible = False
            .TabIndex = 0
            .SelectedIndex = 0
        End With

        cboTips = New ComboBox()
        With cboTips
            .Size = New System.Drawing.Size(300, 100)
            .DropDownStyle = ComboBoxStyle.DropDownList
            .BackColor = Color.White
            .ForeColor = Color.Blue
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BeginUpdate()
            .Items.Add("Add:No Tip Added")
            .Items.Add("Add:18 % Standard Tip For Table of 6 or More")
            .Items.Add("Add:20 % Standard Tip For Table of 6 or More")
            .Items.Add("Add:15 % Standard Tip For Table of 6 or More")
            .EndUpdate()
            .Visible = False
            .TabIndex = 0
            .SelectedIndex = 0
        End With
    End Sub

    Private Sub ShowcboDiscount()
        If cboDiscount Is Nothing Then
            'nop
        Else
            With cboDiscount
                .Location = New Point(fgOrderTotals.Cols.Item(1).Right - cboDiscount.Width, fgOrderTotals.Rows.Item(1).Top)
                .Visible = True
            End With
        End If
    End Sub

    Private Sub ShowcboTax()
        If cboTax Is Nothing Then
            'nop
        Else
            With cboTax
                .Location = New Point(fgOrderTotals.Cols.Item(1).Right - cboTax.Width, fgOrderTotals.Rows.Item(2).Top)
                .Visible = True
            End With
        End If
    End Sub

    Private Sub ShowcboTips()
        If cboTips Is Nothing Then
            'nop
        Else
            With cboTips
                .Location = New Point(fgOrderTotals.Cols.Item(1).Right - cboTips.Width, fgOrderTotals.Rows.Item(3).Top)
                .Visible = True
            End With
        End If
    End Sub

    Private Sub SetOrderGridStyles()
        Dim fgCellstyle As C1.Win.C1FlexGrid.CellStyle
        Dim cs As C1.Win.C1FlexGrid.CellStyle

        fgCellstyle = fgorder.Styles.Fixed
        fgCellstyle.Font = New Font(Font, FontStyle.Bold)
        fgCellstyle.ForeColor = Color.DarkBlue

        cs = fgorder.Styles.Add("Header")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.SandyBrown
            .ForeColor = Color.Black
            .TextAlign = C1FlexGrid.TextAlignEnum.GeneralCenter
        End With

        cs = fgorder.Styles.Add("Footer")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Moccasin
            .ForeColor = Color.Black
        End With

        cs = fgorder.Styles.Add("Group")
        With cs
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Yellow
            .ForeColor = Color.Black
        End With

        cs = fgorder.Styles.Add("VoidItem")
        With cs
            .Font = New Font(fgorder.Font, FontStyle.Strikeout)
            .ForeColor = Color.Red
        End With
        fgorder.Styles.Add("GroupVoidItem").MergeWith(cs)
        fgorder.Styles("GroupVoidItem").BackColor = Color.Yellow

        cs = fgorder.Styles.Add("KOTPrinted")
        With cs
            .ForeColor = Color.Gray
        End With
        fgorder.Styles.Add("GroupKOTPrinted").MergeWith(cs)
        fgorder.Styles("GroupKOTPrinted").BackColor = Color.Yellow

        cs = fgorder.Styles.Add("CheckPrinted")
        With cs
            .ForeColor = Color.DarkGray
        End With
        fgorder.Styles.Add("GroupCheckPrinted").MergeWith(cs)
        fgorder.Styles("GroupCheckPrinted").BackColor = Color.Yellow
    End Sub

    Private Sub LoadTextOfCurrentOrderFiltersSelection()
        btnEditOrderFiters.Text = "Show " & cboOrderStatusFilter.GetItemText(cboOrderStatusFilter.SelectedItem) & _
                                    " Orders For " & SelectedSDSession.SessionName & _
                                    " On " & SelectedSD.SaleDate.ToShortDateString
    End Sub

    Private Sub btnEditOrderFiters_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditOrderFiters.Click
        'Dim SelectedSessionId As Session.enumSessionName
        Dim strTag As String
        If btnEditOrderFiters.Tag Is Nothing Then strTag = " " Else strTag = btnEditOrderFiters.Tag.ToString
        Select Case strTag
            Case "Order Fiters Are Visible"     'click after user is done modifying filters
                With panelSelectOrder
                    .Controls.RemoveAt(0)
                    .Controls.RemoveAt(0)
                    .Controls.AddRange(New System.Windows.Forms.Control() _
                                   {Me.tabCurrentOrders, Me.btnEditOrderFiters})
                End With
                tabCurrentOrders.Visible = True
                tabCurrentOrders.Enabled = True
                btnEditOrderFiters.Tag = " "

                LoadTextOfCurrentOrderFiltersSelection()
                CurrentOrderPageSelected()          'To refresh the current orders tab page that would become visible after this click 

            Case Else                           'Click to show order filters panel
                With panelSelectOrder
                    .Controls.RemoveAt(0)
                    .Controls.RemoveAt(0)
                    .Controls.AddRange(New System.Windows.Forms.Control() _
                    {Me.pnlOrderFilters, Me.btnEditOrderFiters})
                End With
                pnlOrderFilters.Visible = True
                pnlOrderFilters.Enabled = True
                btnEditOrderFiters.Tag = "Order Fiters Are Visible"
                btnEditOrderFiters.Text = "Select Orders you want to see and then click here ..."

        End Select
    End Sub

    Private Sub tabCurrentOrders_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabCurrentOrders.Click
        CurrentOrderPageSelected()
    End Sub

    Private Sub tabCurrentOrders_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabCurrentOrders.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                CurrentOrderPageSelected()
                e.Handled = True
            Case Else
                e.Handled = False
        End Select
    End Sub

    Private Sub CurrentOrderPageSelected()
        'Dim objTable As DTable
        'get rid of prior order row selection when tab page changes
        CurrentRowOfExistingOrderGrid = Nothing
        With tabCurrentOrders.SelectedTab
            Select Case .Text
                Case "TakeOut"
                    If fgSelectOrderTakeOut Is Nothing Then SetSelectOrderTakeOutGrid()
                    .Controls.Add(fgSelectOrderTakeOut)
                    fgSelectOrderTakeOut.Visible = True
                    SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.TakeOut
                    LoadCurrentOrderTakeoutGrid()
                    fgSelectOrderTakeOut.Focus()
                Case "EatIn New Order"
                    'SetNewEatinOrderTabPage()
                    EnableStartNewOrderBtn()
                    LoadEatInNewOrderDefaults()
                Case "TakeOut New Order"
                    'SetNewTakeOutOrderTabPage()
                    LoadTakeOutNewOrderDefaults()
                    EnableStartNewOrderBtn()
                Case Else               'All Eat-in pages
                    .Controls.Add(fgSelectOrderEatin)
                    Select Case .Text
                        Case "A"
                            SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.EatInA
                        Case "B"
                            SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.EatInB
                        Case "M"
                            SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.EatInM
                        Case "T"
                            SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.EatInT
                        Case "W"
                            SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.EatInW
                        Case "All Sections"
                            SelectedOrderTypeOrSection = Orders.EnumFilterOrderTypeOrSection.EatInAll
                    End Select
                    LoadCurrentOrderEatinGrid()
                    fgSelectOrderEatin.Focus()
            End Select
        End With
    End Sub

    Private Property TableForNewOrder() As DTable
        Get
            Return m_TableForNewOrder
        End Get
        Set(ByVal Value As DTable)
            m_TableForNewOrder = Value
            If Value Is Nothing Then
                'nop
            Else
                If SelectedOrders.IsTableAvailable(m_TableForNewOrder.TableName) = True Then
                    'nop
                Else
                    m_TableForNewOrder = Nothing
                End If
                EnableStartNewOrderBtn()
            End If
        End Set
    End Property

    'Private Property CustomerForNewOrder() As Customer
    '    Get
    '        Return m_CustomerForNewOrder
    '    End Get
    '    Set(ByVal Value As Customer)
    '        m_CustomerForNewOrder = Value
    '        CurrentOrder.Customer = Value
    '        LoadOrderGridHeader()
    '        LoadOrderTotalsGrid()
    '        'EnableStartNewOrderBtn()
    '    End Set
    'End Property

    Private Property WaiterForNewOrder() As Employee
        Get
            Return m_WaiterForNewOrder
        End Get
        Set(ByVal Value As Employee)
            m_WaiterForNewOrder = Value
            EnableStartNewOrderBtn()
        End Set
    End Property

    Private Sub SetOrderFilters()
        With SelectedOrders
            .FilterOrderStatus = Me.SelectedOrderStatus
            .FilterOrderTypeOrSection = Me.SelectedOrderTypeOrSection
        End With
    End Sub

    Private Sub LoadCurrentOrderEatinGrid()
        Dim objOrder As Order
        Dim objFilteredOrders As Orders
        Dim intRow As Integer
        Dim objorders As Orders

        SetOrderFilters()
        objFilteredOrders = SelectedOrders.FilteredOrders

        With fgSelectOrderEatin
            intRow = .Rows.Fixed
            .SuspendLayout()
            .Rows.Count = objFilteredOrders.Count + .Rows.Fixed
        End With

        For Each objOrder In objFilteredOrders
            fgSelectOrderEatin.Rows.Item(intRow).UserData = objFilteredOrders.IndexOf(objOrder)
            If objOrder Is CurrentOrder Then CurrentRowOfExistingOrderGrid = fgSelectOrderEatin.Rows.Item(intRow)
            RefreshCurrentOrderEatinGrid(intRow, objOrder)
            intRow += 1
        Next
        SortSelectOrderGrids(fgSelectOrderEatin)
        If (CurrentRowOfExistingOrderGrid Is Nothing) OrElse (CurrentRowOfExistingOrderGrid.Index > fgSelectOrderEatin.Rows.Count - 1) Then
            fgSelectOrderEatin.Select(-1, -1)
        Else
            fgSelectOrderEatin.Select(CurrentRowOfExistingOrderGrid.Index, 0, True)
        End If
        fgSelectOrderEatin.ResumeLayout()
    End Sub

    Private Sub RefreshCurrentOrderEatinGrid(ByVal intRow As Integer, ByVal objOrder As Order)
        With objOrder
            fgSelectOrderEatin.Item(intRow, "TableName") = .Table.TableName
            fgSelectOrderEatin(intRow, "TableSeating") = .TableSeating
            fgSelectOrderEatin(intRow, "WaiterName") = .Waiter.EmployeeFullName
            fgSelectOrderEatin(intRow, "AdultKidNo") = .OrderAdultsKidsCount
            If .EnteredAt = DateTime.MinValue Then
                fgSelectOrderEatin(intRow, "OrderStarted") = " "
            Else
                fgSelectOrderEatin(intRow, "OrderStarted") = Format(.EnteredAt, "HH:mm") & " " & _
                                                   Format(.EnteredAt, "MM/dd/yy")
            End If
            If .FirstKOTPrintedAt = DateTime.MinValue Then
                fgSelectOrderEatin(intRow, "KOTAt") = " "
            Else
                fgSelectOrderEatin(intRow, "KOTAt") = Format(.FirstKOTPrintedAt, "HH:mm") & " " & _
                                                   Format(.FirstKOTPrintedAt, "MM/dd/yy")
            End If
            If .LastKOTPrintedAt = DateTime.MinValue Then
                fgSelectOrderEatin(intRow, "LastKOTAt") = " "
            Else
                fgSelectOrderEatin(intRow, "LastKOTAt") = Format(.LastKOTPrintedAt, "HH:mm") & " " & _
                                                   Format(.LastKOTPrintedAt, "MM/dd/yy")
            End If
            If .FirstCheckPrintedAt = DateTime.MinValue Then
                fgSelectOrderEatin(intRow, "CheckPrinted") = " "
            Else
                fgSelectOrderEatin(intRow, "CheckPrinted") = Format(.FirstCheckPrintedAt, "HH:mm") & " " & _
                                                   Format(.FirstCheckPrintedAt, "MM/dd/yy")
            End If
            fgSelectOrderEatin(intRow, "OrderAmount") = .POSAmount
            fgSelectOrderEatin(intRow, "PaidStatus") = .PaidStatus.ToString
            fgSelectOrderEatin(intRow, "TipPaid") = .TipAmountPaid
            fgSelectOrderEatin(intRow, "PaymentMethod") = .PaidBy
        End With
    End Sub

    Private Sub LoadCurrentOrderTakeoutGrid()
        Dim objOrder As Order
        Dim objFilteredOrders As Orders
        Dim intRow As Integer

        SetOrderFilters()
        objFilteredOrders = SelectedOrders.FilteredOrders

        With fgSelectOrderTakeOut
            intRow = .Rows.Fixed
            .SuspendLayout()
            .Rows.Count = objFilteredOrders.Count + .Rows.Fixed
        End With
        'SetSelectOrderTakeOutGridColumnHdr()

        For Each objOrder In objFilteredOrders
            fgSelectOrderTakeOut.Rows.Item(intRow).UserData = objFilteredOrders.IndexOf(objOrder)
            If objOrder Is CurrentOrder Then CurrentRowOfExistingOrderGrid = fgSelectOrderTakeOut.Rows.Item(intRow)
            RefreshCurrentOrderTakeoutGrid(intRow, objOrder)
            intRow += 1
        Next
        SortSelectOrderGrids(fgSelectOrderTakeOut)
        If (CurrentRowOfExistingOrderGrid Is Nothing) OrElse (CurrentRowOfExistingOrderGrid.Index > fgSelectOrderTakeOut.Rows.Count - 1) Then
            fgSelectOrderTakeOut.Select(-1, -1)
        Else
            fgSelectOrderTakeOut.Select(CurrentRowOfExistingOrderGrid.Index, 0, True)
        End If
        fgSelectOrderTakeOut.ResumeLayout()
    End Sub

    Private Sub RefreshCurrentOrderTakeoutGrid(ByVal intRow As Integer, ByVal objOrder As Order)
        With objOrder
            fgSelectOrderTakeOut(intRow, "CustomerName") = .TakeOutCustomerName
            fgSelectOrderTakeOut(intRow, "Phone") = .TakeOutCustomerPhone
            fgSelectOrderTakeOut(intRow, "WaiterName") = .Waiter.EmployeeFullName
            If .OrderType = Order.enumOrderType.Delivery Then
                fgSelectOrderTakeOut(intRow, "Delivery") = True
            Else
                fgSelectOrderTakeOut(intRow, "Delivery") = False
            End If
            If .EnteredAt = DateTime.MinValue Then
                fgSelectOrderTakeOut(intRow, "OrderStarted") = " "
            Else
                fgSelectOrderTakeOut(intRow, "OrderStarted") = Format(.EnteredAt, "HH:mm") & " " & _
                                                   Format(.EnteredAt, "MM/dd/yy")
            End If
            If .PromisedAt = DateTime.MinValue Then
                fgSelectOrderTakeOut(intRow, "PromisedAt") = " "
            Else
                fgSelectOrderTakeOut(intRow, "PromisedAt") = Format(.PromisedAt, "HH:mm") & " " & _
                                                   Format(.PromisedAt, "MM/dd/yy")
            End If
            If .FirstKOTPrintedAt = DateTime.MinValue Then
                fgSelectOrderTakeOut(intRow, "KOTAt") = " "
            Else
                fgSelectOrderTakeOut(intRow, "KOTAt") = Format(.FirstKOTPrintedAt, "HH:mm") & " " & _
                                                    Format(.FirstKOTPrintedAt, "MM/dd/yy")
            End If
            If .FirstCheckPrintedAt = DateTime.MinValue Then
                fgSelectOrderTakeOut(intRow, "CheckPrinted") = " "
            Else
                fgSelectOrderTakeOut(intRow, "CheckPrinted") = Format(.FirstCheckPrintedAt, "HH:mm") & " " & _
                                                    Format(.FirstCheckPrintedAt, "MM/dd/yy")
            End If
            fgSelectOrderTakeOut(intRow, "OrderAmount") = .POSAmount
            fgSelectOrderTakeOut(intRow, "PaidStatus") = .PaidStatus.ToString
            fgSelectOrderTakeOut(intRow, "TipPaid") = .TipAmountPaid
            fgSelectOrderTakeOut(intRow, "PaymentMethod") = .PaidBy
            fgSelectOrderTakeOut(intRow, "FoodBy") = .FoodProvidedBy
            fgSelectOrderTakeOut(intRow, "OrderReceivedFrom") = .OrderReceivedFrom
        End With
    End Sub

    Private Sub txtAdults_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) _
            Handles txtAdults.KeyPress, txtKids.KeyPress
        Dim txtbox As TextBox

        txtbox = CType(sender, TextBox)
        With txtbox
            Select Case CStr(e.KeyChar)
                Case "+", "="
                    Try
                        .Text = CStr(CType(.Text, Integer) + 1)
                    Catch ex As Exception
                        .Text = "0"
                    Finally
                        e.Handled = True
                    End Try
                Case "-"
                    Try
                        If CType(.Text, Integer) > 0 Then
                            .Text = CStr(CType(.Text, Integer) - 1)
                        End If
                    Catch ex As Exception
                        .Text = "0"
                    Finally
                        e.Handled = True
                    End Try
                Case Else
                    Select Case Char.IsDigit(e.KeyChar)
                        Case True
                            e.Handled = False
                        Case Else
                            Select Case (Char.IsLetter(e.KeyChar)) Or (Char.IsPunctuation(e.KeyChar)) Or (Char.IsSeparator(e.KeyChar))
                                Case True
                                    e.Handled = True
                                Case Else
                                    Select Case Char.IsSymbol(e.KeyChar)
                                        Case True
                                            e.Handled = True
                                        Case Else
                                            e.Handled = True
                                    End Select
                            End Select
                    End Select
            End Select
        End With
    End Sub

    Private Sub EnableStartNewOrderBtn()
        Select Case tabCurrentOrders.SelectedTab.Text
            Case "EatIn New Order"
                If (Me.TableForNewOrder Is Nothing) OrElse _
                    (Me.WaiterForNewOrder Is Nothing) OrElse (txtAdults.Text.Length = 0) Then
                    btnOKEatInNewOrder.Enabled = False
                Else
                    btnOKEatInNewOrder.Enabled = True
                End If
            Case "TakeOut New Order"
                If (txtTakeOutCustomer.Text.Length = 0) OrElse _
                    (Me.WaiterForNewOrder Is Nothing) Then _
                    'OrElse (cboPhoneNo.Text.Length = 0) 
                    btnOKTakeOutNewOrder.Enabled = False
                Else
                    btnOKTakeOutNewOrder.Enabled = True
                End If
            Case Else
                btnOKEatInNewOrder.Enabled = False
                btnOKTakeOutNewOrder.Enabled = False
        End Select
    End Sub

    Private Sub cboEatInCustomers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles cboEatInCustomers.SelectedIndexChanged, cboEatInCustomers.TextChanged, _
                     cboTakeOutCustomers.SelectedIndexChanged, cboTakeOutCustomers.TextChanged
        'HouseCustomerSelected()
        'Dim objCustomer As Customer
        'Dim cbo As ComboBox
        'cbo = CType(sender, ComboBox)
        'If (cbo.SelectedIndex < 0) Or _
        '    (cbo.SelectedIndex > SaleDays.CreateSaleDay.ActiveCustomers.Count - 1) Then
        '    Exit Sub
        'Else
        '    objCustomer = SaleDays.CreateSaleDay.ActiveCustomers.Item(cbo.SelectedIndex)
        'End If
        'If objCustomer Is Nothing Then
        '    MsgBox("Invalid Customer, select another one", MsgBoxStyle.OKOnly, "Invalid Customer")
        'Else
        '    'cboPhoneNo.Text = objCustomer.PhoneNoDisplayValue
        'End If
        'Me.CustomerForNewOrder = objCustomer
    End Sub

    Private Sub cboEatInCustomers_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles _
                                cboEatInCustomers.KeyUp, cboTakeOutCustomers.KeyUp
        Dim cbo As ComboBox
        Dim intindex As Integer
        Dim strtest As String

        cbo = CType(sender, ComboBox)
        strtest = CStr(cbo.Tag)
        If Char.IsLetter(Chr(e.KeyCode)) = True Then
            strtest = strtest & Chr(e.KeyCode)
            intindex = cbo.FindString(strtest, -1) ' & e.KeyData.ToString)
            cbo.SelectedIndex = intindex
            If intindex < 0 Then
                cbo.Text = strtest
                cbo.SelectedIndex = -1
            End If
        ElseIf e.KeyCode = (Keys.Back) Then
            If strtest.Length >= 1 Then
                strtest = strtest.Remove(strtest.Length - 1, 1)
            End If
        End If
        If strtest.Length > 0 Then
            cbo.SelectionStart = strtest.Length
        Else
            cbo.SelectionStart = 0
        End If
        cbo.SelectionLength = 0
        cbo.Tag = strtest
    End Sub

    Private Sub cboEatInCustomers_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles _
                    cboEatInCustomers.Leave, cboTakeOutCustomers.Leave
        HouseCustomerSelected()
    End Sub

    Private Sub HouseCustomerSelected()
        Dim objCustomer As Customer

        cboEatInCustomers.Tag = String.Empty

        If (cboEatInCustomers.SelectedIndex < 0) Or _
            (cboEatInCustomers.SelectedIndex > SaleDays.CreateSaleDay.ActiveCustomers.Count - 1) Then
            objCustomer = Nothing
            CurrentOrder.IsHouseACOrder = False
            'With objCustomer
            '    .CustomerName = cbo.Text
            '    '.CustomerPhoneNo = cboPhoneNo.Text
            '    ' .CustomerAddress1 = txtAddress.Text & " "
            '    SaleDays.CreateSaleDay.ActiveCustomers.Add(objCustomer)
            '    '.SetData()
            '    'SaleDays.CreateSaleDay.ActiveCustomers.SaveData()
            'End With
        Else
            objCustomer = SaleDays.CreateSaleDay.ActiveCustomers.Item(cboEatInCustomers.SelectedIndex)
            Select Case objCustomer.CustomerType
                Case Customer.enumCustomerType.CorpAccount, Customer.enumCustomerType.PUAccount
                    CurrentOrder.IsHouseACOrder = True
                Case Else
                    'nop
            End Select
        End If
        'If objCustomer Is Nothing Then
        '    MsgBox("Invalid Customer, select another one", MsgBoxStyle.OKOnly, "Invalid Customer")
        'End If
        CurrentOrder.Customer = objCustomer
        LoadOrderGridHeader()
        LoadOrderTotalsGrid()
        lblCustomers.Visible = False
        cboEatInCustomers.Visible = False
    End Sub

    Private Sub btnHouseCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHouseCustomer.Click
        If CurrentOrder Is Nothing Then
            'nop
        Else
            If CurrentOrder.AllowHouseACEdit = True Then
                With lblCustomers
                    .Visible = True
                    .Location = New System.Drawing.Point(panelFormBtns.Location.X - lblCustomers.Width, 100)
                    .BringToFront()
                End With
                With cboEatInCustomers
                    .Visible = True
                    .SelectedIndex = -1
                    .Location = New System.Drawing.Point(lblCustomers.Location.X, lblCustomers.Location.Y + lblCustomers.Height + 5)
                    .BringToFront()
                    .Focus()
                End With
            End If
        End If
    End Sub

    Private Property CurrentOrder() As Order
        Get
            Return m_CurrentOrder
        End Get
        Set(ByVal Value As Order)
            Dim intResult As Integer
            If m_CurrentOrder Is Nothing Then
                'nop
            Else
                intResult = m_CurrentOrder.SetData(m_selectedSDSession)
            End If

            m_CurrentOrder = Value
            If Value Is Nothing Then
                fgMenuItems.Enabled = False
                tabCurrentOrders.Focus()
            Else
                LoadOrderGrid()
                If m_frmEditPaymentPr Is Nothing Then
                    fgMenuItems.Enabled = True
                    fgMenuItems.Focus()
                End If
            End If
        End Set
    End Property

    Private Sub LoadMenuItemsGrid()
        Dim objProductCategory As ProductCategory
        Dim objMenuItem As MenuItem
        Dim intRow As Integer

        With fgMenuItems
            .SuspendLayout()
            .Rows.Count = .Rows.Fixed
            .Rows.Count = MenuItemsOfCurrentProductCategory.Count
            For Each objMenuItem In MenuItemsOfCurrentProductCategory
                fgMenuItems(intRow, 0) = intRow + 1
                fgMenuItems(intRow, 1) = objMenuItem.ProductNameHindi
                intRow += 1
            Next
            .ResumeLayout()
        End With
    End Sub

    Private Property CurrentProductCategory() As ProductCategory
        Get
            Return m_ProductCategory
        End Get
        Set(ByVal Value As ProductCategory)
            m_ProductCategory = Value
        End Set
    End Property

    Private ReadOnly Property MenuItemsOfCurrentProductCategory() As MenuItems
        Get
            If CurrentProductCategory Is Nothing Then Exit Property
            Return CurrentProductCategory.ActiveFamilyHeadMenuItems
        End Get
    End Property


    Private Sub fgMenuItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgMenuItems.Click
        MenuItemSelected()
    End Sub

    Private Sub tabMenu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tabMenu.Click
        MenuCategorySelected()
    End Sub

    Private Sub fgMenuItems_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles fgMenuItems.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                MenuItemSelected()
                e.Handled = True
            Case Else
                e.Handled = False
        End Select
    End Sub

    Private Sub MenuItemSelected()
        Dim intRow As Integer
        Dim intCol As Integer
        Dim strProductname As String
        Dim objSelectedMenuItem As MenuItem
        Select Case fgMenuItems.ColSel
            Case 0, 1
                intRow = fgMenuItems.RowSel
                If (intRow <= MenuItemsOfCurrentProductCategory.Count) Then
                    objSelectedMenuItem = MenuItemsOfCurrentProductCategory.Item(fgMenuItems.RowSel)
                    AddOrderItem(objSelectedMenuItem)
                End If
            Case 2
                'nop: this col is for product family items only so ignore click when there are no family products
        End Select

    End Sub

    Private Sub MenuCategorySelected()
        tabMenu.SelectedTab = tabMenu.TabPages(tabMenu.SelectedIndex)
        CurrentProductCategory = SaleDays.CreateSaleDay.ActiveProductCategories.Item(tabMenu.SelectedIndex)
        tabMenu.SuspendLayout()
        tabMenu.TabPages(tabMenu.SelectedIndex).Controls.AddRange(New System.Windows.Forms.Control() _
                    {fgMenuItems, cboProductFamily})
        LoadMenuItemsGrid()
        tabMenu.ResumeLayout()
        fgMenuItems.Focus()
    End Sub

    Private Sub tabMenu_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMenu.KeyUp
        Select Case e.KeyCode
            Case Keys.Enter
                MenuCategorySelected()
                e.Handled = True
            Case Else
                e.Handled = False
        End Select
    End Sub

    'Private ReadOnly Property SelectedOrderItem() As OrderItem
    '    Get
    '        Dim intOrderItemIndex As Integer
    '        Dim objSelectedOrderItem As OrderItem

    '        If (CurrentOrder Is Nothing) OrElse (fgorder.RowSel < fgorder.Rows.Fixed) Then
    '            objSelectedOrderItem = Nothing
    '        Else
    '            intOrderItemIndex = CInt(fgorder.Rows(fgorder.RowSel).UserData)
    '            intOrderItemIndex = fgorder.Rows(fgorder.RowSel).Index - 2
    '            If (intOrderItemIndex < CurrentOrder.AllOrderItems.Count) And _
    '                (intOrderItemIndex >= 0) Then
    '                objSelectedOrderItem = CurrentOrder.AllOrderItems.Item(intOrderItemIndex)
    '                If objSelectedOrderItem.Status = OrderItem.enumStatus.Active Then
    '                    'nop
    '                Else
    '                    objSelectedOrderItem = Nothing
    '                End If
    '            Else
    '                objSelectedOrderItem = Nothing
    '            End If
    '        End If
    '        'EnableDisableFormBtns()
    '        Return objSelectedOrderItem
    '    End Get
    'End Property
    Private _selectedOrderItem As OrderItem
    Private ReadOnly Property SelectedOrderItem() As OrderItem
        Get
            Return _selectedOrderItem
        End Get
    End Property
    Private Sub SetSelectedOrderItem(ByVal selRowIndex As Integer)
        Dim intOrderItemIndex As Integer
        Dim intOrderItemIndex1 As Integer
        'Dim objSelectedOrderItem As OrderItem

        If (CurrentOrder Is Nothing) OrElse (fgorder.RowSel < fgorder.Rows.Fixed) Then
            _selectedOrderItem = Nothing
        Else
            intOrderItemIndex = CInt(fgorder.Rows(selRowIndex).UserData)
            intOrderItemIndex1 = CInt(fgorder.Rows(fgorder.RowSel).UserData)
            If (intOrderItemIndex < CurrentOrder.AllOrderItems.Count) And _
                (intOrderItemIndex >= 0) Then
                _selectedOrderItem = CurrentOrder.AllOrderItems.Item(intOrderItemIndex)
                If _selectedOrderItem.Status = OrderItem.enumStatus.Active Then
                    'nop
                Else
                    _selectedOrderItem = Nothing
                End If
            Else
                _selectedOrderItem = Nothing
            End If
        End If
        'EnableDisableFormBtns()
    End Sub


    Private Sub fgorder_AfterRowColChange(sender As Object, e As C1FlexGrid.RangeEventArgs) Handles fgorder.AfterRowColChange

    End Sub

    Private Sub fgorder_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgorder.BeforeEdit
        SetSelectedOrderItem(e.Row)
        If (SelectedOrderItem Is Nothing) OrElse _
            (SelectedOrderItem.Status = OrderItem.enumStatus.Voided) Then
            'Get rid of Overrides post 6/15/2016 : To allow updatng void reason
            'e.Cancel = True
            EnableDisableFormBtns()
        Else
            Select Case fgorder.Cols.Item(e.Col).Name
                Case "Price"
                    Select Case SelectedOrderItem.MenuItem.ProductPricingType
                        Case Product.enumProductPricingType.UserPricedComboItem, _
                                Product.enumProductPricingType.UserPricedPackageItem, _
                                Product.enumProductPricingType.UserPricedSingleItem
                            e.Cancel = False
                        Case Else
                            e.Cancel = True
                    End Select

            End Select

        End If
    End Sub

    Private Sub fgorder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgorder.Click
        'If (SelectedOrderItem Is Nothing) OrElse (SelectedOrderItem.Status = OrderItem.enumStatus.Voided) Then
        SetSelectedOrderItem(fgorder.RowSel)

        EnableDisableFormBtns()
        'End If
    End Sub
    'Private Sub fgorder_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles fgorder.KeyDown
    '    Select Case e.KeyCode
    '        Case Keys.Enter, Keys.Back, Keys.Right, Keys.Left, Keys.Up
    '            EnableDisableFormBtns()
    '        Case Else
    '            'nop
    '    End Select
    'End Sub
    Private Sub fgOrder_AfterEdit(ByVal sender As System.Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgorder.AfterEdit
        Dim intOrderItemIndex As Integer
        Dim intCol As Integer
        Dim introw As Integer
        Dim strProductname As String
        Dim objSelectedOrderItem As OrderItem

        'Save current position in the grid to restore after load
        With fgorder
            intCol = e.Col
            introw = e.Row
        End With
        SetSelectedOrderItem(e.Row)
        objSelectedOrderItem = SelectedOrderItem
        If objSelectedOrderItem Is Nothing Then
            'nop
        Else
            With objSelectedOrderItem
                Select Case e.Col
                    Case 2
                        If .Course = CType(fgorder(e.Row, 2), String) Then
                            'nop
                        Else
                            .Course = CType(fgorder(e.Row, 2), String)
                            If .Course.StartsWith("Straight") Then
                                LoadOrderGrid()
                            Else
                                RefreshOrderGridRow(introw, SelectedOrderItem)
                            End If
                        End If
                    Case 3
                        If .Quantity = CType(fgorder(e.Row, 3), Integer) Then
                            'nop
                        Else
                            .Quantity = CType(fgorder(e.Row, 3), Integer)
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                            LoadOrderTotalsGrid()
                        End If
                    Case 4
                        If .ItemMod = CType(fgorder(e.Row, 4), String) Then
                            'nop
                        Else
                            .ItemMod = CType(fgorder(e.Row, 4), String)
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                        End If
                    Case 5
                        If .ItemModNote = CType(fgorder(e.Row, 5), String) Then
                            'nop
                        Else
                            .ItemModNote = CType(fgorder(e.Row, 5), String)
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                        End If
                    Case 6
                        If Math.Abs(.Price - CType(fgorder(e.Row, 6), Double)) < 0.01 Then
                            'nop
                        Else
                            .Price = CType(fgorder(e.Row, 6), Double)
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                        End If
                    Case 10
                        If .CheckNumber = CType(fgorder(e.Row, 10), Integer) Then
                            'nop
                        Else
                            .CheckNumber = Math.Abs(CType(fgorder(e.Row, 10), Integer))
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                        End If
                    Case 11
                        If .VoidReason = CType(fgorder(e.Row, 11), String) Then
                            'nop
                        Else
                            .VoidReason = CType(fgorder(e.Row, 11), String)
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                        End If
                    Case 12
                        If .FoodFrom = CType(fgorder(e.Row, 12), String) Then
                            'nop
                        Else
                            .FoodFrom = CType(fgorder(e.Row, 12), String)
                            RefreshOrderGridRow(introw, SelectedOrderItem)
                        End If
                End Select
            End With
            'Loading grid may change user's position in the grid so restore this
            With fgorder
                .Select(introw, intCol)
                .ShowCell(introw, intCol)
            End With
            EnableDisableFormBtns()
            UpdateSelectedOrdersGridRow()
        End If
    End Sub

    Private Sub UpdateSelectedOrdersGridRow()
        If CurrentRowOfExistingOrderGrid Is Nothing Then Exit Sub
        Select Case CurrentOrder.OrderType
            Case Order.enumOrderType.EatIn
                RefreshCurrentOrderEatinGrid(CurrentRowOfExistingOrderGrid.Index, CurrentOrder)
            Case Order.enumOrderType.Delivery, Order.enumOrderType.Pickup
                RefreshCurrentOrderTakeoutGrid(CurrentRowOfExistingOrderGrid.Index, CurrentOrder)
        End Select
    End Sub

    Private Property SortColumnSelectedOrdersEatInGrid() As C1.Win.C1FlexGrid.Column
        Get
            If m_SortColumnSelectedOrdersEatInGrid Is Nothing Then
                Return fgSelectOrderEatin.Cols.Item(0)
            Else
                Return m_SortColumnSelectedOrdersEatInGrid
            End If
        End Get
        Set(ByVal Value As C1.Win.C1FlexGrid.Column)
            If m_SortColumnSelectedOrdersEatInGrid Is Value Then
                If m_SortColumnSelectedOrdersEatInGrid.Sort = C1FlexGrid.SortFlags.Descending Then
                    m_SortColumnSelectedOrdersEatInGrid.Sort = C1FlexGrid.SortFlags.Ascending
                Else
                    m_SortColumnSelectedOrdersEatInGrid.Sort = C1FlexGrid.SortFlags.Descending
                End If
            Else
                m_SortColumnSelectedOrdersEatInGrid = Value
                m_SortColumnSelectedOrdersEatInGrid.Sort = C1FlexGrid.SortFlags.Ascending
            End If
        End Set
    End Property

    Private Property SortColumnSelectedOrdersTakeOutGrid() As C1.Win.C1FlexGrid.Column
        Get
            If m_SortColumnSelectedOrdersTakeOutGrid Is Nothing Then
                Return fgSelectOrderTakeOut.Cols.Item(0)
            Else
                Return m_SortColumnSelectedOrdersTakeOutGrid
            End If
        End Get
        Set(ByVal Value As C1.Win.C1FlexGrid.Column)
            If m_SortColumnSelectedOrdersTakeOutGrid Is Value Then
                If m_SortColumnSelectedOrdersTakeOutGrid.Sort = C1FlexGrid.SortFlags.Descending Then
                    m_SortColumnSelectedOrdersTakeOutGrid.Sort = C1FlexGrid.SortFlags.Ascending
                Else
                    m_SortColumnSelectedOrdersTakeOutGrid.Sort = C1FlexGrid.SortFlags.Descending
                End If
            Else
                m_SortColumnSelectedOrdersTakeOutGrid = Value
                m_SortColumnSelectedOrdersTakeOutGrid.Sort = C1FlexGrid.SortFlags.Ascending
            End If
        End Set
    End Property

    Private Sub btnOKEatInNewOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles btnOKEatInNewOrder.Click, btnOKTakeOutNewOrder.Click
        StartNewOrder()
        ShowAppropriateCurrentOrderTabPage()
    End Sub

    Private Sub StartNewOrder()
        Dim objOrder As Order
        Dim intSQLreturnValue As Integer
        Dim intPromiseHrMins As Integer

        Select Case tabCurrentOrders.SelectedTab.Text
            Case "EatIn New Order"
                SetSelectedSD(dteEatInOrderPromisedAt.Value)
                SetSelectedSDSession(dteEatInOrderPromisedAt.Value)
                objOrder = SelectedOrders.AddNewOrder
                With objOrder
                    .Waiter = WaiterForNewOrder
                    .TakeOutCustomerName = " "
                    .TakeOutCustomerAddress = " "
                    .TakeOutCustomerPhone = " "
                    .OrderType = Order.enumOrderType.EatIn
                    .Table = TableForNewOrder
                    .AdultsCount = CInt(txtAdults.Text)
                    If txtKids.Text = String.Empty Then
                        .KidsCount = 0
                    Else
                        .KidsCount = CInt(txtKids.Text)
                    End If
                    .IsDesiOrder = chkDesiEatin.Checked
                    .IsHouseACOrder = chkHouseACOrderEatIn.Checked
                    .HouseACCheckSignedBy = " "
                    Select Case CInt(txtAdults.Text)
                        Case Is > 5
                            .TipType = Order.enumTips.TipAt18PercentAddedFor6OrMore
                        Case Else
                            .TipType = Order.enumTips.NoTipsAdded
                    End Select
                    .PromisedAt = dteEatInOrderPromisedAt.Value

                    .DiscountType = Order.enumMGCoupon.NoDiscount
                    .Saledate = dteEatInOrderPromisedAt.Value.Date
                End With
            Case "TakeOut New Order"
                SetSelectedSD(dteTakeOutOrderPromisedAt.Value)
                SetSelectedSDSession(dteTakeOutOrderPromisedAt.Value)
                objOrder = SelectedOrders.AddNewOrder
                With objOrder
                    .Waiter = WaiterForNewOrder
                    '.Customer = CustomerForNewOrder
                    .TakeOutCustomerName = txtTakeOutCustomer.Text
                    .TakeOutCustomerAddress = txtAddress.Text '& txtDirections.Text
                    .TakeOutCustomerPhone = cboPhoneNo.Text
                    .FoodProvidedBy = cboFoodProvidedBy.Text
                    .OrderReceivedFrom = cboOrderReceivedFrom.Text
                    '.Table is assigned a dummy table by order itself
                    .AdultsCount = 0
                    .KidsCount = 0
                    .IsDesiOrder = chkDesi.Checked
                    .IsHouseACOrder = chkHouseACOrderTakeOut.Checked
                    .PromisedAt = dteTakeOutOrderPromisedAt.Value
                    .TipType = Order.enumTips.NoTipsAdded
                    intPromiseHrMins = (dteTakeOutOrderPromisedAt.Value.Hour * 100) + dteTakeOutOrderPromisedAt.Value.Minute
                    Select Case chkDelivery.Checked
                        Case True
                            .OrderType = Order.enumOrderType.Delivery
                            .DiscountType = Order.enumMGCoupon.NoDiscount
                        Case False
                            .OrderType = Order.enumOrderType.Pickup
                            If (.IsHouseACOrder = True) Then
                                .DiscountType = Order.enumMGCoupon.NoDiscount
                            Else
                                .DiscountType = Order.enumMGCoupon.HappyCurryHourDiscount
                            End If
                            'Time limit for discount deleted;discount on T/O all the time. 07/15/06
                            '.DiscountType = Order.enumMGCoupon.HappyCurryHourDiscount
                    End Select
                    .Saledate = dteTakeOutOrderPromisedAt.Value.Date
                End With
        End Select
        'change for future SaleDates and SDSession

        intSQLreturnValue = objOrder.SetData(m_selectedSDSession)
        If intSQLreturnValue >= 0 Then
            CurrentOrder = objOrder
        End If
        ShowAppropriateCurrentOrderTabPage()
        LoadTextOfCurrentOrderFiltersSelection()
        LoadSelectedOrdersGrid()
    End Sub

    Private Sub LoadOrderGridHeader()
        Dim strEatinCustomerName As String
        Dim sb1 As New System.Text.StringBuilder()
        Dim sb2 As New System.Text.StringBuilder()
        Dim Row0AllCells As New C1.Win.C1FlexGrid.CellRange()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        Dim i As Integer
        cs = fgorder.Styles("Header")

        For i = 0 To fgorder.Cols.Count - 1
            fgorder.SetCellStyle(0, i, cs)
        Next
        Row0AllCells = fgorder.GetCellRange(0, 0, 0, 11)
        If CurrentOrder Is Nothing Then
            sb1.Append("Select an Order or Start a new Order from above screen".PadLeft(73))
        Else
            With CurrentOrder
                If .Customer Is Nothing Then
                    strEatinCustomerName = String.Empty
                Else
                    strEatinCustomerName = .Customer.CustomerName
                End If
                Select Case .OrderType
                    Case Order.enumOrderType.EatIn

                        sb1.Append(("Table: " & .Table.TableName & "-" & .TableSeating.ToString).PadLeft(20) & _
                                    (" Waiter: " & .Waiter.EmployeeFullName).PadLeft(24) & _
                                     (" Seating: " & Format(.PromisedAt, "hh:mm tt") & " " & _
                                                       Format(.PromisedAt, "MM/dd/yy")).PadLeft(40) & _
                                   (" Customer: " & strEatinCustomerName).PadLeft(24))

                    Case Order.enumOrderType.Pickup
                        sb1.Append((" Pickup: " & .TakeOutCustomerName & "  " & .TakeOutCustomerPhone).PadLeft(40) & _
                                     (" Customer: " & strEatinCustomerName).PadLeft(24))
                        ''(" Waiter: " & .Waiter.EmployeeFullName).PadLeft(24) & _
                        '(" At: " & Format(.PromisedAt, "hh:mm tt") & " " & _
                        '                   Format(.PromisedAt, "MM/dd/yy")).PadLeft(50) & _
                    Case Order.enumOrderType.Delivery
                        sb1.Append((" Deliver: " & .TakeOutCustomerName & "  " & .TakeOutCustomerPhone).PadLeft(40) & _
                                    (" Waiter: " & .Waiter.EmployeeFullName).PadLeft(24) & _
                                    (" Deliver By: " & Format(.PromisedAt, "hh:mm tt") & " " & _
                                                       Format(.PromisedAt, "MM/dd/yy")).PadLeft(50))

                End Select
            End With
        End If
        Row0AllCells.Data = sb1.ToString
    End Sub

    Private Sub AddOrderItem(ByVal objMenuItem As MenuItem)
        Dim menuIndex As Integer
        Dim objOrderItem As OrderItem

        objOrderItem = CurrentOrder.AddOrderItem(objMenuItem)

        If objOrderItem Is Nothing Then
            'nop: Overide required and was not given
        Else
            'With objOrderItem
            '    If CurrentOrder.OrderType = Order.enumOrderType.EatIn Then
            '        .Course = CurrentProductCategory.ProductCategoryCourse
            '    Else
            '        .Course = "Straight Fire"
            '    End If
            '    '.Quantity = 1
            'End With
            LoadOrderGrid()
            UpdateSelectedOrdersGridRow()
        End If
    End Sub

    Private Sub LoadOrderGrid()
        Dim intRow As Integer
        Dim objOrderItem As OrderItem

        LoadOrderGridHeader()

        intRow = fgorder.Rows.Fixed
        With fgorder
            .SuspendLayout()
            .Rows.Count = .Rows.Fixed   'to clear data rows without losing column header fixed row
            .Rows.Count = CurrentOrder.AllOrderItems.Count + .Rows.Fixed
        End With

        With CurrentOrder
            For Each objOrderItem In .AllOrderItems
                RefreshOrderGridRow(intRow, objOrderItem)
                intRow += 1
            Next
        End With
        fgorder.ResumeLayout()
        LoadOrderTotalsGrid()
        EnableDisableFormBtns()
    End Sub

    Private Sub LoadSelectedOrdersGrid()
        Select Case CurrentOrder.OrderType
            Case Order.enumOrderType.EatIn
                LoadCurrentOrderEatinGrid()
                'If CurrentRowOfExistingOrderGrid.Index > fgSelectOrderEatin.Rows.Fixed Then
                If Not (CurrentRowOfExistingOrderGrid Is Nothing) AndAlso (CurrentRowOfExistingOrderGrid.Index > fgSelectOrderEatin.Rows.Fixed) Then
                    fgSelectOrderEatin.Select(CurrentRowOfExistingOrderGrid.Index, 0, True)
                    ExistingOrderSelected(fgSelectOrderEatin)
                Else
                    fgSelectOrderEatin.Select(-1, 0, True)
                End If
            Case Order.enumOrderType.Delivery, Order.enumOrderType.Pickup
                LoadCurrentOrderTakeoutGrid()
                If Not (CurrentRowOfExistingOrderGrid Is Nothing) AndAlso (CurrentRowOfExistingOrderGrid.Index > fgSelectOrderTakeOut.Rows.Fixed) Then
                    fgSelectOrderTakeOut.Select(CurrentRowOfExistingOrderGrid.Index, 0, True)
                    ExistingOrderSelected(fgSelectOrderTakeOut)
                Else
                    fgSelectOrderTakeOut.Select(-1, 0, True)
                End If
                fgSelectOrderTakeOut.Select(CurrentRowOfExistingOrderGrid.Index, 0, True)
        End Select
    End Sub

    Private Sub RefreshOrderGridRow(ByVal introw As Integer, ByVal objOrderItem As OrderItem)
        Dim fgCellstyle As C1.Win.C1FlexGrid.CellStyle
        Dim nameOffset As String

        With objOrderItem
            Select Case .GroupId = 0
                Case True
                    If .Status = OrderItem.enumStatus.Voided Then
                        'fgCellstyle = fgorder.Styles.Item("VoidItem")
                        fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("VoidItem")
                    ElseIf .CheckPrintedAt > DateTime.MinValue Then
                        fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("CheckPrinted")
                    ElseIf .KOTPrintedAt > DateTime.MinValue Then
                        fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("KOTPrinted")
                    Else
                        'nop
                    End If
                Case False
                    Select Case objOrderItem.MenuItem.ProductPricingType
                        Case Product.enumProductPricingType.SystemPricedSingleItem, Product.enumProductPricingType.UserPricedSingleItem
                            nameOffset = "  *"
                            If .Status = OrderItem.enumStatus.Voided Then
                                'fgCellstyle = fgorder.Styles.Item("VoidItem")
                                fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("GroupVoidItem")
                            ElseIf .CheckPrintedAt > DateTime.MinValue Then
                                fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("GroupCheckPrinted")
                            ElseIf .KOTPrintedAt > DateTime.MinValue Then
                                fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("GroupKOTPrinted")
                            Else
                                fgorder.Rows.Item(introw).Style = fgorder.Styles.Item("Group")
                            End If
                        Case Else
                            nameOffset = String.Empty
                    End Select
            End Select
            fgorder(introw, 0) = introw - fgorder.Rows.Fixed + 1
            fgorder.Rows.Item(introw).UserData = CurrentOrder.AllOrderItems.IndexOf(objOrderItem)

            Select Case .OrderItemName = .OrderItemNameHindi
                Case True
                    fgorder(introw, 1) = nameOffset & .OrderItemNameHindi
                Case False
                    fgorder(introw, 1) = nameOffset & .OrderItemNameHindi + " (" + .OrderItemName + ")"
            End Select
            fgorder(introw, 2) = .Course
            fgorder(introw, 3) = .Quantity
            fgorder(introw, 4) = .ItemMod
            fgorder(introw, 5) = .ItemModNote
            fgorder(introw, 6) = .Price
            fgorder(introw, 7) = .OrderItemAmount
            fgorder(introw, 8) = (.KOTPrintedAt > DateTime.MinValue)
            fgorder(introw, 9) = (.CheckPrintedAt > DateTime.MinValue)
            If .CheckNumber > 0 Then fgorder(introw, 10) = .CheckNumber
            fgorder(introw, 11) = .VoidReason
            fgorder(introw, 12) = .FoodFrom
        End With
    End Sub

    Private Sub EnableDisableFormBtns()
        If CurrentOrder Is Nothing OrElse CurrentOrder.ItemsCount = 0 Then
            btnKOT.Enabled = False
            btnCheck.Enabled = False
            btnVoid.Enabled = False
            cboTips.Enabled = False
            cboDiscount.Enabled = False
            cboTax.Enabled = False
        Else
            btnKOT.Enabled = True
            btnCheck.Enabled = True
            cboTips.Enabled = True
            cboDiscount.Enabled = True
            cboTax.Enabled = True
        End If

        If SelectedOrderItem Is Nothing Then
            btnVoid.Enabled = False
        Else
            btnVoid.Enabled = True
        End If

        If CurrentOrder Is Nothing OrElse CurrentOrder.AllOrderItems.Count = 0 Then
            btnPay.Enabled = False
        Else
            btnPay.Enabled = True
        End If

        If CurrentOrder Is Nothing OrElse CurrentOrder.ItemsCount = 0 OrElse CurrentOrder.IsDirty = False Then
            btnReset.Enabled = False
        Else
            btnReset.Enabled = True
        End If

        If CurrentOrder Is Nothing OrElse CurrentOrder.IsDirty = False Then
            btnClose.Enabled = True
        Else
            btnClose.Enabled = False
        End If
    End Sub

    Private Sub ShowAppropriateCurrentOrderTabPage()
        If CurrentOrder Is Nothing Then
            'go to take out page on load 6/27/16
            tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(7)        'W section Page
        Else
            With CurrentOrder
                Select Case .OrderType
                    Case Order.enumOrderType.EatIn
                        Select Case .Table.DiningSection
                            Case DTable.enumDiningSection.A
                                tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(4)
                            Case DTable.enumDiningSection.B
                                tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(2)
                            Case DTable.enumDiningSection.M
                                tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(1)
                            Case DTable.enumDiningSection.T
                                tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(3)
                            Case DTable.enumDiningSection.W
                                tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(0)
                        End Select
                    Case Else
                        tabCurrentOrders.SelectedTab = tabCurrentOrders.TabPages.Item(7)
                End Select
            End With
        End If
        CurrentOrderPageSelected()
    End Sub

    Private ReadOnly Property SelectedOrderStatus() As Orders.EnumFilter
        Get
            Dim strStatus As String
            Dim newOrderEnumFilter As Orders.EnumFilter
            If (cboOrderStatusFilter.SelectedIndex >= 0) Then
                strStatus = cboOrderStatusFilter.GetItemText(cboOrderStatusFilter.SelectedItem)
                newOrderEnumFilter = CType([Enum].Parse(GetType(Orders.EnumFilter), strStatus), Orders.EnumFilter)
                Return newOrderEnumFilter
            Else
                Return Orders.EnumFilter.All
            End If
        End Get
    End Property

    Private Property SelectedOrderTypeOrSection() As Orders.EnumFilterOrderTypeOrSection
        Get
            Return m_SelectedOrderTypeOrSection
        End Get
        Set(ByVal Value As Orders.EnumFilterOrderTypeOrSection)
            m_SelectedOrderTypeOrSection = Value
        End Set
    End Property

    Private Sub cboTables_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles cboTables.SelectedIndexChanged
        Dim objTable As DTable
        'Dim dresult As MsgBoxResult

        If (cboTables.SelectedIndex < 0) Or _
            (cboTables.SelectedIndex > SaleDays.CreateSaleDay.ActiveTables.Count - 1) Then
            Exit Sub
        Else
            objTable = SaleDays.CreateSaleDay.ActiveTables.Item(cboTables.SelectedIndex)
            If objTable Is Nothing Then
                MsgBox("Table not available, select another one", MsgBoxStyle.OkOnly, "Invalid Table")
            Else
                Me.TableForNewOrder = objTable
            End If
        End If
    End Sub

    Private Sub cboEatInWaiters_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
        Handles cboEatInWaiters.SelectedIndexChanged, cboTakeOutWaiters.SelectedIndexChanged
        Dim objWaiter As Employee
        Dim cbo As ComboBox

        cbo = CType(sender, ComboBox)
        If (cbo.SelectedIndex < 0) Or _
            (cbo.SelectedIndex > SaleDays.CreateSaleDay.ActiveCashiers.Count - 1) Then
            Exit Sub
        Else
            objWaiter = SaleDays.CreateSaleDay.ActiveCashiers.Item(cbo.SelectedIndex)
        End If
        If objWaiter Is Nothing Then
            MsgBox("Invalid Waiter, select another one", MsgBoxStyle.OkOnly, "Invalid Waiter")
        End If
        Me.WaiterForNewOrder = objWaiter
    End Sub

    Private Sub txtAdults_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAdults.TextChanged
        EnableStartNewOrderBtn()
    End Sub

    Private Sub LoadTakeOutNewOrderDefaults()
        dteTakeOutOrderPromisedAt.Value = DateAdd(DateInterval.Minute, 20, SaleDays.CreateSaleDay.Now)
        cboTakeOutWaiters.SelectedIndex = 4 'sp
        cboEatInWaiters_SelectedIndexChanged(cboTakeOutWaiters, New System.EventArgs())
        txtTakeOutCustomer.Text = String.Empty
        txtAddress.Text = String.Empty
        cboPhoneNo.SelectedIndex = -1
        cboPhoneNo.Text = String.Empty
        cboFoodProvidedBy.SelectedIndex = 0
        cboOrderReceivedFrom.SelectedIndex = 4
        chkDesi.Checked = False
        chkDelivery.Checked = True
        chkHouseACOrderTakeOut.Checked = False
    End Sub

    Private Sub LoadEatInNewOrderDefaults()
        dteEatInOrderPromisedAt.Value = DateAdd(DateInterval.Minute, 20, SaleDays.CreateSaleDay.Now)
        cboEatInWaiters.SelectedIndex = -1
        cboTables.SelectedIndex = -1
        txtAdults.Text = String.Empty
        txtKids.Text = String.Empty
        chkDesiEatin.Checked = False
        chkHouseACOrderEatIn.Checked = False
        'check for nothing is required to avoid calling enablestartbutton from its set block
        If TableForNewOrder Is Nothing Then
            'nop 
        Else
            TableForNewOrder = Nothing
        End If
        If WaiterForNewOrder Is Nothing Then
            'nop 
        Else
            WaiterForNewOrder = Nothing
        End If

    End Sub

    Private Sub cboPhoneNo_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles cboPhoneNo.SelectedValueChanged, cboPhoneNo.TextChanged
        EnableStartNewOrderBtn()

    End Sub

    Private Sub btnCancelEatInNewOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles btnCancelEatInNewOrder.Click, btnCancelTakeOutNewOrder.Click
        ShowAppropriateCurrentOrderTabPage()
    End Sub

    Private Sub fgorder_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgorder.EnterCell
        Dim intNextRow As Integer
        Dim intNextCol As Integer
        'Dim IsRowActive As Boolean
        'Dim objOrderItem As OrderItem
        'Dim intOrderItemIndex As Integer

        If CurrentOrder Is Nothing OrElse CurrentOrder.AllOrderItems.Count = 0 Then Exit Sub
        With fgorder
            intNextCol = .ColSel
            intNextRow = .RowSel
            Select Case intNextCol
                Case Is < 0
                    Exit Sub
                Case 0, 1
                    .AutoSearch = C1FlexGrid.AutoSearchEnum.FromTop
                Case 2, 3, 4, 5, 6, 10
                    .AutoSearch = C1FlexGrid.AutoSearchEnum.None
                Case Else
                    Select Case intNextCol = .Cols.Count - 1
                        Case True
                            If intNextRow = .Rows.Count - 1 Then
                                intNextRow = .Rows.Fixed()
                            Else
                                intNextRow = .RowSel + 1
                            End If

                            .Select(intNextRow, 0)
                            .ShowCell(intNextRow, 0)
                            EnableDisableFormBtns()
                        Case False
                            intNextCol += 1
                            .Select(intNextRow, intNextCol)
                            .ShowCell(intNextRow, intNextCol)
                    End Select
            End Select
            'Do Until IsRowActive = True
            '    If (fgorder.RowSel < fgorder.Rows.Fixed) Then
            '        IsRowActive = True
            '    Else
            '        intOrderItemIndex = CInt(fgorder.Rows(intNextRow).UserData)
            '        If (intOrderItemIndex < CurrentOrder.AllOrderItems.Count) And _
            '            (intOrderItemIndex >= 0) Then
            '            Select Case CurrentOrder.AllOrderItems.Item(intOrderItemIndex).Status
            '                Case OrderItem.enumStatus.Active
            '                    IsRowActive = True
            '                Case Else
            '                    IsRowActive = False
            '                    intNextRow += 1
            '            End Select
            '        Else
            '            IsRowActive = False
            '            intNextRow += 1
            '        End If
            '    End If
            '    'stop if all items are voided
            '    If intNextRow > fgorder.Rows.Count - 1 Then
            '        intNextRow = -1
            '        IsRowActive = True
            '    End If
            'Loop
            '.Select(intNextRow, intNextCol)
            '.ShowCell(intNextRow, intNextCol)
        End With
    End Sub

    Private Sub fgSelectOrderEatin_Click(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles fgSelectOrderEatin.DoubleClick, fgSelectOrderTakeOut.DoubleClick, _
                     fgSelectOrderEatin.Click, fgSelectOrderTakeOut.Click

        Dim fg As C1.Win.C1FlexGrid.C1FlexGrid
        Dim fgCol As C1.Win.C1FlexGrid.Column

        fg = CType(sender, C1.Win.C1FlexGrid.C1FlexGrid)
        Select Case fg.MouseRow < fg.Rows.Fixed
            Case True
                fgCol = fg.Cols.Item(fg.MouseCol)
                If fg Is fgSelectOrderEatin Then
                    SortColumnSelectedOrdersEatInGrid = fgCol
                ElseIf fg Is fgSelectOrderTakeOut Then
                    SortColumnSelectedOrdersTakeOutGrid = fgCol
                Else
                    'nop
                End If
                SortSelectOrderGrids(fg)
            Case False
                ExistingOrderSelected(sender)
        End Select
    End Sub

    Private Sub SortSelectOrderGrids(ByVal fgGrid As C1.Win.C1FlexGrid.C1FlexGrid)
        With fgGrid
            If fgGrid Is fgSelectOrderEatin Then
                .Sort(C1FlexGrid.SortFlags.UseColSort, SortColumnSelectedOrdersEatInGrid.Index)
            ElseIf fgGrid Is fgSelectOrderTakeOut Then
                .Sort(C1FlexGrid.SortFlags.UseColSort, SortColumnSelectedOrdersTakeOutGrid.Index)
            Else
                Exit Sub
            End If
        End With
    End Sub

    Private Sub fgSelectOrderEatin_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) _
            Handles fgSelectOrderEatin.KeyUp, fgSelectOrderTakeOut.KeyUp
        Dim fg As C1.Win.C1FlexGrid.C1FlexGrid
        Dim intRow As Integer
        Dim intCol As Integer

        fg = CType(sender, C1.Win.C1FlexGrid.C1FlexGrid)
        intRow = fg.RowSel
        intCol = fg.ColSel
        Select Case e.KeyCode
            Case Keys.Down, Keys.Right
                If (intRow >= fg.Rows.Count - 1) OrElse (intRow < fg.Rows.Fixed) Then
                    intRow = fg.Rows.Fixed '+ 1      'Col hdr row is not set as a fixed row for getting sort clicks
                Else
                    intRow += 1
                End If
                fg.Select(intRow, 0)
                e.Handled = True
            Case Keys.Up, Keys.Left
                If (intRow >= fg.Rows.Count - 1) OrElse (intRow <= fg.Rows.Fixed) Then
                    intRow = fg.Rows.Fixed '+ 1
                Else
                    intRow -= 1
                End If
                fg.Select(intRow, 0)
                e.Handled = True
            Case Else
                ExistingOrderSelected(sender)
                e.Handled = True
        End Select
    End Sub

    Private Sub ExistingOrderSelected(ByVal sender As Object)
        Dim intOrderIndex As Integer
        Dim intCol As Integer
        Dim strProductname As String
        Dim objSelectedMenuItem As MenuItem
        Dim fg As C1.Win.C1FlexGrid.C1FlexGrid
        fg = CType(sender, C1.Win.C1FlexGrid.C1FlexGrid)
        If fg.RowSel >= fg.Rows.Fixed Then
            CurrentRowOfExistingOrderGrid = fg.Rows.Item(fg.RowSel)
            intOrderIndex = CInt(CurrentRowOfExistingOrderGrid.UserData())
            CurrentOrder = SelectedOrders.FilteredOrders.Item(intOrderIndex)
        End If
    End Sub

    Private Property CurrentRowOfExistingOrderGrid() As C1.Win.C1FlexGrid.Row
        Get
            Return m_CurrentRowOfExistingOrderGrid
        End Get
        Set(ByVal Value As C1.Win.C1FlexGrid.Row)
            m_CurrentRowOfExistingOrderGrid = Value
        End Set
    End Property

    Private Sub btnKOT_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnKOT.Click
        Dim kotDoc As BOPr.PosDoc
        kotDoc = CurrentOrder.KOTText
        'm_POSText.SendText("+16097516149", "Test Message")
        If (kotDoc Is Nothing) Then
            'because duplicate override was not available
        Else
            If (m_POSPrint.POSPrint(kotDoc) = 0) Then
                CurrentOrder.SetKOTPrintedAt()
                LoadOrderGrid()
                UpdateSelectedOrdersGridRow()

            Else
                MessageBox.Show("Failed to print KOT. Check roll, printer connections and print again.", "Printer Message", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End If

        End If

    End Sub

    Private Sub btnCheck_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheck.Click
        Dim checkNumber As Integer
        Dim checkDoc As BOPr.PosDoc
        Dim sms As PosText
        With CurrentOrder
            If .IsHouseACOrder = True AndAlso (.Customer Is Nothing) Then
                MsgBox("You must select Account Customer before printing Check for House A/C Order")
            Else
                For checkNumber = 1 To .NumberOfSplitChecks
                    checkDoc = .GuestCheckText(checkNumber)
                    sms = New PosText()
                    'sms.SendText("+16097516149", checkDoc)
                    If (checkDoc Is Nothing) Then
                        'because duplicate override was not available
                    Else
                        If (m_POSPrint.POSPrint(checkDoc) = 0) Then
                            If checkNumber = .NumberOfSplitChecks Then CurrentOrder.SetGuestCheckPrintedAt()
                            LoadOrderGrid()
                            UpdateSelectedOrdersGridRow()

                        Else
                            MessageBox.Show("Failed to print your check. Check roll,printer connections and print again.", "Printer Message", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        End If
                    End If
                    'If Not (m_POSPrint.POSPrint(CurrentOrder.GuestCheckText(checkNumber)) = 0) Then
                    '    MessageBox.Show("Failed to print. Check roll and print again.", "Printer Message", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                    '    Exit Sub
                    'End If
                    'm_POSPrint.POSPrint(CurrentOrder.GuestCheckText(checkNumber))
                Next
                LoadOrderGrid()
                UpdateSelectedOrdersGridRow()
            End If
        End With
    End Sub

    Private Sub OnOverrideNeeded(ByVal objOverride As Override) Handles _
            m_CurrentOrder.OverrideNeeded, m_TableForNewOrder.OverrideNeeded, m_SelectedOrders.OverrideNeeded
        'create override form, pass override object and get override from user
        'If user gives an override, the order object, which is waiting for this event to return
        ' proceeds with providing (e.g. KOT, void etc.) whatever user asked for   
        'Get rid of Overrides post 6/15/2016 
        'Dim newfrmPOSOverride As New frmPOSOverride()
        'With newfrmPOSOverride
        '    .newOverride = objOverride
        '    .ShowDialog()
        'End With
        With objOverride
            .OverrideAvailable = True
            .OverrideBy = 129
            .OverrideReason = "Get rid of Overrides post 6/15/2016 "
            .OverrideStatus = Override.enumOverrideStatus.Active
            .SetData()
        End With
    End Sub

    Private Sub btnVoid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVoid1.Click
        Dim index As Integer

        With fgorder
            If .Rows.Selected.Count > 0 Then
                For index = .Rows.Fixed To .Rows.Count - 1
                    If .Rows(index).Selected = True Then
                        CurrentOrder.VoidAnOrderItem(CInt(fgorder.Rows.Item(index).UserData))
                    End If
                Next
                LoadSelectedOrdersGrid()
                btnVoid1.Enabled = True
                'LoadOrderGrid()
                'UpdateSelectedOrdersGridRow()
            Else
                MsgBox("First select an item or items to void and then click Void.", MsgBoxStyle.Critical)
                Exit Sub
            End If
        End With
    End Sub

    Private Sub cboTips_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTips.SelectedIndexChanged
        If cboTips.SelectedIndex < 0 Then cboTips.SelectedIndex = 0
        If CurrentOrder Is Nothing Then
            'nop
        Else
            CurrentOrder.TipType = CType(cboTips.SelectedIndex, Order.enumTips)
        End If
        LoadOrderTotalsGrid()
        UpdateSelectedOrdersGridRow()
        cboTips.Visible = False
    End Sub

    Private Sub cboDiscount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDiscount.SelectedIndexChanged
        If cboDiscount.SelectedIndex < 0 Then cboDiscount.SelectedIndex = 0
        If CurrentOrder Is Nothing Then
            'nop
        Else
            CurrentOrder.DiscountType = CType(cboDiscount.SelectedIndex, Order.enumMGCoupon)
        End If
        LoadOrderTotalsGrid()
        UpdateSelectedOrdersGridRow()
        cboDiscount.Visible = False
    End Sub

    Private Sub cboTax_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTax.SelectedIndexChanged
        If cboTax.SelectedIndex < 0 Then cboTax.SelectedIndex = 0
        If CurrentOrder Is Nothing Then
            'nop
        Else
            CurrentOrder.SalesTaxStatus = CType(cboTax.SelectedIndex, Order.enumOrderSalesTaxStatus)
        End If
        LoadOrderTotalsGrid()
        UpdateSelectedOrdersGridRow()
        cboTax.Visible = False
    End Sub

    Private ReadOnly Property SelectedSD() As SaleDay
        Get
            Return m_SelectedSaleDay
        End Get
    End Property
    Private Sub SetSelectedSD(ByVal targetSaleDate As Date)
        m_SelectedSaleDay = SaleDays.CreateSaleDay.Item(targetSaleDate.Date, True)
    End Sub

    Private ReadOnly Property SelectedSDSession() As SaleDaySession
        Get
            Return m_selectedSDSession
        End Get
    End Property
    Private Sub SetSelectedSDSession(ByVal targetDateTime As DateTime)
        m_selectedSDSession = SelectedSD.GetTargetSaleDaySession(targetDateTime)
    End Sub
    Private Sub SetSelectedSDSession(ByVal newSession As Session.enumSessionName)
        If (SelectedSD Is Nothing) Then
            m_selectedSDSession = Nothing
        Else
            m_selectedSDSession = SelectedSD.AllSaleDaySessions.Item(newSession)
        End If
    End Sub

    Private ReadOnly Property SelectedOrders() As Orders
        Get
            If SelectedSDSession Is Nothing Then
                m_SelectedOrders = SelectedSD.AllOrders
            Else
                m_SelectedOrders = SelectedSDSession.AllOrders
            End If
            Return m_SelectedOrders
        End Get
    End Property

    Private Sub dteSaleDateNewOrder_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles dteEatInOrderPromisedAt.ValueChanged, dteTakeOutOrderPromisedAt.ValueChanged
        Dim DatePicker As DateTimePicker

        DatePicker = CType(sender, DateTimePicker)
        SetSelectedSD(DatePicker.Value.Date)
        SetSelectedSDSession(DatePicker.Value)
    End Sub
    Private Sub dteSaleDateFilter_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
            Handles dteSaleDateFilter.ValueChanged

        SetSelectedSD(dteSaleDateFilter.Value.Date)
        OnSessionFilterChange()
    End Sub
    Private Sub cboSessionFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboSessionFilter.SelectedIndexChanged
        SetSelectedSD(dteSaleDateFilter.Value.Date)
        OnSessionFilterChange()
    End Sub

    Private Sub OnSessionFilterChange()
        Dim strSessionName As String
        Dim enumSessionName As Session.enumSessionName

        strSessionName = cboSessionFilter.GetItemText(cboSessionFilter.SelectedItem)
        enumSessionName = CType([Enum].Parse(GetType(Session.enumSessionName), strSessionName), Session.enumSessionName)
        SetSelectedSDSession(enumSessionName)
    End Sub


    Private Sub btnPay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPay.Click
        Dim objfrmEditPayment As frmEditPaymentPr
        Dim dResult As DialogResult

        If CurrentOrder Is Nothing Then
            'nop
        Else
            fgMenuItems.Enabled = False
            fgSelectOrderEatin.Enabled = False
            fgSelectOrderTakeOut.Enabled = False
            fgOrderTotals.Enabled = False
            tabCurrentOrders.Enabled = False

            m_frmEditPaymentPr = New frmEditPaymentPr()
            With m_frmEditPaymentPr
                Me.AddOwnedForm(m_frmEditPaymentPr)
                .ShowInTaskbar = False
                .ParentOrder = CurrentOrder
                .SelectedOrders = SelectedOrders.FilteredOrders
                .BackColor = Color.White

                .Size = New Size(fgorder.Width + panelFormBtns.Width - 2, fgorder.Height)
                .SetDesktopLocation(Me.panelOrder.PointToScreen(Me.panelOrder.Location).X, Me.panelOrder.PointToScreen(Me.panelOrder.Location).Y)
            End With
            m_frmEditPaymentPr.Show()
        End If
    End Sub

    Private Sub m_frmEditPaymentPr_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_frmEditPaymentPr.Closing
        fgMenuItems.Enabled = True
        fgSelectOrderEatin.Enabled = True
        fgSelectOrderTakeOut.Enabled = True
        fgOrderTotals.Enabled = True
        tabCurrentOrders.Enabled = True
        m_frmEditPaymentPr = Nothing
        If CurrentOrder Is Nothing Then
            'nop
        Else
            If CurrentOrder.SetData(m_selectedSDSession) > 0 Then LoadOrderGrid()
            LoadSelectedOrdersGrid()
        End If
    End Sub

    Private Sub m_frmEditPaymentPr_ParentOrderChanged(ByVal newOrder As BOPr.Order) Handles m_frmEditPaymentPr.ParentOrderChanged
        If (newOrder Is CurrentOrder) OrElse (newOrder Is Nothing) Then
            'nop
        Else
            Select Case newOrder.OrderType
                Case Order.enumOrderType.Delivery, Order.enumOrderType.Pickup
                    LoadCurrentOrderTakeoutGrid()
                Case Else
                    LoadCurrentOrderEatinGrid()
            End Select
            CurrentOrder = newOrder
            LoadSelectedOrdersGrid()
        End If
    End Sub
    Private Sub m_frmEditPaymentPr_FetchNextOrder() Handles m_frmEditPaymentPr.FetchNextOrder
        Dim fgSelectOrder As C1.Win.C1FlexGrid.C1FlexGrid
        Dim introw As Integer

        Select Case CurrentOrder.OrderType
            Case Order.enumOrderType.Delivery, Order.enumOrderType.Pickup
                introw = GetNextRowofSelectOrderGrids(fgSelectOrderTakeOut)

                If introw = fgSelectOrderEatin.Rows.Fixed Then
                    CurrentOrder = Nothing
                Else
                    fgSelectOrderTakeOut.Select(introw, 0)
                    ExistingOrderSelected(fgSelectOrderTakeOut)
                End If
                LoadCurrentOrderTakeoutGrid()
            Case Else
                introw = GetNextRowofSelectOrderGrids(fgSelectOrderEatin)

                If introw = fgSelectOrderEatin.Rows.Fixed Then
                    CurrentOrder = Nothing
                Else
                    If introw = fgSelectOrderEatin.Rows.Fixed Then
                        'nop 
                    Else
                        fgSelectOrderEatin.Select(introw, 0)
                        ExistingOrderSelected(fgSelectOrderEatin)
                    End If
                End If
                LoadCurrentOrderEatinGrid()
        End Select
        m_frmEditPaymentPr.ParentOrder = CurrentOrder
    End Sub

    Private Function GetNextRowofSelectOrderGrids(ByVal fg As C1.Win.C1FlexGrid.C1FlexGrid) As Integer
        Dim introw As Integer
        With fg
            introw = CurrentRowOfExistingOrderGrid.Index
            If introw = .Rows.Count - 1 Then
                introw = .Rows.Fixed
            Else
                introw += 1
            End If
        End With
        Return introw
    End Function

    Private Sub fgMenuItems_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgMenuItems.CellButtonClick
        Dim introw As Integer
        Dim objMenuItem As MenuItem


        If MenuItemsOfCurrentProductCategory Is Nothing Then Exit Sub
        introw = fgMenuItems.RowSel
        If (introw <= MenuItemsOfCurrentProductCategory.Count) Then
            objMenuItem = MenuItemsOfCurrentProductCategory.Item(fgMenuItems.RowSel)
            If objMenuItem.FamilyMenuItems.Count > 0 Then
                With cboProductFamily
                    .BeginUpdate()
                    .BackColor = Color.LightYellow
                    .Items.Clear()
                    For Each objMenuItem In objMenuItem.FamilyMenuItems
                        .Items.Add(objMenuItem.ProductNameHindi)
                    Next
                    .EndUpdate()
                    .Location = New Point(fgMenuItems.Cols.Item(1).Left, fgMenuItems.Rows.Item(fgMenuItems.RowSel).Bottom)
                    .Visible = True
                    .BringToFront()
                End With
            End If
        Else
            'e.Cancel = True
        End If
    End Sub

    Private Sub fgMenuItems_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgMenuItems.EnterCell
        With fgMenuItems
            Select Case .ColSel
                Case 0, 1
                    .AllowEditing = False
                    .AutoSearch = C1FlexGrid.AutoSearchEnum.FromCursor

                Case 2
                    .AllowEditing = True
                    .AutoSearch = C1FlexGrid.AutoSearchEnum.None
            End Select
        End With
    End Sub

    Private Sub cboProductFamily_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProductFamily.SelectedIndexChanged
        Dim objMainMenuItem As MenuItem
        Dim objSelectedMenuItem As MenuItem
        With cboProductFamily
            If .SelectedIndex >= 0 Then
                'first find the HEAD of ProducFamily i.e menu item in the 2nd. column 
                objMainMenuItem = MenuItemsOfCurrentProductCategory.Item(fgMenuItems.RowSel)
                objSelectedMenuItem = objMainMenuItem.FamilyMenuItems.Item(.SelectedIndex)
                AddOrderItem(objSelectedMenuItem)
            End If
            .Visible = False
        End With
        fgMenuItems.Focus()
    End Sub

    Private Sub cboProductFamily_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProductFamily.DropDown
        cboProductFamily.Focus()
    End Sub

    Private Sub fgOrderTotals_EnterCell(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgOrderTotals.EnterCell
        Dim intNextRow As Integer

        With fgOrderTotals
            Select Case .ColSel
                Case Is < 0
                    Exit Sub
                Case 0, 1
                    'nop
                Case Else
                    Select Case .ColSel = .Cols.Count - 1
                        Case True
                            If .RowSel = .Rows.Count - 1 Then
                                intNextRow = .Rows.Fixed()
                            Else
                                intNextRow = .RowSel + 1
                            End If
                            .Select(intNextRow, 0)

                        Case False
                            .Select(.RowSel, .ColSel + 1)
                    End Select
            End Select
        End With
    End Sub

    Private Sub fgorder_StartEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgorder.StartEdit
        SetSelectedOrderItem(e.Row)
        Select Case CurrentOrder.AllowOrderEdit(SelectedOrderItem)
            Case False
                e.Cancel = True  'Terminate any edit action by user
                Exit Sub
            Case True
                'nop
        End Select
    End Sub


    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        CurrentOrder.ResetAllOrderItems()
        LoadOrderGrid()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Friend Sub CloseMe()
        If CurrentOrder Is Nothing Then
            'nop
        Else
            CurrentOrder.SetData(m_selectedSDSession)
        End If
        If Not (m_POSPrint Is Nothing) Then m_POSPrint.ReleasePrinter()
        m_SelectedSaleDay = Nothing
        m_selectedSDSession = Nothing
        m_SelectedOrders = Nothing
        CurrentProductCategory = Nothing
    End Sub

    Private Sub fgorder_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgorder.CellButtonClick
        Dim strSelectedModNotes As String
        Dim i As Integer
        'fgorder.se()
        SetSelectedOrderItem(e.Row)
        If CurrentOrder.AllowOrderEdit(SelectedOrderItem) = False Then
            'nop
        Else
            Select Case fgorder.Cols.Item(e.Col).Name
                Case "ModNote"
                    If chkLbo Is Nothing Then
                        'nop
                    Else
                        With chkLbo
                            .Location = New Point(fgorder.Cols.Item(e.Col).Left, fgorder.Rows.Item(e.Row).Top)
                            If CStr(fgorder.Item(e.Row, e.Col)) = String.Empty Then
                                strSelectedModNotes = " "
                            Else
                                strSelectedModNotes = CStr(fgorder.Item(e.Row, e.Col))
                            End If
                            For i = 0 To chkLbo.Items.Count - 1
                                If strSelectedModNotes.IndexOf(chkLbo.Items(i).ToString) >= 0 Then
                                    chkLbo.SetItemChecked(i, True)
                                Else
                                    chkLbo.SetItemChecked(i, False)
                                End If
                            Next
                            .Visible = True
                            .BringToFront()
                        End With
                    End If
                Case Else
                    'nop
            End Select
        End If
    End Sub

    Private Sub chkLbo_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLbo.Leave, chkLbo.MouseLeave
        Dim objenumerator As IEnumerator
        Dim sbSelectedModNotes As New System.Text.StringBuilder()

        Dim i As Integer
        SetSelectedOrderItem(fgorder.RowSel)
        For i = 0 To chkLbo.Items.Count - 1
            If chkLbo.GetItemChecked(i) = True Then

                If sbSelectedModNotes.Length = 0 Then
                    sbSelectedModNotes.Append(chkLbo.Items(i).ToString)
                Else
                    sbSelectedModNotes.Append("=>")
                    sbSelectedModNotes.Append(chkLbo.Items(i).ToString)
                End If
                'chkLbo.SetItemChecked(i, False)
            End If
        Next
        'chkLbo.ClearSelected()
        SelectedOrderItem.ItemModNote = sbSelectedModNotes.ToString
        RefreshOrderGridRow(fgorder.RowSel, SelectedOrderItem)
        chkLbo.Visible = False

    End Sub

    Private Sub cboProductFamily_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboProductFamily.Leave
        cboProductFamily.Visible = False
    End Sub



    Private Sub txtTakeOutCustomer_LostFocus(sender As Object, e As System.EventArgs) Handles txtTakeOutCustomer.LostFocus
        EnableStartNewOrderBtn()
    End Sub

    Private Sub fgorder_CellChanged(sender As Object, e As C1FlexGrid.RowColEventArgs) Handles fgorder.CellChanged, fgorder.AfterEdit
        'Dim test As String
        'Select Case fgorder.Cols.Item(e.Col).Name
        '    Case "VoidReason"
        '        SelectedOrderItem.VoidReason = fgorder.Item(e.Row, e.Col).ToString()

        '    Case Else
        '        'nop
        'End Select
    End Sub

    Private Sub fgorder_MouseClick(sender As Object, e As MouseEventArgs) Handles fgorder.MouseClick

    End Sub

    Private Sub fgorder_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles fgorder.MouseDoubleClick

    End Sub

    Private Sub fgorder_TabIndexChanged(sender As Object, e As EventArgs) Handles fgorder.TabIndexChanged

    End Sub
End Class
