using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PromoCouponIssued
    {
        public int Id { get; set; }
        public int SEId { get; set; }
        public Nullable<System.DateTime> ValidTill { get; set; }
        public int PromoItemId { get; set; }
        public Nullable<int> PromoCouponRedeemedId { get; set; }
        public byte Status { get; set; }
        public virtual PromoItem PromoItem { get; set; }
        public virtual SE SE { get; set; }
        public virtual PromoCouponRedeemed PromoCouponRedeemed { get; set; }
    }
}
