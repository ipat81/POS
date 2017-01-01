using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SubscriberViewPosMenuTabGroupMenuItemMap : EntityTypeConfiguration<SubscriberViewPosMenuTabGroupMenuItem>
    {
        public SubscriberViewPosMenuTabGroupMenuItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SubscriberViewPosMenuTabGroupMenuItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SubscriberViewPosMenuTabGroupId).HasColumnName("SubscriberViewPosMenuTabGroupId");
            this.Property(t => t.PosMenuItemId).HasColumnName("PosMenuItemId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SubscriberViewPosMenuTabGroupMenuItems)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.PosMenuItem)
                .WithMany(t => t.SubscriberViewPosMenuTabGroupMenuItems)
                .HasForeignKey(d => d.PosMenuItemId);
            this.HasRequired(t => t.SubscriberViewPosMenuTabGroup)
                .WithMany(t => t.SubscriberViewPosMenuTabGroupMenuItems)
                .HasForeignKey(d => d.SubscriberViewPosMenuTabGroupId);

        }
    }
}
