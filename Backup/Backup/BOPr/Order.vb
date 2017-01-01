Option Strict On
Imports System.Data.SqlClient

Public Class Order
    Inherits POSBO
    Public Event OverrideNeeded(ByVal newOverride As Override)

    Public Enum enumOrderSalesTaxStatus
        Taxable
        NJTaxExempt
        OutOfState
    End Enum
    Public Enum enumOrderVoidStatus
        NotVoided
        PartiallyVoided
        Voided
    End Enum
    Public Enum enumOrderPaidStatus
        FullyPaid
        PartiallyPaid
        UnPaid
        PaidButNotEnteredYet
        MissingPayment
        InError
    End Enum
    Public Enum enumOrderVoidReasons
        NoCash
        Emergency
        UnAwareOfPrice
        MadeWrongItemSelection
        WaiterError
        CustomerReason
        MGReason
    End Enum

    Public Enum enumOrderType
        EatIn
        Pickup
        Delivery
    End Enum

    Public Enum enumTips
        NoTipsAdded
        TipAt18PercentAddedFor6OrMore
        TipAt20PercentAddedFor6OrMore
        TipAt15PercentAddedFor6OrMore
    End Enum

    Public Enum enumMGCoupon
        NoDiscount
        HappyCurryHourDiscount
        McArthurCoupon
    End Enum

    Public Enum enumCustomerMsg As Integer
        MGGiftCertificate
        HolidayParties
        LunchBuffet
        HappyCurryHour
    End Enum


    Private m_DiscountType As enumMGCoupon
    Private m_DiscountAmount As Double
    Private m_TipType As enumTips
    Private m_TipAmount As Double

    Private m_OrderId As Integer
    Private m_CRId As Integer

    Private m_SaleDate As DateTime
    Private m_SessionId As Session.enumSessionName

    Private m_OrderSalesTaxStatus As Order.enumOrderSalesTaxStatus

    Private m_EnteredAt As DateTime
    Private m_OrderPromisedAt As DateTime
    Private m_PaidAt As DateTime
    Private m_PaymentChangedAt As DateTime
    Private m_FirstKOTPrintedAt As DateTime
    Private m_LastKOTPrintedAt As DateTime
    Private m_FirstCheckPrintedAt As DateTime
    Private m_LastCheckPrintedAt As DateTime
    Private m_NumberOfSplitChecks As Integer = 1  'Total number of checks an Order is split into

    Private m_OrderPaidStatus As enumOrderPaidStatus
    Private m_OrderVoidStatus As enumOrderVoidStatus
    Private m_PaymentEnteredBy As Integer
    Private m_OrderType As enumOrderType

    Private m_ParentAllOrders As Orders
    Private m_objPayments As Payments
    Private m_objOrderItems As OrderItems
    Private m_Table As DTable
    Private m_TableSeating As Integer   'sequential seating # within a SDSession for a Table
    Private m_Waiter As Employee
    Private m_Customer As Customer

    Private m_IsHouseACOrder As Boolean
    Private m_HouseACId As Integer
    Private m_HouseACCheckSignedBy As String
    Private m_TakeOutCustomerName As String
    Private m_TakeOutCustomerAddress As String
    Private m_TakeOutCustomerPhone As String

    Private m_AdultsCount As Integer
    Private m_KidsCount As Integer
    Private m_IsDesiOrder As Boolean
    'Private m_VoidItems As OrderItems

    Private m_IsDirty As Boolean
    Friend m_IsPersisted As Boolean

    'Root Level Override objects :  must be set to nothing in order.setdata 
    Private m_objOverrideForPaymentEdit As Override
    Private m_objOverrideForAddOrderItem As Override
    Private m_objOverrideForOrderEdit As Override
    Private m_objVoidOverride As Override
    Private m_objGuestCheckOverride As Override
    Public Sub New()
        OrderId = 0
        SetEnteredAt()
    End Sub

    Friend Sub New(ByVal SqlDr As SqlDataReader)
        IsPersisted = True
        LoadData(SqlDr)
    End Sub

    Friend Sub LoadData(ByVal sqlDr As SqlDataReader)
        Dim msg As String
        Dim i As Integer
        Dim SqlDrColName As String
        Dim CustomerId As Integer

        For i = 0 To sqlDr.FieldCount - 1 'reads every column in a record
            SqlDrColName = sqlDr.GetName(i)
            Select Case SqlDrColName
                Case "OrderId"
                    m_OrderId = sqlDr.GetInt32(i)
                Case "CRId"
                    m_CRId = sqlDr.GetInt32(i)
                Case "TableId"
                    If Not sqlDr.IsDBNull(i) Then
                        With SaleDays.CreateSaleDay.ActiveTables
                            m_Table = .Item(.Indexof(sqlDr.GetInt32(i)))
                        End With
                    End If
                Case "TableSeating"
                    m_TableSeating = sqlDr.GetInt32(i)
                Case "CustomerId"
                    If Not sqlDr.IsDBNull(i) Then
                        CustomerId = sqlDr.GetInt32(i)
                    Else
                        CustomerId = -1
                    End If
                    If CustomerId = -1 Then
                        m_Customer = Nothing
                    Else
                        With SaleDays.CreateSaleDay.ActiveCustomers
                            m_Customer = .Item(.Indexof(sqlDr.GetInt32(i)))
                        End With
                    End If
                Case "WaiterId"
                    If Not sqlDr.IsDBNull(i) Then
                        With SaleDays.CreateSaleDay.ActiveCashiers
                            m_Waiter = .Item(.IndexOf(sqlDr.GetInt32(i)))
                        End With
                    End If
                Case "SaleDate"
                    m_SaleDate = sqlDr.GetDateTime(i)
                Case "SessionId"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_SessionId = CType(sqlDr.GetInt32(i), Session.enumSessionName)
                        Else
                            m_SessionId = Session.enumSessionName.Lunch
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "OrderType"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_OrderType = CType(sqlDr.GetInt32(i), Order.enumOrderType)
                        Else
                            m_OrderType = Order.enumOrderType.EatIn
                        End If
                    Catch ex As Exception
                        m_OrderType = Order.enumOrderType.EatIn
                    End Try
                Case "AdultsCount"
                    If Not sqlDr.IsDBNull(i) Then
                        m_AdultsCount = sqlDr.GetInt32(i)
                    End If
                Case "KidsCount"
                    If Not sqlDr.IsDBNull(i) Then
                        m_KidsCount = sqlDr.GetInt32(i)
                    End If
                    '************************************************
                    '***** For now we are going to derive properties
                    '***** available from orderitems
                    '************************************************
                    'Case "BaseAmount"
                    '    If Not sqlDr.IsDBNull(i) Then
                    '        m_OrderBaseAmount = sqlDr.GetDecimal(i)
                    '    Else
                    '        m_OrderBaseAmount = 0
                    '    End If

                    'Case "SalesTaxAmount"
                    '    If Not sqlDr.IsDBNull(i) Then
                    '        m_OrderSalesTax = sqlDr.GetDecimal(i)
                    '    Else
                    '        m_OrderSalesTax = 0
                    '    End If

                Case "SalesTaxStatus"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_OrderSalesTaxStatus = CType(sqlDr.GetInt32(i), Order.enumOrderSalesTaxStatus)
                        Else
                            m_OrderSalesTaxStatus = Order.enumOrderSalesTaxStatus.Taxable
                        End If
                    Catch ex As Exception
                        m_OrderSalesTaxStatus = Order.enumOrderSalesTaxStatus.Taxable
                    End Try
                Case "DiscountType"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_DiscountType = CType(sqlDr.GetInt32(i), Order.enumMGCoupon)
                        Else
                            m_DiscountType = Order.enumMGCoupon.NoDiscount
                        End If
                    Catch ex As Exception
                        m_DiscountType = Order.enumMGCoupon.NoDiscount
                    End Try
                Case "DiscountAmount"
                    If Not sqlDr.IsDBNull(i) Then
                        m_DiscountAmount = sqlDr.GetDecimal(i)
                    Else
                        m_DiscountAmount = 0
                    End If
                Case "TipType"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_TipType = CType(sqlDr.GetInt32(i), Order.enumTips)
                        Else
                            m_TipType = Order.enumTips.NoTipsAdded
                        End If
                    Catch ex As Exception
                        m_TipType = Order.enumTips.NoTipsAdded
                    End Try
                Case "TipAmount"
                    If Not sqlDr.IsDBNull(i) Then
                        m_TipAmount = sqlDr.GetDecimal(i)
                    Else
                        m_TipAmount = 0
                    End If
                Case "EnteredAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_EnteredAt = sqlDr.GetDateTime(i)
                    Else
                        m_EnteredAt = Date.MinValue
                    End If
                Case "PromisedAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_OrderPromisedAt = sqlDr.GetDateTime(i)
                    Else
                        m_OrderPromisedAt = Date.MinValue
                    End If
                Case "FirstKOTPrintedAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_FirstKOTPrintedAt = sqlDr.GetDateTime(i)
                    Else
                        m_FirstKOTPrintedAt = Date.MinValue
                    End If
                Case "LastKOTPrintedAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_LastKOTPrintedAt = sqlDr.GetDateTime(i)
                    Else
                        m_LastKOTPrintedAt = Date.MinValue
                    End If
                Case "FirstCheckPrintedAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_FirstCheckPrintedAt = sqlDr.GetDateTime(i)
                    Else
                        m_FirstCheckPrintedAt = Date.MinValue
                    End If
                Case "LastCheckPrintedAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_LastCheckPrintedAt = sqlDr.GetDateTime(i)
                    Else
                        m_LastCheckPrintedAt = Date.MinValue
                    End If
                Case "PaidAt"
                    If Not sqlDr.IsDBNull(i) Then
                        m_PaidAt = sqlDr.GetDateTime(i)
                    Else
                        m_PaidAt = Date.MinValue
                    End If
                Case "PaidStatus"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_OrderPaidStatus = CType(sqlDr.GetInt32(i), Order.enumOrderPaidStatus)
                        Else
                            m_OrderPaidStatus = Order.enumOrderPaidStatus.UnPaid
                        End If
                    Catch ex As Exception
                        m_OrderPaidStatus = Order.enumOrderPaidStatus.UnPaid
                    End Try
                Case "VoidStatus"
                    Try
                        If Not sqlDr.IsDBNull(i) Then
                            m_OrderVoidStatus = CType(sqlDr.GetInt32(i), Order.enumOrderVoidStatus)
                        Else
                            m_OrderVoidStatus = Order.enumOrderVoidStatus.NotVoided
                        End If
                    Catch ex As Exception
                        m_OrderVoidStatus = Order.enumOrderVoidStatus.NotVoided
                    End Try
                Case "PaymentEnteredBy"
                    If Not sqlDr.IsDBNull(i) Then
                        m_PaymentEnteredBy = sqlDr.GetInt32(i)
                    End If
                Case "IsDesiOrder"
                    If sqlDr.IsDBNull(i) Then
                        m_IsDesiOrder = False
                    Else
                        If sqlDr.GetInt32(i) = 0 Then
                            m_IsDesiOrder = False
                        Else
                            m_IsDesiOrder = True
                        End If
                    End If
                Case "IsHouseACOrder"
                    If sqlDr.IsDBNull(i) Then
                        m_IsHouseACOrder = False
                    Else
                        If sqlDr.GetInt32(i) = 0 Then
                            m_IsHouseACOrder = False
                        Else
                            m_IsHouseACOrder = True
                        End If
                    End If
                Case "TakeOutCustomerName"
                    If sqlDr.IsDBNull(i) Then
                        m_TakeOutCustomerName = String.Empty
                    Else
                        m_TakeOutCustomerName = sqlDr.GetString(i)
                    End If
                Case "TakeOutCustomerAddress"
                    If sqlDr.IsDBNull(i) Then
                        m_TakeOutCustomerAddress = String.Empty
                    Else
                        m_TakeOutCustomerAddress = sqlDr.GetString(i)
                    End If
                Case "TakeOutCustomerPhone"
                    If sqlDr.IsDBNull(i) Then
                        m_TakeOutCustomerPhone = String.Empty
                    Else
                        m_TakeOutCustomerPhone = sqlDr.GetString(i)
                    End If
                Case "HouseACCheckSignedBy"
                    If sqlDr.IsDBNull(i) Then
                        m_HouseACCheckSignedBy = String.Empty
                    Else
                        m_HouseACCheckSignedBy = sqlDr.GetString(i)
                    End If
                Case "NumberOfSplitChecks"
                    If Not sqlDr.IsDBNull(i) Then
                        m_NumberOfSplitChecks = sqlDr.GetInt32(i)
                    Else
                        m_NumberOfSplitChecks = 1
                    End If
            End Select
        Next
    End Sub

    Public Property OrderId() As Integer

        Get
            Return m_OrderId
        End Get

        Set(ByVal Value As Integer)
            m_OrderId = Value
        End Set
    End Property

    Public ReadOnly Property CRId() As Integer
        Get
            Return m_CRId
        End Get
    End Property

    Public Property Table() As DTable
        Get
            Return m_Table
        End Get
        Set(ByVal Value As DTable)
            m_Table = Value
            If m_Table Is Nothing Then
                'nop
            Else
                m_Table.CurrentOrder = Me
                SetTableSeating()
            End If
            MarkDirty()
        End Set
    End Property

    Friend Property ParentAllOrders() As Orders
        Get
            Return m_ParentAllOrders
        End Get
        Set(ByVal value As Orders)
            m_ParentAllOrders = value
        End Set
    End Property

    Public ReadOnly Property TableSeating() As Integer
        Get
            Return m_TableSeating
        End Get

    End Property

    Private Sub SetTableSeating()
        m_TableSeating = Me.ParentAllOrders.SetCurrentTableSeating(Me.Table.TableName)
        'MarkDirty()
    End Sub

    Public Property Waiter() As Employee
        Get
            Return m_Waiter
        End Get
        Set(ByVal Value As Employee)
            m_Waiter = Value
            MarkDirty()
        End Set
    End Property

    Public Property Customer() As Customer
        Get
            Return m_Customer
        End Get
        Set(ByVal Value As Customer)
            m_Customer = Value
            MarkDirty()
            If m_Customer Is Nothing Then
                'nop
            Else
                If m_Customer.CustomerSTaxExempt = True Then
                    SalesTaxStatus = enumOrderSalesTaxStatus.NJTaxExempt
                Else
                    SalesTaxStatus = enumOrderSalesTaxStatus.Taxable
                End If
            End If
        End Set
    End Property

    Public Property IsHouseACOrder() As Boolean
        Get
            Return m_IsHouseACOrder
        End Get
        Set(ByVal Value As Boolean)
            m_IsHouseACOrder = Value
        End Set
    End Property

    'Public Property HouseACId() As Integer
    '    Get
    '        Return m_HouseACId
    '    End Get
    '    Set(ByVal Value As Integer)
    '        m_HouseACId = Value
    '    End Set
    'End Property

    Public Property HouseACCheckSignedBy() As String
        Get
            Return m_HouseACCheckSignedBy
        End Get
        Set(ByVal Value As String)
            m_HouseACCheckSignedBy = Value
        End Set
    End Property

    Public Property TakeOutCustomerName() As String
        Get
            Return m_TakeOutCustomerName
        End Get
        Set(ByVal Value As String)
            m_TakeOutCustomerName = Value
        End Set
    End Property

    Public Property TakeOutCustomerAddress() As String
        Get
            Return m_TakeOutCustomerAddress
        End Get
        Set(ByVal Value As String)
            m_TakeOutCustomerAddress = Value
        End Set
    End Property

    Public Property TakeOutCustomerPhone() As String
        Get
            Return m_TakeOutCustomerPhone
        End Get
        Set(ByVal Value As String)
            m_TakeOutCustomerPhone = Value
        End Set
    End Property

    Public Property IsDesiOrder() As Boolean
        Get
            Return m_IsDesiOrder
        End Get
        Set(ByVal Value As Boolean)
            m_IsDesiOrder = Value
        End Set
    End Property

    Public Property Saledate() As DateTime

        Get
            Return m_SaleDate
        End Get
        Set(ByVal Value As DateTime)
            m_SaleDate = Value
            MarkDirty()
        End Set
    End Property

    Public Property SessionId() As Session.enumSessionName
        Get
            Return m_SessionId
        End Get
        Set(ByVal Value As Session.enumSessionName)
            m_SessionId = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property NonRevenueSale() As Double
        Get
            'Includes only items (like Gift Certificate) which are sold 
            'but do not constitute Revenue when sold.
            Dim dblAmount As Double
            Dim objOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems

                With objOrderItem
                    If objOrderItem.MenuItem Is Nothing Then
                        dblAmount = 0
                    ElseIf (.Status = OrderItem.enumStatus.Active) AndAlso _
                           (objOrderItem.MenuItem.ProductCategoryType = ProductCategory.enumProductCategory.Misc) Then
                        dblAmount += .OrderItemAmount
                    End If

                End With
            Next
            Return Math.Round(dblAmount, 2)
        End Get
    End Property

    '**************************** Retail Revenue
    Public ReadOnly Property TaxExemptRetailBaseRevenue() As Double
        Get
            'This includes Retail Food Sale to Tax Exempt Retail Customers i.e. not invoiced from QB
            ' excludes NonRevenueSale items & Tips
            Dim dblOrderAmount As Double
            Dim objOrderItem As OrderItem

            Select Case Me.IsHouseACOrder
                Case True
                    dblOrderAmount = 0
                Case False
                    Select Case SalesTaxStatus
                        Case Order.enumOrderSalesTaxStatus.NJTaxExempt, enumOrderSalesTaxStatus.OutOfState
                            For Each objOrderItem In Me.AllOrderItems
                                With objOrderItem
                                    If (.Status = OrderItem.enumStatus.Active) AndAlso _
                                       ((objOrderItem.MenuItem Is Nothing) OrElse _
                                        (objOrderItem.MenuItem.ProductCategoryType <> ProductCategory.enumProductCategory.Misc)) Then
                                        dblOrderAmount += .OrderItemAmount - .OrderItemDiscountAmount
                                    End If

                                End With
                            Next
                        Case Else
                            dblOrderAmount = 0
                    End Select
            End Select
            Return Math.Round(dblOrderAmount, 2)
        End Get
    End Property

    Public ReadOnly Property TaxableRetailBaseRevenue() As Double
        Get
            'This includes Retail Food Sale to Taxable Retail Customers i.e. not invoiced from QB
            ' excludes NonRevenueSale items & Tips
            Dim dblOrderAmount As Double
            Dim objOrderItem As OrderItem

            Select Case Me.IsHouseACOrder
                Case True
                    dblOrderAmount = 0
                Case False
                    Select Case SalesTaxStatus
                        Case Order.enumOrderSalesTaxStatus.Taxable
                            For Each objOrderItem In Me.AllOrderItems
                                With objOrderItem
                                    If (.Status = OrderItem.enumStatus.Active) AndAlso _
                                        ((objOrderItem.MenuItem Is Nothing) OrElse _
                                         (objOrderItem.MenuItem.ProductCategoryType <> ProductCategory.enumProductCategory.Misc)) Then
                                        dblOrderAmount += .OrderItemAmount - .OrderItemDiscountAmount
                                    End If
                                End With
                            Next
                        Case Else
                            dblOrderAmount = 0
                    End Select
            End Select
            Return Math.Round(dblOrderAmount, 2)
        End Get
    End Property
    '**************************** House A/c Revenue
    Public ReadOnly Property TaxExemptHouseBaseRevenue() As Double
        Get
            'This includes Retail Food Sale to Tax Exempt Retail Customers i.e. not invoiced from QB
            ' excludes NonRevenueSale items & Tips
            Dim dblOrderAmount As Double
            Dim objOrderItem As OrderItem

            Select Case Me.IsHouseACOrder
                Case True
                    Select Case SalesTaxStatus
                        Case Order.enumOrderSalesTaxStatus.NJTaxExempt, enumOrderSalesTaxStatus.OutOfState
                            For Each objOrderItem In Me.AllOrderItems
                                With objOrderItem
                                    If (.Status = OrderItem.enumStatus.Active) AndAlso _
                                        ((objOrderItem.MenuItem Is Nothing) OrElse _
                                         (objOrderItem.MenuItem.ProductCategoryType <> ProductCategory.enumProductCategory.Misc)) Then
                                        dblOrderAmount += .OrderItemAmount - .OrderItemDiscountAmount
                                    End If
                                End With
                            Next
                        Case Else
                            dblOrderAmount = 0
                    End Select
                Case False
                    dblOrderAmount = 0
            End Select
            Return Math.Round(dblOrderAmount, 2)
        End Get
    End Property

    Public ReadOnly Property TaxableHouseBaseRevenue() As Double
        Get
            'This includes Retail Food Sale to Taxable Retail Customers i.e. not invoiced from QB
            ' excludes NonRevenueSale items & Tips
            Dim dblOrderAmount As Double
            Dim objOrderItem As OrderItem

            Select Case Me.IsHouseACOrder
                Case True
                    Select Case SalesTaxStatus
                        Case Order.enumOrderSalesTaxStatus.Taxable
                            For Each objOrderItem In Me.AllOrderItems
                                With objOrderItem
                                    If (.Status = OrderItem.enumStatus.Active) AndAlso _
                                        ((objOrderItem.MenuItem Is Nothing) OrElse _
                                         (objOrderItem.MenuItem.ProductCategoryType <> ProductCategory.enumProductCategory.Misc)) Then
                                        dblOrderAmount += .OrderItemAmount - .OrderItemDiscountAmount
                                    End If
                                End With
                            Next
                        Case Else
                            dblOrderAmount = 0
                    End Select
                Case False
                    dblOrderAmount = 0
            End Select
            Return Math.Round(dblOrderAmount, 2)
        End Get
    End Property
    '******************************
    Public ReadOnly Property OrderBaseAmount() As Double
        Get
            'This includes NonRevenueSale items as well as Tax Exempt Food Sale 
            Dim dblOrderAmount As Double
            Dim objOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems
                If objOrderItem.Status = OrderItem.enumStatus.Active Then
                    dblOrderAmount += objOrderItem.OrderItemAmount
                End If
            Next
            Return Math.Round(dblOrderAmount, 2)
        End Get
    End Property

    Public Property NumberOfSplitChecks() As Integer
        Get
            Return m_NumberOfSplitChecks
        End Get
        Set(ByVal Value As Integer)
            If m_NumberOfSplitChecks < Value Then
                m_NumberOfSplitChecks = Value
                MarkDirty()
            End If
        End Set
    End Property

    Public ReadOnly Property OrderBaseAmount(ByVal CheckNumber As Integer) As Double
        'For a split check, returns base amount for each check
        Get
            'This includes NonRevenueSale items also
            Dim dblOrderAmount As Double
            Dim objOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems
                With objOrderItem
                    If (.Status = OrderItem.enumStatus.Active) AndAlso _
                        (.CheckNumber = CheckNumber) Then
                        dblOrderAmount += objOrderItem.OrderItemAmount
                    End If
                End With
            Next
            Return Math.Round(dblOrderAmount, 2)
        End Get
    End Property

    Public ReadOnly Property TipAmount() As Double
        Get
            Return Math.Round((POSAmount * (TipPercent / 100)), 2)
        End Get
    End Property

    Public ReadOnly Property TipAmount(ByVal CheckNumber As Integer) As Double
        Get
            Return Math.Round((POSAmount(CheckNumber) * (TipPercent / 100)), 2)
        End Get
    End Property

    Public ReadOnly Property DiscountAmount(ByVal CheckNumber As Integer) As Double
        Get
            Select Case Me.Discount
                Case 0
                    Return 0
                Case Else

                    Dim objOrderItem As OrderItem
                    Dim dblDiscountAmount As Double
                    For Each objOrderItem In Me.AllOrderItems
                        With objOrderItem
                            If (.Status = OrderItem.enumStatus.Active) AndAlso _
                                (.CheckNumber = CheckNumber) Then
                                dblDiscountAmount += .OrderItemDiscountAmount
                            End If
                        End With
                    Next
                    Return -Math.Round(dblDiscountAmount, 2)
            End Select
        End Get
    End Property

    Public ReadOnly Property DiscountAmount() As Double
        Get
            Select Case Me.Discount
                Case 0
                    Return 0
                Case Else

                    Dim objOrderItem As OrderItem
                    Dim dblDiscountAmount As Double
                    For Each objOrderItem In Me.AllOrderItems
                        If objOrderItem.Status = OrderItem.enumStatus. _
                                                          Active Then
                            dblDiscountAmount += objOrderItem.OrderItemDiscountAmount
                        End If
                    Next
                    Return -Math.Round(dblDiscountAmount, 2)
            End Select
        End Get
    End Property

    Public ReadOnly Property OrderSalesTaxAmount() As Double
        Get
            Select Case SalesTaxStatus
                Case Order.enumOrderSalesTaxStatus.NJTaxExempt, enumOrderSalesTaxStatus.OutOfState
                    Return 0
                Case Else

                    Dim objOrderItem As OrderItem
                    Dim dblSalesTaxAmount As Double
                    For Each objOrderItem In Me.AllOrderItems
                        If objOrderItem.Status = OrderItem.enumStatus. _
                                                          Active Then
                            dblSalesTaxAmount += objOrderItem.OrderItemTax()
                        End If
                    Next
                    Return Math.Round(dblSalesTaxAmount, 2)
            End Select
        End Get
    End Property

    Public ReadOnly Property OrderSalesTaxAmount(ByVal CheckNumber As Integer) As Double
        Get
            Select Case SalesTaxStatus
                Case Order.enumOrderSalesTaxStatus.NJTaxExempt, enumOrderSalesTaxStatus.OutOfState
                    Return 0
                Case Else
                    Dim objOrderItem As OrderItem
                    Dim dblSalesTaxAmount As Double
                    For Each objOrderItem In Me.AllOrderItems
                        With objOrderItem
                            If (.Status = OrderItem.enumStatus.Active) AndAlso _
                                (.CheckNumber = CheckNumber) Then
                                dblSalesTaxAmount += .OrderItemTax()
                            End If
                        End With
                    Next
                    Return Math.Round(dblSalesTaxAmount, 2)
            End Select
        End Get
    End Property

    Public Property SalesTaxStatus() As Order.enumOrderSalesTaxStatus
        Get
            Return m_OrderSalesTaxStatus
        End Get
        Set(ByVal Value As Order.enumOrderSalesTaxStatus)
            m_OrderSalesTaxStatus = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property POSAmount() As Double
        Get
            Dim dblPOSAmount As Double
            Dim objOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems
                If objOrderItem.Status = OrderItem.enumStatus.Active Then
                    dblPOSAmount += objOrderItem.OrderItemAmount + objOrderItem.OrderItemTax()
                End If
            Next
            Return Math.Round((dblPOSAmount + DiscountAmount), 2)
        End Get
    End Property

    Public ReadOnly Property POSAmount(ByVal CheckNumber As Integer) As Double
        Get
            Dim dblPOSAmount As Double
            Dim objOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems
                With objOrderItem
                    If (.Status = OrderItem.enumStatus.Active) AndAlso _
                        (.CheckNumber = CheckNumber) Then
                        dblPOSAmount += objOrderItem.OrderItemAmount + objOrderItem.OrderItemTax()
                    End If
                End With
            Next
            Return Math.Round((dblPOSAmount + DiscountAmount(CheckNumber)), 2)
        End Get
    End Property

    Public ReadOnly Property PayAmount() As Double
        Get
            Return Math.Round((POSAmount + TipAmount), 2)
        End Get
    End Property

    Public ReadOnly Property PayAmount(ByVal CheckNumber As Integer) As Double
        Get
            Return Math.Round((POSAmount(CheckNumber) + TipAmount(CheckNumber)), 2)
        End Get
    End Property

    Public ReadOnly Property EnteredAt() As DateTime
        Get
            Return m_EnteredAt
        End Get
    End Property

    Private Sub SetEnteredAt()
        If m_EnteredAt > DateTime.MinValue Then
            'NOP : it's already set
        Else
            m_EnteredAt = SaleDays.CreateSaleDay.Now
            MarkDirty()
        End If
    End Sub

    Public ReadOnly Property FirstKOTPrintedAt() As DateTime
        Get
            Return m_FirstKOTPrintedAt
        End Get
    End Property
    Private Sub SetFirstKOTPrintedAt(ByVal KOTAt As DateTime)
        m_FirstKOTPrintedAt = KOTAt
        MarkDirty()
    End Sub

    Public ReadOnly Property LastKOTPrintedAt() As DateTime
        Get
            Return m_LastKOTPrintedAt
        End Get
    End Property
    Private Sub SetLastKOTPrintedAt(ByVal KOTAt As DateTime)
        m_LastKOTPrintedAt = KOTAt
        MarkDirty()
    End Sub
    Public ReadOnly Property FirstCheckPrintedAt() As DateTime
        Get
            Return m_FirstCheckPrintedAt
        End Get
    End Property
    Private Sub SetFirstCheckPrintedAt(ByVal GuestCheckAt As DateTime)
        m_FirstCheckPrintedAt = GuestCheckAt
        MarkDirty()
    End Sub

    Public ReadOnly Property LastCheckPrintedAt() As DateTime
        Get
            Return m_LastCheckPrintedAt
        End Get
    End Property
    Private Sub SetLastCheckPrintedAt(ByVal GuestCheckAt As DateTime)
        m_LastCheckPrintedAt = GuestCheckAt
        MarkDirty()
    End Sub

    Public ReadOnly Property PaidAt() As DateTime
        Get
            Return m_PaidAt
        End Get
    End Property
    Private Sub SetPaidAt(ByVal TimeNow As DateTime)
        If m_PaidAt = DateTime.MinValue Then m_PaidAt = TimeNow
        MarkDirty()
    End Sub

    Public ReadOnly Property PaymentChangedAt() As DateTime
        Get
            Return m_PaymentChangedAt
        End Get
    End Property
    Private Sub SetPaymentChangedAt(ByVal TimeNow As DateTime)
        m_PaymentChangedAt = TimeNow
        MarkDirty()
    End Sub

    Public Property PaymentEnteredBy() As Integer
        Get
            Return m_PaymentEnteredBy
        End Get
        Set(ByVal Value As Integer)
            m_PaymentEnteredBy = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property VoidStatus() As enumOrderVoidStatus
        Get
            Dim intVoidItems As Integer
            Dim objOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems
                Select Case objOrderItem.Status
                    Case OrderItem.enumStatus.Voided
                        intVoidItems += 1
                    Case Else
                        'nope

                End Select
            Next

            If intVoidItems = Me.AllOrderItems.Count Then
                Return Order.enumOrderVoidStatus.Voided
            Else
                If intVoidItems = 0 Then
                    Return enumOrderVoidStatus.NotVoided
                Else
                    Return Order.enumOrderVoidStatus.PartiallyVoided
                End If
            End If

        End Get
    End Property

    Public ReadOnly Property PaidStatus() As enumOrderPaidStatus
        Get
            Select Case Me.PaymentInError
                Case True
                    Return enumOrderPaidStatus.InError
                Case False
                    Select Case Me.BalancePOSAmount > 0
                        Case True
                            If (Me.BalancePOSAmount = Me.POSAmount) AndAlso (m_OrderPaidStatus = enumOrderPaidStatus.PaidButNotEnteredYet) Then
                                Return enumOrderPaidStatus.PaidButNotEnteredYet
                            ElseIf (Me.BalancePOSAmount = Me.POSAmount) Then
                                Return enumOrderPaidStatus.UnPaid
                            Else
                                Return enumOrderPaidStatus.PartiallyPaid
                            End If
                        Case False
                            If Me.AllOrderItems.Count = 0 Then
                                Return enumOrderPaidStatus.UnPaid   'new order, so return unpaid 
                            Else
                                Return enumOrderPaidStatus.FullyPaid
                            End If
                    End Select
            End Select
        End Get
    End Property

    Friend Sub SetPaidButNotEnteredYet()
        m_OrderPaidStatus = enumOrderPaidStatus.PaidButNotEnteredYet
    End Sub

    Public ReadOnly Property PaidBy() As String
        Get
            Dim objPayment As Payment
            Dim strPaidBy As String

            For Each objPayment In Me.AllPayments
                With objPayment
                    If (.Status = Payment.enumStatus.Active) AndAlso (.DEError = Payment.enumPaymentDEError.NoError) Then
                        If strPaidBy = String.Empty Then
                            strPaidBy = .PaymentMethod.ToString
                        Else
                            strPaidBy = strPaidBy & " + " & .PaymentMethod.ToString
                        End If
                    End If
                End With
            Next
            Return strPaidBy
        End Get
    End Property

    Public Property AdultsCount() As Integer
        Get
            Return m_AdultsCount
        End Get
        Set(ByVal Value As Integer)
            m_AdultsCount = Value
            MarkDirty()
        End Set
    End Property

    Public Property KidsCount() As Integer
        Get
            Return m_KidsCount
        End Get
        Set(ByVal Value As Integer)
            m_KidsCount = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property OrderAdultsKidsCount() As String
        Get
            Return m_AdultsCount.ToString & " + " & m_KidsCount.ToString
        End Get
    End Property

    Public Property PromisedAt() As DateTime
        Get
            Return m_OrderPromisedAt
        End Get
        Set(ByVal Value As DateTime)
            m_OrderPromisedAt = Value
            If (m_OrderPromisedAt.Hour >= 6) AndAlso _
                (m_OrderPromisedAt.Hour < 17) Then
                Me.SessionId = Session.enumSessionName.Lunch
            Else
                Me.SessionId = Session.enumSessionName.Dinner
            End If
            MarkDirty()
        End Set
    End Property

    Public Property OrderType() As enumOrderType
        Get
            Return m_OrderType
        End Get
        Set(ByVal Value As enumOrderType)
            m_OrderType = Value
            Select Case m_OrderType
                Case enumOrderType.Delivery, enumOrderType.Pickup
                    'assign a dummy table - called Z1- for all take outs
                    Me.Table = SaleDays.CreateSaleDay.ActiveTables.Item("Z1")
                Case Else

            End Select
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property OrderNumber() As String
        Get
            Dim startDate As Date
            Dim daysElapsed As Integer
            Dim currentOrderNo As String
            startDate = New Date(2003, 1, 1)
            daysElapsed = Saledate.Subtract(startDate).Days
            currentOrderNo = daysElapsed.ToString() & "-" & OrderId.ToString
            Return currentOrderNo

        End Get
    End Property

    ReadOnly Property IsDirty() As Boolean
        Get
            Return m_IsDirty
        End Get
    End Property

    Friend Property IsPersisted() As Boolean
        Get
            Return m_IsPersisted
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersisted = Value
        End Set
    End Property

    Private Sub MarkEndofGroupItems()
        If Me.AllOrderItems.GroupOrderInProgress = False Then
            'error: no Group ordr is in progress
        Else
            Me.AllOrderItems.GroupOrderInProgress = False
        End If
    End Sub

    Private Sub MarkStartofGroupItems()
        If Me.AllOrderItems.GroupOrderInProgress = True Then
            'error: no Group ordr is in progress
        Else
            Me.AllOrderItems.GroupOrderInProgress = True
            Me.AllOrderItems.CurrentGroupId += 1
        End If
    End Sub

    Public Function AddOrderItem(ByVal objMenuItem As MenuItem) As OrderItem
        Dim enumMenuValidate As OrderItem.enumMenuState
        Dim objOrderItem As OrderItem
        Dim validItem As Boolean
        Dim objOverride As Override

        objOverride = GetOverrideForAddOrderItem()
        objOverride.OverrideContext = objMenuItem.ProductNameHindi & " was added AFTER Check was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
        If objOverride.OverrideAvailable = False Then
            Return Nothing
            Exit Function
        End If

        objOrderItem = New OrderItem()
        With objOrderItem
            .ParentOrder = Me
            .OrderItemId = Me.AllOrderItems.Count + 1
            .SetEnteredAt(SaleDays.CreateSaleDay.Now)
        End With
        'enumMenuValidate = objOrderItem.setMenuitem(objMenuItem)
        enumMenuValidate = OrderItem.enumMenuState.Valid

        Select Case enumMenuValidate
            Case OrderItem.enumMenuState.Valid

                Me.AllOrderItems.Add(objOrderItem)
                MarkDirty()
                With objOrderItem
                    Select Case objMenuItem.MenuItemUOM
                        Case MenuItem.enumUoM.Count
                            .UoM = OrderItem.enumUoM.Count
                        Case MenuItem.enumUoM.PerOz
                            .UoM = OrderItem.enumUoM.PerOz
                        Case MenuItem.enumUoM.PerPound
                            .UoM = OrderItem.enumUoM.PerPound
                    End Select

                    .MenuItem = objMenuItem
                    Select Case .MenuItem.ProductPricingType
                        Case Product.enumProductPricingType.SystemPricedSingleItem, Product.enumProductPricingType.UserPricedSingleItem
                            If Me.AllOrderItems.GroupOrderInProgress = False Then
                                .GroupId = 0
                                .Price = objMenuItem.MenuItemPrice
                                .Quantity = 1
                            Else
                                .GroupId = Me.AllOrderItems.CurrentGroupId
                                .Price = 0
                                .Quantity = 1
                            End If
                        Case Product.enumProductPricingType.SystemPricedPackageItem, Product.enumProductPricingType.UserPricedPackageItem
                            MarkStartofGroupItems()
                            .GroupId = Me.AllOrderItems.CurrentGroupId
                            .Price = objMenuItem.MenuItemPrice
                            .Quantity = Math.Max(Me.AdultsCount + Me.KidsCount, 1)
                        Case Product.enumProductPricingType.EndOfPackage
                            .GroupId = Me.AllOrderItems.CurrentGroupId
                            .Price = 0
                            .Quantity = 0
                            MarkEndofGroupItems()
                        Case Else
                            .GroupId = 0
                            .Price = objMenuItem.MenuItemPrice
                            .Quantity = 1
                    End Select
                    .Status = OrderItem.enumStatus.Active
                    .CheckNumber = 1
                    If Me.OrderType = Order.enumOrderType.EatIn Then
                        .Course = SaleDays.CreateSaleDay.ActiveProductCategories.Item(.MenuItem.ProductCategoryName).ProductCategoryCourse
                    Else
                        .Course = "Straight Fire"
                    End If
                End With
                Return objOrderItem

            Case OrderItem.enumMenuState.Invalid
                objOrderItem = Nothing
                Return Nothing
            Case OrderItem.enumMenuState.Existing
                With objOrderItem
                    Select Case objMenuItem.MenuItemUOM
                        Case MenuItem.enumUoM.Count
                            .Quantity = 1
                            .UoM = OrderItem.enumUoM.Count
                        Case MenuItem.enumUoM.PerOz
                            .UoM = OrderItem.enumUoM.PerOz
                        Case MenuItem.enumUoM.PerPound
                            .UoM = OrderItem.enumUoM.PerPound
                    End Select

                    .MenuItem = objMenuItem
                    '.OrderItemNameHindi = objMenuItem.ProductNameHindi
                    .Price = objMenuItem.MenuItemPrice
                    '.OrderItemComboCount = objMenuItem.ProductComboItemCount 'try removing this
                    .GroupId = Me.AllOrderItems.CurrentGroupId
                    .Status = OrderItem.enumStatus.Active
                End With
                Return objOrderItem
        End Select
    End Function

    Public ReadOnly Property AppsCount() As Integer
        Get
            Dim objOrderItem As OrderItem
            Dim intItemCount As Integer

            For Each objOrderItem In Me.AllOrderItems
                Select Case objOrderItem.MenuItem.ProductCategoryType
                    Case ProductCategory.enumProductCategory.Appetizer
                        intItemCount += 1
                End Select
            Next
            Return intItemCount
        End Get
    End Property

    Public ReadOnly Property EntreesCount() As Integer
        Get
            Dim objOrderItem As OrderItem
            Dim intItemCount As Integer

            For Each objOrderItem In Me.AllOrderItems
                Select Case objOrderItem.MenuItem.ProductCategoryType
                    Case ProductCategory.enumProductCategory.ChickenCurries, _
                            ProductCategory.enumProductCategory.KababTikka, _
                            ProductCategory.enumProductCategory.LambCurries, _
                            ProductCategory.enumProductCategory.SeafoodCurries, _
                            ProductCategory.enumProductCategory.VeggieCurries, _
                            ProductCategory.enumProductCategory.VeggieTandoor, _
                            ProductCategory.enumProductCategory.RiceBiryani
                        intItemCount += 1
                End Select
            Next
            Return intItemCount
        End Get
    End Property

    Public ReadOnly Property DessertsCount() As Integer
        Get
            Dim objOrderItem As OrderItem
            Dim intItemCount As Integer

            For Each objOrderItem In Me.AllOrderItems
                Select Case objOrderItem.MenuItem.ProductCategoryType
                    Case ProductCategory.enumProductCategory.Desserts
                        intItemCount += 1
                End Select
            Next
            Return intItemCount
        End Get
    End Property

    Public ReadOnly Property DrinksCount() As Integer
        Get
            Dim objOrderItem As OrderItem
            Dim intItemCount As Integer

            For Each objOrderItem In Me.AllOrderItems
                Select Case objOrderItem.MenuItem.ProductCategoryType
                    Case ProductCategory.enumProductCategory.HotDrinks, _
                            ProductCategory.enumProductCategory.ColdDrinks
                        intItemCount += 1
                End Select
            Next
            Return intItemCount
        End Get
    End Property
    Public ReadOnly Property OrderItemsCount() As Integer
        Get
            Return m_objOrderItems.Count
        End Get
    End Property

    Public Sub ResetAllOrderItems()
        m_objOrderItems = Nothing
    End Sub

    Public ReadOnly Property AllOrderItems() As OrderItems
        Get
            If m_objOrderItems Is Nothing Then
                Select Case Me.OrderId
                    Case 0              'New Order
                        m_objOrderItems = New OrderItems()
                    Case Else           'Persisted Orders
                        m_objOrderItems = New OrderItems(OrderItems.EnumFilter.All, _
                                                            OrderItems.EnumView.CompleteView, _
                                                            Me.OrderId)
                        m_objOrderItems.SetParentOrder(Me)
                End Select
            End If
            Return m_objOrderItems
        End Get
    End Property

    Public Sub ResetAllPayments()
        m_objPayments = Nothing
    End Sub

    Public ReadOnly Property AllPayments() As Payments
        Get
            If m_objPayments Is Nothing Then
                Select Case Me.IsPersisted
                    Case False              'New Order
                        m_objPayments = New Payments()
                    Case True          'Persisted Orders
                        m_objPayments = New Payments(Payments.EnumFilter.All, _
                                                        Payments.EnumView.CompleteView, _
                                                        OrderId)
                End Select
                m_objPayments.SetParentOrder(Me)
            End If
            Return m_objPayments
        End Get
    End Property

    Public Function AddPayment() As Payment

        Return AllPayments.AddPayment
    End Function

    Public ReadOnly Property GetOrderItem(ByVal index As Integer) As OrderItem
        Get
            Return m_objOrderItems.Item(index)
        End Get
    End Property

    Public ReadOnly Property ItemsCount() As Integer
        Get
            Dim intItemCount As Integer
            Dim intIndex As Integer
            Dim objOrderItem As OrderItem
            Dim lastOrderItem As OrderItem
            For Each objOrderItem In Me.AllOrderItems
                Select Case objOrderItem.Status
                    Case OrderItem.enumStatus.Active
                        If objOrderItem.OrderItemComboCount = 1 Then
                            intItemCount += 1
                        Else
                            'nope
                        End If
                    Case Else
                        'nope
                End Select
            Next

            Return intItemCount
        End Get
    End Property

    Public Function SetData() As Integer
        Dim strSqlCmd As String
        Dim intOrderId As Integer
        Dim intSQLReturnValue As Integer
        Dim i As Integer
        Dim strFirstKOTAt As String
        Dim strLastKOTAt As String
        Dim strFirstCheckAt As String
        Dim strLastCheckAt As String
        Dim strPaidAt As String
        Dim strPaymentChangedAt As String
        Dim strIsDesiOrder As String
        Dim strIsHouseACOrder As String
        Dim strHouseACCustomerId As String

        If Me.FirstKOTPrintedAt = Date.MinValue Then
            strFirstKOTAt = "NULL"
        Else
            strFirstKOTAt = "'" & Me.FirstKOTPrintedAt.ToString & "'"
        End If
        If Me.LastKOTPrintedAt = Date.MinValue Then
            strLastKOTAt = "NULL"
        Else
            strLastKOTAt = "'" & Me.LastKOTPrintedAt.ToString & "'"
        End If
        If Me.FirstCheckPrintedAt = Date.MinValue Then
            strFirstCheckAt = "NULL"
        Else
            strFirstCheckAt = "'" & Me.FirstCheckPrintedAt.ToString & "'"
        End If
        If Me.LastCheckPrintedAt = Date.MinValue Then
            strLastCheckAt = "NULL"
        Else
            strLastCheckAt = "'" & Me.LastCheckPrintedAt.ToString & "'"
        End If
        If Me.PaidAt = Date.MinValue Then
            strPaidAt = "NULL"
        Else
            strPaidAt = "'" & Me.PaidAt.ToString & "'"
        End If
        If Me.IsDesiOrder = True Then
            strIsDesiOrder = "1"
        Else
            strIsDesiOrder = "0"
        End If
        If Me.IsHouseACOrder = True Then
            strIsHouseACOrder = "1"
        Else
            strIsHouseACOrder = "0"
        End If
        If Me.PaymentChangedAt = Date.MinValue Then
            strPaymentChangedAt = "NULL"
        Else
            strPaymentChangedAt = "'" & Me.PaymentChangedAt.ToString & "'"
        End If

        If Me.Customer Is Nothing Then
            strHouseACCustomerId = "-1"
        Else
            strHouseACCustomerId = Me.Customer.CustomerId.ToString
        End If
        DataAccess.CreateDataAccess.StartTxn()
        If Me.IsDirty Then
            Select Case Me.IsPersisted      'True means update row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into [Order] values (" & _
                                Me.Table.TableId.ToString & "," & _
                                Me.TableSeating.ToString & "," & _
                                Me.CRId.ToString & "," & _
                                strHouseACCustomerId & "," & _
                                Me.Waiter.EmployeeId.ToString & ",'" & _
                                Me.Saledate.ToShortDateString & "'," & _
                                CStr(Me.SessionId) & "," & _
                                CStr(Me.OrderType) & "," & _
                                Me.AdultsCount.ToString & "," & _
                                Me.KidsCount.ToString & "," & _
                                Me.OrderBaseAmount.ToString & "," & _
                                Me.OrderSalesTaxAmount.ToString & "," & _
                                CStr(Me.SalesTaxStatus) & "," & _
                                CStr(Me.DiscountType) & "," & _
                                Me.DiscountAmount.ToString & "," & _
                                CStr(Me.TipType) & "," & _
                                Me.TipAmount.ToString & ",'" & _
                                Me.EnteredAt.ToString & "','" & _
                                Me.PromisedAt.ToString & "'," & _
                                strFirstKOTAt & "," & _
                                strLastKOTAt & "," & _
                                strFirstCheckAt & "," & _
                                strLastCheckAt & "," & _
                                strPaidAt & "," & _
                                CStr(Me.PaidStatus) & "," & _
                                CStr(Me.VoidStatus) & "," & _
                                Me.PaymentEnteredBy.ToString & "," & _
                                strIsDesiOrder & "," & _
                                strIsHouseACOrder & ",' " & _
                                Me.HouseACCheckSignedBy & "',' " & _
                                Me.TakeOutCustomerName & "',' " & _
                                Me.TakeOutCustomerAddress & "',' " & _
                                Me.TakeOutCustomerPhone & "'," & _
                                strPaymentChangedAt & "," & _
                                Me.NumberOfSplitChecks.ToString & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue <= 0 Then
                        'Error in insert procesing on SQL server
                    Else
                        Me.OrderId = intSQLReturnValue
                    End If

                Case Else
                    strSqlCmd = "update [Order] set  " & _
                                "TableId=" & _
                                    Me.Table.TableId.ToString & "," & _
                                "TableSeating=" & _
                                    Me.TableSeating.ToString & "," & _
                                "CRId=" & _
                                    Me.CRId.ToString & "," & _
                                "CustomerId=" & _
                                    strHouseACCustomerId & "," & _
                                "WaiterId=" & _
                                    Me.Waiter.EmployeeId.ToString & "," & _
                                "SaleDate='" & _
                                    Me.Saledate.ToShortDateString & "'," & _
                                "SessionId=" & _
                                    CStr(Me.SessionId) & "," & _
                                "OrderType=" & _
                                    CStr(Me.OrderType) & "," & _
                                "AdultsCount=" & _
                                    Me.AdultsCount.ToString & "," & _
                                "KidsCount=" & _
                                    Me.KidsCount.ToString & "," & _
                                "BaseAmount=" & _
                                    Me.OrderBaseAmount.ToString & "," & _
                                "SalesTaxAmount=" & _
                                    Me.OrderSalesTaxAmount.ToString & "," & _
                                "SalesTaxStatus=" & _
                                    CStr(Me.SalesTaxStatus) & "," & _
                                "DiscountType=" & _
                                    CStr(Me.DiscountType) & "," & _
                                "DiscountAmount=" & _
                                    Me.DiscountAmount.ToString & "," & _
                                "TipType=" & _
                                    CStr(Me.TipType) & "," & _
                                "TipAmount=" & _
                                    Me.TipAmount.ToString & "," & _
                                "EnteredAt='" & _
                                    Me.EnteredAt.ToString & "'," & _
                                "PromisedAt='" & _
                                    Me.PromisedAt.ToString & "'," & _
                                "FirstKOTPrintedAt=" & _
                                    strFirstKOTAt & "," & _
                                "LastKOTPrintedAt=" & _
                                    strLastKOTAt & "," & _
                                "FirstCheckPrintedAt=" & _
                                    strFirstCheckAt & "," & _
                                "LastCheckPrintedAt=" & _
                                    strLastCheckAt & "," & _
                                "PaidAt=" & _
                                    strPaidAt & "," & _
                                "PaidStatus=" & _
                                    CStr(Me.PaidStatus) & "," & _
                                "VoidStatus=" & _
                                    CStr(Me.VoidStatus) & "," & _
                                "PaymentEnteredBy=" & _
                                    Me.PaymentEnteredBy.ToString & "," & _
                                "IsHouseACOrder=" & _
                                    strIsHouseACOrder & "," & _
                                "HouseACCheckSignedBy=" & "' " & _
                                    Me.HouseACCheckSignedBy & "'," & _
                                "TakeOutCustomerName=" & "' " & _
                                    Me.TakeOutCustomerName & "'," & _
                                 "TakeOutCustomerAddress=" & "' " & _
                                    Me.TakeOutCustomerAddress & "'," & _
                                 "TakeOutCustomerPhone=" & "' " & _
                                    Me.TakeOutCustomerPhone & "'," & _
                                "PaymentChangedAt=" & _
                                    strPaymentChangedAt & "," & _
                                "NumberOfSplitChecks=" & _
                                    Me.NumberOfSplitChecks.ToString & _
                                    " where OrderId=" & Me.OrderId.ToString

                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
        End If
        For i = 1 To 2
            If intSQLReturnValue < 0 Then
                Exit For
            Else
                Select Case i
                    Case 1
                        intSQLReturnValue = Me.AllOrderItems.SetData()
                    Case 2
                        intSQLReturnValue = Me.AllPayments.SetData()
                    Case 3
                End Select
            End If
        Next

        If intSQLReturnValue < 0 Then
            'NOP
        Else
            DataAccess.CreateDataAccess.EndTxn()
            Me.ParentAllOrders.FilterAnOrder(Me)
            ResetOverrides()
            m_IsDirty = False
            m_IsPersisted = True
        End If
        Return intSQLReturnValue
    End Function

    Private Sub ResetOverrides()
        'Make sure that no root level Overrides can be used again after Setdata
        If m_objOverrideForPaymentEdit Is Nothing Then
            'nop
        Else
            m_objOverrideForPaymentEdit = Nothing
        End If

        If m_objOverrideForAddOrderItem Is Nothing Then
            'nop
        Else
            m_objOverrideForAddOrderItem = Nothing
        End If
        If m_objOverrideForOrderEdit Is Nothing Then
            'nop
        Else
            m_objOverrideForOrderEdit = Nothing
        End If
        If m_objGuestCheckOverride Is Nothing Then
            'nop
        Else
            m_objGuestCheckOverride = Nothing
        End If
        If m_objVoidOverride Is Nothing Then
            'nop
        Else
            m_objVoidOverride = Nothing
        End If
    End Sub

    Friend Sub MarkDirty()
        If m_IsDirty Then
            'NOP
        Else
            m_IsDirty = True
        End If
    End Sub

    Public Property TipType() As enumTips
        Get
            Return m_TipType
        End Get
        Set(ByVal Value As enumTips)
            m_TipType = Value
        End Set
    End Property

    Public ReadOnly Property TipPercent() As Integer
        Get
            Select Case TipType
                Case enumTips.NoTipsAdded
                    Return 0
                Case enumTips.TipAt15PercentAddedFor6OrMore
                    Return 15
                Case enumTips.TipAt18PercentAddedFor6OrMore
                    Return 18
                Case enumTips.TipAt20PercentAddedFor6OrMore
                    Return 20
            End Select
        End Get
    End Property

    Public Property DiscountType() As enumMGCoupon
        Get
            Return m_DiscountType
        End Get
        Set(ByVal Value As enumMGCoupon)
            m_DiscountType = Value
        End Set
    End Property

    Public ReadOnly Property Discount() As Integer
        Get
            Select Case DiscountType
                Case enumMGCoupon.HappyCurryHourDiscount, enumMGCoupon.McArthurCoupon
                    Return 10               'in %
                Case enumMGCoupon.NoDiscount
                    Return 0                'in %
            End Select
        End Get

    End Property

    Private ReadOnly Property KOTOverride() As Override
        Get
            Dim objKOTOverride As Override
            objKOTOverride = New Override()
            With objKOTOverride
                .OverrideType = Override.enumOverrideType.PrintKOT
                .OverrideOldRowId = Me.OrderId
                .OverrideNewRowId = Me.OrderId
                Select Case Me.FirstKOTPrintedAt <= DateTime.MinValue
                    Case True
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                        .OverrideAvailable = True
                    Case False
                        Select Case AllOrderItems.KOTPrintedInFull
                            Case True
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                                .OverrideContext = "Duplicate KOT was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                            Case False
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                                .OverrideAvailable = True
                        End Select
                End Select
            End With
            Return objKOTOverride
        End Get
    End Property

    Public ReadOnly Property KOTText() As PosDoc
        Get
            Dim KOTDoc As PosDoc
            Dim objKOTOverride As Override
            Dim KOTAt As DateTime
            Dim intResultCode As Integer

            objKOTOverride = KOTOverride

            With objKOTOverride
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(objKOTOverride)
                    Case Else
                        'nop
                End Select

                'Reset KOTprinted time for all order items for duplicate KOT print
                If (.OverrideAvailable = True) AndAlso _
                    ((.OverrideLevelNeeded = Override.enumOverrideLevel.ManagerNeeded) OrElse (.OverrideLevelNeeded = Override.enumOverrideLevel.OwnerNeeded)) Then
                    Me.AllOrderItems.UpdateKOTPrintedAt(DateTime.MinValue)
                End If
            End With

            If objKOTOverride.OverrideAvailable = True Then
                KOTDoc = ComposeKOT(objKOTOverride)
                KOTAt = SaleDays.CreateSaleDay.Now
                With Me
                    If .FirstKOTPrintedAt > DateTime.MinValue Then
                        .SetLastKOTPrintedAt(KOTAt)
                    Else
                        .SetFirstKOTPrintedAt(KOTAt)
                    End If
                    .AllOrderItems.UpdateKOTPrintedAt(KOTAt)
                End With
                intResultCode = Me.SetData
            Else
                KOTDoc = Nothing
            End If

            Return KOTDoc
        End Get
    End Property

    Private Function ComposeKOT(ByVal objKOTOverride As Override) As PosDoc
        Dim strLine As String
        Dim strLine2 As String
        Dim strItemName As String

        Dim strCustomerName As String
        Dim intModlength As Integer
        Dim alOrderitems As ArrayList
        Dim alObject As Object
        Dim strPrevKOTGroupKey As String
        Dim objOrderItem As OrderItem
        Dim KOTDoc As New PosDoc()

        With KOTDoc
            .POSDocType = PosDoc.enumPOSDocType.KOT
            .Copies = 2
        End With
        'KOT Header
        Select Case objKOTOverride.OverrideLevelNeeded
            Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                strLine = ("KOT-DUPLICATE".PadLeft(12))
            Case Override.enumOverrideLevel.NotNeeded
                strLine = ("KOT-ORIGINAL".PadLeft(12))
        End Select
        KOTDoc.AddHeaderLine(strLine)
        With Me
            'KOT Id line
            strLine = ("Order: " & .OrderId.ToString & "/" & _
                        "CR-" & SaleDays.CreateSaleDay.CRId.ToString & _
                        " " & (Format(.EnteredAt, "MM/dd/yy hh:mm tt"))).PadLeft(12)
            KOTDoc.AddHeaderLine(strLine)
            strLine = ("Printed at " & (Format(SaleDays.CreateSaleDay.Now, "MM/dd/yy hh:mm tt"))).PadLeft(20)

            KOTDoc.AddHeaderLine(strLine)
            If .IsDesiOrder = True Then
                KOTDoc.AddHeaderLine("_____________________________ D E S I _________")
            Else
                KOTDoc.AddHeaderLine("_______________________________________________")
            End If
        End With

        'Order Item Line
        alOrderitems = Me.AllOrderItems.GetOrderItemsSortedList
        For Each alObject In alOrderitems
            objOrderItem = CType(alObject, OrderItem)

            With objOrderItem
                Select Case (.KOTPrintedAt <= DateTime.MinValue) AndAlso (.Status = OrderItem.enumStatus.Active) AndAlso _
                            ((.MenuItem.ProductPricingType = Product.enumProductPricingType.UserPricedSingleItem) OrElse (.MenuItem.ProductPricingType = Product.enumProductPricingType.SystemPricedSingleItem))
                    Case True
                        'Print only those active items not printed before
                        'Always skip Combo or Package item
                        If strPrevKOTGroupKey = .Course Then
                            'nop
                        Else
                            'change of Course & Category so insert a heading
                            'strPrevKOTGroupKey = .Course & ": (" & .MenuItem.ProductCategoryName & ")"
                            strPrevKOTGroupKey = .Course
                            strLine = "** " & strPrevKOTGroupKey & " **"
                            KOTDoc.AddDetailLine(strLine)
                            KOTDoc.AddDetailLine(" ")
                        End If

                        Select Case .Status
                            Case OrderItem.enumStatus.Active
                                If .OrderItemNameHindi.Length > 15 Then
                                    strItemName = (" " & .OrderItemNameHindi.Substring(0, 15)).PadLeft(16)
                                Else
                                    strItemName = (" " & .OrderItemNameHindi).PadLeft(16)
                                End If
                                Select Case Me.AllOrderItems.IsItemAComboSubItem(objOrderItem)
                                    Case True
                                        strLine = strItemName
                                    Case False
                                        'strLine = (objOrderItem.OrderItemNameHindi.PadLeft((objOrderItem.OrderItemNameHindi.Length + 3), " "c))
                                        strLine = strItemName
                                        Select Case .UoM
                                            Case OrderItem.enumUoM.Count
                                                strLine = strLine & (.Quantity.ToString.PadLeft(4, "."c))

                                            Case OrderItem.enumUoM.PerOz
                                                strLine = strLine & (.Quantity.ToString.PadLeft(4, "."c) + _
                                                                    "oz".PadLeft(4, " "c))
                                            Case OrderItem.enumUoM.PerPound
                                                strLine = strLine & (.Quantity.ToString.PadLeft(4, "."c) + _
                                                                    "lb".PadLeft(4, " "c))
                                        End Select
                                        KOTDoc.AddDetailLine(strLine)

                                End Select
                        End Select

                        'Order Item Mod lines
                        If (.ItemMod = String.Empty) AndAlso (.ItemModNote = String.Empty) Then
                            strLine2 = String.Empty
                        ElseIf (.ItemModNote = String.Empty) Then
                            strLine2 = " =>" & .ItemMod
                        ElseIf (.ItemMod = String.Empty) Then
                            strLine2 = " =>" & .ItemModNote
                        Else
                            strLine2 = " =>" & .ItemMod & "=>" & .ItemModNote
                        End If
                        If strLine2.Length > 26 Then  'we have space for 26 or 10 chars for normal & double size fonts respectively
                            'strLine = strLine '& strLine2.Substring(0, 9)
                            'KOTDoc.AddDetailLine(strLine)
                            strLine = (strLine2.Substring(26))
                            KOTDoc.AddDetailLine((strLine))
                        ElseIf (strLine2 = String.Empty) Then
                            'nop
                        Else
                            'strLine = strLine & strLine2.TrimEnd
                            KOTDoc.AddDetailLine(strLine2.TrimEnd)
                        End If
                        KOTDoc.AddDetailLine(" ")
                    Case False
                        'skip this voided item 
                End Select
            End With
        Next
        If Me.TakeOutCustomerName.Length > 10 Then
            strCustomerName = Me.TakeOutCustomerName.Substring(0, 10)
        Else
            strCustomerName = Me.TakeOutCustomerName
        End If
        Select Case Me.OrderType
            Case enumOrderType.Delivery
                strLine = (Me.TableSeating.ToString & " " & strCustomerName)
                strLine2 = ("___________DELIVERY Promised at " & Me.PromisedAt.ToShortTimeString & "___________")
            Case enumOrderType.Pickup
                strLine = Me.TableSeating.ToString & " " & strCustomerName
                strLine2 = ("___________PICKUP Promised at " & Me.PromisedAt.ToShortTimeString & "___________")
            Case enumOrderType.EatIn
                strLine = Me.Table.TableName & "-" & Me.TableSeating.ToString
                strLine2 = "_______" & Me.Waiter.EmployeeShortName & "  Promised at " & Me.PromisedAt.ToShortTimeString & "______"
        End Select
        KOTDoc.AddFooterLine(strLine)
        KOTDoc.AddFooterLine(strLine2)
        Return KOTDoc
    End Function
    '*************** Prepare Guest Check start
    Public ReadOnly Property GuestCheckText(ByVal checkNumber As Integer) As PosDoc
        Get
            Dim GuestCheckDoc As PosDoc
            Dim GuestCheckAt As DateTime
            Dim intResultCode As Integer
            If checkNumber > Me.NumberOfSplitChecks Then
                GuestCheckDoc = Nothing
            Else
                SetGuestCheckOverride()

                With m_objGuestCheckOverride
                    Select Case .OverrideLevelNeeded
                        Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                            If (.OverrideAvailable = True) Then
                                'nop
                            Else
                                RaiseEvent OverrideNeeded(m_objGuestCheckOverride)
                            End If
                        Case Else
                            'nop
                    End Select

                    'Reset GuestCheckprinted time for all order items for duplicate GuestCheck print
                    If (.OverrideAvailable = True) AndAlso _
                        ((.OverrideLevelNeeded = Override.enumOverrideLevel.ManagerNeeded) OrElse (.OverrideLevelNeeded = Override.enumOverrideLevel.OwnerNeeded)) Then
                        Me.AllOrderItems.UpdateGuestCheckPrintedAt(DateTime.MinValue)
                    End If
                End With

                If m_objGuestCheckOverride.OverrideAvailable = True Then
                    GuestCheckAt = SaleDays.CreateSaleDay.Now
                    With Me
                        If .FirstCheckPrintedAt > DateTime.MinValue Then
                            .SetLastCheckPrintedAt(GuestCheckAt)
                        Else
                            .SetFirstCheckPrintedAt(GuestCheckAt)
                        End If
                        .AllOrderItems.UpdateGuestCheckPrintedAt(GuestCheckAt)
                    End With
                    GuestCheckDoc = ComposeGuestCheck(m_objGuestCheckOverride, checkNumber)
                    'update only when last check is printed to avoid repetitive overrides
                    If checkNumber = Me.NumberOfSplitChecks Then intResultCode = Me.SetData
                Else
                    GuestCheckDoc = Nothing
                End If
            End If

            Return GuestCheckDoc
        End Get
    End Property

    Private Sub SetGuestCheckOverride()
        If (m_objGuestCheckOverride Is Nothing) OrElse _
            (m_objGuestCheckOverride.OverrideAvailable = False) Then
            m_objGuestCheckOverride = New Override()
            With m_objGuestCheckOverride
                .OverrideType = Override.enumOverrideType.PrintGuestCheck
                .OverrideOldRowId = Me.OrderId
                .OverrideNewRowId = Me.OrderId
                Select Case Me.FirstCheckPrintedAt <= DateTime.MinValue
                    Case True
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                        .OverrideAvailable = True
                    Case False
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                        .OverrideContext = "Duplicate Guest Check was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                End Select
            End With
        End If
    End Sub

    Private Sub ComposeGuestCheckHdr(ByVal GuestCheckDoc As PosDoc, ByVal checkNumber As Integer)
        Dim strLine As String
        Dim checkAt As DateTime

        'GuestCheck Name & Address
        strLine = "Masala Grill".PadLeft(18)
        GuestCheckDoc.AddHeaderLine(strLine)

        strLine = ("19 Chambers St. Princeton NJ 08542").PadLeft(40)
        GuestCheckDoc.AddHeaderLine(strLine)

        strLine = ("Phone:(609)921-0500").PadLeft(33)
        GuestCheckDoc.AddHeaderLine(strLine)

        strLine = "E-Mail your Comments, Catering Requests To:"
        GuestCheckDoc.AddHeaderLine(strLine)
        strLine = "masalagrill@msn.com"
        GuestCheckDoc.AddHeaderLine(strLine.PadLeft(30))
        'Select Case Me.FirstCheckPrintedAt > DateTime.MinValue
        '    Case True
        '        strLine = ("**** Duplicate Check ****".PadLeft(35))
        '        GuestCheckDoc.AddHeaderLine(strLine)
        '    Case False
        '        GuestCheckDoc.AddHeaderLine(("_________________").PadLeft(44))
        'End Select
        With Me
            Select Case .OrderType
                Case enumOrderType.Delivery
                    strLine = ("Delivery:" & .TakeOutCustomerName).PadLeft(30)
                Case enumOrderType.Pickup
                    strLine = ("Pick up:" & .TakeOutCustomerName).PadLeft(30)
                Case enumOrderType.EatIn
                    If .NumberOfSplitChecks > 1 Then
                        strLine = (.Table.TableName & "-" & .TableSeating.ToString & " check " & _
                                    checkNumber.ToString & " of " & Me.NumberOfSplitChecks.ToString & _
                                    " (" & .Waiter.EmployeeShortName & ")").PadLeft(34)
                    Else
                        strLine = (.Table.TableName & "-" & .TableSeating.ToString & _
                                    " (" & .Waiter.EmployeeShortName & ")").PadLeft(34)
                    End If
            End Select
            GuestCheckDoc.AddHeaderLine(strLine)
            If .LastCheckPrintedAt > DateTime.MinValue Then
                checkAt = .LastCheckPrintedAt
            Else
                checkAt = .FirstCheckPrintedAt
            End If

            'GuestCheck Id line

            strLine = ("CR-" & SaleDays.CreateSaleDay.CRId.ToString & "/" & .OrderId.ToString & _
                        " Entered:" & (Format(.EnteredAt, "MM/dd/yy hh:mm tt"))).PadLeft(40)
            GuestCheckDoc.AddHeaderLine(strLine)
            strLine = ("  Printed:" & (Format(checkAt, "MM/dd/yy hh:mm tt"))).PadLeft(40)
            GuestCheckDoc.AddHeaderLine(strLine)
        End With
    End Sub

    Private Sub ComposeGuestCheckDetail(ByVal GuestCheckDoc As PosDoc, ByVal checkNumber As Integer)
        Dim strLine As String
        'Dim strLine2 As String
        'Dim checkAt As DateTime
        Dim alOrderitems As ArrayList
        Dim alObject As Object
        'Dim strPrevGuestCheckGroupKey As String
        Dim objOrderItem As OrderItem

        GuestCheckDoc.AddDetailLine(("******** We hope you enjoyed your meal ********").PadLeft(30))

        'Order Item Line
        alOrderitems = Me.AllOrderItems.GetOrderItemsSortedList
        For Each alObject In alOrderitems
            objOrderItem = CType(alObject, OrderItem)

            With objOrderItem

                Select Case (.Status = OrderItem.enumStatus.Active) AndAlso _
                            (.OrderItemAmount > 0) AndAlso _
                            (.CheckNumber = checkNumber)
                    Case True
                        Select Case Me.AllOrderItems.IsItemAComboSubItem(objOrderItem)
                            Case True
                                strLine = ("   " + objOrderItem.OrderItemName.PadLeft(20, " "c))
                            Case False
                                'strLine = (objOrderItem.OrderItemNameHindi.PadLeft((objOrderItem.OrderItemNameHindi.Length + 3), " "c))
                                strLine = (Microsoft.VisualBasic.Left(objOrderItem.OrderItemName, 26)).PadLeft(30, " "c)
                                Select Case objOrderItem.UoM
                                    Case OrderItem.enumUoM.Count
                                        strLine = (objOrderItem.Quantity.ToString.PadRight(3, " "c)) & strLine

                                    Case OrderItem.enumUoM.PerOz
                                        strLine = strLine & (objOrderItem.Quantity.ToString.PadLeft(6, "."c) & _
                                                            "oz".PadRight(4, " "c)) & strLine
                                    Case OrderItem.enumUoM.PerPound
                                        strLine = strLine & (objOrderItem.Quantity.ToString.PadLeft(6, "."c) & _
                                                            "lb".PadRight(4, " "c)) & strLine
                                End Select
                                'append the amount
                                strLine = strLine & (Format(.OrderItemAmount, "C")).PadLeft(10, "."c)
                        End Select
                        'GuestCheckDoc.AddDetailLine((strLine).PadLeft(34))
                        GuestCheckDoc.AddDetailLine((strLine))

                    Case False
                        'skip this item (void or free item like rice or some other check item)
                End Select
            End With
        Next

        With Me
            'Food total, discount & Tax lines
            GuestCheckDoc.AddDetailLine(("_________").PadLeft(43))

            strLine = "TOTAL FOOD ".PadLeft(29, " "c) & (Format(.OrderBaseAmount(checkNumber), "C")).PadLeft(10, "."c)
            GuestCheckDoc.AddDetailLine(strLine.PadLeft(43))

            If .DiscountAmount(checkNumber) < 0 Then
                'strLine = ("Less:" & [Enum].GetName(GetType(Order.enumMGCoupon), .DiscountType)).PadLeft(20, " "c) & _
                strLine = ("==> LESS: Discount " & CStr(.DiscountType)).PadLeft(29, " "c) & _
                            (Format(.DiscountAmount(checkNumber), "C")).PadLeft(10, "."c)
                GuestCheckDoc.AddDetailLine((strLine).PadLeft(43))

                GuestCheckDoc.AddDetailLine(("_________").PadLeft(43))


                strLine = ("Net").PadLeft(29, " "c) & _
                             (Format(.OrderBaseAmount(checkNumber) + .DiscountAmount(checkNumber), "C")).PadLeft(10, "."c)
                GuestCheckDoc.AddDetailLine((strLine).PadLeft(43))
            End If

            strLine = ("NJ Sales Tax ").PadLeft(29, " "c) & _
                             (Format(.OrderSalesTaxAmount(checkNumber), "C")).PadLeft(10, "."c)
            GuestCheckDoc.AddDetailLine((strLine).PadLeft(43))

            GuestCheckDoc.AddDetailLine(("_________").PadLeft(43))

            strLine = ("Total Charge ").PadLeft(29, " "c) & _
                             (Format(.POSAmount(checkNumber), "C")).PadLeft(10, "."c)
            GuestCheckDoc.AddDetailLine((strLine).PadLeft(43))

            GuestCheckDoc.AddDetailLine(("***********************************************").PadLeft(40))
        End With
    End Sub

    Private Function ComposeGuestCheck(ByVal objGuestCheckOverride As Override, ByVal checkNumber As Integer) As PosDoc
        'Composes text of a required check number for a split check order 
        'or entire check for a non-split order
        Dim checkAt As DateTime
        Dim alOrderitems As ArrayList
        Dim alObject As Object
        Dim strPrevGuestCheckGroupKey As String
        Dim objOrderItem As OrderItem
        Dim GuestCheckDoc As New PosDoc()
        If checkNumber > Me.NumberOfSplitChecks Then
            'nop
        Else
            With GuestCheckDoc
                .POSDocType = PosDoc.enumPOSDocType.GuestCheck
                .Copies = 1
            End With
            ComposeGuestCheckHdr(GuestCheckDoc, checkNumber)
            ComposeGuestCheckDetail(GuestCheckDoc, checkNumber)
            ComposeGuestCheckFooter(GuestCheckDoc, checkNumber)
            Return GuestCheckDoc
        End If
    End Function

    Private Sub ComposeGuestCheckFooter(ByVal GuestCheckDoc As PosDoc, ByVal checkNumber As Integer)
        Dim strLine As String
        Dim strLine2 As String

        'Check Total Summary
        With Me
            strLine = ("Total Charge ").PadLeft(14, " "c) & _
                             (Format(.POSAmount(checkNumber), "C")).PadLeft(8, "."c)
            GuestCheckDoc.AddFooterLine(strLine)

            If .TipType = enumTips.NoTipsAdded Then
                strLine = ("+ Tip ").PadLeft(14, " "c)
                GuestCheckDoc.AddFooterLine(strLine)
                GuestCheckDoc.AddFooterLine(("_______").PadLeft(20))
                strLine = .Table.TableName & "-" & .TableSeating.ToString & ("Total To Pay").PadLeft(13, " "c)
                GuestCheckDoc.AddFooterLine(strLine)
            Else
                strLine = ("Tip Added").PadLeft(14, " "c) & _
                             (Format(.TipAmount(checkNumber), "C")).PadLeft(8, "."c)
                GuestCheckDoc.AddFooterLine(strLine)
                GuestCheckDoc.AddFooterLine(("_______").PadLeft(24))
                strLine = ("Total To Pay").PadLeft(14, " "c) & _
                              (Format(.PayAmount(checkNumber), "C")).PadLeft(8, " "c)
                GuestCheckDoc.AddFooterLine(strLine)
                strLine = .Table.TableName & "-" & .TableSeating.ToString
                GuestCheckDoc.AddFooterLine(strLine)
            End If
        End With

        'House Account Name & Sign block in the footer
        If Me.IsHouseACOrder Then
            strLine = Me.Customer.CustomerName
            If Me.HouseACCheckSignedBy Is Nothing Then
                strLine2 = ""
            Else
                strLine2 = Me.HouseACCheckSignedBy
            End If

            If strLine.Length > 24 Then strLine = strLine.Substring(0, 24)
            If strLine2.Length > 24 Then strLine2 = strLine2.Substring(0, 24)

            Select Case Me.Customer.CustomerType
                Case Customer.enumCustomerType.CorpAccount, Customer.enumCustomerType.PUAccount
                    GuestCheckDoc.AddFooterLine(" ")        'add a blank line
                    GuestCheckDoc.AddFooterLine("*House Account Payment")
                    GuestCheckDoc.AddFooterLine(" ")
                    GuestCheckDoc.AddFooterLine(strLine)
                    GuestCheckDoc.AddFooterLine(" ")        'add a blank line
                    GuestCheckDoc.AddFooterLine("Name:" & strLine2) '.PadLeft(20))
                    GuestCheckDoc.AddFooterLine(" ")
                    GuestCheckDoc.AddFooterLine("   ____________________")
                    GuestCheckDoc.AddFooterLine("Phone:") '.PadLeft(20))
                    GuestCheckDoc.AddFooterLine(" ")
                    GuestCheckDoc.AddFooterLine("   ____________________")
                    GuestCheckDoc.AddFooterLine("Signature:") '.PadLeft(18))
                    GuestCheckDoc.AddFooterLine(" ")
                    GuestCheckDoc.AddFooterLine("   ____________________")
                Case Else
            End Select
        End If

        'Customer Message in the footer
        GuestCheckDoc.AddFooterLine(" ")        'add a blank line
        GuestCheckDoc.AddFooterLine(" ")        'add a blank line
        GuestCheckDoc.AddFooterLine(GetCustomerMsg)
    End Sub

    Private Function GetCustomerMsg() As String
        Dim intMsg As Integer
        Dim strMsg As String

        Randomize()
        intMsg = CInt(Int(3 * Rnd()))  ' Generate random value between 0 and 3.
        Select Case CType(intMsg, enumCustomerMsg)
            Case enumCustomerMsg.HappyCurryHour
                strMsg = "Save 10% on Take Out Orders. Everyday, Any time"
            Case enumCustomerMsg.HolidayParties
                strMsg = "Have you tried our Bulk Menu and Catering Services for your parties?"
            Case enumCustomerMsg.LunchBuffet
                strMsg = "All You Can Eat Lunch Buffet from more than a dozen choices for only $9.95"
            Case enumCustomerMsg.MGGiftCertificate
                strMsg = "Masala Grill Gift Certificate makes a great gift.It's Easy To Get & Use."
        End Select
        Return strMsg
    End Function

    Public ReadOnly Property GuestCheckPrintedInFull() As Boolean
        Get
            Dim objOrderItem As OrderItem

            For Each objOrderItem In Me.AllOrderItems
                If objOrderItem.CheckPrintedAt <= DateTime.MinValue Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property

    Public Sub VoidAnOrderItem(ByVal OrderItemIndex As Integer)
        If (m_objVoidOverride Is Nothing) OrElse _
            (m_objVoidOverride.OverrideAvailable = False) Then
            SetVoidOverride(OrderItemIndex)
            If m_objVoidOverride.OverrideAvailable = False Then RaiseEvent OverrideNeeded(m_objVoidOverride)
        End If
        Select Case m_objVoidOverride.OverrideLevelNeeded
            Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                If m_objVoidOverride.OverrideAvailable = True Then
                    'set statuts of orderitem to void
                    SetVoidStatus(OrderItemIndex)
                Else
                    m_objVoidOverride = Nothing
                End If
            Case Else
                SetVoidStatus(OrderItemIndex)
                'Me.AllOrderItems.RemoveAt(OrderItemIndex)   'deletion discontinued due to no setdata code for delete from database
        End Select
    End Sub

    Private Sub SetVoidStatus(ByVal OrderItemIndex As Integer)
        Dim objOrderItem As OrderItem
        Dim intGroupId As Integer

        objOrderItem = Me.AllOrderItems.Item(OrderItemIndex)

        Select Case objOrderItem.MenuItem.ProductPricingType
            Case Product.enumProductPricingType.UserPricedSingleItem, Product.enumProductPricingType.SystemPricedSingleItem
                objOrderItem.Status = OrderItem.enumStatus.Voided
            Case Else
                'Void entire Group
                intGroupId = objOrderItem.GroupId
                Do
                    objOrderItem.Status = OrderItem.enumStatus.Voided
                    OrderItemIndex += 1
                    If OrderItemIndex > Me.AllOrderItems.Count - 1 Then Exit Do
                    objOrderItem = Me.AllOrderItems.Item(OrderItemIndex)
                    'loop back & void it until end of package
                    If objOrderItem.GroupId <> intGroupId Then Exit Do
                Loop
        End Select
    End Sub

    Private Sub SetVoidOverride(ByVal OrderItemIndex As Integer)
        m_objVoidOverride = New Override()
        With m_objVoidOverride
            .OverrideType = Override.enumOverrideType.VoidOrderItem
            .OverrideOldRowId = Me.AllOrderItems.Item(OrderItemIndex).OrderItemId
            .OverrideNewRowId = Me.AllOrderItems.Item(OrderItemIndex).OrderItemId
            Select Case Me.AllOrderItems.Item(OrderItemIndex).KOTPrintedAt <= DateTime.MinValue
                Case True
                    Select Case Me.FirstCheckPrintedAt <= DateTime.MinValue
                        Case True
                            .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                            .OverrideAvailable = True
                        Case False
                            .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                            .OverrideContext = "An Item was voided AFTER Check was printed (NO KOT was printed) for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                    End Select

                Case False
                    .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                    .OverrideContext = "An Item was voided AFTER KOT was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
            End Select
        End With
    End Sub
    '*************** Override for Discount or Tips-Added-To-Order  **********************************
    Public Function AllowedToAddTipsOrDiscount() As Boolean
        Dim objOverride As Override

        'This override is exactly same as the one required for Adding an Order Item after a guest
        'check is printed. So we will use the same code
        objOverride = GetOverrideForAddOrderItem()
        objOverride.OverrideContext = "Tips Or Discount was added AFTER Check was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
        Return objOverride.OverrideAvailable
    End Function

    '*************** Override for AddOrderItem when Check Is Printed*********************************
    Private Function GetOverrideForAddOrderItem() As Override
        If (m_objOverrideForAddOrderItem Is Nothing) OrElse _
            (m_objOverrideForAddOrderItem.OverrideAvailable = False) Then
            m_objOverrideForAddOrderItem = OverrideForAddOrderItem
            With m_objOverrideForAddOrderItem
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(m_objOverrideForAddOrderItem)
                        'nop here: addorderitem will do the actual work
                    Case Else
                        'nop
                End Select
            End With
        Else
            'Use an existing override to avoid repetitive requests for override
        End If

        Return m_objOverrideForAddOrderItem
    End Function

    Private ReadOnly Property OverrideForAddOrderItem() As Override
        Get
            m_objOverrideForAddOrderItem = New Override()

            With m_objOverrideForAddOrderItem
                .OverrideType = Override.enumOverrideType.AddOrderItem
                .OverrideOldRowId = Me.OrderId
                .OverrideNewRowId = Me.OrderId
                Select Case Me.FirstCheckPrintedAt > DateTime.MinValue
                    Case True
                        Select Case Me.PaidStatus
                            Case enumOrderPaidStatus.UnPaid
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                            Case Else
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                        End Select
                    Case Else
                        Select Case Me.PaidStatus
                            Case enumOrderPaidStatus.UnPaid
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                                .OverrideAvailable = True
                            Case Else
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                        End Select

                End Select
            End With

            Return m_objOverrideForAddOrderItem
        End Get
    End Property

    '*************** Override for changing House A/C Customer after Reconciliation or after Check is printed *********************************
    Public ReadOnly Property AllowHouseACEdit() As Boolean
        Get
            Dim objOverrideForHouseACEdit As Override
            objOverrideForHouseACEdit = OverrideForHouseACEdit
            With objOverrideForHouseACEdit
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(objOverrideForHouseACEdit)
                        'nop here: caller will do the actual work
                    Case Else
                        'nop
                End Select
                Return .OverrideAvailable
            End With
        End Get
    End Property

    Private ReadOnly Property OverrideForHouseACEdit() As Override
        Get
            Dim objSaleDay As SaleDay
            Dim objSDSession As SaleDaySession
            Dim objOverrideForHouseACEdit As Override

            objOverrideForHouseACEdit = New Override()
            With objOverrideForHouseACEdit
                .OverrideType = Override.enumOverrideType.EditHouseACOrder
                .OverrideOldRowId = Me.OrderId
                .OverrideNewRowId = Me.OrderId
                Select Case Me.orderSDSession.ReconciliationApprovedAt > DateTime.MinValue
                    Case True
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                        .OverrideContext = "A House A/C customer was linked to or removed AFTER Lunch/Dinner was reconciled and closed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                    Case False
                        Select Case Me.PaidStatus
                            Case enumOrderPaidStatus.FullyPaid, enumOrderPaidStatus.PartiallyPaid
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                                .OverrideContext = "A House A/C customer was linked to or removed AFTER Payment was entered for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                            Case Else
                                Select Case (Me.FirstCheckPrintedAt > Date.MinValue)
                                    Case True
                                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                                        .OverrideContext = "A House A/C customer was linked to or removed AFTER Check was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                                    Case False
                                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                                        .OverrideAvailable = True
                                End Select
                        End Select
                End Select
            End With

            Return objOverrideForHouseACEdit
        End Get
    End Property
    '*************** Override for editing Order Items after KOT/Check is printed *********************************
    Public ReadOnly Property AllowOrderEdit(ByVal objOrderItem As OrderItem) As Boolean
        Get
            SetOverrideForOrderEdit(objOrderItem)
            Return m_objOverrideForOrderEdit.OverrideAvailable
        End Get
    End Property

    Private Sub SetOverrideForOrderEdit(ByVal objOrderItem As OrderItem)
        If (m_objOverrideForOrderEdit Is Nothing) OrElse _
            (m_objOverrideForOrderEdit.OverrideAvailable = False) Then
            m_objOverrideForOrderEdit = OverrideForOrderEdit(objOrderItem)
            With m_objOverrideForOrderEdit
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(m_objOverrideForOrderEdit)
                        'nop here: caller will do the actual work
                    Case Else
                        'nop
                End Select
            End With
        Else
            'Use an existing override to avoid repetitive requests for override
        End If
    End Sub

    Private ReadOnly Property OverrideForOrderEdit(ByVal objOrderItem As OrderItem) As Override
        Get
            Dim objSaleDay As SaleDay
            Dim objSDSession As SaleDaySession

            m_objOverrideForOrderEdit = New Override()
            With m_objOverrideForOrderEdit
                .OverrideType = Override.enumOverrideType.EditOrderItem
                .OverrideOldRowId = objOrderItem.OrderItemId
                .OverrideNewRowId = objOrderItem.OrderItemId
                Select Case Me.orderSDSession.ReconciliationApprovedAt > DateTime.MinValue
                    Case True
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                        .OverrideContext = "An Order Item was changed AFTER Lunch/Dinner was reconciled and closed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                    Case False
                        Select Case Me.PaidStatus
                            Case enumOrderPaidStatus.FullyPaid, enumOrderPaidStatus.PartiallyPaid
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                                .OverrideContext = "An Order Item was changed AFTER Payment was entered for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                            Case Else
                                Select Case (objOrderItem.KOTPrintedAt > Date.MinValue) Or (objOrderItem.CheckPrintedAt > Date.MinValue)
                                    Case True
                                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                                        .OverrideContext = "An Order Item was changed AFTER KOT or Check was printed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                                    Case False
                                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                                        .OverrideAvailable = True
                                End Select
                        End Select
                End Select
            End With

            Return m_objOverrideForOrderEdit
        End Get
    End Property
    '*************************************************
    '*************** Override for editing Payment object *********************************
    Public ReadOnly Property AllowPaymentEdit() As Boolean
        Get
            SetOverrideForPaymentEdit()
            With m_objOverrideForPaymentEdit
                If .OverrideAvailable = True Then
                    Return True
                Else
                    Return False
                End If
            End With
        End Get
    End Property

    Private Sub SetOverrideForPaymentEdit()
        If (m_objOverrideForPaymentEdit Is Nothing) OrElse _
            (m_objOverrideForPaymentEdit.OverrideAvailable = False) Then
            m_objOverrideForPaymentEdit = OverrideForPaymentEdit
            With m_objOverrideForPaymentEdit
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(m_objOverrideForPaymentEdit)
                        'nop here: caller will do the actual work
                    Case Else
                        'nop
                End Select
            End With
        Else
            'Use an existing override to avoid repetitive requests for override
        End If
    End Sub

    Private ReadOnly Property OverrideForPaymentEdit() As Override
        Get
            Dim objSaleDay As SaleDay
            Dim objSDSession As SaleDaySession

            m_objOverrideForPaymentEdit = New Override()
            With m_objOverrideForPaymentEdit
                .OverrideType = Override.enumOverrideType.EditPayment
                .OverrideOldRowId = Me.OrderId
                .OverrideNewRowId = Me.OrderId
                Select Case Me.orderSDSession.ReconciliationApprovedAt > DateTime.MinValue
                    Case True
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                        .OverrideContext = "A Payment was changed AFTER Lunch/Dinner was reconciled and closed for " & Me.Table.TableName & "-" & Me.TableSeating.ToString
                    Case False
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                        .OverrideAvailable = True
                End Select
            End With

            Return m_objOverrideForPaymentEdit
        End Get
    End Property
    '*************************************************
    Public ReadOnly Property orderSaleDay() As SaleDay
        Get
            Return SaleDays.CreateSaleDay.Item(Me.Saledate, True)
        End Get
    End Property

    Public ReadOnly Property orderSDSession() As SaleDaySession
        Get
            Return orderSaleDay.AllSaleDaySessions.Item(Me.SessionId)
        End Get
    End Property

    Public ReadOnly Property POSAmountPaid() As Double
        Get
            Dim objPayment As Payment
            Dim dblAmount As Double
            For Each objPayment In Me.AllPayments
                With objPayment
                    If .Status = Payment.enumStatus.Active Then
                        dblAmount += .POSAmountPaid
                    End If
                End With
            Next
            Return Math.Round((dblAmount), 2)
        End Get
    End Property

    Public ReadOnly Property TipAmountPaid() As Double
        Get
            Dim objPayment As Payment
            Dim dblAmount As Double
            For Each objPayment In Me.AllPayments
                With objPayment
                    If .Status = Payment.enumStatus.Active Then
                        dblAmount += .TipAmountPaid
                    End If
                End With
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property TipAmountPaidPercent() As Double
        Get
            Dim objPayment As Payment
            Dim dblAmount As Double
            If Me.OrderBaseAmount > 0 Then
                For Each objPayment In Me.AllPayments
                    With objPayment
                        If .Status = Payment.enumStatus.Active Then
                            dblAmount += .TipAmountPaid
                        End If
                    End With
                Next
                Return (dblAmount / Me.OrderBaseAmount) * 100
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property AmountTendered() As Double
        Get
            Dim objPayment As Payment
            Dim dblAmount As Double
            For Each objPayment In Me.AllPayments
                With objPayment
                    If .Status = Payment.enumStatus.Active Then
                        dblAmount += .AmountTendered
                    End If
                End With
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property ChangeAmountReturned() As Double
        Get
            Dim objPayment As Payment
            Dim dblAmount As Double
            For Each objPayment In Me.AllPayments
                With objPayment
                    If .Status = Payment.enumStatus.Active Then
                        dblAmount += .ChangeAmountReturned
                    End If
                End With
            Next
            Return Math.Round(dblAmount, 2)
        End Get
    End Property

    Public ReadOnly Property BalancePOSAmount() As Double
        Get
            Dim diff As Double
            diff = (Me.POSAmount - Me.POSAmountPaid)
            If Math.Abs(diff) < 0.01 Then
                Return 0
            Else
                Return Math.Round(diff, 2)
            End If
        End Get
    End Property

    Public ReadOnly Property BalanceTipAmount() As Double
        Get
            Dim diff As Double
            If Me.TipAmount < 0.01 Then
                Return 0
            Else
                diff = (Me.TipAmount - Me.TipAmountPaid)
                If (Math.Abs(diff) < 0.01) OrElse (diff < 0) Then
                    Return 0
                Else
                    Return Math.Round(diff, 2)
                End If
            End If
        End Get
    End Property

    Public Sub VoidAPayment(ByVal PaymentIndex As Integer)
        Dim objPayment As Payment
        With Me.AllPayments.Item(PaymentIndex)
            Select Case AllowPaymentEdit
                Case True
                    .Status = Payment.enumStatus.Voided
                Case False
                    'nop
            End Select
        End With
    End Sub
    Public ReadOnly Property PaymentInError() As Boolean
        Get
            Dim objPayment As Payment
            Dim blnError As Boolean

            For Each objPayment In Me.AllPayments
                With objPayment
                    If (.Status = Payment.enumStatus.Voided) OrElse (.DEError = Payment.enumPaymentDEError.NoError) Then
                        'nop
                    Else
                        blnError = True
                        Exit For
                    End If
                End With
            Next
            Return blnError
        End Get
    End Property


    Public Sub SetPaymentEndTime()
        Dim objPayment As Payment
        Dim TimeNow As DateTime

        TimeNow = SaleDays.CreateSaleDay.Now()
        For Each objPayment In Me.AllPayments
            With objPayment
                If .IsDirty Then .EndTime = TimeNow
            End With
        Next
        Me.SetPaidAt(TimeNow)
        Me.SetPaymentChangedAt(TimeNow)
    End Sub
End Class
