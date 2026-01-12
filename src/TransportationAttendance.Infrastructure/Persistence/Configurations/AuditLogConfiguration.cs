using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransportationAttendance.Domain.Entities;

namespace TransportationAttendance.Infrastructure.Persistence.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("AuditId");

        builder.Property(e => e.UserId).HasColumnName("UserId");
        builder.Property(e => e.Action).HasColumnName("Action").HasMaxLength(100).IsRequired();
        builder.Property(e => e.EntityType).HasColumnName("EntityType").HasMaxLength(100);
        builder.Property(e => e.EntityId).HasColumnName("EntityId");
        builder.Property(e => e.OldValues).HasColumnName("OldValues");
        builder.Property(e => e.NewValues).HasColumnName("NewValues");
        builder.Property(e => e.IpAddress).HasColumnName("IpAddress").HasMaxLength(50);
        builder.Property(e => e.CreatedAt).HasColumnName("CreatedAt");

        // Indexes
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.EntityType);
        builder.HasIndex(e => e.CreatedAt);
    }
}
