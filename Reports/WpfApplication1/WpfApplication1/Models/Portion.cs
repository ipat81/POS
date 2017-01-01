using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Portion
    {
        public Portion()
        {
            this.PortionMenus = new List<PortionMenu>();
        }

        public int Id { get; set; }
        public string NameOnCheck { get; set; }
        public string NameOnKOT { get; set; }
        public decimal SizeMultiplier { get; set; }
        public decimal PriceMultiplier { get; set; }
        public Nullable<byte> RoundPriceTo { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<PortionMenu> PortionMenus { get; set; }
    }
}
