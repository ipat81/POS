using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PosMenuItemUIView
    {
        public int Id { get; set; }
        public int PosMenuItemId { get; set; }
        public int PosUIViewId { get; set; }
        public short ViewPosition { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual PosMenuItem PosMenuItem { get; set; }
        public virtual PosUIView PosUIView { get; set; }
    }
}
