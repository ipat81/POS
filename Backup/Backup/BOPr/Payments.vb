Option Strict On
Imports System.Data.SqlClient

Public Class Payments
    Inherits POSBOs
    Private m_CurrentPayment As Payment
    Private m_order As Order

    Public Enum EnumView As Integer  'This enum determines the select clause
        BaseView
        CompleteView
    End Enum

    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        Voided
    End Enum

    Public Sub New()

    End Sub

    Friend Sub SetParentOrder(ByVal objOrder As Order)
        m_order = objOrder
    End Sub

    'This constructor creates a coll of All Payments receieved for sale done on ANY SaleDay other than Date passed  
    'Used for calculating POS Balances 
    Public Sub New(ByVal enumPaymentFilter As EnumFilter, _
         ByVal enumPaymentView As EnumView, ByVal currentSaleDate As Date)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim nextSaleDate As DateTime

        Dim msg As String

        nextSaleDate = currentSaleDate.AddDays(1)
        Select Case enumPaymentView
            Case EnumView.BaseView
                sqlcommand = "Select PaymentId,OrderID From Payment"

            Case EnumView.CompleteView
                sqlcommand = "Select * from Payment INNER JOIN [order] ON Payment.OrderId=[order].OrderId"
        End Select

        Select Case enumPaymentFilter
            Case EnumFilter.All
                sqlcommand = sqlcommand & " where Payment.EndTime BETWEEN '" & currentSaleDate.ToString & "' AND " & _
                                            "'" & nextSaleDate.ToString & "' and [order].SaleDate <> '" & currentSaleDate.Date.ToShortDateString & "'"
            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where Payment.Status = " & Payment.enumStatus.Active & _
                                         " And EndTime BETWEEN '" & currentSaleDate.ToString & "' AND " & _
                                            "'" & nextSaleDate.ToString & "' and [order].SaleDate <> '" & currentSaleDate.Date.ToShortDateString & "'"
            Case EnumFilter.Voided
                sqlcommand = sqlcommand & " where Payment.Status = " & Payment.enumStatus.Voided & _
                                         " And Payment.EndTime BETWEEN '" & currentSaleDate.ToString & "' AND " & _
                                            "'" & nextSaleDate.ToString & "' and [order].SaleDate <> '" & currentSaleDate.Date.ToShortDateString & "'"
        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class

        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Dim objPayment As Payment

            Do While SqlDr.Read()
                objPayment = New Payment()
                Me.Add(objPayment)
                objPayment.ParentPayments = Me
                objPayment.ReadSqlDr(SqlDr)
            Loop
            SqlDr.Close()
        End If
        objDataAccess.CloseConnection()
    End Sub

    Public Sub New(ByVal enumPaymentFilter As EnumFilter, _
     ByVal enumPaymentView As EnumView, ByVal intOrderId As Integer)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        Dim msg As String


        Select Case enumPaymentView
            Case EnumView.BaseView
                sqlcommand = "Select PaymentId,OrderID From Payment"

            Case EnumView.CompleteView
                sqlcommand = "Select * from Payment"
        End Select

        Select Case enumPaymentFilter
            Case EnumFilter.All
                sqlcommand = sqlcommand & " where OrderId = " & intOrderId
            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where Status = " & Payment.enumStatus.Active & _
                                         " And OrderId = " & intOrderId
            Case EnumFilter.Voided
                sqlcommand = sqlcommand & " where Status = " & Payment.enumStatus.Voided & _
                                         " And OrderId = " & intOrderId
        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class

        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Dim objPayment As Payment

            Do While SqlDr.Read()
                objPayment = New Payment()
                Me.Add(objPayment)
                objPayment.ParentPayments = Me
                objPayment.ReadSqlDr(SqlDr)
            Loop
            SqlDr.Close()
        End If
        objDataAccess.CloseConnection()
    End Sub

    Public ReadOnly Property Item(ByVal index As Integer) As Payment
        Get
            Return CType(MyBase.itemPOSBO(index), Payment)
        End Get
    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myPaymentIds() As Integer
            Return myPaymentIds
        End Get
    End Property

    Public Sub Add(ByRef value As Payment)
        MyBase.addPOSBO(CType(value, POSBO))
    End Sub

    Public ReadOnly Property ActiveCount() As Integer
        Get
            Dim i As Integer
            Dim objPayment As Payment

            For Each objPayment In Me
                Select Case objPayment.Status
                    Case Payment.enumStatus.Active, Payment.enumStatus.NewPayment
                        i = +1
                    Case Else
                        'nop
                End Select
            Next
            Return i
        End Get
    End Property

    Public Sub Remove(ByVal index As Integer)
        MyBase.removePOSBO(index)
    End Sub

    Public Function IndexOf(ByRef value As Payment) As Integer
        Return MyBase.List.IndexOf(value)
    End Function

    Public Function Contains(ByVal value As Payment) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))
    End Function

    Friend Function SetData() As Integer
        Dim objPayment As Payment
        Dim strSqlCmd As String
        Dim intSQLReturnValue As Integer

        For Each objPayment In Me
            intSQLReturnValue = objPayment.SetData()
            If intSQLReturnValue < 0 Then Exit For
        Next
        Return intSQLReturnValue
    End Function

    Public Sub AddNewPaymentObject()
        Dim objPayment As Payment

        If Me.Count > 0 Then
            For Each objPayment In Me
                If objPayment.Status = Payment.enumStatus.NewPayment Then Exit Sub
            Next
        End If
        objPayment = AddPayment()
        objPayment.ParentPayments = Me
        objPayment.Status = Payment.enumStatus.NewPayment
    End Sub

    Public Function AddPayment() As Payment
        m_CurrentPayment = New Payment()
        Me.Add(m_CurrentPayment)
        m_CurrentPayment.ParentPayments = Me
        Return m_CurrentPayment
    End Function

    Public ReadOnly Property ParentOrder() As Order
        Get
            Return m_order
        End Get
    End Property

    Public ReadOnly Property CurrentPayment() As Payment
        Get
            Return m_CurrentPayment
        End Get
    End Property

    Public ReadOnly Property IsDirty() As Boolean
        Get
            Dim objPayment As Payment
            For Each objPayment In Me
                If objPayment.IsDirty = True Then Return True
            Next
            Return False
        End Get
    End Property

End Class
