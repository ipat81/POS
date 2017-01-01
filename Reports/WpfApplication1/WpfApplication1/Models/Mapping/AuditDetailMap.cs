using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class AuditDetailMap : EntityTypeConfiguration<AuditDetail>
    {
        public AuditDetailMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PropertyName)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.OldValue)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.NewValue)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AuditDetail");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PropertyName).HasColumnName("PropertyName");
            this.Property(t => t.OldValue).HasColumnName("OldValue");
            this.Property(t => t.NewValue).HasColumnName("NewValue");
            this.Property(t => t.AuditChangeSetId).HasColumnName("AuditChangeSetId");

            // Relationships
            this.HasRequired(t => t.AuditChangeSet)
                .WithMany(t => t.AuditDetails)
                .HasForeignKey(d => d.AuditChangeSetId);

        }
    }
}
