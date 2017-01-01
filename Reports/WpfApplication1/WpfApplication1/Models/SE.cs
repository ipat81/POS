using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class SE
    {
        public SE()
        {
            this.GCTxns = new List<GCTxn>();
            this.GiftCards = new List<GiftCard>();
            this.Orders = new List<Order>();
            this.Payments = new List<Payment>();
            this.PayRollAdjusts = new List<PayRollAdjust>();
            this.PromoCouponIssueds = new List<PromoCouponIssued>();
            this.PromoCouponRedeemeds = new List<PromoCouponRedeemed>();
            this.ServerTipAllocations = new List<ServerTipAllocation>();
            this.SETasks = new List<SETask>();
            this.TableSeatings = new List<TableSeating>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public System.DateTime SaleDate { get; set; }
        public int SDSessionId { get; set; }
        public byte SESourceEnum { get; set; }
        public byte SETypeEnum { get; set; }
        public System.DateTime DesiredTime { get; set; }
        public Nullable<byte> AdultHeadcount { get; set; }
        public Nullable<byte> TeenHeadCount { get; set; }
        public Nullable<byte> KidHeadcount { get; set; }
        public byte ConfirmEnum { get; set; }
        public Nullable<System.DateTime> ConfirmTime { get; set; }
        public Nullable<byte> ConfirmMethodEnum { get; set; }
        public byte Status { get; set; }
        public string Notes { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<GCTxn> GCTxns { get; set; }
        public virtual ICollection<GiftCard> GiftCards { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<PayRollAdjust> PayRollAdjusts { get; set; }
        public virtual ICollection<PromoCouponIssued> PromoCouponIssueds { get; set; }
        public virtual ICollection<PromoCouponRedeemed> PromoCouponRedeemeds { get; set; }
        public virtual SDSession SDSession { get; set; }
        public virtual ICollection<ServerTipAllocation> ServerTipAllocations { get; set; }
        public virtual ICollection<SETask> SETasks { get; set; }
        public virtual ICollection<TableSeating> TableSeatings { get; set; }
    }
}
