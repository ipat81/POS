Option Strict On
Imports System.Data.SqlClient

Public Class ProductGroup
    Inherits POSBO

    Public Enum enumProductGroup
        Veggie = 1
        Vegan = 2
        Chicken = 3
        Lamb = 4
        SeaFood = 5
        HotBeverage = 6
        ColdBeverage = 7
        Mixed = 8
    End Enum



    Private m_ProductGroupID As Integer
    Private m_ProductGroupName As String


    Private m_isDirty As Boolean

    Public Sub New()
        ProductGroupId = 0
    End Sub



    Friend Sub New(ByVal newproductgroupid As Integer, ByVal newproductgroupname As String)
        ProductGroupId = newproductgroupid
        ProductGroupName = newproductgroupname

    End Sub

    Public Property ProductGroupId() As Integer

        Get
            Return m_ProductGroupID
        End Get

        Set(ByVal Value As Integer)
            m_ProductGroupID = Value
        End Set
    End Property

    Public Property ProductGroupName() As String

        Get
            Return m_ProductGroupName
        End Get

        Set(ByVal Value As String)
            m_ProductGroupName = Value
        End Set

    End Property


End Class

