using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class EmployeePayInfoMap : EntityTypeConfiguration<EmployeePayInfo>
    {
        public EmployeePayInfoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.EmployeeId, t.RoleId });

            // Properties
            this.Property(t => t.EmployeeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RoleId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.EmployeeSSN)
                .IsFixedLength()
                .HasMaxLength(9);

            this.Property(t => t.PayState)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("EmployeePayInfo");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.RoleId).HasColumnName("RoleId");
            this.Property(t => t.EmployeeSSN).HasColumnName("EmployeeSSN");
            this.Property(t => t.PayMethodEnum).HasColumnName("PayMethodEnum");
            this.Property(t => t.PayRateTypeEnum).HasColumnName("PayRateTypeEnum");
            this.Property(t => t.PayRateQB1).HasColumnName("PayRateQB1");
            this.Property(t => t.PayRateQB2).HasColumnName("PayRateQB2");
            this.Property(t => t.FactorQB1).HasColumnName("FactorQB1");
            this.Property(t => t.FactorQB2).HasColumnName("FactorQB2");
            this.Property(t => t.PayState).HasColumnName("PayState");
            this.Property(t => t.FedExemptions).HasColumnName("FedExemptions");
            this.Property(t => t.StateExemptions).HasColumnName("StateExemptions");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.EmployeePayInfoes)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.EmployeePayInfoes)
                .HasForeignKey(d => d.EmployeeId);
            this.HasRequired(t => t.Role)
                .WithMany(t => t.EmployeePayInfoes)
                .HasForeignKey(d => d.RoleId);

        }
    }
}
