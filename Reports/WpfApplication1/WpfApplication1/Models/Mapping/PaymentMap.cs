using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PaymentMap : EntityTypeConfiguration<Payment>
    {
        public PaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("Payment");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.SEID).HasColumnName("SEID");
            this.Property(t => t.CashRegisterId).HasColumnName("CashRegisterId");
            this.Property(t => t.GCTxnId).HasColumnName("GCTxnId");
            this.Property(t => t.PaymentMethodAllowedEnum).HasColumnName("PaymentMethodAllowedEnum");
            this.Property(t => t.PaymentMethodId).HasColumnName("PaymentMethodId");
            this.Property(t => t.POSAmountPaid).HasColumnName("POSAmountPaid");
            this.Property(t => t.TipAmountPaid).HasColumnName("TipAmountPaid");
            this.Property(t => t.AmountTendered).HasColumnName("AmountTendered");
            this.Property(t => t.ChangeAmountReturned).HasColumnName("ChangeAmountReturned");
            this.Property(t => t.CCInvoiceNo).HasColumnName("CCInvoiceNo");
            this.Property(t => t.CCBatchNo).HasColumnName("CCBatchNo");
            this.Property(t => t.CCRRN).HasColumnName("CCRRN");
            this.Property(t => t.CCAuthorizationNo).HasColumnName("CCAuthorizationNo");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.DEError).HasColumnName("DEError");
            this.Property(t => t.CRCountId).HasColumnName("CRCountId");
            this.Property(t => t.OverrideId).HasColumnName("OverrideId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.CashRegister)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.CashRegisterId);
            this.HasOptional(t => t.GCTxn)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.GCTxnId);
            this.HasOptional(t => t.Override)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.OverrideId);
            this.HasRequired(t => t.SE)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.SEID);
            this.HasRequired(t => t.PaymentMethod)
                .WithMany(t => t.Payments)
                .HasForeignKey(d => d.PaymentMethodId);

        }
    }
}
