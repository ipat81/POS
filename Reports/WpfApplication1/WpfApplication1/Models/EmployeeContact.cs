using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class EmployeeContact
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public byte ContactTypeEnum { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string EMail1 { get; set; }
        public string EMail2 { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
