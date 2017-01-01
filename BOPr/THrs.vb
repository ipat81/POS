Option Strict On
Imports System.Data.SqlClient


Public Class THrs

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

        Dim msg As String

        sqlcommand = "Select * from TargetHrs where " & _
                     "(THrsDateTo ='" & _
                      DateTo.ToShortDateString & "') " & _
                         " Order by THrsEmployeeId, ThrsId"
        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            loadCollection(SqlDr)       'then empty Clocks object is returned
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)
        Dim objTHr As Thr
        Dim THrsId As Int32
        Dim THrsEmployeeId As Int32
        Dim THrsDateTo As DateTime
        Dim THrsQBHrs As Int32
        Dim THrsQBMins As Int32
        Dim THrsPOSHrs As Int32
        Dim THrsPOSMins As Int32
        Dim THrsClockRecCount As Int32
        Dim THrsFixMins As Int32

        Dim SqlDrColName As String
        Dim i As Integer
        Dim msg As String

        Do While SqlDr.Read()       'loops through the records in the sqlReader.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "ThrsId"
                        If Not SqlDr.IsDBNull(i) Then

                            THrsId = SqlDr.GetInt32(i)
                        End If

                    Case "ThrsEmployeeId"

                        If Not SqlDr.IsDBNull(i) Then
                            THrsEmployeeId = SqlDr.GetInt32(i)
                        End If


                    Case "ThrsDateTo"

                        If Not SqlDr.IsDBNull(i) Then
                            THrsDateTo = SqlDr.GetDateTime(i)
                        Else
                            THrsDateTo = Date.MinValue
                        End If
                    Case "THrsQBHrs"

                        If Not SqlDr.IsDBNull(i) Then

                            THrsQBHrs = SqlDr.GetInt32(i)
                        End If


                    Case "ThrsQBMins"

                        If Not SqlDr.IsDBNull(i) Then

                            THrsQBMins = SqlDr.GetInt32(i)
                        End If


                    Case "THrsPOSHrs"
                        If Not SqlDr.IsDBNull(i) Then
                            THrsPOSHrs = SqlDr.GetInt32(i)
                        Else
                            THrsPOSHrs = 0
                        End If

                    Case "ThrsPOSMins"
                        If Not SqlDr.IsDBNull(i) Then
                            THrsPOSMins = SqlDr.GetInt32(i)
                        Else
                            THrsPOSMins = 0
                        End If

                    Case "ThrsClockRecCount"
                        If Not SqlDr.IsDBNull(i) Then
                            THrsClockRecCount = SqlDr.GetInt32(i)
                        Else
                            THrsClockRecCount = 0
                        End If
                    Case "ThrsFixMins"
                        If Not SqlDr.IsDBNull(i) Then
                            THrsFixMins = SqlDr.GetInt32(i)
                        Else
                            THrsFixMins = 0
                        End If

                End Select


            Next

            Me.Add(New Thr(THrsId, THrsEmployeeId, THrsDateTo, THrsQBHrs, THrsQBMins, _
            THrsPOSHrs, THrsPOSMins, THrsClockRecCount, THrsFixMins))


        Loop
    End Sub



    Public ReadOnly Property Item(ByVal index As Integer) As Thr
        Get
            Return CType(MyBase.itemPOSBO(index), Thr)
        End Get

    End Property

    Public ReadOnly Property IndexOf(ByVal objThr As Thr) As Integer
        Get
            Return MyBase.List.IndexOf(objThr)
        End Get
    End Property


    Public ReadOnly Property SelectedItem(ByVal index As Integer) As Thr
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

    Private Sub Add(ByRef value As Thr)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub

    'Public Function AddThr() As Thr

    '    m_CurrentThr = New Thr()
    '    Me.Add(m_CurrentThr)
    '    Return m_CurrentThr
    'End Function

    'Public ReadOnly Property CurrentThr() As Thr
    '    Get
    '        Return m_CurrentThr
    '    End Get

    'End Property





    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function currentIndex(ByRef value As Thr) As Integer
        Dim objThr As Thr
        Dim intIndex As Integer

        For Each objThr In Me
            intIndex += 1
            If objThr Is value Then
                currentIndex = intIndex

            End If
        Next
        Return currentIndex
    End Function

    Public Function Contains(ByVal value As Thr) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

    Public Function EmployeeIndex(ByVal EmployeeId As Integer) As Integer
        Dim objThr As Thr
        Dim intIndex As Integer

        For Each objThr In Me
            'If objThr.ThrsEmployeeId = EmployeeId Then Return Me.IndexOf(Me)
        Next
        Return -1
    End Function

    Public Function SetData() As Integer
        Dim ObjTHr As Thr
        Dim objnewTHr As Thr
        Dim objOldTHr As Thr
        Dim intSQLReturnValue As Integer

        DataAccess.CreateDataAccess.StartTxn()

        For Each ObjTHr In Me
            intSQLReturnValue = ObjTHr.SetData()
            If intSQLReturnValue < 0 Then Exit For

        Next


        'check final return code for sql
        Select Case intSQLReturnValue
            Case Is >= 0
                DataAccess.CreateDataAccess.EndTxn()
                For Each ObjTHr In MyBase.List
                    With ObjTHr
                        .IsDirty = False
                        .IsPersistedThrs = True

                    End With
                Next
            Case Else
                'NOP 
        End Select

        Return intSQLReturnValue
    End Function

End Class


