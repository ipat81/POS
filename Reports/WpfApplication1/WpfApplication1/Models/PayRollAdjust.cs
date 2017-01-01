using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PayRollAdjust
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int PayPeriodId { get; set; }
        public byte AdjustTypeEnum { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> Hrs { get; set; }
        public Nullable<int> CRTxnId { get; set; }
        public Nullable<int> SEId { get; set; }
        public string Notes { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual CRTxn CRTxn { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Payroll Payroll { get; set; }
        public virtual SE SE { get; set; }
    }
}
