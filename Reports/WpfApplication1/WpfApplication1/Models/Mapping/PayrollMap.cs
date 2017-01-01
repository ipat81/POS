using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PayrollMap : EntityTypeConfiguration<Payroll>
    {
        public PayrollMap()
        {
            // Primary Key
            this.HasKey(t => new { t.EmployeeId, t.PayPeriodId });

            // Properties
            this.Property(t => t.EmployeeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PayPeriodId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Payroll");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.PayPeriodId).HasColumnName("PayPeriodId");
            this.Property(t => t.EmployeePayInfoId).HasColumnName("EmployeePayInfoId");
            this.Property(t => t.WeekNumber).HasColumnName("WeekNumber");
            this.Property(t => t.ClockHrs).HasColumnName("ClockHrs");
            this.Property(t => t.ClockDays).HasColumnName("ClockDays");
            this.Property(t => t.QB1Hrs).HasColumnName("QB1Hrs");
            this.Property(t => t.QB1Days).HasColumnName("QB1Days");
            this.Property(t => t.QB2Hrs).HasColumnName("QB2Hrs");
            this.Property(t => t.QB2Days).HasColumnName("QB2Days");
            this.Property(t => t.CashTipPaid).HasColumnName("CashTipPaid");
            this.Property(t => t.CCTipPaid).HasColumnName("CCTipPaid");
            this.Property(t => t.HATipPaid).HasColumnName("HATipPaid");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Payrolls)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.Payrolls)
                .HasForeignKey(d => d.EmployeeId);
            this.HasRequired(t => t.PayPeriod)
                .WithMany(t => t.Payrolls)
                .HasForeignKey(d => d.PayPeriodId);

        }
    }
}
