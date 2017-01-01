using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class TimeCard
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public byte ClockType { get; set; }
        public System.DateTime TimeIn { get; set; }
        public Nullable<System.DateTime> TimeOut { get; set; }
        public System.DateTime SaleDate { get; set; }
        public byte Status { get; set; }
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> OverrideId { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Override Override { get; set; }
        public virtual Role Role { get; set; }
    }
}
