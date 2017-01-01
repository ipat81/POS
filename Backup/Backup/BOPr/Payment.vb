Option Strict On
Imports System.Data.SqlClient

Public Class Payment
    Inherits POSBO
    Public Enum enumStatus
        Active
        InActive
        NewPayment
        Voided
    End Enum

    Public Enum enumPaymentMethod
        Cash
        Visa
        MasterCard
        Amex
        Discover
        DinersClub
        DebitCard
        MGGiftCertificate
        PersonalCheck
        TravellersChecks
        KnightExpress
        HouseAccount
        ShortPayment
        MissingPayment
    End Enum

    Public Enum enumPaymentDEError
        NoError
        ChkAmountExceedsPOSAmount
        AmountTenderedDoesNotMatchOtherAmounts
        TipsPaidAreLessThanOrderTips
        CustomerDoesNotHaveHouseAccount
        GuestCheckNotPrinted
    End Enum
    Private m_Payments As Payments

    Private m_PaymentId As Integer
    Private m_CRId As Integer
    Private m_PaymentMethod As enumPaymentMethod
    Private m_POSAmountPaid As Double
    Private m_TipAmountPaid As Double
    Private m_ChangeAmountReturned As Double
    Private m_BeginTime As DateTime
    Private m_EndTime As DateTime
    Private m_ChangedBy As Integer
    Private m_ChangedAt As DateTime
    Private m_CCInvoiceNo As Integer
    Private m_CCBatchNo As Integer
    Private m_CCRN As Integer
    Private m_AmountTendered As Double
    Private m_CCAuthorizationNo As Integer
    Private m_Status As enumStatus
    Private m_IsPersistedPayment As Boolean     'True means existing row from SQL server

    Private m_IsDirty As Boolean
    Private m_DEError As enumPaymentDEError
    Private m_objAllMoneyCounts As MoneyCounts


    Public Sub New()

    End Sub

    Public Property PaymentId() As Integer

        Get
            Return m_PaymentId
        End Get

        Set(ByVal Value As Integer)
            m_PaymentId = Value
        End Set
    End Property

    Public Property ParentPayments() As Payments

        Get
            Return m_Payments
        End Get

        Set(ByVal Value As Payments)
            m_Payments = Value
        End Set
    End Property

    Public ReadOnly Property CRID() As Integer

        Get
            Return SaleDays.CreateSaleDay.CRId
        End Get

    End Property

    Public Property PaymentMethod() As enumPaymentMethod
        Get
            Return m_PaymentMethod
        End Get
        Set(ByVal Value As enumPaymentMethod)
            m_PaymentMethod = Value
            MarkDirty()
        End Set
    End Property

    Public Property POSAmountPaid() As Double

        Get
            'If (Me.Status = enumStatus.Active) AndAlso (Me.DEError = enumPaymentDEError.NoError) Then
            Return Math.Round(m_POSAmountPaid, 2)
            'Else
            '    Return 0
            'End If
        End Get

        Set(ByVal Value As Double)
            m_POSAmountPaid = Value
            m_AmountTendered = m_POSAmountPaid + m_TipAmountPaid + m_ChangeAmountReturned
            MarkDirty()
        End Set
    End Property


    Public Property TipAmountPaid() As Double

        Get
            Return Math.Round(m_TipAmountPaid, 2)
        End Get

        Set(ByVal Value As Double)
            m_TipAmountPaid = Value
            Select Case Me.PaymentMethod
                Case enumPaymentMethod.Cash, enumPaymentMethod.TravellersChecks, enumPaymentMethod.MGGiftCertificate
                    If m_POSAmountPaid = m_AmountTendered Then
                        m_AmountTendered = m_POSAmountPaid + m_TipAmountPaid + m_ChangeAmountReturned
                    End If
                    m_ChangeAmountReturned = m_AmountTendered - m_POSAmountPaid - m_TipAmountPaid

                Case Else
                    m_ChangeAmountReturned = 0
                    m_AmountTendered = m_POSAmountPaid + m_TipAmountPaid
            End Select
            MarkDirty()
        End Set
    End Property

    Public Property AmountTendered() As Double
        Get
            Return Math.Round(m_AmountTendered, 2)
        End Get
        Set(ByVal Value As Double)
            m_AmountTendered = Value
            Select Case Me.PaymentMethod
                Case enumPaymentMethod.Cash, enumPaymentMethod.TravellersChecks, enumPaymentMethod.MGGiftCertificate
                    m_ChangeAmountReturned = m_AmountTendered - m_POSAmountPaid - m_TipAmountPaid
                    m_TipAmountPaid = m_AmountTendered - m_POSAmountPaid - m_ChangeAmountReturned

                Case Else
                    m_ChangeAmountReturned = 0
                    m_TipAmountPaid = m_AmountTendered - m_POSAmountPaid - m_ChangeAmountReturned
            End Select
            MarkDirty()
        End Set
    End Property

    Public Property ChangeAmountReturned() As Double
        Get
            Dim dblChangeAmount As Double
            Select Case Me.PaymentMethod
                Case enumPaymentMethod.Cash, enumPaymentMethod.TravellersChecks, enumPaymentMethod.MGGiftCertificate
                    Return Math.Round(m_ChangeAmountReturned, 2)
                Case Else
                    Return 0
            End Select
        End Get
        Set(ByVal Value As Double)
            m_ChangeAmountReturned = Value
            Select Case Me.PaymentMethod
                Case enumPaymentMethod.Cash, enumPaymentMethod.TravellersChecks, enumPaymentMethod.MGGiftCertificate
                    m_TipAmountPaid = m_AmountTendered - m_POSAmountPaid - m_ChangeAmountReturned

                Case Else
                    m_ChangeAmountReturned = 0
                    m_TipAmountPaid = m_AmountTendered - m_POSAmountPaid - m_ChangeAmountReturned
            End Select
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property DEError() As enumPaymentDEError
        Get
            Dim enumDEError As enumPaymentDEError
            enumDEError = enumPaymentDEError.NoError

            Select Case Me.Status
                Case enumStatus.Voided
                    'nop: return noerror
                Case Else
                    'Guest Check was not printed
                    Select Case Me.ParentPayments.ParentOrder.FirstCheckPrintedAt > DateTime.MinValue
                        Case True
                            'OK
                        Case Else
                            enumDEError = enumPaymentDEError.GuestCheckNotPrinted
                    End Select

                    'Customer must have a House Account if payment mehod= House Account 
                    Select Case PaymentMethod
                        Case enumPaymentMethod.HouseAccount
                            If ParentPayments.ParentOrder.Customer Is Nothing Then
                                enumDEError = enumPaymentDEError.CustomerDoesNotHaveHouseAccount
                            Else
                                Select Case ParentPayments.ParentOrder.Customer.CustomerType
                                    Case Customer.enumCustomerType.CorpAccount, Customer.enumCustomerType.PUAccount
                                        enumDEError = enumPaymentDEError.NoError
                                    Case Else
                                        enumDEError = enumPaymentDEError.CustomerDoesNotHaveHouseAccount
                                End Select
                            End If
                        Case Else
                            'enumDEError = enumPaymentDEError.NoError
                    End Select

                    'Chk Amount Paid must remain less than or equal to total POS amount
                    If ParentPayments.ParentOrder.BalancePOSAmount < 0 Then
                        enumDEError = enumPaymentDEError.ChkAmountExceedsPOSAmount
                        'If Tip are added to Order, Tip paid can not be less than TipType added to order
                    ElseIf Math.Abs((ParentPayments.ParentOrder.TipAmountPaid - ParentPayments.ParentOrder.TipAmount)) > Double.Epsilon AndAlso _
                            (ParentPayments.ParentOrder.TipAmountPaid < ParentPayments.ParentOrder.TipAmount) AndAlso _
                            (ParentPayments.ParentOrder.BalancePOSAmount = 0) Then
                        enumDEError = enumPaymentDEError.TipsPaidAreLessThanOrderTips
                    Else
                        'if all amounts are entered, they must add up to amount tendered 
                        Select Case PaymentMethod
                            Case enumPaymentMethod.Cash, enumPaymentMethod.TravellersChecks, enumPaymentMethod.MGGiftCertificate
                                Select Case (Math.Abs((m_AmountTendered - (m_POSAmountPaid + m_TipAmountPaid + m_ChangeAmountReturned))) >= 0.01)
                                    Case True
                                        If (m_POSAmountPaid > 0) AndAlso (m_AmountTendered > 0) AndAlso ((m_TipAmountPaid + m_ChangeAmountReturned > 0)) Then
                                            enumDEError = enumPaymentDEError.AmountTenderedDoesNotMatchOtherAmounts
                                        End If
                                End Select

                            Case Else
                                Select Case (Math.Abs((m_AmountTendered - (m_POSAmountPaid + m_TipAmountPaid))) >= 0.01)
                                    Case True
                                        If (m_POSAmountPaid > 0) AndAlso (m_AmountTendered > 0) AndAlso ((m_TipAmountPaid > 0)) Then
                                            enumDEError = enumPaymentDEError.AmountTenderedDoesNotMatchOtherAmounts
                                        End If
                                End Select
                                'If (m_POSAmountPaid > 0) AndAlso (m_AmountTendered > 0) AndAlso _
                                '     (m_TipAmountPaid > 0) AndAlso _
                                '     (Math.Abs((m_AmountTendered - (m_POSAmountPaid + m_TipAmountPaid))) >= 0.01) Then _
                                '    enumDEError = enumPaymentDEError.AmountTenderedDoesNotMatchOtherAmounts
                        End Select
                    End If
            End Select

            Return enumDEError
        End Get
    End Property

    'Public ReadOnly Property UpdatePaymentTipAmount() As Double
    '    Get
    '        Return m_AmountTendered - m_POSAmountPaid
    '    End Get

    'End Property

    Public ReadOnly Property BeginTime() As DateTime

        Get
            Return m_BeginTime
        End Get
    End Property
    Private Sub SetBeginTime()
        If m_BeginTime > DateTime.MinValue Then
            'nop
        Else
            m_BeginTime = SaleDays.CreateSaleDay.Now
            MarkDirty()
        End If
    End Sub

    Public Property EndTime() As DateTime

        Get
            Return m_EndTime
        End Get
        Set(ByVal Value As DateTime)
            If m_EndTime <= DateTime.MinValue Then m_EndTime = Value
            'SetChangedAt()
            MarkDirty()
        End Set

    End Property

    Public ReadOnly Property ChangedBy() As Integer
        Get
            Return m_ChangedBy
        End Get
    End Property
    Private Sub setChangedBy()
        'm_ChangedBy =  SaleDays.CreateSaleDay.CurrentSaleDay.CurrentCashier.EmployeeId
        'm_ChangedBy = 1
    End Sub

    Public ReadOnly Property ChangedAt() As DateTime
        Get
            Return m_ChangedAt
        End Get
    End Property

    Private Sub SetChangedAt()
        m_ChangedAt = SaleDays.CreateSaleDay.Now
    End Sub

    Public Property CCInvoiceNo() As Integer

        Get
            Return m_CCInvoiceNo
        End Get

        Set(ByVal Value As Integer)
            m_CCInvoiceNo = Value
            MarkDirty()
        End Set

    End Property


    Public Property CCBatchNo() As Integer
        Get
            Return m_CCBatchNo
        End Get

        Set(ByVal Value As Integer)
            m_CCBatchNo = Value
            MarkDirty()
        End Set
    End Property

    Public Property CCRN() As Integer
        Get
            Return m_CCRN
        End Get

        Set(ByVal Value As Integer)
            m_CCRN = Value
            MarkDirty()
        End Set
    End Property

    Public Property CCAuthorizationNo() As Integer
        Get
            Return m_CCAuthorizationNo
        End Get

        Set(ByVal Value As Integer)
            m_CCAuthorizationNo = Value
            MarkDirty()
        End Set
    End Property

    Public Property Status() As enumStatus
        Get
            Return m_Status
        End Get

        Set(ByVal Value As enumStatus)
            m_Status = Value
            Select Case Value
                Case enumStatus.NewPayment
                    'nop
                Case Else
                    MarkDirty()
            End Select
        End Set
    End Property

    Public ReadOnly Property AllMoneyCounts() As MoneyCounts
        Get
            If m_objAllMoneyCounts Is Nothing Then m_objAllMoneyCounts = New MoneyCounts()
            Return m_objAllMoneyCounts
        End Get
    End Property

    ReadOnly Property IsDirty() As Boolean
        Get
            Return m_IsDirty
        End Get
    End Property

    Friend Sub ReadSqlDr(ByVal SqlDr As SqlDataReader)
        Dim SqlDrColName As String
        Dim sqlcommand As String
        Dim objDataAccess As DataAccess

        Dim i As Integer
        Dim msg As String

        IsPersistedPayment = True

        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record
            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName       'loops through the records in the SqlDr.
                Case "PaymentID"
                    If Not SqlDr.IsDBNull(i) Then
                        m_PaymentId = SqlDr.GetInt32(i)
                    End If
                    'Case "OrderID"

                    '    If Not SqlDr.IsDBNull(i) Then
                    '        OrderId = SqlDr.GetInt32(i)
                    '    End If
                Case "PaymentMethod"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_PaymentMethod = CType(SqlDr.GetInt32(i), Payment.enumPaymentMethod)
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "POSAmountPaid"
                    If Not SqlDr.IsDBNull(i) Then
                        m_POSAmountPaid = SqlDr.GetDecimal(i)
                    Else
                        m_POSAmountPaid = 0
                    End If
                Case "TipAmountPaid"
                    If Not SqlDr.IsDBNull(i) Then
                        m_TipAmountPaid = SqlDr.GetDecimal(i)
                    Else
                        m_TipAmountPaid = 0
                    End If
                Case "AmountTendered"
                    If Not SqlDr.IsDBNull(i) Then
                        m_AmountTendered = SqlDr.GetDecimal(i)
                    Else
                        m_AmountTendered = 0
                    End If
                Case "ChangeAmountReturned"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ChangeAmountReturned = SqlDr.GetDecimal(i)
                    Else
                        m_ChangeAmountReturned = 0
                    End If
                Case "BeginTime"
                    If Not SqlDr.IsDBNull(i) Then
                        m_BeginTime = SqlDr.GetDateTime(i)
                    End If
                Case "EndTime"
                    If Not SqlDr.IsDBNull(i) Then
                        m_EndTime = SqlDr.GetDateTime(i)
                    End If
                Case "ChangedBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ChangedBy = SqlDr.GetInt32(i)
                    Else
                        m_ChangedBy = 0
                    End If
                Case "ChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_ChangedAt = SqlDr.GetDateTime(i)
                    Else
                        m_ChangedAt = Date.MinValue
                    End If
                Case "CCInvoiceNo"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CCInvoiceNo = SqlDr.GetInt32(i)
                    End If
                Case "CCBatchNo"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CCBatchNo = SqlDr.GetInt32(i)
                    End If
                Case "CCRN"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CCRN = SqlDr.GetInt32(i)
                    End If
                Case "CCAuthorizationNo"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CCAuthorizationNo = SqlDr.GetInt32(i)
                    End If
                Case "Status"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_Status = CType(SqlDr.GetInt32(i), Payment.enumStatus)
                        Else
                            m_Status = Payment.enumStatus.Active
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "DEError"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_DEError = CType(SqlDr.GetInt32(i), Payment.enumPaymentDEError)
                        Else
                            m_DEError = Payment.enumPaymentDEError.NoError
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
            End Select
        Next

        m_IsDirty = False
    End Sub

    Friend Property IsPersistedPayment() As Boolean
        Get
            Return m_IsPersistedPayment
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedPayment = Value
        End Set
    End Property

    Private Sub MarkDirty()
        If IsDirty Then
            'NOP
        Else
            SetChangedAt()
            setChangedBy()
            m_IsDirty = True
        End If
    End Sub

    Friend Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        Dim strEndTime As String
        Dim strChangedAt As String

        If Me.EndTime = Date.MinValue Then
            strEndTime = "NULL"
        Else
            strEndTime = "'" & Me.EndTime.ToString & "'"
        End If
        If Me.ChangedAt = Date.MinValue Then
            strChangedAt = "NULL"
        Else
            strChangedAt = "'" & Me.ChangedAt.ToString & "'"
        End If
        If Me.IsDirty Then
            Select Case Me.IsPersistedPayment       'True means update existing row, False means insert row in SQL table
                Case False
                    strSqlCmd = "insert into Payment values (" & _
                                Me.ParentPayments.ParentOrder.OrderId.ToString & "," & _
                                SaleDays.CreateSaleDay.CRId.ToString & "," & _
                                CStr(Me.PaymentMethod) & "," & _
                                Me.POSAmountPaid.ToString & "," & _
                                Me.TipAmountPaid.ToString & "," & _
                                Me.AmountTendered.ToString & "," & _
                                Me.ChangeAmountReturned.ToString & ",'" & _
                                Me.BeginTime & "'," & _
                                strEndTime & "," & _
                                Me.ChangedBy.ToString & "," & _
                                strChangedAt & "," & _
                                Me.CCInvoiceNo.ToString & "," & _
                                Me.CCBatchNo.ToString & "," & _
                                Me.CCRN.ToString & "," & _
                                Me.CCAuthorizationNo.ToString & "," & _
                                CStr(Me.Status) & "," & _
                                CStr(Me.DEError) & _
                                ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                Case Else
                    strSqlCmd = "update Payment set  " & _
                                    "PaymentMethod =" & _
                                        CStr(Me.PaymentMethod) & "," & _
                                    "POSAmountPaid =" & _
                                        Me.POSAmountPaid.ToString & "," & _
                                    "TipAmountPaid =" & _
                                        Me.TipAmountPaid.ToString & "," & _
                                    "AmountTendered =" & _
                                        Me.AmountTendered.ToString & "," & _
                                    "ChangeAmountReturned =" & _
                                        Me.ChangeAmountReturned.ToString & "," & _
                                    "BeginTime ='" & _
                                        Me.BeginTime & "'," & _
                                    "EndTime =" & _
                                        strEndTime & "," & _
                                    "ChangedBy=" & _
                                        Me.ChangedBy.ToString & "," & _
                                    "ChangedAt=" & _
                                        strChangedAt & "," & _
                                    "CCInvoiceNo=" & _
                                        Me.CCInvoiceNo.ToString & "," & _
                                    "CCBatchNo=" & _
                                        Me.CCBatchNo.ToString & "," & _
                                    "CCRRN=" & _
                                        Me.CCRN.ToString & "," & _
                                    "CCAuthorizationNo=" & _
                                        Me.CCAuthorizationNo.ToString & "," & _
                                    "Status=" & _
                                        CStr(Me.Status) & "," & _
                                     "DEError=" & _
                                        CStr(Me.DEError) & _
                                    " where PaymentId=" & Me.PaymentId
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)

            End Select
            If intSQLReturnValue < 0 Then
                'NOP
            Else
                Me.PaymentId = intSQLReturnValue
                m_IsDirty = False
                Me.IsPersistedPayment = True
            End If
        End If
        Return intSQLReturnValue
    End Function
End Class

