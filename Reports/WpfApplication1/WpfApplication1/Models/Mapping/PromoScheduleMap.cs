using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class PromoScheduleMap : EntityTypeConfiguration<PromoSchedule>
    {
        public PromoScheduleMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ExcludeTimeFrom)
                .HasMaxLength(4);

            this.Property(t => t.ExcludeTimeUntil)
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("PromoSchedule");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PromoItemId).HasColumnName("PromoItemId");
            this.Property(t => t.ValidDateFrom).HasColumnName("ValidDateFrom");
            this.Property(t => t.ValidDateUntil).HasColumnName("ValidDateUntil");
            this.Property(t => t.ExcludeTimeFrom).HasColumnName("ExcludeTimeFrom");
            this.Property(t => t.ExcludeTimeUntil).HasColumnName("ExcludeTimeUntil");
            this.Property(t => t.ValidSETypeEnum).HasColumnName("ValidSETypeEnum");
            this.Property(t => t.ValidWithOtherPromo).HasColumnName("ValidWithOtherPromo");
            this.Property(t => t.ValidWeekDayLunch).HasColumnName("ValidWeekDayLunch");
            this.Property(t => t.ValidWeekDayDinner).HasColumnName("ValidWeekDayDinner");
            this.Property(t => t.ValidWeekEndLunch).HasColumnName("ValidWeekEndLunch");
            this.Property(t => t.ValidWeekEndDinner).HasColumnName("ValidWeekEndDinner");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.PromoSchedules)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.PromoItem)
                .WithMany(t => t.PromoSchedules)
                .HasForeignKey(d => d.PromoItemId);

        }
    }
}
