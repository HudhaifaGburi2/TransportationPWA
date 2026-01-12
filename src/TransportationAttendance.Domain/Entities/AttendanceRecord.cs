using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Entities;

public class AttendanceRecord : BaseEntity
{
    public Guid SessionId { get; private set; }
    public Guid StudentId { get; private set; }
    public AttendanceStatus AttendanceStatus { get; private set; }
    public string? Notes { get; private set; }

    // Navigation
    public AttendanceSession? Session { get; private set; }
    public Student? Student { get; private set; }

    private AttendanceRecord() { }

    public static AttendanceRecord Create(
        Guid sessionId,
        Guid studentId,
        AttendanceStatus status = AttendanceStatus.Present,
        string? notes = null)
    {
        return new AttendanceRecord
        {
            SessionId = sessionId,
            StudentId = studentId,
            AttendanceStatus = status,
            Notes = notes
        };
    }

    public void UpdateStatus(AttendanceStatus status, string? notes = null)
    {
        AttendanceStatus = status;
        Notes = notes;
    }
}
