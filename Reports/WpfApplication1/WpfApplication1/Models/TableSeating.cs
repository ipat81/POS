using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class TableSeating
    {
        public int SEId { get; set; }
        public int DTableId { get; set; }
        public Nullable<byte> TableSeatingSequence { get; set; }
        public byte TableSeatingTypeEnum { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual DTable DTable { get; set; }
        public virtual SE SE { get; set; }
    }
}
