using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class MenuMap : EntityTypeConfiguration<Menu>
    {
        public MenuMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Menu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SessionId).HasColumnName("SessionId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MenuTypeEnum).HasColumnName("MenuTypeEnum");
            this.Property(t => t.InEffetFrom).HasColumnName("InEffetFrom");
            this.Property(t => t.InEffectUntil).HasColumnName("InEffectUntil");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.Menus)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Session)
                .WithMany(t => t.Menus)
                .HasForeignKey(d => d.SessionId);

        }
    }
}
