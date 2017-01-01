using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PromoCouponRedeemedMap : EntityTypeConfiguration<PromoCouponRedeemed>
    {
        public PromoCouponRedeemedMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PromoCouponRedeemed");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.ActualDiscount).HasColumnName("ActualDiscount");
            this.Property(t => t.PromoCouponIssuedId).HasColumnName("PromoCouponIssuedId");
            this.Property(t => t.Status).HasColumnName("Status");

            // Relationships
            this.HasRequired(t => t.SE)
                .WithMany(t => t.PromoCouponRedeemeds)
                .HasForeignKey(d => d.SEId);

        }
    }
}
