using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class OverrideMap : EntityTypeConfiguration<Override>
    {
        public OverrideMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Reason)
                .HasMaxLength(120);

            // Table & Column Mappings
            this.ToTable("Override");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Reason).HasColumnName("Reason");
            this.Property(t => t.ApprovedBy).HasColumnName("ApprovedBy");
            this.Property(t => t.ApprovedAt).HasColumnName("ApprovedAt");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Overrides)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.Overrides)
                .HasForeignKey(d => d.ApprovedBy);

        }
    }
}
