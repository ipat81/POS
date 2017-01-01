using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SubscriberViewPosMenuTabGroupMenuItem
    {
        public int Id { get; set; }
        public int SubscriberViewPosMenuTabGroupId { get; set; }
        public int PosMenuItemId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual PosMenuItem PosMenuItem { get; set; }
        public virtual SubscriberViewPosMenuTabGroup SubscriberViewPosMenuTabGroup { get; set; }
    }
}
