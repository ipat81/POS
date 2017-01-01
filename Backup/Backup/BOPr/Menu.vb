Option Strict On
Imports System.Data.SqlClient

Public Class Menu
    Inherits POSBO

    Public Enum enumMenuStatus
        Active
        Inactive
    End Enum

    Private m_MenuID As Integer
    Private m_SessionId As Integer
    Private m_MenuSaleDate As DateTime
    Private m_MenuChangedBy As Integer
    Private m_MenuChangedAt As DateTime
    Private m_MenuStatus As enumMenuStatus
    Private m_IsPersistedMenu As Boolean

    Private m_objMenuItems As MenuItems

    Private m_isDirty As Boolean

    Public Sub New()
        MenuId = 0
        IsPersistedMenu = False
        MenuStatus = enumMenuStatus.Active
        m_objMenuItems = New MenuItems()
        m_objMenuItems.ParentMenu = Me
    End Sub

    Public Sub AddMenuItem(ByVal objMenuItem As MenuItem)
        'IsPersistedMenu = False
        m_objMenuItems.Add(objMenuItem)
        MarkDirty()
    End Sub

    Public Sub New(ByVal enumMenuItemsFilter As MenuItems.EnumFilter, ByVal enumMenuItemsView As MenuItems.EnumView, _
    ByVal menuDate As Date, ByVal newSessionId As Integer)
        GetData(menuDate, newSessionId)
        m_objMenuItems = New MenuItems(enumMenuItemsFilter, enumMenuItemsView, MenuId)
        m_objMenuItems.ParentMenu = Me

    End Sub


    'Friend Sub New(ByVal newmenuid As Integer, ByVal newsessionid As Integer, ByVal newmenusaledate As DateTime, _
    '                ByVal newmenuchangedby As Integer, ByVal newmenuchangedat As DateTime, _
    '                ByVal newmenustatus As enumMenuStatus)
    '    MenuId = newmenuid
    '    SessionId = newsessionid
    '    MenuSaleDate = newmenusaledate
    '    MenuChangedBy = newmenuchangedby
    '    MenuChangedAt = newmenuchangedat
    'End Sub

    Public ReadOnly Property AllMenuItems() As MenuItems

        Get

            Return m_objMenuItems
        End Get
    End Property



    Public Property MenuId() As Integer

        Get
            Return m_MenuID
        End Get

        Set(ByVal Value As Integer)
            m_MenuID = Value
        End Set
    End Property

    Public Property SessionId() As Integer

        Get
            Return m_SessionId
        End Get

        Set(ByVal Value As Integer)
            m_SessionId = Value
            MarkDirty()
        End Set

    End Property


    Public Property MenuSaleDate() As DateTime

        Get
            Return m_MenuSaleDate
        End Get

        Set(ByVal Value As DateTime)
            m_MenuSaleDate = Value
            MarkDirty()
        End Set
    End Property


    Public Property MenuChangedBy() As Integer

        Get
            Return m_MenuChangedBy
        End Get
        Set(ByVal Value As Integer)
            m_MenuChangedBy = 1
        End Set

    End Property

    'Private Sub SetMenuChangedBy()
    '    m_MenuChangedBy = 1

    'End Sub

    Public Property MenuChangedAt() As DateTime


        Get
            Return m_MenuChangedAt
        End Get
        Set(ByVal Value As DateTime)
            m_MenuChangedAt = SaleDays.CreateSaleDay.Now()
        End Set

    End Property

    'Private Sub SetMenuChangedAt()
    '    m_MenuChangedAt = SaleDay.CreateSaleDay.Now()

    'End Sub


    Public Property MenuStatus() As enumMenuStatus
        Get
            Return m_MenuStatus
        End Get

        Set(ByVal Value As enumMenuStatus)

            m_MenuStatus = Value

        End Set
    End Property

    ReadOnly Property IsDirty() As Boolean

        Get
            Return m_isDirty
        End Get

    End Property
    'Private Sub setIsDirty()
    '    m_isDirty = False
    'End Sub


    Friend Property IsPersistedMenu() As Boolean
        Get
            Return m_IsPersistedMenu
        End Get
        Set(ByVal Value As Boolean)
            m_IsPersistedMenu = Value
        End Set
    End Property

    Private Sub MarkDirty()
        If m_isDirty Then
            'NOP
        Else
            m_isDirty = True
            'SetMenuChangedAt()
            'SetMenuChangedBy()

        End If
    End Sub

    Public Sub GetData(ByVal menuDate As Date, ByVal newSessionId As Integer)
        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim SqlDrColName As String
        Dim objDataAccess As DataAccess
        Dim objMenuItem As MenuItem

        Dim msg As String
        Dim i As Integer

        sqlcommand = "select * from Menu where MenuSaleDate = '" & menuDate.ToShortDateString & "'" & _
                       " And SessionId = " & newSessionId & _
                       " And MenuStatus= " & CStr(Me.enumMenuStatus.Active)

        objDataAccess = DataAccess.CreateDataAccess

        'sqlcommand = sqlcommand & " where  MenuId =  " & Me.MenuId
        SqlDr = objDataAccess.GetData(sqlcommand)
        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty
            Do While SqlDr.Read()       'loops through the records in the SqlDr.
                LoadMenuProperties(SqlDr)
            Loop
        End If

    End Sub

    Friend Sub LoadMenuProperties(ByVal SqlDr As SqlDataReader)
        Dim i As Integer
        Dim SqlDrColName As String

        Me.IsPersistedMenu = True

        For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

            SqlDrColName = SqlDr.GetName(i)

            Select Case SqlDrColName

                Case "MenuId"

                    If Not SqlDr.IsDBNull(i) Then
                        MenuId = SqlDr.GetInt32(i)
                    End If

                Case "SessionId"

                    If Not SqlDr.IsDBNull(i) Then
                        SessionId = SqlDr.GetInt32(i)
                    End If


                Case "MenuSaleDate"
                    If Not SqlDr.IsDBNull(i) Then
                        MenuSaleDate = SqlDr.GetDateTime(i)
                    End If


                Case "MenuChangedBy"

                    If Not SqlDr.IsDBNull(i) Then
                        MenuChangedBy = SqlDr.GetInt32(i)
                    Else
                        MenuChangedBy = 0
                    End If

                Case "MenuChangedAt"
                    If Not SqlDr.IsDBNull(i) Then
                        MenuChangedAt = SqlDr.GetDateTime(i)
                    Else
                        MenuChangedAt = Date.MinValue
                    End If

                Case "MenuStatus "
                    Try
                        If Not SqlDr.IsDBNull(i) Then
                            MenuStatus = CType(SqlDr.GetInt16(i), Menu.enumMenuStatus)
                        Else
                            MenuStatus = Menu.enumMenuStatus.Active
                        End If
                    Catch ex As Exception
                        '???errormessage???
                    End Try

            End Select
        Next
    End Sub

    Public Function SetData() As Integer
        Dim strSqlCmd As String
        Dim intOrderId As Integer
        Dim intSQLReturnValue As Integer
        Dim i As Integer

        Dim objPayment As Payment

        DataAccess.CreateDataAccess.StartTxn()
        If Me.IsDirty Then
            Select Case Me.IsPersistedMenu      'True means update row, False means insert row in SQL table 
                Case False
                    'Me.Cashier.EmployeeId & ",'" & _  (replace this line in place of 1 below)
                    strSqlCmd = "insert into [Menu] values (" & _
                                Me.SessionId.ToString & ",'" & _
                                Me.MenuSaleDate.ToShortDateString & "'," & _
                                Me.MenuChangedBy.ToString & ",'" & _
                                Me.MenuChangedAt & "'," & _
                                CStr(Me.MenuStatus) & _
                                ")"

                    intSQLReturnValue = DataAccess.CreateDataAccess.SetDataAndGetPK(strSqlCmd)
                    If intSQLReturnValue < 0 Then
                        'Error in insert procesing on SQL server
                    Else
                        Me.MenuId = intSQLReturnValue

                    End If

                Case Else
                    strSqlCmd = "update [Menu] set  " & _
                                "SessionId=" & _
                                    Me.SessionId & "," & _
                                "MenuSaleDate= '" & _
                                    Me.MenuSaleDate & "'," & _
                                "MenuChangedBy= " & _
                                    Me.MenuChangedBy & "," & _
                                "MenuChangedAt= '" & _
                                    Me.MenuChangedAt & "'," & _
                                   "MenuStatus = " & _
                                    CStr(Me.MenuStatus) & _
                                 " where MenuId=" & Me.MenuId.ToString

                    intSQLReturnValue = DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End Select

            If intSQLReturnValue < 0 Then
                'nope
            Else

                intSQLReturnValue = Me.AllMenuItems().SetData()  'AllMenuItems
            End If
            If intSQLReturnValue < 0 Then
                'NOP
            Else
                DataAccess.CreateDataAccess.EndTxn()
                m_isDirty = False
                m_IsPersistedMenu = True
            End If

        End If
        Return intSQLReturnValue

    End Function


End Class
