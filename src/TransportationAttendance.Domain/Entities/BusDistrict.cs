namespace TransportationAttendance.Domain.Entities;

public class BusDistrict : BaseEntity
{
    public Guid BusId { get; private set; }
    public Guid DistrictId { get; private set; }

    // Navigation
    public Bus Bus { get; private set; } = null!;
    public District District { get; private set; } = null!;

    private BusDistrict() { }

    public static BusDistrict Create(Guid busId, Guid districtId)
    {
        return new BusDistrict
        {
            BusId = busId,
            DistrictId = districtId
        };
    }
}
