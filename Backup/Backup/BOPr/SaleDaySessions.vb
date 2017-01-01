Option Strict On
Imports System.Data.SqlClient
Public Class SaleDaySessions
    Inherits POSBOs
    Private m_CurrentSession As SaleDaySession
    Private m_SaleDate As Date

    Public Event SaleDaySessionClosed(ByVal enumSession As Session.enumSessionName)
    Public Event SaleDaySessionStarted(ByVal enumSession As Session.enumSessionName)

    Public Enum EnumView As Integer  'This enum determines the select clause
        BaseView
        CompleteView
    End Enum

    Public Enum EnumFilter As Integer  'This enum determines the where condition
        Open
        Closed
        All
    End Enum

    Public Enum EnumSessionFilter As Integer  'This enum determines the where condition
        BreakFast
        Lunch
        Dinner
        LateNight
        All
    End Enum

    Public Sub New(ByVal newSaleDate As Date, _
                    ByVal enumSessionfilter As EnumSessionFilter, _
                    ByVal enumSessionview As EnumView)
        MyBase.New()
        GetHistoricalSDSessions(newSaleDate, enumSessionfilter, enumSessionview)

    End Sub
    Private Sub GetHistoricalSDSessions(ByVal newSaleDate As Date, _
                    ByVal enumSessionfilter As EnumSessionFilter, _
                    ByVal enumSessionview As EnumView)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim objSaleDaySession As SaleDaySession
        Dim msg As String

        m_SaleDate = newSaleDate.Date
        Select Case enumSessionview

            Case EnumView.BaseView
                sqlcommand = "Select SessionId,SessionName From Session"

            Case EnumView.CompleteView
                sqlcommand = "Select * from SaleDaySession"

        End Select
        sqlcommand = sqlcommand & " where SaleDate = '" & newSaleDate.ToShortDateString & "'"


        Select Case enumSessionfilter
            Case enumSessionfilter.All
                'nop
            Case Else
                sqlcommand = sqlcommand & " and SessionId = " & CStr(enumSessionfilter)
        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objSaleDaySession = New SaleDaySession()
                Me.Add(objSaleDaySession)
                objSaleDaySession.loadData(SqlDr)
                If objSaleDaySession.IsSessionOpen = True Then m_CurrentSession = objSaleDaySession
            Loop
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Public ReadOnly Property SaleDate() As Date
        Get
            Return m_SaleDate
        End Get
    End Property

    Public ReadOnly Property Item(ByVal index As Integer) As SaleDaySession
        Get
            Return CType(List(index), SaleDaySession)
        End Get

    End Property

    'Public ReadOnly Property Item(ByVal objSession As Session) As SaleDaySession
    '    Get
    '        Dim objSaleDaySession As SaleDaySession
    '        Dim objPOSBO As POSBO
    '        For Each objPOSBO In MyBase.InnerList
    '            If CType(objPOSBO, SaleDaySession).SessionId = objSession.SessionId Then
    '                Exit For
    '            End If
    '        Next
    '        Return CType(objPOSBO, SaleDaySession)
    '    End Get
    'End Property

    Public ReadOnly Property Item(ByVal enumSessionId As Session.enumSessionName) As SaleDaySession
        Get
            Dim intIndex As Integer
            Dim sessionFilter As EnumSessionFilter
            intIndex = Me.Indexof(enumSessionId)

            If intIndex < 0 Then
                Select Case enumSessionId
                    Case Session.enumSessionName.Lunch
                        sessionFilter = EnumSessionFilter.Lunch
                    Case Session.enumSessionName.Dinner
                        sessionFilter = EnumSessionFilter.Dinner
                    Case Else
                        sessionFilter = EnumSessionFilter.All
                End Select
                GetHistoricalSDSessions(Me.SaleDate, sessionFilter, EnumView.CompleteView)
                intIndex = Me.Indexof(enumSessionId)
                If intIndex < 0 Then
                    AddNewSDSession(enumSessionId)
                    intIndex = Me.Indexof(enumSessionId)
                End If
            End If

            Return Me.Item(intIndex)
        End Get
    End Property

    Public Function AddNewSDSession(ByVal enumSessionId As Session.enumSessionName) As SaleDaySession
        Dim objSDSession As SaleDaySession

        If Me.Contains(enumSessionId) Then
            'error
        Else
            objSDSession = New SaleDaySession()
            With objSDSession
                .SessionId = enumSessionId
                .SaleDate = Me.SaleDate
            End With
            Me.Add(objSDSession)
        End If
        Return objSDSession
    End Function

    Friend Sub Add(ByRef value As SaleDaySession)
        MyBase.addPOSBO(CType(value, POSBO))
    End Sub

    Public ReadOnly Property Indexof(ByVal intSessionId As Session.enumSessionName) As Integer
        Get
            Dim objSDSession As SaleDaySession
            Dim intIndex As Integer

            intIndex = -1
            For Each objSDSession In MyBase.List
                If (objSDSession.SaleDate = SaleDate) AndAlso (objSDSession.SessionId = intSessionId) Then
                    intIndex = MyBase.List.IndexOf(objSDSession)
                    Exit For
                End If
            Next
            Return intIndex
        End Get
    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim mySessionIds() As Integer
            Return mySessionIds
        End Get
    End Property


    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function Contains(ByVal value As Session) As Boolean

        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

    Public Function Contains(ByVal value As Session.enumSessionName) As Boolean
        Dim objPOSBO As POSBO
        For Each objPOSBO In MyBase.InnerList
            If CType(objPOSBO, SaleDaySession).SessionId = value Then
                Return True
                Exit Function
            End If
        Next
    End Function

    Public Sub New()

    End Sub

    'Public Sub StartASaleDaySession(ByVal enumNewSession As Session.enumSessionName)
    '    Dim objSaleDaySession As SaleDaySession

    '    If Me.Contains(enumNewSession) Then
    '        objSaleDaySession = Me.Item(enumNewSession)
    '    Else
    '        objSaleDaySession = AddNewSDSession(enumNewSession)
    '        Me.Add(objSaleDaySession)
    '    End If
    '    With objSaleDaySession
    '        .SaleDaySessionFrom = SaleDays.CreateSaleDay.Now
    '        .SaleDaySessionStatus = SaleDaySession.enumSaleDaySessionStatus.Open
    '    End With
    '    If objSaleDaySession.SetData() > 0 Then
    '        m_CurrentSession = objSaleDaySession
    '        RaiseEvent SaleDaySessionStarted(enumNewSession)
    '    End If
    'End Sub

    'Public Sub CloseCurrentSaleDaySession()
    '    Dim objSaleDaySession As SaleDaySession

    '    If CurrentSession Is Nothing Then
    '        'error 
    '    Else
    '        With CurrentSession
    '            .SaleDaySessionTo = SaleDays.CreateSaleDay.Now
    '            .SaleDaySessionStatus = SaleDaySession.enumSaleDaySessionStatus.Closed
    '        End With
    '    End If
    '    'if objSaleDaySession.SetData > 0 then
    '    m_CurrentSession = Nothing
    '    RaiseEvent SaleDaySessionClosed(CurrentSession.SessionId)
    '    'end if
    'End Sub

    'Public ReadOnly Property CurrentSession() As SaleDaySession
    '    Get
    '        Return m_CurrentSession
    '    End Get
    'End Property
    'Public Function SetData(ByVal objSaleDaySession) As Integer
    '    Dim objSaleDaySession As SaleDaySession
    '    Dim intSQLReturnValue As Integer

    '    For Each objSaleDaySession In MyBase.List
    '        intSQLReturnValue = objSaleDaySession.SetData()
    '        If intSQLReturnValue < 0 Then Exit For
    '    Next

    '    Return intSQLReturnValue
    'End Function
End Class

