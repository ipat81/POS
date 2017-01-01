Option Strict On
Imports System.Data.SqlClient

Public Class DTables
    Inherits POSBOs
    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView
    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        Active
        InActive
        All
        Available
        Occupied
    End Enum

    Private m_WOrders As Orders
    Private m_MOrders As Orders
    Private m_TOrders As Orders
    Private m_BOrders As Orders
    Private m_ROrders As Orders


    Public Sub New(ByVal enumDTablesFilter As EnumFilter, ByVal enumDTablesview As EnumView)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim objDTable As DTable

        Dim msg As String


        Select Case enumDTablesview

            Case EnumView.BaseView
                sqlcommand = "Select * From DTable"

            Case EnumView.CompleteView
                sqlcommand = "Select * From DTable"

        End Select


        Select Case enumDTablesFilter


            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  TableStatus =  " & DTable.enumDTableStatus.InActive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where TableStatus = " & DTable.enumDTableStatus.Active

            Case EnumFilter.All

            Case EnumFilter.Available
            Case EnumFilter.Occupied

        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class

        sqlcommand = sqlcommand & " Order By TableName DESC"
        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            ' Me.CurrentCRBalance.ReadSqlDr(SqlDr)
            'loadCollection(SqlDr)       'then empty CRbalance object is returned
            Do While SqlDr.Read()
                objDTable = New DTable()
                Me.Add(objDTable)
                objDTable.loadData(SqlDr)
            Loop
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()

    End Sub

    Public ReadOnly Property Item(ByVal index As Integer) As DTable
        Get
            Return CType(List(index), DTable)
        End Get

    End Property
    Public ReadOnly Property Item(ByVal newTableName As String) As DTable
        Get
            Dim objDtable As DTable
            For Each objDtable In Me
                If objDtable.TableName = newTableName Then
                    Return objDtable
                    Exit For
                End If
            Next
        End Get
    End Property

    Public Sub Add(ByRef value As DTable)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub
    Public ReadOnly Property Indexof(ByVal objDTable As DTable) As Integer
        Get
            Return MyBase.List.IndexOf(CType(objDTable, POSBO))

        End Get
    End Property
    Public ReadOnly Property Indexof(ByVal intTableId As Integer) As Integer
        Get
            Dim objDtable As DTable
            With MyBase.List
                For Each objDtable In MyBase.List
                    If objDtable.TableId = intTableId Then
                        Return MyBase.List.IndexOf(CType(objDtable, POSBO))
                        Exit For
                    End If
                Next
            End With
        End Get
    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myDTableIds() As Integer


            Return myDTableIds

        End Get
    End Property


    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function Contains(ByVal value As DTable) As Boolean

        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

End Class
