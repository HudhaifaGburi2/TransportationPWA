using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class BusConfiguration : IEntityTypeConfiguration<Bus>
{
    public void Configure(EntityTypeBuilder<Bus> builder)
    {
        builder.ToTable("Buses");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("BusId");

        // PlateNumber is the authoritative identifier
        builder.Property(e => e.PlateNumber).HasColumnName("PlateNumber").HasMaxLength(20).IsRequired();
        builder.Property(e => e.PeriodId).HasColumnName("PeriodId").IsRequired();
        builder.Property(e => e.RouteId).HasColumnName("RouteId");
        builder.Property(e => e.DriverId).HasColumnName("DriverId");
        builder.Property(e => e.DepartmentId).HasColumnName("DepartmentId");
        builder.Property(e => e.BusNumber).HasColumnName("BusNumber").HasMaxLength(10);
        
        // Legacy fields - kept for backward compatibility
#pragma warning disable CS0618
        builder.Property(e => e.DriverName).HasColumnName("DriverName").HasMaxLength(200);
        builder.Property(e => e.DriverPhoneNumber).HasColumnName("DriverPhoneNumber").HasMaxLength(20);
#pragma warning restore CS0618

        builder.Property(e => e.Capacity).HasColumnName("Capacity");
        builder.Property(e => e.IsActive).HasColumnName("IsActive");
        builder.Property(e => e.IsMerged).HasColumnName("IsMerged");
        builder.Property(e => e.MergedWithBusId).HasColumnName("MergedWithBusId");

        // Audit fields
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
        builder.Property(e => e.DeletedBy).HasColumnName("DeletedBy");

        // Relationships
        builder.HasOne(e => e.Route)
            .WithMany()
            .HasForeignKey(e => e.RouteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Driver)
            .WithMany(d => d.Buses)
            .HasForeignKey(e => e.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Department)
            .WithMany(d => d.Buses)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.MergedWithBus)
            .WithMany()
            .HasForeignKey(e => e.MergedWithBusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.PeriodId);
        builder.HasIndex(e => e.PlateNumber);
        builder.HasIndex(e => e.DriverId);
        builder.HasIndex(e => e.DepartmentId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);
        
        // Unique constraint: PlateNumber + PeriodId
        builder.HasIndex(e => new { e.PlateNumber, e.PeriodId }).IsUnique();

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
