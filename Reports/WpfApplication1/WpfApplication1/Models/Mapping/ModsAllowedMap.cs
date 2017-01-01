using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class ModsAllowedMap : EntityTypeConfiguration<ModsAllowed>
    {
        public ModsAllowedMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ProductId, t.ModItemId });

            // Properties
            this.Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ModItemId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ModsAllowed");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ModItemId).HasColumnName("ModItemId");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.ModsAlloweds)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.ModItem)
                .WithMany(t => t.ModsAlloweds)
                .HasForeignKey(d => d.ModItemId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.ModsAlloweds)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
