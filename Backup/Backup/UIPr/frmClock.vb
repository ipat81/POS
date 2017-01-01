Option Strict On
Imports BOPr
Public Class frmClock
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
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.Splitter1 = New System.Windows.Forms.Splitter()
        Me.SuspendLayout()
        '
        'Splitter1
        '
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(4, 510)
        Me.Splitter1.TabIndex = 0
        Me.Splitter1.TabStop = False
        '
        'frmClock
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.BackColor = System.Drawing.SystemColors.Desktop
        Me.ClientSize = New System.Drawing.Size(1228, 510)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.Splitter1})
        Me.Name = "frmClock"
        Me.Text = "frmClock"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private m_Employee As Employee
    Private m_Clocks As Clocks
    Private WithEvents m_ClockToOverride As Clock

    'Friend WithEvents fgClocks As ListView
    Private WithEvents pnlEmployees As Panel
    Private WithEvents pnlClocks As Panel
    Private WithEvents txtPassword As TextBox
    Private WithEvents btnClose As Button
    Private txtName As TextBox
    Private WithEvents dteClockOverride As DateTimePicker

    Private WithEvents fgClocks As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lstviewEmployee As ListView
    Friend Event StatusChanged(ByVal StatusMsg As String)

    Private Sub SetUI()
        Dim lblName1 As Label
        Dim lblfrmHeader As Label
        Dim lblPassword1 As Label
        Me.SuspendLayout()
        pnlEmployees = New Panel()
        With pnlEmployees
            .Width = 200
            .Dock = DockStyle.Left
            .BorderStyle = BorderStyle.Fixed3D

        End With
        SetlstviewEmployee()
        pnlEmployees.Controls.Add(lstviewEmployee)

        pnlClocks = New Panel()
        With pnlClocks
            .Dock = DockStyle.Fill
            .BorderStyle = BorderStyle.Fixed3D
        End With

        lblfrmHeader = New Label()
        With lblfrmHeader
            .Dock = DockStyle.Top
            .Height = 20
            .BackColor = Color.SandyBrown
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.MiddleCenter
            .Text = "Click Your Name and Enter Your Password To Clock In Or Out"
        End With

        lblName1 = New Label()
        With lblName1
            .Location = New Point(10, lblfrmHeader.Location.Y + lblfrmHeader.Height + 10)
            .Size = New Size(80, 40)
            .TextAlign = ContentAlignment.TopRight
            .Text = "Name"
        End With

        txtName = New TextBox()
        With txtName
            .Location = New Point(lblName1.Location.X + lblName1.Width + 5, lblName1.Location.Y)
            .Size = New Size(140, lblName1.Height)
            .Text = String.Empty
            .Enabled = False
        End With

        lblPassword1 = New Label()
        With lblPassword1
            .Location = New Point(lblName1.Location.X, lblName1.Location.Y + lblName1.Height + 10)
            .Size = New Size(lblName1.Width, lblName1.Height)
            .TextAlign = ContentAlignment.TopRight
            .Text = "Password"
        End With

        txtPassword = New TextBox()
        With txtPassword
            .Location = New Point(lblPassword1.Location.X + lblPassword1.Width + 5, lblPassword1.Location.Y)
            .Size = New Size(lblName1.Width, lblName1.Height)
            .Text = String.Empty
            .PasswordChar = Chr(42)
        End With

        SetfgClocks()
        With fgClocks
            Dim intWidth As Integer
            Dim fgcol As C1.Win.C1FlexGrid.Column
            For Each fgcol In fgClocks.Cols
                intWidth += fgcol.WidthDisplay
            Next
            .Size = New Size(intWidth, 200)
            .Location = New Point(lblName1.Location.X, lblPassword1.Location.Y + lblPassword1.Height + 30)
        End With
        btnClose = New Button()
        With btnClose
            .Name = "&Close"
            .Size = New Size(60, 40)
            .Location = New Point(fgClocks.Location.X + fgClocks.Width - .Width, fgClocks.Location.Y + fgClocks.Height + 10)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkBlue
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .Text = "Close"
        End With
        SetOverrideClock()

        pnlClocks.Controls.AddRange(New System.Windows.Forms.Control() _
                    {dteClockOverride, lblName1, txtName, lblPassword1, txtPassword, fgClocks, btnClose, lblfrmHeader})
        With Me
            .Size = New Size(970, 450)
            .Controls.AddRange(New System.Windows.Forms.Control() _
                        {pnlClocks, pnlEmployees})
            .ResumeLayout()
        End With
    End Sub

    Private Sub frmClock_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With Me
            .SuspendLayout()
            .AutoScale = False
            .ControlBox = False
            .FormBorderStyle = FormBorderStyle.None
            .BackColor = Color.LightSteelBlue
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ResumeLayout()
        End With
        LoadEmployees()
        RaiseEvent StatusChanged("Click on your name to clock in or out")
        lstviewEmployee.Focus()
    End Sub

    Private Sub LoadEmployees()

        Dim objClock As Clock
        Dim newEmployeeItem As ListViewItem
        Dim objEmployee As Employee

        lstviewEmployee.SuspendLayout()
        lstviewEmployee.Items.Clear()


        For Each objEmployee In SaleDays.CreateSaleDay.ActiveEmployees(Employees.EnumView.CompleteView)
            newEmployeeItem = New ListViewItem()
            newEmployeeItem.ForeColor = Color.Black
            lstviewEmployee.Items.Add(newEmployeeItem)
            newEmployeeItem.Text = objEmployee.EmployeeFullName
        Next
        PaintAlternatingBackColor(lstviewEmployee)
        lstviewEmployee.Refresh()
        lstviewEmployee.ResumeLayout()
    End Sub

    Private Sub SetfgClocks()

        '***************** New fg code starts
        Dim fgCellstyle As C1.Win.C1FlexGrid.CellStyle

        fgClocks = New C1.Win.C1FlexGrid.C1FlexGrid()
        SetfgClocksStyles()

        With fgClocks
            '.Anchor = AnchorStyles.Bottom Or AnchorStyles.Top
            .ScrollBars = ScrollBars.None
            .Cols.Count = 5
            .Cols.Fixed = 0
            .Rows.Count = 1
            .Rows.Fixed = 1

            .ExtendLastCol = True
            .Styles.Normal.Border.Style = C1.Win.C1FlexGrid.BorderStyleEnum.Inset
            .Styles.Normal.Trimming = StringTrimming.EllipsisCharacter
            .Styles.EmptyArea.BackColor = Color.White
            .Styles.Alternate.BackColor = Color.AliceBlue
            .AllowAddNew = False
            .AllowDelete = False
            .AllowEditing = True
            .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictRows
            .AutoSearch = C1.Win.C1FlexGrid.AutoSearchEnum.None
            .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Default
            .KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.MoveAcross
            .KeyActionTab = C1.Win.C1FlexGrid.KeyActionEnum.None
            .Font = New Font(Me.Font.FontFamily, 10.25, FontStyle.Regular)
            .ShowButtons = C1.Win.C1FlexGrid.ShowButtonsEnum.WithFocus
        End With

        'With fgClocks.Cols(0)
        '    .Width = 40
        '    .WidthDisplay = 40
        '    fgClocks(0, 0) = "#"
        '    .Name = "ClocksSeqNum"
        '    .AllowEditing = False
        '    .DataType = GetType(Integer)
        '    .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        'End With

        'With fgClocks.Cols(1)
        '    .Width = 200
        '    .WidthDisplay = 200
        '    fgClocks(0, 1) = "Employee Name"
        '    .Name = "EmployeeName"
        '    .DataType = GetType(String)
        '    .AllowEditing = False
        '    .AllowMerging = True
        '    .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
        'End With

        With fgClocks.Cols(0)
            .Width = 120
            .WidthDisplay = 120
            fgClocks(0, 0) = "       Date"
            .Name = "ClockDate"
            .DataType = GetType(Date)
            .AllowMerging = True
            .AllowEditing = False
            .Format = "ddd MM/dd/yyyy"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgClocks.Cols(1)
            .Width = 90
            .WidthDisplay = 90
            fgClocks(0, 1) = "      Clock In"
            .Name = "ClockInTime"
            .AllowEditing = True
            .ComboList = "..."
            .DataType = GetType(DateTime)
            .Format = "t"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgClocks.Cols(2)
            .Width = 90
            .WidthDisplay = 90
            fgClocks(0, 2) = "     Clock Out"
            .Name = "Clock Out"
            .AllowEditing = True
            .ComboList = "..."
            .DataType = GetType(DateTime)
            .Format = "t"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With
        With fgClocks.Cols(3)
            .Width = 110
            .WidthDisplay = 110
            fgClocks(0, 3) = " Total Hrs:Mins"
            .Name = "TotalTime"
            .DataType = GetType(String)
            '.Format = "t"
            .Style.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.RightCenter
        End With

        With fgClocks.Cols(4)
            .Width = 250
            .WidthDisplay = 250
            .AllowEditing = False
            fgClocks(0, 4) = "    "
            .Name = "DEError"
            .DataType = GetType(String)
        End With
        '******************** new fg code ends

    End Sub
    Private Sub SetlstviewEmployee()
        lstviewEmployee = New ListView()
        With lstviewEmployee
            .Name = "EmployeeView"

            .Bounds = New Rectangle(New Point(0, 30), New Size(pnlEmployees.Width - 10, 320))
            .Visible = True
            .View = View.Details
            .FullRowSelect = True
            .GridLines = True
            .BackColor = Color.LightGray
            .HeaderStyle = ColumnHeaderStyle.Nonclickable

            .Enabled = True
            .MultiSelect = False
            .TabIndex = 1
            .Activation = ItemActivation.OneClick

            '.Columns.Add(" ", -2, HorizontalAlignment.Center).Width = 8
            .Columns.Add("Employee Name", -2, HorizontalAlignment.Left).Width = pnlEmployees.Width - 10
        End With
    End Sub

    Private Sub LoadfgClocks()
        Dim objClock As Clock
        Dim introw As Integer
        Dim intselRow As Integer
        Dim intselCol As Integer

        'save the selected cell before load to restore after load 
        With fgClocks
            intselCol = .ColSel
            intselRow = .RowSel
        End With

        introw = fgClocks.Rows.Fixed
        With fgClocks
            .SuspendLayout()
            .Rows.Count = .Rows.Fixed   'to clear data rows without losing column header fixed row
            .Rows.Count = .Rows.Fixed + AllClocks.Count
        End With
        'LoadPaymentGridHeader()
        For Each objClock In AllClocks
            RefreshfgClockRow(introw, objClock)
            introw += 1
        Next
        'restore the selected cell before load 
        With fgClocks
            If .Rows.Count > .Rows.Fixed Then
                .Sort(C1.Win.C1FlexGrid.SortFlags.Descending, 0, 1)
            End If
            .Select(intselRow, intselCol)
            .ShowCell(intselRow, intselCol)
            .ResumeLayout()
        End With
        'EnableDisableFormBtns()
    End Sub

    Private Sub RefreshfgClockRow(ByVal introw As Integer, ByVal objClock As Clock)
        Dim row As C1.Win.C1FlexGrid.Row

        With fgClocks
            .Rows.Item(introw).UserData = AllClocks.IndexOf(objClock)
            '.Item(introw, 0) = introw - .Rows.Fixed + 1
            '.Item(introw, 1) = SelectedEmployee.EmployeeFullName
            .Item(introw, 0) = objClock.ClockTimeIn.Date
            .Item(introw, 1) = objClock.ClockTimeIn.ToShortTimeString
            If objClock.ClockTimeOut = DateTime.MinValue Then
                '.Item(introw, 4) = " "
            Else
                .Item(introw, 2) = objClock.ClockTimeOut.ToShortTimeString
            End If
            .Item(introw, 3) = objClock.TimeWorkedDisplay

            Select Case objClock.DEError
                Case Clock.enumDEError.TimeInAfterTimeOut
                    .Item(introw, 4) = "Clock Out Time must be AFTER Clock In Time."
                    fgClocks.SetCellStyle(introw, 4, fgClocks.Styles.Item("DEError"))
                Case Clock.enumDEError.NoError
                    .Item(introw, 4) = String.Empty
            End Select

            Select Case objClock.ClockStatus
                Case Clock.enumClockStatus.OverriddenOld
                    fgClocks.SetCellStyle(introw, 1, fgClocks.Styles.Item("OverridenOld"))
                    fgClocks.SetCellStyle(introw, 2, fgClocks.Styles.Item("OverridenOld"))
                Case Clock.enumClockStatus.OverriddenNew
                    fgClocks.SetCellStyle(introw, 1, fgClocks.Styles.Item("OverridenNew"))
                    fgClocks.SetCellStyle(introw, 2, fgClocks.Styles.Item("OverridenNew"))
                Case Clock.enumClockStatus.Active
                    'nop
                Case Clock.enumClockStatus.Voided
                    fgClocks.Rows.Item(introw).Style = fgClocks.Styles.Item("VoidItem")
            End Select
        End With
    End Sub

    'Private Sub frmClock_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
    '    Dim frmOverride As frmClockManagerOverride
    '    Dim OverrideResult As DialogResult

    '    Select Case e.KeyCode
    '        Case Is = Keys.F2
    '            e.Handled = True
    '            Select Case ClockToOverride Is Nothing
    '                Case True
    '                    MsgBox("You need to click a clock item you want to override. Please try again.", MsgBoxStyle.Critical)
    '                    RaiseEvent StatusChanged("You need to click a clock item you want to override.")
    '                Case False
    '                    frmOverride = New frmClockManagerOverride()
    '                    Me.AddOwnedForm(frmOverride)
    '                    RaiseEvent StatusChanged("For Mangers Only:Click on your name,enter your password to override clock.")
    '                    OverrideResult = frmOverride.ShowDialog()

    '                    Select Case OverrideResult
    '                        Case DialogResult.OK, DialogResult.Yes
    '                            RefreshScreen()
    '                        Case Else
    '                            'NOP
    '                    End Select
    '            End Select
    '        Case Else
    '            'NOP
    '    End Select
    'End Sub

    Friend Property AllClocks() As Clocks
        Get
            Return m_Clocks
        End Get
        Set(ByVal Value As Clocks)
            m_Clocks = Value
        End Set
    End Property

    Private Sub lstviewEmployee_ItemActivate(ByVal sender As System.Object, ByVal e As System.EventArgs) _
    Handles lstviewEmployee.ItemActivate

        Dim latestClockIn As DateTime
        Dim latestClockOut As DateTime

        Dim lstIndex As Integer

        'fgClocks.Items.Clear()
        If lstviewEmployee.SelectedIndices.Count > 0 Then
            lstIndex = lstviewEmployee.SelectedIndices.Item(0)
            SelectedEmployee = SaleDays.CreateSaleDay.ActiveEmployees(Employees.EnumView.CompleteView).Item(lstIndex)
            txtName.Text = SelectedEmployee.EmployeeFullName
            fgClocks.Visible = True
            Me.AcceptButton = Nothing
            'txtPassword.Focus()

            AllClocks = New Clocks(SelectedEmployee.EmployeeId)

            LoadfgClocks()
            RaiseEvent StatusChanged("Enter your password to finish clocking in or out.")

            txtPassword.Focus()
        End If
    End Sub
    Private Property SelectedEmployee() As Employee
        Get
            Return m_Employee
        End Get
        Set(ByVal Value As Employee)
            m_Employee = Value
        End Set
    End Property
    Sub PaintAlternatingBackColor(ByVal lstview As ListView)
        Dim item As ListViewItem
        Dim subItem As ListViewItem.ListViewSubItem

        For Each item In lstview.Items
            If (item.Index Mod 2) = 0 Then
                item.BackColor = Color.LightGray
            Else
                item.BackColor = Color.LightYellow
            End If

            For Each subItem In item.SubItems

                subItem.BackColor = item.BackColor
            Next
        Next

    End Sub

    Friend Property ClockToOverride() As Clock
        Get
            Return m_ClockToOverride
        End Get
        Set(ByVal Value As Clock)
            m_ClockToOverride = Value
        End Set
    End Property

    Private Sub fgClocks_ItemActivate(ByVal sender As Object, ByVal e As System.EventArgs)
        'Handles fgClocks.ItemActivate
        Dim objOldClock As Clock
        Dim objNewClock As Clock
        Dim lstIndex As Integer

        'lstIndex = fgClocks.SelectedIndices.Item(0)
        ClockToOverride = AllClocks.SelectedItem(lstIndex)
    End Sub

    'Private Sub btnOKCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Select Case btnOKCancel.Text
    '        Case "OK"
    '            'btnOK_Click(sender, e)
    '        Case "Cancel"
    '            'btnCancel_Click(sender, e)
    '        Case Else
    '            'NOP
    '    End Select
    '    m_Employee = Nothing
    '    AllClocks = Nothing
    '    m_ClockToOverride = Nothing
    '    Me.Close()
    'End Sub

    Private Sub RefreshScreen()
        Dim intSQLReturnValue As Integer

        intSQLReturnValue = AllClocks.SetData()
        If intSQLReturnValue < 0 Then
            MsgBox("SQL Server Error - Please Clock in again", MsgBoxStyle.Critical, "System Error")
            RaiseEvent StatusChanged("SQL Server Error - Please Clock in again")
        Else
            LoadfgClocks()
            'Me.AcceptButton = btnOKCancel
            lstviewEmployee.Focus()
            RaiseEvent StatusChanged("Click on your name to clock in or out")
        End If
    End Sub

    Private Sub txtPassword_textChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
                Handles txtPassword.TextChanged

        Select Case txtPassword.Text.Length = SelectedEmployee.EmployeePassword.Length
            Case True       'Password entered fully 
                Select Case InStr(SelectedEmployee.EmployeePassword, txtPassword.Text, CompareMethod.Binary)
                    Case 1      'Password is correct
                        ClockInOut()
                    Case Else   'Password is wrong.
                        MsgBox("Password entered is not correct. Please try again.", MsgBoxStyle.Critical, "What You Need to Do")
                End Select
                txtPassword.Clear()
            Case False      'Password is being entered
                'NOP: wait till user finishes entering 
        End Select
    End Sub

    Private Sub ClockInOut()
        Dim TimeNow As DateTime

        TimeNow = SaleDays.CreateSaleDay.Now
        With AllClocks.LatestClock
            Select Case .ClockInOut     'dictates user is allowed to clock in, out or none of the two
                Case Clock.enumClockInOut.ClockIn
                    .EmployeeId = SelectedEmployee.EmployeeId
                    .ClockTimeIn = TimeNow
                    .ClockSaleDate = TimeNow.Date

                Case Clock.enumClockInOut.ClockOut
                    .ClockTimeOut = TimeNow

                Case Clock.enumClockInOut.InValid
                    MsgBox("System Error. Please close and open this screen again.", MsgBoxStyle.Critical, "System Error")
            End Select
        End With
        RefreshScreen()

    End Sub
    '**************************** New code
    Private Sub SetfgClocksStyles()
        Dim cs As C1.Win.C1FlexGrid.CellStyle
        Dim rg As C1.Win.C1FlexGrid.CellRange
        cs = fgClocks.Styles.Add("Header")
        With cs
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 12.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.LightBlue
            .ForeColor = Color.DarkBlue
        End With

        cs = fgClocks.Styles.Add("Void")
        With cs
            .Font = New Font(Me.Font, FontStyle.Strikeout)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ForeColor = Color.Red
        End With

        cs = fgClocks.Styles.Add("OverridenOld")
        With cs
            .Font = New Font(Me.Font, FontStyle.Strikeout)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ForeColor = Color.Red
        End With

        cs = fgClocks.Styles.Add("OverridenNew")
        With cs
            .Font = New Font(Me.Font, FontStyle.Bold)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .ForeColor = Color.Green
        End With

        cs = fgClocks.Styles.Add("DEError")
        With cs
            '.Font = New Font(Me.Font, FontStyle.Bold)
            '.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .BackColor = Color.Red
            .ForeColor = Color.Black
        End With
    End Sub

    Private Sub SetOverrideClock()
        dteClockOverride = New DateTimePicker()
        With dteClockOverride
            .Visible = False
            .Format = DateTimePickerFormat.Custom
            .CustomFormat = "hh:mm tt"
            .Size = New Size(fgClocks.Cols.Item("ClockInTime").Width, 60)
        End With
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub fgClocks_CellButtonClick(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgClocks.CellButtonClick
        Select Case e.Col
            Case 1, 2
                With dteClockOverride
                    .Location = New Point(fgClocks.Location.X + fgClocks.Cols.Item(fgClocks.ColSel).Left, fgClocks.Location.Y + fgClocks.Rows.Item(fgClocks.RowSel).Top)

                    .Visible = True
                    Select Case e.Col
                        Case 1
                            .Value = ClockToOverride.ClockTimeIn
                            '.MaxDate = ClockToOverride.ClockTimeIn.Date
                            '.MinDate = ClockToOverride.ClockTimeIn.Date
                        Case 2
                            .Value = ClockToOverride.ClockTimeOut
                            '.MaxDate = ClockToOverride.ClockTimeOut.Date
                            '.MinDate = ClockToOverride.ClockTimeOut.Date
                    End Select
                    .BringToFront()
                    .Focus()
                End With
            Case Else
                'nop : nothing to edit
        End Select
    End Sub

    Private Sub dteClockOverride_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dteClockOverride.Leave
        Dim objOldClock As Clock
        Dim objNewClock As Clock
        objOldClock = AllClocks.Item(CInt(fgClocks.Rows.Item(fgClocks.RowSel).UserData))
        objNewClock = AllClocks.AddClock

        With objNewClock
            .EmployeeId = objOldClock.EmployeeId
            .ClockTimeIn = objOldClock.ClockTimeIn
            .ClockTimeOut = objOldClock.ClockTimeOut
            .ClockStatus = Clock.enumClockStatus.OverriddenNew
            .ClockSaleDate = objOldClock.ClockSaleDate
            Select Case fgClocks.ColSel
                Case 1
                    .ClockTimeIn = dteClockOverride.Value
                Case 2
                    .ClockTimeOut = dteClockOverride.Value
            End Select


        End With
        With objOldClock
            .ClockStatus = Clock.enumClockStatus.OverriddenOld
        End With
        dteClockOverride.Visible = False
        RefreshScreen()
    End Sub


    Private Sub fgClocks_StartEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles fgClocks.StartEdit
        m_ClockToOverride = AllClocks.Item(CInt(fgClocks.Rows.Item(fgClocks.RowSel).UserData))
        Select Case m_ClockToOverride.AllowClockEdit
            Case True

            Case False
                'ignore edit attempt
                e.Cancel = True
        End Select
    End Sub

    Private Sub OnOverrideNeeded(ByVal objClockOverride As BOPr.Override) Handles m_ClockToOverride.OverrideNeeded
        'create override form, pass override object and get override from user
        'If user gives an override, the order object, which is waiting for this event to return
        ' proceeds with providing (e.g. KOT, void etc.) whatever user asked for   
        Dim newfrmPOSOverride As New frmPOSOverride()
        newfrmPOSOverride.newOverride = objClockOverride
        newfrmPOSOverride.ShowDialog()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        ClockToOverride = Nothing
        AllClocks = Nothing
        Me.Close()
    End Sub
End Class


