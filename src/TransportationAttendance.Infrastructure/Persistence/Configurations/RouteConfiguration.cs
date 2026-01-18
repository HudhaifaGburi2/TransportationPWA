using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class RouteConfiguration : IEntityTypeConfiguration<Route>
{
    public void Configure(EntityTypeBuilder<Route> builder)
    {
        builder.ToTable("Routes");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(r => r.Code)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(r => r.District)
            .HasMaxLength(100);

        builder.Property(r => r.MeetingPoint)
            .HasMaxLength(500);

        builder.Property(r => r.EntranceAssignment)
            .HasMaxLength(50);

        builder.Property(r => r.MeetingPointLatitude)
            .HasPrecision(18, 6);

        builder.Property(r => r.MeetingPointLongitude)
            .HasPrecision(18, 6);

        builder.HasIndex(r => r.Code)
            .IsUnique();

        builder.HasIndex(r => r.Name);
    }
}
