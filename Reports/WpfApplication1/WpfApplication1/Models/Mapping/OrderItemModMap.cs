using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class OrderItemModMap : EntityTypeConfiguration<OrderItemMod>
    {
        public OrderItemModMap()
        {
            // Primary Key
            this.HasKey(t => new { t.OrderItemId, t.ModItemId });

            // Properties
            this.Property(t => t.OrderItemId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ModItemId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SelectedModLevelName)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("OrderItemMod");
            this.Property(t => t.OrderItemId).HasColumnName("OrderItemId");
            this.Property(t => t.ModItemId).HasColumnName("ModItemId");
            this.Property(t => t.SelectedModLevelName).HasColumnName("SelectedModLevelName");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.OrderItemMods)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.ModItem)
                .WithMany(t => t.OrderItemMods)
                .HasForeignKey(d => d.ModItemId);
            this.HasRequired(t => t.OrderItem)
                .WithMany(t => t.OrderItemMods)
                .HasForeignKey(d => d.OrderItemId);

        }
    }
}
