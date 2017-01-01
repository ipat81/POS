using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class ServerTipAllocation
    {
        public int EmployeeId { get; set; }
        public int SEId { get; set; }
        public Nullable<decimal> CashTipDue { get; set; }
        public Nullable<decimal> CCTipDue { get; set; }
        public Nullable<decimal> HATipDue { get; set; }
        public Nullable<decimal> CashTipPaid { get; set; }
        public Nullable<System.DateTime> CashTipPaidOn { get; set; }
        public Nullable<decimal> TipToPayroll { get; set; }
        public Nullable<int> PayPeriodId { get; set; }
        public Nullable<int> CRTxnId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual CRTxn CRTxn { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Payroll Payroll { get; set; }
        public virtual SE SE { get; set; }
    }
}
