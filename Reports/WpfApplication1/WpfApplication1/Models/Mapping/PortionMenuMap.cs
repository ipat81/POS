using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PortionMenuMap : EntityTypeConfiguration<PortionMenu>
    {
        public PortionMenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PortionMenu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PortionGroupId).HasColumnName("PortionGroupId");
            this.Property(t => t.PortionId).HasColumnName("PortionId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PortionMenus)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Portion)
                .WithMany(t => t.PortionMenus)
                .HasForeignKey(d => d.PortionId);
            this.HasRequired(t => t.PortionGroup)
                .WithMany(t => t.PortionMenus)
                .HasForeignKey(d => d.PortionGroupId);

        }
    }
}
