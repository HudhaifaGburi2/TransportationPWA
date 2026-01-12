namespace TransportationAttendance.Domain.Interfaces;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    Guid? CreatedBy { get; }
    DateTime? UpdatedAt { get; }
    Guid? UpdatedBy { get; }
    
    void SetCreated(DateTime createdAt, Guid? createdBy);
    void SetUpdated(DateTime updatedAt, Guid? updatedBy);
}
