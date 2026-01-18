using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class ActualBusConfiguration : IEntityTypeConfiguration<ActualBus>
{
    public void Configure(EntityTypeBuilder<ActualBus> builder)
    {
        builder.ToTable("Buses");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("Id");

        builder.Property(e => e.BusNumber).HasColumnName("BusNumber").HasMaxLength(20).IsRequired();
        builder.Property(e => e.LicensePlate).HasColumnName("LicensePlate").HasMaxLength(20).IsRequired();
        builder.Property(e => e.Capacity).HasColumnName("Capacity");
        builder.Property(e => e.Model).HasColumnName("Model").HasMaxLength(100);
        builder.Property(e => e.Year).HasColumnName("Year");
        builder.Property(e => e.Status).HasColumnName("Status");
        builder.Property(e => e.LastMaintenanceDate).HasColumnName("LastMaintenanceDate");
        builder.Property(e => e.NextMaintenanceDate).HasColumnName("NextMaintenanceDate");
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");

        builder.HasIndex(e => e.BusNumber);
        builder.HasIndex(e => e.LicensePlate).IsUnique();
        builder.HasIndex(e => e.Status);
    }
}
