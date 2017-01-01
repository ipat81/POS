using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class ProductDef
    {
        public int ParentProductId { get; set; }
        public int ProductId { get; set; }
        public Nullable<byte> UISortOrder { get; set; }
        public Nullable<byte> KOTLayoutEnum { get; set; }
        public Nullable<byte> CheckLayoutEnum { get; set; }
        public Nullable<byte> Discount { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Product Product { get; set; }
        public virtual Product Product1 { get; set; }
    }
}
