using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PayPeriod
    {
        public PayPeriod()
        {
            this.Payrolls = new List<Payroll>();
        }

        public int Id { get; set; }
        public System.DateTime Starts { get; set; }
        public System.DateTime Ends { get; set; }
        public byte Status { get; set; }
        public Nullable<decimal> TotalKitchenPay { get; set; }
        public Nullable<decimal> TotalKitchenHelpPay { get; set; }
        public Nullable<decimal> TotalServicePay { get; set; }
        public Nullable<decimal> TotalSalary { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
    }
}
