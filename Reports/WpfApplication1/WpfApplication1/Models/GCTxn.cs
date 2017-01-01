using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class GCTxn
    {
        public GCTxn()
        {
            this.Payments = new List<Payment>();
        }

        public int Id { get; set; }
        public int GiftCardId { get; set; }
        public decimal RedemptionAmount { get; set; }
        public int SEId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual GiftCard GiftCard { get; set; }
        public virtual SE SE { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
