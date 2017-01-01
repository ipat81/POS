using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Employee
    {
        public Employee()
        {
            this.CRTxns = new List<CRTxn>();
            this.EmployeeContacts = new List<EmployeeContact>();
            this.EmployeePayInfoes = new List<EmployeePayInfo>();
            this.Escalations = new List<Escalation>();
            this.Orders = new List<Order>();
            this.Overrides = new List<Override>();
            this.Payrolls = new List<Payroll>();
            this.PayRollAdjusts = new List<PayRollAdjust>();
            this.SDSessions = new List<SDSession>();
            this.ServerTipAllocations = new List<ServerTipAllocation>();
            this.SETasks = new List<SETask>();
            this.TimeCards = new List<TimeCard>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public byte Sex { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> HireDate { get; set; }
        public Nullable<System.DateTime> TerminationDate { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<CRTxn> CRTxns { get; set; }
        public virtual Employee Employee1 { get; set; }
        public virtual Employee Employee2 { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<EmployeeContact> EmployeeContacts { get; set; }
        public virtual ICollection<EmployeePayInfo> EmployeePayInfoes { get; set; }
        public virtual ICollection<Escalation> Escalations { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Override> Overrides { get; set; }
        public virtual ICollection<Payroll> Payrolls { get; set; }
        public virtual ICollection<PayRollAdjust> PayRollAdjusts { get; set; }
        public virtual ICollection<SDSession> SDSessions { get; set; }
        public virtual ICollection<ServerTipAllocation> ServerTipAllocations { get; set; }
        public virtual ICollection<SETask> SETasks { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
    }
}
