using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Order");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.OrderTypeEnum).HasColumnName("OrderTypeEnum");
            this.Property(t => t.BaseAmount).HasColumnName("BaseAmount");
            this.Property(t => t.SalesTaxAmount).HasColumnName("SalesTaxAmount");
            this.Property(t => t.SalesTaxStatus).HasColumnName("SalesTaxStatus");
            this.Property(t => t.DiscountAmount).HasColumnName("DiscountAmount");
            this.Property(t => t.TipAmount).HasColumnName("TipAmount");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.HasVoids).HasColumnName("HasVoids");
            this.Property(t => t.NumberOfSplitChecks).HasColumnName("NumberOfSplitChecks");
            this.Property(t => t.OverrideId).HasColumnName("OverrideId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.EmployeeId);
            this.HasOptional(t => t.Override)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.OverrideId);
            this.HasOptional(t => t.SE)
                .WithMany(t => t.Orders)
                .HasForeignKey(d => d.SEId);

        }
    }
}
