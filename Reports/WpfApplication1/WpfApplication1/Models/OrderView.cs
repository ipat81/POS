using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class OrderView
    {
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
        public virtual ICollection<OrderItemView> OrderItemViews { get; set; }
    }
}
