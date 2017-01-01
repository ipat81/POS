using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class OrderViewMap : EntityTypeConfiguration<OrderView>
    {
        public OrderViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //this.Property(t => t.EmployeeId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("OrderView");
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
        }
    }
}
