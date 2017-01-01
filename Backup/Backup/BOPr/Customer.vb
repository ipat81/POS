Option Strict On
Imports System.Data.SqlClient


Public Class Customer
    Inherits POSBO


    Public Enum enumCustomerType As Integer
        RetailCustomer
        PUAccount
        CorpAccount
        Student
        NonStudent
        'HouseAccountHolder
        Staff
    End Enum

    Public Enum enumCustomerStatus As Integer
        Active
        InActive
        Terminated
        Paid
        NotPaid
    End Enum

    Private m_CustomerId As Integer
    Private m_CustomerType As enumCustomerType
    Private m_CustomerName As String
    Private m_CustomerAddress1 As String = " "
    Private m_CustomerAddress2 As String = " "
    Private m_CustomerAddress3 As String = " "
    Private m_DrivingDirections As String = " "
    Private m_CustomerPhoneNo As String = " "
    Private m_CustomerFaxNo As String = " "
    Private m_CustomerEmailId As String = " "
    Private m_CustomerSTaxExempt As Boolean = False
    Private m_CustomerChangedBy As Integer = 0
    Private m_CustomerChangedAt As DateTime = DateTime.MinValue
    Private m_CustomerStatus As enumCustomerStatus = enumCustomerStatus.Active
    Private m_CustomerSortOrder As Integer = 0
    Private m_isDirty As Boolean


    Public Sub New()
        CustomerId = 0
    End Sub



    Friend Sub New(ByVal newcustomerid As Integer, ByVal newcustomertype As enumCustomerType, _
                     ByVal newcustomername As String, ByVal newcustomeraddress1 As String, _
                     ByVal newcustomeraddress2 As String, ByVal newcustomeraddress3 As String, _
                     ByVal newcustomerphoneno As String, ByVal newcustomerfaxno As String, _
                     ByVal newcustomeremailid As String, ByVal newcustomerstaxexempt As Boolean, _
                     ByVal newcustomerchangedby As Integer, ByVal newcustomerchangedat As DateTime, _
                     ByVal newcustomerstatus As enumCustomerStatus)

        CustomerId = newcustomerid
        CustomerType = newcustomertype
        CustomerName = newcustomername
        CustomerAddress1 = newcustomeraddress1
        CustomerAddress2 = newcustomeraddress2
        CustomerAddress3 = newcustomeraddress3
        CustomerPhoneNo = newcustomerphoneno
        CustomerFaxNo = newcustomerfaxno
        CustomerEmailId = newcustomeremailid
        CustomerSTaxExempt = newcustomerstaxexempt
        CustomerChangedBy = newcustomerchangedby
        CustomerChangedAt = newcustomerchangedat
        CustomerStatus = newcustomerstatus

    End Sub

    Public Property CustomerId() As Integer

        Get
            Return m_CustomerId
        End Get

        Set(ByVal Value As Integer)
            m_CustomerId = Value
        End Set
    End Property
    Public ReadOnly Property strCustomerId() As String

        Get
            Return CType(m_CustomerId, String)
        End Get

    End Property
    Public Property CustomerType() As enumCustomerType
        Get
            Return m_CustomerType
        End Get
        Set(ByVal Value As enumCustomerType)
            m_CustomerType = Value
        End Set
    End Property



    Public Property CustomerName() As String

        Get
            Return m_CustomerName
        End Get

        Set(ByVal Value As String)
            m_CustomerName = Value
        End Set
    End Property


    Public Property CustomerAddress1() As String

        Get
            Return m_CustomerAddress1
        End Get

        Set(ByVal Value As String)
            m_CustomerAddress1 = Value
        End Set
    End Property

    Public Property CustomerAddress2() As String

        Get
            Return m_CustomerAddress2
        End Get

        Set(ByVal Value As String)
            m_CustomerAddress2 = Value
        End Set
    End Property


    Public Property CustomerAddress3() As String

        Get
            Return m_CustomerAddress3
        End Get

        Set(ByVal Value As String)
            m_CustomerAddress3 = Value
        End Set
    End Property

    Public Property DrivingDirections() As String

        Get
            Return m_DrivingDirections
        End Get

        Set(ByVal Value As String)
            m_DrivingDirections = Value
        End Set
    End Property

    Public Property CustomerPhoneNo() As String

        Get
            Return m_CustomerPhoneNo
        End Get

        Set(ByVal Value As String)
            If Value = String.Empty Then
                m_CustomerPhoneNo = " "
            Else
                m_CustomerPhoneNo.Replace("-", "")
                m_CustomerPhoneNo.Replace("(", "")
                m_CustomerPhoneNo.Replace(")", "")
                m_CustomerPhoneNo = Value
            End If
        End Set
    End Property

    Public ReadOnly Property PhoneNoDisplayValue() As String
        Get
            Try
                Return String.Format("{0:(###)###-####}", CDbl(CustomerPhoneNo))
            Catch ex As Exception
                Return CustomerPhoneNo
            End Try

        End Get

    End Property


    Public Property CustomerFaxNo() As String

        Get
            Return m_CustomerFaxNo
        End Get

        Set(ByVal Value As String)
            m_CustomerFaxNo = Value
        End Set
    End Property

    Public Property CustomerEmailId() As String

        Get
            Return m_CustomerEmailId
        End Get

        Set(ByVal Value As String)
            m_CustomerEmailId = Value
        End Set
    End Property


    Public Property CustomerSTaxExempt() As Boolean

        Get
            'Select Case CustomerName
            '    Case "Student", "MG Staff"
            '        Return False
            '    Case Else
            '        Return True
            'End Select
            Return m_CustomerSTaxExempt
        End Get

        Set(ByVal Value As Boolean)
            m_CustomerSTaxExempt = Value
        End Set
    End Property


    Public Property CustomerChangedBy() As Integer

        Get
            Return m_CustomerChangedBy
        End Get

        Set(ByVal Value As Integer)
            m_CustomerChangedBy = Value
        End Set
    End Property

    Public Property CustomerSortOrder() As Integer

        Get
            Return m_CustomerSortOrder
        End Get

        Set(ByVal Value As Integer)
            m_CustomerSortOrder = Value
        End Set
    End Property

    Public Property CustomerChangedAt() As DateTime

        Get
            Return m_CustomerChangedAt
        End Get

        Set(ByVal Value As DateTime)
            m_CustomerChangedAt = Value
        End Set
    End Property

    Public Property CustomerStatus() As enumCustomerStatus
        Get
            Return m_CustomerStatus
        End Get

        Set(ByVal Value As enumCustomerStatus)

            m_CustomerStatus = Value

        End Set
    End Property

    ReadOnly Property IsDirty() As Boolean

        Get
            Return m_isDirty
        End Get

    End Property

    Public Sub GetData(ByVal enumCustomerview As Customers.EnumView)

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        Dim msg As String
        Dim i As Integer
        Dim colname As String




        Select Case enumCustomerview
            Case Customers.EnumView.BaseView.BaseView
                sqlcommand = "Select CustomerId,CustomerName From customer"

            Case Customers.EnumView.CompleteView.CompleteView
                sqlcommand = "Select * from customer"

        End Select

        sqlcommand = sqlcommand & " where  CustomerId =  " & Me.CustomerId

        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then

            Do While SqlDr.Read()       'loops through the records in the SqlDr.


                For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                    colname = SqlDr.GetName(i)

                    Select Case colname

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
                                CustomerEmailId = SqlDr.GetString(i)
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
                                End If
                            Catch ex As Exception
                                '???errormessage???
                            End Try
                    End Select

                Next
            Loop
        End If

    End Sub


    Public Sub SetData()
        Dim objCustomer As Customer
        Dim strSqlCmd As String
        Dim strSTaxExempt As String
        Dim intSQLReturnValue As Integer
        Dim strCustomerChangedAt As String
        If Me.CustomerSTaxExempt = True Then
            strSTaxExempt = "1"
        ElseIf Me.CustomerSTaxExempt = False Then
            strSTaxExempt = "0"
        End If
        If Me.CustomerChangedAt = Date.MinValue Then
            strCustomerChangedAt = SaleDays.CreateSaleDay.Now().ToString
        ElseIf Me.CustomerSTaxExempt = False Then
            strSTaxExempt = "0"
        End If
        m_isDirty = True
        If Me.IsDirty Then
            DataAccess.CreateDataAccess.StartTxn()
            Select Case Me.CustomerId      'id=0 means insert new row, all other cases mean update row in SQL table 
                Case 0
                    strSqlCmd = "insert into Customer values (" & _
                                CStr(Me.CustomerType) & ",'" & _
                                Me.CustomerName & "','" & _
                                Me.CustomerAddress1 & "','" & _
                                Me.CustomerAddress2 & "','" & _
                                Me.CustomerAddress3 & "','" & _
                                Me.CustomerPhoneNo & "','" & _
                                Me.CustomerFaxNo & "','" & _
                                Me.CustomerEmailId & "'," & _
                                strSTaxExempt & "," & _
                                CStr(Me.CustomerChangedBy) & ",'" & _
                                strCustomerChangedAt & "'," & _
                                CStr(Me.CustomerStatus) & "," & _
                                Me.CustomerSortOrder & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'Error in insert procesing on SQL server
                    Else
                        Me.CustomerId = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update Customer set  " & _
                                "CustomerName='" & _
                                    Me.CustomerName & "'," & _
                                "CustomerAddress1='" & _
                                    Me.CustomerAddress1 & "'," & _
                                "CustomerAddress2='" & _
                                    Me.CustomerAddress2 & "'," & _
                                "CustomerAddress3='" & _
                                    Me.CustomerAddress3 & "'," & _
                                "CustomerPhoneNo= '" & _
                                    Me.CustomerPhoneNo & "'," & _
                                "CustomerFaxNo= '" & _
                                    Me.CustomerFaxNo & "'," & _
                                 "CustomerEmailId= '" & _
                                    Me.CustomerEmailId & "'," & _
                                "CustomerChangedBy=" & _
                                    CStr(Me.CustomerChangedBy) & "," & _
                                "CustomerChangedAt='" & _
                                    Me.CustomerChangedAt.ToString & "'," & _
                                "CustomerStatus=" & _
                                    CStr(Me.CustomerStatus) & _
                                  ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select

            If intSQLReturnValue > 0 Then DataAccess.CreateDataAccess.EndTxn()
        End If

    End Sub


End Class
