
Option Strict On
Imports System.Data.SqlClient


Public Class MenuItems
    Inherits POSBOs

    Public Enum EnumView As Integer  'This enum determines the select clause
        BaseView
        CompleteView
        JoinView
    End Enum

    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive
        Appetizers
        Entree
        Combo
        SpecialItems

    End Enum

    Private m_CurrentMenuItem As MenuItem
    Private m_ParentMenu As Menu

    Public Sub New()
    End Sub

    Public Sub New(ByVal enumMenuItemsFilter As MenuItems.EnumFilter, _
                   ByVal enumMenuItemsView As MenuItems.EnumView, ByVal intProductCategoryId As Integer)


        MyBase.New()
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objMenuItem As MenuItem

        Select Case enumMenuItemsView
            Case EnumView.BaseView
                sqlcommand = "Select MenuItemId,MenuId,ProductId From MenuItem"

            Case EnumView.CompleteView
                sqlcommand = "Select * from MenuItem"

            Case EnumView.JoinView
                sqlcommand = "Select A.MenuItemId,B.ProductId,B.ProductCategoryId," & _
                "B.ProductComboItemCount,A.MenuItemPrice,A.CrownPrice,A.MehekPrice, A.MenuItemUoM, A.MenuItemStatus," & _
                "B.ProductTaxExempt,B.ProductDiscountAllowed," & _
                "B.ProductNameHindi,B.ProductName,B.ProductFamilyId,B.ProductPricingType" & _
               " from  menuItem A, Product B  Where A.ProductId = " & _
                "B.ProductId and B.ProductCategoryId =" & _
                intProductCategoryId.ToString & " and A.MenuId =1"
                '              &  SaleDay.CreateSaleDay.CurrentMenu.MenuId.ToString
        End Select

        Select Case enumMenuItemsFilter
            Case EnumFilter.All
                'sqlcommand = sqlcommand & " and B.ProductCategoryId Not IN (" & _
                '            CStr(ProductCategory.enumProductCategory.Combo) & ")" & _
                '            " And B.ProductComboItemCount = 1 And A.MenuId = " & "1"

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " and MenuItemStatus = " & MenuItem.enumMenuItemStatus.Active

            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " Where MenuItemStatus = " & MenuItem.enumMenuItemStatus.Inactive

            Case EnumFilter.SpecialItems
                sqlcommand = sqlcommand & " where B.ProductUoM = " & MenuItem.enumUoM.Count
        End Select

        sqlcommand = sqlcommand & _
         " order by  B.ProductCategoryId, B.ProductNameHindi"

        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objMenuItem = New MenuItem()
                Me.Add(objMenuItem)
                objMenuItem.loadCollection(SqlDr)
            Loop
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Public Sub New(ByVal enumMenuItemsFilter As EnumFilter, ByVal enumMenuItemsView As EnumView)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objMenuItem As MenuItem


        Select Case enumMenuItemsView
            Case EnumView.BaseView
                sqlcommand = "Select MenuItemId,MenuId,ProductId From MenuItem"

            Case EnumView.CompleteView
                sqlcommand = "Select * from MenuItem"

            Case EnumView.JoinView
                sqlcommand = "Select A.MenuItemId,B.ProductId," & _
                "B.ProductComboItemCount,B.ProductPrice,B.ProductUoM, B.ProductTaxExempt,B.ProductName,B.ProductNameHindi" & _
               " from  menuItem A, Product B,ProductCategory C Where A.ProductId = " & _
                "B.ProductId and B.ProductCategoryId = C.ProductCategoryId"

        End Select


        Select Case enumMenuItemsFilter


            Case EnumFilter.Appetizers
                'sqlcommand = sqlcommand & " and B.ProductCategoryId IN (" & _
                '                    CStr(ProductCategory.enumProductCategory.Beverages) & "," & _
                '                    CStr(ProductCategory.enumProductCategory.Salads) & "," & _
                '                    CStr(ProductCategory.enumProductCategory.Soups) & "," & _
                '                    CStr(ProductCategory.enumProductCategory.Accompaniments) & ")" & _
                '                      " And B.ProductComboItemCount =  1 And A.MenuId = -1 "
            Case EnumFilter.Entree
                'sqlcommand = sqlcommand & " and B.ProductCategoryId IN (" & _
                '            CStr(ProductCategory.enumProductCategory.Appetizer) & "," & _
                '            CStr(ProductCategory.enumProductCategory.Breads) & "," & _
                '            CStr(ProductCategory.enumProductCategory.Rice) & "," & _
                '            CStr(ProductCategory.enumProductCategory.Curry) & "," & _
                '            CStr(ProductCategory.enumProductCategory.TikkaKabab) & ")" & _
                '            CStr(ProductCategory.enumProductCategory.Desserts) & ")" & _
                '              " And B.ProductComboItemCount = 1"
            Case EnumFilter.Combo
                sqlcommand = sqlcommand & " and B.ProductComboItemCount > 1 And A.MenuId = -1"
            Case EnumFilter.All

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " Where MenuItemStatus = " & MenuItem.enumMenuItemStatus.Active

            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " Where MenuItemStatus = " & MenuItem.enumMenuItemStatus.Inactive

            Case EnumFilter.SpecialItems
                sqlcommand = sqlcommand & " where B.ProductUoM = " & MenuItem.enumUoM.Count
        End Select
        sqlcommand = sqlcommand & " order by B.ProductGroupId, B.ProductCategoryId, B.ProductNameHindi"
        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objMenuItem = New MenuItem()
                Me.Add(objMenuItem)
                objMenuItem.loadCollection(SqlDr)
            Loop
        End If
    End Sub

    Public Function IsModified() As Boolean
        Dim objMenuItem As MenuItem
        Dim modified As Boolean
        For Each objMenuItem In MyBase.List
            Select Case objMenuItem.IsDirty
                Case True
                    modified = True
                    Exit For
                Case False
                    modified = False
            End Select
        Next
        Return modified
    End Function

    Public Function AddMenuItem() As MenuItem

        m_CurrentMenuItem = New MenuItem()
        m_CurrentMenuItem.MenuId = Me.ParentMenu.MenuId
        Me.Add(m_CurrentMenuItem)

        Return m_CurrentMenuItem
    End Function

    Public ReadOnly Property CurrenTMenuItem() As MenuItem
        Get
            Return m_CurrentMenuItem
        End Get
    End Property

    Public ReadOnly Property Item(ByVal index As Integer) As MenuItem
        Get
            Return CType(MyBase.itemPOSBO(index), MenuItem)
        End Get

    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myMenuItemsIds() As Integer

            Return myMenuItemsIds

        End Get
    End Property

    Public Sub Add(ByRef value As MenuItem)
        MyBase.addPOSBO(CType(value, POSBO))
    End Sub

    Public Sub Remove(ByVal index As Integer)
        MyBase.removePOSBO(index)
    End Sub

    Public Function IndexOf(ByRef objMenuItem As MenuItem) As Integer
        Return MyBase.List.IndexOf(objMenuItem)
    End Function

    Public Function IndexOf(ByRef intMenuItemId As Integer) As Integer
        Dim objMenuItem As MenuItem
        Dim intIndex As Integer = -1

        For Each objMenuItem In Me
            If objMenuItem.MenuItemId = intMenuItemId Then
                intIndex = Me.IndexOf(objMenuItem)
                Exit For
            End If
        Next
        Return intIndex
    End Function

    Public Function contains(ByVal value As MenuItem) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))
    End Function

    Public Property ParentMenu() As Menu
        Get
            Return m_ParentMenu
        End Get
        Set(ByVal Value As Menu)
            m_ParentMenu = Value
        End Set
    End Property
    Friend Function SetData() As Integer
        Dim objMenuItem As MenuItem
        Dim intSQLReturnValue As Integer

        For Each objMenuItem In MyBase.List
            intSQLReturnValue = objMenuItem.SetData(ParentMenu.MenuId)
            If intSQLReturnValue < 0 Then Exit For
        Next

        Return intSQLReturnValue
    End Function


End Class



