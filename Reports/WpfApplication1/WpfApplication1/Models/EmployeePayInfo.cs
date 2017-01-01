using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class EmployeePayInfo
    {
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public string EmployeeSSN { get; set; }
        public byte PayMethodEnum { get; set; }
        public byte PayRateTypeEnum { get; set; }
        public Nullable<decimal> PayRateQB1 { get; set; }
        public Nullable<decimal> PayRateQB2 { get; set; }
        public Nullable<byte> FactorQB1 { get; set; }
        public Nullable<byte> FactorQB2 { get; set; }
        public string PayState { get; set; }
        public Nullable<byte> FedExemptions { get; set; }
        public Nullable<byte> StateExemptions { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Role Role { get; set; }
    }
}
