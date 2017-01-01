using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PosMenuTabMap : EntityTypeConfiguration<PosMenuTab>
    {
        public PosMenuTabMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("PosMenuTab");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");
            this.Property(t => t.PositionHint).HasColumnName("PositionHint");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PosMenuTabs)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
