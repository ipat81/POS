using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class AuditDetail
    {
        public long Id { get; set; }
        public string PropertyName { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public long AuditChangeSetId { get; set; }
        public virtual AuditChangeSet AuditChangeSet { get; set; }
    }
}
