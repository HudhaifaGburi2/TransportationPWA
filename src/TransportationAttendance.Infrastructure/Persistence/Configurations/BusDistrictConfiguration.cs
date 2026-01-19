using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class BusDistrictConfiguration : IEntityTypeConfiguration<BusDistrict>
{
    public void Configure(EntityTypeBuilder<BusDistrict> builder)
    {
        builder.ToTable("BusDistrictMappings");

        builder.HasKey(bd => bd.Id);
        builder.Property(bd => bd.Id).HasColumnName("MappingId");

        builder.Property(bd => bd.BusId)
            .IsRequired();

        builder.Property(bd => bd.DistrictId)
            .IsRequired();

        // Map only columns that exist in database
        builder.Property(bd => bd.CreatedAt).HasColumnName("CreatedAt");
        builder.Property(bd => bd.CreatedBy).HasColumnName("CreatedBy");
        
        // Ignore columns that don't exist in the database
        builder.Ignore(bd => bd.UpdatedAt);
        builder.Ignore(bd => bd.UpdatedBy);
        builder.Ignore(bd => bd.IsDeleted);
        builder.Ignore(bd => bd.DeletedAt);
        builder.Ignore(bd => bd.DeletedBy);

        builder.HasOne(bd => bd.Bus)
            .WithMany()
            .HasForeignKey(bd => bd.BusId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(bd => bd.District)
            .WithMany()
            .HasForeignKey(bd => bd.DistrictId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(bd => new { bd.BusId, bd.DistrictId })
            .IsUnique();
    }
}
