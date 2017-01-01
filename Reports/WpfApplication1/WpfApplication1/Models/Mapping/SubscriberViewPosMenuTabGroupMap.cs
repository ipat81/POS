using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SubscriberViewPosMenuTabGroupMap : EntityTypeConfiguration<SubscriberViewPosMenuTabGroup>
    {
        public SubscriberViewPosMenuTabGroupMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SubscriberViewPosMenuTabGroup");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SubscriberViewPosMenuTabId).HasColumnName("SubscriberViewPosMenuTabId");
            this.Property(t => t.PosMenuGroupId).HasColumnName("PosMenuGroupId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SubscriberViewPosMenuTabGroups)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.PosMenuGroup)
                .WithMany(t => t.SubscriberViewPosMenuTabGroups)
                .HasForeignKey(d => d.PosMenuGroupId);
            this.HasRequired(t => t.SubscriberViewPosMenuTab)
                .WithMany(t => t.SubscriberViewPosMenuTabGroups)
                .HasForeignKey(d => d.SubscriberViewPosMenuTabId);

        }
    }
}
