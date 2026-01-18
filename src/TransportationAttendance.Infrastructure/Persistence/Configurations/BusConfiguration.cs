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
        builder.Property(b => b.Id).HasColumnName("BusId");

        builder.Property(b => b.BusNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(b => b.DriverName)
            .HasMaxLength(200);

        builder.Property(b => b.DriverPhoneNumber)
            .HasMaxLength(20);

        builder.HasIndex(b => b.BusNumber);
    }
}
