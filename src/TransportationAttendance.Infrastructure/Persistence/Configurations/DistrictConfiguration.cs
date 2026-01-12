using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class DistrictConfiguration : IEntityTypeConfiguration<District>
{
    public void Configure(EntityTypeBuilder<District> builder)
    {
        builder.ToTable("Districts");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("DistrictId");

        builder.Property(e => e.DistrictNameAr).HasColumnName("DistrictNameAr").HasMaxLength(200).IsRequired();
        builder.Property(e => e.DistrictNameEn).HasColumnName("DistrictNameEn").HasMaxLength(200);
        builder.Property(e => e.IsActive).HasColumnName("IsActive");

        // Audit fields
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
        builder.Property(e => e.DeletedBy).HasColumnName("DeletedBy");

        // Indexes
        builder.HasIndex(e => e.DistrictNameAr);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
