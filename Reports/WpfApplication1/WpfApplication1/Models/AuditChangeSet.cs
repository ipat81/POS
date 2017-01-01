using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class AuditChangeSet
    {
        public AuditChangeSet()
        {
            this.AuditDetails = new List<AuditDetail>();
        }

        public long Id { get; set; }
        public int ChangedBy { get; set; }
        public System.DateTime ChangedAt { get; set; }
        public byte UserAction { get; set; }
        public Nullable<int> WorkstationId { get; set; }
        public long AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<AuditDetail> AuditDetails { get; set; }
    }
}
