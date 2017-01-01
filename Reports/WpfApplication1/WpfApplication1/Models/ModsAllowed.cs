using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class ModsAllowed
    {
        public int ProductId { get; set; }
        public int ModItemId { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ModItem ModItem { get; set; }
        public virtual Product Product { get; set; }
    }
}
