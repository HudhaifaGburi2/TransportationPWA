using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class StudentTransferConfiguration : IEntityTypeConfiguration<StudentTransfer>
{
    public void Configure(EntityTypeBuilder<StudentTransfer> builder)
    {
        builder.ToTable("StudentTransfers");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Reason)
            .HasMaxLength(500);

        builder.HasOne(x => x.Student)
            .WithMany()
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.FromBus)
            .WithMany()
            .HasForeignKey(x => x.FromBusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ToBus)
            .WithMany()
            .HasForeignKey(x => x.ToBusId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => x.StudentId);
        builder.HasIndex(x => x.FromBusId);
        builder.HasIndex(x => x.ToBusId);
        builder.HasIndex(x => x.TransferredAt);
    }
}
