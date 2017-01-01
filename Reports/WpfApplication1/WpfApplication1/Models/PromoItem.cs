using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PromoItem
    {
        public PromoItem()
        {
            this.PromoCouponIssueds = new List<PromoCouponIssued>();
            this.PromoSchedules = new List<PromoSchedule>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte PromoTypeEnum { get; set; }
        public byte UISortOrder { get; set; }
        public byte CheckLayoutEnum { get; set; }
        public Nullable<byte> Discount { get; set; }
        public Nullable<byte> CouponTypeEnym { get; set; }
        public Nullable<byte> CouponValidityMonths { get; set; }
        public Nullable<decimal> MinBaseAmount { get; set; }
        public Nullable<decimal> MaxBaseAmount { get; set; }
        public Nullable<byte> ValidSETypeEnum { get; set; }
        public bool ValidWithOtherPromo { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<PromoCouponIssued> PromoCouponIssueds { get; set; }
        public virtual ICollection<PromoSchedule> PromoSchedules { get; set; }
    }
}
