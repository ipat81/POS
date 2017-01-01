using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class ServerTipAllocationMap : EntityTypeConfiguration<ServerTipAllocation>
    {
        public ServerTipAllocationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.EmployeeId, t.SEId });

            // Properties
            this.Property(t => t.EmployeeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SEId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ServerTipAllocation");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.CashTipDue).HasColumnName("CashTipDue");
            this.Property(t => t.CCTipDue).HasColumnName("CCTipDue");
            this.Property(t => t.HATipDue).HasColumnName("HATipDue");
            this.Property(t => t.CashTipPaid).HasColumnName("CashTipPaid");
            this.Property(t => t.CashTipPaidOn).HasColumnName("CashTipPaidOn");
            this.Property(t => t.TipToPayroll).HasColumnName("TipToPayroll");
            this.Property(t => t.PayPeriodId).HasColumnName("PayPeriodId");
            this.Property(t => t.CRTxnId).HasColumnName("CRTxnId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.ServerTipAllocations)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.CRTxn)
                .WithMany(t => t.ServerTipAllocations)
                .HasForeignKey(d => d.CRTxnId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.ServerTipAllocations)
                .HasForeignKey(d => d.EmployeeId);
            this.HasOptional(t => t.Payroll)
                .WithMany(t => t.ServerTipAllocations)
                .HasForeignKey(d => new { d.EmployeeId, d.PayPeriodId });
            this.HasRequired(t => t.SE)
                .WithMany(t => t.ServerTipAllocations)
                .HasForeignKey(d => d.SEId);

        }
    }
}
