using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Entities;

public class StudentBusAssignment : BaseEntity
{
    public Guid StudentId { get; private set; }
    public Guid BusId { get; private set; }
    public TransportType TransportType { get; private set; }
    public Guid? ArrivalBusId { get; private set; }
    public Guid? ReturnBusId { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public Guid? AssignedBy { get; private set; }

    // Navigation
    public Student? Student { get; private set; }
    public Bus? Bus { get; private set; }
    public Bus? ArrivalBus { get; private set; }
    public Bus? ReturnBus { get; private set; }

    private StudentBusAssignment() { }

    public static StudentBusAssignment Create(
        Guid studentId,
        Guid busId,
        TransportType transportType = TransportType.Both,
        Guid? arrivalBusId = null,
        Guid? returnBusId = null,
        Guid? assignedBy = null)
    {
        return new StudentBusAssignment
        {
            StudentId = studentId,
            BusId = busId,
            TransportType = transportType,
            ArrivalBusId = arrivalBusId,
            ReturnBusId = returnBusId,
            IsActive = true,
            AssignedAt = DateTime.UtcNow,
            AssignedBy = assignedBy
        };
    }

    public void UpdateAssignment(
        Guid busId,
        TransportType transportType,
        Guid? arrivalBusId,
        Guid? returnBusId)
    {
        BusId = busId;
        TransportType = transportType;
        ArrivalBusId = arrivalBusId;
        ReturnBusId = returnBusId;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
