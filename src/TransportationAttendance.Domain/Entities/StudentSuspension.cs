namespace TransportationAttendance.Domain.Entities;

public class StudentSuspension : BaseEntity
{
    public Guid StudentId { get; private set; }
    public Guid? BusId { get; private set; }
    public string Reason { get; private set; } = "غياب لمدة ثلاثة أيام متتالية";
    public DateTime SuspendedAt { get; private set; }
    public Guid SuspendedBy { get; private set; }
    public bool IsReactivated { get; private set; }
    public DateTime? ReactivatedAt { get; private set; }
    public Guid? ReactivatedBy { get; private set; }
    public Guid? NewBusIdAfterReactivation { get; private set; }
    public string? ReactivationNotes { get; private set; }

    // Navigation
    public Student? Student { get; private set; }
    public Bus? Bus { get; private set; }
    public Bus? NewBusAfterReactivation { get; private set; }

    private StudentSuspension() { }

    public static StudentSuspension Create(
        Guid studentId,
        Guid suspendedBy,
        string? reason = null,
        Guid? busId = null)
    {
        return new StudentSuspension
        {
            StudentId = studentId,
            BusId = busId,
            Reason = reason ?? "غياب لمدة ثلاثة أيام متتالية",
            SuspendedAt = DateTime.UtcNow,
            SuspendedBy = suspendedBy,
            IsReactivated = false
        };
    }

    public void Reactivate(Guid reactivatedBy, Guid? newBusId = null, string? notes = null)
    {
        IsReactivated = true;
        ReactivatedAt = DateTime.UtcNow;
        ReactivatedBy = reactivatedBy;
        NewBusIdAfterReactivation = newBusId;
        ReactivationNotes = notes;
    }
}
