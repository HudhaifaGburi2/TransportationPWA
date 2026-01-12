using TransportationAttendance.Domain.Enums;

namespace TransportationAttendance.Domain.Entities;

public class AttendanceSession : BaseEntity
{
    public Guid SupervisorId { get; private set; }
    public Guid BusId { get; private set; }
    public int PeriodId { get; private set; }
    public Guid? LocationId { get; private set; }
    public DateOnly AttendanceDate { get; private set; }
    public AttendanceType AttendanceType { get; private set; }
    public int UnregisteredStudentsCount { get; private set; }
    public SyncStatus SyncStatus { get; private set; }
    public bool CreatedOffline { get; private set; }
    public DateTime? SyncedAt { get; private set; }

    // Navigation
    public Bus? Bus { get; private set; }
    public Location? Location { get; private set; }
    public ICollection<AttendanceRecord> AttendanceRecords { get; private set; } = new List<AttendanceRecord>();

    private AttendanceSession() { }

    public static AttendanceSession Create(
        Guid supervisorId,
        Guid busId,
        int periodId,
        DateOnly attendanceDate,
        AttendanceType attendanceType,
        Guid? locationId = null,
        int unregisteredStudentsCount = 0,
        bool createdOffline = false)
    {
        return new AttendanceSession
        {
            SupervisorId = supervisorId,
            BusId = busId,
            PeriodId = periodId,
            AttendanceDate = attendanceDate,
            AttendanceType = attendanceType,
            LocationId = locationId,
            UnregisteredStudentsCount = unregisteredStudentsCount,
            SyncStatus = createdOffline ? SyncStatus.Pending : SyncStatus.Synced,
            CreatedOffline = createdOffline,
            SyncedAt = createdOffline ? null : DateTime.UtcNow
        };
    }

    public void SetUnregisteredCount(int count) => UnregisteredStudentsCount = count;

    public void MarkAsSynced()
    {
        SyncStatus = SyncStatus.Synced;
        SyncedAt = DateTime.UtcNow;
    }

    public void MarkAsSyncing() => SyncStatus = SyncStatus.Syncing;
    public void MarkAsFailed() => SyncStatus = SyncStatus.Failed;

    public void AddAttendanceRecord(AttendanceRecord record)
    {
        AttendanceRecords.Add(record);
    }
}
