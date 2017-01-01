Option Strict On
Imports System.Data.SqlClient


Public Class Sessions
    Inherits POSBOs
    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView
    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition

        Active
        InActive
        All

    End Enum
    Public Enum enumSessionFilter As Integer
        WholeDay
        Lunch
        Dinner
    End Enum

    Public Sub New(ByVal enumSessionfilter As EnumFilter, ByVal enumSessionview As EnumView)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess

        Dim msg As String


        Select Case enumSessionview

            Case EnumView.BaseView
                sqlcommand = "Select SessionId,SessionName From Session"

            Case EnumView.CompleteView
                sqlcommand = "Select * from Session"

        End Select


        Select Case enumSessionfilter


            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  SessionStatus =  " & Session.enumSessionStatus.InActive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where SessionStatus = " & Session.enumSessionStatus.Active

            Case EnumFilter.All


        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            loadCollection(SqlDr)       'then empty Session object is returned

        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()

    End Sub

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)

        Dim SessionId As Session.enumSessionName
        Dim SessionName As String
        Dim SessionFrom As String
        Dim SessionTo As String
        Dim SessionChangedBy As Integer
        Dim SessionChangedAt As DateTime
        Dim SessionStatus As Session.enumSessionStatus

        Dim msg As String
        Dim i As Integer
        Dim SqlDrColName As String



        Do While SqlDr.Read()       'loops through the records in the SqlDr.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "SessionId"

                        If Not SqlDr.IsDBNull(i) Then
                            SessionId = CType(SqlDr.GetInt32(i), Session.enumSessionName)
                        End If



                    Case "SessionName"
                        If Not SqlDr.IsDBNull(i) Then
                            SessionName = SqlDr.GetString(i)
                        End If

                    Case "SessionFrom"
                        If Not SqlDr.IsDBNull(i) Then
                            SessionFrom = SqlDr.GetString(i)
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
                                SessionStatus = CType(SqlDr.GetInt32(i), Session.enumSessionStatus)
                            Else
                                SessionStatus = Session.enumSessionStatus.Active
                            End If
                        Catch ex As Exception
                            '???errormessage???
                        End Try

                End Select


            Next

            'Session records to the collection
            Me.Add(New Session(SessionId, SessionName, SessionFrom, SessionTo, _
                               SessionChangedBy, SessionChangedAt, SessionStatus))


        Loop
    End Sub

    Public ReadOnly Property Item(ByVal index As Integer) As Session
        Get
            Return CType(List(index - 1), Session)
        End Get

    End Property
    Public ReadOnly Property Item(ByVal strsessionName As String) As Session
        Get
            Dim objSession As Session
            Dim objPOSBO As POSBO
            For Each objPOSBO In MyBase.InnerList
                If CType(objPOSBO, Session).SessionName = strsessionName Then
                    Return CType(objPOSBO, Session)
                    Exit Property
                End If
            Next
        End Get
    End Property


    Public Sub Add(ByRef value As Session)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub
    Public ReadOnly Property Indexof(ByVal enumSessionId As Session.enumSessionName) As Integer
        Get
            Dim objSession As Session
            Dim intIndex As Integer = -1

            For Each objSession In Me
                If objSession.SessionId = enumSessionId Then
                    intIndex = MyBase.List.IndexOf(objSession)
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


End Class


