using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class ModItemMap : EntityTypeConfiguration<ModItem>
    {
        public ModItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ModItemName)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(40);

            this.Property(t => t.ModLevel0Name)
                .HasMaxLength(10);

            this.Property(t => t.ModLevel1Name)
                .HasMaxLength(10);

            this.Property(t => t.ModLevel2Name)
                .HasMaxLength(10);

            this.Property(t => t.ModLevel3Name)
                .HasMaxLength(10);

            this.Property(t => t.ModLevel4Name)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("ModItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ModItemName).HasColumnName("ModItemName");
            this.Property(t => t.ModLevel0Name).HasColumnName("ModLevel0Name");
            this.Property(t => t.ModLevel1Name).HasColumnName("ModLevel1Name");
            this.Property(t => t.ModLevel2Name).HasColumnName("ModLevel2Name");
            this.Property(t => t.ModLevel3Name).HasColumnName("ModLevel3Name");
            this.Property(t => t.ModLevel4Name).HasColumnName("ModLevel4Name");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.ModItems)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
