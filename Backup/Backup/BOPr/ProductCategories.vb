Option Strict On
Imports System.Data.SqlClient

Public Class ProductCategories
    Inherits POSBOs
    Private myProductCategory As ProductCategory

    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView
    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive
        Terminated

    End Enum


    Public Sub New()

    End Sub


    Public Sub New(ByVal enumProductCategoryFilter As EnumFilter, ByVal enumProductCategoryView As EnumView)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        Dim msg As String


        Select Case enumProductCategoryView
            Case EnumView.BaseView
                sqlcommand = "Select ProductCategoryId,ProductCategoryName From ProductCategory"

            Case EnumView.CompleteView
                sqlcommand = "Select * from ProductCategory"

        End Select


        Select Case enumProductCategoryFilter

            Case EnumFilter.All
            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  ProductCategoryStatus =  " & ProductCategory.enumProductCategoryStatus.InActive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where ProductCategoryStatus = " & ProductCategory.enumProductCategoryStatus.Active

            Case EnumFilter.Terminated
                sqlcommand = sqlcommand & "where ProductCategoryStatus = " & ProductCategory.enumProductCategoryStatus.Terminated

        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            loadCollection(SqlDr)       'then empty ProductCategory object is returned

        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()

    End Sub


    'for every record returned by SqlDr an ProductCategory object created and added to the collection

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)

        Dim ProductCategoryId As Integer
        Dim ProductCategoryName As String
        Dim ProductCategoryChangedBy As Integer
        Dim ProductCategoryChangedAt As DateTime
        Dim ProductCategoryStatus As ProductCategory.enumProductCategoryStatus
        Dim ProductCategoryUISortOrder As Integer
        Dim ProductCategoryCourse As String

        Dim SqlDrColName As String
        Dim msg As String
        Dim i As Integer




        Do While SqlDr.Read()       'loops through the records in the SqlDr.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "ProductCategoryId"
                        If Not SqlDr.IsDBNull(i) Then

                            ProductCategoryId = SqlDr.GetInt32(i)
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

                    Case "ProductCategoryUISortOrder"
                        If Not SqlDr.IsDBNull(i) Then

                            ProductCategoryUISortOrder = SqlDr.GetInt32(i)
                        End If
                    Case "ProductCategoryCourse"
                        If Not SqlDr.IsDBNull(i) Then

                            ProductCategoryCourse = SqlDr.GetString(i)
                        End If

                End Select


            Next

            Me.add(New ProductCategory(ProductCategoryId, ProductCategoryName, ProductCategoryStatus, _
                                      ProductCategoryChangedBy, ProductCategoryChangedAt, ProductCategoryUISortOrder, ProductCategoryCourse))



        Loop
    End Sub



    Public ReadOnly Property Item(ByVal index As Integer) As ProductCategory
        Get
            Return CType(MyBase.itemPOSBO(index), ProductCategory)
        End Get
    End Property

    Public ReadOnly Property Item(ByVal strProductCategoryName As String) As ProductCategory
        Get
            Return CType(MyBase.itemPOSBO(Me.indexOf(strProductCategoryName)), ProductCategory)
        End Get
    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myProductCategoryIds() As Integer

            Return myProductCategoryIds

        End Get
    End Property




    Public Sub add(ByRef value As ProductCategory)



        MyBase.addPOSBO(CType(value, POSBO))

    End Sub



    Public Sub remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function indexOf(ByRef newProductCategoryId As Integer) As Integer
        Dim objProductCategory As ProductCategory
        For Each objProductCategory In Me
            If objProductCategory.ProductCategoryID = newProductCategoryId Then
                Return MyBase.List.IndexOf(objProductCategory)
                Exit For
            End If
        Next
    End Function

    Public Function indexOf(ByRef strProductCategoryName As String) As Integer
        Dim objProductCategory As ProductCategory
        For Each objProductCategory In Me
            If objProductCategory.ProductCategoryName = strProductCategoryName Then
                Return MyBase.List.IndexOf(objProductCategory)
                Exit For
            End If
        Next
    End Function

    Public Function contains(ByVal value As ProductCategory) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))


    End Function


End Class
