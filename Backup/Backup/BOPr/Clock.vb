Option Strict On
Imports System.Data.SqlClient
'This is For Princeton
Imports System.Globalization
Public Class Clock
    Inherits POSBO
    Public Event OverrideNeeded(ByVal objClockOverride As Override)
    Public Enum enumDEError As Integer
        NoError
        TimeInAfterTimeOut
    End Enum

    Public Enum enumClockStatus
        Active
        OverriddenOld
        OverriddenNew
        Voided
    End Enum

    Public Enum enumClockOverride
        ClockIn
        ClockOut
    End Enum

    Public Enum enumClockInOut
        ClockIn
        ClockOut
        InValid
    End Enum
    Public Enum enumClockOverrideReasons
        ForgotClockIn
        ForgotClockOut
        ForgotClockinOut
        ComputerDown
        ForgotPassword
        WorkedOffsite

    End Enum


    Private m_ClockId As Integer
    Private m_EmployeeId As Integer
    Private m_ClockTimeIn As DateTime
    Private m_ClockTimeOut As DateTime
    Private m_ClockSaleDate As DateTime
    Private m_ClockChangedBy As Integer
    Private m_ClockChangedAt As DateTime
    Private m_isDirty As Boolean
    Private m_ClockStatus As enumClockStatus
    Private m_objEmployee As Employee
    Private m_objSaleDay As SaleDay

    Private m_IsPersistedClock As Boolean
    Private m_objOverrideForClockEdit As Override

    Public Sub New()
        ClockId = 0
        'ClockTimeIn = DateTime.MinValue
        'ClockTimeOut = System.DBNull.Value
    End Sub

    Friend Sub New(ByVal newclockid As Integer, ByVal newemployeeid As Integer, _
                    ByVal newclocktimein As DateTime, ByVal newclocktimeout As DateTime, _
                    ByVal newclocksaledate As DateTime, ByVal newclockchangedby As Integer, _
                    ByVal newclockchangedat As DateTime, ByVal newclockstatus As enumClockStatus)

        m_ClockId = newclockid
        m_EmployeeId = newemployeeid
        m_ClockTimeIn = newclocktimein
        m_ClockTimeOut = newclocktimeout
        m_ClockSaleDate = newclocksaledate
        m_ClockChangedBy = newclockchangedby
        m_ClockChangedAt = newclockchangedat
        m_ClockStatus = newclockstatus

        IsPersistedClock = True
    End Sub

    Public Property ClockId() As Integer

        Get
            Return m_ClockId
        End Get

        Set(ByVal Value As Integer)
            m_ClockId = Value
        End Set
    End Property


    Public Property EmployeeId() As Integer

        Get
            Return m_EmployeeId
        End Get

        Set(ByVal Value As Integer)
            m_EmployeeId = Value
            MarkDirty()
        End Set
    End Property


    Public Property ClockTimeIn() As DateTime
        Get
            Return m_ClockTimeIn
        End Get
        Set(ByVal newClockTimeIn As DateTime)
            m_ClockTimeIn = newClockTimeIn
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property ClockWeek() As Integer
        Get
            Dim obj As New GregorianCalendar()
            Return obj.GetWeekOfYear(Me.ClockTimeIn, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
        End Get
    End Property

    Public Property ClockTimeOut() As DateTime
        Get
            Return m_ClockTimeOut
        End Get
        Set(ByVal newClockTimeOut As DateTime)
            m_ClockTimeOut = newClockTimeOut
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property TimeWorked() As TimeSpan
        Get
            Select Case Me.ClockStatus
                Case enumClockStatus.Active, enumClockStatus.OverriddenNew
                    If (ClockTimeIn > DateTime.MinValue) AndAlso _
                                    (ClockTimeOut > DateTime.MinValue) Then 'AndAlso _
                        '(ClockTimeIn.Date = ClockTimeOut.Date) Then
                        Return ClockTimeOut.Subtract(ClockTimeIn)
                    Else
                        Return TimeSpan.Zero
                    End If
                Case Else
                    Return TimeSpan.Zero
            End Select
        End Get
    End Property

    Public ReadOnly Property TimeWorkedDisplay() As String
        Get
            Dim intHrs As Integer
            Dim intMins As Integer
            Dim strTimeWorked As String
            If Me.TimeWorked.TotalHours() = 0 Then
                strTimeWorked = String.Empty
            Else
                intHrs = CInt(Math.Floor(Me.TimeWorked.TotalHours()))
                intMins = CInt(Math.Floor(Me.TimeWorked.TotalMinutes - (intHrs * 60)))
                strTimeWorked = Format(intHrs, "00") & ":" & Format(intMins, "00")
            End If
            Return strTimeWorked
        End Get
    End Property

    Public ReadOnly Property DEError() As enumDEError
        Get
            If (ClockTimeIn > DateTime.MinValue) AndAlso (ClockTimeOut > DateTime.MinValue) AndAlso _
                (ClockTimeOut < ClockTimeIn) Then
                Return enumDEError.TimeInAfterTimeOut
            Else
                Return enumDEError.NoError
            End If
        End Get
    End Property


    Public Property ClockSaleDate() As DateTime

        Get
            Return m_ClockSaleDate
        End Get

        Set(ByVal Value As DateTime)
            m_ClockSaleDate = Value
        End Set
    End Property



    Public ReadOnly Property ClockChangedBy() As Integer

        Get
            Return m_ClockChangedBy
        End Get

    End Property

    Public ReadOnly Property ClockChangedAt() As DateTime

        Get
            Return m_ClockChangedAt
        End Get

    End Property

    Public Property ClockStatus() As enumClockStatus
        Get
            Return m_ClockStatus
        End Get
        Set(ByVal Value As enumClockStatus)
            m_ClockStatus = Value
            MarkDirty()
        End Set
    End Property

    Public Property OBJEmployee() As Employee
        Get
            Return m_objEmployee
        End Get

        Set(ByVal Value As Employee)
            m_objEmployee = Value
        End Set
    End Property

    Public Property OBJSaleDay() As SaleDay
        Get
            Return m_objSaleDay
        End Get

        Set(ByVal Value As SaleDay)
            m_objSaleDay = Value
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

    Public Property ManagerOverride() As Override
        Get
            'Return m_Override
        End Get
        Set(ByVal value As Override)
            'm_Override = value
        End Set
    End Property

    Public Sub GetData()
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim SqlDrColName As String
        Dim objDataAccess As DataAccess

        Dim msg As String
        Dim i As Integer

        objDataAccess = DataAccess.CreateDataAccess

        'Select Case enumClockView
        '    Case Clocks.EnumView.BaseView
        '        sqlcommand = "Select ClockId,ClockId,ClockTimeIn From Clock"

        '    Case Clocks.EnumView.CompleteView
        '        sqlcommand = "Select * from Clock"

        'End Select


        sqlcommand = "Select * from Clock" & " where  ClockId =  " & Me.ClockId

        'Employee record accessed from the database.GetData method connects to the database fetches  the record
        SqlDr = objDataAccess.GetData(sqlcommand)
        If Not SqlDr Is Nothing Then


            Do While SqlDr.Read()       'loops through the records in the sqlReader.

                IsPersistedClock = True

                For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                    SqlDrColName = SqlDr.GetName(i)

                    Select Case SqlDrColName

                        Case "ClockId"
                            If Not SqlDr.IsDBNull(i) Then

                                ClockId = SqlDr.GetInt32(i)
                            End If

                        Case "EmployeeId"

                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeId = SqlDr.GetInt32(i)
                            End If


                        Case "ClockTimeIn"

                            If Not SqlDr.IsDBNull(i) Then
                                m_ClockTimeIn = SqlDr.GetDateTime(i)
                                'Else
                                '    m_ClockTimeIn = Date.MinValue
                            End If


                        Case "ClockTimeOut"
                            If Not SqlDr.IsDBNull(i) Then
                                m_ClockTimeOut = SqlDr.GetDateTime(i)
                            Else
                                m_ClockTimeOut = Date.MinValue
                            End If
                            'm_ClockTimeOut = SqlDr.GetDateTime(i)


                        Case "ClockChangedBy"
                            If Not SqlDr.IsDBNull(i) Then
                                m_ClockChangedBy = SqlDr.GetInt32(i)
                            Else
                                m_ClockChangedBy = 0
                            End If

                        Case "ClockChangedAt"
                            If Not SqlDr.IsDBNull(i) Then
                                m_ClockChangedAt = SqlDr.GetDateTime(i)
                            Else
                                m_ClockChangedAt = Date.MinValue
                            End If

                        Case "ClockStatus "
                            Try
                                If Not SqlDr.IsDBNull(i) Then
                                    ClockStatus = CType(SqlDr.GetInt16(i), Clock.enumClockStatus)
                                End If
                            Catch ex As Exception
                                '???errormessage???
                            End Try
                    End Select
                Next
            Loop
        End If
    End Sub
    Friend Property IsPersistedClock() As Boolean
        Get
            Return m_IsPersistedClock
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedClock = Value
        End Set
    End Property
    Public ReadOnly Property ClockInOut() As enumClockInOut
        Get
            If (Me.ClockTimeOut <= Date.MinValue) And (Me.ClockTimeIn > Date.MinValue) Then
                Return enumClockInOut.ClockOut
            ElseIf (Me.ClockTimeOut <= Date.MinValue) And (Me.ClockTimeIn <= Date.MinValue) Then
                Return enumClockInOut.ClockIn
            Else
                Return enumClockInOut.InValid
            End If

        End Get

    End Property

    Friend Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strNull As String
        If Me.ClockTimeOut > DateTime.MinValue Then
            strNull = "'" & Me.ClockTimeOut.ToString & "'"
        Else
            strNull = "NULL"
        End If

        If Me.IsDirty Then
            Select Case Me.IsPersistedClock     'True means update existing row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into Clock values (" & _
                                Me.EmployeeId.ToString & ",'" & _
                                Me.ClockTimeIn & "'," & _
                                strNull & ",'" & _
                                Me.ClockSaleDate.ToShortDateString & "'," & _
                               Me.ClockChangedBy.ToString & ",'" & _
                                Me.ClockChangedAt & "'," & _
                                CStr(Me.ClockStatus) & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.ClockId() = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update Clock set  " & _
                              "ClockTimeIn=" & _
                                "'" & Me.ClockTimeIn.ToString & "'" & "," & _
                              "ClockTimeOut=" & _
                                strNull & "," & _
                                "ClockChangedBy=" & _
                                Me.ClockChangedBy.ToString & "," & _
                                "ClockChangedAt='" & _
                                Me.ClockChangedAt & "'," & _
                                "ClockStatus=" & _
                                CStr(Me.ClockStatus) & _
                                " where ClockId =" & Me.ClockId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
            If intSQLReturnValue < 0 Then
                'NOP
            Else
                m_isDirty = False
                IsPersistedClock = True
            End If
        End If
        Return intSQLReturnValue
    End Function

    Private Sub MarkDirty()
        If IsDirty Then
            'NOP
        Else
            SetClockChangedAt()
            SetClockChangedBy()
            m_isDirty = True
        End If
    End Sub
    Private Sub SetClockChangedAt()
        m_ClockChangedAt = SaleDays.CreateSaleDay.Now
    End Sub

    Private Sub SetClockChangedBy()
        m_ClockChangedBy = Me.EmployeeId
    End Sub

    Public ReadOnly Property AllowClockEdit() As Boolean
        Get
            SetOverrideForClockEdit()
            With m_objOverrideForClockEdit
                If .OverrideAvailable = True Then
                    Return True
                Else
                    Return False
                End If
            End With
        End Get
    End Property

    Private Sub SetOverrideForClockEdit()
        If (m_objOverrideForClockEdit Is Nothing) OrElse _
            (m_objOverrideForClockEdit.OverrideAvailable = False) Then
            m_objOverrideForClockEdit = OverrideForClockEdit
            With m_objOverrideForClockEdit
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(m_objOverrideForClockEdit)
                        'nop here: caller will do the actual work
                    Case Else
                        'nop
                End Select
            End With
        Else
            'Use an existing override to avoid repetitive requests for override
        End If
    End Sub

    Private ReadOnly Property OverrideForClockEdit() As Override
        Get
            Dim objSaleDay As SaleDay
            Dim objSDSession As SaleDaySession

            m_objOverrideForClockEdit = New Override()
            With m_objOverrideForClockEdit
                .OverrideType = Override.enumOverrideType.ClockEdit
                .OverrideOldRowId = Me.ClockId
                .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
            End With

            Return m_objOverrideForClockEdit
        End Get
    End Property
End Class

