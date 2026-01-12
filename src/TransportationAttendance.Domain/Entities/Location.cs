namespace TransportationAttendance.Domain.Entities;

public class Location : BaseEntity
{
    public string LocationCode { get; private set; } = string.Empty;
    public string LocationName { get; private set; } = string.Empty;
    public string? LocationType { get; private set; }
    public bool IsActive { get; private set; }

    private Location() { }

    public static Location Create(string locationCode, string locationName, string? locationType = null)
    {
        return new Location
        {
            LocationCode = locationCode,
            LocationName = locationName,
            LocationType = locationType,
            IsActive = true
        };
    }

    public void Update(string locationCode, string locationName, string? locationType)
    {
        LocationCode = locationCode;
        LocationName = locationName;
        LocationType = locationType;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
