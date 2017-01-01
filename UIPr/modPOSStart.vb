
'Imports System
'Imports System.Runtime.InteropServices
'Imports System.Security.Permissions
''The attribute is placed on the assembly level.
'<Assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name:="FullTrust")> 


Module modPOSStart
    Private startform As frmPOSDesktop

    Sub Main()

        If startform Is Nothing Then startform = New frmPOSDesktop()
        startform.ShowDialog()

    End Sub
End Module
