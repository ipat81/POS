Option Strict On
Imports System.Data.SqlClient


Public Class OrderItem
    Inherits POSBO
    Implements IComparable
    Public Enum enumStatus
        Active
        InActive
        Voided
    End Enum

    Public Enum enumUoM

        PerPound
        Count
        PerOz
    End Enum

    Friend Enum enumMenuState
        Valid
        Invalid
        Existing
    End Enum

    Public Enum enumLineType
        SingleItem
        GroupStart
        PartOfGroup
        GroupEnd
    End Enum


    Private m_OrderItemId As Integer
    'Private m_OrderId As Integer
    Private m_GroupId As Integer
    'Private m_LineType As enumLineType

    Private m_Quantity As Double
    Private m_Course As String
    Private m_OrderItemNameHindi As String
    Private m_Price As Double
    Private m_OrderItemAmount As Double
    Private m_UoM As enumUoM
    Private m_ItemMod As String
    Private m_ItemModNote As String
    Private m_OrderItemChangedBy As Integer
    Private m_ChangedAt As DateTime
    Private m_Status As enumStatus
    Private m_IsPersistedOrderItem As Boolean
    'new properties added
    Private m_EnteredAt As DateTime
    Private m_OrderItemKOTAt As DateTime
    Private m_CheckPrintedAt As DateTime
    Private m_ServedAt As DateTime

    Private m_ComboGroupCount As Integer
    Private m_OrderItemComboCount As Integer
    'For split checks, which check an orderitem is printed on
    Private m_CheckNumber As Integer

    Private m_objMenuItem As MenuItem
    Private m_ParentOrder As Order
    Private m_IsDirty As Boolean

    Public Sub New()

    End Sub

    Friend Sub LoadData(ByVal SqlDr As SqlDataReader)
        Dim SqlDrColName As String
        Dim msg As String
        Dim i As Integer

        IsPersistedOrderItem = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record
            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName
                Case "OrderItemId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_OrderItemId = SqlDr.GetInt32(i)
                    End If
                    'Case "OrderId"
                    '    If Not SqlDr.IsDBNull(i) Then
                    '        m_OrderId = SqlDr.GetInt32(i)
                    '    End If
                Case "MenuItemId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_objMenuItem = SaleDays.CreateSaleDay.MenuItemFromId(SqlDr.GetInt32(i))
                    End If
                Case "GroupId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_GroupId = SqlDr.GetInt32(i)
                    End If
                Case "Course"
                    If Not SqlDr.IsDBNull(i) Then
                        m_Course = SqlDr.GetString(i)
                    End If
                Case "Quantity"
                    If Not SqlDr.IsDBNull(i) Then
                        m_Quantity = SqlDr.GetDecimal(i)
                    End If
                Case "Price"
                    If Not SqlDr.IsDBNull(i) Then
                        m_Price = SqlDr.GetDecimal(i)
                    End If
                Case "UoM"
                    Try
                        m_UoM = CType(SqlDr.GetInt32(i), OrderItem.enumUoM)
                    Catch ex As Exception
                        m_UoM = OrderItem.enumUoM.Count
                    End Try
                Case "ItemMod"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ItemMod = SqlDr.GetString(i)
                    End If
                Case "ItemModNote"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ItemModNote = SqlDr.GetString(i)
                    End If
                Case "EnteredAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_EnteredAt = SqlDr.GetDateTime(i)
                    Else
                        m_EnteredAt = Date.MinValue
                    End If
                Case "KOTPrintedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_OrderItemKOTAt = SqlDr.GetDateTime(i)
                    Else
                        m_OrderItemKOTAt = Date.MinValue
                    End If
                Case "ServedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ServedAt = SqlDr.GetDateTime(i)
                    Else
                        m_ServedAt = Date.MinValue
                    End If
                Case "CheckPrintedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CheckPrintedAt = SqlDr.GetDateTime(i)
                    Else
                        m_CheckPrintedAt = Date.MinValue
                    End If
                Case "ChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ChangedAt = SqlDr.GetDateTime(i)
                    Else
                        m_ChangedAt = Date.MinValue
                    End If
                Case "Status"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_Status = CType(SqlDr.GetInt32(i), OrderItem.enumStatus)
                        End If
                    Catch ex As Exception
                        m_Status = enumStatus.Active
                    End Try
                    'Case "LineType"
                    '    Try
                    '        If Not SqlDr.IsDBNull(i) Then
                    '            m_LineType = CType(SqlDr.GetInt32(i), OrderItem.enumLineType)
                    '        End If
                    '    Catch ex As Exception
                    '        m_LineType = Me.enumLineType.SingleItem
                    '    End Try
                Case "CheckNumber"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CheckNumber = SqlDr.GetInt32(i)
                    Else
                        m_CheckNumber = 1
                    End If
            End Select
        Next
    End Sub

    'Friend Sub New(ByVal neworderitemid As Integer, ByVal neworderid As Integer, _
    '                ByVal newGroupId As Integer, ByVal newQuantity As Decimal, _
    '                ByVal newPrice As Decimal, ByVal neworderitemamount As Decimal, _
    '                ByVal newUoM As enumUoM, _
    '                ByVal neworderitemchangedby As Integer, _
    '                ByVal newChangedAt As DateTime, ByVal newStatus As enumStatus, _
    '                ByVal newIsPersistedOrderItem As Boolean)
    '    OrderItemId = neworderitemid
    '    OrderId = neworderid
    '    GroupId = newGroupId
    '    Quantity = newQuantity
    '    Price = newPrice
    '    'OrderItemAmount = neworderitemamount
    '    UoM = newUoM
    '    'OrderItemChangedBy = neworderitemchangedby
    '    'ChangedAt = newChangedAt
    '    Status = newStatus
    '    IsPersistedOrderItem = newIsPersistedOrderItem
    'End Sub

    Friend Property ParentOrder() As Order
        Get
            Return m_ParentOrder
        End Get
        Set(ByVal Value As Order)
            m_ParentOrder = Value
        End Set
    End Property

    Public Property CheckNumber() As Integer
        Get
            Return m_CheckNumber
        End Get
        Set(ByVal Value As Integer)
            m_CheckNumber = Value
            If m_CheckNumber > 0 Then Me.ParentOrder.NumberOfSplitChecks = m_CheckNumber
        End Set
    End Property

    Public Property OrderItemId() As Integer

        Get
            Return m_OrderItemId
        End Get

        Set(ByVal Value As Integer)
            m_OrderItemId = Value
        End Set
    End Property

    Public ReadOnly Property OrderId() As Integer

        Get
            Return ParentOrder.OrderId
        End Get

        'Set(ByVal Value As Integer)
        '    m_OrderId = Value
        'End Set
    End Property

    Public Property GroupId() As Integer
        Get
            Return m_GroupId
        End Get

        Set(ByVal Value As Integer)
            m_GroupId = Value
        End Set
    End Property

    Public Property Quantity() As Double

        Get
            Select Case UoM
                Case enumUoM.Count, enumUoM.PerPound
                    Return m_Quantity
                Case enumUoM.PerOz
                    Return (m_Quantity * 16)
            End Select
        End Get

        Set(ByVal Value As Double)
            m_Quantity = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property OrderItemNameHindi() As String
        Get
            Return Me.MenuItem.ProductNameHindi
        End Get
    End Property

    Public ReadOnly Property OrderItemName() As String
        Get
            Return Me.MenuItem.ProductName
        End Get
    End Property

    Public Property Price() As Double
        Get
            Return m_Price
        End Get

        Set(ByVal Value As Double)
            m_Price = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property OrderItemComboCount() As Integer
        Get
            Return Me.MenuItem.ProductComboItemCount
        End Get

        'Set(ByVal Value As Integer)
        '    m_OrderItemComboCount = Value
        'End Set
    End Property

    Public ReadOnly Property OrderItemAmount() As Double

        Get
            Return Math.Round((Price * Quantity), 2)
        End Get

        'Set(ByVal Value As Decimal)
        '    m_OrderItemAmount = Value
        'End Set
    End Property

    Public Property UoM() As enumUoM
        Get
            Return m_UoM
        End Get
        Set(ByVal Value As enumUoM)
            m_UoM = Value
            MarkDirty()
        End Set
    End Property

    Public Property ItemMod() As String
        Get
            Return m_ItemMod
        End Get
        Set(ByVal Value As String)
            m_ItemMod = Value
            MarkDirty()
        End Set
    End Property

    Public Property Course() As String
        Get
            Return m_Course
        End Get
        Set(ByVal Value As String)
            m_Course = Value
            If ParentOrder.OrderType = Order.enumOrderType.EatIn Then
                If m_Course.StartsWith("Straight") Then
                    With ParentOrder.AllOrderItems
                        .SetStraightFireOrder(.IndexOf(Me))
                    End With
                End If
            End If
            MarkDirty()
        End Set
    End Property

    Friend Sub SetCourseToStraightFire()
        m_Course = "Straight Fire"
        MarkDirty()
    End Sub

    Public Property ItemModNote() As String
        Get
            Return m_ItemModNote
        End Get
        Set(ByVal Value As String)
            m_ItemModNote = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property OrderItemTax() As Double

        Get
            Dim STRate As Double
            Select Case Me.ParentOrder.PromisedAt < #7/15/2006#
                Case True
                    STRate = 0.06
                Case False
                    STRate = 0.07
            End Select

            If Me.MenuItem() Is Nothing Then Return OrderItemAmount * STRate
            Select Case Me.ParentOrder.SalesTaxStatus
                Case Order.enumOrderSalesTaxStatus.Taxable
                    Select Case Me.MenuItem.ProductTaxExempt
                        Case True
                            Return 0
                        Case False

                            Return OrderItemAmount * STRate
                    End Select
                Case Else
                    Return 0
            End Select
        End Get
    End Property

    Public ReadOnly Property OrderItemDiscountAmount() As Double
        Get
            If Me.MenuItem Is Nothing Then Return 0

            Select Case Me.MenuItem.ProductDiscountAllowed
                Case True
                    Return OrderItemAmount * (Me.ParentOrder.Discount / 100)
                Case False
                    Return 0
            End Select
        End Get
    End Property

    Public ReadOnly Property OrderItemChangedBy() As Integer
        Get
            Return m_OrderItemChangedBy
        End Get
    End Property

    Private Sub SetOrderItemChangedBy()
        'm_OrderItemChangedBy = SaleDay.CreateSaleDay.CurrentCashier.EmployeeId
        m_OrderItemChangedBy = 1
    End Sub

    Public ReadOnly Property KOTPrintedAt() As DateTime
        Get
            Return m_OrderItemKOTAt
        End Get
    End Property
    Friend Sub SetKOTPrintedAt(ByVal KOTAt As DateTime)
        m_OrderItemKOTAt = KOTAt
        MarkDirty()
    End Sub

    Public ReadOnly Property CheckPrintedAt() As DateTime
        Get
            Return m_CheckPrintedAt
        End Get
    End Property
    Friend Sub SetCheckPrintedAt(ByVal GuestCheckAt As DateTime)
        m_CheckPrintedAt = GuestCheckAt
        MarkDirty()
    End Sub

    Friend Sub SetEnteredAt(ByVal TimeNow As DateTime)
        m_EnteredAt = TimeNow
        MarkDirty()
    End Sub

    Public ReadOnly Property EnteredAt() As DateTime
        Get
            Return m_EnteredAt
        End Get
    End Property

    Private Sub SetChangedAt(ByVal TimeNow As DateTime)
        m_ChangedAt = TimeNow
    End Sub

    Public Property ServedAt() As DateTime
        Get
            Return m_ServedAt
        End Get

        Set(ByVal TimeNow As DateTime)
            m_ServedAt = TimeNow
        End Set
    End Property

    Public ReadOnly Property ChangedAt() As DateTime
        Get
            Return m_ChangedAt
        End Get
    End Property
    Public Property Status() As enumStatus
        Get
            Return m_Status
        End Get

        Set(ByVal Value As enumStatus)
            m_Status = Value
            MarkDirty()
        End Set

    End Property

    Public Property MenuItem() As MenuItem
        Get
            Return m_objMenuItem
        End Get
        Set(ByVal Value As MenuItem)
            m_objMenuItem = Value
            MarkDirty()
        End Set
    End Property

    Friend Function setMenuitem(ByVal objMenuItem As MenuItem) As enumMenuState
        Dim enumMenuValidate As enumMenuState
        If objMenuItem.ProductComboItemCount > 1 Then
            enumMenuValidate = Me.ParentOrder.AllOrderItems.validateCurrentCombo(objMenuItem)
        ElseIf objMenuItem.ProductComboItemCount = 1 Then
            enumMenuValidate = validateSubItem(objMenuItem)

        End If
        Return enumMenuValidate
    End Function

    Private Function validateSubItem(ByVal objMenuItem As MenuItem) As enumMenuState
        Dim existing As Boolean
        Dim enumMenuValidate As enumMenuState

        If Me.ParentOrder.AllOrderItems.CurrentCombo Is Nothing Then  'Single item selection
            If objMenuItem.MenuItemUOM = MenuItem.enumUoM.Count Then
                existing = Me.ParentOrder.AllOrderItems.CheckOrderExisting(objMenuItem)
                Select Case existing
                    Case True

                        Return enumMenuState.Existing
                    Case False
                        Me.ParentOrder.AllOrderItems.CurrentGroupId += 1  'new item by count
                End Select
            Else
                'SpecialItem
                Me.ParentOrder.AllOrderItems.CurrentGroupId += 1
            End If

        Else  'This is part of a combo
            Select Case objMenuItem.MenuItemUOM
                Case MenuItem.enumUoM.Count
                    Return enumMenuState.Invalid
                Case Else
                    Me.ParentOrder.AllOrderItems.CurrentComboItemsCount += 1
                    'objMenuItem.ProductNameHindi = objMenuItem.ProductNameHindi
                    Me.ParentOrder.AllOrderItems.CompareComboPrice(objMenuItem)
            End Select
        End If

        Return enumMenuState.Valid
    End Function



    Public ReadOnly Property IsDirty() As Boolean

        Get
            Return m_IsDirty
        End Get

    End Property
    Private Sub MarkDirty()
        Dim TimeNow As DateTime
        If IsDirty Then
            'NOP
        Else
            TimeNow = SaleDays.CreateSaleDay.Now
            SetChangedAt(TimeNow)
            SetOrderItemChangedBy()
            m_IsDirty = True
            Me.ParentOrder.MarkDirty()
        End If

    End Sub
    Friend Property IsPersistedOrderItem() As Boolean
        Get
            Return m_IsPersistedOrderItem
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedOrderItem = Value
        End Set
    End Property
    Friend Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strKOTAt As String
        Dim strServedAt As String
        Dim strCheckAt As String
        Dim strChangedAt As String

        Dim objPayment As Payment
        If Me.KOTPrintedAt = Date.MinValue Then
            strKOTAt = "NULL"
        Else
            strKOTAt = "'" & Me.KOTPrintedAt.ToString & "'"
        End If

        If Me.ServedAt = Date.MinValue Then
            strServedAt = "NULL"
        Else
            strServedAt = "'" & Me.ServedAt.ToString & "'"
        End If

        If Me.CheckPrintedAt = Date.MinValue Then
            strCheckAt = "NULL"
        Else
            strCheckAt = "'" & Me.CheckPrintedAt.ToString & "'"
        End If

        If Me.ChangedAt = Date.MinValue Then
            strChangedAt = "NULL"
        Else
            strChangedAt = "'" & Me.ChangedAt.ToString & "'"
        End If
        If Me.IsDirty Then
            Select Case Me.IsPersistedOrderItem       'True means update existing row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into OrderItem values (" & _
                                Me.ParentOrder.OrderId.ToString & "," & _
                                Me.MenuItem.MenuItemId.ToString & "," & _
                                Me.GroupId.ToString & ",'" & _
                                Me.Course & "'," & _
                                Me.Quantity.ToString & "," & _
                                Me.Price.ToString & "," & _
                                CStr(Me.UoM) & ",' " & _
                                Me.ItemMod & "',' " & _
                                Me.ItemModNote & "','" & _
                                Me.EnteredAt.ToString & "'," & _
                                strKOTAt & "," & _
                                strServedAt & "," & _
                                strCheckAt & "," & _
                                strChangedAt & "," & _
                                CStr(Me.Status) & "," & _
                                Me.CheckNumber.ToString & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.OrderItemId() = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update OrderItem set  " & _
                                    "Quantity=" & _
                                        Me.Quantity.ToString & "," & _
                                    "Price=" & _
                                        Me.Price.ToString & "," & _
                                    "ItemMod='" & _
                                        Me.ItemMod & "'," & _
                                    "ItemModNote='" & _
                                        Me.ItemModNote & "'," & _
                                    "KOTPrintedAt=" & _
                                        strKOTAt & "," & _
                                    "ServedAt=" & _
                                        strServedAt & "," & _
                                    "CheckPrintedAt=" & _
                                        strCheckAt & "," & _
                                    "ChangedAt=" & _
                                        strChangedAt & "," & _
                                    "Status=" & _
                                        CStr(Me.Status) & "," & _
                                     "CheckNumber=" & _
                                        Me.CheckNumber.ToString & _
                                   " where OrderItemId =" & Me.OrderItemId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
            If intSQLReturnValue < 0 Then
                'NOP
            Else
                m_IsDirty = False
                IsPersistedOrderItem = True
            End If
        End If
        Return intSQLReturnValue
    End Function

    Public Overrides Function Tostring() As String
        'Return Me.Course & Me.MenuItem.ProductCategoryName & Me.MenuItem.ProductNameHindi
        Return Me.Course & _
                Me.MenuItem.ProductCategoryUISortOrder.ToString & _
                Me.MenuItem.ProductNameHindi
    End Function

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If CType(obj, OrderItem).ToString > Me.ToString Then
            Return -1
        ElseIf CType(obj, OrderItem).ToString < Me.ToString Then
            Return 1
        Else
            Return 0
        End If
    End Function
End Class

