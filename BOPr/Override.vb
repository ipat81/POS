Option Strict On
Imports System.Data.SqlClient

Public Class Override
    Inherits POSBO
    Implements IComparable

    Public Enum enumOverrideType
        ClockEdit
        VoidOrder
        VoidOrderItem
        AddOrderItem
        EditOrderItem
        PrintKOT
        PrintGuestCheck
        EditPayment
        EditMoneyCount
        EditCRTXN
        MarkPaymentReceivedButNotEntered
        ReconciliationReportApproval
        ReconciliationCancelApproval
        EditHouseACOrder
    End Enum
    Public Enum enumOverrideStatus
        Active
        InActive
    End Enum

    Public Enum enumOverrideLevel
        NotNeeded
        ManagerNeeded
        OwnerNeeded
    End Enum

    Private m_OverrideId As Integer
    Private m_OverrideBy As Integer
    Private m_OverrideAt As DateTime
    Private m_OverrideType As enumOverrideType
    Private m_OverrideOldRowId As Integer
    Private m_OverrideNewRowId As Integer
    Private m_OverrideChangedBy As Integer
    Private m_OverrideChangedAt As DateTime
    Private m_OverrideStatus As enumOverrideStatus
    Private m_OverrideAvailable As Boolean
    Private m_OverrideReason As String
    Private m_OverrideContext As String
    Private m_Memo As String
    Private m_OverrideLevelNeeded As enumOverrideLevel
    Private m_IsOverrideUsed As Boolean
    Private m_IsDirty As Boolean
    Private m_IsPersisted As Boolean

    Public Sub New()

    End Sub

    Public Sub New(ByVal newoverrideid As Integer)
        OverrideId = newoverrideid
    End Sub


    Friend Sub New(ByVal newoverrideid As Integer, ByVal newOverrideBy As Integer, _
                ByVal newoverrideat As DateTime, ByVal newoverridetype As enumOverrideType, _
                ByVal newOverrideOldRowId As Integer, ByVal newOverrideNewRowId As Integer, _
                ByVal newoverridestatus As enumOverrideStatus)

        OverrideId = newoverrideid
        OverrideBy = newOverrideBy
        m_OverrideAt = newoverrideat
        OverrideType = newoverridetype
        OverrideOldRowId = newOverrideOldRowId
        OverrideNewRowId = newOverrideNewRowId

    End Sub

    Public Property OverrideId() As Integer

        Get
            Return m_OverrideId
        End Get

        Set(ByVal Value As Integer)
            m_OverrideId = Value
        End Set
    End Property

    Public Property OverrideBy() As Integer

        Get
            Return m_OverrideBy
        End Get

        Set(ByVal Value As Integer)
            m_OverrideBy = Value
            MarkDirty()
        End Set

    End Property


    Public ReadOnly Property OverrideName() As String
        Get
            Return SaleDays.CreateSaleDay.ActiveCashiers.Item(Me.OverrideBy).EmployeeFullName
        End Get
    End Property

    Public ReadOnly Property OverrideAt() As DateTime

        Get
            Return m_OverrideAt
        End Get

    End Property

    Private Sub SetOverrideAt()
        m_OverrideAt = SaleDays.CreateSaleDay.Now
    End Sub

    Public Property OverrideType() As enumOverrideType

        Get
            Return m_OverrideType
        End Get

        Set(ByVal Value As enumOverrideType)
            m_OverrideType = Value
            MarkDirty()
        End Set
    End Property


    Public Property OverrideOldRowId() As Integer

        Get
            Return m_OverrideOldRowId
        End Get

        Set(ByVal Value As Integer)
            m_OverrideOldRowId = Value
            MarkDirty()
        End Set
    End Property

    Public Property OverrideNewRowId() As Integer

        Get
            Return m_OverrideNewRowId
        End Get

        Set(ByVal Value As Integer)
            m_OverrideNewRowId = Value
            MarkDirty()
        End Set
    End Property




    'Public Property OverrideChangedBy() As Integer


    '    Get
    '        Return m_OverrideChangedBy
    '    End Get

    '    Set(ByVal Value As Integer)
    '        m_OverrideChangedBy = Value
    '    End Set
    'End Property

    'Public Property OverrideChangedAt() As DateTime


    '    Get
    '        Return m_OverrideChangedAt
    '    End Get

    '    Set(ByVal Value As DateTime)
    '        m_OverrideChangedAt = Value
    '    End Set
    'End Property

    Public Property OverrideStatus() As enumOverrideStatus
        Get
            Return m_OverrideStatus
        End Get
        Set(ByVal Value As enumOverrideStatus)
            m_OverrideStatus = Value
        End Set
    End Property

    Public Property OverrideAvailable() As Boolean
        Get
            Return m_OverrideAvailable
        End Get
        Set(ByVal Value As Boolean)
            m_OverrideAvailable = Value
        End Set
    End Property

    Public Property OverrideReason() As String
        Get
            Return m_OverrideReason
        End Get
        Set(ByVal Value As String)
            m_OverrideReason = Value
            MarkDirty()
        End Set
    End Property

    Public Property OverrideContext() As String
        Get
            Return m_OverrideContext
        End Get
        Set(ByVal Value As String)
            m_OverrideContext = Value
            MarkDirty()
        End Set
    End Property

    Public ReadOnly Property OverrideTypeText() As String
        Get
            Dim sb1 As New System.Text.StringBuilder()

            Select Case Me.OverrideType
                Case enumOverrideType.AddOrderItem
                    sb1.Append("Check for this order has been printed, " & Chr(10) & _
                                "To add an item, 18% Tip or Discount to this order, sign in & press OK.")
                Case enumOverrideType.EditOrderItem
                    sb1.Append("KOT or Check for this order item has been printed, " & Chr(10) & _
                                "To change this item, sign in & press OK.")
                Case enumOverrideType.ClockEdit
                    sb1.Append("These Clock times have been saved." & Chr(10) & _
                                "Sign in and Press OK to change them.")
                Case enumOverrideType.EditPayment
                    sb1.Append("These Payments have been applied to this Order and saved." & Chr(10) & _
                                "Sign in and Press OK to change them.")
                Case enumOverrideType.MarkPaymentReceivedButNotEntered
                    sb1.Append("Table you selected for new order already has an open Order." & Chr(10) & _
                               "1. Press Cancel to first enter payment for this open order, OR" & Chr(10) & _
                               "2. Sign in and press OK to mark the open Order as paid without entering a payment for now.")
                Case enumOverrideType.PrintGuestCheck
                    sb1.Append("Check for this order has been printed, " & Chr(10) & _
                                "To print a duplicate Check, sign in & press OK.")
                Case enumOverrideType.PrintKOT
                    sb1.Append("KOT for this order has been printed, " & Chr(10) & _
                                "To print a duplicate KOT, sign in & press OK.")
                Case enumOverrideType.ReconciliationReportApproval
                    sb1.Append(" Sign in and Press OK to Approve Reconciliation of Cash Register for Any Session of the day selected")

                Case enumOverrideType.EditMoneyCount
                    sb1.Append("Sign in and Press OK to Edit the Cash ,Check or Credit Card details for the Session selected")

                Case enumOverrideType.ReconciliationCancelApproval
                    sb1.Append("Sign in and Press OK to Cancel Approval of Reconciliation for any Session of the day which has been approved earlier")
                Case enumOverrideType.VoidOrder
                Case enumOverrideType.VoidOrderItem
                    sb1.Append("KOT or Check including selected item has been printed, " & Chr(10) & _
                                "To void such an item, sign in & press OK.")
                Case enumOverrideType.EditCRTXN
                    sb1.Append("Sign In And Press Ok To enter or modify any cash Register Transaction for today ," & Chr(10) & _
                              "Else press Cancel")
                Case enumOverrideType.EditHouseACOrder
                    sb1.Append("To change an Order to House A/C or remove House A/c after Check is printed," & Chr(10) & _
                              "Sign in and Press OK, or Else press Cancel")
                Case Else
                    sb1.Append("To get an Override, sign in and press OK.")


            End Select
            Return sb1.ToString
        End Get
    End Property
    Friend Property IsDirty() As Boolean

        Get
            Return m_IsDirty
        End Get
        Set(ByVal value As Boolean)
            m_IsDirty = value
        End Set
    End Property

    Public Sub GetData(ByVal SqlDr As SqlDataReader) '(ByVal enumOverrideView As SaleDays.EnumView)
        Dim sqlcommand As String
        Dim SqlDrColName As String
        Dim objDataAccess As DataAccess

        Dim msg As String
        Dim i As Integer



        Me.IsPersisted = True
        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record
            SqlDrColName = SqlDr.GetName(i) 'Reading  the column Name
            Select Case SqlDrColName
                Case "OverrideBy"   'OverrideType who did the override

                    If Not SqlDr.IsDBNull(i) Then
                        m_OverrideBy = SqlDr.GetInt32(i)
                    End If
                Case "OverrideAt"   'Time when manager did the override

                    If Not SqlDr.IsDBNull(i) Then
                        m_OverrideAt = SqlDr.GetDateTime(i)
                    End If
                Case "OverrideType"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_OverrideType = CType(SqlDr.GetInt32(i), Override.enumOverrideType)
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try
                Case "OverrideOldRowId"
                    If Not SqlDr.IsDBNull(i) Then
                        m_OverrideOldRowId = SqlDr.GetInt32(i)
                    End If
                Case "OverrideNewRowId"

                    If Not SqlDr.IsDBNull(i) Then
                        m_OverrideNewRowId = SqlDr.GetInt32(i)

                    End If
                Case "OverrideStatus"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_OverrideStatus = CType(SqlDr.GetInt32(i), Override.enumOverrideStatus)
                        End If
                    Catch
                        '???errormessage???
                    End Try

                Case "OverrideReason"
                    If Not SqlDr.IsDBNull(i) Then
                        m_OverrideReason = SqlDr.GetString(i)
                    End If

                Case "OverrideLevelNeeded"
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            m_OverrideLevelNeeded = CType(SqlDr.GetInt32(i), Override.enumOverrideLevel)
                        End If
                    Catch
                        '???errormessage???
                    End Try
                Case "OverrideContext"
                    If Not SqlDr.IsDBNull(i) Then
                        m_OverrideContext = SqlDr.GetString(i)
                    End If
            End Select
        Next

    End Sub

    Public Function SetData() As Integer
        Dim intSQLReturnValue As Integer
        Dim strSqlCmd As String
        If Me.IsDirty Then
            Select Case Me.IsPersisted
                Case False
                    DataAccess.CreateDataAccess.StartTxn()
                    strSqlCmd = "insert into Override values (" & _
                                           Me.OverrideBy.ToString & ",'" & _
                                           Me.OverrideAt.ToString & "'," & _
                                           CStr(Me.OverrideType) & "," & _
                                           Me.OverrideOldRowId.ToString & "," & _
                                           Me.OverrideNewRowId.ToString & "," & _
                                           CStr(Me.OverrideStatus) & ",'" & _
                                           Me.OverrideReason & "'," & _
                                           CStr(Me.OverrideLevelNeeded) & ",'" & _
                                           Me.OverrideContext & "'" & _
                                           ")"
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        Me.OverrideId() = intSQLReturnValue
                        m_IsDirty = False
                        Me.IsPersisted = True
                        DataAccess.CreateDataAccess.EndTxn()
                    End If
                Case True
                    DataAccess.CreateDataAccess.StartTxn()
                    strSqlCmd = "Update Override set " & _
                                "OverrideNewRowId=" & Me.OverrideNewRowId.ToString & _
                                " where OverideId=" & Me.OverrideId.ToString
                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)

                    If intSQLReturnValue < 0 Then
                        'NOP
                    Else
                        DataAccess.CreateDataAccess.EndTxn()
                    End If
            End Select

        End If
        Return intSQLReturnValue
    End Function

    Private Sub MarkDirty()
        If IsDirty Then
            'NOP
        Else
            SetOverrideAt()
            IsDirty = True
        End If
    End Sub

    Public Overrides Function Tostring() As String
        Return Me.OverrideType.ToString
    End Function

    Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
        If CType(obj, Override).ToString > Me.ToString Then
            Return -1
        ElseIf CType(obj, Override).ToString < Me.ToString Then
            Return 1
        Else
            Return 0
        End If
    End Function
    Public ReadOnly Property OverrideLevelNeeded() As enumOverrideLevel
        Get
            Return m_OverrideLevelNeeded
        End Get
    End Property

    Friend Sub SetOverrideLevelNeeded(ByVal value As enumOverrideLevel)
        m_OverrideLevelNeeded = value
    End Sub

    Public ReadOnly Property IsOverrideUsed() As Boolean
        Get
            Return m_IsOverrideUsed
        End Get
    End Property

    Friend Sub SetIsOverrideUsed(ByVal value As Boolean)
        m_IsOverrideUsed = value
    End Sub

    Private Property IsPersisted() As Boolean
        Get
            Return m_IsPersisted
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersisted = Value
        End Set
    End Property
End Class

