using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SD
    {
        public SD()
        {
            this.SDSessions = new List<SDSession>();
        }

        public int Id { get; set; }
        public System.DateTime SaleDate { get; set; }
        public Nullable<System.DateTime> OpenAt { get; set; }
        public Nullable<int> OpenBy { get; set; }
        public Nullable<System.DateTime> CloseAt { get; set; }
        public Nullable<int> CloseBy { get; set; }
        public Nullable<byte> SaleDayTypeEnum { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<SDSession> SDSessions { get; set; }
    }
}
