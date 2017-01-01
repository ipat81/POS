using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Session
    {
        public Session()
        {
            this.Menus = new List<Menu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public System.TimeSpan SessionFrom { get; set; }
        public System.TimeSpan SessionTo { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<Menu> Menus { get; set; }
    }
}
