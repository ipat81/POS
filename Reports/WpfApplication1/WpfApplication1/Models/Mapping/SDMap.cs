using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SDMap : EntityTypeConfiguration<SD>
    {
        public SDMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SD");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SaleDate).HasColumnName("SaleDate");
            this.Property(t => t.OpenAt).HasColumnName("OpenAt");
            this.Property(t => t.OpenBy).HasColumnName("OpenBy");
            this.Property(t => t.CloseAt).HasColumnName("CloseAt");
            this.Property(t => t.CloseBy).HasColumnName("CloseBy");
            this.Property(t => t.SaleDayTypeEnum).HasColumnName("SaleDayTypeEnum");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SDs)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
