using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class OrderItemViewMap : EntityTypeConfiguration<OrderItemView>
    {
        public OrderItemViewMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.OrderId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.MenuId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.ProductId)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Course)
                .IsRequired()
                .HasMaxLength(20);

            //this.Property(t => t.Quantity)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.Price)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //this.Property(t => t.Expr1)
            //    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("OrderItemView");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderViewId).HasColumnName("OrderId");
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
            this.Property(t => t.Expr1).HasColumnName("Expr1");
            
            this.HasRequired(t => t.OrderView)
               .WithMany(t => t.OrderItemViews)
               .HasForeignKey(d => d.OrderViewId);
        }
    }
}
