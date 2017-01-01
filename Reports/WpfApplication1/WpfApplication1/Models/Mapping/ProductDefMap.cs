using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class ProductDefMap : EntityTypeConfiguration<ProductDef>
    {
        public ProductDefMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ParentProductId, t.ProductId });

            // Properties
            this.Property(t => t.ParentProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ProductId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("ProductDef");
            this.Property(t => t.ParentProductId).HasColumnName("ParentProductId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.UISortOrder).HasColumnName("UISortOrder");
            this.Property(t => t.KOTLayoutEnum).HasColumnName("KOTLayoutEnum");
            this.Property(t => t.CheckLayoutEnum).HasColumnName("CheckLayoutEnum");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.ProductDefs)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.ProductDefs)
                .HasForeignKey(d => d.ProductId);
            this.HasRequired(t => t.Product1)
                .WithMany(t => t.ProductDefs1)
                .HasForeignKey(d => d.ParentProductId);

        }
    }
}
