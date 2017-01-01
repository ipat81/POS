Option Strict On
Imports System.Data.SqlClient

Public Class CRTXN
    Inherits POSBO

    Public Event OverrideNeeded(ByVal newOverride As Override)
    Public Enum enumCRTXNType
        'ClosingBalance
        'OpeningBalance
        'CustomerPayment
        DepositToOfficeSafe
        PurchaseAdvance
        ExpenseReimbursement
        StaffAdvance
        AdvanceCashReturn
        PettyExpenseAdvance
        PettyExpenseReimbursement
        StaffSalaryAdvance
    End Enum

    Public Enum enumCRTXNStatus

        Active
        InActive
        Voided
        NewCrTXn
    End Enum

    Public Enum enumCashRegisterIds
        CR99
        CR98
        CR1
        CR2
    End Enum


    Private m_CRTXNId As Integer
    Private m_CRId As Integer
    Private m_CRTXNType As enumCRTXNType
    Private m_CRTXNAmount As Decimal
    Private m_CRTXNBy As Integer
    Private m_CRTXNAt As DateTime
    Private m_CRTXNOKBy As Integer
    Private m_CRTXNOKAt As DateTime
    Private m_CRTXNChangedBy As Integer
    Private m_CRTXNChangedAt As DateTime
    Private m_OtherCRTXNId As Integer
    Private m_CRTXNComments As String
    Private m_CRTXNStatus As enumCRTXNStatus
    Private m_IsDirty As Boolean
    Private m_ISPersistedCRTXN As Boolean
    Private m_CRTXnReceiptGiven As Boolean
    Private m_objOverrideForCRTXnEdit As Override


    Public Sub New()
        CRTXNId = 0
        m_ISPersistedCRTXN = False
    End Sub

    Public Sub New(ByVal newcrtxnid As Integer)
        CRTXNId = newcrtxnid
    End Sub


    'Friend Sub New(ByVal newcrtxnid As Integer, ByVal newcrid As Integer, _
    '                ByVal newcrtxntype As enumCRTXNType, ByVal newcrtxnamount As Decimal, _
    '                ByVal newcrtxnby As Integer, ByVal newcrtxnokby As Integer, _
    '                ByVal newcrtxnokat As DateTime, ByVal newcrtxnchangedby As Integer, _
    '                ByVal newcrtxnchangedat As DateTime, ByVal newcrtxncomments As String, _
    '                ByVal newcrtxnStatus As enumCRTXNStatus)

    '    CRTXNId = newcrtxnid
    '    CRID = newcrid
    '    CRTXNType = newcrtxntype
    '    CRTXNAmount = newcrtxnamount
    '    CRTXNBy = newcrtxnby
    '    CRTXNOKBy = newcrtxnokby
    '    CRTXNOKAt = newcrtxnokat
    '    CRTXNChangedBy = newcrtxnchangedby
    '    CRTXNChangedAt = newcrtxnchangedat
    '    CRTXNComments = newcrtxncomments
    '    CRTXNStatus = newcrtxnStatus


    'End Sub

    Public Property CRTXNId() As Integer

        Get
            Return m_CRTXNId
        End Get

        Set(ByVal Value As Integer)
            m_CRTXNId = Value
        End Set
    End Property


    Public Property CRID() As Integer

        Get
            Return m_CRId
        End Get

        Set(ByVal Value As Integer)
            m_CRId = Value

        End Set

    End Property

    Public Property CRTXNType() As enumCRTXNType

        Get
            Return m_CRTXNType
        End Get

        Set(ByVal Value As enumCRTXNType)
            m_CRTXNType = Value
            MarkDirty()
        End Set

    End Property




    Public Property CRTXNAmount() As Decimal

        Get
            Return m_CRTXNAmount
        End Get

        Set(ByVal Value As Decimal)
            m_CRTXNAmount = Value
            MarkDirty()
        End Set
    End Property

    Public Property CRTXNBy() As Integer
        Get
            Return m_CRTXNBy
        End Get

        Set(ByVal Value As Integer)

            m_CRTXNBy = Value
            MarkDirty()
        End Set


    End Property


    Public Property CRTXNAt() As DateTime

        Get
            Return m_CRTXNAt
        End Get

        Set(ByVal Value As DateTime)
            m_CRTXNAt = Value
            ' MarkDirty()

        End Set
    End Property


    Public ReadOnly Property CRTXNOKBy() As Integer
        Get
            Return m_CRTXNOKBy
        End Get

    End Property
    Private Sub SetCRTXNOKBy()
        'm_CRTXNOKBy = SaleDays.CreateSaleDay.CurrentSaleDay.CurrentCashier. _
        '                EmployeeId

    End Sub


    Public ReadOnly Property CRTXNOKAt() As DateTime

        Get
            Return m_CRTXNOKAt
        End Get

    End Property

    Private Sub SetCRTXNOKAt()
        m_CRTXNOKAt = SaleDays.CreateSaleDay.Now

    End Sub

    Public ReadOnly Property CRTXNChangedBy() As Integer

        Get
            Return m_CRTXNChangedBy
        End Get
    End Property

    Private Sub SetCRTXNChangedBy()
        'm_CRTXNChangedBy = SaleDays.CreateSaleDay.CurrentSaleDay. _
        '                    CurrentCashier.EmployeeId

    End Sub



    Public ReadOnly Property CRTXNChangedAt() As DateTime

        Get
            Return m_CRTXNChangedAt
        End Get
    End Property

    Private Sub SetCRTXNChangedAt()
        m_CRTXNChangedAt = SaleDays.CreateSaleDay.Now

    End Sub

    'Public Property OtherCRTXNId() As Integer
    '    Get
    '        Return m_OtherCRTXNId
    '    End Get
    '    Set(ByVal Value As Integer)
    '        m_OtherCRTXNId = Value

    '    End Set
    'End Property



    Public Property CRTXNComments() As String


        Get
            Return m_CRTXNComments
        End Get

        Set(ByVal Value As String)
            m_CRTXNComments = Value
            MarkDirty()
        End Set
    End Property

    Public Property CRTXNStatus() As enumCRTXNStatus


        Get
            Return m_CRTXNStatus
        End Get

        Set(ByVal Value As enumCRTXNStatus)
            m_CRTXNStatus = Value
            If (m_CRTXNStatus = enumCRTXNStatus.Active) Or _
                (m_CRTXNStatus = enumCRTXNStatus.Voided) Then
                MarkDirty()
            End If
        End Set
    End Property

    Public Property CRTXNReceiptGiven() As Boolean
        Get
            Return m_CRTXnReceiptGiven
        End Get
        Set(ByVal Value As Boolean)
            m_CRTXnReceiptGiven = Value
            MarkDirty()
        End Set
    End Property

    ReadOnly Property IsDirty() As Boolean

        Get
            Return m_IsDirty
        End Get

    End Property

    Private Sub MarkDirty()
        If m_IsDirty Then
            'NOP
        Else
            m_IsDirty = True


            '  SetCRTXNAT()
            'SetCRTXNBy()
            SetCRTXNChangedAt()
            SetCRTXNChangedBy()
            SetCRTXNOKBy()
            SetCRTXNOKAt()
        End If

    End Sub


    'Public Sub GetData() '(ByVal enumCRTXNView As SaleDays.EnumView)
    '    Dim sqlcommand As String
    '    Dim SqlDr As SqlDataReader
    '    Dim SqlDrColName As String
    '    Dim objDataAccess As DataAccess

    '    Dim msg As String
    '    Dim i As Integer

    '    objDataAccess = DataAccess.CreateDataAccess


    '    'Select Case enumSaleDayView

    '    '    Case SaleDays.EnumView.BaseView
    '    '        sqlcommand = "Select SaleDayId,SaleDate From SaleDay"

    '    '    Case SaleDays.EnumView.CompleteView
    '    '        sqlcommand = "Select * from SaleDay"

    '    'End Select

    '    sqlcommand = sqlcommand & " where  SaleDayId =  " & Me.CRTXNId
    '    objDataAccess = DataAccess.CreateDataAccess

    '    'GetData method connects to the database fetches  the record
    '    SqlDr = objDataAccess.GetData(sqlcommand)

    '    If Not SqlDr Is Nothing Then

    '        Do While SqlDr.Read()       'loops through the records in the SqlDr.

    '            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

    '                SqlDrColName = SqlDr.GetName(i) 'Reading  the column Name

    '                Select Case SqlDrColName


    '                    Case "CRId"

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRID = SqlDr.GetInt32(i)
    '                        End If


    '                    Case "CRTXNType"
    '                        Try
    '                            If Not SqlDr.IsDBNull(i) Then
    '                                CRTXNType = CType(SqlDr.GetInt16(i), CRTXN.enumCRTXNType)
    '                            End If
    '                        Catch
    '                            '???errormessage???
    '                        End Try

    '                    Case "CRTXNAmount"

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRTXNAmount = SqlDr.GetDecimal(i)
    '                        End If


    '                    Case "CRTXNBy"   ' Cashier who did the CRTXN

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRTXNBy = SqlDr.GetInt32(i)
    '                        End If



    '                    Case "CRTXNAt"   'Time when manager did the CRTXN

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRTXNAt = SqlDr.GetDateTime(i)
    '                        End If

    '                    Case "CRTXNChangedBy"
    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRTXNChangedBy = SqlDr.GetInt32(i)
    '                        End If

    '                    Case "CRTXNChangedAt"
    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRTXNChangedAt = SqlDr.GetDateTime(i)
    '                        End If

    '                    Case "CRTXNComments"

    '                        If Not SqlDr.IsDBNull(i) Then
    '                            CRTXNComments = SqlDr.GetString(i)

    '                        End If


    '                    Case "CRTXNStatus"
    '                        Try
    '                            If Not SqlDr.IsDBNull(i) Then
    '                                CRTXNStatus = CType(SqlDr.GetInt16(i), CRTXN.enumCRTXNStatus)
    '                            End If
    '                        Catch
    '                            '???errormessage???
    '                        End Try
    '                End Select

    '            Next
    '        Loop
    '    End If

    'End Sub
    Private Property ISPersistedCRTXN() As Boolean
        Get
            Return m_ISPersistedCRTXN
        End Get
        Set(ByVal Value As Boolean)
            m_ISPersistedCRTXN = Value
        End Set
    End Property


    Friend Sub ReadSqlDr(ByVal SqlDr As SqlDataReader)
        Dim SqlDrColName As String
        Dim i As Integer

        'If Not SqlDr Is Nothing Then
        'Do While SqlDr.Read()       'loops through the records in the sqlReader.


        ISPersistedCRTXN = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName
                Case "CRTXNId"
                    If Not SqlDr.IsDBNull(i) Then
                        CRTXNId = SqlDr.GetInt32(i)
                    End If

                Case "CRId"

                    If Not SqlDr.IsDBNull(i) Then
                        CRID = SqlDr.GetInt32(i)
                    End If

                Case "CRTXNType"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            CRTXNType = CType(SqlDr.GetInt32(i), CRTxn.enumCRTXNType)
                        End If
                    Catch
                        '???errormessage???
                    End Try

                Case "CRTXNAmount"

                    If Not SqlDr.IsDBNull(i) Then
                        CRTXNAmount = SqlDr.GetDecimal(i)
                    End If


                Case "CRTXNBy"   ' Cashier who did the CRTXN

                    If Not SqlDr.IsDBNull(i) Then
                        m_CRTXNBy = SqlDr.GetInt32(i)
                    Else
                        m_CRTXNBy = 0
                    End If


                Case "CRTXNAt"   'Time when manager did the CRTXN

                    If Not SqlDr.IsDBNull(i) Then
                        m_CRTXNAt = SqlDr.GetDateTime(i)
                    Else
                        m_CRTXNAt = Date.MinValue
                    End If

                Case "CRTXNChangedBy"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CRTXNChangedBy = SqlDr.GetInt32(i)
                    Else
                        m_CRTXNChangedBy = 0
                    End If

                Case "CRTXNChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        m_CRTXNChangedAt = SqlDr.GetDateTime(i)
                    Else
                        m_CRTXNChangedAt = Date.MinValue
                    End If

                Case "OtherCRTXNId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_OtherCRTXNId = SqlDr.GetInt32(i)
                    Else
                        m_OtherCRTXNId = 0
                    End If


                Case "CRTXNComments"

                    If Not SqlDr.IsDBNull(i) Then
                        CRTXNComments = SqlDr.GetString(i)

                    End If


                Case "CRTXNStatus"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            CRTXNStatus = CType(SqlDr.GetInt16(i), CRTxn.enumCRTXNStatus)
                        End If
                    Catch
                        '???errormessage???
                    End Try
            End Select
        Next
        ISPersistedCRTXN = True
        m_IsDirty = False
    End Sub



    Public ReadOnly Property CRTXnEditAllowed(ByVal objCrTxn As CRTxn) As Boolean
        Get
            Dim objSaleDay As SaleDay
            Dim objSDSession As SaleDaySession

            If objCrTxn.CRTXNStatus = CRTXN.enumCRTXNStatus.NewCrTXn Then
                SetCRTXNOverride()
            ElseIf objCrTxn.CRTXNAt < Today Then
                Return False
            Else
                objSaleDay = SaleDays.CreateSaleDay.Item(objCrTxn.CRTXNAt.Date, False)
                For Each objSDSession In objSaleDay.AllSaleDaySessions
                    If (objCrTxn.CRTXNAt < objSDSession.SaleDaySessionFrom) Or _
                       (objCrTxn.CRTXNAt >= objSDSession.SaleDaySessionFrom And _
                        objCrTxn.CRTXNAt <= objSDSession.SaleDaySessionTo) Or _
                       (objCrTxn.CRTXNAt > objSDSession.SaleDaySessionFrom And _
                        objSDSession.SaleDaySessionTo = Date.MinValue) Then
                        Select Case objSDSession.ReconciliationApprovedAt > Date.MinValue
                            Case True
                                Return False
                            Case False
                                SetCRTXNOverride()
                        End Select

                    Else
                        'nope
                    End If

                Next
                If objSaleDay.AllSaleDaySessions.Count = 0 Then
                    SetCRTXNOverride()
                End If
                If m_objOverrideForCRTXnEdit Is Nothing Then
                    Return False
                ElseIf m_objOverrideForCRTXnEdit.OverrideAvailable Then
                    Return True

                End If
            End If

            With m_objOverrideForCRTXnEdit
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


    Private Sub SetCRTXNOverride()

        If (m_objOverrideForCRTXnEdit Is Nothing) OrElse _
            (m_objOverrideForCRTXnEdit.OverrideAvailable = False) Then
            m_objOverrideForCRTXnEdit = OverrideForCRTXNEdit
            With m_objOverrideForCRTXnEdit
                Select Case .OverrideLevelNeeded
                    Case Override.enumOverrideLevel.ManagerNeeded, Override.enumOverrideLevel.OwnerNeeded
                        RaiseEvent OverrideNeeded(m_objOverrideForCRTXnEdit)
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

    Private ReadOnly Property CRTXNOverrideContext() As String
        Get
            Dim strContext As New System.Text.StringBuilder()
            If Me.CRID = 0 Then
                strContext.Append(" Attempting to create a new Cash Register Transaction ")
            Else
                strContext.Append(" Changes made to a Cash Register Transaction ")

            End If
            Return strContext.ToString
        End Get
    End Property

    Private ReadOnly Property OverrideForCRTXNEdit() As Override
        Get
            'Dim objSaleDay As SaleDay
            'Dim objSDSession As SaleDaySession

            m_objOverrideForCRTXnEdit = New Override()
            With m_objOverrideForCRTXnEdit
                .OverrideType = Override.enumOverrideType.EditCRTXN
                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)
                .OverrideOldRowId = Me.CRTXNId
                .OverrideNewRowId = Me.CRTXNId
                .OverrideContext = Me.CRTXNOverrideContext()
            End With

            Return m_objOverrideForCRTXnEdit
        End Get

    End Property

    Private Sub ResetOverride()
        'Make sure that no root level Overrides can be used again after Setdata
        If m_objOverrideForCRTXnEdit Is Nothing Then
            'nop
        Else

            m_objOverrideForCRTXnEdit = Nothing
        End If


    End Sub




    Friend Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        'Dim strCashierSignOutAt As String
        'Dim strCRClosingBalanceChangedAt As String
        'Dim strCRBalanceOkAt As String
        Dim strReceiptGiven As String
        If Me.CRTXNReceiptGiven = True Then
            strReceiptGiven = "1"
        ElseIf Me.CRTXNReceiptGiven = False Then
            strReceiptGiven = "0"
        End If




        If Me.IsDirty Then
            Select Case Me.ISPersistedCRTXN     'True means update existing row, False means insert row in SQL table 
                Case False
                    strSqlCmd = "insert into CRTXN values (" & Me.CRID.ToString & "," & _
                                CStr(Me.CRTXNType) & "," & _
                                Me.CRTXNAmount.ToString & "," & _
                                Me.CRTXNBy & ",'" & _
                                Me.CRTXNAt.ToString & "'," & _
                                Me.CRTXNOKBy.ToString & ",'" & _
                                Me.CRTXNOKAt.ToShortDateString & "'," & _
                                Me.CRTXNChangedBy.ToString & ",'" & _
                                Me.CRTXNChangedAt & "','" & _
                                Me.CRTXNComments & " '," & _
                                CStr(Me.CRTXNStatus) & " ," & _
                                strReceiptGiven & _
                                " )"

                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.CRTXNId() = intSQLReturnValue
                    End If
                Case Else
                    strSqlCmd = "update CRTXN set  " & _
                                "CRId = " & _
                                Me.CRID & "," & _
                                "CRTXNType = " & _
                                   CStr(Me.CRTXNType) & "," & _
                                "CRTXNAmount = " & _
                                    Me.CRTXNAmount.ToString & "," & _
                                "CRTXNOKBy=" & _
                                    Me.CRTXNOKBy.ToString & "," & _
                                "CRTXNOKAt='" & _
                                    Me.CRTXNOKAt.ToString & "'," & _
                                 "CRTXNChangedBy=" & _
                                    Me.CRTXNChangedBy.ToString & "," & _
                                "CRTXNChangedAt='" & _
                                    Me.CRTXNChangedAt.ToString & "'," & _
                                "CRTXNComments = '" & _
                                     Me.CRTXNComments & "', " & _
                                "CRTXNStatus=" & _
                                    CStr(Me.CRTXNStatus) & ", " & _
                                "CRTXNReceiptGiven=" & _
                                 strReceiptGiven & _
                                    " where CRTXNId =" & Me.CRTXNId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select

            If intSQLReturnValue < 0 Then
                'NOP
            Else
                Me.m_IsDirty = False
                Me.ISPersistedCRTXN = True
            End If
            ResetOverride()


        End If
        Return intSQLReturnValue
    End Function

End Class

