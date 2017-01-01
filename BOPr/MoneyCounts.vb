
Imports System.Data.SqlClient


Public Class MoneyCounts
    Inherits POSBOs


    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView
    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive
        Voided
    End Enum
    Private m_CurrentMoneyCount As MoneyCount
    Private m_ParentId As Integer

    Friend Sub New()

    End Sub

    Friend Sub New(ByVal enumMoneyCountFilter As EnumFilter, _
                     ByVal enumMoneyCountView As EnumView, _
                    ByVal parentId As Integer)
        MyBase.New()
        GetHistoricalMoneyCount(enumMoneyCountFilter, enumMoneyCountView, parentId)
    End Sub

    Private Sub GetHistoricalMoneyCount(ByVal enumMoneyCountFilter As EnumFilter, ByVal enumMoneyCountView As EnumView, _
                ByVal parentId As Integer)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objMoneyCount As MoneyCount

        m_ParentId = parentId

        Select Case enumMoneyCountView
            Case EnumView.BaseView
                sqlcommand = "Select MoneyCountId,MoneyCountType,MoneyCountParentId From MoneyCount"

            Case EnumView.CompleteView
                sqlcommand = "Select * from MoneyCount"
        End Select

        Select Case enumMoneyCountFilter

            Case EnumFilter.All
            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  MoneyCountStatus =  " & MoneyCount.enumMoneyCountStatus.Inactive

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where MoneyCountStatus = " & MoneyCount.enumMoneyCountStatus.Active

            Case EnumFilter.Voided
                sqlcommand = sqlcommand & " where MoneyCountStatus = " & MoneyCount.enumMoneyCountStatus.Voided

        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class

        sqlcommand = sqlcommand & " And MoneyCountParentId = " & parentId
        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()
                objMoneyCount = New MoneyCount()
                Me.Add(objMoneyCount)
                objMoneyCount.ReadSqlDr(SqlDr)
            Loop
            SqlDr.Close()
        End If
    End Sub

    Public ReadOnly Property Item(ByVal index As Integer) As MoneyCount
        Get
            Return CType(MyBase.itemPOSBO(index), MoneyCount)
        End Get
    End Property

    Public ReadOnly Property Item(ByVal intMoneyCountType As MoneyCount.enumMoneyCountType) As MoneyCount
        Get
            Dim intIndex As Integer
            intIndex = Me.Indexof(intMoneyCountType)

            If (intIndex < 0) AndAlso (ParentId > 0) Then
                GetHistoricalMoneyCount(MoneyCounts.EnumFilter.Active, _
                                    MoneyCounts.EnumView.CompleteView, _
                                       ParentId)
                intIndex = Me.Indexof(intMoneyCountType)
            End If
            If intIndex < 0 Then
                AddMoneyCount(intMoneyCountType)
                intIndex = Me.Indexof(intMoneyCountType)
            End If

            Return Me.Item(intIndex)
        End Get
    End Property

    Public ReadOnly Property ParentId() As Integer
        Get
            Return m_ParentId
        End Get
    End Property

    Public ReadOnly Property Indexof(ByVal intMCountType As MoneyCount.enumMoneyCountType) As Integer
        Get
            Dim objMoneyCount As MoneyCount
            Dim intIndex As Integer

            intIndex = -1
            For Each objMoneyCount In MyBase.List
                If (objMoneyCount.MoneyCountType = intMCountType) Then
                    intIndex = MyBase.List.IndexOf(objMoneyCount)
                    Exit For
                End If
            Next
            Return intIndex
        End Get
    End Property

    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myMoneyCountIds() As Integer

            Return myMoneyCountIds
        End Get
    End Property

    Public Sub Add(ByVal value As MoneyCount)
        MyBase.addPOSBO(CType(value, POSBO))
    End Sub

    Public Sub Remove(ByVal index As Integer)
        MyBase.removePOSBO(index)
    End Sub

    Public ReadOnly Property IndexOf(ByVal objMoneyCount As MoneyCount) As Integer
        Get
            Return MyBase.List.IndexOf(CType(objMoneyCount, POSBO))
        End Get
    End Property

    Public Function Contains(ByVal value As MoneyCount) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))
    End Function

    Friend Function SetData(ByVal intMoneyCountParentId As Integer) As Integer
        Dim objMoneyCount As MoneyCount
        Dim intSQLReturnValue As Integer
        intSQLReturnValue = 999
        For Each objMoneyCount In MyBase.List
            intSQLReturnValue = objMoneyCount.SetData(intMoneyCountParentId)
            If intSQLReturnValue < 0 Then Exit For
        Next
        Return intSQLReturnValue
    End Function

    Private Function AddMoneyCount() As MoneyCount
        m_CurrentMoneyCount = New MoneyCount()
        Me.Add(m_CurrentMoneyCount)
        Return CurrentMoneyCount
    End Function

    Private Function AddMoneyCount(ByVal intMoneyCountType As MoneyCount.enumMoneyCountType) As MoneyCount
        m_CurrentMoneyCount = New MoneyCount()

        Me.Add(m_CurrentMoneyCount)
        CurrentMoneyCount.MoneyCountType = intMoneyCountType
        Select Case intMoneyCountType
            Case MoneyCount.enumMoneyCountType.Closing
                CurrentMoneyCount.MoneyCountParentId = ParentId
        End Select
        Return CurrentMoneyCount
    End Function

    Public ReadOnly Property CurrentMoneyCount() As MoneyCount
        Get
            Return m_CurrentMoneyCount
        End Get
    End Property
End Class

