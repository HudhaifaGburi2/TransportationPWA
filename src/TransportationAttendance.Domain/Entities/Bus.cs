namespace TransportationAttendance.Domain.Entities;

public class Bus : BaseEntity
{
    // PlateNumber is the authoritative identifier from official CSV
    // Uniqueness enforced on (PlateNumber + PeriodId) - same plate can appear in different periods
    public string PlateNumber { get; private set; } = string.Empty;
    public int PeriodId { get; private set; }
    public Guid? RouteId { get; private set; }
    
    // Driver data imported from CSV and mapped to PlateNumber
    public string? DriverName { get; private set; }
    public string? DriverPhoneNumber { get; private set; }
    
    public int Capacity { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsMerged { get; private set; }
    public Guid? MergedWithBusId { get; private set; }

    // Navigation
    public Route? Route { get; private set; }
    public Bus? MergedWithBus { get; private set; }
    public ICollection<BusDistrict> BusDistricts { get; private set; } = new List<BusDistrict>();
    public ICollection<StudentBusAssignment> StudentAssignments { get; private set; } = new List<StudentBusAssignment>();

    private Bus() { }

    public static Bus Create(
        string plateNumber,
        int periodId,
        int capacity = 30,
        Guid? routeId = null,
        string? driverName = null,
        string? driverPhoneNumber = null)
    {
        return new Bus
        {
            PlateNumber = plateNumber,
            PeriodId = periodId,
            Capacity = capacity,
            RouteId = routeId,
            DriverName = driverName,
            DriverPhoneNumber = driverPhoneNumber,
            IsActive = true,
            IsMerged = false
        };
    }

    public void Update(
        string plateNumber,
        int periodId,
        int capacity,
        Guid? routeId,
        string? driverName,
        string? driverPhoneNumber)
    {
        PlateNumber = plateNumber;
        PeriodId = periodId;
        Capacity = capacity;
        RouteId = routeId;
        DriverName = driverName;
        DriverPhoneNumber = driverPhoneNumber;
    }

    public void AssignDriver(string driverName, string driverPhoneNumber)
    {
        DriverName = driverName;
        DriverPhoneNumber = driverPhoneNumber;
    }

    public void MergeWith(Guid targetBusId)
    {
        IsMerged = true;
        MergedWithBusId = targetBusId;
    }

    public void UnMerge()
    {
        IsMerged = false;
        MergedWithBusId = null;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
