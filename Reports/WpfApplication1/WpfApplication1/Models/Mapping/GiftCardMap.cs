using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class GiftCardMap : EntityTypeConfiguration<GiftCard>
    {
        public GiftCardMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("GiftCard");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.IssueAmount).HasColumnName("IssueAmount");
            this.Property(t => t.ValidTill).HasColumnName("ValidTill");
            this.Property(t => t.GCTypeEnum).HasColumnName("GCTypeEnum");
            this.Property(t => t.IssueMethodEnum).HasColumnName("IssueMethodEnum");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.GiftCards)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.Customer)
                .WithMany(t => t.GiftCards)
                .HasForeignKey(d => d.CustomerId);
            this.HasRequired(t => t.SE)
                .WithMany(t => t.GiftCards)
                .HasForeignKey(d => d.SEId);

        }
    }
}
