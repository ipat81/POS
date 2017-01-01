using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class POSFunction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public int RoleId { get; set; }
        public Nullable<System.DateTime> TempAccessAllowedFrom { get; set; }
        public Nullable<System.DateTime> TempAccessAllowedUntil { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Role Role { get; set; }
    }
}
