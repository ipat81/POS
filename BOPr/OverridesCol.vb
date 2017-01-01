Option Strict On

Imports System.Data.SqlClient

Public Class OverridesCol
    Inherits POSBOs

    Public Sub New()
        MyBase.New()
    End Sub


    Public Sub New(ByVal fromDate As Date, ByVal ToDate As Date)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim objOverride As Override


        sqlcommand = "Select * from Override where OverrideAt >= '" & fromDate & "'" & " And OverrideAt <=' " & (ToDate.AddDays(1)).ToString & "'"

        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty


            Do While SqlDr.Read()
                objOverride = New Override()
                Me.Add(objOverride)
                objOverride.GetData(SqlDr)
            Loop
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub

    Public ReadOnly Property Item(ByVal index As Integer) As Override
        Get
            Return CType(MyBase.itemPOSBO(index), Override)
        End Get

    End Property

    Public ReadOnly Property IndexOf(ByVal objOverride As Override) As Integer
        Get
            Return MyBase.List.IndexOf(objOverride)
        End Get
    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myOverrideIds() As Integer

            Return myOverrideIds

        End Get
    End Property

    Private Sub Add(ByRef value As Override)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub

    Public Function Contains(ByVal value As Override) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function GetOverrideSortedList() As ArrayList
        Dim objOverride As Override
        Dim cloneObjectOverrides As ArrayList

        cloneObjectOverrides = CType(Me.InnerList.Clone, ArrayList)
        Return cloneObjectOverrides
    End Function


End Class

