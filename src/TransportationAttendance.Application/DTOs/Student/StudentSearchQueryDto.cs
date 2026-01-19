namespace TransportationAttendance.Application.DTOs.Student;

public record StudentSearchQueryDto
{
    public string? SearchTerm { get; init; }
    public string? Status { get; init; }
    public Guid? DistrictId { get; init; }
    public Guid? BusId { get; init; }
    public int? PeriodId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 50;
}

public record PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = new List<T>();
    public int TotalCount { get; init; }
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
