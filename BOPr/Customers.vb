Option Strict On
Imports System.Data.SqlClient


Public Class Customers
    Inherits POSBOs



    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView
    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive
        Terminated
        Paid
        NotPaid
        Student
        NonStudent
        AccountHolder

    End Enum


    Public Sub New(ByVal enumCustomerfilter As EnumFilter, ByVal enumCustomerview As EnumView)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        Dim msg As String


        Select Case enumCustomerview

            Case EnumView.BaseView
                sqlcommand = "Select CustomerId,CustomerName,CustomerSTaxExempt From customer"

            Case EnumView.CompleteView
                sqlcommand = "Select * from customer"

        End Select


        Select Case enumCustomerfilter

            Case EnumFilter.All
            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  CustomerStatus =  " & Customer.enumCustomerStatus.InActive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where CustomerStatus = " & Customer.enumCustomerStatus.Active

            Case EnumFilter.Terminated
                sqlcommand = sqlcommand & " where CustomerStatus = " & Customer.enumCustomerStatus.Terminated

            Case EnumFilter.Paid
                sqlcommand = sqlcommand & " where CustomerStatus = " & Customer.enumCustomerStatus.Paid

            Case EnumFilter.NotPaid
                sqlcommand = sqlcommand & " where CustomerStatus = " & Customer.enumCustomerStatus.NotPaid

            Case EnumFilter.Student
                sqlcommand = sqlcommand & " where CustomerType = " & Customer.enumCustomerType.Student

            Case EnumFilter.NonStudent
                sqlcommand = sqlcommand & " where CustomerType = " & Customer.enumCustomerType.NonStudent

            Case EnumFilter.AccountHolder
                sqlcommand = sqlcommand & " where CustomerType IN (" & _
                                        CStr(Customer.enumCustomerType.CorpAccount) & "," & _
                                        CStr(Customer.enumCustomerType.PUAccount) & ")"

        End Select
        sqlcommand = sqlcommand & " Order By " & "CustomerType, CustomerName"
        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            loadCollection(SqlDr)       'then empty customer object is returned

        End If

        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)

        Dim CustomerId As Integer
        Dim CustomerType As Customer.enumCustomerType
        Dim CustomerName As String
        Dim CustomerLastName As String
        Dim CustomerAddress1 As String
        Dim CustomerAddress2 As String
        Dim CustomerAddress3 As String
        Dim CustomerPhoneNo As String
        Dim CustomerFaxNo As String
        Dim CustomerEmaiId As String
        Dim CustomerSTaxExempt As Boolean
        Dim CustomerChangedBy As Integer
        Dim CustomerChangedAt As DateTime
        Dim CustomerStatus As Customer.enumCustomerStatus

        Dim msg As String
        Dim i As Integer
        Dim SqlDrColName As String



        Do While SqlDr.Read()       'loops through the records in the SqlDr.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "CustomerId"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerId = SqlDr.GetInt32(i)
                        End If

                    Case "CustomerType"

                        If Not SqlDr.IsDBNull(i) Then
                            Try
                                If Not SqlDr.IsDBNull(i) Then
                                    CustomerType = CType(SqlDr.GetInt32(i), Customer.enumCustomerType)
                                End If
                            Catch ex As Exception
                                '???errormessage???

                            End Try
                        End If

                    Case "CustomerName"
                        If Not SqlDr.IsDBNull(i) Then
                            CustomerName = SqlDr.GetString(i)
                        End If

                    Case "CustomerAddress1"
                        If Not SqlDr.IsDBNull(i) Then
                            CustomerAddress1 = SqlDr.GetString(i)
                        End If


                    Case "CustomerAddress2"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerAddress2 = SqlDr.GetString(i)
                        End If


                    Case "CustomerAddress3"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerAddress2 = SqlDr.GetString(i)
                        End If


                    Case "CustomerPhoneNo"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerPhoneNo = SqlDr.GetString(i)
                        End If

                    Case "CustomerFaxNo"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerFaxNo = SqlDr.GetString(i)
                        End If

                    Case "CustomerEmaiId"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerEmaiId = SqlDr.GetString(i)
                        End If

                    Case "CustomerSTaxExempt"

                        If Not SqlDr.IsDBNull(i) Then
                            CustomerSTaxExempt = SqlDr.GetBoolean(i)
                        End If


                    Case "CustomerChangedBy"
                        If Not SqlDr.IsDBNull(i) Then
                            CustomerChangedBy = SqlDr.GetInt32(i)
                        Else
                            CustomerChangedBy = 0
                        End If

                    Case "CustomerChangedAt"
                        If Not SqlDr.IsDBNull(i) Then
                            CustomerChangedAt = SqlDr.GetDateTime(i)
                        Else
                            CustomerChangedAt = Date.MinValue
                        End If

                    Case "CustomerStatus "
                        Try
                            If Not SqlDr.IsDBNull(i) Then
                                CustomerStatus = CType(SqlDr.GetInt32(i), Customer.enumCustomerStatus)
                            Else
                                CustomerStatus = Customer.enumCustomerStatus.Paid
                            End If
                        Catch ex As Exception
                            '???errormessage???
                        End Try

                End Select


            Next

            'customer records to the collection
            Me.Add(New Customer(CustomerId, CustomerType, CustomerName, CustomerAddress1, CustomerAddress2, CustomerAddress3, _
                                      CustomerPhoneNo, CustomerFaxNo, CustomerEmaiId, CustomerSTaxExempt, CustomerChangedBy, CustomerChangedAt, _
                                       CustomerStatus))


        Loop
    End Sub

    Public Function SetData() As Integer

        Dim objCustomer As Customer
        Dim intSQLReturnValue As Integer
        Dim i As Integer

        'DataAccess.CreateDataAccess.StartTxn()
        'For Each objCustomer In MyBase.List


        '    intSQLReturnValue = objCustomer.SetData()
        '    If intSQLReturnValue < 0 Then
        '        Exit For
        '    End If


        'Next
        'DataAccess.CreateDataAccess.EndTxn()
        'Return intSQLReturnValue

    End Function



    Public ReadOnly Property Item(ByVal index As Integer) As Customer
        Get
            Return CType(List(index), Customer)
        End Get

    End Property


    Public Sub Add(ByRef value As Customer)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub
    Public ReadOnly Property Indexof(ByVal newCustomerId As Integer) As Integer
        Get
            Dim objCustomer As Customer
            Dim objPOSBO As POSBO
            For Each objPOSBO In MyBase.List
                If CType(objPOSBO, Customer).CustomerId = newCustomerId Then
                    Return MyBase.InnerList.IndexOf(objPOSBO)
                    Exit Property
                End If
            Next
        End Get
    End Property
    Public ReadOnly Property Keys() As Integer()
        Get
            Dim mycustomerIds() As Integer


            Return mycustomerIds

        End Get
    End Property


    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function Contains(ByVal value As Customer) As Boolean

        MyBase.containsPOSBO(CType(value, POSBO))

    End Function
    Public Function GetCustomerById(ByVal intCustomerId As Integer) As Customer


    End Function
End Class
