using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class DriverConfiguration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.ToTable("Drivers");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DriverId");

        builder.Property(e => e.FullName)
            .HasColumnName("FullName")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(e => e.PhoneNumber)
            .HasColumnName("PhoneNumber")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(e => e.DepartmentId)
            .HasColumnName("DepartmentId")
            .IsRequired();

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

        // Relationships
        builder.HasOne(e => e.Department)
            .WithMany(d => d.Drivers)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.PhoneNumber);
        builder.HasIndex(e => e.DepartmentId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
