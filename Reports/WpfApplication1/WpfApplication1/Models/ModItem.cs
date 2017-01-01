using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class ModItem
    {
        public ModItem()
        {
            this.ModsAlloweds = new List<ModsAllowed>();
            this.OrderItemMods = new List<OrderItemMod>();
        }

        public int Id { get; set; }
        public string ModItemName { get; set; }
        public string ModLevel0Name { get; set; }
        public string ModLevel1Name { get; set; }
        public string ModLevel2Name { get; set; }
        public string ModLevel3Name { get; set; }
        public string ModLevel4Name { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<ModsAllowed> ModsAlloweds { get; set; }
        public virtual ICollection<OrderItemMod> OrderItemMods { get; set; }
    }
}
