using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class PromoSchedule
    {
        public int Id { get; set; }
        public int PromoItemId { get; set; }
        public System.DateTime ValidDateFrom { get; set; }
        public Nullable<System.DateTime> ValidDateUntil { get; set; }
        public string ExcludeTimeFrom { get; set; }
        public string ExcludeTimeUntil { get; set; }
        public byte ValidSETypeEnum { get; set; }
        public bool ValidWithOtherPromo { get; set; }
        public bool ValidWeekDayLunch { get; set; }
        public bool ValidWeekDayDinner { get; set; }
        public bool ValidWeekEndLunch { get; set; }
        public bool ValidWeekEndDinner { get; set; }
        public byte Status { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual PromoItem PromoItem { get; set; }
    }
}
