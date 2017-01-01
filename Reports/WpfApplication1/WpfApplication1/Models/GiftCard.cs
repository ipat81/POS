using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class GiftCard
    {
        public GiftCard()
        {
            this.GCTxns = new List<GCTxn>();
        }

        public int Id { get; set; }
        public int SEId { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public decimal IssueAmount { get; set; }
        public System.DateTime ValidTill { get; set; }
        public byte GCTypeEnum { get; set; }
        public byte IssueMethodEnum { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<GCTxn> GCTxns { get; set; }
        public virtual SE SE { get; set; }
    }
}
