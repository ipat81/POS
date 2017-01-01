using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class OrderItemView
    {
        public int Id { get; set; }
        public int OrderViewId { get; set; }
        public int MenuId { get; set; }
        public int ProductId { get; set; }
        public Nullable<int> PromoSubProductId { get; set; }
        public Nullable<int> GroupId { get; set; }
        public string Course { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public Nullable<int> PortionId { get; set; }
        public byte Status { get; set; }
        public Nullable<short> CheckNumber { get; set; }
        public Nullable<int> OverrideId { get; set; }
        public Nullable<int> AuditId { get; set; }
        public int Expr1 { get; set; }
        public virtual OrderView OrderView { get; set; }
    }
}
