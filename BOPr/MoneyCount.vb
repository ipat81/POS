Option Strict On
Imports System.Data.SqlClient

Public Class MoneyCount
    Inherits POSBO

    ' Public Event OverrideNeeded(ByVal newOverride As Override)
    Public Event MoneyCountChanged()

    Public Enum enumMoneyCountType
        ClosingBalancex
        OpeningBlancex
        CustomerCashPayment
        Change
        Opening
        Closing
    End Enum

    Public Enum enumMoneyCountStatus
        Active
        Inactive
        Voided
    End Enum
    Public Enum enumCashDenomination
        OneCentLoose
        OneCentRoll
        FiveCentLoose
        FiveCentRoll
        TenCentLoose
        TenCentRoll
        TwentyFiveCentLoose
        TwentyFiveCentRoll
        DollarOneCoin
        DollarOneBill
        DollarFiveBill
        DollarTenBill
        DollarTwentyBill
        DollarFiftyBill
        DollarHundredBill
    End Enum

    Public Enum enumCards
        Visa
        MasterCard
        Amex
        Discover
        DinersClub
        MGGiftCertificate
    End Enum

    Public Enum enumChecks
        PersonalCheck
        TravellersCheck
        GiftCertificate
        HouseAccount
    End Enum

    Public Enum enumMCOuntDEError
        ZeroCountForAmountEntered
        ZeroAmountForCountEntered
    End Enum


    Private m_MoneyCountId As Integer
    Private m_GiftCertificateCount As Integer
    Private m_GiftCertificateAmount As Double
    Private m_TravellersCheckCount As Integer
    Private m_TravellersCheckAmount As Double
    Private m_PersonalCheckCount As Integer
    Private m_PersonalCheckAmount As Double
    Private m_HouseAccountCount As Integer
    Private m_HouseAccountAmount As Double

    Private m_VisaCount As Integer
    Private m_VisaAmount As Double
    Private m_MasterCardCount As Integer
    Private m_MasterCardAmount As Double
    Private m_DiscoverCount As Integer
    Private m_DiscoverAmount As Double
    Private m_DinersClubCount As Integer
    Private m_DinersClubAmount As Double
    Private m_AmexCount As Integer
    Private m_AmexAmount As Double

    Private m_MoneyCountType As enumMoneyCountType
    Private m_MoneyCountParentId As Integer         'PKey of CRBalance,Payment,CRTxn associated with this money count
    Private m_MoneyCountChangedBy As Integer
    Private m_MoneyCountChangedAt As DateTime
    Private m_MoneyCountStatus As enumMoneyCountStatus
    Private m_MoneyCountAmountTendered As Double
    Private m_IsDirty As Boolean
    Private m_IsPersistedMoneyCount As Boolean      'True means existing row from SQL table

    Private m_MoneyCountSaleDate As DateTime
    Private m_MoneyCountSessionID As Integer
    Private m_MoneyCountSDSession As SaleDaySession
    Private m_objOverrideForMoneyCountEdit As Override
    Private m_DenominationCounts() As Integer

    Public Sub New(ByVal newmoneycountid As Integer)
        MoneyCountId = newmoneycountid
        ReDim m_DenominationCounts(16)


    End Sub

    Public Sub New()
        MoneyCountId = 0
        MoneyCountStatus = enumMoneyCountStatus.Active
        IsPersistedMoneyCount = False
        If m_DenominationCounts Is Nothing Then ReDim m_DenominationCounts(16)

    End Sub
    Friend Property MoneyCountId() As Integer

        Get
            Return m_MoneyCountId
        End Get

        Set(ByVal Value As Integer)
            m_MoneyCountId = Value
        End Set
    End Property

    Public Property MoneyCountParentId() As Integer

        Get
            Return m_MoneyCountParentId
        End Get

        Set(ByVal Value As Integer)
            m_MoneyCountParentId = Value
            'MarkDirty()
        End Set
    End Property

    Public Property DenominationCount(ByVal enumDenomination As enumCashDenomination) As Integer
        Get
            Return CType(m_DenominationCounts(CType(enumDenomination, Integer)), Integer)
        End Get

        Set(ByVal Value As Integer)
            If m_DenominationCounts(CType(enumDenomination, Integer)) <> Value Then
                m_DenominationCounts(CType(enumDenomination, Integer)) = Value
                'RaiseEvent MoneyCountChanged()
                MarkDirty()
            End If
        End Set

    End Property

    Public ReadOnly Property DenominationAmount(ByVal enumDenomination As enumCashDenomination) As Double
        Get
            Select Case enumDenomination
                Case enumCashDenomination.OneCentLoose
                    Return DenominationCount(enumDenomination) * 0.01
                Case enumCashDenomination.OneCentRoll
                    Return DenominationCount(enumDenomination) * 0.5
                Case enumCashDenomination.FiveCentLoose
                    Return DenominationCount(enumDenomination) * 0.05
                Case enumCashDenomination.FiveCentRoll
                    Return DenominationCount(enumDenomination) * 2.0
                Case enumCashDenomination.TenCentLoose
                    Return DenominationCount(enumDenomination) * 0.1
                Case enumCashDenomination.TenCentRoll
                    Return DenominationCount(enumDenomination) * 5.0
                Case enumCashDenomination.TwentyFiveCentLoose
                    Return DenominationCount(enumDenomination) * 0.25
                Case enumCashDenomination.TwentyFiveCentRoll
                    Return DenominationCount(enumDenomination) * 10.0
                Case enumCashDenomination.DollarOneCoin, enumCashDenomination.DollarOneBill
                    Return DenominationCount(enumDenomination) * 1.0
                Case enumCashDenomination.DollarFiveBill
                    Return DenominationCount(enumDenomination) * 5.0
                Case enumCashDenomination.DollarTenBill
                    Return DenominationCount(enumDenomination) * 10.0

                Case enumCashDenomination.DollarTwentyBill
                    Return DenominationCount(enumDenomination) * 20.0
                Case enumCashDenomination.DollarFiftyBill
                    Return DenominationCount(enumDenomination) * 50.0
                Case enumCashDenomination.DollarHundredBill
                    Return DenominationCount(enumDenomination) * 100.0

            End Select
        End Get
    End Property
    Public ReadOnly Property TotalCashAmount() As Double

        Get
            Dim dblTotalCashAmount As Double
            Dim i As Integer
            For i = 0 To m_DenominationCounts.Length - 1
                dblTotalCashAmount += DenominationAmount(CType(i, enumCashDenomination))
            Next
            If dblTotalCashAmount > 0 Then
                'nope
            Else
                dblTotalCashAmount = MoneyCountAmountTendered
            End If
            Return dblTotalCashAmount
        End Get

    End Property




    Public Property GiftCertificateCount() As Integer

        Get
            Return m_GiftCertificateCount
        End Get

        Set(ByVal Value As Integer)
            m_GiftCertificateCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set

    End Property



    Public Property GiftCertificateAmount() As Double


        Get
            Return m_GiftCertificateAmount
        End Get

        Set(ByVal Value As Double)
            m_GiftCertificateAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property TravellersCheckCount() As Integer

        Get
            Return m_TravellersCheckCount
        End Get

        Set(ByVal Value As Integer)
            m_TravellersCheckCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property TravellersCheckAmount() As Double
        Get
            Return m_TravellersCheckAmount
        End Get

        Set(ByVal Value As Double)
            m_TravellersCheckAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property HouseAccountCount() As Integer

        Get
            Return m_HouseAccountCount
        End Get

        Set(ByVal Value As Integer)
            m_HouseAccountCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property HouseAccountAmount() As Double
        Get
            Return m_HouseAccountAmount
        End Get

        Set(ByVal Value As Double)
            m_HouseAccountAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property




    Public Property PersonalCheckCount() As Integer
        Get
            Return m_PersonalCheckCount
        End Get

        Set(ByVal Value As Integer)
            m_PersonalCheckCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property PersonalCheckAmount() As Double

        Get
            Return m_PersonalCheckAmount
        End Get

        Set(ByVal Value As Double)
            m_PersonalCheckAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property VisaCount() As Integer
        Get
            Return m_VisaCount
        End Get

        Set(ByVal Value As Integer)
            m_VisaCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property VisaAmount() As Double

        Get
            Return m_VisaAmount
        End Get

        Set(ByVal Value As Double)
            m_VisaAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property MasterCardCount() As Integer
        Get
            Return m_MasterCardCount
        End Get

        Set(ByVal Value As Integer)
            m_MasterCardCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property MasterCardAmount() As Double

        Get
            Return m_MasterCardAmount
        End Get

        Set(ByVal Value As Double)
            m_MasterCardAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property DiscoverCount() As Integer
        Get
            Return m_DiscoverCount
        End Get

        Set(ByVal Value As Integer)
            m_DiscoverCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property



    Public Property DiscoverAmount() As Double

        Get
            Return m_DiscoverAmount
        End Get

        Set(ByVal Value As Double)
            m_DiscoverAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property DinersClubCount() As Integer
        Get
            Return m_DinersClubCount
        End Get

        Set(ByVal Value As Integer)
            m_DinersClubCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property DinersClubAmount() As Double

        Get
            Return m_DinersClubAmount
        End Get

        Set(ByVal Value As Double)
            m_DinersClubAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property AmexCount() As Integer
        Get
            Return m_AmexCount
        End Get

        Set(ByVal Value As Integer)
            m_AmexCount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property


    Public Property AmexAmount() As Double

        Get
            Return m_AmexAmount
        End Get

        Set(ByVal Value As Double)
            m_AmexAmount = Value
            RaiseEvent MoneyCountChanged()
            MarkDirty()
        End Set
    End Property

    Public Property MoneyCountType() As enumMoneyCountType
        Get
            Return m_MoneyCountType
        End Get
        Set(ByVal Value As enumMoneyCountType)
            m_MoneyCountType = Value

        End Set
    End Property

    Public ReadOnly Property MoneyCountChangedBy() As Integer
        Get
            Return m_MoneyCountChangedBy
        End Get
    End Property
    Private Sub SetMoneyCountChangedBy()
        'm_MoneyCountChangedBy = SaleDay.CreateSaleDay.CurrentCashier.EmployeeId
        m_MoneyCountChangedBy = 1
    End Sub

    Public ReadOnly Property MoneyCountChangedAt() As DateTime

        Get
            Return m_MoneyCountChangedAt
        End Get
    End Property

    Private Sub SetMoneyCountChangedAt()
        m_MoneyCountChangedAt = SaleDays.CreateSaleDay.Now
    End Sub



    Public Property MoneyCountStatus() As enumMoneyCountStatus
        Get
            Return m_MoneyCountStatus
        End Get
        Set(ByVal Value As enumMoneyCountStatus)
            m_MoneyCountStatus = Value

        End Set
    End Property
    Public Property MoneyCountAmountTendered() As Double
        Get
            Return m_MoneyCountAmountTendered
        End Get
        Set(ByVal Value As Double)
            m_MoneyCountAmountTendered = Value

        End Set
    End Property

    Public ReadOnly Property TotalCreditCardAmount() As Double
        Get

            Return (VisaAmount + MasterCardAmount + _
                    DinersClubAmount + DiscoverAmount + _
                    AmexAmount)
        End Get
    End Property

    Public ReadOnly Property TotalCheckAmount() As Double
        Get
            Return (PersonalCheckAmount + _
                    TravellersCheckAmount + _
                    GiftCertificateAmount + _
                    HouseAccountAmount)
        End Get
    End Property

    Public ReadOnly Property TotalAmount() As Double
        Get
            Return (TotalCreditCardAmount + TotalCheckAmount + _
                    TotalCashAmount)
        End Get
    End Property


    Public ReadOnly Property IsDirty() As Boolean
        Get
            Return m_IsDirty
        End Get

    End Property


    Public Sub ReadSqlDr(ByVal SqlDr As SqlDataReader)
        'Dim sqlcommand As String
        'Dim objDataAccess As DataAccess
        Dim SqlDrColName As String
        Dim i As Integer

        IsPersistedMoneyCount = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

            SqlDrColName = SqlDr.GetName(i) 'Reading  the column Name

            Select Case SqlDrColName

                Case "MoneyCountId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MoneyCountId = SqlDr.GetInt32(i)
                    End If

                Case "OneCentLoose"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.OneCentLoose) = SqlDr.GetInt16(i)
                    End If

                Case "OneCentRoll"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.OneCentRoll) = SqlDr.GetInt16(i)
                    End If

                Case "FiveCentLoose"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.FiveCentLoose) = SqlDr.GetInt16(i)
                    End If


                Case "FiveCentRoll"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.FiveCentRoll) = SqlDr.GetInt16(i)
                    End If

                Case "TenCentLoosee"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.TenCentLoose) = SqlDr.GetInt16(i)
                    End If


                Case "TenCentRoll"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.TenCentRoll) = SqlDr.GetInt16(i)
                    End If


                Case "TwentyFiveCentLoose"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.TwentyFiveCentLoose) = SqlDr.GetInt16(i)
                    End If


                Case "TwentyFiveCentRoll"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.TwentyFiveCentRoll) = SqlDr.GetInt16(i)
                    End If



                Case "DollarOneCoin"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarOneCoin) = SqlDr.GetInt16(i)
                    End If

                Case "DollarOneBill"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarOneBill) = SqlDr.GetInt16(i)
                    End If

                Case "DollarFiveBill"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarFiveBill) = SqlDr.GetInt16(i)
                    End If

                Case "DollarTenBill"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarTenBill) = SqlDr.GetInt16(i)
                    End If

                Case "DollarTwentyBill"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarTwentyBill) = SqlDr.GetInt16(i)
                    End If
                Case "DollarFiftyBill"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarFiftyBill) = SqlDr.GetInt16(i)
                    End If

                Case "DollarHundredBill"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DenominationCounts(enumCashDenomination.DollarHundredBill) = SqlDr.GetInt16(i)
                    End If


                Case "GiftCertificateCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_GiftCertificateCount = SqlDr.GetInt16(i)
                    End If

                Case "GiftCertificateAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_GiftCertificateAmount = SqlDr.GetDecimal(i)
                    End If

                Case "TravellersCheckCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_TravellersCheckCount = SqlDr.GetInt16(i)
                    End If

                Case "TravellersCheckAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_TravellersCheckAmount = SqlDr.GetDecimal(i)
                    End If

                Case "PersonalCheckCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_PersonalCheckCount = SqlDr.GetInt16(i)
                    End If

                Case "PersonalCheckAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_PersonalCheckAmount = SqlDr.GetDecimal(i)
                    End If

                Case "HouseAccountCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_HouseAccountCount = SqlDr.GetInt16(i)
                    End If

                Case "HouseAccountAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_HouseAccountAmount = SqlDr.GetDecimal(i)
                    End If



                Case "MoneyCountType"

                    If Not SqlDr.IsDBNull(i) Then
                        m_MoneyCountType = CType(SqlDr.GetInt16(i), MoneyCount.enumMoneyCountType)
                    End If

                Case "MoneyCountParentId"

                    If Not SqlDr.IsDBNull(i) Then
                        m_MoneyCountParentId = (SqlDr.GetInt32(i))
                    End If


                Case "MoneyCountChangedBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MoneyCountChangedBy = SqlDr.GetInt32(i)
                    Else
                        m_MoneyCountChangedBy = 0
                    End If

                Case "MoneyCountChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MoneyCountChangedAt = SqlDr.GetDateTime(i)
                    Else
                        m_MoneyCountChangedAt = Date.MinValue
                    End If

                Case "MoneyCountStatus "
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_MoneyCountStatus = CType(SqlDr.GetInt32(i), MoneyCount.enumMoneyCountStatus)

                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "MoneyCountAmountTendered"
                    If Not SqlDr.IsDBNull(i) Then
                        m_MoneyCountAmountTendered = SqlDr.GetDecimal(i)
                    Else
                        m_MoneyCountAmountTendered = 0
                    End If

                Case "VisaCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_VisaCount = SqlDr.GetInt16(i)
                    Else
                        m_VisaCount = 0
                    End If

                Case "VisaAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_VisaAmount = SqlDr.GetDecimal(i)
                    Else
                        m_VisaAmount = 0
                    End If

                Case "MasterCardCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_MasterCardCount = SqlDr.GetInt16(i)
                    Else
                        m_MasterCardCount = 0

                    End If

                Case "MasterCardAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_MasterCardAmount = SqlDr.GetDecimal(i)
                    Else
                        m_MasterCardAmount = 0
                    End If

                Case "DiscoverCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DiscoverCount = SqlDr.GetInt16(i)
                    Else
                        m_DiscoverCount = 0
                    End If

                Case "DiscoverAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DiscoverAmount = SqlDr.GetDecimal(i)
                    Else
                        m_DiscoverAmount = 0
                    End If


                Case "DinersClubCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DinersClubCount = SqlDr.GetInt16(i)
                    Else
                        m_DinersClubCount = 0
                    End If

                Case "DinersClubAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_DinersClubAmount = SqlDr.GetDecimal(i)
                    Else
                        m_DinersClubAmount = 0
                    End If

                Case "AmexCount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_AmexCount = SqlDr.GetInt16(i)
                    Else
                        m_AmexCount = 0

                    End If

                Case "AmexAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        m_AmexAmount = SqlDr.GetDecimal(i)
                    Else
                        m_AmexAmount = 0
                    End If

            End Select

        Next
        IsPersistedMoneyCount = True
        m_IsDirty = False
    End Sub
    Friend Property IsPersistedMoneyCount() As Boolean
        Get
            Return m_IsPersistedMoneyCount
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedMoneyCount = Value
        End Set
    End Property



    Friend Function SetData(ByVal intMoneyCountParentId As Integer) As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String

        If Me.IsDirty Then
            Select Case Me.IsPersistedMoneyCount      'True means update existing row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into MoneyCount values (" & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.OneCentLoose) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.OneCentRoll) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.FiveCentLoose) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.FiveCentRoll) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.TenCentLoose) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.TenCentRoll) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentLoose) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentRoll) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarOneCoin) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarOneBill) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarFiveBill) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarTenBill) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarTwentyBill) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarFiftyBill) & "," & _
                                Me.DenominationCount(MoneyCount.enumCashDenomination.DollarHundredBill) & "," & _
                                Me.GiftCertificateCount & "," & _
                                Me.GiftCertificateAmount & "," & _
                                Me.TravellersCheckCount & "," & _
                                Me.TravellersCheckAmount & "," & _
                                Me.PersonalCheckCount & "," & _
                                Me.PersonalCheckAmount & "," & _
                                Me.MoneyCountType & "," & _
                                intMoneyCountParentId.ToString & "," & _
                                CStr(Me.MoneyCountChangedBy) & ",'" & _
                                Me.MoneyCountChangedAt & "'," & _
                                CStr(Me.MoneyCountStatus) & "," & _
                                Me.MoneyCountAmountTendered.ToString & "," & _
                                Me.VisaCount & "," & _
                                Me.VisaAmount & "," & _
                                Me.MasterCardCount & "," & _
                                Me.MasterCardAmount & "," & _
                                Me.DiscoverCount & "," & _
                                Me.DiscoverAmount & "," & _
                                Me.DinersClubCount & "," & _
                                Me.DinersClubAmount & "," & _
                                Me.AmexCount & "," & _
                                Me.AmexAmount & "," & _
                                Me.HouseAccountCount & "," & _
                                Me.HouseAccountAmount & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.MoneyCountId = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update MoneyCount set  " & _
                                  "OneCentLoose = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.OneCentLoose).ToString & "," & _
                                "OneCentRoll = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.OneCentRoll).ToString & "," & _
                               "FiveCentLoose = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.FiveCentLoose).ToString & "," & _
                                "FiveCentRoll = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.FiveCentRoll).ToString & "," & _
                                "TenCentLoosee = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.TenCentLoose).ToString & "," & _
                                "TenCentRoll = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.TenCentRoll).ToString & "," & _
                               "TwentyFiveCentLoose = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentLoose).ToString & "," & _
                                "TwentyFiveCentRoll = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.TwentyFiveCentRoll).ToString & "," & _
                                "DollarOneCoin = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarOneCoin).ToString & "," & _
                                "DollarOneBill = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarOneBill).ToString & "," & _
                               "DollarFiveBill = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarFiveBill).ToString & "," & _
                                "DollarTenBill = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarTenBill).ToString & "," & _
                                "DollarTwentyBill = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarTwentyBill).ToString & "," & _
                               "DollarFiftyBill = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarFiftyBill).ToString & "," & _
                                "DollarHundredBill = " & _
                               Me.DenominationCount(MoneyCount.enumCashDenomination.DollarHundredBill).ToString & "," & _
                                "GiftCertificateCount = " & _
                               Me.GiftCertificateCount & "," & _
                                 "GiftCertificateAmount = " & _
                                 Me.GiftCertificateAmount & "," & _
                                 "TravellersCheckCount = " & _
                                 Me.TravellersCheckCount & "," & _
                                  "TravellersCheckAmount = " & _
                                 Me.TravellersCheckAmount & "," & _
                                  "PersonalCheckCount = " & _
                                 Me.PersonalCheckCount & "," & _
                                  "PersonalCheckAmount = " & _
                                 Me.PersonalCheckAmount & "," & _
                                  "VisaCount = " & _
                                 Me.VisaCount & "," & _
                                  "VisaAmount = " & _
                                 Me.VisaAmount & "," & _
                                  "MasterCardCount = " & _
                                 Me.MasterCardCount & "," & _
                                  "MasterCardAmount = " & _
                                 Me.MasterCardAmount & "," & _
                                  "DiscoverCount = " & _
                                 Me.DiscoverCount & "," & _
                                  "DiscoverAmount = " & _
                                 Me.DiscoverAmount & "," & _
                                  "DinersClubCount = " & _
                                 Me.DinersClubCount & "," & _
                                  "DinersClubAmount = " & _
                                 Me.DinersClubAmount & "," & _
                                     "AmexCount = " & _
                                 Me.AmexCount & "," & _
                                  "AmexAmount = " & _
                                 Me.AmexAmount & "," & _
                                   "HouseAccountCount = " & _
                                 Me.HouseAccountCount & "," & _
                                  "HouseAccountAmount = " & _
                                 Me.HouseAccountAmount & "," & _
                                   "MoneyCountChangedBy = " & _
                                 Me.MoneyCountChangedBy.ToString & "," & _
                                   "MoneyCountChangedAt='" & _
                                Me.MoneyCountChangedAt.ToString & "'," & _
                                   "MoneyCountStatus=" & _
                                CStr(Me.MoneyCountStatus) & _
                                " where MoneyCountId= " & Me.MoneyCountId.ToString & _
                               " And MoneyCountParentId = " & intMoneyCountParentId.ToString
                    ' ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select
            If intSQLReturnValue < 0 Then
                'NOP
            Else
                m_IsDirty = False
                Me.IsPersistedMoneyCount = True

            End If

        End If
        Return intSQLReturnValue
    End Function

    Private Sub MarkDirty()
        If IsDirty Then
            'NOP
        Else

            SetMoneyCountChangedAt()
            SetMoneyCountChangedBy()
            m_IsDirty = True
        End If
    End Sub

    Public Sub CopyStateFrom(ByVal objMoneyCountFrom As MoneyCount)
        Dim i As Integer
        Dim cashDenomination As enumCashDenomination

        With objMoneyCountFrom
            Me.AmexAmount = .AmexAmount
            Me.AmexCount = .AmexCount
            For i = 0 To m_DenominationCounts.Length - 1
                cashDenomination = CType(i, enumCashDenomination)
                Me.DenominationCount(cashDenomination) = .DenominationCount(cashDenomination)
            Next
            Me.DinersClubAmount = .DinersClubAmount
            Me.DinersClubCount = .DinersClubCount
            Me.DiscoverAmount = .DiscoverAmount
            Me.DiscoverCount = .DiscoverCount
            Me.GiftCertificateAmount = .GiftCertificateAmount
            Me.GiftCertificateCount = .GiftCertificateCount
            Me.MasterCardAmount = .MasterCardAmount
            Me.MasterCardCount = .MasterCardCount
            Me.MoneyCountAmountTendered = .MoneyCountAmountTendered
            Me.MoneyCountStatus = .MoneyCountStatus
            Me.MoneyCountType() = enumMoneyCountType.Opening
            Me.PersonalCheckAmount = .PersonalCheckAmount
            Me.PersonalCheckCount = .PersonalCheckCount
            Me.TravellersCheckAmount = .TravellersCheckAmount
            Me.TravellersCheckCount = .TravellersCheckCount
            Me.HouseAccountAmount = .HouseAccountAmount
            Me.HouseAccountCount = .HouseAccountCount
            Me.VisaAmount = .VisaAmount
            Me.VisaCount = .VisaCount
        End With
    End Sub
End Class

