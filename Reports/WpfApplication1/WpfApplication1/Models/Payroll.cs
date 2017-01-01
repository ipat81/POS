using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Payroll
    {
        public Payroll()
        {
            this.PayRollAdjusts = new List<PayRollAdjust>();
            this.ServerTipAllocations = new List<ServerTipAllocation>();
        }

        public int EmployeeId { get; set; }
        public int PayPeriodId { get; set; }
        public int EmployeePayInfoId { get; set; }
        public byte WeekNumber { get; set; }
        public Nullable<decimal> ClockHrs { get; set; }
        public Nullable<decimal> ClockDays { get; set; }
        public Nullable<decimal> QB1Hrs { get; set; }
        public Nullable<decimal> QB1Days { get; set; }
        public Nullable<decimal> QB2Hrs { get; set; }
        public Nullable<decimal> QB2Days { get; set; }
        public Nullable<decimal> CashTipPaid { get; set; }
        public Nullable<decimal> CCTipPaid { get; set; }
        public Nullable<decimal> HATipPaid { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual PayPeriod PayPeriod { get; set; }
        public virtual ICollection<PayRollAdjust> PayRollAdjusts { get; set; }
        public virtual ICollection<ServerTipAllocation> ServerTipAllocations { get; set; }
    }
}
