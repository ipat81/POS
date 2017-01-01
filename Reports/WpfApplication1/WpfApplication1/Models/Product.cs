using System;
using System.Collections.Generic;

namespace WpfApplication1.Models
{
    public partial class Product
    {
        public Product()
        {
            this.MenuItems = new List<MenuItem>();
            this.ModsAlloweds = new List<ModsAllowed>();
            this.ProductDefs = new List<ProductDef>();
            this.ProductDefs1 = new List<ProductDef>();
        }

        public int Id { get; set; }
        public string NameOnCheck { get; set; }
        public string NameOnKOT { get; set; }
        public Nullable<byte> ProductType { get; set; }
        public Nullable<bool> STaxExempt { get; set; }
        public byte Status { get; set; }
        public bool DiscountAllowed { get; set; }
        public string Description { get; set; }
        public Nullable<int> FreeProductCategoryIncluded { get; set; }
        public Nullable<byte> CuisineEnum { get; set; }
        public Nullable<byte> KitchenStationEnum { get; set; }
        public Nullable<byte> ServiceStationEnum { get; set; }
        public Nullable<byte> DefaultCourseEnum { get; set; }
        public Nullable<byte> UISortOrder { get; set; }
        public Nullable<long> AuditId { get; set; }
        public virtual Audit Audit { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        public virtual ICollection<ModsAllowed> ModsAlloweds { get; set; }
        public virtual ICollection<ProductDef> ProductDefs { get; set; }
        public virtual ICollection<ProductDef> ProductDefs1 { get; set; }
    }
}
