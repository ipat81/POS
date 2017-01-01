using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class CRTxn
    {
        public CRTxn()
        {
            this.PayRollAdjusts = new List<PayRollAdjust>();
            this.ServerTipAllocations = new List<ServerTipAllocation>();
        }

        public int Id { get; set; }
        public int CashRegisterId { get; set; }
        public byte TxnTypeEnum { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> EmployeeIdFor { get; set; }
        public string Comments { get; set; }
        public byte Status { get; set; }
        public Nullable<bool> ReceiptGiven { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual CashRegister CashRegister { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual ICollection<PayRollAdjust> PayRollAdjusts { get; set; }
        public virtual ICollection<ServerTipAllocation> ServerTipAllocations { get; set; }
    }
}
