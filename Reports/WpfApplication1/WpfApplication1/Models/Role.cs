using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Role
    {
        public Role()
        {
            this.Employees = new List<Employee>();
            this.EmployeePayInfoes = new List<EmployeePayInfo>();
            this.POSFunctions = new List<POSFunction>();
            this.PosMenuItems = new List<PosMenuItem>();
            this.TimeCards = new List<TimeCard>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<EmployeePayInfo> EmployeePayInfoes { get; set; }
        public virtual ICollection<POSFunction> POSFunctions { get; set; }
        public virtual ICollection<PosMenuItem> PosMenuItems { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
    }
}
