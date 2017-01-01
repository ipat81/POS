Option Strict On
Imports BOPr.CRBalance
Imports BOPr.Employee
Imports BOPr.SaleDays

Imports System.Windows.Forms
Public Class frmPOSDesktop
    Inherits System.Windows.Forms.Form
#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPOSDesktop))
        '
        'frmPOSDesktop
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmPOSDesktop"
        Me.Text = "frmPOSDesktop"

    End Sub

#End Region

    Friend WithEvents MainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents mnuFile As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEdit As System.Windows.Forms.MenuItem
    Friend WithEvents mnuEmployee As System.Windows.Forms.MenuItem
    Friend WithEvents mnuPOS As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCashRegister As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCustomer As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReports As System.Windows.Forms.MenuItem
    Friend WithEvents mnuHelp As System.Windows.Forms.MenuItem

    Friend WithEvents mnuFileExit As MenuItem

    Friend WithEvents mnuEditCopy As MenuItem
    Friend WithEvents mnuEditPaste As MenuItem
    Friend WithEvents mnuEditCut As MenuItem
    Friend WithEvents mnuEditPreferences As MenuItem

    Friend WithEvents mnuEmployeeEditEmployee As MenuItem
    Friend WithEvents mnuEmployeeClock As MenuItem

    Friend WithEvents mnuPOSRetailOrder As MenuItem
    Friend WithEvents mnuPOSOnHoldOrder As MenuItem
    Friend WithEvents mnuPOSBulkOrder As MenuItem

    Friend WithEvents mnuCRBeginEnd As MenuItem
    Friend WithEvents mnuCRCashierLogOnLogOff As MenuItem
    Friend WithEvents mnuCRCountCash As MenuItem
    Friend WithEvents mnuCRCashTranSaction As MenuItem
    Friend WithEvents mnuCROpenCashRegister As MenuItem
    Friend WithEvents mnuCRReConcileCashRegister As MenuItem

    Friend WithEvents mnuMenuEditMasterMenu As MenuItem
    Friend WithEvents mnuMenuEditDailyMenu As MenuItem

    Friend WithEvents mnuCustomerEditCustomer As MenuItem

    Friend WithEvents mnuReportsSaleSummary As MenuItem
    Friend WithEvents mnuReportsDailySaleOrders As MenuItem
    Friend WithEvents mnuReportsCRBalances As MenuItem
    Friend WithEvents mnuReportsSTSummary As MenuItem
    Friend WithEvents mnuReportsSOverridesSummary As MenuItem
    Friend WithEvents mnuFutureOrderList As MenuItem
    Friend WithEvents mnuQBSalesReport As MenuItem
    Friend WithEvents mnuuReportsMasterMenu As MenuItem
    Friend WithEvents mnuQBRevenueReport As MenuItem
    Friend WithEvents mnuLateTablesList As MenuItem
    Friend WithEvents mnuTargetHrsFix As MenuItem

    Friend toolBar1 As New ToolBar()
    Friend toolBarButtonClock As ToolBarButton
    Friend toolBarButtonOrder As ToolBarButton
    Friend toolBarButtonCashCount As ToolBarButton
    Friend toolBarButtonCashTxn As ToolBarButton
    'Friend toolBarButtonOnHoldOrders As ToolBarButton
    'Friend toolBarButtonCashierSignOn As ToolBarButton

    Private statusBar1 As New StatusBar()


    'Private m_PoleDisplay As frmPoleDisplay
    Private WithEvents m_frmClock As frmClock
    Private WithEvents m_frmEditRetailOrder As frmEditRetailOrder

    Private WithEvents m_frmEditCRTxn As frmEditCRTxn
    Private WithEvents m_frmCashRegisterCount As frmCashRegisterCount
    Private WithEvents m_frmReConcile As frmReconcileCashRegister
    Private m_AppDir As String = "C:\POS2006"
    Private Enum MenuItemChoices
        Clock
        EditRetailOrder
    End Enum

    Friend Event StatusChanged(ByVal StatusMsg As String)

    Private Sub frmPOSDesktop_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'With frmPoleDisplay.CreatePoleDisplay()
        '    .GrabPoleDisplay()

        '    .DisplayText("Welcome to ", frmPoleDisplay.enumPoleDisplayText.DT_Normal)
        '    .DisplayText("Masala Grill", frmPoleDisplay.enumPoleDisplayText.DT_Normal)
        '    '.ReleasePoleDisplay()
        'End With
        'Me.ClientSize = New Size(856, 692)
        'Me.BackColor = Color.LightGray
        Me.BackgroundImage = Image.FromFile(m_AppDir & "\ppbk006.bmp")
        Me.DesktopBounds = Windows.Forms.Screen.PrimaryScreen.WorkingArea   'to cover entire screen
        Me.AutoScroll = False
        Me.IsMdiContainer = True
        Me.Icon = New System.Drawing.Icon(m_AppDir & "\MISC33.ico")
        Me.KeyPreview = True
        Me.Text = "Masala Grill POS"

        createMenuTree()
        createFileSubMenu()
        createEditSubMenu()
        createEmployeeSubMenu()
        createPOSSubmenu()
        createCashregisterSubmenu()
        createMenuSubMenu()
        createCustomerSubmenu()
        createReportsSubMenu()

        Me.Menu = Me.MainMenu1

        InitializeMenuEvents(Me.Menu, AddressOf MenuItem_Click)
        InitializeMyToolBar()
        CreateMyStatusBar()
        'CurrentCashier()

    End Sub

    'Private Sub CurrentCashier()
    '    Dim objRecentCrBalance As POSBOPr.CRBalance
    '    Dim objCurrentCashier As POSBOPr.Employee
    '    objRecentCrBalance = CreateSaleDay.TodaysCRBalances.MostRecentCRBalance
    '    If objRecentCrBalance Is Nothing Then
    '        'nope
    '    Else
    '        For Each objCurrentCashier In CreateSaleDay.ActiveCashiers
    '            If objCurrentCashier.EmployeeId = objRecentCrBalance.CashierId Then
    '                CreateSaleDay.CurrentCashier = objCurrentCashier
    '            End If
    '        Next

    '    End If
    'End Sub


    Private Sub createMenuTree()
        Me.MainMenu1 = New MainMenu()
        Me.mnuFile = New MenuItem()
        Me.mnuFile.ShowShortcut = True

        Me.mnuEdit = New MenuItem()
        Me.mnuEmployee = New MenuItem()
        Me.mnuEmployee.ShowShortcut = True
        Me.mnuPOS = New MenuItem()
        Me.mnuPOS.ShowShortcut = True
        Me.mnuCashRegister = New MenuItem()
        Me.mnuCashRegister.ShowShortcut = True
        Me.mnuMenu = New MenuItem()
        Me.mnuCustomer = New MenuItem()
        Me.mnuReports = New MenuItem()
        Me.mnuReports.ShowShortcut = True
        Me.mnuHelp = New MenuItem()

        'Set menuItem Properties

        Me.mnuFile.Index = 0
        Me.mnuFile.Text = "&File"

        Me.mnuEdit.Index = 1
        Me.mnuEdit.Text = "&Edit"

        Me.mnuEmployee.Index = 2
        Me.mnuEmployee.Text = "&Staff"

        Me.mnuPOS.Index = 3
        Me.mnuPOS.Text = "&POS"

        Me.mnuCashRegister.Index = 4
        Me.mnuCashRegister.Text = "&Cash Register"

        Me.mnuMenu.Index = 5
        Me.mnuMenu.Text = "&Menu"

        Me.mnuCustomer.Index = 6
        Me.mnuCustomer.Text = "&Customer"

        Me.mnuReports.Index = 7
        Me.mnuReports.Text = "&Reports"

        Me.mnuHelp.Index = 8
        Me.mnuHelp.Text = "&Help"

        Me.MainMenu1.MenuItems.AddRange(New MenuItem() _
                       {mnuFile, mnuEdit, mnuEmployee, mnuPOS, mnuCashRegister, mnuMenu, mnuCustomer, mnuReports, mnuHelp})


        'Me.Menu = Me.MainMenu1


    End Sub

    Private Sub createFileSubMenu()

        mnuFileExit = New MenuItem()
        Me.mnuFile.MenuItems.AddRange(New MenuItem() {mnuFileExit})

        mnuFileExit.Index = 0
        mnuFileExit.Text = "&Exit"
    End Sub

    Private Sub createEmployeeSubMenu()

        mnuEmployeeEditEmployee = New MenuItem()
        mnuEmployeeClock = New MenuItem()
        Me.mnuEmployee.MenuItems.AddRange(New MenuItem() {mnuEmployeeEditEmployee, mnuEmployeeClock})

        mnuEmployeeEditEmployee.Index = 0
        mnuEmployeeEditEmployee.Text = "&Edit Employee"

        mnuEmployeeClock.Index = 1
        mnuEmployeeClock.Text = "&Clock"

    End Sub

    Private Sub createEditSubMenu()
        mnuEditCopy = New MenuItem()
        mnuEditPaste = New MenuItem()
        mnuEditCut = New MenuItem()
        mnuEditPreferences = New MenuItem()

        Me.mnuEdit.MenuItems.AddRange(New MenuItem() {mnuEditCopy, mnuEditPaste, mnuEditCut, mnuEditPreferences})

        mnuEditCopy.Index = 0
        mnuEditCopy.Text = "&Copy"

        mnuEditPaste.Index = 1
        mnuEditPaste.Text = "&Paste"

        mnuEditCut.Index = 2
        mnuEditCut.Text = "C&ut"

        mnuEditPreferences.Index = 3
        mnuEditPreferences.Text = "&Preferences"


    End Sub


    Private Sub createPOSSubmenu()

        mnuPOSBulkOrder = New MenuItem()
        mnuPOSOnHoldOrder = New MenuItem()
        mnuPOSRetailOrder = New MenuItem()


        Me.mnuPOS.MenuItems.AddRange(New MenuItem() {mnuPOSRetailOrder, mnuPOSOnHoldOrder, mnuPOSBulkOrder})
        mnuPOSRetailOrder.Index = 0
        mnuPOSRetailOrder.Text = "&Retail Order"

        'mnuPOSOnHoldOrder.Index = 1
        'mnuPOSOnHoldOrder.Text = "On &Hold Order"

        mnuPOSBulkOrder.Index = 1
        mnuPOSBulkOrder.Text = "&Bulk Order"

    End Sub

    Private Sub createCashregisterSubmenu()

        mnuCRBeginEnd = New MenuItem()
        mnuCRCashierLogOnLogOff = New MenuItem()
        mnuCRCashTranSaction = New MenuItem()
        mnuCRCountCash = New MenuItem()
        mnuCROpenCashRegister = New MenuItem()
        mnuCRReConcileCashRegister = New MenuItem()

        Me.mnuCashRegister.MenuItems.AddRange(New MenuItem() {mnuCRBeginEnd, mnuCRCashierLogOnLogOff, mnuCRCountCash, mnuCRCashTranSaction, mnuCROpenCashRegister, mnuCRReConcileCashRegister})

        'mnuCRBeginEnd.Index = 0
        'mnuCRBeginEnd.Text = "Begin/End Day"

        'mnuCRCashierLogOnLogOff.Index = 1
        'mnuCRCashierLogOnLogOff.Text = "Cashier Sign On/Off"

        mnuCRCountCash.Index = 0
        mnuCRCountCash.Text = "Count Cash"

        mnuCRCashTranSaction.Index = 1
        mnuCRCashTranSaction.Text = "Cash Transaction"

        mnuCRReConcileCashRegister.Index = 2
        mnuCRReConcileCashRegister.Text = "Reconcile Cash Register"

        mnuCROpenCashRegister.Index = 3
        mnuCROpenCashRegister.Text = "Open Cash Register"
    End Sub

    Private Sub createMenuSubMenu()

        mnuMenuEditDailyMenu = New MenuItem()
        mnuMenuEditMasterMenu = New MenuItem()
        Me.mnuMenu.MenuItems.AddRange(New MenuItem() {mnuMenuEditMasterMenu, mnuMenuEditDailyMenu})

        mnuMenuEditMasterMenu.Index = 0
        mnuMenuEditMasterMenu.Text = "Create A New Menu"

        mnuMenuEditDailyMenu.Index = 1
        mnuMenuEditDailyMenu.Text = "Edit A Menu"

    End Sub


    Private Sub createCustomerSubmenu()

        mnuCustomerEditCustomer = New MenuItem()
        mnuCustomer.MenuItems.AddRange(New MenuItem() {mnuCustomerEditCustomer})

        mnuCustomerEditCustomer.Index = 0
        mnuCustomerEditCustomer.Text = "Edit Customer"

    End Sub

    Private Sub createReportsSubMenu()
        mnuReportsSaleSummary = New MenuItem()
        mnuReportsDailySaleOrders = New MenuItem()
        mnuReportsCRBalances = New MenuItem()
        mnuReportsSTSummary = New MenuItem()
        mnuReportsSOverridesSummary = New MenuItem()
        mnuFutureOrderList = New MenuItem()
        mnuQBSalesReport = New MenuItem()
        mnuuReportsMasterMenu = New MenuItem()
        mnuQBRevenueReport = New MenuItem()
        mnuLateTablesList = New MenuItem()
        mnuTargetHrsFix = New MenuItem()



        mnuReports.MenuItems.AddRange(New MenuItem() {mnuReportsSaleSummary, mnuReportsDailySaleOrders, mnuReportsSTSummary, _
        mnuReportsCRBalances, mnuReportsSOverridesSummary, mnuFutureOrderList, mnuQBSalesReport, _
        mnuuReportsMasterMenu, mnuQBRevenueReport, mnuLateTablesList, mnuTargetHrsFix})


        mnuReportsSaleSummary.Index = 0
        mnuReportsSaleSummary.Text = "Daily Sale Summary"

        mnuReportsDailySaleOrders.Index = 1
        mnuReportsDailySaleOrders.Text = "Waiter Performance"

        mnuReportsSTSummary.Index = 2
        mnuReportsSTSummary.Text = "Payroll Clock Report"

        mnuReportsSOverridesSummary.Index = 3
        mnuReportsSOverridesSummary.Text = "Overrides Summary"

        mnuFutureOrderList.Index = 4
        mnuFutureOrderList.Text = "Future Order List"

        mnuQBSalesReport.Index = 5
        mnuQBSalesReport.Text = "QB Sales Report"

        mnuReportsCRBalances.Index = 6
        mnuReportsCRBalances.Text = "Cash Register Balances"

        mnuLateTablesList.Index = 7
        mnuLateTablesList.Text = "Tables Closed After Cash Count"

        'mnuuReportsMasterMenu.Index = 7
        'mnuuReportsMasterMenu.Text = "Master Menu"

        mnuQBRevenueReport.Index = 8
        mnuQBRevenueReport.Text = "QB Revenue Report"

        mnuTargetHrsFix.Index = 9
        mnuTargetHrsFix.Text = "Target Hrs Fix"
    End Sub




    ' Make all the menu items in this form point to MenuItem_Click and MenuItem_Select routines.
    ' The main application should call this routine with m = Me.Menu

    Private Sub InitializeMenuEvents(ByVal m As Menu, ByVal ClickEvent As EventHandler)
        Dim mi As MenuItem
        Dim SalesReport As rptSalesSummary

        If TypeOf m Is MenuItem Then
            ' Cast the argument to a MenuItem object.
            mi = DirectCast(m, MenuItem)

            ' Initialize the Click event, but only if this isn't a submenu
            If m.MenuItems.Count = 0 Then
                AddHandler mi.Click, ClickEvent
            End If
        End If

        ' Call recursively for all items in the Menuitems collection
        For Each mi In m.MenuItems
            InitializeMenuEvents(mi, ClickEvent)
        Next

    End Sub

    '  common Click routine for all menu items

    Private Sub MenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim mi As MenuItem = CType(sender, MenuItem)
        Dim SalesReport As rptSalesSummary
        Dim WaiterPerformance As rptWaiterPerformance
        Dim QBPayrollReport As rptQBPayrollReport
        Dim OverrideList As rptOverridesList
        Dim FutureOrderList As rptFutureOrderList
        Dim frmReportViewer As frmFGReportViewer
        Dim QBSalesReport As rptQBSalesReport
        Dim LateTablesList As rptLateTables
        Dim QBRevenueReport As rptQBRevenueReport
        Dim objFixPayroll As fixPayroll


        Select Case mi.Text.ToUpper
            Case "&EXIT"
                Me.Close()
            Case "&COPY"
            Case "&PASTE"
            Case "&CUT"
            Case "&PREFERENCES"

            Case "&EDIT EMPLOYEE"

                ' ShowDialog(frmEmployee)

            Case "&CLOCK"
                ShowClockForm()

            Case "&RETAIL ORDER"
                ShowRetailOrderForm()

            Case "BULK ORDERS"

            Case "CASH TRANSACTION"
                ShowCRTXnForm()

            Case "COUNT CASH"
                ShowCountCashForm()

            Case "RECONCILE CASH REGISTER"
                ShowReconcileCashRegisterForm()

            Case "CREATE A NEW MENU"
                'ShowSetCurrentMenuForm()

            Case "EDIT A MENU"
                'ShowSetUpMenuForm()

            Case "EDIT CUSTOMER"

            Case "DAILY SALE SUMMARY"

                SalesReport = New rptSalesSummary()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                SalesReport.SetReportViewer(frmReportViewer)
            Case "WAITER PERFORMANCE"
                WaiterPerformance = New rptWaiterPerformance()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                WaiterPerformance.SetReportViewer(frmReportViewer)
            Case "PAYROLL CLOCK REPORT"
                QBPayrollReport = New rptQBPayrollReport()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                QBPayrollReport.SetReportViewer(frmReportViewer, "")

            Case "CASH REGISTER BALANCES"
                'ShowCashierCashBalanceReportForm()

            Case "OVERRIDES SUMMARY"
                OverrideList = New rptOverridesList()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                OverrideList.SetReportViewer(frmReportViewer)

            Case "FUTURE ORDER LIST"
                FutureOrderList = New rptFutureOrderList()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                FutureOrderList.SetReportViewer(frmReportViewer)

            Case "QB SALES REPORT"
                QBSalesReport = New rptQBSalesReport()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                QBSalesReport.SetReportViewer(frmReportViewer)

            Case "TABLES CLOSED AFTER CASH COUNT"
                LateTablesList = New rptLateTables()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                LateTablesList.SetReportViewer(frmReportViewer)
            Case "QB REVENUE REPORT"
                QBRevenueReport = New rptQBRevenueReport()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                QBRevenueReport.SetReportViewer(frmReportViewer)
            Case "TARGET HRS FIX"
                objFixPayroll = New fixPayroll()
                frmReportViewer = New frmFGReportViewer()
                SetDockedForm(frmReportViewer)
                objFixPayroll.SetReportViewer(frmReportViewer)
        End Select
    End Sub

    'Private Sub ShowReport(ByVal strreportName As String)
    '    Dim objfrmReportViewer As New frmReportViewer()

    '    SetDockedForm(objfrmReportViewer)

    '    Select Case strreportName
    '        Case "OVERRIDES SUMMARY"
    '            objfrmReportViewer.ReportName = frmReportViewer.enumReportName.OverridesSummary
    '            objfrmReportViewer.lblTitle.Text = "OVERRIDES SUMMARY"

    '        Case "DAILY SALE SUMMARY"
    '            objfrmReportViewer.ReportName = frmReportViewer.enumReportName.DailySaleSummary
    '            objfrmReportViewer.lblTitle.Text = "DAILY SALE SUMMARY"
    '    End Select
    '    objfrmReportViewer.Show()
    'End Sub
    Private Sub SetDockedForm(ByVal posForm As Form)
        With posForm
            .AutoScroll = False
            .MdiParent = Me
            .Anchor = AnchorStyles.None
            .Dock = DockStyle.Fill

            .Size = New Size(Me.ClientSize.Width, Me.ClientSize.Height - Me.toolBar1.Height + 3)
            .StartPosition = FormStartPosition.CenterParent
        End With
    End Sub

    Private Sub SetForm(ByVal posform As Form)
        With posform
            .AutoScroll = False
            .MdiParent = Me
            .Anchor = AnchorStyles.None
            '.StartPosition = FormStartPosition.Manual
            .StartPosition = FormStartPosition.CenterParent
            '.Location = New Point(50, 150)
        End With
    End Sub

    Private Sub ShowClockForm()
        Dim msg As String
        msg = " "
        RaiseEvent StatusChanged(msg)
        If m_frmClock Is Nothing Then
            m_frmClock = New frmClock()
            SetForm(m_frmClock)
            m_frmClock.Show()
        Else
            m_frmClock.Activate()
        End If
    End Sub

    'Private Sub ShowCashierSignOnOffForm()
    '    If m_frmCashierSignOnOff Is Nothing Then
    '        m_frmCashierSignOnOff = New frmCashierSignOnOff()
    '        SetForm(CType(m_frmCashierSignOnOff, Form))
    '        m_frmCashierSignOnOff.Show()
    '    Else
    '        m_frmCashierSignOnOff.Activate()
    '    End If

    'End Sub

    Private Sub ShowCRTXnForm()
        If m_frmEditCRTxn Is Nothing Then
            m_frmEditCRTxn = New frmEditCRTxn()
            SetForm(CType(m_frmEditCRTxn, Form))
            m_frmEditCRTxn.Show()
        Else
            m_frmEditCRTxn.Activate()
        End If
    End Sub

    Private Sub ShowCountCashForm()
        If m_frmCashRegisterCount Is Nothing Then
            m_frmCashRegisterCount = New frmCashRegisterCount()
            SetForm(CType(m_frmCashRegisterCount, Form))
            m_frmCashRegisterCount.Show()
        Else
            m_frmCashRegisterCount.Activate()
        End If
    End Sub

    Private Sub ShowRetailOrderForm()
        Dim msg As String

        If m_frmEditRetailOrder Is Nothing Then
            m_frmEditRetailOrder = New frmEditRetailOrder()
            SetDockedForm(m_frmEditRetailOrder)
            m_frmEditRetailOrder.Show()
        Else
            m_frmEditRetailOrder.Activate()
        End If

        RaiseEvent StatusChanged(msg)
    End Sub

    Private Sub ShowReconcileCashRegisterForm()
        Dim msg As String
        RaiseEvent StatusChanged(msg)
        If m_frmReConcile Is Nothing Then
            m_frmReConcile = New frmReconcileCashRegister()
            SetForm(CType(m_frmReConcile, Form))
            m_frmReConcile.Show()
        Else
            m_frmReConcile.Activate()
        End If
    End Sub

    'Private Sub ShowBeginEndDayForm()
    '    Dim msg As String
    '    msg = " "
    '    RaiseEvent StatusChanged(msg)
    '    If m_frmOpenCloseDay Is Nothing Then
    '        m_frmOpenCloseDay = New frmOpenCloseSaleDay()
    '        SetForm(CType(m_frmOpenCloseDay, Form))
    '        m_frmOpenCloseDay.Show()
    '    Else
    '        m_frmOpenCloseDay.Activate()
    '    End If
    'End Sub

    Private Sub InitializeMyToolBar()
        Dim btnSize As Size
        Dim myImagelist As ImageList

        ' Create and initialize the ToolBar and ToolBarButton controls.
        toolBar1 = New ToolBar()
        toolBar1.BorderStyle = BorderStyle.Fixed3D
        toolBar1.Appearance = ToolBarAppearance.Normal

        'toolBar1.BackgroundImage.FromFile(m_AppDir & "\txbk095.bmp")
        'toolBar1.BackgroundImage.FromFile(m_AppDir & "\ppbk006.bmp")
        myImagelist = New ImageList()
        With myImagelist.Images
            .Add(Image.FromFile(m_AppDir & "\NOTE02.ico")) 'Order entry
            .Add(Image.FromFile(m_AppDir & "\MISC33.ico")) 'POS App
            .Add(Image.FromFile(m_AppDir & "\EXCLEM.ico")) 'Exclamation for Override
            .Add(Image.FromFile(m_AppDir & "\CLOCK05.ico"))    'for clock
        End With
        toolBar1.ImageList = myImagelist
        toolBarButtonClock = New ToolBarButton()

        toolBarButtonOrder = New ToolBarButton()
        toolBarButtonCashCount = New ToolBarButton()
        toolBarButtonCashTxn = New ToolBarButton()
        'toolBarButtonOnHoldOrders = New ToolBarButton()
        'toolBarButtonCashierSignOn = New ToolBarButton()


        ' Set the Text properties of the ToolBarButton controls.
        toolBarButtonClock.Text = "Clock In/Out (F3)"
        toolBarButtonClock.ImageIndex = 3
        toolBarButtonClock.Style = ToolBarButtonStyle.PushButton
        toolBarButtonClock.ToolTipText = "Clock In,Out Or to change time Clocked"
        toolBarButtonOrder.ImageIndex = 0
        toolBarButtonOrder.Text = "Retail Order (F4)"
        toolBarButtonOrder.Style = ToolBarButtonStyle.PushButton
        toolBarButtonOrder.ToolTipText = "Enter Eat in or Take Out Order (Not for Catering)"
        toolBarButtonCashCount.Text = "Cash Count (F5)"
        toolBarButtonCashCount.Style = ToolBarButtonStyle.PushButton
        toolBarButtonCashCount.ToolTipText = "Count Cash/Checks,Enter CC Totals at Start and End of Lunch/Dinner"
        toolBarButtonCashTxn.Text = "Cash Txn (F6)"
        toolBarButtonCashTxn.Style = ToolBarButtonStyle.PushButton
        'toolBarButtonOnHoldOrders.Text = "On Hold Orders"
        'toolBarButtonCashierSignOn.Text = "Cashier Sign On/Off (F7)"
        toolBarButtonCashTxn.ToolTipText = "Enter Petty Cash,Deposit etc."
        toolBarButtonClock.Tag = System.Windows.Forms.Keys.F3
        toolBarButtonOrder.Tag = System.Windows.Forms.Keys.F4
        toolBarButtonCashCount.Tag = System.Windows.Forms.Keys.F5
        toolBarButtonCashTxn.Tag = System.Windows.Forms.Keys.F6
        'toolBarButtonCashierSignOn.Tag = System.Windows.Forms.Keys.F7

        ' Add the ToolBarButton controls to the ToolBar.
        toolBar1.Buttons.Add(toolBarButtonClock)
        toolBar1.Buttons.Add(toolBarButtonOrder)
        toolBar1.Buttons.Add(toolBarButtonCashCount)
        toolBar1.Buttons.Add(toolBarButtonCashTxn)
        'toolBar1.Buttons.Add(toolBarButtonOnHoldOrders)
        'toolBar1.Buttons.Add(toolBarButtonCashierSignOn)

        ' Add the event-handler delegate.
        AddHandler toolBar1.ButtonClick, AddressOf Me.toolBar1_ButtonClick

        ' Add the ToolBar to the Form.
        Controls.Add(toolBar1)
    End Sub
    Private Sub frmPOSDesktop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        Select Case e.KeyCode
            Case Is = CType(toolBarButtonClock.Tag, System.Windows.Forms.Keys)
                ShowClockForm()
                e.Handled = True
            Case Is = CType(toolBarButtonOrder.Tag, System.Windows.Forms.Keys)
                ShowRetailOrderForm()
                e.Handled = True
            Case Is = CType(toolBarButtonCashCount.Tag, System.Windows.Forms.Keys)
                Me.ShowCountCashForm()
                e.Handled = True
            Case Is = CType(toolBarButtonCashTxn.Tag, System.Windows.Forms.Keys)
                Me.ShowCRTXnForm()
                e.Handled = True
                'Case Is = CType(toolBarButtonCashierSignOn.Tag, System.Windows.Forms.Keys)
                '    'ShowCashierSignOnOffForm()
                '    e.Handled = True
            Case System.Windows.Forms.Keys.F12
                'frmPoleDisplay.CreatePoleDisplay.OpenCashDrawer()
                e.Handled = True
            Case Else
                'NOP
        End Select
    End Sub
    Private Sub toolBar1_ButtonClick(ByVal sender As Object, ByVal e As ToolBarButtonClickEventArgs)

        ' Evaluate the Button property to determine which button was clicked.
        Select Case e.Button.Text
            Case "Clock In/Out (F3)"
                ShowClockForm()
            Case "Retail Order (F4)"
                ShowRetailOrderForm()
            Case "Cash Count (F5)"
                Me.ShowCountCashForm()
            Case "Cash Txn (F6)"
                Me.ShowCRTXnForm()
        End Select
    End Sub

    Private Sub CreateMyStatusBar()
        ' Create a StatusBar control.
        'Dim statusBar1 As New StatusBar()
        ' Create two StatusBarPanel objects to display in the StatusBar.
        Dim panel1 As New StatusBarPanel()
        Dim panel2 As New StatusBarPanel()

        ' Display the first panel with a sunken border style.
        panel1.BorderStyle = StatusBarPanelBorderStyle.Sunken
        ' Initialize the text of the panel.
        panel1.Text = "Ready..."
        ' Set the AutoSize property to use all remaining space on the StatusBar.
        panel1.AutoSize = StatusBarPanelAutoSize.Spring
        ' Display the second panel with a raised border style.
        panel2.BorderStyle = StatusBarPanelBorderStyle.Raised
        ' Create ToolTip text that displays the current time.
        panel2.ToolTipText = System.DateTime.Now.ToShortTimeString()
        ' Set the text of the panel to the current date.
        panel2.Text = System.DateTime.Today.ToLongDateString()
        ' Set the AutoSize property to size the panel to the size of the contents.
        panel2.AutoSize = StatusBarPanelAutoSize.Contents

        ' Display panels in the StatusBar control.
        statusBar1.ShowPanels = True

        ' Add both panels to the StatusBarPanelCollection of the StatusBar.         
        statusBar1.Panels.Add(panel1)
        statusBar1.Panels.Add(panel2)

        ' Add the StatusBar to the form.
        Me.Controls.Add(statusBar1)
    End Sub


    Private Sub frmPOSDesktop_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If m_frmEditRetailOrder Is Nothing Then
            'nop
        Else
            If MsgBox("Are you sure you want to close POS? ", MsgBoxStyle.OKCancel, "POS Closing?") = _
                                                           MsgBoxResult.Cancel Then
                e.Cancel = True
            Else
                m_frmEditRetailOrder.Close()
            End If
        End If
    End Sub

    Private Sub m_frmClock_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_frmClock.Closing
        m_frmClock = Nothing
        statusBar1.Panels.Item(0).Text = ""
    End Sub

    Private Sub m_frmEditRetailOrder_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_frmEditRetailOrder.Closing
        m_frmEditRetailOrder = Nothing
        statusBar1.Panels.Item(0).Text = ""
    End Sub

    Private Sub m_frmClock_StatusChanged(ByVal StatusMsg As String) Handles m_frmClock.StatusChanged
        statusBar1.Panels.Item(0).Text = StatusMsg
    End Sub
    'Private Sub m_frmCashierSignOnOff_StatusChanged(ByVal StatusMsg As String) Handles m_frmCashierSignOnOff.StatusChanged
    '    statusBar1.Panels.Item(0).Text = StatusMsg

    'End Sub
    'Private Sub m_frmEditRetailOrder_StatusChanged(ByVal StatusMsg As String) Handles m_frmEditRetailOrder.StatusChanged
    '    statusBar1.Panels.Item(0).Text = StatusMsg

    'End Sub


    Private Sub m_frmEditCRTxn_StatusChanged(ByVal StatusMsg As String) _
    Handles m_frmEditCRTxn.StatusChanged
        statusBar1.Panels.Item(0).Text = StatusMsg
    End Sub

    Private Sub m_frmEditCRTXn_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
   Handles m_frmEditCRTxn.Closing
        m_frmEditCRTxn = Nothing
        statusBar1.Panels.Item(0).Text = ""
    End Sub

    Private Sub m_frmReconcile_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) _
  Handles m_frmReConcile.Closing
        m_frmReConcile = Nothing
        statusBar1.Panels.Item(0).Text = ""
    End Sub


    Private Sub m_frmCashRegisterCount_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles m_frmCashRegisterCount.Closing
        m_frmCashRegisterCount = Nothing
        statusBar1.Panels.Item(0).Text = ""

    End Sub
End Class





