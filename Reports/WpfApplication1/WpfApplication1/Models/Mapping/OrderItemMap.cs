using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Course)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("OrderItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.PromoSubProductId).HasColumnName("PromoSubProductId");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.Course).HasColumnName("Course");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.PortionId).HasColumnName("PortionId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CheckNumber).HasColumnName("CheckNumber");
            this.Property(t => t.OverrideId).HasColumnName("OverrideId");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasRequired(t => t.MenuItem)
                .WithMany(t => t.OrderItems)
                .HasForeignKey(d => new { d.MenuId, d.ProductId });
            this.HasRequired(t => t.Order)
                .WithMany(t => t.OrderItems)
                .HasForeignKey(d => d.OrderId);

        }
    }
}
