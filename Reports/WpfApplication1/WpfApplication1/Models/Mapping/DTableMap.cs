using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class DTableMap : EntityTypeConfiguration<DTable>
    {
        public DTableMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("DTable");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SeatingCapacity).HasColumnName("SeatingCapacity");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Orientation).HasColumnName("Orientation");
            this.Property(t => t.StartsInCell).HasColumnName("StartsInCell");
            this.Property(t => t.EndsInCell).HasColumnName("EndsInCell");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.DTables)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
