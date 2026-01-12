using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class AttendanceSessionConfiguration : IEntityTypeConfiguration<AttendanceSession>
{
    public void Configure(EntityTypeBuilder<AttendanceSession> builder)
    {
        builder.ToTable("AttendanceSessions");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("SessionId");

        builder.Property(e => e.SupervisorId).HasColumnName("SupervisorId").IsRequired();
        builder.Property(e => e.BusId).HasColumnName("BusId").IsRequired();
        builder.Property(e => e.PeriodId).HasColumnName("PeriodId").IsRequired();
        builder.Property(e => e.LocationId).HasColumnName("LocationId");
        builder.Property(e => e.AttendanceDate).HasColumnName("AttendanceDate").IsRequired();
        builder.Property(e => e.AttendanceType).HasColumnName("AttendanceType").HasConversion<string>().HasMaxLength(50).IsRequired();
        builder.Property(e => e.UnregisteredStudentsCount).HasColumnName("UnregisteredStudentsCount");
        builder.Property(e => e.SyncStatus).HasColumnName("SyncStatus").HasConversion<string>().HasMaxLength(50);
        builder.Property(e => e.CreatedOffline).HasColumnName("CreatedOffline");
        builder.Property(e => e.SyncedAt).HasColumnName("SyncedAt");

        // Audit fields
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
        builder.Property(e => e.DeletedBy).HasColumnName("DeletedBy");

        // Relationships
        builder.HasOne(e => e.Bus)
            .WithMany()
            .HasForeignKey(e => e.BusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.AttendanceRecords)
            .WithOne(r => r.Session)
            .HasForeignKey(r => r.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.AttendanceDate);
        builder.HasIndex(e => e.BusId);
        builder.HasIndex(e => e.SupervisorId);
        builder.HasIndex(e => e.SyncStatus);
        builder.HasIndex(e => new { e.BusId, e.AttendanceDate, e.AttendanceType }).IsUnique();
        builder.HasIndex(e => e.IsDeleted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
