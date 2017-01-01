using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PromoCouponRedeemed
    {
        public PromoCouponRedeemed()
        {
            this.PromoCouponIssueds = new List<PromoCouponIssued>();
        }

        public int Id { get; set; }
        public int SEId { get; set; }
        public decimal ActualDiscount { get; set; }
        public int PromoCouponIssuedId { get; set; }
        public byte Status { get; set; }
        public virtual ICollection<PromoCouponIssued> PromoCouponIssueds { get; set; }
        public virtual SE SE { get; set; }
    }
}
