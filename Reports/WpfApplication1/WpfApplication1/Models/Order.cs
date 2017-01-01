using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Order
    {
        public Order()
        {
            this.OrderItems = new List<OrderItem>();
        }

        public int Id { get; set; }
        public Nullable<int> SEId { get; set; }
        public int EmployeeId { get; set; }
        public byte OrderTypeEnum { get; set; }
        public Nullable<decimal> BaseAmount { get; set; }
        public Nullable<decimal> SalesTaxAmount { get; set; }
        public bool SalesTaxStatus { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<decimal> TipAmount { get; set; }
        public byte Status { get; set; }
        public bool HasVoids { get; set; }
        public Nullable<short> NumberOfSplitChecks { get; set; }
        public Nullable<int> OverrideId { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Override Override { get; set; }
        public virtual SE SE { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
