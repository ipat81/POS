using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using BOPr;
namespace Pos2006Reports
{
    public class CrownSaleReportVM
    {

        public ObservableCollection<ReportData> SaleReportData { get; set; }
        public ObservableCollection<string> Reports { get; set; }
        public CrownSaleReportVM()
        {
            Reports = new ObservableCollection<string> { "ST Due Report", "Tips Payable Report" };
            this.SaleReportData = new ObservableCollection<ReportData>();

        }

        public DateTime? rptFrom { get; set; }
        public DateTime? rptTo { get; set; }

        public void LoadReportData()
        {
            if (rptFrom == null || rptTo == null) return;
            this.SaleReportData.Clear();
            this.LoadSaleDays();
            //this.LoadOrders();
            //this.LoadPayments();
        }
        private void LoadPayments(SaleDay sd)
        {
            throw new NotImplementedException();
        }

        private void LoadOrders()
        {
            throw new NotImplementedException();
        }

        private void LoadSaleDays()
        {
            DateTime rptdate = (DateTime) rptFrom;
            SaleDay sd=null;
            SaleDays sdList = null;
            ReportData rptData = null;
            do
            {
                sdList = SaleDays.CreateSaleDay();
                sd = sdList.get_Item(rptdate, true);
                if (sd != null)
                {
                    rptData = new ReportData();
                    rptData.sd = sd;
                    rptData.SaleDate = rptdate;
                    this.SaleReportData.Add(rptData);
                    rptData.LoadRevenueData();
                    //rptData.LoadEatOutData();
                    //this.LoadPayments(sd);
                }
                rptdate = rptdate.AddDays(1);
            } while (rptdate <= rptTo);
        }

        //private void LoadRevenueData(SaleDay sd)
        //{
        //    foreach (SaleDaySession sdSession in sd.AllSaleDaySessions)
        //    {
        //        sd.SetAllCurrentPayments(sdSession.SaleDaySessionFrom, sdSession.SaleDaySessionTo);
        //        sd.SetAllNonCurrentPayments(sdSession.SaleDaySessionFrom, sdSession.SaleDaySessionTo);

        //    }
        //}
    }
}

