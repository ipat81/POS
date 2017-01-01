using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.NameOnCheck)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.NameOnKOT)
                .HasMaxLength(60);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Product");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NameOnCheck).HasColumnName("NameOnCheck");
            this.Property(t => t.NameOnKOT).HasColumnName("NameOnKOT");
            this.Property(t => t.ProductType).HasColumnName("ProductType");
            this.Property(t => t.STaxExempt).HasColumnName("STaxExempt");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DiscountAllowed).HasColumnName("DiscountAllowed");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.FreeProductCategoryIncluded).HasColumnName("FreeProductCategoryIncluded");
            this.Property(t => t.CuisineEnum).HasColumnName("CuisineEnum");
            this.Property(t => t.KitchenStationEnum).HasColumnName("KitchenStationEnum");
            this.Property(t => t.ServiceStationEnum).HasColumnName("ServiceStationEnum");
            this.Property(t => t.DefaultCourseEnum).HasColumnName("DefaultCourseEnum");
            this.Property(t => t.UISortOrder).HasColumnName("UISortOrder");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
