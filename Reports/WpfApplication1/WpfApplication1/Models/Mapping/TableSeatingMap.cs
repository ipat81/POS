using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class TableSeatingMap : EntityTypeConfiguration<TableSeating>
    {
        public TableSeatingMap()
        {
            // Primary Key
            this.HasKey(t => new { t.SEId, t.DTableId });

            // Properties
            this.Property(t => t.SEId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DTableId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TableSeating");
            this.Property(t => t.SEId).HasColumnName("SEId");
            this.Property(t => t.DTableId).HasColumnName("DTableId");
            this.Property(t => t.TableSeatingSequence).HasColumnName("TableSeatingSequence");
            this.Property(t => t.TableSeatingTypeEnum).HasColumnName("TableSeatingTypeEnum");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.TableSeatings)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.DTable)
                .WithMany(t => t.TableSeatings)
                .HasForeignKey(d => d.DTableId);
            this.HasRequired(t => t.SE)
                .WithMany(t => t.TableSeatings)
                .HasForeignKey(d => d.SEId);

        }
    }
}
