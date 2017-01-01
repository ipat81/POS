Option Strict On
Imports System.Data.SqlClient


Public Class Session
    Inherits POSBO

    Public Enum enumSessionName
        BreakFast
        Lunch
        Dinner
        LateNight
        Day
    End Enum

    Public Enum enumSessionStatus
        Active
        InActive
    End Enum

    Private m_SessionId As Session.enumSessionName
    Private m_SessionName As String
    Private m_SessionFrom As String
    Private m_SessionTo As String
    Private m_SessionChangedBy As Integer
    Private m_SessionChangedAt As DateTime
    Private m_SessionStatus As enumSessionStatus


    Public Sub New()

    End Sub

    'Public Sub New(ByVal newsessionid As Integer)
    '    SessionId = newsessionid
    'End Sub


    Friend Sub New(ByVal newsessionid As Session.enumSessionName, ByVal newsessionname As String, _
                    ByVal newsessionfrom As String, ByVal newsessionto As String, _
                    ByVal newsessionchangedby As Integer, ByVal newsessionchangedat As DateTime, _
                    ByVal newsessionstatus As enumSessionStatus)

        SessionId = newsessionid
        SessionName = newsessionname
        SessionFrom = newsessionfrom
        SessionTo = newsessionto
        SessionChangedBy = newsessionchangedby
        SessionChangedAt = newsessionchangedat


    End Sub

    Public Property SessionId() As Session.enumSessionName
        Get
            Return m_SessionId
        End Get

        Set(ByVal Value As Session.enumSessionName)
            m_SessionId = Value
        End Set
    End Property

    Public Property SessionName() As String

        Get
            Return m_SessionName
        End Get

        Set(ByVal Value As String)
            m_SessionName = Value
        End Set

    End Property


    Public Property SessionFrom() As String

        Get
            Return m_SessionFrom
        End Get

        Set(ByVal Value As String)
            m_SessionFrom = Value
        End Set
    End Property


    Public Property SessionTo() As String

        Get
            Return m_SessionTo
        End Get

        Set(ByVal Value As String)
            m_SessionTo = Value
        End Set
    End Property


    Public Property SessionChangedBy() As Integer


        Get
            Return m_SessionChangedBy
        End Get

        Set(ByVal Value As Integer)
            m_SessionChangedBy = Value
        End Set
    End Property


    Public Property SessionChangedAt() As DateTime


        Get
            Return m_SessionChangedAt
        End Get

        Set(ByVal Value As DateTime)
            m_SessionChangedAt = Value
        End Set
    End Property

    Public Property SessionStatus() As enumSessionStatus
        Get
            Return m_SessionStatus
        End Get
        Set(ByVal Value As enumSessionStatus)
            m_SessionStatus = Value

        End Set
    End Property



    Public Sub GetData() '(ByVal enumSaleDayView As SaleDays.EnumView)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim SqlDrColName As String
        Dim objDataAccess As DataAccess

        Dim msg As String
        Dim i As Integer

        objDataAccess = DataAccess.CreateDataAccess

        sqlcommand = sqlcommand & " where  SaleDayId =  " & Me.SessionId

        'SaleDay record accessed from the database.GetData method connects to the database fetches  the record
        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then
            Do While SqlDr.Read()       'loops through the records in the SqlDr.


                For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                    SqlDrColName = SqlDr.GetName(i) 'Reading  the column Name

                    Select Case SqlDrColName


                        Case "SessionName"

                            If Not SqlDr.IsDBNull(i) Then
                                SessionName = SqlDr.GetString(i)
                            End If

                        Case "SessionFrom"
                            If Not SqlDr.IsDBNull(i) Then
                                SessionName = SqlDr.GetString(i)
                            End If

                        Case "SessionTo"
                            If Not SqlDr.IsDBNull(i) Then
                                SessionTo = SqlDr.GetString(i)
                            End If



                        Case "SessionChangedBy"
                            If Not SqlDr.IsDBNull(i) Then
                                SessionChangedBy = SqlDr.GetInt32(i)
                            Else
                                SessionChangedBy = 0
                            End If

                        Case "SessionChangedAt"
                            If Not SqlDr.IsDBNull(i) Then
                                SessionChangedAt = SqlDr.GetDateTime(i)
                            Else
                                SessionChangedAt = Date.MinValue
                            End If

                        Case "SessionStatus "
                            Try
                                If Not SqlDr.IsDBNull(i) Then
                                    SessionStatus = CType(SqlDr.GetInt16(i), Session.enumSessionStatus)

                                End If
                            Catch ex As Exception
                                '???errormessage???
                            End Try
                    End Select

                Next
            Loop
        End If

    End Sub



End Class

