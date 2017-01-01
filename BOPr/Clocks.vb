Option Strict On
Imports System.Data.SqlClient


Public Class Clocks
    Inherits POSBOs
    Private m_CurrentClock As Clock
    Private m_LatestClock As Clock
    Private m_ClockCount As Integer


    '______________________________ 
    Public Enum EnumView As Integer  'This enum determines the select clause

        CompleteView

    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Normal
        Overridden
    End Enum

    Public Sub New()

    End Sub

    Public Sub New(ByVal DateFrom As Date, ByVal DateTo As Date)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        'Dim msg As String
        Dim PayrollStartDate As Date = CDate("12/8/2003")
        Dim dteToday As Date
        Dim dteLoadFrom As Date
        Dim intDays As Integer

        dteToday = SaleDays.CreateSaleDay.Now.Date
        intDays = dteToday.Subtract(PayrollStartDate).Days
        intDays = CInt(Math.Floor((intDays + 1) / 14) * 14)
        dteLoadFrom = PayrollStartDate.AddDays(intDays)
        sqlcommand = "Select * from Clock, Employee where " & _
                             "(ClockSaleDate BETWEEN '" & DateFrom.ToShortDateString & "'" & _
                     " And '" & DateTo.ToShortDateString & "') and (EmployeeRoleId < 2) and (Clock.EmployeeId=Employee.EmployeeId)" _
                                 & " Order by Clock.EmployeeId, ClockId, ClockSaleDate"

        sqlcommand = "Select * from Clock where " & _
                     "(ClockSaleDate BETWEEN '" & DateFrom.ToShortDateString & "'" & _
                     " And '" & DateTo.ToShortDateString & "')" _
        & " Order by EmployeeId, ClockId, ClockSaleDate"

        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            loadCollection(SqlDr)       'then empty Clocks object is returned
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Public Sub New(ByVal empId As Integer)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        'Dim msg As String
        Dim PayrollStartDate As Date = CDate("12/8/2003")
        Dim dteToday As Date
        Dim dteLoadFrom As Date
        Dim intDays As Integer

        dteToday = SaleDays.CreateSaleDay.Now.Date
        intDays = dteToday.Subtract(PayrollStartDate).Days
        intDays = CInt(Math.Floor((intDays) / 14) * 14)
        dteLoadFrom = PayrollStartDate.AddDays(intDays)
        sqlcommand = "Select * from Clock where EmployeeId = " & empId.ToString & _
                             " And ClockSaleDate BETWEEN '" & dteLoadFrom.ToShortDateString & "'" & _
                             " And '" & dteToday.ToShortDateString & "'" _
                             & " Order by ClockId DESC"

        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            loadCollection(SqlDr)       'then empty Clocks object is returned
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)
        'Dim objClock As Clock
        Dim ClockId As Int32
        Dim EmployeeId As Int32
        Dim ClockTimeIn As DateTime
        Dim ClockTimeOut As DateTime
        Dim ClockSaleDate As DateTime
        Dim ClockChangedBy As Integer
        Dim ClockChangedAt As DateTime
        Dim ClockStatus As Clock.enumClockStatus


        Dim SqlDrColName As String
        Dim i As Integer
        'Dim msg As String

        Do While SqlDr.Read()       'loops through the records in the sqlReader.


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


                    Case "ClockSaleDate"

                        If Not SqlDr.IsDBNull(i) Then
                            ClockSaleDate = SqlDr.GetDateTime(i)
                        Else
                            ClockSaleDate = Date.MinValue
                        End If
                    Case "ClockTimeIn"

                        If Not SqlDr.IsDBNull(i) Then
                            ClockTimeIn = SqlDr.GetDateTime(i)
                        Else
                            ClockTimeIn = Date.MinValue
                        End If


                    Case "ClockTimeOut"
                        If Not SqlDr.IsDBNull(i) Then
                            ClockTimeOut = SqlDr.GetDateTime(i)
                        Else
                            ClockTimeOut = Date.MinValue
                        End If


                    Case "ClockChangedBy"
                        If Not SqlDr.IsDBNull(i) Then
                            ClockChangedBy = SqlDr.GetInt32(i)
                        Else
                            ClockChangedBy = 0
                        End If

                    Case "ClockChangedAt"
                        If Not SqlDr.IsDBNull(i) Then
                            ClockChangedAt = SqlDr.GetDateTime(i)
                        Else
                            ClockChangedAt = Date.MinValue
                        End If

                    Case "ClockStatus"
                        Try
                            If Not SqlDr.IsDBNull(i) Then
                                ClockStatus = CType(SqlDr.GetInt32(i), Clock.enumClockStatus)
                            End If
                        Catch ex As Exception
                            '???errormessage???
                        End Try


                End Select


            Next

            Me.Add(New Clock(ClockId, EmployeeId, ClockTimeIn, ClockTimeOut, ClockSaleDate, _
                             ClockChangedBy, ClockChangedAt, ClockStatus))


        Loop
    End Sub



    Public ReadOnly Property Item(ByVal index As Integer) As Clock
        Get
            Return CType(MyBase.itemPOSBO(index), Clock)
        End Get

    End Property

    Public ReadOnly Property IndexOf(ByVal objClock As Clock) As Integer
        Get
            Return MyBase.List.IndexOf(objClock)
        End Get
    End Property


    Public ReadOnly Property SelectedItem(ByVal index As Integer) As Clock
        Get

            Return (Me.Item(index))

        End Get
    End Property



    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myClockIds() As Integer

            Return myClockIds

        End Get
    End Property

    Private Sub Add(ByRef value As Clock)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub

    Public Function AddClock() As Clock

        m_CurrentClock = New Clock()
        Me.Add(m_CurrentClock)
        Return m_CurrentClock
    End Function

    Public ReadOnly Property CurrentClock() As Clock
        Get
            Return m_CurrentClock
        End Get

    End Property

    Public ReadOnly Property LatestClock() As Clock

        Get
            Dim i As Integer
            Dim objLatestClock As Clock
            If Me.Count = 0 Then
                'nop
            Else
                For i = Math.Max(Me.Count - 1, 0) To 0 Step -1
                    With Me.Item(i)
                        If ((.ClockStatus = Clock.enumClockStatus.Active) OrElse (.ClockStatus = Clock.enumClockStatus.OverriddenNew)) And _
                            (.ClockTimeOut <= DateTime.MinValue) And (DateTime.Now.Subtract(.ClockTimeIn).TotalHours < 17) Then
                            objLatestClock = Me.Item(i)
                            Exit For
                        End If
                    End With
                Next
            End If
            If objLatestClock Is Nothing Then objLatestClock = Me.AddClock
            Return objLatestClock
        End Get
    End Property


    'Public ReadOnly Property ClockCount() As Integer
    '    Get
    '        Dim intItemCount As Integer
    '        Dim objClock As Clock

    '        For Each objClock In Me

    '            intItemCount += 1
    '        Next
    '        m_ClockCount = intItemCount
    '        Return m_ClockCount
    '    End Get
    'End Property

    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function currentIndex(ByRef value As Clock) As Integer
        Dim objClock As Clock
        Dim intIndex As Integer

        For Each objClock In Me
            intIndex += 1
            If objClock Is value Then
                currentIndex = intIndex
            End If
        Next
        Return currentIndex
    End Function

    Public Function Contains(ByVal value As Clock) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

    Public Function SetData() As Integer
        Dim ObjClock As Clock
        Dim objnewClock As Clock
        Dim objOldClock As Clock
        Dim intSQLReturnValue As Integer

        DataAccess.CreateDataAccess.StartTxn()

        For Each ObjClock In Me
            intSQLReturnValue = ObjClock.SetData()
            If intSQLReturnValue < 0 Then Exit For
            'Find the old and new clock
            Select Case ObjClock.ClockStatus
                Case Clock.enumClockStatus.OverriddenOld
                    objOldClock = ObjClock
                Case Clock.enumClockStatus.OverriddenNew
                    objnewClock = ObjClock
                Case Else
                    'nop
            End Select
        Next
        'insert Override if present

        'Select Case intSQLReturnValue
        '    Case Is > 0
        '        If (objOldClock Is Nothing) Then
        '            'NOP: not an override case
        '        Else
        '            objOldClock.ManagerOverride.OverrideNewRowId = objnewClock.ClockId
        '            intSQLReturnValue = objOldClock.ManagerOverride.SetData()
        '        End If

        '    Case Else
        '        'NOP
        'End Select

        'check final return code for sql
        Select Case intSQLReturnValue
            Case Is >= 0
                DataAccess.CreateDataAccess.EndTxn()
                For Each ObjClock In MyBase.List
                    With ObjClock
                        .IsDirty = False
                        .IsPersistedClock = True
                        If .ManagerOverride Is Nothing Then
                            'NOP
                        Else
                            .ManagerOverride.IsDirty = False
                        End If
                    End With
                Next
            Case Else
                'NOP 
        End Select

        Return intSQLReturnValue
    End Function

End Class

