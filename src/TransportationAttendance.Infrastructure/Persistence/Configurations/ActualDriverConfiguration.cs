using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class ActualDriverConfiguration : IEntityTypeConfiguration<ActualDriver>
{
    public void Configure(EntityTypeBuilder<ActualDriver> builder)
    {
        builder.ToTable("Drivers");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id");

        builder.Property(e => e.FullName).HasColumnName("FullName").HasMaxLength(200).IsRequired();
        builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(20).IsRequired();
        builder.Property(e => e.LicenseNumber).HasColumnName("LicenseNumber").HasMaxLength(50).IsRequired();
        builder.Property(e => e.LicenseExpiryDate).HasColumnName("LicenseExpiryDate").IsRequired();
        builder.Property(e => e.EmployeeId).HasColumnName("EmployeeId").HasMaxLength(50);
        builder.Property(e => e.IsActive).HasColumnName("IsActive");
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

        builder.HasIndex(e => e.PhoneNumber);
        builder.HasIndex(e => e.LicenseNumber).IsUnique();
        builder.HasIndex(e => e.IsActive);
    }
}
