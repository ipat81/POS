Option Strict On
Imports BOPr
Imports C1.Win
Imports System.Globalization
Public Class fixPayroll

    Private WithEvents m_frmReportviewer As frmFGReportViewer
    Private m_DateFrom As Date
    Private m_DateTo As Date
    Friend Sub New()

    End Sub

    Private Sub LoadData()
        Dim objClocks As Clocks
        Dim objClock As Clock
        Dim objTHrs As THrs
        Dim objTHr As Thr
        Dim objDataAccess As DataAccess
        objDataAccess = DataAccess.CreateDataAccess
        objDataAccess.SqlDBName = "QBPayrollRU"
        objClocks = New Clocks(m_frmReportviewer.DateFrom, m_frmReportviewer.DateTo)
        objDataAccess.SqlDBName = "QBPayrollRU"
        objTHrs = New THrs(m_frmReportviewer.DateFrom, m_frmReportviewer.DateTo)

        'first loop over live POS db (POSPR03 or POSbkp) to calculate total clock hrs for each employee during a pay period
        For Each objTHr In objTHrs
            Dim POSHrs As Int32 = 0
            Dim POSMins As Int32 = 0
            Dim reccount As Int32 = 0
            For Each objClock In objClocks
                If (objClock.EmployeeId = objTHr.ThrsEmployeeId) AndAlso _
                    (TimeSpan.Compare(objClock.TimeWorked, TimeSpan.Zero) > 0) Then

                    POSHrs += objClock.TimeWorked.Hours()
                    POSMins += objClock.TimeWorked.Minutes
                    reccount += 1
                End If
            Next
            objTHr.ThrsClockHrs = POSHrs
            objTHr.ThrsClockMins = POSMins
            objTHr.ThrsClockRecCount = reccount
            If (objTHr.ThrsQBHrs = 0) AndAlso (objTHr.ThrsQBMins = 0) Then
                objTHr.ThrsQBHrs = objTHr.ThrsClockHrs
                objTHr.ThrsQBMins = objTHr.ThrsClockMins
                objTHr.ThrsFixMins = 0
            Else
                objTHr.ThrsFixMins = CInt((((objTHr.ThrsQBHrs * 60) + objTHr.ThrsQBMins) - _
                ((POSHrs * 60) + POSMins)))
            End If

        Next

        'second loop to calculate revised clock hrs for each employee for each clock record
        Dim MinutesToAdjust As Double
        Dim RecsToAdjust As Int32
        Dim ClockMinutes As Double
        Dim MinutesToAdjustPerRec As Double
        Dim QBMinsLeft As Double
        Dim POSMinsLeft As Double
        Dim Ratio As Double
        Dim NewClockMins As Double
        For Each objTHr In objTHrs
            QBMinsLeft = objTHr.ThrsQBHrs * 60 + objTHr.ThrsQBMins
            POSMinsLeft = objTHr.ThrsClockHrs * 60 + objTHr.ThrsClockMins
            For Each objClock In objClocks
                If (objClock.EmployeeId = objTHr.ThrsEmployeeId) AndAlso _
                    (TimeSpan.Compare(objClock.TimeWorked, TimeSpan.Zero) > 0) Then
                    ClockMinutes = objClock.TimeWorked.Hours() * 60 + objClock.TimeWorked.Minutes
                    If QBMinsLeft <= ClockMinutes Then
                        ClockMinutes = QBMinsLeft
                    Else
                        If POSMinsLeft > 0 Then Ratio = QBMinsLeft / POSMinsLeft
                        POSMinsLeft = POSMinsLeft - ClockMinutes
                        ClockMinutes = ClockMinutes * Ratio
                    End If
                    objClock.ClockTimeOut = objClock.ClockTimeIn.AddMinutes(ClockMinutes)
                    ClockMinutes = objClock.TimeWorked.Hours() * 60 + objClock.TimeWorked.Minutes
                    QBMinsLeft = QBMinsLeft - ClockMinutes
                    'POSMinsLeft = POSMinsLeft - ClockMinutes
                    'If QBMinsLeft < 1 Then Exit For
                End If
            Next

        Next

        'Now update sql db
        objTHrs.SetData()
        objClocks.SetData()

        '***********************************

    End Sub
    Friend Sub SetReportViewer(ByVal value As frmFGReportViewer)
        m_frmReportviewer = value
        With m_frmReportviewer
            .Report = frmFGReportViewer.enumReport.FixPayroll
            .AllowFutureDates = False
            .Show()
        End With
    End Sub

    Private Sub m_frmReportviewer_RunReport() Handles m_frmReportviewer.RunReport
        Dim QBPayrollReport As rptQBPayrollReport
        LoadData()
        QBPayrollReport = New rptQBPayrollReport()

        QBPayrollReport.SetReportViewer(m_frmReportviewer, "AUTO")
    End Sub


End Class
