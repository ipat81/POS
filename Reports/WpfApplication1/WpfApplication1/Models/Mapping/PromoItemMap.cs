using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PromoItemMap : EntityTypeConfiguration<PromoItem>
    {
        public PromoItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("PromoItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.PromoTypeEnum).HasColumnName("PromoTypeEnum");
            this.Property(t => t.UISortOrder).HasColumnName("UISortOrder");
            this.Property(t => t.CheckLayoutEnum).HasColumnName("CheckLayoutEnum");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.CouponTypeEnym).HasColumnName("CouponTypeEnym");
            this.Property(t => t.CouponValidityMonths).HasColumnName("CouponValidityMonths");
            this.Property(t => t.MinBaseAmount).HasColumnName("MinBaseAmount");
            this.Property(t => t.MaxBaseAmount).HasColumnName("MaxBaseAmount");
            this.Property(t => t.ValidSETypeEnum).HasColumnName("ValidSETypeEnum");
            this.Property(t => t.ValidWithOtherPromo).HasColumnName("ValidWithOtherPromo");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PromoItems)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
