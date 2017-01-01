Imports BOPr
Imports Interop
'Imports StarTSP100OPOSPOSPrinterServiceObjectLib

Public Class frmPoleDisplay
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    'Friend WithEvents AxOPOSCashDrawer1 As AxOposCashDrawer_1_7_Lib.AxOPOSCashDrawer
    'Friend WithEvents AxOPOSLineDisplay1 As AxOposLineDisplay_1_6_Lib.AxOPOSLineDisplay
    'Friend WithEvents AxOPOSCashDrawer2 As AxOposCashDrawer_1_7_Lib.AxOPOSCashDrawer
    'Friend WithEvents AxOPOSLineDisplay2 As AxOposLineDisplay_1_6_Lib.AxOPOSLineDisplay
    'Friend WithEvents AxOPOSCashDrawer3 As AxOposCashDrawer_1_7_Lib.AxOPOSCashDrawer
    'Friend AxOPOSPrinter3 As OposPOSPrinter_1_9_Lib.OPOSPOSPrinter
    'Friend AxOPOSPrinter3 As StarTSP100OPOSPOSPrinterServiceObjectLib.OPOSPOSPrinter
    'Friend WithEvents AxOPOSLineDisplay3 As AxOposLineDisplay_1_6_Lib.AxOPOSLineDisplay
    'Friend WithEvents AxPOSPrinter1 As AxSMJOPOSPOSPrinterControlObjectLib.AxPOSPrinter
    Friend WithEvents AxPOSPrinter1 As Object
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(frmPoleDisplay))
        Me.AxPOSPrinter1 = New AxSMJOPOSPOSPrinterControlObjectLib.AxPOSPrinter()
        CType(Me.AxPOSPrinter1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'AxPOSPrinter1
        '
        Me.AxPOSPrinter1.Enabled = True
        Me.AxPOSPrinter1.Location = New System.Drawing.Point(288, 304)
        Me.AxPOSPrinter1.Name = "AxPOSPrinter1"
        Me.AxPOSPrinter1.OcxState = CType(resources.GetObject("AxPOSPrinter1.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxPOSPrinter1.Size = New System.Drawing.Size(100, 50)
        Me.AxPOSPrinter1.TabIndex = 0
        '
        'frmPoleDisplay
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(6, 15)
        Me.ClientSize = New System.Drawing.Size(563, 491)
        Me.Controls.AddRange(New System.Windows.Forms.Control() {Me.AxPOSPrinter1})
        Me.Name = "frmPoleDisplay"
        Me.Text = "frmPoleDisplay"
        CType(Me.AxPOSPrinter1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region


    Public Enum enumPoleDisplayText         'Use for displaytext or DisplayTextAt method
        DT_Normal
        DT_Blink
        DT_Reverse
        DT_Blink_Reverse
    End Enum
    Public Enum enumPoleScrollText          'Use with ScrollText method
        ST_Up
        ST_Down
        ST_Left
        ST_Right
    End Enum

    Public Enum enumPOSDocToPrint
        KOT
        Check
        CashCount
        CashTxnReceipt
    End Enum
    Private Shared m_frmPoleDisplay As frmPoleDisplay

    Friend Shared Function CreatePoleDisplay() As frmPoleDisplay
        If m_frmPoleDisplay Is Nothing Then m_frmPoleDisplay = New frmPoleDisplay()
        Return m_frmPoleDisplay
    End Function
    Friend Sub GrabPoleDisplay()

        With AxPOSPrinter1

            .CheckHealth(100)
            .Open("TSP100")
            .ClaimDevice(1000)
            .DeviceEnabled = True
            .ClearOutput()

        End With

    End Sub
    Friend Sub ReleasePoleDisplay()
        'With AxOPOSLineDisplay3
        '    .ClearText()
        '    .DeviceEnabled = False
        '    .ReleaseDevice()
        '    .Close()
        'End With
    End Sub
    Friend Sub DisplayText(ByVal strText As String, ByVal enumTextstyle As enumPoleDisplayText)
        'ClearText()
        'AxOPOSLineDisplay3.DisplayText(strText, enumTextstyle)

    End Sub
    Friend Sub ClearText()
        'AxOPOSLineDisplay3.ClearText()
    End Sub
    Private Sub GrabPOSPrinter()        'Grabs Printer & Cash Drawer
        Dim resultcode As Integer
        With AxPOSPrinter1
            '.Close()
            '.ReleaseDevice()
            .Open("TSP100")
            '.ClearOutput()
            .DeviceEnabled = True
            '.Claim(30)
            .ClaimDevice(3000)

            '.CheckHealth(1)
            .AsyncMode = False
            'resultcode = .MapMode
            'resultcode = .RecLinesToPaperCut()
            'resultcode = .SetBitmap(1, 2, "S:\Masala\Masala Signs\MGATRU.bmp", 50, -1)

            'resultcode = .PrintNormal(2, Chr(27) & "|1B")
            'resultcode = .PrintBitmap(2, "S:\Masala\Masala Signs\MGATRU.bmp", 1500, -1)
        End With

    End Sub

    Friend Sub BeginPrintReceipt()
        Dim enumMsgBoxResult As MsgBoxResult
        GrabPOSPrinter()
        'With StarOPOSPrinter
        '    Do
        '        If .Then Then
        '            enumMsgBoxResult = MsgBox("Paper Roll is finished. Please insert a new one.", _
        '                                    MsgBoxStyle.RetryCancel, "Printer Error")
        '            If enumMsgBoxResult = MsgBoxResult.Cancel Then Exit Sub
        '        Else
        '            GrabPOSPrinter()
        '            .TransactionPrint(2, 11)
        '            .ClearOutput()
        '            Exit Do
        '        End If
        '    Loop
        'End With
    End Sub

    'Private Sub PrintReceiptHeader()
    '    Dim enumMsgBoxResult As MsgBoxResult
    '    With AxOPOSPOSPrinter3
    '        Do While .RecEmpty = True
    '            enumMsgBoxResult = MsgBox("Paper Roll is finished. Please insert a new one.", _
    '                                    MsgBoxStyle.RetryCancel, "Printer Error")
    '            If enumMsgBoxResult = MsgBoxResult.Cancel Then
    '                AbortReceiptPrint()
    '                Exit Sub
    '            End If
    '        Loop

    '        .PrintNormal(2, Chr(10))            'end of line
    '        .PrintNormal(2, Chr(27) + "|4C" _
    '        & "   Masala Grill @RU")      'Double Wide - Double High font
    '        .PrintNormal(2, Chr(10))
    '        .PrintNormal(2, Chr(27) & "|bC" & "  Busch Campus Center, Rutgers University")
    '        .PrintNormal(2, Chr(10))
    '        .PrintNormal(2, Chr(27) & "|bC" & "  Phone: (732) 878-0500")
    '        .PrintNormal(2, Chr(10))
    '        .PrintNormal(2, Chr(27) & "|bC" & "  E-Mail: masalagrill@msn.com")
    '        '.PrintNormal(2, Chr(27) & "|bC" )
    '        .PrintNormal(2, Chr(10))
    '    End With
    'End Sub

    'Friend Sub PrintReceiptLine(ByVal strPrintLine As String)
    '    Dim enumMsgBoxResult As MsgBoxResult

    '    With AxOPOSPOSPrinter3
    '        Do While .RecEmpty = True

    '            enumMsgBoxResult = MsgBox("Paper Roll is finished. Please insert a new one.", _
    '                                    MsgBoxStyle.RetryCancel, "Printer Error")
    '            If enumMsgBoxResult = MsgBoxResult.Cancel Then
    '                AbortReceiptPrint()
    '                Exit Sub
    '            End If
    '        Loop

    '        .PrintNormal(2, Chr(10))            'end of line
    '        .PrintNormal(2, Chr(27) & "|bC" & strPrintLine)
    '        '.PrintNormal(2, Chr(10))            'end of line
    '    End With
    'End Sub

    'Donot use markfeed : it is hard to understand how it works
    Friend Sub EndPrintReceipt()
        Dim enumMsgBoxResult As MsgBoxResult

        'With AxOPOSPOSPrinter3
        '    Do While .RecEmpty = True
        '        enumMsgBoxResult = MsgBox("Paper Roll is finished. Please insert a new one.", _
        '                                MsgBoxStyle.RetryCancel, "Printer Error")
        '        If enumMsgBoxResult = MsgBoxResult.Cancel Then
        '            AbortReceiptPrint()
        '            Exit Sub
        '        End If
        '    Loop
        '    '.PrintNormal(2, Chr(27) + "|4lF")   'feed 4 lines
        '    '.PrintNormal(2, Chr(27) & "|rvC" & SaleDays.CreateSaleDay.CurrentSaleDay.ReceiptFooterMsg)
        '    .PrintNormal(2, Chr(27) + "|1lF")   'feed 2 lines
        '    .PrintNormal(2, (Chr(27)) + "|fP")  'FEED TO THE CUTTING POSITION AND THEN CUT
        '    .TransactionPrint(2, 12)        'ends the transaction print mode and prints buffered lines
        '    .CutPaper(100)
        '    .Close()
        '    .ReleaseDevice()
        'End With
        'With AxOPOSCashDrawer3
        '    .Close()
        '    .ReleaseDevice()
        'End With
    End Sub

    Private Sub AbortReceiptPrint()
        'With AxOPOSPOSPrinter3
        '    .ClearOutput()
        '    .TransactionPrint(2, 12)        'end transaction mode
        '    .Close()
        '    .ReleaseDevice()
        'End With
        'With AxOPOSCashDrawer3
        '    .Close()
        '    .ReleaseDevice()
        'End With
    End Sub

    Friend Sub OpenCashDrawer()
        'With AxOPOSCashDrawer3
        '    If .DeviceEnabled = False Then
        '        .Open("TSP847C(2)")
        '        .ClaimDevice(1000)
        '        .DeviceEnabled = True
        '    End If
        '    If .DrawerOpened = False Then .OpenDrawer()
        'End With
    End Sub

    Public Sub POSPrint(ByVal printDoc As PosDoc)
        Dim i As Integer

        If printDoc Is Nothing Then
            'nop: either no items to be printed or no override was given
        Else
            For i = 1 To printDoc.Copies
                BeginPrintReceipt()
                PrintHeader(printDoc)
                PrintDetail(printDoc)
                PrintFooter(printDoc)
                EndPrintReceipt()
            Next
        End If

    End Sub

    Private Sub PrintHeader(ByVal printDoc As PosDoc)
        Dim enumMsgBoxResult As MsgBoxResult
        Dim strLine As String
        Dim i As Integer

        With AxPOSPrinter1

            .PrintNormal(2, Chr(10))            'end of line
            For Each strLine In printDoc.HeaderLines
                If CheckPaperInPrinter() = MsgBoxResult.Cancel Then
                    AbortReceiptPrint()
                    Exit Sub
                End If
                If i = 0 Then
                    Select Case printDoc.POSDocType
                        Case PosDoc.enumPOSDocType.GuestCheck
                            .PrintNormal(2, Chr(27) + "|4C" & strLine)
                        Case PosDoc.enumPOSDocType.KOT
                            .PrintNormal(2, Chr(27) + "|2C" & strLine)
                    End Select
                Else
                    .PrintNormal(2, Chr(27) & "|bC" & strLine)
                End If
                i += 1
                .PrintNormal(2, Chr(10))
            Next
        End With
    End Sub

    Private Sub PrintDetail(ByVal printDoc As PosDoc)
        Dim enumMsgBoxResult As MsgBoxResult
        Dim strLine As String

        With AxPOSPrinter1
            .PrintNormal(2, Chr(10))            'end of line
            For Each strLine In printDoc.DetailLines
                If CheckPaperInPrinter() = MsgBoxResult.Cancel Then
                    AbortReceiptPrint()
                    Exit Sub
                End If
                Select Case printDoc.POSDocType
                    Case PosDoc.enumPOSDocType.KOT
                        .PrintNormal(2, Chr(27) + "|2C" & strLine)

                    Case PosDoc.enumPOSDocType.GuestCheck
                        .PrintNormal(2, Chr(27) & "|bC" & strLine)
                End Select
                .PrintNormal(2, Chr(10))
            Next
        End With
    End Sub

    Private Sub PrintFooter(ByVal printDoc As PosDoc)
        Dim enumMsgBoxResult As MsgBoxResult
        Dim strLine As String
        Dim i As Integer

        With AxPOSPrinter1
            .PrintNormal(2, Chr(10))            'end of line
            For Each strLine In printDoc.FooterLines
                If CheckPaperInPrinter() = MsgBoxResult.Cancel Then
                    AbortReceiptPrint()
                    Exit Sub
                End If
                Select Case printDoc.POSDocType
                    Case PosDoc.enumPOSDocType.KOT
                        If i = 0 Then
                            .PrintNormal(2, Chr(27) + "|4C" & strLine)
                            '.PrintNormal(2, Chr(27) + "|2C" & strLine)
                        Else
                            .PrintNormal(2, Chr(27) & "|bC" & strLine)
                        End If
                    Case PosDoc.enumPOSDocType.GuestCheck
                        If i = printDoc.FooterLines.Count - 1 Then
                            .PrintNormal(2, Chr(27) & "|bC" & strLine)
                        Else
                            .PrintNormal(2, Chr(27) + "|2C" & strLine)
                        End If
                End Select
                i += 1
                .PrintNormal(2, Chr(10))
            Next
        End With
        'AxOPOSPOSPrinter3.CutPaper(100)
    End Sub

    Private Function CheckPaperInPrinter() As MsgBoxResult
        Dim enumMsgBoxResult As MsgBoxResult

        'With AxOPOSPOSPrinter3
        '    Do While .RecEmpty = True

        '        enumMsgBoxResult = MsgBox("Paper Roll is finished. Please insert a new one.", _
        '                                MsgBoxStyle.RetryCancel, "Printer Error")
        '        If enumMsgBoxResult = MsgBoxResult.Cancel Then
        '            AbortReceiptPrint()
        '            Exit Function
        '        End If
        '    Loop
        'End With
    End Function
End Class

