using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class GCTxnMap : EntityTypeConfiguration<GCTxn>
    {
        public GCTxnMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("GCTxn");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GiftCardId).HasColumnName("GiftCardId");
            this.Property(t => t.RedemptionAmount).HasColumnName("RedemptionAmount");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.GCTxns)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.GiftCard)
                .WithMany(t => t.GCTxns)
                .HasForeignKey(d => d.GiftCardId);
            this.HasRequired(t => t.SE)
                .WithMany(t => t.GCTxns)
                .HasForeignKey(d => d.SEId);

        }
    }
}
