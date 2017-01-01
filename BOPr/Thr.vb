
Option Strict On
Imports System.Data.SqlClient
Imports System.Globalization
Public Class Thr
    Inherits POSBO



    Private m_ThrsId As Integer
    Private m_ThrsEmployeeId As Integer
    Private m_ThrsDateTo As DateTime
    Private m_ThrsQBHrs As Integer
    Private m_ThrsQBMins As Integer
    Private m_ThrsClockHrs As Integer
    Private m_ThrsClockMins As Integer
    Private m_ThrsClockRecCount As Integer
    Private m_ThrsFixMins As Integer

    Private m_isDirty As Boolean
    Private m_objEmployee As Employee
    Private m_objSaleDay As SaleDay

    Private m_IsPersistedThrs As Boolean

    Public Sub New()
        ThrsId = 0
        'ThrDateTo = DateTime.MinValue
        'ThrTimeOut = System.DBNull.Value
    End Sub

    Friend Sub New(ByVal newThrsId As Integer, ByVal newThrsemployeeid As Integer, _
                    ByVal newThrsDateTo As DateTime, ByVal newThrsQBHrs As Integer, _
                    ByVal newThrsQBMins As Integer, ByVal newThrsClockHrs As Integer, _
                    ByVal newThrsClockMins As Integer, ByVal newThrsClockRecCount As Integer, _
                    ByVal newThrsFixMins As Integer)

        m_ThrsId = newThrsId
        m_ThrsEmployeeId = newThrsemployeeid
        m_ThrsDateTo = newThrsDateTo
        m_ThrsQBHrs = newThrsQBHrs
        m_ThrsQBMins = newThrsQBMins
        m_ThrsClockHrs = newThrsClockHrs
        m_ThrsClockMins = newThrsClockMins
        m_ThrsClockRecCount = newThrsClockRecCount
        m_ThrsFixMins = newThrsFixMins

        IsPersistedThrs = True
    End Sub

    Public Property ThrsId() As Integer

        Get
            Return m_ThrsId
        End Get

        Set(ByVal Value As Integer)
            m_ThrsId = Value
        End Set
    End Property


    Public Property ThrsEmployeeId() As Integer

        Get
            Return m_ThrsEmployeeId
        End Get

        Set(ByVal Value As Integer)
            m_ThrsEmployeeId = Value
        End Set
    End Property


    Public Property ThrDateTo() As DateTime
        Get
            Return m_ThrsDateTo
        End Get
        Set(ByVal newThrsDateTo As DateTime)
            m_ThrsDateTo = newThrsDateTo
        End Set
    End Property

    Public Property ThrsQBHrs() As Integer
        Get
            'If m_ThrsQBHrs = 0 Then
            '    Return m_ThrsClockHrs
            'Else
            Return m_ThrsQBHrs
            'End If
        End Get
        Set(ByVal newThrsQBHrs As Integer)
            m_ThrsQBHrs = newThrsQBHrs
        End Set
    End Property

    Public Property ThrsQBMins() As Integer
        Get
            'If m_ThrsQBMins = 0 Then
            '    Return m_ThrsClockMins
            'Else
            Return m_ThrsQBMins
            'End If
        End Get
        Set(ByVal newThrsQBMins As Integer)
            m_ThrsQBMins = newThrsQBMins
        End Set
    End Property
    Public Property ThrsClockHrs() As Integer
        Get
            Return m_ThrsClockHrs
        End Get
        Set(ByVal newThrsClockHrs As Integer)
            m_ThrsClockHrs = newThrsClockHrs
        End Set
    End Property

    Public Property ThrsClockMins() As Integer
        Get
            Return m_ThrsClockMins
        End Get
        Set(ByVal newThrsClockMins As Integer)
            m_ThrsClockMins = newThrsClockMins
        End Set
    End Property
    Public Property ThrsClockRecCount() As Integer
        Get
            Return m_ThrsClockRecCount
        End Get
        Set(ByVal newThrsClockRecCount As Integer)
            m_ThrsClockRecCount = newThrsClockRecCount
        End Set
    End Property
    Public Property ThrsFixMins() As Integer
        Get
            Return m_ThrsFixMins
        End Get
        Set(ByVal newThrsFixMins As Integer)
            m_ThrsFixMins = newThrsFixMins
        End Set
    End Property
    'Public ReadOnly Property TimeWorked() As TimeSpan
    '    'Get
    '    '    Select Case Me.ThrStatus
    '    '        Case enumThrStatus.Active, enumThrStatus.OverriddenNew
    '    '            If (ThrDateTo > DateTime.MinValue) AndAlso _
    '    '                            (ThrTimeOut > DateTime.MinValue) Then 'AndAlso _
    '    '                '(ThrDateTo.Date = ThrTimeOut.Date) Then
    '    '                Return ThrTimeOut.Subtract(ThrDateTo)
    '    '            Else
    '    '                Return TimeSpan.Zero
    '    '            End If
    '    '        Case Else
    '    '            Return TimeSpan.Zero
    '    '    End Select
    'End Get
    'End Property

    'Public ReadOnly Property TimeWorkedDisplay() As String
    '    Get
    '        Dim intHrs As Integer
    '        Dim intMins As Integer
    '        Dim strTimeWorked As String
    '        If Me.TimeWorked.TotalHours() = 0 Then
    '            strTimeWorked = String.Empty
    '        Else
    '            intHrs = CInt(Math.Floor(Me.TimeWorked.TotalHours()))
    '            intMins = CInt(Math.Floor(Me.TimeWorked.TotalMinutes - (intHrs * 60)))
    '            strTimeWorked = Format(intHrs, "00") & ":" & Format(intMins, "00")
    '        End If
    '        Return strTimeWorked
    '    End Get
    'End Property




    Public Property OBJEmployee() As Employee
        Get
            Return m_objEmployee
        End Get

        Set(ByVal Value As Employee)
            m_objEmployee = Value
        End Set
    End Property



    Public Property IsDirty() As Boolean

        Get
            Return m_isDirty
        End Get
        Set(ByVal Value As Boolean)
            m_isDirty = Value
        End Set

    End Property


    'Public Sub GetData()
    '    Dim sqlcommand As String
    '    Dim SqlDr As SqlDataReader
    '    Dim SqlDrColName As String
    '    Dim objDataAccess As DataAccess

    '    Dim msg As String
    '    Dim i As Integer

    '    objDataAccess = DataAccess.CreateDataAccess

    '    'Select Case enumThrView
    '    '    Case Thrs.EnumView.BaseView
    '    '        sqlcommand = "Select ThrId,ThrId,ThrDateTo From Thr"

    '    '    Case Thrs.EnumView.CompleteView
    '    '        sqlcommand = "Select * from Thr"

    '    'End Select


    '    sqlcommand = "Select * from Thr" & " where  ThrId =  " & Me.ThrId

    '    'Employee record accessed from the database.GetData method connects to the database fetches  the record
    '    SqlDr = objDataAccess.GetData(sqlcommand)
    '    If Not SqlDr Is Nothing Then


    '        Do While SqlDr.Read()       'loops through the records in the sqlReader.

    '            IsPersistedThr = True

    '            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

    '                SqlDrColName = SqlDr.GetName(i)

    '                Select Case SqlDrColName

    '                    Case "ThrId"
    '                        If Not SqlDr.IsDBNull(i) Then

    '                            ThrId = SqlDr.GetInt32(i)
    '                        End If

    '                    Case "EmployeeId"

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            EmployeeId = SqlDr.GetInt32(i)
    '                        End If


    '                    Case "ThrDateTo"

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            m_ThrDateTo = SqlDr.GetDateTime(i)
    '                            'Else
    '                            '    m_ThrDateTo = Date.MinValue
    '                        End If


    '                    Case "ThrTimeOut"
    '                        If Not SqlDr.IsDBNull(i) Then
    '                            m_ThrTimeOut = SqlDr.GetDateTime(i)
    '                        Else
    '                            m_ThrTimeOut = Date.MinValue
    '                        End If
    '                        'm_ThrTimeOut = SqlDr.GetDateTime(i)


    '                    Case "ThrChangedBy"
    '                        If Not SqlDr.IsDBNull(i) Then
    '                            m_ThrChangedBy = SqlDr.GetInt32(i)
    '                        Else
    '                            m_ThrChangedBy = 0
    '                        End If

    '                    Case "ThrChangedAt"
    '                        If Not SqlDr.IsDBNull(i) Then
    '                            m_ThrChangedAt = SqlDr.GetDateTime(i)
    '                        Else
    '                            m_ThrChangedAt = Date.MinValue
    '                        End If

    '                    Case "ThrStatus "
    '                        Try
    '                            If Not SqlDr.IsDBNull(i) Then
    '                                ThrStatus = CType(SqlDr.GetInt16(i), Thr.enumThrStatus)
    '                            End If
    '                        Catch ex As Exception
    '                            '???errormessage???
    '                        End Try
    '                End Select
    '            Next
    '        Loop
    '    End If
    'End Sub
    Friend Property IsPersistedThrs() As Boolean
        Get
            Return m_IsPersistedThrs
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedThrs = Value
        End Set
    End Property

    Friend Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strNull As String

        Select Case Me.IsPersistedThrs   'True means update existing row, False means insert row in SQL table 
            Case False
                strSqlCmd = "insert into Clock values (" & _
                            Me.ThrsEmployeeId.ToString & ",'" & _
                            Me.ThrDateTo.ToShortDateString & "'," & _
                            Me.ThrsQBHrs.ToString & "," & _
                            Me.ThrsQBMins.ToString & "," & _
                            Me.ThrsClockHrs.ToString & "," & _
                            Me.ThrsClockMins.ToString & "," & _
                            Me.ThrsClockRecCount.ToString & "," & _
                           Me.ThrsFixMins.ToString & _
                            ")"
                intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                If intSQLReturnValue < 0 Then
                    'NOP
                Else
                    Me.ThrsId = intSQLReturnValue
                End If
            Case Else
                strSqlCmd = "update TargetHrs set  " & _
                  "THrsPOSHrs=" & _
                    Me.m_ThrsClockHrs.ToString & "," & _
                  "ThrsPOSMins=" & _
                    Me.ThrsClockMins.ToString & "," & _
                    "ThrsClockRecCount='" & _
                    Me.ThrsClockRecCount.ToString & "'," & _
                    "ThrsFixMins=" & _
                    Me.ThrsFixMins.ToString & _
                    " where ThrsId =" & Me.ThrsId.ToString
                intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
        End Select
        'If Me.IsDirty Then


        intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
        If intSQLReturnValue < 0 Then
            'NOP
        Else
            m_isDirty = False
            IsPersistedThrs = True
        End If
        'End If
        Return intSQLReturnValue
    End Function




End Class



