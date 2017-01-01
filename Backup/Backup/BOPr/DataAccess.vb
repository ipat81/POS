
Option Strict On
Imports System.Data
Imports System.Data.SqlClient




Public Class DataAccess
    Private m_sqlCnn As SqlConnection
    Private m_sqlTxn As SqlTransaction
    Private Shared m_DataAccess As DataAccess
    Private m_sqldb As String
    Private m_sqlserver As String

    Private Sub New()

    End Sub
    Public Shared Function CreateDataAccess() As DataAccess         'Create a Singleton DataAccess
        If m_DataAccess Is Nothing Then m_DataAccess = New DataAccess()

        Return m_DataAccess
    End Function

    Friend Overloads Function GetData(ByVal sqlcommand As String) As SqlDataReader
        'call this function with sqlcmd=entire sql text, not a stored procedure name
        Dim sqlCmd As SqlCommand
        Dim sqlReader As SqlDataReader
        'Dim sqlerror As SqlDbError


        OpenConnection()

        Try
            sqlCmd = New SqlCommand(sqlcommand, m_sqlCnn)
            sqlReader = sqlCmd.ExecuteReader()
        Catch ex As SqlException
            '???errormessage???
        Finally

        End Try
        Return sqlReader

    End Function

    Friend Overloads Function GetData(ByVal sqlprocedure As String, ByVal parameterarray As Array) As SqlDataReader

    End Function

    Friend Function Now() As DateTime
        Dim sqlCmd As SqlCommand
        OpenConnection()
        Try
            sqlCmd = New SqlCommand("select getdate()", m_sqlCnn)
            Now = CType(sqlCmd.ExecuteScalar(), DateTime)
        Catch ex As SqlException
            '???errormessage???
        Finally
            CloseConnection()
        End Try
    End Function

    Friend Function GetOrderIdAsInteger(ByVal strSqlCmd As String) As Integer
        Dim sqlCmd As SqlCommand
        OpenConnection()
        Try
            sqlCmd = New SqlCommand(strSqlCmd, m_sqlCnn)
            Try
                Return CType(sqlCmd.ExecuteScalar(), Integer)
            Catch ex As Exception
                Return 0
            Finally
            End Try

        Catch ex As SqlException
            '???errormessage???
        Finally
            CloseConnection()
        End Try
    End Function

    Friend Function GetAmountAsDouble(ByVal strSqlCmd As String) As Double
        Dim sqlCmd As SqlCommand
        OpenConnection()
        Try
            sqlCmd = New SqlCommand(strSqlCmd, m_sqlCnn)
            Try
                Return CType(sqlCmd.ExecuteScalar(), Double)
            Catch ex As Exception
                Return 0
            Finally
            End Try

        Catch ex As SqlException
            '???errormessage???
        Finally
            CloseConnection()
        End Try
    End Function

    Friend Function GetCountAsInteger(ByVal strSqlCmd As String) As Integer
        Dim sqlCmd As SqlCommand
        OpenConnection()
        Try
            sqlCmd = New SqlCommand(strSqlCmd, m_sqlCnn)
            Try
                Return CType(sqlCmd.ExecuteScalar(), Integer)
            Catch ex As Exception
                Return 0
            Finally
            End Try

        Catch ex As SqlException
            '???errormessage???

        Finally
            CloseConnection()
        End Try
    End Function

    Public Property SqlDBName() As String
        Get
            If m_sqldb = "" Then m_sqldb = "PosPr03"
            'If m_sqldb = "" Then m_sqldb = "PosRU"
            'If m_sqldb = "" Then m_sqldb = "QBPayroll"
            'If m_sqldb = "" Then m_sqldb = "QBPayrollRU"
            Return m_sqldb
        End Get
        Set(ByVal sqldbname As String)
            m_sqldb = sqldbname
        End Set
    End Property

    Public Property SqlServerName() As String
        Get
            'PR server
            'If m_sqlserver = "" Then m_sqlserver = "MASALA1"
            'Home server
            'If m_sqlserver = "" Then m_sqlserver = "ACER4005"
            'If m_sqlserver = "" Then m_sqlserver = "MGPC1\SQLEXPRESS"
            If m_sqlserver = "" Then m_sqlserver = "(local)"
            Return m_sqlserver

        End Get
        Set(ByVal sqlservername As String)
            m_sqlserver = sqlservername
        End Set
    End Property


    Private Sub OpenConnection()
        Dim sqlConnString As String
        'Dim sqlerror As SqlDbError


        sqlConnString = "Server =" & SqlServerName & " ; Integrated Security = SSPI; database =" & SqlDBName
        'sqlConnString = "Server =(local)" & " ; Integrated Security = SSPI; database =" & SqlDBName
        'sqlConnString = "Server = MASALA1 ; Integrated Security = SSPI; database = POSPr03"

        'sqlConnString = "Server = cc719583-ru1 ; Integrated Security = SSPI; database = POSPr03"
        ''sqlConnString = "Server = cc719583-ru1 ; Integrated Security = SSPI; database = POSBkp"
        'sqlConnString = "Server = cc719583-ru1 ; Integrated Security = SSPI; database = QBPayroll"
        ''sqlConnString = "Server = CC715983-S ; Integrated Security = SSPI; database = POSBkp"
        'MG - PR database string
        ''sqlConnString = "Server = MASALA1 ; Integrated Security = SSPI; database = POSPr03"
        ''sqlConnString = "Server = RUPOS-S ; Integrated Security = SSPI; database = POS"
        ''sqlConnString = "Server = 192.168.0.100 ; Integrated Security = SSPI; database = POSPr03;Connect Timeout=60"

        m_sqlCnn = New SqlConnection(sqlConnString)



        Try
            If m_sqlCnn.State = ConnectionState.Closed Then

                m_sqlCnn.Open()
            End If

        Catch ex As SqlException
            '???errormessage???
        Finally
        End Try

    End Sub
    Friend Sub CloseConnection()
        Try
            If (m_sqlCnn Is Nothing) OrElse (m_sqlCnn.State = ConnectionState.Closed) Then
                'nop
            Else
                m_sqlCnn.Close()
            End If

        Catch ex As SqlException
            '???errormessage???
        Finally
            m_sqlCnn = Nothing
        End Try

    End Sub

    Friend Sub StartTxn()
        OpenConnection()

        m_sqlTxn = m_sqlCnn.BeginTransaction
    End Sub

    Friend Function EndTxn() As Integer
        Dim intSQLReturnValue As Integer
        Try
            If m_sqlTxn Is Nothing Then
                intSQLReturnValue = -1
            Else
                m_sqlTxn.Commit()
            End If
        Catch ex As SqlException
            '???errormessage???
            If m_sqlTxn Is Nothing Then
                intSQLReturnValue = -1
            Else
                m_sqlTxn.Rollback()
                intSQLReturnValue = -1
                'Raise an event here to display msgbox; this will be consumed by the desktop form
            End If
        Finally
            CloseConnection()
        End Try
        Return intSQLReturnValue
    End Function

    Friend Function SetData(ByVal strSqlCmd As String) As Integer
        'sqlcmd should be complete sql action query string for insert, update or delete, use this if you
        'do not want a PK to be returned 
        Dim sqlCmd As SqlCommand
        Dim intRecsAffected As Integer
        'Dim sqlerror As SqlDbError

        Try
            sqlCmd = New SqlCommand(strSqlCmd, m_sqlCnn, m_sqlTxn)
            intRecsAffected = CType(sqlCmd.ExecuteNonQuery(), Integer)
        Catch ex As SqlException
            '???errormessage???
            If m_sqlTxn Is Nothing Then
                intRecsAffected = -1
            Else
                m_sqlTxn.Rollback()
                CloseConnection()
                intRecsAffected = -1
            End If
        Finally
        End Try
        Return intRecsAffected
    End Function

    Friend Function SetDataAndGetPK(ByVal strsqlCmd As String) As Integer
        'sqlcmd should be complete sql action query string for insert, update or delete, use this if you
        'want a PK to be returned for subsequent setdata calls
        Dim sqlCmd As SqlCommand
        Dim intPKey As Integer
        Dim intRecsAffected As Integer
        'Dim sqlerror As SqlDbError
        strsqlCmd = strsqlCmd & " select @@identity"        'to get back PK from sql server
        Try
            sqlCmd = New SqlCommand(strsqlCmd, m_sqlCnn, m_sqlTxn)
            intPKey = CType(sqlCmd.ExecuteScalar(), Integer)

        Catch ex As SqlException
            '???errormessage???
            If m_sqlTxn Is Nothing Then
                intPKey = -1
            Else
                m_sqlTxn.Rollback()
                'Raise an event here to display msgbox; this will be consumed by desktop
                intPKey = -1
            End If
        Finally
        End Try
        Return intPKey
    End Function

End Class
