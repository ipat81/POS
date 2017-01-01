using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PortionGroupMap : EntityTypeConfiguration<PortionGroup>
    {
        public PortionGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.StdPortionNameKOT)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.StdPortionNameCheck)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("PortionGroup");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.StdPortionNameKOT).HasColumnName("StdPortionNameKOT");
            this.Property(t => t.StdPortionNameCheck).HasColumnName("StdPortionNameCheck");
            this.Property(t => t.UOMEnum).HasColumnName("UOMEnum");
            this.Property(t => t.StdPortionSize).HasColumnName("StdPortionSize");
            this.Property(t => t.ValidSETypeEnum).HasColumnName("ValidSETypeEnum");
            this.Property(t => t.ValidOnWeekDayLunch).HasColumnName("ValidOnWeekDayLunch");
            this.Property(t => t.ValidOnWeekDayDinner).HasColumnName("ValidOnWeekDayDinner");
            this.Property(t => t.ValidOnWeekEndLunch).HasColumnName("ValidOnWeekEndLunch");
            this.Property(t => t.ValidOnWeekEndDinner).HasColumnName("ValidOnWeekEndDinner");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PortionGroups)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
