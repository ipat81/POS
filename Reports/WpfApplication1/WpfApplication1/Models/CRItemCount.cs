using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class CRItemCount
    {
        public int SDSessionId { get; set; }
        public int CashRegisterId { get; set; }
        public int CRItemId { get; set; }
        public byte CountTypeEnum { get; set; }
        public short Quantity { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public byte Status { get; set; }
        public Nullable<int> OverrideId { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual CashRegister CashRegister { get; set; }
        public virtual CRItem CRItem { get; set; }
        public virtual Override Override { get; set; }
        public virtual SDSession SDSession { get; set; }
    }
}
