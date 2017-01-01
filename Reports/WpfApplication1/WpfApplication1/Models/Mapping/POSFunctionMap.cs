using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class POSFunctionMap : EntityTypeConfiguration<POSFunction>
    {
        public POSFunctionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("POSFunction");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.TempAccessAllowedFrom).HasColumnName("TempAccessAllowedFrom");
            this.Property(t => t.TempAccessAllowedUntil).HasColumnName("TempAccessAllowedUntil");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.POSFunctions)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.POSFunctions)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
