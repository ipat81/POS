using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class MenuItem
    {
        public MenuItem()
        {
            this.OrderItems = new List<OrderItem>();
        }

        public int MenuId { get; set; }
        public int ProductId { get; set; }
        public byte Status { get; set; }
        public decimal StdPortionPrice { get; set; }
        public Nullable<int> PortionGroupId { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual PortionGroup PortionGroup { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
