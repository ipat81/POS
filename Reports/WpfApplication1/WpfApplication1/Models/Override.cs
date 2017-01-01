using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Override
    {
        public Override()
        {
            this.CRItemCounts = new List<CRItemCount>();
            this.Orders = new List<Order>();
            this.Payments = new List<Payment>();
            this.TimeCards = new List<TimeCard>();
        }

        public int Id { get; set; }
        public byte Status { get; set; }
        public string Reason { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedAt { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<CRItemCount> CRItemCounts { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
    }
}
