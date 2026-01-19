namespace TransportationAttendance.Domain.Entities;

public class StudentTransfer : BaseEntity
{
    public Guid StudentId { get; private set; }
    public Guid FromBusId { get; private set; }
    public Guid ToBusId { get; private set; }
    public string? Reason { get; private set; }
    public DateTime TransferredAt { get; private set; }
    public Guid TransferredBy { get; private set; }
    public DateTime EffectiveDate { get; private set; }

    // Navigation
    public Student? Student { get; private set; }
    public Bus? FromBus { get; private set; }
    public Bus? ToBus { get; private set; }

    private StudentTransfer() { }

    public static StudentTransfer Create(
        Guid studentId,
        Guid fromBusId,
        Guid toBusId,
        Guid transferredBy,
        string? reason = null,
        DateTime? effectiveDate = null)
    {
        return new StudentTransfer
        {
            StudentId = studentId,
            FromBusId = fromBusId,
            ToBusId = toBusId,
            Reason = reason,
            TransferredAt = DateTime.UtcNow,
            TransferredBy = transferredBy,
            EffectiveDate = effectiveDate ?? DateTime.UtcNow
        };
    }
}
