using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PromoCouponIssuedMap : EntityTypeConfiguration<PromoCouponIssued>
    {
        public PromoCouponIssuedMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PromoCouponIssued");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.ValidTill).HasColumnName("ValidTill");
            this.Property(t => t.PromoItemId).HasColumnName("PromoItemId");
            this.Property(t => t.PromoCouponRedeemedId).HasColumnName("PromoCouponRedeemedId");
            this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasRequired(t => t.PromoItem)
                .WithMany(t => t.PromoCouponIssueds)
                .HasForeignKey(d => d.PromoItemId);
            this.HasRequired(t => t.SE)
                .WithMany(t => t.PromoCouponIssueds)
                .HasForeignKey(d => d.SEId);
            this.HasOptional(t => t.PromoCouponRedeemed)
                .WithMany(t => t.PromoCouponIssueds)
                .HasForeignKey(d => d.PromoCouponRedeemedId);

        }
    }
}
