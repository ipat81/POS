using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SubscriberViewPosMenuTabGroup
    {
        public SubscriberViewPosMenuTabGroup()
        {
            this.SubscriberViewPosMenuTabGroupMenuItems = new List<SubscriberViewPosMenuTabGroupMenuItem>();
        }

        public int Id { get; set; }
        public int SubscriberViewPosMenuTabId { get; set; }
        public int PosMenuGroupId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual PosMenuGroup PosMenuGroup { get; set; }
        public virtual SubscriberViewPosMenuTab SubscriberViewPosMenuTab { get; set; }
        public virtual ICollection<SubscriberViewPosMenuTabGroupMenuItem> SubscriberViewPosMenuTabGroupMenuItems { get; set; }
    }
}
