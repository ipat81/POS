using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PayRollAdjustMap : EntityTypeConfiguration<PayRollAdjust>
    {
        public PayRollAdjustMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Notes)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("PayRollAdjust");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.PayPeriodId).HasColumnName("PayPeriodId");
            this.Property(t => t.AdjustTypeEnum).HasColumnName("AdjustTypeEnum");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Hrs).HasColumnName("Hrs");
            this.Property(t => t.CRTxnId).HasColumnName("CRTxnId");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PayRollAdjusts)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.CRTxn)
                .WithMany(t => t.PayRollAdjusts)
                .HasForeignKey(d => d.CRTxnId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.PayRollAdjusts)
                .HasForeignKey(d => d.EmployeeId);
            this.HasRequired(t => t.Payroll)
                .WithMany(t => t.PayRollAdjusts)
                .HasForeignKey(d => new { d.EmployeeId, d.PayPeriodId });
            this.HasOptional(t => t.SE)
                .WithMany(t => t.PayRollAdjusts)
                .HasForeignKey(d => d.SEId);

        }
    }
}
