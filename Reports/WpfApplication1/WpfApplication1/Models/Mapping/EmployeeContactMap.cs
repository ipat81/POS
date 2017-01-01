using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace WpfApplication1.Models.Mapping
{
    public class EmployeeContactMap : EntityTypeConfiguration<EmployeeContact>
    {
        public EmployeeContactMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Address1)
                .HasMaxLength(20);

            this.Property(t => t.Address2)
                .HasMaxLength(20);

            this.Property(t => t.Address3)
                .HasMaxLength(20);

            this.Property(t => t.City)
                .HasMaxLength(100);

            this.Property(t => t.State)
                .IsFixedLength()
                .HasMaxLength(2);

            this.Property(t => t.PostalCode)
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.Phone1)
                .HasMaxLength(10);

            this.Property(t => t.Phone2)
                .HasMaxLength(10);

            this.Property(t => t.EMail1)
                .HasMaxLength(50);

            this.Property(t => t.EMail2)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("EmployeeContact");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EmployeeId).HasColumnName("EmployeeId");
            this.Property(t => t.ContactTypeEnum).HasColumnName("ContactTypeEnum");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Address1).HasColumnName("Address1");
            this.Property(t => t.Address2).HasColumnName("Address2");
            this.Property(t => t.Address3).HasColumnName("Address3");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.State).HasColumnName("State");
            this.Property(t => t.PostalCode).HasColumnName("PostalCode");
            this.Property(t => t.Phone1).HasColumnName("Phone1");
            this.Property(t => t.Phone2).HasColumnName("Phone2");
            this.Property(t => t.EMail1).HasColumnName("EMail1");
            this.Property(t => t.EMail2).HasColumnName("EMail2");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AuditId).HasColumnName("AuditId");

            // Relationships
            this.HasOptional(t => t.Audit)
                .WithMany(t => t.EmployeeContacts)
                .HasForeignKey(d => d.AuditId);
            this.HasRequired(t => t.Employee)
                .WithMany(t => t.EmployeeContacts)
                .HasForeignKey(d => d.EmployeeId);

        }
    }
}
