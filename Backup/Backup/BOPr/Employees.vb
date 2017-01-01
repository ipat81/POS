Option Strict On
Imports System.Data.SqlClient


Public Class Employees
    Inherits POSBOs


    Public Enum EnumView As Integer  'This enum determines the select clause

        BaseView
        CompleteView

    End Enum


    Public Enum EnumFilter As Integer  'This enum determines the where condition
        All
        Active
        InActive
        Terminated
        ActiveManagers
        ActiveCashiers
        ActiveOwners
    End Enum


    Friend Sub New()

    End Sub


    Friend Sub New(ByVal enumEmployeeFilter As EnumFilter, ByVal enumEmployeeView As EnumView)

        MyBase.New()

        Dim sqlcommand As String
        Dim SqlDr As SqlDataReader
        Dim msg As String
        Dim objDataAccess As DataAccess


        Select Case enumEmployeeView
            Case EnumView.BaseView
                sqlcommand = "Select EmployeeId,EmployeeFirstName,EmployeeLastName From Employee"

            Case EnumView.CompleteView
                sqlcommand = "Select * from Employee"
        End Select

        Select Case enumEmployeeFilter
            Case EnumFilter.All, EnumFilter.Active
            Case EnumFilter.InActive
                sqlcommand = sqlcommand & " where  EmployeeStatus =  " & Employee.enumEmployeeStatus.Inactive

                'Case EnumFilter.Active
                'sqlcommand = sqlcommand & " where EmployeeStatus = " & Employee.enumEmployeeStatus.Active

            Case EnumFilter.Terminated
                sqlcommand = sqlcommand & "  where EmployeeStatus = " & Employee.enumEmployeeStatus.Terminated

            Case EnumFilter.ActiveCashiers          'waiters + mgrs + owners for Princeton
                sqlcommand = sqlcommand & " where EmployeeStatus = " & Employee.enumEmployeeStatus.Active & _
                                                " and EmployeeRoleId In(" & Employee.enumEmployeeRoleId.Waiter & _
                                                "," & Employee.enumEmployeeRoleId.Manager & _
                                                "," & Employee.enumEmployeeRoleId.Owner & ")"
            Case EnumFilter.ActiveManagers
                sqlcommand = sqlcommand & " where EmployeeStatus = " & Employee.enumEmployeeStatus.Active & _
                             " And EmployeeRoleID IN (" & CStr(Employee.enumEmployeeRoleId.Manager) & "," & _
                               CStr(Employee.enumEmployeeRoleId.Owner) & ")"

            Case EnumFilter.ActiveOwners
                sqlcommand = sqlcommand & " where EmployeeStatus = " & Employee.enumEmployeeStatus.Active & _
                             " And EmployeeRoleID = " & Employee.enumEmployeeRoleId.Owner
        End Select

        'DataAccess is  wrtten as a singleton class. Therefore its  shared function Create dataAccess
        'is called to get a reference of the class

        sqlcommand = sqlcommand & " Order By EmployeeFirstName, EmployeeLastName"
        objDataAccess = DataAccess.CreateDataAccess

        SqlDr = objDataAccess.GetData(sqlcommand)

        If Not SqlDr Is Nothing Then   'That is If GetData is  returning any records.If sqlReder is empty

            loadCollection(SqlDr)       'then empty Employee object is returned

        End If
        SqlDr.Close()
        objDataAccess.CloseConnection()
    End Sub


    'for every record returned by SqlDr an employee object created and added to the collection

    Private Sub loadCollection(ByVal SqlDr As SqlDataReader)
        Dim objOrderEmployee As Employee
        Dim EmployeeId As Integer
        Dim EmployeeFirstName As String
        Dim EmployeeMiddleName As String
        Dim EmployeeLastName As String
        Dim EmployeeRoleId As Employee.enumEmployeeRoleId
        Dim EmployeeDateOfBirth As DateTime
        Dim EmployeeSex As Employee.enumSex
        Dim EmployeePassword As String
        Dim EmployeeHireDate As DateTime
        Dim EmployeeTerminationDate As DateTime
        Dim EmployeeChangedBy As Integer
        Dim EmployeeChangedAt As DateTime
        Dim EmployeeStatus As Employee.enumEmployeeStatus

        Dim SqlDrColName As String
        Dim msg As String
        Dim i As Integer


        Do While SqlDr.Read()       'loops through the records in the SqlDr.


            For i = 0 To SqlDr.FieldCount - 1 'reads every column in a record

                SqlDrColName = SqlDr.GetName(i)

                Select Case SqlDrColName

                    Case "EmployeeId"
                        If Not SqlDr.IsDBNull(i) Then

                            EmployeeId = SqlDr.GetInt32(i)
                        End If

                    Case "EmployeeFirstName"

                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeFirstName = SqlDr.GetString(i)
                        End If

                    Case "EmployeeMiddleName"
                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeMiddleName = SqlDr.GetString(i)
                        End If

                    Case "EmployeeLastName"
                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeLastName = SqlDr.GetString(i)
                        End If


                    Case "EmployeeRoleId"
                        Try
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeRoleId = CType(SqlDr.GetInt32(i), Employee.enumEmployeeRoleId)
                            End If
                        Catch ex As Exception
                            '???errormessage???

                        End Try

                    Case "EmployeeDateOfBirth"
                        EmployeeDateOfBirth = SqlDr.GetDateTime(i)

                    Case "EmployeeSex"
                        Try
                            EmployeeSex = CType(SqlDr.GetInt16(i), Employee.enumSex)
                        Catch ex As Exception
                            '???errormessage???
                        End Try

                    Case "EmployeePassword"
                        EmployeePassword = SqlDr.GetString(i)


                    Case "EmployeeHireDate"
                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeHireDate = SqlDr.GetDateTime(i)
                        Else
                            EmployeeHireDate = Now()
                        End If


                    Case "EmployeeTerminationDate"
                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeTerminationDate = SqlDr.GetDateTime(i)
                        Else
                            EmployeeTerminationDate = Date.MinValue
                        End If


                    Case "EmployeeChangedBy"
                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeChangedBy = SqlDr.GetInt32(i)
                        Else
                            EmployeeChangedBy = 1
                        End If

                    Case "EmployeeChangedAt"
                        If Not SqlDr.IsDBNull(i) Then
                            EmployeeChangedAt = SqlDr.GetDateTime(i)
                        Else
                            EmployeeChangedAt = Date.MinValue
                        End If

                    Case "EmployeeStatus "
                        Try
                            If Not SqlDr.IsDBNull(i) Then
                                EmployeeStatus = CType(SqlDr.GetInt16(i), Employee.enumEmployeeStatus)
                            Else
                                EmployeeStatus = Employee.enumEmployeeStatus.Active
                            End If
                        Catch ex As Exception
                            '???errormessage???
                        End Try

                End Select


            Next

            'Creates and adds Employee records to the collection

            Me.Add(New Employee(EmployeeId, EmployeeFirstName, EmployeeMiddleName, EmployeeLastName, EmployeeRoleId, _
                                      EmployeeDateOfBirth, EmployeeSex, EmployeePassword, EmployeeHireDate, _
                                      EmployeeTerminationDate, EmployeeChangedBy, EmployeeChangedAt, EmployeeStatus))




        Loop
    End Sub



    Public ReadOnly Property Item(ByVal index As Integer) As Employee
        Get
            Return CType(MyBase.itemPOSBO(index), Employee)
        End Get

    End Property



    Public ReadOnly Property Keys() As Integer()
        Get
            Dim myEmployeeIds() As Integer

            Return myEmployeeIds
        End Get
    End Property


    Public Sub Add(ByRef value As Employee)



        MyBase.addPOSBO(CType(value, POSBO))

    End Sub


    Public Sub Remove(ByVal index As Integer)

        MyBase.removePOSBO(index)

    End Sub

    Public Function IndexOf(ByVal objEmployee As Employee) As Integer

        Return MyBase.List.IndexOf(CType(objEmployee, POSBO))

    End Function
    Public Function IndexOf(ByVal intEmployeeId As Integer) As Integer
        Dim objEmployee As Employee
        Dim i As Integer
        With MyBase.List
            For i = 0 To MyBase.List.Count - 1
                objEmployee = CType(.Item(i), Employee)
                If CType(.Item(i), Employee).EmployeeId = intEmployeeId Then
                    Return i
                    Exit For
                End If
            Next
        End With
    End Function

    Public Function Contains(ByVal value As Employee) As Boolean

        MyBase.containsPOSBO(CType(value, POSBO))

    End Function

    Public Sub SetData()
        Dim objEmployee As Employee
        Dim strSqlCmd As String
        DataAccess.CreateDataAccess.StartTxn()
        For Each objEmployee In MyBase.List
            If objEmployee.IsDirty Then
                Select Case objEmployee.EmployeeId      'id=0 means insert new row, all other cases mean update row in SQL table 
                    Case 0
                        strSqlCmd = "insert into employee values ('" & _
                                    objEmployee.EmployeeFirstName & "','" & _
                                    objEmployee.EmployeeMiddleName & "','" & _
                                    objEmployee.EmployeeLastName & "'," & _
                                    CStr(objEmployee.EmployeeRoleId) & ",'" & _
                                    objEmployee.EmployeeDateOfBirth.ToShortDateString & "'," & _
                                    CStr(objEmployee.EmployeeSex) & ",'" & _
                                    objEmployee.EmployeePassword & "','" & _
                                    objEmployee.EmployeeHireDate.ToShortDateString & "','" & _
                                    objEmployee.EmployeeTerminationDate.ToShortDateString & "'," & _
                                    CStr(objEmployee.EmployeeChangedBy) & ",'" & _
                                    objEmployee.EmployeeChangedAt.ToLongDateString & "'," & _
                                    CStr(objEmployee.EmployeeStatus) & "," & _
                                    ")"
                    Case Else
                        strSqlCmd = "update employee set  " & _
                                    "EmployeeFirstName='" & _
                                        objEmployee.EmployeeFirstName & "'," & _
                                    "EmployeeMiddleName='" & _
                                        objEmployee.EmployeeMiddleName & "'," & _
                                    "EmployeeLastName='" & _
                                        objEmployee.EmployeeLastName & "'," & _
                                    "EmployeeRoleId=" & _
                                        CStr(objEmployee.EmployeeRoleId) & "," & _
                                    "EmployeeDateOfBirth='" & _
                                        objEmployee.EmployeeDateOfBirth.ToShortDateString & "'," & _
                                    "EmployeeSex=" & _
                                        CStr(objEmployee.EmployeeSex) & "," & _
                                    "EmployeePassword='" & _
                                        objEmployee.EmployeePassword & "'," & _
                                    "EmployeeHireDate='" & _
                                        objEmployee.EmployeeHireDate.ToShortDateString & "'," & _
                                    "EmployeeTerminationDate='" & _
                                        objEmployee.EmployeeTerminationDate.ToShortDateString & "'," & _
                                    "EmployeeChangedBy=" & _
                                        CStr(objEmployee.EmployeeChangedBy) & "," & _
                                    "EmployeeChangedAt='" & _
                                        objEmployee.EmployeeChangedAt.ToLongDateString & "'," & _
                                    "EmployeeStatus=" & _
                                        CStr(objEmployee.EmployeeStatus) & ")"
                End Select
                DataAccess.CreateDataAccess.SetData(strSqlCmd)
            End If
        Next
        DataAccess.CreateDataAccess.EndTxn()
    End Sub
End Class
