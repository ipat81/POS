Option Strict On
Imports System.Data.SqlClient

Public Class SaleDaySession
    Inherits POSBO

    Public Enum enumSaleDaySessionStatus
        Open
        Closed
    End Enum

    Dim m_SaleDayId As Integer

    Public Event OverrideNeeded(ByVal newOverride As Override)
    Private m_SaleDaySessionId As Integer
    Private m_SessionId As Session.enumSessionName
    Private m_SaleDaySessionFrom As Date
    Private m_SaleDaySessionTo As Date
    Private m_SaleDaySessionChangedBy As Integer
    Private m_SaleDaySessionChangedAt As DateTime
    Private m_SaleDaySessionStatus As enumSaleDaySessionStatus
    Private m_MenuId As Integer
    Private m_SaleDate As Date

    Private m_AllOrders As Orders

    Private m_MoneyCounts As MoneyCounts

    Private m_ReconciliationApprovedBy As Integer
    Private m_ReconciliationApprovedAt As DateTime
    Private m_ReconciliationReportOverride As Override
    Private m_objOverrideForMoneyCountEdit As Override

    Private m_ReconciliationOverrideContext As String

    Private m_IsPersisted As Boolean
    Private m_IsDirty As Boolean

    'Summary Counts & Total Paid for each Payment Method for Orders of this SDSession
    Private m_CurrentAmexCount As Integer
    Private m_CurrentAmexPOSAmount As Double
    Private m_CurrentAmexTipAmountPaid As Double
    Private m_CurrentCashCount As Integer
    Private m_CurrentCashPOSAmount As Double
    Private m_CurrentCashTipAmountPaid As Double
    Private m_CurrentDebitCardCount As Integer
    Private m_CurrentDebitCardPOSAmount As Double
    Private m_CurrentDebitCardTipAmountPaid As Double
    Private m_CurrentDinersClubCount As Integer
    Private m_CurrentDinersClubPOSAmount As Double
    Private m_CurrentDinersClubTipAmountPaid As Double
    Private m_CurrentDiscoverCount As Integer
    Private m_CurrentDiscoverPOSAmount As Double
    Private m_CurrentDiscoverTipAmountPaid As Double
    Private m_CurrentHouseAccountCount As Integer
    Private m_CurrentHouseAccountPOSAmount As Double
    Private m_CurrentHouseAccountTipAmountPaid As Double
    Private m_CurrentMasterCardCount As Integer
    Private m_CurrentMasterCardPOSAmount As Double
    Private m_CurrentMasterCardTipAmountPaid As Double
    Private m_CurrentMGGiftCertificateCount As Integer
    Private m_CurrentMGGiftCertificatePOSAmount As Double
    Private m_CurrentMGGiftCertificateTipAmountPaid As Double
    Private m_CurrentPersonalCheckCount As Integer
    Private m_CurrentPersonalCheckPOSAmount As Double
    Private m_CurrentPersonalCheckTipAmountPaid As Double
    Private m_CurrentTravellersChecksCount As Integer
    Private m_CurrentTravellersChecksPOSAmount As Double
    Private m_CurrentTravellersChecksTipAmountPaid As Double
    Private m_CurrentVisaCount As Integer
    Private m_CurrentVisaPOSAmount As Double
    Private m_CurrentVisaTipAmountPaid As Double
    Private m_CurrentMissingPaymentCount As Integer
    Private m_CurrentMissingPaymentPOSAmount As Double
    Private m_CurrentMissingPaymentTipAmountPaid As Double
    Private m_CurrentKnightExpressCount As Integer
    Private m_CurrentKnightExpressPOSAmount As Double
    Private m_CurrentKnightExpressTipAmountPaid As Double

    Public Sub New()
    End Sub

    Friend Sub loadData(ByVal SqlDr As SqlDataReader)
        Dim msg As String
        Dim i As Integer
        Dim SqlDrColName As String

        IsPersisted = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record
            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName
                Case "SaleDaySessionId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDaySessionId = SqlDr.GetInt32(i)
                    End If
                Case "SaleDate"
                    m_SaleDate = SqlDr.GetDateTime(i)
                Case "SaleDaySessionFrom"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDaySessionFrom = SqlDr.GetDateTime(i)
                    Else
                        m_SaleDaySessionFrom = Date.MinValue
                    End If
                Case "SaleDaySessionTo"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDaySessionTo = SqlDr.GetDateTime(i)
                    Else
                        m_SaleDaySessionTo = Date.MinValue
                    End If
                Case "SaleDaySessionChangedBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDaySessionChangedBy = SqlDr.GetInt32(i)
                    Else
                        m_SaleDaySessionChangedBy = 0
                    End If
                Case "SaleDaySessionStatus "
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_SaleDaySessionStatus = CType(SqlDr.GetInt32(i), SaleDaySession.enumSaleDaySessionStatus)
                        Else
                            m_SaleDaySessionStatus = SaleDaySession.enumSaleDaySessionStatus.Closed
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "MenuId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MenuId = SqlDr.GetInt32(i)
                    Else
                        m_MenuId = 0
                    End If
                Case "SessionId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SessionId = CType(SqlDr.GetInt32(i), Session.enumSessionName)
                    Else
                        m_SessionId = Session.enumSessionName.Lunch
                    End If
                Case "SaleDaySessionChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_SaleDaySessionChangedAt = SqlDr.GetDateTime(i)
                    Else
                        m_SaleDaySessionChangedAt = Date.MinValue
                    End If

                Case "ReconciliationApprovedBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ReconciliationApprovedBy = SqlDr.GetInt32(i)
                    Else
                        m_ReconciliationApprovedBy = 0
                    End If

                Case "ReconciliationApprovedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ReconciliationApprovedAt = SqlDr.GetDateTime(i)
                    Else
                        m_ReconciliationApprovedAt = Date.MinValue
                    End If
            End Select
        Next
    End Sub

    Public Property SaleDaySessionId() As Integer
        Get
            Return m_SaleDaySessionId
        End Get
        Set(ByVal Value As Integer)
            m_SaleDaySessionId = Value
        End Set
    End Property

    Public Property SessionId() As Session.enumSessionName
        Get
            Return m_SessionId
        End Get

        Set(ByVal Value As Session.enumSessionName)
            m_SessionId = Value
            'MarkDirty(True)
        End Set
    End Property

    Public ReadOnly Property SessionName() As String
        Get
            Return SaleDays.CreateSaleDay.ActiveSessions.Item(Me.SessionId).SessionName
        End Get
    End Property
    Public Property SaleDate() As Date
        Get
            Return m_SaleDate
        End Get

        Set(ByVal Value As DateTime)
            m_SaleDate = Value
            ' MarkDirty(True)
        End Set
    End Property

    Public ReadOnly Property SaleDaySessionFrom() As DateTime
        Get
            If m_SaleDaySessionFrom > DateTime.MinValue Then
                Return m_SaleDaySessionFrom
            Else
                Select Case Me.SessionId
                    Case Session.enumSessionName.Lunch
                        Return CDate(Me.SaleDate.Date.ToShortDateString & " 11:00:00 AM")
                    Case Session.enumSessionName.Dinner
                        Return CDate(Me.SaleDate.Date.ToShortDateString & " 5:00:00 PM")
                End Select
            End If
        End Get
    End Property

    Public Sub SetSaleDaySessionFrom()
        Dim timeNow As DateTime
        timeNow = SaleDays.CreateSaleDay.Now

        If Me.AllMoneyCounts.Item(MoneyCount.enumMoneyCountType.Opening).IsDirty Then
            If m_SaleDaySessionFrom <= DateTime.MinValue Then m_SaleDaySessionFrom = timeNow
            m_SaleDaySessionChangedAt = timeNow
            MarkDirty(True)
        End If
    End Sub

    Public ReadOnly Property SaleDaySessionTo() As DateTime
        Get
            If m_SaleDaySessionTo > DateTime.MinValue Then
                Return m_SaleDaySessionTo
            Else
                Select Case Me.SessionId
                    Case Session.enumSessionName.Lunch
                        Return CDate(Me.SaleDate.Date.ToShortDateString & " 4:55:00 PM")
                    Case Session.enumSessionName.Dinner
                        Return CDate(Me.SaleDate.Date.ToShortDateString & " 11:55:00 PM")
                End Select
            End If
            'Return m_SaleDaySessionTo
        End Get
    End Property

    Public Sub SetSaleDaySessionTo()
        Dim timeNow As DateTime
        timeNow = SaleDays.CreateSaleDay.Now

        If Me.AllMoneyCounts.Item(MoneyCount.enumMoneyCountType.Closing).IsDirty Then
            If m_SaleDaySessionTo <= DateTime.MinValue Then m_SaleDaySessionTo = timeNow
            m_SaleDaySessionChangedAt = timeNow
            MarkDirty(True)
        End If
    End Sub

    Private Sub ResetSaleDaySessionTo()
        m_SaleDaySessionTo = DateTime.MinValue
    End Sub

    Public ReadOnly Property IsSessionOpen() As Boolean
        Get
            Return ((Me.SaleDaySessionFrom > Date.MinValue) AndAlso (Me.SaleDaySessionTo <= Date.MinValue))
        End Get
    End Property

    Public ReadOnly Property AllMoneyCounts() As MoneyCounts
        Get
            If (m_MoneyCounts Is Nothing) AndAlso (Me.SaleDaySessionId > 0) Then
                m_MoneyCounts = New MoneyCounts(MoneyCounts.EnumFilter.Active, _
                                                    MoneyCounts.EnumView.CompleteView, _
                                                    Me.SaleDaySessionId)
            ElseIf m_MoneyCounts Is Nothing Then
                m_MoneyCounts = New MoneyCounts()
            Else
                'nop: return the existing
            End If
            Return m_MoneyCounts
        End Get
    End Property

    Public Property MenuId() As Integer
        Get
            Return m_MenuId
        End Get
        Set(ByVal Value As Integer)
            m_MenuId = Value
        End Set
    End Property

    Public Property SaleDaySessionChangedBy() As Integer
        Get
            Return m_SaleDaySessionChangedBy
        End Get
        Set(ByVal Value As Integer)
            m_SaleDaySessionChangedBy = Value
        End Set
    End Property

    Public ReadOnly Property SaleDaySessionChangedAt() As DateTime
        Get
            Return m_SaleDaySessionChangedAt
        End Get
    End Property

    Private Sub SetSaleDaySessionChangedAt()
        m_SaleDaySessionChangedAt = SaleDays.CreateSaleDay.Now
        'MarkDirty(True)
    End Sub


    Public Property SaleDaySessionStatus() As enumSaleDaySessionStatus
        Get
            Return m_SaleDaySessionStatus
        End Get
        Set(ByVal Value As enumSaleDaySessionStatus)
            m_SaleDaySessionStatus = Value
            'MarkDirty(True)
        End Set
    End Property

    Public ReadOnly Property CrTXns() As CRTXNs
        Get
            Dim objCRTXNs As CRTXNs
            objCRTXNs = New CRTXNs(CrTXns.EnumFilter.Active, CrTXns.EnumView.CompleteView, Me.SaleDate)

            Return objCRTXNs
        End Get
    End Property

    Public Sub SetAllCurrentPayments(ByVal fromTime As DateTime, ByVal ToTime As DateTime)
        'Returns all Payments entered on this Saleday for Orders of this Saleday
        'This is required to reconcile Cash Register
        Dim objPayment As Payment
        Dim objOrder As Order

        m_CurrentAmexCount = 0
        m_CurrentAmexPOSAmount = 0
        m_CurrentAmexTipAmountPaid = 0

        m_CurrentCashCount = 0
        m_CurrentCashPOSAmount = 0
        m_CurrentCashTipAmountPaid = 0

        m_CurrentDebitCardCount = 0
        m_CurrentDebitCardPOSAmount = 0
        m_CurrentDebitCardTipAmountPaid = 0

        m_CurrentDinersClubCount = 0
        m_CurrentDinersClubPOSAmount = 0
        m_CurrentDinersClubTipAmountPaid = 0

        m_CurrentDiscoverCount = 0
        m_CurrentDiscoverPOSAmount = 0
        m_CurrentDiscoverTipAmountPaid = 0

        m_CurrentHouseAccountCount = 0
        m_CurrentHouseAccountPOSAmount = 0
        m_CurrentHouseAccountTipAmountPaid = 0

        m_CurrentKnightExpressCount = 0
        m_CurrentKnightExpressPOSAmount = 0
        m_CurrentKnightExpressTipAmountPaid = 0

        m_CurrentMasterCardCount = 0
        m_CurrentMasterCardPOSAmount = 0
        m_CurrentMasterCardTipAmountPaid = 0

        m_CurrentMGGiftCertificateCount = 0
        m_CurrentMGGiftCertificatePOSAmount = 0
        m_CurrentMGGiftCertificateTipAmountPaid = 0

        m_CurrentMissingPaymentCount = 0
        m_CurrentMissingPaymentPOSAmount = 0
        m_CurrentMissingPaymentTipAmountPaid = 0

        m_CurrentPersonalCheckCount = 0
        m_CurrentPersonalCheckPOSAmount = 0
        m_CurrentPersonalCheckTipAmountPaid = 0

        m_CurrentTravellersChecksCount = 0
        m_CurrentTravellersChecksPOSAmount = 0
        m_CurrentTravellersChecksTipAmountPaid = 0

        m_CurrentVisaCount = 0
        m_CurrentVisaPOSAmount = 0
        m_CurrentVisaTipAmountPaid = 0

        For Each objOrder In Me.AllOrders
            For Each objPayment In objOrder.AllPayments
                With objPayment
                    If (.EndTime > fromTime) AndAlso (.EndTime < ToTime) AndAlso _
                        (.Status = Payment.enumStatus.Active) Then
                        Select Case .PaymentMethod
                            Case Payment.enumPaymentMethod.Amex
                                m_CurrentAmexCount += 1
                                m_CurrentAmexPOSAmount += .POSAmountPaid
                                m_CurrentAmexTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.Cash
                                m_CurrentCashCount += 1
                                m_CurrentCashPOSAmount += .POSAmountPaid
                                m_CurrentCashTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.DebitCard
                                m_CurrentDebitCardCount += 1
                                m_CurrentDebitCardPOSAmount += .POSAmountPaid
                                m_CurrentDebitCardTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.DinersClub
                                m_CurrentDinersClubCount += 1
                                m_CurrentDinersClubPOSAmount += .POSAmountPaid
                                m_CurrentDinersClubTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.Discover
                                m_CurrentDiscoverCount += 1
                                m_CurrentDiscoverPOSAmount += .POSAmountPaid
                                m_CurrentDiscoverTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.HouseAccount
                                m_CurrentHouseAccountCount += 1
                                m_CurrentHouseAccountPOSAmount += .POSAmountPaid
                                m_CurrentHouseAccountTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.KnightExpress
                                m_CurrentKnightExpressCount += 1
                                m_CurrentKnightExpressPOSAmount += .POSAmountPaid
                                m_CurrentKnightExpressTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.MasterCard
                                m_CurrentMasterCardCount += 1
                                m_CurrentMasterCardPOSAmount += .POSAmountPaid
                                m_CurrentMasterCardTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.MGGiftCertificate
                                m_CurrentMGGiftCertificateCount += 1
                                m_CurrentMGGiftCertificatePOSAmount += .POSAmountPaid
                                m_CurrentMGGiftCertificateTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.MissingPayment
                                m_CurrentMissingPaymentCount += 1
                                m_CurrentMissingPaymentPOSAmount += .POSAmountPaid
                                m_CurrentMissingPaymentTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.PersonalCheck
                                m_CurrentPersonalCheckCount += 1
                                m_CurrentPersonalCheckPOSAmount += .POSAmountPaid
                                m_CurrentPersonalCheckTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.TravellersChecks
                                m_CurrentTravellersChecksCount += 1
                                m_CurrentTravellersChecksPOSAmount += .POSAmountPaid
                                m_CurrentTravellersChecksTipAmountPaid += .TipAmountPaid
                            Case Payment.enumPaymentMethod.Visa
                                m_CurrentVisaCount += 1
                                m_CurrentVisaPOSAmount += .POSAmountPaid
                                m_CurrentVisaTipAmountPaid += .TipAmountPaid
                        End Select
                    End If
                End With
            Next
        Next

    End Sub

    Public ReadOnly Property AmexCount() As Integer
        Get
            Return m_CurrentAmexCount
        End Get
    End Property

    Public ReadOnly Property AmexAmount() As Double
        Get
            Return m_CurrentAmexPOSAmount
        End Get
    End Property

    Public ReadOnly Property AmexTipAmount() As Double
        Get
            Return m_CurrentAmexTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property AmexTotal() As Double
        Get
            Return AmexAmount + AmexTipAmount
        End Get
    End Property


    Public ReadOnly Property CashCount() As Integer
        Get
            Return m_CurrentCashCount
        End Get
    End Property

    Public ReadOnly Property CashAmount() As Double
        Get
            Return m_CurrentCashPOSAmount
        End Get
    End Property

    Public ReadOnly Property CashTipAmount() As Double
        Get
            Return m_CurrentCashTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property CashTotal() As Double
        Get
            Return CashAmount + CashTipAmount
        End Get
    End Property

    Public ReadOnly Property DebitCardCount() As Integer
        Get
            Return m_CurrentDebitCardCount
        End Get
    End Property

    Public ReadOnly Property DebitCardAmount() As Double
        Get
            Return m_CurrentDebitCardPOSAmount
        End Get
    End Property

    Public ReadOnly Property DebitCardTipAmount() As Double
        Get
            Return m_CurrentDebitCardTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property DebitCardTotal() As Double
        Get
            Return DebitCardAmount + DebitCardTipAmount
        End Get
    End Property

    Public ReadOnly Property DinersClubCount() As Integer
        Get
            Return m_CurrentDinersClubCount
        End Get
    End Property

    Public ReadOnly Property DinersClubAmount() As Double
        Get
            Return m_CurrentDinersClubPOSAmount
        End Get
    End Property

    Public ReadOnly Property DinersClubTipAmount() As Double
        Get
            Return m_CurrentDinersClubTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property DinersClubTotal() As Double
        Get
            Return DinersClubAmount + DinersClubTipAmount
        End Get
    End Property

    Public ReadOnly Property DiscoverCount() As Integer
        Get
            Return m_CurrentDiscoverCount
        End Get
    End Property

    Public ReadOnly Property DiscoverAmount() As Double
        Get
            Return m_CurrentDiscoverPOSAmount
        End Get
    End Property

    Public ReadOnly Property DiscoverTipAmount() As Double
        Get
            Return m_CurrentDiscoverTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property DiscoverTotal() As Double
        Get
            Return DiscoverAmount + DiscoverTipAmount
        End Get
    End Property

    Public ReadOnly Property HouseAccountCount() As Integer
        Get
            Return m_CurrentHouseAccountCount
        End Get
    End Property

    Public ReadOnly Property HouseAccountAmount() As Double
        Get
            Return m_CurrentHouseAccountPOSAmount
        End Get
    End Property

    Public ReadOnly Property HouseAccountTipAmount() As Double
        Get
            Return m_CurrentHouseAccountTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property HouseAccountTotal() As Double
        Get
            Return HouseAccountAmount + HouseAccountTipAmount
        End Get
    End Property

    Public ReadOnly Property KnightExpressCount() As Integer
        Get
            Return m_CurrentKnightExpressCount
        End Get
    End Property

    Public ReadOnly Property KnightExpressAmount() As Double
        Get
            Return m_CurrentKnightExpressPOSAmount
        End Get
    End Property

    Public ReadOnly Property KnightExpressTipAmount() As Double
        Get
            Return m_CurrentKnightExpressTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property KnightExpressTotal() As Double
        Get
            Return KnightExpressAmount + KnightExpressTipAmount
        End Get
    End Property

    Public ReadOnly Property MasterCardCount() As Integer
        Get
            Return m_CurrentMasterCardCount
        End Get
    End Property

    Public ReadOnly Property MasterCardAmount() As Double
        Get
            Return m_CurrentMasterCardPOSAmount
        End Get
    End Property

    Public ReadOnly Property MasterCardTipAmount() As Double
        Get
            Return m_CurrentMasterCardTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property MasterCardTotal() As Double
        Get
            Return MasterCardAmount + MasterCardTipAmount
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateCount() As Integer
        Get
            Return m_CurrentMGGiftCertificateCount
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateAmount() As Double
        Get
            Return m_CurrentMGGiftCertificatePOSAmount
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateTipAmount() As Double
        Get
            Return m_CurrentMGGiftCertificateTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property MGGiftCertificateTotal() As Double
        Get
            Return MGGiftCertificateAmount + MGGiftCertificateTipAmount
        End Get
    End Property

    '****** Missing Payment Totals
    Public ReadOnly Property MissingPaymentCount() As Integer
        Get
            Return m_CurrentMissingPaymentCount
        End Get
    End Property

    Public ReadOnly Property MissingPaymentAmount() As Double
        Get
            Return m_CurrentMissingPaymentPOSAmount
        End Get
    End Property

    Public ReadOnly Property MissingPaymentTipAmount() As Double
        Get
            Return m_CurrentMissingPaymentTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property MissingPaymentTotal() As Double
        Get

            Return MissingPaymentAmount + MissingPaymentTipAmount
        End Get
    End Property
    'Personal Check Totals

    Public ReadOnly Property PersonalCheckCount() As Integer
        Get
            Return m_CurrentPersonalCheckCount
        End Get
    End Property

    Public ReadOnly Property PersonalCheckAmount() As Double
        Get
            Return m_CurrentPersonalCheckPOSAmount
        End Get
    End Property

    Public ReadOnly Property PersonalCheckTipAmount() As Double
        Get
            Return m_CurrentPersonalCheckTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property PersonalCheckTotal() As Double
        Get
            Return PersonalCheckAmount + PersonalCheckTipAmount
        End Get
    End Property

    Public ReadOnly Property TravellersChecksCount() As Integer
        Get
            Return m_CurrentTravellersChecksCount
        End Get
    End Property

    Public ReadOnly Property TravellersChecksAmount() As Double
        Get
            Return m_CurrentTravellersChecksPOSAmount
        End Get
    End Property

    Public ReadOnly Property TravellersChecksTipAmount() As Double
        Get
            Return m_CurrentTravellersChecksTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property TravellersChecksTotal() As Double
        Get
            Return TravellersChecksAmount + TravellersChecksTipAmount
        End Get
    End Property

    Public ReadOnly Property VisaCount() As Integer
        Get
            Return m_CurrentVisaCount
        End Get
    End Property

    Public ReadOnly Property VisaAmount() As Double
        Get
            Return m_CurrentVisaPOSAmount
        End Get
    End Property

    Public ReadOnly Property VisaTipAmount() As Double
        Get
            Return m_CurrentVisaTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property VisaTotal() As Double
        Get
            Return VisaAmount + VisaTipAmount
        End Get
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

    Private Sub ResetOverride()
        'Make sure that no root level Overrides can be used again after Setdata
        If m_objOverrideForMoneyCountEdit Is Nothing Then
            'nop
        Else
            m_objOverrideForMoneyCountEdit = Nothing
        End If


    End Sub

    Public Function SetData(ByVal blnResetOverrides As Boolean) As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strSessionFrom As String
        Dim strSessionTo As String
        Dim strSessionChangedAt As String
        Dim strApprovedBy As String
        Dim strApprovedAt As String

        If m_SaleDaySessionFrom > DateTime.MinValue Then
            strSessionFrom = "'" & m_SaleDaySessionFrom.ToString & "'"
        Else
            strSessionFrom = "NULL"
        End If

        If m_SaleDaySessionTo > DateTime.MinValue Then
            strSessionTo = "'" & m_SaleDaySessionTo.ToString & "'"
        Else
            strSessionTo = "NULL"
        End If

        If Me.SaleDaySessionChangedAt > DateTime.MinValue Then
            strSessionChangedAt = "'" & Me.SaleDaySessionChangedAt.ToString & "'"
        Else
            strSessionChangedAt = "'" & SaleDays.CreateSaleDay.Now & "'"
        End If


        If Me.ReconciliationApprovedAt > DateTime.MinValue Then
            strApprovedAt = "'" & Me.ReconciliationApprovedAt.ToString & "'"
        Else
            strApprovedAt = "Null"
        End If

        DataAccess.CreateDataAccess.StartTxn()

        If Me.IsDirty Then
            Select Case Me.IsPersisted        'True means update existing row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into SaleDaySession values ('" & _
                                Me.SaleDate.ToShortDateString & "'," & _
                                strSessionFrom & "," & _
                                strSessionTo & "," & _
                                CStr(Me.SessionId) & "," & _
                                Me.MenuId.ToString & "," & _
                                CStr(Me.SaleDaySessionStatus) & "," & _
                                Me.SaleDaySessionChangedBy.ToString & "," & _
                                strSessionChangedAt & "," & _
                                Me.ReconciliationApprovedBy.ToString & "," & _
                                strApprovedAt & "," & _
                                Me.SaleDayId.ToString & ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue <= 0 Then
                        MsgBox("SQL Server Error - while inserting SDSession")
                    Else
                        Me.SaleDaySessionId() = intSQLReturnValue
                        m_IsDirty = False
                        IsPersisted = True
                    End If
                Case Else
                    strSqlCmd = "update SaleDaySession set  " & _
                                "SaleDaySessionTo=" & _
                                   strSessionTo & "," & _
                                "MenuId=" & _
                                    Me.MenuId.ToString & "," & _
                                "SessionId=" & _
                                    CStr(Me.SessionId) & "," & _
                                "SaleDaySessionChangedBy=" & _
                                    Me.SaleDaySessionChangedBy.ToString & "," & _
                                "SaleDaySessionStatus=" & _
                                    CStr(Me.SaleDaySessionStatus) & "," & _
                                "SaleDaySessionChangedAt=" & _
                                    strSessionChangedAt & "," & _
                                "ReconciliationApprovedBy=" & _
                                   Me.ReconciliationApprovedBy.ToString & "," & _
                                "ReConciliationApprovedAt=" & _
                                    strApprovedAt & _
                                " where SaleDaySessionId =" & Me.SaleDaySessionId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
                    If intSQLReturnValue <= 0 Then
                        MsgBox("SQL Server Error - while updating SDSession")
                    Else
                        m_IsDirty = False
                    End If
            End Select
        End If

        If intSQLReturnValue < 0 Then
            'error
        Else
            intSQLReturnValue = Me.AllMoneyCounts.SetData(Me.SaleDaySessionId)

            If intSQLReturnValue < 0 Then
                MsgBox("SQL Server Error - while saving Moneycount")
            Else
                If blnResetOverrides = True Then ResetOverride()
                DataAccess.CreateDataAccess.EndTxn()
            End If
        End If

        Return intSQLReturnValue
    End Function

    Public ReadOnly Property AllOrders() As Orders
        Get
            If m_AllOrders Is Nothing Then
                m_AllOrders = New Orders(Orders.EnumView.CompleteView, Me.SaleDate, Me.SessionId, Orders.EnumFilter.All)
            End If
            Return m_AllOrders
        End Get
    End Property

    Public ReadOnly Property AllOrdersClosed() As Boolean
        Get
            Dim objOrder As Order
            Dim ordersClosed As Boolean
            ordersClosed = True
            For Each objOrder In Me.AllOrders
                If objOrder.PromisedAt < Me.SaleDaySessionTo Then       'exclude future orders
                    Select Case objOrder.PaidStatus
                        Case Order.enumOrderPaidStatus.FullyPaid
                            ordersClosed = True
                        Case Order.enumOrderPaidStatus.PartiallyPaid
                            ordersClosed = True
                        Case Order.enumOrderPaidStatus.MissingPayment
                            ordersClosed = True
                        Case Else
                            ordersClosed = False
                            Exit For
                    End Select
                End If
            Next
            Return ordersClosed
        End Get
    End Property

    Public Property ReconciliationApprovedBy() As Integer
        Get
            Return m_ReconciliationApprovedBy
        End Get
        Set(ByVal Value As Integer)
            m_ReconciliationApprovedBy = Value
            MarkDirty(True)
        End Set
    End Property
    Public Property SaleDayId() As Integer
        Get
            Return m_SaleDayId
        End Get
        Set(ByVal Value As Integer)
            m_SaleDayId = Value
            MarkDirty(True)
        End Set
    End Property
    Public Property ReconciliationApprovedAt() As DateTime
        Get
            Return m_ReconciliationApprovedAt
        End Get
        Set(ByVal Value As DateTime)
            Dim intSqlReturnValue As Integer

            m_ReconciliationApprovedAt = Value
            SetSaleDaySessionChangedAt()
            MarkDirty(True)
            intSqlReturnValue = Me.SetData(True)
            If intSqlReturnValue < 0 Then
                MsgBox("SQL Server Error - while saving Reconciliation Approval")
            Else
                'nope
            End If
        End Set
    End Property

    Private ReadOnly Property ReconciliaationOverrideContext() As String
        Get
            Dim strContext As String

            If Me.ReconciliationApprovedAt = Date.MinValue Then
                strContext = " Wanted to get Approval for " & Me.SessionName & " Reconciliation Report "
            ElseIf Me.ReconciliationApprovedAt > Date.MinValue Then
                strContext = " Wanted to cancel Approval for " & Me.SessionName & " Reconciliation Report "
            End If

            Return strContext
        End Get
    End Property

    Private Property ReconciliationReportOverride(ByVal objoverridetype As Override.enumOverrideType) As Override
        Get
            m_ReconciliationReportOverride = New Override()

            With m_ReconciliationReportOverride
                .OverrideType = objoverridetype
                .OverrideOldRowId = Me.SaleDaySessionId
                .OverrideNewRowId = Me.SaleDaySessionId
                .OverrideContext = ReconciliaationOverrideContext
                If Me.SaleDate.Date = Today.Date Then
                    .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                    ' .OverrideAvailable = True
                ElseIf Me.SaleDate.Date < Today.Date Then
                    .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                    '.OverrideAvailable = True
                End If
            End With
            Return m_ReconciliationReportOverride
        End Get
        Set(ByVal Value As Override)
            m_ReconciliationReportOverride = Value
        End Set
    End Property

    Public Sub getApproval()
        Dim objApprovalOverride As Override
        objApprovalOverride = ReconciliationReportOverride(Override.enumOverrideType.ReconciliationReportApproval)
        With objApprovalOverride
            Select Case .OverrideLevelNeeded
                Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                    RaiseEvent OverrideNeeded(objApprovalOverride)
                Case Else
                    'nop
            End Select
            If .OverrideAvailable = True Then
                ReconciliationApprovedBy = objApprovalOverride.OverrideBy
                'SaleDaySessionTo = SaleDays.CreateSaleDay.Now
                ReconciliationApprovedAt = SaleDays.CreateSaleDay.Now
            End If
        End With
    End Sub

    Public Sub CancelApproval()
        Dim objCancelApprovalOverride As Override
        objCancelApprovalOverride = ReconciliationReportOverride(Override.enumOverrideType.ReconciliationCancelApproval)
        With objCancelApprovalOverride
            Select Case .OverrideLevelNeeded
                Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                    RaiseEvent OverrideNeeded(objCancelApprovalOverride)
                Case Else
                    'nop
            End Select
            If .OverrideAvailable = True Then

                ReconciliationApprovedBy = objCancelApprovalOverride.OverrideBy
                ResetSaleDaySessionTo()
                ReconciliationApprovedAt = Date.MinValue
            End If
        End With
    End Sub

    Public ReadOnly Property MoneyCountEditAllowed(ByVal objMoneyCount As MoneyCount) As Boolean
        Get
            SetMoneyCountOverride(objMoneyCount)
            Dim objCashCountOverride As Override

            With m_objOverrideForMoneyCountEdit
                If .OverrideAvailable = True Then
                    Return True
                Else
                    Select Case .OverrideLevelNeeded
                        Case Override.enumOverrideLevel.NotNeeded
                            Return True
                        Case Else
                            Return False
                    End Select
                End If
            End With
        End Get
    End Property

    Private Sub SetMoneyCountOverride(ByVal objMoneyCount As MoneyCount)

        If (m_objOverrideForMoneyCountEdit Is Nothing) OrElse _
            (m_objOverrideForMoneyCountEdit.OverrideAvailable = False) Then
            m_objOverrideForMoneyCountEdit = OverrideForMoneyCountEdit(objMoneyCount)
            With m_objOverrideForMoneyCountEdit
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(m_objOverrideForMoneyCountEdit)
                        'nop here: caller will do the actual work
                    Case Else
                        '.OverrideAvailable = True
                        'nope
                End Select
            End With
        Else
            'Use an existing override to avoid repetitive requests for override
        End If
    End Sub

    Private ReadOnly Property OverrideForMoneyCountEdit(ByVal objMoneyCount As MoneyCount) As Override
        Get
            Dim objSaleDay As SaleDay
            Dim objSDSession As SaleDaySession
            Dim TimeSinceLastChange As Double
            Dim TimeNow As DateTime

            TimeNow = SaleDays.CreateSaleDay.Now
            m_objOverrideForMoneyCountEdit = New Override()
            With m_objOverrideForMoneyCountEdit
                .OverrideType = Override.enumOverrideType.EditMoneyCount
                .OverrideOldRowId = objMoneyCount.MoneyCountId
                .OverrideNewRowId = objMoneyCount.MoneyCountId
                Select Case objMoneyCount.MoneyCountType
                    Case MoneyCount.enumMoneyCountType.Closing
                        .OverrideContext = "Changed the  Cash Register Count of  " & Me.SessionName & " Closing"
                        If Me.SaleDaySessionTo <= DateTime.MinValue Then
                            TimeSinceLastChange = 0
                        Else
                            TimeSinceLastChange = TimeNow.Subtract(Me.SaleDaySessionTo).TotalMinutes
                        End If
                    Case MoneyCount.enumMoneyCountType.Opening
                        .OverrideContext = "Changed the  Cash Register Count of  " & Me.SessionName & " Opening"
                        If Me.SaleDaySessionFrom <= DateTime.MinValue Then
                            TimeSinceLastChange = 0
                        Else
                            TimeSinceLastChange = TimeNow.Subtract(Me.SaleDaySessionFrom).TotalMinutes
                        End If
                End Select
                Select Case Me.ReconciliationApprovedAt > DateTime.MinValue
                    Case True
                        .SetOverrideLevelNeeded(Override.enumOverrideLevel.OwnerNeeded)
                    Case False
                        'Select Case objMoneyCount.IsPersistedMoneyCount
                        Select Case TimeSinceLastChange
                            Case Is > 60
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                            Case Else
                                .SetOverrideLevelNeeded(Override.enumOverrideLevel.NotNeeded)
                        End Select
                End Select
            End With

            Return m_objOverrideForMoneyCountEdit
        End Get

    End Property

    Public ReadOnly Property TaxableRetailBaseRevenue() As Double
        Get
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objOrder In Me.AllOrders
                dblAmount += objOrder.TaxableRetailBaseRevenue
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property TaxExemptRetailBaseRevenue() As Double
        Get
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objOrder In Me.AllOrders
                dblAmount += objOrder.TaxExemptRetailBaseRevenue
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property TaxableHouseBaseRevenue() As Double
        Get
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objOrder In Me.AllOrders
                dblAmount += objOrder.TaxableHouseBaseRevenue
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property TaxExemptHouseBaseRevenue() As Double
        Get
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objOrder In Me.AllOrders
                dblAmount += objOrder.TaxExemptHouseBaseRevenue
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property NonRevenueSale() As Double
        Get
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objOrder In Me.AllOrders
                dblAmount += objOrder.NonRevenueSale
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property POSSaleAmount(ByVal fromTime As DateTime, ByVal ToTime As DateTime) As Double
        Get
            Dim objOrder As Order
            Dim dblPOSSaleAmount As Double

            For Each objOrder In Me.AllOrders
                If objOrder.PaidAt >= fromTime AndAlso objOrder.PaidAt <= ToTime Then
                    dblPOSSaleAmount += objOrder.POSAmount
                End If
            Next
            Return dblPOSSaleAmount
        End Get
    End Property

    Public ReadOnly Property SalesTaxCollected() As Double
        Get
            Dim objOrder As Order
            Dim dblAmount As Double

            For Each objOrder In Me.AllOrders
                dblAmount += objOrder.OrderSalesTaxAmount
            Next
            Return dblAmount
        End Get
    End Property

    Public ReadOnly Property RetailTipAmountPaid() As Double
        Get
            Dim objOrder As Order
            Dim dblTipAmountPaid As Double

            For Each objOrder In Me.AllOrders
                If objOrder.IsHouseACOrder Then
                    'nop
                Else
                    dblTipAmountPaid += objOrder.TipAmountPaid()
                End If
            Next
            Return dblTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property TipAmountPaid() As Double
        Get
            Dim objOrder As Order
            Dim dblTipAmountPaid As Double

            For Each objOrder In Me.AllOrders
                dblTipAmountPaid += objOrder.TipAmountPaid()
            Next
            Return dblTipAmountPaid
        End Get
    End Property
    Public ReadOnly Property TipAmountPaidOnTaxExemptSale() As Double
        Get
            Dim objOrder As Order
            Dim dblTipAmountPaid As Double

            For Each objOrder In Me.AllOrders
                Select Case objOrder.SalesTaxStatus
                    Case Order.enumOrderSalesTaxStatus.Taxable
                        'nop
                    Case Order.enumOrderSalesTaxStatus.NJTaxExempt, Order.enumOrderSalesTaxStatus.OutOfState
                        dblTipAmountPaid += objOrder.TipAmountPaid()
                End Select
            Next
            Return dblTipAmountPaid
        End Get
    End Property
    Public ReadOnly Property TipAmountPaidOnTaxableSale() As Double
        Get
            Dim objOrder As Order
            Dim dblTipAmountPaid As Double

            For Each objOrder In Me.AllOrders
                Select Case objOrder.SalesTaxStatus
                    Case Order.enumOrderSalesTaxStatus.Taxable
                        dblTipAmountPaid += objOrder.TipAmountPaid()
                    Case Order.enumOrderSalesTaxStatus.NJTaxExempt, Order.enumOrderSalesTaxStatus.OutOfState
                        'nop
                End Select
            Next
            Return dblTipAmountPaid
        End Get
    End Property

    Public ReadOnly Property TipAmountPaid(ByVal fromTime As DateTime, ByVal ToTime As DateTime) As Double
        Get
            Dim objOrder As Order
            Dim dblTipAmountPaid As Double

            For Each objOrder In Me.AllOrders
                If objOrder.PaidAt >= fromTime AndAlso objOrder.PaidAt <= ToTime Then
                    dblTipAmountPaid += objOrder.TipAmountPaid()
                End If
            Next
            Return dblTipAmountPaid
        End Get
    End Property
    Public ReadOnly Property PaymentAmountByMethod(ByVal pm As Integer) As Double
        Get
            Dim objOrder As Order
            Dim dblAmountPaid As Double

            For Each objOrder In Me.AllOrders
                If objOrder.VoidStatus = Order.enumOrderVoidStatus.NotVoided Then
                    dblAmountPaid += objOrder.PaymentAmountByMethod(pm)
                End If
            Next
            Return dblAmountPaid
        End Get
    End Property
    Public ReadOnly Property PosAmountByPaymentMethod(ByVal pm As Integer) As Double
        Get
            Dim objOrder As Order
            Dim dblAmountPaid As Double

            For Each objOrder In Me.AllOrders
                If objOrder.VoidStatus = Order.enumOrderVoidStatus.NotVoided Then
                    dblAmountPaid += objOrder.PosAmountByPaymentMethod(pm)
                End If
            Next
            Return dblAmountPaid
        End Get
    End Property
    Public ReadOnly Property TipAmountByPaymentMethod(ByVal pm As Integer) As Double
        Get
            Dim objOrder As Order
            Dim dblAmountPaid As Double

            For Each objOrder In Me.AllOrders
                If objOrder.VoidStatus = Order.enumOrderVoidStatus.NotVoided Then
                    dblAmountPaid += objOrder.TipAmountByPaymentMethod(pm)
                End If
            Next
            Return dblAmountPaid
        End Get
    End Property

    Public ReadOnly Property CalculatedClosingBalance() As Double
        Get
            'Return SubTotalForSession - CRTXnAmount
        End Get
    End Property

    Public ReadOnly Property OpeningMoneyCount() As MoneyCount
        Get
            Return Me.AllMoneyCounts.Item(MoneyCount.enumMoneyCountType.Opening)
        End Get
    End Property

    Public ReadOnly Property ClosingMoneyCount() As MoneyCount
        Get
            Return Me.AllMoneyCounts.Item(MoneyCount.enumMoneyCountType.Closing)
        End Get
    End Property

End Class
