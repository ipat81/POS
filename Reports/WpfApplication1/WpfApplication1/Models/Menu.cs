using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Menu
    {
        public Menu()
        {
            this.MenuItems = new List<MenuItem>();
            this.SDSessions = new List<SDSession>();
            this.SDSessions1 = new List<SDSession>();
        }

        public int Id { get; set; }
        public int SessionId { get; set; }
        public string Name { get; set; }
        public Nullable<byte> MenuTypeEnum { get; set; }
        public Nullable<System.DateTime> InEffetFrom { get; set; }
        public Nullable<System.DateTime> InEffectUntil { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Session Session { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<SDSession> SDSessions { get; set; }
        public virtual ICollection<SDSession> SDSessions1 { get; set; }
    }
}
