Option Strict On
Imports System.Data.SqlClient
Public Class Products
    Inherits POSBOs

    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView

    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive

    End Enum

    Public Sub New(ByVal enumProductFilter As EnumFilter, _
                    ByVal intProductCategoryId As Integer, ByVal enumProductView As EnumView)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader

        Dim objDataAccess As DataAccess
        Dim msg As String

        Select Case enumProductView
            Case EnumView.BaseView
                sqlcommand = "Select ProductId,ProductName From Product"

            Case EnumView.CompleteView
                sqlcommand = "Select * from Product"
        End Select



        Select Case enumProductFilter

            Case EnumFilter.All
            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  Productstatus =  " & Product.enumProductStatus.InActive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where Productstatus = " & Product.enumProductStatus.Active


        End Select
        sqlcommand = sqlcommand & " And ProductCategoryId=" & intProductCategoryId.ToString
        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            loadCollection(SqlDr)       'then empty Product object is returned

        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()

    End Sub


    'for every record returned by SqlDr an Product object created and added to the collection

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)


        Dim ProductId As Integer
        Dim ProductCategoryId As Integer
        Dim ProductGroupId As Integer
        Dim ProductPricingType As Product.enumProductPricingType
        Dim ProductComboItemCount As Integer
        Dim ProductName As String
        Dim ProductNameHindi As String
        Dim ProductPrice As Decimal
        Dim ProductUoM As MenuItem.enumUoM
        Dim ProductChangedBy As Integer
        Dim ProductChangedAt As DateTime
        Dim ProductTaxExempt As Boolean
        Dim ProductStatus As Product.enumProductStatus
        Dim ProductDiscountAllowed As Boolean
        Dim SqlDrColName As String
        Dim msg As String
        Dim i As Integer

        Do While SqlDr.Read()       'loops through the records in the SqlDr.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "ProductId"
                        If Not SqlDr.IsDBNull(i) Then
                            ProductId = SqlDr.GetInt32(i)
                        End If

                    Case "ProductCategoryId"
                        If Not SqlDr.IsDBNull(i) Then

                            ProductCategoryId = SqlDr.GetInt32(i)
                        End If

                    Case "ProductGroupId"
                        If Not SqlDr.IsDBNull(i) Then

                            ProductGroupId = SqlDr.GetInt32(i)
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

                    Case "ProductPrice"
                        If Not SqlDr.IsDBNull(i) Then
                            ProductPrice = SqlDr.GetDecimal(i)

                        End If

                    Case "ProductUoM"
                        Try
                            ProductUoM = CType(SqlDr.GetInt16(i), MenuItem.enumUoM)
                        Catch ex As Exception
                            '???errormessage???
                        End Try


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
                    Case "ProductDiscountAllowed"
                        If Not SqlDr.IsDBNull(i) Then
                            ProductDiscountAllowed = SqlDr.GetBoolean(i)
                        End If
                    Case "ProductPricingType"
                        Try
                            If Not SqlDr.IsDBNull(i) Then
                                ProductPricingType = CType(SqlDr.GetInt16(i), Product.enumProductPricingType)
                            Else
                                ProductPricingType = Product.enumProductPricingType.SystemPricedSingleItem
                            End If
                        Catch ex As Exception
                            '???errormessage???
                        End Try
                End Select


            Next

            'Tax Exempt Not added in New of Product


            Me.Add(New Product(ProductId, ProductCategoryId, ProductGroupId, ProductComboItemCount, _
                        ProductName, ProductNameHindi, ProductTaxExempt, ProductChangedBy, _
                        ProductChangedAt, ProductStatus, ProductDiscountAllowed, ProductPricingType))

        Loop
    End Sub



    Public ReadOnly Property Item(ByVal index As Integer) As Product
        Get
            Return CType(MyBase.itemPOSBO(index), Product)
        End Get

    End Property



    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myProductIds() As Integer

            Return myProductIds

        End Get
    End Property




    Public Sub Add(ByRef value As Product)



        MyBase.addPOSBO(CType(value, POSBO))

    End Sub



    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function IndexOf(ByRef value As Products) As Integer

    End Function

    Public Function Contains(ByVal value As Product) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))


    End Function


End Class
