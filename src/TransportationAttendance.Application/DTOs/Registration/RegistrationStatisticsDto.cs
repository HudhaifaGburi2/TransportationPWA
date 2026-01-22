namespace TransportationAttendance.Application.DTOs.Registration;

public record RegistrationStatisticsDto
{
    public int TotalRequests { get; init; }
    public int PendingCount { get; init; }
    public int ApprovedCount { get; init; }
    public int RejectedCount { get; init; }
    public int TodayCount { get; init; }
    public int ThisWeekCount { get; init; }
    public IReadOnlyList<DistrictStatDto> ByDistrict { get; init; } = new List<DistrictStatDto>();
}

public record DistrictStatDto
{
    public Guid DistrictId { get; init; }
    public string DistrictName { get; init; } = string.Empty;
    public int Count { get; init; }
}

public record BusSuggestionDto
{
    public Guid BusId { get; init; }
    public string BusNumber { get; init; } = string.Empty;
    public string? RouteName { get; init; }
    public int Capacity { get; init; }
    public int CurrentStudentCount { get; init; }
    public int AvailableSeats => Capacity - CurrentStudentCount;
}
