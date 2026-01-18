using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DepartmentId");

        builder.Property(e => e.Code)
            .HasColumnName("Code")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.NameAr)
            .HasColumnName("NameAr")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.NameEn)
            .HasColumnName("NameEn")
            .HasMaxLength(100);

        builder.Property(e => e.IsActive)
            .HasColumnName("IsActive")
            .HasDefaultValue(true);

        // Audit fields
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
        builder.Property(e => e.DeletedBy).HasColumnName("DeletedBy");

        // Indexes
        builder.HasIndex(e => e.Code).IsUnique();
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
