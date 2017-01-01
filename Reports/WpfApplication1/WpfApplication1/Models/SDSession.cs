using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SDSession
    {
        public SDSession()
        {
            this.CRItemCounts = new List<CRItemCount>();
            this.SEs = new List<SE>();
        }

        public int Id { get; set; }
        public Nullable<int> SDId { get; set; }
        public Nullable<System.DateTime> SessionFrom { get; set; }
        public Nullable<System.DateTime> SessionTo { get; set; }
        public int SessionId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<int> SpecialMenuId { get; set; }
        public byte Status { get; set; }
        public Nullable<int> ReconciliationApprovedBy { get; set; }
        public Nullable<System.DateTime> ReconciliationApprovedAt { get; set; }
        public Nullable<System.DateTime> SuspendPromoMenuFrom { get; set; }
        public Nullable<System.DateTime> SuspendPromoMenuUntil { get; set; }
        public Nullable<System.DateTime> SuspendPortionMenuFrom { get; set; }
        public Nullable<System.DateTime> SuspendPortionMenuUntil { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<CRItemCount> CRItemCounts { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Menu Menu { get; set; }
        public virtual Menu Menu1 { get; set; }
        public virtual SD SD { get; set; }
        public virtual ICollection<SE> SEs { get; set; }
    }
}
