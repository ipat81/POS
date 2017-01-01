Option Strict On
Imports System.Data.SqlClient
Imports System.IO

Public Class SaleDay
    Inherits POSBO

    Public Enum enumSaleDayType As Integer
        Regular
        [Partial]
        Holiday
        BadWeather
    End Enum
    Public Event CashierChanged(ByVal newcashier As Employee)
    Public Event MenuChanged(ByVal newMenu As Menu)

    Private m_SaleDayId As Integer
    Private m_SaleDate As DateTime
    Private m_SaleDayOpenAt As DateTime
    Private m_SaleDayOpenBy As Integer
    Private m_SaleDayCloseAt As DateTime
    Private m_SaleDayCloseBy As Integer
    Private m_SaleDayType As enumSaleDayType
    Private m_IsDirty As Boolean
    Private m_IsPersisted As Boolean

    Private Shared m_SaleDay As SaleDay
    Private m_PriorSaleDay As SaleDay

    Private m_TodaysCashiers As Employees
    Private m_CurrentCashier As Employee
    Private m_Employees As Employees
    Private m_Customers As Customers
    Private m_Cashiers As Employees
    Private m_ActiveTables As DTables
    Private m_Managers As Employees
    Private m_CurrentOrder As Order
    Private m_CurrentOverride As Override
    Private m_CRId As Integer

    Private m_CurrentMenu As Menu
    Private m_CurrentMenuId As Integer

    Private m_LastOrderId As Integer


    Private m_TodaysCRTXNs As CRTXNs
    Private m_ToDaysEmployeeClocks As Clocks

    Private m_SaleDaySessions As SaleDaySessions
    Private m_Sessions As Sessions
    Private m_CurrentSaleDaySession As SaleDaySession

    Private m_ReceiptFooterMsg As String = "We appreciate and thank you for your business."

    Private m_NonCurrentAmexCount As Integer
    Private m_NonCurrentAmexAmount As Double
    Private m_NonCurrentAmexTipAmountPaid As Double

    Private m_NonCurrentCashCount As Integer
    Private m_NonCurrentCashAmount As Double
    Private m_NonCurrentCashTipAmountPaid As Double

    Private m_NonCurrentDebitCardCount As Integer
    Private m_NonCurrentDebitCardAmount As Double
    Private m_NonCurrentDebitCardTipAmountPaid As Double

    Private m_NonCurrentDinersClubCount As Integer
    Private m_NonCurrentDinersClubAmount As Double
    Private m_NonCurrentDinersClubTipAmountPaid As Double

    Private m_NonCurrentDiscoverCount As Integer
    Private m_NonCurrentDiscoverAmount As Double
    Private m_NonCurrentDiscoverTipAmountPaid As Double

    Private m_NonCurrentHouseAccountCount As Integer
    Private m_NonCurrentHouseAccountAmount As Double
    Private m_NonCurrentHouseAccountTipAmountPaid As Double

    Private m_NonCurrentKnightExpressCount As Integer
    Private m_NonCurrentKnightExpressAmount As Double
    Private m_NonCurrentKnightExpressTipAmountPaid As Double

    Private m_NonCurrentMasterCardCount As Integer
    Private m_NonCurrentMasterCardAmount As Double
    Private m_NonCurrentMasterCardTipAmountPaid As Double

    Private m_NonCurrentMGGiftCertificateCount As Integer
    Private m_NonCurrentMGGiftCertificateAmount As Double
    Private m_NonCurrentMGGiftCertificateTipAmountPaid As Double

    Private m_NonCurrentMissingPaymentCount As Integer
    Private m_NonCurrentMissingPaymentAmount As Double
    Private m_NonCurrentMissingPaymentTipAmountPaid As Double

    Private m_NonCurrentPersonalCheckCount As Integer
    Private m_NonCurrentPersonalCheckAmount As Double
    Private m_NonCurrentPersonalCheckTipAmountPaid As Double

    Private m_NonCurrentTravellersChecksCount As Integer
    Private m_NonCurrentTravellersChecksAmount As Double
    Private m_NonCurrentTravellersChecksTipAmountPaid As Double

    Private m_NonCurrentVisaCount As Integer
    Private m_NonCurrentVisaAmount As Double
    Private m_NonCurrentVisaTipAmountPaid As Double

    Friend Sub New()
        SetPhysicalCRId()
    End Sub

    Friend Sub New(ByVal targetSaledate As Date)
        m_SaleDate = targetSaledate.Date
        SetPhysicalCRId()
    End Sub

    Public ReadOnly Property CurrentOrder() As Order
        Get
            Return m_CurrentOrder
        End Get
    End Property

    Public ReadOnly Property LastOrderId() As Integer
        Get

            Dim i As Integer
            Dim objLastOrder As Order
            Dim strSqlCmd As String
            strSqlCmd = " Select Max(OrderId) From Order "
            Return DataAccess.CreateDataAccess.GetOrderIdAsInteger(strSqlCmd)

        End Get
    End Property

    Public ReadOnly Property IsOrderInProgress() As Boolean
        Get
            If CurrentOrder Is Nothing Then
                Return False
            Else
                If CurrentOrder.AllOrderItems.Count > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Get

    End Property

    Public Sub SetCurrentOrder(ByVal objOrder As Order)
        m_CurrentOrder = objOrder
    End Sub

    Public ReadOnly Property ActiveEmployees(ByVal enumEmployeeView As Employees.EnumView) As Employees
        Get
            If m_Employees Is Nothing Then m_Employees = New Employees(Employees.EnumFilter.Active, enumEmployeeView)
            Return m_Employees
        End Get
    End Property

    Public ReadOnly Property ActiveTables() As DTables
        Get
            If m_ActiveTables Is Nothing Then m_ActiveTables = New DTables(DTables.EnumFilter.Active, DTables.EnumView.CompleteView)
            Return m_ActiveTables
        End Get
    End Property

    Public ReadOnly Property ActiveCashiers() As Employees
        Get
            If m_Cashiers Is Nothing Then m_Cashiers = New Employees(Employees.EnumFilter.ActiveCashiers, Employees.EnumView.CompleteView)
            Return m_Cashiers
        End Get
    End Property

    Public ReadOnly Property ActiveCustomers() As Customers
        Get
            If m_Customers Is Nothing Then m_Customers = New Customers(Customers.EnumFilter.Active, Customers.EnumView.CompleteView)
            Return m_Customers
        End Get
    End Property
    Public ReadOnly Property ActiveManagers() As Employees
        Get
            If m_Managers Is Nothing Then m_Managers = New Employees(Employees.EnumFilter.ActiveManagers, Employees.EnumView.CompleteView)
            Return m_Managers
        End Get
    End Property

    Public ReadOnly Property ActiveSessions() As Sessions
        Get
            If m_Sessions Is Nothing Then m_Sessions = New Sessions(Sessions.EnumFilter.Active, Sessions.EnumView.CompleteView)
            Return m_Sessions
        End Get
    End Property

    Public ReadOnly Property AllSaleDaySessions() As SaleDaySessions
        Get
            If m_SaleDaySessions Is Nothing Then _
                m_SaleDaySessions = New SaleDaySessions(Me.SaleDate, _
                                    SaleDaySessions.EnumSessionFilter.All, _
                                    SaleDaySessions.EnumView.CompleteView)
            Return m_SaleDaySessions
        End Get
    End Property

    Public ReadOnly Property PriorSaleDay() As SaleDay
        Get
            Dim PriorDate As Date
            If m_PriorSaleDay Is Nothing Then
                PriorDate = Me.SaleDate.Subtract(System.TimeSpan.FromDays(1))
                m_PriorSaleDay = SaleDays.CreateSaleDay.Item(PriorDate, False)
            End If
            Return m_PriorSaleDay
        End Get
    End Property

    Public Sub SetAllNonCurrentPayments(ByVal fromTime As DateTime, ByVal ToTime As DateTime)
        Dim objAllNonCurrentPayments As Payments
        'Returns all Payments entered on this Saleday for Orders of Prior or future Saledays
        'This is required to reconcile Cash Register
        'This must be called ONCE before accesing individual Payments for EACH SDSession or Day with required from & To time
        Dim objPayment As Payment

        objAllNonCurrentPayments = New Payments(Payments.EnumFilter.Active, Payments.EnumView.CompleteView, Me.SaleDate)
        m_NonCurrentAmexCount = 0
        m_NonCurrentAmexAmount = 0
        m_NonCurrentAmexTipAmountPaid = 0

        m_NonCurrentCashCount = 0
        m_NonCurrentCashAmount = 0
        m_NonCurrentCashTipAmountPaid = 0

        m_NonCurrentDebitCardCount = 0
        m_NonCurrentDebitCardAmount = 0
        m_NonCurrentDebitCardTipAmountPaid = 0

        m_NonCurrentDinersClubCount = 0
        m_NonCurrentDinersClubAmount = 0
        m_NonCurrentDinersClubTipAmountPaid = 0

        m_NonCurrentDiscoverCount = 0
        m_NonCurrentDiscoverAmount = 0
        m_NonCurrentDiscoverTipAmountPaid = 0

        m_NonCurrentHouseAccountCount = 0
        m_NonCurrentHouseAccountAmount = 0
        m_NonCurrentPersonalCheckTipAmountPaid = 0

        m_NonCurrentKnightExpressCount = 0
        m_NonCurrentKnightExpressAmount = 0
        m_NonCurrentKnightExpressTipAmountPaid = 0

        m_NonCurrentMasterCardCount = 0
        m_NonCurrentMasterCardAmount = 0
        m_NonCurrentMasterCardTipAmountPaid = 0

        m_NonCurrentMGGiftCertificateCount = 0
        m_NonCurrentMGGiftCertificateAmount = 0
        m_NonCurrentMGGiftCertificateTipAmountPaid = 0

        m_NonCurrentMissingPaymentCount = 0
        m_NonCurrentMissingPaymentAmount = 0
        m_NonCurrentMissingPaymentTipAmountPaid = 0

        m_NonCurrentPersonalCheckCount = 0
        m_NonCurrentPersonalCheckAmount = 0
        m_NonCurrentPersonalCheckTipAmountPaid = 0

        m_NonCurrentTravellersChecksCount = 0
        m_NonCurrentTravellersChecksAmount = 0
        m_NonCurrentTravellersChecksTipAmountPaid = 0

        m_NonCurrentVisaCount = 0
        m_NonCurrentVisaAmount = 0
        m_NonCurrentVisaTipAmountPaid = 0

        For Each objPayment In objAllNonCurrentPayments
            With objPayment
                If (.EndTime > fromTime) AndAlso (.EndTime < ToTime) AndAlso _
                    (.Status = Payment.enumStatus.Active) Then
                    Select Case .PaymentMethod
                        Case Payment.enumPaymentMethod.Amex
                            m_NonCurrentAmexCount += 1
                            m_NonCurrentAmexAmount += .POSAmountPaid
                            m_NonCurrentAmexTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.Cash
                            m_NonCurrentCashCount += 1
                            m_NonCurrentCashAmount += .POSAmountPaid
                            m_NonCurrentCashTipAmountPaid += +.TipAmountPaid
                        Case Payment.enumPaymentMethod.DebitCard
                            m_NonCurrentDebitCardCount += 1
                            m_NonCurrentDebitCardAmount += .POSAmountPaid
                            m_NonCurrentDebitCardTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.DinersClub
                            m_NonCurrentDinersClubCount += 1
                            m_NonCurrentDinersClubAmount += .POSAmountPaid
                            m_NonCurrentDinersClubTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.Discover
                            m_NonCurrentDiscoverCount += 1
                            m_NonCurrentDiscoverAmount += .POSAmountPaid
                            m_NonCurrentDiscoverTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.HouseAccount
                            m_NonCurrentHouseAccountCount += 1
                            m_NonCurrentHouseAccountAmount += .POSAmountPaid
                            m_NonCurrentHouseAccountTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.KnightExpress
                            m_NonCurrentKnightExpressCount += 1
                            m_NonCurrentKnightExpressAmount += .POSAmountPaid
                            m_NonCurrentKnightExpressTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.MasterCard
                            m_NonCurrentMasterCardCount += 1
                            m_NonCurrentMasterCardAmount += .POSAmountPaid
                            m_NonCurrentMasterCardTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.MGGiftCertificate
                            m_NonCurrentMGGiftCertificateCount += 1
                            m_NonCurrentMGGiftCertificateAmount += .POSAmountPaid
                            m_NonCurrentMGGiftCertificateTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.MissingPayment
                            m_NonCurrentMissingPaymentCount += 1
                            m_NonCurrentMissingPaymentAmount += .POSAmountPaid
                            m_NonCurrentMissingPaymentTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.PersonalCheck
                            m_NonCurrentPersonalCheckCount += 1
                            m_NonCurrentPersonalCheckAmount += .POSAmountPaid
                            m_NonCurrentPersonalCheckTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.TravellersChecks
                            m_NonCurrentTravellersChecksCount += 1
                            m_NonCurrentTravellersChecksAmount += .POSAmountPaid
                            m_NonCurrentTravellersChecksTipAmountPaid += .TipAmountPaid
                        Case Payment.enumPaymentMethod.Visa
                            m_NonCurrentVisaCount += 1
                            m_NonCurrentVisaAmount += .POSAmountPaid
                            m_NonCurrentVisaTipAmountPaid += .TipAmountPaid
                    End Select
                End If
            End With
        Next
    End Sub

    Public ReadOnly Property NonCurrentAmexCount() As Integer
        Get
            Return m_NonCurrentAmexCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentAmexAmount() As Double
        Get
            Return m_NonCurrentAmexAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentAmexTipAmount() As Double
        Get
            Return m_NonCurrentAmexTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentAmexTotalAmount() As Double
        Get
            Return NonCurrentAmexAmount + NonCurrentAmexTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentCashCount() As Integer
        Get
            Return m_NonCurrentCashCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentCashAmount() As Double
        Get
            Return m_NonCurrentCashAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentCashTipAmount() As Double
        Get
            Return m_NonCurrentCashTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentCashTotalAmount() As Double
        Get
            Return NonCurrentCashAmount + NonCurrentCashTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDebitCardCount() As Integer
        Get
            Return m_NonCurrentDebitCardCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDebitCardAmount() As Double
        Get
            Return m_NonCurrentDebitCardAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDebitCardTipAmount() As Double
        Get
            Return m_NonCurrentDebitCardTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentDebitCardTotalAmount() As Double
        Get
            Return NonCurrentDebitCardAmount + NonCurrentDebitCardTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDinersClubCount() As Integer
        Get
            Return m_NonCurrentDinersClubCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDinersClubAmount() As Double
        Get
            Return m_NonCurrentDinersClubAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDinersClubTipAmount() As Double
        Get
            Return m_NonCurrentDinersClubTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentDinersClubTotalAmount() As Double
        Get
            Return NonCurrentDinersClubAmount + NonCurrentDinersClubTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDiscoverCount() As Integer
        Get
            Return m_NonCurrentDiscoverCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDiscoverAmount() As Double
        Get
            Return m_NonCurrentDiscoverAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentDiscoverTipAmount() As Double
        Get
            Return m_NonCurrentDiscoverTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentDiscoverTotalAmount() As Double
        Get
            Return NonCurrentDiscoverAmount + NonCurrentDiscoverTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentHouseAccountCount() As Integer
        Get
            Return m_NonCurrentHouseAccountCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentHouseAccountAmount() As Double
        Get
            Return m_NonCurrentHouseAccountAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentHouseAccountTipAmount() As Double
        Get
            Return m_NonCurrentHouseAccountTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentHouseAccountTotalAmount() As Double
        Get
            Return NonCurrentHouseAccountAmount + NonCurrentHouseAccountTipAmount
        End Get
    End Property
    '****
    Public ReadOnly Property NonCurrentKnightExpressCount() As Integer
        Get
            Return m_NonCurrentKnightExpressCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentKnightExpressAmount() As Double
        Get
            Return m_NonCurrentKnightExpressAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentKnightExpressTipAmount() As Double
        Get
            Return m_NonCurrentKnightExpressTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentKnightExpressTotalAmount() As Double
        Get
            Return NonCurrentKnightExpressAmount + NonCurrentKnightExpressTipAmount
        End Get
    End Property
    '****
    Public ReadOnly Property NonCurrentMasterCardCount() As Integer
        Get
            Return m_NonCurrentMasterCardCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMasterCardAmount() As Double
        Get
            Return m_NonCurrentMasterCardAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMasterCardTipAmount() As Double
        Get
            Return m_NonCurrentMasterCardTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentMasterCardTotalAmount() As Double
        Get
            Return NonCurrentMasterCardAmount + NonCurrentMasterCardTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMGGiftCertificateCount() As Integer
        Get
            Return m_NonCurrentMGGiftCertificateCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMGGiftCertificateAmount() As Double
        Get
            Return m_NonCurrentMGGiftCertificateAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMGGiftCertificateTipAmount() As Double
        Get
            Return m_NonCurrentMGGiftCertificateTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentMGGiftCertificateTotalAmount() As Double
        Get
            Return NonCurrentMGGiftCertificateAmount + NonCurrentMGGiftCertificateTipAmount
        End Get
    End Property
    '*****
    Public ReadOnly Property NonCurrentMissingPaymentCount() As Integer
        Get
            Return m_NonCurrentMissingPaymentCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMissingPaymentAmount() As Double
        Get
            Return m_NonCurrentMissingPaymentAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentMissingPaymentTipAmount() As Double
        Get
            Return m_NonCurrentMissingPaymentTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentMissingPaymentTotalAmount() As Double
        Get
            Return NonCurrentMissingPaymentAmount + NonCurrentMissingPaymentTipAmount
        End Get
    End Property
    '****
    Public ReadOnly Property NonCurrentPersonalCheckCount() As Integer
        Get
            Return m_NonCurrentPersonalCheckCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentPersonalCheckAmount() As Double
        Get
            Return m_NonCurrentPersonalCheckAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentPersonalCheckTipAmount() As Double
        Get
            Return m_NonCurrentPersonalCheckTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentPersonalCheckTotalAmount() As Double
        Get
            Return NonCurrentPersonalCheckAmount + NonCurrentPersonalCheckTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentTravellersChecksCount() As Integer
        Get
            Return m_NonCurrentTravellersChecksCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentTravellersChecksAmount() As Double
        Get
            Return m_NonCurrentTravellersChecksAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentTravellersChecksTipAmount() As Double
        Get
            Return m_NonCurrentTravellersChecksTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentTravellersChecksTotalAmount() As Double
        Get
            Return NonCurrentTravellersChecksAmount + NonCurrentTravellersChecksTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentVisaCount() As Integer
        Get
            Return m_NonCurrentVisaCount
        End Get
    End Property

    Public ReadOnly Property NonCurrentVisaAmount() As Double
        Get
            Return m_NonCurrentVisaAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentVisaTipAmount() As Double
        Get
            Return m_NonCurrentVisaTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property NonCurrentVisaTotalAmount() As Double
        Get
            Return NonCurrentVisaAmount + NonCurrentVisaTipAmount
        End Get
    End Property

    Public ReadOnly Property NonCurrentCreditCardTotalAmount() As Double
        Get
            With Me
                Return .NonCurrentAmexTotalAmount + .NonCurrentDebitCardTotalAmount + .NonCurrentDinersClubTotalAmount + .NonCurrentDiscoverTotalAmount _
                        + .NonCurrentKnightExpressTotalAmount + .NonCurrentMasterCardTotalAmount + .NonCurrentVisaTotalAmount
            End With
        End Get
    End Property

    Public ReadOnly Property NonCurrentOtherTotalAmount() As Double
        Get
            With Me
                Return .NonCurrentPersonalCheckTotalAmount _
                        + .NonCurrentTravellersChecksTotalAmount _
                        + .NonCurrentMGGiftCertificateTotalAmount _
                        + .NonCurrentHouseAccountTotalAmount
            End With
        End Get
    End Property

    Public Property SaleDayId() As Integer

        Get
            Return m_SaleDayId
        End Get

        Set(ByVal Value As Integer)
            m_SaleDayId = Value
        End Set
    End Property

    Public ReadOnly Property SaleDate() As DateTime
        Get
            Return m_SaleDate
        End Get
    End Property
    Friend Sub SetSaleDate(ByVal newSaleDate As Date)
        m_SaleDate = newSaleDate.Date
        MarkDirtyOrClean(True)
    End Sub

    Public ReadOnly Property SaleDayOpenAt() As DateTime
        Get
            Return m_SaleDayOpenAt
        End Get
    End Property
    Friend Sub SetSaleDayOpenAt(ByVal Value As DateTime)
        m_SaleDayOpenAt = Value
        MarkDirtyOrClean(True)
    End Sub

    Public ReadOnly Property SaleDayOpenBy() As Integer

        Get
            Return m_SaleDayOpenBy
        End Get
    End Property
    Friend Sub SetSaleDayOpenBy(ByVal Value As Integer)
        m_SaleDayOpenBy = Value
    End Sub

    Public ReadOnly Property SaleDayCloseAt() As DateTime
        Get
            Return m_SaleDayCloseAt
        End Get
    End Property
    Friend Sub SetSaleDayCloseAt(ByVal Value As DateTime)
        m_SaleDayCloseAt = Value
        MarkDirtyOrClean(True)
    End Sub


    Public ReadOnly Property SaleDayCloseBy() As Integer
        Get
            Return m_SaleDayCloseBy
        End Get
    End Property
    Friend Sub SetSaleDayCloseBy(ByVal Value As Integer)
        m_SaleDayCloseBy = Value
    End Sub


    Public ReadOnly Property SaleDayType() As enumSaleDayType
        Get
            Return m_SaleDayType
        End Get
    End Property
    Friend Sub SetSaleDayType(ByVal Value As enumSaleDayType)
        m_SaleDayType = Value
        MarkDirtyOrClean(True)
    End Sub

    Friend ReadOnly Property IsDirty() As Boolean
        Get
            Return m_IsDirty
        End Get

    End Property

    Private Sub MarkDirtyOrClean(ByVal blnIsDirty As Boolean)
        m_IsDirty = blnIsDirty
    End Sub

    Public Property CurrentCashier() As Employee
        Get
            Return m_CurrentCashier
        End Get
        Set(ByVal value As Employee)
            m_CurrentCashier = value
            RaiseEvent CashierChanged(m_CurrentCashier)
        End Set
    End Property

    Public Property CurrentMenu() As Menu
        Get
            Return m_CurrentMenu

        End Get
        Set(ByVal Value As Menu)
            m_CurrentMenu = Value
        End Set
    End Property

    Public Property CurrentMenuId() As Integer
        Get
            Return m_CurrentMenuId

        End Get
        Set(ByVal Value As Integer)
            m_CurrentMenuId = Value
        End Set
    End Property
    Public ReadOnly Property TodaysCashiers() As Employees
        Get
            If m_TodaysCashiers Is Nothing Then m_TodaysCashiers = New Employees()
            Return m_TodaysCashiers
        End Get
    End Property

    Friend Sub CashierSignOn(ByVal objCashier As Employee)
        If m_CurrentCashier Is Nothing Then
            Select Case TodaysCashiers.Contains(objCashier)
                Case True
                    'NOP
                Case Else
                    TodaysCashiers.Add(objCashier)
            End Select
            m_CurrentCashier = objCashier
        Else
            'raise exception: previous cashier must sign off
        End If
    End Sub

    Friend Sub CashierSignOff()
        If m_CurrentCashier Is Nothing Then
            'raise exception: no cashier has signed on
        Else
            TodaysCashiers.Remove(TodaysCashiers.IndexOf(m_CurrentCashier))
            m_CurrentCashier = Nothing
        End If
    End Sub

    Public ReadOnly Property OtherTotalAmountForDay() As Double
        Get
            With Me
                Return .PersonalCheckTotalAmountForDay _
                        + .TravellersChecksTotalAmountForDay _
                        + .MGGiftCertificateTotalAmountForDay _
                        + .HouseAccountTotalAmountForDay
            End With
        End Get
    End Property

    Public ReadOnly Property POSSaleAmountForADay() As Double
        Get
            Dim dblPOSSaleAmount As Double
            Dim fromTime As DateTime
            Dim ToTime As DateTime

            fromTime = Me.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom
            ToTime = Me.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo

            dblPOSSaleAmount = Me.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).POSSaleAmount(fromTime, ToTime) + _
                                Me.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).POSSaleAmount(fromTime, ToTime)
            Return dblPOSSaleAmount
        End Get
    End Property

    Public ReadOnly Property TotalTipForADay() As Double
        'Tip amount paid for Paid orders from Lunch Open cash count time till Dinner Close cash count time
        Get
            Dim dblTipAmount As Double
            Dim fromTime As DateTime
            Dim ToTime As DateTime

            fromTime = Me.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).SaleDaySessionFrom
            ToTime = Me.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).SaleDaySessionTo

            dblTipAmount += Me.AllSaleDaySessions.Item(Session.enumSessionName.Lunch).TipAmountPaid(fromTime, ToTime) + _
                                Me.AllSaleDaySessions.Item(Session.enumSessionName.Dinner).TipAmountPaid(fromTime, ToTime)
            Return dblTipAmount
        End Get
    End Property

    'Public ReadOnly Property TotalInForTheDay() As Double
    '    Get
    '        Return POSSaleAmountForADay + TotalTipForADay
    '    End Get
    'End Property

    'Public ReadOnly Property TodaysCRTXNs() As CRTXNs
    '    Get
    '        If m_TodaysCRTXNs Is Nothing Then m_TodaysCRTXNs = _
    '                    New CRTXNs(CRTXNs.EnumFilter.Active, CRTXNs.EnumView.CompleteView, Me.SaleDate)
    '        Return m_TodaysCRTXNs
    '    End Get

    'End Property

    Public ReadOnly Property CRTXnAmount(ByVal timeFrom As DateTime, ByVal timeTo As DateTime) As Double
        Get
            Dim dblCrTXnAmount As Double
            Dim objCRTxns As CRTXNs
            Dim objCRTxn As CRTXN

            objCRTxns = New CRTXNs(CRTXNs.EnumFilter.Active, CRTXNs.EnumView.CompleteView, timeFrom, timeTo)

            For Each objCRTxn In objCRTxns
                If (objCRTxn.CRTXNAt >= timeFrom) And (objCRTxn.CRTXNAt <= timeTo) Then
                    dblCrTXnAmount += objCRTxn.CRTXNAmount
                End If
            Next
            Return dblCrTXnAmount
        End Get
    End Property

    Public Sub SetAllCurrentPayments(ByVal fromTime As DateTime, ByVal ToTime As DateTime)
        'Computes & Saves all Payments entered on this Saleday for Orders of this Saleday
        'This is required to reconcile Cash Register. 
        'This must be called ONCE before accesing individual Payments for EACH SDSession or Day with required from & To time
        Dim objSDSession As SaleDaySession

        For Each objSDSession In Me.AllSaleDaySessions
            objSDSession.SetAllCurrentPayments(fromTime, ToTime)
        Next
    End Sub

    Public ReadOnly Property AmexCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intAmexCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intAmexCount += objSDSession.AmexCount
            Next
            Return intAmexCount
        End Get
    End Property

    Public ReadOnly Property AmexAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblAmexAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblAmexAmount += objSDSession.AmexAmount
            Next
            Return dblAmexAmount
        End Get
    End Property

    Public ReadOnly Property AmexTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblAmexTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblAmexTipAmount += objSDSession.AmexTipAmount
            Next
            Return dblAmexTipAmount
        End Get
    End Property

    Public ReadOnly Property AmexTotalAmountForDay() As Double
        Get
            Return AmexAmountForDay + AmexTipAmountForDay
        End Get
    End Property


    Public ReadOnly Property CashCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intCashCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intCashCount += objSDSession.CashCount
            Next
            Return intCashCount
        End Get
    End Property

    Public ReadOnly Property CashAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblCashAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblCashAmount += objSDSession.CashAmount
            Next
            Return dblCashAmount
        End Get
    End Property

    Public ReadOnly Property CashTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblCashTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblCashTipAmount += objSDSession.CashTipAmount
            Next
            Return dblCashTipAmount
        End Get
    End Property

    Public ReadOnly Property CashTotalAmountForDay() As Double
        Get
            Return CashAmountForDay + CashTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property DebitCardCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intDebitCardCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intDebitCardCount += objSDSession.DebitCardCount
            Next
            Return intDebitCardCount
        End Get
    End Property

    Public ReadOnly Property DebitCardAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblDebitCardAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblDebitCardAmount += objSDSession.DebitCardAmount
            Next
            Return dblDebitCardAmount
        End Get
    End Property

    Public ReadOnly Property DebitCardTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblDebitCardTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblDebitCardTipAmount += objSDSession.DebitCardTipAmount
            Next
            Return dblDebitCardTipAmount
        End Get
    End Property

    Public ReadOnly Property DebitCardTotalAmountForDay() As Double
        Get
            Return DebitCardAmountForDay + DebitCardTipAmountForDay
        End Get
    End Property


    Public ReadOnly Property DinersClubCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intDinersClubCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intDinersClubCount += objSDSession.DinersClubCount
            Next
            Return intDinersClubCount
        End Get
    End Property

    Public ReadOnly Property DinersClubAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblDinersClubAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblDinersClubAmount += objSDSession.DinersClubAmount
            Next
            Return dblDinersClubAmount
        End Get
    End Property

    Public ReadOnly Property DinersClubTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblDinersClubTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblDinersClubTipAmount += objSDSession.DinersClubTipAmount
            Next
            Return dblDinersClubTipAmount
        End Get
    End Property

    Public ReadOnly Property DinersClubTotalAmountForDay() As Double
        Get
            Return DinersClubAmountForDay + DinersClubTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property DiscoverCardCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intDiscoverCardCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intDiscoverCardCount += objSDSession.DiscoverCount
            Next
            Return intDiscoverCardCount
        End Get
    End Property

    Public ReadOnly Property DiscoverCardAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblDiscoverCardAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblDiscoverCardAmount += objSDSession.DiscoverAmount
            Next
            Return dblDiscoverCardAmount
        End Get
    End Property

    Public ReadOnly Property DiscoverCardTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblDiscoverCardTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblDiscoverCardTipAmount += objSDSession.DiscoverTipAmount
            Next
            Return dblDiscoverCardTipAmount
        End Get
    End Property

    Public ReadOnly Property DiscoverCardTotalAmountForDay() As Double
        Get
            Return DiscoverCardAmountForDay + DiscoverCardTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property HouseAccountCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intHouseAccountCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intHouseAccountCount += objSDSession.HouseAccountCount
            Next
            Return intHouseAccountCount
        End Get
    End Property

    Public ReadOnly Property HouseAccountAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblHouseAccountAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblHouseAccountAmount += objSDSession.HouseAccountAmount
            Next
            Return dblHouseAccountAmount
        End Get
    End Property

    Public ReadOnly Property HouseAccountTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblHouseAccountTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblHouseAccountTipAmount += objSDSession.HouseAccountTipAmount
            Next
            Return dblHouseAccountTipAmount
        End Get
    End Property

    Public ReadOnly Property HouseAccountTotalAmountForDay() As Double
        Get
            Return HouseAccountAmountForDay + HouseAccountTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property KnightExpressCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intKnightExpressCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intKnightExpressCount += objSDSession.KnightExpressCount
            Next
            Return intKnightExpressCount
        End Get
    End Property

    Public ReadOnly Property KnightExpressAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblKnightExpressAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblKnightExpressAmount += objSDSession.KnightExpressAmount
            Next
            Return dblKnightExpressAmount
        End Get
    End Property

    Public ReadOnly Property KnightExpressTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblKnightExpressTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblKnightExpressTipAmount += objSDSession.KnightExpressTipAmount
            Next
            Return dblKnightExpressTipAmount
        End Get
    End Property

    Public ReadOnly Property KnightExpressTotalAmountForDay() As Double
        Get
            Return KnightExpressAmountForDay + KnightExpressTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property MasterCardCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intMasterCardCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intMasterCardCount += objSDSession.MasterCardCount
            Next
            Return intMasterCardCount
        End Get
    End Property

    Public ReadOnly Property MasterCardAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblMasterCardAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblMasterCardAmount += objSDSession.MasterCardAmount
            Next
            Return dblMasterCardAmount
        End Get
    End Property

    Public ReadOnly Property MasterCardTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblMasterCardTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblMasterCardTipAmount += objSDSession.MasterCardTipAmount
            Next
            Return dblMasterCardTipAmount
        End Get
    End Property

    Public ReadOnly Property MasterCardTotalAmountForDay() As Double
        Get
            Return MasterCardAmountForDay + MasterCardTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intMGGiftCertificateCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intMGGiftCertificateCount += objSDSession.MGGiftCertificateCount
            Next
            Return intMGGiftCertificateCount
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblMGGiftCertificateAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblMGGiftCertificateAmount += objSDSession.MGGiftCertificateAmount
            Next
            Return dblMGGiftCertificateAmount
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblMGGiftCertificateTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblMGGiftCertificateTipAmount += objSDSession.MGGiftCertificateTipAmount
            Next
            Return dblMGGiftCertificateTipAmount
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateTotalAmountForDay() As Double
        Get
            Return MGGiftCertificateAmountForDay + MGGiftCertificateTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property MissingPaymentCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intMissingPaymentCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intMissingPaymentCount += objSDSession.MissingPaymentCount
            Next
            Return intMissingPaymentCount
        End Get
    End Property

    Public ReadOnly Property MissingPaymentAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblMissingPaymentAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblMissingPaymentAmount += objSDSession.MissingPaymentAmount
            Next
            Return dblMissingPaymentAmount
        End Get
    End Property

    Public ReadOnly Property MissingPaymentTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblMissingPaymentTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblMissingPaymentTipAmount += objSDSession.MissingPaymentTipAmount
            Next
            Return dblMissingPaymentTipAmount
        End Get
    End Property

    Public ReadOnly Property MissingPaymentTotalAmountForDay() As Double
        Get
            Return MissingPaymentAmountForDay + MissingPaymentTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property PersonalCheckCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intPersonalCheckCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intPersonalCheckCount += objSDSession.PersonalCheckCount
            Next
            Return intPersonalCheckCount
        End Get
    End Property

    Public ReadOnly Property PersonalCheckAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblPersonalCheckAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblPersonalCheckAmount += objSDSession.PersonalCheckAmount
            Next
            Return dblPersonalCheckAmount
        End Get
    End Property

    Public ReadOnly Property PersonalCheckTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblPersonalCheckTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblPersonalCheckTipAmount += objSDSession.PersonalCheckTipAmount
            Next
            Return dblPersonalCheckTipAmount
        End Get
    End Property

    Public ReadOnly Property PersonalCheckTotalAmountForDay() As Double
        Get
            Return PersonalCheckAmountForDay + PersonalCheckTipAmountForDay
        End Get
    End Property

    'Travellers Checks Totals
    Public ReadOnly Property TravellersChecksCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intTravellersChecksCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intTravellersChecksCount += objSDSession.TravellersChecksCount
            Next
            Return intTravellersChecksCount
        End Get
    End Property

    Public ReadOnly Property TravellersChecksAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblTravellersChecksAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblTravellersChecksAmount += objSDSession.TravellersChecksAmount
            Next
            Return dblTravellersChecksAmount
        End Get
    End Property

    Public ReadOnly Property TravellersChecksTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblTravellersChecksTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblTravellersChecksTipAmount += objSDSession.TravellersChecksTipAmount
            Next
            Return dblTravellersChecksTipAmount
        End Get
    End Property

    Public ReadOnly Property TravellersChecksTotalAmountForDay() As Double
        Get
            Return TravellersChecksAmountForDay + TravellersChecksTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property VisaCountForDay() As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intVisaCount As Integer
            For Each objSDSession In Me.AllSaleDaySessions
                intVisaCount += objSDSession.VisaCount
            Next
            Return intVisaCount
        End Get
    End Property

    Public ReadOnly Property VisaAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblVisaAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblVisaAmount += objSDSession.VisaAmount
            Next
            Return dblVisaAmount
        End Get
    End Property

    Public ReadOnly Property VisaTipAmountForDay() As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim dblVisaTipAmount As Double
            For Each objSDSession In Me.AllSaleDaySessions
                dblVisaTipAmount += objSDSession.VisaTipAmount
            Next
            Return dblVisaTipAmount
        End Get
    End Property

    Public ReadOnly Property VisaTotalAmountForDay() As Double
        Get
            Return VisaAmountForDay + VisaTipAmountForDay
        End Get
    End Property

    Public ReadOnly Property CreditCardTotalAmountForDay() As Double
        Get
            With Me
                Return .AmexTotalAmountForDay + .DebitCardTotalAmountForDay + .DinersClubTotalAmountForDay + .DiscoverCardTotalAmountForDay _
                        + .KnightExpressTotalAmountForDay + .MasterCardTotalAmountForDay + .VisaTotalAmountForDay
            End With
        End Get
    End Property

    Public Property CurrentOverride() As Override
        Get

        End Get
        Set(ByVal Value As Override)

        End Set
    End Property

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
    Public ReadOnly Property ReceiptFooterMsg() As String
        Get
            Return m_ReceiptFooterMsg
        End Get
    End Property

    Private Property IsPersisted() As Boolean
        Get
            Return (m_IsPersisted)
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersisted = Value
        End Set
    End Property

    Public Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strCloseAt As String
        Dim strCloseBy As String

        DataAccess.CreateDataAccess.StartTxn()

        If Me.SaleDayCloseAt > DateTime.MinValue Then
            strCloseAt = "'" & Me.SaleDayCloseAt.ToString & "'"
            strCloseBy = Me.SaleDayCloseBy.ToString
        Else
            strCloseAt = "NULL"
            strCloseBy = "NULL"
        End If

        If Me.IsDirty Then
            Select Case Me.IsPersisted      'True means update existing row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into SaleDay values ('" & _
                                Me.SaleDate.ToShortDateString & "','" & _
                                Me.SaleDayOpenAt.ToString & "'," & _
                                Me.SaleDayOpenBy.ToString & "," & _
                                strCloseAt & "," & strCloseBy & "," & _
                                CStr(Me.SaleDayType) & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.SaleDayId = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update SaleDay set  " & _
                                "SaleDayCloseAt=" & _
                                strCloseAt & "," & _
                                "SaleDayCloseBy=" & _
                                strCloseBy & _
                                " where SaleDayId =" & Me.SaleDayId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
        End If
        If intSQLReturnValue > 0 Then
            MarkDirtyOrClean(False)
            IsPersisted = True
            DataAccess.CreateDataAccess.EndTxn()
        End If
        Return intSQLReturnValue
    End Function


    '************************************  New Report section start ********

    Public Function BaseSale(ByVal forSaleDate As DateTime) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order] where " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    Public Function BaseSale(ByVal forSaleDate As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Public Function BaseSale(ByVal forSaleDate As DateTime, _
                                ByVal forSessionId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                     "([Order].SessionId = " & forSessionId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Private Function SqlWhereClauseForValidPayments() As String
        Return ("(PaymentStatus = " & CStr(Payment.enumStatus.Active) & _
                ")")
    End Function

    Private Function SQLWhereClauseForValidOrders() As String
        Return ("(OrderOverrideStatus In (" & CStr(Order.enumOrderVoidStatus.NotVoided) & _
                                "," & CStr(Order.enumOrderVoidStatus.PartiallyVoided) & ")) and " & _
                                "(OrderStatus=" & CStr(Order.enumOrderPaidStatus.FullyPaid) & _
                                ")")
    End Function
    '************************************  New Report section end ************************
    '               Base Sale For A Period Begin

    '*************************************************************************************


    Public Function BaseSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order] where " & _
                    "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    Public Function BaseSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                     "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Public Function BaseSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                       ByVal forCRId As Integer) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order] where " & _
                    "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                     "(CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Public Function BaseSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                   "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                     "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function



    '******************************************************************************************************

    '                            Base Sale For A Period  Functions  Ends 

    '                            Sales TAx For a Day Functions Begins
    '******************************************************************************************************


    Public Function SalesTaxCollected(ByVal forSaleDate As DateTime) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTax " & _
                    "from [Order] where " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    Public Function SalesTaxCollected(ByVal forSaleDate As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTax " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Public Function SalesTaxCollected(ByVal forSaleDate As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTax " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                     "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    '************************************************************************************
    '                        SalesTax For A Period
    '************************************************************************************



    Public Function SalesTaxForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTaxForAPeriod " & _
                    "from [Order] where " & _
                    "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    Public Function SalesTaxForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTaxForAPeriod  " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                     "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    Public Function SalesTaxForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                        ByVal forCRId As Integer) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTaxForAPeriod " & _
                    "from [Order] where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                      "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                     "(CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    Public Function SalesTaxForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTaxForAPeriod " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                     "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                     "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function
    '***********************************************************************************************
    '            SalesTaxCollected For A Perid Function Ends                  

    '           POSSaleAmount  Functions Begins

    '************************************************************************************************



    Public Function POSSale(ByVal forSaleDate As DateTime) As Double

        Return (BaseSale(forSaleDate) + SalesTaxCollected(forSaleDate))
    End Function

    Public Function POSSale(ByVal forSaleDate As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Return (BaseSale(forSaleDate, forPaymentMethod) + _
                SalesTaxCollected(forSaleDate, forPaymentMethod))
    End Function

    Public Function POSSale(ByVal forSaleDate As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double

        Return (BaseSale(forSaleDate, forCRId, forPaymentMethod) + _
                SalesTaxCollected(forSaleDate, forCRId, forPaymentMethod))
    End Function

    '  ****************************************************************************************************
    '                                          POSSale For A Period
    ' *****************************************************************************************************




    Public Function POSSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) As Double

        Return (BaseSaleForAPeriod(fromDateTime, ToDateTime) + SalesTaxForAPeriod(fromDateTime, ToDateTime))
    End Function

    Public Function POSSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double
        Return (BaseSaleForAPeriod(fromDateTime, ToDateTime, forPaymentMethod) + _
                SalesTaxForAPeriod(fromDateTime, ToDateTime, forPaymentMethod))
    End Function


    Public Function POSSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                    ByVal forCRId As Integer) As Double

        Return (BaseSaleForAPeriod(fromDateTime, ToDateTime, forCRId) + _
                SalesTaxForAPeriod(fromDateTime, ToDateTime, forCRId))
    End Function

    Public Function POSSaleForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Double

        Return (BaseSaleForAPeriod(fromDateTime, ToDateTime, forCRId, forPaymentMethod) + _
                SalesTaxForAPeriod(fromDateTime, ToDateTime, forCRId, forPaymentMethod))
    End Function

    '***************************************************************************************
    '                   POSSale For a Period Ends

    '                   TXNCount Functions Begins
    '**************************************************************************************
    Public Function GetSaleTXNCount(ByVal forSaleDate As DateTime) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order] where " & _
                    "(OrderSaledate = '" & forSaleDate.ToShortDateString & "') And " & _
                    SQLWhereClauseForValidOrders()
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    Public Function GetSaleTXNCount(ByVal forSaleDate As DateTime, ByVal forCRId As Integer) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order] where " & _
                    "(OrderSaledate = '" & forSaleDate.ToShortDateString & "') And " & _
                    "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders()
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    Public Function GetSaleTXNCount(ByVal forSaleDate As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    Public Function GetSaleTXNCount(ByVal forSaleDate As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                     "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    '****************************************************************************************
    '                TXNCount Functions Ends

    '                TXNCount Functions For A Period Begins
    '****************************************************************************************

    Public Function GetSaleTXNCountForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order] where " & _
                     "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    SQLWhereClauseForValidOrders()
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    Public Function GetSaleTXNCountForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
    ByVal forCRId As Integer) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order] where " & _
                    "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders()
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    Public Function GetSaleTXNCountForAPeriod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                   "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function

    Public Function GetSaleTXNCountForAperiod(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime, _
                                ByVal forCRId As Integer, _
                                ByVal forPaymentMethod As Payment.enumPaymentMethod) As Integer
        Dim strSqlCmd As String
        strSqlCmd = "select Count(*) as SaleTXNCount " & _
                    "from [Order], Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
                    "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
                     "([Order].CRID = " & forCRId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                    SqlWhereClauseForValidPayments() & " And " & _
                    "(PaymentMethod = " & CStr(forPaymentMethod) & _
                    ")"
        Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    End Function


    '************************************************************************************
    '          New ReportFunctions
    '************************************************************************************


    Public Function BaseSale(ByVal forSaleDate As DateTime, ByVal forSessionId As Integer) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderAmount) as BaseSale " & _
                    "from [Order] where " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                     "(SessionId = " & forSessionId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Public Function SalesTaxCollected(ByVal forSaleDate As DateTime, ByVal forSessionId As Integer) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(OrderSalesTax) as SalesTax " & _
                    "from [Order] where " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                     "(SessionId = " & forSessionId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function


    Public Function POSSale(ByVal forSaleDate As DateTime, ByVal forSessionId As Integer) As Double

        Return (BaseSale(forSaleDate, forSessionId) + _
                SalesTaxCollected(forSaleDate, forSessionId))
    End Function


    Public Function TipAmount(ByVal forSaleDate As DateTime, ByVal forSessionId As Integer) As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(PaymentTipAmount) as TipsCollected " & _
                    "from [Order],Payment where " & _
                    "([Order].OrderId=Payment.OrderId) and " & _
                    "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
                     "(SessionId = " & forSessionId.ToString & ") And " & _
                    SQLWhereClauseForValidOrders() & " And " & _
                     SqlWhereClauseForValidPayments()
        '")"
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)
    End Function

    Public Function TotalIn(ByVal forSaleDate As DateTime, ByVal forSessionId As Integer) As Double
        Return POSSale(forSaleDate, forSessionId) + TipAmount(forSaleDate, forSessionId)
    End Function






    ''POS Sale Amount = Base Sale + Sales Tax
    'Public Function GetCashPOSSaleAmount(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) _
    '                As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount + OrderSalesTax) as POSSaleAmount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod = " & CStr(Payment.enumPaymentMethod.Cash) & " )" & " and " & _
    '                "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
    '                "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
    '                "([Order].CRId = " & Me.CRId.ToString & ") And " & _
    '                "(OrderStatus=" & CStr(Order.enumOrderStatus.Paid) & " )"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function
    'Public Function GetCashPOSSaleAmount(ByVal forSaleDate As DateTime) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount + OrderSalesTax) as POSSaleAmount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod = " & CStr(Payment.enumPaymentMethod.Cash) & " )" & " and " & _
    '                "(OrderSaledate = '" & forSaleDate.ToShortDateString & "') And " & _
    '                "(OrderStatus=" & CStr(Order.enumOrderStatus.Paid) & " )"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function



    'Public Function GetCashBaseSaleAmount(ByVal forSaleDate As String, ByVal forCRId As Integer) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount) as BaseSaleAmount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod = " & CStr(Payment.enumPaymentMethod.Cash) & " )" & " and " & _
    '                "(OrderSaledate = '" & forSaleDate & "') And " & _
    '                "([Order].CRID = " & forCRId.ToString & ") And " & _
    '                "(OrderStatus=" & CStr(Order.enumOrderStatus.Paid) & " )"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function


    'Public Function GetCashSaleTXNCount(ByVal forSaleDate As String, ByVal forCRId As Integer) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select Count(OrderAmount) as CashTXNCount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod = " & CStr(Payment.enumPaymentMethod.Cash) & " )" & " and " & _
    '                "(OrderSaledate = '" & forSaleDate & "') And " & _
    '                "([Order].CRID = " & forCRId.ToString & ") And " & _
    '                "(OrderStatus=" & CStr(Order.enumOrderStatus.Paid) & " )"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function

    'Public Function GetKnightExpressPOSSaleAmount(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) _
    '        As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount + OrderSalesTax) as POSSaleAmount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod =" & CStr(Payment.enumPaymentMethod.KnightExpress) & ") and " & _
    '                "(OrderDateEnd >= '" & fromDateTime.ToString & "') And " & _
    '                "(OrderDateEnd <= '" & ToDateTime.ToString & "') And " & _
    '                "([Order].CRId = " & Me.CRId.ToString & ") And " & _
    '                "(OrderStatus=" & CStr(Order.enumOrderStatus.Paid) & ")"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function


    'Public Function GetKnightExpressPOSSaleAmount(ByVal forSaleDate As DateTime) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount + OrderSalesTax) as POSSaleAmount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod =" & CStr(Payment.enumPaymentMethod.KnightExpress) & ") and " & _
    '                "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
    '                SQLWhereClauseForValidOrders() & _
    '                ")"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function

    'Public Function GetKnightExpressBaseSaleAmount(ByVal forSaleDate As DateTime, ByVal forCRId As Integer) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount ) as BaseKnightExpressSaleAmount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod =" & CStr(Payment.enumPaymentMethod.KnightExpress) & ") and " & _
    '                "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
    '                 "([Order.CRID] = " & forCRId.ToString & ") And " & _
    '                SQLWhereClauseForValidOrders() & _
    '                ")"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function


    'Public Function GetKnightExpressSaleTXNCount(ByVal forSaleDate As DateTime, ByVal forCRId As Integer) As Integer
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select Count(* ) as KnightExpressTXNCount " & _
    '                "from [Order], Payment where " & _
    '                "([Order].OrderId=Payment.OrderId) and " & _
    '                "(PaymentMethod =" & CStr(Payment.enumPaymentMethod.KnightExpress) & ") and " & _
    '                "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
    '                 "([Order.CRID] = " & forCRId.ToString & ") And " & _
    '                SQLWhereClauseForValidOrders() & _
    '                ")"
    '    Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    'End Function


    'Public Function GetSalesTaxCollected(ByVal forSaleDate As DateTime, ByVal forCRId As Integer) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderSalesTax) as SalesTaxCollected " & _
    '                "from [Order] where " & _
    '                "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
    '                 "(CRID = " & forCRId.ToString & ") And " & _
    '                SQLWhereClauseForValidOrders() & _
    '                ")"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function

    'Public Function GetTotalBaseSale(ByVal forSaleDate As DateTime, ByVal forCRId As Integer) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(OrderAmount) as TotalBaseSale " & _
    '                "from [Order] where " & _
    '                "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
    '                 "(CRID = " & forCRId.ToString & ") And " & _
    '                SQLWhereClauseForValidOrders() & _
    '                ")"
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function

    'Public Function GetTotalSaleTXNCount(ByVal forSaleDate As DateTime, ByVal forCRId As Integer) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select Count(*) as TotalSaleTxnCount " & _
    '                "from [Order] where " & _
    '                "(OrderSaleDate= '" & forSaleDate.ToShortDateString & "') And " & _
    '                 "(CRID = " & forCRId.ToString & ") And " & _
    '                SQLWhereClauseForValidOrders() & _
    '                ")"
    '    Return DataAccess.CreateDataAccess.GetCountAsInteger(strSqlCmd)

    'End Function


    Public Function GetCRTxnAmount(ByVal fromDateTime As DateTime, ByVal ToDateTime As DateTime) _
            As Double
        Dim strSqlCmd As String
        strSqlCmd = "select sum(CRTxnAmount) as CRTxnAmount " & _
                    "from CRTXN where " & _
                    "(CRTXNAt >= '" & fromDateTime.ToString & "') And " & _
                    "(CRTXNAt <= '" & ToDateTime.ToString & "') And " & _
                    "(CRId = " & Me.CRId.ToString & ") And " & _
                    "(CRTXNStatus=)" & CStr(CRTXN.enumCRTXNStatus.Active)
        Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    End Function



    'Public Function GetCRTxnAmount(ByVal forSaleDate As DateTime) As Double
    '    Dim strSqlCmd As String
    '    strSqlCmd = "select sum(CRTxnAmount) as CRTxnAmount " & _
    '                "from CRTXN where " & _
    '                "(CRTXNAt >= '" & forSaleDate.ToShortDateString & "') And " & _
    '                "(CRTXNStatus=)" & CStr(CRTXN.enumCRTXNStatus.Active)
    '    Return DataAccess.CreateDataAccess.GetAmountAsDouble(strSqlCmd)

    'End Function
    Public ReadOnly Property IsDayOpen() As Boolean
        Get
            Return ((Me.SaleDayOpenAt > Date.MinValue) AndAlso (Me.SaleDayCloseAt <= Date.MinValue))
        End Get
    End Property

    Public Property CurrentSaleDaySession() As SaleDaySession
        Get
            Return m_CurrentSaleDaySession
        End Get
        Set(ByVal Value As SaleDaySession)
            m_CurrentSaleDaySession = Value

        End Set
    End Property

    Friend Sub loadData(ByVal SqlDr As SqlDataReader)
        Dim msg As String
        Dim i As Integer
        Dim SqlDrColName As String

        IsPersisted = True
        For i = 0 To SqlDr.FieldCount - 1
            SqlDrColName = SqlDr.GetName(i)
            Select Case SqlDrColName
                Case "SaleDayId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDayId = SqlDr.GetInt32(i)
                    End If

                Case "SaleDate"
                    m_SaleDate = SqlDr.GetDateTime(i)

                Case "SaleDayOpenAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDayOpenAt = SqlDr.GetDateTime(i)
                    Else
                        m_SaleDayOpenAt = Date.MinValue
                    End If

                Case "SaleDayCloseAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDayCloseAt = SqlDr.GetDateTime(i)
                    Else
                        m_SaleDayCloseAt = Date.MinValue
                    End If

                Case "SaleDayOpenBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDayOpenBy = SqlDr.GetInt32(i)
                    Else
                        m_SaleDayOpenBy = 0
                    End If

                Case "SaleDayCloseBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDayCloseBy = SqlDr.GetInt32(i)
                    Else
                        m_SaleDayCloseBy = 0
                    End If

                Case "SaleDayType"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDayType = CType(SqlDr.GetInt32(i), SaleDay.enumSaleDayType)
                    Else
                        m_SaleDayType = SaleDay.enumSaleDayType.Regular
                    End If
            End Select
        Next
    End Sub

    Public ReadOnly Property CRId() As Integer
        Get
            Return m_CRId
        End Get

    End Property
    Private Sub SetPhysicalCRId()
        Dim intCRId As Integer
        Dim strCRID As String

        Dim objStreamReader As StreamReader = File.OpenText("C:\POS2006\PosCRId.ini")
        With objStreamReader
            .BaseStream.Seek(0, SeekOrigin.Begin)
            intCRId = .Read                 'returns ASCII code for CR Id
            strCRID = (Chr(intCRId))        'cant use cint on chr !!
            m_CRId = CInt(strCRID)
            .Close()
        End With
    End Sub

    'new code begins
    Public ReadOnly Property AllOrders() As Orders
        Get
            Dim objOrders As New Orders()
            Dim objSDSession As SaleDaySession
            Dim objOrder As Order

            For Each objSDSession In AllSaleDaySessions
                For Each objOrder In objSDSession.AllOrders
                    objOrders.Add(objOrder)
                Next
            Next
            Return objOrders
        End Get
    End Property

    Public Function GetTargetSaleDaySession(ByVal targetTimeNow As DateTime) As SaleDaySession
        Dim targetSDSessionId As Session.enumSessionName

        Select Case targetTimeNow.Hour
            Case Is > 16
                targetSDSessionId = Session.enumSessionName.Dinner
            Case Else
                targetSDSessionId = Session.enumSessionName.Lunch
        End Select
        Return AllSaleDaySessions.Item(targetSDSessionId)
    End Function

    Public ReadOnly Property OpenOrdersPOSAmount(ByVal AsAtTime As DateTime) As Double
        Get
            Dim objSDSession As SaleDaySession
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objSDSession In Me.AllSaleDaySessions
                With objSDSession
                    If (.SaleDaySessionFrom > AsAtTime) Then
                        'nop: skip this sdsession
                    Else
                        For Each objOrder In .AllOrders
                            If (objOrder.PaidAt = DateTime.MinValue) OrElse (objOrder.PaidAt >= AsAtTime) Then
                                dblAmount += objOrder.POSAmount
                            End If
                        Next
                    End If
                End With
            Next
            Return dblAmount
        End Get
    End Property

End Class
