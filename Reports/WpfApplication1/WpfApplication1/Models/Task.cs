using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Task
    {
        public Task()
        {
            this.SETasks = new List<SETask>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<byte> MaxDuration { get; set; }
        public Nullable<byte> DelayLimit { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<SETask> SETasks { get; set; }
    }
}
