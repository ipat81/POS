Option Strict On
Imports System.Data.SqlClient


Public Class Employee
    Inherits POSBO

    Public Enum enumSex As Integer
        Male
        Female
    End Enum

    Public Enum enumEmployeeRoleId As Integer
        Cashier
        Manager
        Waiter
        KitchenStaff
        Owner
        BusPerson
    End Enum

    Public Enum enumEmployeeStatus As Integer
        Active
        Inactive
        Deleted
        Terminated
    End Enum


    Private m_EmployeeId As Integer
    Private m_EmployeeFirstName As String
    Private m_EmployeeMiddleName As String
    Private m_EmployeeLastName As String
    Private m_EmployeeFullName As String
    Private m_EmployeeRoleId As Employee.enumEmployeeRoleId
    Private m_EmployeeDateOfBirth As DateTime
    Private m_EmployeeSex As Employee.enumSex
    Private m_EmployeePassword As String
    Private m_EmployeeHireDate As DateTime
    Private m_EmployeeTerminationDate As DateTime
    Private m_EmployeeChangedBy As Integer
    Private m_EmployeeChangedAt As DateTime
    Private m_EmployeeStatus As Employee.enumEmployeeStatus
    Private m_IsDirty As Boolean

    Private m_CRBalances As CRBalances

    Public Sub New()
        EmployeeId = 0
    End Sub




    Friend Sub New(ByVal newemployeeid As Integer, ByVal newemployeefirstname As String, _
                    ByVal newemployeemiddlename As String, ByVal newemployeelastname As String, _
                    ByVal newemployeeroleid As Employee.enumEmployeeRoleId, _
                    ByVal newemployeedateofbirth As DateTime, ByVal newemployeeSex As Employee.enumSex, _
                    ByVal newemployeepassword As String, ByVal newemployeehiredate As DateTime, _
                    ByVal newemployeeTerminationdate As DateTime, ByVal newemployeechangedBy As Integer, _
                    ByVal newemployeechangedat As DateTime, ByVal newemployeestatus As Employee.enumEmployeeStatus)

        EmployeeId = newemployeeid
        EmployeeFirstName = newemployeefirstname
        EmployeeMiddleName = newemployeemiddlename
        EmployeeLastName = newemployeelastname
        EmployeeRoleId = newemployeeroleid
        EmployeeDateOfBirth = newemployeedateofbirth
        EmployeeSex = newemployeeSex
        EmployeePassword = newemployeepassword
        EmployeeHireDate = newemployeehiredate
        EmployeeTerminationDate = newemployeeTerminationdate
        EmployeeChangedBy = newemployeechangedBy
        EmployeeChangedAt = newemployeechangedat
        EmployeeStatus = newemployeestatus

    End Sub

    Public Property EmployeeId() As Integer

        Get
            Return m_EmployeeId
        End Get

        Set(ByVal Value As Integer)
            setUpdateStatus(Value)

            m_EmployeeId = Value
        End Set
    End Property


    Public Property EmployeeFirstName() As String

        Get
            Return m_EmployeeFirstName
        End Get

        Set(ByVal Value As String)

            setUpdateStatus(Value)
            m_EmployeeFirstName = Value

        End Set
    End Property


    Public Property EmployeeMiddleName() As String

        Get
            Return m_EmployeeMiddleName
        End Get

        Set(ByVal Value As String)
            setUpdateStatus(Value)
            m_EmployeeMiddleName = Value
        End Set
    End Property

    Public Property EmployeeLastName() As String

        Get
            Return m_EmployeeLastName
        End Get

        Set(ByVal Value As String)
            setUpdateStatus(Value)
            m_EmployeeLastName = Value
        End Set
    End Property


    Public ReadOnly Property EmployeeFullName() As String
        Get
            Return RTrim(EmployeeFirstName) & " " & LTrim(EmployeeLastName)
        End Get
    End Property

    Public ReadOnly Property EmployeeShortName() As String
        Get
            Return RTrim(EmployeeFirstName) & EmployeeLastName.Substring(0)
        End Get
    End Property

    Public Property EmployeeRoleId() As Employee.enumEmployeeRoleId

        Get
            Return m_EmployeeRoleId
        End Get

        Set(ByVal Value As Employee.enumEmployeeRoleId)
            setUpdateStatus(Value)
            m_EmployeeRoleId = Value
        End Set
    End Property


    Public Property EmployeeDateOfBirth() As DateTime

        Get
            Return m_EmployeeDateOfBirth
        End Get

        Set(ByVal Value As DateTime)
            setUpdateStatus(Value)
            m_EmployeeDateOfBirth = Value
        End Set
    End Property

    Public Property EmployeeSex() As Employee.enumSex

        Get
            Return m_EmployeeSex
        End Get

        Set(ByVal Value As Employee.enumSex)
            setUpdateStatus(Value)
            m_EmployeeSex = Value
        End Set
    End Property


    Public Property EmployeePassword() As String

        Get
            Return m_EmployeePassword
        End Get

        Set(ByVal Value As String)
            setUpdateStatus(Value)
            m_EmployeePassword = Value
        End Set
    End Property


    Public Property EmployeeHireDate() As DateTime
        Get
            Return m_EmployeeHireDate
        End Get
        Set(ByVal Value As DateTime)
            setUpdateStatus(Value)
            m_EmployeeHireDate = Value
        End Set
    End Property


    Public Property EmployeeTerminationDate() As DateTime
        Get
            Return m_EmployeeTerminationDate
        End Get
        Set(ByVal Value As DateTime)
            setUpdateStatus(Value)
            m_EmployeeTerminationDate = Value
        End Set
    End Property


    Public Property EmployeeChangedBy() As Integer

        Get
            Return m_EmployeeChangedBy
        End Get

        Set(ByVal Value As Integer)
            setUpdateStatus(Value)
            m_EmployeeChangedBy = Value
        End Set
    End Property


    Public Property EmployeeChangedAt() As DateTime

        Get
            Return m_EmployeeChangedAt
        End Get

        Set(ByVal Value As DateTime)
            setUpdateStatus(Value)
            m_EmployeeChangedAt = Value
        End Set

    End Property


    Public Property EmployeeStatus() As Employee.enumEmployeeStatus

        Get
            Return m_EmployeeStatus
        End Get

        Set(ByVal Value As Employee.enumEmployeeStatus)
            setUpdateStatus(Value)
            m_EmployeeStatus = Value
        End Set
    End Property

    ReadOnly Property IsDirty() As Boolean

        Get
            Return m_IsDirty = False
        End Get

    End Property

    Private Sub setUpdateStatus(ByVal newPropertyValue As Object)

        m_IsDirty = True

    End Sub

    Friend Sub GetData(ByVal enumEmployeeView As Employees.EnumView)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim SqlDrColName As String
        Dim objDataAccess As DataAccess

        Dim msg As String
        Dim i As Integer

        objDataAccess = DataAccess.CreateDataAccess


        Select Case enumEmployeeView

            Case Employees.EnumView.BaseView
                sqlcommand = "Select EmployeeId,EmployeeFirstName,EmployeeLastName From Employee"

            Case Employees.EnumView.CompleteView
                sqlcommand = "Select * from Employee"

        End Select

        sqlcommand = sqlcommand & " where  EmployeeId =  " & Me.EmployeeId

        'Employee record accessed from the database.GetData method connects to the database fetches  the record
        SqlDr = objDataAccess.GetData(sqlcommand)
        If Not SqlDr Is Nothing Then

            Do While SqlDr.Read()       'loops through the records in the SqlDr.


                For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                    SqlDrColName = SqlDr.GetName(i) 'Reading  the column Name

                    Select Case SqlDrColName


                        Case "EmployeeFirstName"

                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeFirstName = SqlDr.GetString(i)
                            End If

                        Case "EmployeeMiddleName"
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeMiddleName = SqlDr.GetString(i)
                            End If

                        Case "EmployeeLastName"
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeLastName = SqlDr.GetString(i)
                            End If


                        Case "EmployeeRoleId"
                            Try
                                If Not SqlDr.IsDBNull(i) Then
                                    EmployeeRoleId = CType(SqlDr.GetInt16(i), Employee.enumEmployeeRoleId)
                                End If
                            Catch ex As Exception
                                '???errormessage???

                            End Try

                        Case "EmployeeDateOfBirth"
                            EmployeeDateOfBirth = SqlDr.GetDateTime(i)

                        Case "EmployeeSex"
                            Try
                                EmployeeSex = CType(SqlDr.GetInt16(i), Employee.enumSex)
                            Catch ex As Exception
                                '???errormessage???
                            End Try

                        Case "EmployeePassword"
                            EmployeePassword = SqlDr.GetString(i)


                        Case "EmployeeHireDate"
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeHireDate = SqlDr.GetDateTime(i)
                            Else
                                EmployeeHireDate = Now()
                            End If


                        Case "EmployeeTerminationDate"
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeTerminationDate = SqlDr.GetDateTime(i)
                            Else
                                EmployeeTerminationDate = Now()
                            End If


                        Case "EmployeeChangedBy"
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeChangedBy = SqlDr.GetInt32(i)
                            Else
                                EmployeeChangedBy = 0
                            End If

                        Case "EmployeeChangedAt"
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeChangedAt = SqlDr.GetDateTime(i)
                            Else
                                EmployeeChangedAt = Date.MinValue
                            End If

                        Case "EmployeeStatus"
                            Try
                                If Not SqlDr.IsDBNull(i) Then
                                    EmployeeStatus = CType(SqlDr.GetInt16(i), Employee.enumEmployeeStatus)
                                End If

                            Catch ex As Exception
                                '???errormessage???
                            End Try
                    End Select

                Next
            Loop
        End If

    End Sub
    'Public ReadOnly Property CashierCashBalances() As CRBalances
    '    Get
    '        Return m_CRBalances
    '    End Get
    'End Property

    'Private Sub RestoreCashierBalances()   'Called when application is started again after opening day
    '    If m_CRBalances Is Nothing Then
    '        m_CRBalances = New CRBalances(CRBalances.EnumFilter.Active, CRBalances.EnumView.CompleteView)
    '    End If
    'End Sub

    'Public Sub CashierSignOn(ByVal objMoneyCount As MoneyCount)
    '    Dim objCRBalance As CRBalance
    '    If m_CRBalances Is Nothing Then m_CRBalances = New CRBalances()
    '    objCRBalance = New CRBalance()
    '    m_CRBalances.Add(objCRBalance)
    '    With objCRBalance
    '        .CRBalanceId = 0        'new
    '        .CRBalanceAmount = objMoneyCount.TotalCashAmount
    '        .CRBalanceChangedBy = Me.EmployeeId
    '        .CRBalanceMoneyCounts.Add(objMoneyCount)
    '        .CRBalanceSaleDate = SaleDay.CreateSaleDay.SaleDate
    '        .CRBalanceStatus = CRBalance.enumCRBalanceStatus.Active
    '        .CRID = SaleDay.CreateSaleDay.CRId
    '        .CashierId = Me.EmployeeId
    '    End With

    'End Sub
End Class
