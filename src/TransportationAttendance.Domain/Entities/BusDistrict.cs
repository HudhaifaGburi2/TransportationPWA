namespace TransportationAttendance.Domain.Entities;

public class BusDistrict : BaseEntity
{
    public Guid BusId { get; private set; }
    public Guid DistrictId { get; private set; }

    // Navigation - District is optional to avoid query filter issues
    public Bus Bus { get; private set; } = null!;
    public District? District { get; private set; }

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
