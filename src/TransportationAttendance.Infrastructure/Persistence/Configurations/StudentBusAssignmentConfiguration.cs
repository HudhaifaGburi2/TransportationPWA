using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class StudentBusAssignmentConfiguration : IEntityTypeConfiguration<StudentBusAssignment>
{
    public void Configure(EntityTypeBuilder<StudentBusAssignment> builder)
    {
        builder.ToTable("StudentBusAssignments");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("StudentBusAssignmentId");

        builder.Property(e => e.StudentId).HasColumnName("StudentId").IsRequired();
        builder.Property(e => e.BusId).HasColumnName("BusId").IsRequired();
        builder.Property(e => e.TransportType).HasColumnName("TransportType").IsRequired();
        builder.Property(e => e.ArrivalBusId).HasColumnName("ArrivalBusId");
        builder.Property(e => e.ReturnBusId).HasColumnName("ReturnBusId");
        builder.Property(e => e.IsActive).HasColumnName("IsActive");
        builder.Property(e => e.AssignedAt).HasColumnName("AssignedAt");
        builder.Property(e => e.AssignedBy).HasColumnName("AssignedBy");

        // Audit fields
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
        builder.Property(e => e.DeletedBy).HasColumnName("DeletedBy");

        // Relationships - make Bus navigation optional to avoid query filter warning
        builder.HasOne(e => e.Student)
            .WithMany()
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Bus)
            .WithMany()
            .HasForeignKey(e => e.BusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ArrivalBus)
            .WithMany()
            .HasForeignKey(e => e.ArrivalBusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.ReturnBus)
            .WithMany()
            .HasForeignKey(e => e.ReturnBusId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.StudentId);
        builder.HasIndex(e => e.BusId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => e.IsDeleted);
        builder.HasIndex(e => new { e.StudentId, e.BusId }).IsUnique().HasFilter("[IsDeleted] = 0");

        // Query filter for soft delete - MUST match Bus entity's query filter
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
