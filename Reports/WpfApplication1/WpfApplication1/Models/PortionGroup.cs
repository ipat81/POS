using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PortionGroup
    {
        public PortionGroup()
        {
            this.MenuItems = new List<MenuItem>();
            this.PortionMenus = new List<PortionMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string StdPortionNameKOT { get; set; }
        public string StdPortionNameCheck { get; set; }
        public byte UOMEnum { get; set; }
        public byte StdPortionSize { get; set; }
        public Nullable<byte> ValidSETypeEnum { get; set; }
        public bool ValidOnWeekDayLunch { get; set; }
        public bool ValidOnWeekDayDinner { get; set; }
        public bool ValidOnWeekEndLunch { get; set; }
        public bool ValidOnWeekEndDinner { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<PortionMenu> PortionMenus { get; set; }
    }
}
