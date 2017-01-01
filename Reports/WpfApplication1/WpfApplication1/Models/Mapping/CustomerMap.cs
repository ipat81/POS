using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Customer");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TypeEnum).HasColumnName("TypeEnum");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.STaxExempt).HasColumnName("STaxExempt");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Customers)
                .HasForeignKey(d => d.AuditId);

        }
    }
}
