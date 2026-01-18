namespace TransportationAttendance.Domain.Entities;

/// <summary>
/// Route entity for transportation management.
/// </summary>
public class Route
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;
    public string District { get; private set; } = string.Empty;
    public string MeetingPoint { get; private set; } = string.Empty;
    public decimal? MeetingPointLatitude { get; private set; }
    public decimal? MeetingPointLongitude { get; private set; }
    public TimeSpan PickupTime { get; private set; }
    public TimeSpan DropoffTime { get; private set; }
    public int Capacity { get; private set; }
    public int Status { get; private set; }
    public int? FloorAssignment { get; private set; }
    public string? EntranceAssignment { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public Guid? DistrictId { get; private set; }

    private Route() 
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public static Route Create(
        string name,
        string code,
        string district,
        string meetingPoint,
        TimeSpan pickupTime,
        TimeSpan dropoffTime,
        int capacity = 30)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Route name is required.", nameof(name));

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Route code is required.", nameof(code));

        return new Route
        {
            Name = name.Trim(),
            Code = code.Trim(),
            District = district?.Trim() ?? string.Empty,
            MeetingPoint = meetingPoint?.Trim() ?? string.Empty,
            PickupTime = pickupTime,
            DropoffTime = dropoffTime,
            Capacity = capacity,
            Status = 1 // Active
        };
    }

    public void Update(
        string name,
        string code,
        string district,
        string meetingPoint,
        TimeSpan pickupTime,
        TimeSpan dropoffTime,
        int capacity)
    {
        Name = name.Trim();
        Code = code.Trim();
        District = district?.Trim() ?? string.Empty;
        MeetingPoint = meetingPoint?.Trim() ?? string.Empty;
        PickupTime = pickupTime;
        DropoffTime = dropoffTime;
        Capacity = capacity;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetLocation(decimal latitude, decimal longitude)
    {
        MeetingPointLatitude = latitude;
        MeetingPointLongitude = longitude;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() 
    { 
        Status = 1; 
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate() 
    { 
        Status = 0; 
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive => Status == 1;
}
