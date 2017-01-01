using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SEMap : EntityTypeConfiguration<SE>
    {
        public SEMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Notes)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SE");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.SaleDate).HasColumnName("SaleDate");
            this.Property(t => t.SDSessionId).HasColumnName("SDSessionId");
            this.Property(t => t.SESourceEnum).HasColumnName("SESourceEnum");
            this.Property(t => t.SETypeEnum).HasColumnName("SETypeEnum");
            this.Property(t => t.DesiredTime).HasColumnName("DesiredTime");
            this.Property(t => t.AdultHeadcount).HasColumnName("AdultHeadcount");
            this.Property(t => t.TeenHeadCount).HasColumnName("TeenHeadCount");
            this.Property(t => t.KidHeadcount).HasColumnName("KidHeadcount");
            this.Property(t => t.ConfirmEnum).HasColumnName("ConfirmEnum");
            this.Property(t => t.ConfirmTime).HasColumnName("ConfirmTime");
            this.Property(t => t.ConfirmMethodEnum).HasColumnName("ConfirmMethodEnum");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SEs)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.SDSession)
                .WithMany(t => t.SEs)
                .HasForeignKey(d => d.SDSessionId);

        }
    }
}
