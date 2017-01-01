using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SubscriberViewPosMenuTab
    {
        public SubscriberViewPosMenuTab()
        {
            this.SubscriberViewPosMenuTabGroups = new List<SubscriberViewPosMenuTabGroup>();
        }

        public int Id { get; set; }
        public int SubscriberViewId { get; set; }
        public int PosMenuTabId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual PosMenuTab PosMenuTab { get; set; }
        public virtual SubscriberView SubscriberView { get; set; }
        public virtual ICollection<SubscriberViewPosMenuTabGroup> SubscriberViewPosMenuTabGroups { get; set; }
    }
}
