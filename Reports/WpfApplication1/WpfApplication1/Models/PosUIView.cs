using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PosUIView
    {
        public PosUIView()
        {
            this.PosMenuItemUIViews = new List<PosMenuItemUIView>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<PosMenuItemUIView> PosMenuItemUIViews { get; set; }
    }
}
