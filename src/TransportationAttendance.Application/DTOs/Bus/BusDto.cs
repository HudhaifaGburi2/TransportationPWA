namespace TransportationAttendance.Application.DTOs.Bus;

public record BusDto
{
    public Guid BusId { get; init; }
    public string BusNumber { get; init; } = string.Empty;
    public int PeriodId { get; init; }
    public string? PeriodName { get; init; }
    public Guid? RouteId { get; init; }
    public string? RouteName { get; init; }
    public string? DriverName { get; init; }
    public string? DriverPhoneNumber { get; init; }
    public int Capacity { get; init; }
    public int CurrentStudentCount { get; init; }
    public decimal UtilizationPercentage { get; init; }
    public bool IsActive { get; init; }
    public bool IsMerged { get; init; }
    public Guid? MergedWithBusId { get; init; }
    public List<DistrictInfoDto> Districts { get; init; } = new();
}

public record DistrictInfoDto
{
    public Guid DistrictId { get; init; }
    public string DistrictNameAr { get; init; } = string.Empty;
    public string? DistrictNameEn { get; init; }
}

public record BusStatisticsDto
{
    public Guid BusId { get; init; }
    public string BusNumber { get; init; } = string.Empty;
    public int TotalStudents { get; init; }
    public int ActiveStudents { get; init; }
    public int SuspendedStudents { get; init; }
    public int Capacity { get; init; }
    public decimal UtilizationRate { get; init; }
    public List<DistrictStudentCount> DistrictBreakdown { get; init; } = new();
}

public record DistrictStudentCount
{
    public Guid DistrictId { get; init; }
    public string DistrictName { get; init; } = string.Empty;
    public int StudentCount { get; init; }
}

public record BusSummaryDto
{
    public int TotalBuses { get; init; }
    public int ActiveBuses { get; init; }
    public int InactiveBuses { get; init; }
    public int TotalCapacity { get; init; }
    public int TotalStudentsAssigned { get; init; }
    public decimal OverallUtilization { get; init; }
    public List<PeriodBusSummary> ByPeriod { get; init; } = new();
}

public record PeriodBusSummary
{
    public int PeriodId { get; init; }
    public string PeriodName { get; init; } = string.Empty;
    public int BusCount { get; init; }
    public int StudentCount { get; init; }
    public int TotalCapacity { get; init; }
    public decimal Utilization { get; init; }
}
