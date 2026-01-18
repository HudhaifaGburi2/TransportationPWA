using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class ActualRouteConfiguration : IEntityTypeConfiguration<ActualRoute>
{
    public void Configure(EntityTypeBuilder<ActualRoute> builder)
    {
        builder.ToTable("Routes");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id");

        builder.Property(e => e.Name).HasColumnName("Name").HasMaxLength(200).IsRequired();
        builder.Property(e => e.Code).HasColumnName("Code").HasMaxLength(20).IsRequired();
        builder.Property(e => e.District).HasColumnName("District").HasMaxLength(200).IsRequired();
        builder.Property(e => e.MeetingPoint).HasColumnName("MeetingPoint").HasMaxLength(500).IsRequired();
        builder.Property(e => e.MeetingPointLatitude).HasColumnName("MeetingPointLatitude").HasPrecision(18, 10);
        builder.Property(e => e.MeetingPointLongitude).HasColumnName("MeetingPointLongitude").HasPrecision(18, 10);
        builder.Property(e => e.PickupTime).HasColumnName("PickupTime");
        builder.Property(e => e.DropoffTime).HasColumnName("DropoffTime");
        builder.Property(e => e.Capacity).HasColumnName("Capacity");
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.FloorAssignment).HasColumnName("FloorAssignment");
        builder.Property(e => e.EntranceAssignment).HasColumnName("EntranceAssignment").HasMaxLength(100);
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.DistrictId).HasColumnName("DistrictId");

        builder.HasIndex(e => e.Code).IsUnique();
        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Status);
    }
}
