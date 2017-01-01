using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SETaskMap : EntityTypeConfiguration<SETask>
    {
        public SETaskMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SETask");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.TaskId).HasColumnName("TaskId");
            this.Property(t => t.ActStartTime).HasColumnName("ActStartTime");
            this.Property(t => t.ActEndTime).HasColumnName("ActEndTime");
            this.Property(t => t.EstStartTime).HasColumnName("EstStartTime");
            this.Property(t => t.EstEndTime).HasColumnName("EstEndTime");
            this.Property(t => t.TaskDoneBy).HasColumnName("TaskDoneBy");
            this.Property(t => t.OrderItemId).HasColumnName("OrderItemId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SETasks)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.SETasks)
                .HasForeignKey(d => d.TaskDoneBy);
            this.HasRequired(t => t.SE)
                .WithMany(t => t.SETasks)
                .HasForeignKey(d => d.SEId);
            this.HasRequired(t => t.Task)
                .WithMany(t => t.SETasks)
                .HasForeignKey(d => d.TaskId);

        }
    }
}
