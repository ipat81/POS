Option Strict On
Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections.Specialized

Public Class SaleDays
    Inherits POSBOs
    Private Shared m_SaleDays As SaleDays
    Private m_CurrentSaleDay As SaleDay

    Private m_Cashiers As Employees
    Private m_Employees As Employees
    Private m_Customers As Customers
    Private m_Sessions As Sessions
    Private m_Managers As Employees
    Private m_Owners As Employees
    Private m_ActiveTables As DTables
    Private m_ActiveProductCategories As ProductCategories

    Private m_CRId As Integer

    Public Event SaleDayClosed()
    Public Event SaleDayStarted()

    Public Enum EnumView As Integer  'This enum determines the select clause
        BaseView
        CompleteView
    End Enum

    Public Enum EnumFilter As Integer  'This enum determines the where condition
        Open
        Closed
        All
    End Enum
    Public Shared Function CreateSaleDay() As SaleDays          'Create a Singleton SaleDays

        If (m_SaleDays Is Nothing) Then
            m_SaleDays = New SaleDays()
        End If
        Return m_SaleDays
    End Function

    Public ReadOnly Property ActiveEmployees(ByVal enumEmployeeView As Employees.EnumView) As Employees
        Get
            If m_Employees Is Nothing Then m_Employees = New Employees(Employees.EnumFilter.Active, enumEmployeeView)
            Return m_Employees
        End Get
    End Property


    Public ReadOnly Property ActiveCashierList() As ListDictionary
        Get
            Dim objEmployee As Employee
            Dim emplist As New ListDictionary()
            With emplist
                For Each objEmployee In ActiveCashiers
                    .Add(objEmployee.EmployeeId, objEmployee.EmployeeFullName)
                Next
            End With
            Return emplist
        End Get
    End Property

    Public ReadOnly Property ActiveProductCategories() As ProductCategories
        Get
            If m_ActiveProductCategories Is Nothing Then
                m_ActiveProductCategories = New ProductCategories _
                (ProductCategories.EnumFilter.Active, ProductCategories.EnumView.CompleteView)
            End If
            Return m_ActiveProductCategories
        End Get
    End Property
    Public ReadOnly Property MenuItemFromId(ByVal intMenuItemId As Integer) As MenuItem
        Get
            Dim objProductCategory As ProductCategory
            Dim objMenuItem As MenuItem
            Dim intIndex As Integer

            For Each objProductCategory In ActiveProductCategories
                intIndex = objProductCategory.ActiveMenuItems.IndexOf(intMenuItemId)
                If intIndex >= 0 Then
                    Return objProductCategory.ActiveMenuItems.Item(intIndex)
                End If
            Next
        End Get
    End Property

    Public ReadOnly Property ActiveCashiers() As Employees
        Get
            If m_Cashiers Is Nothing Then m_Cashiers = New Employees(Employees.EnumFilter.ActiveCashiers, Employees.EnumView.CompleteView)
            Return m_Cashiers
        End Get
    End Property

    Public ReadOnly Property ActiveCustomers() As Customers
        Get
            If m_Customers Is Nothing Then m_Customers = New Customers(Customers.EnumFilter.Active, Customers.EnumView.CompleteView)
            Return m_Customers
        End Get
    End Property
    Public ReadOnly Property ActiveManagers() As Employees
        Get
            If m_Managers Is Nothing Then m_Managers = New Employees(Employees.EnumFilter.ActiveManagers, Employees.EnumView.CompleteView)
            Return m_Managers
        End Get
    End Property

    Public ReadOnly Property ActiveOwners() As Employees
        Get
            If m_Owners Is Nothing Then m_Owners = New Employees(Employees.EnumFilter.ActiveOwners, Employees.EnumView.CompleteView)
            Return m_Owners
        End Get
    End Property

    Public ReadOnly Property ActiveSessions() As Sessions
        Get
            If m_Sessions Is Nothing Then m_Sessions = New Sessions(Sessions.EnumFilter.Active, Sessions.EnumView.CompleteView)
            Return m_Sessions
        End Get
    End Property
    'Public ReadOnly Property CurrentSaleDay() As SaleDay
    '    Get
    '        Dim TargetSaleDate As Date
    '        Dim objSaleDay As SaleDay

    '        If m_CurrentSaleDay Is Nothing Then
    '            TargetSaleDate = GetTargetSaleDate()
    '            GetPersistedSaleDays(TargetSaleDate, TargetSaleDate, EnumView.CompleteView)
    '            objSaleDay = Me.Item(TargetSaleDate)
    '            If (objSaleDay Is Nothing) OrElse (objSaleDay.IsDayOpen = False) Then
    '                m_CurrentSaleDay = Nothing 'nop no current saleday exists
    '            Else
    '                m_CurrentSaleDay = objSaleDay
    '            End If
    '        End If
    '        'returns nothing if startasaleday has not been executed
    '        Return m_CurrentSaleDay
    '    End Get
    'End Property

    Public Function GetTargetSaleDate() As Date
        Dim objDataAccess As DataAccess
        Dim targetSaleDate As DateTime

        objDataAccess = DataAccess.CreateDataAccess
        targetSaleDate = objDataAccess.Now
        If targetSaleDate.ToLongTimeString() <= "06:00:00 AM" Then
            targetSaleDate = DateAndTime.DateAdd(DateInterval.Day, -1, DateAndTime.Today)
        Else
            targetSaleDate = DateAndTime.Today()
        End If
        Return targetSaleDate.Date
    End Function

    'Public Function CreateNewSaleDay(ByVal targetSaleDate As Date) As SaleDay
    '    Dim objSaleDay As SaleDay

    '    objSaleDay = Me.Item(targetSaleDate)
    '    If Not (objSaleDay Is Nothing) Then
    '        Return objSaleDay
    '    Else
    '        If targetSaleDate > Me.CurrentSaleDay.SaleDate Then
    '            objSaleDay = Me.AddNewSaleDay(targetSaleDate)
    '        Else
    '            Me.GetPersistedSaleDays(targetSaleDate, targetSaleDate, SaleDays.EnumView.CompleteView)
    '            objSaleDay = Me.Item(targetSaleDate)
    '        End If
    '    End If

    '    Return objSaleDay
    'End Function

    Private Function AddNewSaleDay(ByVal targetSaleDate As Date) As SaleDay
        Dim objSaleDay As SaleDay
        Dim intSqlReturnValue As Integer

        If targetSaleDate < Me.Now.Date Then
            'error
            objSaleDay = Nothing
        Else
            objSaleDay = New SaleDay(targetSaleDate)
            objSaleDay.SetSaleDayType(SaleDay.enumSaleDayType.Regular)
            objSaleDay.SetSaleDayOpenAt(SaleDays.CreateSaleDay.Now)
            Me.Add(objSaleDay)
            intSqlReturnValue = Me.SetData(objSaleDay)
            If intSqlReturnValue < 0 Then
                MsgBox(" Error in Saving SaleDay")
            Else
                'nope
            End If

        End If
        Return objSaleDay
    End Function

    Private Sub GetPersistedSaleDays(ByVal targetSaleDate As Date, _
                    ByVal enumSaleDayview As EnumView)

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim objSaleDay As SaleDay
        Dim msg As String

        Select Case enumSaleDayview
            Case EnumView.BaseView
                sqlcommand = "Select * From SaleDay"

            Case EnumView.CompleteView
                sqlcommand = "Select * from SaleDay"
        End Select

        sqlcommand = sqlcommand & _
        " where SaleDate ='" & targetSaleDate.ToShortDateString & "'"

        objDataAccess = DataAccess.CreateDataAccess
        SqlDr = objDataAccess.GetData(sqlcommand)
        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objSaleDay = New SaleDay()
                Me.Add(objSaleDay)
                objSaleDay.loadData(SqlDr)
            Loop
            SqlDr.Close()
        End If
        objDataAccess.CloseConnection()
    End Sub

    Public Function Now() As DateTime
        Return DataAccess.CreateDataAccess.Now

    End Function

    Public ReadOnly Property Item(ByVal index As Integer) As SaleDay
        Get
            If index >= 0 Then
                Return CType(List(index), SaleDay)
            End If
        End Get

    End Property

    Public ReadOnly Property Item(ByVal targetSaleDate As Date, ByVal ReturnNew As Boolean) As SaleDay
        Get
            Dim intIndex As Integer
            intIndex = Me.Indexof(targetSaleDate)

            If intIndex < 0 Then
                GetPersistedSaleDays(targetSaleDate, EnumView.CompleteView)
                intIndex = Me.Indexof(targetSaleDate)
                If intIndex < 0 Then
                    Select Case ReturnNew
                        Case True
                            AddNewSaleDay(targetSaleDate)
                            intIndex = Me.Indexof(targetSaleDate)
                        Case False
                            'nop
                    End Select
                End If
            End If
            If intIndex < 0 Then
                Return Nothing
            Else
                Return Me.Item(intIndex)
            End If
        End Get
    End Property

    Private Sub Add(ByRef value As SaleDay)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub
    Public ReadOnly Property Indexof(ByVal newSaleDate As Date) As Integer
        Get
            Dim objSaleDay As SaleDay
            Dim intIndex As Integer

            intIndex = -1
            For Each objSaleDay In Me
                If objSaleDay.SaleDate.Date = newSaleDate.Date Then
                    intIndex = MyBase.List.IndexOf(objSaleDay)
                    Exit For
                End If
            Next
            Return intIndex
        End Get
    End Property
    Public ReadOnly Property Keys() As Integer()
        Get

        End Get
    End Property


    Public Sub Remove(ByVal index As Integer)
        MyBase.removePOSBO(index)
    End Sub

    Public Function Contains(ByVal newSaleDay As SaleDay) As Boolean

        MyBase.containsPOSBO(CType(newSaleDay, POSBO))

    End Function

    Public Function Contains(ByVal newSaleDate As Date) As Boolean
        Dim objSaleDay As SaleDay
        For Each objSaleDay In MyBase.List
            If objSaleDay.SaleDate = newSaleDate Then
                Return True
                Exit Function
            End If
        Next
    End Function

    Private Sub New()
        SetPhysicalCRId()
    End Sub

    'Public Sub StartASaleDay(ByVal newsaledate As Date)
    '    Dim objSaleDay As SaleDay

    '    objSaleDay = Me.Item(newsaledate)
    '    If objSaleDay Is Nothing Then objSaleDay = Me.AddNewSaleDay(newsaledate)
    '    With objSaleDay
    '        .SetSaleDayOpenAt(Me.Now)
    '        .SetSaleDayOpenBy(0)
    '        .SetSaleDate(newsaledate)
    '        .SetSaleDayType(SaleDay.enumSaleDayType.Regular)
    '    End With

    '    If Me.SetData(objSaleDay) > 0 Then
    '        m_CurrentSaleDay = objSaleDay
    '        RaiseEvent SaleDayStarted()
    '    Else
    '        'error
    '    End If
    'End Sub

    'Public Sub CloseCurrentSaleDay()
    '    Dim objSaleDay As SaleDay
    '    objSaleDay = Me.Item(CurrentSaleDay.SaleDate)
    '    If objSaleDay Is Nothing Then
    '        'error
    '    Else
    '        With objSaleDay
    '            .SetSaleDayCloseAt(Me.Now)
    '            .SetSaleDayCloseBy(0)
    '        End With
    '        If Me.SetData(objSaleDay) > 0 Then
    '            m_CurrentSaleDay = Nothing
    '            RaiseEvent SaleDayClosed()
    '        Else
    '            'error
    '        End If
    '    End If
    'End Sub

    Private Function SetData(ByVal objSaleDay As SaleDay) As Integer
        Dim intSQLReturnValue As Integer

        'For Each objSaleDay In MyBase.List
        intSQLReturnValue = objSaleDay.SetData()
        'If intSQLReturnValue < 0 Then Exit For
        'Next

        Return intSQLReturnValue
    End Function
    '************ Moved from SaleDay
    Public ReadOnly Property CRId() As Integer
        Get
            Return m_CRId
        End Get

    End Property
    Private Sub SetPhysicalCRId()
        Dim intCRId As Integer
        Dim strCRID As String

        Dim objStreamReader As StreamReader = File.OpenText("C:\POS2006\PosCRId.ini")
        With objStreamReader
            .BaseStream.Seek(0, SeekOrigin.Begin)
            intCRId = .Read                 'returns ASCII code for CR Id
            strCRID = (Chr(intCRId))        'cant use cint on chr !!
            m_CRId = CInt(strCRID)
            .Close()
        End With
    End Sub
    Public ReadOnly Property ActiveTables() As DTables
        Get
            If m_ActiveTables Is Nothing Then m_ActiveTables = New DTables(DTables.EnumFilter.Active, DTables.EnumView.CompleteView)
            Return m_ActiveTables
        End Get
    End Property
End Class

