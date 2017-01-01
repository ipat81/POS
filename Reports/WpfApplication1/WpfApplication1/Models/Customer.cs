using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Customer
    {
        public Customer()
        {
            this.CustomerContacts = new List<CustomerContact>();
            this.GiftCards = new List<GiftCard>();
        }

        public int Id { get; set; }
        public byte TypeEnum { get; set; }
        public string Name { get; set; }
        public Nullable<bool> STaxExempt { get; set; }
        public byte Status { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<CustomerContact> CustomerContacts { get; set; }
        public virtual ICollection<GiftCard> GiftCards { get; set; }
    }
}
