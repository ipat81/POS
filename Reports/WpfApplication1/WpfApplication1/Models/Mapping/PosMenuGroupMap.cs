using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PosMenuGroupMap : EntityTypeConfiguration<PosMenuGroup>
    {
        public PosMenuGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PosMenuGroup");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");
            this.Property(t => t.PositionHint).HasColumnName("PositionHint");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PosMenuGroups)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
