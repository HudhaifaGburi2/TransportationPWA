using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class BusConfiguration : IEntityTypeConfiguration<Bus>
{
    public void Configure(EntityTypeBuilder<Bus> builder)
    {
        builder.ToTable("Buses");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BusNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.LicensePlate)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.Model)
            .HasMaxLength(100);

        builder.HasIndex(b => b.LicensePlate)
            .IsUnique();

        builder.HasIndex(b => b.BusNumber);
    }
}
