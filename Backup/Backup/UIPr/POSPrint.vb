
Imports BOPr
Imports Interop
Imports StarTSP100OPOSPOSPrinterServiceObjectLib
Imports SMJOPOSPOSPrinterControlObjectLib


Public Class POSPrint





    'Friend AxOPOSPrinter3 As New StarTSP100OPOSPOSPrinterServiceObjectLib.OPOSPOSPrinter()
    Friend AxOPOSPrinter As New SMJOPOSPOSPrinterControlObjectLib.POSPrinter()

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
    Private Shared m_POSPrint As POSPrint

    Friend Shared Function CreatePOSPrint() As POSPrint
        If m_POSPrint Is Nothing Then m_POSPrint = New POSPrint()

        Return m_POSPrint
    End Function
    
   
    Private Sub GrabPOSPrinter()        'Grabs Printer & Cash Drawer
        Dim resultcode As Integer
        With AxOPOSPrinter

            '.Close()
            '.ReleaseDevice()
            '.OpenService(
            If .DeviceEnabled = False Then

                .Open("TSP100")

                .ClearOutput()

                .Claim(4000)
                '.ClaimDevice(4000)
                '.DeviceEnabled = True
                '.Claim(30)

                .CheckHealth(1)

                '.AsyncMode = False
            End If

        End With

    End Sub

   

    
    'Donot use markfeed : it is hard to understand how it works
    

    

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
                GrabPOSPrinter()
                PrintHeader(printDoc)
                PrintDetail(printDoc)
                PrintFooter(printDoc)

            Next
        End If

    End Sub

    Private Sub PrintHeader(ByVal printDoc As PosDoc)
        Dim enumMsgBoxResult As MsgBoxResult
        Dim strLine As String
        Dim i As Integer

        With AxOPOSPrinter

            .PrintNormal(2, Chr(10))            'end of line
            For Each strLine In printDoc.HeaderLines

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

        With AxOPOSPrinter
            .PrintNormal(2, Chr(10))            'end of line
            For Each strLine In printDoc.DetailLines

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

        With AxOPOSPrinter
            .PrintNormal(2, Chr(10))            'end of line
            For Each strLine In printDoc.FooterLines

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
            .PrintNormal(2, Chr(10))
            .PrintNormal(2, Chr(10))
        End With
        AxOPOSPrinter.CutPaper(100)
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


