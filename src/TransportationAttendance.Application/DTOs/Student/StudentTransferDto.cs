namespace TransportationAttendance.Application.DTOs.Student;

public record StudentTransferDto
{
    public Guid Id { get; init; }
    public Guid StudentId { get; init; }
    public string? StudentName { get; init; }
    public Guid FromBusId { get; init; }
    public string? FromBusNumber { get; init; }
    public Guid ToBusId { get; init; }
    public string? ToBusNumber { get; init; }
    public string? Reason { get; init; }
    public DateTime TransferredAt { get; init; }
    public DateTime EffectiveDate { get; init; }
}

public record CreateTransferDto
{
    public Guid StudentId { get; init; }
    public Guid ToBusId { get; init; }
    public string? Reason { get; init; }
    public DateTime? EffectiveDate { get; init; }
}
