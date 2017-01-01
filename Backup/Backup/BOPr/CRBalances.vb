Option Strict On
Imports System.Data.SqlClient
Imports System.IO

Public Class CRBalances
    Inherits POSBOs

    Private myCRBalance As CRBalance

    Public Enum SessionType
        LunchOpening
        LunchClosing
        DinnerOpening
        DinnerClosing
    End Enum



    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView

    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        Overridden
    End Enum

    Private m_SaleDate As DateTime

    Public Sub New()

    End Sub


    Public Sub New(ByVal enumCRBalanceFilter As CRBalances.EnumFilter, _
                    ByVal enumCRBalanceView As CRBalances.EnumView, ByVal forsaledate As Date)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objCRBalance As CRBalance


        Select Case enumCRBalanceView
            Case EnumView.BaseView
                sqlcommand = "Select CRBalanceId,CRId, CashierId,CRBalanceSaleDate,CRBalance From CRBalance"

            Case EnumView.CompleteView
                sqlcommand = "Select * from CRBalance"

        End Select


        Select Case enumCRBalanceFilter

            Case EnumFilter.All

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
                CStr(CRBalance.enumCRBalanceStatus.Active)

            Case EnumFilter.Overridden
                sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
                CStr(CRBalance.enumCRBalanceStatus.Overridden)

        End Select
        sqlcommand = sqlcommand & _
                   " and CRBalanceSaleDate='" & forsaledate.ToShortDateString & "'"
        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            ' Me.CurrentCRBalance.ReadSqlDr(SqlDr)
            'loadCollection(SqlDr)       'then empty CRbalance object is returned
            Do While SqlDr.Read()
                objCRBalance = New CRBalance()
                Me.Add(objCRBalance)
                objCRBalance.ReadSqlDr(SqlDr)
            Loop
            SqlDr.Close()
        End If

        objDataAccess.CloseConnection()
    End Sub


    Public Sub New(ByVal enumCRBalanceFilter As CRBalances.EnumFilter, _
                   ByVal enumCRBalanceView As CRBalances.EnumView, ByVal forsaledate As Date, ByVal forSessionId As Session.enumSessionName)

        MyBase.New()
        GetHistoricalCRBalance(enumCRBalanceFilter, enumCRBalanceView, forsaledate, forSessionId)

        'Dim sqlcommand As String
        'Dim SqlDr As SqlDataReader
        'Dim msg As String
        'Dim objDataAccess As DataAccess
        'Dim objCRBalance As CRBalance


        'Select Case enumCRBalanceView
        '    Case EnumView.BaseView
        '        sqlcommand = "Select CRBalanceId,CRId, CashierId,CRBalanceSaleDate,CRBalance From CRBalance"

        '    Case EnumView.CompleteView
        '        sqlcommand = "Select * from CRBalance"

        'End Select


        'Select Case enumCRBalanceFilter

        '    Case EnumFilter.All

        '    Case EnumFilter.Active
        '        sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
        '        CStr(CRBalance.enumCRBalanceStatus.Active)

        '    Case EnumFilter.Overridden
        '        sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
        '        CStr(CRBalance.enumCRBalanceStatus.Overridden)

        'End Select
        'sqlcommand = sqlcommand & _
        '           " and CRBalanceSaleDate='" & forsaledate.ToShortDateString & "'" & _
        '           " And SessionId = " & forSessionId.ToString
        ''DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        ''is called to get a reference of the class


        'objDataAccess = DataAccess.CreateDataAccess

        'SqlDr = objDataAccess.GetData(sqlcommand)

        'If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

        '    ' Me.CurrentCRBalance.ReadSqlDr(SqlDr)
        '    'loadCollection(SqlDr)       'then empty CRbalance object is returned
        '    Do While SqlDr.Read()
        '        objCRBalance = New CRBalance()
        '        Me.Add(objCRBalance)
        '        objCRBalance.ReadSqlDr(SqlDr)
        '    Loop
        'End If


    End Sub


    'Public Sub New(ByVal enumCRBalanceFilter As CRBalances.EnumFilter, _
    '                ByVal enumCRBalanceView As CRBalances.EnumView)

    '    MyBase.New()

    '    Dim sqlcommand As String
    '    Dim SqlDr As SqlDataReader
    '    Dim msg As String
    '    Dim objDataAccess As DataAccess
    '    Dim objCRBalance As CRBalance


    '    Select Case enumCRBalanceView
    '        Case EnumView.BaseView
    '            sqlcommand = "Select CRBalanceId,CRId, CashierId,CRBalanceSaleDate,CRBalance From CRBalance"

    '        Case EnumView.CompleteView
    '            sqlcommand = "Select * from CRBalance"

    '    End Select


    '    Select Case enumCRBalanceFilter

    '        Case EnumFilter.All

    '        Case EnumFilter.Active
    '            sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
    '            CStr(CRBalance.enumCRBalanceStatus.Active)

    '        Case EnumFilter.Overridden
    '            sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
    '            CStr(CRBalance.enumCRBalanceStatus.Overridden)

    '    End Select
    '    sqlcommand = sqlcommand & _
    '               " and CRBalanceSaleDate='" & SaleDays.CreateSaleDay.CurrentSaleDay.SaleDate.ToShortDateString & "'"
    '    'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
    '    'is called to get a reference of the class


    '    objDataAccess = DataAccess.CreateDataAccess

    '    SqlDr = objDataAccess.GetData(sqlcommand)

    '    If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

    '        ' Me.CurrentCRBalance.ReadSqlDr(SqlDr)
    '        'loadCollection(SqlDr)       'then empty CRbalance object is returned
    '        Do While SqlDr.Read()
    '            objCRBalance = New CRBalance()
    '            Me.Add(objCRBalance)
    '            objCRBalance.ReadSqlDr(SqlDr)
    '        Loop
    '    End If


    'End Sub



    'for every record returned by SqlReader an CRBalance object is created and added to the collection

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)
        Dim objCRBalance As CRBalance
        Dim CrBalanceId As Integer
        Dim CRId As Integer
        Dim CashierId As Integer
        Dim MoneyCountId As Integer
        Dim CRCashierSignInAt As DateTime
        Dim CRCashierSignOutAt As DateTime
        Dim CROpeningBalance As Decimal
        Dim CRClosingBalance As Decimal
        Dim CRBalanceSaleDate As DateTime
        Dim CRBalanceOkBy As Integer
        Dim CRBalanceOkAt As DateTime
        Dim CROpeningBalanceChangedBy As Integer
        Dim CROpeningBalanceChangedAt As DateTime
        Dim CRClosingBalanceChangedBy As Integer
        Dim CRClosingBalanceChangedAt As DateTime
        Dim CRBalanceStatus As CRBalance.enumCRBalanceStatus

        Dim SqlDrColName As String
        Dim i As Integer
        Dim msg As String

        Do While SqlDr.Read()       'loops through the records in the sqlReader.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "CRBalanceId"
                        If Not SqlDr.IsDBNull(i) Then

                            CrBalanceId = SqlDr.GetInt32(i)
                        End If

                    Case "CRId"
                        Try


                            If Not SqlDr.IsDBNull(i) Then
                                CRId = SqlDr.GetInt16(i)
                            End If

                        Catch ex As Exception
                            '???errormessage???
                        End Try
                        'Case "MoneyCountId"

                        '    If Not SqlDr.IsDBNull(i) Then
                        '        MoneyCountId = SqlDr.GetInt32(i)
                        '    End If
                    Case "CashierId"
                        If Not SqlDr.IsDBNull(i) Then
                            CashierId = SqlDr.GetInt32(i)
                        End If


                    Case "CRCashierSignInAt"

                        If Not SqlDr.IsDBNull(i) Then
                            CRCashierSignInAt = SqlDr.GetDateTime(i)
                        End If

                    Case "CRCashierSignOutAt"

                        If Not SqlDr.IsDBNull(i) Then
                            CRCashierSignOutAt = SqlDr.GetDateTime(i)
                        End If


                    Case "CROpeningBalance"
                        If Not SqlDr.IsDBNull(i) Then
                            CROpeningBalance = SqlDr.GetDecimal(i)
                        End If

                    Case "CRClosingBalance"
                        If Not SqlDr.IsDBNull(i) Then
                            CRClosingBalance = SqlDr.GetDecimal(i)
                        End If

                    Case "CRBalanceSaleDate"
                        If Not SqlDr.IsDBNull(i) Then
                            CRBalanceSaleDate = SqlDr.GetDateTime(i)
                        End If

                    Case "CRBalanceOkBy"
                        If Not SqlDr.IsDBNull(i) Then
                            CRBalanceOkBy = SqlDr.GetInt32(i)
                        Else
                            CRBalanceOkBy = 0
                        End If

                    Case "CRBalanceOkAt"
                        If Not SqlDr.IsDBNull(i) Then
                            CRBalanceOkAt = SqlDr.GetDateTime(i)
                        Else
                            CRBalanceOkAt = Date.MinValue
                        End If


                    Case "CROpeningBalanceChangedBy"
                        If Not SqlDr.IsDBNull(i) Then
                            CROpeningBalanceChangedBy = SqlDr.GetInt32(i)
                        Else
                            CROpeningBalanceChangedBy = 0
                        End If

                    Case "CROpeningBalanceChangedAt"
                        If Not SqlDr.IsDBNull(i) Then
                            CROpeningBalanceChangedAt = SqlDr.GetDateTime(i)
                        Else
                            CROpeningBalanceChangedAt = Date.MinValue
                        End If


                    Case "CRClosingBalanceChangedBy"
                        If Not SqlDr.IsDBNull(i) Then
                            CRClosingBalanceChangedBy = SqlDr.GetInt32(i)
                        Else
                            CRClosingBalanceChangedBy = 0
                        End If

                    Case "CRClosingBalanceChangedAt"
                        If Not SqlDr.IsDBNull(i) Then
                            CRClosingBalanceChangedAt = SqlDr.GetDateTime(i)
                        Else
                            CRClosingBalanceChangedAt = Date.MinValue
                        End If

                    Case "CRBalanceStatus "
                        Try
                            If Not SqlDr.IsDBNull(i) Then
                                CRBalanceStatus = CType(SqlDr.GetInt16(i), CRBalance.enumCRBalanceStatus)
                            End If
                        Catch ex As Exception
                            '???errormessage???
                        End Try

                End Select


            Next

            Me.Add(New CRBalance(CrBalanceId, CashierId, CRCashierSignInAt, _
                                 CRCashierSignOutAt, CROpeningBalance, CRClosingBalance, _
                                 CRBalanceSaleDate, CRBalanceOkBy, CRBalanceOkAt, _
                                 CROpeningBalanceChangedBy, CROpeningBalanceChangedAt, _
                                 CRClosingBalanceChangedBy, CRClosingBalanceChangedAt, _
                                 CRBalanceStatus))


        Loop
    End Sub





    Private Sub GetHistoricalCRBalance(ByVal enumCRBalanceFilter As CRBalances.EnumFilter, _
                       ByVal enumCRBalanceView As CRBalances.EnumView, ByVal forsaledate As Date, ByVal forSessionId As Session.enumSessionName)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim objDataAccess As DataAccess
        Dim objCRBalance As CRBalance
        Dim msg As String


        m_SaleDate = forsaledate


        Select Case enumCRBalanceView
            Case EnumView.BaseView
                sqlcommand = "Select CRBalanceId,CRId, CashierId,CRBalanceSaleDate,CRBalance From CRBalance"

            Case EnumView.CompleteView
                sqlcommand = "Select * from CRBalance"

        End Select


        Select Case enumCRBalanceFilter

            Case EnumFilter.All

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
                CStr(CRBalance.enumCRBalanceStatus.Active)

            Case EnumFilter.Overridden
                sqlcommand = sqlcommand & " where CRBalanceStatus = " & _
                CStr(CRBalance.enumCRBalanceStatus.Overridden)

        End Select
        sqlcommand = sqlcommand & _
                   " and CRBalanceSaleDate='" & forsaledate.ToShortDateString & "'" & _
                   " And SessionId = " & CStr(forSessionId)
        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class


        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty


            Do While SqlDr.Read()
                objCRBalance = New CRBalance()
                Me.Add(objCRBalance)
                objCRBalance.ReadSqlDr(SqlDr)
            Loop
        End If

    End Sub

    Public ReadOnly Property SaleDate() As Date
        Get
            Return m_SaleDate
        End Get
    End Property



    Public ReadOnly Property Item(ByVal index As Integer) As CRBalance
        Get
            Return CType(MyBase.itemPOSBO(index), CRBalance)
        End Get

    End Property


    Public ReadOnly Property Item(ByVal enumSessionId As Session.enumSessionName) As CRBalance
        Get
            Dim intIndex As Integer
            intIndex = Me.Indexof(enumSessionId)

            If intIndex < 0 Then
                GetHistoricalCRBalance(CRBalances.EnumFilter.Active, _
                                        CRBalances.EnumView.CompleteView, _
                                        Me.SaleDate, enumSessionId)
                intIndex = Me.Indexof(enumSessionId)
                If intIndex < 0 Then
                    AddCRBalance(enumSessionId)
                    intIndex = Me.Indexof(enumSessionId)
                End If
            End If

            Return Me.Item(intIndex)
        End Get
    End Property

    Public ReadOnly Property Indexof(ByVal intSessionId As Session.enumSessionName) As Integer
        Get
            Dim objCRBalance As CRBalance
            Dim intIndex As Integer

            intIndex = -1
            For Each objCRBalance In MyBase.List
                If (objCRBalance.CRBalanceSaleDate = SaleDate) AndAlso (objCRBalance.SessionID = intSessionId) Then
                    intIndex = MyBase.List.IndexOf(objCRBalance)
                    Exit For
                End If
            Next
            Return intIndex
        End Get
    End Property





    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myCRBalanceIds() As Integer

            Return myCRBalanceIds

        End Get
    End Property

    Public Sub Add(ByRef value As CRBalance)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub



    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub
    'Public Function IndexOf(ByVal objCRBalance As CRBalance) As Integer

    '    Return MyBase.List.IndexOf(CType(objCRBalance, POSBO))

    'End Function

    Public Function Contains(ByVal value As CRBalance) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

    Public Function AddCRBalance() As CRBalance
        Dim objCRBalance As CRBalance

        objCRBalance = New CRBalance()
        objCRBalance.CRBalanceStatus = CRBalance.enumCRBalanceStatus.Active
        objCRBalance.CashierOpeningBalance = 0
        objCRBalance.CashierClosingBalance = 0
        Me.Add(objCRBalance)
        Return objCRBalance
    End Function


    Public Function AddCRBalance(ByVal enumSessionId As Session.enumSessionName) As CRBalance
        Dim objCRBalance As CRBalance

        objCRBalance = New CRBalance()
        With objCRBalance
            .CRBalanceStatus = CRBalance.enumCRBalanceStatus.Active
            '.CashierOpeningBalance = 0
            '.CashierClosingBalance = 0
            .SessionID = enumSessionId
            .CRBalanceSaleDate = SaleDate
        End With
        Me.Add(objCRBalance)
        Return objCRBalance
    End Function

    Friend ReadOnly Property GetCrBalance(ByVal SaleDate As Date, ByVal SessionId As Integer) As CRBalance
        Get
            Dim objCrBalance As CRBalance
            For Each objCrBalance In MyBase.List
                If objCrBalance.CRBalanceSaleDate = SaleDate And _
                    objCrBalance.SessionID = SessionId Then
                    Exit For
                Else
                    objCrBalance = Nothing
                End If
            Next
            Return objCrBalance
        End Get

    End Property



    'Public ReadOnly Property LastCompletedCrBalance() As CRBalance
    '    Get
    '        Dim i As Integer
    '        Dim objLatestCRBalance As CRBalance
    '        If Me.Count = 0 Then
    '            'nop
    '        Else
    '            For i = Math.Max(Me.Count - 1, 0) To 0 Step -1
    '                With Me.Item(i)
    '                    If (.CRBalanceStatus = CRBalance.enumCRBalanceStatus.Active) And _
    '                        (.CashierOpeningBalance > 0) And _
    '                        (.CashierClosingBalance > 0) And _
    '                        .CRBalanceSaleDate = SaleDays.CreateSaleDay.CurrentSaleDay.SaleDate Then
    '                        objLatestCRBalance = Me.Item(i)
    '                        Exit For
    '                    Else
    '                        'nope
    '                    End If
    '                End With
    '            Next
    '        End If
    '        Return objLatestCRBalance
    '    End Get
    'End Property



    Friend ReadOnly Property LastCRBalance() As CRBalance

        Get
            Dim i As Integer
            Dim objLastCRBalance As CRBalance

            'For i = Math.Max(Me.Count - 1, 0) To 0 Step -1
            '    With Me.Item(i)
            '        If (.CRBalanceStatus = CRBalance.enumCRBalanceStatus.Active) And _
            '            (.CRBalanceSaleDate = SaleDay.CreateSaleDay.SaleDate) Then
            '            objLastCRBalance = Me.Item(i)
            '            Exit For
            '        End If
            '    End With
            'Next
            If Me.Count > 0 Then
                objLastCRBalance = Me.Item(Me.Count - 1)
            End If
            Return objLastCRBalance
        End Get
    End Property

    Public ReadOnly Property CrBalnceOf(ByVal intIndex As Integer) As CRBalance
        Get
            Return Me.Item(intIndex)
        End Get
    End Property

    'Public ReadOnly Property CurrentCRBalance() As CRBalance

    '    Get
    '        Dim i As Integer
    '        Dim objCurrentCRBalance As CRBalance

    '        Return objCurrentCRBalance
    '    End Get
    'End Property

    'Public Function SesssionOpenClose() As SessionType

    '    Dim objCrBalance As CRBalance
    '    Dim intCrBalanceCount As Integer
    '    Dim SessionOpenClose As SessionType

    '    intCrBalanceCount = SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.Count()
    '    Select Case intCrBalanceCount
    '        Case 0
    '            SessionOpenClose = SessionType.LunchOpening
    '        Case 1
    '            objCrBalance = SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.CrBalnceOf(0)
    '            If objCrBalance.CashierOpeningBalance > 0 And _
    '                objCrBalance.CashierClosingBalance = 0 Then
    '                SessionOpenClose = SessionType.LunchClosing
    '            ElseIf objCrBalance.CashierOpeningBalance > 0 And _
    '                objCrBalance.CashierClosingBalance > 0 Then
    '                SessionOpenClose = SessionType.DinnerOpening
    '            End If
    '        Case 2
    '            objCrBalance = SaleDays.CreateSaleDay.CurrentSaleDay.TodaysCRBalances.CrBalnceOf(1)
    '            If objCrBalance.CashierOpeningBalance > 0 And _
    '               objCrBalance.CashierClosingBalance = 0 Then
    '                SessionOpenClose = SessionType.DinnerClosing
    '            End If
    '    End Select
    '    Return SessionOpenClose

    'End Function

    Public Function SetData() As Integer
        Dim objCRBalance As CRBalance
        Dim intSQLReturnValue As Integer


        For Each objCRBalance In MyBase.List
            intSQLReturnValue = objCRBalance.SetData()
            If intSQLReturnValue < 0 Then Exit For
        Next

        Return intSQLReturnValue

    End Function



End Class

