using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Subscriber
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte PosStationType { get; set; }
        public int SubscriberViewId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
    }
}
