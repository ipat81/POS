using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SETask
    {
        public SETask()
        {
            this.Escalations = new List<Escalation>();
        }

        public int Id { get; set; }
        public int SEId { get; set; }
        public int TaskId { get; set; }
        public Nullable<System.DateTime> ActStartTime { get; set; }
        public Nullable<System.DateTime> ActEndTime { get; set; }
        public Nullable<System.DateTime> EstStartTime { get; set; }
        public Nullable<System.DateTime> EstEndTime { get; set; }
        public Nullable<int> TaskDoneBy { get; set; }
        public Nullable<int> OrderItemId { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<Escalation> Escalations { get; set; }
        public virtual SE SE { get; set; }
        public virtual Task Task { get; set; }
    }
}
