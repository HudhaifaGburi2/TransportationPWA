using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.ToTable("Students");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("StudentId");

        // Central DB Read-Only Fields
        builder.Property(e => e.StudentUserId).HasColumnName("StudentUserId").IsRequired();
        builder.Property(e => e.StudentId).HasColumnName("ExternalStudentId").HasMaxLength(50).IsRequired();
        builder.Property(e => e.StudentName).HasColumnName("FullNameAr").HasMaxLength(200).IsRequired();
        builder.Property(e => e.HalaqaTypeCode).HasColumnName("HalaqaTypeCode").HasMaxLength(50);
        builder.Property(e => e.HalaqaSectionId).HasColumnName("HalaqaSectionId");
        builder.Property(e => e.HalaqaGender).HasColumnName("HalaqaGender").HasMaxLength(20);
        builder.Property(e => e.PeriodId).HasColumnName("PeriodId");
        builder.Property(e => e.AgeGroupId).HasColumnName("AgeGroupId");
        builder.Property(e => e.HalaqaLocationId).HasColumnName("HalaqaLocationId");
        builder.Property(e => e.TeacherName).HasColumnName("TeacherName").HasMaxLength(200);

        // Transportation-specific fields
        builder.Property(e => e.DistrictId).HasColumnName("DistrictId").IsRequired();
        builder.Property(e => e.HomeAddress).HasColumnName("HomeAddress").HasMaxLength(500);
        builder.Property(e => e.Latitude).HasColumnName("Latitude").HasPrecision(10, 8);
        builder.Property(e => e.Longitude).HasColumnName("Longitude").HasPrecision(11, 8);
        builder.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").HasMaxLength(20);

        // Status
        builder.Property(e => e.Status).HasColumnName("Status").HasConversion<string>().HasMaxLength(50);

        // Audit fields
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(e => e.CreatedBy).HasColumnName("CreatedBy");
        builder.Property(e => e.UpdatedAt).HasColumnName("UpdatedAt");
        builder.Property(e => e.UpdatedBy).HasColumnName("UpdatedBy");
        builder.Property(e => e.IsDeleted).HasColumnName("IsDeleted");
        builder.Property(e => e.DeletedAt).HasColumnName("DeletedAt");
        builder.Property(e => e.DeletedBy).HasColumnName("DeletedBy");

        // Relationships
        builder.HasOne(e => e.District)
            .WithMany()
            .HasForeignKey(e => e.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.StudentUserId).IsUnique();
        builder.HasIndex(e => e.StudentId).IsUnique();
        builder.HasIndex(e => e.DistrictId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.IsDeleted);

        // Query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
