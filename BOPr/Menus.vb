Option Strict On
Imports System.Data.SqlClient

Public Class Menus
    Inherits POSBOs

    Public Enum EnumView As Integer  'This enum determines the select clause
        BaseView
        CompleteView
    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive
        Terminated

    End Enum

    Public Sub New(ByVal enumMenufilter As EnumFilter, ByVal enumMenuview As EnumView)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim objMenu As Menu

        'Dim msg As String


        Select Case enumMenuview
            Case EnumView.BaseView
                sqlcommand = "Select MenuId,SessionId,SaleDate From Menu"

            Case EnumView.CompleteView
                sqlcommand = "Select * from Menu"

        End Select


        Select Case enumMenufilter

            Case EnumFilter.All
            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  MenuStatus =  " & Menu.enumMenuStatus.Inactive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where MenuStatus = " & Menu.enumMenuStatus.Active


        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()       'loops through the records in the SqlDr.
                objMenu = New Menu()
                Me.Add(objMenu)
                objMenu.LoadMenuProperties(SqlDr)
            Loop
        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub


    'Private Sub loadCollection(ByVal SqlDr As SqlDataReader)

    '    Dim MenuId As Integer
    '    Dim SessionId As Integer
    '    Dim MenuSaleDate As DateTime
    '    Dim MenuChangedBy As Integer
    '    Dim MenuChangedAt As DateTime
    '    Dim MenuStatus As Menu.enumMenuStatus

    '    Dim SqlDrColName As String
    '    Dim msg As String
    '    Dim i As Integer


    '    Do While SqlDr.Read()       'loops through the records in the SqlDr.

    '        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

    '            SqlDrColName = SqlDr.GetName(i)

    '            Select Case SqlDrColName

    '                Case "MenuId"
    '                    If Not SqlDr.IsDBNull(i) Then

    '                        MenuId = SqlDr.GetInt32(i)
    '                    End If

    '                Case "SessionId"

    '                    If Not SqlDr.IsDBNull(i) Then
    '                        SessionId = SqlDr.GetInt32(i)
    '                    End If


    '                Case "MenuSaleDate"
    '                    If Not SqlDr.IsDBNull(i) Then
    '                        MenuSaleDate = SqlDr.GetDateTime(i)
    '                    End If


    '                Case "MenuChangedBy"
    '                    If Not SqlDr.IsDBNull(i) Then
    '                        MenuChangedBy = SqlDr.GetInt32(i)
    '                    Else
    '                        MenuChangedBy = 0
    '                    End If

    '                Case "MenuChangedAt"
    '                    If Not SqlDr.IsDBNull(i) Then
    '                        MenuChangedAt = SqlDr.GetDateTime(i)
    '                    Else
    '                        MenuChangedAt = Date.MinValue
    '                    End If

    '                Case "MenuStatus "
    '                    Try
    '                        If Not SqlDr.IsDBNull(i) Then
    '                            MenuStatus = CType(SqlDr.GetInt16(i), Menu.enumMenuStatus)
    '                        Else
    '                            MenuStatus = Menu.enumMenuStatus.Active
    '                        End If
    '                    Catch ex As Exception
    '                        '???errormessage???
    '                    End Try

    '            End Select


    '        Next


    '        Me.Add(New Menu(MenuId, SessionId, MenuSaleDate, MenuChangedBy, MenuChangedAt, MenuStatus))

    '    Loop

    'End Sub



    Public ReadOnly Property Item(ByVal index As Integer) As Menu
        Get
            Return CType(List(index), Menu)
        End Get

    End Property


    Public ReadOnly Property Keys() As ICollection
        Get
            Dim myMenuIds() As Integer

            Return myMenuIds

        End Get
    End Property


    Public Sub Add(ByRef value As Menu)



        MyBase.addPOSBO(CType(value, POSBO))

    End Sub


    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)
    End Sub


    Public Function Contains(ByVal value As Menu) As Boolean

        MyBase.containsPOSBO(CType(value, POSBO))
    End Function


End Class


