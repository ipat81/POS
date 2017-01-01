using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PortionMap : EntityTypeConfiguration<Portion>
    {
        public PortionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.NameOnCheck)
                .HasMaxLength(20);

            this.Property(t => t.NameOnKOT)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Portion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NameOnCheck).HasColumnName("NameOnCheck");
            this.Property(t => t.NameOnKOT).HasColumnName("NameOnKOT");
            this.Property(t => t.SizeMultiplier).HasColumnName("SizeMultiplier");
            this.Property(t => t.PriceMultiplier).HasColumnName("PriceMultiplier");
            this.Property(t => t.RoundPriceTo).HasColumnName("RoundPriceTo");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Portions)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
