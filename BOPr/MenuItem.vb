Option Strict On
Imports System.Data.SqlClient


Public Class MenuItem
    Inherits POSBO

    Public Enum enumMenuItemStatus

        Active
        Inactive
        void
    End Enum

    Public Enum enumUoM
        PerPound
        Count
        PerOz
    End Enum

    Private m_MenuItemId As Integer
    Private m_MenuId As Integer
    Private m_ProductId As Integer
    Private m_MenuItemChangedBy As Integer
    Private m_MenuItemChangedAt As DateTime
    Private m_MenuItemStatus As enumMenuItemStatus
    Private m_IsDirty As Boolean
    Private m_ProductComboItemCount As Integer
    Private m_ProductNameHindi As String
    Private m_ProductName As String
    Private m_MenuItemPrice As Double
    Private m_MehekPrice As Double
    Private m_CrownPrice As Double
    Private m_MenuItemUOM As enumUoM
    Private m_ProductTaxExempt As Boolean
    Private m_ProductDiscountAllowed As Boolean
    Private m_ProductPricingType As Product.enumProductPricingType
    Private m_IsPersistedMenuItem As Boolean
    Private m_ProductCategoryId As Integer
    Private m_FamilyId As Integer
    Private m_FamilyMenuItems As MenuItems

    Public Sub New()
        'MenuItemId = 0
        'MenuItemStatus = enumMenuItemStatus.Active
        'Me.IsPersistedMenuItem = False
    End Sub

    Public Property MenuItemId() As Integer
        Get
            Return m_MenuItemId
        End Get

        Set(ByVal Value As Integer)
            m_MenuItemId = Value
        End Set
    End Property

    Public Property MenuId() As Integer
        Get
            Return m_MenuId
        End Get

        Set(ByVal Value As Integer)
            m_MenuId = Value
            MarkDirty()
        End Set
    End Property


    Public Property ProductId() As Integer
        Get
            Return m_ProductId
        End Get

        Set(ByVal Value As Integer)
            m_ProductId = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property ProductCategoryId() As Integer
        Get
            Return m_ProductCategoryId
        End Get
    End Property

    Public ReadOnly Property ProductCategoryName() As String
        Get
            With SaleDays.CreateSaleDay.ActiveProductCategories
                Return .Item(.indexOf(ProductCategoryId)).ProductCategoryName
            End With
        End Get
    End Property

    Public ReadOnly Property ProductCategoryUISortOrder() As Integer
        Get
            With SaleDays.CreateSaleDay.ActiveProductCategories
                Return .Item(.indexOf(ProductCategoryId)).UISortOrder
            End With
        End Get
    End Property

    Public ReadOnly Property ProductCategoryType() As ProductCategory.enumProductCategory
        Get
            With SaleDays.CreateSaleDay.ActiveProductCategories
                Return .Item(.indexOf(ProductCategoryId)).ProductCategoryType
            End With
        End Get
    End Property

    Public ReadOnly Property ProductComboItemCount() As Integer
        Get
            Return m_ProductComboItemCount
        End Get
    End Property

    Public Property ProductNameHindi() As String
        Get
            Return m_ProductNameHindi
        End Get
        Set(ByVal Value As String)
            m_ProductNameHindi = Value
        End Set
    End Property

    Public Property ProductName() As String
        Get
            Return m_ProductName
        End Get
        Set(ByVal Value As String)
            m_ProductName = Value
        End Set
    End Property

    Public Property MenuItemPrice() As Double
        Get
            Return m_MenuItemPrice
        End Get

        Set(ByVal Value As Double)
            m_MenuItemPrice = Value
        End Set
    End Property

    Public Property CrownPrice() As Double
        Get
            Return m_CrownPrice
        End Get

        Set(ByVal Value As Double)
            m_CrownPrice = Value
        End Set
    End Property
    Public Property MehekPrice() As Double
        Get
            Return m_MehekPrice
        End Get

        Set(ByVal Value As Double)
            m_MehekPrice = Value
        End Set
    End Property
    Public Property MenuItemUOM() As enumUoM
        Get
            Return m_MenuItemUOM
        End Get

        Set(ByVal Value As enumUoM)
            m_MenuItemUOM = Value
        End Set
    End Property

    Public ReadOnly Property ProductTaxExempt() As Boolean
        Get
            Return m_ProductTaxExempt
        End Get
    End Property

    Public ReadOnly Property ProductDiscountAllowed() As Boolean
        Get
            Return m_ProductDiscountAllowed
        End Get
    End Property

    Public ReadOnly Property ProductPricingType() As Product.enumProductPricingType
        Get
            Return m_ProductPricingType
        End Get
    End Property

    Friend Property IsPersistedMenuItem() As Boolean
        Get
            Return m_IsPersistedMenuItem
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedMenuItem = Value
        End Set
    End Property

    Public ReadOnly Property MenuItemChangedBy() As Integer
        Get
            Return m_MenuItemChangedBy
        End Get
    End Property

    Private Sub SetMenuItemChangedBy()
        'If SaleDays.CreateSaleDay.CurrentSaleDay.CurrentCashier Is Nothing Then
        m_MenuItemChangedBy = 0
        'Else
        '    m_MenuItemChangedBy = SaleDays.CreateSaleDay.CurrentSaleDay.CurrentCashier.EmployeeId
        'End If
    End Sub

    Public ReadOnly Property MenuItemChangedAt() As DateTime
        Get
            Return m_MenuItemChangedAt
        End Get
    End Property

    Private Sub SetMenuItemChangedAt()
        m_MenuItemChangedAt = SaleDays.CreateSaleDay.Now
    End Sub

    Public Property MenuItemStatus() As enumMenuItemStatus
        Get
            Return m_MenuItemStatus
        End Get
        Set(ByVal Value As enumMenuItemStatus)
            m_MenuItemStatus = Value
            MarkDirty()
        End Set

    End Property

    Private Sub MarkDirty()
        If IsDirty Then
            'NOP
        Else
            SetMenuItemChangedAt()
            SetMenuItemChangedBy()
            m_IsDirty = True
        End If

    End Sub

    Public ReadOnly Property IsDirty() As Boolean

        Get
            Return m_IsDirty
        End Get

    End Property

    Friend Sub setDirty()
        m_IsDirty = False
    End Sub

    Friend Function SetData(ByVal menuid As Integer) As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String

        If Me.IsDirty Then
            Select Case Me.IsPersistedMenuItem      'True means update existing row, False means insert row in SQL table 
                Case False
                    'How to get value for MenuId and ProductId
                    strSqlCmd = "insert into MenuItem values (" & _
                                menuid.ToString & "," & _
                                Me.ProductId.ToString & "," & _
                                Me.MenuItemChangedBy.ToString & ",'" & _
                                Me.MenuItemChangedAt & "'," & _
                                CStr(Me.MenuItemStatus) & "," & _
                                Me.MenuItemPrice.ToString & "," & _
                                CStr(Me.MenuItemUOM) & "," & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.MenuItemId() = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update MenuItem set  " & _
                                "ProductId = " & _
                                 Me.ProductId.ToString & "," & _
                                "MenuItemPrice = " & _
                                 Me.MenuItemPrice.ToString & "," & _
                                 "MenuItemChangedBy = " & _
                                Me.MenuItemChangedBy.ToString & "," & _
                                "MenuItemUOM = " & _
                                 CStr(Me.MenuItemUOM) & "," & _
                                "MenuItemChangedAt = '" & _
                                    Me.MenuItemChangedAt & "'," & _
                                "MenuItemStatus = " & _
                                    CStr(Me.MenuItemStatus) & _
                                " where MenuItemId =" & Me.MenuItemId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
            If intSQLReturnValue < 0 Then
                'NOP
            Else
                m_IsDirty = False
                IsPersistedMenuItem = True
            End If
        End If
        Return intSQLReturnValue
    End Function

    Friend Sub loadCollection(ByVal SqlDr As SqlDataReader)
        Dim SqlDrColName As String
        Dim i As Integer

        IsPersistedMenuItem = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName

                Case "MenuItemId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MenuItemId = SqlDr.GetInt32(i)
                    End If

                Case "MenuId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MenuId = SqlDr.GetInt32(i)
                    End If

                Case "ProductId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ProductId = SqlDr.GetInt32(i)
                    End If

                Case "ProductCategoryId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ProductCategoryId = SqlDr.GetInt32(i)
                    End If

                Case "ProductComboItemCount"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ProductComboItemCount = SqlDr.GetInt32(i)
                    End If

                Case "ProductName"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ProductName = SqlDr.GetString(i)
                    End If

                Case "ProductNameHindi"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ProductNameHindi = SqlDr.GetString(i)
                    End If
                Case "ProductPricingType"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_ProductPricingType = CType(SqlDr.GetInt32(i), Product.enumProductPricingType)
                        End If
                    Catch ex As Exception
                        m_ProductPricingType = Product.enumProductPricingType.SystemPricedSingleItem
                    End Try
                Case "MenuItemUoM"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_MenuItemUOM = CType(SqlDr.GetInt32(i), enumUoM)
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try


                Case "ProductTaxExempt"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_ProductTaxExempt = SqlDr.GetBoolean(i)
                        Else
                            m_ProductTaxExempt = False
                        End If
                    Catch ex As Exception
                        m_ProductTaxExempt = False
                    End Try

                Case "MenuItemPrice"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MenuItemPrice = SqlDr.GetDecimal(i)
                    End If

                Case "MehekPrice"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MehekPrice = SqlDr.GetDecimal(i)
                    End If

                Case "CrownPrice"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CrownPrice = SqlDr.GetDecimal(i)
                    End If
                Case "MenuItemChangedBy" 'Cashier who changed(voided) the MenuItem
                    If Not SqlDr.IsDBNull(i) Then
                        m_MenuItemChangedBy = SqlDr.GetInt32(i)
                    Else
                        m_MenuItemChangedBy = 0
                    End If

                Case "MenuItemChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MenuItemChangedAt = SqlDr.GetDateTime(i)
                    Else
                        m_MenuItemChangedAt = Date.MinValue
                    End If

                Case "MenuItemStatus"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_MenuItemStatus = CType(SqlDr.GetInt32(i), MenuItem.enumMenuItemStatus)
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "ProductFamilyId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_FamilyId = SqlDr.GetInt32(i)
                    Else
                        m_FamilyId = 0
                    End If
                Case "ProductDiscountAllowed"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_ProductDiscountAllowed = SqlDr.GetBoolean(i)
                        Else
                            m_ProductDiscountAllowed = False
                        End If
                    Catch ex As Exception
                        m_ProductDiscountAllowed = False
                    End Try
            End Select
        Next
    End Sub
    Public Property FamilyId() As Integer
        Get
            Return m_FamilyId
        End Get
        Set(ByVal Value As Integer)
            m_FamilyId = Value
        End Set
    End Property
    Public ReadOnly Property FamilyMenuItems() As MenuItems
        Get
            Dim objMenuItems As MenuItems
            Dim objMenuItem As MenuItem
            'Dim i As Integer

            Select Case Me.FamilyId
                Case 0
                    If m_FamilyMenuItems Is Nothing Then
                        m_FamilyMenuItems = New MenuItems()
                        objMenuItems = SaleDays.CreateSaleDay.ActiveProductCategories.Item(Me.ProductCategoryName).ActiveMenuItems
                        For Each objMenuItem In objMenuItems
                            If objMenuItem.FamilyId = Me.ProductId Then
                                m_FamilyMenuItems.Add(objMenuItem)
                                'i += 1
                            End If
                        Next
                    End If
                Case Else
                    'nop
            End Select
            Return m_FamilyMenuItems
        End Get
    End Property

End Class




