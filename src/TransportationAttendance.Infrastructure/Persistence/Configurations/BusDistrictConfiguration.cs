using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class BusDistrictConfiguration : IEntityTypeConfiguration<BusDistrict>
{
    public void Configure(EntityTypeBuilder<BusDistrict> builder)
    {
        builder.ToTable("BusDistricts");

        builder.HasKey(bd => bd.Id);

        builder.Property(bd => bd.BusId)
            .IsRequired();

        builder.Property(bd => bd.DistrictId)
            .IsRequired();

        builder.HasOne(bd => bd.Bus)
            .WithMany()
            .HasForeignKey(bd => bd.BusId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bd => bd.District)
            .WithMany()
            .HasForeignKey(bd => bd.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(bd => new { bd.BusId, bd.DistrictId })
            .IsUnique();
    }
}
