using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class StudentLeaveConfiguration : IEntityTypeConfiguration<StudentLeave>
{
    public void Configure(EntityTypeBuilder<StudentLeave> builder)
    {
        builder.ToTable("StudentLeaves");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.AttachmentUrl)
            .HasMaxLength(500);

        builder.Property(x => x.AttachmentFileName)
            .HasMaxLength(255);

        builder.Property(x => x.CancellationReason)
            .HasMaxLength(500);

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => new { x.StartDate, x.EndDate });
        builder.HasIndex(x => x.IsApproved);
        builder.HasIndex(x => x.IsCancelled);
    }
}
