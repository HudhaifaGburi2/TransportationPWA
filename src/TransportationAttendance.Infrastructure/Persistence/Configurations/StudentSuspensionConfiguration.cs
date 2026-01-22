using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class StudentSuspensionConfiguration : IEntityTypeConfiguration<StudentSuspension>
{
    public void Configure(EntityTypeBuilder<StudentSuspension> builder)
    {
        builder.ToTable("StudentSuspensions");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.ReactivationNotes)
            .HasMaxLength(500);

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Bus)
            .WithMany()
            .HasForeignKey(x => x.BusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.NewBusAfterReactivation)
            .WithMany()
            .HasForeignKey(x => x.NewBusIdAfterReactivation)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.IsReactivated);
        builder.HasIndex(x => x.SuspendedAt);
    }
}
