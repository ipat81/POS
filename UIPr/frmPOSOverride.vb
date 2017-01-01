Option Strict On
Imports System.Windows.Forms
Imports BOPr
Public Class frmPOSOverride
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
        components = New System.ComponentModel.Container()
        Me.Text = "frmPOSOverride"
    End Sub

#End Region


    Private lblDialogTitle As Label
    Private lblOverrideTypeText As Label
    Private lblName As Label
    Private lblPassword As Label
    Private lblReason As Label
    Private WithEvents cboReason As ComboBox
    Private WithEvents txtPassword As TextBox
    Private WithEvents btnCancel As Button
    Private WithEvents cboManagers As ComboBox
    Private WithEvents btnOk As Button

    Private m_ManagersOrOwners As Employees
    Private m_Override As Override
    Private m_ManagerOrOwner As Employee

    Private Sub SetUI()
        'Me.AutoScale = True
        'Me.SuspendLayout()
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(440, 274)
        Me.Name = "frmPOSOverride"
        Me.Text = "frmPOSOverride"
        Me.ControlBox = False
        Me.BackColor = Color.LightGray
        Me.FormBorderStyle = FormBorderStyle.None
        Me.CenterToScreen()
        Me.Text = "Give an Override"
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        lblDialogTitle = New Label()
        With lblDialogTitle
            .Dock = DockStyle.Top
            .Height = 30
            '.Size = New Size(100, 25)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.DarkRed
            .ForeColor = Color.White
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 10.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.MiddleCenter
            .Text = "Override Required"
        End With

        lblOverrideTypeText = New Label()
        With lblOverrideTypeText
            .Dock = DockStyle.Top
            .Height = 60
            .Location = New Point(0, 30)
            '.Size = New Size(100, 75)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Me.BackColor
            .ForeColor = Color.DarkBlue
            .Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            .TextAlign = ContentAlignment.TopCenter
            .Text = newOverride.OverrideTypeText
        End With
        lblName = New Label()
        With lblName
            .Location = New Point(30, lblOverrideTypeText.Location.Y + lblOverrideTypeText.Height + 10)
            .Size = New Size(100, 25)
            .FlatStyle = FlatStyle.Flat
            .BackColor = Me.BackColor
            .TextAlign = ContentAlignment.MiddleRight
            Select Case newOverride.OverrideLevelNeeded
                Case Override.enumOverrideLevel.ManagerNeeded
                    .Text = "Manager"
                Case Override.enumOverrideLevel.OwnerNeeded
                    .Text = "Owner"
                Case Else
                    .Text = "Manager"
            End Select
        End With

        cboManagers = New ComboBox()
        With cboManagers
            .Location = New Point(lblName.Location.X + lblName.Width + 10, lblName.Location.Y)
            .Size = New Size(lblName.Width + 30, lblName.Size.Height)

            .DropDownStyle = ComboBoxStyle.DropDownList
        End With
        LoadManagers()

        lblPassword = New Label()
        With lblPassword
            .Location = New Point(lblName.Location.X, lblName.Location.Y + lblName.Height + 20)
            .Size = New Size(lblName.Size.Width, lblName.Size.Height)
            .TextAlign = ContentAlignment.MiddleRight
            .FlatStyle = FlatStyle.Flat
            .BackColor = Me.BackColor
            .Text = "Password"
        End With

        txtPassword = New TextBox()
        With txtPassword
            .Location = New Point(lblPassword.Location.X + lblPassword.Width + 10, lblPassword.Location.Y)
            .Size = New Size(lblName.Size.Width, lblName.Size.Height)
            .PasswordChar = Chr(42)
        End With

        lblReason = New Label()
        With lblReason
            .Location = New Point(lblPassword.Location.X, txtPassword.Location.Y + txtPassword.Height + 20)
            .Size = New Size(lblName.Size.Width, lblName.Size.Height)

            .FlatStyle = FlatStyle.Flat
            .BackColor = Me.BackColor
            .TextAlign = ContentAlignment.MiddleRight
            .Text = "Why Override?"
        End With

        cboReason = New ComboBox()
        With cboReason
            .Location = New Point(lblReason.Location.X + lblReason.Width + 10, lblReason.Location.Y)
            .Size = New Size(lblName.Size.Width + 150, lblName.Size.Height)
            .DropDownStyle = ComboBoxStyle.DropDown
        End With
        LoadcboReasons()

        btnOk = New Button()
        With btnOk
            .Location = New Point(cboReason.Location.X, cboReason.Location.Y + cboReason.Height + 20)
            .Size = New Size(60, 40)
            .Text = "OK"
            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.Blue
            .ForeColor = Color.White
            .Enabled = False
        End With

        btnCancel = New Button()
        With btnCancel
            .Location = New Point(btnOk.Location.X + btnOk.Width + 20, btnOk.Location.Y)
            .Size = New Size(60, 40)
            .Text = "Cancel"

            .FlatStyle = FlatStyle.Flat
            .BackColor = Color.Blue
            .ForeColor = Color.White
            .Enabled = True
        End With
        'Me.ResumeLayout()
    End Sub

    Private Sub LoadManagers()
        Dim objEmployee As Employee

        Select Case newOverride.OverrideLevelNeeded
            Case Override.enumOverrideLevel.ManagerNeeded
                m_ManagersOrOwners = SaleDays.CreateSaleDay.ActiveManagers()
            Case Override.enumOverrideLevel.OwnerNeeded
                'm_ManagersOrOwners = SaleDays.CreateSaleDay.ActiveOwners()
                m_ManagersOrOwners = SaleDays.CreateSaleDay.ActiveOwners
            Case Else
                m_ManagersOrOwners = SaleDays.CreateSaleDay.ActiveManagers()
        End Select
        With cboManagers
            .BeginUpdate()
            For Each objEmployee In m_ManagersOrOwners
                .Items.Add(objEmployee.EmployeeFullName)
            Next
            .EndUpdate()
        End With
    End Sub

    Friend Property newOverride() As Override
        Get
            Return m_Override
        End Get
        Set(ByVal Value As Override)
            m_Override = Value
        End Set
    End Property

    Private Sub LoadcboReasons()
        With cboReason
            .BeginUpdate()
            Select Case newOverride.OverrideType
                Case Override.enumOverrideType.ClockEdit
                    .Items.Add("Forgot to clock")
                    .Items.Add("Computer not available")
                    .Items.Add("I was working offsite")
                Case Override.enumOverrideType.EditPayment
                    .Items.Add("Wrong Payment was entered for this Order.")
                    .Items.Add("Customer changed the Payment.")
                    .Items.Add("I want to correct the Tip entered before.")
                    .Items.Add("Found a Missing Payment")
                    .Items.Add("Entering Payment Received but not Entered")
                Case Override.enumOverrideType.PrintGuestCheck
                    .Items.Add("Lost Check")
                    .Items.Add("Customer took all the copies.")
                    .Items.Add("Customer asked for a Copy.")
                    .Items.Add("Customer did not GET some items on the check.")
                    .Items.Add("Customer did not LIKE some items on the check.")
                    .Items.Add("Customer added to Order after Check was given.")
                    .Items.Add("I printed the check too soon before Customer asked for it.")
                    .Items.Add("I want to add some items that I forgot to enter.")

                Case Override.enumOverrideType.PrintKOT
                    .Items.Add("KOT was lost")
                    .Items.Add("I printed wrong KOT")
                    .Items.Add("Want an extra copy")

                Case Override.enumOverrideType.EditMoneyCount
                    .Items.Add("Found a Missing Payment")
                    .Items.Add("Entering Payment Received but not Entered")

                Case Override.enumOverrideType.ReconciliationReportApproval
                    .Items.Add("Cash Register matches. I want to close Lunch/Dinner.")
                    .Items.Add("Cash Register DOES NOT match & I do not know why. But I have to close Lunch/Dinner.")

                Case Override.enumOverrideType.ReconciliationCancelApproval
                    .Items.Add(" I want to correcr a Payment Error made in closing an Order. ")
                    .Items.Add("I want to correct Counting error.")

                Case Override.enumOverrideType.MarkPaymentReceivedButNotEntered
                    .Items.Add("It is a CASH payment & I checked it is correct")
                    .Items.Add("It is a CREDIT CARD payment & I checked it is correct")
                    .Items.Add("It is a CHECK payment & I checked it is correct")
                    .Items.Add("It is a Gift Certificate payment & I checked it is correct")
                    .Items.Add("It is a MIXED payment & I checked it is corect")

                Case Override.enumOverrideType.EditCRTXN
                    .Items.Add("Petty Cash Expense")
                    .Items.Add("Salary Advance")
                    .Items.Add("Purchase Advance")
                    .Items.Add("Deposit to Office Safe")

                Case Override.enumOverrideType.AddOrderItem
                    .Items.Add("Customer added to Order after Check was given.")
                    .Items.Add("I printed the check too soon before Customer asked for it.")
                    .Items.Add("I want to add some items that I forgot to enter.")
                    .Items.Add("I want to add standard Tips that I forgot to enter.")
                    .Items.Add("I want to give discount that I forgot to enter.")
                    .Items.Add("Customer gave me discount coupon after I printed the check.")

                Case Override.enumOverrideType.EditOrderItem
                    .Items.Add("Customer did not GET some items on the check.")
                    .Items.Add("Customer did not LIKE some items on the check.")
                    .Items.Add("Customer did not LIKE some items on the check.")
                Case Override.enumOverrideType.VoidOrderItem
                    .Items.Add("Customer did not GET some items on the check.")
                    .Items.Add("Customer did not LIKE some items on the check.")
                    .Items.Add("We had run out of some items on the check.")
                    .Items.Add("I added some wrong items to the check.")
            End Select

            .EndUpdate()
        End With
    End Sub

    Private Sub frmPOSOverride_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.SuspendLayout()
        SetUI()
        Me.Controls.AddRange(New Control() _
        {lblName, lblOverrideTypeText, cboManagers, lblPassword, txtPassword, lblReason, cboReason, btnOk, btnCancel, lblDialogTitle})
        Me.ResumeLayout()
    End Sub

    Private Sub cboManagers_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboManagers.SelectedIndexChanged
        With cboManagers
            If .SelectedIndex >= 0 Then
                m_ManagerOrOwner = m_ManagersOrOwners.Item(.SelectedIndex)
            Else
                m_ManagerOrOwner = Nothing
            End If
        End With
        EnableDisableBtnOK()
    End Sub

    Private Sub EnableDisableBtnOK()
        If (m_ManagerOrOwner Is Nothing) OrElse _
               Not (m_ManagerOrOwner.EmployeePassword = txtPassword.Text) OrElse _
                (cboReason.Text = String.Empty) Then
            btnOk.Enabled = False
        Else
            btnOk.Enabled = True
        End If
    End Sub

    Private Sub txtPassword_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPassword.Leave
        EnableDisableBtnOK()
    End Sub

    Private Sub cboReason_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) _
                Handles cboReason.SelectedIndexChanged, cboReason.TextChanged
        EnableDisableBtnOK()
    End Sub

    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        With newOverride
            .OverrideAvailable = True
            .OverrideBy = m_ManagerOrOwner.EmployeeId
            .OverrideReason = cboReason.Text
            .OverrideStatus = Override.enumOverrideStatus.Active
            .SetData()
        End With
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        With newOverride
            .OverrideAvailable = False
            .OverrideStatus = Override.enumOverrideStatus.InActive
        End With
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class

