Option Strict On

Public Class PosDoc
    Public Enum enumPOSDocType
        KOT
        GuestCheck
    End Enum


    Private m_DocType As enumPOSDocType
    Private m_Copies As Integer = 1
    Private m_HeaderLines As New Collection()
    Private m_DetailLines As New Collection()
    Private m_FooterLines As New Collection()


    Public Property POSDocType() As enumPOSDocType
        Get
            Return m_DocType
        End Get
        Set(ByVal Value As enumPOSDocType)
            m_DocType = Value
        End Set
    End Property

    Public Property Copies() As Integer
        Get
            Return m_Copies
        End Get
        Set(ByVal Value As Integer)
            m_Copies = Value
        End Set
    End Property

    Public ReadOnly Property HeaderLines() As Collection
        Get
            Return m_HeaderLines
        End Get
    End Property

    Public ReadOnly Property DetailLines() As Collection
        Get
            Return m_DetailLines
        End Get
    End Property

    Public ReadOnly Property FooterLines() As Collection
        Get
            Return m_FooterLines
        End Get
    End Property

    Public Sub AddHeaderLine(ByVal strLine As String)
        m_HeaderLines.Add(strLine)
    End Sub

    Public Sub AddDetailLine(ByVal strLine As String)
        m_DetailLines.Add(strLine)
    End Sub

    Public Sub AddFooterLine(ByVal strLine As String)
        m_FooterLines.Add(strLine)
    End Sub

    Public Overrides Function ToString() As String
        Dim msg As String
        For Each line As String In Me.HeaderLines
            msg += line
        Next
        For Each line As String In Me.DetailLines
            msg += line
        Next
        For Each line As String In Me.FooterLines
            msg += line
        Next
        Return msg
    End Function
End Class
