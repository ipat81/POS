using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class CashRegister
    {
        public CashRegister()
        {
            this.CRItemCounts = new List<CRItemCount>();
            this.CRTxns = new List<CRTxn>();
            this.Payments = new List<Payment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<CRItemCount> CRItemCounts { get; set; }
        public virtual ICollection<CRTxn> CRTxns { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
