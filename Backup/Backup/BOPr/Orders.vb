Option Strict On
Imports System.Data.SqlClient
Imports System.Collections
Public Class Orders
    Inherits POSBOs

    Public Event OverrideNeeded(ByVal newOverride As Override)
    Public Enum EnumView As Integer  'This enum determines the select clause
        BaseView
        CompleteView
    End Enum

    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Closed
        Open
        Voided
        PartiallyVoided
    End Enum
    Public Enum EnumFilterOrderType As Integer
        EatIn
        TakeOut
        Delivery
        CustomerPickup
    End Enum
    Public Enum EnumFilterOrderTypeOrSection As Integer
        EatInAll
        EatInA
        EatInB
        EatInM
        EatInT
        EatInW
        TakeOut
        Delivery
        CustomerPickup
    End Enum

    Private m_FilteredOrders As Orders 'Orders meeting all Filter values for display purposes
    Private m_PaidOrders As Orders 'Orders for Reconciliaton
    'Filters for orders
    Private m_FilterOrderStatus As Orders.EnumFilter
    Private m_FilterDiningSection As DTable.enumDiningSection
    Private m_FilterOrderTypeOrSection As Orders.EnumFilterOrderTypeOrSection
    Private m_IsValidFilteredOrders As Boolean

    Private m_TableSeating As Hashtable

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal fromDate As Date, ByVal ToDate As Date)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objOrder As Order

        sqlcommand = "Select * from [Order]"
        sqlcommand = sqlcommand & " where PromisedAt >'" & fromDate & "'" & _
                                  " and PromisedAt <= '" & ToDate.AddDays(1) & "'"


        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objOrder = New Order(SqlDr)
                Me.Add(objOrder)
                objOrder.LoadData(SqlDr)
                objOrder.ParentAllOrders = Me
            Loop
            LoadTableSeating()
        End If
    End Sub

    Public Sub New(ByVal enumOrderView As Orders.EnumView, ByVal SaleDate As Date, _
                    ByVal SessionId As Session.enumSessionName, _
                    ByVal paymentStatus As EnumFilter)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objOrder As Order

        Select Case enumOrderView
            Case EnumView.BaseView
                sqlcommand = "Select OrderId,CRId, CashierId,Saledate From [Order]"

            Case EnumView.CompleteView
                sqlcommand = "Select * from [Order]"
        End Select
        sqlcommand = sqlcommand & " where Saledate='" & SaleDate.ToShortDateString & "'" & _
                                  " and SessionId=" & CStr(SessionId)
        Select Case paymentStatus
            Case EnumFilter.All
                'nop
            Case EnumFilter.Closed
                sqlcommand = sqlcommand & " and PaidStatus = " & CStr(Order.enumOrderPaidStatus.FullyPaid)
            Case EnumFilter.Open
                sqlcommand = sqlcommand & " and PaidStatus In (= " & CStr(Order.enumOrderPaidStatus.PaidButNotEnteredYet) & "," & _
                                CStr(Order.enumOrderPaidStatus.PartiallyPaid) & "," & _
                                CStr(Order.enumOrderPaidStatus.UnPaid) & ")"
            Case EnumFilter.PartiallyVoided
                sqlcommand = sqlcommand & " and VoidStatus = " & Order.enumOrderVoidStatus.PartiallyVoided
            Case EnumFilter.Voided
                sqlcommand = sqlcommand & " and VoidStatus = " & Order.enumOrderVoidStatus.Voided
        End Select

        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objOrder = New Order(SqlDr)
                Me.Add(objOrder)
                objOrder.LoadData(SqlDr)
                objOrder.ParentAllOrders = Me
            Loop
            SqlDr.Close()
            objDataAccess.CloseConnection()
            LoadTableSeating()
        End If
    End Sub

    Private Sub LoadTableSeating()
        Dim objTable As DTable
        Dim objOrder As Order

        m_TableSeating = New Hashtable()

        For Each objOrder In Me
            Dim intSeating As Integer

            intSeating = CInt(m_TableSeating.Item(objOrder.Table.TableName))
            If objOrder.TableSeating > CInt(m_TableSeating.Item(objOrder.Table.TableName)) Then
                m_TableSeating.Item(objOrder.Table.TableName) = objOrder.TableSeating
            End If
        Next
    End Sub

    Private Function GetCurrentTableSeating(ByVal strTableName As String) As Integer
        Return CInt(m_TableSeating.Item(strTableName))
    End Function

    Friend Function SetCurrentTableSeating(ByVal strTableName As String) As Integer
        Dim intSeating As Integer

        intSeating = GetCurrentTableSeating(strTableName) + 1
        m_TableSeating.Item(strTableName) = intSeating
        Return intSeating
    End Function

    Public Function IsTableAvailable(ByVal strTableName As String) As Boolean
        Dim objPaymentReceivedOverride As Override
        Dim LastOrderForTable As Order
        Dim LastSeating As Integer
        Dim intIndex As Integer

        With Me
            LastSeating = .GetCurrentTableSeating(strTableName)
            intIndex = .IndexOf(strTableName & LastSeating.ToString)
            If intIndex < 0 Then
                LastOrderForTable = Nothing
            Else
                LastOrderForTable = .Item(intIndex)
            End If
        End With

        If (LastOrderForTable Is Nothing) OrElse _
            (LastOrderForTable.FirstCheckPrintedAt > DateTime.MinValue) OrElse _
            ((LastOrderForTable.PromisedAt.Subtract(DateTime.Now)).TotalHours > 1) Then
            Return True
        Else
            objPaymentReceivedOverride = New Override()
            With objPaymentReceivedOverride
                .OverrideType = Override.enumOverrideType.MarkPaymentReceivedButNotEntered
                .OverrideOldRowId = LastOrderForTable.OrderId
                .OverrideNewRowId = LastOrderForTable.OrderId
                .OverrideContext = "New Order was opened BEFORE check was printed for " & _
                                    LastOrderForTable.Table.TableName & "-" & LastOrderForTable.TableSeating.ToString
                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
            End With
            RaiseEvent OverrideNeeded(objPaymentReceivedOverride)
            If objPaymentReceivedOverride.OverrideAvailable = True Then
                LastOrderForTable.SetPaidButNotEnteredYet()
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public ReadOnly Property Item(ByVal index As Integer) As Order
        Get
            Return CType(MyBase.itemPOSBO(index), Order)
        End Get

    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myorderIds() As Integer

            Return myorderIds

        End Get
    End Property

    Public Sub Add(ByRef value As Order)
        MyBase.addPOSBO(CType(value, POSBO))
    End Sub

    Public Function AddNewOrder() As Order
        Dim objOrder As Order
        objOrder = New Order()
        Me.Add(objOrder)
        objOrder.ParentAllOrders = Me
        'AddToFilteredOrders(objOrder)
        'm_FilteredOrders = Nothing
        Return objOrder
    End Function

    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public ReadOnly Property IndexOf(ByVal objOrder As Order) As Integer
        Get
            Return MyBase.List.IndexOf(objOrder)
        End Get
    End Property

    Public ReadOnly Property IndexOf(ByVal strOrderName As String) As Integer
        'OrderName is Table Name & TableSeating (e.g. W11 or M52 etc.)
        Get
            Dim objOrder As Order
            Dim intIndex As Integer = -1

            For Each objOrder In Me
                With objOrder
                    If .Table.TableName & .TableSeating.ToString = strOrderName Then
                        intIndex = Me.IndexOf(objOrder)
                        Exit For
                    End If
                End With
            Next
            Return intIndex
        End Get
    End Property

    Public Function Contains(ByVal value As Order) As Boolean
        Return MyBase.containsPOSBO(CType(value, POSBO))
    End Function

    Public Property FilterOrderStatus() As Orders.EnumFilter
        Get
            Return m_FilterOrderStatus
        End Get
        Set(ByVal newFilterValue As Orders.EnumFilter)
            If m_FilterOrderStatus = newFilterValue Then
                'nop
            Else
                m_FilteredOrders = Nothing
                m_FilterOrderStatus = newFilterValue
            End If
        End Set
    End Property


    Public Property FilterOrderTypeOrSection() As Orders.EnumFilterOrderTypeOrSection
        Get
            Return m_FilterOrderTypeOrSection
        End Get
        Set(ByVal newFilterValue As Orders.EnumFilterOrderTypeOrSection)
            If m_FilterOrderTypeOrSection = newFilterValue Then
                'nop
            Else
                m_FilteredOrders = Nothing
                m_FilterOrderTypeOrSection = newFilterValue
            End If
        End Set
    End Property

    'Not Used Any More
    Public ReadOnly Property PaidOrders(ByVal fromTime As DateTime, ByVal ToTime As DateTime) As Orders       'To display Orders meeting all Filters 
        Get
            Return m_PaidOrders
        End Get
    End Property
    Friend Sub SetPaidOrders(ByVal fromTime As DateTime, ByVal ToTime As DateTime)
        'Creates a subset of Me containing Orders paid between fromTime & ToTime
        m_PaidOrders = New Orders()
        If Me.Count > 0 Then
            LoadPaidOrders(0, Me.Count - 1, fromTime, ToTime)
        End If
    End Sub

    Private Sub LoadPaidOrders(ByVal fromIndex As Integer, ByVal toIndex As Integer, ByVal fromTime As DateTime, ByVal ToTime As DateTime)
        Dim objOrder As Order
        Dim i As Integer
        Dim iFrom As Integer
        Dim iTo As Integer

        iFrom = Math.Min(Math.Max(fromIndex, 0), Me.Count - 1)
        iTo = Math.Max(iFrom, Math.Min(toIndex, Me.Count - 1))
        For i = iFrom To iTo
            objOrder = Me.Item(i)
            If objOrder.PaidAt >= fromTime And objOrder.PaidAt <= ToTime Then
                m_PaidOrders.Add(objOrder)
            End If
        Next
    End Sub


    Public Sub RefreshFilteredOrdersd()
        m_FilteredOrders = Nothing
    End Sub

    Public ReadOnly Property FilteredOrders() As Orders        'To display Orders meeting all Filters 
        Get
            If m_FilteredOrders Is Nothing Then
                m_FilteredOrders = New Orders()
                If Me.Count > 0 Then
                    LoadFilteredOrders(0, Me.Count - 1)
                End If
                'Else
                'nop
            End If
            Return m_FilteredOrders
        End Get
    End Property

    Private Sub LoadFilteredOrders(ByVal fromIndex As Integer, ByVal toIndex As Integer)
        Dim objOrder As Order
        Dim i As Integer
        Dim iFrom As Integer
        Dim iTo As Integer

        iFrom = Math.Min(Math.Max(fromIndex, 0), Me.Count - 1)
        iTo = Math.Max(iFrom, Math.Min(toIndex, Me.Count - 1))
        For i = iFrom To iTo
            objOrder = Me.Item(i)
            AddToFilteredOrders(objOrder)
        Next
    End Sub

    Private Sub AddToFilteredOrders(ByVal objOrder As Order)
        If (ApplyOrderStatusFilter(objOrder) = True) AndAlso _
                (ApplyOrderTypeOrSectionFilter(objOrder) = True) Then
            m_FilteredOrders.Add(objOrder)
        End If
    End Sub

    Private Sub RemoveFromFilteredOrders(ByVal objOrder As Order)
        If (ApplyOrderStatusFilter(objOrder) = False) OrElse _
                (ApplyOrderTypeOrSectionFilter(objOrder) = False) Then
            m_FilteredOrders.Remove(m_FilteredOrders.IndexOf(objOrder))
        End If
    End Sub

    Friend Sub FilterAnOrder(ByVal objOrder As Order)
        If FilteredOrders Is Nothing Then           'this will never be the case
            'nop
        Else
            Select Case m_FilteredOrders.Contains(objOrder)
                Case True
                    RemoveFromFilteredOrders(objOrder)
                Case False
                    AddToFilteredOrders(objOrder)
            End Select
        End If
    End Sub

    Private Function ApplyOrderStatusFilter(ByVal objOrder As Order) As Boolean
        Dim IsOrderSelected As Boolean

        With objOrder
            Select Case Me.FilterOrderStatus
                Case EnumFilter.All
                    IsOrderSelected = True
                Case EnumFilter.Closed
                    Select Case .PaidStatus
                        Case Order.enumOrderPaidStatus.FullyPaid
                            IsOrderSelected = True
                        Case Else
                            IsOrderSelected = False
                    End Select
                Case EnumFilter.Open
                    Select Case .PaidStatus
                        Case Order.enumOrderPaidStatus.FullyPaid
                            IsOrderSelected = False
                        Case Else
                            IsOrderSelected = True
                    End Select

                Case EnumFilter.PartiallyVoided
                    Select Case .VoidStatus
                        Case Order.enumOrderVoidStatus.PartiallyVoided
                            IsOrderSelected = True
                        Case Else
                            IsOrderSelected = False
                    End Select
                Case EnumFilter.Voided
                    Select Case .VoidStatus
                        Case Order.enumOrderVoidStatus.Voided
                            IsOrderSelected = True
                        Case Else
                            IsOrderSelected = False
                    End Select
            End Select
        End With
        Return IsOrderSelected
    End Function

    Private Function ApplyOrderTypeOrSectionFilter(ByVal objOrder As Order) As Boolean
        Dim IsOrderSelected As Boolean
        With objOrder
            Select Case Me.FilterOrderTypeOrSection
                Case EnumFilterOrderTypeOrSection.TakeOut
                    If .Table.DiningSection = DTable.enumDiningSection.Z Then IsOrderSelected = True
                Case EnumFilterOrderTypeOrSection.EatInA
                    If .Table.DiningSection = DTable.enumDiningSection.A Then IsOrderSelected = True
                Case EnumFilterOrderTypeOrSection.EatInB
                    If .Table.DiningSection = DTable.enumDiningSection.B Then IsOrderSelected = True
                Case EnumFilterOrderTypeOrSection.EatInM
                    If .Table.DiningSection = DTable.enumDiningSection.M Then IsOrderSelected = True
                Case EnumFilterOrderTypeOrSection.EatInT
                    If .Table.DiningSection = DTable.enumDiningSection.T Then IsOrderSelected = True
                Case EnumFilterOrderTypeOrSection.EatInW
                    If .Table.DiningSection = DTable.enumDiningSection.W Then IsOrderSelected = True
                Case EnumFilterOrderTypeOrSection.EatInAll
                    If .OrderType = Order.enumOrderType.EatIn Then IsOrderSelected = True
            End Select
        End With

        Return IsOrderSelected
    End Function
    Public Sub SetData()
        Dim objOrder As Order

        For Each objOrder In MyBase.List
            objOrder.SetData()
        Next
    End Sub

End Class

