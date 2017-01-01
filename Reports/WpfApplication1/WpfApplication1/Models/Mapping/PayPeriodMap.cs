using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PayPeriodMap : EntityTypeConfiguration<PayPeriod>
    {
        public PayPeriodMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("PayPeriod");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Starts).HasColumnName("Starts");
            this.Property(t => t.Ends).HasColumnName("Ends");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.TotalKitchenPay).HasColumnName("TotalKitchenPay");
            this.Property(t => t.TotalKitchenHelpPay).HasColumnName("TotalKitchenHelpPay");
            this.Property(t => t.TotalServicePay).HasColumnName("TotalServicePay");
            this.Property(t => t.TotalSalary).HasColumnName("TotalSalary");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PayPeriods)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
