using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable("Locations");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("LocationId");

        builder.Property(e => e.LocationCode).HasColumnName("LocationCode").HasMaxLength(20).IsRequired();
        builder.Property(e => e.LocationName).HasColumnName("LocationName").HasMaxLength(100).IsRequired();
        builder.Property(e => e.LocationType).HasColumnName("LocationType").HasMaxLength(50);
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
        builder.HasIndex(e => e.LocationCode).IsUnique();
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
