Option Strict On
Imports System.Data.SqlClient


Public Class Product
    Inherits POSBO

    Public Enum enumProductStatus
        Active
        InActive
        Terminated
    End Enum

    Public Enum enumProductTaxExempt
        Taxable
        NoTax
    End Enum

    Public Enum enumProductPricingType
        SystemPricedSingleItem
        UserPricedSingleItem
        SystemPricedPackageItem
        UserPricedPackageItem
        SystemPricedComboItem
        UserPricedComboItem
        EndOfPackage
    End Enum

    Private m_ProductID As Integer
    Private m_ProductCategoryID As Integer
    Private m_ProdutGroupId As Integer
    Private m_ProductPricingType As enumProductPricingType
    Private m_ProductComboItemCount As Integer
    Private m_ProductName As String
    Private m_ProductNameHindi As String
    Private m_ProductChangedBy As Integer
    Private m_ProductChangedAt As DateTime
    Private m_ProductTaxExempt As Boolean
    Private m_ProductDiscountAllowed As Boolean
    Private m_ProductStatus As enumProductStatus
    Private m_isDirty As Boolean


    Public Sub New()
        ProductID = 0
    End Sub

    Public Sub New(ByVal newproductid As Integer)
        ProductID = newproductid
    End Sub


    Friend Sub New(ByVal newproductid As Integer, ByVal newproductcategoryid As Integer, _
                    ByVal newproductgroupid As Integer, ByVal newproductcomboitemcount As Integer, _
                    ByVal newproductname As String, ByVal newproductnamehindi As String, _
                    ByVal newproducttaxexempt As Boolean, ByVal newproductchangedby As Integer, _
                    ByVal newproductchangedat As DateTime, _
                    ByVal newproductstatus As enumProductStatus, _
                    ByVal newProductDiscountAllowed As Boolean, _
                    ByVal newProductPricingType As enumProductPricingType)

        ProductID = newproductid
        ProductCategoryID = newproductcategoryid
        ProductGroupID = newproductgroupid
        ProductComboItemCount = newproductcomboitemcount
        ProductName = newproductname
        ProductNameHindi = newproductnamehindi
        'ProductPrice = newproductprice
        'ProductUoM = newproductuom
        ProductChangedBy = newproductchangedby
        ProductChangedAt = newproductchangedat
        ProductStatus = newproductstatus
        ProductDiscountAllowed = newProductDiscountAllowed
        ProductPricingType = newProductPricingType
    End Sub

    Public Property ProductID() As Integer

        Get
            Return m_ProductID
        End Get

        Set(ByVal Value As Integer)
            m_ProductID = Value
        End Set
    End Property

    Public Property ProductCategoryID() As Integer

        Get
            Return m_ProductCategoryID
        End Get

        Set(ByVal Value As Integer)
            m_ProductCategoryID = Value
        End Set

    End Property

    Public Property ProductGroupID() As Integer

        Get
            Return m_ProductCategoryID
        End Get

        Set(ByVal Value As Integer)
            m_ProductCategoryID = Value
        End Set

    End Property

    Public Property ProductComboItemCount() As Integer
        Get
            Return m_ProductComboItemCount
        End Get

        Set(ByVal Value As Integer)
            m_ProductComboItemCount = Value
        End Set
    End Property

    Public Property ProductPricingType() As enumProductPricingType
        Get
            Return m_ProductPricingType
        End Get

        Set(ByVal Value As enumProductPricingType)
            m_ProductPricingType = Value
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


    Public Property ProductNameHindi() As String

        Get
            Return m_ProductNameHindi
        End Get

        Set(ByVal Value As String)
            m_ProductNameHindi = Value
        End Set
    End Property

    Public Property ProductChangedBy() As Integer

        Get
            Return m_ProductChangedBy
        End Get

        Set(ByVal Value As Integer)
            m_ProductChangedBy = Value
        End Set
    End Property

    Public Property ProductChangedAt() As DateTime

        Get
            Return m_ProductChangedAt
        End Get

        Set(ByVal Value As DateTime)
            m_ProductChangedAt = Value
        End Set
    End Property

    Public Property ProductTaxExempt() As Boolean
        Get
            Return m_ProductTaxExempt
        End Get
        Set(ByVal Value As Boolean)
            m_ProductTaxExempt = Value
        End Set
    End Property

    Public Property ProductDiscountAllowed() As Boolean
        Get
            Return m_ProductDiscountAllowed
        End Get
        Set(ByVal Value As Boolean)
            m_ProductDiscountAllowed = Value
        End Set
    End Property

    Public Property ProductStatus() As enumProductStatus
        Get
            Return m_ProductStatus
        End Get
        Set(ByVal Value As enumProductStatus)
            m_ProductStatus = Value
        End Set
    End Property

    ReadOnly Property IsDirty() As Boolean

        Get
            Return m_isDirty = False
        End Get

    End Property

    Public Sub GetData(ByVal enumProductView As Products.EnumView)

        Dim SqlDrColName As String
        Dim SqlDr As SqlDataReader
        Dim sqlcommand As String

        Dim objDataAccess As DataAccess
        Dim msg As String
        Dim i As Integer


        Select Case enumProductView
            Case Products.EnumView.BaseView
                sqlcommand = "Select ProductId,ProductName From Product"

            Case Products.EnumView.CompleteView
                sqlcommand = "Select * from Product"

        End Select

        sqlcommand = sqlcommand & " where  EmployeeId =  " & Me.ProductID

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

                        Case "ProductGroupId"
                            If Not SqlDr.IsDBNull(i) Then

                                ProductGroupID = SqlDr.GetInt32(i)
                            End If

                        Case "ProductComboItemCount"
                            If Not SqlDr.IsDBNull(i) Then

                                ProductComboItemCount = SqlDr.GetInt32(i)
                            End If

                        Case "ProductName"

                            If Not SqlDr.IsDBNull(i) Then
                                ProductName = SqlDr.GetString(i)
                            End If

                        Case "ProductNameHindi"
                            If Not SqlDr.IsDBNull(i) Then
                                ProductNameHindi = SqlDr.GetString(i)
                            End If


                        Case "ProductChangedBy"
                            If Not SqlDr.IsDBNull(i) Then
                                ProductChangedBy = SqlDr.GetInt32(i)
                            Else
                                ProductChangedBy = 0
                            End If

                        Case "ProductChangedAt"
                            If Not SqlDr.IsDBNull(i) Then
                                ProductChangedAt = SqlDr.GetDateTime(i)
                            Else
                                ProductChangedAt = Date.MinValue
                            End If

                        Case "ProductTaxExempt"
                            If Not SqlDr.IsDBNull(i) Then
                                ProductTaxExempt = SqlDr.GetBoolean(i)
                            End If

                        Case "ProductStatus "
                            Try
                                If Not SqlDr.IsDBNull(i) Then
                                    ProductStatus = CType(SqlDr.GetInt16(i), Product.enumProductStatus)
                                Else
                                    ProductStatus = Product.enumProductStatus.Active
                                End If
                            Catch ex As Exception
                                '???errormessage???
                            End Try
                    End Select

                Next

            Loop
        End If
    End Sub


End Class

