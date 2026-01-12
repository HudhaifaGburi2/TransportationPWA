namespace TransportationAttendance.Domain.Interfaces;

public interface ISoftDelete
{
    bool IsDeleted { get; }
    DateTime? DeletedAt { get; }
    Guid? DeletedBy { get; }
    
    void SoftDelete(DateTime deletedAt, Guid? deletedBy);
    void Restore();
}
