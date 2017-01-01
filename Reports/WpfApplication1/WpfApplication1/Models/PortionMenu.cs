using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PortionMenu
    {
        public int Id { get; set; }
        public int PortionGroupId { get; set; }
        public int PortionId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Portion Portion { get; set; }
        public virtual PortionGroup PortionGroup { get; set; }
    }
}
