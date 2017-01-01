using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SDSessionMap : EntityTypeConfiguration<SDSession>
    {
        public SDSessionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SDSession");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SDId).HasColumnName("SDId");
            this.Property(t => t.SessionFrom).HasColumnName("SessionFrom");
            this.Property(t => t.SessionTo).HasColumnName("SessionTo");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.MenuId).HasColumnName("MenuId");
            this.Property(t => t.SpecialMenuId).HasColumnName("SpecialMenuId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ReconciliationApprovedBy).HasColumnName("ReconciliationApprovedBy");
            this.Property(t => t.ReconciliationApprovedAt).HasColumnName("ReconciliationApprovedAt");
            this.Property(t => t.SuspendPromoMenuFrom).HasColumnName("SuspendPromoMenuFrom");
            this.Property(t => t.SuspendPromoMenuUntil).HasColumnName("SuspendPromoMenuUntil");
            this.Property(t => t.SuspendPortionMenuFrom).HasColumnName("SuspendPortionMenuFrom");
            this.Property(t => t.SuspendPortionMenuUntil).HasColumnName("SuspendPortionMenuUntil");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SDSessions)
                .HasForeignKey(d => d.AuditId);
            this.HasOptional(t => t.Employee)
                .WithMany(t => t.SDSessions)
                .HasForeignKey(d => d.ReconciliationApprovedBy);
            this.HasOptional(t => t.Menu)
                .WithMany(t => t.SDSessions)
                .HasForeignKey(d => d.MenuId);
            this.HasOptional(t => t.Menu1)
                .WithMany(t => t.SDSessions1)
                .HasForeignKey(d => d.SpecialMenuId);
            this.HasOptional(t => t.SD)
                .WithMany(t => t.SDSessions)
                .HasForeignKey(d => d.SDId);

        }
    }
}
