using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Escalation
    {
        public int Id { get; set; }
        public int SETaskId { get; set; }
        public Nullable<int> EmployeeIdEscalatedTo { get; set; }
        public System.DateTime EscalatedAt { get; set; }
        public byte PriorityEnum { get; set; }
        public byte RemedialActionEnum { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual SETask SETask { get; set; }
    }
}
