using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class CRItemMap : EntityTypeConfiguration<CRItem>
    {
        public CRItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("CRItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.CRItems)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
