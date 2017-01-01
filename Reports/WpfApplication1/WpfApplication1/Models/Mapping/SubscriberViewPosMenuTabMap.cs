using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class SubscriberViewPosMenuTabMap : EntityTypeConfiguration<SubscriberViewPosMenuTab>
    {
        public SubscriberViewPosMenuTabMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SubscriberViewPosMenuTab");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SubscriberViewId).HasColumnName("SubscriberViewId");
            this.Property(t => t.PosMenuTabId).HasColumnName("PosMenuTabId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.SubscriberViewPosMenuTabs)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.PosMenuTab)
                .WithMany(t => t.SubscriberViewPosMenuTabs)
                .HasForeignKey(d => d.PosMenuTabId);
            this.HasRequired(t => t.SubscriberView)
                .WithMany(t => t.SubscriberViewPosMenuTabs)
                .HasForeignKey(d => d.SubscriberViewId);

        }
    }
}
