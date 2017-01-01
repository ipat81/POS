using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PosMenuItemUIViewMap : EntityTypeConfiguration<PosMenuItemUIView>
    {
        public PosMenuItemUIViewMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("PosMenuItemUIView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PosMenuItemId).HasColumnName("PosMenuItemId");
            this.Property(t => t.PosUIViewId).HasColumnName("PosUIViewId");
            this.Property(t => t.ViewPosition).HasColumnName("ViewPosition");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PosMenuItemUIViews)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.PosMenuItem)
                .WithMany(t => t.PosMenuItemUIViews)
                .HasForeignKey(d => d.PosMenuItemId);
            this.HasRequired(t => t.PosUIView)
                .WithMany(t => t.PosMenuItemUIViews)
                .HasForeignKey(d => d.PosUIViewId);

        }
    }
}
