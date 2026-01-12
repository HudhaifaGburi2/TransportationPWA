namespace TransportationAttendance.Application.DTOs.District;

public record DistrictDto
{
    public Guid Id { get; init; }
    public string DistrictNameAr { get; init; } = string.Empty;
    public string? DistrictNameEn { get; init; }
    public bool IsActive { get; init; }
}
