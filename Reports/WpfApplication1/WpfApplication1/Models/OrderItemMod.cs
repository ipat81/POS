using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class OrderItemMod
    {
        public int OrderItemId { get; set; }
        public int ModItemId { get; set; }
        public string SelectedModLevelName { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ModItem ModItem { get; set; }
        public virtual OrderItem OrderItem { get; set; }
    }
}
