using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class AuditChangeSetMap : EntityTypeConfiguration<AuditChangeSet>
    {
        public AuditChangeSetMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("AuditChangeSet");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ChangedBy).HasColumnName("ChangedBy");
            this.Property(t => t.ChangedAt).HasColumnName("ChangedAt");
            this.Property(t => t.UserAction).HasColumnName("UserAction");
            this.Property(t => t.WorkstationId).HasColumnName("WorkstationId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasRequired(t => t.Audit)
                .WithMany(t => t.AuditChangeSets)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
