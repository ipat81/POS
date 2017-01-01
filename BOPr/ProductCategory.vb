Option Strict On
Imports System.Data.SqlClient

Public Class ProductCategory
    Inherits POSBO

    Public Enum enumProductCategoryStatus
        Active
        InActive
        Terminated
    End Enum

    Public Enum enumProductCategory
        Appetizer = 1
        SoupsSalads = 2
        VeggieCurries = 3
        ChickenCurries = 4
        LambCurries = 5
        SeafoodCurries = 6
        KababTikka = 7
        VeggieTandoor = 8
        RiceBiryani = 9
        Breads = 10
        SideOrders = 11
        Desserts = 12
        HotDrinks = 13
        ColdDrinks = 14
        Misc = 15
        Lunch = 16
    End Enum

    Private m_ProductCategoryID As Integer
    Private m_ProductCategoryName As String
    Private m_ProductCategoryStatus As enumProductCategoryStatus
    Private m_ProductCategoryChangedBy As Integer
    Private m_ProductCategoryChangedAt As DateTime
    Private m_ProductCategorySortOrder As Integer
    Private m_ProductCategoryCourse As String
    Private m_ActiveProducts As Products
    Private m_ActiveMenuItems As MenuItems
    Private m_ActiveFamilyHeadMenuItems As MenuItems

    Private m_IsDirty As Boolean

    Public Sub New()
        ProductCategoryID = 0
    End Sub


    Friend Sub New(ByVal newproductcategoryid As Integer, ByVal newproductcategoryname As String, _
                    ByVal newproductcategorystatus As enumProductCategoryStatus, _
                    ByVal newproductcategorychangedby As Integer, _
                    ByVal newproductcategorychangedat As DateTime, _
                    ByVal newProductCategoryUISortOrder As Integer, _
                    ByVal newProductCategoryCourse As String)

        ProductCategoryID = newproductcategoryid
        ProductCategoryName = newproductcategoryname
        ProductCategoryChangedBy = newproductcategorychangedby
        ProductCategoryChangedAt = newproductcategorychangedat
        UISortOrder = newProductCategoryUISortOrder
        m_ProductCategoryStatus = newproductcategorystatus
        m_ProductCategoryCourse = newProductCategoryCourse
    End Sub

    Public Property ProductCategoryID() As Integer

        Get
            Return m_ProductCategoryID
        End Get

        Set(ByVal Value As Integer)
            m_ProductCategoryID = Value
        End Set
    End Property
    Public ReadOnly Property ProductCategoryType() As ProductCategory.enumProductCategory
        Get
            Return CType(ProductCategoryID, ProductCategory.enumProductCategory)
        End Get
    End Property
    Public Property ProductCategoryCourse() As String
        Get
            Return m_ProductCategoryCourse
        End Get
        Set(ByVal Value As String)
            m_ProductCategoryCourse = Value
        End Set
    End Property

    Public Property UISortOrder() As Integer
        Get
            Return m_ProductCategorySortOrder
        End Get
        Set(ByVal Value As Integer)
            m_ProductCategorySortOrder = Value
        End Set
    End Property

    Public Property ProductCategoryName() As String

        Get
            Return m_ProductCategoryName
        End Get

        Set(ByVal Value As String)
            m_ProductCategoryName = Value
        End Set

    End Property


    Public ReadOnly Property ProductCategoryStatus() As enumProductCategoryStatus

        Get
            Return m_ProductCategoryStatus
        End Get

    End Property


    Public Property ProductCategoryChangedBy() As Integer

        Get
            Return m_ProductCategoryChangedBy
        End Get

        Set(ByVal Value As Integer)
            m_ProductCategoryChangedBy = Value
        End Set
    End Property

    Public Property ProductCategoryChangedAt() As DateTime

        Get
            Return m_ProductCategoryChangedAt
        End Get

        Set(ByVal Value As DateTime)
            m_ProductCategoryChangedAt = Value
        End Set


    End Property


    ReadOnly Property IsDirty() As Boolean

        Get
            Return m_IsDirty = False
        End Get

    End Property

    Public Sub GetData(ByVal enumProductCategoryView As ProductCategories.EnumView)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim SqlDrColName As String

        Dim objDataAccess As DataAccess
        Dim msg As String
        Dim i As Integer

        Select Case enumProductCategoryView
            Case ProductCategories.EnumView.BaseView
                sqlcommand = "Select ProductCategoryId,ProductCategoryName From ProductCategory"

            Case ProductCategories.EnumView.CompleteView
                sqlcommand = "Select * from ProductCategory"
        End Select

        sqlcommand = sqlcommand & "where ProductCategoryId = " & Me.ProductCategoryID

        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)


        If Not SqlDr Is Nothing Then
            Do While SqlDr.Read()       'loops through the records in the SqlDr.


                For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                    SqlDrColName = SqlDr.GetName(i)

                    Select Case SqlDrColName

                        Case "ProductCategoryId"

                            If Not SqlDr.IsDBNull(i) Then

                                ProductCategoryID = SqlDr.GetInt32(i)
                            End If

                        Case "ProductCategoryName"

                            If Not SqlDr.IsDBNull(i) Then
                                ProductCategoryName = SqlDr.GetString(i)
                            End If



                        Case "ProductCategoryChangedBy"
                            If Not SqlDr.IsDBNull(i) Then
                                ProductCategoryChangedBy = SqlDr.GetInt32(i)
                            Else
                                ProductCategoryChangedBy = 0
                            End If

                        Case "ProductCategoryChangedAt"
                            If Not SqlDr.IsDBNull(i) Then
                                ProductCategoryChangedAt = SqlDr.GetDateTime(i)
                            Else
                                ProductCategoryChangedAt = Date.MinValue
                            End If

                    End Select

                Next
            Loop
        End If
    End Sub

    Public ReadOnly Property ActiveProducts() As Products
        Get
            If m_ActiveProducts Is Nothing Then
                m_ActiveProducts = New Products(Products.EnumFilter.Active, _
                                Me.ProductCategoryID, Products.EnumView.CompleteView)

            End If
            Return m_ActiveProducts
        End Get
    End Property

    Public ReadOnly Property ActiveFamilyHeadMenuItems() As MenuItems
        'This returns only Those Menuitems which are printed in the Menu 
        '(i.e. excludes variations of menuitems like Palak Tofu etc. 
        Get
            Dim objMenuItem As MenuItem
            Dim i As Integer

            If m_ActiveFamilyHeadMenuItems Is Nothing Then
                m_ActiveFamilyHeadMenuItems = New MenuItems()
                For Each objMenuItem In ActiveMenuItems
                    If (objMenuItem.FamilyId = 0) AndAlso (objMenuItem.MenuItemStatus = MenuItem.enumMenuItemStatus.Active) Then
                        m_ActiveFamilyHeadMenuItems.Add(objMenuItem)
                    End If
                Next
            End If

            Return m_ActiveFamilyHeadMenuItems
        End Get
    End Property

    Public ReadOnly Property ActiveMenuItems() As MenuItems
        'This returns ALL Active Menu Items i.e. Printed (family head + Variations (family menu items)
        Get
            If m_ActiveMenuItems Is Nothing Then
                m_ActiveMenuItems = New MenuItems(MenuItems.EnumFilter.All, _
                MenuItems.EnumView.JoinView, _
                Me.ProductCategoryID)
            End If

            Return m_ActiveMenuItems
        End Get
    End Property
End Class
