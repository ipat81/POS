Option Strict On
Imports System.Data.SqlClient

Public Class OrderItems
    Inherits POSBOs

    Private m_CurrentCombo As MenuItem
    Private m_CurrentComboPrice As Double
    Private m_CurrentComboItemsCount As Integer
    Private m_ComboSelected As Boolean
    Private m_CurrentGroupId As Integer
    Private m_GroupOrderInProgress As Boolean

    'Public Event WrongMenuSelection(ByVal errorText As String)

    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView

    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        NotVoided
        Voided

    End Enum


    Public Sub New()

    End Sub


    Public Sub New(ByVal enumOrderItemsFilter As EnumFilter, _
                    ByVal enumOrderItemsView As EnumView, _
                    ByVal newOrderId As Integer)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objOrderItem As OrderItem

        Select Case enumOrderItemsView
            Case EnumView.BaseView
                sqlcommand = "Select OrderItemId,OrderId,MenuItemId From OrderItem"

            Case EnumView.CompleteView
                sqlcommand = "Select * from OrderItem"
        End Select
        sqlcommand = sqlcommand & " where OrderId=" & newOrderId.ToString

        Select Case enumOrderItemsFilter
            Case EnumFilter.All
            Case EnumFilter.NotVoided
                sqlcommand = sqlcommand & " and  OrderItemVoidedAt IS  NULL "
            Case EnumFilter.Voided
                sqlcommand = sqlcommand & " and OrderItemVoidedAt IS NOT NULL "
        End Select
        sqlcommand = sqlcommand & " order by OrderItemId"
        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objOrderItem = New OrderItem()
                Me.Add(objOrderItem)
                objOrderItem.LoadData(SqlDr)
            Loop
            SqlDr.Close()
        End If
        objDataAccess.CloseConnection()
    End Sub

    Friend Sub SetParentOrder(ByVal objParentOrder As Order)
        Dim objOrderitem As OrderItem
        For Each objOrderitem In Me
            objOrderitem.ParentOrder = objParentOrder
        Next
    End Sub

    Public Property CurrentComboItemsCount() As Integer

        Get
            Return m_CurrentComboItemsCount
        End Get

        Set(ByVal Value As Integer)
            Dim currentCombo As MenuItem = Nothing
            m_CurrentComboItemsCount = Value
            If Me.CurrentCombo Is Nothing Then
                'Nop
            Else
                If m_CurrentComboItemsCount = Me.CurrentCombo.ProductComboItemCount Then
                    setCurrentCombo(currentCombo)
                    m_CurrentComboItemsCount = 0
                End If
            End If

        End Set

    End Property



    Public ReadOnly Property CurrentCombo() As MenuItem
        Get
            Return m_CurrentCombo
        End Get

    End Property

    Private Sub setCurrentCombo(ByVal objMenuItem As MenuItem)
        m_CurrentCombo = objMenuItem
    End Sub

    Friend Function validateCurrentCombo(ByVal objMenuItem As MenuItem) As OrderItem.enumMenuState
        Dim validMenuState As OrderItem.enumMenuState
        If CurrentCombo Is Nothing Then
            CurrentGroupId += 1
            CurrentComboPrice = objMenuItem.MenuItemPrice
            setCurrentCombo(objMenuItem)
            Return OrderItem.enumMenuState.Valid
        Else
            Return OrderItem.enumMenuState.Invalid
        End If
    End Function


    Public Function currentIndex(ByVal value As OrderItem) As Integer
        Dim objOrderItem As OrderItem
        Dim intIndex As Integer

        For Each objOrderItem In Me
            intIndex += 1
            If objOrderItem Is value Then
                currentIndex = intIndex
            End If
        Next
        Return currentIndex
    End Function


    Friend Property CurrentComboPrice() As Double
        Get
            Return m_CurrentComboPrice
        End Get
        Set(ByVal Value As Double)
            m_CurrentComboPrice = Value
        End Set
    End Property

    Public Property CurrentGroupId() As Integer
        Get
            Return m_CurrentGroupId
        End Get
        Set(ByVal Value As Integer)
            m_CurrentGroupId = Value
        End Set
    End Property

    Public Property GroupOrderInProgress() As Boolean
        Get
            Return m_GroupOrderInProgress
        End Get
        Set(ByVal Value As Boolean)
            m_GroupOrderInProgress = Value
        End Set
    End Property

    Friend Sub CompareComboPrice(ByVal objMenuItem As MenuItem)
        If objMenuItem.MenuItemPrice > CurrentComboPrice Then
            CurrentComboPrice = objMenuItem.MenuItemPrice
            updateCurrentCombo()
        End If
    End Sub
    Private Sub updateCurrentCombo()

        If Me.LatestCombo Is Nothing Then
            'nope
        Else
            Me.LatestCombo.Price = CurrentComboPrice
        End If

    End Sub


    Private ReadOnly Property LatestCombo() As OrderItem
        Get
            Dim index As Integer
            Dim i As Integer
            Dim objOrderItem As OrderItem
            Dim objLatestCombo As OrderItem
            If Me.Count = 0 Then
                'nop
            Else
                For Each objOrderItem In Me

                    If objOrderItem.OrderItemComboCount > 1 Then
                        index = i
                    End If
                    i += 1
                Next
            End If
            Return Me.Item(index)
        End Get
    End Property

    Public Function IsItemAComboSubItem(ByVal objOrderItem As OrderItem) As Boolean
        Dim objComboItem As OrderItem
        Dim groupId As Integer
        Dim IsASubItem As Boolean
        groupId = objOrderItem.GroupId
        If objOrderItem.OrderItemComboCount = 1 Then
            For Each objComboItem In Me
                If objComboItem.GroupId = groupId And _
                         objComboItem.OrderItemComboCount > 1 Then
                    IsASubItem = True
                    Exit For
                Else
                    IsASubItem = False
                End If
            Next
        Else
            IsASubItem = False
        End If
        Return IsASubItem
    End Function

    Public Function getSubItemCombo(ByVal objOrderItem As OrderItem) As OrderItem

        Dim objComboItem As OrderItem
        Dim objSubItem As OrderItem
        Dim groupId As Integer

        If IsItemAComboSubItem(objOrderItem) Then

            groupId = objOrderItem.GroupId

            For Each objSubItem In Me
                If objSubItem.GroupId = groupId And _
                         objSubItem.OrderItemComboCount > 1 Then
                    objComboItem = objSubItem

                End If
            Next
        Else
            objComboItem = Nothing
        End If
        Return objComboItem
    End Function

    Public Sub markItemVoid(ByVal objOrderItem As OrderItem)
        Dim groupId As Integer
        Dim subItem As OrderItem
        If objOrderItem.OrderItemComboCount > 1 Then
            groupId = objOrderItem.GroupId
        End If
        For Each subItem In Me
            If subItem.GroupId = groupId Then
                subItem.Status = OrderItem.enumStatus.Voided
            End If
        Next
        objOrderItem.Status = OrderItem.enumStatus.Voided
        ' markOrderVoid()

    End Sub

    'Private Sub markOrderVoid()
    '    Dim intVoidItems As Integer
    '    Dim objOrderItem As OrderItem
    '    For Each objOrderItem In Me
    '        Select Case objOrderItem.Status
    '            Case OrderItem.enumStatus.Voided
    '                intVoidItems += 1
    '            Case Else
    '                'nope

    '        End Select
    '    Next

    '    If intVoidItems = Me.Count Then
    '        SaleDay.CreateSaleDay.CurrentOrder.OrderOverrideStatus = Order.enumOrderOverrideStatus.Voided
    '        SaleDay.CreateSaleDay.CurrentOrder.OrderStatus = Order.enumOrderStatus.Paid
    '    Else
    '        SaleDay.CreateSaleDay.CurrentOrder.OrderOverrideStatus = Order.enumOrderOverrideStatus.PartiallyVoided
    '    End If

    'End Sub




    Friend Function CheckOrderExisting(ByVal objMenuItem As MenuItem) As Boolean
        Dim objOrderItem As OrderItem
        Dim orderItemIndex As Integer
        Dim existing As Boolean
        If Me.Count > 0 Then
            For Each objOrderItem In Me
                If objOrderItem.MenuItem Is objMenuItem Then
                    updateQuantity(objOrderItem)
                    existing = True
                End If
            Next
        End If

        Return existing
    End Function
    Private Sub updateQuantity(ByVal objOrderItem As OrderItem) 'try making this byref
        objOrderItem.Quantity += 1
    End Sub




    Public ReadOnly Property Item(ByVal index As Integer) As OrderItem
        Get
            Return CType(MyBase.itemPOSBO(index), OrderItem)
        End Get

    End Property



    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myOrderItemsIds() As Integer

            Return myOrderItemsIds

        End Get
    End Property


    Public Sub Add(ByRef value As OrderItem)



        MyBase.addPOSBO(CType(value, POSBO))

    End Sub


    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function IndexOf(ByRef value As OrderItem) As Integer
        Return MyBase.List.IndexOf(value)
    End Function


    Public Function contains(ByVal value As OrderItem) As Boolean
        Return MyBase.containsPOSBO(CType(value, POSBO))
    End Function

    Friend Function SetData() As Integer
        Dim objOrderItem As OrderItem
        Dim intSQLReturnValue As Integer

        For Each objOrderItem In MyBase.List
            intSQLReturnValue = objOrderItem.SetData()
            If intSQLReturnValue < 0 Then Exit For
        Next

        Return intSQLReturnValue
    End Function

    Friend Sub SetStraightFireOrder(ByVal OrderItemIndexTo As Integer)
        Dim i As Integer
        For i = OrderItemIndexTo To 0 Step -1
            Me.Item(i).SetCourseToStraightFire()
        Next
    End Sub

    Public Function KOTPrintedInFull() As Boolean
        Dim objOrderitem As OrderItem

        For Each objOrderitem In Me
            If (objOrderitem.KOTPrintedAt <= DateTime.MinValue) AndAlso _
                (objOrderitem.Status = OrderItem.enumStatus.Active) Then
                Return False
                Exit Function
            End If
        Next
        Return True
    End Function

    Friend Sub UpdateKOTPrintedAt(ByVal KOTAt As DateTime)         'use this for 1st or duplicate KOTs
        Dim objOrderItem As OrderItem

        For Each objOrderItem In Me
            If (objOrderItem.Status = OrderItem.enumStatus.Active) Then
                objOrderItem.SetKOTPrintedAt(KOTAt)
            End If
        Next
    End Sub

    Friend Sub UpdateGuestCheckPrintedAt(ByVal GuestCheckAt As DateTime)         'use this for 1st or duplicate KOTs
        Dim objOrderItem As OrderItem

        For Each objOrderItem In Me
            If (objOrderItem.Status = OrderItem.enumStatus.Active) Then
                objOrderItem.SetCheckPrintedAt(GuestCheckAt)
            End If
        Next
    End Sub

    Friend Function GetOrderItemsSortedList() As ArrayList
        Dim objOrderItem As OrderItem
        Dim cloneObjectOrderItems As ArrayList

        cloneObjectOrderItems = CType(Me.InnerList.Clone(), ArrayList)
        cloneObjectOrderItems.Sort()
        Return cloneObjectOrderItems
    End Function
End Class



