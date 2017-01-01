using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class TimeCardMap : EntityTypeConfiguration<TimeCard>
    {
        public TimeCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TimeCard");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.ClockType).HasColumnName("ClockType");
            this.Property(t => t.TimeIn).HasColumnName("TimeIn");
            this.Property(t => t.TimeOut).HasColumnName("TimeOut");
            this.Property(t => t.SaleDate).HasColumnName("SaleDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.OverrideId).HasColumnName("OverrideId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.TimeCards)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.TimeCards)
                .HasForeignKey(d => d.EmployeeId);
            this.HasOptional(t => t.Override)
                .WithMany(t => t.TimeCards)
                .HasForeignKey(d => d.OverrideId);
            this.HasOptional(t => t.Role)
                .WithMany(t => t.TimeCards)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
