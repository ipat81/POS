using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PaymentMethod
    {
        public PaymentMethod()
        {
            this.Payments = new List<Payment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public byte SummaryCategoryEnum { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
