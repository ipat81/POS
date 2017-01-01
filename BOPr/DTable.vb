Option Strict On
Imports System.Data.SqlClient
Public Class DTable
    Inherits POSBO

    Public Event OverrideNeeded(ByVal objPaymentReceivedOverride As Override)
    Public Enum enumDTableStatus
        Active
        InActive
    End Enum

    Public Enum enumDiningSection As Integer
        W
        M
        T
        B
        A
        Z       'dummy section for all take outs
    End Enum
    Private m_TableId As Integer
    Private m_TableName As String
    Private m_TableStatus As enumDTableStatus
    Private m_TableCapacity As Integer
    Private m_DTableStatus As enumDTableStatus
    Private m_ClosedOrders As Orders
    Private m_OpenOrders As Orders
    Private m_AllOrders As Orders
    Private m_CurrentOrder As Order

    Private m_IsPersisted As Boolean
    Private m_IsDirty As Boolean

    Public Sub New()

    End Sub

    Friend Sub loadData(ByVal SqlDr As SqlDataReader)
        Dim i As Integer
        Dim SqlDrColName As String

        IsPersisted = True

        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName

                Case "TableId"

                    If Not SqlDr.IsDBNull(i) Then
                        m_TableId = SqlDr.GetInt32(i)
                    End If

                Case "TableName"
                    m_TableName = SqlDr.GetString(i)



                Case "TableStatus "
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_TableStatus = CType(SqlDr.GetInt32(i), DTable.enumDTableStatus)
                        Else
                            m_TableStatus = DTable.enumDTableStatus.InActive
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try

                Case "TableCapacity"
                    If Not SqlDr.IsDBNull(i) Then
                        m_TableCapacity = SqlDr.GetInt32(i)
                    Else
                        m_TableCapacity = 0
                    End If
            End Select
        Next
    End Sub

    Public Property TableId() As Integer
        Get
            Return m_TableId
        End Get
        Set(ByVal Value As Integer)
            m_TableId = Value
        End Set
    End Property

    Public ReadOnly Property TableName() As String

        Get
            Return m_TableName
        End Get
    End Property

    Public ReadOnly Property DiningSection() As enumDiningSection

        Get
            Select Case m_TableName.Chars(0)
                Case enumDiningSection.A.ToString.Chars(0)
                    Return enumDiningSection.A
                Case enumDiningSection.B.ToString.Chars(0)
                    Return enumDiningSection.B
                Case enumDiningSection.M.ToString.Chars(0)
                    Return enumDiningSection.M
                Case enumDiningSection.T.ToString.Chars(0)
                    Return enumDiningSection.T
                Case enumDiningSection.W.ToString.Chars(0)
                    Return enumDiningSection.W
                Case enumDiningSection.Z.ToString.Chars(0)
                    Return enumDiningSection.Z
            End Select
        End Get
    End Property

    Public ReadOnly Property TableCapacity() As Integer
        Get
            Return m_TableCapacity
        End Get
    End Property

    Public ReadOnly Property TableDescription() As String

        Get
            Return m_TableName & "-" & TableCapacity.ToString & " Top"
        End Get
    End Property
    Public Property DTableStatus() As enumDTableStatus
        Get
            Return m_DTableStatus
        End Get
        Set(ByVal Value As enumDTableStatus)
            m_DTableStatus = Value
            MarkDirty(True)
        End Set
    End Property

    Private Sub MarkDirty(ByVal value As Boolean)
        m_IsDirty = value
    End Sub

    ReadOnly Property IsDirty() As Boolean
        Get
            Return m_IsDirty
        End Get

    End Property
    Friend Property IsPersisted() As Boolean
        Get
            Return m_IsPersisted
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersisted = Value
        End Set
    End Property

    Public Property CurrentOrder() As Order
        Get
            Return m_CurrentOrder
        End Get
        Set(ByVal Value As Order)
            m_CurrentOrder = Value
        End Set
    End Property

End Class
