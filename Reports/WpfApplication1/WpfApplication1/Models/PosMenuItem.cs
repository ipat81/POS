using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PosMenuItem
    {
        public PosMenuItem()
        {
            this.PosMenuItemUIViews = new List<PosMenuItemUIView>();
            this.SubscriberViewPosMenuTabGroupMenuItems = new List<SubscriberViewPosMenuTabGroupMenuItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public int RoleId { get; set; }
        public Nullable<System.DateTime> TempAccessAllowedFrom { get; set; }
        public Nullable<System.DateTime> TempAccessAllowedUntil { get; set; }
        public string SmallImageFileName { get; set; }
        public string LargeImageFileName { get; set; }
        public byte PositionHint { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<PosMenuItemUIView> PosMenuItemUIViews { get; set; }
        public virtual ICollection<SubscriberViewPosMenuTabGroupMenuItem> SubscriberViewPosMenuTabGroupMenuItems { get; set; }
    }
}
