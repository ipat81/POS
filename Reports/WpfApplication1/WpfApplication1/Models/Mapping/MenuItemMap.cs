using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class MenuItemMap : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MenuId, t.ProductId });

            // Properties
            this.Property(t => t.MenuId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("MenuItem");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StdPortionPrice).HasColumnName("StdPortionPrice");
            this.Property(t => t.PortionGroupId).HasColumnName("PortionGroupId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.MenuItems)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Menu)
                .WithMany(t => t.MenuItems)
                .HasForeignKey(d => d.MenuId);
            this.HasOptional(t => t.PortionGroup)
                .WithMany(t => t.MenuItems)
                .HasForeignKey(d => d.PortionGroupId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.MenuItems)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
