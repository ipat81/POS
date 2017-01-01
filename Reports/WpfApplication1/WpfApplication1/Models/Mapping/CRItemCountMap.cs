using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class CRItemCountMap : EntityTypeConfiguration<CRItemCount>
    {
        public CRItemCountMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SDSessionId, t.CashRegisterId, t.CRItemId, t.CountTypeEnum });

            // Properties
            this.Property(t => t.SDSessionId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.CashRegisterId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CRItemId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("CRItemCount");
            this.Property(t => t.SDSessionId).HasColumnName("SDSessionId");
            this.Property(t => t.CashRegisterId).HasColumnName("CashRegisterId");
            this.Property(t => t.CRItemId).HasColumnName("CRItemId");
            this.Property(t => t.CountTypeEnum).HasColumnName("CountTypeEnum");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.OverrideId).HasColumnName("OverrideId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.CRItemCounts)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.CashRegister)
                .WithMany(t => t.CRItemCounts)
                .HasForeignKey(d => d.CashRegisterId);
            this.HasRequired(t => t.CRItem)
                .WithMany(t => t.CRItemCounts)
                .HasForeignKey(d => d.CRItemId);
            this.HasOptional(t => t.Override)
                .WithMany(t => t.CRItemCounts)
                .HasForeignKey(d => d.OverrideId);
            this.HasRequired(t => t.SDSession)
                .WithMany(t => t.CRItemCounts)
                .HasForeignKey(d => d.SDSessionId);

        }
    }
}
