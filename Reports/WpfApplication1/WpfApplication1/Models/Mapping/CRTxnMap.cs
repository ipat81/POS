using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class CRTxnMap : EntityTypeConfiguration<CRTxn>
    {
        public CRTxnMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comments)
                .IsRequired()
                .HasMaxLength(52);

            // Table & Column Mappings
            this.ToTable("CRTxn");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CashRegisterId).HasColumnName("CashRegisterId");
            this.Property(t => t.TxnTypeEnum).HasColumnName("TxnTypeEnum");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.EmployeeIdFor).HasColumnName("EmployeeIdFor");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ReceiptGiven).HasColumnName("ReceiptGiven");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.CRTxns)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.CashRegister)
                .WithMany(t => t.CRTxns)
                .HasForeignKey(d => d.CashRegisterId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.CRTxns)
                .HasForeignKey(d => d.EmployeeIdFor);

        }
    }
}
