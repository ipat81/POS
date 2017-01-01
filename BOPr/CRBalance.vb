Option Strict On
Imports System.Data.SqlClient

Public Class CRBalance
    Inherits POSBO

    Public Enum enumCRBalanceStatus
        Active
        Overridden
    End Enum
    Public Enum enumSignInOut
        SignIn
        SignOut
        Invalid
    End Enum
    Public Enum enumLunchDinner
        LunchOpening
        LunchClosing
        DinnerOpening
        DinnerClosing
        Invalid
    End Enum

    Private m_CRBalanceId As Integer
    Private m_CRId As Integer
    Private m_SessionId As Session.enumSessionName
    Private m_CashierId As Integer
    Private m_CRBalanceMoneyCountId As Integer
    Private m_CashierSignInAt As DateTime
    Private m_CashierSignOutAt As DateTime
    Private m_CashierOpeningBalance As Double
    Private m_CashierClosingBalance As Double
    Private m_CRBalanceSaleDate As DateTime
    Private m_CRBalanceOKBy As Integer
    Private m_CRBalanceOKAt As DateTime
    Private m_CROpeningBalanceChangedBy As Integer
    Private m_CROpeningBalanceChangedAt As DateTime
    Private m_CRClosingBalanceChangedBy As Integer
    Private m_CRClosingBalanceChangedAt As DateTime
    Private m_CRBalanceStatus As enumCRBalanceStatus
    Private m_IsDirty As Boolean
    Private m_IsPersistedCRBalance As Boolean
    Private m_CrBalanceMoneyCountType As MoneyCount.enumMoneyCountType

    Private m_MoneyCounts As MoneyCounts
    Public Sub New()
        '    SetCRID(SaleDays.CreateSaleDay.CRId)
        '    'm_CRBalanceSaleDate = SaleDays.CreateSaleDay.SaleDate
    End Sub

    'Public Sub New(ByVal newCRBalanceid As Integer)
    '    CRBalanceId = newCRBalanceid
    '    m_CRBalanceSaleDate = SaleDays.CreateSaleDay.CurrentSaleDay.SaleDate
    'End Sub


    Friend Sub New(ByVal newcrbalanceid As Integer, ByVal newcashierid As Integer, _
                 ByVal newcashiersigninat As DateTime, _
                ByVal newm_Cashiersignoutat As DateTime, ByVal newm_CashierOpeningBalance As Decimal, _
                ByVal newcashierclosingbalance As Decimal, ByVal newcrbalancesaledate As DateTime, _
                ByVal newcrbalanceokby As Integer, ByVal newcrbalanceokat As DateTime, _
                ByVal newopeningcrbalancechangedby As Integer, ByVal newopeningcrbalancechangedat As DateTime, _
                ByVal newclosingcrbalancechangedby As Integer, ByVal newclosingcrbalancechangedat As DateTime, _
                ByVal newcrbalancestatus As enumCRBalanceStatus)
        CRBalanceId = newcrbalanceid
        CashierId = newcashierid
        m_CashierSignInAt = newcashiersigninat
        m_CashierSignOutAt = newm_Cashiersignoutat
        m_CashierOpeningBalance = newm_CashierOpeningBalance
        m_CashierClosingBalance = newcashierclosingbalance
        m_CRBalanceSaleDate = newcrbalancesaledate
        m_CRBalanceOKBy = newcrbalanceokby
        m_CRBalanceOKAt = newcrbalanceokat
        m_CROpeningBalanceChangedBy = newopeningcrbalancechangedby
        m_CROpeningBalanceChangedAt = newopeningcrbalancechangedat
        m_CRClosingBalanceChangedBy = newclosingcrbalancechangedby
        m_CRClosingBalanceChangedAt = newclosingcrbalancechangedat
        m_CRBalanceStatus = newcrbalancestatus

        'CRBalanceMoneyCounts = New MoneyCounts()


    End Sub

    Public Property CRBalanceId() As Integer

        Get
            Return m_CRBalanceId
        End Get

        Set(ByVal Value As Integer)
            m_CRBalanceId = Value
        End Set
    End Property


    Public ReadOnly Property CRID() As Integer

        Get
            Return m_CRId
        End Get
    End Property
    Private Sub SetCRID(ByVal value As Integer)
        m_CRId = value
    End Sub

    Public Property SessionID() As Session.enumSessionName
        Get
            Return m_SessionId
        End Get
        Set(ByVal Value As Session.enumSessionName)
            m_SessionId = Value
            'MarkDirty()
        End Set
    End Property

    Public Property CashierId() As Integer
        Get
            Return m_CashierId
        End Get

        Set(ByVal Value As Integer)
            m_CashierId = Value
            'MarkDirty()
        End Set
    End Property

    Public ReadOnly Property CRBalanceMoneyCounts() As MoneyCounts
        Get
            If (m_MoneyCounts Is Nothing) AndAlso (Me.CRBalanceId > 0) Then
                m_MoneyCounts = New MoneyCounts(MoneyCounts.EnumFilter.Active, _
                                                    MoneyCounts.EnumView.CompleteView, _
                                                    CRBalanceId)
            ElseIf m_MoneyCounts Is Nothing Then
                m_MoneyCounts = New MoneyCounts()
            Else
                'nop: return the existing
            End If
            Return m_MoneyCounts
        End Get

    End Property



    Public ReadOnly Property CashierSignInAt() As DateTime

        Get
            Return m_CashierSignInAt
        End Get


    End Property

    Private Sub SetCashierSignInAt()
        m_CashierSignInAt = SaleDays.CreateSaleDay.Now()
    End Sub


    Public Property CashierSignOutAt() As DateTime

        Get
            Return m_CashierSignOutAt
        End Get

        Set(ByVal Value As DateTime)
            m_CashierSignOutAt = Value
            ' MarkDirty()
        End Set
    End Property

    Public Property CashierOpeningBalance() As Double
        Get
            Return m_CashierOpeningBalance
        End Get

        Set(ByVal Value As Double)
            m_CashierOpeningBalance = Value
            SetCashierSignInAt()
            SetCROpeningBalanceChangedBy()
            'SetCROpeningBalanceChangedAt()
            MarkDirty()

        End Set
    End Property

    Public Property CashierClosingBalance() As Double
        Get
            Return m_CashierClosingBalance
        End Get

        Set(ByVal Value As Double)
            m_CashierClosingBalance = Value
            m_CashierSignOutAt = SaleDays.CreateSaleDay.Now()
            SetCRClosingBalanceChangedBy()
            'SetCRClosingBalanceChangedAt()
            MarkDirty()
        End Set
    End Property

    Public Property CRBalanceSaleDate() As DateTime

        Get
            Return m_CRBalanceSaleDate
        End Get

        Set(ByVal Value As DateTime)
            m_CRBalanceSaleDate = Value

        End Set

    End Property


    Public ReadOnly Property CRBalanceOKBy() As Integer
        Get
            Return m_CRBalanceOKBy
        End Get


    End Property

    Private Sub SetCRBalanceOKBy()
        m_CRBalanceOKBy = CashierId
    End Sub


    Public ReadOnly Property CRBalanceOKAt() As DateTime

        Get
            Return m_CRBalanceOKAt
        End Get

    End Property

    Private Sub SetCRBalanceOKAt()
        m_CRBalanceOKAt = SaleDays.CreateSaleDay.Now

    End Sub


    Public ReadOnly Property CROpeningBalanceChangedBy() As Integer


        Get
            Return m_CROpeningBalanceChangedBy
        End Get
    End Property

    Private Sub SetCROpeningBalanceChangedBy()
        m_CROpeningBalanceChangedBy = CashierId
    End Sub


    Public ReadOnly Property CROpeningBalanceChangedAt() As DateTime


        Get
            Return m_CROpeningBalanceChangedAt
        End Get
    End Property

    Friend Sub SetCROpeningBalanceChangedAt(ByVal timeNow As DateTime)
        m_CROpeningBalanceChangedAt = timeNow
        MarkDirty()
    End Sub


    Public ReadOnly Property CRClosingBalanceChangedBy() As Integer

        Get
            Return m_CRClosingBalanceChangedBy
        End Get
    End Property

    Private Sub SetCRClosingBalanceChangedBy()
        m_CRClosingBalanceChangedBy = CashierId
    End Sub


    Public ReadOnly Property CRClosingBalanceChangedAt() As DateTime


        Get
            Return m_CRClosingBalanceChangedAt
        End Get
    End Property

    Friend Sub SetCRClosingBalanceChangedAt(ByVal timeNow As DateTime)
        m_CRClosingBalanceChangedAt = timeNow
        MarkDirty()
    End Sub

    Public Property CRBalanceStatus() As enumCRBalanceStatus
        Get
            Return m_CRBalanceStatus
        End Get
        Set(ByVal Value As enumCRBalanceStatus)
            m_CRBalanceStatus = Value
        End Set
    End Property

    Friend Property IsPersistedCRBalance() As Boolean
        Get
            Return m_IsPersistedCRBalance
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedCRBalance = Value
        End Set
    End Property


    Private ReadOnly Property IsDirty() As Boolean

        Get
            Return m_IsDirty
        End Get

    End Property

    Private Sub MarkDirty()
        If IsDirty Then
            'NOP
        Else
            SetCRBalanceOKAt()
            m_IsDirty = True
        End If
    End Sub

    Public ReadOnly Property CashierSignInOut() As enumSignInOut
        Get
            If (Me.CashierSignOutAt <= Date.MinValue) And (Me.CashierSignInAt > Date.MinValue) Then
                Return enumSignInOut.SignOut
            ElseIf (Me.CashierSignOutAt <= Date.MinValue) And (Me.CashierSignInAt <= Date.MinValue) Then
                Return enumSignInOut.SignIn
            Else
                Return enumSignInOut.Invalid
            End If

        End Get

    End Property

    'Public ReadOnly Property LunchDinnerOpeningClosing() As enumLunchDinner
    '    Get
    '        If SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.Count = 1 _
    '           And SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.LastCRBalance. _
    '            CashierOpeningBalance = 0 Then
    '            Return enumLunchDinner.LunchOpening
    '        ElseIf SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.Count = 1 _
    '             And SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.LastCRBalance. _
    '                CashierOpeningBalance > 0 Then
    '            Return enumLunchDinner.LunchClosing
    '        ElseIf SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.Count = 2 _
    '             And SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.LastCRBalance. _
    '                 CashierOpeningBalance = SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances. _
    '                                        CrBalnceOf(0).CashierClosingBalance Then
    '            Return enumLunchDinner.DinnerOpening
    '        ElseIf SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.Count = 2 _
    '            And (SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.LastCRBalance. _
    '                CashierOpeningBalance) > 0 And _
    '                 (SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.LastCRBalance. _
    '                CashierClosingBalance = 0) Then
    '            Return enumLunchDinner.DinnerClosing
    '        Else
    '            Return enumLunchDinner.Invalid
    '        End If
    '    End Get
    'End Property



    Public Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strTime As String
        Dim strCashierSignInAt As String
        Dim strCashierSignOutAt As String
        Dim strCRClosingBalanceChangedAt As String
        Dim strCRBalanceOkAt As String

        strTime = SaleDays.CreateSaleDay.Now.ToString()
        ' DataAccess.CreateDataAccess.StartTxn()

        If Me.IsDirty Then
            Select Case Me.IsPersistedCRBalance      'True means update existing row, False means insert row in SQL table 
                Case False
                    If Me.CashierSignInAt > DateTime.MinValue Then
                        strCashierSignInAt = "'" & Me.CashierSignInAt.ToString & "'"
                    Else
                        strCashierSignInAt = "NULL"
                    End If
                    If Me.CashierSignOutAt > DateTime.MinValue Then
                        strCashierSignOutAt = "'" & Me.CashierSignOutAt.ToString & "'"
                    Else
                        strCashierSignOutAt = "NULL"
                    End If
                    If Me.CashierSignOutAt > DateTime.MinValue Then
                        strCRClosingBalanceChangedAt = "'" & Me.CRClosingBalanceChangedAt.ToString & "'"
                    Else
                        strCRClosingBalanceChangedAt = "Null"
                    End If

                    Me.CashierId = 1
                    strCRBalanceOkAt = "'" & Me.CRBalanceOKAt.ToString & "'"
                    strCashierSignOutAt = "'" & strTime & "'"
                    strCRClosingBalanceChangedAt = "'" & strTime & "'"

                    strSqlCmd = "insert into CRBalance values (" & Me.CRID.ToString & "," & _
                                Me.CashierId.ToString & "," & _
                                Me.CashierOpeningBalance.ToString & "," & _
                                Me.CashierClosingBalance.ToString & ",'" & _
                                Me.CRBalanceSaleDate.ToShortDateString & "'," & _
                                Me.CRBalanceOKBy.ToString & "," & _
                                strCRBalanceOkAt & "," & _
                                Me.CROpeningBalanceChangedBy.ToString & ",'" & _
                                Me.CROpeningBalanceChangedAt.ToString & "'," & _
                                Me.CRClosingBalanceChangedBy.ToString & "," & _
                                strCRClosingBalanceChangedAt & "," & _
                                CStr(Me.CRBalanceStatus) & "," & _
                                strCashierSignInAt & "," & _
                                strCashierSignOutAt & " , " & _
                                CStr(Me.SessionID) & _
                                ")"

                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.CRBalanceId() = intSQLReturnValue
                    End If
                Case Else

                    If Me.CashierSignOutAt > DateTime.MinValue Then
                        strCashierSignOutAt = "'" & Me.CashierSignOutAt.ToString & "'"
                    Else
                        strCashierSignOutAt = "NULL"
                    End If
                    If Me.CashierSignOutAt > DateTime.MinValue Then
                        strCRClosingBalanceChangedAt = "'" & Me.CRClosingBalanceChangedAt.ToString & "'"
                    Else
                        strCRClosingBalanceChangedAt = "Null"
                    End If

                    strSqlCmd = "update CRBalance set  " & _
                                 "CROpeningBalance=" & _
                                    Me.CashierOpeningBalance.ToString & "," & _
                                "CRClosingBalance=" & _
                                    Me.CashierClosingBalance.ToString & "," & _
                                "CRBalanceOKBy=" & _
                                    Me.CRBalanceOKBy.ToString & "," & _
                                "CRBalanceOKAt='" & _
                                    Me.CRBalanceOKAt.ToString & "'," & _
                                "CROpeningBalanceChangedBy=" & _
                                    Me.CROpeningBalanceChangedBy.ToString & "," & _
                                "CROpeningBalanceChangedAt='" & _
                                    Me.CROpeningBalanceChangedAt.ToString & "'," & _
                                 "CRClosingBalanceChangedBy=" & _
                                    Me.CRClosingBalanceChangedBy.ToString & "," & _
                                "CRClosingBalanceChangedAt= " & _
                                    strCRClosingBalanceChangedAt & "," & _
                                "CRBalanceStatus=" & _
                                    CStr(Me.CRBalanceStatus) & ", " & _
                                "CRCashierSignOutAt= " & _
                                    strCashierSignOutAt & _
                                " where CRBalanceId =" & Me.CRBalanceId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
        End If

        If intSQLReturnValue < 0 Then
            'NOP
        Else
            m_IsDirty = False
            Me.IsPersistedCRBalance = True
        End If

        If intSQLReturnValue < 0 Then
            'nop
        Else
            intSQLReturnValue = Me.CRBalanceMoneyCounts.SetData(Me.CRBalanceId)
        End If

        'If intSQLReturnValue < 0 Then
        '    'NOP
        'Else
        '    DataAccess.CreateDataAccess.EndTxn()
        'End If

        ' End If
        Return intSQLReturnValue
    End Function

    'Public Sub GetData(ByVal enumCRBalanceView As CRBalances.EnumView)
    '    Dim sqlcommand As String
    '    Dim SqlDr As SqlDataReader
    '    Dim SqlDrColName As String
    '    Dim objDataAccess As DataAccess

    '    Dim msg As String
    '    Dim i As Integer

    '    objDataAccess = DataAccess.CreateDataAccess

    '    Select Case enumCRBalanceView

    '        Case enumCRBalanceView.BaseView
    '            sqlcommand = "Select CRBalanceId,CRId, CashierId,CRBalanceSaleDate,CRBalance From CRBalance"

    '        Case enumCRBalanceView.CompleteView
    '            sqlcommand = "Select * from CRBalance"

    '    End Select

    '    sqlcommand = sqlcommand & " where  CRBalanceId =  " & Me.CRBalanceId

    '    'CRBalance record accessed from the database.GetData method connects to the database and fetches  the record
    '    SqlDr = objDataAccess.GetData(sqlcommand)
    '    ReadSqlDr(SqlDr)
    'End Sub

    Friend Sub ReadSqlDr(ByVal SqlDr As SqlDataReader)
        Dim SqlDrColName As String
        Dim i As Integer

        'If Not SqlDr Is Nothing Then
        'Do While SqlDr.Read()       'loops through the records in the sqlReader.


        IsPersistedCRBalance = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName

                Case "CRBalanceId"
                    If Not SqlDr.IsDBNull(i) Then

                        m_CRBalanceId = SqlDr.GetInt32(i)
                    End If

                Case "CRId"

                    If Not SqlDr.IsDBNull(i) Then
                        m_CRId = CInt(SqlDr.GetInt32(i))
                    End If
                Case "SessionId"

                    If Not SqlDr.IsDBNull(i) Then
                        m_SessionId = CType(SqlDr.GetInt32(i), Session.enumSessionName)
                    Else
                        m_SessionId = Session.enumSessionName.Lunch
                    End If

                Case "CashierId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CashierId = SqlDr.GetInt32(i)
                    End If
                Case "CROpeningBalance"
                    If SqlDr.IsDBNull(i) Then
                        m_CashierOpeningBalance = (Double.MinValue)
                    Else
                        m_CashierOpeningBalance = (SqlDr.GetDecimal(i))
                    End If
                Case "CRClosingBalance"
                    If SqlDr.IsDBNull(i) Then
                        m_CashierClosingBalance = (Double.MinValue)
                    Else
                        m_CashierClosingBalance = (SqlDr.GetDecimal(i))
                    End If

                Case "CRBalanceSaleDate"
                    If SqlDr.IsDBNull(i) Then
                        m_CRBalanceSaleDate = Date.MinValue
                    Else
                        m_CRBalanceSaleDate = SqlDr.GetDateTime(i)
                    End If
                Case "CRBalanceOkBy"
                    If SqlDr.IsDBNull(i) Then
                        m_CRBalanceOKBy = 0
                    Else

                        m_CRBalanceOKBy = SqlDr.GetInt32(i)

                    End If

                Case "CRBalanceOkAt"
                    If SqlDr.IsDBNull(i) Then
                        m_CRBalanceOKAt = Date.MinValue
                    Else
                        m_CRBalanceOKAt = SqlDr.GetDateTime(i)
                    End If

                Case "CROpeningBalanceChangedBy"
                    If SqlDr.IsDBNull(i) Then
                        m_CROpeningBalanceChangedBy = 0
                    Else

                        m_CROpeningBalanceChangedBy = SqlDr.GetInt32(i)

                    End If

                Case "CROpeningBalanceChangedAt"
                    If SqlDr.IsDBNull(i) Then
                        m_CROpeningBalanceChangedAt = Date.MinValue
                    Else
                        m_CROpeningBalanceChangedAt = SqlDr.GetDateTime(i)
                    End If

                Case "CRClosingBalanceChangedBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CRClosingBalanceChangedBy = 0
                    Else
                        m_CRClosingBalanceChangedBy = SqlDr.GetInt32(i)
                    End If

                Case "CRClosingBalanceChangedAt"
                    If SqlDr.IsDBNull(i) Then
                        m_CRClosingBalanceChangedAt = Date.MinValue
                    Else
                        m_CRClosingBalanceChangedAt = SqlDr.GetDateTime(i)
                    End If

                Case "CRBalanceStatus "
                    If Not SqlDr.IsDBNull(i) Then

                        m_CRBalanceStatus = CType(SqlDr.GetInt16(i), CRBalance.enumCRBalanceStatus)
                    End If

                Case "CRCashierSignInAt"

                    If SqlDr.IsDBNull(i) Then
                        m_CashierSignInAt = Date.MinValue
                    Else
                        m_CashierSignInAt = SqlDr.GetDateTime(i)
                    End If

                Case "CRCashierSignOutAt"

                    If SqlDr.IsDBNull(i) Then
                        m_CashierSignOutAt = Date.MinValue
                    Else
                        m_CashierSignOutAt = SqlDr.GetDateTime(i)
                    End If

            End Select
        Next
        'Loop
        'End If

    End Sub



End Class


