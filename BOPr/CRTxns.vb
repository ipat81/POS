Option Strict On
Imports System.Data.SqlClient

Public Class CRTXNs
    Inherits POSBOs


    Private myCRTXN As CRTXN
    Private m_objOverrideForCRTXnEdit As Override
    Public Event OverrideNeeded(ByVal newOverride As Override)



    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView

    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        Active
        InActivate
        All
    End Enum

    Public Sub New()

    End Sub

    Public Sub New(ByVal enumCRTXNFilter As CRTxns.EnumFilter, _
                    ByVal enumCRTXNView As CRTxns.EnumView, ByVal fordate As Date)
        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        'Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objCRTXN As CRTXN


        Select Case enumCRTXNView
            Case EnumView.BaseView
                sqlcommand = "Select CRTXNId , CRTXNAmount From CRTXN"

            Case EnumView.CompleteView
                sqlcommand = "Select * from CRTXN"

        End Select


        Select Case enumCRTXNFilter

            Case EnumFilter.All

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where CRTXNStatus = " & _
                CStr(CRTXN.enumCRTXNStatus.Active)

            Case EnumFilter.InActivate
                sqlcommand = sqlcommand & " where CRTXNStatus = " & _
                CStr(CRTXN.enumCRTXNStatus.InActive)

        End Select
        sqlcommand = sqlcommand & _
                    " and CRTXNAT > ='" & fordate.ToString & "'"



        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            Do While SqlDr.Read()
                objCRTXN = New CRTXN()
                Me.Add(objCRTXN)
                objCRTXN.ReadSqlDr(SqlDr)
            Loop
            SqlDr.Close()
        End If
        objDataAccess.CloseConnection()
    End Sub

    Public Sub New(ByVal enumCRTXNFilter As CRTxns.EnumFilter, _
                    ByVal enumCRTXNView As CRTxns.EnumView, ByVal fromDate As DateTime, ByVal toDate As DateTime)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        'Dim msg As String
        Dim objDataAccess As DataAccess
        Dim objCRTXN As CRTXN


        Select Case enumCRTXNView
            Case EnumView.BaseView
                sqlcommand = "Select CRTXNId , CRTXNAmount From CRTXN"

            Case EnumView.CompleteView
                sqlcommand = "Select * from CRTXN"

        End Select


        Select Case enumCRTXNFilter

            Case EnumFilter.All

            Case EnumFilter.Active
                sqlcommand = sqlcommand & " where CRTXNStatus = " & _
                CStr(CRTXN.enumCRTXNStatus.Active)

            Case EnumFilter.InActivate
                sqlcommand = sqlcommand & " where CRTXNStatus = " & _
                CStr(CRTXN.enumCRTXNStatus.InActive)

        End Select
        sqlcommand = sqlcommand & _
                    " and CRTXNAT > ='" & fromDate.ToString & "'" & " And CRTXNAT <=' " & (toDate.AddDays(1)).ToString & "'"



        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            Do While SqlDr.Read()
                objCRTXN = New CRTXN()
                Me.Add(objCRTXN)
                objCRTXN.ReadSqlDr(SqlDr)
            Loop
        End If


    End Sub





    Public ReadOnly Property CRTXnEditAllowed(ByVal objCrTxn As CRTXN) As Boolean
        Get
            If objCrTxn Is Nothing Then
                SetCRTXNOverride()


            ElseIf objCrTxn.CRTXNAt < Today Then
                Return False
            Else
                SetCRTXNOverride()
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

                        'nope
                End Select
            End With
        Else
            'Use an existing override to avoid repetitive requests for override
        End If

    End Sub

    Private ReadOnly Property OverrideForCRTXNEdit() As Override
        Get
            'Dim objSaleDay As SaleDay
            'Dim objSDSession As SaleDaySession

            m_objOverrideForCRTXnEdit = New Override()
            With m_objOverrideForCRTXnEdit
                .OverrideType = Override.enumOverrideType.EditCRTXN

                .SetOverrideLevelNeeded(Override.enumOverrideLevel.ManagerNeeded)

            End With

            Return m_objOverrideForCRTXnEdit
        End Get

    End Property




    Public ReadOnly Property Item(ByVal index As Integer) As CRTXN
        Get
            Return CType(MyBase.itemPOSBO(index), CRTXN)
        End Get

    End Property



    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myCRTXNIds() As Integer

            Return myCRTXNIds

        End Get
    End Property

    Public Sub Add(ByRef value As CRTXN)

        MyBase.addPOSBO(CType(value, POSBO))

    End Sub



    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub
    Public Function IndexOf(ByVal objCRTXN As CRTXN) As Integer

        Return MyBase.List.IndexOf(CType(objCRTXN, POSBO))

    End Function

    Public Function Contains(ByVal value As CRTXN) As Boolean
        MyBase.containsPOSBO(CType(value, POSBO))

    End Function




    Public Function AddCRTXN() As CRTXN
        Dim objCRTXN As CRTXN

        objCRTXN = New CRTXN()
        objCRTXN.CRTXNStatus = CRTXN.enumCRTXNStatus.NewCrTXn
        Me.Add(objCRTXN)
        Return objCRTXN
    End Function



    'Public ReadOnly Property CurrentCRTXN(ByVal intCashierId As Integer) As CRTXN

    '    Get
    '        Dim i As Integer
    '        Dim objLatestCRTXN As CRTXN
    '        If Me.Count = 0 Then
    '            'nop
    '        Else
    '            For i = Math.Max(Me.Count - 1, 0) To 0 Step -1
    '                With Me.Item(i)
    '                    If (.CRTXNStatus = CRTXN.enumCRTXNStatus.Active) And _
    '                        (.CashierId = intCashierId) Then
    '                        objLatestCRTXN = Me.Item(i)
    '                        Exit For
    '                    End If
    '                End With
    '            Next
    '        End If
    '        Return objLatestCRTXN
    '    End Get
    'End Property

    Public Function SetData() As Integer

        Dim objCRTXN As CRTXN
        Dim intSQLReturnValue As Integer
        'Dim i As Integer

        DataAccess.CreateDataAccess.StartTxn()
        For Each objCRTXN In MyBase.List


            intSQLReturnValue = objCRTXN.SetData()
            If intSQLReturnValue < 0 Then
                Exit For
            End If


        Next
        DataAccess.CreateDataAccess.EndTxn()
        Return intSQLReturnValue

    End Function

End Class

