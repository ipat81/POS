using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class EscalationMap : EntityTypeConfiguration<Escalation>
    {
        public EscalationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Escalation");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SETaskId).HasColumnName("SETaskId");
            this.Property(t => t.EmployeeIdEscalatedTo).HasColumnName("EmployeeIdEscalatedTo");
            this.Property(t => t.EscalatedAt).HasColumnName("EscalatedAt");
            this.Property(t => t.PriorityEnum).HasColumnName("PriorityEnum");
            this.Property(t => t.RemedialActionEnum).HasColumnName("RemedialActionEnum");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Escalations)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.Escalations)
                .HasForeignKey(d => d.EmployeeIdEscalatedTo);
            this.HasRequired(t => t.SETask)
                .WithMany(t => t.Escalations)
                .HasForeignKey(d => d.SETaskId);

        }
    }
}
