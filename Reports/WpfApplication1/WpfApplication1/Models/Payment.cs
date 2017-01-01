using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Payment
    {
        public int ID { get; set; }
        public int SEID { get; set; }
        public int CashRegisterId { get; set; }
        public Nullable<int> GCTxnId { get; set; }
        public Nullable<byte> PaymentMethodAllowedEnum { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal POSAmountPaid { get; set; }
        public decimal TipAmountPaid { get; set; }
        public decimal AmountTendered { get; set; }
        public decimal ChangeAmountReturned { get; set; }
        public int CCInvoiceNo { get; set; }
        public int CCBatchNo { get; set; }
        public int CCRRN { get; set; }
        public int CCAuthorizationNo { get; set; }
        public byte Status { get; set; }
        public Nullable<int> DEError { get; set; }
        public Nullable<int> CRCountId { get; set; }
        public Nullable<int> OverrideId { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual CashRegister CashRegister { get; set; }
        public virtual GCTxn GCTxn { get; set; }
        public virtual Override Override { get; set; }
        public virtual SE SE { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
