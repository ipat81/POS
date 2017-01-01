Option Strict On
Imports System.Collections
Imports System.Data.SqlClient



Public Class POSBOs
    Inherits CollectionBase



    Public Sub New()

    End Sub

    Public ReadOnly Property itemPOSBO(ByVal index As Integer) As POSBO
        Get
            Return CType(List(index), POSBO)
        End Get

    End Property



    Public ReadOnly Property keysPOSBO() As Integer()
        Get
            Dim myEmployeeIds() As Integer



            Return myEmployeeIds

        End Get
    End Property




    Public Sub addPOSBO(ByRef value As POSBO)



        List.Add(value)

    End Sub



    Public Sub removePOSBO(ByVal index As Integer)

        List.RemoveAt(index)

    End Sub

    Public Function indexOfPOSBO(ByRef value As POSBO) As Integer

    End Function

    Public Function containsPOSBO(ByVal value As POSBO) As Boolean
        'Dim id As Integer
        'Dim objPOSBO As POSBO
        'Dim contains As Boolean

        Return Me.InnerList.Contains(value)
        'For Each objPOSBO In List
        '    If objPOSBO Is value Then
        '        contains = True
        '        Exit For
        '    Else
        '        contains = False
        '    End If
        'Next
        'Return contains

    End Function

End Class

